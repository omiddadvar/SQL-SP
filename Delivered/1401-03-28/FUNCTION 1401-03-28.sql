
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
