using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
    public class ArtistInfoResponse
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public LastWiki Bio { get; set; }

        public string Mbid { get; set; }

        public Uri Url { get; set; }

        public bool OnTour { get; set; }

        public IReadOnlyList<LastFmTagResponse> Tags { get; set; }

        public IReadOnlyList<ArtistInfoResponse> Similar { get; set; }

        public Uri ImageUrl { get; set; }

        public int? PlayCount { get; set; }

        public LastStats Stats { get; set; }



        public class LastStats
        {
            public int Listeners { get; set; }

            public int Plays { get; set; }
        }

        public class LastWiki
        {
            public DateTimeOffset Published { get; set; }

            public string Summary { get; set; }

            public string Content { get; set; }

            public int YearFormed { get; set; }
        }
    }
}
