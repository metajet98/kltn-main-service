using System;
using System.Linq;
using main_service.Databases;
using main_service.Repositories.Base;

namespace main_service.Repositories
{
    public class FcmTokenRepository : BaseRepository<FcmToken>
    {
        public FcmTokenRepository(AppDBContext context) : base(context)
        {
        }

        public bool InsertToken(string token, int userId)
        {
            try
            {
                var tokenDb = DbSet.FirstOrDefault(x => x.Token.Equals(token) && x.UserId.Equals(userId));
                if (tokenDb != null) return true;
                DbSet.Add(new FcmToken
                {
                    Token = token,
                    UserId = userId,
                    CreatedDate = DateTime.Now,
                });
                Context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool RemoveToken(string token, int userId)
        {
            var tokenDb = DbSet.FirstOrDefault(x => x.Token.Equals(token) && x.UserId.Equals(userId));
            if (tokenDb != null)
            {
                DbSet.Remove(tokenDb);
                Context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}