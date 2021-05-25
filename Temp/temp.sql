select * from Tbl_TamirType

select top 2 * from TblRequest

SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS
  	WHERE TABLE_NAME = 'TblRequest'
  	AND COLUMN_NAME LIKE '%Tamir%'