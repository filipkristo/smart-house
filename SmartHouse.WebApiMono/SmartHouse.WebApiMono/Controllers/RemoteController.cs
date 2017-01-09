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
        public async Task Left()
        {
            await TVService.Left();
        }

        [HttpGet]
        [Route("Right")]
        public async Task Right()
        {
            await TVService.Left();
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

            NotifyClients();
        }

        [HttpGet]
        [Route("Love")]
        public async Task Love()
        {
            var smartHouseState = await SmartHouseService.GetCurrentState();

            if (smartHouseState == SmartHouseState.Pandora)
            {
                var result = PandoraService.ThumbUp();

                NotifyClients();
                PushNotification("Thumb Up");
            }
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
