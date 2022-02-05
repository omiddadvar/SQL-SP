ALTER PROCEDURE [dbo].[spCheckNewCancelSMSEvent]
AS
DECLARE @SMS AS NVARCHAR(2000)
DECLARE @ConfigText AS NVARCHAR(2000)
DECLARE @TelMobile AS NVARCHAR(200)
DECLARE @lDate AS DATETIME = getdate()
DECLARE @RequestId AS BIGINT
DECLARE @AreaId AS INT
DECLARE @DisconnectDatePersian AS VARCHAR(10)
DECLARE @DisconnectTime AS VARCHAR(5)
DECLARE @DisconnectDT AS DATETIME

SELECT @ConfigText = ConfigText
FROM Tbl_Config
WHERE ConfigName = 'SMSCancelRequest'

SELECT RequestId
INTO #tmpRequest
FROM TblRequestCancelSMS 
WHERE SendCancelSMSStatusId = 1

SELECT *
INTO #tblRequestCancel
FROM (
	SELECT #tmpRequest.RequestId,
		TblRequest.DisconnectDatePersian,
		ISNULL(NULLIF(TblRequest.DisconnectTime,''), TblRequest.TamirDisconnectToTime) as DisconnectTime,
		TblRequest.DisconnectDT,
		TblRequest.AreaId
	FROM TblRequest
	INNER JOIN #tmpRequest ON TblRequest.RequestId = #tmpRequest.RequestId
	
	/*UNION
	
	SELECT #tmpRequest.RequestId,
		TblDeleteRequest.DisconnectDatePersian,
		TblDeleteRequest.DisconnectTime,
		cast(NULL AS DATETIME) AS DisconnectDT,
		TblDeleteRequest.AreaId
	FROM TblDeleteRequest
	INNER JOIN #tmpRequest ON TblDeleteRequest.RequestId = #tmpRequest.RequestId*/
	) AS t1

DROP TABLE #tmpRequest



UPDATE #tblRequestCancel
SET DisconnectDT = CONVERT(NVARCHAR(10), dbo.shtom(DisconnectDatePersian), 21) + ' ' + DisconnectTime
WHERE DisconnectDT IS NULL

DECLARE crCancelRequest CURSOR
FOR
SELECT *
FROM #tblRequestCancel

OPEN crCancelRequest

DECLARE @lIsLoop AS BIT

SET @lIsLoop = 1

WHILE @lIsLoop = 1
BEGIN
	FETCH NEXT
	FROM crCancelRequest
	INTO @RequestId,
		@DisconnectDatePersian,
		@DisconnectTime,
		@DisconnectDT,
		@AreaId

	IF @@FETCH_STATUS = 0
	BEGIN
		IF dateadd(MINUTE, - 30, @lDate) > @DisconnectDT
		BEGIN
			UPDATE TblRequestCancelSMS
			SET SendCancelSMSStatusId = 6
			WHERE RequestId = @RequestId

			INSERT INTO Tbl_EventLogCenter (
				TableName,
				TableNameId,
				PrimaryKeyId,
				Operation,
				AreaId,
				WorkingAreaId,
				DataEntryDT,
				SQLCommand
				)
			SELECT 'TblRequestCancelSMS' AS TableName,
				359 AS TableNameId,
				@RequestId AS PrimaryKeyId,
				2 AS Operation,
				@AreaId AS AreaId,
				99 AS WorkingAreaId,
				GETDATE() AS DataEntryDT,
				NULL AS SQLCommand
		END
		ELSE
		BEGIN
			IF NOT @ConfigText IS NULL
			BEGIN
				UPDATE TblRequestCancelSMS
				SET SendCancelSMSStatusId = 3
				WHERE RequestId = @RequestId

				DECLARE @SubscriberInformId AS BIGINT
				DECLARE @SubscriberId AS INT

				DECLARE crSubCancel CURSOR
				FOR
				SELECT TblSubscriberInfom.SubscriberInformId,
					TblSubscriberInfom.SubscriberId,
					Tbl_Subscriber.TelMobile
				FROM TblSubscriberInfom
				INNER JOIN TblRequestInform ON TblSubscriberInfom.RequestInformId = TblRequestInform.RequestInformId
				INNER JOIN Tbl_Subscriber ON TblSubscriberInfom.SubscriberId = Tbl_Subscriber.SubscriberId
				WHERE TblRequestInform.RequestId = @RequestId
					AND SendSMSStatusId = 2

				OPEN crSubCancel

				DECLARE @lIsLoop2 AS BIT

				SET @lIsLoop2 = 1

				WHILE @lIsLoop2 = 1
				BEGIN
					FETCH NEXT
					FROM crSubCancel
					INTO @SubscriberInformId,
						@SubscriberId,
						@TelMobile

					IF @@FETCH_STATUS = 0
					BEGIN
						SET @SMS = @ConfigText
						SET @SMS = Replace(@SMS, 'DisconnectDate', ISNULL(NULLIF(@DisconnectDatePersian,''), N'؟'))
						SET @SMS = Replace(@SMS, 'DisconnectTime', ISNULL(NULLIF(@DisconnectTime,''), N'؟'))

						DECLARE @Desc AS NVARCHAR(100)

						SET @Desc = 'SubInform-ID=' + cast(@SubscriberInformId AS NVARCHAR)

						EXEC spSendSMS @SMS,
							@TelMobile,
							@Desc,
							'SMSCancelRequest',
							@AreaId
					END
					ELSE
						SET @lIsLoop2 = 0
				END

				CLOSE crSubCancel

				DEALLOCATE crSubCancel
				
				UPDATE TblRequestCancelSMS
				SET SendCancelSMSStatusId = 4
				WHERE RequestId = @RequestId
				
				INSERT INTO Tbl_EventLogCenter (
					TableName,
					TableNameId,
					PrimaryKeyId,
					Operation,
					AreaId,
					WorkingAreaId,
					DataEntryDT,
					SQLCommand
					)
				SELECT 'TblRequestCancelSMS' AS TableName,
					359 AS TableNameId,
					@RequestId AS PrimaryKeyId,
					2 AS Operation,
					@AreaId AS AreaId,
					99 AS WorkingAreaId,
					GETDATE() AS DataEntryDT,
					NULL AS SQLCommand
				
			END
		END
	END
	ELSE
		SET @lIsLoop = 0
END

CLOSE crCancelRequest

DEALLOCATE crCancelRequest

DROP TABLE #tblRequestCancel

