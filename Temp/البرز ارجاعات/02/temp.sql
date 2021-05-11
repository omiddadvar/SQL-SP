USE CcRequesterSetad
GO
	
	
select * from Tbl_NetworkType

select NetworkTypeId , count(*) from TblErjaRequest
inner  join TblRequest on TblRequest.RequestId = TblErjaRequest.RequestId
where IsLightRequest = 1
group by NetworkTypeId