
ALTER PROCEDURE dbo.spGetReport_6_2 
	@aStartDatePersian AS VARCHAR(12),
	@aEndDatePersian AS VARCHAR(12),
	@aFromDateService as VARCHAR(12),
	@aToDateService as VARCHAR(12),
	@aAreaIds AS VARCHAR(100),
	@aPriorities AS VARCHAR(100),
	@aIsActive AS INT,
	@aBazdidSpeciality AS VARCHAR(100) = '',
  @aPartIds AS VARCHAR(100) = '',
	@aIsWarmLine AS VARCHAR(1)
AS
	DECLARE @lWhereDTView AS VARCHAR(1000)
	DECLARE @lWhereArea AS VARCHAR(1000)
	DECLARE @lWherePriority AS VARCHAR(100)
	DECLARE @lWhereIsActive AS VARCHAR(1000)
	DECLARE @lWhereIsWarmLine AS VARCHAR(1000)
	DECLARE @lSQL AS VARCHAR(8000)
	DECLARE @lJoinSpecialitySql AS VARCHAR(500) = ''
	DECLARE @lWhereBazdidSpeciality AS VARCHAR(500) = ''

	SET @lWhereArea = ''
	SET @lWhereDTView = ''
	SET @lWherePriority = ''
	SET @lWhereIsActive = ''
	SET @lWhereIsWarmLine = ''

	IF @aStartDatePersian <> ''
	BEGIN
		SET @lWhereDTView = ' AND BTblBazdidResultAddress.StartDatePersian >= ''' + @aStartDatePersian + ''''
	END

	IF @aEndDatePersian <> ''
	BEGIN
		SET @lWhereDTView = @lWhereDTView + ' AND BTblBazdidResultAddress.StartDatePersian <= ''' + @aEndDatePersian + ''''
	END
	IF @aFromDateService <> ''
	BEGIN 
		SET @lWhereDTView = @lWhereDTView + ' AND BTblServiceCheckList.DoneDatePersian >= ''' + @aFromDateService + ''''
	END
	IF @aToDateService <> ''
	BEGIN 
		SET @lWhereDTView = @lWhereDTView + ' AND BTblServiceCheckList.DoneDatePersian <= ''' + @aToDateService + ''''
	END
	IF @aAreaIds <> ''
		SET @lWhereArea = ' AND Tbl_Area.AreaId IN ( ' + @aAreaIds + ' ) '

	IF @aPriorities <> ''
		SET @lWherePriority = ' AND BTblBazdidResultCheckList.Priority IN ( ' + @aPriorities + ' ) '

	IF @aIsActive <> - 1
		SET @lWhereIsActive = ' AND IsActive = ' + CAST(@aIsActive AS VARCHAR)

	IF @aIsWarmLine = '1'
		SET @lWhereIsWarmLine = ' AND   ISNULL(t1.IsWarmLine,0)  = 1 '

	IF @aIsWarmLine = '0'
		SET @lWhereIsWarmLine = ' AND   ISNULL(t1.IsWarmLine,0)  = 0 '

	IF @aBazdidSpeciality <> ''
	BEGIN
		SET @lWhereBazdidSpeciality = ' AND ISNULL(tTS.BazdidSpecialityId,1) IN (' + @aBazdidSpeciality + ')'
		SET @lJoinSpecialitySql = ' LEFT JOIN BTblTimingSpeciality tTS ON BTblBazdidTiming.BazdidTimingId = tTS.BazdidTimingId '
	END
  /*check PartIds added by omid*/
  IF @aPartIds <> '' BEGIN  
  	SET @lJoinSpecialitySql = @lJoinSpecialitySql + 
      ' LEFT JOIN BTblServicePartUse spu ON t1.ServiceId = spu.ServiceId '
    SET @lWhereBazdidSpeciality = @lWhereBazdidSpeciality + ' AND spu.ServicePartId IN ('+ @aPartIds +') '
  END
  
	SET @lSQL = '
		SELECT
			Tbl_Area.AreaId,
			Tbl_Area.Area,
			ISNULL(Tbl_Bazdid.cntMPFeederView,0) AS cntMPFeederView,
			ISNULL(Tbl_Bazdid.cntLPPostView,0) AS cntLPPostView,
			ISNULL(Tbl_Bazdid.cntLPFeederView,0) AS cntLPFeederView,
			ISNULL(Tbl_Service.cntMPFeederDone,0) AS cntMPFeederDone,
			ISNULL(Tbl_Service.cntLPPostDone,0) AS cntLPPostDone,
			ISNULL(Tbl_Service.cntLPFeederDone,0) AS cntLPFeederDone,
			ISNULL(Tbl_LPPostBazdid.ServiceAllCount, 0) AS cntLPPostServiceAllCount
		FROM
			Tbl_Area		
		INNER JOIN
		(
		SELECT
			BTblBazdidResult.AreaId,
			SUM(CASE WHEN (BTblBazdidResult.BazdidTypeId = 1) ' + REPLACE(@lWhereIsActive, 'IsActive', 'Tbl_MPFeeder.IsActive') + ' THEN CASE WHEN BTblBazdidResultCheckList.DefectionCount Is Null OR BTblBazdidResultCheckList.DefectionCount = 0 THEN 1 ELSE BTblBazdidResultCheckList.DefectionCount END END) AS cntMPFeederView,
			SUM(CASE WHEN (BTblBazdidResult.BazdidTypeId = 2) ' + REPLACE(@lWhereIsActive, 'IsActive', 'Tbl_LPPost.IsActive') + 
		' THEN CASE WHEN BTblBazdidResultCheckList.DefectionCount Is Null OR BTblBazdidResultCheckList.DefectionCount = 0 THEN 1 ELSE BTblBazdidResultCheckList.DefectionCount END END) AS cntLPPostView,
			SUM(CASE WHEN (BTblBazdidResult.BazdidTypeId = 3) ' + REPLACE(@lWhereIsActive, 'IsActive', 'Tbl_LPFeeder.IsActive') + 
		' THEN CASE WHEN BTblBazdidResultCheckList.DefectionCount Is Null OR BTblBazdidResultCheckList.DefectionCount = 0 THEN 1 ELSE BTblBazdidResultCheckList.DefectionCount END END) AS cntLPFeederView
		FROM
			BTblBazdidResultCheckList
			INNER JOIN BTblBazdidResultAddress ON BTblBazdidResultCheckList.BazdidResultAddressId = BTblBazdidResultAddress.BazdidResultAddressId
			INNER JOIN BTblBazdidResult ON BTblBazdidResultAddress.BazdidResultId = BTblBazdidResult.BazdidResultId
			LEFT JOIN Tbl_MPFeeder ON BTblBazdidResult.MPFeederId = Tbl_MPFeeder.MPFeederId
			LEFT JOIN Tbl_LPPost ON BTblBazdidResult.LPPostId = Tbl_LPPost.LPPostId
			LEFT JOIN Tbl_LPFeeder ON BTblBazdidResult.LPFeederId = Tbl_LPFeeder.LPFeederId
			Left Join BTblServiceCheckList on BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId
			LEFT JOIN BTblService as t1 ON BTblServiceCheckList.ServiceId = t1.ServiceId  
			LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId
			' + @lJoinSpecialitySql + 
		'
		WHERE
			BTblBazdidResult.BazdidStateId IN (2,3)
			AND BTblBazdidResultCheckList.Priority > 0
			' + @lWherePriority + @lWhereIsWarmLine + '
			' + @lWhereDTView + @lWhereBazdidSpeciality + '
		GROUP BY
			BTblBazdidResult.AreaId
		) as Tbl_Bazdid on Tbl_Area.AreaId = Tbl_Bazdid.AreaId
		LEFT JOIN
		(
		SELECT
			BTblBazdidResult.AreaId,
			SUM(CASE WHEN BTblBazdidResult.BazdidTypeId = 1 ' + REPLACE(@lWhereIsActive, 'IsActive', 'Tbl_MPFeeder.IsActive') + ' THEN CASE WHEN BTblServiceCheckList.ServiceCount Is Null OR BTblServiceCheckList.ServiceCount = 0 THEN 1 ELSE BTblServiceCheckList.ServiceCount END END) AS cntMPFeederDone,
			SUM(CASE WHEN BTblBazdidResult.BazdidTypeId = 2 ' + REPLACE(@lWhereIsActive, 'IsActive', 'Tbl_LPPost.IsActive') + ' THEN CASE WHEN BTblServiceCheckList.ServiceCount Is Null OR BTblServiceCheckList.ServiceCount = 0 THEN 1 ELSE BTblServiceCheckList.ServiceCount END END) AS cntLPPostDone,
			SUM(CASE WHEN BTblBazdidResult.BazdidTypeId = 3 ' + REPLACE(@lWhereIsActive, 'IsActive', 
			'Tbl_LPFeeder.IsActive') + ' THEN CASE WHEN BTblServiceCheckList.ServiceCount Is Null OR BTblServiceCheckList.ServiceCount = 0 THEN 1 ELSE BTblServiceCheckList.ServiceCount END END) AS cntLPFeederDone
		FROM
			BTblServiceCheckList
			INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId
			INNER JOIN BTblBazdidResultAddress ON BTblBazdidResultCheckList.BazdidResultAddressId = BTblBazdidResultAddress.BazdidResultAddressId
			INNER JOIN BTblBazdidResult ON BTblBazdidResultAddress.BazdidResultId = BTblBazdidResult.BazdidResultId
			LEFT JOIN Tbl_MPFeeder ON BTblBazdidResult.MPFeederId = Tbl_MPFeeder.MPFeederId
			LEFT JOIN Tbl_LPPost ON BTblBazdidResult.LPPostId = Tbl_LPPost.LPPostId
			LEFT JOIN Tbl_LPFeeder ON BTblBazdidResult.LPFeederId = Tbl_LPFeeder.LPFeederId
			LEFT JOIN BTblService as t1 ON BTblServiceCheckList.ServiceId = t1.ServiceId
			LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId
			' + @lJoinSpecialitySql + 
		'
		WHERE
			(BTblServiceCheckList.ServiceStateId = 3)
			AND BTblBazdidResultCheckList.Priority > 0
			AND (BTblServiceCheckList.ServiceStateId = 3 OR (BTblServiceCheckList.ServiceStateId = 2 AND BTblServiceCheckList.ServiceCount > 0))
			' + @lWherePriority + @lWhereIsWarmLine + '
			' + @lWhereDTView + @lWhereBazdidSpeciality + 
		'
		GROUP BY
			BTblBazdidResult.AreaId
		) as Tbl_Service on Tbl_Area.AreaId = Tbl_Service.AreaId
		LEFT JOIN
			(
			SELECT
				BTblBazdidResult.AreaId,
				COUNT(DISTINCT Tbl_LPPost.LPPostId) AS ServiceAllCount
			FROM
				BTblBazdidResult
				INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId
				INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId
				INNER JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId
				INNER JOIN Tbl_LPPost ON BTblBazdidResult.LPPostId = Tbl_LPPost.LPPostId
				INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId
				LEFT  JOIN BTblService as t1 ON BTblServiceCheckList.ServiceId = t1.ServiceId
				LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId
				' + @lJoinSpecialitySql + '
			WHERE
				(BTblBazdidResult.BazdidTypeId = 2)
				AND (BTblBazdidResult.BazdidStateId IN (2, 3))
				AND BTblServiceCheckList.ServiceStateId = 3
				' + @lWherePriority + @lWhereIsWarmLine + '
				' + @lWhereDTView + '
				' + REPLACE(@lWhereIsActive, 'IsActive', 'Tbl_LPPost.IsActive') + @lWhereBazdidSpeciality + '
			GROUP BY
				BTblBazdidResult.AreaId
			) AS Tbl_LPPostBazdid ON Tbl_Area.AreaId = Tbl_LPPostBazdid.AreaId
		WHERE IsCenter = 0 ' + @lWhereArea

	exec (@lSQL)
GO

CREATE PROCEDURE dbo.spHomaReportArea
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

/* Not Sure */
ALTER PROCEDURE spGetReport_9_18_ByOperator 
  @FromDate AS VARCHAR(10)
  ,@ToDate AS VARCHAR(10)
  ,@AreaId AS INT = -1
  ,@OperatorIds AS VARCHAR(100)
  ,@IsLight AS BIT = 0
  AS
  BEGIN

    /* enforce filters */
  	DECLARE @lSQL AS VARCHAR(2000) = 'SELECT TblRequest.RequestId, TblRequest.AreaId FROM TblRequest '
    DECLARE @lWhere AS VARCHAR(1000) =  ' WHERE TblRequest.DisconnectDatePersian BETWEEN '''+ @FromDate + ''' AND ''' + @ToDate + ''''
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

  CREATE PROCEDURE dbo.spGetReport_6_21_01
   @aAreaIds AS VARCHAR(100)
  ,@aMPPost AS VARCHAR(100)
  ,@aMPFeederIds AS VARCHAR(100) 
  ,@aLPPostIds AS VARCHAR(100)
  AS 
  BEGIN
    /* LPPost Sectioner */
    DECLARE @lSQL AS VARCHAR(5000)
    DECLARE @lWhere AS VARCHAR(500)
    SET @lSQL = 'SELECT LPP.LPPostId, A.Area, MPP.MPPostName , MPF.MPFeederName , LPP.LPPostName, LPP.LPPostCode , LPP.PostCapacity
          ,Info.IsBarghGir , Info.IsCutout , Info.IsCoverBoshing , info.IsRelatedToAyegh
          ,SpcB_Type.SpecValue AS BType , SpcB_Fac.SpecValue AS BFac, SpcB_Cur.SpecValue AS BCurrent
          ,SpcC_Fuse.SpecValue AS CFCurrent, SpcC_Fac.SpecValue AS CFac, SpcC_Kind.SpecValue AS CKind
          ,Spc_Sec.SpecValue AS SType, Sec.SerialNumber AS SSerial, Sec.IsCutable AS SCutable
          ,Sec.IsGround AS SGround ,Spc_App.SpecValue AS SApp ,Spc_Fac.SpecValue AS SFac
          ,Spc_Mec.SpecValue AS SMech ,Spc_Sar.SpecValue AS SSar, Spc_Size.SpecValue AS SSize
        FROM Tbl_LPPost LPP
        INNER JOIN Tbl_MPFeeder MPF ON LPP.MPFeederId = MPF.MPFeederId
        INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
        INNER JOIN Tbl_Area A ON LPP.AreaId = A.AreaId
        INNER JOIN TblSecsuner Sec ON LPP.LPPostId = Sec.LPPostId
        INNER JOIN Tbl_LPPostInfo Info ON LPP.LPPostId = Info.LPPostId
        LEFT JOIN TblSpec SpcB_Type ON Info.spcBarghGir_JensId = SpcB_Type.SpecId
        LEFT JOIN TblSpec SpcB_Fac ON Info.spcBarghGir_FactoryId = SpcB_Fac.SpecId
        LEFT JOIN TblSpec SpcB_Cur ON Info.spcBarghGir_CurrentId = SpcB_Cur.SpecId
        LEFT JOIN TblSpec SpcC_Fuse ON Info.spcCutout_FuseCurrentId = SpcC_Fuse.SpecId
        LEFT JOIN TblSpec SpcC_Fac ON Info.spcCutout_FactoryId = SpcC_Fac.SpecId
        LEFT JOIN TblSpec SpcC_Kind ON Info.spcCutout_TypeId = SpcC_Kind.SpecId
        LEFT JOIN TblSpec Spc_Sec ON Sec.spcSecsunerTypeId = Spc_Sec.SpecId
        LEFT JOIN TblSpec Spc_App ON Sec.spcApplicationId = Spc_App.SpecId
        LEFT JOIN TblSpec Spc_Fac ON Sec.spcFactoryId = Spc_Fac.SpecId
        LEFT JOIN TblSpec Spc_Mec ON Sec.spcMechanismId = Spc_Mec.SpecId
        LEFT JOIN TblSpec Spc_Sar ON Sec.spcSarkablId = Spc_Sar.SpecId
        LEFT JOIN TblSpec Spc_Size ON Sec.spcCableSizeId = Spc_Size.SpecId '

      SET @lWhere = ' WHERE LPP.IsHavayi = 0'
      IF @aLPPostIds <> '' BEGIN
        SET @lWhere = @lWhere + ' AND LPP.LPPostId IN (' + @aLPPostIds + ')'
      END
      ELSE IF @aMPFeederIds <> '' BEGIN
        SET @lWhere = @lWhere + ' AND MPF.MPFeederId IN (' + @aMPFeederIds + ')' 
      END
      ELSE IF @aMPPost <> '' BEGIN
        SET @lWhere = @lWhere + ' AND MPP.MPPostId IN (' + @aMPPost + ')' 
      END
      ELSE IF @aAreaIds <> '' BEGIN
      	SET @lWhere = @lWhere + ' AND A.AreaId IN (' + @aAreaIds + ')'
      END
      SET @lSQL = @lSQL + @lWhere
      
      EXEC(@lSQL)
  END 

  CREATE PROCEDURE dbo.spGetReport_6_21_02
   @aAreaIds AS VARCHAR(100)
  ,@aMPPost AS VARCHAR(100)
  ,@aMPFeederIds AS VARCHAR(100) 
  ,@aLPPostIds AS VARCHAR(100)
  AS 
  BEGIN
    /* LPPost Fuse-Sectioner */
    DECLARE @lSQL AS VARCHAR(5000)
    DECLARE @lWhere AS VARCHAR(500)
    SET @lSQL = 'SELECT LPP.LPPostId, A.Area, MPP.MPPostName , MPF.MPFeederName , LPP.LPPostName, LPP.LPPostCode , LPP.PostCapacity
          ,Info.IsBarghGir , Info.IsCutout , Info.IsCoverBoshing , info.IsRelatedToAyegh
          ,SpcB_Type.SpecValue AS BType , SpcB_Fac.SpecValue AS BFac, SpcB_Cur.SpecValue AS BCurrent
          ,SpcC_Fuse.SpecValue AS CFCurrent, SpcC_Fac.SpecValue AS CFac, SpcC_Kind.SpecValue AS CKind
          ,Fuse.SerialNumber AS FSerial ,Spc_Fac.SpecValue AS FFac ,Spc_Mod.SpecValue AS FMod 
          ,Spc_App.SpecValue AS FApp ,Spc_Curr.SpecValue AS FCurr
        FROM Tbl_LPPost LPP
        INNER JOIN Tbl_MPFeeder MPF ON LPP.MPFeederId = MPF.MPFeederId
        INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
        INNER JOIN Tbl_Area A ON LPP.AreaId = A.AreaId
        INNER JOIN TblFuseSecsuner Fuse ON LPP.LPPostId = Fuse.LPPostId
        INNER JOIN Tbl_LPPostInfo Info ON LPP.LPPostId = Info.LPPostId
        LEFT JOIN TblSpec SpcB_Type ON Info.spcBarghGir_JensId = SpcB_Type.SpecId
        LEFT JOIN TblSpec SpcB_Fac ON Info.spcBarghGir_FactoryId = SpcB_Fac.SpecId
        LEFT JOIN TblSpec SpcB_Cur ON Info.spcBarghGir_CurrentId = SpcB_Cur.SpecId
        LEFT JOIN TblSpec SpcC_Fuse ON Info.spcCutout_FuseCurrentId = SpcC_Fuse.SpecId
        LEFT JOIN TblSpec SpcC_Fac ON Info.spcCutout_FactoryId = SpcC_Fac.SpecId
        LEFT JOIN TblSpec SpcC_Kind ON Info.spcCutout_TypeId = SpcC_Kind.SpecId
        LEFT JOIN TblSpec Spc_Fac ON Fuse.spcFactoryId = Spc_Fac.SpecId
        LEFT JOIN TblSpec Spc_Mod ON Fuse.spcModelId = Spc_Fac.SpecId
        LEFT JOIN TblSpec Spc_App ON Fuse.spcApplicationId = Spc_Fac.SpecId
        LEFT JOIN TblSpec Spc_Curr ON Fuse.spcFuseCurrentId = Spc_Fac.SpecId '

      SET @lWhere = ' WHERE LPP.IsHavayi = 0'
      IF @aLPPostIds <> '' BEGIN
        SET @lWhere = @lWhere + ' AND LPP.LPPostId IN (' + @aLPPostIds + ')'
      END
      ELSE IF @aMPFeederIds <> '' BEGIN
        SET @lWhere = @lWhere + ' AND MPF.MPFeederId IN (' + @aMPFeederIds + ')' 
      END
      ELSE IF @aMPPost <> '' BEGIN
        SET @lWhere = @lWhere + ' AND MPP.MPPostId IN (' + @aMPPost + ')' 
      END
      ELSE IF @aAreaIds <> '' BEGIN
      	SET @lWhere = @lWhere + ' AND A.AreaId IN (' + @aAreaIds + ')'
      END
      SET @lSQL = @lSQL + @lWhere
      
      EXEC(@lSQL)
  END 

CREATE PROCEDURE dbo.spGetReport_6_21_03
   @aAreaIds AS VARCHAR(100)
  ,@aMPPost AS VARCHAR(100)
  ,@aMPFeederIds AS VARCHAR(100) 
  ,@aLPPostIds AS VARCHAR(100)
  AS 
  BEGIN
    /* LPPost Fuse-Sectioner */
    DECLARE @lSQL AS VARCHAR(5000)
    DECLARE @lWhere AS VARCHAR(500)
    SET @lSQL = 'SELECT LPP.LPPostId, A.Area, MPP.MPPostName , MPF.MPFeederName , LPP.LPPostName, LPP.LPPostCode , LPP.PostCapacity
          ,Info.IsBarghGir , Info.IsCutout , Info.IsCoverBoshing , info.IsRelatedToAyegh
          ,SpcB_Type.SpecValue AS BType , SpcB_Fac.SpecValue AS BFac, SpcB_Cur.SpecValue AS BCurrent
          ,SpcC_Fuse.SpecValue AS CFCurrent, SpcC_Fac.SpecValue AS CFac, SpcC_Kind.SpecValue AS CKind
          ,Dej.SerialNumber ,Spc_Type.SpecValue AS DType , Spc_Fac.SpecValue AS DFac ,Spc_Mod.SpecValue AS DMod
          ,Spc_App.SpecValue AS DApp ,Spc_FCurr.SpecValue AS DFCurr ,Spc_RelehDC.SpecValue AS DRCycle
          ,Dej.IsReleh ,Dej.IsTermometer ,Dej.Isfan ,Dej.IsRelehBokh ,Releh.RelehType
        FROM Tbl_LPPost LPP
        INNER JOIN Tbl_MPFeeder MPF ON LPP.MPFeederId = MPF.MPFeederId
        INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
        INNER JOIN Tbl_Area A ON LPP.AreaId = A.AreaId
        INNER JOIN TblDejenctor Dej ON LPP.LPPostId = Dej.LPPostId
        INNER JOIN Tbl_LPPostInfo Info ON LPP.LPPostId = Info.LPPostId
        LEFT JOIN TblSpec SpcB_Type ON Info.spcBarghGir_JensId = SpcB_Type.SpecId
        LEFT JOIN TblSpec SpcB_Fac ON Info.spcBarghGir_FactoryId = SpcB_Fac.SpecId
        LEFT JOIN TblSpec SpcB_Cur ON Info.spcBarghGir_CurrentId = SpcB_Cur.SpecId
        LEFT JOIN TblSpec SpcC_Fuse ON Info.spcCutout_FuseCurrentId = SpcC_Fuse.SpecId
        LEFT JOIN TblSpec SpcC_Fac ON Info.spcCutout_FactoryId = SpcC_Fac.SpecId
        LEFT JOIN TblSpec SpcC_Kind ON Info.spcCutout_TypeId = SpcC_Kind.SpecId
        LEFT JOIN TblSpec Spc_Type ON Dej.spcDejenctortypeId = Spc_Type.SpecId
        LEFT JOIN TblSpec Spc_Fac ON Dej.spcDejenctor_FactoryId = Spc_Type.SpecId
        LEFT JOIN TblSpec Spc_Mod ON Dej.spcModelId = Spc_Type.SpecId
        LEFT JOIN TblSpec Spc_App ON Dej.spcApplicationId = Spc_Type.SpecId
        LEFT JOIN TblSpec Spc_FCurr ON Dej.spcFCurrentId = Spc_Type.SpecId
        LEFT JOIN TblSpec Spc_RelehDC ON Dej.spcRelehDCCycleType = Spc_Type.SpecId
        LEFT JOIN Tbl_RelehType Releh ON Dej.RelehTypeId = Releh.RelehTypeId '
      
      SET @lWhere = ' WHERE LPP.IsHavayi = 0'
      IF @aLPPostIds <> '' BEGIN
        SET @lWhere = @lWhere + ' AND LPP.LPPostId IN (' + @aLPPostIds + ')'
      END
      ELSE IF @aMPFeederIds <> '' BEGIN
        SET @lWhere = @lWhere + ' AND MPF.MPFeederId IN (' + @aMPFeederIds + ')' 
      END
      ELSE IF @aMPPost <> '' BEGIN
        SET @lWhere = @lWhere + ' AND MPP.MPPostId IN (' + @aMPPost + ')' 
      END
      ELSE IF @aAreaIds <> '' BEGIN
      	SET @lWhere = @lWhere + ' AND A.AreaId IN (' + @aAreaIds + ')'
      END
      SET @lSQL = @lSQL + @lWhere
      
      EXEC(@lSQL)
  END 