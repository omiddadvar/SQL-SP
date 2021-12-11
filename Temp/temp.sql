USE CCRequesterSetad
GO


SELECT *
FROM INFORMATION_SCHEMA.COLUMNS
  WHERE TABLE_NAME = 'TblRequest'
  AND COLUMN_NAME LIKE '%tamir%'

SELECT *
FROM INFORMATION_SCHEMA.TABLES
  WHERE TABLE_NAME LIKE '%color%' AND TABLE_TYPE = 'BASE TABLE'
---------------------------------------
SELECT * FROM TblMPRequest
SELECT * FROM Tbl_Area

EXECUTE spDisHourly_Daily @aAreaIDs = ''
                         ,@aDatePersian = ''
                         ,@aDate = ''
                         ,@aIsLPReq = 0
                         ,@aIsMPReq = 0
                         ,@aIsFoghTReq = 0
                         ,@aMPPostIDs = ''
                         ,@aMPFeederIDs = ''
                         ,@aIsTamir = 0


EXEC spDisHourly_Daily '2','1400/09/17','2021/12/08',True,True,True


EXEC spDisHourly_Daily '2','1400/09/17','2021/12/08',True,True,True,'1','',-1,1
