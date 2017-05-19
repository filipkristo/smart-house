using SmartHouseWebLib.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartHouseWeb.Models
{
    public class UserLocationDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public LocationStatus Status { get; set; }

        public DateTime UpdatedUtc { get; set; }
    }
}