
ALTER PROCEDURE Emergency.spEmergency_GetTemplateData @aMPFeederTemplateId INT = -1
  AS
  BEGIN
    DECLARE @lSQL AS VARCHAR(2000) = '
    SELECT T.*, MPP.MPPostId , K1.KeyName AS MPFeederKey1, K2.KeyName AS MPFeederKey2,
      K3.KeyName AS MPFeederKey3, K4.KeyName AS MPFeederKey4,
      A.Area ,MPP.MPPostName ,MPF.MPFeederName ,LT.MPFeederLimitType
        FROM Emergency.Tbl_MPFeederTemplate T
      INNER JOIN Tbl_MPFeeder MPF ON T.MPFeederId = MPF.MPFeederId
      INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
      LEFT JOIN Emergency.Tbl_MPFeederLimitType LT ON T.MPFeederLimitTypeId = LT.MPFeederLimitTypeId
      LEFT JOIN Tbl_Area A ON T.AreaId = A.AreaId
      LEFT JOIN Tbl_MPFeederKey K1 ON T.MPFeederKeyId1 = K1.MPFeederKeyId
      LEFT JOIN Tbl_MPFeederKey K2 ON T.MPFeederKeyId2 = K2.MPFeederKeyId
      LEFT JOIN Tbl_MPFeederKey K3 ON T.MPFeederKeyId3 = K3.MPFeederKeyId
      LEFT JOIN Tbl_MPFeederKey K4 ON T.MPFeederKeyId4 = K4.MPFeederKeyId
      '
      IF @aMPFeederTemplateId > 0 
        BEGIN
           SET @lSQL = @lSQL + ' WHERE T.MPFeederTemplateId = ' + CAST(@aMPFeederTemplateId AS VARCHAR(100)) 
        END
  END


EXEC Emergency.spEmergency_GetTemplateData 100326569

