using System.ComponentModel.DataAnnotations.Schema;
using main_service.EFEntities.Base;

namespace main_service.EFEntities.Vehicles
{
    [Table(("VehicleType"))]
    public class VehicleType : BaseEntity
    {
        public string Name { get; set; }
    }
}