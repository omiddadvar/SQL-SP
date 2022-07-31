CREATE PROCEDURE dbo.spSendPeymankarAllowSMS
AS
BEGIN
	DECLARE @lNowDT AS DATETIME = GETDATE()
	DECLARE @lReqAreaID AS BIGINT
	DECLARE @lBazdidID AS BIGINT
	DECLARE @lRequestNumber AS BIGINT
	DECLARE @lAllowNumber AS NVARCHAR(50)
	DECLARE @lSMSDesc AS NVARCHAR(50)
	DECLARE @lPeymankar AS NVARCHAR(100)
	DECLARE @lPeymankarMobile AS NVARCHAR(20)
	DECLARE @lSMS AS NVARCHAR(2000)
	DECLARE @lSMSBuff AS NVARCHAR(2000)

	SELECT @lSMS = ConfigText
	FROM Tbl_Config
	WHERE ConfigName = N'SMSPeymankarAllow'

	SELECT 
		tReq.AreaId, 
		tReq.RequestNumber, 
		tBz.BazdidId, 
		tBz.AllowNumber, 
		tTR.Peymankar, 
		tTR.PeymankarMobileNo
	INTO #tmp
	FROM 
		TblRequest tReq
		INNER JOIN TblTamirRequestConfirm TRC ON tReq.RequestId = TRC.RequestId
		INNER JOIN TblTamirRequest tTR ON TRC.TamirRequestId = tTR.TamirRequestId
		INNER JOIN TblBazdid tBz ON tReq.RequestId = tBz.RequestId
		INNER JOIN Homa.TblRequestAllow RA ON tBz.BazdidId = RA.BazdidId
	WHERE 
		RA.AllowStatusId IN (6, 7)
		AND DATEDIFF(DAY, tReq.DisconnectDT, @lNowDT) < 2
		AND tReq.EndJobStateId IN (4, 5)
		AND ISNULL(tTR.IsWarmLine, 0) = 1
		AND LEN(ISNULL(tTR.PeymankarMobileNo, '')) > 9

	DELETE
	FROM #tmp
	WHERE BazdidId IN 
		(
			SELECT BazdidId
			FROM TblBazdidInfo
		)

	IF (NOT EXISTS (SELECT 1 FROM #tmp))
	BEGIN
		INSERT INTO #tmp
		SELECT 
			tReq.AreaId, 
			tReq.RequestNumber, 
			tBz.BazdidId, 
			tBz.AllowNumber, 
			tTR.Peymankar, 
			tTR.PeymankarMobileNo
		FROM 
			TblRequest tReq
			INNER JOIN TblTamirRequestConfirm TRC ON tReq.RequestId = TRC.RequestId
			INNER JOIN TblTamirRequest tTR ON TRC.TamirRequestId = tTR.TamirRequestId
			INNER JOIN TblBazdid tBz ON tReq.RequestId = tBz.RequestId
		WHERE 
			DATEDIFF(DAY, tReq.DisconnectDT, @lNowDT) < 2
			AND tReq.EndJobStateId IN (4, 5)
			AND ISNULL(tTR.IsWarmLine, 0) = 1
			AND LEN(ISNULL(tTR.PeymankarMobileNo, '')) > 9

		DELETE
		FROM #tmp
		WHERE BazdidId IN 
		(
				SELECT BazdidId
				FROM TblBazdidInfo
		)
	END

	DECLARE db_cursor CURSOR
	FOR
	SELECT *
	FROM #tmp

	OPEN db_cursor

	FETCH NEXT
	FROM db_cursor
	INTO @lReqAreaID, @lRequestNumber, @lBazdidID, @lAllowNumber, @lPeymankar, @lPeymankarMobile

	WHILE @@FETCH_STATUS = 0
	BEGIN
		INSERT INTO TblBazdidInfo (BazdidId, IsSentAllowNumberSMS)
		VALUES (@lBazdidID, CAST(1 AS BIT));

		SET @lSMSBuff = Replace(@lSMS, 'RequestNumber', ISNULL(@lRequestNumber, '؟'))
		SET @lSMSBuff = Replace(@lSMSBuff, 'AllowNumber', ISNULL(@lAllowNumber, '؟'))
		SET @lSMSDesc = 'SMS AllowNo : ' + @lAllowNumber

		EXEC spSendSMS @lSMSBuff, @lPeymankarMobile, @lSMSDesc, '', @lReqAreaID

		FETCH NEXT
		FROM db_cursor
		INTO @lReqAreaID, @lRequestNumber, @lBazdidID, @lAllowNumber, @lPeymankar, @lPeymankarMobile
	END

	CLOSE db_cursor
	DEALLOCATE db_cursor
	DROP TABLE #tmp
END
GO