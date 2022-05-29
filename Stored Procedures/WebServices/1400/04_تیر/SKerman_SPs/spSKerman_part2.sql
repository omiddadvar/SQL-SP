USE CcRequesterSetad
GO
CREATE PROCEDURE spSKerman_part2
  @From VARCHAR(10),
  @To VARCHAR(10)
  AS 
  BEGIN
    SELECT RequestId, MPRequestId , LPRequestId INTO #tmp FROM TblRequest WHERE DisconnectDatePersian BETWEEN @From AND @To

    SELECT Tbl.AreaId , Tbl.Area , Tbl.Year , Tbl.Month , MAX(Tbl.DicCnt_Tamir) AS DicCnt_Tamir, MAX(Tbl.DicCnt) AS DicCnt 
      , MAX(Tbl.DisPower) AS DisPower, MAX(Tbl.SubscriberCount) AS SubscriberCount 
      FROM (
      SELECT ta.AreaId , ta.Area , DisCntMP.Year , DisCntMP.Month
        , ISNULL(DisCntMP.DicCnt_Tamir ,0) AS DicCnt_Tamir ,ISNULL(DisCntMP.DicCnt , 0) AS DicCnt  
        , 0 AS DisPower, 0 AS SubscriberCount
        FROM Tbl_Area ta
        LEFT JOIN (
          SELECT MP.AreaId , SUBSTRING(MP.DisconnectDatePersian,1,4) AS Year, SUBSTRING(MP.DisconnectDatePersian,6,2) AS Month
            , COUNT(CASE WHEN MP.IsMPTamir = 1 THEN MP.MPRequestId END) AS DicCnt_Tamir
            , COUNT(CASE WHEN MP.IsMPTamir = 0 THEN MP.MPRequestId END) AS DicCnt
            FROM TblMPRequest MP
            INNER JOIN #tmp t ON t.MPRequestId = MP.MPRequestId
            INNER JOIN TblRequestInfo info ON t.RequestId = info.RequestId
            WHERE MP.DisconnectInterval >= 5
          GROUP BY MP.AreaId , SUBSTRING(MP.DisconnectDatePersian,1,4), SUBSTRING(MP.DisconnectDatePersian,6,2)
          HAVING COUNT(ISNULL(MP.GISDCSubscriberCount , ISNULL(info.DCSubscriberCount , 0))) >= 1000
        ) DisCntMP ON ta.AreaId = DisCntMP.AreaId
        WHERE ta.IsCenter = 0 AND DisCntMP.Year IS NOT NULL AND DisCntMP.Month IS NOT NULL
        UNION ALL
        SELECT ta.AreaId , ta.Area , DisMP.Year , DisMP.Month
        , 0 AS DicCnt_Tamir ,0 AS DicCnt  
        , ISNULL( DisMP.DisPower , 0) AS DisPower, ISNULL( DisMP.SubscriberCount , 0) AS SubscriberCount
        FROM Tbl_Area ta
        LEFT JOIN (
          SELECT MP.AreaId , SUBSTRING(MP.DisconnectDatePersian,1,4) AS Year, SUBSTRING(MP.DisconnectDatePersian,6,2) AS Month
            , SUM(CASE WHEN MP.DisconnectInterval >= 120 THEN  MP.DisconnectPower END) AS DisPower
            , SUM(ISNULL( MP.GISDCSubscriberCount, ISNULL(info.DCSubscriberCount , 0))) AS SubscriberCount
            FROM TblMPRequest MP
            INNER JOIN #tmp t ON t.MPRequestId = MP.MPRequestId
            INNER JOIN TblRequestInfo info ON t.RequestId = info.RequestId
            WHERE MP.IsMPTamir = 1 AND ISNULL( MP.GISDCSubscriberCount, ISNULL(info.DCSubscriberCount , 0)) > 0
          GROUP BY MP.AreaId , SUBSTRING(MP.DisconnectDatePersian , 1,4) , SUBSTRING(MP.DisconnectDatePersian , 6,2)
        ) DisMP ON ta.AreaId = DisMP.AreaId
       WHERE ta.IsCenter = 0 AND DisMP.Year IS NOT NULL AND DisMP.Month IS NOT NULL
    ) AS Tbl
        GROUP BY Tbl.AreaId , Tbl.Area, Tbl.Year , Tbl.Month
        ORDER BY Tbl.AreaId ASC , Tbl.Year DESC , Tbl.Month DESC
    DROP TABLE #tmp
  END

--   EXEC spsKerman_part2 @From = '1390/04/30' ,@To = '1400/04/30' 

