using System;
namespace SmartHouse.Lib
{
	public class PandoraResult
	{
		public string Artist { get; set; }
		public string Song { get; set; }
		public string Radio { get; set; }
		public bool Loved { get; set; }
		public string AlbumUri { get; set; }
		public string Album { get; set; }

		public PandoraResult()
		{
		}
	}
}
