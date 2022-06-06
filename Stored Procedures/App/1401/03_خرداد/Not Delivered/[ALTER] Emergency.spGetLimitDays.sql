
ALTER PROCEDURE Emergency.spGetLimitDays 
	@aMPFeederLimitTypeId as int = NULL
AS
	IF @aMPFeederLimitTypeId <= 0 SET @aMPFeederLimitTypeId = NULL
	SELECT 
		tLD.*,
		tLT.MPFeederLimitType,
		ISNULL(tWD.WeekDay,CASE WHEN tLD.IsHoliday = 1 THEN N'ÑæÒåÇí ÊÚØíá'ELSE N'ÊãÇãí ÑæåÇí åÝÊå'END) AS WeekDay
	FROM 
		Emergency.Tbl_MPFeederLimitDay tLD
		INNER JOIN Emergency.Tbl_MPFeederLimitType tLT ON tLD.MPFeederLimitTypeId = tLT.MPFeederLimitTypeId
		LEFT JOIN Emergency.Tbl_WeekDay tWD ON tLD.WeekDayId = tWD.WeekDayId
	WHERE
		(@aMPFeederLimitTypeId IS NULL OR tLD.MPFeederLimitTypeId = @aMPFeederLimitTypeId)
GO

/*
ALTER PROCEDURE Emergency.spGetLimitDays 
	@aMPFeederLimitTypeId as int = NULL
AS
	IF @aMPFeederLimitTypeId <= 0 SET @aMPFeederLimitTypeId = NULL
	SELECT 
		tLD.*,
		tLT.MPFeederLimitType,
		ISNULL(tWD.WeekDay,N'ÑæÒåÇí ÊÚØíá') AS WeekDay
	FROM 
		Emergency.Tbl_MPFeederLimitDay tLD
		INNER JOIN Emergency.Tbl_MPFeederLimitType tLT ON tLD.MPFeederLimitTypeId = tLT.MPFeederLimitTypeId
		LEFT JOIN Emergency.Tbl_WeekDay tWD ON tLD.WeekDayId = tWD.WeekDayId
	WHERE
		(@aMPFeederLimitTypeId IS NULL OR tLD.MPFeederLimitTypeId = @aMPFeederLimitTypeId)
GO
  */