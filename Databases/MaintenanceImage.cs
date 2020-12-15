using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace main_service.Databases
{
    [Table("MAINTENANCE_IMAGE")]
    public partial class MaintenanceImage
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        public int MaintenanceId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }

        [ForeignKey(nameof(MaintenanceId))]
        [InverseProperty("MaintenanceImage")]
        public virtual Maintenance Maintenance { get; set; }
    }
}
