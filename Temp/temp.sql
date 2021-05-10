USE CcRequesterSetad
GO
IF EXISTS (SELECT * From dbo.sysobjects WHERE id = object_id(N'[dbo].[spName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spOmid]
GO
--Report_ErjaList_OperationAvrageInterval
CREATE PROCEDURE [dbo].[spOmid] 
	@aFromDate as varchar(10),
	@aToDate as varchar(10),
	@aMinTimes as int
AS
BEGIN
	SELECT G.DiscONnectGroupId ,G.DiscONnectGroup , AVG(TblLPRequest.DisconnectInterval) AS Average From TblLPRequest
		INNER JOIN TblRequest ON TblRequest.LPRequestId = TblLPRequest.LPRequestId
		INNER JOIN Tbl_DiscONnectGroupSet S ON S.DiscONnectGroupSetId = TblLPRequest.DiscONnectGroupSetId
		INNER JOIN Tbl_DiscONnectGroup G ON G.DiscONnectGroupId = S.DiscONnectGroupId
		WHERE TblLPRequest.DiscONnectDatePersian >= @aFromDate AND
				TblLPRequest.DisconnectDatePersian <= @aToDate
		GROUP BY G.DiscONnectGroupId , G.DiscONnectGroup 
		HAVING COUNT(*) >= @aMinTimes
	UNION
	SELECT G.DiscONnectGroupId ,G.DiscONnectGroup , AVG(TblMPRequest.DisconnectInterval) AS Average From TblMPRequest
		INNER JOIN TblRequest ON TblRequest.MPRequestId = TblMPRequest.MPRequestId
		INNER JOIN Tbl_DiscONnectGroupSet S ON S.DiscONnectGroupSetId = TblMPRequest.DiscONnectGroupSetId
		INNER JOIN Tbl_DiscONnectGroup G ON G.DiscONnectGroupId = S.DiscONnectGroupId
		WHERE TblMPRequest.DiscONnectDatePersian >= @aFromDate AND
				TblMPRequest.DisconnectDatePersian <= @aToDate
		GROUP BY G.DiscONnectGroupId , G.DiscONnectGroup 
		HAVING COUNT(*) >= @aMinTimes
END
GO




--------------------------------------------------------------------------------------------------------------
USE CcRequesterSetad
GO
DECLARE @aFromDate as varchar(10)
DECLARE @aToDate as varchar(10)
DECLARE @aMinTimes as int
Set @aFromDate = '1398/12/01'
Set @aToDate = '1400/02/01'
Set @aMinTimes = 1

SELECT G.DiscONnectGroupId ,G.DiscONnectGroup , AVG(TblLPRequest.DisconnectInterval) AS Average From TblLPRequest
		INNER JOIN TblRequest ON TblRequest.LPRequestId = TblLPRequest.LPRequestId
		INNER JOIN Tbl_DiscONnectGroupSet S ON S.DiscONnectGroupSetId = TblLPRequest.DiscONnectGroupSetId
		INNER JOIN  Tbl_DiscONnectGroup G ON G.DiscONnectGroupId = S.DiscONnectGroupId
		WHERE TblLPRequest.DiscONnectDatePersian >= @aFromDate AND
				TblLPRequest.DisconnectDatePersian <= @aToDate
		GROUP BY G.DiscONnectGroupId , G.DiscONnectGroup 
		HAVING COUNT(*) >= @aMinTimes
	UNION
	SELECT G.DiscONnectGroupId ,G.DiscONnectGroup , AVG(TblMPRequest.DisconnectInterval) AS Average From TblMPRequest
		INNER JOIN TblRequest ON TblRequest.MPRequestId = TblMPRequest.MPRequestId
		INNER JOIN Tbl_DiscONnectGroupSet S ON S.DiscONnectGroupSetId = TblMPRequest.DiscONnectGroupSetId
		INNER JOIN  Tbl_DiscONnectGroup G ON G.DiscONnectGroupId = S.DiscONnectGroupId
		WHERE TblMPRequest.DiscONnectDatePersian >= @aFromDate AND
				TblMPRequest.DisconnectDatePersian <= @aToDate
		GROUP BY G.DiscONnectGroupId , G.DiscONnectGroup 
		HAVING COUNT(*) >= @aMinTimes
	ORDER BY DisconnectGroupId ASC
	
	
select * from Tbl_NetworkType

select NetworkTypeId , count(*) from TblErjaRequest
inner  join TblRequest on TblRequest.RequestId = TblErjaRequest.RequestId
where IsLightRequest = 1
group by NetworkTypeId






-----------------------------------
SELECT TblRequest.AreaId,Tbl_ReferTo.ReferTo,Tbl_Area.Area, AVG(case when TblRequest.IsLightRequest = 1 then TblErjaRequest.ErjaInterval end) AS LightAvg , AVG(case when TblErjaRequest.NetworkTypeId IN (1,2) AND TblErjaRequest.IsLightRequest = 0 then TblErjaRequest.ErjaInterval end) AS MPIntervalAvg , AVG(case when TblErjaRequest.NetworkTypeId = 1 AND TblErjaRequest.IsLightRequest = 0 then TblErjaRequest.ErjaInterval end) AS MPAirAvg , AVG(case when TblErjaRequest.NetworkTypeId = 2 AND TblErjaRequest.IsLightRequest = 0 then TblErjaRequest.ErjaInterval end) AS MPLandAvg , AVG(case when TblErjaRequest.NetworkTypeId IN (3,4) AND TblErjaRequest.IsLightRequest = 0 then TblErjaRequest.ErjaInterval end) AS LPIntervalAvg, AVG(case when TblErjaRequest.NetworkTypeId = 3 AND TblErjaRequest.IsLightRequest = 0 then TblErjaRequest.ErjaInterval end) AS LPAirAvg, AVG(case when TblErjaRequest.NetworkTypeId = 4 AND TblErjaRequest.IsLightRequest = 0 then TblErjaRequest.ErjaInterval end) AS LPLandAvg, AVG(case when TblErjaRequest.NetworkTypeId = 5 AND TblErjaRequest.IsLightRequest = 0 then TblErjaRequest.ErjaInterval end) AS FTIntervalAvg, AVG(case when TblErjaRequest.NetworkTypeId = 6 AND TblErjaRequest.IsLightRequest = 0 then TblErjaRequest.ErjaInterval end) AS MoshtarekIntervalAvg FROM TblErjaRequest INNER JOIN TblRequest ON TblErjaRequest.RequestId = TblRequest.RequestId INNER JOIN Tbl_Area ON Tbl_Area.AreaId = TblRequest.AreaId INNER JOIN Tbl_ReferTo ON TblErjaRequest.ReferToId=Tbl_ReferTo.ReferToId WHERE TblErjaRequest.ErjaStateId = 4 GROUP BY TblRequest.AreaId,Tbl_ReferTo.ReferTo,Tbl_Area.Area ORDER BY TblRequest.AreaId,Tbl_ReferTo.ReferTo