USE CcRequesterSetad
GO
EXECUTE spDisHourly_Daily '2' ,'1387/06/10' , '2008/08/31' , 1,0,0
GO
EXECUTE spDisHourly_Daily '2,3,5,6,8,9,4','1387/06/10' , '2008/08/31' , 1 , 1 ,1