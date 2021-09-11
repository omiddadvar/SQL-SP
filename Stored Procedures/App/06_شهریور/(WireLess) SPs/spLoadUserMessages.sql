USE WirelessDB
GO
ALTER PROCEDURE spLoadUserMessages
  @aOffset INT
  ,@aSourceId INT
  ,@aTargetId INT
  AS
  BEGIN
    /*Loading Older Messages*/
    SELECT TOP(20) H.MediaId , H.MediaDateTime , M.MediaTime ,M.IsOnlineVoice, ISNULL(H.SourceUserId , -1) AS SourceUserId
        ,ISNULL(H.DestUserId , -1) AS DestUserId , ISNULL(H.DestChannelId , -1) AS DestChannelId
        , U.DisplayName , U.Username , dbo.mtosh(H.MediaDateTime) AS ShamsiDate
        ,CONVERT(VARCHAR(5),H.MediaDateTime,108) AS Time
      FROM TblMediaHistory H
      INNER JOIN Tbl_User U ON U.UserId = H.SourceUserId
      INNER JOIN TblMedia M ON H.MediaId = M.MediaId
      WHERE H.MediaId < @aOffset AND M.MediaTime > 200 AND H.IsRecording = 0 AND
        ((DestUserId = @aSourceId AND SourceUserId = @aTargetId) 
          OR (DestUserId = @aTargetId AND SourceUserId = @aSourceId))
      ORDER BY H.MediaId DESC
  END

EXEC spLoadUserMessages @aOffset = 800
                       ,@aSourceId = 1
                       ,@aTargetId = 5
