CREATE PROCEDURE spTestSerivceEditSMSSubscriber 
  @Name NVARCHAR(100),
  @Mobile VARCHAR(50),
  @IsActive BIT,
  @MaxNumbers INT = 5
  As
  BEGIN
  	DECLARE @ID AS BIGINT

    SELECT @ID = ISNULL(TestServiceSMSId,-1) FROM Tbl_TestServiceSMS WHERE MobileNo LIKE '%' + @Mobile + '%'

    IF @ID > 0
    BEGIN
      UPDATE Tbl_TestServiceSMS SET 
    END
    ELSE
    BEGIN
      INSERT INTO Tbl_TestServiceSMS (Name, MobileNo, IsActive)
        VALUES (@Name, @Mobile, @IsActive)
    END

  END







