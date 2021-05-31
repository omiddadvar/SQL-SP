USE [CcRequesterSetad]
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[erjaStateId]') and OBJECTPROPERTY(id, N'IsScalarFunction') = 1)
  DROP FUNCTION [dbo].[erjaStateId]
GO

CREATE FUNCTION [dbo].[erjaStateId] (@RequestId bigint)
RETURNS int AS  
BEGIN 
	declare @lstateId int
	select top(1) @lstateId = ErjaStateId from TblErjaRequest
		where RequestId = @RequestId
		order by ErjaDT desc
	return @lstateId
END
GO
-----------------------------------------------------------------------------------------------------------------
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

GO
-----------------------------------------------------------------------------------------------------------------
USE CcRequesterSetad
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[KHSH_GetPercentANDLegalCurrent]') and OBJECTPROPERTY(id, N'IsScalarFunction') = 1)
  DROP FUNCTION [dbo].[KHSH_GetPercentANDLegalCurrent]
GO

CREATE FUNCTION dbo.KHSH_GetPercentANDLegalCurrent ( 
	@aTempK FLOAT, 
	@aTempAreaK FLOAT ,
	@aAltDec FLOAT,
	@aAltAreaDec FLOAT , 
	@aPostCapacity FLOAT ,
	@aPeakCurrent FLOAT)
RETURNS FLOAT AS
BEGIN 
	DECLARE @lK AS FLOAT = CASE WHEN @aTempK > 0 THEN @aTempK ELSE 
		(CASE WHEN @aTempAreaK > 0 THEN @aTempAreaK ELSE 0.8 END) END
	DECLARE @lDecrease AS FLOAT = CASE WHEN @aAltDec > 0 THEN @aAltDec ELSE 
		(CASE WHEN @aAltAreaDec > 0 THEN @aAltAreaDec ELSE 1 END) END
	DECLARE @lRes AS FLOAT
	DECLARE @lDenuminator AS FLOAT  = @aPostCapacity * @lK * @lDecrease * 1.44
	DECLARE @lNumerator AS FLOAT
	IF @aPeakCurrent IS NULL
		SET @lRes = @lDenuminator
	ELSE
		BEGIN
			SET @lNumerator = @aPeakCurrent * 100
			SET @lRes = CASE WHEN @lDenuminator > 0 THEN @lNumerator/@lDenuminator ELSE 0 END
		END
	RETURN CONVERT(DECIMAL(10, 2), @lRes)
END
GO
-----------------------------------------------------------------------------------------------------------------
USE CcRequesterSetad
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[KHSH_UnbalancedIndicatorA]') and OBJECTPROPERTY(id, N'IsScalarFunction') = 1)
  DROP FUNCTION [dbo].[KHSH_UnbalancedIndicatorA]
GO

CREATE FUNCTION dbo.KHSH_UnbalancedIndicatorA (
	 @aRCurrent FLOAT,
	 @aSCurrent FLOAT,
	 @aTCurrent FLOAT,
	 @aNolCurrent FLOAT)
RETURNS FLOAT AS
BEGIN 
--SQUARE()
--SQRT()
	DECLARE @lNumerator AS FLOAT = 6 * (SQUARE(@aRCurrent) + SQUARE(@aSCurrent) + SQUARE(@aTCurrent))
	DECLARE @lDenuminator AS FLOAT = SQUARE(@aRCurrent + @aSCurrent + @aTCurrent)
	DECLARE @lFraction AS FLOAT = CASE WHEN @lDenuminator > 0 THEN @lNumerator/@lDenuminator ELSE 0 END
	DECLARE @lRes AS FLOAT = 0
	IF @lFraction - 2 >= 0
		SET @lRes = SQRT(@lFraction - 2) * @aNolCurrent
	ELSE 
		SET @lRes = 0
	RETURN CONVERT(DECIMAL(10, 2), @lRes)
END

GO
-----------------------------------------------------------------------------------------------------------------
USE CcRequesterSetad
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[KHSH_UnbalancedIndicatorB]') and OBJECTPROPERTY(id, N'IsScalarFunction') = 1)
  DROP FUNCTION [dbo].[KHSH_UnbalancedIndicatorB]
GO

CREATE FUNCTION dbo.KHSH_UnbalancedIndicatorB (
	 @aNolCurrent FLOAT,
	 @aPostCapacity FLOAT)
RETURNS FLOAT AS
BEGIN 
	DECLARE @lRes as FLOAT = CASE WHEN @aPostCapacity > 0 
		THEN 100 * @aNolCurrent/(@aPostCapacity * 1.44) ELSE 0 END
	RETURN CONVERT(DECIMAL(10, 2), @lRes)
END
