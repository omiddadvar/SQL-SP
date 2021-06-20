USE CcRequesterDepartment
GO
SELECT *
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'TblErjaRequest' 
--AND ORDINAL_POSITION > 88
AND COLUMN_NAME LIKE '%id%'
--ORDER BY COLUMN_NAME
ORDER BY ORDINAL_POSITION
GO



SELECT * FROM Tbl_GenericPart
GO
SELECT *
FROM INFORMATION_SCHEMA.Tables
WHERE TABLE_NAME like '%usedpart%' 




SELECT tr.RequestNumber,ter.ErjaDatePersian,tup.PartCount,tup.SerghatCount,tup.BarkenarCount,tgp.PartName
  FROM TblUsedPart tup
  INNER JOIN Tbl_GenericPart tgp ON tgp.GenericPartId = tup.GenericPartId
  INNER JOIN TblErjaRequest ter ON ter.ErjaRequestId = tup.ErjaRequestId
  INNER JOIN TblRequest tr ON tr.RequestId = ter.RequestId
ORDER BY ter.ErjaDatePersian DESC

