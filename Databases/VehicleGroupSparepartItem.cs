using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace main_service.Databases
{
    [Table("VEHICLE_GROUP_SPAREPART_ITEM")]
    public partial class VehicleGroupSparepartItem
    {
        public VehicleGroupSparepartItem()
        {
            MaintenanceCheck = new HashSet<MaintenanceCheck>();
        }

        [Key]
        public int Id { get; set; }
        public string? Description { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public int? VehicleGroupId { get; set; }

        [ForeignKey(nameof(VehicleGroupId))]
        [InverseProperty("VehicleGroupSparepartItem")]
        public virtual VehicleGroup VehicleGroup { get; set; }
        [InverseProperty("SparePartItem")]
        public virtual ICollection<MaintenanceCheck> MaintenanceCheck { get; set; }
    }
}
