
CREATE FUNCTION dbo.ShamsiDateTimeToMiladi (@aShDate AS VARCHAR(10) , @aShTime AS VARCHAR(5))
RETURNS DATETIME AS
BEGIN
    DECLARE @lDateTime AS DATETIME,
            @lHour AS INT,
            @lMinute AS INT

   SET @lDateTime = dbo.shtom(@aShDate)
   SET @lHour = CAST(SUBSTRING( @aShTime, 1 ,2) AS INT)
   SET @lMinute = CAST(SUBSTRING( @aShTime, 4 ,2) AS INT)
   SET @lDateTime = DATEADD(HOUR, @lHour , @lDateTime)
   SET @lDateTime = DATEADD(MINUTE, @lMinute , @lDateTime)
 
 RETURN @lDateTime
END


