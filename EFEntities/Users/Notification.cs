using System.ComponentModel.DataAnnotations.Schema;
using main_service.EFEntities.Base;

namespace main_service.EFEntities.Users
{
    [Table("Notification")]
    public class Notification : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Activity { get; set; }
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}