USE WirelessDB
GO
ALTER PROCEDURE spDebugOfflineStatus
  AS
  BEGIN
  	/*------------Channel---------------*/
    SELECT H.MediaId , UC.UserId , CAST(0 AS BIT) IsListen , H.DestChannelId INTO #tmp FROM TblMediaHistory H
      INNER JOIN TblMedia M ON H.MediaId = M.MediaId
      INNER JOIN Tbl_UserChannel UC ON (UC.ChannelId = H.DestChannelId AND UC.UserId <> H.SourceUserId)
      LEFT JOIN TblChannelOfflineStatus OfflineSt ON H.MediaId = OfflineSt.MediaId
      WHERE H.IsRecording = 0 AND M.IsOnlineVoice = 0 AND ISNULL(OfflineSt.MediaId,-1) = -1
      ORDER BY H.MediaId DESC

      INSERT INTO TblChannelOfflineStatus (MediaId, UserId, IsListen, DestChannelId)
      SELECT * FROM #tmp

      DROP TABLE #tmp
  	/*--------------User---------------*/
    SELECT H.MediaId , H.SourceUserId , CAST(0 AS BIT) IsListen , H.DestUserId INTO #tmp  FROM TblMediaHistory H
      INNER JOIN TblMedia M ON H.MediaId = M.MediaId
      LEFT JOIN TblUserOfflineStatus OfflineSt ON H.MediaId = OfflineSt.MediaId
      WHERE H.IsRecording = 0 AND M.IsOnlineVoice = 0 
        AND ISNULL(OfflineSt.MediaId,-1) = -1 AND H.DestUserId IS NOT NULL
      ORDER BY H.MediaId DESC

      INSERT INTO TblUserOfflineStatus (MediaId, UserId, IsListen, DestUserId)
      SELECT * FROM #tmp

      DROP TABLE #tmp
  END










