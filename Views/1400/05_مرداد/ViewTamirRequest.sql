ALTER VIEW [dbo].[ViewTamirRequest] 
AS 
SELECT 
	tTR.TamirRequestId, 
	tTR.TamirRequestNo, 
	ISNULL(tTRC.RequestId, tTR.EmergencyRequestId) As RequestId, 
	tTR.AreaId, 
	Tbl_Area.Server121Id, 
	Tbl_Server121.Server121, 
	Cast(isnull(tTRC.IsConfirm,0) As bit) As IsConfirm, 
	tTR.CityId, 
	tTR.MPPostId, 
	tTR.MPFeederId, 
	tTR.MPCloserTypeId, 
	tTR.AreaUserId, 
	Tbl_AreaUser.UserName As RequestUserName, 
	Tbl_AreaUser.AreaId As RequestUserAreaId, 
	tTRC.AreaUserId As ConfirmAreaUserId, 
	Tbl_AreaUserC.UserName As ConfirmUserName, 
	tTRA.AreaUserId As AllowAreaUserId, 
	Tbl_AreaUserA.UserName As AllowUserName, 
	tTR.ManoeuvreTypeId, 
	tTR.ManoeuvreMPPostId, 
	tTR.ManoeuvreMPFeederId, 
	Tbl_Area.Area, 
	Tbl_City.City, 
	Tbl_MPPost.MPPostName, 
	Tbl_MPFeeder.MPFeederName, 
	Tbl_MPCloserType.MPCloserType, 
	Tbl_AreaUser.UserName, 
	Tbl_ManoeuvreType.ManoeuvreType, 
	Tbl_MPPost_Manoeuvre.MPPostName AS MPPostNameManoeuvre, 
	Tbl_MPFeeder_Manoeuvre.MPFeederName AS MPFeederNameManoeuvre, 
	tTR.Peymankar, 
	tTR.DisconnectDT, 
	tTR.DisconnectDatePersian, 
	tTR.DisconnectTime, 
	tTR.ConnectDT, 
	tTR.ConnectDatePersian, 
	tTR.ConnectTime, 
	(ISNULL(tTR.DisconnectInterval,0) + ISNULL(tTRD.DisconnectInterval,0)) AS DisconnectInterval, 
	tTR.LastMPFeederPeak, 
	tTR.CurrentValue, 
	(ISNULL(tTR.DisconnectPower,0) + ISNULL(tTRD.DisconnectPower,0)) AS DisconnectPower, 
	tTR.CriticalLocations, 
	tTR.CriticalsAddress, 
	tTR.DEDT, 
	tTR.DEDatePersian, 
	tTR.DETime, 
	tTRC.ConfirmDT, 
	tTRC.ConfirmDatePersian, 
	tTRC.ConfirmTime, 
	tTRC.UnConfirmReason, 
	tTR.IsManoeuvre, 
	tBazdid.AllowNumber, 
	tBazdid.AllowDT, 
	tBazdid.AllowDatePersian, 
	tBazdid.AllowTime, 
	tBazdid.AllowEndDT, 
	tBazdid.AllowEndDatePersian, 
	tTRA.AllowEndTime, 
	tTR.WeatherId, 
	tTR.IsWarmLine, 
	tTR.SaturdayDT, 
	tTR.SaturdayDatePersian, 
	tTR.MaximumPower, 
	tTR.TamirRequestStateId, 
	Tbl_TamirRequestState.TamirRequestState, 
	ISNULL(tReq.RequestNumber, tReqE.RequestNumber) As RequestNumber, 
	tTR.ConfirmNahiehAreaUserId, 
	tUsrN.UserName As ConfirmNahiehUserName, 
	tTR.ConfirmCenterAreaUserId, 
	tUsrC.UserName As ConfirmCenterUserName,
	ISNULL(tTR.IsEmergency,0) As IsEmergency, 
	tTR.LPPostId, 
	Tbl_LPPost.LPPostName, 
	tTR.IsSpecialConfirm,
	(tTr.SendCenterDatePersian + ' ' + SendCenterTime) As SendToCenterDateTime,
	(tTr.SendSetadDatePersian + ' ' + SendSetadTime) As SendToSetadDateTime,
	(tTR.DEDatePersian + ' ' + tTR.DETime) As SendDEDateTime, 
	(tTRC.ConfirmDatePersian + ' ' + tTRC.ConfirmTime) As ConfirmDateTime, 
	tFP.FeederPartId, 
	tFP.FeederPart, 
	tTR.IsRequestByPeymankar, 
	tTR.IsInCityService, 
	tTR.TamirNetworkTypeId, 
	Tbl_TamirNetworkType.TamirNetworkType, 
	tTR.LPFeederId, 
	Tbl_LPFeeder.LPFeederName, 
	tTR.TamirTypeId, 
	Tbl_TamirType.TamirType, 
  (ISNULL(tTR.ReturnDatePersian, '') + ' ' + ISNULL(tTR.ReturnTime, '')) AS ReturnDateTime,
	ISNULL(tTR.IsReturned,0) AS IsReturned, 
	tTR.ReturnDesc,
	(1 + ISNULL(tTRD.DCCount,0)) AS DCCount,
	tTR.WarmLineConfirmStateId,
	WarmLineConfirmState = CASE 
		WHEN NULLIF(tTR.WarmLineConfirmStateId,0) IS NULL THEN '-'
		ELSE WarmLineConfirmState
	END,
	DelayInterval = CASE
		WHEN tTR.DisconnectInterval >= ISNULL(tReq.DisconnectInterval, tTR.DisconnectInterval) THEN 0
		ELSE ISNULL(tReq.DisconnectInterval, tTR.DisconnectInterval) - tTR.DisconnectInterval
	END
FROM 
	TblTamirRequest tTR 
	LEFT OUTER JOIN TblTamirRequestConfirm tTRC ON tTR.TamirRequestId = tTRC.TamirRequestId 
	LEFT OUTER JOIN TblTamirRequestAllow tTRA ON tTR.TamirRequestId = tTRA.TamirRequestId 
	LEFT OUTER JOIN Tbl_MPFeeder As Tbl_MPFeeder_Manoeuvre ON tTR.ManoeuvreMPFeederId = Tbl_MPFeeder_Manoeuvre.MPFeederId 
	LEFT OUTER JOIN Tbl_MPPost As Tbl_MPPost_Manoeuvre ON tTR.ManoeuvreMPPostId = Tbl_MPPost_Manoeuvre.MPPostId 
	LEFT OUTER JOIN Tbl_ManoeuvreType ON tTR.ManoeuvreTypeId = Tbl_ManoeuvreType.ManoeuvreTypeId 
	LEFT OUTER JOIN Tbl_AreaUser ON tTR.AreaUserId = Tbl_AreaUser.AreaUserId 
	LEFT OUTER JOIN Tbl_AreaUser Tbl_AreaUserC ON tTRC.AreaUserId = Tbl_AreaUserC.AreaUserId 
	LEFT OUTER JOIN Tbl_AreaUser Tbl_AreaUserA ON tTRA.AreaUserId = Tbl_AreaUserA.AreaUserId 
	LEFT OUTER JOIN Tbl_Area ON tTR.AreaId = Tbl_Area.AreaId 
	LEFT OUTER JOIN Tbl_Server121 ON Tbl_Area.Server121Id = Tbl_Server121.Server121Id 
	LEFT OUTER JOIN Tbl_MPCloserType ON tTR.MPCloserTypeId = Tbl_MPCloserType.MPCloserTypeId 
	LEFT OUTER JOIN Tbl_MPFeeder ON tTR.MPFeederId = Tbl_MPFeeder.MPFeederId 
	LEFT OUTER JOIN Tbl_MPPost ON tTR.MPPostId = Tbl_MPPost.MPPostId 
	LEFT OUTER JOIN Tbl_City ON tTR.CityId = Tbl_City.CityId 
	LEFT OUTER JOIN Tbl_TamirRequestState ON tTR.TamirRequestStateId = Tbl_TamirRequestState.TamirRequestStateId 
	LEFT OUTER JOIN TblRequest tReq ON tTRC.RequestId = tReq.RequestId
	LEFT OUTER JOIN Tbl_AreaUser tUsrN ON tTR.ConfirmNahiehAreaUserId = tUsrN.AreaUserId 
	LEFT OUTER JOIN Tbl_AreaUser tUsrC ON tTR.ConfirmCenterAreaUserId = tUsrC.AreaUserId 
	LEFT OUTER JOIN Tbl_LPPost ON tTR.LPPostId = Tbl_LPPost.LPPostId 
	LEFT OUTER JOIN TblRequest tReqE ON tTR.EmergencyRequestId = tReqE.RequestId
	LEFT OUTER JOIN Tbl_FeederPart tFP ON tTR.FeederPartId = tFP.FeederPartId
	LEFT OUTER JOIN Tbl_TamirNetworkType ON tTR.TamirNetworkTypeId = Tbl_TamirNetworkType.TamirNetworkTypeId
	LEFT OUTER JOIN Tbl_LPFeeder ON tTR.LPFeederId = Tbl_LPFeeder.LPFeederId 
	LEFT OUTER JOIN Tbl_TamirType ON tTR.TamirTypeId = Tbl_TamirType.TamirTypeId
	LEFT OUTER JOIN Tbl_WarmLineConfirmState tWLC ON tTR.WarmLineConfirmStateId = tWLC.WarmLineConfirmStateId
	LEFT OUTER JOIN 
	(
		SELECT 
			TamirRequestId, 
			Sum(DisconnectPower) DisconnectPower, 
			Sum(DisconnectInterval) DisconnectInterval, 
			Count(TamirRequestDisconnectId) DCCount 
		FROM TblTamirRequestDisconnect 
		GROUP BY TamirRequestId
	) tTRD ON tTR.TamirRequestId = tTRD.TamirRequestId
	LEFT OUTER JOIN (SELECT RequestId, MAX(BazdidId) AS BazdidId FROM TblBazdid GROUP BY RequestId ) tBz1 ON tBz1.RequestId = ISNULL(tReq.RequestId, tReqE.RequestId)
	LEFT JOIN
	(
		SELECT 
			RequestId,
			BazdidId,
			ALLowNumber, 
			AllowDT, 
			AllowDatePersian, 
			AllowTime, 
			AllowEndDT, 
			AllowEndDatePersian, 
			AllowEndTime
		FROM
			TblBazdid
	) AS tBazdid ON tBz1.BazdidId = tBazdid.BazdidId
GO
