USE CcRequesterSetad
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[GetPercentANDLegalCurrent]') and OBJECTPROPERTY(id, N'IsScalarFunction') = 1)
  DROP FUNCTION [dbo].[GetPercentANDLegalCurrent]
GO

CREATE FUNCTION dbo.GetPercentANDLegalCurrent ( 
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
	RETURN @lRes
END
GO