CREATE PROCEDURE dbo.spGetReport_6_21_03
   @aAreaIds AS VARCHAR(100)
  ,@aMPPost AS VARCHAR(100)
  ,@aMPFeederIds AS VARCHAR(100) 
  ,@aLPPostIds AS VARCHAR(100)
  AS 
  BEGIN
    /* LPPost Dejector */
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
        LEFT JOIN TblSpec Spc_Fac ON Dej.spcDejenctor_FactoryId = Spc_Type.SpecId
        LEFT JOIN TblSpec Spc_Mod ON Dej.spcModelId = Spc_Type.SpecId
        LEFT JOIN TblSpec Spc_App ON Dej.spcApplicationId = Spc_Type.SpecId
        LEFT JOIN TblSpec Spc_FCurr ON Dej.spcFCurrentId = Spc_Type.SpecId
        LEFT JOIN TblSpec Spc_RelehDC ON Dej.spcRelehDCCycleType = Spc_Type.SpecId
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

--  EXEC spGetReport_6_21_03 '' ,'' ,'',''