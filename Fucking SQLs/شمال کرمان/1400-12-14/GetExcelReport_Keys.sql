SELECT MPP.MPPostName, MPF.MPFeederName
  , K.KeyName, K.MPFeederKeyId , K.GISCode , K.Address , K.IsActive 
  FROM Tbl_MPFeederKey K
  INNER JOIN Tbl_MPFeeder MPF ON K.MPFeederId = MPF.MPFeederId
  INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
  LEFT JOIN Tbl_MPCloserType CT ON K.MPCloserTypeId = CT.MPCloserTypeId
ORDER BY K.IsActive DESC, MPP.MPPostId , MPF.MPFeederId

