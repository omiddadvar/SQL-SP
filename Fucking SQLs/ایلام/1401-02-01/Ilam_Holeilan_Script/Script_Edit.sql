CREATE TABLE #tmp
(
	RequestId BIGINT UNIQUE,
  AreaId INT,
  IsMP BIT,
  Type VARCHAR(3) NULL
)

DECLARE @AreaId AS INT = 12

UPDATE TblRequest
  SET AreaId = @AreaId
  FROM
    TblRequest R
    INNER JOIN #tmp T ON R.RequestId = T.RequestId

UPDATE TblMPRequest
  SET AreaId = @AreaId
  FROM
    TblMPRequest MPR
    INNER JOIN TblRequest R ON MPR.MPRequestId = R.MPRequestId
    INNER JOIN #tmp T ON R.RequestId = T.RequestId
  WHERE T.IsMP = 1

  
UPDATE TblLPRequest
  SET AreaId = @AreaId
  FROM
    TblLPRequest LPR
    INNER JOIN TblRequest R ON LPR.LPRequestId = R.LPRequestId
    INNER JOIN #tmp T ON R.RequestId = T.RequestId
  WHERE T.IsMP = 0

/*-----------------Replicate---------------*/
SELECT * from Tbl_TableName where TableName = 'TblRequest'

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
WHERE H.IsMP = 1


----------------LP------------------
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
	'TblLPRequest' AS TableName,
	21 AS TableNameId,
	LPR.LPRequestId AS PrimaryKeyId,
	3 AS Operation,
	@AreaId AS AreaId,
	99 AS WorkingAreaId,
	GETDATE() AS DataEntryDT
FROM 
	__Tbl_Holeilan H
  INNER JOIN TblRequest R ON H.RequestId = R.RequestId
  INNER JOIN TblLPRequest LPR ON R.LPRequestId = LPR.LPRequestId
WHERE H.IsMP = 0

