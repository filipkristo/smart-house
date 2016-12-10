using System;
using System.ComponentModel.DataAnnotations;

namespace SmartHouse.Lib
{
	public class ModeSettings
	{
		[Required]
		public ModeEnum Mode { get; set; }

		[Required]
		public int Value { get; set; }

		public ModeSettings()
		{
			
		}
	}
}
