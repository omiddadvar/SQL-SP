USE WirelessDB
GO
ALTER PROCEDURE spGetUserChannel
  @aUserId INT
  AS
  BEGIN 
    SELECT Ch.ChannelId , ch.ChannelName , Ch.ChannelNumber , ISNULL(ch.IsActive , 0) AS IsActive
      , ISNULL(Uc.IsMute , 0) AS IsMute, ISNULL(Uc.IsBiDirectional, 0) AS IsBiDirectional , uc.UserId
      FROM Tbl_Channel Ch
      INNER JOIN Tbl_UserChannel Uc ON Ch.ChannelId = Uc.ChannelId
      WHERE Ch.IsActive = 1 AND Uc.UserId = @aUserId
  END


EXEC spGetUserChannel 6

