USE CcRequesterSetad
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[UnbalancedIndicatorB]') and OBJECTPROPERTY(id, N'IsScalarFunction') = 1)
  DROP FUNCTION [dbo].[UnbalancedIndicatorB]
GO

CREATE FUNCTION dbo.UnbalancedIndicatorB (
	 @aNolCurrent FLOAT,
	 @aPostCapacity FLOAT)
RETURNS FLOAT AS
BEGIN 
	DECLARE @lRes as FLOAT = CASE WHEN @aPostCapacity > 0 
		THEN 100 * @aNolCurrent/(@aPostCapacity * 1.44) ELSE 0 END
	RETURN @lRes
END
GO