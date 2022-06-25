Select Sh.LPPostCode ,Sh.areaId , Sh.LPPostId , sh.LPPostName, Sh.IsActive
	  ,Rest.AreaId ,A.Area, Rest.LPPostId , Rest.LPPostName , Rest.IsActive
From Tbl_LPPost Sh
Inner Join Tbl_LPPost Rest on Sh.LPPostCode = Rest.LPPostCode
Inner Join Tbl_Area A on Rest.AreaId = A.AreaId 
Where Sh.AreaId = 2 AND Rest.AreaId <> 2
Order by Sh.IsActive DESC


