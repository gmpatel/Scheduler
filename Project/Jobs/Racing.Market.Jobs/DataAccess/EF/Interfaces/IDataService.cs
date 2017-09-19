using System;
using System.Collections.Generic;
using BET.Market.Jobs.Core.Entities;

namespace BET.Market.Jobs.DataAccess.EF.Interfaces
{
    public interface IDataService : IDisposable
    {
        long Id { get; }
        long Instances { get; }
        bool UpdateData(IList<VenueEntity> venues);
    }
}