USE [CcRequesterSetad]
GO
----------------------------------
  
CREATE PROCEDURE spDisHourly_Daily(
  @aAreaIDs AS VARCHAR(100),
  @aDatePersian AS VARCHAR(10),
  @aDate AS VARCHAR(10),
  @aIsLPReq AS BIT = 1,
  @aIsMPReq AS BIT = 1,
  @aIsFoghTReq AS BIT = 1
  ) AS
  BEGIN
    DECLARE @lsql AS VARCHAR(2000) = '';
    DECLARE @lArea AS NVARCHAR(50) = '';
    DECLARE @lHour1 AS VARCHAR(8), @lHour2 AS VARCHAR(8);
    DECLARE @lSumNTamir AS FLOAT,@lSumTamir1 AS FLOAT,@lSumTamir2 AS FLOAT,@lSumTamir3 AS FLOAT
    DECLARE @lDT1 AS DATETIME, @lDT2 AS DATETIME;
    DECLARE @i AS INT = 0;
    CREATE TABLE #tmpData (Area NVARCHAR(50),Hour INT ,HourFrom VARCHAR(8) ,HourTo VARCHAR(8), 
      cntNotTamir FLOAT, cntTamir1 FLOAT,cntTamir2 FLOAT, cntTamir3 FLOAT)
    CREATE TABLE #tmpNet (IsLP BIT,IsMP Bit ,IsFT BIT)
    IF @aIsMPReq = 1 AND @aIsFoghTReq = 1 AND @aIsLPReq = 1 BEGIN  
    	SET @aIsFoghTReq = NULL
      SET @aIsMPReq = NULL
      SET @aIsLPReq = Null
    END
    INSERT #tmpNet SELECT @aIsLPReq, @aIsMPReq, @aIsFoghTReq
    CREATE TABLE #tmpArea (AreaId INT , Area NVARCHAR(50))
    	SET @lsql = ' SELECT AreaId, Area FROM tbl_Area '
    IF @aAreaIDs <> ''
    	SET  @lsql += ' WHERE AreaId IN (' + @aAreaIDs + ')'
   INSERT #tmpArea EXEC (@lsql)
   BEGIN TRY
      WHILE(@i < 24)
      BEGIN
        SET @lHour1 = CONVERT(VARCHAR(8), @i) + ':00' 
        SET @lHour2 = CONVERT(VARCHAR(8), @i + 1) + ':00'
        IF @i = 23 BEGIN SET @lHour2 = '23:59' END
        SET @lDT1 = CONVERT(DATETIME, @aDate +' '+ @lHour1 , 102)
        SET @lDT2 = CONVERT(DATETIME, @aDate +' '+ @lHour2 , 102)
        IF CHARINDEX(',',@aAreaIDs) > 0  BEGIN  ----------------Many Areas
            INSERT #tmpData SELECT 'Â„Â ‰Ê«ÕÌ' , @i, @lHour1, @lHour2,
           SUM(CASE WHEN t.IsTamir = 0 THEN dbo.MinuteCount(@lDT1,@lDT2, t.DisconnectDT,t.ConnectDT) ELSE 0 END) AS sumNotTamir,
           SUM(CASE WHEN t.IsTamir = 1 AND t.TamirTypeId = 1 THEN dbo.MinuteCount(@lDT1,@lDT2, t.DisconnectDT,t.ConnectDT) ELSE 0 END) AS sumTamir1,
           SUM(CASE WHEN t.IsTamir = 1 AND t.TamirTypeId = 2 THEN dbo.MinuteCount(@lDT1,@lDT2, t.DisconnectDT,t.ConnectDT) ELSE 0 END) AS sumTamir2,
           SUM(CASE WHEN t.IsTamir = 1 AND t.TamirTypeId = 3 THEN dbo.MinuteCount(@lDT1,@lDT2, t.DisconnectDT,t.ConnectDT) ELSE 0 END) AS sumTamir3
          FROM TblRequest t
          INNER JOIN #tmpArea a ON a.AreaId = t.AreaId
          WHERE t.DisconnectInterval > 0 
                AND t.ConnectDT IS NOT NULL
                AND t.DisconnectDatePersian = @aDatePersian
                AND EXISTS (SELECT 1 FROM #tmpNet n WHERE (n.IsFT = t.IsFogheToziRequest 
                      AND n.IsMP = t.IsMPRequest
                      AND n.IsLP = t.IsLPRequest)
                      OR n.IsFT IS NULL)
          SET @lArea = 'Â„Â ‰Ê«ÕÌ'
          END
        ELSE  BEGIN    ----------------One Area
          INSERT #tmpData SELECT a.Area, @i, @lHour1, @lHour2,
           SUM(CASE WHEN t.IsTamir = 0 THEN dbo.MinuteCount(@lDT1,@lDT2, t.DisconnectDT,t.ConnectDT) ELSE 0 END) AS sumNotTamir,
           SUM(CASE WHEN t.IsTamir = 1 AND t.TamirTypeId = 1 THEN dbo.MinuteCount(@lDT1,@lDT2, t.DisconnectDT,t.ConnectDT) ELSE 0 END) AS sumTamir1,
           SUM(CASE WHEN t.IsTamir = 1 AND t.TamirTypeId = 2 THEN dbo.MinuteCount(@lDT1,@lDT2, t.DisconnectDT,t.ConnectDT) ELSE 0 END) AS sumTamir2,
           SUM(CASE WHEN t.IsTamir = 1 AND t.TamirTypeId = 3 THEN dbo.MinuteCount(@lDT1,@lDT2, t.DisconnectDT,t.ConnectDT) ELSE 0 END) AS sumTamir3
          FROM TblRequest t
          INNER JOIN #tmpArea a ON a.AreaId = t.AreaId
          WHERE t.DisconnectInterval > 0 
                AND t.ConnectDT IS NOT NULL
                AND t.DisconnectDatePersian = @aDatePersian
                AND EXISTS (SELECT 1 FROM #tmpNet n WHERE (n.IsFT = t.IsFogheToziRequest 
                      AND n.IsMP = t.IsMPRequest
                      AND n.IsLP = t.IsLPRequest)
                      OR n.IsFT IS NULL)
          GROUP BY a.Area
          SELECT @lArea = Area FROM Tbl_Area WHERE AreaId = @aAreaIDs
          END
        SET @i = @i + 1
      END
      -----------------<Calculate Sum>-------------
      IF (SELECT COUNT(*) FROM #tmpData) = 0 BEGIN GOTO End_Calculation END
      SELECT TOP(24) @lSumNTamir = SUM(cntNotTamir), @lSumTamir1 = SUM(cntTamir1) ,
         @lSumTamir2 = SUM(cntTamir2), @lSumTamir3 = SUM(cntTamir3)
        FROM #tmpData
      INSERT #tmpData SELECT @lArea , 24 , '00:00','23:59', @lSumNTamir, @lSumTamir1 , @lSumTamir2, @lSumTamir3
      End_Calculation:
      -----------------</Calculate Sum>------------
      SELECT * FROM #tmpData
      DROP TABLE #tmpData
      DROP TABLE #tmpArea
      DROP TABLE  #tmpNet
    END TRY  
    BEGIN CATCH  
      DROP TABLE #tmpData
      DROP TABLE #tmpArea
      DROP TABLE  #tmpNet
      SELECT   
          ERROR_NUMBER() AS ErrorNumber  
          ,ERROR_LINE() AS Line
          ,ERROR_MESSAGE() AS ErrorMessage;  
    END CATCH;
END
GO
--------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE dbo.spGetReport_2_1_1
	@lFromDatePersian as VARCHAR(12),
	@lToDatePersian as VARCHAR(12),
	@lAreaId as VARCHAR(1000),
	@lMPPostId as int,
	@lMPFeederIds as VARCHAR(1000),
	@lOwnershipId as int,
	@lIsActive as int,
	@lBazdidMaster as VARCHAR(1000),
	@lMPFeederPart as VARCHAR(1000),
	@lBazdidSpeciality as VARCHAR(100) = '',
	@lIsHavayi as int,
	@lFromDateBazdid as VARCHAR(12),
	@lToDateBazdid as VARCHAR(12),
  @lIsWarmLine AS BIT = 0 ----------omid
AS
	DECLARE @lWhere as NVARCHAR(4000)
	DECLARE @lSql as VARCHAR(8000)
	DECLARE @lJoinSpecialitySql as VARCHAR(500) = ''

	set @lWhere = ''

	if @lFromDatePersian <> ''
		set @lWhere =  ' AND BTblServiceCheckList.DoneDatePersian >= ''' + @lFromDatePersian +''''
	if @lToDatePersian <> ''
		set @lWhere = @lWhere + ' AND BTblServiceCheckList.DoneDatePersian <= ''' + @lToDatePersian +''''

	if @lFromDateBazdid <> ''
		SET @lWhere =  ' AND BTblBazdidResultAddress.StartDatePersian >= ''' + @lFromDateBazdid +''''
	if @lToDateBazdid <> ''
		SET @lWhere = @lWhere + ' AND BTblBazdidResultAddress.StartDatePersian <= ''' + @lToDateBazdid +''''

	if @lMPFeederIds <> ''
		set @lWhere = @lWhere + ' AND Tbl_MPFeeder.MPFeederId IN ( ' + cast(@lMPFeederIds as VARCHAR)+' ) '
	ELSE if @lMPPostId > -1 OR @lAreaId <> ''
		set @lWhere = @lWhere + ' AND Tbl_MPFeeder.MPFeederId IN (SELECT MPFeederId FROM #TmpTbl_MPFeedersId)'
	if @lOwnershipId  > -1
		set @lWhere = @lWhere + ' AND Tbl_MPFeeder.OwnershipId = ' + cast(@lOwnershipId as VARCHAR)
	if @lIsActive = 1
		set @lWhere = @lWhere + ' AND Tbl_MPFeeder.IsActive = 1 '
    if @lBazdidMaster <> ''
		set @lWhere = @lWhere + ' AND BTblService.BazdidMasterId IN (' + @lBazdidMaster + ')'
	if @lMPFeederPart <> ''
		set @lWhere = @lWhere + ' AND BTblBazdidResult.BazdidBasketDetailId IN (' + @lMPFeederPart + ')'
	if @lIsHavayi = 1
		set @lWhere = @lwhere + ' AND ISNULL(Tbl_MPFeederLen.HavayiLen,0) > ISNULL(Tbl_MPFeederLen.ZaminiLen,0) '
	else if @lIsHavayi = 0
		set @lWhere = @lwhere + ' AND ISNULL(Tbl_MPFeederLen.HavayiLen,0) <= ISNULL(Tbl_MPFeederLen.ZaminiLen,0) '	
  if @lIsWarmLine = 1  -----omid
		set @lWhere = @lWhere + ' AND BTblService.IsWarmLine = 1'
	if @lBazdidSpeciality <> ''
	BEGIN
		set @lWhere = @lWhere + ' AND ISNULL(tTS.BazdidSpecialityId,1) IN (' + @lBazdidSpeciality + ')'
		SET @lJoinSpecialitySql = ' LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId ' +
								' LEFT JOIN BTblTimingSpeciality tTS ON BTblBazdidTiming.BazdidTimingId = tTS.BazdidTimingId '
	END

set @lSql =
	'
	CREATE TABLE #TmpTbl_MPFeedersId
	(
		MPFeederId int
	)
	INSERT #TmpTbl_MPFeedersId EXEC spGetMPFeedersId ''' + @lAreaId  + ''',' + cast(@lMPPostId as VARCHAR) + '
	SELECT 
		Tbl_Area.AreaId,
		Tbl_Area.Area,
		Tbl_MPPost.MPPostName,
		Tbl_MPFeeder.MPFeederId,
		Tbl_MPFeeder.MPFeederName,
		ISNULL(Tbl_MPFeeder.HavaeiLength,0) AS HavayiLen,
		ISNULL(Tbl_MPFeeder.ZeminiLength,0) AS ZaminiLen,
		MIN(BTblServiceCheckList.DoneDatePersian) AS StartDate,
		MAX(BTblServiceCheckList.DoneDatePersian) AS EndDate,
		MAX(BTblBazdidResultAddress.StartDatePersian) as ffdf,
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
		INNER JOIN Tbl_MPFeeder ON BTblBazdidResult.MPFeederId = Tbl_MPFeeder.MPFeederId
		INNER JOIN
		(
		SELECT 
			Tbl_MPFeeder.MPFeederId, 
			SUM(ISNULL(Tbl_MPFeeder.HavaeiLength,0)) AS HavayiLen,
			SUM(ISNULL(Tbl_MPFeeder.ZeminiLength,0)) AS ZaminiLen
		FROM
			Tbl_MPFeeder
		GROUP BY 
			Tbl_MPFeeder.MPFeederId
		) AS Tbl_MPFeederLen ON Tbl_MPFeeder.MPFeederId = Tbl_MPFeederLen.MPFeederId
		LEFT JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId
		LEFT OUTER JOIN BTblService ON BTblServiceCheckList.ServiceId = BTblService.ServiceId 
		LEFT OUTER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId
		LEFT OUTER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId
		' + @lJoinSpecialitySql + '
	WHERE 
		(BTblBazdidResult.BazdidTypeId = 1) 
		AND BTblBazdidResultCheckList.Priority > 0
		' + @lWhere + '
	GROUP BY 
		Tbl_Area.AreaId,
		Tbl_Area.Area,
		Tbl_MPPost.MPPostName,
		Tbl_MPFeeder.MPFeederId,
		Tbl_MPFeeder.MPFeederName,
		Tbl_MPFeeder.HavaeiLength,
		Tbl_MPFeeder.ZeminiLength
	DROP TABLE #TmpTbl_MPFeedersId
	'
EXEC(@lSql)
GO
--------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE dbo.spGetReport_2_1_2
	@lFromDatePersian as varchar(12),
	@lToDatePersian as varchar(12),
	@lAreaId as varchar(1000),
	@lMPPostId as int,
	@lMPFeederIds as varchar(1000),
	@lOwnershipId as int,
	@lIsActive as int,
	@lBazdidMaster as varchar(1000),
	@lMPFeederPart as varchar(1000),
	@lBazdidSpeciality as varchar(100) = '',
	@lFromDateBazdid as varchar(12),
	@lToDateBazdid as varchar(12),
  @lIsWarmLine AS BIT = 0 ----------omid
AS
	DECLARE @lWhere as varchar(4000)
	DECLARE @lWhereDT as varchar(4000)
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
		set @lWhere = @lWhere + ' AND Tbl_MPFeeder.MPFeederId IN ( ' + cast(@lMPFeederIds as varchar)+' ) '
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
  if @lIsWarmLine = 1  -----omid
		set @lWhere = @lWhere + ' AND BTblService.IsWarmLine = 1'
	if @lBazdidSpeciality <> ''
	BEGIN
		set @lWhere = @lWhere + ' AND ISNULL(tTS.BazdidSpecialityId,1) IN (' + @lBazdidSpeciality + ')'
		SET @lJoinSpecialitySql = ' LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId ' +
								' LEFT JOIN BTblTimingSpeciality tTS ON BTblBazdidTiming.BazdidTimingId = tTS.BazdidTimingId '
	END

	set @lSql =
	'
	CREATE TABLE #TmpTbl_MPFeedersId
	(
		MPFeederId int
	)
	INSERT #TmpTbl_MPFeedersId EXEC spGetMPFeedersId ''' + @lAreaId  + ''',' + cast(@lMPPostId as varchar) + '
	SELECT	DISTINCT
		Tbl_Area.AreaId,
		Tbl_Area.Area,
		Tbl_MPPost.MPPostName,
		Tbl_MPFeeder.MPFeederId,
		Tbl_MPFeeder.MPFeederName,
		ISNULL(Tbl_MPFeeder.HavaeiLength,0) AS HavayiLen,
		ISNULL(Tbl_MPFeeder.ZeminiLength,0) AS ZaminiLen,
		Tbl_StartEndDate.StartDate,
		Tbl_StartEndDate.EndDate,
		Tbl_StartEndDate.BazdidCount,
		Tbl_StartEndDate.ServiceCount,
		BTbl_BazdidMaster.Name,
		BTblBazdidResult.FromPathTypeId,
		Tbl_PathType_From.PathType AS FromPathType,
		BTblBazdidResult.FromPathTYpeValue,
		BTblBazdidResult.ToPathTypeId,
		Tbl_PathType_To.PathType AS ToPathType,
		BTblBazdidResult.ToPathTypeValue,
		BTblBazdidResult.BazdidResultId,
		Tbl_FeederPart.FeederPart
	FROM 
		BTblBazdidResultAddress
		INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId
		INNER JOIN BTblBazdidResult ON BTblBazdidResultAddress.BazdidResultId = BTblBazdidResult.BazdidResultId
		INNER JOIN Tbl_MPFeeder ON BTblBazdidResult.MPFeederId = Tbl_MPFeeder.MPFeederId
		INNER JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId
		LEFT OUTER JOIN BTblService ON BTblServiceCheckList.ServiceId = BTblService.ServiceId 
		LEFT OUTER JOIN BTbl_BazdidMaster ON BTblService.BazdidMasterId = BTbl_BazdidMaster.BazdidMasterId
		LEFT OUTER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId
		LEFT OUTER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId
		LEFT OUTER JOIN Tbl_PathType Tbl_PathType_From ON BTblBazdidResult.FromPathTypeId = Tbl_PathType_From.PathTypeId
		LEFT OUTER JOIN Tbl_PathType Tbl_PathType_To ON BTblBazdidResult.ToPathTypeId = Tbl_PathType_To.PathTypeId
		LEFT JOIN Tbl_FeederPart On BTblBazdidResult.FeederPartId = Tbl_FeederPart.FeederPartId
		' + @lJoinSpecialitySql + '
		INNER JOIN
		(
		SELECT
			BTblBazdidResult.BazdidResultId,
			MIN(BtblServiceCheckList.DoneDatePersian) AS StartDate,
			MAX(BtblServiceCheckList.DoneDatePersian) AS EndDate,
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
			LEFT JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId
			LEFT OUTER JOIN BTblService ON BTblServiceCheckList.ServiceId = BTblService.ServiceId 
			INNER JOIN Tbl_MPFeeder ON BTblBazdidResult.MPFeederId = Tbl_MPFeeder.MPFeederId
			' + @lJoinSpecialitySql + '
		WHERE
			BTblBazdidResult.BazdidTypeId = 1
			' + @lWhere + @lWhereDT + '
		GROUP BY
			BTblBazdidResult.BazdidResultId
		) AS Tbl_StartEndDate ON BTblBazdidResult.BazdidResultId = Tbl_StartEndDate.BazdidResultId
	WHERE 
		(BTblBazdidResult.BazdidTypeId = 1)
		AND BtblServiceCheckList.ServiceStateId = 3
		' + @lWhere + '
	DROP TABLE #TmpTbl_MPFeedersId
	'
	EXEC(@lSql)
GO
--------------------------------------------------------------------------------------------------------------------------

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
--------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE dbo.spGetReport_2_1_8
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

  if @lIsWarmLine = 1  -----omid
		set @lWhere = @lWhere + ' AND BTblService.IsWarmLine = 1'
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
--------------------------------------------------------------------------------------------------------------------------

ALTER PROCEDURE dbo.spGetReport_2_2_1
	@lStartDatePersian as varchar(12),
	@lEndDatePersian as varchar(12),
	@lAreaId as varchar(1000),
	@lMPPostId as int,
	@lMPFeederIds as varchar(1000),
	@lLPPostId as int,
	@lOwnershipId as int,
	@lIsActive as int,
	@lBazdidSpeciality as varchar(100) = '',
	@lFromDateBazdid as varchar(12),
	@lToDateBazdid as varchar(12),
  @lIsWarmLine AS BIT = 0 ----------omid
AS
	DECLARE @lWhere as varchar(4000)
	DECLARE @lWhere2 as varchar(4000)
	DECLARE @lSql as varchar(8000)
	DECLARE @lWhereDT as varchar(4000)
	DECLARE @lJoinSpecialitySql as varchar(500) = ''

	set @lWhere = ''
	set @lWhere2 = ''
	set @lWhereDT = ''

	if @lStartDatePersian <> ''
		set @lWhereDT =  ' AND DoneDatePersian >= ''' + @lStartDatePersian +''''
	if @lEndDatePersian <> ''
		set @lWhereDT = @lWhereDT + ' AND DoneDatePersian <= ''' + @lEndDatePersian +''''
	
	if @lFromDateBazdid <> ''
		SET @lWhereDT = @lWhereDT + ' AND BTblBazdidResultAddress.StartDatePersian >= ''' + @lFromDateBazdid +''''
	if @lToDateBazdid <> ''
		SET @lWhereDT = @lWhereDT + ' AND BTblBazdidResultAddress.StartDatePersian <= ''' + @lToDateBazdid +''''

		
	if @lMPFeederIds <> ''
	BEGIN
		set @lWhere = @lwhere + ' AND Tbl_MPFeeder.MPFeederId IN ( ' + cast(@lMPFeederIds as varchar) + ')'
		set @lWhere2 = @lwhere2 + ' AND Tbl_MPFeeder.MPFeederId IN ( ' + cast(@lMPFeederIds as varchar) + ')'
	END
	ELSE if @lMPPostId > -1 OR @lAreaId <> ''
		set @lWhere = @lWhere + ' AND Tbl_MPFeeder.MPFeederId IN (SELECT MPFeederId FROM #TmpTbl_MPFeedersId)'
	if @lAreaId <> ''
		set @lWhere2 = @lwhere2 + ' AND Tbl_LPPost.AreaId IN ( ' + @lAreaId + ' )'
	if @lMPPostId > -1
		set @lWhere2 = @lwhere2 + ' AND Tbl_MPFeeder.MPPostId = ' + cast(@lMPPostId as varchar)
	if @lLPPostId > -1
		set @lWhereDT = @lWhereDT + ' AND Tbl_LPPost.LPPostId = ' + cast(@lLPPostId as varchar)
	if @lOwnershipId  > -1
		set @lWhere = @lwhere + ' AND Tbl_MPFeeder.OwnershipId = ' + cast(@lOwnershipId as varchar)
	if @lIsActive <> 0
		set @lWhere = @lWhere + ' AND Tbl_MPFeeder.IsActive = 1 '
	if @lBazdidSpeciality <> ''
	BEGIN
		set @lWhereDT = @lWhereDT + ' AND ISNULL(tTS.BazdidSpecialityId,1) IN (' + @lBazdidSpeciality + ')'
		SET @lJoinSpecialitySql = ' LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId ' +
								' LEFT JOIN BTblTimingSpeciality tTS ON BTblBazdidTiming.BazdidTimingId = tTS.BazdidTimingId '
	END
  IF @lIsWarmLine = 1  -----omid
	BEGIN
		SET @lWhereDT = @lWhereDT + ' AND BTblService.IsWarmLine = 1'
		SET @lJoinSpecialitySql = ' LEFT OUTER JOIN BTblService ON BTblServiceCheckList.ServiceId = BTblService.ServiceId '
	END
	set @lWhereDT = @lWhereDT + @lWhere2
	if( @lWhere2 <> '' )
		set @lWhere2 = ' WHERE ' + Right( @lWhere2 , LEN(@lWhere2) - 4 ) 

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
		Tbl_MPFeeder.MPFeederId,
		Tbl_MPFeeder.MPFeederName,
		Tbl_MPFeeder.Address,
		Tbl_LPPostCount.cntHavayi,
		Tbl_LPPostCount.cntZamini,
		Tbl_Bazdid.cntBazdid,
		Tbl_Bazdid.cntService,
		Tbl_Bazdid.StartDate,
		Tbl_Bazdid.EndDate
	FROM
		Tbl_MPFeeder
		INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId =Tbl_MPPost.MPPostId
		INNER JOIN
		(
			SELECT
				BTblBazdidResult.AreaId,
				Tbl_LPPost.MPFeederId,
				SUM(CASE WHEN BTblBazdidResult.BazdidStateId IN (2, 3) 
					THEN CASE WHEN BTblBazdidResultCheckList.DefectionCount Is Null OR BTblBazdidResultCheckList.DefectionCount = 0 
						THEN 1 ELSE BTblBazdidResultCheckList.DefectionCount END END) AS cntBazdid, 
				SUM(CASE WHEN BTblServiceCheckList.ServiceStateId = 3 OR (BTblServiceCheckList.ServiceStateId = 2 AND BTblServiceCheckList.ServiceCount > 0)
					THEN CASE WHEN BTblServiceCheckList.ServiceCount Is Null OR BTblServiceCheckList.ServiceCount = 0 
						THEN 1 ELSE BTblServiceCheckList.ServiceCount END END) AS cntService,
				MIN(BTblServiceCheckList.DoneDatePersian) AS StartDate,
				MAX(BTblServiceCheckList.DoneDatePersian) AS EndDate
			FROM
				BTblBazdidResultCheckList
				INNER JOIN BTblBazdidResultAddress ON BTblBazdidResultCheckList.BazdidResultAddressId = BTblBazdidResultAddress.BazdidResultAddressId
				INNER JOIN BTblBazdidResult ON BTblBazdidResultAddress.BazdidResultId = BTblBazdidResult.BazdidResultId
				LEFT JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId
				INNER JOIN Tbl_LPPost ON BTblBazdidResult.LPPostId = Tbl_LPPost.LPPostId
				INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId
				' + @lJoinSpecialitySql + '
			WHERE
				BTblBazdidResult.BazdidTypeId = 2
				' + @lWhereDT + '
			GROUP BY
				BTblBazdidResult.AreaId,
				Tbl_LPPost.MPFeederId
		) AS Tbl_Bazdid ON Tbl_MPFeeder.MPFeederId = Tbl_Bazdid.MPFeederId	
		INNER JOIN Tbl_Area ON Tbl_Bazdid.AreaId = Tbl_Area.AreaId
		INNER JOIN
		(
			SELECT
				DISTINCT Tbl_LPPost.MPFeederId, 
				COUNT(CASE WHEN IsHavayi = 1 THEN LPPostId END) AS cntHavayi,
				COUNT(CASE WHEN IsHavayi = 0 THEN LPPostId END) AS cntZamini
			FROM 
				Tbl_LPPost
				INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId
			' + @lWhere2 + '
			GROUP BY 
				Tbl_LPPost.MPFeederId
		) AS Tbl_LPPostCount ON Tbl_MPFeeder.MPFeederId = Tbl_LPPostCount.MPFeederId
	'
	if( @lWhere <> '' )
		set @lSql = @lSql + 'WHERE ' + Right( @lWhere , LEN(@lWhere) - 4 )
EXEC(@lSql)
GO
--------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE dbo.spGetReport_2_2_2
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
	@lToDateBazdid as varchar(12),
  @lIsWarmLine AS BIT = 0 ----------omid
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
--------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE dbo.spGetReport_2_2_5
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
   IF @lIsWarmLine = 1  -----omid
	BEGIN
		SET @lWhere = @lWhere + ' AND BTblService.IsWarmLine = 1'
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
--------------------------------------------------------------------------------------------------------------------------
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
				CAST(''⁄œ„ ÊÃÊœ  ÃÂÌ“'' as nvarchar) 
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
--------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE dbo.spGetReport_2_2_8
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
	@lToDateBazdid as varchar(12),
  @lIsWarmLine AS BIT = 0 ----------omid
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
	IF @lIsWarmLine = 1  -----omid
	BEGIN
		SET @lWhere = @lWhere + ' AND BTblService.IsWarmLine = 1'
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
				CAST(''⁄œ„ ÊÃÊœ  ÃÂÌ“'' as nvarchar) 
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
--------------------------------------------------------------------------------------------------------------------------
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
		SET @lWhere = @lWhere + ' AND BTblService.IsWarmLine = 1'
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
--------------------------------------------------------------------------------------------------------------------------
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
--------------------------------------------------------------------------------------------------------------------------
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
--------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE dbo.spGetReport_2_34_5
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
  IF @lIsWarmLine = 1  -----omid
		SET @lWhere = @lWhere + ' AND BTblService.IsWarmLine = 1'
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
--------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE dbo.spGetReport_2_34_6
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
	@aNotservice as int,
	@lBazdidSpeciality as varchar(100) = '',
	@lFromDateBazdid as varchar(12),
	@lToDateBazdid as varchar(12),
  @lIsWarmLine AS BIT = 0 ----------omid
AS
	DECLARE @lWhere as varchar(6000)
	DECLARE @lServiceCheckList as varchar(8000)
	DECLARE @lSql as varchar(8000)
	DECLARE @lJoinSpecialitySql as varchar(500) = ''
	
	set @lServiceCheckList=' OR BTblServiceCheckList.ServiceStateId <> 3 '

	set @lWhere = ''

	if @lFromDatePersian <> ''
		set @lWhere =  ' AND BTblBazdidResultAddress.StartDatePersian >= ''' + @lFromDatePersian +''''
	if @lToDatePersian <> ''
		set @lWhere = @lWhere + ' AND BTblBazdidResultAddress.StartDatePersian <= ''' + @lToDatePersian +''''
		
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
	if @lIsActive = 1
		set @lWhere = @lWhere + ' AND Tbl_LPFeeder.IsActive = 1 '
    if @lBazdidMaster <> ''
		set @lWhere = @lWhere + ' AND BTblBazdidTiming.BazdidMasterId IN (' + @lBazdidMaster + ')'
	if @lMPFeederPart <> ''
		set @lWhere = @lWhere + ' AND BTblBazdidResult.BazdidBasketDetailId IN (' + @lMPFeederPart + ')'
	if @lPriority <> ''
		set @lWhere = @lWhere + ' AND BTblBazdidResultCheckList.Priority IN (' + @lPriority + ')'
	if @lCheckLists <> ''
		set @lWhere = @lWhere + ' AND BTblBazdidResultCheckList.BazdidCheckListId IN (' + @lCheckLists + ')'
   if @aNotservice = 1
		  set @lServiceCheckList = ''
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
		SELECT 
			Tbl_Area.AreaId,
			Tbl_Area.Area,
			Tbl_LPFeeder.LPPostId,
			Tbl_LPPost.LPPostName,
			Tbl_LPFeeder.LPFeederId,
			Tbl_LPFeeder.LPFeederName,
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
			BTblBazdidResultAddress.StartDatePersian,
			BTblBazdidResultAddress.Address,
			BTblBazdidResultAddress.GPSx,
			BTblBazdidResultAddress.GPSy,
			BTbl_BazdidCheckList.CheckListCode,
			BTbl_BazdidCheckList.CheckListName,
			BTblBazdidResultSubCheckList.BazdidResultSubCheckListId , 
			BTbl_SubCheckList.SubCheckListName, 
			CASE WHEN BTblBazdidResultCheckList.Priority = 4 THEN 
				CAST(''⁄œ„ ÊÃÊœ  ÃÂÌ“'' as nvarchar) 
				ELSE CAST(BTblBazdidResultCheckList.Priority as nvarchar) END AS Priority,
			Tbl_LPFeeder.IsLightFeeder,
			BTbl_BazdidCheckListGroup.IsHavayi,
			BTblBazdidTiming.BazdidName,
			SUM(BTblBazdidResultCheckList.DefectionCount) as  DefectionCount,
			SUM(BTblBazdidResultCheckList.DefectionCount - IsNull(BTblServiceCheckList.ServiceCount,0)) AS CheckListCount
		FROM 
			Tbl_LPFeeder
			INNER JOIN Tbl_LPPost ON Tbl_LPFeeder.LPPostId = Tbl_LPPost.LPPostId
			INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId
			INNER JOIN BTblBazdidResult ON Tbl_LPFeeder.LPFeederId = BTblBazdidResult.LPFeederId
			INNER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId
			INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId
			INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId
			LEFT JOIN BTblBazdidResultSUBCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblBazdidResultSUBCheckList.BazdidResultCheckListId
			LEFT JOIN  BTbl_SubCheckList  ON BTblBazdidResultSUBCheckList.SubCheckListId = BTbl_SubCheckList.SubCheckListId 
			
			LEFT JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId
			LEFT OUTER JOIN Tbl_PathType Tbl_PathType_From ON BTblBazdidResult.FromPathTypeId = Tbl_PathType_From.PathTypeId
			LEFT OUTER JOIN Tbl_PathType Tbl_PathType_To ON BTblBazdidResult.ToPathTypeId = Tbl_PathType_To.PathTypeId
			INNER JOIN BTbl_BazdidCheckList ON BTblBazdidResultCheckList.BazdidCheckListId = BTbl_BazdidCheckList.BazdidCheckListId
			LEFT OUTER JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId
			LEFT JOIN BTbl_BazdidCheckListGroup ON BTbl_BazdidCheckList.BazdidCheckListGroupId = BTbl_BazdidCheckListGroup.BazdidCheckListGroupId
			' + @lJoinSpecialitySql + '
		WHERE 
			BTblBazdidResult.BazdidStateId IN (2,3)
			AND BTblBazdidResult.BazdidTypeId = 3
			AND BTblBazdidResultCheckList.Priority > 0
			AND Tbl_LPFeeder.IsLightFeeder = ' + cast(@lIsLight as varchar) + '
			AND 
			(
				BTblServiceCheckList.ServiceCheckListId IS NULL
				' + @lServiceCheckList + '
			)
			' + @lWhere + '
			Group By 
			Tbl_Area.AreaId,
			Tbl_Area.Area,
			Tbl_LPFeeder.LPPostId,
			Tbl_LPPost.LPPostName,
			Tbl_LPFeeder.LPFeederId,
			Tbl_LPFeeder.LPFeederName,
			BTblBazdidResult.BazdidResultId,
			Tbl_LPFeeder.HavaeiLength,
			Tbl_LPFeeder.ZeminiLength,
			FromToLengthHavayi,
			Tbl_LPFeeder.HavaeiLength,
			FromToLengthZamini,
			Tbl_LPFeeder.ZeminiLength,
			BTblBazdidResult.FromPathTypeId,
			Tbl_PathType_From.PathType,
			BTblBazdidResult.FromPathTYpeValue,
			BTblBazdidResult.ToPathTypeId,
			Tbl_PathType_To.PathType ,
			BTblBazdidResult.ToPathTypeValue,
			BTblBazdidResultAddress.StartDatePersian,
			BTblBazdidResultAddress.Address,
			BTblBazdidResultAddress.GPSx,
			BTblBazdidResultAddress.GPSy,
			BTbl_BazdidCheckList.CheckListCode,
			BTbl_BazdidCheckList.CheckListName,
			BTblBazdidResultSubCheckList.BazdidResultSubCheckListId , 
			BTbl_SubCheckList.SubCheckListName, 
			BTblBazdidResultCheckList.Priority,
			Tbl_LPFeeder.IsLightFeeder,
			BTbl_BazdidCheckListGroup.IsHavayi,
			BTblBazdidTiming.BazdidName '

	Exec(@lSql)
GO
--------------------------------------------------------------------------------------------------------------------------

--------------------------------------------------------------------------------------------------------------------------

--------------------------------------------------------------------------------------------------------------------------

--------------------------------------------------------------------------------------------------------------------------
