
Create PROCEDURE [dbo].[spGetMPFeeders_Multiple] 
	@aAreaIds varchar(4000), 
	@aMPPostIds varchar(4000), 
	@IsAll bit 
AS 
	SELECT * INTO #AreaIds_tmp FROM dbo.Split(@aAreaIds , ',')
	SELECT * INTO #MPPostIds_tmp FROM dbo.Split(@aMPPostIds , ',')


	SELECT * 
	FROM Tbl_MPFeeder 
	INNER JOIN #AreaIds_tmp Area_t ON Tbl_MPFeeder.AreaId = CAST(Area_t.Item AS INT)
	INNER JOIN #MPPostIds_tmp MPPost_t ON Tbl_MPFeeder.MPPostId = CAST(MPPost_t.Item AS INT)
	WHERE 
		( 
			(AreaId In (Area_t.Item) OR @aAreaIds='') 
			OR ( MPFeederId IN (SELECT MPFeederId FROM Tbl_LPPost WHERE (AreaId In (Area_t.Item)) AND (@IsAll=1 OR IsActive=1)) ) 
			OR ( MPFeederId IN (SELECT Tbl_MPCommonFeeder.MPFeederId FROM Tbl_MPCommonFeeder INNER JOIN Tbl_MPFeeder ON Tbl_MPCommonFeeder.MPFeederId = Tbl_MPFeeder.MPFeederId WHERE (Tbl_MPCommonFeeder.AreaId In (Area_t.Item)) AND (@IsAll=1 OR Tbl_MPFeeder.IsActive=1)) ) 
			OR ( MPFeederId IN (SELECT MPFeederId FROM Tbl_LPPost WHERE
				(LPPostId IN 
					(SELECT LPPostId 
					FROM Tbl_LPFeeder 
					WHERE AreaId In (Area_t.Item) AND (@IsAll=1 OR IsActive=1))
				)
				OR (LPPostId IN 
					(SELECT Tbl_LPFeeder.LPPostId 
					FROM Tbl_LPCommonFeeder INNER JOIN Tbl_LPFeeder ON Tbl_LPCommonFeeder.LPFeederId = Tbl_LPFeeder.LPFeederId 
					WHERE Tbl_LPCommonFeeder.AreaId In (Area_t.Item) AND (@IsAll=1 OR IsActive=1))
				)
			))
		)
		AND (MPPostId In (MPPost_t.Item) OR MPPost_t.Item='') 
		AND (@IsAll=1 OR IsActive=1)
		
		DROP TABLE #AreaIds_tmp
		DROP TABLE #MPPostIds_tmp