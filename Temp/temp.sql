
SELECT TOP(10) IsTamir,IsFogheToziRequest,IsMPRequest ,* FROM TblRequest ORDER BY 1 DESC

SELECT * FROM TblFogheToziDisconnect WHERE FogheToziId = 1 --Kambood

SELECT * FROM TblMPRequest WHERE DisconnectReasonId IN (1235 , 1215) --Kambood

SELECT * FROM Homa.TblJob WHERE JobStatusId = 7

/*----------Base ------------------*/
SELECT * FROM Tbl_DisconnectReason  WHERE DisconnectReasonId IN (1235 , 1215)

SELECT * FROM Tbl_FogheTozi WHERE FogheToziId = 1
 
SELECT * FROM Homa.Tbl_JobStatus WHERE JobStatusId = 7
/*------------------------------*/


DECLARE @lcdcdcdcd AS INT = 10
UPDATE TblRequest SET EndJobStateId = CASE 
   	WHEN R.IsTamir = 1 AND (
          (R.IsFogheToziRequest = 1 AND FTD.FogheToziId = 1)
          OR (R.IsMPRequest = 1 AND DisconnectReasonId IN (1235 , 1215))
        ) THEN 7
   	WHEN R.IsTamir = 0 AND (J.JobStatusId = 7) THEN 8
   ELSE EndJobStateId
   END
  FROM TblRequest R 
  LEFT JOIN Homa.TblJob J ON R.RequestId = J.RequestId
  LEFT JOIN TblFogheToziDisconnect FTD ON R.FogheToziDisconnectId = FTD.FogheToziDisconnectId
  LEFT JOIN TblMPRequest MP ON R.MPRequestId = MP.MPRequestId
  WHERE J.RequestId = -1

-----------------------------------------------------------------------------------------------------------------------

EXEC spGetSubscriberOutageInfoByTrackingCode @aTrackingCode = 40099101443 --MP
EXEC spGetSubscriberOutageInfoByTrackingCode @aTrackingCode = 40099101442 --FT
EXEC spGetSubscriberOutageInfoByTrackingCode @aTrackingCode = 40099101441 --Tablet

EXEC Homa.spGetOutageInfoByCallerId @CallerId='09121234567'

EXEC spGetSubscriberOutageInfo @CallerId = '09121234567', @BackTime = 120

-------------------------------------------------------------------------------------------------------------------------


homa
homa@123