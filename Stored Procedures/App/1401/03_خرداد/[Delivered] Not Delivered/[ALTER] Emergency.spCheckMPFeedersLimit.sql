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
