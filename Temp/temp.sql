SELECT *
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Tbl_MPPost'
AND COLUMN_NAME LIKE '%PostCode%'
ORDER BY COLUMN_NAME
--ORDER BY ORDINAL_POSITION

SELECT TOP(100) tr.Address,tr.IsMPRequest, tm.MPPostId ,tm.MPFeederId ,tr.IsLPRequest ,tl.LPPostId,tl.LPFeederId
  FROM TblRequest tr
  INNER JOIN TblMPRequest tm ON tr.MPRequestId = tm.MPRequestId
  INNER JOIN TblLPRequest tl ON tr.LPRequestId = tl.LPRequestId
   

SELECT trbi.* 
  FROM Homa.TblRegisterBillingID trbi
  INNER JOIN Homa.TblRegister tr ON tr.RegisterId = trbi.RegisterId


SELECT TOP(10) tl.LPFeederCode FROM Tbl_LPFeeder tl WHERE tl.LPFeederCode IS NOT NULL
SELECT TOP(10) tl.LPPostCode FROM Tbl_LPPost tl WHERE tl.LPPostCode IS NOT NULL
SELECT TOP(10) tm.MPFeederCode FROM Tbl_MPFeeder tm WHERE tm.MPFeederCode IS NOT NULL 
SELECT TOP(10) tm.MPPostCode FROM Tbl_MPPost tm WHERE tm.MPPostCode IS NOT NULL 

SELECT P.MPPostCode, F.* FROM Tbl_MPFeeder F
  INNER JOIN Tbl_MPPost P ON F.MPPostId = P.MPPostId

SELECT TOP(10) * FROM TblRequest tr

SELECT *
FROM INFORMATION_SCHEMA.Tables
WHERE TABLE_NAME like '%bill%'

  
