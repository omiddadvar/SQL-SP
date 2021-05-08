
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
	DECLARE @MPPostName AS nvarchar(200)
	DECLARE @MPFeederName AS nvarchar(200)
	DECLARE @FogheToziPostName AS nvarchar(200)
	DECLARE @LPPostName AS nvarchar(200)
	DECLARE @LPPostCode AS nvarchar(200)
	DECLARE @LPFeederName AS nvarchar(200)
	DECLARE @DisconnectPowerMW AS nvarchar(200)
	DECLARE @Reason AS nvarchar(200)
	DECLARE @DisconnectPower AS nvarchar(200)
	DECLARE @DTTime AS nvarchar(200)
	DECLARE @DTDate AS nvarchar(200)
	DECLARE @Area AS nvarchar(200)
	DECLARE @FogheToziType AS nvarchar(200)
	DECLARE @FogheToziGroup AS nvarchar(50)
	DECLARE @FogheToziShortText AS nvarchar(100)
	DECLARE @IsTotalLPPostDisconnected AS bit
	DECLARE @IsNotDisconnectFeeder AS bit
	DECLARE @IsDisconnectMPFeeder AS bit
	DECLARE @MPStatus AS nvarchar(200) 
	DECLARE @CurrentValue AS nvarchar(200)
	DECLARE @Address AS nvarchar(200)
	DECLARE @FogheToziDisconnectId AS int
	DECLARE @FogheToziFeederName AS nvarchar(200)
	DECLARE @TamirStr AS nvarchar(10)
	DECLARE @FogheToziTypeId AS int
	DECLARE @ZoneName AS nvarchar(100)

	DECLARE @DCGroup AS nvarchar(100)
	DECLARE @DCGroupSet AS nvarchar(100)
	
	DECLARE @FromPathType As nvarchar(50)
	DECLARE @FromPathTypeValue As nvarchar(50)
	DECLARE @ToPathType As nvarchar(50)
	DECLARE @ToPathTypeValue As nvarchar(50)
	DECLARE @FeederPart As nvarchar(100)
	
	DECLARE @IsSingleSubscriberShow AS bit
	DECLARE @SubscriberType AS nvarchar(10)
	
	DECLARE @OCEFRelayAction AS nvarchar(200)
	DECLARE @TamirNetworkTypeId AS int
	
	DECLARE @RequestAreaId AS int
	DECLARE @LastUpdate AS int
	
	DECLARE @CurrentDate AS varchar(10)
	DECLARE @CurrentMonthDay AS varchar(5)
	SELECT @CurrentDate = dbo.mtosh(getdate())
	SET @CurrentMonthDay = RIGHT(@CurrentDate,5)
	
	SET @SubscriberType = N'کلي'
	SET @IsSingleSubscriberShow = 0
	SELECT @IsSingleSubscriberShow = CASE WHEN ISNULL(ConfigValue,'') = 'True' THEN 1 ELSE 0 END
	FROM Tbl_Config WHERE ConfigName = 'SMSSingleSubs'
	
	SET @FromPathType = ''
	SET @FromPathTypeValue = ''
	SET @ToPathType = ''
	SET @ToPathTypeValue = ''
	SET @FeederPart = ''
	SET @TamirStr = N'؟'
	SET @FogheToziTypeId = -1
	--declare @dt as DateTime=dateadd(day,-1,getdate())
	declare @dt as DateTime=dateadd(hour,-12,getdate())
	--print 'spSendSMSDCManager start' + convert(nvarchar(100),getdate(),21)
	
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
	WHERE 
		tblRequest.DisconnectDT >=@dt and 
		( 
			(TblRequest.IsSingleSubscriber = 0 OR TblRequest.IsSingleSubscriber IS NULL OR (TblRequest.IsSingleSubscriber = 1 AND @IsSingleSubscriberShow = 1)) 
			OR TblRequest.IsMPRequest = 1 OR TblRequest.IsFogheToziRequest = 1
		) 
		AND NOT TblRequest.RequestId IN 
		( 
			SELECT RequestId 
			FROM TblManagerSMSDCSended 
			WHERE 
				NOT RequestId IS NULL 
				AND ManagerSMSDCId =  @ManagerSMSDCId 
				AND (ISNULL(RequestTypeId,@RequestTypeId) > 0 OR (@RequestTypeId = 0 AND ISNULL(RequestTypeId,@RequestTypeId) = 0))
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
			
			OR (@RequestTypeId = 1 AND @IsTamir = 0 AND IsFogheToziRequest = 1)

			OR ((@RequestTypeId = 2 OR (@RequestTypeId = 6 AND ISNULL(TblRequest.IsSingleSubscriber,0) = 0) OR (@RequestTypeId = 7 AND TblRequest.IsSingleSubscriber = 1)) AND IsLPRequest = 1 AND ISNULL(ISNULL(TblTamirRequestEmergency.TamirNetworkTypeId, TblTamirRequest.TamirNetworkTypeId),3) = 3 AND IsTamir=1 AND TblRequest.EndJobStateId <> 4)
				
			OR ((@RequestTypeId = 3 OR (@RequestTypeId = 8 AND ISNULL(TblRequest.IsSingleSubscriber,0) = 0) OR (@RequestTypeId = 9 AND TblRequest.IsSingleSubscriber = 1)) AND IsLPRequest = 1 AND IsTamir=0)
			
			OR ((@RequestTypeId = 4 OR (@RequestTypeId = 10 AND ISNULL(TblMPRequest.IsTotalLPPostDisconnected,0) = 0) OR (@RequestTypeId = 11 AND TblMPRequest.IsTotalLPPostDisconnected = 1)) AND IsMPRequest = 1 AND ISNULL(ISNULL(TblTamirRequestEmergency.TamirNetworkTypeId, TblTamirRequest.TamirNetworkTypeId),2) = 2 AND IsTamir=1 AND TblRequest.EndJobStateId <> 4)

			OR ((@RequestTypeId = 5 OR (@RequestTypeId = 12 AND ISNULL(TblMPRequest.IsTotalLPPostDisconnected,0) = 0) OR (@RequestTypeId = 13 AND TblMPRequest.IsTotalLPPostDisconnected = 1)) AND IsMPRequest = 1 AND IsTamir=0)
			
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
	if ISNULL(@RequestId,0) <=0
		return 0;
	SELECT TOP 1 
		@RequestId = TblRequest.RequestId, 
		@RequestNumber = TblRequest.RequestNumber, 
		@TamirStr = CASE WHEN TblRequest.IsTamir = 1 THEN N'بابرنامه' ELSE N'بي برنامه' END, 
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
		@DisconnectPowerMW = Cast(ISNULL(TblFogheToziDisconnect.DisconnectPowerMW, (sqrt(3)* 20000 * TblFogheToziDisconnect.CurrentValue* 0.85 / 1000000)) AS DECIMAL(9, 2)),
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
		@SubscriberType = CASE WHEN ISNULL(TblLPRequest.IsSingleSubscriber, TblRequest.IsSingleSubscriber) = 1 THEN N'تکي' ELSE N'کلي' END,
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
		
	--print 'spSendSMSDCManager Step2 ' + convert(nvarchar(100),getdate(),21)
	IF( @RequestId IS NULL )
		return 0;
	
	
	DECLARE @ServiceTimeOut as int
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
		SET @MPStatus = N'قطع پست توزيع'
	IF( @IsNotDisconnectFeeder = 1 )
		SET @MPStatus = N'قطعي در سر خط'
	IF( @IsDisconnectMPFeeder = 1 )
		SET @MPStatus = N'قطع کامل فيدر'
		
	IF @RequestTypeId = 1 -- FogheTozi
	BEGIN
		SET @FogheToziGroup = CASE
			WHEN @FogheToziTypeId = 1 THEN N'کمبود توليد'
			WHEN @FogheToziTypeId IN (2,3,4,5) THEN N'انتقال'
			WHEN @FogheToziTypeId IN (6,7,8,9) THEN N'فوق توزيع'
			ELSE N''
		END
		SET @FogheToziShortText = CASE
			WHEN @FogheToziTypeId = 1 THEN N'کمبود توليد'
			WHEN @FogheToziTypeId IN (2,3,4) THEN N'انتقال-بابرنامه'
			WHEN @FogheToziTypeId = 5 THEN N'انتقال-بي برنامه'
			WHEN @FogheToziTypeId IN (6,7,8) THEN N'فوق توزيع-بابرنامه'
			WHEN @FogheToziTypeId = 9 THEN N'فوق توزيع-بي برنامه'
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
	
	/*----- ليست وصل هاي چند مرحله اي ------*/
	DECLARE @MultiStepConnections AS VARCHAR(2000)
	DECLARE @NotComplate AS VARCHAR(20)
	SET @MultiStepConnections = ''
	SET @NotComplate = ''
	EXEC @MultiStepConnections = dbo.GetMultiStepConnections @RequestId
	IF LEN(@MultiStepConnections) > 1
		SET @NotComplate = ' به طور کامل'

	/*------------*/
	
	DECLARE @RequestType AS varchar(50)
	DECLARE @SMS AS nvarchar(2000)
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
		DECLARE @FTFName AS nvarchar(200)
		DECLARE @CurrentValueMW AS float
		DECLARE crFTFeeders CURSOR FOR
			SELECT 
				TblFogheToziDisconnectMPFeeder.FogheToziDisconnectMPFeederId,
				Tbl_MPFeeder.MPFeederName,
				Cast((sqrt(3) * ISNULL(Tbl_MPFeeder.Voltage,20000) * TblFogheToziDisconnectMPFeeder.CurrentValue * isnull(Tbl_MPFeeder.CosinPhi, 0.85) / 1000000) AS DECIMAL(9, 2)) AS CurrentValueMW
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
	
	IF @SMS = '' OR @SMS IS NULL RETURN 0;
	
	INSERT INTO TblManagerSMSDCSended 
		(ManagerSMSDCId, RequestId, IsSendSMSAfterConnect, RequestTypeId) 
		VALUES (@ManagerSMSDCId, @RequestId, @IsSendSMSAfterConnect ^ 1, @RequestTypeId)
	
	SELECT TOP 0 * FROM TblManagerSMSDCSended
	
	SET @SMS = Replace( @SMS,'ReqNo', ISNULL(@RequestNumber,0))
	SET @SMS = Replace( @SMS,'Minutes', ISNULL(@DBMinutes,0))
	SET @SMS = Replace( @SMS,'FogheToziPostName', ISNULL(@FogheToziPostName,N'؟'))
	SET @SMS = Replace( @SMS,'MPPostName', ISNULL(@MPPostName,N'؟'))
	SET @SMS = Replace( @SMS,'Area', ISNULL(@Area,N'؟'))
	SET @SMS = Replace( @SMS,'MPFeederName', ISNULL(@MPFeederName,N'؟'))
	SET @SMS = Replace( @SMS,'LPPostName', ISNULL(@LPPostName,N'؟'))
	SET @SMS = Replace( @SMS,'LPPostCode', ISNULL(@LPPostCode,N'؟'))
	SET @SMS = Replace( @SMS,'LPFeederName', ISNULL(@LPFeederName,N'؟'))
	SET @SMS = Replace( @SMS,'DBAddress', ISNULL(@Address,N'؟'))
	SET @SMS = Replace( @SMS,'FogheToziFeederName', ISNULL(@FogheToziFeederName,N'؟'))
	SET @SMS = Replace( @SMS,'DisconnectPowerMW', ISNULL(@DisconnectPowerMW ,N'؟'))
	SET @SMS = Replace( @SMS,'DisconnectPower', ISNULL(@DisconnectPower,N'؟'))
	SET @SMS = Replace( @SMS,'DTDate', ISNULL(@DTDate,N'؟'))
	SET @SMS = Replace( @SMS,'DTTime', ISNULL(@DTTime,N'؟'))
	SET @SMS = Replace( @SMS,'FogheToziType', ISNULL(@FogheToziType,N'؟'))
	SET @SMS = Replace( @SMS,'FogheToziGroup', ISNULL(@FogheToziGroup,N'؟'))
	SET @SMS = Replace( @SMS,'FogheToziShortText', ISNULL(@FogheToziShortText,N'؟'))
	SET @SMS = Replace( @SMS,'Reason', ISNULL(@Reason,N'؟'))
	SET @SMS = Replace( @SMS,'MPStatus', ISNULL(@MPStatus,N'؟'))
	SET @SMS = Replace( @SMS,'CurrentValue', ISNULL(@CurrentValue,N'؟'))
	SET @SMS = Replace( @SMS,'FromType', CASE WHEN NULLIF(@FromPathType,N'') IS NOT NULL THEN N'از ' + @FromPathType ELSE '' END )
	SET @SMS = Replace( @SMS,'FromValue', @FromPathTypeValue)
	SET @SMS = Replace( @SMS,'ToType', CASE WHEN NULLIF(@ToPathType,N'') IS NOT NULL THEN N'تا ' + @ToPathType ELSE '' END )
	SET @SMS = Replace( @SMS,'ToValue', @ToPathTypeValue)
	SET @SMS = Replace( @SMS,'FeederPart', ISNULL(@FeederPart,N'؟'))
	SET @SMS = Replace( @SMS,'TamirStr', ISNULL(@TamirStr,N'؟'))
	SET @SMS = Replace( @SMS,'ZoneName', ISNULL(@ZoneName,N'؟'))
	SET @SMS = Replace( @SMS,'SubscriberType', ISNULL(@SubscriberType,N'؟'))
	SET @SMS = Replace( @SMS,'OCEFRelayAction', ISNULL(@OCEFRelayAction,N'؟'))
	SET @SMS = Replace( @SMS,'NotComplate', ISNULL(@NotComplate,''))
	SET @SMS = Replace( @SMS,'MultiStepConnections', ISNULL(@MultiStepConnections,''))
	SET @SMS = Replace( @SMS,'CurrentDate', ISNULL(@CurrentDate,''))
	SET @SMS = Replace( @SMS,'CurrentMonthDay', ISNULL(@CurrentMonthDay,''))
	
	SET @SMS = Replace( @SMS,'CRLF', nchar(13))

	DECLARE @Desc AS nvarchar(100)
	SET @Desc = 'ReqId=' + Cast(@RequestNumber AS nvarchar)
	EXEC spSendSMS @SMS , @ManagerMobile, @Desc, @RequestType, @AreaId
--print 'spSendSMSDCManager End ' + convert(nvarchar(100),getdate(),21)
GO





select * from TblRequestPostFeeder 

select * from Tbl_LocationType

select top(10) * from TblRequest

select top(10) * from TblManagerSMSDC

-------------Find the Columns----------
SELECT *
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'TblRequest'
ORDER BY COLUMN_NAME
--ORDER BY ORDINAL_POSITION
