SET IDENTITY_INSERT b_tblServiceId ON
GO

DECLARE @WorkingAreaID as int 
select @WorkingAreaID=WorkingAreaID from TblDepartmentInfo 
set @WorkingAreaID=11

DECLARE @query bigint
set @query =(
		SELECT MAX(PrimaryKeyId - CAST(PrimaryKeyId / 1000000000 AS bigint) * 1000000000)+1 AS ID FROM  Tbl_EventLogCenter 
		WHERE      (LEN(PrimaryKeyId) > 10)
		and ((PrimaryKeyId-(PrimaryKeyId %100000000000))/100000000000)=@WorkingAreaID
		and WorkingAreaId=@WorkingAreaID
		and TableNameId in (174,189,190,191,192,193)
	)
select * from Tbl_EventLogCenter 
where (PrimaryKeyId % 100000000000)=@query-1 and WorkingAreaId=@WorkingAreaID
		and 
		TableNameId in (174,189,190,191,192,193)		
print @query
if @query is null
	truncate table b_tblServiceId
else
if not exists(select id from b_tblServiceId where id=@query) and not @query is null
	INSERT INTO b_tblServiceId (Id) VALUES (@query) 
GO
SET IDENTITY_INSERT b_tblServiceId OFF
GO
