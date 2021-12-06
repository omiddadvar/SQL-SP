USE CCRequesterSetad
GO


SELECT *
FROM INFORMATION_SCHEMA.COLUMNS
  WHERE TABLE_NAME = 'ViewTamirRequest'
  AND COLUMN_NAME LIKE '%return%'

SELECT *
FROM INFORMATION_SCHEMA.TABLES
  WHERE TABLE_NAME LIKE '%color%' AND TABLE_TYPE = 'BASE TABLE'
---------------------------------------

SELECT * FROM Tbl_User


SELECT * FROM Tbl_Config WHERE ConfigName LIKE '%color%'

SELECT * FROM Tbl_TamirRequestState

SELECT * FROM Tbl_WarmLineConfirmState

exec spGetReport_8_24 '1400/03/01','1400/09/14','','2','-1','-1','1','-1'

exec spGetReport_8_24 '1400/09/14','','14:00','','-1','-1','1','-1'


SELECT * FROM Tbl_MPFeederLoad MPL 
  INNER JOIN Tbl_MPFeederLoadHours MPH ON MPL.MPFeederLoadId = MPH.MPFeederLoadId
  WHERE RelDatePersian = '1400/09/14'


SELECT * FROM ViewTamirRequest