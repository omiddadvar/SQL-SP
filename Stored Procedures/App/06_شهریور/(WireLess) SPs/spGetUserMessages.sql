USE WirelessDB
GO
ALTER PROCEDURE spGetUserMessages
  @aOffset INT
  ,@aSourceId INT
  ,@aTargetId INT
  ,@ExtraSearch AS VARCHAR(1000) = ''
  ,@ExtraJoin AS VARCHAR(1000) = 'LEFT'
  AS
  BEGIN
    /*Getting New Messages*/
    DECLARE @SQL AS VARCHAR(MAX) = '
    SELECT TOP(20) H.MediaId , H.MediaDateTime  , M.MediaTime,M.IsOnlineVoice, ISNULL(H.SourceUserId , -1) AS SourceUserId
        ,ISNULL(H.DestUserId , -1) AS DestUserId , ISNULL(H.DestChannelId , -1) AS DestChannelId
        , U.DisplayName , U.Username, dbo.mtosh(H.MediaDateTime) AS ShamsiDate
        ,CONVERT(VARCHAR(5), H.MediaDateTime ,108) AS Time, ISNULL(St.IsListen , 0) AS IsListen
      INTO #tmp
      FROM TblMediaHistory H
      INNER JOIN Tbl_User U ON U.UserId = H.SourceUserId
      INNER JOIN TblMedia M ON H.MediaId = M.MediaId
      ' + @ExtraJoin + ' JOIN TblUserOfflineStatus St ON 
          (H.MediaId = St.MediaId AND H.SourceUserId = St.UserId AND H.DestUserId = St.DestUserId)
      WHERE H.MediaId > ' + CAST(@aOffset AS VARCHAR(20)) + ' AND M.MediaTime > 200 AND H.IsRecording = 0 '+ @ExtraSearch +' 
        AND ((H.DestUserId = ' + CAST(@aSourceId AS VARCHAR(20)) + ' AND H.SourceUserId = ' + CAST(@aTargetId AS VARCHAR(20)) + ') 
          OR (H.DestUserId = ' + CAST(@aTargetId AS VARCHAR(20)) + ' AND H.SourceUserId = ' + CAST(@aSourceId AS VARCHAR(20)) + '))
      ORDER BY H.MediaId DESC

      SELECT * FROM #tmp ORDER BY MediaId ASC
      DROP TABLE #tmp
      '
      EXEC(@SQL)
  END

EXEC spGetUserMessages @aOffset = 0
                      ,@aSourceId = 1
                      ,@aTargetId = 5
                      ,@ExtraSearch = ''
                      ,@ExtraJoin = 'INNER'

EXEC spGetUserMessages 0 ,5, 1