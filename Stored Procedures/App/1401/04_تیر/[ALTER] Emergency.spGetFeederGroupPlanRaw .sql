
ALTER PROC Emergency.spGetFeederGroupPlanRaw 
  @aGroupMPFeederId AS BIGINT,
  @aDayCount AS INT,
  @aDisconnectDT AS DATETIME
AS
BEGIN
  SELECT
    MPT.MPFeederTemplateId
   ,MPF.MPFeederId
   ,MPP.MPPostName
   ,MPF.MPFeederName
   ,Emergency.GetMPFeederDisconnectCount(MPF.MPFeederId, MPT.IsDisconnectMPFeeder, MPT.MPFeederKeyId1,@aDayCount) 
        AS MPFeederDisconnectCount
   ,MPF.Voltage
   ,A.Area
   ,Emergency.GetMPFeederLoadHourId(MPF.MPFeederId , @aDisconnectDT) AS MPFeederLoadHourId
  INTO #tmp
  FROM Emergency.Tbl_MPFeederTemplate MPT
    INNER JOIN Tbl_MPFeeder MPF ON MPT.MPFeederId = MPF.MPFeederId
    INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
    INNER JOIN Tbl_Area A ON MPT.AreaId = A.AreaId
  WHERE GroupMPFeederId = @aGroupMPFeederId

  SELECT
    T.MPFeederTemplateId
   ,T.Area
   ,T.MPFeederId
   ,T.MPPostName
   ,T.MPFeederName
   ,T.MPFeederDisconnectCount
   ,ISNULL(ROUND(SQRT(3) * T.Voltage * H.CurrentValue * H.CosinPhi / 1000000, 2), 0) AS CurrentValueMW
   ,ISNULL(H.CurrentValue, 0) AS CurrentValue
   ,CAST(0 AS BIT) AS IsSelected
  FROM #tmp T
  LEFT JOIN Tbl_MPFeederLoadHours H ON T.MPFeederLoadHourId = H.MPFeederLoadHourId
  ORDER BY T.MPFeederDisconnectCount

  DROP TABLE #tmp
END
GO