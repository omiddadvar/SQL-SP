
ALTER PROCEDURE dbo.spGetReport_2_2_1
	@lStartDatePersian as varchar(12),
	@lEndDatePersian as varchar(12),
	@lAreaId as varchar(1000),
	@lMPPostId as int,
	@lMPFeederIds as varchar(1000),
	@lLPPostId as int,
	@lOwnershipId as int,
	@lIsActive as int,
	@lBazdidSpeciality as varchar(100) = '',
	@lFromDateBazdid as varchar(12),
	@lToDateBazdid as varchar(12),
  @lIsWarmLine AS BIT = 0 ----------omid
AS
	DECLARE @lWhere as varchar(4000)
	DECLARE @lWhere2 as varchar(4000)
	DECLARE @lSql as varchar(8000)
	DECLARE @lWhereDT as varchar(4000)
	DECLARE @lJoinSpecialitySql as varchar(500) = ''

	set @lWhere = ''
	set @lWhere2 = ''
	set @lWhereDT = ''

	if @lStartDatePersian <> ''
		set @lWhereDT =  ' AND DoneDatePersian >= ''' + @lStartDatePersian +''''
	if @lEndDatePersian <> ''
		set @lWhereDT = @lWhereDT + ' AND DoneDatePersian <= ''' + @lEndDatePersian +''''
	
	if @lFromDateBazdid <> ''
		SET @lWhereDT = @lWhereDT + ' AND BTblBazdidResultAddress.StartDatePersian >= ''' + @lFromDateBazdid +''''
	if @lToDateBazdid <> ''
		SET @lWhereDT = @lWhereDT + ' AND BTblBazdidResultAddress.StartDatePersian <= ''' + @lToDateBazdid +''''

		
	if @lMPFeederIds <> ''
	BEGIN
		set @lWhere = @lwhere + ' AND Tbl_MPFeeder.MPFeederId IN ( ' + cast(@lMPFeederIds as varchar) + ')'
		set @lWhere2 = @lwhere2 + ' AND Tbl_MPFeeder.MPFeederId IN ( ' + cast(@lMPFeederIds as varchar) + ')'
	END
	ELSE if @lMPPostId > -1 OR @lAreaId <> ''
		set @lWhere = @lWhere + ' AND Tbl_MPFeeder.MPFeederId IN (SELECT MPFeederId FROM #TmpTbl_MPFeedersId)'
	if @lAreaId <> ''
		set @lWhere2 = @lwhere2 + ' AND Tbl_LPPost.AreaId IN ( ' + @lAreaId + ' )'
	if @lMPPostId > -1
		set @lWhere2 = @lwhere2 + ' AND Tbl_MPFeeder.MPPostId = ' + cast(@lMPPostId as varchar)
	if @lLPPostId > -1
		set @lWhereDT = @lWhereDT + ' AND Tbl_LPPost.LPPostId = ' + cast(@lLPPostId as varchar)
	if @lOwnershipId  > -1
		set @lWhere = @lwhere + ' AND Tbl_MPFeeder.OwnershipId = ' + cast(@lOwnershipId as varchar)
	if @lIsActive <> 0
		set @lWhere = @lWhere + ' AND Tbl_MPFeeder.IsActive = 1 '
	if @lBazdidSpeciality <> ''
	BEGIN
		set @lWhereDT = @lWhereDT + ' AND ISNULL(tTS.BazdidSpecialityId,1) IN (' + @lBazdidSpeciality + ')'
		SET @lJoinSpecialitySql = ' LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId ' +
								' LEFT JOIN BTblTimingSpeciality tTS ON BTblBazdidTiming.BazdidTimingId = tTS.BazdidTimingId '
	END
  IF @lIsWarmLine = 1  -----omid
	BEGIN
		SET @lWhereDT = @lWhereDT + ' AND BTblService.IsWarmLine = 1'
		SET @lJoinSpecialitySql = ' LEFT OUTER JOIN BTblService ON BTblServiceCheckList.ServiceId = BTblService.ServiceId '
	END
	set @lWhereDT = @lWhereDT + @lWhere2
	if( @lWhere2 <> '' )
		set @lWhere2 = ' WHERE ' + Right( @lWhere2 , LEN(@lWhere2) - 4 ) 

	set @lSql =	
	'
	CREATE TABLE #TmpTbl_MPFeedersId
	(
		MPFeederId int
	)
	INSERT #TmpTbl_MPFeedersId EXEC spGetMPFeedersId ''' + @lAreaId  + ''',' + cast(@lMPPostId as varchar) + '
	SELECT 
		Tbl_Area.AreaId,
		Tbl_Area.Area,
		Tbl_MPPost.MPPostName,
		Tbl_MPFeeder.MPFeederId,
		Tbl_MPFeeder.MPFeederName,
		Tbl_MPFeeder.Address,
		Tbl_LPPostCount.cntHavayi,
		Tbl_LPPostCount.cntZamini,
		Tbl_Bazdid.cntBazdid,
		Tbl_Bazdid.cntService,
		Tbl_Bazdid.StartDate,
		Tbl_Bazdid.EndDate
	FROM
		Tbl_MPFeeder
		INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId =Tbl_MPPost.MPPostId
		INNER JOIN
		(
			SELECT
				BTblBazdidResult.AreaId,
				Tbl_LPPost.MPFeederId,
				SUM(CASE WHEN BTblBazdidResult.BazdidStateId IN (2, 3) 
					THEN CASE WHEN BTblBazdidResultCheckList.DefectionCount Is Null OR BTblBazdidResultCheckList.DefectionCount = 0 
						THEN 1 ELSE BTblBazdidResultCheckList.DefectionCount END END) AS cntBazdid, 
				SUM(CASE WHEN BTblServiceCheckList.ServiceStateId = 3 OR (BTblServiceCheckList.ServiceStateId = 2 AND BTblServiceCheckList.ServiceCount > 0)
					THEN CASE WHEN BTblServiceCheckList.ServiceCount Is Null OR BTblServiceCheckList.ServiceCount = 0 
						THEN 1 ELSE BTblServiceCheckList.ServiceCount END END) AS cntService,
				MIN(BTblServiceCheckList.DoneDatePersian) AS StartDate,
				MAX(BTblServiceCheckList.DoneDatePersian) AS EndDate
			FROM
				BTblBazdidResultCheckList
				INNER JOIN BTblBazdidResultAddress ON BTblBazdidResultCheckList.BazdidResultAddressId = BTblBazdidResultAddress.BazdidResultAddressId
				INNER JOIN BTblBazdidResult ON BTblBazdidResultAddress.BazdidResultId = BTblBazdidResult.BazdidResultId
				LEFT JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId
				INNER JOIN Tbl_LPPost ON BTblBazdidResult.LPPostId = Tbl_LPPost.LPPostId
				INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId
				' + @lJoinSpecialitySql + '
			WHERE
				BTblBazdidResult.BazdidTypeId = 2
				' + @lWhereDT + '
			GROUP BY
				BTblBazdidResult.AreaId,
				Tbl_LPPost.MPFeederId
		) AS Tbl_Bazdid ON Tbl_MPFeeder.MPFeederId = Tbl_Bazdid.MPFeederId	
		INNER JOIN Tbl_Area ON Tbl_Bazdid.AreaId = Tbl_Area.AreaId
		INNER JOIN
		(
			SELECT
				DISTINCT Tbl_LPPost.MPFeederId, 
				COUNT(CASE WHEN IsHavayi = 1 THEN LPPostId END) AS cntHavayi,
				COUNT(CASE WHEN IsHavayi = 0 THEN LPPostId END) AS cntZamini
			FROM 
				Tbl_LPPost
				INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId
			' + @lWhere2 + '
			GROUP BY 
				Tbl_LPPost.MPFeederId
		) AS Tbl_LPPostCount ON Tbl_MPFeeder.MPFeederId = Tbl_LPPostCount.MPFeederId
	'
	if( @lWhere <> '' )
		set @lSql = @lSql + 'WHERE ' + Right( @lWhere , LEN(@lWhere) - 4 )
EXEC(@lSql)
GO