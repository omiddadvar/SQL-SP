alter procedure spOmidTest 
AS 
Begin
SELECT
 distinct [TblOnCall].[OnCallId]
	  into #tmpOnCall
	  FROM [CCRequesterSetad].[Homa].[TblTraceSummary]
	  inner join [CCRequesterSetad].[Homa].[TblOnCall]
	  on [TblTraceSummary].OnCallId=[TblOnCall].OnCallId
	  where TargetDatePersian='1400/04/13'
	  and MasterId=20050975
	  
select Homa.TblTrace.*,CAST(0 as bit) as IsFindJob into #tmp from Homa.TblTrace where OnCallId in (
	  SELECT
		 [OnCallId]	      
	  FROM #tmpOnCall
  )
  order by OnCallId,TraceDT
  
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
  --select * from #tmpStartMove
  --select * from #tmpEkipArrive
  select #tmpStartMove.OnCallId,#tmpStartMove.JobId, #tmpStartMove.StartMoveDT,#tmpEkipArrive.ArriveDT  
  into #tmpFind
  from #tmpStartMove 
	inner join #tmpEkipArrive on #tmpStartMove.OnCallId=#tmpEkipArrive.OnCallId
	and #tmpStartMove.JobId=#tmpEkipArrive.JobId
  update #tmp
  set IsFindJob=1
  from  #tmp
  inner join #tmpFind
  on #tmpFind.OnCallId=#tmp.OnCallId
  where TraceDT >=#tmpFind.StartMoveDT
  and TraceDT <=#tmpFind.ArriveDT
  
	
  /*update #tmp
  set IsFindJob=1
  from (*/
  
  select TOP 20 * from #tmp
  drop table #tmpOnCall
  drop table #tmpFind
  drop table #tmpStartMove
  drop table #tmpEkipArrive
  drop table #tmp
End

exec spOmidTest