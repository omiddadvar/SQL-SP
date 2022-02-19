select * from TblDepartmentInfo

select * from Tbl_Area where AreaId = 14
select * from Tbl_AreaInfo where AreaId = 14


select * from TblRequestInfo where RequestId=9900000000115831

select DataEntryDT,DisconnectDT,DisconnectDatePersian,* from TblRequest where RequestId=9900000000115831

update TblRequest set DisconnectDatePersian = '1400/11/28' , DisconnectDT = '2022/02/17 12:23:00.000',
 DataEntryDTPersian = '1400/11/28' , DataEntryDT = '2022/02/17 12:23:05.000'
 where RequestId = 9900000000115831


select * from TblError where ErrorId = 15717148

