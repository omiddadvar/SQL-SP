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

exec spGetReport_8_24 '1400/03/01','1400/09/14','','2','-1','-1','1','-1'

exec spGetReport_8_24 '1400/09/14','','14:00','','-1','-1','1','-1'



SELECT * FROM Tbl_MPFeederLoad MPL 
  INNER JOIN Tbl_MPFeederLoadHours MPH ON MPL.MPFeederLoadId = MPH.MPFeederLoadId
  WHERE RelDatePersian = '1400/09/14'


SELECT * FROM TblTamirRequest 

SELECT 
		Tbl_MPPost.MPPostId,
		Tbl_MPFeeder.MPFeederId, 
		Tbl_MPFeeder.AreaId,
		Tbl_Area.Area,
		Tbl_MPFeederLoad.MPFeederLoadId, 
		Tbl_MPPost.MPPostOwnershipId,
		Tbl_MPFeederLoadHours.MPFeederLoadHourId,
		Tbl_MPFeeder.MPFeederName,
		Tbl_MPPost.MPPostName,
		Tbl_MPFeederLoad.RelDatePersian AS LoadDatePersian, 
		Tbl_Hour.HourId, 
		Tbl_Hour.[Hour],
		Tbl_MPFeederLoadHours.HourExact, 
		Tbl_MPPost.IsActive,
		Tbl_MPFeederLoadHours.CurrentValue, 
		Tbl_MPFeederLoadHours.CurrentValueReActive, 
		Tbl_MPFeederLoadHours.PowerValue
		FROM Tbl_MPFeeder
		INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId 
		LEFT OUTER JOIN Tbl_MPFeederLoad ON Tbl_MPFeeder.MPFeederId = Tbl_MPFeederLoad.MPFeederId
		INNER JOIN Tbl_MPFeederLoadHours ON Tbl_MPFeederLoad.MPFeederLoadId = Tbl_MPFeederLoadHours.MPFeederLoadId
		LEFT JOIN Tbl_Hour ON Tbl_MPFeederLoadHours.HourId = Tbl_Hour.HourId
		LEFT JOIN Tbl_Area ON Tbl_MPFeeder.AreaId = Tbl_Area.AreaId
  WHERE  Tbl_MPPost.IsActive = 1 
  AND Tbl_MPFeederLoad.RelDatePersian = '1400/09/14' 
  AND Tbl_MPFeederLoadHours.HourExact = '14:00'
  ORDER BY Tbl_MPFeederLoad.RelDate DESC


SELECT * FROM ViewTamirRequest