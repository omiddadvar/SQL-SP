
SELECT *
FROM INFORMATION_SCHEMA.COLUMNS c
  WHERE TABLE_NAME = 'TblMedia'
  AND COLUMN_NAME LIKE '%feeder%'


SELECT *, LEN(MediaData) FROM [TblMediaPart] ORDER BY 1 DESC

--TRUNCATE TABLE TblMediaPart

SELECT TOP 1 * FROM TblMedia ORDER BY 1 DESC
SELECT TOP 1 * FROM TblMediaHistory  ORDER BY 1 DESC




EXEC spGetChannelMessages 0 ,1, 1,' AND H.MediaDateTime BETWEEN ''2021/03/21 12:00:00 AM'' AND ''2021/09/28 11:59:00 PM'' AND H.SourceUserId IN (28)',''

