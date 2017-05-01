using System;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.Text;
using System.Net.Http;

namespace SmartHouse.Lib
{
    public class SmartHouseService : ISmartHouseService
    {
        private static SmartHouseState? cacheState;

        private const string stateFile = "state";

        private readonly SettingService settingsService = new SettingService();
        private readonly YamahaService yamahaService = new YamahaService();



        public async Task<Result> SetMode(ModeEnum mode, Action<int> notifyAction)
        {
            var message = string.Empty;
            var setting = await settingsService.GetSettings();
            var powerStatus = await yamahaService.PowerStatus();

            if (powerStatus == PowerStatusEnum.On)
            {
                var defaultMode = setting.ModeSettings.FirstOrDefault(x => x.Mode == mode);
                message = $"Default volume for mode {mode} is {defaultMode.Value}";

                await yamahaService.SetVolume(defaultMode.Value);
                notifyAction?.Invoke(defaultMode.Value);
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
            cacheState = state;

            using (var fileStream = File.Open(stateFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var bytes = Encoding.UTF8.GetBytes(state.ToString());
                await fileStream.WriteAsync(bytes, 0, bytes.Length);
            }
        }

        public async Task<SmartHouseState> GetCurrentState()
        {
            if (cacheState != null)
                return cacheState.Value;

            if (!File.Exists(stateFile))
                return SmartHouseState.Unknown;

            using (var fileStream = File.Open(stateFile, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                var bytes = new byte[fileStream.Length];
                await fileStream.ReadAsync(bytes, 0, bytes.Length);

                var state = Encoding.UTF8.GetString(bytes);
                Logger.LogInfoMessage($"state from file: {state}");

                return (SmartHouseState)Enum.Parse(typeof(SmartHouseState), state);
            }
        }

        public async Task<Result> RestartOpenVPNService()
        {
            BashHelper.ExecBashCommand("sudo service openvpn stop USVPN");
            await Task.Delay(500);
            BashHelper.ExecBashCommand("sudo service openvpn start USVPN");

            return new Result()
            {
                ErrorCode = 0,
                Message = "OpenVPN service has been restarted",
                Ok = true
            };
        }

        public async Task<Result> RestartOpenVPNServiceTcp()
        {
            var tcp = new TcpServer();
            return await tcp.SendCommandToServer<Result>(TcpCommands.SmartHouse.RESTART_VPN);
        }

        public Result PlayAlarm()
        {
            BashHelper.PlayAudio("redalert.wav");

            return new Result()
            {
                ErrorCode = 0,
                Message = "Ok",
                Ok = true
            };
        }

        public async Task<Result> PlayAlarmTcp()
        {
            var tcp = new TcpServer();
            return await tcp.SendCommandToServer<Result>(TcpCommands.SmartHouse.PLAY_ALARM);
        }

        public async Task<Result> TimerTcp()
        {
            var tcp = new TcpServer();
            return await tcp.SendCommandToServer<Result>(TcpCommands.SmartHouse.TIMER);
        }

        public async Task CheckVPNInternet()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(3);                    
                    var response = await client.GetAsync("http://www.google.com");

                    Logger.LogInfoMessage($"Get result code from google: {response.StatusCode}");

                    if (!response.IsSuccessStatusCode)
                        await RestartOpenVPNServiceTcp();
                }
            }
            catch (Exception ex)
            {
                Logger.LogErrorMessage("Exception: Get result code from google", ex);
                RestartOpenVPNServiceTcp().Wait(TimeSpan.FromSeconds(4));
            }

        }
    }
}
