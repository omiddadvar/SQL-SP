
USE CcRequesterSetad
GO
DROP VIEW dbo.View_FeederPost
GO
CREATE VIEW dbo.View_FeederPost
AS
SELECT f.LPFeederCode , f.LPFeederName
	, p.LPPostName
	, fl.LoadDateTimePersian , fl.LoadTime 
	, fl.RCurrent AS FeederRCurrent , fl.SCurrent AS FeederSCurrent
	, fl.TCurrent AS FeederTCurrent, fl.NolCurrent AS FeederNolcurrent
	, ISNULL(fl.EndlineVoltage,0) AS FeederEndlineVoltage
	, pl.RCurrent AS PostRCurrent, pl.SCurrent AS PostSCurrent
	, pl.TCurrent AS PostTCurrent, pl.NolCurrent AS PostNolCurrent
	, ISNULL(pl.vRN,0) AS PostvRN , ISNULL(pl.vSN,0) AS PostvSN, ISNULL(pl.vTN,0) AS PostvTN
	, ISNULL(pl.vRS,0) AS PostvRS, ISNULL(pl.vTR,0) AS PostvTR, ISNULL(pl.vTS,0) AS PostvTS
	, rf.Fuse AS RFuse, sf.Fuse AS SFuse , tf.Fuse AS TFuse
	FROM  Tbl_LPFeeder f
	INNER JOIN Tbl_LPPost p ON p.LPPostId = f.LPPostId  
	LEFT JOIN TblLPFeederLoad fl ON f.LPFeederId = fl.LPFeederId
	LEFT JOIN TblLPPostLoad pl ON p.LPPostId = pl.LPPostId
	INNER JOIN Tbl_Fuse rf ON rf.FuseId = fl.RFuseId
	INNER JOIN Tbl_Fuse sf ON sf.FuseId = fl.SFuseId
	INNER JOIN Tbl_Fuse tf ON tf.FuseId = fl.TFuseId
go


update View_FeederPost set FeederRCurrent = 200
	where LoadDateTimePersian = '1387/04/18' and LoadTime = '09:48'
	and LPFeederCode = '118182'
	
	
Select * from View_FeederPost
	WHERE LPFeederCode IN ('118182')
	

select TblLPFeederLoad.* from TblLPFeederLoad 
	inner join Tbl_LPFeeder on Tbl_LPFeeder.LPFeederId = TblLPFeederLoad.LPFeederId
	where LPFeederCode = '118182'
	and LoadDateTimePersian = '1387/04/18' and LoadTime = '09:48'
	
	fl.LPPostLoadId
select * from Tbl_TableName
	