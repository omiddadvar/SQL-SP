USE CcRequester
SELECT *
FROM INFORMATION_SCHEMA.COLUMNS
  WHERE TABLE_NAME = 'TblRequest'
  AND COLUMN_NAME LIKE '%end%'

SELECT TOP 10 * FROM TblRequest

SELECT * FROM Tbl_EndJobState ORDER BY EndJobStateId
-------------------------------------------------------

SELECT * FROM Tbl_MPCloserType