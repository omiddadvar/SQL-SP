USE CcRequesterSetad
SELECT TOP 5 * FROM TblDejenctor

EXEC Homa.spGetStateTrace 9900000000001438,9900000000000041,'2021/07/14 2:20:00 PM','2021/07/14 3:00:00 PM'


  SELECT TraceDatePersian , OnCallId FROM Homa.TblTrace
    GROUP BY TraceDatePersian , OnCallId
    ORDER BY  TraceDatePersian DESC


  SELECT * FROM ViewMonitoringEx WHERE requestId IN (9900000000001439)