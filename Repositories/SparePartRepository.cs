using System.Collections.Generic;
using System.Linq;
using main_service.Databases;
using main_service.Repositories.Base;

namespace main_service.Repositories
{
    public class SparePartRepository : BaseRepository<VehicleGroupSparepartItem>
    {
        public SparePartRepository(AppDBContext context) : base(context)
        {
        }

        public List<VehicleGroupSparepartItem> GetByVehicleGroupId(int vehicleGroupId)
        {
            var list = DbSet.Where(x => x.VehicleGroupId.Equals(vehicleGroupId)).ToList();
            return list;
        }
    }
}