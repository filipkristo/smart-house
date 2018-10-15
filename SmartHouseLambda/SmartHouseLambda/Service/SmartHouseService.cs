using Amazon.Lambda.Core;
using SmartHouseLambda.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouseLambda.Service
{
    public class SmartHouseService : ISmartHouseService
    {
        private readonly HttpClient _client;

        public SmartHouseService(string token)
        {
            var url = Environment.GetEnvironmentVariable("SMART_HOUSE_SERVICE_URL");

            _client = new HttpClient();
            _client.BaseAddress = new Uri(url);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public Task TurnOnSmartHouse() => PostAsync("/api/SmartHouse/TurnOn");

        public Task TurnOffSmartHouse() => PostAsync("/api/SmartHouse/TurnOff");

        public Task TurnOnAirConditioner() => PostAsync("/api/SmartHouse/TurnOnAirConditioner");

        public Task TurnOffAirConditioner() => PostAsync("/api/SmartHouse/TurnOffAirConditioner");

        public Task Play() => PostAsync("/api/SmartHouse/Play");

        public Task Pause() => PostAsync("/api/SmartHouse/Pause");

        public Task NextSong() => PostAsync("/api/SmartHouse/NextSong");

        public Task VolumeUp() => PostAsync("/api/SmartHouse/VolumeUp");

        public Task VolumeDown() => PostAsync("/api/SmartHouse/VolumeDown");

        public Task Louder() => PostAsync("/api/SmartHouse/Louder");

        public Task ToLouder() => PostAsync("/api/SmartHouse/ToLouder");

        public Task LoveSong() => PostAsync("/api/SmartHouse/Love");

        public Task SetVolume(int volume) => PostAsync($"/api/SmartHouse/Love?volume={volume}");

        public Task Mute() => PostAsync("/api/SmartHouse/Mute");

        public Task TiredOfThisSong() => PostAsync("/api/SmartHouse/TiredOfThisSong");

        public Task Ban() => PostAsync("/api/SmartHouse/Ban");

        public Task Pandora() => PostAsync("/api/SmartHouse/Pandora");

        public Task TV() => PostAsync("/api/SmartHouse/TV");

        private async Task PostAsync(string url)
        {
            LambdaLogger.Log($"Starting: {nameof(SmartHouseService)} => {nameof(PostAsync)}: '{url}'");

            await _client.PostAsync(url, null).ConfigureAwait(false);

            LambdaLogger.Log($"Ending: {nameof(SmartHouseService)} => {nameof(PostAsync)}: '{url}'");
        }
    }
}
