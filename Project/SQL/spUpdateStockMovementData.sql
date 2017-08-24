IF (OBJECT_ID('[dbo].[spUpdateStockMovementData]') IS NOT NULL)
  DROP PROCEDURE [dbo].[spUpdateStockMovementData]
GO

CREATE PROC [dbo].[spUpdateStockMovementData]
 @FromDate bigint = 20170201
AS
	TRUNCATE TABLE [dbo].[StockDetailsAggregated]

	DECLARE @Code nvarchar(50)
	DECLARE SqlCursor2 CURSOR FOR SELECT Code FROM Stocks order by Id
	OPEN SqlCursor2
	FETCH NEXT FROM SqlCursor2 INTO @Code
	
	WHILE @@FETCH_STATUS = 0   
	BEGIN   
		INSERT INTO [dbo].[StockDetailsAggregated] EXEC [dbo].[spStockMovementOverview2] @Code, @FromDate
		FETCH NEXT FROM SqlCursor2 INTO @Code
	END   

	CLOSE SqlCursor2
	DEALLOCATE SqlCursor2
GO