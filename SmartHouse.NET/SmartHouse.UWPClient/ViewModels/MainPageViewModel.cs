using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel;
using Windows.Networking.Sockets;
using Windows.UI.Xaml;
using SmartHouse.UWPClient.Services.SettingsServices;

namespace SmartHouse.UWPClient.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public Visibility VPNVisible { get { return Get<Visibility>(); } set { Set(value); } }

        public string PingStatus { get { return Get<string>(); } set { Set(value); } }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            await Ping();
        }    
        
        public async Task Ping()
        {
            try
            {
                if(String.IsNullOrWhiteSpace(SettingsService.Instance.HostIP) || String.IsNullOrWhiteSpace(SettingsService.Instance.HostPort))
                {
                    Status = "Host IP or port are empty";
                    return;
                }

                Status = "Checking server";
                PingStatus = "";

                VPNVisible = Visibility.Collapsed;

                using (var tcpClient = new StreamSocket())
                {
                    await tcpClient.ConnectAsync(
                        new Windows.Networking.HostName(SettingsService.Instance.HostIP),
                        "80",
                        SocketProtectionLevel.PlainSocket);

                    var localIp = tcpClient.Information.LocalAddress.DisplayName;
                    var remoteIp = tcpClient.Information.RemoteAddress.DisplayName;
                }
                                
                PingStatus = "Connected to server";
            }
            catch (Exception ex)
            {
                PingStatus = $"Error: {ex.Message}";
                VPNVisible = Visibility.Visible;
            }

            Status = "";
        }


    }
}

