
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

/*-----------------------------------------------------------------------------*/
/*--------------------------------FUNCTIONs------------------------------------*/
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

/*-----------------------------------------------------------------------------*/