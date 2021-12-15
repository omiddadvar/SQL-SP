USE CCRequesterSetad
GO


SELECT *
FROM INFORMATION_SCHEMA.COLUMNS
  WHERE TABLE_NAME = 'TblTamirRequest'
  AND COLUMN_NAME LIKE '%subj%'

SELECT *
FROM INFORMATION_SCHEMA.TABLES
  WHERE TABLE_NAME LIKE '%subject%' AND TABLE_TYPE = 'BASE TABLE'

  SELECT DISTINCT
   o.name AS Object_Name,
   o.type_desc
FROM sys.sql_modules m
   INNER JOIN
   sys.objects o
            ON m.object_id = o.object_id
WHERE m.definition Like '%CCRequesterSetad%'
---------------------------------------


EXEC spGetReport_8_24 '1400/09/23', '','12:00','','-1','-1','1','-1'


SELECT * FROM Tbl_MPFeederLoadHours WHERE MPFeederLoadHourId = 9900000990180542

EXEC spGetReport_8_24 '1400/09/23', '1400/09/23','','','-1','-1','1','-1'

SELECT * from Tbl_Hour

SELECT TOP 10 IsDuplicatedRequest ,EndJobStateId ,  DuplicatedRequestId ,RequestNumber,* 
    FROM TblRequest 
  WHERE IsDuplicatedRequest = 1 AND DisconnectDatePersian > '1395/01/01' AND EndJobStateId <> 1



SELECT * FROM tbl_Endjobstate

SELECT * FROM Tbl_TamirRequestSubject

SELECT TOP(10) TR.TamirRequestId , TR.TamirRequestSubjectId , R.RequestId , R.RequestNumber , Sub.TamirRequestSubject
  FROM TblTamirRequest TR
  INNER JOIN TblTamirRequestConfirm C ON TR.TamirRequestId = C.TamirRequestId
  INNER JOIN TblRequest R ON C.RequestId = R.RequestId
  INNER JOIN Tbl_TamirRequestSubject Sub ON TR.TamirRequestSubjectId = Sub.TamirRequestSubjectId


SELECT * FROM TblTamirRequestConfirm

  
SELECT RequestId,DisconnectDatePersian,DisconnectTime,DisconnectInterval FROM TblRequest

EXEC spPrepareNotiMessage 9900000000001448

EXEC spPrepareNotiMessage 99100000062937



IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 84)
INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (84,N'NotifBaBarname',NULL,N' مشترک گرامي برق شما Probably به علت Subject، جهت بهبود نيرو رساني در تاريخ DisconnectDatePersian از ساعت DisconnectTime بمدت تقريبي ss دقيقه قطع خواهد شد. توزيع برق ')
GO

UPDATE Tbl_Config SET ConfigText = N' مشترک گرامي برق شما به علت Subject، جهت بهبود نيرو رساني Probably در تاريخ DisconnectDatePersian از ساعت DisconnectTime بمدت تقريبي ss دقيقه قطع خواهد شد. توزيع برق'
  WHERE ConfigId = 84


SELECT * FROM Tbl_Config WHERE ConfigId = 163


SELECT * FROM Tbl_Config WHERE ConfigName = 'IsHavadesAccess'

SELECT dbo.GetTamirOperationsByRequestId(9900000000001448) AS Reason

SELECT dbo.GetTamirSubjectByRequestId(9900000000001448) AS Reason
