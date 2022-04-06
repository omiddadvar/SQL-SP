
DECLARE @AreaId AS INT = 10
/*----------------MP Request-----------------*/
SELECT TOP(100) R.RequestId
    , CASE WHEN R.IsDisconnectMPFeeder = 1 THEN MPF.AreaId
           WHEN MPR.IsTotalLPPostDisconnected = 1 THEN LPP.AreaId
           ELSE  NULL END AS AreaId
    , CASE WHEN R.IsDisconnectMPFeeder = 1 THEN 'MPF'
           WHEN MPR.IsTotalLPPostDisconnected = 1 THEN 'LPP'
           ELSE  NULL END AS Type
  FROM TblRequest R
  INNER JOIN TblMPRequest MPR ON R.MPRequestId = MPR.MPRequestId
  LEFT JOIN Tbl_MPFeeder MPF ON MPR.MPFeederId = MPF.MPFeederId
  LEFT JOIN Tbl_LPPost LPP ON MPR.LPPostId = LPP.LPPostId
  WHERE R.IsMPRequest = 1 AND R.MPRequestId IS NOT NULL
    AND (R.IsDisconnectMPFeeder = 1 OR MPR.IsTotalLPPostDisconnected = 1)
    AND ISNULL(MPF.AreaId , @AreaId) = @AreaId 
    AND ISNULL(LPP.AreaId , @AreaId) = @AreaId


/*----------------LP Request-----------------*/
SELECT  TOP(100) R.RequestId
    , CASE WHEN LPR.IsTotalLPPostDisconnected = 1 THEN LPP.AreaId
           WHEN LPR.IsTotalLPFeederDisconnected = 1 THEN LPF.AreaId
           ELSE  NULL END AS AreaId
    , CASE WHEN LPR.IsTotalLPPostDisconnected = 1 THEN 'LPP'
           WHEN LPR.IsTotalLPFeederDisconnected = 1 THEN 'LPF'
           ELSE  NULL END AS Type
   FROM TblRequest R
  INNER JOIN TblLPRequest LPR ON R.LPRequestId = LPR.LPRequestId
  LEFT JOIN Tbl_LPPost LPP ON LPR.LPPostId = LPP.LPPostId
  LEFT JOIN Tbl_LPFeeder LPF ON LPR.LPFeederId = LPF.LPFeederId
  WHERE R.IsLPRequest = 1 AND R.LPRequestId IS NOT NULL
    AND (LPR.IsTotalLPPostDisconnected = 1 OR LPR.IsTotalLPFeederDisconnected = 1)
    AND ISNULL(LPP.AreaId , @AreaId) = @AreaId
    AND ISNULL(LPF.AreaId , @AreaId) = @AreaId 
