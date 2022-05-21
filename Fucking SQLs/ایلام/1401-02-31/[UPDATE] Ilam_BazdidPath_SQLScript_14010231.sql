DECLARE @AreaID AS INT = 2 

--CREATE TABLE __Tbl_Mousian_Basket
--(
--	BazdidBasketId BIGINT,
--  AreaId INT,
--	BazdidBasketDetailId BIGINT UNIQUE,
--  DetailAreaId INT
--)
UPDATE BTblBazdidBasketDetail SET AreaId = @AreaID
  FROM BTblBazdidBasketDetail D
    INNER JOIN __Tbl_Mousian_Basket TB ON D.BazdidBasketDetailId = TB.BazdidBasketDetailId

UPDATE BTblBazdidBasket SET AreaId = @AreaID
  FROM BTblBazdidBasket B
    INNER JOIN __Tbl_Mousian_Basket TB ON B.BazdidBasketId = TB.BazdidBasketId


--SELECT * FROM Tbl_TableName WHERE TableName IN ('BTblBazdidBasketDetail' , 'BTblBazdidBasket')

INSERT INTO Tbl_EventLogCenter (
	TableName,
	TableNameId,
	PrimaryKeyId,
	Operation,
	AreaId,
	WorkingAreaId,
	DataEntryDT
	)
SELECT
	'BTblBazdidBasketDetail' AS TableName,
	176 AS TableNameId,
	BazdidBasketDetailId AS PrimaryKeyId,
	3 AS Operation,
	@AreaId AS AreaId,
	99 AS WorkingAreaId,
	GETDATE() AS DataEntryDT
FROM 
	__Tbl_Mousian_Basket



INSERT INTO Tbl_EventLogCenter (
	TableName,
	TableNameId,
	PrimaryKeyId,
	Operation,
	AreaId,
	WorkingAreaId,
	DataEntryDT
	)
SELECT DISTINCT
	'BTblBazdidBasket' AS TableName,
	175 AS TableNameId,
	BazdidBasketId AS PrimaryKeyId,
	3 AS Operation,
	@AreaId AS AreaId,
	99 AS WorkingAreaId,
	GETDATE() AS DataEntryDT
FROM 
	__Tbl_Mousian_Basket

-------------------------------------------------------------------  
--CREATE TABLE __Tbl_Mousian_Timing
--(
--	BazdidTimingId BIGINT,
--  AreaId INT,
--	BazdidResultId BIGINT UNIQUE,
--  ResultAreaId INT
--)

UPDATE BTblBazdidResult SET AreaId = @AreaID 
  FROM BTblBazdidResult
    INNER JOIN __Tbl_Mousian_Timing TT ON BTblBazdidResult.BazdidResultId = TT.BazdidResultId
    
UPDATE BTblBazdidTiming SET AreaId = @AreaID 
  FROM BTblBazdidTiming
    INNER JOIN __Tbl_Mousian_Timing TT ON BTblBazdidTiming.BazdidTimingId = TT.BazdidTimingId

--SELECT * FROM Tbl_TableName WHERE TableName IN ('BTblBazdidResult' , 'BTblBazdidTiming')


INSERT INTO Tbl_EventLogCenter (
	TableName,
	TableNameId,
	PrimaryKeyId,
	Operation,
	AreaId,
	WorkingAreaId,
	DataEntryDT
	)
SELECT
	'BTblBazdidResult' AS TableName,
	184 AS TableNameId,
	BazdidResultId AS PrimaryKeyId,
	3 AS Operation,
	@AreaId AS AreaId,
	99 AS WorkingAreaId,
	GETDATE() AS DataEntryDT
FROM 
	__Tbl_Mousian_Timing 


INSERT INTO Tbl_EventLogCenter (
	TableName,
	TableNameId,
	PrimaryKeyId,
	Operation,
	AreaId,
	WorkingAreaId,
	DataEntryDT
	)
SELECT
	'BTblBazdidTiming' AS TableName,
	180 AS TableNameId,
	BazdidTimingId AS PrimaryKeyId,
	3 AS Operation,
	@AreaId AS AreaId,
	99 AS WorkingAreaId,
	GETDATE() AS DataEntryDT
FROM 
	__Tbl_Mousian_Timing 

