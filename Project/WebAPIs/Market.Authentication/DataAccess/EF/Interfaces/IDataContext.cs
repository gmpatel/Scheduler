using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Market.Authentication.Core.Entities;

namespace Market.Authentication.DataAccess.EF.Interfaces
{
    public interface IDataContext : IDisposable
    {
        long Id { get; }
        long Instances { get; }
        int SaveChanges();
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        DbEntityEntry Entry<TEntity>(TEntity entity) where TEntity : class;
        
        DbSet<ClientEntity> Clients { get; set; }
        DbSet<FamilyTypeEntity> FamilyTypes { get; set; }
        DbSet<IncomeRangeEntity> IncomeRanges { get; set; }
        DbSet<RoleEntity> Roles { get; set; }
        DbSet<StateEntity> States { get; set; }
        DbSet<TokenEntity> Tokens { get; set; }
        DbSet<UserEntity> Users { get; set; }
    }
}