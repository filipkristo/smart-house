using System;
namespace SmartHouse.Lib
{
	public class YamahaSettings
	{
		public string IPAddress { get; set; } = "10.110.167.49";
		public int Port { get; set; } = 80;
		public string username { get; set; }
		public string password { get; set; }

		public string Url => $"http://{IPAddress}:{Port}/";

		public YamahaSettings()
		{
		}
	}
}
