
select * from Venues where Name = 'WARWICK FARM'
select * from Meetings where VenueId = 9
select * from Races where MeetingId = 9
select * from Runners where RaceId in (select Id from Races where MeetingId = 3)

Select * from (Select RaceId, count(*)[Horses] From Runners where (FormBestOverall = 1) group by RaceId ) x where x.Horses <= 4



select * from
(
	select 
	  v.Name [Venue], v.Province, m.Date, r.Number [RaceNumber], r.Name [RaceName], r.Time, z.Numbers [Form], zz.Numbers [Tip]
	from 
	  Races r 
	inner join
	  Meetings m
	On
	  r.MeetingId = m.Id
	inner join
	  Venues v
	On
	  m.VenueId = v.Id
	inner join 
	  (
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
	  ) y -- or TipSky = 1 or FormInWet = 1 or FormClass = 1
	On
	  r.Id = y.RaceId
	inner join
	  (SELECT RaceId, Numbers = 
		STUFF((SELECT ', ' + convert(nvarchar(5), Number)
			   FROM Runners b 
			   WHERE b.RaceId = a.RaceId and (FormBestOverall = 1) order by isnull(FormBestOverallPosition, 999) -- or TipSky = 1 or FormInWet = 1 or FormClass = 1
			  FOR XML PATH('')), 1, 2, '')
	  FROM Runners a GROUP BY RaceId) z
	On
	  r.Id = z.RaceId
	inner join
	  (SELECT RaceId, Numbers = 
		STUFF((SELECT ', ' + convert(nvarchar(5), Number)
			   FROM Runners b 
			   WHERE b.RaceId = a.RaceId and (TipSky = 1) order by isnull(TipSkyPosition, 999) -- or TipSky = 1 or FormInWet = 1 or FormClass = 1
			  FOR XML PATH('')), 1, 2, '')
	  FROM Runners a GROUP BY RaceId) zz
	On
	  r.Id = zz.RaceId
) r
where r.Date = '20170921' and r.Province in ('NSW', 'VIC')  order by r.Venue, r.Province, r.RaceNumber