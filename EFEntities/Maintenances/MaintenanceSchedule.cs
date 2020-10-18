using System;
using System.ComponentModel.DataAnnotations.Schema;
using main_service.EFEntities.Base;
using main_service.EFEntities.Users;

namespace main_service.EFEntities.Maintenances
{
    [Table("MaintenanceSchedule")]
    public class MaintenanceSchedule : BaseEntity
    {
        public int UserVehicleId { get; set; }
        public DateTime Date { get; set; }
        public int Odometer { get; set; }
        public string Notes { get; set; }
        public string Title { get; set; }

        [ForeignKey("UserVehicleId")]
        public virtual UserVehicle UserVehicle { get; set; }
    }
}