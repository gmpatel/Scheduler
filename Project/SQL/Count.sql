select count(*) from Product where WS_Status = 1 and WS_CoverTypeId = 1 and WS_OverseasVisitors = 0 and WS_Enabled = 1 and WS_Visible = 1
select count(*) from Product where WS_Status = 1 and WS_CoverTypeId = 2 and WS_OverseasVisitors = 0 and WS_Enabled = 1 and WS_Visible = 1


SELECT 
count(*)
FROM
	(select * from Product where WS_Status = 1 and WS_CoverTypeId = 1 and WS_OverseasVisitors = 0 and WS_Enabled = 1 and WS_Visible = 1) p1
INNER JOIN
	(select * from Product where WS_Status = 1 and WS_CoverTypeId = 2 and WS_OverseasVisitors = 0 and WS_Enabled = 1 and WS_Visible = 1) p2
ON
	p1.WS_Corporate = p2.WS_Corporate and p1.WS_CategoryId = p2.WS_CategoryId and p1.WS_StateId = p2.WS_StateId



