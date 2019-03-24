using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
    public class AlbumInfoResponse
    {
        public string Name { get; set; }

        public IReadOnlyList<TrackInfoResponse> Tracks { get; set; }

        public string ArtistName { get; set; }

        public DateTimeOffset? ReleaseDateUtc { get; set; }

        public int? ListenerCount { get; set; }

        public int? PlayCount { get; set; }

        public string Mbid { get; set; }

        public Uri Url { get; set; }

        public Uri ImageUrl { get; set; }

        public IReadOnlyList<LastFmTagResponse> TopTags { get; set; }
    }
}
