

CREATE procedure Homa.spGISGetRequest (@aRequestId as bigint=null,@Areas as varchar(500)='')
AS
begin
	if isnull(@aRequestId,0) > 0 
	begin
		
		select 
			RequestId,
			cast(RequestNumber as nvarchar(100)) + ' (' + DisconnectDatePersian+' '+DisconnectTime + ')' as RequestNumber,
			SubscriberName ,
			EndJobStateId,
			IsDuplicatedRequest,		
			DuplicatedRequestId
			
		into #tmpRequest1 from TblRequest 
		where 
			RequestId=@aRequestId
			or (IsDuplicatedRequest=1 and DuplicatedRequestId=@aRequestId)

		select 
			#tmpRequest1.*,
			isnull(Tbl_Master.Name,'') + ' ('+ Tbl_JobStatus.JobStatus + ')' as MasterName ,
			TblJob.gpsX,
			TblJob.gpsY,
			TblJob.OutageTypeId as OutageTypeId
			
			
			
			
			from #tmpRequest1
			inner join Homa.TblJob on #tmpRequest1.RequestId=TblJob.RequestId
			inner join Homa.Tbl_JobStatus on TblJob.JobStatusId=Tbl_JobStatus.JobStatusId
			left join Homa.TblOnCall on TblJob.OncallId=TblOnCall.OnCallId
			left join Homa.Tbl_TabletUser on TblOnCall.TabletUserId=Tbl_TabletUser.TabletUserId
			left join Tbl_Master on Tbl_TabletUser.MasterId=Tbl_Master.MasterId
			where 
				not TblJob.gpsX is null  and 
				not TblJob.gpsY is null
				
		order by IsDuplicate  desc ,RequestId
		drop table #tmpRequest1	
	end
	else
	begin
		select cast(Item as int) as AreaId into #tmpArea from dbo.Split(@Areas,',')
		where isnumeric(Item)=1
		
		declare @DTStart  as varchar(10),@DTEnd  as varchar(10)
		select 
			@DTStart=dbo.mtosh(dateadd(day,-1,getDate())),
			@DTEnd  =dbo.mtosh(dateadd(day,1,getDate()))
		select 
			RequestId,
			cast(RequestNumber as nvarchar(100)) + ' (' + DisconnectDatePersian+' '+DisconnectTime + ')' as RequestNumber,
			SubscriberName ,
			EndJobStateId,
			IsDuplicatedRequest,		
			DuplicatedRequestId,
			AreaId
			
		into #tmpRequest from TblRequest with (index(TblRequest52tza))
		where 
			DisconnectDatePersian >=@DTStart
			and DisconnectDatePersian <=@DTEnd and EndJobStateId in (1,10,4,5)
		delete #tmpRequest
		where not DuplicatedRequestId in (Select RequestId from #tmpRequest)
		if @Areas<>''
		begin
			delete from #tmpRequest
			where not AreaId in (select AreaId from #tmpArea)
		end
		
		drop table #tmpArea
		
		
		select 
			#tmpRequest.*,
			isnull(Tbl_Master.Name,'') + ' ('+ Tbl_JobStatus.JobStatus + ')' as MasterName ,
			TblJob.gpsX,
			TblJob.gpsY,
			TblJob.OutageTypeId as OutageTypeId
			
			
			
			
			from #tmpRequest
			inner join Homa.TblJob on #tmpRequest.RequestId=TblJob.RequestId
			inner join Homa.Tbl_JobStatus on TblJob.JobStatusId=Tbl_JobStatus.JobStatusId
			left join Homa.TblOnCall on TblJob.OncallId=TblOnCall.OnCallId
			left join Homa.Tbl_TabletUser on TblOnCall.TabletUserId=Tbl_TabletUser.TabletUserId
			left join Tbl_Master on Tbl_TabletUser.MasterId=Tbl_Master.MasterId
			where 
				not TblJob.gpsX is null  and 
				not TblJob.gpsY is null
				
		order by IsDuplicate  desc ,RequestId
		drop table #tmpRequest
	end
	
end
GO