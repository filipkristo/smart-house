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

		protected virtual void PushNotification(string message)
		{
			var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<ServerHub>();
			if (context == null)
				return;

			context.Clients.All.notification(message);
		}

        protected virtual void VolumeChangeNotify(short currentVolume)
        {
            var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<ServerHub>();
            if (context == null)
                return;

            context.Clients.All.volumeChange(currentVolume);
        }
    }
}
