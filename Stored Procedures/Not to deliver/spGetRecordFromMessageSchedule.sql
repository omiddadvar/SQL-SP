USE CCRequesterSetad
Go
ALTER PROCEDURE dbo.spGetRecordFromMessageSchedule 
AS
  BEGIN
  	DECLARE @lDT AS DATETIME = GETDATE()
  
  	UPDATE	Tbl_SendMessageSchedule 
      SET ScheduleStatusId = 4
  	  WHERE 
    		(ScheduleStatusId = 1 
    			OR (ScheduleStatusId = 3 
    				AND (Result = 'Bad Request' OR Result LIKE '%timeout%')))
    		and ScheduleTypeId = 1
    		and ExpiredDT < @lDT
  
  
  	SELECT TOP(1) * 
      FROM dbo.Tbl_SendMessageSchedule 
      WHERE (ScheduleStatusId IN (1,5)
  			OR (ScheduleStatusId IN (3,7)
            AND (Result = 'Bad Request' OR Result LIKE '%timeout%')))
  		AND @lDT >= ScheduleDT
  		AND ScheduleTypeId = 1
  		AND @lDT BETWEEN ScheduleDTFrom AND ScheduleDTTo
  	ORDER BY ScheduleDT
  END