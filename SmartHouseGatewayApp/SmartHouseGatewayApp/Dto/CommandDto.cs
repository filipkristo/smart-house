using System;
using System.ComponentModel.DataAnnotations;

namespace SmartHouseGatewayApp.Dto
{
    public class CommandDto
    {
        [Required]
        [MaxLength(255)]
        public string CommandName { get; set; }

        [Required]
        [MaxLength(255)]
        public string DeviceName { get; set; }
    }
}
