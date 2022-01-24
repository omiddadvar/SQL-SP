USE CCRequesterSetad
GO

SELECT *
FROM INFORMATION_SCHEMA.COLUMNS
  WHERE TABLE_NAME = 'TblLPFeederLoad'
  AND COLUMN_NAME LIKE '%current%'

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

-------
SELECT * FROM Tbl_ServiceLogKeyType

SELECT * FROM Tbl_ServiceLogLevel

SELECT * FROM ViewServiceLog ORDER BY LogDT DESC

-------
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


-----------------------------------------------------------

--Maz--http://172.1ي.50.103/TZHavades/Tavanir
SELECT * FROM Tbl_Config WHERE ConfigName LIKE '%Tozi%'


SELECT * FROM Tbl_AccessType

SELECT * FROM TblUserAccess

SELECT * FROM Tbl_User
----------------------------------------------

--Setting = 1
--Reason = 2
--ChangeDate = 3
--ReportReliability = 4
--ReportKambood = 5
--ReportSerghati = ي

SET IDENTITY_INSERT Tbl_AccessType ON
INSERT INTO Tbl_AccessType (AccessTypeId,AccessType, AccessTypeName)
  VALUES  (4,N'Monitoring', N'توانایی دسترسی به مانیتورینگ'),
          (5,N'ReportReliability', N'توانايي گزارشگيري شاخص هاي قابليت اطمينان '),
          (6,N'ReportKambood', N'توانايي گزارشگيري کمبود توليد ساعت به ساعت'),
          (7,N'ReportSerghati', N'توانايي گزارشگيري تجهيزات سرقتي')
SET IDENTITY_INSERT Tbl_AccessType Off


SELECT * FROM Tbl_AccessType

--DELETE FROM Tbl_AccessType WHERE AccessType LIKE 'Report%'

/* -----------------Mazandaran Test
SELECT TamirRequestStateId,* FROM TblTamirRequest WHERE TamirRequestNo = 40099749

SELECT * FROM Tbl_TamirRequestState

--UPDATE TblTamirRequest SET TamirRequestStateId = 2 WHERE TamirRequestId = 9900000000000051
*/


EXEC spGetReport_9_18_ByOperator @FromDate = '1400/01/01'
                                ,@ToDate = '1400/11/01'
                                ,@AreaId = 2
                                ,@OperatorIds = ''
                                ,@IsLight = 0


EXEC spGetReport_9_18_ByOperator '1400/01/01','1400/11/04',2,'',0


EXEC spTavanir_ReportSerghati @FromDate = '1400/01/01'
                             ,@ToDate = '1400/01/30'

SELECT * FROM Tbl_AreaUser WHERE UserName LIKE '%usrs%'


