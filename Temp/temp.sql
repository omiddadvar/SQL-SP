use CcRequesterSetad
go
select * from Tbl_MPCloserType

select * from Tbl_MPFeederKey


select * from Tbl_RecloserAction

select * from Tbl_Area
select * from Tbl_MPPost
select * from Tbl_MPFeeder

SELECT TABLE_NAME As Name , *
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_CATALOG='CcRequesterSetad' 
	AND TABLE_TYPE = 'BASE TABLE' 
    AND TABLE_NAME like '%close%'
    
    --(for MySql, use: TABLE_SCHEMA='dbName' )