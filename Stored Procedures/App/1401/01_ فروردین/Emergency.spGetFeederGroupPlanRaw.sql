USE CCRequesterSetad
GO

ALTER PROC Emergency.spGetFeederGroupPlanRaw @aGroupMPFeederId AS BIGINT
AS
BEGIN
  SELECT
    MPT.MPFeederTemplateId
   ,MPF.MPFeederId
   ,MPP.MPPostName
   ,MPF.MPFeederName
   ,[Emergency].[GetMPFeederDisconnectCount](MPF.MPFeederId, MPT.IsDisconnectMPFeeder, MPT.MPFeederKeyId1) AS MPFeederDisconnectCount
   ,MPF.Voltage
   ,A.Area INTO #tmp
  FROM Emergency.Tbl_MPFeederTemplate MPT
  INNER JOIN Tbl_MPFeeder MPF
    ON MPT.MPFeederId = MPF.MPFeederId
  INNER JOIN Tbl_MPPost MPP
    ON MPF.MPPostId = MPP.MPPostId
  INNER JOIN Tbl_Area A
    ON MPT.AreaId = A.AreaId
  WHERE GroupMPFeederId = @aGroupMPFeederId

  SELECT
    T.MPFeederTemplateId
   ,T.Area
   ,T.MPFeederId
   ,T.MPPostName
   ,T.MPFeederName
   ,T.MPFeederDisconnectCount
   ,ISNULL(ROUND(3 * T.Voltage * C.CurrentValue * C.CosinPhi / 1000000, 2), 0) AS CurrentValueMW
   ,ISNULL(C.CurrentValue, 0) AS CurrentValue
   ,CAST(0 AS BIT) AS IsSelected
  FROM #tmp T
  LEFT JOIN (SELECT
      L.MPFeederId
     ,H.CurrentValue
     ,H.CosinPhi
     ,H.HourId
     ,ROW_NUMBER() OVER (
      PARTITION BY L.MPFeederId ORDER BY L.RelDate
      , H.HourId DESC
      ) AS RowNum
    FROM Tbl_MPFeederLoad L
    INNER JOIN Tbl_MPFeederLoadHours H
      ON L.MPFeederLoadId = H.MPFeederLoadId) C
    ON C.MPFeederId = T.MPFeederId
  WHERE ISNULL(C.RowNum, 1) = 1
  ORDER BY T.MPFeederDisconnectCount

  DROP TABLE #tmp
END
GO