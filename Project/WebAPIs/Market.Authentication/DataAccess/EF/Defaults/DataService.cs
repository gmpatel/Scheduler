using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Market.Authentication.DataAccess.EF.Interfaces;

namespace Market.Authentication.DataAccess.EF.Defaults
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