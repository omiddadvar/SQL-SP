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



DECLARE @aReqId AS BIGINT = 9900000000001388 -- From Argument :D
  , @lIsDuplicate AS BIT
  , @lDuplicateReqId AS BIGINT

SELECT @lIsDuplicate = IsDuplicatedRequest , @lDuplicateReqId = DuplicatedRequestId
  FROM TblRequest WHERE RequestId = @aReqId

SET @aReqId = CASE WHEN @lIsDuplicate = 1 THEN @lDuplicateReqId ELSE @aReqId END

SELECT R.RequestId , R.RequestNumber, R.Address
  ,CASE WHEN R.IsDuplicatedRequest = 1 AND R.EndJobStateId = 4 THEN 'تکراري' ELSE E.EndJobState END AS EndJobState
  , R.IsDuplicatedRequest AS IsDublicate
  ,U.FirstName +' '+ U.LastName AS Operator
  , ISNULL(UR.FirstName, '' ) +' '+ ISNULL(UR.LastName, '' )AS DuplicateOperator
  , R.DisconnectDatePersian +' '+ R.DisconnectTime AS DisconnectDT
  , R.ConnectDatePersian +' '+ R.ConnectTime AS ConnectDT
  , R.DataEntryDTPersian +' '+ R.DataEntryTime AS DataEntryDT
  FROM TblRequest R
  INNER JOIN Tbl_AreaUser U ON R.AreaUserId = U.AreaUserId
  LEFT JOIN Tbl_AreaUser UR ON R.DuplicatedRequestId = UR.AreaUserId
  INNER JOIN Tbl_EndJobState E ON R.EndJobStateId = E.EndJobStateId
  WHERE R.RequestId = @aReqId
    OR (R.DuplicatedRequestId = @aReqId AND R.IsDuplicatedRequest = 1)
  ORDER BY R.IsDuplicatedRequest , R.DisconnectDT


--REQ  9900000000001493           DUP   9900000000001388

SELECT * FROM TblRequest WHERE  RequestNumber = 400992762