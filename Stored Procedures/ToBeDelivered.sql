CREATE PROC Emergency.spGetFeederGroupPlan @lGroupMPFeederId AS BIGINT
AS
BEGIN
	SELECT MPT.MPFeederTemplateId
		,MPF.MPFeederId
		,MPF.MPFeederName
		,COUNT(R.MPRequestId) AS MPFeederDisconnectCount
		,MPF.Voltage
	INTO #tmp
	  FROM Emergency.Tbl_MPFeederTemplate MPT
	INNER JOIN Tbl_MPFeeder MPF ON MPT.MPFeederId = MPF.MPFeederId
	LEFT JOIN TblMPRequest MP ON MPF.MPFeederId = MP.MPFeederId
	LEFT JOIN TblRequest R ON MP.MPRequestId = R.MPRequestId
		AND DATEDIFF(DAY, R.DisconnectDT, GETDATE()) <= 20
	WHERE ISNULL(R.IsDisconnectMPFeeder, 1) = 1
		AND MPT.GroupMPFeederId = @lGroupMPFeederId
	GROUP BY MPT.MPFeederTemplateId
		,MPF.MPFeederId
		,MPF.MPFeederName
		,MPF.Voltage



	SELECT T.MPFeederTemplateId
		,T.MPFeederId
		,T.MPFeederName
		,T.MPFeederDisconnectCount
		,ROUND(3 * T.Voltage * C.CurrentValue * C.CosinPhi / 1000000, 2) AS CurrentValueMW
		,C.CurrentValue
  	FROM (
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
  		) C
	INNER JOIN #tmp T ON C.MPFeederId = T.MPFeederId
	WHERE RowNum = 1

	DROP TABLE #tmp
END
/*----------------------------------------------------------------------*/