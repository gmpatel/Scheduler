using System;
using ASX.Market.Jobs.Core.Entities;

namespace ASX.Market.Jobs.DataAccess.EF.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        long Id { get; }
        long Instances { get; }
        IDataContext DbContext { get; }
        IRepository<ExchangeEntity> ExchangeRepository { get; }
        IRepository<IndexEntity> IndexRepository { get; }
        IRepository<StockEntity> StockRepository { get; }
        IRepository<StockDetailEntity> StockDetailRepository { get; }
        int Save();
    }
}