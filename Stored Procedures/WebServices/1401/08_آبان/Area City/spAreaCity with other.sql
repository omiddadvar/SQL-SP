CREATE TABLE Tbl_AreaCity (
    CityId int NOT NULL UNIQUE,
    AreaId int NOT NULL
);



CREATE PROCEDURE spAddCityToArea @AreaId int,@CityId int
AS
INSERT INTO [Tbl_AreaCity]
           ([AreaId],[CityId])
     VALUES
           (@AreaId,@CityId)
           
           
           
           

CREATE PROCEDURE spRemoveCityFromArea @AreaId int,@CityId int 
AS   
 DELETE FROM [Tbl_AreaCity]
      WHERE AreaId=@AreaId and CityId=@CityId



Create PROCEDURE spGetAreaCity @AreaIds varchar(max)
AS

select *
into #tempAreaIds
from dbo.Split(@AreaIds,',')


SELECT [Tbl_AreaCity].CityId,[Tbl_AreaCity].AreaId,Tbl_City.City,Tbl_Area.Area
FROM [Tbl_AreaCity]
	LEFT JOIN Tbl_City ON [Tbl_AreaCity].CityId=Tbl_City.CityId
	LEFT JOIN Tbl_Area ON [Tbl_AreaCity].AreaId=Tbl_Area.AreaId
	inner join #tempAreaIds on [Tbl_AreaCity].AreaId=#tempAreaIds.Item
	
drop table #tempAreaIds