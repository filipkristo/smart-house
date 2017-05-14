using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Devices.Geolocation.Geofencing;

namespace SmartHouse.UWPLib.BLL
{
    public class GeofenceLocationHelper
    {
        public const string HOME_KEY = "HomeVinodolska";
        public const string WORK_KEY = "WorkZrinjsko";

        private Geofence GenerateGeofence(string key, double latitude, double longitude, double altitude, double radius)
        {
            var calendar = new Windows.Globalization.Calendar();
            calendar.SetToNow();

            BasicGeoposition position;
            position.Latitude = latitude;
            position.Longitude = longitude;
            position.Altitude = altitude;            
            
            Geocircle geocircle = new Geocircle(position, radius);            

            // want to listen for enter geofence, exit geofence and remove geofence events
            // you can select a subset of these event states
            MonitoredGeofenceStates mask = MonitoredGeofenceStates.Entered | MonitoredGeofenceStates.Exited | MonitoredGeofenceStates.Removed;

            var dwellTime = TimeSpan.FromSeconds(5);            

            return new Geofence(key, geocircle, mask, false, dwellTime);
        }

        public Geofence GetHomeLocation()
        {
            return GenerateGeofence(HOME_KEY, 43.5117767433974, 16.4729205173504, 0, 50);
        }

        public Geofence GetWorkLocation()
        {
            return GenerateGeofence(WORK_KEY, 43.5225312, 16.4312707, 0, 50);
        }
    }
}
