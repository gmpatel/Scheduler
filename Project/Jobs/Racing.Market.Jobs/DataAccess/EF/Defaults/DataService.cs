using System;
using System.Collections.Generic;
using System.Linq;
using BET.Market.Jobs.Core.Entities;
using BET.Market.Jobs.DataAccess.EF.Interfaces;

namespace BET.Market.Jobs.DataAccess.EF.Defaults
{
    public class DataService : IDataService
    {
        private readonly DateTime dateTime;

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
            this.dateTime = DateTime.Now;
            this.UnitOfWork = unitOfWork;
        }

        public long Id { get; private set; }
        public long Instances => objectsCounter;

        public void Dispose()
        {
            UnitOfWork.Dispose();
        }

        public bool UpdateData(IList<VenueEntity> venues)
        {
            if (venues.Any())
            {
                foreach (var venue in venues)
                {
                    VenueEntity v;

                    if (UnitOfWork.VenueRepository.FindBy(x => x.Name.Equals(venue.Name) || x.Name2.Equals(venue.Name) || x.Name3.Equals(venue.Name)).Any())
                    {
                        v = UnitOfWork.VenueRepository.FindBy(x => x.Name.Equals(venue.Name) || x.Name2.Equals(venue.Name) || x.Name3.Equals(venue.Name)).First();

                        v.DateTimeLastModified = (venue.DateTimeCreated.Equals(default(DateTime)) ? dateTime : venue.DateTimeCreated);

                        UnitOfWork.VenueRepository.Update(v);
                        UnitOfWork.Save();
                    }
                    else
                    {
                        v = new VenueEntity
                        {
                            Name = venue.Name.ToUpper(),
                            Province = venue.Province,
                            DateTimeCreated = (venue.DateTimeCreated.Equals(default(DateTime)) ? dateTime : venue.DateTimeCreated)
                        };

                        UnitOfWork.VenueRepository.Add(v);
                        UnitOfWork.Save();
                    }

                    if (venue.Meetings.Any())
                    {
                        foreach (var meeting in venue.Meetings)
                        {
                            MeetingEntity m;

                            if (UnitOfWork.MeetingRepository.FindBy(x => x.Date.Equals(meeting.Date) && x.VenueId.Equals(v.Id)).Any())
                            {
                                m = UnitOfWork.MeetingRepository.FindBy(x => x.Date.Equals(meeting.Date) && x.VenueId.Equals(v.Id)).First();

                                m.Date = meeting.Date;
                                m.FormUrl = meeting.FormUrl;
                                m.TipsUrl = meeting.TipsUrl;
                                m.VenueId = v.Id;
                                m.DateTimeLastModified = (meeting.DateTimeCreated.Equals(default(DateTime)) ? dateTime : meeting.DateTimeCreated);
                                
                                UnitOfWork.MeetingRepository.Update(m);
                                UnitOfWork.Save();
                            }
                            else
                            {
                                m = new MeetingEntity
                                {
                                    Date = meeting.Date,
                                    FormUrl = meeting.FormUrl,
                                    TipsUrl = meeting.TipsUrl,
                                    VenueId = v.Id,
                                    DateTimeCreated = (meeting.DateTimeCreated.Equals(default(DateTime)) ? dateTime : meeting.DateTimeCreated)
                                };

                                UnitOfWork.MeetingRepository.Add(m);
                                UnitOfWork.Save();
                            }

                            if (meeting.Races.Any())
                            {
                                foreach (var race in meeting.Races)
                                {
                                    RaceEntity r;

                                    if (UnitOfWork.RaceRepository.FindBy(x => x.MeetingId.Equals(m.Id) && x.Number.Equals(race.Number)).Any())
                                    {
                                        r = UnitOfWork.RaceRepository.FindBy(x => x.MeetingId.Equals(m.Id) && x.Number.Equals(race.Number)).First();

                                        r.Name = race.Name.ToUpper();
                                        r.Time = race.Time;
                                        r.Weather = race.Weather;
                                        r.Track = race.Track;
                                        r.Distance = race.Distance;
                                        r.Class = race.Class;
                                        r.Prizemoney = race.Prizemoney;
                                        r.DateTimeLastModified = (race.DateTimeCreated.Equals(default(DateTime)) ? dateTime : race.DateTimeCreated);

                                        UnitOfWork.RaceRepository.Update(r);
                                        UnitOfWork.Save();
                                    }
                                    else
                                    {
                                        r = new RaceEntity
                                        {
                                            MeetingId = m.Id,
                                            Number = race.Number,
                                            Name = race.Name.ToUpper(),
                                            Time = race.Time,
                                            Weather = race.Weather,
                                            Track = race.Track,
                                            Distance = race.Distance,
                                            Class = race.Class,
                                            Prizemoney = race.Prizemoney,
                                            DateTimeCreated = (race.DateTimeCreated.Equals(default(DateTime)) ? dateTime : race.DateTimeCreated),
                                        };

                                        UnitOfWork.RaceRepository.Add(r);
                                        UnitOfWork.Save();
                                    }

                                    if (race.Runners.Any())
                                    {
                                        foreach (var runner in race.Runners)
                                        {
                                            RunnerEntity n;

                                            if (UnitOfWork.RunnerRepository.FindBy(x => x.RaceId.Equals(r.Id) && x.Number.Equals(runner.Number)).Any())
                                            {
                                                n = UnitOfWork.RunnerRepository.FindBy(x => x.RaceId.Equals(r.Id) && x.Number.Equals(runner.Number)).First();

                                                n.Name = runner.Name.ToUpper();
                                                n.Rating = runner.Rating;
                                                n.LastFiveRuns = runner.LastFiveRuns;
                                                n.Scratched = runner.Scratched;
                                                n.Barrel = runner.Barrel;
                                                n.Tcdw = runner.Tcdw;
                                                n.Trainer = runner.Trainer;
                                                n.Jockey = runner.Jockey;
                                                n.Weight = runner.Weight;
                                                n.ResultPosition = runner.ResultPosition;
                                                n.FormSkyRating = runner.FormSkyRating;
                                                n.FormSkyRatingPosition = runner.FormSkyRatingPosition;
                                                n.FormBest12Months = runner.FormBest12Months;
                                                n.FormBest12MonthsPosition = runner.FormBest12MonthsPosition;
                                                n.FormRecent = runner.FormRecent;
                                                n.FormRecentPosition = runner.FormRecentPosition;
                                                n.FormDistance = runner.FormDistance;
                                                n.FormDistancePosition = runner.FormDistancePosition;
                                                n.FormClass = runner.FormClass;
                                                n.FormClassPosition = runner.FormClassPosition;
                                                n.FormTimeRating = runner.FormTimeRating;
                                                n.FormTimeRatingPosition = runner.FormTimeRatingPosition;
                                                n.FormInWet = runner.FormInWet;
                                                n.FormInWetPosition = runner.FormInWetPosition;
                                                n.FormBestOverall = runner.FormBestOverall;
                                                n.FormBestOverallPosition = runner.FormBestOverallPosition;
                                                n.TipSky = runner.TipSky;
                                                n.TipSkyPosition = runner.TipSkyPosition;
                                                n.DateTimeLastModified = (runner.DateTimeCreated.Equals(default(DateTime)) ? dateTime : runner.DateTimeCreated);

                                                UnitOfWork.RunnerRepository.Update(n);
                                                UnitOfWork.Save();
                                            }
                                            else
                                            {
                                                n = new RunnerEntity
                                                {
                                                    RaceId = r.Id,
                                                    Number = runner.Number,
                                                    Name = runner.Name.ToUpper(),
                                                    Rating = runner.Rating,
                                                    LastFiveRuns = runner.LastFiveRuns,
                                                    Scratched = runner.Scratched,
                                                    Barrel = runner.Barrel,
                                                    Tcdw = runner.Tcdw,
                                                    Trainer = runner.Trainer,
                                                    Jockey = runner.Jockey,
                                                    Weight = runner.Weight,
                                                    ResultPosition = runner.ResultPosition,
                                                    FormSkyRating = runner.FormSkyRating,
                                                    FormSkyRatingPosition = runner.FormSkyRatingPosition,
                                                    FormBest12Months = runner.FormBest12Months,
                                                    FormBest12MonthsPosition = runner.FormBest12MonthsPosition,
                                                    FormRecent = runner.FormRecent,
                                                    FormRecentPosition = runner.FormRecentPosition,
                                                    FormDistance = runner.FormDistance,
                                                    FormDistancePosition = runner.FormDistancePosition,
                                                    FormClass = runner.FormClass,
                                                    FormClassPosition = runner.FormClassPosition,
                                                    FormTimeRating = runner.FormTimeRating,
                                                    FormTimeRatingPosition = runner.FormTimeRatingPosition,
                                                    FormInWet = runner.FormInWet,
                                                    FormInWetPosition = runner.FormInWetPosition,
                                                    FormBestOverall = runner.FormBestOverall,
                                                    FormBestOverallPosition = runner.FormBestOverallPosition,
                                                    TipSky = runner.TipSky,
                                                    TipSkyPosition = runner.TipSkyPosition,
                                                    DateTimeCreated = (runner.DateTimeCreated.Equals(default(DateTime)) ? dateTime : runner.DateTimeCreated)
                                                };

                                                UnitOfWork.RunnerRepository.Add(n);
                                                UnitOfWork.Save();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            return true;
        }
    }
}