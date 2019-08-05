using System;
using System.Collections.Generic;
using System.Text;
using SmartHouseAbstraction.DataStore.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHouseDataStore.Entities
{
    public class SmartHouseState:IEntity
    {
        public int Id { get; set; }

        public int DeviceTypeId { get; set; }

        [ForeignKey(nameof(DeviceTypeId))]
        public virtual DeviceType DeviceType { get; set; }

        [Required]
        public DateTime UpdatedUtc { get; set; }
    }
}
