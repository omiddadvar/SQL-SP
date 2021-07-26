/* ----------------------- */
/*  	SMS Project		   */
/* ----------------------- */

Use [CcRequesterSetad]
GO

/* ----------------------- */
/*  	SET VERSION		   */
/* ----------------------- */

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[ViewSMSVersion]') AND OBJECTPROPERTY(id, N'IsView') = 1)
	DROP VIEW dbo.ViewSMSVersion
GO

CREATE VIEW [dbo].[ViewSMSVersion]
AS
	SELECT 'V2.9.18' AS Version, '1400/04/23' AS PDate
GO

if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TblManagerSMSFTMPFeederDCSended]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	CREATE TABLE [dbo].[TblManagerSMSFTMPFeederDCSended](
		[ManagerSMSFTMPFeederDCSendedId] [int] IDENTITY(1,1) NOT NULL,
		[ManagerSMSDCId] [int] NULL,
		[FogheToziDisconnectMPFeederId] [int] NULL,
		[IsConnected] [bit] NULL,
	 CONSTRAINT [PK_Tbl_ManagerSMSFTMPFeederDCSended] PRIMARY KEY CLUSTERED 
	(
		[ManagerSMSFTMPFeederDCSendedId] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO

IF NOT EXISTS(SELECT * FROM sysobjects INNER JOIN  syscolumns ON sysobjects.id = syscolumns.id WHERE (sysobjects.id = OBJECT_ID(N'TblManagerSMSDC')) AND (syscolumns.name = 'IsSerghat'))
	ALTER TABLE TblManagerSMSDC WITH CHECK ADD [IsSerghat] [bit] NULL
GO 

IF NOT EXISTS(SELECT * FROM sysobjects INNER JOIN  syscolumns ON sysobjects.id = syscolumns.id WHERE (sysobjects.id = OBJECT_ID(N'TblManagerSMSDCSended')) AND (syscolumns.name = 'IsSendSMSSerghat'))
	ALTER TABLE TblManagerSMSDCSended WITH CHECK ADD [IsSendSMSSerghat] [bit] NULL
GO 

IF NOT EXISTS(SELECT * FROM sysobjects INNER JOIN  syscolumns ON sysobjects.id = syscolumns.id WHERE (sysobjects.id = OBJECT_ID(N'TblManagerSMSCriticalMPF')) AND (syscolumns.name = 'SMSType'))
	ALTER TABLE TblManagerSMSCriticalMPF WITH CHECK ADD
	[SMSType] [tinyint] NOT NULL CONSTRAINT [DF_TblManagerSMSCriticalMPF_SMSType] DEFAULT (1)
GO 

IF NOT EXISTS(SELECT * FROM sysobjects INNER JOIN  syscolumns ON sysobjects.id = syscolumns.id WHERE (sysobjects.id = OBJECT_ID(N'TblManagerSMSDCSended')) AND (syscolumns.name = 'RequestTypeId'))
	ALTER TABLE TblManagerSMSDCSended WITH CHECK ADD [RequestTypeId] [int] NULL
GO 

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[TblManagerSMSCriticalMPF]') AND name = N'PK_TblManagerSMSCriticalMPF')
	ALTER TABLE [dbo].[TblManagerSMSCriticalMPF] DROP CONSTRAINT [PK_TblManagerSMSCriticalMPF]
GO

if not exists(SELECT * FROM sysindexes WHERE (name = 'PK_TblManagerSMSCriticalMPF_1')) 
	ALTER TABLE [dbo].[TblManagerSMSCriticalMPF] ADD  CONSTRAINT [PK_TblManagerSMSCriticalMPF_1] PRIMARY KEY CLUSTERED 
	(
		[ManagerSMSDCId] ASC,
		[SMSType] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TblTamirRequestSendAlarmForPeymankar]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
CREATE TABLE [dbo].[TblTamirRequestSendAlarmForPeymankar](
	[TamirRequestSendAlarmForPeymankarId] [bigint] IDENTITY(1,1) NOT NULL,
	[TamirRequestId] [bigint] NOT NULL,
	[PeymankarMobileNo] [nvarchar](20) COLLATE Arabic_CI_AS_WS NULL,
	[IsSendSMSAlarm] [bit] NULL,
	[IsSendSMSConfirm] [bit] NULL,
	CONSTRAINT [PK_TblTamirRequestSendAlarmForPeymankar] PRIMARY KEY CLUSTERED 
	(
		[TamirRequestSendAlarmForPeymankarId] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
GO

if not exists(SELECT * FROM sysindexes WHERE (name = 'Idx_TblTamirRequestSendAlarmForPeymankar_TamirRequestId')) 
	CREATE NONCLUSTERED INDEX [Idx_TblTamirRequestSendAlarmForPeymankar_TamirRequestId] ON [dbo].[TblTamirRequestSendAlarmForPeymankar] 
	(
		[TamirRequestId] ASC
	) ON [PRIMARY]
GO

if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TblManagerSMSTamirReminderSended]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
CREATE TABLE [dbo].[TblManagerSMSTamirReminderSended](
	[ManagerSMSTamirReminderSendedId] [bigint] IDENTITY(1,1) NOT NULL,
	[TamirRequestId] [bigint] NULL,
	[ManagerSMSDCId] [int] NULL,
 CONSTRAINT [PK_TblManagerSMSTamirReminderSended] PRIMARY KEY CLUSTERED 
(
	[ManagerSMSTamirReminderSendedId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

if not exists(SELECT * FROM sysindexes WHERE (name = 'IX_TblManagerSMSTamirReminderSended_ManagerSMSDCId')) 
	CREATE NONCLUSTERED INDEX [IX_TblManagerSMSTamirReminderSended_ManagerSMSDCId] ON [dbo].[TblManagerSMSTamirReminderSended] 
	(
		[ManagerSMSDCId] ASC
	) ON [PRIMARY]
GO

if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TblManagerSMSTamirReturnSended]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
CREATE TABLE [dbo].[TblManagerSMSTamirReturnSended](
	[ManagerSMSTamirReturnSended] [bigint] IDENTITY(1,1) NOT NULL,
	[TamirRequestId] [bigint] NULL,
	[ManagerSMSDCId] [int] NULL,
 CONSTRAINT [PK_TblManagerSMSTamirReturnSended] PRIMARY KEY CLUSTERED 
(
	[ManagerSMSTamirReturnSended] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

if not exists(SELECT * FROM sysindexes WHERE (name = 'IX_TblManagerSMSTamirReturnSended_ManagerSMSDCId')) 
	CREATE NONCLUSTERED INDEX [IX_TblManagerSMSTamirReturnSended_ManagerSMSDCId] ON [dbo].[TblManagerSMSTamirReturnSended] 
	(
		[ManagerSMSDCId] ASC
	) ON [PRIMARY]
GO

if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TblManagerSMSTamirLongSended]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
CREATE TABLE [dbo].[TblManagerSMSTamirLongSended](
	[ManagerSMSTamirLongSendedId] [bigint] IDENTITY(1,1) NOT NULL,
	[RequestId] [bigint] NULL,
	[ManagerSMSDCId] [int] NULL,
 CONSTRAINT [PK_TblManagerSMSTamirLongSended] PRIMARY KEY CLUSTERED 
(
	[ManagerSMSTamirLongSendedId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

if not exists(SELECT * FROM sysindexes WHERE (name = 'IX_TblManagerSMSTamirLongSended_ManagerSMSDCId')) 
	CREATE NONCLUSTERED INDEX [IX_TblManagerSMSTamirLongSended_ManagerSMSDCId] ON [dbo].[TblManagerSMSTamirLongSended] 
	(
		[ManagerSMSDCId] ASC
	) ON [PRIMARY]
GO




ALTER PROCEDURE [dbo].[spUpdateSendSMSStatus] 
	@aSubscriberInformId BIGINT,
	@aSendSMSStatusId INT,
	@IsSendSMSAfterConnect Bit,
	@aAreaId INT
AS
	DECLARE @TableNameId INT;
	DECLARE @Query VARCHAR(100);

	SET @TableNameId = 
	(
		SELECT TableNameId
		FROM Tbl_TableName
		WHERE TableName = 'TblSubscriberInfom'
	);

	UPDATE 
		TblSubscriberInfom
	SET 
		SendSMSDT = getDate(),
		SendSMSStatusId = @aSendSMSStatusId,
		IsSendSMSAfterConnect = @IsSendSMSAfterConnect
	WHERE 
		SubscriberInformId = CAST(@aSubscriberInformId AS VARCHAR);

	INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand)
	VALUES ('TblSubscriberInfom', @TableNameId, @aSubscriberInformId, 2, @aAreaId, 99, GETDATE(), NULL)

	SELECT TOP 0 *
	FROM TblSubscriberInfom
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spUpdateSendEmailStatus]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spUpdateSendEmailStatus]
GO
CREATE PROCEDURE [dbo].[spUpdateSendEmailStatus] 
	@aSubscriberInformId BIGINT,
	@aSendEmailStatusId INT,
	@IsSendSMSAfterConnect Bit,
	@aAreaId INT
AS
	DECLARE @TableNameId INT;
	DECLARE @Query VARCHAR(100);

	SET @TableNameId = 
	(
		SELECT TableNameId
		FROM Tbl_TableName
		WHERE TableName = 'TblSubscriberInfom'
	);

	UPDATE 
		TblSubscriberInfom
	SET 
		SendEmailDT = getDate(),
		SendEmailStatusId = @aSendEmailStatusId,
		IsSendSMSAfterConnect = @IsSendSMSAfterConnect
	WHERE 
		SubscriberInformId = @aSubscriberInformId;

	INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand)
	VALUES ('TblSubscriberInfom', @TableNameId, @aSubscriberInformId, 2, @aAreaId, 99, GETDATE(), NULL)

	SELECT TOP 0 *
	FROM TblSubscriberInfom

GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spUpdateOutgoingCallStatus]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spUpdateOutgoingCallStatus]
GO
CREATE PROCEDURE [dbo].[spUpdateOutgoingCallStatus] 
	@aSubscriberInformId BIGINT,
	@aOutgoingCallStatusId INT,
	@IsSendSMSAfterConnect Bit,
	@aAreaId INT
AS
	DECLARE @TableNameId INT;
	DECLARE @Query VARCHAR(100);

	SET @TableNameId = 
	(
		SELECT TableNameId
		FROM Tbl_TableName
		WHERE TableName = 'TblSubscriberInfom'
	);

	UPDATE 
		TblSubscriberInfom
	SET 
		SendSMSDT = getDate(),
		OutgoingCallStatusId = @aOutgoingCallStatusId,
		IsSendSMSAfterConnect = @IsSendSMSAfterConnect
	WHERE 
		SubscriberInformId = @aSubscriberInformId;

	INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand)
	VALUES ('TblSubscriberInfom', @TableNameId, @aSubscriberInformId, 2, @aAreaId, 99, GETDATE(), NULL)

	SELECT TOP 0 *
	FROM TblSubscriberInfom

GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spUpdateSendFaxStatus]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spUpdateSendFaxStatus]
GO
CREATE PROCEDURE [dbo].[spUpdateSendFaxStatus] 
	@aSubscriberInformId BIGINT,
	@aSendFaxStatusId INT,
	@IsSendSMSAfterConnect Bit,
	@aAreaId INT
AS
	DECLARE @TableNameId INT;
	DECLARE @Query VARCHAR(100);

	SET @TableNameId = 
	(
		SELECT TableNameId
		FROM Tbl_TableName
		WHERE TableName = 'TblSubscriberInfom'
	);

	UPDATE 
		TblSubscriberInfom
	SET 
		SendFaxDT = getDate(),
		SendFaxStatusId = @aSendFaxStatusId,
		IsSendSMSAfterConnect = @IsSendSMSAfterConnect
	WHERE 
		SubscriberInformId = @aSubscriberInformId;

	INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand)
	VALUES ('TblSubscriberInfom', @TableNameId, @aSubscriberInformId, 2, @aAreaId, 99, GETDATE(), NULL)

	SELECT TOP 0 *
	FROM TblSubscriberInfom

GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spSendFax]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spSendFax]
GO
CREATE PROCEDURE [dbo].[spSendFax] (
	@Body AS NVARCHAR(2000)
	,@Fax AS VARCHAR(100)
	,@Desc AS NVARCHAR(50)
	,@DCManagerAreaId AS int
	,@SubscriberInformId AS bigint
	)
AS
BEGIN
	IF @Body IS NULL Return
	DECLARE @WorkingAreaId BIGINT
	DECLARE @MaxId BIGINT
	DECLARE @TableNameId INT

	SELECT @TableNameId = TableNameId
	FROM Tbl_TableName
	WHERE TableName = 'Tbl_SMS'

	SELECT TOP 1 @WorkingAreaId = WorkingAreaID
	FROM TblDepartmentInfo
	
	IF ISNULL(@Fax,'') = '' RETURN

	SELECT @MaxId = max(SMSId)
	FROM Tbl_SMS
	WHERE AreaId = @WorkingAreaId

	IF (@MaxId IS NULL)
	BEGIN
		SET @MaxId = @WorkingAreaId * 100000000 + 1
	END
	ELSE
	BEGIN
		SET @MaxId = @MaxId + 1
	END

	INSERT INTO Tbl_SMS (
		SMSId
		,AreaId
		,Body
		,IsSend
		,IsSMS
		,IsFax
		,IsEmail
		,Phone
		,SMSDesc
		,CreateDate
		,CreateDTPersian
		,CreateTime
		,DCManagerAreaId
		,SubscriberInformId
		)
	VALUES (
		@MaxId
		,@WorkingAreaId
		,@Body
		,0
		,0
		,1
		,0
		,@Fax
		,@Desc
		,GetDate()
		,dbo.mtosh(GetDate())
		,dbo.GetTime(GetDate())
		,@DCManagerAreaId
		,@SubscriberInformId
		)

	IF (
			dbo.IsSlaveServer() = 0
			AND LOWER(db_name()) <> 'ccrequestersetad'
			)
	BEGIN
		INSERT INTO Tbl_EventLogCenter (
			TableName
			,TableNameId
			,PrimaryKeyId
			,Operation
			,AreaId
			,WorkingAreaId
			,DataEntryDT
			)
		VALUES (
			'Tbl_SMS'
			,@TableNameId
			,@MaxId
			,2
			,@WorkingAreaId
			,@WorkingAreaId
			,getDate()
			)
	END
	SELECT @MaxId AS SMSId
END
GO


IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spSendEmail]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spSendEmail]
GO
CREATE PROCEDURE [dbo].[spSendEmail] (
	@Body AS NVARCHAR(2000)
	,@Email AS VARCHAR(100)
	,@Desc AS NVARCHAR(50)
	,@DCManagerAreaId AS int
	,@SubscriberInformId AS bigint
	)
AS
BEGIN
	IF @Body IS NULL Return
	DECLARE @WorkingAreaId BIGINT
	DECLARE @MaxId BIGINT
	DECLARE @TableNameId INT

	SELECT @TableNameId = TableNameId
	FROM Tbl_TableName
	WHERE TableName = 'Tbl_SMS'

	SELECT TOP 1 @WorkingAreaId = WorkingAreaID
	FROM TblDepartmentInfo
	
	IF ISNULL(@Email,'') = '' RETURN

	SELECT @MaxId = max(SMSId)
	FROM Tbl_SMS
	WHERE AreaId = @WorkingAreaId

	IF (@MaxId IS NULL)
	BEGIN
		SET @MaxId = @WorkingAreaId * 100000000 + 1
	END
	ELSE
	BEGIN
		SET @MaxId = @MaxId + 1
	END

	INSERT INTO Tbl_SMS (
		SMSId
		,AreaId
		,Body
		,IsSend
		,IsSMS
		,IsEmail
		,IsFax
		,EMail
		,SMSDesc
		,CreateDate
		,CreateDTPersian
		,CreateTime
		,DCManagerAreaId
		,SubscriberInformId
		,Subject
		)
	VALUES (
		@MaxId
		,@WorkingAreaId
		,@Body
		,0
		,0
		,1
		,0
		,@Email
		,@Desc
		,GetDate()
		,dbo.mtosh(GetDate())
		,dbo.GetTime(GetDate())
		,@DCManagerAreaId
		,@SubscriberInformId
		,N'«ÿ·«⁄ —”«‰Ì Œ«„Ê‘Ì'
		)

	IF (
			dbo.IsSlaveServer() = 0
			AND LOWER(db_name()) <> 'ccrequestersetad'
			)
	BEGIN
		INSERT INTO Tbl_EventLogCenter (
			TableName
			,TableNameId
			,PrimaryKeyId
			,Operation
			,AreaId
			,WorkingAreaId
			,DataEntryDT
			)
		VALUES (
			'Tbl_SMS'
			,@TableNameId
			,@MaxId
			,2
			,@WorkingAreaId
			,@WorkingAreaId
			,getDate()
			)
	END
	SELECT @MaxId AS SMSId
END
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spSendCall]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spSendCall]
GO
CREATE PROCEDURE [dbo].[spSendCall] (
	@Body AS NVARCHAR(2000)
	,@Telephone AS VARCHAR(100)
	,@Desc AS NVARCHAR(50)
	,@DCManagerAreaId AS int
	,@SubscriberInformId AS bigint
	)
AS
BEGIN
	IF @Body IS NULL Return
	DECLARE @WorkingAreaId BIGINT
	DECLARE @MaxId BIGINT
	DECLARE @TableNameId INT

	SELECT @TableNameId = TableNameId
	FROM Tbl_TableName
	WHERE TableName = 'Tbl_SMS'

	SELECT TOP 1 @WorkingAreaId = WorkingAreaID
	FROM TblDepartmentInfo
	
	IF ISNULL(@Telephone,'') = '' RETURN

	SELECT @MaxId = max(SMSId)
	FROM Tbl_SMS
	WHERE AreaId = @WorkingAreaId

	IF (@MaxId IS NULL)
	BEGIN
		SET @MaxId = @WorkingAreaId * 100000000 + 1
	END
	ELSE
	BEGIN
		SET @MaxId = @MaxId + 1
	END

	INSERT INTO Tbl_SMS (
		SMSId
		,AreaId
		,Body
		,IsSend
		,IsSMS
		,IsFax
		,IsEmail
		,Phone
		,SMSDesc
		,CreateDate
		,CreateDTPersian
		,CreateTime
		,DCManagerAreaId
		,SubscriberInformId
		,IsCall
		)
	VALUES (
		@MaxId
		,@WorkingAreaId
		,@Body
		,0
		,0
		,0
		,0
		,@Telephone
		,@Desc
		,GetDate()
		,dbo.mtosh(GetDate())
		,dbo.GetTime(GetDate())
		,@DCManagerAreaId
		,@SubscriberInformId
		,1
		)

	IF (
			dbo.IsSlaveServer() = 0
			AND LOWER(db_name()) <> 'ccrequestersetad'
			)
	BEGIN
		INSERT INTO Tbl_EventLogCenter (
			TableName
			,TableNameId
			,PrimaryKeyId
			,Operation
			,AreaId
			,WorkingAreaId
			,DataEntryDT
			)
		VALUES (
			'Tbl_SMS'
			,@TableNameId
			,@MaxId
			,2
			,@WorkingAreaId
			,@WorkingAreaId
			,getDate()
			)
	END
	SELECT @MaxId AS SMSId
END
GO


/*----------------*/
ALTER VIEW VIEWTransFaultSendSMSList
AS
SELECT 
	TblManagerSMSDC.IsTransFaultSendSMS,
	TblManagerSMSDC.ManagerSMSDCId,
	TblManagerSMSDC.ManagerName,
	TblManagerSMSDC.ManagerMobile,
	VIEWTransFaultSendSMS.RequestId,
	VIEWTransFaultSendSMS.DisconnectGroupSet,
	VIEWTransFaultSendSMS.DisconnectGroup,
	VIEWTransFaultSendSMS.DisconnectDatePersian,
	VIEWTransFaultSendSMS.DisconnectTime,
	VIEWTransFaultSendSMS.DisconnectInterval,
	VIEWTransFaultSendSMS.AreaId,
	VIEWTransFaultSendSMS.Area,
	VIEWTransFaultSendSMS.RequestNumber,
	VIEWTransFaultSendSMS.LPPostName,
	VIEWTransFaultSendSMS.Address,
	VIEWTransFaultSendSMS.DisconnectPower,
	VIEWTransFaultSendSMS.PostCapacity,
	VIEWTransFaultSendSMS.LPPostWorkingState,
	VIEWTransFaultSendSMS.Ownership,
	TblManagerSMSDC.AreaId AS ManagerAreaId
FROM 
	TblManagerSMSDC
	INNER JOIN VIEWTransFaultSendSMS ON 
		TblManagerSMSDC.AreaId = VIEWTransFaultSendSMS.AreaId
		OR TblManagerSMSDC.AreaId = 99
WHERE 
	TblManagerSMSDC.IsTransFaultSendSMS = 1
	AND NOT (CAST(TblManagerSMSDC.ManagerSMSDCId AS VARCHAR) + CAST(VIEWTransFaultSendSMS.RequestId AS VARCHAR)) IN 
	(
		SELECT cast(TblManagerSMSDC.ManagerSMSDCId AS VARCHAR) + cast(VIEWTransFaultSendSMS.RequestId AS VARCHAR)
		FROM TblManagerSMSDC
		INNER JOIN TblManagerSMSDCSended ON TblManagerSMSDC.ManagerSMSDCId = TblManagerSMSDCSended.ManagerSMSDCId
		INNER JOIN VIEWTransFaultSendSMS ON TblManagerSMSDCSended.RequestId = VIEWTransFaultSendSMS.RequestId
		
	)
GO
/*----------------*/

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[GetMultiStepConnections]') and OBJECTPROPERTY(id, N'IsScalarFunction') = 1)
  DROP FUNCTION [dbo].[GetMultiStepConnections]
GO
CREATE FUNCTION GetMultiStepConnections (@RequestId as bigint)
	RETURNS nvarchar(2000)
AS
BEGIN
	DECLARE @Result AS NVARCHAR(2000)
	DECLARE @Final AS NVARCHAR(2000)
	DECLARE @SMS AS nvarchar(2000)
	DECLARE @ConfigName AS nvarchar(100)
	SET @Result = ''
	SET @Final = ''
	SET @SMS = ''
	SET @ConfigName = ''
	
	SELECT 
		@ConfigName =
		CASE 
			WHEN tReq.IsMPRequest = 1 THEN 'SMSMultiStepConnectionMP'
			WHEN tReq.IsLPRequest = 1 THEN 'SMSMultiStepConnectionLP'
			WHEN tReq.IsFogheToziRequest = 1 THEN 'SMSMultiStepConnectionFT'
		END
	FROM TblRequest tReq
	LEFT JOIN TblMPRequest tMPR ON tReq.MPRequestId = tMPR.MPRequestId
	LEFT JOIN TblLPRequest tLPR ON tReq.LPRequestId = tLPR.LPRequestId
	LEFT JOIN TblFogheToziDisconnect tFTR ON TReq.FogheToziDisconnectId = tFTR.FogheToziDisconnectId
	INNER JOIN TblMultistepConnection tMSC ON tMPR.MPRequestId = tMSC.MPRequestId
		OR tFTR.FogheToziDisconnectId = tMSC.FogheToziDisconnectId
		OR tLPR.LPRequestId = tMSC.LPRequestId
	LEFT JOIN Tbl_MPFeeder ON tMSC.SourceMPFeederId = Tbl_MPFeeder.MPFeederId
	WHERE ISNULL(tMSC.IsNotConnect, 0) = 0
		AND tReq.RequestId = @RequestId
		
	SELECT @SMS = ConfigText FROM Tbl_Config WHERE ConfigName = @ConfigName

	DECLARE @ConnectType AS nvarchar(200)
	DECLARE @ConnectDatePersian AS nvarchar(10)
	DECLARE @ConnectTime AS nvarchar(5)
	DECLARE @CurrentValue AS nvarchar(50)
	DECLARE @ConnectDarsad AS nvarchar(100)
	DECLARE @Comments AS nvarchar(250)
	DECLARE @MPFeederName AS nvarchar(500)

	DECLARE @lIsLoop AS bit	
	
	DECLARE crMakeList CURSOR FOR
		SELECT 
			ConnectType = CASE WHEN IsManoeuvre = 1 THEN 'Ê’· »« „«‰Ê—:' ELSE 'Ê’· „Ê›ﬁ: ' END,
			ConnectDatePersian = tMSC.ConnectDatePersian,
			ConnectTime = tMSC.ConnectTime,
			CurrentValue = CAST(tMSC.CurrentValue AS NVARCHAR(10)),
			ConnectDarsad = CAST(tMSC.[Percent] AS NVARCHAR(10)),
			MPFeederName = Tbl_MPFeeder.MPFeederName,
			Comments = tMSC.Comments
		FROM TblRequest tReq
		LEFT JOIN TblMPRequest tMPR ON tReq.MPRequestId = tMPR.MPRequestId
		LEFT JOIN TblLPRequest tLPR ON tReq.LPRequestId = tLPR.LPRequestId
		LEFT JOIN TblFogheToziDisconnect tFTR ON TReq.FogheToziDisconnectId = tFTR.FogheToziDisconnectId
		LEFT JOIN TblMultistepConnection tMSC ON tMPR.MPRequestId = tMSC.MPRequestId
			OR tFTR.FogheToziDisconnectId = tMSC.FogheToziDisconnectId
			OR tLPR.LPRequestId = tMSC.LPRequestId
		LEFT JOIN Tbl_MPFeeder ON tMSC.SourceMPFeederId = Tbl_MPFeeder.MPFeederId
		WHERE ISNULL(tMSC.IsNotConnect, 0) = 0
			AND tReq.RequestId = @RequestId
		ORDER BY tMSC.ConnectDT

		OPEN crMakeList
		
		SET @lIsLoop = 1
		WHILE @lIsLoop = 1
		BEGIN
			FETCH NEXT
				FROM crMakeList
				INTO 
					@ConnectType, 
					@ConnectDatePersian, 
					@ConnectTime, 
					@CurrentValue, 
					@ConnectDarsad, 
					@MPFeederName, 
					@Comments

			if @@FETCH_STATUS = 0 
			BEGIN
				SET @Result = @SMS
				SET @Result = Replace( @Result,'MPFeederName', ISNULL(@MPFeederName,'ø'))
				SET @Result = Replace( @Result,'ConnectType', ISNULL(@ConnectType,''))
				SET @Result = Replace( @Result,'ConnectDatePersian', ISNULL(@ConnectDatePersian,'ø'))
				SET @Result = Replace( @Result,'ConnectTime', ISNULL(@ConnectTime,'ø'))
				SET @Result = Replace( @Result,'CurrentValue', ISNULL(@CurrentValue,'ø'))
				SET @Result = Replace( @Result,'ConnectDarsad', ISNULL(@ConnectDarsad,'ø'))
				SET @Result = Replace( @Result,'Comments', ISNULL(@Comments,''))
				SET @Final = @Final + (nchar(13) + @Result)
			END
			ELSE
				SET @lIsLoop = 0
		END
	CLOSE crMakeList
	DEALLOCATE crMakeList
	RETURN @Final
END
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spSendSMSDCManager]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spSendSMSDCManager]
GO
CREATE PROCEDURE [dbo].[spSendSMSDCManager] 
	@RequestTypeId AS int,  
	@IsActive AS bit, 
	@Minutes AS int,
	@IsTamir AS bit,
	@IsSendSMSAfterConnect AS bit, 
	@ManagerSMSDCId AS int, 
	@ManagerMobile AS varchar(20), 
	@IsSetad AS bit, 
	@IsCenter AS bit, 
	@Server121Id AS int, 
	@AreaId AS int, 
	@IsMPTamir_MPFDC AS bit, 
	@IsMPNotTamir_MPFDC AS bit, 
	@Minutes_MPFDC AS int
AS
	
	IF( @IsActive = 0 ) RETURN 0;
	
	DECLARE @RequestId AS bigint
	DECLARE @RequestNumber AS bigint
	DECLARE @DBMinutes AS int
	DECLARE @MPPostName AS NVARCHAR(200)
	DECLARE @MPFeederName AS NVARCHAR(200)
	DECLARE @FogheToziPostName AS NVARCHAR(200)
	DECLARE @LPPostName AS NVARCHAR(200)
	DECLARE @LPPostCode AS NVARCHAR(200)
	DECLARE @LPFeederName AS NVARCHAR(200)
	DECLARE @DisconnectPowerMW AS NVARCHAR(200)
	DECLARE @Reason AS NVARCHAR(200)
	DECLARE @DisconnectPower AS NVARCHAR(200)
	DECLARE @DTTime AS NVARCHAR(200)
	DECLARE @DTDate AS NVARCHAR(200)
	DECLARE @Area AS NVARCHAR(200)
	DECLARE @FogheToziType AS NVARCHAR(200)
	DECLARE @FogheToziGroup AS NVARCHAR(50)
	DECLARE @FogheToziShortText AS NVARCHAR(100)
	DECLARE @IsTotalLPPostDisconnected AS bit
	DECLARE @IsNotDisconnectFeeder AS bit
	DECLARE @IsDisconnectMPFeeder AS bit
	DECLARE @MPStatus AS NVARCHAR(200) 
	DECLARE @CurrentValue AS NVARCHAR(200)
	DECLARE @Address AS NVARCHAR(200)
	DECLARE @FogheToziDisconnectId AS int
	DECLARE @FogheToziFeederName AS NVARCHAR(200)
	DECLARE @TamirStr AS NVARCHAR(10)
	DECLARE @FogheToziTypeId AS int
	DECLARE @ZoneName AS NVARCHAR(100)

	DECLARE @DCGroup AS NVARCHAR(100)
	DECLARE @DCGroupSet AS NVARCHAR(100)
	
	DECLARE @FromPathType AS NVARCHAR(50)
	DECLARE @FromPathTypeValue AS NVARCHAR(50)
	DECLARE @ToPathType AS NVARCHAR(50)
	DECLARE @ToPathTypeValue AS NVARCHAR(50)
	DECLARE @FeederPart AS NVARCHAR(100)
	
	DECLARE @IsSingleSubscriberShow AS bit
	DECLARE @SubscriberType AS NVARCHAR(10)
	
	DECLARE @OCEFRelayAction AS NVARCHAR(200)
	DECLARE @TamirNetworkTypeId AS int
	
	DECLARE @RequestAreaId AS int
	DECLARE @LastUpdate AS int
	
	DECLARE @CurrentDate AS varchar(10)
	DECLARE @CurrentMonthDay AS varchar(5)
	SELECT @CurrentDate = dbo.mtosh(getdate())
	SET @CurrentMonthDay = RIGHT(@CurrentDate,5)
	
	SET @SubscriberType = N'ò·Ì'
	SET @IsSingleSubscriberShow = 0
	SELECT @IsSingleSubscriberShow = CASE WHEN ISNULL(ConfigValue,'') = 'True' THEN 1 ELSE 0 END
	FROM Tbl_Config WHERE ConfigName = 'SMSSingleSubs'
	
	SET @FromPathType = ''
	SET @FromPathTypeValue = ''
	SET @ToPathType = ''
	SET @ToPathTypeValue = ''
	SET @FeederPart = ''
	SET @TamirStr = N'ø'
	SET @FogheToziTypeId = -1
	--declare @dt AS DateTime=dateadd(day,-1,getdate())
	declare @dt AS DateTime = dateadd(hour,-12,getdate())
	--print 'spSendSMSDCManager start' + convert(NVARCHAR(100),getdate(),21)
	
	SELECT TOP 1 
		@RequestId = TblRequest.RequestId
	FROM 
		TblRequest with(index(TblRequest68Optimized))
		LEFT OUTER JOIN TblFogheToziDisconnect ON TblRequest.FogheToziDisconnectId = TblFogheToziDisconnect.FogheToziDisconnectId 
		LEFT OUTER JOIN TblMPRequest ON TblRequest.MPRequestId = TblMPRequest.MPRequestId 
		LEFT OUTER JOIN TblLPRequest ON TblLPRequest.LPRequestId = TblRequest.LPRequestId 
		LEFT OUTER JOIN Tbl_Area ON TblRequest.AreaId = Tbl_Area.AreaId 			
		LEFT OUTER JOIN TblTamirRequestConfirm ON TblRequest.RequestId = TblTamirRequestConfirm.RequestId
		LEFT OUTER JOIN TblTamirRequest ON TblTamirRequestConfirm.TamirRequestId = TblTamirRequest.TamirRequestId
		LEFT OUTER JOIN TblTamirRequest TblTamirRequestEmergency ON TblRequest.RequestId = TblTamirRequestEmergency.EmergencyRequestId
		LEFT OUTER JOIN TblRequestPostFeeder PostFeeder On PostFeeder.RequestId = TblRequest.RequestId
	WHERE 
		tblRequest.DisconnectDT >= @dt and 
		( 
			(TblRequest.IsSingleSubscriber = 0 
				OR TblRequest.IsSingleSubscriber IS NULL 
				OR (TblRequest.IsSingleSubscriber = 1 
					AND @IsSingleSubscriberShow = 1)) 
			OR 
			(NOT TblRequest.MPRequestId IS NULL 
				AND TblRequest.MPRequestId = 1) 
			OR 
			(NOT TblRequest.FogheToziDisconnectId IS NULL 
				AND TblRequest.IsFogheToziRequest = 1)
		) 
		AND NOT TblRequest.RequestId IN 
		( 
			SELECT RequestId 
			FROM TblManagerSMSDCSended 
			WHERE 
				NOT RequestId IS NULL 
				AND ManagerSMSDCId =  @ManagerSMSDCId 
				AND ((ISNULL(RequestTypeId,@RequestTypeId) > 0 AND ISNULL(RequestTypeId,@RequestTypeId) <> 14) OR (@RequestTypeId = 0 AND ISNULL(RequestTypeId,@RequestTypeId) = 0) OR (@RequestTypeId = 14 AND ISNULL(RequestTypeId,@RequestTypeId) = 14))
		) 
		
		AND 
		(
			(@RequestTypeId = 4 AND @IsMPTamir_MPFDC = 1 AND IsDisconnectMPFeeder=1 AND DATEDIFF([Minute], TblRequest.DisconnectDT, GETDATE()) >=  @Minutes_MPFDC)
			OR (@RequestTypeId = 5 AND @IsMPNotTamir_MPFDC = 1 AND IsDisconnectMPFeeder=1 AND DATEDIFF([Minute], TblRequest.DisconnectDT, GETDATE()) >=  @Minutes_MPFDC)
			OR (DATEDIFF([Minute], TblRequest.DisconnectDT, GETDATE()) >= @Minutes)
		)
		AND TblRequest.IsLightRequest=0 
		AND TblRequest.EndJobStateId IN (4,5) 
		AND LEN(TblRequest.DisconnectTime) = 5 
		AND TblRequest.DisconnectTime <> '__:__'
		AND 
		(
			(@RequestTypeId = 0 AND @IsTamir Is NULL AND IsTamir = 0)
			
			OR (@RequestTypeId = 1 AND @IsTamir = 0 AND (NOT TblRequest.FogheToziDisconnectId IS NULL AND TblRequest.IsFogheToziRequest = 1))

			OR ((@RequestTypeId = 2 OR (@RequestTypeId = 6 AND ISNULL(TblRequest.IsSingleSubscriber,0) = 0) OR (@RequestTypeId = 7 AND TblRequest.IsSingleSubscriber = 1)) AND IsLPRequest = 1 AND ISNULL(ISNULL(TblTamirRequestEmergency.TamirNetworkTypeId, TblTamirRequest.TamirNetworkTypeId),3) = 3 AND IsTamir=1 AND TblRequest.EndJobStateId <> 4)
				
			OR ((@RequestTypeId = 3 OR (@RequestTypeId = 8 AND ISNULL(TblRequest.IsSingleSubscriber,0) = 0) OR (@RequestTypeId = 9 AND TblRequest.IsSingleSubscriber = 1)) AND IsLPRequest = 1 AND IsTamir=0)
			
			OR ((@RequestTypeId = 4 OR (@RequestTypeId = 10 AND ISNULL(TblMPRequest.IsTotalLPPostDisconnected,0) = 0) OR (@RequestTypeId = 11 AND TblMPRequest.IsTotalLPPostDisconnected = 1)) AND IsMPRequest = 1 AND ISNULL(ISNULL(TblTamirRequestEmergency.TamirNetworkTypeId, TblTamirRequest.TamirNetworkTypeId),2) = 2 AND IsTamir=1 AND TblRequest.EndJobStateId <> 4)

			OR ((@RequestTypeId = 5 OR (@RequestTypeId = 12 AND ISNULL(TblMPRequest.IsTotalLPPostDisconnected,0) = 0) OR (@RequestTypeId = 13 AND TblMPRequest.IsTotalLPPostDisconnected = 1)) AND IsMPRequest = 1 AND IsTamir=0)
			
			OR(@RequestTypeId = 14 AND (ISNULL(PostFeeder.LocationTypeId,0) = 2 AND ISNULL(TblRequest.MPRequestId,ISNULL(TblRequest.LPRequestId,ISNULL(TblRequest.FogheToziDisconnectId,0))) = 0 AND TblRequest.IsTamir = 0))
		)
		AND 
		(
			@IsSetad = 1 
			OR (@IsCenter = 1 AND Tbl_Area.Server121Id = @Server121Id) 
			OR (@IsCenter = 0 AND TblRequest.AreaId = @AreaId)
		)
		AND
		(
			ISNULL(ISNULL(TblMPRequest.IsWarmLine,TblLPRequest.IsWarmLine),0)=0
		)
	ORDER BY 
		TblRequest.DisconnectDT
	if ISNULL(@RequestId,0) <= 0
		return 0;
	SELECT TOP 1 
		@RequestId = TblRequest.RequestId, 
		@RequestNumber = TblRequest.RequestNumber, 
		@TamirStr = CASE WHEN TblRequest.IsTamir = 1 THEN N'»«»—‰«„Â' ELSE N'»Ì »—‰«„Â' END, 
		@DBMinutes = DATEDIFF([Minute], TblRequest.DisconnectDT, GETDATE()), 
		@Address = TblRequest.Address, 
		@MPPostName = ISNULL(ISNULL(Tbl_MPPost.MPPostName, tRPFMPP.MPPostName),Tbl_MPPostFogheTozi.MPPostName),
		@MPFeederName = ISNULL(Tbl_MPFeeder.MPFeederName, tRPFMPF.MPFeederName),
		@FogheToziPostName = ISNULL(Tbl_MPPostFogheTozi.MPPostName,ISNULL(Tbl_MPPost.MPPostName, tRPFMPP.MPPostName)), 
		@LPPostName = ISNULL(ISNULL(Tbl_LPPostMP.LPPostName,Tbl_LPPost.LPPostName),tRPFLPP.LPPostName), 
		@LPPostCode = ISNULL(ISNULL(Tbl_LPPostMP.LPPostCode,Tbl_LPPost.LPPostCode),tRPFLPP.LPPostCode), 
		@LPFeederName = ISNULL(Tbl_LPFeeder.LPFeederName, tRPFLPF.LPFeederName),
		@FogheToziDisconnectId = ISNULL(TblRequest.FogheToziDisconnectId,0), 
		@DisconnectPower = TblRequest.DisconnectPower, 
		@FogheToziTypeId = TblFogheToziDisconnect.FogheToziId, 
		@FogheToziType = Tbl_FogheTozi.FogheToz, 
		@DTDate = TblRequest.DisconnectDatePersian, 
		@DTTime = TblRequest.DisconnectTime, 
		@DisconnectPowerMW = CASt(ISNULL(TblFogheToziDisconnect.DisconnectPowerMW, (sqrt(3)* 20000 * TblFogheToziDisconnect.CurrentValue* 0.85 / 1000000)) AS DECIMAL(9, 2)),
		@Area = Tbl_Area.Area, 
		@IsTotalLPPostDisconnected = TblMPRequest.IsTotalLPPostDisconnected, 
		@IsNotDisconnectFeeder = TblMPRequest.IsNotDisconnectFeeder, 
		@CurrentValue = ISNULL(TblMPRequest.CurrentValue, TblRequestInfo.CurrentValueDC),
		@IsDisconnectMPFeeder = TblRequest.IsDisconnectMPFeeder,
		@FromPathType = ISNULL(ISNULL(tFPTMP.PathType,tFPTLP.PathType),''),
		@FromPathTypeValue = ISNULL(ISNULL(TblMPRequest.FromPathTypeValue,TblLPRequest.FromPathTypeValue),''),
		@ToPathType = ISNULL(ISNULL(tTPTMP.PathType,tTPTLP.PathType),''),
		@ToPathTypeValue = ISNULL(ISNULL(TblMPRequest.ToPathTypeValue,TblLPRequest.ToPathTypeValue),''),
		@FeederPart = ISNULL(ISNULL(tFP.FeederPart,tRPFFP.FeederPart),''), 
		@ZoneName = ISNULL(tZone.ZoneName,N''), 
		@SubscriberType = CASE WHEN ISNULL(TblLPRequest.IsSingleSubscriber, TblRequest.IsSingleSubscriber) = 1 THEN N' òÌ' ELSE N'ò·Ì' END,
		@OCEFRelayAction = ISNULL(Tbl_OCEFRelayAction.OCEFRelayAction, Tbl_OCEFRA.OCEFRelayAction),
		@TamirNetworkTypeId = ISNULL(TblTamirRequestEmergency.TamirNetworkTypeId, TblTamirRequest.TamirNetworkTypeId),
		@RequestAreaId = TblRequest.AreaId
	FROM 
		TblRequest 
		LEFT OUTER JOIN TblFogheToziDisconnect ON TblRequest.FogheToziDisconnectId = TblFogheToziDisconnect.FogheToziDisconnectId 
		LEFT OUTER JOIN TblMPRequest ON TblRequest.MPRequestId = TblMPRequest.MPRequestId 
		LEFT OUTER JOIN TblLPRequest ON TblLPRequest.LPRequestId = TblRequest.LPRequestId 
		LEFT OUTER JOIN Tbl_Area ON TblRequest.AreaId = Tbl_Area.AreaId 
		LEFT OUTER JOIN Tbl_MPPost Tbl_MPPostFogheTozi ON TblFogheToziDisconnect.MPPostId = Tbl_MPPostFogheTozi.MPPostId 
		LEFT OUTER JOIN Tbl_MPPost ON TblMPRequest.MPPostId = Tbl_MPPost.MPPostId 
		LEFT OUTER JOIN Tbl_MPFeeder ON TblMPRequest.MPFeederId = Tbl_MPFeeder.MPFeederId 
		LEFT OUTER JOIN Tbl_LPPost Tbl_LPPostMP ON TblMPRequest.LPPostId = Tbl_LPPostMP.LPPostId 
		LEFT OUTER JOIN Tbl_LPPost ON TblLPRequest.LPPostId = Tbl_LPPost.LPPostId 
		LEFT OUTER JOIN Tbl_LPFeeder ON TblLPRequest.LPFeederId = Tbl_LPFeeder.LPFeederId 
		LEFT OUTER JOIN Tbl_FogheTozi ON TblFogheToziDisconnect.FogheToziId = Tbl_FogheTozi.FogheToziId 
		LEFT OUTER JOIN Tbl_PathType tFPTMP ON TblMPRequest.FromPathTypeID = tFPTMP.PathTypeId
		LEFT OUTER JOIN Tbl_PathType tFPTLP ON TblLPRequest.FromPathTypeID = tFPTLP.PathTypeId
		LEFT OUTER JOIN Tbl_PathType tTPTMP ON TblMPRequest.ToPathTypeID = tTPTMP.PathTypeId
		LEFT OUTER JOIN Tbl_PathType tTPTLP ON TblLPRequest.ToPathTypeID = tTPTLP.PathTypeId
		LEFT OUTER JOIN Tbl_FeederPart tFP ON TblMPRequest.FeederPartId = tFP.FeederPartId
		LEFT OUTER JOIN Tbl_Zone tZone ON TblRequest.ZoneId = tZone.ZoneId
		LEFT OUTER JOIN Tbl_OCEFRelayAction ON TblMPRequest.OCEFRelayActionId = Tbl_OCEFRelayAction.OCEFRelayActionId
		LEFT OUTER JOIN TblRequestInfo ON TblRequest.RequestId = TblRequestInfo.RequestId
		LEFT OUTER JOIN Tbl_OCEFRelayAction Tbl_OCEFRA ON TblRequestInfo.OCEFRelayActionId = Tbl_OCEFRA.OCEFRelayActionId
		LEFT OUTER JOIN TblRequestPostFeeder ON TblRequest.RequestId = TblRequestPostFeeder.RequestId
		LEFT OUTER JOIN Tbl_MPPost tRPFMPP ON TblRequestPostFeeder.MPPostId = tRPFMPP.MPPostId
		LEFT OUTER JOIN Tbl_MPFeeder tRPFMPF ON TblRequestPostFeeder.MPFeederId = tRPFMPF.MPFeederId
		LEFT OUTER JOIN Tbl_LPPost tRPFLPP ON TblRequestPostFeeder.LPPostId = tRPFLPP.LPPostId
		LEFT OUTER JOIN Tbl_FeederPart tRPFFP ON TblRequestPostFeeder.FeederPartId = tRPFFP.FeederPartId
		LEFT OUTER JOIN Tbl_LPFeeder tRPFLPF ON TblRequestPostFeeder.LPFeederId = tRPFLPF.LPFeederId
		LEFT OUTER JOIN TblTamirRequestConfirm ON TblRequest.RequestId = TblTamirRequestConfirm.RequestId
		LEFT OUTER JOIN TblTamirRequest ON TblTamirRequestConfirm.TamirRequestId = TblTamirRequest.TamirRequestId
		LEFT OUTER JOIN TblTamirRequest TblTamirRequestEmergency ON TblRequest.RequestId = TblTamirRequestEmergency.EmergencyRequestId
	
	WHERE 
		tblRequest.RequestId=@RequestId
		
	--print 'spSendSMSDCManager Step2 ' + convert(NVARCHAR(100),getdate(),21)
	IF( @RequestId IS NULL )
		return 0;
	
	
	DECLARE @ServiceTimeOut AS int
	SET @ServiceTimeOut = -1
	
	SELECT @ServiceTimeOut = ISNULL(ConfigValue,-1)
	FROM Tbl_Config
	WHERE ConfigName = 'ServiceTimeOut'
	
	IF @ServiceTimeOut > -1
	BEGIN
		SELECT
			@LastUpdate = LastUpdate
		FROM
			View_Area
		WHERE 
			AreaId = @RequestAreaId
			
		IF @LastUpdate > @ServiceTimeOut
			return 0;
	END
	
	IF( @IsTotalLPPostDisconnected = 1 )
		SET @MPStatus = N'ﬁÿ⁄ Å”   Ê“Ì⁄'
	IF( @IsNotDisconnectFeeder = 1 )
		SET @MPStatus = N'ﬁÿ⁄Ì œ— ”— Œÿ'
	IF( @IsDisconnectMPFeeder = 1 )
		SET @MPStatus = N'ﬁÿ⁄ ò«„· ›Ìœ—'
		
	IF @RequestTypeId = 1 -- FogheTozi
	BEGIN
		SET @FogheToziGroup = CASE
			WHEN @FogheToziTypeId = 1 THEN N'ò„»Êœ  Ê·Ìœ'
			WHEN @FogheToziTypeId IN (2,3,4,5) THEN N'«‰ ﬁ«·'
			WHEN @FogheToziTypeId IN (6,7,8,9) THEN N'›Êﬁ  Ê“Ì⁄'
			ELSE N''
		END
		SET @FogheToziShortText = CASE
			WHEN @FogheToziTypeId = 1 THEN N'ò„»Êœ  Ê·Ìœ'
			WHEN @FogheToziTypeId IN (2,3,4) THEN N'«‰ ﬁ«·-»«»—‰«„Â'
			WHEN @FogheToziTypeId = 5 THEN N'«‰ ﬁ«·-»Ì »—‰«„Â'
			WHEN @FogheToziTypeId IN (6,7,8) THEN N'›Êﬁ  Ê“Ì⁄-»«»—‰«„Â'
			WHEN @FogheToziTypeId = 9 THEN N'›Êﬁ  Ê“Ì⁄-»Ì »—‰«„Â'
			ELSE N''
		END
	END

	/*------------*/
	set @DCGroup = NULL
	set @DCGroupSet = NULL
	SELECT 
		@DCGroupSet = Tbl_DisconnectGroupSet.DisconnectGroupSet, 
		@DCGroup = Tbl_DisconnectGroup.DisconnectGroup  
	FROM 
		TblRequest 
		LEFT OUTER JOIN Tbl_DisconnectGroupSet ON TblRequest.DisconnectGroupSetId = Tbl_DisconnectGroupSet.DisconnectGroupSetId
		LEFT OUTER JOIN Tbl_DisconnectGroup ON Tbl_DisconnectGroupSet.DisconnectGroupId = Tbl_DisconnectGroup.DisconnectGroupId
		/* LEFT OUTER JOIN TblLPRequest ON TblRequest.LPRequestId = TblLPRequest.LPRequestId  */
		/* LEFT OUTER JOIN Tbl_DisconnectGroupSet Tbl_DisconnectGroupSet_LP ON TblLPRequest.DisconnectGroupSetId = Tbl_DisconnectGroupSet_LP.DisconnectGroupSetId  */
		/* LEFT OUTER JOIN Tbl_DisconnectGroup Tbl_DisconnectGroup_LP ON Tbl_DisconnectGroupSet_LP.DisconnectGroupId = Tbl_DisconnectGroup_LP.DisconnectGroupId  */
		/* LEFT OUTER JOIN TblMPRequest ON TblRequest.MPRequestId = TblMPRequest.MPRequestId  */
		/* LEFT OUTER JOIN Tbl_DisconnectGroupSet Tbl_DisconnectGroupSet_MP ON TblMPRequest.DisconnectGroupSetId = Tbl_DisconnectGroupSet_MP.DisconnectGroupSetId  */
		/* LEFT OUTER JOIN Tbl_DisconnectGroup Tbl_DisconnectGroup_MP ON Tbl_DisconnectGroupSet_MP.DisconnectGroupId = Tbl_DisconnectGroup_MP.DisconnectGroupId  */
	WHERE 
		TblRequest.RequestId = @RequestId
		
	SET @Reason = ''
	IF NOT @DCGroup IS NULL
		SET @Reason = @DCGroup
	IF NOT @DCGroupSet IS NULL
	BEGIN
		IF NOT @DCGroup IS NULL SET @Reason = @Reason + ' '
		SET @Reason = @Reason + @DCGroupSet
	END
	/*------------*/
	
	/*----- ·Ì”  Ê’· Â«Ì ç‰œ „—Õ·Â «Ì ------*/
	DECLARE @MultiStepConnections AS VARCHAR(2000)
	DECLARE @NotComplate AS VARCHAR(20)
	SET @MultiStepConnections = ''
	SET @NotComplate = ''
	EXEC @MultiStepConnections = dbo.GetMultiStepConnections @RequestId
	IF LEN(@MultiStepConnections) > 1
		SET @NotComplate = ' »Â ÿÊ— ò«„·'

	/*------------*/
	
	DECLARE @RequestType AS varchar(50)
	DECLARE @SMS AS NVARCHAR(2000)
	SET @SMS = ''
	
	IF @RequestTypeId = 0 -- NewRequest
	BEGIN
		SET @RequestType = 'SMSNewRequest'
		SELECT @SMS = ConfigText FROM Tbl_Config WHERE ConfigName = @RequestType
	END
	ELSE IF @RequestTypeId = 1 -- FogheTozi
	BEGIN
		SET @RequestType = 'SMSFT'
		SELECT @SMS = ConfigText FROM Tbl_Config WHERE ConfigName = @RequestType

		SET @FogheToziFeederName = '-'
		DECLARE @FogheToziDisconnectMPFeederId AS int
		DECLARE @FTFName AS NVARCHAR(200)
		DECLARE @CurrentValueMW AS float
		DECLARE crFTFeeders CURSOR FOR
			SELECT 
				TblFogheToziDisconnectMPFeeder.FogheToziDisconnectMPFeederId,
				Tbl_MPFeeder.MPFeederName,
				CASt((sqrt(3) * ISNULL(Tbl_MPFeeder.Voltage,20000) * TblFogheToziDisconnectMPFeeder.CurrentValue * ISNULL(Tbl_MPFeeder.CosinPhi, 0.85) / 1000000) AS DECIMAL(9, 2)) AS CurrentValueMW
			FROM 
				TblFogheToziDisconnect 
				INNER JOIN TblFogheToziDisconnectMPFeeder ON TblFogheToziDisconnect.FogheToziDisconnectId = TblFogheToziDisconnectMPFeeder.FogheToziDisconnectId 
				INNER JOIN Tbl_MPFeeder ON TblFogheToziDisconnectMPFeeder.MPFeederId = Tbl_MPFeeder.MPFeederId 
			WHERE 
				TblFogheToziDisconnect.FogheToziDisconnectId = @FogheToziDisconnectId
				AND TblFogheToziDisconnectMPFeeder.ConnectDT IS NULL
				
			IF EXISTS (SELECT * FROM TblFogheToziDisconnect INNER JOIN TblFogheToziDisconnectMPFeeder ON TblFogheToziDisconnect.FogheToziDisconnectId = TblFogheToziDisconnectMPFeeder.FogheToziDisconnectId WHERE TblFogheToziDisconnect.FogheToziDisconnectId = @FogheToziDisconnectId)
				SET @DisconnectPowerMW = 0
				

			
			DECLARE @IsSendSeparateMPFeederConnect AS bit = 0
			SELECT 
				@IsSendSeparateMPFeederConnect = CAST (ConfigValue AS bit) 
			FROM 
				Tbl_Config 
			WHERE 
				ConfigName = 'IsSendSeparateMPFeederConnect'	
			
			OPEN crFTFeeders				
			DECLARE @lIsLoop AS bit	
			SET @lIsLoop = 1
			WHILE @lIsLoop = 1
			BEGIN
				FETCH NEXT FROM crFTFeeders INTO @FogheToziDisconnectMPFeederId, @FTFName, @CurrentValueMW
				if @@FETCH_STATUS = 0 
				BEGIN
					IF( @FogheToziFeederName = '-' )
						SET @FogheToziFeederName = @FTFName
					else
						SET @FogheToziFeederName = @FogheToziFeederName + 'CRLF' + @FTFName
					
					SET @DisconnectPowerMW = @DisconnectPowerMW + @CurrentValueMW
					
					IF @IsSendSeparateMPFeederConnect = 1
					BEGIN
						INSERT INTO TblManagerSMSFTMPFeederDCSended 
							(ManagerSMSDCId, FogheToziDisconnectMPFeederId, IsConnected)
						VALUES
							(@ManagerSMSDCId, @FogheToziDisconnectMPFeederId, 0)
					END
				END
				ELSE
					SET @lIsLoop = 0
			END
		CLOSE crFTFeeders
		DEALLOCATE crFTFeeders
		
		IF( @FogheToziFeederName = '-' )
		BEGIN
			SET @RequestType = 'SMSFT2'
			SELECT @SMS = ConfigText FROM Tbl_Config WHERE ConfigName = @RequestType
		END
		
	END
	ELSE IF @RequestTypeId = 2 -- LPTamir
	BEGIN
		SET @RequestType = 'SMSLPTamir'
		SELECT @SMS = ConfigText FROM Tbl_Config WHERE ConfigName = @RequestType
	END
	ELSE IF @RequestTypeId = 3 -- LPNotTamir
	BEGIN
		SET @RequestType = 'SMSLPNotTamir'
		SELECT @SMS = ConfigText FROM Tbl_Config WHERE ConfigName = @RequestType
	END	
	ELSE IF @RequestTypeId = 4 -- MPTamir
	BEGIN
		IF @IsNotDisconnectFeeder = 1
			SET @RequestType = 'SMSMPTamirFeederPart'
		ELSE IF @IsTotalLPPostDisconnected = 1
			SET @RequestType = 'SMSMPTamirPost'
		ELSE
			SET @RequestType = 'SMSMPTamir'
		SELECT @SMS = ConfigText FROM Tbl_Config WHERE ConfigName = @RequestType
	END	
	ELSE IF @RequestTypeId = 5 -- MPNotTamir
	BEGIN
		IF @IsNotDisconnectFeeder = 1
			SET @RequestType = 'SMSMPNotTamirFeederPart'
		ELSE IF @IsTotalLPPostDisconnected = 1
			SET @RequestType = 'SMSMPNotTamirPost'
		ELSE
			SET @RequestType = 'SMSMPNotTamir'
		SELECT @SMS = ConfigText FROM Tbl_Config WHERE ConfigName = @RequestType
	END
	ELSE IF @RequestTypeId = 10 
	BEGIN
		IF @IsNotDisconnectFeeder = 1
			SET @RequestType = 'SMSMPTamirFeederPart'
		ELSE
			SET @RequestType = 'SMSMPTamirAll'
		SELECT @SMS = ConfigText FROM Tbl_Config WHERE ConfigName = @RequestType
	END
	ELSE IF @RequestTypeId = 11 
	BEGIN
		SET @RequestType = 'SMSMPTamirPostSingle'
		SELECT @SMS = ConfigText FROM Tbl_Config WHERE ConfigName = @RequestType
	END
	ELSE IF @RequestTypeId = 12 
	BEGIN
		IF @IsNotDisconnectFeeder = 1
			SET @RequestType = 'SMSMPNotTamirFeederPart'
		ELSE
			SET @RequestType = 'SMSMPNotTamirAll'
		SELECT @SMS = ConfigText FROM Tbl_Config WHERE ConfigName = @RequestType
	END
	ELSE IF @RequestTypeId = 13 
	BEGIN
		SET @RequestType = 'SMSMPNotTamirPostSingle'
		SELECT @SMS = ConfigText FROM Tbl_Config WHERE ConfigName = @RequestType
	END
	ELSE IF @RequestTypeId = 6 -- LPTamir All
	BEGIN
		SET @RequestType = 'SMSLPTamirAll' 
		SELECT @SMS = ConfigText FROM Tbl_Config WHERE ConfigName = @RequestType
	END
	ELSE IF @RequestTypeId = 7 -- LPTamir Single
	BEGIN
		SET @RequestType = 'SMSLPTamirSingle' 
		SELECT @SMS = ConfigText FROM Tbl_Config WHERE ConfigName = @RequestType
	END
	ELSE IF @RequestTypeId = 8 -- LPNotTamir All
	BEGIN
		SET @RequestType = 'SMSLPNotTamirAll' 
		SELECT @SMS = ConfigText FROM Tbl_Config WHERE ConfigName = @RequestType
	END
	ELSE IF @RequestTypeId = 9 -- LPNotTamir Single
	BEGIN
		SET @RequestType = 'SMSLPNotTamirSingle' 
		SELECT @SMS = ConfigText FROM Tbl_Config WHERE ConfigName = @RequestType
	END
	ELSE IF @RequestTypeId = 14 -- NewRequestMP
	BEGIN
		SET @RequestType = 'SMSNewRequestMP'
		SELECT @SMS = ConfigText FROM Tbl_Config WHERE ConfigName = @RequestType
	END
	
	IF @SMS = '' OR @SMS IS NULL RETURN 0;
	
	INSERT INTO TblManagerSMSDCSended 
		(ManagerSMSDCId, RequestId, IsSendSMSAfterConnect, RequestTypeId) 
		VALUES (@ManagerSMSDCId, @RequestId, @IsSendSMSAfterConnect ^ 1, @RequestTypeId)
	
	SELECT TOP 0 * FROM TblManagerSMSDCSended
	
	SET @SMS = Replace( @SMS,'ReqNo', ISNULL(@RequestNumber,0))
	SET @SMS = Replace( @SMS,'Minutes', ISNULL(@DBMinutes,0))
	SET @SMS = Replace( @SMS,'FogheToziPostName', ISNULL(@FogheToziPostName,N'ø'))
	SET @SMS = Replace( @SMS,'MPPostName', ISNULL(@MPPostName,N'ø'))
	SET @SMS = Replace( @SMS,'Area', ISNULL(@Area,N'ø'))
	SET @SMS = Replace( @SMS,'MPFeederName', ISNULL(@MPFeederName,N'ø'))
	SET @SMS = Replace( @SMS,'LPPostName', ISNULL(@LPPostName,N'ø'))
	SET @SMS = Replace( @SMS,'LPPostCode', ISNULL(@LPPostCode,N'ø'))
	SET @SMS = Replace( @SMS,'LPFeederName', ISNULL(@LPFeederName,N'ø'))
	SET @SMS = Replace( @SMS,'DBAddress', ISNULL(@Address,N'ø'))
	SET @SMS = Replace( @SMS,'FogheToziFeederName', ISNULL(@FogheToziFeederName,N'ø'))
	SET @SMS = Replace( @SMS,'DisconnectPowerMW', ISNULL(@DisconnectPowerMW ,N'ø'))
	SET @SMS = Replace( @SMS,'DisconnectPower', ISNULL(@DisconnectPower,N'ø'))
	SET @SMS = Replace( @SMS,'DTDate', ISNULL(@DTDate,N'ø'))
	SET @SMS = Replace( @SMS,'DTTime', ISNULL(@DTTime,N'ø'))
	SET @SMS = Replace( @SMS,'FogheToziType', ISNULL(@FogheToziType,N'ø'))
	SET @SMS = Replace( @SMS,'FogheToziGroup', ISNULL(@FogheToziGroup,N'ø'))
	SET @SMS = Replace( @SMS,'FogheToziShortText', ISNULL(@FogheToziShortText,N'ø'))
	SET @SMS = Replace( @SMS,'Reason', ISNULL(@Reason,N'ø'))
	SET @SMS = Replace( @SMS,'MPStatus', ISNULL(@MPStatus,N'ø'))
	SET @SMS = Replace( @SMS,'CurrentValue', ISNULL(@CurrentValue,N'ø'))
	SET @SMS = Replace( @SMS,'FromType', CASE WHEN NULLIF(@FromPathType,N'') IS NOT NULL THEN N'«“ ' + @FromPathType ELSE '' END )
	SET @SMS = Replace( @SMS,'FromValue', @FromPathTypeValue)
	SET @SMS = Replace( @SMS,'ToType', CASE WHEN NULLIF(@ToPathType,N'') IS NOT NULL THEN N' « ' + @ToPathType ELSE '' END )
	SET @SMS = Replace( @SMS,'ToValue', @ToPathTypeValue)
	SET @SMS = Replace( @SMS,'FeederPart', ISNULL(@FeederPart,N'ø'))
	SET @SMS = Replace( @SMS,'TamirStr', ISNULL(@TamirStr,N'ø'))
	SET @SMS = Replace( @SMS,'ZoneName', ISNULL(@ZoneName,N'ø'))
	SET @SMS = Replace( @SMS,'SubscriberType', ISNULL(@SubscriberType,N'ø'))
	SET @SMS = Replace( @SMS,'OCEFRelayAction', ISNULL(@OCEFRelayAction,N'ø'))
	SET @SMS = Replace( @SMS,'NotComplate', ISNULL(@NotComplate,''))
	SET @SMS = Replace( @SMS,'MultiStepConnections', ISNULL(@MultiStepConnections,''))
	SET @SMS = Replace( @SMS,'CurrentDate', ISNULL(@CurrentDate,''))
	SET @SMS = Replace( @SMS,'CurrentMonthDay', ISNULL(@CurrentMonthDay,''))
	
	SET @SMS = Replace( @SMS,'CRLF', nchar(13))

	DECLARE @Desc AS NVARCHAR(100)
	SET @Desc = 'ReqId=' + CASt(@RequestNumber AS NVARCHAR)
	EXEC spSendSMS @SMS , @ManagerMobile, @Desc, @RequestType, @AreaId
--print 'spSendSMSDCManager End ' + convert(NVARCHAR(100),getdate(),21)
GO



IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spSendSMSMPFeederDCCount]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spSendSMSMPFeederDCCount]
GO
CREATE PROCEDURE [dbo].[spSendSMSMPFeederDCCount] 
	@ManagerSMSMPFId AS bigint, 
	@ManagerSMSDCId AS int, 
	@ManagerMobile AS nvarchar(20), 
	@YearMonth AS char(7), 
	@DCCount AS int, 
	@MPPostName nvarchar(50), 
	@MPFeederId AS int, 
	@FeederName nvarchar(50)
AS
	DECLARE @SMS AS nvarchar(2000)
	DECLARE @Area AS nvarchar(50)
	DECLARE @CurrentDate AS varchar(10)
	DECLARE @CurrentMonthDay AS varchar(5)
	SELECT @CurrentDate = dbo.mtosh(getdate())
	SET @CurrentMonthDay = RIGHT(@CurrentDate,5)
	
	IF @ManagerSMSMPFId IS NULL
	BEGIN
		DECLARE @WorkingAreaId bigint
		DECLARE @MaxId bigint
		DECLARE @lNewId bigint
		
		SELECT TOP 1 @WorkingAreaId = WorkingAreaID
		FROM TblDepartmentInfo

		SELECT @MaxId = MAX(ManagerSMSMPFId)
		FROM TblManagerSMSMPF INNER JOIN TblManagerSMSDC ON TblManagerSMSMPF.ManagerSMSDCId = TblManagerSMSDC.ManagerSMSDCId

		IF @MaxId IS NULL
			SET @lNewId = @WorkingAreaId * 100000000 + 1
		ELSE
			SET @lNewId = @MaxId + 1

		INSERT INTO TblManagerSMSMPF (ManagerSMSMPFId, ManagerSMSDCId, YearMonth, MPFeederId, DCCount, IsSendSMS)
		VALUES (@lNewId,@ManagerSMSDCId,@YearMonth,@MPFeederId,@DCCount,0)
	END
	ELSE
		UPDATE TblManagerSMSMPF SET DCCount = @DCCount WHERE ManagerSMSMPFId = @ManagerSMSMPFId
		
	SELECT @Area = Area
	FROM Tbl_MPFeeder INNER JOIN Tbl_Area ON Tbl_MPFeeder.AreaId = Tbl_Area.AreaId
	WHERE Tbl_MPFeeder.MPFeederId = @MPFeederId

	DECLARE @RequestType AS nvarchar(200)
	SET @SMS = NULL
	SET @RequestType = 'SMSMPFDCnTime'
	SELECT @SMS = ConfigText FROM Tbl_Config WHERE ConfigName = @RequestType
	
	DECLARE @lYearMonth AS char(7)
	DECLARE @lMonth AS char(2)
	SET @lYearMonth = LEFT(dbo.mtosh(GETDATE()),7)
	SET @lMonth = RIGHT(@lYearMonth,2)
	
	DECLARE @lMonthName AS nvarchar(20)
	SET @lMonthName = CASE Cast(@lMonth AS int)
		WHEN 1 THEN N'›—Ê—œÌ‰'
		WHEN 2 THEN N'«—œÌ»Â‘ '
		WHEN 3 THEN N'Œ—œ«œ'
		WHEN 4 THEN N' Ì—'
		WHEN 5 THEN N'„—œ«œ'
		WHEN 6 THEN N'‘Â—ÌÊ—'
		WHEN 7 THEN N'„Â—'
		WHEN 8 THEN N'¬»«‰'
		WHEN 9 THEN N'¬–—'
		WHEN 10 THEN N'œÌ'
		WHEN 11 THEN N'»Â„‰'
		WHEN 12 THEN N'«”›‰œ'
		ELSE N'ø'
	END
	
	DECLARE @Cnt AS int
	SET @Cnt = 0
	IF NOT @SMS IS NULL
	BEGIN
		SET @SMS = Replace( @SMS,'Count', ISNULL(@DCCount,0))
		SET @SMS = Replace( @SMS,'Area', ISNULL(@Area,N'ø'))
		SET @SMS = Replace( @SMS,'FeederName', ISNULL(@FeederName,N'ø'))
		SET @SMS = Replace( @SMS,'MPPostName', ISNULL(@MPPostName,N'ø'))
		SET @SMS = Replace( @SMS,'MonthName', ISNULL(@lMonthName,N'ø'))
		SET @SMS = Replace( @SMS,'CurrentDate', ISNULL(@CurrentDate,N''))
		SET @SMS = Replace( @SMS,'CurrentMonthDay', ISNULL(@CurrentMonthDay,N''))

		SET @SMS = Replace( @SMS,'CRLF', nchar(13) )

		DECLARE @Desc AS nvarchar(100)
		DECLARE @AreaId AS int
		SET @Desc = 'FeederId=' + CAST(@MPFeederId AS nvarchar)
		SELECT @AreaId = AreaId FROM TblManagerSMSDC WHERE ManagerSMSDCId = @ManagerSMSDCId
		EXEC spSendSMS @SMS, @ManagerMobile, @Desc, @RequestType, @AreaId
		
		SET @Cnt = 1
	END
	
	RETURN @Cnt
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spSendSMSTamirConfirm]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spSendSMSTamirConfirm]
GO
CREATE PROCEDURE [dbo].[spSendSMSTamirConfirm]
	@TamirRequestId As bigint,  
	@TamirRequestStateId As int, 
	@IsTamir_Confirm As bit, 
	@IsTamir_NotConfirm As bit, 
	@ManagerAreaId As int,
	@ManagerSMSDCId As int, 
	@ManagerMobile As varchar(20), 
	@AreaId As int,
	@IsTamir_ConfirmLP As bit, 
	@IsTamir_NotConfirmLP As bit,
	@IsTamir_ConfirmFT As bit, 
	@IsTamir_NotConfirmFT As bit
AS
	IF NOT (((@IsTamir_Confirm = 1 OR @IsTamir_ConfirmLP = 1 OR @IsTamir_ConfirmFT = 1) AND @TamirRequestStateId = 3) 
		OR ((@IsTamir_NotConfirm = 1 OR @IsTamir_NotConfirmLP = 1 OR @IsTamir_NotConfirmFT = 1) AND @TamirRequestStateId = 8)) RETURN 0;
	
	declare @IsSendSMSConfirmForCommonFeederArea as bit = 0
	SELECT @IsSendSMSConfirmForCommonFeederArea = ISNULL(ConfigValue,0) FROM Tbl_Config WHERE ConfigName = 'IsSendSMSConfirmForCommonFeederArea'
	
	IF @IsSendSMSConfirmForCommonFeederArea = 1 AND @TamirRequestStateId = 3
	BEGIN
		SELECT *
		INTO #tblTRFTFeeder
		FROM TblTamirRequestFTFeeder
		WHERE TamirRequestId = @TamirRequestId

		SELECT AreaId
		INTO #tblArea
		FROM (
			SELECT DISTINCT Tbl_MPCommonFeeder.AreaId
			FROM Tbl_MPCommonFeeder
			INNER JOIN #tblTRFTFeeder ON Tbl_MPCommonFeeder.MPFeederId = #tblTRFTFeeder.MPFeederId
			WHERE #tblTRFTFeeder.TamirRequestId = @TamirRequestId
			
			UNION
			
			SELECT DISTINCT Tbl_MPFeeder.AreaId
			FROM Tbl_MPFeeder
			INNER JOIN #tblTRFTFeeder ON Tbl_MPFeeder.MPFeederId = #tblTRFTFeeder.MPFeederId
			WHERE #tblTRFTFeeder.TamirRequestId = @TamirRequestId
			
			UNION
			
			SELECT DISTINCT Tbl_MPFeeder.AreaId
			FROM Tbl_MPFeeder
			INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId
			INNER JOIN TblTamirRequest ON Tbl_MPPost.MPPostId = TblTamirRequest.MPPostId
			WHERE TblTamirRequest.TamirRequestId = @TamirRequestId
				AND TblTamirRequest.MPFeederId IS NULL
				AND NOT TblTamirRequest.TamirRequestId IN (
					SELECT TamirRequestId
					FROM #tblTRFTFeeder
					)
			
			UNION
			
			SELECT DISTINCT Tbl_MPCommonFeeder.AreaId
			FROM Tbl_MPCommonFeeder
			INNER JOIN Tbl_MPFeeder ON Tbl_MPCommonFeeder.MPFeederId = Tbl_MPFeeder.MPFeederId
			INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId
			INNER JOIN TblTamirRequest ON Tbl_MPPost.MPPostId = TblTamirRequest.MPPostId
			WHERE TblTamirRequest.TamirRequestId = @TamirRequestId
				AND TblTamirRequest.MPFeederId IS NULL
				AND NOT TblTamirRequest.TamirRequestId IN (
					SELECT TamirRequestId
					FROM #tblTRFTFeeder
					)
			
			UNION
			
			SELECT DISTINCT Tbl_MPCommonFeeder.AreaId
			FROM Tbl_MPCommonFeeder
			INNER JOIN TblTamirRequest ON Tbl_MPCommonFeeder.MPFeederId = TblTamirRequest.MPFeederId
			WHERE TblTamirRequest.TamirRequestId = @TamirRequestId
				AND TblTamirRequest.FeederPartId IS NULL
				AND TblTamirRequest.LPPostId IS NULL
				AND TblTamirRequest.LPFeederId IS NULL
				AND TblTamirRequest.TamirNetworkTypeId = 2
			
			UNION
			
			SELECT DISTINCT Tbl_MPFeeder.AreaId
			FROM Tbl_MPFeeder
			INNER JOIN TblTamirRequest ON Tbl_MPFeeder.MPFeederId = TblTamirRequest.MPFeederId
			WHERE TblTamirRequest.TamirRequestId = @TamirRequestId
				AND TblTamirRequest.FeederPartId IS NULL
				AND TblTamirRequest.LPPostId IS NULL
				AND TblTamirRequest.LPFeederId IS NULL
				AND TblTamirRequest.TamirNetworkTypeId = 2
			
			UNION
			
			SELECT DISTINCT TblTamirRequest.AreaId
			FROM TblTamirRequest
			WHERE TblTamirRequest.TamirRequestId = @TamirRequestId
			) t1

		DROP TABLE #tblTRFTFeeder
		
		IF @ManagerAreaId <> 99 AND NOT @ManagerAreaId IN (SELECT AreaId FROM #tblArea)  RETURN 0;
		
		DROP TABLE #tblArea
	END
	ELSE
	BEGIN
		IF @ManagerAreaId <> 99 AND @ManagerAreaId <> @AreaId  RETURN 0;
	END
	
	IF EXISTS(SELECT * FROM TblManagerSMSDCSended WHERE ManagerSMSDCId = @ManagerSMSDCId AND TamirRequestId = @TamirRequestId ) RETURN 0;

	DECLARE @RequestNumber AS bigint
	DECLARE @DBMinutes AS int
	DECLARE @MPPostName AS nvarchar(200)
	DECLARE @MPFeederName AS nvarchar(200)
	DECLARE @LPPostName AS nvarchar(200)
	DECLARE @LPFeederName AS nvarchar(200)
	DECLARE @DisconnectPower AS nvarchar(200)
	DECLARE @DTTime AS nvarchar(200)
	DECLARE @DTDate AS nvarchar(200)
	DECLARE @Area AS nvarchar(200)
	DECLARE @Status AS nvarchar(200) 
	DECLARE @Address AS nvarchar(200)
	DECLARE @CriticalsAddress AS nvarchar(200)
	DECLARE @WarmLineMode AS nvarchar(200)
	DECLARE @WarmLineAdvance AS nvarchar(200)
	DECLARE @IsWarmLine AS bit
	
	DECLARE @MPPostInfo AS nvarchar(400)
	DECLARE @MPFeedersInfo AS nvarchar(1000)
	DECLARE @MPFeederInfo AS nvarchar(400)
	DECLARE @LPPostInfo AS nvarchar(400)
	DECLARE @FeederPartInfo AS nvarchar(400)
	DECLARE @LPFeederInfo AS nvarchar(400)
	
	SET @MPPostInfo = ''
	SET @MPFeedersInfo = ''
	SET @MPFeederInfo = ''
	SET @LPPostInfo = ''
	SET @FeederPartInfo = ''
	SET @LPFeederInfo = ''

	DECLARE @RequestType AS nvarchar(200)
	DECLARE @SMS AS nvarchar(2000)
	
	DECLARE @CurrentDate AS varchar(10)
	DECLARE @CurrentMonthDay AS varchar(5)
	SELECT @CurrentDate = dbo.mtosh(getdate())
	SET @CurrentMonthDay = RIGHT(@CurrentDate,5)
	
	
	IF NOT EXISTS (
		SELECT * FROM TblTamirRequest WHERE 
			TblTamirRequest.TamirRequestId = @TamirRequestId
			AND (((@IsTamir_Confirm = 1 OR @IsTamir_NotConfirm = 1) AND TblTamirRequest.TamirNetworkTypeId = 2 )
			OR ((@IsTamir_ConfirmLP = 1 OR @IsTamir_NotConfirmLP = 1) AND TblTamirRequest.TamirNetworkTypeId = 3 )
			OR ((@IsTamir_ConfirmFT = 1 OR @IsTamir_NotConfirmFT = 1) AND TblTamirRequest.TamirNetworkTypeId = 1 )))
	RETURN 0;

	SELECT 
		@RequestNumber = TblTamirRequest.TamirRequestNo, 
		@MPPostName = Tbl_MPPost.MPPostName, 
		@MPPostInfo = 'Å”  ›Êﬁ  Ê“Ì⁄ ' + Tbl_MPPost.MPPostName + 'CRLF', 
		@MPFeederName = Tbl_MPFeeder.MPFeederName, 
		@MPFeederInfo = '›Ìœ— ›‘«— „ Ê”ÿ ' + Tbl_MPFeeder.MPFeederName + 'CRLF', 
		@LPPostName = Tbl_LPPost.LPPostName, 
		@LPPostInfo = 'Å”   Ê“Ì⁄ ' + Tbl_LPPost.LPPostName + 'CRLF', 
		@FeederPartInfo = ' òÂ ›Ìœ— ' + Tbl_FeederPart.FeederPart + 'CRLF', 
		@LPFeederInfo = '›Ìœ— ›‘«— ÷⁄Ì› ' + Tbl_LPFeeder.LPFeederName + 'CRLF',
		@LPFeederName = Tbl_LPFeeder.LPFeederName, 
		@Address = ISNULL(TblTamirRequest.WorkingAddress, TblTamirRequest.CriticalsAddress),
		@CriticalsAddress = TblTamirRequest.CriticalsAddress, 
		@DBMinutes = TblTamirRequest.DisconnectInterval, 
		@DTDate = ISNULL(TblTamirRequest.DisconnectDatePersian,''), 
		@DTTime = ISNULL(TblTamirRequest.DisconnectTime,''), 
		@DisconnectPower = Cast( Round(ISNULL(TblTamirRequest.DisconnectPower,0),2) AS varchar ), 
		@Area = ISNULL(Tbl_Area.Area,''),
		@WarmLineMode = CASE WHEN TblTamirRequest.IsWarmLine = 1 THEN 'Œÿ ê—„' ELSE 'Œÿ ”—œ' END,
		@IsWarmLine = TblTamirRequest.IsWarmLine
	FROM 
		TblTamirRequest 
		INNER JOIN Tbl_Area ON TblTamirRequest.AreaId = Tbl_Area.AreaId 
		LEFT OUTER JOIN Tbl_MPPost ON TblTamirRequest.MPPostId = Tbl_MPPost.MPPostId 
		LEFT OUTER JOIN Tbl_MPFeeder ON TblTamirRequest.MPFeederId = Tbl_MPFeeder.MPFeederId 
		LEFT OUTER JOIN Tbl_LPPost ON TblTamirRequest.LPPostId = Tbl_LPPost.LPPostId
		LEFT OUTER JOIN Tbl_LPFeeder ON TblTamirRequest.LPFeederId = Tbl_LPFeeder.LPFeederId 
		LEFT OUTER JOIN Tbl_FeederPart ON TblTamirRequest.FeederPartId = Tbl_FeederPart.FeederPartId
	WHERE 
		TblTamirRequest.TamirRequestId = @TamirRequestId
		
	
	DECLARE @MPFeedersName AS nvarchar(1000)
	SET @MPFeedersName = ''
	SELECT 
		@MPFeedersName = @MPFeedersName + MPFeederName + ', '
	FROM 
		TblTamirRequestFTFeeder
		INNER JOIN Tbl_MPFeeder ON TblTamirRequestFTFeeder.MPFeederId = Tbl_MPFeeder.MPFeederId
	WHERE
		TblTamirRequestFTFeeder.TamirRequestId = @TamirRequestId

	IF CHARINDEX(',' , @MPFeedersName) > 0
	BEGIN
		SET @MPFeedersInfo = '›Ìœ—Â«Ì ›‘«— „ Ê”ÿ ' + LEFT(@MPFeedersName, (LEN(@MPFeedersName) - 1)) + 'CRLF'
		SET @MPFeederInfo = ''
	END

	SET @WarmLineAdvance = ''
	if @TamirRequestStateId = 3 
	BEGIN
		SET @Status = N' «ÌÌœ ê—œÌœ'
		if @IsWarmLine = 1
			SET @WarmLineAdvance = '»’Ê—  Œÿ ê—„'
		if @IsWarmLine = 0
			SET @WarmLineAdvance = '»’Ê—  Œÿ ”—œ'
	END
	if @TamirRequestStateId = 8 
		SET @Status = N' «ÌÌœ ‰ê—œÌœ'


	SET @RequestType = 'SMSTamirConfirm'
	SELECT @SMS = ConfigText FROM Tbl_Config WHERE ConfigName = @RequestType
	----
	INSERT INTO TblManagerSMSDCSended 
		(ManagerSMSDCId, TamirRequestId, IsSendSMSAfterConnect) 
	VALUES 
		(@ManagerSMSDCId, @TamirRequestId, 1)
		
	SET @SMS = Replace( @SMS , 'CRLF', nchar(13) );
	SET @SMS = Replace( @SMS , 'TamirReqNo' , ISNULL(@RequestNumber,0))
	SET @SMS = Replace( @SMS , 'Minutes' , ISNULL(@DBMinutes,0))
	SET @SMS = Replace( @SMS , 'MPPostName' , ISNULL(@MPPostName,'-'))
	SET @SMS = Replace( @SMS , 'MPPostInfo ' , ISNULL(@MPPostInfo,''))
	SET @SMS = Replace( @SMS , 'Area' , ISNULL(@Area,N'ø'))
	SET @SMS = Replace( @SMS , 'MPFeederName' , ISNULL(@MPFeederName,'-'))
	SET @SMS = Replace( @SMS , 'MPFeederInfo ' , ISNULL(@MPFeederInfo,''))
	SET @SMS = Replace( @SMS , 'MPFeedersInfo ' , ISNULL(@MPFeedersInfo,''))
	SET @SMS = Replace( @SMS , 'LPPostName' , ISNULL(@LPPostName,'-'))
	SET @SMS = Replace( @SMS , 'LPPostInfo ' , ISNULL(@LPPostInfo,''))
	SET @SMS = Replace( @SMS , 'FeederPartInfo ' , ISNULL(@FeederPartInfo,''))
	SET @SMS = Replace( @SMS , 'LPFeederName' , ISNULL(@LPFeederName,'-'))
	SET @SMS = Replace( @SMS , 'LPFeederInfo ' , ISNULL(@LPFeederInfo,''))
	SET @SMS = Replace( @SMS , 'DBAddress' , ISNULL(@Address,N'ø'))
	SET @SMS = Replace( @SMS , 'CriticalsAddress' , ISNULL(@CriticalsAddress,N'ø'))
	SET @SMS = Replace( @SMS , 'DisconnectPower', ISNULL(@DisconnectPower,N'ø'))
	SET @SMS = Replace( @SMS , 'DTDate' , ISNULL(@DTDate,N'ø'))
	SET @SMS = Replace( @SMS , 'DTTime' , ISNULL(@DTTime,N'ø'))
	SET @SMS = Replace( @SMS , 'WarmLineMode' , ISNULL(@WarmLineMode,N'ø'))
	SET @SMS = Replace( @SMS , 'WarmLineAdvance' , ISNULL(@WarmLineAdvance,N'ø'))
	SET @SMS = Replace( @SMS , 'Status' , ISNULL(@Status,N'ø'))
	SET @SMS = Replace( @SMS , 'CurrentDate', ISNULL(@CurrentDate,N''))
	SET @SMS = Replace( @SMS , 'CurrentMonthDay', ISNULL(@CurrentMonthDay,N''))
	SET @SMS = Replace( @SMS , 'CRLF', nchar(13))

	DECLARE @Desc AS nvarchar(100)
	SET @Desc = 'TamirReqId=' +  + Cast(@RequestNumber as nvarchar)
	EXEC spSendSMS @SMS , @ManagerMobile, @Desc, @RequestType, @ManagerAreaId
GO


IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spSendSMSforDGSRequest]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spSendSMSforDGSRequest]
GO
CREATE PROCEDURE [dbo].[spSendSMSforDGSRequest] 
	@ManagerSMSDCId AS int, 
	@ManagerMobile varchar(20), 
	@AreaId AS int, 
	@Server121Id AS int
AS
	DECLARE @RequestId AS bigint
	DECLARE @RequestNumber AS bigint
	DECLARE @MPPostName AS nvarchar(200)
	DECLARE @MPFeederName AS nvarchar(200)
	DECLARE @FogheToziPostName AS nvarchar(200)
	DECLARE @LPPostName AS nvarchar(200)
	DECLARE @LPFeederName AS nvarchar(200)
	DECLARE @Reason AS nvarchar(200)
	DECLARE @DisconnectPowerFT AS nvarchar(200)
	DECLARE @DisconnectPower AS nvarchar(200)
	DECLARE @DTTime AS nvarchar(200)
	DECLARE @DTDate AS nvarchar(200)
	DECLARE @Area AS nvarchar(200)
	DECLARE @FogheToziType AS nvarchar(200)
	DECLARE @IsTotalLPPostDisconnected AS bit
	DECLARE @IsNotDisconnectFeeder AS bit
	DECLARE @IsDisconnectMPFeeder AS bit
	DECLARE @CurrentValue AS nvarchar(200)
	DECLARE @Address AS nvarchar(200)
	
	DECLARE @FogheToziDisconnectId AS int
	DECLARE @FogheToziFeederName AS nvarchar(200)

	DECLARE @ManagerSMSDGSId AS bigint
	DECLARE @DCGroup AS nvarchar(100)
	DECLARE @DCGroupSet AS nvarchar(100)
	DECLARE @DCGroupSetId AS int
	DECLARE @Description AS nvarchar(200)
	DECLARE @DisconnectReasonId AS int
	
	DECLARE @IsFT AS bit
	DECLARE @IsMP AS bit
	DECLARE @IsLP AS bit
	DECLARE @IsLight AS bit
	
	DECLARE @SMSClause AS nvarchar(2000)
	DECLARE @SMS AS nvarchar(2000)
	DECLARE @RequestType AS nvarchar(200)
	DECLARE @ReqType AS nvarchar(50)
	
	DECLARE @CurrentDate AS varchar(10)
	DECLARE @CurrentMonthDay AS varchar(5)
	SELECT @CurrentDate = dbo.mtosh(getdate())
	SET @CurrentMonthDay = RIGHT(@CurrentDate,5)
	
	DECLARE @IsCenter AS INT = 0
	SELECT 
		@IsCenter = IsCenter
	FROM 
		Tbl_Area
	WHERE 
		AreaId = @AreaId


	DECLARE @lSendCount AS int
	/*------------*/
	SET @RequestType = 'SMSDGSRequest'
	SET @SMSClause = NULL
	SELECT @SMSClause = ConfigText FROM Tbl_Config WHERE ConfigName = @RequestType
	IF @SMSClause IS NUlL RETURN 0
	declare @SDate as datetime=dateadd(DAY,-1,getdate())
	declare @EDate as datetime=getdate()
									 
	SET @FogheToziType = NULL
	/*------------*/

	CREATE Table #tDGS
	(
		RequestId bigint , 
		RequestNumber bigint , 
		MPPostName nvarchar(50) , 
		FogheToziPostName nvarchar(50) , 
		MPFeederName nvarchar(50) , 
		LPPostName nvarchar(50) , 
		LPFeederName nvarchar(50) , 
		FogheToziDisconnectId int , 
		DisconnectPower float , 
		FogheToziType nvarchar(50) , 
		DTDate varchar(10) , 
		DTTime varchar(5) , 
		DisconnectPowerFT float , 
		Area nvarchar(50) , 
		IsDisconnectMPFeeder bit , 
		IsTotalLPPostDisconnected bit , 
		IsNotDisconnectFeeder bit , 
		CurrentValue float ,
		IsFT bit, 
		IsMP bit ,
		IsLP bit ,
		IsLight bit,
		DisconnectGroupSetId int, 
		DisconnectGroupSet nvarchar(100), 
		DisconnectGroup nvarchar(100), 
		DisconnectReasonId int,
		Description nvarchar(200),
		ManagerSMSDGSId bigint
	)

	INSERT INTO #tDGS
	SELECT 
		TblRequest.RequestId, 
		TblRequest.RequestNumber, 
		Tbl_MPPost.MPPostName, 
		Tbl_MPPostFogheTozi.MPPostName AS FogheToziPostName, 
		Tbl_MPFeeder.MPFeederName, 
		ISNULL(Tbl_LPPost_MP.LPPostName, Tbl_LPPost.LPPostName) AS LPPostName, 
		Tbl_LPFeeder.LPFeederName, 
		ISNULL(TblRequest.FogheToziDisconnectId,0) AS FogheToziDisconnectId, 
		TblRequest.DisconnectPower, 
		Tbl_FogheTozi.FogheToz AS FogheToziType, 
		TblRequest.DisconnectDatePersian AS DTDate, 
		TblRequest.DisconnectTime AS DTTime, 
		TblFogheToziDisconnect.DisconnectPower AS DisconnectPowerFT, 
		Tbl_Area.Area, 
		TblRequest.IsDisconnectMPFeeder, 
		TblMPRequest.IsTotalLPPostDisconnected, 
		TblMPRequest.IsNotDisconnectFeeder, 
		ISNULL(TblMPRequest.CurrentValue, TblFogheToziDisconnect.CurrentValue) AS CurrentValue,
		TblRequest.IsFogheToziRequest AS  IsFT,
		TblRequest.IsMPRequest AS IsMP,
		TblRequest.IsLPRequest AS IsLP,
		TblRequest.IsLightRequest As IsLight, 
		ISNULL(TblMPRequest.DisconnectGroupSetId, TblLPRequest.DisconnectGroupSetId) AS DisconnectGroupSetId, 
		ISNULL(Tbl_DisconnectGroupSet_MP.DisconnectGroupSet, Tbl_DisconnectGroupSet_LP.DisconnectGroupSet) AS DisconnectGroupSet, 
		ISNULL(Tbl_DisconnectGroup_MP.DisconnectGroup, Tbl_DisconnectGroup_LP.DisconnectGroup) AS DisconnectGroup, 
		ISNULL(TblMPRequest.DisconnectReasonId, TblLPRequest.DisconnectReasonId) AS DisconnectReasonId, 
		ISNULL(Tbl_DisconnectReason_MP.Description, Tbl_DisconnectReason_LP.Description) AS Description, 
		TblManagerSMSDGS.ManagerSMSDGSId 
	FROM 
		TblRequest 
		LEFT OUTER JOIN TblFogheToziDisconnect ON TblRequest.FogheToziDisconnectId = TblFogheToziDisconnect.FogheToziDisconnectId 
		LEFT OUTER JOIN TblMPRequest ON TblRequest.MPRequestId = TblMPRequest.MPRequestId 
		LEFT OUTER JOIN TblLPRequest ON TblRequest.LPRequestId = TblLPRequest.LPRequestId
		LEFT OUTER JOIN Tbl_Area ON TblRequest.AreaId = Tbl_Area.AreaId 
		LEFT OUTER JOIN Tbl_MPPost Tbl_MPPostFogheTozi ON TblFogheToziDisconnect.MPPostId = Tbl_MPPostFogheTozi.MPPostId 
		LEFT OUTER JOIN Tbl_MPPost ON TblMPRequest.MPPostId = Tbl_MPPost.MPPostId 
		LEFT OUTER JOIN Tbl_MPFeeder ON TblMPRequest.MPFeederId = Tbl_MPFeeder.MPFeederId 
		LEFT OUTER JOIN Tbl_LPPost ON TblLPRequest.LPPostId = Tbl_LPPost.LPPostId 
		LEFT OUTER JOIN Tbl_LPPost Tbl_LPPost_MP ON TblMPRequest.LPPostId = Tbl_LPPost_MP.LPPostId 
		LEFT OUTER JOIN Tbl_LPFeeder ON TblLPRequest.LPFeederId = Tbl_LPFeeder.LPFeederId 
		LEFT OUTER JOIN Tbl_FogheTozi ON TblFogheToziDisconnect.FogheToziId = Tbl_FogheTozi.FogheToziId 
		LEFT OUTER JOIN Tbl_DisconnectGroupSet Tbl_DisconnectGroupSet_LP ON TblLPRequest.DisconnectGroupSetId = Tbl_DisconnectGroupSet_LP.DisconnectGroupSetId 
		LEFT OUTER JOIN Tbl_DisconnectGroup Tbl_DisconnectGroup_LP ON Tbl_DisconnectGroupSet_LP.DisconnectGroupId = Tbl_DisconnectGroup_LP.DisconnectGroupId 
		LEFT OUTER JOIN Tbl_DisconnectGroupSet Tbl_DisconnectGroupSet_MP ON TblMPRequest.DisconnectGroupSetId = Tbl_DisconnectGroupSet_MP.DisconnectGroupSetId 
		LEFT OUTER JOIN Tbl_DisconnectGroup Tbl_DisconnectGroup_MP ON Tbl_DisconnectGroupSet_MP.DisconnectGroupId = Tbl_DisconnectGroup_MP.DisconnectGroupId 
		LEFT OUTER JOIN Tbl_DisconnectReason Tbl_DisconnectReason_LP ON TblLPRequest.DisconnectReasonId = Tbl_DisconnectReason_LP.DisconnectReasonId 
		LEFT OUTER JOIN Tbl_DisconnectReason Tbl_DisconnectReason_MP ON TblMPRequest.DisconnectReasonId = Tbl_DisconnectReason_MP.DisconnectReasonId 
		INNER JOIN TblManagerSMSDGS ON 
			ISNULL(Tbl_DisconnectGroupSet_MP.DisconnectGroupSetId,Tbl_DisconnectGroupSet_LP.DisconnectGroupSetId) = TblManagerSMSDGS.DisconnectGroupSetId
			OR ISNULL(Tbl_DisconnectReason_MP.DisconnectReasonId,Tbl_DisconnectReason_LP.DisconnectReasonId) = TblManagerSMSDGS.DisconnectReasonId
	WHERE 
		(TblRequest.DataEntryDT >=@SDate and TblRequest.DataEntryDT<=@EDate)		  
		AND TblRequest.EndJobStateId IN (4,5) 
		AND (LEN(TblRequest.DisconnectTime) = 5 AND TblRequest.DisconnectTime <> '__:__')
		AND (@AreaId = 99 OR (Tbl_Area.Server121Id = @Server121Id AND @IsCenter = 1) OR Tbl_Area.AreaId = @AreaId)
		AND 
		(
			ISNULL(TblMPRequest.DisconnectGroupSetId,TblLPRequest.DisconnectGroupSetId) IN
			(
				SELECT DisconnectGroupSetId FROM TblManagerSMSDGS
			)
			OR
			ISNULL(TblMPRequest.DisconnectReasonId,TblLPRequest.DisconnectReasonId) IN
			(
				SELECT DisconnectReasonId FROM TblManagerSMSDGS
			)
		)
		AND NOT TblManagerSMSDGS.ManagerSMSDGSId IN
		(
			SELECT ManagerSMSDGSId FROM TblManagerSMSDGSSend 
			WHERE RequestId = TblRequest.RequestId 
		)
		AND
		(
			ISNULL(ISNULL(TblMPRequest.IsWarmLine,TblLPRequest.IsWarmLine),0)=0
		)
		AND TblManagerSMSDGS.ManagerSMSDCId = @ManagerSMSDCId
		
	ORDER BY 
		TblRequest.DisconnectDT

	IF NOt EXISTS(SELECT RequestId FROM #tDGS)
	BEGIN
		DROP TABLE #tDGS
		return 0;
	END

	SET @lSendCount = 0
	DECLARE crDGS CURSOR FOR
		SELECT * FROM #tDGS

		OPEN crDGS
		
		DECLARE @lIsLoop AS bit	
		SET @lIsLoop = 1
		WHILE @lIsLoop = 1
		BEGIN
			FETCH NEXT
				FROM crDGS
				INTO 
					@RequestId , 
					@RequestNumber , 
					@MPPostName , 
					@FogheToziPostName , 
					@MPFeederName , 
					@LPPostName , 
					@LPFeederName , 
					@FogheToziDisconnectId , 
					@DisconnectPower , 
					@FogheToziType , 
					@DTDate , 
					@DTTime , 
					@DisconnectPowerFT , 
					@Area , 
					@IsDisconnectMPFeeder , 
					@IsTotalLPPostDisconnected , 
					@IsNotDisconnectFeeder , 
					@CurrentValue , 
					@IsFT , 
					@IsMP , 
					@IsLP , 
					@IsLight , 
					@DCGroupSetId , 
					@DCGroupSet , 
					@DCGroup , 
					@DisconnectReasonId,
					@Description,
					@ManagerSMSDGSId 

			if @@FETCH_STATUS = 0 
			BEGIN
				SET @Reason = ''
				IF NOT @DCGroupSet IS NULL
					SET @Reason = @DCGroupSet
				IF NOT @Description IS NULL
					SET @Reason = @Reason + ' Ê ‰Ê⁄ «‘ò«· ' + @Description
				ELSE IF NOT @FogheToziType IS NULL 
					SET @Reason = @FogheToziType

				IF @IsFT = 1
					SET @ReqType = N'›Êﬁ  Ê“Ì⁄'
				ELSE IF @IsMP = 1
					SET @ReqType = N'›‘«— „ Ê”ÿ'
				ELSE IF @IsLight = 1
					SET @ReqType = N'—Ê‘‰«ÌÌ „⁄«»—'
				ELSE IF @IsLP = 1
					SET @ReqType = N'›‘«— ÷⁄Ì›'
				ELSE
					SET @ReqType = N'ÃœÌœ'
					
				DECLARE @PostFeeder AS nvarchar(200)
				DECLARE @PostFeederLPF AS nvarchar(200)
				DECLARE @PostFeederLPP AS nvarchar(200)
				DECLARE @PostFeederMPF AS nvarchar(200)
				DECLARE @PostFeederMPP AS nvarchar(200)
				
				SET @PostFeeder = ''
				SET @PostFeederLPF = ''
				SET @PostFeederLPP = ''
				SET @PostFeederMPF = ''
				SET @PostFeederMPP = ''

				IF NOT @LPFeederName IS NULL
				BEGIN
					SET @PostFeeder = N' ›Ìœ— ›‘«— ÷⁄Ì› ' + @LPFeederName
					SET @PostFeederLPF = N' ›Ìœ— ›‘«— ÷⁄Ì› ' + @LPFeederName
				END
				IF NOT @LPPostName IS NULL
				BEGIN
					SET @PostFeeder = N' Å”   Ê“Ì⁄ ' + @LPPostName + @PostFeeder
					SET @PostFeederLPP = N' Å”   Ê“Ì⁄ ' + @LPPostName
				END
				IF NOT @MPFeederName IS NULL
				BEGIN
					SET @PostFeeder = N' ›Ìœ— ›‘«— „ Ê”ÿ ' + @MPFeederName + @PostFeeder
					SET @PostFeederMPF = N' ›Ìœ— ›‘«— „ Ê”ÿ '
				END
				IF NOT @MPPostName IS NULL
				BEGIN
					SET @PostFeeder = N' Å”  ›Êﬁ  Ê“Ì⁄ ' + @MPPostName + @PostFeeder
					SET @PostFeederMPP = N' Å”  ›Êﬁ  Ê“Ì⁄ '
				END
				
				SET @SMS = @SMSClause
				SET @SMS = Replace( @SMS,'ReqNo', ISNULL(@RequestNumber,0) )
				SET @SMS = Replace( @SMS,'ReqType', ISNULL(@ReqType,N'ø') )
				SET @SMS = Replace( @SMS,'Area', ISNULL(@Area,N'ø') )
				SET @SMS = Replace( @SMS,'Reason', ISNULL(@Reason,N'ø') )
				SET @SMS = Replace( @SMS,'Date', ISNULL(@DTDate,N'ø') )
				SET @SMS = Replace( @SMS,'Time', ISNULL(@DTTime,N'ø') )
				SET @SMS = Replace( @SMS,'DCGroup', ISNULL(@DCGroup,N'ø') )
				SET @SMS = Replace( @SMS,'PostFeeder', @PostFeeder )
				SET @SMS = Replace( @SMS,'PostFeederLPF', @PostFeederLPF )
				SET @SMS = Replace( @SMS,'PostFeederLPP', @PostFeederLPP )
				SET @SMS = Replace( @SMS,'PostFeederMPF', @PostFeederMPF )
				SET @SMS = Replace( @SMS,'PostFeederMPP', @PostFeederMPP )
				SET @SMS = Replace( @SMS,'CurrentDate', ISNULL(@CurrentDate,N''))
				SET @SMS = Replace( @SMS,'CurrentMonthDay', ISNULL(@CurrentMonthDay,N''))
				SET @SMS = Replace( @SMS,'CRLF', nchar(13) )
				
				DECLARE @Desc AS nvarchar(100)
				SET @Desc = 'ReqId=' + CAST(@RequestNumber AS nvarchar)
				EXEC spSendSMS @SMS, @ManagerMobile, @Desc, @RequestType, @AreaId
				
				DECLARE @ManagerSMSDGSSendId AS bigint
				
				INSERT INTO TblManagerSMSDGSSend (ManagerSMSDGSId,RequestId)
				VALUES (@ManagerSMSDGSId,@RequestId)
				
				SET @lSendCount = @lSendCount + 1
			END
			ELSE
				SET @lIsLoop = 0
		END
	CLOSE crDGS
	DEALLOCATE crDGS

	/*------------*/

	DROP TABLE #tDGS
	RETURN @lSendCount
GO


/* ------------------- ÅÌ«„ò «—”«· Â‘œ«— »—«Ì Œ«„Ê‘ÌÂ«Ì »«»—‰«„Â «Ì òÂ œ—’œÌ «“ “„«‰ ﬁÿ⁄ ¬‰ ê–‘ Â »«‘œ -------------------- */

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spSendSMSAlarmForTamir]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spSendSMSAlarmForTamir]
GO

CREATE PROCEDURE [dbo].[spSendSMSAlarmForTamir] 
	@ManagerSMSDCId as INT,
	@ManagerMobile AS varchar(20), 
	@IsSetad AS bit, 
	@IsCenter AS bit, 
	@Server121Id AS int, 
	@AreaId AS int 
AS
	DECLARE @lDate as datetime = dateadd(day,-1,getdate())
	DECLARE @SMS AS nvarchar(2000)
	SET @SMS = ''

	SELECT @SMS = ConfigText FROM Tbl_Config WHERE ConfigName = 'SMSSendAlarmForTamir'
	IF @SMS = '' RETURN 

	DECLARE @lPercent AS INT = 75
	SELECT @lPercent = ISNULL(ConfigValue,75) FROM Tbl_Config WHERE ConfigName = 'SendAlarmForTamirPercent'
	IF @lPercent = 0 RETURN

	SELECT 
		TblRequest.RequestId
	INTO 
		#tmpReq
	FROM 
		TblRequest
		LEFT JOIN TblMPRequest ON TblRequest.MPRequestId = TblMPRequest.MPRequestId
		LEFT JOIN TblLPRequest ON TblRequest.LPRequestId = TblLPRequest.LPRequestId
		LEFT JOIN Tbl_Area ON TblRequest.AreaId = Tbl_Area.AreaId 			
	WHERE 
		DATEDIFF(MINUTE, TblRequest.TamirDisconnectFromDT, TblRequest.TamirDisconnectToDT) > 0
		AND TblRequest.DisconnectDT >= @lDate
		AND TblRequest.IsTamir = 1
		AND ISNULL(TblMPRequest.IsWarmLine,0) = 0 AND ISNULL(TblLPRequest.IsWarmLine,0) = 0 
		AND TblRequest.EndJobStateId IN (5)
		AND DATEDIFF(MINUTE, TblRequest.DisconnectDT, GETDATE()) >= ((@lPercent * DATEDIFF(MINUTE, TblRequest.TamirDisconnectFromDT, TblRequest.TamirDisconnectToDT)) / 100)
		AND NOT TblRequest.RequestId IN (SELECT RequestId FROM TblManagerSMSAlarmSended WHERE ManagerSMSDCId = @ManagerSMSDCId)
		AND 
		(
			@IsSetad = 1 
			OR (@IsCenter = 1 AND Tbl_Area.Server121Id = @Server121Id) 
			OR (@IsCenter = 0 AND TblRequest.AreaId = @AreaId)
		)
		AND (NOT TblRequest.MPRequestId IS NULL OR NOT TblRequest.LPRequestId IS NULL OR NOT TblRequest.FogheToziDisconnectId IS NULL)

	DECLARE @lRequestId as bigint
	DECLARE RequestList CURSOR FOR
		SELECT * FROM #tmpReq

		OPEN RequestList
		
		DECLARE @lIsLoop AS bit	
		SET @lIsLoop = 1
		WHILE @lIsLoop = 1
		BEGIN
			FETCH NEXT
				FROM RequestList
				INTO @lRequestId

			if @@FETCH_STATUS = 0 
			BEGIN
				DECLARE @lTamirRequestId as bigint = -1
				DECLARE @lMPRequestId as bigint
				DECLARE @lLPRequestId as bigint
				DECLARE @lFogheToziDisconnectId as bigint
				DECLARE @lRequestNumber AS bigint
				DECLARE @lAddress AS nVarchar(500) = ''
				DECLARE @lArea AS nVarchar(500) = ''
				DECLARE @lTamirDisconnectFromTime as varchar(5) = ''
				DECLARE @lTamirDisconnectToTime as varchar(5) = ''
				DECLARE @lDisconnectTime as varchar(5) = ''
				DECLARE @lConnectTime as varchar(5) = ''
				DECLARE @lTamirInterval as int
				DECLARE @lRequestBy as nvarchar(300) = ''
				DECLARE @lPeymankar as nvarchar(300) = ''
				DECLARE @lEkip as nvarchar(300) = ''
				DECLARE @lTamirNetworkType AS nvarchar(200) = ''
				
				SELECT 
					@lMPRequestId = ISNULL(MPRequestId,-1),
					@lLPRequestId = ISNULL(LPRequestId,-1),
					@lFogheToziDisconnectId = ISNULL(FogheToziDisconnectId,-1),
					@lRequestNumber = RequestNumber,
					@lAddress = Address,
					@lArea = Tbl_Area.Area,
					@lTamirDisconnectFromTime = TamirDisconnectFromTime,
					@lTamirDisconnectToTime = TamirDisconnectToTime,
					@lDisconnectTime = DisconnectTime,
					@lConnectTime = dbo.GetTime(DATEADD(minute,DATEDIFF(MINUTE, TblRequest.TamirDisconnectFromDT, TblRequest.TamirDisconnectToDT),DisconnectDT))
				FROM
					TblRequest
					INNER JOIN Tbl_Area ON TblRequest.AreaId = Tbl_Area.AreaId
				WHERE
					RequestId = @lRequestId
				
				IF @lMPRequestId > -1
					SET @lTamirNetworkType = N'›‘«— „ Ê”ÿ'
				ELSE IF @lLPRequestId > -1
					SET @lTamirNetworkType = N'›‘«— ÷⁄Ì›'
				ELSE IF @lFogheToziDisconnectId > -1
					SET @lTamirNetworkType = N'›Êﬁ  Ê“Ì⁄'
					
				SELECT 
					@lTamirRequestId = TblTamirRequest.TamirRequestId,
					@lRequestBy = CASE WHEN TblTamirRequest.IsRequestByPeymankar = 1 THEN N'ÅÌ„«‰ò«—' WHEN TblTamirRequest.TamirNetworkTypeId = 1 THEN N'›Êﬁ  Ê“Ì⁄' ELSE N'‘—ò   Ê“Ì⁄' END, 
					@lPeymankar = ISNULL(Tbl_Peymankar.PeymankarName, TblTamirRequest.Peymankar)
				FROM 
					TblTamirRequest
					LEFT JOIN TblTamirRequestConfirm ON TblTamirRequest.TamirRequestId = TblTamirRequestConfirm.TamirRequestId
					INNER JOIN TblRequest ON ISNULL(TblTamirRequestConfirm.RequestId, TblTamirRequest.EmergencyRequestId) = TblRequest.RequestId
					LEFT JOIN Tbl_Peymankar ON TblTamirRequest.PeymankarId = Tbl_Peymankar.PeymankarId
				WHERE
					TblRequest.RequestId = @lRequestId
				
				IF ISNULL(@lPeymankar,'') = ''
				BEGIN
					SELECT TOP 1
						@lPeymankar = ISNULL(TblEkipProfile.EkipProfileName,ISNULL(Tbl_Master.Name,''))
					FROM
						TblBazdid
						LEFT JOIN TblEkipProfile ON TblBazdid.EkipProfileId = TblEkipProfile.EkipProfileId
						LEFT JOIN Tbl_Master ON TblBazdid.MasterId = Tbl_Master.MasterId
					WHERE
						RequestId = @lRequestId
					ORDER BY 
						EzamDT DESC
				END
				
				IF ISNULL(@lRequestBy,'') = ''
					SET @lRequestBy = N'‘—ò   Ê“Ì⁄'
					
				DECLARE @SMSBody AS nvarchar(2000) = @SMS
				
				SET @SMSBody = Replace( @SMSBody,'RequestNumber', ISNULL(@lRequestNumber,0))
				SET @SMSBody = Replace( @SMSBody,'Address', ISNULL(@lAddress,'ø'))
				SET @SMSBody = Replace( @SMSBody,'Area', ISNULL(@lArea,'ø'))
				SET @SMSBody = Replace( @SMSBody,'DisconnectTime', ISNULL(@lDisconnectTime,'ø'))
				SET @SMSBody = Replace( @SMSBody,'ConnectTime', ISNULL(@lConnectTime,'ø'))
				SET @SMSBody = Replace( @SMSBody,'TamirNetworkType', ISNULL(@lTamirNetworkType,'ø'))
				SET @SMSBody = Replace( @SMSBody,'EkipName', ISNULL(@lPeymankar,'ø'))
				SET @SMSBody = Replace( @SMSBody,'RequestBy', ISNULL(@lRequestBy,'ø'))
				
				DECLARE @Desc AS nvarchar(100)
				SET @Desc = 'ReqNo=' + Cast(@lRequestNumber AS nvarchar)
				EXEC spSendSMS @SMSBody , @ManagerMobile, @Desc, 'SendAlarmTamir', @AreaId
				
				INSERT INTO TblManagerSMSAlarmSended VALUES(@ManagerSMSDCId,@lRequestId)
											
			END
			ELSE
				SET @lIsLoop = 0
		END
	CLOSE RequestList
	DEALLOCATE RequestList
		
	DROP TABLE #tmpReq
GO

/* ------------------- «—”«· ÅÌ«„ò Ì«œ¬Ê—Ì  ⁄ÌÌ‰ Ê÷⁄Ì  Œ«„Ê‘ÌÂ«Ì œ—«‰ Ÿ«—  «ÌÌœ -------------------- */

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spSendSMSReminderWaitConfirm]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spSendSMSReminderWaitConfirm]
GO

CREATE PROCEDURE [dbo].[spSendSMSReminderWaitConfirm] 
	@ManagerSMSDCId as INT,
	@ManagerMobile AS varchar(20), 
	@IsSetad AS bit, 
	@IsCenter AS bit, 
	@AreaId AS int 
AS
	DECLARE @lDate as datetime = dateadd(day,-1,getdate())
	DECLARE @lCurrentDatePersian as varchar(10)
	DECLARE @IsSendWaitSMSForWarmLine as bit
	SET @IsSendWaitSMSForWarmLine = 1
	select @lCurrentDatePersian = dbo.mtosh(getdate())
	
	DECLARE @SMS AS nvarchar(2000)
	SET @SMS = ''

	SELECT @SMS = ConfigText FROM Tbl_Config WHERE ConfigName = 'SendSMSReminderWaitConfirm'
	IF @SMS = '' RETURN 
	
	SELECT @IsSendWaitSMSForWarmLine = ConfigValue FROM Tbl_Config WHERE ConfigName = 'IsSendWaitSMSForWarmLine'

	DECLARE @lTime AS INT = 24
	SELECT @lTime = ISNULL(ConfigValue,0) FROM Tbl_Config WHERE ConfigName = 'SendSMSForWaitConfirmTime'
	IF @lTime = 0 RETURN

	SELECT 
		TblTamirRequest.TamirRequestId
	INTO 
		#tmpTReq
	FROM 
		TblTamirRequest
	WHERE 
		TblTamirRequest.DisconnectDatePersian >= @lCurrentDatePersian
		AND DATEDIFF(HOUR,GetDate(),TblTamirRequest.DisconnectDT) <= @lTime
		AND TblTamirRequest.TamirRequestStateId in (1,2)
		AND NOT TblTamirRequest.TamirRequestId IN (SELECT TamirRequestId FROM TblManagerSMSTamirReminderSended WHERE ManagerSMSDCId = @ManagerSMSDCId)
		AND 
		(
			(TblTamirRequest.TamirRequestStateId = 1 AND @AreaId <> 99 AND @IsCenter = 1)
			OR (TblTamirRequest.TamirRequestStateId = 2 AND @AreaId = 99 AND @IsSetad = 1)
		)
		AND (TblTamirRequest.IsWarmLine = 0 OR @IsSendWaitSMSForWarmLine = 1)

	DECLARE @lTamirRequestId as bigint
	DECLARE TamirRequestList CURSOR FOR
		SELECT * FROM #tmpTReq

		OPEN TamirRequestList
		
		DECLARE @lIsLoop AS bit	
		SET @lIsLoop = 1
		WHILE @lIsLoop = 1
		BEGIN
			FETCH NEXT
				FROM TamirRequestList
				INTO @lTamirRequestId

			if @@FETCH_STATUS = 0 
			BEGIN
				DECLARE @lTamirRequestNo AS bigint
				DECLARE @lTamirNetworkType AS nVarchar(500) = ''
				DECLARE @lArea AS nVarchar(500) = ''
				DECLARE @lTamirDisconnectDateFrom as varchar(10) = ''
				DECLARE @lTamirDisconnectDateTo as varchar(10) = ''
				DECLARE @lTamirDisconnectTimeFrom as varchar(5) = ''
				DECLARE @lTamirDisconnectTimeTo as varchar(5) = ''
				DECLARE @lStatus as nvarchar(200) = ''
				
				SELECT 
					@lTamirRequestNo = TamirRequestNo,
					@lTamirNetworkType = Tbl_TamirNetworkType.TamirNetworkType,
					@lArea = Tbl_Area.Area,
					@lTamirDisconnectDateFrom = TblTamirRequest.DisconnectDatePersian,
					@lTamirDisconnectDateTo = TblTamirRequest.ConnectDatePersian,
					@lTamirDisconnectTimeFrom = TblTamirRequest.DisconnectTime,
					@lTamirDisconnectTimeTo = TblTamirRequest.ConnectTime,
					@lStatus = Tbl_TamirRequestState.TamirRequestState
				FROM
					TblTamirRequest
					INNER JOIN Tbl_Area ON TblTamirRequest.AreaId = Tbl_Area.AreaId
					INNER JOIN Tbl_TamirNetworkType ON TblTamirRequest.TamirNetworkTypeId = Tbl_TamirNetworkType.TamirNetworkTypeId
					INNER JOIN Tbl_TamirRequestState ON TblTamirRequest.TamirRequestStateId = Tbl_TamirRequestState.TamirRequestStateId
				WHERE
					TblTamirRequest.TamirRequestId = @lTamirRequestId
					
				DECLARE @SMSBody AS nvarchar(2000) = @SMS
				
				SET @SMSBody = Replace( @SMSBody,'TamirRequestNo', ISNULL(@lTamirRequestNo,0))
				SET @SMSBody = Replace( @SMSBody,'TamirNetworkType', ISNULL(@lTamirNetworkType,'ø'))
				SET @SMSBody = Replace( @SMSBody,'TamirDisconnectDateFrom', ISNULL(@lTamirDisconnectDateFrom,'ø'))
				SET @SMSBody = Replace( @SMSBody,'TamirDisconnectDateTo', ISNULL(@lTamirDisconnectDateTo,'ø'))
				SET @SMSBody = Replace( @SMSBody,'TamirDisconnectTimeFrom', ISNULL(@lTamirDisconnectTimeFrom,'ø'))
				SET @SMSBody = Replace( @SMSBody,'TamirDisconnectTimeTo', ISNULL(@lTamirDisconnectTimeTo,'ø'))
				SET @SMSBody = Replace( @SMSBody,'Area', ISNULL(@lArea,'ø'))
				SET @SMSBody = Replace( @SMSBody,'TamirRequestState', ISNULL(@lStatus,'ø'))
				SET @SMSBody = Replace( @SMSBody,'SendSMSForWaitConfirmTime', ISNULL(@lTime,'ø'))
				
				DECLARE @Desc AS nvarchar(100)
				SET @Desc = 'TamirReqNo=' + Cast(@lTamirRequestNo AS nvarchar)
				EXEC spSendSMS @SMSBody , @ManagerMobile, @Desc, 'SendReminder', @AreaId
				
				INSERT INTO TblManagerSMSTamirReminderSended VALUES(@lTamirRequestId,@ManagerSMSDCId)
											
			END
			ELSE
				SET @lIsLoop = 0
		END
	CLOSE TamirRequestList
	DEALLOCATE TamirRequestList
		
	DROP TABLE #tmpTReq
GO


/* ----------------------------  «—”«· ÅÌ«„ò ⁄Êœ  Œ«„Ê‘Ì -------------------- */

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spSendSMSTamirReturned]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spSendSMSTamirReturned]
GO

CREATE PROCEDURE [dbo].[spSendSMSTamirReturned] 
	@ManagerSMSDCId as INT,
	@ManagerMobile AS varchar(20), 
	@AreaId AS int 
AS
	DECLARE @lDate as VARCHAR(10) = ''
	SET @lDate = dbo.mtosh(getdate())
	
	DECLARE @SMS AS nvarchar(2000)
	SET @SMS = ''

	SELECT @SMS = ConfigText FROM Tbl_Config WHERE ConfigName = 'SMSSendTamirReturned'
	IF @SMS = '' RETURN 

	SELECT 
		TblTamirRequest.TamirRequestId
	INTO 
		#tmpTReq
	FROM 
		TblTamirRequest
	WHERE 
		TblTamirRequest.DisconnectDatePersian >= @lDate
		AND TblTamirRequest.TamirRequestStateId = 0
		AND TblTamirRequest.IsReturned = 1
		AND NOT TblTamirRequest.TamirRequestId IN (SELECT TamirRequestId FROM TblManagerSMSTamirReturnSended WHERE ManagerSMSDCId = @ManagerSMSDCId)
		AND TblTamirRequest.AreaId = @AreaId

	DECLARE @lTamirRequestId as bigint
	DECLARE TamirRequestList CURSOR FOR
		SELECT * FROM #tmpTReq

		OPEN TamirRequestList

		DECLARE @lIsLoop AS bit	
		SET @lIsLoop = 1
		WHILE @lIsLoop = 1
		BEGIN
			FETCH NEXT
				FROM TamirRequestList
				INTO @lTamirRequestId

			if @@FETCH_STATUS = 0 
			BEGIN
				DECLARE @lTamirRequestNo AS bigint
				DECLARE @lTamirNetworkType AS nVarchar(500) = ''
				DECLARE @lArea AS nVarchar(500) = ''
				DECLARE @lTamirDisconnectDateFrom as varchar(10) = ''
				DECLARE @lTamirDisconnectDateTo as varchar(10) = ''
				DECLARE @lTamirDisconnectTimeFrom as varchar(5) = ''
				DECLARE @lTamirDisconnectTimeTo as varchar(5) = ''
				DECLARE @lReturnDesc as nvarchar(200) = ''
				
				SELECT 
					@lTamirRequestNo = TamirRequestNo,
					@lTamirNetworkType = Tbl_TamirNetworkType.TamirNetworkType,
					@lArea = Tbl_Area.Area,
					@lTamirDisconnectDateFrom = TblTamirRequest.DisconnectDatePersian,
					@lTamirDisconnectDateTo = TblTamirRequest.ConnectDatePersian,
					@lTamirDisconnectTimeFrom = TblTamirRequest.DisconnectTime,
					@lTamirDisconnectTimeTo = TblTamirRequest.ConnectTime,
					@lReturnDesc = TblTamirRequest.ReturnDesc
				FROM
					TblTamirRequest
					INNER JOIN Tbl_Area ON TblTamirRequest.AreaId = Tbl_Area.AreaId
					INNER JOIN Tbl_TamirNetworkType ON TblTamirRequest.TamirNetworkTypeId = Tbl_TamirNetworkType.TamirNetworkTypeId
				WHERE
					TblTamirRequest.TamirRequestId = @lTamirRequestId
					
				DECLARE @SMSBody AS nvarchar(2000) = @SMS
				
				SET @SMSBody = Replace( @SMSBody,'TamirRequestNo', ISNULL(@lTamirRequestNo,0))
				SET @SMSBody = Replace( @SMSBody,'TamirNetworkType', ISNULL(@lTamirNetworkType,'ø'))
				SET @SMSBody = Replace( @SMSBody,'TamirDisconnectDateFrom', ISNULL(@lTamirDisconnectDateFrom,'ø'))
				SET @SMSBody = Replace( @SMSBody,'TamirDisconnectDateTo', ISNULL(@lTamirDisconnectDateTo,'ø'))
				SET @SMSBody = Replace( @SMSBody,'TamirDisconnectTimeFrom', ISNULL(@lTamirDisconnectTimeFrom,'ø'))
				SET @SMSBody = Replace( @SMSBody,'TamirDisconnectTimeTo', ISNULL(@lTamirDisconnectTimeTo,'ø'))
				SET @SMSBody = Replace( @SMSBody,'Area', ISNULL(@lArea,'ø'))
				SET @SMSBody = Replace( @SMSBody,'ReturnDesc', ISNULL(@lReturnDesc,'ø'))
				
				DECLARE @Desc AS nvarchar(100)
				SET @Desc = 'TamirReqNo=' + Cast(@lTamirRequestNo AS nvarchar)
				EXEC spSendSMS @SMSBody , @ManagerMobile, @Desc, 'SendReturned', @AreaId
				
				INSERT INTO TblManagerSMSTamirReturnSended VALUES(@lTamirRequestId,@ManagerSMSDCId)
											
			END
			ELSE
				SET @lIsLoop = 0
		END
	CLOSE TamirRequestList
	DEALLOCATE TamirRequestList
		
	DROP TABLE #tmpTReq
GO


/* ----------------------------  «—”«· ÅÌ«„ò ÿÊ·«‰Ì  — ‘œ‰ “„«‰ Œ«„Ê‘Ì «“ “„«‰ ÅÌ‘ »Ì‰Ì ‘œÂ »«»—‰«„Â -------------------- */
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spSendSMSTamirLongDC]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spSendSMSTamirLongDC]
GO

CREATE PROCEDURE [dbo].[spSendSMSTamirLongDC] 
	@ManagerSMSDCId as INT,
	@ManagerMobile AS varchar(20), 
	@IsSetad AS bit, 
	@IsCenter AS bit, 
	@Server121Id AS int, 
	@AreaId AS int,
	@SendSMSForLongTamirTime as int
AS
	DECLARE @lDate as VARCHAR(10) = ''
	SET @lDate = dbo.mtosh(getdate())
	
	DECLARE @SMS AS nvarchar(2000)
	SET @SMS = ''

	SELECT @SMS = ConfigText FROM Tbl_Config WHERE ConfigName = 'SMSSendTamirLongDC'
	IF @SMS = '' RETURN 

	SELECT 
		TblRequest.RequestId
	INTO 
		#tmpReq
	FROM 
		TblRequest
		LEFT JOIN TblMPRequest ON TblRequest.MPRequestId = TblMPRequest.MPRequestId
		LEFT JOIN TblLPRequest ON TblRequest.LPRequestId = TblLPRequest.LPRequestId
		inner join Tbl_Area on TblRequest.AreaId = Tbl_Area.AreaId
	WHERE 
		(DATEDIFF(MINUTE, TblRequest.TamirDisconnectFromDT, TblRequest.TamirDisconnectToDT) + @SendSMSForLongTamirTime) < DATEDIFF(MINUTE,TblRequest.DisconnectDT,GETDATE())
		AND TblRequest.DisconnectDatePersian >= @lDate
		AND TblRequest.IsTamir = 1
		AND ISNULL(TblMPRequest.IsWarmLine,0) = 0 AND ISNULL(TblLPRequest.IsWarmLine,0) = 0 
		AND TblRequest.EndJobStateId IN (5)
		AND (NOT TblRequest.MPRequestId IS NULL OR NOT TblRequest.LPRequestId IS NULL OR NOT TblRequest.FogheToziDisconnectId IS NULL)
		AND NOT TblRequest.RequestId IN (select TblManagerSMSTamirLongSended.RequestId from TblManagerSMSTamirLongSended where ManagerSMSDCId = @ManagerSMSDCId)
		AND 
		(
			@IsSetad = 1 
			OR (@IsCenter = 1 AND Tbl_Area.Server121Id = @Server121Id) 
			OR (@IsCenter = 0 AND TblRequest.AreaId = @AreaId)
		)


	DECLARE @lRequestId as bigint
	DECLARE TamirLongDCList CURSOR FOR
		SELECT * FROM #tmpReq

		OPEN TamirLongDCList

		DECLARE @lIsLoop AS bit	
		SET @lIsLoop = 1
		WHILE @lIsLoop = 1
		BEGIN
			FETCH NEXT
				FROM TamirLongDCList
				INTO @lRequestId

			if @@FETCH_STATUS = 0 
			BEGIN
				DECLARE @lArea AS nvarchar(500) = ''
				DECLARE @lTamirDisconnectDateFrom as varchar(10) = ''
				DECLARE @lTamirDisconnectDateTo as varchar(10) = ''
				DECLARE @lTamirDisconnectTimeFrom as varchar(5) = ''
				DECLARE @lTamirDisconnectTimeTo as varchar(5) = ''
				DECLARE @lRequestNumber as varchar(20) = ''
				DECLARE @lTamirDisconnectInterval as int = 0
				declare @lNetwork as nvarchar(500) = ''
				declare @lLongDCInterval as int = 0
				
				SELECT 
					@lRequestNumber = RequestNumber,
					@lArea = Tbl_Area.Area,
					@lTamirDisconnectDateFrom = TblRequest.TamirDisconnectFromDatePersian,
					@lTamirDisconnectDateTo = TblRequest.TamirDisconnectToDatePersian,
					@lTamirDisconnectTimeFrom = TblRequest.TamirDisconnectFromTime,
					@lTamirDisconnectTimeTo = TblRequest.TamirDisconnectToTime,
					@lTamirDisconnectInterval = DATEDIFF(MINUTE,TblRequest.TamirDisconnectFromDT,TblRequest.TamirDisconnectToDT),
					@lLongDCInterval = (DATEDIFF(MINUTE,TblRequest.DisconnectDT,GetDate()) - @SendSMSForLongTamirTime) + 1,
					@lNetwork = case when TblRequest.IsFogheToziRequest = 1 then
									N'›Êﬁ  Ê“Ì⁄'
									when TblRequest.IsMPRequest = 1 then
									N'›‘«— „ Ê”ÿ'
									when TblRequest.IsLPRequest = 1 then
									N'›‘«— ÷⁄Ì›'
						END
				FROM
					TblRequest
					INNER JOIN Tbl_Area ON TblRequest.AreaId = Tbl_Area.AreaId
				WHERE
					TblRequest.RequestId = @lRequestId
					
				DECLARE @SMSBody AS nvarchar(2000) = @SMS
				
				SET @SMSBody = Replace( @SMSBody,'RequestNumber', ISNULL(@lRequestNumber,0))
				SET @SMSBody = Replace( @SMSBody,'NetworkType', ISNULL(@lNetwork,'ø'))
				SET @SMSBody = Replace( @SMSBody,'TamirDisconnectDateFrom', ISNULL(@lTamirDisconnectDateFrom,'ø'))
				SET @SMSBody = Replace( @SMSBody,'TamirDisconnectDateTo', ISNULL(@lTamirDisconnectDateTo,'ø'))
				SET @SMSBody = Replace( @SMSBody,'TamirDisconnectTimeFrom', ISNULL(@lTamirDisconnectTimeFrom,'ø'))
				SET @SMSBody = Replace( @SMSBody,'TamirDisconnectTimeTo', ISNULL(@lTamirDisconnectTimeTo,'ø'))
				SET @SMSBody = Replace( @SMSBody,'Area', ISNULL(@lArea,'ø'))
				SET @SMSBody = Replace( @SMSBody,'TamirDisconnectInterval', ISNULL(@lTamirDisconnectInterval,0))
				SET @SMSBody = Replace( @SMSBody,'LongDCInterval', ISNULL(@lLongDCInterval,0))
				
				DECLARE @Desc AS nvarchar(100)
				SET @Desc = 'ReqNo=' + Cast(@lRequestNumber AS nvarchar)
				EXEC spSendSMS @SMSBody , @ManagerMobile, @Desc, 'SendLongDC', @AreaId
				
				INSERT INTO TblManagerSMSTamirLongSended VALUES(@lRequestId,@ManagerSMSDCId)
											
			END
			ELSE
				SET @lIsLoop = 0
		END
	CLOSE TamirLongDCList
	DEALLOCATE TamirLongDCList
		
	DROP TABLE #tmpReq
GO



IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spCheckNewDCManager]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spCheckNewDCManager]
GO
CREATE PROCEDURE [dbo].[spCheckNewDCManager]
AS
	DECLARE @ManagerSMSDCId as int
	DECLARE @AreaId as int
	DECLARE @ManagerMobile as varchar(20)
	DECLARE @IsLPNotTamir as bit
	DECLARE @LPNotTamirMinutes as int
	DECLARE @IsLPTamir as bit
	DECLARE @LPTamirMinutes as int
	DECLARE @IsMPNotTamir as bit
	DECLARE @MPNotTamirMinutes as int
	DECLARE @IsMPTamir as bit
	DECLARE @MPTamirMinutes as int
	DECLARE @IsFT as bit
	DECLARE @FTMinutes as int
	DECLARE @IsCenter as bit
	DECLARE @IsSetad as bit
	DECLARE @Server121Id as int
	DECLARE @IsTransFaultSendSMS as bit
	DECLARE @IsLPNotTamirSendSMSAfterConnect as bit
	DECLARE @IsLPTamirSendSMSAfterConnect as bit
	DECLARE @IsMPNotTamirSendSMSAfterConnect as bit
	DECLARE @IsMPTamirSendSMSAfterConnect as bit
	DECLARE @IsFTSendSMSAfterConnect as bit
	DECLARE @IsMPTamir_MPFDC as bit
	DECLARE @IsMPNotTamir_MPFDC as bit
	DECLARE @IsTamir_Confirm as bit
	DECLARE @IsTamir_NotConfirm as bit
	DECLARE @IsTamir_ConfirmLP as bit
	DECLARE @IsTamir_NotConfirmLP as bit
	DECLARE @IsTamir_ConfirmFT as bit
	DECLARE @IsTamir_NotConfirmFT as bit
	DECLARE @LastTamirEventId AS bigint
	DECLARE @TamirRequestId AS bigint
	DECLARE @TamirRequestStateId AS int
	DECLARE @TamirRequestAreaId AS int
	DECLARE @IsDGSRequest AS bit
	DECLARE @MPNotTamirMinutes_MPFDC As int
	DECLARE @MPTamirMinutes_MPFDC As int
	
	DECLARE @IsNewRequest as bit
	DECLARE @NewRequestMinutes as int
	DECLARE @IsLPNotTamirAll as bit
	DECLARE @LPNotTamirAllMinutes as int
	DECLARE @IsLPTamirAll as bit
	DECLARE @LPTamirAllMinutes as int
	DECLARE @IsLPTamirSingle as bit
	DECLARE @LPTamirSingleMinutes as int
	DECLARE @IsLPNotTamirSingle as bit
	DECLARE @LPNotTamirSingleMinutes as int
	DECLARE @IsMPNotTamirAll as bit
	DECLARE @MPNotTamirAllMinutes as int
	DECLARE @IsMPTamirAll as bit
	DECLARE @MPTamirAllMinutes as int
	DECLARE @IsMPTamirSingle as bit
	DECLARE @MPTamirSingleMinutes as int
	DECLARE @IsMPNotTamirSingle as bit
	DECLARE @MPNotTamirSingleMinutes as int
	
	DECLARE @IsSendAlarmForTamir as bit
	DECLARE @IsSendSMSForWaitConfirm as bit
	DECLARE @IsTamirReturned as bit
	DECLARE @IsSendSMSForLongTamirDC as bit
	DECLARE @lSendSMSForLongTamirTime as int
	--------------<omid>
	DECLARE @IsNewRequestMP as bit
	DECLARE @NewRequestMPMinutes as int
	--------------</omid>
	DECLARE @LastSMSId as bigint
	select @LastSMSId = MAX(SMSId) from Tbl_SMS
	
	/* -- Tamir Request Check -- */
	SET @LastTamirEventId = 0

	IF NOT EXISTS(SELECT * FROM Tbl_Config WHERE ConfigName = 'smsLastTamirEventId')
		INSERT INTO Tbl_Config (ConfigId, ConfigName, ConfigValue)
		VALUES (700, 'smsLastTamirEventId', N'0')
	--print 'spCheckNewDCManager Start ' + convert(nvarchar(100),getdate(),21)
	
	SELECT @LastTamirEventId = Cast(ConfigValue AS bigint)
	FROM Tbl_Config
	WHERE ConfigName = 'smsLastTamirEventId'
		declare @MaxEventId as bigint
	SELECT TOP 1 
		@MaxEventId= Tbl_EventLogCenter.EventId		
	FROM 
		Tbl_EventLogCenter 					
	ORDER BY 
		Tbl_EventLogCenter.EventId desc						  
 
	SELECT 
		LastTamirEventId = Tbl_EventLogCenter.EventId, 
		TamirRequestId = TblTamirRequest.TamirRequestId, 
		TamirRequestStateId = TblTamirRequest.TamirRequestStateId, 
		TamirRequestAreaId = TblTamirRequest.AreaId 
	INTO #tmpTamirConfrim
	FROM 
		Tbl_EventLogCenter 
		INNER JOIN TblTamirRequest ON Tbl_EventLogCenter.PrimaryKeyId = TblTamirRequest.TamirRequestId 
	WHERE 
		Tbl_EventLogCenter.TableName = 'TblTamirRequest' 
		AND (DATEDIFF(mi, GETDATE(), Tbl_EventLogCenter.DataEntryDT) >= -600) 
		AND EventId > @LastTamirEventId
		AND TblTamirRequest.TamirRequestStateId IN (3,8)
	ORDER BY 
		Tbl_EventLogCenter.EventId
		
	/*  -- End Tamir Request Check -- */
    --print 'spCheckNewDCManager Create Tmp ' + convert(nvarchar(100),getdate(),21)
    SELECT 
			TblManagerSMSDC.ManagerSMSDCId,
			TblManagerSMSDC.AreaId, 
			TblManagerSMSDC.ManagerMobile, 
			TblManagerSMSDC.IsLPNotTamir, 
			TblManagerSMSDC.LPNotTamirMinutes, 
			TblManagerSMSDC.IsLPTamir, 
			TblManagerSMSDC.LPTamirMinutes, 
			TblManagerSMSDC.IsMPNotTamir,
			TblManagerSMSDC.MPNotTamirMinutes, 
			TblManagerSMSDC.IsMPTamir, 
			TblManagerSMSDC.MPTamirMinutes, 
			TblManagerSMSDC.IsFT, 
			TblManagerSMSDC.FTMinutes, 
			Tbl_Area.IsCenter, 
			Tbl_Area.IsSetad, 
			Tbl_Area.Server121Id, 
			TblManagerSMSDC.IsTransFaultSendSMS,
			TblManagerSMSDC.IsLPNotTamirSendSMSAfterConnect,
			TblManagerSMSDC.IsLPTamirSendSMSAfterConnect,
			TblManagerSMSDC.IsMPNotTamirSendSMSAfterConnect,
			TblManagerSMSDC.IsMPTamirSendSMSAfterConnect,
			TblManagerSMSDC.IsFTSendSMSAfterConnect,
			TblManagerSMSDC.IsMPNotTamir_MPFDC,
			TblManagerSMSDC.IsMPTamir_MPFDC,
			TblManagerSMSDC.IsTamir_Confirm,
			TblManagerSMSDC.IsTamir_NotConfirm,
			TblManagerSMSDC.IsDGSRequest,
			ISNULL(TblManagerSMSDC.MPNotTamirMinutes_MPFDC,0) AS MPNotTamirMinutes_MPFDC, 
			ISNULL(TblManagerSMSDC.MPTamirMinutes_MPFDC,0) AS MPTamirMinutes_MPFDC,
			IsNewRequest, 
			NewRequestMinutes, 
			IsLPNotTamirAll, 
			LPNotTamirAllMinutes, 
			IsLPTamirAll, 
			LPTamirAllMinutes, 
			IsLPTamirSingle, 
			LPTamirSingleMinutes, 
			IsLPNotTamirSingle, 
			LPNotTamirSingleMinutes, 
			IsMPNotTamirAll, 
			MPNotTamirAllMinutes, 
			IsMPTamirAll, 
			MPTamirAllMinutes, 
			IsMPTamirSingle, 
			MPTamirSingleMinutes, 
			IsMPNotTamirSingle, 
			MPNotTamirSingleMinutes,
			TblManagerSMSDC.IsTamir_ConfirmLP,
			TblManagerSMSDC.IsTamir_NotConfirmLP,
			TblManagerSMSDC.IsTamir_ConfirmFT,
			TblManagerSMSDC.IsTamir_NotConfirmFT, 
			TblManagerSMSDC.IsSendAlarmForTamir,
			TblManagerSMSDC.IsSendSMSForWaitConfirm,
			TblManagerSMSDC.IsTamirReturned,
			TblManagerSMSDC.IsSendSMSForLongTamirDC,
			ISNULL(TblManagerSMSDC.SendSMSForLongTamirTime,0) AS SendSMSForLongTamirTime,
			ISNULL(TblManagerSMSDC.IsNewRequestMP,0) AS IsNewRequestMP,
			ISNULL(TblManagerSMSDC.NewRequestMPMinutes,0) AS NewRequestMPMinutes
		INTO 
			#TMPSMSDC
		FROM 
			Tbl_Area 
			INNER JOIN TblManagerSMSDC ON Tbl_Area.AreaId = TblManagerSMSDC.AreaId 
		WHERE 
			TblManagerSMSDC.IsActive = 1
   
	--print 'spCheckNewDCManager Loop ' + convert(nvarchar(100),getdate(),21)
	DECLARE crManagerDC CURSOR read_only FOR
		select * from #tmpSMSDC

		OPEN crManagerDC
		
		DECLARE @lIsLoop AS bit	
		SET @lIsLoop = 1
		WHILE @lIsLoop = 1
		BEGIN
			--print 'spCheckNewDCManager Begin Loop ' + convert(nvarchar(100),getdate(),21)
			FETCH NEXT
				FROM crManagerDC
				INTO 
					@ManagerSMSDCId 
					,@AreaId 
					,@ManagerMobile 
					,@IsLPNotTamir 
					,@LPNotTamirMinutes 
					,@IsLPTamir 
					,@LPTamirMinutes 
					,@IsMPNotTamir 
					,@MPNotTamirMinutes 
					,@IsMPTamir 
					,@MPTamirMinutes 
					,@IsFT 
					,@FTMinutes 
					,@IsCenter 
					,@IsSetad 
					,@Server121Id 
					,@IsTransFaultSendSMS 
					,@IsLPNotTamirSendSMSAfterConnect 
					,@IsLPTamirSendSMSAfterConnect 
					,@IsMPNotTamirSendSMSAfterConnect 
					,@IsMPTamirSendSMSAfterConnect 
					,@IsFTSendSMSAfterConnect 
					,@IsMPNotTamir_MPFDC 
					,@IsMPTamir_MPFDC 
					,@IsTamir_Confirm 
					,@IsTamir_NotConfirm 
					,@IsDGSRequest
					,@MPNotTamirMinutes_MPFDC
					,@MPTamirMinutes_MPFDC
					,@IsNewRequest 
					,@NewRequestMinutes 
					,@IsLPNotTamirAll 
					,@LPNotTamirAllMinutes 
					,@IsLPTamirAll 
					,@LPTamirAllMinutes 
					,@IsLPTamirSingle 
					,@LPTamirSingleMinutes 
					,@IsLPNotTamirSingle 
					,@LPNotTamirSingleMinutes 
					,@IsMPNotTamirAll 
					,@MPNotTamirAllMinutes 
					,@IsMPTamirAll 
					,@MPTamirAllMinutes 
					,@IsMPTamirSingle 
					,@MPTamirSingleMinutes 
					,@IsMPNotTamirSingle 
					,@MPNotTamirSingleMinutes
					,@IsTamir_ConfirmLP 
					,@IsTamir_NotConfirmLP 
					,@IsTamir_ConfirmFT 
					,@IsTamir_NotConfirmFT
					,@IsSendAlarmForTamir
					,@IsSendSMSForWaitConfirm
					,@IsTamirReturned
					,@IsSendSMSForLongTamirDC
					,@lSendSMSForLongTamirTime
					,@IsNewRequestMP
					,@NewRequestMPMinutes
			--print 'spCheckNewDCManager Fetch Next ' + convert(nvarchar(100),getdate(),21)
			if @@FETCH_STATUS = 0 
			BEGIN
				
				-- Œ«„Ê‘Ì ÃœÌœ
				EXEC spSendSMSDCManager 0, @IsNewRequest, @NewRequestMinutes, NULL ,0 , @ManagerSMSDCId, @ManagerMobile, @IsSetad, @IsCenter, @Server121Id, @AreaId, 0, 0, 0

				-- Œ«„Ê‘Ì ›‘«— ÷⁄Ì› »« »—‰«„Â ò·Ì
				EXEC spSendSMSDCManager 6, @IsLPTamirAll, @LPTamirAllMinutes, @IsLPTamir ,@IsLPTamirSendSMSAfterConnect , @ManagerSMSDCId, @ManagerMobile, @IsSetad, @IsCenter, @Server121Id, @AreaId, 0, 0, 0
				
				-- Œ«„Ê‘Ì ›‘«— ÷⁄Ì› »« »—‰«„Â  òÌ
				EXEC spSendSMSDCManager 7, @IsLPTamirSingle, @LPTamirSingleMinutes, @IsLPTamir ,@IsLPTamirSendSMSAfterConnect , @ManagerSMSDCId, @ManagerMobile, @IsSetad, @IsCenter, @Server121Id, @AreaId, 0, 0, 0
				
				-- Œ«„Ê‘Ì ›‘«— ÷⁄Ì› »Ì »—‰«„Â ò·Ì
				EXEC spSendSMSDCManager 8, @IsLPNotTamirAll, @LPNotTamirAllMinutes, @IsLPTamir ,@IsLPNotTamirSendSMSAfterConnect, @ManagerSMSDCId, @ManagerMobile, @IsSetad, @IsCenter, @Server121Id, @AreaId, 0, 0, 0
				
				-- Œ«„Ê‘Ì ›‘«— ÷⁄Ì› »Ì »—‰«„Â  òÌ
				EXEC spSendSMSDCManager 9, @IsLPNotTamirSingle, @LPNotTamirSingleMinutes, @IsLPTamir ,@IsLPNotTamirSendSMSAfterConnect , @ManagerSMSDCId, @ManagerMobile, @IsSetad, @IsCenter, @Server121Id, @AreaId, 0, 0, 0
				
				-- Œ«„Ê‘Ì ›Êﬁ  Ê“Ì⁄
				EXEC spSendSMSDCManager 1, @IsFT, @FTMinutes, 0, @IsFTSendSMSAfterConnect, @ManagerSMSDCId, @ManagerMobile, @IsSetad, @IsCenter, @Server121Id, @AreaId, @IsMPTamir_MPFDC, @IsMPNotTamir_MPFDC, 0

				-- Œ«„Ê‘Ì ›‘«— „ Ê”ÿ »« »—‰«„Â ò·Ì
				EXEC spSendSMSDCManager 10, @IsMPTamirAll, @MPTamirAllMinutes, @IsMPTamir , @IsMPTamirSendSMSAfterConnect , @ManagerSMSDCId, @ManagerMobile, @IsSetad, @IsCenter, @Server121Id, @AreaId, @IsMPTamir_MPFDC, @IsMPNotTamir_MPFDC, @MPTamirMinutes_MPFDC
				
				-- Œ«„Ê‘Ì ›‘«— „ Ê”ÿ »« »—‰«„Â  òÌ
				EXEC spSendSMSDCManager 11, @IsMPTamirSingle, @MPTamirSingleMinutes, @IsMPTamir , @IsMPTamirSendSMSAfterConnect , @ManagerSMSDCId, @ManagerMobile, @IsSetad, @IsCenter, @Server121Id, @AreaId, @IsMPTamir_MPFDC, @IsMPNotTamir_MPFDC, @MPTamirMinutes_MPFDC

				-- Œ«„Ê‘Ì ›‘«— „ Ê”ÿ »Ì »—‰«„Â ò·Ì
				EXEC spSendSMSDCManager 12, @IsMPNotTamirAll, @MPNotTamirAllMinutes, @IsMPTamir , @IsMPNotTamirSendSMSAfterConnect , @ManagerSMSDCId, @ManagerMobile, @IsSetad, @IsCenter, @Server121Id, @AreaId, @IsMPTamir_MPFDC, @IsMPNotTamir_MPFDC, @MPNotTamirMinutes_MPFDC
				
				-- Œ«„Ê‘Ì ›‘«— „ Ê”ÿ »Ì »—‰«„Â  òÌ
				EXEC spSendSMSDCManager 13, @IsMPNotTamirSingle, @MPNotTamirSingleMinutes, @IsMPTamir , @IsMPNotTamirSendSMSAfterConnect , @ManagerSMSDCId, @ManagerMobile, @IsSetad, @IsCenter, @Server121Id, @AreaId, @IsMPTamir_MPFDC, @IsMPNotTamir_MPFDC, @MPNotTamirMinutes_MPFDC
				
				-- Œ«„Ê‘Ì ›‘«— ÷⁄Ì› »« »—‰«„Â
				EXEC spSendSMSDCManager 2, @IsLPTamir, @LPTamirMinutes, @IsLPTamir, @IsLPTamirSendSMSAfterConnect , @ManagerSMSDCId, @ManagerMobile, @IsSetad, @IsCenter, @Server121Id, @AreaId, @IsMPTamir_MPFDC, @IsMPNotTamir_MPFDC, 0
				
				-- Œ«„Ê‘Ì ›‘«— ÷⁄Ì› »Ì »—‰«„Â
				EXEC spSendSMSDCManager 3, @IsLPNotTamir, @LPNotTamirMinutes, @IsLPTamir ,@IsLPNotTamirSendSMSAfterConnect , @ManagerSMSDCId, @ManagerMobile, @IsSetad, @IsCenter, @Server121Id, @AreaId, @IsMPTamir_MPFDC, @IsMPNotTamir_MPFDC, 0
				
				-- Œ«„Ê‘Ì ›‘«— „ Ê”ÿ »« »—‰«„Â
				EXEC spSendSMSDCManager 4, @IsMPTamir, @MPTamirMinutes, @IsMPTamir , @IsMPTamirSendSMSAfterConnect , @ManagerSMSDCId, @ManagerMobile, @IsSetad, @IsCenter, @Server121Id, @AreaId, @IsMPTamir_MPFDC, @IsMPNotTamir_MPFDC, @MPTamirMinutes_MPFDC
				
				-- Œ«„Ê‘Ì ›‘«— „ Ê”ÿ »Ì »—‰«„Â
				EXEC spSendSMSDCManager 5, @IsMPNotTamir, @MPNotTamirMinutes, @IsMPTamir , @IsMPNotTamirSendSMSAfterConnect , @ManagerSMSDCId, @ManagerMobile, @IsSetad, @IsCenter, @Server121Id, @AreaId, @IsMPTamir_MPFDC, @IsMPNotTamir_MPFDC, @MPNotTamirMinutes_MPFDC
				
				-- Œ«„Ê‘Ì ÃœÌœ »Ì »—‰«„Â ›Ìœ— ›‘«— „ Ê”ÿ <omid>
				EXEC spSendSMSDCManager 14, @IsNewRequestMP, @NewRequestMPMinutes, NULL ,0 , @ManagerSMSDCId, @ManagerMobile, @IsSetad, @IsCenter, @Server121Id, @AreaId, 0, 0, 0
				----------------------------------------</omid>
				DECLARE TamirConfirmSMS CURSOR FOR
					SELECT * FROM #tmpTamirConfrim

					OPEN TamirConfirmSMS
					
					DECLARE @lIsLoop2 AS bit	
					SET @lIsLoop2 = 1
					WHILE @lIsLoop2 = 1
					BEGIN
						FETCH NEXT
							FROM TamirConfirmSMS
							INTO @LastTamirEventId, @TamirRequestId, @TamirRequestStateId, @TamirRequestAreaId

						if @@FETCH_STATUS = 0 
						BEGIN
							IF( @IsTamir_Confirm = 1 OR @IsTamir_NotConfirm = 1 
							OR @IsTamir_ConfirmLP = 1 OR @IsTamir_NotConfirmLP = 1 
							OR @IsTamir_ConfirmFT = 1 OR @IsTamir_NotConfirmFT = 1) 
							BEGIN
								IF( @TamirRequestStateId = 3   OR @TamirRequestStateId = 8 )
								BEGIN
									EXEC spSendSMSTamirConfirm @TamirRequestId, @TamirRequestStateId, @IsTamir_Confirm, @IsTamir_NotConfirm, @AreaId, @ManagerSMSDCId, @ManagerMobile, @TamirRequestAreaId, @IsTamir_ConfirmLP, @IsTamir_NotConfirmLP, @IsTamir_ConfirmFT, @IsTamir_NotConfirmFT
								END
							END
						END
						ELSE
							SET @lIsLoop2 = 0
					END
				CLOSE TamirConfirmSMS
				DEALLOCATE TamirConfirmSMS

				IF( @IsDGSRequest = 1 )
				BEGIN
					EXEC spSendSMSforDGSRequest @ManagerSMSDCId, @ManagerMobile, @AreaId, @Server121Id
				END
				
				IF (@IsSendAlarmForTamir = 1)
				BEGIN
					EXEC spSendSMSAlarmForTamir @ManagerSMSDCId, @ManagerMobile, @IsSetad, @IsCenter, @Server121Id, @AreaId
				END
				
				IF (@IsSendSMSForWaitConfirm = 1)
				BEGIN
					DECLARE @lNowTime as varchar(5)
					SET @lNowTime = dbo.GetTime(GetDate())
					IF @lNowTime >= '08:00' AND @lNowTime <= '22:00' 
						EXEC spSendSMSReminderWaitConfirm @ManagerSMSDCId, @ManagerMobile, @IsSetad, @IsCenter, @AreaId
				END
				
				IF (@IsTamirReturned = 1)
				BEGIN
					EXEC spSendSMSTamirReturned @ManagerSMSDCId, @ManagerMobile, @AreaId
				END
				
				IF (@IsSendSMSForLongTamirDC = 1)
				BEGIN
					EXEC spSendSMSTamirLongDC @ManagerSMSDCId, @ManagerMobile,@IsSetad, @IsCenter, @Server121Id, @AreaId, @lSendSMSForLongTamirTime
				END
				
			END
			ELSE
				set @lIsLoop = 0
		END
		
	CLOSE crManagerDC
	DEALLOCATE crManagerDC
	drop table #tmpSMSDC
	
	if isnull(@LastTamirEventId,0) <=0
			set @LastTamirEventId=@MaxEventId

	DECLARE @lIsSendConfirmPeymankar AS BIT = 0
	
	SELECT @lIsSendConfirmPeymankar = ISNULL(ConfigValue,0) FROM Tbl_Config WHERE ConfigName = 'IsSendConfirmPeymankar'
	
	IF @lIsSendConfirmPeymankar = 1
	BEGIN
		DECLARE PeymankarConfirmSMS CURSOR FOR
		SELECT * FROM #tmpTamirConfrim 

		OPEN PeymankarConfirmSMS
		
		DECLARE @lIsLoop3 AS bit	
		SET @lIsLoop3 = 1
		WHILE @lIsLoop3 = 1
		BEGIN
			FETCH NEXT
				FROM PeymankarConfirmSMS
				INTO @LastTamirEventId, @TamirRequestId, @TamirRequestStateId, @TamirRequestAreaId

			if @@FETCH_STATUS = 0 
			BEGIN
				IF( @TamirRequestStateId = 3 OR @TamirRequestStateId = 8 )
				BEGIN
					EXEC spSendSMSConfirmForPeymankar @TamirRequestId, @TamirRequestStateId, @AreaId
				END
			END
			ELSE
				SET @lIsLoop3 = 0
		END
		CLOSE PeymankarConfirmSMS
		DEALLOCATE PeymankarConfirmSMS
	END			
				
	select @LastTamirEventId = MAX(LastTamirEventId) FROM #tmpTamirConfrim
	IF NOT @LastTamirEventId IS NULL AND @LastTamirEventId > 0
	BEGIN
		UPDATE Tbl_Config
		SET ConfigValue = @LastTamirEventId
		WHERE ConfigName = 'smsLastTamirEventId'
	END
	
	drop table #tmpTamirConfrim
	
	DECLARE @LastSMSId2 as bigint
	select @LastSMSId2 = MAX(SMSId) from Tbl_SMS
	return @LastSMSId2 - @LastSMSId

GO


IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spCheckNewSTManager]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spCheckNewSTManager]
GO
CREATE PROCEDURE [dbo].[spCheckNewSTManager]
AS
	DECLARE @RequestType AS nvarchar(200)
	DECLARE @SMS AS nvarchar(2000)
	DECLARE @SMSClause AS nvarchar(2000)

	DECLARE @ManagerSMSSTId AS int
	DECLARE @ManagerMobile AS varchar(20)
	DECLARE @AreaId AS int
	DECLARE @IsCenter AS bit
	DECLARE @IsSetad AS bit
	DECLARE @Server121Id AS int
	DECLARE @Area AS nvarchar(50)
	DECLARE @ParentAreaId AS int
	DECLARE @SendTime AS varchar(5)
	DECLARE @LastSendTime AS datetime

	DECLARE @lToday AS datetime
	DECLARE @lThisTime AS datetime
	DECLARE @lHour AS int
	DECLARE @lMin AS int
	DECLARE @lDiff AS int, @lDiffNow AS int
	DECLARE @lDisconnectDateFrom as varchar(10)
	DECLARE @lDisconnectDateTo as varchar(10)

	SET @lToday = GETDATE()
	SET @lToday = DATEADD( ms, - DATEPART(ms,@lToday), @lToday )
	SET @lToday = DATEADD( ss, - DATEPART(ss,@lToday), @lToday )
	SET @lToday = DATEADD( mi, - DATEPART(mi,@lToday), @lToday )
	SET @lToday = DATEADD( hh, - DATEPART(hh,@lToday), @lToday )

	DECLARE @RequestCount AS bigint
	DECLARE @DisconnectPower AS float
	DECLARE @DisconnectInterval AS bigint
	DECLARE @AllDisconnectInterval AS bigint
	DECLARE @AllRequestCount AS bigint
	DECLARE @AllDisconnectPower AS float

	DECLARE @lQuery AS varchar(1000)
	DECLARE @lSql AS varchar(2000)
	DECLARE @lPDate AS varchar(10)
	DECLARE @lDT AS datetime
	
	DECLARE @CurrentDate AS varchar(10)
	DECLARE @CurrentTime AS varchar(5) 

	DECLARE @lCnt AS int
	SET @lCnt = 0

	SET @RequestType = 'SMSDaily'
	SET @SMSClause = NULL
	SELECT @SMSClause = ConfigText FROM Tbl_Config WHERE ConfigName = @RequestType
	IF @SMSClause IS NUlL RETURN 0

	SELECT 
		@CurrentDate = dbo.mtosh(getdate()),
		@CurrentTime = dbo.GetTime(getdate())

	--IF OBJECT_ID('tempdb..#tblInfo') IS NOT NULL
	--	DROP TABLE #tblInfo

	CREATE TABLE #tblInfo
	(
		RequestCount bigint, 
		DisconnectPower float, 
		DisconnectInterval bigint
	)

	DECLARE crSMSST CURSOR FOR
		SELECT 
			TblManagerSMSST.ManagerSMSSTId,
			TblManagerSMSST.ManagerMobile,
			TblManagerSMSST.AreaId,
			Tbl_Area.IsCenter,
			Tbl_Area.IsSetad,
			Tbl_Area.Server121Id,
			Tbl_Area.Area,
			Tbl_Area.ParentAreaId,
			TblManagerSMSST.SendTime,  
			TblManagerSMSST.LastSendTime 
		FROM 
			TblManagerSMSST
			INNER JOIN Tbl_Area ON TblManagerSMSST.AreaId = Tbl_Area.AreaId
		WHERE 
			IsActive = 1

		OPEN crSMSST
		
		DECLARE @lIsLoop AS bit	
		SET @lIsLoop = 1
		WHILE @lIsLoop = 1
		BEGIN
			FETCH NEXT FROM crSMSST
			INTO 
				@ManagerSMSSTId, 
				@ManagerMobile, 
				@AreaId, 
				@IsCenter, 
				@IsSetad, 
				@Server121Id, 
				@Area, 
				@ParentAreaId, 
				@SendTime, 
				@LastSendTime
			
			if @@FETCH_STATUS = 0 
			BEGIN
				SET @SMS = @SMSClause
				SET @lHour = Cast(LEFT(@SendTime,2) AS int)
				SET @lMin = Cast(RIGHT(@SendTime,2) AS int)
				SET @lThisTime = DATEADD(hh, @lHour, @lToday)
				SET @lThisTime = DATEADD(mi, @lMin, @lThisTime)
				SET @lDiff = ISNULL(DATEDIFF(mi, @LastSendTime, @lThisTime),1440)
				SET @lDiffNow = DATEDIFF(mi, @lThisTime, GETDATE())
				SET @lDisconnectDateFrom = dbo.mtosh(DATEADD(dd, -1, @lThisTime))
				SET @lDisconnectDateTo = dbo.mtosh(DATEADD(dd, 2, @lThisTime))
				
				
				IF( @lDiffNow >= 0 AND @lDiff > 10 )
				BEGIN
					SET @AllRequestCount = 0
					SET @AllDisconnectPower = 0
					SET @AllDisconnectInterval = 0

					SET @lQuery =
						'SELECT ' +
							'COUNT(RequestId) AS RequestCount, ' +
							'ISNULL(SUM(TblRequest.DisconnectPower), 0.0) AS DisconnectPower, ' +
							'ISNULL(SUM(TblRequest.DisconnectInterval), 0.0) AS DisconnectInterval ' +
						'FROM ' +
							'TblRequest ' +
							'LEFT JOIN TblMPRequest ON TblRequest.MPRequestId = TblMPRequest.MPRequestId ' +
							'LEFT JOIN TblLPRequest ON TblRequest.LPRequestId = TblLPRequest.LPRequestId ' +
							'INNER JOIN Tbl_Area ON TblRequest.AreaId = Tbl_Area.AreaId ' +
						'WHERE ' +
							'((TblRequest.DisconnectDT <= ''' + Convert(varchar, @lThisTime, 20) + ''' ' +
							'AND TblRequest.DisconnectDT >= ''' + Convert(varchar, DATEADD(dd, -1, @lThisTime), 20) + ''' ) OR ' +
							'(TblRequest.ConnectDT <= ''' + Convert(varchar, @lThisTime, 20) + ''' ' +
							'AND TblRequest.ConnectDT >= ''' + Convert(varchar, DATEADD(dd, -1, @lThisTime), 20) + ''' )) ' +
							'AND TblRequest.DisconnectDatePersian >= ''' + @lDisconnectDateFrom + ''' ' +
							'AND TblRequest.DisconnectDatePersian <= ''' + @lDisconnectDateTo + ''' ' +
							'AND TblRequest.IsLightRequest = 0 ' 

					IF @IsSetad = 0
					BEGIN
						IF @IsCenter = 1 
							SET @lQuery = @lQuery + ' AND Server121Id = ' + cast(@Server121Id AS varchar)
						ELSE 
							SET @lQuery = @lQuery + ' AND TblRequest.AreaId = ' + cast(@AreaId AS varchar)
					END

					/*-------------------*/
					/* All Done          */
					/*-------------------*/
					DECLARE @lWarmLineWhere AS varchar(500) = ' AND (ISNULL(ISNULL(TblMPRequest.IsWarmLine,TblLPRequest.IsWarmLine),0)=0) ' 
				
					SET @lSql = @lQuery + ' AND TblRequest.EndJobStateId In (2,3) AND IsLPRequest = 1 ' + @lWarmLineWhere
					DELETE #tblInfo
					INSERT INTO #tblInfo
					EXEC(@lSql)
					SELECT 
						@RequestCount = RequestCount,
						@DisconnectPower = DisconnectPower,
						@DisconnectInterval= DisconnectInterval
					FROM #tblInfo
					SET @AllRequestCount = @AllRequestCount + @RequestCount
					SET @AllDisconnectPower = @AllDisconnectPower + @DisconnectPower
					SET @AllDisconnectInterval = @AllDisconnectInterval + @DisconnectInterval

					SET @SMS = Replace( @SMS,'LPCount', ISNULL(@RequestCount,0) )
					SET @SMS = Replace( @SMS,'LPTime', ISNULL(@DisconnectInterval,0) )
					SET @SMS = Replace( @SMS,'LPPower', Round(ISNULL(@DisconnectPower,0),1) )
					
					/*-------------------*/
					/* All_LP_WarmLine	 */
					/*-------------------*/
					SET @RequestCount = 0
					SET @lSql = @lQuery + ' AND TblRequest.EndJobStateId In (2,3) AND IsLPRequest = 1 AND ISNULL(TblLPRequest.IsWarmLine,0) = 1 ' 
					DELETE #tblInfo
					INSERT INTO #tblInfo
					EXEC(@lSql)
					SELECT 
						@RequestCount = RequestCount
					FROM #tblInfo

					SET @SMS = Replace( @SMS,'LPWarmLineCount', ISNULL(@RequestCount,0) )
					
					/*-------------------*/
					/* MP_NT_Gozara      */
					/*-------------------*/
					SET @RequestCount = 0
					SET @DisconnectPower = 0
					SET @DisconnectInterval = 0
					SET @lSql = @lQuery + ' AND (TblRequest.EndJobStateId In (2,3)) AND (IsTamir = 0) AND IsMPRequest =1 AND ((TblRequest.DisconnectInterval <= 5) or (TblRequest.DisconnectInterval is null)) AND (TblRequest.MPRequestId IN (SELECT MPRequestId FROM TblMPRequest WHERE NOT DisconnectGroupSetId IN(1129 ,1130) AND (DisconnectReasonId < 1200 OR DisconnectReasonId > 1299  OR DisconnectReasonId IS NULL))) ' + @lWarmLineWhere
					DELETE #tblInfo
					INSERT INTO #tblInfo
					EXEC(@lSql)
					SELECT 
						@RequestCount = RequestCount,
						@DisconnectPower = DisconnectPower,
						@DisconnectInterval= DisconnectInterval
					FROM #tblInfo
					SET @AllRequestCount = @AllRequestCount + @RequestCount
					SET @AllDisconnectPower = @AllDisconnectPower + @DisconnectPower
					SET @AllDisconnectInterval = @AllDisconnectInterval + @DisconnectInterval

					SET @SMS = Replace( @SMS,'MPGozaraCount', ISNULL(@RequestCount,0) )
					SET @SMS = Replace( @SMS,'MPGozaraTime', ISNULL(@DisconnectInterval,0) )
					SET @SMS = Replace( @SMS,'MPGozaraPower', Round(ISNULL(@DisconnectPower,0),1) )
					/*-------------------*/
					/* MP_NT_Mandegar    */
					/*-------------------*/
					SET @RequestCount = 0
					SET @DisconnectPower = 0
					SET @DisconnectInterval = 0
					SET @lSql = @lQuery + ' AND (TblRequest.EndJobStateId In (2,3)) AND (IsTamir = 0) AND IsMPRequest =1 AND (TblRequest.DisconnectInterval > 5)  AND (TblRequest.MPRequestId IN (SELECT MPRequestId FROM TblMPRequest WHERE NOT DisconnectGroupSetId IN(1129 ,1130) AND (DisconnectReasonId < 1200 OR DisconnectReasonId > 1299  OR DisconnectReasonId IS NULL))) ' + @lWarmLineWhere
					DELETE #tblInfo
					INSERT INTO #tblInfo
					EXEC(@lSql)
					SELECT 
						@RequestCount = RequestCount,
						@DisconnectPower = DisconnectPower,
						@DisconnectInterval= DisconnectInterval
					FROM #tblInfo
					SET @AllRequestCount = @AllRequestCount + @RequestCount
					SET @AllDisconnectPower = @AllDisconnectPower + @DisconnectPower
					SET @AllDisconnectInterval = @AllDisconnectInterval + @DisconnectInterval

					SET @SMS = Replace( @SMS,'MPMandegarCount', ISNULL(@RequestCount,0) )
					SET @SMS = Replace( @SMS,'MPMandegarTime', ISNULL(@DisconnectInterval,0) )
					SET @SMS = Replace( @SMS,'MPMandegarPower', Round(ISNULL(@DisconnectPower,0),1) )
					/*-------------------*/
					/* All_MP_WarmLine   */
					/*-------------------*/
					SET @RequestCount = 0
					SET @lSql = @lQuery + ' AND (TblRequest.EndJobStateId In (2,3)) AND (IsTamir = 1) AND IsMPRequest =1 AND ISNULL(TblMPRequest.IsWarmLine,0) = 1 '
					DELETE #tblInfo
					INSERT INTO #tblInfo
					EXEC(@lSql)
					SELECT 
						@RequestCount = RequestCount
					FROM #tblInfo

					SET @SMS = Replace( @SMS,'MPWarmLineCount', ISNULL(@RequestCount,0) )
					
					/*-------------------*/
					/* MP_T_ALL          */
					/*-------------------*/
					SET @RequestCount = 0
					SET @DisconnectPower = 0
					SET @DisconnectInterval = 0
					SET @lSql = @lQuery + ' AND (TblRequest.EndJobStateId In (2,3)) AND (IsTamir = 1) AND IsMPRequest =1 AND (TblRequest.MPRequestId IN (SELECT MPRequestId FROM TblMPRequest WHERE NOT DisconnectGroupSetId IN (1129,1130) AND (DisconnectReasonId < 1200 OR DisconnectReasonId > 1299  OR DisconnectReasonId IS NULL))) ' + @lWarmLineWhere
					DELETE #tblInfo
					INSERT INTO #tblInfo
					EXEC(@lSql)
					SELECT 
						@RequestCount = RequestCount,
						@DisconnectPower = DisconnectPower,
						@DisconnectInterval= DisconnectInterval
					FROM #tblInfo
					SET @AllRequestCount = @AllRequestCount + @RequestCount
					SET @AllDisconnectPower = @AllDisconnectPower + @DisconnectPower
					SET @AllDisconnectInterval = @AllDisconnectInterval + @DisconnectInterval

					SET @SMS = Replace( @SMS,'MPTamirCount', ISNULL(@RequestCount,0) )
					SET @SMS = Replace( @SMS,'MPTamirTime', ISNULL(@DisconnectInterval,0) )
					SET @SMS = Replace( @SMS,'MPTamirPower', Round(ISNULL(@DisconnectPower,0),1) )
					/*-------------------*/
					/* FT_BiBarnameh (NT)*/
					/*-------------------*/
					SET @RequestCount = 0
					SET @DisconnectPower = 0
					SET @DisconnectInterval = 0
					SET @lSql = @lQuery + ' AND (TblRequest.EndJobStateId In (2,3)) AND ((IsFogheToziRequest =1 AND FogheToziDisconnectId in (SELECT FogheToziDisconnectId FROM TblFogheToziDisconnect WHERE (FogheToziId IN (5,9)))) OR (TblRequest.IsMPRequest = 1 AND TblRequest.MPRequestId IN (SELECT MPRequestId FROM TblMPRequest WHERE DisconnectGroupSetId IN (1126,1127,1131,1132)))) ' + @lWarmLineWhere
					DELETE #tblInfo
					INSERT INTO #tblInfo
					EXEC(@lSql)
					SELECT 
						@RequestCount = RequestCount,
						@DisconnectPower = DisconnectPower,
						@DisconnectInterval= DisconnectInterval
					FROM #tblInfo
					SET @AllRequestCount = @AllRequestCount + @RequestCount
					SET @AllDisconnectPower = @AllDisconnectPower + @DisconnectPower
					SET @AllDisconnectInterval = @AllDisconnectInterval + @DisconnectInterval

					SET @SMS = Replace( @SMS,'FTCount', ISNULL(@RequestCount,0) )
					SET @SMS = Replace( @SMS,'FTTime', ISNULL(@DisconnectInterval,0) )
					SET @SMS = Replace( @SMS,'FTPower', Round(ISNULL(@DisconnectPower,0),1) )
					/*-------------------*/
					/* FT_BaBarnameh (T) */
					/*-------------------*/
					SET @RequestCount = 0
					SET @DisconnectPower = 0
					SET @DisconnectInterval = 0
					IF CHARINDEX(N'BaBarnameh',@SMS) > 0
					BEGIN
						SET @lSql = @lQuery + ' AND (TblRequest.EndJobStateId In (2,3)) AND ((IsFogheToziRequest =1 AND FogheToziDisconnectId in (SELECT FogheToziDisconnectId FROM TblFogheToziDisconnect WHERE (FogheToziId in (2,3,4,6,7,8)))) OR (TblRequest.IsMPRequest = 1 AND TblRequest.MPRequestId IN (SELECT MPRequestId FROM TblMPRequest WHERE DisconnectGroupSetId IN (1010,1040,1088) AND NOT DisconnectReasonId IN (1215,1235,1255)))) ' + @lWarmLineWhere
						DELETE #tblInfo
						INSERT INTO #tblInfo
						EXEC(@lSql)
						SELECT 
							@RequestCount = RequestCount,
							@DisconnectPower = DisconnectPower,
							@DisconnectInterval= DisconnectInterval
						FROM #tblInfo
						SET @AllRequestCount = @AllRequestCount + @RequestCount
						SET @AllDisconnectPower = @AllDisconnectPower + @DisconnectPower
						SET @AllDisconnectInterval = @AllDisconnectInterval + @DisconnectInterval

						SET @SMS = Replace( @SMS,'FTBaBarnamehCount', ISNULL(@RequestCount,0) )
						SET @SMS = Replace( @SMS,'FTBaBarnamehTime', ISNULL(@DisconnectInterval,0) )
						SET @SMS = Replace( @SMS,'FTBaBarnamehPower', Round(ISNULL(@DisconnectPower,0),1) )
					END
					/*-------------------*/
					/* FT_Kambood_Tolid  */
					/*-------------------*/
					SET @RequestCount = 0
					SET @DisconnectPower = 0
					SET @DisconnectInterval = 0
					IF CHARINDEX(N'Kambood',@SMS) > 0
					BEGIN
						SET @lSql = @lQuery + ' AND (TblRequest.EndJobStateId In (2,3)) AND ((IsFogheToziRequest =1 AND FogheToziDisconnectId in (SELECT FogheToziDisconnectId FROM TblFogheToziDisconnect WHERE FogheToziId =1)) OR (TblRequest.IsMPRequest = 1 AND TblRequest.MPRequestId IN (SELECT MPRequestId FROM TblMPRequest WHERE DisconnectReasonId IN (1215,1235,1255)))) ' + @lWarmLineWhere
						DELETE #tblInfo
						INSERT INTO #tblInfo
						EXEC(@lSql)
						SELECT 
							@RequestCount = RequestCount,
							@DisconnectPower = DisconnectPower,
							@DisconnectInterval= DisconnectInterval
						FROM #tblInfo
						SET @AllRequestCount = @AllRequestCount + @RequestCount
						SET @AllDisconnectPower = @AllDisconnectPower + @DisconnectPower
						SET @AllDisconnectInterval = @AllDisconnectInterval + @DisconnectInterval

						SET @SMS = Replace( @SMS,'FTKamboodCount', ISNULL(@RequestCount,0))
						SET @SMS = Replace( @SMS,'FTKamboodTime', ISNULL(@DisconnectInterval,0))
						SET @SMS = Replace( @SMS,'FTKamboodPower', Round(ISNULL(@DisconnectPower,0),1))
					END
					/*-------------------*/
					/* New Requests      */
					/*-------------------*/
					SET @RequestCount = 0
					SET @DisconnectPower = 0
					SET @DisconnectInterval = 0
					SET @lSql = @lQuery + ' AND (TblRequest.EndJobStateId = 4) ' + @lWarmLineWhere
					DELETE #tblInfo
					INSERT INTO #tblInfo
					EXEC(@lSql)
					SELECT 
						@RequestCount = RequestCount,
						@DisconnectPower = DisconnectPower,
						@DisconnectInterval= DisconnectInterval
					FROM #tblInfo
					SET @AllRequestCount = @AllRequestCount + @RequestCount

					SET @SMS = Replace( @SMS,'NewCount', ISNULL(@RequestCount,0) )
					/*-------------------*/
					/* Working Requests  */
					/*-------------------*/
					SET @RequestCount = 0
					SET @lSql = @lQuery + ' AND (TblRequest.EndJobStateId = 5) ' + @lWarmLineWhere
					DELETE #tblInfo
					INSERT INTO #tblInfo
					EXEC(@lSql)
					SELECT 
						@RequestCount = RequestCount,
						@DisconnectPower = DisconnectPower,
						@DisconnectInterval= DisconnectInterval
					FROM #tblInfo
					SET @AllRequestCount = @AllRequestCount + @RequestCount

					SET @SMS = Replace( @SMS,'EjraCount', ISNULL(@RequestCount,0) )
					SET @SMS = Replace( @SMS,'WorkingCount', ISNULL(@RequestCount,0) )
					/*-------------------*/
					/*-------------------*/
					/*-------------------*/
						
					SET @SMS = Replace( @SMS,'TotalCount', Cast(@AllRequestCount AS varchar) )
					SET @SMS = Replace( @SMS,'TotalPower', Round(ISNULL(@AllDisconnectPower,0),1) )
					SET @SMS = Replace( @SMS,'TotalTime', Cast(@AllDisconnectInterval AS varchar) )
					
					SET @lDT = GETDATE()
					SET @lPDate = dbo.mtosh(@lDT)
					
					SET @SMS = Replace( @SMS,'yy', SUBSTRING(@lPDate,1,4) )
					SET @SMS = Replace( @SMS,'mm', SUBSTRING(@lPDate,6,2) )
					SET @SMS = Replace( @SMS,'dd', SUBSTRING(@lPDate,9,2) )
					SET @SMS = Replace( @SMS,'hh', Cast(DATEPART(hh,@lDT) AS varchar) )
					SET @SMS = Replace( @SMS,'nn', Cast(DATEPART(mi,@lDT) AS varchar) )
					SET @SMS = Replace( @SMS,'CurrentDate',@CurrentDate)
					SET @SMS = Replace( @SMS,'CurrentTime',@CurrentTime)

					SET @SMS = Replace( @SMS,'Area', @Area )
					SET @SMS = Replace( @SMS,'CRLF', nchar(13) )
					
					SET @lDT = DATEADD(dd,1,@lThisTime)
					SET @lPDate = 
						dbo.mtosh( @lDT ) + ' ' +
						Cast(DATEPART(hh,@lDT) AS varchar) + ':' +
						Cast(DATEPART(mi,@lDT) AS varchar)
					SET @RequestType = 'SMSDaily NextDT:' + @lPDate
					
					DECLARE @Desc AS nvarchar(100)
					SET @Desc = 'Havades-ID=' + CAST(@ManagerSMSSTId AS nvarchar)
					EXEC spSendSMS @SMS, @ManagerMobile, @Desc, @RequestType, @AreaId
					
					UPDATE TblManagerSMSST
					SET LastSendTime = GETDATE()
					WHERE ManagerSMSSTId = @ManagerSMSSTId
					
					SET @lCnt = @lCnt + 1
				END /* of IF( @lDiffNow >= 0 AND @lDiff > 10 ) */
			END /* of WHILE */
			ELSE
				SET @lIsLoop = 0
		END
	CLOSE crSMSST
	DEALLOCATE crSMSST
		
	DROP TABLE #tblInfo

	RETURN @lCnt
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spCheckMPFeederDCCountSMS]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spCheckMPFeederDCCountSMS]
GO
CREATE PROCEDURE [dbo].[spCheckMPFeederDCCountSMS] 
AS
	DECLARE @lYearMonth AS char(7)
	DECLARE @lFromDate AS char(10)
	DECLARE @lToDate AS char(10)
	DECLARE @lSQl AS varchar(2000)

	--SET @lYearMonth = '1392/01'
	--SET @lFromDate = '1392/01/01'
	--SET @lToDate = '1392/01/31'
	SET @lYearMonth = LEFT(dbo.mtosh(GETDATE()),7)
	SET @lFromDate = @lYearMonth + '/01'
	SET @lToDate = @lYearMonth + '/31'

	CREATE Table #tDC
	(
		Server121Id int, 
		AreaId int,
		IsCenter bit,
		MPFeederId int,
		MPPostName nvarchar(50), 
		MPFeederName nvarchar(50), 
		DCCount int
	)

	INSERT INTO #tDC
	SELECT 
		Tbl_Area.Server121Id, 
		tReq.AreaId, 
		Tbl_Area.IsCenter, 
		tMPR.MPFeederId ,
		tMPP.MPPostName, 
		tMPF.MPFeederName, 
		COUNT(tReq.RequestId) AS DCCount
	FROM 
		TblRequest tReq  
		INNER JOIN TblMPRequest tMPR ON tReq.MPRequestId = tMPR.MPRequestId 
		INNER JOIN Tbl_MPFeeder tMPF ON tMPR.MPFeederId = tMPF.MPFeederId 
		INNER JOIN Tbl_MPPost tMPP ON tMPF.MPPostId = tMPP.MPPostId 
		INNER JOIN Tbl_Area ON tReq.AreaId = Tbl_Area.AreaId 
	WHERE  
		tMPR.DisconnectDatePersian >= @lFromDate 
		AND tMPR.DisconnectDatePersian <= @lToDate 
		AND tReq.IsDisconnectMPFeeder = 1 
		AND tReq.IsTamir = 0
	GROUP BY 
		Tbl_Area.Server121Id, 
		tReq.AreaId, 
		Tbl_Area.IsCenter, 
		tMPR.MPFeederId, 
		tMPP.MPPostName, 
		tMPF.MPFeederName, 
		tMPF.OwnershipId

	DECLARE @ManagerSMSMPFId AS bigint
	DECLARE @ManagerSMSDCId AS int
	DECLARE @ManagerMobile AS varchar(20)
	DECLARE @YearMonth AS char(7)
	DECLARE @DCCount AS int
	DECLARE @MPPostName AS nvarchar(50)
	DECLARE @MPFeederId AS int
	DECLARE @MPFeederName AS nvarchar(50)

	DECLARE @lSendCount AS int
	DECLARE @lFindCount AS int

	SET @lSendCount = 0

	DECLARE crMPFDC CURSOR FOR
		SELECT tSmsMPF.ManagerSMSMPFId, tSmsDC.ManagerSMSDCId, tSmsDC.ManagerMobile, tDC.DCCount, tDC.MPPostName, tDC.MPFeederId, tDC.MPFeederName
		FROM
			TblManagerSMSMPF tSmsMPF
			INNER JOIN #tDC tDC ON tSmsMPF.MPFeederId = tDC.MPFeederId AND tSmsMPF.YearMonth = @lYearMonth
			INNER JOIN TblManagerSMSDC tSmsDC ON tSmsMPF.ManagerSMSDCId = tSmsDC.ManagerSMSDCId
			INNER JOIN Tbl_Area tArea ON tSmsDC.AreaId = tArea.AreaId
		WHERE
			tSmsMPF.DCCount < tDC.DCCount
			AND (tSmsDC.AreaId = 99 OR (tDC.IsCenter = 1 AND tArea.Server121Id = tDC.Server121Id) OR tSmsDC.AreaId = tDC.AreaId)
			AND tSmsDC.IsActive = 1
			AND tSmsDC.IsMPFDC_nTime = 1
		UNION
		SELECT CAST(NULL AS bigint) AS ManagerSMSMPFId, tSmsDC.ManagerSMSDCId, tSmsDC.ManagerMobile, tDC.DCCount, tDC.MPPostName, tDC.MPFeederId, tDC.MPFeederName
		FROM 
			(
				TblManagerSMSDC tSmsDC
				INNER JOIN Tbl_Area tArea ON tSmsDC.AreaId = tArea.AreaId
			)
			CROSS JOIN #tDC tDC
		WHERE 
			tSmsDC.IsActive = 1
			AND tSmsDC.IsMPFDC_nTime = 1
			AND (tSmsDC.AreaId = 99 OR (tDC.IsCenter = 1 AND tArea.Server121Id = tDC.Server121Id) OR tSmsDC.AreaId = tDC.AreaId)
			AND tSmsDC.MPFDC_CountOfnTime <= tDC.DCCount
			AND NOT CAST(tSmsDC.ManagerSMSDCId as varchar) + '-' + CAST(tDC.MPFeederId as varchar) in 
			(
				SELECT CAST(ManagerSMSDCId AS VARCHAR) + '-' + CAST(MPFeederId AS VARCHAR)
				FROM TblManagerSMSMPF
				WHERE YearMonth = @lYearMonth
			)

		OPEN crMPFDC
		
		DECLARE @lIsLoop AS bit	
		SET @lIsLoop = 1
		WHILE @lIsLoop = 1
		BEGIN
			FETCH NEXT
				FROM crMPFDC
				INTO 
					@ManagerSMSMPFId, 
					@ManagerSMSDCId, 
					@ManagerMobile, 
					@DCCount, 
					@MPPostName, 
					@MPFeederId, 
					@MPFeederName
					
			if @@FETCH_STATUS = 0 
			BEGIN
				SET @lFindCount = 0
				EXEC @lFindCount = spSendSMSMPFeederDCCount @ManagerSMSMPFId, @ManagerSMSDCId, @ManagerMobile, @lYearMonth, @DCCount, @MPPostName, @MPFeederId, @MPFeederName
				SET @lSendCount = @lSendCount + @lFindCount
			END
			ELSE
				SET @lIsLoop = 0
		END
	CLOSE crMPFDC
	DEALLOCATE crMPFDC
		
	DROP TABLE #tDC

	RETURN @lSendCount
GO

IF NOT EXISTS(SELECT * FROM sysobjects INNER JOIN  syscolumns ON sysobjects.id = syscolumns.id WHERE (sysobjects.id = OBJECT_ID(N'TblSubscriberInfom')) AND (syscolumns.name = 'IsSendSMSAfterConnect'))
	ALTER TABLE TblSubscriberInfom WITH CHECK ADD [IsSendSMSAfterConnect] [bit] NULL
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spCheckNewSMSEvent]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spCheckNewSMSEvent]
GO
CREATE PROCEDURE [dbo].[spCheckNewSMSEvent]
AS
	DECLARE @SMS AS nvarchar(2000)
	DECLARE @TelMobile AS nvarchar(200)
	DECLARE @IsTamir AS bit
	DECLARE @TamirDisconnectFromDT AS datetime
	DECLARE @TamirDisconnectToDT AS datetime
	DECLARE @EndJobStateId AS bigint
	DECLARE @AreaId AS integer
	DECLARE @SubscriberAreaId AS integer
	DECLARE @ServerDT AS datetime
	DECLARE @EstimateDT AS datetime
	DECLARE @SubscriberInformId AS bigint
	DECLARE @RequestType AS nvarchar(200)
	DECLARE @Diff AS int
	DECLARE @Address AS nvarchar(250)
	DECLARE @SendSMSStatusId As int
	DECLARE @IsSendSMSAfterConnect AS bit
	DECLARE @ConnectPDate as varchar(10)
	DECLARE @ConnectTime as varchar(5)
	DECLARE @IsSMS as bit
	DECLARE @TamirDisconnectFromTime AS varchar(5)
	DECLARE @TamirDisconnectToTime AS varchar(5)
  -------------<omid>
  DECLARE @SMSSubject AS NVARCHAR(100)
  -------------</omid>
	SET @Diff = 0
	
	DECLARE crInform CURSOR FOR
		SELECT TOP 50
			IsTamir, 
			EndJobStateId, 
			TelMobile, 
			AreaId, 
			SubscriberInformId, 
			TamirDisconnectFromDT, 
			TamirDisconnectToDT, 
			ServerDT, 
			EstimateDT, 
			SubscriberAreaId,
			Address,
			ConnectDatePersian,
			ConnectTime,
			SendSMSStatusId,
			ISNULL(IsSendSMSAfterConnect,0) AS IsSendSMSAfterConnect,
			TamirDisconnectFromTime,
			TamirDisconnectToTime
		FROM 
			View_SendSMS
		WHERE
			SendSMSStatusId = 1

		OPEN crInform
		
		DECLARE @lIsLoop AS bit	
		SET @lIsLoop = 1
		WHILE @lIsLoop = 1
		BEGIN

			FETCH NEXT
				FROM crInform
				INTO 
					@IsTamir, 
					@EndJobStateId, 
					@TelMobile, 
					@AreaId, 
					@SubscriberInformId, 
					@TamirDisconnectFromDT, 
					@TamirDisconnectToDT, 
					@ServerDT, 
					@EstimateDT, 
					@SubscriberAreaId,
					@Address,
					@ConnectPDate,
					@ConnectTime,
					@SendSMSStatusId,
					@IsSendSMSAfterConnect,
					@TamirDisconnectFromTime,
					@TamirDisconnectToTime

			if @@FETCH_STATUS = 0 
			BEGIN

				SET @IsSMS = 1
				IF( @SendSMSStatusId = 1 )
				BEGIN
					IF( @IsTamir = 1 )
					BEGIN
            ------------------<omid>
            SELECT TOP(1) @SMSSubject = ttrs.TamirRequestSubject FROM TblSubscriberInfom tsi
              INNER JOIN TblRequestInform tri ON tri.RequestInformId = tsi.RequestInformId
              INNER JOIN TblRequest tr ON tr.RequestId = tri.RequestId
              INNER JOIN TblTamirRequestConfirm ttrc ON ttrc.RequestId = tr.RequestId
              INNER JOIN TblTamirRequest ttr ON ttr.TamirRequestId = ttrc.TamirRequestId
              INNER JOIN Tbl_TamirRequestSubject ttrs ON ttrs.TamirRequestSubjectId = ttr.TamirRequestSubjectId
              WHERE tsi.SubscriberInformId = @SubscriberInformId
            ------------------</omid>
						SET @RequestType = 'SMSBaBarnameh'
						SET @Diff = DATEDIFF(mi, @TamirDisconnectFromDT, @TamirDisconnectToDT)
						SET @EstimateDT = @TamirDisconnectFromDT
					END
					ELSE
					BEGIN
						SET @RequestType = 'SMSBiBarnameh'
						IF( @ServerDT > @EstimateDT )
						BEGIN
							EXEC spUpdateSendSMSStatus @SubscriberInformId, 5, 0, @AreaId
							SET @IsSMS = 0
						END
						SET @Diff = DATEDIFF(mi, @ServerDT, @EstimateDT)
					END
				END
				ELSE
					SET @IsSMS = 0
					
				
				IF( @IsSMS = 1 )
				BEGIN
					DECLARE @lDT AS varchar(10)
					SET @lDT = dbo.mtosh(@EstimateDT)

					SET @SMS = NULL
					SELECT @SMS = ConfigText FROM Tbl_Config WHERE ConfigName = @RequestType

					IF( NOT @SMS IS NULL )
					BEGIN
						DECLARE @hh AS varchar(2), @mi AS varchar(2)
						SET @hh = Cast(DATEPART(hh,@EstimateDT) as varchar)
						SET @mi = Cast(DATEPART(mi,@EstimateDT) as varchar)
						IF( cast(@hh as int) < 10 ) SET @hh = '0' + @hh
						IF( cast(@mi as int) < 10 ) SET @mi = '0' + @mi
						
						SET @SMS = Replace( @SMS,'TamirDisconnectFromTime', ISNULL(@TamirDisconnectFromTime,N'ø'))
						SET @SMS = Replace( @SMS,'TamirDisconnectToTime', ISNULL(@TamirDisconnectToTime,N'ø'))
						SET @SMS = Replace( @SMS,'Address', ISNULL(@Address,N'ø'))
            SET @SMS = Replace( @SMS,'SMSSubject', ISNULL(@SMSSubject,N' ⁄„Ì—«  ‘»òÂ'))---------omid
						SET @SMS = Replace( @SMS,'ConnectDate', ISNULL(@ConnectPDate,N'ø'))
						SET @SMS = Replace( @SMS,'ConnectTime', ISNULL(@ConnectTime,N'ø'))
						SET @SMS = Replace( @SMS,'ss', ISNULL(Cast(@Diff as nvarchar),'0'))
						SET @SMS = Replace( @SMS,'yy', SUBSTRING(@lDT,1,4))
						SET @SMS = Replace( @SMS,'mm', SUBSTRING(@lDT,6,2))
						SET @SMS = Replace( @SMS,'dd', SUBSTRING(@lDT,9,2))
						SET @SMS = Replace( @SMS,'hh', @hh )
						SET @SMS = Replace( @SMS,'nn', @mi )
						SET @SMS = Replace( @SMS,'CRLF', nchar(13) )
						
						DECLARE @Desc AS nvarchar(100)
						
						IF( @SendSMSStatusId = 1 AND @EndJobStateId IN (4,5) )
						BEGIN
							SET @Desc = 'SubInform-ID=' + cast(@SubscriberInformId as nvarchar)
							EXEC spSendSMS @SMS , @TelMobile, @Desc, @RequestType, @SubscriberAreaId
							EXEC spUpdateSendSMSStatus @SubscriberInformId, 2, 0, @AreaId
						END
						ELSE
						BEGIN
							EXEC spUpdateSendSMSStatus @SubscriberInformId, 5, 0, @AreaId
						END
					END
				END
			END
			ELSE
				set @lIsLoop = 0
		END
		
	CLOSE crInform
	DEALLOCATE crInform
GO


IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spCheckNewSMSEventAfterConnect]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spCheckNewSMSEventAfterConnect]
GO
CREATE PROCEDURE [dbo].[spCheckNewSMSEventAfterConnect]
AS
	DECLARE @SMS AS nvarchar(2000)
	DECLARE @TelMobile AS nvarchar(200)
	DECLARE @IsTamir AS bit
	DECLARE @TamirDisconnectFromDT AS datetime
	DECLARE @TamirDisconnectToDT AS datetime
	DECLARE @EndJobStateId AS bigint
	DECLARE @AreaId AS integer
	DECLARE @SubscriberAreaId AS integer
	DECLARE @ServerDT AS datetime
	DECLARE @EstimateDT AS datetime
	DECLARE @SubscriberInformId AS bigint
	DECLARE @RequestType AS nvarchar(200)
	DECLARE @Diff AS int
	DECLARE @Address AS nvarchar(250)
	DECLARE @SendSMSStatusId As int
	DECLARE @IsSendSMSAfterConnect AS bit
	DECLARE @ConnectPDate as varchar(10)
	DECLARE @ConnectTime as varchar(5)
	DECLARE @IsSMS as bit

	SET @Diff = 0
	
	DECLARE crInform CURSOR FOR
		SELECT TOP 50
			IsTamir, 
			EndJobStateId, 
			TelMobile, 
			AreaId, 
			SubscriberInformId, 
			TamirDisconnectFromDT, 
			TamirDisconnectToDT, 
			ServerDT, 
			EstimateDT, 
			SubscriberAreaId,
			Address,
			ConnectDatePersian,
			ConnectTime,
			SendSMSStatusId,
			ISNULL(IsSendSMSAfterConnect,0) AS IsSendSMSAfterConnect
		FROM 
			View_SendSMS
		WHERE 
			SendSMSStatusId = 2
		ORDER BY ISNULL(ConnectDT,TamirDisconnectToDT)

		OPEN crInform
		
		DECLARE @lIsLoop AS bit	
		SET @lIsLoop = 1
		WHILE @lIsLoop = 1
		BEGIN

			FETCH NEXT
				FROM crInform
				INTO 
					@IsTamir, 
					@EndJobStateId, 
					@TelMobile, 
					@AreaId, 
					@SubscriberInformId, 
					@TamirDisconnectFromDT, 
					@TamirDisconnectToDT, 
					@ServerDT, 
					@EstimateDT, 
					@SubscriberAreaId,
					@Address,
					@ConnectPDate,
					@ConnectTime,
					@SendSMSStatusId,
					@IsSendSMSAfterConnect

			if @@FETCH_STATUS = 0 
			BEGIN

				SET @IsSMS = 1
				IF( @SendSMSStatusId = 2 AND @IsSendSMSAfterConnect = 0 )
				BEGIN
					SET @RequestType = 'SMSSubAfterConnect'
					SET @EstimateDT = ''
					SET @Diff = 0
				END
				ELSE
					SET @IsSMS = 0
					
				
				IF( @IsSMS = 1 )
				BEGIN
					DECLARE @lDT AS varchar(10)
					SET @lDT = dbo.mtosh(@EstimateDT)

					SET @SMS = NULL
					SELECT @SMS = ConfigText FROM Tbl_Config WHERE ConfigName = @RequestType

					IF( NOT @SMS IS NULL )
					BEGIN
						DECLARE @hh AS varchar(2), @mi AS varchar(2)
						SET @hh = Cast(DATEPART(hh,@EstimateDT) as varchar)
						SET @mi = Cast(DATEPART(mi,@EstimateDT) as varchar)
						IF( cast(@hh as int) < 10 ) SET @hh = '0' + @hh
						IF( cast(@mi as int) < 10 ) SET @mi = '0' + @mi
						
						SET @SMS = Replace( @SMS,'Address', ISNULL(@Address,N'ø'))
						SET @SMS = Replace( @SMS,'ConnectDate', ISNULL(@ConnectPDate,N'ø'))
						SET @SMS = Replace( @SMS,'ConnectTime', ISNULL(@ConnectTime,N'ø'))
						SET @SMS = Replace( @SMS,'ss', ISNULL(Cast(@Diff as nvarchar),'0'))
						SET @SMS = Replace( @SMS,'yy', SUBSTRING(@lDT,1,4))
						SET @SMS = Replace( @SMS,'mm', SUBSTRING(@lDT,6,2))
						SET @SMS = Replace( @SMS,'dd', SUBSTRING(@lDT,9,2))
						SET @SMS = Replace( @SMS,'hh', @hh )
						SET @SMS = Replace( @SMS,'nn', @mi )
						SET @SMS = Replace( @SMS,'CRLF', nchar(13) )
						
						DECLARE @Desc AS nvarchar(100)
						IF( @SendSMSStatusId = 2 AND @EndJobStateId IN (2,3) )
						BEGIN
							SET @Desc = 'SubInform-ID=' + cast(@SubscriberInformId as nvarchar)
							EXEC spSendSMS @SMS , @TelMobile, @Desc, @RequestType, @SubscriberAreaId
							EXEC spUpdateSendSMSStatus @SubscriberInformId, 2, 1, @AreaId
						END
						ELSE IF( @SendSMSStatusId = 2 AND @EndJobStateId IN (4,5) )
						BEGIN
							SET @IsSMS = 0
						END
						ELSE
						BEGIN
							EXEC spUpdateSendSMSStatus @SubscriberInformId, 5, 0, @AreaId
						END
					END
				END
			END
			ELSE
				set @lIsLoop = 0
		END
		
	CLOSE crInform
	DEALLOCATE crInform
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spCheckNewCallEvent]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spCheckNewCallEvent]
GO
CREATE PROCEDURE [dbo].[spCheckNewCallEvent]
AS
	DECLARE @CallBody AS nvarchar(2000)
	DECLARE @Telephone AS nvarchar(200)
	DECLARE @IsTamir AS bit
	DECLARE @TamirDisconnectFromDT AS datetime
	DECLARE @TamirDisconnectToDT AS datetime
	DECLARE @EndJobStateId AS bigint
	DECLARE @AreaId AS integer
	DECLARE @SubscriberAreaId AS integer
	DECLARE @ServerDT AS datetime
	DECLARE @EstimateDT AS datetime
	DECLARE @SubscriberInformId AS bigint
	DECLARE @RequestType AS nvarchar(200)
	DECLARE @Diff AS int
	DECLARE @Address AS nvarchar(250)
	DECLARE @OutgoingCallStatusId As int
	DECLARE @IsSendSMSAfterConnect AS bit
	DECLARE @ConnectPDate as varchar(10)
	DECLARE @ConnectTime as varchar(5)
	DECLARE @TamirDisconnectFromTime AS varchar(5)
	DECLARE @TamirDisconnectToTime AS varchar(5)
	DECLARE @IsSend as bit
	SET @Diff = 0
	
	DECLARE crInform CURSOR FOR
		SELECT TOP 50
			IsTamir, 
			EndJobStateId, 
			Telephone, 
			AreaId, 
			SubscriberInformId, 
			TamirDisconnectFromDT, 
			TamirDisconnectToDT, 
			ServerDT, 
			EstimateDT, 
			SubscriberAreaId,
			Address,
			ConnectDatePersian,
			ConnectTime,
			OutgoingCallStatusId,
			ISNULL(IsSendSMSAfterConnect,0) AS IsSendSMSAfterConnect,
			TamirDisconnectFromTime,
			TamirDisconnectToTime
		FROM 
			View_SendCall
		WHERE
			OutgoingCallStatusId = 1

		OPEN crInform
		
		DECLARE @lIsLoop AS bit	
		SET @lIsLoop = 1
		WHILE @lIsLoop = 1
		BEGIN

			FETCH NEXT
				FROM crInform
				INTO 
					@IsTamir, 
					@EndJobStateId, 
					@Telephone, 
					@AreaId, 
					@SubscriberInformId, 
					@TamirDisconnectFromDT, 
					@TamirDisconnectToDT, 
					@ServerDT, 
					@EstimateDT, 
					@SubscriberAreaId,
					@Address,
					@ConnectPDate,
					@ConnectTime,
					@OutgoingCallStatusId,
					@IsSendSMSAfterConnect,
					@TamirDisconnectFromTime,
					@TamirDisconnectToTime

			if @@FETCH_STATUS = 0 
			BEGIN
				
				SET @IsSend = 1
				IF( @OutgoingCallStatusId = 1 )
				BEGIN
					IF( @IsTamir = 1 )
					BEGIN
						SET @Diff = DATEDIFF(mi, @TamirDisconnectFromDT, @TamirDisconnectToDT)
						SET @EstimateDT = @TamirDisconnectFromDT
					END
					ELSE
					BEGIN
						IF( @ServerDT > @EstimateDT )
						BEGIN
							EXEC spUpdateOutgoingCallStatus @SubscriberInformId, 5, 0, @AreaId
							SET @IsSend = 0
						END
						SET @Diff = DATEDIFF(mi, @ServerDT, @EstimateDT)
					END
					
					IF @IsSend = 1
					BEGIN
						DECLARE @lDT AS varchar(10)
						SET @lDT = dbo.mtosh(@EstimateDT)

						SET @CallBody = ' „«”  ·›‰Ì'

						IF( NOT @CallBody IS NULL )
						BEGIN
							DECLARE @hh AS varchar(2), @mi AS varchar(2)
							SET @hh = Cast(DATEPART(hh,@EstimateDT) as varchar)
							SET @mi = Cast(DATEPART(mi,@EstimateDT) as varchar)
							IF( cast(@hh as int) < 10 ) SET @hh = '0' + @hh
							IF( cast(@mi as int) < 10 ) SET @mi = '0' + @mi
							
							SET @CallBody = Replace( @CallBody,'TamirDisconnectFromTime', ISNULL(@TamirDisconnectFromTime,N'ø'))
							SET @CallBody = Replace( @CallBody,'TamirDisconnectToTime', ISNULL(@TamirDisconnectToTime,N'ø'))
							SET @CallBody = Replace( @CallBody,'Address', ISNULL(@Address,N'ø'))
							SET @CallBody = Replace( @CallBody,'ConnectDate', ISNULL(@ConnectPDate,N'ø'))
							SET @CallBody = Replace( @CallBody,'ConnectTime', ISNULL(@ConnectTime,N'ø'))
							SET @CallBody = Replace( @CallBody,'ss', ISNULL(Cast(@Diff as nvarchar),'0'))
							SET @CallBody = Replace( @CallBody,'yy', SUBSTRING(@lDT,1,4))
							SET @CallBody = Replace( @CallBody,'mm', SUBSTRING(@lDT,6,2))
							SET @CallBody = Replace( @CallBody,'dd', SUBSTRING(@lDT,9,2))
							SET @CallBody = Replace( @CallBody,'hh', @hh )
							SET @CallBody = Replace( @CallBody,'nn', @mi )
							SET @CallBody = Replace( @CallBody,'CRLF', nchar(13) )
							
							DECLARE @Desc AS nvarchar(100)
							
							IF(@EndJobStateId IN (4,5) )
							BEGIN
								SET @Desc = 'Call SubInform-ID=' + cast(@SubscriberInformId as nvarchar)
								EXEC spSendCall @CallBody , @Telephone, @Desc, @SubscriberAreaId, @SubscriberInformId
								EXEC spUpdateOutgoingCallStatus @SubscriberInformId, 2, 0, @AreaId
							END
							ELSE
							BEGIN
								EXEC spUpdateOutgoingCallStatus @SubscriberInformId, 5, 0, @AreaId
							END
						END
					END
				END
			END
			ELSE
				set @lIsLoop = 0
		END
		
	CLOSE crInform
	DEALLOCATE crInform
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spCheckNewFaxEvent]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spCheckNewFaxEvent]
GO
CREATE PROCEDURE [dbo].[spCheckNewFaxEvent]
AS
	DECLARE @FaxBody AS nvarchar(2000)
	DECLARE @Telephone AS nvarchar(200)
	DECLARE @IsTamir AS bit
	DECLARE @TamirDisconnectFromDT AS datetime
	DECLARE @TamirDisconnectToDT AS datetime
	DECLARE @EndJobStateId AS bigint
	DECLARE @AreaId AS integer
	DECLARE @SubscriberAreaId AS integer
	DECLARE @ServerDT AS datetime
	DECLARE @EstimateDT AS datetime
	DECLARE @SubscriberInformId AS bigint
	DECLARE @RequestType AS nvarchar(200)
	DECLARE @Diff AS int
	DECLARE @Address AS nvarchar(250)
	DECLARE @SendFaxStatusId As int
	DECLARE @IsSendSMSAfterConnect AS bit
	DECLARE @ConnectPDate as varchar(10)
	DECLARE @ConnectTime as varchar(5)
	DECLARE @TamirDisconnectFromTime AS varchar(5)
	DECLARE @TamirDisconnectToTime AS varchar(5)
	DECLARE @IsSend as bit
	SET @Diff = 0
	
	DECLARE crInform CURSOR FOR
		SELECT TOP 50
			IsTamir, 
			EndJobStateId, 
			TelFax, 
			AreaId, 
			SubscriberInformId, 
			TamirDisconnectFromDT, 
			TamirDisconnectToDT, 
			ServerDT, 
			EstimateDT, 
			SubscriberAreaId,
			Address,
			ConnectDatePersian,
			ConnectTime,
			SendFaxStatusId,
			ISNULL(IsSendSMSAfterConnect,0) AS IsSendSMSAfterConnect,
			TamirDisconnectFromTime,
			TamirDisconnectToTime
		FROM 
			View_SendFax
		WHERE
			SendFaxStatusId = 1

		OPEN crInform
		
		DECLARE @lIsLoop AS bit	
		SET @lIsLoop = 1
		WHILE @lIsLoop = 1
		BEGIN

			FETCH NEXT
				FROM crInform
				INTO 
					@IsTamir, 
					@EndJobStateId, 
					@Telephone, 
					@AreaId, 
					@SubscriberInformId, 
					@TamirDisconnectFromDT, 
					@TamirDisconnectToDT, 
					@ServerDT, 
					@EstimateDT, 
					@SubscriberAreaId,
					@Address,
					@ConnectPDate,
					@ConnectTime,
					@SendFaxStatusId,
					@IsSendSMSAfterConnect,
					@TamirDisconnectFromTime,
					@TamirDisconnectToTime

			if @@FETCH_STATUS = 0 
			BEGIN
				
				SET @IsSend = 1
				IF( @SendFaxStatusId = 1 )
				BEGIN
					IF( @IsTamir = 1 )
					BEGIN
						SET @RequestType = 'FaxBaBarnameh'
						SET @Diff = DATEDIFF(mi, @TamirDisconnectFromDT, @TamirDisconnectToDT)
						SET @EstimateDT = @TamirDisconnectFromDT
					END
					ELSE
					BEGIN
						SET @RequestType = 'FaxBiBarnameh'
						IF( @ServerDT > @EstimateDT )
						BEGIN
							EXEC spUpdateSendFaxStatus @SubscriberInformId, 5, 0, @AreaId
							SET @IsSend = 0
						END
						SET @Diff = DATEDIFF(mi, @ServerDT, @EstimateDT)
					END
					
					IF @IsSend = 1
					BEGIN
						DECLARE @lDT AS varchar(10)
						SET @lDT = dbo.mtosh(@EstimateDT)

						SET @FaxBody = NULL
						SELECT @FaxBody = ConfigText FROM Tbl_Config WHERE ConfigName = @RequestType

						IF( NOT @FaxBody IS NULL )
						BEGIN
							DECLARE @hh AS varchar(2), @mi AS varchar(2)
							SET @hh = Cast(DATEPART(hh,@EstimateDT) as varchar)
							SET @mi = Cast(DATEPART(mi,@EstimateDT) as varchar)
							IF( cast(@hh as int) < 10 ) SET @hh = '0' + @hh
							IF( cast(@mi as int) < 10 ) SET @mi = '0' + @mi
							
							SET @FaxBody = Replace( @FaxBody,'TamirDisconnectFromTime', ISNULL(@TamirDisconnectFromTime,N'ø'))
							SET @FaxBody = Replace( @FaxBody,'TamirDisconnectToTime', ISNULL(@TamirDisconnectToTime,N'ø'))
							SET @FaxBody = Replace( @FaxBody,'Address', ISNULL(@Address,N'ø'))
							SET @FaxBody = Replace( @FaxBody,'ConnectDate', ISNULL(@ConnectPDate,N'ø'))
							SET @FaxBody = Replace( @FaxBody,'ConnectTime', ISNULL(@ConnectTime,N'ø'))
							SET @FaxBody = Replace( @FaxBody,'ss', ISNULL(Cast(@Diff as nvarchar),'0'))
							SET @FaxBody = Replace( @FaxBody,'yy', SUBSTRING(@lDT,1,4))
							SET @FaxBody = Replace( @FaxBody,'mm', SUBSTRING(@lDT,6,2))
							SET @FaxBody = Replace( @FaxBody,'dd', SUBSTRING(@lDT,9,2))
							SET @FaxBody = Replace( @FaxBody,'hh', @hh )
							SET @FaxBody = Replace( @FaxBody,'nn', @mi )
							SET @FaxBody = Replace( @FaxBody,'CRLF', nchar(13) )
							
							DECLARE @Desc AS nvarchar(100)
							
							IF(@EndJobStateId IN (4,5) )
							BEGIN
								SET @Desc = @RequestType + ' ' + 'SubInform-ID=' + cast(@SubscriberInformId as nvarchar)
								EXEC spSendFax @FaxBody , @Telephone, @Desc, @SubscriberAreaId, @SubscriberInformId
								EXEC spUpdateSendFaxStatus @SubscriberInformId, 2, 0, @AreaId
							END
							ELSE
							BEGIN
								EXEC spUpdateSendFaxStatus @SubscriberInformId, 5, 0, @AreaId
							END
						END
					END
				END
			END
			ELSE
				set @lIsLoop = 0
		END
		
	CLOSE crInform
	DEALLOCATE crInform
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spCheckNewEmailEvent]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spCheckNewEmailEvent]
GO
CREATE PROCEDURE [dbo].[spCheckNewEmailEvent]
AS
	DECLARE @Email AS nvarchar(2000)
	DECLARE @EmailAddress AS nvarchar(200)
	DECLARE @IsTamir AS bit
	DECLARE @TamirDisconnectFromDT AS datetime
	DECLARE @TamirDisconnectToDT AS datetime
	DECLARE @EndJobStateId AS bigint
	DECLARE @AreaId AS integer
	DECLARE @SubscriberAreaId AS integer
	DECLARE @ServerDT AS datetime
	DECLARE @EstimateDT AS datetime
	DECLARE @SubscriberInformId AS bigint
	DECLARE @RequestType AS nvarchar(200)
	DECLARE @Diff AS int
	DECLARE @Address AS nvarchar(250)
	DECLARE @SendEmailStatusId As int
	DECLARE @IsSendSMSAfterConnect AS bit
	DECLARE @ConnectPDate as varchar(10)
	DECLARE @ConnectTime as varchar(5)
	DECLARE @TamirDisconnectFromTime AS varchar(5)
	DECLARE @TamirDisconnectToTime AS varchar(5)
	DECLARE @IsSend as bit
	SET @Diff = 0
	
	DECLARE crInform CURSOR FOR
		SELECT TOP 50
			IsTamir, 
			EndJobStateId, 
			EmailAddress, 
			AreaId, 
			SubscriberInformId, 
			TamirDisconnectFromDT, 
			TamirDisconnectToDT, 
			ServerDT, 
			EstimateDT, 
			SubscriberAreaId,
			Address,
			ConnectDatePersian,
			ConnectTime,
			SendEmailStatusId,
			ISNULL(IsSendSMSAfterConnect,0) AS IsSendSMSAfterConnect,
			TamirDisconnectFromTime,
			TamirDisconnectToTime
		FROM 
			View_SendEmail
		WHERE
			SendEmailStatusId = 1

		OPEN crInform
		
		DECLARE @lIsLoop AS bit	
		SET @lIsLoop = 1
		WHILE @lIsLoop = 1
		BEGIN

			FETCH NEXT
				FROM crInform
				INTO 
					@IsTamir, 
					@EndJobStateId, 
					@EmailAddress, 
					@AreaId, 
					@SubscriberInformId, 
					@TamirDisconnectFromDT, 
					@TamirDisconnectToDT, 
					@ServerDT, 
					@EstimateDT, 
					@SubscriberAreaId,
					@Address,
					@ConnectPDate,
					@ConnectTime,
					@SendEmailStatusId,
					@IsSendSMSAfterConnect,
					@TamirDisconnectFromTime,
					@TamirDisconnectToTime

			if @@FETCH_STATUS = 0 
			BEGIN
				
				SET @IsSend = 1
				IF( @SendEmailStatusId = 1 )
				BEGIN
					IF( @IsTamir = 1 )
					BEGIN
						SET @RequestType = 'EmailBaBarnameh'
						SET @Diff = DATEDIFF(mi, @TamirDisconnectFromDT, @TamirDisconnectToDT)
						SET @EstimateDT = @TamirDisconnectFromDT
					END
					ELSE
					BEGIN
						SET @RequestType = 'EmailBiBarnameh'
						IF( @ServerDT > @EstimateDT )
						BEGIN
							EXEC spUpdateSendEmailStatus @SubscriberInformId, 5, 0, @AreaId
							SET @IsSend = 0
						END
						SET @Diff = DATEDIFF(mi, @ServerDT, @EstimateDT)
					END
					
					IF @IsSend = 1
					BEGIN
						DECLARE @lDT AS varchar(10)
						SET @lDT = dbo.mtosh(@EstimateDT)

						SET @Email = NULL
						SELECT @Email = ConfigText FROM Tbl_Config WHERE ConfigName = @RequestType

						IF( NOT @Email IS NULL )
						BEGIN
							DECLARE @hh AS varchar(2), @mi AS varchar(2)
							SET @hh = Cast(DATEPART(hh,@EstimateDT) as varchar)
							SET @mi = Cast(DATEPART(mi,@EstimateDT) as varchar)
							IF( cast(@hh as int) < 10 ) SET @hh = '0' + @hh
							IF( cast(@mi as int) < 10 ) SET @mi = '0' + @mi
							
							SET @Email = Replace( @Email,'TamirDisconnectFromTime', ISNULL(@TamirDisconnectFromTime,N'ø'))
							SET @Email = Replace( @Email,'TamirDisconnectToTime', ISNULL(@TamirDisconnectToTime,N'ø'))
							SET @Email = Replace( @Email,'Address', ISNULL(@Address,N'ø'))
							SET @Email = Replace( @Email,'ConnectDate', ISNULL(@ConnectPDate,N'ø'))
							SET @Email = Replace( @Email,'ConnectTime', ISNULL(@ConnectTime,N'ø'))
							SET @Email = Replace( @Email,'ss', ISNULL(Cast(@Diff as nvarchar),'0'))
							SET @Email = Replace( @Email,'yy', SUBSTRING(@lDT,1,4))
							SET @Email = Replace( @Email,'mm', SUBSTRING(@lDT,6,2))
							SET @Email = Replace( @Email,'dd', SUBSTRING(@lDT,9,2))
							SET @Email = Replace( @Email,'hh', @hh )
							SET @Email = Replace( @Email,'nn', @mi )
							SET @Email = Replace( @Email,'CRLF', nchar(13) )
							
							DECLARE @Desc AS nvarchar(100)
							
							IF(@EndJobStateId IN (4,5) )
							BEGIN
								SET @Desc = @RequestType + ' ' + 'SubInform-ID=' + cast(@SubscriberInformId as nvarchar)
								EXEC spSendEmail @Email , @EmailAddress, @Desc, @SubscriberAreaId, @SubscriberInformId
								EXEC spUpdateSendEmailStatus @SubscriberInformId, 2, 0, @AreaId
							END
							ELSE
							BEGIN
								EXEC spUpdateSendEmailStatus @SubscriberInformId, 5, 0, @AreaId
							END
						END
					END
				END
			END
			ELSE
				set @lIsLoop = 0
		END
		
	CLOSE crInform
	DEALLOCATE crInform
GO


IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spCheckNewTransFault]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spCheckNewTransFault]
GO
CREATE PROCEDURE [dbo].[spCheckNewTransFault] 
AS
	DECLARE @RequestId AS BIGINT;
	DECLARE @RequestNumber AS bigint
	DECLARE @ManagerMobile AS nvarchar (200);
	DECLARE @ManagerSMSDCId AS int
	DECLARE @MPRequestId AS BIGINT;
	DECLARE @DBMinutes AS int
	DECLARE @ManagerAreaId AS int
	DECLARE @SMS AS nvarchar(2000)
	DECLARE @RequestType AS nvarchar(200)
	DECLARE @MPPostName AS nvarchar(200)
	DECLARE @MPFeederName AS nvarchar(200)
	DECLARE @LPPostName AS nvarchar(200)
	DECLARE @Reason AS nvarchar(200)
	DECLARE @DisconnectPower AS nvarchar(200)
	DECLARE @DTTime AS nvarchar(200)
	DECLARE @DTDate AS nvarchar(200)
	DECLARE @Area AS nvarchar(200)
	DECLARE @Address AS nvarchar(200)
	DECLARE @DisconnectInterval AS varchar(20)
	DECLARE @OwnerShip AS nvarchar(20)
	DECLARE @PostCapacity AS varchar(20)
	
	DECLARE @DG AS nvarchar(50)
	DECLARE @DGS AS nvarchar(100)
	
	DECLARE @CurrentDate AS varchar(10)
	DECLARE @CurrentMonthDay AS varchar(5)
	SELECT @CurrentDate = dbo.mtosh(getdate())
	SET @CurrentMonthDay = RIGHT(@CurrentDate,5)
	
	SET @RequestId = NULL
	SET @MPPostName = ''
	SET @MPFeederName = ''
	SET @LPPostName = ''
	SET @Address = ''
	SET @DisconnectPower = '-'
	SET @DTTime = ''
	SET @DTDate = ''
	SET @Reason = ''
	SET @Area = ''
	SET @ManagerMobile = ''
	SET @ManagerSMSDCId = 0
	SET @OwnerShip = N'⁄„Ê„Ì'
	SET @PostCapacity = ''
	SET @DG = ''
	SET @DGS = ''
	SET @ManagerAreaId = 0

	SELECT TOP 1 
		@RequestId = RequestId,
		@RequestNumber = RequestNumber,
		@Area = ISNULL(Area,''),
		@DBMinutes = ISNULL(DisconnectInterval,0),
		@LPPostName = ISNULL(LPPostName,''),
		@Address = ISNULL(Address,''),
		@DisconnectPower = Cast( ISNULL(DisconnectPower,0) AS varchar),
		@DTDate = ISNULL(DisconnectDatePersian,''),
		@DTTime = ISNULL(DisconnectTime,''),
		@ManagerMobile = ISNULL(ManagerMobile,''),
		@DG = ISNULL(DisconnectGroup,''),
		@DGS = ISNULL(DisconnectGroupSet,''),
		@ManagerSMSDCId = ISNULL(ManagerSMSDCId,0),
		@OwnerShip = ISNULL(OwnerShip,N'⁄„Ê„Ì'),
		@PostCapacity = ISNULL(PostCapacity,0), 
		@ManagerAreaId = ISNULL(ManagerAreaId,0)
	FROM 
		VIEWTransFaultSendSMSList
	
	IF @RequestId IS NULL RETURN 0

	SET @Reason = ISNULL(@DG,N'')
	IF( NOT @DGS IS NULL )
	BEGIN
		IF( @Reason <> N'' ) SET @Reason = @Reason + N' '
		SET @Reason = @Reason + @DGS
	END
	
	INSERT INTO TblManagerSMSDCSended (ManagerSMSDCId, RequestId, IsSendSMSAfterConnect) 
	VALUES (@ManagerSMSDCId, @RequestId, 1)
	
	SET @RequestType = 'SMSTransFault'
	SELECT @SMS = ConfigText FROM Tbl_Config WHERE ConfigName = @RequestType

	SET @SMS = Replace( @SMS,'ReqNo', ISNULL(@RequestNumber,0))
	SET @SMS = Replace( @SMS,'Minutes', ISNULL(@DBMinutes,0))
	SET @SMS = Replace( @SMS,'MPPostName', ISNULL(@MPPostName,N'ø'))
	SET @SMS = Replace( @SMS,'Area', ISNULL(@Area,N'ø'))
	SET @SMS = Replace( @SMS,'MPFeederName', ISNULL(@MPFeederName,N'ø'))
	SET @SMS = Replace( @SMS,'LPPostName', ISNULL(@LPPostName,N'ø'))
	SET @SMS = Replace( @SMS,'DBAddress', ISNULL(@Address,N'ø'))
	SET @SMS = Replace( @SMS,'DisconnectPower', ISNULL(@DisconnectPower,N'ø'))
	SET @SMS = Replace( @SMS,'DTDate', ISNULL(@DTDate,N'ø'))
	SET @SMS = Replace( @SMS,'DTTime', ISNULL(@DTTime,N'ø'))
	SET @SMS = Replace( @SMS,'Reason', ISNULL(@Reason,N'ø'))
	SET @SMS = Replace( @SMS,'OwnerShip', ISNULL(@OwnerShip,N'ø'))
	SET @SMS = Replace( @SMS,'PostCapacity', ISNULL(@PostCapacity,N'ø'))
	SET @SMS = Replace( @SMS,'CurrentDate', ISNULL(@CurrentDate,N''))
	SET @SMS = Replace( @SMS,'CurrentMonthDay', ISNULL(@CurrentMonthDay,N''))
	SET @SMS = Replace( @SMS,'CRLF', nchar(13) )

	DECLARE @Desc AS nvarchar(100)
	SET @Desc = 'ReqId=' + CAST(@RequestNumber AS nvarchar)
	EXEC spSendSMS @SMS, @ManagerMobile, @Desc, @RequestType, @ManagerAreaId

	RETURN 1
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spCheckSMSAfterEdit]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spCheckSMSAfterEdit]
GO
CREATE PROCEDURE [dbo].[spCheckSMSAfterEdit] 
AS
	DECLARE @ManagerSMSDCId AS int
	DECLARE @ManagerMobile AS nvarchar (200)
	DECLARE @ManagerAreaId AS int
	DECLARE @ManagerSMSDCSendedId AS bigint
	DECLARE @IsSendSMSAfterEdit AS bit

	DECLARE @RequestId AS bigint;
	DECLARE @RequestNumber AS bigint
	DECLARE @ChangeTypeId AS int
	DECLARE @ChangeType AS nvarchar(100)
	DECLARE @DEDT AS datetime
	DECLARE @DEPDate AS varchar(10)
	DECLARE @DETime AS varchar(5)
	DECLARE @Server121Id AS int
	DECLARE @AreaId AS int
	DECLARE @Area AS nvarchar(200)
	DECLARE @AreaUserId AS int
	DECLARE @UserName AS nvarchar(20)
	DECLARE @Comments AS nvarchar(300)
	DECLARE @IsFT AS bit
	DECLARE @IsMP AS bit
	DECLARE @IsLP AS bit
	DECLARE @IsLight AS bit

	DECLARE @SMSClause AS nvarchar(2000)
	DECLARE @SMS AS nvarchar(2000)
	DECLARE @RequestType AS nvarchar(200)
	DECLARE @ReqType as nvarchar(50)

	SET @RequestType = 'SMSAfterEdit'
	SET @SMSClause = NULL
	SELECT @SMSClause = ConfigText FROM Tbl_Config WHERE ConfigName = @RequestType
	IF @SMSClause IS NUlL RETURN 0
	
	CREATE Table #tCRH
	(
		RequestId bigint,
		RequestNumber bigint,
		ChangeTypeId int,
		ChangeType nvarchar(100),
		DEDT datetime,
		DEPDate varchar(10),
		DETime varchar(5),
		Server121Id int,
		AreaId int,
		Area nvarchar(200),
		AreaUserId int,
		UserName nvarchar(20),
		Comments nvarchar(300),
		IsFogheToziRequest bit,
		IsMPRequest bit,
		IsLPRequest bit,
		IsLightRequest bit
	)

	INSERT INTO #tCRH
	SELECT
		TblChangeRequestHistory.RequestId,
		TblRequest.RequestNumber,
		TblChangeRequestHistory.ChangeTypeId,
		Tbl_ChangeType.ChangeType,
		TblChangeRequestHistory.ChangeDT,
		TblChangeRequestHistory.ChangeDatePersian,
		TblChangeRequestHistory.ChangeTime,
		Tbl_Area.Server121Id,
		TblRequest.AreaId,
		Tbl_Area.Area, 
		TblChangeRequestHistory.AreaUserId,
		Tbl_AreaUser.UserName,
		TblChangeRequestHistory.Comments,
		TblRequest.IsFogheToziRequest,
		TblRequest.IsMPRequest,
		TblRequest.IsLPRequest,
		TblRequest.IsLightRequest
	FROM 
		TblChangeRequestHistory
		INNER JOIN Tbl_ChangeType ON TblChangeRequestHistory.ChangeTypeId = Tbl_ChangeType.ChangeTypeId
		INNER JOIN Tbl_AreaUser ON TblChangeRequestHistory.AreaUserId = Tbl_AreaUser.AreaUserId
		INNER JOIN TblRequest ON TblChangeRequestHistory.RequestId = TblRequest.RequestId
		INNER JOIN Tbl_Area ON TblRequest.AreaId = Tbl_Area.AreaId
	WHERE 
		DATEDIFF([Hour], ChangeDT, GETDATE()) <= 24
	ORDER BY
		TblChangeRequestHistory.ChangeDT DESC

	DECLARE @lSendCount AS int
	DECLARE @lFindCount AS int

	SET @lSendCount = 0

	DECLARE crEditDC CURSOR FOR
		SELECT 
			tSmsDC.ManagerSMSDCId,
			tSmsDC.ManagerMobile,
			tSmsDC.AreaId, 
			tCRH.*
		FROM
			(
				TblManagerSMSDC tSmsDC
				INNER JOIN Tbl_Area tArea ON tSmsDC.AreaId = tArea.AreaId
			)
			CROSS JOIN #tCRH tCRH
		WHERE
			tSmsDC.IsActive = 1
			AND tSmsDC.IsAfterEditRequest = 1
			AND (tSmsDC.AreaId = 99 OR tArea.Server121Id = tCRH.Server121Id OR tSmsDC.AreaId = tCRH.AreaId)

		OPEN crEditDC
		
		DECLARE @lIsLoop AS bit	
		SET @lIsLoop = 1
		WHILE @lIsLoop = 1
		BEGIN
			FETCH NEXT
				FROM crEditDC
				INTO 
					@ManagerSMSDCId, 
					@ManagerMobile, 
					@ManagerAreaId, 
					@RequestId, 
					@RequestNumber, 
					@ChangeTypeId, 
					@ChangeType, 
					@DEDT, 
					@DEPDate, 
					@DETime, 
					@Server121Id, 
					@AreaId, 
					@Area, 
					@AreaUserId, 
					@UserName, 
					@Comments, 
					@IsFT, 
					@IsMP, 
					@IsLP, 
					@IsLight
				
			if @@FETCH_STATUS = 0 
			BEGIN
				SET @SMS = @SMSClause
				SET @IsSendSMSAfterEdit  = 0
				SET @ManagerSMSDCSendedId = NULL
				
				SELECT 
					@ManagerSMSDCSendedId = ManagerSMSDCSendedId,
					@IsSendSMSAfterEdit = ISNULL(IsSendSMSAfterEdit,0)
				FROM 
					TblManagerSMSDCSended tSend
				WHERE
					ManagerSMSDCId = @ManagerSMSDCId
					AND RequestId = @RequestId
					
				IF(@IsSendSMSAfterEdit = 0)
				BEGIN
					IF @IsFT = 1
						SET @ReqType = N'›Êﬁ  Ê“Ì⁄'
					ELSE IF @IsMP = 1
						SET @ReqType = N'›‘«— „ Ê”ÿ'
					ELSE IF @IsLight = 1
						SET @ReqType = N'—Ê‘‰«ÌÌ „⁄«»—'
					ELSE IF @IsLP = 1
						SET @ReqType = N'›‘«— ÷⁄Ì›'
					ELSE
						SET @ReqType = N'ÃœÌœ'
					
					SET @SMS = Replace( @SMS,'ReqNo', ISNULL(@RequestNumber,''))
					SET @SMS = Replace( @SMS,'ReqType', ISNULL(@ReqType,''))
					SET @SMS = Replace( @SMS,'Area', ISNULL(@Area,''))
					SET @SMS = Replace( @SMS,'Username', ISNULL(@UserName,''))
					SET @SMS = Replace( @SMS,'Date', ISNULL(@DEPDate,''))
					SET @SMS = Replace( @SMS,'Time', ISNULL(@DETime,''))
					SET @SMS = Replace( @SMS,'Comment', ISNULL(@Comments,''))
					SET @SMS = Replace( @SMS,'CRLF', nchar(13) )
					
					DECLARE @Desc AS nvarchar(100)
					SET @Desc = 'Havades-ID=' + CAST(@ManagerSMSDCId AS nvarchar)
					EXEC spSendSMS @SMS, @ManagerMobile, @Desc, @RequestType, @ManagerAreaId

					IF @ManagerSMSDCSendedId IS NULL
					BEGIN
						INSERT INTO TblManagerSMSDCSended (ManagerSMSDCId, RequestId, IsSendSMSAfterEdit)
						VALUES(@ManagerSMSDCId, @RequestId, 1)
						SET @ManagerSMSDCSendedId = @@IDENTITY
					END
					ELSE
					BEGIN
						UPDATE TblManagerSMSDCSended SET IsSendSMSAfterEdit = 1 WHERE ManagerSMSDCSendedId = @ManagerSMSDCSendedId
					END
					
					SET @lSendCount = @lSendCount + 1
				END
			END
			ELSE
				SET @lIsLoop = 0
		END
	CLOSE crEditDC
	DEALLOCATE crEditDC
		
	DROP TABLE #tCRH

	RETURN @lSendCount
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spSendSMSFTMPFeederAfterConnect]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spSendSMSFTMPFeederAfterConnect]
GO
CREATE PROCEDURE [dbo].[spSendSMSFTMPFeederAfterConnect]
AS
	DECLARE @RequestId AS BIGINT;
	DECLARE @ManagerMobile AS nvarchar (200);
	DECLARE @ManagerSMSDCId AS int
	DECLARE @ManagerAreaId AS int
	DECLARE @SMS AS nvarchar(2000)
	DECLARE @RequestType AS nvarchar(200)
	DECLARE @MPPostName AS nvarchar(200)
	DECLARE @MPFeederName AS nvarchar(200)
	DECLARE @DisconnectPower AS nvarchar(200)
	DECLARE @Area AS nvarchar(200)
	DECLARE @FogheToziType AS nvarchar(200)
	DECLARE @DisconnectInterval AS varchar(20)
	DECLARE @ManagerSMSFTMPFeederDCSendedId as int
	
	DECLARE @AreaId AS int 
	
	SET @MPPostName = ''
	SET @MPFeederName = ''
	SET @DisconnectPower = '-'
	SET @FogheToziType = ''
	SET @Area = ''
	SET @ManagerMobile = ''
	SET @ManagerAreaId = 0
	
	SET @AreaId = 0

	SELECT TOP 1
		@RequestId = TblRequest.RequestId
	FROM   
		TblManagerSMSFTMPFeederDCSended 
		INNER JOIN TblManagerSMSDC ON TblManagerSMSFTMPFeederDCSended.ManagerSMSDCId = TblManagerSMSDC.ManagerSMSDCId 
		INNER JOIN TblFogheToziDisconnectMPFeeder ON TblManagerSMSFTMPFeederDCSended.FogheToziDisconnectMPFeederId = TblFogheToziDisconnectMPFeeder.FogheToziDisconnectMPFeederId
		INNER JOIN TblFogheToziDisconnect ON TblFogheToziDisconnectMPFeeder.FogheToziDisconnectId = TblFogheToziDisconnect.FogheToziDisconnectId
		INNER JOIN TblRequest ON TblFogheToziDisconnect.FogheToziDisconnectId = TblRequest.FogheToziDisconnectId
	WHERE  
		TblManagerSMSDC.IsActive = 1
		AND TblManagerSMSFTMPFeederDCSended.IsConnected = 0
		AND NOT TblFogheToziDisconnectMPFeeder.ConnectDT IS NULL
		AND (DATEDIFF([Hour], TblFogheToziDisconnectMPFeeder.ConnectDT, GETDATE()) <= 48);

	IF (@RequestId IS NULL) RETURN 0;
	
	SET @RequestType = 'SMSFTMPFeederAfterConnect'

	DECLARE crFTFeeders CURSOR FOR
		SELECT 
			TblManagerSMSFTMPFeederDCSended.ManagerSMSFTMPFeederDCSendedId,
			TblManagerSMSFTMPFeederDCSended.ManagerSMSDCId,
			Tbl_MPFeeder.MPFeederName,
			DATEDIFF(minute, TblFogheToziDisconnectMPFeeder.DisconnectDT ,TblFogheToziDisconnectMPFeeder.ConnectDT) AS DisconnectInterval,
			TblFogheToziDisconnectMPFeeder.DisconnectPower,
			Tbl_MPPost.MPPostName,
			TblManagerSMSDC.ManagerMobile,
			TblManagerSMSDC.AreaId AS ManagerAreaId,
			Tbl_Area.Area
		FROM 
			TblFogheToziDisconnect 
			INNER JOIN TblFogheToziDisconnectMPFeeder ON TblFogheToziDisconnect.FogheToziDisconnectId = TblFogheToziDisconnectMPFeeder.FogheToziDisconnectId 
			INNER JOIN TblManagerSMSFTMPFeederDCSended ON TblFogheToziDisconnectMPFeeder.FogheToziDisconnectMPFeederId = TblManagerSMSFTMPFeederDCSended.FogheToziDisconnectMPFeederId
			INNER JOIN TblManagerSMSDC ON TblManagerSMSFTMPFeederDCSended.ManagerSMSDCId = TblManagerSMSDC.ManagerSMSDCId 
			INNER JOIN Tbl_MPFeeder ON TblFogheToziDisconnectMPFeeder.MPFeederId = Tbl_MPFeeder.MPFeederId 
			INNER JOIN TblRequest ON TblFogheToziDisconnect.FogheToziDisconnectId = TblRequest.FogheToziDisconnectId 
			INNER JOIN Tbl_MPPost ON TblFogheToziDisconnect.MPPostId = Tbl_MPPost.MPPostId
			INNER JOIN Tbl_Area ON TblRequest.AreaId = Tbl_Area.AreaId
		WHERE 
			TblRequest.RequestId = @RequestId
			AND TblManagerSMSDC.IsActive = 1
			AND TblManagerSMSFTMPFeederDCSended.IsConnected = 0
			AND NOT TblFogheToziDisconnectMPFeeder.ConnectDT IS NULL
	
		OPEN crFTFeeders
		
		SELECT 
			@FogheToziType = tFTR.FogheToz 
		FROM 
			TblRequest tReq 
			INNER JOIN TblFogheToziDisconnect tFT ON tReq.FogheToziDisconnectId = tFT.FogheToziDisconnectId 
			INNER JOIN Tbl_FogheTozi tFTR ON tFT.FogheToziId = tFTR.FogheToziId 
		WHERE 
			tReq.RequestId = @RequestId
		
		DECLARE @lIsLoop AS bit
		SET @lIsLoop = 1
		WHILE @lIsLoop = 1
		BEGIN
			FETCH NEXT FROM crFTFeeders INTO @ManagerSMSFTMPFeederDCSendedId, @ManagerSMSDCId, 
						@MPFeederName, @DisconnectInterval, @DisconnectPower, @MPPostName, @ManagerMobile, 
						@ManagerAreaId, @Area

			if @@FETCH_STATUS = 0 
			BEGIN
				SELECT @SMS = ConfigText FROM Tbl_Config WHERE ConfigName = @RequestType
				SET @SMS = Replace( @SMS,'Minutes', ISNULL(@DisconnectInterval,0))
				SET @SMS = Replace( @SMS,'MPPostName', ISNULL(@MPPostName,N'ø'))
				SET @SMS = Replace( @SMS,'Area', ISNULL(@Area,N'ø'))
				SET @SMS = Replace( @SMS,'MPFeederName', ISNULL(@MPFeederName,N'ø'))
				SET @SMS = Replace( @SMS,'DisconnectPower', ISNULL(@DisconnectPower,N'ø'))
				SET @SMS = Replace( @SMS,'FogheToziType', ISNULL(@FogheToziType,N'ø'))
				
				UPDATE  TblManagerSMSFTMPFeederDCSended 
				SET IsConnected = 1 
				WHERE 
					ManagerSMSFTMPFeederDCSendedId = @ManagerSMSFTMPFeederDCSendedId 
			
				DECLARE @Desc AS nvarchar(50) = ''
				SET @Desc = 'SMSFTMPFeederDCSendedId=' + CAST(@ManagerSMSFTMPFeederDCSendedId AS varchar(25))
				EXEC spSendSMS @SMS , @ManagerMobile, @Desc, @RequestType, @ManagerAreaId
			END
			ELSE
				SET @lIsLoop = 0
		END
	CLOSE crFTFeeders
	DEALLOCATE crFTFeeders

	RETURN 1;
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spSendSMSDCManagerAfterConnect]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spSendSMSDCManagerAfterConnect]
GO
CREATE PROCEDURE [dbo].[spSendSMSDCManagerAfterConnect]
AS
	DECLARE @RequestId AS BIGINT;
	DECLARE @RequestNumber AS bigint
	DECLARE @IsSendSMSAfterConnect AS BIT;
	DECLARE @ManagerMobile AS nvarchar (200);
	DECLARE @ManagerSMSDCId AS int
	DECLARE @MPRequestId AS BIGINT;
	DECLARE @DBMinutes AS int
	DECLARE @ManagerAreaId AS int
	DECLARE @SMS AS nvarchar(2000)
	DECLARE @RequestType AS nvarchar(200)
	DECLARE @MPPostName AS nvarchar(200)
	DECLARE @MPFeederName AS nvarchar(200)
	DECLARE @FogheToziPostName AS nvarchar(200)
	DECLARE @LPPostName AS nvarchar(200)
	DECLARE @LPPostCode AS nvarchar(200)
	DECLARE @LPFeederName AS nvarchar(200)
	DECLARE @DisconnectPowerMW AS nvarchar(200)
	DECLARE @Reason AS nvarchar(200)
	DECLARE @DisconnectPower AS nvarchar(200)
	DECLARE @CurrentValueConnect AS nvarchar(200)
	DECLARE @CurrentValueConnectInfo AS nvarchar(200)
	DECLARE @DTTime AS nvarchar(200)
	DECLARE @DTDate AS nvarchar(200)
	DECLARE @ConnectDatePersian AS nvarchar(10)
	DECLARE @ConnectTime AS nvarchar(5)
	DECLARE @Area AS nvarchar(200)
	DECLARE @FogheToziType AS nvarchar(200)
	DECLARE @FogheToziTypeId AS int
	DECLARE @FogheToziShortText AS nvarchar(100)
	DECLARE @IsTotalLPPostDisconnected AS Bit
	DECLARE @IsNotDisconnectFeeder AS Bit
	DECLARE @IsDisconnectMPFeeder AS Bit
	DECLARE @MPStatus AS nvarchar(200) 
	DECLARE @Address AS nvarchar(200)
	DECLARE @Comments AS nvarchar(300)
	DECLARE @FogheToziFeederName AS nvarchar(200)
	DECLARE @RequestTypeId AS int
	DECLARE @DisconnectInterval AS varchar(20)
	
	DECLARE @IsLPRequest AS Bit
	DECLARE @IsTamir AS bit
	DECLARE	@IsMPRequest AS Bit
	DECLARE @IsFogheToziRequest AS Bit
	
	DECLARE @FromPathType As nvarchar(50)
	DECLARE @FromPathTypeValue As nvarchar(50)
	DECLARE @ToPathType As nvarchar(50)
	DECLARE @ToPathTypeValue As nvarchar(50)
	DECLARE @FeederPart As nvarchar(100)
	
	DECLARE @SubscriberType AS nvarchar(10)
	DECLARE @ZoneName AS nvarchar(100)
	
	DECLARE @DisconnectPowerMPInNowDate AS varchar(1000)
	DECLARE @DisconnectPowerMPInLastDate AS varchar(1000)
	DECLARE @AreaId AS int 
	
	DECLARE @IsSetad AS bit 
	DECLARE @IsCenter AS bit 
	DECLARE @Server121Id AS int 
	
	DECLARE @CurrentDate AS varchar(10)
	DECLARE @CurrentMonthDay AS varchar(5)
	SELECT @CurrentDate = dbo.mtosh(getdate())
	SET @CurrentMonthDay = RIGHT(@CurrentDate,5)
	
	SET @FromPathType = ''
	SET @FromPathTypeValue = ''
	SET @ToPathType = ''
	SET @ToPathTypeValue = ''
	SET @FeederPart = ''
	
	SET @MPPostName = ''
	SET @MPFeederName = ''
	SET @FogheToziPostName = '-'
	SET @LPPostName = ''
	SET @LPFeederName = ''
	SET @Address = ''
	SET @Comments = ''
	SET @FogheToziFeederName = '-'
	SET @DisconnectPower = '-'
	SET @CurrentValueConnect = '-'
	SET @CurrentValueConnectInfo = ''
	SET @DTTime = ''
	SET @DTDate = ''
	SET @FogheToziType = ''
	SET @DisconnectPowerMW = ''
	SET @Reason = ''
	SET @Area = ''
	SET @ManagerMobile = ''
	SET @MPStatus = N'ﬁÿ⁄Ì œ— ”— Œÿ'
	SET @IsTotalLPPostDisconnected = 0
	SET @IsNotDisconnectFeeder = 0
	SET @IsDisconnectMPFeeder = 0
	SET @ManagerAreaId = 0
	
	SET @IsSetad = 0
	SET @IsCenter = 0
	SET @Server121Id = -1
	SET @AreaId = 0

	SET @ConnectDatePersian = N''
	SET @ConnectTime = N''
	
	SET @SubscriberType = N'ò·Ì'
	SET @FogheToziTypeId = -1

	SELECT TOP 1 
		@RequestId = TblRequest.RequestId,
		@IsDisconnectMPFeeder = TblRequest.IsDisconnectMPFeeder,
		@IsSendSMSAfterConnect = TblManagerSMSDCSended.IsSendSMSAfterConnect,
		@ManagerMobile = TblManagerSMSDC.ManagerMobile,
		@ManagerSMSDCId = TblManagerSMSDCSended.ManagerSMSDCId,
		@MPRequestId = TblRequest.MPRequestId, 
		@ManagerAreaId = TblManagerSMSDC.AreaId,
		@RequestTypeId = TblManagerSMSDCSended.RequestTypeId,
		@IsSetad = Tbl_Area.IsSetad,
		@IsCenter = Tbl_Area.IsCenter,
		@Server121Id = Tbl_Area.Server121Id,
		@AreaId = TblRequest.AreaId
	FROM   
		TblManagerSMSDCSended 
		INNER JOIN TblManagerSMSDC ON TblManagerSMSDCSended.ManagerSMSDCId = TblManagerSMSDC.ManagerSMSDCId 
		INNER JOIN TblRequest ON TblManagerSMSDCSended.RequestId = TblRequest.RequestId
		INNER JOIN Tbl_Area ON TblManagerSMSDC.AreaId = Tbl_Area.AreaId
	WHERE  
		TblManagerSMSDC.IsActive = 1
		AND IsSendSMSAfterConnect = 0
		AND EndJobStateId IN (2, 3)
		AND (DATEDIFF([Hour], ConnectDT, GETDATE()) <= 48);

	IF (@RequestId IS NULL) RETURN 0;

	SELECT TOP 1 
		@IsNotDisconnectFeeder = IsNotDisconnectFeeder, 
		@CurrentValueConnect = ISNULL(CurrentValueConnect,CurrentValue),
		@CurrentValueConnectInfo = '»« »«— ' + ISNULL(CAST(CurrentValueConnect AS varchar),'ø') + ' ¬„Å— '
	FROM 
		TblMPRequest 
	WHERE  
		NOT @MPRequestId IS NULL
		AND MPRequestId = @MPRequestId

	SELECT 
		@RequestNumber = ISNULL(TblRequest.RequestNumber, 0),
		@DisconnectInterval = ISNULL(CAST(TblRequest.DisconnectInterval / 60 AS VARCHAR(5)) + ':' + CAST(TblRequest.DisconnectInterval % 60 AS VARCHAR(2)),'0:0'), 
		@Area = ISNULL(Tbl_Area.Area,''), 
		@MPPostName = ISNULL(Tbl_MPPost.MPPostName,''), 
		@MPFeederName = ISNULL(Tbl_MPFeeder.MPFeederName,''), 
		@FogheToziPostName = ISNULL(Tbl_MPPostFogheTozi.MPPostName,'-'), 
		@LPPostName = ISNULL(ISNULL(Tbl_LPPostMP.LPPostName,Tbl_LPPost.LPPostName),''), 
		@LPPostCode = ISNULL(ISNULL(Tbl_LPPostMP.LPPostCode,Tbl_LPPost.LPPostCode),''), 
		@LPFeederName = ISNULL(Tbl_LPFeeder.LPFeederName,''), 
		@Address = ISNULL(TblRequest.Address,''), 
		@DisconnectPower = Cast( Round(ISNULL(TblRequest.DisconnectPower,0),2) AS varchar ), 
		@DisconnectPowerMW = CAST(ISNULL(TblFogheToziDisconnect.DisconnectPowerMW,ISNULL((TblFogheToziDisconnect.DisconnectPower * 60)/ NULLIF(DATEDIFF(MI, TblFogheToziDisconnect.DisconnectDT, TblFogheToziDisconnect.ConnectDT),0) ,0)) AS DECIMAL(9, 2)),
		@DTDate = ISNULL(TblRequest.DisconnectDatePersian,''), 
		@DTTime = ISNULL(TblRequest.DisconnectTime,''), 
		@IsTotalLPPostDisconnected = ISNULL(ISNULL(TblLPRequest.IsTotalLPPostDisconnected, TblMPRequest.IsTotalLPPostDisconnected), 0), 
		@IsLPRequest = TblRequest.IsLPRequest, 
		@IsTamir = TblRequest.IsTamir, 
		@IsMPRequest = TblRequest.IsMPRequest, 
		@IsFogheToziRequest = TblRequest.IsFogheToziRequest, 
		@ConnectDatePersian = TblRequest.ConnectDatePersian, 
		@ConnectTime = TblRequest.ConnectTime,
		@SubscriberType = CASE WHEN ISNULL(TblLPRequest.IsSingleSubscriber, TblRequest.IsSingleSubscriber) = 1 THEN N' òÌ' ELSE N'ò·Ì' END,
		@ZoneName = ISNULL(Tbl_Zone.ZoneName,''),
		@FogheToziTypeId = TblFogheToziDisconnect.FogheToziId
	FROM   
		TblRequest
		LEFT OUTER JOIN TblFogheToziDisconnect ON TblRequest.FogheToziDisconnectId = TblFogheToziDisconnect.FogheToziDisconnectId 
		LEFT OUTER JOIN TblMPRequest ON TblRequest.MPRequestId = TblMPRequest.MPRequestId 
		LEFT OUTER JOIN TblLPRequest ON TblLPRequest.LPRequestId = TblRequest.LPRequestId 
		LEFT OUTER JOIN Tbl_Area ON TblRequest.AreaId = Tbl_Area.AreaId 
		LEFT OUTER JOIN Tbl_MPPost Tbl_MPPostFogheTozi ON TblFogheToziDisconnect.MPPostId = Tbl_MPPostFogheTozi.MPPostId 
		LEFT OUTER JOIN Tbl_MPPost ON TblMPRequest.MPPostId = Tbl_MPPost.MPPostId 
		LEFT OUTER JOIN Tbl_MPFeeder ON TblMPRequest.MPFeederId = Tbl_MPFeeder.MPFeederId 
		LEFT OUTER JOIN Tbl_LPPost Tbl_LPPostMP ON TblMPRequest.LPPostId = Tbl_LPPostMP.LPPostId 
		LEFT OUTER JOIN Tbl_LPPost ON TblLPRequest.LPPostId = Tbl_LPPost.LPPostId 
		LEFT OUTER JOIN Tbl_LPFeeder ON TblLPRequest.LPFeederId = Tbl_LPFeeder.LPFeederId 
		LEFT OUTER JOIN Tbl_FogheTozi ON TblFogheToziDisconnect.FogheToziId = Tbl_FogheTozi.FogheToziId 
		LEFT OUTER JOIN Tbl_Zone ON TblRequest.ZoneId = Tbl_Zone.ZoneId
	WHERE 
		TblRequest.RequestId = @RequestId
		
	IF @IsFogheToziRequest = 1
		SELECT TOP 1 
			@CurrentValueConnect = TblFogheToziDisconnect.CurrentValue
		FROM 
			TblRequest
			INNER JOIN TblFogheToziDisconnect ON TblRequest.FogheToziDisconnectId = TblFogheToziDisconnect.FogheToziDisconnectId 
		WHERE  
			TblRequest.RequestId = @RequestId
		
	SELECT
		@FromPathType = ISNULL(ISNULL(tFPTMP.PathType,tFPTLP.PathType),''),
		@FromPathTypeValue = ISNULL(ISNULL(TblMPRequest.FromPathTypeValue,TblLPRequest.FromPathTypeValue),''),
		@ToPathType = ISNULL(ISNULL(tTPTMP.PathType,tTPTLP.PathType),''),
		@ToPathTypeValue = ISNULL(ISNULL(TblMPRequest.ToPathTypeValue,TblLPRequest.ToPathTypeValue),''),
		@FeederPart = ISNULL(tFP.FeederPart,'')
	FROM
		TblRequest
		LEFT OUTER JOIN TblMPRequest ON TblRequest.MPRequestId = TblMPRequest.MPRequestId
		LEFT OUTER JOIN TblLPRequest ON TblRequest.LPRequestId = TblLPRequest.LPRequestId
		LEFT OUTER JOIN Tbl_PathType tFPTMP ON TblMPRequest.FromPathTypeID = tFPTMP.PathTypeId
		LEFT OUTER JOIN Tbl_PathType tFPTLP ON TblLPRequest.FromPathTypeID = tFPTLP.PathTypeId
		LEFT OUTER JOIN Tbl_PathType tTPTMP ON TblMPRequest.ToPathTypeID = tTPTMP.PathTypeId
		LEFT OUTER JOIN Tbl_PathType tTPTLP ON TblLPRequest.ToPathTypeID = tTPTLP.PathTypeId
		LEFT OUTER JOIN Tbl_FeederPart tFP ON TblMPRequest.FeederPartId = tFP.FeederPartId
	WHERE
		TblRequest.RequestId = @RequestId
		
	SELECT
		@Comments = REPLACE(CASE 
			WHEN IsFogheToziRequest = 1 THEN ISNULL(TblFogheToziDisconnect.Comments,'')
			ELSE '\n' +  ISNULL(TblMPRequest.Comments,'') + '\n' +  ISNULL(TblLPRequest.Comments,'')
		END, '\n\n', '\n')
	FROM
		TblRequest
		LEFT OUTER JOIN TblMPRequest ON TblRequest.MPRequestId = TblMPRequest.MPRequestId
		LEFT OUTER JOIN TblLPRequest ON TblRequest.LPRequestId = TblLPRequest.LPRequestId
		LEFT OUTER JOIN TblFogheToziDisconnect ON TblRequest.FogheToziDisconnectId = TblFogheToziDisconnect.FogheToziDisconnectId
	WHERE
		TblRequest.RequestId = @RequestId
		

	IF @Comments = '\n' SET @Comments = ''
	SET @DisconnectInterval = Replace(@DisconnectInterval,'*','0')
	SET @DBMinutes = Cast( left( @DisconnectInterval , charindex(':',@DisconnectInterval) - 1 ) AS int ) * 60
	SET @DBMinutes = @DBMinutes + Cast( right( @DisconnectInterval , len(@DisconnectInterval) - charindex(':',@DisconnectInterval) ) AS int )

	IF( @IsTotalLPPostDisconnected = 1 )
		SET @MPStatus = N'ﬁÿ⁄ Å”   Ê“Ì⁄'
	IF( @IsNotDisconnectFeeder = 1 )
		SET @MPStatus = N'ﬁÿ⁄Ì œ— ”— Œÿ'
	IF( @IsDisconnectMPFeeder = 1 )
		SET @MPStatus = N'ﬁÿ⁄ ò«„· ›Ìœ—'
		
	IF @RequestTypeId = 1 -- FogheTozi
	BEGIN
		SET @FogheToziShortText = CASE
			WHEN @FogheToziTypeId = 1 THEN N'ò„»Êœ  Ê·Ìœ'
			WHEN @FogheToziTypeId IN (2,3,4) THEN N'«‰ ﬁ«·-»«»—‰«„Â'
			WHEN @FogheToziTypeId = 5 THEN N'«‰ ﬁ«·-»Ì »—‰«„Â'
			WHEN @FogheToziTypeId IN (6,7,8) THEN N'›Êﬁ  Ê“Ì⁄-»«»—‰«„Â'
			WHEN @FogheToziTypeId = 9 THEN N'›Êﬁ  Ê“Ì⁄-»Ì »—‰«„Â'
			ELSE N''
		END
	END
	
	/*----- ·Ì”  Ê’· Â«Ì ç‰œ „—Õ·Â «Ì ------*/
	DECLARE @MultiStepConnections AS VARCHAR(2000)
	SET @MultiStepConnections = ''
	EXEC @MultiStepConnections = dbo.GetMultiStepConnections @RequestId
	IF LEN(@MultiStepConnections) > 1
		SET @CurrentValueConnectInfo = ''

	/*------------*/

	DECLARE @NowDatePersian AS VARCHAR(10)
	DECLARE @LastDatePersian AS VARCHAR(10)

	SET @NowDatePersian = dbo.mtosh(getDate())
	SET @LastDatePersian = CAST(CAST(LEFT(@NowDatePersian, 4) AS INT) - 1 AS VARCHAR) + RIGHT(@NowDatePersian, 6)
	
	SELECT 
		@DisconnectPowerMPInNowDate = SUM(TblMPRequest.DisconnectPower) 
	FROM 
		TblRequest 
		INNER JOIN TblMPRequest ON TblRequest.MPRequestId = TblMPRequest.MPRequestId
		INNER JOIN Tbl_Area ON TblRequest.AreaId = Tbl_Area.AreaId
	WHERE 
		TblRequest.DisconnectDatePersian >= LEFT(@NowDatePersian,8) + '01'
		AND TblRequest.DisconnectDatePersian <= @NowDatePersian
		AND TblRequest.IsMPRequest = 1 
		AND Tbl_Area.AreaId = @AreaId
		AND ISNULL(TblMPRequest.IsWarmLine,0)= 0
		
	SELECT 
		@DisconnectPowerMPInLastDate = SUM(TblMPRequest.DisconnectPower) 
	FROM 
		TblRequest 
		INNER JOIN TblMPRequest ON TblRequest.MPRequestId = TblMPRequest.MPRequestId
		INNER JOIN Tbl_Area ON TblRequest.AreaId = Tbl_Area.AreaId
	WHERE 
		TblRequest.DisconnectDatePersian >= LEFT(@LastDatePersian,8) + '01'
		AND TblRequest.DisconnectDatePersian <= @LastDatePersian
		AND TblRequest.IsMPRequest = 1 
		AND Tbl_Area.AreaId = @AreaId
		AND ISNULL(TblMPRequest.IsWarmLine,0)= 0
		
	IF @RequestTypeId IS NULL OR @RequestTypeId = 0
	BEGIN
		IF @IsLPRequest = 1
		BEGIN
			IF @IsTamir = 1
				SET @RequestTypeId = 2
			ELSE
				SET @RequestTypeId = 3
		END

		IF @IsMPRequest = 1
		BEGIN
			IF @IsTamir = 1
				SET @RequestTypeId = 4
			ELSE
				SET @RequestTypeId = 5
		END
		IF @IsFogheToziRequest = 1
			SET @RequestTypeId = 1
	END

	DECLARE @DG AS nvarchar(50)
	DECLARE @DGS AS nvarchar(100)
	SELECT 
		@DGS = Tbl_DisconnectGroupSet.DisconnectGroupSet, 
		@DG = Tbl_DisconnectGroup.DisconnectGroup,
		@FogheToziType = tFTR.FogheToz 
	FROM 
		TblRequest tReq 
		/* LEFT OUTER JOIN TblLPRequest tLP ON tReq.LPRequestId = tLP.LPRequestId  */
		/* LEFT OUTER JOIN Tbl_DisconnectGroupSet tDGSLP ON tLP.DisconnectGroupSetId = tDGSLP.DisconnectGroupSetId  */
		/* LEFT OUTER JOIN Tbl_DisconnectGroup tDGLP ON tDGSLP.DisconnectGroupId = tDGLP.DisconnectGroupId  */
		/* LEFT OUTER JOIN TblMPRequest tMP ON tReq.MPRequestId = tMP.MPRequestId  */
		/* LEFT OUTER JOIN Tbl_DisconnectGroupSet tDGSMP ON tMP.DisconnectGroupSetId = tDGSMP.DisconnectGroupSetId  */
		/* LEFT OUTER JOIN Tbl_DisconnectGroup tDGMP ON tDGSMP.DisconnectGroupId = tDGMP.DisconnectGroupId  */
		LEFT JOIN Tbl_DisconnectGroupSet ON tReq.DisconnectGroupSetId = Tbl_DisconnectGroupSet.DisconnectGroupSetId
		LEFT JOIN Tbl_DisconnectGroup ON Tbl_DisconnectGroupSet.DisconnectGroupId = Tbl_DisconnectGroup.DisconnectGroupId
		LEFT OUTER JOIN TblFogheToziDisconnect tFT ON tReq.FogheToziDisconnectId = tFT.FogheToziDisconnectId 
		LEFT OUTER JOIN Tbl_FogheTozi tFTR ON tFT.FogheToziId = tFTR.FogheToziId 
	WHERE 
		tReq.RequestId = @RequestId

	SET @Reason = ISNULL(@DG,N'')
	IF( NOT @DGS IS NULL )
	BEGIN
		IF( @Reason <> N'' ) SET @Reason = @Reason + N' '
		SET @Reason = @Reason + @DGS
	END
	
	SET @FogheToziType = ISNULL(@FogheToziType,N'')
	
	IF @RequestTypeId = 1 -- Foghe Tozi
	BEGIN
		SET @RequestType = 'SMSFTAfterConnect'

		SET @FogheToziFeederName = '-'
		
		DECLARE @FTFName AS nvarchar(200)
		
		DECLARE crFTFeeders CURSOR FOR
			SELECT 
				Tbl_MPFeeder.MPFeederName
			FROM 
				TblFogheToziDisconnect 
				INNER JOIN TblFogheToziDisconnectMPFeeder ON TblFogheToziDisconnect.FogheToziDisconnectId = TblFogheToziDisconnectMPFeeder.FogheToziDisconnectId 
				INNER JOIN Tbl_MPFeeder ON TblFogheToziDisconnectMPFeeder.MPFeederId = Tbl_MPFeeder.MPFeederId 
				INNER JOIN TblRequest ON TblFogheToziDisconnect.FogheToziDisconnectId = TblRequest.FogheToziDisconnectId 
			WHERE 
				TblRequest.RequestId = @RequestId

			OPEN crFTFeeders
			
			FETCH NEXT FROM crFTFeeders INTO @FTFName

				WHILE @@FETCH_STATUS = 0
				BEGIN
					IF( @FogheToziFeederName = '-' )
						SET @FogheToziFeederName = @FTFName
					ELSE
						SET @FogheToziFeederName = @FogheToziFeederName + 'CRLF' + @FTFName

					FETCH NEXT FROM crFTFeeders INTO @FTFName
				END
		CLOSE crFTFeeders
		DEALLOCATE crFTFeeders
		
		IF( @FogheToziFeederName = '-' )
			SET @RequestType = 'SMSFT2AfterConnect'
	END
	ELSE IF @RequestTypeId = 2 -- LPTamir
	BEGIN
		SET @RequestType = 'SMSLPTamirAfterConnect'
	END
	ELSE IF @RequestTypeId = 3 -- LPNotTamir
	BEGIN
		SET @RequestType = 'SMSLPNotTamirAfterConnect'
	END
	ELSE IF @RequestTypeId = 4 -- MPTamir
	BEGIN
		IF @IsNotDisconnectFeeder = 1
			SET @RequestType = 'SMSMPTamirAfterConnectFeederPart'
		ELSE IF @IsTotalLPPostDisconnected = 1
			SET @RequestType = 'SMSMPTamirAfterConnectPost'
		ELSE
			SET @RequestType = 'SMSMPTamirAfterConnect'
	END
	ELSE IF @RequestTypeId = 5 -- MPNotTamir
	BEGIN
		IF @IsNotDisconnectFeeder = 1
			SET @RequestType = 'SMSMPNotTamirAfterConnectFeederPart'
		ELSE IF @IsTotalLPPostDisconnected = 1
			SET @RequestType = 'SMSMPNotTamirAfterConnectPost'
		ELSE
			SET @RequestType = 'SMSMPNotTamirAfterConnect'
	END
	ELSE IF @RequestTypeId = 6 -- LPTamir All
	BEGIN
		SET @RequestType = 'SMSLPTamirAllAfterConnect'
	END
	ELSE IF @RequestTypeId = 7 -- LPTamir Single
	BEGIN
		SET @RequestType = 'SMSLPTamirSingleAfterConnect'
	END
	ELSE IF @RequestTypeId = 8 -- LPNotTamir All
	BEGIN
		SET @RequestType = 'SMSLPNotTamirAllAfterConnect'
	END
	ELSE IF @RequestTypeId = 9 -- LPNotTamir Single
	BEGIN
		SET @RequestType = 'SMSLPNotTamirSingleAfterConnect'
	END
	ELSE IF @RequestTypeId = 10
	BEGIN
		IF @IsNotDisconnectFeeder = 1
			SET @RequestType = 'SMSMPTamirAfterConnectFeederPart'
		ELSE
			SET @RequestType = 'SMSMPTamirAllAfterConnect'
	END
	ELSE IF @RequestTypeId = 11
	BEGIN
		SET @RequestType = 'SMSMPTamirAfterConnectPost'
	END
	ELSE IF @RequestTypeId = 12
	BEGIN
		IF @IsNotDisconnectFeeder = 1
			SET @RequestType = 'SMSMPNotTamirAfterConnectFeederPart'
		ELSE
			SET @RequestType = 'SMSMPNotTamirAllAfterConnect'
	END
	ELSE IF @RequestTypeId = 13
	BEGIN
		SET @RequestType = 'SMSMPNotTamirAfterConnectPost'
	END

	SELECT @SMS = ConfigText FROM Tbl_Config WHERE ConfigName = @RequestType

	SET @SMS = Replace( @SMS,'ReqNo', ISNULL(@RequestNumber,0))
	SET @SMS = Replace( @SMS,'Minutes', ISNULL(@DBMinutes,0))
	SET @SMS = Replace( @SMS,'FogheToziPostName', ISNULL(@FogheToziPostName,N'ø'))
	SET @SMS = Replace( @SMS,'MPPostName', ISNULL(@MPPostName,N'ø'))
	SET @SMS = Replace( @SMS,'Area', ISNULL(@Area,N'ø'))
	SET @SMS = Replace( @SMS,'MPFeederName', ISNULL(@MPFeederName,N'ø'))
	SET @SMS = Replace( @SMS,'LPPostName', ISNULL(@LPPostName,N'ø'))
	SET @SMS = Replace( @SMS,'LPPostCode', ISNULL(@LPPostCode,N'ø'))
	SET @SMS = Replace( @SMS,'LPFeederName', ISNULL(@LPFeederName,N'ø'))
	SET @SMS = Replace( @SMS,'DBAddress', ISNULL(@Address,N'ø'))
	SET @SMS = Replace( @SMS,'FogheToziFeederName', ISNULL(@FogheToziFeederName,N'ø'))
	SET @SMS = Replace( @SMS,'DisconnectPowerMPInNowDate', ISNULL(@DisconnectPowerMPInNowDate,N'ø'))
	SET @SMS = Replace( @SMS,'DisconnectPowerMPInLastDate', ISNULL(@DisconnectPowerMPInLastDate,N'ø'))
	SET @SMS = Replace( @SMS,'DisconnectPowerMW', ISNULL(@DisconnectPowerMW ,N'ø'))
	SET @SMS = Replace( @SMS,'DisconnectPower', ISNULL(@DisconnectPower,N'ø'))
	SET @SMS = Replace( @SMS,'CurrentValueConnectInfo', ISNULL(@CurrentValueConnectInfo,N''))
	SET @SMS = Replace( @SMS,'CurrentValueConnect', ISNULL(@CurrentValueConnect,N'ø'))
	SET @SMS = Replace( @SMS,'DTDate', ISNULL(@DTDate,N'ø'))
	SET @SMS = Replace( @SMS,'DTTime', ISNULL(@DTTime,N'ø'))
	SET @SMS = Replace( @SMS,'ConnectDate', ISNULL(@ConnectDatePersian,N''))
	SET @SMS = Replace( @SMS,'ConnectTime', ISNULL(@ConnectTime,N''))
	SET @SMS = Replace( @SMS,'FogheToziType', ISNULL(@FogheToziType,N'ø'))
	SET @SMS = Replace( @SMS,'FogheToziShortText', ISNULL(@FogheToziShortText,N'ø'))
	SET @SMS = Replace( @SMS,'Reason', ISNULL(@Reason,N'ø'))
	SET @SMS = Replace( @SMS,'MPStatus', ISNULL(@MPStatus,N'ø'))
	SET @SMS = Replace( @SMS,'FromType', CASE WHEN NULLIF(@FromPathType,N'') IS NOT NULL THEN N'«“ ' + @FromPathType ELSE '' END )
	SET @SMS = Replace( @SMS,'FromValue', @FromPathTypeValue)
	SET @SMS = Replace( @SMS,'ToType', CASE WHEN NULLIF(@ToPathType,N'') IS NOT NULL THEN N' « ' + @ToPathType ELSE '' END )
	SET @SMS = Replace( @SMS,'ToValue', @ToPathTypeValue)
	SET @SMS = Replace( @SMS,'FeederPart', @FeederPart)
	SET @SMS = Replace( @SMS,'SubscriberType', ISNULL(@SubscriberType,N'ø'))
	SET @SMS = Replace( @SMS,'ZoneName', ISNULL(@ZoneName,N'ø'))
	SET @SMS = Replace( @SMS,'Comments', REPLACE(@Comments,'\n','CRLF'))
	SET @SMS = Replace( @SMS,'MultiStepConnections', ISNULL(@MultiStepConnections,''))
	SET @SMS = Replace( @SMS,'CurrentDate', ISNULL(@CurrentDate,''))
	SET @SMS = Replace( @SMS,'CurrentMonthDay', ISNULL(@CurrentMonthDay,''))
	SET @SMS = Replace( @SMS,'CRLF', nchar(13) )

	UPDATE  TblManagerSMSDCSended 
	SET IsSendSMSAfterConnect = 1 
	WHERE 
		RequestId = @RequestId 
		AND ManagerSMSDCId= @ManagerSMSDCId

	DECLARE @Desc AS nvarchar(100)
	SET @Desc = 'ReqId=' + Cast(@RequestNumber as nvarchar)

	DECLARE @IsSendSeparateMPFeederConnect AS bit = 0	
	IF @RequestType = 'SMSFTAfterConnect'
	BEGIN
		SELECT 
			@IsSendSeparateMPFeederConnect = CAST (ConfigValue AS bit)
		FROM 
			Tbl_Config 
		WHERE 
			ConfigName = 'IsSendSeparateMPFeederConnect'
	END 
	IF @IsSendSeparateMPFeederConnect = 0
		EXEC spSendSMS @SMS , @ManagerMobile, @Desc, @RequestType, @ManagerAreaId
	
	SELECT *
	FROM TblManagerSMSDCSended 
	WHERE 
		RequestId = @RequestId
		AND ManagerSMSDCId= @ManagerSMSDCId

	RETURN 1;
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spCheckNewSTManagerPeak]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spCheckNewSTManagerPeak]
GO
CREATE PROCEDURE [dbo].[spCheckNewSTManagerPeak]
AS
	DECLARE @RequestType AS nvarchar(200)
	DECLARE @SMS AS nvarchar(2000)
	DECLARE @SMSClause AS nvarchar(2000)

	DECLARE @ManagerSMSSTId AS int
	DECLARE @ManagerMobile AS varchar(20)
	DECLARE @AreaId AS int
	DECLARE @IsCenter AS bit
	DECLARE @IsSetad AS bit
	DECLARE @Server121Id AS int
	DECLARE @Area AS nvarchar(50)
	DECLARE @ParentAreaId AS int
	DECLARE @SendTime AS varchar(5)
	DECLARE @LastSendTime AS datetime

	DECLARE @lToday AS datetime
	DECLARE @lThisTime AS datetime
	DECLARE @lHour AS int
	DECLARE @lMin AS int
	DECLARE @lDiff AS int, @lDiffNow AS int

	SET @lToday = GETDATE()
	SET @lToday = DATEADD( ms, - DATEPART(ms,@lToday), @lToday )
	SET @lToday = DATEADD( ss, - DATEPART(ss,@lToday), @lToday )
	SET @lToday = DATEADD( mi, - DATEPART(mi,@lToday), @lToday )
	SET @lToday = DATEADD( hh, - DATEPART(hh,@lToday), @lToday )

	DECLARE @PeakValue AS float
	DECLARE @PeakDate AS varchar(10)
	DECLARE @Percent AS float
	DECLARE @MaxPeak AS float
	DECLARE @MaxPeakDate AS varchar(10)
	DECLARE @LastMaxPeak AS float
	DECLARE @MaxPercent AS float
	
	DECLARE @PeakAsynch AS float
	DECLARE @MaxPeakAsynch AS float
	DECLARE @PercentAsynch AS float
	DECLARE @LastPeakAsynch AS float
	DECLARE @MaxPeakAsynchDate AS varchar(10)
	
	DECLARE @lYear AS varchar(4)
	DECLARE @LastDate AS varchar(10)
	DECLARE @LastPeak AS float
	DECLARE @FirstYearDate AS varchar(10)
	DECLARE @LastPeakDate AS varchar(10)
	
	DECLARE @FirstPastYear AS varchar(10)
	DECLARE @LastPastYear AS varchar(10)

	DECLARE @lQuery AS varchar(1000)
	DECLARE @lSql AS varchar(2000)
	DECLARE @lPDate AS varchar(10)
	DECLARE @lDT AS datetime
	
	DECLARE @PeakDayNight AS float
	DECLARE @MaxPeakDayNight AS float
	DECLARE @MaxDayNightPercent AS float
	
	DECLARE @MainPeakProtocol AS varchar(20)
	
	DECLARE @TopPeak as float
	DECLARE @ForcastPeak as float
	DECLARE @LoadManagement as float
	
	SET @MainPeakProtocol = ''

	DECLARE @lCnt AS int
	SET @lCnt = 0

	SET @RequestType = 'SMSDailyPeak'
	SET @SMSClause = NULL
	SELECT @SMSClause = ConfigText FROM Tbl_Config WHERE ConfigName = @RequestType
	IF @SMSClause IS NUlL RETURN 0

	/*-------------*/
	SET @PeakDate = ''
	SET @PeakValue = 0
	SET @Percent = 0
	SET @MaxPercent = 0
	SET @MaxPeak = 0
	SET @LastMaxPeak = 0
	SET @MaxPeakDate = '-'
	SET @lYear = ''
	SET @LastDate = '-'
	SET @LastPeak = 0
	SET @FirstYearDate = '-'
	SET @FirstPastYear = '-'
	SET @LastPastYear = '-'
	SET @LastPeakDate = '-'
	SET @PeakDayNight = 0
	SET @MaxPeakDayNight = 0
	SET @MaxDayNightPercent = 0
	
	SET @PeakAsynch = 0
	SET @MaxPeakAsynch = 0
	SET @PercentAsynch = 0
	SET @LastPeakAsynch = 0
	SET @MaxPeakAsynchDate = '-'
	SET @TopPeak = 0
	SET @ForcastPeak = 0
	SET @LoadManagement = 0

	SELECT 
		@MainPeakProtocol = ConfigValue
	FROM 
		Tbl_Config
	WHERE 
		ConfigName = 'MainPeakProtocol'
	

	SELECT TOP 1 
		@PeakDate = PeakDatePersian,
		@PeakValue = CASE WHEN @MainPeakProtocol = '' THEN 
						PeakSynch
					ELSE CASE WHEN @MainPeakProtocol = 'MaxPeak'
							THEN CASE WHEN PeakSynch > PeakAsynch
								THEN PeakSynch
							ELSE 
								PeakAsynch
							END
						END
					END,
		@lYear = LEFT(PeakDatePersian, 4),
		@PeakAsynch = PeakAsynch,
		@TopPeak = TopPeak,
		@ForcastPeak = ForcastPeak,
		@LoadManagement = LoadManagement
		
	FROM TblMainPeak
	WHERE CASE WHEN @MainPeakProtocol = '' THEN 
						PeakSynch
					ELSE CASE WHEN @MainPeakProtocol = 'MaxPeak'
							THEN CASE WHEN PeakSynch > PeakAsynch
								THEN PeakSynch
							ELSE 
								PeakAsynch
							END
						END
					END > 0
	ORDER BY PeakDatePersian DESC,
		PeakTime DESC

	
	IF @PeakDate <> ''
	BEGIN
		SET @FirstYearDate = @lYear + '/01/01'
		SET @lYear = Cast( (Cast(@lYear As int) - 1) AS varchar)
		SET @LastDate = @lYear + Right(@PeakDate,6)
		SET @FirstPastYear = @lYear + '/01/01'
		SET @LastPastYear = @lYear + '/12/30'
		
		SELECT 
			@LastPeak = CASE WHEN @MainPeakProtocol = '' THEN 
						PeakSynch
					ELSE CASE WHEN @MainPeakProtocol = 'MaxPeak'
							THEN CASE WHEN PeakSynch > PeakAsynch
								THEN PeakSynch
							ELSE 
								PeakAsynch
							END
						END
					END ,
			@LastPeakAsynch = PeakAsynch
		FROM TblMainPeak
		WHERE PeakDatePersian = @LastDate
		
		IF @LastPeak > 0 
		BEGIN
			SET @Percent = ROUND((@PeakValue - @LastPeak) / @LastPeak * 100,2)
		END
		
		IF @LastPeakAsynch > 0 
		BEGIN
			SET @PercentAsynch = ROUND((@PeakAsynch - @LastPeakAsynch) / @LastPeakAsynch * 100,2)
		END
		
		SELECT @MaxPeak = 
					MAX(CASE WHEN @MainPeakProtocol = '' THEN 
							PeakSynch
						ELSE CASE WHEN @MainPeakProtocol = 'MaxPeak'
								THEN CASE WHEN PeakSynch > PeakAsynch
									THEN PeakSynch
								ELSE 
									PeakAsynch
								END
							END
						END )
		FROM TblMainPeak
		WHERE PeakDatePersian >= @FirstYearDate AND PeakDatePersian <= @PeakDate

		SELECT @MaxPeakAsynch = MAX(PeakAsynch)
		FROM TblMainPeak
		WHERE PeakDatePersian >= @FirstYearDate AND PeakDatePersian <= @PeakDate
		
		SELECT @MaxPeakDate = PeakDatePersian
		FROM TblMainPeak
		WHERE 
			PeakDatePersian >= @FirstYearDate AND PeakDatePersian <= @PeakDate
			AND CASE WHEN @MainPeakProtocol = '' THEN 
					PeakSynch
				ELSE CASE WHEN @MainPeakProtocol = 'MaxPeak'
						THEN CASE WHEN PeakSynch > PeakAsynch
							THEN PeakSynch
						ELSE 
							PeakAsynch
						END
					END
				END = @MaxPeak
				
		SELECT @MaxPeakAsynchDate = PeakDatePersian
		FROM TblMainPeak
		WHERE
			PeakDatePersian >= @FirstPastYear AND PeakDatePersian <= @PeakDate
			AND PeakAsynch = @MaxPeakAsynch
				
		SELECT @LastMaxPeak = 
					ISNULL(MAX(CASE WHEN @MainPeakProtocol = '' THEN 
							PeakSynch
						ELSE CASE WHEN @MainPeakProtocol = 'MaxPeak'
								THEN CASE WHEN PeakSynch > PeakAsynch
									THEN PeakSynch
								ELSE 
									PeakAsynch
								END
							END
						END ),0)
		FROM TblMainPeak
		WHERE PeakDatePersian >= @FirstPastYear AND PeakDatePersian <= @LastPastYear
		
		SELECT @LastPeakDate = PeakDatePersian
		FROM TblMainPeak
		WHERE 
			PeakDatePersian >= @FirstPastYear AND PeakDatePersian <= @LastPastYear
			AND CASE WHEN @MainPeakProtocol = '' THEN 
					PeakSynch
				ELSE CASE WHEN @MainPeakProtocol = 'MaxPeak'
						THEN CASE WHEN PeakSynch > PeakAsynch
							THEN PeakSynch
						ELSE 
							PeakAsynch
						END
					END
				END = @LastMaxPeak
		
		IF @LastMaxPeak > 0
		BEGIN
			SET @MaxPercent = ROUND((@MaxPeak - @LastMaxPeak) / @LastMaxPeak * 100,2)
		END

	/*-------------*/		
	SELECT TOP 1 
		@PeakDayNight = ISNULL(CASE WHEN PeakDay > PeakNight
							THEN PeakDay
						ELSE 
							PeakNight
						END,0)
	FROM TblMainPeak
	WHERE PeakDatePersian = @PeakDate
	
	SELECT @MaxPeakDayNight = ISNULL(MAX(CASE 
				WHEN PeakDay > PeakNight
					THEN PeakDay
				ELSE PeakNight
				END),0)
	FROM TblMainPeak
	WHERE PeakDatePersian >= @FirstYearDate AND PeakDatePersian <= @PeakDate
	
	END
	ELSE
		SET @PeakDate = '-'
		
		
	/*-------------*/
	
	DECLARE crSMSST CURSOR FOR
		SELECT 
			TblManagerSMSST.ManagerSMSSTId,
			TblManagerSMSST.ManagerMobile,
			TblManagerSMSST.AreaId,
			Tbl_Area.IsCenter,
			Tbl_Area.IsSetad,
			Tbl_Area.Server121Id,
			Tbl_Area.Area,
			Tbl_Area.ParentAreaId,
			TblManagerSMSST.Peak_SendTime,  
			TblManagerSMSST.Peak_LastSendTime 
		FROM 
			TblManagerSMSST
			INNER JOIN Tbl_Area ON TblManagerSMSST.AreaId = Tbl_Area.AreaId
		WHERE 
			Peak_IsActive = 1

		OPEN crSMSST
		
		DECLARE @lIsLoop AS bit	
		SET @lIsLoop = 1
		WHILE @lIsLoop = 1
		BEGIN
			FETCH NEXT FROM crSMSST
			INTO 
				@ManagerSMSSTId, 
				@ManagerMobile, 
				@AreaId, 
				@IsCenter, 
				@IsSetad, 
				@Server121Id, 
				@Area, 
				@ParentAreaId, 
				@SendTime, 
				@LastSendTime
			
			if @@FETCH_STATUS = 0 
			BEGIN
				SET @SMS = @SMSClause
				SET @lHour = Cast(LEFT(@SendTime,2) AS int)
				SET @lMin = Cast(RIGHT(@SendTime,2) AS int)
				SET @lThisTime = DATEADD(hh, @lHour, @lToday)
				SET @lThisTime = DATEADD(mi, @lMin, @lThisTime)
				SET @lDiff = ISNULL(DATEDIFF(mi, @LastSendTime, @lThisTime),1440)
				SET @lDiffNow = DATEDIFF(mi, @lThisTime, GETDATE())
				
				IF( @lDiffNow >= 0 AND @lDiff > 10 )
				BEGIN
					SET @lDT = GETDATE()
					SET @lPDate = dbo.mtosh(@lDT)
					
					SET @SMS = Replace( @SMS,'yy', SUBSTRING(@lPDate,1,4) )
					SET @SMS = Replace( @SMS,'mm', SUBSTRING(@lPDate,6,2) )
					SET @SMS = Replace( @SMS,'dd', SUBSTRING(@lPDate,9,2) )
					SET @SMS = Replace( @SMS,'hh', Cast(DATEPART(hh,@lDT) AS varchar) )
					SET @SMS = Replace( @SMS,'nn', Cast(DATEPART(mi,@lDT) AS varchar) )
					
					SET @SMS = Replace( @SMS,'MaxPeakAsynchDate', @MaxPeakAsynchDate )
					SET @SMS = Replace( @SMS,'MaxPeakAsynch', @MaxPeakAsynch )
					SET @SMS = Replace( @SMS,'PercentAsynch', @PercentAsynch )
					SET @SMS = Replace( @SMS,'LastPeakAsynch', @LastPeakAsynch )
					SET @SMS = Replace( @SMS,'PeakAsynch', @PeakAsynch )
					SET @SMS = Replace( @SMS,'LastMaxPeak', @LastMaxPeak )
					SET @SMS = Replace( @SMS,'MaxPeakDayNight', @MaxPeakDayNight )
					SET @SMS = Replace( @SMS,'PeakDayNight', @PeakDayNight )
					SET @SMS = Replace( @SMS,'LastPeakDate', @LastPeakDate )
					SET @SMS = Replace( @SMS,'LastPeak', @LastPeak )
					SET @SMS = Replace( @SMS,'MaxPeakDate', @MaxPeakDate )
					SET @SMS = Replace( @SMS,'MaxPeak', @MaxPeak )
					SET @SMS = Replace( @SMS,'TopPeak', @TopPeak )
					SET @SMS = Replace( @SMS,'ForcastPeak', @ForcastPeak )
					SET @SMS = Replace( @SMS,'LoadManagement', @LoadManagement )
					SET @SMS = Replace( @SMS,'PeakValue', @PeakValue )
					SET @SMS = Replace( @SMS,'PeakDate', @PeakDate )
					SET @SMS = Replace( @SMS,'LastDate', @LastDate )
					SET @SMS = Replace( @SMS,'MaxPercent', @MaxPercent )
					SET @SMS = Replace( @SMS,'Percent', @Percent )
					SET @SMS = Replace( @SMS,'Area', @Area )
					SET @SMS = Replace( @SMS,'CRLF', nchar(13) )
					
					SET @lDT = DATEADD(dd,1,@lThisTime)
					SET @lPDate = 
						dbo.mtosh( @lDT ) + ' ' +
						Cast(DATEPART(hh,@lDT) AS varchar) + ':' +
						Cast(DATEPART(mi,@lDT) AS varchar)
					SET @RequestType = 'SMSDailyPeak NextDT:' + @lPDate
					
					DECLARE @Desc AS nvarchar(100)
					SET @Desc = 'Havades-ID=' + CAST(@ManagerSMSSTId AS nvarchar)
					EXEC spSendSMS @SMS, @ManagerMobile, @Desc, @RequestType, @AreaId
					
					UPDATE TblManagerSMSST
					SET Peak_LastSendTime = GETDATE()
					WHERE ManagerSMSSTId = @ManagerSMSSTId
					
					SET @lCnt = @lCnt + 1
				END /* of IF( @lDiffNow >= 0 AND @lDiff > 10 ) */
			END /* of if @@FETCH_STATUS = 0 */
			ELSE
				SET @lIsLoop = 0
		END /* of WHILE */
	CLOSE crSMSST
	DEALLOCATE crSMSST
		
	RETURN @lCnt
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spCheckSMSSerghat]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spCheckSMSSerghat]
GO
CREATE PROCEDURE [dbo].[spCheckSMSSerghat] 
AS
	DECLARE @ManagerSMSDCId AS int
	DECLARE @ManagerMobile AS nvarchar (200)
	DECLARE @ManagerAreaId AS int
	DECLARE @ManagerSMSDCSendedId AS bigint
	DECLARE @IsSendSMSSerghat AS bit

	DECLARE @RequestId AS bigint;
	DECLARE @RequestNumber AS bigint
	DECLARE @RecordType AS nvarchar(10)
	DECLARE @DEDate AS varchar(10)
	DECLARE @DETime AS varchar(5)
	DECLARE @Server121Id AS int
	DECLARE @AreaId AS int
	DECLARE @Area AS nvarchar(200)
	DECLARE @PartName AS nvarchar(250)
	DECLARE @ValueSerghat AS real
	DECLARE @PartUnit AS nvarchar(30)
	DECLARE @ReferTo AS nvarchar(30)
	DECLARE @Address AS nvarchar(200)
	DECLARE	@PartList AS nvarchar(2000)
	DECLARE @EndJobStateId AS int

	DECLARE @SMSClause AS nvarchar(2000)
	DECLARE @SMS AS nvarchar(2000)
	DECLARE @RequestType AS nvarchar(200)
	DECLARE @ReqType as nvarchar(50)

	SET @RequestType = 'SMSSerghat'
	SET @SMSClause = NULL
	SELECT @SMSClause = ConfigText FROM Tbl_Config WHERE ConfigName = @RequestType
	IF @SMSClause IS NUlL RETURN 0
	
	CREATE Table #tSTL
	(
		RequestId bigint,
		RequestNumber bigint,
		DEDate varchar(10),
		DETime varchar(5),
		EndJobStateId int,
		RecordType nvarchar(10),
		Server121Id int,
		AreaId int,
		Area nvarchar(50),
		PartName nvarchar(250),
		ValueSerghat real,
		PartUnit nvarchar(30),
		ReferTo nvarchar(30),
		Address nvarchar(200),
	)

	CREATE Table #tSRG
	(
		RequestId bigint,
		RequestNumber bigint,
		DEDate varchar(10),
		DETime varchar(5),
		EndJobStateId int,
		RecordType nvarchar(10),
		Server121Id int,
		AreaId int,
		Area nvarchar(50),
		ReferTo nvarchar(30),
		Address nvarchar(200),
		PartList nvarchar(2000)
	)
	declare @SDate as datetime=dateadd(DAY,-1,getdate())
	declare @EDate as datetime=getdate()
	

	INSERT INTO #tSTL
	SELECT * FROM 
	(
		SELECT 
			TblRequest.RequestId,
			TblRequest.RequestNumber, 
			TblRequest.DisconnectDatePersian AS DEDate,
			TblRequest.DisconnectTime AS DETime,
			TblRequest.EndJobStateId,
			RecordType = CASE
				WHEN NOT TblRequest.MPRequestId IS NULL AND NOT TblRequest.LPRequestId IS NULL THEN 'MPLP'
				WHEN NOT TblRequest.LPRequestId IS NULL AND TblRequest.IsLightRequest = 1 THEN 'Light'
				WHEN NOT TblRequest.LPRequestId IS NULL AND TblRequest.IsLightRequest = 0 THEN 'LP'
				WHEN NOT TblRequest.MPRequestId IS NULL THEN 'MP'
			END,
			Tbl_Area.Server121Id, 
			TblRequest.AreaId, 
			Tbl_Area.Area, 
			ISNULL(ISNULL(Tbl_LightPart.LightPart,Tbl_LPPart.LPPart),Tbl_MPPart.MPPart) AS PartName, 
			ISNULL(ISNULL(TblLightRequestPart.LightPartValueSerghat,TblLPRequestPart.LPPartValueSerghat),TblMPRequestPart.MPPartValueSerghat) AS ValueSerghat, 
			ISNULL(ISNULL(ISNULL(Tbl_PartUnit_Light.PartUnit,Tbl_PartUnit_LP.PartUnit),Tbl_PartUnit_MP.PartUnit),'') AS PartUnit, 
			Tbl_ReferTo.ReferTo, 
			TblRequest.Address
		FROM 
			TblRequest
			LEFT JOIN TblMPRequest ON TblRequest.MPRequestId = TblMPRequest.MPRequestId
			LEFT JOIN TblLPRequest ON TblRequest.LPRequestId = TblLPRequest.LPRequestId
			LEFT JOIN TblMPRequestPart ON TblMPRequest.MPRequestId = TblMPRequestPart.MPRequestId
			LEFT JOIN TblLPRequestPart ON TblLPRequest.LPRequestId = TblLPRequestPart.LPRequestId
			LEFT JOIN TblLightRequestPart ON TblLPRequest.LPRequestId = TblLightRequestPart.LPRequestId
			LEFT JOIN Tbl_MPPart ON TblMPRequestPart.MPPartId = Tbl_MPPart.MPPartId
			LEFT JOIN Tbl_LPPart ON TblLPRequestPart.LPPartId = Tbl_LPPart.LPPartId
			LEFT JOIN Tbl_LightPart ON TblLightRequestPart.LightPartId = Tbl_LightPart.LightPartId
			LEFT JOIN Tbl_PartUnit Tbl_PartUnit_MP ON Tbl_MPPart.PartUnitId = Tbl_PartUnit_MP.PartUnitId
			LEFT JOIN Tbl_PartUnit Tbl_PartUnit_LP ON Tbl_LPPart.PartUnitId = Tbl_PartUnit_LP.PartUnitId
			LEFT JOIN Tbl_PartUnit Tbl_PartUnit_Light ON Tbl_LightPart.PartUnitId = Tbl_PartUnit_Light.PartUnitId
			LEFT JOIN Tbl_ReferTo ON TblRequest.ReferToId = Tbl_ReferTo.ReferToId
			LEFT JOIN Tbl_Area ON TblRequest.AreaId = Tbl_Area.AreaId
		WHERE 
			(TblRequest.DataEntryDT >=@SDate and TblRequest.DataEntryDT<=@EDate)
			--(DATEDIFF([Hour], TblRequest.DataEntryDT, GETDATE()) <= 24)
			AND TblRequest.EndJobStateId IN (2,3,4,5) 
			AND NOT (ISNULL(TblRequest.LPRequestId,TblRequest.MPRequestId) IS NULL)
			AND ISNULL(ISNULL(ISNULL(TblLightRequestPart.LightPartValueSerghat,TblLPRequestPart.LPPartValueSerghat),TblMPRequestPart.MPPartValueSerghat),0) > 0

		UNION
			
		SELECT 
			TblRequest.RequestId,
			TblRequest.RequestNumber, 
			TblErjaRequest.ErjaDatePersian AS DEDate,
			TblErjaRequest.ErjaTime AS DETime,
			TblRequest.EndJobStateId,
			'Erja' AS RecordType,
			Tbl_Area.Server121Id, 
			TblRequest.AreaId, 
			Tbl_Area.Area, 
			Tbl_GenericPart.PartName,
			TblUsedPart.SerghatCount AS ValueSerghat, 
			Tbl_PartUnit.PartUnit, 
			Tbl_ReferTo.ReferTo, 
			TblRequest.Address
		FROM 
			TblRequest
			INNER JOIN TblErjaRequest with(index(Idx_TblErjaRequest_ErjaDT)) ON TblRequest.RequestId = TblErjaRequest.RequestId
			INNER JOIN TblUsedPart with(index(Idx1)) ON TblErjaRequest.ErjaRequestId = TblUsedPart.ErjaRequestId
			INNER JOIN Tbl_GenericPart ON TblUsedPart.GenericPartId = Tbl_GenericPart.GenericPartId
			INNER JOIN Tbl_PartUnit ON Tbl_GenericPart.PartUnitId = Tbl_PartUnit.PartUnitId
			INNER JOIN Tbl_ReferTo ON TblErjaRequest.ReferToId = Tbl_ReferTo.ReferToId
			INNER JOIN Tbl_Area ON TblRequest.AreaId = Tbl_Area.AreaId
		WHERE 
			--(DATEDIFF([Hour], TblErjaRequest.ErjaDT, GETDATE()) <= 24)
			(TblErjaRequest.ErjaDT >=@SDate and TblErjaRequest.ErjaDT<=@EDate)
			AND ISNULL(TblUsedPart.SerghatCount,0) > 0
	) ViewSerghat
	ORDER BY
		DEDate DESC,  
		RequestId
		
	/*----------------*/
	DECLARE @lIsLoop AS bit	
	
	DECLARE @LastRequestId AS bigint
	DECLARE @PartCount AS int
	SET @LastRequestId = 0
	SET @PartList = ''
	SET @PartCount = 0
	
	DECLARE crMakeList CURSOR FOR
		SELECT * FROM #tSTL

		OPEN crMakeList
		
		SET @lIsLoop = 1
		WHILE @lIsLoop = 1
		BEGIN
			FETCH NEXT
				FROM crMakeList
				INTO 
					@RequestId, 
					@RequestNumber, 
					@DEDate, 
					@DETime, 
					@EndJobStateId, 
					@RecordType, 
					@Server121Id, 
					@AreaId, 
					@Area, 
					@PartName,
					@ValueSerghat,
					@PartUnit,
					@ReferTo,
					@Address

			if @@FETCH_STATUS = 0 
			BEGIN
				IF @RequestId <> @LastRequestId
				BEGIN
					SET @LastRequestId = @RequestId
					SET @PartList = ''
					SET @PartCount = 0
				END

				SET @PartCount = @PartCount + 1
				SET @PartList = @PartList + (nchar(13) + cast(@PartCount AS nvarchar) + '- ' + @PartName + ': ' + Cast(@ValueSerghat as varchar) + ' ' + @PartUnit)
				IF NOT EXISTS(SELECT * FROM #tSRG WHERE RequestId = @RequestId)
					INSERT INTO #tSRG
						(RequestId,RequestNumber,DEDate,DETime,EndJobStateId,RecordType,Server121Id,AreaId,Area,ReferTo,Address,PartList)
					VALUES
						(@RequestId,@RequestNumber,@DEDate,@DETime,@EndJobStateId,@RecordType,@Server121Id,@AreaId,@Area,@ReferTo,@Address,@PartList)
				ELSE
					UPDATE #tSRG SET PartList = @PartList WHERE RequestId = @RequestId
			END
			ELSE
				SET @lIsLoop = 0
		END
	CLOSE crMakeList
	DEALLOCATE crMakeList
	
	/*----------------*/

	DECLARE @lSendCount AS int
	DECLARE @lFindCount AS int

	SET @lSendCount = 0

	DECLARE crSerghatList CURSOR FOR
		SELECT 
			tSmsDC.ManagerSMSDCId,
			tSmsDC.ManagerMobile,
			tSmsDC.AreaId, 
			tSRG.*
		FROM
			(
				TblManagerSMSDC tSmsDC
				INNER JOIN Tbl_Area tArea ON tSmsDC.AreaId = tArea.AreaId
			)
			CROSS JOIN #tSRG tSRG
		WHERE
			tSmsDC.IsActive = 1
			AND tSmsDC.IsSerghat = 1
			AND (tSmsDC.AreaId = 99 OR tSmsDC.AreaId = tSRG.AreaId)

		OPEN crSerghatList
		
		SET @lIsLoop = 1
		WHILE @lIsLoop = 1
		BEGIN
			FETCH NEXT
				FROM crSerghatList
				INTO 
					@ManagerSMSDCId, 
					@ManagerMobile, 
					@ManagerAreaId, 
					@RequestId, 
					@RequestNumber, 
					@DEDate, 
					@DETime, 
					@EndJobStateId, 
					@RecordType, 
					@Server121Id, 
					@AreaId, 
					@Area, 
					@ReferTo,
					@Address,
					@PartList

			if @@FETCH_STATUS = 0 
			BEGIN
				SET @SMS = @SMSClause
				SET @IsSendSMSSerghat  = 0
				SET @ManagerSMSDCSendedId = NULL
				
				SELECT 
					@ManagerSMSDCSendedId = ManagerSMSDCSendedId,
					@IsSendSMSSerghat = ISNULL(IsSendSMSSerghat,0)
				FROM 
					TblManagerSMSDCSended tSend
				WHERE
					ManagerSMSDCId = @ManagerSMSDCId
					AND RequestId = @RequestId
					
				IF(@IsSendSMSSerghat = 0)
				BEGIN
					IF @RecordType = 'MPLP'
						SET @ReqType = N'›‘«— „ Ê”ÿ Ê ›‘«— ÷⁄Ì›'
					ELSE IF @RecordType = 'MP'
						SET @ReqType = N'›‘«— „ Ê”ÿ'
					ELSE IF @RecordType = 'LP'
						SET @ReqType = N'›‘«— ÷⁄Ì›'
					ELSE IF @RecordType = 'Light'
						SET @ReqType = N'—Ê‘‰«ÌÌ „⁄«»—'
					ELSE IF @RecordType = 'Erja'
						SET @ReqType = N'«—Ã«⁄ ‘œÂ »Â «Ì‰ Ê«Õœ'
					
					SET @SMS = Replace( @SMS,'ReqNo', ISNULL(@RequestNumber,''))
					SET @SMS = Replace( @SMS,'ReqType', ISNULL(@ReqType,''))
					SET @SMS = Replace( @SMS,'Area', ISNULL(@Area,''))
					SET @SMS = Replace( @SMS,'PartList', ISNULL(@PartList,'-') )
					SET @SMS = Replace( @SMS,'Date', ISNULL(@DEDate,''))
					SET @SMS = Replace( @SMS,'Time', ISNULL(@DETime,''))
					SET @SMS = Replace( @SMS,'ReferTo', ISNULL(@ReferTo,''))
					SET @SMS = Replace( @SMS,'Address', ISNULL(@Address,''))
					SET @SMS = Replace( @SMS,'Hello', N'»« ”·«„
' )
					SET @SMS = Replace( @SMS,'CRLF', nchar(13) )
					
					DECLARE @Desc AS nvarchar(100)
					SET @Desc = 'Havades-ID=' + CAST(@RequestId AS nvarchar)
					EXEC spSendSMS @SMS, @ManagerMobile, @Desc, @RequestType, @ManagerAreaId

					IF @ManagerSMSDCSendedId IS NULL
					BEGIN
						INSERT INTO TblManagerSMSDCSended (ManagerSMSDCId, RequestId, IsSendSMSSerghat)
						VALUES(@ManagerSMSDCId, @RequestId, 1)
						SET @ManagerSMSDCSendedId = @@IDENTITY
					END
					ELSE
					BEGIN
						UPDATE TblManagerSMSDCSended SET IsSendSMSSerghat = 1 WHERE ManagerSMSDCSendedId = @ManagerSMSDCSendedId
					END
					
					SET @lSendCount = @lSendCount + 1
				END
			END
			ELSE
				SET @lIsLoop = 0
		END
	CLOSE crSerghatList
	DEALLOCATE crSerghatList
		
	DROP TABLE #tSRG
	DROP TABLE #tSTL

	RETURN @lSendCount
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spCheckNewSTManagerSubscriber]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spCheckNewSTManagerSubscriber]
GO
CREATE PROCEDURE [dbo].[spCheckNewSTManagerSubscriber]
AS
	DECLARE @RequestType AS nvarchar(200)
	DECLARE @SMS AS nvarchar(2000)
	DECLARE @SMSClause AS nvarchar(2000)

	DECLARE @ManagerSMSSTId AS int
	DECLARE @ManagerMobile AS varchar(20)
	DECLARE @AreaId AS int
	DECLARE @IsCenter AS bit
	DECLARE @IsSetad AS bit
	DECLARE @Server121Id AS int
	DECLARE @Area AS nvarchar(50)
	DECLARE @ParentAreaId AS int
	DECLARE @SendDay AS int
	DECLARE @SendTime AS varchar(5)
	DECLARE @LastSendTime AS datetime

	DECLARE @lToday AS datetime
	DECLARE @lTodayPersian AS varchar(10)
	DECLARE @lLastMonthDate AS varchar(10)
	DECLARE @lThisTime AS datetime
	DECLARE @lDay AS int
	DECLARE @lHour AS int
	DECLARE @lMin AS int
	DECLARE @lDiff AS int, @lDiffNow AS int
	
	DECLARE @lPDate AS varchar(10)
	DECLARE @lDT AS datetime
	DECLARE @lYear AS varchar(4)
	DECLARE @lMonth AS varchar(2)
	DECLARE @lLastMonthName AS nvarchar(20)
	DECLARE @lLastMonth AS varchar(2)
	DECLARE @lLastYear AS varchar(4)
	
	SET @lToday = GETDATE()
	SET @lTodayPersian = dbo.mtosh(@lToday)
	SET @lLastMonthDate = dbo.PersainDateAdd('mm',-1,@lTodayPersian)
	
	SET @lTodayPersian = LEFT(@lTodayPersian,8) + '01'
	SET @lToday = dbo.shtom(@lTodayPersian)
		
	SET @lLastMonthName = dbo.GetPersianMonthName(SUBSTRING(@lLastMonthDate,6,2))
	SET @lLastMonth = SUBSTRING(@lLastMonthDate,6,2)
	SET @lLastYear = LEFT(@lLastMonthDate,4)
	
	SET @lToday = DATEADD( ms, - DATEPART(ms,@lToday), @lToday )
	SET @lToday = DATEADD( ss, - DATEPART(ss,@lToday), @lToday )
	SET @lToday = DATEADD( mi, - DATEPART(mi,@lToday), @lToday )
	SET @lToday = DATEADD( hh, - DATEPART(hh,@lToday), @lToday )
	
	DECLARE @lCnt AS int
	SET @lCnt = 0

	SET @RequestType = 'SMSMonthlySubscriber'
	SET @SMSClause = NULL
	SELECT @SMSClause = ConfigText FROM Tbl_Config WHERE ConfigName = @RequestType
	IF @SMSClause IS NUlL RETURN 0
	
	DECLARE @lDisconnectPowerMonth AS float
	DECLARE @lDisconnectPowerYear AS float
	DECLARE @lEnergyMonth AS float
	DECLARE @lSubscriberCountMonth AS float
	DECLARE @lEnergyYear AS float
	DECLARE @lSubscriberCountYear AS float
	DECLARE @lAreaId as int
	DECLARE @lServer121Id as int

	SET @lDisconnectPowerMonth = 0
	SET @lDisconnectPowerYear = 0
	SET @lEnergyMonth = 0
	SET @lSubscriberCountMonth = 0
	SET @lEnergyYear = 0
	SET @lSubscriberCountYear = 0

	/*-------------*/

	DECLARE @lDisconnectPowerSubMonth AS float
	DECLARE @lDonePowerSubMonth AS float
	DECLARE @lIntervalMonth AS float
	DECLARE @lIntervalYear AS float	
	DECLARE @lDisconnectPowerSubYear AS float
	DECLARE @lDonePowerSubYear AS float

	SET @lDisconnectPowerSubMonth = 0
	SET @lDonePowerSubMonth = 0
	SET @lIntervalMonth = 0
	SET @lIntervalYear = 0
	SET @lDisconnectPowerSubYear = 0
	SET @lDonePowerSubYear = 0
	
	DECLARE crSMSST CURSOR FOR
		SELECT 
			TblManagerSMSST.ManagerSMSSTId,
			TblManagerSMSST.ManagerMobile,
			TblManagerSMSST.AreaId,
			Tbl_Area.IsCenter,
			Tbl_Area.IsSetad,
			Tbl_Area.Server121Id,
			Tbl_Area.Area,
			Tbl_Area.ParentAreaId,
			TblManagerSMSST.SubscriberDC_SendDay,
			TblManagerSMSST.SubscriberDC_SendTime,  
			TblManagerSMSST.SubscriberDC_LastSendTime 
		FROM 
			TblManagerSMSST
			INNER JOIN Tbl_Area ON TblManagerSMSST.AreaId = Tbl_Area.AreaId
		WHERE 
			SubscriberDC_IsActive = 1

		OPEN crSMSST
		
		DECLARE @lIsLoop AS bit	
		SET @lIsLoop = 1
		WHILE @lIsLoop = 1
		BEGIN
			FETCH NEXT FROM crSMSST
			INTO 
				@ManagerSMSSTId, 
				@ManagerMobile, 
				@AreaId, 
				@IsCenter, 
				@IsSetad, 
				@Server121Id, 
				@Area, 
				@ParentAreaId, 
				@SendDay,
				@SendTime, 
				@LastSendTime
			
			IF @@FETCH_STATUS = 0 
			BEGIN

				SET @lMonth = SUBSTRING(@lTodayPersian,6,2)
				SET @lYear = LEFT(@lTodayPersian,4)
				
				DECLARE @IsKabise AS bit
				IF @lYear % 4 = 3
					SET @IsKabise = 1
				ELSE
					SET @IsKabise = 0
					
				IF @SendDay = 31 AND @lMonth > 6
					SET @SendDay = 30
				IF (@SendDay = 31 OR @SendDay = 30) AND @lMonth = 12 AND @IsKabise = 0
					SET @SendDay = 29
					
				SET @lTodayPersian = LEFT(@lTodayPersian,8) + cast(@SendDay as varchar(2))
				SET @lToday = dbo.shtom(@lTodayPersian)
				
				SET @SMS = @SMSClause
				SET @lHour = Cast(LEFT(@SendTime,2) AS int)
				SET @lMin = Cast(RIGHT(@SendTime,2) AS int)
				SET @lThisTime = DATEADD(hh, @lHour, @lToday)
				SET @lThisTime = DATEADD(mi, @lMin, @lThisTime)
				SET @lDiff = ISNULL(DATEDIFF(mi, @LastSendTime, @lThisTime),1440)
				SET @lDiffNow = DATEDIFF(mi, @lThisTime, GETDATE())
				
				IF( @lDiffNow >= 0 AND @lDiff > 10 )
				BEGIN
					IF @IsSetad = 1
					BEGIN
						SET @lAreaId = - 1
						SET @lServer121Id = - 1
					END
					ELSE IF @IsCenter = 1
					BEGIN
						SET @lAreaId = - 1
						SET @lServer121Id = @Server121Id
					END
					ELSE
					BEGIN
						SET @lAreaId = @AreaId
						SET @lServer121Id = @Server121Id
					END

					SELECT 
						@lDisconnectPowerMonth = ISNULL(SUM(TblRequest.DisconnectPower), 0)
					FROM 
						TblRequest
						INNER JOIN Tbl_Area ON TblRequest.AreaId = Tbl_Area.AreaId
						LEFT JOIN TblMPRequest ON TblRequest.MPRequestId = TblMPRequest.MPRequestId
					WHERE 
						TblRequest.DisconnectDatePersian LIKE cast(@lLastYear AS VARCHAR(4)) + '/' + cast(@lLastMonth AS VARCHAR(2)) + '%'
							AND (IsFogheToziRequest = 0)
							AND (
								Tbl_Area.Server121Id = @Server121Id
								OR @lServer121Id = - 1
								)
							AND (
								Tbl_Area.AreaId = @AreaId
								OR @lAreaId = - 1
								)

					SELECT 
						@lDisconnectPowerYear = ISNULL(SUM(TblRequest.DisconnectPower), 0)
					FROM 
						TblRequest
						INNER JOIN Tbl_Area ON TblRequest.AreaId = Tbl_Area.AreaId
						LEFT JOIN TblMPRequest ON TblRequest.MPRequestId = TblMPRequest.MPRequestId
					WHERE 
						TblRequest.DisconnectDatePersian >= cast(@lLastYear AS VARCHAR(4)) + '/01/01'
							AND TblRequest.DisconnectDatePersian <= cast(@lLastYear AS VARCHAR(4)) + '/' + cast(@lLastMonth AS VARCHAR(2)) + '/31'
							AND (IsFogheToziRequest = 0)
							AND (
								Tbl_Area.Server121Id = @Server121Id
								OR @lServer121Id = - 1
								)
							AND (
								Tbl_Area.AreaId = @AreaId
								OR @lAreaId = - 1
								)

					SELECT 
						@lEnergyMonth = SUM(ISNULL(TblSubscribers.Energy, 0)),
						@lSubscriberCountMonth = SUM(TblSubscribers.TotalSubscriberCount)
					FROM 
						TblSubscribers
						INNER JOIN Tbl_Area ON TblSubscribers.AreaId = Tbl_Area.AreaId
							AND IsCenter = 0
							AND IsSetad = 0
							AND (
								Tbl_Area.Server121Id = @Server121Id
								OR @lServer121Id = - 1
								)
							AND (
								Tbl_Area.AreaId = @AreaId
								OR @lAreaId = - 1
								)
					WHERE 
						LEFT(TblSubscribers.YearMonth, 4) = cast(@lLastYear AS VARCHAR(4))
						AND RIGHT(TblSubscribers.YearMonth, 3) = '/' + cast(@lLastMonth AS VARCHAR(2))

					SELECT 
						@lEnergyYear = SUM(ISNULL(TblSubscribers.Energy, 0)),
						@lSubscriberCountYear = SUM(TblSubscribers.TotalSubscriberCount)
					FROM 
						TblSubscribers
						INNER JOIN Tbl_Area ON TblSubscribers.AreaId = Tbl_Area.AreaId
							AND IsCenter = 0
							AND IsSetad = 0
							AND (
								Tbl_Area.Server121Id = @Server121Id
								OR @lServer121Id = - 1
								)
							AND (
								Tbl_Area.AreaId = @AreaId
								OR @lAreaId = - 1
								)
					WHERE 
						LEFT(TblSubscribers.YearMonth, 4) = cast(@lLastYear AS VARCHAR(4))
						AND RIGHT(TblSubscribers.YearMonth, 2) <= cast(@lLastMonth AS VARCHAR(2))

					IF @lSubscriberCountMonth > 0
					BEGIN
						SET @lDisconnectPowerSubMonth = @lDisconnectPowerMonth * 1000 / @lSubscriberCountMonth
						SET @lDonePowerSubMonth = @lEnergyMonth * 1000 / @lSubscriberCountMonth
					END

					IF @lDonePowerSubMonth > 0 
						SET @lIntervalMonth = @lDisconnectPowerSubMonth * 24 * 60 / @lDonePowerSubMonth 
					Else If @lEnergyMonth > 0 
						SET @lIntervalMonth = @lDisconnectPowerMonth * 24 * 60 / @lEnergyMonth

					IF @lSubscriberCountYear > 0
					BEGIN
						SET @lDisconnectPowerSubYear = @lDisconnectPowerYear * 1000 / @lSubscriberCountYear
						SET @lDonePowerSubYear = @lEnergyYear * 1000 / @lSubscriberCountYear
					END

					IF @lDonePowerSubYear > 0 
						SET @lIntervalYear = @lDisconnectPowerSubYear * 24 * 60 / @lDonePowerSubYear 
					Else If @lEnergyYear > 0  
						SET @lIntervalYear = @lDisconnectPowerYear * 24 * 60 / @lEnergyYear
				
					SET @lDT = GETDATE()
					SET @lPDate = dbo.mtosh(@lDT)
					
					SET @SMS = Replace( @SMS,'MonthName', @lLastMonthName )
					SET @SMS = Replace( @SMS,'lIntervalMonth', CAST(@lIntervalMonth as varchar))
					SET @SMS = Replace( @SMS,'lIntervalYear', CAST(@lIntervalYear as varchar))

					SET @RequestType = 'SMSMonthlySubscriber NextDT:' + @lPDate
					
					DECLARE @Desc AS nvarchar(100)
					SET @Desc = 'ManagerSMSSTId=' + CAST(@ManagerSMSSTId AS nvarchar)
					EXEC spSendSMS @SMS, @ManagerMobile, @Desc, @RequestType, @AreaId
					
					UPDATE TblManagerSMSST
					SET SubscriberDC_LastSendTime = GETDATE()
					WHERE ManagerSMSSTId = @ManagerSMSSTId
					
					SET @lCnt = @lCnt + 1
				END 
			END 
			ELSE
				SET @lIsLoop = 0
		END 
	CLOSE crSMSST
	DEALLOCATE crSMSST
		
	RETURN @lCnt
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spCheckSMSPanel]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[spCheckSMSPanel]
GO

CREATE PROCEDURE [dbo].[spCheckSMSPanel] 
	
AS
	CREATE TABLE #tblSendSmsToReceiverTels
	(
		SMSTextId BIGINT,
		AreaId INT,
		SMSTextBody NVARCHAR(2000),
		SMSReceiverTel VARCHAR(12)
	)

	CREATE TABLE #tblSendSmsToReceiverContacts
	(
		SMSTextId BIGINT,
		AreaId INT,
		SMSTextBody NVARCHAR(2000),
		SMSReceiverTel VARCHAR(12)
	)

	CREATE TABLE #tblSendSmsToReceiverGroups
	(
		SMSTextId BIGINT,
		AreaId INT,
		SMSTextBody NVARCHAR(2000),
		SMSReceiverTel VARCHAR(12)
	)

	DECLARE @curSmsTextId AS CURSOR

	DECLARE @lSmsId AS BIGINT
	DECLARE @lSMSTextBody AS NVARCHAR(2000)
	DECLARE @lAreaId AS INT
	DECLARE @lSMSReceiverTels AS VARCHAR(8000)
	DECLARE @lHHMM AS VARCHAR(5)
	DECLARE @Desc AS VARCHAR(50)

	SELECT @lHHMM = LEFT(CONVERT(VARCHAR(8),GETDATE(),108),5)

	SET @curSmsTextId = CURSOR FOR
	SELECT SMSTextId
	FROM TblSMSText
	WHERE SMSStateId = 2
		AND (SendDT < GETDATE() OR (SendDT = GETDATE() AND SendTime <= @lHHMM))

	OPEN @curSmsTextId
	FETCH NEXT
	FROM @curSmsTextId INTO @lSmsId
	WHILE @@FETCH_STATUS = 0
	BEGIN
		
		SELECT @lSMSTextBody = SMSBody FROM TblSMSTextBody WHERE SMSTextId = @lSmsId
		SELECT @lAreaId = AreaId FROM TblSMSText WHERE SMSTextId = @lSmsId
		SELECT @lSMSReceiverTels = TelNumbers FROM TblSMSReceiverTel WHERE SMSTextId = @lSmsId
		
		IF @lSMSReceiverTels <> ''
		BEGIN
			INSERT INTO #tblSendSmsToReceiverTels
			SELECT @lSmsId AS SMSTextId, @lAreaId AS AreaId, @lSMSTextBody AS SMSTextBody, Item AS SMSReceiverTel FROM Split(@lSMSReceiverTels,',')
		END
		
		INSERT INTO #tblSendSmsToReceiverContacts
		SELECT @lSmsId AS SMSTextId, @lAreaId AS AreaId, @lSMSTextBody AS SMSTextBody, Mobile AS SMSReceiverTel FROM TblSMSReceiverContact 
		INNER JOIN Tbl_SMSContact ON TblSMSReceiverContact.SMSContactId = Tbl_SMSContact.SMSContactId
		WHERE SMSTextId = @lSmsId
		
		INSERT INTO #tblSendSmsToReceiverGroups
		SELECT @lSmsId AS SMSTextId, @lAreaId AS AreaId, @lSMSTextBody AS SMSTextBody, Mobile AS SMSReceiverTel FROM TblSMSReceiverGroup
		INNER JOIN Tbl_SMSGroup ON TblSMSReceiverGroup.SMSGroupId = Tbl_SMSGroup.SMSGroupId
		INNER JOIN Tbl_SMSContactGroup ON Tbl_SMSGroup.SMSGroupId = Tbl_SMSContactGroup.SMSGroupId
		INNER JOIN Tbl_SMSContact ON Tbl_SMSContactGroup.SMSContactId = Tbl_SMSContact.SMSContactId
		WHERE SMSTextId = @lSmsId
		
		FETCH NEXT
		FROM @curSmsTextId INTO @lSmsId
	END

	CLOSE @curSmsTextId
	DEALLOCATE @curSmsTextId

	DECLARE @curSendSMS AS CURSOR
	DECLARE @lSendSmsId AS BIGINT
	DECLARE @lSendAreaId AS INT
	DECLARE @lSendSmsBody AS NVARCHAR(2000)
	DECLARE @lSendSMSPhone AS VARCHAR(11)

	SET @curSendSMS = CURSOR FOR
	SELECT * FROM (
	SELECT * FROM #tblSendSmsToReceiverTels
	UNION
	SELECT * FROM #tblSendSmsToReceiverContacts
	UNION
	SELECT * FROM #tblSendSmsToReceiverGroups ) AS tblAllSMS

	OPEN @curSendSMS
	FETCH NEXT
	FROM @curSendSMS INTO @lSendSmsId, @lSendAreaId, @lSendSmsBody, @lSendSMSPhone
	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @Desc = 'SmsTextId='  + CAST(@lSendSmsId AS VARCHAR)
		EXEC spSendSMS @lSendSmsBody , @lSendSMSPhone, @Desc, 'SMSPanel', @lSendAreaId
		UPDATE TblSMSText SET SMSStateId = 4 WHERE SMSTextId = @lSendSmsId AND SMSStateId <> 4
		
		FETCH NEXT
		FROM @curSendSMS INTO @lSendSmsId, @lSendAreaId, @lSendSmsBody, @lSendSMSPhone
	END

	CLOSE @curSendSMS
	DEALLOCATE @curSendSMS

	DROP TABLE #tblSendSmsToReceiverTels
	DROP TABLE #tblSendSmsToReceiverContacts
	DROP TABLE #tblSendSmsToReceiverGroups
GO

/* ------------------- ÅÌ«„ò «—”«· Â‘œ«— »Â ÅÌ„«‰ò«—«‰ »—«Ì Œ«„Ê‘ÌÂ«Ì »«»—‰«„Â «Ì òÂ œ—’œÌ «“ “„«‰ ﬁÿ⁄ ¬‰ ê–‘ Â »«‘œ -------------------- */

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spSendSMSAlarmForPeymankar]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spSendSMSAlarmForPeymankar]
GO

CREATE PROCEDURE [dbo].[spSendSMSAlarmForPeymankar] 
AS
	DECLARE @lDate as datetime = dateadd(day,-1,getdate())
	DECLARE @SMS AS nvarchar(2000)
	SET @SMS = ''

	SELECT @SMS = ConfigText FROM Tbl_Config WHERE ConfigName = 'SMSSendAlarmForPeymankar'
	IF @SMS = '' RETURN;

	DECLARE @lPercent AS INT = 75
	SELECT @lPercent = ISNULL(ConfigValue,75) FROM Tbl_Config WHERE ConfigName = 'SendAlarmForTamirPercentPeymankar'
	IF @lPercent = 0 RETURN;

	SELECT 
		TblRequest.RequestId
	INTO 
		#tmpReq
	FROM 
		TblRequest
		LEFT JOIN TblMPRequest ON TblRequest.MPRequestId = TblMPRequest.MPRequestId
		LEFT JOIN TblLPRequest ON TblRequest.LPRequestId = TblLPRequest.LPRequestId
		LEFT JOIN TblTamirRequestConfirm ON TblRequest.RequestId = TblTamirRequestConfirm.RequestId
		INNER JOIN TblTamirRequest ON TblTamirRequestConfirm.TamirRequestId = TblTamirRequest.TamirRequestId OR TblRequest.RequestId = TblTamirRequest.EmergencyRequestId
	WHERE 
		DATEDIFF(MINUTE, TblRequest.TamirDisconnectFromDT, TblRequest.TamirDisconnectToDT) > 0
		AND TblRequest.DisconnectDT >= @lDate
		AND TblRequest.IsTamir = 1
		AND ISNULL(TblMPRequest.IsWarmLine,0) = 0 AND ISNULL(TblLPRequest.IsWarmLine,0) = 0 
		AND TblRequest.EndJobStateId IN (5)
		AND DATEDIFF(MINUTE, TblRequest.DisconnectDT, GETDATE()) >= ((@lPercent * DATEDIFF(MINUTE, TblRequest.TamirDisconnectFromDT, TblRequest.TamirDisconnectToDT)) / 100)
		AND ISNULL(TblTamirRequest.PeymankarMobileNo,'') <> ''
		AND NOT TblTamirRequest.TamirRequestId IN (SELECT TamirRequestId FROM TblTamirRequestSendAlarmForPeymankar WHERE IsSendSMSAlarm = 1 )
		AND (NOT TblRequest.MPRequestId IS NULL OR NOT TblRequest.LPRequestId IS NULL OR NOT TblRequest.FogheToziDisconnectId IS NULL)

	DECLARE @lRequestId as bigint
	DECLARE TamirRequestList CURSOR FOR
		SELECT * FROM #tmpReq

		OPEN TamirRequestList
		
		DECLARE @lIsLoop AS bit	
		SET @lIsLoop = 1
		WHILE @lIsLoop = 1
		BEGIN
			FETCH NEXT
				FROM TamirRequestList
				INTO @lRequestId

			if @@FETCH_STATUS = 0 
			BEGIN
				DECLARE @lTamirRequestId as bigint = -1
				DECLARE @lMPRequestId as bigint
				DECLARE @lLPRequestId as bigint
				DECLARE @lFogheToziDisconnectId as bigint
				DECLARE @lRequestNumber AS bigint
				DECLARE @lAddress AS nVarchar(500) = ''
				DECLARE @lArea AS nVarchar(500) = ''
				DECLARE @lTamirDisconnectFromTime as varchar(5) = ''
				DECLARE @lTamirDisconnectToTime as varchar(5) = ''
				DECLARE @lDisconnectTime as varchar(5) = ''
				DECLARE @lConnectTime as varchar(5) = ''
				DECLARE @lTamirInterval as int
				DECLARE @lRequestBy as nvarchar(300) = ''
				DECLARE @lPeymankar as nvarchar(300) = ''
				DECLARE @PeymankarMobileNo as nvarchar(20) = ''
				DECLARE @lEkip as nvarchar(300) = ''
				DECLARE @lTamirNetworkType AS nvarchar(200) = ''
				DECLARE @lAreaId as int
				
				SELECT 
					@lMPRequestId = ISNULL(MPRequestId,-1),
					@lLPRequestId = ISNULL(LPRequestId,-1),
					@lFogheToziDisconnectId = ISNULL(FogheToziDisconnectId,-1),
					@lRequestNumber = RequestNumber,
					@lAddress = Address,
					@lArea = Tbl_Area.Area,
					@lTamirDisconnectFromTime = TamirDisconnectFromTime,
					@lTamirDisconnectToTime = TamirDisconnectToTime,
					@lDisconnectTime = DisconnectTime,
					@lConnectTime = dbo.GetTime(DATEADD(minute,DATEDIFF(MINUTE, TblRequest.TamirDisconnectFromDT, TblRequest.TamirDisconnectToDT),DisconnectDT)),
					@lAreaId = TblRequest.AreaId
				FROM
					TblRequest
					INNER JOIN Tbl_Area ON TblRequest.AreaId = Tbl_Area.AreaId
				WHERE
					RequestId = @lRequestId
				
				IF @lMPRequestId > -1
					SET @lTamirNetworkType = N'›‘«— „ Ê”ÿ'
				ELSE IF @lLPRequestId > -1
					SET @lTamirNetworkType = N'›‘«— ÷⁄Ì›'
				ELSE IF @lFogheToziDisconnectId > -1
					SET @lTamirNetworkType = N'›Êﬁ  Ê“Ì⁄'
					
				SELECT 
					@lTamirRequestId = TblTamirRequest.TamirRequestId,
					@lRequestBy = CASE WHEN TblTamirRequest.IsRequestByPeymankar = 1 THEN N'ÅÌ„«‰ò«—' WHEN TblTamirRequest.TamirNetworkTypeId = 1 THEN N'›Êﬁ  Ê“Ì⁄' ELSE N'‘—ò   Ê“Ì⁄' END, 
					@lPeymankar = ISNULL(Tbl_Peymankar.PeymankarName, TblTamirRequest.Peymankar),
					@PeymankarMobileNo = TblTamirRequest.PeymankarMobileNo
				FROM 
					TblTamirRequest
					LEFT JOIN TblTamirRequestConfirm ON TblTamirRequest.TamirRequestId = TblTamirRequestConfirm.TamirRequestId
					INNER JOIN TblRequest ON ISNULL(TblTamirRequestConfirm.RequestId, TblTamirRequest.EmergencyRequestId) = TblRequest.RequestId
					LEFT JOIN Tbl_Peymankar ON TblTamirRequest.PeymankarId = Tbl_Peymankar.PeymankarId
				WHERE
					TblRequest.RequestId = @lRequestId
				
				IF ISNULL(@lPeymankar,'') = ''
				BEGIN
					SELECT TOP 1
						@lPeymankar = ISNULL(TblEkipProfile.EkipProfileName,ISNULL(Tbl_Master.Name,''))
					FROM
						TblBazdid
						LEFT JOIN TblEkipProfile ON TblBazdid.EkipProfileId = TblEkipProfile.EkipProfileId
						LEFT JOIN Tbl_Master ON TblBazdid.MasterId = Tbl_Master.MasterId
					WHERE
						RequestId = @lRequestId
					ORDER BY 
						EzamDT DESC
				END
				
				IF ISNULL(@lRequestBy,'') = ''
					SET @lRequestBy = N'‘—ò   Ê“Ì⁄'
					
				DECLARE @SMSBody AS nvarchar(2000) = @SMS
				
				SET @SMSBody = Replace( @SMSBody,'RequestNumber', ISNULL(@lRequestNumber,0))
				SET @SMSBody = Replace( @SMSBody,'Address', ISNULL(@lAddress,'ø'))
				SET @SMSBody = Replace( @SMSBody,'Area', ISNULL(@lArea,'ø'))
				SET @SMSBody = Replace( @SMSBody,'DisconnectTime', ISNULL(@lDisconnectTime,'ø'))
				SET @SMSBody = Replace( @SMSBody,'ConnectTime', ISNULL(@lConnectTime,'ø'))
				SET @SMSBody = Replace( @SMSBody,'TamirNetworkType', ISNULL(@lTamirNetworkType,'ø'))
				SET @SMSBody = Replace( @SMSBody,'EkipName', ISNULL(@lPeymankar,'ø'))
				SET @SMSBody = Replace( @SMSBody,'RequestBy', ISNULL(@lRequestBy,'ø'))
				
				DECLARE @Desc AS nvarchar(100)
				SET @Desc = 'ReqNo=' + Cast(@lRequestNumber AS nvarchar)
				EXEC spSendSMS @SMSBody , @PeymankarMobileNo, @Desc, 'SendAlarmPeymankar', @lAreaId
				
				IF EXISTS (SELECT * FROM TblTamirRequestSendAlarmForPeymankar WHERE TamirRequestId = @lTamirRequestId)
					UPDATE TblTamirRequestSendAlarmForPeymankar SET IsSendSMSAlarm = 1 WHERE TamirRequestId = @lTamirRequestId
				ELSE
					INSERT INTO TblTamirRequestSendAlarmForPeymankar (TamirRequestId,PeymankarMobileNo,IsSendSMSAlarm) VALUES(@lTamirRequestId,@PeymankarMobileNo,1)
											
			END
			ELSE
				SET @lIsLoop = 0
		END
	CLOSE TamirRequestList
	DEALLOCATE TamirRequestList
		
	DROP TABLE #tmpReq
GO


IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spSendCriticalFeederSMS]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[spSendCriticalFeederSMS]
GO

CREATE PROCEDURE [dbo].[spSendCriticalFeederSMS] 
	@aManagerSMSDCId AS INT,
	@aAreaId AS INT,
	@aServer121Id AS INT,
	@aIsCenter AS BIT,
	@aMPFDCCountOfnTime AS INT,
	@aManagerMobile AS VARCHAR(20)
AS
	DECLARE @lSMSTitle AS NVARCHAR(100)
	DECLARE @lSMSBody AS NVARCHAR(4000)
	DECLARE @lYearMonth AS CHAR(7)
	DECLARE @lFromDate AS CHAR(10)
	DECLARE @lToDate AS CHAR(10)
	DECLARE @Desc as nvarchar(50)
	
	SET @lYearMonth = LEFT(dbo.mtosh(DATEADD(day,-1, GETDATE())), 7)
	SET @lFromDate = @lYearMonth + '/01'
	SET @lToDate = @lYearMonth + '/31'
	
	SELECT @lSMSTitle = ConfigText FROM Tbl_Config WHERE ConfigName = 'SMSCriticalMPFeedersTitle'
	SELECT @lSMSBody = ConfigText FROM Tbl_Config WHERE ConfigName = 'SMSCriticalMPFeeders'
	
	DECLARE @lMonth AS NVARCHAR(10)
	SET @lMonth = CASE Cast(RIGHT(@lYearMonth,2) AS int)
		WHEN 1 THEN N'›—Ê—œÌ‰'
		WHEN 2 THEN N'«—œÌ»Â‘ '
		WHEN 3 THEN N'Œ—œ«œ'
		WHEN 4 THEN N' Ì—'
		WHEN 5 THEN N'„—œ«œ'
		WHEN 6 THEN N'‘Â—ÌÊ—'
		WHEN 7 THEN N'„Â—'
		WHEN 8 THEN N'¬»«‰'
		WHEN 9 THEN N'¬–—'
		WHEN 10 THEN N'œÌ'
		WHEN 11 THEN N'»Â„‰'
		WHEN 12 THEN N'«”›‰œ'
		ELSE N'ø'
	END
	SET @lSMSTitle = REPLACE(@lSMSTitle, 'Month', @lMonth)
	SET @lSMSTitle = REPLACE(@lSMSTitle, 'Year', LEFT(@lYearMonth,4))
	
	CREATE TABLE #tblCriticalFeeders
	(
		AreaId INT,
		Area NVARCHAR(50),
		MPFeederId INT,
		MPFeederName NVARCHAR(50),
		DCCount INT
	)
	
	INSERT INTO #tblCriticalFeeders
	SELECT TblMPRequest.AreaId
		,Area
		,TblMPRequest.MPFeederId
		,MPFeederName
		,COUNT(TblMPRequest.MPRequestId) AS DCCount
	FROM TblMPRequest
	INNER JOIN TblRequest ON TblRequest.MPRequestId = TblMPRequest.MPRequestId
	INNER JOIN Tbl_Area ON TblMPRequest.AreaId = Tbl_Area.AreaId
	INNER JOIN Tbl_MPFeeder ON Tbl_MPFeeder.MPFeederId = TblMPRequest.MPFeederId
	WHERE TblRequest.DisconnectDatePersian >= @lFromDate
		AND TblRequest.DisconnectDatePersian <= @lToDate
		AND TblRequest.IsTamir = 0
		AND IsDisconnectMPFeeder = 1
		AND (TblMPRequest.AreaId = @aAreaId OR (@aIsCenter = 1 AND Server121Id = @aServer121Id) OR @aAreaId = 99)
	GROUP BY TblMPRequest.AreaId
		,Area
		,TblMPRequest.MPFeederId
		,MPFeederName
	HAVING COUNT(TblMPRequest.MPRequestId) >= @aMPFDCCountOfnTime

	DECLARE @lAreaId INT
	DECLARE @lArea NVARCHAR(50)
	DECLARE @lCriticalFeedersCount INT
	DECLARE @lFeedersName NVARCHAR(4000)
	DECLARE @lSMSTempBody NVARCHAR(4000)
	DECLARE @curArea AS CURSOR
	
	SET @curArea = CURSOR FOR
	SELECT AreaId,
		Area,
		COUNT(MPFeederId) AS CriticalFeedersCount
	FROM #tblCriticalFeeders
	GROUP BY AreaId, Area
	ORDER BY AreaId

	OPEN @curArea
	FETCH NEXT
	FROM @curArea INTO @lAreaId, @lArea, @lCriticalFeedersCount
	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @lFeedersName = ''
		DECLARE @lFeederName NVARCHAR(50)
		
		DECLARE @curFeeder AS CURSOR
		SET @curFeeder = CURSOR FOR
		SELECT #tblCriticalFeeders.MPFeederName 
		FROM #tblCriticalFeeders 
		WHERE AreaId = @lAreaId
		
		OPEN @curFeeder
		FETCH NEXT
		FROM @curFeeder INTO @lFeederName
		WHILE @@FETCH_STATUS = 0
		BEGIN
			SET @lFeedersName = @lFeedersName + ', ' + @lFeederName
			
			FETCH NEXT
			FROM @curFeeder INTO @lFeederName
		END
		
		SET @lSMSTempBody = @lSMSBody
		SET @lSMSTempBody = REPLACE(@lSMSTempBody, 'Area', @lArea)
		SET @lSMSTempBody = REPLACE(@lSMSTempBody, 'nCount', ISNULL(@lCriticalFeedersCount,0))
		SET @lSMSTempBody = REPLACE(@lSMSTempBody, 'MPFeederList', SUBSTRING(@lFeedersName, 2, LEN(@lFeedersName)))
		SET @lSMSTempBody = @lSMSTitle + @lSMSTempBody
		
		SET @Desc = 'SMSCriticalMPFeeders SMS ManagerDCId=' + CAST (@aManagerSMSDCId AS VARCHAR(100))
		EXEC spSendSMS @lSMSTempBody, @aManagerMobile, @Desc, '', @aAreaId
		
		IF EXISTS (SELECT * FROM TblManagerSMSCriticalMPF WHERE ManagerSMSDCId = @aManagerSMSDCId AND SMSType = 1)
			UPDATE TblManagerSMSCriticalMPF SET YearMonth = @lYearMonth WHERE ManagerSMSDCId = @aManagerSMSDCId AND SMSType = 1
		ELSE
			INSERT INTO TblManagerSMSCriticalMPF(ManagerSMSDCId, YearMonth, SMSType) VALUES(@aManagerSMSDCId, @lYearMonth, 1)
		
		FETCH NEXT
		FROM @curArea INTO @lAreaId, @lArea, @lCriticalFeedersCount
	END

	CLOSE @curArea
	DEALLOCATE @curArea

	DROP TABLE #tblCriticalFeeders
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spCheckCriticalFeederSMS]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[spCheckCriticalFeederSMS]
GO

CREATE PROCEDURE [dbo].[spCheckCriticalFeederSMS] 
	
AS
	DECLARE @lCurrDate AS CHAR(10)
	DECLARE @lCurrMonth AS INT
	DECLARE @lCurrDay AS INT
	DECLARE @lYearMonth AS VARCHAR(7)
	DECLARE @lManagerMobile AS VARCHAR(20)
	DECLARE @lMPFDCCountOfnTime AS INT
	DECLARE @lAreaId AS INT
	DECLARE @lServer121Id AS INT
	DECLARE @lIsCenter AS BIT
	DECLARE @lManagerSMSDCId AS INT
	DECLARE @lSendedCount AS INT
	
	SET @lCurrDate = dbo.mtosh(GETDATE())
	SET @lCurrMonth = CAST(SUBSTRING(@lCurrDate, 6, 2) AS INT)
	SET @lCurrDay = CAST(SUBSTRING(@lCurrDate, 9, 2) AS INT)

	IF @lCurrDay <> 1 RETURN

	SET @lCurrDate = dbo.mtosh(DATEADD(day,-1, GETDATE()))
	SET @lCurrMonth = CAST(SUBSTRING(@lCurrDate, 6, 2) AS INT)
	SET @lCurrDay = CAST(SUBSTRING(@lCurrDate, 9, 2) AS INT)

	SET @lYearMonth = LEFT(@lCurrDate, 4) + '/' 
	IF @lCurrMonth < 10 
		SET @lYearMonth = @lYearMonth + '0' + CAST(@lCurrMonth AS VARCHAR(1))
	ELSE
		SET @lYearMonth = @lYearMonth + CAST(@lCurrMonth AS VARCHAR(2))

	DECLARE @curManagerSMSDC AS CURSOR
	SET @curManagerSMSDC = CURSOR FOR
	SELECT ManagerSMSDCId,
		TblManagerSMSDC.AreaId,
		Server121Id,
		IsCenter,
		ManagerMobile,
		MPFDC_CountOfnTime
	FROM TblManagerSMSDC
	INNER JOIN Tbl_Area ON TblManagerSMSDC.AreaId = Tbl_Area.AreaId
	WHERE IsActive = 1
		AND IsCriticalFeeders = 1

	OPEN @curManagerSMSDC
	FETCH NEXT
	FROM @curManagerSMSDC INTO @lManagerSMSDCId, @lAreaId, @lServer121Id, @lIsCenter, @lManagerMobile, @lMPFDCCountOfnTime
	WHILE @@FETCH_STATUS = 0
	BEGIN
		SELECT @lSendedCount = COUNT(ManagerSMSDCId) FROM TblManagerSMSCriticalMPF WHERE ManagerSMSDCId = @lManagerSMSDCId AND YearMonth >= @lYearMonth AND SMSType = 1
		
		IF @lSendedCount = 0
			EXEC spSendCriticalFeederSMS @lManagerSMSDCId, @lAreaId, @lServer121Id, @lIsCenter, @lMPFDCCountOfnTime, @lManagerMobile
		
		FETCH NEXT
		FROM @curManagerSMSDC INTO @lManagerSMSDCId, @lAreaId, @lServer121Id, @lIsCenter, @lManagerMobile, @lMPFDCCountOfnTime
	END

	CLOSE @curManagerSMSDC
	DEALLOCATE @curManagerSMSDC
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spSendSMSMPFeederDCPower]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[spSendSMSMPFeederDCPower]
GO
CREATE PROCEDURE [dbo].[spSendSMSMPFeederDCPower] 
	@aManagerSMSDCId AS INT,
	@aAreaId AS INT,
	@aServer121Id AS INT,
	@aIsCenter AS BIT,
	@aMPFeederHighPowerCount AS INT,
	@aManagerMobile AS VARCHAR(20)
AS
	DECLARE @lSMSTitle AS NVARCHAR(100)
	DECLARE @lSMSBody AS NVARCHAR(4000)
	DECLARE @lYearMonth AS CHAR(7)
	DECLARE @lFromDate AS CHAR(10)
	DECLARE @lToDate AS CHAR(10)
	DECLARE @Desc as nvarchar(50)
		
	SET @lYearMonth = LEFT(dbo.mtosh(GETDATE()), 7)
	SET @lFromDate = @lYearMonth + '/01'
	SET @lToDate = @lYearMonth + '/31'
	
	SELECT @lSMSTitle = ConfigText FROM Tbl_Config WHERE ConfigName = 'SMSHighPowerMPFeedersTitle'
	SELECT @lSMSBody = ConfigText FROM Tbl_Config WHERE ConfigName = 'SMSHighPowerMPFeeders'
	
	DECLARE @lMonth AS NVARCHAR(10)
	SET @lMonth = CASE Cast(RIGHT(@lYearMonth,2) AS int)
		WHEN 1 THEN N'›—Ê—œÌ‰'
		WHEN 2 THEN N'«—œÌ»Â‘ '
		WHEN 3 THEN N'Œ—œ«œ'
		WHEN 4 THEN N' Ì—'
		WHEN 5 THEN N'„—œ«œ'
		WHEN 6 THEN N'‘Â—ÌÊ—'
		WHEN 7 THEN N'„Â—'
		WHEN 8 THEN N'¬»«‰'
		WHEN 9 THEN N'¬–—'
		WHEN 10 THEN N'œÌ'
		WHEN 11 THEN N'»Â„‰'
		WHEN 12 THEN N'«”›‰œ'
		ELSE N'ø'
	END
	SET @lSMSTitle = REPLACE(@lSMSTitle, 'MonthName', @lMonth)
	SET @lSMSTitle = REPLACE(@lSMSTitle, 'Year', LEFT(@lYearMonth,4))
	
	CREATE TABLE #tblHighPowerFeeders
	(
		AreaId INT,
		Area NVARCHAR(50),
		MPFeederId INT,
		MPFeederName NVARCHAR(50),
		SumPower FLOAT,
		DCCount INT
	)
	
	declare @lSQl as varchar(1000) 
	set @lSQl = 
	'SELECT top ' + cast(@aMPFeederHighPowerCount as varchar(10)) + '
		TblMPRequest.AreaId,
		Tbl_Area.Area,
		Tbl_MPFeeder.MPFeederId,
		Tbl_MPFeeder.MPFeederName,
		SUM(TblRequest.DisconnectPower) SumPower,
		COUNT(TblRequest.RequestId) DCCount
	FROM 
		TblMPRequest
		INNER JOIN TblRequest ON TblMPRequest.MPRequestId = TblRequest.MPRequestId
		INNER JOIN Tbl_MPFeeder ON TblMPRequest.MPFeederId = Tbl_MPFeeder.MPFeederId
		INNER JOIN Tbl_Area ON TblMPRequest.AreaId = Tbl_Area.AreaId
	WHERE 
		TblRequest.DisconnectDatePersian >= ''' + @lFromDate + '''
		AND TblRequest.DisconnectDatePersian <= ''' + @lToDate + '''
		AND NOT TblMPRequest.IsTotalLPPostDisconnected = 1
		AND ISNULL(TblMPRequest.IsWarmLine,0) = 0
		AND TblMPRequest.EndJobStateId IN (2,3)
		AND (TblMPRequest.AreaId = ' + CAST(@aAreaId as varchar) + ' OR (' + CAST(@aIsCenter as varchar) + ' = 1 AND Server121Id = ' + CAST(@aServer121Id as varchar) + ') OR ' + CAST(@aAreaId as varchar) + ' = 99)
	GROUP BY 
		TblMPRequest.AreaId,
		Tbl_Area.Area,
		Tbl_MPFeeder.MPFeederId,
		Tbl_MPFeeder.MPFeederName
	HAVING
		SUM(TblRequest.DisconnectPower) > 0
	ORDER BY 
		SUM(TblRequest.DisconnectPower) DESC
	'
	INSERT INTO #tblHighPowerFeeders
	EXEC (@lSQl)
	
	DECLARE @lArea NVARCHAR(50)
	DECLARE @lFeederName NVARCHAR(50)
	DECLARE @lHighPowerFeeder FLOAT
	DECLARE @lCountFeeder INT
	DECLARE @lSMSTempBody NVARCHAR(4000)
	DECLARE @lSMSSumTempBody NVARCHAR(4000)
	SET @lSMSSumTempBody = ''
	SET @lSMSTempBody = ''
	
	DECLARE curFeeder CURSOR FOR
	SELECT 
		Area,
		MPFeederName,
		SumPower,
		DCCount
	FROM #tblHighPowerFeeders 
	
	OPEN curFeeder
	DECLARE @lIsLoop AS bit	
	SET @lIsLoop = 1
	WHILE @lIsLoop = 1
	BEGIN
		FETCH NEXT
			FROM curFeeder INTO @lArea, @lFeederName, @lHighPowerFeeder, @lCountFeeder
		if @@FETCH_STATUS = 0 
		BEGIN
			SET @lSMSTempBody = @lSMSBody
			SET @lSMSTempBody = REPLACE(@lSMSTempBody, 'Area', @lArea)
			SET @lSMSTempBody = REPLACE(@lSMSTempBody, 'MPFeederName', ISNULL(@lFeederName,'ø'))
			SET @lSMSTempBody = REPLACE(@lSMSTempBody, 'SumPower', ISNULL(@lHighPowerFeeder,0))
			SET @lSMSTempBody = REPLACE(@lSMSTempBody, 'DCCount', ISNULL(@lCountFeeder,0))
			SET @lSMSSumTempBody = @lSMSSumTempBody + '° ' + @lSMSTempBody
		END
		ELSE
			SET @lIsLoop = 0
	END
	CLOSE curFeeder
	DEALLOCATE curFeeder
	
	SET @lSMSTempBody = SUBSTRING(@lSMSSumTempBody, 2, LEN(@lSMSSumTempBody)) 
	SET @lSMSTempBody = @lSMSTitle + '
' +	@lSMSTempBody
	
	SET @Desc = 'SMSHighPowerMPFeeders SMS ManagerDCId=' + CAST (@aManagerSMSDCId AS VARCHAR(100))

	IF EXISTS (SELECT * FROM #tblHighPowerFeeders) AND @lSMSTempBody <> ''
		EXEC spSendSMS @lSMSTempBody, @aManagerMobile, @Desc, '', @aAreaId
		
	IF EXISTS (SELECT * FROM TblManagerSMSCriticalMPF WHERE ManagerSMSDCId = @aManagerSMSDCId AND SMSType = 2)
		UPDATE TblManagerSMSCriticalMPF SET YearMonth = @lYearMonth WHERE ManagerSMSDCId = @aManagerSMSDCId AND SMSType = 2
	ELSE
		INSERT INTO TblManagerSMSCriticalMPF(ManagerSMSDCId, YearMonth, SMSType) VALUES(@aManagerSMSDCId, @lYearMonth, 2)
	

	DROP TABLE #tblHighPowerFeeders
GO


IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spCheckSMSMPFeederDCPower]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[spCheckSMSMPFeederDCPower]
GO

CREATE PROCEDURE [dbo].[spCheckSMSMPFeederDCPower] 
	
AS
	DECLARE @lCurrDate AS CHAR(10)
	DECLARE @lCurrMonth AS INT
	DECLARE @lCurrDay AS INT
	DECLARE @lYearMonth AS VARCHAR(7)
	DECLARE @lManagerMobile AS VARCHAR(20)
	DECLARE @lMPFeederHighPowerCount AS INT
	DECLARE @lAreaId AS INT
	DECLARE @lServer121Id AS INT
	DECLARE @lIsCenter AS BIT
	DECLARE @lManagerSMSDCId AS INT
	DECLARE @lSendedCount AS INT
	
	SET @lCurrDate = dbo.mtosh(GETDATE())
	SET @lCurrMonth = CAST(SUBSTRING(@lCurrDate, 6, 2) AS INT)
	SET @lCurrDay = CAST(SUBSTRING(@lCurrDate, 9, 2) AS INT)

	IF @lCurrMonth <= 6 AND @lCurrDay < 31 RETURN
	ELSE IF @lCurrMonth > 6 AND @lCurrMonth <= 11 AND @lCurrDay < 30 RETURN
	ELSE IF @lCurrMonth = 12 AND @lCurrDay < 29 RETURN

	SET @lYearMonth = LEFT(@lCurrDate, 4) + '/' 
	IF @lCurrMonth < 10 
		SET @lYearMonth = @lYearMonth + '0' + CAST(@lCurrMonth AS VARCHAR(1))
	ELSE
		SET @lYearMonth = @lYearMonth + CAST(@lCurrMonth AS VARCHAR(2))

	DECLARE @curManagerSMSDC AS CURSOR
	SET @curManagerSMSDC = CURSOR FOR
	SELECT ManagerSMSDCId,
		TblManagerSMSDC.AreaId,
		Server121Id,
		IsCenter,
		ManagerMobile,
		MPFeederHighPowerCount
	FROM TblManagerSMSDC
	INNER JOIN Tbl_Area ON TblManagerSMSDC.AreaId = Tbl_Area.AreaId
	WHERE IsActive = 1
		AND IsMPFeederHighPower = 1

	OPEN @curManagerSMSDC
	FETCH NEXT
	FROM @curManagerSMSDC INTO @lManagerSMSDCId, @lAreaId, @lServer121Id, @lIsCenter, @lManagerMobile, @lMPFeederHighPowerCount
	WHILE @@FETCH_STATUS = 0
	BEGIN
		SELECT @lSendedCount = COUNT(ManagerSMSDCId) FROM TblManagerSMSCriticalMPF WHERE ManagerSMSDCId = @lManagerSMSDCId AND YearMonth >= @lYearMonth AND SMSType = 2
		
		IF @lSendedCount = 0
			EXEC spSendSMSMPFeederDCPower @lManagerSMSDCId, @lAreaId, @lServer121Id, @lIsCenter, @lMPFeederHighPowerCount, @lManagerMobile
		
		FETCH NEXT
		FROM @curManagerSMSDC INTO @lManagerSMSDCId, @lAreaId, @lServer121Id, @lIsCenter, @lManagerMobile, @lMPFeederHighPowerCount
	END

	CLOSE @curManagerSMSDC
	DEALLOCATE @curManagerSMSDC
GO



IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spSendSMSDCManagerAfterMultistep]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spSendSMSDCManagerAfterMultistep]
GO
CREATE PROCEDURE [dbo].[spSendSMSDCManagerAfterMultistep]
AS
	DECLARE @RequestId AS BIGINT;
	DECLARE @ManagerMobile AS nvarchar (200);
	DECLARE @ManagerSMSDCId AS int
	DECLARE @ManagerAreaId AS int
	DECLARE @SMSMP AS nvarchar(2000)
	DECLARE @SMSLP AS nvarchar(2000)
	DECLARE @SMSFT AS nvarchar(2000)
	DECLARE @SMS AS nvarchar(2000)
	DECLARE @ConnectDatePersian AS nvarchar(10)
	DECLARE @ConnectTime AS nvarchar(5)
	DECLARE @Area AS nvarchar(200)
	DECLARE @Comments AS nvarchar(300)
	DECLARE @DisconnectDatePersian AS nvarchar(10)
	DECLARE @DisconnectTime AS nvarchar(5)
	DECLARE @RequestNumber AS varchar(100)
	
	DECLARE @MultistepConnectionId AS int
	DECLARE @ConnectType AS nvarchar(100)
	DECLARE @CurrentValue AS varchar(10)
	DECLARE @ConnectDarsad as varchar(10)
	
	DECLARE @MPPostName AS nvarchar(50) = ''
	DECLARE @MPFeederName AS nvarchar(50) = ''
	DECLARE @LPPostName AS nvarchar(50) = ''
	DECLARE @LPFeederName AS nvarchar(50) = ''
	DECLARE @DCCurrentValue AS float
		
	DECLARE @AreaId AS int 
	
	DECLARE @IsSetad AS bit 
	DECLARE @IsCenter AS bit 
	DECLARE @Server121Id AS int 
	
	DECLARE @IsMPRequest AS bit = 0
	DECLARE @IsLPRequest AS bit = 0
	DECLARE @IsFTRequest AS bit = 0
	
	SELECT @SMSMP = ConfigText FROM Tbl_Config WHERE ConfigName = 'SMSMultiStepConnectionAfterSendMP'	
	SELECT @SMSLP = ConfigText FROM Tbl_Config WHERE ConfigName = 'SMSMultiStepConnectionAfterSendLP'
	SELECT @SMSFT = ConfigText FROM Tbl_Config WHERE ConfigName = 'SMSMultiStepConnectionAfterSendFT'
	
	IF NOT @SMSMP IS NULL
		SET @SMS = @SMSMP
	ELSE IF NOT @SMSLP IS NULL
		SET @SMS = @SMSLP
	ELSE
		SET @SMS = @SMSFT
		
	IF @SMS IS NULL RETURN;

	DECLARE MultistepConnectionList CURSOR FOR
		SELECT RequestId, MPMultistepConnectionId, ManagerSMSDCId, ManagerMobile, AreaId, DisconnectDatePersian, DisconnectTime, RequestNumber,
				IsMPRequest, IsLPRequest, IsFTRequest
		FROM (
			SELECT 
				TblRequest.RequestId,
				TblMultistepConnection.MPMultistepConnectionId,
				TblManagerSMSDC.ManagerSMSDCId,
				TblManagerSMSDC.ManagerMobile,
				TblManagerSMSDC.AreaId,
				TblRequest.DisconnectDatePersian,
				TblRequest.DisconnectTime,
				TblRequest.RequestNumber,
				TblMultistepConnection.ConnectDT,
				CAST(1 AS bit) AS IsMPRequest,
				CAST(0 AS bit) AS IsLPRequest,
				CAST(0 AS bit) AS IsFTRequest
			FROM   
				TblManagerSMSDCSended 
				INNER JOIN TblManagerSMSDC ON TblManagerSMSDCSended.ManagerSMSDCId = TblManagerSMSDC.ManagerSMSDCId 
				INNER JOIN TblRequest ON TblManagerSMSDCSended.RequestId = TblRequest.RequestId
				INNER JOIN Tbl_Area ON TblManagerSMSDC.AreaId = Tbl_Area.AreaId
				INNER JOIN TblMultistepConnection ON TblRequest.MPRequestId = TblMultistepConnection.MPRequestId
			WHERE  
				TblManagerSMSDC.IsActive = 1
				AND IsSendSMSAfterConnect = 0
				AND EndJobStateId  = 5
				AND ISNULL(TblMultistepConnection.IsNotConnect,0) = 0
			UNION
			SELECT 
				TblRequest.RequestId,
				TblMultistepConnection.MPMultistepConnectionId,
				TblManagerSMSDC.ManagerSMSDCId,
				TblManagerSMSDC.ManagerMobile,
				TblManagerSMSDC.AreaId,
				TblRequest.DisconnectDatePersian,
				TblRequest.DisconnectTime,
				TblRequest.RequestNumber,
				TblMultistepConnection.ConnectDT,
				CAST(0 AS bit) AS IsMPRequest,
				CAST(1 AS bit) AS IsLPRequest,
				CAST(0 AS bit) AS IsFTRequest
			FROM   
				TblManagerSMSDCSended 
				INNER JOIN TblManagerSMSDC ON TblManagerSMSDCSended.ManagerSMSDCId = TblManagerSMSDC.ManagerSMSDCId 
				INNER JOIN TblRequest ON TblManagerSMSDCSended.RequestId = TblRequest.RequestId
				INNER JOIN Tbl_Area ON TblManagerSMSDC.AreaId = Tbl_Area.AreaId
				INNER JOIN TblMultistepConnection ON TblRequest.LPRequestId = TblMultistepConnection.LPRequestId
			WHERE  
				TblManagerSMSDC.IsActive = 1
				AND IsSendSMSAfterConnect = 0
				AND EndJobStateId  = 5
				AND ISNULL(TblMultistepConnection.IsNotConnect,0) = 0
			UNION
			SELECT 
				TblRequest.RequestId,
				TblMultistepConnection.MPMultistepConnectionId,
				TblManagerSMSDC.ManagerSMSDCId,
				TblManagerSMSDC.ManagerMobile,
				TblManagerSMSDC.AreaId,
				TblRequest.DisconnectDatePersian,
				TblRequest.DisconnectTime,
				TblRequest.RequestNumber,
				TblMultistepConnection.ConnectDT,
				CAST(0 AS bit) AS IsMPRequest,
				CAST(0 AS bit) AS IsLPRequest,
				CAST(1 AS bit) AS IsFTRequest
			FROM   
				TblManagerSMSDCSended 
				INNER JOIN TblManagerSMSDC ON TblManagerSMSDCSended.ManagerSMSDCId = TblManagerSMSDC.ManagerSMSDCId 
				INNER JOIN TblRequest ON TblManagerSMSDCSended.RequestId = TblRequest.RequestId
				INNER JOIN Tbl_Area ON TblManagerSMSDC.AreaId = Tbl_Area.AreaId
				INNER JOIN TblMultistepConnection ON TblRequest.FogheToziDisconnectId = TblMultistepConnection.FogheToziDisconnectId
			WHERE  
				TblManagerSMSDC.IsActive = 1
				AND IsSendSMSAfterConnect = 0
				AND EndJobStateId  = 5
				AND ISNULL(TblMultistepConnection.IsNotConnect,0) = 0
			) t1 
		ORDER BY
			ConnectDt
				
		OPEN MultistepConnectionList
		
		DECLARE @lIsLoop AS bit	
		SET @lIsLoop = 1
		WHILE @lIsLoop = 1
		BEGIN
			FETCH NEXT
				FROM MultistepConnectionList
				INTO @RequestId, @MultistepConnectionId, @ManagerSMSDCId,
					@ManagerMobile, @ManagerAreaId, @DisconnectDatePersian, @DisconnectTime, @RequestNumber,
					@IsMPRequest, @IsLPRequest, @IsFTRequest

			if @@FETCH_STATUS = 0 
			BEGIN
				IF NOT EXISTS (SELECT * FROM TblMultiStepSMS WHERE MultistepConnectionId = @MultistepConnectionId AND ManagerSMSDCId = @ManagerSMSDCId)
				BEGIN
					SELECT 
						@ConnectType = CASE WHEN IsManoeuvre = 1 THEN '»Â Ê”Ì·Â „«‰Ê—' ELSE '»« „Ê›ﬁÌ ' END,
						@ConnectDatePersian = TblMultistepConnection.ConnectDatePersian,
						@ConnectTime = TblMultistepConnection.ConnectTime,
						@CurrentValue = CAST(TblMultistepConnection.CurrentValue AS VARCHAR(10)),
						@ConnectDarsad = CAST(TblMultistepConnection.[Percent] AS VARCHAR(10)),
						@Comments = TblMultistepConnection.Comments
					FROM
						TblMultistepConnection
					WHERE 
						TblMultistepConnection.MPMultistepConnectionId = @MultistepConnectionId
						
					IF ISNULL(@Comments,'') <> '' 
						SET @Comments = ' Ê÷ÌÕ« : ' + @Comments
						
					IF @IsFTRequest = 1
					BEGIN
						SELECT 
							@MPPostName = Tbl_MPPost.MPPostName,
							@MPFeederName = Tbl_MPFeeder.MPFeederName,
							@DCCurrentValue = TblFogheToziDisconnectMPFeeder.CurrentValue
						FROM
							TblRequest 
							INNER JOIN TblFogheToziDisconnect ON TblRequest.FogheToziDisconnectId = TblFogheToziDisconnect.FogheToziDisconnectId
							INNER JOIN TblFogheToziDisconnectMPFeeder ON TblFogheToziDisconnect.FogheToziDisconnectId = TblFogheToziDisconnectMPFeeder.FogheToziDisconnectId
							INNER JOIN Tbl_MPFeeder ON TblFogheToziDisconnectMPFeeder.MPFeederId = Tbl_MPFeeder.MPFeederId
							INNER JOIN Tbl_MPPost ON TblFogheToziDisconnect.MPPostId = Tbl_MPPost.MPPostId
							INNER JOIN TblMultistepConnection ON TblFogheToziDisconnect.FogheToziDisconnectId = TblMultistepConnection.FogheToziDisconnectId
						WHERE
							TblMultistepConnection.MPMultistepConnectionId = @MultistepConnectionId
							AND TblMultistepConnection.SourceMPFeederId = TblFogheToziDisconnectMPFeeder.MPFeederId
							
						SET @SMS = @SMSFT
					END
					ELSE IF @IsLPRequest = 1
					BEGIN
						SELECT 
							@MPPostName = Tbl_MPPost.MPPostName,
							@MPFeederName = Tbl_MPFeeder.MPFeederName,
							@LPPostName = Tbl_LPPost.LPPostName,
							@LPFeederName = Tbl_LPFeeder.LPFeederName,
							@DCCurrentValue = TblLPRequest.CurrentValue
						FROM
							TblRequest 
							INNER JOIN TblLPRequest ON TblRequest.LPRequestId = TblLPRequest.LPRequestId
							LEFT JOIN Tbl_LPFeeder ON TblLPRequest.LPFeederId = Tbl_LPFeeder.LPFeederId
							LEFT JOIN Tbl_LPPost ON TblLPRequest.LPPostId = Tbl_LPPost.LPPostId
							LEFT JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId
							LEFT JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId
							INNER JOIN TblMultistepConnection ON TblLPRequest.LPRequestId = TblMultistepConnection.LPRequestId
						WHERE
							TblMultistepConnection.MPMultistepConnectionId = @MultistepConnectionId
							
						SET @SMS = @SMSLP
					END
					ELSE IF @IsMPRequest = 1
					BEGIN
						SELECT 
							@MPPostName = Tbl_MPPost.MPPostName,
							@MPFeederName = Tbl_MPFeeder.MPFeederName,
							@DCCurrentValue = TblMPRequest.CurrentValue
						FROM
							TblRequest 
							INNER JOIN TblMPRequest ON TblRequest.MPRequestId = TblMPRequest.MPRequestId
							LEFT JOIN Tbl_MPFeeder ON TblMPRequest.MPFeederId = Tbl_MPFeeder.MPFeederId
							LEFT JOIN Tbl_MPPost ON TblMPRequest.MPPostId = Tbl_MPPost.MPPostId
							INNER JOIN TblMultistepConnection ON TblMPRequest.MPRequestId = TblMultistepConnection.MPRequestId
						WHERE
							TblMultistepConnection.MPMultistepConnectionId = @MultistepConnectionId

						SET @SMS = @SMSMP
					END
					
					SET @SMS = Replace(@SMS,'ConnectType', ISNULL(@ConnectType,''))
					SET @SMS = Replace(@SMS,'DisconnectDatePersian', ISNULL(@DisconnectDatePersian,'ø'))
					SET @SMS = Replace(@SMS,'RequestNumber', ISNULL(@RequestNumber,'ø'))
					SET @SMS = Replace(@SMS,'DisconnectTime', ISNULL(@DisconnectTime,'ø'))
					SET @SMS = Replace(@SMS,'MPPostName', ISNULL(@MPPostName,'ø'))
					SET @SMS = Replace(@SMS,'MPFeederName', ISNULL(@MPFeederName,'ø'))
					SET @SMS = Replace(@SMS,'LPPostName', ISNULL(@LPPostName,'ø'))
					SET @SMS = Replace(@SMS,'LPFeederName', ISNULL(@LPFeederName,'ø'))
					SET @SMS = Replace(@SMS,'ConnectDatePersian', ISNULL(@ConnectDatePersian,'ø'))
					SET @SMS = Replace(@SMS,'ConnectTime', ISNULL(@ConnectTime,'ø'))
					SET @SMS = Replace(@SMS,'DCCurrentValue', ISNULL(@DCCurrentValue,'ø'))
					SET @SMS = Replace(@SMS,'CurrentValue', ISNULL(@CurrentValue,'ø'))
					SET @SMS = Replace(@SMS,'ConnectDarsad', ISNULL(@ConnectDarsad,'ø'))
					SET @SMS = Replace(@SMS,'Comments', ISNULL(@Comments,''))
					
					INSERT INTO 
						TblMultiStepSMS (MultistepConnectionId, ManagerSMSDCId, SendDT)
					VALUES 
						(@MultistepConnectionId, @ManagerSMSDCId, GETDATE()) 

					DECLARE @Desc AS nvarchar(100)
					SET @Desc = 'MultistepConnectionId = ' + Cast(@MultistepConnectionId as nvarchar) 
					EXEC spSendSMS @SMS , @ManagerMobile, @Desc, '', @ManagerAreaId
					
				END
			END
			ELSE
				SET @lIsLoop = 0
		END
	CLOSE MultistepConnectionList
	DEALLOCATE MultistepConnectionList

GO


IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spCheckNewSTManagerPeakArea]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spCheckNewSTManagerPeakArea]
GO
CREATE PROCEDURE [dbo].[spCheckNewSTManagerPeakArea]
AS
	DECLARE @RequestType AS NVARCHAR(200)
	DECLARE @SMS AS NVARCHAR(2000)
	DECLARE @SMSClause AS NVARCHAR(2000)
	DECLARE @ManagerSMSSTId AS INT
	DECLARE @ManagerMobile AS VARCHAR(20)
	DECLARE @AreaId AS INT
	DECLARE @Area AS NVARCHAR(50)
	DECLARE @PeakArea_SendTime AS VARCHAR(5)
	DECLARE @PeakArea_LastSendTime AS DATETIME
	DECLARE @lToday AS DATETIME
	DECLARE @lThisTime AS DATETIME
	DECLARE @lHour AS INT
	DECLARE @lMin AS INT
	DECLARE @lDiff AS INT,
		@lDiffNow AS INT

	SET @lToday = GETDATE()
	SET @lToday = DATEADD(ms, - DATEPART(ms, @lToday), @lToday)
	SET @lToday = DATEADD(ss, - DATEPART(ss, @lToday), @lToday)
	SET @lToday = DATEADD(mi, - DATEPART(mi, @lToday), @lToday)
	SET @lToday = DATEADD(hh, - DATEPART(hh, @lToday), @lToday)


	DECLARE @PeakValue AS FLOAT
	DECLARE @PeakDate AS VARCHAR(10)
	DECLARE @Percent AS FLOAT
	DECLARE @MaxPeak AS FLOAT
	DECLARE @MaxPeakDate AS VARCHAR(10)
	DECLARE @LastMaxPeak AS FLOAT
	DECLARE @MaxPercent AS FLOAT
	DECLARE @PeakAsynch AS FLOAT
	DECLARE @MaxPeakAsynch AS FLOAT
	DECLARE @PercentAsynch AS FLOAT
	DECLARE @LastPeakAsynch AS FLOAT
	DECLARE @MaxPeakAsynchDate AS VARCHAR(10)
	DECLARE @lYear AS VARCHAR(4)
	DECLARE @LastDate AS VARCHAR(10)
	DECLARE @LastPeak AS FLOAT
	DECLARE @FirstYearDate AS VARCHAR(10)
	DECLARE @LastPeakDate AS VARCHAR(10)
	DECLARE @FirstPastYear AS VARCHAR(10)
	DECLARE @LastPastYear AS VARCHAR(10)
	DECLARE @lQuery AS VARCHAR(1000)
	DECLARE @lSql AS VARCHAR(2000)
	DECLARE @lPDate AS VARCHAR(10)
	DECLARE @lDT AS DATETIME
	DECLARE @PeakDayNight AS FLOAT
	DECLARE @MaxPeakDayNight AS FLOAT
	DECLARE @MaxDayNightPercent AS FLOAT
	DECLARE @MainPeakProtocol AS VARCHAR(20)
	DECLARE @TopPeak as float
	DECLARE @ForcastPeak as float
	DECLARE @LoadManagement as float
	
	DECLARE @lCnt AS int
	SET @lCnt = 0
	
	SET @MainPeakProtocol = ''

	SET @RequestType = 'SMSDailyPeakArea'
	SET @SMSClause = NULL
	SELECT @SMSClause = ConfigText FROM Tbl_Config WHERE ConfigName = @RequestType
	IF @SMSClause IS NUlL RETURN 0

	CREATE TABLE #tmpAreaBody (AreaId int, SMSBody nvarchar(500))

	DECLARE CursorAreaPeak CURSOR FOR
		SELECT 
			DISTINCT TblManagerSMSSTArea.AreaId, Tbl_Area.Area
		FROM 
			TblManagerSMSST
			INNER JOIN TblManagerSMSSTArea ON TblManagerSMSST.ManagerSMSSTId = TblManagerSMSSTArea.ManagerSMSSTId
			INNER JOIN Tbl_Area ON TblManagerSMSSTArea.AreaId = Tbl_Area.AreaId
			INNER JOIN TblAreaPeak ON TblManagerSMSSTArea.AreaId = TblAreaPeak.AreaId
		WHERE 
			PeakArea_IsActive = 1

		OPEN CursorAreaPeak
		
		DECLARE @lIsLoop AS bit	
		SET @lIsLoop = 1
		WHILE @lIsLoop = 1
		BEGIN
			FETCH NEXT
				FROM CursorAreaPeak
				INTO @AreaId, @Area

			if @@FETCH_STATUS = 0 
			BEGIN
					SET @PeakDate = ''
					SET @PeakValue = 0
					SET @Percent = 0
					SET @MaxPercent = 0
					SET @MaxPeak = 0
					SET @LastMaxPeak = 0
					SET @MaxPeakDate = '-'
					SET @lYear = ''
					SET @LastDate = '-'
					SET @LastPeak = 0
					SET @FirstYearDate = '-'
					SET @FirstPastYear = '-'
					SET @LastPastYear = '-'
					SET @LastPeakDate = '-'
					SET @PeakDayNight = 0
					SET @MaxPeakDayNight = 0
					SET @MaxDayNightPercent = 0
					SET @PeakAsynch = 0
					SET @MaxPeakAsynch = 0
					SET @PercentAsynch = 0
					SET @LastPeakAsynch = 0
					SET @MaxPeakAsynchDate = '-'
					SET @TopPeak = 0
					SET @ForcastPeak = 0
					SET @LoadManagement = 0

					SELECT @MainPeakProtocol = ConfigValue
					FROM Tbl_Config
					WHERE ConfigName = 'MainPeakProtocol'

					SELECT TOP 1 @PeakDate = PeakDatePersian,
						@PeakValue = CASE 
							WHEN @MainPeakProtocol = ''
								THEN PeakSynch
							ELSE CASE 
									WHEN @MainPeakProtocol = 'MaxPeak'
										THEN CASE 
												WHEN PeakSynch > PeakAsynch
													THEN PeakSynch
												ELSE PeakAsynch
												END
									END
							END,
						@lYear = LEFT(PeakDatePersian, 4),
						@PeakAsynch = PeakAsynch,
						@TopPeak = TopPeak,
						@ForcastPeak = ForcastPeak,
						@LoadManagement = LoadManagement
					FROM TblAreaPeak
					WHERE CASE 
							WHEN @MainPeakProtocol = ''
								THEN PeakSynch
							ELSE CASE 
									WHEN @MainPeakProtocol = 'MaxPeak'
										THEN CASE 
												WHEN PeakSynch > PeakAsynch
													THEN PeakSynch
												ELSE PeakAsynch
												END
									END
							END > 0
						AND AreaId = @AreaId
					ORDER BY PeakDatePersian DESC,
						PeakTime DESC

					IF @PeakDate <> ''
					BEGIN
						SET @FirstYearDate = @lYear + '/01/01'
						SET @lYear = Cast((Cast(@lYear AS INT) - 1) AS VARCHAR)
						SET @LastDate = @lYear + Right(@PeakDate, 6)
						SET @FirstPastYear = @lYear + '/01/01'
						SET @LastPastYear = @lYear + '/12/30'

						SELECT @LastPeak = CASE 
								WHEN @MainPeakProtocol = ''
									THEN PeakSynch
								ELSE CASE 
										WHEN @MainPeakProtocol = 'MaxPeak'
											THEN CASE 
													WHEN PeakSynch > PeakAsynch
														THEN PeakSynch
													ELSE PeakAsynch
													END
										END
								END,
							@LastPeakAsynch = PeakAsynch
						FROM TblAreaPeak
						WHERE PeakDatePersian = @LastDate
							AND AreaId = @AreaId

						IF @LastPeak > 0
						BEGIN
							SET @Percent = ROUND((@PeakValue - @LastPeak) / @LastPeak * 100, 2)
						END

						IF @LastPeakAsynch > 0
						BEGIN
							SET @PercentAsynch = ROUND((@PeakAsynch - @LastPeakAsynch) / @LastPeakAsynch * 100, 2)
						END

						SELECT @MaxPeak = MAX(CASE 
									WHEN @MainPeakProtocol = ''
										THEN PeakSynch
									ELSE CASE 
											WHEN @MainPeakProtocol = 'MaxPeak'
												THEN CASE 
														WHEN PeakSynch > PeakAsynch
															THEN PeakSynch
														ELSE PeakAsynch
														END
											END
									END)
						FROM TblAreaPeak
						WHERE PeakDatePersian >= @FirstYearDate
							AND PeakDatePersian <= @PeakDate
							AND AreaId = @AreaId

						SELECT @MaxPeakAsynch = MAX(PeakAsynch)
						FROM TblAreaPeak
						WHERE PeakDatePersian >= @FirstYearDate
							AND PeakDatePersian <= @PeakDate
							AND AreaId = @AreaId

						SELECT @MaxPeakDate = PeakDatePersian
						FROM TblAreaPeak
						WHERE PeakDatePersian >= @FirstYearDate
							AND PeakDatePersian <= @PeakDate
							AND CASE 
								WHEN @MainPeakProtocol = ''
									THEN PeakSynch
								ELSE CASE 
										WHEN @MainPeakProtocol = 'MaxPeak'
											THEN CASE 
													WHEN PeakSynch > PeakAsynch
														THEN PeakSynch
													ELSE PeakAsynch
													END
										END
								END = @MaxPeak
							AND AreaId = @AreaId

						SELECT @MaxPeakAsynchDate = PeakDatePersian
						FROM TblAreaPeak
						WHERE PeakDatePersian >= @FirstPastYear
							AND PeakDatePersian <= @PeakDate
							AND PeakAsynch = @MaxPeakAsynch
							AND AreaId = @AreaId

						SELECT @LastMaxPeak = MAX(CASE 
									WHEN @MainPeakProtocol = ''
										THEN PeakSynch
									ELSE CASE 
											WHEN @MainPeakProtocol = 'MaxPeak'
												THEN CASE 
														WHEN PeakSynch > PeakAsynch
															THEN PeakSynch
														ELSE PeakAsynch
														END
											END
									END)
						FROM TblAreaPeak
						WHERE PeakDatePersian >= @FirstPastYear
							AND PeakDatePersian <= @LastPastYear
							AND AreaId = @AreaId

						SELECT @LastPeakDate = PeakDatePersian
						FROM TblAreaPeak
						WHERE PeakDatePersian >= @FirstPastYear
							AND PeakDatePersian <= @LastPastYear
							AND CASE 
								WHEN @MainPeakProtocol = ''
									THEN PeakSynch
								ELSE CASE 
										WHEN @MainPeakProtocol = 'MaxPeak'
											THEN CASE 
													WHEN PeakSynch > PeakAsynch
														THEN PeakSynch
													ELSE PeakAsynch
													END
										END
								END = @LastMaxPeak
							AND AreaId = @AreaId

						IF @LastMaxPeak > 0
						BEGIN
							SET @MaxPercent = ROUND((@MaxPeak - @LastMaxPeak) / @LastMaxPeak * 100, 2)
						END

						/*-------------*/
						SELECT TOP 1 @PeakDayNight = ISNULL(CASE 
									WHEN PeakDay > PeakNight
										THEN PeakDay
									ELSE PeakNight
									END, 0)
						FROM TblAreaPeak
						WHERE PeakDatePersian = @PeakDate
							AND AreaId = @AreaId

						SELECT @MaxPeakDayNight = ISNULL(MAX(CASE 
										WHEN PeakDay > PeakNight
											THEN PeakDay
										ELSE PeakNight
										END), 0)
						FROM TblAreaPeak
						WHERE PeakDatePersian >= @FirstYearDate
							AND PeakDatePersian <= @PeakDate
							AND AreaId = @AreaId
					END
					ELSE
						SET @PeakDate = '-'

					SET @SMS = @SMSClause
					SET @lDT = GETDATE()
					SET @lPDate = dbo.mtosh(@lDT)
					SET @SMS = Replace(@SMS, 'yy', SUBSTRING(@lPDate, 1, 4))
					SET @SMS = Replace(@SMS, 'mm', SUBSTRING(@lPDate, 6, 2))
					SET @SMS = Replace(@SMS, 'dd', SUBSTRING(@lPDate, 9, 2))
					SET @SMS = Replace(@SMS, 'hh', Cast(DATEPART(hh, @lDT) AS VARCHAR))
					SET @SMS = Replace(@SMS, 'nn', Cast(DATEPART(mi, @lDT) AS VARCHAR))
					SET @SMS = Replace(@SMS, 'MaxPeakAsynchDate', @MaxPeakAsynchDate)
					SET @SMS = Replace(@SMS, 'MaxPeakAsynch', @MaxPeakAsynch)
					SET @SMS = Replace(@SMS, 'PercentAsynch', @PercentAsynch)
					SET @SMS = Replace(@SMS, 'PeakAsynch', @PeakAsynch)
					SET @SMS = Replace(@SMS, 'MaxPeakDayNight', @MaxPeakDayNight)
					SET @SMS = Replace(@SMS, 'PeakDayNight', @PeakDayNight)
					SET @SMS = Replace(@SMS, 'LastPeakDate', @LastPeakDate)
					SET @SMS = Replace(@SMS, 'MaxPeakDate', @MaxPeakDate)
					SET @SMS = Replace(@SMS, 'MaxPeak', @MaxPeak)
					SET @SMS = Replace(@SMS, 'TopPeak', @TopPeak)
					SET @SMS = Replace(@SMS, 'ForcastPeak', @ForcastPeak)
					SET @SMS = Replace(@SMS, 'LoadManagement', @LoadManagement)
					SET @SMS = Replace(@SMS, 'PeakValue', @PeakValue)
					SET @SMS = Replace(@SMS, 'PeakDate', @PeakDate)
					SET @SMS = Replace(@SMS, 'LastDate', @LastDate)
					SET @SMS = Replace(@SMS, 'MaxPercent', @MaxPercent)
					SET @SMS = Replace(@SMS, 'Percent', @Percent)
					SET @SMS = Replace(@SMS, 'Area', @Area)
					SET @SMS = Replace(@SMS, 'CRLF', NCHAR(13))
					SET @lDT = DATEADD(dd, 1, @lThisTime)
					
					INSERT INTO #tmpAreaBody VALUES (@AreaId, @SMS)

			END
			ELSE
				SET @lIsLoop = 0
		END
	CLOSE CursorAreaPeak
	DEALLOCATE CursorAreaPeak
	DECLARE @lAreaId as int
	DECLARE CursorManager CURSOR FOR
		SELECT 
			TblManagerSMSST.ManagerSMSSTId,
			TblManagerSMSST.ManagerMobile,
			TblManagerSMSST.PeakArea_SendTime,  
			TblManagerSMSST.PeakArea_LastSendTime,
			TblManagerSMSST.AreaId
		FROM 
			TblManagerSMSST
		WHERE 
			PeakArea_IsActive = 1

		OPEN CursorManager
		
		SET @lIsLoop = 1
		WHILE @lIsLoop = 1
		BEGIN
			FETCH NEXT
				FROM CursorManager
				INTO @ManagerSMSSTId, @ManagerMobile, @PeakArea_SendTime, @PeakArea_LastSendTime, @lAreaId

			if @@FETCH_STATUS = 0 
			BEGIN
				SET @lHour = Cast(LEFT(@PeakArea_SendTime,2) AS int)
				SET @lMin = Cast(RIGHT(@PeakArea_SendTime,2) AS int)
				SET @lThisTime = DATEADD(hh, @lHour, @lToday)
				SET @lThisTime = DATEADD(mi, @lMin, @lThisTime)
				SET @lDiff = ISNULL(DATEDIFF(mi, @PeakArea_LastSendTime, @lThisTime),1440)
				SET @lDiffNow = DATEDIFF(mi, @lThisTime, GETDATE())
				
				IF( @lDiffNow >= 0 AND @lDiff > 10 )
				BEGIN
				
				
					DECLARE @lPeakAreaId as int
					DECLARE @lAllAreaBody as nvarchar(1000)
					set @lAllAreaBody = ''
					DECLARE CursorArea CURSOR FOR
						SELECT 
							TblManagerSMSSTArea.AreaId 
						FROM 
							TblManagerSMSSTArea
						WHERE 
							ManagerSMSSTId = @ManagerSMSSTId
							
						OPEN CursorArea
						
						DECLARE @lIsAreaLoop as bit
						SET @lIsAreaLoop = 1
						WHILE @lIsAreaLoop = 1
						BEGIN
							FETCH NEXT
								FROM CursorArea
								INTO @lPeakAreaId

							if @@FETCH_STATUS = 0 
							BEGIN
								declare @lSMS as nvarchar(200) = ''
								select @lSMS = ISNULL(SMSBody,'') FROM #tmpAreaBody WHERE AreaId = @lPeakAreaId
								if @lSMS <> ''
								BEGIN
									SET @lAllAreaBody = @lAllAreaBody + @lSMS
								END
							END
							ELSE
								SET @lIsAreaLoop = 0
						END
					CLOSE CursorArea
					DEALLOCATE CursorArea
					
					IF @lAllAreaBody <> ''
					BEGIN
						SET @lDT = DATEADD(dd,1,@lThisTime)
						SET @lPDate = 
							dbo.mtosh( @lDT ) + ' ' +
							Cast(DATEPART(hh,@lDT) AS varchar) + ':' +
							Cast(DATEPART(mi,@lDT) AS varchar)
						SET @RequestType = 'SMSDailyPeakArea NextDT:' + @lPDate
						
						DECLARE @Desc AS nvarchar(100)
						SET @Desc = 'ManagerSMSSTId=' + CAST(@ManagerSMSSTId AS nvarchar)
						EXEC spSendSMS @lAllAreaBody, @ManagerMobile, @Desc, @RequestType, @lAreaId
							
					END
					UPDATE TblManagerSMSST
					SET PeakArea_LastSendTime = GETDATE()
					WHERE ManagerSMSSTId = @ManagerSMSSTId
					
					SET @lCnt = @lCnt + 1
				END
			END
			ELSE
				SET @lIsLoop = 0
		END
	CLOSE CursorManager
	DEALLOCATE CursorManager
	DROP TABLE #tmpAreaBody
		
	RETURN @lCnt
GO

/* ------------------- «—”«· ÅÌ«„ò  «ÌÌœ Ì« ⁄œ„  «ÌÌœ Œ«„Ê‘Ì Â«Ì »«»—‰«„Â »Â ÅÌ„«‰ò«—«‰ -------------------- */

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spSendSMSConfirmForPeymankar]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spSendSMSConfirmForPeymankar]
GO
CREATE PROCEDURE [dbo].[spSendSMSConfirmForPeymankar]
	@TamirRequestId As bigint,  
	@TamirRequestStateId As int, 
	@AreaId As int
AS

	IF EXISTS(SELECT * FROM TblTamirRequestSendAlarmForPeymankar WHERE TamirRequestId = @TamirRequestId AND IsSendSMSConfirm = 1 ) RETURN 0;

	DECLARE @RequestNumber AS bigint
	DECLARE @DBMinutes AS int
	DECLARE @MPPostName AS nvarchar(200)
	DECLARE @MPFeederName AS nvarchar(200)
	DECLARE @LPPostName AS nvarchar(200)
	DECLARE @LPFeederName AS nvarchar(200)
	DECLARE @DisconnectPower AS nvarchar(200)
	DECLARE @DTTime AS nvarchar(200)
	DECLARE @DTDate AS nvarchar(200)
	DECLARE @Area AS nvarchar(200)
	DECLARE @Status AS nvarchar(200) 
	DECLARE @Address AS nvarchar(200)
	DECLARE @CriticalsAddress AS nvarchar(200)
	DECLARE @WarmLineMode AS nvarchar(200)
	DECLARE @WarmLineAdvance AS nvarchar(200)
	DECLARE @IsWarmLine AS bit
	
	DECLARE @MPPostInfo AS nvarchar(400)
	DECLARE @MPFeedersInfo AS nvarchar(1000)
	DECLARE @MPFeederInfo AS nvarchar(400)
	DECLARE @LPPostInfo AS nvarchar(400)
	DECLARE @FeederPartInfo AS nvarchar(400)
	DECLARE @LPFeederInfo AS nvarchar(400)
	
	SET @MPPostInfo = ''
	SET @MPFeedersInfo = ''
	SET @MPFeederInfo = ''
	SET @LPPostInfo = ''
	SET @FeederPartInfo = ''
	SET @LPFeederInfo = ''

	DECLARE @RequestType AS nvarchar(200)
	DECLARE @SMS AS nvarchar(2000)
	
	DECLARE @CurrentDate AS varchar(10)
	DECLARE @CurrentMonthDay AS varchar(5)
	SELECT @CurrentDate = dbo.mtosh(getdate())
	SET @CurrentMonthDay = RIGHT(@CurrentDate,5)
	DECLARE @PeymankarMobileNo as nvarchar(20) = ''
	
	SELECT 
		@RequestNumber = TblTamirRequest.TamirRequestNo, 
		@MPPostName = Tbl_MPPost.MPPostName, 
		@MPPostInfo = 'Å”  ›Êﬁ  Ê“Ì⁄ ' + Tbl_MPPost.MPPostName + 'CRLF', 
		@MPFeederName = Tbl_MPFeeder.MPFeederName, 
		@MPFeederInfo = '›Ìœ— ›‘«— „ Ê”ÿ ' + Tbl_MPFeeder.MPFeederName + 'CRLF', 
		@LPPostName = Tbl_LPPost.LPPostName, 
		@LPPostInfo = 'Å”   Ê“Ì⁄ ' + Tbl_LPPost.LPPostName + 'CRLF', 
		@FeederPartInfo = ' òÂ ›Ìœ— ' + Tbl_FeederPart.FeederPart + 'CRLF', 
		@LPFeederInfo = '›Ìœ— ›‘«— ÷⁄Ì› ' + Tbl_LPFeeder.LPFeederName + 'CRLF',
		@LPFeederName = Tbl_LPFeeder.LPFeederName, 
		@Address = TblTamirRequest.WorkingAddress, 
		@CriticalsAddress = TblTamirRequest.CriticalsAddress, 
		@DBMinutes = TblTamirRequest.DisconnectInterval, 
		@DTDate = ISNULL(TblTamirRequest.DisconnectDatePersian,''), 
		@DTTime = ISNULL(TblTamirRequest.DisconnectTime,''), 
		@DisconnectPower = Cast( Round(ISNULL(TblTamirRequest.DisconnectPower,0),2) AS varchar ), 
		@Area = ISNULL(Tbl_Area.Area,''),
		@WarmLineMode = CASE WHEN TblTamirRequest.IsWarmLine = 1 THEN 'Œÿ ê—„' ELSE 'Œÿ ”—œ' END,
		@IsWarmLine = TblTamirRequest.IsWarmLine,
		@PeymankarMobileNo = TblTamirRequest.PeymankarMobileNo
	FROM 
		TblTamirRequest 
		INNER JOIN Tbl_Area ON TblTamirRequest.AreaId = Tbl_Area.AreaId 
		LEFT OUTER JOIN Tbl_MPPost ON TblTamirRequest.MPPostId = Tbl_MPPost.MPPostId 
		LEFT OUTER JOIN Tbl_MPFeeder ON TblTamirRequest.MPFeederId = Tbl_MPFeeder.MPFeederId 
		LEFT OUTER JOIN Tbl_LPPost ON TblTamirRequest.LPPostId = Tbl_LPPost.LPPostId
		LEFT OUTER JOIN Tbl_LPFeeder ON TblTamirRequest.LPFeederId = Tbl_LPFeeder.LPFeederId 
		LEFT OUTER JOIN Tbl_FeederPart ON TblTamirRequest.FeederPartId = Tbl_FeederPart.FeederPartId
	WHERE 
		TblTamirRequest.TamirRequestId = @TamirRequestId
	
	DECLARE @MPFeedersName AS nvarchar(1000)
	SET @MPFeedersName = ''
	SELECT 
		@MPFeedersName = @MPFeedersName + MPFeederName + ', '
	FROM 
		TblTamirRequestFTFeeder
		INNER JOIN Tbl_MPFeeder ON TblTamirRequestFTFeeder.MPFeederId = Tbl_MPFeeder.MPFeederId
	WHERE
		TblTamirRequestFTFeeder.TamirRequestId = @TamirRequestId

	IF CHARINDEX(',' , @MPFeedersName) > 0
	BEGIN
		SET @MPFeedersInfo = '›Ìœ—Â«Ì ›‘«— „ Ê”ÿ ' + LEFT(@MPFeedersName, (LEN(@MPFeedersName) - 1)) + 'CRLF'
		SET @MPFeederInfo = ''
	END

	SET @WarmLineAdvance = ''
	if @TamirRequestStateId = 3 
	BEGIN
		SET @Status = N' «ÌÌœ ê—œÌœ'
		if @IsWarmLine = 1
			SET @WarmLineAdvance = '»’Ê—  Œÿ ê—„'
		if @IsWarmLine = 0
			SET @WarmLineAdvance = '»’Ê—  Œÿ ”—œ'
	END
	if @TamirRequestStateId = 8 
		SET @Status = N' «ÌÌœ ‰ê—œÌœ'


	SET @RequestType = 'SendConfirmPeymankar'
	SELECT @SMS = ConfigText FROM Tbl_Config WHERE ConfigName = @RequestType
	----
		
	SET @SMS = Replace( @SMS , 'CRLF', nchar(13) );
	SET @SMS = Replace( @SMS , 'TamirReqNo' , ISNULL(@RequestNumber,0))
	SET @SMS = Replace( @SMS , 'Minutes' , ISNULL(@DBMinutes,0))
	SET @SMS = Replace( @SMS , 'MPPostName' , ISNULL(@MPPostName,'-'))
	SET @SMS = Replace( @SMS , 'MPPostInfo ' , ISNULL(@MPPostInfo,''))
	SET @SMS = Replace( @SMS , 'Area' , ISNULL(@Area,N'ø'))
	SET @SMS = Replace( @SMS , 'MPFeederName' , ISNULL(@MPFeederName,'-'))
	SET @SMS = Replace( @SMS , 'MPFeederInfo ' , ISNULL(@MPFeederInfo,''))
	SET @SMS = Replace( @SMS , 'MPFeedersInfo ' , ISNULL(@MPFeedersInfo,''))
	SET @SMS = Replace( @SMS , 'LPPostName' , ISNULL(@LPPostName,'-'))
	SET @SMS = Replace( @SMS , 'LPPostInfo ' , ISNULL(@LPPostInfo,''))
	SET @SMS = Replace( @SMS , 'FeederPartInfo ' , ISNULL(@FeederPartInfo,''))
	SET @SMS = Replace( @SMS , 'LPFeederName' , ISNULL(@LPFeederName,'-'))
	SET @SMS = Replace( @SMS , 'LPFeederInfo ' , ISNULL(@LPFeederInfo,''))
	SET @SMS = Replace( @SMS , 'DBAddress' , ISNULL(@Address,N'ø'))
	SET @SMS = Replace( @SMS , 'CriticalsAddress' , ISNULL(@CriticalsAddress,N'ø'))
	SET @SMS = Replace( @SMS , 'DisconnectPower', ISNULL(@DisconnectPower,N'ø'))
	SET @SMS = Replace( @SMS , 'DTDate' , ISNULL(@DTDate,N'ø'))
	SET @SMS = Replace( @SMS , 'DTTime' , ISNULL(@DTTime,N'ø'))
	SET @SMS = Replace( @SMS , 'WarmLineMode' , ISNULL(@WarmLineMode,N'ø'))
	SET @SMS = Replace( @SMS , 'WarmLineAdvance' , ISNULL(@WarmLineAdvance,N'ø'))
	SET @SMS = Replace( @SMS , 'Status' , ISNULL(@Status,N'ø'))
	SET @SMS = Replace( @SMS , 'CurrentDate', ISNULL(@CurrentDate,N''))
	SET @SMS = Replace( @SMS , 'CurrentMonthDay', ISNULL(@CurrentMonthDay,N''))
	SET @SMS = Replace( @SMS , 'CRLF', nchar(13))

	DECLARE @Desc AS nvarchar(100)
	SET @Desc = 'TamirReqId=' +  + Cast(@RequestNumber as nvarchar)
	
	IF ISNULL(@PeymankarMobileNo,'') <> '' AND ISNULL(@SMS,'') <> ''
		EXEC spSendSMS @SMS , @PeymankarMobileNo, @Desc, @RequestType, @AreaId
		
	IF EXISTS (SELECT * FROM TblTamirRequestSendAlarmForPeymankar WHERE TamirRequestId = @TamirRequestId)
		UPDATE TblTamirRequestSendAlarmForPeymankar SET IsSendSMSConfirm = 1 WHERE TamirRequestId = @TamirRequestId
	ELSE
		INSERT INTO TblTamirRequestSendAlarmForPeymankar (TamirRequestId,PeymankarMobileNo,IsSendSMSConfirm) VALUES(@TamirRequestId,@PeymankarMobileNo,1)
	
GO


/* ------------------- «—”«· ÅÌ«„ò ò‰”· ‘œ‰ Œ«„Ê‘ÌÂ«Ì »«»—‰«„Â »Â „‘ —òÌ‰ -------------------- */


IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spCheckNewCancelSMSEvent]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[spCheckNewCancelSMSEvent]
GO

CREATE PROCEDURE [dbo].[spCheckNewCancelSMSEvent]
AS
DECLARE @SMS AS NVARCHAR(2000)
DECLARE @ConfigText AS NVARCHAR(2000)
DECLARE @TelMobile AS NVARCHAR(200)
DECLARE @lDate AS DATETIME = getdate()
DECLARE @RequestId AS BIGINT
DECLARE @AreaId AS INT
DECLARE @DisconnectDatePersian AS VARCHAR(10)
DECLARE @DisconnectTime AS VARCHAR(5)
DECLARE @DisconnectDT AS DATETIME

SELECT @ConfigText = ConfigText
FROM Tbl_Config
WHERE ConfigName = 'SMSCancelRequest'

SELECT RequestId
INTO #tmpRequest
FROM TblRequestCancelSMS 
WHERE SendCancelSMSStatusId = 1

SELECT *
INTO #tblRequestCancel
FROM (
	SELECT #tmpRequest.RequestId,
		TblRequest.DisconnectDatePersian,
		ISNULL(NULLIF(TblRequest.DisconnectTime,''), TblRequest.TamirDisconnectToTime) as DisconnectTime,
		TblRequest.DisconnectDT,
		TblRequest.AreaId
	FROM TblRequest
	INNER JOIN #tmpRequest ON TblRequest.RequestId = #tmpRequest.RequestId
	
	UNION
	
	SELECT #tmpRequest.RequestId,
		TblDeleteRequest.DisconnectDatePersian,
		TblDeleteRequest.DisconnectTime,
		cast(NULL AS DATETIME) AS DisconnectDT,
		TblDeleteRequest.AreaId
	FROM TblDeleteRequest
	INNER JOIN #tmpRequest ON TblDeleteRequest.RequestId = #tmpRequest.RequestId
	) AS t1

DROP TABLE #tmpRequest

UPDATE #tblRequestCancel
SET DisconnectDT = CONVERT(NVARCHAR(10), dbo.shtom(DisconnectDatePersian), 21) + ' ' + DisconnectTime
WHERE DisconnectDT IS NULL

DECLARE crCancelRequest CURSOR
FOR
SELECT *
FROM #tblRequestCancel

OPEN crCancelRequest

DECLARE @lIsLoop AS BIT

SET @lIsLoop = 1

WHILE @lIsLoop = 1
BEGIN
	FETCH NEXT
	FROM crCancelRequest
	INTO @RequestId,
		@DisconnectDatePersian,
		@DisconnectTime,
		@DisconnectDT,
		@AreaId

	IF @@FETCH_STATUS = 0
	BEGIN
		IF dateadd(MINUTE, - 30, @lDate) > @DisconnectDT
		BEGIN
			UPDATE TblRequestCancelSMS
			SET SendCancelSMSStatusId = 6
			WHERE RequestId = @RequestId

			INSERT INTO Tbl_EventLogCenter (
				TableName,
				TableNameId,
				PrimaryKeyId,
				Operation,
				AreaId,
				WorkingAreaId,
				DataEntryDT,
				SQLCommand
				)
			SELECT 'TblRequestCancelSMS' AS TableName,
				359 AS TableNameId,
				@RequestId AS PrimaryKeyId,
				2 AS Operation,
				@AreaId AS AreaId,
				99 AS WorkingAreaId,
				GETDATE() AS DataEntryDT,
				NULL AS SQLCommand
		END
		ELSE
		BEGIN
			IF NOT @ConfigText IS NULL
			BEGIN
				UPDATE TblRequestCancelSMS
				SET SendCancelSMSStatusId = 3
				WHERE RequestId = @RequestId

				DECLARE @SubscriberInformId AS BIGINT
				DECLARE @SubscriberId AS INT

				DECLARE crSubCancel CURSOR
				FOR
				SELECT TblSubscriberInfom.SubscriberInformId,
					TblSubscriberInfom.SubscriberId,
					Tbl_Subscriber.TelMobile
				FROM TblSubscriberInfom
				INNER JOIN TblRequestInform ON TblSubscriberInfom.RequestInformId = TblRequestInform.RequestInformId
				INNER JOIN Tbl_Subscriber ON TblSubscriberInfom.SubscriberId = Tbl_Subscriber.SubscriberId
				WHERE TblRequestInform.RequestId = @RequestId
					AND SendSMSStatusId = 2

				OPEN crSubCancel

				DECLARE @lIsLoop2 AS BIT

				SET @lIsLoop2 = 1

				WHILE @lIsLoop2 = 1
				BEGIN
					FETCH NEXT
					FROM crSubCancel
					INTO @SubscriberInformId,
						@SubscriberId,
						@TelMobile

					IF @@FETCH_STATUS = 0
					BEGIN
						SET @SMS = @ConfigText
						SET @SMS = Replace(@SMS, 'DisconnectDate', ISNULL(NULLIF(@DisconnectDatePersian,''), N'ø'))
						SET @SMS = Replace(@SMS, 'DisconnectTime', ISNULL(NULLIF(@DisconnectTime,''), N'ø'))

						DECLARE @Desc AS NVARCHAR(100)

						SET @Desc = 'SubInform-ID=' + cast(@SubscriberInformId AS NVARCHAR)

						EXEC spSendSMS @SMS,
							@TelMobile,
							@Desc,
							'SMSCancelRequest',
							@AreaId
					END
					ELSE
						SET @lIsLoop2 = 0
				END

				CLOSE crSubCancel

				DEALLOCATE crSubCancel
				
				UPDATE TblRequestCancelSMS
				SET SendCancelSMSStatusId = 4
				WHERE RequestId = @RequestId
				
				INSERT INTO Tbl_EventLogCenter (
					TableName,
					TableNameId,
					PrimaryKeyId,
					Operation,
					AreaId,
					WorkingAreaId,
					DataEntryDT,
					SQLCommand
					)
				SELECT 'TblRequestCancelSMS' AS TableName,
					359 AS TableNameId,
					@RequestId AS PrimaryKeyId,
					2 AS Operation,
					@AreaId AS AreaId,
					99 AS WorkingAreaId,
					GETDATE() AS DataEntryDT,
					NULL AS SQLCommand
				
			END
		END
	END
	ELSE
		SET @lIsLoop = 0
END

CLOSE crCancelRequest

DEALLOCATE crCancelRequest

DROP TABLE #tblRequestCancel

GO





IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spCheckSMSEvents]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spCheckSMSEvents]
GO
CREATE PROCEDURE [dbo].[spCheckSMSEvents] 
AS
	DECLARE @lOperation AS nvarchar(1000) = ''
	DECLARE @lErrMsg AS nvarchar(500) = ''
	DECLARE @lErrCode as int = 0

	DECLARE @lCounter AS bigint
	DECLARE @lCnt AS bigint
	DECLARE @lConfigId AS int

	SET @lConfigId = NULL
	SET @lCounter = 0

	BEGIN TRY
		--- Read Cofing Counter
		SELECT @lConfigId = ConfigId FROM Tbl_Config WHERE ConfigName = 'smsCounter'
		IF @lConfigId IS NULL
			SET @lCounter = 0
		ELSE
			SELECT @lCounter = Cast(ConfigValue AS bigint) FROM Tbl_Config WHERE ConfigId = @lConfigId

		SET @lCounter = @lCounter + 1
		DECLARE	@return_smsCount int = 1
		IF (@lCounter % 7) = 0 and @return_smsCount > 0
			SET @lOperation = N'( Ê·Ìœ ÅÌ«„ »—«Ì «ÿ·«⁄«  Œ«„Ê‘ÌùÂ« («‰—éÌ Ê “„«‰) Ê ÅÌ«„  √ÌÌœ œ—ŒÊ«”  Œ«„Ê‘Ì)'
		while (@lCounter % 7) = 0 and @return_smsCount > 0
		begin
			EXEC @return_smsCount = spCheckNewDCManager	--  Ê·Ìœ ÅÌ«„ »—«Ì «ÿ·«⁄«  Œ«„Ê‘ÌùÂ« («‰—éÌ Ê “„«‰) Ê ÅÌ«„  √ÌÌœ œ—ŒÊ«”  Œ«„Ê‘Ì
		end

		IF (@lCounter % 5) = 0 
		BEGIN
			SET @lOperation = @lOperation + N'( Ê·Ìœ ÅÌ«„ ¬„«—Ì)'
			EXEC spCheckNewSTManager	--  Ê·Ìœ ÅÌ«„ ¬„«—Ì
		END
		
		IF (@lCounter % 8) = 0 
		BEGIN
			SET @lOperation = @lOperation + N'( Ê·Ìœ ÅÌ«„ ÅÌò »«— „—ò“)( Ê·Ìœ ÅÌ«„ ÅÌò »«— ‰Ê«ÕÌ)'
			EXEC spCheckNewSTManagerPeak	--  Ê·Ìœ ÅÌ«„ ÅÌò »«— „—ò“
			EXEC spCheckNewSTManagerPeakArea	--  Ê·Ìœ ÅÌ«„ ÅÌò »«— ‰Ê«ÕÌ
		END
		
		IF (@lCounter % 23) = 0 
		BEGIN
			SET @lOperation = @lOperation + N'( Ê·Ìœ ÅÌ«„ »—«Ì ﬁÿ⁄ »Ì‘ «“ ç‰œ »«— œ— „«Â ›Ìœ—)'
			EXEC spCheckMPFeederDCCountSMS	--  Ê·Ìœ ÅÌ«„ »—«Ì ﬁÿ⁄ »Ì‘ «“ ç‰œ »«— œ— „«Â ›Ìœ—
		END

		IF (@lCounter % 61) = 0 
		BEGIN
			SET @lOperation = @lOperation + N'( Ê·Ìœ ÅÌ«„ »—«Ì ›Â—”   ÃÂÌ“«  ”—ﬁ  ‘œÂ)'
			EXEC spCheckSMSSerghat	--  Ê·Ìœ ÅÌ«„ »—«Ì ›Â—”   ÃÂÌ“«  ”—ﬁ  ‘œÂ
		END
			
		IF (@lCounter % 3) = 0
		BEGIN
			SET @lOperation = @lOperation + N'( Ê·Ìœ ÅÌ«„Â«Ì À»  ‘œÂ œ— Å‰· ÅÌ«„ò)( Ê·Ìœ ÅÌ«„ Â‘œ«— »Â ÅÌ„«‰ò«—«‰ œ— Œ’Ê’ Œ«„Ê‘ÌÂ«Ì »«»—‰«„Â)'
			EXEC spCheckSMSPanel --  Ê·Ìœ ÅÌ«„Â«Ì À»  ‘œÂ œ— Å‰· ÅÌ«„ò
			EXEC spSendSMSAlarmForPeymankar  --  Ê·Ìœ ÅÌ«„ Â‘œ«— »Â ÅÌ„«‰ò«—«‰ œ— Œ’Ê’ Œ«„Ê‘ÌÂ«Ì »«»—‰«„Â
		END
			
		IF (@lCounter % 113) = 0
		BEGIN
			SET @lOperation = @lOperation + N'( Ê·Ìœ ÅÌ«„ „œ  “„«‰ Œ«„Ê‘Ì „‘ —òÌ‰ œ— —Ê“ »Â œﬁÌﬁÂ)'
			EXEC spCheckNewSTManagerSubscriber --  Ê·Ìœ ÅÌ«„ „œ  “„«‰ Œ«„Ê‘Ì „‘ —òÌ‰ œ— —Ê“ »Â œﬁÌﬁÂ
		END
			
		IF (@lCounter % 157) = 0
		BEGIN
			SET @lOperation = @lOperation + N'( Ê·Ìœ ÅÌ«„ ›Ìœ—Â«Ì »Õ—«‰Ì)'
			EXEC spCheckCriticalFeederSMS --  Ê·Ìœ ÅÌ«„ ›Ìœ—Â«Ì »Õ—«‰Ì
		END
		
		IF (@lCounter % 97) = 0
		BEGIN
			SET @lOperation = @lOperation + N'( Ê·Ìœ ÅÌ«„ n ›Ìœ— «Ê· œ«—«Ì »Ì‘ —Ì‰ «‰—éÌ  Ê“Ì⁄ ‰‘œÂ œ— „«Â)'
			EXEC spCheckSMSMPFeederDCPower --  Ê·Ìœ ÅÌ«„ n ›Ìœ— «Ê· œ«—«Ì »Ì‘ —Ì‰ «‰—éÌ  Ê“Ì⁄ ‰‘œÂ œ— „«Â
		END
		
		IF (@lCounter % 2) = 0 
		BEGIN
			SET @lOperation = @lOperation + N'( Ê·Ìœ ÅÌ«„ Œ«„Ê‘ÌùÂ« »—«Ì „‘ —òÌ‰)( Ê·Ìœ ›ò” Œ«„Ê‘Ì Â« »—«Ì „‘ —òÌ‰)( Ê·Ìœ «Ì„Ì· Œ«„Ê‘Ì Â« »—«Ì „‘ —òÌ‰)( Ê·Ìœ  „«”  ·›‰Ì Œ«„Ê‘Ì Â« »—«Ì „‘ —òÌ‰)( Ê·Ìœ ÅÌ«„ »⁄œ «“ Ê’· Œ«„Ê‘Ì »—«Ì „‘ —òÌ‰)( Ê·Ìœ ÅÌ«„ œ— ’Ê—  ⁄œ„ «⁄“«„ «òÌÅ »—«Ì Œ«„Ê‘Ì «⁄·«„ ‘œÂ  Ê”ÿ „‘ —ò)( Ê·Ìœ ÅÌ«„ ·€Ê Œ«„Ê‘Ì »«»—‰«„Â »Â „‘ —òÌ‰)'
			EXEC spCheckNewSMSEvent	--  Ê·Ìœ ÅÌ«„ Œ«„Ê‘ÌùÂ« »—«Ì „‘ —òÌ‰
			EXEC spCheckNewFaxEvent	--  Ê·Ìœ ›ò” Œ«„Ê‘Ì Â« »—«Ì „‘ —òÌ‰
			EXEC spCheckNewEmailEvent	--  Ê·Ìœ «Ì„Ì· Œ«„Ê‘Ì Â« »—«Ì „‘ —òÌ‰
			EXEC spCheckNewCallEvent	--  Ê·Ìœ  „«”  ·›‰Ì Œ«„Ê‘Ì Â« »—«Ì „‘ —òÌ‰
			EXEC spCheckNewSMSEventAfterConnect	--  Ê·Ìœ ÅÌ«„ »⁄œ «“ Ê’· Œ«„Ê‘Ì »—«Ì „‘ —òÌ‰
			EXEC spCheckSubscriberNotEkip --  Ê·Ìœ ÅÌ«„ œ— ’Ê—  ⁄œ„ «⁄“«„ «òÌÅ »—«Ì Œ«„Ê‘Ì «⁄·«„ ‘œÂ  Ê”ÿ „‘ —ò
			EXEC spCheckNewCancelSMSEvent --  Ê·Ìœ ÅÌ«„ ·€Ê Œ«„Ê‘Ì Â«Ì »«»—‰«„Â »Â „‘ —òÌ‰
		END
		
		IF (@lCounter % 119) = 0 
		BEGIN
			SET @lOperation = @lOperation + N'( Ê·Ìœ ÅÌ«„  —«‰” ”Ê“ÌùÂ«)'
			SET @lCnt = 1
			WHILE @lCnt > 0
			BEGIN
				EXEC @lCnt = spCheckNewTransFault	--  Ê·Ìœ ÅÌ«„  —«‰” ”Ê“ÌùÂ«
			END
		END

		IF (@lCounter % 31) = 0 
		BEGIN
			SET @lOperation = @lOperation + N'( Ê·Ìœ ÅÌ«„ Â‰ê«„ ÊÌ—«Ì‘ Œ«„Ê‘Ì »—ﬁœ«— ‘œÂ)'
			EXEC spCheckSMSAfterEdit	--  Ê·Ìœ ÅÌ«„ Â‰ê«„ ÊÌ—«Ì‘ Œ«„Ê‘Ì »—ﬁœ«— ‘œÂ
		END

		IF (@lCounter % 11) = 0 
		BEGIN
			DECLARE @lCnt2 as int = 1 
			SET @lCnt = 1
			DECLARE @IsSendSeparateMPFeederConnect AS bit = 0
			SELECT 
				@IsSendSeparateMPFeederConnect = CAST (ConfigValue AS bit) 
			FROM 
				Tbl_Config 
			WHERE 
				ConfigName = 'IsSendSeparateMPFeederConnect'

			WHILE @lCnt > 0 OR @lCnt2 > 0
			BEGIN
				IF @IsSendSeparateMPFeederConnect = 1 AND @lCnt2 = 1
				BEGIN
					SET @lOperation = @lOperation + N'( Ê·Ìœ ÅÌ«„ »⁄œ «“ Ê’· Â— ›Ìœ— œ— Œ«„Ê‘Ì Â«Ì ›Êﬁ  Ê“Ì⁄ »Â  ›òÌò ›Ìœ—)'
					EXEC @lCnt2 = spSendSMSFTMPFeederAfterConnect	--  Ê·Ìœ ÅÌ«„ »⁄œ «“ Ê’· Â— ›Ìœ— œ— Œ«„Ê‘Ì Â«Ì ›Êﬁ  Ê“Ì⁄ »Â  ›òÌò ›Ìœ—
				END
				ELSE
					SET @lCnt2 = 0
				
				SET @lOperation = @lOperation + N'( Ê·Ìœ ÅÌ«„ »⁄œ «“ Ê’· Œ«„Ê‘Ì)'
				EXEC @lCnt = spSendSMSDCManagerAfterConnect	--  Ê·Ìœ ÅÌ«„ »⁄œ «“ Ê’· Œ«„Ê‘Ì
			END
		END
		
		IF (@lCounter % 13) = 0 
		BEGIN
			SET @lOperation = @lOperation + N'( Ê·Ìœ ÅÌ«„ »Â «“«Ì Â— Ê’· «“ Ê’·Â«Ì ç‰œ „—Õ·Â «Ì)'
			EXEC spSendSMSDCManagerAfterMultistep	--  Ê·Ìœ ÅÌ«„ »Â «“«Ì Â— Ê’· «“ Ê’·Â«Ì ç‰œ „—Õ·Â «Ì
		END

		--- Write Cofing Counter
		IF @lConfigId IS NULL
			INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (107,N'smsCounter',Cast(@lCounter AS nvarchar),'Last Run: '+Cast(GETDATE() as nvarchar))
		ELSE
			UPDATE Tbl_Config 
			SET 
				ConfigValue = Cast(@lCounter AS nvarchar), 
				ConfigText = 'Last Run: ' + Cast(GETDATE() as nvarchar)
		WHERE ConfigName = 'smsCounter'
			
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SET @lErrCode = ERROR_STATE()
		SET @lErrMsg = ERROR_MESSAGE()
	END CATCH
	
	SELECT 
		@lOperation AS Opration,
		@lErrCode AS ResultCode, 
		@lErrMsg AS ResultMessage,
		@lCounter AS SMSCounter
GO
/*----------------*/

