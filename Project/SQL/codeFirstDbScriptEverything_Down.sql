IF (OBJECT_ID('[dbo].[spStockMovementOverview]') IS NOT NULL)
  DROP PROCEDURE [dbo].[spStockMovementOverview]
GO

IF (OBJECT_ID('[dbo].[spUpdateStockMovementData]') IS NOT NULL)
  DROP PROCEDURE [dbo].[spUpdateStockMovementData]
GO

IF (OBJECT_ID('[dbo].[vStocks]') IS NOT NULL)
  DROP VIEW [dbo].[vStocks]
GO

IF (OBJECT_ID('[dbo].[vStocksAggregated]') IS NOT NULL)
  DROP VIEW [dbo].[vStocksAggregated]
GO

IF (OBJECT_ID('[dbo].[vStocksLatestMovement]') IS NOT NULL)
  DROP VIEW [dbo].[vStocksLatestMovement]
GO