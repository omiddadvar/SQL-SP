
SELECT *
FROM INFORMATION_SCHEMA.COLUMNS c
  WHERE TABLE_NAME = 'Tbl_User'
  AND COLUMN_NAME LIKE '%feeder%'


EXEC spGetUserMessages 0 ,28, 1,' AND H.MediaDateTime BETWEEN ''1400/01/01 00:00:00'' AND ''1400/07/21 23:59:00'' AND H.SourceUserId IN (1,5,6,28) AND M.IsOnlineVoice = 1','LEFT'

SELECT * FROM Tbl_UserChannel

SELECT * FROM Tbl_UserAccess

SELECT * FROM Tbl_Access

EXEC spGetUserChannel 1

EXEC spGetChannelUsers 3

EXEC spGetUsers 5

EXEC spGetDisplayName

SELECT * FROM Tbl_User
  
SELECT * FROM Tbl_Channel

SELECT * FROM Tbl_UserAccess
