-- > Create StoredProcedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spGMaz_Report_14_1]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spName]
GO

CREATE PROCEDURE [dbo].[spGMaz_Report_14_1] 
	@aFrom varchar(12),
	@aTo varchar(12)
AS
BEGIN
	SELECT A.Area, RequestNumber, 
		TrackingCode, DisconnectDatePersian, DisconnectTime, 	
		ExternalService, BillingID, Telephone, Mobile, 	
		R.ConnectDatePersian, R.ConnectTime, 	
		dbo.GetRequestEndJobState(trackingcode) AS EndJobState, 	
		R.Comments, R.SubscriberName , M.MediaType
		FROM TblRequestData RD	
		INNER JOIN TblRequest R ON RD.RequestId = R.RequestId 	
		INNER JOIN Tbl_Area A ON A.AreaId = R.AreaId
		INNER JOIN Tbl_ExternalService EX ON RD.ExternalServiceId = EX.ExternalServiceId 	
		INNER JOIN TblRequestInfo RI ON RI.RequestId = R.RequestId
		INNER JOIN Tbl_EndJobState EJ ON R.EndJobStateId = EJ.EndJobStateId 
		INNER JOIN Tbl_MediaType M ON M.MediaTypeId = RD.MediaTypeId 
		WHERE  EX.ExternalServiceTypeId = 1
			AND R.DisconnectDatePersian >= @aFrom 
			AND R.DisconnectDatePersian <= @aTo 
		ORDER BY A.AreaId
END
GO

--Test :
--EXEC spGMaz_Report_14_1 '1399/02/11' , '1400/02/11';
