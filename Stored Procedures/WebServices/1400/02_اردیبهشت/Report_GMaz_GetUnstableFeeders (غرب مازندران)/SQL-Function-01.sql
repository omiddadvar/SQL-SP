USE CcRequesterSetad
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[GMaz_GetUnstableFeeders]') and OBJECTPROPERTY(id, N'IsScalarFunction') = 1)
  DROP FUNCTION [dbo].[GMaz_GetUnstableFeeders]
GO

CREATE FUNCTION [dbo].[GMaz_GetUnstableFeeders](
		@aMPFeederId INT,
		@aFROM VARCHAR(10),
		@aTo VARCHAR(10))
RETURNS NVARCHAR(2000)
AS
BEGIN
	DECLARE @lReasons AS NVARCHAR(2000)
	SET @lReasons = ''
	
	SELECT @lReasons += ',' + S.DisconnectGroupSet 
		FROM TblMPRequest MP
		INNER JOIN TblRequest R ON R.MPRequestId = MP.MPRequestId
		INNER JOIN Tbl_DiscONnectGroupSet S ON S.DiscONnectGroupSetId = MP.DiscONnectGroupSetId
		INNER JOIN  Tbl_DiscONnectGroup G ON G.DiscONnectGroupId = S.DiscONnectGroupId
		WHERE R.IsTamir = 0 AND
			R.EndJobStateId IN (2,3) AND
			MP.IsWarmLine = 0 AND
			MP.MPFeederId = @aMPFeederId AND 
			MP.DiscONnectDatePersian >= @aFROM AND
			MP.DisconnectDatePersian <= @aTo
		GROUP BY S.DisconnectGroupSet
	
	RETURN(SELECT SUBSTRING(@lReasons,2,LEN(@lReasons)))
END
