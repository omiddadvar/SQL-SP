 SELECT  	Tbl_MPPostTrans.MPPostId, 
	Tbl_MPPostTrans.MPPostTransId, 
	tbl_MPPost.MPPostName,  	
	Tbl_MPPostTrans.MPPostTrans, 
	ViewMPTransHourly.MPPostTransLoadId, 
	ViewMPTransHourly.MPPostTransLoadHourId,
	 ViewMPTransHourly.LoadDatePersian,  	
	 ViewMPTransHourly.HourId,
	  ViewMPTransHourly.[Hour], 
	  ViewMPTransHourly.HourExact,  	
	  ViewMPTransHourly.CurrentValue, 
	  ViewMPTransHourly.CurrentValueReActive, 
	  Tbl_MPPostTrans.SortOrder  FROM  	
		(SELECT '1400/01/25' AS b1, '11:00' AS b2) 
		DERIVEDTBL  	
		INNER JOIN ViewMPTransHourly ON ViewMPTransHourly.LoadDatePersian = DERIVEDTBL.b1 
			COLLATE Arabic_CI_AS_WS AND ViewMPTransHourly.[Hour] = DERIVEDTBL.b2 COLLATE  Arabic_CI_AS_WS  	
		RIGHT OUTER JOIN Tbl_MPPostTrans ON ViewMPTransHourly.MPPostTransId = Tbl_MPPostTrans.MPPostTransId  	
		INNER JOIN Tbl_MPPost ON Tbl_MPPostTrans.MPPostId = Tbl_MPPost.MPPostId  
		ORDER BY ISNULL(Tbl_MPPost.SortOrder,9999999), Tbl_MPPost.MPPostName, 
			ISNULL(Tbl_MPPostTrans.SortOrder,9999999), Tbl_MPPostTrans.MPPostTrans