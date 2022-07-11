ALTER PROCEDURE [dbo].[spGetReport_10_26_All] @FromDatePersian AS VARCHAR(10),
	@ToDatePersian AS VARCHAR(10),
	@AreaIDs AS VARCHAR(100),
	@IsDCMPFeeder AS INT,
	@OwnershipId AS INT,
	@IsWithFogheTozi AS INT,
	@IsTamir AS INT
AS
DECLARE @lSQL AS VARCHAR(8000)
DECLARE @lWhere AS VARCHAR(2000)
DECLARE @lWhereArea AS VARCHAR(1000)
DECLARE @lAllArea AS VARCHAR(500)
DECLARE @IsNotShowCenter AS BIT

SET @lWhereArea = ''
SET @lAllArea = ''
SET @IsNotShowCenter = 1

IF @FromDatePersian <> ''
	SET @lWhere = ' AND TblRequest.DisconnectDatePersian >= ''' + @FromDatePersian + ''''

IF @ToDatePersian <> ''
	SET @lWhere = @lWhere + ' AND TblRequest.DisconnectDatePersian <= ''' + @ToDatePersian + ''''

IF @AreaIDs <> ''
BEGIN
	SET @lWhere = @lWhere + ' AND TblRequest.AreaId IN (' + @AreaIDs + ')'
	SET @lWhereArea = ' WHERE AreaId IN (' + @AreaIDs + ')'
END

IF @IsDCMPFeeder = 1
	SET @lWhere = @lWhere + ' AND TblRequest.IsDisconnectMPFeeder = 1 '

IF @OwnershipId > - 1
	SET @lWhere = @lWhere + ' AND Tbl_MPFeeder.OwnershipId = ' + CAST(@OwnershipId AS VARCHAR(10))

IF @IsWithFogheTozi = 0
	SET @lWhere = @lWhere + ' AND (TblMPRequest.DisconnectReasonId IS NULL OR NOT (TblMPRequest.DisconnectReasonId >= 1200 AND TblMPRequest.DisconnectReasonId <= 1299)) AND (TblMPRequest.DisconnectGroupSetId IS NULL OR NOT (TblMPRequest.DisconnectGroupSetId = 1129 OR TblMPRequest.DisconnectGroupSetId = 1130)) '

IF @IsWithFogheTozi = 1
	SET @lWhere = @lWhere + ' AND (TblMPRequest.DisconnectReasonId IS NOT NULL AND (TblMPRequest.DisconnectReasonId >= 1200 AND TblMPRequest.DisconnectReasonId <= 1299) OR TblMPRequest.DisconnectGroupSetId IS NOT NULL AND (TblMPRequest.DisconnectGroupSetId = 1129 OR TblMPRequest.DisconnectGroupSetId = 1130))'

IF @IsTamir = 0
	SET @lWhere = @lWhere + ' AND TblRequest.IsTamir = 0 '

IF @IsTamir = 1
	SET @lWhere = @lWhere + ' AND TblRequest.IsTamir = 1 '

IF @lWhere <> ''
	SET @lWhere = ' WHERE ' + RIGHT(@lWhere, (LEN(@lWhere) - 4))

CREATE TABLE #tblTmpRequest (RequestId BIGINT)

CREATE TABLE #tblTmpArea (AreaId INT)

SET @lSQL = 'SELECT TblRequest.RequestId FROM TblRequest INNER JOIN TblMPRequest ON TblRequest.MPRequestId = TblMPRequest.MPRequestId INNER JOIN Tbl_MPFeeder ON TblMPRequest.MPFeederId = Tbl_MPFeeder.MPFeederId ' + @lWhere + ' AND TblRequest.EndJobStateId IN (2,3) AND ISNULL(TblMPRequest.IsWarmLine,0) = 0 '

INSERT INTO #tblTmpRequest
EXEC (@lSQL)

SET @lSQL = 'SELECT AreaId FROM Tbl_Area ' + @lWhereArea

INSERT INTO #tblTmpArea
EXEC (@lSQL)

IF @lWhereArea <> ''
BEGIN
	SET @lAllArea = @AreaIDs
	SET @IsNotShowCenter = 1
END
ELSE
BEGIN
	SET @lAllArea = '99'
	SET @IsNotShowCenter = 0
END

DECLARE @lAllSubCount AS FLOAT = 0

SELECT @lSQL = dbo.GetSubscriberCount2(@lAllArea, @ToDatePersian, @IsNotShowCenter)

CREATE TABLE #tblTmpSubscriberCount (TotalSub FLOAT)

INSERT INTO #tblTmpSubscriberCount
EXEC (@lsql)

SELECT @lAllSubCount = NULLIF(TotalSub, 0)
FROM #tblTmpSubscriberCount

DROP TABLE #tblTmpSubscriberCount

DECLARE @lLPPostCount AS INT = 0

SELECT @lSQL = dbo.GetAllLPPostCount(@lAllArea, 0)

CREATE TABLE #tblTmpLPPost (LPPostCount INT)

INSERT INTO #tblTmpLPPost
EXEC (@lsql)

SELECT @lLPPostCount = NULLIF(LPPostCount, 0)
FROM #tblTmpLPPost

DROP TABLE #tblTmpLPPost

DECLARE @lLastAllCurrentValue AS INT = 0

SELECT @lSQL = dbo.GetLastAllCurrentValue(@lAllArea, @ToDatePersian)

CREATE TABLE #tblTmpLastAllCurrentValue (LastCurrentValue FLOAT)

INSERT INTO #tblTmpLastAllCurrentValue
EXEC (@lsql)

SELECT @lLastAllCurrentValue = NULLIF(LastCurrentValue, 0)
FROM #tblTmpLastAllCurrentValue

DROP TABLE #tblTmpLastAllCurrentValue

SELECT ISNULL(ROUND(SUM(t1.SAIFI_Sub) / NULLIF(@lAllSubCount,0), 4),0) AS SAIFI_Sub,
	ISNULL(ROUND(SUM(t5.SAIFI_Amper / NULLIF(@lLastAllCurrentValue, 0)), 4),0) AS SAIFI_Amper,
	ISNULL(ROUND(SUM(t1.SAIDI_Sub) / NULLIF(@lAllSubCount,0), 4),0) AS SAIDI_Sub,
	ISNULL(ROUND(SUM(t5.SAIDI_Amper / NULLIF(@lLastAllCurrentValue, 0)), 4),0) AS SAIDI_Amper,
	ISNULL(ROUND(SUM(t2.T_SAIFI_Trans) / NULLIF(SUM(LPPostCount), 0), 4),0) AS T_SAIFI_Trans,
	ISNULL(ROUND(SUM(t6.T_SAIFI_Amper) / NULLIF(@lLPPostCount,0), 4),0) AS T_SAIFI_Amper,
	ISNULL(ROUND(SUM(t2.T_SAIDI_Trans) / NULLIF(SUM(LPPostCount), 0), 4),0) AS T_SAIDI_Trans,
	ISNULL(ROUND(SUM(t6.T_SAIDI_Amper) / NULLIF(@lLPPostCount,0), 4),0) AS T_SAIDI_Amper,
	ISNULL(ROUND((SUM(t1.SAIDI_Sub) / NULLIF(@lAllSubCount,0)),0) / NULLIF((SUM(t1.SAIFI_Sub) / NULLIF(@lAllSubCount,0)), 0), 4) AS CAIDI,
	ISNULL(ROUND(SUM(t3.MAIFI) / NULLIF(@lAllSubCount,0), 4),0) AS MAIFI,
	ISNULL(ROUND(SUM(t7.ENS), 4),0) AS ENS,
	ISNULL(ROUND(SUM(t4.AENS) / NULLIF(@lAllSubCount,0), 4),0) AS AENS
FROM Tbl_Area
LEFT JOIN (
	SELECT TblRequest.AreaId,
		SUM(CAST(isnull(TblMPRequest.GISDCSubscriberCount, isnull(TblRequestInfo.DCSubscriberCount, 0)) AS FLOAT)) AS SAIFI_Sub,
		SUM(CAST(TblRequest.DisconnectInterval * isnull(TblMPRequest.GISDCSubscriberCount, isnull(TblRequestInfo.DCSubscriberCount, 0)) AS FLOAT)) AS SAIDI_Sub
	FROM TblRequest
	INNER JOIN TblRequestInfo ON TblRequest.RequestId = TblRequestInfo.RequestId
	INNER JOIN TblMPRequest ON TblRequest.MPRequestId = TblMPRequest.MPRequestId
	WHERE TblRequest.RequestId IN (
			SELECT RequestId
			FROM #tblTmpRequest
			)
		AND TblRequest.DisconnectInterval >= 5
	GROUP BY TblRequest.AreaId
	) t1 ON Tbl_Area.AreaId = t1.AreaId
LEFT JOIN (
	SELECT Tbl_Area.AreaId,
		LPPostDCCount AS T_SAIFI_Trans,
		DisconnectInterval * LPPostDCCount AS T_SAIDI_Trans,
		LPPostCount
	FROM Tbl_Area
	INNER JOIN (
		SELECT TblRequest.AreaId,
			SUM(CASE 
					WHEN TblMPRequest.IsTotalLPPostDisconnected = 1
						THEN 1
					WHEN NOT tMSC.MPMultistepConnectionId IS NULL
						THEN dbo.GetMultiStepPostCount(tMSC.MPrequestId)
					WHEN TblRequest.IsDisconnectMPFeeder = 1
						THEN dbo.GetLPPostCount(TblMPRequest.MPFeederId, 0)
					ELSE TblMPRequest.CurrentValue * dbo.GetLPPostCount(TblMPRequest.MPFeederId, 0) / NULLIF(dbo.GetLastMPFeederCurrentValue(TblMPRequest.MPFeederId, TblMPRequest.DisconnectDatePersian), 0)
					END) AS LPPostDCCount,
			SUM(TblRequest.DisconnectInterval) AS DisconnectInterval
		FROM TblRequest
		INNER JOIN TblMPRequest ON TblRequest.MPRequestId = TblMPRequest.MPRequestId
		LEFT JOIN (
			SELECT MPRequestId,
				MAX(TblMultistepConnection.MPMultistepConnectionId) AS MPMultistepConnectionId
			FROM TblMultistepConnection
			WHERE NOT ISNULL(IsNotConnect, 0) = 1
			GROUP BY MPRequestId,
				IsNotConnect
			) tMSC ON TblMPRequest.MPRequestId = tMSC.MPRequestId
		WHERE TblRequest.RequestId IN (
				SELECT RequestId
				FROM #tblTmpRequest
				)
			AND TblRequest.DisconnectInterval >= 5
		GROUP BY TblRequest.AreaId
		) t1 ON Tbl_Area.AreaId = t1.AreaId
	INNER JOIN (
		SELECT AreaId,
			COUNT(*) LPPostCount
		FROM Tbl_LPPost
		WHERE IsActive = 1
		GROUP BY AreaId
		) t2 ON Tbl_Area.AreaId = t2.AreaId
	) t2 ON Tbl_Area.AreaId = t2.AreaId
LEFT JOIN (
	SELECT TblRequest.AreaId,
		SUM(CAST(isnull(TblMPRequest.GISDCSubscriberCount, isnull(TblRequestInfo.DCSubscriberCount, 0)) AS FLOAT)) * COUNT(TblRequest.RequestId) AS MAIFI
	FROM TblRequest
	INNER JOIN TblRequestInfo ON TblRequest.RequestId = TblRequestInfo.RequestId
	INNER JOIN TblMPRequest ON TblRequest.MPRequestId = TblMPRequest.MPRequestId
	WHERE TblRequest.RequestId IN (
			SELECT RequestId
			FROM #tblTmpRequest
			)
		AND TblRequest.DisconnectInterval < 5
	GROUP BY TblRequest.AreaId
	) t3 ON Tbl_Area.AreaId = t3.AreaId
LEFT JOIN (
	SELECT TblRequest.AreaId,
		SUM(TblRequest.DisconnectPower) AS AENS
	FROM TblRequest
	INNER JOIN TblRequestInfo ON TblRequest.RequestId = TblRequestInfo.RequestId
	INNER JOIN TblMPRequest ON TblRequest.MPRequestId = TblMPRequest.MPRequestId
	WHERE TblRequest.RequestId IN (
			SELECT RequestId
			FROM #tblTmpRequest
			)
	GROUP BY TblRequest.AreaId
	) t4 ON Tbl_Area.AreaId = t4.AreaId
LEFT JOIN (
	SELECT TblRequest.AreaId,
		SUM(CASE 
				WHEN TblMPRequest.IsTotalLPPostDisconnected = 1
					THEN TblRequest.DisconnectPower * 60 * 1000 / NULLIF((29.444 * TblRequest.DisconnectInterval), 0)
				ELSE TblMPRequest.CurrentValue
				END) AS SAIFI_Amper,
		SUM((
				CASE 
					WHEN TblMPRequest.IsTotalLPPostDisconnected = 1
						THEN TblRequest.DisconnectPower * 60 * 1000 / NULLIF((29.444 * TblRequest.DisconnectInterval), 0)
					ELSE TblMPRequest.CurrentValue
					END * TblRequest.DisconnectInterval
				)) AS SAIDI_Amper,
		SUM(dbo.GetLastAreaCurrentValue(TblRequest.AreaId, @ToDatePersian)) AS LastCurrentValue
	FROM TblRequest
	INNER JOIN TblMPRequest ON TblRequest.MPRequestId = TblMPRequest.MPRequestId
	WHERE TblRequest.RequestId IN (
			SELECT RequestId
			FROM #tblTmpRequest
			)
		AND TblRequest.DisconnectInterval >= 5
	GROUP BY TblRequest.AreaId
	) t5 ON Tbl_Area.AreaId = t5.AreaId
LEFT JOIN (
	SELECT Tbl_Area.AreaId,
		tTk.tk AS T_SAIFI_Amper,
		(tTk.tk * tTk.DisconnectInterval) AS T_SAIDI_Amper
	FROM Tbl_Area
	INNER JOIN (
		SELECT TblRequest.AreaId,
			SUM((
					CASE 
						WHEN TblMPRequest.IsTotalLPPostDisconnected = 1
							THEN TblRequest.DisconnectPower * 60 * 1000 / NULLIF((29.444 * TblRequest.DisconnectInterval), 0)
						ELSE TblMPRequest.CurrentValue
						END * dbo.GetLPPostCount(TblMPRequest.MPFeederId, 0)
					) / NULLIF(dbo.GetLastMPFeederCurrentValue(TblMPRequest.MPFeederId, TblMPRequest.DisconnectDatePersian), 0)) AS tk,
			SUM(TblRequest.DisconnectInterval) AS DisconnectInterval
		FROM TblRequest
		INNER JOIN TblMPRequest ON TblRequest.MPRequestId = TblMPRequest.MPRequestId
		WHERE TblRequest.RequestId IN (
				SELECT RequestId
				FROM #tblTmpRequest
				)
			AND TblRequest.DisconnectInterval >= 5
		GROUP BY TblRequest.AreaId
		) tTk ON Tbl_Area.AreaId = tTk.AreaId
	) t6 ON Tbl_Area.AreaId = t6.AreaId
LEFT JOIN (
	SELECT TblRequest.AreaId,
		SUM(TblMPRequest.DisconnectPower) AS ENS
	FROM TblRequest
	INNER JOIN TblMPRequest ON TblRequest.MPRequestId = TblMPRequest.MPRequestId
	WHERE TblRequest.RequestId IN (
			SELECT RequestId
			FROM #tblTmpRequest
			)
	GROUP BY TblRequest.AreaId
	) t7 ON Tbl_Area.AreaId = t7.AreaId
WHERE Tbl_Area.AreaId IN (
		SELECT AreaId
		FROM #tblTmpArea
		)

DROP TABLE #tblTmpRequest

DROP TABLE #tblTmpArea


