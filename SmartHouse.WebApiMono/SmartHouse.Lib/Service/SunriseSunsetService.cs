using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
    public class SunriseSunsetService : ISunriseSunsetService
    {
        private const string _uri = "https://api.sunrise-sunset.org/json?lat=43.511821&lng=16.472851300000002";
        private readonly HttpClient _httpClient;

        public SunriseSunsetService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.ParseAdd("application/json");
        }

        public async Task<bool> IsNight()
        {
            var json = await _httpClient.GetStringAsync(_uri).ConfigureAwait(false);
            var result = JsonConvert.DeserializeObject<SunriseSunsetResult>(json);

            var currentTime = DateTime.Now.TimeOfDay;
            return currentTime >= result.Results.AstronomicalTwilightEnd || currentTime < result.Results.Sunrise;
        }
    }
}
