
DECLARE @aDatePersian AS VARCHAR(10) = '1401/02/18'
       ,@aStartTime AS VARCHAR(5) = '14:30'
       ,@aMPFeederTemplateIDs AS VARCHAR(2000) = '990188851,990188854,990188856'
-------------------------------------------------------------
DECLARE @lStartDT AS DATETIME = dbo.ShamsiDateTimeToMiladi(@aDatePersian , @aStartTime)
DECLARE @lDayOfWeekId AS INT = DATEPART(DW, @lStartDT) 
DECLARE @lSQL AS VARCHAR(2000) = 'SELECT MPFeederTemplateId FROM Emergency.Tbl_MPFeederTemplate'
IF LEN(@aMPFeederTemplateIDs) > 0 BEGIN
  SET @lSQL = @lSQL + '  WHERE MPFeederTemplateId IN('+ @aMPFeederTemplateIDs +')'                            	
END

CREATE TABLE #tmp( MPFeederTemplateId INT )
INSERT INTO #tmp EXEC(@lSQL)


SELECT MPT.MPFeederTemplateId
  ,MPT.MPFeederId
  ,MPT.MPFeederLimitTypeId
  ,CASE WHEN MPFLD.MPFeederLimitDayId IS NULL THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS IsOk
  FROM Emergency.Tbl_MPFeederTemplate MPT
    INNER JOIN #tmp ON MPT.MPFeederTemplateId = #tmp.MPFeederTemplateId
    INNER JOIN Emergency.Tbl_MPFeederLimitType MPFL ON MPT.MPFeederLimitTypeId = MPFL.MPFeederLimitTypeId
    LEFT JOIN BTbl_Holiday Holi ON Holi.HolidayDatePersian = @aDatePersian
    LEFT JOIN Emergency.Tbl_MPFeederLimitDay MPFLD ON 
      MPFL.MPFeederLimitTypeId = MPFLD.MPFeederLimitTypeId
      AND (MPFLD.WeekDayId = @lDayOfWeekId OR (MPFLD.IsHoliday = 1 AND Holi.HolidayId IS NOT NULL))
      AND (@aStartTime BETWEEN MPFLD.StartTime AND MPFLD.EndTime)


DROP TABLE #tmp