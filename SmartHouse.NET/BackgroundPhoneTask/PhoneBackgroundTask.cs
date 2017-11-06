using SmartHouse.Lib;
using System;
using SmartHouse.UWPLib.Service;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
        private const string Key = "PhoneCallStartedKey";

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            var isLocalNetwork = await IsInLocalNetwork();

	        var service = new SmartHouseService();
	        if(isLocalNetwork)
            {
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
            settings.Values[Key] = DateTime.UtcNow;
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
                        new Windows.Networking.HostName(SettingsService.Instance.HostIP),
                        "80",
                        SocketProtectionLevel.PlainSocket);
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
