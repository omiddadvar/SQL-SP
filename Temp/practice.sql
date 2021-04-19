
use CcRequesterSetad
go
ALTER VIEW dbo.View_FeederPost
AS
SELECT f.LPFeederCode , f.LPFeederName
	, p.LPPostName
	, fl.LoadDateTimePersian , fl.LoadTime , fl.RCurrent, fl.SCurrent	
	, fl.TCurrent , fl.NolCurrent , fl.EndlineVoltage
	, pl.RCurrent, pl.SCurrent	, pl.TCurrent , pl.NolCurrent 
	, pl. , pl. 
	FROM  Tbl_LPFeeder f
	INNER JOIN Tbl_LPPost p ON p.LPPostId = f.LPPostId  
	LEFT JOIN TblLPFeederLoad fl ON f.LPFeederId = fl.LPFeederId
	LEFT JOIN TblLPPostLoad pl ON p.LPPostId = pl.LPPostId
	INNER JOIN Tbl_Fuse rf ON rf.FuseId = fl.RFuseId
	INNER JOIN Tbl_Fuse sf ON sf.FuseId = fl.SFuseId
	INNER JOIN Tbl_Fuse tf ON tf.FuseId = fl.TFuseId
	WHERE f.LPFeederCode = '118182'
go

Select * from View_FeederPost
