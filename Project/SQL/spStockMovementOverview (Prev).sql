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
	DECLARE @Favourite bit
	DECLARE @Date bigint
	DECLARE @BuyIndicator bit
	DECLARE @Price decimal (18,2)
	DECLARE @Change decimal (18,2)
	DECLARE @ChangePercent decimal (18,2)
	DECLARE @OneYearChangePercent decimal (18,2)
	DECLARE @High decimal (18,2)
	DECLARE @Low decimal (18,2)
	
	DECLARE SqlCursor CURSOR FOR SELECT StockCode, StockName, StockId, Favourite, Date, BuyIndicator, Price, Change, ChangePercent, OneYearChangePercent, High, Low FROM vStocks WHERE StockCode = @StockCode AND [Date] >= @FromDate order by [Date]

	OPEN SqlCursor
	FETCH NEXT FROM SqlCursor INTO @Code, @Name, @StockId, @Favourite, @Date, @BuyIndicator, @Price, @Change, @ChangePercent, @OneYearChangePercent, @High, @Low

	DECLARE @StartDate bigint
	DECLARE @StartDay nvarchar(50)
	DECLARE @StartPrice decimal (18,2)
	DECLARE @StartIndicator bit
	DECLARE @EndDate bigint
	DECLARE @EndDay nvarchar(50)
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
	  Favourite bit,
	  MovementDays bigint,
	  MovementDirection nvarchar(50),
	  StartDate bigint,
	  StartDay nvarchar(50),
	  StartPrice decimal(18,2),
	  StartIndicator bit,
	  EndDate bigint,
	  EndDay nvarchar(50),
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
		  select @StartDay = datename(dw, cast(convert(nvarchar(4), @StartDate/10000) + '-' + right('00' + convert(nvarchar(2), (@StartDate%10000)/100), 2) + '-' + right('00' + convert(nvarchar(4), (@StartDate%100)), 2) + ' 00:00:00' as datetime))
		  SET @StartPrice = @Price
		  SET @StartIndicator = @BuyIndicator
		  SET @EndDate = @Date
		  select @EndDay = datename(dw, cast(convert(nvarchar(4), @EndDate/10000) + '-' + right('00' + convert(nvarchar(2), (@EndDate%10000)/100), 2) + '-' + right('00' + convert(nvarchar(4), (@EndDate%100)), 2) + ' 00:00:00' as datetime))
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
			select @EndDay = datename(dw, cast(convert(nvarchar(4), @EndDate/10000) + '-' + right('00' + convert(nvarchar(2), (@EndDate%10000)/100), 2) + '-' + right('00' + convert(nvarchar(4), (@EndDate%100)), 2) + ' 00:00:00' as datetime))
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
			
			INSERT INTO #StockMovementSummary values (@StockId, @Code, @Name, @Favourite, @Counter - 1, @ChangeIndicator, @StartDate, @StartDay, @StartPrice, @StartIndicator, @EndDate, @EndDay, @EndPrice, @EndIndicator, @TotalChanged, @TotalChangedPercent, @OverallChanged, @OverallChangedPercent, @Max, @Min, @OverallMax, @OverallMin)

			SET @Counter = 1
			SET @StartDate = @Date
			select @StartDay = datename(dw, cast(convert(nvarchar(4), @StartDate/10000) + '-' + right('00' + convert(nvarchar(2), (@StartDate%10000)/100), 2) + '-' + right('00' + convert(nvarchar(4), (@StartDate%100)), 2) + ' 00:00:00' as datetime))
			SET @StartPrice = @EndPrice
			SET @StartIndicator = @BuyIndicator
			SET @EndDate = @Date
			select @EndDay = datename(dw, cast(convert(nvarchar(4), @EndDate/10000) + '-' + right('00' + convert(nvarchar(2), (@EndDate%10000)/100), 2) + '-' + right('00' + convert(nvarchar(4), (@EndDate%100)), 2) + ' 00:00:00' as datetime))
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

		FETCH NEXT FROM SqlCursor INTO @Code, @Name, @StockId, @Favourite, @Date, @BuyIndicator, @Price, @Change, @ChangePercent, @OneYearChangePercent, @High, @Low
		IF @@FETCH_STATUS != 0
		BEGIN
		  SET @NewRowCounter = @NewRowCounter - 1
		  IF @NewRowCounter = 1
		   BEGIN
			SET @StartPrice = @EndPrice + @TotalChanged
		   END

		  INSERT INTO #StockMovementSummary values (@StockId, @Code, @Name, @Favourite, @Counter, @CurrentChangeIndicator, @StartDate, @StartDay, @StartPrice, @StartIndicator, @EndDate, @EndDay, @EndPrice, @EndIndicator, @TotalChanged, @TotalChangedPercent, @OverallChanged, @OverallChangedPercent, @Max, @Min, @OverallMax, @OverallMin)
		END
	END   

	CLOSE SqlCursor
	DEALLOCATE SqlCursor

	SELECT * FROM #StockMovementSummary order by StartDate desc
	DROP TABLE #StockMovementSummary
GO