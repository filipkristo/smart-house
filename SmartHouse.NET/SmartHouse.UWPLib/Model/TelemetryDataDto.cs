using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.UWPLib.Model
{
    public class TelemetryDataDto
    {
        public long Id { get; set; }

        public decimal Temperature { get; set; }

        public decimal Humidity { get; set; }

        public decimal HeatIndex { get; set; }

        public int GasValue { get; set; }

        public DateTime CreatedUtc { get; set; }
        
        public string UserId { get; set; }

        public int RoomId { get; set; }
    }
}
