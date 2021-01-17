using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using main_service.Databases;
using main_service.Helpers;
using main_service.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace main_service.Repositories
{
    public class StatisticRepository
    {
        private readonly AppDBContext _context;

        public StatisticRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<object> GetCurrentStatistic()
        {
            var userReceivaleNotifyCount = await _context.FcmToken
                .Where(x => x.User.Role == Constants.Role.User)
                .CountAsync(x => x.Token != null && x.Token.Length > 0);
            var serviceCount = await _context.MaintenanceService.CountAsync();
            var userCount = await _context.User.CountAsync(x => x.Role == Constants.Role.User);
            var maintenanceStaffCount = await _context.User.CountAsync(x => x.Role == Constants.Role.StaffMaintenance);
            var deskStaffCount = await _context.User.CountAsync(x => x.Role == Constants.Role.StaffDesk);
            var topicCount = await _context.Topic.CountAsync();

            return new
            {
                userReceivaleNotifyCount = userReceivaleNotifyCount,
                serviceCount = serviceCount,
                userCount = userCount,
                maintenanceStaffCount = maintenanceStaffCount,
                deskStaffCount = deskStaffCount,
                topicCount = topicCount
            };
        }

        public async Task<object> GetStatistic(DateTime startDate, DateTime endDate)
        {
            var result = new List<object>();
            var queryMaintenance = await
                _context.Maintenance.Where(x => x.CreatedDate > startDate && x.CreatedDate < endDate)
                    .Include(x => x.MaintenanceBillDetail)
                    .ToListAsync();
            var queryUser = await _context.User.Where(x => x.CreatedDate > startDate && x.CreatedDate < endDate)
                                               .ToListAsync();
            var queryReview = await _context.Review.Where(x => x.CreatedDate > startDate && x.CreatedDate < endDate)
                .ToListAsync();
            
            var queryTopic = await _context.Topic.Where(x => x.CreatedDate > startDate && x.CreatedDate < endDate)
                .ToListAsync();

            foreach (DateTime day in DateTimeHelper.EachDay(startDate, endDate))
            {
                var eachDayMaintenance = queryMaintenance
                    .Where(x => x.CreatedDate?.Day == day.Day && x.CreatedDate?.Month == day.Month &&
                                x.CreatedDate?.Year == day.Year).ToList();
                var eachDayUser = queryUser
                    .Where(x => x.CreatedDate?.Day == day.Day && x.CreatedDate?.Month == day.Month &&
                                x.CreatedDate?.Year == day.Year).ToList();
                var eachDayReviews = queryReview
                    .Where(x => x.CreatedDate.Day == day.Day && x.CreatedDate.Month == day.Month &&
                                x.CreatedDate.Year == day.Year).ToList();
                var eachDayTopic = queryTopic
                    .Where(x => x.CreatedDate.Day == day.Day && x.CreatedDate.Month == day.Month &&
                                x.CreatedDate.Year == day.Year).ToList();
                
                result.Add(new
                {
                    totalBill = eachDayMaintenance
                        .Sum(x => x.MaintenanceBillDetail.Sum(y => y.Quantity * y.LaborCost)),
                    numberBill = eachDayMaintenance.Count,
                    newUserCount = eachDayUser
                        .Count(x => x.Role == Constants.Role.User),
                    reviewAvg = eachDayReviews.Count > 0 ? eachDayReviews.Average(x => x.Star) : 0,
                    topicCount = eachDayTopic.Count,
                    date = day
                });
            }

            return result;
        }
    }
}