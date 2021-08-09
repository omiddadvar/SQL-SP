
SELECT COLUMN_NAME
FROM INFORMATION_SCHEMA.COLUMNS 
  WHERE TABLE_NAME = 'ViewMonitoringEx'
  AND COLUMN_NAME LIKE '%post%'



SELECT * FROM Tbl_Config WHERE ConfigId BETWEEN 700 AND 710 --ConfigName LIKE '%Homa%'

INSERT INTO Tbl_Config (ConfigId, ConfigName, ConfigValue, ConfigText)
  VALUES (709, 'HomaMonitoringISShowLPPostCode', N'True', NULL); 

DELETE FROM Tbl_Config WHERE ConfigId = 202

UPDATE Tbl_Config SET ConfigValue = 'False' WHERE ConfigId = 709

