using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using ASX.Market.Jobs.DataAccess.EF.Interfaces;

namespace ASX.Market.Jobs.DataAccess.EF.Defaults
{
    public class ViewRepository<TEntity> : IViewRepository<TEntity> where TEntity : class
    {
        private static long counter;

        public IDataContext DbContext { get; private set; }

        public ViewRepository(IDataContext dataContext)
        {
            Id = ++counter;

            //this.UnitOfWork = unitOfWork;
            this.DbContext = dataContext;
        }

        public long Id { get; private set; }
        public long Instances => counter;

        public IQueryable<TEntity> GetAll()
        {
            IQueryable<TEntity> query = DbContext.Set<TEntity>();
            return query;
        }

        public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> query = DbContext.Set<TEntity>().Where(predicate);
            return query;
        }
    }
}