
SELECT IsActive,AreaId,* From Tbl_MPPost where MPPostName like '%„Â—«‰%'-- MPPostId in (960256, 990078317)

SELECT IsActive,* FROM Tbl_MPPost WHERE AreaId IN (4 , 17)

SELECT * FROM Tbl_MPFeeder WHERE MPPostId = 990078317 AreaId = 4

SELECT * FROM Tbl_MPPost WHERE MPPostId IN (960256, 990078317)

SELECT MPPostId,* FROM Tbl_MPFeeder WHERE MPPostId IN (960256, 990078317)

SELECT * FROM Tbl_MPCommonPost WHERE AreaId IN (4 , 17)

SELECT * FROM Tbl_MPCommonFeeder WHERE AreaId IN (4 , 17)

SELECT * FROM Tbl_LPCommonFeeder WHERE AreaId IN (4 , 17)

SELECT * FROM Tbl_Area

EXEC spGetMPPosts_v2 4 , 0 , '' , ''  