
SELECT TOP(10) IsTamir,IsFogheToziRequest,IsMPRequest ,* FROM TblRequest ORDER BY 1 DESC

SELECT * FROM TblFogheToziDisconnect WHERE FogheToziId = 1 --Kambood

SELECT * FROM TblMPRequest WHERE DisconnectReasonId IN (1235 , 1215) --Kambood

SELECT * FROM Homa.TblJob WHERE JobStatusId = 7
-------------------------------------------------------------------------------------------------------------------------

SELECT * FROM Tbl_ServiceLogType 

--homa
--homa@123
-------------------------------------------------------------------------------------------------------------------------

SELECT TOP(100) * FROM Homa._tblRegister 

SELECT TOP(100) * FROM Homa.TblRegister WHERE MobileNo = '093612244201'
  
SELECT TOP(100) * FROM Homa.TblRegisterBillingID WHERE BillingID = '1234567890111'

EXEC Homa.spGetSubscriberBilling @CallerId = '0216565655', @BillingIDTypeId = 1

EXEC Homa.spDelSubscriberBilling @CallerId = '0216565655', @BillingID = '1234567890111'

----------------------------------------------------------------------------------------

SELECT DISTINCT "Tbl_Area"."Area"
	,"Tbl_MPPost"."MPPostName"
	,"Tbl_MPFeeder"."MPFeederName"
	,"Tbl_LPPost"."LPPostName"
	,"TblLPPostLoad"."LoadDateTimePersian"
	,"TblLPPostLoad"."PostPeakCurrent"
	,"TblLPPostLoad"."RCurrent"
	,"TblLPPostLoad"."SCurrent"
	,"TblLPPostLoad"."TCurrent"
	,"TblLPPostLoad"."NolCurrent"
	,"TblLPPostLoad"."LPPostId"
	,"TblLPPostLoad"."PostCapacity"
	,"TblLPPostLoad"."LoadDT"
	,"Tbl_Altitude"."DecreaseFactor"
	,"Tbl_Altitude_Area"."DecreaseFactor"
	,"Tbl_Temperature"."K"
	,"Tbl_Temperature_Area"."K"
	,"Tbl_LPPost"."LPPostCode"
	,"Tbl_LPPost"."LocalCode"
	,"TblLPPostLoad"."IsTakFaze"
	,"Tbl_LPPost"."LPPostId"
	,"TblLPPostLoad"."EarthValue"
	,"TblLPPostLoad"."LoadTime"
	,"Tbl_LPPostType"."LPPostType"
	,"Tbl_LPPost"."Address"
FROM (
	(
		(
			(
				(
					(
						(
							(
								"CcRequesterSetad"."dbo"."TblLPPostLoad" "TblLPPostLoad" INNER JOIN "CcRequesterSetad"."dbo"."Tbl_LPPost" "Tbl_LPPost" ON "TblLPPostLoad"."LPPostId" = "Tbl_LPPost"."LPPostId"
								) INNER JOIN "CcRequesterSetad"."dbo"."Tbl_Area" "Tbl_Area" ON "Tbl_LPPost"."AreaId" = "Tbl_Area"."AreaId"
							) INNER JOIN "CcRequesterSetad"."dbo"."Tbl_LPPostType" "Tbl_LPPostType" ON "Tbl_LPPost"."LPPostTypeId" = "Tbl_LPPostType"."LPPostTypeId"
						) INNER JOIN "CcRequesterSetad"."dbo"."Tbl_MPFeeder" "Tbl_MPFeeder" ON "Tbl_LPPost"."MPFeederId" = "Tbl_MPFeeder"."MPFeederId"
					) LEFT OUTER JOIN "CcRequesterSetad"."dbo"."Tbl_Altitude" "Tbl_Altitude" ON "Tbl_LPPost"."AltitudeId" = "Tbl_Altitude"."AltitudeId"
				) LEFT OUTER JOIN "CcRequesterSetad"."dbo"."Tbl_Temperature" "Tbl_Temperature" ON "Tbl_LPPost"."TemperatureId" = "Tbl_Temperature"."TemperatureId"
			) LEFT OUTER JOIN "CcRequesterSetad"."dbo"."Tbl_Altitude" "Tbl_Altitude_Area" ON "Tbl_Area"."AltitudeId" = "Tbl_Altitude_Area"."AltitudeId"
		) LEFT OUTER JOIN "CcRequesterSetad"."dbo"."Tbl_Temperature" "Tbl_Temperature_Area" ON "Tbl_Area"."TemperatureId" = "Tbl_Temperature_Area"."TemperatureId"
	)
INNER JOIN "CcRequesterSetad"."dbo"."Tbl_MPPost" "Tbl_MPPost" ON "Tbl_MPFeeder"."MPPostId" = "Tbl_MPPost"."MPPostId"
WHERE (
		"TblLPPostLoad"."LoadDateTimePersian" >= '1392/01/01'
		AND "TblLPPostLoad"."LoadDateTimePersian" <= '1392/02/21'
		)
ORDER BY "Tbl_Area"."Area"
	,"Tbl_MPPost"."MPPostName"
	,"Tbl_MPFeeder"."MPFeederName"
	,"Tbl_LPPost"."LPPostName"
	,"TblLPPostLoad"."LoadDT" DESC


