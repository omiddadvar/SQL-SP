
Select * From Tbl_LPTrans Where SerialNumber = '643182'

Select * From TblLPPostTrans Where LPTransId = 990131172

Select * From Tbl_TableName Where TableName = 'TblLPPostTrans'



------------------------------------------------------------------


Delete From TblLPPostTrans Where LPPostTransId = 140159056

INSERT INTO Tbl_EventLogCenter (
	TableName
	,TableNameId
	,PrimaryKeyId
	,Operation
	,AreaId
	,WorkingAreaId
	,DataEntryDT
	)
VALUES (
	'TblLPPostTrans'
	,205
	,140159056
	,1
	,NULL
	,99
	,GETDATE()
	)
