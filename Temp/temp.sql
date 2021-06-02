USE CcRequesterSetad
GO
EXEC spReportHourlyDisconnectPower '1400/02/04'


exec spGetReport_8_24 '1390/03/12','1400/03/12','','','','-1','1','-1'

select * from TblManagerSMSDC