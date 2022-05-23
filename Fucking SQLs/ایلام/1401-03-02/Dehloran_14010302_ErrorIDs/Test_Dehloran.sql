Select * From TblError Where ErrorId IN (99942202 , 99942080)

Select * from _tblImages

Select * from 
where ServiceCheckListId > 900000000000 and ServiceCheckListId < 1000000000000
order by ServiceCheckListId desc


Select top 100 * from BTblPicture 
--Where PicId = 900000000000002
Where PicId > 900000000000002 and PicId < 1000000000000000
Order By PicId DESC

--   900000000003399


Select top 100 * from BTblVoice 
Where VoiceId > 900000000000002 and VoiceId < 1000000000000000
Order By VoiceId DESC


/*
SET IDENTITY_INSERT _tblImages ON
GO

Insert into _tblImages (Id) Values (3399)

SET IDENTITY_INSERT _tblImages OFF
GO


*/