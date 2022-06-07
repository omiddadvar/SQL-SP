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
      SET @lState = CASE 
              WHEN (@aIsHoliday = 0 AND @aWeekDayId IS NULL) 
              AND
              (
                 dbo.ShamsiDateTimeToMiladi(@aStartDate, @aLimitStartTime)
                      BETWEEN @lStartDT AND @lEndtDT 
                 OR
                 dbo.ShamsiDateTimeToMiladi(@aStartDate, @aLimitEndTime) 
                      BETWEEN @lStartDT AND @lEndtDT
                 OR
                 dbo.ShamsiDateTimeToMiladi(@aEndDate, @aLimitStartTime) 
                      BETWEEN @lStartDT AND @lEndtDT 
                 OR
                 dbo.ShamsiDateTimeToMiladi(@aEndDate, @aLimitEndTime) 
                      BETWEEN @lStartDT AND @lEndtDT
              ) THEN CAST(0 AS BIT)
            	ELSE CAST(1 AS BIT)
            END
      RETURN @lState
  END