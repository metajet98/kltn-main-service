using main_service.Databases;
using main_service.Repositories.Base;

namespace main_service.Repositories
{
    public class BranchRepository : BaseRepository<Branch>
    {
        public BranchRepository(AppDBContext context) : base(context)
        {
        }
    }
}