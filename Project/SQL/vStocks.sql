IF (OBJECT_ID('[dbo].[vStocks]') IS NOT NULL)
  DROP VIEW [dbo].[vStocks]
GO

CREATE VIEW [dbo].[vStocks]
AS
	select x.*, y.IndexCode [Indices] from
	(
		select 
		  s.Code,
		  s.Name,
		  s.Id [StockId],
		  sd.Date,
		  sd.Flag1 [BuyIndicator],
		  sd.Price,
		  sd.Change,
		  sd.ChangePercent,
		  sd.OneYearChange [OneYearChangePercent],
		  sd.High,
		  sd.Low,
		  sd.Volume,
		  sd.MarketCapital
		from 
		  Stocks s 
		inner join 
		  StockDetails sd 
		on 
		  s.Id = sd.StockId
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