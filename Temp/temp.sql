SELECT *
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'TblTamirRequest'
AND COLUMN_NAME LIKE '%subj%'
ORDER BY COLUMN_NAME
--ORDER BY ORDINAL_POSITION
--TblTamirRequestConfirm
SELECT TamirRequestStateId ,tr.IsReturned,tr.IsWarmLine,* FROM TblTamirRequest tr WHERE TamirRequestNo = 40099714


SELECT * FROM Tbl_TamirRequestState ttrs

UPDATE TblTamirRequest SET TamirRequestStateId = 2 , IsReturned = 0, IsWarmLine = 0 WHERE TamirRequestNo = 40099714

UPDATE TblTamirRequest SET TamirRequestStateId = 0, IsReturned = 1 WHERE TamirRequestNo = 40099714

UPDATE TblTamirRequest SET ReturnTimeoutDT =  NULL WHERE TamirRequestNo = 40099714
--  2021/06/23 12:03:19.000 PM

SELECT * FROM Tbl_TamirRequestSubject



  SELECT * FROM TblSubscriberInfom tsi

SELECT * FROM TblRequestInform tri

SELECT ttrs.* FROM TblSubscriberInfom tsi
  INNER JOIN TblRequestInform tri ON tri.RequestInformId = tsi.RequestInformId
  INNER JOIN TblRequest tr ON tr.RequestId = tri.RequestId
  INNER JOIN TblTamirRequestConfirm ttrc ON ttrc.RequestId = tr.RequestId
  INNER JOIN TblTamirRequest ttr ON ttr.TamirRequestId = ttrc.TamirRequestId
  INNER JOIN Tbl_TamirRequestSubject ttrs ON ttrs.TamirRequestSubjectId = ttr.TamirRequestSubjectId

  UPDATE TblTamirRequest SET TamirRequestSubjectId = 990154512 WHERE TamirRequestNo = 9501716


   EXEC [spCheckNewSMSEvent]