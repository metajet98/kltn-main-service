using main_service.EFEntities.Base;
using main_service.Repositories.Base;

namespace main_service.Repositories.User
{
    public class UserRepository : BaseRepository<EFEntities.Users.User>
    {
        
        public UserRepository(AppDbContext context) : base(context)
        {
        }
    }
}