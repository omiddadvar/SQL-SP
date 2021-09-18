USE WirelessDB
GO
ALTER PROCEDURE spGetOfflineUserMessages
  @aOffset INT
  ,@aSourceId INT
  ,@aTargetId INT
  AS
  BEGIN
    /*Getting New Messages*/
    SELECT TOP(20) H.MediaId , H.MediaDateTime  , M.MediaTime,M.IsOnlineVoice, ISNULL(H.SourceUserId , -1) AS SourceUserId
        ,ISNULL(H.DestUserId , -1) AS DestUserId , ISNULL(H.DestChannelId , -1) AS DestChannelId
        , U.DisplayName , U.Username, dbo.mtosh(H.MediaDateTime) AS ShamsiDate
        ,CONVERT(VARCHAR(5), H.MediaDateTime ,108) AS Time, ISNULL(St.IsListen , 0) AS IsListen
      INTO #tmp
      FROM TblMediaHistory H
      INNER JOIN Tbl_User U ON U.UserId = H.SourceUserId
      INNER JOIN TblMedia M ON H.MediaId = M.MediaId
      LEFT JOIN TblUserOfflineStatus St ON H.MediaId = St.MediaId
      WHERE H.MediaId > @aOffset AND M.MediaTime > 100 AND H.IsRecording = 0 
        AND M.IsOnlineVoice = 0 AND ISNULL(St.IsListen , 0) = 0
        AND H.DestUserId = St.DestUserId AND H.SourceUserId = St.UserId
        AND (H.DestUserId = @aTargetId AND H.SourceUserId = @aSourceId)
      ORDER BY H.MediaId DESC

      SELECT * FROM #tmp ORDER BY MediaId ASC
      DROP TABLE #tmp
  END


EXEC spGetOfflineUserMessages @aOffset = 0
                      ,@aSourceId = 5
                      ,@aTargetId = 1

EXEC spGetOfflineUserMessages 0,1,5

SELECT * FROM TblUserOfflineStatus WHERE MediaId = 1099