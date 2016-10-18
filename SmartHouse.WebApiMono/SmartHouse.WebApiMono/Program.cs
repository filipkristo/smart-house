﻿using System;
using System.Net;
using log4net;
using Microsoft.Owin.Hosting;
using SmartHouse.Lib;

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
	}
}