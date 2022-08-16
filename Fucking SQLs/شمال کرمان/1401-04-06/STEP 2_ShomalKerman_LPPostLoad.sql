
DECLARE @ActiveId AS INT,
        @InActiveId AS INT
DECLARE db_cursor CURSOR FOR 
SELECT
  Active.LPPostId ActiveId,
  InActive.LPPostId InActiveId
FROM Tbl_LPPost Active
INNER JOIN Tbl_LPPost InActive ON Active.LPPostCode = InActive.LPPostCode
WHERE 
  Active.IsActive = 1 
  AND ISNULL(InActive.IsActive, 0) = 0
  AND ISNULL(Active.LPPostCode , '') <> ''

OPEN db_cursor  
FETCH NEXT FROM db_cursor INTO @ActiveId , @InActiveId  

WHILE @@FETCH_STATUS = 0  
BEGIN  

    EXEC spChangeLPPost @aFromLPPostId = @InActiveId
                       ,@aToLPPostId = @ActiveId
                       ,@IsDelete = 0
                       ,@IsMergeLPFeeder = 0
                       ,@IsMergeLPTrans = 0
                       ,@IsReplicated = 1

      FETCH NEXT FROM db_cursor INTO @ActiveId , @InActiveId  
END 

CLOSE db_cursor  
DEALLOCATE db_cursor 
