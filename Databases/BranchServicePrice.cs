using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace main_service.Databases
{
    [Table("BRANCH_SERVICE_PRICE")]
    public partial class BranchServicePrice
    {
        public BranchServicePrice()
        {
            MaintenanceBillDetail = new HashSet<MaintenanceBillDetail>();
        }

        [Key]
        public int Id { get; set; }
        public int BranchId { get; set; }
        public int MaintenanceServiceId { get; set; }
        public int? LaborCost { get; set; }
        public int? SparePartPrice { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifyDate { get; set; }

        [ForeignKey(nameof(BranchId))]
        [InverseProperty("BranchServicePrice")]
        public virtual Branch Branch { get; set; }
        [ForeignKey(nameof(MaintenanceServiceId))]
        [InverseProperty("BranchServicePrice")]
        public virtual MaintenanceService MaintenanceService { get; set; }
        [InverseProperty("BranchServicePrice")]
        public virtual ICollection<MaintenanceBillDetail> MaintenanceBillDetail { get; set; }
    }
}
