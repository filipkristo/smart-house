using SmartHouseDataStoreAbstraction.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHouseDataStore.Entities
{
    public class RoutineCommand : IEntity
    {
        public int Id { get; set; }

        public int RoutineId { get; set; }

        [ForeignKey(nameof(RoutineId))]
        public virtual Routine Routine { get; set; }

        public int CommandId { get; set; }

        [ForeignKey(nameof(CommandId))]
        public virtual Command Command { get; set; }

        [Required]
        [MaxLength(255)]
        public string CommandParameter { get; set; }

        public int ConditionId { get; set; }

        [ForeignKey(nameof(ConditionId))]
        public virtual Condition Condition { get; set; }

        [Required]
        [MaxLength(255)]
        public string ConditionParameter { get; set; }
    }
}
