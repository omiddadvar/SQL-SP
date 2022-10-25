alter PROCEDURE spGetOutageStat_MPFeeders 
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


	SELECT *
	FROM (
         SELECT A.Area
          ,MPP.MPPostName
          ,MPF.MPFeederName
          ,CAST(NULL AS NVARCHAR(100)) AS LPPostName
          ,CAST(NULL AS NVARCHAR(50)) AS LPFeederName
          ,CAST(1 AS BIT) AS IsTotalDisconnect
          ,CAST('MV_Feeder' AS VARCHAR(20)) AS OutageType 
          ,COUNT(*) AS Count 
          ,SUM(ISNULL(R.DisconnectInterval , 0)) AS DisconnectInterval
          ,SUM(case when R.IsTamir=1 then IsNull(R.DisconnectPower,0.0)
						else 0 end) AS DisconnectPowerPlanned

	  ,SUM(case when R.IsTamir=0 then IsNull(R.DisconnectPower,0.0)
						else 0 end) AS DisconnectPowerNotPlanned
        FROM TblRequest R
          INNER JOIN TblFogheToziDisconnect F ON R.FogheToziDisconnectId = F.FogheToziDisconnectId
          INNER JOIN TblFogheToziDisconnectMPFeeder FTMP ON F.FogheToziDisconnectId = FTMP.FogheToziDisconnectId
          INNER JOIN Tbl_MPFeeder MPF ON FTMP.MPFeederId = MPF.MPFeederId
          INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
          INNER JOIN #tempNeedArea A ON R.AreaId = A.AreaId
        WHERE R.DisconnectDatePersian BETWEEN @aFromDate AND @aToDate
        GROUP BY A.Area, MPP.MPPostName ,MPF.MPFeederId , MPF.MPFeederName
        
      UNION 
        
        SELECT A.Area
          ,MPP.MPPostName
          ,MPF.MPFeederName
          ,NULL AS LPPostName
          ,NULL AS LPFeederName
          ,R.IsDisconnectMPFeeder AS IsTotalDisconnect
          ,'MV_Feeder' AS OutageType 
          ,COUNT(*) AS Count 
          ,SUM(ISNULL(R.DisconnectInterval , 0)) AS DisconnectInterval
          ,SUM(case when R.IsTamir=1 then IsNull(R.DisconnectPower,0.0)
						else 0 end) AS DisconnectPowerPlanned

		  ,SUM(case when R.IsTamir=0 then IsNull(R.DisconnectPower,0.0)
						else 0 end) AS DisconnectPowerNotPlanned
          
        FROM TblRequest R
          INNER JOIN TblMPRequest MPR ON R.MPRequestId = MPR.MPRequestId
          INNER JOIN Tbl_DisconnectGroupSet DGSet ON R.DisconnectGroupSetId = DGSet.DisconnectGroupSetId
          INNER JOIN Tbl_DisconnectGroup G ON DGSet.DisconnectGroupId = G.DisconnectGroupId
          INNER JOIN Tbl_MPFeeder MPF ON MPR.MPFeederId = MPF.MPFeederId
          INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
          INNER JOIN #tempNeedArea A ON R.AreaId = A.AreaId
        WHERE R.DisconnectDatePersian BETWEEN @aFromDate AND @aToDate
           AND G.DisconnectGroupId IN (1002 , 1003)
        GROUP BY A.Area, MPP.MPPostName ,MPF.MPFeederId , MPF.MPFeederName ,R.IsDisconnectMPFeeder
      ) Res



      drop table #tempAreaIds
      drop table #tempNeedArea
END  
