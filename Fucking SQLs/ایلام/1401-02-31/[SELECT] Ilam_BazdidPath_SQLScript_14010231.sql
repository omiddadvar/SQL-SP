DECLARE @AreaID AS INT = 2 

CREATE TABLE __Tbl_Mousian_Basket
(
	BazdidBasketId BIGINT,
  AreaId INT,
	BazdidBasketDetailId BIGINT UNIQUE,
  DetailAreaId INT
)
  
INSERT INTO __Tbl_Mousian_Basket (BazdidBasketId, AreaId, BazdidBasketDetailId, DetailAreaId)
SELECT B.BazdidBasketId , B.AreaId , D.BazdidBasketDetailId , D.AreaId AS DetailAreaId 
  FROM BTblBazdidBasket B 
    INNER JOIN BTblBazdidBasketDetail D ON B.BazdidBasketId = D.BazdidBasketId
    INNER JOIN Tbl_MPFeeder MPF ON D.MPFeederId = MPF.MPFeederId
    LEFT JOIN Tbl_LPPost LPP ON D.LPPostId = LPP.LPPostId
    LEFT JOIN Tbl_LPFeeder LPF ON D.LPFeederId = LPF.LPFeederId
  WHERE D.AreaId <> @AreaID AND
      (
          (D.BazdidTypeId = 1 AND MPF.AreaId = @AreaID)
          OR (D.BazdidTypeId = 2 AND LPP.AreaId = @AreaID)
          OR (D.BazdidTypeId = 3 AND LPF.AreaId = @AreaID)
      )

SELECT * FROM __Tbl_Mousian_Basket
DROP TABLE __Tbl_Mousian_Basket

------------------------------------------------------------------------------------------
  
CREATE TABLE __Tbl_Mousian_Timing
(
	BazdidTimingId BIGINT,
  AreaId INT,
	BazdidResultId BIGINT UNIQUE,
  ResultAreaId INT
)
  
INSERT INTO __Tbl_Mousian_Timing (BazdidTimingId, AreaId, BazdidResultId, ResultAreaId)
SELECT T.BazdidTimingId , T.AreaId , R.BazdidResultId , R.AreaId AS ResultAreaId 
  FROM BTblBazdidTiming T 
    INNER JOIN BTblBazdidResult R ON T.BazdidTimingId = R.BazdidTimingId
    INNER JOIN Tbl_MPFeeder MPF ON R.MPFeederId = MPF.MPFeederId
    LEFT JOIN Tbl_LPPost LPP ON R.LPPostId = LPP.LPPostId
    LEFT JOIN Tbl_LPFeeder LPF ON R.LPFeederId = LPF.LPFeederId
  WHERE R.AreaId <> @AreaID AND
      (
          (R.BazdidTypeId = 1 AND MPF.AreaId = @AreaID)
          OR (R.BazdidTypeId = 2 AND LPP.AreaId = @AreaID)
          OR (R.BazdidTypeId = 3 AND LPF.AreaId = @AreaID)
      )



SELECT * FROM __Tbl_Mousian_Timing
DROP TABLE __Tbl_Mousian_Timing