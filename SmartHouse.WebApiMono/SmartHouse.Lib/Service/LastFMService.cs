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
		private const string API_KEY = "f6efd05dff7733d006993bdb97bbdb06";
		private const string API_SECRET = "6fe8591b91d15471d484e3842fa96b96";

		private const string USERNAME = "filip_kristo";
		private const string PASSWORD = "itmMI4:rNAoPm";

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

		public async Task<LastTrack> GetInfo(string artistName, string trackName)
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
	}
}
