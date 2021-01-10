using System;
using System.Collections.Generic;
using System.Linq;
using main_service.Databases;
using main_service.Repositories.Base;
using main_service.RestApi.Requests;
using Microsoft.EntityFrameworkCore;

namespace main_service.Repositories
{
    public class UserCalenderRepository : BaseRepository<CustomerCalender>
    {
        public UserCalenderRepository(AppDBContext context) : base(context)
        {
        }

        public bool Create(UserCalenderRequest request, int userId)
        {
            try
            {
                var calender = DbSet.FirstOrDefault(x =>
                    x.Time.Year == request.Time.Year &&
                    x.Time.Month == request.Time.Month &&
                    x.Time.Day == request.Time.Day && 
                    x.BranchId.Equals(request.BranchId) && 
                    x.UserId.Equals(userId));
                if (calender != null)
                {
                    calender.Notes = request.Notes;
                    calender.Time = request.Time;
                    DbSet.Update(calender);
                    Context.SaveChanges();
                    return true;
                }
                DbSet.Add(new CustomerCalender
                {
                    Notes = request.Notes,
                    Time = request.Time,
                    UserId = userId,
                    BranchId = request.BranchId,
                    CreatedDate = DateTime.Now,
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

        public IEnumerable<CustomerCalender> Query(UserCalenderQuery queryData)
        {
            var query = Context.CustomerCalender.AsQueryable();
            if (queryData.UserId != null)
            {
                query = query.Where(x => x.UserId.Equals(queryData.UserId));
            }

            if (queryData.BranchId != null)
            {
                query = query.Where(x => x.BranchId.Equals(queryData.BranchId));
            }

            if (queryData.From != null && queryData.To != null)
            {
                query = query
                    .Where(x =>
                        x.Time >= queryData.From && x.Time <= queryData.To);
            }

            query = query
                .Include(x => x.Branch)
                .Include(x => x.User);

            return query.ToList();
        }
    }
}