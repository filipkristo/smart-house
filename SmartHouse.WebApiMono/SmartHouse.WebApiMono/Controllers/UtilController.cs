using System;
using System.Web.Http;

namespace SmartHouse.WebApiMono
{
	[RoutePrefix("api/Util")]
	public class UtilController : ApiController
	{
		public UtilController()
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
