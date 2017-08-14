using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ASX.Market.Jobs.Core.Entities;
using ASX.Market.Jobs.DataAccess.EF.Interfaces;

namespace ASX.Market.Jobs.DataAccess.EF.Defaults
{
    public class DataService : IDataService
    {
        public readonly IUnitOfWork UnitOfWork;

        internal static class Constants
        {
        }

        private static long objectsCounter;

        static DataService()
        {
        }

        public DataService() : this(new UnitOfWork())
        {    
        }

        public DataService(IUnitOfWork unitOfWork)
        {
            Id = ++objectsCounter;

            this.UnitOfWork = unitOfWork;
        }

        public long Id { get; private set; }
        public long Instances => objectsCounter;

        public void Dispose()
        {
            UnitOfWork.Dispose();
        }

        public IList<ExchangeEntity> GetExchanges(long? id = null)
        {
            return UnitOfWork.ExchangeRepository.FindBy(x => (id == null || x.Id == id.Value)).ToList();
        }

        public IList<IndexEntity> GetIndices(long? exchangeId = null)
        {
            return UnitOfWork.IndexRepository.FindBy(x => (exchangeId == null || x.ExchangeId == exchangeId.Value)).ToList();
        }

        public StockDetailEntity PushStockDetail(long? indexId, StockDetailEntity stockDetailEntity)
        {
            var dateTime = DateTime.Now;
            var indexEntity = default(IndexEntity);

            if (indexId != null)
            {
                indexEntity = UnitOfWork.IndexRepository.FindBy(x => x.Id.Equals(indexId.Value)).FirstOrDefault();
            }

            if (!UnitOfWork.StockRepository.FindBy(x => x.Code.Equals(stockDetailEntity.Stock.Code, StringComparison.CurrentCultureIgnoreCase)).Any())
            {
                var stockDetail = new StockDetailEntity
                {
                    Date = stockDetailEntity.Date,
                    Price = stockDetailEntity.Price,
                    Change = stockDetailEntity.Change,
                    ChangePercent = stockDetailEntity.ChangePercent,
                    High = stockDetailEntity.High,
                    Low = stockDetailEntity.Low,
                    Volume = stockDetailEntity.Volume,
                    MarketCapital = stockDetailEntity.MarketCapital,
                    OneYearChange = stockDetailEntity.OneYearChange,
                    Flag1 = stockDetailEntity.Flag1,
                    DateTimeCreated = dateTime,
                };

                var stock = new StockEntity
                {
                    Code = stockDetailEntity.Stock.Code.ToUpper(),
                    Name = stockDetailEntity.Stock.Name,
                    DateTimeCreated = dateTime,
                    StockDetails = new List<StockDetailEntity>
                    {
                        stockDetail
                    }
                };

                UnitOfWork.StockRepository.Add(stock);
                indexEntity?.Stocks.Add(stock);

                UnitOfWork.Save();

                return stockDetail;
            }
            else
            {
                var stock = UnitOfWork.StockRepository.FindBy(x => x.Code.Equals(stockDetailEntity.Stock.Code, StringComparison.CurrentCultureIgnoreCase)).First();

                if (indexEntity != null)
                {
                    if (indexEntity.Stocks.FirstOrDefault(x => x.Id.Equals(stock.Id)) == null)
                    {
                        indexEntity.Stocks.Add(stock);
                    }
                }

                if (UnitOfWork.StockDetailRepository.FindBy(x => x.Date.Equals(stockDetailEntity.Date) && x.StockId.Equals(stock.Id)).Any())
                {
                    var stockDetails = UnitOfWork.StockDetailRepository.FindBy(x => x.Date.Equals(stockDetailEntity.Date) && x.StockId.Equals(stock.Id)).First();

                    stockDetails.Date = stockDetailEntity.Date;
                    stockDetails.Price = stockDetailEntity.Price;
                    stockDetails.Change = stockDetailEntity.Change;
                    stockDetails.ChangePercent = stockDetailEntity.ChangePercent;
                    stockDetails.High = stockDetailEntity.High;
                    stockDetails.Low = stockDetailEntity.Low;
                    stockDetails.Volume = stockDetailEntity.Volume;
                    stockDetails.MarketCapital = stockDetailEntity.MarketCapital;
                    stockDetails.OneYearChange = stockDetailEntity.OneYearChange;
                    stockDetails.Flag1 = stockDetailEntity.Flag1;
                    stockDetails.DateTimeLastModified = dateTime;

                    UnitOfWork.StockDetailRepository.Update(stockDetails);
                }
                else
                {
                    var stockDetails = new StockDetailEntity
                    {
                        Date = stockDetailEntity.Date,
                        Price = stockDetailEntity.Price,
                        Change = stockDetailEntity.Change,
                        ChangePercent = stockDetailEntity.ChangePercent,
                        High = stockDetailEntity.High,
                        Low = stockDetailEntity.Low,
                        Volume = stockDetailEntity.Volume,
                        MarketCapital = stockDetailEntity.MarketCapital,
                        OneYearChange = stockDetailEntity.OneYearChange,
                        Flag1 = stockDetailEntity.Flag1,
                        DateTimeCreated = dateTime,
                    };

                    UnitOfWork.StockDetailRepository.Add(stockDetails);
                }

                UnitOfWork.Save();
            }

            return null;
        }
    }
}