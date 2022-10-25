ALTER PROCEDURE [dbo].[spAddServiceLog]
  @aApplicationId AS INT,
	@aLogTypeId As INT,
	@aURL AS NVARCHAR(500),
  @aParams AS NVARCHAR(4000),
  @aResult AS NTEXT,
  @aCompany AS NVARCHAR(50),
  @aMethod AS VARCHAR(100),
  @aError AS NVARCHAR(4000),
  @aKeyId AS BIGINT,
  @aKeyTypeId AS INT = 1,
  @aIsSuccess AS BIT = 1
As
BEGIN
  Declare @lNewId AS BIGINT,
    @lDT AS DATETIME = GETDATE()
  
  INSERT INTO TblServiceLog (LogTypeId, ApplicationId, KeyTypeId, KeyId, LogDT, URL, Company, Method, Params, Result, Error, IsSuccessful)
    VALUES (@aLogTypeId, @aApplicationId, @aKeyTypeId ,@aKeyId ,@lDT, @aURL ,@aCompany ,@aMethod , @aParams, @aResult, @aError, @aIsSuccess);

  SET @lNewId = @@IDENTITY
  IF @lNewId % 100 = 0 BEGIN
    DELETE FROM TblServiceLog WHERE LogDT < DATEADD(DAY, -10, @lDT)
  END
  SELECT @lNewId AS ServiceLogId
END