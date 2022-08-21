CREATE PROCEDURE spGetOutageStat_MPFeeders 
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
    
    
    /* Select MPFeeder Outages */
    /* ----FT----FT----FT----FT----FT */
    SELECT A.Area
      ,MPP.MPPostName
      ,MPF.MPFeederName
      ,NULL AS LPPostName
      ,NULL AS LPFeederName
      ,CAST(1 AS BIT) AS IsTotalDisconnect
      ,'MV_Feeder' AS OutageType 
      ,COUNT(*) AS Count 
      ,SUM(ISNULL(R.DisconnectInterval , 0)) AS DisconnectInterval
      ,SUM(ISNULL(R.DisconnectPower, 0.0)) AS DisconnectPower
    INTO #tmpFT
    FROM TblRequest R
      INNER JOIN TblFogheToziDisconnect F ON R.FogheToziDisconnectId = F.FogheToziDisconnectId
      INNER JOIN TblFogheToziDisconnectMPFeeder FTMP ON F.FogheToziDisconnectId = FTMP.FogheToziDisconnectId
      INNER JOIN Tbl_MPFeeder MPF ON FTMP.MPFeederId = MPF.MPFeederId
      INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
      INNER JOIN #tmpArea A ON R.AreaId = A.AreaId
    WHERE R.DisconnectDatePersian BETWEEN @aFromDate AND @aToDate
    GROUP BY A.Area, MPP.MPPostName ,MPF.MPFeederId , MPF.MPFeederName
      
    /* ----MP----MP----MP----MP----MP */
    SELECT A.Area
      ,MPP.MPPostName
      ,MPF.MPFeederName
      ,NULL AS LPPostName
      ,NULL AS LPFeederName
      ,R.IsDisconnectMPFeeder AS IsTotalDisconnect
      ,'MV_Feeder' AS OutageType 
      ,COUNT(*) AS Count 
      ,SUM(ISNULL(R.DisconnectInterval , 0)) AS DisconnectInterval
      ,SUM(ISNULL(R.DisconnectPower, 0.0)) AS DisconnectPower
    INTO #tmpMP
    FROM TblRequest R
      INNER JOIN TblMPRequest MPR ON R.MPRequestId = MPR.MPRequestId
      INNER JOIN Tbl_DisconnectGroupSet DGSet ON R.DisconnectGroupSetId = DGSet.DisconnectGroupSetId
      INNER JOIN Tbl_DisconnectGroup G ON DGSet.DisconnectGroupId = G.DisconnectGroupId
      INNER JOIN Tbl_MPFeeder MPF ON MPR.MPFeederId = MPF.MPFeederId
      INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
      INNER JOIN #tmpArea A ON R.AreaId = A.AreaId
    WHERE R.DisconnectDatePersian BETWEEN @aFromDate AND @aToDate
      AND G.DisconnectGroupId IN (1002 , 1003)
    GROUP BY A.Area, MPP.MPPostName ,MPF.MPFeederId , MPF.MPFeederName ,R.IsDisconnectMPFeeder
      
    SELECT * FROM 
    (
      SELECT * FROM #tmpFT 
      UNION
      SELECT * FROM #tmpMP
    ) Res
    ORDER BY Res.Area
    
    DROP TABLE #tmpFT
    DROP TABLE #tmpMP
    DROP TABLE #tmpArea
  END

/*

EXEC  spGetOutageStat_MPFeeders @aFromDate = '1401/01/01'
                               ,@aToDate = '1401/05/30'
                               ,@aAreaIds = ''


*/