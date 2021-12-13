
CREATE PROCEDURE dbo.spGetReport_6_21_Post
   @aAreaIds AS VARCHAR(100)
  ,@aMPPost AS VARCHAR(100)
  ,@aMPFeederIds AS VARCHAR(100) 
  ,@aLPPostIds AS VARCHAR(100)
AS 
BEGIN
    DECLARE @lSQL AS VARCHAR(5000)
    DECLARE @lWhere AS VARCHAR(500)
    SET @lSQL = 'SELECT LPP.LPPostId, A.Area, MPP.MPPostName , MPF.MPFeederName , LPP.LPPostName, LPP.LPPostCode , LPP.PostCapacity
          ,Info.IsBarghGir , Info.IsCutout , Info.IsCoverBoshing , info.IsRelatedToAyegh
          ,SpcB_Type.SpecValue AS BType , SpcB_Fac.SpecValue AS BFac, SpcB_Cur.SpecValue AS BCurrent
          ,SpcC_Fuse.SpecValue AS CFCurrent, SpcC_Fac.SpecValue AS CFac, SpcC_Kind.SpecValue AS CKind
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
        LEFT JOIN TblSpec SpcC_Kind ON Info.spcCutout_TypeId = SpcC_Kind.SpecId '

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