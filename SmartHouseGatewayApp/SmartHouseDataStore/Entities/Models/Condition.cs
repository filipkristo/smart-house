using SmartHouseAbstraction.DataStore.Model;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartHouseDataStore.Entities
{
    public class Condition:IEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string ClrAssembly { get; set; }

        [Required]
        [MaxLength(255)]
        public string ClrType { get; set; }

        [Required]
        [MaxLength(255)]
        public string Description { get; set; }

        public virtual ICollection<RoutineCommand> RoutineCommands { get; set; }
    }
}
