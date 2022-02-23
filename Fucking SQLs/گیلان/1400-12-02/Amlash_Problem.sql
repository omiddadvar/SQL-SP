------------------Setad-------------


  select * from Tbl_area order by Server121Id

select * from Tbl_AreaInfo where AreaId in(20,5)

select * from TblRequest where RequestId = 500000000296978

select * from TblSubscriberSMSSend where RequestId = 500000000296978

---------------Amlash---------------

select * from TblSubscriberSMSSend where RequestId = 500000000296978


 Delete from TblSubscriberSMSSend where RequestId = 500000000298341




--------------------121 ÔÑÞ-----------------
DELETE  TblSubscriberSMSSend
WHERE RequestId IN (
	SELECT 
		tS.RequestId
	FROM 
		TblSubscriberSMSSend tS
		INNER JOIN TblRequest tR ON tS.RequestId = tR.RequestId
	WHERE 
		tR.AreaId IN (SELECT AreaId FROM Tbl_Area WHERE Server121Id <> 3)
)


select * from TblSubscriberSMSSend where RequestId Not In 
(
	Select RequestId from TblRequest where DisconnectDatePersian >= '1397/01/01'
)