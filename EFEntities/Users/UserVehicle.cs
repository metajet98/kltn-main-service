using System.ComponentModel.DataAnnotations.Schema;
using main_service.EFEntities.Base;
using main_service.EFEntities.Vehicles;

namespace main_service.EFEntities.Users
{
    [Table("UserVehicle")]
    public class UserVehicle : BaseEntity
    {
        public int UserId { get; set; }
        public int VehicleGroupId { get; set; }
        public string? ChassisNumber { get; set; }
        public string? EngineNumber { get; set; }
        public string? PlateNumber { get; set; }
        public string? Color { get; set; }
        
        [ForeignKey("UserId")] 
        public virtual User User { get; set; }
        [ForeignKey("UserId")] 
        public virtual VehicleGroup VehicleGroup { get; set; }
    }
}