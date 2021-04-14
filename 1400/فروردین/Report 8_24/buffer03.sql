USE [CcRequester]
GO

/****** Object:  View [dbo].[ViewMPFeederHourly]    Script Date: 04/14/2021 13:41:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE VIEW [dbo].[ViewMPFeederHourly]
AS
SELECT
	Tbl_MPPost.MPPostId, 
	Tbl_MPFeeder.AreaId, 
	Tbl_MPFeeder.MPFeederId, 
	Tbl_MPFeeder.MPFeederName, 
	Tbl_MPFeederLoad.MPFeederLoadId, 
	Tbl_MPFeederLoadHours.MPFeederLoadHourId, 
	Tbl_MPFeederLoad.RelDatePersian AS LoadDatePersian, 
	Tbl_Hour.HourId, 
	Tbl_Hour.[Hour], 
	Tbl_MPFeederLoadHours.HourExact, 
    Tbl_MPFeederLoadHours.CurrentValue, 
	Tbl_MPFeederLoadHours.CurrentValueReActive, 
	Tbl_MPFeederLoadHours.PowerValue
FROM 
	Tbl_MPFeeder 
	INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId 
	LEFT OUTER JOIN Tbl_MPFeederLoad ON Tbl_MPFeeder.MPFeederId = Tbl_MPFeederLoad.MPFeederId
    INNER JOIN Tbl_MPFeederLoadHours ON Tbl_MPFeederLoad.MPFeederLoadId = Tbl_MPFeederLoadHours.MPFeederLoadId
	LEFT JOIN Tbl_Hour ON dbo.Tbl_MPFeederLoadHours.HourId = dbo.Tbl_Hour.HourId
GO