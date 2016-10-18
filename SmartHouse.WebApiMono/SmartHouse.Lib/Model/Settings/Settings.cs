﻿using System;
using System.Collections.Generic;

namespace SmartHouse.Lib
{
	public class Settings : ISettings
	{
		public YamahaSettings YamahaSettings { get; set; }
		public KodiSettings KodiSettings { get; set; }
		public List<ModeSettings> ModeSettings { get; set; }

		public Settings()
		{
		}
	}
}