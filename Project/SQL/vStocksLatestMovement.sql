IF (OBJECT_ID('[dbo].[vStocksLatestMovement]') IS NOT NULL)
  DROP VIEW [dbo].[vStocksLatestMovement]
GO

CREATE VIEW [dbo].[vStocksLatestMovement]
AS
  select 
    sa.* 
  from 
    vStocksAggregated sa 
  inner join 
    (select StockId, max(StartDate)[StartDate] from vStocksAggregated group by StockId) x 
  on 
    sa.StockId = x.StockId and 
	sa.StartDate = x.StartDate
GO