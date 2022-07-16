/*------------------------------------------------------*/
/*----------------------Emergency-----------------------*/
/*------------------------------------------------------*/
ALTER FUNCTION Emergency.GetMPFeederDisconnectCount (
	@MPFeederId AS INT
	,@IsDisconnectMPFeeder AS BIT
	,@MPFeederKeyId AS BIGINT,
  @aDayCount AS INT
	)
RETURNS INT
AS
BEGIN
	DECLARE @CntMP1 AS INT = 0
	DECLARE @CntMP2 AS INT = 0
	DECLARE @CntFT AS INT = 0

  IF @aDayCount IS NULL OR @aDayCount <= 0 BEGIN
    RETURN 0                                         	
  END

	SELECT @CntMP1 = COUNT(*)
	FROM TblMPRequest
	INNER JOIN TblRequest ON TblMPRequest.MPRequestId = TblRequest.MPRequestId
	WHERE TblMPRequest.DisconnectDT >= DATEADD(day, - @aDayCount, getdate())
		AND ISNULL(TblRequest.IsDisconnectMPFeeder, 1) = 1
		AND TblMPRequest.MPFeederId = @MPFeederId
    AND ISNULL(TblMPRequest.IsWarmLine , 0) = 0

	IF @IsDisconnectMPFeeder = 0
	BEGIN
		SELECT @CntMP2 = COUNT(*)
		FROM TblMPRequest
		INNER JOIN TblRequest ON TblMPRequest.MPRequestId = TblRequest.MPRequestId
		INNER JOIN TblMPRequestKey ON TblMPRequest.MPRequestId = TblMPRequestKey.MPRequestId
		WHERE TblMPRequest.DisconnectDT >= DATEADD(day, - @aDayCount, getdate())
			AND ISNULL(TblRequest.IsDisconnectMPFeeder, 1) = 0
			AND TblMPRequest.MPFeederId = @MPFeederId
			AND TblMPRequestKey.MPFeederKeyId = @MPFeederKeyId
      AND ISNULL(TblMPRequest.IsWarmLine , 0) = 0
	END

	SELECT @CntFT = COUNT(*)
	FROM dbo.TblFogheToziDisconnect
	INNER JOIN TblRequest ON TblRequest.FogheToziDisconnectId = TblFogheToziDisconnect.FogheToziDisconnectId
	INNER JOIN dbo.TblFogheToziDisconnectMPFeeder ON TblFogheToziDisconnect.FogheToziDisconnectId = TblFogheToziDisconnectMPFeeder.FogheToziDisconnectId
		AND TblRequest.DisconnectDT >= DATEADD(day, - @aDayCount, getdate())
		AND TblFogheToziDisconnectMPFeeder.MPFeederId = @MPFeederId

	RETURN isnull(@CntFT, 0) + isnull(@CntMP1, 0) + isnull(@CntMP2, 0)
END
GO


ALTER PROC Emergency.spGetFeederGroupPlanRaw 
  @aGroupMPFeederId AS BIGINT,
   @aDayCount AS INT,
  @aDisconnectDT AS DATETIME
AS
BEGIN
  SELECT
    MPT.MPFeederTemplateId
   ,MPF.MPFeederId
   ,MPP.MPPostName
   ,MPF.MPFeederName
   ,Emergency.GetMPFeederDisconnectCount(MPF.MPFeederId, MPT.IsDisconnectMPFeeder, MPT.MPFeederKeyId1,@aDayCount) 
        AS MPFeederDisconnectCount
   ,MPF.Voltage
   ,A.Area
   ,Emergency.GetMPFeederLoadHourId(MPF.MPFeederId , @aDisconnectDT) AS MPFeederLoadHourId
  INTO #tmp
  FROM Emergency.Tbl_MPFeederTemplate MPT
    INNER JOIN Tbl_MPFeeder MPF ON MPT.MPFeederId = MPF.MPFeederId
    INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
    INNER JOIN Tbl_Area A ON MPT.AreaId = A.AreaId
  WHERE GroupMPFeederId = @aGroupMPFeederId

  SELECT
    T.MPFeederTemplateId
   ,T.Area
   ,T.MPFeederId
   ,T.MPPostName
   ,T.MPFeederName
   ,T.MPFeederDisconnectCount
   ,ISNULL(ROUND(SQRT(3) * T.Voltage * H.CurrentValue * H.CosinPhi / 1000000, 2), 0) AS CurrentValueMW
   ,ISNULL(H.CurrentValue, 0) AS CurrentValue
   ,CAST(0 AS BIT) AS IsSelected
  FROM #tmp T
  LEFT JOIN Tbl_MPFeederLoadHours H ON T.MPFeederLoadHourId = H.MPFeederLoadHourId
  ORDER BY T.MPFeederDisconnectCount

  DROP TABLE #tmp
END
GO

ALTER PROC Emergency.spGetMPFeederTiming @aTiminigId AS INTEGER
AS
BEGIN
	SELECT TMPF.TimingMPFeederId
		,TMPF.MPFeederTemplateId
		,TMPF.MPFeederId
		,A.Area
		,MPP.MPPostName
		,MPF.MPFeederName
		,TMPF.RequestId
	    ,R.RequestNumber
		,TMPF.DisconnectDatePersian
		,TMPF.DisconnectTime
		,TMPF.ConnectDatePersian
		,TMPF.ConnectTime
		,TMPF.ForecastCurrentValue
		,TMPF.IsDisconnectMPFeeder
		,TMPF.PreCurrentValue
		,TMPF.CurrentValue
		,TMPF.TimingStateId
	INTO #tmp
	FROM Emergency.TblTimingMPFeeder TMPF
  		INNER JOIN Tbl_MPFeeder MPF ON TMPF.MPFeederId = MPF.MPFeederId
  		INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
		INNER JOIN Tbl_Area A ON TMPF.AreaId = A.AreaId
		LEFT JOIN TblRequest R ON TMPF.RequestId = R.RequestId
	WHERE TMPF.TimingId = @aTiminigId

	SELECT T.TimingMPFeederId
		,T.MPFeederTemplateId
		,T.MPFeederId
		,T.RequestId
		,T.RequestNumber
		,T.Area
		,T.MPPostName
		,T.MPFeederName
		,T.DisconnectDatePersian
		,T.DisconnectTime
		,T.ConnectDatePersian
		,T.ConnectTime
		,T.ForecastCurrentValue
		,T.IsDisconnectMPFeeder
		,ISNULL(T.PreCurrentValue , 0) AS PreCurrentValue
		,CASE WHEN T.TimingStateId = 5 THEN 0 ELSE 
		  ISNULL(T.CurrentValue,
				CASE WHEN T.IsDisconnectMPFeeder = 1 THEN ISNULL(LOAD.CurrentValue, 0) ELSE CAST(0 AS FLOAT) END) 
		  END AS CurrentValue	
		,CASE WHEN T.TimingStateId = 5 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS IsCanceled
		,CASE WHEN T.DisconnectDatePersian IS NULL  THEN 0 WHEN T.ConnectDatePersian IS NULL THEN 1 ELSE 2 END AS ChangeStateId
		,CAST(0 AS BIT) AS IsSelected
	INTO #tmpResult
	FROM #tmp T
	LEFT JOIN (
		SELECT L.MPFeederId
			,H.CurrentValue
			,row_number() OVER (
				PARTITION BY L.MPFeederId ORDER BY L.RelDate
					,H.HourId DESC
				) AS RowNum
		FROM Tbl_MPFeederLoad L
		INNER JOIN Tbl_MPFeederLoadHours H ON L.MPFeederLoadId = H.MPFeederLoadId
		) LOAD ON T.MPFeederId = LOAD.MPFeederId
	WHERE ISNULL(LOAD.RowNum, 1) = 1
	
	UPDATE #tmpResult SET PreCurrentValue = CurrentValue WHERE PreCurrentValue = 0
	
	SELECT * FROM #tmpResult

	DROP TABLE #tmp
	DROP TABLE #tmpResult
END
Go

ALTER PROC Emergency.spGetReportDisconnectAreas @aPersianDate AS VARCHAR(10),
@aAreaIDs AS VARCHAR(2000) = ''
AS
BEGIN

  DECLARE @lSQl AS VARCHAR(MAX) = '
    SELECT TMPF.RequestId, A.Area , G.GroupMPFeederName, MPP.MPPostName, MPF.MPFeederName
      ,R.TamirDisconnectFromDatePersian AS DisconnectDatePersian
      ,R.TamirDisconnectFromTime AS DisconnectTime
      ,R.TamirDisconnectToTime AS ConnectDatePersian
      ,R.TamirDisconnectToTime AS ConnectTime
      ,R.Address
    FROM Emergency.TblTiming T 
      INNER JOIN Emergency.TblTimingMPFeeder TMPF ON T.TimingId = TMPF.TimingId
      INNER JOIN TblRequest R ON TMPF.RequestId = R.RequestId
      INNER JOIN Tbl_MPFeeder MPF ON TMPF.MPFeederId = MPF.MPFeederId
      INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
      INNER JOIN Tbl_Area A ON TMPF.AreaId = A.AreaId
      INNER JOIN Emergency.Tbl_GroupMPFeeder G ON T.GroupMPFeederId = G.GroupMPFeederId
    WHERE R.EndJobStateId NOT IN (2,3) AND R.TamirDisconnectFromDatePersian = ''' + @aPersianDate + ''''
  IF LEN(@aAreaIDs) > 0
  BEGIN
    SET @lSQl = @lSQl + ' AND TMPF.AreaId IN(' + @aAreaIDs + ')'
  END
  SET @lSQl = @lSQl + ' ORDER BY TMPF.AreaId, TMPF.DisconnectDatePersian'
  EXEC (@lSQl)
END
GO


/*------------------------------------------------------*/
/*------------------------Ahvaz-------------------------*/
/*------------------------------------------------------*/

CREATE PROCEDURE Meter.spGetLatestActivePower
  @aDate VARCHAR(10),
  @aTime VARCHAR(5)
  AS
  BEGIN
    
    DECLARE @aFromDT AS DATETIME = DATEADD(MINUTE, -1, dbo.ShamsiDateTimeToMiladi(@aDate, @aTime))
    DECLARE @aToDT AS DATETIME = DATEADD(MINUTE, 15, @aFromDT)
    
    SELECT TOP(1) 
       dbo.mtosh(MeterTime) AS PersianDate
       ,(CAST(DATEPART(HOUR, MeterTime) AS VARCHAR(2)) + ':' + CAST(DATEPART(minute, MeterTime) AS VARCHAR(2))) AS Time 
       ,ROUND(ISNULL(Power_Active_Total, 0) , 2) AS ActivePower 
    FROM Meter.TblHourlyMeterValue
    WHERE MeterTime BETWEEN @aFromDT AND @aToDT 
    ORDER BY MeterTime DESC
   
  END
GO

CREATE PROCEDURE spGetDisconnectMPFeeders
  @aAreaIds VARCHAR(MAX)
  AS
  BEGIN
  	SELECT Item AS AreaId INTO #tmpArea FROM dbo.Split(@aAreaIds,',')
    
    
    SELECT * INTO #tmp FROM 
      (
      SELECT MPR.MPFeederId , R.IsDisconnectMPFeeder ,COUNT(MPR.MPRequestId) AS Count
      FROM TblMPRequest MPR
        INNER JOIN TblRequest R ON MPR.MPRequestId = R.MPRequestId
        INNER JOIN Tbl_MPFeeder MPF ON MPR.MPFeederId = MPF.MPFeederId
        INNER JOIN #tmpArea A ON MPF.AreaId = A.AreaId
      WHERE MPR.EndJobStateId IN (4,5)
        AND R.IsDisconnectMPFeeder = 1
      GROUP BY MPR.MPFeederId , R.IsDisconnectMPFeeder
    UNION
      SELECT MPR.MPFeederId , R.IsDisconnectMPFeeder  ,COUNT(MPR.MPRequestId) AS Count
      FROM TblMPRequest MPR
        INNER JOIN TblRequest R ON MPR.MPRequestId = R.MPRequestId
        INNER JOIN #tmpArea A ON MPR.AreaId = A.AreaId
      WHERE MPR.EndJobStateId IN (4,5)
        AND R.IsDisconnectMPFeeder = 0 
        AND MPR.IsNotDisconnectFeeder = 1
      GROUP BY MPR.MPFeederId , R.IsDisconnectMPFeeder
      ) Temp
    
    
    SELECT A.AreaId , A.Area
          ,MPP.MPPostName , MPP.MPPostCode
          ,MPF.MPFeederName , MPF.MPFeederCode
          ,T.IsDisconnectMPFeeder , T.Count
    FROM Tbl_MPFeeder MPF
      INNER JOIN #tmp T ON MPF.MPFeederId = T.MPFeederId
      INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
      INNER JOIN Tbl_Area A ON MPF.AreaId = A.AreaId
    
    
    DROP TABLE #tmpArea
    DROP TABLE #tmp
  END
GO


/* -----------------Mr.Abedi Nejad----------------- */

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
	RequestType=N'Ê˜ ãÔÊÑ˜'

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
		RequestType=N'ÝÔÇÑ ÖÚíÝ',
		PostTozi=
		case when TblLPRequest.IsTotalLPPostDisconnected=1 then N'ÓÊ ÊæÒíÚ'
		else null
		end
		
		from #tmpRequest 
		inner join TblLPRequest on #tmpRequest.LPRequestId=TblLPRequest.LPRequestId
		left join Tbl_DisconnectGroupSet on Tbl_DisconnectGroupSet.DisconnectGroupSetId=TblLPRequest.DisconnectGroupSetId
	where isnull(Tbl_DisconnectGroupSet.DisconnectGroupId,0)<>4

	update #tmpRequest
	set 
		CurrentValue=TblMPRequest.CurrentValue* [dbo].[GetLPPostZarib]( TblMPRequest.LPPostId,TblMPRequest.DisconnectDatePersian,TblMPRequest.DisconnectDT,TblMPRequest.DisconnectTime),
		RequestType=N'ÝÔÇÑ ãÊæÓØ',
		PostTozi=N'ÓÊ ÊæÒíÚ'

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
		RequestType=N'ÝÔÇÑ ãÊæÓØ',
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
				when FogheToziId IN (1) then N'ßãÈæÏ ÊæáíÏ'
				when FogheToziId IN (2,3,4,5) then N'ÇäÊÞÇá'
				when FogheToziId IN (6,7,8,9) then N'ÝæÞ ÊæÒíÚ'
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
				when FogheToziId IN (1) then N'ßãÈæÏ ÊæáíÏ'
				when FogheToziId IN (2,3,4,5) then N'ÇäÊÞÇá'
				when FogheToziId IN (6,7,8,9) then N'ÝæÞ ÊæÒíÚ'
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