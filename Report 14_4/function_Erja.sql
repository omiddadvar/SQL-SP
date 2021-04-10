USE [CcRequesterSetad]
GO
alter function erjaStateId (@RequestId bigint)
returns int
as 
begin
	if @RequestId is null
	begin
		return null
	end
	return (select top(1) ErjaStateId from TblErjaRequest
		where RequestId = @RequestId
		order by ErjaDT desc
	)
end
