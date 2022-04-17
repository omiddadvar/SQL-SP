select top 10 TamirOperationListId from TblTamirOperationList
where 
	TamirOperationListId < 9900000000000000
order by TamirOperationListId desc

Select max(id) from _tblTamirRequestDetail

/*
SET IDENTITY_INSERT _tblTamirRequestDetail ON
GO
INSERT INTO _tblTamirRequestDetail (Id) VALUES (1013) 
GO
SET IDENTITY_INSERT _tblTamirRequestDetail OFF
GO
*/

select top 10 * from TblTamirOperationList order by TamirOperationListId DESC

---------------------------------------------------------------------------------------------

  select count(*) from Tbl_EventLogCenter where WorkingAreaId = 10 and TableName IN (
		'TblTamirRequest'
		,'TblTamirRequestDisconnect'
		)

select * from TblDepartmentInfo