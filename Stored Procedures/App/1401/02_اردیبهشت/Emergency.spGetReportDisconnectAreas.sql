ALTER PROC Emergency.spGetReportDisconnectAreas 
  @aPersianDate AS VARCHAR(10),
  @aAreaIDs AS VARCHAR(2000) = ''
  AS 
  BEGIN

  DECLARE @lSQl AS VARCHAR(MAX) = '
    SELECT A.Area , G.GroupMPFeederName, MPP.MPPostName, MPF.MPFeederName
      ,TMPF.DisconnectDatePersian , TMPF.DisconnectTime 
      ,TMPF.ConnectDatePersian , TMPF.ConnectTime
      ,R.Address
      FROM Emergency.TblTiming T 
      INNER JOIN Emergency.TblTimingMPFeeder TMPF ON T.TimingId = TMPF.TimingId
      INNER JOIN TblRequest R ON TMPF.RequestId = R.RequestId
      INNER JOIN Tbl_MPFeeder MPF ON TMPF.MPFeederId = MPF.MPFeederId
      INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
      INNER JOIN Tbl_Area A ON TMPF.AreaId = A.AreaId
      INNER JOIN Emergency.Tbl_GroupMPFeeder G ON T.GroupMPFeederId = G.GroupMPFeederId
    WHERE TMPF.DisconnectDatePersian = ''' + @aPersianDate + ''''
  IF LEN(@aAreaIDs) > 0 BEGIN
     SET @lSQl = @lSQl + ' AND TMPF.AreaId IN(' + @aAreaIDs + ')'
  END
  SET @lSQl = @lSQl + ' ORDER BY TMPF.AreaId, TMPF.DisconnectDatePersian'
  EXEC(@lSQl)
  END



--     EXEC Emergency.spGetReportDisconnectAreas '1401/02/28' , '2,3'