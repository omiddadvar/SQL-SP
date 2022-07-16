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
      /*--------------------------------------------------------------------------*/
      DECLARE @lStartCheckDT AS DATETIME = 
                    dbo.ShamsiDateTimeToMiladi(CASE WHEN @aIsStart = 1 THEN @aStartDate ELSE @aEndDate END, @aLimitStartTime) 
      DECLARE @lEndCheckDT AS DATETIME = 
                    dbo.ShamsiDateTimeToMiladi(CASE WHEN @aIsStart = 1 THEN @aStartDate ELSE @aEndDate END, @aLimitEndTime) 
      /*--------------------------------------------------------------------------*/
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
