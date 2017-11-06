using Microsoft.Azure.Devices.Client;
using SmartHouse.UWPLib.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHouse.UWPLib.Service;

namespace SmartHouse.UWPClient.Messaging
{
    public class AzureIotMessaging
    {
        public async Task ReceiveDataFromAzure()
        {
            var deviceClient = DeviceClient.CreateFromConnectionString("HostName=SmartHouseHub.azure-devices.net;DeviceId=RaspberryPi;SharedAccessKey=pJH2LNVCpMEvhSA5fDk85zl3ECSA7bReYq7S2PCHqPw=");

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
    }
}
