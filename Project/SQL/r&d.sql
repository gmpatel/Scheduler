select * from vStocks where StockCode = 'ALL' order by BuyIndicator desc, StockCode asc, date desc 
select * from vStocks where StockName like 'the star ent%'

select * from vStocks where stockCode = 'SGR' order by date desc

select StockId, max(DateTimeLastModified) [DateTimeLastModified], count(*) [Rows] from [dbo].[StockDetails] group by StockId order by [DateTimeLastModified], [Rows] desc
select * from StockDetails where StockId = 95 order by date desc

exec spStockMovementOverview 'A2M'
select * from vStocks where StockCode = 'MIN' order by date desc

--SELECT * FROM vStocks WHERE Code = 'ALL'  order by [Date]

select distinct(StockName) from vStocks where BuyIndicator = 1

select datename(dw, cast(convert(nvarchar(4), 20170814/10000) + '-' + right('00' + convert(nvarchar(2), (20170814%10000)/100), 2) + '-' + right('00' + convert(nvarchar(4), (20170814%100)), 2) + ' 00:00:00' as datetime))

select * from indices

select * from vStocks where stockid = 1 order by date desc
