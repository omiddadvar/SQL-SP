SELECT
	'Tbl_MPFeeder' AS TableName,
	11 AS TableNameId,
	F.MPFeederId AS PrimaryKeyId,
	3 AS Operation,
	NULL AS AreaId,
	99 AS WorkingAreaId,
GETDATE() AS DataEntryDT
FROM 
	Tbl_MPFeeder F 
	Inner Join ExcelMPFeeder E ON F.MPFeederCode = E.MPFeederCode