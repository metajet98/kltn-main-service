using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace main_service.Databases
{
    [Table("REVIEW")]
    public partial class Review
    {
        public Review()
        {
            Maintenance = new HashSet<Maintenance>();
        }

        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public double Star { get; set; }
        public string? Comment { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }

        [InverseProperty("Review")]
        public virtual ICollection<Maintenance> Maintenance { get; set; }
    }
}
