USE CCRequesterSetad
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[spHomaReportArea]	
  @aStartDate AS VARCHAR(10)
  ,@aEndDate AS VARCHAR(10)
AS
BEGIN
	DECLARE @lSQLMain VARCHAR(2000)
    ,@lSQL VARCHAR(2000)
    ,@lIsTamir VARCHAR(50) = 'TblJob.IsTamir = 0 AND'
    ,@lTableName VARCHAR(50) = 'TblRequestMP'
  CREATE TABLE #HomaMPNotTamir (AreaId INT, cnt INT)
  CREATE TABLE #HomaMPTamir (AreaId INT, cnt INT)
  CREATE TABLE #HomaLPNotTamir (AreaId INT, cnt INT)
  CREATE TABLE #HomaLPTamir (AreaId INT, cnt INT)
  CREATE TABLE #HomaSubscriber (AreaId INT, cnt INT)
--    CREATE TABLE #HomaSubscriber ( DisconnectDatePersian VARCHAR(10),  AreaId INT, cnt INT)
  SET @lSQLMain ='SELECT TblJob.AreaId,COUNT(*) AS cnt
    	  FROM CCRequesterSetad.Homa.['+ @lTableName +']
    	  INNER JOIN CCRequesterSetad.Homa.TblRequestCommon
    	    ON ['+ @lTableName +'].RequestCommonId=TblRequestCommon.RequestCommonId
    	  INNER JOIN CCRequesterSetad.Homa.TblJob ON TblJob.JobId=TblRequestCommon.JobId
    	  WHERE '+ @lIsTamir +' IsDuplicate=0 AND JobStatusId IN (10,11)
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

  /*TODO*/
  SELECT Area.Area 
    , ISNULL(MPNT.cnt , 0) AS MPNotTamirCount
    , ISNULL(MPT.cnt , 0) AS MPTamirCount
    , ISNULL(LPNT.cnt , 0) AS LPNotTamirCount
    , ISNULL(LPT.cnt , 0) AS LPTamirCount
    FROM Tbl_Area Area
    LEFT JOIN #HomaMPNotTamir MPNT ON Area.AreaId = MPNT.AreaId
    LEFT JOIN #HomaMPTamir MPT ON  Area.AreaId = MPT.AreaId
    LEFT JOIN #HomaLPNotTamir LPNT ON Area.AreaId = LPNT.AreaId
    LEFT JOIN #HomaLPTamir LPT ON Area.AreaId = LPT.AreaId

	DROP TABLE #HomaMPNotTamir
	DROP TABLE #HomaMPTamir
	DROP TABLE #HomaLPNotTamir
	DROP TABLE #HomaLPTamir
	DROP TABLE #HomaSubscriber
END

EXEC dbo.[spHomaReportArea] 
                @aStartDate = '1390/01/01' 
                ,@aEndDate = '1400/01/01'  