using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SmartHouseDataStoreAbstraction.Models;

namespace SmartHouseDataStore.Entities
{
    public class Device : IEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string CrlAssembly { get; set; }

        [Required]
        [MaxLength(255)]
        public string CrlType { get; set; }

        [Required]
        [MaxLength(255)]
        public string Description { get; set; }

        public int DeviceTypeId { get; set; }

        [ForeignKey(nameof(DeviceTypeId))]
        public DeviceType DeviceType { get; set; }

        public virtual ICollection<DeviceSetting> DeviceSettings { get; set; }

        public virtual ICollection<Command> Commands { get; set; }

        public virtual ICollection<DeviceState> DeviceStates { get; set; }
    }
}
