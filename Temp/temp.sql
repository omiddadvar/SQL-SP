SELECT *
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Tbl_LPFeeder'
AND COLUMN_NAME LIKE '%Code%'
ORDER BY COLUMN_NAME
--ORDER BY ORDINAL_POSITION

SELECT TOP(100) tr.Address,tr.IsMPRequest, tm.MPPostId ,tm.MPFeederId ,tr.IsLPRequest ,tl.LPPostId,tl.LPFeederId
  FROM TblRequest tr
  INNER JOIN TblMPRequest tm ON tr.MPRequestId = tm.MPRequestId
  INNER JOIN TblLPRequest tl ON tr.LPRequestId = tl.LPRequestId
   

SELECT tr.MobileNo ,trbi.* 
  FROM Homa.TblRegisterBillingID trbi
  INNER JOIN Homa.TblRegister tr ON tr.RegisterId = trbi.RegisterId
  WHERE trbi.LPPostId = 20054878


--SELECT TOP(10) tl.LPFeederCode FROM Tbl_LPFeeder tl WHERE tl.LPFeederCode IS NOT NULL
--SELECT TOP(10) tl.LPPostCode FROM Tbl_LPPost tl WHERE tl.LPPostCode IS NOT NULL
--SELECT TOP(10) tm.MPFeederCode FROM Tbl_MPFeeder tm WHERE tm.MPFeederCode IS NOT NULL 
--SELECT TOP(10) tm.MPPostCode FROM Tbl_MPPost tm WHERE tm.MPPostCode IS NOT NULL 
--
--SELECT P.MPPostCode, F.* FROM Tbl_MPFeeder F
--  INNER JOIN Tbl_MPPost P ON F.MPPostId = P.MPPostId

--SELECT TOP(10) * FROM TblRequest tr

SELECT *
FROM INFORMATION_SCHEMA.Tables
WHERE TABLE_NAME like '%bill%'

SELECT TOP(10) tr.RequestNumber,tr.IsMPRequest, tm.MPPostId ,mpp.MPPostCode ,tm.MPFeederId , mpf.MPFeederCode
  ,tl.LPPostId, lpp.LPPostCode ,tl.LPFeederId , lpf.LPFeederCode
  FROM TblRequest tr
  INNER JOIN TblMPRequest tm ON tr.MPRequestId = tm.MPRequestId
  INNER JOIN TblLPRequest tl ON tr.LPRequestId = tl.LPRequestId
  INNER JOIN Tbl_LPFeeder lpf ON lpf.LPFeederId = tl.LPFeederId
  INNER JOIN Tbl_LPPost lpp ON lpp.LPPostId = tl.LPPostId
  INNER JOIN Tbl_MPFeeder mpf ON mpf.MPFeederId = tm.MPFeederId
  INNER JOIN Tbl_MPPost mpp ON mpp.MPPostId = tm.MPPostId
  

SELECT tm.MPFeederName,tm.MPFeederCode FROM Tbl_MPFeeder tm

SELECT tl.LPPostCode ,tl.LPPostName , tl.LPPostId FROM Tbl_LPPost tl 
  WHERE tl.LPPostId = 20054878
  --WHERE tl.LPPostCode = '112233'

  EXEC spGetOutageInfoFromNetwork @aMPPostCode = '', 
    @aMPFeederCode = '', 
    @aLPPostCode = '12-0211hg', 
    @aLPFeederCode = ''
-- @aMPFeederCode = '20524', 