using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
    public class SongResult
    {
        [JsonProperty("artist")]
        public string Artist { get; set; }

        [JsonProperty("song")]
        public string Song { get; set; }        

        [JsonProperty("album")]
        public string Album { get; set; }

        [JsonProperty("albumUri")]
        public string AlbumUri { get; set; }

        [JsonProperty("genre")]
        public string Genre { get; set; }

        [JsonProperty("loved")]
        public bool Loved { get; set; }

        [JsonProperty("playedSeconds")]
        public int PlayedSeconds { get; set; }

        [JsonProperty("durationSeconds")]
        public int DurationSeconds { get; set; }                
    }
}
