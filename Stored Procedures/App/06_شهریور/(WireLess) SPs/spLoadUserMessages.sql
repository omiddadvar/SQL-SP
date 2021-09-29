USE WirelessDB
GO
ALTER PROCEDURE spLoadUserMessages
  @aOffset INT
  ,@aSourceId INT
  ,@aTargetId INT
  ,@ExtraSearch AS VARCHAR(1000) = ''
  ,@ExtraJoin AS VARCHAR(1000) = ''
  AS
  BEGIN
    /*Loading Older Messages*/
    DECLARE @SQL AS VARCHAR(MAX) = '
    SELECT TOP(20) H.MediaId , H.MediaDateTime , M.MediaTime ,M.IsOnlineVoice, ISNULL(H.SourceUserId , -1) AS SourceUserId
        ,ISNULL(H.DestUserId , -1) AS DestUserId , ISNULL(H.DestChannelId , -1) AS DestChannelId
        , U.DisplayName , U.Username , dbo.mtosh(H.MediaDateTime) AS ShamsiDate
        ,CONVERT(VARCHAR(5),H.MediaDateTime,108) AS Time, ISNULL(St.IsListen , 0) AS IsListen
      FROM TblMediaHistory H
      INNER JOIN Tbl_User U ON U.UserId = H.SourceUserId
      INNER JOIN TblMedia M ON H.MediaId = M.MediaId
      ' + @ExtraJoin + '
      LEFT JOIN TblUserOfflineStatus St ON (H.MediaId = St.MediaId AND H.SourceUserId = St.UserId AND H.DestUserId = St.DestUserId)
      WHERE H.MediaId < ' + CAST(@aOffset AS VARCHAR(20)) + ' AND M.MediaTime > 100 AND H.IsRecording = 0 '+ @ExtraSearch +' 
        AND ((H.DestUserId = ' + CAST(@aSourceId AS VARCHAR(20)) + ' AND H.SourceUserId = ' + CAST(@aTargetId AS VARCHAR(20)) + ') 
          OR (H.DestUserId = ' + CAST(@aTargetId AS VARCHAR(20)) + ' AND H.SourceUserId = ' + CAST(@aSourceId AS VARCHAR(20)) + '))
      ORDER BY H.MediaId DESC
      '
      EXEC(@SQL)
  END

EXEC spLoadUserMessages @aOffset = 800
                       ,@aSourceId = 1
                       ,@aTargetId = 5
