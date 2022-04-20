
ALTER PROC Emergency.spGetFeederGroupTiming
    @aGroupMPFeederIds AS VARCHAR(2000),
    @aFromDate AS VARCHAR(10) = '',
    @aFromTime AS VARCHAR(5) = '',
    @aToDate AS VARCHAR(10) = '',
    @aToTime AS VARCHAR(5) = ''
  AS
  BEGIN
    CREATE TABLE #tmp ( GroupMPFeederId INT )
    DECLARE @lFromDT AS DATETIME = NULL,
            @lToDT AS DATETIME = NULL,
            @lSQL AS VARCHAR(2000) = 'SELECT GroupMPFeederId FROM Emergency.Tbl_GroupMPFeeder'
    IF LEN(@aGroupMPFeederIds) > 0 BEGIN  
      SET @lSQL = @lSQL + ' WHERE GroupMPFeederId IN (' + @aGroupMPFeederIds + ')'	
    END
    INSERT INTO #tmp EXEC(@lSQL)

    IF LEN(@aFromDate) * LEN(@aFromTime) > 0 BEGIN  
      SET @lFromDT = dbo.ShamsiDateTimeToMiladi(@aFromDate , @aFromTime)
    END
    IF LEN(@aToDate) * LEN(@aToTime) > 0 BEGIN  
      SET @lToDT = dbo.ShamsiDateTimeToMiladi(@aToDate , @aToTime)
    END

    SELECT T.TimingId
        	,T.GroupMPFeederId
        	,E.EndJobState
        	,G.GroupMPFeederName
        	,T.ForecastDisconnectDatePersian
        	,T.ForecastDisconnectTime
        	,T.ForecastConnectDatePersian
        	,T.ForecastConnectTime
        	,T.ForecastMW
      FROM Emergency.TblTiming T
      INNER JOIN Tbl_EndJobState E ON T.EndJobStateId = E.EndJobStateId
      INNER JOIN Emergency.Tbl_GroupMPFeeder G ON T.GroupMPFeederId = G.GroupMPFeederId
      INNER JOIN #tmp ON T.GroupMPFeederId = #tmp.GroupMPFeederId
      WHERE (LEN(@aFromDate) * LEN(@aFromTime) > 0 OR T.ForecastDisconnectDT >= @lFromDT)
      	AND (LEN(@aToDate) * LEN(@aToTime) > 0 OR T.ForecastDisconnectDT <= @lToDT)
    
    DROP TABLE #tmp
  END

/*

EXEC Emergency.spGetFeederGroupTiming @aGroupMPFeederIds = ''
                                     ,@aFromDate = '1400/01/20'
                                     ,@aFromTime = '24:00'
                                     ,@aToDate = ''
                                     ,@aToTime = '' 

*/