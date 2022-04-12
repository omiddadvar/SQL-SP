USE CCRequesterSetad
GO
ALTER PROCEDURE Homa.spGetOnCall (
	@CheckSum AS BIGINT
	,@aRequestId AS BIGINT = NULL
	,@aOnCallId AS BIGINT = NULL
  ,@Areas AS VARCHAR(500) = ''
) AS
BEGIN	
	CREATE TABLE #tmp (OnCallId BIGINT, TabletName NVARCHAR(50) , MasterName NVARCHAR(500)
    ,DatePersian VARCHAR(10) , Time VARCHAR(5),Area NVARCHAR(50) , AreaId INT , IsChecked BIT)
	IF ISNULL(@aRequestId,0) > 0 OR ISNULL(@aOnCallId,0) > 0
	BEGIN
    CREATE TABLE #tmpJobOncall ( JobId BIGINT, OnCallId BIGINT)
    IF @aRequestId > 0 BEGIN  
      	INSERT INTO #tmpJobOncall SELECT TOP(1) JobId , OncallId FROM Homa.TblJob WHERE RequestId = @aRequestId
    END
    ELSE IF @aOnCallId > 0 BEGIN
      	INSERT INTO #tmpJobOncall SELECT JobId , OncallId FROM Homa.TblJob WHERE OncallId = @aOnCallId
    END
		CREATE TABLE #tmpOncall ( OnCallId BIGINT,DT DATETIME)
		INSERT INTO #tmpOncall
			SELECT Arr.OnCallId, Arr.ArriveDT AS DT FROM Homa.TblEkipArrive Arr
      INNER JOIN #tmpJobOncall OnCall ON OnCall.JobId = Arr.JobId
		INSERT INTO #tmpOncall		
			SELECT StartM.OnCallId, StartM.StartMoveDT AS DT FROM Homa.TblEkipStartMove StartM
      INNER JOIN #tmpJobOncall OnCall ON OnCall.JobId = StartM.JobId
		INSERT INTO #tmpOncall
			SELECT UnDone.OnCallId, UnDone.DataEntryDT AS DT FROM Homa.TblEkipUndoneWork UnDone
      INNER JOIN #tmpJobOncall OnCall ON OnCall.JobId = UnDone.JobId
		INSERT INTO #tmpOncall
			SELECT TJ.OnCallId, TJ.DataEntryDT AS DT  FROM Homa.TblJob TJ
      INNER JOIN #tmpJobOncall OnCall ON OnCall.JobId = TJ.JobId
						
		INSERT INTO #tmp (OnCallId,TabletName,MasterName ,DatePersian,Time,Area, AreaId , IsChecked)
		SELECT  OnCallId, Tbl_Tablet.TabletName , Tbl_Master.Name,
      TblOnCall.OnCallStartDatePersian ,TblOnCall.OnCallStartTime ,Tbl_Area.Area, Tbl_Area.AreaId , CAST(1 AS BIT)
		FROM Homa.TblOnCall
		INNER JOIN Homa.Tbl_TabletUser ON TblOnCall.TabletUserId=Tbl_TabletUser.TabletUserId
		INNER JOIN Tbl_Area ON Tbl_TabletUser.BaseAreaId=Tbl_Area.AreaId
		INNER JOIN Tbl_Master ON Tbl_TabletUser.MasterId=Tbl_Master.MasterId
		INNER JOIN Homa.Tbl_Tablet ON Tbl_Tablet.TabletId=TblOnCall.TabletId		
		WHERE TblOnCall.OnCallId in (SELECT OnCallId FROM #tmpOncall)
		ORDER BY Tbl_Area.Area,Tbl_Master.Name 
		DROP TABLE #tmpOncall
    DROP TABLE #tmpJobOncall
	END
	ELSE
	BEGIN
		SELECT CAST(Item AS INT) AS AreaId INTO #tmpArea FROM dbo.Split(@Areas,',')
		WHERE ISNUMERIC(Item)=1
		
		INSERT INTO #tmp (OnCallId,TabletName,MasterName ,DatePersian,Time,Area, AreaId , IsChecked)
      SELECT  OnCallId, Tbl_Tablet.TabletName , Tbl_Master.Name, TblOnCall.OnCallStartDatePersian 
        ,TblOnCall.OnCallStartTime ,Tbl_Area.Area, Tbl_Area.AreaId , CAST(1 AS BIT)
		FROM Homa.TblOnCall
		INNER JOIN Homa.Tbl_TabletUser on TblOnCall.TabletUserId=Tbl_TabletUser.TabletUserId
		INNER JOIN Tbl_Area on Tbl_TabletUser.BaseAreaId=Tbl_Area.AreaId
		INNER JOIN Tbl_Master on Tbl_TabletUser.MasterId=Tbl_Master.MasterId
		INNER JOIN Homa.Tbl_Tablet on Tbl_Tablet.TabletId=TblOnCall.TabletId		
		WHERE TblOnCall.IsActive=1 		
		ORDER BY Tbl_Area.Area,Tbl_Master.Name 
		IF @Areas<>''
		BEGIN
			DELETE FROM #tmp
			WHERE NOT AreaId IN (SELECT AreaId FROM #tmpArea)
		END
		
		DROP TABLE #tmpArea
	END
	
	DECLARE @NewCheckSum AS BIGINT
	SELECT @NewCheckSum = CHECKSUM_AGG(CHECKSUM(*)) FROM #tmp
	
	
	IF ISNULL(@CheckSum, 0) = ISNULL(@NewCheckSum, 0) 
	BEGIN
		SELECT ISNULL(@NewCheckSum,0) AS [CheckSum]
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