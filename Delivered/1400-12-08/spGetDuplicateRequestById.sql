CREATE PROCEDURE spGetDuplicateRequestById 
  @aReqId BIGINT
  AS 
  BEGIN
    DECLARE @lIsDuplicate AS BIT
          , @lDuplicateReqId AS BIGINT

  SELECT @lIsDuplicate = IsDuplicatedRequest , @lDuplicateReqId = DuplicatedRequestId
    FROM TblRequest WHERE RequestId = @aReqId
  
  SET @aReqId = CASE WHEN @lIsDuplicate = 1 THEN @lDuplicateReqId ELSE @aReqId END
  
  SELECT R.RequestId , R.RequestNumber, R.Address ,E.EndJobState
    , R.IsDuplicatedRequest AS IsDuplicate
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
  END


