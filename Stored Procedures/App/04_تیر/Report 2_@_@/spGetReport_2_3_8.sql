ALTER PROCEDURE dbo.spGetReport_2_3_8
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
	@lMPFeederPart as varchar(1000),
	@lPriority as varchar(20),
	@lCheckLists as varchar(1000),
	@lAddress as varchar(1000),
	@lMinCheckList as int,
	@lBazdidSpeciality as varchar(100) = '',
	@lFromDateBazdid as varchar(12),
	@lToDateBazdid as varchar(12),
  @lIsWarmLine AS BIT = 0 ----------omid
AS
	DECLARE @lWhere as varchar(8000)
	DECLARE @lHaving as varchar(8000)
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
		set @lWhere = @lwhere + ' AND Tbl_LPFeeder.AreaId IN ( ' + @lAreaId + ' )'
	if @lMPPostId > -1
		set @lWhere = @lWhere + ' AND Tbl_MPFeeder.MPPostId = ' + cast(@lMPPostId as varchar)
	if @lMPFeederIds <> ''
		set @lWhere = @lWhere + ' AND Tbl_LPPost.MPFeederId IN ( ' + cast(@lMPFeederIds as varchar) + ' ) '
	if @lLPPostId > -1
		set @lWhere = @lWhere + ' AND Tbl_LPFeeder.LPPostId = ' + cast(@lLPPostId as varchar)
	if @lLPFeederId > -1
		set @lWhere = @lWhere + ' AND Tbl_LPFeeder.LPFeederId = ' + cast(@lLPFeederId as varchar)
	if @lOwnershipId  > -1
		set @lWhere = @lwhere + ' AND Tbl_LPFeeder.OwnershipId = ' + cast(@lOwnershipId as varchar)
	if @lIsActive = 1
		set @lWhere = @lWhere + ' AND Tbl_LPFeeder.IsActive = 1 '
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
  IF @lIsWarmLine = 1  -----omid
	BEGIN
		SET @lWhere = @lWhere + ' AND BTblService.IsWarmLine = 1'
		SET @lJoinSpecialitySql = ' LEFT OUTER JOIN BTblService ON BTblServiceCheckList.ServiceId = BTblService.ServiceId '
	END
	if @lMinCheckList > 0
		set @lHaving = ' HAVING SUM(CASE WHEN BTblServiceCheckList.ServiceCount Is Null OR BTblServiceCheckList.ServiceCount = 0 THEN 1 ELSE BTblServiceCheckList.ServiceCount END) >= ' + cast(@lMinCheckList as varchar)
	set @lSql =
		'
		SELECT DISTINCT
			Tbl_Area.AreaId,
			Tbl_Area.Area,
			Tbl_LPFeeder.LPPostId,
			Tbl_LPPost.LPPostName,
			Tbl_LPFeeder.LPFeederId,
			Tbl_LPFeeder.LPFeederName,
			Tbl_LPPost.LPPostCode,
			BTblBazdidResult.BazdidResultId,
			ISNULL(Tbl_LPFeeder.HavaeiLength,0) AS LPFeederHavaeiLen,
			ISNULL(Tbl_LPFeeder.ZeminiLength,0) AS LPFeederZeminiLen,
			ISNULL(FromToLengthHavayi,Tbl_LPFeeder.HavaeiLength) AS HavaeiLength,
			ISNULL(FromToLengthZamini,Tbl_LPFeeder.ZeminiLength) AS ZeminiLength,
			BTblBazdidResult.FromPathTypeId,
			Tbl_PathType_From.PathType AS FromPathType,
			BTblBazdidResult.FromPathTYpeValue,
			BTblBazdidResult.ToPathTypeId,
			Tbl_PathType_To.PathType AS ToPathType,
			BTblBazdidResult.ToPathTypeValue,
			BTblBazdidResultAddress.Address,
			BTblBazdidResultAddress.GPSx,
			BTblBazdidResultAddress.GPSy,
			BTbl_BazdidCheckList.CheckListCode,
			BTbl_BazdidCheckList.CheckListName,
			CASE WHEN BTblBazdidResultCheckList.Priority = 4 THEN 
				CAST(''⁄œ„ ÊÃÊœ  ÃÂÌ“'' as nvarchar) 
				ELSE CAST(BTblBazdidResultCheckList.Priority as nvarchar) END AS Priority,
			Tbl_LPFeeder.IsLightFeeder,
			GetDoneDatePersian.DoneDatePersian,
			BTbl_BazdidCheckListGroup.IsHavayi,
			BTblBazdidResultAddress.StartDatePersian,
			ISnull(ServiceNotDoneReason,'''') as ServiceNotDoneReason,
			BTblServiceCheckList.CreateDatePersian,
			SUM(CASE WHEN BTblServiceCheckList.ServiceCount Is Null OR BTblServiceCheckList.ServiceCount = 0 THEN 1 ELSE BTblServiceCheckList.ServiceCount END) AS ServiceCount
		FROM 
			Tbl_LPFeeder
			INNER JOIN Tbl_LPPost ON Tbl_LPFeeder.LPPostId = Tbl_LPPost.LPPostId
			INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId
			INNER JOIN BTblBazdidResult ON Tbl_LPFeeder.LPFeederId = BTblBazdidResult.LPFeederId
			INNER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId
			INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId
			INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId
			INNER JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId
			LEFT OUTER JOIN Tbl_PathType Tbl_PathType_From ON BTblBazdidResult.FromPathTypeId = Tbl_PathType_From.PathTypeId
			LEFT OUTER JOIN Tbl_PathType Tbl_PathType_To ON BTblBazdidResult.ToPathTypeId = Tbl_PathType_To.PathTypeId
			INNER JOIN BTbl_BazdidCheckList ON BTblBazdidResultCheckList.BazdidCheckListId = BTbl_BazdidCheckList.BazdidCheckListId
			LEFT OUTER JOIN BTblService ON BTblServiceCheckList.ServiceId = BTblService.ServiceId
			LEFT OUTER JOIN BTbl_ServiceNotDoneReason on BTblServiceCheckList.ServiceNotDoneReasonId = BTbl_ServiceNotDoneReason.ServiceNotDoneReasonId
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
			BTblBazdidResult.BazdidStateId IN (2,3)
			AND BTblBazdidResult.BazdidTypeId = 3
			AND BTblBazdidResultCheckList.Priority > 0
			AND (BTblServiceCheckList.ServiceStateId = 3 OR (BTblServiceCheckList.ServiceStateId = 2 AND BTblServiceCheckList.ServiceCount > 0))
			AND Tbl_LPFeeder.IsLightFeeder = ' + cast(@lIsLight as varchar) + '
			' + @lAddress + @lWhere + '
		GROUP BY
			Tbl_Area.AreaId,
			Tbl_Area.Area,
			Tbl_LPFeeder.LPPostId,
			Tbl_LPPost.LPPostName,
			Tbl_LPFeeder.LPFeederId,
			Tbl_LPFeeder.LPFeederName,
			BTblBazdidResult.BazdidResultId,
			ISNULL(Tbl_LPFeeder.HavaeiLength,0),
			ISNULL(Tbl_LPFeeder.ZeminiLength,0),
			ISNULL(FromToLengthHavayi,Tbl_LPFeeder.HavaeiLength),
			ISNULL(FromToLengthZamini,Tbl_LPFeeder.ZeminiLength),
			BTblBazdidResult.FromPathTypeId,
			Tbl_PathType_From.PathType,
			BTblBazdidResult.FromPathTYpeValue,
			BTblBazdidResult.ToPathTypeId,
			Tbl_PathType_To.PathType,
			BTblBazdidResult.ToPathTypeValue,
			BTblBazdidResultAddress.Address,
			BTblBazdidResultAddress.GPSx,
			BTblBazdidResultAddress.GPSy,
			BTbl_BazdidCheckList.CheckListCode,
			BTbl_BazdidCheckList.CheckListName,
			CASE WHEN BTblBazdidResultCheckList.Priority = 4 THEN 
				CAST(''⁄œ„ ÊÃÊœ  ÃÂÌ“'' as nvarchar) 
				ELSE CAST(BTblBazdidResultCheckList.Priority as nvarchar) END,
			Tbl_LPFeeder.IsLightFeeder,
			GetDoneDatePersian.DoneDatePersian,
			BTbl_BazdidCheckListGroup.IsHavayi,
			BTblBazdidResultAddress.StartDatePersian,
			ServiceNotDoneReason,
			BTblServiceCheckList.CreateDatePersian,
			Tbl_LPPost.LPPostCode
			 '
		+ @lHaving

	EXEC(@lSql)
GO