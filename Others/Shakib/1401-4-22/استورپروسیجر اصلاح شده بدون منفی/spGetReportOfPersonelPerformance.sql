--Main sp
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
