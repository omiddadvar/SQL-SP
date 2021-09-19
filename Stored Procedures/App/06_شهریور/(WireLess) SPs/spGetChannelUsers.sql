USE WirelessDB
GO
ALTER PROCEDURE spGetChannelUsers
  @aChannelId INT
  AS
  BEGIN 
    SELECT U.UserId , U.Username , U.UserNumber , U.DisplayName , U.IsActive 
      FROM Tbl_User U
      INNER JOIN Tbl_UserChannel UC ON UC.UserId = U.UserId 
      WHERE UC.ChannelId = @aChannelId 
      ORDER BY U.username 
  END


EXEC spGetChannelUsers 1


