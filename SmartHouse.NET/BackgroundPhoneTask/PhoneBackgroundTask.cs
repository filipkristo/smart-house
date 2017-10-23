using SmartHouse.Lib;
using SmartHouse.UWPLib.BLL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Networking.Sockets;
using Windows.Storage;

namespace BackgroundPhoneTask
{
    public sealed class PhoneBackgroundTask : IBackgroundTask
    {
        private const string Key = "PhoneCallStarted";

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();

            var isLocalNetwork = await IsInLocalNetwork();

            if(isLocalNetwork)
            {
                var service = new SmartHouseService();
                var started = IsPhoneCallStarted();

                if (started)
                {
                    PhoneCallStarted();

                    var model = GetPhoneCallModel();
                    await service.PhoneCallStarted(model);
                }
                else
                {
                    PhoneCallEnded();
                    await service.PhoneCallEnded();
                }
            }

            deferral.Complete();
        }

        private PhoneCallData GetPhoneCallModel()
        {
            return new PhoneCallData()
            {
                Id = Guid.NewGuid().ToString(),
                Started = DateTime.UtcNow,
                State = "State",
                PhoneNumber = "unknown"
            };
        }

        private bool IsPhoneCallStarted()
        {
            var settings = ApplicationData.Current.LocalSettings;
            return !settings.Values.ContainsKey(Key);
        }

        private void PhoneCallStarted()
        {
            var settings = ApplicationData.Current.LocalSettings;
            settings.Values[Key] = DateTime.UtcNow.ToString();
        }

        private void PhoneCallEnded()
        {
            var settings = ApplicationData.Current.LocalSettings;
            settings.Values.Remove(Key);
        }

        private async Task<bool> IsInLocalNetwork()
        {
            try
            {
                using (var tcpClient = new StreamSocket())
                {
                    await tcpClient.ConnectAsync(
                        new Windows.Networking.HostName("10.110.166.90"),
                        "80",
                        SocketProtectionLevel.PlainSocket);

                    var localIp = tcpClient.Information.LocalAddress.DisplayName;
                    var remoteIp = tcpClient.Information.RemoteAddress.DisplayName;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
