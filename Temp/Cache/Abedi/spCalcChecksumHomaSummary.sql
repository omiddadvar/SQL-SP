USE [HavadesDashboard]
GO
/****** Object:  StoredProcedure [dbo].[spCalcChecksumHomaSummary]    Script Date: 08/09/2021 14:39:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



ALTER PROCEDURE [dbo].[spCalcChecksumHomaSummary]	
AS
BEGIN
	/*if (select dbo.IsNeedUpdate('ChecksumHomaSummaryLastUpdate'))=0 
	begin
		return 0
	end*/
	
	--select   @MinYear as MinYear,@MaxYear as MaxYear
	select OnCallStartDatePersian as DisconnectDatePersian,CHECKSUM_AGG(CHECKSUM(*)) as [CHECKSUM]  into #tmp2 from CcRequesterSetad.Homa.tblOncall
	group by OnCallStartDatePersian
	
	declare @Today as varchar(10)
	set @Today =dbo.MiladiToShamsi(getDate())
	
	select distinct #tmp2.* into #tmp3 from #tmp2
	left join 
	dbo.TblHomaSummary on #tmp2.DisconnectDatePersian =TblHomaSummary.DisconnectDatePersian 
	where (TblHomaSummary.DisconnectDatePersian is null) or #tmp2.[CHECKSUM]<>isnull(TblHomaSummary.[CHECKSUM],-1)
	or #tmp2.DisconnectDatePersian= @Today
	
	
	select TblJob.DisconnectDatePersian,TblJob.AreaId,COUNT(*) as cnt
	into #HomaMPNotTamir
	  FROM CCRequesterSetad.Homa.[TblRequestMP]
	  inner join CCRequesterSetad.Homa.TblRequestCommon
	  on [TblRequestMP].RequestCommonId=TblRequestCommon.RequestCommonId
	  inner join CCRequesterSetad.Homa.TblJob on TblJob.JobId=TblRequestCommon.JobId
	  inner join CCRequesterSetad.dbo.TblRequest on TblRequest.RequestId=TblJob.RequestId
	  where TblJob.IsTamir=0 and IsDuplicate=0 and JobStatusId in (10,11)
	  and TblJob.DisconnectDatePersian in (select DisconnectDatePersian from #tmp3)
	  group by TblJob.DisconnectDatePersian,TblJob.AreaId
			  
	select TblJob.DisconnectDatePersian,TblJob.AreaId,COUNT(*) as cnt
	into #HomaMPTamir
	  FROM CCRequesterSetad.Homa.[TblRequestMP]
	  inner join CCRequesterSetad.Homa.TblRequestCommon
	  on [TblRequestMP].RequestCommonId=TblRequestCommon.RequestCommonId
	  inner join CCRequesterSetad.Homa.TblJob on TblJob.JobId=TblRequestCommon.JobId
	  where IsTamir=1 and IsDuplicate=0 and JobStatusId in (10,11)
	  and TblJob.DisconnectDatePersian in (select DisconnectDatePersian from #tmp3)
	  group by TblJob.DisconnectDatePersian,TblJob.AreaId

	select TblJob.DisconnectDatePersian,TblJob.AreaId,COUNT(*) as cnt
	into #HomaLPNotTamir
	  FROM CCRequesterSetad.Homa.[TblRequestLP]
	  inner join CCRequesterSetad.Homa.TblRequestCommon
	  on [TblRequestLP].RequestCommonId=TblRequestCommon.RequestCommonId
	  inner join CCRequesterSetad.Homa.TblJob on TblJob.JobId=TblRequestCommon.JobId
	  where IsTamir=0 and IsDuplicate=0 and JobStatusId in (10,11)
	  and TblJob.DisconnectDatePersian in (select DisconnectDatePersian from #tmp3)
	  group by DisconnectDatePersian,TblJob.AreaId

	select TblJob.DisconnectDatePersian,TblJob.AreaId,COUNT(*) as cnt
	into #HomaLPTamir
	  FROM CCRequesterSetad.Homa.[TblRequestLP]
	  inner join CCRequesterSetad.Homa.TblRequestCommon
	  on [TblRequestLP].RequestCommonId=TblRequestCommon.RequestCommonId
	  inner join CCRequesterSetad.Homa.TblJob on TblJob.JobId=TblRequestCommon.JobId
	  where IsTamir=1 and IsDuplicate=0 and JobStatusId in (10,11)
	  and TblJob.DisconnectDatePersian in (select DisconnectDatePersian from #tmp3)
	  group by TblJob.DisconnectDatePersian,TblJob.AreaId

	select TblJob.DisconnectDatePersian,TblJob.AreaId,COUNT(*) as cnt
	into #HomaSubscriber
	  FROM CCRequesterSetad.Homa.[TblRequestSubscriber]
	  inner join CCRequesterSetad.Homa.TblRequestCommon
	  on [TblRequestSubscriber].RequestCommonId=TblRequestCommon.RequestCommonId
	  inner join CCRequesterSetad.Homa.TblJob on TblJob.JobId=TblRequestCommon.JobId
	  where IsDuplicate=0 and JobStatusId in (10,11)
	  and TblJob.DisconnectDatePersian in (select DisconnectDatePersian from #tmp3)
	  group by TblJob.DisconnectDatePersian,TblJob.AreaId
	select #tmp3.DisconnectDatePersian,Tbl_Area.AreaId
	into #HomaJob
	  from #tmp3 cross join CcRequesterSetad.dbo.Tbl_Area
	  
	  group by #tmp3.DisconnectDatePersian,Tbl_Area.AreaId
	  
	/*select * from #tmp3
	select * from #HomaJob*/

	select 
		#tmp3.DisconnectDatePersian,
		#tmp3.[CHECKSUM],
		#HomaJob.AreaId,
		sum(#HomaMPNotTamir.cnt) as MPNotTamirCount,
		sum(#HomaMPTamir.cnt) as MPTamirCount,
		isnull(sum(#HomaLPNotTamir.cnt),0)+isnull(sum(#HomaSubscriber.cnt),0) as LPNotTamirCount,
		sum(#HomaLPTamir.cnt) as LPTamirCount
		into #tmp4
	from #tmp3
	inner join #HomaJob on   #tmp3.DisconnectDatePersian=#HomaJob.DisconnectDatePersian
	left join #HomaMPNotTamir on #HomaJob.DisconnectDatePersian=#HomaMPNotTamir.DisconnectDatePersian 
		and #HomaJob.AreaId=#HomaMPNotTamir.AreaId
	
	left join #HomaMPTamir on #HomaJob.DisconnectDatePersian=#HomaMPTamir.DisconnectDatePersian 
		and #HomaJob.AreaId=#HomaMPTamir.AreaId
	
	left join #HomaLPNotTamir on #HomaJob.DisconnectDatePersian=#HomaLPNotTamir.DisconnectDatePersian 
		and #HomaJob.AreaId=#HomaLPNotTamir.AreaId
	
	left join #HomaLPTamir on #HomaJob.DisconnectDatePersian=#HomaLPTamir.DisconnectDatePersian 
		and #HomaJob.AreaId=#HomaLPTamir.AreaId
	
	left join #HomaSubscriber on #HomaJob.DisconnectDatePersian=#HomaSubscriber.DisconnectDatePersian 
		and #HomaJob.AreaId=#HomaSubscriber.AreaId
	
	group by #tmp3.DisconnectDatePersian,#tmp3.[CHECKSUM],#HomaJob.AreaId
	order by #tmp3.DisconnectDatePersian

	delete [dbo].[TblHomaSummary]
	where [DisconnectDatePersian]
	in (select DisconnectDatePersian from #tmp4)

	INSERT INTO [dbo].[TblHomaSummary]
			   ([DisconnectDatePersian]
			   ,[CHECKSUM]
			   ,[AreaId]
			   ,LPTamirCount
			   ,LPNotTamirCount
			   ,MPTamirCount
			   ,MPNotTamirCount
			   
			   )
	select 
		DisconnectDatePersian ,
		[CHECKSUM],
		AreaId
		,LPTamirCount
		,LPNotTamirCount
		,MPTamirCount
		,MPNotTamirCount
	from #tmp4
	where not DisconnectDatePersian in (select DisconnectDatePersian from [dbo].[TblHomaSummary] )

	
	 
	drop table #HomaMPNotTamir
	drop table #HomaMPTamir
	drop table #HomaLPNotTamir
	drop table #HomaLPTamir
	drop table #HomaJob
	drop table #HomaSubscriber
	
	drop table #tmp2
	drop table #tmp3
	drop table #tmp4
	declare @CurrentDT as varchar(100)=convert(nvarchar(100),getDate(),21)
	exec dbo.UpdateConfig 'ChecksumHomaSummaryLastUpdate' ,@CurrentDT


	
END


