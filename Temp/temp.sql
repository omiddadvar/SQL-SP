
SELECT *
FROM INFORMATION_SCHEMA.COLUMNS c
  WHERE TABLE_NAME = 'TblUserOfflineStatus'
  AND COLUMN_NAME LIKE '%feeder%'


SELECT *, LEN(MediaData) FROM [TblMediaPart] ORDER BY 1 DESC

--TRUNCATE TABLE TblMediaPart

SELECT TOP 1 * FROM TblMedia ORDER BY 1 DESC
SELECT TOP 1 * FROM TblMediaHistory  ORDER BY 1 DESC


EXEC spGetChannelOfflineCount @UserId = 1

EXEC spGetUserOfflineCount @UserId = 5

EXEC spGetUsers @UserId = 1

EXEC spGetUserChannel @aUserId = 1



SELECT U.UserId , U.Username , U.UserNumber , U.DisplayName , U.IsActive FROM Tbl_User U
  INNER JOIN Tbl_UserChannel UC ON UC.UserId = U.UserId 
  WHERE UC.ChannelId = 1 
  ORDER BY U.username 


EXEC spGetUserOfflineCount @UserId = 6


SELECT * FROM TblUserOfflineStatus ORDER BY MediaId DESC

SELECT * FROM TblChannelOfflineStatus 

EXEC spCheckListened '1186,1213,1221,1290,1291,1446,1536,1537,1539,1540,1541,1542,1552,1588,1589,1590,1591,1592,1593,1594,1595,1596,1597' ,5, 1 , 0


SELECT * FROM TblMediaHistory WHERE MediaId = 1586

SELECT * FROM TblMedia WHERE MediaId = 1591

SELECT * FROM TblUserOfflineStatus WHERE MediaId = 1597

EXEC spVoiceHeard @aMediaId = 0
                 ,@aUserId = 0
                 ,@aIsChannel = 0
                 ,@aSourceUserId = 0

EXEC spVoiceHeard 1593 , 1, 0 ,5

SELECT * FROM Tbl_Channel

EXEC spGetChannelMessages @aOffset = 0
                         ,@aUserId = 1
                         ,@aChannelId = 5

EXEC spGetUserOfflineCount @UserId = 1
EXEC spGetChannelOfflineCount @UserId = 1


EXEC spGetUserMessages 0 ,5, 1