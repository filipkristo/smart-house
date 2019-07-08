using System;
using System.Web.Http;
using SmartHouse.Lib;

namespace SmartHouse.WebApiMono
{
    public class BaseController : ApiController
    {
        protected readonly ISettingsService SettingsService;
        protected readonly IRabbitMqService RabbitMqService;

        public BaseController(ISettingsService service, IRabbitMqService rabbitMqService)
        {
            this.SettingsService = service;
            this.RabbitMqService = rabbitMqService;
        }

        protected virtual void NotifyClients()
        {
            RabbitMqService.SendDashboardData(new DashboardData());

            var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<ServerHub>();
            if (context == null)
                return;

            context.Clients.All.refreshAll();
        }

        protected virtual void PushNotification(string message)
        {
            RabbitMqService.SendNotification(message);

            var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<ServerHub>();
            if (context == null)
                return;

            context.Clients.All.notification(message);
        }

        protected virtual void VolumeChangeNotify(int currentVolume)
        {
            RabbitMqService.VoulumeChange(currentVolume);

            var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<ServerHub>();
            if (context == null)
                return;

            context.Clients.All.volumeChange(currentVolume);
        }
    }
}
