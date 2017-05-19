using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Devices.Geolocation;
using Windows.Storage;
using Windows.UI.Core;

namespace SmartHouse.UWPClient.BackgroundTasks
{
    public class GeofenceTask
    {
        private const string BackgroundTaskName = "SmartHouseGeofenceBackgroundTask";
        private const string BackgroundTaskEntryPoint = "BackgroundTask.GeofenceBackgroundTask";

        private IBackgroundTaskRegistration backgroundTask = null;

        public async Task<IBackgroundTaskRegistration> RegisterBackgroundTask()
        {
            if (BackgroundTaskRegistration.AllTasks.Any(x => x.Value.Name == BackgroundTaskName))
                return null;

            var backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();
            switch (backgroundAccessStatus)
            {
                case BackgroundAccessStatus.AlwaysAllowed:
                case BackgroundAccessStatus.AllowedSubjectToSystemPolicy:
                    RequestLocationAccess();
                    break;                    
                default:
                    throw new Exception("Background task disabled");                    
            }

            var geofenceTaskBuilder = new BackgroundTaskBuilder()
            {
                Name = BackgroundTaskName,
                TaskEntryPoint = BackgroundTaskEntryPoint
            };

            var trigger = new LocationTrigger(LocationTriggerType.Geofence);
            geofenceTaskBuilder.SetTrigger(trigger);

            backgroundTask = geofenceTaskBuilder.Register();
            return backgroundTask;            
        }

        /// <summary>
        /// Get permission for location from the user. If the user has already answered once,
        /// this does nothing and the user must manually update their preference via Settings.
        /// </summary>
        private async void RequestLocationAccess()
        {
            // Request permission to access location
            var accessStatus = await Geolocator.RequestAccessAsync();

            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:
                    break;

                case GeolocationAccessStatus.Denied:                    
                    break;

                case GeolocationAccessStatus.Unspecified:                    
                    break;
            }
        }

        /// <summary>
        /// This is the callback when background event has been handled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCompleted(IBackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs e)
        {
            if (sender != null)
            {                
                try
                {
                    // If the background task threw an exception, display the exception in
                    // the error text box.
                    e.CheckResult();

                    // Update the UI with the completion status of the background task
                    // The Run method of the background task sets the LocalSettings. 
                    var settings = ApplicationData.Current.LocalSettings;

                    // get status
                    if (settings.Values.ContainsKey("Status"))
                    {
                        //_rootPage.NotifyUser(settings.Values["Status"].ToString(), NotifyType.StatusMessage);
                    }

                    //FillEventListBoxWithExistingEvents();
                }
                catch (Exception ex)
                {
                    // The background task had an error
                    //_rootPage.NotifyUser(ex.ToString(), NotifyType.ErrorMessage);
                }
            }
        }        
    }
}
