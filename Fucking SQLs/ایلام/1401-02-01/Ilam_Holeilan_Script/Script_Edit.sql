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

