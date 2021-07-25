USE CcRequesterSetad
GO
CREATE PROCEDURE spSKerman_part1
  @From VARCHAR(10),
  @To VARCHAR(10)
  AS 
  BEGIN
    DECLARE @FromYM VARCHAR(10) = SUBSTRING( @From, 1, 7) 
    DECLARE @TOYM VARCHAR(10) = SUBSTRING( @To, 1, 7)
    DECLARE @Days INT = DATEDIFF(DAY , dbo.shtom(@From) , dbo.shtom(@TO))
    DECLARE @CntAllPublicMPFeeders AS INT = (SELECT COUNT(*) FROM Tbl_MPFeeder WHERE ISNULL(OwnershipId,2) = 2)
    DECLARE @CntAllPrivateMPFeeders AS INT = (SELECT COUNT(*) FROM Tbl_MPFeeder WHERE OwnershipId = 1)
	  SELECT RequestId, MPRequestId , LPRequestId INTO #tmp FROM TblRequest WHERE DisconnectDatePersian BETWEEN @From AND @To
    
    SELECT ta.AreaId ,ta.Area ,ROUND(ISNULL(TMP.DisPowerMP_Tamir,0),2) AS DisPowerMP_Tamir 
    ,ISNULL(TMP.DisIntervalMP_Tamir,0) AS DisIntervalMP_Tamir
	  ,ROUND(ISNULL(TMP.DisPowerMP,0),2) AS DisPowerMP, ISNULL(TMP.DisIntervalMP,0) AS DisIntervalMP
    ,ISNULL(TMP.CountMP_Tamir,0) AS CntMP_Tamir, ISNULL(TMP.CountMP,0) AS CntMP
	  ,ROUND(ISNULL(TMP.DisAvgMP,0),2) AS DisAvgMP , ROUND(ISNULL( TLP.DisAvgLP,0),2) AS DisAvgLP
    ,ISNULL(TMP.CountMPFromPost,0) AS CntMPFromPost ,ROUND(ISNULL(TLP.DisPowerLP_Tamir,0),2) AS DisPowerLP_Tamir
	  ,ISNULL(TLP.DisIntervalLP_Tamir,0) AS DisIntervalLP_Tamir, ROUND(ISNULL(TLP.DisPowerLP,0),2) AS DisPowerLP
	  ,ISNULL(TLP.DisIntervalLP,0) AS DisIntervalLP ,ISNULL(TLP.CountLP_Tamir,0) AS CntLP_Tamir
	  ,ISNULL(TLP.CountLP,0) AS CntLP ,ISNULL(TMP.CountMPFromPost_Tamir,0) AS CntMPFromPost_Tamir 
    ,ISNULL(FeederReq.FeederCount,0) AS FeederCnt ,ISNULL(ROUND(FeederReq.FeederPercent,2),0) AS FeederPercent
    ,@CntAllPublicMPFeeders AS cntPublicMPFeeder , @CntAllPrivateMPFeeders AS cntPrivateMPFeeder
    ,ISNULL(FaultyFeeder.MPFeederName , '') AS FaultyFeederName
    ,ROUND(ISNULL(SAIDI.DisPowTamir,0),2) AS SAIDI_Tamir ,ROUND(ISNULL(SAIDI.DisPow,0),2) AS SAIDI
    ,ROUND(ISNULL(Rate.Rate,0),2) AS Rate ,ROUND(ISNULL(Rate.DisIntRate,0),2) AS DisRate
      FROM
    Tbl_Area ta LEFT JOIN(
    SELECT R.AreaId, SUM( CASE WHEN R.IsTamir = 1 THEN R.DisconnectPower END ) AS DisPowerMP_Tamir
        , SUM( CASE WHEN R.IsTamir = 1 THEN R.DisconnectInterval END ) AS DisIntervalMP_Tamir
        , COUNT( CASE WHEN R.IsTamir = 1 THEN R.RequestId END ) AS CountMP_Tamir
        , SUM( CASE WHEN R.IsTamir = 0 AND R.DisconnectInterval >= 5  THEN R.DisconnectPower END ) AS DisPowerMP
        , SUM( CASE WHEN R.IsTamir = 0 AND R.DisconnectInterval >= 5 THEN R.DisconnectInterval END ) AS DisIntervalMP
        , COUNT( CASE WHEN R.IsTamir = 0 AND R.DisconnectInterval >= 5 THEN R.RequestId END ) AS CountMP
        , CAST(SUM(CASE WHEN R.IsTamir = 0 THEN R.DisconnectInterval END) AS FLOAT) /
            COUNT(CASE WHEN R.IsTamir = 0 THEN R.RequestId END) AS DisAvgMP
        , COUNT( CASE WHEN R.IsTamir = 1 AND R.IsDisconnectMPFeeder = 1 THEN R.RequestId END ) AS CountMPFromPost_Tamir
        , COUNT( CASE WHEN R.IsTamir = 0 AND R.IsDisconnectMPFeeder = 1 THEN R.RequestId END ) AS CountMPFromPost 
      FROM TblMPRequest MP
        INNER JOIN TblRequest R ON MP.MPRequestId = R.MPRequestId
        INNER JOIN #tmp ON R.RequestId = #tmp.RequestId
        WHERE (MP.DisconnectReasonId IS NULL OR MP.DisconnectReasonId < 1200 OR MP.DisconnectReasonId > 1299 ) 
        AND (MP.DisconnectGroupSetId IS NULL OR MP.DisconnectGroupSetId <> 1129 AND MP.DisconnectGroupSetId <> 1130)
        AND R.DisconnectInterval >= 5
        GROUP BY  R.AreaId ) TMP ON ta.AreaId = TMP.AreaId
    LEFT JOIN(
      SELECT R.AreaId, SUM(CASE WHEN R.IsTamir = 1 THEN R.DisconnectPower END) AS DisPowerLP_Tamir
        , SUM(CASE WHEN R.IsTamir = 1 THEN R.DisconnectInterval END) AS DisIntervalLP_Tamir
        , COUNT(CASE WHEN R.IsTamir = 1 THEN R.RequestId END) AS CountLP_Tamir
        , SUM(CASE WHEN R.IsTamir = 0 THEN R.DisconnectPower END) AS DisPowerLP
        , SUM(CASE WHEN R.IsTamir = 0 THEN R.DisconnectInterval END) AS DisIntervalLP
        , COUNT(CASE WHEN R.IsTamir = 0 THEN R.RequestId END) AS CountLP
        , CAST(SUM(CASE WHEN R.IsTamir = 0 THEN R.DisconnectInterval END) AS FLOAT) /
            COUNT(CASE WHEN R.IsTamir = 0 THEN R.RequestId END) AS DisAvgLP
      FROM TblLPRequest LP
        INNER JOIN TblRequest R ON LP.LPRequestId = R.LPRequestId
        INNER JOIN #tmp ON R.RequestId = #tmp.RequestId
        GROUP BY R.AreaId
        ) TLP ON ta.AreaId = TLP.AreaId
    LEFT JOIN(
      SELECT R.AreaId
        ,1000 * CAST(SUM(R.DisconnectPower) AS FLOAT) / SUM(Sub.Energy) AS Rate
        ,24 * 60 * CAST(SUM(R.DisconnectInterval) AS FLOAT) / SUM(Sub.Energy) AS DisIntRate
      FROM TblRequest R
        INNER JOIN TblMPRequest MP ON R.MPRequestId = MP.MPRequestId
        INNER JOIN #tmp ON R.RequestId = #tmp.RequestId
        INNER JOIN TblSubscribers Sub ON Sub.AreaId = R.AreaId
        WHERE Sub.YearMonth BETWEEN @FromYM AND @TOYM
        AND (R.IsLPRequest = 1 OR (R.IsMPRequest = 1 AND (MP.DisconnectReasonId IS NULL OR MP.DisconnectReasonId < 1200 OR
            MP.DisconnectReasonId > 1299 ) AND (MP.DisconnectGroupSetId IS NULL OR MP.DisconnectGroupSetId <> 1129 
            AND MP.DisconnectGroupSetId <> 1130)))
        GROUP BY R.AreaId
        ) Rate ON ta.AreaId = Rate.AreaId
    LEFT JOIN(
      SELECT MP.AreaId 
        , COUNT(DISTINCT Feeder.MPFeederId) FeederCount
      , 100 * CAST(COUNT(DISTINCT Feeder.MPFeederId) AS FLOAT) / @CntAllPublicMPFeeders AS FeederPercent
        FROM TblMPRequest MP
        INNER JOIN TblRequest R ON MP.MPRequestId = R.MPRequestId
        INNER JOIN Tbl_MPFeeder Feeder ON MP.MPFeederId = Feeder.MPFeederId
        INNER JOIN #tmp ON MP.MPRequestId = #tmp.MPRequestId
        WHERE (MP.DisconnectReasonId IS NULL OR MP.DisconnectReasonId < 1200 OR MP.DisconnectReasonId > 1299 ) 
          AND (MP.DisconnectGroupSetId IS NULL OR MP.DisconnectGroupSetId <> 1129 AND MP.DisconnectGroupSetId <> 1130)
          AND R.IsDisconnectMPFeeder = 1 AND R.IsTamir = 0 AND ISNULL(Feeder.OwnershipId,2) = 2
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
        GROUP BY MP.AreaId , MP.MPFeederId , Feeder.MPFeederName
        ORDER BY SUM(MP.DisconnectPower) DESC
        ) FaultyFeeder ON  ta.AreaId = FaultyFeeder.AreaId
    LEFT JOIN(
        SELECT R.AreaId
        ,60 * 24 * @Days * CAST(SUM(CASE WHEN R.IsTamir = 1 THEN R.DisconnectPower END) AS FLOAT) / SUM(Sub.Energy) AS DisPowTamir
        ,60 * 24 * @Days * CAST(SUM(CASE WHEN R.IsTamir = 0 THEN R.DisconnectPower END) AS FLOAT) / SUM(Sub.Energy) AS DisPow
        FROM TblRequest R
        INNER JOIN #tmp t ON R.RequestId = t.RequestId
        INNER JOIN TblSubscribers Sub ON Sub.AreaId = R.AreaId
        WHERE Sub.YearMonth BETWEEN @FromYM AND @TOYM
        GROUP BY R.AreaId
        )SAIDI ON ta.AreaId = SAIDI.AreaId
      WHERE ta.IsCenter = 0
    ORDER BY ta.AreaId ASC
    
    DROP TABLE #tmp
  END

--   EXEC spsKerman_part1 @From = '1397/04/30' ,@To = '1400/04/30' 