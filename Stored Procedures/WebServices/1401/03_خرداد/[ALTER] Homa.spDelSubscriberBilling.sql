ALTER PROCEDURE Homa.spDelSubscriberBilling (@CallerId as varchar(20), @BillingId as varchar(20))
AS
	DECLARE @lMobile AS varchar(20) = null
	DECLARE @lTel AS varchar(20) = null
	DECLARE @lIsTel as bit
	
	IF LEN(ISNULL(@CallerId,'')) < 8
	BEGIN
		SELECT CAST(0 AS BIT) as IsSuccess, 'CallerID Is InValid' AS ResultMessage
		RETURN;
	END
	
	IF LEN(@CallerId) < 10 OR (LEN(@CallerId) >= 10 AND LEFT(RIGHT(@CallerId,10),1) <> 9 )
	BEGIN
--		SET @lTel = RIGHT(@CallerId,8)
		SET @lTel = @CallerId
		SET @lIsTel = 1
	END
	ELSE
	BEGIN
		IF LEFT(@CallerId,2) = '09' 
		BEGIN
			SET @CallerId = '09' + Right( @CallerId , LEN(@CallerId) - 2 )
		END
		ELSE IF LEFT(@CallerId,1) = '9' 
		BEGIN
			SET @CallerId = '09' + Right( @CallerId , LEN(@CallerId) - 1 )
		END
		ELSE IF LEFT(@CallerId,4) = '0098' 
		BEGIN
			SET @CallerId = '0' + Right( @CallerId , LEN(@CallerId) - 4 )
		END
		ELSE IF LEFT(@CallerId,3) = '098' OR LEFT(@CallerId,3) = '+98'  
		BEGIN
			SET @CallerId = '0' + Right( @CallerId , LEN(@CallerId) - 3 )
		END
		SET @lMobile = @CallerId
		SET @lIsTel = 0
	END

	DECLARE @lRegisterId as bigint = -1

	IF @lIsTel = 1
		SELECT @lRegisterId = RegisterId FROM Homa.TblRegister where TelNo = @lTel
	ELSE
		SELECT @lRegisterId = RegisterId FROM Homa.TblRegister where MobileNo = @lMobile
	
PRINT(@lIsTel)
PRINT(@lTel)
PRINT(@lMobile)

	IF @lRegisterId = -1
	BEGIN
		SELECT CAST(0 AS BIT) as IsSuccess, 'CallerId IS NOT There' AS ResultMessage
		RETURN;
	END
	
	DECLARE @lRegisterBillingIDId as bigint = -1
	
	SELECT 
		@lRegisterBillingIDId = tBill.RegisterBillingIDId
	 FROM 
		Homa.TblRegisterBillingID tBill 
	WHERE 
		tBill.RegisterId = @lRegisterId 
		AND tBill.BillingID = @BillingId
		
	IF ISNULL(@lRegisterBillingIDId,-1) > -1
	BEGIN
	
		DECLARE @lTblBillingTableId AS int 
		SELECT @lTblBillingTableId = TableNameId from Tbl_TableName WHERE TableName = 'Homa.TblRegisterBillingID'	
		
		DECLARE @lWorkingAreaId AS int 
		SELECT @lWorkingAreaId = WorkingAreaId from TblDepartmentInfo
	
		DELETE FROM Homa.TblRegisterBillingID WHERE RegisterBillingIDId = @lRegisterBillingIDId
		
		INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand)
		SELECT 	
			'Homa.TblRegisterBillingID' AS TableName,
			@lTblBillingTableId AS TableNameId,
			@lRegisterBillingIDId AS PrimaryKeyId,
			1 AS Operation,
			NULL AS AreaId,
			@lWorkingAreaId AS WorkingAreaId,
			GETDATE() AS DataEntryDT,
			NULL AS SQLCommand
					
		SELECT CAST(1 AS BIT) as IsSuccess, '' AS ResultMessage
		RETURN;
	END
	ELSE
	BEGIN
		SELECT CAST(0 AS BIT) as IsSuccess, 'BillingID IS NOT There' AS ResultMessage
		RETURN;
	END
GO