USE WirelessDB
GO
ALTER PROCEDURE spGetUserMessages
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
      LEFT JOIN TblUserOfflineStatus St ON (H.MediaId = St.MediaId AND U.UserId = St.UserId)
      WHERE H.MediaId > @aOffset AND M.MediaTime > 200 AND H.IsRecording = 0 AND
        ((H.DestUserId = @aSourceId AND SourceUserId = @aTargetId) 
          OR (H.DestUserId = @aTargetId AND SourceUserId = @aSourceId))
      ORDER BY H.MediaId DESC

      SELECT * FROM #tmp ORDER BY MediaId ASC
      DROP TABLE #tmp
  END


EXEC spGetUserMessages @aOffset = 0
                      ,@aSourceId = 1
                      ,@aTargetId = 5
