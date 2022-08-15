--CREATE PROCEDURE Homa.spFindCurrentAndFutureOutageByBillingID
--    @aCityId AS INT,
--    @aBillingId AS VARCHAR(1000),
--    @aFromDatePersian AS VARCHAR(10),
--    @aToDatePersian AS VARCHAR(10)
--  AS
--  BEGIN
--  	
--  END

DECLARE
    @aFromDatePersian AS VARCHAR(10) = '1401/04/01',
    @aToDatePersian AS VARCHAR(10) = '1401/05/30',
    @aCityId AS INT = -1,
    @aBillingId AS VARCHAR(20) = ''


SELECT R.RequestId, 
  R.RequestNumber,
  R.AreaId,
  R.IsTamir,
  R.DisconnectDatePersian AS DisconnectDate,
  R.DisconnectTime AS DisconnectTime,
  R.ConnectDatePersian AS ConnectDate,
  R.ConnectTime,
  R.DataEntryDTPersian AS DataEntryDate,
  R.Address,
  AU.UserName,
  Au.FirstName + ' ' + Au.LastName AS Name,
--  CASE WHEN R.IsLightRequest = 1 THEN CRL.CallReasonLight ELSE CR.CallReason END Reason
  CR.CallReason AS Reason
INTO  #tmpCurrent
FROM TblRequest R
  INNER JOIN Tbl_AreaUser AU ON R.AreaUserId = AU.AreaUserId
  LEFT JOIN Tbl_CallReason CR ON R.CallReasonId = CR.CallReasonId
--  LEFT JOIN Tbl_CallReasonLight CRL ON R. = CRL.CallReasonLightId
WHERE IsTamir = 0 AND 
      DisconnectDatePersian BETWEEN @aFromDatePersian AND @aToDatePersian
      AND R.EndJobStateId = 5
      AND (R.LPRequestId IS NOT NULL OR R.MPRequestId IS NOT NULL)

SELECT R.RequestId, 
  R.RequestNumber,
  R.AreaId,
  R.IsTamir,
  R.TamirDisconnectFromDatePersian AS DisconnectDate,
  R.TamirDisconnectFromTime AS DisconnectTime,
  R.TamirDisconnectToDatePersian AS ConnectDate,
  R.TamirDisconnectToTime AS ConnectTime,
  R.DataEntryDTPersian AS DataEntryDate,
  Address,
  AU.UserName,
  Au.FirstName + ' ' + Au.LastName AS Name,
  CR.CallReason AS Reason
INTO  #tmpFuture
FROM TblRequest R
  INNER JOIN Tbl_AreaUser AU ON R.AreaUserId = AU.AreaUserId
  LEFT JOIN Tbl_CallReason CR ON R.CallReasonId = CR.CallReasonId
WHERE IsTamir = 1 AND 
      DisconnectDatePersian BETWEEN @aFromDatePersian AND @aToDatePersian
      AND R.EndJobStateId = 4

SELECT TEMP.* INTO #tmpReq FROM (
    SELECT * FROM #tmpCurrent 
      UNION ALL
    SELECT * FROM #tmpFuture
) TEMP

DROP Table #tmpCurrent
DROP Table #tmpFuture

SELECT T.* ,A.CityId
FROM #tmpReq T
  LEFT JOIN Tbl_Area A ON T.AreaId = A.AreaId
  LEFT JOIN TblRequestInfo I ON T.RequestId = I.RequestId
WHERE (@aCityId <= 0 OR A.CityId = @aCityId) 
  AND (LEN(@aBillingId) = 0 OR I.BillingID = @aBillingId)
ORDER BY DisconnectDate DESC , DisconnectTime DESC


DROP Table #tmpReq



SELECT * FROM TblRequest 
WHERE rea
  --IsLightRequest = 1 AND IsTamir = 1

SELECT * FROM Tbl_CallReason 
SELECT * FROM Tbl_CallReasonLight 
