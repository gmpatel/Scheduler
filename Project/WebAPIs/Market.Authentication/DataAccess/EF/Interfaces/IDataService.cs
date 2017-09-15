using System;
using System.Collections.Generic;

namespace Market.Authentication.DataAccess.EF.Interfaces
{
    public interface IDataService : IDisposable
    {
        long Id { get; }
        long Instances { get; }
    }
}