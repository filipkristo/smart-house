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
        private const string _uri = "http://10.110.166.95/api/SunriseSunset/IsNight";
        private static readonly HttpClient _httpClient;

        static SunriseSunsetService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.ParseAdd("application/json");
        }

        public async Task<bool> IsNight()
        {
            var json = await _httpClient.GetStringAsync(_uri).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<bool>(json);
        }
    }
}
