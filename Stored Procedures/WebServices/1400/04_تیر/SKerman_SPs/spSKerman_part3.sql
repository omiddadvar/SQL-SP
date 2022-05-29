USE CcRequesterSetad
GO
CREATE PROCEDURE spSKerman_part3
  AS 
  BEGIN
      select Tbl_Area.areaId, Area, ISNULL( t1.cntMPPublic,0) + ISNULL(t2.cntLPPublic,0) as CntPublic,
      ISNULL(t1.cntMPPublic,0) as CntPublicMP, ISNULL(t3.cntLP,0) as CntLP
      from Tbl_Area
      left join (
      select AreaId, COUNT(*) as cntMPPublic from Tbl_MPFeeder where IsActive = 1 and isnull(ownershipid,2) = 2 
      group by AreaId ) as t1
      on Tbl_Area.AreaId = t1.AreaId
      left join (
      select AreaId, COUNT(*) as cntLPPublic from Tbl_LPFeeder where IsActive = 1 and isnull(ownershipid,2) = 2
      group by AreaId ) as t2
      on Tbl_Area.AreaId = t2.AreaId
      left join (
		select AreaId, COUNT(*) as cntLP from Tbl_LPFeeder where IsActive = 1
		group by AreaId) as t3
      on Tbl_Area.AreaId = t3.AreaId
      where IsCenter = 0
  END

--EXEC spsKerman_part3