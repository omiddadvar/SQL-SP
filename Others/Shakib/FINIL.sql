USE CCRequesterSetad
GO
/*spGetReport_LPPostLoadBalance*/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spGetReport_LPPostLoadBalance]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spGetReport_LPPostLoadBalance]
GO

CREATE PROCEDURE [dbo].[spGetReport_LPPostLoadBalance] 
	@aLPPostCode AS NVARCHAR(100),
	@aFrom AS VARCHAR(10),
	@aTo AS VARCHAR(10)
AS
	SELECT 
		 Area.Area
		,ISNULL(Post.OwnershipId, 2) AS Ownership
		,MPost.MPPostName
		,MFeeder.MPFeederName
		,Post.LPPostName
		,ISNULL(Post.Address, '') AS Address
		,Post.LPPostCode
		,ISNULL(Post.LocalCode, '') AS LocalCode
		,dbo.KHSH_GetPercentANDLegalCurrent(TEMP.K, Temp_Area.K, Alt.DecreaseFactor, Alt_Area.DecreaseFactor, PL.PostCapacity, NULL) AS Nominal
		,PL.PostCapacity
		,PL.RCurrent
		,PL.SCurrent
		,PL.TCurrent
		,PL.NolCurrent
		,PL.PostPeakCurrent AS Average
		,ISNULL(PL.EarthValue, 0) AS EarthValue
		,PostType.LPPostType
		,Post.LPFeederCount
		,CASE 
			WHEN PL.IsTakFaze = 0
				THEN dbo.KHSH_UnbalancedIndicatorA(PL.RCurrent, PL.SCurrent, PL.TCurrent, PL.NolCurrent)
			ELSE 0
			END AS IndicatorA
		,CASE 
			WHEN PL.IsTakFaze = 0
				THEN dbo.KHSH_UnbalancedIndicatorB(PL.NolCurrent, PL.PostCapacity)
			ELSE 0
			END AS IndicatorB
		,PL.LoadDateTimePersian AS LoadDate
		,PL.LoadTime
		,dbo.KHSH_GetPercentANDLegalCurrent(TEMP.K, Temp_Area.K, Alt.DecreaseFactor, Alt_Area.DecreaseFactor, PL.PostCapacity, PL.PostPeakCurrent) AS CurrentPercent
	FROM 
		TblLPPostLoad PL
		INNER JOIN Tbl_LPPost Post ON Post.LPPostId = PL.LPPostId
		INNER JOIN Tbl_LPPostType PostType ON PostType.LPPostTypeId = Post.LPPostTypeId
		INNER JOIN Tbl_Area Area ON Area.AreaId = Post.AreaId
		INNER JOIN Tbl_MPFeeder MFeeder ON MFeeder.MPFeederId = POst.MPFeederId
		INNER JOIN Tbl_MPPost MPost ON MPost.MPPostId = MFeeder.MPPostId
		LEFT JOIN Tbl_Temperature TEMP ON TEMP.TemperatureId = Post.TemperatureId
		LEFT JOIN Tbl_Temperature Temp_Area ON Temp_Area.TemperatureId = Area.TemperatureId
		LEFT JOIN Tbl_Altitude Alt ON alt.AltitudeId = Post.AltitudeId
		LEFT JOIN Tbl_Altitude Alt_Area ON Alt_Area.AltitudeId = Post.AltitudeId
	WHERE 
		(
			Post.LPPostCode = @aLPPostCode
			OR @aLPPostCode = ''
		)
		AND PL.LoadDateTimePersian BETWEEN @aFrom AND @aTo
	ORDER BY 
		PL.LoadDateTimePersian DESC
GO

/*spGetLPFeeders_Multiple*/
create PROCEDURE [dbo].[spGetLPFeeders_Multiple] @aAreaIds varchar(4000), @aLPPostIds varchar(4000), @IsAll bit
AS 
	SELECT * INTO #AreaIds_tmp FROM dbo.Split(@aAreaIds , ',')
	SELECT * INTO #LPPostIds_tmp FROM dbo.Split(@aLPPostIds , ',')
	
	SELECT * 
	FROM Tbl_LPFeeder 
	INNER JOIN #AreaIds_tmp Area_t ON Tbl_LPFeeder.AreaId = CAST(Area_t.Item AS INT)
	INNER JOIN #LPPostIds_tmp LPPost_t ON Tbl_LPFeeder.LPPostId = CAST(LPPost_t.Item AS INT)
	WHERE 
		(
			(AreaId In (Area_t.Item)) 
			OR (LPFeederId IN 
				(SELECT Tbl_LPCommonFeeder.LPFeederId 
				FROM Tbl_LPCommonFeeder INNER JOIN Tbl_LPFeeder ON Tbl_LPCommonFeeder.LPFeederId = Tbl_LPFeeder.LPFeederId 
				WHERE Tbl_LPCommonFeeder.AreaId In (Area_t.Item) and (@IsAll=1 or IsActive=1))
				) 
		) 
		AND (LPPostId In (LPPost_t.Item) OR LPPost_t.Item='')
		AND (@IsAll=1 or IsActive=1)
		
		DROP TABLE #AreaIds_tmp
		DROP TABLE #LPPostIds_tmp
GO		


/*spGetLPPosts_Multiple*/
create PROCEDURE [dbo].[spGetLPPosts_Multiple] @aAreaIds nvarchar(4000), @aMPFeederIds nvarchar(4000), @IsAll bit
AS 
	SELECT * INTO #AreaIds_tmp FROM dbo.Split(@aAreaIds , ',')
	SELECT * INTO #MPFeederIds_temp FROM dbo.Split(@aMPFeederIds , ',')
	
	SELECT *, LPPostName + ' - ' + LPPostCode as LPPostNameCode
	FROM Tbl_LPPost 
	INNER JOIN #AreaIds_tmp Area_t ON Tbl_LPPost.AreaId = CAST(Area_t.Item AS INT)
	INNER JOIN #MPFeederIds_temp MPFeeder_t ON Tbl_LPPost.MPFeederId = CAST(MPFeeder_t.Item AS INT)
	
	WHERE 
		(
			(AreaId In (Area_t.Item)) 
			OR (LPPostId IN 
				(SELECT LPPostId 
				FROM Tbl_LPFeeder 
				WHERE AreaId In (Area_t.Item) and (@IsAll=1 or IsActive=1))
				) 
			OR (LPPostId IN 
				(SELECT Tbl_LPFeeder.LPPostId 
				FROM Tbl_LPCommonFeeder INNER JOIN Tbl_LPFeeder ON Tbl_LPCommonFeeder.LPFeederId = Tbl_LPFeeder.LPFeederId 
				WHERE Tbl_LPCommonFeeder.AreaId In (Area_t.Item) and (@IsAll=1 or IsActive=1))
				) 
		) 
		AND (MPFeederId In (MPFeeder_t.Item) OR MPFeeder_t.Item='')
		AND (@IsAll=1 or IsActive=1)
	ORDER BY 
		LPPostName
		
		DROP TABLE #AreaIds_tmp
		DROP TABLE #MPFeederIds_temp
GO		

/*spGetMPFeeders_Multiple*/
Create PROCEDURE [dbo].[spGetMPFeeders_Multiple] 
	@aAreaIds varchar(4000), 
	@aMPPostIds varchar(4000), 
	@IsAll bit 
AS 
	SELECT * INTO #AreaIds_tmp FROM dbo.Split(@aAreaIds , ',')
	SELECT * INTO #MPPostIds_tmp FROM dbo.Split(@aMPPostIds , ',')


	SELECT * 
	FROM Tbl_MPFeeder 
	INNER JOIN #AreaIds_tmp Area_t ON Tbl_MPFeeder.AreaId = CAST(Area_t.Item AS INT)
	INNER JOIN #MPPostIds_tmp MPPost_t ON Tbl_MPFeeder.MPPostId = CAST(MPPost_t.Item AS INT)
	WHERE 
		( 
			(AreaId In (Area_t.Item) OR @aAreaIds='') 
			OR ( MPFeederId IN (SELECT MPFeederId FROM Tbl_LPPost WHERE (AreaId In (Area_t.Item)) AND (@IsAll=1 OR IsActive=1)) ) 
			OR ( MPFeederId IN (SELECT Tbl_MPCommonFeeder.MPFeederId FROM Tbl_MPCommonFeeder INNER JOIN Tbl_MPFeeder ON Tbl_MPCommonFeeder.MPFeederId = Tbl_MPFeeder.MPFeederId WHERE (Tbl_MPCommonFeeder.AreaId In (Area_t.Item)) AND (@IsAll=1 OR Tbl_MPFeeder.IsActive=1)) ) 
			OR ( MPFeederId IN (SELECT MPFeederId FROM Tbl_LPPost WHERE
				(LPPostId IN 
					(SELECT LPPostId 
					FROM Tbl_LPFeeder 
					WHERE AreaId In (Area_t.Item) AND (@IsAll=1 OR IsActive=1))
				)
				OR (LPPostId IN 
					(SELECT Tbl_LPFeeder.LPPostId 
					FROM Tbl_LPCommonFeeder INNER JOIN Tbl_LPFeeder ON Tbl_LPCommonFeeder.LPFeederId = Tbl_LPFeeder.LPFeederId 
					WHERE Tbl_LPCommonFeeder.AreaId In (Area_t.Item) AND (@IsAll=1 OR IsActive=1))
				)
			))
		)
		AND (MPPostId In (MPPost_t.Item) OR MPPost_t.Item='') 
		AND (@IsAll=1 OR IsActive=1)
		
		DROP TABLE #AreaIds_tmp
		DROP TABLE #MPPostIds_tmp
GO		
	
		
/*spGetMPPosts_Multiple*/
Create PROCEDURE [dbo].[spGetMPPosts_Multiple] @aAreaIds varchar(4000),
	@IsAll BIT,
	@aWhereClause VARCHAR(4000),
	@aWhereClauseOR VARCHAR(4000)
AS
DECLARE @lSQL NVARCHAR(4000)

SET @lSQL = 'SELECT * FROM Tbl_MPPost WHERE ( (' + Cast(@IsAll AS NVARCHAR) + '=1 or IsActive=1) AND MPPostId IN ( SELECT MPPostId FROM Tbl_MPPost WHERE ( (AreaId In (' + @aAreaIds + ') OR ' + +''''+@aAreaIds+''''+  ' = '''') OR (MPPostId IN (SELECT DISTINCT MPPostId FROM Tbl_MPFeeder WHERE AreaId In (' + @aAreaIds + ') and (' + Cast(@IsAll AS NVARCHAR) + '=1 or IsActive=1)) ) OR (MPPostId IN (SELECT DISTINCT Tbl_MPFeeder.MPPostId FROM Tbl_MPFeeder INNER JOIN Tbl_LPPost ON Tbl_MPFeeder.MPFeederId = Tbl_LPPost.MPFeederId WHERE Tbl_LPPost.AreaId In (' + @aAreaIds + ') and (' + Cast(@IsAll AS NVARCHAR) + '=1 or Tbl_LPPost.IsActive=1)) ) OR (MPPostId IN (SELECT Tbl_MPCommonPost.MPPostId FROM Tbl_MPCommonPost INNER JOIN Tbl_MPPost ON Tbl_MPCommonPost.MPPostId = Tbl_MPPost.MPPostId WHERE Tbl_MPCommonPost.AreaId In (' + @aAreaIds + ') and (' + Cast(@IsAll AS NVARCHAR) + 
	'=1 or Tbl_MPPost.IsActive=1)) ) OR (MPPostId IN (SELECT Tbl_MPFeeder.MPPostId FROM Tbl_MPCommonFeeder INNER JOIN Tbl_MPFeeder ON Tbl_MPCommonFeeder.MPFeederId = Tbl_MPFeeder.MPFeederId WHERE Tbl_MPCommonFeeder.AreaId In (' + @aAreaIds + ') and (' + Cast(@IsAll AS NVARCHAR) + '=1 or Tbl_MPFeeder.IsActive=1)) ) ))'

IF @aWhereClause <> ''
	SET @lSQL = @lSQL + ' AND ( ' + @aWhereClause + ' )'
SET @lSQL = @lSQL + ')'

IF @aWhereClauseOR <> ''
	SET @lSQL = @lSQL + ' OR ( ' + @aWhereClauseOR + ' )'
SET @lSQL = @lSQL + ' UNION SELECT * FROM Tbl_MPPost WHERE ( (' + Cast(@IsAll AS NVARCHAR) + '=1 or IsActive=1) AND (MPPostId IN (SELECT Tbl_MPFeeder.MPPostId FROM Tbl_MPFeeder WHERE ( MPFeederId IN (SELECT MPFeederId FROM Tbl_LPPost WHERE (LPPostId IN (SELECT LPPostId FROM Tbl_LPFeeder WHERE AreaId In (' + @aAreaIds + ') and (' + Cast(@IsAll AS NVARCHAR) + '=1 or IsActive=1)) ) OR (LPPostId IN (SELECT Tbl_LPFeeder.LPPostId FROM Tbl_LPCommonFeeder INNER JOIN Tbl_LPFeeder ON Tbl_LPCommonFeeder.LPFeederId = Tbl_LPFeeder.LPFeederId WHERE Tbl_LPCommonFeeder.AreaId In (' + @aAreaIds + ') and (' + Cast(@IsAll AS NVARCHAR) + '=1 or IsActive=1)) ) )) )) '

IF @aWhereClause <> ''
	SET @lSQL = @lSQL + ' AND ( ' + @aWhereClause + ' )'
SET @lSQL = @lSQL + ')'

IF @aWhereClauseOR <> ''
	SET @lSQL = @lSQL + ' OR ( ' + @aWhereClauseOR + ' )'
print @lSQL
EXEC (@lSQL)
GO

/*ViewRequestInformMonitoring*/		
CREATE VIEW [dbo].[ViewRequestInformMonitoring]
AS 
SELECT 
	TblRequest.RequestId, 
	TblRequest.AreaId, 
	ISNULL(TblRequestInform.RequestInformJobStateId, 1) AS RequestInformJobStateId, 
	ISNULL(Tbl_RequestInformJobState.RequestInformJobState, 'جديد') AS RequestInformJobState, 
	TblRequest.IsTamir, 
	TblRequest.DisconnectDatePersian, 
	DisconnectTime = 
		Case TblRequest.isTamir 
			when 1 then TblRequest.TamirDisconnectFromTime 
			else TblRequest.DisconnectTime 
		end, 
	DCInterval = 
		Case TblRequest.isTamir 
			when 1 then CAST( datediff(mi, TblRequest.TamirDisconnectFromDT, TblRequest.TamirDisconnectToDT) AS nvarchar(100) ) 
			else CAST(datediff(mi, Getdate(), TblMPRequest.EstimateDT) AS nvarchar(100)) 
		end, 
	TblRequestInform.RequestInformId, 
	TblRequest.EndJobStateId, 
	TblRequest.RequestNumber, 
	TblRequest.Address, 
	TblRequest.ExtraComments, 
	Tbl_Area.Area,
	TblMPRequest.MPFeederId,
	Tbl_MPFeeder.MPFeederName,
	Tbl_MPPost.MPPostId,
	Tbl_MPPost.MPPostName,
	ISNULL(TblMPRequest.LPPostId,TblLPRequest.LPPostId) AS LPPostId,
	ISNULL(tMPLPPost.LPPostName,tLPLPPost.LPPostName) AS LPPostName,
	TblLPRequest.LPFeederId,
	Tbl_LPFeeder.LPFeederName
FROM 
	TblRequest 
	LEFT OUTER JOIN TblMPRequest ON TblRequest.MPRequestId = TblMPRequest.MPRequestId 
	LEFT OUTER JOIN TblLPRequest ON TblRequest.LPRequestId = TblLPRequest.LPRequestId 
	LEFT OUTER JOIN TblRequestInform ON TblRequest.RequestId = TblRequestInform.RequestId 
	LEFT OUTER JOIN Tbl_RequestInformJobState ON TblRequestInform.RequestInformJobStateId = Tbl_RequestInformJobState.RequestInformJobStateId 
	LEFT OUTER JOIN Tbl_Area ON TblRequest.AreaId = Tbl_Area.AreaId 
	LEFT JOIN Tbl_MPFeeder ON TblMPRequest.MPFeederId = Tbl_MPFeeder.MPFeederId
	LEFT JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId
	LEFT JOIN TblTamirRequestConfirm ON TblRequest.RequestId = TblTamirRequestConfirm.RequestId
	LEFT JOIN TblTamirRequest ON TblTamirRequestConfirm.TamirRequestId = TblTamirRequest.TamirRequestId 
	LEFT JOIN TblTamirRequest TblTamirRequest2 ON TblRequest.RequestId = TblTamirRequest2.EmergencyRequestId
	LEFT JOIN Tbl_LPPost tMPLPPost on TblMPRequest.LPPostId = tMPLPPost.LPPostId
	LEFT JOIN Tbl_LPPost tLPLPPost on TblLPRequest.LPPostId = tLPLPPost.LPPostId
	LEFT JOIN Tbl_LPFeeder on TblLPRequest.LPFeederId = Tbl_LPFeeder.LPFeederId
WHERE 
	 /*( 
		(TblRequest.IsTamir = 1 AND TblRequest.IsLPRequest = 0) 
		OR (TblRequest.IsTamir = 0 AND TblRequest.IsMPRequest = 1) 
	) 
	AND*/ ( NOT TblRequest.EndJobStateId IN (9,7,1,8,6))
	AND ISNULL(TblMPRequest.IsWarmLine,0) = 0
	AND ISNULL(TblTamirRequest.IsWarmLine,0) = 0
GO












/*spGetReportOfPersonelPerformance_Raw*/

CREATE PROCEDURE spGetReportOfPersonelPerformance_Raw 
	@FromDate VARCHAR(10)
	,@ToDate VARCHAR(10)
	,@Areas VARCHAR(4000) = ''
	,@AreaUsers VARCHAR(4000) = ''
AS
BEGIN
	DECLARE @lHaveArea AS BIT = 
			CASE WHEN NULLIF(@Areas, '') IS NOT NULL THEN 1 ELSE 0 END
	DECLARE @lHaveAreaUser AS BIT = 
			CASE WHEN NULLIF(@AreaUsers, '') IS NOT NULL THEN 1 ELSE 0 END

	/* Request IDs */
	SELECT RequestId 
	INTO #tmpRequest 
	FROM TblRequest 
	WHERE DisconnectDatePersian 
	BETWEEN @FromDate AND @ToDate
	
	

	SELECT *
	INTO #AreasTemp
	FROM dbo.Split(@Areas, ',')


	SELECT *
	INTO #AreaUsersTemp
	FROM dbo.Split(@AreaUsers, ',')
	
	/* محاسبه تعداد خاموشی های بی برنامه */
	SELECT AreaUserId
		,Count(*) CntNotPlanned
	INTO #temp1
	FROM TblRequest R
	INNER JOIN #tmpRequest T ON R.RequestId = T.RequestId
	WHERE IsTamir = 0
		AND (
			@lHaveAreaUser = 0
			OR 
			R.AreaUserId IN (
				SELECT Item
				FROM #AreaUsersTemp
				)
			)
	GROUP BY AreaUserId
	
	/* محاسبه تعداد خاموشی های برنامه ریزی شده */
	SELECT AreaUserId
		,Count(*) CntPlanned
	INTO #temp2
	FROM TblRequest R
	INNER JOIN #tmpRequest T ON R.RequestId = T.RequestId
	WHERE IsTamir = 1
		AND (
			@lHaveAreaUser = 0
			OR R.AreaUserId IN (
				SELECT Item
				FROM #AreaUsersTemp
				)
			)
	GROUP BY AreaUserId

	/* محاسبه تعداد خاموشی های روشنایی معابر */
	SELECT AreaUserId
		,Count(*) CntIsLightRequest
	INTO #temp3
	FROM TblRequest R
	INNER JOIN #tmpRequest T ON R.RequestId = T.RequestId
	WHERE IsLightRequest = 1
		AND (
			@lHaveAreaUser = 0
			OR AreaUserId IN (
				SELECT Item
				FROM #AreaUsersTemp
				)
			)
	GROUP BY AreaUserId

	/* محاسبه تعداد خاموشی های ارجا داده شده */
	SELECT AreaUserId
		,COUNT(*) CntErja
	INTO #temp4
	FROM TblRequest R
	INNER JOIN #tmpRequest T ON R.RequestId = T.RequestId
	WHERE EndJobStateId = 9
		AND (
			@lHaveAreaUser = 0
			OR R.AreaUserId IN (
				SELECT Item
				FROM #AreaUsersTemp
				)
			)
	GROUP BY AreaUserId

	/* محاسبه تعداد خاموشی های غیر مرتبط */
	SELECT AreaUserId
		,COUNT(*) CntNotRelated
	INTO #temp5
	FROM TblRequest R
	INNER JOIN #tmpRequest T ON R.RequestId = T.RequestId
	WHERE R.IsNotRelated = 1
		AND (
			@lHaveAreaUser = 0
			OR R.AreaUserId IN (
				SELECT Item
				FROM #AreaUsersTemp
				)
			)
	GROUP BY AreaUserId

	/* محاسبه تعداد خاموشی تکراری ثبت شده */
	SELECT AreaUserId
		,COUNT(*) CntDuplicatedRequest
	INTO #temp6
	FROM TblRequest R
	INNER JOIN #tmpRequest T ON R.RequestId = T.RequestId
	WHERE IsDuplicatedRequest = 1
		AND (
			@lHaveAreaUser = 0
			OR R.AreaUserId IN (
				SELECT Item
				FROM #AreaUsersTemp
				)
			)
	GROUP BY AreaUserId

	/* محاسبه تعداد شماره اشتراک
 یا شناسه قبض ثبت نشده
 در تماسهای ورودی */
	SELECT DISTINCT Req.AreaUserId
		,COUNT(*) AS CntNotRecordedSubscriberCode
	INTO #temp7
	FROM TblRequest Req
	INNER JOIN #tmpRequest T ON Req.RequestId = T.RequestId
	INNER JOIN TblRequestInfo ReqInfo ON Req.RequestId = ReqInfo.RequestId
	WHERE (NULLIF(ReqInfo.SubscriberCode, '') IS NULL)
		AND (
			@lHaveAreaUser = 0
			OR AreaUserId IN ( SELECT Item FROM #AreaUsersTemp )
			)
	GROUP BY Req.AreaUserId
	
	
	/* محاسبه مدت زمان تکمیل پرونده */
	SELECT DISTINCT Req.AreaUserId
		,SUM(CASE 
				WHEN ISNULL(MPReq.RequestDEInterval - MPReq.DisconnectInterval, MPReq.RequestDEInterval - MPReq.DisconnectInterval) > 0
					THEN ISNULL(MPReq.RequestDEInterval - MPReq.DisconnectInterval, MPReq.RequestDEInterval - MPReq.DisconnectInterval)
				ELSE 0
				END) AS DelayIntervalOperator
	INTO #temp8
	FROM TblRequest Req
	INNER JOIN #tmpRequest T ON Req.RequestId = T.RequestId
	INNER JOIN TblRequestInfo ReqInfo ON Req.RequestId = ReqInfo.RequestId
	LEFT JOIN TblMPRequest MPReq ON Req.MPRequestId = MPReq.MPRequestId
	LEFT JOIN TblLPRequest LPReq ON Req.LPRequestId = LPReq.LPRequestId
	WHERE (NULLIF(ReqInfo.SubscriberCode, '') IS NULL)
		AND (
			@lHaveAreaUser = 0
			OR Req.AreaUserId IN (
				SELECT Item
				FROM #AreaUsersTemp
				)
			)
	GROUP BY Req.AreaUserId
	
	/*-------------- Ultimate Result --------------*/
	SELECT DISTINCT AU.AreaUserId
		,A.Area
		,AU.FirstName
		,AU.LastName
		,AU.UserName
		,ISNULL(#temp1.CntNotPlanned , 0) AS CntNotPlanned
		,ISNULL(#temp2.CntPlanned , 0) AS CntPlanned
		,ISNULL(#temp3.CntIsLightRequest , 0) AS CntIsLightRequest
		,ISNULL(#temp4.CntErja , 0) AS CntErja
		,ISNULL(#temp5.CntNotRelated , 0) AS CntNotRelated
		,ISNULL(#temp6.CntDuplicatedRequest , 0) AS CntDuplicatedRequest
		,ISNULL(#temp7.CntNotRecordedSubscriberCode , 0) AS CntNotRecordedSubscriberCode
		,ISNULL(#temp8.DelayIntervalOperator , 0) AS DelayIntervalOperator
	FROM Tbl_AreaUser AU
	LEFT JOIN #temp1 ON AU.AreaUserId = #temp1.AreaUserId
	LEFT JOIN #temp2 ON AU.AreaUserId = #temp2.AreaUserId
	LEFT JOIN #temp3 ON AU.AreaUserId = #temp3.AreaUserId
	LEFT JOIN #temp4 ON AU.AreaUserId = #temp4.AreaUserId
	LEFT JOIN #temp5 ON AU.AreaUserId = #temp5.AreaUserId
	LEFT JOIN #temp6 ON AU.AreaUserId = #temp6.AreaUserId
	LEFT JOIN #temp7 ON AU.AreaUserId = #temp7.AreaUserId
	LEFT JOIN #temp8 ON AU.AreaUserId = #temp8.AreaUserId
	LEFT JOIN Tbl_Area A ON Au.AreaId = A.AreaId
	WHERE (
			@lHaveArea = 0
			OR AU.AreaId IN ( SELECT Item FROM #AreasTemp )
			)
		AND (
			@lHaveAreaUser = 0
			OR AU.AreaUserId IN ( SELECT Item FROM #AreaUsersTemp )
			)
			
	DROP TABLE #AreasTemp
	DROP TABLE #AreaUsersTemp
	DROP TABLE #temp1
	DROP TABLE #temp2
	DROP TABLE #temp3
	DROP TABLE #temp4
	DROP TABLE #temp5
	DROP TABLE #temp6
	DROP TABLE #temp7
	DROP TABLE #temp8
	Drop Table #tmpRequest

END

		
GO

CREATE PROCEDURE spGetReportOfPersonelPerformance @FromDate VARCHAR(10)
	,@ToDate VARCHAR(10)
	,@Areas VARCHAR(4000) = ''
	,@AreaUsers VARCHAR(4000) = ''
AS
BEGIN
	/* Raw-Data Table Structure */
	CREATE TABLE #tempRaw (
		AreaUserId INT
		,Area VARCHAR(200)
		,FirstName VARCHAR(200)
		,LastName VARCHAR(200)
		,UserName VARCHAR(200)
		,CntNotPlanned INT
		,CntPlanned INT
		,CntIsLightRequest INT
		,CntErja INT
		,CntNotRelated INT
		,CntDuplicatedRequest INT
		,CntNotRecordedSubscriberCode INT
		,DelayIntervalOperator INT
		);
	
	INSERT #tempRaw
	EXEC spGetReportOfPersonelPerformance_Raw @FromDate
		,@ToDate
		,@Areas
		,@AreaUsers

	DECLARE @MaxTotalRegistration AS INT
		,@MaxSubscriptionNumberRegistrationPerformance AS FLOAT
		,@MaxDelaysInRegistration AS FLOAT
		,@MaxDelaysInRegistration3 AS FLOAT
		,@AVGOperatorPerformanceInlateDelaysRegistration AS FLOAT
		,@AVGOvertime_30_50_20 AS FLOAT
		,@AVGOvertimeBasedOnSystemPerformance AS INT
		,@AVGFinalOvertime_withAverage20 AS INT
	
	/* Result Table Structure */
	CREATE TABLE #tempEachRow (
		AreaUserId INT
		,Area VARCHAR(200)
		,FirstName VARCHAR(200)
		,LastName VARCHAR(200)
		,UserName VARCHAR(200)
		,CntNotPlanned INT
		,CntPlanned INT
		,CntIsLightRequest INT
		,CntErja INT
		,CntNotRelated INT
		,CntDuplicatedRequest INT
		,TotalRegistration INT
		,CoefficientOfRegistration FLOAT
		,NotRegistredSubscription INT
		,SubscriptionNumberRegistrationPerformance FLOAT
		,DelaysInRegistration INT
		,DelaysInRegistration2 FLOAT
		,DelaysInRegistration3 FLOAT
		,OperatorPerformanceInlateDelaysRegistration FLOAT
		,Overtime_30_50_20 FLOAT
		,OvertimeBasedOnSystemPerformance INT
		,FinalOvertime_withAverage20 INT
		,[Description] VARCHAR(500)
		);


	INSERT #tempEachRow (
		AreaUserId
		,Area
		,FirstName
		,LastName
		,UserName
		,CntNotPlanned
		,CntPlanned
		,CntIsLightRequest
		,CntErja
		,CntNotRelated
		,CntDuplicatedRequest
		,TotalRegistration
		,NotRegistredSubscription
		,DelaysInRegistration
		)
	SELECT AreaUserId
		,Area
		,FirstName
		,LastName
		,UserName
		,CntNotPlanned
		,CntPlanned
		,CntIsLightRequest
		,CntErja
		,CntNotRelated
		,CntDuplicatedRequest
		,CntNotPlanned + CntPlanned + CntIsLightRequest + CntErja + CntNotRelated + CntDuplicatedRequest AS TotalRegistration
		,CntNotRecordedSubscriberCode
		,DelayIntervalOperator
	FROM #tempRaw

	
	SET @MaxTotalRegistration = (
			SELECT MAX(TotalRegistration)
			FROM #tempEachRow
			)
	/* ضریب تعداد ثبت و
	 عملکرد ثبت شماره اشتراک یا شناسه قبض  در تماسهای ورودی
	 */
	UPDATE #tempEachRow
	SET CoefficientOfRegistration = 
	CASE WHEN  ISNULL((1.00*TotalRegistration - 2) / NULLIF(1.00*@MaxTotalRegistration - 2,0),0) <0
		THEN 0
	ELSE 	
		ISNULL((1.00*TotalRegistration - 2) / NULLIF(1.00*@MaxTotalRegistration - 2,0),0)
	END	
		,SubscriptionNumberRegistrationPerformance = ISNULL(1 - (1.00* NotRegistredSubscription / NULLIF(TotalRegistration,0)),0)
	FROM #tempEachRow

	SET @MaxDelaysInRegistration = (
			SELECT MAX(DelaysInRegistration)
			FROM #tempEachRow
			)


	UPDATE #tempEachRow
	SET DelaysInRegistration2 = 
			CASE WHEN ISNULL((1.00 * DelaysInRegistration - 1) / (NULLIF(1.00*@MaxDelaysInRegistration,1) - 1),0) < 0
			THEN 0
			ELSE ISNULL((1.00 * DelaysInRegistration - 1) / (NULLIF(1.00*@MaxDelaysInRegistration,1) - 1),0)
			END
		,DelaysInRegistration3 = 
			CASE WHEN ISNULL(1 / NULLIF(((ISNULL(1.00* DelaysInRegistration,1) - 1) / (NULLIF(1.00*
			@MaxDelaysInRegistration,1) - 1)),0),0) < 0
			THEN 0
			ELSE ISNULL(1 / NULLIF(((ISNULL(1.00* DelaysInRegistration,1) - 1) / (NULLIF(1.00*@MaxDelaysInRegistration,1) - 1)),0),0)	
			END				
	FROM #tempEachRow

	SET @MaxDelaysInRegistration3 = (
			SELECT MAX(DelaysInRegistration3)
			FROM #tempEachRow
			)

	/* عملکرد اپراتور در
 تاخیر در ثبت ها */
	UPDATE #tempEachRow
	SET OperatorPerformanceInlateDelaysRegistration =
			CASE WHEN ISNULL((1.00*DelaysInRegistration3 - 1) / NULLIF(1.00*@MaxDelaysInRegistration3 - 1,0),0) < 0
			THEN 0
			ELSE
			ISNULL((1.00*DelaysInRegistration3 - 1) / NULLIF(1.00*@MaxDelaysInRegistration3 - 1,0),0)
			END							 

	FROM #tempEachRow

	/* محاسبه اضافه کار بر اساس 30درصد در ثبت و 50 در صد در ثبت شناسه قبض و 20 درصد در تاخیرات */
	UPDATE #tempEachRow
	SET Overtime_30_50_20 = CoefficientOfRegistration * 0.3 + SubscriptionNumberRegistrationPerformance * 0.5 + OperatorPerformanceInlateDelaysRegistration * 0.2
	FROM #tempEachRow

	SET @AVGOvertime_30_50_20 = (
			SELECT AVG(Overtime_30_50_20)
			FROM #tempEachRow
			)

	/* اضافه کار محاسبه شده بر اساس عملکرد سیستمی  */
	UPDATE #tempEachRow
	SET OvertimeBasedOnSystemPerformance = ISNULL(((1.00 * Overtime_30_50_20 * 14) / NULLIF(1.00*@AVGOvertime_30_50_20,0)),0)
		,FinalOvertime_withAverage20 = ISNULL(((1.00 * Overtime_30_50_20 * 14) / NULLIF(1.00*@AVGOvertime_30_50_20,0)),0) + 6
	FROM #tempEachRow

	SET @MaxSubscriptionNumberRegistrationPerformance = (
			SELECT Max(SubscriptionNumberRegistrationPerformance)
			FROM #tempEachRow
			)
	SET @AVGOperatorPerformanceInlateDelaysRegistration = (
			SELECT AVG(OperatorPerformanceInlateDelaysRegistration)
			FROM #tempEachRow
			)
	SET @AVGOvertimeBasedOnSystemPerformance = (
			SELECT AVG(OvertimeBasedOnSystemPerformance)
			FROM #tempEachRow
			)
	SET @AVGFinalOvertime_withAverage20 = (
			SELECT AVG(FinalOvertime_withAverage20)
			FROM #tempEachRow
			)
			
			

	SELECT
	 	AreaUserId,
		Area,
		ISNULL(FirstName ,'') FirstName,
		ISNULL(LastName,'') LastName,
		UserName,
		CntNotPlanned,
		CntPlanned,
		CntIsLightRequest,
		CntErja,
		CntNotRelated,
		CntDuplicatedRequest,
		Round(TotalRegistration,5) TotalRegistration,
		Round(CoefficientOfRegistration,5) CoefficientOfRegistration,
		Round(NotRegistredSubscription,5) NotRegistredSubscription,
		Round(SubscriptionNumberRegistrationPerformance,5) SubscriptionNumberRegistrationPerformance,
		Round(DelaysInRegistration,5) DelaysInRegistration,
		Round(DelaysInRegistration2,5) DelaysInRegistration2,
		Round(DelaysInRegistration3,5) DelaysInRegistration3,
		Round(OperatorPerformanceInlateDelaysRegistration,3) OperatorPerformanceInlateDelaysRegistration,
		Round(Overtime_30_50_20,5) Overtime_30_50_20,
		Round(OvertimeBasedOnSystemPerformance,5) OvertimeBasedOnSystemPerformance,
		Round(FinalOvertime_withAverage20,3) FinalOvertime_withAverage20,
		[Description]
	FROM #tempEachRow


	/* Result */
	SELECT @MaxTotalRegistration MaxTotalRegistration
		,Round(@MaxSubscriptionNumberRegistrationPerformance,5) MaxSubscriptionNumberRegistrationPerformance
		,@MaxDelaysInRegistration MaxDelaysInRegistration
		,Round(@MaxDelaysInRegistration3,5) MaxDelaysInRegistration3
		,ROUND(@AVGOperatorPerformanceInlateDelaysRegistration,5) AVGOperatorPerformanceInlateDelaysRegistration
		,Round(@AVGOvertime_30_50_20,5) AVGOvertime_30_50_20
		,@AVGOvertimeBasedOnSystemPerformance AVGOvertimeBasedOnSystemPerformance
		,@AVGFinalOvertime_withAverage20 AVGFinalOvertime_withAverage20

	DROP TABLE #tempRaw

	DROP TABLE #tempEachRow
END
GO




















	
		



		