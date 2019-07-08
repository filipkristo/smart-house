using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using SmartHouse.Lib;

namespace SmartHouse.WebApiMono.Controllers
{
    [RoutePrefix("api/SmartBlub")]
    public class SmartBlubController : BaseController
    {
        private readonly ISmartBulbService _smartBulbService;

        public SmartBlubController(ISettingsService service, ISmartBulbService smartBulbService, IRabbitMqService rabbitMqService) : base(service, rabbitMqService)
        {
            _smartBulbService = smartBulbService;
        }

        [HttpPost]
        [Route("Toggle")]
        public async Task<Result> Toggle()
        {
            await _smartBulbService.Initialize();
            await _smartBulbService.Toggle();

            NotifyClients();
            PushNotification("Toggle lights");

            return new Result
            {
                Ok = true,
                Message = "Ok"
            };
        }

        [HttpPost]
        [Route("TurnOn")]
        public async Task<Result> TurnOn()
        {
            await _smartBulbService.Initialize();
            await _smartBulbService.TurnOn();

            NotifyClients();
            PushNotification("Turn on");

            return new Result
            {
                Ok = true,
                Message = "Ok"
            };
        }

        [HttpPost]
        [Route("TurnOff")]
        public async Task<Result> TurnOff()
        {
            await _smartBulbService.Initialize();
            await _smartBulbService.TurnOff();

            NotifyClients();
            PushNotification("Turn off");

            return new Result
            {
                Ok = true,
                Message = "Ok"
            };
        }

        [HttpPost]
        [Route("SetWhite")]
        public async Task<Result> SetWhite()
        {
            await _smartBulbService.Initialize();
            await _smartBulbService.SetWhite();

            PushNotification("Set white lights");

            return new Result
            {
                Ok = true,
                Message = "Ok"
            };
        }

        [HttpPost]
        [Route("SetRed")]
        public async Task<Result> SetRed()
        {
            await _smartBulbService.Initialize();
            await _smartBulbService.SetRed();

            PushNotification("Set red lights");

            return new Result
            {
                Ok = true,
                Message = "Ok"

            };
        }

        [HttpGet]
        [Route("IsTurnOn")]
        public async Task<bool> IsTurnOn()
        {
            await _smartBulbService.Initialize();
            return await _smartBulbService.IsTurnOn();
        }
    }
}
