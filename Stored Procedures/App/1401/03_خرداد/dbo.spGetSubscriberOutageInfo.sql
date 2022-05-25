ALTER PROCEDURE dbo.spGetSubscriberOutageInfo
	@CallerId as varchar(20),
	@BackTime as int
AS
	DECLARE @lMPPostId as INT
	DECLARE @lMPFeederId as INT
	DECLARE @lLPPostId as INT
	DECLARE @lLPFeederId as INT
	DECLARE @lFeederPartId as INT
	DECLARE @lRequestId as BIGINT
	DECLARE @lFeederMode as bit
	DECLARE @lSubscriberId as INT
	DECLARE @lDuplicatedRequestId as BIGINT
	DECLARE @lEndJobStateId as INT
	DECLARE @lIsNotSubscriber as BIT
	
	SET @lMPPostId = NULL
	SET @lMPFeederId = NULL
	SET @lLPPostId = NULL
	SET @lLPFeederId = NULL
	SET @lFeederPartId = NULL
	SET @lRequestId = NULL
	SET @lFeederMode = 0
	SET @lSubscriberId = NULL
	SET @lIsNotSubscriber = 1
	
  SET @BackTime = ISNULL(NULLIF(@BackTime ,0),180) /* Null or 0 => 180 */
	
	DECLARE @lBackDate AS datetime = DATEADD(mi,-@BackTime,GETDATE())
	
	
	SELECT TOP 1 
		@lRequestId = RequestId,
		@lDuplicatedRequestId = DuplicatedRequestId,
		@lEndJobStateId = EndJobStateId
	FROM 
		TblRequest 
	WHERE
		TblRequest.DisconnectDT > @lBackDate
		AND RIGHT(Telephone,8) = RIGHT(@CallerId,8)
	ORDER BY
		TblRequest.DisconnectDT DESC

	IF @lRequestId IS NULL
	BEGIN
		
		SELECT 
			CAST(0 AS INT) AS Status,
			NULL AS IsTamir, 
			NULL AS DisconnectDate, 
			NULL AS DisconnectTime, 
			NULL AS ConnectDate, 
			NULL AS ConnectTime,
			CAST(0 AS FLOAT) AS DebtPrice
		RETURN
	END
	
	
	IF @lEndJobStateId = 1
	
	BEGIN
		SELECT 
			@lRequestId = RequestId
		FROM
			TblRequest
		WHERE
			RequestId = @lDuplicatedRequestId
	END
	
	SELECT
		STATUS = 
			CASE WHEN TblRequest.EndJobStateId = 2
				
				THEN CAST(1 AS INT)
			WHEN TblRequest.EndJobStateId = 3
				
				THEN CAST(1 AS INT)
			WHEN TblRequest.EndJobStateId = 4
				
				THEN CAST(2 AS INT)
			WHEN TblRequest.EndJobStateId = 5
				
				THEN CAST(3 AS INT)
			WHEN TblRequest.EndJobStateId = 6
				
				THEN CAST(0 AS INT)
			WHEN TblRequest.EndJobStateId = 8
				
				THEN CAST(4 AS INT)
			WHEN TblRequest.EndJobStateId = 9
				
				THEN CAST(5 AS INT)
			WHEN TblRequest.EndJobStateId = 10
				
				THEN CAST(6 AS INT)
			END,
		TblRequest.IsTamir,
		TblRequest.DisconnectDatePersian AS DisconnectDate,
		TblRequest.DisconnectTime,
		ConnectDate = CASE 
			WHEN IsTamir = 1
				THEN TblRequest.TamirDisconnectToDatePersian
			WHEN NOT EstimateDT IS NULL
				THEN dbo.mtosh(EstimateDT)
		END,
		ConnectTime = CASE 
			WHEN IsTamir = 1 THEN TblRequest.TamirDisconnectToTime
			WHEN NOT EstimateDT IS NULL THEN 
				CASE 
					WHEN LEN(DATEPART(hh, EstimateDT)) = 1 THEN '0' + CAST(DATEPART(hh, EstimateDT) AS VARCHAR)
					ELSE CAST(DATEPART(hh, EstimateDT) AS VARCHAR)
				END 
				+ ':' + 
				CASE 
					WHEN LEN(DATEPART(mi,EstimateDT)) = 1 THEN '0' + CAST(DATEPART(mi,EstimateDT) AS VARCHAR)
					ELSE CAST(DATEPART(mi, EstimateDT) AS VARCHAR)
				END
		END,
		CAST(0 AS FLOAT) AS DebtPrice,
    TblRequest.RequestId,
    TblRequest.IsMPRequest,
    TblRequest.MPRequestId,
    TblRequest.IsFogheToziRequest,
    TblRequest.FogheToziDisconnectId
  INTO #tmpResult
	FROM 
		TblRequest
		LEFT JOIN TblMPRequest ON TblRequest.MPRequestId = TblMPRequest.MPRequestId
	WHERE
		RequestId = @lRequestId


  /*_____Update STATUS Kambood : 7 , Arrived : 8_____*/
  UPDATE #tmpResult SET STATUS = CASE 
   	WHEN R.IsTamir = 1 AND (
          (R.IsFogheToziRequest = 1 AND FTD.FogheToziId = 1)
          OR (R.IsMPRequest = 1 AND MP.DisconnectReasonId IN (1235 , 1215))
        ) THEN 7
   	WHEN R.IsTamir = 0 AND (J.JobStatusId = 7) THEN 8
   ELSE STATUS
   END
  FROM #tmpResult R 
  LEFT JOIN Homa.TblJob J ON R.RequestId = J.RequestId
  LEFT JOIN TblFogheToziDisconnect FTD ON R.FogheToziDisconnectId = FTD.FogheToziDisconnectId
  LEFT JOIN TblMPRequest MP ON R.MPRequestId = MP.MPRequestId

  SELECT STATUS,
  	IsTamir,
		DisconnectDate,
		DisconnectTime,
    ConnectDate,
    ConnectTime,
    DebtPrice
  FROM  #tmpResult

  DROP TABLE #tmpResult
GO