CREATE PROCEDURE spGetOutageStat_MPPosts
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
    
    
    /* Select MPPost Outages */
    SELECT A.Area
      ,MPP.MPPostName
      ,NULL AS MPFeederName
      ,NULL AS LPPostName
      ,NULL AS LPFeederName
      ,F.IsTotalMPPostDisconnected AS IsTotalDisconnect
      ,'HV_Post' AS OutageType 
      ,COUNT(*) AS Count 
      ,SUM(ISNULL(R.DisconnectInterval , 0)) AS DisconnectInterval
      ,SUM(ISNULL(R.DisconnectPower, 0.0)) AS DisconnectPower
    FROM TblRequest R
      INNER JOIN TblFogheToziDisconnect F ON R.FogheToziDisconnectId = F.FogheToziDisconnectId
      INNER JOIN #tmpArea A ON R.AreaId = A.AreaId
      INNER JOIN Tbl_MPPost MPP ON F.MPPostId = MPP.MPPostId
    WHERE F.IsFeederMode = 0
      AND R.DisconnectDatePersian BETWEEN @aFromDate AND @aToDate
    GROUP BY A.AreaId ,A.Area, F.MPPostId ,MPP.MPPostName, F.IsTotalMPPostDisconnected
    ORDER BY A.Area
    
    DROP TABLE #tmpArea
  END


/*


EXEC spGetOutageStat_MPPosts @aFromDate = '1400/01/01'
                            ,@aToDate = '1401/05/30'
                            ,@aAreaIds = ''
    
*/