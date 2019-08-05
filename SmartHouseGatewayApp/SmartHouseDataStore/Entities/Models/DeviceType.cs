using System;
using System.Collections.Generic;
using System.Text;
using SmartHouseAbstraction.DataStore.Model;
using System.ComponentModel.DataAnnotations;

namespace SmartHouseDataStore.Entities
{
    public class DeviceType:IEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string Description { get; set; }

        public virtual ICollection<SmartHouseState> SmartHouseStates { get; set; }

        public virtual ICollection<Device> Devices { get; set; }
    }
}
