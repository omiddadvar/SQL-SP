USE CcRequesterSetad
GO
ALTER PROCEDURE spsKerman_part1
  @From VARCHAR(10),
  @To VARCHAR(10)
  AS 
  BEGIN
    DECLARE @FromYM VARCHAR(10) = SUBSTRING( @From, 1, 7) 
    DECLARE @TOYM VARCHAR(10) = SUBSTRING( @To, 1, 7)
    DECLARE @Days INT = DATEDIFF(DAY , dbo.shtom(@From) , dbo.shtom(@TO))
	  SELECT RequestId, MPRequestId , LPRequestId INTO #tmp FROM TblRequest WHERE DisconnectDatePersian BETWEEN @From AND @To
    
    SELECT ta.AreaId ,ta.Area ,ISNULL(TMP.DisPowerMP_Tamir,0) AS DisPowerMP_Tamir 
    ,ISNULL(TMP.DisIntervalMP_Tamir,0) AS DisIntervalMP_Tamir
	  ,ISNULL(TMP.DisPowerMP,0) AS DisPowerMP, ISNULL(TMP.DisIntervalMP,0) AS DisIntervalMP
    ,ISNULL(TMP.CountMP_Tamir,0) AS CountMP_Tamir, ISNULL(TMP.CountMP,0) AS CntMP
	  ,ISNULL(TMP.DisAvgMP,0) AS DisAvgMP ,ISNULL(TMP.CountMPFromPost_Tamir,0) AS CntMPFromPost_Tamir 
    ,ISNULL(TMP.CountMPFromPost,0) AS CountMPFromPost ,ISNULL(TLP.DisPowerLP_Tamir,0) AS DisPowerLP_Tamir
	  ,ISNULL(TLP.DisIntervalLP_Tamir,0) AS DisIntervalLP_Tamir, ISNULL(TLP.DisPowerLP,0) AS DisPowerLP
	  ,ISNULL(TLP.DisIntervalLP,0) AS DisIntervalLP ,ISNULL(TLP.CountLP_Tamir,0) AS CntLP_Tamir
	  ,ISNULL(TLP.CountLP,0) AS CountLP, ISNULL( TLP.DisAvgLP,0) AS DisAvgLP
	  ,ISNULL(TAvgMP.AverageDisInterval,0) AS AvgDisIntervalMP , ISNULL(TAvgLP.AverageDisInterval,0) AS AvgDisIntervalLP
    ,ISNULL(FeederReq.FeederCount,0) AS FeederCnt ,ISNULL(FaultyFeeder.MPFeederName , '') AS FaultyFeederName
    ,ISNULL(SAIDI.DisPowTamir,0) AS SAIDI_Tamir ,ISNULL(SAIDI.DisPow,0) AS SAIDI
    ,ISNULL(Rate.Rate,0) AS Rate ,ISNULL(Rate.DisIntRate,0) AS DisRate
      FROM
    Tbl_Area ta LEFT JOIN(
    SELECT R.AreaId, SUM( CASE WHEN R.IsTamir = 1 THEN R.DisconnectPower END ) AS DisPowerMP_Tamir
        , SUM( CASE WHEN R.IsTamir = 1 THEN R.DisconnectInterval END ) AS DisIntervalMP_Tamir
        , COUNT( CASE WHEN R.IsTamir = 1 THEN R.RequestId END ) AS CountMP_Tamir
        , SUM( CASE WHEN R.IsTamir = 0 AND R.DisconnectInterval >= 5  THEN R.DisconnectPower END ) AS DisPowerMP
        , SUM( CASE WHEN R.IsTamir = 0 AND R.DisconnectInterval >= 5 THEN R.DisconnectInterval END ) AS DisIntervalMP
        , COUNT( CASE WHEN R.IsTamir = 0 AND R.DisconnectInterval >= 5 THEN R.RequestId END ) AS CountMP
        , SUM(CASE WHEN R.IsTamir = 0 THEN MP.DisconnectInterval END) /
            COUNT(CASE WHEN R.IsTamir = 0 THEN MP.MPRequestId END) AS DisAvgMP
        , COUNT( CASE WHEN R.IsTamir = 1 AND R.IsDisconnectMPFeeder = 1 THEN R.RequestId END ) AS CountMPFromPost_Tamir
        , COUNT( CASE WHEN R.IsTamir = 0 AND R.IsDisconnectMPFeeder = 1 THEN R.RequestId END ) AS CountMPFromPost 
      FROM TblMPRequest MP
        INNER JOIN TblRequest R ON MP.MPRequestId = R.MPRequestId
        INNER JOIN #tmp ON R.RequestId = #tmp.RequestId
        WHERE (MP.DisconnectReasonId IS NULL OR MP.DisconnectReasonId < 1200 OR MP.DisconnectReasonId > 1299 ) 
        AND (MP.DisconnectGroupSetId IS NULL OR MP.DisconnectGroupSetId <> 1129 AND MP.DisconnectGroupSetId <> 1130)
        AND R.DisconnectInterval >= 5 AND R.DisconnectDatePersian BETWEEN @From AND @To
        GROUP BY  R.AreaId ) TMP ON ta.AreaId = TMP.AreaId
    LEFT JOIN(
      SELECT R.AreaId, SUM(CASE WHEN R.IsTamir = 1 THEN R.DisconnectPower END) AS DisPowerLP_Tamir
        , SUM(CASE WHEN R.IsTamir = 1 THEN R.DisconnectInterval END) AS DisIntervalLP_Tamir
        , COUNT(CASE WHEN R.IsTamir = 1 THEN R.RequestId END) AS CountLP_Tamir
        , SUM(CASE WHEN R.IsTamir = 0 THEN R.DisconnectPower END) AS DisPowerLP
        , SUM(CASE WHEN R.IsTamir = 0 THEN R.DisconnectInterval END) AS DisIntervalLP
        , COUNT(CASE WHEN R.IsTamir = 0 THEN R.RequestId END) AS CountLP
        , SUM(CASE WHEN R.IsTamir = 0 THEN LP.DisconnectInterval END) /
            COUNT(CASE WHEN R.IsTamir = 0 THEN LP.LPRequestId END) AS DisAvgLP
      FROM TblLPRequest LP
        INNER JOIN TblRequest R ON LP.LPRequestId = R.LPRequestId
        INNER JOIN #tmp ON R.RequestId = #tmp.RequestId
        WHERE R.DisconnectDatePersian BETWEEN @From AND @To
        GROUP BY R.AreaId
        ) TLP ON ta.AreaId = TLP.AreaId
    LEFT JOIN(
      SELECT R.AreaId
        ,1000 * SUM(R.DisconnectPower) / SUM(Sub.Energy) AS Rate
        ,24 * 60 * SUM(R.DisconnectInterval) / SUM(Sub.Energy) AS DisIntRate
      FROM TblRequest R
        INNER JOIN TblMPRequest MP ON R.MPRequestId = MP.MPRequestId
        INNER JOIN #tmp ON R.RequestId = #tmp.RequestId
        INNER JOIN TblSubscribers Sub ON Sub.AreaId = R.AreaId
        WHERE R.DisconnectDatePersian BETWEEN @From AND @To AND Sub.YearMonth BETWEEN @FromYM AND @TOYM
        AND (R.IsLPRequest = 1 OR (R.IsMPRequest = 1 AND (MP.DisconnectReasonId IS NULL OR MP.DisconnectReasonId < 1200 OR
            MP.DisconnectReasonId > 1299 ) AND (MP.DisconnectGroupSetId IS NULL OR MP.DisconnectGroupSetId <> 1129 
            AND MP.DisconnectGroupSetId <> 1130)))
        GROUP BY R.AreaId
        ) Rate ON ta.AreaId = Rate.AreaId
    LEFT JOIN(
      SELECT R.AreaId, SUM(R.DisconnectInterval) / COUNT(R.RequestId) AS AverageDisInterval
      FROM TblMPRequest MP
        INNER JOIN TblRequest R ON MP.MPRequestId = R.MPRequestId
        INNER JOIN #tmp ON R.RequestId = #tmp.RequestId
        WHERE R.IsTamir = 0 AND R.DisconnectDatePersian BETWEEN @From AND @To
        GROUP BY R.AreaId
        ) TAvgMP ON ta.AreaId = TAvgMP.AreaId
    LEFT JOIN(
      SELECT R.AreaId, SUM(R.DisconnectInterval) / COUNT(R.RequestId) AS AverageDisInterval
      FROM TblLPRequest LP
        INNER JOIN TblRequest R ON LP.LPRequestId = R.LPRequestId
        INNER JOIN #tmp ON R.RequestId = #tmp.RequestId
        WHERE R.IsTamir = 0 AND R.DisconnectDatePersian BETWEEN @From AND @To
        GROUP BY R.AreaId
        ) TAvgLP ON ta.AreaId = TAvgLP.AreaId
        ----------------------------------TODO----------
    LEFT JOIN(
      SELECT COUNT(DISTINCT Feeder.MPFeederId) FeederCount,  MP.AreaId 
        FROM TblMPRequest MP
        INNER JOIN TblRequest R ON MP.MPRequestId = R.MPRequestId
        INNER JOIN Tbl_MPFeeder Feeder ON MP.MPFeederId = Feeder.MPFeederId
        INNER JOIN #tmp ON MP.MPRequestId = #tmp.MPRequestId
        WHERE (MP.DisconnectReasonId IS NULL OR MP.DisconnectReasonId < 1200 OR MP.DisconnectReasonId > 1299 ) 
          AND (MP.DisconnectGroupSetId IS NULL OR MP.DisconnectGroupSetId <> 1129 AND MP.DisconnectGroupSetId <> 1130)
          AND MP.DisconnectDatePersian BETWEEN @From AND @To 
          AND R.IsDisconnectMPFeeder = 1 AND R.IsTamir = 0 AND Feeder.IsPrivate = 0
        GROUP BY MP.AreaId
        HAVING COUNT(MP.MPRequestId) >= 3 
        ) FeederReq ON  ta.AreaId = FeederReq.AreaId
    LEFT JOIN(
      SELECT TOP(1) Feeder.MPFeederName , MP.AreaId 
        FROM TblMPRequest MP
        INNER JOIN Tbl_MPFeeder Feeder ON MP.MPFeederId = Feeder.MPFeederId
        INNER JOIN #tmp ON MP.MPRequestId = #tmp.MPRequestId
        WHERE (MP.DisconnectReasonId IS NULL OR MP.DisconnectReasonId < 1200 OR MP.DisconnectReasonId > 1299 ) 
          AND (MP.DisconnectGroupSetId IS NULL OR MP.DisconnectGroupSetId <> 1129 AND MP.DisconnectGroupSetId <> 1130)
          AND MP.DisconnectDatePersian BETWEEN @From AND @To
        GROUP BY MP.AreaId , MP.MPFeederId , Feeder.MPFeederName
        ORDER BY SUM(MP.DisconnectPower) DESC
        ) FaultyFeeder ON  ta.AreaId = FaultyFeeder.AreaId
    LEFT JOIN(
        SELECT R.AreaId
        ,60 * 24 * @Days * SUM(CASE WHEN R.IsTamir = 1 THEN R.DisconnectPower END) / SUM(Sub.Energy) AS DisPowTamir
        ,60 * 24 * @Days * SUM(CASE WHEN R.IsTamir = 0 THEN R.DisconnectPower END) / SUM(Sub.Energy) AS DisPow
        FROM TblRequest R
        INNER JOIN #tmp t ON R.RequestId = t.RequestId
        INNER JOIN TblSubscribers Sub ON Sub.AreaId = R.AreaId
        WHERE R.DisconnectDatePersian BETWEEN @From AND @To
         AND Sub.YearMonth BETWEEN @FromYM AND @TOYM
        GROUP BY R.AreaId
        )SAIDI ON ta.AreaId = SAIDI.AreaId
      WHERE ta.IsCenter = 0
    ORDER BY ta.AreaId ASC
    
    DROP TABLE #tmp
  END

--   EXEC spsKerman_part1 @From = '1397/04/30' ,@To = '1400/04/30' 