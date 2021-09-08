USE WirelessDB
GO
CREATE PROCEDURE spGetDisplayName
  AS
  BEGIN
    SELECT UserId
          ,ISNULL(Username , 'Default UserName') AS Username
          ,ISNULL(UserNumber , -1) AS UserNumber
          ,ISNULL(DisplayName , 'Default DisplayName') AS DisplayName
          ,ISNULL(IsActive , 0) AS IsActive
      FROM Tbl_User
  END