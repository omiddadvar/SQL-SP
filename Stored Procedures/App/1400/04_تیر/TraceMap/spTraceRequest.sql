CREATE PROCEDURE Homa.spTraceRequest
  @StartDate as varchar(10),
  @EndDate as varchar(10),
  @StartTime as varchar(8),
  @EndTime as varchar(8),
  @AreaId as INT  =  0 ,
  @Master as varchar(4000) = '',
  @Tablet as varchar(4000) = '',
  @Area as varchar(4000) = ''
  AS
  BEGIN
    /*Calculate Time: */
    if len(@StartTime) = 5
  		set @StartTime = @StartTime + ':00'
  	if len(@EndTime) = 5
  		set @EndTime = @EndTime + ':59'
  	declare @StartDT as Datetime
  	declare @EndDT as Datetime
  	declare @StartStr as varchar(13)
  	declare @EndStr as varchar(13)
  	if @StartDate = ''
  	begin
  		set @StartDT = DATEADD(YEAR,-20,GETDATE())
  		set @StartStr = '1300/01/01 00'
  	end
  	else
  	begin
  		set @StartDT = dbo.shtom(@StartDate)
  		set @StartStr = @StartDate  +  ' 00'
  		if @StartTime <> ''
  		begin
  			set @StartDT = DATEADD(HOUR,CAST(LEFT(@StartTime,2) as int),@StartDT)
  			set @StartDT = DATEADD(MINUTE,CAST(SUBSTRING(@StartTime,4,2)as int),@StartDT)
  			set @StartStr = @StartDate +  ' ' + LEFT(@StartTime,2)
  		end
  	end
  	if @EndDate = ''
  	begin
  		set @EndDT = DATEADD(YEAR, + 20,GETDATE())
  		set @EndStr = '1500/01/01 00'
  	end
  	else
  	begin
  		set @EndDT = dbo.shtom(@EndDate)
  		set @EndStr = @EndDate  +  ' 23'
  		if @EndTime <> ''
  		begin
  			set @EndDT = DATEADD(HOUR,CAST(LEFT(@EndTime,2) as int),@EndDT)
  			set @EndDT = DATEADD(MINUTE,CAST(SUBSTRING(@EndTime,4,2)as int),@EndDT)
  			set @EndDT = DATEADD(SECOND,59,@EndDT)
  			set @EndStr = @EndDate +  ' ' + LEFT(@EndTime,2)
  		end
  	END
    /*Gather Data: */
  	SELECT
		  distinct [TblOnCall].[OnCallId],[TblTraceSummary].AreaId,TargetDatePersian+
		  case when TargetTime<=9 then ' 0' 
		  else
		  ' ' end+ CAST(TargetTime AS varchar(10))
		  as TargetDT,
		  [TblTraceSummary].TraceLen
		  
		  
		  into #tmpOnCall1
		  FROM [Homa].[TblTraceSummary]
		  inner join [CCRequesterSetad].[Homa].[TblOnCall]
		  on [TblTraceSummary].OnCallId=[TblOnCall].OnCallId
		  where 
		  (TargetDatePersian>=@StartDate or @StartDate='')and
		  (TargetDatePersian<=@EndDate or @EndDate='')and
		  (AreaId<=@AreaId or @AreaId<=0)
		  
  	if @StartTime<>'' 
  		delete from #tmpOnCall1 where TargetDT <@StartStr
  	if @EndTime<>'' 
  		delete from #tmpOnCall1 where TargetDT >@EndStr
  	
  	select distinct #tmpOnCall1.OnCallId,#tmpOnCall1.AreaId into #tmpOnCall from #tmpOnCall1 
  	drop table #tmpOnCall1
    /*Check Master-Tablet-Area: */
  	if @Master<>'' 
  	begin
  		delete from #tmpOnCall
  		where not OnCallId
  		in (
  			select #tmpOnCall.OnCallId from #tmpOnCall 
  			inner join homa.TblOnCall on TblOnCall.OnCallId =#tmpOnCall.OnCallId
  			where MasterId in (
  			select item from dbo.Split(@Master,','))
  		)
  	end
  	if @Tablet<>'' 
  	begin
  		delete from #tmpOnCall
  		where not OnCallId
  		in (
  			select #tmpOnCall.OnCallId from #tmpOnCall 
  			inner join homa.TblOnCall on TblOnCall.OnCallId =#tmpOnCall.OnCallId
  			where TabletId in (
  			select item from dbo.Split(@Tablet,','))
  		)
  	end
  	if @Area<>'' 
  	begin
  		delete from #tmpOnCall
  		where not OnCallId
  		in (
  			select #tmpOnCall.OnCallId from #tmpOnCall 
  			where AreaId in (
  			select item from dbo.Split(@Area,','))
  		)
  	END 

    select * into #tmpEkipArrive from Homa.TblEkipArrive 
      where OnCallId in (
        SELECT
      		 [OnCallId]	      
      	  FROM #tmpOnCall
      )
    select * into #tmpStartMove from Homa.TblEkipStartMove
      where OnCallId in (
    	  SELECT
    		 [OnCallId]	      
    	  FROM #tmpOnCall
      )
    select TblJob.JobId, TblRequest.ConnectDT 
      into #tmpJob
      from Homa.TblJob 
      inner join TblRequest on TblJob.RequestId=TblRequest.RequestId
      where OncallId
       in (
    	  SELECT
    		 [OnCallId]	      
    	  FROM #tmpOnCall
      )
    select #tmpStartMove.OnCallId,#tmpStartMove.JobId, #tmpStartMove.StartMoveDT,#tmpEkipArrive.ArriveDT  ,#tmpJob.ConnectDT
      into #tmpFind
      from #tmpStartMove 
    	inner join #tmpJob on #tmpJob.JobId=#tmpStartMove.JobId
    	
    	inner join #tmpEkipArrive on #tmpStartMove.OnCallId=#tmpEkipArrive.OnCallId
    	and #tmpStartMove.JobId=#tmpEkipArrive.JobId
    	where	 
    	(		
    		#tmpJob.ConnectDT >=@StartDT 
    		and 
    		#tmpJob.ConnectDT <=@EndDT
    	)or
    	(
    		#tmpStartMove.StartMoveDT>=@StartDT
    		and 
    		#tmpStartMove.StartMoveDT<=@EndDT
    	)
    	or
    	(
    		#tmpStartMove.StartMoveDT<=@StartDT
    		and 
    		#tmpJob.ConnectDT>=@StartDT
    	)
    SELECT #tmpFind.OnCallId,#tmpFind.JobId,TblOnCall.TabletId,TblOnCall.MasterId,#tmpOnCall.AreaId,TblJob.RequestId,R.RequestNumber,
      R.Address , R.Telephone , R.DisconnectDatePersian , R.DisconnectTime,
    	R.ConnectDatePersian, R.ConnectTime , CAST(0 AS BIT) AS IsChecked
      FROM #tmpFind
      inner join homa.TblOnCall on TblOnCall.OnCallId=#tmpFind.OnCallId
      inner join homa.TblJob on TblJob.JobId=#tmpFind.JobId
      inner join TblRequest R on R.RequestId=TblJob.RequestId
      inner join #tmpOnCall on #tmpOnCall.OnCallId=#tmpFind.OnCallId

    drop table #tmpOnCall
    drop table #tmpFind
    drop table #tmpStartMove
    drop table #tmpEkipArrive
    drop table #tmpJob
  END
/*
EXEC Homa.spTraceRequest @StartDate = '1400/04/13'
                        ,@EndDate = '1400/04/13'
                        ,@StartTime = '10:41'
                        ,@EndTime = '10:45'
                        ,@AreaId = 0
                        ,@Master = ''
                        ,@Tablet = ''
                        ,@Area = ''
*/
GO