 SELECT Tbl_MPPart.MPPart, MPRP.MPRequestId, 
    MPRP.MPPartValueMansubeh, MPRP.MPPartValueZayeaat,
    MPRP.MPPartValueBarkenar, MPRP.MPPartValueSerghat, 
    R.RequestNumber, RP_MP.Used_NoDraft_Count, 
    RP_MP.Used_Draft_Count, RP_MP.New_NoDraft_Count, 
    RP_MP.New_Draft_Count
 FROM   (TblRequestPart RP_MP 
LEFT OUTER JOIN (
    (TblRequest R INNER JOIN TblMPRequest MPR ON R.MPRequestId = MPR.MPRequestId) 
            INNER JOIN TblMPRequestPart MPRP ON MPR.MPRequestId = MPRP.MPRequestId)
  ON RP_MP.MPRequestPartId = MPRP.MPRequestPartId) 
INNER JOIN Tbl_MPPart Tbl_MPPart ON MPRP.MPPartId = Tbl_MPPart.MPPartId
 WHERE  R.RequestNumber = 9699196
----------------------------------------------------------------------------------------------------------------------

  SELECT * FROM Tbl_MPPart tm
  
  SELECT tr.DisconnectDatePersian,* FROM TblRequest tr WHERE tr.RequestNumber = 9699196
  
  SELECT tm.MPPart,trp.* FROM TblMPRequestPart tmp 
    INNER JOIN Tbl_MPPart tm ON tm.MPPartId = tmp.MPPartId
    LEFT JOIN TblRequestPart trp ON trp.MPRequestPartId = tmp.MPRequestPartId
    WHERE tmp.MPRequestId = 99100000087248

SELECT * FROM TblRequestPart WHERE MPRequestPartId = 990161905