
Select AreaUserId,IsMPRequest , IsLightRequest, IsLPRequest ,* 
From TblRequest 
Where RequestNumber = 40199269795 

Select * From TblLPRequest Where LPRequestId = 9900000000437873

Select * From TblMPRequest Where MPRequestId = 9900000000437873

Select * From Tbl_EventLogCenter 
Where TableName = 'TblRequest' and PrimaryKeyId = 9900000000437873
Order By DataEntryDT



----------------------------------------------------------------------------------


UPDATE TblRequest SET IsLightRequest = 0 WHERE RequestId = 9900000000437873

INSERT INTO Tbl_EventLogCenter
  (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand, AreaUserId)
  VALUES ('TblRequest', 115, 9900000000437873, 3, 7, 99, GETDATE(), NULL, NULL);

