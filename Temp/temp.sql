SELECT *
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'TblMPRequest'
  AND COLUMN_NAME LIKE '%date%'
ORDER BY COLUMN_NAME
--ORDER BY ORDINAL_POSITION

SELECT TOP 10 * FROM TblMPRequest tm

SELECT TOP 10 * FROM Tbl_MPFeeder tm


-------------------------------------------------------------------------------------
DECLARE @From AS VARCHAR(10) = '1398/04/30' 
DECLARE @To AS VARCHAR(10) = '1400/04/30'
--GO
SELECT  SUM(R.DisconnectPower) AS SumDisconnectPower, SUM(R.DisconnectInterval) AS SumDisconnectInterval,
  COUNT(MP.MPRequestId) AS Count, IsMPTamir FROM TblMPRequest MP
  INNER JOIN TblRequest R ON MP.MPRequestId = R.MPRequestId
  WHERE R.DisconnectDatePersian BETWEEN @From AND @To
  GROUP BY IsMPTamir

SELECT  SUM(R.DisconnectPower) AS SumDisconnectPower, SUM(R.DisconnectInterval) AS SumDisconnectInterval,
  COUNT(LP.LPRequestId) AS Count, IsLPTamir FROM TblLPRequest LP
  INNER JOIN TblRequest R ON LP.LPRequestId = R.LPRequestId
  WHERE R.DisconnectDatePersian BETWEEN @From AND @To
  GROUP BY IsLPTamir

SELECT COUNT(DISTINCT F.MPFeederId) AS CntFeeder , F.IsPrivate 
  FROM TblMPRequest MP
  INNER JOIN TblRequest R ON MP.MPRequestId = R.MPRequestId
  INNER JOIN Tbl_MPFeeder F ON F.MPFeederId = F.MPFeederId
  WHERE R.DisconnectDatePersian BETWEEN @From AND @To
  GROUP BY IsPrivate

SELECT SUM(R.DisconnectInterval) / COUNT(MP.MPRequestId) AS AverageDisInterval
  FROM TblMPRequest MP
  INNER JOIN TblRequest R ON MP.MPRequestId = R.MPRequestId
  WHERE IsMPTamir = 0 AND  R.DisconnectDatePersian BETWEEN @From AND @To