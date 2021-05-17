Imports System.Data.SqlClient

Public Class frmRecloserFunction
    Private mRecloserFunctionId As Integer
    Private mCnn As SqlConnection = New SqlConnection(GetConnection())
    Private mDs As New DataSet()
    'Private mDs As New DatasetCcRequester()
    Private mDataRow As DataRow
    Private mEntryDT As DTContainer
    Private mInputDT As DTContainer
    Private mSaveErrorMsg As String
    Private mMPFeederKeyId As Long
    Private mAreaId, mMPPostId, mMPFeederId, mMPCloserTypeId, mRecloserId, mRecloserTypeId, mRecloserModelId As Int64
    Private mRelatedTables As String() = New String() {"Tbl_Area", "Tbl_MPPost", "Tbl_MPFeeder", "Tbl_MPFeederKey"}
    Public Sub New(aRecloserFunctionId As Integer, Optional aMPFeederKeyId As Long = -1)
        MyBase.New()
        InitializeComponent()
        Me.mRecloserFunctionId = aRecloserFunctionId
        Me.mMPFeederKeyId = aMPFeederKeyId
        Me.mSaveErrorMsg = "خطا در ثبت اطلاعات"
    End Sub
    Private Sub frmRecloserFunction_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            loadRecloserTypeData()
            loadRecloserModelData()
            loadAreaData()
            loadData()
        Catch ex As Exception
            ShowError(ex)
            Me.Close()
        End Try
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
        Me.Close()
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
        Me.txtBoxAddress.Text = ""
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
    Private Sub loadData()
        Dim lSQL As String
        If mRecloserFunctionId < 0 Then
            lSQL = "SELECT TOP 0 * FROM TblRecloserFunction"
            BindingTable(lSQL, mCnn, mDs, "TblRecloserFunction", , , , , , , True)
            mDataRow = mDs.Tables("TblRecloserFunction").NewRow()
            mDataRow("RecloserFunctionId") = GetAutoInc()
            mDs.Tables("TblRecloserFunction").Rows.Add(mDataRow)
            Exit Sub
        End If
        lSQL = "SELECT * FROM TblRecloserFunction WHERE RecloserFunctionId = " + mRecloserFunctionId.ToString
        BindingTable(lSQL, mCnn, mDs, "TblRecloserFunction", aIsClearTable:=True)
        mDataRow = mDs.Tables("TblRecloserFunction").Rows(0)
        '-------------------------Initialize GUI
        lSQL = "select a.AreaId, a.Area ,p.MPPostId ,p.MPPostName ,f.MPFeederId , f.MPFeederName ,
             t.MPCloserTypeId , t.MPCloserType , k.MPFeederKeyId , k.KeyName
               from Tbl_MPFeederKey k
            inner join Tbl_MPFeeder f on f.MPFeederId = k.MPFeederId
            inner join Tbl_MPPost p on p.MPPostId = f.MPPostId
            inner join Tbl_Area a on a.AreaId = p.AreaId
            inner join Tbl_MPCloserType t on t.MPCloserTypeId = k.MPCloserTypeId
            where MPFeederKeyId = " + mDataRow("MPFeederKeyId").ToString
        BindingTable(lSQL, mCnn, mDs, "TblLoadCache", aIsClearTable:=True)
        If mDs.Tables("TblLoadCache").Rows.Count = 0 Then
            Exit Sub
        End If
        Dim lLoadData As DataRow = mDs.Tables("TblLoadCache").Rows(0)

        cboArea.SelectedValue = lLoadData("AreaId")
        cboArea_SelectedIndexChanged(cboArea, EventArgs.Empty)
        cboMPPost.SelectedValue = lLoadData("MPPostId")
        cboMPPost_SelectedIndexChanged(cboMPPost, EventArgs.Empty)
        cboMPFeeder.SelectedValue = lLoadData("MPFeederId")
        cboMPFeeder_SelectedIndexChanged(cboMPFeeder, EventArgs.Empty)
        cboKeyType.SelectedValue = lLoadData("MPCloserTypeId")
        cboKeyType_SelectedIndexChanged(cboKeyType, EventArgs.Empty)
        cboRecloser.SelectedValue = lLoadData("MPFeederKeyId")
        cboRecloser_SelectedIndexChanged(cboRecloser, EventArgs.Empty)
        cboRecloserType.SelectedValue = mDataRow("spcRecloserTypeId")
        cboRecloserModel.SelectedValue = mDataRow("spcRecloserModelId")
        txtReadDate.Text = mDataRow("ReadDatePersian")
        txtReadTime.Text = mDataRow("ReadTime")
        txtRestartNumber.Text = mDataRow("RestartCounterCount")
        txtTripNumber.Text = mDataRow("TripCounterCount")
        txtFaultNumber.Text = mDataRow("FaultCounterCount")
    End Sub
    Private Sub save()
        Try
            Dim lDBManipulate As New frmUpdateDataSetBT
            If Not isSaveOk() Then
                Throw New Exception()
            End If
            'mDataRow.MPFeederId = ""   ===> this can be also done!
            mDataRow("MPFeederId") = cboMPFeeder.SelectedValue
            mDataRow("MPFeederKeyId") = cboRecloser.SelectedValue
            mDataRow("spcRecloserTypeId") = If(cboRecloserType.SelectedValue, DBNull.Value)
            mDataRow("spcRecloserModelId") = If(cboRecloserModel.SelectedValue, DBNull.Value)
            mDataRow("ReadDT") = mInputDT.MiladiDate
            mDataRow("ReadDatePersian") = mInputDT.ShamsiDate
            mDataRow("ReadTime") = mInputDT.Time
            mDataRow("RestartCounterCount") = If(txtRestartNumber.Text.Length > 0, Val(txtRestartNumber.Text), 0)
            mDataRow("TripCounterCount") = If(txtTripNumber.Text.Length > 0, Val(txtTripNumber.Text), 0)
            mDataRow("FaultCounterCount") = If(txtFaultNumber.Text.Length > 0, Val(txtFaultNumber.Text), 0)
            mDataRow("DataEntryDT") = mEntryDT.MiladiDate
            mDataRow("DataEntryDatePersian") = mEntryDT.ShamsiDate
            mDataRow("DataEntryTime") = mEntryDT.Time
            mDataRow("AreaUserId") = WorkingUserId

            If lDBManipulate.UpdateDataSet("TblRecloserFunction", mDs) Then
                ShowInfo("ثبت با موفقیت انجام شد.")
                Me.DialogResult = DialogResult.OK
                Me.Close()
            End If
        Catch ex As Exception
            ShowError(mSaveErrorMsg)
            Me.mSaveErrorMsg = "خطا در ثبت اطلاعات"
        End Try
    End Sub
    Private Sub prepareTimes()
        Dim lServerDT As CTimeInfo = GetServerTimeInfo()

        mInputDT = New DTContainer
        txtReadDate.InsertTime(txtReadTime)
        mInputDT.ShamsiDate = txtReadDate.Text
        mInputDT.MiladiDate = txtReadDate.MiladiDT
        mInputDT.Time = txtReadTime.Text

        mEntryDT = New DTContainer
        mEntryDT.ShamsiDate = lServerDT.ShamsiDate
        mEntryDT.MiladiDate = lServerDT.MiladiDate
        mEntryDT.Time = lServerDT.HourMin
    End Sub
    Private Function isSaveOk() As Boolean
        Dim returnVal As Boolean = True
        If cboRecloser.SelectedIndex < 0 Then
            cboArea.Focus()
            cboArea.SelectedIndex = -1
            cboMPPost.SelectedIndex = -1
            cboMPFeeder.SelectedIndex = -1
            cboKeyType.SelectedIndex = -1
            mSaveErrorMsg += vbCrLf + "• " + "ریکلوزر"
            returnVal = False
        End If
        If Not txtReadDate.IsOK Then
            mSaveErrorMsg += vbCrLf + "• " + "تاريخ"
            returnVal = False
        End If
        If Not txtReadTime.IsOK Then
            mSaveErrorMsg += vbCrLf + "• " + "زمان"
            returnVal = False
        End If

        prepareTimes()

        If mInputDT.MiladiDate > mEntryDT.MiladiDate Then
            mSaveErrorMsg += vbCrLf + "• " + "تاریخ و زمان"
            returnVal = False
        End If
        Return returnVal
    End Function
End Class

Public Structure DTContainer
    Public ShamsiDate As String
    Public MiladiDate As Object
    Public Time As String
End Structure
