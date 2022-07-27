
ALTER PROCEDURE dbo.spGetReport_14_4
	@MediaTypeId int,
	@AreaIDs as varchar(500),
	@FromDate as nvarchar(10),
	@ToDate as nvarchar(10),
	@ExternalServiceIds as varchar(500)
AS
begin
	declare @lsql as nvarchar(4000)

	create table #tmpArea (AreaId int)
		set @lsql = ' select AreaId from tbl_Area '
	if @AreaIDs <> ''
		set  @lsql += ' where AreaId in (' + @AreaIDs + ')'
	insert #tmpArea exec (@lsql)

	create table #tmpExternal (ExternalServiceId int)
		set @lsql = ' select ExternalServiceId from Tbl_ExternalService '
	if @ExternalServiceIds <> ''
		set  @lsql += ' where ExternalServiceId in (' + @ExternalServiceIds + ')'
	insert #tmpExternal exec (@lsql)

	select tl.AreaId , Area , SUM(cntAll) as cntAll ,SUM(cntNew6) as cntNew6,SUM(cntDone6) as cntDone6,
		SUM(cntNotDone6) as cntNotDone6,SUM(avgInterval6) as avgInterval6,
		ISNULL(Round(CAST(SUM(cntSum6) AS float) / NullIF(CAST(SUM(cntAll) AS float ),0) ,3), 0) as cntNesbat6 ,
		SUM(cntNew5) as cntNew5,SUM(cntDone5) as cntDone5,SUM(cntNotDone5) as cntNotDone5,
		SUM(avgInterval5) as avgInterval5 , 
		ISNULL(Round(CAST(SUM(cntSum5) AS float) / NullIF(CAST(SUM(cntAll) AS float ),0) , 3) ,0) as cntNesbat5 
		from
		(
		select 6 as mediaTypeId, TblRequest.areaId,
			sum(case when EndJobStateId in (4,5) then 1 
					when EndJobStateId = 9 and dbo.erjaStateId(TblRequest.RequestId) in (1,3) then 1
					else 0 end ) as cntNew6,
			sum(case when EndJobStateId in (2,3) then  1 
					when EndJobStateId = 9 and dbo.erjaStateId(TblRequest.RequestId) in (4) then 1
					else 0 end ) as cntDone6,
			sum(case when EndJobStateId in (8) then  1 
					when EndJobStateId = 9 and dbo.erjaStateId(TblRequest.RequestId) in (5) then 1
					else 0 end ) as cntNotDone6,
			sum(case when EndJobStateId in (2,3,4,5,8) then  1
					when EndJobStateId = 9 and dbo.erjaStateId(TblRequest.RequestId) in (1,3,4,5) then 1
					else 0 end ) as cntSum6,
			avg(case when EndJobStateId in (2,3) then DisconnectInterval
					when EndJobStateId = 9 and dbo.erjaStateId(TblRequest.RequestId) in (4) then DisconnectInterval
					 end) as avgInterval6,
			0 as cntNew5,
			0 as cntDone5,
			0 as cntNotDone5,
			0 as cntSum5,
			0 as avgInterval5,
			0 as cntAll
				from TblRequest 
			inner join #tmpArea on TblRequest.AreaId = #tmpArea.AreaId
			LEFT JOIN Tbl_GISSMSRecevied tGSMS ON TblRequest.RequestId = tGSMS.RequestId
			INNER JOIN TblRequestData tRD on TblRequest.RequestId = tRD.RequestId
			INNER JOIN #tmpExternal on tRD.ExternalServiceId = #tmpExternal.ExternalServiceId
			where TblRequest.DisconnectDatePersian >= @FromDate
				and TblRequest.DisconnectDatePersian <= @ToDate
				and ISNULL(tGSMS.MediaTypeId, ISNULL(tRD.MediaTypeId,
					case when not TblRequest.CallId is null then 2 else 10 end)) = 6
				and (@MediaTypeId = 6 or @MediaTypeId = 0)
			group by TblRequest.AreaId
		union
		select 5 as mediaTypeId, TblRequest.areaId,
			0 as cntNew6,
			0 as cntDone6,
			0 as cntNotDone6,
			0 as cntSum6,
			0 as avgInterval6,
			sum(case when EndJobStateId in (4,5) then 1 
					when EndJobStateId = 9 and dbo.erjaStateId(TblRequest.RequestId) in (1,3) then 1
					else 0 end ) as cntNew5,
			sum(case when EndJobStateId in (2,3) then  1 
					when EndJobStateId = 9 and dbo.erjaStateId(TblRequest.RequestId) in (4) then 1
					else 0 end ) as cntDone5,
			sum(case when EndJobStateId in (8) then  1 
					when EndJobStateId = 9 and dbo.erjaStateId(TblRequest.RequestId) in (5) then 1
					else 0 end ) as cntNotDone5,
			sum(case when EndJobStateId in (2,3,4,5,8) then  1
					when EndJobStateId = 9 and dbo.erjaStateId(TblRequest.RequestId) in (1,3,4,5) then 1
					else 0 end ) as cntSum5,
			avg(case when EndJobStateId in (2,3) then DisconnectInterval
					when EndJobStateId = 9 and dbo.erjaStateId(TblRequest.RequestId) in (4) then DisconnectInterval				
					 end) as avgInterval5,
			0 as cntAll
				from TblRequest
			inner join #tmpArea on TblRequest.AreaId = #tmpArea.AreaId
			LEFT JOIN Tbl_GISSMSRecevied tGSMS ON TblRequest.RequestId = tGSMS.RequestId
			INNER JOIN TblRequestData tRD on TblRequest.RequestId = tRD.RequestId
			INNER JOIN #tmpExternal on tRD.ExternalServiceId = #tmpExternal.ExternalServiceId
			where TblRequest.DisconnectDatePersian >= @FromDate
				and TblRequest.DisconnectDatePersian <= @ToDate
				and ISNULL(tGSMS.MediaTypeId, ISNULL(tRD.MediaTypeId, 
					case when not TblRequest.CallId is null then 2 else 10 end)) = 5
				and (@MediaTypeId = 5 or @MediaTypeId = 0)
			group by TblRequest.AreaId
			union 
			select 0 as mediaTypeId, TblRequest.areaId,
			0 as cntNew6,
			0 as cntDone6,
			0 as cntNotDone6,
			0 as cntSum6,
			0 as avgInterval6,
			0 as cntNew5,
			0 as cntDone5,
			0 as cntNotDone5,
			0 as cntSum5,
			0 as avgInterval5,
			sum(case when EndJobStateId not in (6,7,10) then  1 else 0 end ) as cntAll
				from TblRequest
			inner join #tmpArea on TblRequest.AreaId = #tmpArea.AreaId
			LEFT JOIN Tbl_GISSMSRecevied tGSMS ON TblRequest.RequestId = tGSMS.RequestId
			INNER JOIN TblRequestData tRD on TblRequest.RequestId = tRD.RequestId
			INNER JOIN #tmpExternal on tRD.ExternalServiceId = #tmpExternal.ExternalServiceId
			where TblRequest.DisconnectDatePersian >= @FromDate
				and TblRequest.DisconnectDatePersian <= @ToDate
				and (ISNULL(tGSMS.MediaTypeId, ISNULL(tRD.MediaTypeId, 
					case when not TblRequest.CallId is null then 2 else 10 end)) = @MediaTypeId 
					or @MediaTypeId = 0)
			group by TblRequest.AreaId
		) tl 
		inner join Tbl_Area on Tbl_Area.AreaId = tl.AreaId
		where IsCenter = 0
		group by tl.AreaId , Area

	drop table #tmpArea
	drop table #tmpExternal
END