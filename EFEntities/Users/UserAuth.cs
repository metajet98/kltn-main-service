using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using main_service.EFEntities.Base;

namespace main_service.EFEntities.Users
{
    [Table("UserAuth")]
    public class UserAuth : BaseEntity
    {
        public int UserId { get; set; }
        public string Salt { get; set; }
        public string HashPassword { get; set; }
        
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}