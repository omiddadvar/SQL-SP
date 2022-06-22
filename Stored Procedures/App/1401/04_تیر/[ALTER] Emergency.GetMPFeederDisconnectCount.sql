ALTER FUNCTION Emergency.GetMPFeederDisconnectCount (
	@MPFeederId AS INT
	,@IsDisconnectMPFeeder AS BIT
	,@MPFeederKeyId AS BIGINT,
  @aDayCount AS INT
	)
RETURNS INT
AS
BEGIN
	DECLARE @CntMP1 AS INT = 0
	DECLARE @CntMP2 AS INT = 0
	DECLARE @CntFT AS INT = 0

  IF @aDayCount IS NULL OR @aDayCount <= 0 BEGIN
    RETURN 0                                         	
  END

	SELECT @CntMP1 = COUNT(*)
	FROM TblMPRequest
	INNER JOIN TblRequest ON TblMPRequest.MPRequestId = TblRequest.MPRequestId
	WHERE TblMPRequest.DisconnectDT >= DATEADD(day, - @aDayCount, getdate())
		AND ISNULL(TblRequest.IsDisconnectMPFeeder, 1) = 1
		AND TblMPRequest.MPFeederId = @MPFeederId

	IF @IsDisconnectMPFeeder = 0
	BEGIN
		SELECT @CntMP2 = COUNT(*)
		FROM TblMPRequest
		INNER JOIN TblRequest ON TblMPRequest.MPRequestId = TblRequest.MPRequestId
		INNER JOIN TblMPRequestKey ON TblMPRequest.MPRequestId = TblMPRequestKey.MPRequestId
		WHERE TblMPRequest.DisconnectDT >= DATEADD(day, - @aDayCount, getdate())
			AND ISNULL(TblRequest.IsDisconnectMPFeeder, 1) = 0
			AND TblMPRequest.MPFeederId = @MPFeederId
			AND TblMPRequestKey.MPFeederKeyId = @MPFeederKeyId
	END

	SELECT @CntFT = COUNT(*)
	FROM dbo.TblFogheToziDisconnect
	INNER JOIN TblRequest ON TblRequest.FogheToziDisconnectId = TblFogheToziDisconnect.FogheToziDisconnectId
	INNER JOIN dbo.TblFogheToziDisconnectMPFeeder ON TblFogheToziDisconnect.FogheToziDisconnectId = TblFogheToziDisconnectMPFeeder.FogheToziDisconnectId
		AND TblRequest.DisconnectDT >= DATEADD(day, - @aDayCount, getdate())
		AND TblFogheToziDisconnectMPFeeder.MPFeederId = @MPFeederId

	RETURN isnull(@CntFT, 0) + isnull(@CntMP1, 0) + isnull(@CntMP2, 0)
END
GO
