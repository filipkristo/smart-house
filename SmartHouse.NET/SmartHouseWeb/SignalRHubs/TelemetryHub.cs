using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using SmartHouseWeb.Models;
using SmartHouseWebLib.DomainService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SmartHouseWeb.SignalRHubs
{
    [Authorize()]
    public class TelemetryHub : Hub
    {
        public static readonly ConnectionMapping<string> Connections = new ConnectionMapping<string>();

        public void NotifyClient(TelemetryDataDto telemetry, string userId)
        {
            if (Connections.UserExists(userId))
            {
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<TelemetryHub>();
                var clients = Connections.GetConnections(userId).ToList();

                hubContext.Clients.Clients(clients).receiveTelemetry(telemetry);
            }
        }

        public override Task OnConnected()
        {
            var userId = Context.User.Identity.GetUserId();
            Connections.Add(userId, Context.ConnectionId);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var userId = Context.User.Identity.GetUserId();
            Connections.Remove(userId, Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            var userId = Context.User.Identity.GetUserId();
            if (!Connections.GetConnections(userId).Contains(Context.ConnectionId))
            {
                Connections.Add(userId, Context.ConnectionId);
            }

            return base.OnReconnected();
        }
    }
}