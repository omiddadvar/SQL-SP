USE CcRequesterSetad
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spOmid]
GO
--Report_ErjaList_OperationAvrageInterval
CREATE PROCEDURE [dbo].[spOmid] 
	@aFROMDate as varchar(10),
	@aToDate as varchar(10),
	@aMinTimes as int
AS
BEGIN
	SELECT G.DiscONnectGroupId ,G.DiscONnectGroup , AVG(TblLPRequest.DisconnectInterval) AS Average FROM TblLPRequest
		INNER JOIN TblRequest ON TblRequest.LPRequestId = TblLPRequest.LPRequestId
		INNER JOIN Tbl_DiscONnectGroupSet S ON S.DiscONnectGroupSetId = TblLPRequest.DiscONnectGroupSetId
		INNER JOIN Tbl_DiscONnectGroup G ON G.DiscONnectGroupId = S.DiscONnectGroupId
		WHERE TblLPRequest.DiscONnectDatePersian >= @aFROMDate AND
				TblLPRequest.DisconnectDatePersian <= @aToDate
		GROUP BY G.DiscONnectGroupId , G.DiscONnectGroup 
		HAVING COUNT(*) >= @aMinTimes
	UNION
	SELECT G.DiscONnectGroupId ,G.DiscONnectGroup , AVG(TblMPRequest.DisconnectInterval) AS Average FROM TblMPRequest
		INNER JOIN TblRequest ON TblRequest.MPRequestId = TblMPRequest.MPRequestId
		INNER JOIN Tbl_DiscONnectGroupSet S ON S.DiscONnectGroupSetId = TblMPRequest.DiscONnectGroupSetId
		INNER JOIN Tbl_DiscONnectGroup G ON G.DiscONnectGroupId = S.DiscONnectGroupId
		WHERE TblMPRequest.DiscONnectDatePersian >= @aFROMDate AND
				TblMPRequest.DisconnectDatePersian <= @aToDate
		GROUP BY G.DiscONnectGroupId , G.DiscONnectGroup 
		HAVING COUNT(*) >= @aMinTimes
END
GO




--------------------------------------------------------------------------------------------------------------
USE CcRequesterSetad
GO
DECLARE @aFROMDate as varchar(10)
DECLARE @aToDate as varchar(10)
DECLARE @aMinTimes as int
Set @aFROMDate = '1397/12/01'
Set @aToDate = '1400/02/01'
Set @aMinTimes = 1

SELECT G.DiscONnectGroupId ,G.DiscONnectGroup , COUNT(*)  AS Times,
		@aFROMDate AS FromDate, @aToDate AS ToDate
		FROM TblLPRequest
		INNER JOIN TblRequest ON TblRequest.LPRequestId = TblLPRequest.LPRequestId
		INNER JOIN Tbl_DiscONnectGroupSet S ON S.DiscONnectGroupSetId = TblLPRequest.DiscONnectGroupSetId
		INNER JOIN  Tbl_DiscONnectGroup G ON G.DiscONnectGroupId = S.DiscONnectGroupId
		WHERE TblLPRequest.DiscONnectDatePersian >= @aFROMDate AND
				TblLPRequest.DisconnectDatePersian <= @aToDate
		GROUP BY G.DiscONnectGroupId , G.DiscONnectGroup
		HAVING COUNT(*) >= @aMinTimes
UNION
SELECT G.DiscONnectGroupId ,G.DiscONnectGroup ,COUNT(*) AS Times,
		@aFROMDate AS FromDate, @aToDate AS ToDate
		FROM TblMPRequest
		INNER JOIN TblRequest ON TblRequest.MPRequestId = TblMPRequest.MPRequestId
		INNER JOIN Tbl_DiscONnectGroupSet S ON S.DiscONnectGroupSetId = TblMPRequest.DiscONnectGroupSetId
		INNER JOIN  Tbl_DiscONnectGroup G ON G.DiscONnectGroupId = S.DiscONnectGroupId
		WHERE TblMPRequest.DiscONnectDatePersian >= @aFROMDate AND
				TblMPRequest.DisconnectDatePersian <= @aToDate
		GROUP BY G.DiscONnectGroupId , G.DiscONnectGroup
		HAVING COUNT(*) >= @aMinTimes
	ORDER BY G.DiscONnectGroup ASC
	
	
	
	
select * FROM Tbl_DiscONnectGroupSet order by SortOrder
