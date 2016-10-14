using System;
namespace SmartHouse.Lib
{
	public class Settings : ISettings
	{
		public YamahaSettings YamahaSettings { get; set; }
		public KodiSettings KodiSettings { get; set; }

		public Settings()
		{
		}
	}
}
