using Newtonsoft.Json;
using SmartHouse.Lib;
using SmartHouse.UWPLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System.Profile;

namespace SmartHouse.UWPLib.Service
{
    public class WebClientService
    {
        private const string TokenKey = "Tokek";

        private OAuthResponse oauthResponse;

        private readonly string baseUrl;
        private readonly string username;
        private readonly string password;

        public WebClientService(string baseUrl, string username, string password)
        {
            this.baseUrl = baseUrl;
            this.username = username;
            this.password = password;
        }

        public async Task Login()
        {
            if (!CheckAccessToken())
                await Authenticate();
        }

        private async Task Authenticate()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseUrl);
                
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", username),
                    new KeyValuePair<string, string>("password", password)
                });

                var response = await httpClient.PostAsync("/Token", content);
                var result = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {                    
                    oauthResponse = JsonConvert.DeserializeObject<OAuthResponse>(result);
                    oauthResponse.IssuedUtc = DateTime.UtcNow;

                    var settings = ApplicationData.Current.LocalSettings;
                    var JSON = JsonConvert.SerializeObject(oauthResponse);
                    settings.Values[TokenKey] = JSON;
                }   
                else
                {
                    throw new Exception(result);
                }
            }
        }

        private bool CheckAccessToken()
        {
            var settings = ApplicationData.Current.LocalSettings;

            if (settings.Values.ContainsKey(TokenKey))
            {
                var JSON = settings.Values[TokenKey].ToString();
                var oauth = JsonConvert.DeserializeObject<OAuthResponse>(JSON);

                if (oauth.IssuedUtc.AddSeconds(oauth.ExpireIn) > DateTime.UtcNow)
                {
                    oauthResponse = oauth;
                    return true;
                }   
                else
                {
                    settings.Values.Remove(TokenKey);
                }
            }

            return false;
        }

        public async Task<int> SendUserLocation(UserLocation userLocation)
        {
            userLocation.Name += " - " + AnalyticsInfo.VersionInfo.DeviceFamily;

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", oauthResponse.AccessToken);
                httpClient.BaseAddress = new Uri(baseUrl);

                var JSON = JsonConvert.SerializeObject(userLocation);
                var content = new StringContent(JSON, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("/api/Location/SaveUserLocation", content);
                var result = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<int>(result);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    var settings = ApplicationData.Current.LocalSettings;
                    settings.Values.Remove(TokenKey);
                    return -1;
                }
                else
                {
                    throw new Exception(result);
                }
            }
        }

        public async Task<int> SendTelemetryData(TelemetryData telemetry)
        {
            var telemetryDto = new TelemetryDataDto()
            {
                CreatedUtc = telemetry.Measured,
                GasValue = (int)telemetry.GasValue,
                HeatIndex = telemetry.HeatIndex,
                Humidity = telemetry.Humidity,
                Temperature = telemetry.Temperature                
            };

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", oauthResponse.AccessToken);
                httpClient.BaseAddress = new Uri(baseUrl);

                var JSON = JsonConvert.SerializeObject(telemetryDto);
                var content = new StringContent(JSON, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("/api/Telemetry/SaveTelemetryData", content);
                var result = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<int>(result);
                }
                else if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    var settings = ApplicationData.Current.LocalSettings;
                    settings.Values.Remove(TokenKey);
                    return -1;
                }
                else
                {
                    throw new Exception(result);
                }
            }
        }
    }
}
