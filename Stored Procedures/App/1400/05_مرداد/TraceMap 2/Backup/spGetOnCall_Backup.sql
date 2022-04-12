
CREATE procedure Homa.spGetOnCall (
	@CheckSum as bigint 
	,@aRequestId as bigint=null,
	@Areas as varchar(500)=''
) AS
BEGIN	
	create table #tmp (OnCallId bigint,MasterName nvarchar(500),AreaId int)
	IF ISNULL(@aRequestId , 0) > 0 BEGIN  
		declare @lJobId as bigint
		declare @lOnCallId as bigint
		select top 1 @lJobId=JobId ,@lOnCallId=OncallId from Homa.TblJob where RequestId=@aRequestId
		create table #tmpOncall ( OnCallId bigint,DT Datetime)
		insert into #tmpOncall
			select OnCallId,TblEkipArrive.ArriveDT as DT from Homa.TblEkipArrive where JobId=@lJobId
		insert into #tmpOncall		
			select OnCallId,TblEkipStartMove.StartMoveDT as DT from Homa.TblEkipStartMove where JobId=@lJobId
		insert into #tmpOncall
			select OnCallId,TblEkipUndoneWork.DataEntryDT as DT from Homa.TblEkipUndoneWork where JobId=@lJobId
		insert into #tmpOncall
			select OnCallId,DataEntryDT as DT  from Homa.TblJob where JobId=@lJobId
						
		insert into #tmp (OnCallId ,MasterName,AreaId)
		select  OnCallId,+Tbl_Area.Area+ '(تبلت '+Tbl_Tablet.TabletName+ ') -استاد کار ' +Tbl_Master.Name
		+' '+ TblOnCall.OnCallStartDatePersian + ' ' +TblOnCall.OnCallStartTime as MasterName 
		,Tbl_Area.AreaId
		from Homa.TblOnCall
		inner join Homa.Tbl_TabletUser on TblOnCall.TabletUserId=Tbl_TabletUser.TabletUserId
		inner join Tbl_Area on Tbl_TabletUser.BaseAreaId=Tbl_Area.AreaId
		inner join Tbl_Master on Tbl_TabletUser.MasterId=Tbl_Master.MasterId
		inner join Homa.Tbl_Tablet on Tbl_Tablet.TabletId=TblOnCall.TabletId		
		where TblOnCall.OnCallId in (select OnCallId from #tmpOncall)
		order by Tbl_Area.Area,Tbl_Master.Name 
		drop table #tmpOncall
	END
	ELSE
	BEGIN
		select cast(Item as int) as AreaId into #tmpArea from dbo.Split(@Areas,',')
		where isnumeric(Item)=1
		
		insert into #tmp (OnCallId ,MasterName,AreaId)
		select  OnCallId,+Tbl_Area.Area+ '(تبلت '+Tbl_Tablet.TabletName+ ') -استاد کار ' +Tbl_Master.Name
		+' '+ TblOnCall.OnCallStartDatePersian + ' ' +TblOnCall.OnCallStartTime as MasterName 
		,Tbl_Area.AreaId
		from Homa.TblOnCall
		inner join Homa.Tbl_TabletUser on TblOnCall.TabletUserId=Tbl_TabletUser.TabletUserId
		inner join Tbl_Area on Tbl_TabletUser.BaseAreaId=Tbl_Area.AreaId
		inner join Tbl_Master on Tbl_TabletUser.MasterId=Tbl_Master.MasterId
		inner join Homa.Tbl_Tablet on Tbl_Tablet.TabletId=TblOnCall.TabletId		
		where TblOnCall.IsActive=1 		
		order by Tbl_Area.Area,Tbl_Master.Name 
		if @Areas<>''
		BEGIN
			delete from #tmp
			where not AreaId in (select AreaId from #tmpArea)
		END
		
		drop table #tmpArea
	END
	
	DECLARE @NewCheckSum AS BIGINT
	SELECT @NewCheckSum = CHECKSUM_AGG(CHECKSUM(*)) FROM #tmp
	
	
	IF ISNULL(@CheckSum, 0) = ISNULL(@NewCheckSum, 0) 
	BEGIN
		SELECT ISNULL(@NewCheckSum,0) as [CheckSum]
	END
	ELSE
	BEGIN
		SELECT *,
			@NewCheckSum AS [CheckSum]
		FROM #tmp
	END
	DROP TABLE #tmp
	
END
GO