using System.ComponentModel.DataAnnotations.Schema;
using main_service.EFEntities.Base;
using main_service.EFEntities.Vehicles;

namespace main_service.EFEntities.Branches
{
    [Table("BrandItemPrice")]
    public class BrandItemPrice : BaseEntity
    {
        public int LaborCost { get; set; }
        public int SparePartPrice { get; set; }
        public int MaintenanceItemId { get; set; }
        public int BranchId { get; set; }

        [ForeignKey("MaintenanceItemId")]
        public virtual MaintenanceItem MaintenanceItem { get; set; }
        [ForeignKey("BranchId")] 
        public virtual Branch Branch { get; set; }
    }
}