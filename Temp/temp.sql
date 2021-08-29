
SELECT *
FROM INFORMATION_SCHEMA.COLUMNS c
  WHERE TABLE_NAME = 'Tbl_LPPost'
  AND COLUMN_NAME LIKE 'Is%'

SELECT * FROM Tbl_LPPost

SELECT * FROM TblSecsuner

SELECT * FROM TblSpec



SELECT LPP.LPPostId, A.Area, MPP.MPPostName , MPF.MPFeederName , LPP.LPPostName, LPP.LPPostCode , LPP.PostCapacity
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
  LEFT JOIN TblSpec Spc_Size ON Sec.spcCableSizeId = Spc_Size.SpecId
WHERE LPP.IsHavayi = 0 AND A.AreaId = 3
