using main_service.Databases;
using main_service.Repositories.Base;

namespace main_service.Repositories.Vehicle
{
    public class VehicleGroupRepository : BaseRepository<VehicleGroup>
    {
        public VehicleGroupRepository(AppDBContext context) : base(context)
        {
        }
    }
}