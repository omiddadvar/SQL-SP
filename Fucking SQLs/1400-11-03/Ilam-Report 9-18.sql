
select I.QnsAreaUserId , U.UserName, COUNT(*) from TblRequestInfo I
	inner join TblRequest R on I.RequestId = R.RequestId
	left join Tbl_AreaUser U on I.QnsAreaUserId = U.AreaUserId
	where I.QnsAreaUserId is not null
	  and R.DisconnectDatePersian > '1400/10/01'
	Group By I.QnsAreaUserId, U.UserName
	  
	  
select top 100 * from TblRequestInfo 

--118116