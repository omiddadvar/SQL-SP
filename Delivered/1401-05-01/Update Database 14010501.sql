
/*---------------------------------------------------------------------------*/
/*--------[ALTER]------Emergency.spGetLimitDays------------------------------*/
/*---------------------------------------------------------------------------*/

ALTER PROCEDURE Emergency.spGetLimitDays 
	@aMPFeederLimitTypeId as int = NULL
AS
	IF @aMPFeederLimitTypeId <= 0 SET @aMPFeederLimitTypeId = NULL
	SELECT 
		tLD.*,
		tLT.MPFeederLimitType,
		ISNULL(tWD.WeekDay,CASE WHEN tLD.IsHoliday = 1 THEN N'روزهای تعطیل'ELSE N'تمامي روزهاي هفته'END) AS WeekDay
	FROM 
		Emergency.Tbl_MPFeederLimitDay tLD
		INNER JOIN Emergency.Tbl_MPFeederLimitType tLT ON tLD.MPFeederLimitTypeId = tLT.MPFeederLimitTypeId
		LEFT JOIN Emergency.Tbl_WeekDay tWD ON tLD.WeekDayId = tWD.WeekDayId
	WHERE
		(@aMPFeederLimitTypeId IS NULL OR tLD.MPFeederLimitTypeId = @aMPFeederLimitTypeId)
GO


/*---------------------------------------------------------------------------*/
/*--------[ALTER]------Emergency.fnCheckMPFeedersLimit_SomeDays--------------*/
/*---------------------------------------------------------------------------*/


ALTER FUNCTION Emergency.fnCheckMPFeedersLimit_SomeDays(
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
                      OR
                      (@lStartDT <> @lEndCheckDT AND @lStartDT BETWEEN @lStartCheckDT AND @lEndCheckDT)
                    ) THEN CAST(0 AS BIT)
                  	ELSE CAST(1 AS BIT)
                  END
      RETURN @lState
  END
GO

/*---------------------------------------------------------------------------*/
/*--------[ALTER]------Emergency.fnCheckMPFeedersLimit_AllDays---------------*/
/*---------------------------------------------------------------------------*/

ALTER FUNCTION Emergency.fnCheckMPFeedersLimit_AllDays(
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
                OR
                (@lStartDT <> @lEndCheckDTEnd AND @lStartDT BETWEEN @lStartCheckDTStart AND @lEndCheckDTEnd)
              ) THEN CAST(0 AS BIT)
            	ELSE CAST(1 AS BIT)
            END
      RETURN @lState
  END
GO