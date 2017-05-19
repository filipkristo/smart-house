using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartHouse.Dto.Model
{
    public class UserLocationDto
    {
        public virtual int Id { get; set; }
        
        public virtual string Name { get; set; }

        public virtual double Latitude { get; set; }

        public virtual double Longitude { get; set; }

        public virtual LocationStatus Status { get; set; }

        public virtual DateTime UpdatedUtc { get; set; }

        public virtual string UserId { get; set; }

    }
}
