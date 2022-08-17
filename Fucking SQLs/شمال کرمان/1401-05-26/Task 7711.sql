
Select R.RequestId , R.RequestNumber,
		R.DisconnectDatePersian , R.EndJobStateId , E.EndJobState,
	    MP.DisconnectDatePersian,  MP.EndJobStateId , MPE.EndJobState ,MP.MPRequestId
From TblRequest R
Inner Join TblMPRequest MP On R.MPRequestId = MP.MPRequestId
Inner Join Tbl_EndJobState E On R.EndJobStateId = E.EndJobStateId
Inner Join Tbl_EndJobState MPE On MP.EndJobStateId = MPE.EndJobStateId
Where R.EndJobStateId <> MP.EndJobStateId
AND R.DisconnectDatePersian >= '1400/01/01'
AND NOT (R.EndJobStateId = 2 AND MP.EndJobStateId = 5) 
AND R.EndJobStateId = 5
Order By R.DisconnectDT DESC


Select * From Tbl_EventLogCenter
Where TableName = 'TblRequest' 
AND PrimaryKeyId = 9900000000636832
Order By DataEntryDT DESC