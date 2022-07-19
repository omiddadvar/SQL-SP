
Select RequestId,  
DataEntryDTPersian , DataEntryDTPersian , DataEntryTime,
DisconnectDatePersian , DisconnectDT , DisconnectTime,
ConnectDatePersian , ConnectDT , ConnectTime
From TblRequest Where RequestNumber =  40104261 


-- '2022-05-18 11:11:00.000'
-- '1401/02/28'


----------------------------------------------

Update TblRequest 
SET DataEntryDTPersian = '1401/02/28' , DataEntryDT = '2022-05-18 11:11:00.000',
	DisconnectDatePersian = '1401/02/28' , DisconnectDT = '2022-05-18 11:11:00.000'
Where RequestNumber =  40104261 



Insert Into Tbl_EventLogCenter (TableName , TableNameId , PrimaryKeyId , Operation , AreaId , WorkingAreaId)
Values ('TblRequest' ,115 ,400000000000388 , 3 , 4 , 99)


