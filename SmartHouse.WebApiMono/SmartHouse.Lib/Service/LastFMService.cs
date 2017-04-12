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

namespace SmartHouse.Lib
{
    public class LastFMService : ILastFMService
    {
        private const string API_KEY = "XXX";
        private const string API_SECRET = "XXX";

        private const string USERNAME = "XXX";
        private const string PASSWORD = "XXX";

        public LastFMService()
        {

        }

        private async Task Authenticate(LastfmClient client)
        {            
            var fileName = "last.fm";

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
                var session = JsonConvert.DeserializeObject(json, typeof(LastUserSession)) as LastUserSession;

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
            using (var client = new LastfmClient(API_KEY, API_SECRET))
            {
                await Authenticate(client);

                var scrobble = new Scrobble(song.Artist, song.Album, song.Song, DateTime.UtcNow);
                var response = await client.Scrobbler.ScrobbleAsync(scrobble);

                return response.Status.ToString();
            }
        }

        public async Task<string> UpdateNowPlaying(SongDetails song)
        {
            using (var client = new LastfmClient(API_KEY, API_SECRET))
            {
                await Authenticate(client);

                var scrobble = new Scrobble(song.Artist, song.Album, song.Song, DateTime.UtcNow);
                var response = await client.Track.UpdateNowPlayingAsync(scrobble);

                return response.Status.ToString();
            }
        }

        public async Task<string> LoveSong(string artistName, string trackName)
        {
            using (var client = new LastfmClient(API_KEY, API_SECRET))
            {
                await Authenticate(client);

                var response = await client.Track.LoveAsync(trackName, artistName);
                return response.Status.ToString();
            }
        }

        public async Task<string> UnloveSong(string artistName, string trackName)
        {
            using (var client = new LastfmClient(API_KEY, API_SECRET))
            {
                await Authenticate(client);

                var response = await client.Track.UnloveAsync(trackName, artistName);
                return response.Status.ToString();
            }
        }

        public async Task<LastTrack> GetSongInfo(string artistName, string trackName)
        {
            using (var client = new LastfmClient(API_KEY, API_SECRET))
            {
                await Authenticate(client);

                var response = await client.Track.GetInfoAsync(trackName, artistName, USERNAME);

                return response.Content;
            }
        }

        public async Task<List<LastTrack>> GetTopTracks()
        {
            using (var client = new LastfmClient(API_KEY, API_SECRET))
            {
                await Authenticate(client);

                var response = await client.Chart.GetTopTracksAsync(1, 50);
                return response.Content.ToList();
            }
        }

        public async Task<LastAlbum> GetAlbumInfo(string artist, string album)
        {
            using (var client = new LastfmClient(API_KEY, API_SECRET))
            {
                await Authenticate(client);

                var response = await client.Album.GetInfoAsync(artist, album, true);
                return response.Content;
            }
        }

        public async Task<LastArtist> GetArtistInfo(string artist)
        {
            using (var client = new LastfmClient(API_KEY, API_SECRET))
            {
                await Authenticate(client);

                var response = await client.Artist.GetInfoAsync(artist: artist, autocorrect: true);
                return response.Content;
            }
        }

        public async Task<IEnumerable<LastArtist>> GetSimilarArtist(string artist, int limit = 50)
        {
            using (var client = new LastfmClient(API_KEY, API_SECRET))
            {
                await Authenticate(client);

                var response = await client.Artist.GetSimilarAsync(artist, true, limit);
                return response.Content;
            }
        }
    }
}
