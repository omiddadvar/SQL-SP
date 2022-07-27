CREATE PROCEDURE Emergency.spGetLimitDays 
	@aMPFeederLimitTypeId as int = NULL
AS
	IF @aMPFeederLimitTypeId <= 0 SET @aMPFeederLimitTypeId = NULL
	SELECT 
		tLD.*,
		tLT.MPFeederLimitType,
		ISNULL(tWD.WeekDay,CASE WHEN tLD.IsHoliday = 1 THEN N'روزهای تعطیل'ELSE N'تمامي روزهاي هفته'END) AS WeekDay
	FROM 
		Emergency.Tbl_MPFeederLimitDay tLD
		INNER JOIN Emergency.Tbl_MPFeederLimitType tLT ON tLD.MPFeederLimitTypeId = tLT.MPFeederLimitTypeId
		LEFT JOIN Emergency.Tbl_WeekDay tWD ON tLD.WeekDayId = tWD.WeekDayId
	WHERE
		(@aMPFeederLimitTypeId IS NULL OR tLD.MPFeederLimitTypeId = @aMPFeederLimitTypeId)
GO