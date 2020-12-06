using main_service.Databases;
using main_service.Repositories.Base;

namespace main_service.Repositories
{
    public class MaintenanceServiceRepository : BaseRepository<MaintenanceService>
    {
        public MaintenanceServiceRepository(AppDBContext context) : base(context)
        {
        }
    }
}