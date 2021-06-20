ALTER PROCEDURE spOmid(
  @aAreaIDs AS VARCHAR(100),
  @aDatePersian AS VARCHAR(10),
  @aDate AS VARCHAR(10)
  ) AS
  BEGIN
    DECLARE @lsql AS VARCHAR(500) = '';
    DECLARE @lHour1 AS VARCHAR(5);
    DECLARE @lHour2 AS VARCHAR(5);
    DECLARE @lDT1 AS DATETIME;
    DECLARE @lDT2 AS DATETIME;
    DECLARE @i AS INT = 0;
    CREATE TABLE #tmpData (Area NVARCHAR(100), HourFrom VARCHAR(5) ,HourTo VARCHAR(5), 
      cntNotTamir FLOAT, cntTamir1 FLOAT,cntTamir2 FLOAT, cntTamir3 FLOAT)
    CREATE TABLE #tmpArea (AreaId INT , Area NVARCHAR(50))
    	SET @lsql = ' SELECT AreaId, Area FROM tbl_Area '
    IF @aAreaIDs <> ''
    	SET  @lsql += ' WHERE AreaId IN (' + @aAreaIDs + ')'
   INSERT #tmpArea EXEC (@lsql)
   BEGIN TRY
      WHILE(@i < 24)
      BEGIN
        SET @lHour1 = CONVERT(VARCHAR(5), @i) + ':00' 
        SET @lHour2 = CONVERT(VARCHAR(5), @i + 1) + ':00'
        SET @lDT1 = CONVERT(DATETIME, @aDate +' '+ @lHour1)
        SET @lDT2 = CONVERT(DATETIME, @aDate +' '+ @lHour2 , 102)

        SELECT @i,@aDate ,@lHour1,@lDt1,@lHour2 ,@lDT2,@aAreaIDs

--        INSERT #tmpData SELECT a.Area, @lHour1, @lHour2,
--           SUM(CASE WHEN t.IsTamir = 0 THEN dbo.MinuteCount(@lDT1,@lDT2,t.ConnectDT, t.DisconnectDT) ELSE 0 END) AS sumNotTamir,
--           SUM(CASE WHEN t.IsTamir = 1 AND t.TamirTypeId = 1 THEN dbo.MinuteCount(@lDT1,@lDT2,t.ConnectDT, t.DisconnectDT) ELSE 0 END) AS sumTamir1,
--           SUM(CASE WHEN t.IsTamir = 1 AND t.TamirTypeId = 2 THEN dbo.MinuteCount(@lDT1,@lDT2,t.ConnectDT, t.DisconnectDT) ELSE 0 END) AS sumTamir2,
--           SUM(CASE WHEN t.IsTamir = 1 AND t.TamirTypeId = 3 THEN dbo.MinuteCount(@lDT1,@lDT2,t.ConnectDT, t.DisconnectDT) ELSE 0 END) AS sumTamir3
--          FROM TblRequest t
--          INNER JOIN #tmpArea a ON a.AreaId = t.AreaId
--          WHERE t.DisconnectInterval > 0 
--                AND t.ConnectDT IS NOT NULL
--                AND t.DisconnectDatePersian = @aDatePersian
--          GROUP BY a.Area
        SET @i = @i + 1
      END
--      SELECT * FROM #tmpData
      DROP TABLE #tmpData
      DROP TABLE #tmpArea
    END TRY  
    BEGIN CATCH  
      DROP TABLE #tmpData
      DROP TABLE #tmpArea
      SELECT   
          ERROR_NUMBER() AS ErrorNumber  
          ,ERROR_LINE() AS Line
          ,ERROR_MESSAGE() AS ErrorMessage;  
    END CATCH;
END
GO

EXECUTE spOmid '2,3,5,6,8,9,4','1400/02/13' , '2021/05/03'



  SELECT TOP(10) DisconnectDT ,DisconnectDatePersian FROM TblRequest  WHERE DisconnectInterval > 0  AND ConnectDT IS NOT NULL
    ORDER BY DisconnectDatePersian DESC

SELECT AreaId, Area FROM tbl_Area