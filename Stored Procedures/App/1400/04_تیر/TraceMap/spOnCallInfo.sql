USE CCRequesterSetad
GO
CREATE PROCEDURE Homa.spOnCallInfo
  @OnCallId BIGINT
  AS
  BEGIN
  	SELECT O.OnCallId, O.MasterId , O.TabletId , A.AreaId , M.Name AS MasterName, T.TabletName FROM Homa.TblOnCall O
  INNER JOIN Tbl_AreaUser A ON O.AreaUserId = A.AreaUserId
  INNER JOIN Homa.Tbl_Tablet T ON O.TabletId = T.TabletId
  INNER JOIN Tbl_Master M ON O.MasterId = M.MasterId
  WHERE O.OnCallId = @OnCallId
  END