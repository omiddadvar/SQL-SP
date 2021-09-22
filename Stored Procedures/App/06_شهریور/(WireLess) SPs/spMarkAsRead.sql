USE WirelessDB
GO
ALTER PROCEDURE spMarkAasRead
  @aSourceUserId INT
  ,@aDestId INT 
  ,@aIsChannel BIT
  AS
  BEGIN
    IF @aIsChannel = 1 BEGIN
      SELECT ChannelOfflineStatusId AS AutoIncId ,MediaId,UserId ,IsListen ,DestChannelId AS DestId
        INTO #tmp1 FROM TblChannelOfflineStatus 
        WHERE DestChannelId = @aDestId
          AND UserId = @aSourceUserId
          AND IsListen = 0
     	UPDATE TblChannelOfflineStatus SET IsListen = 1
        WHERE DestChannelId = @aDestId
          AND UserId = @aSourceUserId
          AND IsListen = 0
      SELECT * FROM #tmp1
      DROP TABLE #tmp1
    END
    ELSE BEGIN
      SELECT UserOfflineStatusId AS AutoIncId ,MediaId,UserId ,IsListen ,DestUserId AS DestId
        INTO #tmp2 FROM TblUserOfflineStatus
        WHERE DestUserId = @aDestId 
          AND UserId = @aSourceUserId 
          AND IsListen = 0
	    UPDATE TblUserOfflineStatus	SET IsListen = 1 
        WHERE DestUserId = @aDestId 
          AND UserId = @aSourceUserId 
          AND IsListen = 0
        SELECT * FROM #tmp2
        DROP TABLE #tmp2
    END
  END
