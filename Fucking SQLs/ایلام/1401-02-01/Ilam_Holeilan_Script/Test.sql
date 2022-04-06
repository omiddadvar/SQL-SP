SELECT TOP(10) RequestId , AreaId , LPRequestId , MPRequestId 
   , MPRequestId , IsMPRequest , LPRequestId , IsLPRequest FROM TblRequest

SELECT TOP(10) MPRequestId , AreaId , MPPostId , MPFeederId , LPPostId  FROM TblMPRequest

SELECT TOP(10) LPRequestId , AreaId , LPPostId , LPFeederId FROM TblLPRequest

-------------------------------

SELECT TOP(10) * FROM Tbl_MPPost

SELECT TOP(10) * FROM Tbl_MPFeeder

SELECT TOP(10) * FROM Tbl_LPPost

SELECT TOP(10) * FROM Tbl_LPFeeder

--------------------------------------------------------------------


SELECT R.AreaId  , A.Area ,COUNT(R.RequestId) FROM TblRequest R
  INNER JOIN Tbl_Area A ON R.AreaId = A.AreaId
  GROUP BY R.AreaId , A.Area













