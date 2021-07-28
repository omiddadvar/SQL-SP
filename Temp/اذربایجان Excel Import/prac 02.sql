USE CcRequesterSetad
--USE CcRequester
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[Sp-PostFeederSelect]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[Sp-PostFeederSelect] 
GO
CREATE PROCEDURE [dbo].[Sp-PostFeederSelect] 
	@feederIds as NVARCHAR(1000),
	@state as INT
AS
BEGIN
	Declare @lsql AS NVARCHAR(4000)
	/*------------------------- Post Load */
	IF @state = 1
		SET @lsql = 'SELECT DISTINCT pl.* , f.LPFeederCode , p.LPPostName
		, P.PostCapacity AS LPPostPostCapacity, P.IsTakFaze AS LPPostIsTakFaze
			FROM  Tbl_LPFeeder f
			INNER JOIN Tbl_LPPost p ON p.LPPostId = f.LPPostId
			LEFT JOIN TblLPFeederLoad fl ON f.LPFeederId = fl.LPFeederId
			LEFT JOIN TblLPPostLoad pl ON p.LPPostId = pl.LPPostId
			WHERE f.LPFeederId IN (' + @feederIds + ')
		ORDER BY pl.LPPostLoadId DESC';
	/*------------------------- Feeder Load */
	ELSE IF @state = 2
		SET @lsql = 'SELECT fl.* , f.LPFeederCode
			FROM  Tbl_LPFeeder f
			INNER JOIN TblLPFeederLoad fl ON f.LPFeederId = fl.LPFeederId
			WHERE f.LPFeederId IN (' + @feederIds + ')'
	/*------------------------- LoadDateTimePersian */
	ELSE IF @state = 3
		SET @lsql = 'Select Distinct fl.LoadDateTimePersian , 
			f.LPFeederCode from  TblLPFeederLoad fl
			Inner join Tbl_LPFeeder f on f.LPFeederId = fl.LPFeederId
			WHERE fl.LPFeederId IN (' + @feederIds + ')'
	EXEC(@lsql)
END
GO

exec [dbo].[Sp-PostFeederSelect] '20050983,20051023,20054050,
	20051067,20054053', 1;

exec [dbo].[Sp-PostFeederSelect] '20050983,20051023,20054050,
	20051067,20054053' , 2;



exec [dbo].[Sp-PostFeederSelect] '20050983,20051023' , 3;

select * from Tbl_LPFeeder where LPFeederId = 20050983
select * from TblLPFeederLoad where LPFeederId = 9900000990180523

select TOP(10) * from TblLPFeederLoad where EarthValue IS Not NULL

select TOP(10) * from Tbl_LPFeeder where LPFeederId IN ('20050983','20051023')

select  Count(*) from TblLPPostLoad 

select TOP(10) * from Tbl_LPPost 

delete from TblLPPostLoad where LPPostId Is Null

SELECT COLUMN_NAME
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'TblLPPostLoad'
ORDER BY ORDINAL_POSITION

select top(2) * from TblLPFeederLoad
select top(2) * from TblLPPostLoad where LPPostId Is Null



Select f.* , fl.LPFeederLoadId from  Tbl_LPFeeder f
Inner join TblLPFeederLoad fl on f.LPFeederId = fl.LPFeederId
WHERE fl.LPFeederId IN ('20050983','20051023')

Select f.* from  Tbl_LPFeeder f
WHERE f.LPFeederId IN ('20050983','20051023')

Select top(20) * from  Tbl_LPFeeder where LPFeederCode Is NOT NULL Order by LPFeederId desc
exec [dbo].[Sp-PostFeederSelect] '20054589', 1;
exec [dbo].[Sp-PostFeederSelect] '20054589' , 2;
select * from Tbl_LPPost where LPPostId = 10050844



SELECT PostPeakCurrent As Peak , * FROM Tbl_LPPost WHERE LPPostId IN (10050860,20051020);
SELECT FeederPeakCurrent As Peak , * FROM Tbl_LPFeeder WHERE LPFeederId IN ('20050983','20051023')
