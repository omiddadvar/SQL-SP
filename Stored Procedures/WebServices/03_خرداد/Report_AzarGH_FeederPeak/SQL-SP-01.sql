USE CcRequesterSetad
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spGetReport_FeederPeak]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spGetReport_FeederPeak]
GO
CREATE PROCEDURE [dbo].[spGetReport_FeederPeak]
	@aFeederCode AS NVARCHAR(100), @aFROM AS VARCHAR(11), @aTo AS VARCHAR(11)
AS
BEGIN

	SELECT TblMPFeederPeak.MPFeederId, MAX(PeakCurrentValue) AS peak, LEFT(LoadDatePersian,7) AS date INTO #tmp
	FROM TblMPFeederPeak
	INNER JOIN Tbl_MPFeeder F ON F.MPFeederId = TblMPFeederPeak.MPFeederId
		WHERE MPFeederCode = @aFeederCode 
		AND LoadDatePersian between @aFROM and @aTo
			AND IsDaily = 1
	GROUP BY 
		LEFT(LoadDatePersian,7),
		TblMPFeederPeak.MPFeederId
	ORDER BY MAX(PeakCurrentValue) DESC

	SELECT TblMPFeederPeak.* INTO #tmp2 FROM 
	TblMPFeederPeak
	inner join #tmp ON TblMPFeederPeak.MPFeederId = #tmp.MPFeederId
	and PeakCurrentValue = #tmp.peak
	and LEFT(LoadDatePersian,7) = #tmp.date

	ORDER BY LoadDatePersian DESC


	SELECT LoadDatePersian, PeakCurrentValue AS Peak FROM 
	(
	SELECT 
	  ROW_NUMBER() OVER(PARTITION BY Left(LoadDatePersian,7) ORDER BY LoadDatePersian DESC) 
		AS Row#,
	 *
	FROM #tmp2
	) t1 WHERE Row# = 1
	ORDER BY LoadDatePersian DESC


	DROP TABLE #tmp
	DROP TABLE #tmp2
END
GO

--TEST :
--EXEC spGetReport_FeederPeak '20524', '1390/01/01' , '1400/02/01'
