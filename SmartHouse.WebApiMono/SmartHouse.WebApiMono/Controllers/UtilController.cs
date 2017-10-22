using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
			var date = DateTime.Now;
			PushNotification(date.ToString());

			return date;
		}

		[HttpGet]
		[Route("DownloadLog")]
		public HttpResponseMessage DownloadLog()
		{
			try
			{
				var path = "smarthouse.log";
				var fileName = Path.GetFileName(path);

				var result = new HttpResponseMessage(HttpStatusCode.OK);
				var stream = new FileStream(path, FileMode.Open);

				result.Content = new StreamContent(stream);
				result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
				result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
				{
					FileName = fileName
				};

				return result;
			}
			catch (Exception ex)
			{
				var response = this.Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
				throw new HttpResponseException(response);
			}
		}

		[HttpGet]
		[Route("SignalRTest")]
		public bool SignalRTest(string param = null)
		{
			var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<ServerHub>();
			if (context == null)
				return false;

			context.Clients.All.hello(param ?? "Call from server");
			return true;
		}
	}
}
