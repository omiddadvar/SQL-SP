-----------------Setad------------------
select * from Tbl_Area where AreaId = 17

select * from Tbl_AreaInfo where AreaId = 17

select MPRequestId,* from TblRequest where RequestNumber = 400992589564 and RequestId = 9900000001283455

select * from TblMPRequest where MPRequestId = 9900000001283455

select * from TblMPRequestKey where MPRequestId = 9900000001283455

select * from Tbl_EventLogCenter 
	where TableName = 'TblMPRequestKey' 
		and PrimaryKeyId = 99000000015960
		
insert into Tbl_EventLogCenter (TableName,TableNameId ,PrimaryKeyId,Operation
		,AreaId,WorkingAreaId,DataEntryDT) 
		select TableName,TableNameId ,PrimaryKeyId,Operation
		,AreaId,WorkingAreaId,GETDATE() as DataEntryDT 
from Tbl_EventLogCenter where EventId = 297355715




------------------Prianshahr----------------
select * from Tbl_EventLogCenter 
	where TableName = 'TblMPRequestKey' 
		and PrimaryKeyId = 99000000015960
		and AreaId = 17

select * from TblDepartmentInfo

select * from TblMPRequest where MPRequestId = 9900000001283455

select * from TblMPRequestKey where MPRequestKeyId = 99000000015960

