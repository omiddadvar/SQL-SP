USE CCRequesterSetad
GO

SELECT *
FROM INFORMATION_SCHEMA.TABLES
  WHERE 
    TABLE_NAME like '%history%' 
--    AND TABLE_SCHEMA = 'Homa'
    AND 
  COLUMN_NAME LIKE '%history%'

SELECT *
FROM INFORMATION_SCHEMA.TABLES
  WHERE TABLE_NAME LIKE '%access%' AND TABLE_TYPE = 'BASE TABLE'

SELECT *
FROM INFORMATION_SCHEMA.COLUMNS
  WHERE  COLUMN_NAME LIKE '%PocketPC%'


--------------------------------------------------------------------------------------------------
---------------------------------------DJ Spin That Shit------------------------------------------
--------------------------------------------------------------------------------------------------

SELECT * FROM Emergency.Tbl_MPFeederLimitDay --WHERE MPFeederLimitTypeId = 990188911

SELECT * FROM Emergency.Tbl_WeekDay 

SELECT * FROM Emergency.Tbl_MPFeederLimitType 

SELECT * FROM Emergency.TblTiming
  
SELECT * FROM Emergency.Tbl_TimingState 


SELECT * FROM Emergency.TblTimingMPFeeder WHERE TimingId = 990000009
SELECT * FROM Emergency.Tbl_GroupMPFeeder 

SELECT * FROM Emergency.Tbl_MPFeederTemplate WHERE GroupMPFeederId = 990188852

SELECT * FROM Tbl_EndJobState 
--------------------------------------------------------------------------------------------------

EXEC Emergency.spGetFeederGroupPlanRaw @aGroupMPFeederId = 990188852

EXEC Emergency.spGetFeederGroupPlan 990188852,990188894
-------------------------------------------------------


EXEC spGetMPPosts_v2 @aAreaId = 2
                    ,@IsAll = 0
                    ,@aWhereClause = ''
                    ,@aWhereClauseOR = ''

EXEC spGetMPFeeders_v2 @aAreaId = 2
                      ,@aMPPostId = 5
                      ,@IsAll = 0

---------------------------------------------------------

EXEC Emergency.spGetMPFeederTiming @aTiminigId = 990188926


SELECT * FROM BTbl_Holiday

EXEC Emergency.spTimingMonitoring @aGroupMPFeederIds = ''
                                     ,@aFromDate = '1401/01/20'
                                     ,@aFromTime = '24:00'
                                     ,@aToDate = '1401/03/01'
                                     ,@aToTime = '12:00' 

EXEC Emergency.spTimingMonitoring '','1401/02/21','00:00','1401/02/21','23:59'



--DELETE FROM Tbl_FormObjectApplication WHERE FormObjectApplicationId >= 4000


SELECT * FROM Tbl_FormObject  WHERE FormObjectId >= 4000

SELECT * FROM Tbl_FormObjectApplication WHERE FormObjectApplicationId >= 4000


SELECT * FROM Tbl_SysObj 


EXEC Emergency.spGetFeederGroupPlan 990188955,-1


SELECT * FROM TblMPFeederPeak

EXEC Emergency.spGetLimitDays @aMPFeederLimitTypeId = 0

EXEC Emergency.spCheckMPFeedersLimit @aStartDate = '1401/03/07'
                                    ,@aStartTime = '23:00'
                                    ,@aEndDate = '1401/03/08'
                                    ,@aEndTime = '05:00'
                                    ,@aMPFeederTemplateIDs = ''

SELECT * FROM Emergency.Tbl_MPFeederLimitType WHERE MPFeederLimitTypeId =990188911


SELECT * FROM Emergency.Tbl_MPFeederLimitDay WHERE MPFeederLimitTypeId =990188911


SELECT * FROM BTbl_Holiday WHERE HolidayDatePersian BETWEEN '1401/03/10' AND '1401/03/11'

SELECT * FROM Tbl_Config WHERE ConfigId = 715

-----------------------------------------------------------------------------------------------

EXEC Emergency.spGetFeederGroupPlan @aGroupMPFeederId = 990188866
                                   ,@aDayCount = 20
                                   ,@aTimingId = 990000003

/*

DELETE FROM Emergency.TblTiming WHERE ForecastDisconnectDatePersian = '1401/03/23'

*/

SELECT * FROM Emergency.TblTiming WHERE ForecastDisconnectDatePersian = '1401/03/23'

SELECT * FROM Emergency.TblTimingMPFeeder WHERE TimingId = 990000135


-----------------------------------------------------------------------------------------------


EXEC spGetDisconnectPower @aStartDate = '1401/01/30'
                         ,@aTime = '10:00'


---------------

EXEC Meter.spGetEnergyChart @aPersianDate = '1401/04/11'


SELECT * FROM Meter.TblHourlyMeterValue



SELECT * FROM Emergency.TblTiming 


SELECT * FROM Tbl_TamirRequestSubject 

SELECT * FROM TblRequest WHERE DisconnectRequestForId
SELECT * FROM Tbl_DisconnectLPRequestFor 


