using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using main_service.EFEntities.Base;
using main_service.EFEntities.Users;

namespace main_service.EFEntities.Maintenances
{
    [Table("Maintenance")]
    public class Maintenance : BaseEntity
    {
        public string? Notes { get; set; }
        public int Odometer { get; set; }
        public int UserVehicleId { get; set; }
        public int ReceptionStaffId { get; set; }
        public int MaintenanceStaffId { get; set; }

        [ForeignKey("UserVehicleId")]
        public virtual UserVehicle UserVehicle { get; set; }
        [ForeignKey("ReceptionStaffId")]
        public virtual User ReceptionStaff { get; set; }
        [ForeignKey("MaintenanceStaffId")]
        public virtual User MaintenanceStaff { get; set; }

        public virtual IEnumerable<MaintenanceImage> MaintenanceImages { get; set; }
    }
}