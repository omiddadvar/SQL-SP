USE [CcRequesterSetad]
GO
/****** Object:  StoredProcedure [dbo].[spGetReport_8_15]    Script Date: 04/11/2021 12:13:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spGetReport_8_15]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[spGetReport_8_15]
GO

CREATE PROCEDURE [dbo].[spGetReport_8_15] @aPeakDatePersian AS VARCHAR(50)
	,@aPeakTime AS INT
	,@aAreaId AS VARCHAR(1000)
	,@aMPPostId AS INT
	,@aMPFeederId AS INT
	,@aIsActive AS INT
	,@aOwnershipId AS INT
AS
BEGIN
	IF ltrim(rtrim(@aAreaId)) = ''
		SET @aAreaId = NULL

	SELECT Tbl_Area.AreaId
		,Tbl_Area.Area
		,Tbl_MPPost.MPPostId
		,Tbl_MPPost.MPPostName
		,Tbl_MPFeeder.MPFeederId
		,Tbl_MPFeeder.MPFeederName
		,Tbl_MPFeederLoad.RelDatePersian
		,Tbl_MPFeederLoadHours.HourId
		,Tbl_MPFeederLoadHours.CurrentValue
		,ISNULL(trans.MPPostTrans , '»œÊ‰  —«‰”') AS MPPostTrans
		,ISNULL(trans.MPPostTransPower , 0) AS MPPostTransPower
	FROM Tbl_MPFeederLoad
	INNER JOIN Tbl_MPFeederLoadHours ON Tbl_MPFeederLoad.MPFeederLoadId = Tbl_MPFeederLoadHours.MPFeederLoadId
	INNER JOIN Tbl_MPFeeder ON Tbl_MPFeederLoad.MPFeederId = Tbl_MPFeeder.MPFeederId
	INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId
	INNER JOIN Tbl_Area ON Tbl_MPFeeder.AreaId = Tbl_Area.AreaId
	LEFT JOIN Tbl_MPPostTrans trans ON Tbl_MPFeeder.MPPostTransId = trans.MPPostTransId
	WHERE Tbl_MPFeederLoad.RelDatePersian = @aPeakDatePersian
		AND Tbl_MPFeederLoadHours.HourId = @aPeakTime
		AND (Tbl_MPFeeder.AreaId IN (SELECT * FROM Split(@aAreaId,',')) OR @aAreaId IS NULL)
		AND (Tbl_MPFeeder.MPPostId = @aMPPostId OR @aMPPostId = -1)
		AND (Tbl_MPFeeder.MPFeederId = @aMPFeederId OR @aMPFeederId = -1)
		AND (Tbl_MPFeeder.IsActive = @aIsActive OR @aIsActive = - 1)
		AND (Tbl_MPFeeder.OwnershipId = @aOwnershipId OR @aOwnershipId = - 1)
END	
