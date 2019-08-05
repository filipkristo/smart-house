using System;
using System.Collections.Generic;
using SmartHouseAbstraction.DataStore.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHouseDataStore.Entities
{
    public class Command : IEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string DeviceMethod { get; set; }

        [Required]
        [MaxLength(255)]
        public string Description { get; set; }

        public int DeviceId { get; set; }

        [ForeignKey(nameof(DeviceId))]
        public virtual Device Device { get; set; }

        public virtual ICollection<RoutineCommand> RoutineCommands { get; set; }
    }
}
