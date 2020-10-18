using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace main_service.Databases
{
    [Table("USER_AUTH")]
    public partial class UserAuth
    {
        public int Id { get; set; }
        [Key]
        public int UserId { get; set; }
        [Required]
        public string Salt { get; set; }
        [Required]
        public string Hash { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifyDate { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("UserAuth")]
        public virtual User User { get; set; }
    }
}
