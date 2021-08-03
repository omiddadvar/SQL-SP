USE CCRequesterSetad
GO
ALTER PROCEDURE Homa.spTraceArea
  @StartDate AS VARCHAR(10),
  @EndDate AS VARCHAR(10),
  @StartTime AS VARCHAR(8),
  @EndTime AS VARCHAR(8),
  @AreaId AS int = 0,
  @TabletId AS BIGINT = -1
  AS 
  BEGIN
    /*Calculation on time: */
  	IF len(@StartTime) = 5
		  SET @StartTime = @StartTime + ':00'
  	IF len(@EndTime) = 5
  		SET @EndTime = @EndTime + ':59'
  	DECLARE @StartDT AS Datetime
  	DECLARE @EndDT AS Datetime
  	DECLARE @StartStr AS VARCHAR(13)
  	DECLARE @EndStr AS VARCHAR(13)
  	IF @StartDate = ''
  	BEGIN 
  		SET @StartDT = DATEADD(YEAR,-20,GETDATE())
  		SET @StartStr = '1300/01/01 00'
  	end
  	ELSE
  	BEGIN 
  		SET @StartDT = dbo.shtom(@StartDate)
  		SET @StartStr = @StartDate + ' 00'
  		IF @StartTime <> ''
  		BEGIN 
  			SET @StartDT = DATEADD(HOUR,CAST(LEFT(@StartTime,2) AS int),@StartDT)
  			SET @StartDT = DATEADD(MINUTE,CAST(SUBSTRING(@StartTime,4,2)AS int),@StartDT)
  			SET @StartStr = @StartDate + ' ' + LEFT(@StartTime,2)
  		END
  	END
  IF @EndDate = ''
	BEGIN 
		SET @EndDT = DATEADD(YEAR, + 20,GETDATE())
		SET @EndStr = '1500/01/01 00'
	END
	ELSE
	BEGIN 
		SET @EndDT = dbo.shtom(@EndDate)
		SET @EndStr = @EndDate + ' 23'
		IF @EndTime <> ''
		BEGIN 
			SET @EndDT = DATEADD(HOUR,CAST(LEFT(@EndTime,2) AS int),@EndDT)
			SET @EndDT = DATEADD(MINUTE,CAST(SUBSTRING(@EndTime,4,2)AS int),@EndDT)
			SET @EndDT = DATEADD(SECOND,59,@EndDT)
			SET @EndStr = @EndDate + ' ' + LEFT(@EndTime,2)
		END
	END
  /*Gathering Data: */
	SELECT
		  [TblOnCall].[OnCallId],[TblTraceSummary].AreaId,TargetDatePersian + 
		  case when TargetTime< = 9 THEN ' 0' 
		  ELSE
		  ' ' END + CAST(TargetTime AS VARCHAR(10))
		  AS TargetDT,
		  [TblTraceSummary].TraceLen		  
		  INTO #tmpOnCall1
		  FROM [Homa].[TblTraceSummary]
		  INNER JOIN [CCRequesterSetad].[Homa].[TblOnCall]
		  ON [TblTraceSummary].OnCallId = [TblOnCall].OnCallId
		  WHERE 
		  (TargetDatePersian> = @StartDate or @StartDate = '')and
		  (TargetDatePersian< = @EndDate or @EndDate = '')and
		  (AreaId = @AreaId or @AreaId< = 0)
  		  
  	IF @StartTime <> '' 
  		DELETE FROM #tmpOnCall1 WHERE TargetDT < @StartStr
  	IF @EndTime <> '' 
  		DELETE FROM #tmpOnCall1 WHERE TargetDT > @EndStr
		SELECT #tmpOnCall1.OnCallId,#tmpOnCall1.AreaId,SUM(#tmpOnCall1.TraceLen) AS TraceLen INTO #tmpOnCall FROM #tmpOnCall1 
	    GROUP BY #tmpOnCall1.OnCallId,#tmpOnCall1.AreaId
    DROP TABLE #tmpOnCall1

    /*Area Data :*/
    DECLARE @lSQL VARCHAR(2000) = N'SELECT #tmpOnCall.AreaId , min(Tbl_Area.Area) AS Area 
        , round(sum(#tmpOnCall.TraceLen)/1000,2) AS TraceLen
        ,CAST(0 AS BIT) AS IsChecked
      FROM #tmpOnCall
      INNER JOIN homa.TblOnCall ON TblOnCall.OnCallId = #tmpOnCall.OnCallId
      INNER JOIN Tbl_Area ON Tbl_Area.AreaId = #tmpOnCall.AreaId'
    IF ISNULL(@TabletId , 0) > 0 BEGIN  
      SET @lSQL = @lSQL +' WHERE TblOnCall.TabletId = ' + CAST(@TabletId AS VARCHAR(30)) 
    END
    SET @lSQL = @lSQL +' GROUP BY #tmpOnCall.AreaId'
    /*Final Result*/
    EXEC(@lSQL)

    DROP TABLE #tmpOnCall
  END