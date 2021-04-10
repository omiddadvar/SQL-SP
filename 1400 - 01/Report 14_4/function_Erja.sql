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