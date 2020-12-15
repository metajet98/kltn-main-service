using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace main_service.Databases
{
    [Table("MAINTENANCE_BILL_DETAIL")]
    public partial class MaintenanceBillDetail
    {
        [Key]
        public int Id { get; set; }
        public int BranchServicePriceId { get; set; }
        public int? Quantity { get; set; }
        public int? TotalPrice { get; set; }
        public int MaintenanceId { get; set; }
        public int? SparePartPrice { get; set; }
        public int? LaborCost { get; set; }
        public string? Title { get; set; }

        [ForeignKey(nameof(BranchServicePriceId))]
        [InverseProperty("MaintenanceBillDetail")]
        public virtual BranchServicePrice BranchServicePrice { get; set; }
        [ForeignKey(nameof(MaintenanceId))]
        [InverseProperty("MaintenanceBillDetail")]
        public virtual Maintenance Maintenance { get; set; }
    }
}
