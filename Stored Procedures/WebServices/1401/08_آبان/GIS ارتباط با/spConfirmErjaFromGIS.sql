ALTER Procedure spConfirmErjaFromGIS @aRequestNumber bigint=1,@aISDone bit=0
AS
BEGIN

	DECLARE 
		@lReferTo INT,
		@lErjaRequestId bigint,
		@lErjaStateId int


	SELECT @lReferTo = cast(ConfigValue AS INT)
	FROM Tbl_Config
	WHERE ConfigName = 'GIS_Refer_Unit'
	

	select top 1 
		@lErjaRequestId = TblErjaRequest.ErjaRequestId,
		@lErjaStateId = TblErjaRequest.ErjaStateId
	from TblErjaRequest
		INNER JOIN TblRequest ON TblErjaRequest.RequestId = TblRequest.RequestId
	where TblRequest.RequestNumber=@aRequestNumber and TblRequest.ReferToId=@lReferTo
	order by ErjaDT desc


	If(@lErjaRequestId IS NULL) 
	begin
		RAISERROR(N'خاموشی یافت نشد',16,1)
	END
	ELSE If(@lErjaStateId IN (4,5))
	begin
		RAISERROR(N'وضعیت خاموشی قابل تغییر نمی باشد',16,1)
	end
	
	
	
	If (@aISDone=1)
		set @lErjaStateId=4
	else
		set @lErjaStateId=5


	update TblErjaRequest
	set ErjaStateId=@lErjaStateId
	where ErjaRequestId=@lErjaRequestId

END