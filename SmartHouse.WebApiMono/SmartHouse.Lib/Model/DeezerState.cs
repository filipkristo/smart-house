using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.Lib.Model
{
    public class DeezerState
    {
        public int Id { get; set; }

        public int TrackPosition { get; set; }

        public string StreamUrl { get; set; }

        public string StreamName { get; set; }

        public DateTime UpdatedUtc { get; set; }

        public int SongId { get; set; }

        public string SongName { get; set; }

        public string AlbumName { get; set; }

        public string AlbumUri { get; set; }

        public string ArtistName { get; set; }

        public int TrackIndex { get; set; }

        public int PlaylistLength { get; set; }

        [JsonProperty("isPlayling")]
        public bool IsPlaying { get; set; }
    }
}
