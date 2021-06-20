-------------------------
CREATE FUNCTION dbo.MinuteCount ( @aDate1 DateTime , @aDate2 DateTime , @aDT DateTime , @aCT DateTime )
 RETURNS float AS   BEGIN       
  declare @n  int     
  if  (@aDT < @aDate1 and @aCT < @aDate1 ) or ( @aDT > @aDate2 and @aCT > @aDate2) 
    set @n = 0
  else if( @aDT <= @aDate1 and @aCT <= @aDate2 ) 
    set @n = DateDiff( n , @aDate1 , @aCT )       
  else if @aDT <= @aDate1 and @aCT >= @aDate2              
    set @n = DateDiff( n , @aDate1 , @aDate2 )         
  else if @aDT >= @aDate1 and @aCT >= @aDate2  
    set @n = DateDiff( n , @aDT , @aDate2 )         
  else if @aDT >= @aDate1 and @aCT <= @aDate2              
    set @n = DateDiff( n , @aDT , @aCT )  
  return @n   
END 
GO

-------------------------------------------------------------------

CREATE Procedure dbo.spReportHourlyDisconnectPower(@aStartDate as varchar(10)) as begin set nocount on declare @IsKambood as bit=1 declare @aEndDate as varchar(10)='' declare @StartDate as varchar(10)=@aStartDate
,@EndDate as varchar(10) set @EndDate=@aStartDate set @EndDate=@StartDate set @StartDate=dbo.mtosh(dateadd(day
,-1
,dbo.shtom(@EndDate))) print @StartDate print @EndDate select RequestId
, LPRequestId
, MPRequestId
, FogheToziDisconnectId
, IsTamir
,DisconnectDT
, DisconnectTime
, ConnectDT
, AreaId
, IsSingleSubscriber
, EndJobStateId
,CAST(0 as float) as CurrentValue
, cast(0 as bigint) as FogheToziDisconnectMPFeederId
, CAST(0 as float) as Bar 
, CAST(null as nvarchar(100)) as RequestType
, CAST(null as nvarchar(100)) as PostTozi into #tmpRequest from TblRequest with (index(TblRequest52tza)) where TblRequest.DisconnectDatePersian>=@StartDate and TblRequest.DisconnectDatePersian<=@EndDate and IsLightRequest=0 and EndJobStateId in (2
,5
,3) and isnull(IsDuplicatedRequest
,0)=0 and ( (@IsKambood=1 and (IsFogheToziRequest=1 or IsMPRequest=1)) or (@IsKambood=0) ) if (@IsKambood=1 ) begin delete from #tmpRequest where not FogheToziDisconnectId is null and not RequestId in ( select RequestId from #tmpRequest inner join TblFogheToziDisconnect on #tmpRequest.FogheToziDisconnectId=TblFogheToziDisconnect.FogheToziDisconnectId where FogheToziId=1 ) end if (@IsKambood=1 ) begin delete from #tmpRequest where not MPRequestId is null and not MPRequestId in ( select RequestId from #tmpRequest inner join TblMPRequest on #tmpRequest.MPRequestId=TblMPRequest.MPRequestId where TblMPRequest.DisconnectReasonId in (1215
,1235
,1255) ) end if @IsKambood =0 begin update #tmpRequest set CurrentValue=TblLPRequest.CurrentValue/8.0
, IsSingleSubscriber=1
, RequestType=N'Ê˜ ãÔÊÑ˜' from #tmpRequest inner join TblLPRequest on #tmpRequest.LPRequestId=TblLPRequest.LPRequestId inner join Tbl_DisconnectGroupSet on Tbl_DisconnectGroupSet.DisconnectGroupSetId=TblLPRequest.DisconnectGroupSetId where Tbl_DisconnectGroupSet.DisconnectGroupId=4 end if @IsKambood =0 begin update #tmpRequest set CurrentValue=TblLPRequest.CurrentValue* [dbo].[GetLPPostZarib]( TblLPRequest.LPPostId
,TblLPRequest.DisconnectDatePersian
,TblLPRequest.DisconnectDT
,TblLPRequest.DisconnectTime)
, RequestType=N'İÔÇÑ ÖÚíİ'
, PostTozi= case when TblLPRequest.IsTotalLPPostDisconnected=1 then N'ÓÊ ÊæÒíÚ' else null end from #tmpRequest inner join TblLPRequest on #tmpRequest.LPRequestId=TblLPRequest.LPRequestId left join Tbl_DisconnectGroupSet on Tbl_DisconnectGroupSet.DisconnectGroupSetId=TblLPRequest.DisconnectGroupSetId where isnull(Tbl_DisconnectGroupSet.DisconnectGroupId
,0)<>4 end update #tmpRequest set CurrentValue=TblMPRequest.CurrentValue* [dbo].[GetLPPostZarib]( TblMPRequest.LPPostId
,TblMPRequest.DisconnectDatePersian
,TblMPRequest.DisconnectDT
,TblMPRequest.DisconnectTime)
, RequestType=N'İÔÇÑ ãÊæÓØ'
, PostTozi=N'ÓÊ ÊæÒíÚ' from #tmpRequest inner join TblMPRequest on #tmpRequest.MPRequestId=TblMPRequest.MPRequestId where not TblMPRequest.LPPostId is null and TblMPRequest.IsWarmLine=0 and TblMPRequest.IsTotalLPPostDisconnected=1 update #tmpRequest set Bar=CurrentValue*SQRT(3)*400*.85/8.0 where CurrentValue >0 update #tmpRequest set CurrentValue=TblMPRequest.CurrentValue
, RequestType=N'İÔÇÑ ãÊæÓØ'
, Bar=TblMPRequest.CurrentValue* isnull(Tbl_MPFeeder.Voltage
,20000)*sqrt(3)*isnull(Tbl_MPFeeder.CosinPhi
,.85) from #tmpRequest inner join TblMPRequest on #tmpRequest.MPRequestId=TblMPRequest.MPRequestId left join Tbl_MPFeeder on TblMPRequest.MPFeederId=Tbl_MPFeeder.MPFeederId where isnull(TblMPRequest.IsTotalLPPostDisconnected
,0)=0 and TblMPRequest.IsWarmLine=0 update #tmpRequest set CurrentValue=TblFogheToziDisconnect.CurrentValue
, Bar=TblFogheToziDisconnect.CurrentValue* isnull(Tbl_MPPost.VoltageOut
,20000)*sqrt(3)*.85
, RequestType = case when FogheToziId IN (1) then N'ßãÈæÏ ÊæáíÏ' when FogheToziId IN (2
,3
,4
,5) then N'ÇäÊŞÇá' when FogheToziId IN (6
,7
,8
,9) then N'İæŞ ÊæÒíÚ' end from #tmpRequest inner join TblFogheToziDisconnect on #tmpRequest.FogheToziDisconnectId=TblFogheToziDisconnect.FogheToziDisconnectId left join Tbl_MPPost on TblFogheToziDisconnect.MPPostId=Tbl_MPPost.MPPostId where not TblFogheToziDisconnect.FogheToziDisconnectId is null and TblFogheToziDisconnect.IsFeederMode=0 insert into #tmpRequest( RequestId
, LPRequestId
, MPRequestId
, FogheToziDisconnectId
, IsTamir
,DisconnectDT
, DisconnectTime
, ConnectDT
, AreaId
, IsSingleSubscriber
, EndJobStateId
,CurrentValue
, FogheToziDisconnectMPFeederId
, Bar 
, RequestType
, PostTozi) select RequestId
, LPRequestId
, MPRequestId
, null as FogheToziDisconnectId
, IsTamir
, TblFogheToziDisconnectMPFeeder.DisconnectDT
, TblFogheToziDisconnectMPFeeder.DisconnectTime
, TblFogheToziDisconnectMPFeeder.ConnectDT
, TblFogheToziDisconnect.AreaId
, 0 as IsSingleSubscriber
, TblFogheToziDisconnect.EndJobStateId
, TblFogheToziDisconnectMPFeeder.CurrentValue
, TblFogheToziDisconnectMPFeeder.FogheToziDisconnectMPFeederId
, TblFogheToziDisconnectMPFeeder.CurrentValue* isnull(Tbl_MPFeeder.Voltage
,20000)*sqrt(3)*ISNULL(Tbl_MPFeeder.CosinPhi
,.85) as Bar 
, case when FogheToziId IN (1) then N'ßãÈæÏ ÊæáíÏ' when FogheToziId IN (2
,3
,4
,5) then N'ÇäÊŞÇá' when FogheToziId IN (6
,7
,8
,9) then N'İæŞ ÊæÒíÚ' end as RequestType
, 0 as PostTozi from #tmpRequest inner join TblFogheToziDisconnect on #tmpRequest.FogheToziDisconnectId=TblFogheToziDisconnect.FogheToziDisconnectId inner join TblFogheToziDisconnectMPFeeder on TblFogheToziDisconnect.FogheToziDisconnectId=TblFogheToziDisconnectMPFeeder.FogheToziDisconnectId inner join Tbl_MPFeeder on TblFogheToziDisconnectMPFeeder.MPFeederId=Tbl_MPFeeder.MPFeederId delete #tmpRequest where Bar=0 Create Table #tmp(DT datetime
,Value float CONSTRAINT [PK_Tmp] PRIMARY KEY CLUSTERED ( DT ASC ) ) declare @dt as datetime set @dt=dateadd(day
,1
,dbo.shtom(@StartDate) ) declare @EndDT as datetime set @EndDT=DATEADD(day
,1
,@dt) while (@dt<@EndDT) begin insert into #tmp(DT
,Value)values (@dt
,0) set @dt=dateadd(minute
,1
,@dt) end declare @RequestId as bigint
,@Bar as float 
,@DisconnectDT as datetime
,@ConnectDT as Datetime DECLARE TaskCR CURSOR read_only FOR SELECT RequestId
,Bar
,DisconnectDT
,ConnectDT from #tmpRequest OPEN TaskCR FETCH NEXT FROM TaskCR INTO @RequestId
,@Bar
,@DisconnectDT
,@ConnectDT WHILE @@FETCH_STATUS = 0 BEGIN update #tmp set Value=Value+ @Bar where dt >=@DisconnectDT and (dt<@ConnectDT or @ConnectDT is null) FETCH NEXT FROM TaskCR INTO @RequestId
,@Bar
,@DisconnectDT
,@ConnectDT END CLOSE TaskCR DEALLOCATE TaskCR select TblMultistepConnection.ConnectDT as DT
,#tmpRequest.ConnectDT
, TblMultistepConnection.CurrentValue * isnull(Tbl_MPFeeder.Voltage
,20000)*sqrt(3)*ISNULL(Tbl_MPFeeder.CosinPhi
,.85) as Bar into #tmp2 from TblMultistepConnection inner join #tmpRequest on TblMultistepConnection.MPRequestId=#tmpRequest.MPRequestId inner join TblMPRequest on TblMPRequest.MPRequestId=#tmpRequest.MPRequestId inner join Tbl_MPFeeder on Tbl_MPFeeder.MPFeederId=TblMPRequest.MPFeederId where TblMultistepConnection.IsNotConnect=0 insert into #tmp2(DT
,ConnectDT
,Bar) select TblMultistepConnection.ConnectDT as DT
,TblFogheToziDisconnectMPFeeder.ConnectDT
, TblMultistepConnection.CurrentValue * isnull(Tbl_MPFeeder.Voltage
,20000)*sqrt(3)*ISNULL(Tbl_MPFeeder.CosinPhi
,.85) as Bar from #tmpRequest inner join TblFogheToziDisconnectMPFeeder on #tmpRequest.FogheToziDisconnectId=TblFogheToziDisconnectMPFeeder.FogheToziDisconnectId inner join TblMultistepConnection on TblMultistepConnection.FogheToziDisconnectId=TblFogheToziDisconnectMPFeeder.FogheToziDisconnectId and TblMultistepConnection.MPFeederId=TblFogheToziDisconnectMPFeeder.MPFeederId inner join Tbl_MPFeeder on Tbl_MPFeeder.MPFeederId=TblMultistepConnection.MPFeederId where TblMultistepConnection.IsNotConnect=0 DECLARE TaskCR CURSOR read_only FOR SELECT Bar
,DT
,ConnectDT from #tmp2 OPEN TaskCR FETCH NEXT FROM TaskCR INTO @Bar
,@DisconnectDT
,@ConnectDT WHILE @@FETCH_STATUS = 0 BEGIN update #tmp set Value=Value- @Bar where dt >=@DisconnectDT and (dt<@ConnectDT or @ConnectDT is null) FETCH NEXT FROM TaskCR INTO @Bar
,@DisconnectDT
,@ConnectDT END CLOSE TaskCR DEALLOCATE TaskCR update #tmp set Value=0 where Value <0 declare @DTToday as datetime=dbo.shtom(@EndDate) declare @DTYesterday as datetime=dateadd(day
,-1
,@DTToday) select dbo.GetTime(#tmp.DT) as DT
,Value/1000000.0 as 'ãíÒÇä ÈÇÑ ŞØÚ ÔÏå' into #tmpToday from #tmp where DT >= @DTToday select LEFT(a1.DT
,2) as 'Hour'
,round(sum(a1.[ãíÒÇä ÈÇÑ ŞØÚ ÔÏå])/60
,2) as [ÇäÑí ÊæÒíÚ äÔÏå] into #tmp3 FROM #tmpToday AS a1 group by LEFT(a1.DT
,2) order by LEFT(a1.DT
,2) declare @Province as nvarchar(100) select top 1 @Province=Province from Tbl_Province select @Province as Province
, sum(case when (#tmp3.[Hour]='00') then #tmp3.[ÇäÑí ÊæÒíÚ äÔÏå] else 0 end) as [Hour00]
, sum(case when (#tmp3.[Hour]='01') then #tmp3.[ÇäÑí ÊæÒíÚ äÔÏå] else 0 end) as [Hour01]
, sum(case when (#tmp3.[Hour]='02') then #tmp3.[ÇäÑí ÊæÒíÚ äÔÏå] else 0 end) as [Hour02]
, sum(case when (#tmp3.[Hour]='03') then #tmp3.[ÇäÑí ÊæÒíÚ äÔÏå] else 0 end) as [Hour03]
, sum(case when (#tmp3.[Hour]='04') then #tmp3.[ÇäÑí ÊæÒíÚ äÔÏå] else 0 end) as [Hour04]
, sum(case when (#tmp3.[Hour]='05') then #tmp3.[ÇäÑí ÊæÒíÚ äÔÏå] else 0 end) as [Hour05]
, sum(case when (#tmp3.[Hour]='06') then #tmp3.[ÇäÑí ÊæÒíÚ äÔÏå] else 0 end) as [Hour06]
, sum(case when (#tmp3.[Hour]='07') then #tmp3.[ÇäÑí ÊæÒíÚ äÔÏå] else 0 end) as [Hour07]
, sum(case when (#tmp3.[Hour]='08') then #tmp3.[ÇäÑí ÊæÒíÚ äÔÏå] else 0 end) as [Hour08]
, sum(case when (#tmp3.[Hour]='09') then #tmp3.[ÇäÑí ÊæÒíÚ äÔÏå] else 0 end) as [Hour09]
, sum(case when (#tmp3.[Hour]='10') then #tmp3.[ÇäÑí ÊæÒíÚ äÔÏå] else 0 end) as [Hour10]
, sum(case when (#tmp3.[Hour]='11') then #tmp3.[ÇäÑí ÊæÒíÚ äÔÏå] else 0 end) as [Hour11]
, sum(case when (#tmp3.[Hour]='12') then #tmp3.[ÇäÑí ÊæÒíÚ äÔÏå] else 0 end) as [Hour12]
, sum(case when (#tmp3.[Hour]='13') then #tmp3.[ÇäÑí ÊæÒíÚ äÔÏå] else 0 end) as [Hour13]
, sum(case when (#tmp3.[Hour]='14') then #tmp3.[ÇäÑí ÊæÒíÚ äÔÏå] else 0 end) as [Hour14]
, sum(case when (#tmp3.[Hour]='15') then #tmp3.[ÇäÑí ÊæÒíÚ äÔÏå] else 0 end) as [Hour15]
, sum(case when (#tmp3.[Hour]='16') then #tmp3.[ÇäÑí ÊæÒíÚ äÔÏå] else 0 end) as [Hour16]
, sum(case when (#tmp3.[Hour]='17') then #tmp3.[ÇäÑí ÊæÒíÚ äÔÏå] else 0 end) as [Hour17]
, sum(case when (#tmp3.[Hour]='18') then #tmp3.[ÇäÑí ÊæÒíÚ äÔÏå] else 0 end) as [Hour18]
, sum(case when (#tmp3.[Hour]='19') then #tmp3.[ÇäÑí ÊæÒíÚ äÔÏå] else 0 end) as [Hour19]
, sum(case when (#tmp3.[Hour]='20') then #tmp3.[ÇäÑí ÊæÒíÚ äÔÏå] else 0 end) as [Hour20]
, sum(case when (#tmp3.[Hour]='21') then #tmp3.[ÇäÑí ÊæÒíÚ äÔÏå] else 0 end) as [Hour21]
, sum(case when (#tmp3.[Hour]='22') then #tmp3.[ÇäÑí ÊæÒíÚ äÔÏå] else 0 end) as [Hour22]
, sum(case when (#tmp3.[Hour]='23') then #tmp3.[ÇäÑí ÊæÒíÚ äÔÏå] else 0 end) as [Hour23] from #tmp3 drop table #tmp drop table #tmp2 drop table #tmp3 drop table #tmpRequest drop table #tmpToday end 
GO