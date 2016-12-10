using System;
using System.ComponentModel.DataAnnotations;

namespace SmartHouse.Lib
{
	public class KodiSettings
	{
		[Required]
		public string IPAddress { get; set; }
		[Required]
		public int Port { get; set; }

		public string username { get; set; }
		public string password { get; set; }

		public string Url => $"http://{IPAddress}:{Port}/";

		public KodiSettings()
		{
		}
	}
}
