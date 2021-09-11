
SELECT *
FROM INFORMATION_SCHEMA.COLUMNS c
  WHERE TABLE_NAME = 'TblMedia'
  AND COLUMN_NAME LIKE '%feeder%'


SELECT *
FROM INFORMATION_SCHEMA.TABLES

SELECT * FROM Tbl_User


EXEC spGetDisplayName

SELECT * FROM TblMediaHistory 

SELECT * FROM Tbl_UserChannel WHERE UserId = 1

SELECT * FROM Tbl_Channel

SELECT * FROM TblMedia

SELECT TOP 20 U.UserName ,H.* --, M.IsOnlineVoice , M.Content
  FROM TblMediaHistory H 
  INNER JOIN TblMedia M ON H.MediaId = M.MediaId
  INNER JOIN Tbl_User U ON H.SourceUserId = U.UserId 
  WHERE H.DestChannelId = 5
  ORDER BY H.MediaId DESC

SELECT * FROM TblMedia M
  INNER JOIN TblMediaHistory H ON M.MediaId = H.MediaId
  WHERE M.MediaId = 921