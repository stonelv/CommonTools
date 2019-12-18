select * from T_DT_Orders
select * from [dbo].[T_DT_Logs] where [DTOrderID]=5

declare @pageSize int
declare @pageIndex int
set @pageSize = 20
set @pageIndex = 2

SELECT * FROM 
(
    SELECT TOP (@pageIndex * @pageSize) ROW_NUMBER() OVER (ORDER BY ID) AS RowNum, * 
    FROM T_DT_Orders WHERE 1=1
) AS tempTable
WHERE RowNum > (@pageIndex-1)*@pageSize
ORDER BY RowNum;

select count(*) as totalCount from T_DT_Orders

