using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace main_service.Databases
{
    [Table("CUSTOMER_CALENDER")]
    public partial class CustomerCalender
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Notes { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Time { get; set; }
        public int BranchId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }

        [ForeignKey(nameof(BranchId))]
        [InverseProperty("CustomerCalender")]
        public virtual Branch Branch { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("CustomerCalender")]
        public virtual User User { get; set; }
    }
}
