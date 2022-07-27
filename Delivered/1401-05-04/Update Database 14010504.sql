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
		CASE 
			WHEN COUNT(MPFLD.MPFeederLimitDayId) > 0
				THEN CAST(0 AS BIT)
			ELSE CAST(1 AS BIT)
			END AS IsOk,
    COUNT(MPFLD.MPFeederLimitDayId) AS FaultyCount
	FROM 
    Emergency.Tbl_MPFeederTemplate MPT  
  	INNER JOIN #tmp ON MPT.MPFeederTemplateId = #tmp.MPFeederTemplateId
  	LEFT JOIN Emergency.Tbl_MPFeederLimitType MPFL ON MPT.MPFeederLimitTypeId = MPFL.MPFeederLimitTypeId
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
	GROUP BY MPT.MPFeederTemplateId
		,MPT.MPFeederId

	DROP TABLE #tmp
END
GO


/*------------------------SP-------------Tavanir-----------------------------*/
CREATE PROCEDURE dbo.spTavanir_GetMPFeedersDisPower
   @aFromDate AS VARCHAR(10)
   ,@aFromTime AS VARCHAR(5)
   ,@aToDate AS VARCHAR(10)
   ,@aToTime AS VARCHAR(5)
AS
  BEGIN
    DECLARE @lFromDate AS DATETIME = dbo.ShamsiDateTimeToMiladi(@aFromDate , @aFromTime)
           ,@lToDate AS DATETIME = dbo.ShamsiDateTimeToMiladi(@aToDate , @aToTime)

    SELECT CAST(1 AS Bit) AS AllAreas 
          ,COUNT(MPR.MPFeederId) AS DisconnectFeederCount
    INTO #tmpDisFeederCount
    FROM TblRequest R
      INNER JOIN TblMPRequest MPR ON R.MPRequestId = MPR.MPRequestId
      WHERE MPR.EndJobStateId IN (4,5)
        AND R.IsDisconnectMPFeeder = 1
    
    
    SELECT 
         CAST(1 AS Bit) AS AllAreas
        ,ISNULL(ROUND(AVG(ISNULL(V.CurrentValue, 0)) , 3), 0) AS CurrentValueAvg
        ,ISNULL(AVG(ABS(ISNULL(R.DisconnectInterval ,0))), 0) AS DisconnectIntervalAvg
        ,ISNULL(ROUND(SUM(ISNULL(V.DisconnectPower, 0)) , 3),0) AS DisconnectPowerSum
    INTO #tmpMonitoring
    FROM ViewMonitoring V
      INNER JOIN TblRequest R ON V.RequestId = R.RequestId
    WHERE R.DisconnectDT BETWEEN @lFromDate AND @lToDate
      AND R.IsDisconnectMPFeeder = 1
    
    
    SELECT M.CurrentValueAvg
         ,CAST(M.DisconnectIntervalAvg / 60 AS varchar(5)) + ':' + CAST(M.DisconnectIntervalAvg % 60 AS varchar(2)) 
              AS DisconnectIntervalAvg
         , M.DisconnectPowerSum 
         , C.DisconnectFeederCount
    FROM #tmpMonitoring M
      INNER JOIN #tmpDisFeederCount C ON M.AllAreas = C.AllAreas
    
    
    DROP TABLE #tmpMonitoring
    DROP TABLE #tmpDisFeederCount

  END
GO