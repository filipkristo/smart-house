using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
    public class PhoneCallData
    {
        [Required]
        public string Id { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Started { get; set; }
        public string State { get; set; }
    }
}
