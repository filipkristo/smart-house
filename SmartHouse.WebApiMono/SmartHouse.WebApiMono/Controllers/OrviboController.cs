using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using SmartHouse.Lib;
using OrviboController.Common;

namespace SmartHouse.WebApiMono.Controllers
{
    [RoutePrefix("api/Orvibo")]
    public class OrviboController : BaseController
    {
        private readonly IOrviboService OrvibioService;

        public OrviboController(ISettingsService settingsService, IOrviboService orvibioService) : base(settingsService)
        {
            OrvibioService = orvibioService;
        }

        [Route("TurnOn")]
        [HttpGet]
        public Result TurnOn()
        {
            return OrvibioService.TurnOn();
        }

        [Route("TurnOff")]
        [HttpGet]
        public Result TurnOff()
        {
            return OrvibioService.TurnOff();
        }

        [Route("GetDevice")]
        [HttpGet]
        public Device GetDevice()
        {
            return OrvibioService.GetDevice();
        }
    }
}
