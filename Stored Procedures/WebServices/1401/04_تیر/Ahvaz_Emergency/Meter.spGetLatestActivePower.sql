
CREATE PROCEDURE Meter.spGetLatestActivePower
  @aDate VARCHAR(10),
  @aTime VARCHAR(5)
  AS
  BEGIN
    
    DECLARE @aFromDT AS DATETIME = DATEADD(MINUTE, -1, dbo.ShamsiDateTimeToMiladi(@aDate, @aTime))
    DECLARE @aToDT AS DATETIME = DATEADD(MINUTE, 15, @aFromDT)
    
    SELECT TOP(1) 
       dbo.mtosh(MeterTime) AS PersianDate
       ,(CAST(DATEPART(HOUR, MeterTime) AS VARCHAR(2)) + ':' + CAST(DATEPART(minute, MeterTime) AS VARCHAR(2))) AS Time 
       ,ROUND(ISNULL(Power_Active_Total, 0) , 2) AS ActivePower 
    FROM Meter.TblHourlyMeterValue
    WHERE MeterTime BETWEEN @aFromDT AND @aToDT 
    ORDER BY MeterTime DESC
   
  END


/*

EXEC Meter.spGetLatestActivePower @aDate = '1401/04/11'
                                ,@aTime = '10:31'

*/