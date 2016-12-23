using System;
using System.ComponentModel.DataAnnotations;

namespace SmartHouse.Lib
{
	public class SongDetails
	{
		[Required]
		public string Artist { get; set; }

		[Required]
		public string Album { get; set; }

		[Required]
		public string Song { get; set; }

		public SongDetails()
		{
		}
	}
}
