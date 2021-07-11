USE CCRequesterSetad
GO
CREATE PROCEDURE Homa.spTraceOnCall
   @StartDate as varchar(10),
   @EndDate as varchar(10),
   @StartTime as varchar(8),
   @EndTime as varchar(8),
   @AreaId as INT=0,
   @Master as varchar(4000)='',
   @Tablet as varchar(4000)='',
   @Area as varchar(4000)=''
  AS
  BEGIN
    /*Calculate Time: */
  	if len(@StartTime)=5
  		set @StartTime=@StartTime+':00'
  	if len(@EndTime)=5
  		set @EndTime=@EndTime+':59'
  	declare @StartDT as Datetime
  	declare @EndDT as Datetime
  	declare @StartStr as varchar(13)
  	declare @EndStr as varchar(13)
  	if @StartDate=''
  	begin
  		set @StartDT=DATEADD(YEAR,-20,GETDATE())
  		set @StartStr='1300/01/01 00'
  	end
  	else
  	begin
  		set @StartDT=dbo.shtom(@StartDate)
  		set @StartStr=@StartDate + ' 00'
  		if @StartTime <>''
  		begin
  			set @StartDT=DATEADD(HOUR,CAST(LEFT(@StartTime,2) as int),@StartDT)
  			set @StartDT=DATEADD(MINUTE,CAST(SUBSTRING(@StartTime,4,2)as int),@StartDT)
  			set @StartStr=@StartDate+ ' '+LEFT(@StartTime,2)
  		end
  	end
  	if @EndDate=''
  	begin
  		set @EndDT=DATEADD(YEAR,+20,GETDATE())
  		set @EndStr='1500/01/01 00'
  	end
  	else
  	begin
  		set @EndDT=dbo.shtom(@EndDate)
  		set @EndStr=@EndDate + ' 23'
  		if @EndTime <>''
  		begin
  			set @EndDT=DATEADD(HOUR,CAST(LEFT(@EndTime,2) as int),@EndDT)
  			set @EndDT=DATEADD(MINUTE,CAST(SUBSTRING(@EndTime,4,2)as int),@EndDT)
  			set @EndDT=DATEADD(SECOND,59,@EndDT)
  			set @EndStr=@EndDate+ ' '+LEFT(@EndTime,2)
  		end
  	END
    /*Gathering Data: */
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
  	
  	select distinct #tmpOnCall1.OnCallId into #tmpOnCall from #tmpOnCall1 
  	  
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
    	end
    
    select distinct OnCallId from #tmpOnCall
    	      
    drop table #tmpOnCall
  END
/*
EXEC Homa.spTraceOnCall @StartDate = '1400/04/13'
                       ,@EndDate = '1400/04/13'
                       ,@StartTime = '09:54'
                       ,@EndTime = '11:56'
                       ,@AreaId = 0
                       ,@Master = ''
                       ,@Tablet = ''
                       ,@Area = ''
*/