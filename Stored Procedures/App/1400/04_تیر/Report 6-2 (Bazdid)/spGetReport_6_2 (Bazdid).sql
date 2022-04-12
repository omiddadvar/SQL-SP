
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