USE WirelessDB
GO
ALTER PROCEDURE spVoiceHeard
  @aMediaId INT
  ,@aUserId INT
  ,@aIsChannel BIT
  ,@aSourceUserId INT = -1
  AS
  BEGIN
    IF @aIsChannel = 1 BEGIN 
         UPDATE TblChannelOfflineStatus SET IsListen = 1 
            WHERE MediaId = @aMediaId AND UserId = @aUserId
    END
    ELSE BEGIN
         UPDATE TblUserOfflineStatus SET IsListen = 1
            WHERE MediaId = @aMediaId AND UserId = @aSourceUserId AND DestUserId = @aUserId
    END
  END

EXEC spVoiceHeard 1062 , 28 , 1