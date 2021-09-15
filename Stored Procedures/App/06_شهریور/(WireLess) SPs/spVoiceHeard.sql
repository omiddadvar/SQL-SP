USE WirelessDB
GO
ALTER PROCEDURE spVoiceHeard
  @aMediaId INT
  ,@aUserId INT
  ,@aIsChannel BIT
  AS
  BEGIN
    IF @aIsChannel = 1 BEGIN 
         UPDATE TblChannelOfflineStatus SET IsListen = 1 
            WHERE MediaId = @aMediaId AND UserId = @aUserId
    END
    ELSE BEGIN
         UPDATE TblUserOfflineStatus SET IsListen = 1 
            WHERE MediaId = @aMediaId AND UserId = @aUserId
    END
  END


EXEC spVoiceHeard 1062 , 28 , 1