USE [CcRequesterSetad]
GO
/****** Object:  StoredProcedure [dbo].[spGetReport_2_2_5]    Script Date: 04/10/2021 15:10:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[spGetReport_2_2_5]
	@lFromDatePersian as varchar(12),
	@lToDatePersian as varchar(12),
	@lAreaId as varchar(1000),
	@lMPPostId as int,
	@lMPFeederIds as varchar(1000),
	@lLPPostId as int,
	@lOwnershipId as int,
	@lIsActive as int,
	@lParts as varchar(1000),
	@lAddress as varchar(1000),
	@aIsHavayi as int,
	@lBazdidSpeciality as varchar(100) = '',
	@lServiceNumber as varchar(1000)= '',
	@lFromDateBazdid as varchar(12),
	@lToDateBazdid as varchar(12),
	@lWorkCommand as nvarchar(100)
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

	
		
	if @lAreaId <> ''
		set @lWhere = @lwhere + ' AND Tbl_LPPost.AreaId IN ( ' + @lAreaId + ' )'
	if @lLPPostId > -1
		set @lWhere = @lWhere + ' AND Tbl_LPPost.LPPostId = ' + cast(@lLPPostId as varchar)
	else if @lMPFeederIds <> ''
		set @lWhere = @lwhere + ' AND Tbl_LPPost.MPFeederId IN (  ' + cast(@lMPFeederIds as varchar)+ ')'
	else if @lMPPostId > -1
		set @lWhere = @lwhere + ' AND Tbl_MPFeeder.MPPostId = ' + cast(@lMPPostId as varchar)
	if @lOwnershipId  > -1
		set @lWhere = @lwhere + ' AND Tbl_LPPost.OwnershipId = ' + cast(@lOwnershipId as varchar)
	if @lServiceNumber  <> ''
		set @lWhere = @lwhere + ' AND BTblService.ServiceNumber = ''' + cast(@lServiceNumber as varchar) + ''''
	if @lIsActive = 1
		set @lWhere = @lWhere + ' AND Tbl_LPPost.IsActive = 1 '
	if @lParts <> ''
		set @lWhere = @lWhere + ' AND BTbl_ServicePart.ServicePartId IN (' + @lParts + ')'
	if @lAddress <> ''
		set @lAddress = ' AND (' + dbo.MergeFarsiAndArabi('Tbl_LPPost.Address',@lAddress) + ')'
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
	if @lWorkCommand <> ''
		set @lWhere = @lwhere + ' AND BTblService.WorkCommandNo = ''' + cast(@lWorkCommand as nvarchar) + ''''
	
	set @lSql =
		'
		SELECT DISTINCT
			Tbl_Area.AreaId,
			Tbl_Area.Area,
			Tbl_MPPost.MPPostName,
			Tbl_MPFeeder.MPFeederName,
			Tbl_LPPost.LPPostId,
			Tbl_LPPost.LPPostName,
			BTblBazdidResult.BazdidResultId,
			BTblBazdidResultAddress.BazdidResultAddressId,
			Tbl_LPPost.Address,
			BTblBazdidResultAddress.GPSx,
			BTblBazdidResultAddress.GPSy,
			BTblServicePartUse.Quantity,
			BTbl_ServicePart.ServicePartName, 
			BTbl_ServicePart.PriceOne, 
			BTbl_ServicePart.ServicePrice, 
			BTbl_ServicePart.ServicePartCode, 
			Tbl_PartUnit.PartUnit
		FROM 
			BTblBazdidResultAddress
			INNER JOIN BTblBazdidResult ON BTblBazdidResultAddress.BazdidResultId = BTblBazdidResult.BazdidResultId
			INNER JOIN BTblServicePartUse ON BTblBazdidResultAddress.BazdidResultAddressId = BTblServicePartUse.BazdidResultAddressId
			INNER Join BTblService On BTblServicePartuse.ServiceId = BTblService.ServiceId
			INNER JOIN BTbl_ServicePart ON BTblServicePartUse.ServicePartId = BTbl_ServicePart.ServicePartId
			LEFT OUTER JOIN Tbl_PartUnit ON BTbl_ServicePart.PartUnitId = Tbl_PartUnit.PartUnitId
			INNER JOIN Tbl_LPPost ON BTblBazdidResult.LPPostId = Tbl_LPPost.LPPostId
			INNER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId
			INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId
			INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId
			' + @lJoinSpecialitySql + '
			INNER JOIN
			(
				SELECT
					BTblBazdidResultCheckList.BazdidResultAddressId
				FROM
					BTblBazdidResultCheckList
					INNER JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId
					INNER JOIN BTblService ON BTblServiceCheckList.ServiceId = BTblService.ServiceId
				' + @lWhereDT + '
			) AS Tbl_FilterDate ON BTblBazdidResultAddress.BazdidResultAddressId = Tbl_FilterDate.BazdidResultAddressId
		WHERE 
			BTblBazdidResult.BazdidStateId IN (2,3)
			AND BTblBazdidResult.BazdidTypeId = 2
			AND NOT BTbl_ServicePart.ServicePartId IS NULL
		' + @lAddress + @lWhere

	EXEC(@lSql)