
SELECT *
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'TblTamirRequestFile' 
--AND ORDINAL_POSITION > 88
--AND COLUMN_NAME LIKE '%warm%'
--ORDER BY COLUMN_NAME
ORDER BY ORDINAL_POSITION


select top(2) tol.* from TblTamirRequest tr
inner join TblTamirOperationList tol on tol.TamirRequestId = tr.TamirRequestId

SELECT * from Tbl_TamirOperation

SELECT * from Tbl_TamirOperationGroup

select * from Tbl_Area

select * from Tbl_Peymankar

select * from TblFileServer

select * from TblFileServer



select * from Tbl_NetworkType

select * from Tbl_MPCloserType

select * from Tbl_Peymankar

select * from Tbl_Area

select * from Tbl_TamirRequestState

select top (10) * from Tbl_MPPost

select tol.TamirOperationListId ,tr.* from TblTamirRequest tr
inner join TblTamirOperationList tol on tol.TamirRequestId = tr.TamirRequestId
where tr.ConnectDatePersian = '1400/04/02'

--update TblTamirRequest set WarmLineConfirmStateId = 2
update TblTamirRequest set TamirRequestStateId = 0 where TamirRequestId = 9900000000000033


select * from TblTamirRequestFile where TamirRequestId = 9900000000000033