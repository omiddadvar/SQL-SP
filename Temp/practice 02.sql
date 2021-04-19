--select top(100) * from Tbl_LPFeeder where FeederCode IS NOT Null and LEN(FeederCode) > 0
use CcRequesterSetad
go
declare @code nvarchar(100)
set @code = '118182'
CREATE VIEW dbo.View_FeederPost
AS
SELECT fl.* , f.* FROM  Tbl_LPFeeder f
	INNER JOIN Tbl_LPPost p ON p.LPPostId = f.LPPostId  
	LEFT JOIN TblLPFeederLoad fl ON f.LPFeederId = fl.LPFeederId
	LEFT JOIN TblLPPostLoad pl ON p.LPPostId = pl.LPPostId
	INNER JOIN Tbl_Fuse rf ON rf.FuseId = fl.RFuseId
	INNER JOIN Tbl_Fuse sf ON sf.FuseId = fl.SFuseId
	INNER JOIN Tbl_Fuse tf ON tf.FuseId = fl.TFuseId
	WHERE f.LPFeederCode = @code
go

--select top(2) * from Tbl_LPFeeder where LPFeederCode is not null
--select top(2) * from Tbl_LPPost

--select top(2) * from TblLPPostLoad
--select top(2) * from TblLPFeederLoad

--select top(2) * from Tbl_Fuse

