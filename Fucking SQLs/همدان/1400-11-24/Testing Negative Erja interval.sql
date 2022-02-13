SELECT tblAll.AreaId
	,Area
	,ISNULL(tblLP.IntervalAvrage, 0) AS LPIntervalAvg
	,ISNULL(tblMP.IntervalAvrage, 0) AS MPIntervalAvg
	,ISNULL(tblFT.IntervalAvrage, 0) AS FTIntervalAvg
	,ISNULL(tblMoshtarek.IntervalAvrage, 0) AS MoshtarekIntervalAvgFROM(SELECT TblRequest.AreaId, Tbl_Area.Area FROM TblErjaRequest INNER JOIN TblRequest ON TblErjaRequest.RequestId = TblRequest.RequestId INNER JOIN Tbl_ErjaState ON TblErjaRequest.ErjaStateId = Tbl_ErjaState.ErjaStateId INNER JOIN Tbl_Area ON TblRequest.AreaId = Tbl_Area.AreaId WHERE TblErjaRequest.ErjaDatePersian >= '1400/01/01'
		AND TblErjaRequest.ErjaDatePersian <= '1400/11/24'
		AND TblErjaRequest.ErjaStateId = 4 GROUP BY TblRequest.AreaId, Tbl_Area.Area) AS tblAllLEFT
JOIN (
	SELECT TblRequest.AreaId
		,AVG(TblErjaRequest.ErjaInterval) AS IntervalAvrage
	FROM TblErjaRequest
	INNER JOIN TblRequest ON TblErjaRequest.RequestId = TblRequest.RequestId
	WHERE TblErjaRequest.NetworkTypeId IN (
			1
			,2
			)
	GROUP BY TblRequest.AreaId
		,TblErjaRequest.NetworkTypeId
	) AS tblMP ON tblMP.AreaId = tblALL.AreaIdLEFT
JOIN (
	SELECT TblRequest.AreaId
		,AVG(TblErjaRequest.ErjaInterval) AS IntervalAvrage
	FROM TblErjaRequest
	INNER JOIN TblRequest ON TblErjaRequest.RequestId = TblRequest.RequestId
	WHERE TblErjaRequest.NetworkTypeId IN (
			3
			,4
			)
	GROUP BY TblRequest.AreaId
		,TblErjaRequest.NetworkTypeId
	) AS tblLP ON tblLP.AreaId = tblAll.AreaIdLEFT
JOIN (
	SELECT TblRequest.AreaId
		,AVG(TblErjaRequest.ErjaInterval) AS IntervalAvrage
	FROM TblErjaRequest
	INNER JOIN TblRequest ON TblErjaRequest.RequestId = TblRequest.RequestId
	WHERE TblErjaRequest.NetworkTypeId = 5
	GROUP BY TblRequest.AreaId
		,TblErjaRequest.NetworkTypeId
	) AS tblFT ON tblFT.AreaId = tblAll.AreaIdLEFT
JOIN (
	SELECT TblRequest.AreaId
		,AVG(TblErjaRequest.ErjaInterval) AS IntervalAvrage
	FROM TblErjaRequest
	INNER JOIN TblRequest ON TblErjaRequest.RequestId = TblRequest.RequestId
	WHERE TblErjaRequest.NetworkTypeId = 6
	GROUP BY TblRequest.AreaId
		,TblErjaRequest.NetworkTypeId
	) AS tblMoshtarek ON tblMoshtarek.AreaId = tblAll.AreaIdORDER BY tblAll.AreaId 
  -----------------------------------------          
    SELECT TblRequest.AreaId , TblErjaRequest.NetworkTypeId		,AVG(TblErjaRequest.ErjaInterval) AS IntervalAvrage        FROM TblErjaRequest        INNER JOIN TblRequest ON TblErjaRequest.RequestId = TblRequest.RequestId       
      --WHERE TblErjaRequest.NetworkTypeId = 5        
        WHERE AreaId = 4        GROUP BY TblRequest.AreaId		,TblErjaRequest.NetworkTypeId  select * from Tbl_NetworkType
          
          
    SELECT count(E.ErjaRequestId) ,R.AreaId SELECT E.ErjaRequestId ,R.RequestNumber, A.Area ,E.ErjaDatePersian ,E.ErjaInterval FROM TblErjaRequest EINNER JOIN TblRequest R ON E.RequestId = R.RequestIdINNER JOIN Tbl_Area A ON A.AreaId = R.AreaIdwhere ErjaInterval < 0order by R.AreaId ,E.ErjaDatePersian desc--group by R.AreaId  40099230398
