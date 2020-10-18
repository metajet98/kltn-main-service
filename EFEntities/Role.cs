using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using main_service.EFEntities.Base;
using main_service.EFEntities.Users;

namespace main_service.EFEntities
{
    [Table(("Role"))]
    public class Role : BaseEntity
    {
        public string RoleName { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}