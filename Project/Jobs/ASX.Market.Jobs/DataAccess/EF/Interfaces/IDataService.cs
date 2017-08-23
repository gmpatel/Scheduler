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
        IList<IndexEntity> GetIndicesByCodes(IEnumerable<string> codes = null);
        IList<StockEntity> GetStocks(long? id = null);
        IList<StockEntity> GetStocksByCodes(IEnumerable<string> codes = null);
        StockDetailEntity PushStockDetail(long? indexId, StockDetailEntity stockDetailEntity, DateTime dateTime);
        bool CheckStockDetailExists(long stockId, long date);
    }
}