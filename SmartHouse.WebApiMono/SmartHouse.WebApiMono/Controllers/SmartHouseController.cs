using System;
using System.Web.Http;
using SmartHouse.Lib;

namespace SmartHouse.WebApiMono
{
	[RoutePrefix("api/SmartHouse")]
	public class SmartHouseController : BaseController
	{
		public SmartHouseController(ISettingsService service) : base(service)
		{
			
		}


		public Result TurnOn()
		{
			return null;	
		}

		public Result TurnOff()
		{
			return null;
		}
	}
}
