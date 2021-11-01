Imports Tazarv.TacLog
Public Module mdlTacLog
    Private _SubSystem As SubSystem
    Private _LogManager As LogManager
    Private _LogTopic As LogTopic
    Private _FileName, _FolderName, _Message As String
    Private _TC As TC
    Public Sub Initialize()
        _LogManager = LogManager.Create(LogUtilities.FolderLogAlg.Monthly)
        _SubSystem = _LogManager.AddSubSystem("GIS_SendDCInfo_Subsystem")
        _LogTopic = _SubSystem.AddTopic("GIS_SendDCInfo_Logging")
    End Sub
    Public Sub CreateLog(aText As String, aFileName As String, aFolderName As String)
        _FileName = aFileName
        _FolderName = aFolderName
        _Message = aText
        _Trim()
        _SaveLog()
    End Sub
    Private Sub _SaveLog()
        If Not _FileMap.Keys.Contains(_FileName) Then
            Dim lErrLog = String.Format("Unknown in Map; FileName : {0} , FolderName : {1}", _FileName, _FolderName)
            _LogTopic.Error(5, 5000, lErrLog)
            Exit Sub
        End If
        _TC = _FileMap(_FileName)
        Select Case _TC.Type
            Case TC.LogType.DebugLog
                _LogTopic.Debug(_TC.TraceLevel, _TC.ContextId, _Message)
            Case TC.LogType.ErrorLog
                _LogTopic.Error(_TC.TraceLevel, _TC.ContextId, _Message)
            Case TC.LogType.EventLog
                _LogTopic.Event(_TC.TraceLevel, _TC.ContextId, _Message)
        End Select
    End Sub
    Private Sub _Trim()
        Dim lSplitter As String
        If _FileName.Contains("/") Then
            lSplitter = "/"
        ElseIf _FileName.Contains("\\") Then
            lSplitter = "\\"
        Else
            Exit Sub
        End If
        Dim lArray() As String = _FileName.Split(lSplitter)
        _FileName = lArray(lArray.Length - 1)
    End Sub
    Private _FileMap As Dictionary(Of String, TC) = New Dictionary(Of String, TC) From
    {
        {"EventLog.txt", New TC(1, 1000, TC.LogType.EventLog, "EventLog.txt", "")},
        {"SendInfoLog.txt", New TC(1, 1002, TC.LogType.EventLog, "SendInfoLog.txt", "SendInfoLogs")},
        {"SendServiceLog.txt", New TC(2, 1003, TC.LogType.EventLog, "SendServiceLog.txt", "")},
        {"Not_SendServiceLog.txt", New TC(2, 1004, TC.LogType.EventLog, "Not_SendServiceLog.txt", "")},
        {"GeneralEventLog.txt", New TC(2, 1005, TC.LogType.EventLog, "GeneralEventLog.txt", "GeneralServiceLogs")},
        {"GeneralService_Log.txt", New TC(2, 1006, TC.LogType.EventLog, "GeneralService_Log.txt", "GISServiceLogs|AVLServiceLogs|GeneralServiceLogs")},
        {"GISEventLog.txt", New TC(2, 1007, TC.LogType.EventLog, "GISEventLog.txt", "GISServiceLogs")},
        {"SendGISLog.txt", New TC(2, 1008, TC.LogType.EventLog, "SendGISLog.txt", "GISServiceLogs")},
        {"SendGISKeyLog.txt", New TC(2, 1009, TC.LogType.EventLog, "SendGISKeyLog.txt", "")},
        {"GISKey_SendGISKeyLog.txt", New TC(2, 1010, TC.LogType.EventLog, "GISKey_SendGISKeyLog.txt", "")},
        {"Billing_GPS_Log.txt", New TC(2, 1011, TC.LogType.EventLog, "Billing_GPS_Log.txt", "AVLDailyLogs")},
        {"AVLEventLog.txt", New TC(2, 10012, TC.LogType.EventLog, "AVLEventLog.txt", "GISServiceLogs|AVLServiceLogs|GeneralServiceLogs")},
        {"LoadingEventLog.txt", New TC(2, 1013, TC.LogType.EventLog, "LoadingEventLog.txt", "GISServiceLogs|AVLServiceLogs|GeneralServiceLogs")},
        {"ErrorLog.txt", New TC(1, 3000, TC.LogType.ErrorLog, "ErrorLog.txt", "")},
        {"SendServiceErrorLog.txt", New TC(2, 3001, TC.LogType.ErrorLog, "SendServiceErrorLog.txt", "GISServiceLogs|AVLServiceLogs|GeneralServiceLogs")},
        {"SendAVLErrorLog.txt", New TC(2, 3002, TC.LogType.ErrorLog, "SendAVLErrorLog.txt", "GISServiceLogs|AVLServiceLogs|GeneralServiceLogs")},
        {"SendErjaErrorLog.txt", New TC(2, 3003, TC.LogType.ErrorLog, "SendErjaErrorLog.txt", "GISServiceLogs|AVLServiceLogs|GeneralServiceLogs")},
        {"GeneralEventErrorLog.txt", New TC(2, 3004, TC.LogType.ErrorLog, "GeneralEventErrorLog.txt", "GISServiceLogs|AVLServiceLogs|GeneralServiceLogs")},
        {"SendGISKeyErrorLog.txt", New TC(2, 3005, TC.LogType.ErrorLog, "SendGISKeyErrorLog.txt", "")},
        {"AppLog.txt", New TC(1, 2000, TC.LogType.DebugLog, "AppLog.txt", "GISServiceLogs|AVLServiceLogs|GeneralServiceLogs")}
    }
    Private Structure TC
        Public Sub New(aTrace As Integer, aContex As Integer, aType As LogType, aFile As String, aFolder As String)
            Me.TraceLevel = aTrace
            Me.ContextId = aContex
            Me.Type = aType
            Me.File = aFile
            Me.Folder = aFolder
        End Sub
        Public File As String
        Public Folder As String
        Public TraceLevel As Integer
        Public ContextId As Integer
        Public Type As LogType
        Public Enum LogType
            ErrorLog = 0
            EventLog = 1
            DebugLog = 2
        End Enum
    End Structure
End Module
