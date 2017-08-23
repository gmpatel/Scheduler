using BET.Market.Jobs.DataAccess.EF.Interfaces;

namespace BET.Market.Jobs.DataAccess.EF.Defaults
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
    }
}