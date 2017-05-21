using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouseWebLib.Models
{
    public class TelemetryData
    {
        public long Id { get; set; }

        public decimal Temperature { get; set; }

        public decimal Humidity { get; set; }

        public decimal HeatIndex { get; set; }

        public int GasValue { get; set; }

        [Index("IDX_Date", IsUnique = true)]
        public DateTime CreatedUtc { get; set; }

        //[MaxLength(128)]
        //[Required]
        //[ForeignKey(nameof(User))]
        //public string UserId { get; set; }

        public int RoomId { get; set; }

        //public virtual ApplicationUser User { get; set; }

        public virtual Room Room { get; set; }

    }
}
