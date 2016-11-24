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

		protected virtual void NotifyClients()
		{
			var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<ServerHub>();
			if (context == null)
				return;

			context.Clients.All.refreshAll();
		}
	}
}
