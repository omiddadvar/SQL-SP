USE CcRequesterSetad
GO
-------------------------Omid--------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spGMaz_GetUnstableFeeders]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spGMaz_GetUnstableFeeders]
GO

CREATE PROCEDURE [dbo].[spGMaz_GetUnstableFeeders] 
	@aFROMDate as varchar(10),
	@aToDate as varchar(10),
	@aMinTimes as int
	AS
	BEGIN
	SELECT MPF.MPFeederName, MPF.MPFeederCode, COUNT(*) AS OutageCount ,
	dbo.GMaz_GetUnstableFeeders(MPF.MPFeederId,@aFROMDate ,@aToDate ) As Reasons
	FROM TblMPRequest MP
		INNER JOIN TblRequest R ON R.MPRequestId = MP.MPRequestId
		INNER JOIN Tbl_MPFeeder MPF ON MPF.MPFeederId = MP.MPFeederId
		WHERE R.IsTamir = 0 AND
			R.EndJobStateId IN (2,3) AND
			MP.IsWarmLine = 0 AND
			MP.DiscONnectDatePersian >= @aFROMDate AND
			MP.DisconnectDatePersian <= @aToDate
		GROUP BY MPF.MPFeederId, MPF.MPFeederName, MPF.MPFeederCode
		HAVING COUNT(*) >= @aMinTimes
		ORDER BY OutageCount DESC
	END
GO


--Test
--EXEC [spGMaz_GetUnstableFeeders] '1397/12/01','1400/02/01',3


