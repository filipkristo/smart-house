using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartHouseWeb.Controllers
{
    [Authorize]
    public class TelemetryController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Chart()
        {
            return View();
        }
    }
}