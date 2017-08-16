using System;
using System.Collections.Generic;
using ASX.Market.Jobs.Core.Entities;

namespace ASX.Market.Jobs.DataAccess.EF.Interfaces
{
    public interface IDataService : IDisposable
    {
        long Id { get; }
        long Instances { get; }

        IList<ExchangeEntity> GetExchanges(long? id = null);
        IList<IndexEntity> GetIndices(long? exchangeId = null);
        IList<StockEntity> GetStocks(long? id = null);
        IList<StockEntity> GetStocksByCodes(IEnumerable<string> codes = null);
        StockDetailEntity PushStockDetail(long? indexId, StockDetailEntity stockDetailEntity);
        bool CheckStockDetailExists(long stockId, long date);
    }
}