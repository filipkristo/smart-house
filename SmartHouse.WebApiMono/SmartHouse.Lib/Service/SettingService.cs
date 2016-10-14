using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
				return new Settings() { KodiSettings = new KodiSettings(), YamahaSettings = new YamahaSettings() };

			using (var stream = File.Open(fileName, FileMode.Open, FileAccess.Read))
			{
				var buffer = new byte[stream.Length];
				await stream.ReadAsync(buffer, 0, buffer.Length);

				var json = Encoding.UTF8.GetString(buffer);

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
	}
}
