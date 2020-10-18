using System.ComponentModel.DataAnnotations.Schema;
using main_service.EFEntities.Base;

namespace main_service.EFEntities.Users
{
    [Table("FcmToken")]
    public class FcmToken : BaseEntity
    {
        public int UserId { get; set; }
        public string Token { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}