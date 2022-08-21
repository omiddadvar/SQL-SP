
CREATE PROCEDURE spGetOutageStat_LPPosts
  @aFromDate AS VARCHAR(11),
  @aToDate AS VARCHAR(11),
  @aAreaIds AS VARCHAR(2000) = ''
AS
  BEGIN
    /* Create Area Temp Table */
    CREATE TABLE #tmpArea (AreaId INT, Area NVARCHAR(50))
          
    DECLARE @lSQL AS NVARCHAR(MAX) = 'SELECT AreaId , Area FROM Tbl_Area'
    
    IF LEN(@aAreaIds) > 0 
      SET @lSQL = @lSQL + ' WHERE AreaId IN (''' + @aAreaIds + ''')'
    
    INSERT INTO #tmpArea EXEC(@lSQL)
    
    
    /* Select LPPost Outages */
    /* ----MP----MP----MP----MP----MP */
    SELECT A.Area
      ,MPP.MPPostName
      ,MPF.MPFeederName
      ,LPP.LPPostName
      ,NULL AS LPFeederName
      ,MPR.IsTotalLPPostDisconnected AS IsTotalDisconnect
      ,'LV_Post' AS OutageType 
      ,COUNT(*) AS Count 
      ,SUM(ISNULL(R.DisconnectInterval , 0)) AS DisconnectInterval
      ,SUM(ISNULL(R.DisconnectPower, 0.0)) AS DisconnectPower
    INTO #tmpMP
    FROM TblRequest R
      INNER JOIN TblMPRequest MPR ON R.MPRequestId = MPR.MPRequestId
      INNER JOIN Tbl_LPPost LPP ON MPR.LPPostId = LPP.LPPostId
      INNER JOIN Tbl_MPFeeder MPF ON LPP.MPFeederId = MPF.MPFeederId
      INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
      INNER JOIN Tbl_DisconnectGroupSet DGSet ON MPR.DisconnectGroupSetId = DGSet.DisconnectGroupSetId
      INNER JOIN Tbl_DisconnectGroup G ON DGSet.DisconnectGroupId = G.DisconnectGroupId
      INNER JOIN #tmpArea A ON R.AreaId = A.AreaId
    WHERE R.DisconnectDatePersian BETWEEN @aFromDate AND @aToDate
      AND G.DisconnectGroupId IN (1004 , 1005)
    GROUP BY A.Area, MPP.MPPostName ,MPF.MPFeederName, LPP.LPPostId ,LPP.LPPostName ,MPR.IsTotalLPPostDisconnected
    
    /* ----LP----LP----LP----LP----LP */
    SELECT A.Area
      ,MPP.MPPostName
      ,MPF.MPFeederName
      ,LPP.LPPostName
      ,NULL AS LPFeederName
      ,LPR.IsTotalLPPostDisconnected AS IsTotalDisconnect
      ,'LV_Post' AS OutageType 
      ,COUNT(*) AS Count 
      ,SUM(ISNULL(R.DisconnectInterval , 0)) AS DisconnectInterval
      ,SUM(ISNULL(R.DisconnectPower, 0.0)) AS DisconnectPower
    INTO #tmpLP
    FROM TblRequest R
      INNER JOIN TblLPRequest LPR ON R.LPRequestId = LPR.LPRequestId
      INNER JOIN Tbl_LPPost LPP ON LPR.LPPostId = LPP.LPPostId
      INNER JOIN Tbl_MPFeeder MPF ON LPP.MPFeederId = MPF.MPFeederId
      INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
      INNER JOIN #tmpArea A ON R.AreaId = A.AreaId
    WHERE R.DisconnectDatePersian BETWEEN @aFromDate AND @aToDate
      AND LPR.IsTotalLPPostDisconnected = 1
    GROUP BY A.Area, MPP.MPPostName ,MPF.MPFeederName, LPP.LPPostId ,LPP.LPPostName ,LPR.IsTotalLPPostDisconnected
    
    
    SELECT * FROM 
    (
      SELECT * FROM #tmpMP
      UNION
      SELECT * FROM #tmpLP
    ) Res
    ORDER BY Res.Area
    
    DROP TABLE #tmpLP
    DROP TABLE #tmpMP
    DROP TABLE #tmpArea
  END

/*

EXEC spGetOutageStat_LPPosts @aFromDate = '1401/01/01'
                           ,@aToDate = '1401/05/30'
                           ,@aAreaIds = ''

*/