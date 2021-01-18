using System.Collections.Generic;
using System.Linq;
using main_service.Databases;
using main_service.Repositories.Base;
using main_service.RestApi.Requests;

namespace main_service.Repositories
{
    public class NotificationsRepository : BaseRepository<Notification>
    {
        public NotificationsRepository(AppDBContext context) : base(context)
        {
        }
        
        public IEnumerable<Notification> QueryNotifications(NotificationQuery notificationQuery)
        {
            IQueryable<Notification> query = DbSet;
            if (notificationQuery.UserId != null)
            {
                query = query.Where(x => x.UserId.Equals(notificationQuery.UserId));
            }

            query = query.OrderByDescending(x => x.CreatedDate);

            return query.ToList();
        }
        
        public IEnumerable<Banner> QueryBanners()
        {
            IQueryable<Banner> query = Context.Banner.AsQueryable();
            return query.ToList();
        }
        
        public IEnumerable<News> QueryNews()
        {
            IQueryable<News> query = Context.News.AsQueryable();
            return query.ToList();
        }
    }
}