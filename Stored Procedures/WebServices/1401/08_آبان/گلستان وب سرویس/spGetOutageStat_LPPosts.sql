CREATE PROCEDURE spGetOutageStat_LPPosts
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
  
            SELECT * FROM (
            SELECT A.Area
              ,MPP.MPPostName
              ,MPF.MPFeederName
              ,LPP.LPPostName
              ,CAST(NULL AS NVARCHAR(50)) AS LPFeederName
              ,MPR.IsTotalLPPostDisconnected AS IsTotalDisconnect
              ,CAST('LV_Post' AS VARCHAR(20)) AS OutageType 
              ,COUNT(*) AS Count 
              ,SUM(ISNULL(R.DisconnectInterval , 0)) AS DisconnectInterval
              ,SUM(case when R.IsTamir=1 then IsNull(R.DisconnectPower,0.0)
						else 0 end) AS DisconnectPowerPlanned
			  ,SUM(case when R.IsTamir=0 then IsNull(R.DisconnectPower,0.0)
						else 0 end) AS DisconnectPowerNotPlanned
            FROM TblRequest R
              INNER JOIN TblMPRequest MPR ON R.MPRequestId = MPR.MPRequestId
              INNER JOIN Tbl_LPPost LPP ON MPR.LPPostId = LPP.LPPostId
              INNER JOIN Tbl_MPFeeder MPF ON LPP.MPFeederId = MPF.MPFeederId
              INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
              INNER JOIN Tbl_DisconnectGroupSet DGSet ON MPR.DisconnectGroupSetId = DGSet.DisconnectGroupSetId
              INNER JOIN Tbl_DisconnectGroup G ON DGSet.DisconnectGroupId = G.DisconnectGroupId
              INNER JOIN #tempNeedArea A ON R.AreaId = A.AreaId
            WHERE R.DisconnectDatePersian BETWEEN @aFromDate AND @aToDate
              AND G.DisconnectGroupId IN (1004 , 1005)
            GROUP BY A.Area, MPP.MPPostName ,MPF.MPFeederName, LPP.LPPostId ,LPP.LPPostName ,MPR.IsTotalLPPostDisconnected
            
          UNION
      
            SELECT A.Area
              ,MPP.MPPostName
              ,MPF.MPFeederName
              ,LPP.LPPostName
              ,NULL AS LPFeederName
              ,LPR.IsTotalLPPostDisconnected AS IsTotalDisconnect
              ,'LV_Post' AS OutageType 
              ,COUNT(*) AS Count 
              ,SUM(ISNULL(R.DisconnectInterval , 0)) AS DisconnectInterval
              ,SUM(case when R.IsTamir=1 then IsNull(R.DisconnectPower,0.0)
						else 0 end) AS DisconnectPowerPlanned
			  ,SUM(case when R.IsTamir=0 then IsNull(R.DisconnectPower,0.0)
						else 0 end) AS DisconnectPowerNotPlanned
            FROM TblRequest R
              INNER JOIN TblLPRequest LPR ON R.LPRequestId = LPR.LPRequestId
              INNER JOIN Tbl_LPPost LPP ON LPR.LPPostId = LPP.LPPostId
              INNER JOIN Tbl_MPFeeder MPF ON LPP.MPFeederId = MPF.MPFeederId
              INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
              INNER JOIN #tempNeedArea A ON R.AreaId = A.AreaId
            WHERE R.DisconnectDatePersian BETWEEN @aFromDate AND @aToDate
              AND LPR.IsTotalLPPostDisconnected = 1
            GROUP BY A.Area, MPP.MPPostName ,MPF.MPFeederName, LPP.LPPostId ,LPP.LPPostName ,LPR.IsTotalLPPostDisconnected
          ) Res 
  
      drop table #tempAreaIds
      drop table #tempNeedArea
  END
