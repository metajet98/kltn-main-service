using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace main_service.Databases
{
    [Table("BRANCH_STAFF")]
    public partial class BranchStaff
    {
        [Key]
        public int Id { get; set; }
        public int BranchId { get; set; }
        public int StaffId { get; set; }

        [ForeignKey(nameof(BranchId))]
        [InverseProperty("BranchStaff")]
        public virtual Branch Branch { get; set; }
        [ForeignKey(nameof(StaffId))]
        [InverseProperty(nameof(User.BranchStaff))]
        public virtual User Staff { get; set; }
    }
}
