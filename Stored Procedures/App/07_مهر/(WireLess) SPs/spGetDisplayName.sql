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
--      INTO #temp
      FROM Tbl_User U
      CROSS APPLY
        (
        SELECT TOP(1) A.* FROM Tbl_Access A
          INNER JOIN Tbl_UserAccess UA ON A.AccessId = UA.AccessId
          WHERE UA.UserId = U.UserId
          ORDER BY A.AccessId ASC
        ) AC
  END

EXEC spGetDisplayName