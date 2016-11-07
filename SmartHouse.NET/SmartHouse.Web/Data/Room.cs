using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHouse.Web.Data
{
    public class Room
    {
        public Room()
        {
            TemperatureData = new List<TemperatureData>();
        }

        public int Id { get; set; }

        [MaxLength(255)]
        [Required]
        public string  Name { get; set; }

        [MaxLength(255)]
        [Required]
        public string Description { get; set; }

        [MaxLength(255)]
        [Required]
        public string Picture { get; set; }

        public virtual ICollection<TemperatureData> TemperatureData { get; set; }
    }
}
