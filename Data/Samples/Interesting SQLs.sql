use CcRequesterSetad
go
-------------Find the Columns----------
SELECT *
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'TblRequest'
ORDER BY COLUMN_NAME
--ORDER BY ORDINAL_POSITION

SELECT TABLE_NAME As Name , *
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_CATALOG='CcRequesterSetad' 
	AND TABLE_TYPE = 'BASE TABLE' 
    AND TABLE_NAME like '%close%'

exec sp_help TblRecloserFunction
exec sp_columns TblRecloserFunction

exec sp_databases
exec sp_tables 
exec sp_tables TblRecloserFunction
exec sp_table_privileges TblRecloserFunction
exec sp_statistics TblRequest