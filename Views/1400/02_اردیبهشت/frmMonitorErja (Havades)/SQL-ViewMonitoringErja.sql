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
