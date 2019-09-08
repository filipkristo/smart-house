using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SmartHouseDataStoreAbstraction.Models;

namespace SmartHouseDataStore.Entities
{
    public class Routine : IEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string Description { get; set; }

        public virtual ICollection<RoutineCommand> RoutineCommands { get; set; }
    }
}
