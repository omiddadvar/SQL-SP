Imports System.Text.RegularExpressions
Imports CExcelMng = Bargh_Common.CExcelManager

Public Class frmMakeReports
    Inherits FormBase

#Region "Data Members"
    Private mCnn As SqlConnection = New SqlConnection(GetConnection())
    Private mDs As New DatasetBT, mDsBazdid As New DatasetBazdid
    Dim mExcel As CExcelMng = Nothing

    Private mIsLoading As Boolean = False
    Private mIsFirst As Boolean = True
    Private mFilterInfo As String = ""
    Private mReportFile As String = ""
    Private mReportNo As String = ""
    Private mIsSubReport As Boolean = False
    Private mBazdidType As BazdidTypes = BazdidVariables.BazdidTypes.bzt_None
    Private mIsLightReport As Boolean = False
    Private mIsServiceRep As Boolean = False
    Private mIsShowMasters As Boolean = True

    Private mFF As New CFolmulaFields
    Private mTblReportType As DataTable = Nothing
    Private mReportNameFilter As String = ""

    Private mCheckLists As String = ""
    Private mSubCheckLists As String = ""
    Private mParts As String = ""

    Private mSubFormulas As New SortedList
    Private mOriginalHeight As Integer

    '''''''''''''''''''''''''''''''''''''''''''''''''
    Private mFromDate As String = "''"
    Private mToDate As String = "''"
    Private mFromDateBazdid As String = "''"
    Private mToDateBazdid As String = "''"
    Private mAreaId As String = "''"
    Private mMPPostId As Integer = -1
    Private mMPFeederId As Integer = -1
    Private mLPPostId As Integer = -1
    Private mLPFeederId As Integer = -1
    Private mOwnershipId As Integer = -1
    Private mIsHavaei As Integer = -1
    Private mBazdidMasterIDs As String = ""
    Private mBasketDetailIDs As String = ""
    Private mIsLightFeeder As Integer = -1
    Private mIsActive As Integer = 0
    Private mIsWarmLine As String = ""
    Private mIsWarmLineSP As Int16 = 0
    Private mIsExistMap As Integer = -1
    Private mIsUpdateMap As Integer = -1
    Private mIsOKNetDesign As Integer = -1
    Private mPrs As String = ""
    Private mAddress As String = ""
    Private mMinCheckListCount As Integer = 0
    Private mWorkCommand As String = ""

    Private mControlsVisibleState As Hashtable
    Private mFormHeight As Integer
    Private mChkIsActivePosition As Point
    Private mLPPOstCode As String
    Private mLpFeederLengt As String
    Private mNotService As Integer = 0
    Private mBazdidSpeciality As String = ""
    Private mIsNotCheckList As Integer = 0
    Friend WithEvents lblBazdidSpeciality As System.Windows.Forms.Label
    Friend WithEvents chkBazdidSpeciality As Bargh_Common.ChkCombo
    Friend WithEvents lblServiceNumber As System.Windows.Forms.Label
    Friend WithEvents txtServiceNumber As System.Windows.Forms.TextBox
    Friend WithEvents PnlServiceNumber As System.Windows.Forms.Panel
    Friend WithEvents chkIsNotCheckList As System.Windows.Forms.CheckBox


    Private mWhere As String = ""
    Private mWherePart As String = ""
    Private mWhereDT As String = ""
    Private mWhereDT2 As String = ""
    Private mWhere2 As String = ""
    Private mWherePost As String = ""
    Private mJoinSpecialitySql As String = ""
    Private mMPFeederIDs As String = ""
    Private mHaving As String = ""
    Friend WithEvents chkArea As Bargh_Common.ChkCombo
    Friend WithEvents chkMPFeeder As Bargh_Common.ChkCombo
    Friend WithEvents chkMPPost As Bargh_Common.ChkCombo
    Friend WithEvents chkLPPost As Bargh_Common.ChkCombo
    Friend WithEvents chkLPFeeder As Bargh_Common.ChkCombo
    Friend WithEvents lblBazdidType As System.Windows.Forms.Label
    Friend WithEvents cmbBazdidType As System.Windows.Forms.ComboBox
    Friend WithEvents cmbIsWarmLine As Bargh_Common.ComboBoxPersian
    Friend WithEvents lblIsWarmLine As System.Windows.Forms.Label
    Friend WithEvents pnlIsWarmLine As System.Windows.Forms.Panel
    Friend WithEvents chkSpliteCheckList As System.Windows.Forms.CheckBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend txtDateFromBazdid As Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor
    Friend WithEvents btnDateFromBazdid As Bargh_Common.DateButton
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend txtDateToBazdid As Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor
    Friend WithEvents btnDateToBazdid As Bargh_Common.DateButton
    Friend WithEvents Label14 As Label
    Friend WithEvents txtWorkCommandNo As TextBox
    Private mCheckItemsArray As New ArrayList
#End Region

#Region " Windows Form Designer generated code "

    Public Sub New(Optional ByVal aReportNo As String = "")
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        mOriginalHeight = Me.Height
        mReportNo = aReportNo
        MakeReportTypes()
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
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents btnReturn As System.Windows.Forms.Button
    Friend txtDateFrom As Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor
    Friend WithEvents btnDateFrom As Bargh_Common.DateButton
    Friend txtDateTo As Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor
    Friend WithEvents btnDateTo As Bargh_Common.DateButton
    Friend WithEvents rbHavayi As System.Windows.Forms.RadioButton
    Friend WithEvents rbZamini As System.Windows.Forms.RadioButton
    Friend WithEvents rbBoth As System.Windows.Forms.RadioButton
    Friend WithEvents cmbOwnership As Bargh_Common.ComboBoxPersian
    Friend WithEvents pnlHavayiZamini As System.Windows.Forms.Panel
    Friend WithEvents ckcmbBazdidMaster As Bargh_Common.ChkCombo
    Friend WithEvents ckcmbBasketDetail As Bargh_Common.ChkCombo
    Friend WithEvents btnMakeReport As System.Windows.Forms.Button
    Friend WithEvents cmbReportName As System.Windows.Forms.ComboBox
    Friend WithEvents lblHavayiZamini As System.Windows.Forms.Label
    Friend WithEvents cmbLPPost As Bargh_Common.ComboBoxPersian
    Friend WithEvents lbllPPost As System.Windows.Forms.Label
    Friend WithEvents cmbLPFeeder As Bargh_Common.ComboBoxPersian
    Friend WithEvents lblLPFeeder As System.Windows.Forms.Label
    Friend WithEvents pnlHideHavayiZamini As System.Windows.Forms.Panel
    Friend WithEvents lblDate As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents pnlPriority As System.Windows.Forms.Panel
    Friend WithEvents cklstPriority As System.Windows.Forms.CheckedListBox
    Friend WithEvents btnCheckLists As System.Windows.Forms.Button
    Friend WithEvents btnParts As System.Windows.Forms.Button
    Friend WithEvents pnlMaster As System.Windows.Forms.Panel
    Friend WithEvents pnlBasketDetail As System.Windows.Forms.Panel
    Friend WithEvents chkIsActive As System.Windows.Forms.CheckBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents pnlComplateInfo As System.Windows.Forms.Panel
    Friend WithEvents chkIsUpdateMapYes As System.Windows.Forms.CheckBox
    Friend WithEvents chkIsOKNetDesignYes As System.Windows.Forms.CheckBox
    Friend WithEvents chkIsOKNetDesignNo As System.Windows.Forms.CheckBox
    Friend WithEvents chkIsUpdateMapNo As System.Windows.Forms.CheckBox
    Friend WithEvents chkIsExistMapYes As System.Windows.Forms.CheckBox
    Friend WithEvents chkIsExistMapNo As System.Windows.Forms.CheckBox
    Friend WithEvents pnlIsNoValue As System.Windows.Forms.Panel
    Friend WithEvents chkIsNoValue As System.Windows.Forms.CheckBox
    Friend WithEvents cmbArea As Bargh_Common.ComboBoxPersian
    Friend WithEvents cmbMPFeeder As Bargh_Common.ComboBoxPersian
    Friend WithEvents cmbMPPost As Bargh_Common.ComboBoxPersian
    Friend WithEvents pnlAddress As System.Windows.Forms.Panel
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txtAddress As System.Windows.Forms.TextBox
    Friend WithEvents pnlFeeder As System.Windows.Forms.Panel
    Friend WithEvents chkSeprateBasket As System.Windows.Forms.CheckBox
    Friend WithEvents btnSubCheckList As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents txtMinCheckListCount As Bargh_Common.TextBoxPersian
    Friend WithEvents pnlOtherFilter As Bargh_Common.PanelGroupBox
    Friend WithEvents lnkOtherFilter As System.Windows.Forms.LinkLabel
    Friend WithEvents lblMinCheckListCount As System.Windows.Forms.Label
    Friend WithEvents chkMode2 As System.Windows.Forms.CheckBox
    Friend WithEvents chkIsTafkikMPFeeder As System.Windows.Forms.CheckBox
    Friend WithEvents lblOwnership As System.Windows.Forms.Label
    Friend WithEvents lblFeederPart As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtLPPostCode As System.Windows.Forms.TextBox
    Friend WithEvents txtFeederLength As Bargh_Common.TextBoxPersian
    Friend WithEvents lblFeederLength As System.Windows.Forms.Label
    Friend WithEvents chkNotService As System.Windows.Forms.CheckBox
    Friend WithEvents lblFeederLengthUnit As System.Windows.Forms.Label
    Friend WithEvents chkBazdidName As System.Windows.Forms.CheckBox
    Friend WithEvents btnSerachFeederPart As System.Windows.Forms.Button
    Friend WithEvents txtSearchFeederPart As Bargh_Common.TextBoxPersian
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMakeReports))
        Me.txtDateFrom = New Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor()
        Me.btnDateFrom = New Bargh_Common.DateButton()
        Me.lblDate = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtDateTo = New Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor()
        Me.btnDateTo = New Bargh_Common.DateButton()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.lblFeederPart = New System.Windows.Forms.Label()
        Me.lblHavayiZamini = New System.Windows.Forms.Label()
        Me.rbHavayi = New System.Windows.Forms.RadioButton()
        Me.rbZamini = New System.Windows.Forms.RadioButton()
        Me.rbBoth = New System.Windows.Forms.RadioButton()
        Me.cmbOwnership = New Bargh_Common.ComboBoxPersian()
        Me.lblOwnership = New System.Windows.Forms.Label()
        Me.btnMakeReport = New System.Windows.Forms.Button()
        Me.btnReturn = New System.Windows.Forms.Button()
        Me.cmbReportName = New System.Windows.Forms.ComboBox()
        Me.pnlHavayiZamini = New System.Windows.Forms.Panel()
        Me.cmbLPPost = New Bargh_Common.ComboBoxPersian()
        Me.lbllPPost = New System.Windows.Forms.Label()
        Me.cmbLPFeeder = New Bargh_Common.ComboBoxPersian()
        Me.lblLPFeeder = New System.Windows.Forms.Label()
        Me.pnlHideHavayiZamini = New System.Windows.Forms.Panel()
        Me.pnlPriority = New System.Windows.Forms.Panel()
        Me.cklstPriority = New System.Windows.Forms.CheckedListBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblServiceNumber = New System.Windows.Forms.Label()
        Me.txtServiceNumber = New System.Windows.Forms.TextBox()
        Me.btnCheckLists = New System.Windows.Forms.Button()
        Me.btnParts = New System.Windows.Forms.Button()
        Me.pnlMaster = New System.Windows.Forms.Panel()
        Me.ckcmbBazdidMaster = New Bargh_Common.ChkCombo()
        Me.pnlBasketDetail = New System.Windows.Forms.Panel()
        Me.btnSerachFeederPart = New System.Windows.Forms.Button()
        Me.ckcmbBasketDetail = New Bargh_Common.ChkCombo()
        Me.chkIsActive = New System.Windows.Forms.CheckBox()
        Me.pnlComplateInfo = New System.Windows.Forms.Panel()
        Me.chkIsExistMapYes = New System.Windows.Forms.CheckBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.chkIsUpdateMapYes = New System.Windows.Forms.CheckBox()
        Me.chkIsOKNetDesignYes = New System.Windows.Forms.CheckBox()
        Me.chkIsOKNetDesignNo = New System.Windows.Forms.CheckBox()
        Me.chkIsUpdateMapNo = New System.Windows.Forms.CheckBox()
        Me.chkIsExistMapNo = New System.Windows.Forms.CheckBox()
        Me.pnlIsNoValue = New System.Windows.Forms.Panel()
        Me.chkIsNoValue = New System.Windows.Forms.CheckBox()
        Me.pnlFeeder = New System.Windows.Forms.Panel()
        Me.chkMPFeeder = New Bargh_Common.ChkCombo()
        Me.chkMPPost = New Bargh_Common.ChkCombo()
        Me.chkArea = New Bargh_Common.ChkCombo()
        Me.cmbArea = New Bargh_Common.ComboBoxPersian()
        Me.cmbMPFeeder = New Bargh_Common.ComboBoxPersian()
        Me.cmbMPPost = New Bargh_Common.ComboBoxPersian()
        Me.pnlAddress = New System.Windows.Forms.Panel()
        Me.txtAddress = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.chkSeprateBasket = New System.Windows.Forms.CheckBox()
        Me.btnSubCheckList = New System.Windows.Forms.Button()
        Me.pnlOtherFilter = New Bargh_Common.PanelGroupBox()
        Me.pnlIsWarmLine = New System.Windows.Forms.Panel()
        Me.cmbIsWarmLine = New Bargh_Common.ComboBoxPersian()
        Me.lblIsWarmLine = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.txtWorkCommandNo = New System.Windows.Forms.TextBox()
        Me.btnDateToBazdid = New Bargh_Common.DateButton()
        Me.txtDateToBazdid = New Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtDateFromBazdid = New Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor()
        Me.btnDateFromBazdid = New Bargh_Common.DateButton()
        Me.chkSpliteCheckList = New System.Windows.Forms.CheckBox()
        Me.cmbBazdidType = New System.Windows.Forms.ComboBox()
        Me.lblBazdidType = New System.Windows.Forms.Label()
        Me.chkIsNotCheckList = New System.Windows.Forms.CheckBox()
        Me.txtMinCheckListCount = New Bargh_Common.TextBoxPersian()
        Me.lblMinCheckListCount = New System.Windows.Forms.Label()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.lnkOtherFilter = New System.Windows.Forms.LinkLabel()
        Me.chkMode2 = New System.Windows.Forms.CheckBox()
        Me.chkIsTafkikMPFeeder = New System.Windows.Forms.CheckBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtLPPostCode = New System.Windows.Forms.TextBox()
        Me.lblFeederLength = New System.Windows.Forms.Label()
        Me.txtFeederLength = New Bargh_Common.TextBoxPersian()
        Me.chkNotService = New System.Windows.Forms.CheckBox()
        Me.lblFeederLengthUnit = New System.Windows.Forms.Label()
        Me.chkBazdidName = New System.Windows.Forms.CheckBox()
        Me.txtSearchFeederPart = New Bargh_Common.TextBoxPersian()
        Me.lblBazdidSpeciality = New System.Windows.Forms.Label()
        Me.PnlServiceNumber = New System.Windows.Forms.Panel()
        Me.chkLPFeeder = New Bargh_Common.ChkCombo()
        Me.chkLPPost = New Bargh_Common.ChkCombo()
        Me.chkBazdidSpeciality = New Bargh_Common.ChkCombo()
        Me.pnlHavayiZamini.SuspendLayout()
        Me.pnlPriority.SuspendLayout()
        Me.pnlMaster.SuspendLayout()
        Me.pnlBasketDetail.SuspendLayout()
        Me.pnlComplateInfo.SuspendLayout()
        Me.pnlIsNoValue.SuspendLayout()
        Me.pnlFeeder.SuspendLayout()
        Me.pnlAddress.SuspendLayout()
        Me.pnlOtherFilter.SuspendLayout()
        Me.pnlIsWarmLine.SuspendLayout()
        Me.PnlServiceNumber.SuspendLayout()
        Me.SuspendLayout()
        '
        'HelpMaker
        '
        Me.HelpMaker.HelpNamespace = "ReportsHelp.chm"
        '
        'txtDateFrom
        '
        Me.txtDateFrom.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDateFrom.BackColor = System.Drawing.SystemColors.Window
        Me.txtDateFrom.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDateFrom.IntegerDate = 0
        Me.txtDateFrom.IsShadow = False
        Me.txtDateFrom.IsShowCurrentDate = False
        Me.txtDateFrom.Location = New System.Drawing.Point(164, 16)
        Me.txtDateFrom.MaxLength = 10
        Me.txtDateFrom.MiladiDT = CType(resources.GetObject("txtDateFrom.MiladiDT"), Object)
        Me.txtDateFrom.Name = "txtDateFrom"
        Me.txtDateFrom.ReadOnly = True
        Me.txtDateFrom.ReadOnlyMaskedEdit = False
        Me.txtDateFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDateFrom.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtDateFrom.ShamsiDT = "____/__/__"
        Me.txtDateFrom.Size = New System.Drawing.Size(80, 22)
        Me.txtDateFrom.TabIndex = 0
        Me.txtDateFrom.Text = "____/__/__"
        Me.txtDateFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtDateFrom.TimeMaskedEditorOBJ = Nothing
        '
        'btnDateFrom
        '
        Me.btnDateFrom.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDateFrom.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDateFrom.Location = New System.Drawing.Point(140, 16)
        Me.btnDateFrom.Name = "btnDateFrom"
        Me.btnDateFrom.Size = New System.Drawing.Size(24, 22)
        Me.btnDateFrom.TabIndex = 1
        Me.btnDateFrom.Text = "..."
        '
        'lblDate
        '
        Me.lblDate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDate.AutoSize = True
        Me.lblDate.Font = New System.Drawing.Font("Mitra", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblDate.Location = New System.Drawing.Point(255, 16)
        Me.lblDate.Name = "lblDate"
        Me.lblDate.Size = New System.Drawing.Size(104, 19)
        Me.lblDate.TabIndex = 3
        Me.lblDate.Text = "از تاريخ شروع بازديد"
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Mitra", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Label1.Location = New System.Drawing.Point(124, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(17, 19)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "تا"
        '
        'txtDateTo
        '
        Me.txtDateTo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDateTo.BackColor = System.Drawing.SystemColors.Window
        Me.txtDateTo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDateTo.IntegerDate = 0
        Me.txtDateTo.IsShadow = False
        Me.txtDateTo.IsShowCurrentDate = False
        Me.txtDateTo.Location = New System.Drawing.Point(36, 16)
        Me.txtDateTo.MaxLength = 10
        Me.txtDateTo.MiladiDT = CType(resources.GetObject("txtDateTo.MiladiDT"), Object)
        Me.txtDateTo.Name = "txtDateTo"
        Me.txtDateTo.ReadOnly = True
        Me.txtDateTo.ReadOnlyMaskedEdit = False
        Me.txtDateTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDateTo.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtDateTo.ShamsiDT = "____/__/__"
        Me.txtDateTo.Size = New System.Drawing.Size(80, 22)
        Me.txtDateTo.TabIndex = 2
        Me.txtDateTo.Text = "____/__/__"
        Me.txtDateTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtDateTo.TimeMaskedEditorOBJ = Nothing
        '
        'btnDateTo
        '
        Me.btnDateTo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDateTo.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDateTo.Location = New System.Drawing.Point(12, 16)
        Me.btnDateTo.Name = "btnDateTo"
        Me.btnDateTo.Size = New System.Drawing.Size(24, 22)
        Me.btnDateTo.TabIndex = 3
        Me.btnDateTo.Text = "..."
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Mitra", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label3.Location = New System.Drawing.Point(255, 44)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(61, 19)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "ناحيه مرتبط"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Mitra", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Label4.Location = New System.Drawing.Point(248, 1)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(120, 19)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "تکنسين/کارشناس بازديد"
        '
        'Label5
        '
        Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Mitra", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label5.Location = New System.Drawing.Point(255, 72)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(82, 19)
        Me.Label5.TabIndex = 3
        Me.Label5.Text = "پست فوق توزيع"
        '
        'Label6
        '
        Me.Label6.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Mitra", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label6.Location = New System.Drawing.Point(255, 100)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(84, 19)
        Me.Label6.TabIndex = 3
        Me.Label6.Text = "فيدر فشار متوسط"
        '
        'lblFeederPart
        '
        Me.lblFeederPart.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFeederPart.AutoSize = True
        Me.lblFeederPart.Font = New System.Drawing.Font("Mitra", 9.75!, System.Drawing.FontStyle.Bold)
        Me.lblFeederPart.Location = New System.Drawing.Point(248, 0)
        Me.lblFeederPart.Name = "lblFeederPart"
        Me.lblFeederPart.Size = New System.Drawing.Size(46, 19)
        Me.lblFeederPart.TabIndex = 3
        Me.lblFeederPart.Text = "تکه فيدر"
        '
        'lblHavayiZamini
        '
        Me.lblHavayiZamini.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblHavayiZamini.AutoSize = True
        Me.lblHavayiZamini.Font = New System.Drawing.Font("Mitra", 9.75!, System.Drawing.FontStyle.Bold)
        Me.lblHavayiZamini.Location = New System.Drawing.Point(251, 308)
        Me.lblHavayiZamini.Name = "lblHavayiZamini"
        Me.lblHavayiZamini.Size = New System.Drawing.Size(50, 19)
        Me.lblHavayiZamini.TabIndex = 3
        Me.lblHavayiZamini.Text = "نوع شبکه"
        '
        'rbHavayi
        '
        Me.rbHavayi.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rbHavayi.Font = New System.Drawing.Font("Mitra", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.rbHavayi.Location = New System.Drawing.Point(88, 2)
        Me.rbHavayi.Name = "rbHavayi"
        Me.rbHavayi.Size = New System.Drawing.Size(56, 17)
        Me.rbHavayi.TabIndex = 0
        Me.rbHavayi.Text = "هوايي"
        '
        'rbZamini
        '
        Me.rbZamini.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rbZamini.Font = New System.Drawing.Font("Mitra", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.rbZamini.Location = New System.Drawing.Point(48, 2)
        Me.rbZamini.Name = "rbZamini"
        Me.rbZamini.Size = New System.Drawing.Size(48, 17)
        Me.rbZamini.TabIndex = 1
        Me.rbZamini.Text = "زميني"
        '
        'rbBoth
        '
        Me.rbBoth.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rbBoth.Checked = True
        Me.rbBoth.Font = New System.Drawing.Font("Mitra", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.rbBoth.Location = New System.Drawing.Point(-8, 2)
        Me.rbBoth.Name = "rbBoth"
        Me.rbBoth.Size = New System.Drawing.Size(58, 17)
        Me.rbBoth.TabIndex = 2
        Me.rbBoth.TabStop = True
        Me.rbBoth.Text = "هر دو"
        '
        'cmbOwnership
        '
        Me.cmbOwnership.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbOwnership.BackColor = System.Drawing.Color.White
        Me.cmbOwnership.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOwnership.IsReadOnly = False
        Me.cmbOwnership.Location = New System.Drawing.Point(116, 273)
        Me.cmbOwnership.Name = "cmbOwnership"
        Me.cmbOwnership.Size = New System.Drawing.Size(128, 22)
        Me.cmbOwnership.TabIndex = 11
        '
        'lblOwnership
        '
        Me.lblOwnership.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblOwnership.AutoSize = True
        Me.lblOwnership.Font = New System.Drawing.Font("Mitra", 9.75!, System.Drawing.FontStyle.Bold)
        Me.lblOwnership.Location = New System.Drawing.Point(255, 273)
        Me.lblOwnership.Name = "lblOwnership"
        Me.lblOwnership.Size = New System.Drawing.Size(57, 19)
        Me.lblOwnership.TabIndex = 3
        Me.lblOwnership.Text = "نوع مالکيت"
        '
        'btnMakeReport
        '
        Me.btnMakeReport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnMakeReport.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.btnMakeReport.Image = CType(resources.GetObject("btnMakeReport.Image"), System.Drawing.Image)
        Me.btnMakeReport.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnMakeReport.Location = New System.Drawing.Point(272, 368)
        Me.btnMakeReport.Name = "btnMakeReport"
        Me.btnMakeReport.Size = New System.Drawing.Size(104, 24)
        Me.btnMakeReport.TabIndex = 31
        Me.btnMakeReport.Text = "تهيه گزارش"
        Me.btnMakeReport.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnReturn
        '
        Me.btnReturn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnReturn.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnReturn.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.btnReturn.Image = CType(resources.GetObject("btnReturn.Image"), System.Drawing.Image)
        Me.btnReturn.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnReturn.Location = New System.Drawing.Point(9, 368)
        Me.btnReturn.Name = "btnReturn"
        Me.btnReturn.Size = New System.Drawing.Size(104, 24)
        Me.btnReturn.TabIndex = 32
        Me.btnReturn.Text = "بازگشت"
        Me.btnReturn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbReportName
        '
        Me.cmbReportName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbReportName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbReportName.Location = New System.Drawing.Point(12, 340)
        Me.cmbReportName.Name = "cmbReportName"
        Me.cmbReportName.Size = New System.Drawing.Size(364, 22)
        Me.cmbReportName.TabIndex = 30
        '
        'pnlHavayiZamini
        '
        Me.pnlHavayiZamini.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlHavayiZamini.Controls.Add(Me.rbBoth)
        Me.pnlHavayiZamini.Controls.Add(Me.rbZamini)
        Me.pnlHavayiZamini.Controls.Add(Me.rbHavayi)
        Me.pnlHavayiZamini.Location = New System.Drawing.Point(104, 306)
        Me.pnlHavayiZamini.Name = "pnlHavayiZamini"
        Me.pnlHavayiZamini.Size = New System.Drawing.Size(144, 21)
        Me.pnlHavayiZamini.TabIndex = 14
        '
        'cmbLPPost
        '
        Me.cmbLPPost.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbLPPost.BackColor = System.Drawing.Color.White
        Me.cmbLPPost.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbLPPost.DropDownWidth = 250
        Me.cmbLPPost.IsReadOnly = False
        Me.cmbLPPost.Location = New System.Drawing.Point(152, 128)
        Me.cmbLPPost.Name = "cmbLPPost"
        Me.cmbLPPost.Size = New System.Drawing.Size(160, 22)
        Me.cmbLPPost.TabIndex = 7
        '
        'lbllPPost
        '
        Me.lbllPPost.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbllPPost.AutoSize = True
        Me.lbllPPost.Font = New System.Drawing.Font("Mitra", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lbllPPost.Location = New System.Drawing.Point(312, 130)
        Me.lbllPPost.Name = "lbllPPost"
        Me.lbllPPost.Size = New System.Drawing.Size(62, 19)
        Me.lbllPPost.TabIndex = 3
        Me.lbllPPost.Text = "پست توزيع"
        '
        'cmbLPFeeder
        '
        Me.cmbLPFeeder.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbLPFeeder.BackColor = System.Drawing.Color.White
        Me.cmbLPFeeder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbLPFeeder.IsReadOnly = False
        Me.cmbLPFeeder.Location = New System.Drawing.Point(12, 156)
        Me.cmbLPFeeder.Name = "cmbLPFeeder"
        Me.cmbLPFeeder.Size = New System.Drawing.Size(232, 22)
        Me.cmbLPFeeder.TabIndex = 8
        '
        'lblLPFeeder
        '
        Me.lblLPFeeder.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblLPFeeder.AutoSize = True
        Me.lblLPFeeder.Font = New System.Drawing.Font("Mitra", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblLPFeeder.Location = New System.Drawing.Point(255, 156)
        Me.lblLPFeeder.Name = "lblLPFeeder"
        Me.lblLPFeeder.Size = New System.Drawing.Size(82, 19)
        Me.lblLPFeeder.TabIndex = 3
        Me.lblLPFeeder.Text = "فيدر فشار ضعيف"
        '
        'pnlHideHavayiZamini
        '
        Me.pnlHideHavayiZamini.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlHideHavayiZamini.Location = New System.Drawing.Point(104, 301)
        Me.pnlHideHavayiZamini.Name = "pnlHideHavayiZamini"
        Me.pnlHideHavayiZamini.Size = New System.Drawing.Size(216, 32)
        Me.pnlHideHavayiZamini.TabIndex = 13
        Me.pnlHideHavayiZamini.Visible = False
        '
        'pnlPriority
        '
        Me.pnlPriority.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlPriority.Controls.Add(Me.cklstPriority)
        Me.pnlPriority.Controls.Add(Me.Label2)
        Me.pnlPriority.Location = New System.Drawing.Point(328, 242)
        Me.pnlPriority.Name = "pnlPriority"
        Me.pnlPriority.Size = New System.Drawing.Size(48, 96)
        Me.pnlPriority.TabIndex = 15
        Me.pnlPriority.Visible = False
        '
        'cklstPriority
        '
        Me.cklstPriority.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cklstPriority.Items.AddRange(New Object() {"1", "2", "3", "4"})
        Me.cklstPriority.Location = New System.Drawing.Point(6, 24)
        Me.cklstPriority.Name = "cklstPriority"
        Me.cklstPriority.Size = New System.Drawing.Size(40, 72)
        Me.cklstPriority.TabIndex = 16
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Mitra", 8.0!, System.Drawing.FontStyle.Bold)
        Me.Label2.Location = New System.Drawing.Point(15, 8)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(33, 16)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "اولويت"
        '
        'lblServiceNumber
        '
        Me.lblServiceNumber.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblServiceNumber.BackColor = System.Drawing.Color.Transparent
        Me.lblServiceNumber.Font = New System.Drawing.Font("Mitra", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblServiceNumber.Location = New System.Drawing.Point(-11, 2)
        Me.lblServiceNumber.Name = "lblServiceNumber"
        Me.lblServiceNumber.Size = New System.Drawing.Size(84, 21)
        Me.lblServiceNumber.TabIndex = 39
        Me.lblServiceNumber.Text = "شماره سرويس"
        Me.lblServiceNumber.Visible = False
        '
        'txtServiceNumber
        '
        Me.txtServiceNumber.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtServiceNumber.Location = New System.Drawing.Point(5, 18)
        Me.txtServiceNumber.MaxLength = 50
        Me.txtServiceNumber.Name = "txtServiceNumber"
        Me.txtServiceNumber.Size = New System.Drawing.Size(66, 22)
        Me.txtServiceNumber.TabIndex = 40
        Me.txtServiceNumber.Visible = False
        '
        'btnCheckLists
        '
        Me.btnCheckLists.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCheckLists.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnCheckLists.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.btnCheckLists.Location = New System.Drawing.Point(12, 292)
        Me.btnCheckLists.Name = "btnCheckLists"
        Me.btnCheckLists.Size = New System.Drawing.Size(72, 24)
        Me.btnCheckLists.TabIndex = 16
        Me.btnCheckLists.Text = "عيوب"
        Me.btnCheckLists.Visible = False
        '
        'btnParts
        '
        Me.btnParts.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnParts.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnParts.Location = New System.Drawing.Point(12, 306)
        Me.btnParts.Name = "btnParts"
        Me.btnParts.Size = New System.Drawing.Size(72, 22)
        Me.btnParts.TabIndex = 17
        Me.btnParts.Text = "تجهيزات"
        Me.btnParts.Visible = False
        '
        'pnlMaster
        '
        Me.pnlMaster.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlMaster.BackColor = System.Drawing.Color.Transparent
        Me.pnlMaster.Controls.Add(Me.ckcmbBazdidMaster)
        Me.pnlMaster.Controls.Add(Me.Label4)
        Me.pnlMaster.Location = New System.Drawing.Point(5, 213)
        Me.pnlMaster.Name = "pnlMaster"
        Me.pnlMaster.Size = New System.Drawing.Size(379, 30)
        Me.pnlMaster.TabIndex = 9
        '
        'ckcmbBazdidMaster
        '
        Me.ckcmbBazdidMaster.CheckComboDropDownWidth = 0
        Me.ckcmbBazdidMaster.CheckGroup = CType(resources.GetObject("ckcmbBazdidMaster.CheckGroup"), System.Collections.ArrayList)
        Me.ckcmbBazdidMaster.DropDownDirection = UtilityLibrary.Combos.DropDownDirection.Down
        Me.ckcmbBazdidMaster.DropHeight = 500
        Me.ckcmbBazdidMaster.IsGroup = False
        Me.ckcmbBazdidMaster.IsMultiSelect = True
        Me.ckcmbBazdidMaster.Location = New System.Drawing.Point(7, 1)
        Me.ckcmbBazdidMaster.Name = "ckcmbBazdidMaster"
        Me.ckcmbBazdidMaster.ReadOnlyList = ""
        Me.ckcmbBazdidMaster.Size = New System.Drawing.Size(232, 23)
        Me.ckcmbBazdidMaster.TabIndex = 7
        Me.ckcmbBazdidMaster.Text = "ChkCombo1"
        Me.ckcmbBazdidMaster.TreeImageList = Nothing
        '
        'pnlBasketDetail
        '
        Me.pnlBasketDetail.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlBasketDetail.Controls.Add(Me.btnSerachFeederPart)
        Me.pnlBasketDetail.Controls.Add(Me.ckcmbBasketDetail)
        Me.pnlBasketDetail.Controls.Add(Me.lblFeederPart)
        Me.pnlBasketDetail.Location = New System.Drawing.Point(5, 243)
        Me.pnlBasketDetail.Name = "pnlBasketDetail"
        Me.pnlBasketDetail.Size = New System.Drawing.Size(336, 25)
        Me.pnlBasketDetail.TabIndex = 10
        '
        'btnSerachFeederPart
        '
        Me.btnSerachFeederPart.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSerachFeederPart.Image = CType(resources.GetObject("btnSerachFeederPart.Image"), System.Drawing.Image)
        Me.btnSerachFeederPart.Location = New System.Drawing.Point(5, 2)
        Me.btnSerachFeederPart.Name = "btnSerachFeederPart"
        Me.btnSerachFeederPart.Size = New System.Drawing.Size(20, 20)
        Me.btnSerachFeederPart.TabIndex = 218
        Me.GlobalToolTip.SetToolTip(Me.btnSerachFeederPart, "جستجوي سريع تکه فيدر")
        '
        'ckcmbBasketDetail
        '
        Me.ckcmbBasketDetail.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ckcmbBasketDetail.CheckComboDropDownWidth = 0
        Me.ckcmbBasketDetail.CheckGroup = CType(resources.GetObject("ckcmbBasketDetail.CheckGroup"), System.Collections.ArrayList)
        Me.ckcmbBasketDetail.DropDownDirection = UtilityLibrary.Combos.DropDownDirection.Down
        Me.ckcmbBasketDetail.DropHeight = 500
        Me.ckcmbBasketDetail.IsGroup = False
        Me.ckcmbBasketDetail.IsMultiSelect = True
        Me.ckcmbBasketDetail.Location = New System.Drawing.Point(31, 0)
        Me.ckcmbBasketDetail.Name = "ckcmbBasketDetail"
        Me.ckcmbBasketDetail.ReadOnlyList = ""
        Me.ckcmbBasketDetail.Size = New System.Drawing.Size(208, 23)
        Me.ckcmbBasketDetail.TabIndex = 8
        Me.ckcmbBasketDetail.Text = "ChkCombo1"
        Me.ckcmbBasketDetail.TreeImageList = Nothing
        '
        'chkIsActive
        '
        Me.chkIsActive.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkIsActive.Font = New System.Drawing.Font("Mitra", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.chkIsActive.Location = New System.Drawing.Point(12, 273)
        Me.chkIsActive.Name = "chkIsActive"
        Me.chkIsActive.Size = New System.Drawing.Size(92, 19)
        Me.chkIsActive.TabIndex = 12
        Me.chkIsActive.Text = "حالت شبکه فعال"
        '
        'pnlComplateInfo
        '
        Me.pnlComplateInfo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlComplateInfo.Controls.Add(Me.chkIsExistMapYes)
        Me.pnlComplateInfo.Controls.Add(Me.Label8)
        Me.pnlComplateInfo.Controls.Add(Me.Label10)
        Me.pnlComplateInfo.Controls.Add(Me.Label11)
        Me.pnlComplateInfo.Controls.Add(Me.chkIsUpdateMapYes)
        Me.pnlComplateInfo.Controls.Add(Me.chkIsOKNetDesignYes)
        Me.pnlComplateInfo.Controls.Add(Me.chkIsOKNetDesignNo)
        Me.pnlComplateInfo.Controls.Add(Me.chkIsUpdateMapNo)
        Me.pnlComplateInfo.Controls.Add(Me.chkIsExistMapNo)
        Me.pnlComplateInfo.Location = New System.Drawing.Point(8, 213)
        Me.pnlComplateInfo.Name = "pnlComplateInfo"
        Me.pnlComplateInfo.Size = New System.Drawing.Size(240, 56)
        Me.pnlComplateInfo.TabIndex = 10
        Me.pnlComplateInfo.Visible = False
        '
        'chkIsExistMapYes
        '
        Me.chkIsExistMapYes.Font = New System.Drawing.Font("Mitra", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.chkIsExistMapYes.Location = New System.Drawing.Point(72, -1)
        Me.chkIsExistMapYes.Name = "chkIsExistMapYes"
        Me.chkIsExistMapYes.Size = New System.Drawing.Size(40, 16)
        Me.chkIsExistMapYes.TabIndex = 0
        Me.chkIsExistMapYes.Text = "بله"
        '
        'Label8
        '
        Me.Label8.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Mitra", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label8.Location = New System.Drawing.Point(143, -3)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(86, 16)
        Me.Label8.TabIndex = 3
        Me.Label8.Text = "نقشه شبکه وجود دارد"
        '
        'Label10
        '
        Me.Label10.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Mitra", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label10.Location = New System.Drawing.Point(120, 17)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(104, 16)
        Me.Label10.TabIndex = 3
        Me.Label10.Text = "نقشه شبکه بروز شده است"
        '
        'Label11
        '
        Me.Label11.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Mitra", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label11.Location = New System.Drawing.Point(121, 37)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(104, 16)
        Me.Label11.TabIndex = 3
        Me.Label11.Text = "طراحي شبکه مناسب است"
        '
        'chkIsUpdateMapYes
        '
        Me.chkIsUpdateMapYes.Font = New System.Drawing.Font("Mitra", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.chkIsUpdateMapYes.Location = New System.Drawing.Point(72, 19)
        Me.chkIsUpdateMapYes.Name = "chkIsUpdateMapYes"
        Me.chkIsUpdateMapYes.Size = New System.Drawing.Size(40, 16)
        Me.chkIsUpdateMapYes.TabIndex = 2
        Me.chkIsUpdateMapYes.Text = "بله"
        '
        'chkIsOKNetDesignYes
        '
        Me.chkIsOKNetDesignYes.Font = New System.Drawing.Font("Mitra", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.chkIsOKNetDesignYes.Location = New System.Drawing.Point(72, 39)
        Me.chkIsOKNetDesignYes.Name = "chkIsOKNetDesignYes"
        Me.chkIsOKNetDesignYes.Size = New System.Drawing.Size(40, 16)
        Me.chkIsOKNetDesignYes.TabIndex = 4
        Me.chkIsOKNetDesignYes.Text = "بله"
        '
        'chkIsOKNetDesignNo
        '
        Me.chkIsOKNetDesignNo.Font = New System.Drawing.Font("Mitra", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.chkIsOKNetDesignNo.Location = New System.Drawing.Point(24, 39)
        Me.chkIsOKNetDesignNo.Name = "chkIsOKNetDesignNo"
        Me.chkIsOKNetDesignNo.Size = New System.Drawing.Size(40, 16)
        Me.chkIsOKNetDesignNo.TabIndex = 5
        Me.chkIsOKNetDesignNo.Text = "خير"
        '
        'chkIsUpdateMapNo
        '
        Me.chkIsUpdateMapNo.Font = New System.Drawing.Font("Mitra", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.chkIsUpdateMapNo.Location = New System.Drawing.Point(24, 19)
        Me.chkIsUpdateMapNo.Name = "chkIsUpdateMapNo"
        Me.chkIsUpdateMapNo.Size = New System.Drawing.Size(40, 16)
        Me.chkIsUpdateMapNo.TabIndex = 3
        Me.chkIsUpdateMapNo.Text = "خير"
        '
        'chkIsExistMapNo
        '
        Me.chkIsExistMapNo.Font = New System.Drawing.Font("Mitra", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.chkIsExistMapNo.Location = New System.Drawing.Point(24, -1)
        Me.chkIsExistMapNo.Name = "chkIsExistMapNo"
        Me.chkIsExistMapNo.Size = New System.Drawing.Size(40, 16)
        Me.chkIsExistMapNo.TabIndex = 1
        Me.chkIsExistMapNo.Text = "خير"
        '
        'pnlIsNoValue
        '
        Me.pnlIsNoValue.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlIsNoValue.Controls.Add(Me.chkIsNoValue)
        Me.pnlIsNoValue.Location = New System.Drawing.Point(248, 213)
        Me.pnlIsNoValue.Name = "pnlIsNoValue"
        Me.pnlIsNoValue.Size = New System.Drawing.Size(88, 56)
        Me.pnlIsNoValue.TabIndex = 9
        Me.pnlIsNoValue.Visible = False
        '
        'chkIsNoValue
        '
        Me.chkIsNoValue.Font = New System.Drawing.Font("Mitra", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.chkIsNoValue.Location = New System.Drawing.Point(4, 8)
        Me.chkIsNoValue.Name = "chkIsNoValue"
        Me.chkIsNoValue.Size = New System.Drawing.Size(80, 40)
        Me.chkIsNoValue.TabIndex = 0
        Me.chkIsNoValue.Text = "حداقل يکي از موارد خير باشد"
        '
        'pnlFeeder
        '
        Me.pnlFeeder.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlFeeder.Controls.Add(Me.chkMPFeeder)
        Me.pnlFeeder.Controls.Add(Me.chkMPPost)
        Me.pnlFeeder.Controls.Add(Me.chkArea)
        Me.pnlFeeder.Controls.Add(Me.cmbArea)
        Me.pnlFeeder.Controls.Add(Me.cmbMPFeeder)
        Me.pnlFeeder.Controls.Add(Me.cmbMPPost)
        Me.pnlFeeder.Location = New System.Drawing.Point(112, 42)
        Me.pnlFeeder.Name = "pnlFeeder"
        Me.pnlFeeder.Size = New System.Drawing.Size(136, 84)
        Me.pnlFeeder.TabIndex = 4
        '
        'chkMPFeeder
        '
        Me.chkMPFeeder.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkMPFeeder.CheckComboDropDownWidth = 0
        Me.chkMPFeeder.CheckGroup = CType(resources.GetObject("chkMPFeeder.CheckGroup"), System.Collections.ArrayList)
        Me.chkMPFeeder.DropDownDirection = UtilityLibrary.Combos.DropDownDirection.Down
        Me.chkMPFeeder.DropHeight = 500
        Me.chkMPFeeder.IsGroup = False
        Me.chkMPFeeder.IsMultiSelect = True
        Me.chkMPFeeder.Location = New System.Drawing.Point(4, 59)
        Me.chkMPFeeder.Name = "chkMPFeeder"
        Me.chkMPFeeder.ReadOnlyList = ""
        Me.chkMPFeeder.Size = New System.Drawing.Size(127, 22)
        Me.chkMPFeeder.TabIndex = 3
        Me.chkMPFeeder.Text = "ChkCombo1"
        Me.chkMPFeeder.TreeImageList = Nothing
        '
        'chkMPPost
        '
        Me.chkMPPost.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkMPPost.CheckComboDropDownWidth = 0
        Me.chkMPPost.CheckGroup = CType(resources.GetObject("chkMPPost.CheckGroup"), System.Collections.ArrayList)
        Me.chkMPPost.DropDownDirection = UtilityLibrary.Combos.DropDownDirection.Down
        Me.chkMPPost.DropHeight = 500
        Me.chkMPPost.IsGroup = False
        Me.chkMPPost.IsMultiSelect = True
        Me.chkMPPost.Location = New System.Drawing.Point(4, 31)
        Me.chkMPPost.Name = "chkMPPost"
        Me.chkMPPost.ReadOnlyList = ""
        Me.chkMPPost.Size = New System.Drawing.Size(127, 22)
        Me.chkMPPost.TabIndex = 3
        Me.chkMPPost.Text = "ChkCombo1"
        Me.chkMPPost.TreeImageList = Nothing
        '
        'chkArea
        '
        Me.chkArea.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkArea.CheckComboDropDownWidth = 0
        Me.chkArea.CheckGroup = CType(resources.GetObject("chkArea.CheckGroup"), System.Collections.ArrayList)
        Me.chkArea.DropDownDirection = UtilityLibrary.Combos.DropDownDirection.Down
        Me.chkArea.DropHeight = 500
        Me.chkArea.IsGroup = False
        Me.chkArea.IsMultiSelect = True
        Me.chkArea.Location = New System.Drawing.Point(4, 3)
        Me.chkArea.Name = "chkArea"
        Me.chkArea.ReadOnlyList = ""
        Me.chkArea.Size = New System.Drawing.Size(127, 22)
        Me.chkArea.TabIndex = 3
        Me.chkArea.TreeImageList = Nothing
        '
        'cmbArea
        '
        Me.cmbArea.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbArea.BackColor = System.Drawing.Color.White
        Me.cmbArea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbArea.IsReadOnly = False
        Me.cmbArea.Location = New System.Drawing.Point(4, 3)
        Me.cmbArea.Name = "cmbArea"
        Me.cmbArea.Size = New System.Drawing.Size(127, 22)
        Me.cmbArea.TabIndex = 0
        '
        'cmbMPFeeder
        '
        Me.cmbMPFeeder.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbMPFeeder.BackColor = System.Drawing.Color.White
        Me.cmbMPFeeder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMPFeeder.IsReadOnly = False
        Me.cmbMPFeeder.Location = New System.Drawing.Point(4, 59)
        Me.cmbMPFeeder.Name = "cmbMPFeeder"
        Me.cmbMPFeeder.Size = New System.Drawing.Size(127, 22)
        Me.cmbMPFeeder.TabIndex = 2
        '
        'cmbMPPost
        '
        Me.cmbMPPost.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbMPPost.BackColor = System.Drawing.Color.White
        Me.cmbMPPost.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMPPost.IsReadOnly = False
        Me.cmbMPPost.Location = New System.Drawing.Point(4, 31)
        Me.cmbMPPost.Name = "cmbMPPost"
        Me.cmbMPPost.Size = New System.Drawing.Size(127, 22)
        Me.cmbMPPost.TabIndex = 1
        '
        'pnlAddress
        '
        Me.pnlAddress.Controls.Add(Me.txtAddress)
        Me.pnlAddress.Controls.Add(Me.Label12)
        Me.pnlAddress.Location = New System.Drawing.Point(8, 42)
        Me.pnlAddress.Name = "pnlAddress"
        Me.pnlAddress.Size = New System.Drawing.Size(104, 84)
        Me.pnlAddress.TabIndex = 5
        Me.pnlAddress.Visible = False
        '
        'txtAddress
        '
        Me.txtAddress.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAddress.Location = New System.Drawing.Point(8, 24)
        Me.txtAddress.Multiline = True
        Me.txtAddress.Name = "txtAddress"
        Me.txtAddress.Size = New System.Drawing.Size(92, 56)
        Me.txtAddress.TabIndex = 4
        '
        'Label12
        '
        Me.Label12.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Mitra", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label12.Location = New System.Drawing.Point(64, 2)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(36, 19)
        Me.Label12.TabIndex = 3
        Me.Label12.Text = "آدرس"
        '
        'chkSeprateBasket
        '
        Me.chkSeprateBasket.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkSeprateBasket.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.chkSeprateBasket.Location = New System.Drawing.Point(119, 368)
        Me.chkSeprateBasket.Name = "chkSeprateBasket"
        Me.chkSeprateBasket.Size = New System.Drawing.Size(144, 24)
        Me.chkSeprateBasket.TabIndex = 33
        Me.chkSeprateBasket.Text = "به تفکيک مسير در هر صفحه"
        Me.chkSeprateBasket.Visible = False
        '
        'btnSubCheckList
        '
        Me.btnSubCheckList.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSubCheckList.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnSubCheckList.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.btnSubCheckList.Location = New System.Drawing.Point(12, 316)
        Me.btnSubCheckList.Name = "btnSubCheckList"
        Me.btnSubCheckList.Size = New System.Drawing.Size(72, 24)
        Me.btnSubCheckList.TabIndex = 34
        Me.btnSubCheckList.Text = "ريزخرابي"
        Me.btnSubCheckList.Visible = False
        '
        'pnlOtherFilter
        '
        Me.pnlOtherFilter.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlOtherFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlOtherFilter.CaptionBackColor = System.Drawing.Color.Teal
        Me.pnlOtherFilter.CaptionFont = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.pnlOtherFilter.CaptionForeColor = System.Drawing.Color.White
        Me.pnlOtherFilter.CaptionHeight = 16
        Me.pnlOtherFilter.CaptionText = "ساير فيلترها"
        Me.pnlOtherFilter.Controls.Add(Me.pnlIsWarmLine)
        Me.pnlOtherFilter.Controls.Add(Me.Label14)
        Me.pnlOtherFilter.Controls.Add(Me.txtWorkCommandNo)
        Me.pnlOtherFilter.Controls.Add(Me.btnDateToBazdid)
        Me.pnlOtherFilter.Controls.Add(Me.txtDateToBazdid)
        Me.pnlOtherFilter.Controls.Add(Me.Label13)
        Me.pnlOtherFilter.Controls.Add(Me.Label9)
        Me.pnlOtherFilter.Controls.Add(Me.txtDateFromBazdid)
        Me.pnlOtherFilter.Controls.Add(Me.btnDateFromBazdid)
        Me.pnlOtherFilter.Controls.Add(Me.chkSpliteCheckList)
        Me.pnlOtherFilter.Controls.Add(Me.cmbBazdidType)
        Me.pnlOtherFilter.Controls.Add(Me.lblBazdidType)
        Me.pnlOtherFilter.Controls.Add(Me.chkIsNotCheckList)
        Me.pnlOtherFilter.Controls.Add(Me.txtMinCheckListCount)
        Me.pnlOtherFilter.Controls.Add(Me.lblMinCheckListCount)
        Me.pnlOtherFilter.Controls.Add(Me.btnOK)
        Me.pnlOtherFilter.IsMoveable = False
        Me.pnlOtherFilter.IsWindowMove = False
        Me.pnlOtherFilter.Location = New System.Drawing.Point(12, 203)
        Me.pnlOtherFilter.Name = "pnlOtherFilter"
        Me.pnlOtherFilter.Size = New System.Drawing.Size(368, 159)
        Me.pnlOtherFilter.TabIndex = 35
        Me.pnlOtherFilter.Visible = False
        '
        'pnlIsWarmLine
        '
        Me.pnlIsWarmLine.Controls.Add(Me.cmbIsWarmLine)
        Me.pnlIsWarmLine.Controls.Add(Me.lblIsWarmLine)
        Me.pnlIsWarmLine.Location = New System.Drawing.Point(213, 97)
        Me.pnlIsWarmLine.Name = "pnlIsWarmLine"
        Me.pnlIsWarmLine.Size = New System.Drawing.Size(147, 25)
        Me.pnlIsWarmLine.TabIndex = 6
        Me.pnlIsWarmLine.Visible = False
        '
        'cmbIsWarmLine
        '
        Me.cmbIsWarmLine.BackColor = System.Drawing.Color.White
        Me.cmbIsWarmLine.FormattingEnabled = True
        Me.cmbIsWarmLine.IsReadOnly = False
        Me.cmbIsWarmLine.Items.AddRange(New Object() {"بدون خط گرم", "کار در خط گرم"})
        Me.cmbIsWarmLine.Location = New System.Drawing.Point(4, 3)
        Me.cmbIsWarmLine.Name = "cmbIsWarmLine"
        Me.cmbIsWarmLine.Size = New System.Drawing.Size(94, 22)
        Me.cmbIsWarmLine.TabIndex = 36
        '
        'lblIsWarmLine
        '
        Me.lblIsWarmLine.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblIsWarmLine.AutoSize = True
        Me.lblIsWarmLine.Font = New System.Drawing.Font("Mitra", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblIsWarmLine.Location = New System.Drawing.Point(98, 3)
        Me.lblIsWarmLine.Name = "lblIsWarmLine"
        Me.lblIsWarmLine.Size = New System.Drawing.Size(46, 19)
        Me.lblIsWarmLine.TabIndex = 36
        Me.lblIsWarmLine.Text = "خط گرم"
        '
        'Label14
        '
        Me.Label14.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Mitra", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label14.Location = New System.Drawing.Point(279, 52)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(83, 19)
        Me.Label14.TabIndex = 229
        Me.Label14.Text = "شماره دستور کار"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtWorkCommandNo
        '
        Me.txtWorkCommandNo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtWorkCommandNo.Location = New System.Drawing.Point(141, 51)
        Me.txtWorkCommandNo.MaxLength = 50
        Me.txtWorkCommandNo.Name = "txtWorkCommandNo"
        Me.txtWorkCommandNo.Size = New System.Drawing.Size(136, 22)
        Me.txtWorkCommandNo.TabIndex = 230
        Me.txtWorkCommandNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnDateToBazdid
        '
        Me.btnDateToBazdid.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDateToBazdid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDateToBazdid.Location = New System.Drawing.Point(30, 23)
        Me.btnDateToBazdid.Name = "btnDateToBazdid"
        Me.btnDateToBazdid.Size = New System.Drawing.Size(24, 22)
        Me.btnDateToBazdid.TabIndex = 228
        Me.btnDateToBazdid.Text = "..."
        '
        'txtDateToBazdid
        '
        Me.txtDateToBazdid.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDateToBazdid.BackColor = System.Drawing.SystemColors.Window
        Me.txtDateToBazdid.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDateToBazdid.IntegerDate = 0
        Me.txtDateToBazdid.IsShadow = False
        Me.txtDateToBazdid.IsShowCurrentDate = False
        Me.txtDateToBazdid.Location = New System.Drawing.Point(54, 23)
        Me.txtDateToBazdid.MaxLength = 10
        Me.txtDateToBazdid.MiladiDT = CType(resources.GetObject("txtDateToBazdid.MiladiDT"), Object)
        Me.txtDateToBazdid.Name = "txtDateToBazdid"
        Me.txtDateToBazdid.ReadOnly = True
        Me.txtDateToBazdid.ReadOnlyMaskedEdit = False
        Me.txtDateToBazdid.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDateToBazdid.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtDateToBazdid.ShamsiDT = "____/__/__"
        Me.txtDateToBazdid.Size = New System.Drawing.Size(80, 22)
        Me.txtDateToBazdid.TabIndex = 225
        Me.txtDateToBazdid.Text = "____/__/__"
        Me.txtDateToBazdid.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtDateToBazdid.TimeMaskedEditorOBJ = Nothing
        '
        'Label13
        '
        Me.Label13.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Mitra", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Label13.Location = New System.Drawing.Point(135, 23)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(17, 19)
        Me.Label13.TabIndex = 227
        Me.Label13.Text = "تا"
        '
        'Label9
        '
        Me.Label9.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Mitra", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label9.Location = New System.Drawing.Point(260, 23)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(104, 19)
        Me.Label9.TabIndex = 226
        Me.Label9.Text = "از تاريخ شروع بازديد"
        '
        'txtDateFromBazdid
        '
        Me.txtDateFromBazdid.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDateFromBazdid.BackColor = System.Drawing.SystemColors.Window
        Me.txtDateFromBazdid.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDateFromBazdid.IntegerDate = 0
        Me.txtDateFromBazdid.IsShadow = False
        Me.txtDateFromBazdid.IsShowCurrentDate = False
        Me.txtDateFromBazdid.Location = New System.Drawing.Point(182, 23)
        Me.txtDateFromBazdid.MaxLength = 10
        Me.txtDateFromBazdid.MiladiDT = CType(resources.GetObject("txtDateFromBazdid.MiladiDT"), Object)
        Me.txtDateFromBazdid.Name = "txtDateFromBazdid"
        Me.txtDateFromBazdid.ReadOnly = True
        Me.txtDateFromBazdid.ReadOnlyMaskedEdit = False
        Me.txtDateFromBazdid.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDateFromBazdid.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtDateFromBazdid.ShamsiDT = "____/__/__"
        Me.txtDateFromBazdid.Size = New System.Drawing.Size(80, 22)
        Me.txtDateFromBazdid.TabIndex = 223
        Me.txtDateFromBazdid.Text = "____/__/__"
        Me.txtDateFromBazdid.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtDateFromBazdid.TimeMaskedEditorOBJ = Nothing
        '
        'btnDateFromBazdid
        '
        Me.btnDateFromBazdid.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDateFromBazdid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDateFromBazdid.Location = New System.Drawing.Point(158, 23)
        Me.btnDateFromBazdid.Name = "btnDateFromBazdid"
        Me.btnDateFromBazdid.Size = New System.Drawing.Size(24, 22)
        Me.btnDateFromBazdid.TabIndex = 224
        Me.btnDateFromBazdid.Text = "..."
        '
        'chkSpliteCheckList
        '
        Me.chkSpliteCheckList.AutoSize = True
        Me.chkSpliteCheckList.Font = New System.Drawing.Font("Mitra", 9.75!, System.Drawing.FontStyle.Bold)
        Me.chkSpliteCheckList.Location = New System.Drawing.Point(54, 74)
        Me.chkSpliteCheckList.Name = "chkSpliteCheckList"
        Me.chkSpliteCheckList.Size = New System.Drawing.Size(165, 23)
        Me.chkSpliteCheckList.TabIndex = 36
        Me.chkSpliteCheckList.Text = "شامل تمامی عيوب انتخاب شده"
        Me.chkSpliteCheckList.UseVisualStyleBackColor = True
        Me.chkSpliteCheckList.Visible = False
        '
        'cmbBazdidType
        '
        Me.cmbBazdidType.FormattingEnabled = True
        Me.cmbBazdidType.Items.AddRange(New Object() {"موردی", "دوره ای", "هردو"})
        Me.cmbBazdidType.Location = New System.Drawing.Point(229, 51)
        Me.cmbBazdidType.Name = "cmbBazdidType"
        Me.cmbBazdidType.Size = New System.Drawing.Size(73, 22)
        Me.cmbBazdidType.TabIndex = 35
        Me.cmbBazdidType.Visible = False
        '
        'lblBazdidType
        '
        Me.lblBazdidType.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblBazdidType.AutoSize = True
        Me.lblBazdidType.Font = New System.Drawing.Font("Mitra", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblBazdidType.Location = New System.Drawing.Point(308, 51)
        Me.lblBazdidType.Name = "lblBazdidType"
        Me.lblBazdidType.Size = New System.Drawing.Size(54, 19)
        Me.lblBazdidType.TabIndex = 4
        Me.lblBazdidType.Text = "نوع بازديد"
        Me.lblBazdidType.Visible = False
        '
        'chkIsNotCheckList
        '
        Me.chkIsNotCheckList.AutoSize = True
        Me.chkIsNotCheckList.Font = New System.Drawing.Font("Mitra", 9.75!, System.Drawing.FontStyle.Bold)
        Me.chkIsNotCheckList.Location = New System.Drawing.Point(220, 74)
        Me.chkIsNotCheckList.Name = "chkIsNotCheckList"
        Me.chkIsNotCheckList.Size = New System.Drawing.Size(138, 23)
        Me.chkIsNotCheckList.TabIndex = 33
        Me.chkIsNotCheckList.Text = "فقط مسيرهای بدون عيب"
        Me.chkIsNotCheckList.UseVisualStyleBackColor = True
        '
        'txtMinCheckListCount
        '
        Me.txtMinCheckListCount.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMinCheckListCount.CaptinText = ""
        Me.txtMinCheckListCount.HasCaption = False
        Me.txtMinCheckListCount.IsForceText = False
        Me.txtMinCheckListCount.IsFractional = False
        Me.txtMinCheckListCount.IsIP = False
        Me.txtMinCheckListCount.IsNumberOnly = True
        Me.txtMinCheckListCount.IsYear = False
        Me.txtMinCheckListCount.Location = New System.Drawing.Point(8, 51)
        Me.txtMinCheckListCount.Name = "txtMinCheckListCount"
        Me.txtMinCheckListCount.Size = New System.Drawing.Size(72, 22)
        Me.txtMinCheckListCount.TabIndex = 0
        Me.txtMinCheckListCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtMinCheckListCount.Visible = False
        '
        'lblMinCheckListCount
        '
        Me.lblMinCheckListCount.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMinCheckListCount.AutoSize = True
        Me.lblMinCheckListCount.Font = New System.Drawing.Font("Mitra", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblMinCheckListCount.Location = New System.Drawing.Point(86, 51)
        Me.lblMinCheckListCount.Name = "lblMinCheckListCount"
        Me.lblMinCheckListCount.Size = New System.Drawing.Size(131, 19)
        Me.lblMinCheckListCount.TabIndex = 32
        Me.lblMinCheckListCount.Text = "حداقل تعداد عيب ديده شده"
        Me.lblMinCheckListCount.Visible = False
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnOK.Image = CType(resources.GetObject("btnOK.Image"), System.Drawing.Image)
        Me.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnOK.Location = New System.Drawing.Point(286, 127)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(72, 24)
        Me.btnOK.TabIndex = 1
        Me.btnOK.Text = "تأييد"
        Me.btnOK.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lnkOtherFilter
        '
        Me.lnkOtherFilter.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lnkOtherFilter.AutoSize = True
        Me.lnkOtherFilter.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lnkOtherFilter.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.lnkOtherFilter.Location = New System.Drawing.Point(23, 320)
        Me.lnkOtherFilter.Name = "lnkOtherFilter"
        Me.lnkOtherFilter.Size = New System.Drawing.Size(55, 13)
        Me.lnkOtherFilter.TabIndex = 36
        Me.lnkOtherFilter.TabStop = True
        Me.lnkOtherFilter.Text = "ساير موارد"
        Me.lnkOtherFilter.Visible = False
        '
        'chkMode2
        '
        Me.chkMode2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkMode2.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkMode2.Location = New System.Drawing.Point(208, 368)
        Me.chkMode2.Name = "chkMode2"
        Me.chkMode2.Size = New System.Drawing.Size(57, 24)
        Me.chkMode2.TabIndex = 37
        Me.chkMode2.Text = "نمونه 2"
        Me.chkMode2.Visible = False
        '
        'chkIsTafkikMPFeeder
        '
        Me.chkIsTafkikMPFeeder.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkIsTafkikMPFeeder.Enabled = False
        Me.chkIsTafkikMPFeeder.Font = New System.Drawing.Font("Mitra", 8.0!, System.Drawing.FontStyle.Bold)
        Me.chkIsTafkikMPFeeder.Location = New System.Drawing.Point(120, 368)
        Me.chkIsTafkikMPFeeder.Name = "chkIsTafkikMPFeeder"
        Me.chkIsTafkikMPFeeder.Size = New System.Drawing.Size(80, 24)
        Me.chkIsTafkikMPFeeder.TabIndex = 38
        Me.chkIsTafkikMPFeeder.Text = "به تفکيک فيدر"
        Me.chkIsTafkikMPFeeder.Visible = False
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Mitra", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Label7.Location = New System.Drawing.Point(100, 131)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(48, 16)
        Me.Label7.TabIndex = 39
        Me.Label7.Text = "کدپست"
        '
        'txtLPPostCode
        '
        Me.txtLPPostCode.Location = New System.Drawing.Point(12, 128)
        Me.txtLPPostCode.Name = "txtLPPostCode"
        Me.txtLPPostCode.Size = New System.Drawing.Size(96, 22)
        Me.txtLPPostCode.TabIndex = 40
        '
        'lblFeederLength
        '
        Me.lblFeederLength.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFeederLength.AutoSize = True
        Me.lblFeederLength.Font = New System.Drawing.Font("Mitra", 8.0!, System.Drawing.FontStyle.Bold)
        Me.lblFeederLength.Location = New System.Drawing.Point(80, 311)
        Me.lblFeederLength.Name = "lblFeederLength"
        Me.lblFeederLength.Size = New System.Drawing.Size(68, 16)
        Me.lblFeederLength.TabIndex = 41
        Me.lblFeederLength.Text = "حداقل طول فيدر"
        '
        'txtFeederLength
        '
        Me.txtFeederLength.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFeederLength.CaptinText = ""
        Me.txtFeederLength.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.txtFeederLength.HasCaption = False
        Me.txtFeederLength.IsForceText = False
        Me.txtFeederLength.IsFractional = True
        Me.txtFeederLength.IsIP = False
        Me.txtFeederLength.IsNumberOnly = True
        Me.txtFeederLength.IsYear = False
        Me.txtFeederLength.Location = New System.Drawing.Point(35, 309)
        Me.txtFeederLength.Name = "txtFeederLength"
        Me.txtFeederLength.Size = New System.Drawing.Size(44, 20)
        Me.txtFeederLength.TabIndex = 43
        Me.txtFeederLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'chkNotService
        '
        Me.chkNotService.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkNotService.Font = New System.Drawing.Font("Mitra", 8.0!, System.Drawing.FontStyle.Bold)
        Me.chkNotService.Location = New System.Drawing.Point(8, 317)
        Me.chkNotService.Name = "chkNotService"
        Me.chkNotService.Size = New System.Drawing.Size(80, 24)
        Me.chkNotService.TabIndex = 44
        Me.chkNotService.Text = "واگذار نشده ها"
        Me.chkNotService.Visible = False
        '
        'lblFeederLengthUnit
        '
        Me.lblFeederLengthUnit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFeederLengthUnit.AutoSize = True
        Me.lblFeederLengthUnit.Font = New System.Drawing.Font("Mitra", 7.0!, System.Drawing.FontStyle.Bold)
        Me.lblFeederLengthUnit.Location = New System.Drawing.Point(4, 312)
        Me.lblFeederLengthUnit.Name = "lblFeederLengthUnit"
        Me.lblFeederLengthUnit.Size = New System.Drawing.Size(30, 15)
        Me.lblFeederLengthUnit.TabIndex = 41
        Me.lblFeederLengthUnit.Text = "کيلومتر"
        '
        'chkBazdidName
        '
        Me.chkBazdidName.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkBazdidName.Font = New System.Drawing.Font("Mitra", 8.0!, System.Drawing.FontStyle.Bold)
        Me.chkBazdidName.Location = New System.Drawing.Point(96, 301)
        Me.chkBazdidName.Name = "chkBazdidName"
        Me.chkBazdidName.Size = New System.Drawing.Size(72, 32)
        Me.chkBazdidName.TabIndex = 45
        Me.chkBazdidName.Text = "به تفکيک شماره بازديد"
        Me.chkBazdidName.Visible = False
        '
        'txtSearchFeederPart
        '
        Me.txtSearchFeederPart.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSearchFeederPart.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtSearchFeederPart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSearchFeederPart.CaptinText = ""
        Me.txtSearchFeederPart.HasCaption = False
        Me.txtSearchFeederPart.IsForceText = False
        Me.txtSearchFeederPart.IsFractional = False
        Me.txtSearchFeederPart.IsIP = False
        Me.txtSearchFeederPart.IsNumberOnly = False
        Me.txtSearchFeederPart.IsYear = False
        Me.txtSearchFeederPart.Location = New System.Drawing.Point(10, 221)
        Me.txtSearchFeederPart.Name = "txtSearchFeederPart"
        Me.txtSearchFeederPart.Size = New System.Drawing.Size(182, 22)
        Me.txtSearchFeederPart.TabIndex = 219
        Me.txtSearchFeederPart.Visible = False
        '
        'lblBazdidSpeciality
        '
        Me.lblBazdidSpeciality.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblBazdidSpeciality.AutoSize = True
        Me.lblBazdidSpeciality.Font = New System.Drawing.Font("Mitra", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblBazdidSpeciality.Location = New System.Drawing.Point(255, 183)
        Me.lblBazdidSpeciality.Name = "lblBazdidSpeciality"
        Me.lblBazdidSpeciality.Size = New System.Drawing.Size(97, 19)
        Me.lblBazdidSpeciality.TabIndex = 3
        Me.lblBazdidSpeciality.Text = "بازديدهای تخصصی"
        '
        'PnlServiceNumber
        '
        Me.PnlServiceNumber.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PnlServiceNumber.Controls.Add(Me.txtServiceNumber)
        Me.PnlServiceNumber.Controls.Add(Me.lblServiceNumber)
        Me.PnlServiceNumber.Location = New System.Drawing.Point(307, 291)
        Me.PnlServiceNumber.Name = "PnlServiceNumber"
        Me.PnlServiceNumber.Size = New System.Drawing.Size(74, 43)
        Me.PnlServiceNumber.TabIndex = 1
        Me.PnlServiceNumber.Visible = False
        '
        'chkLPFeeder
        '
        Me.chkLPFeeder.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkLPFeeder.CheckComboDropDownWidth = 0
        Me.chkLPFeeder.CheckGroup = CType(resources.GetObject("chkLPFeeder.CheckGroup"), System.Collections.ArrayList)
        Me.chkLPFeeder.DropDownDirection = UtilityLibrary.Combos.DropDownDirection.Down
        Me.chkLPFeeder.DropHeight = 500
        Me.chkLPFeeder.IsGroup = False
        Me.chkLPFeeder.IsMultiSelect = True
        Me.chkLPFeeder.Location = New System.Drawing.Point(12, 156)
        Me.chkLPFeeder.Name = "chkLPFeeder"
        Me.chkLPFeeder.ReadOnlyList = ""
        Me.chkLPFeeder.Size = New System.Drawing.Size(232, 22)
        Me.chkLPFeeder.TabIndex = 222
        Me.chkLPFeeder.TreeImageList = Nothing
        '
        'chkLPPost
        '
        Me.chkLPPost.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkLPPost.CheckComboDropDownWidth = 0
        Me.chkLPPost.CheckGroup = CType(resources.GetObject("chkLPPost.CheckGroup"), System.Collections.ArrayList)
        Me.chkLPPost.DropDownDirection = UtilityLibrary.Combos.DropDownDirection.Down
        Me.chkLPPost.DropHeight = 500
        Me.chkLPPost.IsGroup = False
        Me.chkLPPost.IsMultiSelect = True
        Me.chkLPPost.Location = New System.Drawing.Point(152, 128)
        Me.chkLPPost.Name = "chkLPPost"
        Me.chkLPPost.ReadOnlyList = ""
        Me.chkLPPost.Size = New System.Drawing.Size(160, 22)
        Me.chkLPPost.TabIndex = 221
        Me.chkLPPost.TreeImageList = Nothing
        '
        'chkBazdidSpeciality
        '
        Me.chkBazdidSpeciality.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkBazdidSpeciality.CheckComboDropDownWidth = 0
        Me.chkBazdidSpeciality.CheckGroup = CType(resources.GetObject("chkBazdidSpeciality.CheckGroup"), System.Collections.ArrayList)
        Me.chkBazdidSpeciality.DropDownDirection = UtilityLibrary.Combos.DropDownDirection.Down
        Me.chkBazdidSpeciality.DropHeight = 500
        Me.chkBazdidSpeciality.IsGroup = False
        Me.chkBazdidSpeciality.IsMultiSelect = True
        Me.chkBazdidSpeciality.Location = New System.Drawing.Point(12, 183)
        Me.chkBazdidSpeciality.Name = "chkBazdidSpeciality"
        Me.chkBazdidSpeciality.ReadOnlyList = ""
        Me.chkBazdidSpeciality.Size = New System.Drawing.Size(232, 23)
        Me.chkBazdidSpeciality.TabIndex = 220
        Me.chkBazdidSpeciality.Text = "ChkCombo1"
        Me.chkBazdidSpeciality.TreeImageList = Nothing
        '
        'frmMakeReports
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(386, 397)
        Me.Controls.Add(Me.chkLPFeeder)
        Me.Controls.Add(Me.chkLPPost)
        Me.Controls.Add(Me.pnlOtherFilter)
        Me.Controls.Add(Me.PnlServiceNumber)
        Me.Controls.Add(Me.chkBazdidSpeciality)
        Me.Controls.Add(Me.txtSearchFeederPart)
        Me.Controls.Add(Me.chkBazdidName)
        Me.Controls.Add(Me.lblFeederLengthUnit)
        Me.Controls.Add(Me.chkNotService)
        Me.Controls.Add(Me.txtFeederLength)
        Me.Controls.Add(Me.lblFeederLength)
        Me.Controls.Add(Me.txtLPPostCode)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.pnlComplateInfo)
        Me.Controls.Add(Me.chkIsTafkikMPFeeder)
        Me.Controls.Add(Me.pnlIsNoValue)
        Me.Controls.Add(Me.chkMode2)
        Me.Controls.Add(Me.lnkOtherFilter)
        Me.Controls.Add(Me.btnSubCheckList)
        Me.Controls.Add(Me.btnCheckLists)
        Me.Controls.Add(Me.pnlFeeder)
        Me.Controls.Add(Me.pnlPriority)
        Me.Controls.Add(Me.cmbReportName)
        Me.Controls.Add(Me.btnMakeReport)
        Me.Controls.Add(Me.btnReturn)
        Me.Controls.Add(Me.lblDate)
        Me.Controls.Add(Me.txtDateFrom)
        Me.Controls.Add(Me.btnDateFrom)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtDateTo)
        Me.Controls.Add(Me.btnDateTo)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.lblHavayiZamini)
        Me.Controls.Add(Me.cmbOwnership)
        Me.Controls.Add(Me.lblOwnership)
        Me.Controls.Add(Me.pnlHavayiZamini)
        Me.Controls.Add(Me.cmbLPPost)
        Me.Controls.Add(Me.lbllPPost)
        Me.Controls.Add(Me.cmbLPFeeder)
        Me.Controls.Add(Me.lblBazdidSpeciality)
        Me.Controls.Add(Me.lblLPFeeder)
        Me.Controls.Add(Me.pnlHideHavayiZamini)
        Me.Controls.Add(Me.chkIsActive)
        Me.Controls.Add(Me.chkSeprateBasket)
        Me.Controls.Add(Me.btnParts)
        Me.Controls.Add(Me.pnlMaster)
        Me.Controls.Add(Me.pnlAddress)
        Me.Controls.Add(Me.pnlBasketDetail)
        Me.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpMaker.SetHelpNavigator(Me, System.Windows.Forms.HelpNavigator.Topic)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmMakeReports"
        Me.HelpMaker.SetShowHelp(Me, True)
        Me.Text = "گزارشات  بازديد خطوط فشار متوسط"
        Me.pnlHavayiZamini.ResumeLayout(False)
        Me.pnlPriority.ResumeLayout(False)
        Me.pnlPriority.PerformLayout()
        Me.pnlMaster.ResumeLayout(False)
        Me.pnlMaster.PerformLayout()
        Me.pnlBasketDetail.ResumeLayout(False)
        Me.pnlBasketDetail.PerformLayout()
        Me.pnlComplateInfo.ResumeLayout(False)
        Me.pnlComplateInfo.PerformLayout()
        Me.pnlIsNoValue.ResumeLayout(False)
        Me.pnlFeeder.ResumeLayout(False)
        Me.pnlAddress.ResumeLayout(False)
        Me.pnlAddress.PerformLayout()
        Me.pnlOtherFilter.ResumeLayout(False)
        Me.pnlOtherFilter.PerformLayout()
        Me.pnlIsWarmLine.ResumeLayout(False)
        Me.pnlIsWarmLine.PerformLayout()
        Me.PnlServiceNumber.ResumeLayout(False)
        Me.PnlServiceNumber.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

#Region "Events"
    Private Sub frmMakeReports_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Init()
        ReadControlsVisibleState(Me.Controls, mControlsVisibleState, "pnlAddress,pnlPriority") ' omidpp
        mFormHeight = Me.Height + 5
        mChkIsActivePosition = chkIsActive.Location
    End Sub
    Private Sub frmMakeReports_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        If Not mIsFirst Then Exit Sub
        mIsFirst = False

        If IsCenter Then
            cmbArea.SelectedIndex = -1
            cmbArea.SelectedIndex = -1
        End If

        cmbMPPost.SelectedIndex = -1
        cmbMPPost.SelectedIndex = -1
        cmbMPFeeder.SelectedIndex = -1
        cmbMPFeeder.SelectedIndex = -1

        cmbLPPost.SelectedIndex = -1
        cmbLPPost.SelectedIndex = -1
        cmbLPFeeder.SelectedIndex = -1
        cmbLPFeeder.SelectedIndex = -1

        cmbReportName.SelectedValue = mReportNo
        cmbReportName.SelectedValue = mReportNo
        cmbReportName_SelectedIndexChanged(sender, e)

        cmbOwnership.SelectedIndex = -1
        cmbOwnership.SelectedIndex = -1

    End Sub
    Private Sub btnMakeReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMakeReport.Click
        Try
            'If mReportFile = ".rpt" Then
            Me.Cursor = Cursors.WaitCursor
            MakeReport()
            'End If
        Catch ex As Exception
            ShowError(ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub
    Private Sub cmbArea_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbArea.SelectedIndexChanged
        If cmbArea.SelectedIndex > -1 Then
            mDs.Tbl_MPFeeder.Clear()
            mDs.Tbl_MPPost.Clear()
            'mDs.Tbl_MPPost.DefaultView.RowFilter = "AreaId = " & cmbArea.SelectedValue
            Dim lSQl As String = "EXEC spGetMPPosts_v2 " & cmbArea.SelectedValue & ",1,'',''"
            BindingTable(lSQl, mCnn, mDs, "Tbl_MPPost", , , , , , , True)

            mDsBazdid.BTbl_BazdidMaster.DefaultView.RowFilter = "AreaId = " & cmbArea.SelectedValue
            ckcmbBazdidMaster.Fill(mDsBazdid.BTbl_BazdidMaster, "Name", "BazdidMasterId", 20)
        Else
            mDs.Tbl_LPFeeder.Clear()
            mDs.Tbl_LPPost.Clear()
            mDs.Tbl_MPFeeder.Clear()
            mDs.Tbl_MPPost.Clear()
            mDsBazdid.BTbl_BazdidMaster.DefaultView.RowFilter = ""
        End If

        cmbMPPost.SelectedIndex = -1
        cmbMPPost.SelectedIndex = -1

        cmbMPFeeder.SelectedIndex = -1
        cmbMPFeeder.SelectedIndex = -1

        BasketRowFilter()
    End Sub
    Private Sub cmbMPPost_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMPPost.SelectedIndexChanged
        If cmbMPPost.SelectedIndex > -1 Then
            mDs.Tbl_LPFeeder.Clear()
            mDs.Tbl_LPPost.Clear()
            mDs.Tbl_MPFeeder.Clear()
            'mDs.Tbl_MPFeeder.DefaultView.RowFilter = "MPPostId = " & cmbMPPost.SelectedValue
            GetMPFeeders(chkArea.GetDataList(), cmbMPPost.SelectedValue)

        Else
            mDs.Tbl_MPFeeder.Clear()
        End If

        cmbMPFeeder.SelectedIndex = -1
        cmbMPFeeder.SelectedIndex = -1

        cmbLPPost.SelectedIndex = -1
        cmbLPPost.SelectedIndex = -1

        cmbLPFeeder.SelectedIndex = -1
        cmbLPFeeder.SelectedIndex = -1

        If ckcmbBasketDetail.Visible Then
            BasketRowFilter()
        End If
    End Sub
    Private Sub cmbMPFeeder_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMPFeeder.SelectedIndexChanged
        If ckcmbBasketDetail.Visible Then
            BasketRowFilter()
        End If
        If Not cmbLPPost.Visible Then Exit Sub
        If cmbMPFeeder.SelectedIndex > -1 Then
            mDs.Tbl_LPFeeder.Clear()
            mDs.Tbl_LPPost.Clear()
            'mDs.Tbl_LPPost.DefaultView.RowFilter = "MPFeederId = " & cmbMPFeeder.SelectedValue
            Dim lSQl As String = "EXEC spGetLPPosts " & cmbArea.SelectedValue & "," & cmbMPFeeder.SelectedValue & ",1"
            BindingTable(lSQl, mCnn, mDs, "Tbl_LPPost", , , , , , , True)

        Else
            mDs.Tbl_LPPost.Clear()
        End If

        cmbLPPost.SelectedIndex = -1
        cmbLPPost.SelectedIndex = -1

        cmbLPFeeder.SelectedIndex = -1
        cmbLPFeeder.SelectedIndex = -1
    End Sub
    Private Sub cmbLPPost_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbLPPost.SelectedIndexChanged
        If Not cmbLPFeeder.Visible Then Exit Sub
        If cmbLPPost.SelectedIndex > -1 Then
            mDs.Tbl_LPFeeder.Clear()
            'mDs.Tbl_LPFeeder.DefaultView.RowFilter = "LPPostId = " & cmbLPPost.SelectedValue '& IIf(mIsLightReport, " AND IsLightFeeder = 1", "")
            If chkArea.Visible Then
                GetLPFeeders(chkArea.GetDataList(), cmbLPPost.SelectedValue())
            Else
                Dim lSQl As String = "EXEC spGetLPFeeders " & cmbArea.SelectedValue & "," & cmbLPPost.SelectedValue & ",1"
                BindingTable(lSQl, mCnn, mDs, "Tbl_LPFeeder", , , , , , , True)
            End If
        Else
            mDs.Tbl_LPFeeder.Clear() '.DefaultView.RowFilter = ""
        End If

        cmbLPFeeder.SelectedIndex = -1
        cmbLPFeeder.SelectedIndex = -1
    End Sub
    Private Sub cmbReportName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbReportName.SelectedIndexChanged
        If mIsLoading Then Exit Sub
        ckcmbBazdidMaster.Enabled = True
        SetControlVisibleState(mControlsVisibleState) 'omidpp

        Dim lId As String = cmbReportName.SelectedValue
        Dim lRows() As DataRow = mTblReportType.Select("ReportTypeId = '" & lId & "'")
        If lRows.Length > 0 Then
            mReportFile = lRows(0)("ReportFile")
            mReportNo = lId
            mIsSubReport = IIf(lRows(0)("IsSubReport") = "1", True, False)
        Else
            mReportFile = ""
            mReportNo = "      "
            mIsSubReport = False
        End If

        If Regex.IsMatch(mReportNo, "1-[34]-6") Or mReportNo = "1-1-7" Then
            pnlBasketDetail.Visible = False
            pnlMaster.Visible = False
            pnlComplateInfo.Visible = True
            pnlIsNoValue.Visible = True
        ElseIf Not Regex.IsMatch(mReportNo, "^[34]") Then
            pnlBasketDetail.Visible = True
            pnlMaster.Visible = True
            pnlComplateInfo.Visible = False
            pnlIsNoValue.Visible = False
        End If

        Dim lIsShowHavayiZamini As Boolean
        lIsShowHavayiZamini = Not (mReportNo = "3-1-1" Or mReportNo = "3-1-2")
        pnlHavayiZamini.Visible = lIsShowHavayiZamini
        lblHavayiZamini.Visible = lIsShowHavayiZamini
        pnlIsWarmLine.Visible = False
        If pnlHavayiZamini.Visible = True Then
            lblHavayiZamini.Visible = True
        Else
            lblHavayiZamini.Visible = False
        End If

        If Regex.IsMatch(mReportNo, "^[34]") Then
            'Me.Height = mOriginalHeight - pnlMaster.Height
            'Me.Height -= pnlMaster.Height
            If mIsShowMasters Then
                Me.Height -= (pnlMaster.Height + pnlBasketDetail.Height)
                Dim lTop As Integer = 0
                If Regex.IsMatch(mReportNo, "[34]-1-[12345]") Then
                    lTop = pnlFeeder.Top + pnlFeeder.Height + 2
                ElseIf Regex.IsMatch(mReportNo, "[34]-2-[12345]") Then
                    lTop = cmbLPPost.Top + cmbLPPost.Height + 5
                ElseIf Regex.IsMatch(mReportNo, "[34]-[34]-[12345]") Then
                    lTop = cmbLPFeeder.Top + cmbLPFeeder.Height + 5
                End If
                lblBazdidSpeciality.Top = lTop
                lblBazdidSpeciality.BringToFront()
                chkBazdidSpeciality.Top = lTop
            End If
            mIsShowMasters = False
            ckcmbBazdidMaster.SelectNone()
            ckcmbBazdidMaster.Enabled = False
            pnlBasketDetail.Visible = False
            pnlMaster.Visible = False
        End If

        If Regex.IsMatch(mReportNo, "^[24]-2-") Then
            'pnlMaster.Enabled = False
            'ckcmbBasketDetail.Enabled = False
            Me.Height = mOriginalHeight - (pnlMaster.Height + pnlBasketDetail.Height + cmbLPFeeder.Height)
            lblBazdidSpeciality.Top = cmbLPPost.Top + cmbLPPost.Height + 5
            chkBazdidSpeciality.Top = cmbLPPost.Top + cmbLPPost.Height + 5
            lblBazdidSpeciality.BringToFront()
            pnlMaster.Hide()
            pnlBasketDetail.Hide()
            If mReportNo = "2-2-2" Then
                cmbLPPost.Enabled = False
                chkLPPost.Enabled = False
                lblDate.Text = "از تاريخ انجام سرويس"
            ElseIf mReportNo = "3-2-1" Then
                lblDate.Text = "از تاريخ شروع بازديد"
            Else
                cmbLPPost.Enabled = True
                chkLPPost.Enabled = True
                lblDate.Text = "از تاريخ شروع سرويس"
            End If
        End If

        If Regex.IsMatch(mReportNo, "^2|^4") And Not Regex.IsMatch(mReportNo, "2-2-2") Then
            lblDate.Text = "از تاريخ شروع سرويس"
            lblIsWarmLine.Visible = True
            '----------omid-------------
            'pnlIsWarmLine.BringToFront()
            pnlIsWarmLine.Visible = True
            lblIsWarmLine.Visible = True
            cmbIsWarmLine.Visible = True
            lnkOtherFilter.Visible = True
            If Regex.IsMatch(mReportNo, "^(2-[1234]-6)$") Then
                lblDate.Text = "از تاريخ شروع بازديد"
            End If
        End If

        If Regex.IsMatch(mReportNo, "^([12]-[1-4]-[34])|([34]-[1-4]-3)|(2-[1234]-[69])$") Or Regex.IsMatch(mReportNo, "2-4-8") Then
            pnlPriority.Visible = True
            btnCheckLists.Visible = True
        Else
            pnlPriority.Visible = False
            btnCheckLists.Visible = False
        End If
        If Regex.IsMatch(mReportNo, "2-[1234]-[349]") Or Regex.IsMatch(mReportNo, "6-2") Or Regex.IsMatch(mReportNo, "4-[1234]-[45]") Or Regex.IsMatch(mReportNo, "2-4-8") Then
            If Regex.IsMatch(mReportNo, "2-2-[349]") Then
                pnlHideHavayiZamini.Visible = True
                pnlHavayiZamini.Visible = True
                lblHavayiZamini.Visible = True
                'pnlIsWarmLine.Top = chkBazdidSpeciality.Top + chkBazdidSpeciality.Height
                cmbOwnership.Top = pnlIsWarmLine.Top + pnlIsWarmLine.Height + 5
                lblOwnership.Top = cmbOwnership.Top
                'pnlIsWarmLine.BringToFront()
                'ElseIf Regex.IsMatch(mReportNo, "^2") Then '------------------omid
                '    pnlIsWarmLine.Top = chkBazdidSpeciality.Top + chkBazdidSpeciality.Height
                '    pnlIsWarmLine.BringToFront()
            Else
                pnlIsWarmLine.Top = cmbOwnership.Top + cmbOwnership.Height + 5
                pnlHideHavayiZamini.Visible = False
                pnlHavayiZamini.Visible = False
                lblHavayiZamini.Visible = False
            End If
            pnlIsWarmLine.Visible = True
            pnlIsWarmLine.BringToFront()
            cmbIsWarmLine.Visible = True
            lblIsWarmLine.Visible = True
        End If
        'If Regex.IsMatch(mReportNo, "2-[1234]-[9]") Or Regex.IsMatch(mReportNo, "2-4-8") Then
        '    pnlIsWarmLine.Visible = False
        'End If
        'دستو کار
        txtWorkCommandNo.Visible = False
        Label14.Visible = False

        If Regex.IsMatch(mReportNo, "-5$") Then
            btnParts.Visible = True
            'test 
            If Regex.IsMatch(mReportNo, "^2-[1-4]-5$") Then
                txtWorkCommandNo.Visible = True
                Label14.Visible = True
                lblServiceNumber.Visible = True
                txtServiceNumber.Visible = True
                PnlServiceNumber.Visible = True
            End If
        Else
            btnParts.Visible = False
        End If

        If mReportNo = "3-1-3" Then
            btnCheckLists.Visible = False
            pnlPriority.Visible = True
        End If
        If mReportNo = "3-2-3" Then
            btnCheckLists.Visible = False
            pnlPriority.Visible = True
        End If
        If mReportNo = "1-2-1" Then
            cmbLPPost.Enabled = False
            chkLPPost.Enabled = False
        Else
            cmbLPPost.Enabled = True
            chkLPPost.Enabled = True
        End If
        If Regex.IsMatch(mReportNo, "[34]-[34]-2") Then
            cmbLPFeeder.Enabled = False
            chkLPFeeder.Enabled = False
            cmbLPPost.Enabled = False
            chkLPPost.Enabled = False
            chkMode2.Visible = True
            chkIsTafkikMPFeeder.Visible = True
        Else
            cmbLPFeeder.Enabled = True
            chkLPFeeder.Enabled = True
            chkMode2.Visible = False
            chkIsTafkikMPFeeder.Visible = False
        End If
        If Regex.IsMatch(mReportNo, "[12]-[1234]-[345]") Or Regex.IsMatch(mReportNo, "2-[1234]-[9]") Or Regex.IsMatch(mReportNo, "2-4-8") Then
            pnlAddress.Visible = True
            pnlFeeder.Size = New Size(136, pnlFeeder.Size.Height)
            pnlFeeder.Location = New Point(112, pnlFeeder.Location.Y)
        Else
            txtAddress.Clear()
            pnlAddress.Visible = False
            pnlFeeder.Size = New Size(240, pnlFeeder.Size.Height)
            pnlFeeder.Location = New Point(8, pnlFeeder.Location.Y)
        End If
        If Regex.IsMatch(mReportNo, "[12]-[1234]-[349]") Then
            ' chkSpliteCheckList.Visible = True
        Else
            chkSpliteCheckList.Visible = False
            chkSpliteCheckList.Checked = False
        End If

        If Regex.IsMatch(mReportNo, "1-[34]-1") Then
            chkSeprateBasket.Visible = True
        Else
            chkSeprateBasket.Visible = False
        End If
        If Regex.IsMatch(mReportNo, "[12]-[1234]-4") Or Regex.IsMatch(mReportNo, "2-[1234]-[89]") Then
            btnSubCheckList.Visible = True
        Else
            btnSubCheckList.Visible = False
        End If
        If Regex.IsMatch(mReportNo, "[1234]-[1234]-3") Or Regex.IsMatch(mReportNo, "1-[12]-2") Or Regex.IsMatch(mReportNo, "1-[34]-1") Then
            lnkOtherFilter.Visible = True
            If Regex.IsMatch(mReportNo, "[24]-[1234]-3") Then
                lblMinCheckListCount.Text = "حداقل تعداد عيب رفع شده"
            End If

            If Regex.IsMatch(mReportNo, "[1234]-[1234]-3") Then
                lblMinCheckListCount.Visible = True
                txtMinCheckListCount.Visible = True
                chkIsNotCheckList.Visible = False
                chkIsNotCheckList.Checked = False
            Else
                lblMinCheckListCount.Visible = False
                txtMinCheckListCount.Visible = False
                chkIsNotCheckList.Visible = True
            End If
        ElseIf Regex.IsMatch(mReportNo, "^(?![24])") Then
            lnkOtherFilter.Visible = False
        End If
        If Regex.IsMatch(mReportNo, "1-[123]-[345]") Or Regex.IsMatch(mReportNo, "3-[1234]-[35]") Then
            cmbBazdidType.Visible = True
            lblBazdidType.Visible = True
            If Regex.IsMatch(mReportNo, "1-[1234]-[45]") Then
                lnkOtherFilter.Top = btnReturn.Top
                lnkOtherFilter.Left = btnReturn.Left + btnReturn.Width + 10
                lnkOtherFilter.Visible = True
            ElseIf Regex.IsMatch(mReportNo, "3-[1234]-[5]") Then
                lnkOtherFilter.Top = cmbReportName.Top + cmbReportName.Height - 50
                lnkOtherFilter.BringToFront()
                lnkOtherFilter.Left = btnParts.Left + btnParts.Width + 10
                lnkOtherFilter.Visible = True
            Else
                lnkOtherFilter.Top = btnReturn.Top
                lnkOtherFilter.Left = btnCheckLists.Left + 10
                lnkOtherFilter.Visible = True
            End If
        ElseIf Regex.IsMatch(mReportNo, "[12]-[1234]-[34]") Or Regex.IsMatch(mReportNo, "2-[1234]-[9]") Then
            lnkOtherFilter.Top = txtAddress.Top + 20
            lnkOtherFilter.Visible = True
        Else
            lnkOtherFilter.Top = btnReturn.Top
        End If
        If mReportNo = "1-2-8" Then
            btnCheckLists.Visible = True
            cmbLPPost.Visible = False
            chkLPPost.Visible = False
            lbllPPost.Visible = False
            cmbLPFeeder.Visible = False
            chkLPFeeder.Visible = False
            pnlMaster.Visible = False
            cmbOwnership.Visible = False
            lblOwnership.Visible = False
            ckcmbBasketDetail.Visible = False
            lblFeederPart.Visible = False
            pnlHavayiZamini.Visible = False
            lblHavayiZamini.Visible = False
            btnSerachFeederPart.Visible = False
            lblBazdidSpeciality.Visible = False
            chkBazdidSpeciality.Visible = False
            Me.Height = 270
            chkIsActive.Top = btnCheckLists.Top
            chkIsActive.BringToFront()
            chkIsActive.Left = btnCheckLists.Left + btnCheckLists.Width + 10
        ElseIf mReportNo.StartsWith("1-2") Then
            Me.Height = mFormHeight
            chkIsActive.Left = btnCheckLists.Left
            chkIsActive.Top = cmbOwnership.Top
            lblBazdidSpeciality.Visible = True
            chkBazdidSpeciality.Visible = True
        End If
        If Not Regex.IsMatch(mReportNo, "^1-[34]-2") Then
            Label7.Visible = False
            txtLPPostCode.Visible = False
            cmbLPPost.Left = cmbLPFeeder.Left
            chkLPPost.Left = chkLPFeeder.Left
            cmbLPPost.Width = cmbLPFeeder.Width
            chkLPPost.Width = chkLPFeeder.Width
            lbllPPost.Left = lblLPFeeder.Left
            cmbLPPost.Refresh()
            pnlHideHavayiZamini.Left = 88
            lblFeederLength.Visible = False
            txtFeederLength.Visible = False
            lblFeederLengthUnit.Visible = False
        Else
            Label7.Visible = True
            txtLPPostCode.Visible = True
            cmbLPPost.Left = 152
            cmbLPPost.Width = 160
            chkLPPost.Left = 152
            chkLPPost.Width = 160
            lbllPPost.Left = 312
            pnlHideHavayiZamini.Left = 160
            lblFeederLength.Visible = True
            txtFeederLength.Visible = True
            lblFeederLengthUnit.Visible = True
        End If
        If Regex.IsMatch(mReportNo, "^2-[1234]-6") Then
            chkNotService.Visible = True
        Else
            chkNotService.Visible = False
        End If
        If Regex.IsMatch(mReportNo, "2-2-[23456]") Then
            pnlHavayiZamini.Visible = True
            lblHavayiZamini.Visible = True
            pnlHavayiZamini.Enabled = True
            pnlHideHavayiZamini.Visible = False
        End If
        If Regex.IsMatch(mReportNo, "2-[1234]-2") Or Regex.IsMatch(mReportNo, "2-1-1") Then
            'pnlIsWarmLine.Visible = False
            pnlHideHavayiZamini.Visible = True
            pnlHavayiZamini.Visible = True
            lblHavayiZamini.Visible = True
            lblHavayiZamini.BringToFront()
            pnlHavayiZamini.BringToFront()
            pnlHavayiZamini.Enabled = True
        End If
        If Regex.IsMatch(mReportNo, "2-[1234]") Then
            txtDateFromBazdid.Visible = True
            txtDateToBazdid.Visible = True
            Label9.Visible = True
            Label13.Visible = True
            btnDateFromBazdid.Visible = True
            btnDateToBazdid.Visible = True
            lnkOtherFilter.BringToFront()
            lnkOtherFilter.Left = btnReturn.Left + btnReturn.Width + 10
            lnkOtherFilter.Top = btnReturn.Top

            lnkOtherFilter.Visible = True
        Else
            txtDateFromBazdid.Visible = False
            txtDateToBazdid.Visible = False
            Label9.Visible = False
            Label13.Visible = False
            btnDateFromBazdid.Visible = False
            btnDateToBazdid.Visible = False
        End If
        If Regex.IsMatch(mReportNo, "[34]") Then
            Label4.Text = "تکنسين بازديد"
            pnlMaster.Visible = True
            ckcmbBazdidMaster.Enabled = True
            pnlMaster.Top = cmbOwnership.Top + cmbOwnership.Height + 5
            pnlMaster.Width = cmbOwnership.Width + Label4.Width + 20
            pnlMaster.Left = cmbOwnership.Left - 5

            ckcmbBazdidMaster.Width = pnlMaster.Width - Label4.Width - 20
            Label4.Left = ckcmbBazdidMaster.Left + ckcmbBazdidMaster.Width
            '---------------------------omid----------
            If Regex.IsMatch(mReportNo, "^(?!.*(4-1-3|4-[34]-2))") Then
                lnkOtherFilter.Top = btnReturn.Top
                'btnParts.Top - btnParts.Height + 5
                lnkOtherFilter.Left = btnReturn.Left + btnReturn.Width
            Else
                lnkOtherFilter.Top = btnCheckLists.Top + btnCheckLists.Height + 5
                lnkOtherFilter.Left = btnCheckLists.Left
            End If
            pnlMaster.BringToFront()
            ckcmbBazdidMaster.BringToFront()
            btnParts.BringToFront()
        End If
        pnlOtherFilter.Visible = False
        txtMinCheckListCount.Text = ""
        pnlFeeder.Refresh()
    End Sub
    Private Sub btnCheckLists_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCheckLists.Click
        SelectCheckLists()
    End Sub
    Private Sub btnParts_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnParts.Click
        SelectParts()
    End Sub
    Private Sub chkIsNoValue_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIsNoValue.CheckedChanged
        pnlComplateInfo.Enabled = Not chkIsNoValue.Checked
    End Sub
    Private Sub btnSubCheckList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSubCheckList.Click
        SelectSubCheckList()
    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        pnlOtherFilter.Visible = False
    End Sub
    Private Sub lnkOtherFilter_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkOtherFilter.LinkClicked
        pnlOtherFilter.Visible = True
        btnOK.Visible = True
        pnlOtherFilter.Focus()
        pnlOtherFilter.BringToFront()
        '----------omid----------
        If Regex.IsMatch(mReportNo, "^2|^4") Then
            pnlMaster.Visible = False
            pnlIsWarmLine.Visible = True
            'pnlIsWarmLine.BringToFront()
        End If
    End Sub
    Private Sub chkMode2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMode2.CheckedChanged
        chkIsTafkikMPFeeder.Enabled = chkMode2.Checked
        If Not chkMode2.Checked Then
            chkIsTafkikMPFeeder.Checked = False
        End If
    End Sub
    Private Sub chkNotService_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkNotService.CheckedChanged
        If chkNotService.Checked Then
            chkBazdidName.Visible = True
            chkBazdidName.BringToFront()
        Else
            chkBazdidName.Visible = False
        End If
    End Sub
    Private Sub btnSerachFeederPart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSerachFeederPart.Click
        If txtSearchFeederPart.Visible Then
            txtSearchFeederPart.Visible = False
        Else
            txtSearchFeederPart.Visible = True
            txtSearchFeederPart.Focus()
        End If
    End Sub
    Private Sub txtSearchFeederPart_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSearchFeederPart.KeyPress
        If e.KeyChar = Convert.ToChar(Keys.Enter) Then
            txtSearchFeederPart.Visible = False
            ckcmbBasketDetail.Focus()
        End If
    End Sub
    Private Sub txtSearchFeederPart_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSearchFeederPart.TextChanged
        BasketRowFilter()
        Dim lValues As String = ""
        For Each lStr As String In mCheckItemsArray
            lValues &= IIf(lValues.Length > 0, "," & lStr, lStr)
        Next
        ckcmbBasketDetail.SetData(lValues)
    End Sub
    Private Sub ckcmbBasketDetail_AfterCheck(ByVal sender As Object, ByVal e As Bargh_Common.chkComboEventArgs) Handles ckcmbBasketDetail.AfterCheck
        If mCheckItemsArray.Contains(e.SelectedID) Then
            If Not e.IsCheck Then
                mCheckItemsArray.Remove(e.SelectedID)
            End If
        Else
            If e.IsCheck Then
                mCheckItemsArray.Add(e.SelectedID)
            End If
        End If
    End Sub
    Private Sub chkArea_AfterCheck(sender As Object, e As chkComboEventArgs) Handles chkArea.AfterCheck

        If chkArea.GetDataList().Length > 0 Then
            mDs.Tbl_MPFeeder.Clear()
            mDs.Tbl_MPPost.Clear()

            GetMPPosts(chkArea.GetDataList())

            mDsBazdid.BTbl_BazdidMaster.DefaultView.RowFilter = "AreaId IN (" & chkArea.GetDataList() & ")"
            ckcmbBazdidMaster.Fill(mDsBazdid.BTbl_BazdidMaster, "Name", "BazdidMasterId", 20)
        Else

            mDs.Tbl_LPFeeder.Clear()
            mDs.Tbl_LPPost.Clear()
            mDs.Tbl_MPFeeder.Clear()
            mDs.Tbl_MPPost.Clear()
            chkMPPost.Fill(mDs.Tables("Tbl_MPPost"), "MPPostName", "MPPostId", 10)
            mDsBazdid.BTbl_BazdidMaster.DefaultView.RowFilter = ""
        End If

        chkMPPost.UnCheckAll()
        chkMPFeeder.UnCheckAll()
        cmbMPPost.SelectedIndex = -1
        cmbMPFeeder.SelectedIndex = -1
        BasketRowFilterCheckCombo()
        BasketRowFilter()
    End Sub
    Private Sub chkMPPost_AfterCheck(sender As Object, e As chkComboEventArgs) Handles chkMPPost.AfterCheck
        If chkMPPost.GetDataList.Length > 0 Then
            mDs.Tbl_LPFeeder.Clear()
            mDs.Tbl_LPPost.Clear()
            mDs.Tbl_MPFeeder.Clear()

            GetMPFeeders(chkArea.GetDataList(), chkMPPost.GetDataList())
        Else
            mDs.Tbl_MPFeeder.Clear()
            chkMPFeeder.Fill(mDs.Tables("Tbl_MPFeeder"), "MPFeederName", "MPFeederId", 10)
        End If
        chkMPFeeder.UnCheckAll()

        If ckcmbBasketDetail.Visible Then
            BasketRowFilterCheckCombo()
        End If
    End Sub
    Private Sub chkMPFeeder_AfterCheck(sender As Object, e As chkComboEventArgs) Handles chkMPFeeder.AfterCheck
        If ckcmbBasketDetail.Visible Then
            BasketRowFilterCheckCombo()
        End If
        If chkMPFeeder.GetDataList().Length > 0 Then
            mDs.Tbl_LPFeeder.Clear()
            mDs.Tbl_LPPost.Clear()
            GetLPPosts(chkArea.GetDataList(), chkMPFeeder.GetDataList())

        Else
            mDs.Tbl_LPPost.Clear()
            chkLPPost.Fill(mDs.Tables("Tbl_LPPost"), "LPPostName", "LPPostId", 10)
        End If

        chkLPPost.UnCheckAll()
        chkLPFeeder.UnCheckAll()
        cmbLPPost.SelectedIndex = -1
    End Sub
    Private Sub chkLPPost_AfterCheck(sender As Object, e As chkComboEventArgs) Handles chkLPPost.AfterCheck
        If Not chkLPFeeder.Visible Then Exit Sub
        If chkLPPost.GetDataList().Length > 0 Then
            mDs.Tbl_LPFeeder.Clear()
            GetLPFeeders(chkArea.GetDataList(), chkLPPost.GetDataList())
        Else
            mDs.Tbl_LPFeeder.Clear() '.DefaultView.RowFilter = ""
            chkLPFeeder.Fill(mDs.Tables("Tbl_LPFeeder"), "LPFeederName", "LPFeederId", 10)
        End If

        chkLPFeeder.UnCheckAll()
    End Sub
#End Region

#Region "Methods"

    Private Sub Init()
        Dim lSQL As String = ""
        mIsLightReport = False

        'if mReportNo.

        cmbArea.DataSource = mDs.Tbl_Area
        cmbArea.DisplayMember = "Area"
        cmbArea.ValueMember = "AreaId"

        cmbMPPost.DataSource = mDs.Tbl_MPPost
        cmbMPPost.DisplayMember = "MPPostName"
        cmbMPPost.ValueMember = "MPPostId"

        cmbMPFeeder.DataSource = mDs.Tbl_MPFeeder
        cmbMPFeeder.DisplayMember = "MPFeederName"
        cmbMPFeeder.ValueMember = "MPFeederId"

        cmbLPPost.DataSource = mDs.Tbl_LPPost
        cmbLPPost.DisplayMember = "LPPostName"
        cmbLPPost.ValueMember = "LPPostId"

        cmbLPFeeder.DataSource = mDs.Tbl_LPFeeder
        cmbLPFeeder.DisplayMember = "LPFeederName"
        cmbLPFeeder.ValueMember = "LPFeederId"

        cmbOwnership.DataSource = mDs.Tbl_Ownership
        cmbOwnership.DisplayMember = "Ownership"
        cmbOwnership.ValueMember = "OwnershipId"



        'BindingTable("SELECT * FROM Tbl_MPPost WHERE IsActive = 1", mCnn, mDs, "Tbl_MPPost", , , , True)
        'BindingTable("SELECT * FROM Tbl_MPFeeder WHERE IsActive = 1", mCnn, mDs, "Tbl_MPFeeder", , , , True)
        'BindingTable("SELECT * FROM Tbl_LPPost WHERE IsActive = 1", mCnn, mDs, "Tbl_LPPost", , , , True)
        BindingTable("SELECT * FROM Tbl_Ownership", mCnn, mDs, "Tbl_Ownership", , , , True)
        BindingTable("SELECT * FROM BTbl_BazdidMaster", mCnn, mDsBazdid, "BTbl_BazdidMaster", , , , True)
        BindingTable("SELECT * FROM BTbl_BazdidSpeciality", mCnn, mDs, "BTbl_BazdidSpeciality")
        chkBazdidSpeciality.Fill(mDs.Tables("BTbl_BazdidSpeciality"), "SpecialityName", "BazdidSpecialityId")

        ckcmbBazdidMaster.Fill(mDsBazdid.BTbl_BazdidMaster, "Name", "BazdidMasterId", 50)

        lSQL =
            " SELECT DISTINCT " &
            "	tBR.BazdidBasketDetailId, tBR.AreaId, Tbl_MPFeeder.MPPostId, tBR.MPFeederId, " &
            "	BasketDetailName = " &
            "		tAr.Area + ' - ' + CASE WHEN not tBBD.FeederPartId IS NULL THEN '' ELSE 'از ' END + " &
            "		ISNULL(ISNULL(tBBD.FromPathTYpeValue,tBR.FromPathTYpeValue),N'ابتداي خط') + " &
            "		CASE WHEN not tBBD.FeederPartId IS NULL THEN '' ELSE ' تا ' END + " &
            "		ISNULL(ISNULL(tBBD.ToPathTypeValue,tBR.ToPathTypeId),N'انتهاي خط') " &
            "FROM BTblBazdidResult tBR " &
            "	LEFT OUTER JOIN Tbl_Area tAr ON tBR.AreaId = tAr.AreaId " &
            "   LEFT JOIN Tbl_MPFeeder ON tBR.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "	LEFT OUTER JOIN BTblBazdidBasketDetail tBBD ON tBR.BazdidBasketDetailId = tBBD.BazdidBasketDetailId " &
            "WHERE " &
            "	(NOT ISNULL(tBBD.FromPathTYpeValue,tBR.FromPathTYpeValue) IS NULL " &
            "	OR NOT ISNULL(tBBD.ToPathTypeValue,tBR.ToPathTypeId) IS NULL) " &
            "   AND NOT tBR.BazdidBasketDetailId Is Null " &
            "ORDER BY " &
            "	tBR.AreaId "
        RemoveMoreSpaces(lSQL)
        BindingTable(lSQL, mCnn, mDsBazdid, "View_BasketDetail", , , , True)
        ckcmbBasketDetail.Fill(mDsBazdid.Tables("View_BasketDetail").DefaultView, "BasketDetailName", "BazdidBasketDetailId", 20)

        LoadAreaDataTable(mCnn, mDs)
        chkArea.Fill(mDs.Tables("Tbl_Area"), "Area", "AreaId", 10)

        If IsCenter Then
            chkArea.Enabled = True
        Else
            chkArea.SelectNone()
            chkArea.SetData(WorkingAreaId.ToString())
            chkArea.Enabled = False
        End If

        chkArea_AfterCheck(Nothing, Nothing)

        Try

            Dim lFilter As String = ""
            Dim lFilterArray() As String = mReportNo.Split("-")
            Dim lReportSection As String = ""

            If lFilterArray.Length > 2 Then
                lFilter = lFilterArray(0) & "-" & lFilterArray(1)
                lReportSection = lFilterArray(1)
                If lReportSection = "1" Then
                    mBazdidType = BazdidVariables.BazdidTypes.bzt_MPFeeder
                ElseIf lReportSection = "2" Then
                    mBazdidType = BazdidVariables.BazdidTypes.bzt_LPPost
                ElseIf lReportSection = "3" Or lReportSection = "4" Then
                    mBazdidType = BazdidVariables.BazdidTypes.bzt_LPFeeder
                    If lReportSection = "4" Then
                        mIsLightReport = True
                    End If
                ElseIf lReportSection = "5" Then
                    mBazdidType = BazdidVariables.BazdidTypes.bzt_None
                End If
            End If

            BindingTable(
                "SELECT ServicePartId, ServicePartName, ServicePartCode FROM BTbl_ServicePart " &
                "WHERE BazdidTypeId IS NULL OR BazdidTypeId = " & mBazdidType,
                mCnn, mDsBazdid, "Tbl_ServicePart")



            If lFilterArray.Length > 0 Then
                mIsServiceRep = (lFilterArray(0) = "2") Or (lFilterArray(0) = "4")
                If mIsServiceRep Then
                    lblDate.Text = "از تاريخ شروع سرويس"
                End If
            End If

            If lFilter <> "" Then
                mTblReportType.DefaultView.RowFilter = "ReportTypeId LIKE '" & lFilter & "-%'"
            End If

            If mReportFile Is Nothing OrElse mReportFile = "" Then
                Me.Text = "گزارش"
            End If

            If mBazdidType = BazdidVariables.BazdidTypes.bzt_MPFeeder Then
                cmbLPPost.Visible = False
                chkLPPost.Visible = False
                lbllPPost.Visible = False
                cmbLPFeeder.Visible = False
                chkLPFeeder.Visible = False
                lblLPFeeder.Visible = False
                Me.Height -= 2 * 28
            ElseIf mBazdidType = BazdidVariables.BazdidTypes.bzt_LPPost Then
                cmbLPFeeder.Visible = False
                chkLPFeeder.Visible = False
                lblLPFeeder.Visible = False
                Me.Height -= 1 * 28
            ElseIf mBazdidType = BazdidVariables.BazdidTypes.bzt_LPFeeder Then
                'BindingTable("SELECT * FROM Tbl_LPFeeder WHERE IsActive = 1 AND IsLightFeeder = " & IIf(mIsLightReport, "1", "0"), mCnn, mDs, "Tbl_LPFeeder")
                If mIsLightReport Then
                    lblLPFeeder.Text = "فيدر معابر"
                End If
            Else
                'BindingTable("SELECT * FROM Tbl_LPFeeder WHERE IsActive = 1", mCnn, mDs, "Tbl_LPFeeder")
            End If

            If (mIsServiceRep) Or Regex.IsMatch(mReportNo, "^3-") Then
                pnlHavayiZamini.Enabled = False
                pnlHideHavayiZamini.Visible = True
                pnlHideHavayiZamini.BringToFront()
            End If

            If Regex.IsMatch(mReportNo, "2-[1234]-2") Or Regex.IsMatch(mReportNo, "2-1-1") Then
                pnlHideHavayiZamini.Visible = True
                pnlHavayiZamini.Visible = True
                lblHavayiZamini.Visible = True

                pnlHavayiZamini.Enabled = True
                pnlHideHavayiZamini.BringToFront()
                pnlHavayiZamini.BringToFront()
            End If
            If Regex.IsMatch(mReportNo, "2-[1234]") Then
                txtDateFromBazdid.Visible = True
                txtDateToBazdid.Visible = True
                Label9.Visible = True
                Label13.Visible = True
                btnDateFromBazdid.Visible = True
                btnDateToBazdid.Visible = True
                lnkOtherFilter.BringToFront()
                lnkOtherFilter.Left = btnReturn.Left + btnReturn.Width + 10
                lnkOtherFilter.Top = btnReturn.Top
            Else
                txtDateFromBazdid.Visible = False
                txtDateToBazdid.Visible = False
                Label9.Visible = False
                Label13.Visible = False
                btnDateFromBazdid.Visible = False
                btnDateToBazdid.Visible = False
            End If
            BindingTable("SELECT BTbl_BazdidCheckList.BazdidCheckListId, (BTbl_BazdidCheckListGroup.BazdidCheckListGroupName + ' - ' + BTbl_BazdidCheckList.CheckListName) As CheckListName, BTbl_BazdidCheckList.CheckListCode FROM BTbl_BazdidCheckList INNER JOIN BTbl_BazdidCheckListGroup ON BTbl_BazdidCheckList.BazdidCheckListGroupId = BTbl_BazdidCheckListGroup.BazdidCheckListGroupId WHERE BTbl_BazdidCheckListGroup.BazdidTypeId = " & mBazdidType, mCnn, mDsBazdid, "Tbl_CheckList", , , , , , , True)

            lSQL =
                "SELECT  " &
                "	BTbl_SubCheckList.SubCheckListId, " &
                "	BTbl_BazdidCheckList.CheckListName + ' - ' + BTbl_SubCheckList.SubCheckListName AS SubCheckListName, " &
                "	BTbl_SubCheckList.SubCheckListCode " &
                "FROM  " &
                "	BTbl_SubCheckList " &
                "	INNER JOIN BTbl_BazdidCheckList ON BTbl_SubCheckList.BazdidCheckListId = BTbl_BazdidCheckList.BazdidCheckListId " &
                "WHERE " &
                "	BTbl_BazdidCheckList.BazdidTypeId = " & mBazdidType &
                "ORDER BY BTbl_SubCheckList.SubCheckListCode "
            BindingTable(lSQL, mCnn, mDsBazdid, "Tbl_SubCheckList", , , , , , , True)

            If Regex.IsMatch(mReportNo, "-[345]$") Then
                'pnlPriority.Visible = True
                'btnCheckLists.Visible = True
                'BindingTable("SELECT BTbl_BazdidCheckList.BazdidCheckListId, (BTbl_BazdidCheckListGroup.BazdidCheckListGroupName + ' - ' + BTbl_BazdidCheckList.CheckListName) As CheckListName, BTbl_BazdidCheckList.CheckListCode FROM BTbl_BazdidCheckList INNER JOIN BTbl_BazdidCheckListGroup ON BTbl_BazdidCheckList.BazdidCheckListGroupId = BTbl_BazdidCheckListGroup.BazdidCheckListGroupId WHERE BTbl_BazdidCheckListGroup.BazdidTypeId = " & mBazdidType, mCnn, mDsBazdid, "Tbl_CheckList", , , , , , , True)

                'If Regex.IsMatch(mReportNo, "-5$") Then
                '    BindingTable("SELECT ServicePartId, ServicePartName FROM BTbl_ServicePart WHERE BazdidTypeId IS NULL OR BazdidTypeId = " & mBazdidType, mCnn, mDsBazdid, "Tbl_ServicePart")
                'End If
            ElseIf Regex.IsMatch(mReportNo, "^[3]-[1-4]-5$") Then
                'BindingTable("SELECT ServicePartId, ServicePartName FROM BTbl_ServicePart WHERE BazdidTypeId IS NULL OR BazdidTypeId = " & mBazdidType, mCnn, mDsBazdid, "Tbl_ServicePart")
            Else
                pnlPriority.Visible = False
                btnCheckLists.Visible = False
            End If
            If Not Regex.IsMatch(mReportNo, "^1-[34]-2") Then
                Label7.Visible = False
                txtLPPostCode.Visible = False
                cmbLPPost.Left = cmbLPFeeder.Left
                cmbLPPost.Width = cmbLPFeeder.Width
                chkLPPost.Left = chkLPFeeder.Left
                chkLPPost.Width = chkLPFeeder.Width
                lbllPPost.Left = lblLPFeeder.Left
            Else
                Label7.Visible = True
                txtLPPostCode.Visible = True
                cmbLPPost.Left = 152
                cmbLPPost.Width = 160
                chkLPPost.Left = 152
                chkLPPost.Width = 160
                lbllPPost.Left = 312

            End If

            If mReportNo = "3-1-3" Then
                btnCheckLists.Visible = False
                pnlPriority.Visible = True
            End If
            If mReportNo = "3-2-3" Then
                btnCheckLists.Visible = False
                pnlPriority.Visible = True
            End If
            If Regex.IsMatch(mReportNo, "^2-[1234]-6") Then
                chkNotService.Visible = True
            Else
                chkNotService.Visible = False
            End If

        Catch ex As Exception
            ShowError(ex)
        End Try
        If Regex.IsMatch(mReportNo, "2-[1234]-[349]") Or Regex.IsMatch(mReportNo, "6-2") Or Regex.IsMatch(mReportNo, "4-[1234]-[45]") Or Regex.IsMatch(mReportNo, "2-4-8") Then
            pnlHideHavayiZamini.Visible = False
            pnlHavayiZamini.Visible = False
            lblHavayiZamini.Visible = False
            pnlIsWarmLine.Visible = True
            pnlIsWarmLine.BringToFront()
            If Regex.IsMatch(mReportNo, "2-2-[349]") Then
                pnlHideHavayiZamini.Visible = True
                pnlHavayiZamini.Visible = True
                lblHavayiZamini.Visible = True
                pnlIsWarmLine.Top = chkBazdidSpeciality.Top + chkBazdidSpeciality.Height
                cmbOwnership.Top = pnlIsWarmLine.Top + pnlIsWarmLine.Height + 5
                lblOwnership.Top = cmbOwnership.Top
                pnlIsWarmLine.BringToFront()
            Else
                pnlIsWarmLine.Top = cmbOwnership.Top + cmbOwnership.Height + 5
                pnlHideHavayiZamini.Visible = False
                pnlHavayiZamini.Visible = False
                lblHavayiZamini.Visible = False
            End If
            pnlIsWarmLine.Visible = True
            pnlIsWarmLine.BringToFront()
            cmbIsWarmLine.Visible = True
            lblIsWarmLine.Visible = True
        End If
        'If Regex.IsMatch(mReportNo, "2-[1234]-[9]") Or Regex.IsMatch(mReportNo, "2-4-8") Then
        '    pnlIsWarmLine.Visible = False
        'End If


        If IsCenter Then
            cmbArea.Enabled = True
        Else
            cmbArea.Enabled = False
            cmbArea.SelectedValue = WorkingAreaId
            cmbArea_SelectedIndexChanged(Nothing, Nothing)
        End If

        mFF.Clear()
        If Regex.IsMatch(mReportNo, "1-[123]-[345]") Or Regex.IsMatch(mReportNo, "3-[1234]-[35]") Then
            cmbBazdidType.Visible = True
            lblBazdidType.Visible = True
            If Regex.IsMatch(mReportNo, "-[45]") Then
                lnkOtherFilter.Top = txtAddress.Top + 20
                lnkOtherFilter.Visible = True
            Else
                lnkOtherFilter.Top = cmbReportName.Top + cmbReportName.Height - 50
                lnkOtherFilter.Visible = True
            End If
        End If
        If Regex.IsMatch(mReportNo, "[12]-[1234]-[349]") Then
            ' chkSpliteCheckList.Visible = True
        Else
            chkSpliteCheckList.Visible = False
            chkSpliteCheckList.Checked = False
        End If

        If Not Regex.IsMatch(mReportNo, "^[34]") Then
            chkArea.Visible = True
            chkMPPost.Visible = False
            chkLPPost.Visible = False
            chkLPFeeder.Visible = False
        Else
            cmbArea.Visible = False
            cmbMPPost.Visible = False
            cmbMPFeeder.Visible = False
            cmbLPPost.Visible = False
            cmbLPFeeder.Visible = False
        End If
        If Regex.IsMatch(mReportNo, "[34]") Then
            pnlMaster.Visible = True
            pnlMaster.Top = cmbOwnership.Top + cmbOwnership.Height
        End If
    End Sub
    Private Sub MakeReportTypes()
        mIsLoading = True

        mTblReportType = New DataTable("Tbl_ReportTypes")
        mTblReportType.Columns.Add("ReportTypeId", GetType(String))
        mTblReportType.Columns.Add("ReportType", GetType(String))
        mTblReportType.Columns.Add("ReportFile", GetType(String))
        mTblReportType.Columns.Add("IsSubReport", GetType(String))

        With mTblReportType.Rows
            .Add(New String() {"1-1-1", "گزارش خطوط فشارمتوسط بازديد شده", "Report_1_1_1.rpt", "0"})
            .Add(New String() {"1-1-2", "گزارش مسيرهاي فشارمتوسط بازديد شده", "Report_1_1_2.rpt", "0"})
            .Add(New String() {"1-1-3", "گزارش عيوب اصلي ديده شده مسيرهاي فشار متوسط", "Report_1_1_3.rpt", "0"})
            .Add(New String() {"1-1-4", "گزارش ريز خرابي‌هاي ديده شده مسيرهاي فشار متوسط", "Report_1_1_4.rpt", "0"})
            .Add(New String() {"1-1-5", "گزارش تجهيزات مورد نياز تعميرات مسيرهاي فشار متوسط", "Report_1_1_5.rpt", "1"})
            .Add(New String() {"1-1-6", "گزارش عملکرد نواحي در خصوص بازديد خطوط فشار متوسط", "Report_1_1_6.rpt", "0"})
            .Add(New String() {"1-1-7", "گزارش اطلاعات ثبت شده در مورد طراحي و نقشه شبکه", "Report_1_1_7.rpt", "0"})

            .Add(New String() {"1-2-1", "گزارش خطوط فشار متوسطي که پستهاي آنها بازديد شده است", "Report_1_2_1.rpt", "0"})
            .Add(New String() {"1-2-2", "گزارش پست‌هاي توزيع بازديد شده", "Report_1_2_2.rpt", "0"})
            .Add(New String() {"1-2-3", "گزارش عيوب اصلي ديده شده پست‌هاي توزيع", "Report_1_2_3.rpt", "0"})
            .Add(New String() {"1-2-4", "گزارش ريز خرابي‌هاي ديده شده پست‌هاي توزيع", "Report_1_2_4.rpt", "0"})
            .Add(New String() {"1-2-5", "گزارش تجهيزات مورد نياز تعميرات پست‌هاي توزيع", "Report_1_2_5.rpt", "1"})
            .Add(New String() {"1-2-8", "گزارش آماري عيوب ديده شده در پست هاي توزيع به تفکيک مالکِت", "Report_1_2_8.rpt", "0"})
            .Add(New String() {"1-2-9", "گزارش عملکرد نواحي در خصوص بازديد پست هاي توزيع", "Report_1_2_9.rpt", "0"})

            .Add(New String() {"1-3-1", "گزارش مسيرهاي فشار ضعيف بازديد شده", "Report_1_3_1.rpt", "0"})
            .Add(New String() {"1-3-2", "گزارش خطوط فشار ضعيف بازديد شده", "Report_1_3_2.rpt", "0"})
            .Add(New String() {"1-3-3", "گزارش عيوب اصلي ديده شده مسيرهاي فشار ضعيف", "Report_1_3_3.rpt", "0"})
            .Add(New String() {"1-3-4", "گزارش ريز خرابي‌هاي ديده شده مسيرهاي فشار ضعيف", "Report_1_3_4.rpt", "0"})
            .Add(New String() {"1-3-5", "گزارش تجهيزات مورد نياز تعميرات مسيرهاي فشار ضعيف", "Report_1_3_5.rpt", "1"})
            .Add(New String() {"1-3-6", "گزارش اطلاعات ثبت شده در مورد طراحي و نقشه شبکه", "Report_1_3_6.rpt", "0"})
            .Add(New String() {"1-3-9", "گزارش عمکرد نواحی در خصوص بازديد فشار ضعيف", "Report_1_3_9.rpt", "1"})

            .Add(New String() {"1-4-1", "گزارش مسيرهاي روشنايي معابر بازديد شده ", "Report_1_3_1.rpt", "0"})
            .Add(New String() {"1-4-2", "گزارش خطوط روشنايي معابر بازديد شده", "Report_1_3_2.rpt", "0"})
            .Add(New String() {"1-4-3", "گزارش عيوب اصلي ديده شده شبکه هاي روشنايي معابر", "Report_1_3_3.rpt", "0"})
            .Add(New String() {"1-4-4", "گزارش ريز خرابي هاي ديده شده شبکه هاي روشنايي معابر", "Report_1_3_4.rpt", "0"})
            .Add(New String() {"1-4-5", "گزارش تجهيزات مورد نياز تعمير شبکه هاي روشنايي معابر ", "Report_1_3_5.rpt", "1"})
            .Add(New String() {"1-4-6", "گزارش اطلاعات ثبت شده در مورد طراحي و نقشه شبکه", "Report_1_3_6.rpt", "0"})

            .Add(New String() {"2-1-1", "گزارش خطوط فشار متوسط سرويس شده", "Report_2_1_1.rpt", "0"})
            .Add(New String() {"2-1-2", "گزارش مسيرهاي سرويس شده فشار متوسط", "Report_2_1_2.rpt", "0"})
            .Add(New String() {"2-1-3", "گزارش عيوب اصلي رفع شده مسيرهاي فشار متوسط", "Report_2_1_3.rpt", "0"})
            .Add(New String() {"2-1-4", "گزارش ريز خرابي‌هاي برطرف شده مسيرهاي فشار متوسط", "Report_2_1_4.rpt", "0"})
            .Add(New String() {"2-1-5", "گزارش تجهيزات مصرف شده در تعمير خطوط فشار متوسط", "Report_2_1_5.rpt", "1"})
            .Add(New String() {"2-1-6", "گزارش عيوب اصلي رفع نشده خطوط فشار متوسط", "Report_2_1_3.rpt", "0"})
            .Add(New String() {"2-1-8", "گزارش وضعیت رفع عیب خطوط فشار متوسط", "Report_2_1_8.rpt", "0"})
            .Add(New String() {"2-1-9", "گزارش ريز خرابي‌هاي برطرف نشده مسيرهاي فشار متوسط", "Report_2_1_9.mrt", "0"})

            .Add(New String() {"2-2-1", "گزارش پستهاي سرويس شده خطوط فشار متوسط", "Report_2_2_1.rpt", "0"})
            .Add(New String() {"2-2-2", "گزارش پست‌هاي توزيع سرويس شده", "Report_2_2_2.rpt", "0"})
            .Add(New String() {"2-2-3", "گزارش عيوب اصلي برطرف شده پستهاي توزيع", "Report_2_2_3.rpt", "0"})
            .Add(New String() {"2-2-4", "گزارش ريز خرابي‌هاي برطرف شده پست‌هاي توزيع", "Report_2_2_4.rpt", "0"})
            .Add(New String() {"2-2-5", "گزارش تجهيزات مصرف شده در تعمير پست‌هاي توزيع", "Report_2_2_5.rpt", "1"})
            .Add(New String() {"2-2-6", "گزارش عيوب اصلي رفع نشده پست‌هاي توزيع", "Report_2_2_3.rpt", "0"})
            .Add(New String() {"2-2-8", "گزارش وضعیت رفع عیب پست‌هاي توزيع", "Report_2_2_8.rpt", "0"})
            .Add(New String() {"2-2-9", "گزارش ريز خرابي‌هاي برطرف نشده پستهاي توزيع", "Report_2_2_9.mrt", "0"})



            .Add(New String() {"2-3-1", "گزارش پستهايي که خطوط فشار ضعيف آنها سرويس شده است", "Report_2_3_1.rpt", "0"})
            .Add(New String() {"2-3-2", "گزارش خطوط فشار ضعيف سرويس شده", "Report_2_3_2.rpt", "0"})
            .Add(New String() {"2-3-3", "گزارش عيوب اصلي رفع شده مسيرهاي فشار ضعيف", "Report_2_3_3.rpt", "0"})
            .Add(New String() {"2-3-4", "گزارش ريز خرابي‌هاي برطرف شده مسيرهاي فشار ضعيف", "Report_2_3_4.rpt", "0"})
            .Add(New String() {"2-3-5", "گزارش تجهيزات مصرف شده در تعمير مسيرهاي فشار ضعيف", "Report_2_3_5.rpt", "1"})
            .Add(New String() {"2-3-6", "گزارش عيوب اصلي رفع نشده خطوط فشار ضعيف", "Report_2_3_6.rpt", "0"})
            .Add(New String() {"2-3-8", "گزارش وضعیت رفع عیب  فشار ضعيف", "Report_2_3_3.rpt", "0"})
            .Add(New String() {"2-3-9", "گزارش ريز خرابي‌هاي برطرف نشده مسيرهاي فشار ضعيف", "Report_2_34_9.mrt", "0"})


            .Add(New String() {"2-4-1", "گزارش پستهايي که خطوط روشنايي معابر آنها سرويس شده است", "Report_2_3_1.rpt", "0"})
            .Add(New String() {"2-4-2", "گزارش خطوط روشنايي معابر سرويس شده", "Report_2_3_2.rpt", "0"})
            .Add(New String() {"2-4-3", "گزارش عيوب اصلي رفع شده مسيرهاي روشنايي معابر", "Report_2_3_3.rpt", "0"})
            .Add(New String() {"2-4-4", "گزارش ريز خرابي‌هاي برطرف شده مسيرهاي روشنايي معابر", "Report_2_3_4.rpt", "0"})
            .Add(New String() {"2-4-5", "گزارش تجهيزات مصرف شده در تعمير مسيرهاي روشنايي معابر", "Report_2_3_5.rpt", "1"})
            .Add(New String() {"2-4-6", "گزارش عيوب اصلي رفع نشده خطوط روشنايي معابر", "Report_2_4_6.rpt", "0"})
            .Add(New String() {"2-4-8", "گزارش ريز خرابي‌هاي برطرف نشده خطوط روشنايي معابر", "Report_2_34_9.mrt", "0"})

            .Add(New String() {"3-1-1", "گزارش تجمعي خطوط فشار متوسط بازديد شده", "Report_3_1_1.rpt", "0"})
            .Add(New String() {"3-1-2", "گزارش تجمعي تکه فيدرهاي بازديد شده فشار متوسط", "Report_3_1_2.rpt", "1"})
            .Add(New String() {"3-1-3", "گزارش تجمعي عيوب اصلي ديده شده در خطوط فشار متوسط", "Report_3_1_3.rpt", "1"})
            .Add(New String() {"3-1-4", "گزارش تجمعي ريز خرابي‌هاي ديده شده خطوط فشار متوسط", "Report_3_1_4.rpt", "1"})
            .Add(New String() {"3-1-5", "گزارش تجهيزات مورد نياز تعمير خطوط فشار متوسط ", "Report_3_1_5.rpt", "1"})

            .Add(New String() {"3-2-1", "گزارش  تجمعي خطوط فشار متوسط که پستهاي توزيع آنها بازديد شده", "Report_3_2_1.rpt", "1"})
            .Add(New String() {"3-2-2", "گزارش  تجمعي پستهاي توزيع بازديد شده فشارمتوسط", "Report_3_2_2.rpt", "0"})
            .Add(New String() {"3-2-3", "گزارش تجمعي عيوب اصلي ديده شده پست‌هاي توزيع", "Report_3_2_3.rpt", "1"})
            .Add(New String() {"3-2-4", "گزارش تجمعي ريز خرابي‌هاي ديده شده پست‌هاي توزيع", "Report_3_2_4.rpt", "1"})
            .Add(New String() {"3-2-5", "گزارش تجهيزات مورد نياز تعميرات پست‌هاي توزيع", "Report_3_2_5.rpt", "1"})

            .Add(New String() {"3-3-1", "گزارش تجمعي خطوط بازديد شده فشار ضعيف", "Report_3_3_1.rpt", "1"})
            .Add(New String() {"3-3-2", "پستهاي توزيع که خطوط فشار ضعيف آنها بازديد شده", "Report_3_3_2.rpt", "1"})
            .Add(New String() {"3-3-3", "گزارش تجمعي عيوب اصلي ديده شده در خطوط فشار ضعيف", "Report_3_3_3.rpt", "1"})
            .Add(New String() {"3-3-4", "گزارش تجمعي ريز خرابي‌هاي ديده شده خطوط فشار ضعيف", "Report_3_3_4.rpt", "0"})
            .Add(New String() {"3-3-5", "گزارش تجهيزات مورد نياز تعمير خطوط فشار ضعيف ", "Report_3_3_5.rpt", "1"})

            .Add(New String() {"3-4-1", "گزارش تجمعي خطوط بازديد شده روشنايي معابر", "Report_3_3_1.rpt", "1"})
            .Add(New String() {"3-4-2", "پستهاي توزيع که خطوط روشنايي معابر آنها بازديد شده", "Report_3_3_2.rpt", "1"})
            .Add(New String() {"3-4-3", "گزارش تجمعي عيوب اصلي ديده شده در خطوط روشنايي معابر", "Report_3_3_3.rpt", "1"})
            .Add(New String() {"3-4-4", "گزارش تجمعي ريز خرابي‌هاي ديده شده خطوط روشنايي معابر", "Report_3_3_4.rpt", "0"})
            .Add(New String() {"3-4-5", "گزارش تجهيزات مورد نياز تعمير خطوط روشنايي معابر ", "Report_3_3_5.rpt", "1"})

            .Add(New String() {"4-1-1", "گزارش تجمعي خطوط سرويس شده فشار متوسط", "Report_4_1_1.rpt", "1"})
            .Add(New String() {"4-1-2", "گزارش تجمعي تکه فيدرهاي سرويس شده فشار متوسط", "Report_4_1_2.rpt", "1"})
            .Add(New String() {"4-1-3", "گزارش تجمعي عيوب اصلي رفع شده در خطوط فشار متوسط", "Report_4_1_3.rpt", "1"})
            .Add(New String() {"4-1-4", "گزارش تجمعي ريز خرابي‌هاي رفع شده خطوط فشار متوسط", "Report_4_1_4.rpt", "0"})
            .Add(New String() {"4-1-5", "گزارش تجهيزات مصرف شده در تعمير خطوط فشار متوسط ", "Report_4_1_5.rpt", "1"})

            .Add(New String() {"4-2-1", "گزارش  تجمعي خطوط فشار متوسطي که پست‌هاي توزيعشان سرويس شده", "Report_4_2_1.rpt", "1"})
            .Add(New String() {"4-2-2", "گزارش  تجمعي پستهاي توزيع سرويس شده فشارمتوسط", "Report_4_2_2.rpt", "1"})
            .Add(New String() {"4-2-3", "گزارش تجمعي عيوب اصلي رفع شده پست‌هاي توزيع", "Report_4_2_3.rpt", "1"})
            .Add(New String() {"4-2-4", "گزارش تجمعي ريز خرابي‌هاي رفع شده پست‌هاي توزيع", "Report_4_2_4.rpt", "0"})
            .Add(New String() {"4-2-5", "گزارش تجهيزات مصرف شده در تعمير پست‌هاي توزيع", "Report_4_2_5.rpt", "1"})

            .Add(New String() {"4-3-1", "گزارش تجمعي خطوط سرويس شده فشار ضعيف", "Report_4_3_1.rpt", "1"})
            .Add(New String() {"4-3-2", "پستهاي توزيع که خطوط فشار ضعيف آنها سرويس شده", "Report_4_3_2.rpt", "1"})
            .Add(New String() {"4-3-3", "گزارش تجمعي عيوب اصلي رفع شده در خطوط فشار ضعيف", "Report_4_3_3.rpt", "1"})
            .Add(New String() {"4-3-4", "گزارش تجمعي ريز خرابي‌هاي رفع شده خطوط فشار ضعيف", "Report_4_3_4.rpt", "0"})
            .Add(New String() {"4-3-5", "گزارش تجهيزات مصرف شده در تعمير خطوط فشار ضعيف ", "Report_4_3_5.rpt", "1"})

            .Add(New String() {"4-4-1", "گزارش تجمعي شبکه سرويس شده روشنايي معابر", "Report_4_3_1.rpt", "1"})
            .Add(New String() {"4-4-2", "پستهاي توزيع که شبکه روشنايي معابر آنها سرويس شده", "Report_4_3_2.rpt", "1"})
            .Add(New String() {"4-4-3", "گزارش تجمعي عيوب اصلي رفع شده در شبکه روشنايي معابر", "Report_4_3_3.rpt", "1"})
            .Add(New String() {"4-4-4", "گزارش تجمعي ريز خرابي‌هاي رفع شده شبکه روشنايي معابر", "Report_4_3_4.rpt", "0"})
            .Add(New String() {"4-4-5", "گزارش تجهيزات مصرف شده در تعمير شبکه روشنايي معابر ", "Report_4_3_5.rpt", "1"})
        End With

        '--------------------------------

        cmbReportName.DataSource = mTblReportType
        cmbReportName.DisplayMember = "ReportType"
        cmbReportName.ValueMember = "ReportTypeId"

        mIsLoading = False
    End Sub
    Private Function CreateWhere() As String
        CreateWhere = ""
        Dim lWhere As String = "", lTempWhere As String = ""
        Dim lWhereParam As String = "", lSubWhere As String = ""
        Dim lIsParam As Boolean
        Dim lIsRep3_3_12 As Boolean = False
        Dim lIsNeedMainWhere As Boolean = True

        Dim lWhereMaster As String = ""
        Dim lIsMainNeedMaster As Boolean = False
        Dim lIsSubNeedMaster As Boolean = False

        mFilterInfo = ""
        mFF.Clear()
        mSubFormulas.Clear()

        lIsParam = Regex.IsMatch(mReportNo, "^3-[12]-")
        lIsRep3_3_12 = Regex.IsMatch(mReportNo, "^3-[34]-[12]")
        lIsNeedMainWhere = Not Regex.IsMatch(mReportNo, "^(3-[12]-|4-[34]-[12])")

        If Not mIsServiceRep Then
            lTempWhere =
                " AND {BTblBazdidResult.BazdidStateId} IN [" & BazdidStates.bst_Done & "," & BazdidStates.bst_Working & "]"

            If Not lIsRep3_3_12 Then
                If lIsNeedMainWhere Then
                    lWhere &= lTempWhere
                Else
                    lSubWhere &= lTempWhere
                End If
            Else
                AddSubReportFormula("StartEndDates", lTempWhere)
            End If

            If lIsParam Then
                lWhereParam &=
                " AND (BTblBazdidResult.BazdidStateId IS NULL OR " &
                "BTblBazdidResult.BazdidStateId IN (" &
                BazdidStates.bst_Done & "," & BazdidStates.bst_Working &
                "))"
            End If
        ElseIf _
            mReportNo > "2-1-2" And mReportNo <= "2-1-5" Or
            mReportNo > "2-2-2" And mReportNo <= "2-2-5" Or
            mReportNo > "4-1-2" And mReportNo < "4-1-5" Or
            mReportNo > "4-2-2" And mReportNo < "4-2-5" Or
            mReportNo > "4-3-2" And mReportNo < "4-3-5" _
        Then
            lWhere &= " AND {BTblServiceCheckList.ServiceStateId} = " & ServiceStates.sst_Done
        End If

        lTempWhere = ""
        If mBazdidType = BazdidVariables.BazdidTypes.bzt_LPPost Then
            lTempWhere &= " AND {BTblBazdidResult.BazdidTypeId} = " & BazdidTypes.bzt_LPPost
        ElseIf mBazdidType = BazdidVariables.BazdidTypes.bzt_MPFeeder Then
            lTempWhere &= " AND {BTblBazdidResult.BazdidTypeId} = " & BazdidTypes.bzt_MPFeeder
        ElseIf mBazdidType = BazdidVariables.BazdidTypes.bzt_LPFeeder Then
            lTempWhere &= " AND {BTblBazdidResult.BazdidTypeId} = " & BazdidTypes.bzt_LPFeeder
            lTempWhere &= " AND " & IIf(mIsLightReport, "", "NOT") & " {Tbl_LPFeeder.IsLightFeeder}"
        End If

        If Not lIsRep3_3_12 Then
            If lIsNeedMainWhere Then
                lWhere &= lTempWhere
            Else
                lSubWhere &= lTempWhere
            End If
        Else
            AddSubReportFormula("StartEndDates", lTempWhere)
        End If

        If cmbArea.SelectedIndex > -1 Then
            If mReportNo <> "3-2-2" Then
                lWhere &= " AND {Tbl_Area.AreaId} = " & cmbArea.SelectedValue
            Else
                mFF.AddFormulaFields("AreaId", cmbArea.SelectedValue)
            End If
            If lIsParam Then lWhereParam &= " AND Tbl_MPFeeder.AreaId = " & cmbArea.SelectedValue
        Else
            lWhere &= " AND NOT {Tbl_Area.IsCenter}"
            If mReportNo = "3-2-2" Then
                mFF.AddFormulaFields("AreaId", "0")
            End If
        End If

        If txtDateFrom.IsOK Then
            mFF.AddFormulaFields("FromDate", "'" & txtDateFrom.Text & "'")
            If mIsServiceRep And (mReportNo = "4-3-2" Or Not Regex.IsMatch(mReportNo, "^4-[123]-[12]$")) Then
                If lIsNeedMainWhere Then
                    lWhere &= " AND ({BTblServiceCheckList.DoneDatePersian} >= '" & txtDateFrom.Text & "')"
                Else
                    lSubWhere &= " AND ({BTblServiceCheckList.DoneDatePersian} >= '" & txtDateFrom.Text & "')"
                End If

                mFilterInfo &= " - از تاريخ شروع سرويس "
            Else
                If Regex.IsMatch(mReportNo, "^3-2-") Then
                    lWhere &= " AND {BTblBazdidTiming.StartDatePersian} >= '" & txtDateFrom.Text & "'"
                Else
                    lTempWhere = " AND {BTblBazdidTiming.StartDatePersian} >= '" & txtDateFrom.Text & "'"
                    If Regex.IsMatch(mReportNo, "^3-3-[1234]") Then
                        lTempWhere = lTempWhere.Replace("BTblBazdidTiming", "BTblBazdidResultAddress")
                    End If

                    If Not lIsRep3_3_12 Then
                        If lIsNeedMainWhere Then
                            lWhere &= lTempWhere
                        Else
                            lSubWhere &= lTempWhere
                        End If
                    Else
                        AddSubReportFormula("StartEndDates", lTempWhere)
                    End If
                End If
                mFilterInfo &= " - از تاريخ شروع بازديد "
            End If
            If lIsParam Then lWhereParam &= " AND BTblBazdidTiming.StartDatePersian >= '" & txtDateFrom.Text & "'"
            mFilterInfo &= DateReverce(txtDateFrom.Text)
        End If

        If txtDateTo.IsOK Then
            mFF.AddFormulaFields("ToDate", "'" & txtDateTo.Text & "'")
            If mIsServiceRep And (mReportNo = "4-3-2" Or Not Regex.IsMatch(mReportNo, "^4-[123]-[12]$")) Then
                If lIsNeedMainWhere Then
                    lWhere &= " AND ({BTblServiceCheckList.DoneDatePersian} <= '" & txtDateTo.Text & "')"
                Else
                    lSubWhere &= " AND ({BTblServiceCheckList.DoneDatePersian} <= '" & txtDateTo.Text & "')"
                End If
                mFilterInfo &= " - تا تاريخ شروع سرويس "
            Else
                If (mReportNo.Substring(0, 4) = "3-2-") Then
                    lWhere &= " AND {BTblBazdidTiming.StartDatePersian} <= '" & txtDateTo.Text & "'"
                Else
                    lTempWhere = " AND {BTblBazdidTiming.StartDatePersian} <= '" & txtDateTo.Text & "'"
                    If Regex.IsMatch(mReportNo, "^3-3-[1234]") Then
                        lTempWhere = lTempWhere.Replace("BTblBazdidTiming", "BTblBazdidResultAddress")
                    End If

                    If Not lIsRep3_3_12 Then
                        If lIsNeedMainWhere Then
                            lWhere &= lTempWhere
                        Else
                            lSubWhere &= lTempWhere
                        End If
                    Else
                        AddSubReportFormula("StartEndDates", lTempWhere)
                    End If
                End If
                mFilterInfo &= " - تا تاريخ شروع بازديد "
            End If
            If lIsParam Then lWhereParam &= " AND BTblBazdidTiming.StartDatePersian <= '" & txtDateTo.Text & "'"
            mFilterInfo &= DateReverce(txtDateTo.Text)
        End If

        If cmbMPPost.SelectedIndex > -1 Then
            'If mReportNo.Substring(0, 2) = "3-" Or mReportNo = "1-1-6" Then
            If Regex.IsMatch(mReportNo, "^(3-|1-1-6)") Then
                lTempWhere = " AND {Tbl_MPFeeder.MPPostId} = " & cmbMPPost.SelectedValue
                If Not lIsRep3_3_12 Then
                    If lIsNeedMainWhere Then
                        lWhere &= lTempWhere
                    Else
                        lSubWhere &= lTempWhere
                    End If
                Else
                    AddSubReportFormula("StartEndDates", lTempWhere)
                End If
            Else
                If Regex.IsMatch(mReportNo, "^4-1-3") Then
                    lWhere &= " AND {Tbl_MPFeeder.MPPostId} = " & cmbMPPost.SelectedValue
                Else
                    lWhere &= " AND {Tbl_MPPost.MPPostId} = " & cmbMPPost.SelectedValue
                End If
            End If
            If lIsParam Then lWhereParam &= " AND Tbl_MPFeeder.MPPostId = " & cmbMPPost.SelectedValue
            mFilterInfo &= " - پست فوق توزيع " & cmbMPPost.Text
        End If

        If cmbMPFeeder.SelectedIndex > -1 Then
            lTempWhere = " AND {Tbl_MPFeeder.MPFeederId} = " & cmbMPFeeder.SelectedValue
            If Not lIsRep3_3_12 Then
                If lIsNeedMainWhere Then
                    lWhere &= lTempWhere
                Else
                    lSubWhere &= lTempWhere
                End If
            Else
                AddSubReportFormula("StartEndDates", lTempWhere)
            End If
            If lIsParam Then lWhereParam &= " AND Tbl_MPFeeder.MPFeederId = " & cmbMPFeeder.SelectedValue
            mFilterInfo &= " - فيدر فشار متوسط " & cmbMPFeeder.Text
        End If

        If mBazdidType = BazdidVariables.BazdidTypes.bzt_MPFeeder Then
            'lTempWhere &= " AND {Tbl_MPFeeder.IsActive}"
            If lIsNeedMainWhere Then
                lWhere &= lTempWhere
            Else
                lSubWhere &= lTempWhere
            End If
        End If

        If mBazdidType = BazdidVariables.BazdidTypes.bzt_LPPost Then
            'lTempWhere = " AND {Tbl_LPPost.IsActive}"
            If mReportNo <> "3-2-4" And Not lIsRep3_3_12 Then
                If mReportNo <> "3-2-1" Then
                    If lIsNeedMainWhere Then
                        lWhere &= lTempWhere
                    Else
                        lSubWhere &= lTempWhere
                    End If
                End If
            ElseIf mReportNo = "3-2-4" Then
                AddSubReportFormula("AreaPriorityInfo", lTempWhere)
                AddSubReportFormula("TotalPriorityInfo", lTempWhere)
            ElseIf lIsRep3_3_12 Then
                AddSubReportFormula("StartEndDates", lTempWhere)
            End If
        End If
        If mBazdidType >= BazdidVariables.BazdidTypes.bzt_LPPost And cmbLPPost.SelectedIndex > -1 Then
            lTempWhere &= " AND {Tbl_LPPost.LPPostId} = " & cmbLPPost.SelectedValue
            If mReportNo <> "3-2-4" And Not lIsRep3_3_12 Then
                If lIsNeedMainWhere Then
                    lWhere &= lTempWhere
                Else
                    lSubWhere &= lTempWhere
                End If
            ElseIf mReportNo = "3-2-4" Then
                AddSubReportFormula("AreaPriorityInfo", lTempWhere)
                AddSubReportFormula("TotalPriorityInfo", lTempWhere)
            ElseIf lIsRep3_3_12 Then
                AddSubReportFormula("StartEndDates", lTempWhere)
            End If
            mFilterInfo &= " - پست توزيع " & cmbLPPost.Text
        End If

        lTempWhere = ""
        If mBazdidType = BazdidVariables.BazdidTypes.bzt_LPFeeder Then
            'lTempWhere &= " AND {Tbl_LPFeeder.IsActive}"
        End If
        If mBazdidType = BazdidVariables.BazdidTypes.bzt_LPFeeder And cmbLPFeeder.SelectedIndex > -1 Then
            lTempWhere &= " AND {Tbl_LPFeeder.LPFeederId} = " & cmbLPFeeder.SelectedValue
            mFilterInfo &= IIf(mIsLightReport, " - فيدر روشنايي معابر ", " - فيدر فشار ضعيف ") & cmbLPFeeder.Text
        End If
        If Not lIsRep3_3_12 Then
            If lIsNeedMainWhere Then
                lWhere &= lTempWhere
            Else
                lSubWhere &= lTempWhere
            End If
        Else
            AddSubReportFormula("StartEndDates", lTempWhere)
        End If

        lTempWhere = ""
        If cmbOwnership.SelectedIndex > -1 Then
            If lIsParam Then lWhereParam &= " AND Tbl_MPFeeder.OwnershipId = " & cmbOwnership.SelectedValue
            If mBazdidType = BazdidVariables.BazdidTypes.bzt_MPFeeder Then
                lTempWhere &= " AND {Tbl_MPFeeder.OwnershipId} = " & cmbOwnership.SelectedValue
                mFilterInfo &= " - فيدرهاي با مالکيت " & cmbOwnership.Text
            ElseIf mBazdidType = BazdidVariables.BazdidTypes.bzt_LPPost Then
                If mReportNo <> "3-2-1" Then
                    lTempWhere &= " AND {Tbl_LPPost.OwnershipId} = " & cmbOwnership.SelectedValue
                    mFilterInfo &= " - پست‌هاي با مالکيت " & cmbOwnership.Text
                Else
                    lTempWhere &= " AND {Tbl_MPFeeder.OwnershipId} = " & cmbOwnership.SelectedValue
                    mFilterInfo &= " - فيدرهاي با مالکيت " & cmbOwnership.Text
                End If
            ElseIf mBazdidType = BazdidVariables.BazdidTypes.bzt_LPFeeder Then
                lTempWhere &= " AND {Tbl_LPFeeder.OwnershipId} = " & cmbOwnership.SelectedValue
                mFilterInfo &= " - فيدرهاي با مالکيت " & cmbOwnership.Text
            End If
        End If
        If Not lIsRep3_3_12 Then
            If lIsNeedMainWhere Then
                lWhere &= lTempWhere
            Else
                lSubWhere &= lTempWhere
            End If
        Else
            AddSubReportFormula("StartEndDates", lTempWhere)
        End If

        lTempWhere = ""
        If Not mIsServiceRep Then

            If mBazdidType = BazdidVariables.BazdidTypes.bzt_MPFeeder And mReportNo <> "3-1-1" And mReportNo <> "3-1-2" Then

                If rbHavayi.Checked Then
                    lTempWhere &= " AND ( isnull({Tbl_MPFeeder.ZeminiLength}) OR {Tbl_MPFeeder.HavaeiLength} >= {Tbl_MPFeeder.ZeminiLength} )"
                    If lIsParam Then lWhereParam &= " AND ( (BTblBazdidResult.FromToLengthZamini IS NULL) OR BTblBazdidResult.FromToLengthHavayi >= BTblBazdidResult.FromToLengthZamini )"
                    mFilterInfo &= " - بخش هوايي"
                End If

                If rbZamini.Checked Then
                    lTempWhere &= " AND ( isnull({Tbl_MPFeeder.HavaeiLength}) OR {Tbl_MPFeeder.HavaeiLength} < {Tbl_MPFeeder.ZeminiLength} )"
                    If lIsParam Then lWhereParam &= " AND ( (BTblBazdidResult.FromToLengthZamini IS NULL) OR BTblBazdidResult.FromToLengthHavayi < BTblBazdidResult.FromToLengthZamini )"
                    mFilterInfo &= " - بخش زميني"
                End If

            ElseIf mBazdidType = BazdidVariables.BazdidTypes.bzt_LPFeeder Then

                If rbHavayi.Checked Then
                    lTempWhere &= " AND ( isnull({Tbl_LPFeeder.ZeminiLength}) OR {Tbl_LPFeeder.HavaeiLength} >= {Tbl_LPFeeder.ZeminiLength} )"
                    If lIsParam Then lWhereParam &= " AND ( (BTblBazdidResult.FromToLengthZamini IS NULL) OR BTblBazdidResult.FromToLengthHavayi >= BTblBazdidResult.FromToLengthZamini )"
                    mFilterInfo &= " - بخش هوايي"
                End If

                If rbZamini.Checked Then
                    lTempWhere &= " AND ( isnull({Tbl_LPFeeder.HavaeiLength}) OR {Tbl_LPFeeder.HavaeiLength} < {Tbl_LPFeeder.ZeminiLength} )"
                    If lIsParam Then lWhereParam &= " AND ( (BTblBazdidResult.FromToLengthZamini IS NULL) OR BTblBazdidResult.FromToLengthHavayi < BTblBazdidResult.FromToLengthZamini )"
                    mFilterInfo &= " - بخش زميني"
                End If

            ElseIf mBazdidType = BazdidVariables.BazdidTypes.bzt_LPPost Then

                If rbHavayi.Checked Then
                    lTempWhere &= " AND {Tbl_LPPost.IsHavayi}"
                    mFilterInfo &= " - پست‌هاي هوايي"
                End If

                If rbZamini.Checked Then
                    lTempWhere &= " AND NOT {Tbl_LPPost.IsHavayi}"
                    mFilterInfo &= " - پست‌هاي زميني"
                End If

            End If

        End If
        If Not lIsRep3_3_12 Then
            If lIsNeedMainWhere Then
                lWhere &= lTempWhere
            Else
                lSubWhere &= lTempWhere
            End If
        Else
            AddSubReportFormula("StartEndDates", lTempWhere)
        End If
        lTempWhere = ""

        Dim lMasterIDs As String, lBasketDetailIDs As String
        lMasterIDs = ckcmbBazdidMaster.GetDataList()
        lBasketDetailIDs = ckcmbBasketDetail.GetDataList()

        lWhereMaster = ""
        If lMasterIDs <> "" Then
            If mReportNo = "1-1-6" Then
                lWhereMaster &= " AND {BTblBazdidTiming.BazdidMasterId} IN [" & lMasterIDs & "]"
            Else
                lWhereMaster &= " AND {BTbl_BazdidMaster.BazdidMasterId} IN [" & lMasterIDs & "]"
            End If
            mFilterInfo &= """ + chr(13) + ""- فقط اين بازديد کننده(ها): " & ckcmbBazdidMaster.GetDataTextList()
            If lIsParam Then lWhereParam &= " AND BTblBazdidTiming.BazdidMasterId IN (" & lMasterIDs & ")"
        End If
        If Not lIsRep3_3_12 Then
            If lIsNeedMainWhere Or Regex.IsMatch(mReportNo, "^3-2-[345]") Then
                lWhere &= lWhereMaster
            Else
                lSubWhere &= lWhereMaster
            End If
        Else
            AddSubReportFormula("StartEndDates", lWhereMaster)
        End If
        lTempWhere = ""

        If lBasketDetailIDs <> "" Then
            lTempWhere &= " AND {BTblBazdidResult.BazdidBasketDetailId} IN [" & lBasketDetailIDs & "]"
            If lIsParam Then lWhereParam &= " AND BTblBazdidResult.BazdidBasketDetailId IN (" & lBasketDetailIDs & ")"
            mFilterInfo &= """ + chr(13) + ""- فقط اين تکه فيدرها(ها): " & ckcmbBasketDetail.GetDataTextList()
        End If
        If Not lIsRep3_3_12 Then
            If lIsNeedMainWhere Then
                lWhere &= lTempWhere
            Else
                lSubWhere &= lTempWhere
            End If

        Else
            AddSubReportFormula("StartEndDates", lTempWhere)
        End If
        lTempWhere = ""

        If Regex.IsMatch(mReportNo, "^([124]-[1-4]-[345]|3-[34]-[1-5])$") Then
            Dim lPrs As String = ""
            For Each lIndex As Integer In cklstPriority.CheckedIndices
                lPrs &= IIf(lPrs <> "", ",", "") & lIndex + 1
            Next
            If lPrs <> "" Then
                lTempWhere &= " AND {BTblBazdidResultCheckList.Priority} IN [" & lPrs & "]"
                mFilterInfo &= " - اولويتهاي: " & lPrs.Replace(",", " و ")
            End If

            If mCheckLists <> "" Then
                lTempWhere &= " AND {BTblBazdidResultCheckList.BazdidCheckListId} IN [" & mCheckLists & "]"
            End If

            If Not lIsRep3_3_12 Then
                If lIsNeedMainWhere Then
                    lWhere &= lTempWhere
                Else
                    lSubWhere &= lTempWhere
                End If
            Else
                AddSubReportFormula("StartEndDates", lTempWhere)
            End If

            If Regex.IsMatch(mReportNo, "-5$") AndAlso mParts <> "" Then
                mFF.AddSubReportFormula("Parts", "{BTbl_ServicePart.ServicePartId} IN [" & mParts & "]")
                mFF.AddSubReportFormula("SumParts", "{BTbl_ServicePart.ServicePartId} IN [" & mParts & "]")
            End If
        ElseIf Regex.IsMatch(mReportNo, "^[3]-[12]-5$") Then
            mFF.AddSubReportFormula("Parts", "{BTbl_ServicePart.ServicePartId} IN [" & mParts & "]")
        End If

        lTempWhere = ""
        If mFilterInfo.Length > 15 AndAlso mFilterInfo.Substring(0, 15) = """ + chr(13) + """ Then
            mFilterInfo = mFilterInfo.Remove(0, 15)
        End If
        mFilterInfo = mFilterInfo.Replace("ي", "ي")

        If lIsParam Then
            mFF.AddFormulaFields("WhereClause", """" & lWhereParam & """")
        End If

        If mReportNo = "3-4-2" Then
            mFF.AddFormulaFields("ColumnTitleBazdid", "'تعداد پست توزيع عمومي که فيدرهاي روشنايي معابرشان بازديد شده است'")
        End If

        mFF.AddFormulaFields("ReportMaker", """" & WorkingUserName & """")
        If lWhere.Length > 5 Then
            lWhere = lWhere.Remove(0, 5)
        End If

        Dim lStrFrml As String = ""
        If mReportNo = "3-2-3" Then
            mFF.AddSubReportFormula("HavayiSubReport", lWhere & lSubWhere)
            mFF.AddSubReportFormula("ZaminiSubReport", lWhere & lSubWhere)
        ElseIf mReportNo = "3-2-4" Then
            lStrFrml = GetSubReportFormula("AreaPriorityInfo", lWhere & lSubWhere)
            mFF.AddSubReportFormula("AreaPriorityInfo", lStrFrml)
            lStrFrml = GetSubReportFormula("TotalPriorityInfo", lWhere & lSubWhere)
            mFF.AddSubReportFormula("TotalPriorityInfo", lStrFrml)
        ElseIf mReportNo = "3-2-5" Then
            mFF.AddSubReportFormula("AreaCheckListParts", lWhere & lSubWhere)
            mFF.AddSubReportFormula("TotalCheckListParts", lWhere & lSubWhere)
            mFF.AddSubReportFormula("AreaBazdidInfo", lWhere & lSubWhere)
            mFF.AddSubReportFormula("SumAreaBazdidInfo", lWhere & lSubWhere)
        ElseIf Regex.IsMatch(mReportNo, "^3-[34]-[12]") Then
            lStrFrml = GetSubReportFormula("StartEndDates", lWhere)
            mFF.AddSubReportFormula("StartEndDates", lStrFrml)
        ElseIf Regex.IsMatch(mReportNo, "^3-[34]-3") Then
            mFF.AddSubReportFormula("SubReportTotal", lWhere)
        ElseIf Regex.IsMatch(mReportNo, "^3-[34]-5") Then
            mFF.AddSubReportFormula("Parts", lWhere)
            mFF.AddSubReportFormula("SumParts", lWhere)
            mFF.AddSubReportFormula("AreaBazdidInfo", lWhere)
        ElseIf mReportNo = "4-2-1" Then
            mFF.AddSubReportFormula("Subreport1", lWhere)
            mFF.AddSubReportFormula("Subreport2", lWhere)
        ElseIf Regex.IsMatch(mReportNo, "^4-(1-[12]|2-2|[34]-[12])$") Then
            mFF.AddSubReportFormula("StartEndDates", lWhere & lSubWhere)
        ElseIf Regex.IsMatch(mReportNo, "^4-[134]-3") Then
            mFF.AddSubReportFormula("Totals", lWhere)
        ElseIf mReportNo = "4-2-3" Then
            mFF.AddSubReportFormula("HavayiSubReport", lWhere)
            mFF.AddSubReportFormula("ZaminiSubReport", lWhere)
        ElseIf Regex.IsMatch(mReportNo, "^4-[1-4]-5") Then
            mFF.AddSubReportFormula("ServiceDates", lWhere)
            If mParts <> "" Then
                lWhere &= " AND {BTbl_ServicePart.ServicePartId} IN [" & mParts & "]"
            End If
            lWhere = lWhere.Replace("{BTblServiceCheckList.DoneDatePersian}", "{BTblService.StartDatePersian}")
            mFF.AddSubReportFormula("SumParts", lWhere)
            If Regex.IsMatch(mReportNo, "^4-[34]-5") Then
                mFF.AddSubReportFormula("Totals", lWhere)
            End If
        End If

        Return lWhere
    End Function
    Private Sub MakeReport()
        If mReportFile = "" Then
            ShowError("لطفا نوع گزارش را مشخص نماييد", False, MsgBoxIcon.MsgIcon_Hand)
            Exit Sub
        End If
        Dim lWhere = ""
        If mReportNo = "1-1-1" Then
            MakeReport_1_1_1()
            Exit Sub
        ElseIf mReportNo = "1-1-2" Then
            MakeReport_1_1_2()
            Exit Sub
        ElseIf mReportNo = "1-1-3" Then
            MakeReport_1_1_3()
            Exit Sub
        ElseIf mReportNo = "1-1-4" Then
            MakeReport_1_1_4()
            Exit Sub
        ElseIf mReportNo = "1-1-5" Then
            MakeReport_1_1_5()
            Exit Sub
        ElseIf mReportNo = "1-1-6" Then
            MakeReport_1_1_6()
            Exit Sub
        ElseIf mReportNo = "1-1-7" Then
            MakeReport_1_1_7()
            Exit Sub
        ElseIf mReportNo = "1-2-1" Then
            MakeReport_1_2_1()
            Exit Sub
        ElseIf mReportNo = "1-2-2" Then
            MakeReport_1_2_2()
            Exit Sub
        ElseIf mReportNo = "1-2-3" Then
            MakeReport_1_2_3()
            Exit Sub
        ElseIf mReportNo = "1-2-4" Then
            MakeReport_1_2_4()
            Exit Sub
        ElseIf mReportNo = "1-2-5" Then
            MakeReport_1_2_5()
            Exit Sub
        ElseIf mReportNo = "1-2-8" Then
            If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.ver_950431) Then
                ShowHavadesInfoMessage()
                Exit Sub
            End If
            MakeReport_1_2_8()
            Exit Sub
        ElseIf mReportNo = "1-2-9" Then
            MakeReport_1_2_9()
            Exit Sub
        ElseIf Regex.IsMatch(mReportNo, "^1-[34]-1") Then
            MakeReport_1_34_1()
            Exit Sub
        ElseIf Regex.IsMatch(mReportNo, "^1-[34]-2") Then
            MakeReport_1_34_2()
            Exit Sub
        ElseIf Regex.IsMatch(mReportNo, "^1-[34]-3") Then
            MakeReport_1_34_3()
            Exit Sub
        ElseIf Regex.IsMatch(mReportNo, "^1-[34]-4") Then
            MakeReport_1_34_4()
            Exit Sub
        ElseIf Regex.IsMatch(mReportNo, "^1-[34]-5") Then
            MakeReport_1_34_5()
            Exit Sub
        ElseIf Regex.IsMatch(mReportNo, "^1-[34]-6") Then
            MakeReport_1_34_6()
            Exit Sub
        ElseIf mReportNo = "1-3-9" Then
            MakeReport_1_3_9()
            Exit Sub
        ElseIf mReportNo = "2-1-1" Then
            MakeReport_2_1_1()
            Exit Sub
        ElseIf mReportNo = "2-1-2" Then
            MakeReport_2_1_2()
            Exit Sub
        ElseIf mReportNo = "2-1-3" Then
            MakeReport_2_1_3()
            Exit Sub
        ElseIf mReportNo = "2-1-4" Then
            MakeReport_2_1_4()
            Exit Sub
        ElseIf mReportNo = "2-1-5" Then
            MakeReport_2_1_5()
            Exit Sub
        ElseIf mReportNo = "2-1-6" Then
            MakeReport_2_1_6()
            Exit Sub
        ElseIf mReportNo = "2-1-8" Then
            MakeReport_2_1_8()
            Exit Sub
        ElseIf mReportNo = "2-1-9" Then
            MakeReport_2_1_9()
            Exit Sub
        ElseIf mReportNo = "2-2-1" Then
            MakeReport_2_2_1()
            Exit Sub
        ElseIf mReportNo = "2-2-2" Then
            MakeReport_2_2_2()
            Exit Sub
        ElseIf mReportNo = "2-2-3" Then
            MakeReport_2_2_3()
            Exit Sub
        ElseIf mReportNo = "2-2-4" Then
            MakeReport_2_2_4()
            Exit Sub
        ElseIf mReportNo = "2-2-5" Then
            MakeReport_2_2_5()
            Exit Sub
        ElseIf mReportNo = "2-2-6" Then
            MakeReport_2_2_6()
            Exit Sub
        ElseIf mReportNo = "2-2-8" Then
            MakeReport_2_2_8()
            Exit Sub
        ElseIf mReportNo = "2-2-9" Then
            MakeReport_2_2_9()
            Exit Sub
        ElseIf Regex.IsMatch(mReportNo, "^2-[34]-1") Then
            MakeReport_2_34_1()
            Exit Sub
        ElseIf Regex.IsMatch(mReportNo, "^2-[34]-2") Then
            MakeReport_2_34_2()
            Exit Sub
        ElseIf Regex.IsMatch(mReportNo, "^2-[34]-3") Then
            MakeReport_2_34_3()
            Exit Sub
        ElseIf Regex.IsMatch(mReportNo, "^2-[34]-4") Then
            MakeReport_2_34_4()
            Exit Sub
        ElseIf Regex.IsMatch(mReportNo, "^2-[34]-5") Then
            MakeReport_2_34_5()
            Exit Sub
        ElseIf Regex.IsMatch(mReportNo, "^2-[34]-6") Then
            MakeReport_2_34_6()
            Exit Sub
        ElseIf Regex.IsMatch(mReportNo, "2-3-8") Then
            MakeReport_2_3_8()
            Exit Sub
        ElseIf Regex.IsMatch(mReportNo, "2-3-9") Or Regex.IsMatch(mReportNo, "2-4-8") Then
            MakeReport_2_34_9()
            Exit Sub
        ElseIf mReportNo = "3-1-1" Then
            MakeReport_3_1_1()
            Exit Sub
        ElseIf mReportNo = "3-1-2" Then
            MakeReport_3_1_2()
            Exit Sub
        ElseIf mReportNo = "3-1-3" Then
            MakeReport_3_1_3()
            Exit Sub
        ElseIf mReportNo = "3-1-4" Then
            MakeReport_3_1_4()
            Exit Sub
        ElseIf mReportNo = "3-1-5" Then
            MakeReport_3_1_5()
            Exit Sub
        ElseIf mReportNo = "3-2-1" Then
            MakeReport_3_2_1()
            Exit Sub
        ElseIf mReportNo = "3-2-2" Then
            MakeReport_3_2_2()
            Exit Sub
        ElseIf mReportNo = "3-2-3" Then
            MakeReport_3_2_3()
            Exit Sub
        ElseIf mReportNo = "3-2-4" Then
            MakeReport_3_2_4()
            Exit Sub
        ElseIf mReportNo = "3-2-5" Then
            MakeReport_3_2_5()
            Exit Sub
        ElseIf Regex.IsMatch(mReportNo, "^3-[34]-1") Then
            MakeReport_3_34_1()
            Exit Sub
        ElseIf Regex.IsMatch(mReportNo, "^3-[34]-2") Then
            If Not chkMode2.Checked Then
                MakeReport_3_34_2()
            ElseIf Not chkIsTafkikMPFeeder.Checked Then
                MakeReport_3_34_2_Mode2()
            Else
                MakeReport_3_34_2_Mode2_MPFeeder()
            End If
            Exit Sub
        ElseIf Regex.IsMatch(mReportNo, "^3-[34]-3") Then
            MakeReport_3_34_3()
            Exit Sub
        ElseIf Regex.IsMatch(mReportNo, "^3-[34]-4") Then
            MakeReport_3_34_4()
            Exit Sub
        ElseIf Regex.IsMatch(mReportNo, "^3-[34]-5") Then
            MakeReport_3_34_5()
            Exit Sub
        ElseIf mReportNo = "4-1-1" Then
            MakeReport_4_1_1()
            Exit Sub
        ElseIf mReportNo = "4-1-2" Then
            MakeReport_4_1_2()
            Exit Sub
        ElseIf mReportNo = "4-1-3" Then
            MakeReport_4_1_3()
            Exit Sub
        ElseIf mReportNo = "4-1-4" Then
            MakeReport_4_1_4()
            Exit Sub
        ElseIf mReportNo = "4-1-5" Then
            MakeReport_4_1_5()
            Exit Sub
        ElseIf mReportNo = "4-2-1" Then
            MakeReport_4_2_1()
            Exit Sub
        ElseIf mReportNo = "4-2-2" Then
            MakeReport_4_2_2()
            Exit Sub
        ElseIf mReportNo = "4-2-3" Then
            MakeReport_4_2_3()
            Exit Sub
        ElseIf mReportNo = "4-2-4" Then
            MakeReport_4_2_4()
            Exit Sub
        ElseIf mReportNo = "4-2-5" Then
            MakeReport_4_2_5()
            Exit Sub
        ElseIf Regex.IsMatch(mReportNo, "^4-[34]-1") Then
            MakeReport_4_34_1()
            Exit Sub
        ElseIf Regex.IsMatch(mReportNo, "^4-[34]-2") Then
            If Not chkMode2.Checked Then
                MakeReport_4_34_2()
            ElseIf Not chkIsTafkikMPFeeder.Checked Then
                MakeReport_4_34_2_Mode2()
            Else
                MakeReport_4_34_2_Mode2_MPFeeder()
            End If
            Exit Sub
        ElseIf Regex.IsMatch(mReportNo, "^4-[34]-3") Then
            MakeReport_4_34_3()
            Exit Sub
        ElseIf Regex.IsMatch(mReportNo, "^4-[34]-4") Then
            MakeReport_4_34_4()
            Exit Sub
        ElseIf Regex.IsMatch(mReportNo, "^4-[34]-5") Then
            MakeReport_4_34_5()
            Exit Sub
        Else
            lWhere = CreateWhere()
        End If

        If mFilterInfo.Length > 3 Then
            mFilterInfo = mFilterInfo.Remove(0, 3)
        End If

        Dim lFolder As String = "Reports\Bazdid\"
        If mIsSubReport Then
            If IsSetadMode Then
                lFolder &= "Setad\"
            ElseIf IsCenter Or IsMiscMode Then
                lFolder &= "Center\"
            Else
                lFolder &= "Department\"
            End If
        End If
        ShowReport(lWhere, lFolder & mReportFile, "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
    End Sub
    Private Sub SelectCheckLists()
        Dim lDlg As New frmSelectGridItems(mDsBazdid.Tables("Tbl_CheckList"), "Tbl_CheckList", "BazdidCheckListId", "CheckListName", "CheckListCode", , "انتخاب عيب", True, "عيوب مورد نظر را از فهرست زير انتخاب نماييد:", mCheckLists)
        If lDlg.ShowDialog() = DialogResult.Cancel Then Exit Sub

        mCheckLists = ""
        Dim lTbl As CommonDataSet.ViewSelectionDataTable = lDlg.ViewSelectionTable
        For Each lRow As CommonDataSet.ViewSelectionRow In lTbl.Rows
            If lRow.ItemIsSelected Then
                mCheckLists &= IIf(mCheckLists <> "", ",", "") & lRow("ItemId")
            End If
        Next

        If mCheckLists <> "" Then
            btnCheckLists.Text = "[ عيوب ]"
        Else
            btnCheckLists.Text = "عيوب"
        End If

    End Sub
    Private Sub SelectParts()
        Dim lDlg As New frmSelectGridItems(mDsBazdid.Tables("Tbl_ServicePart"), "Tbl_ServicePart", "ServicePartId", "ServicePartName", "ServicePartCode", , "انتخاب تجهيز", True, "تجهيزات مورد نظر را از فهرست زير انتخاب نماييد:", mParts)
        If lDlg.ShowDialog() = DialogResult.Cancel Then Exit Sub

        mParts = ""
        Dim lTbl As CommonDataSet.ViewSelectionDataTable = lDlg.ViewSelectionTable
        For Each lRow As CommonDataSet.ViewSelectionRow In lTbl.Rows
            If lRow.ItemIsSelected Then
                mParts &= IIf(mParts <> "", ",", "") & lRow("ItemId")
            End If
        Next

        If mParts <> "" Then
            btnParts.Text = "[ تجهيزات ]"
        Else
            btnParts.Text = "تجهيزات"
        End If

    End Sub
    Private Sub AddSubReportFormula(ByVal aSubReportName As String, ByVal aFormula As String)
        If Not mSubFormulas.ContainsKey(aSubReportName) Then
            mSubFormulas.Add(aSubReportName, "")
        End If

        Dim lFormul As String = mSubFormulas(aSubReportName)
        mSubFormulas(aSubReportName) = lFormul & " " & aFormula
    End Sub
    Private Function GetSubReportFormula(ByVal aSubReportName As String, Optional ByVal aMainWhereClause As String = "") As String
        GetSubReportFormula = ""
        Dim lStr As String = ""

        Try
            If mSubFormulas.ContainsKey(aSubReportName) Then
                lStr = mSubFormulas(aSubReportName)
                lStr = Regex.Replace(lStr, "^[ ]*(AND|OR)[ ]+", " ")
                If aMainWhereClause <> "" Then
                    lStr = aMainWhereClause & " AND (" & lStr & ")"
                End If
            End If
        Catch ex As Exception
        End Try

        Return lStr
    End Function
    Private Sub SelectSubCheckList()
        Dim lDlg As New frmSelectGridItems(mDsBazdid.Tables("Tbl_SubCheckList"), "Tbl_SubCheckList", "SubCheckListId", "SubCheckListName", "SubCheckListCode", , "انتخاب ريز خرابي", True, "ريزخرابي مورد نظر را از فهرست زير انتخاب نماييد", mSubCheckLists)
        If lDlg.ShowDialog() = DialogResult.Cancel Then Exit Sub

        mSubCheckLists = ""
        Dim lTbl As CommonDataSet.ViewSelectionDataTable = lDlg.ViewSelectionTable
        For Each lRow As CommonDataSet.ViewSelectionRow In lTbl.Rows
            If lRow.ItemIsSelected Then
                mSubCheckLists &= IIf(mSubCheckLists <> "", ",", "") & lRow("ItemId")
            End If
        Next

        If mSubCheckLists <> "" Then
            btnSubCheckList.Text = "[ ريزخرابي ]"
        Else
            btnSubCheckList.Text = "ريزخرابي"
        End If
    End Sub
    Private Sub MakeQuery()
        mFilterInfo = ""
        mFromDate = "''"
        mToDate = "''"
        mFromDateBazdid = "''"
        mToDateBazdid = "''"
        mAreaId = "''"
        mMPPostId = -1
        mMPFeederId = -1
        mLPPostId = -1
        mLPFeederId = -1
        mOwnershipId = -1
        mIsHavaei = -1
        mBazdidMasterIDs = ""
        mBasketDetailIDs = ""
        mIsLightFeeder = IIf(mIsLightReport, 1, 0)
        mIsActive = 0
        mIsWarmLine = ""
        mIsWarmLineSP = 0
        mIsExistMap = -1
        mIsUpdateMap = -1
        mIsOKNetDesign = -1
        mPrs = ""
        mAddress = ""
        mMinCheckListCount = 0
        mLPPOstCode = ""
        mLpFeederLengt = ""
        mNotService = 0
        mBazdidSpeciality = ""
        mIsNotCheckList = 0

        mWhere = ""
        mWhereDT = ""
        mWhereDT2 = ""
        mWhere2 = ""
        mWherePost = ""
        mWherePart = ""
        mJoinSpecialitySql = ""
        mMPFeederIDs = ""
        mHaving = ""

        Dim lAreaIDs As String = ""
        Dim lMPPostIDs As String = ""
        Dim lLPPostIDs As String = ""
        Dim lLPFeederIDs As String = ""

        If txtDateFrom.IsOK Then
            mFromDate = "'" & txtDateFrom.Text & "'"
            mFilterInfo = " از تاريخ " & txtDateFrom.Text
        End If
        If txtDateTo.IsOK Then
            mToDate = "'" & txtDateTo.Text & "'"
            mFilterInfo &= " تا تاريخ " & txtDateTo.Text
        End If
        If txtDateFromBazdid.IsOK Then
            mFromDateBazdid = "'" & txtDateFromBazdid.Text & "'"
            mFilterInfo = "  از تاريخ بازديد  " & txtDateFromBazdid.Text
        End If
        If txtDateToBazdid.IsOK Then
            mToDateBazdid = "'" & txtDateToBazdid.Text & "'"
            mFilterInfo &= " تا تاريخ " & txtDateToBazdid.Text
        End If
        If txtServiceNumber.Text <> "" Then
            mFilterInfo &= "- شماره سرويس " & txtServiceNumber.Text
        End If

        If chkArea.GetDataList().Length > 0 Then
            lAreaIDs = chkArea.GetDataList()
            mAreaId = chkArea.GetDataList
            mFilterInfo &= " ناحيه " & chkArea.GetDataTextList()
        End If
        If chkMPPost.GetDataList().Length > 0 Then
            lMPPostIDs = chkMPPost.GetDataList()
            mFilterInfo &= " پست فوق توزيع " & chkMPPost.GetDataTextList()
        End If
        If chkMPFeeder.GetDataList().Length > 0 Then
            mMPFeederIDs = chkMPFeeder.GetDataList()
            mFilterInfo &= " فيدر فشار متوسط " & chkMPFeeder.GetDataTextList()
        End If
        If chkLPPost.GetDataList().Length > 0 Then
            lLPPostIDs = chkLPPost.GetDataList()
            mFilterInfo &= " پست توزيع " & chkLPPost.GetDataTextList()
        End If
        If chkLPFeeder.GetDataList().Length > 0 Then
            lLPFeederIDs = chkLPFeeder.GetDataList()
            mFilterInfo &= " فيدر فشار ضعيف " & chkLPFeeder.GetDataTextList()
        End If

        If IsCenter And chkArea.GetDataList.Length = 0 Then ' cmbArea.SelectedIndex = -1
            mAreaId = GetLegalAreaIDs(mCnn)
        End If
        If cmbArea.Visible AndAlso cmbArea.SelectedIndex > -1 Then
            mAreaId = cmbArea.SelectedValue
        End If
        If mAreaId = "" Or mAreaId = "''" Then
            mAreaId = GetNotCenterAreaIDs(cmbArea.SelectedValue)
        End If
        If cmbArea.Visible AndAlso cmbArea.SelectedIndex > -1 Then
            mFilterInfo &= " ناحيه " & cmbArea.Text
        End If
        If cmbMPPost.Visible AndAlso cmbMPPost.SelectedIndex > -1 Then
            mMPPostId = cmbMPPost.SelectedValue
            mFilterInfo &= " پست فوق توزيع " & cmbMPPost.Text
        End If
        If cmbMPFeeder.Visible AndAlso cmbMPFeeder.SelectedIndex > -1 Then
            mMPFeederId = cmbMPFeeder.SelectedValue
            mFilterInfo &= " فيدر فشار متوسط " & cmbMPFeeder.Text
        End If
        If cmbLPPost.Visible AndAlso cmbLPPost.SelectedIndex > -1 Then
            mLPPostId = cmbLPPost.SelectedValue
            mFilterInfo &= " پست توزيع " & cmbLPPost.Text
        End If
        If txtLPPostCode.Text <> "" Then
            mLPPOstCode = txtLPPostCode.Text
            mFilterInfo &= "کد پست" & txtLPPostCode.Text
        End If
        If txtFeederLength.Text <> "" Then
            mLpFeederLengt = txtFeederLength.Text
            mFilterInfo &= "حداقل طول فيدر" & txtFeederLength.Text
        End If
        If cmbLPFeeder.Visible AndAlso cmbLPFeeder.SelectedIndex > -1 Then
            mLPFeederId = cmbLPFeeder.SelectedValue
            mFilterInfo &= " فيدر فشار ضعيف " & cmbLPFeeder.Text
        End If
        If cmbOwnership.SelectedIndex > -1 Then
            mOwnershipId = cmbOwnership.SelectedValue
            mFilterInfo &= " مالکيت " & cmbOwnership.Text
        End If
        If rbHavayi.Checked Then
            mIsHavaei = 1
            mFilterInfo &= " نوع شبکه هوايي "
        ElseIf rbZamini.Checked Then
            mIsHavaei = 0
            mFilterInfo &= " نوع شبکه زميني "
        End If
        If chkIsActive.Checked Then
            mIsActive = 1
            mFilterInfo &= " حالت شبکه فعال "
        End If
        '--------------omid----------
        If cmbIsWarmLine.SelectedIndex > -1 And cmbIsWarmLine.Visible Then
            mIsWarmLine = cmbIsWarmLine.SelectedIndex
            mIsWarmLineSP = 1
            mFilterInfo &= cmbIsWarmLine.Text
        End If
        If chkNotService.Checked Then
            mNotService = 1
            mFilterInfo &= " واگذار نشده ها "
        End If
        If chkIsExistMapYes.Checked And Not chkIsExistMapNo.Checked Then
            mIsExistMap = 1
            mFilterInfo &= " نقشه شبکه وجود دارد "
        ElseIf Not chkIsExistMapYes.Checked And chkIsExistMapNo.Checked Then
            mIsExistMap = 0
            mFilterInfo &= " نقشه شبکه وجود ندارد "
        End If
        If chkIsUpdateMapYes.Checked And Not chkIsUpdateMapNo.Checked Then
            mIsUpdateMap = 1
            mFilterInfo &= " نقشه شبکه بروز شده است "
        ElseIf Not chkIsUpdateMapYes.Checked And chkIsUpdateMapNo.Checked Then
            mIsUpdateMap = 0
            mFilterInfo &= " نقشه شبکه بروز نشده است "
        End If
        If chkIsOKNetDesignYes.Checked And Not chkIsOKNetDesignNo.Checked Then
            mIsOKNetDesign = 1
            mFilterInfo &= " طراحي شبکه مناسب است "
        ElseIf Not chkIsOKNetDesignYes.Checked And chkIsOKNetDesignNo.Checked Then
            mIsOKNetDesign = 0
            mFilterInfo &= " طراحي شبکه مناسب نيست "
        End If
        If txtAddress.Text <> "" Then
            mAddress = txtAddress.Text
            mFilterInfo &= " آدرس محل عيب : " & txtAddress.Text
        End If
        If chkIsNotCheckList.Checked Then
            mIsNotCheckList = 1
            mFilterInfo &= " فقط مسيرهاي بي عيب "
        End If
        If Val(txtMinCheckListCount.Text) > 0 Then
            mMinCheckListCount = Val(txtMinCheckListCount.Text)
            If Regex.IsMatch(mReportNo, "[24]-[1234]-3") Then
                mFilterInfo &= " حداقل تعداد عيب رفع شده : " & txtMinCheckListCount.Text
            Else
                mFilterInfo &= " حداقل تعداد عيب ديده شده : " & txtMinCheckListCount.Text
            End If
        End If
        If chkBazdidSpeciality.GetDataList().Length > 0 Then
            mBazdidSpeciality = chkBazdidSpeciality.GetDataList()
            mFilterInfo &= " بازديدهاي تخصصی : " & chkBazdidSpeciality.GetDataTextList
        End If

        mBazdidMasterIDs = ckcmbBazdidMaster.GetDataList()
        mBasketDetailIDs = ckcmbBasketDetail.GetDataList()
        For Each lIndex As Integer In cklstPriority.CheckedIndices
            mPrs &= IIf(mPrs <> "", ",", "") & lIndex + 1
        Next
        If mPrs <> "" Then
            mFilterInfo &= " - اولويتهاي: " & mPrs.Replace(",", " و ")
        End If
        mFF.AddFormulaFields("ReportMaker", """" & WorkingUserName & """")
        If mIsWarmLine = "1" Then
            If Regex.IsMatch(mReportNo, "^4-[34]-[12]") Then
                mWhereDT &= " AND ISNULL(BTblService.IsWarmLine,0)  =  " & mIsWarmLine & " "
            Else
                mWhere &= " AND ISNULL(BTblService.IsWarmLine,0)  =  " & mIsWarmLine & " "
            End If
        End If
        mWorkCommand = ""
        If txtWorkCommandNo.Text <> "" Then
            mWorkCommand = txtWorkCommandNo.Text
            mFilterInfo &= " - دستور کار: " & mWorkCommand
        End If
        '--------------omid----------
        If Regex.IsMatch(mReportNo, "^2|^4-(?!1-5)") Then
            mJoinSpecialitySql &= " LEFT JOIN BTblService ON BTblServiceCheckList.ServiceId = BTblService.ServiceId "
        End If
        If Regex.IsMatch(mReportNo, "2-1-6") Or Regex.IsMatch(mReportNo, "[12]-[1234]-[34]") Or Regex.IsMatch(mReportNo, "[2]-[1234]-[89]") Then 'Visible Base Controil
            If mAddress <> "" Then
                ' mWhere &= " AND dbo.MergeFarsiAndArabi('BTblBazdidResultAddress.Address'," & mAddress & ") "
                mWhere &= " AND " & MergeFarsiAndArabi("BTblBazdidResultAddress.Address", mAddress)

            End If
            If mAreaId <> "" Then
                mWhere &= " AND Tbl_Area.areaId IN ( " & mAreaId & ") "
                mWherePost &= " AND Tbl_Area.areaId IN ( " & mAreaId & ") "
            End If
            If mIsActive = 1 Then
                If Regex.IsMatch(mReportNo, "[12]-1-[34]") Or Regex.IsMatch(mReportNo, "2-1-[69]") Then
                    mWhere &= " AND Tbl_MPFeeder.IsActive = 1  "
                ElseIf Regex.IsMatch(mReportNo, "[12]-2-[34]") Then
                    mWhere &= " AND Tbl_LPPost.IsActive = 1  "
                    mWherePost &= " AND Tbl_LPPost.IsActive = 1  "
                ElseIf Regex.IsMatch(mReportNo, "[12]-[34]-[34]") Or Regex.IsMatch(mReportNo, "2-[34]-9") Then
                    mWhere &= " AND Tbl_LPFeeder.IsActive = 1  "
                End If
            End If
            If mOwnershipId > -1 Then
                If Regex.IsMatch(mReportNo, "[12]-1-[34]") Or Regex.IsMatch(mReportNo, "2-1-6") Or Regex.IsMatch(mReportNo, "2-[1234]-[9]") Then
                    mWhere &= " AND ISNULL(Tbl_MPFeeder.OwnershipId,2) = " & mOwnershipId
                ElseIf Regex.IsMatch(mReportNo, "[12]-2-[349]") Then
                    mWhere &= " AND ISNULL(Tbl_LPPost.OwnershipId,2) = " & mOwnershipId
                    mWherePost &= " AND ISNULL(Tbl_LPPost.OwnershipId,2) = " & mOwnershipId
                ElseIf Regex.IsMatch(mReportNo, "[12]-[34]-[34]") Then
                    mWhere &= " AND ISNULL(Tbl_LPFeeder.OwnershipId,2) = " & mOwnershipId
                End If
            End If
            If mIsHavaei > -1 Then
                If Regex.IsMatch(mReportNo, "[12]-1-[34]") Or Regex.IsMatch(mReportNo, "2-1-6") Then
                    If mIsHavaei = 1 Then
                        mWhere &= " AND ISNULL(Tbl_MPFeeder.HavaeiLength,0) > ISNULL(Tbl_MPFeeder.ZeminiLength,0) "
                    Else
                        mWhere &= " AND ISNULL(Tbl_MPFeeder.HavaeiLength,0) <= ISNULL(Tbl_MPFeeder.ZeminiLength,0) "
                    End If
                ElseIf Regex.IsMatch(mReportNo, "[12]-2-[34]") Then
                    mWhere &= "AND Tbl_LPPost.IsHavayi = " & mIsHavaei
                    mWherePost &= "AND Tbl_LPPost.IsHavayi = " & mIsHavaei
                ElseIf Regex.IsMatch(mReportNo, "[12]-[34]-[34]") Then
                    If mIsHavaei = 1 Then
                        mWhere &= " AND ISNULL(Tbl_LPFeeder.HavaeiLength,0) > ISNULL(Tbl_LPFeeder.ZeminiLength,0) "
                    Else
                        mWhere &= " AND ISNULL(Tbl_LPFeeder.HavaeiLength,0) <= ISNULL(Tbl_LPFeeder.ZeminiLength,0) "
                    End If
                End If
            End If
            If mLPPostId > -1 Then
                mWhere &= " AND Tbl_LPPost.LPPostId IN (" & mLPPostId & ") "
                mWherePost &= " AND Tbl_LPPost.LPPostId IN (" & mLPPostId & ") "
            ElseIf mMPFeederIDs <> "" Then
                mWhere &= " AND Tbl_MPFeeder.MPFeederId IN (" & mMPFeederIDs & ") "
                mWherePost &= " AND Tbl_MPFeeder.MPFeederId IN (" & mMPFeederIDs & ") "
            ElseIf mMPPostId > -1 Then
                mWhere &= " AND Tbl_MPPost.MPPostId IN (" & mMPPostId & ") "
                mWherePost &= " AND Tbl_MPPost.MPPostId IN (" & mMPPostId & ") "
            End If
            If mBazdidMasterIDs <> "" Then
                mWhere &= " AND BTblBazdidTiming.BazdidMasterId IN ( " & mBazdidMasterIDs & " )"
            End If
            If mBasketDetailIDs <> "" Then
                mWhere &= " AND BTblBazdidResult.BazdidBasketDetailId IN (  " & mBasketDetailIDs & "  )"
            End If
            If mMinCheckListCount > 0 Then
                If Regex.IsMatch(mReportNo, "[1]-[1234]-[34]") Then
                    mHaving = " HAVING SUM(CASE WHEN BTblBazdidResultCheckList.DefectionCount Is Null OR BTblBazdidResultCheckList.DefectionCount = 0 THEN 1 ELSE BTblBazdidResultCheckList.DefectionCount END) >= '" & mMinCheckListCount & "'"
                ElseIf Regex.IsMatch(mReportNo, "[2]-[1234]-[34]") Then
                    mHaving = " HAVING SUM(CASE WHEN BTblServiceCheckList.ServiceCount Is Null OR BTblServiceCheckList.ServiceCount = 0 THEN 1 ELSE BTblServiceCheckList.ServiceCount END) >=   " & mMinCheckListCount
                End If
            End If
            If mFromDate <> "''" Then
                If Regex.IsMatch(mReportNo, "[2]-[1234]-[34]") Or Regex.IsMatch(mReportNo, "[2]-[1234]-[89]") Then
                    mWhere &= " AND  BTblServiceCheckList.DoneDatePersian >= " & mFromDate
                    If Regex.IsMatch(mReportNo, "[2]-[1234]-[89]") Then
                        mWhereDT &= " BTblServiceSubCheckList.DataEntryDatePersian >= " & mFromDate
                    End If
                Else
                    mWhere &= " AND BTblBazdidResultAddress.StartDatePersian >= " & mFromDate
                End If
            End If
            If mToDate <> "''" Then
                If Regex.IsMatch(mReportNo, "[2]-[1234]-[34]") Or Regex.IsMatch(mReportNo, "[2]-[1234]-[89]") Then
                    mWhere &= " AND  BTblServiceCheckList.DoneDatePersian <= " & mToDate
                    If Regex.IsMatch(mReportNo, "[2]-[1234]-[89]") Then
                        mWhereDT &= "AND BTblServiceSubCheckList.DataEntryDatePersian <= " & mToDate
                    End If
                Else
                    mWhere &= " AND BTblBazdidResultAddress.StartDatePersian <= " & mToDate
                End If
            End If
            If mFromDateBazdid <> "''" Then
                mWhere &= " AND BTblBazdidResultAddress.StartDatePersian >= " & mFromDateBazdid
            End If
            If mToDateBazdid <> "''" Then
                mWhere &= " AND BTblBazdidResultAddress.StartDatePersian <= " & mToDateBazdid
            End If
            If mBazdidSpeciality <> "" And Regex.IsMatch(mReportNo, "2-[123]-[69]") Then
                '  mJoinSpecialitySql &= " LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId "
                mWhere &= "  AND ISNULL(tTS.BazdidSpecialityId,1) IN ( " & mBazdidSpeciality & " )"
                mJoinSpecialitySql &= " LEFT JOIN BTblTimingSpeciality tTS ON BTblBazdidTiming.BazdidTimingId = tTS.BazdidTimingId "
            End If

            If Regex.IsMatch(mReportNo, "[1]-[1234]-[34]") And mBazdidSpeciality <> "" Then
                mJoinSpecialitySql &= " LEFT JOIN BTblTimingSpeciality tTS ON BTblBazdidTiming.BazdidTimingId = tTS.BazdidTimingId "

            ElseIf Regex.IsMatch(mReportNo, "[2]-[1234]-[34]") And mBazdidSpeciality <> "" Then
                mJoinSpecialitySql &= " LEFT JOIN BTblTimingSpeciality tTS ON BTblBazdidTiming.BazdidTimingId = tTS.BazdidTimingId "

                '" LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId  " & _
            End If

            If (mMPPostId > -1 Or mAreaId <> "") And mReportNo = "2-1-6" Then
                mWhere &= " AND Tbl_MPFeeder.MPFeederId IN (SELECT MPFeederId FROM #TmpTbl_MPFeedersId)"
            End If
        End If
        If Regex.IsMatch(mReportNo, "^3-") Then
            If mFromDate <> "''" Then
                mWhereDT &= " AND BTblBazdidResultAddress.StartDatePersian >= " & mFromDate
            End If
            If mToDate <> "''" Then
                mWhereDT &= " AND BTblBazdidResultAddress.StartDatePersian <= " & mToDate
            End If
            If mBazdidMasterIDs <> "" Then
                mWhere &= " AND BTblBazdidTiming.BazdidMasterId IN ( " & mBazdidMasterIDs & " )"
            End If
        End If


        If Regex.IsMatch(mReportNo, "^4-") Then
            If mFromDate <> "''" Then
                mWhereDT &= " AND BTblServiceCheckList.DoneDatePersian >= " & mFromDate
                If Regex.IsMatch(mReportNo, "^4-2-[34]") Then
                    mWhere &= " AND BTblServiceCheckList.DoneDatePersian >= " & mFromDate
                    'mWhere &= IIf(mIsWarmLine <> "", " AND BTblService.IsWarmLine  =  " & mIsWarmLine & " ", "")
                End If
            End If
            If mToDate <> "''" Then
                mWhereDT &= " AND BTblServiceCheckList.DoneDatePersian <= " & mToDate
                If Regex.IsMatch(mReportNo, "^4-2-[34]") Then
                    mWhere &= " AND BTblServiceCheckList.DoneDatePersian <= " & mToDate
                    'mWhere &= IIf(mIsWarmLine <> "", " AND BTblService.IsWarmLine  =  " & mIsWarmLine & " ", "")
                End If
            End If
            If mBazdidMasterIDs <> "" Then
                mWhere &= " AND BTblBazdidTiming.BazdidMasterId IN ( " & mBazdidMasterIDs & " )"
            End If
        End If

        Dim lDs As New DataSet

        If Regex.IsMatch(mReportNo, "3-1-[12345]") Or mReportNo = "3-2-1" Or Regex.IsMatch(mReportNo, "4-1-[1245]") Or Regex.IsMatch(mReportNo, "4-2-1") Then
            If lLPPostIDs <> "" Then
                mWhereDT &= " AND Tbl_LPPost.LPPostId IN (" & lLPPostIDs & ") "
                mWherePost &= " AND Tbl_LPPost.LPPostId IN (" & lLPPostIDs & ") "
            ElseIf mMPFeederIDs <> "" Then
                mWhere &= " AND Tbl_MPFeeder.MPFeederId IN (" & mMPFeederIDs & ") "
                mWherePost &= " AND Tbl_MPFeeder.MPFeederId IN (" & mMPFeederIDs & ") "
            ElseIf lMPPostIDs <> "" Then
                mWhere &= " AND Tbl_MPFeeder.MPPostId IN (" & lMPPostIDs & ") "
                mWherePost &= " AND Tbl_MPFeeder.MPPostId IN (" & lMPPostIDs & ") "
            End If
            If lAreaIDs <> "" Then
                GetMPFeederIDs(lAreaIDs)
                For Each lRow As DataRow In mDs.Tables("TmpTbl_MPFeedersId").Rows
                    mMPFeederIDs &= "," & lRow("MPFeederId")
                Next
                mMPFeederIDs = Regex.Replace(mMPFeederIDs, "^,", "")
                mWhere &= " AND Tbl_MPFeeder.MPFeederId IN (" & mMPFeederIDs & ") "
                mWherePost &= " AND Tbl_MPFeeder.MPFeederId IN (" & mMPFeederIDs & ") "
            End If

            If mOwnershipId > -1 Then
                mWhere &= " AND Tbl_MPFeeder.OwnershipId = " & mOwnershipId
            End If
            If mIsActive = 1 Then
                mWhere = mWhere & " AND Tbl_MPFeeder.IsActive = 1 "
                mWhereDT &= " AND Tbl_MPFeeder.IsActive = 1 "
            End If
        ElseIf Regex.IsMatch(mReportNo, "3-2-[2345]") Or Regex.IsMatch(mReportNo, "[34]-[34]-[2]") Or Regex.IsMatch(mReportNo, "4-2-[234]") Then

            If lAreaIDs <> "" Then
                mWhere = mWhere & " AND Tbl_LPPost.AreaId IN ( " & lAreaIDs & ")"
                mWherePost = mWherePost & " AND Tbl_LPPost.AreaId IN ( " & lAreaIDs & ")"
            End If

            If lLPPostIDs <> "" Then
                mWhereDT &= " AND Tbl_LPPost.LPPostId IN (" & lLPPostIDs & ") "
                mWhere &= " AND Tbl_LPPost.LPPostId IN (" & lLPPostIDs & ") "
                mWherePost &= " AND Tbl_LPPost.LPPostId IN (" & lLPPostIDs & ") "
            ElseIf mMPFeederIDs <> "" Then
                mWhere &= " AND Tbl_LPPost.MPFeederId IN (" & mMPFeederIDs & ") "
                mWherePost &= " AND Tbl_LPPost.MPFeederId IN (" & mMPFeederIDs & ") "
            ElseIf lMPPostIDs <> "" Then
                mWhere &= " AND Tbl_MPFeeder.MPPostId IN (" & lMPPostIDs & ") "
                mWherePost &= " AND Tbl_MPFeeder.MPPostId IN (" & lMPPostIDs & ") "
            End If

            If mOwnershipId > -1 Then
                mWhere &= " AND Tbl_LPPost.OwnershipId = " & mOwnershipId
                mWherePost &= " AND Tbl_LPPost.OwnershipId = " & mOwnershipId
            End If
            If mIsActive = 1 Then
                mWhere = mWhere & " AND Tbl_LPPost.IsActive = 1 "
                mWhereDT &= " AND Tbl_LPPost.IsActive = 1 "
                mWherePost = mWherePost & " AND Tbl_LPPost.IsActive = 1 "
            End If
        ElseIf Regex.IsMatch(mReportNo, "[34]-[34]-3") Then
            If lAreaIDs <> "" Then
                If Regex.IsMatch(mReportNo, "3-[34]-3") Then
                    mWhere = mWhere & " AND Tbl_Area.AreaId IN ( " & lAreaIDs & ")"
                Else
                    mWhere = mWhere & " AND (Tbl_Area_Feeder.AreaId IN ( " & lAreaIDs & ") OR Tbl_Area_Post.AreaId IN ( " & lAreaIDs & "))"
                End If
            End If
            If lLPFeederIDs <> "" Then
                mWhere &= " AND Tbl_LPFeeder.LPFeederId IN (" & lLPFeederIDs & ") "
            ElseIf lLPPostIDs <> "" Then
                mWhere &= " AND (Tbl_LPFeeder.LPPostId IN (" & lLPPostIDs & ") OR Tbl_LPPost.LPPostId IN (" & lLPPostIDs & ")) "
            ElseIf mMPFeederIDs <> "" Then
                mWhere &= " AND (Tbl_LPPost.MPFeederId IN (" & mMPFeederIDs & ") OR Tbl_LPPost_Feeder.MPFeederId IN (" & mMPFeederIDs & ")) "
            ElseIf lMPPostIDs <> "" Then
                mWhere &= " AND (Tbl_MPFeeder.MPPostId IN (" & lMPPostIDs & ") OR Tbl_MPFeeder_Post.MPPostId IN (" & lMPPostIDs & ")) "
            End If
            If mOwnershipId > -1 Then
                mWhere &= " AND Tbl_LPFeeder.OwnershipId = " & mOwnershipId
            End If
            If mIsActive = 1 Then
                mWhere = mWhere & " AND Tbl_LPFeeder.IsActive = 1 "
                mWhereDT &= " AND Tbl_LPFeeder.IsActive = 1 "
            End If
        ElseIf Regex.IsMatch(mReportNo, "[34]-[34]-[145]") Then
            If lAreaIDs <> "" Then
                mWhere = mWhere & " AND Tbl_LPFeeder.AreaId IN ( " & lAreaIDs & ")"
            End If
            If lLPFeederIDs <> "" Then
                mWhere &= " AND Tbl_LPFeeder.LPFeederId IN (" & lLPFeederIDs & ") "
            ElseIf lLPPostIDs <> "" Then
                mWhere &= " AND Tbl_LPPost.LPPostId IN (" & lLPPostIDs & ") "
            ElseIf mMPFeederIDs <> "" Then
                mWhere &= " AND Tbl_LPPost.MPFeederId IN (" & mMPFeederIDs & ") "
            ElseIf lMPPostIDs <> "" Then
                mWhere &= " AND Tbl_MPFeeder.MPPostId IN (" & lMPPostIDs & ") "
            End If
            If mOwnershipId > -1 Then
                mWhere &= " AND Tbl_LPFeeder.OwnershipId = " & mOwnershipId
            End If
            If mIsActive = 1 Then
                mWhere = mWhere & " AND Tbl_LPFeeder.IsActive = 1 "
                mWhereDT &= " AND Tbl_LPFeeder.IsActive = 1 "
            End If
        ElseIf Regex.IsMatch(mReportNo, "4-1-3") Then
            If mMPFeederIDs <> "" Then
                mWhere &= " AND Tbl_MPFeeder.MPFeederId IN (" & mMPFeederIDs & ") "
            ElseIf lMPPostIDs <> "" Then
                mWhere &= " AND Tbl_MPFeeder.MPPostId IN (" & lMPPostIDs & ") "
            End If
            If lAreaIDs <> "" And lMPPostIDs = "" Then
                GetMPFeederIDs(lAreaIDs)
                For Each lRow As DataRow In mDs.Tables("TmpTbl_MPFeedersId").Rows
                    mMPFeederIDs &= "," & lRow("MPFeederId")
                Next
                mMPFeederIDs = Regex.Replace(mMPFeederIDs, "^,", "")
                mWhere &= " AND Tbl_MPFeeder.MPFeederId IN (" & mMPFeederIDs & ") "
            End If
            If mOwnershipId > -1 Then
                mWhere &= " AND Tbl_MPFeeder.OwnershipId = " & mOwnershipId
            End If
            If mIsActive = 1 Then
                mWhere = mWhere & " AND Tbl_MPFeeder.IsActive = 1 "
                mWhereDT &= " AND Tbl_MPFeeder.IsActive = 1 "
            End If
        End If


        If mWhere <> "" Then
            mWhere2 = " WHERE " + Regex.Replace(mWhere, "^ AND", "")
        End If

        If mWhereDT <> "" Then
            mWhereDT2 = " WHERE " + Regex.Replace(mWhereDT, "^ AND", "")
        End If

        If mWherePost <> "" Then
            mWherePost = " WHERE " + Regex.Replace(mWherePost, "^ AND", "")
        End If

        If mBazdidSpeciality <> "" Or cmbBazdidType.SelectedIndex > -1 Then

            If mBazdidSpeciality <> "" Then
                If Not Regex.IsMatch(mReportNo, "4-[34]-5") Then
                    mWhereDT &= " AND ISNULL(tTS.BazdidSpecialityId,1) IN (" & mBazdidSpeciality & ") "
                End If
            End If
            If mJoinSpecialitySql = "" Then
                mJoinSpecialitySql &= " LEFT JOIN BTblTimingSpeciality tTS ON BTblBazdidTiming.BazdidTimingId = tTS.BazdidTimingId "
                '" LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
            End If

            If mBazdidSpeciality <> "" Then
                If mReportNo <> "3-1-1" And Not Regex.IsMatch(mReportNo, "3-[34]-[12]") And Not Regex.IsMatch(mReportNo, "4-1-[12]") _
                                    And Not Regex.IsMatch(mReportNo, "4-[34]-[12]") Then
                    mWhere = mWhere & " AND ISNULL(tTS.BazdidSpecialityId,1) IN (" & mBazdidSpeciality & ") "
                End If
            End If
            mFilterInfo &= " " & cmbBazdidType.Text
            If Regex.IsMatch(mReportNo, "3-[1234]-[35]") Then
                If cmbBazdidType.SelectedIndex = 0 Then
                    mWhere &= " AND BTblBazdidTiming.BazdidAutoTimingId IS NULL"
                ElseIf cmbBazdidType.SelectedIndex = 1 Then
                    mWhere &= " AND BTblBazdidTiming.BazdidAutoTimingId IS NOT NULL "
                End If
            End If
        End If

        If mPrs <> "" Then
            mWhere = mWhere & " AND BTblBazdidResultCheckList.Priority IN (" & mPrs & ")"
        End If

        If mCheckLists <> "" Then
            If chkSpliteCheckList.Checked Then
                Dim words As String() = mCheckLists.Split(New Char() {","c})
                For i As Integer = 0 To words.Length - 1
                    mWhere &= " AND BTblBazdidResultCheckList.BazdidCheckListId  =  (" & words(i) & ") "
                Next
            Else
                mWhere &= " AND BTblBazdidResultCheckList.BazdidCheckListId IN (" & mCheckLists & ")"
            End If
        End If
        If mSubCheckLists <> "" Then
            mWhere &= " AND BTbl_SubCheckList.SubCheckListId IN (" & mSubCheckLists & " )"
        End If

        If mMinCheckListCount > 0 Then
            If Regex.IsMatch(mReportNo, "^3") Then
                mHaving = " HAVING SUM(CASE WHEN BTblBazdidResultCheckList.DefectionCount Is Null OR BTblBazdidResultCheckList.DefectionCount = 0 THEN 1 ELSE BTblBazdidResultCheckList.DefectionCount END) >= " & mMinCheckListCount
            ElseIf Regex.IsMatch(mReportNo, "^4") Then
                mHaving = " HAVING SUM(DISTINCT CASE WHEN BTblServiceCheckList.ServiceCount Is Null OR BTblServiceCheckList.ServiceCount = 0 THEN 1 ELSE BTblServiceCheckList.ServiceCount END) >= " & mMinCheckListCount
            End If
        End If

        If mParts <> "" Then
            If Regex.IsMatch(mReportNo, "4-[1234]-5") Then
                mWherePart = " AND BTbl_ServicePart.ServicePartId IN (" & mParts & ") "
            Else
                mWhere &= " AND BTbl_ServicePart.ServicePartId IN (" & mParts & ") "
            End If
        End If

    End Sub
    Private Sub MakeReport_1_1_1()
        MakeQuery()
        Dim lSQL As String = ""
        lSQL = "EXEC spGetReport_1_1_1 " & mFromDate & "," & mToDate & ",'" & mAreaId & "','" & mMPFeederIDs & "'," & mMPPostId & "," & mOwnershipId & "," & mIsHavaei & "," & mIsActive & ",'" & mBazdidMasterIDs & "','" & mBasketDetailIDs & "','" & mBazdidSpeciality & "'"
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_1_1_1", , True)
        If lDS.Tables.Contains("Report_1_1_1") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_1_1_1.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_1_1_1.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_1_1_2()
        MakeQuery()
        Dim lSQL As String = ""
        lSQL = "EXEC spGetReport_1_1_2 " & mFromDate & "," & mToDate & ",'" & mAreaId & "','" & mMPFeederIDs & "'," & mMPPostId & "," & mOwnershipId & "," & mIsHavaei & "," & mIsActive & ",'" & mBazdidMasterIDs & "','" & mBasketDetailIDs & "','" & mBazdidSpeciality & "'," & mIsNotCheckList
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_1_1_2", , True)
        If lDS.Tables.Contains("Report_1_1_2") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_1_1_2.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_1_1_2.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_1_1_3()
        MakeQuery()
        Dim lSQL As String = ""
        Dim lHaving As String = ""

        'lSQL = "EXEC spGetReport_1_1_3 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mOwnershipId & "," & mIsHavaei & "," & mIsActive & ",'" & mBazdidMasterIDs & "','" & mBasketDetailIDs & "','" & mPrs & "','" & mCheckLists & "','" & mAddress & "'," & mMinCheckListCount & ",'" & mBazdidSpeciality & "','" & cmbBazdidType.SelectedIndex & "'"

        lSQL = "    CREATE TABLE #TmpTbl_MPFeedersId " &
                    " 	(MPFeederId int)" &
    " INSERT #TmpTbl_MPFeedersId EXEC spGetMPFeedersId  '" & mAreaId & "' ,'" & mMPPostId & "' " &
    " 	SELECT  Tbl_MPFeeder.AreaId, " &
            " 	Tbl_Area.Area, " &
            " 	Tbl_MPPost.MPPostName, " &
            " 	Tbl_MPFeeder.MPFeederName, " &
            " 	Tbl_MPFeeder.MPFeederId, " &
            " 	ISNULL(FromToLengthHavayi,Tbl_MPFeeder.HavaeiLength) AS HavaeiLength, " &
            " 	ISNULL(FromToLengthZamini,Tbl_MPFeeder.ZeminiLength) AS ZeminiLength, " &
            " 	BTblbazdidResult.FromPathTypeId, " &
            " 	Tbl_PathType_From.PathType AS FromPathType, " &
            " 	BTblbazdidResult.FromPathTYpeValue, " &
            " 	BTblbazdidResult.ToPathTypeId, " &
            " 	Tbl_PathType_To.PathType AS ToPathType, " &
            " 	BTblbazdidResult.ToPathTypeValue, " &
            " 	BTblBazdidResultAddress.StartDatePersian, " &
            " 	BTblBazdidResultAddress.Address, " &
            " 	BTblBazdidResultAddress.GPSx, " &
            " 	BTblBazdidResultAddress.GPSy, " &
            " 	BTbl_BazdidCheckList.CheckListCode, " &
            " 	BTbl_BazdidCheckList.CheckListName, " &
            " 	Tbl_FeederPart.FeederPart, " &
            " 	CASE WHEN BTblBazdidResultCheckList.Priority = 4 THEN  " &
            " 		CAST('عدم وجود تجهيز' as nvarchar)  " &
            " 		ELSE CAST(BTblBazdidResultCheckList.Priority as nvarchar) END AS Priority, " &
            " 		SUM(CASE WHEN BTblBazdidResultCheckList.DefectionCount Is Null OR BTblBazdidResultCheckList.DefectionCount = 0 THEN 1 ELSE BTblBazdidResultCheckList.DefectionCount END) AS CheckListCount, " &
            " 	BTbl_BazdidCheckListGroup.IsHavayi, " &
            " 	BTblBazdidTiming.BazdidTimingName, " &
            " 	BTblBazdidTiming.BazdidName, " &
            " 	BTblBazdidResultCheckList.Comment, " &
            " 	CASE " &
            " 		WHEN BTblBazdidTiming.BazdidAutoTimingId IS NULL THEN " &
                " 	'موردي' " &
                " 		 WHEN BTblBazdidTiming.BazdidAutoTimingId IS NOT NULL THEN " &
                " 		 'دوره اي' " &
            " 	END AS BazdidAutoTiming " &
    " 	FROM  	Tbl_MPFeeder " &
            " 	INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            " 	INNER JOIN BTblbazdidResult  ON Tbl_MPFeeder.MPFeederId = BTblbazdidResult.MPFeederId " &
            " 	INNER JOIN Tbl_Area ON BTblbazdidResult.AreaId = Tbl_Area.AreaId " &
            " 	INNER JOIN BTblBazdidResultAddress  ON BTblbazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
            " 	INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
            " 	LEFT OUTER JOIN Tbl_PathType Tbl_PathType_From ON BTblbazdidResult.FromPathTypeId = Tbl_PathType_From.PathTypeId " &
            " 	LEFT OUTER JOIN Tbl_PathType Tbl_PathType_To ON BTblbazdidResult.ToPathTypeId = Tbl_PathType_To.PathTypeId " &
            " 	INNER JOIN BTbl_BazdidCheckList ON BTblBazdidResultCheckList.BazdidCheckListId = BTbl_BazdidCheckList.BazdidCheckListId " &
            " 	LEFT OUTER JOIN BTblBazdidTiming ON BTblbazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
            " 	LEFT JOIN Tbl_FeederPart On BTblbazdidResult.FeederPartId = Tbl_FeederPart.FeederPartId " &
            " 	LEFT JOIN BTbl_BazdidCheckListGroup ON BTbl_BazdidCheckList.BazdidCheckListGroupId = BTbl_BazdidCheckListGroup.BazdidCheckListGroupId " &
            " " & mJoinSpecialitySql & " " &
            " 	WHERE  " &
                             " 		BTblbazdidResult.BazdidStateId IN (2,3) " &
                             " 		AND BTblbazdidResult.BazdidTypeId = 1 " &
                             " 		AND BTblBazdidResultCheckList.Priority > 0 " &
                            " 	" & mWhere &
                            " 		GROUP BY " &
                            " 		Tbl_MPFeeder.AreaId, " &
                            " 		Tbl_Area.Area, " &
                            " 		Tbl_MPPost.MPPostName, " &
                            " 		Tbl_MPFeeder.MPFeederName, " &
                            " 		Tbl_MPFeeder.MPFeederId, " &
                            " 		ISNULL(FromToLengthHavayi,Tbl_MPFeeder.HavaeiLength), " &
                            " 		ISNULL(FromToLengthZamini,Tbl_MPFeeder.ZeminiLength), " &
                            " 		BTblbazdidResult.FromPathTypeId, " &
                            " 		Tbl_PathType_From.PathType, " &
                            " 		BTblbazdidResult.FromPathTYpeValue, " &
                            " 		BTblbazdidResult.ToPathTypeId, " &
                            " 		Tbl_PathType_To.PathType, " &
                            " 		BTblbazdidResult.ToPathTypeValue, " &
                            " 		BTblBazdidResultAddress.StartDatePersian, " &
                        " 		BTblBazdidResultAddress.Address, " &
                        " 		BTblBazdidResultAddress.GPSx, " &
                        " 		BTblBazdidResultAddress.GPSy, " &
                        " 		BTbl_BazdidCheckList.CheckListCode, " &
                        " 		BTbl_BazdidCheckList.CheckListName, " &
                        " 		BTblBazdidResultCheckList.Priority, " &
                        " 		Tbl_FeederPart.FeederPart, " &
                        " 		BTbl_BazdidCheckListGroup.IsHavayi, BTblBazdidTiming.BazdidTimingName,BTblBazdidTiming.BazdidName, " &
                        " 		BTblBazdidTiming.BazdidAutoTimingId,BTblBazdidResultCheckList.Comment  " &
            " 		" & lHaving &
    " 	DROP TABLE #TmpTbl_MPFeedersId	"

        '"	" & mWhere &
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_1_1_3", , True)
        If lDS.Tables.Contains("Report_1_1_3") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_1_1_3.xml", XmlWriteMode.WriteSchema)
            Dim lDlg As New frmReportPreviewStim("", lFolder & "Report_1_1_3.mrt", "" & cmbReportName.Text & "", "" & cmbReportName.Text & "", , mFilterInfo, mFF, "1-1-3", , , lDS)
            lDlg.Show()
        End If
    End Sub
    Private Sub MakeReport_1_1_4()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_1_1_4 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mOwnershipId & "," & mIsHavaei & "," & mIsActive & ",'" & mBazdidMasterIDs & "','" & mBasketDetailIDs & "','" & mPrs & "','" & mCheckLists & "','" & mAddress & "','" & mSubCheckLists & "','" & mBazdidSpeciality & "','" & cmbBazdidType.SelectedIndex & "'"

        Dim lDS As New DataSet

        lSQL = " CREATE TABLE #TmpTbl_MPFeedersId " &
                  "	( " &
                  "		MPFeederId int ) " &
                  "	INSERT #TmpTbl_MPFeedersId EXEC spGetMPFeedersId  '" & mAreaId & "' ,'" & mMPPostId & "' " &
                  " SELECT  " &
                    " Tbl_MPFeeder.AreaId, " &
                      "	Tbl_Area.Area, " &
                      "	Tbl_MPPost.MPPostName, " &
                      "	Tbl_MPFeeder.MPFeederName, " &
                      "	Tbl_MPFeeder.MPFeederId, " &
                      "	BTblBazdidResult.FromToLengthHavayi, " &
                      "	Tbl_MPFeeder.HavaeiLength AS MPFeederHavayiLen, " &
                      "	BTblBazdidResult.FromToLengthZamini, " &
                      "	Tbl_MPFeeder.ZeminiLength AS MPFeederZaminiLen, " &
                      "	ISNULL(BTblBazdidResult.FromToLengthHavayi,Tbl_MPFeeder.HavaeiLength) AS HavaeiLength, " &
                      "	ISNULL(BTblBazdidResult.FromToLengthZamini,Tbl_MPFeeder.ZeminiLength) AS ZeminiLength, " &
                      "	ISNULL(BTbl_SubCheckList_New.SubCheckListCode,BTbl_SubCheckList_Old.SubCheckListCode) AS SubCheckListCode, " &
                      "	ISNULL(BTbl_SubCheckList_New.SubCheckListName,BTbl_SubCheckList_Old.SubCheckListName) AS SubCheckListName, " &
                      "	BTblBazdidResult.FromPathTypeId, " &
                      "	Tbl_PathType_From.PathType AS FromPathType, " &
                      "	BTblBazdidResult.FromPathTYpeValue, " &
                      "	BTblBazdidResult.ToPathTypeId, " &
                      "	Tbl_PathType_To.PathType AS ToPathType, " &
                      "	BTblBazdidResult.ToPathTypeValue, " &
                      "	BTblBazdidResultAddress.Address, " &
                      "	BTblBazdidResultAddress.GPSx, " &
                      "	BTblBazdidResultAddress.GPSy, " &
                      "	BTbl_BazdidCheckList.CheckListCode, " &
                      "	BTbl_BazdidCheckList.CheckListName, " &
                      "	BTblBazdidResultCheckList.Priority, " &
                      "	BTblBazdidResultCheckList.BazdidResultCheckListId, " &
                      "	Tbl_FeederPart.FeederPart, " &
                      "	BTbl_BazdidCheckListGroup.IsHavayi, " &
                      "	ISNULL(BTblBazdidResultSubCheckList.SubCheckListId,BTblBazdidResultCheckList.SubCheckListId) AS SubCheckListId, " &
                      "	BTblBazdidTiming.BazdidName, " &
                      "	CASE " &
                      "		 WHEN BTblBazdidTiming.BazdidAutoTimingId IS NULL THEN " &
                      "	   'موردي' " &
                        "	 WHEN BTblBazdidTiming.BazdidAutoTimingId IS NOT NULL THEN " &
                        "	   'دوره اي' " &
                    "	END AS BazdidAutoTiming   " &
        " 	FROM  	Tbl_MPFeeder " &
            "	INNER JOIN BTblBazdidResult ON Tbl_MPFeeder.MPFeederId = BTblBazdidResult.MPFeederId " &
            "	INNER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId " &
            "	INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
            "	INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
            "	LEFT OUTER JOIN BTblBazdidResultSubCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblBazdidResultSubCheckList.BazdidResultCheckListId " &
            "	LEFT OUTER JOIN Tbl_PathType Tbl_PathType_From ON BTblBazdidResult.FromPathTypeId = Tbl_PathType_From.PathTypeId " &
            "	LEFT OUTER JOIN Tbl_PathType Tbl_PathType_To ON BTblBazdidResult.ToPathTypeId = Tbl_PathType_To.PathTypeId " &
            "	LEFT OUTER JOIN BTbl_BazdidCheckList ON BTblBazdidResultCheckList.BazdidCheckListId = BTbl_BazdidCheckList.BazdidCheckListId " &
            "	LEFT OUTER JOIN BTbl_SubCheckList AS BTbl_SubCheckList_Old ON BTblBazdidResultCheckList.SubCheckListId = BTbl_SubCheckList_Old.SubCheckListId " &
            "	LEFT OUTER JOIN BTbl_SubCheckList AS BTbl_SubCheckList_New ON BTblBazdidResultSubCheckList.SubCheckListId = BTbl_SubCheckList_New.SubCheckListId " &
            "	LEFT OUTER JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
            "	LEFT OUTER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "	LEFT JOIN Tbl_FeederPart On BTblBazdidResult.FeederPartId = Tbl_FeederPart.FeederPartId " &
            "	LEFT JOIN BTbl_BazdidCheckListGroup ON BTbl_BazdidCheckList.BazdidCheckListGroupId = BTbl_BazdidCheckListGroup.BazdidCheckListGroupId " &
            "	" & mJoinSpecialitySql &
        " WHERE  " &
            "	BTblBazdidResult.BazdidStateId IN (2,3) " &
            "	AND BTblBazdidResult.BazdidTypeId = 1 " &
            "	AND BTblBazdidResultCheckList.Priority > 0 " &
           "	" & mWhere.Replace("BTbl_SubCheckList.SubCheckListId", "ISNULL(BTbl_SubCheckList_New.SubCheckListId,BTbl_SubCheckList_Old.SubCheckListId)") &
        "	DROP TABLE #TmpTbl_MPFeedersId  "


        BindingTable(lSQL, mCnn, lDS, "Report_1_1_4", , True)
        If lDS.Tables.Contains("Report_1_1_4") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_1_1_4.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_1_1_4.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_1_1_5()
        MakeQuery()
        Dim lSQL As String = ""
        lSQL = "EXEC spGetReport_1_1_5 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & ",'" & mMPFeederIDs & "'," & mOwnershipId & "," & mIsHavaei & "," & mIsActive & ",'" & mBazdidMasterIDs & "','" & mBasketDetailIDs & "','" & mParts & "','" & mAddress & "','" & mBazdidSpeciality & "','" & cmbBazdidType.SelectedIndex & "'"
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_1_1_5", , True)
        If lDS.Tables.Contains("Report_1_1_5") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_1_1_5.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_1_1_5.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_1_1_6()
        MakeQuery()
        Dim lSQL As String = ""
        lSQL = "EXEC spGetReport_1_1_6 " & mFromDate & "," & mToDate & ",'" & mAreaId & "','" & mMPFeederIDs & "'," & mMPPostId & "," & mOwnershipId & "," & mIsHavaei & "," & mIsActive & ",'" & mBazdidMasterIDs & "','" & mBasketDetailIDs & "','" & mBazdidSpeciality & "'"
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_1_1_6", , True)
        If lDS.Tables.Contains("Report_1_1_6") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_1_1_6.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_1_1_6.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_1_1_7()
        MakeQuery()
        Dim lSQL As String = ""
        lSQL = "EXEC spGetReport_1_1_7 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & ",'" & mMPFeederIDs & "'," & mOwnershipId & "," & mIsHavaei & "," & mIsActive & "," & mIsExistMap & "," & mIsUpdateMap & "," & mIsOKNetDesign & "," & IIf(chkIsNoValue.Checked, 1, 0) & ",'" & mBazdidSpeciality & "'"
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_1_1_7", , True)
        If lDS.Tables.Contains("Report_1_1_7") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_1_1_7.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_1_1_7.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_1_2_1()
        MakeQuery()
        Dim lSQL As String = ""
        lSQL = "EXEC spGetReport_1_2_1 " & mFromDate & "," & mToDate & ",'" & mAreaId & "','" & mMPFeederIDs & "'," & mMPPostId & "," & mOwnershipId & "," & mIsHavaei & "," & mIsActive & ",'" & mBazdidMasterIDs & "','" & mBasketDetailIDs & "','" & mBazdidSpeciality & "'"
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_1_2_1", , True)
        If lDS.Tables.Contains("Report_1_2_1") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_1_2_1.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_1_2_1.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_1_2_2()
        MakeQuery()
        Dim lSQL As String = ""
        lSQL = "EXEC spGetReport_1_2_2 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mLPPostId & ",'" & mMPFeederIDs & "'," & mMPPostId & "," & mOwnershipId & "," & mIsHavaei & "," & mIsActive & ",'" & mBazdidMasterIDs & "','" & mBasketDetailIDs & "','" & mBazdidSpeciality & "'," & mIsNotCheckList
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_1_2_2", , True)
        If lDS.Tables.Contains("Report_1_2_2") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_1_2_2.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_1_2_2.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_1_2_3()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_1_2_3 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mLPPostId & "," & mOwnershipId & "," & mIsHavaei & "," & mIsActive & ",'" & mBazdidMasterIDs & "','" & mBasketDetailIDs & "','" & mPrs & "','" & mCheckLists & "','" & mAddress & "'," & mMinCheckListCount & ",'" & mBazdidSpeciality & "','" & cmbBazdidType.SelectedIndex & "'"
        Dim lDS As New DataSet

        lSQL = "SELECT DISTINCT " &
        "	Tbl_Area.AreaId, " &
        "	Tbl_Area.Area, " &
        "	Tbl_MPPost.MPPostName, " &
        "	Tbl_MPFeeder.MPFeederName, " &
        "	Tbl_MPFeeder.MPFeederId, " &
        "	Tbl_LPPost.LPPostName, " &
        "	Tbl_LPPost.LPPostId, " &
        "	Tbl_LPPost.Address, " &
        "	Tbl_LPPost.LPPostCode, " &
        "	BTblBazdidResultAddress.GPSx, " &
        "	BTblBazdidResultAddress.GPSy, " &
        "	BTbl_BazdidCheckList.CheckListCode, " &
        "	BTbl_BazdidCheckList.CheckListName, " &
        "	CASE WHEN BTblBazdidResultCheckList.Priority = 4 THEN  " &
        "	CAST('عدم وجود تجهيز' as nvarchar)  " &
        "		ELSE CAST(BTblBazdidResultCheckList.Priority as nvarchar) END AS Priority, " &
        "		SUM(CASE WHEN BTblBazdidResultCheckList.DefectionCount Is Null OR BTblBazdidResultCheckList.DefectionCount = 0 THEN 1 ELSE BTblBazdidResultCheckList.DefectionCount END) AS CheckListCount, " &
        "	BTblBazdidTiming.BazdidTimingName, " &
        "	BTblBazdidTiming.BazdidName , " &
        "	CASE " &
        "	WHEN BTblBazdidTiming.BazdidAutoTimingId IS NULL THEN " &
        "		'موردي'" &
        "	 WHEN BTblBazdidTiming.BazdidAutoTimingId IS NOT NULL THEN" &
        "		'دوره اي' " &
        "	END AS BazdidAutoTiming  , " &
        "	BTblBazdidResultCheckList.Comment " &
    "	FROM  Tbl_LPPost " &
        "	INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
        "   INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
        "	INNER JOIN BTblBazdidResult ON Tbl_LPPost.LPPostId = BTblBazdidResult.LPPostId " &
        "	INNER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId " &
        "	INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
        "	INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
        "	INNER JOIN BTbl_BazdidCheckList ON BTblBazdidResultCheckList.BazdidCheckListId = BTbl_BazdidCheckList.BazdidCheckListId " &
        "	LEFT OUTER JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
        "	" & mJoinSpecialitySql & " " &
    "	WHERE  	BTblBazdidResult.BazdidStateId IN (2,3) " &
        "	AND BTblBazdidResult.BazdidTypeId = 2 " &
        "	AND BTblBazdidResultCheckList.Priority > 0 " &
        " 	" & mAddress & " " &
        "	" & mWhere &
        "	GROUP BY " &
        "	Tbl_Area.AreaId, " &
        "	Tbl_Area.Area, " &
        "	Tbl_MPPost.MPPostName, " &
        "	Tbl_MPFeeder.MPFeederName, " &
        "	Tbl_MPFeeder.MPFeederId, " &
        "	Tbl_LPPost.LPPostName, " &
        "	Tbl_LPPost.LPPostId, " &
        "	Tbl_LPPost.Address, " &
        "	BTblBazdidResultAddress.GPSx, " &
        "	BTblBazdidResultAddress.GPSy, " &
        "	BTbl_BazdidCheckList.CheckListCode, " &
        "	BTbl_BazdidCheckList.CheckListName, " &
        "	BTblBazdidResultCheckList.Priority, " &
        "	BTblBazdidTiming.BazdidTimingName, " &
        "	Tbl_LPPost.LPPostCode,BTblBazdidTiming.BazdidName, " &
        "	BTblBazdidTiming.BazdidAutoTimingId , " &
        "	BTblBazdidResultCheckList.Comment " &
        "	" & mHaving

        BindingTable(lSQL, mCnn, lDS, "Report_1_2_3", , True)
        If lDS.Tables.Contains("Report_1_2_3") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_1_2_3.xml", XmlWriteMode.WriteSchema)
            Dim lDlg As New frmReportPreviewStim("", lFolder & "Report_1_2_3.mrt", "" & cmbReportName.Text & "", "" & cmbReportName.Text & "", , mFilterInfo, mFF, "1-2-3", , , lDS)
            lDlg.Show()
        End If
    End Sub
    Private Sub MakeReport_1_2_4()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_1_2_4 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mLPPostId & "," & mOwnershipId & "," & mIsHavaei & "," & mIsActive & ",'" & mBazdidMasterIDs & "','" & mBasketDetailIDs & "','" & mPrs & "','" & mCheckLists & "','" & mAddress & "','" & mSubCheckLists & "','" & mBazdidSpeciality & "','" & cmbBazdidType.SelectedIndex & "'"
        Dim lDS As New DataSet

        lSQL = " 		SELECT " &
                        " Tbl_Area.AreaId, " &
                        "	Tbl_Area.Area, " &
                        "	Tbl_MPPost.MPPostName, " &
                        "	Tbl_MPFeeder.MPFeederName, " &
                        "	Tbl_MPFeeder.MPFeederId, " &
                        "	Tbl_LPPost.LPPostName, " &
                        "	Tbl_LPPost.LPPostId, " &
                        "	Tbl_LPPost.Address, " &
                        "	BTblBazdidResultAddress.GPSx, " &
                        "	BTblBazdidResultAddress.GPSy, " &
                        "	BTbl_BazdidCheckList.CheckListCode, " &
                        "	BTbl_BazdidCheckList.CheckListName, " &
                        "	BTblBazdidResultCheckList.Priority, " &
                        "	ISNULL(BTbl_SubCheckList_New.SubCheckListCode,BTbl_SubCheckList_Old.SubCheckListCode) AS SubCheckListCode, " &
                        "	ISNULL(BTbl_SubCheckList_New.SubCheckListName,BTbl_SubCheckList_Old.SubCheckListName) AS SubCheckListName, " &
                        "	BTblBazdidTiming.BazdidName , " &
                        "	CASE " &
                        "	 WHEN BTblBazdidTiming.BazdidAutoTimingId IS NULL THEN " &
                        "		'موردي' " &
                        "	 WHEN BTblBazdidTiming.BazdidAutoTimingId IS NOT NULL THEN " &
                        "		'دوره اي' " &
                        "	END AS BazdidAutoTiming   " &
            " FROM  	Tbl_LPPost " &
                        "	INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
                        "   INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
                        "	INNER JOIN BTblBazdidResult ON Tbl_LPPost.LPPostId = BTblBazdidResult.LPPostId " &
                        "	INNER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId " &
                        "	INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
                        "	INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
                        "	LEFT OUTER JOIN BTblBazdidResultSubCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblBazdidResultSubCheckList.BazdidResultCheckListId " &
                        "	INNER JOIN BTbl_BazdidCheckList ON BTblBazdidResultCheckList.BazdidCheckListId = BTbl_BazdidCheckList.BazdidCheckListId " &
                        "	LEFT OUTER JOIN BTbl_SubCheckList AS BTbl_SubCheckList_Old ON BTblBazdidResultCheckList.SubCheckListId = BTbl_SubCheckList_Old.SubCheckListId " &
                        "	LEFT OUTER JOIN BTbl_SubCheckList AS BTbl_SubCheckList_New ON BTblBazdidResultSubCheckList.SubCheckListId = BTbl_SubCheckList_New.SubCheckListId " &
                        "	LEFT OUTER JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
        "	" & mJoinSpecialitySql &
        " WHERE  " &
                        "	BTblBazdidResult.BazdidStateId IN (2,3) " &
                        "	AND BTblBazdidResult.BazdidTypeId = 2 " &
                        "	AND BTblBazdidResultCheckList.Priority > 0 " &
                        "	 " & mAddress & " " & mWhere.Replace("BTbl_SubCheckList.SubCheckListId", "ISNULL(BTbl_SubCheckList_New.SubCheckListId,BTbl_SubCheckList_Old.SubCheckListId)")

        '"	AND " & _
        '        "	( " & _
        '        "		NOT ISNULL(BTbl_SubCheckList_New.SubCheckListCode,BTbl_SubCheckList_Old.SubCheckListCode) IS NULL " & _
        '        "		OR NOT ISNULL(BTbl_SubCheckList_New.SubCheckListName,BTbl_SubCheckList_Old.SubCheckListName) IS NULL )" & _
        BindingTable(lSQL, mCnn, lDS, "Report_1_2_4", , True)
        If lDS.Tables.Contains("Report_1_2_4") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_1_2_4.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_1_2_4.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_1_2_5()
        MakeQuery()
        Dim lSQL As String = ""
        lSQL = "EXEC spGetReport_1_2_5 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & ",'" & mMPFeederIDs & "'," & mLPPostId & "," & mOwnershipId & "," & mIsHavaei & "," & mIsActive & ",'" & mBazdidMasterIDs & "','" & mBasketDetailIDs & "','" & mParts & "','" & mAddress & "','" & mBazdidSpeciality & "','" & cmbBazdidType.SelectedIndex & "'"
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_1_2_5", , True)
        If lDS.Tables.Contains("Report_1_2_5") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_1_2_5.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_1_2_5.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_1_2_8()
        MakeQuery()
        Dim lSQL As String = ""
        lSQL = "EXEC spGetReport_1_2_8 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & ",'" & mMPFeederIDs & "'," & mIsActive & ",'" & mCheckLists & "'"
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_1_2_8", , , , , , , True)
        If lDS.Tables.Contains("Report_1_2_8") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_1_2_8.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_1_2_8.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_1_2_9()
        MakeQuery()
        Dim lSQL As String = ""
        lSQL = "EXEC spGetReport_1_2_9 " & mFromDate & "," & mToDate & ",'" & mAreaId & "','" & mMPFeederIDs & "'," & mMPPostId & "," & mOwnershipId & "," & mIsHavaei & "," & mIsActive & ",'" & mBazdidMasterIDs & "','" & mBasketDetailIDs & "'," & mLPPostId & ",'" & mBazdidSpeciality & "'"
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_1_2_9", , True)
        If lDS.Tables.Contains("Report_1_2_9") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_1_2_9.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_1_2_9.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_1_34_1()
        MakeQuery()
        Dim lSQL As String = ""
        lSQL = "EXEC spGetReport_1_34_1 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & ",'" & mMPFeederIDs & "'," & mLPPostId & "," & mLPFeederId & "," & mOwnershipId & "," & mIsHavaei & "," & mIsActive & "," & IIf(mIsLightReport, 1, 0) & ",'" & mBazdidMasterIDs & "','" & mBasketDetailIDs & "','" & mBazdidSpeciality & "'," & mIsNotCheckList
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_1_34_1", , True)
        If lDS.Tables.Contains("Report_1_34_1") Then
            Dim lFolder As String = "Reports\Bazdid\"
            mFF.AddFormulaFields("FeederType", IIf(mIsLightReport, """ فيدر روشنايي معابر """, """ فيدر فشار ضعيف """))
            lDS.WriteXml(ReportsXMLPath & "Report_1_34_1.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & IIf(chkSeprateBasket.Checked, "Report_1_34_1_SeprateBasket.rpt", "Report_1_34_1.rpt"), "", """" & IIf(chkSeprateBasket.Checked, cmbReportName.Text & " به تفکيک مسير در هر صفحه", cmbReportName.Text) & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_1_34_2()
        MakeQuery()
        Dim lSQL As String = ""
        lSQL = "EXEC spGetReport_1_34_2 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & ",'" & mMPFeederIDs & "'," & mLPPostId & "," & mLPFeederId & "," & mOwnershipId & "," & mIsHavaei & "," & mIsActive & "," & IIf(mIsLightReport, 1, 0) & ",'" & mBazdidMasterIDs & "','" & mBasketDetailIDs & "','" & mLPPOstCode & "','" & mLpFeederLengt & "','" & mBazdidSpeciality & "'"

        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_1_34_2", , True)
        If lDS.Tables.Contains("Report_1_34_2") Then
            Dim lFolder As String = "Reports\Bazdid\"
            mFF.AddFormulaFields("FeederType", IIf(mIsLightReport, """ فيدر روشنايي معابر """, """ فيدر فشار ضعيف """))
            lDS.WriteXml(ReportsXMLPath & "Report_1_34_2.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_1_34_2.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_1_34_3()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_1_34_3 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mLPPostId & "," & mLPFeederId & "," & mOwnershipId & "," & mIsHavaei & "," & mIsActive & "," & IIf(mIsLightReport, 1, 0) & ",'" & mBazdidMasterIDs & "','" & mBasketDetailIDs & "','" & mPrs & "','" & mCheckLists & "','" & mAddress & "'," & mMinCheckListCount & ",'" & mBazdidSpeciality & "','" & cmbBazdidType.SelectedIndex & "'"
        Dim lDS As New DataSet

        lSQL = "	SELECT " &
        "   Tbl_Area.AreaId, " &
        "	Tbl_Area.Area, " &
        "	Tbl_LPFeeder.LPPostId, " &
        "	Tbl_LPPost.LPPostName, " &
        "	Tbl_LPPost.LPPostCode, " &
        "	Tbl_LPFeeder.LPFeederId, " &
        "	Tbl_LPFeeder.LPFeederName, " &
        "    Tbl_LPFeeder.Address as LPFeederAddress ,  " &
        "	BTblBazdidResult.BazdidResultId, " &
        "	ISNULL(Tbl_LPFeeder.HavaeiLength,0) AS LPFeederHavaeiLen, " &
        "	ISNULL(Tbl_LPFeeder.ZeminiLength,0) AS LPFeederZeminiLen, " &
        "	ISNULL(FromToLengthHavayi,Tbl_LPFeeder.HavaeiLength) AS HavaeiLength, " &
        "	ISNULL(FromToLengthZamini,Tbl_LPFeeder.ZeminiLength) AS ZeminiLength, " &
        "	BTblBazdidResult.FromPathTypeId, " &
        "	Tbl_P_From.PathType AS FromPathType, " &
        "	BTblBazdidResult.FromPathTYpeValue, " &
        "	BTblBazdidResult.ToPathTypeId, " &
        "	Tbl_P_To.PathType AS ToPathType, " &
        "	BTblBazdidResult.ToPathTypeValue, " &
        "	BTblBazdidResultAddress.Address, " &
        "	BTblBazdidResultAddress.StartDatePersian, " &
        "	BTblBazdidResultAddress.GPSx, " &
        "	BTblBazdidResultAddress.GPSy, " &
        "	BTbl_BazdidCheckList.CheckListCode, " &
        "	BTbl_BazdidCheckList.CheckListName, " &
        "	Tbl_FeederPart.FeederPart, " &
        "	CASE WHEN BTblBazdidResultCheckList.Priority = 4 THEN  " &
        "	CAST('عدم وجود تجهيز' as nvarchar)  " &
        "	ELSE CAST(BTblBazdidResultCheckList.Priority as nvarchar) END AS Priority, " &
        "	Tbl_LPFeeder.IsLightFeeder, " &
        "	BTbl_BazdidCheckListGroup.IsHavayi, " &
        "	SUM(CASE WHEN BTblBazdidResultCheckList.DefectionCount Is Null OR BTblBazdidResultCheckList.DefectionCount = 0 THEN 1 ELSE BTblBazdidResultCheckList.DefectionCount END) AS CheckListCount, " &
        "	BTblBazdidTiming.BazdidTimingName, " &
        "	BTblBazdidTiming.BazdidName, " &
        "	CASE " &
        "	 WHEN BTblBazdidTiming.BazdidAutoTimingId IS NULL THEN " &
        "		'موردي' " &
        "	 WHEN BTblBazdidTiming.BazdidAutoTimingId IS NOT NULL THEN " &
        "		'دوره اي' " &
        "	END AS BazdidAutoTiming  , " &
        "	BTblBazdidResultCheckList.Comment   " &
    "	FROM  " &
        "	Tbl_LPFeeder " &
        "	INNER JOIN Tbl_LPPost ON Tbl_LPFeeder.LPPostId = Tbl_LPPost.LPPostId " &
        "	INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
        "	INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
        "	INNER JOIN BTblBazdidResult ON Tbl_LPFeeder.LPFeederId = BTblBazdidResult.LPFeederId " &
        "	INNER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId " &
        "	INNER JOIN BTblBazdidResultAddress  ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
        "	INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
        "	LEFT OUTER JOIN Tbl_PathType Tbl_P_From ON BTblBazdidResult.FromPathTypeId = Tbl_P_From.PathTypeId " &
        "	LEFT OUTER JOIN Tbl_PathType Tbl_P_To ON BTblBazdidResult.ToPathTypeId = Tbl_P_To.PathTypeId " &
        "	INNER JOIN BTbl_BazdidCheckList ON BTblBazdidResultCheckList.BazdidCheckListId = BTbl_BazdidCheckList.BazdidCheckListId " &
        "	LEFT OUTER JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
        "	LEFT JOIN Tbl_FeederPart On BTblBazdidResult.FeederPartId = Tbl_FeederPart.FeederPartId " &
        "	LEFT JOIN BTbl_BazdidCheckListGroup ON BTbl_BazdidCheckList.BazdidCheckListGroupId = BTbl_BazdidCheckListGroup.BazdidCheckListGroupId " &
        "	" & mJoinSpecialitySql & " " &
    "	WHERE  " &
        "	BTblBazdidResult.BazdidStateId IN (2,3) " &
        "	AND BTblBazdidResult.BazdidTypeId = 3 " &
        "	AND BTblBazdidResultCheckList.Priority > 0 " &
        "	AND Tbl_LPFeeder.IsLightFeeder = " & IIf(mIsLightReport, 1, 0) &
        "	" & mAddress & " " &
        "	" & mWhere &
         " GROUP BY " &
        "	Tbl_Area.AreaId, " &
        "	Tbl_Area.Area, " &
        "	Tbl_LPFeeder.LPPostId, " &
        "	Tbl_LPPost.LPPostName, " &
        "	Tbl_LPFeeder.LPFeederId, " &
        "	Tbl_LPFeeder.LPFeederName, " &
        "	BTblBazdidResult.BazdidResultId, " &
        "	ISNULL(FromToLengthHavayi,Tbl_LPFeeder.HavaeiLength), " &
        "	ISNULL(FromToLengthZamini,Tbl_LPFeeder.ZeminiLength), " &
        "	BTblBazdidResult.FromPathTypeId, " &
        "	Tbl_P_From.PathType, " &
        "	BTblBazdidResult.FromPathTYpeValue, " &
        "	BTblBazdidResult.ToPathTypeId, " &
        "	Tbl_P_To.PathType, " &
        "	BTblBazdidResult.ToPathTypeValue, " &
        "	BTblBazdidResultAddress.Address, " &
        "	BTblBazdidResultAddress.StartDatePersian, " &
        "	BTblBazdidResultAddress.GPSx, " &
        "	BTblBazdidResultAddress.GPSy, " &
        "	BTbl_BazdidCheckList.CheckListCode, " &
        "	BTbl_BazdidCheckList.CheckListName, " &
        "	BTblBazdidResultCheckList.Priority, " &
        "	ISNULL(Tbl_LPFeeder.HavaeiLength,0), " &
        "	ISNULL(Tbl_LPFeeder.ZeminiLength,0), " &
        "	Tbl_FeederPart.FeederPart, " &
        "	Tbl_LPFeeder.IsLightFeeder, " &
        "	Tbl_LPPost.LPPostCode, " &
        "	BTbl_BazdidCheckListGroup.IsHavayi, " &
        "	BTblBazdidTiming.BazdidTimingName,BTblBazdidTiming.BazdidName, " &
        "	BTblBazdidTiming.BazdidAutoTimingId , " &
        "	BTblBazdidResultCheckList.Comment   ,  " &
        "    Tbl_LPFeeder.Address   " &
         "  " & mHaving

        BindingTable(lSQL, mCnn, lDS, "Report_1_34_3", , True)
        If lDS.Tables.Contains("Report_1_34_3") Then
            Dim lFolder As String = "Reports\Bazdid\"
            mFF.AddFormulaFields("FeederType", IIf(mIsLightReport, " فيدر روشنايي معابر ", " فيدر فشار ضعيف "))
            lDS.WriteXml(ReportsXMLPath & "Report_1_34_3.xml", XmlWriteMode.WriteSchema)
            Dim lDlg As New frmReportPreviewStim("", lFolder & "Report_1_34_3.mrt", "" & cmbReportName.Text & "", "" & cmbReportName.Text & "", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), , , lDS)
            lDlg.Show()
        End If
    End Sub
    Private Sub MakeReport_1_34_4()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_1_34_4 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mLPPostId & "," & mLPFeederId & "," & mOwnershipId & "," & mIsHavaei & "," & mIsActive & ",'" & mBazdidMasterIDs & "','" & mBasketDetailIDs & "','" & mPrs & "','" & mCheckLists & "'," & IIf(mIsLightReport, 1, 0) & ",'" & mAddress & "','" & mSubCheckLists & "','" & mBazdidSpeciality & "','" & cmbBazdidType.SelectedIndex & "'"
        Dim lDS As New DataSet
        lSQL = " SELECT  Tbl_LPFeeder.AreaId, " &
                    "	Tbl_Area.Area,  " &
                    "	Tbl_LPPost.LPPostName,		" &
                    "	Tbl_LPFeeder.LPFeederName, " &
                    "   Tbl_LPFeeder.Address  as  FeederAddrss  	," &
                    "	Tbl_LPFeeder.LPFeederId, " &
                    "	BTblBazdidResult.FromToLengthHavayi, " &
                    "	ISNULL(Tbl_LPFeeder.HavaeiLength,0) AS LPFeederHavayiLen, " &
                    "	BTblBazdidResult.FromToLengthZamini, " &
                    "	ISNULL(Tbl_LPFeeder.ZeminiLength,0) AS LPFeederZaminiLen, " &
                    "	ISNULL(BTblBazdidResult.FromToLengthHavayi,Tbl_LPFeeder.HavaeiLength) AS HavaeiLength, " &
                    "	ISNULL(BTblBazdidResult.FromToLengthZamini,Tbl_LPFeeder.ZeminiLength) AS ZeminiLength, " &
                    "	ISNULL(BTbl_SubCheckList_New.SubCheckListCode,BTbl_SubCheckList_Old.SubCheckListCode) AS SubCheckListCode, " &
                    "	ISNULL(BTbl_SubCheckList_New.SubCheckListName,BTbl_SubCheckList_Old.SubCheckListName) AS SubCheckListName, " &
                    "	BTblBazdidResult.FromPathTypeId, " &
                    "	Tbl_PathType_From.PathType AS FromPathType, " &
                    "	BTblBazdidResult.FromPathTYpeValue, " &
                    "	BTblBazdidResult.ToPathTypeId, " &
                    "	Tbl_PathType_To.PathType AS ToPathType, " &
                    "	BTblBazdidResult.ToPathTypeValue, " &
                    "	BTblBazdidResultAddress.Address, " &
                    "	BTblBazdidResultAddress.GPSx, " &
                    "	BTblBazdidResultAddress.GPSy, " &
                    "	BTbl_BazdidCheckList.CheckListCode, " &
                    "	BTbl_BazdidCheckList.CheckListName, " &
                    "	BTblBazdidResultCheckList.Priority, " &
                    "	Tbl_FeederPart.FeederPart, " &
                    "	BTbl_BazdidCheckListGroup.IsHavayi, " &
                    "	BTblBazdidTiming.BazdidName, " &
                    "	CASE " &
                    "	 WHEN BTblBazdidTiming.BazdidAutoTimingId IS NULL THEN " &
                    "		'موردي' " &
                    "	 WHEN BTblBazdidTiming.BazdidAutoTimingId IS NOT NULL THEN " &
                    "		'دوره اي' " &
                    "	END AS BazdidAutoTiming   " &
        " FROM  Tbl_LPFeeder " &
                    "	INNER JOIN Tbl_Area ON Tbl_LPFeeder.AreaId = Tbl_Area.AreaId " &
                    "	INNER JOIN BTblBazdidResult ON Tbl_LPFeeder.LPFeederId = BTblBazdidResult.LPFeederId " &
                    "	INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
                    "	INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
                    "	LEFT OUTER JOIN BTblBazdidResultSubCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblBazdidResultSubCheckList.BazdidResultCheckListId " &
                    "	LEFT OUTER JOIN Tbl_PathType Tbl_PathType_From ON BTblBazdidResult.FromPathTypeId = Tbl_PathType_From.PathTypeId " &
                    "	LEFT OUTER JOIN Tbl_PathType Tbl_PathType_To ON BTblBazdidResult.ToPathTypeId = Tbl_PathType_To.PathTypeId " &
                    "	LEFT OUTER JOIN BTbl_BazdidCheckList ON BTblBazdidResultCheckList.BazdidCheckListId = BTbl_BazdidCheckList.BazdidCheckListId " &
                    "	LEFT OUTER JOIN BTbl_SubCheckList AS BTbl_SubCheckList_Old ON BTblBazdidResultCheckList.SubCheckListId = BTbl_SubCheckList_Old.SubCheckListId " &
                    "	LEFT OUTER JOIN BTbl_SubCheckList AS BTbl_SubCheckList_New ON BTblBazdidResultSubCheckList.SubCheckListId = BTbl_SubCheckList_New.SubCheckListId " &
                    "	LEFT OUTER JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                    "	INNER JOIN Tbl_LPPost ON Tbl_LPFeeder.LPPostId = Tbl_LPPost.LPPostId " &
                    "	INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
                    "	INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
                    "	LEFT JOIN Tbl_FeederPart On BTblBazdidResult.FeederPartId = Tbl_FeederPart.FeederPartId " &
                    "	LEFT JOIN BTbl_BazdidCheckListGroup ON BTbl_BazdidCheckList.BazdidCheckListGroupId = BTbl_BazdidCheckListGroup.BazdidCheckListGroupId " &
                    "	" & mJoinSpecialitySql & " " &
        " WHERE  " &
                    "	BTblBazdidResult.BazdidStateId IN (2,3) " &
                    "	AND BTblBazdidResult.BazdidTypeId = 3 " &
                    "	AND BTblBazdidResultCheckList.Priority > 0 " &
                    "	AND Tbl_LPFeeder.IsLightFeeder =  " & IIf(mIsLightReport, 1, 0) & " " & mAddress & " " & mWhere.Replace("BTbl_SubCheckList.SubCheckListId", "ISNULL(BTbl_SubCheckList_New.SubCheckListId,BTbl_SubCheckList_Old.SubCheckListId)")

        '--"	AND  " & _
        '           "	( " & _
        '           "		NOT ISNULL(BTbl_SubCheckList_New.SubCheckListCode,BTbl_SubCheckList_Old.SubCheckListCode) IS NULL " & _
        '           "		OR NOT ISNULL(BTbl_SubCheckList_New.SubCheckListName,BTbl_SubCheckList_Old.SubCheckListName) IS NULL " & _
        '           "	) " & _

        BindingTable(lSQL, mCnn, lDS, "Report_1_34_4", , True)


        If lDS.Tables.Contains("Report_1_34_4") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_1_34_4.xml", XmlWriteMode.WriteSchema)
            mFF.AddFormulaFields("FeederType", IIf(mIsLightReport, """ روشنايي معابر """, """ فشار ضعيف """))
            ShowReport("", lFolder & "Report_1_34_4.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_1_34_5()
        MakeQuery()
        Dim lSQL As String = ""
        lSQL = "EXEC spGetReport_1_34_5 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & ",'" & mMPFeederIDs & "'," & mLPPostId & "," & mLPFeederId & "," & mOwnershipId & "," & mIsHavaei & "," & mIsActive & "," & IIf(mIsLightReport, 1, 0) & ",'" & mBazdidMasterIDs & "','" & mBasketDetailIDs & "','" & mParts & "','" & mAddress & "','" & mBazdidSpeciality & "','" & cmbBazdidType.SelectedIndex & "'"
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_1_34_5", , True)
        If lDS.Tables.Contains("Report_1_34_5") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_1_34_5.xml", XmlWriteMode.WriteSchema)
            mFF.AddFormulaFields("FeederType", IIf(mIsLightReport, """ روشنايي معابر """, """ فشار ضعيف """))
            ShowReport("", lFolder & "Report_1_34_5.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_1_34_6()
        MakeQuery()
        Dim lSQL As String = ""
        lSQL = "EXEC spGetReport_1_34_6 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & ",'" & mMPFeederIDs & "'," & mLPPostId & "," & mLPFeederId & "," & mOwnershipId & "," & mIsHavaei & "," & mIsActive & "," & IIf(mIsLightReport, 1, 0) & "," & mIsExistMap & "," & mIsUpdateMap & "," & mIsOKNetDesign & "," & IIf(chkIsNoValue.Checked, 1, 0) & ",'" & mBazdidSpeciality & "'"

        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_1_34_6", , True)
        If lDS.Tables.Contains("Report_1_34_6") Then
            Dim lFolder As String = "Reports\Bazdid\"
            mFF.AddFormulaFields("FeederType", IIf(mIsLightReport, """ روشنايي معابر """, """ فشار ضعيف """))
            lDS.WriteXml(ReportsXMLPath & "Report_1_34_6.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_1_34_6.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_1_3_9()
        MakeQuery()
        Dim lSQL As String = ""
        lSQL = "EXEC spGetReport_1_3_9 " & mFromDate & "," & mToDate & ",'" & mAreaId & "','" & mMPFeederIDs & "'," & mMPPostId & "," & mLPPostId & "," & mLPFeederId & "," & mOwnershipId & "," & mIsHavaei & "," & mIsActive & ",'" & mBazdidMasterIDs & "','" & mBasketDetailIDs & "','" & mBazdidSpeciality & "'"
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_1_3_9", , True)
        If lDS.Tables.Contains("Report_1_3_9") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_1_3_9.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_1_3_9.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub

    Private Sub MakeReport_2_1_1()
        MakeQuery()
        Dim lSQL As String = ""
        lSQL = "EXEC spGetReport_2_1_1 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & ",'" &
            mMPFeederIDs & "'," & mOwnershipId & "," & mIsActive & ",'" & mBazdidMasterIDs & "','" &
            mBasketDetailIDs & "','" & mBazdidSpeciality & "','" & mIsHavaei & "'," & mFromDateBazdid & "," &
            mToDateBazdid & "," & mIsWarmLineSP & ""
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_2_1_1", , True)
        If lDS.Tables.Contains("Report_2_1_1") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_2_1_1.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_2_1_1.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_2_1_2()
        MakeQuery()
        Dim lSQL As String = ""
        lSQL = "EXEC spGetReport_2_1_2 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & ",'" &
            mMPFeederIDs & "'," & mOwnershipId & "," & mIsActive & ",'" & mBazdidMasterIDs & "','" &
            mBasketDetailIDs & "','" & mBazdidSpeciality & "'," & mFromDateBazdid & "," &
            mToDateBazdid & "," & mIsWarmLineSP & ""
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_2_1_2", , True)
        If lDS.Tables.Contains("Report_2_1_2") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_2_1_2.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_2_1_2.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_2_1_3()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_2_1_3 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mOwnershipId & "," & mIsActive & ",'" & mBazdidMasterIDs & "','" & mBasketDetailIDs & "','" & mPrs & "','" & mCheckLists & "','" & mAddress & "'," & mMinCheckListCount & ",'" & mBazdidSpeciality & "','" & mIsWarmLine & "'"
        Dim lDS As New DataSet

        lSQL = "CREATE TABLE #TmpTbl_MPFeedersId " &
                " ( MPFeederId int ) " &
    "	INSERT #TmpTbl_MPFeedersId EXEC spGetMPFeedersId  '" & mAreaId & "' ,'" & mMPPostId & "' " &
    "	SELECT DISTINCT " &
                "	Tbl_Area.AreaId, " &
                "	Tbl_Area.Area, " &
                "	Tbl_MPPost.MPPostName, " &
                "	Tbl_MPFeeder.MPFeederId, " &
                "	Tbl_MPFeeder.MPFeederName, " &
                "	ISNULL(Tbl_MPFeeder.HavaeiLength,0) AS HavayiLen, " &
                "	ISNULL(Tbl_MPFeeder.ZeminiLength,0) AS ZaminiLen, " &
                "	BTblBazdidResult.BazdidResultId, " &
                "	ISNULL(BTblBazdidResult.FromToLengthHavayi,Tbl_MPFeeder.HavaeiLength) AS BazdidHavayi, " &
                "	ISNULL(BTblBazdidResult.FromToLengthZamini,Tbl_MPFeeder.ZeminiLength) AS BazdidZamini, " &
                "	BTblBazdidResult.FromPathTypeId, " &
                "	Tbl_PathType_From.PathType AS FromPathType, " &
                "	BTblBazdidResult.FromPathTYpeValue, " &
                "	BTblBazdidResult.ToPathTypeId, " &
                "	Tbl_PathType_To.PathType AS ToPathType, " &
                "	BTblBazdidResult.ToPathTypeValue, " &
                "	BTblBazdidResultAddress.Address, " &
                "	BTblBazdidResultAddress.GPSx, " &
                "	BTblBazdidResultAddress.GPSy, " &
                "	Tbl_FeederPart.FeederPart, " &
                "	BTblBazdidResultCheckList.BazdidResultCheckListId, " &
                "	CASE WHEN BTblBazdidResultCheckList.Priority = 4 THEN  " &
                "	CAST('عدم وجود تجهيز' as nvarchar)  " &
                "	ELSE CAST(BTblBazdidResultCheckList.Priority as nvarchar) END AS Priority, " &
                "	BTbl_BazdidCheckList.CheckListCode, " &
                "	BTbl_BazdidCheckList.CheckListName, " &
                "	GetDoneDatePersian.DoneDatePersian, " &
                "	BTbl_BazdidCheckListGroup.IsHavayi, " &
                "	SUM(CASE WHEN BTblServiceCheckList.ServiceCount Is Null OR BTblServiceCheckList.ServiceCount = 0 THEN 1 ELSE BTblServiceCheckList.ServiceCount END) AS ServiceCount " &
                " FROM	BTblBazdidResultAddress " &
                "	INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
                "	INNER JOIN BTblBazdidResult ON BTblBazdidResultAddress.BazdidResultId = BTblBazdidResult.BazdidResultId " &
                "	INNER JOIN Tbl_MPFeeder ON BTblBazdidResult.MPFeederId = Tbl_MPFeeder.MPFeederId " &
                "	INNER JOIN BTbl_BazdidCheckList ON BTblBazdidResultCheckList.BazdidCheckListId = BTbl_BazdidCheckList.BazdidCheckListId " &
                "	INNER JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId " &
                "	LEFT OUTER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId " &
                "	LEFT OUTER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
                "	LEFT OUTER JOIN Tbl_PathType Tbl_PathType_From ON BTblBazdidResult.FromPathTypeId = Tbl_PathType_From.PathTypeId " &
                "	LEFT OUTER JOIN Tbl_PathType Tbl_PathType_To ON BTblBazdidResult.ToPathTypeId = Tbl_PathType_To.PathTypeId " &
                "	LEFT JOIN Tbl_FeederPart On BTblBazdidResult.FeederPartId = Tbl_FeederPart.FeederPartId " &
                "	LEFT JOIN BTbl_BazdidCheckListGroup ON BTbl_BazdidCheckList.BazdidCheckListGroupId = BTbl_BazdidCheckListGroup.BazdidCheckListGroupId " &
                "   LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                "	" & mJoinSpecialitySql & " " &
                "	INNER JOIN  ( " &
                "   	SELECT  " &
                "      	BTblServiceCheckList.BazdidResultCheckListId, " &
                "		MAX(DoneDatePersian) AS DoneDatePersian " &
                "		FROM  " &
                "		    BTblServiceCheckList " &
                "       	GROUP BY    BTblServiceCheckList.BazdidResultCheckListId " &
                "	) AS GetDoneDatePersian ON BTblBazdidResultCheckList.BazdidResultCheckListId = GetDoneDatePersian.BazdidResultCheckListId " &
                "   WHERE  " &
                "	(BTblBazdidResult.BazdidTypeId = 1) " &
                "	AND BTblBazdidResultCheckList.Priority > 0 " &
                "	AND (BTblServiceCheckList.ServiceStateId = 3 OR (BTblServiceCheckList.ServiceStateId = 2 AND BTblServiceCheckList.ServiceCount > 0)) " &
                "	" & mAddress & " " & mWhere &
                "   GROUP BY " &
                "	Tbl_Area.AreaId, " &
                "	Tbl_Area.Area, " &
                "	Tbl_MPPost.MPPostName, " &
                "	Tbl_MPFeeder.MPFeederId, " &
                "	Tbl_MPFeeder.MPFeederName, " &
                "	ISNULL(Tbl_MPFeeder.HavaeiLength,0), " &
                "	ISNULL(Tbl_MPFeeder.ZeminiLength,0), " &
                "	BTblBazdidResult.BazdidResultId, " &
                "	ISNULL(BTblBazdidResult.FromToLengthHavayi,Tbl_MPFeeder.HavaeiLength), " &
                "	ISNULL(BTblBazdidResult.FromToLengthZamini,Tbl_MPFeeder.ZeminiLength), " &
                "	BTblBazdidResult.FromPathTypeId, " &
                "	Tbl_PathType_From.PathType, " &
                "	BTblBazdidResult.FromPathTYpeValue, " &
                "	BTblBazdidResult.ToPathTypeId, " &
                "	Tbl_PathType_To.PathType, " &
                "	BTblBazdidResult.ToPathTypeValue, " &
                "	BTblBazdidResultAddress.Address, " &
                "	BTblBazdidResultAddress.GPSx, " &
                "	BTblBazdidResultAddress.GPSy, " &
                "	Tbl_FeederPart.FeederPart, " &
                "	BTblBazdidResultCheckList.BazdidResultCheckListId, " &
                "	CASE WHEN BTblBazdidResultCheckList.Priority = 4 THEN  " &
                "	CAST('عدم وجود تجهيز' as nvarchar)  " &
                "	ELSE CAST(BTblBazdidResultCheckList.Priority as nvarchar) END , " &
                "	BTbl_BazdidCheckList.CheckListCode, " &
                "	BTbl_BazdidCheckList.CheckListName, " &
                "	GetDoneDatePersian.DoneDatePersian, " &
                "	BTbl_BazdidCheckListGroup.IsHavayi  " &
                "  " & mHaving & "  ORDER BY   GetDoneDatePersian.donedatepersian  DESC    DROP TABLE #TmpTbl_MPFeedersId "

        BindingTable(lSQL, mCnn, lDS, "Report_2_1_3", , True)
        If lDS.Tables.Contains("Report_2_1_3") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_2_1_3.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_2_1_3.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_2_1_4()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_2_1_4 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mOwnershipId & "," & mIsActive & ",'" & mBazdidMasterIDs & "','" & mBasketDetailIDs & "','" & mPrs & "','" & mCheckLists & "','" & mAddress & "','" & mSubCheckLists & "','" & mBazdidSpeciality & "','" & mIsWarmLine & "'"
        Dim lDS As New DataSet

        lSQL = "    CREATE TABLE #TmpTbl_MPFeedersId" &
                    " (   MPFeederId Int) " &
                    " INSERT #TmpTbl_MPFeedersId EXEC spGetMPFeedersId  '" & mAreaId & "' ,'" & mMPPostId & "' " &
            " Select DISTINCT	Tbl_Area.AreaId, " &
                "	Tbl_Area.Area, " &
                "	Tbl_MPPost.MPPostName,Tbl_MPFeeder.MPFeederId, Tbl_MPFeeder.MPFeederName,ISNULL(Tbl_MPFeeder.HavaeiLength,0) AS HavayiLen, " &
                "	ISNULL(Tbl_MPFeeder.ZeminiLength,0) AS ZaminiLen,BTblBazdidResult.BazdidResultId,ISNULL(BTblBazdidResult.FromToLengthHavayi,Tbl_MPFeeder.HavaeiLength) AS BazdidHavayi, " &
                "	ISNULL(BTblBazdidResult.FromToLengthZamini,Tbl_MPFeeder.ZeminiLength) AS BazdidZamini,BTblBazdidResult.FromPathTypeId, " &
                "	Tbl_PathType_From.PathType AS FromPathType, BTblBazdidResult.FromPathTYpeValue,BTblBazdidResult.ToPathTypeId, " &
                "	Tbl_PathType_To.PathType AS ToPathType,BTblBazdidResult.ToPathTypeValue,BTblBazdidResultAddress.Address, " &
                "	BTblBazdidResultAddress.GPSx,BTblBazdidResultAddress.GPSy,BTblBazdidResultCheckList.BazdidResultCheckListId, " &
                "	BTblBazdidResultCheckList.Priority,BTbl_BazdidCheckList.CheckListCode,BTbl_BazdidCheckList.CheckListName, " &
                "	ISNULL(BTbl_SubCheckList_New.SubCheckListCode,BTbl_SubCheckList_Old.SubCheckListCode) AS SubCheckListCode, " &
                "	ISNULL(BTbl_SubCheckList_New.SubCheckListName,BTbl_SubCheckList_Old.SubCheckListName) AS SubCheckListName, " &
                "	Tbl_FeederPart.FeederPart,BTbl_BazdidCheckListGroup.IsHavayi " &
                "	FROM  	BTblBazdidResultAddress " &
                "	INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
                "	INNER JOIN BTblBazdidResult ON BTblBazdidResultAddress.BazdidResultId = BTblBazdidResult.BazdidResultId " &
                "	INNER JOIN Tbl_MPFeeder ON BTblBazdidResult.MPFeederId = Tbl_MPFeeder.MPFeederId " &
                "	INNER JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId " &
                "	INNER JOIN BTbl_BazdidCheckList ON BTblBazdidResultCheckList.BazdidCheckListId = BTbl_BazdidCheckList.BazdidCheckListId " &
                "	LEFT OUTER JOIN BTbl_BazdidMaster ON BTblService.BazdidMasterId = BTbl_BazdidMaster.BazdidMasterId " &
                "	LEFT OUTER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId " &
                "	LEFT OUTER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
                "	LEFT OUTER JOIN Tbl_PathType Tbl_PathType_From ON BTblBazdidResult.FromPathTypeId = Tbl_PathType_From.PathTypeId " &
                "	LEFT OUTER JOIN Tbl_PathType Tbl_PathType_To ON BTblBazdidResult.ToPathTypeId = Tbl_PathType_To.PathTypeId " &
                "	LEFT OUTER JOIN BTblBazdidResultSubCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblBazdidResultSubCheckList.BazdidResultCheckListId " &
                "   INNER JOIN		BTblServiceSubCheckList  on BTblBazdidResultSubCheckList.BazdidResultSubCheckListId = BTblServiceSubCheckList.BazdidResultSubCheckListId " &
                "	LEFT OUTER JOIN BTbl_SubCheckList AS BTbl_SubCheckList_Old ON BTblBazdidResultCheckList.SubCheckListId = BTbl_SubCheckList_Old.SubCheckListId " &
                "	LEFT OUTER JOIN BTbl_SubCheckList AS BTbl_SubCheckList_New ON BTblBazdidResultSubCheckList.SubCheckListId = BTbl_SubCheckList_New.SubCheckListId " &
                "	LEFT JOIN Tbl_FeederPart On BTblBazdidResult.FeederPartId = Tbl_FeederPart.FeederPartId " &
                "	LEFT JOIN BTbl_BazdidCheckListGroup ON BTbl_BazdidCheckList.BazdidCheckListGroupId = BTbl_BazdidCheckListGroup.BazdidCheckListGroupId " &
                "   LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                "	" & mJoinSpecialitySql &
                    "	WHERE  " &
                    "	(BTblBazdidResult.BazdidTypeId = 1)  " &
                    "	AND (BTblServiceCheckList.ServiceStateId = 3) " &
                    "	AND ( " &
                    "		 	NOT ISNULL(BTbl_SubCheckList_New.SubCheckListCode,BTbl_SubCheckList_Old.SubCheckListCode) IS NULL " &
                    "		 	OR NOT ISNULL(BTbl_SubCheckList_New.SubCheckListName,BTbl_SubCheckList_Old.SubCheckListName) IS NULL " &
                    "		) " &
                    "	" & mAddress & mWhere.Replace("BTbl_SubCheckList.SubCheckListId", "ISNULL(BTbl_SubCheckList_New.SubCheckListId,BTbl_SubCheckList_Old.SubCheckListId)") &
                "  UNION ALL " &
             " Select DISTINCT	Tbl_Area.AreaId, " &
                "	Tbl_Area.Area, " &
                "	Tbl_MPPost.MPPostName,Tbl_MPFeeder.MPFeederId, Tbl_MPFeeder.MPFeederName,ISNULL(Tbl_MPFeeder.HavaeiLength,0) AS HavayiLen, " &
                "	ISNULL(Tbl_MPFeeder.ZeminiLength,0) AS ZaminiLen,BTblBazdidResult.BazdidResultId,ISNULL(BTblBazdidResult.FromToLengthHavayi,Tbl_MPFeeder.HavaeiLength) AS BazdidHavayi, " &
                "	ISNULL(BTblBazdidResult.FromToLengthZamini,Tbl_MPFeeder.ZeminiLength) AS BazdidZamini,BTblBazdidResult.FromPathTypeId, " &
                "	Tbl_PathType_From.PathType AS FromPathType, BTblBazdidResult.FromPathTYpeValue,BTblBazdidResult.ToPathTypeId, " &
                "	Tbl_PathType_To.PathType AS ToPathType,BTblBazdidResult.ToPathTypeValue,BTblBazdidResultAddress.Address, " &
                "	BTblBazdidResultAddress.GPSx,BTblBazdidResultAddress.GPSy,BTblBazdidResultCheckList.BazdidResultCheckListId, " &
                "	BTblBazdidResultCheckList.Priority,BTbl_BazdidCheckList.CheckListCode,BTbl_BazdidCheckList.CheckListName, " &
                "	ISNULL(BTbl_SubCheckList_New.SubCheckListCode,BTbl_SubCheckList_Old.SubCheckListCode) AS SubCheckListCode, " &
                "	ISNULL(BTbl_SubCheckList_New.SubCheckListName,BTbl_SubCheckList_Old.SubCheckListName) AS SubCheckListName, " &
                "	Tbl_FeederPart.FeederPart,BTbl_BazdidCheckListGroup.IsHavayi " &
                "	FROM  	BTblBazdidResultAddress " &
                "	INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
                "	INNER JOIN BTblBazdidResult ON BTblBazdidResultAddress.BazdidResultId = BTblBazdidResult.BazdidResultId " &
                "	INNER JOIN Tbl_MPFeeder ON BTblBazdidResult.MPFeederId = Tbl_MPFeeder.MPFeederId " &
                "	INNER JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId " &
                "	INNER JOIN BTbl_BazdidCheckList ON BTblBazdidResultCheckList.BazdidCheckListId = BTbl_BazdidCheckList.BazdidCheckListId " &
                "	LEFT OUTER JOIN BTbl_BazdidMaster ON BTblService.BazdidMasterId = BTbl_BazdidMaster.BazdidMasterId " &
                "	LEFT OUTER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId " &
                "	LEFT OUTER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
                "	LEFT OUTER JOIN Tbl_PathType Tbl_PathType_From ON BTblBazdidResult.FromPathTypeId = Tbl_PathType_From.PathTypeId " &
                "	LEFT OUTER JOIN Tbl_PathType Tbl_PathType_To ON BTblBazdidResult.ToPathTypeId = Tbl_PathType_To.PathTypeId " &
                "	LEFT OUTER JOIN BTblBazdidResultSubCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblBazdidResultSubCheckList.BazdidResultCheckListId " &
                "	LEFT OUTER JOIN BTbl_SubCheckList AS BTbl_SubCheckList_Old ON BTblBazdidResultCheckList.SubCheckListId = BTbl_SubCheckList_Old.SubCheckListId " &
                "	LEFT OUTER JOIN BTbl_SubCheckList AS BTbl_SubCheckList_New ON BTblBazdidResultSubCheckList.SubCheckListId = BTbl_SubCheckList_New.SubCheckListId " &
                "	LEFT JOIN Tbl_FeederPart On BTblBazdidResult.FeederPartId = Tbl_FeederPart.FeederPartId " &
                "	LEFT JOIN BTbl_BazdidCheckListGroup ON BTbl_BazdidCheckList.BazdidCheckListGroupId = BTbl_BazdidCheckListGroup.BazdidCheckListGroupId " &
                "   LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                "	" & mJoinSpecialitySql &
                    "	WHERE  " &
                    "	(BTblBazdidResult.BazdidTypeId = 1)  " &
                    "	AND (BTblServiceCheckList.ServiceStateId = 3) " &
                    "   AND NOT BTblServiceCheckList.ServiceCheckListId IN (select ServiceCheckListId FROM BTblServiceSubCheckList   ) " &
                    "	AND ( " &
                    "		 	NOT ISNULL(BTbl_SubCheckList_New.SubCheckListCode,BTbl_SubCheckList_Old.SubCheckListCode) IS NULL " &
                    "		 	OR NOT ISNULL(BTbl_SubCheckList_New.SubCheckListName,BTbl_SubCheckList_Old.SubCheckListName) IS NULL " &
                    "		) " &
                    "	" & mAddress & mWhere.Replace("BTbl_SubCheckList.SubCheckListId", "ISNULL(BTbl_SubCheckList_New.SubCheckListId,BTbl_SubCheckList_Old.SubCheckListId)") &
                " DROP TABLE #TmpTbl_MPFeedersId "

        BindingTable(lSQL, mCnn, lDS, "Report_2_1_4", , True)
        If lDS.Tables.Contains("Report_2_1_4") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_2_1_4.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_2_1_4.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_2_1_5()
        MakeQuery()
        Dim lSQL As String = ""
        lSQL = "EXEC spGetReport_2_1_5 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & ",'" &
            mMPFeederIDs & "'," & mOwnershipId & "," & mIsActive & ",'" & mBazdidMasterIDs & "','" &
            mBasketDetailIDs & "','" & mParts & "','" & mAddress & "','" & mBazdidSpeciality & "','" &
            txtServiceNumber.Text & "'," & mFromDateBazdid & "," & mToDateBazdid & ",'" &
            mWorkCommand & "," & mIsWarmLineSP & "'"
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_2_1_5", , True)
        If lDS.Tables.Contains("Report_2_1_5") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_2_1_5.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_2_1_5.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_2_1_6()
        MakeQuery()
        Dim lSQL As String = ""
        Dim lServiceCheckList As String
        'Dim lSQL2 As String = "EXEC spGetReport_2_1_6 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mOwnershipId & "," & mIsActive & ",'" & mBazdidMasterIDs & "','" & mBasketDetailIDs & "','" & mPrs & "','" & mCheckLists & "'," & mNotService & ",'" & mBazdidSpeciality & "'"
        lServiceCheckList = " OR BTblServiceCheckList.ServiceStateId <> 3 "
        If mNotService = 1 Then
            lServiceCheckList = ""
        End If
        lSQL = "  CREATE TABLE #TmpTbl_MPFeedersId" &
                  " (	MPFeederId int )" &
                  "     INSERT #TmpTbl_MPFeedersId EXEC spGetMPFeedersId '" & mAreaId & "','" & mMPPostId & "' " &
                  "  SELECT DISTINCT" &
                  "	    Tbl_Area.AreaId," &
                  " 	Tbl_Area.Area," &
                  " 	Tbl_MPPost.MPPostName," &
                  " 	Tbl_MPFeeder.MPFeederId," &
                  "	    Tbl_MPFeeder.MPFeederName," &
                  " 	ISNULL(Tbl_MPFeeder.HavaeiLength,0) AS HavayiLen," &
                  " 	ISNULL(Tbl_MPFeeder.ZeminiLength,0) AS ZaminiLen," &
                  " 	BTblBazdidResult.BazdidResultId," &
                  " 	ISNULL(BTblBazdidResult.FromToLengthHavayi,Tbl_MPFeeder.HavaeiLength) AS BazdidHavayi," &
                  " 	ISNULL(BTblBazdidResult.FromToLengthZamini,Tbl_MPFeeder.ZeminiLength) AS BazdidZamini," &
                  " 	BTblBazdidResult.FromPathTypeId," &
                  " 	Tbl_PathType_From.PathType AS FromPathType," &
                  "	    BTblBazdidResult.FromPathTYpeValue," &
                  "	    BTblBazdidResult.ToPathTypeId," &
                  " 	Tbl_PathType_To.PathType AS ToPathType," &
                  " 	BTblBazdidResult.ToPathTypeValue," &
                  "	    BTblBazdidResultAddress.StartDatePersian," &
                  " 	BTblBazdidResultAddress.Address," &
                  " 	BTblBazdidResultAddress.GPSx," &
                  " 	BTblBazdidResultAddress.GPSy," &
                  " 	Tbl_FeederPart.FeederPart," &
                  " 	BTblBazdidResultCheckList.BazdidResultCheckListId," &
                  " 	CASE WHEN BTblBazdidResultCheckList.Priority = 4 THEN" &
                  "	    CAST( 'عدم وجود تجهيز' as nvarchar)" &
                  "	    ELSE CAST(BTblBazdidResultCheckList.Priority as nvarchar) END AS Priority," &
                  " 	BTbl_BazdidCheckList.CheckListCode," &
                  "	    BTbl_BazdidCheckList.CheckListName," &
                  "     BTblBazdidResultSubCheckList.BazdidResultSubCheckListId , " &
                  "     BTbl_SubCheckList.SubCheckListName," &
                  "	    BTbl_BazdidCheckListGroup.IsHavayi," &
                  " 	BTblBazdidTiming.BazdidName," &
                  " 	SUM(BTblBazdidResultCheckList.DefectionCount) AS DefectionCount," &
                  "	    SUM(BTblBazdidResultCheckList.DefectionCount - IsNull(BTblServiceCheckList.ServiceCount,0)) AS CheckListCount " &
                  " FROM	BTblBazdidResultAddress " &
                  " 	INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId" &
                  " 	INNER JOIN BTblBazdidResult ON BTblBazdidResultAddress.BazdidResultId = BTblBazdidResult.BazdidResultId" &
                  " 	INNER JOIN Tbl_MPFeeder ON BTblBazdidResult.MPFeederId = Tbl_MPFeeder.MPFeederId" &
                  " 	INNER JOIN BTbl_BazdidCheckList ON BTblBazdidResultCheckList.BazdidCheckListId = BTbl_BazdidCheckList.BazdidCheckListId" &
                  "     LEFT JOIN BTblBazdidResultSubCheckList ON BTblBazdidResultChecklist.BazdidResultCheckListId = BTblBazdidResultSubCheckList.BazdidResultCheckListId " &
                  "     LEFT JOIN BTbl_SubCheckList ON BTblBazdidResultSubCheckList.SubCheckListId = BTbl_SubCheckList.SubCheckListId" &
                  "     LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                  " 	LEFT JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId" &
                  " 	LEFT OUTER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId" &
                  " 	LEFT OUTER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId" &
                  "	    LEFT OUTER JOIN Tbl_PathType Tbl_PathType_From ON BTblBazdidResult.FromPathTypeId = Tbl_PathType_From.PathTypeId" &
                  "	    LEFT OUTER JOIN Tbl_PathType Tbl_PathType_To ON BTblBazdidResult.ToPathTypeId = Tbl_PathType_To.PathTypeId" &
                  " 	LEFT JOIN Tbl_FeederPart On BTblBazdidResult.FeederPartId = Tbl_FeederPart.FeederPartId" &
                  "	    LEFT JOIN BTbl_BazdidCheckListGroup ON BTbl_BazdidCheckList.BazdidCheckListGroupId = BTbl_BazdidCheckListGroup.BazdidCheckListGroupId" &
                  "	  " & mJoinSpecialitySql & "  " &
                       "   WHERE " &
                    "	(BTblBazdidResult.BazdidTypeId = 1)" &
                    "	AND BTblBazdidResultCheckList.Priority > 0" &
                    "	AND" &
                    "	( BTblServiceCheckList.ServiceCheckListId IS NULL " &
                    "	" & lServiceCheckList & ")" &
                    "	" & mWhere &
                    "	 Group by 	 Tbl_Area.AreaId," &
                    "	Tbl_Area.Area," &
                    "	Tbl_MPPost.MPPostName," &
                    "	Tbl_MPFeeder.MPFeederId," &
                    "   Tbl_MPFeeder.MPFeederName," &
                    "   Tbl_MPFeeder.HavaeiLength  ," &
                    "	Tbl_MPFeeder.ZeminiLength   ," &
                    "	BTblBazdidResult.BazdidResultId," &
                    "	BTblBazdidResult.FromToLengthHavayi   ," &
                    "	BTblBazdidResult.FromToLengthZamini   ," &
                    "	BTblBazdidResult.FromPathTypeId," &
                    "	Tbl_PathType_From.PathType ," &
                    "	BTblBazdidResult.FromPathTYpeValue," &
                    "	BTblBazdidResult.ToPathTypeId," &
                    "	Tbl_PathType_To.PathType ," &
                    "	BTblBazdidResult.ToPathTypeValue," &
                    "	BTblBazdidResultAddress.StartDatePersian," &
                    "	BTblBazdidResultAddress.Address," &
                    "	BTblBazdidResultAddress.GPSx," &
                    "	BTblBazdidResultAddress.GPSy," &
                    "	Tbl_FeederPart.FeederPart," &
                    "	BTblBazdidResultCheckList.BazdidResultCheckListId," &
                    "	BTblBazdidResultCheckList.Priority  ," &
                    "	BTbl_BazdidCheckList.CheckListCode," &
                    "	BTbl_BazdidCheckList.CheckListName," &
                    "   BTblBazdidResultSubCheckList.BazdidResultSubCheckListId , " &
                    "   BTbl_SubCheckList.SubCheckListName, " &
                    "	BTbl_BazdidCheckListGroup.IsHavayi," &
                    "	BTblBazdidTiming.BazdidName" &
                    "   DROP TABLE #TmpTbl_MPFeedersId "
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_2_1_6", , True)
        If lDS.Tables.Contains("Report_2_1_6") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_2_1_6.xml", XmlWriteMode.WriteSchema)
            If chkNotService.Checked And chkBazdidName.Checked Then
                ShowReport("", lFolder & "Report_2_1_6_ReportNumber.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
            Else
                ShowReport("", lFolder & "Report_2_1_6.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
            End If
        End If
    End Sub
    Private Sub MakeReport_2_1_8()
        MakeQuery()
        Dim lSQL As String = ""
        Dim lDS As New DataSet
        Dim lPath As String = "Reports\Bazdid\Excel\"
        Dim lFileName As String = "spGetExcel_2_1_8.xlsx"
        Dim lSheet As String = "خطوط فشار متوسط"
        Dim lTitle As String = "شرکت توزيع نيروي برق - " & CConfig.ReadConfig("ToziName")
        Dim lDateRows(), lRow As DataRow

        lSQL = "EXEC spGetReport_2_1_8 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & ",'" &
            mMPFeederIDs & "'," & mOwnershipId & "," & mIsActive & ",'" & mBazdidMasterIDs & "','" &
            mBasketDetailIDs & "','" & mPrs & "','" & mCheckLists & "','" & mAddress & "'," & mMinCheckListCount & ",'" &
            mBazdidSpeciality & "'," & mFromDateBazdid & "," & mToDateBazdid & "," & mIsWarmLineSP & ""
        BindingTable(lSQL, mCnn, lDS, "spGetReport_2_1_8", , True)
        lDateRows = lDS.Tables("spGetReport_2_1_8").Select()

        If lDS.Tables.Contains("spGetReport_2_1_8") Then
            If mExcel Is Nothing Then
                mExcel = New CExcelMng
            Else
                mExcel.CloseWorkBook()
            End If
            mExcel.OpenWorkBook(lPath & lFileName)
            Dim i As Integer = 1
            mExcel.WriteCell(lSheet, "A1", mExcel.ReadCell(lSheet, "A1") + " " + lTitle, False, CExcelManager.TextHAlign.Center.Center, CExcelManager.TextVAlign.Center)
            For Each lRow In lDateRows
                mExcel.WriteCell(lSheet, "A" & i + 2, i)
                mExcel.WriteCell(lSheet, "B" & i + 2, lRow.Item("MPFeederName"))
                mExcel.WriteCell(lSheet, "C" & i + 2, lRow.Item("FeederPart"))
                mExcel.WriteCell(lSheet, "D" & i + 2, lRow.Item("HavayiLen") + lRow.Item("ZaminiLen"))
                mExcel.WriteCell(lSheet, "E" & i + 2, lRow.Item("Address"))
                mExcel.WriteCell(lSheet, "F" & i + 2, lRow.Item("GPSx"))
                mExcel.WriteCell(lSheet, "G" & i + 2, lRow.Item("Gpsy"))
                mExcel.WriteCell(lSheet, "H" & i + 2, lRow.Item("StartDatePersian"))
                mExcel.WriteCell(lSheet, "I" & i + 2, lRow.Item("CheckListCode"))
                mExcel.WriteCell(lSheet, "J" & i + 2, lRow.Item("Priority"))
                mExcel.WriteCell(lSheet, "K" & i + 2, lRow.Item("CheckListName"))
                mExcel.WriteCell(lSheet, "L" & i + 2, lRow.Item("CreateDatePersian"))
                mExcel.WriteCell(lSheet, "M" & i + 2, lRow.Item("DoneDatePersian"))
                mExcel.WriteCell(lSheet, "N" & i + 2, lRow.Item("ServiceNotDoneReason"))
                i = i + 1
            Next
            mExcel.ShowExcel_2007Up()
        End If
    End Sub
    Private Sub MakeReport_2_1_9()
        MakeQuery()
        Dim lSQL As String = ""
        Dim lDS As New DataSet
        Dim lServiceCheckList As String

        lServiceCheckList = " OR NOT BTblBazdidResultCheckList.BazdidResultCheckListId IN " &
         " (SELECT DISTINCT BazdidResultCheckListId FROM BTblServiceCheckList  WHERE BTblServiceCheckList.ServiceStateId <> 3	)"
        If mNotService = 1 Then
            lServiceCheckList = ""
        End If



        lSQL = "  CREATE TABLE #TmpTbl_MPFeedersId" &
                  " (	MPFeederId int )" &
                  "     INSERT #TmpTbl_MPFeedersId EXEC spGetMPFeedersId '" & mAreaId & "','" & mMPPostId & "' " &
                  "  SELECT DISTINCT" &
                  "	    Tbl_Area.AreaId," &
                  " 	Tbl_Area.Area," &
                  " 	Tbl_MPPost.MPPostName," &
                  " 	Tbl_MPFeeder.MPFeederId," &
                  "	    Tbl_MPFeeder.MPFeederName," &
                  "     BTbl_SubCheckList.SubCheckListCode, " &
                  "     BTbl_SubCheckList.SubCheckListName, " &
                  " 	ISNULL(Tbl_MPFeeder.HavaeiLength,0) AS HavayiLen," &
                  " 	ISNULL(Tbl_MPFeeder.ZeminiLength,0) AS ZaminiLen," &
                  " 	BTblBazdidResult.BazdidResultId," &
                  " 	ISNULL(BTblBazdidResult.FromToLengthHavayi,Tbl_MPFeeder.HavaeiLength) AS BazdidHavayi," &
                  " 	ISNULL(BTblBazdidResult.FromToLengthZamini,Tbl_MPFeeder.ZeminiLength) AS BazdidZamini," &
                  " 	BTblBazdidResult.FromPathTypeId," &
                  " 	Tbl_PathType_From.PathType AS FromPathType," &
                  "	    BTblBazdidResult.FromPathTYpeValue," &
                  "	    BTblBazdidResult.ToPathTypeId," &
                  " 	Tbl_PathType_To.PathType AS ToPathType," &
                  " 	BTblBazdidResult.ToPathTypeValue," &
                  "	    BTblBazdidResultAddress.StartDatePersian," &
                  " 	BTblBazdidResultAddress.Address," &
                  " 	BTblBazdidResultAddress.GPSx," &
                  " 	BTblBazdidResultAddress.GPSy," &
                  " 	Tbl_FeederPart.FeederPart," &
                  " 	BTblBazdidResultCheckList.BazdidResultCheckListId," &
                  " 	CASE WHEN BTblBazdidResultCheckList.Priority = 4 THEN" &
                  "	    CAST( 'عدم وجود تجهيز' as nvarchar)" &
                  "	    ELSE CAST(BTblBazdidResultCheckList.Priority as nvarchar) END AS Priority," &
                  " 	BTbl_BazdidCheckList.CheckListCode," &
                  "	    BTbl_BazdidCheckList.CheckListName," &
                  "	    BTbl_BazdidCheckListGroup.IsHavayi," &
                  " 	BTblBazdidTiming.BazdidName," &
                  " 	SUM(BTblBazdidResultCheckList.DefectionCount) AS DefectionCount," &
                  "	    SUM(BTblBazdidResultCheckList.DefectionCount - IsNull(BTblServiceCheckList.ServiceCount,0)) AS CheckListCount " &
                  " FROM	BTblBazdidResultAddress " &
                  " 	INNER JOIN BTblBazdidResultCheckList    ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId" &
                  "     INNER JOIN BTblBazdidResultSubCheckList	ON	BTblBazdidResultCheckList.BazdidResultCheckListId = 	BTblBazdidResultSubCheckList.BazdidResultCheckListId " &
                  " 	INNER JOIN BTblBazdidResult             ON BTblBazdidResultAddress.BazdidResultId = BTblBazdidResult.BazdidResultId" &
                  "     LEFT JOIN BTblBazdidTiming              ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                  " 	INNER JOIN Tbl_MPFeeder                 ON BTblBazdidResult.MPFeederId = Tbl_MPFeeder.MPFeederId" &
                  " 	INNER JOIN BTbl_BazdidCheckList         ON BTblBazdidResultCheckList.BazdidCheckListId = BTbl_BazdidCheckList.BazdidCheckListId" &
                  " 	LEFT JOIN BTblServiceCheckList          ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId" &
                  "     INNER JOIN	BTblServiceSubCheckList	    ON BTblServiceCheckList.ServiceCheckListId = BTblServiceSubCheckList.ServiceCheckListId " &
                  "     INNER JOIN	BTbl_SubCheckList	        ON BTblBazdidResultSubCheckList.SubCheckListId = 	BTbl_SubCheckList.SubCheckListId " &
                  " 	LEFT OUTER JOIN BTblService             ON BTblServiceCheckList.ServiceId = BTblService.ServiceId" &
                  " 	LEFT OUTER JOIN Tbl_Area                ON BTblBazdidResult.AreaId = Tbl_Area.AreaId" &
                  " 	LEFT OUTER JOIN Tbl_MPPost              ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId" &
                  "	    LEFT OUTER JOIN Tbl_PathType Tbl_PathType_From  ON BTblBazdidResult.FromPathTypeId = Tbl_PathType_From.PathTypeId" &
                  "	    LEFT OUTER JOIN Tbl_PathType Tbl_PathType_To    ON BTblBazdidResult.ToPathTypeId = Tbl_PathType_To.PathTypeId" &
                  " 	LEFT JOIN Tbl_FeederPart                        ON BTblBazdidResult.FeederPartId = Tbl_FeederPart.FeederPartId" &
                  "	    LEFT JOIN BTbl_BazdidCheckListGroup             ON BTbl_BazdidCheckList.BazdidCheckListGroupId = BTbl_BazdidCheckListGroup.BazdidCheckListGroupId" &
                  "	  " & mJoinSpecialitySql & "  " &
                       "   WHERE " &
                    "	(BTblBazdidResult.BazdidTypeId = 1)" &
                    "	AND BTblBazdidResultCheckList.Priority > 0" &
                    "	AND BTblBazdidResultSubCheckList.SubCheckListId NOT  IN   (SELECT  SubCheckListId from BTblServiceSubCheckList " & mWhereDT2 & "  )" &
                    "	" & mWhere &
                    "   Group by 	 Tbl_Area.AreaId," &
                            "	Tbl_Area.Area," &
                            "	Tbl_MPPost.MPPostName," &
                            "	Tbl_MPFeeder.MPFeederId," &
                            "   Tbl_MPFeeder.MPFeederName," &
                            "   Tbl_MPFeeder.HavaeiLength  ," &
                            "	Tbl_MPFeeder.ZeminiLength   ," &
                            "	BTblBazdidResult.BazdidResultId," &
                            "	BTblBazdidResult.FromToLengthHavayi   ," &
                            "	BTblBazdidResult.FromToLengthZamini   ," &
                            "	BTblBazdidResult.FromPathTypeId," &
                            "	Tbl_PathType_From.PathType ," &
                            "	BTblBazdidResult.FromPathTYpeValue," &
                            "	BTblBazdidResult.ToPathTypeId," &
                            "	Tbl_PathType_To.PathType ," &
                            "	BTblBazdidResult.ToPathTypeValue," &
                            "	BTblBazdidResultAddress.StartDatePersian," &
                            "	BTblBazdidResultAddress.Address," &
                            "	BTblBazdidResultAddress.GPSx," &
                            "	BTblBazdidResultAddress.GPSy," &
                            "	Tbl_FeederPart.FeederPart," &
                            "	BTblBazdidResultCheckList.BazdidResultCheckListId," &
                            "	BTblBazdidResultCheckList.Priority  ," &
                            "	BTbl_BazdidCheckList.CheckListCode," &
                            "	BTbl_BazdidCheckList.CheckListName," &
                            "   BTbl_SubCheckList.SubCheckListCode, " &
                            "   BTbl_SubCheckList.SubCheckListName, " &
                            "	BTbl_BazdidCheckListGroup.IsHavayi," &
                            "	BTblBazdidTiming.BazdidName" &
                            "   DROP TABLE #TmpTbl_MPFeedersId "


        BindingTable(lSQL, mCnn, lDS, "Report_2_1_9", , True)
        If lDS.Tables.Contains("Report_2_1_9") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_2_1_9.xml", XmlWriteMode.WriteSchema)
            Dim lDlg As New frmReportPreviewStim("", lFolder & "Report_2_1_9.mrt", cmbReportName.Text, cmbReportName.Text, , mFilterInfo, mFF, Strings.StrReverse(mReportNo), , , lDS)
            lDlg.ShowDialog()
        End If

    End Sub
    Private Sub MakeReport_2_2_1()
        MakeQuery()
        Dim lSQL As String = ""
        lSQL = "EXEC spGetReport_2_2_1 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & ",'" &
            mMPFeederIDs & "'," & mLPPostId & "," & mOwnershipId & "," & mIsActive & ",'" & mBazdidSpeciality & "'," &
            mFromDateBazdid & "," & mToDateBazdid & "," & mIsWarmLineSP & ""
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_2_2_1", , True)
        If lDS.Tables.Contains("Report_2_2_1") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_2_2_1.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_2_2_1.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_2_2_2()
        MakeQuery()
        Dim lSQL As String = ""
        lSQL = "EXEC spGetReport_2_2_2 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & ",'" &
            mMPFeederIDs & "'," & mLPPostId & "," & mOwnershipId & "," & mIsActive & "," & mIsHavaei & ",'" &
            mBazdidSpeciality & "'," & mFromDateBazdid & "," & mToDateBazdid & "," & mIsWarmLineSP & ""
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_2_2_2", , True)
        If lDS.Tables.Contains("Report_2_2_2") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_2_2_2.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_2_2_2.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_2_2_3()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_2_2_3 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mLPPostId & "," & mOwnershipId & "," & mIsActive & ",'" & mPrs & "','" & mCheckLists & "','" & mAddress & "'," & mMinCheckListCount & "," & mIsHavaei & ",'" & mBazdidSpeciality & "','" & mIsWarmLine & "'"
        Dim lDS As New DataSet

        lSQL = "  Select DISTINCT Tbl_Area.AreaId, " &
                 "	Tbl_Area.Area, " &
                 "	Tbl_MPPost.MPPostName, " &
                 "	Tbl_MPFeeder.MPFeederName, " &
                 "	Tbl_MPFeeder.MPFeederId, " &
                 "	Tbl_LPPost.LPPostName, " &
                 "	Tbl_LPPost.LPPostId, " &
                 "	Tbl_LPPost.Address, " &
                 "	BTblBazdidResultAddress.GPSx, " &
                 "	BTblBazdidResultAddress.GPSy, " &
                 "	GetDoneDatePersian.DoneDatePersian, " &
                 "	BTbl_BazdidCheckList.CheckListCode, " &
                 "	BTbl_BazdidCheckList.CheckListName, " &
                 "	CASE WHEN BTblBazdidResultCheckList.Priority = 4 THEN  " &
                 "		CAST('عدم وجود تجهيز' as nvarchar)  " &
                 "		ELSE CAST(BTblBazdidResultCheckList.Priority as nvarchar) END AS Priority, " &
                 "	SUM(CASE WHEN BTblServiceCheckList.ServiceCount Is Null OR BTblServiceCheckList.ServiceCount = 0 THEN 1 ELSE BTblServiceCheckList.ServiceCount END) AS CheckListCount " &
" 	FROM  " &
                 "	Tbl_LPPost " &
                 "	INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
                 "	INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
                 "	INNER JOIN BTblBazdidResult ON Tbl_LPPost.LPPostId = BTblBazdidResult.LPPostId " &
                 "	INNER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId " &
                 "	INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
                 "	INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
                 "	INNER JOIN BTbl_BazdidCheckList ON BTblBazdidResultCheckList.BazdidCheckListId = BTbl_BazdidCheckList.BazdidCheckListId " &
                 "	INNER JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId " &
                 "  LEFT JOIN BTblBazdidTiming  ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                 "	" & mJoinSpecialitySql &
                 "	INNER JOIN  " &
                 "	( " &
                 "		SELECT  " &
                 "			BTblServiceCheckList.BazdidResultCheckListId, " &
                 "			MAX(DoneDatePersian) AS DoneDatePersian " &
                 "		FROM  " &
                 "			BTblServiceCheckList " &
                 "		GROUP BY  " &
                 "			BTblServiceCheckList.BazdidResultCheckListId " &
                 "	) AS GetDoneDatePersian ON BTblBazdidResultCheckList.BazdidResultCheckListId = GetDoneDatePersian.BazdidResultCheckListId " &
                 "    WHERE 	BTblBazdidResult.BazdidStateId IN (2,3) " &
                 "	AND BTblBazdidResult.BazdidTypeId = 2 " &
                 "	AND BTblBazdidResultCheckList.Priority > 0 " &
                 "	AND (BTblServiceCheckList.ServiceStateId = 3 OR (BTblServiceCheckList.ServiceStateId = 2 AND BTblServiceCheckList.ServiceCount > 0)) " &
                 "	" & mAddress & " " & mWhere &
                 "   GROUP BY " &
                 "	Tbl_Area.AreaId, " &
                 "	Tbl_Area.Area, " &
                 "	Tbl_MPPost.MPPostName, " &
                 "	Tbl_MPFeeder.MPFeederName, " &
                 "	Tbl_MPFeeder.MPFeederId, " &
                 "	Tbl_LPPost.LPPostName, " &
                 "	Tbl_LPPost.LPPostId, " &
                 "	Tbl_LPPost.Address, " &
                 "	BTblBazdidResultAddress.GPSx, " &
                 "	BTblBazdidResultAddress.GPSy, " &
                 "	GetDoneDatePersian.DoneDatePersian, " &
                 "	BTbl_BazdidCheckList.CheckListCode, " &
                 "	BTbl_BazdidCheckList.CheckListName, " &
                 "	BTblBazdidResultCheckList.Priority " &
                 " " & mHaving



        BindingTable(lSQL, mCnn, lDS, "Report_2_2_3", , True)
        If lDS.Tables.Contains("Report_2_2_3") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_2_2_3.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_2_2_3.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_2_2_4()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_2_2_4 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mLPPostId & "," & mOwnershipId & "," & mIsActive & ",'" & mPrs & "','" & mCheckLists & "','" & mAddress & "','" & mSubCheckLists & "'," & mIsHavaei & ",'" & mBazdidSpeciality & "','" & mIsWarmLine & "'"
        Dim lDS As New DataSet


        lSQL = "    SELECT Tbl_Area.AreaId,Tbl_Area.Area,Tbl_MPPost.MPPostName,Tbl_MPFeeder.MPFeederName, " &
                    "		Tbl_MPFeeder.MPFeederId,Tbl_LPPost.LPPostName,Tbl_LPPost.LPPostId,Tbl_LPPost.Address, " &
                    "		BTblBazdidResultAddress.GPSx,BTblBazdidResultAddress.GPSy, " &
                    "		BTbl_BazdidCheckList.CheckListCode,BTbl_BazdidCheckList.CheckListName, " &
                    "		BTblBazdidResultCheckList.Priority, " &
                    "		ISNULL(BTbl_SubCheckList_New.SubCheckListCode,BTbl_SubCheckList_Old.SubCheckListCode) AS SubCheckListCode, " &
                    "		ISNULL(BTbl_SubCheckList_New.SubCheckListName,BTbl_SubCheckList_Old.SubCheckListName) AS SubCheckListName " &
                    "	FROM  " &
                    "		Tbl_LPPost " &
                    "		INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
                    "		INNER JOIN BTblBazdidResult ON Tbl_LPPost.LPPostId = BTblBazdidResult.LPPostId " &
                    "		INNER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId " &
                    "		INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
                    "		INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
                    "		INNER JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId " &
                    "		LEFT OUTER JOIN BTblBazdidResultSubCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblBazdidResultSubCheckList.BazdidResultCheckListId " &
                    "		INNER JOIN BTbl_BazdidCheckList ON BTblBazdidResultCheckList.BazdidCheckListId = BTbl_BazdidCheckList.BazdidCheckListId " &
                    "		LEFT OUTER JOIN BTbl_SubCheckList AS BTbl_SubCheckList_Old ON BTblBazdidResultCheckList.SubCheckListId = BTbl_SubCheckList_Old.SubCheckListId " &
                    "		LEFT OUTER JOIN BTbl_SubCheckList AS BTbl_SubCheckList_New ON BTblBazdidResultSubCheckList.SubCheckListId = BTbl_SubCheckList_New.SubCheckListId " &
                    "		LEFT OUTER JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                    "		LEFT OUTER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
                    "		" & mJoinSpecialitySql &
                    "      INNER JOIN   BTblServiceSubCheckList  on BTblBazdidResultSubCheckList.BazdidResultSubCheckListId = BTblServiceSubCheckList.BazdidResultSubCheckListId " &
                    "	WHERE  " &
                    "		BTblBazdidResult.BazdidStateId IN (2,3) " &
                    "		AND BTblBazdidResult.BazdidTypeId = 2 " &
                    "		AND BTblBazdidResultCheckList.Priority > 0 " &
                    "		AND BTblServiceCheckList.ServiceStateId = 3 " &
                    "		AND " &
                    "		( " &
                    "   	NOT ISNULL(BTbl_SubCheckList_New.SubCheckListCode,BTbl_SubCheckList_Old.SubCheckListCode) IS NULL " &
                    "	    OR NOT ISNULL(BTbl_SubCheckList_New.SubCheckListName,BTbl_SubCheckList_Old.SubCheckListName) IS NULL " &
                    "		) " &
                    " " & mAddress & mWhere.Replace("BTbl_SubCheckList.SubCheckListId", "ISNULL(BTbl_SubCheckList_New.SubCheckListId,BTbl_SubCheckList_Old.SubCheckListId)") &
                    " UNION ALL " &
                    " SELECT Tbl_Area.AreaId,Tbl_Area.Area,Tbl_MPPost.MPPostName,Tbl_MPFeeder.MPFeederName, " &
                    "		Tbl_MPFeeder.MPFeederId,Tbl_LPPost.LPPostName,Tbl_LPPost.LPPostId,Tbl_LPPost.Address, " &
                    "		BTblBazdidResultAddress.GPSx,BTblBazdidResultAddress.GPSy, " &
                    "		BTbl_BazdidCheckList.CheckListCode,BTbl_BazdidCheckList.CheckListName, " &
                    "		BTblBazdidResultCheckList.Priority, " &
                    "		ISNULL(BTbl_SubCheckList_New.SubCheckListCode,BTbl_SubCheckList_Old.SubCheckListCode) AS SubCheckListCode, " &
                    "		ISNULL(BTbl_SubCheckList_New.SubCheckListName,BTbl_SubCheckList_Old.SubCheckListName) AS SubCheckListName " &
                    "	FROM  " &
                    "		Tbl_LPPost " &
                    "		INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
                    "		INNER JOIN BTblBazdidResult ON Tbl_LPPost.LPPostId = BTblBazdidResult.LPPostId " &
                    "		INNER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId " &
                    "		INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
                    "		INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
                    "		INNER JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId " & "		LEFT OUTER JOIN BTblBazdidResultSubCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblBazdidResultSubCheckList.BazdidResultCheckListId " &
                    "		INNER JOIN BTbl_BazdidCheckList ON BTblBazdidResultCheckList.BazdidCheckListId = BTbl_BazdidCheckList.BazdidCheckListId " &
                    "		LEFT OUTER JOIN BTbl_SubCheckList AS BTbl_SubCheckList_Old ON BTblBazdidResultCheckList.SubCheckListId = BTbl_SubCheckList_Old.SubCheckListId " &
                    "		LEFT OUTER JOIN BTbl_SubCheckList AS BTbl_SubCheckList_New ON BTblBazdidResultSubCheckList.SubCheckListId = BTbl_SubCheckList_New.SubCheckListId " &
                    "		LEFT OUTER JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                    "		LEFT OUTER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
                    "		" & mJoinSpecialitySql &
                    "	WHERE  " &
                    "		BTblBazdidResult.BazdidStateId IN (2,3) " &
                    "		AND BTblBazdidResult.BazdidTypeId = 2 " &
                    "       AND NOT BTblServiceCheckList.ServiceCheckListId IN (SELECT ServiceCheckListId FROM BTblServiceSubCheckList   )" &
                    "		AND BTblBazdidResultCheckList.Priority > 0 " &
                    "		AND BTblServiceCheckList.ServiceStateId = 3 " &
                    "		AND " &
                    "		( " &
                    "   	NOT ISNULL(BTbl_SubCheckList_New.SubCheckListCode,BTbl_SubCheckList_Old.SubCheckListCode) IS NULL " &
                    "	    OR NOT ISNULL(BTbl_SubCheckList_New.SubCheckListName,BTbl_SubCheckList_Old.SubCheckListName) IS NULL " &
                    "		) " &
                    " " & mAddress & mWhere.Replace("BTbl_SubCheckList.SubCheckListId", "ISNULL(BTbl_SubCheckList_New.SubCheckListId,BTbl_SubCheckList_Old.SubCheckListId)")

        BindingTable(lSQL, mCnn, lDS, "Report_2_2_4", , True)
        If lDS.Tables.Contains("Report_2_2_4") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_2_2_4.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_2_2_4.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_2_2_5()
        MakeQuery()
        Dim lSQL As String = ""
        lSQL = "EXEC spGetReport_2_2_5 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & ",'" &
            mMPFeederIDs & "'," & mLPPostId & "," & mOwnershipId & "," & mIsActive & ",'" & mParts & "','" &
            mAddress & "'," & mIsHavaei & ",'" & mBazdidSpeciality & "','" & txtServiceNumber.Text & "'," &
            mFromDateBazdid & "," & mToDateBazdid & ",'" & mWorkCommand & "' ," & mIsWarmLineSP & ""
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_2_2_5", , True)
        If lDS.Tables.Contains("Report_2_2_5") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_2_2_5.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_2_2_5.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_2_2_6()
        MakeQuery()
        Dim lSQL As String = ""
        lSQL = "EXEC spGetReport_2_2_6 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & ",'" &
            mMPFeederIDs & "'," & mLPPostId & "," & mOwnershipId & "," & mIsActive & ",'" & mPrs & "','" &
            mCheckLists & "'," & mNotService & "," & mIsHavaei & ",'" & mBazdidSpeciality & "' ," & mIsWarmLineSP & ""
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_2_2_6", , True)
        If lDS.Tables.Contains("Report_2_2_6") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_2_2_6.xml", XmlWriteMode.WriteSchema)
            If chkNotService.Checked And chkBazdidName.Checked Then
                ShowReport("", lFolder & "Report_2_2_6_ReportNumber.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
            Else
                ShowReport("", lFolder & "Report_2_2_6.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
            End If
        End If
    End Sub
    Private Sub MakeReport_2_2_8()
        MakeQuery()
        Dim lSQL As String = ""
        Dim lDS As New DataSet
        Dim lPath As String = "Reports\Bazdid\Excel\"
        Dim lFileName As String = "spGetExcel_2_2_8.xlsx"
        Dim lSheet As String = "پست های توزیع"
        Dim lTitle As String = "شرکت توزيع نيروي برق - " & CConfig.ReadConfig("ToziName")
        Dim lDateRows(), lRow As DataRow

        lSQL = "EXEC spGetReport_2_2_8 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & ",'" &
            mMPFeederIDs & "'," & mLPPostId & "," & mOwnershipId & "," & mIsActive & ",'" & mPrs & "','" &
            mCheckLists & "','" & mAddress & "'," & mMinCheckListCount & "," & mIsHavaei & ",'" & mBazdidSpeciality & "'," &
            mFromDateBazdid & "," & mToDateBazdid & "," & mIsWarmLineSP & ""
        BindingTable(lSQL, mCnn, lDS, "Report_2_2_8", , True)
        lDateRows = lDS.Tables("Report_2_2_8").Select()

        If lDS.Tables.Contains("Report_2_2_8") Then
            If mExcel Is Nothing Then
                mExcel = New CExcelMng
            Else
                mExcel.CloseWorkBook()
            End If
            mExcel.OpenWorkBook(lPath & lFileName)
            mExcel.WriteCell(lSheet, "A1", mExcel.ReadCell(lSheet, "A1") + " " + lTitle, False, CExcelManager.TextHAlign.Center.Center, CExcelManager.TextVAlign.Center)
            Dim i As Integer = 1
            For Each lRow In lDateRows
                mExcel.WriteCell(lSheet, "A" & i + 2, i)
                mExcel.WriteCell(lSheet, "B" & i + 2, lRow.Item("MPFeederName"))
                mExcel.WriteCell(lSheet, "C" & i + 2, lRow.Item("LPPostName"))
                mExcel.WriteCell(lSheet, "D" & i + 2, lRow.Item("LPPostCode"))
                mExcel.WriteCell(lSheet, "E" & i + 2, lRow.Item("Address"))
                mExcel.WriteCell(lSheet, "F" & i + 2, lRow.Item("GPSx"))
                mExcel.WriteCell(lSheet, "G" & i + 2, lRow.Item("Gpsy"))
                mExcel.WriteCell(lSheet, "H" & i + 2, lRow.Item("StartDatePersian"))
                mExcel.WriteCell(lSheet, "I" & i + 2, lRow.Item("CheckListCode"))
                mExcel.WriteCell(lSheet, "J" & i + 2, lRow.Item("Priority"))
                mExcel.WriteCell(lSheet, "K" & i + 2, lRow.Item("CheckListName"))
                mExcel.WriteCell(lSheet, "L" & i + 2, lRow.Item("CreateDatePersian"))
                mExcel.WriteCell(lSheet, "M" & i + 2, lRow.Item("DoneDatePersian"))
                mExcel.WriteCell(lSheet, "N" & i + 2, lRow.Item("ServiceNotDoneReason"))
                i = i + 1
            Next
            mExcel.ShowExcel_2007Up()
        End If
    End Sub
    Private Sub MakeReport_2_2_9()
        MakeQuery()
        Dim lFolder As String = "Reports\Bazdid\"
        Dim lDs As New DataSet
        Dim lSQL As String = ""
        Dim lServiceCheckList As String = " OR NOT BTblBazdidResultCheckList.BazdidResultCheckListId IN " &
                " (SELECT DISTINCT BazdidResultCheckListId FROM BTblServiceCheckList  WHERE BTblServiceCheckList.ServiceStateId <> 3	)"

        lSQL = "	SELECT DISTINCT " &
                "		Tbl_Area.AreaId, " &
                "		Tbl_Area.Area, " &
                "		Tbl_MPPost.MPPostName, " &
                "		Tbl_MPFeeder.MPFeederName, " &
                "		Tbl_MPFeeder.MPFeederId, " &
                "		Tbl_LPPost.LPPostName, " &
                "		Tbl_LPPost.LPPostId, " &
                "      BTbl_SubCheckList.SubCheckListCode, " &
                "      BTbl_SubCheckList.SubCheckListName, " &
                "		Tbl_LPPost.Address, " &
                "		BTblBazdidResultAddress.GPSx, " &
                "		BTblBazdidResultAddress.GPSy, " &
                "		BTblBazdidResultAddress.StartDatePersian, " &
                "		BTbl_BazdidCheckList.CheckListCode, " &
                "		BTbl_BazdidCheckList.CheckListName, " &
                "		BTblBazdidTiming.BazdidName, " &
                "		SUM(BTblBazdidResultCheckList.DefectionCount) as  DefectionCount, " &
                "		CASE WHEN BTblBazdidResultCheckList.Priority = 4 THEN  " &
                "		CAST('عدم وجود تجهيز' as nvarchar)  " &
                "		ELSE CAST(BTblBazdidResultCheckList.Priority as nvarchar) END AS Priority, " &
                "		SUM(BTblBazdidResultCheckList.DefectionCount - IsNull(BTblServiceCheckList.ServiceCount,0)) AS CheckListCount " &
                "	FROM  Tbl_LPPost " &
                "		INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
                "		INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
                "		INNER JOIN BTblBazdidResult ON Tbl_LPPost.LPPostId = BTblBazdidResult.LPPostId " &
                "		INNER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId " &
                "		INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
                "		INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
                "       INNER JOIN BTblBazdidResultSubCheckList				on	BTblBazdidResultCheckList.BazdidResultCheckListId = 	BTblBazdidResultSubCheckList.BazdidResultCheckListId  " &
                "		INNER JOIN BTbl_BazdidCheckList ON BTblBazdidResultCheckList.BazdidCheckListId = BTbl_BazdidCheckList.BazdidCheckListId " &
                "		LEFT JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId " &
                "       INNER JOIN	BTblServiceSubCheckList					ON		BTblServiceCheckList.ServiceCheckListId = BTblServiceSubCheckList.ServiceCheckListId " &
                "		LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                "       INNER JOIN		BTbl_SubCheckList	ON		BTblBazdidResultSubCheckList.SubCheckListId = 	BTbl_SubCheckList.SubCheckListId " &
                "	  " & mJoinSpecialitySql & "  " &
                "	WHERE BTblBazdidResult.BazdidStateId IN (2,3) " &
                "		AND BTblBazdidResult.BazdidTypeId = 2 " &
                "		AND BTblBazdidResultCheckList.Priority > 0 " &
                "       AND BTblBazdidResultSubCheckList.SubCheckListId NOT  IN   (SELECT  SubCheckListId from BTblServiceSubCheckList " & mWhereDT2 & "  )" &
                " " & mWhere &
                "	GROUP BY " &
                "		Tbl_Area.AreaId, " &
                "		Tbl_Area.Area, " &
                "		Tbl_MPPost.MPPostName, " &
                "		Tbl_MPFeeder.MPFeederName, " &
                "		Tbl_MPFeeder.MPFeederId, " &
                "		Tbl_LPPost.LPPostName, " &
                "		Tbl_LPPost.LPPostId, " &
                "		Tbl_LPPost.Address, " &
                "		BTblBazdidResultAddress.GPSx, " &
                "		BTblBazdidResultAddress.GPSy, " &
                "		BTblBazdidResultAddress.StartDatePersian, " &
                "		BTbl_BazdidCheckList.CheckListCode, " &
                "		BTbl_BazdidCheckList.CheckListName, " &
                "       BTbl_SubCheckList.SubCheckListCode, " &
                "       BTbl_SubCheckList.SubCheckListName, " &
                "		BTblBazdidResultCheckList.Priority, " &
                "		BTblBazdidTiming.BazdidName"
        BindingTable(lSQL, mCnn, lDs, "Report_2_2_9", , , , , , , True)
        If lDs.Tables.Contains("Report_2_2_9") Then
            lDs.WriteXml(ReportsXMLPath & "Report_2_2_9.xml", XmlWriteMode.WriteSchema)
            Dim lDlg As New frmReportPreviewStim("", lFolder & "Report_2_2_9.mrt", cmbReportName.Text, cmbReportName.Text, , mFilterInfo, mFF, Strings.StrReverse(mReportNo), , , lDs)
            lDlg.ShowDialog()
        End If

    End Sub
    Private Sub MakeReport_2_34_1()
        MakeQuery()
        Dim lSQL As String = ""
        lSQL = "EXEC spGetReport_2_34_1 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & ",'" &
            mMPFeederIDs & "'," & mLPPostId & "," & mLPFeederId & "," & mOwnershipId & "," & mIsActive & "," &
            IIf(mIsLightReport, 1, 0) & ",'" & mBazdidMasterIDs & "','" & mBasketDetailIDs & "','" &
            mBazdidSpeciality & "'," & mFromDateBazdid & "," & mToDateBazdid & "," & mIsWarmLineSP & ""

        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_2_34_1", , True)
        If lDS.Tables.Contains("Report_2_34_1") Then
            Dim lFolder As String = "Reports\Bazdid\"
            mFF.AddFormulaFields("FeederType", IIf(mIsLightReport, """ فيدر روشنايي معابر """, """ فيدر فشار ضعيف """))
            lDS.WriteXml(ReportsXMLPath & "Report_2_34_1.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_2_34_1.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_2_34_2()
        MakeQuery()
        Dim lSQL As String = ""
        lSQL = "EXEC spGetReport_2_34_2 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & ",'" &
            mMPFeederIDs & "'," & mLPPostId & "," & mLPFeederId & "," & mOwnershipId & "," & mIsActive & "," &
            IIf(mIsLightReport, 1, 0) & ",'" & mBazdidMasterIDs & "','" & mBasketDetailIDs & "','" &
            mBazdidSpeciality & "','" & mIsHavaei & "'," & mFromDateBazdid & "," & mToDateBazdid & "," & mIsWarmLineSP & ""
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_2_34_2", , True)
        If lDS.Tables.Contains("Report_2_34_2") Then
            Dim lFolder As String = "Reports\Bazdid\"
            mFF.AddFormulaFields("FeederType", IIf(mIsLightReport, """ فيدر روشنايي معابر """, """ فيدر فشار ضعيف """))
            lDS.WriteXml(ReportsXMLPath & "Report_2_34_2.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_2_34_2.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_2_34_3()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_2_34_3 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mLPPostId & "," & mLPFeederId & "," & mOwnershipId & "," & mIsActive & "," & IIf(mIsLightReport, 1, 0) & ",'" & mBazdidMasterIDs & "','" & mBasketDetailIDs & "','" & mPrs & "','" & mCheckLists & "','" & mAddress & "'," & mMinCheckListCount & ",'" & mBazdidSpeciality & "','" & mIsWarmLine & "'"
        Dim lDS As New DataSet

        lSQL = "	SELECT DISTINCT " &
                "	Tbl_Area.AreaId, " &
                "	Tbl_Area.Area, " &
                "	Tbl_LPFeeder.LPPostId, " &
                "	Tbl_LPPost.LPPostName, " &
                "	Tbl_LPFeeder.LPFeederId, " &
                "	Tbl_LPFeeder.LPFeederName, " &
                "	Tbl_LPFeeder.Address as LPFeederAddress, " &
                "	BTblBazdidResult.BazdidResultId, " &
                "	ISNULL(Tbl_LPFeeder.HavaeiLength,0) AS LPFeederHavaeiLen, " &
                "	ISNULL(Tbl_LPFeeder.ZeminiLength,0) AS LPFeederZeminiLen, " &
                "	ISNULL(FromToLengthHavayi,Tbl_LPFeeder.HavaeiLength) AS HavaeiLength, " &
                "	ISNULL(FromToLengthZamini,Tbl_LPFeeder.ZeminiLength) AS ZeminiLength, " &
                "	BTblBazdidResult.FromPathTypeId, " &
                "	Tbl_PathType_From.PathType AS FromPathType, " &
                "	BTblBazdidResult.FromPathTYpeValue, " &
                "	BTblBazdidResult.ToPathTypeId, " &
                "	Tbl_PathType_To.PathType AS ToPathType, " &
                "	BTblBazdidResult.ToPathTypeValue, " &
                "	BTblBazdidResultAddress.Address, " &
                "	BTblBazdidResultAddress.GPSx, " &
                "	BTblBazdidResultAddress.GPSy, " &
                "	BTbl_BazdidCheckList.CheckListCode, " &
                "	BTbl_BazdidCheckList.CheckListName, " &
                "	CASE WHEN BTblBazdidResultCheckList.Priority = 4 THEN  " &
                "		CAST('عدم وجود تجهيز' as nvarchar)  " &
                "		ELSE CAST(BTblBazdidResultCheckList.Priority as nvarchar) END AS Priority, " &
                "	Tbl_LPFeeder.IsLightFeeder, " &
                "	GetDoneDatePersian.DoneDatePersian, " &
                "	BTbl_BazdidCheckListGroup.IsHavayi, " &
                "	SUM(CASE WHEN BTblServiceCheckList.ServiceCount Is Null OR BTblServiceCheckList.ServiceCount = 0 THEN 1 ELSE BTblServiceCheckList.ServiceCount END) AS ServiceCount " &
    "			FROM  " &
                "	Tbl_LPFeeder " &
                "	INNER JOIN Tbl_LPPost ON Tbl_LPFeeder.LPPostId = Tbl_LPPost.LPPostId " &
                "	INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
                "	INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
                "	INNER JOIN BTblBazdidResult ON Tbl_LPFeeder.LPFeederId = BTblBazdidResult.LPFeederId " &
                "	INNER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId " &
                "	INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
                "	INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
                "	INNER JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId " &
                "	LEFT OUTER JOIN Tbl_PathType Tbl_PathType_From ON BTblBazdidResult.FromPathTypeId = Tbl_PathType_From.PathTypeId " &
                "	LEFT OUTER JOIN Tbl_PathType Tbl_PathType_To ON BTblBazdidResult.ToPathTypeId = Tbl_PathType_To.PathTypeId " &
                "	INNER JOIN BTbl_BazdidCheckList ON BTblBazdidResultCheckList.BazdidCheckListId = BTbl_BazdidCheckList.BazdidCheckListId " &
                "	LEFT JOIN BTbl_BazdidCheckListGroup ON BTbl_BazdidCheckList.BazdidCheckListGroupId = BTbl_BazdidCheckListGroup.BazdidCheckListGroupId " &
                "   LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                "	" & mJoinSpecialitySql &
                "	INNER JOIN  " &
                "	( " &
                "		SELECT  " &
                "			BTblServiceCheckList.BazdidResultCheckListId, " &
                "			MAX(DoneDatePersian) AS DoneDatePersian " &
                "		FROM  " &
                "			BTblServiceCheckList " &
                "		GROUP BY  " &
                "			BTblServiceCheckList.BazdidResultCheckListId " &
                "	) AS GetDoneDatePersian ON BTblBazdidResultCheckList.BazdidResultCheckListId = GetDoneDatePersian.BazdidResultCheckListId " &
    "			WHERE  " &
                "	BTblBazdidResult.BazdidStateId IN (2,3) " &
                "	AND BTblBazdidResult.BazdidTypeId = 3 " &
                "	AND BTblBazdidResultCheckList.Priority > 0 " &
                "	AND (BTblServiceCheckList.ServiceStateId = 3 OR (BTblServiceCheckList.ServiceStateId = 2 AND BTblServiceCheckList.ServiceCount > 0)) " &
                "	AND Tbl_LPFeeder.IsLightFeeder = " & IIf(mIsLightReport, 1, 0) &
                 "	" & mAddress & " " & mWhere &
                "   GROUP BY " &
                "	Tbl_Area.AreaId, " &
                "	Tbl_Area.Area, " &
                "	Tbl_LPFeeder.LPPostId, " &
                "	Tbl_LPPost.LPPostName, " &
                "	Tbl_LPFeeder.LPFeederId, " &
                "	Tbl_LPFeeder.LPFeederName, " &
                "	Tbl_LPFeeder.Address, " &
                "	BTblBazdidResult.BazdidResultId, " &
                "	ISNULL(Tbl_LPFeeder.HavaeiLength,0), " &
                "	ISNULL(Tbl_LPFeeder.ZeminiLength,0), " &
                "	ISNULL(FromToLengthHavayi,Tbl_LPFeeder.HavaeiLength), " &
                "	ISNULL(FromToLengthZamini,Tbl_LPFeeder.ZeminiLength), " &
                "	BTblBazdidResult.FromPathTypeId, " &
                "	Tbl_PathType_From.PathType, " &
                "	BTblBazdidResult.FromPathTYpeValue, " &
                "	BTblBazdidResult.ToPathTypeId, " &
                "	Tbl_PathType_To.PathType, " &
                "	BTblBazdidResult.ToPathTypeValue, " &
                "	BTblBazdidResultAddress.Address, " &
                "	BTblBazdidResultAddress.GPSx, " &
                "	BTblBazdidResultAddress.GPSy, " &
                "	BTbl_BazdidCheckList.CheckListCode, " &
                "	BTbl_BazdidCheckList.CheckListName, " &
                "	CASE WHEN BTblBazdidResultCheckList.Priority = 4 THEN  " &
                "		CAST('عدم وجود تجهيز' as nvarchar)  " &
                "		ELSE CAST(BTblBazdidResultCheckList.Priority as nvarchar) END, " &
                "	Tbl_LPFeeder.IsLightFeeder, " &
                "	GetDoneDatePersian.DoneDatePersian, " &
                "	BTbl_BazdidCheckListGroup.IsHavayi   " &
                "	" & mHaving

        BindingTable(lSQL, mCnn, lDS, "Report_2_34_3", , True)
        If lDS.Tables.Contains("Report_2_34_3") Then
            Dim lFolder As String = "Reports\Bazdid\"
            mFF.AddFormulaFields("FeederType", IIf(mIsLightReport, """ فيدر روشنايي معابر """, """ فيدر فشار ضعيف """))
            lDS.WriteXml(ReportsXMLPath & "Report_2_34_3.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_2_34_3.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_2_34_4()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_2_34_4 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mLPPostId & "," & mLPFeederId & "," & mOwnershipId & "," & mIsActive & "," & IIf(mIsLightReport, 1, 0) & ",'" & mBazdidMasterIDs & "','" & mBasketDetailIDs & "','" & mPrs & "','" & mCheckLists & "','" & mAddress & "','" & mSubCheckLists & "','" & mBazdidSpeciality & "','" & mIsWarmLine & "'"
        Dim lDS As New DataSet

        lSQL = " SELECT Tbl_Area.AreaId, " &
                "		Tbl_Area.Area, " &
                "		Tbl_LPPost.LPPostName, " &
                "		Tbl_LPFeeder.LPFeederName, " &
                "		Tbl_LPFeeder.LPFeederId, " &
                "		BTblBazdidResult.FromToLengthHavayi, " &
                "		ISNULL(Tbl_LPFeeder.HavaeiLength,0) AS LPFeederHavayiLen, " &
                "		BTblBazdidResult.FromToLengthZamini, " &
                "		ISNULL(Tbl_LPFeeder.ZeminiLength,0) AS LPFeederZaminiLen, " &
                "		ISNULL(BTblBazdidResult.FromToLengthHavayi,Tbl_LPFeeder.HavaeiLength) AS HavaeiLength, " &
                "		ISNULL(BTblBazdidResult.FromToLengthZamini,Tbl_LPFeeder.ZeminiLength) AS ZeminiLength, " &
                "		ISNULL(BTbl_SubCheckList_New.SubCheckListCode,BTbl_SubCheckList_Old.SubCheckListCode) AS SubCheckListCode, " &
                "		ISNULL(BTbl_SubCheckList_New.SubCheckListName,BTbl_SubCheckList_Old.SubCheckListName) AS SubCheckListName, " &
                "		BTblBazdidResult.FromPathTypeId, " &
                "		Tbl_PathType_From.PathType AS FromPathType, " &
                "		BTblBazdidResult.FromPathTYpeValue, " &
                "		BTblBazdidResult.ToPathTypeId, " &
                "		Tbl_PathType_To.PathType AS ToPathType, " &
                "		BTblBazdidResult.ToPathTypeValue, " &
                "		BTblBazdidResultAddress.Address, " &
                "		BTblBazdidResultAddress.GPSx, " &
                "		BTblBazdidResultAddress.GPSy, " &
                "		BTbl_BazdidCheckList.CheckListCode, " &
                "		BTbl_BazdidCheckList.CheckListName, " &
                "		BTblBazdidResultCheckList.Priority, " &
                "		BTbl_BazdidCheckListGroup.IsHavayi " &
                "	FROM  " &
                "		Tbl_LPFeeder " &
                "		INNER JOIN BTblBazdidResult ON Tbl_LPFeeder.LPFeederId = BTblBazdidResult.LPFeederId " &
                "		INNER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId " &
                "		INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
                "		INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
                "		INNER JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId " &
                "		LEFT OUTER JOIN BTblBazdidResultSubCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblBazdidResultSubCheckList.BazdidResultCheckListId " &
                "		LEFT OUTER JOIN Tbl_PathType Tbl_PathType_From ON BTblBazdidResult.FromPathTypeId = Tbl_PathType_From.PathTypeId " &
                "		LEFT OUTER JOIN Tbl_PathType Tbl_PathType_To ON BTblBazdidResult.ToPathTypeId = Tbl_PathType_To.PathTypeId " &
                "		LEFT OUTER JOIN BTbl_BazdidCheckList ON BTblBazdidResultCheckList.BazdidCheckListId = BTbl_BazdidCheckList.BazdidCheckListId " &
                "		LEFT OUTER JOIN BTbl_SubCheckList AS BTbl_SubCheckList_Old ON BTblBazdidResultCheckList.SubCheckListId = BTbl_SubCheckList_Old.SubCheckListId " &
                "		LEFT OUTER JOIN BTbl_SubCheckList AS BTbl_SubCheckList_New ON BTblBazdidResultSubCheckList.SubCheckListId = BTbl_SubCheckList_New.SubCheckListId " &
                "		INNER JOIN Tbl_LPPost ON Tbl_LPFeeder.LPPostId = Tbl_LPPost.LPPostId " &
                "		INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
                "	    INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
                "		LEFT JOIN BTbl_BazdidCheckListGroup ON BTbl_BazdidCheckList.BazdidCheckListGroupId = BTbl_BazdidCheckListGroup.BazdidCheckListGroupId " &
                "		LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                "	" & mJoinSpecialitySql &
                "       INNER JOIN		BTblServiceSubCheckList  on BTblBazdidResultSubCheckList.BazdidResultSubCheckListId = BTblServiceSubCheckList.BazdidResultSubCheckListId " &
                "	WHERE  " &
                "		BTblBazdidResult.BazdidStateId IN (2,3) " &
                "		AND BTblBazdidResult.BazdidTypeId = 3 " &
                "		AND BTblBazdidResultCheckList.Priority > 0 " &
                "		AND BTblServiceCheckList.ServiceStateId = 3 " &
                "		AND  " &
                "		( " &
                "		NOT ISNULL(BTbl_SubCheckList_New.SubCheckListCode,BTbl_SubCheckList_Old.SubCheckListCode) IS NULL " &
                "		OR NOT ISNULL(BTbl_SubCheckList_New.SubCheckListName,BTbl_SubCheckList_Old.SubCheckListName) IS NULL " &
                "		) " &
                "		AND Tbl_LPFeeder.IsLightFeeder = " & IIf(mIsLightReport, 1, 0) & " " & mAddress & mWhere.Replace("BTbl_SubCheckList.SubCheckListId", "ISNULL(BTbl_SubCheckList_New.SubCheckListId,BTbl_SubCheckList_Old.SubCheckListId)") &
                " UNION ALL " &
                " SELECT Tbl_Area.AreaId, " &
                "		Tbl_Area.Area, " &
                "		Tbl_LPPost.LPPostName, " &
                "		Tbl_LPFeeder.LPFeederName, " &
                "		Tbl_LPFeeder.LPFeederId, " &
                "		BTblBazdidResult.FromToLengthHavayi, " &
                "		ISNULL(Tbl_LPFeeder.HavaeiLength,0) AS LPFeederHavayiLen, " &
                "		BTblBazdidResult.FromToLengthZamini, " &
                "		ISNULL(Tbl_LPFeeder.ZeminiLength,0) AS LPFeederZaminiLen, " &
                "		ISNULL(BTblBazdidResult.FromToLengthHavayi,Tbl_LPFeeder.HavaeiLength) AS HavaeiLength, " &
                "		ISNULL(BTblBazdidResult.FromToLengthZamini,Tbl_LPFeeder.ZeminiLength) AS ZeminiLength, " &
                "		ISNULL(BTbl_SubCheckList_New.SubCheckListCode,BTbl_SubCheckList_Old.SubCheckListCode) AS SubCheckListCode, " &
                "		ISNULL(BTbl_SubCheckList_New.SubCheckListName,BTbl_SubCheckList_Old.SubCheckListName) AS SubCheckListName, " &
                "		BTblBazdidResult.FromPathTypeId, " &
                "		Tbl_PathType_From.PathType AS FromPathType, " &
                "		BTblBazdidResult.FromPathTYpeValue, " &
                "		BTblBazdidResult.ToPathTypeId, " &
                "		Tbl_PathType_To.PathType AS ToPathType, " &
                "		BTblBazdidResult.ToPathTypeValue, " &
                "		BTblBazdidResultAddress.Address, " &
                "		BTblBazdidResultAddress.GPSx, " &
                "		BTblBazdidResultAddress.GPSy, " &
                "		BTbl_BazdidCheckList.CheckListCode, " &
                "		BTbl_BazdidCheckList.CheckListName, " &
                "		BTblBazdidResultCheckList.Priority, " &
                "		BTbl_BazdidCheckListGroup.IsHavayi " &
                "	FROM  " &
                "		Tbl_LPFeeder " &
                "		INNER JOIN BTblBazdidResult ON Tbl_LPFeeder.LPFeederId = BTblBazdidResult.LPFeederId " &
                "		INNER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId " &
                "		INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
                "		INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
                "		INNER JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId " &
                "		LEFT OUTER JOIN BTblBazdidResultSubCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblBazdidResultSubCheckList.BazdidResultCheckListId " &
                "		LEFT OUTER JOIN Tbl_PathType Tbl_PathType_From ON BTblBazdidResult.FromPathTypeId = Tbl_PathType_From.PathTypeId " &
                "		LEFT OUTER JOIN Tbl_PathType Tbl_PathType_To ON BTblBazdidResult.ToPathTypeId = Tbl_PathType_To.PathTypeId " &
                "		LEFT OUTER JOIN BTbl_BazdidCheckList ON BTblBazdidResultCheckList.BazdidCheckListId = BTbl_BazdidCheckList.BazdidCheckListId " &
                "		LEFT OUTER JOIN BTbl_SubCheckList AS BTbl_SubCheckList_Old ON BTblBazdidResultCheckList.SubCheckListId = BTbl_SubCheckList_Old.SubCheckListId " &
                "		LEFT OUTER JOIN BTbl_SubCheckList AS BTbl_SubCheckList_New ON BTblBazdidResultSubCheckList.SubCheckListId = BTbl_SubCheckList_New.SubCheckListId " &
                "		INNER JOIN Tbl_LPPost ON Tbl_LPFeeder.LPPostId = Tbl_LPPost.LPPostId " &
                "		INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
                "	    INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
                "		LEFT JOIN BTbl_BazdidCheckListGroup ON BTbl_BazdidCheckList.BazdidCheckListGroupId = BTbl_BazdidCheckListGroup.BazdidCheckListGroupId " &
                "		LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                "	" & mJoinSpecialitySql &
                "	WHERE  " &
                "		BTblBazdidResult.BazdidStateId IN (2,3) " &
                "		AND BTblBazdidResult.BazdidTypeId = 3 " &
                "       AND NOT BTblServiceCheckList.ServiceCheckListId IN (SELECT ServiceCheckListId FROM BTblServiceSubCheckList) " &
                "		AND BTblBazdidResultCheckList.Priority > 0 " &
                "		AND BTblServiceCheckList.ServiceStateId = 3 " &
                "		AND  " &
                "		( " &
                "		NOT ISNULL(BTbl_SubCheckList_New.SubCheckListCode,BTbl_SubCheckList_Old.SubCheckListCode) IS NULL " &
                "		OR NOT ISNULL(BTbl_SubCheckList_New.SubCheckListName,BTbl_SubCheckList_Old.SubCheckListName) IS NULL " &
                "		) " &
                "		AND Tbl_LPFeeder.IsLightFeeder = " & IIf(mIsLightReport, 1, 0) & " " & mAddress & mWhere.Replace("BTbl_SubCheckList.SubCheckListId", "ISNULL(BTbl_SubCheckList_New.SubCheckListId,BTbl_SubCheckList_Old.SubCheckListId)")

        BindingTable(lSQL, mCnn, lDS, "Report_2_34_4", , True)
        If lDS.Tables.Contains("Report_2_34_4") Then
            Dim lFolder As String = "Reports\Bazdid\"
            mFF.AddFormulaFields("FeederType", IIf(mIsLightReport, """ فيدر روشنايي معابر """, """ فيدر فشار ضعيف """))
            lDS.WriteXml(ReportsXMLPath & "Report_2_34_4.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_2_34_4.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_2_34_5()
        MakeQuery()
        Dim lSQL As String = ""
        lSQL = "EXEC spGetReport_2_34_5 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & ",'" &
            mMPFeederIDs & "'," & mLPPostId & "," & mLPFeederId & "," & mOwnershipId & "," & mIsActive & "," &
            IIf(mIsLightReport, 1, 0) & ",'" & mBazdidMasterIDs & "','" & mBasketDetailIDs & "','" & mParts & "','" &
            mAddress & "','" & mBazdidSpeciality & "','" & txtServiceNumber.Text & "'," & mFromDateBazdid & "," &
            mToDateBazdid & ",'" & mWorkCommand & "' ," & mIsWarmLineSP & ""
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_2_34_5", , True)
        If lDS.Tables.Contains("Report_2_34_5") Then
            Dim lFolder As String = "Reports\Bazdid\"
            mFF.AddFormulaFields("FeederType", IIf(mIsLightReport, """ روشنايي معابر """, """ فشار ضعيف """))
            lDS.WriteXml(ReportsXMLPath & "Report_2_34_5.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_2_34_5.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_2_34_6()
        MakeQuery()
        Dim lSQL As String = ""
        lSQL = "EXEC spGetReport_2_34_6 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & ",'" &
            mMPFeederIDs & "'," & mLPPostId & "," & mLPFeederId & "," & mOwnershipId & "," & mIsActive & "," &
            IIf(mIsLightReport, 1, 0) & ",'" & mBazdidMasterIDs & "','" & mBasketDetailIDs & "','" & mPrs & "','" &
            mCheckLists & "'," & mNotService & ",'" & mBazdidSpeciality & "'," & mFromDateBazdid & "," &
            mToDateBazdid & "," & mIsWarmLineSP & ""
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_2_34_6", , True)
        If lDS.Tables.Contains("Report_2_34_6") Then
            Dim lFolder As String = "Reports\Bazdid\"
            mFF.AddFormulaFields("FeederType", IIf(mIsLightReport, """ فيدر روشنايي معابر """, """ فيدر فشار ضعيف """))
            lDS.WriteXml(ReportsXMLPath & "Report_2_34_6.xml", XmlWriteMode.WriteSchema)
            If chkNotService.Checked And chkBazdidName.Checked Then
                ShowReport("", lFolder & "Report_2_34_6_ReportNumber.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
            Else
                ShowReport("", lFolder & "Report_2_34_6.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
            End If
        End If
    End Sub
    Private Sub MakeReport_2_3_8()
        MakeQuery()
        Dim lSQL As String = ""
        Dim lDS As New DataSet
        Dim lPath As String = "Reports\Bazdid\Excel\"
        Dim lFileName As String = "spGetExcel_2_3_8.xlsx"
        Dim lSheet As String = "شبکه فشار ضعیف"
        Dim lTitle As String = "شرکت توزيع نيروي برق - " & CConfig.ReadConfig("ToziName")
        Dim lDateRows(), lRow As DataRow

        lSQL = "EXEC spGetReport_2_3_8 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & ",'" &
            mMPFeederIDs & "'," & mLPPostId & "," & mLPFeederId & "," & mOwnershipId & "," & mIsActive & "," &
            IIf(mIsLightReport, 1, 0) & ",'" & mBazdidMasterIDs & "','" & mBasketDetailIDs & "','" & mPrs & "','" &
            mCheckLists & "','" & mAddress & "'," & mMinCheckListCount & ",'" & mBazdidSpeciality & "'," &
            mFromDateBazdid & "," & mToDateBazdid & "," & mIsWarmLineSP & ""
        BindingTable(lSQL, mCnn, lDS, "spGetReport_2_3_8", , True)
        lDateRows = lDS.Tables("spGetReport_2_3_8").Select()

        If lDS.Tables.Contains("spGetReport_2_3_8") Then
            If mExcel Is Nothing Then
                mExcel = New CExcelMng
            Else
                mExcel.CloseWorkBook()
            End If
            mExcel.OpenWorkBook(lPath & lFileName)
            Dim i As Integer = 1
            mExcel.WriteCell(lSheet, "A1", mExcel.ReadCell(lSheet, "A1") + " " + lTitle, False, CExcelManager.TextHAlign.Center.Center, CExcelManager.TextVAlign.Center)
            For Each lRow In lDateRows
                mExcel.WriteCell(lSheet, "A" & i + 2, i)
                mExcel.WriteCell(lSheet, "B" & i + 2, lRow.Item("LPFeederName"))
                mExcel.WriteCell(lSheet, "C" & i + 2, lRow.Item("LPPostName"))
                mExcel.WriteCell(lSheet, "D" & i + 2, lRow.Item("LPPostCode"))
                mExcel.WriteCell(lSheet, "F" & i + 2, lRow.Item("Address"))
                mExcel.WriteCell(lSheet, "G" & i + 2, lRow.Item("GPSx"))
                mExcel.WriteCell(lSheet, "H" & i + 2, lRow.Item("Gpsy"))
                mExcel.WriteCell(lSheet, "I" & i + 2, lRow.Item("StartDatePersian"))
                mExcel.WriteCell(lSheet, "J" & i + 2, lRow.Item("CheckListCode"))
                mExcel.WriteCell(lSheet, "K" & i + 2, lRow.Item("Priority"))
                mExcel.WriteCell(lSheet, "L" & i + 2, lRow.Item("CheckListName"))
                mExcel.WriteCell(lSheet, "M" & i + 2, lRow.Item("CreateDatePersian"))
                mExcel.WriteCell(lSheet, "N" & i + 2, lRow.Item("DoneDatePersian"))
                mExcel.WriteCell(lSheet, "O" & i + 2, lRow.Item("ServiceNotDoneReason"))
                i = i + 1
            Next
            mExcel.ShowExcel_2007Up()
        End If
    End Sub
    Private Sub MakeReport_2_34_9()
        MakeQuery()
        Dim lSQL As String = ""
        Dim lFolder As String = "Reports\Bazdid\"
        Dim lServiceCheckList As String = " OR NOT BTblBazdidResultCheckList.BazdidResultCheckListId IN " &
              " (SELECT DISTINCT BazdidResultCheckListId FROM BTblServiceCheckList  WHERE BTblServiceCheckList.ServiceStateId <> 3	)"
        Dim lDS As New DataSet

        lSQL = " 	SELECT  " &
                    "       Tbl_Area.AreaId, " &
                    "		Tbl_Area.Area, " &
                    "		Tbl_LPFeeder.LPPostId, " &
                    "		Tbl_LPPost.LPPostName, " &
                    "		Tbl_LPFeeder.LPFeederId, " &
                    "		Tbl_LPFeeder.LPFeederName, " &
                    "       BTbl_SubCheckList.SubCheckListCode, " &
                    "       BTbl_SubCheckList.SubCheckListName, " &
                    "		BTblBazdidResult.BazdidResultId, " &
                    "		ISNULL(Tbl_LPFeeder.HavaeiLength,0) AS LPFeederHavaeiLen, " &
                    "		ISNULL(Tbl_LPFeeder.ZeminiLength,0) AS LPFeederZeminiLen, " &
                    "		ISNULL(FromToLengthHavayi,Tbl_LPFeeder.HavaeiLength) AS HavaeiLength, " &
                    "		ISNULL(FromToLengthZamini,Tbl_LPFeeder.ZeminiLength) AS ZeminiLength, " &
                    "		BTblBazdidResult.FromPathTypeId, " &
                    "		Tbl_PathType_From.PathType AS FromPathType, " &
                    "		BTblBazdidResult.FromPathTYpeValue, " &
                    "		BTblBazdidResult.ToPathTypeId, " &
                    "		Tbl_PathType_To.PathType AS ToPathType, " &
                    "		BTblBazdidResult.ToPathTypeValue, " &
                    "		BTblBazdidResultAddress.StartDatePersian, " &
                    "		BTblBazdidResultAddress.Address, " &
                    "		BTblBazdidResultAddress.GPSx, " &
                    "		BTblBazdidResultAddress.GPSy, " &
                    "		BTbl_BazdidCheckList.CheckListCode, " &
                    "		BTbl_BazdidCheckList.CheckListName, " &
                    "		CASE WHEN BTblBazdidResultCheckList.Priority = 4 THEN  " &
                    "		CAST('عدم وجود تجهيز' as nvarchar)  " &
                    "		ELSE CAST(BTblBazdidResultCheckList.Priority as nvarchar) END AS Priority, " &
                    "		Tbl_LPFeeder.IsLightFeeder, " &
                    "		BTbl_BazdidCheckListGroup.IsHavayi, " &
                    "		BTblBazdidTiming.BazdidName, " &
                    "		SUM(BTblBazdidResultCheckList.DefectionCount) as  DefectionCount, " &
                    "		SUM(BTblBazdidResultCheckList.DefectionCount - IsNull(BTblServiceCheckList.ServiceCount,0)) AS CheckListCount " &
                    "	FROM Tbl_LPFeeder " &
                    "		INNER JOIN Tbl_LPPost ON Tbl_LPFeeder.LPPostId = Tbl_LPPost.LPPostId " &
                    "		INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
                    "       LEFT OUTER JOIN  Tbl_MPPost   ON    Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
                    "		INNER JOIN BTblBazdidResult ON Tbl_LPFeeder.LPFeederId = BTblBazdidResult.LPFeederId " &
                    "		INNER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId " &
                    "		INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
                    "		INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
                    "       INNER JOIN BTblBazdidResultSubCheckList		ON	BTblBazdidResultCheckList.BazdidResultCheckListId = 	BTblBazdidResultSubCheckList.BazdidResultCheckListId  " &
                    "		LEFT JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId " &
                    "		LEFT OUTER JOIN Tbl_PathType Tbl_PathType_From ON BTblBazdidResult.FromPathTypeId = Tbl_PathType_From.PathTypeId " &
                    "		LEFT OUTER JOIN Tbl_PathType Tbl_PathType_To ON BTblBazdidResult.ToPathTypeId = Tbl_PathType_To.PathTypeId " &
                    "		INNER JOIN BTbl_BazdidCheckList ON BTblBazdidResultCheckList.BazdidCheckListId = BTbl_BazdidCheckList.BazdidCheckListId " &
                    "       INNER JOIN		BTbl_SubCheckList	ON		BTblBazdidResultSubCheckList.SubCheckListId = 	BTbl_SubCheckList.SubCheckListId " &
                    "		LEFT OUTER JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                    "		LEFT JOIN BTbl_BazdidCheckListGroup ON BTbl_BazdidCheckList.BazdidCheckListGroupId = BTbl_BazdidCheckListGroup.BazdidCheckListGroupId " &
                    "	  " & mJoinSpecialitySql & "  " &
                    "	WHERE  " &
                    "		BTblBazdidResult.BazdidStateId IN (2,3) " &
                    "		AND BTblBazdidResult.BazdidTypeId = 3 " &
                    "		AND BTblBazdidResultCheckList.Priority > 0 " &
                    "		AND Tbl_LPFeeder.IsLightFeeder = " & IIf(mIsLightReport, 1, 0) &
                    "       AND BTblBazdidResultSubCheckList.SubCheckListId NOT  IN   (SELECT  SubCheckListId from BTblServiceSubCheckList " & mWhereDT2 & "  )" &
                    "		" & mWhere &
                    "		Group By  " &
                    "		Tbl_Area.AreaId, " &
                    "		Tbl_Area.Area, " &
                    "		Tbl_LPFeeder.LPPostId, " &
                    "		Tbl_LPPost.LPPostName, " &
                    "		Tbl_LPFeeder.LPFeederId, " &
                    "		Tbl_LPFeeder.LPFeederName, " &
                    "		BTblBazdidResult.BazdidResultId, " &
                    "		Tbl_LPFeeder.HavaeiLength, " &
                    "		Tbl_LPFeeder.ZeminiLength, " &
                    "		FromToLengthHavayi, " &
                    "		Tbl_LPFeeder.HavaeiLength, " &
                    "		FromToLengthZamini, " &
                    "		Tbl_LPFeeder.ZeminiLength, " &
                    "		BTblBazdidResult.FromPathTypeId, " &
                    "		Tbl_PathType_From.PathType, " &
                    "		BTblBazdidResult.FromPathTYpeValue, " &
                    "		BTblBazdidResult.ToPathTypeId, " &
                    "		Tbl_PathType_To.PathType , " &
                    "		BTblBazdidResult.ToPathTypeValue, " &
                    "		BTblBazdidResultAddress.StartDatePersian, " &
                    "		BTblBazdidResultAddress.Address, " &
                    "		BTblBazdidResultAddress.GPSx, " &
                    "		BTblBazdidResultAddress.GPSy, " &
                    "		BTbl_BazdidCheckList.CheckListCode, " &
                    "		BTbl_BazdidCheckList.CheckListName, " &
                    "       BTbl_SubCheckList.SubCheckListCode, " &
                    "       BTbl_SubCheckList.SubCheckListName, " &
                    "		BTblBazdidResultCheckList.Priority, " &
                    "		Tbl_LPFeeder.IsLightFeeder, " &
                    "		BTbl_BazdidCheckListGroup.IsHavayi, " &
                    "		BTblBazdidTiming.BazdidName "

        BindingTable(lSQL, mCnn, lDS, "Report_2_34_9", , True)
        If lDS.Tables.Contains("Report_2_34_9") Then
            mFF.AddFormulaFields("FeederType", IIf(mIsLightReport, " فيدر روشنايي معابر ", " فيدر فشار ضعيف "))
            lDS.WriteXml(ReportsXMLPath & "Report_2_34_9.xml", XmlWriteMode.WriteSchema)
            Dim lDlg As New frmReportPreviewStim("", lFolder & "Report_2_34_9.mrt", cmbReportName.Text, cmbReportName.Text, , mFilterInfo, mFF, mReportNo, , , lDS)
            lDlg.ShowDialog()
        End If
    End Sub

    Private Sub MakeReport_3_1_1()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_3_1_1 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mOwnershipId & "," & mIsActive & ",'" & mBazdidSpeciality & "'"

        Dim lDS As New DataSet

        lSQL =
            "SELECT  " &
            "		Tbl_Area.Area, " &
            "		ISNULL(MPFHavayiCount,0) AS MPFHavayiCount, " &
            "		ISNULL(MPFZaminiCount,0) AS MPFZaminiCount, " &
            "		ISNULL(MPFHavayiLen,0) AS MPFHavayiLen, " &
            "		ISNULL(MPFZaminiLen,0) AS MPFZaminiLen, " &
            "		Tbl_Bazdid.StartDatePersian, " &
            "		Tbl_Bazdid.EndDatePersian, " &
            "		ISNULL(BazdidHavayiCount,0) AS BazdidHavayiCount, " &
            "		ISNULL(BazdidZaminiCount,0) AS BazdidZaminiCount, " &
            "		ISNULL(BazdidHavayiLen,0) AS BazdidHavayiLen, " &
            "		ISNULL(BazdidZaminiLen,0) AS BazdidZaminiLen " &
            "	FROM " &
            "		Tbl_Area " &
            "		INNER JOIN " &
            "		( " &
    "		SELECT  " &
            "			BTblBazdidResult.AreaId, " &
            "			COUNT(DISTINCT CASE WHEN ISNULL(Tbl_MPFeeder.HavaeiLength,0) > ISNULL(Tbl_MPFeeder.ZeminiLength,0) THEN Tbl_MPFeeder.MPFeederId END) AS MPFHavayiCount, " &
            "			COUNT(DISTINCT CASE WHEN ISNULL(Tbl_MPFeeder.HavaeiLength,0) <= ISNULL(Tbl_MPFeeder.ZeminiLength,0) THEN Tbl_MPFeeder.MPFeederId END) AS MPFZaminiCount, " &
            "			SUM(CASE WHEN ISNULL(BTblBazdidResult.FromToLengthZamini,0) > 0 THEN BTblBazdidResult.FromToLengthZamini  ELSE  Tbl_MPFeeder.ZeminiLength END) AS MPFZaminiLen, " &
            "			SUM(CASE WHEN ISNULL(BTblBazdidResult.FromToLengthHavayi,0) > 0 THEN BTblBazdidResult.FromToLengthHavayi  ELSE  Tbl_MPFeeder.HavaeiLength END) AS MPFHavayiLen " &
            "		FROM  " &
            "			BTblBazdidResult " &
            "			INNER JOIN Tbl_MPFeeder ON BTblBazdidResult.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "      		LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
            "		WHERE  " &
            "			(BTblBazdidResult.BazdidTypeId = 1) " &
            "			 " & mWhere & Replace(mWhereDT, "BTblBazdidResultAddress", "BTblBazdidTiming") &
            "		GROUP BY  " &
            "			BTblBazdidResult.AreaId " &
            "		) AS Tbl_MPFeederLen ON Tbl_Area.AreaId = Tbl_MPFeederLen.AreaId " &
            "		INNER JOIN " &
            "		( " &
            "		SELECT  " &
            "			BTblBazdidResult.AreaId, " &
            "			MIN(Tbl_StartEndDate.StartDate) StartDatePersian, " &
            "			MAX(Tbl_StartEndDate.EndDate) EndDatePersian, " &
            "			COUNT(DISTINCT CASE WHEN ISNULL(Tbl_MPFeeder.HavaeiLength,0) > ISNULL(Tbl_MPFeeder.ZeminiLength,0) THEN Tbl_MPFeeder.MPFeederId END) AS BazdidHavayiCount, " &
            "			COUNT(DISTINCT CASE WHEN ISNULL(Tbl_MPFeeder.HavaeiLength,0) <= ISNULL(Tbl_MPFeeder.ZeminiLength,0) THEN Tbl_MPFeeder.MPFeederId END) AS BazdidZaminiCount, " &
            "			SUM(CASE WHEN ISNULL(BTblBazdidResult.BazdidZaminiLen,0) > 0 THEN BTblBazdidResult.BazdidZaminiLen ELSE ISNULL(BTblBazdidResult.FromToLengthZamini, Tbl_MPFeeder.ZeminiLength) END) AS BazdidZaminiLen, " &
            "			SUM(CASE WHEN ISNULL(BTblBazdidResult.BazdidHavayiLen,0) > 0 THEN BTblBazdidResult.BazdidHavayiLen ELSE ISNULL(BTblBazdidResult.FromToLengthHavayi, Tbl_MPFeeder.HavaeiLength) END) AS BazdidHavayiLen " &
            "		FROM  " &
            "			BTblBazdidResult " &
            "			INNER JOIN Tbl_MPFeeder ON BTblBazdidResult.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "      		LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
            "			INNER JOIN " &
            "			( " &
            "				SELECT  " &
            "					BTblBazdidResult.BazdidResultId, " &
            "					MIN(BTblBazdidResultAddress.StartDatePersian) AS StartDate, " &
            "					MAX(BTblBazdidResultAddress.StartDatePersian) AS EndDate " &
            "				FROM  " &
            "					BTblBazdidResultAddress " &
            "					INNER JOIN BTblBazdidResult ON BTblBazdidResultAddress.BazdidResultId = BTblBazdidResult.BazdidResultId " &
            "					INNER JOIN Tbl_MPFeeder ON BTblBazdidResult.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "           		LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
            "					" & mJoinSpecialitySql &
            "				WHERE  " &
            "					(BTblBazdidResult.BazdidTypeId = 1) " &
            "					AND (BTblBazdidResult.BazdidStateId IN (2,3)) " &
                                mWhereDT &
            "				GROUP BY  " &
            "					BTblBazdidResult.BazdidResultId " &
            "			) AS Tbl_StartEndDate ON BTblBazdidResult.BazdidResultId = Tbl_StartEndDate.BazdidResultId " &
            "		WHERE  " &
            "			(BTblBazdidResult.BazdidTypeId = 1) " &
            "			AND (BTblBazdidResult.BazdidStateId IN (2,3)) " & mWhere &
            "		GROUP BY  " &
            "			BTblBazdidResult.AreaId " &
            "		) AS Tbl_Bazdid ON Tbl_Area.AreaId = Tbl_Bazdid.AreaId " &
            "		INNER JOIN Tbl_MPFeeder ON Tbl_Area.AreaId = Tbl_MPFeeder.AreaId "

        BindingTable(lSQL, mCnn, lDS, "Report_3_1_1", , True)
        If lDS.Tables.Contains("Report_3_1_1") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_3_1_1.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_3_1_1.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_3_1_2()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_3_1_2 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mOwnershipId & "," & mIsActive & ",'" & mBazdidSpeciality & "'"
        Dim lDS As New DataSet

        lSQL =
            "SELECT DISTINCT " &
            "	Tbl_Area.Area, " &
            "	Tbl_Area.AreaId, " &
            "	Tbl_MPFeederL.HavayiCount, " &
            "	Tbl_MPFeederL.HavayiLen, " &
            "	Tbl_MPFeederL.ZaminiCount, " &
            "	Tbl_MPFeederL.ZaminiLen, " &
            "	Tbl_Bazdid.StartDate, " &
            "	Tbl_Bazdid.EndDate, " &
            "	Tbl_Bazdid.BazdidHavayiCount, " &
            "	Tbl_Bazdid.BazdidZaminiCount, " &
            "	Tbl_Bazdid.BazdidHavayiLen, " &
            "	Tbl_Bazdid.BazdidZaminiLen " &
            "FROM  " &
            "	Tbl_Area " &
            "	INNER JOIN Tbl_MPFeeder ON Tbl_Area.AreaId = Tbl_MPFeeder.AreaId " &
            "	INNER JOIN " &
            "	( " &
             "SELECT " &
            "	Tbl_MPFeeder.AreaId, " &
            "	COUNT(CASE WHEN ISNULL(BTblBazdidResult.FromToLengthHavayi, ISNULL(Tbl_MPFeeder.HavaeiLength,0)) > ISNULL(BTblBazdidResult.FromToLengthZamini, ISNULL(Tbl_MPFeeder.ZeminiLength,0)) THEN BTblBazdidResult.BazdidResultId END) AS HavayiCount, " &
            "	COUNT(CASE WHEN ISNULL(BTblBazdidResult.FromToLengthHavayi, ISNULL(Tbl_MPFeeder.HavaeiLength,0)) <= ISNULL(BTblBazdidResult.FromToLengthZamini, ISNULL(Tbl_MPFeeder.ZeminiLength,0)) THEN BTblBazdidResult.BazdidResultId END) AS ZaminiCount, " &
            "	ISNULL(SUM(CASE WHEN ISNULL(BTblBazdidResult.FromToLengthHavayi,0) > 0 THEN BTblBazdidResult.FromToLengthHavayi ELSE  Tbl_MPFeeder.HavaeiLength END) ,0) AS HavayiLen, " &
            "	ISNULL(SUM(CASE WHEN ISNULL(BTblBazdidResult.FromToLengthZamini,0) > 0 THEN BTblBazdidResult.FromToLengthZamini ELSE  Tbl_MPFeeder.ZeminiLength END) ,0) AS ZaminiLen " &
            "FROM  " &
            "	BTblBazdidResult " &
            "	INNER JOIN Tbl_MPFeeder ON BTblBazdidResult.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "   LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
            mJoinSpecialitySql &
            "WHERE " &
            "	 BTblBazdidResult.BazdidTypeId = 1 " &
            mWhere &
            "GROUP BY Tbl_MPFeeder.AreaId " &
            ") AS Tbl_MPFeederL ON Tbl_MPFeeder.AreaId = Tbl_MPFeederL.AreaId " &
            "INNER JOIN " &
            "( " &
            "SELECT  " &
            "	BTblBazdidResult.AreaId, " &
            "	MIN(Tbl_StartEndDate.StartDate) AS StartDate, " &
            "	MAX(Tbl_StartEndDate.EndDate) AS EndDate, " &
            "	COUNT(CASE WHEN ISNULL(BTblBazdidResult.FromToLengthHavayi, ISNULL(Tbl_MPFeeder.HavaeiLength,0)) > ISNULL(BTblBazdidResult.FromToLengthZamini, ISNULL(Tbl_MPFeeder.ZeminiLength,0)) THEN BTblBazdidResult.BazdidResultId END) AS BazdidHavayiCount, " &
            "	COUNT(CASE WHEN ISNULL(BTblBazdidResult.FromToLengthHavayi, ISNULL(Tbl_MPFeeder.HavaeiLength,0)) <= ISNULL(BTblBazdidResult.FromToLengthZamini, ISNULL(Tbl_MPFeeder.ZeminiLength,0)) THEN BTblBazdidResult.BazdidResultId END) AS BazdidZaminiCount, " &
            "	SUM(CASE WHEN ISNULL(BTblBazdidResult.BazdidZaminiLen,0) > 0 THEN BTblBazdidResult.BazdidZaminiLen ELSE ISNULL(BTblBazdidResult.FromToLengthZamini, Tbl_MPFeeder.ZeminiLength) END) AS BazdidZaminiLen, " &
            "	SUM(CASE WHEN ISNULL(BTblBazdidResult.BazdidHavayiLen,0) > 0 THEN BTblBazdidResult.BazdidHavayiLen ELSE ISNULL(BTblBazdidResult.FromToLengthHavayi, Tbl_MPFeeder.HavaeiLength) END) AS BazdidHavayiLen " &
            "FROM  " &
            "	BTblBazdidResult " &
            "	INNER JOIN Tbl_MPFeeder ON BTblBazdidResult.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "   LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
            mJoinSpecialitySql &
            "	INNER JOIN  " &
            "	( " &
            "	SELECT  " &
            "		BTblBazdidResult.BazdidResultId, " &
            "		MIN(BTblBazdidResultAddress.StartDatePersian) AS StartDate, " &
            "		MAX(BTblBazdidResultAddress.StartDatePersian) AS EndDate " &
            "	FROM  " &
            "		BTblBazdidResult " &
            "		INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
            "		INNER JOIN Tbl_MPFeeder ON BTblBazdidResult.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "       LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
            mJoinSpecialitySql &
            "	WHERE " &
            "		BTblBazdidResult.BazdidStateId IN (2,3) " &
            "		AND BTblBazdidResult.BazdidTypeId = 1 " &
            mWhereDT &
            "	GROUP BY  " &
            "		BTblBazdidResult.BazdidResultId " &
            "	) AS Tbl_StartEndDate ON BTblBazdidResult.BazdidResultId = Tbl_StartEndDate.BazdidResultId " &
            "WHERE " &
            "	BTblBazdidResult.BazdidStateId IN (2,3) " &
            "	AND BTblBazdidResult.BazdidTypeId = 1 " &
            mWhere &
            "GROUP BY  " &
            "	BTblBazdidResult.AreaId " &
            " " &
            "	) AS Tbl_Bazdid ON Tbl_MPFeeder.AreaId = Tbl_Bazdid.AreaId "


        BindingTable(lSQL, mCnn, lDS, "Report_3_1_2", , True)
        If lDS.Tables.Contains("Report_3_1_2") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_3_1_2.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_3_1_2.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_3_1_3()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_3_1_3 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mOwnershipId & "," & mIsActive & "," & mMinCheckListCount & ",'" & mBazdidSpeciality & "'"
        Dim lDS As New DataSet

        lSQL =
        "	SELECT  " &
        "		Tbl_Area.AreaId, " &
        "		Tbl_Area.Area, " &
        "		MIN(view_StartEndDate.StartDate) AS StartDate, " &
        "		MAX(view_StartEndDate.EndDate) AS EndDate, " &
        "		ISNULL(BTbl_BazdidCheckListSubGroup.BazdidCheckListSubGroupId,111222) AS BazdidCheckListSubGroupId, " &
        "		ISNULL(BTbl_BazdidCheckListSubGroup.BazdidCheckListSubGroupName,'ساير') AS BazdidCheckListSubGroupName, " &
        "		SUM(CASE WHEN BTblBazdidResultCheckList.DefectionCount Is Null OR BTblBazdidResultCheckList.DefectionCount = 0 THEN 1 ELSE BTblBazdidResultCheckList.DefectionCount END) AS cnt, " &
        "		BTbl_BazdidCheckListGroup.IsHavayi " &
        "	FROM  " &
        "		BTblBazdidResult " &
        "		INNER JOIN Tbl_MPFeeder ON BTblBazdidResult.MPFeederId = Tbl_MPFeeder.MPFeederId " &
        "		INNER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId " &
        "		INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
        "		INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
        "		LEFT OUTER JOIN BTbl_BazdidCheckList ON BTblBazdidResultCheckList.BazdidCheckListId = BTbl_BazdidCheckList.BazdidCheckListId " &
        "		LEFT OUTER JOIN BTbl_BazdidCheckListSubGroup ON BTbl_BazdidCheckList.BazdidCheckListSubGroupId = BTbl_BazdidCheckListSubGroup.BazdidCheckListSubGroupId " &
        "		LEFT JOIN BTbl_BazdidCheckListGroup ON BTbl_BazdidCheckList.BazdidCheckListGroupId = BTbl_BazdidCheckListGroup.BazdidCheckListGroupId " &
        "       LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                mJoinSpecialitySql &
        "		INNER JOIN ( " &
        "			SELECT  " &
        "				BTblBazdidResultAddress.BazdidResultAddressId, " &
        "				MIN(BTblBazdidResultAddress.StartDatePersian) AS StartDate, " &
        "				MAX(BTblBazdidResultAddress.StartDatePersian) AS EndDate " &
        "			FROM  " &
        "				BTblBazdidResult " &
        "				INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
        "				INNER JOIN Tbl_MPFeeder ON BTblBazdidResult.MPFeederId = Tbl_MPFeeder.MPFeederId " &
        "               LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                        mJoinSpecialitySql &
        "			WHERE  " &
        "				BTblBazdidResult.BazdidStateId IN (2,3) " &
        "				AND BTblBazdidResult.BazdidTypeId = 1 " & mWhereDT &
        "			GROUP BY  " &
        "				BTblBazdidResultAddress.BazdidResultAddressId " &
        "			) AS view_StartEndDate ON BTblBazdidResultAddress.BazdidResultAddressId = view_StartEndDate.BazdidResultAddressId " &
        "	WHERE  " &
        "		BTblBazdidResult.BazdidStateId IN (2,3) " &
        "		AND BTblBazdidResultCheckList.Priority > 0 " &
        "		AND BTblBazdidResult.BazdidTypeId = 1 " & mWhere &
        "	GROUP BY  " &
        "		Tbl_Area.AreaId, " &
        "		ISNULL(BTbl_BazdidCheckListSubGroup.BazdidCheckListSubGroupId,111222), " &
        "		ISNULL(BTbl_BazdidCheckListSubGroup.BazdidCheckListSubGroupName,'ساير'), " &
        "		Tbl_Area.Area, " &
        "		BTbl_BazdidCheckListGroup.IsHavayi " &
            mHaving &
        "	ORDER BY  " &
        "		Tbl_Area.Area "

        BindingTable(lSQL, mCnn, lDS, "Report_3_1_3", , True)
        If lDS.Tables.Contains("Report_3_1_3") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_3_1_3.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_3_1_3.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2, True)
        End If
    End Sub
    Private Sub MakeReport_3_1_4()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_3_1_4 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mOwnershipId & "," & mIsActive & ",'" & mBazdidSpeciality & "'"

        lSQL =
            "	SELECT  " &
            "		Tbl_Area.AreaId, " &
            "		Tbl_Area.Area, " &
            "		COUNT(ISNULL(BTblBazdidResultSubCheckList.BazdidResultSubCheckListId,BTblBazdidResultCheckList.SubCheckListId)) AS cnt, " &
            "		BTblBazdidResultCheckList.Priority, " &
            "		MIN(viewStartEndDate.StartDate) AS StartDate, " &
            "		MAX(viewStartEndDate.EndDate) AS EndDate, " &
            "		viewStartEndDate.IsHavayi " &
            "	FROM  " &
            "		Tbl_MPFeeder " &
            "		INNER JOIN BTblBazdidResult ON Tbl_MPFeeder.MPFeederId = BTblBazdidResult.MPFeederId " &
            "		INNER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId " &
            "		INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
            "		INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
            "		LEFT OUTER JOIN BTblBazdidResultSubCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblBazdidResultSubCheckList.BazdidResultCheckListId " &
            "       LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                    mJoinSpecialitySql &
            "		INNER JOIN ( " &
            "			SELECT  " &
            "				BTblBazdidResult.BazdidResultId, " &
            "				MIN(BTblBazdidResultAddress.StartDatePersian) AS StartDate, " &
            "				MAX(BTblBazdidResultAddress.StartDatePersian) AS EndDate, " &
            "				CASE WHEN (Tbl_MPFeeder.HavaeiLength > Tbl_MPFeeder.ZeminiLength) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS IsHavayi " &
            "			FROM  " &
            "				BTblBazdidResult " &
            "				INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
            "				INNER JOIN Tbl_MPFeeder ON BTblBazdidResult.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "               LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                            mJoinSpecialitySql &
            "			WHERE  " &
            "				BTblBazdidResult.BazdidStateId IN (2,3) " &
            "				AND BTblBazdidResult.BazdidTypeId = 1 " & mWhereDT &
            "			GROUP BY  " &
            "				BTblBazdidResult.BazdidResultId ,  " &
            "				Tbl_MPFeeder.HavaeiLength , " &
            "				Tbl_MPFeeder.ZeminiLength " &
            "			) AS viewStartEndDate ON BTblBazdidResult.BazdidResultId = viewStartEndDate.BazdidResultId " &
            "	WHERE " &
            "		BTblBazdidResult.BazdidStateId IN (2,3) " &
            "		AND BTblBazdidResult.BazdidTypeId = 1 " & mWhere &
            "	GROUP BY  " &
            "		Tbl_Area.AreaId, " &
            "		Tbl_Area.Area, " &
            "		BTblBazdidResultCheckList.Priority, " &
            "		viewStartEndDate.IsHavayi"
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_3_1_4", , True)
        If lDS.Tables.Contains("Report_3_1_4") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_3_1_4.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_3_1_4.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2, True)
        End If
    End Sub
    Private Sub MakeReport_3_1_5()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_3_1_5 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mOwnershipId & "," & mIsActive & " ,'" & mParts & "','" & mBazdidSpeciality & "'"

        lSQL =
            "		SELECT  " &
            "			Tbl_Area.AreaId, " &
            "			Tbl_Area.Area, " &
            "			MIN(Tbl_StartEndDate.StartDate) AS StartDate, " &
            "			MAX(Tbl_StartEndDate.EndDate) AS EndDate, " &
            "			COUNT(BTblBazdidResult.BazdidResultId) AS cntBazdid, " &
            "			SUM(ISNULL(BTblBazdidResult.FromToLengthHavayi,Tbl_MPFeeder.HavaeiLength)) AS BazdidHavayi, " &
            "			SUM(ISNULL(BTblBazdidResult.FromToLengthZamini,Tbl_MPFeeder.ZeminiLength)) AS BazdidZamini, " &
            "			BTbl_ServicePart.ServicePartCode,  " &
            "			BTbl_ServicePart.ServicePartName,  " &
            "			Tbl_PartUnit.PartUnit, " &
            "			SUM(BTblBazdidCheckListPart.Quantity) cntCheckList, " &
            "			BTbl_ServicePart.PriceOne,  " &
            "			BTbl_ServicePart.ServicePrice,  " &
            "			SUM(BTbl_ServicePart.PriceOne * BTblBazdidCheckListPart.Quantity) AS PriceCntCheckList,  " &
            "			SUM(BTbl_ServicePart.ServicePrice * BTblBazdidCheckListPart.Quantity) AS PriceCntPrice " &
            "		FROM  " &
            "			BTblBazdidResultCheckList " &
            "			INNER JOIN BTblBazdidResultAddress ON BTblBazdidResultCheckList.BazdidResultAddressId = BTblBazdidResultAddress.BazdidResultAddressId " &
            "			INNER JOIN BTblBazdidResult ON BTblBazdidResultAddress.BazdidResultId = BTblBazdidResult.BazdidResultId " &
            "			INNER JOIN Tbl_MPFeeder ON BTblBazdidResult.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "			INNER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId " &
            "			INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId			 " &
            "			LEFT OUTER JOIN BTblBazdidCheckListPart ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblBazdidCheckListPart.BazdidResultCheckListId " &
            "			LEFT OUTER JOIN BTbl_ServicePart ON BTblBazdidCheckListPart.ServicePartId = BTbl_ServicePart.ServicePartId " &
            "			LEFT OUTER JOIN Tbl_PartUnit ON BTbl_ServicePart.PartUnitId = Tbl_PartUnit.PartUnitId " &
            "           LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                        mJoinSpecialitySql &
            "			INNER JOIN  " &
            "			( " &
            "				SELECT " &
            "					BTblBazdidResult.BazdidResultId, " &
            "					MIN(BTblBazdidResultAddress.StartDatePersian) AS StartDate, " &
            "					MAX(BTblBazdidResultAddress.StartDatePersian) AS EndDate " &
            "				FROM  " &
            "					BTblBazdidResult " &
            "					INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
            "					INNER JOIN Tbl_MPFeeder ON BTblBazdidResult.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "                   LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                                mJoinSpecialitySql &
            "				WHERE " &
            "					BTblBazdidResult.BazdidStateId IN (2,3) " &
            "					AND BTblBazdidResult.BazdidTypeId = 1 " &
                                mWhereDT &
            "				GROUP BY " &
            "					BTblBazdidResult.BazdidResultId " &
            "			) AS Tbl_StartEndDate ON BTblBazdidResult.BazdidResultId = Tbl_StartEndDate.BazdidResultId " &
            "			 " &
            "		WHERE  " &
            "			BTblBazdidResult.BazdidStateId IN (2,3) " &
            "			AND BTblBazdidResult.BazdidTypeId = 1 " &
            "			AND BTblBazdidResultCheckList.Priority > 0 " &
            "			AND  " &
            "			( " &
            "				NOT BTbl_ServicePart.ServicePartCode IS NULL " &
            "				OR NOT BTbl_ServicePart.ServicePartName IS NULL " &
            "			) " &
                        mWhere &
            "		GROUP BY " &
            "			Tbl_Area.AreaId, " &
            "			Tbl_Area.Area, " &
            "			BTbl_ServicePart.ServicePartCode,  " &
            "			BTbl_ServicePart.ServicePartName,  " &
            "			Tbl_PartUnit.PartUnit, " &
            "			BTbl_ServicePart.PriceOne,  " &
            "			BTbl_ServicePart.ServicePrice "

        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_3_1_5", , True)
        If lDS.Tables.Contains("Report_3_1_5") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_3_1_5.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_3_1_5.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2, True)
        End If
    End Sub
    Private Sub MakeReport_3_2_1()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_3_2_1 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mLPPostId & "," & mOwnershipId & "," & mIsActive & ",'" & mBazdidSpeciality & "'"
        lSQL =
            " SELECT DISTINCT " &
            "	Tbl_Area.AreaId, " &
            "	Tbl_Area.Area, " &
            "	HavayiCount, " &
            "	ZaminiCount, " &
            "	PrivateCount, " &
            "	HavayiLen, " &
            "	ZaminiLen, " &
            "	PrivateLen, " &
            "	BazdidHavayiCount, " &
            "	BazdidZaminiCount, " &
            "	BazdidPrivateCount, " &
            "	StartDatePersian, " &
            "	EndDatePersian " &
            "FROM " &
            "	Tbl_Area " &
            "	INNER JOIN Tbl_MPFeeder ON Tbl_Area.AreaId = Tbl_MPFeeder.AreaId " &
            "	INNER JOIN " &
            "	( " &
          "	SELECT      " &
            "		BTblBazdidResult.AreaId, " &
            "		COUNT(DISTINCT CASE WHEN ISNULL(Tbl_MPFeeder.HavaeiLength,0) > ISNULL(Tbl_MPFeeder.ZeminiLength,0)  THEN Tbl_MPFeeder.MPFeederId END) AS HavayiCount, " &
            "		COUNT(DISTINCT CASE WHEN ISNULL(Tbl_MPFeeder.HavaeiLength,0) <= ISNULL(Tbl_MPFeeder.ZeminiLength,0) THEN Tbl_MPFeeder.MPFeederId END) AS ZaminiCount, " &
            "		COUNT(DISTINCT CASE WHEN Tbl_MPFeeder.OwnershipId = 1 THEN Tbl_MPFeeder.MPFeederId END) AS PrivateCount, " &
            "	    SUM(CASE WHEN ISNULL(BTblBazdidResult.FromToLengthZamini,0) > 0 THEN BTblBazdidResult.FromToLengthZamini  ELSE  Tbl_MPFeeder.ZeminiLength END) AS ZaminiLen, " &
            "		SUM(CASE WHEN ISNULL(BTblBazdidResult.FromToLengthHavayi,0) > 0 THEN BTblBazdidResult.FromToLengthHavayi  ELSE  Tbl_MPFeeder.HavaeiLength END) AS HavayiLen, " &
            "		ISNULL(SUM(CASE WHEN Tbl_MPFeeder.OwnershipId = 1 THEN ISNULL(Tbl_MPFeeder.HavaeiLength,0) + ISNULL(Tbl_MPFeeder.ZeminiLength,0) END),0) AS PrivateLen " &
            "	FROM          " &
            "		BTblBazdidResult " &
            "		INNER JOIN Tbl_MPFeeder ON BTblBazdidResult.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "       LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                    mJoinSpecialitySql &
            "	WHERE      " &
            "		(BTblBazdidResult.BazdidTypeId = 2) " &
             mWhere &
            "	GROUP BY " &
            "		BTblBazdidResult.AreaId " &
            "	) AS Tbl_MPFeederLen ON Tbl_MPFeeder.AreaId = Tbl_MPFeederLen.AreaId " &
            "	INNER JOIN " &
            "	( " &
            "	SELECT      " &
            "		BTblBazdidResult.AreaId, " &
            "		COUNT(DISTINCT CASE WHEN ISNULL(Tbl_MPFeeder.HavaeiLength,0) > ISNULL(Tbl_MPFeeder.ZeminiLength,0) THEN Tbl_MPFeeder.MPFeederId END) AS BazdidHavayiCount, " &
            "		COUNT(DISTINCT CASE WHEN ISNULL(Tbl_MPFeeder.HavaeiLength,0) <= ISNULL(Tbl_MPFeeder.ZeminiLength,0) THEN Tbl_MPFeeder.MPFeederId END) AS BazdidZaminiCount, " &
            "		COUNT(DISTINCT CASE WHEN Tbl_MPFeeder.OwnershipId = 1 THEN Tbl_MPFeeder.MPFeederId END) AS BazdidPrivateCount, " &
            "		MIN(Tbl_StartEndDate.StartDatePersian) AS StartDatePersian, " &
            "		MAX(Tbl_StartEndDate.EndDatePersian) AS EndDatePersian " &
            "	FROM          " &
            "		BTblBazdidResult " &
            "		INNER JOIN Tbl_MPFeeder ON BTblBazdidResult.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "       LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                    mJoinSpecialitySql &
            "		INNER JOIN " &
            "			( " &
            "				SELECT      " &
            "					BTblBazdidResult.BazdidResultId,  " &
            "					MIN(BTblBazdidResultAddress.StartDatePersian) AS StartDatePersian,  " &
            "					MAX(BTblBazdidResultAddress.StartDatePersian) AS EndDatePersian  " &
            "				FROM          " &
            "					BTblBazdidResultAddress  " &
            "					INNER JOIN BTblBazdidResult ON BTblBazdidResultAddress.BazdidResultId = BTblBazdidResult.BazdidResultId  " &
            "					INNER JOIN Tbl_LPPost ON BTblBazdidResult.LPPostId = Tbl_LPPost.LPPostId  " &
            "					INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "	                INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "                   LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                                mJoinSpecialitySql &
            "				 WHERE  " &
            "					BTblBazdidResult.BazdidStateId IN (2,3) " &
            "					AND (BTblBazdidResult.BazdidTypeId = 2)  " &
                                mWhereDT &
            "				GROUP BY  " &
            "					BTblBazdidResult.BazdidResultId  " &
            "			) AS Tbl_StartEndDate ON BTblBazdidResult.BazdidResultId = Tbl_StartEndDate.BazdidResultId " &
            "	WHERE      " &
            "		(BTblBazdidResult.BazdidTypeId = 2) " &
            "		AND BTblBazdidResult.BazdidStateId IN (2,3) " &
                    mWhere &
            "	GROUP BY " &
            "		BTblBazdidResult.AreaId " &
            " ) AS Tbl_Bazdid ON Tbl_MPFeeder.AreaId = Tbl_Bazdid.AreaId "

        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_3_2_1", , True)
        If lDS.Tables.Contains("Report_3_2_1") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_3_2_1.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_3_2_1.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2, True)
        End If
    End Sub
    Private Sub MakeReport_3_2_2()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_3_2_2 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mLPPostId & "," & mOwnershipId & "," & mIsActive & ",'" & mBazdidSpeciality & "'"
        lSQL =
            "SELECT DISTINCT " &
            "	Tbl_Area.AreaId, " &
            "	Tbl_Area.Area, " &
            "	HavayiPublicCount, " &
            "	HavayiPrivateCount, " &
            "	ZaminiPublicCount, " &
            "	ZaminiPrivateCount, " &
            "	OtherCount, " &
            "	AllCount, " &
            "	StartDate, " &
            "	EndDate, " &
            "	BazdidHavayiPublicCount, " &
            "	BazdidHavayiPrivateCount, " &
            "	BazdidZaminiPublicCount, " &
            "	BazdidZaminiPrivateCount, " &
            "	BazdidOtherCount, " &
            "	BazdidAllCount " &
            "FROM " &
            "	Tbl_LPPost " &
            "	INNER JOIN Tbl_Area ON Tbl_LPPost.AreaId = Tbl_Area.AreaId " &
            "	INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "   INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "	INNER JOIN " &
            "	( " &
            "	SELECT " &
            "		Tbl_LPPost.AreaId, " &
            "		COUNT(CASE WHEN Tbl_LPPost.IsHavayi = 1 AND Tbl_LPPost.OwnershipId = 2 THEN Tbl_LPPost.LPPostId END) AS HavayiPublicCount, " &
            "		COUNT(CASE WHEN Tbl_LPPost.IsHavayi = 1 AND Tbl_LPPost.OwnershipId = 1 THEN Tbl_LPPost.LPPostId END) AS HavayiPrivateCount, " &
            "		COUNT(CASE WHEN Tbl_LPPost.IsHavayi = 0 AND Tbl_LPPost.OwnershipId = 2 THEN Tbl_LPPost.LPPostId END) AS ZaminiPublicCount, " &
            "		COUNT(CASE WHEN Tbl_LPPost.IsHavayi = 0 AND Tbl_LPPost.OwnershipId = 1 THEN Tbl_LPPost.LPPostId END) AS ZaminiPrivateCount, " &
            "		COUNT(CASE WHEN Tbl_LPPost.OwnershipId = 3 OR Tbl_LPPost.OwnershipId IS NULL THEN Tbl_LPPost.LPPostId END) AS OtherCount, " &
            "		COUNT(Tbl_LPPost.LPPostId) AS AllCount " &
            "	FROM " &
            "		Tbl_LPPost " &
            "		INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "       INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
                    mWherePost &
            "	GROUP BY " &
            "		Tbl_LPPost.AreaId " &
            "	) AS Tbl_LPPostLen ON Tbl_LPPost.AreaId = Tbl_LPPostLen.AreaId " &
            "	INNER JOIN " &
            "	( " &
            "	SELECT " &
            "		BTblBazdidResult.AreaId, " &
            "		MIN(StartDate) AS StartDate, " &
            "		MAX(EndDate) AS EndDate, " &
            "		COUNT(DISTINCT CASE WHEN Tbl_LPPost.IsHavayi = 1 AND Tbl_LPPost.OwnershipId = 2 THEN Tbl_LPPost.LPPostId END) AS BazdidHavayiPublicCount, " &
            "		COUNT(DISTINCT CASE WHEN Tbl_LPPost.IsHavayi = 1 AND Tbl_LPPost.OwnershipId = 1 THEN Tbl_LPPost.LPPostId END) AS BazdidHavayiPrivateCount, " &
            "		COUNT(DISTINCT CASE WHEN Tbl_LPPost.IsHavayi = 0 AND Tbl_LPPost.OwnershipId = 2 THEN Tbl_LPPost.LPPostId END) AS BazdidZaminiPublicCount, " &
            "		COUNT(DISTINCT CASE WHEN Tbl_LPPost.IsHavayi = 0 AND Tbl_LPPost.OwnershipId = 1 THEN Tbl_LPPost.LPPostId END) AS BazdidZaminiPrivateCount, " &
            "		COUNT(DISTINCT CASE WHEN Tbl_LPPost.OwnershipId = 3 OR Tbl_LPPost.OwnershipId IS NULL THEN Tbl_LPPost.LPPostId END) AS BazdidOtherCount, " &
            "		COUNT(DISTINCT Tbl_LPPost.LPPostId) AS BazdidAllCount " &
            "	FROM  " &
            "		BTblBazdidResult  " &
            "		INNER JOIN Tbl_LPPost ON BTblBazdidResult.LPPostId = Tbl_LPPost.LPPostId " &
            "		INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "       INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "       LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                    mJoinSpecialitySql &
            "		INNER JOIN " &
            "		( " &
            "		SELECT      " &
            "			BTblBazdidResult.BazdidResultId,  " &
            "			MIN(BTblBazdidResultAddress.StartDatePersian) AS StartDate,  " &
            "			MAX(BTblBazdidResultAddress.StartDatePersian) AS EndDate " &
            "		FROM " &
            "			BTblBazdidResult  " &
            "			INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId  " &
            "			INNER JOIN Tbl_LPPost ON BTblBazdidResult.LPPostId = Tbl_LPPost.LPPostId  " &
            "			INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "           INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "           LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                        mJoinSpecialitySql &
            "		WHERE      " &
            "			(BTblBazdidResult.BazdidTypeId = 2)  " &
            "			AND (BTblBazdidResult.BazdidStateId IN (2, 3)) " &
                        mWhereDT &
            "		GROUP BY  " &
            "			BTblBazdidResult.BazdidResultId " &
            "		) AS Tbl_StartEndDate ON BTblBazdidResult.BazdidResultId = Tbl_StartEndDate.BazdidResultId " &
            "	WHERE      " &
            "		(BTblBazdidResult.BazdidTypeId = 2)  " &
            "		AND (BTblBazdidResult.BazdidStateId IN (2, 3)) " &
                    mWhere &
            "	GROUP BY  " &
            "		BTblBazdidResult.AreaId " &
            "	) AS Tbl_Bazdid ON Tbl_LPPost.AreaId = Tbl_Bazdid.AreaId  "

        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_3_2_2", , True)
        If lDS.Tables.Contains("Report_3_2_2") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_3_2_2.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_3_2_2.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2, True)
        End If
    End Sub
    Private Sub MakeReport_3_2_3()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_3_2_3 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mLPPostId & "," & mOwnershipId & "," & mIsActive & "," & mMinCheckListCount & ",'" & mBazdidSpeciality & "'"
        lSQL =
        "	SELECT  " &
        "		Tbl_Area.AreaId, " &
        "		Tbl_Area.Area, " &
        "		MIN(StartDate) AS StartDate, " &
        "		MAX(EndDate) AS EndDate, " &
        "		SUM(CASE WHEN BTblBazdidResultCheckList.DefectionCount Is Null OR BTblBazdidResultCheckList.DefectionCount = 0 THEN 1 ELSE BTblBazdidResultCheckList.DefectionCount END) AS CheckListCount, " &
        "		BTbl_LPPostDetail.LPPostDetailName, " &
        "		BTbl_BazdidCheckListGroup.BazdidCheckListGroupId, " &
        "		ISNULL(BTbl_BazdidCheckListSubGroup.BazdidCheckListSubGroupId,111222) AS BazdidCheckListSubGroupId, " &
        "		ISNULL(BTbl_BazdidCheckListSubGroup.BazdidCheckListSubGroupName,'ساير') AS BazdidCheckListSubGroupName " &
        "	FROM " &
        "		Tbl_LPPost " &
        "		INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
        "       INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
        "		INNER JOIN BTblBazdidResult ON Tbl_LPPost.LPPostId = BTblBazdidResult.LPPostId " &
        "		INNER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId " &
        "		INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
        "		INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
        "		INNER JOIN BTbl_BazdidCheckList ON BTblBazdidResultCheckList.BazdidCheckListId = BTbl_BazdidCheckList.BazdidCheckListId " &
        "		INNER JOIN BTbl_LPPostDetail ON BTbl_BazdidCheckList.LPPostDetailId = BTbl_LPPostDetail.LPPostDetailId " &
        "		LEFT OUTER JOIN BTbl_BazdidCheckListGroup ON BTbl_BazdidCheckList.BazdidCheckListGroupId = BTbl_BazdidCheckListGroup.BazdidCheckListGroupId " &
        "		LEFT OUTER JOIN BTbl_BazdidCheckListSubGroup ON BTbl_BazdidCheckList.BazdidCheckListSubGroupId = BTbl_BazdidCheckListSubGroup.BazdidCheckListSubGroupId " &
        "       LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                mJoinSpecialitySql &
        "		INNER JOIN " &
        "		( " &
        "		SELECT      " &
        "			BTblBazdidResultAddress.BazdidResultAddressId,  " &
        "			MIN(BTblBazdidResultAddress.StartDatePersian) AS StartDate,  " &
        "			MAX(BTblBazdidResultAddress.StartDatePersian) AS EndDate " &
        "		FROM          " &
        "			BTblBazdidResult  " &
        "			INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId  " &
        "			INNER JOIN Tbl_LPPost ON BTblBazdidResult.LPPostId = Tbl_LPPost.LPPostId  " &
        "			INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
        "           INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
        "           LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                    mJoinSpecialitySql &
        "		WHERE      " &
        "			(BTblBazdidResult.BazdidTypeId = 2)  " &
        "			AND (BTblBazdidResult.BazdidStateId IN (2, 3)) " &
                    mWhereDT &
        "		GROUP BY  " &
        "			BTblBazdidResultAddress.BazdidResultAddressId " &
        "		) AS Tbl_StartEndDate ON BTblBazdidResultAddress.BazdidResultAddressId = Tbl_StartEndDate.BazdidResultAddressId " &
        "	WHERE      " &
        "		(BTblBazdidResult.BazdidTypeId = 2)  " &
        "		AND (BTblBazdidResult.BazdidStateId IN (2, 3)) " &
        "		AND BTblBazdidResultCheckList.Priority > 0 " &
                mWhere &
        "	GROUP BY  " &
        "		Tbl_Area.AreaId, " &
        "		Tbl_Area.Area, " &
        "		BTbl_LPPostDetail.LPPostDetailName, " &
        "		BTbl_BazdidCheckListGroup.BazdidCheckListGroupId, " &
        "		ISNULL(BTbl_BazdidCheckListSubGroup.BazdidCheckListSubGroupId,111222), " &
        "		ISNULL(BTbl_BazdidCheckListSubGroup.BazdidCheckListSubGroupName,'ساير') " &
        mHaving
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_3_2_3", , True)
        If lDS.Tables.Contains("Report_3_2_3") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_3_2_3.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_3_2_3.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2, True)
        End If
    End Sub
    Private Sub MakeReport_3_2_4()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_3_2_4 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mLPPostId & "," & mOwnershipId & "," & mIsActive & ",'" & mBazdidSpeciality & "'"

        lSQL =
            "	SELECT  " &
            "		Tbl_Area.AreaId, " &
            "		Tbl_Area.Area, " &
            "		COUNT(ISNULL(BTblBazdidResultSubCheckList.BazdidResultSubCheckListId,BTblBazdidResultCheckList.SubCheckListId)) AS cnt, " &
            "		BTblBazdidResultCheckList.Priority, " &
            "		MIN(viewStartEndDate.StartDate) AS StartDate, " &
            "		MAX(viewStartEndDate.EndDate) AS EndDate, " &
            "		viewStartEndDate.IsHavayi " &
            "	FROM  " &
            "		Tbl_LPPost " &
            "		INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "       INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "		INNER JOIN BTblBazdidResult ON Tbl_LPPost.LPPostId = BTblBazdidResult.LPPostId " &
            "		INNER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId " &
            "		INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
            "		INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
            "       LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
            mJoinSpecialitySql &
            "		LEFT OUTER JOIN BTblBazdidResultSubCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblBazdidResultSubCheckList.BazdidResultCheckListId " &
            "		INNER JOIN ( " &
            "		SELECT  " &
            "			BTblBazdidResult.BazdidResultId, " &
            "			MIN(BTblBazdidResultAddress.StartDatePersian) AS StartDate, " &
            "			MAX(BTblBazdidResultAddress.StartDatePersian) AS EndDate, " &
            "			Tbl_LPPost.IsHavayi	 " &
            "		FROM  " &
            "			BTblBazdidResult " &
            "			INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
            "			INNER JOIN Tbl_LPPost ON BTblBazdidResult.LPPostId = Tbl_LPPost.LPPostId " &
            "			INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "           INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "           LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
            mJoinSpecialitySql &
            "		WHERE  " &
            "			BTblBazdidResult.BazdidStateId IN (2,3) " &
            "			AND BTblBazdidResult.BazdidTypeId = 2 " &
            mWhereDT &
            "		GROUP BY  " &
            "			BTblBazdidResult.BazdidResultId ,  " &
            "			Tbl_LPPost.IsHavayi " &
            "			) AS viewStartEndDate ON BTblBazdidResult.BazdidResultId = viewStartEndDate.BazdidResultId " &
            "	WHERE " &
            "		BTblBazdidResult.BazdidStateId IN (2,3) " &
            "		AND BTblBazdidResult.BazdidTypeId = 2  " &
            mWhere &
            "	GROUP BY  " &
            "		Tbl_Area.AreaId, " &
            "		Tbl_Area.Area, " &
            "		BTblBazdidResultCheckList.Priority, " &
            "		viewStartEndDate.IsHavayi "
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_3_2_4", , True)
        If lDS.Tables.Contains("Report_3_2_4") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_3_2_4.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_3_2_4.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2, True)
        End If
    End Sub
    Private Sub MakeReport_3_2_5()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_3_2_5 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mLPPostId & "," & mOwnershipId & "," & mIsActive & " ,'" & mParts & "','" & mBazdidSpeciality & "'"
        lSQL =
            "       Select " &
            "			Tbl_Area.AreaId, " &
            "			Tbl_Area.Area, " &
            "			MIN(Tbl_StartEndDate.StartDate) AS StartDate, " &
            "			MAX(Tbl_StartEndDate.EndDate) AS EndDate, " &
            "			COUNT(BTblBazdidResult.BazdidResultId) AS cntBazdid, " &
            "			SUM(Tbl_LPPost.PostCapacity) AS sumCapacity, " &
            "			BTbl_ServicePart.ServicePartCode,  " &
            "			BTbl_ServicePart.ServicePartName,  " &
            "			Tbl_PartUnit.PartUnit, " &
            "			SUM(BTblBazdidCheckListPart.Quantity) cntCheckList, " &
            "			BTbl_ServicePart.PriceOne,  " &
            "			BTbl_ServicePart.ServicePrice,  " &
            "			SUM(BTbl_ServicePart.PriceOne * BTblBazdidCheckListPart.Quantity) AS PriceCntCheckList,  " &
            "			SUM(BTbl_ServicePart.ServicePrice * BTblBazdidCheckListPart.Quantity) AS PriceCntPrice " &
            "		FROM  " &
            "			BTblBazdidResultCheckList " &
            "			INNER JOIN BTblBazdidResultAddress ON BTblBazdidResultCheckList.BazdidResultAddressId = BTblBazdidResultAddress.BazdidResultAddressId " &
            "			INNER JOIN BTblBazdidResult ON BTblBazdidResultAddress.BazdidResultId = BTblBazdidResult.BazdidResultId " &
            "			INNER JOIN Tbl_LPPost ON BTblBazdidResult.LPPostId = Tbl_LPPost.LPPostId " &
            "			INNER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId " &
            "			INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "           INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "           LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                        mJoinSpecialitySql &
            "			LEFT OUTER JOIN BTblBazdidCheckListPart ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblBazdidCheckListPart.BazdidResultCheckListId " &
            "			LEFT OUTER JOIN BTbl_ServicePart ON BTblBazdidCheckListPart.ServicePartId = BTbl_ServicePart.ServicePartId " &
            "			LEFT OUTER JOIN Tbl_PartUnit ON BTbl_ServicePart.PartUnitId = Tbl_PartUnit.PartUnitId " &
            "			INNER JOIN  " &
            "			( " &
            "				SELECT " &
            "					BTblBazdidResult.BazdidResultId, " &
            "					MIN(BTblBazdidResultAddress.StartDatePersian) AS StartDate, " &
            "					MAX(BTblBazdidResultAddress.StartDatePersian) AS EndDate " &
            "				FROM  " &
            "					BTblBazdidResult " &
            "					INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
            "					INNER JOIN Tbl_LPPost ON BTblBazdidResult.LPPostId = Tbl_LPPost.LPPostId " &
            "                   LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                                mJoinSpecialitySql &
            "				WHERE " &
            "					BTblBazdidResult.BazdidStateId IN (2,3) " &
            "					AND BTblBazdidResult.BazdidTypeId = 2 " &
                                mWhereDT &
            "				GROUP BY " &
            "					BTblBazdidResult.BazdidResultId " &
            "			) AS Tbl_StartEndDate ON BTblBazdidResult.BazdidResultId = Tbl_StartEndDate.BazdidResultId " &
            "			 " &
            "		WHERE  " &
            "			BTblBazdidResult.BazdidStateId IN (2,3) " &
            "			AND BTblBazdidResult.BazdidTypeId = 2 " &
            "			AND BTblBazdidResultCheckList.Priority > 0 " &
            "			AND  " &
            "			( " &
            "				NOT BTbl_ServicePart.ServicePartCode IS NULL " &
            "				OR NOT BTbl_ServicePart.ServicePartName IS NULL " &
            "			) " &
                        mWhere &
            "		GROUP BY " &
            "			Tbl_Area.AreaId, " &
            "			Tbl_Area.Area, " &
            "			BTbl_ServicePart.ServicePartCode,  " &
            "			BTbl_ServicePart.ServicePartName,  " &
            "			Tbl_PartUnit.PartUnit, " &
            "			BTbl_ServicePart.PriceOne,  " &
            "			BTbl_ServicePart.ServicePrice"

        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_3_2_5", , True)
        If lDS.Tables.Contains("Report_3_2_5") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_3_2_5.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_3_2_5.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2, True)
        End If
    End Sub
    Private Sub MakeReport_3_34_1()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_3_34_1 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mLPPostId & "," & mLPFeederId & "," & mOwnershipId & "," & mIsActive & "," & IIf(mIsLightReport, 1, 0) & ",'" & mBazdidSpeciality & "'"
        lSQL =
            "    Select " &
            "		Tbl_Area.Area, " &
            "		ISNULL(LPFHavayiCount,0) AS LPFHavayiCount, " &
            "		ISNULL(LPFZaminiCount,0) AS LPFZaminiCount, " &
            "		ISNULL(LPFHavayiLen,0) AS LPFHavayiLen, " &
            "		ISNULL(LPFZaminiLen,0) AS LPFZaminiLen, " &
            "		StartDatePersian, " &
            "		EndDatePersian, " &
            "		ISNULL(BazdidHavayiCount,0) AS BazdidHavayiCount, " &
            "		ISNULL(BazdidZaminiCount,0) AS BazdidZaminiCount, " &
            "		ISNULL(BazdidHavayiLen,0) AS BazdidHavayiLen, " &
            "		ISNULL(BazdidZaminiLen,0) AS BazdidZaminiLen " &
            "	FROM " &
            "		Tbl_Area " &
            "		INNER JOIN Tbl_LPFeeder ON Tbl_Area.AreaId = Tbl_LPFeeder.AreaId " &
            "		INNER JOIN Tbl_LPPost ON Tbl_LPFeeder.LPPostId = Tbl_LPPost.LPPostId " &
            "		INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "       INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "		INNER JOIN " &
            "		( " &
            "		SELECT  " &
            "			BTblBazdidResult.AreaId, " &
            "			COUNT(DISTINCT CASE WHEN ISNULL(Tbl_LPFeeder.HavaeiLength,0) > ISNULL(Tbl_LPFeeder.ZeminiLength,0) THEN Tbl_LPFeeder.LPFeederId END) AS LPFHavayiCount, " &
            "			COUNT(DISTINCT CASE WHEN ISNULL(Tbl_LPFeeder.HavaeiLength,0) <= ISNULL(Tbl_LPFeeder.ZeminiLength,0) THEN Tbl_LPFeeder.LPFeederId END) AS LPFZaminiCount, " &
            "			SUM(CASE WHEN ISNULL(BTblBazdidResult.FromToLengthZamini,0) > 0 THEN BTblBazdidResult.FromToLengthZamini ELSE  Tbl_LPFeeder.ZeminiLength END) AS LPFZaminiLen, " &
            "			SUM(CASE WHEN ISNULL(BTblBazdidResult.FromToLengthHavayi,0) > 0 THEN BTblBazdidResult.FromToLengthHavayi ELSE  Tbl_LPFeeder.HavaeiLength END) AS LPFHavayiLen " &
            "		FROM  " &
            "			BTblBazdidResult " &
            "           LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                        mJoinSpecialitySql &
            "			INNER JOIN Tbl_LPFeeder ON BTblBazdidResult.LPFeederId = Tbl_LPFeeder.LPFeederId " &
            "			INNER JOIN Tbl_LPPost ON Tbl_LPFeeder.LPPostId = Tbl_LPPost.LPPostId " &
            "			INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "           INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "		WHERE  " &
            "			(BTblBazdidResult.BazdidTypeId = 3) " &
            "			" & mWhere &
            "			AND Tbl_LPFeeder.IsLightFeeder = " & mIsLightFeeder &
            "		GROUP BY  " &
            "			BTblBazdidResult.AreaId " &
                       "		) AS Tbl_LPFeederLen ON Tbl_Area.AreaId = Tbl_LPFeederLen.AreaId " &
            "		INNER JOIN " &
            "		( " &
            "		SELECT  " &
            "			BTblBazdidResult.AreaId, " &
            "			MIN(Tbl_StartEndDate.StartDate) StartDatePersian, " &
            "			MAX(Tbl_StartEndDate.EndDate) EndDatePersian, " &
            "			COUNT(DISTINCT CASE WHEN ISNULL(Tbl_LPFeeder.HavaeiLength,0) > ISNULL(Tbl_LPFeeder.ZeminiLength,0) THEN Tbl_LPFeeder.LPFeederId END) AS BazdidHavayiCount, " &
            "			COUNT(DISTINCT CASE WHEN ISNULL(Tbl_LPFeeder.HavaeiLength,0) <= ISNULL(Tbl_LPFeeder.ZeminiLength,0) THEN Tbl_LPFeeder.LPFeederId END) AS BazdidZaminiCount, " &
            "			SUM(CASE WHEN ISNULL(BTblBazdidResult.BazdidZaminiLen,0) > 0 THEN BTblBazdidResult.BazdidZaminiLen ELSE ISNULL(BTblBazdidResult.FromToLengthZamini, Tbl_LPFeeder.ZeminiLength) END) AS BazdidZaminiLen, " &
            "			SUM(CASE WHEN ISNULL(BTblBazdidResult.BazdidHavayiLen,0) > 0 THEN BTblBazdidResult.BazdidHavayiLen ELSE ISNULL(BTblBazdidResult.FromToLengthHavayi, Tbl_LPFeeder.HavaeiLength) END) AS BazdidHavayiLen " &
            "		FROM  " &
            "			BTblBazdidResult " &
            "           LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                        mJoinSpecialitySql &
            "			INNER JOIN Tbl_LPFeeder ON BTblBazdidResult.LPFeederId = Tbl_LPFeeder.LPFeederId " &
            "			INNER JOIN Tbl_LPPost ON Tbl_LPFeeder.LPPostId = Tbl_LPPost.LPPostId " &
            "			INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "           INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "			INNER JOIN " &
            "			( " &
            "				SELECT  " &
            "					BTblBazdidResult.BazdidResultId, " &
            "					MIN(BTblBazdidResultAddress.StartDatePersian) AS StartDate, " &
            "					MAX(BTblBazdidResultAddress.StartDatePersian) AS EndDate " &
            "				FROM  " &
            "					BTblBazdidResultAddress " &
            "					INNER JOIN BTblBazdidResult ON BTblBazdidResultAddress.BazdidResultId = BTblBazdidResult.BazdidResultId " &
            "                   LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                                mJoinSpecialitySql &
            "					INNER JOIN Tbl_LPFeeder ON BTblBazdidResult.LPFeederId = Tbl_LPFeeder.LPFeederId " &
            "				WHERE  " &
            "					(BTblBazdidResult.BazdidTypeId = 3) " &
            "					AND (BTblBazdidResult.BazdidStateId IN (2,3)) " &
            "					AND Tbl_LPFeeder.IsLightFeeder = " & mIsLightFeeder &
                                mWhereDT &
            "				GROUP BY  " &
            "					BTblBazdidResult.BazdidResultId " &
            "			) AS Tbl_StartEndDate ON BTblBazdidResult.BazdidResultId = Tbl_StartEndDate.BazdidResultId " &
            "		WHERE  " &
            "			(BTblBazdidResult.BazdidTypeId = 3) " &
            "			AND (BTblBazdidResult.BazdidStateId IN (2,3)) " & mWhere &
            "			AND Tbl_LPFeeder.IsLightFeeder = " & mIsLightFeeder &
            "		GROUP BY  " &
            "			BTblBazdidResult.AreaId " &
            "		) AS Tbl_Bazdid ON Tbl_Area.AreaId = Tbl_Bazdid.AreaId "

        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_3_34_1", , True)
        If lDS.Tables.Contains("Report_3_34_1") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_3_34_1.xml", XmlWriteMode.WriteSchema)
            mFF.AddFormulaFields("CaptionAll", IIf(mIsLightReport, """ تعداد و طول فيدرهاي روشنايي معابر پيش بيني شده """, """ تعداد و طول فيدرهاي فشار ضعيف پيش بيني شده """))
            mFF.AddFormulaFields("CaptionBazdid", IIf(mIsLightReport, """ عملکرد فيدرهاي روشنايي معابر بازديد شده """, """ عملکرد فيدرهاي فشار ضعيف بازديد شده """))
            ShowReport("", lFolder & "Report_3_34_1.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_3_34_2()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_3_34_2 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mIsActive & "," & IIf(mIsLightReport, 1, 0) & "," & mOwnershipId & ",'" & mBazdidSpeciality & "'"
        lSQL =
            " SELECT DISTINCT " &
            "	Tbl_Area.AreaId, " &
            "	Tbl_Area.Area, " &
            "	Tbl_LPPostCount.cntPublicHavayi, " &
            "	Tbl_LPPostCount.cntPublicZamini, " &
            "	Tbl_Bazdid.cntBazdidHavayi, " &
            "	Tbl_Bazdid.cntBazdidZamini, " &
            "	Tbl_Bazdid.StartDate, " &
            "	Tbl_Bazdid.EndDate " &
            "FROM " &
            "	Tbl_LPPost " &
            "	INNER JOIN Tbl_Area ON Tbl_LPPost.AreaId = Tbl_Area.AreaId " &
            "	INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "   INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "   LEFT JOIN BTblBazdidResult ON Tbl_MPFeeder.MPFeederId = BTblBazdidResult.MPFeederId " &
            "   LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
    "	INNER JOIN " &
            "	( " &
            "	SELECT " &
            "		BTblBazdidResult.AreaId, " &
            "		COUNT (DISTINCT CASE WHEN Tbl_LPPost.IsHavayi = 1 THEN Tbl_LPPost.LPPostId END) AS cntPublicHavayi, " &
            "		COUNT (DISTINCT CASE WHEN Tbl_LPPost.IsHavayi = 0 THEN Tbl_LPPost.LPPostId END) AS cntPublicZamini " &
            "	FROM " &
            "		BTblBazdidResult " &
            "		INNER JOIN Tbl_LPPost ON BTblBazdidResult.LPPostId = Tbl_LPPost.LPPostId " &
            "		INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "       INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "       LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                    mWhere2 &
            "	GROUP BY " &
            "		BTblBazdidResult.AreaId " &
            "	) AS Tbl_LPPostCount ON Tbl_LPPost.AreaId = Tbl_LPPostCount.AreaId " &
            "	INNER JOIN " &
            "	( " &
            "	SELECT " &
            "		BTblBazdidResult.AreaId, " &
            "		COUNT (DISTINCT CASE WHEN Tbl_LPPost.IsHavayi = 1 THEN Tbl_LPPost.LPPostId END) AS cntBazdidHavayi, " &
            "		COUNT (DISTINCT CASE WHEN Tbl_LPPost.IsHavayi = 0 THEN Tbl_LPPost.LPPostId END) AS cntBazdidZamini, " &
            "		MIN(Tbl_StartEndDate.StartDate) AS StartDate, " &
            "		MAX(Tbl_StartEndDate.EndDate) AS EndDate " &
            "	FROM " &
            "		BTblBazdidResult " &
            "		INNER JOIN Tbl_LPPost ON BTblBazdidResult.LPPostId = Tbl_LPPost.LPPostId " &
            "		INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "       INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "		INNER JOIN " &
            "			( " &
            "			SELECT " &
            "				BTblBazdidResult.BazdidResultId, " &
            "				MIN(BTblBazdidResultAddress.StartDatePersian) AS StartDate, " &
            "				MAX(BTblBazdidResultAddress.StartDatePersian) AS EndDate " &
            "			FROM " &
            "				BTblBazdidResult " &
            "				INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
            "               LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                            mJoinSpecialitySql &
            "				INNER JOIN Tbl_LPFeeder ON BTblBazdidResult.LPFeederId = Tbl_LPFeeder.LPFeederId " &
            "				INNER JOIN Tbl_LPPost ON Tbl_LPFeeder.LPPostId = Tbl_LPPost.LPPostId " &
            "				INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "               INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "			WHERE " &
            "				Tbl_LPFeeder.IsLightFeeder = " & mIsLightFeeder &
            "				AND BTblBazdidResult.BazdidStateId IN (2,3)  " &
            "				AND BTblBazdidResult.BazdidTypeId = 3 " & mWhereDT &
            "			GROUP BY " &
            "				BTblBazdidResult.BazdidResultId " &
            "			) AS Tbl_StartEndDate ON BTblBazdidResult.BazdidResultId = Tbl_StartEndDate.BazdidResultId " &
            "               LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                    mWhere2 &
            "	GROUP BY " &
            "		BTblBazdidResult.AreaId " &
            "	) AS Tbl_Bazdid ON Tbl_LPPost.AreaId = Tbl_Bazdid.AreaId "
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_3_34_2", , True)
        If lDS.Tables.Contains("Report_3_34_2") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_3_34_2.xml", XmlWriteMode.WriteSchema)
            mFF.AddFormulaFields("FeederType", IIf(mIsLightReport, """ تعداد پست توزيع که فيدرهاي روشنايي معابر آنها بازديد شده است """, """ تعداد پست توزيع که فيدرهاي فشار ضعيف آنها بازديد شده است """))
            ShowReport("", lFolder & "Report_3_34_2.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_3_34_2_Mode2()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_3_34_2_Mod2 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mIsActive & "," & IIf(mIsLightReport, 1, 0) & "," & mOwnershipId & ",'" & mBazdidSpeciality & "'"
        lSQL =
            "SELECT DISTINCT " &
            "	Tbl_Area.AreaId, " &
            "	Tbl_Area.Area, " &
            "	Tbl_LPPostCount.cntPrivate, " &
            "	Tbl_LPPostCount.cntPublic, " &
            "	Tbl_LPPostCount.cntPublicPrivate, " &
            "	Tbl_LPPostCount.cntAll, " &
            "	Tbl_Bazdid.cntBazdidPrivate, " &
            "	Tbl_Bazdid.cntBazdidPublic, " &
            "	Tbl_Bazdid.cntBazdidPrvPub, " &
            "	Tbl_Bazdid.cntBazdidAll, " &
            "	Tbl_Bazdid.StartDate, " &
            "	Tbl_Bazdid.EndDate " &
            "FROM " &
            "	Tbl_LPPost " &
            "	INNER JOIN Tbl_Area ON Tbl_LPPost.AreaId = Tbl_Area.AreaId " &
            "	INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "   INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "	INNER JOIN " &
            "	( " &
            "	SELECT " &
            "		Tbl_LPPost.AreaId, " &
            "		COUNT(CASE WHEN Tbl_LPPost.OwnershipId = 1 THEN Tbl_LPPost.LPPostId END) AS cntPrivate, " &
            "		COUNT(CASE WHEN Tbl_LPPost.OwnershipId = 2 THEN Tbl_LPPost.LPPostId END) AS cntPublic, " &
            "		COUNT(CASE WHEN Tbl_LPPost.OwnershipId = 3 THEN Tbl_LPPost.LPPostId END) AS cntPublicPrivate, " &
            "		COUNT(Tbl_LPPost.LPPostId) AS cntAll " &
            "	FROM " &
            "		Tbl_LPPost " &
            "		INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "       INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
                    mWhere &
            "	GROUP BY " &
            "		Tbl_LPPost.AreaId " &
            "	) AS Tbl_LPPostCount ON Tbl_LPPost.AreaId = Tbl_LPPostCount.AreaId " &
            "	INNER JOIN " &
            "	( " &
            "	SELECT " &
            "		BTblBazdidResult.AreaId, " &
            "		COUNT (DISTINCT CASE WHEN Tbl_LPPost.OwnershipId = 1 THEN Tbl_LPPost.LPPostId END) AS cntBazdidPrivate, " &
            "		COUNT (DISTINCT CASE WHEN Tbl_LPPost.OwnershipId = 2 THEN Tbl_LPPost.LPPostId END) AS cntBazdidPublic, " &
            "		COUNT (DISTINCT CASE WHEN Tbl_LPPost.OwnershipId = 3 THEN Tbl_LPPost.LPPostId END) AS cntBazdidPrvPub, " &
            "		COUNT (DISTINCT Tbl_LPPost.LPPostId) AS cntBazdidAll, " &
            "		MIN(Tbl_StartEndDate.StartDate) AS StartDate, " &
            "		MAX(Tbl_StartEndDate.EndDate) AS EndDate " &
            "	FROM " &
            "		BTblBazdidResult " &
            "		INNER JOIN Tbl_LPPost ON BTblBazdidResult.LPPostId = Tbl_LPPost.LPPostId " &
            "		INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "       INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "		INNER JOIN " &
            "			( " &
            "			SELECT " &
            "				BTblBazdidResult.BazdidResultId, " &
            "				MIN(BTblBazdidResultAddress.StartDatePersian) AS StartDate, " &
            "				MAX(BTblBazdidResultAddress.StartDatePersian) AS EndDate " &
            "			FROM " &
            "				BTblBazdidResult " &
            "				INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
            "				INNER JOIN Tbl_LPFeeder ON BTblBazdidResult.LPFeederId = Tbl_LPFeeder.LPFeederId " &
            "				INNER JOIN Tbl_LPPost ON Tbl_LPFeeder.LPPostId = Tbl_LPPost.LPPostId " &
            "				INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "               INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "               LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                            mJoinSpecialitySql &
            "			WHERE " &
            "				Tbl_LPFeeder.IsLightFeeder = " & mIsLightFeeder &
            "				AND BTblBazdidResult.BazdidStateId IN (2,3)  " &
            "				AND BTblBazdidResult.BazdidTypeId = 3 " & mWhereDT &
            "			GROUP BY " &
            "				BTblBazdidResult.BazdidResultId " &
            "			) AS Tbl_StartEndDate ON BTblBazdidResult.BazdidResultId = Tbl_StartEndDate.BazdidResultId " &
                    mWhere &
            "	GROUP BY " &
            "		BTblBazdidResult.AreaId " &
            "	) AS Tbl_Bazdid ON Tbl_LPPost.AreaId = Tbl_Bazdid.AreaId " &
            ""
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_3_34_2", , True)
        If lDS.Tables.Contains("Report_3_34_2") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_3_34_2.xml", XmlWriteMode.WriteSchema)
            mFF.AddFormulaFields("FeederType", IIf(mIsLightReport, """ تعداد پست توزيع که فيدرهاي روشنايي معابر آنها بازديد شده است """, """ تعداد پست توزيع که فيدرهاي فشار ضعيف آنها بازديد شده است """))
            ShowReport("", lFolder & "Report_3_34_2_Mode2.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_3_34_2_Mode2_MPFeeder()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_3_34_2_Mod2_MPFeeder " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mIsActive & "," & IIf(mIsLightReport, 1, 0) & "," & mOwnershipId & ",'" & mBazdidSpeciality & "'"
        lSQL =
            "SELECT DISTINCT " &
            "	Tbl_Area.AreaId, " &
            "	Tbl_Area.Area, " &
            "	Tbl_MPPost.MPPostId, " &
            "	Tbl_MPPost.MPPostName, " &
            "	Tbl_MPFeeder.MPFeederId, " &
            "	Tbl_MPFeeder.MPFeederName, " &
            "	Tbl_LPPostCount.cntPrivate, " &
            "	Tbl_LPPostCount.cntPublic, " &
            "	Tbl_LPPostCount.cntPublicPrivate, " &
            "	Tbl_LPPostCount.cntAll, " &
            "	ISNULL(Tbl_Bazdid.cntBazdidPrivate,0) as cntBazdidPrivate, " &
            "	ISNULL(Tbl_Bazdid.cntBazdidPublic,0) as cntBazdidPublic, " &
            "	ISNULL(Tbl_Bazdid.cntBazdidPrvPub,0) as cntBazdidPrvPub, " &
            "	ISNULL(Tbl_Bazdid.cntBazdidAll,0) as cntBazdidAll, " &
            "	ISNULL(Tbl_Bazdid.StartDate,'-') as StartDate, " &
            "	ISNULL(Tbl_Bazdid.EndDate,'-') as EndDate " &
            "FROM " &
            "	Tbl_LPPost " &
            "	INNER JOIN Tbl_Area ON Tbl_LPPost.AreaId = Tbl_Area.AreaId " &
            "	INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "	INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "	INNER JOIN " &
            "	( " &
            "	SELECT " &
            "		Tbl_LPPost.AreaId, " &
            "		Tbl_LPPost.MPFeederId, " &
            "		COUNT(CASE WHEN Tbl_LPPost.OwnershipId = 1 THEN Tbl_LPPost.LPPostId END) AS cntPrivate, " &
            "		COUNT(CASE WHEN Tbl_LPPost.OwnershipId = 2 THEN Tbl_LPPost.LPPostId END) AS cntPublic, " &
            "		COUNT(CASE WHEN Tbl_LPPost.OwnershipId = 3 THEN Tbl_LPPost.LPPostId END) AS cntPublicPrivate, " &
            "		COUNT(Tbl_LPPost.LPPostId) AS cntAll " &
            "	FROM " &
            "		Tbl_LPPost " &
            "		INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "       INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
                    mWhere &
            "	GROUP BY " &
            "		Tbl_LPPost.AreaId, " &
            "		Tbl_LPPost.MPFeederId " &
            "	) AS Tbl_LPPostCount ON Tbl_LPPost.MPFeederId = Tbl_LPPostCount.MPFeederId AND Tbl_LPPost.AreaId = Tbl_LPPostCount.AreaId " &
            "	LEFT JOIN " &
            "	( " &
            "	SELECT " &
            "		Tbl_LPPost.AreaId, " &
            "		Tbl_LPPost.MPFeederId, " &
            "		COUNT (DISTINCT CASE WHEN Tbl_LPPost.OwnershipId = 1 THEN Tbl_LPPost.LPPostId END) AS cntBazdidPrivate, " &
            "		COUNT (DISTINCT CASE WHEN Tbl_LPPost.OwnershipId = 2 THEN Tbl_LPPost.LPPostId END) AS cntBazdidPublic, " &
            "		COUNT (DISTINCT CASE WHEN Tbl_LPPost.OwnershipId = 3 THEN Tbl_LPPost.LPPostId END) AS cntBazdidPrvPub, " &
            "		COUNT (DISTINCT Tbl_LPPost.LPPostId) AS cntBazdidAll, " &
            "		MIN(Tbl_StartEndDate.StartDate) AS StartDate, " &
            "		MAX(Tbl_StartEndDate.EndDate) AS EndDate " &
            "	FROM " &
            "		BTblBazdidResult " &
            "		INNER JOIN Tbl_LPPost ON BTblBazdidResult.LPPostId = Tbl_LPPost.LPPostId " &
            "		INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "       INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "		INNER JOIN " &
            "			( " &
            "			SELECT " &
            "				BTblBazdidResult.BazdidResultId, " &
            "				MIN(BTblBazdidResultAddress.StartDatePersian) AS StartDate, " &
            "				MAX(BTblBazdidResultAddress.StartDatePersian) AS EndDate " &
            "			FROM " &
            "				BTblBazdidResult " &
            "				INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
            "               LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                            mJoinSpecialitySql &
            "				INNER JOIN Tbl_LPFeeder ON BTblBazdidResult.LPFeederId = Tbl_LPFeeder.LPFeederId " &
            "				INNER JOIN Tbl_LPPost ON Tbl_LPFeeder.LPPostId = Tbl_LPPost.LPPostId " &
            "				INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "               INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "			WHERE " &
            "				Tbl_LPFeeder.IsLightFeeder = " & mIsLightFeeder &
            "				AND BTblBazdidResult.BazdidStateId IN (2,3)  " &
            "				AND BTblBazdidResult.BazdidTypeId = 3 " & mWhereDT &
            "			GROUP BY " &
            "				BTblBazdidResult.BazdidResultId " &
            "			) AS Tbl_StartEndDate ON BTblBazdidResult.BazdidResultId = Tbl_StartEndDate.BazdidResultId " &
                    mWhere &
            "	GROUP BY " &
            "		Tbl_LPPost.AreaId, " &
            "		Tbl_LPPost.MPFeederId " &
            "	) AS Tbl_Bazdid ON Tbl_LPPost.MPFeederId = Tbl_Bazdid.MPFeederId AND Tbl_LPPost.AreaId = Tbl_Bazdid.AreaId"
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_3_34_2", , True)
        If lDS.Tables.Contains("Report_3_34_2") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_3_34_2.xml", XmlWriteMode.WriteSchema)
            mFF.AddFormulaFields("FeederType", IIf(mIsLightReport, """ تعداد پست توزيع که فيدرهاي روشنايي معابر آنها بازديد شده است """, """ تعداد پست توزيع که فيدرهاي فشار ضعيف آنها بازديد شده است """))
            ShowReport("", lFolder & "Report_3_34_2_Mode2_MPFeeder.rpt", "", """" & cmbReportName.Text & " به تفکيک فيدر فشار متوسط " & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_3_34_3()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_3_34_3 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mLPPostId & "," & mLPFeederId & "," & mOwnershipId & "," & mIsActive & "," & IIf(mIsLightReport, 1, 0) & ",'" & mPrs & "','" & mCheckLists & "'," & mMinCheckListCount & ",'" & mBazdidSpeciality & "'"
        lSQL =
            "SELECT " &
            "	Tbl_Area.AreaId, " &
            "	Tbl_Area.Area, " &
            "	MIN(view_StartEndDate.StartDate) AS StartDate, " &
            "	MAX(view_StartEndDate.EndDate) AS EndDate, " &
            "	ISNULL(BTbl_BazdidCheckListSubGroup.BazdidCheckListSubGroupId,111222) AS BazdidCheckListSubGroupId, " &
            "	ISNULL(BTbl_BazdidCheckListSubGroup.BazdidCheckListSubGroupName,'ساير') AS BazdidCheckListSubGroupName, " &
            "	SUM(CASE WHEN BTbl_BazdidCheckListGroup.IsHavayi = 1 AND BTblBazdidResult.BazdidTypeId = 3 THEN CASE WHEN BTblBazdidResultCheckList.DefectionCount Is Null OR BTblBazdidResultCheckList.DefectionCount = 0 THEN 1 ELSE BTblBazdidResultCheckList.DefectionCount END END) AS cntHavayi, " &
            "	SUM(CASE WHEN BTbl_BazdidCheckListGroup.IsHavayi = 0 AND BTblBazdidResult.BazdidTypeId = 3 THEN CASE WHEN BTblBazdidResultCheckList.DefectionCount Is Null OR BTblBazdidResultCheckList.DefectionCount = 0 THEN 1 ELSE BTblBazdidResultCheckList.DefectionCount END END) AS cntZamini, " &
            "	SUM(CASE WHEN Tbl_LPPost.IsHavayi = 1 AND BTblBazdidResult.BazdidTypeId = 2 THEN CASE WHEN BTblBazdidResultCheckList.DefectionCount Is Null OR BTblBazdidResultCheckList.DefectionCount = 0 THEN 1 ELSE BTblBazdidResultCheckList.DefectionCount END END) AS cntHavayiPost, " &
            "	SUM(CASE WHEN Tbl_LPPost.IsHavayi = 0 AND BTblBazdidResult.BazdidTypeId = 2 THEN CASE WHEN BTblBazdidResultCheckList.DefectionCount Is Null OR BTblBazdidResultCheckList.DefectionCount = 0 THEN 1 ELSE BTblBazdidResultCheckList.DefectionCount END END) AS cntZaminiPost, " &
            "	view_StartEndDate.BazdidTypeId " &
            "FROM  " &
            "	BTblBazdidResult " &
            "	INNER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId " &
            "	LEFT OUTER JOIN Tbl_LPPost ON BTblBazdidResult.LPPostId = Tbl_LPPost.LPPostId " &
            "	LEFT OUTER JOIN Tbl_MPFeeder AS Tbl_MPFeeder_Post ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder_Post.MPFeederId " &
            "	LEFT OUTER JOIN Tbl_LPFeeder ON BTblBazdidResult.LPFeederId = Tbl_LPFeeder.LPFeederId " &
            "	LEFT OUTER JOIN Tbl_LPPost AS Tbl_LPPost_Feeder ON Tbl_LPFeeder.LPPostId = Tbl_LPPost_Feeder.LPPostId " &
            "	LEFT OUTER JOIN Tbl_MPFeeder ON Tbl_LPPost_Feeder.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "	INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
            "	INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
            "	LEFT OUTER JOIN BTbl_BazdidCheckList ON BTblBazdidResultCheckList.BazdidCheckListId = BTbl_BazdidCheckList.BazdidCheckListId " &
            "	LEFT OUTER JOIN BTbl_BazdidCheckListSubGroup ON BTbl_BazdidCheckList.BazdidCheckListSubGroupId = BTbl_BazdidCheckListSubGroup.BazdidCheckListSubGroupId " &
            "	LEFT JOIN BTbl_BazdidCheckListGroup ON BTbl_BazdidCheckList.BazdidCheckListGroupId = BTbl_BazdidCheckListGroup.BazdidCheckListGroupId " &
            "   LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                mJoinSpecialitySql &
            "	INNER JOIN " &
            "	( " &
            "	SELECT  " &
            "		BTblBazdidResultAddress.BazdidResultAddressId, " &
            "		MIN(BTblBazdidResultAddress.StartDatePersian) AS StartDate, " &
            "		MAX(BTblBazdidResultAddress.StartDatePersian) AS EndDate, " &
            "		BTblBazdidResult.BazdidTypeId " &
            "	FROM  " &
            "		BTblBazdidResult " &
            "		INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
            "		LEFT JOIN Tbl_LPFeeder ON BTblBazdidResult.LPFeederId = Tbl_LPFeeder.LPFeederId " &
            "		LEFT JOIN Tbl_LPPost ON BTblBazdidResult.LPPostId = Tbl_LPPost.LPPostId " &
            "       LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                    mJoinSpecialitySql &
            "	WHERE BTblBazdidResult.BazdidStateId IN (2,3) " &
            "		AND ((BTblBazdidResult.BazdidTypeId = 2) OR (BTblBazdidResult.BazdidTypeId = 3 AND Tbl_LPFeeder.IsLightFeeder = " & mIsLightFeeder & " )) " &
                    mWhereDT &
            "	GROUP BY  " &
            "		BTblBazdidResultAddress.BazdidResultAddressId, " &
            "		BTblBazdidResult.BazdidTypeId " &
            "	) AS view_StartEndDate ON BTblBazdidResultAddress.BazdidResultAddressId = view_StartEndDate.BazdidResultAddressId " &
            "WHERE  " &
            "	(BTblBazdidResult.BazdidStateId IN (2,3)) " &
            "	AND (BTblBazdidResult.BazdidTypeId IN (2,3)) " &
            "	AND BTblBazdidResultCheckList.Priority > 0 " &
                mWhere &
            "GROUP BY  " &
            "	Tbl_Area.AreaId, " &
            "	Tbl_Area.Area, " &
            "	ISNULL(BTbl_BazdidCheckListSubGroup.BazdidCheckListSubGroupId,111222), " &
            "	ISNULL(BTbl_BazdidCheckListSubGroup.BazdidCheckListSubGroupName, 'ساير'), " &
            "	view_StartEndDate.BazdidTypeId " &
                mHaving
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_3_34_3", , True)
        If lDS.Tables.Contains("Report_3_34_3") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_3_34_3.xml", XmlWriteMode.WriteSchema)
            mFF.AddFormulaFields("FeederType", IIf(mIsLightReport, """ روشنايي معابر """, """ فشار ضعيف """))
            ShowReport("", lFolder & "Report_3_34_3.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_3_34_4()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_3_34_4 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mLPPostId & "," & mLPFeederId & "," & mOwnershipId & "," & mIsActive & "," & IIf(mIsLightReport, 1, 0) & ",'" & mBazdidSpeciality & "'"
        lSQL =
            "	SELECT  " &
            "		Tbl_Area.AreaId, " &
            "		Tbl_Area.Area, " &
            "		COUNT(ISNULL(BTblBazdidResultSubCheckList.BazdidResultSubCheckListId,BTblBazdidResultCheckList.SubCheckListId)) AS cnt, " &
            "		BTblBazdidResultCheckList.Priority, " &
            "		MIN(viewStartEndDate.StartDate) AS StartDate, " &
            "		MAX(viewStartEndDate.EndDate) AS EndDate, " &
            "		viewStartEndDate.IsHavayi " &
            "	FROM  " &
            "		Tbl_LPFeeder " &
            "		INNER JOIN Tbl_LPPost ON Tbl_LPFeeder.LPPostId = Tbl_LPPost.LPPostId " &
            "		INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "       INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "		INNER JOIN BTblBazdidResult ON Tbl_LPFeeder.LPFeederId = BTblBazdidResult.LPFeederId " &
            "		INNER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId " &
            "		INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
            "		INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
            "       LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                    mJoinSpecialitySql &
            "		LEFT OUTER JOIN BTblBazdidResultSubCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblBazdidResultSubCheckList.BazdidResultCheckListId " &
            "		INNER JOIN ( " &
            "			SELECT  " &
            "				BTblBazdidResult.BazdidResultId, " &
            "				MIN(BTblBazdidResultAddress.StartDatePersian) AS StartDate, " &
            "				MAX(BTblBazdidResultAddress.StartDatePersian) AS EndDate, " &
            "				CASE WHEN (ISNULL(Tbl_LPFeeder.HavaeiLength,0) > ISNULL(Tbl_LPFeeder.ZeminiLength,0)) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS IsHavayi " &
            "			FROM  " &
            "				BTblBazdidResult " &
            "				INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
            "               LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                            mJoinSpecialitySql &
            "				INNER JOIN Tbl_LPFeeder ON BTblBazdidResult.LPFeederId = Tbl_LPFeeder.LPFeederId " &
            "				INNER JOIN Tbl_LPPost ON Tbl_LPFeeder.LPPostId = Tbl_LPPost.LPPostId " &
            "				INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "               INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "			WHERE  " &
            "				BTblBazdidResult.BazdidStateId IN (2,3) " &
            "				AND BTblBazdidResult.BazdidTypeId = 3 AND Tbl_LPFeeder.IsLightFeeder = " & mIsLightFeeder & mWhereDT &
            "			GROUP BY  " &
            "				BTblBazdidResult.BazdidResultId ,  " &
            "				Tbl_LPFeeder.HavaeiLength , " &
            "				Tbl_LPFeeder.ZeminiLength " &
            "			) AS viewStartEndDate ON BTblBazdidResult.BazdidResultId = viewStartEndDate.BazdidResultId " &
            "	WHERE " &
            "		BTblBazdidResult.BazdidStateId IN (2,3) " &
            "		AND BTblBazdidResult.BazdidTypeId = 3 AND Tbl_LPFeeder.IsLightFeeder = " & mIsLightFeeder & mWhere &
            "	GROUP BY  " &
            "		Tbl_Area.AreaId, " &
            "		Tbl_Area.Area, " &
            "		BTblBazdidResultCheckList.Priority, " &
            "		viewStartEndDate.IsHavayi"
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_3_34_4", , True)
        If lDS.Tables.Contains("Report_3_34_4") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_3_34_4.xml", XmlWriteMode.WriteSchema)
            mFF.AddFormulaFields("FeederType", IIf(mIsLightReport, """ روشنايي معابر """, """ فشار ضعيف """))
            ShowReport("", lFolder & "Report_3_34_4.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_3_34_5()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_3_34_5 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mLPPostId & "," & mLPFeederId & "," & mOwnershipId & "," & mIsActive & ",'" & mParts & "'," & IIf(mIsLightReport, 1, 0) & ",'" & mBazdidSpeciality & "'"
        lSQL =
            "		SELECT  " &
            "			Tbl_Area.AreaId, " &
            "			Tbl_Area.Area, " &
            "			MIN(Tbl_StartEndDate.StartDate) AS StartDate, " &
            "			MAX(Tbl_StartEndDate.EndDate) AS EndDate, " &
            "			COUNT(BTblBazdidResult.BazdidResultId) AS cntBazdid, " &
            "			SUM(ISNULL(BTblBazdidResult.FromToLengthHavayi,Tbl_LPFeeder.HavaeiLength)) AS BazdidHavayi, " &
            "			SUM(ISNULL(BTblBazdidResult.FromToLengthZamini,Tbl_LPFeeder.ZeminiLength)) AS BazdidZamini, " &
            "			BTbl_ServicePart.ServicePartCode,  " &
            "			BTbl_ServicePart.ServicePartName,  " &
            "			Tbl_PartUnit.PartUnit, " &
            "			SUM(BTblBazdidCheckListPart.Quantity) cntCheckList, " &
            "			BTbl_ServicePart.PriceOne,  " &
            "			BTbl_ServicePart.ServicePrice,  " &
            "			SUM(BTbl_ServicePart.PriceOne * BTblBazdidCheckListPart.Quantity) AS PriceCntCheckList,  " &
            "			SUM(BTbl_ServicePart.ServicePrice * BTblBazdidCheckListPart.Quantity) AS PriceCntPrice " &
            "		FROM  " &
            "			BTblBazdidResultCheckList " &
            "			INNER JOIN BTblBazdidResultAddress ON BTblBazdidResultCheckList.BazdidResultAddressId = BTblBazdidResultAddress.BazdidResultAddressId " &
            "			INNER JOIN BTblBazdidResult ON BTblBazdidResultAddress.BazdidResultId = BTblBazdidResult.BazdidResultId " &
            "			INNER JOIN Tbl_LPFeeder ON BTblBazdidResult.LPFeederId = Tbl_LPFeeder.LPFeederId " &
            "			INNER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId " &
            "			INNER JOIN Tbl_LPPost ON Tbl_LPFeeder.LPPostId = Tbl_LPPost.LPPostId " &
            "			INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "           INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "			LEFT OUTER JOIN BTblBazdidCheckListPart ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblBazdidCheckListPart.BazdidResultCheckListId " &
            "			LEFT OUTER JOIN BTbl_ServicePart ON BTblBazdidCheckListPart.ServicePartId = BTbl_ServicePart.ServicePartId " &
            "			LEFT OUTER JOIN Tbl_PartUnit ON BTbl_ServicePart.PartUnitId = Tbl_PartUnit.PartUnitId " &
            "           LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                        mJoinSpecialitySql &
            "			INNER JOIN  " &
            "			( " &
            "				SELECT " &
            "					BTblBazdidResult.BazdidResultId, " &
            "					MIN(BTblBazdidResultAddress.StartDatePersian) AS StartDate, " &
            "					MAX(BTblBazdidResultAddress.StartDatePersian) AS EndDate " &
            "				FROM  " &
            "					BTblBazdidResult " &
            "					INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
            "					INNER JOIN Tbl_LPFeeder ON BTblBazdidResult.LPFeederId = Tbl_LPFeeder.LPFeederId " &
            "                   LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                                mJoinSpecialitySql &
            "				WHERE " &
            "					BTblBazdidResult.BazdidStateId IN (2,3) " &
            "					AND BTblBazdidResult.BazdidTypeId = 3 " &
            "					AND Tbl_LPFeeder.IsLightFeeder = " & mIsLightFeeder &
                                mWhereDT &
            "				GROUP BY " &
            "					BTblBazdidResult.BazdidResultId " &
            "			) AS Tbl_StartEndDate ON BTblBazdidResult.BazdidResultId = Tbl_StartEndDate.BazdidResultId " &
            "			 " &
            "		WHERE  " &
            "			BTblBazdidResult.BazdidStateId IN (2,3) " &
            "			AND BTblBazdidResult.BazdidTypeId = 3 " &
            "			AND BTblBazdidResultCheckList.Priority > 0 " &
            "			AND  " &
            "			( " &
            "				NOT BTbl_ServicePart.ServicePartCode IS NULL " &
            "				OR NOT BTbl_ServicePart.ServicePartName IS NULL " &
            "			) " &
            "			AND Tbl_LPFeeder.IsLightFeeder = " & mIsLightFeeder &
                        mWhere &
            "		GROUP BY " &
            "			Tbl_Area.AreaId, " &
            "			Tbl_Area.Area, " &
            "			BTbl_ServicePart.ServicePartCode,  " &
            "			BTbl_ServicePart.ServicePartName,  " &
            "			Tbl_PartUnit.PartUnit, " &
            "			BTbl_ServicePart.PriceOne,  " &
            "			BTbl_ServicePart.ServicePrice"
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_3_34_5", , True)
        If lDS.Tables.Contains("Report_3_34_5") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_3_34_5.xml", XmlWriteMode.WriteSchema)
            mFF.AddFormulaFields("FeederType", IIf(mIsLightReport, """ روشنايي معابر """, """ فشار ضعيف """))
            ShowReport("", lFolder & "Report_3_34_5.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub

    Private Sub MakeReport_4_1_1()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_4_1_1 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mOwnershipId & "," & mIsActive & ",'" & mBazdidSpeciality & "'"
        lSQL =
            " SELECT  " &
            "	Tbl_Area.AreaId, " &
            "	Tbl_Area.Area, " &
            "	MIN(BTblServiceCheckList.DoneDatePersian) AS StartDate, " &
            "	MAX(BTblServiceCheckList.DoneDatePersian) AS EndDate, " &
            "	MAX(Tbl_MPFeederLen.MPFHavayiCount) AS MPFHavayiCount, " &
            "	MAX(Tbl_MPFeederLen.MPFHavayiLen) AS MPFHavayiLen, " &
            "	MAX(Tbl_MPFeederLen.MPFZaminiCount) AS MPFZaminiCount, " &
            "	MAX(Tbl_MPFeederLen.MPFZaminiLen) AS MPFZaminiLen, " &
            "	MAX(Tbl_MPFeederLen.MPFPrivateCount) AS MPFPrivateCount, " &
            "	MAX(Tbl_MPFeederLen.MPFPrivateLen) AS MPFPrivateLen, " &
            "	COUNT(DISTINCT CASE WHEN ISNULL(Tbl_MPFeeder.HavaeiLength,0) > ISNULL(Tbl_MPFeeder.ZeminiLength,0) THEN Tbl_MPFeeder.MPFeederId END) AS ServiceHavayiCount, " &
            "	COUNT(DISTINCT CASE WHEN ISNULL(Tbl_MPFeeder.HavaeiLength,0) <= ISNULL(Tbl_MPFeeder.ZeminiLength,0) THEN Tbl_MPFeeder.MPFeederId END) AS ServiceZaminiCount, " &
            "	SUM(ISNULL(CASE WHEN ISNULL(BTblBazdidResult.BazdidHavayiLen,0) > 0 THEN BTblBazdidResult.BazdidHavayiLen ELSE ISNULL(BTblBazdidResult.FromToLengthHavayi, Tbl_MPFeeder.HavaeiLength) END ,0)) AS ServiceHavayiLen, " &
            "	SUM(ISNULL(CASE WHEN ISNULL(BTblBazdidResult.BazdidZaminiLen,0) > 0 THEN BTblBazdidResult.BazdidZaminiLen ELSE ISNULL(BTblBazdidResult.FromToLengthZamini, Tbl_MPFeeder.ZeminiLength) END ,0)) AS ServiceZaminiLen " &
            "FROM  " &
            "	BTblBazdidResultAddress " &
            "	INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
            "	INNER JOIN BTblBazdidResult ON BTblBazdidResultAddress.BazdidResultId = BTblBazdidResult.BazdidResultId " &
            "	INNER JOIN Tbl_MPFeeder ON BTblBazdidResult.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "	LEFT JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId " &
            "	LEFT OUTER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId " &
            "   LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                mJoinSpecialitySql &
            "	INNER JOIN " &
            "	( " &
            "	SELECT  " &
            "		Tbl_MPFeeder.AreaId, " &
            "		COUNT(CASE WHEN ISNULL(Tbl_MPFeeder.HavaeiLength,0) > ISNULL(Tbl_MPFeeder.ZeminiLength,0) THEN Tbl_MPFeeder.MPFeederId END) AS MPFHavayiCount, " &
            "		COUNT(CASE WHEN ISNULL(Tbl_MPFeeder.HavaeiLength,0) <= ISNULL(Tbl_MPFeeder.ZeminiLength,0) THEN Tbl_MPFeeder.MPFeederId END) AS MPFZaminiCount, " &
            "		COUNT(CASE WHEN Tbl_MPFeeder.OwnershipId = 1 THEN Tbl_MPFeeder.MPFeederId END) AS MPFPrivateCount, " &
            "		SUM(ISNULL(Tbl_MPFeeder.HavaeiLength,0)) AS MPFHavayiLen, " &
            "		SUM(ISNULL(Tbl_MPFeeder.ZeminiLength,0)) AS MPFZaminiLen, " &
            "		SUM(ISNULL(CASE WHEN Tbl_MPFeeder.OwnershipId = 1 THEN ISNULL(Tbl_MPFeeder.HavaeiLength,0) + ISNULL(Tbl_MPFeeder.ZeminiLength,0) END,0)) AS MPFPrivateLen " &
            "	FROM " &
            "		Tbl_MPFeeder " &
            "	LEFT JOIN BTblBazdidResult ON Tbl_MPFeeder.MPFeederId = BTblBazdidResult.BazdidResultId " &
            "   LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                    mWhere2 &
            "	GROUP BY  " &
            "		Tbl_MPFeeder.AreaId " &
            "	) AS Tbl_MPFeederLen ON Tbl_Area.AreaId = Tbl_MPFeederLen.AreaId " &
            "WHERE  " &
            "	(BTblBazdidResult.BazdidTypeId = 1)  " &
            "	AND (BTblServiceCheckList.ServiceStateId = 3) " &
                mWhere & mWhereDT &
            "GROUP BY  " &
            "	Tbl_Area.AreaId, " &
            "	Tbl_Area.Area "

        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_4_1_1", , True)
        If lDS.Tables.Contains("Report_4_1_1") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_4_1_1.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_4_1_1.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_4_1_2()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_4_1_2 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mOwnershipId & "," & mIsActive & ",'" & mBazdidSpeciality & "'"
        lSQL =
            "SELECT DISTINCT " &
            "	Tbl_Area.Area, " &
            "	Tbl_Area.AreaId, " &
            "	Tbl_MPFeederLen.HavayiCount, " &
            "	Tbl_MPFeederLen.HavayiLen, " &
            "	Tbl_MPFeederLen.ZaminiCount, " &
            "	Tbl_MPFeederLen.ZaminiLen, " &
            "	Tbl_Service.StartDate, " &
            "	Tbl_Service.EndDate, " &
            "	Tbl_Service.ServiceHavayiCount, " &
            "	Tbl_Service.ServiceZaminiCount, " &
            "	Tbl_Service.ServiceHavayiLen, " &
            "	Tbl_Service.ServiceZaminiLen " &
            "FROM  " &
            "	Tbl_Area " &
            "	INNER JOIN Tbl_MPFeeder ON Tbl_Area.AreaId = Tbl_MPFeeder.AreaId " &
            "	INNER JOIN " &
            "	( " &
            "		SELECT " &
            "			Tbl_MPFeeder.AreaId, " &
            "			COUNT(CASE WHEN ISNULL(BTblBazdidResult.FromToLengthHavayi, Tbl_MPFeeder.HavaeiLength) > ISNULL(BTblBazdidResult.FromToLengthZamini, Tbl_MPFeeder.ZeminiLength) THEN BTblBazdidResult.BazdidResultId END) AS HavayiCount, " &
            "			COUNT(CASE WHEN ISNULL(BTblBazdidResult.FromToLengthHavayi, Tbl_MPFeeder.HavaeiLength) <= ISNULL(BTblBazdidResult.FromToLengthZamini, Tbl_MPFeeder.ZeminiLength) THEN BTblBazdidResult.BazdidResultId END) AS ZaminiCount, " &
            "			ISNULL(SUM(ISNULL(BTblBazdidResult.FromToLengthHavayi, Tbl_MPFeeder.HavaeiLength)),0) AS HavayiLen, " &
            "			ISNULL(SUM(ISNULL(BTblBazdidResult.FromToLengthZamini, Tbl_MPFeeder.ZeminiLength)),0) AS ZaminiLen " &
            "		FROM " &
            "			Tbl_MPFeeder " &
            "			INNER JOIN BTblBazdidResult ON Tbl_MPFeeder.MPFeederId = BTblBazdidResult.MPFeederId " &
            "           LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
            "		WHERE " &
            "			BazdidTypeId = 1 " &
                        mWhere &
            "		GROUP BY Tbl_MPFeeder.AreaId " &
            "	) AS Tbl_MPFeederLen ON Tbl_MPFeeder.AreaId = Tbl_MPFeederLen.AreaId " &
            "	INNER JOIN " &
            "	( " &
            "		SELECT  " &
            "			BTblBazdidResult.AreaId, " &
            "			MIN(BTblServiceCheckList.DoneDatePersian) AS StartDate, " &
            "			MAX(BTblServiceCheckList.DoneDatePersian) AS EndDate, " &
            "			COUNT(DISTINCT CASE WHEN ISNULL(BTblBazdidResult.FromToLengthHavayi, Tbl_MPFeeder.HavaeiLength) > ISNULL(BTblBazdidResult.FromToLengthZamini, Tbl_MPFeeder.ZeminiLength) THEN BTblBazdidResult.BazdidResultId END) AS ServiceHavayiCount, " &
            "			COUNT(DISTINCT CASE WHEN ISNULL(BTblBazdidResult.FromToLengthHavayi, Tbl_MPFeeder.HavaeiLength) <= ISNULL(BTblBazdidResult.FromToLengthZamini, Tbl_MPFeeder.ZeminiLength) THEN BTblBazdidResult.BazdidResultId END) AS ServiceZaminiCount, " &
            "			SUM(ISNULL(CASE WHEN ISNULL(BTblBazdidResult.BazdidHavayiLen,0) > 0 THEN BTblBazdidResult.BazdidHavayiLen ELSE ISNULL(BTblBazdidResult.FromToLengthHavayi, Tbl_MPFeeder.HavaeiLength) END ,0)) AS ServiceHavayiLen, " &
            "			SUM(ISNULL(CASE WHEN ISNULL(BTblBazdidResult.BazdidZaminiLen,0) > 0 THEN BTblBazdidResult.BazdidZaminiLen ELSE ISNULL(BTblBazdidResult.FromToLengthZamini, Tbl_MPFeeder.ZeminiLength) END ,0)) AS ServiceZaminiLen " &
            "		FROM  " &
            "			BTblBazdidResult " &
            "			INNER JOIN Tbl_MPFeeder ON BTblBazdidResult.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "			INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
            "			INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
            "			INNER JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId " &
            "           LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                        mJoinSpecialitySql &
            "		WHERE " &
            "			BTblServiceCheckList.ServiceStateId = 3 " &
            "			AND BTblBazdidResult.BazdidTypeId = 1 " &
                        mWhere & mWhereDT &
            "		GROUP BY  " &
            "			BTblBazdidResult.AreaId " &
            "	) AS Tbl_Service ON Tbl_MPFeeder.AreaId = Tbl_Service.AreaId "

        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_4_1_2", , True)
        If lDS.Tables.Contains("Report_4_1_2") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_4_1_2.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_4_1_2.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_4_1_3()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_4_1_3 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mOwnershipId & "," & mIsActive & " ,'" & mPrs & "','" & mCheckLists & "'," & mMinCheckListCount & ",'" & mBazdidSpeciality & "'"
        lSQL =
            "SELECT " &
            "	ISNULL(Tbl_Area_Feeder.AreaId, Tbl_Area_Post.AreaId) AS AreaId, " &
            "	ISNULL(Tbl_Area_Feeder.Area, Tbl_Area_Post.Area) AS Area, " &
            "	MIN(view_StartEndDate.StartDate) AS StartDate, " &
            "	MAX(view_StartEndDate.EndDate) AS EndDate, " &
            "	ISNULL(BTbl_BazdidCheckListSubGroup.BazdidCheckListSubGroupId,111222) AS BazdidCheckListSubGroupId, " &
            "	ISNULL(BTbl_BazdidCheckListSubGroup.BazdidCheckListSubGroupName,'ساير') AS BazdidCheckListSubGroupName, " &
            "	SUM(CASE WHEN BTbl_BazdidCheckListGroup.IsHavayi = 1 AND BTblBazdidResult.BazdidTypeId = 1 THEN CASE WHEN BTblServiceCheckList.ServiceCount Is Null OR BTblServiceCheckList.ServiceCount = 0 THEN 1 ELSE BTblServiceCheckList.ServiceCount END END) AS cntHavayi, " &
            "	SUM(CASE WHEN BTbl_BazdidCheckListGroup.IsHavayi = 0 AND BTblBazdidResult.BazdidTypeId = 1 THEN CASE WHEN BTblServiceCheckList.ServiceCount Is Null OR BTblServiceCheckList.ServiceCount = 0 THEN 1 ELSE BTblServiceCheckList.ServiceCount END END) AS cntZamini, " &
            "	SUM(DISTINCT CASE WHEN Tbl_LPPost.IsHavayi = 1 AND BTblBazdidResult.BazdidTypeId = 2 THEN CASE WHEN BTblServiceCheckList.ServiceCount Is Null OR BTblServiceCheckList.ServiceCount = 0 THEN 1 ELSE BTblServiceCheckList.ServiceCount END END) AS cntHavayiPost, " &
            "	SUM(DISTINCT CASE WHEN Tbl_LPPost.IsHavayi = 0 AND BTblBazdidResult.BazdidTypeId = 2 THEN CASE WHEN BTblServiceCheckList.ServiceCount Is Null OR BTblServiceCheckList.ServiceCount = 0 THEN 1 ELSE BTblServiceCheckList.ServiceCount END END) AS cntZaminiPost, " &
            "	view_StartEndDate.BazdidTypeId " &
            "FROM  " &
            "	BTblBazdidResult " &
            "	LEFT OUTER JOIN Tbl_LPPost ON BTblBazdidResult.LPPostId = Tbl_LPPost.LPPostId " &
            "	LEFT OUTER JOIN Tbl_MPFeeder AS Tbl_MPFeeder_Post ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder_Post.MPFeederId " &
            "	LEFT OUTER JOIN Tbl_Area AS Tbl_Area_Post ON BTblBazdidResult.AreaId = Tbl_Area_Post.AreaId " &
            "	LEFT OUTER JOIN Tbl_MPFeeder ON BTblBazdidResult.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "	LEFT OUTER JOIN Tbl_Area AS Tbl_Area_Feeder ON BTblBazdidResult.AreaId = Tbl_Area_Feeder.AreaId " &
            "	INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
            "	INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
            "	LEFT OUTER JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId " &
            "	LEFT OUTER JOIN BTbl_BazdidCheckList ON BTblBazdidResultCheckList.BazdidCheckListId = BTbl_BazdidCheckList.BazdidCheckListId " &
            "	LEFT OUTER JOIN BTbl_BazdidCheckListSubGroup ON BTbl_BazdidCheckList.BazdidCheckListSubGroupId = BTbl_BazdidCheckListSubGroup.BazdidCheckListSubGroupId " &
            "	LEFT JOIN BTbl_BazdidCheckListGroup ON BTbl_BazdidCheckList.BazdidCheckListGroupId = BTbl_BazdidCheckListGroup.BazdidCheckListGroupId " &
            "   LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                mJoinSpecialitySql &
            "	INNER JOIN " &
            "	( " &
            "	SELECT BTblServiceCheckList.ServiceCheckListId, " &
            "		MIN(BTblServiceCheckList.DoneDatePersian) AS StartDate, " &
            "		MAX(BTblServiceCheckList.DoneDatePersian) AS EndDate, " &
            "		BTblBazdidResult.BazdidTypeId " &
            "	FROM  " &
            "		BTblBazdidResult " &
            "		INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
            "		INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
            "		INNER JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId " &
            "		LEFT JOIN Tbl_MPFeeder ON BTblBazdidResult.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "		LEFT JOIN Tbl_LPPost ON BTblBazdidResult.LPPostId = Tbl_LPPost.LPPostId " &
            "       LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                    mJoinSpecialitySql &
            "	WHERE  " &
            "		(BTblBazdidResult.BazdidStateId IN (2,3)) " &
            "		AND BTblBazdidResult.BazdidTypeId = 1  " &
            "			AND (BTblServiceCheckList.ServiceStateId = 3  " &
            "			OR (BTblServiceCheckList.ServiceStateId = 2 AND BTblServiceCheckList.ServiceCount > 0)) " &
                        mWhereDT &
            "	GROUP BY  " &
            "		BTblServiceCheckList.ServiceCheckListId, " &
            "		BTblBazdidResult.BazdidTypeId " &
            "	) AS view_StartEndDate ON BTblServiceCheckList.ServiceCheckListId = view_StartEndDate.ServiceCheckListId " &
            "WHERE  " &
            "	(BTblBazdidResult.BazdidStateId IN (2,3)) " &
            "	AND (BTblServiceCheckList.ServiceStateId = 3 OR (BTblServiceCheckList.ServiceStateId = 2 AND BTblServiceCheckList.ServiceCount > 0)) " &
            "	AND BTblBazdidResultCheckList.Priority > 0 " &
                mWhere &
            "GROUP BY  " &
            "	ISNULL(Tbl_Area_Feeder.AreaId, Tbl_Area_Post.AreaId), " &
            "	ISNULL(Tbl_Area_Feeder.Area, Tbl_Area_Post.Area), " &
            "	ISNULL(BTbl_BazdidCheckListSubGroup.BazdidCheckListSubGroupId,111222), " &
            "	ISNULL(BTbl_BazdidCheckListSubGroup.BazdidCheckListSubGroupName, 'ساير'), " &
            "	view_StartEndDate.BazdidTypeId " &
                mHaving

        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_4_1_3", , True, , , , 500)
        If lDS.Tables.Contains("Report_4_1_3") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_4_1_3.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_4_1_3.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_4_1_4()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_4_1_4 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mOwnershipId & "," & mIsActive & ",'" & mBazdidSpeciality & "'"
        lSQL =
            "SELECT " &
            "	Tbl_Area.AreaId, " &
            "	Tbl_Area.Area, " &
            "	COUNT(DISTINCT BTblServiceCheckList.BazdidResultCheckListId) AS cnt, " &
            "	BTblBazdidResultCheckList.Priority, " &
            "	MIN(Tbl_StartEndDate.StartDate) AS StartDate, " &
            "	MAX(Tbl_StartEndDate.EndDate) AS EndDate, " &
            "	Tbl_StartEndDate.IsHavayi " &
            "FROM  " &
            "	Tbl_MPFeeder " &
            "	INNER JOIN BTblBazdidResult ON Tbl_MPFeeder.MPFeederId = BTblBazdidResult.MPFeederId " &
            "	INNER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId " &
            "	INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
            "	INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
            "	INNER JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId " &
            "	LEFT OUTER JOIN BTblBazdidResultSubCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblBazdidResultSubCheckList.BazdidResultCheckListId " &
            "	LEFT OUTER JOIN BTbl_SubCheckList AS BTbl_SubCheckList_Old ON BTblBazdidResultCheckList.SubCheckListId = BTbl_SubCheckList_Old.SubCheckListId " &
            "	LEFT OUTER JOIN BTbl_SubCheckList AS BTbl_SubCheckList_New ON BTblBazdidResultSubCheckList.SubCheckListId = BTbl_SubCheckList_New.SubCheckListId " &
            "   LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                mJoinSpecialitySql &
            "	INNER JOIN  " &
            "		( " &
            "			SELECT  " &
            "				BTblBazdidResult.BazdidResultId, " &
            "				MIN(BTblServiceCheckList.DoneDatePersian) AS StartDate, " &
            "				MAX(BTblServiceCheckList.DoneDatePersian) AS EndDate, " &
            "				CASE WHEN (ISNULL(Tbl_MPFeeder.HavaeiLength,0) > ISNULL(Tbl_MPFeeder.ZeminiLength,0)) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS IsHavayi " &
            "			FROM " &
            "				Tbl_MPFeeder " &
            "				INNER JOIN BTblBazdidResult ON Tbl_MPFeeder.MPFeederId = BTblBazdidResult.MPFeederId " &
            "				INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
            "				INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
            "				INNER JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId " &
            "               LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                            mJoinSpecialitySql &
            "			WHERE " &
            "				BTblBazdidResult.BazdidTypeId = 1 " &
            "				AND BTblServiceCheckList.ServiceStateId = 3 " &
                            mWhereDT &
            "			GROUP BY " &
            "				BTblBazdidResult.BazdidResultId, " &
            "				Tbl_MPFeeder.HavaeiLength, " &
            "				Tbl_MPFeeder.ZeminiLength " &
            "		) AS Tbl_StartEndDate ON BTblBazdidResult.BazdidResultId = Tbl_StartEndDate.BazdidResultId " &
            "WHERE " &
            "	BTblServiceCheckList.ServiceStateId = 3 " &
            "	AND BTblBazdidResult.BazdidTypeId = 1  " &
            "	AND ( " &
            "		NOT ISNULL(BTbl_SubCheckList_New.SubCheckListCode,BTbl_SubCheckList_Old.SubCheckListCode) IS NULL " &
            "		OR NOT ISNULL(BTbl_SubCheckList_New.SubCheckListName,BTbl_SubCheckList_Old.SubCheckListName) IS NULL " &
            "		) " &
                mWhere.Replace("BTbl_SubCheckList.SubCheckListId", "ISNULL(BTbl_SubCheckList_New.SubCheckListId,BTbl_SubCheckList_Old.SubCheckListId)") &
            "GROUP BY  " &
            "	Tbl_Area.AreaId, " &
            "	Tbl_Area.Area, " &
            "	BTblBazdidResultCheckList.Priority, " &
            "	Tbl_StartEndDate.IsHavayi "

        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_4_1_4", , True)
        If lDS.Tables.Contains("Report_4_1_4") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_4_1_4.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_4_1_4.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_4_1_5()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_4_1_5 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mOwnershipId & "," & mIsActive & ",'" & mParts & "','" & mBazdidSpeciality & "'"
        lSQL =
            "SELECT " &
            "	Tbl_Area.AreaId, " &
            "	Tbl_Area.Area, " &
            "	MIN(StartDate) AS StartDate, " &
            "	MAX(EndDate) AS EndDate, " &
            "	cntBazdid, " &
            "	cntService, " &
            "	BTbl_ServicePart.ServicePartCode,  " &
            "	BTbl_ServicePart.ServicePartName,  " &
            "	SUM(BTblServicePartUse.Quantity) AS Quantity, " &
            "	BTbl_ServicePart.PriceOne,  " &
            "	BTbl_ServicePart.ServicePrice,  " &
            "	Tbl_PartUnit.PartUnit " &
            "FROM  " &
            "	BTblBazdidResultAddress " &
            "	INNER JOIN BTblBazdidResult ON BTblBazdidResultAddress.BazdidResultId = BTblBazdidResult.BazdidResultId " &
            "	INNER JOIN BTblServicePartUse ON BTblBazdidResultAddress.BazdidResultAddressId = BTblServicePartUse.BazdidResultAddressId " &
            "	INNER JOIN BTbl_ServicePart ON BTblServicePartUse.ServicePartId = BTbl_ServicePart.ServicePartId " &
            "	LEFT OUTER JOIN Tbl_PartUnit ON BTbl_ServicePart.PartUnitId = Tbl_PartUnit.PartUnitId " &
            "	INNER JOIN Tbl_MPFeeder ON BTblBazdidResult.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "	INNER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId " &
            "   LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                mJoinSpecialitySql &
            "	INNER JOIN " &
            "	( " &
            "		SELECT DISTINCT " &
            "			BTblBazdidResultCheckList.BazdidResultAddressId, " &
            "			MIN(BTblServiceCheckList.DoneDatePersian) AS StartDate, " &
            "			MAX(BTblServiceCheckList.DoneDatePersian) AS EndDate " &
            "		FROM " &
            "			BTblBazdidResultCheckList " &
            "			INNER JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId " &
            "			INNER JOIN BTblService ON BTblServiceCheckList.ServiceId = BTblService.ServiceId " &
            "           INNER JOIN BTblBazdidResultAddress ON BTblBazdidResultCheckList.BazdidResultAddressId = BTblBazdidResultAddress.BazdidResultAddressId " &
            "           INNER JOIN BTblBazdidResult ON BTblBazdidResultAddress.BazdidResultId = BTblBazdidResult.BazdidResultId " &
            "           LEFT JOIN Tbl_MPFeeder ON BTblBazdidResult.MPFeederId = Tbl_MPFeeder.MPFeederId " &
                        mWhereDT2 &
            "		GROUP BY " &
            "			BTblBazdidResultCheckList.BazdidResultAddressId " &
            "	) AS Tbl_FilterDate ON BTblBazdidResultAddress.BazdidResultAddressId = Tbl_FilterDate.BazdidResultAddressId " &
            "	LEFT JOIN " &
            "	( " &
            "		SELECT DISTINCT " &
            "			BTblBazdidResult.AreaId, " &
            "			COUNT(DISTINCT CASE WHEN BTblBazdidResult.BazdidStateId IN (2,3) THEN BTblBazdidResultCheckList.BazdidResultCheckListId END) AS cntBazdid, " &
            "			COUNT(DISTINCT CASE WHEN BTblServiceCheckList.ServiceStateId = 3 THEN BTblServiceCheckList.BazdidResultCheckListId END) AS cntService " &
            "		FROM " &
            "			BTblBazdidResult " &
            "			INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
            "			INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
            "			LEFT JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId " &
            "			INNER JOIN Tbl_MPFeeder ON BTblBazdidResult.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "           LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                        mJoinSpecialitySql &
            "		WHERE " &
            "			BTblBazdidResult.BazdidTypeId = 1 " &
                        mWhereDT & mWhere &
            "		GROUP BY " &
            "			BTblBazdidResult.AreaId " &
            "	) AS Tbl_BazdidService ON BTblBazdidResult.AreaId = Tbl_BazdidService.AreaId " &
            "WHERE  " &
            "	BTblBazdidResult.BazdidStateId IN (2,3) " &
            "	AND BTblBazdidResult.BazdidTypeId = 1 " &
            "	AND NOT BTbl_ServicePart.ServicePartId IS NULL " &
                mWhere & mWherePart &
            "GROUP BY " &
            "	Tbl_Area.AreaId, " &
            "	Tbl_Area.Area, " &
            "	cntBazdid, " &
            "	cntService, " &
            "	BTbl_ServicePart.ServicePartCode,  " &
            "	BTbl_ServicePart.ServicePartName,  " &
            "	BTbl_ServicePart.PriceOne,  " &
            "	BTbl_ServicePart.ServicePrice,  " &
            "	Tbl_PartUnit.PartUnit "

        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_4_1_5", , True)
        If lDS.Tables.Contains("Report_4_1_5") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_4_1_5.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_4_1_5.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_4_2_1()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_4_2_1 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mLPPostId & "," & mOwnershipId & "," & mIsActive & ",'" & mBazdidSpeciality & "'"
        lSQL =
            " SELECT DISTINCT " &
            "	Tbl_Area.AreaId, " &
            "	Tbl_Area.Area, " &
            "	HavayiCount, " &
            "	ZaminiCount, " &
            "	PrivateCount, " &
            "	ServiceHavayiCount, " &
            "	ServiceZaminiCount, " &
            "	ServicePrivateCount, " &
            "	Tbl_Bazdid.StartDatePersian, " &
            "	Tbl_Bazdid.EndDatePersian " &
            "FROM " &
            "	Tbl_Area " &
            "	INNER JOIN Tbl_MPFeeder ON Tbl_Area.AreaId = Tbl_MPFeeder.AreaId " &
            "   LEFT JOIN BTblBazdidResult ON Tbl_MPFeeder.MPFeederId = BTblBazdidResult.MPFeederId " &
            "   LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
            "	INNER JOIN " &
            "	( " &
            "	SELECT  " &
            "		Tbl_MPFeeder.AreaId, " &
            "		COUNT(CASE WHEN ISNULL(Tbl_MPFeeder.HavaeiLength,0) > ISNULL(Tbl_MPFeeder.ZeminiLength,0) THEN Tbl_MPFeeder.MPFeederId END) AS HavayiCount, " &
            "		COUNT(CASE WHEN ISNULL(Tbl_MPFeeder.HavaeiLength,0) <= ISNULL(Tbl_MPFeeder.ZeminiLength,0) THEN Tbl_MPFeeder.MPFeederId END) AS ZaminiCount, " &
            "		COUNT(CASE WHEN Tbl_MPFeeder.OwnershipId = 1 THEN Tbl_MPFeeder.MPFeederId END) AS PrivateCount " &
            "	FROM " &
            "		Tbl_MPFeeder " &
            "			LEFT JOIN BTblBazdidResult ON Tbl_MPFeeder.MPFeederId = BTblBazdidResult.MPFeederId " &
            "           LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                    mWhere2 &
            "	GROUP BY  " &
            "		Tbl_MPFeeder.AreaId " &
            "	) AS Tbl_MPFeederLen ON Tbl_Area.AreaId = Tbl_MPFeederLen.AreaId " &
            "	INNER JOIN " &
            "	( " &
            "	SELECT " &
            "		BTblBazdidResult.AreaId, " &
            "		COUNT(DISTINCT CASE WHEN ISNULL(Tbl_MPFeeder.HavaeiLength,0) > ISNULL(Tbl_MPFeeder.ZeminiLength,0) THEN Tbl_MPFeeder.MPFeederId END) AS ServiceHavayiCount, " &
            "		COUNT(DISTINCT CASE WHEN ISNULL(Tbl_MPFeeder.HavaeiLength,0) <= ISNULL(Tbl_MPFeeder.ZeminiLength,0) THEN Tbl_MPFeeder.MPFeederId END) AS ServiceZaminiCount, " &
            "		COUNT(DISTINCT CASE WHEN Tbl_MPFeeder.OwnershipId = 1 THEN Tbl_MPFeeder.MPFeederId END) AS ServicePrivateCount, " &
            "		MIN(BTblServiceCheckList.DoneDatePersian) AS StartDatePersian, " &
            "		MAX(BTblServiceCheckList.DoneDatePersian) AS EndDatePersian " &
            "	FROM " &
            "		BTblBazdidResult " &
            "		INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
            "		INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
            "		INNER JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId " &
            "		INNER JOIN Tbl_LPPost ON BTblBazdidResult.LPPostId = Tbl_LPPost.LPPostId " &
            "		INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "       INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "       LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                    mJoinSpecialitySql &
            "	WHERE      " &
            "		(BTblBazdidResult.BazdidTypeId = 2) " &
            "		AND BTblBazdidResult.BazdidStateId IN (2,3) " &
            "		AND BTblServiceCheckList.ServiceStateId = 3 " &
                    mWhereDT & mWhere &
            "	GROUP BY " &
            "		BTblBazdidResult.AreaId " &
            "	) AS Tbl_Bazdid ON Tbl_Area.AreaId = Tbl_Bazdid.AreaId " &
            mWhere2

        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_4_2_1", , True)
        If lDS.Tables.Contains("Report_4_2_1") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_4_2_1.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_4_2_1.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_4_2_2()
        MakeQuery()

        Dim lWhereDT As String = ""
        If mFromDate <> "''" Then
            lWhereDT = " AND BTblBazdidResultAddress.StartDatePersian >= " & mFromDate
        End If
        If mToDate <> "''" Then
            lWhereDT &= " AND BTblBazdidResultAddress.StartDatePersian <= " & mToDate
        End If

        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_4_2_2 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mLPPostId & "," & mOwnershipId & "," & mIsActive & ",'" & mBazdidSpeciality & "'"
        lSQL =
            "SELECT DISTINCT " &
            "	Tbl_Area.AreaId, " &
            "	Tbl_Area.Area, " &
            "	HavayiPublicCount, " &
            "	HavayiPrivateCount, " &
            "	ZaminiPublicCount, " &
            "	ZaminiPrivateCount, " &
            "	OtherCount, " &
            "	AllCount, " &
            "	StartDate, " &
            "	EndDate, " &
            "	cntBazdid, " &
            "	ServiceHavayiPublicCount, " &
            "	ServiceHavayiPrivateCount, " &
            "	ServiceZaminiPublicCount, " &
            "	ServiceZaminiPrivateCount, " &
            "	ServiceOtherCount, " &
            "	ServiceAllCount " &
            "FROM " &
            "	Tbl_LPPost " &
            "	INNER JOIN Tbl_Area ON Tbl_LPPost.AreaId = Tbl_Area.AreaId " &
            "	INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "   INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "	INNER JOIN " &
            "	( " &
            "	SELECT " &
            "		Tbl_LPPost.AreaId, " &
            "		COUNT(CASE WHEN Tbl_LPPost.IsHavayi = 1 AND Tbl_LPPost.OwnershipId = 2 THEN Tbl_LPPost.LPPostId END) AS HavayiPublicCount, " &
            "		COUNT(CASE WHEN Tbl_LPPost.IsHavayi = 1 AND Tbl_LPPost.OwnershipId = 1 THEN Tbl_LPPost.LPPostId END) AS HavayiPrivateCount, " &
            "		COUNT(CASE WHEN Tbl_LPPost.IsHavayi = 0 AND Tbl_LPPost.OwnershipId = 2 THEN Tbl_LPPost.LPPostId END) AS ZaminiPublicCount, " &
            "		COUNT(CASE WHEN Tbl_LPPost.IsHavayi = 0 AND Tbl_LPPost.OwnershipId = 1 THEN Tbl_LPPost.LPPostId END) AS ZaminiPrivateCount, " &
            "		COUNT(CASE WHEN Tbl_LPPost.OwnershipId = 3 OR Tbl_LPPost.OwnershipId IS NULL THEN Tbl_LPPost.LPPostId END) AS OtherCount, " &
            "		COUNT(Tbl_LPPost.LPPostId) AS AllCount " &
            "	FROM " &
            "		Tbl_LPPost " &
            "		INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "       INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            mWherePost &
            "	GROUP BY " &
            "		Tbl_LPPost.AreaId " &
            "	) AS Tbl_LPPostLen ON Tbl_Area.AreaId = Tbl_LPPostLen.AreaId " &
            "	INNER JOIN " &
            "	( " &
            "	SELECT " &
            "		BTblBazdidResult.AreaId, " &
            "		MIN(BTblServiceCheckList.DoneDatePersian) AS StartDate, " &
            "		MAX(BTblServiceCheckList.DoneDatePersian) AS EndDate, " &
            "		COUNT(DISTINCT CASE WHEN Tbl_LPPost.IsHavayi = 1 AND Tbl_LPPost.OwnershipId = 2 THEN Tbl_LPPost.LPPostId END) AS ServiceHavayiPublicCount, " &
            "		COUNT(DISTINCT CASE WHEN Tbl_LPPost.IsHavayi = 1 AND Tbl_LPPost.OwnershipId = 1 THEN Tbl_LPPost.LPPostId END) AS ServiceHavayiPrivateCount, " &
            "		COUNT(DISTINCT CASE WHEN Tbl_LPPost.IsHavayi = 0 AND Tbl_LPPost.OwnershipId = 2 THEN Tbl_LPPost.LPPostId END) AS ServiceZaminiPublicCount, " &
            "		COUNT(DISTINCT CASE WHEN Tbl_LPPost.IsHavayi = 0 AND Tbl_LPPost.OwnershipId = 1 THEN Tbl_LPPost.LPPostId END) AS ServiceZaminiPrivateCount, " &
            "		COUNT(DISTINCT CASE WHEN Tbl_LPPost.OwnershipId = 3 OR Tbl_LPPost.OwnershipId IS NULL THEN Tbl_LPPost.LPPostId END) AS ServiceOtherCount, " &
            "		COUNT(DISTINCT Tbl_LPPost.LPPostId) AS ServiceAllCount " &
            "	FROM  " &
            "		BTblBazdidResult  " &
            "		INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
            "		INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
            "		INNER JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId " &
            "		INNER JOIN Tbl_LPPost ON BTblBazdidResult.LPPostId = Tbl_LPPost.LPPostId " &
            "		INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "       INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "       LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                    mJoinSpecialitySql &
            "	WHERE      " &
            "		(BTblBazdidResult.BazdidTypeId = 2)  " &
            "		AND (BTblBazdidResult.BazdidStateId IN (2, 3)) " &
            "		AND BTblServiceCheckList.ServiceStateId = 3 " &
                    mWhereDT & mWhere &
            "	GROUP BY  " &
            "		BTblBazdidResult.AreaId " &
            "	) AS Tbl_Bazdid ON Tbl_LPPost.AreaId = Tbl_Bazdid.AreaId  " &
            "	INNER JOIN " &
            "	( " &
            "	SELECT " &
            "		BTblBazdidResult.AreaId, " &
            "		COUNT(DISTINCT Tbl_LPPost.LPPostId) as cntBazdid " &
            "	FROM " &
            "		Tbl_LPPost " &
            "		INNER JOIN BTblBazdidResult ON Tbl_LPPost.LPPostId = BTblBazdidResult.LPPostId " &
            "		INNER JOIN " &
            "		( " &
            "			SELECT  " &
            "				BTblBazdidResult.BazdidResultId " &
            "			FROM " &
            "				BTblBazdidResult " &
            "				INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
            "				INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
            "				INNER JOIN Tbl_LPPost ON BTblBazdidResult.LPPostId = Tbl_LPPost.LPPostId " &
            "				INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "               INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "               LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                            mJoinSpecialitySql &
            "			WHERE " &
            "				BTblBazdidResult.BazdidTypeId = 2 " &
            "				AND BTblBazdidResult.BazdidStateId IN (2,3) " &
            "				AND BTblBazdidResultCheckList.Priority > 0 " &
                            lWhereDT & mWhere &
            "		) AS Tbl_StartEndDate ON BTblBazdidResult.BazdidResultId = Tbl_StartEndDate.BazdidResultId " &
            "	WHERE " &
            "		BTblBazdidResult.BazdidTypeId = 2 " &
            "		AND BTblBazdidResult.BazdidStateId IN (2,3) " &
            "	GROUP BY " &
            "		BTblBazdidResult.AreaId " &
            "	) as Tbl_BazdidCount ON Tbl_Area.AreaId = Tbl_BazdidCount.AreaId"
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_4_2_2", , True)
        If lDS.Tables.Contains("Report_4_2_2") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_4_2_2.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_4_2_2.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_4_2_3()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_4_2_3 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mLPPostId & "," & mOwnershipId & "," & mIsActive & ",'" & mPrs & "','" & mCheckLists & "'," & mMinCheckListCount & ",'" & mBazdidSpeciality & "'"
        lSQL =
            "SELECT  " &
            "	Tbl_Area.AreaId, " &
            "	Tbl_Area.Area, " &
            "	MIN(BTblServiceCheckList.DoneDatePersian) AS StartDate, " &
            "	MAX(BTblServiceCheckList.DoneDatePersian) AS EndDate, " &
            "	SUM(CASE WHEN BTblServiceCheckList.ServiceCount Is Null OR BTblServiceCheckList.ServiceCount = 0 THEN 1 ELSE BTblServiceCheckList.ServiceCount END) AS CheckListCount, " &
            "	BTbl_LPPostDetail.LPPostDetailName, " &
            "	BTbl_BazdidCheckListGroup.BazdidCheckListGroupId, " &
            "	ISNULL(BTbl_BazdidCheckListSubGroup.BazdidCheckListSubGroupId,111222) AS BazdidCheckListSubGroupId, " &
            "	ISNULL(BTbl_BazdidCheckListSubGroup.BazdidCheckListSubGroupName,'ساير') AS BazdidCheckListSubGroupName " &
            "FROM " &
            "	Tbl_LPPost " &
            "	INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "   INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "	INNER JOIN BTblBazdidResult ON Tbl_LPPost.LPPostId = BTblBazdidResult.LPPostId " &
            "	INNER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId " &
            "	INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
            "	INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
            "	INNER JOIN BTbl_BazdidCheckList ON BTblBazdidResultCheckList.BazdidCheckListId = BTbl_BazdidCheckList.BazdidCheckListId " &
            "	INNER JOIN BTbl_LPPostDetail ON BTbl_BazdidCheckList.LPPostDetailId = BTbl_LPPostDetail.LPPostDetailId " &
            "	LEFT OUTER JOIN BTbl_BazdidCheckListGroup ON BTbl_BazdidCheckList.BazdidCheckListGroupId = BTbl_BazdidCheckListGroup.BazdidCheckListGroupId " &
            "	LEFT OUTER JOIN BTbl_BazdidCheckListSubGroup ON BTbl_BazdidCheckList.BazdidCheckListSubGroupId = BTbl_BazdidCheckListSubGroup.BazdidCheckListSubGroupId " &
            "	INNER JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId " &
            "   LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                mJoinSpecialitySql &
            "WHERE      " &
            "	(BTblBazdidResult.BazdidTypeId = 2)  " &
            "	AND (BTblBazdidResult.BazdidStateId IN (2, 3)) " &
            "	AND (BTblServiceCheckList.ServiceStateId = 3 OR (BTblServiceCheckList.ServiceStateId = 2 AND BTblServiceCheckList.ServiceCount > 0)) " &
            "	AND BTblBazdidResultCheckList.Priority > 0 " &
                mWhere &
            "GROUP BY  " &
            "	Tbl_Area.AreaId, " &
            "	Tbl_Area.Area, " &
            "	BTbl_LPPostDetail.LPPostDetailName, " &
            "	BTbl_BazdidCheckListGroup.BazdidCheckListGroupId, " &
            "	ISNULL(BTbl_BazdidCheckListSubGroup.BazdidCheckListSubGroupId,111222), " &
            "	ISNULL(BTbl_BazdidCheckListSubGroup.BazdidCheckListSubGroupName,'ساير') " &
                mHaving
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_4_2_3", , True)
        If lDS.Tables.Contains("Report_4_2_3") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_4_2_3.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_4_2_3.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_4_2_4()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_4_2_4 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mLPPostId & "," & mOwnershipId & "," & mIsActive & ",'" & mBazdidSpeciality & "'"
        lSQL =
            "SELECT  " &
            "	Tbl_Area.AreaId, " &
            "	Tbl_Area.Area, " &
            "	COUNT(ISNULL(BTblServiceCheckList.BazdidResultCheckListId,ISNULL(BTblBazdidResultSubCheckList.BazdidResultSubCheckListId,BTblBazdidResultCheckList.SubCheckListId))) AS cnt, " &
            "	BTblBazdidResultCheckList.Priority, " &
            "	MIN(BTblServiceCheckList.DoneDatePersian) AS StartDate, " &
            "	MAX(BTblServiceCheckList.DoneDatePersian) AS EndDate, " &
            "	Tbl_LPPost.IsHavayi " &
            "FROM  " &
            "	Tbl_LPPost " &
            "	INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "   INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "	INNER JOIN BTblBazdidResult ON Tbl_LPPost.LPPostId = BTblBazdidResult.LPPostId " &
            "	INNER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId " &
            "	INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
            "	INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
            "	INNER JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId " &
            "	LEFT OUTER JOIN BTblBazdidResultSubCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblBazdidResultSubCheckList.BazdidResultCheckListId " &
            "	LEFT OUTER JOIN BTbl_SubCheckList AS BTbl_SubCheckList_Old ON BTblBazdidResultCheckList.SubCheckListId = BTbl_SubCheckList_Old.SubCheckListId " &
            "	LEFT OUTER JOIN BTbl_SubCheckList AS BTbl_SubCheckList_New ON BTblBazdidResultSubCheckList.SubCheckListId = BTbl_SubCheckList_New.SubCheckListId		 " &
            "   LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                mJoinSpecialitySql &
            "WHERE " &
            "	BTblBazdidResult.BazdidStateId IN (2,3) " &
            "	AND BTblBazdidResult.BazdidTypeId = 2  " &
            "	AND BTblServiceCheckList.ServiceStateId = 3 " &
            "	AND " &
            "	( " &
            "		NOT ISNULL(BTbl_SubCheckList_New.SubCheckListCode,BTbl_SubCheckList_Old.SubCheckListCode) IS NULL " &
            "		OR NOT ISNULL(BTbl_SubCheckList_New.SubCheckListName,BTbl_SubCheckList_Old.SubCheckListName) IS NULL " &
            "	) " &
                mWhere.Replace("BTbl_SubCheckList.SubCheckListId", "ISNULL(BTbl_SubCheckList_New.SubCheckListId,BTbl_SubCheckList_Old.SubCheckListId)") &
            "GROUP BY  " &
            "	Tbl_Area.AreaId, " &
            "	Tbl_Area.Area, " &
            "	BTblBazdidResultCheckList.Priority, " &
            "	Tbl_LPPost.IsHavayi"
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_4_2_4", , True)
        If lDS.Tables.Contains("Report_4_2_4") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_4_2_4.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_4_2_4.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_4_2_5()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_4_2_5 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mLPPostId & "," & mOwnershipId & "," & mIsActive & ",'" & mParts & "','" & mBazdidSpeciality & "'"
        lSQL =
            "SELECT  " &
            "	Tbl_Area.AreaId, " &
            "	Tbl_Area.Area, " &
            "	MIN(StartDate) AS StartDate, " &
            "	MAX(EndDate) AS EndDate, " &
            "	cntBazdid, " &
            "	cntService, " &
            "	BTbl_ServicePart.ServicePartCode,  " &
            "	BTbl_ServicePart.ServicePartName,  " &
            "	SUM(BTblServicePartUse.Quantity) AS Quantity, " &
            "	BTbl_ServicePart.PriceOne,  " &
            "	BTbl_ServicePart.ServicePrice,  " &
            "	Tbl_PartUnit.PartUnit " &
            "FROM  " &
            "	BTblBazdidResultAddress " &
            "	INNER JOIN BTblBazdidResult ON BTblBazdidResultAddress.BazdidResultId = BTblBazdidResult.BazdidResultId " &
            "	INNER JOIN BTblServicePartUse ON BTblBazdidResultAddress.BazdidResultAddressId = BTblServicePartUse.BazdidResultAddressId " &
            "	INNER JOIN BTbl_ServicePart ON BTblServicePartUse.ServicePartId = BTbl_ServicePart.ServicePartId " &
            "	LEFT OUTER JOIN Tbl_PartUnit ON BTbl_ServicePart.PartUnitId = Tbl_PartUnit.PartUnitId " &
            "	INNER JOIN Tbl_LPPost ON BTblBazdidResult.LPPostId = Tbl_LPPost.LPPostId " &
            "	INNER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId " &
            "	INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "   INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "   LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                mJoinSpecialitySql &
            "	INNER JOIN " &
            "	( " &
            "		SELECT DISTINCT " &
            "			BTblBazdidResultCheckList.BazdidResultAddressId, " &
            "			MIN(BTblServiceCheckList.DoneDatePersian) AS StartDate, " &
            "			MAX(BTblServiceCheckList.DoneDatePersian) AS EndDate " &
            "		FROM " &
            "			BTblBazdidResultCheckList " &
            "			INNER JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId " &
            "			INNER JOIN BTblService ON BTblServiceCheckList.ServiceId = BTblService.ServiceId " &
                        mWhereDT2 &
            "		GROUP BY " &
            "			BTblBazdidResultCheckList.BazdidResultAddressId " &
            "	) AS Tbl_FilterDate ON BTblBazdidResultAddress.BazdidResultAddressId = Tbl_FilterDate.BazdidResultAddressId " &
            "	LEFT JOIN " &
            "	( " &
            "		SELECT DISTINCT " &
            "			BTblBazdidResult.AreaId, " &
            "			COUNT(DISTINCT CASE WHEN BTblBazdidResult.BazdidStateId IN (2,3) THEN BTblBazdidResultCheckList.BazdidResultCheckListId END) AS cntBazdid, " &
            "			COUNT(DISTINCT CASE WHEN BTblServiceCheckList.ServiceStateId = 3 THEN BTblServiceCheckList.BazdidResultCheckListId END) AS cntService " &
            "		FROM " &
            "			BTblBazdidResult " &
            "			INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
            "			INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
            "			LEFT JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId " &
            "			INNER JOIN Tbl_LPPost ON BTblBazdidResult.LPPostId = Tbl_LPPost.LPPostId " &
            "			INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "           INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "           LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                        mJoinSpecialitySql &
            "		WHERE " &
            "			BTblBazdidResult.BazdidTypeId = 2 " &
                        mWhereDT & mWhere &
            "		GROUP BY " &
            "			BTblBazdidResult.AreaId " &
            "	) AS Tbl_BazdidService ON Tbl_Area.AreaId = Tbl_BazdidService.AreaId " &
            "WHERE  " &
            "	BTblBazdidResult.BazdidStateId IN (2,3) " &
            "	AND BTblBazdidResult.BazdidTypeId = 2 " &
            "	AND NOT BTbl_ServicePart.ServicePartId IS NULL " &
                mWhere & mWherePart &
            "GROUP BY " &
            "	Tbl_Area.AreaId, " &
            "	Tbl_Area.Area, " &
            "	cntBazdid, " &
            "	cntService, " &
            "	BTbl_ServicePart.ServicePartCode,  " &
            "	BTbl_ServicePart.ServicePartName,  " &
            "	BTbl_ServicePart.PriceOne,  " &
            "	BTbl_ServicePart.ServicePrice,  " &
            "	Tbl_PartUnit.PartUnit"
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_4_2_5", , True)
        If lDS.Tables.Contains("Report_4_2_5") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_4_2_5.xml", XmlWriteMode.WriteSchema)
            ShowReport("", lFolder & "Report_4_2_5.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_4_34_1()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_4_34_1 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mLPPostId & "," & mLPFeederId & "," & mOwnershipId & "," & mIsActive & "," & IIf(mIsLightReport, 1, 0) & ",'" & mBazdidSpeciality & "'"
        lSQL =
            "SELECT DISTINCT " &
            "	Tbl_Area.Area, " &
            "	ISNULL(LPFHavayiCount,0) AS LPFHavayiCount, " &
            "	ISNULL(LPFZaminiCount,0) AS LPFZaminiCount, " &
            "	ISNULL(LPFHavayiLen,0) AS LPFHavayiLen, " &
            "	ISNULL(LPFZaminiLen,0) AS LPFZaminiLen, " &
            "	StartDatePersian, " &
            "	EndDatePersian, " &
            "	ISNULL(ServiceHavayiCount,0) AS ServiceHavayiCount, " &
            "	ISNULL(ServiceZaminiCount,0) AS ServiceZaminiCount, " &
            "	ISNULL(ServiceHavayiLen,0) AS ServiceHavayiLen, " &
            "	ISNULL(ServiceZaminiLen,0) AS ServiceZaminiLen " &
            "FROM " &
            "	Tbl_Area " &
            "	INNER JOIN Tbl_LPFeeder ON Tbl_Area.AreaId = Tbl_LPFeeder.AreaId " &
            "	INNER JOIN Tbl_LPPost ON Tbl_LPFeeder.LPPostId = Tbl_LPPost.LPPostId " &
            "	INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "   INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "	INNER JOIN " &
            "	( " &
            "	SELECT  " &
            "		Tbl_LPFeeder.AreaId, " &
            "		COUNT(CASE WHEN ISNULL(Tbl_LPFeeder.HavaeiLength,0) > ISNULL(Tbl_LPFeeder.ZeminiLength,0) THEN Tbl_LPFeeder.LPFeederId END) AS LPFHavayiCount, " &
            "		COUNT(CASE WHEN ISNULL(Tbl_LPFeeder.HavaeiLength,0) <= ISNULL(Tbl_LPFeeder.ZeminiLength,0) THEN Tbl_LPFeeder.LPFeederId END) AS LPFZaminiCount, " &
            "		SUM(ISNULL(Tbl_LPFeeder.HavaeiLength,0)) AS LPFHavayiLen, " &
            "		SUM(ISNULL(Tbl_LPFeeder.ZeminiLength,0)) AS LPFZaminiLen " &
            "	FROM " &
            "		Tbl_LPFeeder " &
            "		INNER JOIN Tbl_LPPost ON Tbl_LPFeeder.LPPostId = Tbl_LPPost.LPPostId " &
            "		INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "       INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "		LEFT JOIN BTblBazdidResult ON Tbl_MPFeeder.MPFeederId = BTblBazdidResult.MPFeederId " &
            "       LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
            "	WHERE " &
            "		Tbl_LPFeeder.IsLightFeeder = " & mIsLightFeeder & mWhere &
            "	GROUP BY  " &
            "		Tbl_LPFeeder.AreaId " &
            "	) AS Tbl_LPFeederLen ON Tbl_Area.AreaId = Tbl_LPFeederLen.AreaId " &
            "	INNER JOIN " &
            "	( " &
            "	SELECT  " &
            "		BTblBazdidResult.AreaId, " &
            "		MIN(BTblServiceCheckList.DoneDatePersian) StartDatePersian, " &
            "		MAX(BTblServiceCheckList.DoneDatePersian) EndDatePersian, " &
            "		COUNT(DISTINCT CASE WHEN ISNULL(Tbl_LPFeeder.HavaeiLength,0) > ISNULL(Tbl_LPFeeder.ZeminiLength,0) THEN Tbl_LPFeeder.LPFeederId END) AS ServiceHavayiCount, " &
            "		COUNT(DISTINCT CASE WHEN ISNULL(Tbl_LPFeeder.HavaeiLength,0) <= ISNULL(Tbl_LPFeeder.ZeminiLength,0) THEN Tbl_LPFeeder.LPFeederId END) AS ServiceZaminiCount, " &
            "		SUM(ISNULL(CASE WHEN ISNULL(BTblBazdidResult.BazdidHavayiLen,0) > 0 THEN BTblBazdidResult.BazdidHavayiLen ELSE ISNULL(BTblBazdidResult.FromToLengthHavayi, Tbl_LPFeeder.HavaeiLength) END ,0)) AS ServiceHavayiLen, " &
            "		SUM(ISNULL(CASE WHEN ISNULL(BTblBazdidResult.BazdidZaminiLen,0) > 0 THEN BTblBazdidResult.BazdidZaminiLen ELSE ISNULL(BTblBazdidResult.FromToLengthZamini, Tbl_LPFeeder.ZeminiLength) END ,0)) AS ServiceZaminiLen " &
            "	FROM  " &
            "		BTblBazdidResult " &
            "		INNER JOIN Tbl_LPFeeder ON BTblBazdidResult.LPFeederId = Tbl_LPFeeder.LPFeederId " &
            "		INNER JOIN Tbl_LPPost ON Tbl_LPFeeder.LPPostId = Tbl_LPPost.LPPostId " &
            "		INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "       INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "		INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
            "		INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
            "		INNER JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId " &
            "       LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                    mJoinSpecialitySql &
            "	WHERE  " &
            "		(BTblBazdidResult.BazdidTypeId = 3) " &
            "		AND BTblServiceCheckList.ServiceStateId = 3 " &
            "		AND (BTblBazdidResult.BazdidStateId IN (2,3))  " &
                    mWhere + mWhereDT &
            "		AND Tbl_LPFeeder.IsLightFeeder = " & mIsLightFeeder &
            "	GROUP BY  " &
            "		BTblBazdidResult.AreaId " &
            "	) AS Tbl_Bazdid ON Tbl_Area.AreaId = Tbl_Bazdid.AreaId"
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_4_34_1", , True)
        If lDS.Tables.Contains("Report_4_34_1") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_4_34_1.xml", XmlWriteMode.WriteSchema)
            mFF.AddFormulaFields("CaptionAll", IIf(mIsLightReport, """ تعداد و طول فيدرهاي روشنايي معابر پيش بيني شده """, """ تعداد و طول فيدرهاي فشار ضعيف  پيش بيني شده """))
            mFF.AddFormulaFields("CaptionBazdid", IIf(mIsLightReport, """ عملکرد فيدرهاي روشنايي معابر سرويس شده """, """ عملکرد فيدرهاي فشار ضعيف سرويس شده """))
            ShowReport("", lFolder & "Report_4_34_1.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_4_34_2()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_4_34_2 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mLPPostId & "," & mLPFeederId & "," & mOwnershipId & "," & mIsActive & "," & IIf(mIsLightReport, 1, 0) & ",'" & mBazdidSpeciality & "'"
        lSQL =
            "SELECT DISTINCT " &
            "	Tbl_LPPost.AreaId, " &
            "	Tbl_Area.Area, " &
            "	HavayiCount, " &
            "	ZaminiCount, " &
            "	StartDate, " &
            "	EndDate, " &
            "	HavayiService, " &
            "	ZaminiService " &
            "FROM " &
            "	Tbl_LPPost " &
            "	INNER JOIN Tbl_Area ON Tbl_LPPost.AreaId = Tbl_Area.AreaId " &
            "	INNER JOIN " &
            "	( " &
            "	SELECT " &
            "		Tbl_LPPost.AreaId, " &
            "		COUNT(CASE WHEN Tbl_LPPost.IsHavayi = 1 THEN Tbl_LPPost.LPPostId END) AS HavayiCount, " &
            "		COUNT(CASE WHEN Tbl_LPPost.IsHavayi = 0 THEN Tbl_LPPost.LPPostId END) AS ZaminiCount " &
            "	FROM " &
            "		Tbl_LPPost " &
            "		INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "       INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
             "	    LEFT JOIN BTblBazdidResult ON Tbl_MPFeeder.MPFeederId = BTblBazdidResult.MPFeederId " &
            "       LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                    mWhere &
            "	GROUP BY " &
            "		Tbl_LPPost.AreaId " &
            "	) AS Tbl_LPPostCount ON Tbl_LPPost.AreaId = Tbl_LPPostCount.AreaId " &
            "	INNER JOIN " &
            "	( " &
            "	SELECT " &
            "		Tbl_LPPost.AreaId, " &
            "		MIN(StartDate) AS StartDate, " &
            "		MAX(EndDate) AS EndDate, " &
            "		COUNT(CASE WHEN Tbl_LPPost.IsHavayi = 1 THEN Tbl_LPPost.LPPostId END) AS HavayiService, " &
            "		COUNT(CASE WHEN Tbl_LPPost.IsHavayi = 0 THEN Tbl_LPPost.LPPostId END) AS ZaminiService " &
            "	FROM " &
            "		Tbl_LPPost " &
            "		INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "       INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "	    LEFT JOIN BTblBazdidResult ON Tbl_MPFeeder.MPFeederId = BTblBazdidResult.MPFeederId " &
            "       LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
            "		INNER JOIN " &
            "		( " &
            "		SELECT DISTINCT " &
            "			Tbl_LPFeeder.LPPostId, " &
            "			MIN(BTblServiceCheckList.DoneDatePersian) AS StartDate, " &
            "			MAX(BTblServiceCheckList.DoneDatePersian) AS EndDate " &
            "		FROM " &
            "			Tbl_LPFeeder " &
            "           INNER JOIN Tbl_LPPost ON Tbl_LPFeeder.LPPostId = Tbl_LPPost.LPPostId " &
            "			INNER JOIN BTblBazdidResult ON Tbl_LPFeeder.LPFeederId = BTblBazdidResult.LPFeederId AND BTblBazdidResult.BazdidTypeId = 3 " &
            "			INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
            "			INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
            "			INNER JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId " &
            "           LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                        mJoinSpecialitySql &
            "		WHERE " &
            "			BTblServiceCheckList.ServiceStateId = 3 " &
            "			AND Tbl_LPFeeder.IsLightFeeder = " & mIsLightFeeder &
                        mWhereDT &
            "		GROUP BY " &
            "			Tbl_LPFeeder.LPPostId " &
            "		) AS Tbl_Service ON Tbl_LPPost.LPPostId = Tbl_Service.LPPostId " &
                    mWhere &
            "	GROUP BY " &
            "		Tbl_LPPost.AreaId " &
            "	) AS Tbl_ServiceCount ON Tbl_LPPost.AreaId = Tbl_ServiceCount.AreaId	"
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_4_34_2", , True)
        If lDS.Tables.Contains("Report_4_34_2") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_4_34_2.xml", XmlWriteMode.WriteSchema)
            mFF.AddFormulaFields("CaptionBazdid", IIf(mIsLightReport, """ روشنايي معابر """, """ فشار ضعيف """))
            ShowReport("", lFolder & "Report_4_34_2.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_4_34_2_Mode2()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_4_34_2_Mode2 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mLPPostId & "," & mLPFeederId & "," & mOwnershipId & "," & mIsActive & "," & IIf(mIsLightReport, 1, 0) & ",'" & mBazdidSpeciality & "'"
        lSQL =
            " SELECT DISTINCT " &
            "	Tbl_Area.AreaId, " &
            "	Tbl_Area.Area, " &
            "	Tbl_LPPostCount.cntPrivate, " &
            "	Tbl_LPPostCount.cntPublic, " &
            "	Tbl_LPPostCount.cntPublicPrivate, " &
            "	Tbl_LPPostCount.cntAll, " &
            "	Tbl_ServiceCount.cntServicePrivate, " &
            "	Tbl_ServiceCount.cntServicePublic, " &
            "	Tbl_ServiceCount.cntServicePrvPub, " &
            "	Tbl_ServiceCount.cntServiceAll, " &
            "	Tbl_ServiceCount.StartDate, " &
            "	Tbl_ServiceCount.EndDate " &
            "FROM " &
            "	Tbl_LPPost " &
            "	INNER JOIN Tbl_Area ON Tbl_LPPost.AreaId = Tbl_Area.AreaId " &
            "	INNER JOIN " &
            "	( " &
            "	SELECT " &
            "		Tbl_LPPost.AreaId, " &
            "		COUNT(CASE WHEN Tbl_LPPost.OwnershipId = 1 THEN Tbl_LPPost.LPPostId END) AS cntPrivate, " &
            "		COUNT(CASE WHEN Tbl_LPPost.OwnershipId = 2 THEN Tbl_LPPost.LPPostId END) AS cntPublic, " &
            "		COUNT(CASE WHEN Tbl_LPPost.OwnershipId = 3 THEN Tbl_LPPost.LPPostId END) AS cntPublicPrivate, " &
            "		COUNT(Tbl_LPPost.LPPostId) AS cntAll " &
            "	FROM " &
            "		Tbl_LPPost " &
            "		INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "       INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            mWhere &
            "	GROUP BY " &
            "		Tbl_LPPost.AreaId " &
            "	) AS Tbl_LPPostCount ON Tbl_LPPost.AreaId = Tbl_LPPostCount.AreaId " &
            "	INNER JOIN " &
            "	( " &
            "	SELECT " &
            "		BTblBazdidResult.AreaId, " &
            "		COUNT (DISTINCT CASE WHEN Tbl_LPPost.OwnershipId = 1 THEN Tbl_LPPost.LPPostId END) AS cntServicePrivate, " &
            "		COUNT (DISTINCT CASE WHEN Tbl_LPPost.OwnershipId = 2 THEN Tbl_LPPost.LPPostId END) AS cntServicePublic, " &
            "		COUNT (DISTINCT CASE WHEN Tbl_LPPost.OwnershipId = 3 THEN Tbl_LPPost.LPPostId END) AS cntServicePrvPub, " &
            "		COUNT (DISTINCT Tbl_LPPost.LPPostId) AS cntServiceAll, " &
            "		MIN(Tbl_Service.StartDate) AS StartDate, " &
            "		MAX(Tbl_Service.EndDate) AS EndDate " &
            "	FROM " &
            "		BTblBazdidResult  " &
            "		INNER JOIN Tbl_LPPost ON BTblBazdidResult.LPPostId = Tbl_LPPost.LPPostId " &
            "		INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "       INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "		INNER JOIN " &
            "		( " &
            "		SELECT DISTINCT " &
            "			Tbl_LPFeeder.LPPostId, " &
            "			MIN(BTblServiceCheckList.DoneDatePersian) AS StartDate, " &
            "			MAX(BTblServiceCheckList.DoneDatePersian) AS EndDate " &
            "		FROM " &
            "			Tbl_LPFeeder " &
            "           INNER JOIN Tbl_LPPost ON Tbl_LPFeeder.LPPostId = Tbl_LPPost.LPPostId " &
            "			INNER JOIN BTblBazdidResult ON Tbl_LPFeeder.LPFeederId = BTblBazdidResult.LPFeederId AND BTblBazdidResult.BazdidTypeId = 3 " &
            "			INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
            "			INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
            "			INNER JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId " &
            "           LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                        mJoinSpecialitySql &
            "		WHERE " &
            "			BTblServiceCheckList.ServiceStateId = 3 " &
            "			AND Tbl_LPFeeder.IsLightFeeder = " & mIsLightFeeder &
                        mWhereDT &
            "		GROUP BY " &
            "			Tbl_LPFeeder.LPPostId " &
            "		) AS Tbl_Service ON Tbl_LPPost.LPPostId = Tbl_Service.LPPostId " &
                    mWhere &
            "	GROUP BY " &
            "		BTblBazdidResult.AreaId " &
            "	) AS Tbl_ServiceCount ON Tbl_LPPost.AreaId = Tbl_ServiceCount.AreaId "
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_4_34_2_Mode2", , True)
        If lDS.Tables.Contains("Report_4_34_2_Mode2") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_4_34_2_Mode2.xml", XmlWriteMode.WriteSchema)
            mFF.AddFormulaFields("FeederType", IIf(mIsLightReport, """ تعداد پست توزيع که فيدرهاي روشنايي معابر آنها سرويس شده است """, """ تعداد پست توزيع که فيدرهاي فشار ضعيف آنها سرويس شده است """))
            ShowReport("", lFolder & "Report_4_34_2_Mode2.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_4_34_2_Mode2_MPFeeder()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_4_34_2_Mode2_MPFeeder " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mLPPostId & "," & mLPFeederId & "," & mOwnershipId & "," & mIsActive & "," & IIf(mIsLightReport, 1, 0) & ",'" & mBazdidSpeciality & "'"
        lSQL =
            "SELECT DISTINCT " &
            "	Tbl_Area.AreaId, " &
            "	Tbl_Area.Area, " &
            "	Tbl_MPPost.MPPostId, " &
            "	Tbl_MPPost.MPPostName, " &
            "	Tbl_MPFeeder.MPFeederId, " &
            "	Tbl_MPFeeder.MPFeederName, " &
            "	Tbl_LPPostCount.cntPrivate, " &
            "	Tbl_LPPostCount.cntPublic, " &
            "	Tbl_LPPostCount.cntPublicPrivate, " &
            "	Tbl_LPPostCount.cntAll, " &
            "	ISNULL(Tbl_ServiceCount.cntServicePrivate,0) as cntServicePrivate, " &
            "	ISNULL(Tbl_ServiceCount.cntServicePublic,0) as cntServicePublic, " &
            "	ISNULL(Tbl_ServiceCount.cntServicePrvPub,0) as cntServicePrvPub, " &
            "	ISNULL(Tbl_ServiceCount.cntServiceAll,0) as cntServiceAll, " &
            "	ISNULL(Tbl_ServiceCount.StartDate,'-') as StartDate, " &
            "	ISNULL(Tbl_ServiceCount.EndDate,'-') as EndDate " &
            "FROM " &
            "	Tbl_LPPost " &
            "	INNER JOIN Tbl_Area ON Tbl_LPPost.AreaId = Tbl_Area.AreaId " &
            "	INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "	INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "	INNER JOIN " &
            "	( " &
            "	SELECT " &
            "		Tbl_LPPost.AreaId, " &
            "		Tbl_LPPost.MPFeederId, " &
            "		COUNT(CASE WHEN Tbl_LPPost.OwnershipId = 1 THEN Tbl_LPPost.LPPostId END) AS cntPrivate, " &
            "		COUNT(CASE WHEN Tbl_LPPost.OwnershipId = 2 THEN Tbl_LPPost.LPPostId END) AS cntPublic, " &
            "		COUNT(CASE WHEN Tbl_LPPost.OwnershipId = 3 THEN Tbl_LPPost.LPPostId END) AS cntPublicPrivate, " &
            "		COUNT(Tbl_LPPost.LPPostId) AS cntAll " &
            "	FROM " &
            "		Tbl_LPPost " &
            "		INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "       INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
                    mWhere &
            "	GROUP BY " &
            "		Tbl_LPPost.AreaId, " &
            "		Tbl_LPPost.MPFeederId " &
            "	) AS Tbl_LPPostCount ON Tbl_LPPost.MPFeederId = Tbl_LPPostCount.MPFeederId AND Tbl_LPPost.AreaId = Tbl_LPPostCount.AreaId " &
            "	LEFT JOIN " &
            "	( " &
            "	SELECT " &
            "		Tbl_LPPost.AreaId, " &
            "		Tbl_LPPost.MPFeederId, " &
            "		COUNT (DISTINCT CASE WHEN Tbl_LPPost.OwnershipId = 1 THEN Tbl_LPPost.LPPostId END) AS cntServicePrivate, " &
            "		COUNT (DISTINCT CASE WHEN Tbl_LPPost.OwnershipId = 2 THEN Tbl_LPPost.LPPostId END) AS cntServicePublic, " &
            "		COUNT (DISTINCT CASE WHEN Tbl_LPPost.OwnershipId = 3 THEN Tbl_LPPost.LPPostId END) AS cntServicePrvPub, " &
            "		COUNT (DISTINCT Tbl_LPPost.LPPostId) AS cntServiceAll, " &
            "		MIN(Tbl_Service.StartDate) AS StartDate, " &
            "		MAX(Tbl_Service.EndDate) AS EndDate " &
            "	FROM " &
            "		BTblBazdidResult  " &
            "		INNER JOIN Tbl_LPPost ON BTblBazdidResult.LPPostId = Tbl_LPPost.LPPostId " &
            "		INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "       INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "		INNER JOIN " &
            "		( " &
            "		SELECT DISTINCT " &
            "			Tbl_LPFeeder.LPPostId, " &
            "			MIN(BTblServiceCheckList.DoneDatePersian) AS StartDate, " &
            "			MAX(BTblServiceCheckList.DoneDatePersian) AS EndDate " &
            "		FROM " &
            "			Tbl_LPFeeder " &
            "           INNER JOIN Tbl_LPPost ON Tbl_LPFeeder.LPPostId = Tbl_LPPost.LPPostId " &
            "			INNER JOIN BTblBazdidResult ON Tbl_LPFeeder.LPFeederId = BTblBazdidResult.LPFeederId AND BTblBazdidResult.BazdidTypeId = 3 " &
            "			INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
            "			INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
            "			INNER JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId " &
            "           LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                        mJoinSpecialitySql &
            "		WHERE " &
            "			BTblServiceCheckList.ServiceStateId = 3 " &
            "			AND Tbl_LPFeeder.IsLightFeeder = " & mIsLightFeeder &
                        mWhereDT &
            "		GROUP BY " &
            "			Tbl_LPFeeder.LPPostId " &
            "		) AS Tbl_Service ON Tbl_LPPost.LPPostId = Tbl_Service.LPPostId " &
                    mWhere &
            "	GROUP BY " &
            "		Tbl_LPPost.AreaId, " &
            "		Tbl_LPPost.MPFeederId " &
            "	) AS Tbl_ServiceCount ON Tbl_LPPost.AreaId = Tbl_ServiceCount.AreaId AND Tbl_LPPost.MPFeederId = Tbl_ServiceCount.MPFeederId	 " &
            "	"
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_4_34_2_Mode2_MPFeeder", , True)
        If lDS.Tables.Contains("Report_4_34_2_Mode2_MPFeeder") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_4_34_2_Mode2_MPFeeder.xml", XmlWriteMode.WriteSchema)
            mFF.AddFormulaFields("FeederType", IIf(mIsLightReport, """ تعداد پست توزيع که فيدرهاي روشنايي معابر آنها سرويس شده است """, """ تعداد پست توزيع که فيدرهاي فشار ضعيف آنها سرويس شده است """))
            ShowReport("", lFolder & "Report_4_34_2_Mode2_MPFeeder.rpt", "", """" & cmbReportName.Text & " به تفکيک فيدر فشار متوسط " & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_4_34_3()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_4_34_3 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mLPPostId & "," & mLPFeederId & "," & mOwnershipId & "," & mIsActive & "," & IIf(mIsLightReport, 1, 0) & ",'" & mPrs & "','" & mCheckLists & "'," & mMinCheckListCount & ",'" & mBazdidSpeciality & "'"
        lSQL =
            "SELECT " &
        "		ISNULL(Tbl_Area_Feeder.AreaId, Tbl_Area_Post.AreaId) AS AreaId, " &
        "		ISNULL(Tbl_Area_Feeder.Area, Tbl_Area_Post.Area) AS Area, " &
        "		MIN(view_StartEndDate.StartDate) AS StartDate, " &
        "		MAX(view_StartEndDate.EndDate) AS EndDate, " &
        "		ISNULL(BTbl_BazdidCheckListSubGroup.BazdidCheckListSubGroupId,111222) AS BazdidCheckListSubGroupId, " &
        "		ISNULL(BTbl_BazdidCheckListSubGroup.BazdidCheckListSubGroupName,'ساير') AS BazdidCheckListSubGroupName, " &
        "		SUM(CASE WHEN BTbl_BazdidCheckListGroup.IsHavayi = 1 AND BTblBazdidResult.BazdidTypeId = 3 THEN CASE WHEN BTblServiceCheckList.ServiceCount Is Null OR BTblServiceCheckList.ServiceCount = 0 THEN 1 ELSE BTblServiceCheckList.ServiceCount END END) AS cntHavayi, " &
        "		SUM(CASE WHEN BTbl_BazdidCheckListGroup.IsHavayi = 0 AND BTblBazdidResult.BazdidTypeId = 3 THEN CASE WHEN BTblServiceCheckList.ServiceCount Is Null OR BTblServiceCheckList.ServiceCount = 0 THEN 1 ELSE BTblServiceCheckList.ServiceCount END END) AS cntZamini, " &
        "		SUM(DISTINCT CASE WHEN Tbl_LPPost.IsHavayi = 1 AND BTblBazdidResult.BazdidTypeId = 2 THEN CASE WHEN BTblServiceCheckList.ServiceCount Is Null OR BTblServiceCheckList.ServiceCount = 0 THEN 1 ELSE BTblServiceCheckList.ServiceCount END END) AS cntHavayiPost, " &
        "		SUM(DISTINCT CASE WHEN Tbl_LPPost.IsHavayi = 0 AND BTblBazdidResult.BazdidTypeId = 2 THEN CASE WHEN BTblServiceCheckList.ServiceCount Is Null OR BTblServiceCheckList.ServiceCount = 0 THEN 1 ELSE BTblServiceCheckList.ServiceCount END END) AS cntZaminiPost, " &
        "		view_StartEndDate.BazdidTypeId " &
        "	FROM  " &
        "		BTblBazdidResult " &
        "		LEFT OUTER JOIN Tbl_LPPost ON BTblBazdidResult.LPPostId = Tbl_LPPost.LPPostId " &
        "		LEFT OUTER JOIN Tbl_MPFeeder AS Tbl_MPFeeder_Post ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder_Post.MPFeederId " &
        "		LEFT OUTER JOIN Tbl_Area AS Tbl_Area_Post ON BTblBazdidResult.AreaId = Tbl_Area_Post.AreaId " &
        "		LEFT OUTER JOIN Tbl_LPFeeder ON BTblBazdidResult.LPFeederId = Tbl_LPFeeder.LPFeederId " &
        "		LEFT OUTER JOIN Tbl_LPPost AS Tbl_LPPost_Feeder ON Tbl_LPFeeder.LPPostId = Tbl_LPPost_Feeder.LPPostId " &
        "		LEFT OUTER JOIN Tbl_MPFeeder ON Tbl_LPPost_Feeder.MPFeederId = Tbl_MPFeeder.MPFeederId " &
        "		LEFT OUTER JOIN Tbl_Area AS Tbl_Area_Feeder ON BTblBazdidResult.AreaId = Tbl_Area_Feeder.AreaId " &
        "		INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
        "		INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
        "		LEFT OUTER JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId " &
        "		LEFT OUTER JOIN BTbl_BazdidCheckList ON BTblBazdidResultCheckList.BazdidCheckListId = BTbl_BazdidCheckList.BazdidCheckListId " &
        "		LEFT OUTER JOIN BTbl_BazdidCheckListSubGroup ON BTbl_BazdidCheckList.BazdidCheckListSubGroupId = BTbl_BazdidCheckListSubGroup.BazdidCheckListSubGroupId " &
        "		LEFT JOIN BTbl_BazdidCheckListGroup ON BTbl_BazdidCheckList.BazdidCheckListGroupId = BTbl_BazdidCheckListGroup.BazdidCheckListGroupId " &
        "       LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                mJoinSpecialitySql &
        "		INNER JOIN " &
        "		( " &
        "		SELECT BTblServiceCheckList.ServiceCheckListId, " &
        "			MIN(BTblServiceCheckList.DoneDatePersian) AS StartDate, " &
        "			MAX(BTblServiceCheckList.DoneDatePersian) AS EndDate, " &
        "			BTblBazdidResult.BazdidTypeId " &
        "		FROM  " &
        "			BTblBazdidResult " &
        "			INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
        "			INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
        "			INNER JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId " &
        "			LEFT JOIN Tbl_LPFeeder ON BTblBazdidResult.LPFeederId = Tbl_LPFeeder.LPFeederId " &
        "			LEFT JOIN Tbl_LPPost ON BTblBazdidResult.LPPostId = Tbl_LPPost.LPPostId " &
        "           LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                    mJoinSpecialitySql &
        "		WHERE  " &
        "			(BTblBazdidResult.BazdidStateId IN (2,3)) " &
        "			AND BTblBazdidResult.BazdidTypeId = 3  " &
        "			AND (BTblServiceCheckList.ServiceStateId = 3 OR (BTblServiceCheckList.ServiceStateId = 2 AND BTblServiceCheckList.ServiceCount > 0))" &
        "           AND Tbl_LPFeeder.IsLightFeeder = " & mIsLightFeeder &
                    mWhereDT &
        "		GROUP BY  " &
        "			BTblServiceCheckList.ServiceCheckListId, " &
        "			BTblBazdidResult.BazdidTypeId " &
        "		) AS view_StartEndDate ON BTblServiceCheckList.ServiceCheckListId = view_StartEndDate.ServiceCheckListId " &
        "	WHERE  " &
        "		(BTblBazdidResult.BazdidStateId IN (2,3)) " &
        "		AND (BTblServiceCheckList.ServiceStateId = 3 OR (BTblServiceCheckList.ServiceStateId = 2 AND BTblServiceCheckList.ServiceCount > 0)) AND Tbl_LPFeeder.IsLightFeeder =  " & mIsLightFeeder &
        "		AND BTblBazdidResultCheckList.Priority > 0 " &
                mWhere &
        "	GROUP BY  " &
        "		ISNULL(Tbl_Area_Feeder.AreaId, Tbl_Area_Post.AreaId), " &
        "		ISNULL(Tbl_Area_Feeder.Area, Tbl_Area_Post.Area), " &
        "		ISNULL(BTbl_BazdidCheckListSubGroup.BazdidCheckListSubGroupId,111222), " &
        "		ISNULL(BTbl_BazdidCheckListSubGroup.BazdidCheckListSubGroupName,'ساير'), " &
        "		view_StartEndDate.BazdidTypeId " &
            mHaving
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_4_34_3", , True)
        If lDS.Tables.Contains("Report_4_34_3") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_4_34_3.xml", XmlWriteMode.WriteSchema)
            mFF.AddFormulaFields("FeederType", IIf(mIsLightReport, """ روشنايي معابر """, """ فشار ضعيف """))
            ShowReport("", lFolder & "Report_4_34_3.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_4_34_4()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_4_34_4 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mLPPostId & "," & mLPFeederId & "," & mOwnershipId & "," & mIsActive & "," & IIf(mIsLightReport, 1, 0) & ",'" & mBazdidSpeciality & "'"
        lSQL =
            "SELECT  " &
            "	Tbl_Area.AreaId, " &
            "	Tbl_Area.Area, " &
            "	COUNT(DISTINCT BTblServiceCheckList.BazdidResultCheckListId) AS cnt, " &
            "	BTblBazdidResultCheckList.Priority, " &
            "	MIN(Tbl_StartEndDate.StartDate) AS StartDate, " &
            "	MAX(Tbl_StartEndDate.EndDate) AS EndDate, " &
            "	Tbl_StartEndDate.IsHavayi " &
            "FROM  " &
            "	Tbl_LPFeeder " &
            "	INNER JOIN Tbl_LPPost ON Tbl_LPFeeder.LPPostId = Tbl_LPPost.LPPostId " &
            "	INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "   INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "	INNER JOIN BTblBazdidResult ON Tbl_LPFeeder.LPFeederId = BTblBazdidResult.LPFeederId " &
            "	INNER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId " &
            "	INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
            "	INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
            "	INNER JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId " &
            "	LEFT OUTER JOIN BTblBazdidResultSubCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblBazdidResultSubCheckList.BazdidResultCheckListId " &
            "	LEFT OUTER JOIN BTbl_SubCheckList AS BTbl_SubCheckList_Old ON BTblBazdidResultCheckList.SubCheckListId = BTbl_SubCheckList_Old.SubCheckListId " &
            "	LEFT OUTER JOIN BTbl_SubCheckList AS BTbl_SubCheckList_New ON BTblBazdidResultSubCheckList.SubCheckListId = BTbl_SubCheckList_New.SubCheckListId " &
            "   LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                mJoinSpecialitySql &
            "	INNER JOIN  " &
            "		( " &
            "			SELECT  " &
            "				BTblBazdidResult.BazdidResultId, " &
            "				MIN(BTblServiceCheckList.DoneDatePersian) AS StartDate, " &
            "				MAX(BTblServiceCheckList.DoneDatePersian) AS EndDate, " &
            "				CASE WHEN (ISNULL(Tbl_LPFeeder.HavaeiLength,0) > ISNULL(Tbl_LPFeeder.ZeminiLength,0)) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS IsHavayi " &
            "			FROM " &
            "				Tbl_LPFeeder " &
            "				INNER JOIN BTblBazdidResult ON Tbl_LPFeeder.LPFeederId = BTblBazdidResult.LPFeederId " &
            "				INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
            "				INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
            "				INNER JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId " &
            "               LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                            mJoinSpecialitySql &
            "			WHERE " &
            "				BTblBazdidResult.BazdidTypeId = 3 " &
            "				AND BTblServiceCheckList.ServiceStateId = 3 " &
            "				AND Tbl_LPFeeder.IsLightFeeder = " & mIsLightFeeder &
                            mWhereDT &
            "			GROUP BY " &
            "				BTblBazdidResult.BazdidResultId, " &
            "				Tbl_LPFeeder.HavaeiLength, " &
            "				Tbl_LPFeeder.ZeminiLength " &
            "		) AS Tbl_StartEndDate ON BTblBazdidResult.BazdidResultId = Tbl_StartEndDate.BazdidResultId " &
            "WHERE " &
            "	BTblServiceCheckList.ServiceStateId = 3 " &
            "	AND BTblBazdidResult.BazdidTypeId = 3  " &
            "	AND ( " &
            "		NOT ISNULL(BTbl_SubCheckList_New.SubCheckListCode,BTbl_SubCheckList_Old.SubCheckListCode) IS NULL " &
            "		OR NOT ISNULL(BTbl_SubCheckList_New.SubCheckListName,BTbl_SubCheckList_Old.SubCheckListName) IS NULL " &
            "		) " &
            "	AND Tbl_LPFeeder.IsLightFeeder = " & mIsLightFeeder &
                mWhere.Replace("BTbl_SubCheckList.SubCheckListId", "ISNULL(BTbl_SubCheckList_New.SubCheckListId,BTbl_SubCheckList_Old.SubCheckListId)") &
            "GROUP BY  " &
            "	Tbl_Area.AreaId, " &
            "	Tbl_Area.Area, " &
            "	BTblBazdidResultCheckList.Priority, " &
            "	Tbl_StartEndDate.IsHavayi "
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_4_34_4", , True)
        If lDS.Tables.Contains("Report_4_34_4") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_4_34_4.xml", XmlWriteMode.WriteSchema)
            mFF.AddFormulaFields("FeederType", IIf(mIsLightReport, """ روشنايي معابر """, """ فشار ضعيف """))
            ShowReport("", lFolder & "Report_4_34_4.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub
    Private Sub MakeReport_4_34_5()
        MakeQuery()
        Dim lSQL As String = ""
        'lSQL = "EXEC spGetReport_4_34_5 " & mFromDate & "," & mToDate & ",'" & mAreaId & "'," & mMPPostId & "," & mMPFeederId & "," & mLPPostId & "," & mLPFeederId & "," & mOwnershipId & "," & mIsActive & "," & IIf(mIsLightReport, 1, 0) & ",'" & mParts & "','" & mBazdidSpeciality & "'"
        lSQL =
            "SELECT  " &
            "	Tbl_Area.AreaId, " &
            "	Tbl_Area.Area, " &
            "	MIN(StartDate) AS StartDate, " &
            "	MAX(EndDate) AS EndDate, " &
            "	cntBazdid, " &
            "	cntService, " &
            "	BTbl_ServicePart.ServicePartCode,  " &
            "	BTbl_ServicePart.ServicePartName,  " &
            "	SUM(BTblServicePartUse.Quantity) AS Quantity, " &
            "	BTbl_ServicePart.PriceOne,  " &
            "	BTbl_ServicePart.ServicePrice,  " &
            "	Tbl_PartUnit.PartUnit " &
            "FROM  " &
            "	BTblBazdidResultAddress " &
            "	INNER JOIN BTblBazdidResult ON BTblBazdidResultAddress.BazdidResultId = BTblBazdidResult.BazdidResultId " &
            "	INNER JOIN BTblServicePartUse ON BTblBazdidResultAddress.BazdidResultAddressId = BTblServicePartUse.BazdidResultAddressId " &
            "	INNER JOIN BTbl_ServicePart ON BTblServicePartUse.ServicePartId = BTbl_ServicePart.ServicePartId " &
            "	LEFT OUTER JOIN Tbl_PartUnit ON BTbl_ServicePart.PartUnitId = Tbl_PartUnit.PartUnitId " &
            "	INNER JOIN Tbl_LPFeeder ON BTblBazdidResult.LPFeederId = Tbl_LPFeeder.LPFeederId " &
            "	INNER JOIN Tbl_LPPost ON Tbl_LPFeeder.LPPostId = Tbl_LPPost.LPPostId " &
            "	INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "   INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "	INNER JOIN Tbl_Area ON BTblBazdidResult.AreaId = Tbl_Area.AreaId " &
            "   LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                mJoinSpecialitySql &
            "	INNER JOIN " &
            "	( " &
            "		SELECT DISTINCT " &
            "			BTblBazdidResultCheckList.BazdidResultAddressId, " &
            "			MIN(BTblServiceCheckList.DoneDatePersian) AS StartDate, " &
            "			MAX(BTblServiceCheckList.DoneDatePersian) AS EndDate " &
            "		FROM " &
            "			BTblBazdidResultCheckList " &
            "			INNER JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId " &
            "			INNER JOIN BTblService ON BTblServiceCheckList.ServiceId = BTblService.ServiceId " &
            "           INNER JOIN BTblBazdidResultAddress ON BTblBazdidResultCheckList.BazdidResultAddressId = BTblBazdidResultAddress.BazdidResultAddressId " &
            "           INNER JOIN BTblBazdidResult ON BTblBazdidResultAddress.BazdidResultId = BTblBazdidResult.BazdidResultId " &
            "           LEFT JOIN Tbl_LPFeeder ON BTblBazdidResult.LPFeederId = Tbl_LPFeeder.LPFeederId " &
                        mWhereDT2 &
            "		GROUP BY " &
            "			BTblBazdidResultCheckList.BazdidResultAddressId " &
            "	) AS Tbl_FilterDate ON BTblBazdidResultAddress.BazdidResultAddressId = Tbl_FilterDate.BazdidResultAddressId " &
            "	LEFT JOIN " &
            "	( " &
            "		SELECT DISTINCT " &
            "			BTblBazdidResult.AreaId, " &
            "			COUNT(DISTINCT CASE WHEN BTblBazdidResult.BazdidStateId IN (2,3) THEN BTblBazdidResultCheckList.BazdidResultCheckListId END) AS cntBazdid, " &
            "			COUNT(DISTINCT CASE WHEN BTblServiceCheckList.ServiceStateId = 3 THEN BTblServiceCheckList.BazdidResultCheckListId END) AS cntService " &
            "		FROM " &
            "			BTblBazdidResult " &
            "			INNER JOIN BTblBazdidResultAddress ON BTblBazdidResult.BazdidResultId = BTblBazdidResultAddress.BazdidResultId " &
            "			INNER JOIN BTblBazdidResultCheckList ON BTblBazdidResultAddress.BazdidResultAddressId = BTblBazdidResultCheckList.BazdidResultAddressId " &
            "			LEFT JOIN BTblServiceCheckList ON BTblBazdidResultCheckList.BazdidResultCheckListId = BTblServiceCheckList.BazdidResultCheckListId " &
            "			LEFT JOIN BTblService ON BTblServiceCheckList.ServiceId = BTblService.ServiceId " &
            "			INNER JOIN Tbl_LPFeeder ON BTblBazdidResult.LPFeederId = Tbl_LPFeeder.LPFeederId " &
            "			INNER JOIN Tbl_LPPost ON Tbl_LPFeeder.LPPostId = Tbl_LPPost.LPPostId " &
            "			INNER JOIN Tbl_MPFeeder ON Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
            "           INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
            "           LEFT JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " &
                        mJoinSpecialitySql &
            "		WHERE " &
            "			BTblBazdidResult.BazdidTypeId = 3 " &
            "			AND Tbl_LPFeeder.IsLightFeeder = " & mIsLightFeeder &
                        mWhereDT & mWhere &
            "		GROUP BY " &
            "			BTblBazdidResult.AreaId " &
            "	) AS Tbl_BazdidService ON Tbl_LPFeeder.AreaId = Tbl_BazdidService.AreaId " &
            "WHERE  " &
            "	BTblBazdidResult.BazdidStateId IN (2,3) " &
            "	AND BTblBazdidResult.BazdidTypeId = 3 " &
            "	AND NOT BTbl_ServicePart.ServicePartId IS NULL " &
            "	AND Tbl_LPFeeder.IsLightFeeder = " & mIsLightFeeder &
                mWhere & mWherePart &
            "GROUP BY " &
            "	Tbl_Area.AreaId, " &
            "	Tbl_Area.Area, " &
            "	cntBazdid, " &
            "	cntService, " &
            "	BTbl_ServicePart.ServicePartCode,  " &
            "	BTbl_ServicePart.ServicePartName,  " &
            "	BTbl_ServicePart.PriceOne,  " &
            "	BTbl_ServicePart.ServicePrice,  " &
            "	Tbl_PartUnit.PartUnit "
        Dim lDS As New DataSet
        BindingTable(lSQL, mCnn, lDS, "Report_4_34_5", , True)
        If lDS.Tables.Contains("Report_4_34_5") Then
            Dim lFolder As String = "Reports\Bazdid\"
            lDS.WriteXml(ReportsXMLPath & "Report_4_34_5.xml", XmlWriteMode.WriteSchema)
            mFF.AddFormulaFields("FeederType", IIf(mIsLightReport, """ روشنايي معابر """, """ فشار ضعيف """))
            ShowReport("", lFolder & "Report_4_34_5.rpt", "", """" & cmbReportName.Text & """", , mFilterInfo, mFF, Strings.StrReverse(mReportNo), 2)
        End If
    End Sub

    Private Sub BasketRowFilter()
        Dim lFilter As String = ""
        If cmbMPFeeder.SelectedIndex > -1 Then
            lFilter = " AND MPFeederId = " & cmbMPFeeder.SelectedValue
        ElseIf cmbMPPost.SelectedIndex > -1 Then
            lFilter = " AND MPPostId = " & cmbMPPost.SelectedValue
        ElseIf cmbArea.SelectedIndex > -1 Or chkArea.GetDataList.Length > 0 Then
            lFilter = " AND AreaId = " & cmbArea.SelectedValue
        End If
        Dim lFeederPartFilter As String = ""
        If txtSearchFeederPart.Text.Trim().Length > 0 Then
            lFeederPartFilter = " AND " & MergeFarsiAndArabi("BasketDetailName", txtSearchFeederPart.Text.Replace("*", "[*]").Replace("%", "[%]"), , "")
        End If

        lFilter = lFilter & lFeederPartFilter
        lFilter = Regex.Replace(lFilter, "^ AND", "")

        mDsBazdid.Tables("View_BasketDetail").DefaultView.RowFilter = lFilter
        ckcmbBasketDetail.Fill(mDsBazdid.Tables("View_BasketDetail").DefaultView, "BasketDetailName", "BazdidBasketDetailId", 15)
    End Sub

    Private Sub BasketRowFilterCheckCombo()
        Dim lFilter As String = ""
        If chkMPFeeder.GetDataList().Length > 0 Then
            lFilter = " AND MPFeederId IN (" & chkMPFeeder.GetDataList() & ") "
        ElseIf chkMPPost.GetDataList().Length > 0 Then
            lFilter = " AND MPPostId IN (" & chkMPPost.GetDataList() & ") "
        ElseIf chkArea.GetDataList().Length > 0 Then
            lFilter = " AND AreaId IN (" & chkArea.GetDataList() & ") "
        End If
        Dim lFeederPartFilter As String = ""
        If txtSearchFeederPart.Text.Trim().Length > 0 Then
            lFeederPartFilter = " AND " & MergeFarsiAndArabi("BasketDetailName", txtSearchFeederPart.Text.Replace("*", "[*]").Replace("%", "[%]"), , "")
        End If

        lFilter = lFilter & lFeederPartFilter
        lFilter = Regex.Replace(lFilter, "^ AND", "")

        mDsBazdid.Tables("View_BasketDetail").DefaultView.RowFilter = lFilter
        ckcmbBasketDetail.Fill(mDsBazdid.Tables("View_BasketDetail").DefaultView, "BasketDetailName", "BazdidBasketDetailId", 15)
    End Sub

    Private Sub GetMPPosts(aAreaIDs As String)

        Dim lSQL As String =
            "SELECT * FROM Tbl_MPPost WHERE MPPostId IN " &
            " ( SELECT MPPostId FROM Tbl_MPPost WHERE AreaId IN ( " & aAreaIDs & " )) " &
            "   OR (MPPostId IN (SELECT DISTINCT MPPostId FROM Tbl_MPFeeder WHERE AreaId IN (" & aAreaIDs & ")))" &
            "   OR (MPPostId IN (SELECT DISTINCT Tbl_MPFeeder.MPPostId FROM Tbl_MPFeeder INNER JOIN Tbl_LPPost ON Tbl_MPFeeder.MPFeederId = Tbl_LPPost.MPFeederId WHERE Tbl_LPPost.AreaId IN (" & aAreaIDs & "  ))) " &
            "   OR (MPPostId IN (SELECT Tbl_MPCommonPost.MPPostId FROM Tbl_MPCommonPost INNER JOIN Tbl_MPPost ON Tbl_MPCommonPost.MPPostId = Tbl_MPPost.MPPostId WHERE Tbl_MPCommonPost.AreaId IN (" & aAreaIDs & "))) " &
            "   OR (MPPostId IN (SELECT Tbl_MPFeeder.MPPostId FROM Tbl_MPCommonFeeder INNER JOIN Tbl_MPFeeder ON Tbl_MPCommonFeeder.MPFeederId = Tbl_MPFeeder.MPFeederId WHERE Tbl_MPCommonFeeder.AreaId IN (" & aAreaIDs & " ))) " &
            "ORDER BY MPPostName "

        BindingTable(lSQL, mCnn, mDs, "Tbl_MPPost", aIsClearTable:=True)
        chkMPPost.Fill(mDs.Tables("Tbl_MPPost"), "MPPostName", "MPPostId", 10)
    End Sub
    Private Sub GetMPFeeders(aAreaIDs As String, aMPPostIDs As String)
        Dim lSQL As String =
            "SELECT *  " &
            "FROM Tbl_MPFeeder  " &
            "WHERE  " &
            "	(  " &
            "		(AreaId IN (" & aAreaIDs & "))  " &
            "		OR ( MPFeederId IN (SELECT MPFeederId FROM Tbl_LPPost WHERE (AreaId IN (" & aAreaIDs & ")))) " &
            "		OR ( MPFeederId IN (SELECT Tbl_MPCommonFeeder.MPFeederId FROM Tbl_MPCommonFeeder INNER JOIN Tbl_MPFeeder ON Tbl_MPCommonFeeder.MPFeederId = Tbl_MPFeeder.MPFeederId WHERE (Tbl_MPCommonFeeder.AreaId IN (" & aAreaIDs & "))) )  " &
            "		OR ( MPFeederId IN (SELECT MPFeederId FROM Tbl_LPPost WHERE " &
            "			(LPPostId IN  " &
            "				(SELECT LPPostId  " &
            "				FROM Tbl_LPFeeder  " &
            "				WHERE AreaId IN (" & aAreaIDs & ")) " &
            "			) " &
            "			OR (LPPostId IN  " &
            "				(SELECT Tbl_LPFeeder.LPPostId  " &
            "				FROM Tbl_LPCommonFeeder INNER JOIN Tbl_LPFeeder ON Tbl_LPCommonFeeder.LPFeederId = Tbl_LPFeeder.LPFeederId  " &
            "				WHERE Tbl_LPCommonFeeder.AreaId IN (" & aAreaIDs & ")) " &
            "			) " &
            "		)) " &
            "	) " &
            "	AND (MPPostId IN (" & aMPPostIDs & ")) " &
            "ORDER by MPFeederName "
        BindingTable(lSQL, mCnn, mDs, "Tbl_MPFeeder", aIsClearTable:=True)
        chkMPFeeder.Fill(mDs.Tables("Tbl_MPFeeder"), "MPFeederName", "MPFeederId", 10)
        cmbMPFeeder.DataSource = mDs.Tables("Tbl_MPFeeder")
        cmbMPFeeder.ValueMember = "MPFeederId"
        cmbMPFeeder.DisplayMember = "MPFeederName"
    End Sub
    Private Sub GetLPPosts(aAreaIDs As String, aMPFeederIDs As String)

        Dim lSQL As String =
            "SELECT * " &
            "FROM Tbl_LPPost " &
            "WHERE  " &
            "	((AreaId IN (" & aAreaIDs & ")) " &
            "	OR (LPPostId IN " &
            "		(SELECT LPPostId " &
            "		FROM Tbl_LPFeeder " &
            "		WHERE AreaId IN (" & aAreaIDs & ")) " &
            "		) " &
            "	OR (LPPostId IN " &
            "		(SELECT Tbl_LPFeeder.LPPostId " &
            "		FROM Tbl_LPCommonFeeder INNER JOIN Tbl_LPFeeder ON Tbl_LPCommonFeeder.LPFeederId = Tbl_LPFeeder.LPFeederId " &
            "		WHERE Tbl_LPCommonFeeder.AreaId IN (" & aAreaIDs & ")) " &
            "		)) " &
            "	AND (MPFeederId IN (" & aMPFeederIDs & ")) " &
            "ORDER BY " &
            "	LPPostName "

        BindingTable(lSQL, mCnn, mDs, "Tbl_LPPost", aIsClearTable:=True)
        chkLPPost.Fill(mDs.Tables("Tbl_LPPost"), "LPPostName", "LPPostId", 10)
        cmbLPPost.DataSource = mDs.Tables("Tbl_LPPost")
        cmbLPPost.ValueMember = "LPPostId"
        cmbLPPost.DisplayMember = "LPPostName"
    End Sub
    Private Sub GetLPFeeders(aAreaIDs As String, aLPPostIDs As String)
        Dim lSQL As String =
            "SELECT * " &
            "FROM Tbl_LPFeeder  " &
            "WHERE  " &
            "	((AreaId IN (" & aAreaIDs & "))" &
            "		OR (LPFeederId IN  " &
            "			(SELECT Tbl_LPCommonFeeder.LPFeederId  " &
            "			FROM Tbl_LPCommonFeeder INNER JOIN Tbl_LPFeeder ON Tbl_LPCommonFeeder.LPFeederId = Tbl_LPFeeder.LPFeederId  " &
            "			WHERE Tbl_LPCommonFeeder.AreaId IN (" & aAreaIDs & "))" &
            "	))  " &
            "	AND (LPPostId IN (" & aLPPostIDs & ")) " &
            "ORDER BY LPFeederName "

        BindingTable(lSQL, mCnn, mDs, "Tbl_LPFeeder", aIsClearTable:=True)
        chkLPFeeder.Fill(mDs.Tables("Tbl_LPFeeder"), "LPFeederName", "LPFeederId", 10)
        cmbLPFeeder.DataSource = mDs.Tables("Tbl_LPFeeder")
    End Sub
    Private Sub GetMPFeederIDs(aAreaIDs As String)
        Dim lSQL As String =
            "SELECT MPFeederId " &
            "FROM Tbl_MPFeeder " &
            "WHERE " &
            "	(  " &
            "		( MPFeederId IN (SELECT MPFeederId FROM Tbl_LPPost WHERE (AreaId IN (" & aAreaIDs & "))))  " &
            "		OR ( MPFeederId IN (SELECT Tbl_MPCommonFeeder.MPFeederId FROM Tbl_MPCommonFeeder INNER JOIN Tbl_MPFeeder ON Tbl_MPCommonFeeder.MPFeederId = Tbl_MPFeeder.MPFeederId WHERE (Tbl_MPCommonFeeder.AreaId IN (" & aAreaIDs & ")))) " &
            "		OR (AreaId IN (" & aAreaIDs & ")) " &
            "	) "
        BindingTable(lSQL, mCnn, mDs, "TmpTbl_MPFeedersId", aIsClearTable:=True)
    End Sub

    Private Sub cmbReportName_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cmbReportName.SelectionChangeCommitted

    End Sub
#End Region

End Class

