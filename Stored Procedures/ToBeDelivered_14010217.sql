
CREATE FUNCTION dbo.ShamsiDateTimeToMiladi (@aShDate AS VARCHAR(10) , @aShTime AS VARCHAR(5))
RETURNS DATETIME AS
BEGIN
    DECLARE @lDateTime AS DATETIME,
            @lHour AS INT,
            @lMinute AS INT

   SET @lDateTime = dbo.shtom(@aShDate)
   SET @lHour = CAST(SUBSTRING( @aShTime, 1 ,2) AS INT)
   SET @lMinute = CAST(SUBSTRING( @aShTime, 4 ,2) AS INT)
   SET @lDateTime = DATEADD(HOUR, @lHour , @lDateTime)
   SET @lDateTime = DATEADD(MINUTE, @lMinute , @lDateTime)
 
 RETURN @lDateTime
END


/*---------------------------------------------------------------------------------------------*/

CREATE PROC Emergency.spGetFeederGroupPlanRaw @aGroupMPFeederId AS BIGINT
AS
BEGIN
	SELECT MPT.MPFeederTemplateId
		,MPF.MPFeederId
    ,MPP.MPPostName
		,MPF.MPFeederName
		,COUNT(R.MPRequestId) AS MPFeederDisconnectCount
		,MPF.Voltage
	INTO #tmp
	  FROM Emergency.Tbl_MPFeederTemplate MPT
	INNER JOIN Tbl_MPFeeder MPF ON MPT.MPFeederId = MPF.MPFeederId
  INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
	LEFT JOIN TblMPRequest MP ON MPF.MPFeederId = MP.MPFeederId
	LEFT JOIN TblRequest R ON MP.MPRequestId = R.MPRequestId
		AND DATEDIFF(DAY, R.DisconnectDT, GETDATE()) <= 20
	WHERE ISNULL(R.IsDisconnectMPFeeder, 1) = 1
		AND MPT.GroupMPFeederId = @aGroupMPFeederId
	GROUP BY MPT.MPFeederTemplateId
		,MPF.MPFeederId
		,MPF.MPFeederName
		,MPF.Voltage
    ,MPP.MPPostName


	SELECT T.MPFeederTemplateId
		,T.MPFeederId
    ,T.MPPostName
		,T.MPFeederName
		,T.MPFeederDisconnectCount
		,ISNULL(ROUND(3 * T.Voltage * C.CurrentValue * C.CosinPhi / 1000000, 2),0) AS CurrentValueMW
		,ISNULL(C.CurrentValue, 0) AS CurrentValue
    ,CAST(0 AS BIT) AS IsSelected
  	FROM #tmp T
    LEFT JOIN (
  		SELECT L.MPFeederId
  			,H.CurrentValue
  			,H.CosinPhi
  			,H.HourId
  			,row_number() OVER (
  				PARTITION BY L.MPFeederId ORDER BY L.RelDate
  					,H.HourId DESC
  				) AS RowNum
  		FROM Tbl_MPFeederLoad L
  		INNER JOIN Tbl_MPFeederLoadHours H ON L.MPFeederLoadId = H.MPFeederLoadId
  		) C ON C.MPFeederId = T.MPFeederId
	WHERE ISNULL(C.RowNum , 1) = 1

	DROP TABLE #tmp
END


/*---------------------------------------------------------------------------------------------*/

CREATE PROC Emergency.spGetFeederGroupTiming
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
        	,E.TimingStateId
        	,E.TimingState
        	,G.GroupMPFeederName
        	,T.ForecastDisconnectDatePersian
        	,T.ForecastDisconnectTime
        	,T.ForecastConnectDatePersian
        	,T.ForecastConnectTime
        	,T.ForecastMW
      FROM Emergency.TblTiming T
      INNER JOIN Emergency.Tbl_TimingState E ON T.TimingStateId = E.TimingStateId
      INNER JOIN Emergency.Tbl_GroupMPFeeder G ON T.GroupMPFeederId = G.GroupMPFeederId
      INNER JOIN #tmp ON T.GroupMPFeederId = #tmp.GroupMPFeederId
      WHERE (LEN(@aFromDate) * LEN(@aFromTime) = 0 OR T.ForecastDisconnectDT >= @lFromDT)
      	AND (LEN(@aToDate) * LEN(@aToTime) = 0 OR T.ForecastDisconnectDT <= @lToDT)
    
    DROP TABLE #tmp
  END

/*---------------------------------------------------------------------------------------------*/

CREATE PROC Emergency.spGetMPFeederTiming @aTiminigId AS INTEGER
AS
BEGIN
	SELECT TMPF.TimingMPFeederId
		,TMPF.MPFeederTemplateId
		,TMPF.MPFeederId
		,MPP.MPPostName
		,MPF.MPFeederName
		,TMPF.DisconnectDatePersian
		,TMPF.DisconnectTime
		,TMPF.ConnectDatePersian
		,TMPF.ConnectTime
		,TMPF.ForecastCurrentValue
    ,TMPF.IsDisconnectMPFeeder
	INTO #tmp
	FROM Emergency.TblTimingMPFeeder TMPF
  	INNER JOIN Tbl_MPFeeder MPF ON TMPF.MPFeederId = MPF.MPFeederId
  	INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
  WHERE TMPF.TimingId = @aTiminigId

	SELECT T.* , CAST(0 AS FLOAT) AS PreCurrentValue
		,CASE WHEN T.IsDisconnectMPFeeder = 1 THEN ISNULL(LOAD.CurrentValue, 0) ELSE CAST(0 AS FLOAT) END AS CurrentValue
    , CAST(0 AS BIT) AS NotDone
	FROM #tmp T
	LEFT JOIN (
		SELECT L.MPFeederId
			,H.CurrentValue
			,row_number() OVER (
				PARTITION BY L.MPFeederId ORDER BY L.RelDate
					,H.HourId DESC
				) AS RowNum
		FROM Tbl_MPFeederLoad L
		INNER JOIN Tbl_MPFeederLoadHours H ON L.MPFeederLoadId = H.MPFeederLoadId
		) LOAD ON T.MPFeederId = LOAD.MPFeederId
	WHERE ISNULL(LOAD.RowNum, 1) = 1

	DROP TABLE #tmp
END


/*---------------------------------------------------------------------------------------------*/


CREATE PROC Emergency.spGetFeederGroupPlan @aGroupMPFeederId AS BIGINT
	,@aTimingId AS INT = - 1
AS
BEGIN
	CREATE TABLE #tmpResult (
		MPFeederTemplateId INT
		,MPFeederId INT
		,MPPostName NVARCHAR(30)
		,MPFeederName NVARCHAR(50)
		,MPFeederDisconnectCount INT
		,CurrentValue FLOAT
		,CurrentValueMW FLOAT
		,IsSelected BIT
		)

	INSERT INTO #tmpResult
	EXEC Emergency.spGetFeederGroupPlanRaw @aGroupMPFeederId

	UPDATE #tmpResult
  	SET IsSelected = CASE 
  			WHEN TMPF.TimingMPFeederId IS NOT NULL
  				THEN CAST(1 AS BIT)
  			ELSE CAST(0 AS BIT)
  			END
  FROM #tmpResult
  	INNER JOIN Emergency.Tbl_MPFeederTemplate MPFT ON #tmpResult.MPFeederTemplateId = MPFT.MPFeederTemplateId
  	LEFT JOIN Emergency.TblTiming T ON MPFT.GroupMPFeederId = T.GroupMPFeederId
  	LEFT JOIN Emergency.TblTimingMPFeeder TMPF ON T.TimingId = TMPF.TimingId
  		AND TMPF.MPFeederTemplateId = MPFT.MPFeederTemplateId
    WHERE ISNULL(T.TimingId , -1) = @aTimingId

	SELECT * FROM #tmpResult

	DROP TABLE #tmpResult
END

