using System;
using ASX.Market.Jobs.Core.Entities;
using ASX.Market.Jobs.DataAccess.EF.Defaults;

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
        IRepository<StockDetailAggregatedEntity> StockDetailAggregatedRepository { get; }
        IRepository<StockDetailAggregatedLatestMovementEntity> StockDetailAggregatedLatestMovementRepository { get; }
        
        int Save();
    }
}