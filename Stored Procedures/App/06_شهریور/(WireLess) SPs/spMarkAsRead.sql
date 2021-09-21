USE WirelessDB
GO
ALTER PROCEDURE spMarkAasRead
  @aSourceUserId INT
  ,@aDestId INT 
  ,@aIsChannel BIT
  AS
  BEGIN
    IF @aIsChannel = 1 BEGIN
     	UPDATE TblChannelOfflineStatus SET IsListen = 1
        WHERE DestChannelId = @aDestId
          AND UserId = @aSourceUserId
          AND IsListen = 0
    END
    ELSE BEGIN
	    UPDATE TblUserOfflineStatus	SET IsListen = 1 
        WHERE DestUserId = @aDestId 
          AND UserId = @aSourceUserId 
          AND IsListen = 0
    END
  END
