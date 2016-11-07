using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHouse.Web.Data
{
    public class TemperatureData
    {
        public int Id { get; set; }
        public decimal Temperature { get; set; }
        public decimal Humidity { get; set; }
        public decimal? HeatIndex { get; set; }
        public DateTime Added { get; set; }
        public int RoomId { get; set; }

        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; }
    }
}
