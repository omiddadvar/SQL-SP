--------Multiple Erja------------
Select R.RequestNumber,E.* From TblRequest R
Inner Join TblErjaRequest E On R.RequestId = E.RequestId
Where R.RequestNumber IN (40101222996 , 40101225250 , 40101226048)

--------LPPosts Not Loaded--------------

SELECT
  Active.LPPostId ActiveId,
  InActive.LPPostId InActiveId,
  Active.LPPostCode ActiveCode,
  InActive.LPPostCode InActiveCode
INTO #tmp
FROM Tbl_LPPost Active
INNER JOIN Tbl_LPPost InActive ON Active.LPPostCode = InActive.LPPostCode
WHERE 
  Active.IsActive = 1 
  AND ISNULL(InActive.IsActive, 0) = 0
  AND ISNULL(Active.LPPostCode , '') <> ''

Select * From  #tmp
Where ActiveCode IN ('11489','8968', '10072')
Or InActiveCode IN ('11489','8968', '10072')

Drop Table #tmp


Select * From Tbl_LPPost Where LPPostCode Like '%11489%'

Select * From Tbl_LPPost Where LPPostCode IN ('11489','8968', '10072')



------------------------------------------------------------------------------------------
Select A.Area , U.* from Tbl_AreaUser U
Inner join Tbl_Area A On U.AreaId = A.AreaId
Where U.UserName Like '%shamaeipour%'

--992645610

Select R.RequestNumber,E.AreaUserId ,E.CreateAreaUserId , E.DoneAreaUserId ,E.* 
From TblRequest R
Inner Join TblErjaRequest E On R.RequestId = E.RequestId
Where R.RequestNumber  = 40101222996 
AND (E.AreaUserId = 992645610 Or E.DoneAreaUserId = 992645610)


Select * 
From Tbl_AreaUserReferTo 
Where AreaUserId = 992645610
Order By 1 desc


/*

Where R.RequestNumber IN (40101222996 , 40101225250 , 40101226048)
*/