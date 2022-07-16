CREATE Procedure dbo.spGetDisconnectPower(@aStartDate as varchar(10),@aTime as varchar(5))	
as 
begin
	declare @StartDate as varchar(10)=@aStartDate,@EndDate as varchar(10),@TargetDT as datetime
	
	set @TargetDT= CONVERT(datetime,dbo.shtom(@aStartDate) + ' ' +@aTime + '',21)
	print @TargetDT
	
	set @EndDate=@aStartDate
	
	set @EndDate=@StartDate
	set @StartDate=dbo.mtosh(dateadd(day,-2,dbo.shtom(@EndDate)))
	
	print @StartDate
	print @EndDate

		select 
		RequestId,
		LPRequestId,
		MPRequestId,
		FogheToziDisconnectId,
		IsTamir,DisconnectDT,		
		DisconnectTime,
		ConnectDT,
		AreaId,
		IsSingleSubscriber,
		EndJobStateId,CAST(0 as float) as CurrentValue,
		cast(0 as bigint) as FogheToziDisconnectMPFeederId,
		CAST(0 as float) as Bar ,
		CAST(null as nvarchar(100)) as RequestType,
		CAST(null as nvarchar(100)) as PostTozi

	into #tmpRequest
	from TblRequest with (index(TblRequest52tza))
	where TblRequest.DisconnectDatePersian>=@StartDate and TblRequest.DisconnectDatePersian<=@EndDate
	and IsLightRequest=0
	and EndJobStateId in (2,5,3)
	and isnull(IsDuplicatedRequest,0)=0
	--------------
	delete from #tmpRequest
	where ConnectDT <@TargetDT and EndJobStateId in (2,3)
	
	update #tmpRequest
	set 
	CurrentValue=TblLPRequest.CurrentValue/8.0,
	IsSingleSubscriber=1,
	RequestType=N'تک مشترک'

		from #tmpRequest 
		inner join TblLPRequest on #tmpRequest.LPRequestId=TblLPRequest.LPRequestId
		inner join Tbl_DisconnectGroupSet on Tbl_DisconnectGroupSet.DisconnectGroupSetId=TblLPRequest.DisconnectGroupSetId
		where Tbl_DisconnectGroupSet.DisconnectGroupId=4
	---------------
	--update #tmpRequest
	--set CurrentValue=TblLPRequest.CurrentValue
	update #tmpRequest
	set 
		CurrentValue=TblLPRequest.CurrentValue* [dbo].[GetLPPostZarib]( TblLPRequest.LPPostId,TblLPRequest.DisconnectDatePersian,TblLPRequest.DisconnectDT,TblLPRequest.DisconnectTime),
		RequestType=N'فشار ضعیف',
		PostTozi=
		case when TblLPRequest.IsTotalLPPostDisconnected=1 then N'پست توزیع'
		else null
		end
		
		from #tmpRequest 
		inner join TblLPRequest on #tmpRequest.LPRequestId=TblLPRequest.LPRequestId
		left join Tbl_DisconnectGroupSet on Tbl_DisconnectGroupSet.DisconnectGroupSetId=TblLPRequest.DisconnectGroupSetId
	where isnull(Tbl_DisconnectGroupSet.DisconnectGroupId,0)<>4

	update #tmpRequest
	set 
		CurrentValue=TblMPRequest.CurrentValue* [dbo].[GetLPPostZarib]( TblMPRequest.LPPostId,TblMPRequest.DisconnectDatePersian,TblMPRequest.DisconnectDT,TblMPRequest.DisconnectTime),
		RequestType=N'فشار متوسط',
		PostTozi=N'پست توزیع'

	from #tmpRequest 
		inner join TblMPRequest on #tmpRequest.MPRequestId=TblMPRequest.MPRequestId	
		where not TblMPRequest.LPPostId is null
		and TblMPRequest.IsWarmLine=0
		and TblMPRequest.IsTotalLPPostDisconnected=1
	update #tmpRequest 
	set Bar=CurrentValue*SQRT(3)*400*.85/8.0
	where CurrentValue >0
	update #tmpRequest
	set 
		CurrentValue=TblMPRequest.CurrentValue,
		RequestType=N'فشار متوسط',
		Bar=TblMPRequest.CurrentValue* isnull(Tbl_MPFeeder.Voltage,20000)*sqrt(3)*isnull(Tbl_MPFeeder.CosinPhi,.85)

	from #tmpRequest 
		inner join TblMPRequest on #tmpRequest.MPRequestId=TblMPRequest.MPRequestId	
		left join Tbl_MPFeeder on TblMPRequest.MPFeederId=Tbl_MPFeeder.MPFeederId
		where isnull(TblMPRequest.IsTotalLPPostDisconnected,0)=0
		and TblMPRequest.IsWarmLine=0

	update #tmpRequest
	set 
		CurrentValue=TblFogheToziDisconnect.CurrentValue,
		Bar=TblFogheToziDisconnect.CurrentValue* isnull(Tbl_MPPost.VoltageOut,20000)*sqrt(3)*.85,	
		RequestType =
			case 
				when FogheToziId IN (1) then N'كمبود توليد'
				when FogheToziId IN (2,3,4,5) then N'انتقال'
				when FogheToziId IN (6,7,8,9) then N'فوق توزیع'
			end
	from #tmpRequest 
		inner join TblFogheToziDisconnect on #tmpRequest.FogheToziDisconnectId=TblFogheToziDisconnect.FogheToziDisconnectId	
		left join Tbl_MPPost on TblFogheToziDisconnect.MPPostId=Tbl_MPPost.MPPostId
		where not TblFogheToziDisconnect.FogheToziDisconnectId	 is null
		and TblFogheToziDisconnect.IsFeederMode=0
	
	insert into #tmpRequest(
		RequestId,
		LPRequestId,
		MPRequestId,
		FogheToziDisconnectId,
		IsTamir,DisconnectDT,		
		DisconnectTime,
		ConnectDT,
		AreaId,
		IsSingleSubscriber,
		EndJobStateId,CurrentValue,
		FogheToziDisconnectMPFeederId,
		Bar ,
		RequestType,
		PostTozi)
	select RequestId,
		LPRequestId,
		MPRequestId,
		null as FogheToziDisconnectId,
		IsTamir,
		TblFogheToziDisconnectMPFeeder.DisconnectDT,		
		TblFogheToziDisconnectMPFeeder.DisconnectTime,
		TblFogheToziDisconnectMPFeeder.ConnectDT,
		TblFogheToziDisconnect.AreaId,
		0 as IsSingleSubscriber,
		TblFogheToziDisconnect.EndJobStateId,
		TblFogheToziDisconnectMPFeeder.CurrentValue,
		TblFogheToziDisconnectMPFeeder.FogheToziDisconnectMPFeederId,
		TblFogheToziDisconnectMPFeeder.CurrentValue* isnull(Tbl_MPFeeder.Voltage,20000)*sqrt(3)*ISNULL(Tbl_MPFeeder.CosinPhi,.85) as Bar  ,
		case 
				when FogheToziId IN (1) then N'كمبود توليد'
				when FogheToziId IN (2,3,4,5) then N'انتقال'
				when FogheToziId IN (6,7,8,9) then N'فوق توزیع'
			end as  RequestType,
		0 as PostTozi
		
		from #tmpRequest 		
		inner join TblFogheToziDisconnect on #tmpRequest.FogheToziDisconnectId=TblFogheToziDisconnect.FogheToziDisconnectId	
		
		inner join TblFogheToziDisconnectMPFeeder on TblFogheToziDisconnect.FogheToziDisconnectId=TblFogheToziDisconnectMPFeeder.FogheToziDisconnectId
		inner join Tbl_MPFeeder on TblFogheToziDisconnectMPFeeder.MPFeederId=Tbl_MPFeeder.MPFeederId		
	
	
	delete #tmpRequest where Bar=0
		
		
	
	declare @dt as datetime
	set @dt=dbo.shtom(@StartDate)	
	declare @EndDT as datetime
	set @EndDT=DATEADD(day,3,@dt)
	
	declare @TargetBar as float=0
	
	declare @RequestId as bigint,@Bar as float ,@DisconnectDT as datetime,@ConnectDT as Datetime
	DECLARE TaskCR CURSOR read_only FOR 
	   SELECT RequestId,Bar,DisconnectDT,ConnectDT  from #tmpRequest
	OPEN TaskCR
	FETCH NEXT
	   FROM TaskCR
	   INTO @RequestId,@Bar,@DisconnectDT,@ConnectDT
	WHILE @@FETCH_STATUS = 0
	BEGIN
	   if (@TargetDT >=@DisconnectDT and (@TargetDT<=@ConnectDT or @ConnectDT is null))	   
	   begin
		 set @TargetBar=@TargetBar+ @Bar				
	   end
	   FETCH NEXT
	   FROM TaskCR
	   INTO @RequestId,@Bar,@DisconnectDT,@ConnectDT
	END
	CLOSE TaskCR
	DEALLOCATE TaskCR
	
	select 	TblMultistepConnection.ConnectDT as DT,#tmpRequest.ConnectDT,
		TblMultistepConnection.CurrentValue * isnull(Tbl_MPFeeder.Voltage,20000)*sqrt(3)*ISNULL(Tbl_MPFeeder.CosinPhi,.85) as Bar into #tmp2
		from TblMultistepConnection
		inner join #tmpRequest on TblMultistepConnection.MPRequestId=#tmpRequest.MPRequestId
		inner join TblMPRequest on TblMPRequest.MPRequestId=#tmpRequest.MPRequestId		
		inner join Tbl_MPFeeder on Tbl_MPFeeder.MPFeederId=TblMPRequest.MPFeederId
		where TblMultistepConnection.IsNotConnect=0
	
	insert into #tmp2(DT,ConnectDT,Bar)
		select 	TblMultistepConnection.ConnectDT as DT,TblFogheToziDisconnectMPFeeder.ConnectDT,
		TblMultistepConnection.CurrentValue * isnull(Tbl_MPFeeder.Voltage,20000)*sqrt(3)*ISNULL(Tbl_MPFeeder.CosinPhi,.85) as Bar 
		from #tmpRequest
		inner join TblFogheToziDisconnectMPFeeder 
			on #tmpRequest.FogheToziDisconnectId=TblFogheToziDisconnectMPFeeder.FogheToziDisconnectId
		inner join TblMultistepConnection on 
			TblMultistepConnection.FogheToziDisconnectId=TblFogheToziDisconnectMPFeeder.FogheToziDisconnectId	
			and 
			TblMultistepConnection.MPFeederId=TblFogheToziDisconnectMPFeeder.MPFeederId
			
		inner join Tbl_MPFeeder on Tbl_MPFeeder.MPFeederId=TblMultistepConnection.MPFeederId
		where TblMultistepConnection.IsNotConnect=0
	
	
	DECLARE TaskCR CURSOR read_only FOR 
	   SELECT Bar,DT,ConnectDT  from #tmp2
	OPEN TaskCR
	FETCH NEXT
	   FROM TaskCR
	   INTO @Bar,@DisconnectDT,@ConnectDT
	WHILE @@FETCH_STATUS = 0
	BEGIN
	   if (@TargetDT >=@DisconnectDT and (@TargetDT<=@ConnectDT or @ConnectDT is null))
		 set @TargetBar=@TargetBar- @Bar	
	   --print 'Add Bar ' + 	cast(@Bar as varchar(100))
	   FETCH NEXT
	   FROM TaskCR
	   INTO @Bar,@DisconnectDT,@ConnectDT
	END
	CLOSE TaskCR
	DEALLOCATE TaskCR
		
	if @TargetBar <0 
		set @TargetBar=0
	
	
	select round(@TargetBar/1000000,2) as TargetBar		
	
	drop table #tmp2
	drop table #tmpRequest
	
	
	
end	
GO