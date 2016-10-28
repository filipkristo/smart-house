using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;

namespace SmartHouse.Lib
{
	public class SettingService : ISettingsService
	{
		private const string fileName = "settings.json";

		public SettingService()
		{
			
		}

		public async Task<ISettings> GetSettings()
		{
			if (!File.Exists(fileName))
			{
				var setting = GetDefaultSettings();
				return setting;
			}				

			using (var stream = File.Open(fileName, FileMode.Open, FileAccess.Read))
			{
				var buffer = new byte[stream.Length];
				await stream.ReadAsync(buffer, 0, buffer.Length);

				var json = Encoding.UTF8.GetString(buffer);
				Logger.LogInfoMessage(json);

				var settings = JsonConvert.DeserializeObject(json) as Settings;
				return settings;
			}
		}

		public async Task SaveSettings(Settings settings)
		{
			var json = JsonConvert.SerializeObject(settings);
			var buffer = Encoding.UTF8.GetBytes(json);

			using (var stream = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
			{
				await stream.WriteAsync(buffer, 0, buffer.Length);
			}
		}

		private Settings GetDefaultSettings()
		{
			return new Settings() 
			{ 
				KodiSettings = new KodiSettings(),
				YamahaSettings = new YamahaSettings(),
				ModeSettings = new[] { 
					new ModeSettings() { Mode = ModeEnum.Quiete, Value = -420 }, 
					new ModeSettings() { Mode = ModeEnum.Normal, Value = -320 }, 
					new ModeSettings() { Mode = ModeEnum.Loud, Value = -230 }, 
					new ModeSettings() { Mode = ModeEnum.ExtraLoud, Value = -180 } 
				}.ToList()
			};
		}
	}
}
