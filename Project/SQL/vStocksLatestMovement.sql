IF (OBJECT_ID('[dbo].[vStocksLatestMovement]') IS NOT NULL)
  DROP VIEW [dbo].[vStocksLatestMovement]
GO

CREATE VIEW [dbo].[vStocksLatestMovement]
AS
  select x.*, y.IndexCode [Indices] from
	(
		select
		  s.Id [StockId],
		  s.Code [StockCode],
		  s.Name [StockName],
		  s.Flag1 [Favourite],
		  sdalm.MovementDays,
		  sdalm.MovementDirection,
		  sdalm.StartDate,
		  sdalm.StartDay,
		  sdalm.StartPrice,
		  sdalm.StartIndicator,
		  sdalm.EndDate,
		  sdalm.EndDay,
		  sdalm.EndPrice,
		  sdalm.EndIndicator,
		  sdalm.Changed,
		  sdalm.ChangedPercent,
		  sdalm.OverallChanged,
		  sdalm.OverallChangedPercent,
		  sdalm.MaxPrice,
		  sdalm.MinPrice,
		  sdalm.OverallMaxPrice,
		  sdalm.OverallMinPrice
		from 
		  Stocks s 
		inner join 
		  StockDetailsAggregatedLatestMovement sdalm 
		on 
		  s.Id = sdalm.StockId
	) x
	left join
	(
		select StockId, IndexCode = 
		  stuff((select ', ' + IndexCode
			from (select idst.StockId, id.Code [IndexCode] from IndicesStocks idst left join Indices id on idst.IndexId = id.Id) b 
			where b.StockId = a.StockId
			for xml path('')), 1, 2, ''
		  )
		from (select idst.StockId, id.Code [IndexCode] from IndicesStocks idst left join Indices id on idst.IndexId = id.Id) a
		group by StockId
	) y
	on
	x.StockId = y.StockId
GO