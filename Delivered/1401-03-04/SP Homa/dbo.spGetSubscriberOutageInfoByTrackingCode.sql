ALTER PROCEDURE dbo.spGetSubscriberOutageInfoByTrackingCode
	@aTrackingCode as bigint
AS
	DECLARE @lRequestId as BIGINT
	DECLARE @lDuplicatedRequestId as BIGINT
	DECLARE @lEndJobStateId as INT
	
	SET @lRequestId = NULL
	
	SELECT TOP 1 
		@lRequestId = RequestId,
		@lDuplicatedRequestId = DuplicatedRequestId,
		@lEndJobStateId = EndJobStateId
	FROM 
		TblRequest 
	WHERE
		TblRequest.TrackingCode = @aTrackingCode

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