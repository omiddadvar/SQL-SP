ALTER PROCEDURE spGetReport_8_24 
	@aFromPeakDatePersian AS VARCHAR(10)
	,@aToPeakDatePersian AS VARCHAR(10) 
	,@aPeakTime AS VARCHAR(10)
	,@aAreaIds AS VARCHAR(2000) = ''
	,@aMPPostIds AS VARCHAR(5000) = ''
	,@aMPFeederId AS INT = -1
	,@aIsActive AS INT = -1
	,@aOwnershipId AS INT = -1
  ,@aIsSortByFeeder AS BIT = 1 /* Else => By Date */
AS
BEGIN
	DECLARE @lWhere AS VARCHAR(2000) = ''
	       ,@lSQL AS VARCHAR(4000) = ''
	
	IF @aAreaIds <> ''
		SET @lWhere += ' AND Tbl_MPFeeder.AreaId IN (' + @aAreaIds + ')'
	IF @aMPPostIds <> ''
		SET @lWhere += ' AND Tbl_MPPost.MPPostId IN (' + @aMPPostIds + ')'
	IF @aMPFeederId > -1
		SET @lWhere += ' AND Tbl_MPFeeder.MPFeederId = ' + CAST(@aMPFeederId AS VARCHAR(20))
	IF @aIsActive > -1
		SET @lWhere += ' AND Tbl_MPPost.IsActive = ' + CAST(@aIsActive AS VARCHAR(1))
	IF @aOwnershipId > -1
		SET @lWhere += ' AND Tbl_MPPost.MPPostOwnershipId = ' + CAST(@aOwnershipId AS VARCHAR(20))
	
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
		ROUND(ISNULL(Tbl_MPFeederLoadHours.CurrentValue,0) , 2) AS CurrentValue, 
		ROUND(ISNULL(Tbl_MPFeederLoadHours.CurrentValueReActive,0) , 2) AS CurrentValueReActive, 
		Tbl_MPFeederLoadHours.PowerValue
		FROM Tbl_MPFeeder
		INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId 
		LEFT OUTER JOIN Tbl_MPFeederLoad ON Tbl_MPFeeder.MPFeederId = Tbl_MPFeederLoad.MPFeederId
		INNER JOIN Tbl_MPFeederLoadHours ON Tbl_MPFeederLoad.MPFeederLoadId = Tbl_MPFeederLoadHours.MPFeederLoadId
		LEFT JOIN Tbl_Hour ON Tbl_MPFeederLoadHours.HourId = Tbl_Hour.HourId
		LEFT JOIN Tbl_Area ON Tbl_MPFeeder.AreaId = Tbl_Area.AreaId
		' + @lWhere
    
    IF @aIsSortByFeeder = 1
      SET @lSQL = @lSQL + ' ORDER BY Tbl_MPFeeder.MPFeederId , Tbl_MPFeederLoad.RelDate DESC'
    ELSE
      SET @lSQL = @lSQL + ' ORDER BY Tbl_MPFeederLoad.RelDate DESC ,Tbl_MPFeeder.MPFeederId'
  
  PRINT(@lSQL)
  EXEC(@lSQL)
END
GO