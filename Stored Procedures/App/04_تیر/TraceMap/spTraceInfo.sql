USE CCRequesterSetad
GO
CREATE PROCEDURE Homa.spTraceInfo
   @StartDate as varchar(10),
   @EndDate as varchar(10),
   @StartTime as varchar(8),
   @EndTime as varchar(8),
   @OnCallId as BIGINT
  AS
  BEGIN
    /*Calculate Time:*/
    if len(@StartTime) = 5
  		set @StartTime = @StartTime+':00'
  	if len(@EndTime) = 5
  		set @EndTime = @EndTime+':59'
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
  		set @StartStr = @StartDate + ' 00'
  		if @StartTime <> ''
  		begin
  			set @StartDT = DATEADD(HOUR,CAST(LEFT(@StartTime,2) as int),@StartDT)
  			set @StartDT = DATEADD(MINUTE,CAST(SUBSTRING(@StartTime,4,2)as int),@StartDT)
  			set @StartStr = @StartDate+ ' '+LEFT(@StartTime,2)
  		end
  	end
  	if @EndDate = ''
  	begin
  		set @EndDT = DATEADD(YEAR,+20,GETDATE())
  		set @EndStr = '1500/01/01 00'
  	end
  	else
  	begin
  		set @EndDT = dbo.shtom(@EndDate)
  		set @EndStr = @EndDate + ' 23'
  		if @EndTime <> ''
  		begin
  			set @EndDT = DATEADD(HOUR,CAST(LEFT(@EndTime,2) as int),@EndDT)
  			set @EndDT = DATEADD(MINUTE,CAST(SUBSTRING(@EndTime,4,2)as int),@EndDT)
  			set @EndDT = DATEADD(SECOND,59,@EndDT)
  			set @EndStr = @EndDate+ ' '+LEFT(@EndTime,2)
  		end
  		else
  			set @EndDT = DATEADD(day,1,@EndDT)
  	END
    /*Gather Data: */
    select * from Homa.TblTrace	
      where TblTrace.OnCallId = @OnCallId
      and 
      TblTrace.TraceDT >= @StartDT and TblTrace.TraceDT <= @EndDT
      order by TraceId
  END
/*
EXEC Homa.spTraceInfo @StartDate = '1399/11/04'
                     ,@EndDate = '1399/11/04'
                     ,@StartTime = ''
                     ,@EndTime = ''
                     ,@OnCallId = 9900000000000002
*/