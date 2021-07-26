
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spSendPeymankarAllowSMS]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spSendPeymankarAllowSMS]
GO
CREATE PROCEDURE [dbo].[spSendPeymankarAllowSMS]
AS
BEGIN
  DECLARE @lNowDT AS DATETIME = GETDATE()
  DECLARE @lReqID AS BIGINT, @lReqAllowID AS BIGINT, @lRequestNumber AS BIGINT
  DECLARE @lAllowNumber AS NVARCHAR(50), @lSMSDesc AS NVARCHAR(50)
  DECLARE @lPeymankar AS NVARCHAR(100)
  DECLARE @lPeymankarMobile AS NVARCHAR(20)
  DECLARE @lSMS AS NVARCHAR(2000) ,@lSMSBuff AS NVARCHAR(2000)
  SELECT @lSMS = ConfigText FROM Tbl_Config WHERE ConfigName = N'SMSPeymankarAllow'
  SELECT R.RequestId ,R.RequestNumber, RA.RequestAllowId ,B.AllowNumber , TR.Peymankar , TR.PeymankarMobileNo
    INTO #tmp FROM TblRequest R
    INNER JOIN TblTamirRequestConfirm TRC ON R.RequestId = TRC.RequestId
    INNER JOIN TblTamirRequest TR ON TRC.TamirRequestId = TR.TamirRequestId
    INNER JOIN TblBazdid B ON R.RequestId = B.RequestId
    INNER JOIN Homa.TblRequestAllow RA ON B.BazdidId = RA.BazdidId
    WHERE RA.AllowStatusId IN (6,7) AND DATEDIFF(DAY , @lNowDT , R.DisconnectDT) < 2
      AND R.EndJobStateId IN (4,5)
    DELETE FROM #tmp WHERE RequestAllowId IN (SELECT RequestAllowId FROM Homa.TblRequestAllowInfo)
    
    DECLARE db_cursor CURSOR FOR SELECT * FROM #tmp  
    OPEN db_cursor  
    FETCH NEXT FROM db_cursor INTO @lReqID, @lRequestNumber, @lReqAllowID, @lAllowNumber, @lPeymankar, @lPeymankarMobile
    WHILE @@FETCH_STATUS = 0  
    BEGIN
        INSERT INTO Homa.TblRequestAllowInfo (RequestAllowId, IsSentSMS) VALUES (@lReqAllowID , CAST(1 AS BIT));
        SET @lSMSBuff = Replace( @lSMS,'RequestNumber', ISNULL(@lRequestNumber,'؟'))
        SET @lSMSBuff = Replace( @lSMS,'AllowNumber', ISNULL(@lAllowNumber,'؟'))
        SET @lSMSDesc = 'SMS AllowNo : ' + @lAllowNumber + ', ReqID : ' + CAST(@lReqID AS VARCHAR(20))
        EXEC spSendSMS @lSMSBuff , @lPeymankarMobile, @lSMSDesc , '' , @lReqID
    FETCH NEXT FROM db_cursor INTO @lReqID, @lRequestNumber, @lReqAllowID, @lAllowNumber, @lPeymankar, @lPeymankarMobile
    END 

    CLOSE db_cursor  
    DEALLOCATE db_cursor 
    DROP TABLE #tmp
END

  EXEC [spSendPeymankarAllowSMS]
--SELECT * FROM Homa.TblRequestAllow 
--SELECT * FROM TblBazdid WHERE LEN(AllowNumber) > 0
--SELECT * FROM Homa.Tbl_AllowStatus 
--SELECT * FROM TblTamirRequest 
--SELECT * FROM Homa.TblRequestAllowInfo