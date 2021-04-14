SELECT Tbl_MPFeeder.MPPostId, 
	Tbl_MPFeeder.AreaId, 
	Tbl_MPFeeder.MPFeederId, 
	Tbl_MPFeeder.CosinPhi, 
	tbl_MPPost.MPPostName, 
	Tbl_MPFeeder.MPFeederName 
		+ CASE WHEN ISNULL(Tbl_MPFeeder.MPFeederCode,'') <> '' THEN ' - ' 
		+ Tbl_MPFeeder.MPFeederCode ELSE '' END AS MPFeederName, 	
	ViewMPFeederHourly.MPFeederLoadId, 
	ViewMPFeederHourly.MPFeederLoadHourId,  
	ViewMPFeederHourly.LoadDatePersian, 	
	ViewMPFeederHourly.HourId, 
	ViewMPFeederHourly.[Hour], 
	ViewMPFeederHourly.HourExact, 	
	ViewMPFeederHourly.CurrentValue AS PeakSynch, 
	ViewMPFeederHourly.CurrentValueReActive, 
	ViewMPFeederHourly.PowerValue, 	
	CAST(NULL AS FLOAT) AS PeakCurrentValueMW 
	FROM 	(SELECT '1400/01/25' AS b1, '11:00' AS b2) 
		DERIVEDTBL 	
		INNER JOIN ViewMPFeederHourly ON ViewMPFeederHourly.LoadDatePersian = DERIVEDTBL.b1 
			COLLATE Arabic_CI_AS_WS AND ViewMPFeederHourly.[Hour] = DERIVEDTBL.b2 
			COLLATE Arabic_CI_AS_WS 	
		RIGHT OUTER JOIN Tbl_MPFeeder ON Tbl_MPFeeder.MPFeederId = ViewMPFeederHourly.MPFeederId 	
		INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId  
	WHERE  Tbl_MPFeeder.IsActive = 1 
		AND (Tbl_MPFeeder.AreaId = 2) 
		AND (Tbl_MPFeeder.MPPostId = 1) 
	ORDER BY ISNULL(Tbl_MPPost.SortOrder,9999999), 
		Tbl_MPPost.MPPostName, 
		ISNULL(Tbl_MPFeeder.SortOrder,9999999),
		Tbl_MPFeeder.MPFeederName 