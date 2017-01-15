using Libmpc;
using SmartHouse.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace SmartHouse.WebApiMono.Controllers
{
    [RoutePrefix("api/Remote")]
    public class RemoteController : BaseController
    {
        private readonly IYamahaService YamahaService;
        private readonly IPanodraService PandoraService;
        private readonly ISmartHouseService SmartHouseService;
        private readonly IMPDService MpdService;
        private readonly ITVService TVService;

        public RemoteController(ISettingsService service, IYamahaService yamahaService, IPanodraService pandoraService, ISmartHouseService smartHouseService, IMPDService mpdService, ITVService tvService) 
			: base(service)
		{
            YamahaService = yamahaService;
            PandoraService = pandoraService;
            SmartHouseService = smartHouseService;
            MpdService = mpdService;
            TVService = tvService;
        }

        [HttpGet]
        [Route("Up")]
        public async Task Up()
        {
            var smartHouseState = await SmartHouseService.GetCurrentState();

            if(smartHouseState == SmartHouseState.Pandora)
            {
                var result = PandoraService.NextStation();

                NotifyClients();
                PushNotification(result.Message);
            }
            else if(smartHouseState == SmartHouseState.TV)
            {
                await TVService.Up();
            }
        }

        [HttpGet]
        [Route("Down")]
        public async Task Down()
        {
            var smartHouseState = await SmartHouseService.GetCurrentState();

            if (smartHouseState == SmartHouseState.Pandora)
            {
                var result = PandoraService.PrevStation();

                NotifyClients();
                PushNotification(result.Message);
            }
            else if (smartHouseState == SmartHouseState.TV)
            {
                await TVService.Down();
            }
        }

        [HttpGet]
        [Route("Left")]
        public async Task<Result> Left()
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
                    await TVService.Left();
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
        [Route("Right")]
        public async Task<Result> Right()
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
                    await TVService.Right();
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
        [Route("Ok")]
        public async Task<Result> OkButton()
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
                    await TVService.Ok();
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
        [Route("Stop")]
        public async Task Stop()
        {
            var powerStatus = await YamahaService.PowerStatus();
            var smartHouseState = await SmartHouseService.GetCurrentState();

            if (powerStatus == PowerStatusEnum.StandBy)
                return;

            if (smartHouseState == SmartHouseState.Pandora)
            {
                await PandoraService.StopTcp();
                PushNotification("Pianobar has exited");
            }
            else if(smartHouseState == SmartHouseState.Music)
            {
                MpdService.Stop();
            }
            else if(smartHouseState == SmartHouseState.TV)
            {
                await TVService.Stop();
            } 
        }

        [HttpGet]
        [Route("Pause")]
        public async Task Pause()
        {
            var powerStatus = await YamahaService.PowerStatus();
            var smartHouseState = await SmartHouseService.GetCurrentState();

            if (powerStatus == PowerStatusEnum.StandBy)
                return;

            if (smartHouseState == SmartHouseState.Pandora)
            {
                PandoraService.Pause();
                PushNotification("Pianobar has exited");
            }
            else if (smartHouseState == SmartHouseState.Music)
            {
                MpdService.Pause();
            }
            else if (smartHouseState == SmartHouseState.TV)
            {
                await TVService.Pause();
            }            
        }

        [HttpGet]
        [Route("Love")]
        public async Task<Result> Love()
        {
            var smartHouseState = await SmartHouseService.GetCurrentState();
            var pandoraInfo = PandoraService.GetCurrentSongInfo();

            if (pandoraInfo.Loved)
                return new Result()
                {
                    Ok = true,
                    ErrorCode = 0,
                    Message = "You already liked this song"
                };

            if (smartHouseState == SmartHouseState.Pandora)
            {
                var result = PandoraService.ThumbUp();

                NotifyClients();
                PushNotification("Thumb Up");

                return new Result
                {
                    Ok = true,
                    ErrorCode = 0,
                    Message = $"You liked {pandoraInfo.Song} song"
                };
            }

            return new Result()
            {
                ErrorCode = 0,
                Message = "You can like only on Pandora input",
                Ok = true
            };
        }

        [HttpGet]
        [Route("Ban")]
        public async Task Ban()
        {
            var smartHouseState = await SmartHouseService.GetCurrentState();

            if (smartHouseState == SmartHouseState.Pandora)
            {
                var result = PandoraService.ThumbDown();

                NotifyClients();
                PushNotification("Thumb Down");
            }
        }
    }
}
