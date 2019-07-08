using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Http;

namespace SmartHouse.Lib
{
    public class LastFMService : ILastFMService
    {
        private static readonly HttpClient _httpClient;

        static LastFMService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://10.110.166.99/");
            _httpClient.DefaultRequestHeaders.Accept.ParseAdd("application/json");
        }

        public async Task<string> StartScrobble(SongDetails song)
        {
            var payload = JsonConvert.SerializeObject(song);
            var stringContent = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("apiservice/api/LastFm/StartScrobble", stringContent).ConfigureAwait(false);
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                throw new ApplicationException(json);

            return JsonConvert.DeserializeObject<string>(json);
        }

        public async Task<string> UpdateNowPlaying(SongDetails song)
        {
            var payload = JsonConvert.SerializeObject(song);
            var stringContent = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("apiservice/api/LastFm/UpdateNowPlaying", stringContent).ConfigureAwait(false);
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                throw new ApplicationException(json);

            return JsonConvert.DeserializeObject<string>(json);
        }

        public async Task<string> LoveSong(string artistName, string trackName)
        {
            var json = await _httpClient.GetStringAsync($"apiservice/api/LastFm/LoveSong?artistName={Uri.EscapeDataString(artistName)}&songName={Uri.EscapeDataString(trackName)}").ConfigureAwait(false);
            return JsonConvert.DeserializeObject<string>(json);
        }

        public async Task<string> UnloveSong(string artistName, string trackName)
        {
            var json = await _httpClient.GetStringAsync($"apiservice/api/LastFm/UnloveSong?artistName={Uri.EscapeDataString(artistName)}&songName={Uri.EscapeDataString(trackName)}").ConfigureAwait(false);
            return JsonConvert.DeserializeObject<string>(json);
        }

        public async Task<TrackInfoResponse> GetSongInfo(string artistName, string trackName)
        {
            var json = await _httpClient.GetStringAsync($"apiservice/api/LastFm/GetSongInfo?artistName={Uri.EscapeDataString(artistName)}&songName={Uri.EscapeDataString(trackName)}").ConfigureAwait(false);
            return JsonConvert.DeserializeObject<TrackInfoResponse>(json);
        }

        public async Task<IReadOnlyList<TrackInfoResponse>> GetTopTracks()
        {
            var json = await _httpClient.GetStringAsync("apiservice/api/LastFm/GetTopTracks").ConfigureAwait(false);
            return JsonConvert.DeserializeObject<List<TrackInfoResponse>>(json);
        }

        public async Task<AlbumInfoResponse> GetAlbumInfo(string artist, string album)
        {
            var json = await _httpClient.GetStringAsync($"apiservice/api/LastFm/GetAlbumInfo?artist={Uri.EscapeDataString(artist)}&album={Uri.EscapeDataString(album)}").ConfigureAwait(false);
            return JsonConvert.DeserializeObject<AlbumInfoResponse>(json);
        }

        public async Task<ArtistInfoResponse> GetArtistInfo(string artist)
        {
            var json = await _httpClient.GetStringAsync($"apiservice/api/LastFm/GetArtistInfo?artist={Uri.EscapeDataString(artist)}").ConfigureAwait(false);
            return JsonConvert.DeserializeObject<ArtistInfoResponse>(json);
        }

        public async Task<IReadOnlyList<ArtistInfoResponse>> GetSimilarArtist(string artist)
        {
            var json = await _httpClient.GetStringAsync($"apiservice/api/LastFm/GetSimilarArtist?artist={Uri.EscapeDataString(artist)}").ConfigureAwait(false);
            return JsonConvert.DeserializeObject<List<ArtistInfoResponse>>(json);
        }

        public async Task<IReadOnlyList<ArtistTileData>> GetRecentTopArtists()
        {
            var json = await _httpClient.GetStringAsync("apiservice/api/LastFm/GetRecentTopArtists").ConfigureAwait(false);
            return JsonConvert.DeserializeObject<List<ArtistTileData>>(json);
        }
    }
}
