--IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spGetLPPostLoadTaadol]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
--  DROP PROCEDURE [dbo].[spGetLPPostLoadTaadol]
--GO

--CREATE PROCEDURE [dbo].[spGetLPPostLoadTaadol] 
--	@aLPPostCode AS NVARCHAR(100),
--	@aFrom AS VARCHAR(10),
--	@aTo AS VARCHAR(10)
--AS
--	SELECT PL.* , Post.LPPostName , Area.Area, MFeeder.MPFeederName , MPost.MPPostName
--		FROM TblLPPostLoad PL 
--		INNER JOIN Tbl_LPPost Post ON Post.LPPostId = PL.LPPostId
--		INNER JOIN Tbl_Area Area ON Area.AreaId = Post.AreaId
--		INNER JOIN Tbl_MPFeeder MFeeder ON MFeeder.MPFeederId = POst.MPFeederId
--		INNER JOIN Tbl_MPPost MPost ON MPost.MPPostId = MFeeder.MPPostId
		
--GO
USE CcRequesterSetad
GO
Declare @aLPPostCode AS NVARCHAR(100) = '11-0109hg'
Declare @aFrom AS VARCHAR(10) = '1387/01/01'
Declare @aTo AS VARCHAR(10) = '1393/01/01'

SELECT Area.Area 
	,MPost.MPPostName 
	,MFeeder.MPFeederName 
	,Post.LPPostName 
	,ISNULL(Post.Address , '') AS Address
	,Post.LPPostCode 
	,ISNULL(Post.LocalCode , '') AS LocalCode
	,dbo.GetPercentANDLegalCurrent(Temp.K , Temp_Area.K, Alt.DecreaseFactor ,
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
	,dbo.UnbalancedIndicatorA (PL.RCurrent, PL.SCurrent , PL.TCurrent , PL.NolCurrent) AS IndicatorA
	,dbo.UnbalancedIndicatorB (PL.NolCurrent , PL.PostCapacity) AS IndicatorB
	,PL.LoadDateTimePersian
	,PL.LoadTime
	,dbo.GetPercentANDLegalCurrent(Temp.K , Temp_Area.K, Alt.DecreaseFactor ,
		 Alt_Area.DecreaseFactor, PL.PostCapacity , PL.PostPeakCurrent) AS CurrentPercent
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
	WHERE Post.LPPostCode = @aLPPostCode 
		AND PL.LoadDateTimePersian BETWEEN @aFrom AND @aTo
