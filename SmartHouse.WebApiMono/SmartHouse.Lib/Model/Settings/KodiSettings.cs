using System;
namespace SmartHouse.Lib
{
	public class KodiSettings
	{
		public string IPAddress { get; set; }
		public int Port { get; set; }
		public string username { get; set; }
		public string password { get; set; }

		public string Url => $"http://{IPAddress}:{Port}/";

		public KodiSettings()
		{
		}
	}
}
