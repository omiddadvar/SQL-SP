DECLARE @aTiminigMPFeederId AS INTEGER = 1000

  SELECT MPFeederId , MPFeederName , IsPrivate INTO #tmp FROM Tbl_MPFeeder
  
	SELECT T.*
		,CASE WHEN T.IsPrivate = 0 THEN ISNULL(LOAD.CurrentValue, 0) ELSE 0 END AS CurrentValueMW
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