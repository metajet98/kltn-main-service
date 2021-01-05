using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace main_service.Databases
{
    [Table("REVIEW")]
    public partial class Review
    {
        [Key]
        public int Id { get; set; }
        public int? MaintenanceId { get; set; }
        public int UserId { get; set; }
        public double Star { get; set; }
        public string? Comment { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }

        [ForeignKey(nameof(MaintenanceId))]
        [InverseProperty("Review")]
        public virtual Maintenance Maintenance { get; set; }
    }
}
