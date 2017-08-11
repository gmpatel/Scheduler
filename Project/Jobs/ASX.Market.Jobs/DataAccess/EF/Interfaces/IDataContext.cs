using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using ASX.Market.Jobs.Core.Entities;

namespace ASX.Market.Jobs.DataAccess.EF.Interfaces
{
    public interface IDataContext : IDisposable
    {
        long Id { get; }
        long Instances { get; }
        int SaveChanges();
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        DbEntityEntry Entry<TEntity>(TEntity entity) where TEntity : class;
        
        DbSet<ExchangeEntity> Exchanges { get; set; }
        DbSet<IndexEntity> Indices { get; set; }
    }
}
