using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
    public class SunriseSunsetData
    {
        [JsonProperty("sunrise")]
        [JsonConverter(typeof(TimeSpanJsonConverter))]
        public TimeSpan Sunrise { get; set; }

        [JsonProperty("sunset")]
        [JsonConverter(typeof(TimeSpanJsonConverter))]
        public TimeSpan Sunset { get; set; }

        [JsonProperty("solar_noon")]
        [JsonConverter(typeof(TimeSpanJsonConverter))]
        public TimeSpan SolarNoon { get; set; }

        [JsonProperty("day_length")]
        [JsonConverter(typeof(TimeSpanJsonConverter))]
        public TimeSpan DayLength { get; set; }

        [JsonProperty("civil_twilight_begin")]
        [JsonConverter(typeof(TimeSpanJsonConverter))]
        public TimeSpan CivilTwilightBegin { get; set; }

        [JsonProperty("civil_twilight_end")]
        [JsonConverter(typeof(TimeSpanJsonConverter))]
        public TimeSpan CivilTwilightEnd { get; set; }

        [JsonProperty("nautical_twilight_begin")]
        [JsonConverter(typeof(TimeSpanJsonConverter))]
        public TimeSpan NauticalTwilightBegin { get; set; }

        [JsonProperty("nautical_twilight_end")]
        [JsonConverter(typeof(TimeSpanJsonConverter))]
        public TimeSpan NauticalTwilightEnd { get; set; }

        [JsonProperty("astronomical_twilight_begin")]
        [JsonConverter(typeof(TimeSpanJsonConverter))]
        public TimeSpan AstronomicalTwilightBegin { get; set; }

        [JsonProperty("astronomical_twilight_end")]
        [JsonConverter(typeof(TimeSpanJsonConverter))]
        public TimeSpan AstronomicalTwilightEnd { get; set; }
    }
}
