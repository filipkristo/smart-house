using System;
using System.Net;
using log4net;
using Microsoft.Owin.Hosting;
using SmartHouse.Lib;
using System.Threading;
using System.Net.Http;

namespace SmartHouse.WebApiMono
{
	public class MainClass
	{
		public static readonly ILog Log = LogManager.GetLogger(typeof(MainClass));

		public static String SslPort { get; private set; } = "5001";
		public static String SslUrl => $"https://*:{SslPort}";

		public static String Port { get; private set; } = "8081";
		public static String Url => $"http://*:{Port}";

		public static void Main(string[] args)
		{
			try
			{
				if (args.Length > 0)
					Port = args[0];

				if (args.Length > 1)
					SslPort = args[1];

				log4net.Config.XmlConfigurator.Configure();
				Log.Info("Application_Start");

				StartTcpServer();
				StartUdpTemperature();
                StartAlarmClock();

				StartSelfHosting();

				Console.ReadLine();
			}
			catch (Exception ex)
			{
				Log.Error("Unhandled exception", ex);
				throw;
			}
		}

		private static void StartSelfHosting()
		{
			try
			{
				ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, error) =>
				{					
					return true;
				};

				ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

				var Server = WebApp.Start<WebApiConfig>(Url);
				var message = $"Started self hosting at {Url}.";
				Log.Info(message);

			}
			catch (Exception ex)
			{
				Log.Error($"Exception StartSelfHosting", ex);
			}
		}

		private static async void StartTcpServer()
		{
			try
			{
				var tcp = new TcpServer();
				await tcp.StartTcpServer();
			}
			catch (Exception ex)
			{
				Log.Error("TCP Server Error", ex);
			}
		}

		private static async void StartUdpTemperature()
		{
			try
			{
				var udp = new TemperatureUdp(SignalRTemperature);
				await udp.StartListen();
			}
			catch (Exception ex)
			{
				Log.Error("UDP Server Error", ex);
			}
		}

        private static async void StartAlarmClock()
        {
            var timeSpan = new TimeSpan(4, 30, 0);
            Action action = () =>
            {
                using (var client = new HttpClient())
                    client.GetAsync("http://127.0.0.1:8081/api/SmartHouse/TurnOff");

                var smartHouse = new SmartHouseService();                

                using (var orvibioService = new OrvibioService())
                {
                    var turnOffResult = orvibioService.TurnOff();
                    Logger.LogInfoMessage($"TurnOff result: {turnOffResult.Message}");

                    Thread.Sleep(TimeSpan.FromSeconds(10));

                    var turnOnResult = orvibioService.TurnOn();
                    Logger.LogInfoMessage($"TurnOn result: {turnOnResult.Message}");

                    Thread.Sleep(TimeSpan.FromSeconds(30));

                    Logger.LogInfoMessage("Starting to restart VPN");
                    smartHouse.RestartOpenVPNService().Wait(TimeSpan.FromSeconds(15));
                    Logger.LogInfoMessage("VPN Restarted");
                }                    
            };
            var alarmClock = new AlarmClock(DateTime.Today.AddDays(1).Date.AddTicks(timeSpan.Ticks), action);
            await alarmClock.Start();         
        }

		private static void SignalRTemperature(TemperatureData temperature)
		{
			var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<ServerHub>();
			if (context == null)
				return;
			
			context.Clients.All.temperature(temperature);
		}

	}
}
