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
using SmartHouse.UWPLib.Service;
using Windows.Devices.Geolocation;
using Windows.Devices.Geolocation.Geofencing;
using Windows.ApplicationModel.Background;
using System.Collections.ObjectModel;
using SmartHouse.UWPClient.BackgroundTasks;
using System.Diagnostics;
using Newtonsoft.Json;
using SmartHouse.UWPLib.BLL;
using Windows.Storage;
using Windows.Data.Json;
using Windows.Security.Credentials;
using SmartHouse.UWPLib.Service;
using SmartHouse.UWPLib.Model;
using Windows.Foundation.Metadata;

namespace SmartHouse.UWPClient.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private WebClientService webClient;
        private readonly GeofenceTask geofenceTask;
        private IList<Geofence> geofences = new List<Geofence>();

        public Visibility VPNVisible { get { return Get<Visibility>(); } set { Set(value); } }

        public string PingStatus { get { return Get<string>(); } set { Set(value); } }
        public ObservableCollection<string> GeofenceBackgroundEvents { get { return Get<ObservableCollection<string>>(); } set { Set(value); } }

        public MainPageViewModel()
        {
            geofenceTask = new GeofenceTask();
            GeofenceBackgroundEvents = new ObservableCollection<string>();
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            await Ping();

            if (ApiInformation.IsTypePresent("Windows.Devices.Geolocation.Geolocator"))
            {
                await InitializeGeolocation();
            }        
            if(ApiInformation.IsTypePresent("Windows.ApplicationModel.Background.PhoneTrigger"))
            {
                InitializePhoneTask();
            }

            InitializeTileBackgroundTask();
        }      
        
        private void InitializePhoneTask()
        {
            var task = new PhoneCallTask();
            var backgroundTask = task.RegisterBackgroundTask();
        }

        private void InitializeTileBackgroundTask()
        {
            var register = new TileBackgroundTaskRegister();
            register.RegisterBackgroundTask();
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
                    FillEventListBoxWithExistingEvents();

                    // register for state change events
                    GeofenceMonitor.Current.GeofenceStateChanged += Current_GeofenceStateChanged;
                    GeofenceMonitor.Current.StatusChanged += Current_StatusChanged;

                    TryToInitializeWebClient();                                  
                    break;

                case GeolocationAccessStatus.Denied:
                    Status = "Access denied.";
                    break;

                case GeolocationAccessStatus.Unspecified:
                    Status = "Unspecified error.";
                    break;
            }
        }

        private async void Current_StatusChanged(GeofenceMonitor sender, object args)
        {
            Debug.Write("Sender: "); 
            Debug.WriteLine(JsonConvert.SerializeObject(sender));

            Debug.Write("Args: ");
            Debug.WriteLine(JsonConvert.SerializeObject(args));

            var userLocation = new UserLocation()
            {
                Latitude = sender.LastKnownGeoposition.Coordinate.Point.Position.Latitude,
                Longitude = sender.LastKnownGeoposition.Coordinate.Point.Position.Longitude,
                Name = "Current_StatusChanged",
                UpdatedUtc = DateTime.UtcNow,
                Status = LocationStatus.None
            };

            if (webClient != null)
                await UploadLocationToCloud(userLocation);
        }

        private async void Current_GeofenceStateChanged(GeofenceMonitor sender, object args)
        {
            Debug.Write("Sender: ");
            Debug.WriteLine(JsonConvert.SerializeObject(sender));

            Debug.Write("Args: ");
            Debug.WriteLine(JsonConvert.SerializeObject(args));

            var userLocation = new UserLocation()
            {
                Latitude = sender.LastKnownGeoposition.Coordinate.Point.Position.Latitude,
                Longitude = sender.LastKnownGeoposition.Coordinate.Point.Position.Longitude,
                Name = "GeofenceStateChanged",
                UpdatedUtc = DateTime.UtcNow,
                Status = LocationStatus.None
            };

            if(webClient != null)
                await UploadLocationToCloud(userLocation);
        }

        private void TryToInitializeWebClient()
        {
            try
            {
                var settings = SettingsService.Instance;
                var credential = settings.GetCredentialFromLocker();

                if (!string.IsNullOrWhiteSpace(settings.WebHost) && credential != null)
                {
                    webClient = new WebClientService(settings.WebHost, credential.UserName, credential.Password);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
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

        private void FillEventListBoxWithExistingEvents()
        {
            var settings = ApplicationData.Current.LocalSettings;
            if (settings.Values.ContainsKey("BackgroundGeofenceEventCollection"))
            {
                var geofenceEvent = settings.Values["BackgroundGeofenceEventCollection"].ToString();

                if (geofenceEvent.Length != 0)
                {                    
                    GeofenceBackgroundEvents.Clear();

                    var events = JsonValue.Parse(geofenceEvent).GetArray();

                    // NOTE: the events are accessed in reverse order
                    // because the events were added to JSON from
                    // newer to older.  _geofenceBackgroundEvents.Insert() adds
                    // each new entry to the beginning of the collection.
                    for (int pos = events.Count - 1; pos >= 0; pos--)
                    {
                        var element = events.GetStringAt((uint)pos);
                        GeofenceBackgroundEvents.Insert(0, element);
                    }
                }
            }
        }

        private async Task UploadLocationToCloud(UserLocation userLocation)
        {
            try
            {
                await webClient.Login();
                var id = await webClient.SendUserLocation(userLocation);
                Debug.WriteLine($"Uploaded to web server: {id}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }



    }
}

