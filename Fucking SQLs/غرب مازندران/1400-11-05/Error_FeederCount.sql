
select * from Tbl_Area 

select AreaId,* from Tbl_MPPost where MPPostId = 102

Select * from Tbl_MPFeeder where MPPostId = 102

Select * from Tbl_MPFeeder where MPFeederId = 991423112

select AreaId,InstallDatePersian,IsActive,MPPostId,* from Tbl_MPFeeder where MPFeederId = 991423112

select * from TblMPRequest where MPRequestId = 991423112

----
 SELECT Tbl_Area.Server121Id, COUNT(Tbl_MPFeeder.MPFeederId) AS Cnt 
 FROM Tbl_MPFeeder INNER JOIN Tbl_Area ON Tbl_MPFeeder.AreaId = Tbl_Area.AreaId  
 GROUP BY Tbl_Area.Server121Id 
 
SELECT COUNT(Tbl_MPFeeder.MPFeederId) AS Cnt 
 FROM Tbl_MPFeeder INNER JOIN Tbl_Area ON Tbl_MPFeeder.AreaId = Tbl_Area.AreaId  
 

select top 100 * from TblSubscribers order by 2 desc --.FeederCount


