ALTER FUNCTION Emergency.GetMPFeederLoadHourId (
	@MPFeederId AS INT
	,@aDisconnectDT AS DATETIME
	)
RETURNS BIGINT
AS
BEGIN
	DECLARE @lDayOfWeek AS INT = DATEPART(dw, @aDisconnectDT)
		,@lHour AS INT = DATEPART(HOUR, @aDisconnectDT) 
		,@lID AS BIGINT

	IF @lDayOfWeek IN (7,1,2,3,4)
	BEGIN
		SELECT TOP (1) @lID = H.MPFeederLoadHourId
		FROM Tbl_MPFeederLoad L
		INNER JOIN Tbl_MPFeederLoadHours H ON L.MPFeederLoadId = H.MPFeederLoadId
		WHERE L.MPFeederId = @MPFeederId
			AND H.HourId = @lHour
			AND L.RelDate <= @aDisconnectDT
			AND DATEPART(dw, L.RelDate) IN (7,1,2,3,4)
		ORDER BY L.RelDate DESC
	END
	ELSE
	BEGIN
		SELECT TOP (1) @lID = H.MPFeederLoadHourId
		FROM Tbl_MPFeederLoad L
		INNER JOIN Tbl_MPFeederLoadHours H ON L.MPFeederLoadId = H.MPFeederLoadId
		WHERE L.MPFeederId = @MPFeederId
			AND H.HourId = @lHour
			AND L.RelDate <= @aDisconnectDT
			AND DATEPART(dw, L.RelDate) = @lDayOfWeek
		ORDER BY L.RelDate DESC
	END

	RETURN @lID
END
GO