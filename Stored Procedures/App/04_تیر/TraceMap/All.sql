CREATE PROCEDURE Homa.spTraceTablet
  @StartDate AS VARCHAR(10),
  @EndDate AS VARCHAR(10),
  @StartTime AS VARCHAR(8),
  @EndTime AS VARCHAR(8),
  @AreaId AS int = 0
  AS 
  BEGIN
    /*Calculation on time: */
  	IF len(@StartTime) = 5
		  SET @StartTime = @StartTime + ':00'
  	IF len(@EndTime) = 5
  		SET @EndTime = @EndTime + ':59'
  	DECLARE @StartDT AS Datetime
  	DECLARE @EndDT AS Datetime
  	DECLARE @StartStr AS VARCHAR(13)
  	DECLARE @EndStr AS VARCHAR(13)
  	IF @StartDate = ''
  	BEGIN 
  		SET @StartDT = DATEADD(YEAR,-20,GETDATE())
  		SET @StartStr = '1300/01/01 00'
  	end
  	ELSE
  	BEGIN 
  		SET @StartDT = dbo.shtom(@StartDate)
  		SET @StartStr = @StartDate + ' 00'
  		IF @StartTime <> ''
  		BEGIN 
  			SET @StartDT = DATEADD(HOUR,CAST(LEFT(@StartTime,2) AS int),@StartDT)
  			SET @StartDT = DATEADD(MINUTE,CAST(SUBSTRING(@StartTime,4,2)AS int),@StartDT)
  			SET @StartStr = @StartDate + ' ' + LEFT(@StartTime,2)
  		END
  	END
  IF @EndDate = ''
	BEGIN 
		SET @EndDT = DATEADD(YEAR, + 20,GETDATE())
		SET @EndStr = '1500/01/01 00'
	END
	ELSE
	BEGIN 
		SET @EndDT = dbo.shtom(@EndDate)
		SET @EndStr = @EndDate + ' 23'
		IF @EndTime <> ''
		BEGIN 
			SET @EndDT = DATEADD(HOUR,CAST(LEFT(@EndTime,2) AS int),@EndDT)
			SET @EndDT = DATEADD(MINUTE,CAST(SUBSTRING(@EndTime,4,2)AS int),@EndDT)
			SET @EndDT = DATEADD(SECOND,59,@EndDT)
			SET @EndStr = @EndDate + ' ' + LEFT(@EndTime,2)
		END
	END
  /*Gathering Data: */
	SELECT
		  [TblOnCall].[OnCallId],[TblTraceSummary].AreaId,TargetDatePersian + 
		  case when TargetTime< = 9 THEN ' 0' 
		  ELSE
		  ' ' END + CAST(TargetTime AS VARCHAR(10))
		  AS TargetDT,
		  [TblTraceSummary].TraceLen		  
		  INTO #tmpOnCall1
		  FROM [Homa].[TblTraceSummary]
		  INNER JOIN [CCRequesterSetad].[Homa].[TblOnCall]
		  ON [TblTraceSummary].OnCallId = [TblOnCall].OnCallId
		  WHERE 
		  (TargetDatePersian> = @StartDate or @StartDate = '')and
		  (TargetDatePersian< = @EndDate or @EndDate = '')and
		  (AreaId = @AreaId or @AreaId< = 0)
  		  
  	IF @StartTime <> '' 
  		DELETE FROM #tmpOnCall1 WHERE TargetDT < @StartStr
  	IF @EndTime <> '' 
  		DELETE FROM #tmpOnCall1 WHERE TargetDT > @EndStr
		SELECT #tmpOnCall1.OnCallId,#tmpOnCall1.AreaId,SUM(#tmpOnCall1.TraceLen) AS TraceLen INTO #tmpOnCall FROM #tmpOnCall1 
	    GROUP BY #tmpOnCall1.OnCallId,#tmpOnCall1.AreaId
    DROP TABLE #tmpOnCall1

    /*Tablet Data :*/
    SELECT TblOnCall.TabletId,min(Tbl_Tablet.TabletName) AS TabletName,round(sum(#tmpOnCall.TraceLen)/1000,2) AS TraceLen,
      CAST(0 AS BIT) AS IsChecked
      FROM #tmpOnCall
      INNER JOIN homa.TblOnCall ON TblOnCall.OnCallId = #tmpOnCall.OnCallId
      INNER JOIN Homa.Tbl_Tablet ON Tbl_Tablet.TabletId = TblOnCall.TabletId
      GROUP BY TblOnCall.TabletId

    DROP TABLE #tmpOnCall
  END

CREATE PROCEDURE Homa.spTraceArea
  @StartDate AS VARCHAR(10),
  @EndDate AS VARCHAR(10),
  @StartTime AS VARCHAR(8),
  @EndTime AS VARCHAR(8),
  @AreaId AS int = 0
  AS 
  BEGIN
    /*Calculation on time: */
  	IF len(@StartTime) = 5
		  SET @StartTime = @StartTime + ':00'
  	IF len(@EndTime) = 5
  		SET @EndTime = @EndTime + ':59'
  	DECLARE @StartDT AS Datetime
  	DECLARE @EndDT AS Datetime
  	DECLARE @StartStr AS VARCHAR(13)
  	DECLARE @EndStr AS VARCHAR(13)
  	IF @StartDate = ''
  	BEGIN 
  		SET @StartDT = DATEADD(YEAR,-20,GETDATE())
  		SET @StartStr = '1300/01/01 00'
  	end
  	ELSE
  	BEGIN 
  		SET @StartDT = dbo.shtom(@StartDate)
  		SET @StartStr = @StartDate + ' 00'
  		IF @StartTime <> ''
  		BEGIN 
  			SET @StartDT = DATEADD(HOUR,CAST(LEFT(@StartTime,2) AS int),@StartDT)
  			SET @StartDT = DATEADD(MINUTE,CAST(SUBSTRING(@StartTime,4,2)AS int),@StartDT)
  			SET @StartStr = @StartDate + ' ' + LEFT(@StartTime,2)
  		END
  	END
  IF @EndDate = ''
	BEGIN 
		SET @EndDT = DATEADD(YEAR, + 20,GETDATE())
		SET @EndStr = '1500/01/01 00'
	END
	ELSE
	BEGIN 
		SET @EndDT = dbo.shtom(@EndDate)
		SET @EndStr = @EndDate + ' 23'
		IF @EndTime <> ''
		BEGIN 
			SET @EndDT = DATEADD(HOUR,CAST(LEFT(@EndTime,2) AS int),@EndDT)
			SET @EndDT = DATEADD(MINUTE,CAST(SUBSTRING(@EndTime,4,2)AS int),@EndDT)
			SET @EndDT = DATEADD(SECOND,59,@EndDT)
			SET @EndStr = @EndDate + ' ' + LEFT(@EndTime,2)
		END
	END
  /*Gathering Data: */
	SELECT
		  [TblOnCall].[OnCallId],[TblTraceSummary].AreaId,TargetDatePersian + 
		  case when TargetTime< = 9 THEN ' 0' 
		  ELSE
		  ' ' END + CAST(TargetTime AS VARCHAR(10))
		  AS TargetDT,
		  [TblTraceSummary].TraceLen		  
		  INTO #tmpOnCall1
		  FROM [Homa].[TblTraceSummary]
		  INNER JOIN [CCRequesterSetad].[Homa].[TblOnCall]
		  ON [TblTraceSummary].OnCallId = [TblOnCall].OnCallId
		  WHERE 
		  (TargetDatePersian> = @StartDate or @StartDate = '')and
		  (TargetDatePersian< = @EndDate or @EndDate = '')and
		  (AreaId = @AreaId or @AreaId< = 0)
  		  
  	IF @StartTime <> '' 
  		DELETE FROM #tmpOnCall1 WHERE TargetDT < @StartStr
  	IF @EndTime <> '' 
  		DELETE FROM #tmpOnCall1 WHERE TargetDT > @EndStr
		SELECT #tmpOnCall1.OnCallId,#tmpOnCall1.AreaId,SUM(#tmpOnCall1.TraceLen) AS TraceLen INTO #tmpOnCall FROM #tmpOnCall1 
	    GROUP BY #tmpOnCall1.OnCallId,#tmpOnCall1.AreaId
    DROP TABLE #tmpOnCall1

    /*Area Data :*/
    SELECT #tmpOnCall.AreaId,min(Tbl_Area.Area) AS Area,round(sum(#tmpOnCall.TraceLen)/1000,2) AS TraceLen
      ,CAST(0 AS BIT) AS IsChecked
      FROM #tmpOnCall
      INNER JOIN homa.TblOnCall ON TblOnCall.OnCallId = #tmpOnCall.OnCallId
      INNER JOIN Tbl_Area ON Tbl_Area.AreaId = #tmpOnCall.AreaId
      GROUP BY #tmpOnCall.AreaId

    DROP TABLE #tmpOnCall
  END

CREATE PROCEDURE Homa.spTraceMaster 
  @StartDate AS VARCHAR(10),
  @EndDate AS VARCHAR(10),
  @StartTime AS VARCHAR(8),
  @EndTime AS VARCHAR(8),
  @AreaId AS int = 0
  AS 
  BEGIN
    /*Calculation on time: */
  	IF len(@StartTime) = 5
		  SET @StartTime = @StartTime + ':00'
  	IF len(@EndTime) = 5
  		SET @EndTime = @EndTime + ':59'
  	DECLARE @StartDT AS Datetime
  	DECLARE @EndDT AS Datetime
  	DECLARE @StartStr AS VARCHAR(13)
  	DECLARE @EndStr AS VARCHAR(13)
  	IF @StartDate = ''
  	BEGIN 
  		SET @StartDT = DATEADD(YEAR,-20,GETDATE())
  		SET @StartStr = '1300/01/01 00'
  	end
  	ELSE
  	BEGIN 
  		SET @StartDT = dbo.shtom(@StartDate)
  		SET @StartStr = @StartDate + ' 00'
  		IF @StartTime <> ''
  		BEGIN 
  			SET @StartDT = DATEADD(HOUR,CAST(LEFT(@StartTime,2) AS int),@StartDT)
  			SET @StartDT = DATEADD(MINUTE,CAST(SUBSTRING(@StartTime,4,2)AS int),@StartDT)
  			SET @StartStr = @StartDate + ' ' + LEFT(@StartTime,2)
  		END
  	END
  IF @EndDate = ''
	BEGIN 
		SET @EndDT = DATEADD(YEAR, + 20,GETDATE())
		SET @EndStr = '1500/01/01 00'
	END
	ELSE
	BEGIN 
		SET @EndDT = dbo.shtom(@EndDate)
		SET @EndStr = @EndDate + ' 23'
		IF @EndTime <> ''
		BEGIN 
			SET @EndDT = DATEADD(HOUR,CAST(LEFT(@EndTime,2) AS int),@EndDT)
			SET @EndDT = DATEADD(MINUTE,CAST(SUBSTRING(@EndTime,4,2)AS int),@EndDT)
			SET @EndDT = DATEADD(SECOND,59,@EndDT)
			SET @EndStr = @EndDate + ' ' + LEFT(@EndTime,2)
		END
	END
  /*Gathering Data: */
	SELECT
		  [TblOnCall].[OnCallId],[TblTraceSummary].AreaId,TargetDatePersian + 
		  case when TargetTime< = 9 THEN ' 0' 
		  ELSE
		  ' ' END + CAST(TargetTime AS VARCHAR(10))
		  AS TargetDT,
		  [TblTraceSummary].TraceLen		  
		  INTO #tmpOnCall1
		  FROM [Homa].[TblTraceSummary]
		  INNER JOIN [CCRequesterSetad].[Homa].[TblOnCall]
		  ON [TblTraceSummary].OnCallId = [TblOnCall].OnCallId
		  WHERE 
		  (TargetDatePersian> = @StartDate or @StartDate = '')and
		  (TargetDatePersian< = @EndDate or @EndDate = '')and
		  (AreaId = @AreaId or @AreaId< = 0)
  		  
  	IF @StartTime <> '' 
  		DELETE FROM #tmpOnCall1 WHERE TargetDT < @StartStr
  	IF @EndTime <> '' 
  		DELETE FROM #tmpOnCall1 WHERE TargetDT > @EndStr
		SELECT #tmpOnCall1.OnCallId,#tmpOnCall1.AreaId,SUM(#tmpOnCall1.TraceLen) AS TraceLen INTO #tmpOnCall FROM #tmpOnCall1 
	    GROUP BY #tmpOnCall1.OnCallId,#tmpOnCall1.AreaId
    DROP TABLE #tmpOnCall1

    /*Master Data :*/
    SELECT TblOnCall.MasterId,min(Tbl_Master.Name) AS MasterName,round(sum(#tmpOnCall.TraceLen)/1000, 2) AS TraceLen
      , CAST(0 AS BIT) AS IsChecked
      FROM #tmpOnCall
      INNER JOIN homa.TblOnCall ON TblOnCall.OnCallId = #tmpOnCall.OnCallId
      INNER JOIN Tbl_Master ON Tbl_Master.MasterId = TblOnCall.MasterId
      GROUP BY TblOnCall.MasterId

    DROP TABLE #tmpOnCall
  END

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
   	create table #tmpTrace2 
	(
		Radif int,
		TraceDT datetime,
		GpsX float,
		GpsY FLOAT
	)
	insert into #tmpTrace2
    EXEC Homa.spGetTrace @OnCallId , @StartDT , @EndDT


    SELECT #tmpTrace2.* , @OnCallId AS OnCallId, CAST(0 AS bit) AS IsFindJob	INTO #tmp 
      FROM #tmpTrace2
      ORDER BY Radif

    select * into #tmpEkipArrive from Homa.TblEkipArrive 
      where OnCallId  = @OnCallId
    select * into #tmpStartMove from Homa.TblEkipStartMove
      where OnCallId = @OnCallId
    select #tmpStartMove.OnCallId,#tmpStartMove.JobId, #tmpStartMove.StartMoveDT,#tmpEkipArrive.ArriveDT  
      into #tmpFind
      from #tmpStartMove 
    	inner join #tmpEkipArrive on #tmpStartMove.OnCallId=#tmpEkipArrive.OnCallId
    	and #tmpStartMove.JobId=#tmpEkipArrive.JobId

    update #tmp set IsFindJob=1
      from  #tmp
      inner join #tmpFind on #tmpFind.OnCallId=#tmp.OnCallId
      where TraceDT BETWEEN #tmpFind.StartMoveDT AND #tmpFind.ArriveDT

   select * from #tmp
    
    drop table #tmpFind
    drop table #tmpStartMove
    drop table #tmpEkipArrive
    drop table #tmp
  END

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
  	
  	select distinct OnCallId , AreaId into #tmpOnCall from #tmpOnCall1 
  	  
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
CREATE PROCEDURE Homa.spGetStateTrace
  @RequestId AS BIGINT
  ,@OnCallId AS BIGINT
  ,@StartDT AS DATETIME
  ,@EnsDT AS DATETIME
  AS 
  BEGIN
  	CREATE TABLE #tmpResult
    	(
    		Radif INT,
    		TraceDT DATETIME,
    		GpsX FLOAT,
    		GpsY FLOAT,
        Flag VARCHAR(10)
    	)
      CREATE TABLE #tmpTrace
    	(
    		Radif INT,
    		TraceDT DATETIME,
    		GpsX FLOAT,
    		GpsY FLOAT
    	)
     CREATE TABLE #tmpDT
    	(
    		TraceDT DATETIME,
        Flag VARCHAR(10)
    	)
      INSERT INTO  #tmpTrace EXEC Homa.spGetTrace @OnCallId

      /* Start Times */
      INSERT INTO #tmpDT SELECT tesm.StartMoveDT , 'Start'  FROM  Homa.TblJob tj
      INNER JOIN Homa.TblEkipStartMove tesm ON tj.JobId = tesm.JobId
      WHERE tj.RequestId = @RequestId

      /* Arrive Times */
      INSERT INTO #tmpDT SELECT tea.ArriveDT , 'Arrive'  FROM  Homa.TblJob tj
      INNER JOIN Homa.TblEkipArrive tea ON tj.JobId = tea.JobId
      WHERE tj.RequestId = @RequestId

      /* Finish Times */
      INSERT INTO #tmpDT SELECT tr.ConnectDT , 'Finish'  FROM  Homa.TblJob tj
      INNER JOIN dbo.TblRequest tr ON tr.RequestId = tj.RequestId
      WHERE tj.RequestId = @RequestId
        
      /*Get TraceStates */
        DECLARE db_cursor CURSOR FOR SELECT * FROM #tmpDT
        DECLARE @lDate DATETIME , @lFlag VARCHAR(10)
        OPEN db_cursor  
        FETCH NEXT FROM db_cursor INTO @lDate , @lFlag
        
        WHILE @@FETCH_STATUS = 0  
        BEGIN  
              INSERT INTO #tmpResult SELECT  TOP 1 * , @lFlag FROM #tmpTrace
                ORDER BY ABS(DATEDIFF(SECOND , @lDate , TraceDT))
              FETCH NEXT FROM db_cursor INTO @lDate , @lFlag
        END 
        
        CLOSE db_cursor  
        DEALLOCATE db_cursor 
      
      SELECT * FROM #tmpResult WHERE TraceDT BETWEEN @StartDT AND @EnsDT
      
      DROP TABLE #tmpResult
      DROP TABLE #tmpDT
      DROP TABLE #tmpTrace
  END