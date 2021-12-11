IF NOT EXISTS (SELECT * FROM Tbl_FormObject WHERE FormObjectId = 1521)
            INSERT Tbl_FormObject (FormObjectId,Tag,PersianTag,SysObjId) VALUES (1521,N'ConfirmNazer',N' Ê«‰«ÌÌ  «ÌÌœ ‰«Ÿ—',30)
ELSE
            UPDATE Tbl_FormObject SET Tag = N'ConfirmNazer',PersianTag = N' Ê«‰«ÌÌ  «ÌÌœ ‰«Ÿ—',SysObjId = 30 WHERE FormObjectId = 1521
GO

IF NOT EXISTS (SELECT * FROM Tbl_FormObjectApplication WHERE FormObjectApplicationId = 2026)
            INSERT Tbl_FormObjectApplication (FormObjectApplicationId,ApplicationId,FormObjectId) VALUES (2026,4,1521)
ELSE
            UPDATE Tbl_FormObjectApplication SET ApplicationId = 4,FormObjectId = 1521 WHERE FormObjectApplicationId = 2026
GO
