using System.ComponentModel.DataAnnotations.Schema;
using main_service.EFEntities.Base;

namespace main_service.EFEntities.Vehicles
{
    [Table(("CheckingItem"))]
    public class CheckingItem : BaseEntity
    {
        public string? Description { get; set; }
        public int VehicleGroupId { get; set; }
        
        [ForeignKey("VehicleGroupId")]
        public virtual VehicleGroup VehicleGroup { get; set; }
    }
}