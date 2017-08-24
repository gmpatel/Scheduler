IF (OBJECT_ID('[dbo].[vStocksAggregated]') IS NOT NULL)
  DROP VIEW [dbo].[vStocksAggregated]
GO

CREATE VIEW [dbo].[vStocksAggregated]
AS
	select x.*, y.IndexCode [Indices] from
	(
		select
		  s.Id [StockId],
		  s.Code [StockCode],
		  s.Name [StockName],
		  s.Flag1 [Favourite],
		  sda.MovementDays,
		  sda.MovementDirection,
		  sda.StartDate,
		  sda.StartDay,
		  sda.StartPrice,
		  sda.StartIndicator,
		  sda.EndDate,
		  sda.EndDay,
		  sda.EndPrice,
		  sda.EndIndicator,
		  sda.Changed,
		  sda.ChangedPercent,
		  sda.OverallChanged,
		  sda.OverallChangedPercent,
		  sda.MaxPrice,
		  sda.MinPrice,
		  sda.OverallMaxPrice,
		  sda.OverallMinPrice
		from 
		  Stocks s 
		inner join 
		  StockDetailsAggregated sda 
		on 
		  s.Id = sda.StockId
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