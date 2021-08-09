USE [HavadesDashboard]
GO
/****** Object:  StoredProcedure [dbo].[spReportHomaAreaSummary]    Script Date: 08/09/2021 14:31:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[spReportHomaAreaSummary](@aStartDate as varchar(10),@aEndDate as varchar(10)) AS
BEGIN			
    select 
		AreaId,
		sum(BiBarnamehCount) as  'تعداد خاموشی فشار ضعیف بی برنامه' ,
		sum(BaBarnamehCount) as  'تعداد خاموشی فشار ضعیف با برنامه'
		into #tmpLPSummary
    from TblLPSummary
    where DisconnectDatePersian>=@aStartDate and DisconnectDatePersian<=@aEndDate
    group by AreaId
    
    select 
		AreaId,
		sum(BiBarnamehCount) as  'تعداد خاموشی فشار متوسط بی برنامه' ,
		sum(BaBarnamehCount) as  'تعداد خاموشی فشار متوسط با برنامه'
		into #tmpMPSummary
    from TblMPSummary
    where DisconnectDatePersian>=@aStartDate and DisconnectDatePersian<=@aEndDate
    group by AreaId
    
    
    
    
    select AreaId,
		SUM(LPTamirCount) as 'هما - تعداد خاموشی فشار ضعیف با برنامه' ,
		SUM(LPNotTamirCount) as 'هما - تعداد خاموشی فشار ضعیف بی برنامه' ,
		SUM(MPTamirCount) as 'هما - تعداد خاموشی فشار متوسط با برنامه' ,
		SUM(MPNotTamirCount) as 'هما - تعداد خاموشی فشار متوسط بی برنامه'  
		into #tmpHoma
	from TblHomaSummary 
	where DisconnectDatePersian>=@aStartDate and DisconnectDatePersian<=@aEndDate	
	group by AreaId
	select AreaId,
		sum(TblTraceSummary.TraceLen) as TraceLen,
		SUM(TblTraceSummary.OnCallHour) as OnCallHour,
		COUNT(*) as OnCallCount
	into #tmpTrace
	from CcRequesterSetad2.Homa.TblTraceSummary
	where TargetDatePersian>=@aStartDate and TargetDatePersian<=@aEndDate	
	group by AreaId

	        
	        
	select 
		Tbl_Area.AreaId,
		Tbl_Area.Area,		
		isnull(#tmpLPSummary.[تعداد خاموشی فشار ضعیف با برنامه],0)+
		isnull(#tmpLPSummary.[تعداد خاموشی فشار ضعیف بی برنامه],0)+
		isnull(#tmpMPSummary.[تعداد خاموشی فشار متوسط با برنامه],0)+
		isnull(#tmpMPSummary.[تعداد خاموشی فشار متوسط بی برنامه],0)
		as [جمع خاموشی ها],
		#tmpLPSummary.[تعداد خاموشی فشار ضعیف با برنامه],
		#tmpLPSummary.[تعداد خاموشی فشار ضعیف بی برنامه],
		#tmpMPSummary.[تعداد خاموشی فشار متوسط با برنامه],
		#tmpMPSummary.[تعداد خاموشی فشار متوسط بی برنامه],
		#tmpHoma.[هما - تعداد خاموشی فشار ضعیف با برنامه],
		#tmpHoma.[هما - تعداد خاموشی فشار ضعیف بی برنامه],
		#tmpHoma.[هما - تعداد خاموشی فشار متوسط با برنامه],
		#tmpHoma.[هما - تعداد خاموشی فشار متوسط بی برنامه],
		
		#tmpTrace.OnCallCount as  [تعداد آنکال],
		#tmpTrace.OnCallHour as [(مدت آنکالی(ساعت],
		round(#tmpTrace.TraceLen/1000,2) as [(مسافت طی شده(کیلومتر]
	into #tmpHomaSummary
	from CcRequesterSetad2.dbo.Tbl_Area
	inner join dbo.TblAreaQuota on Tbl_Area.AreaId=TblAreaQuota.AreaId
	left join #tmpHoma on #tmpHoma.AreaId=Tbl_Area.AreaId	
		
	left join #tmpLPSummary on #tmpLPSummary.AreaId=#tmpHoma.AreaId
	left join #tmpMPSummary on #tmpMPSummary.AreaId=#tmpHoma.AreaId
	left join #tmpTrace on #tmpTrace.AreaId=#tmpHoma.AreaId
	where Tbl_Area.IsCenter=0 and Tbl_Area.IsSetad=0
		and isnull(TblAreaQuota.X,0) >0
	
	insert into #tmpHomaSummary
	(
		AreaId,
		Area,
		[(مدت آنکالی(ساعت] ,
		[(مسافت طی شده(کیلومتر],
		[تعداد آنکال],
		[تعداد خاموشی فشار ضعیف با برنامه],
		[تعداد خاموشی فشار ضعیف بی برنامه],
		[تعداد خاموشی فشار متوسط با برنامه],
		[تعداد خاموشی فشار متوسط بی برنامه],
		[جمع خاموشی ها],
		[هما - تعداد خاموشی فشار ضعیف با برنامه],
		[هما - تعداد خاموشی فشار ضعیف بی برنامه],
		[هما - تعداد خاموشی فشار متوسط با برنامه],
		[هما - تعداد خاموشی فشار متوسط بی برنامه]		
	)
	select 
		99 as AreaId,
		'جمع کل شرکت' as Area,
		sum(#tmpHomaSummary.[(مدت آنکالی(ساعت] ),
		sum(#tmpHomaSummary.[(مسافت طی شده(کیلومتر]),
		sum(#tmpHomaSummary.[تعداد آنکال]),
		sum(#tmpHomaSummary.[تعداد خاموشی فشار ضعیف با برنامه]),
		sum(#tmpHomaSummary.[تعداد خاموشی فشار ضعیف بی برنامه]),
		sum(#tmpHomaSummary.[تعداد خاموشی فشار متوسط با برنامه]),
		sum(#tmpHomaSummary.[تعداد خاموشی فشار متوسط بی برنامه]),
		sum(#tmpHomaSummary.[جمع خاموشی ها]),
		sum(#tmpHomaSummary.[هما - تعداد خاموشی فشار ضعیف با برنامه]),
		sum(#tmpHomaSummary.[هما - تعداد خاموشی فشار ضعیف بی برنامه]),
		sum(#tmpHomaSummary.[هما - تعداد خاموشی فشار متوسط با برنامه]),
		sum(#tmpHomaSummary.[هما - تعداد خاموشی فشار متوسط بی برنامه])		
		from #tmpHomaSummary
	select * from #tmpHomaSummary
	order by AreaId
	
	
	drop table #tmpLPSummary
	drop table #tmpMPSummary
	drop table #tmpHoma
	drop table #tmpTrace
	drop table #tmpHomaSummary
	
end	

