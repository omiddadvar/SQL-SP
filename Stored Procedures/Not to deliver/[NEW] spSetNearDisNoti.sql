USE CCRequesterSetad
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.spSetNearDisNoti') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE dbo.spSetNearDisNoti
GO
CREATE PROCEDURE dbo.spSetNearDisNoti 
  AS
  BEGIN
  	BEGIN TRY
      DECLARE @lNow DATETIME = GETDATE()
        , @lMessage NVARCHAR(MAX)
        , @lTimeout INT

      SELECT @lMessage = ConfigText FROM Tbl_Config WHERE ConfigName = 'NearDisconnectNotiMsg'
      SELECT @lTimeout = CAST(ConfigValue AS INT)  FROM Tbl_Config WHERE ConfigName = 'NearDisconnectNotiTimeout'
      
      INSERT INTO Tbl_SendMessageSchedule (RequestId, Command, ScheduleStatusId, ScheduleDT, ExpiredDT, ScheduleTypeId, MessageTitle,  
        MessageText, SendDT, ScheduleDTFrom, ScheduleDTTo, Result)
      SELECT S.RequestId, @lMessage, 5 , S.ScheduleDT, S.ExpiredDT, S.ScheduleTypeId, S.MessageTitle,
        S.MessageText, S.SendDT, S.ScheduleDTFrom, S.ScheduleDTTo, S.Result
        FROM Tbl_SendMessageSchedule S
        INNER JOIN TblRequest R ON S.RequestId = R.RequestId
        WHERE S.ScheduleStatusId = 2 AND R.EndJobStateId = 4
          AND DATEDIFF(MINUTE , @lNow , S.ExpiredDT) BETWEEN @lTimeout AND (@lTimeout + 5)
          AND NOT EXISTS(SELECT C.RequestId FROM TblRequestCancelSMS C WHERE RequestId = R.RequestId)
          AND NOT EXISTS(SELECT SC.SendMessageScheduleId FROM Tbl_SendMessageSchedule SC 
              WHERE SC.RequestId = R.RequestId AND  SC.ScheduleStatusId BETWEEN 5 AND 7)

      SELECT CAST(1 AS BIT) AS result
    END TRY
    BEGIN CATCH
      SELECT CAST(0 AS BIT) AS result
    END CATCH
  END

--  EXEC spSetNearDisNoti
--
--DECLARE @lNow DATETIME = GETDATE()
--SELECT S.RequestId, S.Command, s.ScheduleStatusId , DATEDIFF(MINUTE , @lNow , S.ExpiredDT) AS now , S.ScheduleDT, S.ExpiredDT, S.ScheduleTypeId, S.MessageTitle,
--  S.MessageText, S.SendDT, S.ScheduleDTFrom, S.ScheduleDTTo, S.Result
--  FROM Tbl_SendMessageSchedule S
--  INNER JOIN TblRequest R ON S.RequestId = R.RequestId
--  WHERE S.ScheduleStatusId = 2 AND R.EndJobStateId = 4
--    AND DATEDIFF(MINUTE , @lNow , S.ExpiredDT) BETWEEN 60 AND 120
--   -- AND NOT EXISTS(SELECT C.RequestId FROM TblRequestCancelSMS C WHERE RequestId = R.RequestId)
--
--  SELECT C.RequestId FROM TblRequestCancelSMS C WHERE RequestId = 9900000000001142