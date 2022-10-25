CREATE PROCEDURE spGetDisconnectDashboard @aFromDate varchar(10),@aToDate varchar(10)
AS
-- با برنامه گذرا 
SELECT AreaId,COUNT(*) CntPlannedTransitory
INTO #tempCntPlannedTransitory
FROM TblRequest
WHERE  DisconnectDatePersian BETWEEN @aFromDate AND @aToDate
	and DisconnectTime <= '05:00' AND IsTamir=1
GROUP BY AreaId


-- با برنامه ماندگار 
SELECT AreaId,COUNT(*) CntPlannedLasting
INTO #tempCntPlannedLasting
FROM TblRequest
WHERE  DisconnectDatePersian BETWEEN @aFromDate AND @aToDate
	 AND DisconnectTime > '05:00' AND IsTamir=1
GROUP BY AreaId


-- بی برنامه گذرا 
SELECT AreaId,COUNT(*) CntNotPlannedTransitory
INTO #tempCntNotPlannedTransitory
FROM TblRequest
WHERE  DisconnectDatePersian BETWEEN @aFromDate AND @aToDate
	   AND DisconnectTime <= '05:00' AND IsTamir=0
GROUP BY AreaId



-- بی برنامه ماندگار 
SELECT AreaId,COUNT(*) CntNotPlannedLasting
INTO #tempCntNotPlannedLasting
FROM TblRequest
WHERE  DisconnectDatePersian BETWEEN @aFromDate AND @aToDate
	 AND DisconnectTime > '05:00' AND IsTamir=0
GROUP BY AreaId



-- با برنامه
SELECT AreaId,COUNT(*) CntPlanned
INTO #tempCntPlanned
FROM TblRequest
WHERE  DisconnectDatePersian BETWEEN @aFromDate AND @aToDate
	 AND DisconnectTime > '05:00'
GROUP BY AreaId


--بی برنامه
SELECT AreaId,COUNT(*) CntNotPlanned
INTO #tempCntNotPlanned
FROM TblRequest
WHERE  DisconnectDatePersian BETWEEN @aFromDate AND @aToDate
	 AND DisconnectTime > '05:00'
GROUP BY AreaId


-- انرژی توزیع نشده 
SELECT AreaId,IsTamir,Sum(DisconnectPower) AS DisconnectPower
INTO #tempDisconnectPower
FROM TblRequest
WHERE  DisconnectDatePersian BETWEEN @aFromDate AND @aToDate
GROUP BY AreaId,IsTamir


-- قطع کامل فیدر 
SELECT AreaId,COUNT(IsDisconnectMPFeeder) AS MPFeederDisconnect
INTO #tempDisconnectMPFeeder
FROM TblRequest
WHERE  DisconnectDatePersian BETWEEN @aFromDate AND @aToDate
GROUP BY AreaId


--قطع انشعاب و قطع پست
SELECT AreaId,COUNT(IsTotalLPPostDisconnected) AS PostDisconnect,COUNT(IsNotDisconnectFeeder) AS Ensheab_Disconnect
INTO #tempDisconnectPostAnd_Ensheab
FROM TblMPRequest
WHERE  DisconnectDatePersian BETWEEN @aFromDate AND @aToDate
GROUP BY AreaId


--مدت زمان خاموشی 
SELECT AreaId,IsTamir,SUM(DisconnectInterval) AS TotalDisconnectInterval
INTO #tempDisconnectTime
FROM TblRequest
WHERE DisconnectDatePersian BETWEEN @aFromDate AND @aToDate
GROUP BY AreaId,IsTamir



----Main Join----
SELECT Area.AreaId,
	Area.Area,
	IsNULL(#tempCntPlannedTransitory.CntPlannedTransitory,0) AS PlannedTransitoryCount,
	IsNULL(#tempCntPlannedLasting.CntPlannedLasting,0) AS PlannedLastingCount,
	IsNULL(#tempCntNotPlannedTransitory.CntNotPlannedTransitory,0) AS NotPlannedTransitoryCount,
	IsNULL(#tempCntNotPlannedLasting.CntNotPlannedLasting,0) AS NotPlannedLastingCount,
	IsNULL(#tempCntPlanned.CntPlanned,0) AS PlannedDisconnectCount,
	IsNULL(#tempCntNotPlanned.CntNotPlanned,0) AS NotPlannedDisconnectCount,
	IsNull(tempPlannedDisconnectPower.DisconnectPower,0) AS PlannedDisconnectPower,
	IsNull(tempNotPlannedDisconnectPower.DisconnectPower,0) AS NotPlannedDisconnectPower,
	IsNULL(#tempDisconnectMPFeeder.MPFeederDisconnect,0) AS MPFeederDisconnectCount,
	IsNULL(#tempDisconnectPostAnd_Ensheab.Ensheab_Disconnect,0) AS Ensheab_DisconnectCount,
	IsNULL(#tempDisconnectPostAnd_Ensheab.PostDisconnect,0) AS PostDisconnectCount,
	ISNULL(tempPlannedDisconnectTime.TotalDisconnectInterval,0) AS PlannedDisconnectTime,
	ISNULL(tempNotPlannedDisconnectTime.TotalDisconnectInterval,0) AS NotPlannedDisconnectTime

FROM Tbl_Area AS Area
 LEFT JOIN #tempCntPlannedTransitory ON Area.AreaId=#tempCntPlannedTransitory.AreaId
 LEFT JOIN #tempCntPlannedLasting ON Area.AreaId=#tempCntPlannedLasting.AreaId
 LEFT JOIN #tempCntNotPlannedTransitory ON Area.AreaId=#tempCntNotPlannedTransitory.AreaId
 LEFT JOIN #tempCntNotPlannedLasting ON Area.AreaId=#tempCntNotPlannedLasting.AreaId
 LEFT JOIN #tempCntPlanned ON Area.AreaId=#tempCntPlanned.AreaId
 LEFT JOIN #tempCntNotPlanned ON Area.AreaId=#tempCntNotPlanned.AreaId
 LEFT JOIN 
		(SELECT AreaId,DisconnectPower FROM #tempDisconnectPower where IsTamir=0) 
				AS tempPlannedDisconnectPower ON Area.AreaId=tempPlannedDisconnectPower.AreaId
				
 LEFT JOIN 
		(SELECT AreaId,DisconnectPower FROM #tempDisconnectPower where IsTamir=1)
				AS tempNotPlannedDisconnectPower ON Area.AreaId=tempNotPlannedDisconnectPower.AreaId
 
 LEFT JOIN #tempDisconnectMPFeeder ON Area.AreaId=#tempDisconnectMPFeeder.AreaId
 LEFT JOIN #tempDisconnectPostAnd_Ensheab ON Area.AreaId=#tempDisconnectPostAnd_Ensheab.AreaId
 LEFT JOIN 
		(SELECT AreaId,TotalDisconnectInterval FROM #tempDisconnectTime where IsTamir=1) 
				AS tempPlannedDisconnectTime ON Area.AreaId=tempPlannedDisconnectTime.AreaId
 LEFT JOIN 
		(SELECT AreaId,TotalDisconnectInterval FROM #tempDisconnectTime where IsTamir=0)
				AS tempNotPlannedDisconnectTime ON Area.AreaId=tempNotPlannedDisconnectTime.AreaId


DROP TABLE #tempCntPlannedTransitory
DROP TABLE #tempCntPlannedLasting
DROP TABLE #tempCntNotPlannedTransitory
DROP TABLE #tempCntNotPlannedLasting
DROP TABLE #tempCntPlanned
DROP TABLE #tempCntNotPlanned
DROP TABLE #tempDisconnectPower
DROP TABLE #tempDisconnectMPFeeder
DROP TABLE #tempDisconnectPostAnd_Ensheab
DROP TABLE #tempDisconnectTime
