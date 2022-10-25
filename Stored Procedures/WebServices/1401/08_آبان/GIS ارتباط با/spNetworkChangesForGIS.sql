ALTER PROCEDURE [dbo].[spNetworkChangesForGIS] @aFromDate VARCHAR(10)
	,@aToDate VARCHAR(10)
AS
BEGIN
	DECLARE @lReferTo INT

	SELECT @lReferTo = cast(ConfigValue AS INT)
	FROM Tbl_Config
	WHERE ConfigName = 'GIS_Refer_Unit'
	

	SELECT TblRequest.RequestNumber
		,Isnull(NULLIF(cast(TblErjaRequest.Comments as nvarchar(max)),''), TblRequest.Comments) AS Comments
		,ISNULL(TblErjaRequest.GpsX, TblRequest.GPSx) AS GPSx
		,ISNULL(TblErjaRequest.GpsY, TblRequest.GpsY) AS GpsY
		,ISNULL(NULLIF(cast(TblErjaRequest.[Address] as nvarchar(max)),''), TblRequest.[Address]) AS [Address]
		,Tbl_ErjaReason.ErjaReason
	FROM TblErjaRequest
	INNER JOIN TblRequest ON TblErjaRequest.RequestId = TblRequest.RequestId
	LEFT JOIN Tbl_ErjaReason ON TblErjaRequest.ErjaReasonId = Tbl_ErjaReason.ErjaReasonId
	WHERE TblErjaRequest.ErjaDatePersian BETWEEN @aFromDate
			AND @aToDate
		AND TblErjaRequest.ReferToId = @lReferTo
END

