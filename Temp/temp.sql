USE CCRequesterSetad
GO

SELECT *
FROM INFORMATION_SCHEMA.COLUMNS
  WHERE 
    TABLE_NAME = 'TblErjaRequest' AND
    COLUMN_NAME LIKE '%dt%'


SELECT *
FROM INFORMATION_SCHEMA.TABLES
  WHERE TABLE_NAME LIKE '%access%' AND TABLE_TYPE = 'BASE TABLE'

  
--------------------------------------------------------------------------------------------------
--TRUNCATE TABLE TblServiceLog

SELECT * FROM TblServiceLog

SELECT * FROM Tbl_ServiceLogType

SELECT * FROM Tbl_ServiceLogApplication

-------------------------

SELECT * FROM Tbl_ServiceLogKeyType

SELECT * FROM Tbl_ServiceLogLevel

SELECT * FROM ViewServiceLog ORDER BY LogDT DESC

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

--------------------------------------------------------------------------

SELECT CriticalsAddress , WorkingAddress,MPPostId,* FROM TblTamirRequest WHERE TamirRequestNo IN (40099741)

SELECT * FROM Tbl_TestService
  
SELECT * FROM Tbl_TestServiceSMS
------------------------------------------------------------------------

SELECT * FROM TblErjaRequest 
---------------------------------------------
