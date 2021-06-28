
ALTER PROCEDURE dbo.spGetReport_2_1_5
	@lFromDatePersian as varchar(12),
	@lToDatePersian as varchar(12),
	@lAreaId as varchar(1000),
	@lMPPostId as int,
	@lMPFeederIds as varchar(1000),
	@lOwnershipId as int,
	@lIsActive as int,
	@lBazdidMaster as varchar(1000),
	@lMPFeederPart as varchar(1000),
	@lParts as varchar(1000),
	@lAddress as varchar(1000),
	@lBazdidSpeciality as varchar(100) = '',
	@lServiceNumber as varchar(1000)= '',
	@lFromDateBazdid as varchar(12),
	@lToDateBazdid as varchar(12),
	@lWorkCommand as nvarchar(100),
  @lIsWarmLine AS BIT = 0 ----------omid
AS
	DECLARE @lWhere as varchar(4000)
	DECLARE @lWhereDT as varchar(2000)
	DECLARE @lSql as varchar(8000)
	DECLARE @lJoinSpecialitySql as varchar(500) = ''

	set @lWhere = ''
	set @lWhereDT = ''

	if @lFromDatePersian <> ''
		set @lWhereDT =  ' AND BTblServiceCheckList.DoneDatePersian >= ''' + @lFromDatePersian +''''
	if @lToDatePersian <> ''
		set @lWhereDT = @lWhereDT + ' AND BTblServiceCheckList.DoneDatePersian <= ''' + @lToDatePersian +''''
		
	if @lFromDateBazdid <> ''
		SET @lWhere = @lWhere + ' AND BTblBazdidResultAddress.StartDatePersian >= ''' + @lFromDateBazdid +''''
	if @lToDateBazdid <> ''
		SET @lWhere = @lWhere + ' AND BTblBazdidResultAddress.StartDatePersian <= ''' + @lToDateBazdid +''''

		
	if @lMPFeederIds <> ''
		set @lWhere = @lWhere + ' AND Tbl_MPFeeder.MPFeederId IN ( ' + cast(@lMPFeederIds as varchar) +' ) '
	ELSE if @lMPPostId > -1 OR @lAreaId <> ''
		set @lWhere = @lWhere + ' AND Tbl_MPFeeder.MPFeederId IN (SELECT MPFeederId FROM #TmpTbl_MPFeedersId)'
	if @lOwnershipId  > -1
		set @lWhere = @lwhere + ' AND Tbl_MPFeeder.OwnershipId = ' + cast(@lOwnershipId as varchar)
	if @lServiceNumber  <> ''
		set @lWhere = @lwhere + ' AND BTblService.ServiceNumber = ''' + cast(@lServiceNumber as varchar) + ''''
	if @lIsActive = 1
		set @lWhere = @lWhere + ' AND Tbl_MPFeeder.IsActive = 1 '
    if @lBazdidMaster <> ''
		set @lWhereDT = @lWhereDT + ' AND BTblService.BazdidMasterId IN (' + @lBazdidMaster + ')'
	if @lMPFeederPart <> ''
		set @lWhere = @lWhere + ' AND BTblBazdidResult.BazdidBasketDetailId IN (' + @lMPFeederPart + ')'
	if @lParts <> ''
		set @lWhere = @lWhere + ' AND BTbl_ServicePart.ServicePartId IN (' + @lParts + ')'
	if @lWhereDT <> ''
		set @lWhereDT = ' WHERE ' + RIGHT(@lWhereDT, (LEN(@lWhereDT)-4))
	if @lAddress <> ''
		set @lAddress = ' AND (' + dbo.MergeFarsiAndArabi('BTblBazdidResultAddress.Address',@lAddress) + ')'

  if @lIsWarmLine = 1  -----omid
		set @lWhere = @lWhere + ' AND BTblService.IsWarmLine = 1'
	if @lBazdidSpeciality <> ''
	BEGIN
		set @lWhere = @lWhere + ' AND ISNULL(tTS.BazdidSpecialityId,1) IN (' + @lBazdidSpeciality + ')'
		SET @lJoinSpecialitySql = ' LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId ' +
								' LEFT JOIN BTblTimingSpeciality tTS ON BTblBazdidTiming.BazdidTimingId = tTS.BazdidTimingId '
	END
	if @lWorkCommand <> ''
		set @lWhere = @lwhere + ' AND BTblService.WorkCommandNo = ''' + cast(@lWorkCommand as nvarchar) + ''''
	
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
			Tbl_MPFeeder.MPFeederName,
			Tbl_MPFeeder.MPFeederId,
			BTblBazdidResult.BazdidResultId,
			BTblBazdidResult.FromToLengthHavayi,
			Tbl_MPFeeder.HavaeiLength AS MPFeederHavayiLen,
			BTblBazdidResult.FromToLengthZamini,
			Tbl_MPFeeder.ZeminiLength AS MPFeederZaminiLen,
			ISNULL(BTblBazdidResult.FromToLengthHavayi,Tbl_MPFeeder.HavaeiLength) AS HavaeiLength,
			ISNULL(BTblBazdidResult.FromToLengthZamini,Tbl_MPFeeder.ZeminiLength) AS ZeminiLength,
			BTblBazdidResultAddress.BazdidResultAddressId,
			BTblBazdidResult.FromPathTypeId,
			Tbl_PathType_From.PathType AS FromPathType,
			BTblBazdidResult.FromPathTYpeValue,
			BTblBazdidResult.ToPathTypeId,
			Tbl_PathType_To.PathType AS ToPathType,
			BTblBazdidResult.ToPathTypeValue,
			BTblBazdidResultAddress.Address,
			BTblBazdidResultAddress.GPSx,
			BTblBazdidResultAddress.GPSy,
			BTblServicePartUse.Quantity,
			BTbl_ServicePart.ServicePartName, 
			BTbl_ServicePart.PriceOne, 
			BTbl_ServicePart.ServicePrice, 
			BTbl_ServicePart.ServicePartCode, 
			Tbl_PartUnit.PartUnit,
			Tbl_FeederPart.FeederPart
		FROM 
			BTblBazdidResultAddress
			INNER JOIN BTblBazdidResult ON BTblBazdidResultAddress.BazdidResultId = BTblBazdidResult.BazdidResultId
			INNER JOIN BTblServicePartUse ON BTblBazdidResultAddress.BazdidResultAddressId = BTblServicePartUse.BazdidResultAddressId
			INNER Join BTblService On BTblServicePartuse.ServiceId = BTblService.ServiceId
			INNER JOIN BTbl_ServicePart ON BTblServicePartUse.ServicePartId = BTbl_ServicePart.ServicePartId
			LEFT OUTER JOIN Tbl_PartUnit ON BTbl_ServicePart.PartUnitId = Tbl_PartUnit.PartUnitId
			LEFT OUTER JOIN Tbl_PathType Tbl_PathType_From ON BTblBazdidResult.FromPathTypeId = Tbl_PathType_From.PathTypeId
			LEFT OUTER JOIN Tbl_PathType Tbl_PathType_To ON BTblBazdidResult.ToPathTypeId = Tbl_PathType_To.PathTypeId
			INNER JOIN Tbl_MPFeeder ON BTblBazdidResult.MPFeederId = Tbl_MPFeeder.MPFeederId
			INNER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId
			INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId
			LEFT JOIN Tbl_FeederPart On BTblBazdidResult.FeederPartId = Tbl_FeederPart.FeederPartId
			' + @lJoinSpecialitySql + '
			INNER JOIN
			(
				SELECT DISTINCT
					BTblBazdidResultCheckList.BazdidResultAddressId
				FROM
					BTblBazdidResultCheckList
					INNER JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId
					INNER JOIN BTblService ON BTblServiceCheckList.ServiceId = BTblService.ServiceId
				' + @lWhereDT + '
			) AS Tbl_FilterDate ON BTblBazdidResultAddress.BazdidResultAddressId = Tbl_FilterDate.BazdidResultAddressId
		WHERE 
			BTblBazdidResult.BazdidStateId IN (2,3)
			AND BTblBazdidResult.BazdidTypeId = 1
			AND NOT BTbl_ServicePart.ServicePartId IS NULL
		' + @lAddress + @lWhere + '
	DROP TABLE #TmpTbl_MPFeedersId '

	EXEC(@lSql)
GO