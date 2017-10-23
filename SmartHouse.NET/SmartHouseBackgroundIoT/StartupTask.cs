using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using SmartHouse.UWPLib.Service;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using SmartHouse.Lib;
using System.Diagnostics;
using Microsoft.Azure.Devices.Client;
using SmartHouse.UWPLib.BLL;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace SmartHouseBackgroundIoT
{
    public sealed class StartupTask : IBackgroundTask
    {
        private int skiped;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();

            await StartSignalRClient();
            await ReceiveDataFromAzure();

            deferral.Complete();
        }        

        private async Task StartSignalRClient()
        {
            var IP = "10.110.166.90";
            var Port = "8081";

            var hubConnection = new HubConnection($"http://{IP}:{Port}/");
            var hubProxy = hubConnection.CreateHubProxy("ServerHub");
            hubProxy.On<TelemetryData>("temperature", async (data) => await UploadToCloud(data));

            await hubConnection.Start();
        }

        private async Task UploadToCloud(TelemetryData telemetry)
        {
            if (skiped == 6)
            {
                skiped = 0;

                try
                {
                    var settings = SettingsService.Instance;
                    var credential = settings.GetCredentialFromLocker();

                    if (!string.IsNullOrWhiteSpace(settings.WebHost) && credential != null)
                    {
                        var webclient = new WebClientService(settings.WebHost, credential.UserName, credential.Password);

                        await webclient.Login();
                        var result = await webclient.SendTelemetryData(telemetry);

                        Debug.WriteLine($"Result: {result}");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
            else
            {
                skiped++;
            }
        }

        private async Task ReceiveDataFromAzure()
        {
            var deviceClient = DeviceClient.CreateFromConnectionString("XXX");

            while (true)
            {
                var receivedMessage = await deviceClient.ReceiveAsync();
                if (receivedMessage != null)
                {
                    var messageData = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                    await deviceClient.CompleteAsync(receivedMessage);

                    switch (messageData)
                    {
                        case "TurnOn":
                            await TurnOnAirCondition();
                            break;
                        case "TurnOff":
                            await TurnOffAirCondition();
                            break;
                    }
                }
            }
        }

        private async Task TurnOffAirCondition()
        {
            var smartHouseService = new SmartHouseService();
            await smartHouseService.TurnOffAirConditioner();
        }

        private async Task TurnOnAirCondition()
        {
            var smartHouseService = new SmartHouseService();
            await smartHouseService.TurnOnAirConditioner();
        }

        //private void OnUserProximitySensorApproach()

        //{ // leave if Cortana isn't available

        //    if (!Windows.Services.Cortana.CortanaPermissionsManager.GetDefault().)

        //    {

        //        return;

        //    }

        //    // enable voice activation if allowed and not already done

        //    if
        //    (!Windows.Services.Cortana.)

        //    {

        //        // voice activation isn’t allowed by user

        //        //

        //        // Note that, user consent can be obtained by launching

        //        // ms-cortana://CapabilitiesPrompt/?RequestedCapabilities

        //        // =InputPersonalization,Microphone&QuerySource=

        //        // Microphone&QuerySourceSecondaryId=IoT

        //        return;

        //    }

        //    else if
        //    (!Windows.Services.Cortana.CortanaSettings.IsVoiceActivationEnabled)

        //    {

        //        Windows.Services.Cortana.CortanaSettings.IsVoiceActivationEnabled =
        //        true;

        //    }

        //}
    }
}
