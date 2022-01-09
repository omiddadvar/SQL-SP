CREATE VIEW ViewServiceLog 
  AS
	SELECT L.ServiceLogId
  	,T.ServiceLogType
  	,A.LogApplication
  	,K.KeyTypeName
  	,L.KeyId
  	,L.Method
  	,LEVEL.LevelName
  	,dbo.MiladiToShamsi(L.LogDT) AS LogPersianDate
  	,dbo.GetTime(L.LogDT) AS LogTime
  	,L.LogDT
  	,L.URL
  	,L.Params
  	,L.Result
  	,L.Error
  	,L.IsSuccessful
  	,L.LogTypeId
  	,L.ApplicationId
  	,L.KeyTypeId
  	,L.LevelId
  FROM TblServiceLog L
  LEFT JOIN Tbl_ServiceLogType T ON L.LogTypeId = T.ServiceLogTypeId
  LEFT JOIN Tbl_ServiceLogApplication A ON L.ApplicationId = A.LogApplicationId
  LEFT JOIN Tbl_ServiceLogKeyType K ON L.KeyTypeId = K.KeyTypeId
  LEFT JOIN Tbl_ServiceLogLevel LEVEL ON L.LevelId = LEVEL.LevelId
