
SELECT * FROM Emergency.TblTiming WHERE GroupMPFeederId = 990188916

SELECT * FROM Emergency.TblTimingMPFeeder  WHERE TimingId = 990188926

SELECT * FROM Emergency.Tbl_TimingState 


SELECT * FROM Emergency.Tbl_GroupMPFeeder


SELECT * FROM Tbl_MPFeeder WHERE MPFeederId = 3

EXEC  Emergency.spGetMPFeederTiming 990188926



--UPDATE Emergency.TblTimingMPFeeder SET ConnectDT = GETDATE() , 
--      ConnectDatePersian = '1401/02/20' , ConnectTime = '17:30'
--    WHERE TimingMPFeederId IN (990188939)
--
--
--990188940





/*
SELECT * FROM Tbl_ServiceLogApplication 
SELECT * FROM Tbl_ServiceLogKeyType 
SELECT * FROM Tbl_ServiceLogType
SELECT * FROM Tbl_ServiceLogLevel

SELECT * FROM TblServiceLog 


SELECT * FROM ViewServiceLog 
*/