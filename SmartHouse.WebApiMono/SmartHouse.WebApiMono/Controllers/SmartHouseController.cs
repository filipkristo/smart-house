using System;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Libmpc;
using SmartHouse.Lib;
using System.Collections.Generic;
using System.IO;

namespace SmartHouse.WebApiMono
{
    [RoutePrefix("api/SmartHouse")]
    public class SmartHouseController : BaseController
    {
        private readonly IYamahaService _yamahaService;
        private readonly IPanodraService _pandoraService;
        private readonly ISmartHouseService _smartHouseService;
        private readonly IMPDService _mpdService;
        private readonly ITVService _tvService;
        private readonly ITelemetryService _telemetryService;
        private readonly ISmartBulbService _smartBulbService;
        private readonly ISunriseSunsetService _sunriseSunsetService;

        public SmartHouseController(ISettingsService settingsService, IYamahaService yamahaService, IPanodraService pandoraService, ISmartHouseService smartHouseService, IMPDService mpdService, ITVService tvService, ITelemetryService telemetryService, ISmartBulbService smartBulbService, ISunriseSunsetService sunriseSunsetService)
            : base(settingsService)
        {
            _yamahaService = yamahaService;
            _pandoraService = pandoraService;
            _smartHouseService = smartHouseService;
            _mpdService = mpdService;
            _tvService = tvService;
            _telemetryService = telemetryService;
            _smartBulbService = smartBulbService;
            _sunriseSunsetService = sunriseSunsetService;
        }

        [HttpGet]
        [Route("Dashboard")]
        public async Task<DashboardData> Dashboard()
        {
            var nowPlaying = await NowPlaying();
            var currentInput = await _smartHouseService.GetCurrentState();
            var telemetryData = await _telemetryService.GetLastTemperature();
            var isTurnOn = (await _yamahaService.PowerStatus()) == PowerStatusEnum.On;
            var volume = await _yamahaService.GetVolume();
            var isLightsTurnOn = await _smartBulbService.IsTurnOn();
            var isAirConditionerTurnOn = (await _telemetryService.GetAirConditionState()) == 1;

            return new DashboardData
            {
                NowPlaying = nowPlaying,
                CurrentInput = currentInput.ToString(),
                TelemetryData = telemetryData,
                IsTurnOn = isTurnOn,
                Volume = volume,
                IsAirConditionerTurnOn = isAirConditionerTurnOn,
                IsLightsTurnOn = isLightsTurnOn
            };
        }

        [HttpGet]
        [Route("IsTurnOn")]
        public async Task<bool> IsTurnOn()
        {
            var powerStatus = await _yamahaService.PowerStatus();
            return powerStatus == PowerStatusEnum.On;
        }

        [HttpGet]
        [Route("Power")]
        public async Task<Result> Power()
        {
            var powerStatus = await _yamahaService.PowerStatus();

            if (powerStatus == PowerStatusEnum.On)
                return await TurnOff();
            else
                return await TurnOn();
        }

        [HttpGet]
        [Route("TurnOn")]
        public async Task<Result> TurnOn()
        {
            var sb = new StringBuilder();
            var powerStatus = await _yamahaService.PowerStatus();

            if (powerStatus == PowerStatusEnum.StandBy)
            {
                PushNotification("Please wait...");

                await _tvService.Power();
                sb.AppendLine("Turning on TV");

                await _yamahaService.TurnOn();
                sb.AppendLine("Yamaha Turn on");
                
                if(await _sunriseSunsetService.IsNight())
                {
                    await _smartBulbService.Initialize();
                    await _smartBulbService.PowerOn();
                    sb.AppendLine("Smart blub turn on");
                }

                await Task.Delay(TimeSpan.FromSeconds(8));
            }

            if(!DefaultVolumeSetterState.IsDefaultVolumeSetted())
            {
                await _smartHouseService.SetMode(ModeEnum.Normal, VolumeChangeNotify);
                sb.AppendLine("Setting Normal mode");

                DefaultVolumeSetterState.DefaultVolumeSetted();
            }

            var state = await _smartHouseService.GetCurrentState();

            if (state == SmartHouseState.Music && _mpdService.GetStatus().State != Libmpc.MpdState.Play)
            {
                if(_pandoraService.IsPlaying())
                    _pandoraService.Pause();

                await _yamahaService.SetInput("HDMI1");
                sb.AppendLine("Setting HDMI1 input");

                _mpdService.Play();
                sb.AppendLine("Playing MPD");
            }

            else if (state == SmartHouseState.Pandora)
            {
                await _yamahaService.SetInput("HDMI1");
                sb.AppendLine("Setting HDMI1 input");

                if(!_pandoraService.IsPlaying())
                {
                    await _pandoraService.StartTcp();
                    _pandoraService.Play();
                    sb.AppendLine("Playing pandora radio");
                }
            }
            else if (state == SmartHouseState.TV)
            {
                await Task.Delay(TimeSpan.FromSeconds(2));

                await _tvService.Home();
                sb.AppendLine("TV Home IR button");

                await _yamahaService.SetInput("AUDIO1");
                sb.AppendLine("Setting AUDIO1 input");
            }

            NotifyClients();
            PushNotification("Smart house is turn on");

            return new Result()
            {
                ErrorCode = 0,
                Message = sb.ToString(),
                Ok = true
            };
        }

        [HttpGet]
        [Route("TurnOff")]
        public async Task<Result> TurnOff()
        {
            var sb = new StringBuilder();
            var powerStatus = await _yamahaService.PowerStatus();

            if (_pandoraService.IsPlaying())
            {
                _pandoraService.Pause();
                sb.AppendLine("Pausing pandora radio");
            }

            if (_mpdService.GetStatus().State == Libmpc.MpdState.Play)
            {
                _mpdService.Stop();
                sb.AppendLine("Stopping MPD");
            }

            if (powerStatus == PowerStatusEnum.On)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                await _yamahaService.TurnOff();
                sb.AppendLine("Yamaha Turn Off");

                await _tvService.Power();
            }

            if (await _telemetryService.GetAirConditionState() == 1)
                await _telemetryService.AirCondition(0);

            if (await _sunriseSunsetService.IsNight())
            {
                await _smartBulbService.Initialize();
                await _smartBulbService.PowerOff();
            }

            NotifyClients();
            PushNotification("Smart house is turn off");

            return new Result()
            {
                ErrorCode = 0,
                Message = sb.ToString(),
                Ok = true
            };
        }

        [HttpGet]
        [Route("VolumeUp")]
        public async Task<Result> VolumeUp()
        {
            var sb = new StringBuilder();
            var powerStatus = await _yamahaService.PowerStatus();

            if (powerStatus == PowerStatusEnum.On)
            {
                var newVolume = await _yamahaService.VolumeUp();
                sb.AppendLine("Yamaha Volume Up.");

                VolumeChangeNotify(newVolume);
                sb.Append($"{newVolume} db");
            }
            else
            {
                sb.AppendLine("Yamaha is turned off");
                PushNotification("Yamaha is turned off. Operation canceled.");
            }

            return new Result()
            {
                ErrorCode = 0,
                Message = sb.ToString(),
                Ok = true
            };
        }

        [HttpGet]
        [Route("VolumeDown")]
        public async Task<Result> VolumeDown()
        {
            var sb = new StringBuilder();
            var powerStatus = await _yamahaService.PowerStatus();

            if (powerStatus == PowerStatusEnum.On)
            {
                var newVolume = await _yamahaService.VolumeDown();
                sb.AppendLine("Yamaha Volume Down.");

                VolumeChangeNotify(newVolume);
                sb.Append($"{newVolume} db");
            }
            else
            {
                sb.AppendLine("Yamaha is turned off.");
                PushNotification("Yamaha is turned off. Operation canceled");
            }

            return new Result()
            {
                ErrorCode = 0,
                Message = sb.ToString(),
                Ok = true
            };
        }

        [HttpGet]
        [Route("Next")]
        public async Task<Result> Next()
        {
            var sb = new StringBuilder();
            var powerStatus = await _yamahaService.PowerStatus();

            if (powerStatus == PowerStatusEnum.On)
            {
                var mpdState = _mpdService.GetStatus().State;
                var smartHouseState = await _smartHouseService.GetCurrentState();

                if (smartHouseState == SmartHouseState.Music && (mpdState == MpdState.Play || mpdState == MpdState.Pause))
                {
                    _mpdService.Next();
                    sb.AppendLine("MPD Next song");
                }
                else if (smartHouseState == SmartHouseState.Pandora)
                {
                    _pandoraService.Next();
                    sb.AppendLine("Pandora next song");
                }
                else if (smartHouseState == SmartHouseState.TV)
                {
                    await _tvService.Forward();
                    sb.AppendLine("TV forward");
                }
            }
            else
            {
                sb.AppendLine("Yamaha is turned off. Operation canceled");
                PushNotification("Yamaha is turned off. Operation canceled");
            }

            return new Result()
            {
                ErrorCode = 0,
                Message = sb.ToString(),
                Ok = true
            };
        }

        [HttpGet]
        [Route("Play")]
        public async Task<Result> Play()
        {
            var sb = new StringBuilder();
            var powerStatus = await _yamahaService.PowerStatus();

            if (powerStatus == PowerStatusEnum.On)
            {
                var state = await _smartHouseService.GetCurrentState();

                if (state == SmartHouseState.Pandora)
                {
                    _pandoraService.Play();
                    sb.AppendLine("Starting to play/pause Pandora");
                }
                else if (state == SmartHouseState.Music)
                {
                    if (_mpdService.GetStatus().State == MpdState.Play)
                        _mpdService.Pause();
                    else if (_mpdService.GetStatus().State == MpdState.Pause)
                        _mpdService.Play();
                }
                else if (state == SmartHouseState.TV)
                {
                    await _tvService.Play();
                }
            }
            else
            {
                sb.AppendLine("Yamaha is turned off. Operation canceled");
                PushNotification("Yamaha is turned off. Operation canceled");
            }

            return new Result()
            {
                ErrorCode = 0,
                Message = sb.ToString(),
                Ok = true
            };
        }


        [HttpGet]
        [Route("Prev")]
        public async Task<Result> Prev()
        {
            var sb = new StringBuilder();
            var powerStatus = await _yamahaService.PowerStatus();

            if (powerStatus == PowerStatusEnum.On)
            {
                var mpdState = _mpdService.GetStatus().State;
                var state = await _smartHouseService.GetCurrentState();

                if (state == SmartHouseState.Music && (mpdState == MpdState.Play || mpdState == MpdState.Pause))
                {
                    _mpdService.Previous();
                    sb.AppendLine("MPD Previous song");
                }
                else if (state == SmartHouseState.Pandora)
                {
                    _pandoraService.Next();
                    sb.AppendLine("Pandora next song. Pandora can't go previous");
                }
                else if (state == SmartHouseState.TV)
                {
                    await _tvService.Rewind();
                    sb.AppendLine("TV Rewind");
                }
            }
            else
            {
                sb.AppendLine("Yamaha is turned off. Operation canceled");
                PushNotification("Yamaha is turned off. Operation canceled");
            }

            return new Result()
            {
                ErrorCode = 0,
                Message = sb.ToString(),
                Ok = true
            };
        }

        [HttpGet]
        [Route("SetMode")]
        public async Task<Result> SetMode(string mode)
        {
            var modeEnum = (ModeEnum)Enum.Parse(typeof(ModeEnum), mode);

            var result = await _smartHouseService.SetMode(modeEnum, VolumeChangeNotify);
            return result;
        }

        [HttpGet]
        [Route("Xbox")]
        public async Task<Result> Xbox()
        {
            var sb = new StringBuilder();
            var powerStatus = await _yamahaService.PowerStatus();

            if (powerStatus == PowerStatusEnum.StandBy)
            {
                await _yamahaService.TurnOn();
                sb.AppendLine("Yamaha Turn on");
                await Task.Delay(TimeSpan.FromSeconds(8));
            }

            if (_mpdService.GetStatus().State == Libmpc.MpdState.Play)
            {
                _mpdService.Stop();
                sb.AppendLine("Stopping MPD");
            }

            if (_pandoraService.IsPlaying())
            {
                _pandoraService.Pause();
                sb.AppendLine("Pausing pandora radio");
            }

            await _yamahaService.SetInput("HDMI2");
            sb.AppendLine("Set HDMI2 input");

            await _smartHouseService.SaveState(SmartHouseState.XBox);

            await _tvService.Source();
            await Task.Delay(2000);
            await _tvService.Ok();
            await _tvService.Ok();

            NotifyClients();
            PushNotification("XBox");

            return new Result()
            {
                ErrorCode = 0,
                Message = sb.ToString(),
                Ok = true
            };
        }

        [HttpGet]
        [Route("Pandora")]
        public async Task<Result> Pandora()
        {
            var sb = new StringBuilder();
            var powerStatus = await _yamahaService.PowerStatus();

            if (powerStatus == PowerStatusEnum.StandBy)
            {
                await _yamahaService.TurnOn();
                sb.AppendLine("Yamaha Turn on");
                await Task.Delay(TimeSpan.FromSeconds(8));
            }

            if (_mpdService.GetStatus().State == Libmpc.MpdState.Play || _mpdService.GetStatus().State == MpdState.Pause)
            {
                _mpdService.Stop();
                sb.AppendLine("Stopping MPD");
            }

            if (!_pandoraService.IsPlaying())
            {
                _pandoraService.StartTcp().Wait(1000);

                _pandoraService.Play();
                sb.AppendLine("Playing pandora radio");
            }

            await _yamahaService.SetInput("HDMI1");
            sb.AppendLine("Set HDMI1 input");

            await _smartHouseService.SaveState(SmartHouseState.Pandora);

            await _tvService.Source();
            await Task.Delay(2000);
            await _tvService.Ok();
            await _tvService.Ok();

            if(await _sunriseSunsetService.IsNight())
            {
                await _smartBulbService.Initialize();
                await _smartBulbService.SetWhite();
            }

            NotifyClients();
            PushNotification("Pandora");

            return new Result()
            {
                ErrorCode = 0,
                Message = sb.ToString(),
                Ok = true
            };
        }

        [HttpGet]
        [Route("Music")]
        public async Task<Result> Music()
        {
            var sb = new StringBuilder();
            var powerStatus = await _yamahaService.PowerStatus();

            if (powerStatus == PowerStatusEnum.StandBy)
            {
                await _yamahaService.TurnOn();
                sb.AppendLine("Yamaha Turn on");
                await Task.Delay(TimeSpan.FromSeconds(8));
            }

            if(_pandoraService.IsPlaying())
            {
                _pandoraService.Pause();
                sb.AppendLine("Stopping pandora radio");
            }

            await _yamahaService.SetInput("HDMI1");
            sb.AppendLine("Set HDMI1 input");

            _mpdService.Play();

            await _smartHouseService.SaveState(SmartHouseState.Music);

            await _tvService.Source();
            await Task.Delay(2000);
            await _tvService.Ok();
            await _tvService.Ok();

            NotifyClients();
            PushNotification("Music");

            return new Result()
            {
                ErrorCode = 0,
                Message = sb.ToString(),
                Ok = true
            };
        }

        [HttpGet]
        [Route("TV")]
        public async Task<Result> TV()
        {
            var sb = new StringBuilder();
            var powerStatus = await _yamahaService.PowerStatus();

            if (powerStatus == PowerStatusEnum.StandBy)
            {
                await _yamahaService.TurnOn();
                sb.AppendLine("Yamaha Turn on");
                await Task.Delay(TimeSpan.FromSeconds(8));
            }

            if (_mpdService.GetStatus().State == Libmpc.MpdState.Play)
            {
                _mpdService.Stop();
                sb.AppendLine("Stopping MPD");
            }

            if (_pandoraService.IsPlaying())
            {
                _pandoraService.Pause();
                sb.AppendLine("Pause pandora radio");
            }

            await _yamahaService.SetInput("AUDIO1");
            sb.AppendLine("Setting AUDIO1 input");

            await _smartHouseService.SaveState(SmartHouseState.TV);

            await _tvService.Home();

            if (await _sunriseSunsetService.IsNight())
            {
                await _smartBulbService.Initialize();
                await _smartBulbService.SetRed();
            }

            NotifyClients();
            PushNotification("TV");

            return new Result()
            {
                ErrorCode = 0,
                Message = sb.ToString(),
                Ok = true
            };
        }

        [HttpGet]
        [Route("TVCommand")]
        public async Task TVCommand(string c)
        {
            if (string.IsNullOrWhiteSpace(c))
                return;

            var commandEnum = (IRCommands)Enum.Parse(typeof(IRCommands), c.Trim(), true);
            await _tvService.SendCommand(commandEnum);
        }

        [HttpGet]
        [Route("GetCurrentState")]
        public async Task<string> GetCurrentState()
        {
            var state = await _smartHouseService.GetCurrentState();
            return state.ToString();
        }

        [HttpGet]
        [Route("RestartOpenVPN")]
        public async Task<Result> RestartOpenVPN()
        {
            return await _smartHouseService.RestartOpenVPNServiceTcp();
        }

        [HttpGet]
        [Route("PlayAlarm")]
        public async Task<Result> PlayAlarm()
        {
            return await _smartHouseService.PlayAlarmTcp();
        }

        [HttpGet]
        [Route("Notify")]
        public void Notify()
        {
            NotifyClients();
        }

        [HttpGet]
        [Route("PushNotification")]
        public void SendPushNotification(string message)
        {
            PushNotification(message);
        }

        [HttpGet]
        [Route("TurnOfTimer")]
        public async Task<Result> TurnOfTimer(int minutes)
        {
            Timer.TimeoutMinutes = minutes;
            return await _smartHouseService.TimerTcp();
        }

        [HttpGet]
        [Route("NowPlaying")]
        public async Task<PandoraResult> NowPlaying(bool lastFm = true)
        {
            var state = await _smartHouseService.GetCurrentState();

            switch (state)
            {
                case SmartHouseState.Pandora:
                    return await _pandoraService.GetNowPlaying();
                case SmartHouseState.Music:
                    return await _mpdService.GetNowPlaying(lastFm);
                default:
                    return null;
            }
        }

        [HttpPost]
        [Route("PhoneCallStarted")]
        public async Task<bool> PhoneCallStarted(PhoneCallData phoneCall)
        {
            PushNotification("Phone call started");

            var powerStatus = await _yamahaService.PowerStatus();
            var isTurnOn = powerStatus == PowerStatusEnum.On;

            var state = await _smartHouseService.GetCurrentState();
            var isPlaying = false;

            if (state == SmartHouseState.Pandora && isTurnOn)
                isPlaying = _pandoraService.IsPlaying();
            else if (state == SmartHouseState.Music && isTurnOn)
                isPlaying = _mpdService.GetStatus().State == MpdState.Play;

            if(isPlaying)
            {
                if (state == SmartHouseState.Pandora)
                {
                    if(!PhoneCallsStack.PhoneCallActive())
                        _pandoraService.Pause();

                    PhoneCallsStack.AddPhoneCall(phoneCall);
                }
                else if (state == SmartHouseState.Music)
                {
                    if(!PhoneCallsStack.PhoneCallActive())
                        _mpdService.Pause();

                    PhoneCallsStack.AddPhoneCall(phoneCall);
                }
            }

            return isPlaying;
        }

        [HttpPost]
        [Route("PhoneCallEnded")]
        public async Task<bool> PhoneCallEnded()
        {
            PushNotification("Phone call ended");

            var shouldStartWithMusic = PhoneCallsStack.ShouldStartWithMusic();
            if (!shouldStartWithMusic)
                return false;

            var powerStatus = await _yamahaService.PowerStatus();
            var isTurnOn = powerStatus == PowerStatusEnum.On;
            var state = await _smartHouseService.GetCurrentState();

            if(state == SmartHouseState.Pandora && isTurnOn)
            {
                _pandoraService.Play();
            }
            else if(state == SmartHouseState.Music && isTurnOn && _mpdService.GetStatus().State == MpdState.Pause)
            {
                _mpdService.Play();
            }

            return true;
        }

        [HttpGet]
        [Route("PhoneCalls")]
        public IEnumerable<PhoneCallData> AllPhoneCalls()
        {
            return PhoneCallsStack.AllPhoneCalls();
        }

        [HttpPost]
        [Route("UploadContent")]
        public void UploadContent(ContentUploadModel model)
        {
            if (model?.ContentCategoryEnum != ContentCategoryEnum.Image)
                throw new Exception("Only image files are supported");

            var contentBytes = Convert.FromBase64String(model.ContentBase64);
            File.WriteAllBytes("lastPicture.jpg", contentBytes);
        }


    }
}
