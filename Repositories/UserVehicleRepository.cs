using System.Collections.Generic;
using System.Linq;
using main_service.Databases;
using main_service.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace main_service.Repositories
{
    public class UserVehicleRepository : BaseRepository<UserVehicle>
    {
        public UserVehicleRepository(AppDBContext context) : base(context)
        {
        }
        
        public IEnumerable<UserVehicle> FindByUserId(int userId)
        {
            return DbSet.Where(x => x.UserId == userId).Include(x => x.VehicleGroup).ToList();
        }
    }
}