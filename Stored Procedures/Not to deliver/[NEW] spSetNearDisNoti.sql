USE CCRequesterSetad
GO
ALTER PROCEDURE spSetNearDisNoti 
  AS
  BEGIN
  	BEGIN TRY
      DECLARE @lNow DATETIME = GETDATE()
        , @lMessage NVARCHAR(MAX)
        , @lTimeout INT
        , @lEdJobState INT

      SELECT @lMessage = ConfigText FROM Tbl_Config WHERE ConfigName = 'NearDisconnectNotiMsg'
      SELECT @lTimeout = CAST(ConfigValue AS INT)  FROM Tbl_Config WHERE ConfigName = 'NearDisconnectNotiTimeout'

      INSERT INTO Tbl_SendMessageSchedule (RequestId, Command, ScheduleStatusId, ScheduleDT, ExpiredDT, ScheduleTypeId, MessageTitle,  
        MessageText, SendDT, ScheduleDTFrom, ScheduleDTTo, Result)
      SELECT S.RequestId, @lMessage, 5 , S.ScheduleDT, S.ExpiredDT, S.ScheduleTypeId, S.MessageTitle,
        S.MessageText, S.SendDT, S.ScheduleDTFrom, S.ScheduleDTTo, S.Result
        FROM Tbl_SendMessageSchedule S
        INNER JOIN TblRequest R ON S.RequestId = R.RequestId
        WHERE S.ScheduleStatusId = 2 AND R.EndJobStateId = 4
          AND DATEDIFF(MINUTE , @lNow , S.ScheduleDT) > @lTimeout

      SELECT CAST(1 AS BIT) AS result
    END TRY
    BEGIN CATCH
      SELECT CAST(0 AS BIT) AS result
    END CATCH
  END


--  EXEC spSetNearDisNoti