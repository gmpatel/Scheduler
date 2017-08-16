IF (OBJECT_ID('[dbo].[spStockMovementOverview]') IS NOT NULL)
  DROP PROCEDURE [dbo].[spStockMovementOverview]
GO

CREATE PROC [dbo].[spStockMovementOverview]
 @StockCode nvarchar(50),
 @FromDate bigint = 20170201
AS
	DECLARE @Code nvarchar(50)
	DECLARE @Name nvarchar(50)
	DECLARE @StockId bigint
	DECLARE @Date bigint
	DECLARE @BuyIndicator bit
	DECLARE @Price decimal (18,2)
	DECLARE @Change decimal (18,2)
	DECLARE @ChangePercent decimal (18,2)
	DECLARE @OneYearChangePercent decimal (18,2)
	DECLARE @High decimal (18,2)
	DECLARE @Low decimal (18,2)
	
	DECLARE SqlCursor CURSOR FOR SELECT Code, Name, StockId, Date, BuyIndicator, Price, Change, ChangePercent, OneYearChangePercent, High, Low FROM vStocks WHERE Code = @StockCode AND [Date] >= @FromDate order by [Date]

	OPEN SqlCursor
	FETCH NEXT FROM SqlCursor INTO @Code, @Name, @StockId, @Date, @BuyIndicator, @Price, @Change, @ChangePercent, @OneYearChangePercent, @High, @Low

	DECLARE @StartDate bigint
	DECLARE @StartPrice decimal (18,2)
	DECLARE @StartIndicator bit
	DECLARE @EndDate bigint
	DECLARE @EndPrice decimal (18,2)
	DECLARE @EndIndicator bit
	DECLARE @TotalChanged decimal (18,2)
	DECLARE @TotalChangedPercent decimal (18,2)
	DECLARE @OverallChanged decimal (18,2)
	DECLARE @OverallChangedPercent decimal (18,2)
	DECLARE @OverallMax decimal (18,2)
	DECLARE @OverallMin decimal (18,2)
	DECLARE @Max decimal (18,2)
	DECLARE @Min decimal (18,2)
	DECLARE @ChangeIndicator nvarchar(50)

	DECLARE @Counter bigint
	SET @Counter = 0

	DECLARE @NewRowCounter int
	SET @NewRowCounter = 0

	Create Table #StockMovementSummary (
	  SotckId bigint,
	  StockCode nvarchar(50),
	  StockName nvarchar(100),
	  MovementDays bigint,
	  MovementDirection nvarchar(50),
	  StartDate bigint,
	  StartPrice decimal(18,2),
	  StartIndicator bit,
	  EndDate bigint,
	  EndPrice decimal(18,2),
	  EndIndicator bit,
	  Changed decimal(18,2),
	  ChangedPercent decimal(18,2),
	  OverallChanged decimal(18,2),
	  OverallChangedPercent decimal(18,2),
	  MaxPrice decimal(18,2),
	  MinPrice decimal(18,2),
	  OverallMaxPrice decimal(18,2),
	  OverallMinPrice decimal(18,2)
	)

	WHILE @@FETCH_STATUS = 0   
	BEGIN   
		SET @Counter = @Counter + 1

		IF @Counter = 1
		 BEGIN
		  SET @StartDate = @Date
		  SET @StartPrice = @Price
		  SET @StartIndicator = @BuyIndicator
		  SET @EndDate = @Date
		  SET @EndPrice = @Price
		  SET @EndIndicator = @BuyIndicator
		  SET @TotalChanged = @Change
		  SET @TotalChangedPercent = @ChangePercent
		  SET @OverallChanged = @Change
		  SET @OverallChangedPercent = @ChangePercent
		  SET @Max = @High
		  SET @Min = @Low
		  SET @OverallMax = @High
		  SET @OverallMin = @Low
	  
		  IF @Change < 0
		   SET @ChangeIndicator = 'Down'
		  ELSE
		   SET @ChangeIndicator = 'Up'
		 END
		ELSE
		 BEGIN
		  DECLARE @CurrentChangeIndicator nvarchar(50)
	  
		  IF @Change < 0
		   SET @CurrentChangeIndicator = 'Down'
		  ELSE
		   SET @CurrentChangeIndicator = 'Up'
	  
		  IF @ChangeIndicator = @CurrentChangeIndicator
		  BEGIN
			SET @EndDate = @Date
			SET @EndPrice = @Price
			SET @EndIndicator = @BuyIndicator
			SET @TotalChanged = @TotalChanged + @Change
			SET @TotalChangedPercent = @TotalChangedPercent + @ChangePercent
			SET @OverallChanged = @OverallChanged + @Change
			SET @OverallChangedPercent = @OverallChangedPercent + @ChangePercent

			IF @High > @Max
			 SET @Max = @High

			IF @Low < @Min
			 SET @Min = @Low

			IF @High > @OverallMax
			 SET @OverallMax = @High

			IF @Low < @OverallMin
			 SET @OverallMin = @Low
		  END
		  ELSE
		  BEGIN
			SET @NewRowCounter = @NewRowCounter + 1
			IF @NewRowCounter = 1
			 BEGIN
			  SET @StartPrice = @EndPrice - @TotalChanged
			 END
			
			INSERT INTO #StockMovementSummary values (@StockId, @Code, @Name, @Counter - 1, @ChangeIndicator, @StartDate, @StartPrice, @StartIndicator, @EndDate, @EndPrice, @EndIndicator, @TotalChanged, @TotalChangedPercent, @OverallChanged, @OverallChangedPercent, @Max, @Min, @OverallMax, @OverallMin)

			SET @Counter = 1
			SET @StartDate = @Date
			SET @StartPrice = @EndPrice
			SET @StartIndicator = @BuyIndicator
			SET @EndDate = @Date
			SET @EndPrice = @Price
			SET @EndIndicator = @BuyIndicator
			SET @TotalChanged = @Change
			SET @TotalChangedPercent = @ChangePercent
			SET @OverallChanged = @OverallChanged + @Change
			SET @OverallChangedPercent = @OverallChangedPercent + @ChangePercent
			SET @Max = @High
			SET @Min = @Low
			SET @ChangeIndicator = @CurrentChangeIndicator

			IF @High > @OverallMax
			 SET @OverallMax = @High

			IF @Low < @OverallMin
			 SET @OverallMin = @Low
		  END
		 END

		FETCH NEXT FROM SqlCursor INTO @Code, @Name, @StockId, @Date, @BuyIndicator, @Price, @Change, @ChangePercent, @OneYearChangePercent, @High, @Low
		IF @@FETCH_STATUS != 0
		BEGIN
		  SET @NewRowCounter = @NewRowCounter - 1
		  IF @NewRowCounter = 1
		   BEGIN
			SET @StartPrice = @EndPrice + @TotalChanged
		   END

		  INSERT INTO #StockMovementSummary values (@StockId, @Code, @Name, @Counter, @CurrentChangeIndicator, @StartDate, @StartPrice, @StartIndicator, @EndDate, @EndPrice, @EndIndicator, @TotalChanged, @TotalChangedPercent, @OverallChanged, @OverallChangedPercent, @Max, @Min, @OverallMax, @OverallMin)
		END
	END   

	CLOSE SqlCursor
	DEALLOCATE SqlCursor

	SELECT * FROM #StockMovementSummary order by StartDate desc
	DROP TABLE #StockMovementSummary
GO