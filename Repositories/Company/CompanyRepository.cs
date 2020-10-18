using main_service.EFEntities.Base;
using main_service.EFEntities.Vehicles;
using main_service.Repositories.Base;

namespace main_service.Repositories.Company
{
    public class CompanyRepository : BaseRepository<VehicleCompany>
    {
        public CompanyRepository(AppDbContext context) : base(context)
        {
        }
    }
}