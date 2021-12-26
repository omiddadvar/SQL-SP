USE CCRequesterSetad
GO

SELECT *
FROM INFORMATION_SCHEMA.COLUMNS
  WHERE TABLE_NAME = 'TblDejenctor'
  AND COLUMN_NAME LIKE '%spc%'

SELECT *
FROM INFORMATION_SCHEMA.TABLES
  WHERE TABLE_NAME LIKE '%rele%' AND TABLE_TYPE = 'BASE TABLE'


  SELECT DISTINCT
   o.name AS Object_Name,
   o.type_desc
FROM sys.sql_modules m
   INNER JOIN
   sys.objects o
            ON m.object_id = o.object_id
WHERE m.definition Like '%title%'

  
EXEC sp_fkeys 'Tbl_RelehType'
-----------------------------------------------------------------------------------

SELECT * FROM Tbl_ServiceLogType 

SELECT * FROM Tbl_ServiceLogApplication

SELECT T.*,L.* FROM TblServiceLog L
  INNER JOIN Tbl_ServiceLogType T ON L.LogTypeId = T.ServiceLogTypeId


EXEC spAddServiceLog @aLogTypeId = 1
                    ,@aApplicationId = 2
                    ,@aURL = N'www.adrinsoft.ir'
                    ,@aParams = N'{name : "Naser"  , task : "Run your own business !!!!"}'
                    ,@aResult = N'{status : False , Data : "Fuck Off :D"}'
                    ,@aIsSuccess = 1


--TRUNCATE TABLE TblServiceLog

--INSERT INTO Tbl_ServiceLogType (ServiceLogTypeId, ServiceLogType) VALUES (2, N'GIS');
--  INSERT INTO Tbl_ServiceLogApplication (LogApplicationId, LogApplication) VALUES (2, N'TzServices');

SELECT * FROM TblServiceLog 
--  WHERE Result LIKE '%Tamir%'
  ORDER BY 1 DESC

------------------------------------------------------------
EXEC spGetReport_6_21_02 '' , '' , '' ,''

SELECT S1.SpecValue ,S2.SpecValue, S3.SpecValue,
  D.SerialNumber , D.AdjustCurrentEF_2 , D.AdjustTimeEF_2 , D.AdjustCurrentOC_2 , D.AdjustTimeOC_2 , D.AdjustReleh_2
  FROM TblDejenctor D
  LEFT JOIN TblSpec S1 ON D.spcAdjustCurrent_1Id = S1.SpecId
  LEFT JOIN TblSpec S2 ON D.spcAdjustTime_1Id = S2.SpecId
  LEFT JOIN TblSpec S3 ON D.spcAdjustReleh_1Id = S3.SpecId
  WHERE  SerialNumber = 25235

SELECT * FROM Tbl_RelehType
















 SELECT LPP.LPPostId
          ,Spc_Sec.SpecValue AS SType, Sec.SerialNumber AS SSerial, Sec.IsCutable AS SCutable
          ,Sec.IsGround AS SGround ,Spc_AppSec.SpecValue AS SApp ,Spc_FacSec.SpecValue AS SFac
          ,Spc_Mec.SpecValue AS SMech ,Spc_Sar.SpecValue AS SSar, Spc_Size.SpecValue AS SSize
		  ,Dej.SerialNumber ,Spc_Type.SpecValue AS DType , Spc_FacD.SpecValue AS DFac ,Spc_ModD.SpecValue AS DMod
          ,Spc_AppD.SpecValue AS DApp ,Spc_FCurr.SpecValue AS DFCurr ,Spc_RelehDC.SpecValue AS DRCycle
          ,Dej.IsReleh ,Dej.IsTermometer ,Dej.Isfan ,Dej.IsRelehBokh ,Releh.RelehType
		  ,Fuse.SerialNumber AS FSerial ,Spc_FacF.SpecValue AS FFac ,Spc_ModF.SpecValue AS FMod 
          ,Spc_AppF.SpecValue AS FApp ,Spc_Curr.SpecValue AS FCurr
          ,CASE WHEN FuseSecsunerId IS NOT NULL THEN 'سکسيونر فيوزدار' ELSE '' END AS FType
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
        LEFT JOIN TblSpec Spc_Size ON Sec.spcCableSizeId = Spc_Size.SpecId  WHERE LPP.IsHavayi = 0 AND (not Fuse.FuseSecsunerId is null or not Sec.SecsunerId is null or not Dej.DejenctorId is null)
