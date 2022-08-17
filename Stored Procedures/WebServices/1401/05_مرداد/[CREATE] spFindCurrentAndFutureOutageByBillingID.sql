
CREATE PROCEDURE spFindCurrentAndFutureOutageByBillingID
    @aFromDate AS VARCHAR(10),
    @aToDate AS VARCHAR(10),
    @aAreaId AS INT = -1,
    @aBillingId AS VARCHAR(1000) = ''
  AS
  BEGIN

    SELECT R.RequestId, 
      R.RequestNumber,
      R.AreaId,
      R.IsTamir,
      R.DisconnectDatePersian,
      R.DisconnectTime AS DisconnectTime,
      R.ConnectDatePersian AS ConnectDate,
      R.ConnectTime,
      R.DataEntryDTPersian,
      R.Address,
      AU.UserName,
      CASE WHEN R.IsTamir = 1 THEN ReasonFor.DisconnectLPRequestFor ELSE CR.CallReason END DisconnectGroupSet
    INTO  #tmpCurrent
    FROM TblRequest R
      INNER JOIN Tbl_AreaUser AU ON R.AreaUserId = AU.AreaUserId
      LEFT JOIN Tbl_CallReason CR ON R.CallReasonId = CR.CallReasonId
      LEFT JOIN Tbl_DisconnectLPRequestFor ReasonFor ON R.DisconnectRequestForId = ReasonFor.DisconnectLPRequestForId
    WHERE IsTamir = 0 AND 
          DisconnectDatePersian BETWEEN @aFromDate AND @aToDate
          AND R.EndJobStateId = 5
          AND (R.LPRequestId IS NOT NULL OR R.MPRequestId IS NOT NULL)
    
    SELECT R.RequestId, 
      R.RequestNumber,
      R.AreaId,
      R.IsTamir,
      R.TamirDisconnectFromDatePersian AS DisconnectDatePersian,
      R.TamirDisconnectFromTime AS DisconnectTime,
      R.TamirDisconnectToDatePersian AS ConnectDate,
      R.TamirDisconnectToTime AS ConnectTime,
      R.DataEntryDTPersian,
      Address,
      AU.UserName,
      Sub.TamirRequestSubject AS DisconnectGroupSet
    INTO  #tmpFuture
    FROM TblRequest R
      INNER JOIN Tbl_AreaUser AU ON R.AreaUserId = AU.AreaUserId
      INNER JOIN TblTamirRequestConfirm C ON R.RequestId = C.RequestId
      INNER JOIN TblTamirRequest TR ON C.TamirRequestId = TR.TamirRequestId
      LEFT JOIN Tbl_TamirRequestSubject Sub ON TR.TamirRequestSubjectId = Sub.TamirRequestSubjectId
    WHERE IsTamir = 1 AND 
          R.TamirDisconnectFromDatePersian BETWEEN @aFromDate AND @aToDate
          AND R.EndJobStateId = 4
    
    SELECT TEMP.* INTO #tmpReq FROM (
        SELECT * FROM #tmpCurrent 
          UNION ALL
        SELECT * FROM #tmpFuture
    ) TEMP
    
    DROP Table #tmpCurrent
    DROP Table #tmpFuture
    
    SELECT T.* ,A.Area
    FROM #tmpReq T
      INNER JOIN Tbl_Area A ON T.AreaId = A.AreaId
      LEFT JOIN TblRequestInfo I ON T.RequestId = I.RequestId
    WHERE (@aAreaId <= 0 OR A.AreaId = @aAreaId)
      AND (LEN(@aBillingId) = 0 OR I.BillingID = @aBillingId)
    ORDER BY T.DisconnectDatePersian DESC , T.DisconnectTime DESC
    
    
    DROP Table #tmpReq

  END

/*

EXEC spFindCurrentAndFutureOutageByBillingID @aFromDate = '1401/01/01',
                                             @aToDate = '1401/05/30',
                                             @aAreaId = -1,
                                             @aBillingId = '1234567890123'

*/


