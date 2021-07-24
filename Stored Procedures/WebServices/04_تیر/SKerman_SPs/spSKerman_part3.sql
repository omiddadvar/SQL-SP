USE CcRequesterSetad
GO
CREATE PROCEDURE spSKerman_part3
  AS 
  BEGIN
    SELECT ta.AreaId , ta.Area
      ,ISNULL(TblCnt.CntPublic , 0) AS CntPublic 
      ,ISNULL(TblCnt.CntPublicMP , 0) AS CntPublicMP 
      ,ISNULL(TblCnt.CntLP , 0) AS CntLP
      FROM Tbl_Area ta
      LEFT JOIN (
        SELECT MP.AreaId 
          ,COUNT(DISTINCT MP.MPFeederId) + COUNT(DISTINCT CASE WHEN ISNULL(LP.OwnershipId , 2) = 2 THEN LP.LPFeederId END) AS CntPublic
          ,COUNT(DISTINCT MP.MPFeederId) AS CntPublicMP
          ,COUNT(DISTINCT LP.LPFeederId) AS CntLP
          FROM Tbl_MPFeeder MP
          INNER JOIN Tbl_LPFeeder LP ON MP.AreaId = LP.AreaId
          WHERE ISNULL(MP.OwnershipId , 2) = 2
          GROUP BY MP.AreaId
      )TblCnt ON ta.AreaId = TblCnt.AreaId
      WHERE ta.IsCenter = 0
  END

--   EXEC spsKerman_part3  