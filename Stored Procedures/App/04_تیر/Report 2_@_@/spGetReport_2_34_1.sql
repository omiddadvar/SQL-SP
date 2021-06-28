ALTER PROCEDURE dbo.spGetReport_2_34_1
	@lFromDatePersian as varchar(12),
	@lToDatePersian as varchar(12),
	@lAreaId as varchar(1000),
	@lMPPostId as int,
	@lMPFeederIds as varchar(1000),
	@lLPPostId as int,
	@lLPFeederId as int,
	@lOwnershipId as int,
	@lIsActive as int,
	@lIsLight as bit,
	@lBazdidMaster as varchar(1000),
	@lMPFeederPart as varchar(5000),
	@lBazdidSpeciality as varchar(100) = '',
	@lFromDateBazdid as varchar(12),
	@lToDateBazdid as varchar(12),
  @lIsWarmLine AS BIT = 0 ----------omid
AS
	DECLARE @lWhere as varchar(4000)
	DECLARE @lSQL as varchar(8000)
	DECLARE @lJoinSpecialitySql as varchar(500) = ''
	
	set @lwhere = ''
	set @lSQL = ''

	if @lFromDatePersian <> ''
		set @lWhere =  ' AND BTblServiceCheckList.DoneDatePersian >= ''' + @lFromDatePersian +''''
	if @lToDatePersian <> ''
		set @lWhere = @lWhere + ' AND BTblServiceCheckList.DoneDatePersian <= ''' + @lToDatePersian +''''
		
	if @lFromDateBazdid <> ''
		SET @lWhere = @lWhere + ' AND BTblBazdidResultAddress.StartDatePersian >= ''' + @lFromDateBazdid +''''
	if @lToDateBazdid <> ''
		SET @lWhere = @lWhere + ' AND BTblBazdidResultAddress.StartDatePersian <= ''' + @lToDateBazdid +''''

		
		
	if @lAreaId <> ''
		set @lWhere = @lwhere + ' AND Tbl_LPFeeder.AreaId IN ( ' + @lAreaId + ' )'
	if @lLPFeederId > -1
		set @lWhere = @lwhere + ' AND Tbl_LPFeeder.LPFeederId = ' + cast(@lLPFeederId as varchar)
	else if @lLPPostId > -1
		set @lWhere = @lwhere + ' AND Tbl_LPFeeder.LPPostId = ' + cast(@lLPPostId as varchar)
	else if @lMPFeederIds <> ''
		set @lWhere = @lwhere + ' AND Tbl_LPPost.MPFeederId IN ( ' + cast(@lMPFeederIds as varchar)+ ')'
	else if @lMPPostId > -1
		set @lWhere = @lwhere + ' AND Tbl_MPFeeder.MPPostId = ' + cast(@lMPPostId as varchar)
	if @lOwnershipId  > -1
		set @lWhere = @lwhere + ' AND Tbl_LPFeeder.OwnershipId = ' + cast(@lOwnershipId as varchar)
	if @lIsActive = 1
		set @lWhere = @lWhere + ' AND Tbl_LPFeeder.IsActive = 1 '
	if @lBazdidMaster <> ''
		set @lWhere = @lWhere + ' AND BTblService.BazdidMasterId IN (' + @lBazdidMaster + ')'
	if @lMPFeederPart <> ''
		set @lWhere = @lWhere + ' AND BTblBazdidResult.BazdidBasketDetailId IN (' + @lMPFeederPart + ')'
	if @lBazdidSpeciality <> ''
	BEGIN
		set @lWhere = @lWhere + ' AND ISNULL(tTS.BazdidSpecialityId,1) IN (' + @lBazdidSpeciality + ')'
		SET @lJoinSpecialitySql = ' LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId ' +
								' LEFT JOIN BTblTimingSpeciality tTS ON BTblBazdidTiming.BazdidTimingId = tTS.BazdidTimingId '
	END
  IF @lIsWarmLine = 1  -----omid
		SET @lWhere = @lWhere + ' AND BTblService.IsWarmLine = 1'
set @lSQL =
	'		
	SELECT
		Tbl_Area.AreaId,
		Tbl_Area.Area,
		Tbl_MPFeeder.MPFeederName,
		Tbl_LPPost.LPPostName,
		Tbl_LPFeeder.LPFeederId,
		Tbl_LPFeeder.LPFeederName,
		MIN(BTblServiceCheckList.DoneDatePersian) as StartDate,
		MAX(BTblServiceCheckList.DoneDatePersian) as EndDate,
		BTblBazdidResult.FromPathTypeId,
		Tbl_PathType_From.PathType,
		BTblBazdidResult.FromPathTypeValue,
		BTblBazdidResult.ToPathTypeId,
		Tbl_PathType_To.PathType,
		BTblBazdidResult.ToPathTypeValue,
		BTbl_BazdidMaster.Name
	FROM
		Tbl_LPFeeder
		INNER JOIN Tbl_LPPost ON Tbl_LPFeeder.LPPostId = Tbl_LPPost.LPPostId
		INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId
		INNER JOIN BTblBazdidResult ON Tbl_LPFeeder.LPFeederId = BTblBazdidResult.LPFeederId
		INNER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId
		INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId
		INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId
		INNER JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId
		LEFT JOIN Tbl_PathType AS Tbl_PathType_From ON BTblBazdidResult.FromPathTypeId = Tbl_PathType_From.PathTypeId
		LEFT JOIN Tbl_PathType AS Tbl_PathType_To ON BTblBazdidResult.ToPathTypeId = Tbl_PathType_To.PathTypeId
		LEFT JOIN BTblService ON BTblServiceCheckList.ServiceId = BTblService.ServiceId
		LEFT JOIN BTbl_BazdidMaster ON BTblService.BazdidMasterId = BTbl_BazdidMaster.BazdidMasterId
		' + @lJoinSpecialitySql + '
	WHERE
		BTblBazdidResult.BazdidTypeId = 3
		AND BTblBazdidResult.BazdidStateId IN (2,3)
		AND BTblServiceCheckList.ServiceStateId = 3
		AND Tbl_LPFeeder.IsLightFeeder = ' + cast(@lIsLight as varchar) + '
		' + @lWhere + '
	GROUP BY
		Tbl_Area.AreaId,
		Tbl_Area.Area,
		Tbl_MPFeeder.MPFeederName,
		Tbl_LPPost.LPPostName,
		Tbl_LPFeeder.LPFeederId,
		Tbl_LPFeeder.LPFeederName,
		BTblBazdidResult.FromPathTypeId,
		Tbl_PathType_From.PathType,
		BTblBazdidResult.FromPathTypeValue,
		BTblBazdidResult.ToPathTypeId,
		Tbl_PathType_To.PathType,
		BTblBazdidResult.ToPathTypeValue,
		BTbl_BazdidMaster.Name 
	'

	EXEC(@lSQL)
GO