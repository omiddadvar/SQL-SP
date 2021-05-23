DECLARE @aFrom AS VARCHAR(11)
DECLARE @aTo AS VARCHAR(11)
DECLARE @aPostCode AS VARCHAR(11)
DECLARE @aISPostLoad AS BIT
DECLARE @aLimit AS INT
SET @aFrom = '1392/01/01'
SET @aTo = '1400/01/01'
SET @aPostCode = '11-0259hg'
SET @aISPostLoad = 1 --PostLoad OR FeederLoad
SET @aLimit = 5

EXEC spGetReport_PostFeederLoad '11-0259hg', 3 , '1392/01/01' , '1400/02/01' , 0







DECLARE @From AS VARCHAR(11)
DECLARE @To AS VARCHAR(11)
DECLARE @FeederCode AS VARCHAR(11)
SET @From = '1395/01/01'
SET @To = '1400/09/31'
SET @FeederCode = '20377'

SELECT MAX(P.PeakCurrentValue) AS Peak, FL.LoadDatePersian
FROM TblMPFeederPeak P
INNER JOIN Tbl_MPFeeder F ON F.MPFeederId = P.MPFeederId
INNER JOIN TblMPFeederPeak FL ON FL.MPFeederPeakId = P.MPFeederPeakId
WHERE F.MPFeederCode = @FeederCode AND
	P.LoadDatePersian BETWEEN @From AND @To
	AND P.IsDaily = 1
GROUP BY LEFT(P.LoadDatePersian,7) , FL.LoadDatePersian
ORDER BY FL.LoadDatePersian DESC

select top 1 * from TblMPFeederPeak
