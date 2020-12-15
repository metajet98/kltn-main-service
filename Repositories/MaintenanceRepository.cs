using System.Linq;
using main_service.Databases;
using main_service.Repositories.Base;
using main_service.RestApi.Requests;

namespace main_service.Repositories
{
    public class MaintenanceRepository : BaseRepository<Maintenance>
    {
        public MaintenanceRepository(AppDBContext context) : base(context)
        {
        }

        public Maintenance GetMaintenance(int maintenanceId)
        {
            var maintenance = Get(x => x.Id.Equals(maintenanceId), includeProperties: "UserVehicle").FirstOrDefault();
            if (maintenance == null) return null;
            var query =
                from checkList
                    in Context.VehicleGroupSparepartItem
                        .Where(x => x.VehicleGroupId.Equals(maintenance.UserVehicle.VehicleGroupId))
                join statusDetails
                    in Context.SparepartCheckDetail.Where(x => x.MaintenanceId.Equals(maintenanceId))
                    on checkList.Id equals statusDetails.SparePartItemId
                    into result
                from x in result.DefaultIfEmpty()
                select new SparepartCheckDetail
                {
                    Id = x.Id,
                    Status = x.Status,
                    MaintenanceId = maintenanceId,
                    StatusId = x.StatusId,
                    SparePartItemId = checkList.Id,
                    SparePartItem = checkList,
                    Maintenance = maintenance
                };
            var sparePartStatusDetail = query.ToList();
            maintenance.SparepartCheckDetail = sparePartStatusDetail;
            return maintenance;
        }

        public bool InsertMaintenanceCheck(int statusId, int vehicleSparePartId, int maintenanceId)
        {
            var maintenance = Get(x => x.Id.Equals(maintenanceId), includeProperties: "SparepartCheckDetail")
                .FirstOrDefault();
            if (maintenance == null) return false;

            var sparePartCheckDetail =
                maintenance.SparepartCheckDetail.FirstOrDefault(x => x.SparePartItemId.Equals(vehicleSparePartId));
            if (sparePartCheckDetail != null)
            {
                sparePartCheckDetail.StatusId = statusId;
                Context.SparepartCheckDetail.Update(sparePartCheckDetail);
                Context.SaveChanges();
                return true;
            }
            else
            {
                Context.SparepartCheckDetail.Add(new SparepartCheckDetail
                {
                    MaintenanceId = maintenanceId,
                    StatusId = statusId,
                    SparePartItemId = vehicleSparePartId
                });
                Context.SaveChanges();
                return true;
            }
        }
        
        public bool InsertMaintenanceBill(MaintenanceBillRequest request, int maintenanceId)
        {
            var maintenance = Get(x => x.Id.Equals(maintenanceId), includeProperties: "MaintenanceBillDetail").FirstOrDefault();
            if (maintenance == null) return false;
            var servicePrice =
                Context.BranchServicePrice.FirstOrDefault(x => x.Id.Equals(request.BranchServicePriceId));
            if (servicePrice == null) return false;
            var item = maintenance.MaintenanceBillDetail.FirstOrDefault(x =>
                x.BranchServicePriceId.Equals(request.BranchServicePriceId));
            if (item == null)
            {
                Context.MaintenanceBillDetail.Add(new MaintenanceBillDetail
                {
                    Quantity = request.Quantity,
                    MaintenanceId = maintenanceId,
                    TotalPrice = request.Quantity * (servicePrice.LaborCost + servicePrice.SparePartPrice),
                    BranchServicePriceId = request.BranchServicePriceId,
                });
            }
            else
            {
                item.Quantity = request.Quantity;
                item.TotalPrice = request.Quantity * (servicePrice.LaborCost + servicePrice.SparePartPrice);
                Context.MaintenanceBillDetail.Update(item);
            }
            Context.SaveChanges();
            return true;
        }
    }
}