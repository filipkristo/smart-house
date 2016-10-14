using System;
using System.Web.Http;
using SmartHouse.Lib;

namespace SmartHouse.WebApiMono
{
	public class BaseController : ApiController
	{
		protected readonly ISettingsService SettingsService;

		public BaseController(ISettingsService service)
		{
			this.SettingsService = service;
		}
	}
}
