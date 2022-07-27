
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
          ,COUNT(MPR.MPFeederId) AS DisconnectFeederCount
    INTO #tmpDisFeederCount
    FROM TblRequest R
      INNER JOIN TblMPRequest MPR ON R.MPRequestId = MPR.MPRequestId
      WHERE MPR.EndJobStateId IN (4,5)
        AND R.IsDisconnectMPFeeder = 1
    
    
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
         ,CAST(M.DisconnectIntervalAvg / 60 AS varchar(5)) + ':' + CAST(M.DisconnectIntervalAvg % 60 AS varchar(2)) 
              AS DisconnectIntervalAvg
         , M.DisconnectPowerSum 
         , C.DisconnectFeederCount
    FROM #tmpMonitoring M
      INNER JOIN #tmpDisFeederCount C ON M.AllAreas = C.AllAreas
    
    
    DROP TABLE #tmpMonitoring
    DROP TABLE #tmpDisFeederCount

  END
GO



/*

EXEC spTavanir_GetMPFeedersDisPower '1401/05/03', '00:00', '1401/05/03' , '23:59'

EXEC spTavanir_GetMPFeedersDisPower 
                                @aFromDate = '1401/03/01'
                                ,@aFromTime = '11:00'
                                ,@aToDate = '1401/05/01'
                                ,@aToTime = '12:00'



*/
/*

SELECT * FROM Tbl_AccessType 

Delete From Tbl_AccessType Where AccessType = 'ReportMPFeederDis'
-------------------------

SET IDENTITY_INSERT Tbl_AccessType ON
INSERT INTO Tbl_AccessType (AccessTypeId, AccessType, AccessTypeName)
  VALUES (8, N'ReportMPFeederDis', N'توانايي گزارشگيري گزارش تجمعي قطعي فيدرهاي فشار متوسط');
SET IDENTITY_INSERT Tbl_AccessType OFF


*/

