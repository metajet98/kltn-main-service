using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace main_service.Databases
{
    [Table("MAINTENANCE_BILL")]
    public partial class MaintenanceBill
    {
        [Key]
        public int Id { get; set; }
        public int? BranchMaintenanceItemPriceId { get; set; }
        public int? Quantity { get; set; }
        public int? TotalPrice { get; set; }
        public int? MaintenanceId { get; set; }

        [ForeignKey(nameof(BranchMaintenanceItemPriceId))]
        [InverseProperty("MaintenanceBill")]
        public virtual BranchMaintenanceItemPrice BranchMaintenanceItemPrice { get; set; }
        [ForeignKey(nameof(MaintenanceId))]
        [InverseProperty("MaintenanceBill")]
        public virtual Maintenance Maintenance { get; set; }
    }
}
