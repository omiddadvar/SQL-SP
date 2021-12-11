AND ((TblMPRequest.DisconnectReasonId IS NULL OR TblMPRequest.DisconnectReasonId < 1200 OR TblMPRequest.DisconnectReasonId > 1299 ) 
            AND ( TblMPRequest.DisconnectGroupSetId IS NULL OR TblMPRequest.DisconnectGroupSetId <> 1129 
AND TblMPRequest.DisconnectGroupSetId <> 1130) )
