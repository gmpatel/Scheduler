using System;
using System.Linq;
using System.Linq.Expressions;

namespace ASX.Market.Jobs.DataAccess.EF.Interfaces
{
    public interface IViewRepository<TEntity> where TEntity : class
    {
        long Id { get; }
        long Instances { get; }
        IDataContext DbContext { get; }

        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
    }
}