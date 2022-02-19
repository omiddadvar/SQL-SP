CREATE PROCEDURE dbo.spAddTestServiceSMSLog (
	@TestServiceLogId AS BIGINT
	,@TestServiceStatus AS INT
	)
AS
BEGIN
	DECLARE @TestServiceId AS BIGINT
	DECLARE @Desc AS NVARCHAR(1000)
	DECLARE @LogDT AS DATETIME

	SELECT @TestServiceId = TestServiceId
		,@Desc = isnull([Description], '')
		,@LogDT = LogDT
	FROM dbo.TblTestServiceLog
	WHERE TestServiceLogId = @TestServiceLogId

	SELECT TestServiceSMSId
		,MobileNo
		,CAST(CASE WHEN @TestServiceStatus = 1
					THEN SMSOk
				  ELSE SMSError
				END AS NVARCHAR(2000)) AS SMSBody
		,CAST(NULL AS BIGINT) AS SMSId
	INTO #tmp
	FROM dbo.Tbl_TestServiceSMS
	CROSS JOIN dbo.Tbl_TestService
	WHERE Tbl_TestServiceSMS.IsActive = 1
		AND Tbl_TestService.TestServiceId = @TestServiceId

	UPDATE #tmp
	SET SMSBody = replace(SMSBody, '@Desc', @Desc)

	UPDATE #tmp
	SET SMSBody = replace(SMSBody, '@Date', dbo.mtosh(@LogDT))

	UPDATE #tmp
	SET SMSBody = replace(SMSBody, '@Time', dbo.GetTime(@LogDT))

	SELECT *
	FROM #tmp

	DECLARE @fetch_status INT

	DECLARE Cur_Cursor CURSOR LOCAL
	FOR
	SELECT TestServiceSMSId
		,MobileNo
		,SMSBody
	FROM #tmp

	OPEN Cur_Cursor

	DECLARE @TestServiceSMSId AS BIGINT
	DECLARE @MobileNo AS VARCHAR(50)
	DECLARE @SMSBody AS NVARCHAR(2000)
	DECLARE @SCHEMA_ID INT

	SELECT @fetch_status = 0

	WHILE @fetch_status = 0
	BEGIN
		FETCH NEXT
		FROM Cur_Cursor
		INTO @TestServiceSMSId
			,@MobileNo
			,@SMSBody

		SELECT @fetch_status = @@fetch_status

		IF @fetch_status <> 0
		BEGIN
			CONTINUE
		END

		CREATE TABLE #tmpSMSId (SMSId BIGINT)

		INSERT INTO #tmpSMSId
		EXEC dbo.spCreateSMS @SMSBody
			,@MobileNo
			,'TestServiceLog'
			,NULL

		DECLARE @SMSId AS BIGINT

		SELECT @SMSId = SMSId
		FROM #tmpSMSId

		DROP TABLE #tmpSMSId

		UPDATE #tmp
		SET SMSId = @SMSId
		WHERE TestServiceSMSId = @TestServiceSMSId
	END --While end

	CLOSE Cur_Cursor

	DEALLOCATE Cur_Cursor

	INSERT INTO [TblTestServiceSMSLog] (
		 TestServiceSMSId
		,TestServiceLogId
		,TestServiceStatus
		,SMSId
		,SMSDT
		)
	SELECT TestServiceSMSId
		,@TestServiceLogId
		,@TestServiceStatus
		,SMSId
		,GETDATE()
	FROM #tmp

	DROP TABLE #tmp
END
GO