using System;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Objects;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Net;
using IF.Lastfm.Core.Api.Helpers;

namespace SmartHouse.Lib
{
    public class LastFMService : ILastFMService
    {
        private const string API_KEY = "XXX";
        private const string API_SECRET = "XXX";

        private const string USERNAME = "XXX";
        private const string PASSWORD = "XXX";

        private readonly LastfmClient client;

        public LastFMService()
        {
            client = new LastfmClient(API_KEY, API_SECRET);
        }

        private async Task Authenticate(LastfmClient client)
        {
            const string fileName = "last.fm";

            if (!File.Exists(fileName))
            {
                var result = await client.Auth.GetSessionTokenAsync(USERNAME, PASSWORD);

                if (client.Auth.Authenticated)
                {
                    var json = JsonConvert.SerializeObject(client.Auth.UserSession);
                    File.WriteAllText("last.fm", json, Encoding.UTF8);
                }
                else
                {
                    throw new Exception("Last.FM Authenticate problem");
                }
            }
            else
            {
                var json = File.ReadAllText(fileName, Encoding.UTF8);
                var session = JsonConvert.DeserializeObject<LastUserSession>(json);

                client.Auth.LoadSession(session);

                if (!client.Auth.Authenticated)
                {
                    File.Delete(fileName);
                    throw new Exception("Last.FM Authenticate problem");
                }

            }
        }

        public async Task<string> StartScrobble(SongDetails song)
        {
            if (!client.Auth.Authenticated)
                await Authenticate(client);

            var scrobble = new Scrobble(song.Artist, song.Album, song.Song, DateTime.UtcNow);
            var response = await client.Scrobbler.ScrobbleAsync(scrobble);

            return response.Status.ToString();
        }

        /// <summary>
        /// Dependency: https://github.com/filipkristo/LastfmWSClient.git
        /// </summary>
        /// <param name="song"></param>
        /// <returns></returns>
        public string StartScrobbleBash(SongDetails song)
        {
            const string script = "./scrobble";

            var result = BashHelper.ExecBashScript(script, song.Artist, song.Song, song.Album, Environment.NewLine);
            return string.IsNullOrWhiteSpace(result.Error) ? $"StartScrobbleBash - OK: {result.Message}" : $"StartScrobbleBash - FAIL: {result.Error}";
        }

        public async Task<string> UpdateNowPlaying(SongDetails song)
        {
            if (!client.Auth.Authenticated)
                await Authenticate(client);

            var scrobble = new Scrobble(song.Artist, song.Album, song.Song, DateTime.UtcNow);
            var response = await client.Track.UpdateNowPlayingAsync(scrobble);

            return response.Status.ToString();
        }

        public async Task<string> LoveSong(string artistName, string trackName)
        {
            if (!client.Auth.Authenticated)
                await Authenticate(client);

            var response = await client.Track.LoveAsync(trackName, artistName);
            return response.Status.ToString();
        }

        public async Task<string> UnloveSong(string artistName, string trackName)
        {
            if (!client.Auth.Authenticated)
                await Authenticate(client);

            var response = await client.Track.UnloveAsync(trackName, artistName);
            return response.Status.ToString();
        }

        public async Task<LastTrack> GetSongInfo(string artistName, string trackName)
        {
            if (!client.Auth.Authenticated)
                await Authenticate(client);

            var response = await client.Track.GetInfoAsync(trackName, artistName, USERNAME);
            return response.Content;
        }

        public async Task<List<LastTrack>> GetTopTracks()
        {
            if (!client.Auth.Authenticated)
                await Authenticate(client);

            var response = await client.Chart.GetTopTracksAsync(1, 50);
            return response.Content.ToList();
        }

        public async Task<LastAlbum> GetAlbumInfo(string artist, string album)
        {
            if (!client.Auth.Authenticated)
                await Authenticate(client);

            var response = await client.Album.GetInfoAsync(artist, album, true);
            return response.Content;
        }

        public async Task<LastArtist> GetArtistInfo(string artist)
        {
            if (!client.Auth.Authenticated)
                await Authenticate(client);

            var response = await client.Artist.GetInfoAsync(artist: artist, autocorrect: true);
            return response.Content;
        }

        public async Task<IEnumerable<LastArtist>> GetSimilarArtist(string artist, int limit = 50)
        {
            if(!client.Auth.Authenticated)
                await Authenticate(client);

            var response = await client.Artist.GetSimilarAsync(artist, true, limit);
            return response.Content;
        }

        public async Task<IEnumerable<ArtistTileData>> GetRecentTopArtists(DateTimeOffset since, int count)
        {
            var pageNum = 1;
            var scrobblers = new List<LastTrack>();
            IEnumerable<LastTrack> pagedTracks = null;

            do
            {
                pagedTracks = await LoadScrobbles(pageNum, since).ConfigureAwait(false);
                scrobblers.AddRange(pagedTracks);

                pageNum++;

            } while (pagedTracks.Any());

            var artists = scrobblers
                .GroupBy(x => new { Id = x.ArtistMbid, Name = x.ArtistName })
                .OrderByDescending(x => x.Count())
                .Select(x => new ArtistTileData()
                {
                    Id = x.Key.Id,
                    Name = x.Key.Name,
                    Count = x.Count()
                })
                .Take(count)
                .ToList();

            foreach (var artist in artists)
            {
                LastResponse<LastArtist> artistInfo = null;

                if (!string.IsNullOrWhiteSpace(artist.Id))
                    artistInfo = await client.Artist.GetInfoByMbidAsync(artist.Id).ConfigureAwait(false);
                else
                    artistInfo = await client.Artist.GetInfoAsync(artist.Name, autocorrect: true).ConfigureAwait(false);

                artist.ImageUrl = artistInfo?.Content?.MainImage?.Large?.OriginalString ?? artistInfo?.Content?.MainImage?.LastOrDefault(x => !string.IsNullOrWhiteSpace(x?.OriginalString))?.OriginalString;
                artist.Url = artistInfo?.Content?.Url?.OriginalString;
            }

            return artists;
        }

        private async Task<IEnumerable<LastTrack>> LoadScrobbles(int pageNum, DateTimeOffset since)
        {
            var tracks = await client.User.GetRecentScrobbles(USERNAME, DateTime.Today.AddDays(-1), pageNum, 30).ConfigureAwait(false);
            return tracks.Where(x => x.IsNowPlaying != true);
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}
