USE CCRequesterSetad
GO

SELECT *
FROM INFORMATION_SCHEMA.COLUMNS
  WHERE 
    TABLE_NAME = 'TblMPRequest' AND
    COLUMN_NAME LIKE 'Is%'

SELECT *
FROM INFORMATION_SCHEMA.TABLES
  WHERE TABLE_NAME LIKE '%access%' AND TABLE_TYPE = 'BASE TABLE'

--------------------------------------------------------------------------------------------------

SELECT * FROM Emergency.TblTiming 

SELECT * FROM Emergency.TblTimingMPFeeder 

SELECT * FROM Emergency.Tbl_GroupMPFeeder 

SELECT * FROM Emergency.Tbl_MPFeederTemplate 


SELECT * FROM Tbl_EndJobState 
--------------------------------------------------------------------------------------------------

EXEC Emergency.SpGetFeederGroupPlan @lGroupMPFeederId = 990188852
