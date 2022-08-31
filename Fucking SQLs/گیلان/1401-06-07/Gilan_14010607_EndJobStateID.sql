

SELECT EndJobStateId,* 
From TblRequest 
Where RequestNumber = 40101261382

Select EndJobStateId,* 
From TblLPRequest
WHERE LPRequestId = 100000994640541


Select * From Tbl_AreaUser Where AreaUserId IN( 10016753 , 991292830)


Select R.RequestId, R.RequestNumber , R.DisconnectDatePersian , R.EndJobStateId , LP.EndJobStateId 
From TblRequest R
Inner Join TblLPRequest LP On R.LPRequestId = LP.LPRequestId
Where R.EndJobStateId <> LP.EndJobStateId
And R.DisconnectDatePersian >= '1400/01/01'
AND R.EndJobStateId = 5
Order By R.DisconnectDT DESC


-----------------
Select * 
From Tbl_EventLogCenter
Where TableName = 'TblRequest'
AND PrimaryKeyId = 100000994640541
ORder By DataEntryDT DESC




/*-----------------------------Solution-----------------------------*/
----1700000000352807 AND 100000994640541
UPDATE TblRequest SET EndJobStateId = 2 WHERE RequestId = 100000994640541

INSERT INTO Tbl_EventLogCenter (
	TableName
	,TableNameId
	,PrimaryKeyId
	,Operation
	,AreaId
	,WorkingAreaId
	,DataEntryDT
	,SQLCommand
	,AreaUserId
	)
VALUES (
	'TblRequest'
	,115
	,100000994640541
	,3
	,55
	,99
	,GETDATE()
	,NULL
	,NULL
	);
