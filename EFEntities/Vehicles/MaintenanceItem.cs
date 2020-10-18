using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using main_service.EFEntities.Base;
using main_service.EFEntities.Branches;

namespace main_service.EFEntities.Vehicles
{
    [Table(("MaintenanceItem"))]
    public class MaintenanceItem : BaseEntity
    {
        public string? Description { get; set; }
        public string Name { get; set; }
        public int VehicleGroupId { get; set; }
        
        [ForeignKey("VehicleGroupId")]
        public virtual VehicleGroup VehicleGroup { get; set; }
        
        public virtual ICollection<BrandItemPrice> BrandItemPrices { get; set; }
    }
}