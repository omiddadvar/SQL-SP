ALTER PROCEDURE Homa.spGetSubscriberBilling (
	@CallerId AS VARCHAR(20)
	,@BillingIDTypeId AS INT
	)
AS
DECLARE @lMobile AS VARCHAR(20) = NULL
DECLARE @lTel AS VARCHAR(20) = NULL
DECLARE @lIsTel AS BIT

IF LEN(ISNULL(@CallerId, '')) < 8
BEGIN
	SELECT TOP 0 NULL AS BillingID
		,NULL AS BillingIDTypeId
		,NULL AS BillingIDType

	RETURN;
END

IF LEN(@CallerId) < 10
	OR (
		LEN(@CallerId) >= 10
		AND LEFT(RIGHT(@CallerId, 10), 1) <> 9
		)
BEGIN
--	SET @lTel = RIGHT(@CallerId, 8)
	SET @lTel = @CallerId
	SET @lIsTel = 1
END
ELSE
BEGIN
	IF LEFT(@CallerId, 2) = '09'
	BEGIN
		SET @CallerId = '09' + Right(@CallerId, LEN(@CallerId) - 2)
	END
	ELSE IF LEFT(@CallerId, 1) = '9'
	BEGIN
		SET @CallerId = '09' + Right(@CallerId, LEN(@CallerId) - 1)
	END
	ELSE IF LEFT(@CallerId, 4) = '0098'
	BEGIN
		SET @CallerId = '0' + Right(@CallerId, LEN(@CallerId) - 4)
	END
	ELSE IF LEFT(@CallerId, 3) = '098'
		OR LEFT(@CallerId, 3) = '+98'
	BEGIN
		SET @CallerId = '0' + Right(@CallerId, LEN(@CallerId) - 3)
	END

	SET @lMobile = @CallerId
	SET @lIsTel = 0
END

DECLARE @lRegisterId AS BIGINT = - 1

IF @lIsTel = 1
	SELECT @lRegisterId = RegisterId
	FROM Homa.TblRegister
	WHERE TelNo = @lTel
ELSE
	SELECT @lRegisterId = RegisterId
	FROM Homa.TblRegister
	WHERE MobileNo = @lMobile


DECLARE @Top AS INT = 1

IF @BillingIDTypeId = - 1
	SET @Top = 1000

SELECT TOP (@Top) tBill.BillingID
	,tBill.BillingIDTypeId
	,tBillType.BillingIDType
	,ISNULL(tBill.Address, tBill.AddressInBilling) AS [Address]
	,CAST(ISNULL(tOSMSt.OSMVoiceId, 0) AS BIGINT) AS VoiceId
FROM Homa.TblRegister tReg
	INNER JOIN Homa.TblRegisterBillingID tBill ON tReg.RegisterId = tBill.RegisterId
	INNER JOIN Homa.Tbl_BillingIDType tBillType ON tBill.BillingIDTypeId = tBillType.BillingIDTypeId
	LEFT JOIN Homa.TblOSMRegisterBillingID tOSMBill ON tBill.RegisterBillingIDId = tOSMBill.RegisterBillingIDId
	LEFT JOIN Homa.TblOSMStreet tOSMSt ON tOSMBill.OSMStreetId = tOSMSt.OSMStreetId
WHERE tReg.RegisterId = @lRegisterId
	AND (
		@BillingIDTypeId = - 1
		OR tBill.BillingIDTypeId = @BillingIDTypeId
		)
GO