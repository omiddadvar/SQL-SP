
ALTER procedure dbo.spSimaSaidi (@FromDate varchar(10), @ToDate varchar(10), @AreaIds varchar(1000) )
as 
	create table #tblArea (AreaId int)
	declare @lSQL as varchar(2000) = 'select AreaId from tbl_Area'
	if @AreaIds <> ''
		set @lSQL = @lSQL + ' where AreaId in (' + @AreaIds + ')'

	insert into #tblArea exec (@lSQL) 

	select RequestId, #tblArea.AreaId, IsTamir,MPRequestId, LPRequestId, TamirTypeId
	into #tmpReq
	from 
		TblRequest with(index(IX_MonitoringIndex))
		inner join #tblArea on TblRequest.AreaId = #tblArea.AreaId
	where 
		TblRequest.DisconnectDatePersian >= @FromDate
		AND TblRequest.DisconnectDatePersian <=  @ToDate
		AND EndJobStateId in (2,3)
		AND ISNULL(IsLightRequest,0) = 0

	SELECT t1.AreaId,
		ISNULL(sum(DisconnectPowerMP),0) AS SumPowerMP,
		ISNULL(sum(DisconnectPowerLP),0) AS SumPowerLP,
		ISNULL(SUM(DisconnectPowerTamir),0) AS SumPowerMPTamir,
		ISNULL(SUM(DisconnectPowerNotTamirPaydar),0) AS SumPowerMPNotTamirPaydar,
		ISNULL(SUM(DisconnectPowerNotTamirGozara),0) AS SumPowerMPNotTamirGozara,
		ISNULL(SUM(DisconnectPowerTamirBarnameh),0) AS SumPowerMPBarnameh,
		ISNULL(SUM(DisconnectPowerTamirMovafegh),0) AS SumPowerMPMovafegh,
		ISNULL(SUM(DisconnectPowerTamirEzterar),0) AS SumPowerMPEzterar,
		ISNULL(SUM(DisconnectPowerTamirUnknown),0) as SumPowerMPUnknown,
		ISNULL(SUM(cntDCMP),0) as cntDCMP,
		ISNULL(SUM(cntDCMPPaydar),0) as cntDCMPPaydar,
		ISNULL(SUM(cntDCMPGozara),0) as cntDCMPGozara,
		ISNULL(SUM(cntDCMPBarnameh),0) as cntDCMPBarnameh,
		ISNULL(SUM(cntDCMPMovafegh),0) as cntDCMPMovafegh,
		ISNULL(SUM(cntDCMPEzterar),0) as cntDCMPEzterar,
		ISNULL(SUM(cntDCMPUnknown),0) as cntDCMPUnknown,
		ISNULL(sum(DisconnectPowerLPTamir),0) AS SumPowerLPTamir,
		ISNULL(sum(DisconnectPowerLPNotTamir),0) AS SumPowerLPNotTamir
	FROM (
		SELECT #tmpReq.AreaId,
			0 AS DisconnectPowerLP,
			SUM(TblMPRequest.DisconnectPower) AS DisconnectPowerMP,
			SUM(CASE WHEN #tmpReq.IsTamir = 1 THEN TblMPRequest.DisconnectPower END) AS DisconnectPowerTamir,
			SUM(CASE WHEN #tmpReq.IsTamir = 0 AND TblMPRequest.DisconnectInterval > 5 THEN TblMPRequest.DisconnectPower END) AS DisconnectPowerNotTamirPaydar,
			SUM(CASE WHEN #tmpReq.IsTamir = 0 AND TblMPRequest.DisconnectInterval <= 5 THEN TblMPRequest.DisconnectPower END) AS DisconnectPowerNotTamirGozara,
			SUM(CASE WHEN #tmpReq.IsTamir = 1 AND #tmpReq.TamirTypeId = 1 THEN TblMPRequest.DisconnectPower END) AS DisconnectPowerTamirBarnameh,
			SUM(CASE WHEN #tmpReq.IsTamir = 1 AND #tmpReq.TamirTypeId = 2 THEN TblMPRequest.DisconnectPower END) AS DisconnectPowerTamirMovafegh,
			SUM(CASE WHEN #tmpReq.IsTamir = 1 AND #tmpReq.TamirTypeId = 3 THEN TblMPRequest.DisconnectPower END) AS DisconnectPowerTamirEzterar,
			SUM(CASE WHEN #tmpReq.IsTamir = 1 AND #tmpReq.TamirTypeId is null THEN TblMPRequest.DisconnectPower END) AS DisconnectPowerTamirUnknown,
			count(#tmpReq.RequestId) AS cntDCMP,
			SUM(CASE WHEN #tmpReq.IsTamir = 0 AND TblMPRequest.DisconnectInterval > 5 THEN 1 END) AS cntDCMPPaydar,
			SUM(CASE WHEN #tmpReq.IsTamir = 0 AND TblMPRequest.DisconnectInterval <= 5 THEN 1 END) AS cntDCMPGozara,
			SUM(CASE WHEN #tmpReq.IsTamir = 1 AND #tmpReq.TamirTypeId = 1 THEN 1 END) AS cntDCMPBarnameh,
			SUM(CASE WHEN #tmpReq.IsTamir = 1 AND #tmpReq.TamirTypeId = 2 THEN 1 END) AS cntDCMPMovafegh,
			SUM(CASE WHEN #tmpReq.IsTamir = 1 AND #tmpReq.TamirTypeId = 3 THEN 1 END) AS cntDCMPEzterar,
			SUM(CASE WHEN #tmpReq.IsTamir = 1 AND #tmpReq.TamirTypeId is null THEN 1 END) AS cntDCMPUnknown,
			0 as DisconnectPowerLPTamir,
			0 as DisconnectPowerLPNotTamir
			
		FROM #tmpReq
		INNER JOIN TblMPRequest ON #tmpReq.MPRequestId = TblMPRequest.MPRequestId
		INNER JOIN Tbl_DisconnectGroupSet ON TblMPRequest.DisconnectGroupSetId = Tbl_DisconnectGroupSet.DisconnectGroupSetId
		INNER JOIN Tbl_Area ON #tmpReq.AreaId = Tbl_Area.AreaId
		LEFT JOIN Tbl_MPFeeder ON TblMPRequest.MPFeederId = Tbl_MPFeeder.MPFeederId
		WHERE 
			
			ISNULL(TblMPRequest.IsWarmLine, 0) = 0
			AND (
				(
					TblMPRequest.DisconnectReasonId IS NULL
					OR TblMPRequest.DisconnectReasonId < 1200
					OR TblMPRequest.DisconnectReasonId > 1299
					)
				AND (
					TblMPRequest.DisconnectGroupSetId IS NULL
					OR TblMPRequest.DisconnectGroupSetId <> 1129
					AND TblMPRequest.DisconnectGroupSetId <> 1130
					)
				)
			AND TblMPRequest.EndJobStateId IN (2, 3)
			AND ISNULL(Tbl_MPFeeder.OwnershipId,2) = 2
		GROUP BY
			#tmpReq.AreaId
			
		UNION
		
		SELECT #tmpReq.AreaId,
			SUM(TblLPRequest.DisconnectPower) / 1000 AS DisconnectPowerLP,
			0 AS DisconnectPowerMP,
			0 as DisconnectPowerTamir,
			0 as DisconnectPowerNotTamirPaydar,
			0 as DisconnectPowerNotTamirGozara,
			0 as DisconnectPowerTamirBarnameh,
			0 as DisconnectPowerTamirMovafegh,
			0 as DisconnectPowerTamirEzterar,
			0 as DisconnectPowerTamirUnknown,
			0 AS cntDCMP,
			0 AS cntDCMPPaydar,
			0 AS cntDCMPGozara,
			0 AS cntDCMPBarnameh,
			0 AS cntDCMPMovafegh,
			0 AS cntDCMPEzterar,
			0 AS cntDCMPUnknown,
			SUM(case when #tmpReq.IsTamir = 1 then TblLPRequest.DisconnectPower END) / 1000 AS DisconnectPowerLPTamir,
			SUM(case when #tmpReq.IsTamir = 0 then TblLPRequest.DisconnectPower END) / 1000 AS DisconnectPowerLPNotTamir

		FROM #tmpReq
		INNER JOIN TblLPRequest ON #tmpReq.LPRequestId = TblLPRequest.LPRequestId
		INNER JOIN Tbl_DisconnectGroupSet ON TblLPRequest.DisconnectGroupSetId = Tbl_DisconnectGroupSet.DisconnectGroupSetId
    LEFT JOIN Tbl_LPPost LPP ON TblLPRequest.LPPostId = LPP.LPPostId
    LEFT JOIN Tbl_LPFeeder LPF ON TblLPRequest.LPFeederId = LPF.LPFeederId
		INNER JOIN Tbl_Area ON #tmpReq.AreaId = Tbl_Area.AreaId
		WHERE 
			ISNULL(TblLPRequest.IsWarmLine, 0) = 0
			AND TblLPRequest.EndJobStateId IN (2, 3)
			AND ISNULL(TblLPRequest.IsSingleSubscriber, 0) = 0
			AND ISNULL(TblLPRequest.IsLightRequest, 0) = 0
      AND ISNULL(LPF.OwnershipId , ISNULL(LPP.OwnershipId , 2)) = 2
		GROUP BY
			#tmpReq.AreaId
		) as t1
	GROUP BY 
		t1.AreaId
		
	drop table #tmpReq
	drop table #tblArea
GO