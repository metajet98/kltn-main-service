using main_service.Databases;
using main_service.Repositories.Base;

namespace main_service.Repositories
{
    public class BranchStaffRepository : BaseRepository<BranchStaff>
    {
        public BranchStaffRepository(AppDBContext context) : base(context)
        {
        }
    }
}