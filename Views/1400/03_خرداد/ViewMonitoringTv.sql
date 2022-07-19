USE CcRequesterSetad
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[ViewMonitoringTv]') AND OBJECTPROPERTY(id, N'IsView') = 1)
	DROP VIEW dbo.ViewMonitoringTv
GO
CREATE VIEW dbo.ViewMonitoringTv
AS
SELECT 
	tReq.RequestId,
	tReq.RequestNumber, 
	tReq.AreaId, 
	tArea.Area, 
	tReq.ZoneId, 
	tZone.ZoneName, 
	tReq.Address,
	NetworkType = CASE 
		WHEN tReq.IsMPRequest = 1 THEN 'MP' 
		WHEN tReq.IsLPRequest = 1 THEN 'LP' 
		WHEN tReq.IsFogheToziRequest = 1 THEN 'FT' 
		ELSE 'UnKnown' 
	END, 
	tReq.IsTamir, 
	DisconnectDatePersian = CASE tReq.IsTamir WHEN 0 THEN tReq.DisconnectDatePersian ELSE tReq.TamirDisconnectFromDatePersian END, 
	DisconnectTime = CASE tReq.IsTamir WHEN 0 THEN tReq.DisconnectTime ELSE tReq.TamirDisconnectFromTime END, 
	ConnectDatePersian = CASE tReq.IsTamir WHEN 0 THEN tReq.ConnectDatePersian ELSE tReq.TamirDisconnectToDatePersian END, 
	ConnectTime = CASE tReq.IsTamir WHEN 0 THEN tReq.ConnectTime ELSE tReq.TamirDisconnectToTime END, 
	MPPostName = ISNULL(CASE tReq.IsMPRequest WHEN 1 THEN tMPP_MPR.MPPostName ELSE tMPP_LPR.MPPostName END, tMPP_PF.MPPostName), 
	MPFeederName = ISNULL(CASE tReq.IsMPRequest WHEN 1 THEN tMPF_MPR.MPFeederName ELSE tMPF_LPR.MPFeederName END, tMPF_PF.MPFeederName), 
	LPPostName = ISNULL(tLPP.LPPostName, tLPP_PF.LPPostName), 
	LPFeederName = ISNULL(tLPF.LPFeederName, tLPF_PF.LPFeederName), 
	DisconnectGroup = CASE tReq.IsMPRequest WHEN 1 THEN tDG_MPR.DisconnectGroup ELSE tDG_LPR.DisconnectGroup END, 
	DisconnectGroupSet = CASE tReq.IsMPRequest WHEN 1 THEN tDGS_MPR.DisconnectGroupSet ELSE tDGS_LPR.DisconnectGroupSet END, 
	DisconnectReason = CASE tReq.IsMPRequest WHEN 1 THEN tDR_MPR.Description ELSE tDR_LPR.Description END, 
	EkipName = ISNULL(tEKP.EkipProfileName, tMaster.Name), 
	tBz.EzamDatePersian, 
	tBz.EzamTime, 
	tReq.IsLifeEvent, 
	tReq.EndJobStateId AS StatusId,
	Tbl_EndJobState.EndJobState AS Status,
	tReq.SubscriberName
FROM 
	TblRequest tReq
	LEFT OUTER JOIN Tbl_Area tArea ON tReq.AreaId = tArea.AreaId 
	LEFT OUTER JOIN Tbl_Zone tZone ON tReq.ZoneId = tZone.ZoneId
	LEFT OUTER JOIN TblMPRequest tMPR ON tReq.MPRequestId = tMPR.MPRequestId
	LEFT OUTER JOIN TblLPRequest tLPR ON tReq.LPRequestId = tLPR.LPRequestId
	LEFT OUTER JOIN Tbl_MPPost tMPP_MPR ON tMPR.MPPostId = tMPP_MPR.MPPostId 
	LEFT OUTER JOIN Tbl_MPFeeder tMPF_MPR ON tMPR.MPFeederId = tMPF_MPR.MPFeederId 
	LEFT OUTER JOIN Tbl_LPPost tLPP ON tLPR.LPPostId = tLPP.LPPostId 
	LEFT OUTER JOIN Tbl_LPFeeder tLPF ON tLPR.LPFeederId = tLPF.LPFeederId 
	LEFT OUTER JOIN Tbl_MPFeeder tMPF_LPR ON tLPP.MPFeederId = tMPF_LPR.MPFeederId 
	LEFT OUTER JOIN Tbl_MPPost tMPP_LPR ON tMPF_LPR.MPPostId = tMPP_LPR.MPPostId 
	LEFT OUTER JOIN Tbl_DisconnectGroupSet tDGS_MPR ON tMPR.DisconnectGroupSetId = tDGS_MPR.DisconnectGroupSetId
	LEFT OUTER JOIN Tbl_DisconnectGroup tDG_MPR ON tDGS_MPR.DisconnectGroupId = tDG_MPR.DisconnectGroupId
	LEFT OUTER JOIN Tbl_DisconnectGroupSet tDGS_LPR ON tLPR.DisconnectGroupSetId = tDGS_LPR.DisconnectGroupSetId
	LEFT OUTER JOIN Tbl_DisconnectGroup tDG_LPR ON tDGS_LPR.DisconnectGroupId = tDG_LPR.DisconnectGroupId
	INNER JOIN Tbl_EndJobState ON Tbl_EndJobState.EndJobStateId = tReq.EndJobStateId
	LEFT OUTER JOIN 
	(
		SELECT RequestId, Max(BazdidId) BazdidId FROM TblBazdid GROUP BY RequestId
	) tBz1 ON tReq.RequestId = tBz1.RequestId
	LEFT OUTER JOIN TblBazdid tBz ON tBz1.BazdidId = tBz.BazdidId
	LEFT OUTER JOIN TblEkipProfile tEKP ON tBz.EkipProfileId = tEKP.EkipProfileId 
	LEFT OUTER JOIN Tbl_Master tMaster ON tBz.MasterId = tMaster.MasterId 
	LEFT OUTER JOIN Tbl_DisconnectReason tDR_MPR ON tMPR.DisconnectReasonId = tDR_MPR.DisconnectReasonId
	LEFT OUTER JOIN Tbl_DisconnectReason tDR_LPR ON tLPR.DisconnectReasonId = tDR_LPR.DisconnectReasonId
	LEFT OUTER JOIN 
	(
		SELECT RequestId, Max(RequestPostFeederId) RequestPostFeederId FROM TblRequestPostFeeder GROUP BY RequestId
	) tPF ON tReq.RequestId = tPF.RequestId
	LEFt OUTER JOIN TblRequestPostFeeder tPostFeeder ON tPF.RequestPostFeederId = tPostFeeder.RequestPostFeederId
	LEFT OUTER JOIN Tbl_MPPost tMPP_PF ON tPostFeeder.MPPostId = tMPP_PF.MPPostId 
	LEFT OUTER JOIN Tbl_MPFeeder tMPF_PF ON tPostFeeder.MPFeederId = tMPF_PF.MPFeederId 
	LEFT OUTER JOIN Tbl_LPPost tLPP_PF ON tPostFeeder.LPPostId = tLPP_PF.LPPostId 
	LEFT OUTER JOIN Tbl_LPFeeder tLPF_PF ON tPostFeeder.LPFeederId = tLPF_PF.LPFeederId 
WHERE
	tReq.EndJobStateId IN (2,3,4,5) 
	OR tReq.IsLifeEvent = 1
GO
