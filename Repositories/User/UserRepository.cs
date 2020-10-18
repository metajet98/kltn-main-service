using main_service.Databases;
using main_service.Repositories.Base;

namespace main_service.Repositories.User
{
    public class UserRepository : BaseRepository<Databases.User>
    {
        
        public UserRepository(AppDBContext context) : base(context)
        {
        }
    }
}