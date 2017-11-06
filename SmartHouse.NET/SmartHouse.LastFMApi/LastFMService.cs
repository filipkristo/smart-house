using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using SmartHouse.LastFMApi.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace SmartHouse.LastFMApi
{
    public class LastFMService : IDisposable
    {
        private const string LastFMSessionKey = "LastFMSession";
        private readonly LastfmClient client;

        private const string API_KEY = "XXX";
        private const string API_SECRET = "XXX";

        private const string USERNAME = "XXX";
        private const string PASSWORD = "XXX";

        public LastFMService()
        {
            client = new LastfmClient(API_KEY, API_SECRET);
        }

        public async Task Login()
        {
            var sessionExists = SessionExists();

            if(sessionExists)
            {
                var userSession = LoadSession();
                client.Auth.LoadSession(userSession);
            }
            else
            {
                var response = await client.Auth.GetSessionTokenAsync(USERNAME, PASSWORD);

                if(client.Auth.Authenticated)
                {
                    var session = client.Auth.UserSession;
                    SaveSession(session);
                }
            }
        }

        public async Task<IEnumerable<ArtistTileData>> RecentTopArtists()
        {
            var pageNum = 1;
            var scrobblers = new List<LastTrack>();
            IEnumerable<LastTrack> pagedTracks = null;

            do
            {
                pagedTracks = await LoadScrobbles(pageNum);
                scrobblers.AddRange(pagedTracks);

                pageNum++;

            } while (pagedTracks.Any());

            scrobblers.ForEach(x => Debug.WriteLine(x.ArtistName));

            var artists = scrobblers
                .GroupBy(x => new { Id = x.ArtistMbid, Name = x.ArtistName })
                .OrderByDescending(x => x.Count())
                .Select(x => new ArtistTileData()
                {
                    Id = x.Key.Id,
                    Name = x.Key.Name,
                    Count = x.Count()
                })
                .Take(5)
                .ToList();

            foreach (var item in artists)
            {
                LastResponse<LastArtist> artistInfo = null;

                if (!string.IsNullOrWhiteSpace(item.Id))
                    artistInfo = await client.Artist.GetInfoByMbidAsync(item.Id);
                else
                    artistInfo = await client.Artist.GetInfoAsync(item.Name, autocorrect: true);

                item.Url = artistInfo.Content?.MainImage?.Large?.OriginalString ?? artistInfo.Content?.MainImage?.LastOrDefault(x => !string.IsNullOrWhiteSpace(x?.OriginalString))?.OriginalString;
            }

            Debug.WriteLine("TOP ARTISTS: ");
            artists.ForEach(x => Debug.WriteLine($"Artist: {x.Name} Count: {x.Count}"));

            return artists;
        }

        private async Task<IEnumerable<LastTrack>> LoadScrobbles(int page)
        {
            var tracks = await client.User.GetRecentScrobbles(USERNAME, DateTime.Today.AddDays(-1), page, 20);
            return tracks.Where(x => !(x.IsNowPlaying == true));
        }

        private bool SessionExists()
        {
            var settings = ApplicationData.Current.LocalSettings;
            return settings.Values.ContainsKey(LastFMSessionKey);
        }

        private void SaveSession(LastUserSession session)
        {
            var json = JsonConvert.SerializeObject(session);

            var settings = ApplicationData.Current.LocalSettings;
            settings.Values[LastFMSessionKey] = json;
        }

        private LastUserSession LoadSession()
        {
            var settings = ApplicationData.Current.LocalSettings;
            var sessionJson = settings.Values[LastFMSessionKey] as string;

            return JsonConvert.DeserializeObject<LastUserSession>(sessionJson);
        }

        private void RemoveSession()
        {
            var settings = ApplicationData.Current.LocalSettings;
            settings.Values.Remove(LastFMSessionKey);
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}
