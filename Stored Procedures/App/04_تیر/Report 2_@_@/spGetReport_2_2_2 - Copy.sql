CREATE PROCEDURE dbo.spGetReport_2_2_2
	@lStartDatePersian as varchar(12),
	@lEndDatePersian as varchar(12),
	@lAreaId as varchar(1000),
	@lMPPostId as int,
	@lMPFeederIds as varchar(1000),
	@lLPPostId as int,
	@lOwnershipId as int,
	@lIsActive as int,
	@aIsHavayi as int,
	@lBazdidSpeciality as varchar(100) = ''	,
	@lFromDateBazdid as varchar(12),
	@lToDateBazdid as varchar(12)
AS
	DECLARE @lWhere as varchar(4000)
	DECLARE @lSql as varchar(8000)
	DECLARE @lJoinSpecialitySql as varchar(500) = ''

	set @lWhere = ''
	
	if @lStartDatePersian <> ''
		set @lWhere =  ' AND DoneDatePersian >= ''' + @lStartDatePersian +''''
	if @lEndDatePersian <> ''
		set @lWhere = @lWhere + ' AND DoneDatePersian <= ''' + @lEndDatePersian +''''
		
	if @lFromDateBazdid <> ''
		SET @lWhere = @lWhere + ' AND BTblBazdidResultAddress.StartDatePersian >= ''' + @lFromDateBazdid +''''
	if @lToDateBazdid <> ''
		SET @lWhere = @lWhere + ' AND BTblBazdidResultAddress.StartDatePersian <= ''' + @lToDateBazdid +''''
	
		
	if @lAreaId <> ''
		set @lWhere = @lwhere + ' AND Tbl_LPPost.AreaId IN ( ' + @lAreaId + ' )'
	if @lLPPostId > -1
		set @lWhere = @lWhere + ' AND Tbl_LPPost.LPPostId = ' + cast(@lLPPostId as varchar)
	else if @lMPFeederIds <> ''
		set @lWhere = @lwhere + ' AND Tbl_LPPost.MPFeederId IN (  ' + cast(@lMPFeederIds as varchar) + ')'
	else if @lMPPostId > -1
		set @lWhere = @lwhere + ' AND Tbl_MPFeeder.MPPostId = ' + cast(@lMPPostId as varchar)
	if @lOwnershipId  > -1
		set @lWhere = @lwhere + ' AND Tbl_LPPost.OwnershipId = ' + cast(@lOwnershipId as varchar)
	if @lIsActive <> 0
		set @lWhere = @lWhere + ' AND Tbl_LPPost.IsActive = 1 '
	if @aIsHavayi = 1
		set @lWhere = @lwhere + ' AND Tbl_LPPost.IsHavayi = 1 '
	else if @aIsHavayi = 0
		set @lWhere = @lwhere + ' AND Tbl_LPPost.IsHavayi = 0 '	
	if @lBazdidSpeciality <> ''
	BEGIN
		set @lWhere = @lWhere + ' AND ISNULL(tTS.BazdidSpecialityId,1) IN (' + @lBazdidSpeciality + ')'
		SET @lJoinSpecialitySql = ' LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId ' +
								' LEFT JOIN BTblTimingSpeciality tTS ON BTblBazdidTiming.BazdidTimingId = tTS.BazdidTimingId '
	END

	set @lSql =	
	'
	SELECT DISTINCT
		Tbl_Area.AreaId,
		Tbl_Area.Area,
		Tbl_MPPost.MPPostName,
		Tbl_MPFeeder.MPFeederName,
		Tbl_LPPost.LPPostName,
		Tbl_LPPost.Address,
		Tbl_LPPost.GPSx,
		Tbl_LPPost.GPSy,
		Tbl_LPPost.LPPostCode,
		Tbl_LPPost.PostCapacity,
		Tbl_LPPost.IsHavayi,
		Tbl_MPFeederUseType.MPFeederUseType,
		MIN(BTblServiceCheckList.DoneDatePersian) AS StartDate,
		MAX(BTblServiceCheckList.DoneDatePersian) AS EndDate
	FROM
		BTblServiceCheckList
		INNER JOIN BTblBazdidResultCheckList ON BTblServiceCheckList.BazdidResultCheckListId = BTblBazdidResultCheckList.BazdidResultCheckListId
		INNER JOIN BTblBazdidResultAddress ON BTblBazdidResultCheckList.BazdidResultAddressId = BTblBazdidResultAddress.BazdidResultAddressId
		INNER JOIN BTblBazdidResult ON BTblBazdidResultAddress.BazdidResultId = BTblBazdidResult.BazdidResultId
		INNER JOIN Tbl_LPPost ON BTblBazdidResult.LPPostId = Tbl_LPPost.LPPostId
		INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId
		INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId
		INNER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId
		LEFT JOIN Tbl_MPFeederUseType ON Tbl_LPPost.LPPostUseTypeId = Tbl_MPFeederUseType.MPFeederUseTypeId
		' + @lJoinSpecialitySql + '
	WHERE
		BTblServiceCheckList.ServiceStateId = 3
		AND BTblBazdidResult.BazdidTypeId = 2
		' + @lWhere + '
	GROUP BY
		Tbl_Area.AreaId,
		Tbl_Area.Area,
		Tbl_MPPost.MPPostName,
		Tbl_MPFeeder.MPFeederName,
		Tbl_LPPost.LPPostName,
		Tbl_LPPost.Address,
		Tbl_LPPost.GPSx,
		Tbl_LPPost.GPSy,
		Tbl_LPPost.LPPostCode,
		Tbl_LPPost.PostCapacity,
		Tbl_LPPost.IsHavayi,
		Tbl_MPFeederUseType.MPFeederUseType
	'

	EXEC(@lSql)
GO