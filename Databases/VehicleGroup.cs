using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace main_service.Databases
{
    [Table("VEHICLE_GROUP")]
    public partial class VehicleGroup
    {
        public VehicleGroup()
        {
            MaintenanceItem = new HashSet<MaintenanceItem>();
            UserVehicle = new HashSet<UserVehicle>();
            VehicleGroupSparepartItem = new HashSet<VehicleGroupSparepartItem>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int VehicleTypeId { get; set; }
        public int VehicleCompanyId { get; set; }
        public int Capacity { get; set; }
        public string Image { get; set; }

        [ForeignKey(nameof(VehicleCompanyId))]
        [InverseProperty("VehicleGroup")]
        public virtual VehicleCompany VehicleCompany { get; set; }
        [ForeignKey(nameof(VehicleTypeId))]
        [InverseProperty("VehicleGroup")]
        public virtual VehicleType VehicleType { get; set; }
        [InverseProperty("VehicleGroup")]
        public virtual ICollection<MaintenanceItem> MaintenanceItem { get; set; }
        [InverseProperty("VehicleGroup")]
        public virtual ICollection<UserVehicle> UserVehicle { get; set; }
        [InverseProperty("VehicleGroup")]
        public virtual ICollection<VehicleGroupSparepartItem> VehicleGroupSparepartItem { get; set; }
    }
}
