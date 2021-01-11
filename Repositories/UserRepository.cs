using System.Linq;
using main_service.Databases;
using main_service.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace main_service.Repositories
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
        
        public User? GetUserInfo(int userId)
        {
            var query = DbSet.Where(x => x.Id.Equals(userId))
                .Include(x => x.UserAuth)
                .Include(x => x.BranchStaff).ThenInclude(y => y.Branch);
            return query.FirstOrDefault();
        }
    }
}