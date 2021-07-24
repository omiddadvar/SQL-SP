SELECT *
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'TblLPRequest'
  AND COLUMN_NAME LIKE '%feeder%'
ORDER BY COLUMN_NAME
--ORDER BY ORDINAL_POSITION

SELECT TOP 10 * FROM TblMPRequest tm

SELECT TOP 10 * FROM Tbl_MPFeeder tm


---------------------------------------------------------------------------------------------------
DECLARE @From AS VARCHAR(10) = '1399/04/30' 
DECLARE @To AS VARCHAR(10) = '1400/04/30'

--SELECT  SUM(R.DisconnectPower) AS SumDisconnectPower, SUM(R.DisconnectInterval) AS SumDisconnectInterval,
--  COUNT(R.RequestId) AS Count, IsTamir FROM TblMPRequest MP
--  INNER JOIN TblRequest R ON MP.MPRequestId = R.MPRequestId
--  WHERE R.DisconnectDatePersian BETWEEN @From AND @To
--  GROUP BY IsTamir
--
SELECT  SUM(R.DisconnectPower) AS SumDisconnectPower, SUM(R.DisconnectInterval) AS SumDisconnectInterval,
  COUNT(R.RequestId) AS Count , IsTamir 
  FROM TblLPRequest LP
  INNER JOIN TblRequest R ON LP.LPRequestId = R.LPRequestId
  WHERE R.DisconnectDatePersian BETWEEN @From AND @To
  GROUP BY IsTamir

--SELECT COUNT(DISTINCT F.MPFeederId) AS CntFeeder , F.IsPrivate
--  FROM TblMPRequest MP
--  INNER JOIN TblRequest R ON MP.MPRequestId = R.MPRequestId
--  INNER JOIN Tbl_MPFeeder F ON F.MPFeederId = F.MPFeederId
--  WHERE R.DisconnectDatePersian BETWEEN @From AND @To
--  GROUP BY IsPrivate

SELECT SUM(R.DisconnectInterval) / COUNT(R.RequestId) AS AverageDisInterval
  FROM TblMPRequest MP
  INNER JOIN TblRequest R ON MP.MPRequestId = R.MPRequestId
  WHERE R.IsTamir = 0 AND  R.DisconnectDatePersian BETWEEN @From AND @To
-----------------------------------------------------------------------------------

  SELECT * FROM TblSubscribers ts


  DECLARE @Days INT = DATEDIFF(DAY ,  dbo.shtom('1400/04/20'), dbo.shtom('1400/05/25'))
  SELECT @Days;



-------------------------------------
DECLARE @From AS VARCHAR(10) = '1395/04/30' 
DECLARE @To AS VARCHAR(10) = '1400/04/30'
DECLARE @FromYM VARCHAR(10) = SUBSTRING( @From, 1, 7) 
DECLARE @TOYM VARCHAR(10) = SUBSTRING( @To, 1, 7)
DECLARE @Days INT = DATEDIFF(DAY , dbo.shtom(@From) , dbo.shtom(@TO))

 SELECT RequestId, MPRequestId , LPRequestId INTO #tmp FROM TblRequest WHERE DisconnectDatePersian BETWEEN @From AND @To

SELECT R.AreaId
      ,60 * 24 * @Days * SUM(CASE WHEN R.IsTamir = 1 THEN R.DisconnectPower END) / SUM(Sub.Energy) AS DisPowTamir
      ,60 * 24 * @Days * SUM(CASE WHEN R.IsTamir = 0 THEN R.DisconnectPower END) / SUM(Sub.Energy) AS DisPow
      FROM TblRequest R
      INNER JOIN #tmp t ON R.RequestId = t.RequestId
      INNER JOIN TblSubscribers Sub ON Sub.AreaId = R.AreaId
      WHERE R.DisconnectDatePersian BETWEEN @From AND @To
       AND Sub.YearMonth BETWEEN @FromYM AND @TOYM
      GROUP BY R.AreaId

  DROP TABLE #tmp