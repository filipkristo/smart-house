using System;
using System.ComponentModel.DataAnnotations;

namespace SmartHouse.Lib
{
	public class MPDSettings
	{
		[Required]
		public string IPAddress { get; set; } = "10.110.166.90";

		[Required]
		public int Port { get; set; } = 6600;

		public MPDSettings()
		{
		}
	}
}
