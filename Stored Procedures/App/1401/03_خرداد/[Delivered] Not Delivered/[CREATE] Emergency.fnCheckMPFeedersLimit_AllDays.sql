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
      /*--------------------------------------------------------------------------*/
      DECLARE @lStartCheckDTStart AS DATETIME = dbo.ShamsiDateTimeToMiladi(@aStartDate, @aLimitStartTime)
      DECLARE @lStartCheckDTEnd AS DATETIME = dbo.ShamsiDateTimeToMiladi(@aStartDate, @aLimitEndTime) 
      DECLARE @lEndCheckDTStart AS DATETIME = dbo.ShamsiDateTimeToMiladi(@aEndDate, @aLimitStartTime) 
      DECLARE @lEndCheckDTEnd AS DATETIME = dbo.ShamsiDateTimeToMiladi(@aEndDate, @aLimitEndTime) 
      /*--------------------------------------------------------------------------*/
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