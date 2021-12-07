
ALTER PROCEDURE spGetReport_6_21_01
   @aAreaIds AS VARCHAR(100)
  ,@aMPPost AS VARCHAR(100)
  ,@aMPFeederIds AS VARCHAR(100) 
  ,@aLPPostIds AS VARCHAR(100)
AS 
BEGIN
    /* LPPost Sectioner */
    DECLARE @lSQL AS VARCHAR(5000)
    DECLARE @lWhere AS VARCHAR(500)
    SET @lSQL = 'SELECT LPP.LPPostId, A.Area, MPP.MPPostName , MPF.MPFeederName , LPP.LPPostName, LPP.LPPostCode , LPP.PostCapacity
          ,Info.IsBarghGir , Info.IsCutout , Info.IsCoverBoshing , info.IsRelatedToAyegh
          ,SpcB_Type.SpecValue AS BType , SpcB_Fac.SpecValue AS BFac, SpcB_Cur.SpecValue AS BCurrent
          ,SpcC_Fuse.SpecValue AS CFCurrent, SpcC_Fac.SpecValue AS CFac, SpcC_Kind.SpecValue AS CKind
          ,Spc_Sec.SpecValue AS SType, Sec.SerialNumber AS SSerial, Sec.IsCutable AS SCutable
          ,Sec.IsGround AS SGround ,Spc_App.SpecValue AS SApp ,Spc_Fac.SpecValue AS SFac
          ,Spc_Mec.SpecValue AS SMech ,Spc_Sar.SpecValue AS SSar, Spc_Size.SpecValue AS SSize
        FROM Tbl_LPPost LPP
        INNER JOIN Tbl_MPFeeder MPF ON LPP.MPFeederId = MPF.MPFeederId
        INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
        INNER JOIN Tbl_Area A ON LPP.AreaId = A.AreaId
        INNER JOIN TblSecsuner Sec ON LPP.LPPostId = Sec.LPPostId
        INNER JOIN Tbl_LPPostInfo Info ON LPP.LPPostId = Info.LPPostId
        LEFT JOIN TblSpec SpcB_Type ON Info.spcBarghGir_JensId = SpcB_Type.SpecId
        LEFT JOIN TblSpec SpcB_Fac ON Info.spcBarghGir_FactoryId = SpcB_Fac.SpecId
        LEFT JOIN TblSpec SpcB_Cur ON Info.spcBarghGir_CurrentId = SpcB_Cur.SpecId
        LEFT JOIN TblSpec SpcC_Fuse ON Info.spcCutout_FuseCurrentId = SpcC_Fuse.SpecId
        LEFT JOIN TblSpec SpcC_Fac ON Info.spcCutout_FactoryId = SpcC_Fac.SpecId
        LEFT JOIN TblSpec SpcC_Kind ON Info.spcCutout_TypeId = SpcC_Kind.SpecId
        LEFT JOIN TblSpec Spc_Sec ON Sec.spcSecsunerTypeId = Spc_Sec.SpecId
        LEFT JOIN TblSpec Spc_App ON Sec.spcApplicationId = Spc_App.SpecId
        LEFT JOIN TblSpec Spc_Fac ON Sec.spcFactoryId = Spc_Fac.SpecId
        LEFT JOIN TblSpec Spc_Mec ON Sec.spcMechanismId = Spc_Mec.SpecId
        LEFT JOIN TblSpec Spc_Sar ON Sec.spcSarkablId = Spc_Sar.SpecId
        LEFT JOIN TblSpec Spc_Size ON Sec.spcCableSizeId = Spc_Size.SpecId '

      SET @lWhere = ' WHERE LPP.IsHavayi = 0'
      IF @aLPPostIds <> '' BEGIN
        SET @lWhere = @lWhere + ' AND LPP.LPPostId IN (' + @aLPPostIds + ')'
      END
      ELSE IF @aMPFeederIds <> '' BEGIN
        SET @lWhere = @lWhere + ' AND MPF.MPFeederId IN (' + @aMPFeederIds + ')' 
      END
      ELSE IF @aMPPost <> '' BEGIN
        SET @lWhere = @lWhere + ' AND MPP.MPPostId IN (' + @aMPPost + ')' 
      END
      ELSE IF @aAreaIds <> '' BEGIN
      	SET @lWhere = @lWhere + ' AND A.AreaId IN (' + @aAreaIds + ')'
      END
      SET @lSQL = @lSQL + @lWhere
      
      EXEC(@lSQL)
END 
GO


ALTER PROCEDURE dbo.spGetReport_6_21_02
   @aAreaIds AS VARCHAR(100)
  ,@aMPPost AS VARCHAR(100)
  ,@aMPFeederIds AS VARCHAR(100) 
  ,@aLPPostIds AS VARCHAR(100)
AS 
BEGIN
    /* LPPost Fuse-Sectioner */
    DECLARE @lSQL AS VARCHAR(5000)
    DECLARE @lWhere AS VARCHAR(500)
    SET @lSQL = 'SELECT LPP.LPPostId, A.Area, MPP.MPPostName , MPF.MPFeederName , LPP.LPPostName, LPP.LPPostCode , LPP.PostCapacity
          ,Info.IsBarghGir , Info.IsCutout , Info.IsCoverBoshing , info.IsRelatedToAyegh
          ,SpcB_Type.SpecValue AS BType , SpcB_Fac.SpecValue AS BFac, SpcB_Cur.SpecValue AS BCurrent
          ,SpcC_Fuse.SpecValue AS CFCurrent, SpcC_Fac.SpecValue AS CFac, SpcC_Kind.SpecValue AS CKind
          ,Fuse.SerialNumber AS FSerial ,Spc_Fac.SpecValue AS FFac ,Spc_Mod.SpecValue AS FMod 
          ,Spc_App.SpecValue AS FApp ,Spc_Curr.SpecValue AS FCurr
        FROM Tbl_LPPost LPP
        INNER JOIN Tbl_MPFeeder MPF ON LPP.MPFeederId = MPF.MPFeederId
        INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
        INNER JOIN Tbl_Area A ON LPP.AreaId = A.AreaId
        INNER JOIN TblFuseSecsuner Fuse ON LPP.LPPostId = Fuse.LPPostId
        INNER JOIN Tbl_LPPostInfo Info ON LPP.LPPostId = Info.LPPostId
        LEFT JOIN TblSpec SpcB_Type ON Info.spcBarghGir_JensId = SpcB_Type.SpecId
        LEFT JOIN TblSpec SpcB_Fac ON Info.spcBarghGir_FactoryId = SpcB_Fac.SpecId
        LEFT JOIN TblSpec SpcB_Cur ON Info.spcBarghGir_CurrentId = SpcB_Cur.SpecId
        LEFT JOIN TblSpec SpcC_Fuse ON Info.spcCutout_FuseCurrentId = SpcC_Fuse.SpecId
        LEFT JOIN TblSpec SpcC_Fac ON Info.spcCutout_FactoryId = SpcC_Fac.SpecId
        LEFT JOIN TblSpec SpcC_Kind ON Info.spcCutout_TypeId = SpcC_Kind.SpecId
        LEFT JOIN TblSpec Spc_Fac ON Fuse.spcFactoryId = Spc_Fac.SpecId
        LEFT JOIN TblSpec Spc_Mod ON Fuse.spcModelId = Spc_Mod.SpecId
        LEFT JOIN TblSpec Spc_App ON Fuse.spcApplicationId = Spc_App.SpecId
        LEFT JOIN TblSpec Spc_Curr ON Fuse.spcFuseCurrentId = Spc_Curr.SpecId '

      SET @lWhere = ' WHERE LPP.IsHavayi = 0'
      IF @aLPPostIds <> '' BEGIN
        SET @lWhere = @lWhere + ' AND LPP.LPPostId IN (' + @aLPPostIds + ')'
      END
      ELSE IF @aMPFeederIds <> '' BEGIN
        SET @lWhere = @lWhere + ' AND MPF.MPFeederId IN (' + @aMPFeederIds + ')' 
      END
      ELSE IF @aMPPost <> '' BEGIN
        SET @lWhere = @lWhere + ' AND MPP.MPPostId IN (' + @aMPPost + ')' 
      END
      ELSE IF @aAreaIds <> '' BEGIN
      	SET @lWhere = @lWhere + ' AND A.AreaId IN (' + @aAreaIds + ')'
      END
      SET @lSQL = @lSQL + @lWhere
      
      EXEC(@lSQL)
END 
GO


ALTER PROCEDURE dbo.spGetReport_6_21_03
   @aAreaIds AS VARCHAR(100)
  ,@aMPPost AS VARCHAR(100)
  ,@aMPFeederIds AS VARCHAR(100) 
  ,@aLPPostIds AS VARCHAR(100)
AS 
BEGIN
    /* LPPost Dejenctort */
    DECLARE @lSQL AS VARCHAR(5000)
    DECLARE @lWhere AS VARCHAR(500)
    SET @lSQL = 'SELECT LPP.LPPostId, A.Area, MPP.MPPostName , MPF.MPFeederName , LPP.LPPostName, LPP.LPPostCode , LPP.PostCapacity
          ,Info.IsBarghGir , Info.IsCutout , Info.IsCoverBoshing , info.IsRelatedToAyegh
          ,SpcB_Type.SpecValue AS BType , SpcB_Fac.SpecValue AS BFac, SpcB_Cur.SpecValue AS BCurrent
          ,SpcC_Fuse.SpecValue AS CFCurrent, SpcC_Fac.SpecValue AS CFac, SpcC_Kind.SpecValue AS CKind
          ,Dej.SerialNumber ,Spc_Type.SpecValue AS DType , Spc_Fac.SpecValue AS DFac ,Spc_Mod.SpecValue AS DMod
          ,Spc_App.SpecValue AS DApp ,Spc_FCurr.SpecValue AS DFCurr ,Spc_RelehDC.SpecValue AS DRCycle
          ,Dej.IsReleh ,Dej.IsTermometer ,Dej.Isfan ,Dej.IsRelehBokh ,Releh.RelehType
        FROM Tbl_LPPost LPP
        INNER JOIN Tbl_MPFeeder MPF ON LPP.MPFeederId = MPF.MPFeederId
        INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
        INNER JOIN Tbl_Area A ON LPP.AreaId = A.AreaId
        INNER JOIN TblDejenctor Dej ON LPP.LPPostId = Dej.LPPostId
        INNER JOIN Tbl_LPPostInfo Info ON LPP.LPPostId = Info.LPPostId
        LEFT JOIN TblSpec SpcB_Type ON Info.spcBarghGir_JensId = SpcB_Type.SpecId
        LEFT JOIN TblSpec SpcB_Fac ON Info.spcBarghGir_FactoryId = SpcB_Fac.SpecId
        LEFT JOIN TblSpec SpcB_Cur ON Info.spcBarghGir_CurrentId = SpcB_Cur.SpecId
        LEFT JOIN TblSpec SpcC_Fuse ON Info.spcCutout_FuseCurrentId = SpcC_Fuse.SpecId
        LEFT JOIN TblSpec SpcC_Fac ON Info.spcCutout_FactoryId = SpcC_Fac.SpecId
        LEFT JOIN TblSpec SpcC_Kind ON Info.spcCutout_TypeId = SpcC_Kind.SpecId
        LEFT JOIN TblSpec Spc_Type ON Dej.spcDejenctortypeId = Spc_Type.SpecId
        LEFT JOIN TblSpec Spc_Fac ON Dej.spcDejenctor_FactoryId = Spc_Fac.SpecId
        LEFT JOIN TblSpec Spc_Mod ON Dej.spcModelId = Spc_Mod.SpecId
        LEFT JOIN TblSpec Spc_App ON Dej.spcApplicationId = Spc_App.SpecId
        LEFT JOIN TblSpec Spc_FCurr ON Dej.spcFCurrentId = Spc_FCurr.SpecId
        LEFT JOIN TblSpec Spc_RelehDC ON Dej.spcRelehDCCycleType = Spc_RelehDC.SpecId
        LEFT JOIN Tbl_RelehType Releh ON Dej.RelehTypeId = Releh.RelehTypeId '
      
      SET @lWhere = ' WHERE LPP.IsHavayi = 0'
      IF @aLPPostIds <> '' BEGIN
        SET @lWhere = @lWhere + ' AND LPP.LPPostId IN (' + @aLPPostIds + ')'
      END
      ELSE IF @aMPFeederIds <> '' BEGIN
        SET @lWhere = @lWhere + ' AND MPF.MPFeederId IN (' + @aMPFeederIds + ')' 
      END
      ELSE IF @aMPPost <> '' BEGIN
        SET @lWhere = @lWhere + ' AND MPP.MPPostId IN (' + @aMPPost + ')' 
      END
      ELSE IF @aAreaIds <> '' BEGIN
      	SET @lWhere = @lWhere + ' AND A.AreaId IN (' + @aAreaIds + ')'
      END
      SET @lSQL = @lSQL + @lWhere
      
      EXEC(@lSQL)
END
GO

ALTER PROCEDURE spHomaReportArea	
  @aAreaIDs AS VARCHAR(100)
  ,@aStartDate AS VARCHAR(10)
  ,@aEndDate AS VARCHAR(10)
AS
BEGIN
	DECLARE @lSQLMain VARCHAR(2000)
    ,@lSQL VARCHAR(2000)
    ,@lIsTamir VARCHAR(50) = 'TblJob.IsTamir = 0 AND'
    ,@lTableName VARCHAR(50) = 'TblRequestMP'
  CREATE TABLE #tmpArea (AreaId INT)
  CREATE TABLE #HomaMPNotTamir (AreaId INT, cnt INT)
  CREATE TABLE #HomaMPTamir (AreaId INT, cnt INT)
  CREATE TABLE #HomaLPNotTamir (AreaId INT, cnt INT)
  CREATE TABLE #HomaLPTamir (AreaId INT, cnt INT)
  CREATE TABLE #HomaSubscriber (AreaId INT, cnt INT)
  INSERT INTO #tmpArea EXEC('SELECT AreaId FROM Tbl_Area WHERE AreaId IN ('+ @aAreaIDs +')')
  SET @lSQLMain ='SELECT TblJob.AreaId,COUNT(*) AS cnt
    	  FROM Homa.['+ @lTableName +']
    	  INNER JOIN Homa.TblRequestCommon
    	    ON ['+ @lTableName +'].RequestCommonId=TblRequestCommon.RequestCommonId
    	  INNER JOIN Homa.TblJob ON TblJob.JobId=TblRequestCommon.JobId
    	  WHERE '+ @lIsTamir +' IsDuplicate=0 AND JobStatusId IN (10,11)
          AND TblJob.AreaId IN ('+ @aAreaIDs +')
          AND TblJob.DisconnectDatePersian BETWEEN '''+ @aStartDate +''' AND '''+ @aEndDate + ''' 
    	  GROUP BY TblJob.AreaId'
  INSERT INTO #HomaMPNotTamir EXEC(@lSQLMain)

  SET @lSQL = REPLACE(@lSQLMain, @lIsTamir , 'TblJob.IsTamir = 1 AND')
  INSERT INTO #HomaMPTamir EXEC(@lSQL)

  SET @lSQL = REPLACE(@lSQLMain, @lTableName , 'TblRequestLP')
  INSERT INTO #HomaLPNotTamir EXEC(@lSQL)

  SET @lSQL = REPLACE(@lSQLMain, @lIsTamir , 'TblJob.IsTamir = 1 AND')
  SET @lSQL = REPLACE(@lSQL, @lTableName , 'TblRequestLP')
  INSERT INTO #HomaLPTamir EXEC(@lSQL)

  SET @lSQL = REPLACE(@lSQLMain, @lIsTamir , '')
  SET @lSQL = REPLACE(@lSQL, @lTableName , 'TblRequestSubscriber')
  INSERT INTO #HomaSubscriber EXEC(@lSQL)

  SELECT AreaId,
		SUM(TblTraceSummary.TraceLen) AS TraceLen,
		SUM(TblTraceSummary.OnCallHour) AS OnCallHour,
		COUNT(*) AS OnCallCount
	INTO #tmpTrace
  	FROM Homa.TblTraceSummary
  	WHERE TargetDatePersian BETWEEN @aStartDate AND @aEndDate	
  	GROUP BY AreaId
  /*TODO*/
  SELECT Area.Area 
    , ISNULL(MPNT.cnt , 0) AS MPNotTamirCount
    , ISNULL(MPT.cnt , 0) AS MPTamirCount
    , ISNULL(LPNT.cnt , 0)+ ISNULL(Sub.cnt , 0) AS LPNotTamirCount
    , ISNULL(LPT.cnt , 0)  AS LPTamirCount
    , ISNULL(ROUND(TR.TraceLen/1000,2) , 0) AS TraceLen
    , ISNULL(ROUND(OnCallHour,2) , 0) AS OnCallHour
    , ISNULL(TR.OnCallCount , 0) AS OnCallCount
    FROM Tbl_Area Area
    INNER JOIN #tmpArea A ON Area.AreaId = A.AreaId
    LEFT JOIN #HomaMPNotTamir MPNT ON Area.AreaId = MPNT.AreaId
    LEFT JOIN #HomaMPTamir MPT ON  Area.AreaId = MPT.AreaId
    LEFT JOIN #HomaLPNotTamir LPNT ON Area.AreaId = LPNT.AreaId
    LEFT JOIN #HomaLPTamir LPT ON Area.AreaId = LPT.AreaId
    LEFT JOIN #tmpTrace TR ON Area.AreaId = TR.AreaId
    LEFT JOIN #HomaSubscriber Sub ON Area.AreaId = Sub.AreaId
    

	DROP TABLE #HomaMPNotTamir
	DROP TABLE #HomaMPTamir
	DROP TABLE #HomaLPNotTamir
	DROP TABLE #HomaLPTamir
	DROP TABLE #HomaSubscriber
  DROP TABLE #tmpTrace
  DROP TABLE #tmpArea
END
GO