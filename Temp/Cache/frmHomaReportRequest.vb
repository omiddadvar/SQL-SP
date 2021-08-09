Imports System.Data.SqlClient
Public Class frmHomaReportRequest
    Private mDS As DataSet,
        mSQL As String,
        mConn As SqlConnection,
        mWhere As String,
        mFilterInfo As String
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        InitData()
    End Sub
#Region "Events"
    '-------------------------------Events----------------------------------------
    Private Sub frmHomaReportRequest_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitializeAreaCombo()
    End Sub
    Private Sub btnMakeReport_Click(sender As Object, e As EventArgs) Handles btnMakeReport.Click

    End Sub
#End Region
#Region "Methods"
    '-------------------------------Methods---------------------------------------
    Private Sub InitData()
        mDS = New DataSet
        mConn = New SqlConnection(GetConnection())
    End Sub
    Private Sub InitializeAreaCombo()
        LoadAreaDataTable()
        cmbArea.Fill(mDS.Tables("Tbl_Area"), "Area", "AreaId", 20)
    End Sub
    Private Sub LoadAreaDataTable(Optional ByRef cbo As ComboBox = Nothing, Optional ByVal aIsShowCenterAndSetad As Boolean = False, Optional ByVal aIsForceShowSetad As Boolean = False, Optional ByVal aIsNotShowSetad As Boolean = False)
        Dim lWhere As String
        Dim lLegalArea As String = ""
        Dim lSQL As String

        If aIsShowCenterAndSetad Then
            lWhere = ""
            If aIsNotShowSetad Then
                lWhere = "IsSetad<>1"
            End If
        Else
            lWhere = "IsCenter<>1 and IsSetad<>1"
        End If

        If IsCenter Then
            lLegalArea = GetLegalArea(mConn, mDS) ', aIsShowCenterAndSetad)
        End If

        If IsSetadMode Or mIsSetadVisible Then
            Dim lWhr As String = ""
            lWhr =
                lWhere &
                IIf(lWhere <> "" And lLegalArea <> "", " AND ", "") & lLegalArea

            If lWhr <> "" Then
                lWhr = "(" & lWhr & ")" & IIf(aIsForceShowSetad, " OR (AreaId = 99)", "")
            End If

            lSQL =
                "SELECT Tbl_Area.* FROM Tbl_Area " &
                IIf(lWhr <> "", " WHERE ", "") & lWhr &
               " ORDER BY Area"
        Else
            lSQL =
                "SELECT Tbl_Area.* FROM Tbl_Area WHERE (" &
                lWhere &
                IIf(lWhere = "", "", " AND ") & " Server121Id=" & WorkingServer121Id &
                IIf(lLegalArea <> "", " AND ", "") & lLegalArea &
                ") " &
                IIf(aIsForceShowSetad, " OR (AreaId = 99)", "") &
                "ORDER BY Area"
        End If

        Try : mDS.Tables("Tbl_Area").Clear() : Catch ex As Exception : End Try
        BindingTable(lSQL, mConn, mDS, "Tbl_Area")

        If Not IsNothing(cbo) Then
            Try
                cbo.SelectedValue = WorkingAreaId
            Catch ex As Exception

            End Try

        End If

    End Sub
    Private Sub MakeReportData()
        Try
            mSQL = "EXEC spGetcdcdcddcdd '" & "','" & "','" & "'"
            BindingTable(mSQL, mConn, mDS, "TblMasterTrace", aIsClearTable:=True)
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub
    Private Sub MakeReport()
        Dim lRepName As String = ""
        Try
            If mDS.Tables.Contains("TblMasterTrace") Then
                mDS.WriteXml(ReportsXMLPath & "TblMasterTrace.xml", XmlWriteMode.WriteSchema)
                Dim lDlg As New frmReportPreviewStim("", "Reports\rptOmid.mrt", lRepName, lRepName, , mFilterInfo, , "8-50", , , mDS)
                lDlg.Show()
            End If
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub
#End Region
End Class