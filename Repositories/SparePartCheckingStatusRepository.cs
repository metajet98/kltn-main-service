using main_service.Databases;
using main_service.Repositories.Base;

namespace main_service.Repositories
{
    public class SparePartCheckingStatusRepository : BaseRepository<SparepartStatus>
    {
        public SparePartCheckingStatusRepository(AppDBContext context) : base(context)
        {
        }
    }
}