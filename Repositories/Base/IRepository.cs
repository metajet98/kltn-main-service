using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace main_service.Repositories.Base
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Delete(TEntity entityToDelete);
        void Delete(object id);

        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        TEntity? GetById(object id);

        IEnumerable<TEntity> GetWithRawSql(string query,
            params object[] parameters);

        void Insert(TEntity entity);
        void Update(TEntity entityToUpdate);

        void Save();
    }
}