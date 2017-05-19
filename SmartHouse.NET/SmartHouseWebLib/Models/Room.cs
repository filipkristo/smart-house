using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouseWebLib.Models
{
    public class Room
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string ImageUrl { get; set; }

        public int HouseId { get; set; }
        public virtual House House { get; set; }

    }
}
