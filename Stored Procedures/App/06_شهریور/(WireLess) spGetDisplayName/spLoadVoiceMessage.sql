USE WirelessDB
GO
CREATE PROCEDURE spLoadVoiceMessage
  @aMediaId INT
  AS
  BEGIN
    SELECT * FROM TblMedia WHERE MediaId = @aMediaId
  END


--EXEC spLoadVoiceMessage @aMediaId = 900