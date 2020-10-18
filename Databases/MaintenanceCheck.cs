using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace main_service.Databases
{
    [Table("MAINTENANCE_CHECK")]
    public partial class MaintenanceCheck
    {
        [Key]
        public int Id { get; set; }
        public int MaintenanceId { get; set; }
        public int StatusId { get; set; }
        public int SparePartItemId { get; set; }

        [ForeignKey(nameof(MaintenanceId))]
        [InverseProperty("MaintenanceCheck")]
        public virtual Maintenance Maintenance { get; set; }
        [ForeignKey(nameof(SparePartItemId))]
        [InverseProperty(nameof(VehicleGroupSparepartItem.MaintenanceCheck))]
        public virtual VehicleGroupSparepartItem SparePartItem { get; set; }
        [ForeignKey(nameof(StatusId))]
        [InverseProperty(nameof(SparepartStatus.MaintenanceCheck))]
        public virtual SparepartStatus Status { get; set; }
    }
}
