using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using BET.Market.Jobs.Core.Entities;

namespace BET.Market.Jobs.DataAccess.EF.Interfaces
{
    public interface IDataContext : IDisposable
    {
        long Id { get; }
        long Instances { get; }
        int SaveChanges();
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        DbEntityEntry Entry<TEntity>(TEntity entity) where TEntity : class;
        
        DbSet<MeetingEntity> Meetings { get; set; }
        DbSet<RaceEntity> Races { get; set; }
        DbSet<RunnerEntity> Runners { get; set; }
    }
}
