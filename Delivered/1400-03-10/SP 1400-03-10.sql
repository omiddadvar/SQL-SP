USE [CcRequesterSetad]
GO
/****** Object:  StoredProcedure [dbo].[spGetReport_2_1_5]    Script Date: 04/10/2021 14:24:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spGetReport_2_1_5]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spGetReport_2_1_5]
GO

CREATE PROCEDURE [dbo].[spGetReport_2_1_5]
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
-----------------------------------------------------------------------------------------------------------------
USE [CcRequesterSetad]
GO
/****** Object:  StoredProcedure [dbo].[spGetReport_2_2_5]    Script Date: 04/10/2021 15:10:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spGetReport_2_2_5]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spGetReport_2_2_5]
GO
CREATE PROCEDURE [dbo].[spGetReport_2_2_5]
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
GO
-----------------------------------------------------------------------------------------------------------------
USE [CcRequesterSetad]
GO
/****** Object:  StoredProcedure [dbo].[spGetReport_2_34_5]    Script Date: 04/10/2021 15:16:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spGetReport_2_34_5]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spGetReport_2_34_5]
GO

CREATE PROCEDURE [dbo].[spGetReport_2_34_5]
	@lFromDatePersian as varchar(12),
	@lToDatePersian as varchar(12),
	@lAreaId as varchar(1000),
	@lMPPostId as int,
	@lMPFeederIds as varchar(1000),
	@lLPPostId as int,
	@lLPFeederId as int,
	@lOwnershipId as int,
	@lIsActive as int,
	@lIsLight as int,
	@lBazdidMaster as varchar(1000),
	@lMPFeederPart as varchar(1000),
	@lParts as varchar(1000),
	@lAddress as varchar(1000),
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
		set @lWhere = @lwhere + ' AND Tbl_LPFeeder.AreaId IN ( ' + @lAreaId + ' )'
	if @lLPFeederId > -1
		set @lWhere = @lWhere + ' AND Tbl_LPFeeder.LPFeederId = ' + cast(@lLPFeederId as varchar)
	else if @lLPPostId > -1
		set @lWhere = @lWhere + ' AND Tbl_LPFeeder.LPPostId = ' + cast(@lLPPostId as varchar)
	else if @lMPFeederIds <> ''
		set @lWhere = @lWhere + ' AND Tbl_LPPost.MPFeederId IN ( ' + cast(@lMPFeederIds as varchar) + ' ) '
	else if @lMPPostId > -1
		set @lWhere = @lWhere + ' AND Tbl_MPFeeder.MPPostId = ' + cast(@lMPPostId as varchar)
	if @lOwnershipId  > -1
		set @lWhere = @lwhere + ' AND Tbl_LPFeeder.OwnershipId = ' + cast(@lOwnershipId as varchar)
	if @lServiceNumber  <> ''
		set @lWhere = @lwhere + ' AND BTblService.ServiceNumber = ''' + cast(@lServiceNumber as varchar) + ''''
	if @lIsActive = 1
		set @lWhere = @lWhere + ' AND Tbl_LPFeeder.IsActive = 1 '
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
			Tbl_LPPost.LPPostName,
			Tbl_LPFeeder.LPFeederName,
			Tbl_LPFeeder.LPFeederId,
			BTblBazdidResult.BazdidResultId,
			BTblBazdidResult.FromToLengthHavayi,
			Tbl_LPFeeder.HavaeiLength AS LPFeederHavayiLen,
			BTblBazdidResult.FromToLengthZamini,
			Tbl_LPFeeder.ZeminiLength AS LPFeederZaminiLen,
			ISNULL(BTblBazdidResult.FromToLengthHavayi,Tbl_LPFeeder.HavaeiLength) AS HavaeiLength,
			ISNULL(BTblBazdidResult.FromToLengthZamini,Tbl_LPFeeder.ZeminiLength) AS ZeminiLength,
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
			Tbl_PartUnit.PartUnit
		FROM 
			BTblBazdidResultAddress
			INNER JOIN BTblBazdidResult ON BTblBazdidResultAddress.BazdidResultId = BTblBazdidResult.BazdidResultId
			INNER JOIN BTblServicePartUse ON BTblBazdidResultAddress.BazdidResultAddressId = BTblServicePartUse.BazdidResultAddressId
			INNER Join BTblService On BTblServicePartuse.ServiceId = BTblService.ServiceId
			INNER JOIN BTbl_ServicePart ON BTblServicePartUse.ServicePartId = BTbl_ServicePart.ServicePartId
			LEFT OUTER JOIN Tbl_PartUnit ON BTbl_ServicePart.PartUnitId = Tbl_PartUnit.PartUnitId
			LEFT OUTER JOIN Tbl_PathType Tbl_PathType_From ON BTblBazdidResult.FromPathTypeId = Tbl_PathType_From.PathTypeId
			LEFT OUTER JOIN Tbl_PathType Tbl_PathType_To ON BTblBazdidResult.ToPathTypeId = Tbl_PathType_To.PathTypeId
			INNER JOIN Tbl_LPFeeder ON BTblBazdidResult.LPFeederId = Tbl_LPFeeder.LPFeederId
			INNER JOIN Tbl_LPPost ON Tbl_LPFeeder.LPPostId = Tbl_LPPost.LPPostId
			INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId
			INNER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId
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
			AND BTblBazdidResult.BazdidTypeId = 3
			AND NOT BTbl_ServicePart.ServicePartId IS NULL
			AND Tbl_LPFeeder.IslightFeeder = ' + CAST(@lIsLight as varchar) + '
		' + @lAddress + @lWhere

	EXEC(@lSql)
GO
-----------------------------------------------------------------------------------------------------------------
USE [CcRequesterSetad]
GO
/****** Object:  StoredProcedure [dbo].[spGetReport_8_15]    Script Date: 04/11/2021 12:13:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spGetReport_8_15]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spGetReport_8_15]
GO

CREATE PROCEDURE [dbo].[spGetReport_8_15] @aPeakDatePersian AS VARCHAR(50)
	,@aPeakTime AS INT
	,@aAreaId AS VARCHAR(1000)
	,@aMPPostId AS INT
	,@aMPFeederId AS INT
	,@aIsActive AS INT
	,@aOwnershipId AS INT
AS
BEGIN
	IF ltrim(rtrim(@aAreaId)) = ''
		SET @aAreaId = NULL

	SELECT Tbl_Area.AreaId
		,Tbl_Area.Area
		,Tbl_MPPost.MPPostId
		,Tbl_MPPost.MPPostName
		,Tbl_MPFeeder.MPFeederId
		,Tbl_MPFeeder.MPFeederName
		,Tbl_MPFeederLoad.RelDatePersian
		,Tbl_MPFeederLoadHours.HourId
		,Tbl_MPFeederLoadHours.CurrentValue
		,ISNULL(trans.MPPostTrans , '»œÊ‰  —«‰”') AS MPPostTrans
		,ISNULL(trans.MPPostTransPower , 0) AS MPPostTransPower
	FROM Tbl_MPFeederLoad
	INNER JOIN Tbl_MPFeederLoadHours ON Tbl_MPFeederLoad.MPFeederLoadId = Tbl_MPFeederLoadHours.MPFeederLoadId
	INNER JOIN Tbl_MPFeeder ON Tbl_MPFeederLoad.MPFeederId = Tbl_MPFeeder.MPFeederId
	INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId
	INNER JOIN Tbl_Area ON Tbl_MPFeeder.AreaId = Tbl_Area.AreaId
	LEFT JOIN Tbl_MPPostTrans trans ON Tbl_MPFeeder.MPPostTransId = trans.MPPostTransId
	WHERE Tbl_MPFeederLoad.RelDatePersian = @aPeakDatePersian
		AND Tbl_MPFeederLoadHours.HourId = @aPeakTime
		AND (Tbl_MPFeeder.AreaId IN (SELECT * FROM Split(@aAreaId,',')) OR @aAreaId IS NULL)
		AND (Tbl_MPFeeder.MPPostId = @aMPPostId OR @aMPPostId = -1)
		AND (Tbl_MPFeeder.MPFeederId = @aMPFeederId OR @aMPFeederId = -1)
		AND (Tbl_MPFeeder.IsActive = @aIsActive OR @aIsActive = - 1)
		AND (Tbl_MPFeeder.OwnershipId = @aOwnershipId OR @aOwnershipId = - 1)
END	
GO
-----------------------------------------------------------------------------------------------------------------
USE [CcRequesterSetad]
GO
/****** Object:  StoredProcedure [dbo].[spGetReport_8_15]    Script Date: 04/11/2021 12:13:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spGetReport_8_24]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spGetReport_8_24]
GO

CREATE PROCEDURE [dbo].[spGetReport_8_24] @aFromPeakDatePersian AS VARCHAR(50)
	,@aToPeakDatePersian AS VARCHAR(50)
	,@aPeakTime AS VARCHAR(10)
	,@aAreaIds AS VARCHAR(1000)
	,@aMPPostIds AS INT
	,@aMPFeederId AS INT
	,@aIsActive AS INT
	,@aOwnershipId AS INT
AS
BEGIN
	DECLARE @lWhere AS VARCHAR(2000)
	DECLARE @lSQL AS VARCHAR(4000)
	SET @lSQL = ''
	SET @lWhere = ''
	
	IF @aAreaIds <> ''
		SET @lWhere += ' AND Tbl_MPFeeder.AreaId IN (' + @aAreaIds + ')'
	IF @aMPPostIds <> ''
		SET @lWhere += ' AND Tbl_MPPost.MPPostId IN (' + @aMPPostIds + ')'
	IF @aMPFeederId > -1
		SET @lWhere += ' AND Tbl_MPFeeder.MPFeederId = ' + cast(@aMPFeederId as varchar(20))
	IF @aIsActive > -1
		SET @lWhere += ' AND Tbl_MPPost.IsActive = ' + cast(@aIsActive as varchar(20))
	IF @aOwnershipId > -1
		SET @lWhere += ' AND Tbl_MPPost.MPPostOwnershipId = ' + cast(@aOwnershipId as varchar(20))
	
--------------- Whether is Exact (@aPeakTime) or an Interval (~@aPeakTime)
	IF @aPeakTime = '' AND @aFromPeakDatePersian <> ''
		SET @lWhere += ' AND Tbl_MPFeederLoad.RelDatePersian >= ''' + @aFromPeakDatePersian + ''''
	IF @aPeakTime = '' AND @aToPeakDatePersian <> ''
		SET @lWhere += ' AND Tbl_MPFeederLoad.RelDatePersian <= ''' + @aToPeakDatePersian + ''''
	IF @aPeakTime <> '' AND @aFromPeakDatePersian <> ''
		SET @lWhere += ' AND Tbl_MPFeederLoad.RelDatePersian = ''' + @aFromPeakDatePersian + ''''
				+ ' AND Tbl_MPFeederLoadHours.HourExact = ''' + @aPeakTime + ''''
--------------- Remove first "AND"
	IF @lWhere <> ''
		SET @lWhere = RIGHT(@lWhere, LEN(@lWhere) - 4)
	SET @lSQL = 'SELECT 
		Tbl_MPPost.MPPostId,
		Tbl_MPFeeder.MPFeederId, 
		Tbl_MPFeeder.AreaId,
		Tbl_Area.Area,
		Tbl_MPFeederLoad.MPFeederLoadId, 
		Tbl_MPPost.MPPostOwnershipId,
		Tbl_MPFeederLoadHours.MPFeederLoadHourId,
		Tbl_MPFeeder.MPFeederName,
		Tbl_MPPost.MPPostName,
		Tbl_MPFeederLoad.RelDatePersian AS LoadDatePersian, 
		Tbl_Hour.HourId, 
		Tbl_Hour.[Hour],
		Tbl_MPFeederLoadHours.HourExact, 
		Tbl_MPPost.IsActive,
		Tbl_MPFeederLoadHours.CurrentValue, 
		Tbl_MPFeederLoadHours.CurrentValueReActive, 
		Tbl_MPFeederLoadHours.PowerValue
		FROM Tbl_MPFeeder
		INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId 
		LEFT OUTER JOIN Tbl_MPFeederLoad ON Tbl_MPFeeder.MPFeederId = Tbl_MPFeederLoad.MPFeederId
		INNER JOIN Tbl_MPFeederLoadHours ON Tbl_MPFeederLoad.MPFeederLoadId = Tbl_MPFeederLoadHours.MPFeederLoadId
		LEFT JOIN Tbl_Hour ON Tbl_MPFeederLoadHours.HourId = Tbl_Hour.HourId
		LEFT JOIN Tbl_Area ON Tbl_MPFeeder.AreaId = Tbl_Area.AreaId
		WHERE' + @lWhere
	EXEC(@lSQL)
END
GO
-----------------------------------------------------------------------------------------------------------------
USE [CcRequesterSetad]
GO
/****** Object:  StoredProcedure [dbo].[spAAAAa]    Script Date: 04/06/2021 14:36:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spGetReport_14_4]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spGetReport_14_4]
GO

CREATE PROCEDURE [dbo].[spGetReport_14_4]
	@MediaTypeId int,
	@AreaIDs as varchar(500),
	@FromDate as nvarchar(10),
	@ToDate as nvarchar(10),
	@ExternalServiceIds as varchar(500)
--@EndJobStateIDs as varchar(500)
as
begin

declare @lsql as nvarchar(4000)

create table #tmpArea (AreaId int)
	set @lsql = ' select AreaId from tbl_Area '
if @AreaIDs <> ''
	set  @lsql += ' where AreaId in (' + @AreaIDs + ')'
insert #tmpArea exec (@lsql)

create table #tmpExternal (ExternalServiceId int)
	set @lsql = ' select ExternalServiceId from Tbl_ExternalService '
if @ExternalServiceIds <> ''
	set  @lsql += ' where ExternalServiceId in (' + @ExternalServiceIds + ')'
insert #tmpExternal exec (@lsql)

select tl.AreaId , Area , SUM(cntAll) as cntAll ,SUM(cntNew6) as cntNew6,SUM(cntDone6) as cntDone6,
	SUM(cntNotDone6) as cntNotDone6,SUM(avgInterval6) as avgInterval6,
	ISNULL(Round(CAST(SUM(cntSum6) AS float) / NullIF(CAST(SUM(cntAll) AS float ),0) ,3), 0) as cntNesbat6 ,
	SUM(cntNew5) as cntNew5,SUM(cntDone5) as cntDone5,SUM(cntNotDone5) as cntNotDone5,
	SUM(avgInterval5) as avgInterval5 , 
	ISNULL(Round(CAST(SUM(cntSum5) AS float) / NullIF(CAST(SUM(cntAll) AS float ),0) , 3) ,0) as cntNesbat5 
	from
	(
	select 6 as mediaTypeId, TblRequest.areaId,
		sum(case when EndJobStateId in (4,5) then 1 
				when EndJobStateId = 9 and dbo.erjaStateId(TblRequest.RequestId) in (1,3) then 1
				else 0 end ) as cntNew6,
		sum(case when EndJobStateId in (2,3) then  1 
				when EndJobStateId = 9 and dbo.erjaStateId(TblRequest.RequestId) in (4) then 1
				else 0 end ) as cntDone6,
		sum(case when EndJobStateId in (8) then  1 
				when EndJobStateId = 9 and dbo.erjaStateId(TblRequest.RequestId) in (5) then 1
				else 0 end ) as cntNotDone6,
		sum(case when EndJobStateId in (2,3,4,5,8) then  1
				when EndJobStateId = 9 and dbo.erjaStateId(TblRequest.RequestId) in (1,3,4,5) then 1
				else 0 end ) as cntSum6,
		avg(case when EndJobStateId in (2,3) then DisconnectInterval
				when EndJobStateId = 9 and dbo.erjaStateId(TblRequest.RequestId) in (4) then DisconnectInterval
				 end) as avgInterval6,
		0 as cntNew5,
		0 as cntDone5,
		0 as cntNotDone5,
		0 as cntSum5,
		0 as avgInterval5,
		0 as cntAll
			from TblRequest 
		inner join #tmpArea on TblRequest.AreaId = #tmpArea.AreaId
		LEFT JOIN Tbl_GISSMSRecevied tGSMS ON TblRequest.RequestId = tGSMS.RequestId
		LEFT JOIN TblRequestData tRD on TblRequest.RequestId = tRD.RequestId
		LEFT JOIN #tmpExternal on tRD.ExternalServiceId = #tmpExternal.ExternalServiceId
		where TblRequest.DisconnectDatePersian >= @FromDate
			and TblRequest.DisconnectDatePersian <= @ToDate
			and ISNULL(tGSMS.MediaTypeId, ISNULL(tRD.MediaTypeId,
				case when not TblRequest.CallId is null then 2 else 10 end)) = 6
			and (@MediaTypeId = 6 or @MediaTypeId = 0)
		group by TblRequest.AreaId
	union
	select 5 as mediaTypeId, TblRequest.areaId,
		0 as cntNew6,
		0 as cntDone6,
		0 as cntNotDone6,
		0 as cntSum6,
		0 as avgInterval6,
		sum(case when EndJobStateId in (4,5) then 1 
				when EndJobStateId = 9 and dbo.erjaStateId(TblRequest.RequestId) in (1,3) then 1
				else 0 end ) as cntNew5,
		sum(case when EndJobStateId in (2,3) then  1 
				when EndJobStateId = 9 and dbo.erjaStateId(TblRequest.RequestId) in (4) then 1
				else 0 end ) as cntDone5,
		sum(case when EndJobStateId in (8) then  1 
				when EndJobStateId = 9 and dbo.erjaStateId(TblRequest.RequestId) in (5) then 1
				else 0 end ) as cntNotDone5,
		sum(case when EndJobStateId in (2,3,4,5,8) then  1
				when EndJobStateId = 9 and dbo.erjaStateId(TblRequest.RequestId) in (1,3,4,5) then 1
				else 0 end ) as cntSum5,
		avg(case when EndJobStateId in (2,3) then DisconnectInterval
				when EndJobStateId = 9 and dbo.erjaStateId(TblRequest.RequestId) in (4) then DisconnectInterval				
				 end) as avgInterval5,
		0 as cntAll
			from TblRequest
		inner join #tmpArea on TblRequest.AreaId = #tmpArea.AreaId
		LEFT JOIN Tbl_GISSMSRecevied tGSMS ON TblRequest.RequestId = tGSMS.RequestId
		LEFT JOIN TblRequestData tRD on TblRequest.RequestId = tRD.RequestId
		LEFT JOIN #tmpExternal on tRD.ExternalServiceId = #tmpExternal.ExternalServiceId
		where TblRequest.DisconnectDatePersian >= @FromDate
			and TblRequest.DisconnectDatePersian <= @ToDate
			and ISNULL(tGSMS.MediaTypeId, ISNULL(tRD.MediaTypeId, 
				case when not TblRequest.CallId is null then 2 else 10 end)) = 5
			and (@MediaTypeId = 5 or @MediaTypeId = 0)
		group by TblRequest.AreaId
		union 
		select 0 as mediaTypeId, TblRequest.areaId,
		0 as cntNew6,
		0 as cntDone6,
		0 as cntNotDone6,
		0 as cntSum6,
		0 as avgInterval6,
		0 as cntNew5,
		0 as cntDone5,
		0 as cntNotDone5,
		0 as cntSum5,
		0 as avgInterval5,
		sum(case when EndJobStateId not in (6,7,10) then  1 else 0 end ) as cntAll
			from TblRequest
		inner join #tmpArea on TblRequest.AreaId = #tmpArea.AreaId
		LEFT JOIN Tbl_GISSMSRecevied tGSMS ON TblRequest.RequestId = tGSMS.RequestId
		LEFT JOIN TblRequestData tRD on TblRequest.RequestId = tRD.RequestId
		LEFT JOIN #tmpExternal on tRD.ExternalServiceId = #tmpExternal.ExternalServiceId
		where TblRequest.DisconnectDatePersian >= @FromDate
			and TblRequest.DisconnectDatePersian <= @ToDate
			and (ISNULL(tGSMS.MediaTypeId, ISNULL(tRD.MediaTypeId, 
				case when not TblRequest.CallId is null then 2 else 10 end)) = @MediaTypeId 
				or @MediaTypeId = 0)
		group by TblRequest.AreaId
	) tl 
	inner join Tbl_Area on Tbl_Area.AreaId = tl.AreaId
	where IsCenter = 0
	group by tl.AreaId , Area

drop table #tmpArea
drop table #tmpExternal

end
GO
-----------------------------------------------------------------------------------------------------------------
USE CcRequesterSetad
--USE CcRequester
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[Sp-PostFeederSelect]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[Sp-PostFeederSelect] 
GO
CREATE PROCEDURE [dbo].[Sp-PostFeederSelect] 
	@feederIds as VARCHAR(1000),
	@state as INT
AS
BEGIN
	Declare @lsql AS NVARCHAR(4000)
	------------------------- Post Load
	IF @state = 1
		SET @lsql = 'SELECT DISTINCT pl.* , f.LPFeederCode , p.LPPostName
		, P.PostCapacity AS LPPostPostCapacity, P.IsTakFaze AS LPPostIsTakFaze
			FROM  Tbl_LPFeeder f
			INNER JOIN Tbl_LPPost p ON p.LPPostId = f.LPPostId
			LEFT JOIN TblLPFeederLoad fl ON f.LPFeederId = fl.LPFeederId
			LEFT JOIN TblLPPostLoad pl ON p.LPPostId = pl.LPPostId
			WHERE f.LPFeederId IN (' + @feederIds + ')
		ORDER BY pl.LPPostLoadId DESC';
	------------------------- Feeder Load
	ELSE IF @state = 2
		SET @lsql = 'SELECT fl.* , f.LPFeederCode
			FROM  Tbl_LPFeeder f
			INNER JOIN TblLPFeederLoad fl ON f.LPFeederId = fl.LPFeederId
			WHERE f.LPFeederId IN (' + @feederIds + ')'
	------------------------- LoadDateTimePersian
	ELSE IF @state = 3
		SET @lsql = 'Select Distinct fl.LoadDateTimePersian , 
			f.LPFeederCode from  TblLPFeederLoad fl
			Inner join Tbl_LPFeeder f on f.LPFeederId = fl.LPFeederId
			WHERE fl.LPFeederId IN (' + @feederIds + ')'
	EXEC(@lsql)
END

GO
-----------------------------------------------------------------------------------------------------------------
USE [CcRequesterSetad]
GO
/****** Object:  StoredProcedure [dbo].[spAAAAa]    Script Date: 04/06/2021 14:36:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spGetReport_Sabz_Erja]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spGetReport_Sabz_Erja]
GO

CREATE PROCEDURE [dbo].[spGetReport_Sabz_Erja]
	@MediaTypeId int,
	@AreaIDs AS varchar(500),
	@FromDate AS NVARCHAR(10),
	@ToDate AS NVARCHAR(10),
	@ExternalServiceIds AS varchar(500)
AS
BEGIN

DECLARE @lsql AS NVARCHAR(4000)

CREATE TABLE #tmpArea (AreaId INT)
	SET @lsql = ' SELECT AreaId FROM tbl_Area '
IF @AreaIDs <> ''
	SET  @lsql += ' WHERE AreaId in (' + @AreaIDs + ')'
INSERT #tmpArea EXEC (@lsql)

CREATE TABLE #tmpExternal (ExternalServiceId INT)
	SET @lsql = ' SELECT ExternalServiceId FROM Tbl_ExternalService '
IF @ExternalServiceIds <> ''
	SET  @lsql += ' WHERE ExternalServiceId in (' + @ExternalServiceIds + ')'
INSERT #tmpExternal EXEC (@lsql)

SELECT tl.AreaId , Area , SUM(cntAll) AS cntAll ,SUM(cntNew6) AS cntNew6,SUM(cntDone6) AS cntDone6,
	SUM(cntNotDone6) AS cntNotDone6,SUM(avgInterval6) AS avgInterval6,
	ISNULL(Round(CAST(SUM(cntSum6) AS float) / NullIF(CAST(SUM(cntAll) AS float),0) ,3), 0) AS cntNesbat6 ,
	SUM(cntNew5) AS cntNew5,SUM(cntDone5) AS cntDone5,SUM(cntNotDone5) AS cntNotDone5,
	SUM(avgInterval5) AS avgInterval5 , 
	ISNULL(Round(CAST(SUM(cntSum5) AS float) / NullIF(CAST(SUM(cntAll) AS float),0) , 3) ,0) AS cntNesbat5 
	from
	(
	SELECT 6 AS mediaTypeId, TblRequest.areaId,
		SUM(CASE WHEN ER.ErjaStateId IN (1,3) THEN 1 ELSE 0 END) AS cntNew6,
		SUM(CASE WHEN ER.ErjaStateId IN (4) THEN 1 ELSE 0 END) AS cntDone6,
		SUM(CASE WHEN ER.ErjaStateId IN (5) THEN 1 ELSE 0 END) AS cntNotDone6,
		SUM(CASE WHEN ER.ErjaStateId IN (1,3,4,5) THEN 1 ELSE 0 END) AS cntSum6,
		AVG(CASE WHEN ER.ErjaStateId IN (4) THEN DisconnectInterval END) AS avgInterval6,
		0 AS cntNew5,
		0 AS cntDone5,
		0 AS cntNotDone5,
		0 AS cntSum5,
		0 AS avgInterval5,
		0 AS cntAll
			FROM TblRequest 
		INNER JOIN #tmpArea on TblRequest.AreaId = #tmpArea.AreaId
		LEFT JOIN Tbl_GISSMSRecevied tGSMS ON TblRequest.RequestId = tGSMS.RequestId
		LEFT JOIN TblRequestData tRD on TblRequest.RequestId = tRD.RequestId
		LEFT JOIN #tmpExternal on tRD.ExternalServiceId = #tmpExternal.ExternalServiceId
		INNER JOIN TblErjaRequest ER on TblRequest.RequestId = ER.RequestId
		WHERE TblRequest.DisconnectDatePersian >= @FromDate
			and TblRequest.DisconnectDatePersian <= @ToDate
			and ISNULL(tGSMS.MediaTypeId, ISNULL(tRD.MediaTypeId,
				CASE WHEN not TblRequest.CallId is null THEN 2 ELSE 10 END)) = 6
			and (@MediaTypeId = 6 or @MediaTypeId = 0)
		GROUP BY TblRequest.AreaId
	union
	SELECT 5 AS mediaTypeId, TblRequest.areaId,
		0 AS cntNew6,
		0 AS cntDone6,
		0 AS cntNotDone6,
		0 AS cntSum6,
		0 AS avgInterval6,
		SUM(CASE WHEN ER.ErjaStateId IN (1,3) THEN 1 ELSE 0 END) AS cntNew5,
		SUM(CASE WHEN ER.ErjaStateId IN (4) THEN 1 ELSE 0 END) AS cntDone5,
		SUM(CASE WHEN ER.ErjaStateId IN (5) THEN 1 ELSE 0 END) AS cntNotDone5,
		SUM(CASE WHEN ER.ErjaStateId IN (1,3,4,5) THEN 1 ELSE 0 END) AS cntSum5,
		AVG(CASE WHEN ER.ErjaStateId IN (4) THEN DisconnectInterval END) AS avgInterval5,
		0 AS cntAll
			FROM TblRequest
		INNER JOIN #tmpArea on TblRequest.AreaId = #tmpArea.AreaId
		LEFT JOIN Tbl_GISSMSRecevied tGSMS ON TblRequest.RequestId = tGSMS.RequestId
		LEFT JOIN TblRequestData tRD on TblRequest.RequestId = tRD.RequestId
		LEFT JOIN #tmpExternal on tRD.ExternalServiceId = #tmpExternal.ExternalServiceId
		INNER JOIN TblErjaRequest ER on TblRequest.RequestId = ER.RequestId
		WHERE TblRequest.DisconnectDatePersian >= @FromDate
			and TblRequest.DisconnectDatePersian <= @ToDate
			and ISNULL(tGSMS.MediaTypeId, ISNULL(tRD.MediaTypeId, 
				CASE WHEN not TblRequest.CallId is null THEN 2 ELSE 10 END)) = 5
			and (@MediaTypeId = 5 or @MediaTypeId = 0)
		GROUP BY TblRequest.AreaId
		union 
		SELECT 0 AS mediaTypeId, TblRequest.areaId,
		0 AS cntNew6,
		0 AS cntDone6,
		0 AS cntNotDone6,
		0 AS cntSum6,
		0 AS avgInterval6,
		0 AS cntNew5,
		0 AS cntDone5,
		0 AS cntNotDone5,
		0 AS cntSum5,
		0 AS avgInterval5,
		SUM(CASE WHEN ER.ErjaStateId not in (6,7,10) THEN 1 ELSE 0 END) AS cntAll
			FROM TblRequest
		INNER JOIN #tmpArea on TblRequest.AreaId = #tmpArea.AreaId
		LEFT JOIN Tbl_GISSMSRecevied tGSMS ON TblRequest.RequestId = tGSMS.RequestId
		LEFT JOIN TblRequestData tRD on TblRequest.RequestId = tRD.RequestId
		LEFT JOIN #tmpExternal on tRD.ExternalServiceId = #tmpExternal.ExternalServiceId
		INNER JOIN TblErjaRequest ER on TblRequest.RequestId = ER.RequestId
		WHERE TblRequest.DisconnectDatePersian >= @FromDate
			and TblRequest.DisconnectDatePersian <= @ToDate
			and (ISNULL(tGSMS.MediaTypeId, ISNULL(tRD.MediaTypeId, 
				CASE WHEN not TblRequest.CallId is null THEN 2 ELSE 10 END)) = @MediaTypeId 
				or @MediaTypeId = 0)
		GROUP BY TblRequest.AreaId
	) tl 
	INNER JOIN Tbl_Area on Tbl_Area.AreaId = tl.AreaId
	WHERE IsCenter = 0 
	GROUP BY tl.AreaId , Area

drop table #tmpArea
drop table #tmpExternal

end
GO
-----------------------------------------------------------------------------------------------------------------
-- > Create StoredProcedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spGMaz_Report_14_1]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spGMaz_Report_14_1]
GO

CREATE PROCEDURE [dbo].[spGMaz_Report_14_1] 
	@aFrom varchar(12),
	@aTo varchar(12)
AS
BEGIN
	SELECT A.Area, RequestNumber, 
		TrackingCode, DisconnectDatePersian, DisconnectTime, 	
		ExternalService, BillingID, Telephone, Mobile, 	
		R.ConnectDatePersian, R.ConnectTime, 	
		dbo.GetRequestEndJobState(trackingcode) AS EndJobState, 	
		R.Comments, R.SubscriberName , M.MediaType
		FROM TblRequestData RD	
		INNER JOIN TblRequest R ON RD.RequestId = R.RequestId 	
		INNER JOIN Tbl_Area A ON A.AreaId = R.AreaId
		INNER JOIN Tbl_ExternalService EX ON RD.ExternalServiceId = EX.ExternalServiceId 	
		INNER JOIN TblRequestInfo RI ON RI.RequestId = R.RequestId
		INNER JOIN Tbl_EndJobState EJ ON R.EndJobStateId = EJ.EndJobStateId 
		INNER JOIN Tbl_MediaType M ON M.MediaTypeId = RD.MediaTypeId 
		WHERE  EX.ExternalServiceTypeId = 1
			AND R.DisconnectDatePersian >= @aFrom 
			AND R.DisconnectDatePersian <= @aTo 
		ORDER BY A.AreaId
END
GO

--Test :
--EXEC spGMaz_Report_14_1 '1399/02/11' , '1400/02/11';
GO
-----------------------------------------------------------------------------------------------------------------
USE CcRequesterSetad
GO
----------------Omid------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spGMaz_GetUnstableFeeders]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spGMaz_GetUnstableFeeders]
GO

CREATE PROCEDURE [dbo].[spGMaz_GetUnstableFeeders] 
	@aFROMDate as varchar(10),
	@aToDate as varchar(10),
	@aMinTimes as int
	AS
	BEGIN
	SELECT MPF.MPFeederName, MPF.MPFeederCode, COUNT(*) AS OutageCount ,
	dbo.GMaz_GetUnstableFeeders(MPF.MPFeederId,@aFROMDate ,@aToDate ) As Reasons
	FROM TblMPRequest MP
		INNER JOIN TblRequest R ON R.MPRequestId = MP.MPRequestId
		INNER JOIN Tbl_MPFeeder MPF ON MPF.MPFeederId = MP.MPFeederId
		WHERE R.IsTamir = 0 AND
			R.EndJobStateId IN (2,3) AND
			MP.IsWarmLine = 0 AND
			MP.DiscONnectDatePersian >= @aFROMDate AND
			MP.DisconnectDatePersian <= @aToDate
		GROUP BY MPF.MPFeederId, MPF.MPFeederName, MPF.MPFeederCode
		HAVING COUNT(*) >= @aMinTimes
		ORDER BY OutageCount DESC
	END
GO


--Test
--EXEC [spGMaz_GetUnstableFeeders] '1397/12/01','1400/02/01',3

GO
-----------------------------------------------------------------------------------------------------------------
USE CcRequesterSetad
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spGetReport_FeederPeak]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spGetReport_FeederPeak]
GO
CREATE PROCEDURE [dbo].[spGetReport_FeederPeak]
	@aFeederCode AS NVARCHAR(100), @aFROM AS VARCHAR(11), @aTo AS VARCHAR(11)
AS
BEGIN

	SELECT TblMPFeederPeak.MPFeederId, MAX(PeakCurrentValue) AS peak, LEFT(LoadDatePersian,7) AS date INTO #tmp
	FROM TblMPFeederPeak
	INNER JOIN Tbl_MPFeeder F ON F.MPFeederId = TblMPFeederPeak.MPFeederId
		WHERE MPFeederCode = @aFeederCode 
		AND LoadDatePersian between @aFROM and @aTo
			AND IsDaily = 1
	GROUP BY 
		LEFT(LoadDatePersian,7),
		TblMPFeederPeak.MPFeederId
	ORDER BY MAX(PeakCurrentValue) DESC

	SELECT TblMPFeederPeak.* INTO #tmp2 FROM 
	TblMPFeederPeak
	inner join #tmp ON TblMPFeederPeak.MPFeederId = #tmp.MPFeederId
	and PeakCurrentValue = #tmp.peak
	and LEFT(LoadDatePersian,7) = #tmp.date

	ORDER BY LoadDatePersian DESC


	SELECT LoadDatePersian, PeakCurrentValue AS Peak FROM 
	(
	SELECT 
	  ROW_NUMBER() OVER(PARTITION BY Left(LoadDatePersian,7) ORDER BY LoadDatePersian DESC) 
		AS Row#,
	 *
	FROM #tmp2
	) t1 WHERE Row# = 1
	ORDER BY LoadDatePersian DESC


	DROP TABLE #tmp
	DROP TABLE #tmp2
END
GO

--TEST :
--EXEC spGetReport_FeederPeak '20524', '1390/01/01' , '1400/02/01'

GO
-----------------------------------------------------------------------------------------------------------------
USE CcRequesterSetad
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spGetReport_PostFeederLoad]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spGetReport_PostFeederLoad]
GO

CREATE PROCEDURE [dbo].[spGetReport_PostFeederLoad]
	@aPostCode as NVARCHAR(100),@aLimit AS INT,  @aISPostLoad AS BIT
AS
BEGIN
	IF @aISPostLoad = 1
	  BEGIN
		  SELECT TOP(@aLimit) P.LPPostName, P.LPPostCode, PL.PostCapacity, PL.PostPeakCurrent,
				PL.RCurrent, PL.SCurrent, PL.TCurrent, PL.NolCurrent,
				PL.LoadDateTimePersian AS PostLoadDate, PL.LoadTime AS PostLoadTime
			FROM Tbl_LPPost P
			INNER JOIN TblLPPostLoad PL ON PL.LPPostId = P.LPPostId
			where LPPostCode = @aPostCode
			ORDER BY P.LPPostId , PL.LoadDT DESC;
	  END
	ELSE
	  BEGIN
		  SELECT F.LPFeederName, F.LPFeederCode, Fl.FeederPeakCurrent,
				FL.RCurrent , FL.SCurrent , FL.TCurrent , FL.NolCurrent,
				FL.LoadDateTimePersian AS FeederLoadDate, FL.LoadTime AS FeederLoadTime
			FROM Tbl_LPFeeder F
			INNER JOIN 
			(SELECT ROW_NUMBER() OVER(PARTITION BY LPFeederId ORDER BY LoadDT DESC)
				AS Row# , * FROM TblLPFeederLoad) FL ON FL.LPFeederId = F.LPFeederId
			INNER JOIN Tbl_LPPost P ON P.LPPostId = F.LPPostId
			where FL.Row# <= @aLimit AND LPPostCode = @aPostCode
			ORDER BY F.LPFeederId , FL.LoadDT DESC;
	  END
END
GO
--TEST :
--EXEC spGetReport_PostFeederLoad '11-0259hg', 3, 1
GO
-----------------------------------------------------------------------------------------------------------------
USE CcRequesterSetad
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spGetReport_LPPostLoadBalance]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spGetReport_LPPostLoadBalance]
GO
CREATE PROCEDURE [dbo].[spGetReport_LPPostLoadBalance] 
	@aLPPostCode AS NVARCHAR(100),
	@aFrom AS VARCHAR(10),
	@aTo AS VARCHAR(10)
AS
SELECT Area.Area 
	,MPost.MPPostName 
	,MFeeder.MPFeederName 
	,Post.LPPostName 
	,ISNULL(Post.Address , '') AS Address
	,Post.LPPostCode 
	,ISNULL(Post.LocalCode , '') AS LocalCode
	,dbo.KHSH_GetPercentANDLegalCurrent(Temp.K , Temp_Area.K, Alt.DecreaseFactor ,
		 Alt_Area.DecreaseFactor, PL.PostCapacity , NULL) AS Nominal
	,PL.PostCapacity
	,PL.RCurrent 
	,PL.SCurrent
	,PL.TCurrent
	,PL.NolCurrent
	,PL.PostPeakCurrent AS Average
	,ISNULL(PL.EarthValue, 0) AS EarthValue
	,PostType.LPPostType
	,Post.LPFeederCount
	,CASE WHEN PL.IsTakFaze = 0 THEN dbo.KHSH_UnbalancedIndicatorA (PL.RCurrent, PL.SCurrent , PL.TCurrent , PL.NolCurrent)
		 ELSE 0 END AS IndicatorA
	,CASE WHEN PL.IsTakFaze = 0 THEN dbo.KHSH_UnbalancedIndicatorB (PL.NolCurrent , PL.PostCapacity)
		 ELSE 0 END AS IndicatorB
	,PL.LoadDateTimePersian AS LoadDate
	,PL.LoadTime
	,dbo.KHSH_GetPercentANDLegalCurrent(Temp.K , Temp_Area.K, Alt.DecreaseFactor ,
		 Alt_Area.DecreaseFactor, PL.PostCapacity , PL.PostPeakCurrent) AS CurrentPercent
	FROM TblLPPostLoad PL
	INNER JOIN Tbl_LPPost Post ON Post.LPPostId = PL.LPPostId
	INNER JOIN Tbl_LPPostType PostType ON PostType.LPPostTypeId = Post.LPPostTypeId
	INNER JOIN Tbl_Area Area ON Area.AreaId = Post.AreaId
	INNER JOIN Tbl_MPFeeder MFeeder ON MFeeder.MPFeederId = POst.MPFeederId
	INNER JOIN Tbl_MPPost MPost ON MPost.MPPostId = MFeeder.MPPostId
	LEFT JOIN Tbl_Temperature Temp ON Temp.TemperatureId = Post.TemperatureId
	LEFT JOIN Tbl_Temperature Temp_Area ON Temp_Area.TemperatureId = Area.TemperatureId
	LEFT JOIN Tbl_Altitude Alt ON alt.AltitudeId = Post.AltitudeId
	LEFT JOIN Tbl_Altitude Alt_Area ON Alt_Area.AltitudeId = Post.AltitudeId
	WHERE Post.LPPostCode = @aLPPostCode 
		AND PL.LoadDateTimePersian BETWEEN @aFrom AND @aTo
	ORDER BY PL.LoadDateTimePersian DESC
GO

--Test
--EXEC spGetReport_LPPostLoadBalance '11-0109hg', '1387/01/01', '1393/01/01'