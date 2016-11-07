using System;
namespace SmartHouse.Lib
{
	public class MPDSettings
	{
		public string IPAddress { get; set; } = "10.110.166.90";
		public int Port { get; set; } = 6600;

		public MPDSettings()
		{
		}
	}
}
