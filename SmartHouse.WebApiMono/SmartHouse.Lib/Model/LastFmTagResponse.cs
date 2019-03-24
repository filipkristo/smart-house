using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
    public class LastFmTagResponse
    {
        public string Name { get; set; }

        public Uri Url { get; set; }

        public int? Count { get; set; }

        public string RelatedTo { get; set; }

        public bool? Streamable { get; set; }

        public int? Reach { get; set; }
    }
}
