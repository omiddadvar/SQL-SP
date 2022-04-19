
SELECT R.RequestId , R.MPRequestId , MP.EndJobStateId , R.AreaId
  INTO __Tbl_EndJobState_Diff
  FROM TblRequest R
    INNER JOIN TblMPRequest MP ON R.MPRequestId = MP.MPRequestId
  WHERE MP.EndJobStateId IN (4,5) AND R.EndJobStateId IN (2,3)


UPDATE TblRequest SET EndJobStateId = T.EndJobStateId
  FROM TblRequest R
    INNER JOIN __Tbl_EndJobState_Diff T ON R.RequestId = T.RequestId


/*-------------Replicate--------------*/
INSERT INTO Tbl_EventLogCenter (
	TableName,
	TableNameId,
	PrimaryKeyId,
	Operation,
	AreaId,
	WorkingAreaId,
	DataEntryDT
	)
SELECT
	'TblRequest' AS TableName,
	115 AS TableNameId,
	RequestId AS PrimaryKeyId,
	3 AS Operation,
	AreaId AS AreaId,
	99 AS WorkingAreaId,
	GETDATE() AS DataEntryDT
FROM 
	__Tbl_EndJobState_Diff


/*

SELECT EndJobStateId,* FROM  TblRequest WHERE RequestId = 9900000000001532

UPDATE TblRequest SET EndJobStateId = 2 WHERE RequestId = 9900000000001532

*/