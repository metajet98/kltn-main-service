using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace main_service.Databases
{
    [Table("BRANCH")]
    public partial class Branch
    {
        public Branch()
        {
            BranchMaintenanceItemPrice = new HashSet<BranchMaintenanceItemPrice>();
            BranchStaff = new HashSet<BranchStaff>();
            Maintenance = new HashSet<Maintenance>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(30)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string Address { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifyDate { get; set; }
        public string? Logo { get; set; }

        [InverseProperty("Branch")]
        public virtual ICollection<BranchMaintenanceItemPrice> BranchMaintenanceItemPrice { get; set; }
        [InverseProperty("Branch")]
        public virtual ICollection<BranchStaff> BranchStaff { get; set; }
        [InverseProperty("Branch")]
        public virtual ICollection<Maintenance> Maintenance { get; set; }
    }
}
