USE WirelessDB
GO
ALTER PROCEDURE spGetChannelUsers
  @aChannelId INT
  AS
  BEGIN 
    SELECT U.UserId , U.Username , U.UserNumber , U.DisplayName , U.IsActive , AC.*
      FROM Tbl_User U
      INNER JOIN Tbl_UserChannel UC ON UC.UserId = U.UserId
      CROSS APPLY
        (
        SELECT TOP(1) A.* FROM Tbl_Access A
          INNER JOIN Tbl_UserAccess UA ON A.AccessId = UA.AccessId
          WHERE UA.UserId = U.UserId
          ORDER BY A.AccessId ASC
        ) AC
      WHERE UC.ChannelId = @aChannelId 
      ORDER BY U.username 
  END


EXEC spGetChannelUsers 1
