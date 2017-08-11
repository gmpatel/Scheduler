using System;

namespace ASX.Market.Jobs.DataAccess.EF.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        long Id { get; }
        long Instances { get; }
        IDataContext DbContext { get; }
        int Save();
    }
}