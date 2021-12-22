USE CCRequesterSetad
GO

SELECT *
FROM INFORMATION_SCHEMA.COLUMNS
  WHERE TABLE_NAME = 'TblRequest'
  AND COLUMN_NAME LIKE '%Id'

SELECT *
FROM INFORMATION_SCHEMA.TABLES
  WHERE TABLE_NAME LIKE '%load%' AND TABLE_TYPE = 'BASE TABLE'


  SELECT DISTINCT
   o.name AS Object_Name,
   o.type_desc
FROM sys.sql_modules m
   INNER JOIN
   sys.objects o
            ON m.object_id = o.object_id
WHERE m.definition Like '%title%'

-----------------------------------------------------------------------------------


SELECT * FROM Tbl_ServiceLogType 


SELECT T.*,L.* FROM TblServiceLog L
  INNER JOIN Tbl_ServiceLogType T ON L.LogTypeId = T.ServiceLogTypeId


EXEC spAddServiceLog @aLogTypeId = 1
                    ,@aApplicationId = 2
                    ,@aURL = N'www.adrinsoft.ir'
                    ,@aParams = N'{name : "Naser"  , task : "Run your own business !!!!"}'
                    ,@aResult = N'{status : False , Data : "Fuck Off :D"}'
                    ,@aIsSuccess = 1


TRUNCATE TABLE TblServiceLog

--INSERT INTO Tbl_ServiceLogType (ServiceLogTypeId, ServiceLogType) VALUES (2, N'GIS');
--  INSERT INTO Tbl_ServiceLogApplication (LogApplicationId, LogApplication) VALUES (2, N'TzServices');

SELECT * FROM TblServiceLog 
--  WHERE Result LIKE '%Tamir%'
  ORDER BY 1 DESC
  