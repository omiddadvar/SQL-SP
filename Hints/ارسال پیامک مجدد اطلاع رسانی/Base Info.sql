USE CCRequesterSetad
GO

SELECT * FROM TblRequest WHERE RequestNumber = 4009921088

SELECT * FROM TblRequestInform WHERE RequestId = 9900000000001539 -- Delete

SELECT * FROM Tbl_RequestInformJobState

SELECT * FROM TblSubscriberInfom WHERE RequestInformId = 98900000098363  -- If Exists -- Delete

SELECT * FROM Tbl_SendSMSStatus 