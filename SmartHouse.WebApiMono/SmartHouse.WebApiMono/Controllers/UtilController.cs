using System;
using System.Web.Http;
using SmartHouse.Lib;

namespace SmartHouse.WebApiMono
{
	[RoutePrefix("api/Util")]
	public class UtilController : BaseController
	{
		public UtilController(ISettingsService service) : base(service)
		{

		}

		[HttpGet]
		[Route("Test")]
		public string GetTest()
		{
			return $"test {DateTime.Now}";
		}

		[HttpGet]
		[Route("ServerDate")]
		public DateTime GetServerDate()
		{
			return DateTime.Now;
		}


	}
}
