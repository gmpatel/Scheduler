using System;
using System.Linq;
using System.Linq.Expressions;

namespace BET.Market.Jobs.DataAccess.EF.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        long Id { get; }
        long Instances { get; }
        IDataContext DbContext { get; }

        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
        TEntity Add(TEntity entity);
        void Delete(TEntity entity);
        void Update(TEntity entity);
    }
}