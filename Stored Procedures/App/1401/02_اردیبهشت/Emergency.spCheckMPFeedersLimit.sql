ALTER PROC Emergency.spCheckMPFeedersLimit @aStartDate AS VARCHAR(10),
	@aStartTime AS VARCHAR(5),
	@aEndDate AS VARCHAR(10),
	@aEndTime AS VARCHAR(5),
	@aMPFeederTemplateIDs AS VARCHAR(5000) = ''
AS
BEGIN
	DECLARE @lStartDT AS DATETIME = dbo.ShamsiDateTimeToMiladi(@aStartDate, @aStartTime)
	DECLARE @lEndtDT AS DATETIME = dbo.ShamsiDateTimeToMiladi(@aEndDate, @aEndTime)
	DECLARE @lStartDayOfWeekId AS INT = DATEPART(DW, @lStartDT)
	DECLARE @lEndDayOfWeekId AS INT = DATEPART(DW, @lEndtDT)
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
  			(
  				(
  					MPFLD.WeekDayId = @lStartDayOfWeekId
  					OR (
  						MPFLD.IsHoliday = 1
  						AND Holi.HolidayId IS NOT NULL
              AND Holi.HolidayDatePersian = @aStartDate
  						)
  					)
  				AND (
  					(
  						dbo.ShamsiDateTimeToMiladi(@aStartDate, MPFLD.StartTime) BETWEEN @lStartDT
  							AND @lEndtDT
  						)
  					OR (
  						dbo.ShamsiDateTimeToMiladi(@aStartDate, MPFLD.EndTime) BETWEEN @lStartDT
  							AND @lEndtDT
  						)
  					)
  				)
  			OR (
  				(
  					MPFLD.WeekDayId = @lEndDayOfWeekId
  					OR (
  						MPFLD.IsHoliday = 1
  						AND Holi.HolidayId IS NOT NULL
              AND Holi.HolidayDatePersian = @aEndDate
  						)
  					)
  				AND (
  					(
  						dbo.ShamsiDateTimeToMiladi(@aEndDate, MPFLD.StartTime) BETWEEN @lStartDT
  							AND @lEndtDT
  						)
  					OR (
  						dbo.ShamsiDateTimeToMiladi(@aEndDate, MPFLD.EndTime) BETWEEN @lStartDT
  							AND @lEndtDT
  						)
  					)
  				)
  			)
	GROUP BY MPT.MPFeederTemplateId,
		MPT.MPFeederId,
		MPT.MPFeederLimitTypeId

	DROP TABLE #tmp
END
	/*
  
EXEC Emergency.spCheckMPFeedersLimit @aStartDate = '1401/02/18'
                                    ,@aStartTime = '23:00'
                                    ,@aEndDate = '1401/02/19'
                                    ,@aEndTime = '01:00'
                                    ,@aMPFeederTemplateIDs = '990188851,990188854,990188856'
  
*/
