ALTER PROCEDURE spReportHourlyDisPowerGeneral
    @aFromDate VARCHAR(10),
    @aToDate VARCHAR(10)
  AS 
  BEGIN
    DECLARE @DTStart AS DATETIME
    DECLARE @DTEnd AS DATETIME
    SELECT @DTStart = dbo.shtom(@aFromDate)
    SELECT @DTEnd = dbo.shtom(@aToDate)
    CREATE TABLE #tmpDaily(
    	[Province] NVARCHAR(100) NULL,
    	[Hour00] FLOAT NULL,
    	[Hour01] FLOAT NULL,
    	[Hour02] FLOAT NULL,
    	[Hour03] FLOAT NULL,
    	[Hour04] FLOAT NULL,
    	[Hour05] FLOAT NULL,
    	[Hour06] FLOAT NULL,
    	[Hour07] FLOAT NULL,
    	[Hour08] FLOAT NULL,
    	[Hour09] FLOAT NULL,
    	[Hour10] FLOAT NULL,
    	[Hour11] FLOAT NULL,
    	[Hour12] FLOAT NULL,
    	[Hour13] FLOAT NULL,
    	[Hour14] FLOAT NULL,
    	[Hour15] FLOAT NULL,
    	[Hour16] FLOAT NULL,
    	[Hour17] FLOAT NULL,
    	[Hour18] FLOAT NULL,
    	[Hour19] FLOAT NULL,
    	[Hour20] FLOAT NULL,
    	[Hour21] FLOAT NULL,
    	[Hour22] FLOAT NULL,
    	[Hour23] FLOAT NULL
    ) ON [PRIMARY]
    CREATE TABLE #tmpTotal(
    	[Province] NVARCHAR(100) NULL,
    	[DatePersian] VARCHAR(10) NULL,
    	[Hour00] FLOAT NULL,
    	[Hour01] FLOAT NULL,
    	[Hour02] FLOAT NULL,
    	[Hour03] FLOAT NULL,
    	[Hour04] FLOAT NULL,
    	[Hour05] FLOAT NULL,
    	[Hour06] FLOAT NULL,
    	[Hour07] FLOAT NULL,
    	[Hour08] FLOAT NULL,
    	[Hour09] FLOAT NULL,
    	[Hour10] FLOAT NULL,
    	[Hour11] FLOAT NULL,
    	[Hour12] FLOAT NULL,
    	[Hour13] FLOAT NULL,
    	[Hour14] FLOAT NULL,
    	[Hour15] FLOAT NULL,
    	[Hour16] FLOAT NULL,
    	[Hour17] FLOAT NULL,
    	[Hour18] FLOAT NULL,
    	[Hour19] FLOAT NULL,
    	[Hour20] FLOAT NULL,
    	[Hour21] FLOAT NULL,
    	[Hour22] FLOAT NULL,
    	[Hour23] FLOAT NULL
    ) ON [PRIMARY]
    DECLARE @DT AS DATETIME = @DTStart
    WHILE @DT <= @DTEnd
    BEGIN
    	DECLARE @CurrentDate AS VARCHAR(10)
    	SELECT @CurrentDate = dbo.mtosh(@DT)
    	INSERT INTO #tmpDaily 	
    	  EXEC dbo.spReportHourlyDisconnectPower @CurrentDate
    	INSERT INTO #tmpTotal (Province, DatePersian, Hour00, Hour01, Hour02, Hour03, Hour04, Hour05
          , Hour06, Hour07, Hour08, Hour09, Hour10, Hour11, Hour12, Hour13, Hour14, Hour15, Hour16, Hour17
          , Hour18, Hour19, Hour20, Hour21, Hour22, Hour23)
    	  SELECT Province , @CurrentDate , 1, Hour01, Hour02, Hour03, 7, 5
          , Hour06 , Hour07, 12, Hour09, Hour10, 4, Hour12, 1, Hour14, 3, Hour16, 10.5
          , 3.5, Hour19, 4.5, Hour21, 6, Hour23
        FROM #tmpDaily
    	DELETE #tmpDaily
    	SET @DT = DATEADD(day, 1, @DT)
    END
    SELECT * FROM #tmpTotal
    DROP TABLE	#tmpDaily 
    DROP TABLE	#tmpTotal
  END
/*

  CREATE PROCEDURE spReportHourlyDisPowerGeneral
    @aFromDate VARCHAR(10),
    @aToDate VARCHAR(10)
  AS 
  BEGIN
    DECLARE @DTStart AS DATETIME
    DECLARE @DTEnd AS DATETIME
    SELECT @DTStart = dbo.shtom(@aFromDate)
    SELECT @DTEnd = dbo.shtom(@aToDate)
    CREATE TABLE #tmpDaily(
    	[Province] NVARCHAR(100) NULL,
    	[Hour00] FLOAT NULL,
    	[Hour01] FLOAT NULL,
    	[Hour02] FLOAT NULL,
    	[Hour03] FLOAT NULL,
    	[Hour04] FLOAT NULL,
    	[Hour05] FLOAT NULL,
    	[Hour06] FLOAT NULL,
    	[Hour07] FLOAT NULL,
    	[Hour08] FLOAT NULL,
    	[Hour09] FLOAT NULL,
    	[Hour10] FLOAT NULL,
    	[Hour11] FLOAT NULL,
    	[Hour12] FLOAT NULL,
    	[Hour13] FLOAT NULL,
    	[Hour14] FLOAT NULL,
    	[Hour15] FLOAT NULL,
    	[Hour16] FLOAT NULL,
    	[Hour17] FLOAT NULL,
    	[Hour18] FLOAT NULL,
    	[Hour19] FLOAT NULL,
    	[Hour20] FLOAT NULL,
    	[Hour21] FLOAT NULL,
    	[Hour22] FLOAT NULL,
    	[Hour23] FLOAT NULL
    ) ON [PRIMARY]
    CREATE TABLE #tmpTotal(
    	[Province] NVARCHAR(100) NULL,
    	[DatePersian] VARCHAR(10) NULL,
    	[Hour00] FLOAT NULL,
    	[Hour01] FLOAT NULL,
    	[Hour02] FLOAT NULL,
    	[Hour03] FLOAT NULL,
    	[Hour04] FLOAT NULL,
    	[Hour05] FLOAT NULL,
    	[Hour06] FLOAT NULL,
    	[Hour07] FLOAT NULL,
    	[Hour08] FLOAT NULL,
    	[Hour09] FLOAT NULL,
    	[Hour10] FLOAT NULL,
    	[Hour11] FLOAT NULL,
    	[Hour12] FLOAT NULL,
    	[Hour13] FLOAT NULL,
    	[Hour14] FLOAT NULL,
    	[Hour15] FLOAT NULL,
    	[Hour16] FLOAT NULL,
    	[Hour17] FLOAT NULL,
    	[Hour18] FLOAT NULL,
    	[Hour19] FLOAT NULL,
    	[Hour20] FLOAT NULL,
    	[Hour21] FLOAT NULL,
    	[Hour22] FLOAT NULL,
    	[Hour23] FLOAT NULL
    ) ON [PRIMARY]
    DECLARE @DT AS DATETIME = @DTStart
    WHILE @DT <= @DTEnd
    BEGIN
    	DECLARE @CurrentDate AS VARCHAR(10)
    	SELECT @CurrentDate = dbo.mtosh(@DT)
    	INSERT INTO #tmpDaily 	
    	  EXEC dbo.spReportHourlyDisconnectPower @CurrentDate
    	INSERT INTO #tmpTotal (Province, DatePersian, Hour00, Hour01, Hour02, Hour03, Hour04, Hour05
          , Hour06, Hour07, Hour08, Hour09, Hour10, Hour11, Hour12, Hour13, Hour14, Hour15, Hour16, Hour17
          , Hour18, Hour19, Hour20, Hour21, Hour22, Hour23)
    	  SELECT Province , @CurrentDate , Hour00, Hour01, Hour02, Hour03, Hour04, Hour05
          , Hour06 , Hour07, Hour08, Hour09, Hour10, Hour11, Hour12, Hour13, Hour14, Hour15, Hour16, Hour17
          , Hour18, Hour19, Hour20, Hour21, Hour22, Hour23
        FROM #tmpDaily
    	DELETE #tmpDaily
    	SET @DT = DATEADD(day, 1, @DT)
    END
    SELECT * FROM #tmpTotal
    DROP TABLE	#tmpDaily 
    DROP TABLE	#tmpTotal
  END
