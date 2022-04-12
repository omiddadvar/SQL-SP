USE WirelessDB
GO
ALTER PROCEDURE spGetDisplayName
  AS
  BEGIN
    SELECT U.UserId
          ,ISNULL(U.Username , 'Default UserName') AS Username
          ,ISNULL(U.UserNumber , -1) AS UserNumber
          ,ISNULL(U.DisplayName , 'Default DisplayName') AS DisplayName
          ,ISNULL(U.IsActive , 0) AS IsActive 
          ,ISNULL(U.Password , '') AS Password
          ,AC.*
    FROM Tbl_User U
      INNER JOIN Tbl_UserAccess UA ON U.UserId = UA.UserId
      INNER JOIN Tbl_Access AC ON UA.AccessId = AC.AccessId
  END

EXEC spGetDisplayName