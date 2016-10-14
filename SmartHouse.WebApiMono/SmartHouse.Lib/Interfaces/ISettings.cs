using System;
namespace SmartHouse.Lib
{
	public interface ISettings
	{
		YamahaSettings YamahaSettings { get; set; }
		KodiSettings KodiSettings { get; set; }
	}
}
