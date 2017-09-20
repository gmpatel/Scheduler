using System;
using System.Collections.Generic;
using BET.Market.Jobs.Core.Entities;

namespace BET.Market.Jobs.DataAccess.EF.Interfaces
{
    public interface IDataServiceBET : IDisposable
    {
        long Id { get; }
        long Instances { get; }
        IList<VenueEntity> GetVenues();
        bool UpdateData(IList<VenueEntity> venues);
    }
}