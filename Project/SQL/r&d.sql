select * from vStocks where StockCode = 'ALL' order by BuyIndicator desc, StockCode asc, date desc 
select * from Stocks where Name like '%entertainment%'

select * from vStocks where stockCode = 'WBC' order by date desc
select cast(convert(nvarchar(4), date/10000) + '-' + right('00' + convert(nvarchar(2), (date%10000)/100), 2) + '-' + right('00' + convert(nvarchar(4), (date%100)), 2) + ' 00:00:00' as datetime)[date], Price from vStocks where stockCode = 'WBC' order by date desc

select StockId, max(DateTimeLastModified) [DateTimeLastModified], count(*) [Rows] from [dbo].[StockDetails] group by StockId order by [DateTimeLastModified], [Rows] desc

select * from StockDetails where StockId = 95 order by date desc


--SELECT * FROM vStocks WHERE Code = 'ALL'  order by [Date]

select distinct(StockName) from vStocks where BuyIndicator = 1

select datename(dw, cast(convert(nvarchar(4), 20170814/10000) + '-' + right('00' + convert(nvarchar(2), (20170814%10000)/100), 2) + '-' + right('00' + convert(nvarchar(4), (20170814%100)), 2) + ' 00:00:00' as datetime))

select * from indices

select * from vStocks where stockid = 1 order by date desc


select * from [dbo].[vStocksAggregated]

select * from vStocksLatestMovement where (ChangedPercent >= 2 or ChangedPercent <= -2)  and Favourite = 1

-- update stocks set Flag1 = 1 where Code in ('ANZ', 'A2M', 'BHP', 'ALL', 'CBA', 'BSL', 'FMG', 'RIO', 'NAB', 'WBC', 'MIN', 'SVW', 'KGN', 'BEN', 'CSR', 'CSL', 'ABC', 'SGR', 'WES', 'WOW')
-- update stocks set Flag1 = 1 where Code in (select code from stocks where id in (select distinct(StockId) from StockDetails where Flag1 = 1))

select code from stocks where id in (select distinct(StockId) from StockDetails where Flag1 = 1)

select * from [dbo].[StockDetailsAggregated] 

select * from [dbo].[vStocksLatestMovement] where ChangedPercent > 0.00 and Favourite = 1 order by ChangedPercent 

select * from vStocks where StockCode = 'ANZ' order by date 

select * from vStocksAggregated where StockCode = 'CBA'
select * from vStocksAggregated where StockCode = 'ANZ'
select * from vStocksAggregated where StockCode = 'WBC'
select * from vStocksAggregated where StockCode = 'ALL'

select * from vStocksAggregated where StockCode = 'BSL'
select * from vStocksAggregated where StockCode = 'FMG'

select * from vStocksAggregated where StockCode = 'BHP'
select * from vStocksAggregated where StockCode = 'MIN'
select * from vStocksAggregated where StockCode = 'RIO'

select * from vStocksAggregated where StockCode = 'SDF'
select * from vStocksAggregated where StockCode = 'GXY'
select * from vStocksAggregated where StockCode = 'A2M'
