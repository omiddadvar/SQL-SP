USE CcRequesterSetad
Go
CREATE PROCEDURE spDisHourly_Daily(
  @aAreaIDs AS VARCHAR(100),
  @aDatePersian AS VARCHAR(10),
  @aDate AS VARCHAR(10),
  @aIsLPReq AS BIT = 1,
  @aIsMPReq AS BIT = 1,
  @aIsFoghTReq AS BIT = 1
  ) AS
  BEGIN
    DECLARE @lsql AS VARCHAR(2000) = '';
    DECLARE @lArea AS NVARCHAR(50) = '';
    DECLARE @lHour1 AS VARCHAR(8), @lHour2 AS VARCHAR(8);
    DECLARE @lSumNTamir AS FLOAT,@lSumTamir1 AS FLOAT,@lSumTamir2 AS FLOAT,@lSumTamir3 AS FLOAT
    DECLARE @lDT1 AS DATETIME, @lDT2 AS DATETIME;
    DECLARE @i AS INT = 0;
    CREATE TABLE #tmpData (Area NVARCHAR(50),Hour INT ,HourFrom VARCHAR(8) ,HourTo VARCHAR(8), 
      cntNotTamir FLOAT, cntTamir1 FLOAT,cntTamir2 FLOAT, cntTamir3 FLOAT)
    CREATE TABLE #tmpNet (IsLP BIT,IsMP Bit ,IsFT BIT)
    IF @aIsMPReq = 1 AND @aIsFoghTReq = 1 AND @aIsLPReq = 1 BEGIN  
    	SET @aIsFoghTReq = NULL
      SET @aIsMPReq = NULL
      SET @aIsLPReq = Null
    END
    INSERT #tmpNet SELECT @aIsLPReq, @aIsMPReq, @aIsFoghTReq
    CREATE TABLE #tmpArea (AreaId INT , Area NVARCHAR(50))
    	SET @lsql = ' SELECT AreaId, Area FROM tbl_Area '
    IF @aAreaIDs <> ''
    	SET  @lsql += ' WHERE AreaId IN (' + @aAreaIDs + ')'
   INSERT #tmpArea EXEC (@lsql)
   BEGIN TRY
      WHILE(@i < 24)
      BEGIN
        SET @lHour1 = CONVERT(VARCHAR(8), @i) + ':00' 
        SET @lHour2 = CONVERT(VARCHAR(8), @i + 1) + ':00'
        IF @i = 23 BEGIN SET @lHour2 = '23:59' END
        SET @lDT1 = CONVERT(DATETIME, @aDate +' '+ @lHour1 , 102)
        SET @lDT2 = CONVERT(DATETIME, @aDate +' '+ @lHour2 , 102)
        IF CHARINDEX(',',@aAreaIDs) > 0  BEGIN  ----------------Many Areas
            INSERT #tmpData SELECT 'åãå äæÇÍí' , @i, @lHour1, @lHour2,
           SUM(CASE WHEN t.IsTamir = 0 THEN dbo.MinuteCount(@lDT1,@lDT2, t.DisconnectDT,t.ConnectDT) ELSE 0 END) AS sumNotTamir,
           SUM(CASE WHEN t.IsTamir = 1 AND t.TamirTypeId = 1 THEN dbo.MinuteCount(@lDT1,@lDT2, t.DisconnectDT,t.ConnectDT) ELSE 0 END) AS sumTamir1,
           SUM(CASE WHEN t.IsTamir = 1 AND t.TamirTypeId = 2 THEN dbo.MinuteCount(@lDT1,@lDT2, t.DisconnectDT,t.ConnectDT) ELSE 0 END) AS sumTamir2,
           SUM(CASE WHEN t.IsTamir = 1 AND t.TamirTypeId = 3 THEN dbo.MinuteCount(@lDT1,@lDT2, t.DisconnectDT,t.ConnectDT) ELSE 0 END) AS sumTamir3
          FROM TblRequest t
          INNER JOIN #tmpArea a ON a.AreaId = t.AreaId
          WHERE t.DisconnectInterval > 0 
                AND t.ConnectDT IS NOT NULL
                AND t.DisconnectDatePersian = @aDatePersian
                AND EXISTS (SELECT 1 FROM #tmpNet n WHERE (n.IsFT = t.IsFogheToziRequest 
                      AND n.IsMP = t.IsMPRequest
                      AND n.IsLP = t.IsLPRequest)
                      OR n.IsFT IS NULL)
          SET @lArea = 'åãå äæÇÍí'
          END
        ELSE  BEGIN    ----------------One Area
          INSERT #tmpData SELECT a.Area, @i, @lHour1, @lHour2,
           SUM(CASE WHEN t.IsTamir = 0 THEN dbo.MinuteCount(@lDT1,@lDT2, t.DisconnectDT,t.ConnectDT) ELSE 0 END) AS sumNotTamir,
           SUM(CASE WHEN t.IsTamir = 1 AND t.TamirTypeId = 1 THEN dbo.MinuteCount(@lDT1,@lDT2, t.DisconnectDT,t.ConnectDT) ELSE 0 END) AS sumTamir1,
           SUM(CASE WHEN t.IsTamir = 1 AND t.TamirTypeId = 2 THEN dbo.MinuteCount(@lDT1,@lDT2, t.DisconnectDT,t.ConnectDT) ELSE 0 END) AS sumTamir2,
           SUM(CASE WHEN t.IsTamir = 1 AND t.TamirTypeId = 3 THEN dbo.MinuteCount(@lDT1,@lDT2, t.DisconnectDT,t.ConnectDT) ELSE 0 END) AS sumTamir3
          FROM TblRequest t
          INNER JOIN #tmpArea a ON a.AreaId = t.AreaId
          WHERE t.DisconnectInterval > 0 
                AND t.ConnectDT IS NOT NULL
                AND t.DisconnectDatePersian = @aDatePersian
                AND EXISTS (SELECT 1 FROM #tmpNet n WHERE (n.IsFT = t.IsFogheToziRequest 
                      AND n.IsMP = t.IsMPRequest
                      AND n.IsLP = t.IsLPRequest)
                      OR n.IsFT IS NULL)
          GROUP BY a.Area
          SELECT @lArea = Area FROM Tbl_Area WHERE AreaId = @aAreaIDs
          END
        SET @i = @i + 1
      END
      -----------------<Calculate Sum>-------------
      IF (SELECT COUNT(*) FROM #tmpData) = 0 BEGIN GOTO End_Calculation END
      SELECT TOP(24) @lSumNTamir = SUM(cntNotTamir), @lSumTamir1 = SUM(cntTamir1) ,
         @lSumTamir2 = SUM(cntTamir2), @lSumTamir3 = SUM(cntTamir3)
        FROM #tmpData
      INSERT #tmpData SELECT @lArea , 24 , '00:00','23:59', @lSumNTamir, @lSumTamir1 , @lSumTamir2, @lSumTamir3
      End_Calculation:
      -----------------</Calculate Sum>------------
      SELECT * FROM #tmpData
      DROP TABLE #tmpData
      DROP TABLE #tmpArea
      DROP TABLE  #tmpNet
    END TRY  
    BEGIN CATCH  
      DROP TABLE #tmpData
      DROP TABLE #tmpArea
      DROP TABLE  #tmpNet
      SELECT   
          ERROR_NUMBER() AS ErrorNumber  
          ,ERROR_LINE() AS Line
          ,ERROR_MESSAGE() AS ErrorMessage;  
    END CATCH;
END
GO
  -------------------------------------------------------------------------------------------------------
EXECUTE spDisHourly_Daily '2' ,'1387/06/10' , '2008/08/31' , 1,0,0


EXECUTE spDisHourly_Daily '2,3,5,6,8,9,4','1387/06/10' , '2008/08/31' , 1 , 1 ,1



  SELECT TOP(100) DisconnectDT, DisconnectDatePersian , AreaId FROM TblRequest  
    WHERE DisconnectInterval > 0  AND ConnectDT IS NOT NULL AND DisconnectDatePersian ='1387/06/10'
    ORDER BY DisconnectDatePersian DESC

  SELECT DisconnectDT, DisconnectDatePersian , COUNT(AreaId) AS count FROM TblRequest  
    WHERE DisconnectInterval > 0  AND ConnectDT IS NOT NULL
    GROUP BY DisconnectDT, DisconnectDatePersian
    ORDER BY COUNT(AreaId) DESC ,DisconnectDatePersian DESC

SELECT AreaId, Area FROM tbl_Area

SELECT * FROM Tbl_TamirType 

-------------------------Test---------------------

  SELECT * FROM Tbl_Area
