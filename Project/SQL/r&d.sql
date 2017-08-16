select * from vStocks where Code = 'ALL' order by BuyIndicator desc, code asc, date desc 

select * from vStocks where stockid in (427, 470)

select StockId, count(*) [Rows] from [dbo].[StockDetails] group by StockId order by [Rows] desc

update StockDetails set Flag1 = 1 where StockId in (select distinct(stockId) from StockDetails where Flag1 = 1) and Flag1 = 0


exec spStockMovementOverview 'CBA'
--SELECT * FROM vStocks WHERE Code = 'ALL'  order by [Date]

   

select datename(dw, cast(convert(nvarchar(4), 20170814/10000) + '-' + right('00' + convert(nvarchar(2), (20170814%10000)/100), 2) + '-' + right('00' + convert(nvarchar(4), (20170814%100)), 2) + ' 00:00:00' as datetime))

select * from indices