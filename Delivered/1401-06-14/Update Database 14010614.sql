ALTER FUNCTION Emergency.GetMPFeederLoadHourId (
	@MPFeederId AS INT
	,@aDisconnectDT AS DATETIME
	)
RETURNS BIGINT
AS
BEGIN
	DECLARE @lDayOfWeek AS INT = DATEPART(dw, @aDisconnectDT)
		,@lHour AS INT = DATEPART(HOUR, @aDisconnectDT) 
		,@lID AS BIGINT

	IF @lDayOfWeek IN (7,1,2,3,4)
	BEGIN
		SELECT TOP (1) @lID = H.MPFeederLoadHourId
		FROM Tbl_MPFeederLoad L
		INNER JOIN Tbl_MPFeederLoadHours H ON L.MPFeederLoadId = H.MPFeederLoadId
		WHERE L.MPFeederId = @MPFeederId
			AND H.HourId = @lHour
			AND L.RelDate <= @aDisconnectDT
			AND DATEPART(dw, L.RelDate) IN (7,1,2,3,4)
		ORDER BY L.RelDate DESC
	END
	ELSE
	BEGIN
		SELECT TOP (1) @lID = H.MPFeederLoadHourId
		FROM Tbl_MPFeederLoad L
		INNER JOIN Tbl_MPFeederLoadHours H ON L.MPFeederLoadId = H.MPFeederLoadId
		WHERE L.MPFeederId = @MPFeederId
			AND H.HourId = @lHour
			AND L.RelDate <= @aDisconnectDT
			AND DATEPART(dw, L.RelDate) = @lDayOfWeek
		ORDER BY L.RelDate DESC
	END

	RETURN @lID
END
GO
/*---------------------------------------------------------------------------*/
ALTER FUNCTION Emergency.GetMPFeederDisconnectCount (
	@MPFeederId AS INT
	,@IsDisconnectMPFeeder AS BIT
	,@MPFeederKeyId AS BIGINT,
  @aDayCount AS INT
	)
RETURNS INT
AS
BEGIN
	DECLARE @lCntMP1 AS INT = 0,
          @lCntMP2 AS INT = 0,
          @lCntFT AS INT = 0,
          @lDayLimit AS DATETIME = DATEADD(DAY, - @aDayCount ,CAST(CAST(GETDATE() as DATE) as DATETIME))

  IF @aDayCount IS NULL OR @aDayCount <= 0 BEGIN
    RETURN 0                                         	
  END

	SELECT @lCntMP1 = COUNT(*)
	FROM TblMPRequest
	INNER JOIN TblRequest ON TblMPRequest.MPRequestId = TblRequest.MPRequestId
	WHERE TblMPRequest.DisconnectDT >= @lDayLimit
		AND ISNULL(TblRequest.IsDisconnectMPFeeder, 1) = 1
		AND TblMPRequest.MPFeederId = @MPFeederId
    AND ISNULL(TblMPRequest.IsWarmLine , 0) = 0

	IF @IsDisconnectMPFeeder = 0
	BEGIN
		SELECT @lCntMP2 = COUNT(*)
		FROM TblMPRequest
		INNER JOIN TblRequest ON TblMPRequest.MPRequestId = TblRequest.MPRequestId
		INNER JOIN TblMPRequestKey ON TblMPRequest.MPRequestId = TblMPRequestKey.MPRequestId
		WHERE TblMPRequest.DisconnectDT >= @lDayLimit
			AND ISNULL(TblRequest.IsDisconnectMPFeeder, 1) = 0
			AND TblMPRequest.MPFeederId = @MPFeederId
			AND TblMPRequestKey.MPFeederKeyId = @MPFeederKeyId
      AND ISNULL(TblMPRequest.IsWarmLine , 0) = 0
	END

	SELECT @lCntFT = COUNT(*)
	FROM dbo.TblFogheToziDisconnect
	INNER JOIN TblRequest ON TblRequest.FogheToziDisconnectId = TblFogheToziDisconnect.FogheToziDisconnectId
	INNER JOIN dbo.TblFogheToziDisconnectMPFeeder ON TblFogheToziDisconnect.FogheToziDisconnectId = TblFogheToziDisconnectMPFeeder.FogheToziDisconnectId
		AND TblRequest.DisconnectDT >= @lDayLimit
		AND TblFogheToziDisconnectMPFeeder.MPFeederId = @MPFeederId

	RETURN ISNULL(@lCntFT, 0) + ISNULL(@lCntMP1, 0) + ISNULL(@lCntMP2, 0)
END
GO
/*------------------------------------------------------*/

ALTER PROCEDURE dbo.spTavanir_GetMPFeedersDisPower 
   @aFromDate AS VARCHAR(10)
	,@aFromTime AS VARCHAR(5)
	,@aToDate AS VARCHAR(10)
	,@aToTime AS VARCHAR(5)
AS
BEGIN
	DECLARE @lFromDate AS DATETIME = dbo.ShamsiDateTimeToMiladi(@aFromDate, @aFromTime)
	       ,@lToDate AS DATETIME = dbo.ShamsiDateTimeToMiladi(@aToDate, @aToTime)
	DECLARE @lNow AS DATETIME = GETDATE()
		     ,@lYesterday AS DATETIME = DATEADD(DAY, - 1, GETDATE())

	SELECT CAST(1 AS BIT) AS AllAreas
		,CAST(ISNULL(Round(SUM(CASE 
				WHEN R.IsDisconnectMPFeeder = 1
					THEN 1
				WHEN R.IsDisconnectMPFeeder = 0 AND MPR.IsNotDisconnectFeeder = 1
					THEN CASE 
							WHEN ISNULL((MPR.CurrentValue / NULLIF(MPR.PreCurrentValue, 0)), 0) > 1
								THEN 1
							ELSE ISNULL((MPR.CurrentValue / NULLIF(MPR.PreCurrentValue, 0)), 0)
							END
				ELSE 0
				END) , 0),0) AS INT) AS DisconnectFeederCount
	/*COUNT(DISTINCT MPR.MPFeederId) AS DisconnectFeederCount*/
	INTO #tmpDisFeederCount
	FROM TblRequest R
	INNER JOIN TblMPRequest MPR ON R.MPRequestId = MPR.MPRequestId
	WHERE (R.IsDisconnectMPFeeder = 1 OR MPR.IsNotDisconnectFeeder = 1)
		AND R.DisconnectDT BETWEEN @lFromDate AND @lToDate

	SELECT CAST(1 AS BIT) AS AllAreas
		/*,COUNT(DISTINCT LiveMPR.MPFeederId) AS LiveDisconnectFeederCount*/
		,CAST(ISNULL(Round(SUM(CASE 
				WHEN R.IsDisconnectMPFeeder = 1
					THEN 1
				WHEN R.IsDisconnectMPFeeder = 0 AND MPR.IsNotDisconnectFeeder = 1
					THEN CASE 
							WHEN ISNULL((MPR.CurrentValue / NULLIF(MPR.PreCurrentValue, 0)), 0) > 1
								THEN 1
							ELSE ISNULL((MPR.CurrentValue / NULLIF(MPR.PreCurrentValue, 0)), 0)
							END
				ELSE 0
				END) ,0),0) AS INT) AS LiveDisconnectFeederCount
	INTO #tmpLiveDisFeederCount
	FROM TblRequest R
	INNER JOIN TblMPRequest MPR ON R.MPRequestId = MPR.MPRequestId
	WHERE (R.IsDisconnectMPFeeder = 1 OR MPR.IsNotDisconnectFeeder = 1)
		AND MPR.EndJobStateId IN (4,5)
		AND MPR.EndJobStateId = R.EndJobStateId
		AND R.DisconnectDT BETWEEN @lYesterday AND @lNow

	SELECT CAST(1 AS BIT) AS AllAreas
		,ISNULL(ROUND(AVG(ISNULL(V.CurrentValue, 0)), 3), 0) AS CurrentValueAvg
		,ISNULL(AVG(ABS(ISNULL(R.DisconnectInterval, 0))), 0) AS DisconnectIntervalAvg
		,ISNULL(ROUND(SUM(ISNULL(V.DisconnectPower, 0)), 3), 0) AS DisconnectPowerSum
	INTO #tmpMonitoring
	FROM ViewMonitoring V
	INNER JOIN TblRequest R ON V.RequestId = R.RequestId
	INNER JOIN TblMPRequest MPR ON R.MPRequestId = MPR.MPRequestId
	WHERE R.DisconnectDT BETWEEN @lFromDate AND @lToDate
		AND (R.IsDisconnectMPFeeder = 1 OR MPR.IsNotDisconnectFeeder = 1)

	SELECT M.CurrentValueAvg
		,CAST(M.DisconnectIntervalAvg / 60 AS VARCHAR(5)) + ':' + CAST(M.DisconnectIntervalAvg % 60 AS VARCHAR(2)) AS DisconnectIntervalAvg
		,M.DisconnectPowerSum
		,C.DisconnectFeederCount
		,L.LiveDisconnectFeederCount
	FROM #tmpMonitoring M
	INNER JOIN #tmpDisFeederCount C ON M.AllAreas = C.AllAreas
	INNER JOIN #tmpLiveDisFeederCount L ON M.AllAreas = C.AllAreas

	DROP TABLE #tmpMonitoring
	DROP TABLE #tmpDisFeederCount
	DROP TABLE #tmpLiveDisFeederCount
END
GO
/*---------------------------------------------------------------------*/
CREATE PROCEDURE spFindCurrentAndFutureOutageByBillingID
    @aFromDate AS VARCHAR(10),
    @aToDate AS VARCHAR(10),
    @aAreaId AS INT = -1,
    @aBillingId AS VARCHAR(1000) = ''
  AS
  BEGIN

    SELECT R.RequestId, 
      R.RequestNumber,
      R.AreaId,
      R.IsTamir,
      R.DisconnectDatePersian,
      R.DisconnectTime AS DisconnectTime,
      R.ConnectDatePersian AS ConnectDate,
      R.ConnectTime,
      R.DataEntryDTPersian,
      R.Address,
      AU.UserName,
      CASE WHEN R.IsTamir = 1 THEN ReasonFor.DisconnectLPRequestFor ELSE CR.CallReason END DisconnectGroupSet
    INTO  #tmpCurrent
    FROM TblRequest R
      INNER JOIN Tbl_AreaUser AU ON R.AreaUserId = AU.AreaUserId
      LEFT JOIN Tbl_CallReason CR ON R.CallReasonId = CR.CallReasonId
      LEFT JOIN Tbl_DisconnectLPRequestFor ReasonFor ON R.DisconnectRequestForId = ReasonFor.DisconnectLPRequestForId
    WHERE IsTamir = 0 AND 
          DisconnectDatePersian BETWEEN @aFromDate AND @aToDate
          AND R.EndJobStateId = 5
          AND (R.LPRequestId IS NOT NULL OR R.MPRequestId IS NOT NULL)
    
    SELECT R.RequestId, 
      R.RequestNumber,
      R.AreaId,
      R.IsTamir,
      R.TamirDisconnectFromDatePersian AS DisconnectDatePersian,
      R.TamirDisconnectFromTime AS DisconnectTime,
      R.TamirDisconnectToDatePersian AS ConnectDate,
      R.TamirDisconnectToTime AS ConnectTime,
      R.DataEntryDTPersian,
      Address,
      AU.UserName,
      Sub.TamirRequestSubject AS DisconnectGroupSet
    INTO  #tmpFuture
    FROM TblRequest R
      INNER JOIN Tbl_AreaUser AU ON R.AreaUserId = AU.AreaUserId
      INNER JOIN TblTamirRequestConfirm C ON R.RequestId = C.RequestId
      INNER JOIN TblTamirRequest TR ON C.TamirRequestId = TR.TamirRequestId
      LEFT JOIN Tbl_TamirRequestSubject Sub ON TR.TamirRequestSubjectId = Sub.TamirRequestSubjectId
    WHERE IsTamir = 1 AND 
          R.TamirDisconnectFromDatePersian BETWEEN @aFromDate AND @aToDate
          AND R.EndJobStateId = 4
    
    SELECT TEMP.* INTO #tmpReq FROM (
        SELECT * FROM #tmpCurrent 
          UNION ALL
        SELECT * FROM #tmpFuture
    ) TEMP
    
    DROP Table #tmpCurrent
    DROP Table #tmpFuture
    
    SELECT T.* ,A.Area
    FROM #tmpReq T
      INNER JOIN Tbl_Area A ON T.AreaId = A.AreaId
      LEFT JOIN TblRequestInfo I ON T.RequestId = I.RequestId
    WHERE (@aAreaId <= 0 OR A.AreaId = @aAreaId)
      AND (LEN(@aBillingId) = 0 OR I.BillingID = @aBillingId)
    ORDER BY T.DisconnectDatePersian DESC , T.DisconnectTime DESC
    
    
    DROP Table #tmpReq

  END
GO
/*--------------------------------------------------------------------------------*/
CREATE PROCEDURE spGetOutageStat_MPFeeders 
  @aFromDate AS VARCHAR(11),
  @aToDate AS VARCHAR(11),
  @aAreaIds AS VARCHAR(2000) = ''
AS
  BEGIN
    DECLARE @lSQL AS NVARCHAR(MAX) = '
       SELECT * FROM (
         SELECT A.Area
          ,MPP.MPPostName
          ,MPF.MPFeederName
          ,CAST(NULL AS NVARCHAR(100)) AS LPPostName
          ,CAST(NULL AS NVARCHAR(50)) AS LPFeederName
          ,CAST(1 AS BIT) AS IsTotalDisconnect
          ,CAST(''MV_Feeder'' AS VARCHAR(20)) AS OutageType 
          ,COUNT(*) AS Count 
          ,SUM(ISNULL(R.DisconnectInterval , 0)) AS DisconnectInterval
          ,SUM(ISNULL(R.DisconnectPower, 0.0)) AS DisconnectPower
        FROM TblRequest R
          INNER JOIN TblFogheToziDisconnect F ON R.FogheToziDisconnectId = F.FogheToziDisconnectId
          INNER JOIN TblFogheToziDisconnectMPFeeder FTMP ON F.FogheToziDisconnectId = FTMP.FogheToziDisconnectId
          INNER JOIN Tbl_MPFeeder MPF ON FTMP.MPFeederId = MPF.MPFeederId
          INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
          INNER JOIN Tbl_Area A ON R.AreaId = A.AreaId
        WHERE R.DisconnectDatePersian BETWEEN ''' + @aFromDate + ''' AND ''' + @aToDate + '''
        ' + CASE WHEN LEN(@aAreaIds) > 0  THEN ' AND A.AreaId IN (' + @aAreaIds + ')' ELSE '' END + 
        '
        GROUP BY A.Area, MPP.MPPostName ,MPF.MPFeederId , MPF.MPFeederName
        
      UNION 
        
        SELECT A.Area
          ,MPP.MPPostName
          ,MPF.MPFeederName
          ,NULL AS LPPostName
          ,NULL AS LPFeederName
          ,R.IsDisconnectMPFeeder AS IsTotalDisconnect
          ,''MV_Feeder'' AS OutageType 
          ,COUNT(*) AS Count 
          ,SUM(ISNULL(R.DisconnectInterval , 0)) AS DisconnectInterval
          ,SUM(ISNULL(R.DisconnectPower, 0.0)) AS DisconnectPower
        FROM TblRequest R
          INNER JOIN TblMPRequest MPR ON R.MPRequestId = MPR.MPRequestId
          INNER JOIN Tbl_DisconnectGroupSet DGSet ON R.DisconnectGroupSetId = DGSet.DisconnectGroupSetId
          INNER JOIN Tbl_DisconnectGroup G ON DGSet.DisconnectGroupId = G.DisconnectGroupId
          INNER JOIN Tbl_MPFeeder MPF ON MPR.MPFeederId = MPF.MPFeederId
          INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
          INNER JOIN Tbl_Area A ON R.AreaId = A.AreaId
        WHERE R.DisconnectDatePersian BETWEEN ''' + @aFromDate + ''' AND ''' + @aToDate + '''
          ' + CASE WHEN LEN(@aAreaIds) > 0  THEN ' AND A.AreaId IN (' + @aAreaIds + ')' ELSE '' END + 
          ' AND G.DisconnectGroupId IN (1002 , 1003)
        GROUP BY A.Area, MPP.MPPostName ,MPF.MPFeederId , MPF.MPFeederName ,R.IsDisconnectMPFeeder
      ) Res
      '
      PRINT(@lSQL)
      EXEC(@lSQL)
  END
 GO
/*-----------------------------------------------------------------------*/
CREATE PROCEDURE spGetOutageStat_LPPosts
  @aFromDate AS VARCHAR(11),
  @aToDate AS VARCHAR(11),
  @aAreaIds AS VARCHAR(2000) = ''
AS
  BEGIN
      DECLARE @lSQL AS NVARCHAR(MAX) = '
          SELECT * FROM (
            SELECT A.Area
              ,MPP.MPPostName
              ,MPF.MPFeederName
              ,LPP.LPPostName
              ,CAST(NULL AS NVARCHAR(50)) AS LPFeederName
              ,MPR.IsTotalLPPostDisconnected AS IsTotalDisconnect
              ,CAST(''LV_Post'' AS VARCHAR(20)) AS OutageType 
              ,COUNT(*) AS Count 
              ,SUM(ISNULL(R.DisconnectInterval , 0)) AS DisconnectInterval
              ,SUM(ISNULL(R.DisconnectPower, 0.0)) AS DisconnectPower
            FROM TblRequest R
              INNER JOIN TblMPRequest MPR ON R.MPRequestId = MPR.MPRequestId
              INNER JOIN Tbl_LPPost LPP ON MPR.LPPostId = LPP.LPPostId
              INNER JOIN Tbl_MPFeeder MPF ON LPP.MPFeederId = MPF.MPFeederId
              INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
              INNER JOIN Tbl_DisconnectGroupSet DGSet ON MPR.DisconnectGroupSetId = DGSet.DisconnectGroupSetId
              INNER JOIN Tbl_DisconnectGroup G ON DGSet.DisconnectGroupId = G.DisconnectGroupId
              INNER JOIN Tbl_Area A ON R.AreaId = A.AreaId
            WHERE R.DisconnectDatePersian BETWEEN ''' + @aFromDate + ''' AND ''' + @aToDate + '''
            ' + CASE WHEN LEN(@aAreaIds) > 0  THEN ' AND A.AreaId IN (' + @aAreaIds + ')' ELSE '' END + '
              AND G.DisconnectGroupId IN (1004 , 1005)
            GROUP BY A.Area, MPP.MPPostName ,MPF.MPFeederName, LPP.LPPostId ,LPP.LPPostName ,MPR.IsTotalLPPostDisconnected
            
          UNION
      
            SELECT A.Area
              ,MPP.MPPostName
              ,MPF.MPFeederName
              ,LPP.LPPostName
              ,NULL AS LPFeederName
              ,LPR.IsTotalLPPostDisconnected AS IsTotalDisconnect
              ,''LV_Post'' AS OutageType 
              ,COUNT(*) AS Count 
              ,SUM(ISNULL(R.DisconnectInterval , 0)) AS DisconnectInterval
              ,SUM(ISNULL(R.DisconnectPower, 0.0)) AS DisconnectPower
            FROM TblRequest R
              INNER JOIN TblLPRequest LPR ON R.LPRequestId = LPR.LPRequestId
              INNER JOIN Tbl_LPPost LPP ON LPR.LPPostId = LPP.LPPostId
              INNER JOIN Tbl_MPFeeder MPF ON LPP.MPFeederId = MPF.MPFeederId
              INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
              INNER JOIN Tbl_Area A ON R.AreaId = A.AreaId
            WHERE R.DisconnectDatePersian BETWEEN ''' + @aFromDate + ''' AND ''' + @aToDate + '''
            ' + CASE WHEN LEN(@aAreaIds) > 0  THEN ' AND A.AreaId IN (' + @aAreaIds + ')' ELSE '' END + '
              AND LPR.IsTotalLPPostDisconnected = 1
            GROUP BY A.Area, MPP.MPPostName ,MPF.MPFeederName, LPP.LPPostId ,LPP.LPPostName ,LPR.IsTotalLPPostDisconnected
          ) Res 
          '
      PRINT(@lSQL)
      EXEC(@lSQL)
  END
 Go
/*-------------------------------------------------------------*/
CREATE PROCEDURE spGetOutageStat_LPFeeders
    @aFromDate AS VARCHAR(11),
    @aToDate AS VARCHAR(11),
    @aAreaIds AS VARCHAR(2000) = ''
  AS
  BEGIN
      DECLARE @lSQL AS NVARCHAR(MAX) = ' 
        SELECT A.Area
          ,MPP.MPPostName
          ,MPF.MPFeederName
          ,LPP.LPPostName
          ,LPF.LPFeederName
          ,LPR.IsTotalLPFeederDisconnected AS IsTotalDisconnect
          ,CAST(''LV_Feeder'' AS VARCHAR(20)) AS OutageType 
          ,COUNT(*) AS Count 
          ,SUM(ISNULL(R.DisconnectInterval , 0)) AS DisconnectInterval
          ,SUM(ISNULL(R.DisconnectPower, 0.0)) AS DisconnectPower
        FROM TblRequest R
          INNER JOIN TblLPRequest LPR ON R.LPRequestId = LPR.LPRequestId
          INNER JOIN Tbl_LPFeeder LPF ON LPR.LPFeederId = LPF.LPFeederId
          INNER JOIN Tbl_LPPost LPP ON LPF.LPPostId = LPP.LPPostId
          INNER JOIN Tbl_MPFeeder MPF ON LPP.MPFeederId = MPF.MPFeederId
          INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
          INNER JOIN Tbl_Area A ON R.AreaId = A.AreaId
        WHERE R.DisconnectDatePersian BETWEEN ''' + @aFromDate + ''' AND ''' + @aToDate +'''
        ' + CASE WHEN LEN(@aAreaIds) > 0  THEN ' AND A.AreaId IN (' + @aAreaIds + ')' ELSE '' END + 
        ' AND LPR.IsTotalLPPostDisconnected = 0 
          AND ISNULL(LPR.IsSingleSubscriber , 0) = 0
        GROUP BY A.Area, MPP.MPPostName ,MPF.MPFeederName, LPP.LPPostName , 
                LPF.LPFeederId , LPF.LPFeederName ,LPR.IsTotalLPFeederDisconnected
        '
      PRINT(@lSQL)
      EXEC(@lSQL)
  END
 GO
/*---------------------------------------------------------------*/

CREATE PROCEDURE spGetOutageStat_MPPosts
  @aFromDate AS VARCHAR(11),
  @aToDate AS VARCHAR(11),
  @aAreaIds AS VARCHAR(2000) = ''
AS
  BEGIN
    DECLARE @lSQL AS NVARCHAR(MAX) = '
      SELECT A.Area
        ,MPP.MPPostName
        ,CAST(NULL AS NVARCHAR(50)) AS MPFeederName
        ,CAST(NULL AS NVARCHAR(100)) AS LPPostName
        ,CAST(NULL AS NVARCHAR(50)) AS LPFeederName
        ,F.IsTotalMPPostDisconnected AS IsTotalDisconnect
        ,CAST(''HV_Post'' AS VARCHAR(20)) AS OutageType 
        ,COUNT(*) AS Count 
        ,SUM(ISNULL(R.DisconnectInterval , 0)) AS DisconnectInterval
        ,SUM(ISNULL(R.DisconnectPower, 0.0)) AS DisconnectPower
      FROM TblRequest R
        INNER JOIN TblFogheToziDisconnect F ON R.FogheToziDisconnectId = F.FogheToziDisconnectId
        INNER JOIN Tbl_Area A ON R.AreaId = A.AreaId
        INNER JOIN Tbl_MPPost MPP ON F.MPPostId = MPP.MPPostId
      WHERE F.IsFeederMode = 0
        AND R.DisconnectDatePersian BETWEEN ''' + @aFromDate + ''' AND ''' + @aToDate +'''
      '+ CASE WHEN LEN(@aAreaIds) > 0  THEN ' AND A.AreaId IN (' + @aAreaIds + ')' ELSE '' END + 
      '
        GROUP BY A.AreaId ,A.Area, F.MPPostId ,MPP.MPPostName, F.IsTotalMPPostDisconnected
        ORDER BY A.Area
      '
    PRINT(@lSQL)
    EXEC(@lSQL)
  END
GO
/*----------------------------------------------------------*/
CREATE PROCEDURE spGetOutageStat 
        @aFromDate AS VARCHAR(11),
        @aToDate AS VARCHAR(11),
        @aAreaIds AS VARCHAR(2000) = ''
AS
  BEGIN
    CREATE TABLE #tmpReq 
    (
        Area NVARCHAR(50),
        MPPostName NVARCHAR(30),
        MPFeederName NVARCHAR(50),
        LPPostName NVARCHAR(100),
        LPFeederName NVARCHAR(50),
        IsTotalDisconnect BIT,
        OutageType VARCHAR(20),
        Count INT,
        DisconnectInterval INT,
        DisconnectPower FLOAT
    )
    
    
    INSERT INTO #tmpReq EXEC spGetOutageStat_MPPosts @aFromDate , @aToDate, @aAreaIds
    INSERT INTO #tmpReq EXEC spGetOutageStat_MPFeeders @aFromDate , @aToDate, @aAreaIds
    INSERT INTO #tmpReq EXEC spGetOutageStat_LPPosts @aFromDate , @aToDate, @aAreaIds
    INSERT INTO #tmpReq EXEC spGetOutageStat_LPFeeders @aFromDate , @aToDate, @aAreaIds
    
    SELECT * FROM #tmpReq 
    ORDER BY Area , OutageType , IsTotalDisconnect 
    
    DROP TABLE #tmpReq  	
  END
GO

/*--------------------------------------------------------------*/

ALTER procedure dbo.spSimaSaidi (@FromDate varchar(10), @ToDate varchar(10), @AreaIds varchar(1000) )
as 
	create table #tblArea (AreaId int)
	declare @lSQL as varchar(2000) = 'select AreaId from tbl_Area'
	if @AreaIds <> ''
		set @lSQL = @lSQL + ' where AreaId in (' + @AreaIds + ')'

	insert into #tblArea exec (@lSQL) 

	select RequestId, #tblArea.AreaId, IsTamir,MPRequestId, LPRequestId, TamirTypeId
	into #tmpReq
	from 
		TblRequest with(index(IX_MonitoringIndex))
		inner join #tblArea on TblRequest.AreaId = #tblArea.AreaId
	where 
		TblRequest.DisconnectDatePersian >= @FromDate
		AND TblRequest.DisconnectDatePersian <=  @ToDate
		AND EndJobStateId in (2,3)
		AND ISNULL(IsLightRequest,0) = 0

	SELECT t1.AreaId,
		ISNULL(sum(DisconnectPowerMP),0) AS SumPowerMP,
		ISNULL(sum(DisconnectPowerLP),0) AS SumPowerLP,
		ISNULL(SUM(DisconnectPowerTamir),0) AS SumPowerMPTamir,
		ISNULL(SUM(DisconnectPowerNotTamirPaydar),0) AS SumPowerMPNotTamirPaydar,
		ISNULL(SUM(DisconnectPowerNotTamirGozara),0) AS SumPowerMPNotTamirGozara,
		ISNULL(SUM(DisconnectPowerTamirBarnameh),0) AS SumPowerMPBarnameh,
		ISNULL(SUM(DisconnectPowerTamirMovafegh),0) AS SumPowerMPMovafegh,
		ISNULL(SUM(DisconnectPowerTamirEzterar),0) AS SumPowerMPEzterar,
		ISNULL(SUM(DisconnectPowerTamirUnknown),0) as SumPowerMPUnknown,
		ISNULL(SUM(cntDCMP),0) as cntDCMP,
		ISNULL(SUM(cntDCMPPaydar),0) as cntDCMPPaydar,
		ISNULL(SUM(cntDCMPGozara),0) as cntDCMPGozara,
		ISNULL(SUM(cntDCMPBarnameh),0) as cntDCMPBarnameh,
		ISNULL(SUM(cntDCMPMovafegh),0) as cntDCMPMovafegh,
		ISNULL(SUM(cntDCMPEzterar),0) as cntDCMPEzterar,
		ISNULL(SUM(cntDCMPUnknown),0) as cntDCMPUnknown,
		ISNULL(sum(DisconnectPowerLPTamir),0) AS SumPowerLPTamir,
		ISNULL(sum(DisconnectPowerLPNotTamir),0) AS SumPowerLPNotTamir
	FROM (
		SELECT #tmpReq.AreaId,
			0 AS DisconnectPowerLP,
			SUM(TblMPRequest.DisconnectPower) AS DisconnectPowerMP,
			SUM(CASE WHEN #tmpReq.IsTamir = 1 THEN TblMPRequest.DisconnectPower END) AS DisconnectPowerTamir,
			SUM(CASE WHEN #tmpReq.IsTamir = 0 AND TblMPRequest.DisconnectInterval > 5 THEN TblMPRequest.DisconnectPower END) AS DisconnectPowerNotTamirPaydar,
			SUM(CASE WHEN #tmpReq.IsTamir = 0 AND TblMPRequest.DisconnectInterval <= 5 THEN TblMPRequest.DisconnectPower END) AS DisconnectPowerNotTamirGozara,
			SUM(CASE WHEN #tmpReq.IsTamir = 1 AND #tmpReq.TamirTypeId = 1 THEN TblMPRequest.DisconnectPower END) AS DisconnectPowerTamirBarnameh,
			SUM(CASE WHEN #tmpReq.IsTamir = 1 AND #tmpReq.TamirTypeId = 2 THEN TblMPRequest.DisconnectPower END) AS DisconnectPowerTamirMovafegh,
			SUM(CASE WHEN #tmpReq.IsTamir = 1 AND #tmpReq.TamirTypeId = 3 THEN TblMPRequest.DisconnectPower END) AS DisconnectPowerTamirEzterar,
			SUM(CASE WHEN #tmpReq.IsTamir = 1 AND #tmpReq.TamirTypeId is null THEN TblMPRequest.DisconnectPower END) AS DisconnectPowerTamirUnknown,
			count(#tmpReq.RequestId) AS cntDCMP,
			SUM(CASE WHEN #tmpReq.IsTamir = 0 AND TblMPRequest.DisconnectInterval > 5 THEN 1 END) AS cntDCMPPaydar,
			SUM(CASE WHEN #tmpReq.IsTamir = 0 AND TblMPRequest.DisconnectInterval <= 5 THEN 1 END) AS cntDCMPGozara,
			SUM(CASE WHEN #tmpReq.IsTamir = 1 AND #tmpReq.TamirTypeId = 1 THEN 1 END) AS cntDCMPBarnameh,
			SUM(CASE WHEN #tmpReq.IsTamir = 1 AND #tmpReq.TamirTypeId = 2 THEN 1 END) AS cntDCMPMovafegh,
			SUM(CASE WHEN #tmpReq.IsTamir = 1 AND #tmpReq.TamirTypeId = 3 THEN 1 END) AS cntDCMPEzterar,
			SUM(CASE WHEN #tmpReq.IsTamir = 1 AND #tmpReq.TamirTypeId is null THEN 1 END) AS cntDCMPUnknown,
			0 as DisconnectPowerLPTamir,
			0 as DisconnectPowerLPNotTamir
			
		FROM #tmpReq
		INNER JOIN TblMPRequest ON #tmpReq.MPRequestId = TblMPRequest.MPRequestId
		INNER JOIN Tbl_DisconnectGroupSet ON TblMPRequest.DisconnectGroupSetId = Tbl_DisconnectGroupSet.DisconnectGroupSetId
		INNER JOIN Tbl_Area ON #tmpReq.AreaId = Tbl_Area.AreaId
		LEFT JOIN Tbl_MPFeeder ON TblMPRequest.MPFeederId = Tbl_MPFeeder.MPFeederId
		WHERE 
			
			ISNULL(TblMPRequest.IsWarmLine, 0) = 0
			AND (
				(
					TblMPRequest.DisconnectReasonId IS NULL
					OR TblMPRequest.DisconnectReasonId < 1200
					OR TblMPRequest.DisconnectReasonId > 1299
					)
				AND (
					TblMPRequest.DisconnectGroupSetId IS NULL
					OR TblMPRequest.DisconnectGroupSetId <> 1129
					AND TblMPRequest.DisconnectGroupSetId <> 1130
					)
				)
			AND TblMPRequest.EndJobStateId IN (2, 3)
			AND ISNULL(Tbl_MPFeeder.OwnershipId,2) = 2
		GROUP BY
			#tmpReq.AreaId
			
		UNION
		
		SELECT #tmpReq.AreaId,
			SUM(TblLPRequest.DisconnectPower) / 1000 AS DisconnectPowerLP,
			0 AS DisconnectPowerMP,
			0 as DisconnectPowerTamir,
			0 as DisconnectPowerNotTamirPaydar,
			0 as DisconnectPowerNotTamirGozara,
			0 as DisconnectPowerTamirBarnameh,
			0 as DisconnectPowerTamirMovafegh,
			0 as DisconnectPowerTamirEzterar,
			0 as DisconnectPowerTamirUnknown,
			0 AS cntDCMP,
			0 AS cntDCMPPaydar,
			0 AS cntDCMPGozara,
			0 AS cntDCMPBarnameh,
			0 AS cntDCMPMovafegh,
			0 AS cntDCMPEzterar,
			0 AS cntDCMPUnknown,
			SUM(case when #tmpReq.IsTamir = 1 then TblLPRequest.DisconnectPower END) / 1000 AS DisconnectPowerLPTamir,
			SUM(case when #tmpReq.IsTamir = 0 then TblLPRequest.DisconnectPower END) / 1000 AS DisconnectPowerLPNotTamir

		FROM #tmpReq
		INNER JOIN TblLPRequest ON #tmpReq.LPRequestId = TblLPRequest.LPRequestId
		INNER JOIN Tbl_DisconnectGroupSet ON TblLPRequest.DisconnectGroupSetId = Tbl_DisconnectGroupSet.DisconnectGroupSetId
    LEFT JOIN Tbl_LPPost LPP ON TblLPRequest.LPPostId = LPP.LPPostId
    LEFT JOIN Tbl_LPFeeder LPF ON TblLPRequest.LPFeederId = LPF.LPFeederId
		INNER JOIN Tbl_Area ON #tmpReq.AreaId = Tbl_Area.AreaId
		WHERE 
			ISNULL(TblLPRequest.IsWarmLine, 0) = 0
			AND TblLPRequest.EndJobStateId IN (2, 3)
			AND ISNULL(TblLPRequest.IsSingleSubscriber, 0) = 0
			AND ISNULL(TblLPRequest.IsLightRequest, 0) = 0
      AND ISNULL(LPF.OwnershipId , ISNULL(LPP.OwnershipId , 2)) = 2
		GROUP BY
			#tmpReq.AreaId
		) as t1
	GROUP BY 
		t1.AreaId
		
	drop table #tmpReq
	drop table #tblArea
GO