USE WirelessDB
GO
ALTER PROCEDURE spUpdateUser
  @aUserId INT
  ,@aPass NVARCHAR(100)
  ,@aNumber INT
  ,@aDisplay NVARCHAR(100)
  ,@aIsActive BIT
  AS
  BEGIN
    UPDATE Tbl_User 
      SET Password = @aPass
         ,UserNumber = @aNumber
         ,DisplayName = @aDisplay
         ,IsActive = @aIsActive
      WHERE UserId = @aUserId;
  END
--
--EXEC spUpdateUser 1 , '68E5D5EEC2D3AEDC27F20A1592A283B799EDA18221EC54C7994F73692BE526CE'
--  , 501 , '„œÌ—', 1