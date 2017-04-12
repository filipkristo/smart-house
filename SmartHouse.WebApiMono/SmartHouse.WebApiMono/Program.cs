using System;
using System.Net;
using log4net;
using Microsoft.Owin.Hosting;
using SmartHouse.Lib;
using System.Threading;
using System.Net.Http;
using System.Web.Routing;

namespace SmartHouse.WebApiMono
{
    public class MainClass
    {
        public static readonly ILog Log = LogManager.GetLogger(typeof(MainClass));               
        public static string Port { get; private set; } = "8081";
        public static string Url => $"http://*:{Port}";

        public static void Main(string[] args)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;                

                if (args.Length > 0)
                    Port = args[0];                

                log4net.Config.XmlConfigurator.Configure();
                Log.Info("Application_Start");

                StartTcpServer();
                StartUdpTemperature();
                StartAlarmClock();
                //StartIRCommandPipes();                

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
                    client.GetAsync($"http://127.0.0.1:{Port}/api/SmartHouse/TurnOff").Wait();

                Logger.LogInfoMessage("Smart house turn off");

                var smartHouse = new SmartHouseService();

                using (var orvibioService = new OrviboService())
                {
                    var turnOffResult = orvibioService.TurnOff();
                    Logger.LogInfoMessage("orvibioService.TurnOff");
                    Logger.LogInfoMessage(turnOffResult.Message);

                    Thread.Sleep(TimeSpan.FromMinutes(5));

                    var turnOnResult = orvibioService.TurnOn();
                    Logger.LogInfoMessage("orvibioService.TurnOn");
                    Logger.LogInfoMessage(turnOnResult.Message);                    
                }
            };

            var alarmClock = new AlarmClock(DateTime.Today.AddDays(1).Date.AddTicks(timeSpan.Ticks), action);
            await alarmClock.Start();
        }

        private static async void StartIRCommandPipes()
        {
            var pipe = new IRRemotePipe();
            await pipe.Start();
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
