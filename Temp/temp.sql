USE CcRequesterDepartment
GO
SELECT *
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'TblErjaRequest' 
--AND ORDINAL_POSITION > 88
AND COLUMN_NAME LIKE '%id%'
--ORDER BY COLUMN_NAME
ORDER BY ORDINAL_POSITION
GO

------------------++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
SELECT SUM (
    		dbo.MinuteCount(
			CONVERT(DATETIME, '2020/04/25 0:0' , 102),
			CONVERT(DATETIME, '2020/04/26 1:0' , 102),
			TblRequest.DisconnectDT,
			TblRequest.ConnectDT
      ) / TblRequest.DisconnectInterval * TblRequest.DisconnectPower 	) AS PartPower  
FROM TblRequest 
WHERE 	(TblRequest.DisconnectInterval > 0) 
  AND TblRequest.ConnectDT IS NOT NULL
  AND TblRequest.DisconnectDatePersian >= '1399/02/05' 
  AND TblRequest.DisconnectDatePersian <= '1399/02/06' 	
  AND (IsMPRequest=1 OR IsLPRequest=1 OR IsFogheToziRequest=1)     
ORDER BY PartPower DESC 



SELECT TOP(100) IsTamir , TamirTypeId FROM TblRequest ORDER BY DisconnectDatePersian DESC

SELECT * FROM Tbl_TamirType ttt
