
SELECT TOP(10) IsTamir,IsFogheToziRequest,IsMPRequest ,* FROM TblRequest ORDER BY 1 DESC

SELECT * FROM TblFogheToziDisconnect WHERE FogheToziId = 1 --Kambood

SELECT * FROM TblMPRequest WHERE DisconnectReasonId IN (1235 , 1215) --Kambood

SELECT * FROM Homa.TblJob WHERE JobStatusId = 7
-------------------------------------------------------------------------------------------------------------------------

SELECT * FROM Tbl_ServiceLogType 

--homa
--homa@123
-------------------------------------------------------------------------------------------------------------------------

SELECT DisconnectGroupSetId,DisconnectGroupSet FROM Tbl_DisconnectGroupSet

EXEC spHomaReportArea @aAreaIDs = '2,3'
                     ,@aStartDate = '1390/01/01'
                     ,@aEndDate = '1401/01/01'
                     ,@aDisGroupSetIDs = '1,2,3,4,5,6,7,8,9,10'
                     ,@aIsWarmline = NULL



SELECT * FROM Homa.TblRequestMP

SELECT * FROM Homa.TblRequestCommon

