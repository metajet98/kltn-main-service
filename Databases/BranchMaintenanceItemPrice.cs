using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace main_service.Databases
{
    [Table("BRANCH_MAINTENANCE_ITEM_PRICE")]
    public partial class BranchMaintenanceItemPrice
    {
        public BranchMaintenanceItemPrice()
        {
            MaintenanceBill = new HashSet<MaintenanceBill>();
        }

        [Key]
        public int Id { get; set; }
        public int BranchId { get; set; }
        public int MaintenanceItemId { get; set; }
        public int LaborCost { get; set; }
        public int SparePartPrice { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifyDate { get; set; }

        [ForeignKey(nameof(BranchId))]
        [InverseProperty("BranchMaintenanceItemPrice")]
        public virtual Branch Branch { get; set; }
        [ForeignKey(nameof(MaintenanceItemId))]
        [InverseProperty("BranchMaintenanceItemPrice")]
        public virtual MaintenanceItem MaintenanceItem { get; set; }
        [InverseProperty("BranchMaintenanceItemPrice")]
        public virtual ICollection<MaintenanceBill> MaintenanceBill { get; set; }
    }
}
