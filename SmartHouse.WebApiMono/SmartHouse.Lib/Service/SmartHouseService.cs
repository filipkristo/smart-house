using System;
using System.Threading.Tasks;
using System.Linq;

namespace SmartHouse.Lib
{
	public class SmartHouseService : ISmartHouseService
	{
		private readonly SettingService settingsService = new SettingService();
		private readonly YamahaService yamahaService = new YamahaService();

		public async Task<Result> SetMode(ModeEnum mode)
		{
			var message = string.Empty;
			var setting = await settingsService.GetSettings();
			var powerStatus = await yamahaService.PowerStatus();

			if (powerStatus == PowerStatusEnum.On)
			{
				var defaultMode = setting.ModeSettings.FirstOrDefault(x => x.Mode == mode);
				message = $"Default volume for mode {defaultMode.Mode} is {defaultMode.Value}";

				await yamahaService.SetVolume(defaultMode.Value);
			}
			else
			{
				message = "Yamaha is turned off.";	
			}

			return new Result
			{
				ErrorCode = 0,
				Message = message,
				Ok = true
			};
		}
	}
}
