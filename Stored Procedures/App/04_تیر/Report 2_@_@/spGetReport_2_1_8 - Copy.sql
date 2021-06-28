CREATE PROCEDURE dbo.spGetReport_2_1_8
	@lFromDatePersian as varchar(12),
	@lToDatePersian as varchar(12),
	@lAreaId as varchar(1000),
	@lMPPostId as int,
	@lMPFeederIds as varchar(1000),
	@lOwnershipId as int,
	@lIsActive as int,
	@lBazdidMaster as varchar(1000),
	@lMPFeederPart as varchar(1000),
	@lPriority as varchar(20),
	@lCheckLists as varchar(1000),
	@lAddress as varchar(1000),
	@lMinCheckList as int,
	@lBazdidSpeciality as varchar(100) = '',
	@lFromDateBazdid as varchar(12),
	@lToDateBazdid as varchar(12)
AS
	DECLARE @lWhere as varchar(8000)
	DECLARE @lHaving as varchar(8000)
	DECLARE @lSql as varchar(8000)
	DECLARE @lJoinSpecialitySql as varchar(500) = ''

	set @lWhere = ''
	set @lHaving = ''

	if @lFromDatePersian <> '' 
		set @lWhere =  ' AND (BTblServiceCheckList.DoneDatePersian >= ''' + @lFromDatePersian +''' OR (BTblServiceCheckList.ServiceStateId = 4 AND BTblServiceCheckList.DataEntryDatePersian >= ''' +  @lFromDatePersian + ''' )) '
		
	if @lToDatePersian <> ''
		set @lWhere = @lWhere + ' AND (BTblServiceCheckList.DoneDatePersian <= ''' + @lToDatePersian +''' OR (BTblServiceCheckList.ServiceStateId = 4 AND BTblServiceCheckList.DataEntryDatePersian <= ''' +  @lToDatePersian + ''' )) '
		
	if @lFromDateBazdid <> ''
		SET @lWhere = @lWhere + ' AND BTblBazdidResultAddress.StartDatePersian >= ''' + @lFromDateBazdid +''''
	if @lToDateBazdid <> ''
		SET @lWhere = @lWhere + ' AND BTblBazdidResultAddress.StartDatePersian <= ''' + @lToDateBazdid +''''

		
	if @lMPFeederIds <> ''
		set @lWhere = @lWhere + ' AND Tbl_MPFeeder.MPFeederId IN ( ' + cast(@lMPFeederIds as varchar) + ')'
	ELSE if @lMPPostId > -1 OR @lAreaId <> ''
		set @lWhere = @lWhere + ' AND Tbl_MPFeeder.MPFeederId IN (SELECT MPFeederId FROM #TmpTbl_MPFeedersId)'
	if @lOwnershipId  > -1
		set @lWhere = @lwhere + ' AND Tbl_MPFeeder.OwnershipId = ' + cast(@lOwnershipId as varchar)
	if @lIsActive = 1
		set @lWhere = @lWhere + ' AND Tbl_MPFeeder.IsActive = 1 '
	if @lBazdidMaster <> ''
		set @lWhere = @lWhere + ' AND BTblService.BazdidMasterId IN (' + @lBazdidMaster + ')'
	if @lMPFeederPart <> ''
		set @lWhere = @lWhere + ' AND BTblBazdidResult.BazdidBasketDetailId IN (' + @lMPFeederPart + ')'
	if @lPriority <> ''
		set @lWhere = @lWhere + ' AND BTblBazdidResultCheckList.Priority IN (' + @lPriority + ')'
	if @lCheckLists <> ''
		set @lWhere = @lWhere + ' AND BTblBazdidResultCheckList.BazdidCheckListId IN (' + @lCheckLists + ')'
	if @lAddress <> ''
		set @lAddress = ' AND (' + dbo.MergeFarsiAndArabi('BTblBazdidResultAddress.Address',@lAddress) + ')'
	if @lBazdidSpeciality <> ''
	BEGIN
		set @lWhere = @lWhere + ' AND ISNULL(tTS.BazdidSpecialityId,1) IN (' + @lBazdidSpeciality + ')'
		SET @lJoinSpecialitySql = ' LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId ' +
								' LEFT JOIN BTblTimingSpeciality tTS ON BTblBazdidTiming.BazdidTimingId = tTS.BazdidTimingId '
	END
	if @lMinCheckList > 0
		set @lHaving = ' HAVING SUM(CASE WHEN BTblServiceCheckList.ServiceCount Is Null OR BTblServiceCheckList.ServiceCount = 0 THEN 1 ELSE BTblServiceCheckList.ServiceCount END) >= ' + cast(@lMinCheckList as varchar)
	set @lSql =
		'
	CREATE TABLE #TmpTbl_MPFeedersId
	(
		MPFeederId int
	)
	INSERT #TmpTbl_MPFeedersId EXEC spGetMPFeedersId ''' + @lAreaId  + ''',' + cast(@lMPPostId as varchar) + '
	SELECT DISTINCT
		Tbl_Area.AreaId,
		Tbl_Area.Area,
		Tbl_MPPost.MPPostName,
		Tbl_MPFeeder.MPFeederId,
		Tbl_MPFeeder.MPFeederName,
		ISNULL(Tbl_MPFeeder.HavaeiLength,0) AS HavayiLen,
		ISNULL(Tbl_MPFeeder.ZeminiLength,0) AS ZaminiLen,
		BTblBazdidResult.BazdidResultId,
		ISNULL(BTblBazdidResult.FromToLengthHavayi,Tbl_MPFeeder.HavaeiLength) AS BazdidHavayi,
		ISNULL(BTblBazdidResult.FromToLengthZamini,Tbl_MPFeeder.ZeminiLength) AS BazdidZamini,
		BTblBazdidResult.FromPathTypeId,
		Tbl_PathType_From.PathType AS FromPathType,
		BTblBazdidResult.FromPathTYpeValue,
		BTblBazdidResult.ToPathTypeId,
		Tbl_PathType_To.PathType AS ToPathType,
		BTblBazdidResult.ToPathTypeValue,
		BTblBazdidResultAddress.Address,
		BTblBazdidResultAddress.GPSx,
		BTblBazdidResultAddress.GPSy,
		Tbl_FeederPart.FeederPart,
		BTblBazdidResultCheckList.BazdidResultCheckListId,
		CASE WHEN BTblBazdidResultCheckList.Priority = 4 THEN 
			CAST(''⁄œ„ ÊÃÊœ  ÃÂÌ“'' as nvarchar) 
			ELSE CAST(BTblBazdidResultCheckList.Priority as nvarchar) END AS Priority,
		BTbl_BazdidCheckList.CheckListCode,
		BTbl_BazdidCheckList.CheckListName,
		GetDoneDatePersian.DoneDatePersian,
		BTbl_BazdidCheckListGroup.IsHavayi,
		BTblServiceCheckList.CreateDatePersian,
		BTblBazdidResultAddress.StartDatePersian,
		ISnull(ServiceNotDoneReason,'''') as ServiceNotDoneReason,
		SUM(CASE WHEN BTblServiceCheckList.ServiceCount Is Null OR BTblServiceCheckList.ServiceCount = 0 THEN 1 ELSE BTblServiceCheckList.ServiceCount END) AS ServiceCount
	FROM 
		BTblBazdidResultAddress
		INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId
		INNER JOIN BTblBazdidResult ON BTblBazdidResultAddress.BazdidResultId = BTblBazdidResult.BazdidResultId
		INNER JOIN Tbl_MPFeeder ON BTblBazdidResult.MPFeederId = Tbl_MPFeeder.MPFeederId
		INNER JOIN BTbl_BazdidCheckList ON BTblBazdidResultCheckList.BazdidCheckListId = BTbl_BazdidCheckList.BazdidCheckListId
		INNER JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId
		LEFT OUTER JOIN BTblService ON BTblServiceCheckList.ServiceId = BTblService.ServiceId 
		Left outer join BTbl_ServiceNotDoneReason on BTblServiceCheckList.ServiceNotDoneReasonId = BTbl_ServiceNotDoneReason.ServiceNotDoneReasonId
		LEFT OUTER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId
		LEFT OUTER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId
		LEFT OUTER JOIN Tbl_PathType Tbl_PathType_From ON BTblBazdidResult.FromPathTypeId = Tbl_PathType_From.PathTypeId
		LEFT OUTER JOIN Tbl_PathType Tbl_PathType_To ON BTblBazdidResult.ToPathTypeId = Tbl_PathType_To.PathTypeId
		LEFT JOIN Tbl_FeederPart On BTblBazdidResult.FeederPartId = Tbl_FeederPart.FeederPartId
		LEFT JOIN BTbl_BazdidCheckListGroup ON BTbl_BazdidCheckList.BazdidCheckListGroupId = BTbl_BazdidCheckListGroup.BazdidCheckListGroupId
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
		(BTblBazdidResult.BazdidTypeId = 1)
		AND BTblBazdidResultCheckList.Priority > 0
		AND (BTblServiceCheckList.ServiceStateId = 3 OR (BTblServiceCheckList.ServiceStateId = 2 AND BTblServiceCheckList.ServiceCount > 0))
		' + @lAddress + @lWhere + '
	GROUP BY
		Tbl_Area.AreaId,
		Tbl_Area.Area,
		Tbl_MPPost.MPPostName,
		Tbl_MPFeeder.MPFeederId,
		Tbl_MPFeeder.MPFeederName,
		ISNULL(Tbl_MPFeeder.HavaeiLength,0),
		ISNULL(Tbl_MPFeeder.ZeminiLength,0),
		BTblBazdidResult.BazdidResultId,
		ISNULL(BTblBazdidResult.FromToLengthHavayi,Tbl_MPFeeder.HavaeiLength),
		ISNULL(BTblBazdidResult.FromToLengthZamini,Tbl_MPFeeder.ZeminiLength),
		BTblBazdidResult.FromPathTypeId,
		Tbl_PathType_From.PathType,
		BTblBazdidResult.FromPathTYpeValue,
		BTblBazdidResult.ToPathTypeId,
		Tbl_PathType_To.PathType,
		BTblBazdidResult.ToPathTypeValue,
		BTblBazdidResultAddress.Address,
		BTblBazdidResultAddress.GPSx,
		BTblBazdidResultAddress.GPSy,
		Tbl_FeederPart.FeederPart,
		BTblBazdidResultCheckList.BazdidResultCheckListId,
		CASE WHEN BTblBazdidResultCheckList.Priority = 4 THEN 
			CAST(''⁄œ„ ÊÃÊœ  ÃÂÌ“'' as nvarchar) 
			ELSE CAST(BTblBazdidResultCheckList.Priority as nvarchar) END ,
		BTbl_BazdidCheckList.CheckListCode,
		BTbl_BazdidCheckList.CheckListName,
		GetDoneDatePersian.DoneDatePersian,
		BTbl_BazdidCheckListGroup.IsHavayi  ,
		BTblServiceCheckList.DoneDatePersian,
		BTblServiceCheckList.CreateDatePersian,
		BTblBazdidResultAddress.StartDatePersian,
		ServiceNotDoneReason,ServiceNotDoneReason'
		+ @lHaving + '
	DROP TABLE #TmpTbl_MPFeedersId 
	'

	EXEC(@lSql)
GO