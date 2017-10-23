using Microsoft.AspNet.Identity;
using Microsoft.Azure.Devices;
using SmartHouseWebLib.DomainService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SmartHouseWeb.Controllers
{
    [Authorize]
    public class TelemetryController : Controller
    {
        private readonly ITelemetryDataService telemetryDataService;
        private readonly IRoomService roomService;

        public TelemetryController(ITelemetryDataService telemetryDataService, IRoomService roomService)
        {
            this.telemetryDataService = telemetryDataService;
            this.roomService = roomService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> TurnOn()
        {
            await SendMessageToDevice("TurnOn");

            if (Request.IsAjaxRequest())
                return new HttpStatusCodeResult(HttpStatusCode.Created, "Ok");
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> TurnOff()
        {
            await SendMessageToDevice("TurnOff");

            if (Request.IsAjaxRequest())
                return new HttpStatusCodeResult(HttpStatusCode.Created, "Ok");
            else
                return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Chart()
        {
            var userId = User.Identity.GetUserId();
            var data = await telemetryDataService.GetChartData(userId);

            return View(data);
        }

        [HttpGet]
        public async Task<ActionResult> GetLastHour()
        {
            var userId = User.Identity.GetUserId();

            var data = await telemetryDataService.GetChartLastHoutData(userId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        private async Task SendMessageToDevice(string message)
        {
            using (var serviceClient = ServiceClient.CreateFromConnectionString(Config.ConnectionStringShared))
            {
                var commandMessage = new Message(Encoding.UTF8.GetBytes(message));
                await serviceClient.SendAsync(Config.DeviceId, commandMessage);                
            }             
            
        }
    }
}