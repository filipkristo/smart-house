using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using SmartHouse.Lib;

namespace SmartHouse.WebApiMono.Controllers
{
    [RoutePrefix("api/Orvibio")]
    public class OrvibioController : BaseController
    {
        private readonly IOrvibioService OrvibioService;

        public OrvibioController(ISettingsService settingsService, IOrvibioService orvibioService) : base(settingsService)
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
    }
}
