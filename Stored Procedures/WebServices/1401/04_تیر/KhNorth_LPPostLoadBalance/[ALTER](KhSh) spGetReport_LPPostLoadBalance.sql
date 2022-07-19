
ALTER PROCEDURE dbo.spGetReport_LPPostLoadBalance 
	@aLPPostCode AS NVARCHAR(100),
	@aFrom AS VARCHAR(10),
	@aTo AS VARCHAR(10)
AS
SELECT Area.Area 
	,MPost.MPPostName 
	,MFeeder.MPFeederName 
	,Post.LPPostName 
	,ISNULL(Post.Address , '') AS Address
	,Post.LPPostCode 
	,ISNULL(Post.LocalCode , '') AS LocalCode
	,dbo.KHSH_GetPercentANDLegalCurrent(Temp.K , Temp_Area.K, Alt.DecreaseFactor ,
		 Alt_Area.DecreaseFactor, PL.PostCapacity , NULL) AS Nominal
	,PL.PostCapacity
	,PL.RCurrent 
	,PL.SCurrent
	,PL.TCurrent
	,PL.NolCurrent
	,PL.PostPeakCurrent AS Average
	,ISNULL(PL.EarthValue, 0) AS EarthValue
	,PostType.LPPostType
	,Post.LPFeederCount
	,CASE WHEN PL.IsTakFaze = 0 THEN dbo.KHSH_UnbalancedIndicatorA (PL.RCurrent, PL.SCurrent , PL.TCurrent , PL.NolCurrent)
		 ELSE 0 END AS IndicatorA
	,CASE WHEN PL.IsTakFaze = 0 THEN dbo.KHSH_UnbalancedIndicatorB (PL.NolCurrent , PL.PostCapacity)
		 ELSE 0 END AS IndicatorB
	,PL.LoadDateTimePersian AS LoadDate
	,PL.LoadTime
	,dbo.KHSH_GetPercentANDLegalCurrent(Temp.K , Temp_Area.K, Alt.DecreaseFactor ,
		 Alt_Area.DecreaseFactor, PL.PostCapacity , PL.PostPeakCurrent) AS CurrentPercent
  ,Own.OwnershipId
  ,Own.Ownership
	FROM TblLPPostLoad PL
	INNER JOIN Tbl_LPPost Post ON Post.LPPostId = PL.LPPostId
	INNER JOIN Tbl_LPPostType PostType ON PostType.LPPostTypeId = Post.LPPostTypeId
	INNER JOIN Tbl_Area Area ON Area.AreaId = Post.AreaId
	INNER JOIN Tbl_MPFeeder MFeeder ON MFeeder.MPFeederId = POst.MPFeederId
	INNER JOIN Tbl_MPPost MPost ON MPost.MPPostId = MFeeder.MPPostId
	LEFT JOIN Tbl_Temperature Temp ON Temp.TemperatureId = Post.TemperatureId
	LEFT JOIN Tbl_Temperature Temp_Area ON Temp_Area.TemperatureId = Area.TemperatureId
	LEFT JOIN Tbl_Altitude Alt ON alt.AltitudeId = Post.AltitudeId
	LEFT JOIN Tbl_Altitude Alt_Area ON Alt_Area.AltitudeId = Post.AltitudeId
  LEFT JOIN Tbl_Ownership Own ON Post.OwnershipId = Own.OwnershipId
	WHERE 
		(Post.LPPostCode = @aLPPostCode OR @aLPPostCode = '')
		AND PL.LoadDateTimePersian BETWEEN @aFrom AND @aTo
	ORDER BY PL.LoadDateTimePersian DESC
GO