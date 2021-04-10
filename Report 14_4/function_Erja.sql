USE [CcRequesterSetad]
GO
alter function erjaStateId (@RequestId bigint)
returns int
as 
begin
	declare @lstateId int
	select top(1) @lstateId = ErjaStateId from TblErjaRequest
		where RequestId = @RequestId
		order by ErjaDT desc
	return @lstateId
end
