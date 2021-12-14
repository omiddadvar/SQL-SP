
ALTER PROCEDURE spGetReport_6_21
   @aAreaIds AS VARCHAR(100)
  ,@aMPPost AS VARCHAR(100)
  ,@aMPFeederIds AS VARCHAR(100) 
  ,@aLPPostIds AS VARCHAR(100)
AS 
BEGIN
    DECLARE @lSQL AS VARCHAR(5000)
    DECLARE @lWhere AS VARCHAR(500)
    SET @lSQL = 'SELECT LPP.LPPostId
          ,Spc_Sec.SpecValue AS SType, Sec.SerialNumber AS SSerial, Sec.IsCutable AS SCutable
          ,Sec.IsGround AS SGround ,Spc_AppSec.SpecValue AS SApp ,Spc_FacSec.SpecValue AS SFac
          ,Spc_Mec.SpecValue AS SMech ,Spc_Sar.SpecValue AS SSar, Spc_Size.SpecValue AS SSize
		  ,Dej.SerialNumber ,Spc_Type.SpecValue AS DType , Spc_FacD.SpecValue AS DFac ,Spc_ModD.SpecValue AS DMod
          ,Spc_AppD.SpecValue AS DApp ,Spc_FCurr.SpecValue AS DFCurr ,Spc_RelehDC.SpecValue AS DRCycle
          ,Dej.IsReleh ,Dej.IsTermometer ,Dej.Isfan ,Dej.IsRelehBokh ,Releh.RelehType
		  ,Fuse.SerialNumber AS FSerial ,Spc_FacF.SpecValue AS FFac ,Spc_ModF.SpecValue AS FMod 
          ,Spc_AppF.SpecValue AS FApp ,Spc_Curr.SpecValue AS FCurr
          ,CASE WHEN FuseSecsunerId IS NOT NULL THEN ''”ò”ÌÊ‰— ›ÌÊ“œ«—'' ELSE '''' END AS FType
        FROM Tbl_LPPost LPP
        INNER JOIN Tbl_MPFeeder MPF ON LPP.MPFeederId = MPF.MPFeederId
        INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
        INNER JOIN Tbl_Area A ON LPP.AreaId = A.AreaId
        LEFT JOIN TblFuseSecsuner Fuse ON LPP.LPPostId = Fuse.LPPostId
		LEFT JOIN TblSecsuner Sec ON LPP.LPPostId = Sec.LPPostId
		LEFT JOIN TblDejenctor Dej ON LPP.LPPostId = Dej.LPPostId
        INNER JOIN Tbl_LPPostInfo Info ON LPP.LPPostId = Info.LPPostId
        LEFT JOIN TblSpec SpcB_Type ON Info.spcBarghGir_JensId = SpcB_Type.SpecId
        LEFT JOIN TblSpec SpcB_Fac ON Info.spcBarghGir_FactoryId = SpcB_Fac.SpecId
        LEFT JOIN TblSpec SpcB_Cur ON Info.spcBarghGir_CurrentId = SpcB_Cur.SpecId
        LEFT JOIN TblSpec SpcC_Fuse ON Info.spcCutout_FuseCurrentId = SpcC_Fuse.SpecId
        LEFT JOIN TblSpec SpcC_Fac ON Info.spcCutout_FactoryId = SpcC_Fac.SpecId
        LEFT JOIN TblSpec SpcC_Kind ON Info.spcCutout_TypeId = SpcC_Kind.SpecId
        LEFT JOIN TblSpec Spc_FacF ON Fuse.spcFactoryId = Spc_FacF.SpecId
        LEFT JOIN TblSpec Spc_ModF ON Fuse.spcModelId = Spc_ModF.SpecId
        LEFT JOIN TblSpec Spc_AppF ON Fuse.spcApplicationId = Spc_AppF.SpecId
        LEFT JOIN TblSpec Spc_Curr ON Fuse.spcFuseCurrentId = Spc_Curr.SpecId 
        LEFT JOIN TblSpec Spc_Type ON Dej.spcDejenctortypeId = Spc_Type.SpecId
        LEFT JOIN TblSpec Spc_FacD ON Dej.spcDejenctor_FactoryId = Spc_FacD.SpecId
        LEFT JOIN TblSpec Spc_ModD ON Dej.spcModelId = Spc_ModD.SpecId
        LEFT JOIN TblSpec Spc_AppD ON Dej.spcApplicationId = Spc_AppD.SpecId
        LEFT JOIN TblSpec Spc_FCurr ON Dej.spcFCurrentId = Spc_FCurr.SpecId
        LEFT JOIN TblSpec Spc_RelehDC ON Dej.spcRelehDCCycleType = Spc_RelehDC.SpecId
        LEFT JOIN Tbl_RelehType Releh ON Dej.RelehTypeId = Releh.RelehTypeId 
		LEFT JOIN TblSpec Spc_Sec ON Sec.spcSecsunerTypeId = Spc_Sec.SpecId
        LEFT JOIN TblSpec Spc_AppSec ON Sec.spcApplicationId = Spc_AppSec.SpecId
        LEFT JOIN TblSpec Spc_FacSec ON Sec.spcFactoryId = Spc_FacSec.SpecId
        LEFT JOIN TblSpec Spc_Mec ON Sec.spcMechanismId = Spc_Mec.SpecId
        LEFT JOIN TblSpec Spc_Sar ON Sec.spcSarkablId = Spc_Sar.SpecId
        LEFT JOIN TblSpec Spc_Size ON Sec.spcCableSizeId = Spc_Size.SpecId '

      SET @lWhere = ' WHERE LPP.IsHavayi = 0 AND (not Fuse.FuseSecsunerId is null or not Sec.SecsunerId is null or not Dej.DejenctorId is null) '
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