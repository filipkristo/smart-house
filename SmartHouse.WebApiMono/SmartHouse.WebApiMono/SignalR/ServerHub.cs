using System;
using Microsoft.AspNet.SignalR;

namespace SmartHouse.WebApiMono
{
	public class ServerHub : Hub
	{
		public ServerHub()
		{
		}

		public void Hello(string param = null)
		{
			Clients.All.hello(param ?? "Call from server");
		}

		public void NotifyClients()
		{
			Clients.All.refreshAll();
		}

		public void PushNotification(string message)
		{
			Clients.All.notification(message);
		}
	}
}
