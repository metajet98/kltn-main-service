using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using main_service.EFEntities.Base;

namespace main_service.EFEntities.Vehicles
{
    [Table(("VehicleGroup"))]
    public class VehicleGroup : BaseEntity
    {
        public string Name { get; set; }
        public int VehicleCompanyId { get; set; }
        public int VehicleTypeId { get; set; }

        [ForeignKey("VehicleTypeId")]
        public virtual VehicleType VehicleType { get; set; }
        [ForeignKey("VehicleCompanyId")]
        public virtual VehicleCompany VehicleCompany { get; set; }
        
        public virtual ICollection<CheckingItem> CheckingItems { get; set; }
        public virtual ICollection<MaintenanceItem> MaintenanceItems { get; set; }
    }
}