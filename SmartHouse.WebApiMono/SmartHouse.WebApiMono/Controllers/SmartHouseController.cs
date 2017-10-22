using System;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Libmpc;
using SmartHouse.Lib;
using System.Collections.Generic;

namespace SmartHouse.WebApiMono
{
    [RoutePrefix("api/SmartHouse")]
    public class SmartHouseController : BaseController
    {
        private readonly IYamahaService YamahaService;
        private readonly IPanodraService PandoraService;
        private readonly ISmartHouseService SmartHouseService;
        private readonly IMPDService MpdService;
        private readonly ITVService TVService;
        private readonly ITelemetryService TelemetryService;

        public SmartHouseController(ISettingsService settingsService, IYamahaService yamahaService, IPanodraService pandoraService, ISmartHouseService smartHouseService, IMPDService mpdService, ITVService tvService, ITelemetryService telemetryService)
            : base(settingsService)
        {
            YamahaService = yamahaService;
            PandoraService = pandoraService;
            SmartHouseService = smartHouseService;
            MpdService = mpdService;
            TVService = tvService;
            TelemetryService = telemetryService;
        }

        [HttpGet]
        [Route("IsTurnOn")]
        public async Task<bool> IsTurnOn()
        {
            var powerStatus = await YamahaService.PowerStatus();
            return powerStatus == PowerStatusEnum.On;
        }

        [HttpGet]
        [Route("Power")]
        public async Task<Result> Power()
        {
            var powerStatus = await YamahaService.PowerStatus();

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
            var powerStatus = await YamahaService.PowerStatus();

            if (powerStatus == PowerStatusEnum.StandBy)
            {
                PushNotification("Please wait...");

                await TVService.Power();
                sb.AppendLine("Turning on TV");

                await YamahaService.TurnOn();
                sb.AppendLine("Yamaha Turn on");

                await Task.Delay(TimeSpan.FromSeconds(8));
            }

            if(!DefaultVolumeSetterState.IsDefaultVolumeSetted())
            {
                await SmartHouseService.SetMode(ModeEnum.Normal, VolumeChangeNotify);
                sb.AppendLine("Setting Normal mode");

                DefaultVolumeSetterState.DefaultVolumeSetted();
            }

            var state = await SmartHouseService.GetCurrentState();

            if (state == SmartHouseState.Music && MpdService.GetStatus().State != Libmpc.MpdState.Play)
            {
                if(PandoraService.IsPlaying())
                    PandoraService.Pause();

                await YamahaService.SetInput("HDMI1");
                sb.AppendLine("Setting HDMI1 input");

                MpdService.Play();
                sb.AppendLine("Playing MPD");
            }

            else if (state == SmartHouseState.Pandora)
            {
                await YamahaService.SetInput("HDMI1");
                sb.AppendLine("Setting HDMI1 input");

                if(!PandoraService.IsPlaying())
                {
                    await PandoraService.StartTcp();
                    PandoraService.Play();
                    sb.AppendLine("Playing pandora radio");
                }
            }
            else if (state == SmartHouseState.TV)
            {
                await Task.Delay(TimeSpan.FromSeconds(2));

                await TVService.Home();
                sb.AppendLine("TV Home IR button");

                await YamahaService.SetInput("AUDIO1");
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
            var powerStatus = await YamahaService.PowerStatus();

            if (PandoraService.IsPlaying())
            {
                PandoraService.Pause();
                sb.AppendLine("Pausing pandora radio");
            }

            if (MpdService.GetStatus().State == Libmpc.MpdState.Play)
            {
                MpdService.Stop();
                sb.AppendLine("Stopping MPD");
            }

            if (powerStatus == PowerStatusEnum.On)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                await YamahaService.TurnOff();
                sb.AppendLine("Yamaha Turn Off");

                await TVService.Power();
            }

            if (await TelemetryService.GetAirConditionState() == 1)
                await TelemetryService.AirCondition(0);

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
            var powerStatus = await YamahaService.PowerStatus();

            if (powerStatus == PowerStatusEnum.On)
            {
                var newVolume = await YamahaService.VolumeUp();
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
            var powerStatus = await YamahaService.PowerStatus();

            if (powerStatus == PowerStatusEnum.On)
            {
                var newVolume = await YamahaService.VolumeDown();
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
            var powerStatus = await YamahaService.PowerStatus();

            if (powerStatus == PowerStatusEnum.On)
            {
                var mpdState = MpdService.GetStatus().State;
                var smartHouseState = await SmartHouseService.GetCurrentState();

                if (smartHouseState == SmartHouseState.Music && (mpdState == MpdState.Play || mpdState == MpdState.Pause))
                {
                    MpdService.Next();
                    sb.AppendLine("MPD Next song");
                }
                else if (smartHouseState == SmartHouseState.Pandora)
                {
                    PandoraService.Next();
                    sb.AppendLine("Pandora next song");
                }
                else if (smartHouseState == SmartHouseState.TV)
                {
                    await TVService.Forward();
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
            var powerStatus = await YamahaService.PowerStatus();

            if (powerStatus == PowerStatusEnum.On)
            {
                var state = await SmartHouseService.GetCurrentState();

                if (state == SmartHouseState.Pandora)
                {
                    PandoraService.Play();
                    sb.AppendLine("Starting to play/pause Pandora");
                }
                else if (state == SmartHouseState.Music)
                {
                    if (MpdService.GetStatus().State == MpdState.Play)
                        MpdService.Pause();
                    else if (MpdService.GetStatus().State == MpdState.Pause)
                        MpdService.Play();
                }
                else if (state == SmartHouseState.TV)
                {
                    await TVService.Play();
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
            var powerStatus = await YamahaService.PowerStatus();

            if (powerStatus == PowerStatusEnum.On)
            {
                var mpdState = MpdService.GetStatus().State;
                var state = await SmartHouseService.GetCurrentState();

                if (state == SmartHouseState.Music && (mpdState == MpdState.Play || mpdState == MpdState.Pause))
                {
                    MpdService.Previous();
                    sb.AppendLine("MPD Previous song");
                }
                else if (state == SmartHouseState.Pandora)
                {
                    PandoraService.Next();
                    sb.AppendLine("Pandora next song. Pandora can't go previous");
                }
                else if (state == SmartHouseState.TV)
                {
                    await TVService.Rewind();
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

            var result = await SmartHouseService.SetMode(modeEnum, VolumeChangeNotify);
            return result;
        }

        [HttpGet]
        [Route("Xbox")]
        public async Task<Result> Xbox()
        {
            var sb = new StringBuilder();
            var powerStatus = await YamahaService.PowerStatus();

            if (powerStatus == PowerStatusEnum.StandBy)
            {
                await YamahaService.TurnOn();
                sb.AppendLine("Yamaha Turn on");
                await Task.Delay(TimeSpan.FromSeconds(8));
            }

            if (MpdService.GetStatus().State == Libmpc.MpdState.Play)
            {
                MpdService.Stop();
                sb.AppendLine("Stopping MPD");
            }

            if (PandoraService.IsPlaying())
            {
                PandoraService.Pause();
                sb.AppendLine("Pausing pandora radio");
            }

            await YamahaService.SetInput("HDMI2");
            sb.AppendLine("Set HDMI2 input");

            await SmartHouseService.SaveState(SmartHouseState.XBox);

            await TVService.Source();
            await Task.Delay(2000);
            await TVService.Ok();
            await TVService.Ok();

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
            var powerStatus = await YamahaService.PowerStatus();

            if (powerStatus == PowerStatusEnum.StandBy)
            {
                await YamahaService.TurnOn();
                sb.AppendLine("Yamaha Turn on");
                await Task.Delay(TimeSpan.FromSeconds(8));
            }

            if (MpdService.GetStatus().State == Libmpc.MpdState.Play || MpdService.GetStatus().State == MpdState.Pause)
            {
                MpdService.Stop();
                sb.AppendLine("Stopping MPD");
            }

            if (!PandoraService.IsPlaying())
            {
                PandoraService.StartTcp().Wait(1000);

                PandoraService.Play();
                sb.AppendLine("Playing pandora radio");
            }

            await YamahaService.SetInput("HDMI1");
            sb.AppendLine("Set HDMI1 input");

            await SmartHouseService.SaveState(SmartHouseState.Pandora);

            await TVService.Source();
            await Task.Delay(2000);
            await TVService.Ok();
            await TVService.Ok();

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
            var powerStatus = await YamahaService.PowerStatus();

            if (powerStatus == PowerStatusEnum.StandBy)
            {
                await YamahaService.TurnOn();
                sb.AppendLine("Yamaha Turn on");
                await Task.Delay(TimeSpan.FromSeconds(8));
            }

            if(PandoraService.IsPlaying())
            {
                PandoraService.Pause();
                sb.AppendLine("Stopping pandora radio");
            }

            await YamahaService.SetInput("HDMI1");
            sb.AppendLine("Set HDMI1 input");

            MpdService.Play();

            await SmartHouseService.SaveState(SmartHouseState.Music);

            await TVService.Source();
            await Task.Delay(2000);
            await TVService.Ok();
            await TVService.Ok();

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
            var powerStatus = await YamahaService.PowerStatus();

            if (powerStatus == PowerStatusEnum.StandBy)
            {
                await YamahaService.TurnOn();
                sb.AppendLine("Yamaha Turn on");
                await Task.Delay(TimeSpan.FromSeconds(8));
            }

            if (MpdService.GetStatus().State == Libmpc.MpdState.Play)
            {
                MpdService.Stop();
                sb.AppendLine("Stopping MPD");
            }

            if (PandoraService.IsPlaying())
            {
                PandoraService.Pause();
                sb.AppendLine("Pause pandora radio");
            }

            await YamahaService.SetInput("AUDIO1");
            sb.AppendLine("Setting AUDIO1 input");

            await SmartHouseService.SaveState(SmartHouseState.TV);

            await TVService.Home();

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
            await TVService.SendCommand(commandEnum);
        }

        [HttpGet]
        [Route("GetCurrentState")]
        public async Task<string> GetCurrentState()
        {
            var state = await SmartHouseService.GetCurrentState();
            return state.ToString();
        }

        [HttpGet]
        [Route("RestartOpenVPN")]
        public async Task<Result> RestartOpenVPN()
        {
            var result = await SmartHouseService.RestartOpenVPNServiceTcp();
            return result;
        }

        [HttpGet]
        [Route("PlayAlarm")]
        public async Task<Result> PlayAlarm()
        {
            var result = await SmartHouseService.PlayAlarmTcp();
            return result;
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
            var result = await SmartHouseService.TimerTcp();
            return result;
        }

        [HttpGet]
        [Route("NowPlaying")]
        public async Task<SongResult> NowPlaying(bool lastFm = true)
        {
            var state = await SmartHouseService.GetCurrentState();

            if (state == SmartHouseState.Pandora)
                return await PandoraService.GetNowPlaying();
            else if (state == SmartHouseState.Music)
                return await MpdService.GetNowPlaying(lastFm);
            else
                return null;
        }

        [HttpPost]
        [Route("PhoneCallStarted")]
        public async Task<bool> PhoneCallStarted(PhoneCallData phoneCall)
        {
            PushNotification("Phone call started");

            var powerStatus = await YamahaService.PowerStatus();
            var isTurnOn = powerStatus == PowerStatusEnum.On;

            var state = await SmartHouseService.GetCurrentState();
            var isPlaying = false;

            if (state == SmartHouseState.Pandora && isTurnOn)
                isPlaying = PandoraService.IsPlaying();
            else if (state == SmartHouseState.Music && isTurnOn)
                isPlaying = MpdService.GetStatus().State == MpdState.Play;

            if(isPlaying)
            {
                if (state == SmartHouseState.Pandora)
                {
                    if(!PhoneCallsStack.PhoneCallActive())
                        PandoraService.Pause();

                    PhoneCallsStack.AddPhoneCall(phoneCall);
                }
                else if (state == SmartHouseState.Music)
                {
                    if(!PhoneCallsStack.PhoneCallActive())
                        MpdService.Pause();

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

            var powerStatus = await YamahaService.PowerStatus();
            var isTurnOn = powerStatus == PowerStatusEnum.On;
            var state = await SmartHouseService.GetCurrentState();

            if(shouldStartWithMusic && state == SmartHouseState.Pandora && isTurnOn)
            {
                PandoraService.Play();
            }
            else if(shouldStartWithMusic && state == SmartHouseState.Music && isTurnOn && MpdService.GetStatus().State == MpdState.Pause)
            {
                MpdService.Play();
            }

            return shouldStartWithMusic;
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


        }
    }
}
