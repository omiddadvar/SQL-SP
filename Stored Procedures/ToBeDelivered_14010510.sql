CREATE PROCEDURE dbo.spTavanir_GetMPFeedersDisPower
   @aFromDate AS VARCHAR(10)
   ,@aFromTime AS VARCHAR(5)
   ,@aToDate AS VARCHAR(10)
   ,@aToTime AS VARCHAR(5)
AS
  BEGIN
    DECLARE @lFromDate AS DATETIME = dbo.ShamsiDateTimeToMiladi(@aFromDate , @aFromTime)
           ,@lToDate AS DATETIME = dbo.ShamsiDateTimeToMiladi(@aToDate , @aToTime)

    SELECT CAST(1 AS Bit) AS AllAreas 
          ,COUNT(DISTINCT MPR.MPFeederId) AS DisconnectFeederCount
    INTO #tmpDisFeederCount
    FROM TblRequest R
      INNER JOIN TblMPRequest MPR ON R.MPRequestId = MPR.MPRequestId
      WHERE R.IsDisconnectMPFeeder = 1
        AND MPR.DisconnectDT BETWEEN @lFromDate AND @lToDate

    SELECT CAST(1 AS Bit) AS AllAreas 
          ,COUNT(DISTINCT LiveMPR.MPFeederId) AS LiveDisconnectFeederCount
    INTO #tmpLiveDisFeederCount
    FROM TblRequest R
      INNER JOIN TblMPRequest LiveMPR ON R.MPRequestId = LiveMPR.MPRequestId 
      WHERE R.IsDisconnectMPFeeder = 1
        AND LiveMPR.EndJobStateId IN (4,5)
    
    
    SELECT 
         CAST(1 AS Bit) AS AllAreas
        ,ISNULL(ROUND(AVG(ISNULL(V.CurrentValue, 0)) , 3), 0) AS CurrentValueAvg
        ,ISNULL(AVG(ABS(ISNULL(R.DisconnectInterval ,0))), 0) AS DisconnectIntervalAvg
        ,ISNULL(ROUND(SUM(ISNULL(V.DisconnectPower, 0)) , 3),0) AS DisconnectPowerSum
    INTO #tmpMonitoring
    FROM ViewMonitoring V
      INNER JOIN TblRequest R ON V.RequestId = R.RequestId
    WHERE R.DisconnectDT BETWEEN @lFromDate AND @lToDate
      AND R.IsDisconnectMPFeeder = 1
    
    
    SELECT M.CurrentValueAvg
         ,CAST(M.DisconnectIntervalAvg / 60 AS varchar(5)) + ':' + CAST(M.DisconnectIntervalAvg % 60 AS varchar(2))               AS DisconnectIntervalAvg
         , M.DisconnectPowerSum 
         , C.DisconnectFeederCount
         , L.LiveDisconnectFeederCount
    FROM #tmpMonitoring M
      INNER JOIN #tmpDisFeederCount C ON M.AllAreas = C.AllAreas
      INNER JOIN #tmpLiveDisFeederCount L ON M.AllAreas = C.AllAreas
    
    
    DROP TABLE #tmpMonitoring
    DROP TABLE #tmpDisFeederCount
    DROP TABLE #tmpLiveDisFeederCount

  END
GO