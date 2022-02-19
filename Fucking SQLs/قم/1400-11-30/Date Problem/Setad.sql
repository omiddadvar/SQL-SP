
select * from TblRequestInfo where RequestId=9900000000115831

select DataEntryDT,DisconnectDT,DisconnectDatePersian,* from TblRequest where RequestId=9900000000115831


select AreaId,DataEntryDT,DataEntryDTPersian,DisconnectDT,DisconnectDatePersian,
TamirRequestDatePersian,
TamirDisconnectFromDatePersian,
TamirDisconnectToDatePersian,
ConnectDatePersian,
LastUpdateAreaUserId,
AreaUserId,RequestNumber from TblRequest 
where DisconnectDatePersian > '1401/01/01'

  
--<Setad & Center>

update TblRequest set DisconnectDatePersian = '1400/11/28' , DisconnectDT = '2022/02/17 12:23:00.000',
 DataEntryDTPersian = '1400/11/28' , DataEntryDT = '2022/02/17 12:23:05.000'
 where RequestId = 9900000000115831 AND AreaId = 14
/*
2022/02/17 12:23:05.000
1400/11/28
*/
---------------------------------------------------------------------

select * from Tbl_AreaInfo where AreaId = 6

--<Setad & Center>

update TblRequest set DisconnectDatePersian = '1400/11/28' , DisconnectDT = '2022/02/17 12:34:00.000',
 DataEntryDTPersian = '1400/11/28' , DataEntryDT = '2022/02/17 12:34:34.000'
 where RequestId = 9900000000115833

----------------------------------------------------------------------


update TblRequest set DisconnectDatePersian = '1400/11/28' , DisconnectDT = '2022/02/17 12:28:00.000',
 DataEntryDTPersian = '1400/11/28' , DataEntryDT = '2022/02/17 12:28:33.000'
 where RequestId = 9900000000115832

