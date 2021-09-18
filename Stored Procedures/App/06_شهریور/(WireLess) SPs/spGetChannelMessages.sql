USE WirelessDB
GO
ALTER PROCEDURE spGetChannelMessages
  @aUserId INT
  ,@aOffset INT
  ,@aChannelId INT
  AS
  BEGIN
    SELECT TOP(20) H.MediaId , H.SourceUserId INTO #tmpMedia FROM TblMediaHistory H
      INNER JOIN TblMedia M ON H.MediaId = M.MediaId
      WHERE  H.MediaId > @aOffset AND M.MediaTime > 100 
        AND H.IsRecording = 0 AND H.DestChannelId = @aChannelId
      ORDER BY H.MediaId DESC

    SELECT OffState.MediaId , (CASE WHEN COUNT(*) > 0 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END) AS IsListen
      INTO #tmpListen FROM TblChannelOfflineStatus OffState
        INNER JOIN #tmpMedia ON OffState.MediaId = #tmpMedia.MediaId
        WHERE OffState.IsListen = 1 AND #tmpMedia.SourceUserId = @aUserId
        GROUP BY OffState.MediaId

    /*Getting New Messages*/
    SELECT H.MediaId, H.MediaDateTime, M.MediaTime, M.IsOnlineVoice, ISNULL(H.SourceUserId , -1) AS SourceUserId
        , ISNULL(H.DestUserId , -1) AS DestUserId, ISNULL(H.DestChannelId , -1) AS DestChannelId
        , U.DisplayName, U.Username, dbo.mtosh(H.MediaDateTime) AS ShamsiDate
        , CONVERT(VARCHAR(5), H.MediaDateTime ,108) AS Time
        , (CASE WHEN H.SourceUserId = @aUserId THEN ISNULL(L.IsListen, 0) ELSE  ISNULL(St.IsListen , 0) END) AS IsListen
      INTO #tmp
      FROM TblMediaHistory H
      INNER JOIN Tbl_User U ON U.UserId = H.SourceUserId
      INNER JOIN TblMedia M ON H.MediaId = M.MediaId
      INNER JOIN #tmpMedia ON H.MediaId = #tmpMedia.MediaId
      LEFT JOIN TblChannelOfflineStatus St ON (H.MediaId = St.MediaId AND St.UserId = @aUserId)
      LEFT JOIN #tmpListen L ON H.MediaId = L.MediaId
      ORDER BY H.MediaId DESC

      SELECT * FROM #tmp ORDER BY MediaId ASC
      DROP TABLE #tmp
      DROP TABLE #tmpMedia
      DROP TABLE #tmpListen
  END

EXEC spGetChannelMessages @aUserId = 1
                         ,@aOffset = 0
                         ,@aChannelId = 1