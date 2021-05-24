USE CcRequesterSetad
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spGetReport_PostFeederLoad]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spGetReport_PostFeederLoad]
GO

CREATE PROCEDURE [dbo].[spGetReport_PostFeederLoad]
	@aPostCode as NVARCHAR(100),@aLimit AS INT,  @aISPostLoad AS BIT
AS
BEGIN
	IF @aISPostLoad = 1
	  BEGIN
		  SELECT TOP(@aLimit) P.LPPostName, P.LPPostCode, PL.PostCapacity, PL.PostPeakCurrent,
				PL.RCurrent, PL.SCurrent, PL.TCurrent, PL.NolCurrent,
				PL.LoadDateTimePersian AS PostLoadDate, PL.LoadTime AS PostLoadTime
			FROM Tbl_LPPost P
			INNER JOIN TblLPPostLoad PL ON PL.LPPostId = P.LPPostId
			where LPPostCode = @aPostCode
			ORDER BY P.LPPostId , PL.LoadDT DESC;
	  END
	ELSE
	  BEGIN
		  SELECT F.LPFeederName, F.LPFeederCode, Fl.FeederPeakCurrent,
				FL.RCurrent , FL.SCurrent , FL.TCurrent , FL.NolCurrent,
				FL.LoadDateTimePersian AS FeederLoadDate, FL.LoadTime AS FeederLoadTime
			FROM Tbl_LPFeeder F
			INNER JOIN 
			(SELECT ROW_NUMBER() OVER(PARTITION BY LPFeederId ORDER BY LoadDT DESC)
				AS Row# , * FROM TblLPFeederLoad) FL ON FL.LPFeederId = F.LPFeederId
			INNER JOIN Tbl_LPPost P ON P.LPPostId = F.LPPostId
			where FL.Row# <= @aLimit AND LPPostCode = @aPostCode
			ORDER BY F.LPFeederId , FL.LoadDT DESC;
	  END
END
GO
--TEST :
--EXEC spGetReport_PostFeederLoad '11-0259hg', 3, 1