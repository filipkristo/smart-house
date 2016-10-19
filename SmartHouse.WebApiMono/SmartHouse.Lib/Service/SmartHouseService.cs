using System;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.Text;

namespace SmartHouse.Lib
{
	public class SmartHouseService : ISmartHouseService
	{
		private const string stateFile = "state";

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
				message = $"Default volume for mode {mode} is {defaultMode.Value}";

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

		public async Task SaveState(SmartHouseState state)
		{
			using (var fileStream = File.Open(stateFile, FileMode.Create, FileAccess.Write, FileShare.None))
			{
				var bytes = Encoding.UTF8.GetBytes(state.ToString());
				await fileStream.WriteAsync(bytes, 0, bytes.Length);
			}
		}

		public async Task<SmartHouseState> GetCurrentState()
		{
			if (!File.Exists(stateFile))
				return SmartHouseState.Unknown;

			using (var fileStream = File.Open(stateFile, FileMode.Open, FileAccess.Read))
			{
				var bytes = new byte[fileStream.Length];
				await fileStream.ReadAsync(bytes, 0, bytes.Length);

				var state = Encoding.UTF8.GetString(bytes);
				Logger.LogInfoMessage($"state from file: {state}");

				return (SmartHouseState)Enum.Parse(typeof(SmartHouseState), state);
			}
		}
	}
}
