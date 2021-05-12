Imports System.Data.SqlClient

Public Class frmRecloserFunction

    Private mCnn As SqlConnection = New SqlConnection(GetConnection())
    Private mDs As New DataSet()
    Private mAreaId, mMPPostId, mMPFeederId, mMPCloserTypeId, mRecloserId, mRecloserTypeId, mRecloserModelId As Int64
    Private mRelatedTables As String() = New String() {"Tbl_Area", "Tbl_MPPost", "Tbl_MPFeeder", "Tbl_MPFeederKey"}
    Private Sub frmRecloserFunction_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        loadRecloserTypeData()
        loadRecloserModelData()
        loadAreaData()
    End Sub

    Private Sub cboArea_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboArea.SelectedIndexChanged
        mAreaId = cboArea.SelectedValue
        clearCombo(1)
        loadMPPostData()
    End Sub

    Private Sub cboMPPost_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboMPPost.SelectedIndexChanged
        mMPPostId = cboMPPost.SelectedValue
        clearCombo(2)
        loadMPFeederData()
    End Sub

    Private Sub cboMPFeeder_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboMPFeeder.SelectedIndexChanged
        mMPFeederId = cboMPFeeder.SelectedValue
        clearCombo(3)
        clearRecloserCombo()
        loadKeyData()
    End Sub
    Private Sub cboKeyType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboKeyType.SelectedIndexChanged
        mMPCloserTypeId = cboKeyType.SelectedValue
        clearRecloserCombo()
        loadRecloserData()
    End Sub
    Private Sub cboRecloser_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboRecloser.SelectedIndexChanged
        mRecloserId = cboRecloser.SelectedValue
        loadAddressData()
    End Sub

    Private Sub cboRecloserType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboRecloserType.SelectedIndexChanged
        mRecloserTypeId = cboRecloserType.SelectedValue
    End Sub

    Private Sub cboRecloserModel_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboRecloserModel.SelectedIndexChanged
        mRecloserModelId = cboRecloserModel.SelectedValue
    End Sub
    Private Sub BttnReturn_Click(sender As Object, e As EventArgs) Handles BttnReturn.Click
        Me.Dispose()
    End Sub

    Private Sub ButtonSave_Click(sender As Object, e As EventArgs) Handles ButtonSave.Click
        save()
    End Sub
    Private Sub loadAreaData()
        Dim lSQL As String = "SELECT * FROM Tbl_Area"
        BindingTable(lSQL, mCnn, mDs, "Tbl_Area", cboArea, aIsClearTable:=True)
        Me.cboArea.DataSource = mDs.Tables("Tbl_Area")
        Me.cboArea.SelectedIndex = -1
    End Sub

    Private Sub loadMPPostData()
        If Me.cboArea.SelectedIndex < 0 Then
            Exit Sub
        End If
        Dim lSQL As String = "SELECT * FROM Tbl_MPPost WHERE AreaId = " + mAreaId.ToString
        BindingTable(lSQL, mCnn, mDs, "Tbl_MPPost", cboMPPost, aIsClearTable:=True)
        Me.cboMPPost.DataSource = mDs.Tables("Tbl_MPPost")
        Me.cboMPPost.SelectedIndex = -1
    End Sub
    Private Sub loadMPFeederData()
        If Me.cboMPPost.SelectedIndex < 0 Then
            Exit Sub
        End If
        Dim lSQL As String = "SELECT * FROM Tbl_MPFeeder WHERE MPPostId = " + mMPPostId.ToString
        BindingTable(lSQL, mCnn, mDs, "Tbl_MPFeeder", cboMPFeeder, aIsClearTable:=True)
        Me.cboMPFeeder.DataSource = mDs.Tables("Tbl_MPFeeder")
        Me.cboMPFeeder.SelectedIndex = -1
    End Sub
    Private Sub loadKeyData()
        If Me.cboMPFeeder.SelectedIndex < 0 Then
            Exit Sub
        End If
        Dim lSQL As String = "SELECT * FROM Tbl_MPCloserType"
        BindingTable(lSQL, mCnn, mDs, "Tbl_MPCloserType", cboKeyType, aIsClearTable:=True)
        Me.cboKeyType.DataSource = mDs.Tables("Tbl_MPCloserType")
        Me.cboKeyType.SelectedIndex = -1
    End Sub

    Private Sub loadRecloserData()
        If Me.cboKeyType.SelectedIndex < 0 Then
            Exit Sub
        End If
        Dim lSQL As String = "SELECT * FROM Tbl_MPFeederKey WHERE MPFeederId = " + mMPFeederId.ToString +
            "AND MPCloserTypeId = " + mMPCloserTypeId.ToString
        BindingTable(lSQL, mCnn, mDs, "Tbl_MPFeederKey", cboRecloser, aIsClearTable:=True)
        Me.cboRecloser.DataSource = mDs.Tables("Tbl_MPFeederKey")
        Me.cboRecloser.SelectedIndex = -1
    End Sub

    Private Sub loadAddressData()
        Dim lDataRow As DataRow() = mDs.Tables("Tbl_MPFeederKey").Select("MPFeederKeyId = " + mRecloserId.ToString)
        If lDataRow.Length > 0 AndAlso Not IsDBNull(lDataRow(0).Item("Address")) Then
            txtBoxAddress.Text = lDataRow(0).Item("Address")
        End If

    End Sub
    Private Sub loadRecloserTypeData()
        Dim lSQL As String = "SELECT * FROM TblSpec WHERE SpecTypeId = 107"
        BindingTable(lSQL, mCnn, mDs, "Tbl_RecloserType", cboRecloserType, aIsClearTable:=True)
        Me.cboRecloserType.DataSource = mDs.Tables("Tbl_RecloserType")
        Me.cboRecloserType.SelectedIndex = -1
    End Sub
    Private Sub loadRecloserModelData()
        Dim lSQL As String = "SELECT * FROM TblSpec WHERE SpecTypeId = 108"
        BindingTable(lSQL, mCnn, mDs, "Tbl_RecloserModel", cboRecloserModel, aIsClearTable:=True)
        Me.cboRecloserModel.DataSource = mDs.Tables("Tbl_RecloserModel")
        Me.cboRecloserModel.SelectedIndex = -1
    End Sub
    Private Sub clearCombo(aStep As Integer)
        For i As Integer = aStep To mRelatedTables.Length - 1
            If mDs.Tables.Contains(mRelatedTables(i)) Then
                ClearTable(mDs, mRelatedTables(i))
            End If
        Next
    End Sub
    Private Sub clearRecloserCombo()
        If mDs.Tables.Contains("Tbl_MPFeederKey") Then
            ClearTable(mDs, "Tbl_MPFeederKey")
        End If
    End Sub
    Private Sub save()

    End Sub
End Class