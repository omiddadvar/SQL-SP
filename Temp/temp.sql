SELECT *
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME like 'tbltrace'
AND COLUMN_NAME like '%trace%'

  SELECT TOP 100 * FROM Homa.TblTrace tt
  
  SELECT TOP 10 * FROM Homa.Tbl_Tablet
  
  SELECT TOP 10 * FROM dbo.Tbl_Master
  
  SELECT TOP 10 * FROM dbo.Tbl_Area
  
  SELECT TOP 10 * FROM Homa.TblOnCall
  
  SELECT TOP 10 * FROM TblRequest
  
  
    SELECT T.TabletName , M.Name FROM Homa.TblOnCall O
    inner join dbo.Tbl_Master M on O.MasterId = M.MasterId
    inner join  Homa.Tbl_Tablet T on O.TabletId = T.TabletId
    where OnCallId = 9900000000000037