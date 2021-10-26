USE CCRequesterSetad
GO
ALTER PROCEDURE dbo.spSetRequestForCancelNotification 
	@aRequestId AS BIGINT
AS
	DECLARE @MessageText AS NVARCHAR(1000)
	DECLARE @MessageTitle AS NVARCHAR(1000)
	DECLARE @AreaId AS INT
  SET @MessageText = N'„‘ —ò ê—«„Ì∫ »—‰«„Â ﬁÿ⁄ »—ﬁ ‘„« ·€Ê ê—œÌœÂ «”  '
	SET @MessageTitle = N'·€Ê Œ«„Ê‘Ì »«»—‰«„Â'
	
	IF EXISTS(SELECT SendMessageScheduleId FROM Tbl_SendMessageSchedule	WHERE RequestId = @aRequestId AND ScheduleStatusId = 2)
	BEGIN
		INSERT INTO Tbl_SendMessageSchedule
		  (RequestId ,Command ,ScheduleStatusId ,ScheduleDT ,ExpiredDT ,ScheduleTypeId ,MessageTitle 
          ,MessageText ,SendDT ,ScheduleDTFrom ,ScheduleDTTo)
		SELECT 
			RequestId,Command,1,ScheduleDT,ExpiredDT,1,@MessageTitle,@MessageText,NULL,ScheduleDTFrom,ScheduleDTTo
		FROM 
			Tbl_SendMessageSchedule
		WHERE 
			Tbl_SendMessageSchedule.RequestId = @aRequestId AND ScheduleStatusId = 2

		UPDATE TblRequestCancelSMS
		  SET SendCancelNotStatusId = 2		
		  WHERE RequestId = @aRequestId
	END
	ELSE
	BEGIN
		UPDATE TblRequestCancelSMS
  		SET SendCancelNotStatusId = 6
  		WHERE RequestId = @aRequestId
	END 
	
	SELECT @AreaId = AreaId FROM TblRequest WHERE RequestId = @aRequestId
	
	INSERT INTO Tbl_EventLogCenter (TableName, TableNameId, PrimaryKeyId, Operation, AreaId, WorkingAreaId, DataEntryDT, SQLCommand)
	SELECT 	
		'TblRequestCancelSMS' AS TableName,
		359 AS TableNameId,
		@aRequestId AS PrimaryKeyId,
		2 AS Operation,
		@AreaId AS AreaId,
		99 AS WorkingAreaId,
		GETDATE() AS DataEntryDT,
		NULL AS SQLCommand
GO