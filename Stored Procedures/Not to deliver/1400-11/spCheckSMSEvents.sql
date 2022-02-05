USE [CCRequesterSetad]
GO
/****** Object:  StoredProcedure [dbo].[spCheckSMSEvents]    Script Date: 02/05/2022 12:04:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[spCheckSMSEvents] 
AS
	DECLARE @lOperation AS nvarchar(1000) = ''
	DECLARE @lErrMsg AS nvarchar(500) = ''
	DECLARE @lErrCode as int = 0

	DECLARE @lCounter AS bigint
	DECLARE @lCnt AS bigint
	DECLARE @lConfigId AS int

	SET @lConfigId = NULL
	SET @lCounter = 0

	BEGIN TRY
		--- Read Cofing Counter
		SELECT @lConfigId = ConfigId FROM Tbl_Config WHERE ConfigName = 'smsCounter'
		IF @lConfigId IS NULL
			SET @lCounter = 0
		ELSE
			SELECT @lCounter = Cast(ConfigValue AS bigint) FROM Tbl_Config WHERE ConfigId = @lConfigId

		SET @lCounter = @lCounter + 1
		DECLARE	@return_smsCount int = 1
		IF (@lCounter % 7) = 0 and @return_smsCount > 0
			SET @lOperation = N'(توليد پيام براي اطلاعات خاموشي‌ها (انرژي و زمان) و پيام تأييد درخواست خاموشي)'
		while (@lCounter % 7) = 0 and @return_smsCount > 0
		begin
			EXEC @return_smsCount = spCheckNewDCManager	-- توليد پيام براي اطلاعات خاموشي‌ها (انرژي و زمان) و پيام تأييد درخواست خاموشي
		end

		IF (@lCounter % 5) = 0 
		BEGIN
			SET @lOperation = @lOperation + N'(توليد پيام آماري)'
			EXEC spCheckNewSTManager	-- توليد پيام آماري
		END
		
		IF (@lCounter % 8) = 0 
		BEGIN
			SET @lOperation = @lOperation + N'(توليد پيام پيک بار مرکز)(توليد پيام پيک بار نواحي)'
			EXEC spCheckNewSTManagerPeak	-- توليد پيام پيک بار مرکز
			EXEC spCheckNewSTManagerPeakArea	-- توليد پيام پيک بار نواحي
		END
		
		IF (@lCounter % 23) = 0 
		BEGIN
			SET @lOperation = @lOperation + N'(توليد پيام براي قطع بيش از چند بار در ماه فيدر)'
			EXEC spCheckMPFeederDCCountSMS	-- توليد پيام براي قطع بيش از چند بار در ماه فيدر
		END

		IF (@lCounter % 61) = 0 
		BEGIN
			SET @lOperation = @lOperation + N'(توليد پيام براي فهرست تجهيزات سرقت شده)'
			EXEC spCheckSMSSerghat	-- توليد پيام براي فهرست تجهيزات سرقت شده
		END
			
		IF (@lCounter % 3) = 0
		BEGIN
			SET @lOperation = @lOperation + N'(توليد پيامهاي ثبت شده در پنل پيامک)(توليد پيام هشدار به پيمانکاران در خصوص خاموشيهاي بابرنامه)'
			EXEC spCheckSMSPanel -- توليد پيامهاي ثبت شده در پنل پيامک
			EXEC spSendSMSAlarmForPeymankar  -- توليد پيام هشدار به پيمانکاران در خصوص خاموشيهاي بابرنامه
		END
			
		IF (@lCounter % 113) = 0
		BEGIN
			SET @lOperation = @lOperation + N'(توليد پيام مدت زمان خاموشي مشترکين در روز به دقيقه)'
			EXEC spCheckNewSTManagerSubscriber -- توليد پيام مدت زمان خاموشي مشترکين در روز به دقيقه
		END
			
		IF (@lCounter % 157) = 0
		BEGIN
			SET @lOperation = @lOperation + N'(توليد پيام فيدرهاي بحراني)'
			EXEC spCheckCriticalFeederSMS -- توليد پيام فيدرهاي بحراني
		END
		
		IF (@lCounter % 97) = 0
		BEGIN
			SET @lOperation = @lOperation + N'(توليد پيام n فيدر اول داراي بيشترين انرژي توزيع نشده در ماه)'
			EXEC spCheckSMSMPFeederDCPower -- توليد پيام n فيدر اول داراي بيشترين انرژي توزيع نشده در ماه
		END
		IF (@lCounter % 2) = 0 
		BEGIN
			SET @lOperation = @lOperation + N'(توليد پيام خاموشي‌ها براي مشترکين)(توليد فکس خاموشي ها براي مشترکين)(توليد ايميل خاموشي ها براي مشترکين)(توليد تماس تلفني خاموشي ها براي مشترکين)(توليد پيام بعد از وصل خاموشي براي مشترکين)(توليد پيام در صورت عدم اعزام اکيپ براي خاموشي اعلام شده توسط مشترک)(توليد پيام لغو خاموشي بابرنامه به مشترکين)'
			EXEC spCheckNewSMSEvent	-- توليد پيام خاموشي‌ها براي مشترکين
			EXEC spCheckNewFaxEvent	-- توليد فکس خاموشي ها براي مشترکين
			EXEC spCheckNewEmailEvent	-- توليد ايميل خاموشي ها براي مشترکين
			EXEC spCheckNewCallEvent	-- توليد تماس تلفني خاموشي ها براي مشترکين
			EXEC spCheckNewSMSEventAfterConnect	-- توليد پيام بعد از وصل خاموشي براي مشترکين
			EXEC spCheckSubscriberNotEkip -- توليد پيام در صورت عدم اعزام اکيپ براي خاموشي اعلام شده توسط مشترک
			EXEC spCheckNewCancelSMSEvent -- توليد پيام لغو خاموشي هاي بابرنامه به مشترکين
      EXEC spSendPeymankarAllowSMS -- پيمانکار کار اجازه صدور تاييد ارسال
		END
		IF (@lCounter % 119) = 0 
		BEGIN
			SET @lOperation = @lOperation + N'(توليد پيام ترانس سوزي‌ها)'
			SET @lCnt = 1
			WHILE @lCnt > 0
			BEGIN
				EXEC @lCnt = spCheckNewTransFault	-- توليد پيام ترانس سوزي‌ها
			END
		END

		IF (@lCounter % 31) = 0 
		BEGIN
			SET @lOperation = @lOperation + N'(توليد پيام هنگام ويرايش خاموشي برقدار شده)'
			EXEC spCheckSMSAfterEdit	-- توليد پيام هنگام ويرايش خاموشي برقدار شده
		END

		IF (@lCounter % 11) = 0 
		BEGIN
			DECLARE @lCnt2 as int = 1 
			SET @lCnt = 1
			DECLARE @IsSendSeparateMPFeederConnect AS bit = 0
			SELECT 
				@IsSendSeparateMPFeederConnect = CAST (ConfigValue AS bit) 
			FROM 
				Tbl_Config 
			WHERE 
				ConfigName = 'IsSendSeparateMPFeederConnect'

			WHILE @lCnt > 0 OR @lCnt2 > 0
			BEGIN
				IF @IsSendSeparateMPFeederConnect = 1 AND @lCnt2 = 1
				BEGIN
					SET @lOperation = @lOperation + N'(توليد پيام بعد از وصل هر فيدر در خاموشي هاي فوق توزيع به تفکيک فيدر)'
					EXEC @lCnt2 = spSendSMSFTMPFeederAfterConnect	-- توليد پيام بعد از وصل هر فيدر در خاموشي هاي فوق توزيع به تفکيک فيدر
				END
				ELSE
					SET @lCnt2 = 0
				
				SET @lOperation = @lOperation + N'(توليد پيام بعد از وصل خاموشي)'
				EXEC @lCnt = spSendSMSDCManagerAfterConnect	-- توليد پيام بعد از وصل خاموشي
			END
		END
		IF (@lCounter % 13) = 0 
		BEGIN
			SET @lOperation = @lOperation + N'(توليد پيام به ازاي هر وصل از وصلهاي چند مرحله اي)'
			EXEC spSendSMSDCManagerAfterMultistep	-- توليد پيام به ازاي هر وصل از وصلهاي چند مرحله اي
		END

		--- Write Cofing Counter
		IF @lConfigId IS NULL
			INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (107,N'smsCounter',Cast(@lCounter AS nvarchar),'Last Run: '+Cast(GETDATE() as nvarchar))
		ELSE
			UPDATE Tbl_Config 
			SET 
				ConfigValue = Cast(@lCounter AS nvarchar), 
				ConfigText = 'Last Run: ' + Cast(GETDATE() as nvarchar)
		WHERE ConfigName = 'smsCounter'
			
	END TRY
	BEGIN CATCH
		SET @lErrCode = ERROR_STATE()
		SET @lErrMsg = ERROR_MESSAGE()
		BEGIN TRY
			ROLLBACK TRANSACTION
		END TRY
		BEGIN CATCH
		END CATCH
		
	END CATCH
	
	SELECT 
		@lOperation AS Opration,
		@lErrCode AS ResultCode, 
		@lErrMsg AS ResultMessage,
		@lCounter AS SMSCounter
