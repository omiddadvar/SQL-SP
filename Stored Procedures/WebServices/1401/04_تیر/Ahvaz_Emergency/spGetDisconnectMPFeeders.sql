
CREATE PROCEDURE spGetDisconnectMPFeeders
  @aAreaIds VARCHAR(MAX)
  AS
  BEGIN
  	SELECT Item AS AreaId INTO #tmpArea FROM dbo.Split(@aAreaIds,',')
    
    
    SELECT * INTO #tmp FROM 
      (
      SELECT MPR.MPFeederId , R.IsDisconnectMPFeeder ,COUNT(MPR.MPRequestId) AS Count
      FROM TblMPRequest MPR
        INNER JOIN TblRequest R ON MPR.MPRequestId = R.MPRequestId
        INNER JOIN Tbl_MPFeeder MPF ON MPR.MPFeederId = MPF.MPFeederId
        INNER JOIN #tmpArea A ON MPF.AreaId = A.AreaId
      WHERE MPR.EndJobStateId IN (4,5)
        AND R.IsDisconnectMPFeeder = 1
      GROUP BY MPR.MPFeederId , R.IsDisconnectMPFeeder
    UNION
      SELECT MPR.MPFeederId , R.IsDisconnectMPFeeder  ,COUNT(MPR.MPRequestId) AS Count
      FROM TblMPRequest MPR
        INNER JOIN TblRequest R ON MPR.MPRequestId = R.MPRequestId
        INNER JOIN #tmpArea A ON MPR.AreaId = A.AreaId
      WHERE MPR.EndJobStateId IN (4,5)
        AND R.IsDisconnectMPFeeder = 0 
        AND MPR.IsNotDisconnectFeeder = 1
      GROUP BY MPR.MPFeederId , R.IsDisconnectMPFeeder
      ) Temp
    
    
    SELECT A.AreaId , A.Area
          ,MPP.MPPostName , MPP.MPPostCode
          ,MPF.MPFeederName , MPF.MPFeederCode
          ,T.IsDisconnectMPFeeder , T.Count
    FROM Tbl_MPFeeder MPF
      INNER JOIN #tmp T ON MPF.MPFeederId = T.MPFeederId
      INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
      INNER JOIN Tbl_Area A ON MPF.AreaId = A.AreaId
    
    
    DROP TABLE #tmpArea
    DROP TABLE #tmp
  END


EXEC spGetDisconnectMPFeeders @aAreaIds = '2,3'