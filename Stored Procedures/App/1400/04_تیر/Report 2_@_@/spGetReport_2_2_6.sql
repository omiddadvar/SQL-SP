ALTER PROCEDURE dbo.spGetReport_2_2_6
	@lFromDatePersian as varchar(12),
	@lToDatePersian as varchar(12),
	@lAreaId as varchar(1000),
	@lMPPostId as int,
	@lMPFeederIds as varchar(1000),
	@lLPPostId as int,
	@lOwnershipId as int,
	@lIsActive as int,
	@lPriority as varchar(20),
	@lCheckLists as varchar(1000),
	@aNotservice as int,
	@aIsHavayi as int,
	@lBazdidSpeciality as varchar(100) = '',
  @lIsWarmLine AS BIT = 0 ----------omid
AS
	DECLARE @lWhere as nvarchar(4000)
	DECLARE @lServiceCheckList as varchar(8000)
	DECLARE @lSql as varchar(8000)
	DECLARE @lJoinSpecialitySql as varchar(500) = ''
	
	set @lServiceCheckList=' OR BTblServiceCheckList.ServiceStateId <> 3 '

	set @lWhere = ''

	if @lFromDatePersian <> '' 
		set @lWhere =  ' AND BTblBazdidResultAddress.StartDatePersian >= ''' + @lFromDatePersian +''''
	if @lToDatePersian <> ''
		set @lWhere = @lWhere + ' AND BTblBazdidResultAddress.StartDatePersian <= ''' + @lToDatePersian +''''
	if @lAreaId <> ''
		set @lWhere = @lwhere + ' AND Tbl_LPPost.AreaId IN ( ' + @lAreaId + ' )'
	if @lLPPostId > -1
		set @lWhere = @lWhere + ' AND Tbl_LPPost.LPPostId = ' + cast(@lLPPostId as varchar)
	else if @lMPFeederIds <> ''
		set @lWhere = @lwhere + ' AND Tbl_LPPost.MPFeederId IN (  ' + cast(@lMPFeederIds as varchar) +' ) '
	else if @lMPPostId > -1
		set @lWhere = @lwhere + ' AND Tbl_MPFeeder.MPPostId = ' + cast(@lMPPostId as varchar)
	if @lOwnershipId  > -1
		set @lWhere = @lwhere + ' AND Tbl_LPPost.OwnershipId = ' + cast(@lOwnershipId as varchar)
	if @lIsActive = 1
		set @lWhere = @lWhere + ' AND Tbl_LPPost.IsActive = 1 '
	if @lPriority <> ''
		set @lWhere = @lWhere + ' AND BTblBazdidResultCheckList.Priority IN (' + @lPriority + ')'
	if @lCheckLists <> ''
		set @lWhere = @lWhere + ' AND BTblBazdidResultCheckList.BazdidCheckListId IN (' + @lCheckLists + ')'
	if @aNotservice = 1
		  set @lServiceCheckList = ''
	if @aIsHavayi = 1
		set @lWhere = @lwhere + ' AND Tbl_LPPost.IsHavayi = 1 '
	else if @aIsHavayi = 0
		set @lWhere = @lwhere + ' AND Tbl_LPPost.IsHavayi = 0 '	
	if @lBazdidSpeciality <> ''
	BEGIN
		set @lWhere = @lWhere + ' AND ISNULL(tTS.BazdidSpecialityId,1) IN (' + @lBazdidSpeciality + ')'
		SET @lJoinSpecialitySql = ' LEFT JOIN BTblTimingSpeciality tTS ON BTblBazdidTiming.BazdidTimingId = tTS.BazdidTimingId '
	END
  IF @lIsWarmLine = 1  -----omid
	BEGIN
		SET @lWhere = @lWhere + ' AND BTblService.IsWarmLine = 1'
		SET @lJoinSpecialitySql = ' LEFT OUTER JOIN BTblService ON BTblServiceCheckList.ServiceId = BTblService.ServiceId '
	END
	set @lSql =
		'
		SELECT DISTINCT
			Tbl_Area.AreaId,
			Tbl_Area.Area,
			Tbl_MPPost.MPPostName,
			Tbl_MPFeeder.MPFeederName,
			Tbl_MPFeeder.MPFeederId,
			Tbl_LPPost.LPPostName,
			Tbl_LPPost.LPPostId,
			Tbl_LPPost.Address,
			BTblBazdidResultAddress.GPSx,
			BTblBazdidResultAddress.GPSy,
			BTblBazdidResultAddress.StartDatePersian,
			BTbl_BazdidCheckList.CheckListCode,
			BTbl_BazdidCheckList.CheckListName,
			BTblBazdidResultSubCheckList.BazdidResultSubCheckListId , 
			BTbl_SubCheckList.SubCheckListName, 
			BTblBazdidTiming.BazdidName,
			SUM(BTblBazdidResultCheckList.DefectionCount) as  DefectionCount,
			CASE WHEN BTblBazdidResultCheckList.Priority = 4 THEN 
				CAST(''ÚÏã æÌæÏ ÊÌåíÒ'' as nvarchar) 
				ELSE CAST(BTblBazdidResultCheckList.Priority as nvarchar) END AS Priority,
			SUM(BTblBazdidResultCheckList.DefectionCount - IsNull(BTblServiceCheckList.ServiceCount,0)) AS CheckListCount
		FROM 
			Tbl_LPPost
			INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId
			INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId
			INNER JOIN BTblBazdidResult ON Tbl_LPPost.LPPostId = BTblBazdidResult.LPPostId
			INNER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId
			INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId
			INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId
			LEFT JOIN BTblBazdidResultSUBCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblBazdidResultSUBCheckList.BazdidResultCheckListId
			LEFT JOIN  BTbl_SubCheckList  ON BTblBazdidResultSUBCheckList.SubCheckListId = BTbl_SubCheckList.SubCheckListId 
			INNER JOIN BTbl_BazdidCheckList ON BTblBazdidResultCheckList.BazdidCheckListId = BTbl_BazdidCheckList.BazdidCheckListId
			LEFT JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId
			LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId
			' + @lJoinSpecialitySql + '
		WHERE 
			BTblBazdidResult.BazdidStateId IN (2,3)
			AND BTblBazdidResult.BazdidTypeId = 2
			AND BTblBazdidResultCheckList.Priority > 0
			AND 
			(
				BTblServiceCheckList.ServiceCheckListId IS NULL
				' + @lServiceCheckList + '
			)
		' + @lWhere + '
		GROUP BY
			Tbl_Area.AreaId,
			Tbl_Area.Area,
			Tbl_MPPost.MPPostName,
			Tbl_MPFeeder.MPFeederName,
			Tbl_MPFeeder.MPFeederId,
			Tbl_LPPost.LPPostName,
			Tbl_LPPost.LPPostId,
			Tbl_LPPost.Address,
			BTblBazdidResultAddress.GPSx,
			BTblBazdidResultAddress.GPSy,
			BTblBazdidResultAddress.StartDatePersian,
			BTbl_BazdidCheckList.CheckListCode,
			BTbl_BazdidCheckList.CheckListName,
			BTblBazdidResultSubCheckList.BazdidResultSubCheckListId , 
			BTbl_SubCheckList.SubCheckListName, 
			BTblBazdidResultCheckList.Priority,
			BTblBazdidTiming.BazdidName
	'
	EXEC(@lSql)
GO