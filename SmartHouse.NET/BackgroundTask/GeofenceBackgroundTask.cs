using Newtonsoft.Json;
using SmartHouse.UWPLib.BLL;
using SmartHouse.UWPLib.Model;
using SmartHouse.UWPLib.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Data.Json;
using Windows.Devices.Geolocation;
using Windows.Devices.Geolocation.Geofencing;
using Windows.Security.Credentials;
using Windows.Storage;
using Windows.UI.Notifications;

namespace BackgroundTask
{
    public sealed class GeofenceBackgroundTask : IBackgroundTask
    {
        private ObservableCollection<string> _geofenceBackgroundEvents = null;
        private WebClientService webclient;
        private const int maxEventDescriptors = 42;                     // Value determined by how many max length event descriptors (91 chars) 
                                                                        // stored as a JSON string can fit in 8K (max allowed for local settings)

        public GeofenceBackgroundTask()
        {
            _geofenceBackgroundEvents = new ObservableCollection<string>();
        }

        async void IBackgroundTask.Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();

            try
            {
                var geoLocator = new Geolocator();
                var position = await geoLocator.GetGeopositionAsync();

                TryToInitializeWebClient();
                await GetGeofenceStateChangedReports(position);
            }
            catch (UnauthorizedAccessException)
            {
                WriteStatusToAppData("Location permissions are disabled. Enable access through the settings.");
                WipeGeofenceDataFromAppData();
            }
            finally
            {
                deferral.Complete();
            }
        }

        private void WipeGeofenceDataFromAppData()
        {
            var settings = ApplicationData.Current.LocalSettings;
            settings.Values["GeofenceEvent"] = "";
        }

        private void WriteStatusToAppData(string status)
        {
            var settings = ApplicationData.Current.LocalSettings;
            settings.Values["Status"] = status;
        }

        private async Task GetGeofenceStateChangedReports(Geoposition pos)
        {
            _geofenceBackgroundEvents.Clear();
            FillEventCollectionWithExistingEvents();

            GeofenceMonitor monitor = GeofenceMonitor.Current;
            Geoposition posLastKnown = monitor.LastKnownGeoposition;

            Windows.Globalization.Calendar calendar = new Windows.Globalization.Calendar();
            Windows.Globalization.DateTimeFormatting.DateTimeFormatter formatterLongTime;
            formatterLongTime = new Windows.Globalization.DateTimeFormatting.DateTimeFormatter("{hour.integer}:{minute.integer(2)}:{second.integer(2)}", new[] { "hr-HR" }, "HR", Windows.Globalization.CalendarIdentifiers.Gregorian, Windows.Globalization.ClockIdentifiers.TwentyFourHour);

            bool eventOfInterest = true;

            // NOTE TO DEVELOPER:
            // Registered geofence events can be filtered out if the
            // geofence event time is stale.
            var eventDateTime = pos.Coordinate.Timestamp;

            calendar.SetToNow();
            var nowDateTime = calendar.GetDateTime();
            var diffTimeSpan = nowDateTime - eventDateTime;

            var deltaInSeconds = diffTimeSpan.TotalSeconds;

            if (deltaInSeconds > 120)
                eventOfInterest = false;

            if (eventOfInterest)
            {
                // NOTE TO DEVELOPER:
                // This event can be filtered out if the
                // geofence event location is too far away.
                var distance = 0.0d;

                if ((posLastKnown.Coordinate.Point.Position.Latitude != pos.Coordinate.Point.Position.Latitude) ||
                    (posLastKnown.Coordinate.Point.Position.Longitude != pos.Coordinate.Point.Position.Longitude))
                {

                    distance = new HaversineFormula().Distance(posLastKnown.Coordinate.Point.Position, pos.Coordinate.Point.Position, HaversineFormula.DistanceType.Kilometers);

                    // NOTE TO DEVELOPER:
                    // Use an algorithm like the Haversine formula or Vincenty's formulae to determine
                    // the distance between the current location (pos.Coordinate)
                    // and the location of the geofence event (latitudeEvent/longitudeEvent).
                    // If too far apart set eventOfInterest to false to
                    // filter the event out.
                }

                if (eventOfInterest)
                {
                    string geofenceItemEvent = null;
                    int numEventsOfInterest = 0;

                    // Retrieve a vector of state change reports
                    var reports = GeofenceMonitor.Current.ReadReports();

                    foreach (var report in reports)
                    {
                        GeofenceState state = report.NewState;
                        geofenceItemEvent = report.Geofence.Id + " " + formatterLongTime.Format(eventDateTime);

                        if (state == GeofenceState.Removed)
                        {
                            GeofenceRemovalReason reason = report.RemovalReason;
                            if (reason == GeofenceRemovalReason.Expired)
                            {
                                geofenceItemEvent += " (Removed/Expired)";
                            }
                            else if (reason == GeofenceRemovalReason.Used)
                            {
                                geofenceItemEvent += " (Removed/Used)";
                            }
                        }
                        else if (state == GeofenceState.Entered)
                        {
                            geofenceItemEvent += " (Entered)";
                        }
                        else if (state == GeofenceState.Exited)
                        {
                            geofenceItemEvent += " (Exited)";
                        }

                        geofenceItemEvent += $" - {deltaInSeconds}";
                        geofenceItemEvent += $"\nDistance - {distance}";

                        AddGeofenceEvent(geofenceItemEvent);
                        ++numEventsOfInterest;

                        var userLocation = new UserLocation()
                        {
                            Latitude = report.Geoposition.Coordinate.Point.Position.Latitude,
                            Longitude = report.Geoposition.Coordinate.Point.Position.Longitude,
                            Name = report.Geofence.Id,
                            UpdatedUtc = eventDateTime.UtcDateTime,
                            Status = (LocationStatus)(int)state
                        };

                        await UploadDataToCloud(userLocation);
                    }

                    if (eventOfInterest == true && numEventsOfInterest != 0)
                    {
                        SaveExistingEvents();
                        // NOTE: Other notification mechanisms can be used here, such as Badge and/or Tile updates.
                        //DoToast(numEventsOfInterest, geofenceItemEvent);
                    }
                }
            }
        }

        /// <summary>
        /// Helper method to pop a toast
        /// </summary>
        private void DoToast(int numEventsOfInterest, string eventName)
        {
            // pop a toast for each geofence event
            ToastNotifier ToastNotifier = ToastNotificationManager.CreateToastNotifier();

            // Create a two line toast and add audio reminder

            // Here the xml that will be passed to the 
            // ToastNotification for the toast is retrieved
            Windows.Data.Xml.Dom.XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);

            // Set both lines of text
            Windows.Data.Xml.Dom.XmlNodeList toastNodeList = toastXml.GetElementsByTagName("text");
            toastNodeList.Item(0).AppendChild(toastXml.CreateTextNode("Geolocation Sample"));

            if (numEventsOfInterest == 1)
            {
                toastNodeList.Item(1).AppendChild(toastXml.CreateTextNode(eventName));
            }
            else
            {
                string secondLine = "There are " + numEventsOfInterest + " new geofence events";
                toastNodeList.Item(1).AppendChild(toastXml.CreateTextNode(secondLine));
            }

            // now create a xml node for the audio source
            Windows.Data.Xml.Dom.IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
            Windows.Data.Xml.Dom.XmlElement audio = toastXml.CreateElement("audio");
            audio.SetAttribute("src", "ms-winsoundevent:Notification.SMS");

            ToastNotification toast = new ToastNotification(toastXml);
            ToastNotifier.Show(toast);
        }

        private void FillEventCollectionWithExistingEvents()
        {
            var settings = ApplicationData.Current.LocalSettings;
            if (settings.Values.ContainsKey("BackgroundGeofenceEventCollection"))
            {
                var geofenceEvent = settings.Values["BackgroundGeofenceEventCollection"].ToString();

                if (geofenceEvent.Length != 0)
                {
                    var events = JsonValue.Parse(geofenceEvent).GetArray();

                    // NOTE: the events are accessed in reverse order
                    // because the events were added to JSON from
                    // newer to older.  AddGeofenceEvent() adds
                    // each new entry to the beginning of the collection.
                    for (int pos = events.Count - 1; pos >= 0; pos--)
                    {
                        var element = events.GetStringAt((uint)pos);
                        AddGeofenceEvent(element);
                    }
                }
            }
        }

        private void SaveExistingEvents()
        {
            var jsonArray = new JsonArray();

            foreach (var eventDescriptor in _geofenceBackgroundEvents)
            {
                jsonArray.Add(JsonValue.CreateStringValue(eventDescriptor.ToString()));
            }

            string jsonString = jsonArray.Stringify();

            var settings = ApplicationData.Current.LocalSettings;
            settings.Values["BackgroundGeofenceEventCollection"] = jsonString;
        }

        private void AddGeofenceEvent(string eventDescription)
        {
            if (_geofenceBackgroundEvents.Count == maxEventDescriptors)
            {
                _geofenceBackgroundEvents.RemoveAt(maxEventDescriptors - 1);
            }

            _geofenceBackgroundEvents.Insert(0, eventDescription);
        }

        private void TryToInitializeWebClient()
        {
            try
            {
                var settings = SettingsService.Instance;
                var credential = settings.GetCredentialFromLocker();

                if (!string.IsNullOrWhiteSpace(settings.WebHost) && credential != null)
                {
                    webclient = new WebClientService(settings.WebHost, credential.UserName, credential.Password);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async Task UploadDataToCloud(UserLocation userLocation)
        {
            try
            {
                await webclient.Login();
                var id = await webclient.SendUserLocation(userLocation);
                Debug.WriteLine($"Uploaded to web server: {id}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
