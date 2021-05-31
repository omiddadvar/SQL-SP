USE [CcRequesterSetad]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ViewMonitoringErja]') and OBJECTPROPERTY(id, N'IsView') = 1)
	DROP VIEW [dbo].[ViewMonitoringErja]
GO

CREATE VIEW [dbo].[ViewMonitoringErja]
AS
	SELECT 
		tEReq.ErjaRequestId, 
		tEReq.ReferToId, 
		tEReq.NetworkTypeId, 
		tEReq.ErjaReasonId, 
		tEReq.RequestId, 
		tEReq.ErjaStateId, 
		TblRequest.AreaId, 
		Tbl_AreaUser_Done.AreaId AS DoneAreaId, 
		TblRequest.RequestNumber, 
		Tbl_ErjaState.ErjaState, 
		tRef.ReferTo, 
		tEReq.ErjaDT, 
		tEReq.ErjaDatePersian, 
		tEReq.ErjaTime, 
		tEReq.EndDT, 
		tEReq.EndDatePersian, 
		tEReq.EndTime, 
		CAST(tEReq.ErjaInterval / 60 AS varchar(5)) + ':' + CAST(tEReq.ErjaInterval % 60 AS varchar(2)) AS ErjaInterval, 
		Tbl_Area.Area, 
		tNTT.NetworkType, 
		Tbl_ErjaReason.ErjaReason, 
		Tbl_Area_Done.Area AS DoneArea, 
		TblBazdidErja.BazdidErjaId, 
		TblBazdidErja.EzamDatePersian, 
		TblBazdidErja.EzamTime, 
		Tbl_Master.Name AS MasterName, 
		tEReq.IsWatched, 
		TblRequest.Address,
		ISNULL(TblRequestInfo.IsDCErja,0) AS IsDCErja,
		ISNULL(tEReq.FileNo, tSub.FileNo) AS FileNo,
		TblRequest.DataEntryDTPersian AS DataEntryDatePersian,
		TblRequestInfo.RamzCode,
		TblRequest.Telephone,
		TblRequest.Mobile,
		TblRequest.SubscriberName,
		-----------------Newly Added By Omid 
		ISNULL(Tbl_MPPost.MPPostName,'') AS MPPostName,
		ISNULL(Tbl_MPFeeder.MPFeederName,'') AS MPFeederName,
		ISNULL(Tbl_LPFeeder.LPFeederCode,'') AS LPFeederCode,
		ISNULL(Tbl_AreaUser.UserName,'') AS UserName,
		ISNULL(ISNULL((tMPR.RequestDEInterval - (tMPR.DisconnectInterval + ISNULL(TblRequest.OverlapTime,0))),
		 (tLPR.RequestDEInterval - tLPR.DisconnectInterval)),0) AS DelayInterval
	FROM 
		TblErjaRequest tEReq 
		LEFT OUTER JOIN TblRequest ON tEReq.RequestId  = TblRequest.RequestId 
		LEFT OUTER JOIN TblRequestInfo ON TblRequest.RequestId  = TblRequestInfo.RequestId 
		LEFT OUTER JOIN TblBazdidErja ON tEReq.ErjaRequestId  = TblBazdidErja.ErjaRequestId 
		LEFT OUTER JOIN Tbl_ErjaReason ON tEReq.ErjaReasonId = Tbl_ErjaReason.ErjaReasonId 
		LEFT OUTER JOIN Tbl_ErjaState ON tEReq.ErjaStateId = Tbl_ErjaState.ErjaStateId  
		LEFT OUTER JOIN Tbl_AreaUser Tbl_AreaUser_Done ON tEReq.DoneAreaUserId = Tbl_AreaUser_Done.AreaUserId 
		LEFT OUTER JOIN Tbl_AreaUser ON tEReq.CreateAreaUserId = Tbl_AreaUser.AreaUserId 
		LEFT OUTER JOIN Tbl_NetworkType tNTT ON tEReq.NetworkTypeId = tNTT.NetworkTypeId 
		LEFT OUTER JOIN Tbl_ReferTo tRef ON tEReq.ReferToId = tRef.ReferToId
		LEFT OUTER JOIN Tbl_Area Tbl_Area_Done ON Tbl_AreaUser_Done.AreaId = Tbl_Area_Done.AreaId 
		LEFT OUTER JOIN Tbl_Master ON TblBazdidErja.MasterId = Tbl_Master.MasterId 
		LEFT OUTER JOIN Tbl_Area ON TblRequest.AreaId = Tbl_Area.AreaId
		LEFT OUTER JOIN Tbl_Subscriber tSub ON TblRequest.SubscriberId = tSub.SubscriberId
		-----------------Newly Added By Omid 
		LEFT JOIN TblMPRequest tMPR ON TblRequest.MPRequestId = tMPR.MPRequestId
		LEFT JOIN TblLPRequest tLPR ON TblRequest.LPRequestId = tLPR.LPRequestId
		LEFT JOIN Tbl_MPPost ON tMPR.MPPostId = Tbl_MPPost.MPPostId
		LEFT JOIN Tbl_MPFeeder ON tMPR.MPFeederId = Tbl_MPFeeder.MPFeederId
		LEFT JOIN Tbl_LPFeeder ON tLPR.LPFeederId = Tbl_LPFeeder.LPFeederId
GO

----Test:
--select top(10) * from [dbo].[ViewMonitoringErja] order by MPPostName desc
GO
--------------------------------------------------------------------------------------------------------
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