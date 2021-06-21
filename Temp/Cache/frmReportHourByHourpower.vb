Imports System
Imports System.Data.SqlClient

Public Class frmReportHourByHourpower
    Inherits FormBase
    Private mIsReporting As Boolean = False
    Private mIsLP As Boolean = False, mIsMP As Boolean = False, mIsFT As Boolean = False
    Private mIsTamir As Boolean = False, mIsNotTamir As Boolean = False
    Private mDCGroupSetGroupId As String
    Private mTamirTypeId As String
    Private mCnn As SqlConnection = New SqlConnection(GetConnection())
    Private mAreaId As Integer
    Private mAreaIDs As String = ""
    Private mMPFeederId As Integer
    Private mMPFeederIds As String
    Private mMPFeederPartId As Integer
    Private mFiltersInfo As String = ""
    Private mExcel As CExcel = Nothing
    Private mReportType As String = ""
    Private mReportNo As String = ""
    Private mErr As Boolean = False
    Private mIsProgress As Boolean = False
    Friend WithEvents rdbMPFeederPartFilter As System.Windows.Forms.RadioButton
    Friend WithEvents rdbMPFeederFilter As System.Windows.Forms.RadioButton
    Friend WithEvents chkFilterSep As System.Windows.Forms.CheckBox
    Private mDS As DatasetCcRequester = Nothing
    Private mMPFeederName As String
    Friend WithEvents pnlFilter As System.Windows.Forms.Panel
    Friend WithEvents lblMPFeeder As System.Windows.Forms.Label
    Friend WithEvents cmbMPPost As Bargh_Common.ComboBoxPersian
    Friend WithEvents lblMPPost As System.Windows.Forms.Label
    Friend WithEvents chkCmbMPFeeder As Bargh_Common.ChkCombo
    Private mFeederPartName As String
    Friend WithEvents pnlMain As System.Windows.Forms.Panel
    Friend WithEvents pnlDetails As System.Windows.Forms.Panel
    Friend WithEvents chkArea As Bargh_Common.ChkCombo
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents ckmbDCGroupSetGroup As Bargh_Common.ChkCombo
    Friend WithEvents Label7 As Label
    Friend WithEvents ckmbTamirType As ChkCombo
    Private mIsLoading As Boolean = False

#Region " Windows Form Designer generated code "

    Public Sub New(Optional ByVal aReportType As String = "", Optional ByVal aReportNo As String = "10-3")
        MyBase.New()


        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        mReportType = aReportType
        mReportNo = aReportNo
        mDS = DatasetCcRequester1
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
    Friend WithEvents BttnMakeReport As System.Windows.Forms.Button
    Friend WithEvents BttnReturn As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cboArea As ComboBoxPersian
    Friend WithEvents bttnFromDeDate As System.Windows.Forms.Button
    Friend WithEvents bttnToDeDate As System.Windows.Forms.Button
    Friend WithEvents Label49 As System.Windows.Forms.Label
    Friend WithEvents Label51 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend txtToDate As PersianMaskedEditor
    Friend txtFromDate As PersianMaskedEditor
    Friend WithEvents cmbRequestType As System.Windows.Forms.ComboBox
    Friend WithEvents cmbNwtworkType As System.Windows.Forms.ComboBox
    Friend WithEvents DatasetCcRequester1 As Bargh_DataSets.DatasetCcRequester
    Friend WithEvents prgs As System.Windows.Forms.ProgressBar
    Friend WithEvents lblProgress As System.Windows.Forms.Label
    Friend WithEvents txtNumber As TextBoxPersian
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmReportHourByHourpower))
        Me.BttnMakeReport = New System.Windows.Forms.Button()
        Me.BttnReturn = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.bttnFromDeDate = New System.Windows.Forms.Button()
        Me.bttnToDeDate = New System.Windows.Forms.Button()
        Me.Label49 = New System.Windows.Forms.Label()
        Me.txtToDate = New Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor()
        Me.txtFromDate = New Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor()
        Me.Label51 = New System.Windows.Forms.Label()
        Me.pnlFilter = New System.Windows.Forms.Panel()
        Me.rdbMPFeederPartFilter = New System.Windows.Forms.RadioButton()
        Me.rdbMPFeederFilter = New System.Windows.Forms.RadioButton()
        Me.chkFilterSep = New System.Windows.Forms.CheckBox()
        Me.pnlMain = New System.Windows.Forms.Panel()
        Me.pnlDetails = New System.Windows.Forms.Panel()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.ckmbTamirType = New Bargh_Common.ChkCombo()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.ckmbDCGroupSetGroup = New Bargh_Common.ChkCombo()
        Me.cmbRequestType = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cmbNwtworkType = New System.Windows.Forms.ComboBox()
        Me.txtNumber = New Bargh_Common.TextBoxPersian()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.chkArea = New Bargh_Common.ChkCombo()
        Me.chkCmbMPFeeder = New Bargh_Common.ChkCombo()
        Me.lblMPFeeder = New System.Windows.Forms.Label()
        Me.cmbMPPost = New Bargh_Common.ComboBoxPersian()
        Me.lblMPPost = New System.Windows.Forms.Label()
        Me.cboArea = New Bargh_Common.ComboBoxPersian()
        Me.DatasetCcRequester1 = New Bargh_DataSets.DatasetCcRequester()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.prgs = New System.Windows.Forms.ProgressBar()
        Me.lblProgress = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.pnlFilter.SuspendLayout()
        Me.pnlMain.SuspendLayout()
        Me.pnlDetails.SuspendLayout()
        CType(Me.DatasetCcRequester1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'HelpMaker
        '
        Me.HelpMaker.HelpNamespace = "ReportsHelp.chm"
        '
        'BttnMakeReport
        '
        Me.BttnMakeReport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BttnMakeReport.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.BttnMakeReport.Location = New System.Drawing.Point(277, 332)
        Me.BttnMakeReport.Name = "BttnMakeReport"
        Me.BttnMakeReport.Size = New System.Drawing.Size(75, 23)
        Me.BttnMakeReport.TabIndex = 78
        Me.BttnMakeReport.Text = "تهيه &گزارش"
        '
        'BttnReturn
        '
        Me.BttnReturn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BttnReturn.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.BttnReturn.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.BttnReturn.Location = New System.Drawing.Point(9, 332)
        Me.BttnReturn.Name = "BttnReturn"
        Me.BttnReturn.Size = New System.Drawing.Size(75, 23)
        Me.BttnReturn.TabIndex = 80
        Me.BttnReturn.Text = "&بازگشت"
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.bttnFromDeDate)
        Me.GroupBox1.Controls.Add(Me.bttnToDeDate)
        Me.GroupBox1.Controls.Add(Me.Label49)
        Me.GroupBox1.Controls.Add(Me.txtToDate)
        Me.GroupBox1.Controls.Add(Me.txtFromDate)
        Me.GroupBox1.Controls.Add(Me.Label51)
        Me.GroupBox1.Controls.Add(Me.pnlFilter)
        Me.GroupBox1.Controls.Add(Me.pnlMain)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 8)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(344, 318)
        Me.GroupBox1.TabIndex = 81
        Me.GroupBox1.TabStop = False
        '
        'bttnFromDeDate
        '
        Me.bttnFromDeDate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bttnFromDeDate.Font = New System.Drawing.Font("Mitra", 6.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.bttnFromDeDate.Location = New System.Drawing.Point(158, 19)
        Me.bttnFromDeDate.Name = "bttnFromDeDate"
        Me.bttnFromDeDate.Size = New System.Drawing.Size(24, 20)
        Me.bttnFromDeDate.TabIndex = 84
        Me.bttnFromDeDate.Text = "..."
        '
        'bttnToDeDate
        '
        Me.bttnToDeDate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bttnToDeDate.Font = New System.Drawing.Font("Mitra", 6.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.bttnToDeDate.Location = New System.Drawing.Point(30, 19)
        Me.bttnToDeDate.Name = "bttnToDeDate"
        Me.bttnToDeDate.Size = New System.Drawing.Size(24, 20)
        Me.bttnToDeDate.TabIndex = 86
        Me.bttnToDeDate.Text = "..."
        '
        'Label49
        '
        Me.Label49.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label49.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label49.Location = New System.Drawing.Point(134, 22)
        Me.Label49.Name = "Label49"
        Me.Label49.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label49.Size = New System.Drawing.Size(16, 14)
        Me.Label49.TabIndex = 88
        Me.Label49.Text = "تا"
        Me.Label49.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtToDate
        '
        Me.txtToDate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtToDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtToDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtToDate.IntegerDate = 0
        Me.txtToDate.IsShadow = False
        Me.txtToDate.IsShowCurrentDate = False
        Me.txtToDate.Location = New System.Drawing.Point(54, 19)
        Me.txtToDate.MaxLength = 10
        Me.txtToDate.MiladiDT = CType(resources.GetObject("txtToDate.MiladiDT"), Object)
        Me.txtToDate.Name = "txtToDate"
        Me.txtToDate.ReadOnly = True
        Me.txtToDate.ReadOnlyMaskedEdit = False
        Me.txtToDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtToDate.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtToDate.ShamsiDT = "____/__/__"
        Me.txtToDate.Size = New System.Drawing.Size(72, 20)
        Me.txtToDate.TabIndex = 85
        Me.txtToDate.Text = "____/__/__"
        Me.txtToDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtToDate.TimeMaskedEditorOBJ = Nothing
        '
        'txtFromDate
        '
        Me.txtFromDate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFromDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtFromDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFromDate.IntegerDate = 0
        Me.txtFromDate.IsShadow = False
        Me.txtFromDate.IsShowCurrentDate = False
        Me.txtFromDate.Location = New System.Drawing.Point(182, 19)
        Me.txtFromDate.MaxLength = 10
        Me.txtFromDate.MiladiDT = CType(resources.GetObject("txtFromDate.MiladiDT"), Object)
        Me.txtFromDate.Name = "txtFromDate"
        Me.txtFromDate.ReadOnly = True
        Me.txtFromDate.ReadOnlyMaskedEdit = False
        Me.txtFromDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFromDate.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtFromDate.ShamsiDT = "____/__/__"
        Me.txtFromDate.Size = New System.Drawing.Size(72, 20)
        Me.txtFromDate.TabIndex = 83
        Me.txtFromDate.Text = "____/__/__"
        Me.txtFromDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtFromDate.TimeMaskedEditorOBJ = Nothing
        '
        'Label51
        '
        Me.Label51.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label51.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label51.Location = New System.Drawing.Point(263, 22)
        Me.Label51.Name = "Label51"
        Me.Label51.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label51.Size = New System.Drawing.Size(71, 14)
        Me.Label51.TabIndex = 87
        Me.Label51.Text = "از تاريخ"
        '
        'pnlFilter
        '
        Me.pnlFilter.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlFilter.Controls.Add(Me.rdbMPFeederPartFilter)
        Me.pnlFilter.Controls.Add(Me.rdbMPFeederFilter)
        Me.pnlFilter.Controls.Add(Me.chkFilterSep)
        Me.pnlFilter.Location = New System.Drawing.Point(6, 45)
        Me.pnlFilter.Name = "pnlFilter"
        Me.pnlFilter.Size = New System.Drawing.Size(330, 57)
        Me.pnlFilter.TabIndex = 252
        Me.pnlFilter.Visible = False
        '
        'rdbMPFeederPartFilter
        '
        Me.rdbMPFeederPartFilter.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rdbMPFeederPartFilter.Enabled = False
        Me.rdbMPFeederPartFilter.Font = New System.Drawing.Font("Mitra", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.rdbMPFeederPartFilter.Location = New System.Drawing.Point(17, 28)
        Me.rdbMPFeederPartFilter.Name = "rdbMPFeederPartFilter"
        Me.rdbMPFeederPartFilter.Size = New System.Drawing.Size(146, 26)
        Me.rdbMPFeederPartFilter.TabIndex = 100
        Me.rdbMPFeederPartFilter.Text = "به تفکيک تکه فيدر فشار متوسط"
        '
        'rdbMPFeederFilter
        '
        Me.rdbMPFeederFilter.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rdbMPFeederFilter.Enabled = False
        Me.rdbMPFeederFilter.Font = New System.Drawing.Font("Mitra", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.rdbMPFeederFilter.Location = New System.Drawing.Point(30, 4)
        Me.rdbMPFeederFilter.Name = "rdbMPFeederFilter"
        Me.rdbMPFeederFilter.Size = New System.Drawing.Size(133, 17)
        Me.rdbMPFeederFilter.TabIndex = 99
        Me.rdbMPFeederFilter.Text = "به تفکيک فيدر فشار متوسط"
        '
        'chkFilterSep
        '
        Me.chkFilterSep.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkFilterSep.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkFilterSep.Location = New System.Drawing.Point(156, 16)
        Me.chkFilterSep.Name = "chkFilterSep"
        Me.chkFilterSep.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.chkFilterSep.Size = New System.Drawing.Size(168, 23)
        Me.chkFilterSep.TabIndex = 101
        Me.chkFilterSep.Text = "فيلتر تفکيک انرژی توزيع نشده"
        '
        'pnlMain
        '
        Me.pnlMain.Controls.Add(Me.pnlDetails)
        Me.pnlMain.Controls.Add(Me.chkArea)
        Me.pnlMain.Controls.Add(Me.chkCmbMPFeeder)
        Me.pnlMain.Controls.Add(Me.lblMPFeeder)
        Me.pnlMain.Controls.Add(Me.cmbMPPost)
        Me.pnlMain.Controls.Add(Me.lblMPPost)
        Me.pnlMain.Controls.Add(Me.cboArea)
        Me.pnlMain.Controls.Add(Me.Label2)
        Me.pnlMain.Location = New System.Drawing.Point(4, 105)
        Me.pnlMain.Name = "pnlMain"
        Me.pnlMain.Size = New System.Drawing.Size(330, 199)
        Me.pnlMain.TabIndex = 259
        '
        'pnlDetails
        '
        Me.pnlDetails.Controls.Add(Me.Label7)
        Me.pnlDetails.Controls.Add(Me.ckmbTamirType)
        Me.pnlDetails.Controls.Add(Me.Label6)
        Me.pnlDetails.Controls.Add(Me.ckmbDCGroupSetGroup)
        Me.pnlDetails.Controls.Add(Me.cmbRequestType)
        Me.pnlDetails.Controls.Add(Me.Label4)
        Me.pnlDetails.Controls.Add(Me.cmbNwtworkType)
        Me.pnlDetails.Controls.Add(Me.txtNumber)
        Me.pnlDetails.Controls.Add(Me.Label5)
        Me.pnlDetails.Controls.Add(Me.Label1)
        Me.pnlDetails.Controls.Add(Me.Label3)
        Me.pnlDetails.Location = New System.Drawing.Point(3, 90)
        Me.pnlDetails.Name = "pnlDetails"
        Me.pnlDetails.Size = New System.Drawing.Size(324, 110)
        Me.pnlDetails.TabIndex = 259
        '
        'Label7
        '
        Me.Label7.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label7.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label7.Font = New System.Drawing.Font("Mitra", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label7.Location = New System.Drawing.Point(237, 80)
        Me.Label7.Name = "Label7"
        Me.Label7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label7.Size = New System.Drawing.Size(59, 21)
        Me.Label7.TabIndex = 270
        Me.Label7.Text = "نوع موافقت"
        '
        'ckmbTamirType
        '
        Me.ckmbTamirType.CheckComboDropDownWidth = 0
        Me.ckmbTamirType.CheckGroup = CType(resources.GetObject("ckmbTamirType.CheckGroup"), System.Collections.ArrayList)
        Me.ckmbTamirType.DropDownDirection = UtilityLibrary.Combos.DropDownDirection.Down
        Me.ckmbTamirType.DropHeight = 500
        Me.ckmbTamirType.Font = New System.Drawing.Font("Mitra", 9.75!, System.Drawing.FontStyle.Bold)
        Me.ckmbTamirType.IsGroup = False
        Me.ckmbTamirType.IsMultiSelect = True
        Me.ckmbTamirType.Location = New System.Drawing.Point(30, 80)
        Me.ckmbTamirType.Name = "ckmbTamirType"
        Me.ckmbTamirType.ReadOnlyList = ""
        Me.ckmbTamirType.Size = New System.Drawing.Size(200, 21)
        Me.ckmbTamirType.TabIndex = 271
        Me.ckmbTamirType.Text = "ChkCombo1"
        Me.ckmbTamirType.TreeImageList = Nothing
        '
        'Label6
        '
        Me.Label6.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label6.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label6.Font = New System.Drawing.Font("Mitra", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label6.Location = New System.Drawing.Point(237, 55)
        Me.Label6.Name = "Label6"
        Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label6.Size = New System.Drawing.Size(59, 21)
        Me.Label6.TabIndex = 260
        Me.Label6.Text = "علت قطع"
        '
        'ckmbDCGroupSetGroup
        '
        Me.ckmbDCGroupSetGroup.CheckComboDropDownWidth = 0
        Me.ckmbDCGroupSetGroup.CheckGroup = CType(resources.GetObject("ckmbDCGroupSetGroup.CheckGroup"), System.Collections.ArrayList)
        Me.ckmbDCGroupSetGroup.DropDownDirection = UtilityLibrary.Combos.DropDownDirection.Down
        Me.ckmbDCGroupSetGroup.DropHeight = 500
        Me.ckmbDCGroupSetGroup.Font = New System.Drawing.Font("Mitra", 9.75!, System.Drawing.FontStyle.Bold)
        Me.ckmbDCGroupSetGroup.IsGroup = False
        Me.ckmbDCGroupSetGroup.IsMultiSelect = True
        Me.ckmbDCGroupSetGroup.Location = New System.Drawing.Point(30, 55)
        Me.ckmbDCGroupSetGroup.Name = "ckmbDCGroupSetGroup"
        Me.ckmbDCGroupSetGroup.ReadOnlyList = ""
        Me.ckmbDCGroupSetGroup.Size = New System.Drawing.Size(200, 21)
        Me.ckmbDCGroupSetGroup.TabIndex = 261
        Me.ckmbDCGroupSetGroup.Text = "ChkCombo1"
        Me.ckmbDCGroupSetGroup.TreeImageList = Nothing
        '
        'cmbRequestType
        '
        Me.cmbRequestType.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbRequestType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbRequestType.Items.AddRange(New Object() {"با برنامه", "بي برنامه", "هر دو"})
        Me.cmbRequestType.Location = New System.Drawing.Point(30, 3)
        Me.cmbRequestType.Name = "cmbRequestType"
        Me.cmbRequestType.Size = New System.Drawing.Size(200, 21)
        Me.cmbRequestType.TabIndex = 91
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label4.Location = New System.Drawing.Point(198, 38)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(104, 14)
        Me.Label4.TabIndex = 92
        Me.Label4.Text = "تقسيم روز به بازه هاي"
        Me.Label4.Visible = False
        '
        'cmbNwtworkType
        '
        Me.cmbNwtworkType.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbNwtworkType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbNwtworkType.Items.AddRange(New Object() {"فشار ضعيف", "فشار متوسط", "فوق توزيع", "همه"})
        Me.cmbNwtworkType.Location = New System.Drawing.Point(30, 30)
        Me.cmbNwtworkType.Name = "cmbNwtworkType"
        Me.cmbNwtworkType.Size = New System.Drawing.Size(200, 21)
        Me.cmbNwtworkType.TabIndex = 91
        '
        'txtNumber
        '
        Me.txtNumber.CaptinText = ""
        Me.txtNumber.HasCaption = False
        Me.txtNumber.IsForceText = False
        Me.txtNumber.IsFractional = True
        Me.txtNumber.IsIP = False
        Me.txtNumber.IsNumberOnly = True
        Me.txtNumber.IsYear = False
        Me.txtNumber.Location = New System.Drawing.Point(152, 36)
        Me.txtNumber.MaxLength = 2
        Me.txtNumber.Name = "txtNumber"
        Me.txtNumber.Size = New System.Drawing.Size(40, 20)
        Me.txtNumber.TabIndex = 88
        Me.txtNumber.Visible = False
        '
        'Label5
        '
        Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label5.Location = New System.Drawing.Point(118, 38)
        Me.Label5.Name = "Label5"
        Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label5.Size = New System.Drawing.Size(32, 14)
        Me.Label5.TabIndex = 93
        Me.Label5.Text = "ساعته"
        Me.Label5.Visible = False
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label1.Location = New System.Drawing.Point(235, 6)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(71, 14)
        Me.Label1.TabIndex = 87
        Me.Label1.Text = "نوع خاموشي"
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label3.Location = New System.Drawing.Point(239, 33)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(71, 14)
        Me.Label3.TabIndex = 87
        Me.Label3.Text = "نوع شبکه"
        '
        'chkArea
        '
        Me.chkArea.CheckComboDropDownWidth = 0
        Me.chkArea.CheckGroup = CType(resources.GetObject("chkArea.CheckGroup"), System.Collections.ArrayList)
        Me.chkArea.DropDownDirection = UtilityLibrary.Combos.DropDownDirection.Down
        Me.chkArea.DropHeight = 500
        Me.chkArea.Font = New System.Drawing.Font("Mitra", 9.75!, System.Drawing.FontStyle.Bold)
        Me.chkArea.IsGroup = False
        Me.chkArea.IsMultiSelect = True
        Me.chkArea.Location = New System.Drawing.Point(32, 9)
        Me.chkArea.Name = "chkArea"
        Me.chkArea.ReadOnlyList = ""
        Me.chkArea.Size = New System.Drawing.Size(199, 21)
        Me.chkArea.TabIndex = 260
        Me.chkArea.Text = "ChkCombo1"
        Me.chkArea.TreeImageList = Nothing
        '
        'chkCmbMPFeeder
        '
        Me.chkCmbMPFeeder.CheckComboDropDownWidth = 0
        Me.chkCmbMPFeeder.CheckGroup = CType(resources.GetObject("chkCmbMPFeeder.CheckGroup"), System.Collections.ArrayList)
        Me.chkCmbMPFeeder.DropDownDirection = UtilityLibrary.Combos.DropDownDirection.Down
        Me.chkCmbMPFeeder.DropHeight = 500
        Me.chkCmbMPFeeder.Enabled = False
        Me.chkCmbMPFeeder.Font = New System.Drawing.Font("Tahoma", 8.0!)
        Me.chkCmbMPFeeder.IsGroup = False
        Me.chkCmbMPFeeder.IsMultiSelect = True
        Me.chkCmbMPFeeder.Location = New System.Drawing.Point(32, 63)
        Me.chkCmbMPFeeder.Name = "chkCmbMPFeeder"
        Me.chkCmbMPFeeder.ReadOnlyList = ""
        Me.chkCmbMPFeeder.Size = New System.Drawing.Size(200, 21)
        Me.chkCmbMPFeeder.TabIndex = 258
        Me.chkCmbMPFeeder.TreeImageList = Nothing
        '
        'lblMPFeeder
        '
        Me.lblMPFeeder.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMPFeeder.AutoSize = True
        Me.lblMPFeeder.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMPFeeder.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.lblMPFeeder.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.lblMPFeeder.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMPFeeder.Location = New System.Drawing.Point(239, 69)
        Me.lblMPFeeder.Name = "lblMPFeeder"
        Me.lblMPFeeder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMPFeeder.Size = New System.Drawing.Size(88, 13)
        Me.lblMPFeeder.TabIndex = 253
        Me.lblMPFeeder.Text = "فيدر فشار متوسط"
        '
        'cmbMPPost
        '
        Me.cmbMPPost.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbMPPost.BackColor = System.Drawing.Color.White
        Me.cmbMPPost.DisplayMember = "MPPostName"
        Me.cmbMPPost.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMPPost.Enabled = False
        Me.cmbMPPost.IsReadOnly = False
        Me.cmbMPPost.Location = New System.Drawing.Point(32, 36)
        Me.cmbMPPost.Name = "cmbMPPost"
        Me.cmbMPPost.Size = New System.Drawing.Size(200, 21)
        Me.cmbMPPost.TabIndex = 256
        Me.cmbMPPost.ValueMember = "MPPostId"
        '
        'lblMPPost
        '
        Me.lblMPPost.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMPPost.AutoSize = True
        Me.lblMPPost.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMPPost.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.lblMPPost.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.lblMPPost.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMPPost.Location = New System.Drawing.Point(239, 39)
        Me.lblMPPost.Name = "lblMPPost"
        Me.lblMPPost.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMPPost.Size = New System.Drawing.Size(80, 13)
        Me.lblMPPost.TabIndex = 255
        Me.lblMPPost.Text = "پست فوق توزيع"
        '
        'cboArea
        '
        Me.cboArea.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboArea.BackColor = System.Drawing.Color.White
        Me.cboArea.DataSource = Me.DatasetCcRequester1.Tbl_Area
        Me.cboArea.DisplayMember = "Area"
        Me.cboArea.Enabled = False
        Me.cboArea.IsReadOnly = False
        Me.cboArea.Location = New System.Drawing.Point(32, 9)
        Me.cboArea.Name = "cboArea"
        Me.cboArea.Size = New System.Drawing.Size(200, 21)
        Me.cboArea.TabIndex = 89
        Me.cboArea.ValueMember = "AreaId"
        '
        'DatasetCcRequester1
        '
        Me.DatasetCcRequester1.DataSetName = "DatasetCcRequester"
        Me.DatasetCcRequester1.Locale = New System.Globalization.CultureInfo("en-US")
        Me.DatasetCcRequester1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label2.Location = New System.Drawing.Point(241, 12)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(71, 14)
        Me.Label2.TabIndex = 90
        Me.Label2.Text = "ناحيه مرتبط"
        '
        'prgs
        '
        Me.prgs.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.prgs.Location = New System.Drawing.Point(92, 348)
        Me.prgs.Name = "prgs"
        Me.prgs.Size = New System.Drawing.Size(176, 8)
        Me.prgs.TabIndex = 82
        Me.prgs.Visible = False
        '
        'lblProgress
        '
        Me.lblProgress.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblProgress.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.lblProgress.Location = New System.Drawing.Point(92, 332)
        Me.lblProgress.Name = "lblProgress"
        Me.lblProgress.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblProgress.Size = New System.Drawing.Size(176, 14)
        Me.lblProgress.TabIndex = 87
        Me.lblProgress.Text = "در حال تهيه گزارش "
        Me.lblProgress.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblProgress.Visible = False
        '
        'frmReportHourByHourpower
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.BttnReturn
        Me.ClientSize = New System.Drawing.Size(360, 361)
        Me.Controls.Add(Me.prgs)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.BttnMakeReport)
        Me.Controls.Add(Me.BttnReturn)
        Me.Controls.Add(Me.lblProgress)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.HelpMaker.SetHelpNavigator(Me, System.Windows.Forms.HelpNavigator.Topic)
        Me.MaximizeBox = False
        Me.Name = "frmReportHourByHourpower"
        Me.HelpMaker.SetShowHelp(Me, True)
        Me.Text = "گزارش انرژي توزيع نشده ساعت به ساعت"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.pnlFilter.ResumeLayout(False)
        Me.pnlMain.ResumeLayout(False)
        Me.pnlMain.PerformLayout()
        Me.pnlDetails.ResumeLayout(False)
        Me.pnlDetails.PerformLayout()
        CType(Me.DatasetCcRequester1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub frmReportHourByHourpower_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ApplySkin(Me)
        Dim Ds As Bargh_DataSets.DatasetCcRequester = DatasetCcRequester1
        Dim Cnn As SqlConnection = New SqlConnection(GetConnection())

        LoadAreaDataTable(Cnn, Ds)
        chkArea.Fill(DatasetCcRequester1.Tbl_Area, "Area", "AreaId")
        If IsCenter Then
            chkArea.Enabled = True
            chkArea.UnCheckAll()
            cboArea.Enabled = True
            cboArea.SelectedIndex = -1
        Else
            cboArea.SelectedValue = WorkingAreaId
            chkArea.SetData(WorkingAreaId)
        End If
        If mReportType = "ExcelReport" Then
            cmbNwtworkType.Visible = False
            Label3.Visible = False
            ckmbDCGroupSetGroup.Visible = False
            Label7.Visible = False
            ckmbTamirType.Visible = False
            Label6.Visible = False
            txtNumber.Visible = True
            Label4.Visible = True
            Label5.Visible = True
            Me.Text = "گزارش قطعي فيدرها در ساعات روز"
            Me.Height = Me.Height - (ckmbDCGroupSetGroup.Height + ckmbTamirType.Height)
            Me.GroupBox1.Height = GroupBox1.Height - ckmbTamirType.Height
            pnlFilter.Visible = True
            chkArea.Visible = False
            cboArea.Visible = True
            pnlDetails.Height = pnlDetails.Height - (ckmbDCGroupSetGroup.Height + ckmbTamirType.Height)
        Else
            Select Case mReportNo
                Case "10-3"
                    Dim lSql As String = "SELECT * FROM Tbl_DisconnectGroupSetGroup "
                    BindingTable(lSql, mCnn, mDS, "Tbl_DisconnectGroupSetGroup", aIsClearTable:=True)
                    lSql = "SELECT * FROM Tbl_TamirType "
                    BindingTable(lSql, mCnn, mDS, "Tbl_TamirType", aIsClearTable:=True)
                    chkArea.Visible = True
                    cboArea.Visible = False
                    Me.Height = Me.Height - pnlFilter.Height - pnlDetails.Height + ckmbDCGroupSetGroup.Height + ckmbTamirType.Height
                    pnlMain.Top = txtFromDate.Top + txtFromDate.Height
                    pnlDetails.Top = chkArea.Top + chkArea.Height + 10
                    lblMPPost.Visible = False
                    cmbMPPost.Visible = False
                    chkCmbMPFeeder.Visible = False
                    lblMPFeeder.Visible = False
                    ckmbDCGroupSetGroup.Fill(mDS.Tbl_DisconnectGroupSetGroup, "DisconnectGroupSetGroup", "DisconnectGroupSetGroupId")
                    ckmbDCGroupSetGroup.Visible = True
                    Label7.Visible = True
                    ckmbTamirType.Visible = True
                    ckmbTamirType.Fill(mDS.Tbl_TamirType, "TamirType", "TamirTypeId")
                    Label6.Visible = True
                Case "10-4"
                    Me.cmbNwtworkType.Items.AddRange(New Object() {"فشار متوسط", "فوق توزيع", "همه"})
            End Select
        End If
        cmbRequestType.SelectedIndex = 2
        cmbNwtworkType.SelectedIndex = 3
        txtFromDate.ClipDate = Now.AddDays(-1)
        txtToDate.ClipDate = Now
        mIsLoading = False
    End Sub
    Private Sub frm_HelpRequested(ByVal sender As Object, ByVal hlpevent As System.Windows.Forms.HelpEventArgs) Handles MyBase.HelpRequested
        ShowReportHelp(Me, , "rptHourByHourPower.rpt")
        hlpevent.Handled = True
    End Sub

    Private Sub bttnFromDeDate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bttnFromDeDate.Click
        ShowCalendar(txtFromDate.Text, txtFromDate)
    End Sub
    Private Sub bttnToDate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bttnToDeDate.Click
        ShowCalendar(txtToDate.Text, txtToDate)
    End Sub

    Private Sub CreateQuery()
        mFiltersInfo = ""
        mErr = False
        '---------------------------<omid>
        Select Case mReportNo
            Case "10-3"
                If (Not CQC.checkFromDate(Me)) Then Exit Sub
                If (Not CQC.checkToDate(Me)) Then Exit Sub
                CQC.checkReqType(Me)
                CQC.checkTamirType(Me)
                CQC.checkGroupSet(Me)
                CQC.checkMPFeeder(Me)
            Case "10-4"
                If (Not CQC.checkFromDate(Me, True)) Then Exit Sub
            Case Else
                If (Not CQC.checkFromDate(Me)) Then Exit Sub
                If (Not CQC.checkToDate(Me)) Then Exit Sub
                CQC.checkReqType(Me)
                CQC.checkTamirType(Me)
                CQC.checkGroupSet(Me)
        End Select
        If (Not CQC.checkReportType(Me)) Then Exit Sub
        CQC.checkWorkingArea(Me)
        CQC.checkAreaID(Me)
        If mFiltersInfo <> "" Then
            mFiltersInfo = mFiltersInfo.Remove(0, 3)
        End If
        Exit Sub
        '---------------------------</omid>
        If txtFromDate.Text = "____/__/__" Then
            MsgBoxF("تاريخ شروع الزامي مي‌باشد", "توجه", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Information, MessageBoxDefaultButton.Button1)
            mErr = True
            Exit Sub
        End If
        If txtToDate.Text = "____/__/__" Then
            MsgBoxF("تاريخ پايان مي‌باشد", "توجه", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Information, MessageBoxDefaultButton.Button1)
            mErr = True
            Exit Sub
        End If

        If txtToDate.MiladiDT < txtFromDate.MiladiDT Then
            MsgBoxF("تاريخ شروع بايد قبل از تاريخ پايان باشد", "توجه", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Information, MessageBoxDefaultButton.Button1)
            mErr = True
            Exit Sub
        End If

        If mReportType = "ExcelReport" And (txtNumber.Text = "" Or Val(txtNumber.Text) <= 0 Or Val(txtNumber.Text) > 24) Then
            MsgBoxF("بازه هاي ساعتي معتبر نمي‌باشد", "توجه", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Information, MessageBoxDefaultButton.Button1)
            mErr = True
            Exit Sub
        End If

        mFiltersInfo &= " - از تاريخ " & ConvertReportDate(txtFromDate.Text)
        mFiltersInfo &= " تا تاريخ " & ConvertReportDate(txtToDate.Text)

        mAreaId = -1
        mAreaIDs = ""
        If cboArea.Visible Then
            If cboArea.SelectedIndex > -1 Then
                mAreaId = cboArea.SelectedValue
                mFiltersInfo &= " - ناحيه " & cboArea.Text
            ElseIf Not IsSetadMode Then
                mAreaId = WorkingAreaId
            End If
        End If

        If chkArea.Visible Then
            If IsCenter And chkArea.GetDataList = "" Then
                mAreaIDs = GetLegalAreaIDs(mCnn)
            End If
            If mAreaIDs = "" Then
                mAreaIDs = chkArea.GetDataList()
                If mAreaIDs <> "" Then
                    If mAreaIDs.Split(",").Length > 1 Then
                        mFiltersInfo &= " نواحي "
                    Else
                        mFiltersInfo &= " ناحيه "
                    End If
                    mFiltersInfo &= chkArea.GetDataTextList()
                End If
            End If

            If mAreaIDs = "" Then
                mAreaIDs = GetNotCenterAreaIDs()
            End If
        End If

        If chkCmbMPFeeder.GetDataList <> "" Then
            mMPFeederIds = chkCmbMPFeeder.GetDataList
            mFiltersInfo &= " - " & chkCmbMPFeeder.GetDataList
        ElseIf Not IsSetadMode Then
            mMPFeederIds = ""
        End If
        If cmbNwtworkType.SelectedIndex >= 0 Then
            Select Case cmbNwtworkType.Text
                Case "فشار ضعيف"
                    mIsLP = True
                    mIsMP = False
                    mIsFT = False
                    mFiltersInfo &= " - شبکه فشار ضعيف"
                Case "فشار متوسط"
                    mIsMP = True
                    mIsLP = False
                    mIsFT = False
                    mFiltersInfo &= " - شبکه فشار متوسط"
                Case "فوق توزيع"
                    mIsFT = True
                    mIsLP = False
                    mIsMP = False
                    mFiltersInfo &= " - شبکه فوق توزيع"
            End Select
        End If

        If cmbRequestType.SelectedIndex >= 0 Then
            Select Case cmbRequestType.Text
                Case "با برنامه"
                    mIsTamir = True
                    mIsNotTamir = False
                    mFiltersInfo &= " - با برنامه"
                Case "بي برنامه"
                    mIsTamir = False
                    mIsNotTamir = True
                    mFiltersInfo &= " - بي برنامه"
            End Select
        End If
        If ckmbDCGroupSetGroup.GetDataList.Length > 0 Then
            mDCGroupSetGroupId = ckmbDCGroupSetGroup.GetDataList
            mFiltersInfo &= " - علت قطع" & ckmbDCGroupSetGroup.GetDataTextList
        End If

        If ckmbTamirType.GetDataList.Length > 0 Then
            mTamirTypeId = ckmbTamirType.GetDataList
            mFiltersInfo &= " - نوع موافقت " & ckmbTamirType.GetDataTextList
        End If

        If mFiltersInfo <> "" Then
            mFiltersInfo = mFiltersInfo.Remove(0, 3)
        End If
    End Sub
    Private Function GetpartPower(ByVal aDate1 As Date, ByVal aDate2 As Date, ByVal aFromDate As String, ByVal aToDate As String) As Double
        Dim lSQL As String = ""
        Dim lDs As New Bargh_DataSets.DatasetCcRequester
        Dim lCnn As SqlConnection = New SqlConnection(GetConnection())
        Dim lDate1 As String, lDate2 As String

        Dim lWhereFTinMP As String
        lWhereFTinMP = _
            " ( TblMPRequest.DisconnectGroupSetId IN (1129,1130) " & _
            " OR (TblMPRequest.DisconnectReasonId >= 1200 AND TblMPRequest.DisconnectReasonId <= 1299 AND NOT TblMPRequest.DisconnectReasonId IS NULL) ) "

        lDate1 = aDate1.ToShortDateString & " " & aDate1.Hour & ":" & aDate1.Minute
        lDate2 = aDate2.ToShortDateString & " " & aDate2.Hour & ":" & aDate2.Minute

        lSQL &= _
            " SELECT " & _
            "	SUM " & _
            "	( " & _
            "		dbo.MinuteCount " & _
            "		( " & _
            "			CONVERT(DATETIME, '" & lDate1 & "' , 102), " & _
            "			CONVERT(DATETIME, '" & lDate2 & "' , 102), " & _
            "			TblRequest.DisconnectDT, " & _
            "			TblRequest.ConnectDT " & _
            "		) " & _
            "		/ TblRequest.DisconnectInterval " & _
            "		* TblRequest.DisconnectPower " & _
            "	) AS PartPower " & _
            " FROM TblRequest"

        If mIsMP Or mIsFT Then
            lSQL &= " LEFT OUTER JOIN TblMPRequest ON TblRequest.MPRequestId = TblMPRequest.MPRequestId"
        End If

        lSQL &=
            " WHERE " &
            "	(TblRequest.DisconnectInterval > 0) AND (NOT (TblRequest.ConnectDT IS NULL)) " &
            "	AND TblRequest.DisconnectDatePersian >= '" & aFromDate & "' AND TblRequest.DisconnectDatePersian <= '" & aToDate & "' " &
            "	AND " & IIf(mIsLP, "IsLPRequest=1", IIf(mIsMP, "(IsMPRequest=1 AND NOT " & lWhereFTinMP & ")", IIf(mIsFT, "(IsFogheToziRequest=1 OR (IsMpRequest=1 AND" & lWhereFTinMP & "))", "(IsMPRequest=1 OR IsLPRequest=1 OR IsFogheToziRequest=1)"))) &
            "	" & IIf(mIsTamir, "AND IsTamir=1", IIf(mIsNotTamir, "AND IsTamir=0", "")) &
            "	" & IIf(mAreaId > 0, "AND TblRequest.AreaId = " & mAreaId, "") &
            "	" & IIf(mAreaIDs <> "", "AND TblRequest.AreaId IN (" & mAreaIDs & ") ", "") &
            "   " & IIf(mDCGroupSetGroupId <> "", " AND DisconnectGroupSetId IN ( " & mDCGroupSetGroupId & ")", "") &
            "   " & IIf(mTamirTypeId <> "", " AND TblRequest.TamirTypeId IN ( " & mTamirTypeId & ")", "") &
            " ORDER BY  " &
            "	PartPower DESC "

        RemoveMoreSpaces(lSQL)
        BindingTable(lSQL, lCnn, lDs, "TblPartPower")
        Dim lPower As Object = lDs.Tables("TblPartPower").Rows(0)("PartPower")
        If IsDBNull(lPower) Then lPower = 0
        Return Math.Round(Convert.ToDouble(lPower), 3)
    End Function
    Private Function MakeData(ByVal aPath As String) As Boolean
        MakeData = False
        If mIsReporting Then Exit Function
        mIsReporting = True

        Dim lDs As New DataSet, lTbl As New DataTable
        Dim lDate1 As Date, lDate2 As Date, lDate3 As Date
        Dim lDate As String, i As Integer
        Dim lColsNum As Integer = 26
        Dim lSumPower As Double
        Dim lPrgsMax As Long
        Dim lColCounter As Integer = 2
        Dim lColSumTotal As Integer = 26
        Try
            Me.Cursor = Cursors.WaitCursor

            lDate1 = txtFromDate.MiladiDT
            lDate2 = txtToDate.MiladiDT

            lPrgsMax = DateDiff(DateInterval.Day, lDate1, lDate2) + 1
            lPrgsMax *= 24

            lTbl.Columns.Add("DatePersian")
            lTbl.Columns.Add("DT")

            If chkFilterSep.Checked Then
                lTbl.Columns.Add("FeederName")
                lColsNum = 27
                If rdbMPFeederPartFilter.Checked Then
                    lTbl.Columns.Add("FeederPart")
                    lColsNum = 28
                End If
            End If

            Dim lCols(lColsNum) As Object

            Dim lColName As String
            For i = 0 To 23
                lColName = "W" & IIf(i = 0, "00", i) & "_" & IIf(i < 23, i + 1, "00")
                lTbl.Columns.Add(lColName, System.Type.GetType("System.Double"))
            Next

            lTbl.Columns.Add("SumPower", System.Type.GetType("System.Double"))

            prgs.Maximum = lPrgsMax
            prgs.Value = 0
            prgs.Visible = True
            lblProgress.Visible = True
            Do While lDate1 <= lDate2
                lDate = GetPersianDate(lDate1)
                lblProgress.Text = "در حال تهيه گزارش روز " & lDate

                lCols(0) = lDate
                lCols(1) = lDate1

                If chkFilterSep.Checked Then
                    lCols(2) = lDate1
                    lColCounter = 3
                    lColSumTotal = 27
                    If rdbMPFeederPartFilter.Checked Then
                        lCols(3) = lDate1
                        lColCounter = 4
                        lColSumTotal = 28
                    End If
                End If

                lDate3 = lDate1
                lSumPower = 0
                For i = 0 To 23
                    lCols(i + lColCounter) = GetpartPower(lDate3, lDate3.AddHours(1), txtFromDate.Text, txtToDate.Text)
                    lSumPower += lCols(i + 2)
                    prgs.Value += 1
                    lDate3 = lDate3.AddHours(1)
                    If Not mIsReporting Then Exit For
                Next
                lCols(lColSumTotal) = lSumPower
                If Not mIsReporting Then Exit Do

                Application.DoEvents()
                lTbl.Rows.Add(lCols)
                lDate1 = lDate1.AddDays(1)
            Loop
            lblProgress.Visible = False
            prgs.Visible = False
            If Not mIsReporting Then Exit Function

            lDs.Tables.Add(lTbl)
            lDs.WriteXml(ReportsXMLPath & "Dates.xml", XmlWriteMode.WriteSchema)

            MakeData = True
        Catch ex As Exception
        Finally
            Me.Cursor = Cursors.Default
            mIsReporting = False
        End Try

    End Function

    Private Sub BttnMakeReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BttnMakeReport.Click
        CreateQuery()
        If mErr Then Exit Sub

        Dim PathRep As String = ""

        If mReportType = "ExcelReport" Then
            If chkFilterSep.Checked Then
                MakeExcel_FeederByFeeder()
            Else
                MakeExcel()
            End If
        Else
            If IsSetadMode Then
                PathRep = "Reports\Setad\"
            ElseIf IsCenter Or IsMiscMode Then
                PathRep = "Reports\Center\"
            Else
                PathRep = "Reports\Department\"
            End If

            If MakeData(PathRep) Then
                PathRep &= "rptHourByHourPower.rpt"
                ShowReport("", PathRep, "گزارش انرژي توزيع نشده ساعت به ساعت", """ميزان انرژي توزيع نشده ساعت به ساعت توزيع " & WorkingProvinceName & """", , mFiltersInfo, , "(10-3)")
            End If

            mIsLP = False
            mIsMP = False
            mIsFT = False
            mIsTamir = False
            mIsNotTamir = False
        End If
    End Sub
    Private Sub BttnReturn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BttnReturn.Click
        mIsReporting = False
    End Sub

    Private Sub frmReportHourByHourpower_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed
        If Not mExcel Is Nothing Then
            mExcel.CloseApp()
        End If
    End Sub
    Private Sub MakeExcel()

        If mIsReporting Then Exit Sub
        mIsReporting = True

        Dim lHour As Integer = txtNumber.Text
        Dim lColumnCount As Integer = Math.Ceiling(24 / lHour)
        Dim lPath As String = VB6.GetPath & "\Reports\Excel\"
        Dim lFileName As String = "Report_10_23"
        Dim lRow As DataRow
        Dim lLastCol As String, lSheet As String
        Dim lTbl As New DataTable
        Dim lDs As New DataSet
        Dim lDate1 As Date, lDate2 As Date, lDate3 As Date
        Dim lDate As String
        Dim i As Integer
        Dim k As Integer = 0
        Dim lCellNameFilter As String = "d1"
        Dim lCountFeeder As Double
        Dim lPrgsMax As Long
        Dim lColCounter As Integer = 2
        If rdbMPFeederFilter.Checked Then
            lColCounter = 3
        ElseIf rdbMPFeederPartFilter.Checked Then
            lColCounter = 4
        End If
        Dim lCols(lColumnCount + lColCounter) As Object
        Try

            Me.Cursor = Cursors.WaitCursor

            lDate1 = txtFromDate.MiladiDT
            lDate2 = txtToDate.MiladiDT

            lPrgsMax = DateDiff(DateInterval.Day, lDate1, lDate2) + 1
            'lPrgsMax *= 24

            lTbl.Columns.Add("DatePersian")
            lTbl.Columns.Add("DT")
            If chkFilterSep.Checked Then
                lTbl.Columns.Add("FeederName")
                If rdbMPFeederPartFilter.Checked Then
                    lTbl.Columns.Add("FeederPart")
                End If
            End If

            Dim lColName As String

            For i = 0 To lColumnCount - 1
                lColName = (i * lHour).ToString("0#") & "_" & IIf(i < lColumnCount - 1, ((i + 1) * lHour).ToString("0#"), "00")
                lTbl.Columns.Add(lColName, System.Type.GetType("System.Double"))
                'lCols(i) = lCountFeeder
            Next

            lTbl.Columns.Add("lCountFeeder", System.Type.GetType("System.Double"))

            If mExcel Is Nothing Then
                mExcel = New CExcel
            Else
                mExcel.CloseWorkBook()
            End If
            mExcel.OpenWorkBook(lPath & lFileName)
            lLastCol = "V"
            lSheet = "Sheet1"

            '---------------------------------------

            prgs.Maximum = lPrgsMax
            prgs.Value = 0
            prgs.Visible = True
            lblProgress.Visible = True


            Do While lDate1 <= lDate2
                lDate = GetPersianDate(lDate1)
                lblProgress.Text = "در حال تهيه گزارش روز " & lDate
                lCols(0) = lDate
                lCols(1) = lDate1.ToShortDateString

                lDate3 = lDate1
                lCountFeeder = 0
                For i = 0 To lColumnCount - 1
                    lCols(i + lColCounter) = GetDisMPFeeder(lDate3, lDate3.AddHours(lHour), txtFromDate.Text, txtToDate.Text)
                    If chkFilterSep.Checked Then
                        If mMPFeederName <> "" Then
                            lCols(2) = mMPFeederName
                        End If

                        lCellNameFilter = "e1"
                        If rdbMPFeederPartFilter.Checked Then
                            lCols(3) = mFeederPartName
                            lCellNameFilter = "f1"
                        End If
                    End If

                    lCountFeeder += lCols(i + lColCounter)
                    'prgs.Value += 1
                    lDate3 = lDate3.AddHours(lHour)
                    If Not mIsReporting Then Exit For
                Next
              
                lCols(i + lColCounter) = lCountFeeder
                If Not mIsReporting Then Exit Do

                AdvanceProgressBar()

                lTbl.Rows.Add(lCols)
                lDate1 = lDate1.AddDays(1)
            Loop

            Dim lChangeRow As Integer = 0
            Dim lIsDupTrans As Boolean = False
            Dim lIsDupTablo As Boolean = False

            lDs.Tables.Add(lTbl)
            Dim CellAddress As String = lCellNameFilter
            For k = 0 To lColumnCount - 1
                lColName = (k * lHour).ToString("0#") & "_" & IIf(k < lColumnCount - 1, ((k + 1) * lHour).ToString("0#"), "00")
                mExcel.WriteCell(lSheet, CellAddress, lColName, CExcel.ExcelLineStyles.xlContinuous, , CExcel.TextHAlign.Center, CExcel.TextVAlign.Center)
                If k < lColumnCount - 1 Then
                    mExcel.InsertColumn(lSheet, CellAddress, CExcel.ExcelReadingOrders.xlRTL)
                    CellAddress = CExcel.GetNextHCell(CellAddress)
                End If
            Next

            prgs.Maximum = lTbl.Rows.Count
            prgs.Value = 0
            lblProgress.Text = "در حال درج اطلاعات"
            i = 2
            For Each lRow In lTbl.Rows

                Dim lColumns As Integer = lTbl.Columns.Count
                mExcel.WriteCell(lSheet, "A" & i, (i - 2).ToString(), CExcel.ExcelLineStyles.xlContinuous, , CExcel.TextHAlign.Center, CExcel.TextVAlign.Center)
                mExcel.WriteCell(lSheet, "B" & i, lRow("DatePersian"), CExcel.ExcelLineStyles.xlContinuous, , CExcel.TextHAlign.Center, CExcel.TextVAlign.Center)
                mExcel.WriteCell(lSheet, "c" & i, lRow("DT"), CExcel.ExcelLineStyles.xlContinuous, , CExcel.TextHAlign.Center, CExcel.TextVAlign.Center)

                CellAddress = "d" & i
                If chkFilterSep.Checked Then
                    mExcel.WriteCell(lSheet, "D" & i, lRow("FeederName"), CExcel.ExcelLineStyles.xlContinuous, , CExcel.TextHAlign.Center, CExcel.TextVAlign.Center)
                    CellAddress = "e" & i
                    If rdbMPFeederPartFilter.Checked Then
                        mExcel.WriteCell(lSheet, "E" & i, lRow("FeederPart"), CExcel.ExcelLineStyles.xlContinuous, True, CExcel.TextHAlign.Center, CExcel.TextVAlign.Center)
                        CellAddress = "f" & i
                    End If
                End If

                For k = 0 To lColumnCount - 1
                    lColName = (k * lHour).ToString("0#") & "_" & IIf(k < lColumnCount - 1, ((k + 1) * lHour).ToString("0#"), "00")
                    mExcel.WriteCell(lSheet, CellAddress, lRow(lColName), CExcel.ExcelLineStyles.xlContinuous, , CExcel.TextHAlign.Center, CExcel.TextVAlign.Center)
                    CellAddress = mExcel.GetNextHCell(CellAddress)
                Next
          
                mExcel.WriteCell(lSheet, CellAddress, lRow("lCountFeeder"), CExcel.ExcelLineStyles.xlContinuous, , CExcel.TextHAlign.Center, CExcel.TextVAlign.Center)
                If chkFilterSep.Checked Then
                    Dim lSumHeader As String = Microsoft.VisualBasic.Left(CellAddress, 1) & "1"
                    mExcel.WriteCell(lSheet, "D1", "فیدرفشار متوسط", CExcel.ExcelLineStyles.xlContinuous, , CExcel.TextHAlign.Center, CExcel.TextVAlign.Center)
                    If rdbMPFeederPartFilter.Checked Then
                        mExcel.WriteCell(lSheet, "E1", "تکه فیدر ", CExcel.ExcelLineStyles.xlContinuous, , CExcel.TextHAlign.Center, CExcel.TextVAlign.Center)
                    End If
                    mExcel.WriteCell(lSheet, lSumHeader, "جمع فیدرها", CExcel.ExcelLineStyles.xlContinuous, , CExcel.TextHAlign.Center, CExcel.TextVAlign.Center)
                End If

                If i Mod 4 = 0 Then AdvanceProgressBar(4)
                i += 1
            Next
            prgs.Value = prgs.Maximum
            '---------------------------------------
            mExcel.SelectRange(lSheet, "A1")
            mExcel.ShowExcel()

        Catch ex As Exception
            ShowError(ex)
        Finally
            Me.Cursor = Cursors.Default
            mIsReporting = False
        End Try
        lblProgress.Visible = False
        prgs.Visible = False
        If Not mIsReporting Then Exit Sub

    End Sub

    Private Sub MakeExcel_FeederByFeeder()

        If mIsReporting Then Exit Sub
        mIsReporting = True

        mMPFeederIds = ""
        Dim lHour As Integer = txtNumber.Text
        Dim lColumnCount As Integer = Math.Ceiling(24 / lHour)
        Dim lPath As String = VB6.GetPath & "\Reports\Excel\"
        Dim lFileName As String = "Report_10_23"
        Dim lRow As DataRow
        Dim lLastCol As String, lSheet As String
        Dim lTbl As New DataTable
        Dim lDs As New DataSet
        Dim lDate1 As Date, lDate2 As Date, lDate3 As Date
        Dim lDate As String
        Dim i As Integer
        Dim k As Integer = 0
        Dim lCellNameFilter As String = "d1"
        Dim lCountFeeder As Double
        Dim lPrgsMax As Long
        Dim lColCounter As Integer = 2
        If rdbMPFeederFilter.Checked Then
            lColCounter = 3
        ElseIf rdbMPFeederPartFilter.Checked Then
            lColCounter = 4
        End If
        Dim lCols(lColumnCount + lColCounter) As Object
        Try

            Me.Cursor = Cursors.WaitCursor

            lDate1 = txtFromDate.MiladiDT
            lDate2 = txtToDate.MiladiDT

            lPrgsMax = DateDiff(DateInterval.Day, lDate1, lDate2) + 1
            'lPrgsMax *= 24

            lTbl.Columns.Add("DatePersian")
            lTbl.Columns.Add("DT")
            If chkFilterSep.Checked Then
                lTbl.Columns.Add("FeederName")
                If rdbMPFeederPartFilter.Checked Then
                    lTbl.Columns.Add("FeederPart")
                End If
            End If

            Dim lColName As String

            For i = 0 To lColumnCount - 1
                lColName = (i * lHour).ToString("0#") & "_" & IIf(i < lColumnCount - 1, ((i + 1) * lHour).ToString("0#"), "00")
                lTbl.Columns.Add(lColName, System.Type.GetType("System.Double"))
                'lCols(i) = lCountFeeder
            Next

            lTbl.Columns.Add("lCountFeeder", System.Type.GetType("System.Double"))

            If mExcel Is Nothing Then
                mExcel = New CExcel
            Else
                mExcel.CloseWorkBook()
            End If
            mExcel.OpenWorkBook(lPath & lFileName)
            lLastCol = "V"
            lSheet = "Sheet1"

            '---------------------------------------

            prgs.Maximum = lPrgsMax
            prgs.Value = 0
            prgs.Visible = True
            lblProgress.Visible = True


            Do While lDate1 <= lDate2
                lDate = GetPersianDate(lDate1)
                lblProgress.Text = "در حال تهيه گزارش روز " & lDate
                lCols(0) = lDate
                lCols(1) = lDate1.ToShortDateString



                GetDisMPFeeder_FeederList(txtFromDate.Text, txtToDate.Text)
                For Each Index As DataRow In mDS.Tables("TblCountFeederList").Select("DisconnectMPFeeder > 0")
                    lCountFeeder = 0
                    lDate3 = lDate1
                    lCols(2) = Index.Item("MPFeederName")
                    lCellNameFilter = "e1"
                    If rdbMPFeederPartFilter.Checked Then
                        lCols(3) = Index.Item("FeederPart")
                        lCellNameFilter = "f1"
                    End If
                    For i = 0 To lColumnCount - 1
                        lCols(i + lColCounter) = GetDisMPFeeder_FeederByFeeder(lDate3, lDate3.AddHours(lHour), txtFromDate.Text, txtToDate.Text, Index.Item("MPFeederId"))
                        lCountFeeder += lCols(i + lColCounter)
                        lDate3 = lDate3.AddHours(lHour)

                        'prgs.Value += 1
                    Next
                    lCols(i + lColCounter) = lCountFeeder
                    lTbl.Rows.Add(lCols)
                Next

                If Not mIsReporting Then Exit Do
                AdvanceProgressBar()

                lDate1 = lDate1.AddDays(1)
   
                AdvanceProgressBar()
            Loop

            Dim lChangeRow As Integer = 0
            Dim lIsDupTrans As Boolean = False
            Dim lIsDupTablo As Boolean = False

            lDs.Tables.Add(lTbl)
            Dim CellAddress As String = lCellNameFilter
            For k = 0 To lColumnCount - 1
                lColName = (k * lHour).ToString("0#") & "_" & IIf(k < lColumnCount - 1, ((k + 1) * lHour).ToString("0#"), "00")
                mExcel.WriteCell(lSheet, CellAddress, lColName, CExcel.ExcelLineStyles.xlContinuous, , CExcel.TextHAlign.Center, CExcel.TextVAlign.Center)
                If k < lColumnCount - 1 Then
                    mExcel.InsertColumn(lSheet, CellAddress, CExcel.ExcelReadingOrders.xlRTL)
                    CellAddress = CExcel.GetNextHCell(CellAddress)
                End If
            Next

            prgs.Maximum = lTbl.Rows.Count
            prgs.Value = 0
            lblProgress.Text = "در حال درج اطلاعات"
            i = 2
            For Each lRow In lTbl.Rows

                Dim lColumns As Integer = lTbl.Columns.Count
                mExcel.WriteCell(lSheet, "A" & i, (i - 2).ToString(), CExcel.ExcelLineStyles.xlContinuous, , CExcel.TextHAlign.Center, CExcel.TextVAlign.Center)
                mExcel.WriteCell(lSheet, "B" & i, lRow("DatePersian"), CExcel.ExcelLineStyles.xlContinuous, , CExcel.TextHAlign.Center, CExcel.TextVAlign.Center)
                mExcel.WriteCell(lSheet, "c" & i, lRow("DT"), CExcel.ExcelLineStyles.xlContinuous, , CExcel.TextHAlign.Center, CExcel.TextVAlign.Center)

                CellAddress = "d" & i
                If chkFilterSep.Checked Then
                    mExcel.WriteCell(lSheet, "D" & i, lRow("FeederName"), CExcel.ExcelLineStyles.xlContinuous, , CExcel.TextHAlign.Center, CExcel.TextVAlign.Center)
                    CellAddress = "e" & i
                    If rdbMPFeederPartFilter.Checked Then
                        mExcel.WriteCell(lSheet, "E" & i, lRow("FeederPart"), CExcel.ExcelLineStyles.xlContinuous, True, CExcel.TextHAlign.Center, CExcel.TextVAlign.Center)
                        CellAddress = "f" & i
                    End If
                End If

                For k = 0 To lColumnCount - 1
                    lColName = (k * lHour).ToString("0#") & "_" & IIf(k < lColumnCount - 1, ((k + 1) * lHour).ToString("0#"), "00")
                    mExcel.WriteCell(lSheet, CellAddress, lRow(lColName), CExcel.ExcelLineStyles.xlContinuous, , CExcel.TextHAlign.Center, CExcel.TextVAlign.Center)
                    CellAddress = mExcel.GetNextHCell(CellAddress)
                Next

                mExcel.WriteCell(lSheet, CellAddress, lRow("lCountFeeder"), CExcel.ExcelLineStyles.xlContinuous, , CExcel.TextHAlign.Center, CExcel.TextVAlign.Center)
                If chkFilterSep.Checked Then
                    Dim lSumHeader As String = Microsoft.VisualBasic.Left(CellAddress, 1) & "1"
                    mExcel.WriteCell(lSheet, "D1", "فیدرفشار متوسط", CExcel.ExcelLineStyles.xlContinuous, , CExcel.TextHAlign.Center, CExcel.TextVAlign.Center)
                    If rdbMPFeederPartFilter.Checked Then
                        mExcel.WriteCell(lSheet, "E1", "تکه فیدر ", CExcel.ExcelLineStyles.xlContinuous, , CExcel.TextHAlign.Center, CExcel.TextVAlign.Center)
                    End If
                    mExcel.WriteCell(lSheet, lSumHeader, "جمع فیدرها", CExcel.ExcelLineStyles.xlContinuous, , CExcel.TextHAlign.Center, CExcel.TextVAlign.Center)
                End If

                If i Mod 4 = 0 Then AdvanceProgressBar(4)
                i += 1
            Next
            prgs.Value = prgs.Maximum
            '---------------------------------------
            mExcel.SelectRange(lSheet, "A1")
            mExcel.ShowExcel()

        Catch ex As Exception
            ShowError(ex)
        Finally
            Me.Cursor = Cursors.Default
            mIsReporting = False
        End Try
        lblProgress.Visible = False
        prgs.Visible = False
        If Not mIsReporting Then Exit Sub

    End Sub
    Private Function GetDisMPFeeder(ByVal aDate1 As Date, ByVal aDate2 As Date, ByVal aFromDate As String, ByVal aToDate As String) As Double
        Dim lSQL As String = ""
        Dim lDs As New Bargh_DataSets.DatasetCcRequester
        Dim lCnn As SqlConnection = New SqlConnection(GetConnection())
        Dim lDate1 As String, lDate2 As String
        Dim lFieldName As String = ""
        Dim lJoin As String = ""
        Dim lGroupBy As String = ""
        Dim lWhereFTinMP As String
        lWhereFTinMP = _
            " ( TblMPRequest.DisconnectGroupSetId IN (1129,1130) " & _
            " OR (TblMPRequest.DisconnectReasonId >= 1200 AND TblMPRequest.DisconnectReasonId <= 1299 AND NOT TblMPRequest.DisconnectReasonId IS NULL) ) "

        lDate1 = aDate1.ToShortDateString & " " & aDate1.Hour & ":" & aDate1.Minute
        lDate2 = aDate2.ToShortDateString & " " & aDate2.Hour & ":" & aDate2.Minute

        If chkFilterSep.Checked Then
            lFieldName = " TblMPRequest.MPFeederId,Tbl_MPFeeder.MPFeederName, "
            lJoin = " INNER JOIN  Tbl_MPFeeder  ON   TblMPRequest.MPFeederId=Tbl_MPFeeder.MPFeederId "
            lGroupBy = "TblMPRequest.MPFeederId,Tbl_MPFeeder.MPFeederName, "

            If rdbMPFeederPartFilter.Checked Then
                lFieldName &= "Tbl_FeederPart.FeederPartId,Tbl_FeederPart.FeederPart, "
                lJoin &= " INNER JOIN Tbl_FeederPart on Tbl_MPFeeder.MPFeederId = Tbl_FeederPart.MPFeederId "
                lGroupBy &= "Tbl_FeederPart.FeederPartId,Tbl_FeederPart.FeederPart, "
            End If
        End If

        lSQL &= _
            " SELECT " & _
            " " & lFieldName & " " & _
            " Count(DISTINCT TblMPRequest.MPFeederId) " & _
            "	 AS DisconnectMPFeeder " & _
            " FROM TblRequest" & _
            " Inner JOIN TblMPRequest on TblRequest.MPRequestId=TblMPRequest.MPRequestId " & lJoin & _
            " WHERE " & _
            "	(TblRequest.IsDisconnectMPFeeder = 1) AND (NOT (TblRequest.ConnectDT IS NULL)) " & _
            "	AND TblRequest.DisconnectDatePersian >= '" & aFromDate & "' AND TblRequest.DisconnectDatePersian <= '" & aToDate & "' " & _
                IIf(mIsTamir, "AND IsTamir=1", IIf(mIsNotTamir, "AND IsTamir=0", "")) & _
                IIf(mAreaId > 0, " And TblRequest.AreaId = " & mAreaId, "") & _
                IIf(mAreaIDs <> "", " AND TblRequest.AreaId IN (" & mAreaIDs & ") ", "") & _
            "    AND ((TblRequest.DisconnectDT >= '" & lDate1 & "' AND TblRequest.DisconnectDT <= '" & lDate2 & "') " & _
            "    OR (TblRequest.ConnectDT >= '" & lDate1 & "' AND TblRequest.ConnectDT < ' " & lDate2 & " ') " & _
            "    OR (TblRequest.DisconnectDT < '" & lDate1 & "' AND TblRequest.ConnectDT > '" & lDate2 & "')) " & _
            "  Group by " & lGroupBy & "TblRequest.DisconnectDatePersian " & _
            " ORDER BY  " & _
            "	DisconnectMPFeeder DESC "

        RemoveMoreSpaces(lSQL)
        BindingTable(lSQL, lCnn, lDs, "TblCountFeeder", , , , , , , True)
        Dim lFeeder As Object
        If lDs.Tables("TblCountFeeder").Rows.Count > 0 Then
            lFeeder = lDs.Tables("TblCountFeeder").Rows(0)("DisconnectMPFeeder")
            If rdbMPFeederFilter.Checked Then
                mMPFeederName = lDs.Tables("TblCountFeeder").Rows(0)("MPFeederName")
            ElseIf rdbMPFeederPartFilter.Checked Then
                mMPFeederName = lDs.Tables("TblCountFeeder").Rows(0)("MPFeederName")
                mFeederPartName = lDs.Tables("TblCountFeeder").Rows(0)("FeederPart")
            End If
        End If
        If IsDBNull(lFeeder) Then lFeeder = 0
        Return Math.Round(Convert.ToDouble(lFeeder), 3)
    End Function

    Private Function GetDisMPFeeder_FeederByFeeder(ByVal aDate1 As Date, ByVal aDate2 As Date, ByVal aFromDate As String, ByVal aToDate As String, Optional ByVal aMPFeederId As Integer = -1) As Double
        Dim lSQL As String = ""
        Dim lCnn As SqlConnection = New SqlConnection(GetConnection())
        Dim lDate1 As String, lDate2 As String
        Dim lFieldName As String = ""
        Dim lJoin As String = ""
        Dim lGroupBy As String = ""
        Dim lWhereFTinMP As String
        Dim lWhere As String = ""

        lWhereFTinMP = _
            " ( TblMPRequest.DisconnectGroupSetId IN (1129,1130) " & _
            " OR (TblMPRequest.DisconnectReasonId >= 1200 AND TblMPRequest.DisconnectReasonId <= 1299 AND NOT TblMPRequest.DisconnectReasonId IS NULL) ) "

        lDate1 = aDate1.ToShortDateString & " " & aDate1.Hour & ":" & aDate1.Minute
        lDate2 = aDate2.ToShortDateString & " " & aDate2.Hour & ":" & aDate2.Minute
        If aMPFeederId >= 0 Then
            lWhere = " AND TblMPRequest.MPFeederId = " & aMPFeederId
        End If

        If chkFilterSep.Checked Then
            lJoin = " INNER JOIN  Tbl_MPFeeder  ON   TblMPRequest.MPFeederId=Tbl_MPFeeder.MPFeederId "
            lGroupBy = "TblMPRequest.MPFeederId,Tbl_MPFeeder.MPFeederName "

            If rdbMPFeederPartFilter.Checked Then
                lJoin &= " INNER JOIN Tbl_FeederPart on Tbl_MPFeeder.MPFeederId = Tbl_FeederPart.MPFeederId "
                lGroupBy &= "Tbl_FeederPart.FeederPartId,Tbl_FeederPart.FeederPart, "
            End If
        End If
        lSQL &= _
            " SELECT " & _
            " " & lFieldName & " " & _
            " Count(*) " & _
            "	 AS DisconnectMPFeeder " & _
            " FROM TblRequest" & _
            " Inner JOIN TblMPRequest on TblRequest.MPRequestId=TblMPRequest.MPRequestId " & lJoin & _
            " WHERE " & _
            "	(TblRequest.IsDisconnectMPFeeder = 1)  AND (NOT (TblRequest.ConnectDT IS NULL)) " & lWhere & _
            "	AND TblRequest.DisconnectDatePersian >= '" & aFromDate & "' AND TblRequest.DisconnectDatePersian <= '" & aToDate & "' " & _
                IIf(mIsTamir, "AND IsTamir=1", IIf(mIsNotTamir, "AND IsTamir=0", "")) & _
                IIf(mAreaId > 0, " And TblRequest.AreaId = " & mAreaId, "") & _
                IIf(mAreaIDs <> "", " AND TblRequest.AreaId IN (" & mAreaIDs & ") ", "") & _
            "    AND ((TblRequest.DisconnectDT >= '" & lDate1 & "' AND TblRequest.DisconnectDT <= '" & lDate2 & "') " & _
            "    OR (TblRequest.ConnectDT >= '" & lDate1 & "' AND TblRequest.ConnectDT < ' " & lDate2 & " ') " & _
            "    OR (TblRequest.DisconnectDT < '" & lDate1 & "' AND TblRequest.ConnectDT > '" & lDate2 & "')) "
        RemoveMoreSpaces(lSQL)
        BindingTable(lSQL, lCnn, mDS, "TblCountFeeder", , , , , , , True)
        Dim lFeeder As Object
        If mDS.Tables("TblCountFeeder").Rows.Count > 0 Then
            lFeeder = mDS.Tables("TblCountFeeder").Rows(0)("DisconnectMPFeeder")
        End If
        If IsDBNull(lFeeder) Then lFeeder = 0
        Return Math.Round(Convert.ToDouble(lFeeder), 3)
    End Function

    Private Function GetDisMPFeeder_FeederList(ByVal aFromDate As String, ByVal aToDate As String)
        Dim lSQL As String = ""
        Dim lCnn As SqlConnection = New SqlConnection(GetConnection())
        Dim lDate1 As String, lDate2 As String
        Dim lFieldName As String = ""
        Dim lJoin As String = ""
        Dim lGroupBy As String = ""
        Dim lWhereFTinMP As String
        Dim lWhere As String = ""

        lWhereFTinMP = _
            " ( TblMPRequest.DisconnectGroupSetId IN (1129,1130) " & _
            " OR (TblMPRequest.DisconnectReasonId >= 1200 AND TblMPRequest.DisconnectReasonId <= 1299 AND NOT TblMPRequest.DisconnectReasonId IS NULL) ) "

        If chkFilterSep.Checked Then
            lFieldName = " TblMPRequest.MPFeederId,Tbl_MPFeeder.MPFeederName, "
            lJoin = " INNER JOIN  Tbl_MPFeeder  ON   TblMPRequest.MPFeederId=Tbl_MPFeeder.MPFeederId "
            lGroupBy = "TblMPRequest.MPFeederId,Tbl_MPFeeder.MPFeederName "

            If rdbMPFeederPartFilter.Checked Then
                lFieldName &= "Tbl_FeederPart.FeederPartId,Tbl_FeederPart.FeederPart, "
                lJoin &= " INNER JOIN Tbl_FeederPart on Tbl_MPFeeder.MPFeederId = Tbl_FeederPart.MPFeederId "
                lGroupBy &= ",Tbl_FeederPart.FeederPartId,Tbl_FeederPart.FeederPart "
            End If
        End If
        lSQL &= _
            " SELECT " & _
            " " & lFieldName & " " & _
            " Count(*) " & _
            "	 AS DisconnectMPFeeder " & _
            " FROM TblRequest" & _
            " Inner JOIN TblMPRequest on TblRequest.MPRequestId=TblMPRequest.MPRequestId " & lJoin & _
            " WHERE " & _
            "	(TblRequest.IsDisconnectMPFeeder = 1) AND (NOT (TblRequest.ConnectDT IS NULL)) " & _
            "	AND TblRequest.DisconnectDatePersian >= '" & aFromDate & "' AND TblRequest.DisconnectDatePersian <= '" & aToDate & "' " & _
                 IIf(mIsTamir, "AND IsTamir=1", IIf(mIsNotTamir, "AND IsTamir=0", "")) & _
                 IIf(mAreaId > 0, " And TblRequest.AreaId = " & mAreaId, "") & _
                 IIf(mAreaIDs <> "", " AND TblRequest.AreaId IN (" & mAreaIDs & ") ", "") & _
                 IIf(mMPFeederIds <> "", " And TblMPRequest.MPFeederId IN ( " & mMPFeederIds & ")", "") & _
            "  Group by " & lGroupBy & _
            " ORDER BY  	DisconnectMPFeeder DESC "

        RemoveMoreSpaces(lSQL)
        BindingTable(lSQL, lCnn, mDS, "TblCountFeederList", , , , , , , True)
    End Function
    Private Sub AdvanceProgressBar(Optional ByVal aIncValue As Integer = 1)
        Try
            AdvanceProgress(prgs, aIncValue)
        Catch ex As Exception
        End Try
        Application.DoEvents()
    End Sub
    Private Sub chkFilterSep_CheckedChanged(sender As Object, e As EventArgs) Handles chkFilterSep.CheckedChanged
        Dim lStatus As Boolean = chkFilterSep.Checked
        rdbMPFeederFilter.Enabled = lStatus
        rdbMPFeederPartFilter.Enabled = lStatus
        rdbMPFeederPartFilter.Checked = lStatus
        rdbMPFeederFilter.Checked = lStatus
        chkCmbMPFeeder.Clear()
    End Sub
    Private Sub cboArea_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboArea.SelectedIndexChanged
        If mIsLoading And Not IsNothing(sender) Then Exit Sub
        Dim lSQL As String = ""

        If cboArea.SelectedIndex = -1 Then
            mDS.Tbl_MPPost.Clear()
            mDS.Tbl_MPFeeder.Clear()
            Exit Sub
        End If
        lSQL = "exec spGetMPPosts_v2 " & cboArea.SelectedValue & ",1,'',''"
        mIsLoading = True

        mDS.Tbl_MPPost.Clear()
        BindingTable(lSQL, mCnn, mDS, "Tbl_MPPost")
        mDS.Tbl_MPPost.DefaultView.Sort = "MPPostName"
        cmbMPPost.DataSource = mDS.Tbl_MPPost

        cmbMPPost.SelectedIndex = -1
        cmbMPPost.SelectedIndex = -1
        chkCmbMPFeeder.Clear()
        mDS.Tbl_MPFeeder.Clear()

        mIsLoading = False
    End Sub

    Private Sub cmbMPPost_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbMPPost.SelectedIndexChanged
        If mIsLoading Then Exit Sub
        Dim lSQL As String = ""
        Dim lAreaId As Integer = -1
        Dim lMPPostId As Integer = -1

        If cmbMPPost.SelectedIndex > -1 Then lMPPostId = cmbMPPost.SelectedValue
        If cboArea.SelectedIndex > -1 Then
            lAreaId = cboArea.SelectedValue
        End If

        If lMPPostId = -1 Then
            mDS.Tbl_MPFeeder.Clear()
            chkCmbMPFeeder.Clear()
            Exit Sub
        End If

        lSQL = "exec spGetMPFeeders_v2 " & lAreaId & "," & lMPPostId & ",1"

        mIsLoading = True

        mDS.Tbl_MPFeeder.Clear()
        BindingTable(lSQL, mCnn, mDS, "Tbl_MPFeeder")
        mDS.Tbl_MPFeeder.DefaultView.Sort = "MPFeederName"
        chkCmbMPFeeder.Fill(mDS.Tbl_MPFeeder, "MPFeederName", "MPFeederId")
        mIsLoading = False
    End Sub
    Private Sub rdbMPFeederFilter_CheckedChanged(sender As Object, e As EventArgs) Handles rdbMPFeederFilter.CheckedChanged
        Dim lIsActive As Boolean = rdbMPFeederFilter.Checked
        cmbMPPost.Enabled = lIsActive
        chkCmbMPFeeder.Enabled = lIsActive
        cboArea_SelectedIndexChanged(sender, e)
        chkCmbMPFeeder.Clear()
    End Sub

    Private Class CQC
        Public Shared Function checkFromDate(ByRef parent As frmReportHourByHourpower, Optional aExactTime As Boolean = False) As Boolean
            If parent.txtFromDate.Text = "____/__/__" Then
                MsgBoxF("تاريخ شروع الزامي مي‌باشد", "توجه", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Information, MessageBoxDefaultButton.Button1)
                parent.mErr = True
                Return False
            End If
            parent.mFiltersInfo &= If(aExactTime, " - تاريخ ", " - از تاريخ ") & ConvertReportDate(txtFromDate.Text)
            Return True
        End Function
        Public Shared Function checkToDate(ByRef parent As frmReportHourByHourpower) As Boolean
            If parent.txtToDate.Text = "____/__/__" Then
                MsgBoxF("تاريخ پايان مي‌باشد", "توجه", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Information, MessageBoxDefaultButton.Button1)
                parent.mErr = True
                Return False
            End If
            If parent.txtToDate.MiladiDT < parent.txtFromDate.MiladiDT Then
                MsgBoxF("تاريخ شروع بايد قبل از تاريخ پايان باشد", "توجه", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Information, MessageBoxDefaultButton.Button1)
                parent.mErr = True
                Return False
            End If
            parent.mFiltersInfo &= " - از تاريخ " & ConvertReportDate(parent.txtFromDate.Text)
            Return True
        End Function
        Public Shared Function checkReportType(ByRef parent As frmReportHourByHourpower) As Boolean
            If parent.mReportType = "ExcelReport" And (parent.txtNumber.Text = "" Or Val(parent.txtNumber.Text) <= 0 Or Val(parent.txtNumber.Text) > 24) Then
                MsgBoxF("بازه هاي ساعتي معتبر نمي‌باشد", "توجه", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Information, MessageBoxDefaultButton.Button1)
                parent.mErr = True
                Return False
            End If
        End Function
        Public Shared Sub checkWorkingArea(ByRef parent As frmReportHourByHourpower)
            parent.mAreaId = -1
            If parent.cboArea.Visible Then
                If parent.cboArea.SelectedIndex > -1 Then
                    parent.mAreaId = parent.cboArea.SelectedValue
                    parent.mFiltersInfo &= " - ناحيه " & parent.cboArea.Text
                ElseIf Not IsSetadMode Then
                    parent.mAreaId = WorkingAreaId
                End If
            End If
        End Sub
        Public Shared Sub checkAreaID(ByRef parent As frmReportHourByHourpower)
            parent.mAreaIDs = ""
            If parent.chkArea.Visible Then
                If IsCenter And parent.chkArea.GetDataList = "" Then
                    parent.mAreaIDs = GetLegalAreaIDs(parent.mCnn)
                End If
                If parent.mAreaIDs = "" Then
                    parent.mAreaIDs = parent.chkArea.GetDataList()
                    If parent.mAreaIDs <> "" Then
                        If parent.mAreaIDs.Split(",").Length > 1 Then
                            parent.mFiltersInfo &= " نواحي "
                        Else
                            parent.mFiltersInfo &= " ناحيه "
                        End If
                        parent.mFiltersInfo &= parent.chkArea.GetDataTextList()
                    End If
                End If
                If parent.mAreaIDs = "" Then
                    parent.mAreaIDs = GetNotCenterAreaIDs()
                End If
            End If
        End Sub
        Public Shared Sub checkNetworkType(ByRef parent As frmReportHourByHourpower)
            If parent.cmbNwtworkType.SelectedIndex >= 0 Then
                Select Case parent.cmbNwtworkType.Text
                    Case "فشار ضعيف"
                        parent.mIsLP = True
                        parent.mIsMP = False
                        parent.mIsFT = False
                        parent.mFiltersInfo &= " - شبکه فشار ضعيف"
                    Case "فشار متوسط"
                        parent.mIsMP = True
                        parent.mIsLP = False
                        parent.mIsFT = False
                        parent.mFiltersInfo &= " - شبکه فشار متوسط"
                    Case "فوق توزيع"
                        parent.mIsFT = True
                        parent.mIsLP = False
                        parent.mIsMP = False
                        parent.mFiltersInfo &= " - شبکه فوق توزيع"
                End Select
            End If
        End Sub
        Public Shared Sub checkMPFeeder(ByRef parent As frmReportHourByHourpower)
            If parent.chkCmbMPFeeder.GetDataList <> "" Then
                parent.mMPFeederIds = parent.chkCmbMPFeeder.GetDataList
                parent.mFiltersInfo &= " - " & parent.chkCmbMPFeeder.GetDataList
            ElseIf Not IsSetadMode Then
                parent.mMPFeederIds = ""
            End If
        End Sub
        Public Shared Sub checkReqType(ByRef parent As frmReportHourByHourpower)
            If parent.cmbRequestType.SelectedIndex >= 0 Then
                Select Case parent.cmbRequestType.Text
                    Case "با برنامه"
                        parent.mIsTamir = True
                        parent.mIsNotTamir = False
                        parent.mFiltersInfo &= " - با برنامه"
                    Case "بي برنامه"
                        parent.mIsTamir = False
                        parent.mIsNotTamir = True
                        parent.mFiltersInfo &= " - بي برنامه"
                End Select
            End If
        End Sub
        Public Shared Sub checkGroupSet(ByRef parent As frmReportHourByHourpower)
            If parent.ckmbDCGroupSetGroup.GetDataList.Length > 0 Then
                parent.mDCGroupSetGroupId = parent.ckmbDCGroupSetGroup.GetDataList
                parent.mFiltersInfo &= " - علت قطع" & parent.ckmbDCGroupSetGroup.GetDataTextList
            End If
        End Sub
        Public Shared Sub checkTamirType(ByRef parent As frmReportHourByHourpower)
            If parent.ckmbTamirType.GetDataList.Length > 0 Then
                parent.mTamirTypeId = parent.ckmbTamirType.GetDataList
                parent.mFiltersInfo &= " - نوع موافقت " & parent.ckmbTamirType.GetDataTextList
            End If
        End Sub
    End Class
End Class
