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
    
select * from TblSpec where SpecTypeId in (107,108)
select * from TblRecloserFunction


select a.Area ,p.MPPostName ,f.MPFeederName , t.MPCloserType , k.*   from Tbl_MPFeederKey k
inner join Tbl_MPFeeder f on f.MPFeederId = k.MPFeederId
inner join Tbl_MPPost p on p.MPPostId = f.MPPostId
inner join Tbl_Area a on a.AreaId = p.AreaId
inner join Tbl_MPCloserType t on t.MPCloserTypeId = k.MPCloserTypeId