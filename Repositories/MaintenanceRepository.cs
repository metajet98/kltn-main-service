using System;
using main_service.Databases;

namespace main_service.Repositories
{
    public class MaintenanceRepository
    {
        private readonly AppDBContext _dbContext;
        
        public MaintenanceRepository(AppDBContext context)
        {
            _dbContext = context;
        }

        public void CreateNewMaintenance(Maintenance maintenance)
        {
            
        }
    }
}