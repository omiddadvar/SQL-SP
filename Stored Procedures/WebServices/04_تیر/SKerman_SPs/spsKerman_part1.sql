USE CcRequesterSetad
GO
ALTER PROCEDURE spsKerman_part1
  @From VARCHAR(10),
  @To VARCHAR(10)
  AS 
  BEGIN
	  SELECT RequestId INTO #tmp FROM TblRequest WHERE DisconnectDatePersian BETWEEN @From AND @To
    
    SELECT ta.AreaId ,ta.Area ,ISNULL(TMP.DisPowerMP_Tamir,0) AS DisPowerMP_Tamir 
    ,ISNULL(TMP.DisIntervalMP_Tamir,0) AS DisIntervalMP_Tamir
	  ,ISNULL(TMP.DisPowerMP,0) AS DisPowerMP, ISNULL(TMP.DisIntervalMP,0) AS DisIntervalMP
    ,ISNULL(TMP.CountMP_Tamir,0) AS CountMP_Tamir, ISNULL(TMP.CountMP,0) AS CountMP
	  ,ISNULL(TMP.DisAvgMP,0) AS DisAvgMP ,ISNULL(TMP.CountMPFromPost_Tamir,0) AS CountMPFromPost_Tamir 
    ,ISNULL(TMP.CountMPFromPost,0) AS CountMPFromPost ,ISNULL(TLP.DisPowerLP_Tamir,0) AS DisPowerLP_Tamir
	  ,ISNULL(TLP.DisIntervalLP_Tamir,0) AS DisIntervalLP_Tamir, ISNULL(TLP.DisPowerLP,0) AS DisPowerLP
	  ,ISNULL(TLP.DisIntervalLP,0) AS DisIntervalLP ,ISNULL(TLP.CountLP_Tamir,0) AS CountLP_Tamir
	  ,ISNULL(TLP.CountLP,0) AS CountLP, ISNULL( TLP.DisAvgLP,0) AS DisAvgLP
	  ,ISNULL(TAVG.AverageDisInterval,0) AS AverageDisInterval
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
        AND R.DisconnectInterval >= 5 
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
        GROUP BY R.AreaId
        ) TLP ON ta.AreaId = TLP.AreaId
    LEFT JOIN(
      SELECT R.AreaId, SUM(R.DisconnectInterval) / COUNT(R.RequestId) AS AverageDisInterval
      FROM TblMPRequest MP
        INNER JOIN TblRequest R ON MP.MPRequestId = R.MPRequestId
        INNER JOIN #tmp ON R.RequestId = #tmp.RequestId
        WHERE R.IsTamir = 0
        GROUP BY R.AreaId
        ) TAVG ON ta.AreaId = TAVG.AreaId
      WHERE ta.IsCenter = 0

    DROP TABLE #tmp
  END

--EXEC spsKerman_part1 @From = '1399/04/30' ,@To = '1400/04/30' 