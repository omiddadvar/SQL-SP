USE CCRequesterSetad
GO


SELECT *
FROM INFORMATION_SCHEMA.COLUMNS
  WHERE TABLE_NAME = 'TblTamirRequest'
  AND COLUMN_NAME LIKE '%subj%'

SELECT *
FROM INFORMATION_SCHEMA.TABLES
  WHERE TABLE_NAME LIKE '%subject%' AND TABLE_TYPE = 'BASE TABLE'

  SELECT DISTINCT
   o.name AS Object_Name,
   o.type_desc
FROM sys.sql_modules m
   INNER JOIN
   sys.objects o
            ON m.object_id = o.object_id
WHERE m.definition Like '%CCRequesterSetad%'

-------------------------------------------------------------



DECLARE @lReqId AS BIGINT = 9900000000001388 -- From Argument :D
  , @lIsDuplicate AS BIT
  , @lDuplicateReqId AS BIGINT

SELECT @lIsDuplicate = IsDuplicatedRequest , @lDuplicateReqId = DuplicatedRequestId
  FROM TblRequest WHERE RequestId = @lReqId

SET @lReqId = CASE WHEN @lIsDuplicate = 1 THEN @lDuplicateReqId ELSE @lReqId END

SELECT R.RequestId, R.IsDuplicatedRequest , R.DuplicatedRequestId , R.DuplicateAreaUserId
  , U.FirstName, U.LastName , R.DataEntryDTPersian , R.DataEntryTime 
  FROM TblRequest R
  INNER JOIN Tbl_AreaUser U ON R.AreaUserId = U.AreaUserId
  WHERE R.RequestId = @lReqId
    OR (R.DuplicatedRequestId = @lReqId AND R.IsDuplicatedRequest = 1)
  ORDER BY R.IsDuplicatedRequest , R.DisconnectDT DESC


--REQ  9900000000001493           DUP   9900000000001388