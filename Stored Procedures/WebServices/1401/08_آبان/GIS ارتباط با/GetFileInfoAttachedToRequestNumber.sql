--select *
--from TblRequestFile
--where RequestId=9900000000000613

ALTER Procedure GetFileInfoAttachedToRequestNumber @RequestNumber bigint=98992318
AS
BEGIN
declare @RequestId bigint 

select @RequestId=RequestId
from TblRequest
where RequestNumber=@RequestNumber


select FileServerId
INTO #tempFileServerIds
from TblRequestFile
where RequestId=@RequestId

select TblFileServer.FileServerId,FileId,[FileName],FileSize,UserName
from TblFileServer
inner join #tempFileServerIds on TblFileServer.FileServerId=#tempFileServerIds.FileServerId
left join Tbl_AreaUser on TblFileServer.AreaUserId=Tbl_AreaUser.AreaUserId


drop table #tempFileServerIds


END