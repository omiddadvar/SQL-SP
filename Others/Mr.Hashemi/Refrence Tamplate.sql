-- > ExecQuery
IF NOT EXISTS(SELECT * FROM Tbl_ExecQuery WHERE ExecQueryId  =0000) 
   Insert Into Tbl_ExecQuery (ExecQueryId,Query) VALUES (0000,'')
GO

-- > Inser New record
if not exists (select * from TableName where PrimaryKeyFieldName = #) 
	INSERT TableName(Field1,...) VALUES(Value1,...)
else
	UPDATE TableName SET Field1 = Value1,... WHERE PrimaryKeyFieldName = #
GO

-- > Create new table
if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TableName]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
CREATE TABLE [dbo].[TableName] (
	[fld1] [type1] NOT NULL ,
	[fld2] [type2] (len2) COLLATE Arabic_CI_AS_WS NULL ,
	CONSTRAINT [PK_TableName] PRIMARY KEY  CLUSTERED 
	(
		[pkid]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO

-- > Create new field
IF NOT EXISTS(SELECT * FROM sysobjects INNER JOIN  syscolumns ON sysobjects.id = syscolumns.id WHERE (sysobjects.id = OBJECT_ID(N'TableName')) AND (syscolumns.name = 'FieldName'))
	ALTER TABLE TableName WITH CHECK ADD FieldName nvarchar(50) NULL
GO 

-- > Delete an existing field
IF EXISTS(SELECT * FROM sysobjects INNER JOIN  syscolumns ON sysobjects.id = syscolumns.id WHERE (sysobjects.id = OBJECT_ID(N'TableName')) AND (syscolumns.name = 'FieldName'))
	ALTER TABLE TableName DROP COLUMN FieldName
GO

-- > Create Foreign key
if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Tbl_PK_Tbl_FK]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
    ALTER TABLE [dbo].[Tbl_FK] ADD 
	CONSTRAINT [FK_Tbl_PK_Tbl_FK] FOREIGN KEY 
	(
		[FK]
	) REFERENCES [dbo].[Tbl_PK] (
		[PK]
	)
GO

-- > Version Update
Alter VIEW dbo.ViewDBVersion
AS
SELECT DISTINCT 'V1.5.1(85/10/02)' AS Version, '85/10/02' AS PDate
FROM         dbo.Tbl_Area

-- > Create New View
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ViewName]') and OBJECTPROPERTY(id, N'IsView') = 1)
	DROP VIEW dbo.ViewName
GO

CREATE VIEW dbo.ViewName
AS
...
GO
-- > Change Field Type
ALTER TABLE TabelName
	ALTER COLUMN ColumnName [FieldType] NOT NULL
GO
-- OR --
EXECUTE sp_rename N'dbo.TableName.FieldName', N'FieldName1', 'COLUMN'
GO
ALTER TABLE TableName WITH CHECK ADD FieldName REAL NULL
GO
UPDATE TableName
SET FieldName = FieldName1
GO
ALTER TABLE TableName DROP COLUMN FieldName1
GO

-- > Delete Bad FormObjects
DELETE Tbl_FormObject WHERE FormObjectId < 0 OR FormObjectId > 1000
GO
IF NOT EXISTS(SELECT * FROM Tbl_ExecQuery WHERE ExecQueryId  = 0000)
   Insert Into Tbl_ExecQuery (ExecQueryId,Query) VALUES (0000,'DELETE Tbl_FormObject WHERE FormObjectId < 0 OR FormObjectId > 1000')
GO

-- > Insert a table into Tbl_Tablename
if not exists (select * from Tbl_TableName where TableName = 'TableName') 
  Insert Into Tbl_TableName (TableNameId,TableName,IsBroadcast,IsCenterUpdate,IsSetadUpdate,IsMiscModeUpdate,IsNahiehUpdate) 
  VALUES (xxx,N'TableName',1,Null,Null,Null,Null)
GO

-- > Change Or Add FK Constraint
if exists(SELECT * FROM sysobjects WHERE (name LIKE N'FK_Name'))
ALTER TABLE dbo.TableName
	DROP CONSTRAINT FK_Name;
GO

ALTER TABLE dbo.TableName WITH NOCHECK ADD CONSTRAINT
	FK_Name FOREIGN KEY
	(
	FK
	) REFERENCES dbo.TableNamePK
	(
	PK
	) ON UPDATE CASCADE
	 ON DELETE CASCADE;
GO

-- > Create Index
if exists(SELECT * FROM sysindexes WHERE (name = 'Index_Name')) 
	DROP INDEX dbo.TableName.Index_Name
GO

CREATE INDEX [Index_Name] ON [dbo].[TableName]([FieldName],...) ON [PRIMARY]
GO

-- > Drop Index
if exists(SELECT * FROM sysindexes WHERE (name = 'Index_Name')) 
	DROP INDEX [TableName].[Index_Name]
GO

-- > Create NonClustered Index
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[TableName]') AND name = N'Index_Name')
	CREATE NONCLUSTERED INDEX [Index_Name] ON [dbo].[TableName] ([FieldName],...)
GO

-- > Create STATISTICS
if not exists(SELECT * FROM sysindexes WHERE (name = 'st_IndexName')) 
	CREATE STATISTICS [st_IndexName] ON [dbo].[TableName] ([FieldName],...)
GO

-- > Insert/Update Form Object Record
if not exists (select * from Tbl_FormObject where FormObjectId = xx) 
	Insert Into Tbl_FormObject (FormObjectId,Tag,PersianTag,SysObjId) VALUES (xx,N'rptQuestionnaire',N'sss',yy)
ELSE
	update Tbl_FormObject Set Tag=N'rptQuestionnaire',PersianTag=N'sss',SysObjId=yy Where FormObjectId=xx
GO

-- > Create New FormObject
/*
	#tag		=> Tag
	#ptag		=> PersianTag
	#fobjid 	=> FormObjectId
	#sobjid		=> SysObjId
	#fobjappid	=> FormObjectApplicationId
	#appid		=> ApplicationId
*/
IF NOT EXISTS (SELECT * FROM Tbl_FormObject WHERE FormObjectId = #fobjid) 
	INSERT INTO Tbl_FormObject (FormObjectId,Tag,PersianTag,SysObjId) VALUES (#fobjid,N'#tag',N'#ptag',#sobjid)
ELSE
	UPDATE Tbl_FormObject SET Tag=N'#tag',PersianTag=N'#ptag',SysObjId=#sobjid WHERE FormObjectId=#fobjid
GO

IF NOT EXISTS (SELECT * FROM Tbl_FormObjectApplication WHERE FormObjectApplicationId = #fobjappid)
	INSERT Tbl_FormObjectApplication (FormObjectApplicationId,ApplicationId,FormObjectId) VALUES (#fobjappid,#appid,#fobjid)
ELSE
	UPDATE Tbl_FormObjectApplication SET FormObjectApplicationId = #fobjappid,ApplicationId = #appid,FormObjectId = #fobjid WHERE FormObjectApplicationId = #fobjappid
GO

-- > Create New User Function
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[funcName]') and OBJECTPROPERTY(id, N'IsScalarFunction') = 1)
  drop function [dbo].[funcName]
GO

CREATE FUNCTION [dbo].[GetOtherBasketDetailsQuery] (@aBazdidTypeId as int, @aNotBazdidBasketId as bigint) 
RETURNS varchar(500) AS  
BEGIN 
...
END
GO

-- OR
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[function_name]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
execute dbo.sp_executesql @statement = N'CREATE function [dbo].[function_name] ...'

-- > Add Constraint
if exists(SELECT * FROM sysobjects WHERE (name LIKE N'FK_BTblBazdidResultAddress_BTblVoice'))
ALTER TABLE dbo.BTblBazdidResultAddress
	DROP CONSTRAINT FK_BTblBazdidResultAddress_BTblVoice
GO

ALTER TABLE [BTblBazdidResultAddress] 
	WITH CHECK ADD 
	CONSTRAINT [FK_BTblBazdidResultAddress_BTblVoice] FOREIGN KEY 
	(
		[BazdidVoiceId]
	) REFERENCES [BTblVoice] (
		[VoiceId]
	)
GO

--> Add Default Constraint
IF NOT EXISTS (SELECT * FROM sysobjects WHERE (name LIKE N'DF_Name') and (OBJECTPROPERTY(id, N'IsDefaultCnst') = 1))
	ALTER TABLE [dbo].[TableName] ADD CONSTRAINT [DF_Name] DEFAULT ((DefaultValue)) FOR [ColumnName]
GO

-- > Create StoredProcedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spName]
GO

CREATE PROCEDURE [dbo].[spName] 
	@aParamNamne as varchar(10), 
	...
AS
	...
GO

-- > Create StoredProcedure : New Mode
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Homa].[spName]') AND type in (N'P', N'PC'))

-- > Create StoredProcedure : with proceduire numbner
IF NOT EXISTS (SELECT * FROM sys.numbered_procedures WHERE object_id = OBJECT_ID(N'[Homa].[spName]') AND procedure_number = x)

-- > Create UserFunction
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[FnName]') and OBJECTPROPERTY(id, N'IsScalarFunction') = 1)
  DROP FUNCTION [dbo].[FnName]
GO

CREATE FUNCTION dbo.FnName ( @aParam int )
RETURNS int AS  
BEGIN 
	declare @lRet as int
	...
	return @lRet
END
GO

-- > Create Cursor
DECLARE CursorName CURSOR FOR
	SELECT ...

	OPEN CursorName
	
	FETCH NEXT
		FROM CursorName
		INTO 
			@v1, @v2, ...

		WHILE @@FETCH_STATUS = 0
		BEGIN

			FETCH NEXT
				FROM CursorName
				INTO 
					@v1, @v2, ...
	END
CLOSE CursorName
DEALLOCATE CursorName
	
-- > Check Field Type
SELECT 
	* 
FROM 
	[CcRequesterSetad].information_schema.columns
WHERE 
	TABLE_NAME = 'TableName'
	AND COLUMN_NAME = 'FieldName'
	
-- > Insert Records into Tbl_EventLogCenter
INSERT INTO Tbl_EventLogCenter (
	TableName,
	TableNameId,
	PrimaryKeyId,
	Operation,
	AreaId,
	WorkingAreaId,
	DataEntryDT
	)
SELECT
	'TableName' AS TableName,
	xxx AS TableNameId,
	PKIdName AS PrimaryKeyId,
	3 AS Operation,
	DestAreaId AS AreaId,
	99 AS WorkingAreaId,
	GETDATE() AS DataEntryDT
FROM 
	Tbl_LPTrans
	
-- > New Section
