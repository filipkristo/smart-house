using System;
using Newtonsoft.Json;

namespace SmartHouse.Lib
{
	public class PandoraResult
	{
		[JsonProperty("artist")]
		public string Artist { get; set; }

		[JsonProperty("song")]
		public string Song { get; set; }

		[JsonProperty("radio")]
		public string Radio { get; set; }

		[JsonProperty("loved")]
		public bool Loved { get; set; }

		[JsonProperty("albumUri")]
		public string AlbumUri { get; set; }

		[JsonProperty("album")]
		public string Album { get; set; }

		[JsonProperty("durationSeconds")]
		public int DurationSeconds { get; set; }

		[JsonProperty("isPlaying")]
		public bool IsPlaying { get; set; }

		[JsonProperty("lastModifed")]
		public DateTime LastModifed { get; set; }

		public PandoraResult()
		{
		}
	}
}
