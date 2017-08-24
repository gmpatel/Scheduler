select * from vStocks where StockCode = 'ALL' order by BuyIndicator desc, StockCode asc, date desc 
select * from vStocks where StockName like 'sev%'

select * from vStocks where stockCode = 'SGR' order by date desc

select StockId, max(DateTimeLastModified) [DateTimeLastModified], count(*) [Rows] from [dbo].[StockDetails] group by StockId order by [DateTimeLastModified], [Rows] desc

select * from StockDetails where StockId = 95 order by date desc

exec spStockMovementOverview 'SVW'
exec [spStockMovementOverview2] 'A2M'


--SELECT * FROM vStocks WHERE Code = 'ALL'  order by [Date]

select distinct(StockName) from vStocks where BuyIndicator = 1

select datename(dw, cast(convert(nvarchar(4), 20170814/10000) + '-' + right('00' + convert(nvarchar(2), (20170814%10000)/100), 2) + '-' + right('00' + convert(nvarchar(4), (20170814%100)), 2) + ' 00:00:00' as datetime))

select * from indices

select * from vStocks where stockid = 1 order by date desc

select * from vStocksAggregated where StockCode = 'A2M'

exec spStockMovementOverview 'RIO'

select * from [dbo].[vStocksAggregated]

select * from vStocksLatestMovement where (ChangedPercent >= 2 or ChangedPercent <= -2)  and Favourite = 1


update stocks set Flag1 = 1 where Code in ('ANZ', 'A2M', 'BHP', 'ALL', 'CBA', 'BSL', 'FMG', 'RIO', 'NAB', 'WBC', 'MIN', 'SVW', 'KGN', 'BEN', 'CSR', 'CSL', 'ABC', 'SGR', 'WES', 'WOW')
update stocks set Flag1 = 1 where Code in (select code from stocks where id in (select distinct(StockId) from StockDetails where Flag1 = 1))

select code from stocks where id in (select distinct(StockId) from StockDetails where Flag1 = 1)

