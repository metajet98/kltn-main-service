using System.ComponentModel.DataAnnotations.Schema;
using main_service.EFEntities.Base;

namespace main_service.EFEntities.Vehicles
{
    [Table(("VehicleCompany"))]
    public class VehicleCompany : BaseEntity
    {
        public string Name { get; set; }
    }
}