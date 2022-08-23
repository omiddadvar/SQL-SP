ALTER PROCEDURE spGetOutageStat_MPPosts
  @aFromDate AS VARCHAR(11),
  @aToDate AS VARCHAR(11),
  @aAreaIds AS VARCHAR(2000) = ''
AS
  BEGIN
    DECLARE @lSQL AS NVARCHAR(MAX) = '
      SELECT A.Area
        ,MPP.MPPostName
        ,CAST(NULL AS NVARCHAR(50)) AS MPFeederName
        ,CAST(NULL AS NVARCHAR(100)) AS LPPostName
        ,CAST(NULL AS NVARCHAR(50)) AS LPFeederName
        ,F.IsTotalMPPostDisconnected AS IsTotalDisconnect
        ,CAST(''HV_Post'' AS VARCHAR(20)) AS OutageType 
        ,COUNT(*) AS Count 
        ,SUM(ISNULL(R.DisconnectInterval , 0)) AS DisconnectInterval
        ,SUM(ISNULL(R.DisconnectPower, 0.0)) AS DisconnectPower
      FROM TblRequest R
        INNER JOIN TblFogheToziDisconnect F ON R.FogheToziDisconnectId = F.FogheToziDisconnectId
        INNER JOIN Tbl_Area A ON R.AreaId = A.AreaId
        INNER JOIN Tbl_MPPost MPP ON F.MPPostId = MPP.MPPostId
      WHERE F.IsFeederMode = 0
        AND R.DisconnectDatePersian BETWEEN ''' + @aFromDate + ''' AND ''' + @aToDate +'''
      '+ CASE WHEN LEN(@aAreaIds) > 0  THEN ' AND A.AreaId IN (' + @aAreaIds + ')' ELSE '' END + 
      '
        GROUP BY A.AreaId ,A.Area, F.MPPostId ,MPP.MPPostName, F.IsTotalMPPostDisconnected
        ORDER BY A.Area
      '
    PRINT(@lSQL)
    EXEC(@lSQL)
  END


/*


EXEC spGetOutageStat_MPPosts @aFromDate = '1400/01/01'
                            ,@aToDate = '1401/05/30'
                            ,@aAreaIds = ''
    
*/