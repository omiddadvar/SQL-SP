CREATE PROCEDURE dbo.spAddTestServiceLog @TestServiceId AS BIGINT
	,@TestServiceStatus AS INT
	,@Description AS NVARCHAR(max)
AS
BEGIN
	/* 0 = Œÿ«; 1 = ”«·„  */
	IF EXISTS (
			SELECT *
			FROM (
				SELECT TOP 1 *
				FROM TblTestServiceLog WITH (INDEX (Idx1))
				WHERE TestServiceId = @TestServiceId
				ORDER BY TestServiceLogId DESC
				) AS a1
			WHERE ISNULL(a1.[Description], '') = LEFT(ISNULL(@Description, ''), 1000)
				AND TestServiceId = @TestServiceId
			)
	BEGIN
		RETURN
	END

	DECLARE @TestServiceLogId AS BIGINT

	INSERT INTO TblTestServiceLog (
		 TestServiceId
		,TestServiceStatus
		,[Description]
		,LogDT
		)
	VALUES (
		@TestServiceId
		,@TestServiceStatus
		,LEFT(@Description, 1000)
		,GETDATE()
		)

	SET @TestServiceLogId = @@IDENTITY

	EXEC dbo.spAddTestServiceSMSLog @TestServiceLogId
		,@TestServiceStatus
END
GO