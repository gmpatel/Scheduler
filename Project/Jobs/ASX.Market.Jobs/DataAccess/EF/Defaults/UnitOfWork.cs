using System;
using ASX.Market.Jobs.Core.Entities;
using ASX.Market.Jobs.DataAccess.EF.Interfaces;

namespace ASX.Market.Jobs.DataAccess.EF.Defaults
{
    public class UnitOfWork : IUnitOfWork
    {
        private static long counter;

        private Repository<ExchangeEntity> exchangeRepository;
        private Repository<IndexEntity> indexRepository;
        private Repository<StockEntity> stockRepository;
        private Repository<StockDetailEntity> stockDetailRepository;

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

        public IRepository<ExchangeEntity> ExchangeRepository
        {
            get
            {
                if (this.exchangeRepository == null)
                {
                    this.exchangeRepository = new Repository<ExchangeEntity>(DbContext);
                }

                return this.exchangeRepository;
            }
        }

        public IRepository<IndexEntity> IndexRepository
        {
            get
            {
                if (this.indexRepository == null)
                {
                    this.indexRepository = new Repository<IndexEntity>(DbContext);
                }

                return this.indexRepository;
            }
        }

        public IRepository<StockEntity> StockRepository
        {
            get
            {
                if (this.stockRepository == null)
                {
                    this.stockRepository = new Repository<StockEntity>(DbContext);
                }

                return this.stockRepository;
            }
        }

        public IRepository<StockDetailEntity> StockDetailRepository
        {
            get
            {
                if (this.stockDetailRepository == null)
                {
                    this.stockDetailRepository = new Repository<StockDetailEntity>(DbContext);
                }

                return this.stockDetailRepository;
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
