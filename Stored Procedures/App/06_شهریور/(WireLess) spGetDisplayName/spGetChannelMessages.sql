USE WirelessDB
GO
ALTER PROCEDURE spGetChannelMessages
  @aOffset INT
  ,@aChannelId INT
  AS
  BEGIN
    /*Getting New Messages*/
    SELECT TOP(20) H.MediaId , H.MediaDateTime , ISNULL(H.SourceUserId , -1) AS SourceUserId
        ,ISNULL(H.DestUserId , -1) AS DestUserId , ISNULL(H.DestChannelId , -1) AS DestChannelId
        , U.DisplayName , U.Username 
      FROM TblMediaHistory H
      INNER JOIN Tbl_User U ON U.UserId = H.SourceUserId
      WHERE H.MediaId > @aOffset AND H.IsRecording = 0 
        AND H.DestChannelId = @aChannelId
      ORDER BY H.MediaId DESC
  END


--EXEC spGetChannelMessages @aOffset = 0
--                         ,@aChannelId = 1