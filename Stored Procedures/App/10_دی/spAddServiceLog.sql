
ALTER PROCEDURE spAddServiceLog
  @aApplicationId AS INT,
	@aLogTypeId As INT,
	@aURL AS NVARCHAR(500),
  @aParams AS NVARCHAR(4000),
  @aResult AS NTEXT,
  @aError AS NVARCHAR(4000),
  @aIsSuccess AS BIT
As
BEGIN
  Declare @lNewId AS BIGINT,
    @lDT AS DATETIME = GETDATE()
  
  INSERT INTO TblServiceLog (LogTypeId,ApplicationId,LogDT, URL, Params, Result , Error, IsSuccessful)
    VALUES (@aLogTypeId, @aApplicationId, @lDT, @aURL, @aParams, @aResult, @aError, @aIsSuccess);

  SET @lNewId = @@IDENTITY
  IF @lNewId % 1000 = 0 BEGIN
    DELETE FROM TblServiceLog WHERE LogDT < DATEADD(DAY,-10,@lDT)
  END
  SELECT @lNewId AS ServiceLogId
END
