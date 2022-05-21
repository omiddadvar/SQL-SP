--update BTblBazdidTiming
--set AreaId = tBT.AreaId
select tB.BazdidTimingId, tBT.BazdidTimingId, tB.AreaId, tBT.AreaId
from CcRequester.dbo.BTblBazdidTiming tBT
inner join BTblBazdidTiming tB on tBT.BazdidTimingId = tB.BazdidTimingId

--select COUNT(*) from BTblBazdidTiming

--update BTblBazdidResult
--set AreaId = tbR.AreaId
select tB.BazdidResultId, tbR.BazdidResultId, tB.AreaId, tbR.AreaId
from CcRequester.dbo.BTblBazdidResult tbR
inner join BTblBazdidResult tB on tbR.BazdidResultId = tb.BazdidResultId
