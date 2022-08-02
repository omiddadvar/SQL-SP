ALTER PROCEDURE dbo.spTavanir_GetMPFeedersDisPower @aFromDate AS VARCHAR(10)
	,@aFromTime AS VARCHAR(5)
	,@aToDate AS VARCHAR(10)
	,@aToTime AS VARCHAR(5)
AS
BEGIN
	DECLARE @lFromDate AS DATETIME = dbo.ShamsiDateTimeToMiladi(@aFromDate, @aFromTime)
		,@lToDate AS DATETIME = dbo.ShamsiDateTimeToMiladi(@aToDate, @aToTime)
	DECLARE @lNow AS DATETIME = GETDATE()
		,@lYesterday AS DATETIME = DATEADD(DAY, - 1, GETDATE())

	SELECT CAST(1 AS BIT) AS AllAreas
		,CAST(ISNULL(Round(SUM(CASE 
				WHEN R.IsDisconnectMPFeeder = 1
					THEN 1
				WHEN R.IsDisconnectMPFeeder = 0 AND MPR.IsNotDisconnectFeeder = 1
					THEN CASE 
							WHEN ISNULL((MPR.CurrentValue / NULLIF(MPR.PreCurrentValue, 0)), 0) > 1
								THEN 1
							ELSE ISNULL((MPR.CurrentValue / NULLIF(MPR.PreCurrentValue, 0)), 0)
							END
				ELSE 0
				END) , 0),0) AS INT) AS DisconnectFeederCount
	--COUNT(DISTINCT MPR.MPFeederId) AS DisconnectFeederCount
	INTO #tmpDisFeederCount
	FROM TblRequest R
	INNER JOIN TblMPRequest MPR ON R.MPRequestId = MPR.MPRequestId
	WHERE (R.IsDisconnectMPFeeder = 1 OR MPR.IsNotDisconnectFeeder = 1)
		AND R.DisconnectDT BETWEEN @lFromDate AND @lToDate

	SELECT CAST(1 AS BIT) AS AllAreas
		--,COUNT(DISTINCT LiveMPR.MPFeederId) AS LiveDisconnectFeederCount
		,CAST(ISNULL(Round(SUM(CASE 
				WHEN R.IsDisconnectMPFeeder = 1
					THEN 1
				WHEN R.IsDisconnectMPFeeder = 0 AND MPR.IsNotDisconnectFeeder = 1
					THEN CASE 
							WHEN ISNULL((MPR.CurrentValue / NULLIF(MPR.PreCurrentValue, 0)), 0) > 1
								THEN 1
							ELSE ISNULL((MPR.CurrentValue / NULLIF(MPR.PreCurrentValue, 0)), 0)
							END
				ELSE 0
				END) ,0),0) AS INT) AS LiveDisconnectFeederCount
	INTO #tmpLiveDisFeederCount
	FROM TblRequest R
	INNER JOIN TblMPRequest MPR ON R.MPRequestId = MPR.MPRequestId
	WHERE (R.IsDisconnectMPFeeder = 1 OR MPR.IsNotDisconnectFeeder = 1)
		AND MPR.EndJobStateId IN (4,5)
		AND MPR.EndJobStateId = R.EndJobStateId
		AND R.DisconnectDT BETWEEN @lYesterday AND @lNow

	SELECT CAST(1 AS BIT) AS AllAreas
		,ISNULL(ROUND(AVG(ISNULL(V.CurrentValue, 0)), 3), 0) AS CurrentValueAvg
		,ISNULL(AVG(ABS(ISNULL(R.DisconnectInterval, 0))), 0) AS DisconnectIntervalAvg
		,ISNULL(ROUND(SUM(ISNULL(V.DisconnectPower, 0)), 3), 0) AS DisconnectPowerSum
	INTO #tmpMonitoring
	FROM ViewMonitoring V
	INNER JOIN TblRequest R ON V.RequestId = R.RequestId
	INNER JOIN TblMPRequest MPR ON R.MPRequestId = MPR.MPRequestId
	WHERE R.DisconnectDT BETWEEN @lFromDate AND @lToDate
		AND (R.IsDisconnectMPFeeder = 1 OR MPR.IsNotDisconnectFeeder = 1)

	SELECT M.CurrentValueAvg
		,CAST(M.DisconnectIntervalAvg / 60 AS VARCHAR(5)) + ':' + CAST(M.DisconnectIntervalAvg % 60 AS VARCHAR(2)) AS DisconnectIntervalAvg
		,M.DisconnectPowerSum
		,C.DisconnectFeederCount
		,L.LiveDisconnectFeederCount
	FROM #tmpMonitoring M
	INNER JOIN #tmpDisFeederCount C ON M.AllAreas = C.AllAreas
	INNER JOIN #tmpLiveDisFeederCount L ON M.AllAreas = C.AllAreas

	DROP TABLE #tmpMonitoring
	DROP TABLE #tmpDisFeederCount
	DROP TABLE #tmpLiveDisFeederCount
END
