CREATE PROCEDURE spGetOutageStat_LPFeeders
    @aFromDate AS VARCHAR(11),
    @aToDate AS VARCHAR(11),
    @aAreaIds AS VARCHAR(2000) = ''
  AS
  BEGIN
      DECLARE @lSQL AS NVARCHAR(MAX) = ' 
        SELECT A.Area
          ,MPP.MPPostName
          ,MPF.MPFeederName
          ,LPP.LPPostName
          ,LPF.LPFeederName
          ,LPR.IsTotalLPFeederDisconnected AS IsTotalDisconnect
          ,CAST(''LV_Feeder'' AS VARCHAR(20)) AS OutageType 
          ,COUNT(*) AS Count 
          ,SUM(ISNULL(R.DisconnectInterval , 0)) AS DisconnectInterval
          ,SUM(ISNULL(R.DisconnectPower, 0.0)) AS DisconnectPower
        FROM TblRequest R
          INNER JOIN TblLPRequest LPR ON R.LPRequestId = LPR.LPRequestId
          INNER JOIN Tbl_LPFeeder LPF ON LPR.LPFeederId = LPF.LPFeederId
          INNER JOIN Tbl_LPPost LPP ON LPF.LPPostId = LPP.LPPostId
          INNER JOIN Tbl_MPFeeder MPF ON LPP.MPFeederId = MPF.MPFeederId
          INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
          INNER JOIN Tbl_Area A ON R.AreaId = A.AreaId
        WHERE R.DisconnectDatePersian BETWEEN ''' + @aFromDate + ''' AND ''' + @aToDate +'''
        ' + CASE WHEN LEN(@aAreaIds) > 0  THEN ' AND A.AreaId IN (' + @aAreaIds + ')' ELSE '' END + 
        ' AND LPR.IsTotalLPPostDisconnected = 0 
          AND ISNULL(LPR.IsSingleSubscriber , 0) = 0
        GROUP BY A.Area, MPP.MPPostName ,MPF.MPFeederName, LPP.LPPostName , 
                LPF.LPFeederId , LPF.LPFeederName ,LPR.IsTotalLPFeederDisconnected
        '
      PRINT(@lSQL)
      EXEC(@lSQL)
  END

/*

EXEC spGetOutageStat_LPFeeders @aFromDate = '1400/01/01'
                             ,@aToDate = '1401/05/30'
                             ,@aAreaIds = ''

*/