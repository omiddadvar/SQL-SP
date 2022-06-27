
---------------Shomal Kerman-----------------------------

SELECT
  Active.LPPostId ActiveId,
  Active.MPFeederId ActiveMPFeederId,
  Active.AreaId ActiveAreaId,
  Active.LPPostName ActiveLPPostName,
  InActive.LPPostId InActiveId,
  InActive.MPFeederId InActiveMPFeederId,
  InActive.AreaId InActiveAreaId,
  InActive.LPPostName InActiveLPPostName,
  Active.LPPostCode 
INTO #tmp
FROM Tbl_LPPost Active
INNER JOIN Tbl_LPPost InActive ON Active.LPPostCode = InActive.LPPostCode
WHERE 
  Active.IsActive = 1 
  AND ISNULL(InActive.IsActive, 0) = 0
  AND ISNULL(Active.LPPostCode , '') <> ''

SELECT 
  DISTINCT T.ActiveId, 
  T.LPPostCode, 
  A.Area,
  MPF.MPFeederName,
  MPP.MPPostName
INTO #tmpAcvtive
FROM #tmp T
  INNER JOIN Tbl_MPFeeder MPF ON T.ActiveMPFeederId = MPF.MPFeederId
  INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
  INNER JOIN Tbl_Area A ON T.ActiveAreaId = A.AreaId

SELECT 
  DISTINCT T.InActiveId, 
  T.LPPostCode, 
  A.Area,
  MPF.MPFeederName,
  MPP.MPPostName
INTO #tmpInAcvtive
FROM #tmp T
  INNER JOIN Tbl_MPFeeder MPF ON T.InActiveMPFeederId = MPF.MPFeederId
  INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
  INNER JOIN Tbl_Area A ON T.InActiveAreaId = A.AreaId

SELECT
  T.LPPostCode,
  T.ActiveId,
  T.InActiveId,
  T.ActiveLPPostName,
  TA.Area AS ActiveArea,
  TA.MPFeederName AS ActiveMPFeederName,
  TA.MPPostName AS ActiveMPPostName,
  T.InActiveLPPostName,
  TI.Area AS InActiveArea,
  TI.MPFeederName AS InActiveMPFeederName,
  TI.MPPostName AS InActiveMPPostName
FROM #tmp T
  INNER JOIN #tmpAcvtive TA ON T.ActiveId = TA.ActiveId
  INNER JOIN #tmpInAcvtive TI ON T.InActiveId = TI.InActiveId
Order By T.InActiveAreaId , T.InActiveMPFeederId

--SELECT * FROM  #tmp 
DROP TABLE #tmp
DROP TABLE #tmpAcvtive
DROP TABLE #tmpInAcvtive

----------------------------------------------
