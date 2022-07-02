Select T.TamirRequestNo , T.CancelDatePersian , T.CancelTime
	,U.UserName , U.FirstName , U.LastName,
	AreaU.Area , AreaT.Area
From TblTamirRequest T
Inner Join Tbl_AreaUser U On T.CancelAreaUserId = U.AreaUserId
Inner Join Tbl_Area AreaU On U.AreaId = AreaU.AreaId
Inner Join Tbl_Area AreaT On T.AreaId = AreaT.AreaId
Where TamirRequestNo IN (
401077529,
401077530,
401087376,
401087336,
401087337,
401087338
)
