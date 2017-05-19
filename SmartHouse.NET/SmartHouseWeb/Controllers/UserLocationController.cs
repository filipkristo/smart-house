using SmartHouseWebLib.DomainService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SmartHouseWeb.Controllers
{
    public class UserLocationController : Controller
    {
        private readonly IUserLocationService userLocationService;

        public UserLocationController(IUserLocationService userLocationService)
        {
            this.userLocationService = userLocationService;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var model = await userLocationService.GetAllDescAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            var model = await userLocationService.GetAsync(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            var model = await userLocationService.GetAsync(id);
            await userLocationService.Delete(model);
            return RedirectToAction("Index");
        }
    }
}
