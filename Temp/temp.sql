USE CCRequesterSetad
GO

SELECT *
FROM INFORMATION_SCHEMA.COLUMNS
  WHERE 
    TABLE_NAME like '%history%' AND
    COLUMN_NAME LIKE '%eco%'

SELECT *
FROM INFORMATION_SCHEMA.TABLES
  WHERE TABLE_NAME LIKE '%access%' AND TABLE_TYPE = 'BASE TABLE'

--------------------------------------------------------------------------------------------------
SELECT * FROM Emergency.Tbl_MPFeederLimitDay

SELECT * FROM Emergency.Tbl_MPFeederLimitType 

SELECT * FROM Emergency.TblTiming 

SELECT * FROM Emergency.TblTimingMPFeeder

SELECT * FROM Emergency.Tbl_GroupMPFeeder 

SELECT * FROM Emergency.Tbl_MPFeederTemplate WHERE GroupMPFeederId = 990188852

SELECT * FROM Tbl_EndJobState 
--------------------------------------------------------------------------------------------------

EXEC Emergency.spGetFeederGroupPlanRaw @aGroupMPFeederId = 990188852

EXEC Emergency.spGetFeederGroupPlan 990188852,990188894
-------------------------------------------------------


EXEC spGetMPPosts_v2 @aAreaId = 2
                    ,@IsAll = 0
                    ,@aWhereClause = ''
                    ,@aWhereClauseOR = ''

EXEC spGetMPFeeders_v2 @aAreaId = 2
                      ,@aMPPostId = 5
                      ,@IsAll = 0

