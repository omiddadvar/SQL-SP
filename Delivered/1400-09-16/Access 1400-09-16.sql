IF NOT EXISTS (SELECT * FROM Tbl_FormObject WHERE FormObjectId = 201)
            INSERT Tbl_FormObject (FormObjectId,Tag,PersianTag,SysObjId) VALUES (201,N'CanSendRepeatSMS',N'توانايي ارسال مجدد پيامک خاموشی به مشترکين',24)
ELSE
            UPDATE Tbl_FormObject SET Tag = N'CanSendRepeatSMS',PersianTag = N'توانايي ارسال مجدد پيامک خاموشی به مشترکين',SysObjId = 24 WHERE FormObjectId = 201
GO

IF NOT EXISTS (SELECT * FROM Tbl_FormObjectApplication WHERE FormObjectApplicationId = 292)
            INSERT Tbl_FormObjectApplication (FormObjectApplicationId,ApplicationId,FormObjectId) VALUES (292,1,201)
ELSE
            UPDATE Tbl_FormObjectApplication SET ApplicationId = 1,FormObjectId = 201 WHERE FormObjectApplicationId = 292
GO
