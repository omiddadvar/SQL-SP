if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TblRecloserFunction]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
CREATE TABLE [dbo].[TblRecloserFunction](
	[RecloserFunctionId] [bigint] NOT NULL,
	[MPFeederId] [int] NULL,
	[MPFeederKeyId] [bigint] NULL,
	[spcRecloserTypeId] [int] NULL,
	[spcRecloserModelId] [int] NULL,
	[ReadDT] [datetime] NULL,
	[ReadDatePersian] [varchar](10) NULL,
	[ReadTime] [varchar](5) NULL,
	[RestartCounterCount] [int] NULL,
	[TripCounterCount] [int] NULL,
	[FaultCounterCount] [int] NULL,
	[DataEntryDT] [datetime] NULL,
	[DataEntryDatePersian] [varchar](10) NULL,
	[DataEntryTime] [varchar](5) NULL,
	[AreaUserId] [int] NULL,
	CONSTRAINT [PK_TblRecloserFunction] PRIMARY KEY CLUSTERED 
	(
		[RecloserFunctionId] ASC
	) ON [PRIMARY],
	CONSTRAINT [FK_TblRecloserFunction_TblSpecModel] FOREIGN KEY([spcRecloserModelId]) REFERENCES [dbo].[TblSpec] ([SpecId]),
	CONSTRAINT [FK_TblRecloserFunction_TblSpecType] FOREIGN KEY([spcRecloserTypeId]) REFERENCES [dbo].[TblSpec] ([SpecId])
) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM Tbl_TableName WHERE TableName = 'TblRecloserFunction') 
  INSERT INTO Tbl_TableName (TableNameId,TableName,IsBroadcast,IsCenterUpdate,IsSetadUpdate,IsMiscModeUpdate,IsNahiehUpdate) 
  VALUES (358,N'TblRecloserFunction',1,Null,Null,Null,Null)
GO

IF NOT EXISTS (SELECT * FROM Tbl_SpecType WHERE SpecTypeId = 107)
	INSERT Tbl_SpecType (SpecTypeId,SpecTypeName,SpecTypeValueType,IsEditable,SpecTypeGroupId) VALUES (107,Replace(N'انواع ر\0x6CCکلوزر','\0x6CC',nchar(1610)),N'text',1,14)
ELSE
	UPDATE Tbl_SpecType SET SpecTypeName = Replace(N'انواع ر\0x6CCکلوزر','\0x6CC',nchar(1610)),SpecTypeValueType = N'text',IsEditable = 1,SpecTypeGroupId = 14 WHERE SpecTypeId = 107
GO
IF NOT EXISTS (SELECT * FROM Tbl_SpecType WHERE SpecTypeId = 108)
	INSERT Tbl_SpecType (SpecTypeId,SpecTypeName,SpecTypeValueType,IsEditable,SpecTypeGroupId) VALUES (108,Replace(N'مدل‌ها\0x6CC ر\0x6CCکلوزر','\0x6CC',nchar(1610)),N'text',1,14)
ELSE
	UPDATE Tbl_SpecType SET SpecTypeName = Replace(N'مدل‌ها\0x6CC ر\0x6CCکلوزر','\0x6CC',nchar(1610)),SpecTypeValueType = N'text',IsEditable = 1,SpecTypeGroupId = 14 WHERE SpecTypeId = 108
GO
