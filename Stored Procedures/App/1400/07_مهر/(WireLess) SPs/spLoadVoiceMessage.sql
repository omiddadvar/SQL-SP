USE WirelessDB
GO
ALTER PROCEDURE spLoadVoiceMessage
  @aMediaId INT
  AS
  BEGIN
    SELECT * FROM TblMedia 
      WHERE MediaId = @aMediaId AND (Hcls3000Id IS NOT NULL OR Content IS NOT NULL)
  END


--EXEC spLoadVoiceMessage @aMediaId = 922