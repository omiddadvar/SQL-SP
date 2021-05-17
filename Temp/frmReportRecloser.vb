Imports System.Data.SqlClient

Public Class frmReportRecloser
    Private mCnn As SqlConnection = New SqlConnection(GetConnection())
    Private mDs As New DataSet()
    Private mWhere As String
    Private mErrorMessage As String
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Private Sub frmReportRecloser_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadArea()
        loadRecloserModel()
    End Sub
    Private Sub cmbArea_CloseDropDown(sender As Object, e As EventArgs) Handles cmbArea.DropDownHided
        LoadMPPost()
        cmbMPFeeder.Clear()
    End Sub
    Private Sub cmbMPPost_CloseDropDown(sender As Object, e As EventArgs) Handles cmbMPPost.DropDownHided
        loadMPFeeder()
    End Sub
    Private Sub btnMakeReport_Click(sender As Object, e As EventArgs) Handles btnMakeReport.Click
        GetReport()
    End Sub
    Private Sub LoadArea()
        Dim lSQL As String = "SELECT * FROM Tbl_Area"
        BindingTable(lSQL, mCnn, mDs, "Tbl_Area", aIsClearTable:=True)
        cmbArea.Fill(mDs.Tables("Tbl_Area"), "Area", "AreaId", 20)
        cmbArea.Enabled = True
    End Sub
    Private Sub LoadMPPost()
        Dim lAreaIDs As String = cmbArea.GetDataList()
        If lAreaIDs.Length = 0 Then
            Exit Sub
        End If
        Dim lSQL As String = "SELECT * FROM Tbl_MPPost WHERE AreaId IN (" & lAreaIDs & ")"
        BindingTable(lSQL, mCnn, mDs, "Tbl_MPPost", aIsClearTable:=True)
        cmbMPPost.Fill(mDs.Tables("Tbl_MPPost"), "MPPostName", "MPPostId", 10)
        cmbMPPost.Enabled = True
    End Sub
    Private Sub loadMPFeeder()
        Dim lPostIDs As String = cmbMPPost.GetDataList()
        If lPostIDs.Length = 0 Then
            Exit Sub
        End If
        Dim lSQL As String = "SELECT * FROM Tbl_MPFeeder WHERE MPPostId IN (" & lPostIDs & ")"
        BindingTable(lSQL, mCnn, mDs, "Tbl_MPFeeder", aIsClearTable:=True)
        cmbMPFeeder.Fill(mDs.Tables("Tbl_MPFeeder"), "MPFeederName", "MPFeederId", 10)
        cmbMPFeeder.Enabled = True
    End Sub
    Private Sub loadRecloserModel()
        Dim lSQL As String = "SELECT * FROM TblSpec WHERE SpecTypeId = 108"
        BindingTable(lSQL, mCnn, mDs, "Tbl_RecloserModel", aIsClearTable:=True)
        cmbRecloserModel.Fill(mDs.Tables("Tbl_RecloserModel"), "SpecValue", "SpecId", 10)
        cmbRecloserModel.Enabled = True
    End Sub
    Private Sub AssignFilters(ByRef aSQL As String)
        mWhere = ""
        Dim lFilterFlag As Boolean = False
        Dim lAreaIDs As String = cmbArea.GetDataList()
        Dim lPostIDs As String = cmbMPPost.GetDataList()
        Dim lFeederIDs As String = cmbMPFeeder.GetDataList()
        Dim lRecloserModelIDs As String = cmbRecloserModel.GetDataList()
        Dim lFaultNumberTxt As String = txtFaultNumber.Text
        If lAreaIDs.Length > 0 Then
            AddFilter("A.AreaId", lAreaIDs, aIsRange:=True)
            lFilterFlag = True
        End If
        If lPostIDs.Length > 0 Then
            AddFilter("P.MPPostId", lPostIDs, aIsRange:=True)
            lFilterFlag = True
        End If
        If lFeederIDs.Length > 0 Then
            AddFilter("F.MPFeederId", lFeederIDs, aIsRange:=True)
            lFilterFlag = True
        End If
        If lRecloserModelIDs.Length > 0 Then
            AddFilter("RM.SpecId", lRecloserModelIDs, aIsRange:=True)
            lFilterFlag = True
        End If
        If lFaultNumberTxt.Length > 0 Then
            AddFilter("RF.FaultCounterCount", Val(lFaultNumberTxt), aOperation:=">")
            lFilterFlag = True
        End If
        If txtFromDate.Text <> "____/__/__" Then
            AddFilter("RF.ReadDatePersian", "'" & txtFromDate.Text & "'", aOperation:=">")
            lFilterFlag = True
        End If
        If txtToDate.Text <> "____/__/__" Then
            AddFilter("RF.ReadDatePersian", "'" & txtToDate.Text & "'", aOperation:="<")
            lFilterFlag = True
        End If
        mWhere &= " ORDER BY RF.FaultCounterCount DESC"
        If lFilterFlag Then
            aSQL &= " WHERE" & mWhere.Substring(4)
        End If
    End Sub
    Private Function IsReportOk() As Boolean
        Dim lIsOk As Boolean = True
        Me.mErrorMessage = "خطا در ايجاد گزارش، اطلاعات ورودی را بازبيني کنید"
        Dim lFromDate As String = txtFromDate.Text
        Dim lToDate As String = txtToDate.Text
        Dim lFaultNumberTxt As String = txtFaultNumber.Text
        Dim lFaultNumber As Long
        If lFaultNumberTxt.Length > 0 AndAlso Not Long.TryParse(lFaultNumberTxt, lFaultNumber) Then
            mErrorMessage += vbCrLf + "• " + "حداقل تعداد قطعي"
            mErrorMessage += vbCrLf + "-> " + lFaultNumberTxt
            lIsOk = False
        End If
        If txtFromDate.Text <> "____/__/__" AndAlso Not txtFromDate.IsOK Then
            mErrorMessage += vbCrLf + "• " + "تاريخ شروع"
            mErrorMessage += vbCrLf + "-> " + txtFromDate.Text
            lIsOk = False
        End If
        If txtToDate.Text <> "____/__/__" AndAlso Not txtToDate.IsOK Then
            mErrorMessage += vbCrLf + "• " + "تاريخ پايان"
            mErrorMessage += vbCrLf + "-> " + txtToDate.Text
            lIsOk = False
        End If
        Return lIsOk
    End Function
    Private Sub GetReport()
        Dim lSQL As String = "SELECT A.Area ,P.MPPostName ,F.MPFeederName ,
	            FK.KeyName As Recloser, FK.Address , RM.SpecValue AS RecloserModel , 
	            RT.SpecValue AS RecloserType, RF.ReadDatePersian As ReadDate , RF.ReadTime,
	            RF.RestartCounterCount , RF.TripCounterCount , RF.FaultCounterCount
               FROM TblRecloserFunction RF
            INNER JOIN Tbl_MPFeederKey FK on FK.MPFeederKeyId = RF.MPFeederKeyId
            INNER JOIN Tbl_MPFeeder F on F.MPFeederId = FK.MPFeederId
            INNER JOIN Tbl_MPPost p on P.MPPostId = F.MPPostId
            INNER JOIN Tbl_Area A on a.AreaId = P.AreaId
            INNER JOIN TblSpec RM on RM.SpecId = RF.spcRecloserModelId 
            INNER JOIN TblSpec RT on RT.SpecId = RF.spcRecloserTypeId 
            INNER JOIN Tbl_MPCloserType CT on ct.MPCloserTypeId = FK.MPCloserTypeId"
        Try
            If Not IsReportOk() Then
                Throw New Exception
            End If
            AssignFilters(lSQL)
            BindingTable(lSQL, mCnn, mDs, "Tbl_Report", aIsClearTable:=True)
            Dim p As Int16 = 10
        Catch ex As Exception
            ShowError(mErrorMessage + vbCrLf + vbCrLf + vbCrLf + ex.Message)
        End Try
    End Sub
    Private Sub AddFilter(aFilter As String, aValue As String, Optional aIsRange As Boolean = False)
        mWhere &= " AND " & aFilter
        mWhere &= If(aIsRange, " IN (", " = ")
        mWhere &= aValue
        mWhere &= If(aIsRange, ")", "")
    End Sub
    Private Sub AddFilter(aFilter As String, aValue As String, aOperation As String)
        mWhere &= " AND " & aFilter
        mWhere &= " " & aOperation & " "
        mWhere &= aValue
    End Sub
End Class
