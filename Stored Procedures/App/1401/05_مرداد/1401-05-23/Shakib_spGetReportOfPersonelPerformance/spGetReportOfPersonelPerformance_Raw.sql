
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