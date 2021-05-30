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