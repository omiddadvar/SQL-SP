ALTER PROCEDURE spDisHourly_Daily(
  @aAreaIDs AS VARCHAR(100),
  @aDatePersian AS VARCHAR(10),
  @aDate AS VARCHAR(10),
  @aIsLPReq AS BIT = 1,
  @aIsMPReq AS BIT = 1,
  @aIsFoghTReq AS BIT = 1,
  @aMPPostIDs AS VARCHAR(MAX),
  @aMPFeederIDs AS VARCHAR(MAX),
  @aIsTamir AS INT, /*Ranges through -1,0,1 (-1 : Both)*/
  @aIsWithFT AS INT /*Ranges through -1,0,1 (-1 : All)*/
  ) AS
  BEGIN
    DECLARE @lsql AS NVARCHAR(MAX) = ''
      ,@lArea AS NVARCHAR(50)
      ,@lAreaGroupBy AS VARCHAR(50)
      ,@lMPPostCondition AS VARCHAR(MAX)
      ,@lMPFeederCondition AS VARCHAR(MAX)
      ,@lTamirCondition AS VARCHAR(MAX)
      ,@lFTCondition AS VARCHAR(MAX)
      ,@lHour1 AS VARCHAR(8), @lHour2 AS VARCHAR(8)
      ,@lSumNTamir AS FLOAT,@lSumTamir1 AS FLOAT,@lSumTamir2 AS FLOAT,@lSumTamir3 AS FLOAT
      ,@lDT1 AS DATETIME, @lDT2 AS DATETIME
      ,@i AS INT = 0;
    /*--Define Tables--*/
    CREATE TABLE #tmpData (Area NVARCHAR(50),Hour INT ,HourFrom VARCHAR(8) ,HourTo VARCHAR(8), 
      cntNotTamir FLOAT, cntTamir1 FLOAT,cntTamir2 FLOAT, cntTamir3 FLOAT)
    CREATE TABLE #tmpNet (IsLP BIT,IsMP Bit ,IsFT BIT)
    CREATE TABLE #tmpArea (AreaId INT , Area NVARCHAR(50))
    /*--Initialize Tables--*/
    IF @aIsMPReq = 1 AND @aIsFoghTReq = 1 AND @aIsLPReq = 1 BEGIN  
    	SET @aIsFoghTReq = NULL
      SET @aIsMPReq = NULL
      SET @aIsLPReq = Null
    END
    INSERT #tmpNet SELECT @aIsLPReq, @aIsMPReq, @aIsFoghTReq
  	SET @lsql = ' SELECT AreaId, Area FROM tbl_Area '
    IF @aAreaIDs <> ''
    	SET  @lsql += ' WHERE AreaId IN (' + @aAreaIDs + ')'
   INSERT #tmpArea EXEC (@lsql)
   /*--Fill Main DataTable--*/
   SET @lMPPostCondition = CASE WHEN @aMPPostIDs = '' THEN '' ELSE ' AND MPR.MPPostId IN ('+ @aMPPostIDs +')' END
   SET @lMPFeederCondition = CASE WHEN @aMPFeederIDs = '' THEN '' ELSE ' AND MPR.MPFeederId IN ('+ @aMPFeederIDs +')' END
   SET @lTamirCondition = CASE WHEN @aIsTamir = -1 THEN '' ELSE ' AND t.IsTamir = ' + CAST(@aIsTamir AS VARCHAR(2)) + '' END
   SET @lFTCondition = '((MPR.DisconnectReasonId IS NULL
      OR MPR.DisconnectReasonId < 1200 OR MPR.DisconnectReasonId > 1299 ) 
      AND ( MPR.DisconnectGroupSetId IS NULL OR MPR.DisconnectGroupSetId <> 1129 
      AND MPR.DisconnectGroupSetId <> 1130))'
   SET @lFTCondition = CASE 
      WHEN @aIsWithFT = 1 THEN ' AND NOT ' + @lFTCondition
      WHEN @aIsWithFT = 0 THEN ' AND ' + @lFTCondition ELSE '' END
   BEGIN TRY
      WHILE(@i < 24)
      BEGIN
        SET @lHour1 = CONVERT(VARCHAR(8), @i) + ':00' 
        SET @lHour2 = CONVERT(VARCHAR(8), @i + 1) + ':00'
        IF @i = 23 BEGIN SET @lHour2 = '23:59' END
        SET @lDT1 = CONVERT(DATETIME, @aDate +' '+ @lHour1 , 102)
        SET @lDT2 = CONVERT(DATETIME, @aDate +' '+ @lHour2 , 102)
        SET @lAreaGroupBy = CASE WHEN (CHARINDEX(',',@aAreaIDs) > 0) THEN '' ELSE ' GROUP BY a.Area' END
        SET @lArea = CASE WHEN CHARINDEX(',',@aAreaIDs) > 0 THEN '''åãå äæÇÍí''' ELSE 'a.Area' END
        SET @lsql = 'SELECT '+ @lArea + ',' + CAST(@i AS VARCHAR(2))
            + ',''' + @lHour1 +''','''+ @lHour2 +''',
           SUM(CASE WHEN t.IsTamir = 0 THEN dbo.MinuteCount('''
            + CAST(@lDT1 AS VARCHAR(50))+''','''+ CAST(@lDT2 AS VARCHAR(50))
              +''', t.DisconnectDT,t.ConnectDT) ELSE 0 END) AS sumNotTamir,
           SUM(CASE WHEN t.IsTamir = 1 AND t.TamirTypeId = 1 THEN dbo.MinuteCount('''
            + CAST(@lDT1 AS VARCHAR(50))+''','''+ CAST(@lDT2 AS VARCHAR(50))
              +''', t.DisconnectDT,t.ConnectDT) ELSE 0 END) AS sumTamir1,
           SUM(CASE WHEN t.IsTamir = 1 AND t.TamirTypeId = 2 THEN dbo.MinuteCount('''
            + CAST(@lDT1 AS VARCHAR(50))+''','''+ CAST(@lDT2 AS VARCHAR(50))
              +''',t.DisconnectDT,t.ConnectDT) ELSE 0 END) AS sumTamir2,
           SUM(CASE WHEN t.IsTamir = 1 AND t.TamirTypeId = 3 THEN dbo.MinuteCount('''
            + CAST(@lDT1 AS VARCHAR(50))+''','''+ CAST(@lDT2 AS VARCHAR(50))
              +''',t.DisconnectDT,t.ConnectDT) ELSE 0 END) AS sumTamir3
          FROM TblRequest t
          INNER JOIN #tmpArea a ON a.AreaId = t.AreaId
          LEFT JOIN TblMPRequest MPR ON t.MPRequestId = MPR.MPRequestId
          WHERE t.DisconnectInterval > 0 
                AND t.ConnectDT IS NOT NULL
                AND t.DisconnectDatePersian = '''+ @aDatePersian +'''
                AND EXISTS (SELECT 1 FROM #tmpNet n WHERE (n.IsFT = t.IsFogheToziRequest 
                      AND n.IsMP = t.IsMPRequest
                      AND n.IsLP = t.IsLPRequest)
                      OR n.IsFT IS NULL )'
                + @lMPPostCondition + @lMPFeederCondition + @lTamirCondition + @lFTCondition
                + @lAreaGroupBy
        INSERT #tmpData EXEC(@lsql)
        SET @i = @i + 1
      END
--      PRINT(@lsql)
      /*-----------------<Calculate Sum>-------------*/
      IF (SELECT COUNT(*) FROM #tmpData) > 0 BEGIN  
        SELECT TOP(24) @lArea = Area, @lSumNTamir = SUM(cntNotTamir), @lSumTamir1 = SUM(cntTamir1) ,
          @lSumTamir2 = SUM(cntTamir2), @lSumTamir3 = SUM(cntTamir3)
          FROM #tmpData
          GROUP BY Area
        INSERT #tmpData SELECT @lArea , 24 , '00:00','23:59', @lSumNTamir, @lSumTamir1 , @lSumTamir2, @lSumTamir3
      END
      /*-----------------</Calculate Sum>------------*/
      SELECT * FROM #tmpData
      DROP TABLE #tmpData
      DROP TABLE #tmpArea
      DROP TABLE  #tmpNet
    END TRY  
    BEGIN CATCH  
      DECLARE @ErrMsg AS VARCHAR(MAX)
          ,@ErrState AS INT
          ,@ErrSeverity AS INT
      DROP TABLE #tmpData
      DROP TABLE #tmpArea
      DROP TABLE  #tmpNet
      SELECT @ErrMsg = ERROR_MESSAGE() , @ErrState = ERROR_STATE() , @ErrSeverity = ERROR_SEVERITY()
      RAISERROR (@ErrMsg , @ErrSeverity , @ErrState);
    END CATCH
END
GO