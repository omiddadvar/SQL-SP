
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
  , CASE WHEN T.DisconnectDatePersian IS NULL THEN 0 WHEN T.ConnectDatePersian IS NULL THEN 1 ELSE 2 END AS ChangeStateId
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