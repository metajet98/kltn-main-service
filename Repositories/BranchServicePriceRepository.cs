using System;
using System.Collections.Generic;
using System.Linq;
using main_service.Databases;
using main_service.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace main_service.Repositories
{
    public class BranchServicePriceRepository : BaseRepository<BranchServicePrice>
    {
        private readonly DbSet<MaintenanceService> _maintenanceServices;

        public BranchServicePriceRepository(AppDBContext context) : base(context)
        {
            _maintenanceServices = context.MaintenanceService;
        }

        public List<BranchServicePrice> GetAllPriceByVehicleGroupId(int vehicleGroupId, int branchId)
        {
            var query = 
                from service in _maintenanceServices.Where(x => x.VehicleGroupId.Equals(vehicleGroupId))
                join branchPrice in DbSet.Where(x => x.BranchId.Equals(branchId)) on service.Id equals branchPrice
                    .MaintenanceServiceId into result
                from x in result.DefaultIfEmpty()
                select new BranchServicePrice
                {
                    Id = x.Id,
                    Branch = x.Branch,
                    BranchId = branchId,
                    CreatedDate = x.CreatedDate,
                    LaborCost = x.LaborCost,
                    MaintenanceService = service,
                    ModifyDate = x.ModifyDate,
                    MaintenanceBillDetail = x.MaintenanceBillDetail,
                    MaintenanceServiceId = x.MaintenanceServiceId,
                    SparePartPrice = x.SparePartPrice
                };
            return query.ToList();
        }

        public bool CheckExist(int id)
        {
            return DbSet.Any(x => x.MaintenanceServiceId.Equals(id));
        }

        public void CreateOrUpdatePrice(int maintenanceServiceId, int newLaborCost, int newSparePartPrice, int branchId)
        {
            var price = DbSet.FirstOrDefault(x => x.MaintenanceServiceId.Equals(maintenanceServiceId));
            if (price != null)
            {
                price.LaborCost = newLaborCost;
                price.SparePartPrice = newSparePartPrice;
                price.ModifyDate = DateTime.Now;
                DbSet.Update(price);
            }
            else
            {
                DbSet.Add(new BranchServicePrice
                {
                    BranchId = branchId,
                    SparePartPrice = newSparePartPrice,
                    LaborCost = newLaborCost,
                    CreatedDate = DateTime.Now,
                    MaintenanceServiceId = maintenanceServiceId
                });
            }
        }
    }
}