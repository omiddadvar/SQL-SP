ALTER FUNCTION Emergency.GetMPFeederDisconnectCount (
	@MPFeederId AS INT
	,@IsDisconnectMPFeeder AS BIT
	,@MPFeederKeyId AS BIGINT,
  @aDayCount AS INT
	)
RETURNS INT
AS
BEGIN
	DECLARE @lCntMP1 AS INT = 0,
          @lCntMP2 AS INT = 0,
          @lCntFT AS INT = 0,
          @lDayLimit AS DATETIME = DATEADD(DAY, - @aDayCount ,CAST(CAST(GETDATE() as DATE) as DATETIME))

  IF @aDayCount IS NULL OR @aDayCount <= 0 BEGIN
    RETURN 0                                         	
  END

	SELECT @lCntMP1 = COUNT(*)
	FROM TblMPRequest
	INNER JOIN TblRequest ON TblMPRequest.MPRequestId = TblRequest.MPRequestId
	WHERE TblMPRequest.DisconnectDT >= @lDayLimit
		AND ISNULL(TblRequest.IsDisconnectMPFeeder, 1) = 1
		AND TblMPRequest.MPFeederId = @MPFeederId
    AND ISNULL(TblMPRequest.IsWarmLine , 0) = 0

	IF @IsDisconnectMPFeeder = 0
	BEGIN
		SELECT @lCntMP2 = COUNT(*)
		FROM TblMPRequest
		INNER JOIN TblRequest ON TblMPRequest.MPRequestId = TblRequest.MPRequestId
		INNER JOIN TblMPRequestKey ON TblMPRequest.MPRequestId = TblMPRequestKey.MPRequestId
		WHERE TblMPRequest.DisconnectDT >= @lDayLimit
			AND ISNULL(TblRequest.IsDisconnectMPFeeder, 1) = 0
			AND TblMPRequest.MPFeederId = @MPFeederId
			AND TblMPRequestKey.MPFeederKeyId = @MPFeederKeyId
      AND ISNULL(TblMPRequest.IsWarmLine , 0) = 0
	END

	SELECT @lCntFT = COUNT(*)
	FROM dbo.TblFogheToziDisconnect
	INNER JOIN TblRequest ON TblRequest.FogheToziDisconnectId = TblFogheToziDisconnect.FogheToziDisconnectId
	INNER JOIN dbo.TblFogheToziDisconnectMPFeeder ON TblFogheToziDisconnect.FogheToziDisconnectId = TblFogheToziDisconnectMPFeeder.FogheToziDisconnectId
		AND TblRequest.DisconnectDT >= @lDayLimit
		AND TblFogheToziDisconnectMPFeeder.MPFeederId = @MPFeederId

	RETURN ISNULL(@lCntFT, 0) + ISNULL(@lCntMP1, 0) + ISNULL(@lCntMP2, 0)
END
GO





