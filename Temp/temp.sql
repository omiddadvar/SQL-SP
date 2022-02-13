USE CCRequesterSetad
GO

SELECT *
FROM INFORMATION_SCHEMA.COLUMNS
  WHERE TABLE_NAME = 'TblTamirRequest'
  AND COLUMN_NAME LIKE '%user%'


SELECT *
FROM INFORMATION_SCHEMA.TABLES
  WHERE TABLE_NAME LIKE '%access%' AND TABLE_TYPE = 'BASE TABLE'

SELECT DISTINCT
   o.name AS Object_Name,
   o.type_desc
FROM sys.sql_modules m
   INNER JOIN
   sys.objects o
            ON m.object_id = o.object_id
WHERE m.definition Like '%spReportHourlyDisconnectPower%'
  
EXEC sp_fkeys 'Tbl_RelehType'

EXEC sp_spaceused 'TblRequest'

EXEC sp_spaceused 'TblServiceLog'

--------------------------------------------------------------------------------------------------
--TRUNCATE TABLE TblServiceLog

SELECT * FROM TblServiceLog

SELECT * FROM Tbl_ServiceLogType

SELECT * FROM Tbl_ServiceLogApplication

-------------------------

SELECT * FROM Tbl_ServiceLogKeyType

SELECT * FROM Tbl_ServiceLogLevel

SELECT * FROM ViewServiceLog ORDER BY LogDT DESC

-------------------------
SELECT T.*,L.* FROM TblServiceLog L
  INNER JOIN Tbl_ServiceLogType T ON L.LogTypeId = T.ServiceLogTypeId
  INNER JOIN Tbl_ServiceLogApplication A ON L.LogTypeId = T.ServiceLogTypeId


EXEC spAddServiceLog @aLogTypeId = 1
                    ,@aApplicationId = 2
                    ,@aURL = N'www.adrinsoft.ir'
                    ,@aParams = N'{name : "Naser"  , task : "Run your own business !!!!"}'
                    ,@aResult = N'{status : False , Data : "Fuck Off :D"}'
                    ,@aCompany = N'Tazarv'
                    ,@aMethod = 'GetSubscriberParents'
                    ,@aError= 'No Error'
                    ,@aKeyId = 400098655
                    ,@aKeyTypeId = 1
                    ,@aIsSuccess = 1
                    ,@aLevelId = 1

  
SELECT * FROM TblServiceLog ORDER BY 1 DESC

-------------------------------------------------------------------------------------

SELECT * FROM Tbl_NetworkType 

SELECT * FROM TblRequestPostFeeder

SELECT * FROM TblTamirRequestKey

SELECT * FROM Tbl_AreaUser

EXEC spReportHourlyDisconnectPower '1400/03/01'


EXEC spReportHourlyDisPowerGeneral @aFromDate = '1400/05/01'
                                  ,@aToDate = '1400/0ي/31'

EXEC spReportHourlyDisPowerGeneral '1400/10/25' , '1400/10/25'

----------------------------------------------

EXEC spGetReport_9_18_ByOperator @FromDate = '1400/01/01'
                                ,@ToDate = '1400/11/01'
                                ,@AreaId = 1
                                ,@OperatorIds = ''
                                ,@IsLight = 0
----------------------------------------------------------

SELECT OffPostCount,* FROM TblMultistepConnection ORDER BY ConnectDT DESC

SELECT dbo.GetLPTransCount(54 , 1)



SELECT DisconnectInterval,DisconnectPower,* FROM TblRequest WHERE RequestNumber IN(4009921166,4009921169)

SELECT * FROM Tbl_Area

-------------------------------------------------------------



SELECT * FROM Tbl_Subscriber  WHERE SubscriberId = 10050006


SELECT * FROM Tbl_SubscriberInfo WHERE BillingID = '123456'

SELECT * FROM Tbl_Config WHERE ConfigName LIKE '%TZHavadesTavanirURL%'

  
UPDATE Tbl_Config SET ConfigValue = 'http://localhost:3136/Tavanir' WHERE ConfigName LIKE '%TZHavadesTavanirURL%'

SELECT TOP 10 * FROM Tbl_SMS 

--------------------------------------------------------------------------

SELECT * FROM Tbl_Area 

SELECT CriticalsAddress , WorkingAddress,* FROM TblTamirRequest WHERE TamirRequestNo IN (40099740)


SELECT * FROM Tbl_AreaUser WHERE AreaUserId = 1004

