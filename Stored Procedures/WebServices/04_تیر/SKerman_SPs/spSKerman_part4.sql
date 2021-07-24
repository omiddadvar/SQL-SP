USE CcRequesterSetad
GO
CREATE PROCEDURE spSKerman_part4
  @Year VARCHAR(4),
  @MinDisconnect INT
  AS 
  BEGIN
    SELECT ta.AreaId , ta.Area , COUNT(DISTINCT LP.LPFeederId) + COUNT(DISTINCT MP.MPFeederId) AS Count FROM TblRequest R
      INNER JOIN Tbl_Area ta ON R.AreaId = ta.AreaId
      LEFT JOIN TblMPRequest MP ON R.MPRequestId = MP.MPRequestId
      LEFT JOIN TblLPRequest LP ON R.LPRequestId = LP.LPRequestId
      INNER JOIN Tbl_MPFeeder MPFeeder ON MP.MPFeederId = MPFeeder.MPFeederId
      INNER JOIN Tbl_LPFeeder LPFeeder ON LP.LPFeederId = LPFeeder.LPFeederId
      WHERE LEFT(R.DisconnectDatePersian , 4) = @Year
        AND R.IsTamir = 0 AND R.DisconnectInterval >= 5
        AND ISNULL(LPFeeder.OwnershipId, 2) = 2 AND ISNULL(MPFeeder.OwnershipId, 2) = 2 
      GROUP BY ta.AreaId , ta.Area
      HAVING COUNT(R.RequestId) >= @MinDisconnect
  END

--   EXEC spsKerman_part4 '1390' , 100
