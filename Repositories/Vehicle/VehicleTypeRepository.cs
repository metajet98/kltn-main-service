using main_service.Databases;
using main_service.Repositories.Base;

namespace main_service.Repositories.Vehicle
{
    public class VehicleTypeRepository : BaseRepository<VehicleType>
    {
        public VehicleTypeRepository(AppDBContext context) : base(context)
        {
        }
    }
}