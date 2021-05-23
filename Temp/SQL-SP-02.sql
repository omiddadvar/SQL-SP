IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spGetReport_FeederPeak]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spGetReport_FeederPeak]
GO

CREATE PROCEDURE [dbo].[spGetReport_FeederPeak]
	@aFeederCode as NVARCHAR(100), @aFrom AS VARCHAR(11), @aTo AS VARCHAR(11)
AS
BEGIN
	SELECT MAX(P.PeakCurrentValue) AS Peak, FL.LoadDatePersian
		FROM TblMPFeederPeak P
		INNER JOIN Tbl_MPFeeder F ON F.MPFeederId = P.MPFeederId
		INNER JOIN TblMPFeederPeak FL ON FL.MPFeederPeakId = P.MPFeederPeakId
		WHERE F.MPFeederCode = @aFeederCode AND
			P.LoadDatePersian BETWEEN @aFrom AND @aTo
			AND P.IsDaily = 1
		GROUP BY LEFT(P.LoadDatePersian,7) , FL.LoadDatePersian
		ORDER BY FL.LoadDatePersian DESC
END
GO

--TEST :
--EXEC spGetReport_FeederPeak '20377', '1393/01/01' , '1400/02/01'
