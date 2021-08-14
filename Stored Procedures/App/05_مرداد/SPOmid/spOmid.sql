DECLARE @FromDate AS VARCHAR(10) = '1398/01/01'
DECLARE @ToDate AS VARCHAR(10) = '1400/05/01'
DECLARE @AreaId AS VARCHAR(10) = '3'
DECLARE @lSQL AS VARCHAR(500) = 'SELECT RequestId, AreaId FROM TblRequest ' +
  'WHERE DisconnectDatePersian BETWEEN '''+ @FromDate + ''' AND ''' + @ToDate + ''''
CREATE TABLE #tmp(
  RequestId BIGINT,
  AreaId BIGINT
  )

IF @AreaId IS NOT NULL OR @AreaId > -1 BEGIN
	 SET @lSQL = @lSQL + ' AND AreaId = ' + @AreaId
END
INSERT INTO #tmp EXEC(@lSQL)
/*To Do :*/
SELECT *,
  FROM TblRequest R
  INNER JOIN #tmp T ON R.RequestId = T.RequestId
  LEFT JOIN TblRequestInfo Info ON R.RequestId = Info.RequestId

DROP TABLE #tmp
