USE WirelessDB
GO
ALTER PROCEDURE spLoadOfflineChannelMessages
  @aUserId INT
  ,@aOffset INT
  ,@aChannelId INT
  AS
  BEGIN
    /*Loading Older Messages*/
    SELECT H.MediaId , H.MediaDateTime  , M.MediaTime, M.IsOnlineVoice,ISNULL(H.SourceUserId , -1) AS SourceUserId
        ,ISNULL(H.DestUserId , -1) AS DestUserId , ISNULL(H.DestChannelId , -1) AS DestChannelId
        , U.DisplayName , U.Username , dbo.mtosh(H.MediaDateTime) AS ShamsiDate
        ,CONVERT(VARCHAR(5), H.MediaDateTime ,108) AS Time, ISNULL(St.IsListen , 0) AS IsListen
      FROM TblMediaHistory H
      INNER JOIN Tbl_User U ON U.UserId = H.SourceUserId
      INNER JOIN TblMedia M ON H.MediaId = M.MediaId
      INNER JOIN TblChannelOfflineStatus St ON (H.MediaId = St.MediaId AND st.UserId = @aUserId)
      WHERE ISNULL(St.IsListen , 0) = 0 AND M.IsOnlineVoice = 0 
        AND H.MediaId < @aOffset AND M.MediaTime > 100 
        AND H.IsRecording = 0 AND H.DestChannelId = @aChannelId
      ORDER BY H.MediaId DESC
  END


EXEC spLoadOfflineChannelMessages @aUserId = 1
                          ,@aOffset = 1354
                          ,@aChannelId = 1