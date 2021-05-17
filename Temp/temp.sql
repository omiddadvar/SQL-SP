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


select a.AreaId, a.Area ,p.MPPostId ,p.MPPostName ,f.MPFeederId , f.MPFeederName ,
 --t.MPCloserTypeId , t.MPCloserType ,
  k.MPFeederKeyId , k.KeyName
   from Tbl_MPFeederKey k
inner join Tbl_MPFeeder f on f.MPFeederId = k.MPFeederId
inner join Tbl_MPPost p on p.MPPostId = f.MPPostId
inner join Tbl_Area a on a.AreaId = p.AreaId
--inner join Tbl_MPCloserType t on t.MPCloserTypeId = k.MPCloserTypeId
--where MPFeederKeyId = 99000000000003


select * from TblRecloserFunction 
Tbl_ErjaPart

exec sp_help TblRecloserFunction
go
exec sp_columns TblRecloserFunction
go
SELECT *
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'TblRecloserFunction'
--ORDER BY COLUMN_NAME
ORDER BY ORDINAL_POSITION

select * from ViewLPPostLoad

SELECT rf.*,f.MPFeederName, k.KeyName From TblRecloserFunction rf
INNER JOIN tbl_MPfeeder f ON f.MPFeederId = rf.MPFeederId
INNER JOIN Tbl_MPFeederKey k ON k.MPFeederKeyId = rf.MPFeederKeyId

select top(2) * from Tbl_MPFeederKey

SELECT * FROM TblSpec WHERE SpecTypeId = 108

-----------Report-----------Report-----------Report-----------Report-----------Report-----------Report-----------
SELECT A.Area ,P.MPPostName ,F.MPFeederName ,
	            FK.KeyName As Recloser, FK.Address , RM.SpecValue AS RecloserModel , 
	            RT.SpecValue AS RecloserType, RF.DataEntryDatePersian As ReadDate , RF.ReadTime,
	            RF.RestartCounterCount , RF.TripCounterCount , RF.FaultCounterCount
               FROM TblRecloserFunction RF
            INNER JOIN Tbl_MPFeederKey FK on FK.MPFeederKeyId = RF.MPFeederKeyId
            INNER JOIN Tbl_MPFeeder F on F.MPFeederId = FK.MPFeederId
            INNER JOIN Tbl_MPPost p on P.MPPostId = F.MPPostId
            INNER JOIN Tbl_Area A on a.AreaId = P.AreaId
            INNER JOIN TblSpec RM on RM.SpecId = RF.spcRecloserModelId 
            INNER JOIN TblSpec RT on RT.SpecId = RF.spcRecloserTypeId 
            INNER JOIN Tbl_MPCloserType CT on ct.MPCloserTypeId = FK.MPCloserTypeId 
            WHERE AND RF.ReadDatePersian > '1400/02/12' ORDER BY RF.FaultCounterCount DESC

