using System.Linq;
using main_service.Databases;
using main_service.Repositories.Base;

namespace main_service.Repositories
{
    public class UserAuthRepository : BaseRepository<UserAuth>
    {
        public UserAuthRepository(AppDBContext context) : base(context)
        {
        }
        
        public UserAuth FindByUserId(int userId)
        {
            var userAuth = DbSet.FirstOrDefault(x => x.UserId.Equals(userId));
            return userAuth;
        }
    }
}