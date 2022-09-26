use CcRequester
Go
Select * From TblError Where ErrorId = 108208115


Select * From _tblBaseTablesLongId

Select * From _tblLPTransLog

SET IDENTITY_INSERT _tblLPTransLog ON
GO
insert into _tblLPTransLog (id) values (43)
GO
SET IDENTITY_INSERT _tblLPTransLog OFF
GO

Select * From TblLPTransLog 
Where LPTransLogId Between 100000000 AND 110000000
Order by LPTransLogId DESC

--100000037


System.Data.SqlClient.SqlException (0x80131904):
 Violation of PRIMARY KEY constraint 'PK_TblLPTransLog'.
  Cannot insert duplicate key in object 'dbo.TblLPTransLog'.
   The duplicate key value is (100000037).  The statement has been terminated.   
at Bargh_UpdateDatabase.frmUpdateDataSetBT.UpdateDataSet(String BT_Name, 
	 DataSet& ds, Object RelatedAreaId, Boolean aIsAutoID, SqlTransaction aTrans,
	  Boolean aIsThrowError, Int32 aDeleteTimeOut, Boolean aIsCheckedArea, Boolean aIsReplicate)    
 at Bargh_BaseTables.frmBS_LPTrans.LogTrans(SqlTransaction aTransaction, Int32 aLPTransId,
  Int32 aLPTransStateId, String aActionDT, String aActionDatePersian, String aActionTime,
   DateTime aDataEntryDT, String aDataEntryDatePersian, String aDataEntryTime, String aComment,
    Int32 aOldAreaId, Int32 aNewAreaId)     
at Bargh_BaseTables.frmBS_LPTrans.SaveInfo()  ClientConnectionId:3003038b-9c13-4eae-a879-5e1aa406ec0f  
Error Number:2627,State:1,Class:14
