CREATE VIEW ViewAzGh_MPFeeder_BaseInfo
As
SELECT A.AreaId 
      ,A.Area
      ,MPP.MPPostId 
      ,MPP.MPPostName 
      ,MPP.MPPostCode 
      ,MPP.IsActive AS IsMPPostActive
      ,MPF.MPFeederId 
      ,MPF.MPFeederName 
      ,MPF.MPFeederCode 
      ,MPF.IsPrivate AS IsMPFeederPrivate
      ,MPF.IsActive AS IsMPFeederActive
FROM Tbl_MPFeeder MPF
  INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
  INNER JOIN Tbl_Area A ON MPF.AreaId = A.AreaId
 
/*

SELECT * FROM ViewAzGh_MPFeeder_BaseInfo


*/