CREATE FUNCTION Emergency.GetMPFeederLoadHourId (
	@MPFeederId AS INT,
	@aDisconnectDT AS DATETIME
	)
RETURNS BIGINT
AS
BEGIN
	Declare @lDayOfWeek as Int = DATEPART(dw,@aDisconnectDT),
			@lHour as INT = DATEPART(HOUR,@aDisconnectDT) + 1,
			@lID as BIGINT
	if @lDayOfWeek in (7,1,2,3,4) 
	begin
		Select TOP(1) @lID = H.MPFeederLoadHourId FROM Tbl_MPFeederLoad L
		INNER JOIN Tbl_MPFeederLoadHours H ON L.MPFeederLoadId = H.MPFeederLoadId
		Where L.MPFeederId = @MPFeederId 
			AND H.HourId = @lHour 
			AND L.RelDate <= @aDisconnectDT
			AND DATEPART(dw,L.RelDate) in (7,1,2,3,4)
		Order By L.RelDate DESC
	End
	Else 
	Begin 
		Select TOP(1) @lID = H.MPFeederLoadHourId FROM Tbl_MPFeederLoad L
		INNER JOIN Tbl_MPFeederLoadHours H ON L.MPFeederLoadId = H.MPFeederLoadId
		Where L.MPFeederId = @MPFeederId 
			AND H.HourId = @lHour 
			AND L.RelDate <= @aDisconnectDT
			AND DATEPART(dw,L.RelDate) = @lDayOfWeek
		Order By L.RelDate DESC
	END
	Return @lID
END
GO
