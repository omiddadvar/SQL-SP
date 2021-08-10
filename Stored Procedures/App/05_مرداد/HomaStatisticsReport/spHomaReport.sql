USE CCRequesterSetad
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[spHomaReportArea]	
  @aAreaIDs AS VARCHAR(100)
  ,@aStartDate AS VARCHAR(10)
  ,@aEndDate AS VARCHAR(10)
AS
BEGIN
	DECLARE @lSQLMain VARCHAR(2000)
    ,@lSQL VARCHAR(2000)
    ,@lIsTamir VARCHAR(50) = 'TblJob.IsTamir = 0 AND'
    ,@lTableName VARCHAR(50) = 'TblRequestMP'
  CREATE TABLE #tmpArea (AreaId INT)
  CREATE TABLE #HomaMPNotTamir (AreaId INT, cnt INT)
  CREATE TABLE #HomaMPTamir (AreaId INT, cnt INT)
  CREATE TABLE #HomaLPNotTamir (AreaId INT, cnt INT)
  CREATE TABLE #HomaLPTamir (AreaId INT, cnt INT)
  CREATE TABLE #HomaSubscriber (AreaId INT, cnt INT)
  INSERT INTO #tmpArea EXEC('SELECT AreaId FROM Tbl_Area WHERE AreaId IN ('+ @aAreaIDs +')')
  SET @lSQLMain ='SELECT TblJob.AreaId,COUNT(*) AS cnt
    	  FROM CCRequesterSetad.Homa.['+ @lTableName +']
    	  INNER JOIN CCRequesterSetad.Homa.TblRequestCommon
    	    ON ['+ @lTableName +'].RequestCommonId=TblRequestCommon.RequestCommonId
    	  INNER JOIN CCRequesterSetad.Homa.TblJob ON TblJob.JobId=TblRequestCommon.JobId
    	  WHERE '+ @lIsTamir +' IsDuplicate=0 AND JobStatusId IN (10,11)
          AND TblJob.AreaId IN ('+ @aAreaIDs +')
          AND TblJob.DisconnectDatePersian BETWEEN '''+ @aStartDate +''' AND '''+ @aEndDate + ''' 
    	  GROUP BY TblJob.AreaId'
  INSERT INTO #HomaMPNotTamir EXEC(@lSQLMain)

  SET @lSQL = REPLACE(@lSQLMain, @lIsTamir , 'TblJob.IsTamir = 1 AND')
  INSERT INTO #HomaMPTamir EXEC(@lSQL)

  SET @lSQL = REPLACE(@lSQLMain, @lTableName , 'TblRequestLP')
  INSERT INTO #HomaLPNotTamir EXEC(@lSQL)

  SET @lSQL = REPLACE(@lSQLMain, @lIsTamir , 'TblJob.IsTamir = 1 AND')
  SET @lSQL = REPLACE(@lSQL, @lTableName , 'TblRequestLP')
  INSERT INTO #HomaLPTamir EXEC(@lSQL)

  SET @lSQL = REPLACE(@lSQLMain, @lIsTamir , '')
  SET @lSQL = REPLACE(@lSQL, @lTableName , 'TblRequestSubscriber')
  INSERT INTO #HomaSubscriber EXEC(@lSQL)

  SELECT AreaId,
		SUM(TblTraceSummary.TraceLen) AS TraceLen,
		SUM(TblTraceSummary.OnCallHour) AS OnCallHour,
		COUNT(*) AS OnCallCount
	INTO #tmpTrace
  	FROM CcRequesterSetad.Homa.TblTraceSummary
  	WHERE TargetDatePersian BETWEEN @aStartDate AND @aEndDate	
  	GROUP BY AreaId
  /*TODO*/
  SELECT Area.Area 
    , ISNULL(MPNT.cnt , 0) AS MPNotTamirCount
    , ISNULL(MPT.cnt , 0) AS MPTamirCount
    , ISNULL(LPNT.cnt , 0) AS LPNotTamirCount
    , ISNULL(LPT.cnt , 0) AS LPTamirCount
    , ISNULL(ROUND(TR.TraceLen/1000,2) , 0) AS TraceLen
    , ISNULL(ROUND(OnCallHour,2) , 0) AS OnCallHour
    , ISNULL(TR.OnCallCount , 0) AS OnCallCount
    FROM Tbl_Area Area
    INNER JOIN #tmpArea A ON Area.AreaId = A.AreaId
    LEFT JOIN #HomaMPNotTamir MPNT ON Area.AreaId = MPNT.AreaId
    LEFT JOIN #HomaMPTamir MPT ON  Area.AreaId = MPT.AreaId
    LEFT JOIN #HomaLPNotTamir LPNT ON Area.AreaId = LPNT.AreaId
    LEFT JOIN #HomaLPTamir LPT ON Area.AreaId = LPT.AreaId
    LEFT JOIN #tmpTrace TR ON Area.AreaId = TR.AreaId

	DROP TABLE #HomaMPNotTamir
	DROP TABLE #HomaMPTamir
	DROP TABLE #HomaLPNotTamir
	DROP TABLE #HomaLPTamir
	DROP TABLE #HomaSubscriber
  DROP TABLE #tmpTrace
  DROP TABLE #tmpArea
END


EXEC dbo.[spHomaReportArea] '2,3,4' , '1390/01/01', '1400/01/01' 


