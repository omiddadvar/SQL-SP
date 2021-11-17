USE CCRequesterSetad
GO


SELECT *
FROM INFORMATION_SCHEMA.COLUMNS
  WHERE TABLE_NAME = 'Tbl_Media'
  AND COLUMN_NAME LIKE '%Serv%'

SELECT *
FROM INFORMATION_SCHEMA.TABLES
  WHERE TABLE_NAME LIKE '%Disconnect%' AND TABLE_TYPE = 'BASE TABLE'
---------------------------------------

SELECT TOP 10 ServiceId,* FROM BTblService

--mwhere =>  AND ISNULL(BTblService.IsWarmLine,0)  =  1 

------------


SELECT 
                Tbl_Area.AreaId,
                Tbl_Area.Area,
                MIN(StartDate) As StartDate,
                MAX(EndDate) As EndDate,
                cntBazdid, 
                cntService,
                BTbl_ServicePart.ServicePartCode, 
                BTbl_ServicePart.ServicePartName,  
                SUM(BTblServicePartUse.Quantity) As Quantity, 
                BTbl_ServicePart.PriceOne, 
                BTbl_ServicePart.ServicePrice, 
                Tbl_PartUnit.PartUnit 
            FROM  
                BTblBazdidResultAddress
                INNER JOIN BTblBazdidResult On BTblBazdidResultAddress.BazdidResultId = BTblBazdidResult.BazdidResultId
                INNER Join BTblServicePartUse On BTblBazdidResultAddress.BazdidResultAddressId = BTblServicePartUse.BazdidResultAddressId 
                INNER JOIN BTbl_ServicePart On BTblServicePartUse.ServicePartId = BTbl_ServicePart.ServicePartId 
                Left OUTER JOIN Tbl_PartUnit On BTbl_ServicePart.PartUnitId = Tbl_PartUnit.PartUnitId
                INNER JOIN Tbl_MPFeeder On BTblBazdidResult.MPFeederId = Tbl_MPFeeder.MPFeederId 
                INNER Join Tbl_Area On BTblBazdidResult.AreaId = Tbl_Area.AreaId
                Left JOIN BTblBazdidTiming On BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId 
 LEFT JOIN BTblService ON BTblServicePartUse.ServiceId = BTblService.ServiceId 

                INNER JOIN 
                ( 
                    Select DISTINCT 
                        BTblBazdidResultCheckList.BazdidResultAddressId, 
                    	MIN(BTblServiceCheckList.DoneDatePersian) As StartDate, 
                    	MAX(BTblServiceCheckList.DoneDatePersian) As EndDate 
                    FROM
                        BTblBazdidResultCheckList
                        INNER JOIN BTblServiceCheckList On BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId
                    	INNER JOIN BTblService On BTblServiceCheckList.ServiceId = BTblService.ServiceId 
                        INNER JOIN BTblBazdidResultAddress On BTblBazdidResultCheckList.BazdidResultAddressId = BTblBazdidResultAddress.BazdidResultAddressId 
                        INNER Join BTblBazdidResult On BTblBazdidResultAddress.BazdidResultId = BTblBazdidResult.BazdidResultId 
                        Left JOIN Tbl_MPFeeder On BTblBazdidResult.MPFeederId = Tbl_MPFeeder.MPFeederId 
 WHERE  BTblServiceCheckList.DoneDatePersian >= '1399/08/23' AND BTblServiceCheckList.DoneDatePersian <= '1400/08/23'
 
                Group BY
                        BTblBazdidResultCheckList.BazdidResultAddressId
                ) As Tbl_FilterDate On BTblBazdidResultAddress.BazdidResultAddressId = Tbl_FilterDate.BazdidResultAddressId
            	LEFT JOIN 
            	( 
            		Select DISTINCT
                        BTblBazdidResult.AreaId,
            			COUNT(DISTINCT Case When BTblBazdidResult.BazdidStateId In (2,3) Then BTblBazdidResultCheckList.BazdidResultCheckListId End) As cntBazdid, 
            			COUNT(DISTINCT Case When BTblServiceCheckList.ServiceStateId = 3 Then BTblServiceCheckList.BazdidResultCheckListId End) As cntService 
            		FROM 
            			BTblBazdidResult 
            			INNER JOIN BTblBazdidResultAddress On BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId 
            			INNER JOIN BTblBazdidResultCheckList On BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId 
            			LEFT JOIN BTblServiceCheckList On BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId 
            			INNER JOIN Tbl_MPFeeder On BTblBazdidResult.MPFeederId = Tbl_MPFeeder.MPFeederId
                       LEFT JOIN BTblBazdidTiming On BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId 
 LEFT JOIN BTblService ON BTblServiceCheckList.ServiceId = BTblService.ServiceId 

                WHERE 
            	BTblBazdidResult.BazdidTypeId = 1 
 AND BTblServiceCheckList.DoneDatePersian >= '1399/08/23' AND BTblServiceCheckList.DoneDatePersian <= '1400/08/23' AND ISNULL(BTblService.IsWarmLine,0)  =  1 
		GROUP BY 
            			BTblBazdidResult.AreaId 
            	) As Tbl_BazdidService On BTblBazdidResult.AreaId = Tbl_BazdidService.AreaId 
            WHERE  
            	BTblBazdidResult.BazdidStateId In (2,3) 
                And BTblBazdidResult.BazdidTypeId = 1 
            	And Not BTbl_ServicePart.ServicePartId Is NULL 
 AND ISNULL(BTblService.IsWarmLine,0)  =  1 
GROUP BY 
            	Tbl_Area.AreaId, 
            	Tbl_Area.Area,
                cntBazdid,
                cntService,
                BTbl_ServicePart.ServicePartCode,
                BTbl_ServicePart.ServicePartName,
                BTbl_ServicePart.PriceOne,
                BTbl_ServicePart.ServicePrice,
                Tbl_PartUnit.PartUnit 
            ---------------------------------------------------









  SELECT 
                Tbl_Area.AreaId,
                Tbl_Area.Area,
                MIN(StartDate) As StartDate,
                MAX(EndDate) As EndDate,
                cntBazdid, 
                cntService,
                BTbl_ServicePart.ServicePartCode, 
                BTbl_ServicePart.ServicePartName,  
                SUM(BTblServicePartUse.Quantity) As Quantity, 
                BTbl_ServicePart.PriceOne, 
                BTbl_ServicePart.ServicePrice, 
                Tbl_PartUnit.PartUnit 
            FROM  
                BTblBazdidResultAddress
                INNER JOIN BTblBazdidResult On BTblBazdidResultAddress.BazdidResultId = BTblBazdidResult.BazdidResultId
                INNER Join BTblServicePartUse On BTblBazdidResultAddress.BazdidResultAddressId = BTblServicePartUse.BazdidResultAddressId 
                INNER JOIN BTbl_ServicePart On BTblServicePartUse.ServicePartId = BTbl_ServicePart.ServicePartId 
                Left OUTER JOIN Tbl_PartUnit On BTbl_ServicePart.PartUnitId = Tbl_PartUnit.PartUnitId
                INNER JOIN Tbl_MPFeeder On BTblBazdidResult.MPFeederId = Tbl_MPFeeder.MPFeederId 
                INNER Join Tbl_Area On BTblBazdidResult.AreaId = Tbl_Area.AreaId
                Left JOIN BTblBazdidTiming On BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId 
 LEFT JOIN BTblService ON BTblServicePartUse.ServiceId = BTblService.ServiceId 

                INNER JOIN 
                ( 
                    Select DISTINCT 
                        BTblBazdidResultCheckList.BazdidResultAddressId, 
                    	MIN(BTblServiceCheckList.DoneDatePersian) As StartDate, 
                    	MAX(BTblServiceCheckList.DoneDatePersian) As EndDate 
                    FROM
                        BTblBazdidResultCheckList
                        INNER JOIN BTblServiceCheckList On BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId
                    	INNER JOIN BTblService On BTblServiceCheckList.ServiceId = BTblService.ServiceId 
                        INNER JOIN BTblBazdidResultAddress On BTblBazdidResultCheckList.BazdidResultAddressId = BTblBazdidResultAddress.BazdidResultAddressId 
                        INNER Join BTblBazdidResult On BTblBazdidResultAddress.BazdidResultId = BTblBazdidResult.BazdidResultId 
                        Left JOIN Tbl_MPFeeder On BTblBazdidResult.MPFeederId = Tbl_MPFeeder.MPFeederId 
 WHERE  BTblServiceCheckList.DoneDatePersian >= '1399/08/23' AND BTblServiceCheckList.DoneDatePersian <= '1400/08/23'
 
                Group BY
                        BTblBazdidResultCheckList.BazdidResultAddressId
                ) As Tbl_FilterDate On BTblBazdidResultAddress.BazdidResultAddressId = Tbl_FilterDate.BazdidResultAddressId
            	LEFT JOIN 
            	( 
            		Select DISTINCT
                        BTblBazdidResult.AreaId,
            			COUNT(DISTINCT Case When BTblBazdidResult.BazdidStateId In (2,3) Then BTblBazdidResultCheckList.BazdidResultCheckListId End) As cntBazdid, 
            			COUNT(DISTINCT Case When BTblServiceCheckList.ServiceStateId = 3 Then BTblServiceCheckList.BazdidResultCheckListId End) As cntService 
            		FROM 
            			BTblBazdidResult 
            			INNER JOIN BTblBazdidResultAddress On BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId 
            			INNER JOIN BTblBazdidResultCheckList On BTblBazdidResultAddress.BazdidResultAddressId = 
                      BTblBazdidResultCheckList.BazdidResultAddressId 
            			LEFT JOIN BTblServiceCheckList On BTblBazdidResultCheckList.BazdidResultCheckListId = 
                        BTblServiceCheckList.   BazdidResultCheckListId 
            			INNER JOIN Tbl_MPFeeder On BTblBazdidResult.MPFeederId = Tbl_MPFeeder.MPFeederId
                     LEFT JOIN BTblBazdidTiming On BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId 
                  LEFT JOIN BTblService ON BTblServicePartUse.ServiceId = BTblService.ServiceId 

                WHERE 
            	BTblBazdidResult.BazdidTypeId = 1 
 AND BTblServiceCheckList.DoneDatePersian >= '1399/08/23' AND BTblServiceCheckList.DoneDatePersian <= '1400/08/23' AND ISNULL(BTblService.IsWarmLine,0)  =  1 
		GROUP BY 
            			BTblBazdidResult.AreaId 
            	) As Tbl_BazdidService On BTblBazdidResult.AreaId = Tbl_BazdidService.AreaId 
            WHERE  
            	BTblBazdidResult.BazdidStateId In (2,3) 
                And BTblBazdidResult.BazdidTypeId = 1 
            	And Not BTbl_ServicePart.ServicePartId Is NULL 
 AND ISNULL(BTblService.IsWarmLine,0)  =  1 
GROUP BY 
            	Tbl_Area.AreaId, 
            	Tbl_Area.Area,
                cntBazdid,
                cntService,
                BTbl_ServicePart.ServicePartCode,
                BTbl_ServicePart.ServicePartName,
                BTbl_ServicePart.PriceOne,
                BTbl_ServicePart.ServicePrice,
                Tbl_PartUnit.PartUnit 
            