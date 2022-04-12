ALTER PROCEDURE dbo.spGetReport_2_34_2
	@lFromDatePersian as varchar(12),
	@lToDatePersian as varchar(12),
	@lAreaId as varchar(1000),
	@lMPPostId as int,
	@lMPFeederIds as varchar(1000),
	@lLPPostId as  int,
	@lLPFeederId as int,
	@lOwnershipId as int,
	@lIsActive as int,
	@lIsLight as bit,
	@lBazdidMaster as varchar(1000),
	@lMPFeederPart as varchar(1000),
	@lBazdidSpeciality as varchar(100) = '',
	@lIsHavayi as int,
	@lFromDateBazdid as varchar(12),
	@lToDateBazdid as varchar(12),
  @lIsWarmLine AS BIT = 0 ----------omid
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
		SET @lWhere = @lWhere + ' AND BTblBazdidResultAddress.StartDatePersian >= ''' + @lFromDateBazdid +''''
	if @lToDateBazdid <> ''
		SET @lWhere = @lWhere + ' AND BTblBazdidResultAddress.StartDatePersian <= ''' + @lToDateBazdid +''''
	
	if @lIsHavayi = 1
		set @lWhere = @lwhere + ' AND ISNULL(Tbl_LPFeeder.HavaeiLength,0) > ISNULL(Tbl_LPFeeder.ZeminiLength,0) '
	else if @lIsHavayi = 0
		set @lWhere = @lwhere + ' AND ISNULL(Tbl_LPFeeder.HavaeiLength,0) <= ISNULL(Tbl_LPFeeder.ZeminiLength,0) '	

		
	if @lAreaId <> ''
		set @lWhere = @lWhere + ' AND Tbl_LPFeeder.AreaId IN ( ' + @lAreaId + ' )'
	if @lLPFeederId > -1
		set @lWhere = @lWhere + ' AND Tbl_LPFeeder.LPFeederId = ' + cast(@lLPFeederId as varchar)
	else if @lLPPostId > -1
		set @lWhere = @lWhere + ' AND Tbl_LPFeeder.LPPostId = ' + cast(@lLPPostId as varchar)
	else if @lMPFeederIds <> ''
		set @lWhere = @lWhere + ' AND Tbl_LPPost.MPFeederId IN ( ' + cast(@lMPFeederIds as varchar)+' ) '
	else if @lMPPostId > -1
		set @lWhere = @lWhere + ' AND Tbl_MPFeeder.MPPostId = ' + cast(@lMPPostId as varchar)
	if @lOwnershipId  > -1
		set @lWhere = @lWhere + ' AND Tbl_LPFeeder.OwnershipId = ' + cast(@lOwnershipId as varchar)
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
set @lSql =
	'
	SELECT 
		Tbl_Area.AreaId,
		Tbl_Area.Area,
		Tbl_LPPost.LPPostName,
		Tbl_LPFeeder.LPFeederId,
		Tbl_LPFeeder.LPFeederName,
		ISNULL(Tbl_LPFeeder.HavaeiLength,0) AS HavayiLen,
		ISNULL(Tbl_LPFeeder.ZeminiLength,0) AS ZaminiLen,
		MIN(BTblServiceCheckList.DoneDatePersian) AS StartDate,
		MAX(BTblServiceCheckList.DoneDatePersian) AS EndDate,
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
		INNER JOIN Tbl_LPFeeder ON BTblBazdidResult.LPFeederId = Tbl_LPFeeder.LPFeederId
		LEFT JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId
		LEFT OUTER JOIN BTblService ON BTblServiceCheckList.ServiceId = BTblService.ServiceId 
		LEFT OUTER JOIN Tbl_Area ON BtblBazdidResult.AreaId = Tbl_Area.AreaId
		LEFT OUTER JOIN Tbl_LPPost ON Tbl_LPFeeder.LPPostId = Tbl_LPPost.LPPostId
		LEFT OUTER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId
		' + @lJoinSpecialitySql + '
	WHERE 
		(BTblBazdidResult.BazdidTypeId = 3) 
		AND BTblBazdidResultCheckList.Priority > 0
		AND Tbl_LPFeeder.IsLightFeeder = ' + cast(@lIsLight as varchar) + '
		' + @lWhere + '
	GROUP BY 
		Tbl_Area.AreaId,
		Tbl_Area.Area,
		Tbl_LPPost.LPPostName,
		Tbl_LPFeeder.LPFeederId,
		Tbl_LPFeeder.LPFeederName,
		Tbl_LPFeeder.HavaeiLength,
		Tbl_LPFeeder.ZeminiLength
	'

	EXEC(@lSql)
GO