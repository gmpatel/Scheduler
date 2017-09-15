using System;
using Market.Authentication.Core.Entities;

namespace Market.Authentication.DataAccess.EF.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        long Id { get; }
        long Instances { get; }

        IDataContext DbContext { get; }

        IRepository<ClientEntity> ClientRepository { get; }
        IRepository<FamilyTypeEntity> FamilyTypeRepository { get; }
        IRepository<IncomeRangeEntity> IncomeRangeRepository { get; }
        IRepository<RoleEntity> RoleRepository { get; }
        IRepository<StateEntity> StateRepository { get; }
        IRepository<TokenEntity> TokenRepository { get; }
        IRepository<UserEntity> UserRepository { get; }

        int Save();
    }
}