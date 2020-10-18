using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace main_service.Databases
{
    [Table("MAINTENANCE_ITEM")]
    public partial class MaintenanceItem
    {
        public MaintenanceItem()
        {
            BranchMaintenanceItemPrice = new HashSet<BranchMaintenanceItemPrice>();
        }

        [Key]
        public int Id { get; set; }
        public int VehicleGroupId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public string Description { get; set; }

        [ForeignKey(nameof(VehicleGroupId))]
        [InverseProperty("MaintenanceItem")]
        public virtual VehicleGroup VehicleGroup { get; set; }
        [InverseProperty("MaintenanceItem")]
        public virtual ICollection<BranchMaintenanceItemPrice> BranchMaintenanceItemPrice { get; set; }
    }
}
