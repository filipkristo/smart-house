using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using SmartHouseDataStoreAbstraction.Models;

namespace SmartHouseDataStore.Entities
{
    public class Condition : IEntity
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
