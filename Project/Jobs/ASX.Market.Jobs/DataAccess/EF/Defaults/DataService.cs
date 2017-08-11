using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASX.Market.Jobs.Core.Entities;
using ASX.Market.Jobs.DataAccess.EF.Interfaces;

namespace ASX.Market.Jobs.DataAccess.EF.Defaults
{
    public class DataService : IDataService
    {
        public readonly IUnitOfWork UnitOfWork;
        public readonly IDataContext DataContext;

        public readonly IRepository<ExchangeEntity> ExchangeRepository;
        public readonly IRepository<IndexEntity> IndexRepository;

        internal static class Constants
        {
        }

        private static long objectsCounter;

        static DataService()
        {
        }

        public DataService() : this(new UnitOfWork(), new DataContext(), 
            new Repository<ExchangeEntity>(),
            new Repository<IndexEntity>()
        )
        {    
        }

        public DataService(IUnitOfWork unitOfWork, IDataContext dataContext,
            IRepository<ExchangeEntity> exchangeRepository,
            IRepository<IndexEntity> indexRepository
        )
        {
            Id = ++objectsCounter;

            this.UnitOfWork = unitOfWork;
            this.DataContext = dataContext;

            this.ExchangeRepository = exchangeRepository;
            this.IndexRepository = indexRepository;
        }

        public long Id { get; private set; }
        public long Instances => objectsCounter;

        public void Dispose()
        {
            UnitOfWork.Dispose();
        }

        public IList<ExchangeEntity> GetExchanges(long? id = null)
        {
            return ExchangeRepository.FindBy(x => (id == null || x.Id == id.Value)).ToList();
        }

        public IList<IndexEntity> GetIndices(long? exchangeId = null)
        {
            return IndexRepository.FindBy(x => (exchangeId == null || x.ExchangeId == exchangeId.Value)).ToList();
        }
    }
}