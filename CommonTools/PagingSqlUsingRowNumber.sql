
declare @pageSize int
declare @pageIndex int
set @pageSize = 20
set @pageIndex = 2

SELECT * FROM 
(
    SELECT TOP (@pageIndex * @pageSize) ROW_NUMBER() OVER (ORDER BY ID) AS RowNum, * 
    FROM TableName WHERE 1=1
) AS tempTable
WHERE RowNum > (@pageIndex-1)*@pageSize
ORDER BY RowNum;

select count(*) as totalCount from TableName

