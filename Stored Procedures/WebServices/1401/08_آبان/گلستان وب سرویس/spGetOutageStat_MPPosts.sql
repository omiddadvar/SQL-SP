Alter PROCEDURE spGetOutageStat_MPPosts
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
        ,CAST(NULL AS NVARCHAR(50)) AS MPFeederName
        ,CAST(NULL AS NVARCHAR(100)) AS LPPostName
        ,CAST(NULL AS NVARCHAR(50)) AS LPFeederName
        ,F.IsTotalMPPostDisconnected AS IsTotalDisconnect
        ,CAST('HV_Post' AS VARCHAR(20)) AS OutageType 
        ,COUNT(*) AS Count 
        ,SUM(ISNULL(R.DisconnectInterval , 0)) AS DisconnectInterval
        ,SUM(case when R.IsTamir=1 then IsNull(R.DisconnectPower,0)
						else 0 end) AS DisconnectPowerPlanned
	
	
		,SUM(case when R.IsTamir=0 then IsNull(R.DisconnectPower,0)
						else 0 end) AS DisconnectPowerNotPlanned
  
      FROM TblRequest R
        INNER JOIN TblFogheToziDisconnect F ON R.FogheToziDisconnectId = F.FogheToziDisconnectId
        INNER JOIN #tempNeedArea A on R.AreaId=A.AreaId
        INNER JOIN Tbl_MPPost MPP ON F.MPPostId = MPP.MPPostId
      WHERE F.IsFeederMode = 0
        AND R.DisconnectDatePersian BETWEEN @aFromDate AND @aToDate
	    GROUP BY A.AreaId ,A.Area, F.MPPostId ,MPP.MPPostName, F.IsTotalMPPostDisconnected
        ORDER BY A.Area
        
      drop table  #tempAreaIds
      drop table #tempNeedArea
      
END