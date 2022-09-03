create PROCEDURE [dbo].[spGetLPFeeders_Multiple] @aAreaIds varchar(4000), @aLPPostIds varchar(4000), @IsAll bit
AS 
	SELECT * INTO #AreaIds_tmp FROM dbo.Split(@aAreaIds , ',')
	SELECT * INTO #LPPostIds_tmp FROM dbo.Split(@aLPPostIds , ',')
	
	SELECT * 
	FROM Tbl_LPFeeder 
	INNER JOIN #AreaIds_tmp Area_t ON Tbl_LPFeeder.AreaId = CAST(Area_t.Item AS INT)
	INNER JOIN #LPPostIds_tmp LPPost_t ON Tbl_LPFeeder.LPPostId = CAST(LPPost_t.Item AS INT)
	WHERE 
		(
			(AreaId In (Area_t.Item)) 
			OR (LPFeederId IN 
				(SELECT Tbl_LPCommonFeeder.LPFeederId 
				FROM Tbl_LPCommonFeeder INNER JOIN Tbl_LPFeeder ON Tbl_LPCommonFeeder.LPFeederId = Tbl_LPFeeder.LPFeederId 
				WHERE Tbl_LPCommonFeeder.AreaId In (Area_t.Item) and (@IsAll=1 or IsActive=1))
				) 
		) 
		AND (LPPostId In (LPPost_t.Item) OR LPPost_t.Item='')
		AND (@IsAll=1 or IsActive=1)
		
		DROP TABLE #AreaIds_tmp
		DROP TABLE #LPPostIds_tmp