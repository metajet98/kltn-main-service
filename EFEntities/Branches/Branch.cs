using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using main_service.EFEntities.Base;
using main_service.EFEntities.Users;

namespace main_service.EFEntities.Branches
{
    [Table("Branch")]
    public class Branch : BaseEntity
    {
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        
        public virtual ICollection<BrandItemPrice> BrandItemPrices { get; set; }
        public virtual ICollection<User> Staff { get; set; }
    }
}