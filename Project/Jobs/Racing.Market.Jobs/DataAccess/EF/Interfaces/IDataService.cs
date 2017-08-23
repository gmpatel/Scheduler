using System;

namespace BET.Market.Jobs.DataAccess.EF.Interfaces
{
    public interface IDataService : IDisposable
    {
        long Id { get; }
        long Instances { get; }
    }
}