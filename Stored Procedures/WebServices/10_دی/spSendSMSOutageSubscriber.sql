ALTER PROCEDURE spSendSMSOutageSubscriber
  @aRequestId AS BIGINT,
  @aMobile AS VARCHAR(20)
  AS
  BEGIN
    DECLARE @lBody AS NVARCHAR(2000)= N''
           ,@lProvince AS NVARCHAR(100)
           ,@lDesc AS NVARCHAR(50) = N''
    BEGIN TRY
      SELECT @lBody = ConfigText FROM Tbl_Config WHERE ConfigName = 'SMSSubscriberNewRequest'
      SELECT @lProvince = ConfigValue FROM Tbl_Config WHERE ConfigName = 'ToziName'
      
      SELECT TOP(1) CAST(RequestNumber AS VARCHAR(20)) AS RequestNumber , CAST(TrackingCode AS VARCHAR(20)) AS TrackingCode,
          Address , DataEntryDTPersian ,DataEntryTime , DisconnectDatePersian , DisconnectTime
        INTO #tmp
        FROM TblRequest
        WHERE RequestId = @aRequestId
      SET @lBody = REPLACE(@lBody , 'DataEntryDate' , ISNULL((SELECT TOP(1) DataEntryDTPersian FROM #tmp) , '____/__/__'))
      SET @lBody = REPLACE(@lBody , 'DataEntryTime' , ISNULL((SELECT TOP(1) DataEntryTime FROM #tmp) , '__:__'))
      SET @lBody = REPLACE(@lBody , 'DisconnectDate' , ISNULL((SELECT TOP(1) DisconnectDatePersian FROM #tmp) , '____/__/__'))
      SET @lBody = REPLACE(@lBody , 'DisconnectTime' , ISNULL((SELECT TOP(1) DisconnectTime FROM #tmp) , '__:__'))
      SET @lBody = REPLACE(@lBody , 'Address' , ISNULL((SELECT TOP(1) Address FROM #tmp) , '____'))
      SET @lBody = REPLACE(@lBody , 'Province' , ISNULL(@lProvince , '__'))
      SET @lBody = REPLACE(@lBody , 'RequestNumber' , ISNULL((SELECT TOP(1) RequestNumber FROM #tmp) , '__'))
      SET @lBody = REPLACE(@lBody , 'TrackingCode' , ISNULL((SELECT TOP(1) TrackingCode FROM #tmp) , '__'))
      
      SET @lDesc = 'SMSSubscriberNewRequest : ' + ISNULL((SELECT TOP(1) RequestNumber FROM #tmp) , '#RequestNumber#')
      DROP TABLE #tmp

      EXEC spCreateSMS @lBody ,@aMobile ,@lDesc , NULL
    END TRY
    BEGIN CATCH
    END CATCH
  END