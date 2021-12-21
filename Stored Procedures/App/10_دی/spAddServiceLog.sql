ALTER PROCEDURE spAddServiceLog
	@aLogTypeId As INT,
	@aURL AS NVARCHAR(500),
  @aParams AS NVARCHAR(4000),
  @aResult AS NVARCHAR(4000),
  @aIsSuccess AS BIT
As
BEGIN
  Declare @lNewId AS BIGINT,
    @lDT AS DATETIME = GETDATE()
  
  INSERT INTO TblServiceLog (LogTypeId,LogDT, URL, Params, Result, IsSuccessful)
    VALUES (@aLogTypeId,@lDT, @aURL, @aParams, @aResult, @aIsSuccess);

  SET @lNewId = @@IDENTITY
  IF @lNewId % 1000 = 0 BEGIN
    DELETE FROM TblServiceLog WHERE LogDT < DATEADD(DAY,-10,@lDT)
  END
  SELECT @lNewId AS LogTypeId
END

EXEC spAddServiceLog @aLogTypeId = 1
                    ,@aURL = N'www.adrinsoft.ir'
                    ,@aParams = N'{name : "omid" , task : "Run your business !!!!"}'
                    ,@aResult = N'{status : True , Data : "Fuck Off :D"}'
                    ,@aIsSuccess = 1


SELECT * FROM TblServiceLog 

