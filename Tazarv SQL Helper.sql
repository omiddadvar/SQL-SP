
-- Company IP : 85.15.45.235

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

-- > Check Field Type
IF EXISTS(
	SELECT COLUMN_NAME 
	FROM Information_Schema.Columns
	WHERE Table_Name = 'Tbl_EXecQuery' AND DATA_TYPE = 'ntext'
) ...
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
  Insert Into Tbl_TableName (TableName,IsBroadcast,IsCenterUpdate,IsSetadUpdate,IsMiscModeUpdate,IsNahiehUpdate) VALUES (N'TableName',1,Null,Null,Null,Null)
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

-- > Create Index Mode 2
if not exists(SELECT * FROM sysindexes WHERE (name = 'Index_Name')) 
	CREATE INDEX [Index_Name] ON [dbo].[TableName]([FieldName],...) ON [PRIMARY]
GO


-- > Drop Index
if exists(SELECT * FROM sysindexes WHERE (name = 'Index_Name')) 
	DROP INDEX [TableName].[Index_Name]
GO

-- > Create NonClustered Index
if not exists(SELECT * FROM sysindexes WHERE (name = 'Index_Name')) 
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

CREATE FUNCTION [dbo].[funcName] (@aParam1 as int, @aParam2 as bigint, ...) 
RETURNS varchar(500) AS  
BEGIN 
...
END
GO

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

-- > Create Cursor Model 1
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

-- > Create Cursor Model 2
DECLARE CursorName CURSOR FOR
	SELECT ...

	OPEN CursorName
	
	DECLARE @lIsLoop AS bit	
	SET @lIsLoop = 1
	WHILE @lIsLoop = 1
	BEGIN
		FETCH NEXT
			FROM CursorName
			INTO @v1, @v2, ...

		if @@FETCH_STATUS = 0 
		BEGIN
			...
		END
		ELSE
			SET @lIsLoop = 0
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
	
-- > Disable & Enable Primary Key on Tables
ALTER TABLE mytable DROP CONSTRAINT PK_Name
ALTER TABLE mytable ADD CONSTRAINT PK_Name PRIMARY KEY /* CLUSTERED */ (pk_column)

-- > Tbl_EventLogCenter
INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand)
SELECT 	
	'A' AS TableName,
	B AS TableNameId,
	C AS PrimaryKeyId,
	D AS Operation,
	E AS AreaId,
	F AS WorkingAreaId,
	GETDATE() AS DataEntryDT,
	'H' AS SQLCommand
FROM 
	Tbl_xxx

-- > When Database Restore
DECLARE @dbname sysname, @days int

SET @dbname = 'CcRequester'	--substitute for whatever database name you want
SET @days = -365 			--previous number of days, script will default to 30

SELECT
	rsh.destination_database_name AS [Database],
	rsh.user_name AS [Restored By],
	CASE 
		WHEN rsh.restore_type = 'D' THEN 'Database'
		WHEN rsh.restore_type = 'F' THEN 'File'
		WHEN rsh.restore_type = 'G' THEN 'Filegroup'
		WHEN rsh.restore_type = 'I' THEN 'Differential'
		WHEN rsh.restore_type = 'L' THEN 'Log'
		WHEN rsh.restore_type = 'V' THEN 'Verifyonly'
		WHEN rsh.restore_type = 'R' THEN 'Revert'
		ELSE rsh.restore_type 
	END AS [Restore Type],
	rsh.restore_date AS [Restore Started],
	bmf.physical_device_name AS [Restored From], 
	rf.destination_phys_name AS [Restored To]
FROM 
	msdb.dbo.restorehistory rsh
	INNER JOIN msdb.dbo.backupset bs ON rsh.backup_set_id = bs.backup_set_id
	INNER JOIN msdb.dbo.restorefile rf ON rsh.restore_history_id = rf.restore_history_id
	INNER JOIN msdb.dbo.backupmediafamily bmf ON bmf.media_set_id = bs.media_set_id
WHERE 
	rsh.restore_date >= DATEADD(dd, ISNULL(@days, -30), GETDATE()) --want to search for previous days
	AND destination_database_name = ISNULL(@dbname, destination_database_name) --if no dbname, then return all
ORDER BY 
	rsh.restore_history_id DESC
--

-- > Update TblRequest.IsSingleSubscriber

UPDATE 
	TblRequest
SET 
	IsSingleSubscriber = TblLPRequest.IsSingleSubscriber
FROM 
	TblRequest
	INNER JOIN TblLPRequest ON TblRequest.LPRequestId = TblLPRequest.LPRequestId
	AND TblRequest.IsSingleSubscriber <> TblLPRequest.IsSingleSubscriber
WHERE 
	TblRequest.DisconnectDatePersian >= '1395/01/01'
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) 
VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'')

--UPDATE TblRequest SET IsSingleSubscriber = TblLPRequest.IsSingleSubscriber FROM TblRequest INNER JOIN TblLPRequest ON TblRequest.LPRequestId = TblLPRequest.LPRequestId AND TblRequest.IsSingleSubscriber <> TblLPRequest.IsSingleSubscriber WHERE TblRequest.DisconnectDatePersian >= ''1395/01/01''
GO

-----

---------------------- Server Info Query -------------------------
select Subject, InfoName from ( select N'Province' As Subject, Province As InfoName, 1 as Radif from Tbl_Province union select N'Area' As Subject, t2.Area As InfoName, 3 as Radif from TblDepartmentInfo t1 inner join Tbl_Area t2 on t1.WorkingAreaId = t2.AreaId union select N'DB Version' As Subject, PDate As InfoName, 3 as Radif from ViewDBVersion union SELECT N'End Garantee' As Subject, Cast( (C ^ 78007800) As varchar) As InfoName, 4 as Radif FROM tbl_HavadesInfo union SELECT Replace(Replace(Replace(FileName,'SetadPatch_V4.EXE',' Version'),'bargh','Havades'),'SetadServicePatch.EXE','Service Version') As 'Subject', Version As InfoName, 5 as Radif FROM Tbl_Patch WHERE FileName LIKE N'%setad%' GROUP BY FileName, Version ) ViewInfo order by Radif, Subject


-- Create SMS in Tbl_SMS
INSERT INTO [Tbl_SMS]
           ([SMSId],[AreaId],[Body],[IsSend],[IsSMS],[IsEmail],[IsFax],[Mobile],[SMSDesc],[CreateDate],[CreateDTPersian],[CreateTime])
     VALUES
           (1,3,'test',0,1,0,0,'09111402456','test',getdate(),'1396/01/29','09:56')
GO


------------------------ SQL Server 2008 ----------------------------
-- Row_Number :

ROW_NUMBER ( )   
    OVER ( [ PARTITION BY value_expression , ... [ n ] ] order_by_clause )  
	
example :
SELECT 
  ROW_NUMBER() OVER(PARTITION BY recovery_model_desc ORDER BY name ASC) 
    AS Row#,
  name, recovery_model_desc
FROM sys.databases WHERE database_id < 5



------------------------ Table Space Query ----------------------------
SELECT 
    t.NAME AS TableName,
    s.Name AS SchemaName,
    p.rows AS RowCounts,
    SUM(a.total_pages) * 8 AS TotalSpaceKB, 
    CAST(ROUND(((SUM(a.total_pages) * 8) / 1024.00), 2) AS NUMERIC(36, 2)) AS TotalSpaceMB,
    SUM(a.used_pages) * 8 AS UsedSpaceKB, 
    CAST(ROUND(((SUM(a.used_pages) * 8) / 1024.00), 2) AS NUMERIC(36, 2)) AS UsedSpaceMB, 
    (SUM(a.total_pages) - SUM(a.used_pages)) * 8 AS UnusedSpaceKB,
    CAST(ROUND(((SUM(a.total_pages) - SUM(a.used_pages)) * 8) / 1024.00, 2) AS NUMERIC(36, 2)) AS UnusedSpaceMB
FROM 
    sys.tables t
INNER JOIN      
    sys.indexes i ON t.OBJECT_ID = i.object_id
INNER JOIN 
    sys.partitions p ON i.object_id = p.OBJECT_ID AND i.index_id = p.index_id
INNER JOIN 
    sys.allocation_units a ON p.partition_id = a.container_id
LEFT OUTER JOIN 
    sys.schemas s ON t.schema_id = s.schema_id
WHERE 
    t.NAME NOT LIKE 'dt%' 
    AND t.is_ms_shipped = 0
    AND i.OBJECT_ID > 255 
GROUP BY 
    t.Name, s.Name, p.Rows
ORDER BY 
SUM(a.total_pages) * 8 desc



------------------------ Table Space Best Query ----------------------------
set nocount on

print 'Show Size, Space Used, Unused Space, Type, and Name of all database files'

select
	[FileSizeMB]	=
		convert(numeric(10,2),sum(round(a.size/128.,2))),
        [UsedSpaceMB]	=
		convert(numeric(10,2),sum(round(fileproperty( a.name,'SpaceUsed')/128.,2))) ,
        [UnusedSpaceMB]	=
		convert(numeric(10,2),sum(round((a.size-fileproperty( a.name,'SpaceUsed'))/128.,2))) ,
	[Type] =
		case when a.groupid is null then '' when a.groupid = 0 then 'Log' else 'Data' end,
	[DBFileName]	= isnull(a.name,'*** Total for all files ***')
from
	sysfiles a
group by
	groupid,
	a.name
	with rollup
having
	a.groupid is null or
	a.name is not null
order by
	case when a.groupid is null then 99 when a.groupid = 0 then 0 else 1 end,
	a.groupid,
	case when a.name is null then 99 else 0 end,
	a.name




create table #TABLE_SPACE_WORK
(
	TABLE_NAME 	sysname		not null ,
	TABLE_ROWS 	numeric(18,0)	not null ,
	RESERVED 	varchar(50) 	not null ,
	DATA 		varchar(50) 	not null ,
	INDEX_SIZE 	varchar(50) 	not null ,
	UNUSED 		varchar(50) 	not null ,
)

create table #TABLE_SPACE_USED
(
	Seq		int		not null	
	identity(1,1)	primary key clustered,
	TABLE_NAME 	sysname		not null ,
	TABLE_ROWS 	numeric(18,0)	not null ,
	RESERVED 	varchar(50) 	not null ,
	DATA 		varchar(50) 	not null ,
	INDEX_SIZE 	varchar(50) 	not null ,
	UNUSED 		varchar(50) 	not null ,
)

create table #TABLE_SPACE
(
	Seq		int		not null
	identity(1,1)	primary key clustered,
	TABLE_NAME 	SYSNAME 	not null ,
	TABLE_ROWS 	int	 	not null ,
	RESERVED 	int	 	not null ,
	DATA 		int	 	not null ,
	INDEX_SIZE 	int	 	not null ,
	UNUSED 		int	 	not null ,
	USED_MB				numeric(18,4)	not null,
	USED_GB				numeric(18,4)	not null,
	AVERAGE_BYTES_PER_ROW		numeric(18,5)	null,
	AVERAGE_DATA_BYTES_PER_ROW	numeric(18,5)	null,
	AVERAGE_INDEX_BYTES_PER_ROW	numeric(18,5)	null,
	AVERAGE_UNUSED_BYTES_PER_ROW	numeric(18,5)	null,
)

declare @fetch_status int

declare @proc 	varchar(200)
select	@proc	= rtrim(db_name())+'.dbo.sp_spaceused'

declare Cur_Cursor cursor local
for
select
	TABLE_NAME	= 
	rtrim(TABLE_SCHEMA)+'.'+rtrim(TABLE_NAME)
from
	INFORMATION_SCHEMA.TABLES 
where
	TABLE_TYPE	= 'BASE TABLE'
order by
	1

open Cur_Cursor

declare @TABLE_NAME 	varchar(200)

select @fetch_status = 0

while @fetch_status = 0
	begin

	fetch next from Cur_Cursor
	into
		@TABLE_NAME

	select @fetch_status = @@fetch_status

	if @fetch_status <> 0
		begin
		continue
		end

	truncate table #TABLE_SPACE_WORK

	insert into #TABLE_SPACE_WORK
		(
		TABLE_NAME,
		TABLE_ROWS,
		RESERVED,
		DATA,
		INDEX_SIZE,
		UNUSED
		)
	exec @proc @objname = 
		@TABLE_NAME ,@updateusage = 'true'


	-- Needed to work with SQL 7
	update #TABLE_SPACE_WORK
	set
		TABLE_NAME = @TABLE_NAME

	insert into #TABLE_SPACE_USED
		(
		TABLE_NAME,
		TABLE_ROWS,
		RESERVED,
		DATA,
		INDEX_SIZE,
		UNUSED
		)
	select
		TABLE_NAME,
		TABLE_ROWS,
		RESERVED,
		DATA,
		INDEX_SIZE,
		UNUSED
	from
		#TABLE_SPACE_WORK

	end 	--While end

close Cur_Cursor

deallocate Cur_Cursor

insert into #TABLE_SPACE
	(
	TABLE_NAME,
	TABLE_ROWS,
	RESERVED,
	DATA,
	INDEX_SIZE,
	UNUSED,
	USED_MB,
	USED_GB,
	AVERAGE_BYTES_PER_ROW,
	AVERAGE_DATA_BYTES_PER_ROW,
	AVERAGE_INDEX_BYTES_PER_ROW,
	AVERAGE_UNUSED_BYTES_PER_ROW

	)
select
	TABLE_NAME,
	TABLE_ROWS,
	RESERVED,
	DATA,
	INDEX_SIZE,
	UNUSED,
	USED_MB			=
		round(convert(numeric(25,10),RESERVED)/
		convert(numeric(25,10),1024),4),
	USED_GB			=
		round(convert(numeric(25,10),RESERVED)/
		convert(numeric(25,10),1024*1024),4),
	AVERAGE_BYTES_PER_ROW	=
		case
		when TABLE_ROWS <> 0
		then round(
		(1024.000000*convert(numeric(25,10),RESERVED))/
		convert(numeric(25,10),TABLE_ROWS),5)
		else null
		end,
	AVERAGE_DATA_BYTES_PER_ROW	=
		case
		when TABLE_ROWS <> 0
		then round(
		(1024.000000*convert(numeric(25,10),DATA))/
		convert(numeric(25,10),TABLE_ROWS),5)
		else null
		end,
	AVERAGE_INDEX_BYTES_PER_ROW	=
		case
		when TABLE_ROWS <> 0
		then round(
		(1024.000000*convert(numeric(25,10),INDEX_SIZE))/
		convert(numeric(25,10),TABLE_ROWS),5)
		else null
		end,
	AVERAGE_UNUSED_BYTES_PER_ROW	=
		case
		when TABLE_ROWS <> 0
		then round(
		(1024.000000*convert(numeric(25,10),UNUSED))/
		convert(numeric(25,10),TABLE_ROWS),5)
		else null
		end
from
	(
	select
		TABLE_NAME,
		TABLE_ROWS,
		RESERVED	= 
		convert(int,rtrim(replace(RESERVED,'KB',''))),
		DATA		= 
		convert(int,rtrim(replace(DATA,'KB',''))),
		INDEX_SIZE	= 
		convert(int,rtrim(replace(INDEX_SIZE,'KB',''))),
		UNUSED		= 
		convert(int,rtrim(replace(UNUSED,'KB','')))
	from
		#TABLE_SPACE_USED aa
	) a
order by
	TABLE_NAME

print 'Show results in descending order by size in MB'

--select * from #TABLE_SPACE order by USED_MB desc
select * from #TABLE_SPACE order by Table_Rows desc
go

drop table #TABLE_SPACE_WORK
drop table #TABLE_SPACE_USED 
drop table #TABLE_SPACE

GO 

------------------------ Table INDEX STATS Query ----------------------------
SELECT OBJECT_NAME(A.[OBJECT_ID]) AS [OBJECT NAME], 
       I.[NAME] AS [INDEX NAME], 
       A.LEAF_INSERT_COUNT, 
       A.LEAF_UPDATE_COUNT, 
       A.LEAF_DELETE_COUNT ,
       A.range_scan_count       
FROM   SYS.DM_DB_INDEX_OPERATIONAL_STATS (NULL,NULL,NULL,NULL ) A 
       INNER JOIN SYS.INDEXES AS I 
         ON I.[OBJECT_ID] = A.[OBJECT_ID] 
            AND I.INDEX_ID = A.INDEX_ID 
WHERE  OBJECTPROPERTY(A.[OBJECT_ID],'IsUserTable') = 1
and OBJECT_NAME(A.[OBJECT_ID]) ='tblrequest'
GO

------------------------ Find Text in StoredProcedure ----------------------------
SELECT DISTINCT
   o.name AS Object_Name,
   o.type_desc
FROM sys.sql_modules m
   INNER JOIN
   sys.objects o
	 ON m.object_id = o.object_id
WHERE m.definition Like '%ABD%'




update TblRequest set SubscriberName = N'ËÈÊ ÎæÏ˜ÇÑ', Address = N'ËÈÊ ÔÏå ÏÑ ÕäÏæÞ ÕæÊí' where DisconnectDatePersian > '1399/10/01' and AreaUserId = 1004 and ISNULL(SubscriberName,'') = ''
GO
INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) 
VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'update TblRequest set SubscriberName = N''ËÈÊ ÎæÏ˜ÇÑ'', Address = N''ËÈÊ ÔÏå ÏÑ ÕäÏæÞ ÕæÊí'' where DisconnectDatePersian > ''1399/10/01'' and AreaUserId = 1004 and ISNULL(SubscriberName,'''') = ''''')
GO


