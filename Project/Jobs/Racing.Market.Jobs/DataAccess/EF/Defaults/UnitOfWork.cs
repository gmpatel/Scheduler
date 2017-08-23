using System;
using BET.Market.Jobs.Core.Entities;
using BET.Market.Jobs.DataAccess.EF.Interfaces;

namespace BET.Market.Jobs.DataAccess.EF.Defaults
{
    public class UnitOfWork : IUnitOfWork
    {
        private static long counter;

        private Repository<MeetingEntity> meetingRepository;
        private Repository<RaceEntity> raceRepository;
        private Repository<RunnerEntity> runnerRepository;
        
        public long Id { get; private set; }
        public long Instances => counter;

        public UnitOfWork() : this(new DataContext())
        {
        }

        public UnitOfWork(IDataContext dbContext)
        {
            Id = ++counter;
            DbContext = dbContext;
        }

        public IDataContext DbContext { get; private set; }

        public IRepository<MeetingEntity> MeetingRepository
        {
            get
            {
                if (this.meetingRepository == null)
                {
                    this.meetingRepository = new Repository<MeetingEntity>(DbContext);
                }

                return this.meetingRepository;
            }
        }

        public IRepository<RaceEntity> RaceRepository
        {
            get
            {
                if (this.raceRepository == null)
                {
                    this.raceRepository = new Repository<RaceEntity>(DbContext);
                }

                return this.raceRepository;
            }
        }

        public IRepository<RunnerEntity> RunnerRepository
        {
            get
            {
                if (this.runnerRepository == null)
                {
                    this.runnerRepository = new Repository<RunnerEntity>(DbContext);
                }

                return this.runnerRepository;
            }
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public int Save()
        {
            return DbContext.SaveChanges();
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (DbContext != null)
                {
                    DbContext.Dispose();
                    DbContext = null;
                }
            }
        }
    }
}