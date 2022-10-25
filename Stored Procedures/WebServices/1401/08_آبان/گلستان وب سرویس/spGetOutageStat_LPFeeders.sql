CREATE PROCEDURE spGetOutageStat_LPFeeders
    @aFromDate AS VARCHAR(11),
    @aToDate AS VARCHAR(11),
    @aAreaIds AS VARCHAR(2000) = ''
  AS
  BEGIN
  
  select *
into #tempAreaIds
from dbo.Split(@aAreaIds,',')

select A.AreaId,A.Area
into #tempNeedArea
from Tbl_Area A
where (len(@aAreaIds)<=0 or A.AreaId in (select Item from #tempAreaIds))

  
          SELECT A.Area
          ,MPP.MPPostName
          ,MPF.MPFeederName
          ,LPP.LPPostName
          ,LPF.LPFeederName
          ,LPR.IsTotalLPFeederDisconnected AS IsTotalDisconnect
          ,CAST('LV_Feeder' AS VARCHAR(20)) AS OutageType 
          ,COUNT(*) AS Count 
          ,SUM(ISNULL(R.DisconnectInterval , 0)) AS DisconnectInterval
          ,SUM(case when R.IsTamir=1 then IsNull(R.DisconnectPower,0.0)
						else 0 end) AS DisconnectPowerPlanned
		  ,SUM(case when R.IsTamir=0 then IsNull(R.DisconnectPower,0.0)
						else 0 end) AS DisconnectPowerNotPlanned
        FROM TblRequest R
          INNER JOIN TblLPRequest LPR ON R.LPRequestId = LPR.LPRequestId
          INNER JOIN Tbl_LPFeeder LPF ON LPR.LPFeederId = LPF.LPFeederId
          INNER JOIN Tbl_LPPost LPP ON LPF.LPPostId = LPP.LPPostId
          INNER JOIN Tbl_MPFeeder MPF ON LPP.MPFeederId = MPF.MPFeederId
          INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
          INNER JOIN #tempNeedArea A ON R.AreaId = A.AreaId
        WHERE R.DisconnectDatePersian BETWEEN @aFromDate AND @aToDate
         AND LPR.IsTotalLPPostDisconnected = 0 
          AND ISNULL(LPR.IsSingleSubscriber , 0) = 0
        GROUP BY A.Area, MPP.MPPostName ,MPF.MPFeederName, LPP.LPPostName , 
                LPF.LPFeederId , LPF.LPFeederName ,LPR.IsTotalLPFeederDisconnected
  
  
      drop table #tempAreaIds
      drop table #tempNeedArea

  END