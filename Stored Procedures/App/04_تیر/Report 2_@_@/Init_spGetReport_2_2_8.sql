CREATE PROCEDURE dbo.spGetReport_2_2_8
	@lFromDatePersian as varchar(12),
	@lToDatePersian as varchar(12),
	@lAreaId as varchar(1000),
	@lMPPostId as int,
	@lMPFeederIds  as varchar(1000),
	@lLPPostId as int,
	@lOwnershipId as int,
	@lIsActive as int,
	@lPriority as varchar(20),
	@lCheckLists as varchar(1000),
	@lAddress as varchar(1000),
	@lMinCheckList as int,
	@aIsHavayi as int,
	@lBazdidSpeciality as varchar(100) = ''	,
	@lFromDateBazdid as varchar(12),
	@lToDateBazdid as varchar(12)
AS
	DECLARE @lWhere as nvarchar(4000)
	DECLARE @lHaving as nvarchar(4000)
	DECLARE @lSql as varchar(8000)
	DECLARE @lJoinSpecialitySql as varchar(500) = ''

	set @lWhere = ''
	set @lHaving = ''

	if @lFromDatePersian <> '' 
		set @lWhere =  ' AND (BTblServiceCheckList.DoneDatePersian >= ''' + @lFromDatePersian +'''OR (BTblServiceCheckList.ServiceStateId = 4 AND BTblServiceCheckList.DataEntryDatePersian >= ''' +  @lFromDatePersian + ''' )) '
	if @lToDatePersian <> ''  
		set @lWhere = @lWhere + ' AND (BTblServiceCheckList.DoneDatePersian <= ''' + @lToDatePersian +'''OR (BTblServiceCheckList.ServiceStateId = 4 AND BTblServiceCheckList.DataEntryDatePersian <= ''' +  @lToDatePersian + ''' )) '
	
	if @lFromDateBazdid <> ''
		SET @lWhere = @lWhere + ' AND BTblBazdidResultAddress.StartDatePersian >= ''' + @lFromDateBazdid +''''
	if @lToDateBazdid <> ''
		SET @lWhere = @lWhere + ' AND BTblBazdidResultAddress.StartDatePersian <= ''' + @lToDateBazdid +''''
		
	if @lAreaId <> ''
		set @lWhere = @lwhere + ' AND Tbl_LPPost.AreaId IN ( ' + @lAreaId + ' )'
	if @lLPPostId > -1
		set @lWhere = @lWhere + ' AND Tbl_LPPost.LPPostId = ' + cast(@lLPPostId as varchar)
	else if @lMPFeederIds <> ''
		set @lWhere = @lwhere + ' AND Tbl_LPPost.MPFeederId IN ( ' + cast(@lMPFeederIds as varchar) + ')'
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
	if @lAddress <> ''
		set @lAddress = ' AND (' + dbo.MergeFarsiAndArabi('Tbl_LPPost.Address',@lAddress) + ')'
	if @lMinCheckList > 0
		set @lHaving = ' HAVING SUM(CASE WHEN BTblServiceCheckList.ServiceCount Is Null OR BTblServiceCheckList.ServiceCount = 0 THEN 1 ELSE BTblServiceCheckList.ServiceCount END) >= ' + cast(@lMinCheckList as varchar)	
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
			Tbl_MPFeeder.MPFeederId,
			Tbl_LPPost.LPPostName,
			Tbl_LPPost.LPPostId,
			Tbl_LPPost.Address,
			Tbl_LPPost.LPPostCode,
			BTblBazdidResultAddress.GPSx,
			BTblBazdidResultAddress.GPSy,
			GetDoneDatePersian.DoneDatePersian,
			BTbl_BazdidCheckList.CheckListCode,
			BTbl_BazdidCheckList.CheckListName,
			CASE WHEN BTblBazdidResultCheckList.Priority = 4 THEN 
				CAST(''ÚÏã æÌæÏ ÊÌåíÒ'' as nvarchar) 
				ELSE CAST(BTblBazdidResultCheckList.Priority as nvarchar) END AS Priority,
			BTblServiceCheckList.CreateDatePersian,
			BTblBazdidResultAddress.StartDatePersian,
			ISnull(BTbl_ServiceNotDoneReason.ServiceNotDoneReason,'''') as ServiceNotDoneReason,
			SUM(CASE WHEN BTblServiceCheckList.ServiceCount Is Null OR BTblServiceCheckList.ServiceCount = 0 THEN 1 ELSE BTblServiceCheckList.ServiceCount END) AS CheckListCount
		FROM 
			Tbl_LPPost
			INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId
			INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId
			INNER JOIN BTblBazdidResult ON Tbl_LPPost.LPPostId = BTblBazdidResult.LPPostId
			INNER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId
			INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId
			INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId
			INNER JOIN BTbl_BazdidCheckList ON BTblBazdidResultCheckList.BazdidCheckListId = BTbl_BazdidCheckList.BazdidCheckListId
			INNER JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId
			LEFT OUTER JOIN BTblService ON BTblServiceCheckList.ServiceId = BTblService.ServiceId 
			LEFT OUTER JOIN BTbl_ServiceNotDoneReason on BTblServiceCheckList.ServiceNotDoneReasonId = BTbl_ServiceNotDoneReason.ServiceNotDoneReasonId
			' + @lJoinSpecialitySql + '
			INNER JOIN 
			(
				SELECT 
					BTblServiceCheckList.BazdidResultCheckListId,
					MAX(DoneDatePersian) AS DoneDatePersian
				FROM 
					BTblServiceCheckList
				GROUP BY 
					BTblServiceCheckList.BazdidResultCheckListId
			) AS GetDoneDatePersian ON BTblBazdidResultCheckList.BazdidResultCheckListId = GetDoneDatePersian.BazdidResultCheckListId
		WHERE 
			BTblBazdidResult.BazdidStateId IN (2,3)
			AND BTblBazdidResult.BazdidTypeId = 2
			AND BTblBazdidResultCheckList.Priority > 0
			AND (BTblServiceCheckList.ServiceStateId = 3 OR (BTblServiceCheckList.ServiceStateId = 2 AND BTblServiceCheckList.ServiceCount > 0))
		' + @lAddress + @lWhere + '
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
			GetDoneDatePersian.DoneDatePersian,
			BTbl_BazdidCheckList.CheckListCode,
			BTbl_BazdidCheckList.CheckListName,
			BTblBazdidResultCheckList.Priority,
			BTblServiceCheckList.CreateDatePersian,
			BTblBazdidResultAddress.StartDatePersian,
			ServiceNotDoneReason,
			LPPostCode
	' + @lHaving

	exec(@lSql)
GO