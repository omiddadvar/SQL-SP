DECLARE @AreaId AS INT = 12

  INSERT INTO __Tbl_Holeilan
  SELECT R.RequestId, R.AreaId , CAST(1 AS BIT) IsMP ,'PMP' AS Type
    FROM 
      TblRequest R
      INNER JOIN TblMPRequest MPR ON R.MPRequestId = MPR.MPRequestId
      LEFT JOIN Tbl_MPFeeder MPF ON MPR.MPFeederId = MPF.MPFeederId
      LEFT JOIN Tbl_LPPost LPP ON MPR.LPPostId = LPP.LPPostId
    WHERE R.AreaId <> @AreaId 
      AND (MPR.IsNotDisconnectFeeder = 1 AND MPF.AreaId = @AreaId) 

