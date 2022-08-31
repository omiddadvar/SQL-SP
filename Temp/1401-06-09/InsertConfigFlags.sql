/*--------- Havades ---------*/
/*    تنظيمات نرم افزارها    */
/* ------------------------- */
/*      Version: 1.1.6       */
/*      Date: 97/05/16       */
/*---------------------------*/

-- کد پست توزيع به روش اراک
IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 100) 
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(100,N'IsShowLPPostNumber',N'True')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) 
VALUES ('Tbl_Config',148,0,4,NULL,99,GETDATE(),'IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 100) INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(100,N''IsShowLPPostNumber'',N''True'')')
GO

-- اجباري بودن انتخاب منطقه و نوع منطقه(شهري و روستايي) در حالت خاموشي بابرنامه ـ
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 101)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (101,N'IsForceZone',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 101) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (101,N''IsForceZone'',N''True'',NULL)')
GO

-- اجباري بودن انتخاب نوع منطقه(شهري و روستايي) ـ
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 102)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (102,N'IsForceEnvironment',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 102) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (102,N''IsForceEnvironment'',N''True'',NULL)')
GO

-- غير فعال نمودن ذخيره تکنسين و اکيپ در نسخه مرکز و ستاد
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 103)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (103,N'IsDisableMasterInCenter',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 103) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (103,N''IsDisableMasterInCenter'',N''True'',NULL)')
GO

-- اجباري بودن انتخاب منطقه و نوع منطقه(شهري و روستايي) در همه پرونده‌ها ـ
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 104)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (104,N'IsForceZoneAll',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 104) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (104,N''IsForceZoneAll'',N''True'',NULL)')
GO

-- عدم توانايي ثبت خاموشي فشار متوسط براي تعداد روز خاصي به قبل
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = 'ApplyDCCountMP')
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (105,N'ApplyDCCountMP',N'20',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = ''ApplyDCCountMP'') INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (105,N''ApplyDCCountMP'',N''20'',NULL)')
GO

-- عدم توانايي ثبت خاموشي فشار ضعيف براي تعداد روز خاصي به قبل
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = 'ApplyDCCountLP')
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (106,N'ApplyDCCountLP',N'20',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = ''ApplyDCCountLP'') INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (106,N''ApplyDCCountLP'',N''20'',NULL)')
GO

-- شمارنده براي بررسي نياز به ارسال پيامک
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 107)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (107,N'smsCounter',N'0',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 107) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (107,N''smsCounter'',N''0'',NULL)')
GO

-- پيام بروز رساني جديد
/*
ConfigId = 108 =>	ConfigName = StartTipBazdid		=>	Bazdid Service
ConfigId = 109 =>	ConfigName = StartTipHavades	=>	Sabte Havades
ConfigId = 110 =>	ConfigName = StartTipTamir		=>	Darkhast Khamooshi (Tamirat)
ConfigId = 111 =>	ConfigName = StartTipErja		=>	Erjaat
*/
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 108) 
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(108,N'StartTipHavades',N'0','')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 108) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (108,N''StartTipHavades'',N''0'','')')
GO

IF NOT EXISTS(SELECT * FROM Tbl_Config WHERE ConfigId = 108 AND Cast(ConfigValue AS INT) > 1)
BEGIN
	UPDATE Tbl_Config 
	SET 
		ConfigName = N'StartTipHavades',
		ConfigValue = N'1',
		ConfigText = Replace(N'اطلاع رسان\0x6CC:' + char(10) + char(13) + 'آيا مي‌دانيد که در نسخه جديد نرم افزار، راهنما\0x6CC کامل و دستورالعمل کار با سامانه بازديد سرويس به آن اضافه شده و از منو\0x6CC « راهنما » قابل مشاهده است؟','\0x6CC',nchar(1740)) 
	WHERE ConfigId = 108
END
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS(SELECT * FROM Tbl_Config WHERE ConfigId = 108 AND Cast(ConfigValue AS INT) > 1) BEGIN UPDATE Tbl_Config SET ConfigName = N''StartTipHavades'', ConfigValue = N''1'', ConfigText = Replace(N''اطلاع رسان\0x6CC:'' + char(10) + char(13) + ''آيا مي‌دانيد که در نسخه جديد نرم افزار، راهنما\0x6CC کامل و دستورالعمل کار با سامانه بازديد سرويس به آن اضافه شده و از منو\0x6CC « راهنما » قابل مشاهده است؟'',''\0x6CC'',nchar(1740)) WHERE ConfigId = 108 END')
GO

-- spGetMPPost_v2 Update
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 112)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(112,N'spGetMPPosts_v2',N'ver2',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 112) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (112,N''spGetMPPosts_v2'',N''ver2'',NULL)')
GO

-- اجباري شدن پر کردن مختصات جي.پي.اس در تکميل پرونده فشار ضعيف
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 113)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(113,N'IsForceLPReqGPS',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 113) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (113,N''IsForceLPReqGPS'',N''True'',NULL)')
GO

-- جلوگيري از ثبت خاموشي‌هاي بابرنامه با زمان قطع صفر دقيقه
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 114)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(114,N'IsForceTamirTime',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 114) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (114,N''IsForceTamirTime'',N''،True'',NULL)')
GO

-- عدم اجباري شدن تعيين ظرفيت پست توزيع در اطلاعات پايه
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 115)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(115,N'IsForceLPPostCapacity',N'False',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 115) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (115,N''IsForceLPPostCapacity'',N''False'',NULL)')
GO

-- اجباري شدن تعيين طول فيدر فشار ضعيف
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 116)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(116,N'IsForceLPFeederLen',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 116) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (116,N''IsForceLPFeederLen'',N''True'',NULL)')
GO

-- اجباري شدن تعيين آدرس فيدر فشار ضعيف
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 117)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(117,N'IsForceLPFeederAddress',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 117) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (117,N''IsForceLPFeederAddress'',N''True'',NULL)')
GO

-- اجباري شدن تعييت مالکيت فيدر فشار ضعيف
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 118)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(118,N'IsForceLPFeederOwnership',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 118) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (118,N''IsForceLPFeederOwnership'',N''True'',NULL)')
GO

-- توانايي ثبت بيش از يک وصل ناموفق در وصل‌هاي چند مرحله‌اي
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 119)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(119,N'IsOnlyBeforeConnect',N'False',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 119) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (119,N''IsOnlyBeforeConnect'',N''False'',NULL)')
GO

-- محاسبه اتوماتيک تعداد مشترکين پست توزيع از روي جمع تعداد مشتريکن فيدرهاي خروجي و عدم امکان تغيير دستي آن
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 120)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(120,N'IsAutoLPPostSubCount',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 120) INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(120,N''IsAutoLPPostSubCount'',N''True'',NULL)')
GO

-- فعال شدن گزينه «مرکز 121» در اطلاعات پايه نواحي
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 121)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(121,N'IsAllowParentAreaEnabled',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 121) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (121,N''IsAllowParentAreaEnabled'',N''True'',NULL)')
GO

-- جستجوي تمام شماره تلفنهاي شبيه تلفن تماس گرفته شده در هنگام Popup
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 122)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(122,N'IsLikeTelSearching',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 122) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (122,N''IsLikeTelSearching'',N''True'',NULL)')
GO

-- فعالسازي سرويس باز شدن Popup با پيامک از طرف مشترک
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 123)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(123,N'SMSPopupSystem',N'Active',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 123) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (123,N''SMSPopupSystem'',N''Active'',NULL)')
GO

-- عدم توانايي تکميل پرونده فشار متوسط، بعد از گذشت مدت زمان مشخصي از ثبت اوليه
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = 'DCSaveDurationMP')
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (124,N'DCSaveDurationMP',N'20',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = ''DCSaveDurationMP'') INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (124,N''DCSaveDurationMP'',N''20'',NULL)')
GO

-- عدم توانايي تکميل پرونده فشار ضعيف، بعد از گذشت مدت زمان مشخصي از ثبت اوليه
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = 'DCSaveDurationLP')
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (125,N'DCSaveDurationLP',N'20',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = ''DCSaveDurationLP'') INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (125,N''DCSaveDurationLP'',N''20'',NULL)')
GO

-- فعال نمودن امکان انتخاب از...تا يا تکه فيدر به هنگام ثبت خاموشي فشار متوسط روي کل فيدر (منجر به قطع فيدر شده باشد) ـ
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = 'EnableFeederPartDCOnAllFeeder')
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (126,N'EnableFeederPartDCOnAllFeeder',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = ''EnableFeederPartDCOnAllFeeder'') INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (126,N''EnableFeederPartDCOnAllFeeder'',N''True'',NULL)')
GO

-- حذف اجباري بودن ورود اطلاعات تاريخ نصب، بهره‌برداري و برداشته شدن ترانس از روي پست
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = 'IsForceLPPostTransDates')
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (127,N'IsForceLPPostTransDates',N'False',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = ''IsForceLPPostTransDates'') INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (127,N''IsForceLPPostTransDates'',N''False'',NULL)')
GO

--اجباري شدن اعزام اکيپ براي تمامي خاموشي‌هاي فشار متوسط 
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = 'IsForceEzamEkip')
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (128,N'IsForceEzamEkip',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = ''IsForceEzamEkip'') INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (128,N''IsForceEzamEkip'',N''True'',NULL)')
GO

--نام شرکت توزيع که در نگارشهاي بعد از 941015 در نرم افزار از اين رکورد استفاده ميگردد.
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = 'ToziName')
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) 
	SELECT TOP 1 '129' AS ConfigId,N'ToziName' AS ConfigName, Province AS ConfigValue , NULL AS ConfigText FROM Tbl_Province
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = ''ToziName'')	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) SELECT TOP 1 ''129'' AS ConfigId,N''ToziName'' AS ConfigName, Province AS ConfigValue , NULL AS ConfigText FROM Tbl_Province')
GO

--عدم اجباري شدن انتخاب فيدر فشار ضعيف براي قطع تک مشترک
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = 'IsForceSelectLPFeeder')
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (130,N'IsForceSelectLPFeeder',N'False',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = ''IsForceSelectLPFeeder'') INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (130,N''IsForceSelectLPFeeder'',N''False'',NULL)')
GO

--پر شدن فيلد ولتاژ انتهاي خط در بارگيري فيدرهاي فشار ضعيف بطور اتوماتيک
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = 'IsAutoFillEndLineVoltage')
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (131,N'IsAutoFillEndLineVoltage',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = ''IsAutoFillEndLineVoltage'') INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (131,N''IsAutoFillEndLineVoltage'',N''True'',NULL)')
GO

-- فعالسازي ارسال پيامک براي خاموشي تک مشترک
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 132)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(132,N'SMSSingleSubs',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 132) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (132,N''SMSSingleSubs'',N''True'',NULL)')
GO

-- اجباري شدن انتخاب خودرو در هنگام اعزام اکيپ
DELETE FROM Tbl_Config WHERE ConfigId = 133
GO
INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'DELETE FROM Tbl_Config WHERE ConfigId = 133')
GO

IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 133)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(133,N'IsForceAVLCar',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 133) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (133,N''IsForceAVLCar'',N''True'',NULL)')
GO

-- فعالسازي امکان انتخاب خودرو بدون در نظر گرفتن لايسنس شرکت توزيع
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 134)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(134,N'IsAVLisence',N'_Valid_',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 134) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (134,N''IsAVLisence'',N''_Valid_'',NULL)')
GO

-- اجباري شدن انتخاب آيتم شرايط جوي در تکميل پرونده فشار متوسط
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 135)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(135,N'IsForceMPWeather',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 135) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (135,N''IsForceMPWeather'',N''True'',NULL)')
GO

-- اجباري شدن انتخاب آيتم شرايط جوي در تکميل پرونده فشار ضعيف
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 136)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(136,N'IsForceLPWeather',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 136) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (136,N''IsForceLPWeather'',N''True'',NULL)')
GO

--اجباري شدن مقداردهي آيتم محل حادثه در خاموشيهاي فشار متوسط
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 137)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(137,N'IsForceMPEventLocation',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 137) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (137,N''IsForceMPEventLocation'',N''True'',NULL)')
GO

--اجباري شدن مقداردهي آيتم توضيحات در تکميل پرونده فشار متوسط
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 138)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(138,N'IsForceMPComment',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 138) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (138,N''IsForceMPComment'',N''True'',NULL)')
GO

--اجباري شدن مقداردهي آيتم توضيحات در تکميل پرونده فشار ضعيف
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 139)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(139,N'IsForceLPComment',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 139) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (139,N''IsForceLPComment'',N''True'',NULL)')
GO

--قابليت انتخاب پست هاي توزيعي که ناحيه اي متفاوت با ناحيه فيدر فشار متوسط دارند، اما داراي فيدر فشار ضعيف مشترک مي باشند. در تکميل پرونده فشار متوسط
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 140)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(140,N'IsShowLPPostsWithCommonLPFeeder',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 140) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (140,N''IsShowLPPostsWithCommonLPFeeder'',N''True'',NULL)')
GO

--توانايي ثبت زمان وصل خاموشي قبل از زمان اعزام اکيپ
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 141)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(141,N'IsForceConnectAfterEzam',N'False',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 141) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (141,N''IsForceConnectAfterEzam'',N''False'',NULL)')
GO

--مدت زمان توانايي ثبت قطعات مصرفي پس از برقدار شدن خاموشي
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 142)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(142,N'DurationRegisterPartAfterConnect',N'0',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 142) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (142,N''DurationRegisterPartAfterConnect'',N''0'',NULL)')
GO

--حداکثر مدت زمان توانايي ثبت قطعات مصرفي پس از برقدار شدن خاموشي
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 143)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(143,N'MaxDurationRegisterPartAfterConnect',N'96',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 143) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (143,N''MaxDurationRegisterPartAfterConnect'',N''96'',NULL)')
GO

--نحوه محاسبه پيک بار مرکز در ارسال SMS
/*
	MaxPeak = ماکسيمم مقدار پيک همزمان و غيرهمزمان
*/
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 144)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(144,N'MainPeakProtocol',N'MaxPeak',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 144) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (144,N''MainPeakProtocol'',N''MaxPeak'',NULL)')
GO

-- توانايي ويرايش شماره تلفن، براي خاموشيهايي که از طريق پاپ آپ ثبت ميگردند
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 145)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(145,N'IsAbilityChangeCallNumber',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 145) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (145,N''IsAbilityChangeCallNumber'',N''True'',NULL)')
GO

-- عدم ارسال پيامک خاموشي به دليل قطع بودن ارتباط سرورها پس از n دقيقه
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 146)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(146,N'ServiceTimeOut',N'1',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 146) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (146,N''ServiceTimeOut'',N''1'',NULL)')
GO

-- نمايش منوي پنل پيامک (SMS)
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 147)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(147,N'SMSPanel',N'_Active_',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 147) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (147,N''SMSPanel'',N''_Active_'',NULL)')
GO

-- درج کد فرم برای گزارش اجازه کار
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 148)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(148,N'EjazeKarReportCode',N'',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 148) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (148,N''EjazeKarReportCode'',N'''',NULL)')
GO

-- اجباری شدن ثبت وضعيت کلید فیدرهای فشار متوسط در تکمیل پرونده خطوط فشار متوسط
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 149)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(149,N'IsForceMPRequestKey',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 149) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (149,N''IsForceMPRequestKey'',N''True'',NULL)')
GO

-- اجباري شدن انتخاب آيتم نوع قطع کننده در تکميل پرونده فشار متوسط بابرنامه
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 150)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(150,N'IsForceMPCloserType',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 150) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (150,N''IsForceMPCloserType'',N''True'',NULL)')
GO

-- توانايي اعزام اکيپ ، حتی درصورت عدم دسترسي به تکمیل پرونده فشار متوسط یا فشار ضعیف
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 151)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(151,N'IsAccessRequestButtons',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 151) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (151,N''IsAccessRequestButtons'',N''True'',NULL)')
GO

-- عدم نمایش اکیپها و تکنسینهای غیر فعال حتی در اطلاعات پایه
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 152)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(152,N'IsShowActiveEkipOnly',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 152) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (152,N''IsShowActiveEkipOnly'',N''True'',NULL)')
GO

-- توانایی تکمیل پرونده خاموشیهای فشار متوسط و فوق توزیع در ستاد، حتی اگر خاموشی در نواحی ثبت شده باشد
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 153)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(153,N'IsCanRequestConfirmInSetad',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 153) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (153,N''IsCanRequestConfirmInSetad'',N''True'',NULL)')
GO

-- اجباري شدن شماره اشتراک در صورت انتخاب قطع تک مشترک در خاموشي هاي بي برنامه فشار ضعيف
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 154)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(154,N'IsForceSubscriberCode',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 154) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (154,N''IsForceSubscriberCode'',N''True'',NULL)')
GO

-- اجباری شدن انتخاب عملکرد رله وقتیکه قطع کامل فیدر رخ داده باشد
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 155)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(155,N'IsForceOCEFRelayAction',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 155) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (155,N''IsForceOCEFRelayAction'',N''True'',NULL)')
GO

-- جلوگیری از انتخاب عملکرد رله در صورتیکه منجر به قطع کامل فیدر نشده باشد
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 156)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(156,N'IsNotForceOCEFRelayAction',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 156) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (156,N''IsNotForceOCEFRelayAction'',N''True'',NULL)')
GO

-- فعالسازي ثبت خاموشي خودکار  بدون در نظر گرفتن لايسنس شرکت توزيع
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 157)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(157,N'IsWebServiceAutoRequest',N'_Valid_',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 157) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (157,N''IsWebServiceAutoRequest'',N''_Valid_'',NULL)')
GO

-- عدم نمايش پیام مربوط به محاسبه انرژی توزیع نشده فیدر یا پست خصوصی در نسخه نواحی و متمرکز 
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 158)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(158,N'IsShowPrivateMessage',N'False',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 158) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (158,N''IsShowPrivateMessage'',N''False'',NULL)')
GO

-- عدم توانایی انتخاب گزینه "منجر به قطع فیدر شده "در پرونده های فشار متوسط در نسخه نواحی و متمرکز 
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 159)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(159,N'DisableDCMPFeederInNavahi',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 159) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (159,N''DisableDCMPFeederInNavahi'',N''True'',NULL)')
GO

-- عدم نمایش پیغام آزمایشی بودن گزارش شاخصهای قابلیت اطمینان
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 160)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(160,N'DisablePilotMessageForReportIndex',N'False',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 160) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (160,N''DisablePilotMessageForReportIndex'',N''False'',NULL)')
GO

-- اجباری بودن انتخاب ترانس در بارگیری پستهای توزیع در صورت موجود بودن ترانس
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 161)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(161,N'IsForceLPTransLoading',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 161) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (161,N''IsForceLPTransLoading'',N''True'',NULL)')
GO

-- فعالسازی امکان ارسال پیام به نرم افزار موبایل و تنظیم آدرس وب سرویس برای آن
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 162)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(162,N'NotifyServiceAddress',N'http://IP:Port',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 162) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (162,N''NotifyServiceAddress'',N''http://IP:Port'',NULL)')
GO

-- دسترسي به نرم افزار ثبت حوادث
IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 163) 
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(163,N'IsHavadesAccess',N'True')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) 
VALUES ('Tbl_Config',148,0,4,NULL,99,GETDATE(),'IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 163) INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(163,N''IsHavadesAccess'',N''True'')')
GO


-- آدرس دسترسی به وب سرویس حوادث => TZServices
IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 164) 
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(164,N'TZServicesAddress',N'http://IP:Port/TZServices/ExternalLinks.asmx')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) 
VALUES ('Tbl_Config',148,0,4,NULL,99,GETDATE(),'IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 164) INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(164,N''TZServicesAddress'',N''http://IP:Port/TZServices/ExternalLinks.asmx'')')
GO

-- نمایش تاریخچه تماس مشترک، در صورت دریافت از سیستم جامع مشترکین
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 165)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(165,N'IsBillingLoadHistory',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 165) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (165,N''IsBillingLoadHistory'',N''True'',NULL)')
GO

-- محاسبه انرژی توزیع نشده برای حالت قطع تک مشترک
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 166)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(166,N'IsComputeSingleSubscriber',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 166) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (166,N''IsComputeSingleSubscriber'',N''True'',NULL)')
GO

-- مشاهده 8 بند اول گزارش 10-24
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 167)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(167,N'Rep10_24_IsShowFixData',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 167) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (167,N''Rep10_24_IsShowFixData'',N''True'',NULL)')
GO

-- اجازه دادن ثبت اعزام اکیپ جدید، در روزهای بعد از آخرین اعزام اکیپی که عدم انجام کار شده است
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 168)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(168,N'IsCanNotEzamAfterNotDone',N'False',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 168) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (168,N''IsCanNotEzamAfterNotDone'',N''False'',NULL)')
GO

-- عدم توانایی تغییر اطلاعات AVL توسط توزیع برقهای دارای وب سرویس ارتباط با AVL
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 169)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(169,N'IsDisableAVLInfo',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 169) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (169,N''IsDisableAVLInfo'',N''True'',NULL)')
GO

-- نمایش بدهی مشترک در پنجره popup حتی اگر قطع به علت بدهی نباشد
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 170)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(170,N'IsShowDeptOnlyOnPopup',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 170) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (170,N''IsShowDeptOnlyOnPopup'',N''True'',NULL)')
GO

-- اجباری شدن انتخاب ترانس پست توزيع در تکمیل پرونده خاموشی های فشار ضعیف
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 171)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(171,N'IsForceLPTransInLPConfirm',N'False',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 171) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (171,N''IsForceLPTransInLPConfirm'',N''False'',NULL)')
GO

-- غیر فعال شدن نرم افزارهای قدیمی ثبت حوادث
/* این کانفیگ در نسخه بروز رسانی دیتابیس روی سایت گذاشته شده است */
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 172)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(172,N'OldSoftExpDT',Convert(varchar,dateadd(dd,-1,GETDATE()),120),NULL)
GO

-- فعال شدن ارسال پیامک خاموشیها به مشترکین 
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 173)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(173,N'SMSSubscriber',N'_Active_',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 173) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (173,N''SMSSubscriber'',N''_Active_'',NULL)')
GO

-- عدم اجباری بودن مقداردهی فیلد بار کل فیدر پیش از قطع در تکمیل پرونده فشار متوسط
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 174)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(174,N'IsForcePreCurrentValue',N'False',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 174) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (174,N''IsForcePreCurrentValue'',N''False'',NULL)')
GO

--حداکثر سایز فایل صوتی بر حسب کیلوبایت مخصوص فیدرهای فشار متوسط در IVR
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 175)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(175,N'MaxIVRFileSize',N'600',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 175) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (175,N''MaxIVRFileSize'',N''600'',NULL)')
GO

-- اجباری شدن انتخاب خاص بودن مکالمه در فرم ثبت خاموشی
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 176)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(176,N'IsForceSpecialCall',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 176) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (176,N''IsForceSpecialCall'',N''True'',NULL)')
GO

-- عدم ارسال پیام به کاربر پس از تایید درخواست خاموشی
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 177)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(177,N'IsSendMessageForConfirm',N'False',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 177) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (177,N''IsSendMessageForConfirm'',N''False'',NULL)')
GO

-- عدم ارسال پیام به کاربر پس از عدم تایید درخواست خاموشی
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 178)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(178,N'IsSendMessageForNotConfirm',N'False',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 178) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (178,N''IsSendMessageForNotConfirm'',N''False'',NULL)')
GO

-- ماههايي که ضريب بار روزانه پستهاي توزيع بايد از نوع دوم محاسبه گردد
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 179)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(179,N'PostZaribMonthTypeId',N'3,4,5,6',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 179) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (179,N''PostZaribMonthTypeId'',N''3,4,5,6'',NULL)')
GO

-- آدرس سرور فایل سرور
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 180)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(180,N'FileServerAddress',N'http://IP:Port/Fileserver.asmx',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 180) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (180,N''FileServerAddress'',N''http://IP:Port/Fileserver.asmx'',NULL)')
GO

-- اجباری شدن انتخاب عملکرد ریکلوزر در تکمیل پرونده های فشار متوسط
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 181)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(181,N'IsForceRecloserAction',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 181) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (181,N''IsForceRecloserAction'',N''True'',NULL)')
GO

-- فعال شدن ارسال پيامک راهنمايي مشترکين از طریق وب سرویس
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 182)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(182,N'IsActiveGuideSMSSent',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 182) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (182,N''IsActiveGuideSMSSent'',N''True'',NULL)')
GO

IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 183)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(183,N'CCI_MaxTry',N'5',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 183) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (183,N''CCI_MaxTry'',N''5'',NULL)')
GO

IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 184)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(184,N'CCI_WaitTime',N'5',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 184) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (184,N''CCI_WaitTime'',N''5'',NULL)')
GO

IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 185)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(185,N'CCI_TrunkExtNo',N'9',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 185) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (185,N''CCI_TrunkExtNo'',N''9'',NULL)')
GO

-- عدم توانايي ثبت ترانس برای پستهای پاساژ و همچنین اجباری نبودن ثبت ظرفِت براي آنها
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 186)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(186,N'IsPasajPost',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 186) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (186,N''IsPasajPost'',N''True'',NULL)')
GO

-- نمایش ستون رضایتمندی مشترک پس از دریافت پیامک
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 187)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(187,N'SubscriberSMSReceive',N'_Active_',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 187) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (187,N''SubscriberSMSReceive'',N''_Active_'',NULL)')
GO

-- ماکزیمم تعداد کاراکتر مجازی که میتوان پشت سرهم در پسوورد ذخیره نمود
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 188)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(188,N'MaxSequentialLetters',N'4',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 188) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (188,N''MaxSequentialLetters'',N''4'',NULL)')
GO

-- ارسال کد جی.آی.اس پست یا فیدر به همراه شماره خاموشی در هنگام نمایش خاموشی روی نقشه
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 189)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(189,N'IsSendGISCodesForShowDC',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 189) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (189,N''IsSendGISCodesForShowDC'',N''True'',NULL)')
GO

-- فقط نمایش فیدرهای مربوط به پست فوق توزیع انتخاب شده در تکمیل پرونده
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 190)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(190,N'IsShowOnlyMPFeederForMPPost',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 190) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (190,N''IsShowOnlyMPFeederForMPPost'',N''True'',NULL)')
GO

-- متن نوتیفیکیشن مربوط به اطلاع رسانی خاموشی بابرنامه به مشترکین از طریق نرم افزار موبایل
-- JKNotificationTamirBody
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 191)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(191,N'JKNotificationTamirBody',NULL,'مشترک گرامي، برق شما در تاريخ DisconnectDate، از ساعت FromTime تا ساعت ToTime به دليل تعميرات قطع خواهد شد')
GO

-- متن نوتیفیکیشن مربوط به اطلاع رسانی خاموشی بی برنامه به مشترکین از طریق نرم افزار موبایل
-- JKNotificationNotTamirBody
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 192)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(192,N'JKNotificationNotTamirBody',NULL,'مشترک گرامي، با عرض پوزش از خاموشي بوجود آمده، همکاران ما پيگير رفع خاموشي مي باشند')
GO

-- دسترسی به منوی ثبت چند خاموشی فوق توزیع به صورت یکجا
-- IsAccessToRequestMultiDC
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 193)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(193,N'IsAccessToRequestMultiDC',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 193) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (193,N''IsAccessToRequestMultiDC'',N''True'',NULL)')
GO

-- مدت زمان ریست شدن تعداد خطای ورود ناموفق کاربران
-- TimeForResetLogin
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 194)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(194,N'TimeForResetLogin',N'12',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 194) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (194,N''TimeForResetLogin'',N''12'',NULL)')
GO

-- عدم نمایش کد فیدر در ستون نام فیدر در فرم بارگیری فیدرهای فشار متوسط
-- IsShowMPFeederCodeInLoading
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 195)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(195,N'IsShowMPFeederCodeInLoading',N'False',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 195) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (195,N''IsShowMPFeederCodeInLoading'',N''False'',NULL)')
GO

-- عدم ارسال پيامک تعيين وضعيت تاييد يا عدم تاييد براي خاموشیهای خط گرم
-- IsSendWaitSMSForWarmLine
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 196)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(196,N'IsSendWaitSMSForWarmLine',N'0',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 196) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (196,N''IsSendWaitSMSForWarmLine'',N''0'',NULL)')
GO

-- عدم تعیین مقدار فیلد خط گرم از روی پرونده درخواست خاموشی
-- IsDisableTamirIsWarmLine
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 197)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(197,N'IsDisableTamirIsWarmLine',N'False',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 197) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (197,N''IsDisableTamirIsWarmLine'',N''False'',NULL)')
GO

-- فعال شدن صدای مردم
-- ActivePeopleVoice
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 198)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(198,N'ActivePeopleVoice',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 198) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (198,N''ActivePeopleVoice'',N''True'',NULL)')
GO

-- آدرس دسترسی به وب سرویس فایل سرور در TZServices => TZServicesFileServerAddress
IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 199) 
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(199,N'TZServicesFileServerAddress',N'http://IP:Port/TZServices/RestServices/GetPostedFiles.aspx')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) 
VALUES ('Tbl_Config',148,0,4,NULL,99,GETDATE(),'IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 199) INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(199,N''TZServicesFileServerAddress'',N''http://IP:Port/TZServices/RestServices/GetPostedFiles.aspx'')')
GO


-- مقدار پیش فرض نوع خاموشی بابرنامه در نرم افزار ثبت حوادث
-- TamirTypeForHavades
IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 701) 
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(701,N'TamirTypeForHavades',N'1')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) 
VALUES ('Tbl_Config',148,0,4,NULL,99,GETDATE(),'IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 701) INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(701,N''TamirTypeForHavades'',N''1'')')
GO

-- غیرفعال شدن تغییر مقدار پیش فرض نوع خاموشی بابرنامه در نرم افزار ثبت حوادث - پیش فرض قابل تغییر می باشد
-- IsActiveTamirType
IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 702) 
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(702,N'IsActiveTamirType',N'False')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) 
VALUES ('Tbl_Config',148,0,4,NULL,99,GETDATE(),'IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 702) INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(702,N''IsActiveTamirType'',N''False'')')
GO

-- اجباري شدن آيتم نوع خاموشي بابرنامه در ثبت پرونده بابرنامه
-- IsForceTamirType
IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 703) 
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(703,N'IsForceTamirType',N'True')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) 
VALUES ('Tbl_Config',148,0,4,NULL,99,GETDATE(),'IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 703) INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(703,N''IsForceTamirType'',N''True'')')
GO

-- آدرس وب سرويسهاي نسخه تحت وب توانير => TZHavadesTavanirURL
IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 704) 
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(704,N'TZHavadesTavanirURL',N'http://IP:Port/Tavanir/')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) 
VALUES ('Tbl_Config',148,0,4,NULL,99,GETDATE(),'IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 704) INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(704,N''TZHavadesTavanirURL'',N''http://IP:Port/Tavanir/'')')
GO

-- اجباری کردن اعزام اکیپ برای پرونده هایی که به درخواست مدیریت اضطراری بار میباشند (پیش فرض اعزام اکیپ اجباری نمی باشد)
IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 705) 
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(705,N'IsNotForceEzamForBar',N'false')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) 
VALUES ('Tbl_Config',148,0,4,NULL,99,GETDATE(),'IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 705) INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(705,N''IsNotForceEzamForBar'',N''false'')')
GO

-- تک انتخابي کردن فرم انتخاب پست و فيدر خاموشي (فقط مي توان يک عارضه را به عنوان عارضه خاموش شده انتخاب نمود)
IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 706) 
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(706,N'IsOnlySinglePostFeederSelect',N'true')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) 
VALUES ('Tbl_Config',148,0,4,NULL,99,GETDATE(),'IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 706) INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(706,N''IsOnlySinglePostFeederSelect'',N''true'')')
GO

-- نوع فيلد دريافتي مشترکين از GIS در متد SetOpration براي ارسال پيامک
-- FileNo = 1 , SubscriberCode = 2 , RamzCode = 3 , BillingId = 4 

IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 707) 
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(707,N'SubscriberCodeTypes',N'3')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) 
VALUES ('Tbl_Config',148,0,4,NULL,99,GETDATE(),'IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 707) INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(707,N''SubscriberCodeTypes'',N''3'')')
GO

-- فعال کردن دریافت فهرست مشترکین از GIS جهت ارسال پیامک اطلاع رسانی خاموشی
IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 708) 
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(708,N'IsCanGetSubFromGIS',N'True')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) 
VALUES ('Tbl_Config',148,0,4,NULL,99,GETDATE(),'IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 708) INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(708,N''IsCanGetSubFromGIS'',N''True'')')
GO

-- ارسال پيامک خاموشي فقط به مشترکين حساس زماني که اطلاعات مشترکين از GIS دريافت ميگردد
IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 709) 
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(709,N'OnlySMSSensitivity',N'True')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) 
VALUES ('Tbl_Config',148,0,4,NULL,99,GETDATE(),'IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 709) INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(709,N''OnlySMSSensitivity'',N''True'')')
GO

-- دريافت آدرس خاموشي از روي نام پست و فيدرهاي انتخاب شده - پيش فرض از روي آدرس پست و فيدرهاي انتخاب شده پر ميشود
IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 710) 
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(710,N'IsReadAddressFromName',N'True')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) 
VALUES ('Tbl_Config',148,0,4,NULL,99,GETDATE(),'IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 710) INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(710,N''IsReadAddressFromName'',N''True'')')
GO

-- جهت پرکردن فيلد نام پست و کد پست به طور همزمان در مانيتورينگ هما
IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 711) 
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(711,N'HomaMonitoringISShowLPPostCode',N'True')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) 
VALUES ('Tbl_Config',148,0,4,NULL,99,GETDATE(),'IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 711) INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(711,N''HomaMonitoringISShowLPPostCode'',N''True'')')
GO

-- فعالسازي سرويس باز شدن پاپ آپ برای وقوع رخداد از طرف سامانه اسکادا
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 712)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(712,N'ScadaPopupSystem',N'Active',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 712) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (712,N''ScadaPopupSystem'',N''Active'',NULL)')
GO

-- تولید خاموشی فوق توزیع در ماژول مدیریت اضطراری بار
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 713)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(713,N'IsFogheToziEmergencyOutage',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 713) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (713,N''IsFogheToziEmergencyOutage'',N''True'',NULL)')
GO
-- دسترسی به ماژول مدیریت اضطراری بار
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 714)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(714,N'AccessEmergency',N'_Active_',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 714) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (714,N''AccessEmergency'',N''_Active_'',NULL)')
GO

-- قابلیت نمایش منوی مدیریت اضطراری بار در نواحی غیر ستادی
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 715)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(715,N'IsShowEmergencyForAll',N'False',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 715) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (715,N''IsShowEmergencyForAll'',N''False'',NULL)')
GO

-- اختیاری شدن اعمال DecrEaseFactor در محاسبه 'جريان نامي ترانس' در گزارش 8-1 فایل rptlppostload.rpt
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 716)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(716,N'IsDecreaseFactorApplied',N'False',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 716) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (716,N''IsDecreaseFactorApplied'',N''False'',NULL)')
GO

-- اعمال IsForceMPFeederMaxDCTime (اجباری بود رعایت حداکثر زمان مجاز خاموشی فیدرهای فشار متوسط) در نرم افزار درخواست خاموشی برای کار بر روی خط گرم
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 717)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(717,N'WarmLineEnforce_IsForceMPFeederMaxDCTime',N'False',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 717) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (717,N''WarmLineEnforce_IsForceMPFeederMaxDCTime'',N''False'',NULL)')
GO

/*--------- Erjaat ----------*/
/*     نرم افزار ارجاعات     */
/*---------------------------*/

-- دسترسي به ارجاعات در ثبت حوادث
IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 200) 
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(200,N'IsErjaAccess',N'True')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) 
VALUES ('Tbl_Config',148,0,4,NULL,99,GETDATE(),'IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 200) INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(200,N''IsErjaAccess'',N''True'')')
GO

-- دسترسي به برداشت اطلاعات پست توزيع در اطلاعات پايه
IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 210) 
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(210,N'IsAccessLPPostInfo',N'True')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) 
VALUES ('Tbl_Config',148,0,4,NULL,99,GETDATE(),'IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 210) INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(210,N''IsAccessLPPostInfo'',N''True'')')
GO

-- اجباری شدن شماره پرونده مشترک در مشخصات ارجاع در صورت انتخاب نوع شبکه مشترکین
IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 201) 
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(201,N'IsForceFileNo',N'True')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) 
VALUES ('Tbl_Config',148,0,4,NULL,99,GETDATE(),'IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 201) INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(201,N''IsForceFileNo'',N''True'')')
GO

-- عدم بررسی وجود موجودی برای ثبت قطعات مصرفی در ارجاعات
IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 202) 
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(202,N'IsCheckErjaPartCount',N'False')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) 
VALUES ('Tbl_Config',148,0,4,NULL,99,GETDATE(),'IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 202) INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(202,N''IsCheckErjaPartCount'',N''False'')')
GO

-- مشخص سازی واحد اجرایی انشعابات مشترکان که باید در البرز به سامانه جامع مشترکین ارسال گردند
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 203)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(203,N'ReferToSubscribersIDs',N'2',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 203) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (203,N''ReferToSubscribersIDs'',N''2'',NULL)')
GO

-- اجباری شدن رمز رایانه مشترک در مشخصات ارجاع در صورت انتخاب نوع شبکه مشترکین
IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 204) 
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(204,N'IsForceRamzCode',N'True')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) 
VALUES ('Tbl_Config',148,0,4,NULL,99,GETDATE(),'IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 204) INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(204,N''IsForceRamzCode'',N''True'')')
GO

-- اجباری شدن یکی از موارد رمز رایانه یا شماره پرونده هنگام ارجاع پرونده در پرونده مشخصات ارجاع
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 205)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(205,N'IsForceFileNoOrRamzCode',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 205) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (205,N''IsForceFileNoOrRamzCode'',N''True'',NULL)')
GO

/*------- TamirRequest ------*/
/*  درخواست خاموشي بابرنامه  */
/*---------------------------*/

-- دسترسي به درخواست اجازه کار خاموشي بابرنامه
IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 400) 
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(400,N'IsTamirRequestAccess',N'True')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) 
VALUES ('Tbl_Config',148,0,4,NULL,99,GETDATE(),'IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 400) INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(400,N''IsTamirRequestAccess'',N''True'')')
GO

-- نوع ديد به نواحي در هنگام محاسبه سقف خاموشي ها
IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 401) 
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(401,N'IsTamirServer121',N'True')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) 
VALUES ('Tbl_Config',148,0,4,NULL,99,GETDATE(),'IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 401) INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(401,N''IsTamirServer121'',N''True'')')
GO

-- توانايي تأييد درخواست خاموشي تکه فيدر و پست در ناحيه
IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 402) 
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(402,N'IsSpecialConfirm',N'True')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) 
VALUES ('Tbl_Config',148,0,4,NULL,99,GETDATE(),'IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 402) INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(402,N''IsSpecialConfirm'',N''True'')')
GO

-- غير فعال نشدن «منجر به قطع فيدر شده» در خاموشي بابرنامه (به درخواست) روي پست توزيع به هنگام تکميل پرونده ـ
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 403)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (403,N'IsEnableMPFeederDCOnTamirPost',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 403) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (403,N''IsEnableMPFeederDCOnTamirPost'',N''True'',NULL)')
GO

-- نمايش همه انواع شبکه در خاموشي بابرنامه (به درخواست) روي پست توزيع به هنگام تکميل پرونده ـ
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 404)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (404,N'IsEnableAllGroupOnTamirPost',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 404) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (404,N''IsEnableAllGroupOnTamirPost'',N''True'',NULL)')
GO

-- قبول درخواست خاموشي قبل از ساعت خاص روز قبل
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = 'IsTamirBeforeHour')
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (405,N'IsTamirBeforeHour',N'9',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = ''IsTamirBeforeHour'') INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (405,N''IsTamirBeforeHour'',N''9'',NULL)')
GO

-- ارسال مستقيم درخواست خاموشي از ناحيه به ستاد
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 406)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (406,N'IsTamirDirectToSetad',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 406) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (406,N''IsTamirDirectToSetad'',N''True'',NULL)')
GO

-- قبول درخواست خاموشي براي شنبه قبل از ساعت خاص روز پنجشنبه
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = 'IsTamirRequestOnThursday')
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (407,N'IsTamirRequestOnThursday',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = ''IsTamirRequestOnThursday'') INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (407,N''IsTamirRequestOnThursday'',N''True'',NULL)')
GO

-- اجباري شدن ورود اطلاعات اجازه کار هنگام اعزام اکيپ براي خاموشي‌هاي بابرنامه
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 408)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(408,N'IsForceAllow',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 408) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (408,N''IsForceAllow'',N''True'',NULL)')
GO

-- قبول درخواست خاموشي قبل از ساعت خاص روز چهارشنبه
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = 'TamirBeforeHourWednesday')
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (409,N'TamirBeforeHourWednesday',N'9',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = ''TamirBeforeHourWednesday'') INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (409,N''TamirBeforeHourWednesday'',N''9'',NULL)')
GO

-- محاسبه ماهيانه سهميه خاموشي نواحي
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 410)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (410,N'IsTamirPowerMonthly',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 410) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (410,N''IsTamirPowerMonthly'',N''True'',NULL)')
GO

-- تشخيص خودکار نوع خاموشي بابرنامه بر اساس تاريخ درخواست خاموشي ـ نامه جنوب کرمان
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 411)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(411,N'TamirRequestMonthDay',N'15',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 411) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (411,N''TamirRequestMonthDay'',N''15'',NULL)')
GO

-- حذف محدوديت زمان ارسال درخواست براي خاموشي‌هاي فشار ضعيف
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 412)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(412,N'NoLPTamirLimit',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 412) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (412,N''NoLPTamirLimit'',N''True'',NULL)')
GO

-- عدم مشاهده پرونده‌هاي تأييد شده در نواحي براي تکه فيدرها و پست‌ها براي کاربر ستاد
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 413)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(413,N'IsShowSpecialConfirms',N'False',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 413) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (413,N''IsShowSpecialConfirms'',N''False'',NULL)')
GO

-- برداشتن محدوديت زماني به صورت کلي براي ثبت درخواست خاموشي بابرنامه
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = 'TamirTimeLimit')
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(414,N'TamirTimeLimit',N'False',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = ''TamirTimeLimit'') INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (414,N''TamirTimeLimit'',N''False'',NULL)')
GO

-- عدم اجباري بودن مدت زمان فهرست عمليات
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = 'IsForceTamirOperationTime')
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(415,N'IsForceTamirOperationTime',N'False',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = ''IsForceTamirOperationTime'') INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (415,N''IsForceTamirOperationTime'',N''False'',NULL)')
GO

-- عدم مقداردهي نوع منطقه(شهري، روستايي) در نرم افزار ثبت حوادث با توجه به اطلاعات (شهري و روستايي ) در نرم افزار درخواست خاموشي
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = 'IsForceEnvironmentTypeId')
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(416,N'IsForceEnvironmentTypeId',N'False',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = ''IsForceEnvironmentTypeId'') INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (416,N''IsForceEnvironmentTypeId'',N''False'',NULL)')
GO

/*اجباري شدن مقداردهي فيلد شماره فرم درخواست ثبت تغييرات GIS*/
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = 'IsForceGISFormNo')
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(417,N'IsForceGISFormNo',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = ''IsForceGISFormNo'') INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (417,N''IsForceGISFormNo'',N''True'',NULL)')
GO

--عدم اجباري شدن پر نمودن تاريخ شروع و پايان قطع براي مانور در در خواست خاموشي بابرنامه
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 418)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(418,N'IsForceTamirManovrDC',N'False',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 418) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (418,N''IsForceTamirManovrDC'',N''False'',NULL)')
GO

--اجباري شدن پر نمودن مختصات X و Y در در خواست خاموشي بابرنامه
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 419)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(419,N'IsForceTamirGPS',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 419) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (419,N''IsForceTamirGPS'',N''True'',NULL)')
GO

-- اجباري شدن انتخاب  پيمانکاران، در صورتيکه خاموشي به درخواست پيمانکار باشد
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 420)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(420,N'IsForcePeymankar',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 420) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (420,N''IsForcePeymankar'',N''True'',NULL)')
GO

-- چک نکردن زمان درخواست خاموشي، براي درخواستهاي در انتظار تاييد (فقط در هنگام ثبت اوليه زمان درخواست بررسي ميگردد)
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 421)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(421,N'IsCheckTimeForAll',N'False',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 421) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (421,N''IsCheckTimeForAll'',N''False'',NULL)')
GO

-- عدم اجباري بودن تاييد ناظر، براي خاموشي هاي فشار ضعيف
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 422)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(422,N'IsForceTaeedNazerForLPFeeder',N'False',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 422) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (422,N''IsForceTaeedNazerForLPFeeder'',N''False'',NULL)')
GO

-- اجباری بودن رعایت حداکثر زمان خاموشی مجاز فیدر فشار متوسط
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 423)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(423,N'IsForceMPFeederMaxDCTime',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 423) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (423,N''IsForceMPFeederMaxDCTime'',N''True'',NULL)')
GO

-- اجباري شدن تاييد ناظر براي خاموشي هاي خط گرم
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 424)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(424,N'IsForceTaeedNazerForWarmLine',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 424) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (424,N''IsForceTaeedNazerForWarmLine'',N''True'',NULL)')
GO

-- انتخاب شدن گزينه خط گرم براي خاموشي هاي جديد
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 425)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(425,N'IsWarmLineDefault',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 425) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (425,N''IsWarmLineDefault'',N''True'',NULL)')
GO

-- توانايي ارسال پيامک خاموشي مشترکين به صورت خودکار در هنگام تاييد درخواست خاموشي
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 426)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(426,N'AutoSendSMSSensitive',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 426) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (426,N''AutoSendSMSSensitive'',N''True'',NULL)')
GO

-- انتخاب شدن گزينه ارسال پيامک خاموشي به مشترکين حساس، به صورت پيش فرض
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 427)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(427,N'IsCheckSendSMSSensitive',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 427) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (427,N''IsCheckSendSMSSensitive'',N''True'',NULL)')
GO

-- عدم توانايي ثبت درخواست خاموشي براي کمتر از 2 روز آینده
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 428)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(428,N'IsTamirLimitDay',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 428) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (428,N''IsTamirLimitDay'',N''True'',NULL)')
GO

-- عدم توانايي ثبت درخواست خاموشی قبل از ساعت خاص روز قبل
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 429)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(429,N'IsTamirLimitHour',N'72',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 429) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (429,N''IsTamirLimitHour'',N''72'',NULL)')
GO

-- اعمال محدودیت مدت زمان خاص برای خاموشیهای باموافقت (پیش فرض محدودیت زمانی فقط برای خاموشیهای برنامه ریزی شده اعمال شده است)
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 430)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(430,N'IsBaMovafeghatLimit',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 430) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (430,N''IsBaMovafeghatLimit'',N''True'',NULL)')
GO

-- ذخیره شدن آدرس مناطق حساسی که خاموش میگردند نرم افزار درخواست خاموشی، در فیلد آدرس نرم افزار ثبت حوادث
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 431)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(431,N'IsUseCriticalsAddressForDC',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 431) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (431,N''IsUseCriticalsAddressForDC'',N''True'',NULL)')
GO

-- ارسال پیامک به مشترکینی که از طرف GIS و با توجه به کلیدهای قطع شده خاموش میگردند
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 432)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(432,N'IsSendGISSubscriberDCInfo',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 432) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (432,N''IsSendGISSubscriberDCInfo'',N''True'',NULL)')
GO

-- حداکثر مدت زمان رسیدن اطلاعات مشترکین GIS ، از زمان درخواست به دقیقه
IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 433) 
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(433,N'DurationOfGetGISSubscriber',N'60')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) 
VALUES ('Tbl_Config',148,0,4,NULL,99,GETDATE(),'IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 433) INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(433,N''DurationOfGetGISSubscriber'',N''60'')')
GO

-- پیش فرض نوع موافقت درخواست خاموشی
IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 434) 
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(434,N'TamirType',N'1')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) 
VALUES ('Tbl_Config',148,0,4,NULL,99,GETDATE(),'IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 434) INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(434,N''TamirType'',N''1'')')
GO

-- عدم اجباری شدن دلیل اضطراری بودن خاموشی
IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 435) 
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(435,N'IsForceEmergencyReason',N'False')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) 
VALUES ('Tbl_Config',148,0,4,NULL,99,GETDATE(),'IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 435) INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(435,N''IsForceEmergencyReason'',N''False'')')
GO

-- عدم چک کردن مدت زمان انجام عملیات خط گرم و مدت زمان عملیات انتخاب شده در فهرست عملیات
IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 436) 
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(436,N'IsCheckWarmLineOperationTime',N'False')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) 
VALUES ('Tbl_Config',148,0,4,NULL,99,GETDATE(),'IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 436) INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(436,N''IsCheckWarmLineOperationTime'',N''False'')')
GO

-- اضافه شدن گزينه اطلاع رساني از طريق نرم افزار موبايل در هنگام تاييد خاموشي
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 437)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(437,N'IsSendToInformApp',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 437) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (437,N''IsSendToInformApp'',N''True'',NULL)')
GO
--  اجباری شدن نوع قطع کننده در ثبت درخواست اجازه کار خاموشي با برنامه 
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = 'IsForceTamirMPCloserType')
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(438,N'IsForceTamirMPCloserType',N'True',NULL)
GO
INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) 
VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = N''IsForceTamirMPCloserType'') INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (438,N''IsForceTamirMPCloserType'',N''True'',NULL)')
GO
--  اجباری شدن آدرس محل کار در ثبت درخواست اجازه کار خاموشي با برنامه
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = 'IsForceWorkingAddress')
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(439,N'IsForceWorkingAddress',N'True',NULL)
GO
INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) 
VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = N''IsForceWorkingAddress'') INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (439,N''IsForceWorkingAddress'',N''True'',NULL)')
GO
--اجباری شدن آدرس مناطقی که خاموش می گردد در ثبت درخواست اجازه کار خاموشی با برنامه -درحالت عادی با انتخاب خط گرم انتخاب محدود حادثه غیرفعال است.
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = 'IsForceCriticalsAddress')
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(440,N'IsForceCriticalsAddress',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = ''IsForceCriticalsAddress'') INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (440,N''IsForceCriticalsAddress'',N''True'',NULL)')
GO

-- اجباری بودن کلید زنی در هنگام قطع در فرم درخواست خاموشی با برنامه
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = 'IsForceSelectMPFeederKey')
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(441,N'IsForceSelectMPFeederKey',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand)
 VALUES ('Tbl_Config',148,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = ''IsForceSelectMPFeederKey'') INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (441,N''IsForceSelectMPFeederKey'',N''True'',NULL)')
GO

-- ارسال پيامک تاييد خاموشی برای همه نواحی فیدرهای مشترک
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = 'IsSendSMSConfirmForCommonFeederArea')
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(442,N'IsSendSMSConfirmForCommonFeederArea',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand)
 VALUES ('Tbl_Config',148,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigName = ''IsSendSMSConfirmForCommonFeederArea'') INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (442,N''IsSendSMSConfirmForCommonFeederArea'',N''True'',NULL)')
GO

-- اجباري شدن تاييد ناظر قبل از تاييد خاموشي
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 443)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(443,N'IsForceTaeedNazer',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 443) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (443,N''IsForceTaeedNazer'',N''True'',NULL)')
GO

-- اجباري شدن تاييد خط گرم، توسط کاربر خط گرم
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 444)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(444,N'IsForceWarmLineConfirm',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 444) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (444,N''IsForceWarmLineConfirm'',N''True'',NULL)')
GO

-- اجباری شدن ثبت شماره موبایل پیمانکار در هنگام درخواست خاموشی 
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 445)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(445,N'IsForcePeymankarTel',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 445) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (445,N''IsForcePeymankarTel'',N''True'',NULL)')
GO

-- چک نمودن مدت زمان درخواستی در پرونده های عودت داده شده در هنگام ویرایش - پیش فرض محدودیت زمانی برای پرونده های عودت داده شده چک نمی گردد 
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 446)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(446,N'IsCheckReturnTimeInNewTamir',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 446) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (446,N''IsCheckReturnTimeInNewTamir'',N''True'',NULL)')
GO

-- استفاده از عدد سهمیه مازاد استفاده شده در دوره قبل، هنگام ثبت درخواست خاموشی بابرنامه
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 447)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(447,N'UseLastExtraPower',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 447) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (447,N''UseLastExtraPower'',N''True'',NULL)')
GO

--قابلیت فعال/غیرفعال بودن تاریخ شروع عملیات عودت داده شده.
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 448)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(448,N'FromDateChangeableReturned',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 448) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (448,N''FromDateChangeableReturned'',N''True'',NULL)')
GO


--امکان تغییر نوع موافقت در خاموشی های عودتی مخصوص ستاد
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 449)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(449,N'CanChangeTamirTypeForIsReturned',N'False',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 449) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (449,N''CanChangeTamirTypeForIsReturned'',N''False'',NULL)')
GO

/*------------ PM ------------*/
/*  نرم افزار بازديد و سرويس  */
/*----------------------------*/

-- دسترسي به ارتباط با اندرويد در بازديد سرويس
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 500)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (500,N'AndroidOS',N'OK',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 500) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (500,N''AndroidOS'',N''OK'',NULL)')
GO
-- فعال و غير فعال نمودن دسترسي به اندرويد
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 500)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (500,N'AndroidOS',N'OK',NULL)
ELSE
	DELETE Tbl_Config WHERE ConfigId = 500
GO
SELECT * FROM Tbl_Config WHERE ConfigId = 500

-- دسترسي به فرم اطلاعات جامع شبکه و گزارشات آن
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 501)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (501,N'IsShowNetworkInfo',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 501) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (501,N''IsShowNetworkInfo'',N''True'',NULL)')
GO

-- اجباري بودن پر نمودن اطلاعات تکميلي پست
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 502)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (502,N'IsForceLPPostExtraInfo',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 502) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (502,N''IsForceLPPostExtraInfo'',N''True'',NULL)')
GO

-- اجباري نبودن پر نمودن اطلاعات تکميلي خطوط
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 503)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (503,N'IsForceFeederExtraInfo',N'False',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 503) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (503,N''IsForceFeederExtraInfo'',N''False'',NULL)')
GO

-- تبديل چک ليستهاي توانير براي اراک
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 504)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(504,N'PMConvert',N'True')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 504) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (504,N''PMConvert'',N''True'',NULL)')
GO

-- دسترسي به نرم افزار بازديد سرويس
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 505)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(505,N'IsBSAccess',N'True')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 505) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue) VALUES (505,N''IsBSAccess'',N''True'')')
GO

-- اجباري شدن انتخاب تکه فيدر در هنگام ساخت مسير بازديد روي بخشي از خط فشار متوسط
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 506)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(506,N'IsForceFeederPart',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 506) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (506,N''IsForceFeederPart'',N''True'',NULL)')
GO

-- اجباري شدن انتخاب تکه فيدر در تعريف پست توزيع
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 507)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(507,N'IsForceFeederPartOnLPPost',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 507) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (507,N''IsForceFeederPartOnLPPost'',N''True'',NULL)')
GO

-- فعال شدن فيلدهاي مختصات جي.پي.اس در بخش اجراي بازديد
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 508)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(508,N'IsEnabledRunBazdidGPS',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 508) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (508,N''IsEnabledRunBazdidGPS'',N''True'',NULL)')
GO

-- فعال شدن امکان تغيير ناحيه ترانس‌هاي پست‌هاي توزيع
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 509)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(509,N'IsEnabledChangeLPTransArea',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 509) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (509,N''IsEnabledChangeLPTransArea'',N''True'',NULL)')
GO

-- امکان حذف سرویسهای بازدید تنها در صورت تایید در ستاد
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 510)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(510,N'IsEnableRequestForDelete',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 510) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (510,N''IsEnableRequestForDelete'',N''True'',NULL)')
GO

-- فعال شدن بازدیدهای تخصصی
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 512)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(512,N'BazdidSpeciality',N'Active',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 512) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (512,N''BazdidSpeciality'',N''Active'',NULL)')
GO

-- تغییر خودکار تعداد عیب دیده شده از روی تعداد عیب رفع شده در پنجره انجام سرویس
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 513)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(513,N'IsAutoUpdateCheckListCount',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 513) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (513,N''IsAutoUpdateCheckListCount'',N''True'',NULL)')
GO

-- اجباری شدن انتخاب ریز خرابی هنگام ثبت انجام سرویس
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 514)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(514,N'IsForceServiceSubCheckList',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 514) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (514,N''IsForceServiceSubCheckList'',N''True'',NULL)')
GO

-- غیر فعال شدن نرم افزارهای قدیمی بازديد سرويس
/* این کانفیگ در نسخه بروز رسانی دیتابیس روی سایت گذاشته شده است */
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 515)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(515,N'OldPMSoftExpDT',Convert(varchar,dateadd(dd,-1,GETDATE()),120),NULL)
GO

-- فعال شدن نقشه در نرم افزار بازدید و سرویس
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 516)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(516,N'IsActiveBazdidAppMap',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 516) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (516,N''IsActiveBazdidAppMap'',N''True'',NULL)')
GO


/*---------- Misc -----------*/
/*          متفـرقه          */
/*---------------------------*/

-- شناسه شرکت توزيع مخصوص توانير
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 600)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (600,N'ToziId',N'600',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 600) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (600,N''ToziId'',N''600'',NULL)')
GO

-- آخرين شناسه ارسال شده به ديتابيس تجميعي توانير
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 601)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(601,N'TavanirEventId',N'0',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 601) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (601,N''TavanirEventId'',N''0'',NULL)')
GO

-- شناسايي سرور به عنوان مستر
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 602)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (602,N'IsSlave',N'True',NULL)
GO
/* اين آيتم نياز به انتشار در نواحي ندارد */

-- شناسه واحد اجرایی انبارک فشار متوسط
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 603)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(603,N'MPReferToId',N'',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 603) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (603,N''MPReferToId'',N'''',NULL)')
GO

-- شناسه واحد اجرایی انبارک فشار ضعیف
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 604)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(604,N'LPReferToId',N'',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 604) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (604,N''LPReferToId'',N'''',NULL)')
GO

-- شناسه واحد اجرایی انبارک روشنایی معابر
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 605)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(605,N'LIReferToId',N'',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 605) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (605,N''LIReferToId'',N'''',NULL)')
GO

-- شناسه واحد اجرایی انبارک سرویس PM
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 606)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(606,N'SRReferToId',N'',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 606) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (606,N''SRReferToId'',N'''',NULL)')
GO

-- فعالسازی لایسنس استفاده از انبارک در نرم افزار
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 607)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(607,N'IsEnableStore',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 607) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (607,N''IsEnableStore'',N''True'',NULL)')
GO

-- پیام مرتبط با زمان وصل نامعلوم در پرتال هوشمند سازی
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 608)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(608,N'UnKnownConnectTimeMessage',N'هنوز زمان وصل مشخص نشده است',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 608) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (608,N''UnKnownConnectTimeMessage'',N''هنوز زمان وصل مشخص نشده است'',NULL)')
GO

-- محتوای ستون زمان وصل نامعلوم در پرتال هوشمند سازی
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 609)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(609,N'UnKnownConnectTime',N'نامعلوم',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 609) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (609,N''UnKnownConnectTime'',N''نامعلوم'',NULL)')
GO

-- ارسال پيامک وصل خاموشي هاي فوق توزيع به تفکيک فيدر بعد از وصل شدن هر فيدر
-- IsSendSeparateMPFeederConnect
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 610)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) 
	VALUES(610,N'IsSendSeparateMPFeederConnect','True',NULL)
GO

--------------------------------------------------------  غیرفعال شدن فیلدهای دریافت شده از طریق جی.آی.اس --------------------------------------------------
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 611)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(611,N'GIS_DisableControl',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 611) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (611,N''GIS_DisableControl'',N''True'',NULL)')
GO

IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 612)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(612,N'GIS_Disable_MPPost',NULL,N'cboArea,txtMPPost,cboVoltageIn,cboVoltageOut')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 612) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (612,N''GIS_Disable_MPPost'',NULL,N''cboArea,txtMPPost,cboVoltageIn,cboVoltageOut'')')
GO

IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 613)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(613,N'GIS_Disable_MPPostTrans',NULL,N'cboMPPost,txtMPPostTrans,cboVoltageIn,cboVoltageOut,TextBox1')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 613) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (613,N''GIS_Disable_MPPostTrans'',NULL,N''cboMPPost,txtMPPostTrans,cboVoltageIn,cboVoltageOut,TextBox1'')')
GO

IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 614)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(614,N'GIS_Disable_MPFeeder',NULL,N'cboArea,cboMPPost,txtMPFeeder,cboMPPostTrans,txtZaminiLen,txtHavayiLen,cmbOwnership')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 614) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (614,N''GIS_Disable_MPFeeder'',NULL,N''cboArea,cboMPPost,txtMPFeeder,cboMPPostTrans,txtZaminiLen,txtHavayiLen,cmbOwnership'')')
GO

IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 615)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(615,N'GIS_Disable_MPFeederKey',NULL,N'cmbMainArea,cmbMainMPPost,cmbMainMPFeeder,txtAddress,txtComment')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 615) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (615,N''GIS_Disable_MPFeederKey'',NULL,N''cmbMainArea,cmbMainMPPost,cmbMainMPFeeder,txtAddress,txtComment'')')
GO

IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 616)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(616,N'GIS_Disable_LPPost',NULL,N'cboArea,cboMPPost,cboMPFeeder,cmbFeederPart,txtLPPost,cmbOwnership,cmbLPPostType,cmbUseType,txtBuildYear,cboLPPostType,txtPostCapacity,btnAddTrans,btnEditLPTrans,btnDeleteTrans')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 616) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (616,N''GIS_Disable_LPPost'',NULL,N''cboArea,cboMPPost,cboMPFeeder,cmbFeederPart,txtLPPost,cmbOwnership,cmbLPPostType,cmbUseType,txtBuildYear,cboLPPostType,txtPostCapacity,btnAddTrans,btnEditLPTrans,btnDeleteTrans'')')
GO

IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 617)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(617,N'GIS_Disable_LPTrans',NULL,N'cmbArea,cmbPower,txtBuildYear,txtSerialNumber,chkIsSelikajel,cmbTabCount,cmbTransType')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 617) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (617,N''GIS_Disable_LPTrans'',NULL,N''cmbArea,cmbPower,txtBuildYear,txtSerialNumber,chkIsSelikajel,cmbTabCount,cmbTransType'')')
GO

IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 618)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(618,N'GIS_Disable_LPFeeder',NULL,N'cboArea,cboMPPost,cboMPFeeder,cboLPPost,cmbLPTrans,txtLPFeeder,cmbOwnership,chkIsLightFeeder,txtAverageCurrent')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 618) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (618,N''GIS_Disable_LPFeeder'',NULL,N''cboArea,cboMPPost,cboMPFeeder,cboLPPost,cmbLPTrans,txtLPFeeder,cmbOwnership,chkIsLightFeeder,txtAverageCurrent'')')
GO
------------------------------------------------------------------------------------------------------------------------------------------------------------

-- غیر فعال شدن فیلدهای دلخواه از فرم دلخواه در نرم افزار
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 619)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(619,N'FormsLockFields',NULL,N'[{"FormName":"frmBS_MPPost","LockFields":["txtMPPostCode","txtMPPost","cboVoltageIn","cboVoltageOut","TextBox2","TextBox3","cboMPPostOwnership"]}]')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 619) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (619,N''FormsLockFields'',NULL,N''[{"FormName":"frmBS_MPPost","LockFields":["txtMPPostCode","txtMPPost","cboVoltageIn","cboVoltageOut","TextBox2","TextBox3","cboMPPostOwnership"]}]'')')
GO

--   فعالسازی بخش رصد ترانسفورماتور در نرم افزار ثبت حوادث
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 620)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(620,N'IsLPTransMonitoringAccess',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 620) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (620,N''IsLPTransMonitoringAccess'',N''True'',NULL)')
GO
 
-- فعالسازی گزارش 3-37 در نرم افزار ثبت حوادث
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 621)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(621,N'IsShow_Rpt_3_37',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 621) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (621,N''IsShow_Rpt_3_37'',N''True'',NULL)')
GO

-- فعالسازی آیتم «تنظیمات امنیتی» در منوی سامانه نرم افزار
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 622)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(622,N'NoSCU',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 622) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (622,N''NoSCU'',N''True'',NULL)')
GO


-- ثبت کد توزیع با توجه به دستورالعمل هاب توانیر
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 623)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(623,N'ToziCode',NULL,NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 623) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (623,N''ToziCode'',NULL,NULL)')
GO

-- فعالسازی لایسنس دسترسی به حالت تجمیعی نرم افزار در بخش پیکربندی
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 624)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(624,N'TajmiMode',N'_active_',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 624) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (624,N''TajmiMode'',N''_active_'',NULL)')
GO

-- فعالسازی گزارشات پیش بینی (داده کاوی) در نرم افزار ثبت حوادث
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 625)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(625,N'IsShowExtractionReports',N'_Active_',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 625) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (625,N''IsShowExtractionReports'',N''_Active_'',NULL)')
GO

-- نام شرکت توزیع در فکس اطلاع رسانی مشترکین
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 626)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(626,N'FaxToziName',N'نام شرکت توزیع',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 626) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (626,N''FaxToziName'',N''نام شرکت توزیع'',NULL)')
GO

-- جانمایی متن اطلاع رسانی و نام شرکت توزیع در فکس ارسالی به مشترکین
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 627)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(627,N'FaxPaperInfo',N'{"TextX":50,"TextY":225,"TitleX":600,"TitleY":100}',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 627) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (627,N''FaxPaperInfo'',N''{"TextX":50,"TextY":225,"TitleX":600,"TitleY":100}'',NULL)')
GO

IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 628)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(628,N'IsServiceDBLogActive',N'False',N'Info : Enables CLogDB logging.')
GO

--------------------------------------------------------

-- مخصوص مازندران
IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 300) 
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(300,N'IsMazandaran',N'True')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) 
VALUES ('Tbl_Config',148,0,4,NULL,99,GETDATE(),'IF NOT Exists (SELECT * FROM Tbl_Config WHERE ConfigId = 300) INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue) VALUES(300,N''IsMazandaran'',N''True'')')
GO

-- تنظيم پيش فرض گزارش برق غير مجاز در ثبت اوليه
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 1000)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue) VALUES (1000,N'CallReason_Illegal',N'1000')
ELSE
	UPDATE Tbl_Config SET ConfigName = N'CallReason_Illegal',ConfigValue = N'1000'WHERE ConfigId = 1000
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 1000) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue) VALUES (1000,N''CallReason_Illegal'',N''1000'') ELSE UPDATE Tbl_Config SET ConfigName = N''CallReason_Illegal'',ConfigValue = N''1000''WHERE ConfigId = 1000')
GO

-- شناسه معادل برق غیر مجاز در واحدهای اجرایی
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 1001)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(1001,N'ReferTo_Illegal',N'7',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 1001) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (1001,N''ReferTo_Illegal'',N''7'',NULL)')
GO

-- فهرست دلایل تماس مربوط به افت ولتاژ در پرونده های ارجاع شده - وب سرویس شمال کرمان
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 1002)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(1002,N'CallReason_VoltageDrop',N'3,990088935,2',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 1002) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (1002,N''CallReason_VoltageDrop'',N''3,990088935,2'',NULL)')
GO



/* ------------------------------- HOMA ---------------------------------------- */

-- فعال شدن طرح هما در توزیع برق
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3008)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(3008,N'HomaIsActive',N'True',NULL)
GO
INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3008) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (3008,N''HomaIsActive'',N''True'',NULL)')
GO

--
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3000)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(3000,N'HomaLastEventId',N'0',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3000) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (3000,N''HomaLastEventId'',N''0'',NULL)')
GO

--
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3001)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(3001,N'HomaSipAddress',N'192.168.101.55',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3001) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (3001,N''HomaSipAddress'',N''192.168.101.55'',NULL)')
GO

--
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3002)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(3002,N'HomaSipIsUDP',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3002) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (3002,N''HomaSipIsUDP'',N''True'',NULL)')
GO

--
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3003)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(3003,N'HomaSipPort',N'5060',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3003) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (3003,N''HomaSipPort'',N''5060'',NULL)')
GO

IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3004)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(3004,N'HomaSipPass',N'',NULL)
GO

--
INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3004) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (3004,N''HomaSipPass'',N'''',NULL)')
GO

--
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3005)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(3005,N'HomaSipPrefix',N'9',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3005) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (3005,N''HomaSipPrefix'',N''9'',NULL)')
GO

--
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3006)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(3006,N'HomaTabletLicense',N'01000000CCE50CD68556B4D86EF4C7DA659896BED5552874B6BEB725',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3006) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (3006,N''HomaTabletLicense'',N''01000000CCE50CD68556B4D86EF4C7DA659896BED5552874B6BEB725'',NULL)')
GO

--
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3007)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(3007,N'HomaWorkingAreas',N'1,3',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3007) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (3007,N''HomaWorkingAreas'',N''1,3'',NULL)')
GO

IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3010)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(3010,N'HomaCallReason1',N'11',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3010) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (3010,N''HomaCallReason1'',N''11'',NULL)')
GO

IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3011)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(3011,N'HomaCallReason2',N'3',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3011) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (3011,N''HomaCallReason2'',N''3'',NULL)')
GO

IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3012)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(3012,N'HomaCallReason3',N'6',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3012) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (3012,N''HomaCallReason3'',N''6'',NULL)')
GO

IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3013)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(3013,N'HomaReferToId1',N'990181894',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3013) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (3013,N''HomaReferToId1'',N''990181894'',NULL)')
GO

IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3014)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(3014,N'HomaReferToId2',N'5',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3014) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (3014,N''HomaReferToId2'',N''5'',NULL)')
GO

IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3015)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(3015,N'HomaReferToId3',N'5',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3015) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (3015,N''HomaReferToId3'',N''5'',NULL)')
GO

-- غیر اجباری شدن انتخاب فیدر فشار ضعیف در تبلت
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3016)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(3016,N'Homa_IsNotForceLPFeeder',N'True',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3016) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (3016,N''Homa_IsNotForceLPFeeder'',N''True'',NULL)')
GO

-- تعداد تبلتهای فعال اپراتور 121
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3018)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(3018,N'HomaOperatorTabletLicense',N'01000000CCE50CD68556B4D86EF4C7DA659896BED5552874B6BEB725',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3018) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (3018,N''HomaOperatorTabletLicense'',N''01000000CCE50CD68556B4D86EF4C7DA659896BED5552874B6BEB725'',NULL)')
GO

-- علت تماس پیش فرض در خاموشیهای ثبت شده از طریق IVR
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3019)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(3019,N'HomaOutageCallReason',N'1',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3019) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (3019,N''HomaOutageCallReason'',N''1'',NULL)')
GO

-- فعالسازی OSM
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3020)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(3020,N'IsOSMActive',N'_active_',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3020) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (3020,N''IsOSMActive'',N''_active_'',NULL)')
GO
---

/* ------------------------------- GIS MAP ---------------------------------------- */
--
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3500)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(3500,N'GISToken',N'qk1iWABmZP-DHBZIKGiaPDpKQPEz8mA_YwYs7hi5_KP-E-61HqyHV4_hFdSVopOk',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3500) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (3500,N''GISToken'',N''qk1iWABmZP-DHBZIKGiaPDpKQPEz8mA_YwYs7hi5_KP-E-61HqyHV4_hFdSVopOk'',NULL)')
GO

--
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3501)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(3501,N'GISEnvelopExtent',N'5655978.829570528*4227844.161013588*5778278.074826779*4287923.665245721',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3501) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (3501,N''GISEnvelopExtent'',N''5655978.829570528*4227844.161013588*5778278.074826779*4287923.665245721'',NULL)')
GO

--
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3502)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(3502,N'GISWmsUrl',N'',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3502) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (3502,N''GISWmsUrl'',N'''',NULL)')
GO

--
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3503)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(3503,N'GISX',N'111',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3503) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (3503,N''GISX'',N''111'',NULL)')
GO

--
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3504)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(3504,N'GISY',N'11',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3504) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (3504,N''GISY'',N''11'',NULL)')
GO

--
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3505)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(3505,N'GISZoom',N'12',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3505) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (3505,N''GISZoom'',N''12'',NULL)')
GO

--
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3506)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(3506,N'GISScale',N'12',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3506) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (3506,N''GISScale'',N''12'',NULL)')
GO

--
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3507)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(3507,N'GISUserName',N'-',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3507) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (3507,N''GISUserName'',N''-'',NULL)')
GO

--
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3508)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(3508,N'GISPassword',N'-',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3508) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (3508,N''GISPassword'',N''-'',NULL)')
GO

--
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3509)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(3509,N'GISTokenUrl',N'-',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3509) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (3509,N''GISTokenUrl'',N''-'',NULL)')
GO

--
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3510)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(3510,N'GISWmsLayerNames',N'-',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3510) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (3510,N''WmsLayerNames'',N''-'',NULL)')
GO

--
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3511)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(3511,N'GISBingKey',N'AjmZ9JR1nuGAJRBLnsGS4kpbwfNBCIx8PLCfYU9OsHhhRMGttqU8b9VSBSyu77fX',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3511) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (3511,N''GISBingKey'',N''AjmZ9JR1nuGAJRBLnsGS4kpbwfNBCIx8PLCfYU9OsHhhRMGttqU8b9VSBSyu77fX'',NULL)')
GO

--
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3512)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(3512,N'GISProvider',N'Arc',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3512) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (3512,N''GISProvider'',N''Arc'',NULL)')
GO

--
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3513)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(3513,N'GISIsSecure',N'false',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3513) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (3513,N''GISIsSecure'',N''false'',NULL)')
GO

--
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3514)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(3514,N'GISArcUrl',N'http://192.168.10.228:8000/',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 3514) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (3514,N''GISArcUrl'',N''http://192.168.10.228:8000/'',NULL)')
GO

----------------------------------------------- Tavanir Portal ReferTo -----------------------------------------------
PortalVoltageReferToId = 4801 
PortalDamageReferToId = 4802
PortalTheftReferToId = 4803
PortalPeopleDamageReferToId = 4804
PortalFrontageReferToId = 4805
PortalDangerReferToId = 4806
PortalGovernmentReferToId = 4807
PortalPlannedReferToId = 4808
----------------------------------------------- END -----------------------------------------------


/*---------------------------*/
/*---------------------------*/
/*---------------------------*/
/* 
-- ؟؟؟
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = xxx)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) VALUES(xxx,N'ConfigNameStr',N'ConfigValueStr',NULL)
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = xxx) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (xxx,N''ConfigNameStr'',N''ConfigValueStr'',NULL)')
GO
*/
---
