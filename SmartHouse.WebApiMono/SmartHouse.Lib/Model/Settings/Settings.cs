using System;
using System.Collections.Generic;

namespace SmartHouse.Lib
{
	public class Settings : ISettings
	{
		public YamahaSettings YamahaSettings { get; set; }
		public KodiSettings KodiSettings { get; set; }
		public MPDSettings MPDSettings { get; set; }
		public List<ModeSettings> ModeSettings { get; set; }
		public bool LastFM { get; set; } = true;
		public bool RestartVpn { get; set; } = true;

		public Settings()
		{
		}
	}
}
