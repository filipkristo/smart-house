using System;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Objects;
using System.Linq;
using System.Collections.Generic;

namespace SmartHouse.Lib
{
	public class LastFMService : ILastFMService
	{
        private const string API_KEY = "XXXX";
        private const string API_SECRET = "XXXX";

        private const string USERNAME = "XXXX";
        private const string PASSWORD = "XXXX";

        public LastFMService()
		{
			
		}

		public async Task<string> StartScrobble(SongDetails song)
		{
			using (var client = new LastfmClient(API_KEY, API_SECRET))
			{
				await client.Auth.GetSessionTokenAsync(USERNAME, PASSWORD);

				var scrobble = new Scrobble(song.Artist, song.Album, song.Song, DateTime.UtcNow);
				var response = await client.Scrobbler.ScrobbleAsync(scrobble);

				return response.Status.ToString();
			}
		}

		public async Task<string> UpdateNowPlaying(SongDetails song)
		{
			using (var client = new LastfmClient(API_KEY, API_SECRET))
			{
				await client.Auth.GetSessionTokenAsync(USERNAME, PASSWORD);

				var scrobble = new Scrobble(song.Artist, song.Album, song.Song, DateTime.UtcNow);
				var response = await client.Track.UpdateNowPlayingAsync(scrobble);

				return response.Status.ToString();
			}
		}

		public async Task<string> LoveSong(string artistName, string trackName)
		{
			using (var client = new LastfmClient(API_KEY, API_SECRET))
			{
				await client.Auth.GetSessionTokenAsync(USERNAME, PASSWORD);

				var response = await client.Track.LoveAsync(trackName, artistName);
				return response.Status.ToString();
			}
		}

        public async Task<string> UnloveSong(string artistName, string trackName)
        {
            using (var client = new LastfmClient(API_KEY, API_SECRET))
            {
                await client.Auth.GetSessionTokenAsync(USERNAME, PASSWORD);

                var response = await client.Track.UnloveAsync(trackName, artistName);
                return response.Status.ToString();
            }
        }

        public async Task<LastTrack> GetSongInfo(string artistName, string trackName)
		{
			using (var client = new LastfmClient(API_KEY, API_SECRET))
			{
				await client.Auth.GetSessionTokenAsync(USERNAME, PASSWORD);

				var response = await client.Track.GetInfoAsync(trackName, artistName, USERNAME);

				return response.Content;
			}
		}

		public async Task<List<LastTrack>> GetTopTracks()
		{
			using (var client = new LastfmClient(API_KEY, API_SECRET))
			{
				await client.Auth.GetSessionTokenAsync(USERNAME, PASSWORD);

				var response = await client.Chart.GetTopTracksAsync(1, 50);
				return response.Content.ToList();
			}
		}

        public async Task<LastAlbum> GetAlbumInfo(string artist, string album)
        {
            using (var client = new LastfmClient(API_KEY, API_SECRET))
            {
                await client.Auth.GetSessionTokenAsync(USERNAME, PASSWORD);

                var response = await client.Album.GetInfoAsync(artist, album, true);
                return response.Content;
            }
        }

        public async Task<LastArtist> GetArtistInfo(string artist)
        {
            using (var client = new LastfmClient(API_KEY, API_SECRET))
            {
                await client.Auth.GetSessionTokenAsync(USERNAME, PASSWORD);

                var response = await client.Artist.GetInfoAsync(artist: artist, autocorrect: true);
                return response.Content;
            }
        }

        public async Task<IEnumerable<LastArtist>> GetSimilarArtist(string artist, int limit = 50)
        {
            using (var client = new LastfmClient(API_KEY, API_SECRET))
            {
                await client.Auth.GetSessionTokenAsync(USERNAME, PASSWORD);

                var response = await client.Artist.GetSimilarAsync(artist, true, limit);
                return response.Content;
            }
        }
    }
}
