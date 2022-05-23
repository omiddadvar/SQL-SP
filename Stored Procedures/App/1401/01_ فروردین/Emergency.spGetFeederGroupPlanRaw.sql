﻿
ALTER PROC Emergency.spGetFeederGroupPlanRaw @aGroupMPFeederId AS BIGINT
AS
BEGIN
	SELECT MPT.MPFeederTemplateId
		,MPF.MPFeederId
    ,MPP.MPPostName
		,MPF.MPFeederName
		,COUNT(R.MPRequestId) AS MPFeederDisconnectCount
		,MPF.Voltage
    ,A.Area
	INTO #tmp
	  FROM Emergency.Tbl_MPFeederTemplate MPT
	INNER JOIN Tbl_MPFeeder MPF ON MPT.MPFeederId = MPF.MPFeederId
  INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
  INNER JOIN Tbl_Area A ON MPT.AreaId = A.AreaId
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
    ,A.Area


	SELECT T.MPFeederTemplateId
    ,T.Area
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


-- EXEC Emergency.spGetFeederGroupPlan @aGroupMPFeederId = 990188852
