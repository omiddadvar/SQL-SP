
ALTER PROCEDURE dbo.sp_Raya_RegOutage
	@First_name as nvarchar(50),
	@Last_name as nvarchar(50),
	@National_code as nvarchar(20),
	@Req_national_code as nvarchar(20),
	@Req_mobile as nvarchar(20),
	@Phone as nvarchar(20),
	@Mobile as nvarchar(20),
	@Bill_id as nvarchar(20),
	@Address as nvarchar(200),
	@CityId as int,
	@AreaId as int,
	@Comment as nvarchar(200),
	@RefertoId as int,
	@DepartmentId as int,
	@EnvironmentTypeId as int,
	@WorkingAreaUserId as int,
	@aCallId as nvarchar(450) = '',
	@IsVoltage as bit = 0,
	@IsLightRequest as bit = 0,
	@IsDamage as bit = 0,
	@DisconnectDate as varchar(10) = '',
	@DisconnectTime as varchar(5) = '',
	@IsTheft as bit = 0,
	@IsPeopleDamage as bit = 0,
	@ErjaComment as nvarchar(max) = '',
	@IsFrontage as bit = 0,
	@IsDanger as bit = 0,
	@IsGovernment as bit = 0,
	@IsPlanned as bit = 0,
	@GPSx as varchar(50) = '',
	@GPSy as varchar(50) = '',
	@FileServerId as bigint = -1,
	@FileName as nvarchar(200) = '',
	@FileDesc as nvarchar(2000) = '',
	@MediaTypeId as int = -1,
	@IsSingleSubscriber as bit = 0
AS
	DECLARE @RequestId as bigint
	DECLARE @InsertRequest varchar(20) = 'InsertRequest';  
	DECLARE @ErrorMessage as nvarchar(500) = ''

	set @FileServerId = NULLIF(@FileServerId,-1)

	set @aCallId = nullif(@aCallId,'')
	declare @lTrackingCode as bigint = -1
	create table #TblTrackingCode (TrackingCode bigint)

	declare @ExternalServiceId as int 
	declare @NetworkTypeId as int 
	declare @CallReasonId as int 
	declare @ErjaReasonId as int 
	

	if @AreaId = -1
		select top 1 @AreaId = AreaId, @CityId = CityId from Tbl_Area where IsCenter = 0

	SET @RefertoId = NULLIF(@RefertoId,-1)
	if @IsVoltage = 1 OR @IsDamage = 1 OR @IsTheft = 1 OR @IsPeopleDamage = 1 or @IsFrontage = 1 or @IsDanger = 1 or @IsGovernment = 1 or @IsPlanned = 1
	BEGIN
		if @IsVoltage = 1
		begin
			select @RefertoId = ConfigValue from Tbl_Config where ConfigName = 'PortalVoltageReferToId'
			set @ExternalServiceId = 3
		end
		else if @IsDamage = 1
		begin
			select @RefertoId = ConfigValue from Tbl_Config where ConfigName = 'PortalDamageReferToId'
			set @ExternalServiceId = 4
		end
		else if @IsTheft = 1
		begin
			select @RefertoId = ConfigValue from Tbl_Config where ConfigName = 'PortalTheftReferToId'
			set @ExternalServiceId = 5
		end
		else if @IsPeopleDamage = 1
		begin
			select @RefertoId = ConfigValue from Tbl_Config where ConfigName = 'PortalPeopleDamageReferToId'
			set @ExternalServiceId = 6
		end
		else if @IsFrontage = 1
		begin
			select @RefertoId = ConfigValue from Tbl_Config where ConfigName = 'PortalFrontageReferToId'
			set @ExternalServiceId = 7
		end
		else if @IsDanger = 1
		begin
			select @RefertoId = ConfigValue from Tbl_Config where ConfigName = 'PortalDangerReferToId'
			set @ExternalServiceId = 8
		end
		else if @IsGovernment = 1
		begin
			select @RefertoId = ConfigValue from Tbl_Config where ConfigName = 'PortalGovernmentReferToId'
			set @ExternalServiceId = 9
		end
		else if @IsPlanned = 1
		begin
			select @RefertoId = ConfigValue from Tbl_Config where ConfigName = 'PortalPlannedReferToId'
			set @ExternalServiceId = 10
		end
		
		declare @lData as nvarchar(1000)
		if not @ExternalServiceId is null
		begin
			select @lData = ExternalData from Tbl_ExternalService where ExternalServiceId = @ExternalServiceId
			if isnull(@lData,'') <> '' and CHARINDEX('{',@lData,0)>0
			begin
				select @RefertoId = StringValue from [dbo].[ParseJSON](@lData) where Name = 'refertoid'
				select @NetworkTypeId = StringValue from [dbo].[ParseJSON](@lData) where Name = 'networktypeid'
				select @ErjaReasonId = StringValue from [dbo].[ParseJSON](@lData) where Name = 'erjareasonid'
				select @CallReasonId = StringValue from [dbo].[ParseJSON](@lData) where Name = 'callreasonid'
			end
			else if isnull(@lData,'') <> ''
			begin
				select @RefertoId = ExternalData from Tbl_ExternalService where ExternalServiceId = @ExternalServiceId
			end
		end
			
		if @RefertoId IS NULL
			select top 1 @RefertoId = RefertoId from Tbl_ReferTo
	END
	else
	begin
		set @RefertoId = null
		set @ExternalServiceId = 1
	end
	
	set @NetworkTypeId = ISNULL(@NetworkTypeId,6)
		
	if @DepartmentId = -1
		select top 1 @DepartmentId = DepartmentId from Tbl_Department where IsActive = 1

	if @EnvironmentTypeId = -1
		set @EnvironmentTypeId = 1
		
	set @Address = nullif(@Address,'')
	set @Comment = nullif(@Comment,'')
	set @ErjaComment = nullif(@ErjaComment,'')

	if @Address is null
		set @Address = ISNULL(@Comment,N'À»  ‘œÂ «“ ÿ—Ìﬁ Ê» ”—ÊÌ”  Ê«‰Ì—')
		
	DECLARE @lGPSx as float = null
	DECLARE @lGPSy as float = null

	if ISNULL(@GPSx,'') <> ''
		SET @lGPSx = CAST(@GPSx as float)
		
	if ISNULL(@GPSy,'') <> ''
		SET @lGPSy = CAST(@GPSy as float)

	declare @lCallReasonId as int
	select top 1 @lCallReasonId = CallReasonId from Tbl_CallReason where CallReasonType = 1
	if @IsLightRequest = 1
		select top 1 @lCallReasonId = CallReasonId from Tbl_CallReason where CallReasonType = 2
	
	if not @CallReasonId is null
		set @lCallReasonId = @CallReasonId

	create table #TblRequest (TrackingCode bigint, ResultMessage nvarchar(500))
	BEGIN TRAN @InsertRequest

		if ISNULL(@aCallId,-1) > -1 and @aCallId <> ''
			insert into #TblTrackingCode EXEC spGetCallTrackingCode @aCallId
		else
			insert into #TblTrackingCode EXEC spGetNewCallTrackingCode
		
		select top 1 @lTrackingCode = TrackingCode from #TblTrackingCode
		drop table #TblTrackingCode
		
		declare @RequestNumber as bigint
		
		
		declare @lId as bigint
		INSERT INTO _tblRequest DEFAULT VALUES ;
		Select @lId = Id FROM _tblRequest
		WHERE Id = @@IDENTITY

		SET @RequestId = 100000000000000 * 99 + @lId

		UPDATE TblDepartmentInfo SET CurrentRequestAutoNumber = 0 WHERE CurrentRequestAutoNumber IS NULL; 
		UPDATE TblDepartmentInfo SET CurrentRequestAutoNumber = CurrentRequestAutoNumber + 1 ; 
		SELECT @RequestNumber = cast(Workingyear as varchar) + '99' + '2' + CAST(CurrentRequestAutoNumber as varchar) from TblDepartmentInfo
		
		declare @lDT as datetime = getdate()
		declare @lDatePersian as varchar(10) = dbo.mtosh(@lDT)
		declare @lTime as varchar(5) = dbo.gettime(@lDT)
		
		if @IsDamage = 1 OR @IsTheft = 1
		begin
			if @DisconnectDate <> ''
				SET @lDatePersian = @DisconnectDate
			if @DisconnectTime <> ''
				SET @lTime = @DisconnectTime
			
			set @lDT = dbo.shtom(@lDatePersian)
			
			declare @lDTStr as nvarchar(100)
			set @lDTStr = CONVERT(nvarchar(21),@lDT,23)
			
			set @lDT = CONVERT(datetime,@lDTStr + ' ' + @lTime + ':00',21)

		end
		
		declare @SubscriberName as nvarchar(50) = ''
		if ISNULL(@First_name,'')<>''
			set @SubscriberName = @First_name + ' '
		
		set @SubscriberName = @SubscriberName + ISNULL(@Last_name,'')
		
		if isnull(@SubscriberName,'') = ''
			set @SubscriberName = ' Ê«‰Ì—'

		declare @IsOnePhaseSingleSubscriber as bit = 1
		declare @lIsSingleSubscriber as bit
		set @lIsSingleSubscriber = @IsSingleSubscriber
		
		declare @lEndJobStateId as int = 4
		if @IsVoltage = 1 or @IsDamage = 1 OR @IsTheft = 1 or @IsPeopleDamage = 1 or @IsFrontage = 1 or @IsDanger = 1 or @IsGovernment = 1 or @IsPlanned = 1
		begin
			set @lEndJobStateId = 9
			set @IsOnePhaseSingleSubscriber = 0
			set @lIsSingleSubscriber = 0
		end
		
		if @IsLightRequest = 1
		begin
			set @IsOnePhaseSingleSubscriber = 0
			set @lIsSingleSubscriber = 0
			set @ExternalServiceId = 2
		end
		
		insert into TblRequest
			(RequestId, AreaUserId,DataEntrancePersonName,DisconnectDT,DisconnectDatePersian,DisconnectTime,SubscriberName,
			CityId,Address,Telephone,AreaId,Priority,IsSingleSubscriber,DisconnectInterval,DisconnectPower,
			DataEntryDT,DataEntryDTPersian,DataEntryTime,IsDuplicatedRequest,EndJobStateId,ReferToId,Comments,
			IsSentToRelatedArea,IsManoeuvred,TotalManoeuvreTime,CommentsDisconnect,RequestNumber,IsNotRelated,
			AccurateAddress,RequestDEInterval,
			IsLightRequest, IsLPRequest, IsMPRequest,IsTamir, ConnectDatePersian, ConnectTime,
			IsDisconnectMPFeeder, IsFogheToziRequest, CreateTimeInterval, ExtraComments, HasManoeuvre,  
			OverlapTime, IsLifeEvent, Mobile, DepartmentId, TrackingCode,CallReasonId, CallId,
			IsOnePhaseSingleSubscriber,GPSx, GPSy)
		Values
			(@RequestId, @WorkingAreaUserId, 1, @lDT, @lDatePersian, @lTime,@SubscriberName,
			@CityId, @Address, @Phone, @AreaId, 1, @lIsSingleSubscriber, 0, 0,
			@lDT, @lDatePersian, @lTime, 0, @lEndJobStateId, @RefertoId, LEFT(@Comment,199),
			0, 0, 0, '', @RequestNumber, 0,
			'', 0,
			@IsLightRequest, 0, 0, 0 , '', '',
			0, 0, 0, '', 0, 
			0, 0, @Mobile, @DepartmentId, @lTrackingCode,@lCallReasonId, @aCallId,
			@IsOnePhaseSingleSubscriber, @lGPSx, @lGPSy)
			
		
		declare @TableNameId as int
		select @TableNameId = TableNameId from Tbl_TableName where TableName = 'TblRequest'
			
		INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand, AreaUserId)
		VALUES ('TblRequest', @TableNameId, @RequestId, 2, @AreaId, 99, GETDATE(), NULL, @WorkingAreaUserId)

		
		insert into TblRequestInfo 
			(RequestId, SendTimeInterval, EnvironmentTypeId, BillingID, IsWatched, IsWebService)
		Values
			(@RequestId, 1, @EnvironmentTypeId, @Bill_id, 0, 1)
			
		select @TableNameId = TableNameId from Tbl_TableName where TableName = 'TblRequestInfo'
				
		INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand, AreaUserId)
		VALUES ('TblRequestInfo', @TableNameId, @RequestId, 2, @AreaId, 99, GETDATE(), NULL, @WorkingAreaUserId)
		
		if not @FileServerId is null and exists(select * from dbo.sysobjects where id = object_id(N'[dbo].[TblRequestFile]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
		begin
			
			if exists (select * from TblFileServer where FileServerId = @FileServerId)
			begin
				declare @lRequestFileId as bigint
				INSERT INTO _tblBaseTablesLongId DEFAULT VALUES ;
				Select @lId = Id FROM _tblBaseTablesLongId
				WHERE Id = @@IDENTITY
				
				SET @lRequestFileId = 100000000000000 * 99 + @lId
				
				set @FileName = NULLIF(@FileName,'')
				set @FileDesc = NULLIF(@FileDesc,'')
				
				insert into TblRequestFile 
					(RequestFileId, RequestId, FileServerId, Subject, Comment)
				Values
					(@lRequestFileId, @RequestId, @FileServerId, ISNULL(@FileDesc,@Comment), ISNULL(@FileName,@SubscriberName))
					
				select @TableNameId = TableNameId from Tbl_TableName where TableName = 'TblRequestFile'
						
				INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand, AreaUserId)
				VALUES ('TblRequestFile', @TableNameId, @lRequestFileId, 2, @AreaId, 99, GETDATE(), NULL, @WorkingAreaUserId)
			end
		end
		
		if exists(select * from dbo.sysobjects where id = object_id(N'[dbo].[TblRequestData]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
		begin
			insert into TblRequestData
				(RequestId, MediaTypeId, ExternalServiceId)
			values
				(@RequestId, @MediaTypeId, @ExternalServiceId)
				
			select @TableNameId = TableNameId from Tbl_TableName where TableName = 'TblRequestData'
					
			INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand, AreaUserId)
			VALUES ('TblRequestData', @TableNameId, @RequestId, 2, @AreaId, 99, GETDATE(), NULL, @WorkingAreaUserId)
		end
		
		if @IsVoltage = 1 or @IsDamage = 1 OR @IsTheft = 1 or @IsPeopleDamage = 1 or @IsFrontage = 1 or @IsDanger = 1 or @IsGovernment = 1 or @IsPlanned = 1
		BEGIN
			declare @ErjaRequestId as bigint
			if exists(select * from dbo.sysobjects where id = object_id(N'[dbo].[_tblErja]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
			begin
				INSERT INTO _tblErja DEFAULT VALUES ;
				Select @lId = Id FROM _tblErja
				WHERE Id = @@IDENTITY
			end
			else
			begin
				INSERT INTO _tblBaseTablesLongId DEFAULT VALUES ;
				Select @lId = Id FROM _tblBaseTablesLongId
				WHERE Id = @@IDENTITY
			end
			
			SET @ErjaRequestId = 100000000000000 * 99 + @lId
			
			insert into TblErjaRequest 
					(ErjaRequestId, ReferToId, IsWatched, ErjaDT, ErjaDatePersian, ErjaTime,
					AreaUserId, ErjaStateId, CreateAreaUserId, RequestId, Comments, NetworkTypeId,
					GPSx, GPSy, ErjaReasonId)
				VALUES(@ErjaRequestId, @ReferToId,0 , @lDT, @lDatePersian, @lTime,
					@WorkingAreaUserId, 1, @WorkingAreaUserId, @RequestId, @ErjaComment, @NetworkTypeId,
					@lGPSx, @lGPSy, @ErjaReasonId)
				
			select @TableNameId = TableNameId from Tbl_TableName where TableName = 'TblErjaRequest'
					
			INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand, AreaUserId)
			VALUES ('TblErjaRequest', @TableNameId, @ErjaRequestId, 2, @AreaId, 99, GETDATE(), NULL, @WorkingAreaUserId)
		END

	IF @@ERROR = 1
	BEGIN
		SET @ErrorMessage = ERROR_MESSAGE()
		ROLLBACK TRAN @InsertRequest
		SET @lTrackingCode = -1
	END
	ELSE
	BEGIN
		COMMIT TRAN @InsertRequest
	END
  /*------------------------------------<SMS>------------------------------------*/
  /*Execute spCreateSMS and trap it here with #tmpDummy in order to not return any SMSId*/
  CREATE TABLE #tmpDummy ( SMSId BIGINT )
  INSERT INTO #tmpDummy EXEC spSendSMSOutageSubscriber @RequestNumber , @Mobile
  DROP TABLE #tmpDummy
  /*-----------------------------------</SMS>-----------------------------------*/
	SELECT @lTrackingCode AS TrackingCode, @ErrorMessage AS ResultMessage
GO