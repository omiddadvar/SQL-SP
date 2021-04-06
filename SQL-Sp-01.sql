create proc dbo.spGreen_Zanjan @MediaTypeId int,
	@AreaIDs as nvarchar(500),
	@FromDate as nvarchar(10),
	@ToDate as nvarchar(10),
	@ExternalServiceIDs as nvarchar(500),
	@EndJobStateIDs as nvarchar(500)
as
begin
end

select top(10) * from TblRequest
go
select * from Tbl_ExternalService order by ExternalServiceId asc
go
select * from Tbl_ErjaState order by ErjaStateId asc
go
select * from Tbl_EndJobState order by EndJobStateId asc
go
select * from Tbl_MediaType order by MediaTypeId asc

	
execute spAAAAa @MediaTypeId = 5 ,
	@AreaIDs = '2,3,5',
	@FromDate = '1399/04/05',
	@ToDate = '1400/01/01',
	@ExternalServiceIDs = '1,2',
	@EndJobStateIDs = '1,2'