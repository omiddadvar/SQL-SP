Imports System.Collections.Generic
Imports System.Data.SqlClient
Public Class frmReportPostZamini
    Private mDS As DataSet
    Private mSPName As String
    Private lCnn As SqlConnection
    Private tblData As SortedList(Of String, CmbData)
    Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        mDS = New DataSet
        lCnn = New SqlConnection(GetConnection())
        tblData = New SortedList(Of String, CmbData)
        FillcmbData()
    End Sub
    '----------------------------Event----------------------------Event-----------------------------Event----------------------------
#Region "Events"
    Private Sub frmReportPostZamini_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FillTable("Tbl_Area", cmbArea)
    End Sub
    Private Sub cmbArea_DropDownHided(sender As Object, e As EventArgs) Handles cmbArea.DropDownHided
        AreaChange()
    End Sub
    Private Sub cmbMPPost_DropDownHided(sender As Object, e As EventArgs) Handles cmbMPPost.DropDownHided
        MPPostChange()
    End Sub
    Private Sub cmbMPFeeder_DropDownHided(sender As Object, e As EventArgs) Handles cmbMPFeeder.DropDownHided
        MPFeederChange()
    End Sub
    Private Sub cmbLPPost_DropDownHided(sender As Object, e As EventArgs) Handles cmbLPPost.DropDownHided
        LPPostChange()
    End Sub
#End Region
    '----------------------------Method----------------------------Method----------------------------Method----------------------------
#Region "Methods"
    Private Sub FillTable(tableName As String, ByRef cmb As ChkCombo)
        Dim lSQL As String = "SELECT * FROM " & tableName
        If tableName <> "Tbl_Area" Then lSQL &= " WHERE " & tblData(tableName).ParentID & " IN (" & tblData(tableName).ParentIDs & ")"
        BindingTable(lSQL, lCnn, mDS, tableName, aIsClearTable:=True)
        cmb.Clear()
        If mDS.Tables.Contains(tableName) Then
            cmb.Fill(mDS.Tables(tableName), tblData(tableName).Name, tblData(tableName).ID, 5)
            cmb.Enabled = True
        End If
    End Sub
    Private Sub EmptyTable(tableName As String, ByRef cmb As ChkCombo)
        If mDS.Tables.Contains(tableName) Then
            mDS.Tables(tableName).Clear()
            cmb.Clear()
            cmb.Enabled = False
            tblData(tableName).IDs = ""
            tblData(tableName).ParentIDs = ""
        End If
    End Sub
    Private Sub FillcmbData()
        Dim tmp As CmbData = New CmbData("Area", "AreaId", "")
        tblData.Add("Tbl_Area", tmp)
        tmp = New CmbData("MPPostName", "MPPostId", "AreaId")
        tblData.Add("Tbl_MPPost", tmp)
        tmp = New CmbData("MPFeederName", "MPFeederId", "MPPostId")
        tblData.Add("Tbl_MPFeeder", tmp)
        tmp = New CmbData("LPPostName", "LPPostId", "MPFeederId")
        tblData.Add("Tbl_LPPost", tmp)
    End Sub
    Private Sub AreaChange()
        Dim lAreaIDs As String = cmbArea.GetDataList()
        If lAreaIDs = "" Then lAreaIDs = cmbArea.GetAllList()

        EmptyTable("Tbl_MPPost", cmbMPFeeder)
        EmptyTable("Tbl_MPFeeder", cmbMPFeeder)
        EmptyTable("Tbl_LPPost", cmbLPPost)
        tblData("Tbl_MPPost").ParentIDs = lAreaIDs
        tblData("Tbl_Area").IDs = lAreaIDs
        FillTable("Tbl_MPPost", cmbMPPost)
    End Sub
    Private Sub MPPostChange()
        EmptyTable("Tbl_MPFeeder", cmbMPFeeder)
        EmptyTable("Tbl_LPPost", cmbLPPost)
        tblData("Tbl_MPPost").IDs = cmbMPPost.GetDataList()
        tblData("Tbl_MPFeeder").ParentIDs = cmbMPPost.GetDataList()
        FillTable("Tbl_MPFeeder", cmbMPFeeder)
    End Sub
    Private Sub MPFeederChange()
        EmptyTable("Tbl_LPPost", cmbLPPost)
        tblData("Tbl_MPFeeder").IDs = cmbMPFeeder.GetDataList()
        tblData("Tbl_LPPost").ParentIDs = cmbMPFeeder.GetDataList()
        FillTable("Tbl_LPPost", cmbLPPost)
    End Sub
    Private Sub LPPostChange()
        tblData("Tbl_LPPost").IDs = cmbLPPost.GetDataList()
    End Sub
    Private Sub SetSPName()
        Select Case True
            Case rbSec.Checked
                mSPName = "spGetReport_6_21_01"
            Case rbFuse.Checked
                mSPName = "spGetReport_6_21_02"
            Case rbDej.Checked
                mSPName = "spGetReport_6_21_03"
        End Select
    End Sub
    Private Sub MakeReport()
        SetSPName()
        Dim lSQL As String = String.Format("EXEC {0} '{1}' , '{2}' , '{3}' , '{4}'", mSPName,
            tblData("Tbl_Area").IDs, tblData("Tbl_MPPost").IDs, tblData("Tbl_MPFeeder").IDs, tblData("Tbl_LPPost").IDs)
        BindingTable(lSQL, lCnn, mDS, "Tbl_Report", aIsClearTable:=True)
    End Sub
    Private Class CmbData
        Public ID As String
        Public ParentID As String
        Public Name As String
        Public IDs As String
        Public ParentIDs As String
        Public Sub New(aName As String, aID As String, aParentID As String)
            ID = aID
            Name = aName
            ParentID = aParentID
            ParentIDs = ""
            IDs = ""
        End Sub
    End Class

    Private Sub BttnMakeReport_Click(sender As Object, e As EventArgs) Handles BttnMakeReport.Click
        MakeReport()
    End Sub
#End Region
End Class