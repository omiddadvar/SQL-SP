/* ----------------------- */
/*  SMS Records: 97/02/03  */
/* ----------------------- */
/*     Version: 2.1.3      */
/* ----------------------- */
Use [CcRequesterSetad]
GO

-- SMSTamirConfirm
-- CriticalsAddress = آدرس مناطقي که خاموش مي گردند
-- DBAddress = آدرس محل کار
DECLARE @IsUpdate AS BIT SET @IsUpdate = 0
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 21)
INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (21,N'SMSTamirConfirm',NULL,N'درخواست خاموشي TamirReqNo با مشخصات:
MPPostInfo MPFeederInfo MPFeedersInfo LPPostInfo FeederPartInfo LPFeederInfo تاريخ :DTDateساعت:DTTime
مدت:Minutes
ناحيه:Area
آدرس:DBAddress
Status')
ELSE IF @IsUpdate = 1
	UPDATE Tbl_Config 
	SET ConfigText = N'درخواست خاموشي TamirReqNo با مشخصات:
MPPostInfo MPFeederInfo MPFeedersInfo LPPostInfo FeederPartInfo LPFeederInfo تاريخ :DTDateساعت:DTTime
مدت:Minutes
ناحيه:Area
آدرس:DBAddress
Status'
	WHERE ConfigId = 21
GO


--SMSMPFDCnTime
--فيدر FeederName از پست فوق توزيع MPPostName ناحيه Area در ماه جاري تاکنون به تعداد Count بار قطع گرديده است.

DECLARE @IsUpdate AS BIT SET @IsUpdate = 0
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 22)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) 
	VALUES (22,N'SMSMPFDCnTime',NULL,N'فيدر FeederName از پست فوق توزيع MPPostName ناحيه Area در ماه جاري تاکنون به تعداد Count بار قطع گرديده است.')
ELSE IF @IsUpdate = 1
	UPDATE Tbl_Config
	SET ConfigText = N'فيدر FeederName از پست فوق توزيع MPPostName ناحيه Area در ماه جاري تاکنون به تعداد Count بار قطع گرديده است.'
	WHERE ConfigId = 22
GO

--SMSAfterEdit
--پرونده ReqType با شماره ReqNo توسط کاربر Username در تاريخ Date ساعت Time به دليل Comment تغيير کرد.

DECLARE @IsUpdate AS BIT SET @IsUpdate = 0
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 23)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) 
	VALUES (23,N'SMSAfterEdit',NULL,N'پرونده ReqType با شماره ReqNo توسط کاربر Username در تاريخ Date ساعت Time به دليل Comment تغيير کرد.')
GO


--SMSDGSRequest
--خاموشي ReqType در ناحيه Area روي PostFeeder به علت Reason در تاريخ Date ساعت Time در DCGroup اتفاق افتاده است.

DECLARE @IsUpdate AS BIT SET @IsUpdate = 0
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 24)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) 
	VALUES (24,N'SMSDGSRequest',NULL,N'خاموشي ReqType در ناحيه Area روي PostFeeder به علت Reason در تاريخ Date ساعت Time در DCGroup اتفاق افتاده است.')
GO

--SMSDailyPeak
--آخرين پيک شرکت به ميزان PeakValue مگاوات مورخ PeakDate با افزايش Percent درصد نسبت به سال گذشته و بالاترين پيک تاکنون MaxPeak مگاوات مورخ MaxPeakDate مي‌باشد.

DECLARE @IsUpdate AS BIT SET @IsUpdate = 0
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 25)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) 
	VALUES (25,N'SMSDailyPeak',NULL,N'آخرين پيک شرکت به ميزان PeakValue مگاوات مورخ PeakDate با افزايش Percent درصد نسبت به سال گذشته و بالاترين پيک تاکنون MaxPeak مگاوات مورخ MaxPeakDate مي‌باشد.')
GO

--SMSSerghat
--Helloتجهيزات زير در ناحيه Area توسط واحد ReferTo مورخ Date در پرونده ReqType با شماره ReqNo به عنوان سرقتي ثبت شدند:PartList

DECLARE @IsUpdate AS BIT SET @IsUpdate = 0
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 26)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) 
	VALUES (26,N'SMSSerghat',NULL,N'Helloتجهيزات زير در ناحيه Area توسط واحد ReferTo مورخ Date در پرونده ReqType با شماره ReqNo به عنوان سرقتي ثبت شدند:PartList')
ELSE IF @IsUpdate = 1
	UPDATE Tbl_Config 
	SET ConfigText = N'Helloتجهيزات زير در ناحيه Area توسط واحد ReferTo مورخ Date در پرونده ReqType با شماره ReqNo به عنوان سرقتي ثبت شدند:PartList'
	WHERE ConfigId = 26
GO

--SMSMPNotTamirPost
--پست توزيع LPPostName از فيدر MPFeederName با بار CurrentValue آمپر در ساعت DTTime قطع گرديده و با گذشت بيش از Minutes دقيقه از زمان قطع، وصل نگرديده است.

DECLARE @IsUpdate AS BIT SET @IsUpdate = 0
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 27)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) 
	VALUES (27,N'SMSMPNotTamirPost',NULL,N'پست توزيع LPPostName از فيدر MPFeederName با بار CurrentValue آمپر در ساعت DTTime قطع گرديده و با گذشت بيش از Minutes دقيقه از زمان قطع، وصل نگرديده است.')
GO

--SMSMPTamirPost
--پست توزيع LPPostName از فيدر MPFeederName بيش از Minutes دقيقه به علت Reason بابرنامه قطع ميباشد

DECLARE @IsUpdate AS BIT SET @IsUpdate = 0
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 28)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) 
	VALUES (28,N'SMSMPTamirPost',NULL,N'پست توزيع LPPostName از فيدر MPFeederName بيش از Minutes دقيقه به علت Reason بابرنامه قطع ميباشد')
GO

--SMSMPTamirAfterConnectPost
--پست توزيع LPPostName از فيدر MPFeederName پس از Minutes دقيقه با علت Reason و بار CurrentValueConnect آمپر وصل گرديد

DECLARE @IsUpdate AS BIT SET @IsUpdate = 0
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 29)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) 
	VALUES (29,N'SMSMPTamirAfterConnectPost',NULL,N'پست توزيع LPPostName از فيدر MPFeederName پس از Minutes دقيقه با علت Reason و بار CurrentValueConnect آمپر وصل گرديد')
ELSE IF @IsUpdate = 1
	UPDATE Tbl_Config 
	SET ConfigText = N'پست توزيع LPPostName از فيدر MPFeederName پس از Minutes دقيقه با علت Reason و بار CurrentValueConnect آمپر وصل گرديد'
	WHERE ConfigId = 29
GO

--SMSMPNotTamirAfterConnectPost
--پست توزيع LPPostName از فيدر MPFeederName که در ساعت DTTime قطع شده بود، پس از Minutes دقيقه با بار CurrentValueConnect آمپر وصل گرديد.

DECLARE @IsUpdate AS BIT SET @IsUpdate = 0
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 30)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) 
	VALUES (30,N'SMSMPNotTamirAfterConnectPost',NULL,N'پست توزيع LPPostName از فيدر MPFeederName که در ساعت DTTime قطع شده بود، پس از Minutes دقيقه با بار CurrentValueConnect آمپر وصل گرديد.')
ELSE IF @IsUpdate = 1
	UPDATE Tbl_Config 
	SET ConfigText = N'پست توزيع LPPostName از فيدر MPFeederName که در ساعت DTTime قطع شده بود، پس از Minutes دقيقه با بار CurrentValueConnect آمپر وصل گرديد.'
	WHERE ConfigId = 30
GO

--SMSMPNotTamirFeederPart
--سرخط فيدر MPFeederName از پست فوق توزيع MPPostName با بار CurrentValue در ساعت DTTime قطع گرديده (MPStatus) و با گذشت بيش از Minutes دقيقه از زمان قطع، وصل نگرديده است.CRLFFFeederPartromType FromValue ToType ToValue

DECLARE @IsUpdate AS BIT SET @IsUpdate = 0
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 31)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) 
	VALUES (31,N'SMSMPNotTamirFeederPart',NULL,N'سرخط فيدر MPFeederName از پست فوق توزيع MPPostName با بار CurrentValue در ساعت DTTime قطع گرديده (MPStatus) و با گذشت بيش از Minutes دقيقه از زمان قطع، وصل نگرديده است.CRLFFeederPartFromType FromValue ToType ToValue')
ELSE IF @IsUpdate = 1
	UPDATE Tbl_Config 
	SET ConfigText = N'سرخط فيدر MPFeederName از پست فوق توزيع MPPostName با بار CurrentValue در ساعت DTTime قطع گرديده (MPStatus) و با گذشت بيش از Minutes دقيقه از زمان قطع، وصل نگرديده است.CRLFFeederPartFromType FromValue ToType ToValue'
	WHERE ConfigId = 31
GO

--SMSMPTamirFeederPart
--سرخط فيدر MPFeederName از پست فوق توزيع MPPostName بيش از Minutes دقيقه به علت Reason بابرنامه قطع ميباشد (MPStatus)CRLFFeederPartFromType FromValue ToType ToValue

DECLARE @IsUpdate AS BIT SET @IsUpdate = 0
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 32)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) 
	VALUES (32,N'SMSMPTamirFeederPart',NULL,N'سرخط فيدر MPFeederName از پست فوق توزيع MPPostName بيش از Minutes دقيقه به علت Reason بابرنامه قطع ميباشد (MPStatus)CRLFFeederPartFromType FromValue ToType ToValue')
ELSE IF @IsUpdate = 1
	UPDATE Tbl_Config 
	SET ConfigText = N'سرخط فيدر MPFeederName از پست فوق توزيع MPPostName بيش از Minutes دقيقه به علت Reason بابرنامه قطع ميباشد (MPStatus)CRLFFeederPartFromType FromValue ToType ToValue'
	WHERE ConfigId = 32
GO

--SMSMPNotTamirAfterConnectFeederPart
--خاموشي فشارمتوسط بي برنامه به شماره ReqNo روي سرخط فيدر MPFeederName (MPStatus)  از پست فوق توزيع MPPostName پس از Minutes دقيقه با علت Reason و بار CurrentValueConnect آمپر وصل گرديدCRLFFeederPartFromType FromValue ToType ToValue

DECLARE @IsUpdate AS BIT SET @IsUpdate = 0
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 33)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) 
	VALUES (33,N'SMSMPNotTamirAfterConnectFeederPart',NULL,N'خاموشي فشارمتوسط بي برنامه به شماره ReqNo روي سرخط فيدر MPFeederName (MPStatus)  از پست فوق توزيع MPPostName پس از Minutes دقيقه با علت Reason و بار CurrentValueConnect آمپر وصل وصل گرديدCRLFFeederPartFromType FromValue ToType ToValue')
ELSE IF @IsUpdate = 1
	UPDATE Tbl_Config 
	SET ConfigText = N'خاموشي فشارمتوسط بي برنامه به شماره ReqNo روي سرخط فيدر MPFeederName (MPStatus)  از پست فوق توزيع MPPostName پس از Minutes دقيقه با علت Reason و بار CurrentValueConnect آمپر وصل گرديدCRLFFeederPartFromType FromValue ToType ToValue'
	WHERE ConfigId = 33
GO

--SMSMPTamirAfterConnectFeederPart
--خاموشي فشارمتوسط بابرنامه به شماره ReqNo روي سرخط فيدر MPFeederName (MPStatus)  از پست فوق توزيع MPPostName که در ساعت DTTime قطع شده بود، پس از Minutes دقيقه با بار CurrentValueConnect آمپر وصل گرديد.CRLFFeederPartFromType FromValue ToType ToValue

DECLARE @IsUpdate AS BIT SET @IsUpdate = 0
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 34)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) 
	VALUES (34,N'SMSMPTamirAfterConnectFeederPart',NULL,N'خاموشي فشارمتوسط بابرنامه به شماره ReqNo روي سرخط فيدر MPFeederName (MPStatus)  از پست فوق توزيع MPPostName که در ساعت DTTime قطع شده بود، پس از Minutes دقيقه با بار CurrentValueConnect آمپر وصل گرديد.CRLFFeederPartFromType FromValue ToType ToValue')
ELSE IF @IsUpdate = 1
	UPDATE Tbl_Config 
	SET ConfigText = N'خاموشي فشارمتوسط بابرنامه به شماره ReqNo روي سرخط فيدر MPFeederName (MPStatus)  از پست فوق توزيع MPPostName که در ساعت DTTime قطع شده بود، پس از Minutes دقيقه با بار CurrentValueConnect آمپر وصل گرديد.CRLFFeederPartFromType FromValue ToType ToValue'
	WHERE ConfigId = 34
GO

--SMSSubAfterConnect
--مشترک گراميCRLF برق شما در ساعت ConnectTime وصل گرديد
/*
SMSBaBarnameh:(ارسال خاموشي با برنامه به مشتركين)
    CRLF=Carriage return & Linefeed
    yy=سال
    mm=ماه
    dd=روز
    hh=ساعت
    nn=دقيقه
    ss=مدت قطع
	Address=آدرس مشترک
	ConnectDate=تاريخ وصل واقعي
	ConnectTime=ساعت وصل واقعي
*/

DECLARE @IsUpdate AS BIT SET @IsUpdate = 0
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 35)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) 
	VALUES (35,N'SMSSubAfterConnect',NULL,N'مشترک گراميCRLF برق شما در ساعت ConnectTime وصل گرديد')
ELSE IF @IsUpdate = 1
	UPDATE Tbl_Config 
	SET ConfigText = N'مشترک گراميCRLF برق شما در ساعت ConnectTime وصل گرديد'
	WHERE ConfigId = 35
GO

IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 36)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (36,N'SMSLPTamirAll',NULL,N'رکورد فشار ضعيف با برنامه کلي به شماره ReqNo بيش از Minutes دقيقه به علت Reason قطع مي باشد. CRLFپست: LPPostName CRLFکد پست: LPPostCode CRLFآدرس: DBAddress CRLFمنطقه: Area')
GO

IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 37)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (37,N'SMSLPTamirSingle',NULL,N'رکورد فشار ضعيف با برنامه تکي به شماره ReqNo بيش از Minutes دقيقه به علت Reason قطع مي باشد. CRLFآدرس: DBAddress CRLFمنطقه: Area')
GO

IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 38)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (38,N'SMSLPNotTamirAll',NULL,N'رکورد فشار ضعيف بي برنامه کلي به شماره ReqNo بيش از Minutes دقيقه به علت Reason قطع مي باشد. CRLFپست: LPPostName CRLFکد پست: LPPostCode CRLFآدرس: DBAddress CRLFمنطقه: Area')
GO

IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 39)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (39,N'SMSLPNotTamirSingle',NULL,N'رکورد فشار ضعيف بي برنامه تکي به شماره ReqNo بيش از Minutes دقيقه به علت Reason قطع مي باشد. CRLFآدرس: DBAddress CRLFمنطقه : Area')
GO

IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 40)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (40,N'SMSNewRequest',NULL,N'خاموشي بي برنامه جديد به شماره ReqNo بيش از Minutes دقيقه قطع مي باشد. CRLFآدرس: DBAddress CRLFمنطقه : Area')
GO

IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 41)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (41,N'SMSLPTamirAllAfterConnect',NULL,N'رکورد فشار ضعيف با برنامه کلي به شماره ReqNo پس از Minutes دقيقه با علت Reason وصل گرديد. CRLFپست: LPPostName CRLFکد پست: LPPostCode CRLFآدرس: DBAddress CRLFمنطقه: Area')
GO

IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 42)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (42,N'SMSLPTamirSingleAfterConnect',NULL,N'رکورد فشار ضعيف با برنامه تکي به شماره ReqNo پس از Minutes دقيقه با علت Reason وصل گرديد. CRLFآدرس: DBAddress CRLFمنطقه: Area')
GO

IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 43)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (43,N'SMSLPNotTamirAllAfterConnect',NULL,N'رکورد فشار ضعيف بي برنامه کلي به شماره ReqNo پس از Minutes دقيقه با علت Reason وصل گرديد. CRLFپست: LPPostName CRLFکد پست: LPPostCode CRLFآدرس: DBAddress CRLFمنطقه: Area')
GO

IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 44)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (44,N'SMSLPNotTamirSingleAfterConnect',NULL,N'رکورد فشار ضعيف بي برنامه تکي به شماره ReqNo پس از Minutes دقيقه با علت Reason وصل گرديد. CRLFآدرس: DBAddress CRLFمنطقه: Area')
GO

IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 45)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (45,N'SMSMPTamirAll',NULL,N'فيدر MPFeederName از پست فوق توزيع MPPostName با بار CurrentValue در ساعت DTTime به علت Reason قطع گرديده است و با گذشت بيش از Minutes دقيقه از زمان قطع، وصل نگرديده است.')
GO

IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 46)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (46,N'SMSMPNotTamirAll',NULL,N'فيدر MPFeederName از پست فوق توزيع MPPostName با بار CurrentValue با عملکرد رله OCEFRelayAction در ساعت DTTime قطع گرديده است و با گذشت بيش از Minutes دقيقه از زمان قطع، وصل نگرديده است.')
GO

IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 47)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (47,N'SMSMPTamirAllAfterConnect',NULL,N'فيدر MPFeederName از پست فوق توزيع MPPostName که در ساعت DTTime به علت Reason قطع شده بود، پس از گذشت Minutes دقيقه وصل گرديد.')
GO

IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 48)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (48,N'SMSMPNotTamirAllAfterConnect',NULL,N'فيدر MPFeederName از پست فوق توزيع MPPostName که در ساعت DTTime به علت Reason قطع شده بود، پس از گذشت Minutes دقيقه وصل گرديد.')
GO

IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 49)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (49,N'SMSCriticalMPFeeders',NULL,N'ناحيه Area تعداد فيدر بحراني nCount نام فيدرها MPFeederList
')
GO

IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 50)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (50,N'SMSCriticalMPFeedersTitle',NULL,N'آمار فيدرهاي بحراني در پايان ماه Month سال Year:
')
GO

IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 51)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (51,N'SMSMonthlySubscriber',NULL,N'مدت زمان خاموشي هر مشترک در روز به دقيقه
در ماه MonthName : lIntervalMonth
از ابتداي سال تا پايان MonthName : lIntervalYear')
GO

-- CurrentValue = بار .. آمپر , ConnectDarsad = درصد برقدار شده , Comments = توضيحات وصل چندمرحله اي
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 52)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (52,N'SMSMultiStepConnectionMP',NULL,N'ConnectType تاريخ ConnectDatePersian ساعت ConnectTime بار CurrentValue آمپر')
GO

-- CurrentValue = بار .. آمپر , ConnectDarsad = درصد برقدار شده , Comments = توضيحات وصل چندمرحله اي
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 53)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (53,N'SMSMultiStepConnectionLP',NULL,N'ConnectType تاريخ ConnectDatePersian ساعت ConnectTime درصد برقدار شده ConnectDarsad')
GO

-- CurrentValue = بار .. آمپر , ConnectDarsad = درصد برقدار شده , Comments = توضيحات وصل چندمرحله اي
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 54)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (54,N'SMSMultiStepConnectionFT',NULL,N'فيدر MPFeederName ConnectType تاريخ ConnectDatePersian ساعت ConnectTime بار CurrentValue آمپر')
GO

-- SMS Config For Subscriber
-- SMSSubscriberLightRequest
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 55)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) 
	VALUES(55,N'SMSSubscriberLightRequest',NULL,N'مشترک گرامي
درخواست اصلاح روشنايي معابر شما در تاريخ DataEntryDTPersian ساعت DataEntryTime آدرس Address در سامانه پيگيري شرکت توزيع نيروي برق استان Province ثبت گرديد.
شماره پيگيري RequestNumber')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 55) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (55,N''SMSSubscriberLightRequest'',NULL,N''مشترک گرامي
درخواست اصلاح روشنايي معابر شما در تاريخ DataEntryDTPersian ساعت DataEntryTime آدرس Address در سامانه پيگيري شرکت توزيع نيروي برق استان Province ثبت گرديد.
شماره پيگيري RequestNumber'')')
GO

-- SMSSubscriberLightConnect
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 56)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) 
	VALUES(56,N'SMSSubscriberLightConnect',NULL,N'مشترک گرامي
درخواست اصلاح روشنايي معابر مربوط به آدرس Address با شماره پيگيري RequestNumber در تاريخ ConnectDatePersian انجام گرديد.')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 56) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (56,N''SMSSubscriberLightConnect'',NULL,N''مشترک گرامي
درخواست اصلاح روشنايي معابر مربوط به آدرس Address با شماره پيگيري RequestNumber در تاريخ ConnectDatePersian انجام گرديد.'')')
GO

-- SMSSubscriberLightConnect :  در صورتي که سامانه دريافت تأييديه از مشترکين فعال باشد
UPDATE Tbl_Config
SET ConfigText = N'مشترک گرامي
درخواست اصلاح روشنايي معابر مربوط به آدرس Address با شماره پيگيري RequestNumber در تاريخ ConnectDatePersian انجام گرديد.
در صورت عدم اصلاح روشنايي معابر، عدد يک را به اين سامانه پيامک نماييد.'
WHERE ConfigId = 56
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'UPDATE Tbl_Config SET ConfigText = N''مشترک گرامي 
درخواست اصلاح روشنايي معابر مربوط به آدرس Address با شماره پيگيري RequestNumber در تاريخ ConnectDatePersian انجام گرديد. 
در صورت عدم اصلاح روشنايي معابر، عدد يک را به اين سامانه پيامک نماييد.'' WHERE ConfigId = 56')
GO


-- PeopleVoiceSubscriberSMS
/*
DELETE FROM Tbl_Config WHERE ConfigId = 57
GO
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 57)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (57,N'PeopleVoiceSubscriberSMS',NULL,N'مشترک گرامي درخواست شما با شماره پيگيري RequestNumber اين مرکز ثبت گرديد و TimeoutDesc')
GO
*/


--SMSMPTamirPostSingle
--پست توزيع LPPostName از فيدر MPFeederName بيش از Minutes دقيقه به علت Reason بابرنامه قطع ميباشد

DECLARE @IsUpdate AS BIT SET @IsUpdate = 0
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 58)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) 
	VALUES (58,N'SMSMPTamirPostSingle',NULL,N'پست توزيع LPPostName از فيدر MPFeederName بيش از Minutes دقيقه به علت Reason بابرنامه قطع ميباشد')
GO

--SMSMPNotTamirPostSingle
--پست توزيع LPPostName از فيدر MPFeederName با بار CurrentValue آمپر در ساعت DTTime قطع گرديده و با گذشت بيش از Minutes دقيقه از زمان قطع، وصل نگرديده است.

DECLARE @IsUpdate AS BIT SET @IsUpdate = 0
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 59)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) 
	VALUES (59,N'SMSMPNotTamirPostSingle',NULL,N'پست توزيع LPPostName از فيدر MPFeederName با بار CurrentValue آمپر در ساعت DTTime قطع گرديده و با گذشت بيش از Minutes دقيقه از زمان قطع، وصل نگرديده است.')
GO

--SMSHighPowerMPFeedersTitle
--سال Year ماه MonthName 

IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 60)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) 
	VALUES (60,N'SMSHighPowerMPFeedersTitle',NULL,N'فيدرهاي داراي بيشترين انرژي توزيع نشده در پايان MonthName Year')
GO

--SMSHighPowerMPFeeders
--ناحيه Area فيدر MPFeederName انرژي توزيع نشده SumPower MWh تعداد قطع DCCount

DECLARE @IsUpdate AS BIT SET @IsUpdate = 0
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 61)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) 
	VALUES (61,N'SMSHighPowerMPFeeders',NULL,N'ناحيه Area فيدر MPFeederName انرژي توزيع نشده SumPower MWh تعداد قطع DCCount')
GO

/* -----------------------------------   BEGIN SMS FOR SUSCRIBERS REQUEST   -------------------------------------*/

-- SMSSubscriberNewRequest
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 62)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) 
	VALUES(62,N'SMSSubscriberNewRequest',NULL,N'سامانه فوريتهاي برق (121)
مشترک گرامي، درخواست پيگيري قطع برق شما گزارش شده در ساعت: DataEntryTime آدرس: Address با کد رهگيري RequestNumber در سامانه پيگيري شرکت توزيع نيروي برق استان Province ثبت گرديد')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 62) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (62,N''SMSSubscriberNewRequest'',NULL,N''سامانه فوريتهاي برق (121)
مشترک گرامي، درخواست پيگيري قطع برق شما گزارش شده در ساعت: DataEntryTime آدرس: Address با کد رهگيري RequestNumber در سامانه پيگيري شرکت توزيع نيروي برق استان Province ثبت گرديد'')')
GO

-- SMSSubscriberNotEkip1
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 63)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) 
	VALUES(63,N'SMSSubscriberNotEkip1',NULL,N'سامانه فوريتهاي برق (121):
مشترک گرامي، درخواست پيگيري قطع برق شما در ساعت: DisconnectTime آدرس: Address با کد رهگيري RequestNumber در حال پيگيري است.')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 63) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (63,N''SMSSubscriberNotEkip1'',NULL,N''سامانه فوريتهاي برق (121):
مشترک گرامي، درخواست پيگيري قطع برق شما در ساعت: DisconnectTime آدرس: Address با کد رهگيري RequestNumber در حال پيگيري است.'')')
GO

-- SMSSubscriberNotEkip2
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 64)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) 
	VALUES(64,N'SMSSubscriberNotEkip2',NULL,N'سامانه فوريتهاي برق (121): 
مشترک گرامي، بدليل حجم بالاي گزارشهاي خاموشي، درخواست پيگيري خاموشي گزارش شده در ساعت: DisconnectTime آدرس: Address با کد رهگيري RequestNumber در حال حاضر امکان پذير نبوده و همچنان در دستور پيگيري قرار دارد.')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 64) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (64,N''SMSSubscriberNotEkip2'',NULL,N''سامانه فوريتهاي برق (121): 
مشترک گرامي، بدليل حجم بالاي گزارشهاي خاموشي، درخواست پيگيري خاموشي گزارش شده در ساعت: DisconnectTime آدرس: Address با کد رهگيري RequestNumber در حال حاضر امکان پذير نبوده و همچنان در دستور پيگيري قرار دارد.'')')
GO

-- SMSSubscriberEzamEkip
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 65)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) 
	VALUES(65,N'SMSSubscriberEzamEkip',NULL,N'سامانه فوريتهاي برق (121): 
مشترک گرامي، جهت رسيدگي به درخواست پيگيري قطع برق شما در ساعت: DisconnectTime آدرس: Address با کد رهگيري RequestNumber گروه عمليات اعزام شده است لطفا جهت راهنمايي گروه در محل حضور داشته باشيد.')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 65) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (65,N''SMSSubscriberEzamEkip'',NULL,N''سامانه فوريتهاي برق (121): 
مشترک گرامي، جهت رسيدگي به درخواست پيگيري قطع برق شما در ساعت: DisconnectTime آدرس: Address با کد رهگيري RequestNumber گروه عمليات اعزام شده است لطفا جهت راهنمايي گروه در محل حضور داشته باشيد.'')')
GO

-- SMSSubscriberConnectRequest
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 66)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) 
	VALUES(66,N'SMSSubscriberConnectRequest',NULL,N'سامانه فوريتهاي برق (121): 
مشترک گرامي، درخواست پيگيري قطع برق شما در ساعت: DisconnectTime آدرس: Address در ساعت: ConnectTime با کد رهگيري RequestNumber انجام گرديد.')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 66) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (66,N''SMSSubscriberConnectRequest'',NULL,N''سامانه فوريتهاي برق (121): 
مشترک گرامي، درخواست پيگيري قطع برق شما در ساعت: DisconnectTime آدرس: Address در ساعت: ConnectTime با کد رهگيري RequestNumber انجام گرديد.'')')
GO

-- SMSSubscriberConnectRequest :  در صورتي که سامانه دريافت تأييديه از مشترکين فعال باشد
UPDATE Tbl_Config
SET ConfigText = N'سامانه فوريتهاي برق (121): 
مشترک گرامي، درخواست پيگيري قطع برق شما در ساعت: DisconnectTime آدرس: Address در ساعت: ConnectTime با کد رهگيري RequestNumber انجام گرديد.
در صورت رضايت از عملکرد گروههاي عملياتي، عدد يک و در صورت عدم رضايت، عدد دو را به اين سامانه پيامک نماييد.'
WHERE ConfigId = 66
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'UPDATE Tbl_Config SET ConfigText = N''سامانه فوريتهاي برق (121): 
مشترک گرامي، درخواست پيگيري قطع برق شما در ساعت: DisconnectTime آدرس: Address در ساعت: ConnectTime با کد رهگيري RequestNumber انجام گرديد. 
در صورت رضايت از عملکرد گروههاي عملياتي، عدد يک و در صورت عدم رضايت، عدد دو را به اين سامانه پيامک نماييد.'' WHERE ConfigId = 66')
GO

-- SMSSubscriberDuplicateTamir
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 67)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) 
	VALUES(67,N'SMSSubscriberDuplicateTamir',NULL,N'سامانه فوريتهاي برق (121): 
مشترک گرامي، برق شما به علت سرويس جهت بهبود نيرو رساني مربوط به  آدرس : Address از ساعت: DisconnectTime به مدت DisconnectInterval قطع است.')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 67) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (67,N''SMSSubscriberDuplicateTamir'',NULL,N''سامانه فوريتهاي برق (121): 
مشترک گرامي، برق شما به علت سرويس جهت بهبود نيرو رساني مربوط به  آدرس : Address از ساعت: DisconnectTime به مدت DisconnectInterval قطع است.'')')
GO

-- SMSSubscriberDuplicateNotTamir
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 68)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) 
	VALUES(68,N'SMSSubscriberDuplicateNotTamir',NULL,N'سامانه فوريتهاي برق (121): 
مشترک گرامي، برق شما به علت اشکال در شبکه برق مربوط به آدرس: Address، قطع و در حال پيگيري است.')
GO

INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand) VALUES ('Tbl_Area',46,1,4,NULL,99,GETDATE(),N'IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 68) INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (68,N''SMSSubscriberDuplicateNotTamir'',NULL,N''سامانه فوريتهاي برق (121): 
مشترک گرامي، برق شما به علت اشکال در شبکه برق مربوط به آدرس: Address، قطع و در حال پيگيري است.'')')
GO

/* -----------------------------------   END SMS FOR SUSCRIBERS REQUEST   -------------------------------------*/

-- SMSMultiStepConnectionAfterSendMP
-- CurrentValue = بار .. آمپر , ConnectDarsad = درصد برقدار شده , Comments = توضيحات وصل چندمرحله اي
DECLARE @IsUpdate AS BIT SET @IsUpdate = 0
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 69)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (69,N'SMSMultiStepConnectionAfterSendMP',NULL,N'بخشي از فيدر MPFeederName از پست MPPostName که در تاريخ DisconnectDatePersian و ساعت DisconnectTime با بار DCCurrentValue آمپر قطع شده بود، در ساعت ConnectTime، با مقدار بار CurrentValue آمپر، ConnectType وصل گرديد.
Comments')
ELSE IF @IsUpdate = 1
	UPDATE Tbl_Config SET ConfigText = N'بخشي از فيدر MPFeederName از پست MPPostName که در تاريخ DisconnectDatePersian و ساعت DisconnectTime با بار DCCurrentValue آمپر قطع شده بود، در ساعت ConnectTime، با مقدار بار CurrentValue آمپر، ConnectType وصل گرديد.
Comments'
	WHERE ConfigId = 69
GO

-- SMSMultiStepConnectionAfterSendLP
-- CurrentValue = بار .. آمپر , ConnectDarsad = درصد برقدار شده , Comments = توضيحات وصل چندمرحله اي
DECLARE @IsUpdate AS BIT SET @IsUpdate = 0
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 70)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (70,N'SMSMultiStepConnectionAfterSendLP',NULL,N'ConnectDarsad درصد از فيدر LPFeederName پست LPPostName که در تاريخ DisconnectDatePersian و ساعت DisconnectTime قطع شده بود، در ساعت ConnectTime، ConnectType وصل گرديد.
Comments')
ELSE IF @IsUpdate = 1
	UPDATE Tbl_Config SET ConfigText = N'ConnectDarsad درصد از فيدر LPFeederName پست LPPostName که در تاريخ DisconnectDatePersian و ساعت DisconnectTime قطع شده بود، در ساعت ConnectTime، ConnectType وصل گرديد.
Comments'
	WHERE ConfigId = 70
GO

-- SMSMultiStepConnectionAfterSendFT
-- CurrentValue = بار .. آمپر , ConnectDarsad = درصد برقدار شده , Comments = توضيحات وصل چندمرحله اي
DECLARE @IsUpdate AS BIT SET @IsUpdate = 0
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 71)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (71,N'SMSMultiStepConnectionAfterSendFT',NULL,N'بخشي از فيدر MPFeederName از پست MPPostName که در تاريخ DisconnectDatePersian و ساعت DisconnectTime با بار DCCurrentValue آمپر قطع شده بود، در ساعت ConnectTime، با مقدار بار CurrentValue آمپر، ConnectType وصل گرديد.
Comments')
ELSE IF @IsUpdate = 1
	UPDATE Tbl_Config SET ConfigText = N'بخشي از فيدر MPFeederName از پست MPPostName که در تاريخ DisconnectDatePersian و ساعت DisconnectTime با بار DCCurrentValue آمپر قطع شده بود، در ساعت ConnectTime، با مقدار بار CurrentValue آمپر، ConnectType وصل گرديد.
Comments'
	WHERE ConfigId = 71
GO

--SMSDailyPeakArea
--آخرين پيک شرکت به ميزان PeakValue مگاوات مورخ PeakDate با افزايش Percent درصد نسبت به سال گذشته و بالاترين پيک تاکنون MaxPeak مگاوات مورخ MaxPeakDate مي‌باشد.

DECLARE @IsUpdate AS BIT SET @IsUpdate = 0
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 72)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) 
	VALUES (72,N'SMSDailyPeakArea',NULL,N'ناحيه Area 
آخرين پيک: PeakValue مگاوات مورخ PeakDate 
افزايش نسبت به سال گذشته: Percent درصد
بالاترين پيک تاکنون: MaxPeak مگاوات مورخ MaxPeakDate
')
ELSE IF @IsUpdate = 1
	UPDATE Tbl_Config SET ConfigText = N'ناحيه Area 
آخرين پيک: PeakValue مگاوات مورخ PeakDate 
افزايش نسبت به سال گذشته: Percent درصد
بالاترين پيک تاکنون: MaxPeak مگاوات مورخ MaxPeakDate
'
	WHERE ConfigId = 72
GO

--GuideSMSSent
DECLARE @IsUpdate AS BIT SET @IsUpdate = 0
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 73)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) 
	VALUES (73,N'GuideSMSSent',NULL,N'راهنمايي به مشترک ارسال شد')
ELSE IF @IsUpdate = 1
	UPDATE Tbl_Config SET ConfigText = N'راهنمايي به مشترک ارسال شد'
	WHERE ConfigId = 73
GO

-- FaxBaBarnameh
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 74)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (74,N'FaxBaBarnameh',NULL,N':توزيع برقCRLFبرق شما hh:nn yy/mm/dd بمدت ss دقيقه قطع ميگردد')
go

-- FaxBiBarnameh
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 75)
	INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (75,N'FaxBiBarnameh',NULL,N'توزيع برق:CRLF برق شما تا hh:nn yy/mm/dd وصل ميگردد')
GO

-- متن پيامک وصل خاموشي هاي فوق توزيع به تفکيک فيدر بعد از وصل شدن هر فيدر
-- SMSFTMPFeederAfterConnect
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 76)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) 
	VALUES(76,N'SMSFTMPFeederAfterConnect',NULL,N'فيدر MPFeederName با DisconnectPower مگاوات ساعت از پست فوق توزيع MPPostName پس از Minutes دقيقه با علت:FogheToziType وصل گرديد‍')
GO

-- متن پيامک هشدار اتمام خاموشيهاي بابرنامه به مديران
-- SMSSendAlarmForTamir
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 77)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) 
	VALUES(77,N'SMSSendAlarmForTamir',NULL,N'هشدار
زمان خاموشي موافقت شده از ساعت DisconnectTime تا ConnectTime به درخواست RequestBy در آدرس Address ناحيه Area توسط EkipName رو به اتمام است. لطفاً پيگيري لازم جهت اتمام به موقع کار به عمل آيد')
GO

-- متن پيامک هشدار اتمام خاموشيهاي بابرنامه به پيمانکاران
-- SMSSendAlarmForPeymankar
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 78)
	INSERT INTO Tbl_Config(ConfigId,ConfigName,ConfigValue,ConfigText) 
	VALUES(78,N'SMSSendAlarmForPeymankar',NULL,N'هشدار
زمان خاموشي موافقت شده از ساعت DisconnectTime تا ConnectTime به درخواست RequestBy در آدرس Address ناحيه Area توسط EkipName رو به اتمام است. لطفاً پيگيري لازم جهت اتمام به موقع کار به عمل آيد')
GO

-- ارسال پيامک تاييد خاموشي بابرنامه به پيمانکاران
-- SendConfirmPeymankar
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 79)
INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (79,N'SendConfirmPeymankar',NULL,N'درخواست خاموشي TamirReqNo با مشخصات:
MPPostInfo MPFeederInfo MPFeedersInfo LPPostInfo FeederPartInfo LPFeederInfo تاريخ :DTDateساعت:DTTime
مدت:Minutes
ناحيه:Area
آدرس:DBAddress
Status')

-- ارسال پيامک يادآوري تعيين وضعيت درخواستهاي در انتظار تاييد
-- SendSMSReminderWaitConfirm
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 80)
INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (80,N'SendSMSReminderWaitConfirm',NULL,N'درخواست خاموشي به شماره TamirRequestNo در ناحيه Area روي شبکه TamirNetworkType، TamirRequestState مي باشد و تاکنون وضعيت تأييد يا عدم تأييد آن مشخص نشده است')
GO

-- ارسال پيامک درخواست خاموشيهاي عودت داده شده
-- SMSSendTamirReturned
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 81)
INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (81,N'SMSSendTamirReturned',NULL,N'درخواست خاموشي به شماره TamirRequestNo در ناحيه Area روي شبکه TamirNetworkType، به دليل ReturnDesc عودت داده شد')
GO

-- ارسال پيامک خاموشيهاي بابرنامه اي که بيشتر از زمان پيش بيني شده طول کشيده اند
-- SMSSendTamirLongDC
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 82)
INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (82,N'SMSSendTamirLongDC',NULL,N'خاموشي بابرنامه به شماره پرونده RequestNumber، ناحيه Area روي شبکه NetworkType، با مدت پيش بيني TamirDisconnectInterval دقيقه، در حال حاضر برقدار نگرديده و حدود LongDCInterval دقيقه از زمان برقداري آن گذشته است')
GO

-- ارسال پيامک خاموشيهاي بي برنامه جدید که در هنگام ثبت اولیه فیدر فشار متوسط آن انتخاب شده است
-- SMSNewRequestMP
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 83)
INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (83,N'SMSNewRequestMP',NULL,N'فيدر MPFeederName از پست فوق توزيع MPPostName در ساعت DTTime بر اثر اتصال در مسیر تغذیه قطع گرديد.')
GO


-- ارسال نوتيفيکيشن اطلاع رسانی خاموشیهاي بابرنامه به اپلیکیشن برق من (نرم افزار موبایل)
-- NotifBaBarname
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 84)
INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (84,N'NotifBaBarname',NULL,N'مشترک گرامي برق شما به علت مديريت بار، احتمالاً در تاريخ DisconnectDatePersian از ساعت DisconnectTime بمدت تقريبي ss دقيقه قطع خواهد شد.')
GO


-- ارسال نوتيفيکيشن اطلاع رسانی لغو خاموشیهاي بابرنامه به اپلیکیشن برق من (نرم افزار موبایل)
-- SMSCancelRequest
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 85)
INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (85,N'SMSCancelRequest',NULL,N'مشترک گرامي برنامه قطع برق شما در تاريخ DisconnectDate و ساعت DisconnectTime لغو گرديد')
GO

-- ارسال پیامک صدور اجازه کار به پیمانکار 
-- SMSPeymankarAllow
IF NOT EXISTS (SELECT * FROM Tbl_Config WHERE ConfigId = 86)
INSERT Tbl_Config (ConfigId,ConfigName,ConfigValue,ConfigText) VALUES (86,N'SMSPeymankarAllow',NULL,N'پيمانکار گرامي،
درخواست اجازه کار شما براي پرونده شماره RequestNumber با موفقيت صادر گرديد.
شماره اجازه کار : AllowNumber
')
GO
