using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using main_service.EFEntities.Base;
using main_service.EFEntities.Branches;

namespace main_service.EFEntities.Users
{
    [Table(("User"))]
    public class User : BaseEntity
    {
        public string? FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public DateTime? Birthday { get; set; }
        public int RoleId { get; set; }
        public int BranchId { get; set; }

        public virtual UserAuth UserAuth { get; set; }
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }
        
        public virtual ICollection<FcmToken> FcmTokens { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}