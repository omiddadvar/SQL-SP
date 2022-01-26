
--Maz--http://172.1Ì.50.103/TZHavades/Tavanir
SELECT * FROM Tbl_Config WHERE ConfigName LIKE '%Tozi%'


SELECT * FROM Tbl_AccessType

SELECT * FROM TblUserAccess

SELECT * FROM Tbl_User


---------------------------------------------------
SET IDENTITY_INSERT Tbl_AccessType ON
INSERT INTO Tbl_AccessType (AccessTypeId,AccessType, AccessTypeName)
  VALUES  (4,N'Monitoring', N' Ê«‰«ÌÌ œ” —”Ì »Â „«‰Ì Ê—Ì‰ê'),
          (5,N'ReportReliability', N' Ê«‰«ÌÌ ê“«—‘êÌ—Ì ‘«Œ’ Â«Ì ﬁ«»·Ì  «ÿ„Ì‰«‰ '),
          (6,N'ReportKambood', N' Ê«‰«ÌÌ ê“«—‘êÌ—Ì ò„»Êœ  Ê·Ìœ ”«⁄  »Â ”«⁄ '),
          (7,N'ReportSerghati', N' Ê«‰«ÌÌ ê“«—‘êÌ—Ì  ÃÂÌ“«  ”—ﬁ Ì')
SET IDENTITY_INSERT Tbl_AccessType Off
