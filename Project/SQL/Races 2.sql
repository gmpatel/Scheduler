
select * from Venues where Name = 'WARWICK FARM'
select * from Meetings where VenueId = 9
select * from Races where MeetingId = 9
select * from Runners where RaceId in (select Id from Races where MeetingId = 3)

Select * from (Select RaceId, count(*)[Horses] From Runners where (FormBestOverall = 1) group by RaceId ) x where x.Horses <= 4


select * from Runners where RaceId = 65
select * from
(
	select 
	  RaceId,
	  count(*) [Total], 
	  count(case Scratched when 1 then 1 end) [Scratched], 
	  count(case when len(LastFiveRuns) >= 4 and Scratched = 0 then 1 end) [Good], 
	  count(case when len(LastFiveRuns) < 4 and Scratched = 0 then 1 end) [New],
	  count(case when FormBestOverall = 1 and Scratched = 0 then 1 end) [BestOverall]
	from 
	  Runners 
	group by
	  RaceId
) r where (r.Total - r.Scratched) = r.Good



(Select * from (Select RaceId, count(*)[Horses] From Runners where (FormBestOverall = 1) group by RaceId ) x where x.Horses <= 4) y -- or TipSky = 1 or FormInWet = 1 or FormClass = 1
