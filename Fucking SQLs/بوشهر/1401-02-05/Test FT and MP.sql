--    Select top 2 * from Tbl_EventLogCenter  Order By DataEntryDT DESC

Select RequestNumber,IsFogheToziRequest,FogheToziDisconnectId,* from TblRequest where RequestNumber in (
40099175518,
40099175519
)
Select * from TblFogheToziDisconnect where FogheToziDisconnectId in (
990699026
)


/*40099176707 -> 40099175519*/
Select RequestNumber,FogheToziDisconnectId,IsMPRequest,* from TblRequest where RequestNumber in (
40099176707,
40099176708,
40099176709
)
select * from TblTamirRequestConfirm where RequestId in (9900000000236178,9900000000237237)

Select * from Tbl_EventLogCenter where 
	(TableName = 'TblMPRequest' AND PrimaryKeyId = 9900000000237237) OR
	(TableName = 'TblFogheToziDisconnect' AND PrimaryKeyId = 990699026) OR
	(TableName = 'TblTamirRequestConfirm' AND PrimaryKeyId IN(9900000000003553,1800000000003317))
	Order By DataEntryDT DESC

-----------------------------------------------------------------------
Select RequestNumber,RequestId,DisconnectPower,DisconnectPowerECO,IsFogheToziRequest,OverlapTime,
	DataEntryDT,ConnectDT,ConnectDatePersian,ConnectTime,DisconnectDatePersian,DisconnectTime
	,* from TblRequest where RequestNumber in (

40099175519,
40099176707
)

Select * from TblDCOverlap where TamirRequestId in (
9900000000003553,
1800000000003317
)
Select * from Tbl_AreaUser where AreaUserId in (
990078293,
990173474
)


Select * from TblChangeRequestHistory where RequestId in (
9900000000236178
)
order by ChangeDT



Select * from TblTamirRequestConfirm where RequestId in ( 
40099175267,
40099175519
)