using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace main_service.Databases
{
    [Table("USER_VEHICLE")]
    public partial class UserVehicle
    {
        public UserVehicle()
        {
            Maintenance = new HashSet<Maintenance>();
            MaintenanceSchedule = new HashSet<MaintenanceSchedule>();
        }

        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int VehicleGroupId { get; set; }
        [StringLength(50)]
        public string ChassisNumber { get; set; }
        [StringLength(50)]
        public string EngineNumber { get; set; }
        [Required]
        [StringLength(20)]
        public string PlateNumber { get; set; }
        [StringLength(20)]
        public string Color { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("UserVehicle")]
        public virtual User User { get; set; }
        [ForeignKey(nameof(VehicleGroupId))]
        [InverseProperty("UserVehicle")]
        public virtual VehicleGroup VehicleGroup { get; set; }
        [InverseProperty("UserVehicle")]
        public virtual ICollection<Maintenance> Maintenance { get; set; }
        [InverseProperty("UserVehicle")]
        public virtual ICollection<MaintenanceSchedule> MaintenanceSchedule { get; set; }
    }
}
