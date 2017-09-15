using System;
using Market.Authentication.Core.Entities;
using Market.Authentication.DataAccess.EF.Interfaces;

namespace Market.Authentication.DataAccess.EF.Defaults
{
    public class UnitOfWork : IUnitOfWork
    {
        private static long counter;

        private Repository<ClientEntity> clientRepository;
        private Repository<FamilyTypeEntity> familyTypeRepository;
        private Repository<IncomeRangeEntity> incomeRangeRepository;
        private Repository<RoleEntity> roleRepository;
        private Repository<StateEntity> stateRepository;
        private Repository<TokenEntity> tokenRepository;
        private Repository<UserEntity> userRepository;

        public long Id { get; private set; }
        public long Instances => counter;

        public UnitOfWork() : this(new DataContext())
        {
        }

        public UnitOfWork(IDataContext dbContext)
        {
            Id = ++counter;
            DbContext = dbContext;
        }

        public IDataContext DbContext { get; private set; }

        public IRepository<ClientEntity> ClientRepository
        {
            get
            {
                if (this.clientRepository == null)
                {
                    this.clientRepository = new Repository<ClientEntity>(DbContext);
                }

                return this.clientRepository;
            }
        }

        public IRepository<FamilyTypeEntity> FamilyTypeRepository
        {
            get
            {
                if (this.familyTypeRepository == null)
                {
                    this.familyTypeRepository = new Repository<FamilyTypeEntity>(DbContext);
                }

                return this.familyTypeRepository;
            }
        }

        public IRepository<IncomeRangeEntity> IncomeRangeRepository
        {
            get
            {
                if (this.incomeRangeRepository == null)
                {
                    this.incomeRangeRepository = new Repository<IncomeRangeEntity>(DbContext);
                }

                return this.incomeRangeRepository;
            }
        }

        public IRepository<RoleEntity> RoleRepository
        {
            get
            {
                if (this.roleRepository == null)
                {
                    this.roleRepository = new Repository<RoleEntity>(DbContext);
                }

                return this.roleRepository;
            }
        }
        
        public IRepository<StateEntity> StateRepository
        {
            get
            {
                if (this.stateRepository== null)
                {
                    this.stateRepository = new Repository<StateEntity>(DbContext);
                }

                return this.stateRepository;
            }
        }

        public IRepository<TokenEntity> TokenRepository
        {
            get
            {
                if (this.tokenRepository == null)
                {
                    this.tokenRepository = new Repository<TokenEntity>(DbContext);
                }

                return this.tokenRepository;
            }
        }

        public IRepository<UserEntity> UserRepository
        {
            get
            {
                if (this.userRepository == null)
                {
                    this.userRepository = new Repository<UserEntity>(DbContext);
                }

                return this.userRepository;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public int Save()
        {
            return DbContext.SaveChanges();
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (DbContext != null)
                {
                    DbContext.Dispose();
                    DbContext = null;
                }
            }
        }
    }
}