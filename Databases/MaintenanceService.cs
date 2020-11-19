using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace main_service.Databases
{
    [Table("MAINTENANCE_SERVICE")]
    public partial class MaintenanceService
    {
        public MaintenanceService()
        {
            BranchServicePrice = new HashSet<BranchServicePrice>();
        }

        [Key]
        public int Id { get; set; }
        public int VehicleGroupId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public string? Description { get; set; }

        [ForeignKey(nameof(VehicleGroupId))]
        [InverseProperty("MaintenanceService")]
        public virtual VehicleGroup VehicleGroup { get; set; }
        [InverseProperty("MaintenanceService")]
        public virtual ICollection<BranchServicePrice> BranchServicePrice { get; set; }
    }
}
