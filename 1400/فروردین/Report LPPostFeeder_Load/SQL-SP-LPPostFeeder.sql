USE CcRequesterSetad
--USE CcRequester
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[Sp-PostFeederSelect]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[Sp-PostFeederSelect] 
GO
CREATE PROCEDURE [dbo].[Sp-PostFeederSelect] 
	@feederIds as VARCHAR(1000),
	@state as INT
AS
BEGIN
	Declare @lsql AS NVARCHAR(4000)
	------------------------- Post Load
	IF @state = 1
		SET @lsql = 'SELECT DISTINCT pl.* , f.LPFeederCode , p.LPPostName
		, P.PostCapacity AS LPPostPostCapacity, P.IsTakFaze AS LPPostIsTakFaze
			FROM  Tbl_LPFeeder f
			INNER JOIN Tbl_LPPost p ON p.LPPostId = f.LPPostId
			LEFT JOIN TblLPFeederLoad fl ON f.LPFeederId = fl.LPFeederId
			LEFT JOIN TblLPPostLoad pl ON p.LPPostId = pl.LPPostId
			WHERE f.LPFeederId IN (' + @feederIds + ')
		ORDER BY pl.LPPostLoadId DESC';
	------------------------- Feeder Load
	ELSE IF @state = 2
		SET @lsql = 'SELECT fl.* , f.LPFeederCode
			FROM  Tbl_LPFeeder f
			INNER JOIN TblLPFeederLoad fl ON f.LPFeederId = fl.LPFeederId
			WHERE f.LPFeederId IN (' + @feederIds + ')'
	------------------------- LoadDateTimePersian
	ELSE IF @state = 3
		SET @lsql = 'Select Distinct fl.LoadDateTimePersian , 
			f.LPFeederCode from  TblLPFeederLoad fl
			Inner join Tbl_LPFeeder f on f.LPFeederId = fl.LPFeederId
			WHERE fl.LPFeederId IN (' + @feederIds + ')'
	EXEC(@lsql)
END
GO
