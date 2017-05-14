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
using Windows.Devices.Geolocation;
using Windows.Devices.Geolocation.Geofencing;
using Windows.ApplicationModel.Background;
using System.Collections.ObjectModel;
using SmartHouse.UWPClient.BackgroundTasks;
using System.Diagnostics;
using Newtonsoft.Json;
using SmartHouse.UWPLib.BLL;

namespace SmartHouse.UWPClient.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private readonly GeofenceTask geofenceTask;

        public Visibility VPNVisible { get { return Get<Visibility>(); } set { Set(value); } }

        public string PingStatus { get { return Get<string>(); } set { Set(value); } }
        private IList<Geofence> geofences = new List<Geofence>();

        public MainPageViewModel()
        {
            geofenceTask = new GeofenceTask();
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            await InitializeGeolocation();                            
            await Ping();
        }       

        private async Task InitializeGeolocation()
        {
            // Get permission to use location
            var accessStatus = await Geolocator.RequestAccessAsync();
            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:                    
                    CheckGeoFancesAddress();                    
                    await geofenceTask.RegisterBackgroundTask();

                    // register for state change events
                    GeofenceMonitor.Current.GeofenceStateChanged += Current_GeofenceStateChanged;
                    GeofenceMonitor.Current.StatusChanged += Current_StatusChanged;
                    break;

                case GeolocationAccessStatus.Denied:
                    Status = "Access denied.";
                    break;

                case GeolocationAccessStatus.Unspecified:
                    Status = "Unspecified error.";
                    break;
            }
        }

        private void Current_StatusChanged(GeofenceMonitor sender, object args)
        {
            Debug.Write("Sender: "); 
            Debug.WriteLine(JsonConvert.SerializeObject(sender));

            Debug.Write("Args: ");
            Debug.WriteLine(JsonConvert.SerializeObject(args));
        }

        private void Current_GeofenceStateChanged(GeofenceMonitor sender, object args)
        {
            Debug.Write("Sender: ");
            Debug.WriteLine(JsonConvert.SerializeObject(sender));

            Debug.Write("Args: ");
            Debug.WriteLine(JsonConvert.SerializeObject(args));
        }

        private void CheckGeoFancesAddress()
        {
            var helper = new GeofenceLocationHelper();

            geofences = GeofenceMonitor.Current.Geofences;            

            if(!geofences.Any(x => x.Id == GeofenceLocationHelper.HOME_KEY))
            {
                var geofence = helper.GetHomeLocation();
                geofences.Add(geofence);
            }

            if (!geofences.Any(x => x.Id == GeofenceLocationHelper.WORK_KEY))
            {
                var geofence = helper.GetWorkLocation();
                geofences.Add(geofence);
            }
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

