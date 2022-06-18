CREATE PROCEDURE Homa.spGetOutageInfoByCallerId @CallerId AS VARCHAR(20)
AS
BEGIN
	DECLARE @lMobile AS VARCHAR(20) = NULL
	DECLARE @lTel AS VARCHAR(20) = NULL
	DECLARE @lIsTel AS BIT

	IF LEN(ISNULL(@CallerId, '')) < 8
	BEGIN
		SELECT TOP 0 NULL AS BillingID
			,NULL AS BillingIDTypeId
			,NULL AS BillingIDType

		RETURN;
	END

	IF LEN(@CallerId) < 10
		OR (
			LEN(@CallerId) >= 10
			AND LEFT(RIGHT(@CallerId, 10), 1) <> 9
			)
	BEGIN
		SET @lTel = RIGHT(@CallerId, 8)
		SET @lIsTel = 1
	END
	ELSE
	BEGIN
		IF LEFT(@CallerId, 2) = '09'
		BEGIN
			SET @CallerId = '09' + Right(@CallerId, LEN(@CallerId) - 2)
		END
		ELSE IF LEFT(@CallerId, 1) = '9'
		BEGIN
			SET @CallerId = '09' + Right(@CallerId, LEN(@CallerId) - 1)
		END
		ELSE IF LEFT(@CallerId, 4) = '0098'
		BEGIN
			SET @CallerId = '0' + Right(@CallerId, LEN(@CallerId) - 4)
		END
		ELSE IF LEFT(@CallerId, 3) = '098'
			OR LEFT(@CallerId, 3) = '+98'
		BEGIN
			SET @CallerId = '0' + Right(@CallerId, LEN(@CallerId) - 3)
		END

		SET @lMobile = @CallerId
		SET @lIsTel = 0
	END

	DECLARE @lRegisterId AS BIGINT = - 1

	IF @lIsTel = 1
		SELECT @lRegisterId = RegisterId
		FROM Homa.TblRegister
		WHERE TelNo = @lTel
	ELSE
		SELECT @lRegisterId = RegisterId
		FROM Homa.TblRegister
		WHERE MobileNo = @lMobile

	SELECT tBill.BillingID
		,tBill.BillingIDTypeId
		,tBillType.BillingIDType
		,Tbl_MPFeeder.MPPostId
		,Tbl_MPFeeder.MPFeederId
		,tBill.LPPostId
		,LPFeederId
		,Tbl_MPPost.MPPostName
		,Tbl_MPFeeder.MPFeederName
	INTO #register
	FROM Homa.TblRegister tReg
	INNER JOIN Homa.TblRegisterBillingID tBill ON tReg.RegisterId = tBill.RegisterId
	INNER JOIN Homa.Tbl_BillingIDType tBillType ON tBill.BillingIDTypeId = tBillType.BillingIDTypeId
	INNER JOIN Tbl_LPPost ON Tbl_LPPost.LPPostId = tBill.LPPostId
	INNER JOIN Tbl_MPFeeder ON Tbl_MPFeeder.MPFeederId = Tbl_LPPost.MPFeederId
	INNER JOIN Tbl_MPPost ON Tbl_MPPost.MPPostId = Tbl_MPFeeder.MPPostId
	WHERE tReg.RegisterId = @lRegisterId

	/*select * from #register		*/
	DECLARE @BillingID AS VARCHAR(50)
		,@BillingIDTypeId AS INT
		,@BillingIDType AS NVARCHAR(50)
		,@LPPostId AS INT
		,@LPFeederId AS INT
		,@MPPostId AS INT
		,@MPFeederId AS INT

	SELECT *
		,cast(0 AS INT) AS STATUS
		,0 AS MPFeederId
		,0 AS BillingIDTypeId
		,cast('' AS NVARCHAR(100)) AS BillingID
	INTO #tmpAllReq
	FROM TblRequest
	WHERE RequestId = - 1

	SELECT *
		,cast(0 AS INT) AS STATUS
	INTO #tmpReq2
	FROM TblRequest
	WHERE RequestId = - 1

	DECLARE contact_cursor CURSOR READ_ONLY
	FOR
	SELECT BillingID
		,BillingIDTypeId
		,BillingIDType
		,MPPostId
		,MPFeederId
		,LPPostId
		,LPFeederId
	FROM #register

	OPEN contact_cursor

	FETCH NEXT
	FROM contact_cursor
	INTO @BillingID
		,@BillingIDTypeId
		,@BillingIDType
		,@MPPostId
		,@MPFeederId
		,@LPPostId
		,@LPFeederId

	WHILE @@FETCH_STATUS = 0
	BEGIN
		PRINT '@LPPostId :' + cast(@LPPostId AS NVARCHAR(100))

		INSERT INTO #tmpReq2
		EXEC [dbo].[spGetOutageInfoFromNetworkById] @MPPostId
			,@MPFeederId
			,@LPPostId
			,@LPFeederId

		INSERT INTO #tmpAllReq
		SELECT *
			,@MPFeederId AS MPFeederId
			,@BillingIDTypeId AS BillingIDTypeId
			,@BillingID AS BillingID
		FROM #tmpReq2

		DELETE
		FROM #tmpReq2

		FETCH NEXT
		FROM contact_cursor
		INTO @BillingID
			,@BillingIDTypeId
			,@BillingIDType
			,@MPPostId
			,@MPFeederId
			,@LPPostId
			,@LPFeederId
	END

	CLOSE contact_cursor

	DEALLOCATE contact_cursor

  /*_____Update STATUS Kambood : 7 , Arrived : 8_____*/
    UPDATE #tmpAllReq SET STATUS = CASE 
     	WHEN R.IsTamir = 1 AND (
            (R.IsFogheToziRequest = 1 AND FTD.FogheToziId = 1)
            OR (R.IsMPRequest = 1 AND MP.DisconnectReasonId IN (1235 , 1215))
          ) THEN 7
     	WHEN R.IsTamir = 0 AND (J.JobStatusId = 7) THEN 8
      ELSE STATUS
     END
    FROM #tmpAllReq R 
    LEFT JOIN Homa.TblJob J ON R.RequestId = J.RequestId
    LEFT JOIN TblFogheToziDisconnect FTD ON R.FogheToziDisconnectId = FTD.FogheToziDisconnectId
    LEFT JOIN TblMPRequest MP ON R.MPRequestId = MP.MPRequestId


	SELECT TOP 1 #tmpAllReq.ConnectDatePersian
		,#tmpAllReq.ConnectTime
		,#tmpAllReq.Address
		,#tmpAllReq.RequestNumber
		,#tmpAllReq.DisconnectDatePersian
		,#tmpAllReq.DisconnectTime
		,#tmpAllReq.Comments
		,#tmpAllReq.IsTamir
		,#tmpAllReq.STATUS
		,Tbl_MPFeeder.VoiceCode
		,Tbl_MPFeeder.VoiceId
		,#tmpAllReq.BillingID
	FROM #tmpAllReq
	INNER JOIN Tbl_MPFeeder ON #tmpAllReq.MPFeederId = Tbl_MPFeeder.MPFeederId
	ORDER BY BillingIDTypeId

	DROP TABLE #register

	DROP TABLE #tmpAllReq

	DROP TABLE #tmpReq2
END
GO


