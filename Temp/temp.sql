
SELECT COLUMN_NAME
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'TblTamirRequest' --AND COLUMN_NAME LIKE '%file%'
ORDER BY COLUMN_NAME
--ORDER BY ORDINAL_POSITION


select top(2) tol.* from TblTamirRequest tr
inner join TblTamirOperationList tol on tol.TamirRequestId = tr.TamirRequestId

SELECT * from Tbl_TamirOperation

select * from Tbl_Area

select * from Tbl_Peymankar

select * from TblFileServer

select * from TblTamirRequestFile

select * from Tbl_NetworkType
