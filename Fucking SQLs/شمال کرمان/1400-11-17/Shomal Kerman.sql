--SELECT * FROM Sheet1$



SELECT LPP.IsActive,LPP.* FROM Tbl_LPPost LPP
  INNER JOIN Sheet1$ S ON LPP.LPPostId = S.LPPostId


UPDATE LPP SET LPP.IsActive = 0 
  FROM Tbl_LPPost LPP
  INNER JOIN Sheet1$ S ON LPP.LPPostId = S.LPPostId

------------------------------------------------------------------

INSERT INTO Tbl_EventLogCenter (
     TableName,
     TableNameId,
     PrimaryKeyId,
     Operation,
     AreaId,
     WorkingAreaId,
     DataEntryDT
     )
SELECT
     'Tbl_LPPost' AS TableName,
     2 AS TableNameId,
     LPP.LPPostId AS PrimaryKeyId,
     3 AS Operation,
     NULL AS AreaId,
     99 AS WorkingAreaId,
     GETDATE() AS DataEntryDT
FROM 
     Tbl_LPPost LPP
   INNER JOIN Sheet1$ S ON LPP.LPPostId = S.LPPostId


