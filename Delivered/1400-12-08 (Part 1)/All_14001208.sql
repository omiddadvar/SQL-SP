ALTER PROCEDURE spDisHourly_Daily(
  @aAreaIDs AS VARCHAR(100),
  @aDatePersian AS VARCHAR(10),
  @aDate AS VARCHAR(10),
  @aIsLPReq AS BIT = 1,
  @aIsMPReq AS BIT = 1,
  @aIsFoghTReq AS BIT = 1,
  @aMPPostIDs AS VARCHAR(MAX),
  @aMPFeederIDs AS VARCHAR(MAX),
  @aIsTamir AS INT, /*Ranges through -1,0,1 (-1 : Both)*/
  @aIsWithFT AS INT /*Ranges through -1,0,1 (-1 : All)*/
  ) AS
  BEGIN
    DECLARE @lsql AS NVARCHAR(MAX) = ''
      ,@lArea AS NVARCHAR(50)
      ,@lAreaGroupBy AS VARCHAR(50)
      ,@lMPPostCondition AS VARCHAR(MAX)
      ,@lMPFeederCondition AS VARCHAR(MAX)
      ,@lTamirCondition AS VARCHAR(MAX)
      ,@lFTCondition AS VARCHAR(MAX)
      ,@lHour1 AS VARCHAR(8), @lHour2 AS VARCHAR(8)
      ,@lSumNTamir AS FLOAT,@lSumTamir1 AS FLOAT,@lSumTamir2 AS FLOAT,@lSumTamir3 AS FLOAT
      ,@lDT1 AS DATETIME, @lDT2 AS DATETIME
      ,@i AS INT = 0;
    /*--Define Tables--*/
    CREATE TABLE #tmpData (Area NVARCHAR(50),Hour INT ,HourFrom VARCHAR(8) ,HourTo VARCHAR(8), 
      cntNotTamir FLOAT, cntTamir1 FLOAT,cntTamir2 FLOAT, cntTamir3 FLOAT)
    CREATE TABLE #tmpNet (IsLP BIT,IsMP Bit ,IsFT BIT)
    CREATE TABLE #tmpArea (AreaId INT , Area NVARCHAR(50))
    /*--Initialize Tables--*/
    IF @aIsMPReq = 1 AND @aIsFoghTReq = 1 AND @aIsLPReq = 1 BEGIN  
    	SET @aIsFoghTReq = NULL
      SET @aIsMPReq = NULL
      SET @aIsLPReq = Null
    END
    INSERT #tmpNet SELECT @aIsLPReq, @aIsMPReq, @aIsFoghTReq
  	SET @lsql = ' SELECT AreaId, Area FROM tbl_Area '
    IF @aAreaIDs <> ''
    	SET  @lsql += ' WHERE AreaId IN (' + @aAreaIDs + ')'
   INSERT #tmpArea EXEC (@lsql)
   /*--Fill Main DataTable--*/
   SET @lMPPostCondition = CASE WHEN @aMPPostIDs = '' THEN '' ELSE ' AND MPR.MPPostId IN ('+ @aMPPostIDs +')' END
   SET @lMPFeederCondition = CASE WHEN @aMPFeederIDs = '' THEN '' ELSE ' AND MPR.MPFeederId IN ('+ @aMPFeederIDs +')' END
   SET @lTamirCondition = CASE WHEN @aIsTamir = -1 THEN '' ELSE ' AND t.IsTamir = ' + CAST(@aIsTamir AS VARCHAR(2)) + '' END
   SET @lFTCondition = '((MPR.DisconnectReasonId IS NULL
      OR MPR.DisconnectReasonId < 1200 OR MPR.DisconnectReasonId > 1299 ) 
      AND ( MPR.DisconnectGroupSetId IS NULL OR MPR.DisconnectGroupSetId <> 1129 
      AND MPR.DisconnectGroupSetId <> 1130))'
   SET @lFTCondition = CASE 
      WHEN @aIsWithFT = 1 THEN ' AND NOT ' + @lFTCondition
      WHEN @aIsWithFT = 0 THEN ' AND ' + @lFTCondition ELSE '' END
   BEGIN TRY
      WHILE(@i < 24)
      BEGIN
        SET @lHour1 = CONVERT(VARCHAR(8), @i) + ':00' 
        SET @lHour2 = CONVERT(VARCHAR(8), @i + 1) + ':00'
        IF @i = 23 BEGIN SET @lHour2 = '23:59' END
        SET @lDT1 = CONVERT(DATETIME, @aDate +' '+ @lHour1 , 102)
        SET @lDT2 = CONVERT(DATETIME, @aDate +' '+ @lHour2 , 102)
        SET @lAreaGroupBy = CASE WHEN (CHARINDEX(',',@aAreaIDs) > 0) THEN '' ELSE ' GROUP BY a.Area' END
        SET @lArea = CASE WHEN CHARINDEX(',',@aAreaIDs) > 0 THEN '''Â„Â ‰Ê«ÕÌ''' ELSE 'a.Area' END
        SET @lsql = 'SELECT '+ @lArea + ',' + CAST(@i AS VARCHAR(2))
            + ',''' + @lHour1 +''','''+ @lHour2 +''',
           SUM(CASE WHEN t.IsTamir = 0 THEN dbo.MinuteCount('''
            + CAST(@lDT1 AS VARCHAR(50))+''','''+ CAST(@lDT2 AS VARCHAR(50))
              +''', t.DisconnectDT,t.ConnectDT) ELSE 0 END) AS sumNotTamir,
           SUM(CASE WHEN t.IsTamir = 1 AND t.TamirTypeId = 1 THEN dbo.MinuteCount('''
            + CAST(@lDT1 AS VARCHAR(50))+''','''+ CAST(@lDT2 AS VARCHAR(50))
              +''', t.DisconnectDT,t.ConnectDT) ELSE 0 END) AS sumTamir1,
           SUM(CASE WHEN t.IsTamir = 1 AND t.TamirTypeId = 2 THEN dbo.MinuteCount('''
            + CAST(@lDT1 AS VARCHAR(50))+''','''+ CAST(@lDT2 AS VARCHAR(50))
              +''',t.DisconnectDT,t.ConnectDT) ELSE 0 END) AS sumTamir2,
           SUM(CASE WHEN t.IsTamir = 1 AND t.TamirTypeId = 3 THEN dbo.MinuteCount('''
            + CAST(@lDT1 AS VARCHAR(50))+''','''+ CAST(@lDT2 AS VARCHAR(50))
              +''',t.DisconnectDT,t.ConnectDT) ELSE 0 END) AS sumTamir3
          FROM TblRequest t
          INNER JOIN #tmpArea a ON a.AreaId = t.AreaId
          LEFT JOIN TblMPRequest MPR ON t.MPRequestId = MPR.MPRequestId
          WHERE t.DisconnectInterval > 0 
                AND t.ConnectDT IS NOT NULL
                AND t.DisconnectDatePersian = '''+ @aDatePersian +'''
                AND EXISTS (SELECT 1 FROM #tmpNet n WHERE (n.IsFT = t.IsFogheToziRequest 
                      AND n.IsMP = t.IsMPRequest
                      AND n.IsLP = t.IsLPRequest)
                      OR n.IsFT IS NULL )'
                + @lMPPostCondition + @lMPFeederCondition + @lTamirCondition + @lFTCondition
                + @lAreaGroupBy
        INSERT #tmpData EXEC(@lsql)
        SET @i = @i + 1
      END
      /*-----------------<Calculate Sum>-------------*/
      IF (SELECT COUNT(*) FROM #tmpData) > 0 BEGIN  
        SELECT TOP(24) @lArea = Area, @lSumNTamir = SUM(cntNotTamir), @lSumTamir1 = SUM(cntTamir1) ,
          @lSumTamir2 = SUM(cntTamir2), @lSumTamir3 = SUM(cntTamir3)
          FROM #tmpData
          GROUP BY Area
        INSERT #tmpData SELECT @lArea , 24 , '00:00','23:59', @lSumNTamir, @lSumTamir1 , @lSumTamir2, @lSumTamir3
      END
      /*-----------------</Calculate Sum>------------*/
      SELECT * FROM #tmpData
      DROP TABLE #tmpData
      DROP TABLE #tmpArea
      DROP TABLE  #tmpNet
    END TRY  
    BEGIN CATCH  
      DECLARE @ErrMsg AS VARCHAR(MAX)
          ,@ErrState AS INT
          ,@ErrSeverity AS INT
      DROP TABLE #tmpData
      DROP TABLE #tmpArea
      DROP TABLE  #tmpNet
      SELECT @ErrMsg = ERROR_MESSAGE() , @ErrState = ERROR_STATE() , @ErrSeverity = ERROR_SEVERITY()
      RAISERROR (@ErrMsg , @ErrSeverity , @ErrState);
    END CATCH
END
GO

CREATE PROCEDURE spGetDuplicateRequestById 
  @aReqId BIGINT
  AS 
  BEGIN
    DECLARE @lIsDuplicate AS BIT
          , @lDuplicateReqId AS BIGINT

  SELECT @lIsDuplicate = IsDuplicatedRequest , @lDuplicateReqId = DuplicatedRequestId
    FROM TblRequest WHERE RequestId = @aReqId
  
  SET @aReqId = CASE WHEN @lIsDuplicate = 1 THEN @lDuplicateReqId ELSE @aReqId END
  
  SELECT R.RequestId , R.RequestNumber, R.Address ,E.EndJobState
    , R.IsDuplicatedRequest AS IsDuplicate
    ,U.FirstName +' '+ U.LastName AS Operator
    , ISNULL(UR.FirstName, '' ) +' '+ ISNULL(UR.LastName, '' )AS DuplicateOperator
    , R.DisconnectDatePersian +' '+ R.DisconnectTime AS DisconnectDT
    , R.ConnectDatePersian +' '+ R.ConnectTime AS ConnectDT
    , R.DataEntryDTPersian +' '+ R.DataEntryTime AS DataEntryDT
    FROM TblRequest R
    INNER JOIN Tbl_AreaUser U ON R.AreaUserId = U.AreaUserId
    LEFT JOIN Tbl_AreaUser UR ON R.DuplicatedRequestId = UR.AreaUserId
    INNER JOIN Tbl_EndJobState E ON R.EndJobStateId = E.EndJobStateId
    WHERE R.RequestId = @aReqId
      OR (R.DuplicatedRequestId = @aReqId AND R.IsDuplicatedRequest = 1)
    ORDER BY R.IsDuplicatedRequest , R.DisconnectDT
  END
GO

CREATE PROCEDURE spReportHourlyDisPowerGeneral
    @aFromDate VARCHAR(10),
    @aToDate VARCHAR(10)
  AS 
  BEGIN
    DECLARE @DTStart AS DATETIME
    DECLARE @DTEnd AS DATETIME
    SELECT @DTStart = dbo.shtom(@aFromDate)
    SELECT @DTEnd = dbo.shtom(@aToDate)
    CREATE TABLE #tmpDaily(
    	[Province] NVARCHAR(100) NULL,
    	[Hour00] FLOAT NULL,
    	[Hour01] FLOAT NULL,
    	[Hour02] FLOAT NULL,
    	[Hour03] FLOAT NULL,
    	[Hour04] FLOAT NULL,
    	[Hour05] FLOAT NULL,
    	[Hour0Ì] FLOAT NULL,
    	[Hour07] FLOAT NULL,
    	[Hour08] FLOAT NULL,
    	[Hour09] FLOAT NULL,
    	[Hour10] FLOAT NULL,
    	[Hour11] FLOAT NULL,
    	[Hour12] FLOAT NULL,
    	[Hour13] FLOAT NULL,
    	[Hour14] FLOAT NULL,
    	[Hour15] FLOAT NULL,
    	[Hour1Ì] FLOAT NULL,
    	[Hour17] FLOAT NULL,
    	[Hour18] FLOAT NULL,
    	[Hour19] FLOAT NULL,
    	[Hour20] FLOAT NULL,
    	[Hour21] FLOAT NULL,
    	[Hour22] FLOAT NULL,
    	[Hour23] FLOAT NULL
    ) ON [PRIMARY]
    CREATE TABLE #tmpTotal(
    	[Province] NVARCHAR(100) NULL,
    	[DatePersian] VARCHAR(10) NULL,
    	[Hour00] FLOAT NULL,
    	[Hour01] FLOAT NULL,
    	[Hour02] FLOAT NULL,
    	[Hour03] FLOAT NULL,
    	[Hour04] FLOAT NULL,
    	[Hour05] FLOAT NULL,
    	[Hour0Ì] FLOAT NULL,
    	[Hour07] FLOAT NULL,
    	[Hour08] FLOAT NULL,
    	[Hour09] FLOAT NULL,
    	[Hour10] FLOAT NULL,
    	[Hour11] FLOAT NULL,
    	[Hour12] FLOAT NULL,
    	[Hour13] FLOAT NULL,
    	[Hour14] FLOAT NULL,
    	[Hour15] FLOAT NULL,
    	[Hour1Ì] FLOAT NULL,
    	[Hour17] FLOAT NULL,
    	[Hour18] FLOAT NULL,
    	[Hour19] FLOAT NULL,
    	[Hour20] FLOAT NULL,
    	[Hour21] FLOAT NULL,
    	[Hour22] FLOAT NULL,
    	[Hour23] FLOAT NULL
    ) ON [PRIMARY]
    DECLARE @DT AS DATETIME = @DTStart
    WHILE @DT <= @DTEnd
    BEGIN
    	DECLARE @CurrentDate AS VARCHAR(10)
    	SELECT @CurrentDate = dbo.mtosh(@DT)
    	INSERT INTO #tmpDaily 	
    	  EXEC dbo.spReportHourlyDisconnectPower @CurrentDate
    	INSERT INTO #tmpTotal (Province, DatePersian, Hour00, Hour01, Hour02, Hour03, Hour04, Hour05
          , Hour0Ì, Hour07, Hour08, Hour09, Hour10, Hour11, Hour12, Hour13, Hour14, Hour15, Hour1Ì, Hour17
          , Hour18, Hour19, Hour20, Hour21, Hour22, Hour23)
    	  SELECT Province , @CurrentDate , ISNULL(Hour00,0), ISNULL(Hour01,0), ISNULL(Hour02,0), ISNULL(Hour03,0), ISNULL(Hour04,0), ISNULL(Hour05,0)
          , ISNULL(Hour0Ì ,0), ISNULL(Hour07,0), ISNULL(Hour08,0), ISNULL(Hour09,0), ISNULL(Hour10,0), ISNULL(Hour11,0), ISNULL(Hour12,0)
          , ISNULL(Hour13,0), ISNULL(Hour14,0), ISNULL(Hour15,0), ISNULL(Hour1Ì,0), ISNULL(Hour17,0)
          , ISNULL(Hour18,0), ISNULL(Hour19,0), ISNULL(Hour20,0), ISNULL(Hour21,0), ISNULL(Hour22,0), ISNULL(Hour23,0)
        FROM #tmpDaily
    	DELETE #tmpDaily
    	SET @DT = DATEADD(day, 1, @DT)
    END
    SELECT * FROM #tmpTotal
    DROP TABLE	#tmpDaily 
    DROP TABLE	#tmpTotal
  END
GO


CREATE PROCEDURE spPrepareNotiMessage @aReqId BIGINT
AS
  BEGIN
    DECLARE @lText AS NVARCHAR(MAX) = ''
      ,@lRaw AS NVARCHAR(MAX) = ''
      ,@lSubject AS NVARCHAR(1000) = 'Default-Value'
      ,@lDuration AS INT
      ,@lDisconnectDatePersian AS VARCHAR(10)
      ,@lDisconnectTime AS VARCHAR(5)
      ,@lIsOk AS BIT = CAST(0 AS BIT)
  	
    SELECT @lText = ConfigText FROM Tbl_Config WHERE ConfigName = 'NotifBaBarname'
    IF @lText IS NULL OR @lText = '' 
      BEGIN
        RAISERROR('NotifBaBarname Is Empty - Tbl_Config' , 16 , 1); 	
      END
    SELECT * INTO #tmp FROM TblRequest WHERE RequestId = @aReqId
    
    SET @lIsOk = CAST(CASE WHEN (SELECT COUNT(*) FROM #tmp) > 0 THEN 1 ELSE 0 END AS BIT)
    
    SELECT @lDuration =  DATEDIFF(MINUTE, TamirDisconnectFromDT , TamirDisconnectToDT)
      , @lDisconnectDatePersian = DisconnectDatePersian 
      , @lDisconnectTime = DisconnectTime
       FROM #tmp

    SELECT @lSubject = Sub.TamirRequestSubject
      FROM #tmp R
      INNER JOIN TblTamirRequestConfirm C ON R.RequestId = C.RequestId
      INNER JOIN TblTamirRequest TR ON C.TamirRequestId = TR.TamirRequestId 
      INNER JOIN Tbl_TamirRequestSubject Sub ON TR.TamirRequestSubjectId = Sub.TamirRequestSubjectId
      WHERE R.RequestId = @aReqId
    SET @lText = REPLACE(@lText , 'Ì' , 'Ì')
    SET @lSubject = REPLACE(@lSubject , 'Ì' , 'Ì')
    SET @lText = REPLACE(@lText , 'Probably' , CASE WHEN CHARINDEX('„œÌ—Ì  «÷ÿ—«—Ì »«—', @lSubject) > 0 THEN N'«Õ „«·«' ELSE '' END)
    SET @lText = REPLACE(@lText , 'DisconnectDatePersian' , @lDisconnectDatePersian)
    SET @lText = REPLACE(@lText , 'DisconnectTime' , @lDisconnectTime)
    SET @lText = REPLACE(@lText , 'SS' , CAST(@lDuration AS VARCHAR(4)))
    SET @lText = REPLACE(@lText , 'Subject' , @lSubject)
    
    SELECT @lIsOk AS IsOk, @lSubject AS Subject, @lText AS Text
    DROP TABLE #tmp
  END
GO

/*-------? Web Services-------? Web Services-------? Web Services-------? Web Services-------*/



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


CREATE PROCEDURE spSendSMSSubscriberNewRequest
  @aRequestNumber AS BIGINT,
  @aMobile AS NVARCHAR(20)
  AS
  BEGIN
    DECLARE @lBody AS NVARCHAR(2000)= N''
           ,@lProvince AS NVARCHAR(100)
           ,@lDesc AS NVARCHAR(50) = N''
    BEGIN TRY
      SELECT @lBody = ConfigText FROM Tbl_Config WHERE ConfigName = 'SMSSubscriberNewRequest'
      SELECT @lProvince = ConfigValue FROM Tbl_Config WHERE ConfigName = 'ToziName'
      
      SELECT TOP(1) RequestNumber , CAST(TrackingCode AS VARCHAR(20)) AS TrackingCode,
          Address , DataEntryDTPersian ,DataEntryTime , DisconnectDatePersian , DisconnectTime
        INTO #tmp
        FROM TblRequest
        WHERE RequestNumber = @aRequestNumber

      SET @lBody = REPLACE(@lBody , 'DataEntryDate' , ISNULL((SELECT TOP(1) DataEntryDTPersian FROM #tmp) , '____/__/__'))
      SET @lBody = REPLACE(@lBody , 'DataEntryTime' , ISNULL((SELECT TOP(1) DataEntryTime FROM #tmp) , '__:__'))
      SET @lBody = REPLACE(@lBody , 'DisconnectDate' , ISNULL((SELECT TOP(1) DisconnectDatePersian FROM #tmp) , '____/__/__'))
      SET @lBody = REPLACE(@lBody , 'DisconnectTime' , ISNULL((SELECT TOP(1) DisconnectTime FROM #tmp) , '__:__'))
      SET @lBody = REPLACE(@lBody , 'Address' , ISNULL((SELECT TOP(1) Address FROM #tmp) , '____'))
      SET @lBody = REPLACE(@lBody , 'Province' , ISNULL(@lProvince , '__'))
      SET @lBody = REPLACE(@lBody , 'RequestNumber' , ISNULL((SELECT TOP(1) RequestNumber FROM #tmp) , '__'))
      SET @lBody = REPLACE(@lBody , 'TrackingCode' , ISNULL((SELECT TOP(1) TrackingCode FROM #tmp) , '__'))
      
      SET @lDesc = 'SMSSubscriberNewRequest : ' + ISNULL(CAST(@aRequestNumber AS VARCHAR(20)) , '#RequestNumber#')
      DROP TABLE #tmp
      
      EXEC spCreateSMS @lBody ,@aMobile ,@lDesc , NULL
    END TRY
    BEGIN CATCH
    END CATCH
  END
Go

