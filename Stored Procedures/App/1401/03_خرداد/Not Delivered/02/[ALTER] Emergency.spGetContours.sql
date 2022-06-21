ALTER PROC Emergency.spGetContours
  @aToolIDs AS VARCHAR(5000) = '',
  @aStationIDs AS VARCHAR(5000) = '',
  @aTypeID AS  VARCHAR(5000) = ''
  AS 
  BEGIN
  DECLARE @lWhere AS NVARCHAR(MAX) = ''
          ,@lSQL AS NVARCHAR(MAX) = '
SELECT A.Area , A.AreaId 
  , MPP.MPPostId , MPP.MPPostName 
  , MPT.MPPostTrans, MPF.MPFeederName 
  ,K.*, CASE WHEN ToolTypeId = 4 THEN ''فيدر''
      	WHEN ToolTypeId = 1 THEN ''ترانس''
        ELSE ''نامشخص''
        END AS ToolType 
FROM Meter.Tbl_Kontor K
  LEFT JOIN Tbl_MPFeeder MPF ON K.MPFeederId = MPF.MPFeederId
  LEFT JOIN Tbl_MPPostTrans MPT ON K.MPPostTransId = MPT.MPPostTransId
  LEFT JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId OR MPT.MPPostId = MPP.MPPostId
  LEFT JOIN Tbl_Area A ON MPF.AreaId = A.AreaId OR (K.MPPostTransId IS NOT NULL AND MPP.AreaId = A.AreaId)'
  
  SET @lWhere = ' AND ToolTypeId IS NOT NULL'
  IF LEN(@aToolIDs) > 0 BEGIN
    SET @lWhere = @lWhere + ' AND K.ToolId IN ('+ @aToolIDs +')'                	
  END
  IF LEN(@aStationIDs) > 0 BEGIN
    SET @lWhere = @lWhere + ' AND K.StationId IN ('+ @aStationIDs +')'                	
  END
  IF LEN(@aTypeID) > 0 BEGIN
    SET @lWhere = @lWhere + ' AND K.ToolTypeId IN ('+ @aTypeID +')'            	
  END
  /*--------------------------------------------------*/
  SET @lWhere = ' WHERE ' + RIGHT(@lWhere ,LEN(@lWhere) - 4)
  SET @lSQL = @lSQL + @lWhere
  EXEC(@lSQL)
END


/*

EXEC Emergency.spGetContours @aToolIDs = ''
                            ,@aStationIDs = ''
                            ,@aTypeID = ''

*/
