using main_service.Databases;
using main_service.Repositories.Base;

namespace main_service.Repositories.Company
{
    public class CompanyRepository : BaseRepository<VehicleCompany>
    {
        public CompanyRepository(AppDBContext context) : base(context)
        {
        }
    }
}