CREATE PROCEDURE spGetOutageStat_MPFeeders 
  @aFromDate AS VARCHAR(11),
  @aToDate AS VARCHAR(11),
  @aAreaIds AS VARCHAR(2000) = ''
AS
  BEGIN
    DECLARE @lSQL AS NVARCHAR(MAX) = '
       SELECT * FROM (
         SELECT A.Area
          ,MPP.MPPostName
          ,MPF.MPFeederName
          ,CAST(NULL AS NVARCHAR(100)) AS LPPostName
          ,CAST(NULL AS NVARCHAR(50)) AS LPFeederName
          ,CAST(1 AS BIT) AS IsTotalDisconnect
          ,CAST(''MV_Feeder'' AS VARCHAR(20)) AS OutageType 
          ,COUNT(*) AS Count 
          ,SUM(ISNULL(R.DisconnectInterval , 0)) AS DisconnectInterval
          ,SUM(ISNULL(R.DisconnectPower, 0.0)) AS DisconnectPower
        FROM TblRequest R
          INNER JOIN TblFogheToziDisconnect F ON R.FogheToziDisconnectId = F.FogheToziDisconnectId
          INNER JOIN TblFogheToziDisconnectMPFeeder FTMP ON F.FogheToziDisconnectId = FTMP.FogheToziDisconnectId
          INNER JOIN Tbl_MPFeeder MPF ON FTMP.MPFeederId = MPF.MPFeederId
          INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
          INNER JOIN Tbl_Area A ON R.AreaId = A.AreaId
        WHERE R.DisconnectDatePersian BETWEEN ''' + @aFromDate + ''' AND ''' + @aToDate + '''
        ' + CASE WHEN LEN(@aAreaIds) > 0  THEN ' AND A.AreaId IN (' + @aAreaIds + ')' ELSE '' END + 
        '
        GROUP BY A.Area, MPP.MPPostName ,MPF.MPFeederId , MPF.MPFeederName
        
      UNION 
        
        SELECT A.Area
          ,MPP.MPPostName
          ,MPF.MPFeederName
          ,NULL AS LPPostName
          ,NULL AS LPFeederName
          ,R.IsDisconnectMPFeeder AS IsTotalDisconnect
          ,''MV_Feeder'' AS OutageType 
          ,COUNT(*) AS Count 
          ,SUM(ISNULL(R.DisconnectInterval , 0)) AS DisconnectInterval
          ,SUM(ISNULL(R.DisconnectPower, 0.0)) AS DisconnectPower
        FROM TblRequest R
          INNER JOIN TblMPRequest MPR ON R.MPRequestId = MPR.MPRequestId
          INNER JOIN Tbl_DisconnectGroupSet DGSet ON R.DisconnectGroupSetId = DGSet.DisconnectGroupSetId
          INNER JOIN Tbl_DisconnectGroup G ON DGSet.DisconnectGroupId = G.DisconnectGroupId
          INNER JOIN Tbl_MPFeeder MPF ON MPR.MPFeederId = MPF.MPFeederId
          INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
          INNER JOIN Tbl_Area A ON R.AreaId = A.AreaId
        WHERE R.DisconnectDatePersian BETWEEN ''' + @aFromDate + ''' AND ''' + @aToDate + '''
          ' + CASE WHEN LEN(@aAreaIds) > 0  THEN ' AND A.AreaId IN (' + @aAreaIds + ')' ELSE '' END + 
          ' AND G.DisconnectGroupId IN (1002 , 1003)
        GROUP BY A.Area, MPP.MPPostName ,MPF.MPFeederId , MPF.MPFeederName ,R.IsDisconnectMPFeeder
      ) Res
      '
      PRINT(@lSQL)
      EXEC(@lSQL)
  END

/*

EXEC  spGetOutageStat_MPFeeders @aFromDate = '1401/01/01'
                               ,@aToDate = '1401/05/30'
                               ,@aAreaIds = ''


*/