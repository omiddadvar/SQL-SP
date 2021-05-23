select top(10) * from TblMPFeederPeak
select top(10) * from Tbl_MPFeeder

select top(10) * from TblLPPostLoad
--20524 20322 20377

SELECT * FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_CATALOG='CcRequesterSetad' 
	AND TABLE_TYPE = 'BASE TABLE' 
	--AND TABLE_TYPE = 'VIEW' 
    AND TABLE_NAME like '%peak%'

select top(100) LPPostCode AS CODE, * from Tbl_LPPost 
select top(10) FeederPeakCurrent As FeederPeak, * from Tbl_LPFeeder where LPPostId = 10050368






---------------------------------------------------------------------------
DECLARE @From AS VARCHAR(11)
DECLARE @To AS VARCHAR(11)
DECLARE @FeederCode AS VARCHAR(11)
SET @From = '1395/01/01'
SET @To = '1400/01/01'
SET @FeederCode = '20377'

SELECT MAX(P.PeakCurrentValue) AS Peak FROM TblMPFeederPeak P
--select P.* from TblMPFeederPeak P
INNER JOIN Tbl_MPFeeder F ON F.MPFeederId = P.MPFeederId
WHERE F.MPFeederCode = @FeederCode AND 
	P.LoadDatePersian BETWEEN @From AND @To
--ORDER BY P.PeakCurrentValue DESC


-----------------------------------------------------------------------------------
SELECT P.LPPostName, P.LPPostCode,
	PL.PostCapacity, PL.RCurrent AS PostRCurrent, PL.SCurrent AS PostSCurrent, 
	PL.TCurrent AS PostTCurrent , PL.NolCurrent AS PostNolCurrent,
	PL.LoadDateTimePersian AS PostLoadDate, PL.LoadTime AS PostLoadTime,
	F.LPFeederName, F.LPFeederCode, F.LPFeederId, Fl.FeederPeakCurrent,
	FL.RCurrent , FL.SCurrent , FL.TCurrent , FL.NolCurrent,
	FL.LoadDateTimePersian AS FeederLoadDate, FL.LoadTime AS FeederLoadTime
	
	
SELECT F.LPFeederId , FL.LPFeederLoadId
	FROM Tbl_LPFeeder F
	INNER JOIN 
	(SELECT ROW_NUMBER() OVER(PARTITION BY LPFeederId ORDER BY LoadDT DESC)
		AS Row# , * FROM TblLPFeederLoad) FL ON FL.LPFeederId = F.LPFeederId
	INNER JOIN Tbl_LPPost P ON P.LPPostId = F.LPPostId
--	INNER JOIN TblLPPostLoad PL ON PL.LPPostId = P.LPPostId
	where FL.Row# <= 3 AND LPPostCode = '11-0259hg'
	ORDER BY F.LPFeederId , FL.LoadDT DESC;
	
	


SELECT * FROM(
SELECT 
  ROW_NUMBER() OVER(PARTITION BY LPFeederId ORDER BY LoadDT DESC)
    AS Row# , LPFeederId , LPFeederLoadId , LoadDT
FROM TblLPFeederLoad) t
WHERE t.Row# < 4 
ORDER BY LPFeederId,LoadDT DESC