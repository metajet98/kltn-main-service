using System;
using System.Collections.Generic;
using System.Linq;
using main_service.Constants;
using main_service.Databases;
using main_service.Repositories.Base;
using main_service.RestApi.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace main_service.Repositories
{
    public class MaintenanceRepository : BaseRepository<Maintenance>
    {
        public MaintenanceRepository(AppDBContext context) : base(context)
        {
        }

        public Maintenance GetMaintenanceAllDetail(int maintenanceId)
        {
            var maintenance = Get(x => x.Id.Equals(maintenanceId),
                    includeProperties: "UserVehicle,MaintenanceBillDetail,Branch,ReceptionStaff,MaintenanceStaff")
                .FirstOrDefault();
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

        public Maintenance GetMaintenance(int maintenanceId)
        {
            var query = DbSet.Where(x => x.Id.Equals(maintenanceId))
                .Include(x => x.UserVehicle).ThenInclude(y => y.User)
                .Include(x => x.SparepartCheckDetail).ThenInclude(y => y.Status)
                .Include(x => x.SparepartCheckDetail).ThenInclude(y => y.SparePartItem);
            
            query.Select(x => x.UserVehicle).Load();
            query.Select(x => x.MaintenanceBillDetail).Load();
            query.Select(x => x.Branch).Load();
            query.Select(x => x.Review).Load();
            query.Select(x => x.ReceptionStaff).Load();
            query.Select(x => x.MaintenanceImage).Load();
            query.Select(x => x.MaintenanceStaff).Load();
            query.Select(x => x.MaintenanceSchedule).Load();
            
            // var result = DbSet
            //     .Where(x => x.Id.Equals(maintenanceId))
            //     .Include(x => x.UserVehicle)
            //     .Include(x => x.MaintenanceBillDetail)
            //     .Include(x => x.Branch)
            //     .Include(x => x.Review)
            //     .Include(x => x.UserVehicle).ThenInclude(y => y.User)
            //     .Include(x => x.ReceptionStaff)
            //     .Include(x => x.MaintenanceImage)
            //     .Include(x => x.MaintenanceStaff)
            //     .Include(x => x.MaintenanceSchedule)
            //     .Include(x => x.SparepartCheckDetail).ThenInclude(y => y.Status)
            //     .Include(x => x.SparepartCheckDetail).ThenInclude(y => y.SparePartItem)
            //     .FirstOrDefault();
            return query.FirstOrDefault();
        }

        public Maintenance GetMaintenanceForPdf(int maintenanceId)
        {
            var result = DbSet
                .Include(x => x.UserVehicle).ThenInclude(y => y.VehicleGroup)
                .Include(x => x.UserVehicle).ThenInclude(y => y.User)
                .Include("MaintenanceBillDetail.BranchServicePrice.MaintenanceService")
                .Include(x => x.Branch)
                .Include(x => x.SparepartCheckDetail).ThenInclude(y => y.Status)
                .Include(x => x.SparepartCheckDetail).ThenInclude(y => y.SparePartItem)
                .FirstOrDefault(x => x.Id.Equals(maintenanceId));
            return result;
        }

        public IEnumerable<Maintenance> Query(int? userVehicleId, int? staffId, int? branchId, DateTime? date)
        {
            var query = DbSet.AsQueryable();
            if (userVehicleId != null)
            {
                query = query.Where(x => x.UserVehicleId.Equals(userVehicleId));
            }

            if (staffId != null)
            {
                query = query.Where(x => x.MaintenanceStaffId.Equals(staffId) || x.ReceptionStaffId.Equals(staffId));
            }

            if (branchId != null)
            {
                query = query.Where(x => x.BranchId.Equals(branchId));
            }

            if (date != null)
            {
                query = query.Where(x => x.CreatedDate.Value.Year == date.Value.Year
                                         && x.CreatedDate.Value.Month == date.Value.Month
                                         && x.CreatedDate.Value.Day == date.Value.Day);
            }

            query = query.OrderByDescending(x => x.CreatedDate);

            query = query.Include(x => x.Branch);
            query = query.Include(x => x.UserVehicle);
            return query.ToList();
        }

        public bool InsertMaintenanceChecks(int maintenanceId, List<SparePartMaintenanceCheck> listCheck)
        {
            try
            {
                var maintenance = Get(x => x.Id.Equals(maintenanceId), includeProperties: "SparepartCheckDetail")
                    .FirstOrDefault();
                if (maintenance == null) return false;

                var sparePartDetails = maintenance.SparepartCheckDetail;

                foreach (var sparePartMaintenanceCheck in listCheck)
                {
                    if (sparePartDetails.Any(
                        x => x.SparePartItemId.Equals(sparePartMaintenanceCheck.VehicleSparePartId)))
                    {
                        var old = Context.SparepartCheckDetail.FirstOrDefault(x =>
                            x.MaintenanceId.Equals(maintenanceId) &&
                            x.SparePartItemId.Equals(sparePartMaintenanceCheck.VehicleSparePartId));
                        old.StatusId = sparePartMaintenanceCheck.StatusId;
                        Context.SparepartCheckDetail.Update(old);
                    }
                    else
                    {
                        Context.SparepartCheckDetail.Add(new SparepartCheckDetail
                        {
                            StatusId = sparePartMaintenanceCheck.StatusId,
                            SparePartItemId = sparePartMaintenanceCheck.VehicleSparePartId,
                            MaintenanceId = maintenanceId,
                        });
                    }
                }

                Context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool InsertMaintenanceBill(MaintenanceBillRequest request, int maintenanceId)
        {
            var maintenance = Get(x => x.Id.Equals(maintenanceId), includeProperties: "MaintenanceBillDetail")
                .FirstOrDefault();
            if (maintenance == null) return false;
            var servicePrice =
                Context.BranchServicePrice.Include(x => x.MaintenanceService)
                    .FirstOrDefault(x => x.Id.Equals(request.BranchServicePriceId));
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
                    Title = servicePrice.MaintenanceService.Name,
                    LaborCost = servicePrice.LaborCost ?? 0,
                    SparePartPrice = servicePrice.SparePartPrice ?? 0
                });
            }
            else
            {
                if (request.Quantity > 0)
                {
                    item.Quantity = request.Quantity;
                    item.TotalPrice = request.Quantity * (servicePrice.LaborCost + servicePrice.SparePartPrice);
                    Context.MaintenanceBillDetail.Update(item);
                }
                else
                {
                    Context.MaintenanceBillDetail.Remove(item);
                }
            }

            Context.SaveChanges();
            return true;
        }

        public bool InsertMaintenanceImages(int maintenanceId, List<string> images)
        {
            Context.MaintenanceImage.AddRange(images.Select(image => new MaintenanceImage
            {
                MaintenanceId = maintenanceId,
                ImageUrl = image,
                CreatedDate = DateTime.Now
            }));
            Context.SaveChanges();
            return true;
        }

        public bool DeleteMaintenanceImage(int imageId)
        {
            var image = Context.MaintenanceImage.FirstOrDefault(x => x.Id.Equals(imageId));
            if (image == null) return false;
            Context.MaintenanceImage.Remove(image);
            Context.SaveChanges();
            return true;
        }

        public bool AddMaintenanceImage(int maintenanceId, string imageUrl)
        {
            Context.MaintenanceImage.Add(new MaintenanceImage
            {
                MaintenanceId = maintenanceId,
                ImageUrl = imageUrl,
                CreatedDate = DateTime.Now
            });
            Context.SaveChanges();
            return true;
        }

        public bool UpdateMaintainer(int maintenanceId, int maintainerId)
        {
            var maintenance = DbSet.FirstOrDefault(x => x.Id.Equals(maintenanceId));
            if (maintenance == null) return false;
            switch (maintenance.Status)
            {
                case MaintenanceStatus.UnderMaintenance:
                    return true;
                case MaintenanceStatus.Created:
                    maintenance.MaintenanceStaffId = maintainerId;
                    maintenance.Status = MaintenanceStatus.UnderMaintenance;
                    Context.SaveChanges();
                    return true;
                default:
                    return false;
            }
        }

        public bool FinishMaintenance(int maintenanceId)
        {
            try
            {
                var maintenance = DbSet
                    .Include(x => x.MaintenanceBillDetail)
                    .FirstOrDefault(x => x.Id.Equals(maintenanceId));
                if (maintenance == null) return false;
                maintenance.Status = MaintenanceStatus.Finish;

                var billDetails = Context.MaintenanceBillDetail
                    .Include(x => x.BranchServicePrice)
                    .ThenInclude(y => y.MaintenanceService)
                    .Where(x => x.MaintenanceId.Equals(maintenanceId))
                    .ToList();

                billDetails.ForEach(x =>
                {
                    if (x.BranchServicePrice.MaintenanceService.WarrantyOdo != null ||
                        x.BranchServicePrice.MaintenanceService.WarrantyPeriod != null)
                    {
                        DateTime? date = null;
                        int? odo = null;
                        if (x.BranchServicePrice.MaintenanceService.WarrantyPeriod != null)
                        {
                            date = DateTime.Now.AddMonths(x.BranchServicePrice.MaintenanceService.WarrantyPeriod.Value);
                        }

                        if (maintenance.Odometer != null && x.BranchServicePrice.MaintenanceService.WarrantyOdo != null)
                        {
                            odo = maintenance.Odometer + x.BranchServicePrice.MaintenanceService.WarrantyOdo;
                        }

                        Context.MaintenanceSchedule.Add(new MaintenanceSchedule
                        {
                            MaintenanceId = maintenanceId,
                            UserVehicleId = maintenance.UserVehicleId,
                            Date = date,
                            Odometer = odo,
                            Title = "Bảo hành " + x.BranchServicePrice.MaintenanceService.Name,
                            Content = "Vui lòng mang xe ra chi nhánh để kiểm tra khi hết hạn bảo hành"
                        });
                    }
                });

                Context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool InsertSchedule(int maintenanceId, MaintenanceScheduleRequest request)
        {
            try
            {
                var maintenance = DbSet.FirstOrDefault(x => x.Id.Equals(maintenanceId));
                if (maintenance == null) return false;

                Context.MaintenanceSchedule.Add(new MaintenanceSchedule
                {
                    Content = request.Content,
                    Date = request.Date,
                    MaintenanceId = maintenanceId,
                    Odometer = request.Odometer,
                    Title = request.Title,
                    UserVehicleId = maintenance.UserVehicleId
                });
                Context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool InsertReview(int maintenanceId, ReviewRequest request, int userId)
        {
            try
            {
                var maintenance = DbSet
                    .Include(x => x.UserVehicle)
                    .FirstOrDefault(x => x.Id.Equals(maintenanceId));
                if (maintenance?.UserVehicle?.UserId != userId) return false;
                if (maintenance.ReviewId != null)
                {
                    var review = Context.Review.FirstOrDefault(x => x.Id.Equals(maintenance.ReviewId));
                    if (review == null) return false;
                    review.Comment = request.Comment;
                    review.Star = request.Star;
                    Context.Review.Update(review);
                    Context.SaveChanges();
                    return true;
                }

                var newReview = new Review
                {
                    Comment = request.Comment,
                    Star = request.Star,
                    CreatedDate = DateTime.Now,
                    UserId = userId
                };

                Context.Review.Add(newReview);
                Context.SaveChanges();

                maintenance.ReviewId = newReview.Id;
                Context.Maintenance.Update(maintenance);
                Context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}