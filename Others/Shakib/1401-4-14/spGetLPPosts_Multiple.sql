create PROCEDURE [dbo].[spGetLPPosts_Multiple] @aAreaIds nvarchar(4000), @aMPFeederIds nvarchar(4000), @IsAll bit
AS 
	SELECT * INTO #AreaIds_tmp FROM dbo.Split(@aAreaIds , ',')
	SELECT * INTO #MPFeederIds_temp FROM dbo.Split(@aMPFeederIds , ',')
	
	SELECT *, LPPostName + ' - ' + LPPostCode as LPPostNameCode
	FROM Tbl_LPPost 
	INNER JOIN #AreaIds_tmp Area_t ON Tbl_LPPost.AreaId = CAST(Area_t.Item AS INT)
	INNER JOIN #MPFeederIds_temp MPFeeder_t ON Tbl_LPPost.MPFeederId = CAST(MPFeeder_t.Item AS INT)
	
	WHERE 
		(
			(AreaId In (Area_t.Item)) 
			OR (LPPostId IN 
				(SELECT LPPostId 
				FROM Tbl_LPFeeder 
				WHERE AreaId In (Area_t.Item) and (@IsAll=1 or IsActive=1))
				) 
			OR (LPPostId IN 
				(SELECT Tbl_LPFeeder.LPPostId 
				FROM Tbl_LPCommonFeeder INNER JOIN Tbl_LPFeeder ON Tbl_LPCommonFeeder.LPFeederId = Tbl_LPFeeder.LPFeederId 
				WHERE Tbl_LPCommonFeeder.AreaId In (Area_t.Item) and (@IsAll=1 or IsActive=1))
				) 
		) 
		AND (MPFeederId In (MPFeeder_t.Item) OR MPFeeder_t.Item='')
		AND (@IsAll=1 or IsActive=1)
	ORDER BY 
		LPPostName
		
		DROP TABLE #AreaIds_tmp
		DROP TABLE #MPFeederIds_temp