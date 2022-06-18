
ALTER PROC Emergency.spGetFeederGroupPlan 
  @aGroupMPFeederId AS BIGINT
  ,@aDayCount AS INT
	,@aTimingId AS INT = - 1
  ,@aDisDatePersian AS VARCHAR(10)
  ,@aDisTime AS VARCHAR(5)
AS
BEGIN
  DECLARE @lDisconnectDatTime AS DATETIME = dbo.ShamsiDateTimeToMiladi(@aDisDatePersian,@aDisTime)

	CREATE TABLE #tmpResult (
		MPFeederTemplateId INT
    ,Area NVARCHAR(50)
		,MPFeederId INT
		,MPPostName NVARCHAR(30)
		,MPFeederName NVARCHAR(50)
		,MPFeederDisconnectCount INT
		,CurrentValueMW FLOAT
		,CurrentValue FLOAT
		,IsSelected BIT
		)

	INSERT INTO #tmpResult
	EXEC Emergency.spGetFeederGroupPlanRaw @aGroupMPFeederId, @aDayCount, @lDisconnectDatTime 

	UPDATE #tmpResult
  	SET IsSelected = CASE 
  			WHEN TMPF.TimingMPFeederId IS NOT NULL
  				THEN CAST(1 AS BIT)
  			ELSE CAST(0 AS BIT)
  			END
  FROM #tmpResult
  	INNER JOIN Emergency.Tbl_MPFeederTemplate MPFT ON #tmpResult.MPFeederTemplateId = MPFT.MPFeederTemplateId
  	LEFT JOIN Emergency.TblTiming T ON MPFT.GroupMPFeederId = T.GroupMPFeederId
  	LEFT JOIN Emergency.TblTimingMPFeeder TMPF ON T.TimingId = TMPF.TimingId
  		AND TMPF.MPFeederTemplateId = MPFT.MPFeederTemplateId
    WHERE ISNULL(T.TimingId , -1) = @aTimingId

	SELECT * , CAST(1 AS BIT) AS IsOk FROM #tmpResult
  ORDER BY MPFeederDisconnectCount DESC

	DROP TABLE #tmpResult
END
GO
/*-----------------------------------------------------------------------------*/

ALTER FUNCTION Emergency.GetMPFeederDisconnectCount (
	@MPFeederId AS INT
	,@IsDisconnectMPFeeder AS BIT
	,@MPFeederKeyId AS BIGINT,
  @aDayCount AS INT
	)
RETURNS INT
AS
BEGIN
	DECLARE @CntMP1 AS INT = 0
	DECLARE @CntMP2 AS INT = 0
	DECLARE @CntFT AS INT = 0

  IF @aDayCount IS NULL OR @aDayCount <= 0 BEGIN
    RETURN 0                                         	
  END

	SELECT @CntMP1 = COUNT(*)
	FROM TblMPRequest
	INNER JOIN TblRequest ON TblMPRequest.MPRequestId = TblRequest.MPRequestId
	WHERE TblMPRequest.DisconnectDT >= DATEADD(day, - 20, getdate())
		AND ISNULL(TblRequest.IsDisconnectMPFeeder, 1) = 1
		AND TblMPRequest.MPFeederId = @MPFeederId

	IF @IsDisconnectMPFeeder = 0
	BEGIN
		SELECT @CntMP2 = COUNT(*)
		FROM TblMPRequest
		INNER JOIN TblRequest ON TblMPRequest.MPRequestId = TblRequest.MPRequestId
		INNER JOIN TblMPRequestKey ON TblMPRequest.MPRequestId = TblMPRequestKey.MPRequestId
		WHERE TblMPRequest.DisconnectDT >= DATEADD(day, - @aDayCount, getdate())
			AND ISNULL(TblRequest.IsDisconnectMPFeeder, 1) = 0
			AND TblMPRequest.MPFeederId = @MPFeederId
			AND TblMPRequestKey.MPFeederKeyId = @MPFeederKeyId
	END

	SELECT @CntFT = COUNT(*)
	FROM dbo.TblFogheToziDisconnect
	INNER JOIN TblRequest ON TblRequest.FogheToziDisconnectId = TblFogheToziDisconnect.FogheToziDisconnectId
	INNER JOIN dbo.TblFogheToziDisconnectMPFeeder ON TblFogheToziDisconnect.FogheToziDisconnectId = TblFogheToziDisconnectMPFeeder.FogheToziDisconnectId
		AND TblRequest.DisconnectDT >= DATEADD(day, - @aDayCount, getdate())
		AND TblFogheToziDisconnectMPFeeder.MPFeederId = @MPFeederId

	RETURN isnull(@CntFT, 0) + isnull(@CntMP1, 0) + isnull(@CntMP2, 0)
END
GO

/*-----------------------------------------------------------------------------*/

ALTER PROC Emergency.spCheckMPFeedersLimit 
  @aStartDate AS VARCHAR(10),
	@aStartTime AS VARCHAR(5),
	@aEndDate AS VARCHAR(10),
	@aEndTime AS VARCHAR(5),
	@aMPFeederTemplateIDs AS VARCHAR(5000) = ''
AS
BEGIN
	DECLARE @lSQL AS VARCHAR(2000) = 'SELECT MPFeederTemplateId FROM Emergency.Tbl_MPFeederTemplate'

	IF LEN(@aMPFeederTemplateIDs) > 0
	BEGIN
		SET @lSQL = @lSQL + '  WHERE MPFeederTemplateId IN(' + @aMPFeederTemplateIDs + ')'
	END

	CREATE TABLE #tmp (MPFeederTemplateId INT)

	INSERT INTO #tmp
	EXEC (@lSQL)

	SELECT 
    MPT.MPFeederTemplateId,
		MPT.MPFeederId,
		MPT.MPFeederLimitTypeId,
		CASE 
			WHEN COUNT(MPFLD.MPFeederLimitDayId) > 0
				THEN CAST(0 AS BIT)
			ELSE CAST(1 AS BIT)
			END AS IsOk,
    COUNT(MPFLD.MPFeederLimitDayId) AS FaultyCount
	FROM 
    Emergency.Tbl_MPFeederTemplate MPT  
  	INNER JOIN #tmp ON MPT.MPFeederTemplateId = #tmp.MPFeederTemplateId
  	INNER JOIN Emergency.Tbl_MPFeederLimitType MPFL ON MPT.MPFeederLimitTypeId = MPFL.MPFeederLimitTypeId
  	LEFT JOIN BTbl_Holiday Holi ON Holi.HolidayDatePersian IN (
  			@aStartDate,
  			@aEndDate
  			)
  	LEFT JOIN Emergency.Tbl_MPFeederLimitDay MPFLD ON MPFL.MPFeederLimitTypeId = MPFLD.MPFeederLimitTypeId
  		AND (
        Emergency.fnCheckMPFeedersLimit_SomeDays(@aStartDate , @aStartTime , @aEndDate, @aEndTime 
                , MPFLD.IsHoliday, Holi.HolidayDatePersian, MPFLD.WeekDayId
                , MPFLD.StartTime , MPFLD.EndTime , 1) = 0
        OR
        Emergency.fnCheckMPFeedersLimit_SomeDays(@aStartDate , @aStartTime , @aEndDate, @aEndTime 
                , MPFLD.IsHoliday, Holi.HolidayDatePersian, MPFLD.WeekDayId
                , MPFLD.StartTime , MPFLD.EndTime , 0) = 0
        OR
        Emergency.fnCheckMPFeedersLimit_AllDays(@aStartDate , @aStartTime , @aEndDate, @aEndTime 
                , MPFLD.IsHoliday, MPFLD.WeekDayId, MPFLD.StartTime , MPFLD.EndTime) = 0
  			)
	GROUP BY MPT.MPFeederTemplateId,
		MPT.MPFeederId,
		MPT.MPFeederLimitTypeId

	DROP TABLE #tmp
END
GO

/*-----------------------------------------------------------------------------*/

ALTER PROC Emergency.spGetMPFeederTiming @aTiminigId AS INTEGER
AS
BEGIN
	SELECT TMPF.TimingMPFeederId
		,TMPF.MPFeederTemplateId
		,TMPF.MPFeederId
    ,A.Area
		,MPP.MPPostName
		,MPF.MPFeederName
    ,TMPF.RequestId
    ,R.RequestNumber
		,TMPF.DisconnectDatePersian
		,TMPF.DisconnectTime
		,TMPF.ConnectDatePersian
		,TMPF.ConnectTime
		,TMPF.ForecastCurrentValue
    ,TMPF.IsDisconnectMPFeeder
    ,TMPF.PreCurrentValue
    ,TMPF.CurrentValue
    ,TMPF.TimingStateId
	INTO #tmp
	FROM Emergency.TblTimingMPFeeder TMPF
  	INNER JOIN Tbl_MPFeeder MPF ON TMPF.MPFeederId = MPF.MPFeederId
  	INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
    INNER JOIN Tbl_Area A ON TMPF.AreaId = A.AreaId
    LEFT JOIN TblRequest R ON TMPF.RequestId = R.RequestId
  WHERE TMPF.TimingId = @aTiminigId

	SELECT T.TimingMPFeederId
		,T.MPFeederTemplateId
		,T.MPFeederId
    ,T.RequestId
    ,T.RequestNumber
    ,T.Area
		,T.MPPostName
		,T.MPFeederName
		,T.DisconnectDatePersian
		,T.DisconnectTime
		,T.ConnectDatePersian
		,T.ConnectTime
		,T.ForecastCurrentValue
    ,T.IsDisconnectMPFeeder
    ,ISNULL(T.PreCurrentValue , 0) AS PreCurrentValue
		,CASE WHEN T.TimingStateId = 5 THEN 0 ELSE 
          ISNULL(T.CurrentValue,
                CASE WHEN T.IsDisconnectMPFeeder = 1 THEN ISNULL(LOAD.CurrentValue, 0) ELSE CAST(0 AS FLOAT) END) 
      END AS CurrentValue
    ,CASE WHEN T.TimingStateId = 5 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS IsCanceled
    ,CASE WHEN T.DisconnectDatePersian IS NULL  THEN 0 WHEN T.ConnectDatePersian IS NULL THEN 1 ELSE 2 END AS ChangeStateId
    ,CAST(0 AS BIT) AS IsSelected
	FROM #tmp T
	LEFT JOIN (
		SELECT L.MPFeederId
			,H.CurrentValue
			,row_number() OVER (
				PARTITION BY L.MPFeederId ORDER BY L.RelDate
					,H.HourId DESC
				) AS RowNum
		FROM Tbl_MPFeederLoad L
		INNER JOIN Tbl_MPFeederLoadHours H ON L.MPFeederLoadId = H.MPFeederLoadId
		) LOAD ON T.MPFeederId = LOAD.MPFeederId
	WHERE ISNULL(LOAD.RowNum, 1) = 1

	DROP TABLE #tmp
END
Go
/*-----------------------------------------------------------------------------*/


ALTER PROC Emergency.spGetFeederGroupPlanRaw 
  @aGroupMPFeederId AS BIGINT,
  @aDayCount AS INT,
  @aDisconnectDT AS DATETIME
AS
BEGIN
  SELECT
    MPT.MPFeederTemplateId
   ,MPF.MPFeederId
   ,MPP.MPPostName
   ,MPF.MPFeederName
   ,Emergency.GetMPFeederDisconnectCount(MPF.MPFeederId, MPT.IsDisconnectMPFeeder, MPT.MPFeederKeyId1,@aDayCount) 
        AS MPFeederDisconnectCount
   ,MPF.Voltage
   ,A.Area
   ,Emergency.GetMPFeederLoadHourId(MPF.MPFeederId , @aDisconnectDT) AS MPFeederLoadHourId
  INTO #tmp
  FROM Emergency.Tbl_MPFeederTemplate MPT
    INNER JOIN Tbl_MPFeeder MPF ON MPT.MPFeederId = MPF.MPFeederId
    INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
    INNER JOIN Tbl_Area A ON MPT.AreaId = A.AreaId
  WHERE GroupMPFeederId = @aGroupMPFeederId

  SELECT
    T.MPFeederTemplateId
   ,T.Area
   ,T.MPFeederId
   ,T.MPPostName
   ,T.MPFeederName
   ,T.MPFeederDisconnectCount
   ,ISNULL(ROUND(3 * T.Voltage * H.CurrentValue * H.CosinPhi / 1000000, 2), 0) AS CurrentValueMW
   ,ISNULL(H.CurrentValue, 0) AS CurrentValue
   ,CAST(0 AS BIT) AS IsSelected
  FROM #tmp T
  LEFT JOIN Tbl_MPFeederLoadHours H ON T.MPFeederLoadHourId = H.MPFeederLoadHourId
  ORDER BY T.MPFeederDisconnectCount

  DROP TABLE #tmp
END
GO


/*-----------------------------------------------------------------------------*/


ALTER PROCEDURE Emergency.spGetLimitDays 
	@aMPFeederLimitTypeId as int = NULL
AS
	IF @aMPFeederLimitTypeId <= 0 SET @aMPFeederLimitTypeId = NULL
	SELECT 
		tLD.*,
		tLT.MPFeederLimitType,
		ISNULL(tWD.WeekDay,CASE WHEN tLD.IsHoliday = 1 THEN N'ÑæÒåÇí ÊÚØíá'ELSE N'ÊãÇãí ÑæåÇí åÝÊå'END) AS WeekDay
	FROM 
		Emergency.Tbl_MPFeederLimitDay tLD
		INNER JOIN Emergency.Tbl_MPFeederLimitType tLT ON tLD.MPFeederLimitTypeId = tLT.MPFeederLimitTypeId
		LEFT JOIN Emergency.Tbl_WeekDay tWD ON tLD.WeekDayId = tWD.WeekDayId
	WHERE
		(@aMPFeederLimitTypeId IS NULL OR tLD.MPFeederLimitTypeId = @aMPFeederLimitTypeId)
GO

/*-----------------------------------------------------------------------------*/

ALTER PROC Emergency.spGetReportDisconnectAreas 
  @aPersianDate AS VARCHAR(10),
  @aAreaIDs AS VARCHAR(2000) = ''
  AS 
  BEGIN

  DECLARE @lSQl AS VARCHAR(MAX) = '
    SELECT TMPF.RequestId, A.Area , G.GroupMPFeederName, MPP.MPPostName, MPF.MPFeederName
      ,R.TamirDisconnectFromDatePersian AS DisconnectDatePersian
      ,R.TamirDisconnectFromTime AS DisconnectTime
      ,R.TamirDisconnectToTime AS ConnectDatePersian
      ,R.TamirDisconnectToTime AS ConnectTime
      ,R.Address
    FROM Emergency.TblTiming T 
      INNER JOIN Emergency.TblTimingMPFeeder TMPF ON T.TimingId = TMPF.TimingId
      INNER JOIN TblRequest R ON TMPF.RequestId = R.RequestId
      INNER JOIN Tbl_MPFeeder MPF ON TMPF.MPFeederId = MPF.MPFeederId
      INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
      INNER JOIN Tbl_Area A ON TMPF.AreaId = A.AreaId
      INNER JOIN Emergency.Tbl_GroupMPFeeder G ON T.GroupMPFeederId = G.GroupMPFeederId
    WHERE R.EndJobStateId NOT IN (2,3) AND TMPF.DisconnectDatePersian = ''' + @aPersianDate + ''''
  IF LEN(@aAreaIDs) > 0 BEGIN
     SET @lSQl = @lSQl + ' AND TMPF.AreaId IN(' + @aAreaIDs + ')'
  END
  SET @lSQl = @lSQl + ' ORDER BY TMPF.AreaId, TMPF.DisconnectDatePersian'
  EXEC(@lSQl)
  END
Go
/*-----------------------------------------------------------------------------*/

CREATE FUNCTION dbo.ShamsiDateTimeToMiladi (@aShDate AS VARCHAR(10) , @aShTime AS VARCHAR(5))
RETURNS DATETIME AS
BEGIN
    DECLARE @lDateTime AS DATETIME,
            @lHour AS INT,
            @lMinute AS INT

   SET @lDateTime = dbo.shtom(@aShDate)
   SET @lHour = CAST(SUBSTRING( @aShTime, 1 ,2) AS INT)
   SET @lMinute = CAST(SUBSTRING( @aShTime, 4 ,2) AS INT)
   SET @lDateTime = DATEADD(HOUR, @lHour , @lDateTime)
   SET @lDateTime = DATEADD(MINUTE, @lMinute , @lDateTime)
 
 RETURN @lDateTime
END

Go
/*-----------------------------------------------------------------------------*/
  
CREATE FUNCTION Emergency.GetMPFeederLoadHourId (
	@MPFeederId AS INT,
	@aDisconnectDT AS DATETIME
	)
RETURNS BIGINT
AS
BEGIN
	Declare @lDayOfWeek as Int = DATEPART(dw,@aDisconnectDT),
			@lHour as INT = DATEPART(HOUR,@aDisconnectDT) + 1,
			@lID as BIGINT
	if @lDayOfWeek in (7,1,2,3,4) 
	begin
		Select TOP(1) @lID = H.MPFeederLoadHourId FROM Tbl_MPFeederLoad L
		INNER JOIN Tbl_MPFeederLoadHours H ON L.MPFeederLoadId = H.MPFeederLoadId
		Where L.MPFeederId = @MPFeederId 
			AND H.HourId = @lHour 
			AND L.RelDate <= @aDisconnectDT
			AND DATEPART(dw,L.RelDate) in (7,1,2,3,4)
		Order By L.RelDate DESC
	End
	Else 
	Begin 
		Select TOP(1) @lID = H.MPFeederLoadHourId FROM Tbl_MPFeederLoad L
		INNER JOIN Tbl_MPFeederLoadHours H ON L.MPFeederLoadId = H.MPFeederLoadId
		Where L.MPFeederId = @MPFeederId 
			AND H.HourId = @lHour 
			AND L.RelDate <= @aDisconnectDT
			AND DATEPART(dw,L.RelDate) = @lDayOfWeek
		Order By L.RelDate DESC
	END
	Return @lID
END
GO

/*-----------------------------------------------------------------------------*/

CREATE FUNCTION Emergency.fnCheckMPFeedersLimit_SomeDays(
  @aStartDate AS VARCHAR(10),
  @aStartTime AS VARCHAR(5),
  @aEndDate AS VARCHAR(10),
  @aEndTime AS VARCHAR(5),
  @aIsHoliday AS BIT,
  @aHoliDayDatePersian AS VARCHAR(10),
  @aWeekDayId AS INT,
  @aLimitStartTime AS VARCHAR(5),
  @aLimitEndTime AS VARCHAR(5),
  @aIsStart AS BIT
  ) RETURNS BIT
  AS 
  BEGIN
      DECLARE @lStartDT AS DATETIME = dbo.ShamsiDateTimeToMiladi(@aStartDate, @aStartTime)
  	  DECLARE @lEndtDT AS DATETIME = dbo.ShamsiDateTimeToMiladi(@aEndDate, @aEndTime)
    	DECLARE @lStartDayOfWeekId AS INT = DATEPART(DW, @lStartDT)
  	  DECLARE @lEndDayOfWeekId AS INT = DATEPART(DW, @lEndtDT)
      DECLARE @lState AS BIT = CAST(0 AS BIT)
      /*------------------------------------------------------*/
      DECLARE @lStartCheckDT AS DATETIME = 
                    dbo.ShamsiDateTimeToMiladi(CASE WHEN @aIsStart = 1 THEN @aStartDate ELSE @aEndDate END, @aLimitStartTime) 
      DECLARE @lEndCheckDT AS DATETIME = 
                    dbo.ShamsiDateTimeToMiladi(CASE WHEN @aIsStart = 1 THEN @aStartDate ELSE @aEndDate END, @aLimitEndTime) 
      /*------------------------------------------------------*/
      SET @lState = CASE 
                      WHEN (
                      @aWeekDayId = CASE WHEN @aIsStart = 1 THEN @lStartDayOfWeekId  ELSE @lEndDayOfWeekId END
                      OR (@aIsHoliday = 1 
                          AND (@aHoliDayDatePersian = CASE WHEN @aIsStart = 1 THEN @aStartDate ELSE @aEndDate END))
                      )    
                      AND (
                        (@lStartCheckDT > @lStartDT AND @lStartCheckDT < @lEndtDT) 
                        OR
                        (@lEndCheckDT > @lStartDT AND @lEndCheckDT < @lEndtDT)
                      ) THEN CAST(0 AS BIT)
                    	ELSE CAST(1 AS BIT)
                    END
      RETURN @lState
  END
Go

/*-----------------------------------------------------------------------------*/

CREATE FUNCTION Emergency.fnCheckMPFeedersLimit_AllDays(
  @aStartDate AS VARCHAR(10),
  @aStartTime AS VARCHAR(5),
  @aEndDate AS VARCHAR(10),
  @aEndTime AS VARCHAR(5),
  @aIsHoliday AS BIT,
  @aWeekDayId AS INT,
  @aLimitStartTime AS VARCHAR(5),
  @aLimitEndTime AS VARCHAR(5)
  ) RETURNS BIT
  AS
  BEGIN
      DECLARE @lStartDT AS DATETIME = dbo.ShamsiDateTimeToMiladi(@aStartDate, @aStartTime)
  	  DECLARE @lEndtDT AS DATETIME = dbo.ShamsiDateTimeToMiladi(@aEndDate, @aEndTime)
      DECLARE @lState AS BIT = CAST(0 AS BIT)
      /*-------------------------------------------------------*/
      DECLARE @lStartCheckDTStart AS DATETIME = dbo.ShamsiDateTimeToMiladi(@aStartDate, @aLimitStartTime)
      DECLARE @lStartCheckDTEnd AS DATETIME = dbo.ShamsiDateTimeToMiladi(@aStartDate, @aLimitEndTime) 
      DECLARE @lEndCheckDTStart AS DATETIME = dbo.ShamsiDateTimeToMiladi(@aEndDate, @aLimitStartTime) 
      DECLARE @lEndCheckDTEnd AS DATETIME = dbo.ShamsiDateTimeToMiladi(@aEndDate, @aLimitEndTime) 
      /*-------------------------------------------------------*/
      SET @lState = CASE 
              WHEN (@aIsHoliday = 0 AND @aWeekDayId IS NULL) 
              AND
              (
                (@lStartCheckDTStart > @lStartDT AND @lStartCheckDTStart < @lEndtDT)
                 OR
                (@lStartCheckDTEnd > @lStartDT AND @lStartCheckDTEnd < @lEndtDT)
                 OR
                (@lEndCheckDTStart > @lStartDT AND @lEndCheckDTStart < @lEndtDT)
                 OR
                (@lEndCheckDTEnd > @lStartDT AND @lEndCheckDTEnd < @lEndtDT)
              ) THEN CAST(0 AS BIT)
            	ELSE CAST(1 AS BIT)
            END
      RETURN @lState
  END
Go
/*-----------------------------------------------------------------------------*/
  
CREATE FUNCTION Emergency.GetMPFeederLoadHourId (
	@MPFeederId AS INT,
	@aDisconnectDT AS DATETIME
	)
RETURNS BIGINT
AS
BEGIN
	Declare @lDayOfWeek as Int = DATEPART(dw,@aDisconnectDT),
			@lHour as INT = DATEPART(HOUR,@aDisconnectDT) + 1,
			@lID as BIGINT
	if @lDayOfWeek in (7,1,2,3,4) 
	begin
		Select TOP(1) @lID = H.MPFeederLoadHourId FROM Tbl_MPFeederLoad L
		INNER JOIN Tbl_MPFeederLoadHours H ON L.MPFeederLoadId = H.MPFeederLoadId
		Where L.MPFeederId = @MPFeederId 
			AND H.HourId = @lHour 
			AND L.RelDate <= @aDisconnectDT
			AND DATEPART(dw,L.RelDate) in (7,1,2,3,4)
		Order By L.RelDate DESC
	End
	Else 
	Begin 
		Select TOP(1) @lID = H.MPFeederLoadHourId FROM Tbl_MPFeederLoad L
		INNER JOIN Tbl_MPFeederLoadHours H ON L.MPFeederLoadId = H.MPFeederLoadId
		Where L.MPFeederId = @MPFeederId 
			AND H.HourId = @lHour 
			AND L.RelDate <= @aDisconnectDT
			AND DATEPART(dw,L.RelDate) = @lDayOfWeek
		Order By L.RelDate DESC
	END
	Return @lID
END
GO

/*-----------------------------------------------------------------------------*/

CREATE FUNCTION Emergency.fnCheckMPFeedersLimit_SomeDays(
  @aStartDate AS VARCHAR(10),
  @aStartTime AS VARCHAR(5),
  @aEndDate AS VARCHAR(10),
  @aEndTime AS VARCHAR(5),
  @aIsHoliday AS BIT,
  @aHoliDayDatePersian AS VARCHAR(10),
  @aWeekDayId AS INT,
  @aLimitStartTime AS VARCHAR(5),
  @aLimitEndTime AS VARCHAR(5),
  @aIsStart AS BIT
  ) RETURNS BIT
  AS 
  BEGIN
      DECLARE @lStartDT AS DATETIME = dbo.ShamsiDateTimeToMiladi(@aStartDate, @aStartTime)
  	  DECLARE @lEndtDT AS DATETIME = dbo.ShamsiDateTimeToMiladi(@aEndDate, @aEndTime)
    	DECLARE @lStartDayOfWeekId AS INT = DATEPART(DW, @lStartDT)
  	  DECLARE @lEndDayOfWeekId AS INT = DATEPART(DW, @lEndtDT)
      DECLARE @lState AS BIT = CAST(0 AS BIT)
      /*------------------------------------------------------*/
      DECLARE @lStartCheckDT AS DATETIME = 
                    dbo.ShamsiDateTimeToMiladi(CASE WHEN @aIsStart = 1 THEN @aStartDate ELSE @aEndDate END, @aLimitStartTime) 
      DECLARE @lEndCheckDT AS DATETIME = 
                    dbo.ShamsiDateTimeToMiladi(CASE WHEN @aIsStart = 1 THEN @aStartDate ELSE @aEndDate END, @aLimitEndTime) 
      /*------------------------------------------------------*/
      SET @lState = CASE 
                      WHEN (
                      @aWeekDayId = CASE WHEN @aIsStart = 1 THEN @lStartDayOfWeekId  ELSE @lEndDayOfWeekId END
                      OR (@aIsHoliday = 1 
                          AND (@aHoliDayDatePersian = CASE WHEN @aIsStart = 1 THEN @aStartDate ELSE @aEndDate END))
                      )    
                      AND (
                        (@lStartCheckDT > @lStartDT AND @lStartCheckDT < @lEndtDT) 
                        OR
                        (@lEndCheckDT > @lStartDT AND @lEndCheckDT < @lEndtDT)
                      ) THEN CAST(0 AS BIT)
                    	ELSE CAST(1 AS BIT)
                    END
      RETURN @lState
  END
Go

/*-----------------------------------------------------------------------------*/

CREATE FUNCTION Emergency.fnCheckMPFeedersLimit_AllDays(
  @aStartDate AS VARCHAR(10),
  @aStartTime AS VARCHAR(5),
  @aEndDate AS VARCHAR(10),
  @aEndTime AS VARCHAR(5),
  @aIsHoliday AS BIT,
  @aWeekDayId AS INT,
  @aLimitStartTime AS VARCHAR(5),
  @aLimitEndTime AS VARCHAR(5)
  ) RETURNS BIT
  AS
  BEGIN
      DECLARE @lStartDT AS DATETIME = dbo.ShamsiDateTimeToMiladi(@aStartDate, @aStartTime)
  	  DECLARE @lEndtDT AS DATETIME = dbo.ShamsiDateTimeToMiladi(@aEndDate, @aEndTime)
      DECLARE @lState AS BIT = CAST(0 AS BIT)
      /*-------------------------------------------------------*/
      DECLARE @lStartCheckDTStart AS DATETIME = dbo.ShamsiDateTimeToMiladi(@aStartDate, @aLimitStartTime)
      DECLARE @lStartCheckDTEnd AS DATETIME = dbo.ShamsiDateTimeToMiladi(@aStartDate, @aLimitEndTime) 
      DECLARE @lEndCheckDTStart AS DATETIME = dbo.ShamsiDateTimeToMiladi(@aEndDate, @aLimitStartTime) 
      DECLARE @lEndCheckDTEnd AS DATETIME = dbo.ShamsiDateTimeToMiladi(@aEndDate, @aLimitEndTime) 
      /*-------------------------------------------------------*/
      SET @lState = CASE 
              WHEN (@aIsHoliday = 0 AND @aWeekDayId IS NULL) 
              AND
              (
                (@lStartCheckDTStart > @lStartDT AND @lStartCheckDTStart < @lEndtDT)
                 OR
                (@lStartCheckDTEnd > @lStartDT AND @lStartCheckDTEnd < @lEndtDT)
                 OR
                (@lEndCheckDTStart > @lStartDT AND @lEndCheckDTStart < @lEndtDT)
                 OR
                (@lEndCheckDTEnd > @lStartDT AND @lEndCheckDTEnd < @lEndtDT)
              ) THEN CAST(0 AS BIT)
            	ELSE CAST(1 AS BIT)
            END
      RETURN @lState
  END
Go
/*--------------*************--------------------*********----------------------*/
/*--------------*************--------------------*********----------------------*/
/*--------------*************--------------------*********----------------------*/

  
ALTER PROCEDURE dbo.spHomaReportArea	
  @aAreaIDs AS VARCHAR(100)
  ,@aStartDate AS VARCHAR(10)
  ,@aEndDate AS VARCHAR(10)
  ,@aDisGroupSetIDs AS VARCHAR(5000) = ''
  ,@aIsWarmline AS BIT = NULL
AS
BEGIN
	DECLARE @lSQLMain VARCHAR(2000)
    ,@lSQL VARCHAR(2000)
    ,@lIsTamir VARCHAR(50) = 'TblJob.IsTamir = 0 AND'
    ,@lTableName VARCHAR(50) = 'TblRequestMP'
  CREATE TABLE #tmpArea (AreaId INT)
  CREATE TABLE #HomaMPNotTamir (AreaId INT, cnt INT)
  CREATE TABLE #HomaMPTamir (AreaId INT, cnt INT)
  CREATE TABLE #HomaLPNotTamir (AreaId INT, cnt INT)
  CREATE TABLE #HomaLPTamir (AreaId INT, cnt INT)
  CREATE TABLE #HomaSubscriber (AreaId INT, cnt INT)
  INSERT INTO #tmpArea EXEC('SELECT AreaId FROM Tbl_Area WHERE AreaId IN ('+ @aAreaIDs +')')
  SET @lSQLMain ='SELECT TblJob.AreaId,COUNT(*) AS cnt
    	  FROM Homa.['+ @lTableName +']
    	  INNER JOIN Homa.TblRequestCommon
    	    ON ['+ @lTableName +'].RequestCommonId=TblRequestCommon.RequestCommonId
    	  INNER JOIN Homa.TblJob ON TblJob.JobId=TblRequestCommon.JobId
    	  WHERE '+ @lIsTamir +' IsDuplicate=0 AND JobStatusId IN (10,11)
          AND TblJob.AreaId IN ('+ @aAreaIDs +')
          AND TblJob.DisconnectDatePersian BETWEEN '''+ @aStartDate +''' AND '''+ @aEndDate + ''' 
    	  '
  IF LEN(@aDisGroupSetIDs) > 0 BEGIN
      SET @lSQLMain = @lSQLMain + ' AND TblRequestCommon.TblRequestCommon IN (' + @aDisGroupSetIDs + ')'                   	
  END
  IF @aIsWarmline IS NOT NULL BEGIN
      SET @lSQLMain = @lSQLMain + ' AND TblRequestCommon.IsWarmLine = ' + CAST(@aIsWarmline AS VARCHAR(1))                   	
  END
  SET @lSQLMain = @lSQLMain + ' GROUP BY TblJob.AreaId'


  INSERT INTO #HomaMPNotTamir EXEC(@lSQLMain)

  PRINT(@lSQLMain)

  SET @lSQL = REPLACE(@lSQLMain, @lIsTamir , 'TblJob.IsTamir = 1 AND')
  INSERT INTO #HomaMPTamir EXEC(@lSQL)

  SET @lSQL = REPLACE(@lSQLMain, @lTableName , 'TblRequestLP')
  INSERT INTO #HomaLPNotTamir EXEC(@lSQL)

  SET @lSQL = REPLACE(@lSQLMain, @lIsTamir , 'TblJob.IsTamir = 1 AND')
  SET @lSQL = REPLACE(@lSQL, @lTableName , 'TblRequestLP')
  INSERT INTO #HomaLPTamir EXEC(@lSQL)

  SET @lSQL = REPLACE(@lSQLMain, @lIsTamir , '')
  SET @lSQL = REPLACE(@lSQL, @lTableName , 'TblRequestSubscriber')
  INSERT INTO #HomaSubscriber EXEC(@lSQL)

  PRINT(@lSQL)

  SELECT AreaId,
		SUM(TblTraceSummary.TraceLen) AS TraceLen,
		SUM(TblTraceSummary.OnCallHour) AS OnCallHour,
		COUNT(*) AS OnCallCount
	INTO #tmpTrace
  	FROM Homa.TblTraceSummary
  	WHERE TargetDatePersian BETWEEN @aStartDate AND @aEndDate	
  	GROUP BY AreaId

  SELECT Area.Area 
    , ISNULL(MPNT.cnt , 0) AS MPNotTamirCount
    , ISNULL(MPT.cnt , 0) AS MPTamirCount
    , ISNULL(LPNT.cnt , 0)+ ISNULL(Sub.cnt , 0) AS LPNotTamirCount
    , ISNULL(LPT.cnt , 0)  AS LPTamirCount
    , ISNULL(ROUND(TR.TraceLen/1000,2) , 0) AS TraceLen
    , ISNULL(ROUND(OnCallHour,2) , 0) AS OnCallHour
    , ISNULL(TR.OnCallCount , 0) AS OnCallCount
    FROM Tbl_Area Area
    INNER JOIN #tmpArea A ON Area.AreaId = A.AreaId
    LEFT JOIN #HomaMPNotTamir MPNT ON Area.AreaId = MPNT.AreaId
    LEFT JOIN #HomaMPTamir MPT ON  Area.AreaId = MPT.AreaId
    LEFT JOIN #HomaLPNotTamir LPNT ON Area.AreaId = LPNT.AreaId
    LEFT JOIN #HomaLPTamir LPT ON Area.AreaId = LPT.AreaId
    LEFT JOIN #tmpTrace TR ON Area.AreaId = TR.AreaId
    LEFT JOIN #HomaSubscriber Sub ON Area.AreaId = Sub.AreaId
    

	DROP TABLE #HomaMPNotTamir
	DROP TABLE #HomaMPTamir
	DROP TABLE #HomaLPNotTamir
	DROP TABLE #HomaLPTamir
	DROP TABLE #HomaSubscriber
  DROP TABLE #tmpTrace
  DROP TABLE #tmpArea
END
GO

/*-----------------------------------------------------------------------------*/

ALTER PROCEDURE dbo.spGetSubscriberOutageInfoByTrackingCode
	@aTrackingCode as bigint
AS
	DECLARE @lRequestId as BIGINT
	DECLARE @lDuplicatedRequestId as BIGINT
	DECLARE @lEndJobStateId as INT
	
	SET @lRequestId = NULL
	
	SELECT TOP 1 
		@lRequestId = RequestId,
		@lDuplicatedRequestId = DuplicatedRequestId,
		@lEndJobStateId = EndJobStateId
	FROM 
		TblRequest 
	WHERE
		TblRequest.TrackingCode = @aTrackingCode

	IF @lRequestId IS NULL
	BEGIN
		SELECT 
			CAST(0 AS INT) AS Status,
			NULL AS IsTamir, 
			NULL AS DisconnectDate, 
			NULL AS DisconnectTime, 
			NULL AS ConnectDate, 
			NULL AS ConnectTime,
			CAST(0 AS FLOAT) AS DebtPrice
		RETURN
	END
	
	IF @lEndJobStateId = 1
	
	BEGIN
		SELECT 
			@lRequestId = RequestId
		FROM
			TblRequest
		WHERE
			RequestId = @lDuplicatedRequestId
	END
	
	SELECT
		STATUS = 
			CASE WHEN TblRequest.EndJobStateId = 2
				
				THEN CAST(1 AS INT)
			WHEN TblRequest.EndJobStateId = 3
				
				THEN CAST(1 AS INT)
			WHEN TblRequest.EndJobStateId = 4
				
				THEN CAST(2 AS INT)
			WHEN TblRequest.EndJobStateId = 5
				
				THEN CAST(3 AS INT)
			WHEN TblRequest.EndJobStateId = 6
				
				THEN CAST(0 AS INT)
			WHEN TblRequest.EndJobStateId = 8
				
				THEN CAST(4 AS INT)
			WHEN TblRequest.EndJobStateId = 9
				
				THEN CAST(5 AS INT)
			WHEN TblRequest.EndJobStateId = 10
				
				THEN CAST(6 AS INT)
			END,
		TblRequest.IsTamir,
		TblRequest.DisconnectDatePersian AS DisconnectDate,
		TblRequest.DisconnectTime,
		ConnectDate = CASE 
			WHEN IsTamir = 1
				THEN TblRequest.TamirDisconnectToDatePersian
			WHEN NOT EstimateDT IS NULL
				THEN dbo.mtosh(EstimateDT)
		END,
		ConnectTime = CASE 
			WHEN IsTamir = 1 THEN TblRequest.TamirDisconnectToTime
			WHEN NOT EstimateDT IS NULL THEN 
				CASE 
					WHEN LEN(DATEPART(hh, EstimateDT)) = 1 THEN '0' + CAST(DATEPART(hh, EstimateDT) AS VARCHAR)
					ELSE CAST(DATEPART(hh, EstimateDT) AS VARCHAR)
				END 
				+ ':' + 
				CASE 
					WHEN LEN(DATEPART(mi,EstimateDT)) = 1 THEN '0' + CAST(DATEPART(mi,EstimateDT) AS VARCHAR)
					ELSE CAST(DATEPART(mi, EstimateDT) AS VARCHAR)
				END
		END,
		CAST(0 AS FLOAT) AS DebtPrice,
    TblRequest.RequestId,
    TblRequest.IsMPRequest,
    TblRequest.MPRequestId,
    TblRequest.IsFogheToziRequest,
    TblRequest.FogheToziDisconnectId
  INTO #tmpResult
	FROM 
		TblRequest
		LEFT JOIN TblMPRequest ON TblRequest.MPRequestId = TblMPRequest.MPRequestId
	WHERE
		RequestId = @lRequestId

  
  /*_____Update STATUS Kambood : 7 , Arrived : 8_____*/
  UPDATE #tmpResult SET STATUS = CASE 
   	WHEN R.IsTamir = 1 AND (
          (R.IsFogheToziRequest = 1 AND FTD.FogheToziId = 1)
          OR (R.IsMPRequest = 1 AND MP.DisconnectReasonId IN (1235 , 1215))
        ) THEN 7
   	WHEN R.IsTamir = 0 AND (J.JobStatusId = 7) THEN 8
   ELSE STATUS
   END
  FROM #tmpResult R
  LEFT JOIN Homa.TblJob J ON R.RequestId = J.RequestId
  LEFT JOIN TblFogheToziDisconnect FTD ON R.FogheToziDisconnectId = FTD.FogheToziDisconnectId
  LEFT JOIN TblMPRequest MP ON R.MPRequestId = MP.MPRequestId


  SELECT STATUS,
  	IsTamir,
		DisconnectDate,
		DisconnectTime,
    ConnectDate,
    ConnectTime,
    DebtPrice
  FROM  #tmpResult

  DROP TABLE #tmpResult
GO


/*-----------------------------------------------------------------------------*/
ALTER PROCEDURE Homa.spGetOutageInfoByCallerId @CallerId AS VARCHAR(20)
AS
BEGIN
	DECLARE @lMobile AS VARCHAR(20) = NULL
	DECLARE @lTel AS VARCHAR(20) = NULL
	DECLARE @lIsTel AS BIT

	IF LEN(ISNULL(@CallerId, '')) < 8
	BEGIN
		SELECT TOP 0 NULL AS BillingID
			,NULL AS BillingIDTypeId
			,NULL AS BillingIDType

		RETURN;
	END

	IF LEN(@CallerId) < 10
		OR (
			LEN(@CallerId) >= 10
			AND LEFT(RIGHT(@CallerId, 10), 1) <> 9
			)
	BEGIN
		SET @lTel = RIGHT(@CallerId, 8)
		SET @lIsTel = 1
	END
	ELSE
	BEGIN
		IF LEFT(@CallerId, 2) = '09'
		BEGIN
			SET @CallerId = '09' + Right(@CallerId, LEN(@CallerId) - 2)
		END
		ELSE IF LEFT(@CallerId, 1) = '9'
		BEGIN
			SET @CallerId = '09' + Right(@CallerId, LEN(@CallerId) - 1)
		END
		ELSE IF LEFT(@CallerId, 4) = '0098'
		BEGIN
			SET @CallerId = '0' + Right(@CallerId, LEN(@CallerId) - 4)
		END
		ELSE IF LEFT(@CallerId, 3) = '098'
			OR LEFT(@CallerId, 3) = '+98'
		BEGIN
			SET @CallerId = '0' + Right(@CallerId, LEN(@CallerId) - 3)
		END

		SET @lMobile = @CallerId
		SET @lIsTel = 0
	END

	DECLARE @lRegisterId AS BIGINT = - 1

	IF @lIsTel = 1
		SELECT @lRegisterId = RegisterId
		FROM Homa.TblRegister
		WHERE TelNo = @lTel
	ELSE
		SELECT @lRegisterId = RegisterId
		FROM Homa.TblRegister
		WHERE MobileNo = @lMobile

	SELECT tBill.BillingID
		,tBill.BillingIDTypeId
		,tBillType.BillingIDType
		,Tbl_MPFeeder.MPPostId
		,Tbl_MPFeeder.MPFeederId
		,tBill.LPPostId
		,LPFeederId
		,Tbl_MPPost.MPPostName
		,Tbl_MPFeeder.MPFeederName
	INTO #register
	FROM Homa.TblRegister tReg
	INNER JOIN Homa.TblRegisterBillingID tBill ON tReg.RegisterId = tBill.RegisterId
	INNER JOIN Homa.Tbl_BillingIDType tBillType ON tBill.BillingIDTypeId = tBillType.BillingIDTypeId
	INNER JOIN Tbl_LPPost ON Tbl_LPPost.LPPostId = tBill.LPPostId
	INNER JOIN Tbl_MPFeeder ON Tbl_MPFeeder.MPFeederId = Tbl_LPPost.MPFeederId
	INNER JOIN Tbl_MPPost ON Tbl_MPPost.MPPostId = Tbl_MPFeeder.MPPostId
	WHERE tReg.RegisterId = @lRegisterId

	/*select * from #register		*/
	DECLARE @BillingID AS VARCHAR(50)
		,@BillingIDTypeId AS INT
		,@BillingIDType AS NVARCHAR(50)
		,@LPPostId AS INT
		,@LPFeederId AS INT
		,@MPPostId AS INT
		,@MPFeederId AS INT

	SELECT *
		,cast(0 AS INT) AS STATUS
		,0 AS MPFeederId
		,0 AS BillingIDTypeId
		,cast('' AS NVARCHAR(100)) AS BillingID
	INTO #tmpAllReq
	FROM TblRequest
	WHERE RequestId = - 1

	SELECT *
		,cast(0 AS INT) AS STATUS
	INTO #tmpReq2
	FROM TblRequest
	WHERE RequestId = - 1

	DECLARE contact_cursor CURSOR READ_ONLY
	FOR
	SELECT BillingID
		,BillingIDTypeId
		,BillingIDType
		,MPPostId
		,MPFeederId
		,LPPostId
		,LPFeederId
	FROM #register

	OPEN contact_cursor

	FETCH NEXT
	FROM contact_cursor
	INTO @BillingID
		,@BillingIDTypeId
		,@BillingIDType
		,@MPPostId
		,@MPFeederId
		,@LPPostId
		,@LPFeederId

	WHILE @@FETCH_STATUS = 0
	BEGIN
		PRINT '@LPPostId :' + cast(@LPPostId AS NVARCHAR(100))

		INSERT INTO #tmpReq2
		EXEC [dbo].[spGetOutageInfoFromNetworkById] @MPPostId
			,@MPFeederId
			,@LPPostId
			,@LPFeederId

		INSERT INTO #tmpAllReq
		SELECT *
			,@MPFeederId AS MPFeederId
			,@BillingIDTypeId AS BillingIDTypeId
			,@BillingID AS BillingID
		FROM #tmpReq2

		DELETE
		FROM #tmpReq2

		FETCH NEXT
		FROM contact_cursor
		INTO @BillingID
			,@BillingIDTypeId
			,@BillingIDType
			,@MPPostId
			,@MPFeederId
			,@LPPostId
			,@LPFeederId
	END

	CLOSE contact_cursor

	DEALLOCATE contact_cursor

  /*_____Update STATUS Kambood : 7 , Arrived : 8_____*/
    UPDATE #tmpAllReq SET STATUS = CASE 
     	WHEN R.IsTamir = 1 AND (
            (R.IsFogheToziRequest = 1 AND FTD.FogheToziId = 1)
            OR (R.IsMPRequest = 1 AND MP.DisconnectReasonId IN (1235 , 1215))
          ) THEN 7
     	WHEN R.IsTamir = 0 AND (J.JobStatusId = 7) THEN 8
      ELSE STATUS
     END
    FROM #tmpAllReq R 
    LEFT JOIN Homa.TblJob J ON R.RequestId = J.RequestId
    LEFT JOIN TblFogheToziDisconnect FTD ON R.FogheToziDisconnectId = FTD.FogheToziDisconnectId
    LEFT JOIN TblMPRequest MP ON R.MPRequestId = MP.MPRequestId


	SELECT TOP 1 #tmpAllReq.ConnectDatePersian
		,#tmpAllReq.ConnectTime
		,#tmpAllReq.Address
		,#tmpAllReq.RequestNumber
		,#tmpAllReq.DisconnectDatePersian
		,#tmpAllReq.DisconnectTime
		,#tmpAllReq.Comments
		,#tmpAllReq.IsTamir
		,#tmpAllReq.STATUS
		,Tbl_MPFeeder.VoiceCode
		,Tbl_MPFeeder.VoiceId
		,#tmpAllReq.BillingID
	FROM #tmpAllReq
	INNER JOIN Tbl_MPFeeder ON #tmpAllReq.MPFeederId = Tbl_MPFeeder.MPFeederId
	ORDER BY BillingIDTypeId

	DROP TABLE #register

	DROP TABLE #tmpAllReq

	DROP TABLE #tmpReq2
END
GO
/*-----------------------------------------------------------------------------*/
  ALTER PROCEDURE dbo.spGetSubscriberOutageInfo
	@CallerId as varchar(20),
	@BackTime as int
AS
	DECLARE @lMPPostId as INT
	DECLARE @lMPFeederId as INT
	DECLARE @lLPPostId as INT
	DECLARE @lLPFeederId as INT
	DECLARE @lFeederPartId as INT
	DECLARE @lRequestId as BIGINT
	DECLARE @lFeederMode as bit
	DECLARE @lSubscriberId as INT
	DECLARE @lDuplicatedRequestId as BIGINT
	DECLARE @lEndJobStateId as INT
	DECLARE @lIsNotSubscriber as BIT
	
	SET @lMPPostId = NULL
	SET @lMPFeederId = NULL
	SET @lLPPostId = NULL
	SET @lLPFeederId = NULL
	SET @lFeederPartId = NULL
	SET @lRequestId = NULL
	SET @lFeederMode = 0
	SET @lSubscriberId = NULL
	SET @lIsNotSubscriber = 1
	
  SET @BackTime = ISNULL(NULLIF(@BackTime ,0),180) /* Null or 0 => 180 */
	
	DECLARE @lBackDate AS datetime = DATEADD(mi,-@BackTime,GETDATE())
	
	
	SELECT TOP 1 
		@lRequestId = RequestId,
		@lDuplicatedRequestId = DuplicatedRequestId,
		@lEndJobStateId = EndJobStateId
	FROM 
		TblRequest 
	WHERE
		TblRequest.DisconnectDT > @lBackDate
		AND RIGHT(Telephone,8) = RIGHT(@CallerId,8)
	ORDER BY
		TblRequest.DisconnectDT DESC

	IF @lRequestId IS NULL
	BEGIN
		
		SELECT 
			CAST(0 AS INT) AS Status,
			NULL AS IsTamir, 
			NULL AS DisconnectDate, 
			NULL AS DisconnectTime, 
			NULL AS ConnectDate, 
			NULL AS ConnectTime,
			CAST(0 AS FLOAT) AS DebtPrice
		RETURN
	END
	
	
	IF @lEndJobStateId = 1
	
	BEGIN
		SELECT 
			@lRequestId = RequestId
		FROM
			TblRequest
		WHERE
			RequestId = @lDuplicatedRequestId
	END
	
	SELECT
		STATUS = 
			CASE WHEN TblRequest.EndJobStateId = 2
				
				THEN CAST(1 AS INT)
			WHEN TblRequest.EndJobStateId = 3
				
				THEN CAST(1 AS INT)
			WHEN TblRequest.EndJobStateId = 4
				
				THEN CAST(2 AS INT)
			WHEN TblRequest.EndJobStateId = 5
				
				THEN CAST(3 AS INT)
			WHEN TblRequest.EndJobStateId = 6
				
				THEN CAST(0 AS INT)
			WHEN TblRequest.EndJobStateId = 8
				
				THEN CAST(4 AS INT)
			WHEN TblRequest.EndJobStateId = 9
				
				THEN CAST(5 AS INT)
			WHEN TblRequest.EndJobStateId = 10
				
				THEN CAST(6 AS INT)
			END,
		TblRequest.IsTamir,
		TblRequest.DisconnectDatePersian AS DisconnectDate,
		TblRequest.DisconnectTime,
		ConnectDate = CASE 
			WHEN IsTamir = 1
				THEN TblRequest.TamirDisconnectToDatePersian
			WHEN NOT EstimateDT IS NULL
				THEN dbo.mtosh(EstimateDT)
		END,
		ConnectTime = CASE 
			WHEN IsTamir = 1 THEN TblRequest.TamirDisconnectToTime
			WHEN NOT EstimateDT IS NULL THEN 
				CASE 
					WHEN LEN(DATEPART(hh, EstimateDT)) = 1 THEN '0' + CAST(DATEPART(hh, EstimateDT) AS VARCHAR)
					ELSE CAST(DATEPART(hh, EstimateDT) AS VARCHAR)
				END 
				+ ':' + 
				CASE 
					WHEN LEN(DATEPART(mi,EstimateDT)) = 1 THEN '0' + CAST(DATEPART(mi,EstimateDT) AS VARCHAR)
					ELSE CAST(DATEPART(mi, EstimateDT) AS VARCHAR)
				END
		END,
		CAST(0 AS FLOAT) AS DebtPrice,
    TblRequest.RequestId,
    TblRequest.IsMPRequest,
    TblRequest.MPRequestId,
    TblRequest.IsFogheToziRequest,
    TblRequest.FogheToziDisconnectId
  INTO #tmpResult
	FROM 
		TblRequest
		LEFT JOIN TblMPRequest ON TblRequest.MPRequestId = TblMPRequest.MPRequestId
	WHERE
		RequestId = @lRequestId


  /*_____Update STATUS Kambood : 7 , Arrived : 8_____*/
  UPDATE #tmpResult SET STATUS = CASE 
   	WHEN R.IsTamir = 1 AND (
          (R.IsFogheToziRequest = 1 AND FTD.FogheToziId = 1)
          OR (R.IsMPRequest = 1 AND MP.DisconnectReasonId IN (1235 , 1215))
        ) THEN 7
   	WHEN R.IsTamir = 0 AND (J.JobStatusId = 7) THEN 8
   ELSE STATUS
   END
  FROM #tmpResult R 
  LEFT JOIN Homa.TblJob J ON R.RequestId = J.RequestId
  LEFT JOIN TblFogheToziDisconnect FTD ON R.FogheToziDisconnectId = FTD.FogheToziDisconnectId
  LEFT JOIN TblMPRequest MP ON R.MPRequestId = MP.MPRequestId

  SELECT STATUS,
  	IsTamir,
		DisconnectDate,
		DisconnectTime,
    ConnectDate,
    ConnectTime,
    DebtPrice
  FROM  #tmpResult

  DROP TABLE #tmpResult
GO
/*-----------------------------------------------------------------------------*/