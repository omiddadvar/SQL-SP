ALTER PROC Emergency.spGetReportDisconnectAreas 
  @aPersianDate AS VARCHAR(10),
  @aAreaIDs AS VARCHAR(2000) = ''
  AS 
  BEGIN

  DECLARE @lSQl AS VARCHAR(MAX) = '
    SELECT TMPF.RequestId, A.Area , G.GroupMPFeederName, MPP.MPPostName, MPF.MPFeederName
      ,R.TamirDisconnectFromDatePersian AS DisconnectDatePersian
      ,R.TamirDisconnectFromTime AS DisconnectTime
      ,R.TamirDisconnectToTime AS ConnectDatePersian
      ,R.TamirDisconnectToTime AS ConnectTime
      ,R.Address
    FROM Emergency.TblTiming T 
      INNER JOIN Emergency.TblTimingMPFeeder TMPF ON T.TimingId = TMPF.TimingId
      INNER JOIN TblRequest R ON TMPF.RequestId = R.RequestId
      INNER JOIN Tbl_MPFeeder MPF ON TMPF.MPFeederId = MPF.MPFeederId
      INNER JOIN Tbl_MPPost MPP ON MPF.MPPostId = MPP.MPPostId
      INNER JOIN Tbl_Area A ON TMPF.AreaId = A.AreaId
      INNER JOIN Emergency.Tbl_GroupMPFeeder G ON T.GroupMPFeederId = G.GroupMPFeederId
    WHERE R.EndJobStateId NOT IN (2,3) AND TMPF.DisconnectDatePersian = ''' + @aPersianDate + ''''
  IF LEN(@aAreaIDs) > 0 BEGIN
     SET @lSQl = @lSQl + ' AND TMPF.AreaId IN(' + @aAreaIDs + ')'
  END
  SET @lSQl = @lSQl + ' ORDER BY TMPF.AreaId, TMPF.DisconnectDatePersian'
  EXEC(@lSQl)
  END

/*

EXEC Emergency.spGetReportDisconnectAreas '1401/03/24' , '2,3'

*/
