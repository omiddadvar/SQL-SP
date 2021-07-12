USE CCRequesterSetad
GO
CREATE PROCEDURE Homa.spTraceArea
  @RequestId AS BIGINT
  AS 
  BEGIN
    DECLARE @startDT AS DATETIME
    DECLARE @arriveDT AS DATETIME
    DECLARE @endDT AS DATETIME

    SELECT * FROM Homa.TblJob tj
      INNER JOIN Homa.TblEkipStartMove tesm ON tj.OncallId = tesm.OnCallId
      INNER JOIN Homa.TblEkipArrive tea ON tj.JobId = tea.JobId
      WHERE tj.RequestId = @RequestId
      ORDER BY tj.DisconnectDT
  END
-------------------------------------