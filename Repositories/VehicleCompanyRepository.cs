using main_service.Databases;
using main_service.Repositories.Base;

namespace main_service.Repositories
{
    public class VehicleCompanyRepository : BaseRepository<VehicleCompany>
    {
        public VehicleCompanyRepository(AppDBContext context) : base(context)
        {
        }
    }
}