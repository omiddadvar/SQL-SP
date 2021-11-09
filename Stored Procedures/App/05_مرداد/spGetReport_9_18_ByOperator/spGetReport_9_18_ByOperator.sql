
ALTER PROCEDURE spGetReport_9_18_ByOperator 
  @FromDate AS VARCHAR(10)
  ,@ToDate AS VARCHAR(10)
  ,@AreaId AS INT = -1
  ,@OperatorIds AS VARCHAR(MAX)
  ,@IsLight AS BIT = 0
  AS
  BEGIN

    /* enforce filters */
  	DECLARE @lSQL AS VARCHAR(MAX) = 'SELECT TblRequest.RequestId, TblRequest.AreaId FROM TblRequest '
    DECLARE @lWhere AS VARCHAR(MAX) =  ' WHERE TblRequest.DisconnectDatePersian BETWEEN '''+ @FromDate + ''' AND ''' + @ToDate + ''''
    CREATE TABLE #tmp(
        RequestId BIGINT,
        AreaId BIGINT
        )
    IF LEN(@OperatorIds) > 0 BEGIN
      SET @lSQL = @lSQL + 'INNER JOIN TblRequestInfo Info ON TblRequest.RequestId = Info.RequestId '     
      SET @lWhere = @lWhere + ' AND Info.QnsAreaUserId IN (' + @OperatorIds + ')'
    END
    IF @AreaId IS NOT NULL AND @AreaId > -1 BEGIN
    	 SET @lWhere = @lWhere + ' AND TblRequest.AreaId = ' + CAST(@AreaId AS VARCHAR(10))
    END
    IF @IsLight = 1 BEGIN
    	 SET @lWhere = @lWhere + ' AND TblRequest.IsLightRequest = ' + CAST(@IsLight AS VARCHAR(1))
    END
    SET @lSQL = @lSQL + @lWhere
    INSERT INTO #tmp EXEC(@lSQL)

      /*  Main Data :*/
      SELECT R.AreaId ,A.Area , ISNULL(Info.QnsAreaUserId , -1) AS OperatorId
        , COUNT(R.RequestId) AS CntAll
        , COUNT(CASE WHEN R.EndJobStateId NOT IN (1,6) THEN R.RequestId END) AS CntRequest
        , COUNT(CASE WHEN R.EndJobStateId IN (2,3) THEN R.RequestId END) AS CntRequestDone
        , COUNT(CASE WHEN R.EndJobStateId = 6 THEN R.RequestId END) AS CntNotRelated
        , COUNT(CASE WHEN R.EndJobStateId = 6 AND Info.IsRealNotRelated IS NOT NULL THEN R.RequestId END) AS CntNotRelatedControled
        , COUNT(CASE WHEN R.EndJobStateId = 6 AND Info.IsRealNotRelated = 1 THEN R.RequestId END) AS CntRealNotRelated
        , COUNT(CASE WHEN R.EndJobStateId = 1 THEN R.RequestId END) AS CntDuplicated
        , COUNT(CASE WHEN R.EndJobStateId = 1 AND info.IsRealDuplicated = 1 THEN R.RequestId END) AS CntRealDuplicated
        , COUNT(CASE WHEN R.EndJobStateId = 1 AND info.IsRealDuplicated IS NOT NULL THEN R.RequestId END) AS CntDuplicatedControled
        , COUNT(CASE WHEN NOT (info.IsRealNotRelated IS NULL AND Info.OperatorConvsType IS NULL
            AND  Info.IsSaveParts IS NULL AND Info.IsInformTamir IS NULL AND info.IsSaveLight IS NULL) 
            THEN R.RequestId END) AS CntRequestControled
        , COUNT(CASE WHEN info.OperatorConvsType = 1 THEN R.RequestId END) AS OperatorConvsBad
        , COUNT(CASE WHEN info.OperatorConvsType = 2 THEN R.RequestId END) AS OperatorConvsGood
        , COUNT(CASE WHEN info.OperatorConvsType = 3 THEN R.RequestId END) AS OperatorConvsVeryGood
        , COUNT(CASE WHEN info.OperatorConvsType = 4 THEN R.RequestId END) AS OperatorConvsExcellent
        , COUNT(CASE WHEN R.DisconnectInterval <= 20 THEN R.RequestId END) AS CntDCInterval_1
        , COUNT(CASE WHEN R.DisconnectInterval > 20 AND R.DisconnectInterval <= 30 THEN R.RequestId END) AS CntDCInterval_2
        , COUNT(CASE WHEN R.DisconnectInterval >= 60 THEN R.RequestId END) AS CntDCInterval_3
        , COUNT(CASE WHEN Info.IsSaveParts IS NOT NULL THEN R.RequestId END) AS CntSavePartsDoneControled
        , COUNT(CASE WHEN Info.IsSaveParts = 1 THEN R.RequestId END) AS CntSavePartsDone
        , COUNT(CASE WHEN Info.IsSaveLight IS NOT NULL THEN R.RequestId END) AS CntSaveLightDoneControled
        , COUNT(CASE WHEN Info.IsSaveParts = 1 THEN R.RequestId END) AS CntSaveLightDone
        , COUNT(CASE WHEN Info.IsInformTamir IS NOT NULL  THEN R.RequestId END) AS CntInformTamirDoneControled
        , COUNT(CASE WHEN Info.IsInformTamir = 1 THEN R.RequestId END) AS CntInformTamirDone
        , COUNT(CASE WHEN Info.SubscriberOKType = 1 THEN R.RequestId END) AS CntSubscriberOKBad
        , COUNT(CASE WHEN Info.SubscriberOKType = 2 THEN R.RequestId END) AS CntSubscriberOKGood
        , COUNT(CASE WHEN Info.SubscriberOKType = 3 THEN R.RequestId END) AS CntSubscriberOKVeryGood
        , COUNT(CASE WHEN Info.SubscriberOKType = 4 THEN R.RequestId END) AS CntSubscriberOKExcellent
        , COUNT(CASE WHEN NOT (IsRealNotRelated IS NULL) OR NOT (OperatorConvsType IS NULL) OR
        NOT (IsSaveParts IS NULL) OR NOT (IsRealDuplicated IS NULL) THEN R.RequestId END) AS CntAllRequests
        INTO #tempCount FROM TblRequest R
        INNER JOIN #tmp T ON R.RequestId = T.RequestId
        INNER JOIN Tbl_Area A ON R.AreaId = A.AreaId
        LEFT JOIN TblRequestInfo Info ON R.RequestId = Info.RequestId
        GROUP BY R.AreaId, A.Area, Info.QnsAreaUserId
      SELECT #tempCount.* , ISNULL(AU.UserName , '') AS Operator
        , ROUND(ISNULL(CAST(CntRequest AS FLOAT) / NULLIF(CntAll ,0) , 0) * 100 , 2) AS PercentRequestDone 
        , ROUND(ISNULL(CAST(CntNotRelated AS FLOAT) / NULLIF(CntRequest ,0) , 0) * 100 , 2) AS PercenetNotRelated 
        , ROUND(ISNULL(CAST(CntRealNotRelated AS FLOAT) / NULLIF(CntNotRelatedControled ,0) , 0) * 100 , 2) AS PercentRealNotRelated
        , ROUND(ISNULL(CAST(CntDuplicated AS FLOAT) / NULLIF(CntRequest ,0) , 0) * 100 , 2) AS PercenetDuplicated
        , ROUND(ISNULL(CAST(CntRealDuplicated AS FLOAT) / NULLIF(CntDuplicatedControled ,0) , 0) * 100 , 2) AS PercentRealDuplicated
        , ROUND(ISNULL(CAST(CntSavePartsDone AS FLOAT) / NULLIF(CntSavePartsDoneControled ,0) , 0) * 100 , 2) AS PrcentSavePartsDone
        , ROUND(ISNULL(CAST(CntSaveLightDone AS FLOAT) / NULLIF(CntSaveLightDoneControled ,0) , 0) * 100 , 2) AS PrcentSaveLightDone
        , ROUND(ISNULL(CAST(CntInformTamirDone AS FLOAT) / NULLIF(CntInformTamirDoneControled ,0) , 0) * 100 , 2) AS PrcentInformTamirDone
        , 0 AS ScorePercentRequestDone, 0 AS ScoreDCINTerval, 0 AS ScoreSavePartsDoneControled
        , 0 AS ScoreOperatorConvs, 0 AS ScorePrcentInformTamirDone, 0 AS ScoreNotRelated
        , CAST('' AS VARCHAR(MAX)) AS RequestNumbers
        FROM #tempCount 
        LEFT JOIN Tbl_AreaUser AU ON #tempCount.OperatorId = AU.AreaUserId
        ORDER BY #tempCount.AreaId
        
      DROP TABLE #tempCount
      DROP TABLE #tmp
  END

