USE WirelessDB
GO
ALTER PROCEDURE spLoadVoiceMessage
  @aMediaId INT
  AS
  BEGIN
    SELECT * FROM TblMedia 
      WHERE MediaId = @aMediaId AND Content IS NOT NULL
  END


--EXEC spLoadVoiceMessage @aMediaId = 922