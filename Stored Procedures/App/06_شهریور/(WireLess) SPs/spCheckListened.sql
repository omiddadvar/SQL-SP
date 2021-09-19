USE WirelessDB
GO
CREATE PROCEDURE spCheckListened
  @aMediaIds VARCHAR(2000)
  ,@aSourceUserId INT
  ,@aDestId INT
  ,@aIsChannel BIT
  AS
  BEGIN
    DECLARE @lSQL VARCHAR(5000) = ''
    IF @aIsChannel = 1 BEGIN 
      SET @lSQL = 'SELECT * FROM TblChannelOfflineStatus
        WHERE MediaId IN ('+ @aMediaIds +') AND DestChannelId = ' + CAST(@aDestId AS VARCHAR(10))
    END
    ELSE BEGIN
        SET @lSQL = 'SELECT * FROM TblUserOfflineStatus
        WHERE MediaId IN ('+ @aMediaIds +') AND UserId = ' +
          CAST(@aSourceUserId AS VARCHAR(10)) + ' AND DestUserId = ' + CAST(@aDestId AS VARCHAR(10))
    END
    EXEC(@lSQL)
  END

EXEC spCheckListened 1062 , 28 , 5 , 1
