using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouseWebLib.Models
{
    public class UserLocation
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public LocationStatus Status { get; set; }

        public DateTime UpdatedUtc { get; set; }

        [MaxLength(255)]
        public string DeviceInfo { get; set; }

        [MaxLength(128)]
        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
