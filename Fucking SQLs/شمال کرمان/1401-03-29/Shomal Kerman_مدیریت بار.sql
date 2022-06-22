USE [CCRequesterSetad]
GO
/****** Object:  StoredProcedure [dbo].[spRayaGetTamirOutageByAreaDate]    Script Date: 2022/06/19 01:02:59 ب.ظ ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[spRayaGetTamirOutageByAreaDate] 
	@aAreaId int,
	@aFromDate varchar(10), 
	@aToDate varchar(10)
AS
	
	SELECT *, CAST(NULL as nvarchar(4000)) AS DisconnectGroupSet, CAST(NULL as nvarchar(200)) AS UserName INTO #tmpReq FROM TblRequest WHERE RequestId = -1

	SELECT tReq.*,
		DisconnectGroupSet = ISNULL(dbo.GetTamirOperationsByRequestId(tReq.RequestId), Tbl_DisconnectLPRequestFor.DisconnectLPRequestFor),
		Tbl_AreaUser.UserName
	into #tmp
	FROM 
		TblRequest tReq with (index(TblRequest52tza)) 
		INNER JOIN Tbl_AreaUser ON tReq.AreaUserId = Tbl_AreaUser.AreaUserId
		left JOIN Tbl_DisconnectLPRequestFor ON tReq.DisconnectRequestForId = Tbl_DisconnectLPRequestFor.DisconnectLPRequestForId
	WHERE 
		tReq.DisconnectDatePersian >= @aFromDate
		AND tReq.DisconnectDatePersian <= @aToDate
		AND tReq.EndJobStateId IN (4,5)
		AND tReq.AreaId = @aAreaId
		AND tReq.IsTamir = 1
		AND (tReq.MPRequestId IS NULL AND tReq.LPRequestId IS NULL AND tReq.FogheToziDisconnectId IS NULL)

	delete from #tmp
		where RequestId in 
		(	SELECT #tmp.RequestId
	FROM 
		#tmp
		inner JOIN TblTamirRequestConfirm ON #tmp.RequestId = TblTamirRequestConfirm.RequestId
		inner JOIN TblTamirRequest ON TblTamirRequestConfirm.TamirRequestId = TblTamirRequest.TamirRequestId
	WHERE 
		ISNULL(TblTamirRequest.IsWarmLine,0) = 1)
		
	INSERT INTO #tmpReq
	SELECT #tmp.*
	FROM 
		#tmp
	
		
	UNION

	SELECT tReq.*,
		DisconnectGroupSet = 
			CASE WHEN Tbl_FogheTozi.FogheToziId = 1 THEN 'مديريت بار'
			ELSE ISNULL(isnull(Tbl_FogheTozi.FogheToz, isnull(Tbl_CallReason.CallReason, dbo.GetTamirOperationsByRequestId(tReq.RequestId))),'مديريت بار') END,
		Tbl_AreaUser.UserName 
	FROM 
		TblRequest tReq  with (index(TblRequest52tza)) 
		INNER JOIN TblFogheToziDisconnect tReqFT  ON tReq.FogheToziDisconnectId = tReqFT.FogheToziDisconnectId
		LEFT JOIN Tbl_FogheTozi ON tReqFT.FogheToziId = Tbl_FogheTozi.FogheToziId
		INNER JOIN Tbl_AreaUser ON tReq.AreaUserId = Tbl_AreaUser.AreaUserId
		LEFT JOIN Tbl_CallReason ON tReq.CallReasonId = Tbl_CallReason.CallReasonId
	WHERE 
		tReq.DisconnectDatePersian >= @aFromDate
		AND tReq.DisconnectDatePersian <= @aToDate
		AND tReq.EndJobStateId IN (4,5)
		AND tReq.AreaId = @aAreaId
		AND (tReq.DisconnectDT > GETDATE())
		

	UPDATE 
		#tmpReq
	SET 
		TamirDisconnectFromDT = tReqFTMPF.DisconnectDT,
		DisconnectDT = tReqFTMPF.DisconnectDT,
		TamirDisconnectToDT = ISNULL(tReqFTMPF.ConnectDT,#tmpReq.TamirDisconnectToDT),
		DisconnectTime = tReqFTMPF.DisconnectTime,
		DisconnectDatePersian = tReqFTMPF.DisconnectDatePersian
	From	
		#tmpReq
		INNER JOIN TblFogheToziDisconnect tReqFT  ON #tmpReq.FogheToziDisconnectId = tReqFT.FogheToziDisconnectId
		INNER JOIN TblFogheToziDisconnectMPFeeder tReqFTMPF ON tReqFT.FogheToziDisconnectId = tReqFTMPF.FogheToziDisconnectId
		
		
	UPDATE
		#tmpReq
	SET 
		DisconnectDT = TamirDisconnectFromDT
	WHERE
		EndJobStateId = 4
		AND isnull(TamirDisconnectFromDT,'') <> ''
		
	UPDATE #tmpReq
	SET 
		DisconnectDatePersian = CASE WHEN ISNULL(DisconnectDatePersian,'') = '' THEN TamirDisconnectFromDatePersian ELSE DisconnectDatePersian END,
		DisconnectTime = CASE WHEN ISNULL(DisconnectTime,'') = '' THEN TamirDisconnectFromTime ELSE DisconnectTime END,
		ConnectDatePersian = dbo.mtosh(DATEADD(minute,datediff(minute,TamirDisconnectFromDT,TamirDisconnectToDT),#tmpReq.DisconnectDT)),
		ConnectTime = dbo.GetTime(DATEADD(minute,datediff(minute,TamirDisconnectFromDT,TamirDisconnectToDT),#tmpReq.DisconnectDT))
	WHERE
		IsTamir = 1
		
	UPDATE #tmpReq
	SET 
		DisconnectGroupSet = N'مديريت بار'
	where
		isnull(DisconnectGroupSet,'') = ''


	UPDATE #tmpReq
	SET 
		Address = LEFT(ISNULL(CriticalsAddress, #tmpReq.Address),200)
	FROM
		#tmpReq
		INNER JOIN TblTamirRequestConfirm ON #tmpReq.RequestId = TblTamirRequestConfirm.RequestId
		INNER JOIN TblTamirRequest ON TblTamirRequestConfirm.TamirRequestId = TblTamirRequest.TamirRequestId
			
	UPDATE #tmpReq
	SET 
		Address = LEFT(ISNULL(CriticalsAddress, #tmpReq.Address),200)
	FROM
		#tmpReq
		INNER JOIN TblTamirRequest ON #tmpReq.RequestId = TblTamirRequest.EmergencyRequestId
		
	SELECT DISTINCT * FROM #tmpReq ORDER BY DisconnectDatePersian, DisconnectTime
	DROP TABLE #tmpReq
	drop table #tmp

