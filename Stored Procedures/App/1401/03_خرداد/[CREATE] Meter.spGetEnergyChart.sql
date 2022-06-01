ALTER PROCEDURE Meter.spGetEnergyChart @aPersianDate AS VARCHAR(10)
AS
BEGIN
	SELECT HourlyMeterValueId
		,CONVERT(VARCHAR(5), CAST(MeterTime AS TIME), 108) AS Time
		,ROUND(ISNULL(Power_Active_Total, 0), 2) AS PowerActive
	FROM Meter.TblHourlyMeterValue
	WHERE MeterTime BETWEEN dbo.ShamsiDateTimeToMiladi(@aPersianDate, '00:00')
			AND dbo.ShamsiDateTimeToMiladi(@aPersianDate, '23:59')
	ORDER BY MeterTime ASC
END


EXEC Meter.spGetEnergyChart @aPersianDate = '1401/03/11'