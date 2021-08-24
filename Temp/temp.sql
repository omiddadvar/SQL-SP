
SELECT ORDINAL_POSITION ,COLUMN_NAME , DATA_TYPE
FROM INFORMATION_SCHEMA.COLUMNS 
  WHERE TABLE_NAME = 'TblRequestInfo'
  AND COLUMN_NAME LIKE '%user%'

--SELECT * FROM Tbl_EndJobState ORDER BY EndJobStateId

DECLARE @rrr AS VARCHAR(MAX) = ''
DECLARE @ReqNo AS BIGINT

DECLARE req_cursor CURSOR FOR 
  SELECT RequestNumber FROM TblRequest R 
    INNER JOIN TblRequestInfo I ON R.RequestId = I.RequestId
    WHERE NOT IsRealNotRelated IS NULL OR NOT OperatorConvsType IS NULL OR NOT IsSaveParts IS NULL OR NOT IsInformTamir IS NULL 
      OR NOT IsSaveLight IS NULL OR NOT SubscriberOKType IS NULL OR NOT IsRealDuplicated IS NULL

OPEN req_cursor;
FETCH NEXT FROM req_cursor INTO @ReqNo;
WHILE @@FETCH_STATUS = 0
BEGIN  
  SET @rrr = CASE WHEN @ReqNo IS NULL THEN '' ELSE  @rrr + ',' + CAST(@ReqNo AS VARCHAR(15)) END
  FETCH NEXT FROM req_cursor INTO @ReqNo;
END  
CLOSE req_cursor;  
DEALLOCATE req_cursor;

SELECT SUBSTRING(@rrr , 2 , LEN(@rrr))

EXEC spGetReport_9_18_ByOperator '1397/01/01','1400/05/25',-1, '',0

EXEC spGetReport_9_18_ByOperator '1398/05/25','1400/05/25',-1,'8,10000065,10000112',0