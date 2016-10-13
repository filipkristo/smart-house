using System;
using System.Net;
using Microsoft.Owin.Hosting;

namespace SmartHouse.WebApiMono
{
	class MainClass
	{
		public static String Port { get; private set; } = "5000";
		public static String Url => $"http://*:{Port}";

		public static void Main(string[] args)
		{
			if (args.Length > 0)
				Port = args[0];

			StartSelfHosting();
			Console.ReadLine();
		}

		private static void StartSelfHosting()
		{
			try
			{
				ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, error) =>
				{
					// Ignore errors
					return true;
				};
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

				var Server = WebApp.Start<SelfhostStartup>(Url);

				var message = $"Started self hosting at {Url}.";
				Console.WriteLine(message);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Exception: {ex.Message}");
			}
		}
	}
}
