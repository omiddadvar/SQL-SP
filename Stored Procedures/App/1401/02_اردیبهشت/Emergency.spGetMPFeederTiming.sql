
ALTER PROC Emergency.spGetMPFeederTiming @aTiminigId AS INTEGER
AS
BEGIN
	SELECT TMPF.TimingMPFeederId
		,TMPF.MPFeederTemplateId
		,TMPF.MPFeederId
		,MPP.MPPostName
		,MPF.MPFeederName
    ,R.RequestNumber
		,TMPF.DisconnectDatePersian
		,TMPF.DisconnectTime
		,TMPF.ConnectDatePersian
		,TMPF.ConnectTime
		,TMPF.ForecastCurrentValue
    ,TMPF.IsDisconnectMPFeeder
    ,TMPF.PreCurrentValue
    ,TMPF.CurrentValue
    ,TMPF.TimingStateId
	INTO #tmp
	FROM Emergency.TblTimingMPFeeder TMPF
  	INNER JOIN Tbl_MPFeeder MPF ON TMPF.MPFeederId = MPF.MPFeederId
  	INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
    LEFT JOIN TblRequest R ON TMPF.RequestId = R.RequestId
  WHERE TMPF.TimingId = @aTiminigId

	SELECT T.TimingMPFeederId
		,T.MPFeederTemplateId
		,T.MPFeederId
    ,T.RequestNumber
		,T.MPPostName
		,T.MPFeederName
		,T.DisconnectDatePersian
		,T.DisconnectTime
		,T.ConnectDatePersian
		,T.ConnectTime
		,T.ForecastCurrentValue
    ,T.IsDisconnectMPFeeder
    ,ISNULL(T.PreCurrentValue , 0) AS PreCurrentValue
		,CASE WHEN T.TimingStateId = 5 THEN 0 ELSE 
          ISNULL(T.CurrentValue,
                CASE WHEN T.IsDisconnectMPFeeder = 1 THEN ISNULL(LOAD.CurrentValue, 0) ELSE CAST(0 AS FLOAT) END) 
      END AS CurrentValue
    ,CASE WHEN T.TimingStateId = 5 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS IsCanceled
    ,CASE WHEN T.DisconnectDatePersian IS NULL  THEN 0 WHEN T.ConnectDatePersian IS NULL THEN 1 ELSE 2 END AS ChangeStateId
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


--  EXEC Emergency.spGetMPFeederTiming @aTiminigId = 990188926
/*

UPDATE Emergency.TblTimingMPFeeder SET ConnectDT = NULL,
  ConnectDatePersian = NULL , ConnectTime = NULL
  WHERE TimingMPFeederId IN (990188927,990188939,990188940)


*/