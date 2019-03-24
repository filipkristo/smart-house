using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
    public class TrackInfoResponse
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public TimeSpan? Duration { get; set; }

        public string Mbid { get; set; }

        public string ArtistName { get; set; }

        public string ArtistMbid { get; set; }

        public Uri Url { get; set; }

        public Uri ImageUrl { get; set; }

        public string AlbumName { get; set; }

        public int? ListenerCount { get; set; }

        public int? PlayCount { get; set; }

        public int? UserPlayCount { get; set; }

        public IReadOnlyList<LastFmTagResponse> TopTags { get; set; }

        public DateTimeOffset? TimePlayed { get; set; }

        public bool? IsLoved { get; set; }

        public bool? IsNowPlaying { get; set; }

        public int? Rank { get; set; }
    }
}
