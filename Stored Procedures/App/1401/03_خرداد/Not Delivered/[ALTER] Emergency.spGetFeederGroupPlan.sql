
ALTER PROC Emergency.spGetFeederGroupPlan @aGroupMPFeederId AS BIGINT
  ,@aDayCount AS INT
	,@aTimingId AS INT = - 1
AS
BEGIN
	CREATE TABLE #tmpResult (
		MPFeederTemplateId INT
    ,Area NVARCHAR(50)
		,MPFeederId INT
		,MPPostName NVARCHAR(30)
		,MPFeederName NVARCHAR(50)
		,MPFeederDisconnectCount INT
		,CurrentValue FLOAT
		,CurrentValueMW FLOAT
		,IsSelected BIT
		)

	INSERT INTO #tmpResult
	EXEC Emergency.spGetFeederGroupPlanRaw @aGroupMPFeederId, @aDayCount

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

	SELECT * , CAST(1 AS BIT) AS IsOk FROM #tmpResult 
  ORDER BY MPFeederDisconnectCount DESC

	DROP TABLE #tmpResult
END

--  EXEC Emergency.spGetFeederGroupPlan 990188866,20,990188897

