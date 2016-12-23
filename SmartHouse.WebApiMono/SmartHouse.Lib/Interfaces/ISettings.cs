using System;
using System.Collections.Generic;

namespace SmartHouse.Lib
{
	public interface ISettings
	{
		YamahaSettings YamahaSettings { get; set; }
		KodiSettings KodiSettings { get; set; }
		List<ModeSettings> ModeSettings { get; set; }
		bool LastFM { get; set; }
	}
}
