/*create proc dbo.spGreen_Zanjan @MediaTypeId int,
	@AreaIDs as nvarchar(500),
	@FromDate as nvarchar(10),
	@ToDate as nvarchar(10),
	@ExternalServiceIDs as nvarchar(500),
	@EndJobStateIDs as nvarchar(500)
as
begin
end
*/
select top(5) * from TblRequest
go
select top(5) * from TblRequestData
go
select * from Tbl_ExternalService order by ExternalServiceId asc
go
select * from Tbl_MediaType order by MediaTypeId asc
go
select top(10) * from Tbl_GISSMSRecevied
go
select top(5) * from TblRequest
go
select data.MediaTypeId , data.ExternalServiceId , erja.* from TblErjaRequest erja
	inner join TblRequest req on req.RequestId = erja.RequestId
	left JOIN TblRequestData data on data.RequestId = req.RequestId
	where req.RequestNumber = 99992349
go
select * from Tbl_ErjaState order by ErjaStateId asc
go	
select * from Tbl_EndJobState order by EndJobStateId asc
go
select top(100) * from TblErjaRequest

USE [CcRequesterSetad]
GO
exec spGetReport_14_4 0,'10,2,3,7,5,13,8,4,6,15,11,12,14,9','1399/01/21','1400/01/21','' , 1

--execute spAAAAa @MediaTypeId = 5 ,
--	@AreaIDs = '2,3,5',
--	@FromDate = '1399/04/05',
--	@ToDate = '1400/01/01',
--	@ExternalServices = ''
--	@EndJobStateIDs = '1,2'

exec spGetReport_14_4 0,'10,2,3,7,5,13,8,4,6,15,11,12,14,9','1400/02/29','1400/02/29',''
exec [spGetReport_Sabz_Erja] 0,'','1400/02/29','1400/02/29',''

exec [spGetReport_Sabz_Erja] 0,'','1399/02/29','1400/02/29',''

select * from TblRequest where DisconnectDatePersian  = '1400/02/29'
select * from TblErjaRequest where RequestId = 9900000000001010

select * from TblRequestData where RequestId  = 9900000000001010

insert into TblRequestData values (9900000000001010,5,NULL,NULL)

select * from Tbl_EndJobState order by EndJobStateId

select AVG(case when 1 = 2 then 1 else 0 end)