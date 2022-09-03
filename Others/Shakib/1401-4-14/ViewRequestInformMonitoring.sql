CREATE VIEW [dbo].[ViewRequestInformMonitoring]
AS 
SELECT 
	TblRequest.RequestId, 
	TblRequest.AreaId, 
	ISNULL(TblRequestInform.RequestInformJobStateId, 1) AS RequestInformJobStateId, 
	ISNULL(Tbl_RequestInformJobState.RequestInformJobState, 'ÌÏíÏ') AS RequestInformJobState, 
	TblRequest.IsTamir, 
	TblRequest.DisconnectDatePersian, 
	DisconnectTime = 
		Case TblRequest.isTamir 
			when 1 then TblRequest.TamirDisconnectFromTime 
			else TblRequest.DisconnectTime 
		end, 
	DCInterval = 
		Case TblRequest.isTamir 
			when 1 then CAST( datediff(mi, TblRequest.TamirDisconnectFromDT, TblRequest.TamirDisconnectToDT) AS nvarchar(100) ) 
			else CAST(datediff(mi, Getdate(), TblMPRequest.EstimateDT) AS nvarchar(100)) 
		end, 
	TblRequestInform.RequestInformId, 
	TblRequest.EndJobStateId, 
	TblRequest.RequestNumber, 
	TblRequest.Address, 
	TblRequest.ExtraComments, 
	Tbl_Area.Area,
	TblMPRequest.MPFeederId,
	Tbl_MPFeeder.MPFeederName,
	Tbl_MPPost.MPPostId,
	Tbl_MPPost.MPPostName,
	ISNULL(TblMPRequest.LPPostId,TblLPRequest.LPPostId) AS LPPostId,
	ISNULL(tMPLPPost.LPPostName,tLPLPPost.LPPostName) AS LPPostName,
	TblLPRequest.LPFeederId,
	Tbl_LPFeeder.LPFeederName
FROM 
	TblRequest 
	LEFT OUTER JOIN TblMPRequest ON TblRequest.MPRequestId = TblMPRequest.MPRequestId 
	LEFT OUTER JOIN TblLPRequest ON TblRequest.LPRequestId = TblLPRequest.LPRequestId 
	LEFT OUTER JOIN TblRequestInform ON TblRequest.RequestId = TblRequestInform.RequestId 
	LEFT OUTER JOIN Tbl_RequestInformJobState ON TblRequestInform.RequestInformJobStateId = Tbl_RequestInformJobState.RequestInformJobStateId 
	LEFT OUTER JOIN Tbl_Area ON TblRequest.AreaId = Tbl_Area.AreaId 
	LEFT JOIN Tbl_MPFeeder ON TblMPRequest.MPFeederId = Tbl_MPFeeder.MPFeederId
	LEFT JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId
	LEFT JOIN TblTamirRequestConfirm ON TblRequest.RequestId = TblTamirRequestConfirm.RequestId
	LEFT JOIN TblTamirRequest ON TblTamirRequestConfirm.TamirRequestId = TblTamirRequest.TamirRequestId 
	LEFT JOIN TblTamirRequest TblTamirRequest2 ON TblRequest.RequestId = TblTamirRequest2.EmergencyRequestId
	LEFT JOIN Tbl_LPPost tMPLPPost on TblMPRequest.LPPostId = tMPLPPost.LPPostId
	LEFT JOIN Tbl_LPPost tLPLPPost on TblLPRequest.LPPostId = tLPLPPost.LPPostId
	LEFT JOIN Tbl_LPFeeder on TblLPRequest.LPFeederId = Tbl_LPFeeder.LPFeederId
WHERE 
	 /*( 
		(TblRequest.IsTamir = 1 AND TblRequest.IsLPRequest = 0) 
		OR (TblRequest.IsTamir = 0 AND TblRequest.IsMPRequest = 1) 
	) 
	AND*/ ( NOT TblRequest.EndJobStateId IN (9,7,1,8,6))
	AND ISNULL(TblMPRequest.IsWarmLine,0) = 0
	AND ISNULL(TblTamirRequest.IsWarmLine,0) = 0


GO


