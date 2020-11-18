using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace main_service.Databases
{
    [Table("MAINTENANCE_SCHEDULE")]
    public partial class MaintenanceSchedule
    {
        [Key]
        public int Id { get; set; }
        public int? UserVehicleId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Date { get; set; }
        public int? Odometer { get; set; }
        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        public string? Content { get; set; }

        [ForeignKey(nameof(UserVehicleId))]
        [InverseProperty("MaintenanceSchedule")]
        public virtual UserVehicle UserVehicle { get; set; }
    }
}
