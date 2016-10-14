using System;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
	public interface ISettingsService
	{
		Task<ISettings> GetSettings();
		Task SaveSettings(Settings settings);
	}
}
