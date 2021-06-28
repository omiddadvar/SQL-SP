CREATE PROCEDURE dbo.spGetReport_2_1_1
	@lFromDatePersian as varchar(12),
	@lToDatePersian as varchar(12),
	@lAreaId as varchar(1000),
	@lMPPostId as int,
	@lMPFeederIds as varchar(1000),
	@lOwnershipId as int,
	@lIsActive as int,
	@lBazdidMaster as varchar(1000),
	@lMPFeederPart as varchar(1000),
	@lBazdidSpeciality as varchar(100) = '',
	@lIsHavayi as int,
	@lFromDateBazdid as varchar(12),
	@lToDateBazdid as varchar(12)
AS
	DECLARE @lWhere as nvarchar(4000)
	DECLARE @lSql as varchar(8000)
	DECLARE @lJoinSpecialitySql as varchar(500) = ''

	set @lWhere = ''

	if @lFromDatePersian <> ''
		set @lWhere =  ' AND BTblServiceCheckList.DoneDatePersian >= ''' + @lFromDatePersian +''''
	if @lToDatePersian <> ''
		set @lWhere = @lWhere + ' AND BTblServiceCheckList.DoneDatePersian <= ''' + @lToDatePersian +''''

	if @lFromDateBazdid <> ''
		SET @lWhere =  ' AND BTblBazdidResultAddress.StartDatePersian >= ''' + @lFromDateBazdid +''''
	if @lToDateBazdid <> ''
		SET @lWhere = @lWhere + ' AND BTblBazdidResultAddress.StartDatePersian <= ''' + @lToDateBazdid +''''

	if @lMPFeederIds <> ''
		set @lWhere = @lWhere + ' AND Tbl_MPFeeder.MPFeederId IN ( ' + cast(@lMPFeederIds as varchar)+' ) '
	ELSE if @lMPPostId > -1 OR @lAreaId <> ''
		set @lWhere = @lWhere + ' AND Tbl_MPFeeder.MPFeederId IN (SELECT MPFeederId FROM #TmpTbl_MPFeedersId)'
	if @lOwnershipId  > -1
		set @lWhere = @lWhere + ' AND Tbl_MPFeeder.OwnershipId = ' + cast(@lOwnershipId as varchar)
	if @lIsActive = 1
		set @lWhere = @lWhere + ' AND Tbl_MPFeeder.IsActive = 1 '
    if @lBazdidMaster <> ''
		set @lWhere = @lWhere + ' AND BTblService.BazdidMasterId IN (' + @lBazdidMaster + ')'
	if @lMPFeederPart <> ''
		set @lWhere = @lWhere + ' AND BTblBazdidResult.BazdidBasketDetailId IN (' + @lMPFeederPart + ')'
	if @lIsHavayi = 1
		set @lWhere = @lwhere + ' AND ISNULL(Tbl_MPFeederLen.HavayiLen,0) > ISNULL(Tbl_MPFeederLen.ZaminiLen,0) '
	else if @lIsHavayi = 0
		set @lWhere = @lwhere + ' AND ISNULL(Tbl_MPFeederLen.HavayiLen,0) <= ISNULL(Tbl_MPFeederLen.ZaminiLen,0) '	

	if @lBazdidSpeciality <> ''
	BEGIN
		set @lWhere = @lWhere + ' AND ISNULL(tTS.BazdidSpecialityId,1) IN (' + @lBazdidSpeciality + ')'
		SET @lJoinSpecialitySql = ' LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId ' +
								' LEFT JOIN BTblTimingSpeciality tTS ON BTblBazdidTiming.BazdidTimingId = tTS.BazdidTimingId '
	END

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
		ISNULL(Tbl_MPFeeder.HavaeiLength,0) AS HavayiLen,
		ISNULL(Tbl_MPFeeder.ZeminiLength,0) AS ZaminiLen,
		MIN(BTblServiceCheckList.DoneDatePersian) AS StartDate,
		MAX(BTblServiceCheckList.DoneDatePersian) AS EndDate,
		MAX(BTblBazdidResultAddress.StartDatePersian) as ffdf,
        SUM(CASE WHEN BTblBazdidResult.BazdidStateId IN (2, 3) 
			THEN CASE WHEN BTblBazdidResultCheckList.DefectionCount Is Null OR BTblBazdidResultCheckList.DefectionCount = 0 
				THEN 1 ELSE BTblBazdidResultCheckList.DefectionCount END END) AS BazdidCount, 
		SUM(CASE WHEN BTblServiceCheckList.ServiceStateId = 3 OR (BTblServiceCheckList.ServiceStateId = 2 AND BTblServiceCheckList.ServiceCount > 0)
			THEN CASE WHEN BTblServiceCheckList.ServiceCount Is Null OR BTblServiceCheckList.ServiceCount = 0 
				THEN 1 ELSE BTblServiceCheckList.ServiceCount END END) AS ServiceCount
	FROM 
		BTblBazdidResultAddress
		INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId
		INNER JOIN BTblBazdidResult ON BTblBazdidResultAddress.BazdidResultId = BTblBazdidResult.BazdidResultId
		INNER JOIN Tbl_MPFeeder ON BTblBazdidResult.MPFeederId = Tbl_MPFeeder.MPFeederId
		INNER JOIN
		(
		SELECT 
			Tbl_MPFeeder.MPFeederId, 
			SUM(ISNULL(Tbl_MPFeeder.HavaeiLength,0)) AS HavayiLen,
			SUM(ISNULL(Tbl_MPFeeder.ZeminiLength,0)) AS ZaminiLen
		FROM
			Tbl_MPFeeder
		GROUP BY 
			Tbl_MPFeeder.MPFeederId
		) AS Tbl_MPFeederLen ON Tbl_MPFeeder.MPFeederId = Tbl_MPFeederLen.MPFeederId
		LEFT JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId
		LEFT OUTER JOIN BTblService ON BTblServiceCheckList.ServiceId = BTblService.ServiceId 
		LEFT OUTER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId
		LEFT OUTER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId
		' + @lJoinSpecialitySql + '
	WHERE 
		(BTblBazdidResult.BazdidTypeId = 1) 
		AND BTblBazdidResultCheckList.Priority > 0
		' + @lWhere + '
	GROUP BY 
		Tbl_Area.AreaId,
		Tbl_Area.Area,
		Tbl_MPPost.MPPostName,
		Tbl_MPFeeder.MPFeederId,
		Tbl_MPFeeder.MPFeederName,
		Tbl_MPFeeder.HavaeiLength,
		Tbl_MPFeeder.ZeminiLength
	DROP TABLE #TmpTbl_MPFeedersId
	'
EXEC(@lSql)
GO