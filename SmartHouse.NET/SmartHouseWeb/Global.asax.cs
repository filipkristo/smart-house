using Microsoft.ServiceBus.Messaging;
using SmartHouseWeb.App_Start;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Newtonsoft.Json;
using SmartHouseWeb.SignalRHubs;
using SmartHouseWebLib.Models;

namespace SmartHouseWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private EventHubClient eventHubClient;

        protected void Application_Start()
        {
            DIConfig.Configure();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //HostingEnvironment.QueueBackgroundWorkItem((ct) => ReceiveMessages(ct));
        }

        private void ReceiveMessages(CancellationToken token)
        {
            Debug.WriteLine("Receive messages.");

            eventHubClient = EventHubClient.CreateFromConnectionString(Config.ConnectionString, Config.IoTHubD2cEndpoint);
            var d2cPartitions = eventHubClient.GetRuntimeInformation().PartitionIds;

            var tasks = new List<Task>();
            foreach (var partition in d2cPartitions)
            {
                tasks.Add(ReceiveMessagesFromDeviceAsync(partition, token));
            }

            Task.WaitAll(tasks.ToArray(), token);
        }

        private async Task ReceiveMessagesFromDeviceAsync(string partition, CancellationToken ct)
        {
            var eventHubReceiver = eventHubClient.GetDefaultConsumerGroup().CreateReceiver(partition, DateTime.UtcNow);

            while (true)
            {
                if (ct.IsCancellationRequested) break;
                var eventData = await eventHubReceiver.ReceiveAsync();
                if (eventData == null) continue;

                var data = Encoding.UTF8.GetString(eventData.GetBytes());
                Debug.WriteLine("Message received. Partition: {0} Data: '{1}'", partition, data);

                var temperature = JsonConvert.DeserializeObject<TelemetryData>(data);
                //temperature.PartitionId = partition;

                var hub = new TelemetryHub();
                //hub.NotifyClient(temperature);
            }
        }
    }
}
