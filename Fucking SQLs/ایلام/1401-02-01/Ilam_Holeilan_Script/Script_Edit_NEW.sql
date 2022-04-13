DECLARE @AreaId AS INT = 12


UPDATE TblRequest
  SET AreaId = @AreaId
  FROM
    TblRequest R
    INNER JOIN __Tbl_Holeilan T ON R.RequestId = T.RequestId
  WHERE T.Type = 'PMP'

UPDATE TblMPRequest
  SET AreaId = @AreaId
  FROM
    TblMPRequest MPR
    INNER JOIN TblRequest R ON MPR.MPRequestId = R.MPRequestId
    INNER JOIN __Tbl_Holeilan T ON R.RequestId = T.RequestId
  WHERE T.IsMP = 1 AND T.Type = 'PMP'

/*-----------------Replicate---------------*/

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
	@AreaId AS AreaId,
	99 AS WorkingAreaId,
	GETDATE() AS DataEntryDT
FROM 
	__Tbl_Holeilan
  WHERE Type = 'PMP'


----------------MP------------------
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
	'TblMPRequest' AS TableName,
	26 AS TableNameId,
	MPR.MPRequestId AS PrimaryKeyId,
	3 AS Operation,
	@AreaId AS AreaId,
	99 AS WorkingAreaId,
	GETDATE() AS DataEntryDT
FROM 
	__Tbl_Holeilan H
  INNER JOIN TblRequest R ON H.RequestId = R.RequestId
  INNER JOIN TblMPRequest MPR ON R.MPRequestId = MPR.MPRequestId
WHERE H.IsMP = 1 AND H.Type = 'PMP'

