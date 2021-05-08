Imports System.Data.SqlClient

Public Class frmManagerSMS_DC
    Inherits FormBase

#Region "Data Members"
    Dim mCnn As SqlConnection = New SqlConnection(GetConnection())
    Private mDs As New DataSet
    Private mTblSMS As New DataTable
    Private mRowEditing As DataRow
    Private m_ManagerSMSDCId As Integer
    Friend WithEvents chkIsMPFeederHighPower As System.Windows.Forms.CheckBox
    Friend WithEvents txtMPFeederHighPowerCount As Bargh_Common.TextBoxPersian
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents picLockMPFeederHighPower As System.Windows.Forms.PictureBox
    Friend WithEvents chkIsTamir_NotConfirmLP As System.Windows.Forms.CheckBox
    Friend WithEvents chkIsTamir_ConfirmLP As System.Windows.Forms.CheckBox
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents chkIsTamir_NotConfirmFT As System.Windows.Forms.CheckBox
    Friend WithEvents chkIsTamir_ConfirmFT As System.Windows.Forms.CheckBox
    Friend WithEvents HatchPanel17 As Bargh_Common.HatchPanel
    Friend WithEvents chkIsSendAlarmForTamir As System.Windows.Forms.CheckBox
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents picLockSendAlarmForTamir As System.Windows.Forms.PictureBox
    Friend WithEvents lblSendAlarmForTamirPercent As System.Windows.Forms.Label
    Friend WithEvents Label46 As System.Windows.Forms.Label
    Friend WithEvents HatchPanel18 As Bargh_Common.HatchPanel
    Friend WithEvents chkIsSendSMSForWaitConfirm As System.Windows.Forms.CheckBox
    Friend WithEvents picLockSendSMSForWaitConfirm As System.Windows.Forms.PictureBox
    Friend WithEvents lblSendSMSForWaitConfirmTime As System.Windows.Forms.Label
    Friend WithEvents Label47 As System.Windows.Forms.Label
    Friend WithEvents chkIsTamirReturned As System.Windows.Forms.CheckBox
    Friend WithEvents HatchPanel19 As HatchPanel
    Friend WithEvents chkIsSendSMSForLongTamirDC As CheckBox
    Friend WithEvents PickLockSendSMSForLongTamirDC As PictureBox
    Friend WithEvents txtSendSMSForLongTamirTime As TextBoxPersian
    Friend WithEvents Label48 As Label
    Friend WithEvents Label49 As Label
    Friend WithEvents Label50 As Label
    Friend WithEvents txtNew_MPF_Min As TextBoxPersian
    Friend WithEvents chkIsNew_MPF As CheckBox
    Friend WithEvents picLockRequestNewMP As PictureBox
    Private mDisconnectReasons As String = ""

#End Region

#Region " Windows Form Designer generated code "

    Public Sub New(Optional ByVal aManagerSMSDCId As Integer = -1)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        m_ManagerSMSDCId = aManagerSMSDCId
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents cmbArea As Bargh_Common.ComboBoxPersian
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnReturn As System.Windows.Forms.Button
    Friend WithEvents chkIsActive As System.Windows.Forms.CheckBox
    Friend WithEvents txtMobile As Bargh_Common.TextBoxPersian
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents chkIsMP_NT As System.Windows.Forms.CheckBox
    Friend WithEvents chkIsLP_T As System.Windows.Forms.CheckBox
    Friend WithEvents chkISMP_T As System.Windows.Forms.CheckBox
    Friend WithEvents chkIsFT As System.Windows.Forms.CheckBox
    Friend WithEvents txtLP_NTMin As Bargh_Common.TextBoxPersian
    Friend WithEvents txtMP_NTMin As Bargh_Common.TextBoxPersian
    Friend WithEvents txtFT_Min As Bargh_Common.TextBoxPersian
    Friend WithEvents txtLP_TMin As Bargh_Common.TextBoxPersian
    Friend WithEvents txtMP_TMin As Bargh_Common.TextBoxPersian
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents BracketPanel1 As Bargh_Common.BracketPanel
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents chkISLP_NT_CN As System.Windows.Forms.CheckBox
    Friend WithEvents chkIsMP_NT_CN As System.Windows.Forms.CheckBox
    Friend WithEvents chkIsLP_T_CN As System.Windows.Forms.CheckBox
    Friend WithEvents chkISMP_T_CN As System.Windows.Forms.CheckBox
    Friend WithEvents chkIsFT_CN As System.Windows.Forms.CheckBox
    Friend WithEvents chkIsTransFault As System.Windows.Forms.CheckBox
    Friend WithEvents HatchPanel1 As Bargh_Common.HatchPanel
    Friend WithEvents HatchPanel2 As Bargh_Common.HatchPanel
    Friend WithEvents chkIsMP_NT_MPFDC As System.Windows.Forms.CheckBox
    Friend WithEvents HatchPanel3 As Bargh_Common.HatchPanel
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents BracketPanel2 As Bargh_Common.BracketPanel
    Friend WithEvents chkIsTamir_Confirm As System.Windows.Forms.CheckBox
    Friend WithEvents chkIsTamir_NotConfirm As System.Windows.Forms.CheckBox
    Friend WithEvents HatchPanel4 As Bargh_Common.HatchPanel
    Friend WithEvents chkIsAfterEdit As System.Windows.Forms.CheckBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents HatchPanel5 As Bargh_Common.HatchPanel
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents txtMP_TMin_MPFDC As Bargh_Common.TextBoxPersian
    Friend WithEvents txtMP_NTMin_MPFDC As Bargh_Common.TextBoxPersian
    Friend WithEvents txtFT_Min_MPPDC As Bargh_Common.TextBoxPersian
    Friend WithEvents chkIsFT_MPPDC As System.Windows.Forms.CheckBox
    Friend WithEvents chkIsMP_T_MPFDC As System.Windows.Forms.CheckBox
    Friend WithEvents picLockAfterEdit As System.Windows.Forms.PictureBox
    Friend WithEvents picLockFT As System.Windows.Forms.PictureBox
    Friend WithEvents chkIsMPF_nTime As System.Windows.Forms.CheckBox
    Friend WithEvents txtMPF_CountOfnTime As Bargh_Common.TextBoxPersian
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents picLockMPF_nTime As System.Windows.Forms.PictureBox
    Friend WithEvents btnSelectDisconnectGroup As System.Windows.Forms.Button
    Friend WithEvents chkIsDGSRequest As System.Windows.Forms.CheckBox
    Friend WithEvents picLockDGSRequest As System.Windows.Forms.PictureBox
    Friend WithEvents HatchPanel6 As Bargh_Common.HatchPanel
    Friend WithEvents chkIsSerghat As System.Windows.Forms.CheckBox
    Friend WithEvents picLockSerghat As System.Windows.Forms.PictureBox
    Friend WithEvents chkIsLP_NT As System.Windows.Forms.CheckBox
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents Label34 As System.Windows.Forms.Label
    Friend WithEvents Label35 As System.Windows.Forms.Label
    Friend WithEvents Label36 As System.Windows.Forms.Label
    Friend WithEvents Label37 As System.Windows.Forms.Label
    Friend WithEvents Label38 As System.Windows.Forms.Label
    Friend WithEvents Label39 As System.Windows.Forms.Label
    Friend WithEvents Label40 As System.Windows.Forms.Label
    Friend WithEvents Label41 As System.Windows.Forms.Label
    Friend WithEvents Label42 As System.Windows.Forms.Label
    Friend WithEvents Label43 As System.Windows.Forms.Label
    Friend WithEvents Label44 As System.Windows.Forms.Label
    Friend WithEvents Label45 As System.Windows.Forms.Label
    Friend WithEvents HatchPanel7 As Bargh_Common.HatchPanel
    Friend WithEvents HatchPanel8 As Bargh_Common.HatchPanel
    Friend WithEvents HatchPanel9 As Bargh_Common.HatchPanel
    Friend WithEvents HatchPanel10 As Bargh_Common.HatchPanel
    Friend WithEvents HatchPanel11 As Bargh_Common.HatchPanel
    Friend WithEvents HatchPanel12 As Bargh_Common.HatchPanel
    Friend WithEvents HatchPanel13 As Bargh_Common.HatchPanel
    Friend WithEvents HatchPanel14 As Bargh_Common.HatchPanel
    Friend WithEvents HatchPanel15 As Bargh_Common.HatchPanel
    Friend WithEvents txtMP_NTSingleMin As Bargh_Common.TextBoxPersian
    Friend WithEvents txtMP_NTAllMin As Bargh_Common.TextBoxPersian
    Friend WithEvents chkIsMP_NTSingle As System.Windows.Forms.CheckBox
    Friend WithEvents chkIsMP_NTAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkIsNew As System.Windows.Forms.CheckBox
    Friend WithEvents txtNew_Min As Bargh_Common.TextBoxPersian
    Friend WithEvents chkIsMP_TAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkIsMP_TSingle As System.Windows.Forms.CheckBox
    Friend WithEvents txtMP_TAllMin As Bargh_Common.TextBoxPersian
    Friend WithEvents txtMP_TSingleMin As Bargh_Common.TextBoxPersian
    Friend WithEvents chkIsLP_NTAll As System.Windows.Forms.CheckBox
    Friend WithEvents txtLP_NTAllMin As Bargh_Common.TextBoxPersian
    Friend WithEvents chkIsLP_NTSingle As System.Windows.Forms.CheckBox
    Friend WithEvents txtLP_NTSingleMin As Bargh_Common.TextBoxPersian
    Friend WithEvents txtLP_TSingleMin As Bargh_Common.TextBoxPersian
    Friend WithEvents txtLP_TAllMin As Bargh_Common.TextBoxPersian
    Friend WithEvents chkIsLP_TSingle As System.Windows.Forms.CheckBox
    Friend WithEvents chkIsLP_TAll As System.Windows.Forms.CheckBox
    Friend WithEvents pnlSinglOrAll As System.Windows.Forms.Panel
    Friend WithEvents picLockIsMP_TSingle As System.Windows.Forms.PictureBox
    Friend WithEvents picLockIsLP_TSingle As System.Windows.Forms.PictureBox
    Friend WithEvents picLockIsMP_NTSingle As System.Windows.Forms.PictureBox
    Friend WithEvents picLockIsLP_NTSingle As System.Windows.Forms.PictureBox
    Friend WithEvents picLockIsMP_TAll As System.Windows.Forms.PictureBox
    Friend WithEvents picLockIsLP_TAll As System.Windows.Forms.PictureBox
    Friend WithEvents picLockIsMP_NTAll As System.Windows.Forms.PictureBox
    Friend WithEvents picLockIsLP_NTAll As System.Windows.Forms.PictureBox
    Friend WithEvents chkIsCriticalFeeders As System.Windows.Forms.CheckBox
    Friend WithEvents HatchPanel16 As Bargh_Common.HatchPanel
    Friend WithEvents picLockCriticalFeeders As System.Windows.Forms.PictureBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmManagerSMS_DC))
        Me.chkIsActive = New System.Windows.Forms.CheckBox()
        Me.cmbArea = New Bargh_Common.ComboBoxPersian()
        Me.txtMobile = New Bargh_Common.TextBoxPersian()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnReturn = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label49 = New System.Windows.Forms.Label()
        Me.Label50 = New System.Windows.Forms.Label()
        Me.txtNew_MPF_Min = New Bargh_Common.TextBoxPersian()
        Me.txtSendSMSForLongTamirTime = New Bargh_Common.TextBoxPersian()
        Me.PickLockSendSMSForLongTamirDC = New System.Windows.Forms.PictureBox()
        Me.HatchPanel19 = New Bargh_Common.HatchPanel()
        Me.HatchPanel18 = New Bargh_Common.HatchPanel()
        Me.HatchPanel17 = New Bargh_Common.HatchPanel()
        Me.picLockMPFeederHighPower = New System.Windows.Forms.PictureBox()
        Me.picLockCriticalFeeders = New System.Windows.Forms.PictureBox()
        Me.HatchPanel16 = New Bargh_Common.HatchPanel()
        Me.chkIsMPFeederHighPower = New System.Windows.Forms.CheckBox()
        Me.chkIsCriticalFeeders = New System.Windows.Forms.CheckBox()
        Me.picLockIsMP_TSingle = New System.Windows.Forms.PictureBox()
        Me.picLockIsLP_TSingle = New System.Windows.Forms.PictureBox()
        Me.picLockIsMP_NTSingle = New System.Windows.Forms.PictureBox()
        Me.picLockIsLP_NTSingle = New System.Windows.Forms.PictureBox()
        Me.picLockIsMP_TAll = New System.Windows.Forms.PictureBox()
        Me.picLockIsLP_TAll = New System.Windows.Forms.PictureBox()
        Me.picLockIsMP_NTAll = New System.Windows.Forms.PictureBox()
        Me.picLockIsLP_NTAll = New System.Windows.Forms.PictureBox()
        Me.HatchPanel15 = New Bargh_Common.HatchPanel()
        Me.HatchPanel14 = New Bargh_Common.HatchPanel()
        Me.HatchPanel13 = New Bargh_Common.HatchPanel()
        Me.HatchPanel12 = New Bargh_Common.HatchPanel()
        Me.HatchPanel11 = New Bargh_Common.HatchPanel()
        Me.txtLP_NTMin = New Bargh_Common.TextBoxPersian()
        Me.txtMP_NTMin = New Bargh_Common.TextBoxPersian()
        Me.txtLP_TMin = New Bargh_Common.TextBoxPersian()
        Me.txtMP_TMin = New Bargh_Common.TextBoxPersian()
        Me.txtFT_Min = New Bargh_Common.TextBoxPersian()
        Me.txtMPFeederHighPowerCount = New Bargh_Common.TextBoxPersian()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.txtMP_TMin_MPFDC = New Bargh_Common.TextBoxPersian()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.txtMP_NTMin_MPFDC = New Bargh_Common.TextBoxPersian()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.txtFT_Min_MPPDC = New Bargh_Common.TextBoxPersian()
        Me.lblSendSMSForWaitConfirmTime = New System.Windows.Forms.Label()
        Me.lblSendAlarmForTamirPercent = New System.Windows.Forms.Label()
        Me.Label48 = New System.Windows.Forms.Label()
        Me.Label47 = New System.Windows.Forms.Label()
        Me.Label46 = New System.Windows.Forms.Label()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.pnlSinglOrAll = New System.Windows.Forms.Panel()
        Me.chkIsNew_MPF = New System.Windows.Forms.CheckBox()
        Me.HatchPanel10 = New Bargh_Common.HatchPanel()
        Me.HatchPanel9 = New Bargh_Common.HatchPanel()
        Me.HatchPanel8 = New Bargh_Common.HatchPanel()
        Me.HatchPanel7 = New Bargh_Common.HatchPanel()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.txtLP_NTAllMin = New Bargh_Common.TextBoxPersian()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.Label33 = New System.Windows.Forms.Label()
        Me.txtLP_NTSingleMin = New Bargh_Common.TextBoxPersian()
        Me.Label34 = New System.Windows.Forms.Label()
        Me.Label35 = New System.Windows.Forms.Label()
        Me.txtMP_NTSingleMin = New Bargh_Common.TextBoxPersian()
        Me.Label36 = New System.Windows.Forms.Label()
        Me.txtMP_NTAllMin = New Bargh_Common.TextBoxPersian()
        Me.Label37 = New System.Windows.Forms.Label()
        Me.Label38 = New System.Windows.Forms.Label()
        Me.Label39 = New System.Windows.Forms.Label()
        Me.txtLP_TSingleMin = New Bargh_Common.TextBoxPersian()
        Me.Label40 = New System.Windows.Forms.Label()
        Me.txtLP_TAllMin = New Bargh_Common.TextBoxPersian()
        Me.Label41 = New System.Windows.Forms.Label()
        Me.Label42 = New System.Windows.Forms.Label()
        Me.txtMP_TAllMin = New Bargh_Common.TextBoxPersian()
        Me.Label43 = New System.Windows.Forms.Label()
        Me.txtMP_TSingleMin = New Bargh_Common.TextBoxPersian()
        Me.Label44 = New System.Windows.Forms.Label()
        Me.Label45 = New System.Windows.Forms.Label()
        Me.chkIsMP_NTAll = New System.Windows.Forms.CheckBox()
        Me.chkIsLP_TAll = New System.Windows.Forms.CheckBox()
        Me.chkIsMP_TAll = New System.Windows.Forms.CheckBox()
        Me.chkIsLP_NTAll = New System.Windows.Forms.CheckBox()
        Me.chkIsMP_NTSingle = New System.Windows.Forms.CheckBox()
        Me.chkIsLP_TSingle = New System.Windows.Forms.CheckBox()
        Me.chkIsMP_TSingle = New System.Windows.Forms.CheckBox()
        Me.chkIsLP_NTSingle = New System.Windows.Forms.CheckBox()
        Me.picLockSendSMSForWaitConfirm = New System.Windows.Forms.PictureBox()
        Me.picLockSendAlarmForTamir = New System.Windows.Forms.PictureBox()
        Me.picLockSerghat = New System.Windows.Forms.PictureBox()
        Me.picLockDGSRequest = New System.Windows.Forms.PictureBox()
        Me.btnSelectDisconnectGroup = New System.Windows.Forms.Button()
        Me.chkIsDGSRequest = New System.Windows.Forms.CheckBox()
        Me.txtMPF_CountOfnTime = New Bargh_Common.TextBoxPersian()
        Me.picLockMPF_nTime = New System.Windows.Forms.PictureBox()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.chkIsMPF_nTime = New System.Windows.Forms.CheckBox()
        Me.picLockFT = New System.Windows.Forms.PictureBox()
        Me.picLockAfterEdit = New System.Windows.Forms.PictureBox()
        Me.HatchPanel3 = New Bargh_Common.HatchPanel()
        Me.chkIsMP_NT = New System.Windows.Forms.CheckBox()
        Me.chkIsLP_T = New System.Windows.Forms.CheckBox()
        Me.chkISMP_T = New System.Windows.Forms.CheckBox()
        Me.chkIsFT = New System.Windows.Forms.CheckBox()
        Me.chkISLP_NT_CN = New System.Windows.Forms.CheckBox()
        Me.chkIsMP_NT_CN = New System.Windows.Forms.CheckBox()
        Me.chkIsLP_T_CN = New System.Windows.Forms.CheckBox()
        Me.chkISMP_T_CN = New System.Windows.Forms.CheckBox()
        Me.chkIsFT_CN = New System.Windows.Forms.CheckBox()
        Me.BracketPanel1 = New Bargh_Common.BracketPanel()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.chkIsTransFault = New System.Windows.Forms.CheckBox()
        Me.chkIsMP_T_MPFDC = New System.Windows.Forms.CheckBox()
        Me.chkIsMP_NT_MPFDC = New System.Windows.Forms.CheckBox()
        Me.HatchPanel2 = New Bargh_Common.HatchPanel()
        Me.HatchPanel1 = New Bargh_Common.HatchPanel()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.BracketPanel2 = New Bargh_Common.BracketPanel()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.chkIsTamir_NotConfirmLP = New System.Windows.Forms.CheckBox()
        Me.chkIsTamir_NotConfirm = New System.Windows.Forms.CheckBox()
        Me.chkIsTamir_ConfirmLP = New System.Windows.Forms.CheckBox()
        Me.chkIsTamirReturned = New System.Windows.Forms.CheckBox()
        Me.chkIsTamir_NotConfirmFT = New System.Windows.Forms.CheckBox()
        Me.chkIsTamir_ConfirmFT = New System.Windows.Forms.CheckBox()
        Me.chkIsTamir_Confirm = New System.Windows.Forms.CheckBox()
        Me.HatchPanel4 = New Bargh_Common.HatchPanel()
        Me.chkIsAfterEdit = New System.Windows.Forms.CheckBox()
        Me.chkIsFT_MPPDC = New System.Windows.Forms.CheckBox()
        Me.HatchPanel5 = New Bargh_Common.HatchPanel()
        Me.HatchPanel6 = New Bargh_Common.HatchPanel()
        Me.chkIsSendSMSForLongTamirDC = New System.Windows.Forms.CheckBox()
        Me.chkIsSendSMSForWaitConfirm = New System.Windows.Forms.CheckBox()
        Me.chkIsSendAlarmForTamir = New System.Windows.Forms.CheckBox()
        Me.chkIsSerghat = New System.Windows.Forms.CheckBox()
        Me.chkIsLP_NT = New System.Windows.Forms.CheckBox()
        Me.chkIsNew = New System.Windows.Forms.CheckBox()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.txtNew_Min = New Bargh_Common.TextBoxPersian()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.picLockRequestNewMP = New System.Windows.Forms.PictureBox()
        Me.Panel1.SuspendLayout()
        CType(Me.PickLockSendSMSForLongTamirDC, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picLockMPFeederHighPower, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picLockCriticalFeeders, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picLockIsMP_TSingle, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picLockIsLP_TSingle, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picLockIsMP_NTSingle, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picLockIsLP_NTSingle, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picLockIsMP_TAll, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picLockIsLP_TAll, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picLockIsMP_NTAll, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picLockIsLP_NTAll, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlSinglOrAll.SuspendLayout()
        CType(Me.picLockSendSMSForWaitConfirm, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picLockSendAlarmForTamir, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picLockSerghat, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picLockDGSRequest, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picLockMPF_nTime, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picLockFT, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picLockAfterEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.BracketPanel2.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.picLockRequestNewMP, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'HelpMaker
        '
        Me.HelpMaker.HelpNamespace = "ReportsHelp.chm"
        '
        'chkIsActive
        '
        Me.chkIsActive.Location = New System.Drawing.Point(20, 14)
        Me.chkIsActive.Name = "chkIsActive"
        Me.chkIsActive.Size = New System.Drawing.Size(14, 18)
        Me.chkIsActive.TabIndex = 175
        '
        'cmbArea
        '
        Me.cmbArea.BackColor = System.Drawing.Color.White
        Me.cmbArea.DisplayMember = "Area"
        Me.cmbArea.Enabled = False
        Me.cmbArea.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.cmbArea.IsReadOnly = False
        Me.cmbArea.ItemHeight = 13
        Me.cmbArea.Location = New System.Drawing.Point(124, 13)
        Me.cmbArea.Name = "cmbArea"
        Me.cmbArea.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.cmbArea.Size = New System.Drawing.Size(152, 21)
        Me.cmbArea.TabIndex = 174
        Me.cmbArea.ValueMember = "AreaId"
        '
        'txtMobile
        '
        Me.txtMobile.CaptinText = ""
        Me.txtMobile.HasCaption = False
        Me.txtMobile.IsForceText = False
        Me.txtMobile.IsFractional = False
        Me.txtMobile.IsIP = False
        Me.txtMobile.IsNumberOnly = True
        Me.txtMobile.IsYear = False
        Me.txtMobile.Location = New System.Drawing.Point(360, 12)
        Me.txtMobile.Name = "txtMobile"
        Me.txtMobile.Size = New System.Drawing.Size(114, 22)
        Me.txtMobile.TabIndex = 2
        Me.txtMobile.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtName
        '
        Me.txtName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtName.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.txtName.Location = New System.Drawing.Point(548, 12)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(224, 22)
        Me.txtName.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label1.Location = New System.Drawing.Point(778, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(82, 18)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "نام و نام خانوادگي"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label2.Location = New System.Drawing.Point(476, 13)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(60, 18)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "شماره موبايل"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label4.Location = New System.Drawing.Point(284, 13)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(61, 18)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "ناحيه مربوطه"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label5.Location = New System.Drawing.Point(36, 13)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(78, 18)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "فعال بودن ارسال"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.btnSave.Image = CType(resources.GetObject("btnSave.Image"), System.Drawing.Image)
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSave.Location = New System.Drawing.Point(808, 652)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(80, 23)
        Me.btnSave.TabIndex = 178
        Me.btnSave.Text = "&ذخيره"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnReturn
        '
        Me.btnReturn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnReturn.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnReturn.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnReturn.Image = CType(resources.GetObject("btnReturn.Image"), System.Drawing.Image)
        Me.btnReturn.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnReturn.Location = New System.Drawing.Point(8, 652)
        Me.btnReturn.Name = "btnReturn"
        Me.btnReturn.Size = New System.Drawing.Size(80, 24)
        Me.btnReturn.TabIndex = 177
        Me.btnReturn.Text = "&بازگشت"
        Me.btnReturn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.Label49)
        Me.Panel1.Controls.Add(Me.Label50)
        Me.Panel1.Controls.Add(Me.txtNew_MPF_Min)
        Me.Panel1.Controls.Add(Me.txtSendSMSForLongTamirTime)
        Me.Panel1.Controls.Add(Me.PickLockSendSMSForLongTamirDC)
        Me.Panel1.Controls.Add(Me.HatchPanel19)
        Me.Panel1.Controls.Add(Me.HatchPanel18)
        Me.Panel1.Controls.Add(Me.HatchPanel17)
        Me.Panel1.Controls.Add(Me.picLockMPFeederHighPower)
        Me.Panel1.Controls.Add(Me.picLockCriticalFeeders)
        Me.Panel1.Controls.Add(Me.HatchPanel16)
        Me.Panel1.Controls.Add(Me.chkIsMPFeederHighPower)
        Me.Panel1.Controls.Add(Me.chkIsCriticalFeeders)
        Me.Panel1.Controls.Add(Me.picLockIsMP_TSingle)
        Me.Panel1.Controls.Add(Me.picLockIsLP_TSingle)
        Me.Panel1.Controls.Add(Me.picLockIsMP_NTSingle)
        Me.Panel1.Controls.Add(Me.picLockIsLP_NTSingle)
        Me.Panel1.Controls.Add(Me.picLockIsMP_TAll)
        Me.Panel1.Controls.Add(Me.picLockIsLP_TAll)
        Me.Panel1.Controls.Add(Me.picLockIsMP_NTAll)
        Me.Panel1.Controls.Add(Me.picLockIsLP_NTAll)
        Me.Panel1.Controls.Add(Me.HatchPanel15)
        Me.Panel1.Controls.Add(Me.HatchPanel14)
        Me.Panel1.Controls.Add(Me.HatchPanel13)
        Me.Panel1.Controls.Add(Me.HatchPanel12)
        Me.Panel1.Controls.Add(Me.HatchPanel11)
        Me.Panel1.Controls.Add(Me.txtLP_NTMin)
        Me.Panel1.Controls.Add(Me.txtMP_NTMin)
        Me.Panel1.Controls.Add(Me.txtLP_TMin)
        Me.Panel1.Controls.Add(Me.txtMP_TMin)
        Me.Panel1.Controls.Add(Me.txtFT_Min)
        Me.Panel1.Controls.Add(Me.txtMPFeederHighPowerCount)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.Label10)
        Me.Panel1.Controls.Add(Me.Label11)
        Me.Panel1.Controls.Add(Me.Label13)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.Label12)
        Me.Panel1.Controls.Add(Me.Label14)
        Me.Panel1.Controls.Add(Me.Label17)
        Me.Panel1.Controls.Add(Me.txtMP_TMin_MPFDC)
        Me.Panel1.Controls.Add(Me.Label18)
        Me.Panel1.Controls.Add(Me.Label19)
        Me.Panel1.Controls.Add(Me.txtMP_NTMin_MPFDC)
        Me.Panel1.Controls.Add(Me.Label20)
        Me.Panel1.Controls.Add(Me.Label21)
        Me.Panel1.Controls.Add(Me.txtFT_Min_MPPDC)
        Me.Panel1.Controls.Add(Me.lblSendSMSForWaitConfirmTime)
        Me.Panel1.Controls.Add(Me.lblSendAlarmForTamirPercent)
        Me.Panel1.Controls.Add(Me.Label48)
        Me.Panel1.Controls.Add(Me.Label47)
        Me.Panel1.Controls.Add(Me.Label46)
        Me.Panel1.Controls.Add(Me.Label27)
        Me.Panel1.Controls.Add(Me.Label22)
        Me.Panel1.Controls.Add(Me.pnlSinglOrAll)
        Me.Panel1.Controls.Add(Me.picLockSendSMSForWaitConfirm)
        Me.Panel1.Controls.Add(Me.picLockSendAlarmForTamir)
        Me.Panel1.Controls.Add(Me.picLockSerghat)
        Me.Panel1.Controls.Add(Me.picLockDGSRequest)
        Me.Panel1.Controls.Add(Me.btnSelectDisconnectGroup)
        Me.Panel1.Controls.Add(Me.chkIsDGSRequest)
        Me.Panel1.Controls.Add(Me.txtMPF_CountOfnTime)
        Me.Panel1.Controls.Add(Me.picLockMPF_nTime)
        Me.Panel1.Controls.Add(Me.Label24)
        Me.Panel1.Controls.Add(Me.Label23)
        Me.Panel1.Controls.Add(Me.chkIsMPF_nTime)
        Me.Panel1.Controls.Add(Me.picLockFT)
        Me.Panel1.Controls.Add(Me.picLockAfterEdit)
        Me.Panel1.Controls.Add(Me.HatchPanel3)
        Me.Panel1.Controls.Add(Me.chkIsMP_NT)
        Me.Panel1.Controls.Add(Me.chkIsLP_T)
        Me.Panel1.Controls.Add(Me.chkISMP_T)
        Me.Panel1.Controls.Add(Me.chkIsFT)
        Me.Panel1.Controls.Add(Me.chkISLP_NT_CN)
        Me.Panel1.Controls.Add(Me.chkIsMP_NT_CN)
        Me.Panel1.Controls.Add(Me.chkIsLP_T_CN)
        Me.Panel1.Controls.Add(Me.chkISMP_T_CN)
        Me.Panel1.Controls.Add(Me.chkIsFT_CN)
        Me.Panel1.Controls.Add(Me.BracketPanel1)
        Me.Panel1.Controls.Add(Me.Label15)
        Me.Panel1.Controls.Add(Me.chkIsTransFault)
        Me.Panel1.Controls.Add(Me.chkIsMP_T_MPFDC)
        Me.Panel1.Controls.Add(Me.chkIsMP_NT_MPFDC)
        Me.Panel1.Controls.Add(Me.HatchPanel2)
        Me.Panel1.Controls.Add(Me.HatchPanel1)
        Me.Panel1.Controls.Add(Me.Label16)
        Me.Panel1.Controls.Add(Me.BracketPanel2)
        Me.Panel1.Controls.Add(Me.HatchPanel4)
        Me.Panel1.Controls.Add(Me.chkIsAfterEdit)
        Me.Panel1.Controls.Add(Me.chkIsFT_MPPDC)
        Me.Panel1.Controls.Add(Me.HatchPanel5)
        Me.Panel1.Controls.Add(Me.HatchPanel6)
        Me.Panel1.Controls.Add(Me.chkIsSendSMSForLongTamirDC)
        Me.Panel1.Controls.Add(Me.chkIsSendSMSForWaitConfirm)
        Me.Panel1.Controls.Add(Me.chkIsSendAlarmForTamir)
        Me.Panel1.Controls.Add(Me.chkIsSerghat)
        Me.Panel1.Controls.Add(Me.chkIsLP_NT)
        Me.Panel1.Controls.Add(Me.chkIsNew)
        Me.Panel1.Controls.Add(Me.Label28)
        Me.Panel1.Controls.Add(Me.Label29)
        Me.Panel1.Controls.Add(Me.txtNew_Min)
        Me.Panel1.Location = New System.Drawing.Point(8, 64)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(880, 584)
        Me.Panel1.TabIndex = 179
        '
        'Label49
        '
        Me.Label49.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label49.AutoSize = True
        Me.Label49.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label49.Location = New System.Drawing.Point(6, 12)
        Me.Label49.Name = "Label49"
        Me.Label49.Size = New System.Drawing.Size(30, 18)
        Me.Label49.TabIndex = 76
        Me.Label49.Text = "دقيقه"
        '
        'Label50
        '
        Me.Label50.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label50.AutoSize = True
        Me.Label50.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label50.Location = New System.Drawing.Point(75, 12)
        Me.Label50.Name = "Label50"
        Me.Label50.Size = New System.Drawing.Size(38, 18)
        Me.Label50.TabIndex = 77
        Me.Label50.Text = "بالاتر از"
        '
        'txtNew_MPF_Min
        '
        Me.txtNew_MPF_Min.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNew_MPF_Min.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtNew_MPF_Min.CaptinText = ""
        Me.txtNew_MPF_Min.Enabled = False
        Me.txtNew_MPF_Min.HasCaption = False
        Me.txtNew_MPF_Min.IsForceText = False
        Me.txtNew_MPF_Min.IsFractional = False
        Me.txtNew_MPF_Min.IsIP = False
        Me.txtNew_MPF_Min.IsNumberOnly = True
        Me.txtNew_MPF_Min.IsYear = False
        Me.txtNew_MPF_Min.Location = New System.Drawing.Point(36, 11)
        Me.txtNew_MPF_Min.Name = "txtNew_MPF_Min"
        Me.txtNew_MPF_Min.Size = New System.Drawing.Size(38, 22)
        Me.txtNew_MPF_Min.TabIndex = 78
        Me.txtNew_MPF_Min.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtSendSMSForLongTamirTime
        '
        Me.txtSendSMSForLongTamirTime.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtSendSMSForLongTamirTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSendSMSForLongTamirTime.CaptinText = ""
        Me.txtSendSMSForLongTamirTime.Enabled = False
        Me.txtSendSMSForLongTamirTime.HasCaption = False
        Me.txtSendSMSForLongTamirTime.IsForceText = False
        Me.txtSendSMSForLongTamirTime.IsFractional = False
        Me.txtSendSMSForLongTamirTime.IsIP = False
        Me.txtSendSMSForLongTamirTime.IsNumberOnly = True
        Me.txtSendSMSForLongTamirTime.IsYear = False
        Me.txtSendSMSForLongTamirTime.Location = New System.Drawing.Point(618, 554)
        Me.txtSendSMSForLongTamirTime.Name = "txtSendSMSForLongTamirTime"
        Me.txtSendSMSForLongTamirTime.Size = New System.Drawing.Size(38, 22)
        Me.txtSendSMSForLongTamirTime.TabIndex = 75
        Me.txtSendSMSForLongTamirTime.Text = "0"
        Me.txtSendSMSForLongTamirTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'PickLockSendSMSForLongTamirDC
        '
        Me.PickLockSendSMSForLongTamirDC.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PickLockSendSMSForLongTamirDC.Image = CType(resources.GetObject("PickLockSendSMSForLongTamirDC.Image"), System.Drawing.Image)
        Me.PickLockSendSMSForLongTamirDC.Location = New System.Drawing.Point(851, 556)
        Me.PickLockSendSMSForLongTamirDC.Name = "PickLockSendSMSForLongTamirDC"
        Me.PickLockSendSMSForLongTamirDC.Size = New System.Drawing.Size(20, 20)
        Me.PickLockSendSMSForLongTamirDC.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PickLockSendSMSForLongTamirDC.TabIndex = 74
        Me.PickLockSendSMSForLongTamirDC.TabStop = False
        Me.PickLockSendSMSForLongTamirDC.Visible = False
        '
        'HatchPanel19
        '
        Me.HatchPanel19.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HatchPanel19.Direction = Bargh_Common.HatchPanel.LeftRight.Right
        Me.HatchPanel19.HatchColor = System.Drawing.Color.DarkBlue
        Me.HatchPanel19.HatchCount = 3.0!
        Me.HatchPanel19.HatchSpaceWidth = 20.0!
        Me.HatchPanel19.HatchStyle = System.Drawing.Drawing2D.DashStyle.Dash
        Me.HatchPanel19.HatchWidth = 1.0!
        Me.HatchPanel19.Location = New System.Drawing.Point(10, 547)
        Me.HatchPanel19.Name = "HatchPanel19"
        Me.HatchPanel19.Size = New System.Drawing.Size(857, 3)
        Me.HatchPanel19.TabIndex = 73
        '
        'HatchPanel18
        '
        Me.HatchPanel18.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HatchPanel18.Direction = Bargh_Common.HatchPanel.LeftRight.Right
        Me.HatchPanel18.HatchColor = System.Drawing.Color.DarkBlue
        Me.HatchPanel18.HatchCount = 3.0!
        Me.HatchPanel18.HatchSpaceWidth = 20.0!
        Me.HatchPanel18.HatchStyle = System.Drawing.Drawing2D.DashStyle.Dash
        Me.HatchPanel18.HatchWidth = 1.0!
        Me.HatchPanel18.Location = New System.Drawing.Point(11, 512)
        Me.HatchPanel18.Name = "HatchPanel18"
        Me.HatchPanel18.Size = New System.Drawing.Size(857, 3)
        Me.HatchPanel18.TabIndex = 72
        '
        'HatchPanel17
        '
        Me.HatchPanel17.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HatchPanel17.Direction = Bargh_Common.HatchPanel.LeftRight.Right
        Me.HatchPanel17.HatchColor = System.Drawing.Color.DarkBlue
        Me.HatchPanel17.HatchCount = 3.0!
        Me.HatchPanel17.HatchSpaceWidth = 20.0!
        Me.HatchPanel17.HatchStyle = System.Drawing.Drawing2D.DashStyle.Dash
        Me.HatchPanel17.HatchWidth = 1.0!
        Me.HatchPanel17.Location = New System.Drawing.Point(11, 479)
        Me.HatchPanel17.Name = "HatchPanel17"
        Me.HatchPanel17.Size = New System.Drawing.Size(857, 3)
        Me.HatchPanel17.TabIndex = 72
        '
        'picLockMPFeederHighPower
        '
        Me.picLockMPFeederHighPower.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picLockMPFeederHighPower.Image = CType(resources.GetObject("picLockMPFeederHighPower.Image"), System.Drawing.Image)
        Me.picLockMPFeederHighPower.Location = New System.Drawing.Point(387, 397)
        Me.picLockMPFeederHighPower.Name = "picLockMPFeederHighPower"
        Me.picLockMPFeederHighPower.Size = New System.Drawing.Size(20, 20)
        Me.picLockMPFeederHighPower.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picLockMPFeederHighPower.TabIndex = 48
        Me.picLockMPFeederHighPower.TabStop = False
        Me.picLockMPFeederHighPower.Visible = False
        '
        'picLockCriticalFeeders
        '
        Me.picLockCriticalFeeders.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picLockCriticalFeeders.Image = CType(resources.GetObject("picLockCriticalFeeders.Image"), System.Drawing.Image)
        Me.picLockCriticalFeeders.Location = New System.Drawing.Point(580, 397)
        Me.picLockCriticalFeeders.Name = "picLockCriticalFeeders"
        Me.picLockCriticalFeeders.Size = New System.Drawing.Size(20, 20)
        Me.picLockCriticalFeeders.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picLockCriticalFeeders.TabIndex = 48
        Me.picLockCriticalFeeders.TabStop = False
        Me.picLockCriticalFeeders.Visible = False
        '
        'HatchPanel16
        '
        Me.HatchPanel16.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HatchPanel16.Direction = Bargh_Common.HatchPanel.LeftRight.Right
        Me.HatchPanel16.HatchColor = System.Drawing.Color.DarkBlue
        Me.HatchPanel16.HatchCount = 1.0!
        Me.HatchPanel16.HatchSpaceWidth = 20.0!
        Me.HatchPanel16.HatchStyle = System.Drawing.Drawing2D.DashStyle.Dash
        Me.HatchPanel16.HatchWidth = 1.0!
        Me.HatchPanel16.Location = New System.Drawing.Point(608, 407)
        Me.HatchPanel16.Name = "HatchPanel16"
        Me.HatchPanel16.Size = New System.Drawing.Size(24, 3)
        Me.HatchPanel16.TabIndex = 70
        '
        'chkIsMPFeederHighPower
        '
        Me.chkIsMPFeederHighPower.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkIsMPFeederHighPower.AutoSize = True
        Me.chkIsMPFeederHighPower.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkIsMPFeederHighPower.Location = New System.Drawing.Point(301, 399)
        Me.chkIsMPFeederHighPower.Name = "chkIsMPFeederHighPower"
        Me.chkIsMPFeederHighPower.Size = New System.Drawing.Size(102, 22)
        Me.chkIsMPFeederHighPower.TabIndex = 70
        Me.chkIsMPFeederHighPower.Text = "ارسال SMS تعداد "
        '
        'chkIsCriticalFeeders
        '
        Me.chkIsCriticalFeeders.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkIsCriticalFeeders.AutoSize = True
        Me.chkIsCriticalFeeders.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkIsCriticalFeeders.Location = New System.Drawing.Point(428, 399)
        Me.chkIsCriticalFeeders.Name = "chkIsCriticalFeeders"
        Me.chkIsCriticalFeeders.Size = New System.Drawing.Size(167, 22)
        Me.chkIsCriticalFeeders.TabIndex = 69
        Me.chkIsCriticalFeeders.Text = "ارسال ماهيانه آمار فيدرهاي بحراني"
        '
        'picLockIsMP_TSingle
        '
        Me.picLockIsMP_TSingle.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picLockIsMP_TSingle.Image = CType(resources.GetObject("picLockIsMP_TSingle.Image"), System.Drawing.Image)
        Me.picLockIsMP_TSingle.Location = New System.Drawing.Point(271, 149)
        Me.picLockIsMP_TSingle.Name = "picLockIsMP_TSingle"
        Me.picLockIsMP_TSingle.Size = New System.Drawing.Size(20, 20)
        Me.picLockIsMP_TSingle.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picLockIsMP_TSingle.TabIndex = 67
        Me.picLockIsMP_TSingle.TabStop = False
        Me.picLockIsMP_TSingle.Visible = False
        '
        'picLockIsLP_TSingle
        '
        Me.picLockIsLP_TSingle.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picLockIsLP_TSingle.Image = CType(resources.GetObject("picLockIsLP_TSingle.Image"), System.Drawing.Image)
        Me.picLockIsLP_TSingle.Location = New System.Drawing.Point(271, 120)
        Me.picLockIsLP_TSingle.Name = "picLockIsLP_TSingle"
        Me.picLockIsLP_TSingle.Size = New System.Drawing.Size(20, 20)
        Me.picLockIsLP_TSingle.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picLockIsLP_TSingle.TabIndex = 66
        Me.picLockIsLP_TSingle.TabStop = False
        Me.picLockIsLP_TSingle.Visible = False
        '
        'picLockIsMP_NTSingle
        '
        Me.picLockIsMP_NTSingle.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picLockIsMP_NTSingle.Image = CType(resources.GetObject("picLockIsMP_NTSingle.Image"), System.Drawing.Image)
        Me.picLockIsMP_NTSingle.Location = New System.Drawing.Point(271, 65)
        Me.picLockIsMP_NTSingle.Name = "picLockIsMP_NTSingle"
        Me.picLockIsMP_NTSingle.Size = New System.Drawing.Size(20, 20)
        Me.picLockIsMP_NTSingle.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picLockIsMP_NTSingle.TabIndex = 65
        Me.picLockIsMP_NTSingle.TabStop = False
        Me.picLockIsMP_NTSingle.Visible = False
        '
        'picLockIsLP_NTSingle
        '
        Me.picLockIsLP_NTSingle.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picLockIsLP_NTSingle.Image = CType(resources.GetObject("picLockIsLP_NTSingle.Image"), System.Drawing.Image)
        Me.picLockIsLP_NTSingle.Location = New System.Drawing.Point(271, 37)
        Me.picLockIsLP_NTSingle.Name = "picLockIsLP_NTSingle"
        Me.picLockIsLP_NTSingle.Size = New System.Drawing.Size(20, 20)
        Me.picLockIsLP_NTSingle.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picLockIsLP_NTSingle.TabIndex = 64
        Me.picLockIsLP_NTSingle.TabStop = False
        Me.picLockIsLP_NTSingle.Visible = False
        '
        'picLockIsMP_TAll
        '
        Me.picLockIsMP_TAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picLockIsMP_TAll.Image = CType(resources.GetObject("picLockIsMP_TAll.Image"), System.Drawing.Image)
        Me.picLockIsMP_TAll.Location = New System.Drawing.Point(495, 149)
        Me.picLockIsMP_TAll.Name = "picLockIsMP_TAll"
        Me.picLockIsMP_TAll.Size = New System.Drawing.Size(20, 20)
        Me.picLockIsMP_TAll.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picLockIsMP_TAll.TabIndex = 63
        Me.picLockIsMP_TAll.TabStop = False
        Me.picLockIsMP_TAll.Visible = False
        '
        'picLockIsLP_TAll
        '
        Me.picLockIsLP_TAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picLockIsLP_TAll.Image = CType(resources.GetObject("picLockIsLP_TAll.Image"), System.Drawing.Image)
        Me.picLockIsLP_TAll.Location = New System.Drawing.Point(495, 120)
        Me.picLockIsLP_TAll.Name = "picLockIsLP_TAll"
        Me.picLockIsLP_TAll.Size = New System.Drawing.Size(20, 20)
        Me.picLockIsLP_TAll.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picLockIsLP_TAll.TabIndex = 62
        Me.picLockIsLP_TAll.TabStop = False
        Me.picLockIsLP_TAll.Visible = False
        '
        'picLockIsMP_NTAll
        '
        Me.picLockIsMP_NTAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picLockIsMP_NTAll.Image = CType(resources.GetObject("picLockIsMP_NTAll.Image"), System.Drawing.Image)
        Me.picLockIsMP_NTAll.Location = New System.Drawing.Point(495, 65)
        Me.picLockIsMP_NTAll.Name = "picLockIsMP_NTAll"
        Me.picLockIsMP_NTAll.Size = New System.Drawing.Size(20, 20)
        Me.picLockIsMP_NTAll.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picLockIsMP_NTAll.TabIndex = 61
        Me.picLockIsMP_NTAll.TabStop = False
        Me.picLockIsMP_NTAll.Visible = False
        '
        'picLockIsLP_NTAll
        '
        Me.picLockIsLP_NTAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picLockIsLP_NTAll.Image = CType(resources.GetObject("picLockIsLP_NTAll.Image"), System.Drawing.Image)
        Me.picLockIsLP_NTAll.Location = New System.Drawing.Point(495, 37)
        Me.picLockIsLP_NTAll.Name = "picLockIsLP_NTAll"
        Me.picLockIsLP_NTAll.Size = New System.Drawing.Size(20, 20)
        Me.picLockIsLP_NTAll.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picLockIsLP_NTAll.TabIndex = 60
        Me.picLockIsLP_NTAll.TabStop = False
        Me.picLockIsLP_NTAll.Visible = False
        '
        'HatchPanel15
        '
        Me.HatchPanel15.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HatchPanel15.Direction = Bargh_Common.HatchPanel.LeftRight.Right
        Me.HatchPanel15.HatchColor = System.Drawing.Color.DarkBlue
        Me.HatchPanel15.HatchCount = 1.0!
        Me.HatchPanel15.HatchSpaceWidth = 20.0!
        Me.HatchPanel15.HatchStyle = System.Drawing.Drawing2D.DashStyle.Dash
        Me.HatchPanel15.HatchWidth = 1.0!
        Me.HatchPanel15.Location = New System.Drawing.Point(101, 216)
        Me.HatchPanel15.Name = "HatchPanel15"
        Me.HatchPanel15.Size = New System.Drawing.Size(440, 3)
        Me.HatchPanel15.TabIndex = 59
        '
        'HatchPanel14
        '
        Me.HatchPanel14.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HatchPanel14.Direction = Bargh_Common.HatchPanel.LeftRight.Right
        Me.HatchPanel14.HatchColor = System.Drawing.Color.DarkBlue
        Me.HatchPanel14.HatchCount = 1.0!
        Me.HatchPanel14.HatchSpaceWidth = 20.0!
        Me.HatchPanel14.HatchStyle = System.Drawing.Drawing2D.DashStyle.Dash
        Me.HatchPanel14.HatchWidth = 1.0!
        Me.HatchPanel14.Location = New System.Drawing.Point(517, 78)
        Me.HatchPanel14.Name = "HatchPanel14"
        Me.HatchPanel14.Size = New System.Drawing.Size(24, 3)
        Me.HatchPanel14.TabIndex = 58
        '
        'HatchPanel13
        '
        Me.HatchPanel13.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HatchPanel13.Direction = Bargh_Common.HatchPanel.LeftRight.Right
        Me.HatchPanel13.HatchColor = System.Drawing.Color.DarkBlue
        Me.HatchPanel13.HatchCount = 1.0!
        Me.HatchPanel13.HatchSpaceWidth = 20.0!
        Me.HatchPanel13.HatchStyle = System.Drawing.Drawing2D.DashStyle.Dash
        Me.HatchPanel13.HatchWidth = 1.0!
        Me.HatchPanel13.Location = New System.Drawing.Point(517, 132)
        Me.HatchPanel13.Name = "HatchPanel13"
        Me.HatchPanel13.Size = New System.Drawing.Size(24, 3)
        Me.HatchPanel13.TabIndex = 57
        '
        'HatchPanel12
        '
        Me.HatchPanel12.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HatchPanel12.Direction = Bargh_Common.HatchPanel.LeftRight.Right
        Me.HatchPanel12.HatchColor = System.Drawing.Color.DarkBlue
        Me.HatchPanel12.HatchCount = 1.0!
        Me.HatchPanel12.HatchSpaceWidth = 20.0!
        Me.HatchPanel12.HatchStyle = System.Drawing.Drawing2D.DashStyle.Dash
        Me.HatchPanel12.HatchWidth = 1.0!
        Me.HatchPanel12.Location = New System.Drawing.Point(517, 162)
        Me.HatchPanel12.Name = "HatchPanel12"
        Me.HatchPanel12.Size = New System.Drawing.Size(24, 3)
        Me.HatchPanel12.TabIndex = 56
        '
        'HatchPanel11
        '
        Me.HatchPanel11.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HatchPanel11.Direction = Bargh_Common.HatchPanel.LeftRight.Right
        Me.HatchPanel11.HatchColor = System.Drawing.Color.DarkBlue
        Me.HatchPanel11.HatchCount = 1.0!
        Me.HatchPanel11.HatchSpaceWidth = 20.0!
        Me.HatchPanel11.HatchStyle = System.Drawing.Drawing2D.DashStyle.Dash
        Me.HatchPanel11.HatchWidth = 1.0!
        Me.HatchPanel11.Location = New System.Drawing.Point(517, 49)
        Me.HatchPanel11.Name = "HatchPanel11"
        Me.HatchPanel11.Size = New System.Drawing.Size(24, 3)
        Me.HatchPanel11.TabIndex = 55
        '
        'txtLP_NTMin
        '
        Me.txtLP_NTMin.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLP_NTMin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtLP_NTMin.CaptinText = ""
        Me.txtLP_NTMin.Enabled = False
        Me.txtLP_NTMin.HasCaption = False
        Me.txtLP_NTMin.IsForceText = False
        Me.txtLP_NTMin.IsFractional = False
        Me.txtLP_NTMin.IsIP = False
        Me.txtLP_NTMin.IsNumberOnly = True
        Me.txtLP_NTMin.IsYear = False
        Me.txtLP_NTMin.Location = New System.Drawing.Point(573, 39)
        Me.txtLP_NTMin.Name = "txtLP_NTMin"
        Me.txtLP_NTMin.Size = New System.Drawing.Size(38, 22)
        Me.txtLP_NTMin.TabIndex = 2
        Me.txtLP_NTMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtMP_NTMin
        '
        Me.txtMP_NTMin.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMP_NTMin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMP_NTMin.CaptinText = ""
        Me.txtMP_NTMin.Enabled = False
        Me.txtMP_NTMin.HasCaption = False
        Me.txtMP_NTMin.IsForceText = False
        Me.txtMP_NTMin.IsFractional = False
        Me.txtMP_NTMin.IsIP = False
        Me.txtMP_NTMin.IsNumberOnly = True
        Me.txtMP_NTMin.IsYear = False
        Me.txtMP_NTMin.Location = New System.Drawing.Point(573, 68)
        Me.txtMP_NTMin.Name = "txtMP_NTMin"
        Me.txtMP_NTMin.Size = New System.Drawing.Size(38, 22)
        Me.txtMP_NTMin.TabIndex = 2
        Me.txtMP_NTMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtLP_TMin
        '
        Me.txtLP_TMin.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLP_TMin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtLP_TMin.CaptinText = ""
        Me.txtLP_TMin.Enabled = False
        Me.txtLP_TMin.HasCaption = False
        Me.txtLP_TMin.IsForceText = False
        Me.txtLP_TMin.IsFractional = False
        Me.txtLP_TMin.IsIP = False
        Me.txtLP_TMin.IsNumberOnly = True
        Me.txtLP_TMin.IsYear = False
        Me.txtLP_TMin.Location = New System.Drawing.Point(573, 122)
        Me.txtLP_TMin.Name = "txtLP_TMin"
        Me.txtLP_TMin.Size = New System.Drawing.Size(38, 22)
        Me.txtLP_TMin.TabIndex = 2
        Me.txtLP_TMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtMP_TMin
        '
        Me.txtMP_TMin.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMP_TMin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMP_TMin.CaptinText = ""
        Me.txtMP_TMin.Enabled = False
        Me.txtMP_TMin.HasCaption = False
        Me.txtMP_TMin.IsForceText = False
        Me.txtMP_TMin.IsFractional = False
        Me.txtMP_TMin.IsIP = False
        Me.txtMP_TMin.IsNumberOnly = True
        Me.txtMP_TMin.IsYear = False
        Me.txtMP_TMin.Location = New System.Drawing.Point(573, 152)
        Me.txtMP_TMin.Name = "txtMP_TMin"
        Me.txtMP_TMin.Size = New System.Drawing.Size(38, 22)
        Me.txtMP_TMin.TabIndex = 2
        Me.txtMP_TMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtFT_Min
        '
        Me.txtFT_Min.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFT_Min.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtFT_Min.CaptinText = ""
        Me.txtFT_Min.Enabled = False
        Me.txtFT_Min.HasCaption = False
        Me.txtFT_Min.IsForceText = False
        Me.txtFT_Min.IsFractional = False
        Me.txtFT_Min.IsIP = False
        Me.txtFT_Min.IsNumberOnly = True
        Me.txtFT_Min.IsYear = False
        Me.txtFT_Min.Location = New System.Drawing.Point(573, 206)
        Me.txtFT_Min.Name = "txtFT_Min"
        Me.txtFT_Min.Size = New System.Drawing.Size(38, 22)
        Me.txtFT_Min.TabIndex = 2
        Me.txtFT_Min.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtMPFeederHighPowerCount
        '
        Me.txtMPFeederHighPowerCount.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMPFeederHighPowerCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMPFeederHighPowerCount.CaptinText = ""
        Me.txtMPFeederHighPowerCount.Enabled = False
        Me.txtMPFeederHighPowerCount.HasCaption = False
        Me.txtMPFeederHighPowerCount.IsForceText = False
        Me.txtMPFeederHighPowerCount.IsFractional = False
        Me.txtMPFeederHighPowerCount.IsIP = False
        Me.txtMPFeederHighPowerCount.IsNumberOnly = True
        Me.txtMPFeederHighPowerCount.IsYear = False
        Me.txtMPFeederHighPowerCount.Location = New System.Drawing.Point(263, 399)
        Me.txtMPFeederHighPowerCount.MaxLength = 2
        Me.txtMPFeederHighPowerCount.Name = "txtMPFeederHighPowerCount"
        Me.txtMPFeederHighPowerCount.Size = New System.Drawing.Size(38, 22)
        Me.txtMPFeederHighPowerCount.TabIndex = 71
        Me.txtMPFeederHighPowerCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label6
        '
        Me.Label6.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label6.Location = New System.Drawing.Point(543, 40)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(30, 18)
        Me.Label6.TabIndex = 1
        Me.Label6.Text = "دقيقه"
        '
        'Label7
        '
        Me.Label7.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label7.Location = New System.Drawing.Point(543, 69)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(30, 18)
        Me.Label7.TabIndex = 1
        Me.Label7.Text = "دقيقه"
        '
        'Label10
        '
        Me.Label10.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label10.Location = New System.Drawing.Point(543, 123)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(30, 18)
        Me.Label10.TabIndex = 1
        Me.Label10.Text = "دقيقه"
        '
        'Label11
        '
        Me.Label11.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label11.Location = New System.Drawing.Point(543, 153)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(30, 18)
        Me.Label11.TabIndex = 1
        Me.Label11.Text = "دقيقه"
        '
        'Label13
        '
        Me.Label13.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label13.Location = New System.Drawing.Point(543, 207)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(30, 18)
        Me.Label13.TabIndex = 1
        Me.Label13.Text = "دقيقه"
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label3.Location = New System.Drawing.Point(612, 40)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(38, 18)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "بالاتر از"
        '
        'Label8
        '
        Me.Label8.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label8.Location = New System.Drawing.Point(612, 69)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(38, 18)
        Me.Label8.TabIndex = 1
        Me.Label8.Text = "بالاتر از"
        '
        'Label9
        '
        Me.Label9.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label9.Location = New System.Drawing.Point(612, 123)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(38, 18)
        Me.Label9.TabIndex = 1
        Me.Label9.Text = "بالاتر از"
        '
        'Label12
        '
        Me.Label12.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label12.Location = New System.Drawing.Point(612, 153)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(38, 18)
        Me.Label12.TabIndex = 1
        Me.Label12.Text = "بالاتر از"
        '
        'Label14
        '
        Me.Label14.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label14.Location = New System.Drawing.Point(612, 207)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(38, 18)
        Me.Label14.TabIndex = 1
        Me.Label14.Text = "بالاتر از"
        '
        'Label17
        '
        Me.Label17.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label17.Location = New System.Drawing.Point(543, 177)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(30, 18)
        Me.Label17.TabIndex = 1
        Me.Label17.Text = "دقيقه"
        '
        'txtMP_TMin_MPFDC
        '
        Me.txtMP_TMin_MPFDC.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMP_TMin_MPFDC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMP_TMin_MPFDC.CaptinText = ""
        Me.txtMP_TMin_MPFDC.Enabled = False
        Me.txtMP_TMin_MPFDC.HasCaption = False
        Me.txtMP_TMin_MPFDC.IsForceText = False
        Me.txtMP_TMin_MPFDC.IsFractional = False
        Me.txtMP_TMin_MPFDC.IsIP = False
        Me.txtMP_TMin_MPFDC.IsNumberOnly = True
        Me.txtMP_TMin_MPFDC.IsYear = False
        Me.txtMP_TMin_MPFDC.Location = New System.Drawing.Point(573, 176)
        Me.txtMP_TMin_MPFDC.Name = "txtMP_TMin_MPFDC"
        Me.txtMP_TMin_MPFDC.Size = New System.Drawing.Size(38, 22)
        Me.txtMP_TMin_MPFDC.TabIndex = 2
        Me.txtMP_TMin_MPFDC.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label18
        '
        Me.Label18.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label18.Location = New System.Drawing.Point(612, 177)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(38, 18)
        Me.Label18.TabIndex = 1
        Me.Label18.Text = "بالاتر از"
        '
        'Label19
        '
        Me.Label19.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label19.Location = New System.Drawing.Point(543, 93)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(30, 18)
        Me.Label19.TabIndex = 1
        Me.Label19.Text = "دقيقه"
        '
        'txtMP_NTMin_MPFDC
        '
        Me.txtMP_NTMin_MPFDC.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMP_NTMin_MPFDC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMP_NTMin_MPFDC.CaptinText = ""
        Me.txtMP_NTMin_MPFDC.Enabled = False
        Me.txtMP_NTMin_MPFDC.HasCaption = False
        Me.txtMP_NTMin_MPFDC.IsForceText = False
        Me.txtMP_NTMin_MPFDC.IsFractional = False
        Me.txtMP_NTMin_MPFDC.IsIP = False
        Me.txtMP_NTMin_MPFDC.IsNumberOnly = True
        Me.txtMP_NTMin_MPFDC.IsYear = False
        Me.txtMP_NTMin_MPFDC.Location = New System.Drawing.Point(573, 92)
        Me.txtMP_NTMin_MPFDC.Name = "txtMP_NTMin_MPFDC"
        Me.txtMP_NTMin_MPFDC.Size = New System.Drawing.Size(38, 22)
        Me.txtMP_NTMin_MPFDC.TabIndex = 2
        Me.txtMP_NTMin_MPFDC.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label20
        '
        Me.Label20.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label20.Location = New System.Drawing.Point(612, 93)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(38, 18)
        Me.Label20.TabIndex = 1
        Me.Label20.Text = "بالاتر از"
        '
        'Label21
        '
        Me.Label21.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label21.AutoSize = True
        Me.Label21.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label21.Location = New System.Drawing.Point(543, 231)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(30, 18)
        Me.Label21.TabIndex = 1
        Me.Label21.Text = "دقيقه"
        '
        'txtFT_Min_MPPDC
        '
        Me.txtFT_Min_MPPDC.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFT_Min_MPPDC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtFT_Min_MPPDC.CaptinText = ""
        Me.txtFT_Min_MPPDC.Enabled = False
        Me.txtFT_Min_MPPDC.HasCaption = False
        Me.txtFT_Min_MPPDC.IsForceText = False
        Me.txtFT_Min_MPPDC.IsFractional = False
        Me.txtFT_Min_MPPDC.IsIP = False
        Me.txtFT_Min_MPPDC.IsNumberOnly = True
        Me.txtFT_Min_MPPDC.IsYear = False
        Me.txtFT_Min_MPPDC.Location = New System.Drawing.Point(573, 230)
        Me.txtFT_Min_MPPDC.Name = "txtFT_Min_MPPDC"
        Me.txtFT_Min_MPPDC.Size = New System.Drawing.Size(38, 22)
        Me.txtFT_Min_MPPDC.TabIndex = 2
        Me.txtFT_Min_MPPDC.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblSendSMSForWaitConfirmTime
        '
        Me.lblSendSMSForWaitConfirmTime.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSendSMSForWaitConfirmTime.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblSendSMSForWaitConfirmTime.ForeColor = System.Drawing.Color.Red
        Me.lblSendSMSForWaitConfirmTime.Location = New System.Drawing.Point(452, 521)
        Me.lblSendSMSForWaitConfirmTime.Name = "lblSendSMSForWaitConfirmTime"
        Me.lblSendSMSForWaitConfirmTime.Size = New System.Drawing.Size(28, 22)
        Me.lblSendSMSForWaitConfirmTime.TabIndex = 1
        Me.lblSendSMSForWaitConfirmTime.Text = "24"
        Me.lblSendSMSForWaitConfirmTime.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblSendAlarmForTamirPercent
        '
        Me.lblSendAlarmForTamirPercent.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSendAlarmForTamirPercent.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblSendAlarmForTamirPercent.ForeColor = System.Drawing.Color.Red
        Me.lblSendAlarmForTamirPercent.Location = New System.Drawing.Point(451, 488)
        Me.lblSendAlarmForTamirPercent.Name = "lblSendAlarmForTamirPercent"
        Me.lblSendAlarmForTamirPercent.Size = New System.Drawing.Size(28, 22)
        Me.lblSendAlarmForTamirPercent.TabIndex = 1
        Me.lblSendAlarmForTamirPercent.Text = "75"
        Me.lblSendAlarmForTamirPercent.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label48
        '
        Me.Label48.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label48.AutoSize = True
        Me.Label48.Font = New System.Drawing.Font("Mitra", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label48.ForeColor = System.Drawing.Color.Blue
        Me.Label48.Location = New System.Drawing.Point(88, 558)
        Me.Label48.Name = "Label48"
        Me.Label48.Size = New System.Drawing.Size(221, 16)
        Me.Label48.TabIndex = 1
        Me.Label48.Text = "(اگر بر روي صفر تنظيم شده باشد، بلافاصله ارسال می گردد)"
        '
        'Label47
        '
        Me.Label47.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label47.AutoSize = True
        Me.Label47.Font = New System.Drawing.Font("Mitra", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label47.ForeColor = System.Drawing.Color.Blue
        Me.Label47.Location = New System.Drawing.Point(133, 523)
        Me.Label47.Name = "Label47"
        Me.Label47.Size = New System.Drawing.Size(214, 16)
        Me.Label47.TabIndex = 1
        Me.Label47.Text = "(ساعت در تنظيمات کلي اطلاع رساني قابل تغيير مي باشد)"
        '
        'Label46
        '
        Me.Label46.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label46.AutoSize = True
        Me.Label46.Font = New System.Drawing.Font("Mitra", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label46.ForeColor = System.Drawing.Color.Blue
        Me.Label46.Location = New System.Drawing.Point(107, 490)
        Me.Label46.Name = "Label46"
        Me.Label46.Size = New System.Drawing.Size(210, 16)
        Me.Label46.TabIndex = 1
        Me.Label46.Text = "(درصد در تنظيمات کلي اطلاع رساني قابل تغيير مي باشد)"
        '
        'Label27
        '
        Me.Label27.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label27.AutoSize = True
        Me.Label27.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label27.Location = New System.Drawing.Point(324, 490)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(121, 18)
        Me.Label27.TabIndex = 1
        Me.Label27.Text = "درصد از مدت زمان خاموشي"
        '
        'Label22
        '
        Me.Label22.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label22.AutoSize = True
        Me.Label22.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label22.Location = New System.Drawing.Point(612, 231)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(38, 18)
        Me.Label22.TabIndex = 1
        Me.Label22.Text = "بالاتر از"
        '
        'pnlSinglOrAll
        '
        Me.pnlSinglOrAll.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlSinglOrAll.Controls.Add(Me.picLockRequestNewMP)
        Me.pnlSinglOrAll.Controls.Add(Me.chkIsNew_MPF)
        Me.pnlSinglOrAll.Controls.Add(Me.HatchPanel10)
        Me.pnlSinglOrAll.Controls.Add(Me.HatchPanel9)
        Me.pnlSinglOrAll.Controls.Add(Me.HatchPanel8)
        Me.pnlSinglOrAll.Controls.Add(Me.HatchPanel7)
        Me.pnlSinglOrAll.Controls.Add(Me.Label30)
        Me.pnlSinglOrAll.Controls.Add(Me.Label31)
        Me.pnlSinglOrAll.Controls.Add(Me.txtLP_NTAllMin)
        Me.pnlSinglOrAll.Controls.Add(Me.Label32)
        Me.pnlSinglOrAll.Controls.Add(Me.Label33)
        Me.pnlSinglOrAll.Controls.Add(Me.txtLP_NTSingleMin)
        Me.pnlSinglOrAll.Controls.Add(Me.Label34)
        Me.pnlSinglOrAll.Controls.Add(Me.Label35)
        Me.pnlSinglOrAll.Controls.Add(Me.txtMP_NTSingleMin)
        Me.pnlSinglOrAll.Controls.Add(Me.Label36)
        Me.pnlSinglOrAll.Controls.Add(Me.txtMP_NTAllMin)
        Me.pnlSinglOrAll.Controls.Add(Me.Label37)
        Me.pnlSinglOrAll.Controls.Add(Me.Label38)
        Me.pnlSinglOrAll.Controls.Add(Me.Label39)
        Me.pnlSinglOrAll.Controls.Add(Me.txtLP_TSingleMin)
        Me.pnlSinglOrAll.Controls.Add(Me.Label40)
        Me.pnlSinglOrAll.Controls.Add(Me.txtLP_TAllMin)
        Me.pnlSinglOrAll.Controls.Add(Me.Label41)
        Me.pnlSinglOrAll.Controls.Add(Me.Label42)
        Me.pnlSinglOrAll.Controls.Add(Me.txtMP_TAllMin)
        Me.pnlSinglOrAll.Controls.Add(Me.Label43)
        Me.pnlSinglOrAll.Controls.Add(Me.txtMP_TSingleMin)
        Me.pnlSinglOrAll.Controls.Add(Me.Label44)
        Me.pnlSinglOrAll.Controls.Add(Me.Label45)
        Me.pnlSinglOrAll.Controls.Add(Me.chkIsMP_NTAll)
        Me.pnlSinglOrAll.Controls.Add(Me.chkIsLP_TAll)
        Me.pnlSinglOrAll.Controls.Add(Me.chkIsMP_TAll)
        Me.pnlSinglOrAll.Controls.Add(Me.chkIsLP_NTAll)
        Me.pnlSinglOrAll.Controls.Add(Me.chkIsMP_NTSingle)
        Me.pnlSinglOrAll.Controls.Add(Me.chkIsLP_TSingle)
        Me.pnlSinglOrAll.Controls.Add(Me.chkIsMP_TSingle)
        Me.pnlSinglOrAll.Controls.Add(Me.chkIsLP_NTSingle)
        Me.pnlSinglOrAll.Location = New System.Drawing.Point(104, 0)
        Me.pnlSinglOrAll.Name = "pnlSinglOrAll"
        Me.pnlSinglOrAll.Size = New System.Drawing.Size(416, 280)
        Me.pnlSinglOrAll.TabIndex = 54
        '
        'chkIsNew_MPF
        '
        Me.chkIsNew_MPF.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkIsNew_MPF.AutoSize = True
        Me.chkIsNew_MPF.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkIsNew_MPF.Location = New System.Drawing.Point(26, 11)
        Me.chkIsNew_MPF.Name = "chkIsNew_MPF"
        Me.chkIsNew_MPF.Size = New System.Drawing.Size(380, 22)
        Me.chkIsNew_MPF.TabIndex = 9
        Me.chkIsNew_MPF.Text = "خاموشي بي برنامه جديد که در هنگام ثبت اوليه فيدر فشار متوسط آن انتخاب شده است"
        '
        'HatchPanel10
        '
        Me.HatchPanel10.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.HatchPanel10.Direction = Bargh_Common.HatchPanel.LeftRight.Right
        Me.HatchPanel10.HatchColor = System.Drawing.Color.DarkBlue
        Me.HatchPanel10.HatchCount = 1.0!
        Me.HatchPanel10.HatchSpaceWidth = 20.0!
        Me.HatchPanel10.HatchStyle = System.Drawing.Drawing2D.DashStyle.Dash
        Me.HatchPanel10.HatchWidth = 1.0!
        Me.HatchPanel10.Location = New System.Drawing.Point(186, 162)
        Me.HatchPanel10.Name = "HatchPanel10"
        Me.HatchPanel10.Size = New System.Drawing.Size(24, 3)
        Me.HatchPanel10.TabIndex = 8
        '
        'HatchPanel9
        '
        Me.HatchPanel9.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.HatchPanel9.Direction = Bargh_Common.HatchPanel.LeftRight.Right
        Me.HatchPanel9.HatchColor = System.Drawing.Color.DarkBlue
        Me.HatchPanel9.HatchCount = 1.0!
        Me.HatchPanel9.HatchSpaceWidth = 20.0!
        Me.HatchPanel9.HatchStyle = System.Drawing.Drawing2D.DashStyle.Dash
        Me.HatchPanel9.HatchWidth = 1.0!
        Me.HatchPanel9.Location = New System.Drawing.Point(186, 132)
        Me.HatchPanel9.Name = "HatchPanel9"
        Me.HatchPanel9.Size = New System.Drawing.Size(24, 3)
        Me.HatchPanel9.TabIndex = 7
        '
        'HatchPanel8
        '
        Me.HatchPanel8.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.HatchPanel8.Direction = Bargh_Common.HatchPanel.LeftRight.Right
        Me.HatchPanel8.HatchColor = System.Drawing.Color.DarkBlue
        Me.HatchPanel8.HatchCount = 1.0!
        Me.HatchPanel8.HatchSpaceWidth = 20.0!
        Me.HatchPanel8.HatchStyle = System.Drawing.Drawing2D.DashStyle.Dash
        Me.HatchPanel8.HatchWidth = 1.0!
        Me.HatchPanel8.Location = New System.Drawing.Point(186, 78)
        Me.HatchPanel8.Name = "HatchPanel8"
        Me.HatchPanel8.Size = New System.Drawing.Size(24, 3)
        Me.HatchPanel8.TabIndex = 6
        '
        'HatchPanel7
        '
        Me.HatchPanel7.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.HatchPanel7.Direction = Bargh_Common.HatchPanel.LeftRight.Right
        Me.HatchPanel7.HatchColor = System.Drawing.Color.DarkBlue
        Me.HatchPanel7.HatchCount = 1.0!
        Me.HatchPanel7.HatchSpaceWidth = 20.0!
        Me.HatchPanel7.HatchStyle = System.Drawing.Drawing2D.DashStyle.Dash
        Me.HatchPanel7.HatchWidth = 1.0!
        Me.HatchPanel7.Location = New System.Drawing.Point(186, 49)
        Me.HatchPanel7.Name = "HatchPanel7"
        Me.HatchPanel7.Size = New System.Drawing.Size(24, 3)
        Me.HatchPanel7.TabIndex = 5
        '
        'Label30
        '
        Me.Label30.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label30.AutoSize = True
        Me.Label30.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label30.Location = New System.Drawing.Point(218, 41)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(30, 18)
        Me.Label30.TabIndex = 1
        Me.Label30.Text = "دقيقه"
        '
        'Label31
        '
        Me.Label31.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label31.AutoSize = True
        Me.Label31.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label31.Location = New System.Drawing.Point(294, 41)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(38, 18)
        Me.Label31.TabIndex = 1
        Me.Label31.Text = "بالاتر از"
        '
        'txtLP_NTAllMin
        '
        Me.txtLP_NTAllMin.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtLP_NTAllMin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtLP_NTAllMin.CaptinText = ""
        Me.txtLP_NTAllMin.Enabled = False
        Me.txtLP_NTAllMin.HasCaption = False
        Me.txtLP_NTAllMin.IsForceText = False
        Me.txtLP_NTAllMin.IsFractional = False
        Me.txtLP_NTAllMin.IsIP = False
        Me.txtLP_NTAllMin.IsNumberOnly = True
        Me.txtLP_NTAllMin.IsYear = False
        Me.txtLP_NTAllMin.Location = New System.Drawing.Point(254, 39)
        Me.txtLP_NTAllMin.Name = "txtLP_NTAllMin"
        Me.txtLP_NTAllMin.Size = New System.Drawing.Size(38, 22)
        Me.txtLP_NTAllMin.TabIndex = 2
        Me.txtLP_NTAllMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label32
        '
        Me.Label32.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label32.AutoSize = True
        Me.Label32.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label32.Location = New System.Drawing.Point(-1, 41)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(30, 18)
        Me.Label32.TabIndex = 1
        Me.Label32.Text = "دقيقه"
        '
        'Label33
        '
        Me.Label33.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label33.AutoSize = True
        Me.Label33.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label33.Location = New System.Drawing.Point(69, 41)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(38, 18)
        Me.Label33.TabIndex = 1
        Me.Label33.Text = "بالاتر از"
        '
        'txtLP_NTSingleMin
        '
        Me.txtLP_NTSingleMin.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtLP_NTSingleMin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtLP_NTSingleMin.CaptinText = ""
        Me.txtLP_NTSingleMin.Enabled = False
        Me.txtLP_NTSingleMin.HasCaption = False
        Me.txtLP_NTSingleMin.IsForceText = False
        Me.txtLP_NTSingleMin.IsFractional = False
        Me.txtLP_NTSingleMin.IsIP = False
        Me.txtLP_NTSingleMin.IsNumberOnly = True
        Me.txtLP_NTSingleMin.IsYear = False
        Me.txtLP_NTSingleMin.Location = New System.Drawing.Point(30, 39)
        Me.txtLP_NTSingleMin.Name = "txtLP_NTSingleMin"
        Me.txtLP_NTSingleMin.Size = New System.Drawing.Size(38, 22)
        Me.txtLP_NTSingleMin.TabIndex = 2
        Me.txtLP_NTSingleMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label34
        '
        Me.Label34.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label34.AutoSize = True
        Me.Label34.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label34.Location = New System.Drawing.Point(69, 70)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(38, 18)
        Me.Label34.TabIndex = 1
        Me.Label34.Text = "بالاتر از"
        '
        'Label35
        '
        Me.Label35.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label35.AutoSize = True
        Me.Label35.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label35.Location = New System.Drawing.Point(218, 70)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(30, 18)
        Me.Label35.TabIndex = 1
        Me.Label35.Text = "دقيقه"
        '
        'txtMP_NTSingleMin
        '
        Me.txtMP_NTSingleMin.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtMP_NTSingleMin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMP_NTSingleMin.CaptinText = ""
        Me.txtMP_NTSingleMin.Enabled = False
        Me.txtMP_NTSingleMin.HasCaption = False
        Me.txtMP_NTSingleMin.IsForceText = False
        Me.txtMP_NTSingleMin.IsFractional = False
        Me.txtMP_NTSingleMin.IsIP = False
        Me.txtMP_NTSingleMin.IsNumberOnly = True
        Me.txtMP_NTSingleMin.IsYear = False
        Me.txtMP_NTSingleMin.Location = New System.Drawing.Point(30, 68)
        Me.txtMP_NTSingleMin.Name = "txtMP_NTSingleMin"
        Me.txtMP_NTSingleMin.Size = New System.Drawing.Size(38, 22)
        Me.txtMP_NTSingleMin.TabIndex = 2
        Me.txtMP_NTSingleMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label36
        '
        Me.Label36.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label36.AutoSize = True
        Me.Label36.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label36.Location = New System.Drawing.Point(294, 70)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(38, 18)
        Me.Label36.TabIndex = 1
        Me.Label36.Text = "بالاتر از"
        '
        'txtMP_NTAllMin
        '
        Me.txtMP_NTAllMin.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtMP_NTAllMin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMP_NTAllMin.CaptinText = ""
        Me.txtMP_NTAllMin.Enabled = False
        Me.txtMP_NTAllMin.HasCaption = False
        Me.txtMP_NTAllMin.IsForceText = False
        Me.txtMP_NTAllMin.IsFractional = False
        Me.txtMP_NTAllMin.IsIP = False
        Me.txtMP_NTAllMin.IsNumberOnly = True
        Me.txtMP_NTAllMin.IsYear = False
        Me.txtMP_NTAllMin.Location = New System.Drawing.Point(254, 68)
        Me.txtMP_NTAllMin.Name = "txtMP_NTAllMin"
        Me.txtMP_NTAllMin.Size = New System.Drawing.Size(38, 22)
        Me.txtMP_NTAllMin.TabIndex = 2
        Me.txtMP_NTAllMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label37
        '
        Me.Label37.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label37.AutoSize = True
        Me.Label37.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label37.Location = New System.Drawing.Point(-1, 70)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(30, 18)
        Me.Label37.TabIndex = 1
        Me.Label37.Text = "دقيقه"
        '
        'Label38
        '
        Me.Label38.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label38.AutoSize = True
        Me.Label38.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label38.Location = New System.Drawing.Point(218, 124)
        Me.Label38.Name = "Label38"
        Me.Label38.Size = New System.Drawing.Size(30, 18)
        Me.Label38.TabIndex = 1
        Me.Label38.Text = "دقيقه"
        '
        'Label39
        '
        Me.Label39.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label39.AutoSize = True
        Me.Label39.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label39.Location = New System.Drawing.Point(69, 124)
        Me.Label39.Name = "Label39"
        Me.Label39.Size = New System.Drawing.Size(38, 18)
        Me.Label39.TabIndex = 1
        Me.Label39.Text = "بالاتر از"
        '
        'txtLP_TSingleMin
        '
        Me.txtLP_TSingleMin.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtLP_TSingleMin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtLP_TSingleMin.CaptinText = ""
        Me.txtLP_TSingleMin.Enabled = False
        Me.txtLP_TSingleMin.HasCaption = False
        Me.txtLP_TSingleMin.IsForceText = False
        Me.txtLP_TSingleMin.IsFractional = False
        Me.txtLP_TSingleMin.IsIP = False
        Me.txtLP_TSingleMin.IsNumberOnly = True
        Me.txtLP_TSingleMin.IsYear = False
        Me.txtLP_TSingleMin.Location = New System.Drawing.Point(30, 122)
        Me.txtLP_TSingleMin.Name = "txtLP_TSingleMin"
        Me.txtLP_TSingleMin.Size = New System.Drawing.Size(38, 22)
        Me.txtLP_TSingleMin.TabIndex = 2
        Me.txtLP_TSingleMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label40
        '
        Me.Label40.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label40.AutoSize = True
        Me.Label40.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label40.Location = New System.Drawing.Point(294, 124)
        Me.Label40.Name = "Label40"
        Me.Label40.Size = New System.Drawing.Size(38, 18)
        Me.Label40.TabIndex = 1
        Me.Label40.Text = "بالاتر از"
        '
        'txtLP_TAllMin
        '
        Me.txtLP_TAllMin.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtLP_TAllMin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtLP_TAllMin.CaptinText = ""
        Me.txtLP_TAllMin.Enabled = False
        Me.txtLP_TAllMin.HasCaption = False
        Me.txtLP_TAllMin.IsForceText = False
        Me.txtLP_TAllMin.IsFractional = False
        Me.txtLP_TAllMin.IsIP = False
        Me.txtLP_TAllMin.IsNumberOnly = True
        Me.txtLP_TAllMin.IsYear = False
        Me.txtLP_TAllMin.Location = New System.Drawing.Point(254, 122)
        Me.txtLP_TAllMin.Name = "txtLP_TAllMin"
        Me.txtLP_TAllMin.Size = New System.Drawing.Size(38, 22)
        Me.txtLP_TAllMin.TabIndex = 2
        Me.txtLP_TAllMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label41
        '
        Me.Label41.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label41.AutoSize = True
        Me.Label41.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label41.Location = New System.Drawing.Point(-1, 124)
        Me.Label41.Name = "Label41"
        Me.Label41.Size = New System.Drawing.Size(30, 18)
        Me.Label41.TabIndex = 1
        Me.Label41.Text = "دقيقه"
        '
        'Label42
        '
        Me.Label42.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label42.AutoSize = True
        Me.Label42.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label42.Location = New System.Drawing.Point(-1, 154)
        Me.Label42.Name = "Label42"
        Me.Label42.Size = New System.Drawing.Size(30, 18)
        Me.Label42.TabIndex = 1
        Me.Label42.Text = "دقيقه"
        '
        'txtMP_TAllMin
        '
        Me.txtMP_TAllMin.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtMP_TAllMin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMP_TAllMin.CaptinText = ""
        Me.txtMP_TAllMin.Enabled = False
        Me.txtMP_TAllMin.HasCaption = False
        Me.txtMP_TAllMin.IsForceText = False
        Me.txtMP_TAllMin.IsFractional = False
        Me.txtMP_TAllMin.IsIP = False
        Me.txtMP_TAllMin.IsNumberOnly = True
        Me.txtMP_TAllMin.IsYear = False
        Me.txtMP_TAllMin.Location = New System.Drawing.Point(254, 152)
        Me.txtMP_TAllMin.Name = "txtMP_TAllMin"
        Me.txtMP_TAllMin.Size = New System.Drawing.Size(38, 22)
        Me.txtMP_TAllMin.TabIndex = 2
        Me.txtMP_TAllMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label43
        '
        Me.Label43.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label43.AutoSize = True
        Me.Label43.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label43.Location = New System.Drawing.Point(294, 154)
        Me.Label43.Name = "Label43"
        Me.Label43.Size = New System.Drawing.Size(38, 18)
        Me.Label43.TabIndex = 1
        Me.Label43.Text = "بالاتر از"
        '
        'txtMP_TSingleMin
        '
        Me.txtMP_TSingleMin.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtMP_TSingleMin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMP_TSingleMin.CaptinText = ""
        Me.txtMP_TSingleMin.Enabled = False
        Me.txtMP_TSingleMin.HasCaption = False
        Me.txtMP_TSingleMin.IsForceText = False
        Me.txtMP_TSingleMin.IsFractional = False
        Me.txtMP_TSingleMin.IsIP = False
        Me.txtMP_TSingleMin.IsNumberOnly = True
        Me.txtMP_TSingleMin.IsYear = False
        Me.txtMP_TSingleMin.Location = New System.Drawing.Point(30, 152)
        Me.txtMP_TSingleMin.Name = "txtMP_TSingleMin"
        Me.txtMP_TSingleMin.Size = New System.Drawing.Size(38, 22)
        Me.txtMP_TSingleMin.TabIndex = 2
        Me.txtMP_TSingleMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label44
        '
        Me.Label44.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label44.AutoSize = True
        Me.Label44.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label44.Location = New System.Drawing.Point(69, 154)
        Me.Label44.Name = "Label44"
        Me.Label44.Size = New System.Drawing.Size(38, 18)
        Me.Label44.TabIndex = 1
        Me.Label44.Text = "بالاتر از"
        '
        'Label45
        '
        Me.Label45.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label45.AutoSize = True
        Me.Label45.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label45.Location = New System.Drawing.Point(218, 154)
        Me.Label45.Name = "Label45"
        Me.Label45.Size = New System.Drawing.Size(30, 18)
        Me.Label45.TabIndex = 1
        Me.Label45.Text = "دقيقه"
        '
        'chkIsMP_NTAll
        '
        Me.chkIsMP_NTAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkIsMP_NTAll.AutoSize = True
        Me.chkIsMP_NTAll.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsMP_NTAll.Enabled = False
        Me.chkIsMP_NTAll.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkIsMP_NTAll.Location = New System.Drawing.Point(338, 68)
        Me.chkIsMP_NTAll.Name = "chkIsMP_NTAll"
        Me.chkIsMP_NTAll.Size = New System.Drawing.Size(67, 22)
        Me.chkIsMP_NTAll.TabIndex = 0
        Me.chkIsMP_NTAll.Text = "چند پست"
        '
        'chkIsLP_TAll
        '
        Me.chkIsLP_TAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkIsLP_TAll.AutoSize = True
        Me.chkIsLP_TAll.Enabled = False
        Me.chkIsLP_TAll.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkIsLP_TAll.Location = New System.Drawing.Point(329, 122)
        Me.chkIsLP_TAll.Name = "chkIsLP_TAll"
        Me.chkIsLP_TAll.Size = New System.Drawing.Size(76, 22)
        Me.chkIsLP_TAll.TabIndex = 0
        Me.chkIsLP_TAll.Text = "چند مشترک"
        '
        'chkIsMP_TAll
        '
        Me.chkIsMP_TAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkIsMP_TAll.AutoSize = True
        Me.chkIsMP_TAll.Enabled = False
        Me.chkIsMP_TAll.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkIsMP_TAll.Location = New System.Drawing.Point(338, 152)
        Me.chkIsMP_TAll.Name = "chkIsMP_TAll"
        Me.chkIsMP_TAll.Size = New System.Drawing.Size(67, 22)
        Me.chkIsMP_TAll.TabIndex = 0
        Me.chkIsMP_TAll.Text = "چند پست"
        '
        'chkIsLP_NTAll
        '
        Me.chkIsLP_NTAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkIsLP_NTAll.AutoSize = True
        Me.chkIsLP_NTAll.Enabled = False
        Me.chkIsLP_NTAll.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkIsLP_NTAll.Location = New System.Drawing.Point(329, 39)
        Me.chkIsLP_NTAll.Name = "chkIsLP_NTAll"
        Me.chkIsLP_NTAll.Size = New System.Drawing.Size(76, 22)
        Me.chkIsLP_NTAll.TabIndex = 0
        Me.chkIsLP_NTAll.Text = "چند مشترک"
        '
        'chkIsMP_NTSingle
        '
        Me.chkIsMP_NTSingle.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkIsMP_NTSingle.AutoSize = True
        Me.chkIsMP_NTSingle.Enabled = False
        Me.chkIsMP_NTSingle.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkIsMP_NTSingle.Location = New System.Drawing.Point(116, 68)
        Me.chkIsMP_NTSingle.Name = "chkIsMP_NTSingle"
        Me.chkIsMP_NTSingle.Size = New System.Drawing.Size(66, 22)
        Me.chkIsMP_NTSingle.TabIndex = 0
        Me.chkIsMP_NTSingle.Text = "تک پست"
        '
        'chkIsLP_TSingle
        '
        Me.chkIsLP_TSingle.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkIsLP_TSingle.AutoSize = True
        Me.chkIsLP_TSingle.Enabled = False
        Me.chkIsLP_TSingle.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkIsLP_TSingle.Location = New System.Drawing.Point(107, 122)
        Me.chkIsLP_TSingle.Name = "chkIsLP_TSingle"
        Me.chkIsLP_TSingle.Size = New System.Drawing.Size(75, 22)
        Me.chkIsLP_TSingle.TabIndex = 0
        Me.chkIsLP_TSingle.Text = "تک مشترک"
        '
        'chkIsMP_TSingle
        '
        Me.chkIsMP_TSingle.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkIsMP_TSingle.AutoSize = True
        Me.chkIsMP_TSingle.Enabled = False
        Me.chkIsMP_TSingle.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkIsMP_TSingle.Location = New System.Drawing.Point(116, 152)
        Me.chkIsMP_TSingle.Name = "chkIsMP_TSingle"
        Me.chkIsMP_TSingle.Size = New System.Drawing.Size(66, 22)
        Me.chkIsMP_TSingle.TabIndex = 0
        Me.chkIsMP_TSingle.Text = "تک پست"
        '
        'chkIsLP_NTSingle
        '
        Me.chkIsLP_NTSingle.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkIsLP_NTSingle.AutoSize = True
        Me.chkIsLP_NTSingle.Enabled = False
        Me.chkIsLP_NTSingle.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkIsLP_NTSingle.Location = New System.Drawing.Point(107, 39)
        Me.chkIsLP_NTSingle.Name = "chkIsLP_NTSingle"
        Me.chkIsLP_NTSingle.Size = New System.Drawing.Size(75, 22)
        Me.chkIsLP_NTSingle.TabIndex = 0
        Me.chkIsLP_NTSingle.Text = "تک مشترک"
        '
        'picLockSendSMSForWaitConfirm
        '
        Me.picLockSendSMSForWaitConfirm.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picLockSendSMSForWaitConfirm.Image = CType(resources.GetObject("picLockSendSMSForWaitConfirm.Image"), System.Drawing.Image)
        Me.picLockSendSMSForWaitConfirm.Location = New System.Drawing.Point(851, 522)
        Me.picLockSendSMSForWaitConfirm.Name = "picLockSendSMSForWaitConfirm"
        Me.picLockSendSMSForWaitConfirm.Size = New System.Drawing.Size(20, 20)
        Me.picLockSendSMSForWaitConfirm.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picLockSendSMSForWaitConfirm.TabIndex = 48
        Me.picLockSendSMSForWaitConfirm.TabStop = False
        Me.picLockSendSMSForWaitConfirm.Visible = False
        '
        'picLockSendAlarmForTamir
        '
        Me.picLockSendAlarmForTamir.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picLockSendAlarmForTamir.Image = CType(resources.GetObject("picLockSendAlarmForTamir.Image"), System.Drawing.Image)
        Me.picLockSendAlarmForTamir.Location = New System.Drawing.Point(851, 487)
        Me.picLockSendAlarmForTamir.Name = "picLockSendAlarmForTamir"
        Me.picLockSendAlarmForTamir.Size = New System.Drawing.Size(20, 20)
        Me.picLockSendAlarmForTamir.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picLockSendAlarmForTamir.TabIndex = 48
        Me.picLockSendAlarmForTamir.TabStop = False
        Me.picLockSendAlarmForTamir.Visible = False
        '
        'picLockSerghat
        '
        Me.picLockSerghat.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picLockSerghat.Image = CType(resources.GetObject("picLockSerghat.Image"), System.Drawing.Image)
        Me.picLockSerghat.Location = New System.Drawing.Point(851, 453)
        Me.picLockSerghat.Name = "picLockSerghat"
        Me.picLockSerghat.Size = New System.Drawing.Size(20, 20)
        Me.picLockSerghat.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picLockSerghat.TabIndex = 48
        Me.picLockSerghat.TabStop = False
        Me.picLockSerghat.Visible = False
        '
        'picLockDGSRequest
        '
        Me.picLockDGSRequest.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picLockDGSRequest.Image = CType(resources.GetObject("picLockDGSRequest.Image"), System.Drawing.Image)
        Me.picLockDGSRequest.Location = New System.Drawing.Point(851, 421)
        Me.picLockDGSRequest.Name = "picLockDGSRequest"
        Me.picLockDGSRequest.Size = New System.Drawing.Size(20, 20)
        Me.picLockDGSRequest.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picLockDGSRequest.TabIndex = 48
        Me.picLockDGSRequest.TabStop = False
        Me.picLockDGSRequest.Visible = False
        '
        'btnSelectDisconnectGroup
        '
        Me.btnSelectDisconnectGroup.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSelectDisconnectGroup.Font = New System.Drawing.Font("Tahoma", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.btnSelectDisconnectGroup.Location = New System.Drawing.Point(616, 423)
        Me.btnSelectDisconnectGroup.Name = "btnSelectDisconnectGroup"
        Me.btnSelectDisconnectGroup.Size = New System.Drawing.Size(96, 22)
        Me.btnSelectDisconnectGroup.TabIndex = 53
        Me.btnSelectDisconnectGroup.Text = "انتخاب علل قطع"
        Me.btnSelectDisconnectGroup.Visible = False
        '
        'chkIsDGSRequest
        '
        Me.chkIsDGSRequest.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkIsDGSRequest.AutoSize = True
        Me.chkIsDGSRequest.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkIsDGSRequest.Location = New System.Drawing.Point(713, 423)
        Me.chkIsDGSRequest.Name = "chkIsDGSRequest"
        Me.chkIsDGSRequest.Size = New System.Drawing.Size(154, 22)
        Me.chkIsDGSRequest.TabIndex = 52
        Me.chkIsDGSRequest.Text = "ارسال SMS بر اساس علل قطع"
        '
        'txtMPF_CountOfnTime
        '
        Me.txtMPF_CountOfnTime.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMPF_CountOfnTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMPF_CountOfnTime.CaptinText = ""
        Me.txtMPF_CountOfnTime.Enabled = False
        Me.txtMPF_CountOfnTime.HasCaption = False
        Me.txtMPF_CountOfnTime.IsForceText = False
        Me.txtMPF_CountOfnTime.IsFractional = False
        Me.txtMPF_CountOfnTime.IsIP = False
        Me.txtMPF_CountOfnTime.IsNumberOnly = True
        Me.txtMPF_CountOfnTime.IsYear = False
        Me.txtMPF_CountOfnTime.Location = New System.Drawing.Point(682, 396)
        Me.txtMPF_CountOfnTime.Name = "txtMPF_CountOfnTime"
        Me.txtMPF_CountOfnTime.Size = New System.Drawing.Size(32, 22)
        Me.txtMPF_CountOfnTime.TabIndex = 50
        Me.txtMPF_CountOfnTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'picLockMPF_nTime
        '
        Me.picLockMPF_nTime.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picLockMPF_nTime.Image = CType(resources.GetObject("picLockMPF_nTime.Image"), System.Drawing.Image)
        Me.picLockMPF_nTime.Location = New System.Drawing.Point(851, 397)
        Me.picLockMPF_nTime.Name = "picLockMPF_nTime"
        Me.picLockMPF_nTime.Size = New System.Drawing.Size(20, 20)
        Me.picLockMPF_nTime.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picLockMPF_nTime.TabIndex = 48
        Me.picLockMPF_nTime.TabStop = False
        Me.picLockMPF_nTime.Visible = False
        '
        'Label24
        '
        Me.Label24.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label24.AutoSize = True
        Me.Label24.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label24.Location = New System.Drawing.Point(48, 401)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(210, 18)
        Me.Label24.TabIndex = 51
        Me.Label24.Text = "فيدر با بيشترين انرژي توزيع نشده در پايان هر ماه"
        '
        'Label23
        '
        Me.Label23.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label23.AutoSize = True
        Me.Label23.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label23.Location = New System.Drawing.Point(632, 399)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(44, 18)
        Me.Label23.TabIndex = 51
        Me.Label23.Text = "بار در ماه"
        '
        'chkIsMPF_nTime
        '
        Me.chkIsMPF_nTime.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkIsMPF_nTime.AutoSize = True
        Me.chkIsMPF_nTime.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkIsMPF_nTime.Location = New System.Drawing.Point(716, 399)
        Me.chkIsMPF_nTime.Name = "chkIsMPF_nTime"
        Me.chkIsMPF_nTime.Size = New System.Drawing.Size(151, 22)
        Me.chkIsMPF_nTime.TabIndex = 49
        Me.chkIsMPF_nTime.Text = "قطع فيدر فشار متوسط بيش از"
        '
        'picLockFT
        '
        Me.picLockFT.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picLockFT.Image = CType(resources.GetObject("picLockFT.Image"), System.Drawing.Image)
        Me.picLockFT.Location = New System.Drawing.Point(800, 232)
        Me.picLockFT.Name = "picLockFT"
        Me.picLockFT.Size = New System.Drawing.Size(20, 20)
        Me.picLockFT.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picLockFT.TabIndex = 48
        Me.picLockFT.TabStop = False
        Me.picLockFT.Visible = False
        '
        'picLockAfterEdit
        '
        Me.picLockAfterEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picLockAfterEdit.Image = CType(resources.GetObject("picLockAfterEdit.Image"), System.Drawing.Image)
        Me.picLockAfterEdit.Location = New System.Drawing.Point(851, 373)
        Me.picLockAfterEdit.Name = "picLockAfterEdit"
        Me.picLockAfterEdit.Size = New System.Drawing.Size(20, 20)
        Me.picLockAfterEdit.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picLockAfterEdit.TabIndex = 48
        Me.picLockAfterEdit.TabStop = False
        Me.picLockAfterEdit.Visible = False
        '
        'HatchPanel3
        '
        Me.HatchPanel3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HatchPanel3.Direction = Bargh_Common.HatchPanel.LeftRight.Right
        Me.HatchPanel3.HatchColor = System.Drawing.Color.DarkBlue
        Me.HatchPanel3.HatchCount = 3.0!
        Me.HatchPanel3.HatchSpaceWidth = 20.0!
        Me.HatchPanel3.HatchStyle = System.Drawing.Drawing2D.DashStyle.Dash
        Me.HatchPanel3.HatchWidth = 1.0!
        Me.HatchPanel3.Location = New System.Drawing.Point(11, 288)
        Me.HatchPanel3.Name = "HatchPanel3"
        Me.HatchPanel3.Size = New System.Drawing.Size(857, 3)
        Me.HatchPanel3.TabIndex = 5
        '
        'chkIsMP_NT
        '
        Me.chkIsMP_NT.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkIsMP_NT.AutoSize = True
        Me.chkIsMP_NT.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkIsMP_NT.Location = New System.Drawing.Point(711, 71)
        Me.chkIsMP_NT.Name = "chkIsMP_NT"
        Me.chkIsMP_NT.Size = New System.Drawing.Size(156, 22)
        Me.chkIsMP_NT.TabIndex = 0
        Me.chkIsMP_NT.Text = "خاموشي فشار متوسط بي برنامه"
        '
        'chkIsLP_T
        '
        Me.chkIsLP_T.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkIsLP_T.AutoSize = True
        Me.chkIsLP_T.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkIsLP_T.Location = New System.Drawing.Point(719, 125)
        Me.chkIsLP_T.Name = "chkIsLP_T"
        Me.chkIsLP_T.Size = New System.Drawing.Size(148, 22)
        Me.chkIsLP_T.TabIndex = 0
        Me.chkIsLP_T.Text = "خاموشي فشار ضعيف با برنامه"
        '
        'chkISMP_T
        '
        Me.chkISMP_T.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkISMP_T.AutoSize = True
        Me.chkISMP_T.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkISMP_T.Location = New System.Drawing.Point(716, 155)
        Me.chkISMP_T.Name = "chkISMP_T"
        Me.chkISMP_T.Size = New System.Drawing.Size(151, 22)
        Me.chkISMP_T.TabIndex = 0
        Me.chkISMP_T.Text = "خاموشي فشار متوسط با برنامه"
        '
        'chkIsFT
        '
        Me.chkIsFT.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkIsFT.AutoSize = True
        Me.chkIsFT.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkIsFT.Location = New System.Drawing.Point(761, 209)
        Me.chkIsFT.Name = "chkIsFT"
        Me.chkIsFT.Size = New System.Drawing.Size(106, 22)
        Me.chkIsFT.TabIndex = 0
        Me.chkIsFT.Text = "خاموشي فوق توزيع"
        '
        'chkISLP_NT_CN
        '
        Me.chkISLP_NT_CN.Location = New System.Drawing.Point(82, 42)
        Me.chkISLP_NT_CN.Name = "chkISLP_NT_CN"
        Me.chkISLP_NT_CN.Size = New System.Drawing.Size(16, 16)
        Me.chkISLP_NT_CN.TabIndex = 0
        '
        'chkIsMP_NT_CN
        '
        Me.chkIsMP_NT_CN.Location = New System.Drawing.Point(82, 71)
        Me.chkIsMP_NT_CN.Name = "chkIsMP_NT_CN"
        Me.chkIsMP_NT_CN.Size = New System.Drawing.Size(16, 16)
        Me.chkIsMP_NT_CN.TabIndex = 0
        '
        'chkIsLP_T_CN
        '
        Me.chkIsLP_T_CN.Location = New System.Drawing.Point(82, 125)
        Me.chkIsLP_T_CN.Name = "chkIsLP_T_CN"
        Me.chkIsLP_T_CN.Size = New System.Drawing.Size(16, 16)
        Me.chkIsLP_T_CN.TabIndex = 0
        '
        'chkISMP_T_CN
        '
        Me.chkISMP_T_CN.Location = New System.Drawing.Point(82, 155)
        Me.chkISMP_T_CN.Name = "chkISMP_T_CN"
        Me.chkISMP_T_CN.Size = New System.Drawing.Size(16, 16)
        Me.chkISMP_T_CN.TabIndex = 0
        '
        'chkIsFT_CN
        '
        Me.chkIsFT_CN.Location = New System.Drawing.Point(82, 209)
        Me.chkIsFT_CN.Name = "chkIsFT_CN"
        Me.chkIsFT_CN.Size = New System.Drawing.Size(16, 16)
        Me.chkIsFT_CN.TabIndex = 0
        '
        'BracketPanel1
        '
        Me.BracketPanel1.BracketWidth = 5.0!
        Me.BracketPanel1.Direction = Bargh_Common.BracketPanel.Directions.Left
        Me.BracketPanel1.LineColor = System.Drawing.Color.DarkBlue
        Me.BracketPanel1.LineWidth = 1.0!
        Me.BracketPanel1.Location = New System.Drawing.Point(66, 37)
        Me.BracketPanel1.Name = "BracketPanel1"
        Me.BracketPanel1.Size = New System.Drawing.Size(13, 193)
        Me.BracketPanel1.TabIndex = 3
        '
        'Label15
        '
        Me.Label15.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label15.ForeColor = System.Drawing.Color.Navy
        Me.Label15.Location = New System.Drawing.Point(5, 32)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(63, 200)
        Me.Label15.TabIndex = 1
        Me.Label15.Text = "ارسال SMS پس از رفع خاموشي"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'chkIsTransFault
        '
        Me.chkIsTransFault.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkIsTransFault.AutoSize = True
        Me.chkIsTransFault.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkIsTransFault.Location = New System.Drawing.Point(776, 260)
        Me.chkIsTransFault.Name = "chkIsTransFault"
        Me.chkIsTransFault.Size = New System.Drawing.Size(91, 22)
        Me.chkIsTransFault.TabIndex = 0
        Me.chkIsTransFault.Text = "ترانس سوزي‌ها"
        '
        'chkIsMP_T_MPFDC
        '
        Me.chkIsMP_T_MPFDC.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkIsMP_T_MPFDC.AutoSize = True
        Me.chkIsMP_T_MPFDC.Enabled = False
        Me.chkIsMP_T_MPFDC.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkIsMP_T_MPFDC.ForeColor = System.Drawing.Color.DarkGreen
        Me.chkIsMP_T_MPFDC.Location = New System.Drawing.Point(672, 179)
        Me.chkIsMP_T_MPFDC.Name = "chkIsMP_T_MPFDC"
        Me.chkIsMP_T_MPFDC.Size = New System.Drawing.Size(144, 22)
        Me.chkIsMP_T_MPFDC.TabIndex = 0
        Me.chkIsMP_T_MPFDC.Text = "منجر به قطع فيدر شده باشد"
        '
        'chkIsMP_NT_MPFDC
        '
        Me.chkIsMP_NT_MPFDC.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkIsMP_NT_MPFDC.AutoSize = True
        Me.chkIsMP_NT_MPFDC.Enabled = False
        Me.chkIsMP_NT_MPFDC.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkIsMP_NT_MPFDC.ForeColor = System.Drawing.Color.DarkGreen
        Me.chkIsMP_NT_MPFDC.Location = New System.Drawing.Point(672, 95)
        Me.chkIsMP_NT_MPFDC.Name = "chkIsMP_NT_MPFDC"
        Me.chkIsMP_NT_MPFDC.Size = New System.Drawing.Size(144, 22)
        Me.chkIsMP_NT_MPFDC.TabIndex = 0
        Me.chkIsMP_NT_MPFDC.Text = "منجر به قطع فيدر شده باشد"
        '
        'HatchPanel2
        '
        Me.HatchPanel2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HatchPanel2.Direction = Bargh_Common.HatchPanel.LeftRight.Right
        Me.HatchPanel2.HatchColor = System.Drawing.Color.DarkBlue
        Me.HatchPanel2.HatchCount = 3.0!
        Me.HatchPanel2.HatchSpaceWidth = 20.0!
        Me.HatchPanel2.HatchStyle = System.Drawing.Drawing2D.DashStyle.Dash
        Me.HatchPanel2.HatchWidth = 1.0!
        Me.HatchPanel2.Location = New System.Drawing.Point(815, 80)
        Me.HatchPanel2.Name = "HatchPanel2"
        Me.HatchPanel2.Size = New System.Drawing.Size(45, 24)
        Me.HatchPanel2.TabIndex = 4
        '
        'HatchPanel1
        '
        Me.HatchPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HatchPanel1.Direction = Bargh_Common.HatchPanel.LeftRight.Right
        Me.HatchPanel1.HatchColor = System.Drawing.Color.DarkBlue
        Me.HatchPanel1.HatchCount = 3.0!
        Me.HatchPanel1.HatchSpaceWidth = 20.0!
        Me.HatchPanel1.HatchStyle = System.Drawing.Drawing2D.DashStyle.Dash
        Me.HatchPanel1.HatchWidth = 1.0!
        Me.HatchPanel1.Location = New System.Drawing.Point(808, 163)
        Me.HatchPanel1.Name = "HatchPanel1"
        Me.HatchPanel1.Size = New System.Drawing.Size(53, 24)
        Me.HatchPanel1.TabIndex = 4
        '
        'Label16
        '
        Me.Label16.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label16.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label16.ForeColor = System.Drawing.Color.Navy
        Me.Label16.Location = New System.Drawing.Point(818, 301)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(48, 55)
        Me.Label16.TabIndex = 1
        Me.Label16.Text = "درخواست خاموشي بابرنامه"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BracketPanel2
        '
        Me.BracketPanel2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BracketPanel2.BracketWidth = 5.0!
        Me.BracketPanel2.Controls.Add(Me.Label26)
        Me.BracketPanel2.Controls.Add(Me.Label25)
        Me.BracketPanel2.Controls.Add(Me.chkIsTamir_NotConfirmLP)
        Me.BracketPanel2.Controls.Add(Me.chkIsTamir_NotConfirm)
        Me.BracketPanel2.Controls.Add(Me.chkIsTamir_ConfirmLP)
        Me.BracketPanel2.Controls.Add(Me.chkIsTamirReturned)
        Me.BracketPanel2.Controls.Add(Me.chkIsTamir_NotConfirmFT)
        Me.BracketPanel2.Controls.Add(Me.chkIsTamir_ConfirmFT)
        Me.BracketPanel2.Controls.Add(Me.chkIsTamir_Confirm)
        Me.BracketPanel2.Direction = Bargh_Common.BracketPanel.Directions.Right
        Me.BracketPanel2.LineColor = System.Drawing.Color.DarkBlue
        Me.BracketPanel2.LineWidth = 1.0!
        Me.BracketPanel2.Location = New System.Drawing.Point(33, 296)
        Me.BracketPanel2.Name = "BracketPanel2"
        Me.BracketPanel2.Size = New System.Drawing.Size(784, 71)
        Me.BracketPanel2.TabIndex = 3
        '
        'Label26
        '
        Me.Label26.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label26.AutoSize = True
        Me.Label26.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label26.Location = New System.Drawing.Point(542, 26)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(224, 18)
        Me.Label26.TabIndex = 4
        Me.Label26.Text = "ارسال SMS بعد از عدم تأييد درخواست خاموشی های"
        '
        'Label25
        '
        Me.Label25.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label25.AutoSize = True
        Me.Label25.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label25.Location = New System.Drawing.Point(536, 4)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(230, 18)
        Me.Label25.TabIndex = 4
        Me.Label25.Text = "ارسال SMS بعد از تأييد نهايي درخواست خاموشی های"
        '
        'chkIsTamir_NotConfirmLP
        '
        Me.chkIsTamir_NotConfirmLP.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkIsTamir_NotConfirmLP.AutoSize = True
        Me.chkIsTamir_NotConfirmLP.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkIsTamir_NotConfirmLP.Location = New System.Drawing.Point(239, 24)
        Me.chkIsTamir_NotConfirmLP.Name = "chkIsTamir_NotConfirmLP"
        Me.chkIsTamir_NotConfirmLP.Size = New System.Drawing.Size(75, 22)
        Me.chkIsTamir_NotConfirmLP.TabIndex = 5
        Me.chkIsTamir_NotConfirmLP.Text = "فشار ضعيف"
        '
        'chkIsTamir_NotConfirm
        '
        Me.chkIsTamir_NotConfirm.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkIsTamir_NotConfirm.AutoSize = True
        Me.chkIsTamir_NotConfirm.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkIsTamir_NotConfirm.Location = New System.Drawing.Point(345, 24)
        Me.chkIsTamir_NotConfirm.Name = "chkIsTamir_NotConfirm"
        Me.chkIsTamir_NotConfirm.Size = New System.Drawing.Size(78, 22)
        Me.chkIsTamir_NotConfirm.TabIndex = 4
        Me.chkIsTamir_NotConfirm.Text = "فشار متوسط"
        '
        'chkIsTamir_ConfirmLP
        '
        Me.chkIsTamir_ConfirmLP.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkIsTamir_ConfirmLP.AutoSize = True
        Me.chkIsTamir_ConfirmLP.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkIsTamir_ConfirmLP.Location = New System.Drawing.Point(239, 2)
        Me.chkIsTamir_ConfirmLP.Name = "chkIsTamir_ConfirmLP"
        Me.chkIsTamir_ConfirmLP.Size = New System.Drawing.Size(75, 22)
        Me.chkIsTamir_ConfirmLP.TabIndex = 2
        Me.chkIsTamir_ConfirmLP.Text = "فشار ضعيف"
        '
        'chkIsTamirReturned
        '
        Me.chkIsTamirReturned.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkIsTamirReturned.AutoSize = True
        Me.chkIsTamirReturned.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkIsTamirReturned.Location = New System.Drawing.Point(518, 47)
        Me.chkIsTamirReturned.Name = "chkIsTamirReturned"
        Me.chkIsTamirReturned.Size = New System.Drawing.Size(248, 22)
        Me.chkIsTamirReturned.TabIndex = 6
        Me.chkIsTamirReturned.Text = "ارسال SMS در صورت عودت شدن درخواست خاموشي"
        '
        'chkIsTamir_NotConfirmFT
        '
        Me.chkIsTamir_NotConfirmFT.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkIsTamir_NotConfirmFT.AutoSize = True
        Me.chkIsTamir_NotConfirmFT.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkIsTamir_NotConfirmFT.Location = New System.Drawing.Point(454, 24)
        Me.chkIsTamir_NotConfirmFT.Name = "chkIsTamir_NotConfirmFT"
        Me.chkIsTamir_NotConfirmFT.Size = New System.Drawing.Size(69, 22)
        Me.chkIsTamir_NotConfirmFT.TabIndex = 3
        Me.chkIsTamir_NotConfirmFT.Text = "فوق توزيع"
        '
        'chkIsTamir_ConfirmFT
        '
        Me.chkIsTamir_ConfirmFT.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkIsTamir_ConfirmFT.AutoSize = True
        Me.chkIsTamir_ConfirmFT.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkIsTamir_ConfirmFT.Location = New System.Drawing.Point(454, 2)
        Me.chkIsTamir_ConfirmFT.Name = "chkIsTamir_ConfirmFT"
        Me.chkIsTamir_ConfirmFT.Size = New System.Drawing.Size(69, 22)
        Me.chkIsTamir_ConfirmFT.TabIndex = 0
        Me.chkIsTamir_ConfirmFT.Text = "فوق توزيع"
        '
        'chkIsTamir_Confirm
        '
        Me.chkIsTamir_Confirm.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkIsTamir_Confirm.AutoSize = True
        Me.chkIsTamir_Confirm.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkIsTamir_Confirm.Location = New System.Drawing.Point(345, 2)
        Me.chkIsTamir_Confirm.Name = "chkIsTamir_Confirm"
        Me.chkIsTamir_Confirm.Size = New System.Drawing.Size(78, 22)
        Me.chkIsTamir_Confirm.TabIndex = 1
        Me.chkIsTamir_Confirm.Text = "فشار متوسط"
        '
        'HatchPanel4
        '
        Me.HatchPanel4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HatchPanel4.Direction = Bargh_Common.HatchPanel.LeftRight.Right
        Me.HatchPanel4.HatchColor = System.Drawing.Color.DarkBlue
        Me.HatchPanel4.HatchCount = 3.0!
        Me.HatchPanel4.HatchSpaceWidth = 20.0!
        Me.HatchPanel4.HatchStyle = System.Drawing.Drawing2D.DashStyle.Dash
        Me.HatchPanel4.HatchWidth = 1.0!
        Me.HatchPanel4.Location = New System.Drawing.Point(11, 369)
        Me.HatchPanel4.Name = "HatchPanel4"
        Me.HatchPanel4.Size = New System.Drawing.Size(857, 3)
        Me.HatchPanel4.TabIndex = 5
        '
        'chkIsAfterEdit
        '
        Me.chkIsAfterEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkIsAfterEdit.AutoSize = True
        Me.chkIsAfterEdit.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkIsAfterEdit.Location = New System.Drawing.Point(639, 375)
        Me.chkIsAfterEdit.Name = "chkIsAfterEdit"
        Me.chkIsAfterEdit.Size = New System.Drawing.Size(228, 22)
        Me.chkIsAfterEdit.TabIndex = 0
        Me.chkIsAfterEdit.Text = "ارسال SMS بعد از ويرايش پرونده‌هاي تکميل شده"
        '
        'chkIsFT_MPPDC
        '
        Me.chkIsFT_MPPDC.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkIsFT_MPPDC.AutoSize = True
        Me.chkIsFT_MPPDC.Enabled = False
        Me.chkIsFT_MPPDC.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkIsFT_MPPDC.ForeColor = System.Drawing.Color.DarkGreen
        Me.chkIsFT_MPPDC.Location = New System.Drawing.Point(652, 233)
        Me.chkIsFT_MPPDC.Name = "chkIsFT_MPPDC"
        Me.chkIsFT_MPPDC.Size = New System.Drawing.Size(164, 22)
        Me.chkIsFT_MPPDC.TabIndex = 0
        Me.chkIsFT_MPPDC.Text = "منجر به قطع کل پست شده باشد"
        '
        'HatchPanel5
        '
        Me.HatchPanel5.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HatchPanel5.Direction = Bargh_Common.HatchPanel.LeftRight.Right
        Me.HatchPanel5.HatchColor = System.Drawing.Color.DarkBlue
        Me.HatchPanel5.HatchCount = 3.0!
        Me.HatchPanel5.HatchSpaceWidth = 20.0!
        Me.HatchPanel5.HatchStyle = System.Drawing.Drawing2D.DashStyle.Dash
        Me.HatchPanel5.HatchWidth = 1.0!
        Me.HatchPanel5.Location = New System.Drawing.Point(808, 220)
        Me.HatchPanel5.Name = "HatchPanel5"
        Me.HatchPanel5.Size = New System.Drawing.Size(53, 24)
        Me.HatchPanel5.TabIndex = 4
        '
        'HatchPanel6
        '
        Me.HatchPanel6.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HatchPanel6.Direction = Bargh_Common.HatchPanel.LeftRight.Right
        Me.HatchPanel6.HatchColor = System.Drawing.Color.DarkBlue
        Me.HatchPanel6.HatchCount = 3.0!
        Me.HatchPanel6.HatchSpaceWidth = 20.0!
        Me.HatchPanel6.HatchStyle = System.Drawing.Drawing2D.DashStyle.Dash
        Me.HatchPanel6.HatchWidth = 1.0!
        Me.HatchPanel6.Location = New System.Drawing.Point(11, 445)
        Me.HatchPanel6.Name = "HatchPanel6"
        Me.HatchPanel6.Size = New System.Drawing.Size(857, 3)
        Me.HatchPanel6.TabIndex = 6
        '
        'chkIsSendSMSForLongTamirDC
        '
        Me.chkIsSendSMSForLongTamirDC.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkIsSendSMSForLongTamirDC.AutoSize = True
        Me.chkIsSendSMSForLongTamirDC.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkIsSendSMSForLongTamirDC.Location = New System.Drawing.Point(313, 555)
        Me.chkIsSendSMSForLongTamirDC.Name = "chkIsSendSMSForLongTamirDC"
        Me.chkIsSendSMSForLongTamirDC.Size = New System.Drawing.Size(554, 22)
        Me.chkIsSendSMSForLongTamirDC.TabIndex = 0
        Me.chkIsSendSMSForLongTamirDC.Text = "ارسال SMS در صورتی که مدت قطع بيش از                  دقيقه از زمان پيش بينی شده " &
    "در خاموشي هاي بابرنامه طول کشيده باشد"
        '
        'chkIsSendSMSForWaitConfirm
        '
        Me.chkIsSendSMSForWaitConfirm.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkIsSendSMSForWaitConfirm.AutoSize = True
        Me.chkIsSendSMSForWaitConfirm.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkIsSendSMSForWaitConfirm.Location = New System.Drawing.Point(350, 521)
        Me.chkIsSendSMSForWaitConfirm.Name = "chkIsSendSMSForWaitConfirm"
        Me.chkIsSendSMSForWaitConfirm.Size = New System.Drawing.Size(517, 22)
        Me.chkIsSendSMSForWaitConfirm.TabIndex = 0
        Me.chkIsSendSMSForWaitConfirm.Text = "ارسال SMS يادآوري جهت تعيين وضعيت نمودن درخواست خاموشي هاي در انتظار تأييد،      " &
    "         ساعت قبل از خاموشي"
        '
        'chkIsSendAlarmForTamir
        '
        Me.chkIsSendAlarmForTamir.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkIsSendAlarmForTamir.AutoSize = True
        Me.chkIsSendAlarmForTamir.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkIsSendAlarmForTamir.Location = New System.Drawing.Point(485, 488)
        Me.chkIsSendAlarmForTamir.Name = "chkIsSendAlarmForTamir"
        Me.chkIsSendAlarmForTamir.Size = New System.Drawing.Size(382, 22)
        Me.chkIsSendAlarmForTamir.TabIndex = 0
        Me.chkIsSendAlarmForTamir.Text = "ارسال SMS هشدار جهت يادآوري زمان وصل خاموشي هاي بابرنامه، بعد از سپري شدن "
        '
        'chkIsSerghat
        '
        Me.chkIsSerghat.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkIsSerghat.AutoSize = True
        Me.chkIsSerghat.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkIsSerghat.Location = New System.Drawing.Point(686, 455)
        Me.chkIsSerghat.Name = "chkIsSerghat"
        Me.chkIsSerghat.Size = New System.Drawing.Size(181, 22)
        Me.chkIsSerghat.TabIndex = 0
        Me.chkIsSerghat.Text = "ارسال SMS فهرست تجهيزات سرقتي"
        '
        'chkIsLP_NT
        '
        Me.chkIsLP_NT.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkIsLP_NT.AutoSize = True
        Me.chkIsLP_NT.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkIsLP_NT.Location = New System.Drawing.Point(714, 42)
        Me.chkIsLP_NT.Name = "chkIsLP_NT"
        Me.chkIsLP_NT.Size = New System.Drawing.Size(153, 22)
        Me.chkIsLP_NT.TabIndex = 0
        Me.chkIsLP_NT.Text = "خاموشي فشار ضعيف بي برنامه"
        '
        'chkIsNew
        '
        Me.chkIsNew.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkIsNew.AutoSize = True
        Me.chkIsNew.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkIsNew.Location = New System.Drawing.Point(660, 13)
        Me.chkIsNew.Name = "chkIsNew"
        Me.chkIsNew.Size = New System.Drawing.Size(207, 22)
        Me.chkIsNew.TabIndex = 0
        Me.chkIsNew.Text = "خاموشي بي برنامه جديد (نوع شبکه نامعلوم)"
        '
        'Label28
        '
        Me.Label28.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label28.AutoSize = True
        Me.Label28.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label28.Location = New System.Drawing.Point(543, 11)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(30, 18)
        Me.Label28.TabIndex = 1
        Me.Label28.Text = "دقيقه"
        '
        'Label29
        '
        Me.Label29.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label29.AutoSize = True
        Me.Label29.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label29.Location = New System.Drawing.Point(612, 11)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(38, 18)
        Me.Label29.TabIndex = 1
        Me.Label29.Text = "بالاتر از"
        '
        'txtNew_Min
        '
        Me.txtNew_Min.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNew_Min.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtNew_Min.CaptinText = ""
        Me.txtNew_Min.Enabled = False
        Me.txtNew_Min.HasCaption = False
        Me.txtNew_Min.IsForceText = False
        Me.txtNew_Min.IsFractional = False
        Me.txtNew_Min.IsIP = False
        Me.txtNew_Min.IsNumberOnly = True
        Me.txtNew_Min.IsYear = False
        Me.txtNew_Min.Location = New System.Drawing.Point(573, 10)
        Me.txtNew_Min.Name = "txtNew_Min"
        Me.txtNew_Min.Size = New System.Drawing.Size(38, 22)
        Me.txtNew_Min.TabIndex = 2
        Me.txtNew_Min.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Panel2
        '
        Me.Panel2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.Label5)
        Me.Panel2.Controls.Add(Me.txtMobile)
        Me.Panel2.Controls.Add(Me.chkIsActive)
        Me.Panel2.Controls.Add(Me.cmbArea)
        Me.Panel2.Controls.Add(Me.txtName)
        Me.Panel2.Controls.Add(Me.Label1)
        Me.Panel2.Controls.Add(Me.Label4)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Location = New System.Drawing.Point(8, 8)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(880, 48)
        Me.Panel2.TabIndex = 180
        '
        'picLockRequestNewMP
        '
        Me.picLockRequestNewMP.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picLockRequestNewMP.Image = CType(resources.GetObject("picLockRequestNewMP.Image"), System.Drawing.Image)
        Me.picLockRequestNewMP.Location = New System.Drawing.Point(390, 9)
        Me.picLockRequestNewMP.Name = "picLockRequestNewMP"
        Me.picLockRequestNewMP.Size = New System.Drawing.Size(20, 20)
        Me.picLockRequestNewMP.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picLockRequestNewMP.TabIndex = 49
        Me.picLockRequestNewMP.TabStop = False
        Me.picLockRequestNewMP.Visible = False
        '
        'frmManagerSMS_DC
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.CancelButton = Me.btnReturn
        Me.ClientSize = New System.Drawing.Size(898, 680)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnReturn)
        Me.Controls.Add(Me.Panel2)
        Me.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.HelpMaker.SetHelpNavigator(Me, System.Windows.Forms.HelpNavigator.Topic)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmManagerSMS_DC"
        Me.HelpMaker.SetShowHelp(Me, True)
        Me.Text = "خاموشي‌ها به مديران SMS ارسال"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.PickLockSendSMSForLongTamirDC, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picLockMPFeederHighPower, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picLockCriticalFeeders, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picLockIsMP_TSingle, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picLockIsLP_TSingle, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picLockIsMP_NTSingle, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picLockIsLP_NTSingle, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picLockIsMP_TAll, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picLockIsLP_TAll, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picLockIsMP_NTAll, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picLockIsLP_NTAll, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlSinglOrAll.ResumeLayout(False)
        Me.pnlSinglOrAll.PerformLayout()
        CType(Me.picLockSendSMSForWaitConfirm, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picLockSendAlarmForTamir, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picLockSerghat, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picLockDGSRequest, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picLockMPF_nTime, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picLockFT, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picLockAfterEdit, System.ComponentModel.ISupportInitialize).EndInit()
        Me.BracketPanel2.ResumeLayout(False)
        Me.BracketPanel2.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.picLockRequestNewMP, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Events"
    Private Sub frmManagerSMS_DC_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        LoadAreaDataTable(mCnn, mDs, cmbArea, True)
        cmbArea.DataSource = mDs.Tables("Tbl_Area")
        cmbArea.DisplayMember = "Area"
        cmbArea.ValueMember = "AreaId"
        cmbArea.Enabled = IsCenter

        BindingTable("select * from TblManagerSMSDC where ManagerSMSDCId = -1", mCnn, mDs, "TblManagerSMSDC")
        mTblSMS = mDs.Tables("TblManagerSMSDC")

        BindingTable("SELECT * FROM TblManagerSMSDGS WHERE ManagerSMSDCId = -1", mCnn, mDs, "TblManagerSMSDGS")

        LoadInfo()

        'If Not IsSetadMode OrElse Not IsAdmin() Then
        '    btnSave.Enabled = False
        'End If

        CheckLockInfo()
    End Sub
    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If SaveInfo() Then
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End If
    End Sub
    Private Sub chk_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIsNew.CheckedChanged, chkIsLP_NT.CheckedChanged, chkIsLP_T.CheckedChanged, chkIsMP_NT.CheckedChanged, chkISMP_T.CheckedChanged, chkIsLP_NTSingle.CheckedChanged, chkIsLP_NTAll.CheckedChanged, chkIsMP_NTSingle.CheckedChanged, chkIsMP_NTAll.CheckedChanged, chkIsLP_TSingle.CheckedChanged, chkIsLP_TAll.CheckedChanged, chkIsMP_TSingle.CheckedChanged, chkIsMP_TAll.CheckedChanged, chkIsFT.CheckedChanged, chkIsMP_NT_MPFDC.CheckedChanged, chkIsFT_MPPDC.CheckedChanged, chkIsMP_T_MPFDC.CheckedChanged, chkIsMPF_nTime.CheckedChanged, chkIsMPFeederHighPower.CheckedChanged, chkIsNew_MPF.CheckedChanged

        Dim lChk As CheckBox = CType(sender, CheckBox)
        If lChk.Name = "chkIsNew" Then
            txtNew_Min.Enabled = lChk.Checked
        End If
        Select Case lChk.Name
            Case "chkIsLP_NT"
                txtLP_NTMin.Enabled = lChk.Checked
                chkISLP_NT_CN.Checked = lChk.Checked
                chkIsLP_NTAll.Enabled = lChk.Checked
                chkIsLP_NTSingle.Enabled = lChk.Checked
                If Not lChk.Checked Then
                    chkIsLP_NTAll.Checked = False
                    chkIsLP_NTSingle.Checked = False
                End If
            Case "chkIsLP_T"
                txtLP_TMin.Enabled = lChk.Checked
                chkIsLP_T_CN.Checked = lChk.Checked
                chkIsLP_TAll.Enabled = lChk.Checked
                chkIsLP_TSingle.Enabled = lChk.Checked
                If Not lChk.Checked Then
                    chkIsLP_TAll.Checked = False
                    chkIsLP_TSingle.Checked = False
                End If
            Case "chkIsMP_NT"
                txtMP_NTMin.Enabled = lChk.Checked
                chkIsMP_NT_CN.Checked = lChk.Checked
                chkIsMP_NT_MPFDC.Enabled = lChk.Checked
                chkIsMP_NTAll.Enabled = lChk.Checked
                chkIsMP_NTSingle.Enabled = lChk.Checked
                If Not lChk.Checked Then
                    chkIsMP_NT_MPFDC.Checked = False
                    chkIsMP_NTAll.Checked = False
                    chkIsMP_NTSingle.Checked = False
                End If
            Case "chkIsMP_NT_MPFDC"
                txtMP_NTMin_MPFDC.Enabled = lChk.Checked
            Case "chkISMP_T"
                txtMP_TMin.Enabled = lChk.Checked
                chkISMP_T_CN.Checked = lChk.Checked
                chkIsMP_T_MPFDC.Enabled = lChk.Checked
                chkIsMP_TAll.Enabled = lChk.Checked
                chkIsMP_TSingle.Enabled = lChk.Checked
                If Not lChk.Checked Then
                    chkIsMP_T_MPFDC.Checked = False
                    chkIsMP_TAll.Checked = False
                    chkIsMP_TSingle.Checked = False
                End If
            Case "chkIsMP_T_MPFDC"
                txtMP_TMin_MPFDC.Enabled = lChk.Checked
            Case "chkIsFT"
                txtFT_Min.Enabled = lChk.Checked
                chkIsFT_CN.Checked = lChk.Checked
                chkIsFT_MPPDC.Enabled = lChk.Checked
                If Not lChk.Checked Then
                    chkIsFT_MPPDC.Checked = False
                End If
            Case "chkIsFT_MPPDC"
                txtFT_Min_MPPDC.Enabled = lChk.Checked
            Case "chkIsMPF_nTime"
                txtMPF_CountOfnTime.Enabled = lChk.Checked
            Case "chkIsLP_NTAll"
                txtLP_NTAllMin.Enabled = lChk.Checked
            Case "chkIsLP_NTSingle"
                txtLP_NTSingleMin.Enabled = lChk.Checked
            Case "chkIsLP_TAll"
                txtLP_TAllMin.Enabled = lChk.Checked
            Case "chkIsLP_TSingle"
                txtLP_TSingleMin.Enabled = lChk.Checked
            Case "chkIsMP_NTAll"
                txtMP_NTAllMin.Enabled = lChk.Checked
            Case "chkIsMP_NTSingle"
                txtMP_NTSingleMin.Enabled = lChk.Checked
            Case "chkIsMP_TAll"
                txtMP_TAllMin.Enabled = lChk.Checked
            Case "chkIsMP_TSingle"
                txtMP_TSingleMin.Enabled = lChk.Checked
            Case "chkIsMPFeederHighPower"
                txtMPFeederHighPowerCount.Enabled = lChk.Checked
                '-------------<omid>
            Case "chkIsNew_MPF"
                txtNew_MPF_Min.Enabled = lChk.Checked
                '-------------</omid>
        End Select
    End Sub
    Private Sub chk_CN_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkIsFT_CN.CheckedChanged, chkISLP_NT_CN.CheckedChanged, chkIsLP_T_CN.CheckedChanged, chkIsMP_NT_CN.CheckedChanged, chkISMP_T_CN.CheckedChanged
        Dim lChk As CheckBox = CType(sender, CheckBox)
        If lChk.Checked Then
            Select Case lChk.Name
                Case "chkISLP_NT_CN"
                    chkIsLP_NT.Checked = True
                Case "chkIsLP_T_CN"
                    chkIsLP_T.Checked = True
                Case "chkIsMP_NT_CN"
                    chkIsMP_NT.Checked = True
                Case "chkISMP_T_CN"
                    chkISMP_T.Checked = True
                Case "chkIsFT_CN"
                    chkIsFT.Checked = True
            End Select
        End If
    End Sub
    Private Sub chkIsDGSRequest_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIsDGSRequest.CheckedChanged
        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.ver_921211) Then Exit Sub
        Try
            If Not chkIsDGSRequest.Checked And mDisconnectReasons <> "" Then
                If MsgBoxF("هم اکنون در قسمت علل قطع، موارد انتخاب شده‌اي وجود دارند" & vbCrLf & " آيا از غير فعال نمودن اين گزينه اطمينان داريد؟ ", "هشدار", MessageBoxButtons.YesNo, MsgBoxIcon.MsgIcon_Warning, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                    Dim lArrDel As New ArrayList
                    Dim lRow As DataRow
                    mDisconnectReasons = ""
                    btnSelectDisconnectGroup.Text = "انتخاب علل قطع"
                Else
                    chkIsDGSRequest.Checked = True
                End If
            End If
            btnSelectDisconnectGroup.Visible = chkIsDGSRequest.Checked
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub
    Private Sub btnSelectDisconnectGroup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectDisconnectGroup.Click

        Dim lDlg As New Bargh_BaseTables.frmDisconnectReason(False, False)

        lDlg.DisconnectReasones = mDisconnectReasons
        If lDlg.ShowDialog() = DialogResult.OK Then
            mDisconnectReasons = lDlg.DisconnectReasones
        End If
        lDlg.Dispose()

        If mDisconnectReasons = "" Then
            btnSelectDisconnectGroup.Text = "انتخاب علل قطع"
        Else
            btnSelectDisconnectGroup.Text = "[انتخاب علل قطع]"
        End If

    End Sub
#End Region

#Region "Methods"
    Private Sub CheckLockInfo()
        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.rep_ECOPower) Then
            chkIsAfterEdit.Enabled = False
            chkIsAfterEdit.Checked = False
            chkIsFT_MPPDC.Enabled = False
            chkIsFT_MPPDC.Checked = False
            picLockAfterEdit.Visible = True
            picLockAfterEdit.BringToFront()
            picLockFT.Visible = True
            picLockFT.BringToFront()
            GlobalToolTip.SetToolTip(picLockAfterEdit, mdlHavadesInfo.cHavadesInfoPartMessage)
            GlobalToolTip.SetToolTip(picLockFT, mdlHavadesInfo.cHavadesInfoPartMessage)
        End If
        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.ver_920812) Then
            chkIsMPF_nTime.Enabled = False
            chkIsMPF_nTime.Checked = False
            txtMPF_CountOfnTime.Enabled = False
            txtMPF_CountOfnTime.Text = ""
            picLockMPF_nTime.Visible = True
            picLockMPF_nTime.BringToFront()
        End If
        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.ver_921211) Then
            chkIsDGSRequest.Enabled = False
            chkIsDGSRequest.Checked = False
            btnSelectDisconnectGroup.Enabled = False
            picLockDGSRequest.Visible = True
            picLockDGSRequest.BringToFront()
        End If
        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.ver_930414) Then
            chkIsSerghat.Enabled = False
            chkIsSerghat.Checked = False
            picLockSerghat.Visible = True
            picLockSerghat.BringToFront()
        End If
        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.ver_950431) Then
            pnlSinglOrAll.Enabled = False
            picLockIsLP_NTAll.Visible = True
            picLockIsLP_NTSingle.Visible = True
            picLockIsLP_TAll.Visible = True
            picLockIsLP_TSingle.Visible = True
            picLockIsMP_NTAll.Visible = True
            picLockIsMP_NTSingle.Visible = True
            picLockIsMP_TAll.Visible = True
            picLockIsMP_TSingle.Visible = True
        End If
        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.ver_950803) Then
            chkIsCriticalFeeders.Enabled = False
            chkIsCriticalFeeders.Checked = False
            picLockCriticalFeeders.Visible = True
            picLockCriticalFeeders.BringToFront()
        End If
        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.ver_970131) Then
            chkIsMPFeederHighPower.Enabled = False
            chkIsMPFeederHighPower.Checked = False
            txtMPFeederHighPowerCount.Enabled = False
            txtMPFeederHighPowerCount.Text = ""
            picLockMPFeederHighPower.Visible = True
            picLockMPFeederHighPower.BringToFront()
        End If
        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.ver_980730) And Not IsTozi("Ilam") Then
            chkIsSendAlarmForTamir.Checked = False
            chkIsSendAlarmForTamir.Enabled = False
            picLockSendAlarmForTamir.Visible = True
            picLockSendAlarmForTamir.BringToFront()
        End If
        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.ver_981215) And Not IsTozi("Kermanshah") Then
            chkIsSendSMSForWaitConfirm.Checked = False
            chkIsSendSMSForWaitConfirm.Enabled = False
            picLockSendSMSForWaitConfirm.Visible = True
            picLockSendSMSForWaitConfirm.BringToFront()
        End If
        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.ver_4000122) And Not IsTozi("Zanjan") Then
            chkIsSendSMSForLongTamirDC.Checked = False
            chkIsSendSMSForLongTamirDC.Enabled = False
            txtSendSMSForLongTamirTime.Enabled = False
            PickLockSendSMSForLongTamirDC.Visible = True
            PickLockSendSMSForLongTamirDC.BringToFront()
        End If
    End Sub
    Private Sub LoadInfo()
        Dim lSQL As String = ""

        If m_ManagerSMSDCId = -1 Then
            mRowEditing = mTblSMS.NewRow()
            mRowEditing("ManagerSMSDCId") = GetAutoInc()
            mRowEditing("ManagerName") = ""
            mRowEditing("ManagerMobile") = ""
            mRowEditing("AreaId") = WorkingAreaId
            mRowEditing("IsActive") = True

            mRowEditing("IsLPNotTamir") = False
            mRowEditing("LPNotTamirMinutes") = DBNull.Value
            mRowEditing("IsLPTamir") = False
            mRowEditing("LPTamirMinutes") = DBNull.Value
            mRowEditing("IsMPNotTamir") = False
            mRowEditing("MPNotTamirMinutes") = DBNull.Value
            mRowEditing("IsMPTamir") = False
            mRowEditing("MPTamirMinutes") = DBNull.Value
            mRowEditing("IsFT") = False
            mRowEditing("FTMinutes") = DBNull.Value

            mRowEditing("IsMPFDC_nTime") = False
            mRowEditing("MPFDC_CountOfnTime") = DBNull.Value

            mRowEditing("IsLPNotTamirSendSMSAfterConnect") = False
            mRowEditing("IsLPTamirSendSMSAfterConnect") = False
            mRowEditing("IsMPNotTamirSendSMSAfterConnect") = False
            mRowEditing("IsMPTamirSendSMSAfterConnect") = False
            mRowEditing("IsFTSendSMSAfterConnect") = False
            mRowEditing("IsTransFaultSendSMS") = False

            mRowEditing("IsMPNotTamir_MPFDC") = False
            mRowEditing("IsMPTamir_MPFDC") = False

            mRowEditing("IsTamir_Confirm") = False
            mRowEditing("IsTamir_NotConfirm") = False

            mRowEditing("IsTamir_ConfirmLP") = False
            mRowEditing("IsTamir_NotConfirmLP") = False

            mRowEditing("IsAfterEditRequest") = False
            mRowEditing("IsDGSRequest") = False

            mRowEditing("MPNotTamirMinutes_MPFDC") = DBNull.Value
            mRowEditing("MPTamirMinutes_MPFDC") = DBNull.Value
            mRowEditing("IsFT_MPPDC") = False
            mRowEditing("FTMinutes_MPPDC") = DBNull.Value

            mRowEditing("IsSerghat") = False
            mRowEditing("IsCriticalFeeders") = False

            mRowEditing("IsMPFeederHighPower") = False
            mRowEditing("MPFeederHighPowerCount") = DBNull.Value

            mRowEditing("IsSendAlarmForTamir") = False
            '-------------<omid>
            mRowEditing("IsNewRequestMP") = False
            mRowEditing("NewRequestMPMinutes") = DBNull.Value
            '--------------</omid>
            mTblSMS.Rows.Add(mRowEditing)
        Else
            lSQL = "SELECT * FROM TblManagerSMSDC WHERE ManagerSMSDCId = " & m_ManagerSMSDCId
            BindingTable(lSQL, mCnn, mDs, "TblManagerSMSDC")
            mRowEditing = mDs.Tables("TblManagerSMSDC").Rows(0)

            lSQL = "SELECT * FROM TblManagerSMSDGS WHERE ManagerSMSDCId = " & m_ManagerSMSDCId
            BindingTable(lSQL, mCnn, mDs, "TblManagerSMSDGS")

            For Each lRow As DataRow In mDs.Tables("TblManagerSMSDGS").Rows

                If Not IsDBNull(lRow("DisconnectGroupSetId")) Then
                    mDisconnectReasons &= If(mDisconnectReasons <> "", "," & "DGS" & lRow("DisconnectGroupSetId"), "DGS" & lRow("DisconnectGroupSetId"))
                End If

                If Not IsDBNull(lRow("DisconnectReasonId")) Then
                    mDisconnectReasons &= If(mDisconnectReasons <> "", "," & "DCR" & lRow("DisconnectReasonId"), "DCR" & lRow("DisconnectReasonId"))
                End If
            Next
        End If

        txtName.Text = mRowEditing("ManagerName")
        txtMobile.Text = mRowEditing("ManagerMobile")
        cmbArea.SelectedValue = mRowEditing("AreaId")
        chkIsActive.Checked = mRowEditing("IsActive")

        chkIsLP_NT.Checked = mRowEditing("IsLPNotTamir")
        txtLP_NTMin.Text = If(IsDBNull(mRowEditing("LPNotTamirMinutes")), "", mRowEditing("LPNotTamirMinutes"))
        chkIsLP_T.Checked = mRowEditing("IsLPTamir")
        txtLP_TMin.Text = If(IsDBNull(mRowEditing("LPTamirMinutes")), "", mRowEditing("LPTamirMinutes"))
        chkIsMP_NT.Checked = mRowEditing("IsMPNotTamir")
        txtMP_NTMin.Text = If(IsDBNull(mRowEditing("MPNotTamirMinutes")), "", mRowEditing("MPNotTamirMinutes"))
        chkISMP_T.Checked = mRowEditing("IsMPTamir")
        txtMP_TMin.Text = If(IsDBNull(mRowEditing("MPTamirMinutes")), "", mRowEditing("MPTamirMinutes"))
        chkIsFT.Checked = mRowEditing("IsFT")
        txtFT_Min.Text = If(IsDBNull(mRowEditing("FTMinutes")), "", mRowEditing("FTMinutes"))

        GetDBValue(chkIsMPF_nTime, mRowEditing("IsMPFDC_nTime"))
        txtMPF_CountOfnTime.Text = If(IsDBNull(mRowEditing("MPFDC_CountOfnTime")), "", mRowEditing("MPFDC_CountOfnTime"))

        GetDBValue(chkIsNew, mRowEditing("IsNewRequest"))
        txtNew_Min.Text = If(IsDBNull(mRowEditing("NewRequestMinutes")), "", mRowEditing("NewRequestMinutes"))
        '--------------<omid>
        GetDBValue(chkIsNew_MPF, mRowEditing("IsNewRequestMP"))
        txtNew_Min.Text = If(IsDBNull(mRowEditing("NewRequestMPMinutes")), "", mRowEditing("NewRequestMPMinutes"))
        '--------------</omid>
        GetDBValue(chkIsLP_NTAll, mRowEditing("IsLPNotTamirAll"))
        txtLP_NTAllMin.Text = If(IsDBNull(mRowEditing("LPNotTamirAllMinutes")), "", mRowEditing("LPNotTamirAllMinutes"))
        GetDBValue(chkIsLP_TAll, mRowEditing("IsLPTamirAll"))
        txtLP_TAllMin.Text = If(IsDBNull(mRowEditing("LPTamirAllMinutes")), "", mRowEditing("LPTamirAllMinutes"))
        GetDBValue(chkIsLP_TSingle, mRowEditing("IsLPTamirSingle"))
        txtLP_TSingleMin.Text = If(IsDBNull(mRowEditing("LPTamirSingleMinutes")), "", mRowEditing("LPTamirSingleMinutes"))
        GetDBValue(chkIsLP_NTSingle, mRowEditing("IsLPNotTamirSingle"))
        txtLP_NTSingleMin.Text = If(IsDBNull(mRowEditing("LPNotTamirSingleMinutes")), "", mRowEditing("LPNotTamirSingleMinutes"))
        GetDBValue(chkIsMP_NTAll, mRowEditing("IsMPNotTamirAll"))
        txtMP_NTAllMin.Text = If(IsDBNull(mRowEditing("MPNotTamirAllMinutes")), "", mRowEditing("MPNotTamirAllMinutes"))
        GetDBValue(chkIsMP_TAll, mRowEditing("IsMPTamirAll"))
        txtMP_TAllMin.Text = If(IsDBNull(mRowEditing("MPTamirAllMinutes")), "", mRowEditing("MPTamirAllMinutes"))
        GetDBValue(chkIsMP_TSingle, mRowEditing("IsMPTamirSingle"))
        txtMP_TSingleMin.Text = If(IsDBNull(mRowEditing("MPTamirSingleMinutes")), "", mRowEditing("MPTamirSingleMinutes"))
        GetDBValue(chkIsMP_NTSingle, mRowEditing("IsMPNotTamirSingle"))
        txtMP_NTSingleMin.Text = If(IsDBNull(mRowEditing("MPNotTamirSingleMinutes")), "", mRowEditing("MPNotTamirSingleMinutes"))

        GetDBValue(chkISLP_NT_CN, mRowEditing("IsLPNotTamirSendSMSAfterConnect"))
        GetDBValue(chkIsLP_T_CN, mRowEditing("IsLPTamirSendSMSAfterConnect"))
        GetDBValue(chkIsMP_NT_CN, mRowEditing("IsMPNotTamirSendSMSAfterConnect"))
        GetDBValue(chkISMP_T_CN, mRowEditing("IsMPTamirSendSMSAfterConnect"))
        GetDBValue(chkIsFT_CN, mRowEditing("IsFTSendSMSAfterConnect"))
        GetDBValue(chkIsTransFault, mRowEditing("IsTransFaultSendSMS"))

        GetDBValue(chkIsMP_T_MPFDC, mRowEditing("IsMPTamir_MPFDC"))
        GetDBValue(chkIsMP_NT_MPFDC, mRowEditing("IsMPNotTamir_MPFDC"))

        GetDBValue(chkIsTamir_Confirm, mRowEditing("IsTamir_Confirm"))
        GetDBValue(chkIsTamir_NotConfirm, mRowEditing("IsTamir_NotConfirm"))

        GetDBValue(chkIsTamir_ConfirmLP, mRowEditing("IsTamir_ConfirmLP"))
        GetDBValue(chkIsTamir_NotConfirmLP, mRowEditing("IsTamir_NotConfirmLP"))

        GetDBValue(chkIsTamir_ConfirmFT, mRowEditing("IsTamir_ConfirmFT"))
        GetDBValue(chkIsTamir_NotConfirmFT, mRowEditing("IsTamir_NotConfirmFT"))

        GetDBValue(chkIsTamirReturned, mRowEditing("IsTamirReturned"))

        GetDBValue(chkIsAfterEdit, mRowEditing("IsAfterEditRequest"))
        GetDBValue(chkIsDGSRequest, mRowEditing("IsDGSRequest"))

        GetDBValue(txtMP_NTMin_MPFDC, mRowEditing("MPNotTamirMinutes_MPFDC"))
        GetDBValue(txtMP_TMin_MPFDC, mRowEditing("MPTamirMinutes_MPFDC"))
        GetDBValue(chkIsFT_MPPDC, mRowEditing("IsFT_MPPDC"))
        GetDBValue(txtFT_Min_MPPDC, mRowEditing("FTMinutes_MPPDC"))

        GetDBValue(chkIsSerghat, mRowEditing("IsSerghat"))
        GetDBValue(chkIsCriticalFeeders, mRowEditing("IsCriticalFeeders"))

        GetDBValue(chkIsSendAlarmForTamir, mRowEditing("IsSendAlarmForTamir"))
        lblSendAlarmForTamirPercent.Text = CConfig.ReadConfig("SendAlarmForTamirPercent", "75")

        GetDBValue(chkIsSendSMSForWaitConfirm, mRowEditing("IsSendSMSForWaitConfirm"))

        If chkIsDGSRequest.Checked Then btnSelectDisconnectGroup.Text = "[انتخاب علل قطع]"

        GetDBValue(chkIsMPFeederHighPower, mRowEditing("IsMPFeederHighPower"))
        txtMPFeederHighPowerCount.Text = If(IsDBNull(mRowEditing("MPFeederHighPowerCount")), "", mRowEditing("MPFeederHighPowerCount"))

        GetDBValue(chkIsSendSMSForLongTamirDC, mRowEditing("IsSendSMSForLongTamirDC"))
        GetDBValue(txtSendSMSForLongTamirTime, mRowEditing("SendSMSForLongTamirTime"))

    End Sub
    Private Function IsSaveOK() As Boolean
        IsSaveOK = False

        If txtName.Text.Trim = "" Then
            MsgBoxF("نام و نام خانوادگي را وارد نماييد", "خطا", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Error, MessageBoxDefaultButton.Button1, True)
            txtName.Focus()
            Exit Function
        End If

        If txtMobile.Text = "" Then
            MsgBoxF("شماره موبايل را وارد نماييد", "خطا", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Error, MessageBoxDefaultButton.Button1, True)
            txtMobile.Focus()
            Exit Function
        End If

        If cmbArea.SelectedIndex = -1 Then
            MsgBoxF("ناحيه مربوطه را مشخص نماييد", "خطا", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Error, MessageBoxDefaultButton.Button1, True)
            cmbArea.Focus()
            Exit Function
        End If

        If Not (
                chkISMP_T.Checked Or chkIsMP_NT.Checked Or chkIsLP_T.Checked Or chkIsLP_NT.Checked Or
                chkIsFT.Checked Or chkIsTamir_Confirm.Checked Or chkIsTamir_NotConfirm.Checked Or chkIsTamir_ConfirmLP.Checked Or chkIsTamir_NotConfirmLP.Checked _
                Or chkIsTransFault.Checked Or chkIsAfterEdit.Checked Or chkIsMPF_nTime.Checked Or chkIsTamir_ConfirmFT.Checked Or chkIsTamir_NotConfirmFT.Checked _
                Or chkIsDGSRequest.Checked Or chkIsSerghat.Checked Or chkIsNew.Checked Or chkIsMPFeederHighPower.Checked Or chkIsTamirReturned.Checked _
                Or chkIsSendSMSForWaitConfirm.Checked Or chkIsSendAlarmForTamir.Checked Or chkIsSendSMSForLongTamirDC.Checked) Then
            MsgBoxF("نوع پيام را مشخص نماييد", "خطا", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Error, MessageBoxDefaultButton.Button1, True)
            Exit Function
        ElseIf chkIsFT.Checked AndAlso Val(txtFT_Min.Text) = 0 Then
            MsgBoxF("حداقل زمان خاموشي فوق توزيع را مشخص نماييد", "خطا", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Error, MessageBoxDefaultButton.Button1, True)
            txtFT_Min.Focus()
            Exit Function
        ElseIf chkIsLP_NT.Checked AndAlso Val(txtLP_NTMin.Text) = 0 Then
            MsgBoxF("حداقل زمان خاموشي فشار ضعيف بي برنامه را مشخص نماييد", "خطا", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Error, MessageBoxDefaultButton.Button1, True)
            txtLP_NTMin.Focus()
            Exit Function
        ElseIf chkIsLP_T.Checked AndAlso Val(txtLP_TMin.Text) = 0 Then
            MsgBoxF("حداقل زمان خاموشي فشار ضعيف با برنامه را مشخص نماييد", "خطا", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Error, MessageBoxDefaultButton.Button1, True)
            txtLP_TMin.Focus()
            Exit Function
        ElseIf chkIsMP_NT.Checked AndAlso Val(txtMP_NTMin.Text) = 0 Then
            MsgBoxF("حداقل زمان خاموشي فشار متوسط بي برنامه را مشخص نماييد", "خطا", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Error, MessageBoxDefaultButton.Button1, True)
            txtMP_NTMin.Focus()
            Exit Function
        ElseIf chkISMP_T.Checked AndAlso Val(txtMP_TMin.Text) = 0 Then
            MsgBoxF("حداقل زمان خاموشي فشار متوسط با برنامه را مشخص نماييد", "خطا", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Error, MessageBoxDefaultButton.Button1, True)
            txtMP_TMin.Focus()
            Exit Function
        ElseIf chkIsMPF_nTime.Checked AndAlso Val(txtMPF_CountOfnTime.Text) = 0 Then
            MsgBoxF("حداقل تعداد قطع فيدر فشار متوسط را مشخص نماييد", "خطا", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Error, MessageBoxDefaultButton.Button1, True)
            txtMPF_CountOfnTime.Focus()
            Exit Function
        ElseIf chkIsNew.Checked AndAlso Val(txtNew_Min.Text) = 0 Then
            MsgBoxF("حداقل زمان خاموشي بي برنامه جديد را مشخص نماييد", "خطا", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Error, MessageBoxDefaultButton.Button1, True)
            txtNew_Min.Focus()
            Exit Function
        ElseIf chkIsLP_NTAll.Checked AndAlso Val(txtLP_NTAllMin.Text) = 0 Then
            MsgBoxF("حداقل زمان خاموشي فشار ضعيف بي برنامه کلي را مشخص نماييد", "خطا", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Error, MessageBoxDefaultButton.Button1, True)
            txtLP_NTAllMin.Focus()
            Exit Function
        ElseIf chkIsLP_NTSingle.Checked AndAlso Val(txtLP_NTSingleMin.Text) = 0 Then
            MsgBoxF("حداقل زمان خاموشي فشار ضعيف بي برنامه تکي را مشخص نماييد", "خطا", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Error, MessageBoxDefaultButton.Button1, True)
            txtLP_NTSingleMin.Focus()
            Exit Function
        ElseIf chkIsMP_NTAll.Checked AndAlso Val(txtMP_NTAllMin.Text) = 0 Then
            MsgBoxF("حداقل زمان خاموشي فشار متوسط بي برنامه کلي را مشخص نماييد", "خطا", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Error, MessageBoxDefaultButton.Button1, True)
            txtMP_NTAllMin.Focus()
            Exit Function
        ElseIf chkIsMP_NTSingle.Checked AndAlso Val(txtMP_NTSingleMin.Text) = 0 Then
            MsgBoxF("حداقل زمان خاموشي فشار متوسط بي برنامه تکي را مشخص نماييد", "خطا", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Error, MessageBoxDefaultButton.Button1, True)
            txtMP_NTSingleMin.Focus()
            Exit Function
        ElseIf chkIsLP_TAll.Checked AndAlso Val(txtLP_TAllMin.Text) = 0 Then
            MsgBoxF("حداقل زمان خاموشي فشار ضعيف با برنامه کلي را مشخص نماييد", "خطا", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Error, MessageBoxDefaultButton.Button1, True)
            txtLP_TAllMin.Focus()
            Exit Function
        ElseIf chkIsLP_TSingle.Checked AndAlso Val(txtLP_TSingleMin.Text) = 0 Then
            MsgBoxF("حداقل زمان خاموشي فشار ضعيف با برنامه تکي را مشخص نماييد", "خطا", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Error, MessageBoxDefaultButton.Button1, True)
            txtLP_TSingleMin.Focus()
            Exit Function
        ElseIf chkIsMP_TAll.Checked AndAlso Val(txtMP_TAllMin.Text) = 0 Then
            MsgBoxF("حداقل زمان خاموشي فشار متوسط با برنامه کلي را مشخص نماييد", "خطا", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Error, MessageBoxDefaultButton.Button1, True)
            txtMP_TAllMin.Focus()
            Exit Function
        ElseIf chkIsMP_TSingle.Checked AndAlso Val(txtMP_TSingleMin.Text) = 0 Then
            MsgBoxF("حداقل زمان خاموشي فشار متوسط با برنامه تکي را مشخص نماييد", "خطا", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Error, MessageBoxDefaultButton.Button1, True)
            txtMP_TSingleMin.Focus()
            Exit Function
        ElseIf chkIsMPFeederHighPower.Checked AndAlso Val(txtMPFeederHighPowerCount.Text) = 0 Then
            MsgBoxF("حداکثر تعداد فيدر فشار متوسط داراي بيشترين انرژي توزيع نشده، را مشخص نماييد", "خطا", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Error, MessageBoxDefaultButton.Button1, True)
            txtMPFeederHighPowerCount.Focus()
            Exit Function
        ElseIf chkIsDGSRequest.Checked AndAlso mDisconnectReasons = "" Then
            MsgBoxF("علل قطع جهت ارسال پيامک بر اساس علل قطع را مشخص نماييد", "خطا", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Error, MessageBoxDefaultButton.Button1, True)
            btnSelectDisconnectGroup_Click(Nothing, Nothing)
            Exit Function
        ElseIf chkIsSendAlarmForTamir.Checked AndAlso Val(lblSendAlarmForTamirPercent.Text) = 0 Then
            ShowError("لطفاً در قسمت تنظيمات کلي سامانه، ميزان درصد مدت زمان سپري شده را جهت هشدار وصل خاموشي بابرنامه وارد نماييد")
            Exit Function
        ElseIf chkIsSendSMSForWaitConfirm.Checked AndAlso Val(lblSendSMSForWaitConfirmTime.Text) = 0 Then
            ShowError("لطفاً در قسمت تنظيمات کلي سامانه، مدت زمان ارسال پيامک يادآوري جهت تعيين وضعيت نمودن درخواست خاموشي هاي در انتظار تأييد را مشخص نماييد")
            Exit Function
            '-------------<omid>
        ElseIf chkIsNew_MPF.Checked AndAlso Val(txtNew_MPF_Min.Text) = 0 Then
            MsgBoxF("حداقل زمان خاموشي جدید بي برنامه فيدر فشار ضعيف را مشخص نماييد", "خطا", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Error, MessageBoxDefaultButton.Button1, True)
            txtNew_MPF_Min.Focus()
            Exit Function
            '-------------</omid>
        End If

        IsSaveOK = True
    End Function
    Private Function SaveInfo() As Boolean
        SaveInfo = False

        If Not IsSaveOK() Then Exit Function

        Dim lTrans As SqlTransaction
        Dim lUpdate As New frmUpdateDataSetBT
        Dim lIsSaveOK As Boolean = True

        Try
            mRowEditing("ManagerMobile") = txtMobile.Text
            mRowEditing("ManagerName") = ReplaceFarsiToArabi(txtName.Text)
            mRowEditing("AreaId") = cmbArea.SelectedValue
            mRowEditing("IsActive") = chkIsActive.Checked

            mRowEditing("IsLPNotTamir") = chkIsLP_NT.Checked
            mRowEditing("LPNotTamirMinutes") = If(chkIsLP_NT.Checked, txtLP_NTMin.Text, DBNull.Value)
            mRowEditing("IsLPTamir") = chkIsLP_T.Checked
            mRowEditing("LPTamirMinutes") = If(chkIsLP_T.Checked, txtLP_TMin.Text, DBNull.Value)
            mRowEditing("IsMPNotTamir") = chkIsMP_NT.Checked
            mRowEditing("MPNotTamirMinutes") = If(chkIsMP_NT.Checked, txtMP_NTMin.Text, DBNull.Value)
            mRowEditing("IsMPTamir") = chkISMP_T.Checked
            mRowEditing("MPTamirMinutes") = If(chkISMP_T.Checked, txtMP_TMin.Text, DBNull.Value)
            mRowEditing("IsFT") = chkIsFT.Checked
            mRowEditing("FTMinutes") = If(chkIsFT.Checked, txtFT_Min.Text, DBNull.Value)

            mRowEditing("IsMPFDC_nTime") = chkIsMPF_nTime.Checked
            mRowEditing("MPFDC_CountOfnTime") = If(chkIsMPF_nTime.Checked, txtMPF_CountOfnTime.Text, DBNull.Value)

            mRowEditing("IsNewRequest") = chkIsNew.Checked
            mRowEditing("NewRequestMinutes") = If(chkIsNew.Checked, txtNew_Min.Text, DBNull.Value)

            mRowEditing("IsLPNotTamirAll") = chkIsLP_NTAll.Checked
            mRowEditing("LPNotTamirAllMinutes") = If(chkIsLP_NTAll.Checked, txtLP_NTAllMin.Text, DBNull.Value)
            mRowEditing("IsLPTamirAll") = chkIsLP_TAll.Checked
            mRowEditing("LPTamirAllMinutes") = If(chkIsLP_TAll.Checked, txtLP_TAllMin.Text, DBNull.Value)
            mRowEditing("IsLPTamirSingle") = chkIsLP_TSingle.Checked
            mRowEditing("LPTamirSingleMinutes") = If(chkIsLP_TSingle.Checked, txtLP_TSingleMin.Text, DBNull.Value)
            mRowEditing("IsLPNotTamirSingle") = chkIsLP_NTSingle.Checked
            mRowEditing("LPNotTamirSingleMinutes") = If(chkIsLP_NTSingle.Checked, txtLP_NTSingleMin.Text, DBNull.Value)
            mRowEditing("IsMPNotTamirAll") = chkIsMP_NTAll.Checked
            mRowEditing("MPNotTamirAllMinutes") = If(chkIsMP_NTAll.Checked, txtMP_NTAllMin.Text, DBNull.Value)
            mRowEditing("IsMPTamirAll") = chkIsMP_TAll.Checked
            mRowEditing("MPTamirAllMinutes") = If(chkIsMP_TAll.Checked, txtMP_TAllMin.Text, DBNull.Value)
            mRowEditing("IsMPTamirSingle") = chkIsMP_TSingle.Checked
            mRowEditing("MPTamirSingleMinutes") = If(chkIsMP_TSingle.Checked, txtMP_TSingleMin.Text, DBNull.Value)
            mRowEditing("IsMPNotTamirSingle") = chkIsMP_NTSingle.Checked
            mRowEditing("MPNotTamirSingleMinutes") = If(chkIsMP_NTSingle.Checked, txtMP_NTSingleMin.Text, DBNull.Value)

            mRowEditing("IsMPFeederHighPower") = chkIsMPFeederHighPower.Checked
            mRowEditing("MPFeederHighPowerCount") = If(chkIsMPFeederHighPower.Checked, txtMPFeederHighPowerCount.Text, DBNull.Value)
            '---------------------<omid>-------------
            mRowEditing("IsNewRequestMP") = chkIsNew_MPF.Checked
            mRowEditing("NewRequestMPMinutes") = If(chkIsNew_MPF.Checked, txtNew_MPF_Min.Text, DBNull.Value)
            '--------------------</omid>-------------
            SetDBValue(chkISLP_NT_CN, mRowEditing("IsLPNotTamirSendSMSAfterConnect"))
            SetDBValue(chkIsLP_T_CN, mRowEditing("IsLPTamirSendSMSAfterConnect"))
            SetDBValue(chkIsMP_NT_CN, mRowEditing("IsMPNotTamirSendSMSAfterConnect"))
            SetDBValue(chkISMP_T_CN, mRowEditing("IsMPTamirSendSMSAfterConnect"))
            SetDBValue(chkIsFT_CN, mRowEditing("IsFTSendSMSAfterConnect"))
            SetDBValue(chkIsTransFault, mRowEditing("IsTransFaultSendSMS"))

            SetDBValue(chkIsMP_T_MPFDC, mRowEditing("IsMPTamir_MPFDC"))
            SetDBValue(chkIsMP_NT_MPFDC, mRowEditing("IsMPNotTamir_MPFDC"))

            SetDBValue(chkIsTamir_Confirm, mRowEditing("IsTamir_Confirm"))
            SetDBValue(chkIsTamir_NotConfirm, mRowEditing("IsTamir_NotConfirm"))

            SetDBValue(chkIsTamir_ConfirmLP, mRowEditing("IsTamir_ConfirmLP"))
            SetDBValue(chkIsTamir_NotConfirmLP, mRowEditing("IsTamir_NotConfirmLP"))

            SetDBValue(chkIsTamir_ConfirmFT, mRowEditing("IsTamir_ConfirmFT"))
            SetDBValue(chkIsTamir_NotConfirmFT, mRowEditing("IsTamir_NotConfirmFT"))
            SetDBValue(chkIsTamirReturned, mRowEditing("IsTamirReturned"))

            SetDBValue(chkIsAfterEdit, mRowEditing("IsAfterEditRequest"))
            SetDBValue(chkIsDGSRequest, mRowEditing("IsDGSRequest"))

            SetDBValue(chkIsSerghat, mRowEditing("IsSerghat"))
            SetDBValue(chkIsCriticalFeeders, mRowEditing("IsCriticalFeeders"))

            If chkIsMP_NT_MPFDC.Checked Then
                SetDBValue(txtMP_NTMin_MPFDC, mRowEditing("MPNotTamirMinutes_MPFDC"))
            Else
                mRowEditing("MPNotTamirMinutes_MPFDC") = DBNull.Value
            End If

            If chkIsMP_T_MPFDC.Checked Then
                SetDBValue(txtMP_TMin_MPFDC, mRowEditing("MPTamirMinutes_MPFDC"))
            Else
                mRowEditing("MPTamirMinutes_MPFDC") = DBNull.Value
            End If

            SetDBValue(chkIsFT_MPPDC, mRowEditing("IsFT_MPPDC"))
            If chkIsFT_MPPDC.Checked Then
                SetDBValue(txtFT_Min_MPPDC, mRowEditing("FTMinutes_MPPDC"))
            Else
                mRowEditing("FTMinutes_MPPDC") = DBNull.Value
            End If

            SetDBValue(chkIsSendSMSForWaitConfirm, mRowEditing("IsSendSMSForWaitConfirm"))

            SetDBValue(chkIsSendAlarmForTamir, mRowEditing("IsSendAlarmForTamir"))

            SetDBValue(chkIsSendSMSForLongTamirDC, mRowEditing("IsSendSMSForLongTamirDC"))
            SetDBValue(txtSendSMSForLongTamirTime, mRowEditing("SendSMSForLongTamirTime"))

            Dim lDisconnectGroupSetIDs As String = ""
            Dim lDisconnectReasonIDs As String = ""

            For Each lStr As String In mDisconnectReasons.Split(",")
                If lStr.IndexOf("DGS") > -1 Then
                    lDisconnectGroupSetIDs &= If(lDisconnectGroupSetIDs <> "", "," & lStr.Replace("DGS", ""), lStr.Replace("DGS", ""))
                ElseIf lStr.IndexOf("DCR") > -1 Then
                    lDisconnectReasonIDs &= If(lDisconnectReasonIDs <> "", "," & lStr.Replace("DCR", ""), lStr.Replace("DCR", ""))
                End If
            Next

            Dim lDelArray As New ArrayList
            For Each lRow As DataRow In mDs.Tables("TblManagerSMSDGS").Rows
                If lRow.RowState <> DataRowState.Deleted Then
                    If Not IsDBNull(lRow("DisconnectGroupSetId")) AndAlso Not lDisconnectGroupSetIDs.Contains("DisconnectGroupSetId") Then
                        lDelArray.Add(lRow)
                    End If
                    If Not IsDBNull(lRow("DisconnectReasonId")) AndAlso Not lDisconnectReasonIDs.Contains("DisconnectReasonId") Then
                        lDelArray.Add(lRow)
                    End If
                End If
            Next

            For Each lRow As DataRow In lDelArray
                lRow.Delete()
            Next

            Dim lNewRow As DataRow

            If lDisconnectGroupSetIDs <> "" Then
                For Each lStr As String In lDisconnectGroupSetIDs.Split(",")
                    Dim lRows() As DataRow = mDs.Tables("TblManagerSMSDGS").Select("DisconnectGroupSetId = " & lStr)
                    If lRows.Length = 0 Then
                        lNewRow = mDs.Tables("TblManagerSMSDGS").NewRow()
                        lNewRow("ManagerSMSDGSId") = GetAutoInc()
                        lNewRow("ManagerSMSDCId") = m_ManagerSMSDCId
                        lNewRow("DisconnectGroupSetId") = lStr
                        lNewRow("DisconnectReasonId") = DBNull.Value
                        mDs.Tables("TblManagerSMSDGS").Rows.Add(lNewRow)
                    End If
                Next
            End If

            If lDisconnectReasonIDs <> "" Then
                For Each lStr As String In lDisconnectReasonIDs.Split(",")
                    Dim lRows() As DataRow = mDs.Tables("TblManagerSMSDGS").Select("DisconnectReasonId = " & lStr)
                    If lRows.Length = 0 Then
                        lNewRow = mDs.Tables("TblManagerSMSDGS").NewRow()
                        lNewRow("ManagerSMSDGSId") = GetAutoInc()
                        lNewRow("ManagerSMSDCId") = m_ManagerSMSDCId
                        lNewRow("DisconnectGroupSetId") = DBNull.Value
                        lNewRow("DisconnectReasonId") = lStr
                        mDs.Tables("TblManagerSMSDGS").Rows.Add(lNewRow)
                    End If
                Next
            End If

            If mCnn.State <> ConnectionState.Open Then
                mCnn.Open()
            End If
            lTrans = mCnn.BeginTransaction()

            lIsSaveOK = lUpdate.UpdateDataSet("TblManagerSMSDC", mDs, , , lTrans)

            If lIsSaveOK Then
                Dim lRows() As DataRow
                For Each lRow As DataRow In mDs.Tables("TblManagerSMSDGS").Rows
                    If lRow.RowState <> DataRowState.Deleted Then
                        lRow("ManagerSMSDCId") = mRowEditing("ManagerSMSDCId")
                    End If
                Next
                lIsSaveOK = lUpdate.UpdateDataSet("TblManagerSMSDGS", mDs, , , lTrans)
            End If

            If lIsSaveOK Then
                lTrans.Commit()
            Else
                lTrans.Rollback()
            End If

            SaveInfo = lIsSaveOK
        Catch ex As Exception
            If Not IsNothing(lTrans) Then
                lTrans.Rollback()
            End If

            ShowError(ex)
        Finally
            If mCnn.State = ConnectionState.Open Then
                mCnn.Close()
            End If
        End Try
    End Function

    Private Sub chkIsSendSMSForLongTamirDC_CheckedChanged(sender As Object, e As EventArgs) Handles chkIsSendSMSForLongTamirDC.CheckedChanged
        txtSendSMSForLongTamirTime.Enabled = chkIsSendSMSForLongTamirDC.Checked
    End Sub

    'Private Sub chkIsNew_MPF_CheckedChanged(sender As Object, e As EventArgs) Handles chkIsNew_MPF.CheckedChanged
    '    txtNew_MPF_Min.Enabled = !
    'End Sub

#End Region

End Class
