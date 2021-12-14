USE CCRequesterSetad
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].spPrepareNotiMessage') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].spPrepareNotiMessage
GO
CREATE PROCEDURE [dbo].spPrepareNotiMessage @aReqId BIGINT
AS
  BEGIN
    DECLARE @lText AS NVARCHAR(MAX) = ''
      ,@lSubject AS NVARCHAR(1000) = 'DEFAULT VALUE'
      ,@lDuration AS INT
      ,@lDisconnectDatePersian AS VARCHAR(10)
      ,@lDisconnectTime AS VARCHAR(5)
      ,@lIsOk AS BIT = CAST(0 AS BIT)
  	
    SELECT @lText = ConfigText FROM Tbl_Config WHERE ConfigName = 'NotifBaBarname'
    IF @lText IS NULL OR @lText = '' 
      BEGIN
        RAISERROR('NotifBaBarname Is Empty - Tbl_Config' , 16 , 1);                      	
      END
    SELECT * INTO #tmp FROM TblRequest WHERE RequestId = @aReqId
    
    SET @lIsOk = CAST(CASE WHEN (SELECT COUNT(*) FROM #tmp) > 0 THEN 1 ELSE 0 END AS BIT)

    SELECT @lDuration =  DATEDIFF(MINUTE, TamirDisconnectFromDT , TamirDisconnectToDT)
      , @lDisconnectDatePersian = DisconnectDatePersian 
      , @lDisconnectTime = DisconnectTime
       FROM #tmp

    SELECT @lSubject = Sub.TamirRequestSubject
      FROM #tmp R
      INNER JOIN TblTamirRequestConfirm C ON R.RequestId = C.RequestId
      INNER JOIN TblTamirRequest TR ON C.TamirRequestId = TR.TamirRequestId 
      INNER JOIN Tbl_TamirRequestSubject Sub ON TR.TamirRequestSubjectId = Sub.TamirRequestSubjectId
      WHERE R.RequestId = @aReqId
    
    SET @lText = REPLACE(@lText , 'DisconnectDatePersian' , @lDisconnectDatePersian)
    SET @lText = REPLACE(@lText , 'DisconnectTime' , @lDisconnectTime)
    SET @lText = REPLACE(@lText , 'ss' , CAST(@lDuration AS VARCHAR(4)))
    SET @lText = REPLACE(@lText , 'Subject' , @lSubject)
    
    SELECT @lIsOk AS IsOk, @lSubject AS Subject, @lText AS Text
    DROP TABLE #tmp
  END

--IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 84)
--INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (84,N'NotifBaBarname',NULL,N'„‘ —ò ê—«„Ì »—ﬁ ‘„« »Â ⁄·  Subject° «Õ „«·« œ—  «—ÌŒ DisconnectDatePersian «“ ”«⁄  DisconnectTime »„œ   ﬁ—Ì»Ì ss œﬁÌﬁÂ ﬁÿ⁄ ŒÊ«Âœ ‘œ. 
-- Ê“Ì⁄ »—ﬁ ')
--GO
