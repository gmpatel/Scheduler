using System;
using BET.Market.Jobs.Core.Entities;

namespace BET.Market.Jobs.DataAccess.EF.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        long Id { get; }
        long Instances { get; }
        IDataContext DbContext { get; }
        IRepository<MeetingEntity> MeetingRepository { get; }
        IRepository<RaceEntity> RaceRepository { get; }
        IRepository<RunnerEntity> RunnerRepository { get; }
        int Save();
    }
}