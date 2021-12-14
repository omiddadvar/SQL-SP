
ALTER PROCEDURE dbo.spGetReport_8_24 @aFromPeakDatePersian AS VARCHAR(50)
	,@aToPeakDatePersian AS VARCHAR(50)
	,@aPeakTime AS VARCHAR(10)
	,@aAreaIds AS INT
	,@aMPPostId AS INT
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
		SET @lWhere += ' AND Tbl_MPFeeder.AreaId IN (' + CAST(@aAreaIds AS VARCHAR(10)) + ')'
	IF @aMPPostId <> -1
		SET @lWhere += ' AND Tbl_MPPost.MPPostId IN (' + CAST(@aMPPostId AS VARCHAR(20)) + ')'
	IF @aMPFeederId > -1
		SET @lWhere += ' AND Tbl_MPFeeder.MPFeederId = ' + cast(@aMPFeederId as varchar(20))
	IF @aIsActive > -1
		SET @lWhere += ' AND Tbl_MPPost.IsActive = ' + cast(@aIsActive as varchar(20))
	IF @aOwnershipId > -1
		SET @lWhere += ' AND Tbl_MPPost.MPPostOwnershipId = ' + cast(@aOwnershipId as varchar(20))
	
	/*Whether is Exact (@aPeakTime) or an Interval (~@aPeakTime)*/
	IF @aPeakTime = '' AND @aFromPeakDatePersian <> ''
		SET @lWhere += ' AND Tbl_MPFeederLoad.RelDatePersian >= ''' + @aFromPeakDatePersian + ''''
	IF @aPeakTime = '' AND @aToPeakDatePersian <> ''
		SET @lWhere += ' AND Tbl_MPFeederLoad.RelDatePersian <= ''' + @aToPeakDatePersian + ''''
	IF @aPeakTime <> '' AND @aFromPeakDatePersian <> ''
		SET @lWhere += ' AND Tbl_MPFeederLoad.RelDatePersian = ''' + @aFromPeakDatePersian + ''''
				+ ' AND Tbl_Hour.Hour = ''' + @aPeakTime + ''''
	/*Remove first "AND"*/
	IF @lWhere <> '' BEGIN
		SET @lWhere = RIGHT(@lWhere , LEN(@lWhere) - 4)
    SET @lWhere = ' WHERE ' + @lWhere
  END
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
		LEFT JOIN Tbl_Area ON Tbl_MPFeeder.AreaId = Tbl_Area.AreaId ' + @lWhere
    
	EXEC(@lSQL)
END
GO