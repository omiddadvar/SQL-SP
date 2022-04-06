
--DROP Table __Tbl_Holeilan

--SELECT * FROM __Tbl_Holeilan ORDER BY type

DECLARE @AreaId AS INT = 12

SELECT * INTO __Tbl_Holeilan From (
/*----------------MP Request-----------------*/
  SELECT R.RequestId, R.AreaId , CAST(1 AS BIT) IsMP
      , CASE WHEN R.IsDisconnectMPFeeder = 1 THEN 'MPF'
             WHEN MPR.IsTotalLPPostDisconnected = 1 THEN 'LPP'
             ELSE  NULL END AS Type
    FROM 
      TblRequest R
      INNER JOIN TblMPRequest MPR ON R.MPRequestId = MPR.MPRequestId
      LEFT JOIN Tbl_MPFeeder MPF ON MPR.MPFeederId = MPF.MPFeederId
      LEFT JOIN Tbl_LPPost LPP ON MPR.LPPostId = LPP.LPPostId
    WHERE R.AreaId <> @AreaId 
      AND (
        (R.IsDisconnectMPFeeder = 1 AND MPF.AreaId = @AreaId) OR
        (MPR.IsTotalLPPostDisconnected = 1 AND LPP.AreaId = @AreaId) 
      ) 

UNION
/*----------------LP Request-----------------*/
  SELECT R.RequestId, R.AreaId ,CAST(0 AS BIT) IsMP
      , CASE WHEN LPR.IsTotalLPPostDisconnected = 1 THEN 'LPP'
             ELSE 'LPF' END AS Type
    FROM 
      TblRequest R
      INNER JOIN TblLPRequest LPR ON R.LPRequestId = LPR.LPRequestId
      LEFT JOIN Tbl_LPPost LPP ON LPR.LPPostId = LPP.LPPostId
      LEFT JOIN Tbl_LPFeeder LPF ON LPR.LPFeederId = LPF.LPFeederId
    WHERE R.AreaId <> @AreaId
        AND (
        (LPR.IsTotalLPPostDisconnected = 1 AND LPP.AreaId = @AreaId) OR
        (ISNULL(LPR.IsTotalLPPostDisconnected ,0) = 0 AND LPF.AreaId = @AreaId) 
      ) 
  ) tA


  
/*-----------------Replicate---------------*/

