using System.Linq;
using main_service.Databases;
using main_service.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace main_service.Repositories.User
{
    public class UserRepository : BaseRepository<Databases.User>
    {
        
        public UserRepository(AppDBContext context) : base(context)
        {
        }
        
        public Databases.User? FindByPhoneNumber(string phoneNumber)
        {
            var user = DbSet.Where(x => x.PhoneNumber.Equals(phoneNumber)).Include(x => x.UserAuth).FirstOrDefault();
            return user;
        }
    }
}