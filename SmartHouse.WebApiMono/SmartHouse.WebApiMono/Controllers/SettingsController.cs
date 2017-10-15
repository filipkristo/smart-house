using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using SmartHouse.Lib;

namespace SmartHouse.WebApiMono
{
	public class SettingsController : BaseController
	{
		public SettingsController(ISettingsService service) : base(service)
		{
			
		}

		[HttpGet]
		[Route("GetSettings")]
		public async Task<ISettings> GetSettings()
		{
			return await SettingsService.GetSettings();
		}

		[HttpPost]
		[Route("SaveSettings")]
		public async Task<HttpResponseMessage> SaveSettings(Settings Settings)
		{
            await SettingsService.SaveSettings(Settings);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
	}
}
