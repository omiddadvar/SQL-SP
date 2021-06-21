Imports System.Data.SqlClient

Public Class frmReportDisHourByHour
    Private mDs As DataSet
    Private mConn As SqlConnection
    Private mAreaIDs As String
    Private mAreaIdArray As String()
    Private mFiltersInfo As String
    Private mIsOk As Boolean
    Private Sub frmReportDisHourByHour_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.mDs = New DataSet
        Me.mConn = New SqlConnection(GetConnection())
        LoadFromDB()
        cmbArea.Fill(mDs.Tables("Tbl_Area"), "Area", "AreaId", 5)
        Me.cmbNwtworkType.Items.AddRange(New Object() {"فشار متوسط", "فوق توزيع", "همه"})
    End Sub
    Private Sub LoadFromDB()
        Dim lSQL As String = "SELECT * FROM Tbl_Area"
        BindingTable(lSQL, mConn, mDs, "Tbl_Area", , , , , , , True)
    End Sub
    Private Sub BttnMakeReport_Click(sender As Object, e As EventArgs) Handles BttnMakeReport.Click
        Try
            check()
            If Not mIsOk Then Exit Sub
            MakeReport()
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub
    Private Sub MakeReport()
        Dim Desktop As String = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        Dim lRepNo As String = "10-4"
        Dim lTitle As String = "گزارش آماري عملکرد ريکلوزرهای فشار متوسط"
        Dim lRepTitle As String = "گزارش عملکرد ريکلوزرهای فشار متوسط"
        'Dim lFileName As String = "Reports\Report_DisHourly.mrt"
        Dim lFileName As String = Desktop & "\Report_DisHourly.mrt"
        Dim lCnn As New SqlConnection(GetConnection())
        MakeData()
        '-------------StimulSoft Report----------------
        If Not mDs.Tables.Contains("Tbl_Report") Then
            Exit Sub
        End If
        mDs.WriteXml(Desktop & "/Report_10_4.xml", XmlWriteMode.WriteSchema)
        Dim lDlg As New frmReportPreviewStim("", lFileName, lTitle, lRepTitle, , mFiltersInfo, , lRepNo, , , mDs)
        lDlg.Show()
    End Sub
    Private Sub check()
        If Not checkDate() Then Exit Sub
        checkArea()
        checkNetworkType()
    End Sub
    Private Function checkDate() As Boolean
        If txtDate.Text = "____/__/__" Then
            MsgBoxF("تاريخ الزامي مي‌باشد", "توجه", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Information, MessageBoxDefaultButton.Button1)
            mIsOk = False
            Return False
        End If
        mFiltersInfo &= " - تاريخ " & ConvertReportDate(txtDate.Text)
        Return True
    End Function
    Private Sub checkArea()
        If mAreaIDs = "" Then
            mAreaIDs = cmbArea.GetDataList()
        End If
        If mAreaIDs <> "" Then
            mAreaIdArray = mAreaIDs.Split(",")
            If mAreaIdArray.Length > 1 Then
                mFiltersInfo &= " نواحي "
            Else
                mFiltersInfo &= " ناحيه "
            End If
            mFiltersInfo &= cmbArea.GetDataTextList()
            mIsOk = True
        End If
    End Sub
    Private Sub checkNetworkType()
        If cmbNwtworkType.SelectedIndex >= 0 Then
            Select Case cmbNwtworkType.Text
                Case "فشار متوسط"
                    mFiltersInfo &= " - شبکه فشار متوسط"
                Case "فوق توزيع"
                    mFiltersInfo &= " - شبکه فوق توزيع"
            End Select
        End If
    End Sub
    Private Sub MakeData()
        Dim lSQL As String
        Dim lMiladiDate As String = txtDate.MiladiDT.ToString().Substring(0, 10)
        If mDs.Tables.Contains("Tbl_Report") Then
            mDs.Tables("Tbl_Report").Clear()
        End If
        For Each areaId As String In mAreaIdArray
            lSQL = String.Format("EXEC spOmid '{0}','{1}','{2}'", areaId, txtDate.Text, lMiladiDate)
            BindingTable(lSQL, mConn, mDs, "Tbl_Report", aIsClearTable:=False)
        Next
        lSQL = String.Format("EXEC spOmid '{0}','{1}','{2}'", mAreaIDs, txtDate.Text, lMiladiDate)
        BindingTable(lSQL, mConn, mDs, "Tbl_Report", aIsClearTable:=False)
    End Sub
End Class