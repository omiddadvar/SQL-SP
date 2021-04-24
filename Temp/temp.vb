Imports System.IO
Imports System.Data.SqlClient
Imports Microsoft.VisualBasic.Compatibility
Imports System.Collections.Generic
Public Class frmImportMPPostTransLoad
    Inherits FormBase

    Private mCnn As SqlConnection = New SqlConnection(GetConnection())
    Private mDs As New DataSet
    Private mOfd As New OpenFileDialog
    Private mExcel As CExcelManager = Nothing
    Private mSheet As String
    Private mTbl_MPPostTransLoadExcel As DataTable = Nothing

    Private mTbl_MPPostTransLoad As DataTable = Nothing
    Private mTbl_MPPostTransLoadNew As DataTable = Nothing

    Private mTbl_LPPostLoad As DataTable = Nothing
    Private mTbl_LPPostLoadExcel As DataTable = Nothing

    Private mTbl_LPFeederLoad As DataTable = Nothing
    Private mTbl_LPFeederLoadExcel As DataTable = Nothing
    Private mLPFeederLoadIDs As New ArrayList


    Private mTbl_MPFeederLoad As DataTable = Nothing
    Private mTbl_MPFeederLoadExcel As DataTable = Nothing
    Private mTbl_MPFeederLoadNew As DataTable = Nothing
    Private mTbl_MPFeederLoadExcelNew As DataTable = Nothing

    Private mTbl_MPPostEnergy As DataTable = Nothing
    Private mTbl_MPPostEnergyExcel As DataTable = Nothing

    Private mTbl_MPFeederEnergy As DataTable = Nothing
    Private mTbl_MPFeederEnergyExcel As DataTable = Nothing

    Private mTbl_Subscribers As DataTable = Nothing
    Private mTbl_SubscribersExcel As DataTable = Nothing

    Private mTbl_Subscriber As DataTable = Nothing
    Private mTbl_SubscriberExcel As DataTable = Nothing

    Private mSortedListLoadHour As New SortedList
    Private mSortedListLoad As New SortedList
    Friend WithEvents cmnuChecking As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents OldPattern As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewPattern As System.Windows.Forms.ToolStripMenuItem

    Private mFormType As String
    Private mVersionTypeId As Integer = -1
    Friend WithEvents dgNewLoad As Bargh_Common.JGrid

    Private mSortedArea As New SortedList(Of Integer, CAreaInfo)
    Private mSortedSnesetivity As New SortedList
    Private mSortedSpec As New SortedList
    Private mSortedUseType As New SortedList

    '----------------------<Omid>---------------
    Private mTbl_PostFeeder As DataTable
    Private mTbl_PostFeederExcel As DataTable
    Private excel As ExcelModel
    Private columns As List(Of ExcelCol)
    Private TblFieldsToExcelConverter As Dictionary(Of String, Object)
    Private TblDBColumns As Dictionary(Of String, List(Of String))
    '----------------------</Omid>---------------

    Private mSortedMPPost As New SortedList(Of String, CMPPostInfo)
    Private mSortedMPFeeder As New SortedList(Of String, CMPFeederInfo)
    Private mSortedLPPost As New SortedList(Of String, CLPPostInfo)
    Private mSortedLPFeeder As New SortedList(Of String, CLPFeederInfo)

    Dim mSortedSubscriber As New SortedList(Of String, DataRow)
    Dim mUseSorted As Boolean = False

    Private lArrHour() As String = {"H2", "I2", "J2", "K2", "L2", "M2", "N2", "O2", "P2", "Q2", "R2", "S2", "T2" _
                                          , "U2", "V2", "W2", "X2", "Y2", "Z2", "AA2", "AB2", "AC2", "AD2", "AE2"}
    Private lLoadValue1, lLoadValue2, lLoadValue3, lLoadValue4, lLoadValue5, lLoadValue6, lLoadValue7, lLoadValue8,
               lLoadValue9, lLoadValue10, lLoadValue11, lLoadValue12, lLoadValue13, lLoadValue14, lLoadValue15, lLoadValue16,
               lLoadValue17, lLoadValue18, lLoadValue19, lLoadValue20, lLoadValue21, lLoadValue22, lLoadValue23, lLoadValue24 As String
    Friend WithEvents dgMPFeederNew As Bargh_Common.JGrid
    Friend WithEvents dgSubscriberInfo As Bargh_Common.JGrid
    Friend WithEvents pnlSubsriber As System.Windows.Forms.Panel
    Friend WithEvents rbRamz As System.Windows.Forms.RadioButton
    Friend WithEvents rbCode As System.Windows.Forms.RadioButton
    Friend WithEvents rbFileNo As System.Windows.Forms.RadioButton
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnHelp As System.Windows.Forms.Button
    Friend WithEvents dgLPFeederLoad As Bargh_Common.JGrid
    Friend WithEvents dgPostFeeder As JGrid
    Friend WithEvents dgViewLPPostLoad As Bargh_Common.JGrid
    Enum Version As Integer
        OldVer = 0
        NewVer = 1
    End Enum
#Region " Windows Form Designer generated code "

    Public Sub New(Optional ByVal aFormType As String = "")
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        mFormType = aFormType
        'Add any initialization after the InitializeComponent() call

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
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents pg As System.Windows.Forms.ProgressBar
    Friend WithEvents btnShowTemplate As System.Windows.Forms.Button
    Friend WithEvents btnSelect As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lblCount As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dg As Bargh_Common.JGrid
    Friend WithEvents btnReturn As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents DatasetCcRequester1 As Bargh_DataSets.DatasetCcRequester
    Friend WithEvents dgMPFeeder As Bargh_Common.JGrid
    Friend WithEvents dgMPPostEnergy As Bargh_Common.JGrid
    Friend WithEvents dgMPFeederEnergy As Bargh_Common.JGrid
    Friend WithEvents dgSubscriber As Bargh_Common.JGrid
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmImportMPPostTransLoad))
        Dim dgPostFeeder_DesignTimeLayout As Janus.Windows.GridEX.GridEXLayout = New Janus.Windows.GridEX.GridEXLayout()
        Dim dgLPFeederLoad_DesignTimeLayout As Janus.Windows.GridEX.GridEXLayout = New Janus.Windows.GridEX.GridEXLayout()
        Dim dgSubscriberInfo_DesignTimeLayout As Janus.Windows.GridEX.GridEXLayout = New Janus.Windows.GridEX.GridEXLayout()
        Dim dgSubscriber_DesignTimeLayout As Janus.Windows.GridEX.GridEXLayout = New Janus.Windows.GridEX.GridEXLayout()
        Dim dgMPFeederNew_DesignTimeLayout As Janus.Windows.GridEX.GridEXLayout = New Janus.Windows.GridEX.GridEXLayout()
        Dim dgViewLPPostLoad_DesignTimeLayout As Janus.Windows.GridEX.GridEXLayout = New Janus.Windows.GridEX.GridEXLayout()
        Dim dgNewLoad_DesignTimeLayout As Janus.Windows.GridEX.GridEXLayout = New Janus.Windows.GridEX.GridEXLayout()
        Dim dgMPFeederEnergy_DesignTimeLayout As Janus.Windows.GridEX.GridEXLayout = New Janus.Windows.GridEX.GridEXLayout()
        Dim dgMPPostEnergy_DesignTimeLayout As Janus.Windows.GridEX.GridEXLayout = New Janus.Windows.GridEX.GridEXLayout()
        Dim dgMPFeeder_DesignTimeLayout As Janus.Windows.GridEX.GridEXLayout = New Janus.Windows.GridEX.GridEXLayout()
        Dim dg_DesignTimeLayout As Janus.Windows.GridEX.GridEXLayout = New Janus.Windows.GridEX.GridEXLayout()
        Me.btnShowTemplate = New System.Windows.Forms.Button()
        Me.btnSelect = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.pg = New System.Windows.Forms.ProgressBar()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.dgPostFeeder = New Bargh_Common.JGrid()
        Me.dgLPFeederLoad = New Bargh_Common.JGrid()
        Me.dgSubscriberInfo = New Bargh_Common.JGrid()
        Me.dgSubscriber = New Bargh_Common.JGrid()
        Me.dgMPFeederNew = New Bargh_Common.JGrid()
        Me.dgViewLPPostLoad = New Bargh_Common.JGrid()
        Me.dgNewLoad = New Bargh_Common.JGrid()
        Me.dgMPFeederEnergy = New Bargh_Common.JGrid()
        Me.dgMPPostEnergy = New Bargh_Common.JGrid()
        Me.dgMPFeeder = New Bargh_Common.JGrid()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblCount = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.dg = New Bargh_Common.JGrid()
        Me.DatasetCcRequester1 = New Bargh_DataSets.DatasetCcRequester()
        Me.cmnuChecking = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.OldPattern = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewPattern = New System.Windows.Forms.ToolStripMenuItem()
        Me.pnlSubsriber = New System.Windows.Forms.Panel()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.rbRamz = New System.Windows.Forms.RadioButton()
        Me.rbCode = New System.Windows.Forms.RadioButton()
        Me.rbFileNo = New System.Windows.Forms.RadioButton()
        Me.btnHelp = New System.Windows.Forms.Button()
        Me.btnReturn = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        CType(Me.dgPostFeeder, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgLPFeederLoad, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgSubscriberInfo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgSubscriber, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgMPFeederNew, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgViewLPPostLoad, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgNewLoad, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgMPFeederEnergy, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgMPPostEnergy, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgMPFeeder, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        CType(Me.dg, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DatasetCcRequester1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmnuChecking.SuspendLayout()
        Me.pnlSubsriber.SuspendLayout()
        Me.SuspendLayout()
        '
        'HelpMaker
        '
        Me.HelpMaker.HelpNamespace = "ReportsHelp.chm"
        '
        'btnShowTemplate
        '
        Me.btnShowTemplate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnShowTemplate.Location = New System.Drawing.Point(761, 8)
        Me.btnShowTemplate.Name = "btnShowTemplate"
        Me.btnShowTemplate.Size = New System.Drawing.Size(104, 34)
        Me.btnShowTemplate.TabIndex = 0
        Me.btnShowTemplate.Text = "مشاهده فايل الگو جهت ورود اطلاعات"
        '
        'btnSelect
        '
        Me.btnSelect.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSelect.Location = New System.Drawing.Point(593, 47)
        Me.btnSelect.Name = "btnSelect"
        Me.btnSelect.Size = New System.Drawing.Size(272, 23)
        Me.btnSelect.TabIndex = 0
        Me.btnSelect.Text = "دريافت اطلاعات از فايل اکسل و نمايش آنها در جدول"
        '
        'Label2
        '
        Me.Label2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.Font = New System.Drawing.Font("Mitra", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.DarkGreen
        Me.Label2.Location = New System.Drawing.Point(12, 8)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(741, 35)
        Me.Label2.TabIndex = 33
        Me.Label2.Text = resources.GetString("Label2.Text")
        '
        'pg
        '
        Me.pg.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pg.Location = New System.Drawing.Point(134, 540)
        Me.pg.Name = "pg"
        Me.pg.Size = New System.Drawing.Size(602, 21)
        Me.pg.TabIndex = 32
        Me.pg.Tag = "999"
        Me.pg.Visible = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.dgPostFeeder)
        Me.GroupBox1.Controls.Add(Me.dgLPFeederLoad)
        Me.GroupBox1.Controls.Add(Me.dgSubscriberInfo)
        Me.GroupBox1.Controls.Add(Me.dgSubscriber)
        Me.GroupBox1.Controls.Add(Me.dgMPFeederNew)
        Me.GroupBox1.Controls.Add(Me.dgViewLPPostLoad)
        Me.GroupBox1.Controls.Add(Me.dgNewLoad)
        Me.GroupBox1.Controls.Add(Me.dgMPFeederEnergy)
        Me.GroupBox1.Controls.Add(Me.dgMPPostEnergy)
        Me.GroupBox1.Controls.Add(Me.dgMPFeeder)
        Me.GroupBox1.Controls.Add(Me.Panel1)
        Me.GroupBox1.Controls.Add(Me.dg)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 72)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(857, 459)
        Me.GroupBox1.TabIndex = 34
        Me.GroupBox1.TabStop = False
        '
        'dgPostFeeder
        '
        Me.dgPostFeeder.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgPostFeeder.BuiltInTextsData = resources.GetString("dgPostFeeder.BuiltInTextsData")
        Me.dgPostFeeder.CurrentRowIndex = -1
        dgPostFeeder_DesignTimeLayout.LayoutString = resources.GetString("dgPostFeeder_DesignTimeLayout.LayoutString")
        Me.dgPostFeeder.DesignTimeLayout = dgPostFeeder_DesignTimeLayout
        Me.dgPostFeeder.EnableFormatEvent = True
        Me.dgPostFeeder.EnableSaveLayout = True
        Me.dgPostFeeder.GroupByBoxFormatStyle.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.dgPostFeeder.GroupByBoxInfoFormatStyle.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.dgPostFeeder.GroupByBoxVisible = False
        Me.dgPostFeeder.IsColor = False
        Me.dgPostFeeder.IsColumnContextMenu = True
        Me.dgPostFeeder.IsForMonitoring = False
        Me.dgPostFeeder.Location = New System.Drawing.Point(9, 37)
        Me.dgPostFeeder.Name = "dgPostFeeder"
        Me.dgPostFeeder.PrintLandScape = True
        Me.dgPostFeeder.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.[True]
        Me.dgPostFeeder.SaveGridName = ""
        Me.dgPostFeeder.Size = New System.Drawing.Size(838, 414)
        Me.dgPostFeeder.TabIndex = 42
        Me.dgPostFeeder.Visible = False
        '
        'dgLPFeederLoad
        '
        Me.dgLPFeederLoad.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgLPFeederLoad.BuiltInTextsData = resources.GetString("dgLPFeederLoad.BuiltInTextsData")
        Me.dgLPFeederLoad.CurrentRowIndex = -1
        dgLPFeederLoad_DesignTimeLayout.LayoutString = resources.GetString("dgLPFeederLoad_DesignTimeLayout.LayoutString")
        Me.dgLPFeederLoad.DesignTimeLayout = dgLPFeederLoad_DesignTimeLayout
        Me.dgLPFeederLoad.EnableFormatEvent = True
        Me.dgLPFeederLoad.EnableSaveLayout = True
        Me.dgLPFeederLoad.GroupByBoxFormatStyle.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.dgLPFeederLoad.GroupByBoxInfoFormatStyle.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.dgLPFeederLoad.GroupByBoxVisible = False
        Me.dgLPFeederLoad.IsColor = False
        Me.dgLPFeederLoad.IsColumnContextMenu = True
        Me.dgLPFeederLoad.IsForMonitoring = False
        Me.dgLPFeederLoad.Location = New System.Drawing.Point(11, 37)
        Me.dgLPFeederLoad.Name = "dgLPFeederLoad"
        Me.dgLPFeederLoad.PrintLandScape = True
        Me.dgLPFeederLoad.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.[True]
        Me.dgLPFeederLoad.SaveGridName = ""
        Me.dgLPFeederLoad.Size = New System.Drawing.Size(838, 414)
        Me.dgLPFeederLoad.TabIndex = 41
        Me.dgLPFeederLoad.Visible = False
        '
        'dgSubscriberInfo
        '
        Me.dgSubscriberInfo.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.[False]
        Me.dgSubscriberInfo.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgSubscriberInfo.BuiltInTextsData = resources.GetString("dgSubscriberInfo.BuiltInTextsData")
        Me.dgSubscriberInfo.CurrentRowIndex = -1
        dgSubscriberInfo_DesignTimeLayout.LayoutString = resources.GetString("dgSubscriberInfo_DesignTimeLayout.LayoutString")
        Me.dgSubscriberInfo.DesignTimeLayout = dgSubscriberInfo_DesignTimeLayout
        Me.dgSubscriberInfo.EnableFormatEvent = True
        Me.dgSubscriberInfo.EnableSaveLayout = True
        Me.dgSubscriberInfo.GroupByBoxFormatStyle.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.dgSubscriberInfo.GroupByBoxInfoFormatStyle.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.dgSubscriberInfo.GroupByBoxVisible = False
        Me.dgSubscriberInfo.IsColor = False
        Me.dgSubscriberInfo.IsColumnContextMenu = True
        Me.dgSubscriberInfo.IsForMonitoring = False
        Me.dgSubscriberInfo.Location = New System.Drawing.Point(11, 37)
        Me.dgSubscriberInfo.Name = "dgSubscriberInfo"
        Me.dgSubscriberInfo.PrintLandScape = True
        Me.dgSubscriberInfo.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.[True]
        Me.dgSubscriberInfo.SaveGridName = ""
        Me.dgSubscriberInfo.Size = New System.Drawing.Size(838, 414)
        Me.dgSubscriberInfo.TabIndex = 40
        Me.dgSubscriberInfo.Visible = False
        '
        'dgSubscriber
        '
        Me.dgSubscriber.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgSubscriber.BuiltInTextsData = resources.GetString("dgSubscriber.BuiltInTextsData")
        Me.dgSubscriber.CurrentRowIndex = -1
        dgSubscriber_DesignTimeLayout.LayoutString = resources.GetString("dgSubscriber_DesignTimeLayout.LayoutString")
        Me.dgSubscriber.DesignTimeLayout = dgSubscriber_DesignTimeLayout
        Me.dgSubscriber.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular
        Me.dgSubscriber.EnableFormatEvent = True
        Me.dgSubscriber.EnableSaveLayout = True
        Me.dgSubscriber.GroupByBoxFormatStyle.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.dgSubscriber.GroupByBoxInfoFormatStyle.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.dgSubscriber.GroupByBoxVisible = False
        Me.dgSubscriber.IsColor = False
        Me.dgSubscriber.IsColumnContextMenu = True
        Me.dgSubscriber.IsForMonitoring = False
        Me.dgSubscriber.Location = New System.Drawing.Point(11, 37)
        Me.dgSubscriber.Name = "dgSubscriber"
        Me.dgSubscriber.PrintLandScape = True
        Me.dgSubscriber.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.[True]
        Me.dgSubscriber.SaveGridName = ""
        Me.dgSubscriber.Size = New System.Drawing.Size(838, 414)
        Me.dgSubscriber.TabIndex = 36
        Me.dgSubscriber.Visible = False
        '
        'dgMPFeederNew
        '
        Me.dgMPFeederNew.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgMPFeederNew.BuiltInTextsData = resources.GetString("dgMPFeederNew.BuiltInTextsData")
        Me.dgMPFeederNew.CurrentRowIndex = -1
        dgMPFeederNew_DesignTimeLayout.LayoutString = resources.GetString("dgMPFeederNew_DesignTimeLayout.LayoutString")
        Me.dgMPFeederNew.DesignTimeLayout = dgMPFeederNew_DesignTimeLayout
        Me.dgMPFeederNew.EnableFormatEvent = True
        Me.dgMPFeederNew.EnableSaveLayout = True
        Me.dgMPFeederNew.GroupByBoxFormatStyle.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.dgMPFeederNew.GroupByBoxInfoFormatStyle.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.dgMPFeederNew.GroupByBoxVisible = False
        Me.dgMPFeederNew.IsColor = False
        Me.dgMPFeederNew.IsColumnContextMenu = True
        Me.dgMPFeederNew.IsForMonitoring = False
        Me.dgMPFeederNew.Location = New System.Drawing.Point(8, 37)
        Me.dgMPFeederNew.Name = "dgMPFeederNew"
        Me.dgMPFeederNew.PrintLandScape = True
        Me.dgMPFeederNew.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.[True]
        Me.dgMPFeederNew.SaveGridName = ""
        Me.dgMPFeederNew.Size = New System.Drawing.Size(841, 414)
        Me.dgMPFeederNew.TabIndex = 39
        Me.dgMPFeederNew.Visible = False
        '
        'dgViewLPPostLoad
        '
        Me.dgViewLPPostLoad.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgViewLPPostLoad.BuiltInTextsData = resources.GetString("dgViewLPPostLoad.BuiltInTextsData")
        Me.dgViewLPPostLoad.CurrentRowIndex = -1
        dgViewLPPostLoad_DesignTimeLayout.LayoutString = resources.GetString("dgViewLPPostLoad_DesignTimeLayout.LayoutString")
        Me.dgViewLPPostLoad.DesignTimeLayout = dgViewLPPostLoad_DesignTimeLayout
        Me.dgViewLPPostLoad.EnableFormatEvent = True
        Me.dgViewLPPostLoad.EnableSaveLayout = True
        Me.dgViewLPPostLoad.GroupByBoxFormatStyle.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.dgViewLPPostLoad.GroupByBoxInfoFormatStyle.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.dgViewLPPostLoad.GroupByBoxVisible = False
        Me.dgViewLPPostLoad.IsColor = False
        Me.dgViewLPPostLoad.IsColumnContextMenu = True
        Me.dgViewLPPostLoad.IsForMonitoring = False
        Me.dgViewLPPostLoad.Location = New System.Drawing.Point(8, 37)
        Me.dgViewLPPostLoad.Name = "dgViewLPPostLoad"
        Me.dgViewLPPostLoad.PrintLandScape = True
        Me.dgViewLPPostLoad.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.[True]
        Me.dgViewLPPostLoad.SaveGridName = ""
        Me.dgViewLPPostLoad.Size = New System.Drawing.Size(841, 414)
        Me.dgViewLPPostLoad.TabIndex = 38
        Me.dgViewLPPostLoad.Visible = False
        '
        'dgNewLoad
        '
        Me.dgNewLoad.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgNewLoad.BuiltInTextsData = resources.GetString("dgNewLoad.BuiltInTextsData")
        Me.dgNewLoad.CurrentRowIndex = -1
        dgNewLoad_DesignTimeLayout.LayoutString = resources.GetString("dgNewLoad_DesignTimeLayout.LayoutString")
        Me.dgNewLoad.DesignTimeLayout = dgNewLoad_DesignTimeLayout
        Me.dgNewLoad.EnableFormatEvent = True
        Me.dgNewLoad.EnableSaveLayout = True
        Me.dgNewLoad.GroupByBoxFormatStyle.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.dgNewLoad.GroupByBoxInfoFormatStyle.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.dgNewLoad.GroupByBoxVisible = False
        Me.dgNewLoad.IsColor = False
        Me.dgNewLoad.IsColumnContextMenu = True
        Me.dgNewLoad.IsForMonitoring = False
        Me.dgNewLoad.Location = New System.Drawing.Point(8, 37)
        Me.dgNewLoad.Name = "dgNewLoad"
        Me.dgNewLoad.PrintLandScape = True
        Me.dgNewLoad.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.[True]
        Me.dgNewLoad.SaveGridName = ""
        Me.dgNewLoad.Size = New System.Drawing.Size(841, 414)
        Me.dgNewLoad.TabIndex = 37
        Me.dgNewLoad.Visible = False
        '
        'dgMPFeederEnergy
        '
        Me.dgMPFeederEnergy.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgMPFeederEnergy.BuiltInTextsData = resources.GetString("dgMPFeederEnergy.BuiltInTextsData")
        Me.dgMPFeederEnergy.CurrentRowIndex = -1
        dgMPFeederEnergy_DesignTimeLayout.LayoutString = resources.GetString("dgMPFeederEnergy_DesignTimeLayout.LayoutString")
        Me.dgMPFeederEnergy.DesignTimeLayout = dgMPFeederEnergy_DesignTimeLayout
        Me.dgMPFeederEnergy.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular
        Me.dgMPFeederEnergy.EnableFormatEvent = True
        Me.dgMPFeederEnergy.EnableSaveLayout = True
        Me.dgMPFeederEnergy.GroupByBoxFormatStyle.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.dgMPFeederEnergy.GroupByBoxInfoFormatStyle.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.dgMPFeederEnergy.GroupByBoxVisible = False
        Me.dgMPFeederEnergy.IsColor = False
        Me.dgMPFeederEnergy.IsColumnContextMenu = True
        Me.dgMPFeederEnergy.IsForMonitoring = False
        Me.dgMPFeederEnergy.Location = New System.Drawing.Point(8, 37)
        Me.dgMPFeederEnergy.Name = "dgMPFeederEnergy"
        Me.dgMPFeederEnergy.PrintLandScape = True
        Me.dgMPFeederEnergy.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.[True]
        Me.dgMPFeederEnergy.SaveGridName = ""
        Me.dgMPFeederEnergy.Size = New System.Drawing.Size(841, 414)
        Me.dgMPFeederEnergy.TabIndex = 35
        Me.dgMPFeederEnergy.Visible = False
        '
        'dgMPPostEnergy
        '
        Me.dgMPPostEnergy.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgMPPostEnergy.BuiltInTextsData = resources.GetString("dgMPPostEnergy.BuiltInTextsData")
        Me.dgMPPostEnergy.CurrentRowIndex = -1
        dgMPPostEnergy_DesignTimeLayout.LayoutString = resources.GetString("dgMPPostEnergy_DesignTimeLayout.LayoutString")
        Me.dgMPPostEnergy.DesignTimeLayout = dgMPPostEnergy_DesignTimeLayout
        Me.dgMPPostEnergy.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular
        Me.dgMPPostEnergy.EnableFormatEvent = True
        Me.dgMPPostEnergy.EnableSaveLayout = True
        Me.dgMPPostEnergy.GroupByBoxFormatStyle.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.dgMPPostEnergy.GroupByBoxInfoFormatStyle.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.dgMPPostEnergy.GroupByBoxVisible = False
        Me.dgMPPostEnergy.IsColor = False
        Me.dgMPPostEnergy.IsColumnContextMenu = True
        Me.dgMPPostEnergy.IsForMonitoring = False
        Me.dgMPPostEnergy.Location = New System.Drawing.Point(8, 37)
        Me.dgMPPostEnergy.Name = "dgMPPostEnergy"
        Me.dgMPPostEnergy.PrintLandScape = True
        Me.dgMPPostEnergy.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.[True]
        Me.dgMPPostEnergy.SaveGridName = ""
        Me.dgMPPostEnergy.Size = New System.Drawing.Size(841, 414)
        Me.dgMPPostEnergy.TabIndex = 34
        Me.dgMPPostEnergy.Visible = False
        '
        'dgMPFeeder
        '
        Me.dgMPFeeder.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgMPFeeder.BuiltInTextsData = resources.GetString("dgMPFeeder.BuiltInTextsData")
        Me.dgMPFeeder.CurrentRowIndex = -1
        dgMPFeeder_DesignTimeLayout.LayoutString = resources.GetString("dgMPFeeder_DesignTimeLayout.LayoutString")
        Me.dgMPFeeder.DesignTimeLayout = dgMPFeeder_DesignTimeLayout
        Me.dgMPFeeder.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular
        Me.dgMPFeeder.EnableFormatEvent = True
        Me.dgMPFeeder.EnableSaveLayout = True
        Me.dgMPFeeder.GroupByBoxFormatStyle.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.dgMPFeeder.GroupByBoxInfoFormatStyle.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.dgMPFeeder.GroupByBoxVisible = False
        Me.dgMPFeeder.IsColor = False
        Me.dgMPFeeder.IsColumnContextMenu = True
        Me.dgMPFeeder.IsForMonitoring = False
        Me.dgMPFeeder.Location = New System.Drawing.Point(8, 37)
        Me.dgMPFeeder.Name = "dgMPFeeder"
        Me.dgMPFeeder.PrintLandScape = True
        Me.dgMPFeeder.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.[True]
        Me.dgMPFeeder.SaveGridName = ""
        Me.dgMPFeeder.Size = New System.Drawing.Size(841, 414)
        Me.dgMPFeeder.TabIndex = 33
        Me.dgMPFeeder.Visible = False
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.Color.Maroon
        Me.Panel1.Controls.Add(Me.lblCount)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Location = New System.Drawing.Point(8, 12)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(841, 24)
        Me.Panel1.TabIndex = 32
        Me.Panel1.Tag = "999"
        '
        'lblCount
        '
        Me.lblCount.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCount.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblCount.ForeColor = System.Drawing.Color.White
        Me.lblCount.Location = New System.Drawing.Point(681, 5)
        Me.lblCount.Name = "lblCount"
        Me.lblCount.Size = New System.Drawing.Size(56, 23)
        Me.lblCount.TabIndex = 32
        Me.lblCount.Tag = "999"
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(737, 3)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(96, 23)
        Me.Label1.TabIndex = 31
        Me.Label1.Tag = "999"
        Me.Label1.Text = "تعداد فهرست شده :"
        '
        'dg
        '
        Me.dg.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dg.BuiltInTextsData = resources.GetString("dg.BuiltInTextsData")
        Me.dg.CurrentRowIndex = -1
        dg_DesignTimeLayout.LayoutString = resources.GetString("dg_DesignTimeLayout.LayoutString")
        Me.dg.DesignTimeLayout = dg_DesignTimeLayout
        Me.dg.EnableFormatEvent = True
        Me.dg.EnableSaveLayout = True
        Me.dg.GroupByBoxFormatStyle.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.dg.GroupByBoxInfoFormatStyle.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.dg.GroupByBoxVisible = False
        Me.dg.IsColor = False
        Me.dg.IsColumnContextMenu = True
        Me.dg.IsForMonitoring = False
        Me.dg.Location = New System.Drawing.Point(8, 37)
        Me.dg.Name = "dg"
        Me.dg.PrintLandScape = True
        Me.dg.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.[True]
        Me.dg.SaveGridName = ""
        Me.dg.Size = New System.Drawing.Size(841, 414)
        Me.dg.TabIndex = 2
        '
        'DatasetCcRequester1
        '
        Me.DatasetCcRequester1.DataSetName = "DatasetCcRequester"
        Me.DatasetCcRequester1.Locale = New System.Globalization.CultureInfo("en-US")
        Me.DatasetCcRequester1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'cmnuChecking
        '
        Me.cmnuChecking.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OldPattern, Me.NewPattern})
        Me.cmnuChecking.Name = "cmnuChecking"
        Me.cmnuChecking.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.cmnuChecking.Size = New System.Drawing.Size(128, 52)
        '
        'OldPattern
        '
        Me.OldPattern.Font = New System.Drawing.Font("B Mitra", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.OldPattern.Name = "OldPattern"
        Me.OldPattern.Size = New System.Drawing.Size(127, 24)
        Me.OldPattern.Text = "الگوی قدیم"
        '
        'NewPattern
        '
        Me.NewPattern.Font = New System.Drawing.Font("B Mitra", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.NewPattern.Name = "NewPattern"
        Me.NewPattern.Size = New System.Drawing.Size(127, 24)
        Me.NewPattern.Text = "الگوی جدید"
        '
        'pnlSubsriber
        '
        Me.pnlSubsriber.Controls.Add(Me.Label3)
        Me.pnlSubsriber.Controls.Add(Me.rbRamz)
        Me.pnlSubsriber.Controls.Add(Me.rbCode)
        Me.pnlSubsriber.Controls.Add(Me.rbFileNo)
        Me.pnlSubsriber.Location = New System.Drawing.Point(12, 46)
        Me.pnlSubsriber.Name = "pnlSubsriber"
        Me.pnlSubsriber.Size = New System.Drawing.Size(442, 24)
        Me.pnlSubsriber.TabIndex = 37
        Me.pnlSubsriber.Visible = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label3.Location = New System.Drawing.Point(259, 2)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(131, 18)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "آيتم منحصر به فرد مشترکين :"
        '
        'rbRamz
        '
        Me.rbRamz.AutoSize = True
        Me.rbRamz.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.rbRamz.Location = New System.Drawing.Point(7, 0)
        Me.rbRamz.Name = "rbRamz"
        Me.rbRamz.Size = New System.Drawing.Size(63, 22)
        Me.rbRamz.TabIndex = 0
        Me.rbRamz.TabStop = True
        Me.rbRamz.Text = "رمز رايانه"
        Me.rbRamz.UseVisualStyleBackColor = True
        '
        'rbCode
        '
        Me.rbCode.AutoSize = True
        Me.rbCode.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.rbCode.Location = New System.Drawing.Point(76, 0)
        Me.rbCode.Name = "rbCode"
        Me.rbCode.Size = New System.Drawing.Size(83, 22)
        Me.rbCode.TabIndex = 0
        Me.rbCode.TabStop = True
        Me.rbCode.Text = "شماره اشتراک"
        Me.rbCode.UseVisualStyleBackColor = True
        '
        'rbFileNo
        '
        Me.rbFileNo.AutoSize = True
        Me.rbFileNo.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.rbFileNo.Location = New System.Drawing.Point(165, 0)
        Me.rbFileNo.Name = "rbFileNo"
        Me.rbFileNo.Size = New System.Drawing.Size(78, 22)
        Me.rbFileNo.TabIndex = 0
        Me.rbFileNo.TabStop = True
        Me.rbFileNo.Text = "شماره پرونده"
        Me.rbFileNo.UseVisualStyleBackColor = True
        '
        'btnHelp
        '
        Me.btnHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnHelp.Location = New System.Drawing.Point(460, 47)
        Me.btnHelp.Name = "btnHelp"
        Me.btnHelp.Size = New System.Drawing.Size(127, 23)
        Me.btnHelp.TabIndex = 38
        Me.btnHelp.Text = "راهنماي ورود اطلاعات"
        Me.btnHelp.UseVisualStyleBackColor = True
        Me.btnHelp.Visible = False
        '
        'btnReturn
        '
        Me.btnReturn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnReturn.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnReturn.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnReturn.Image = CType(resources.GetObject("btnReturn.Image"), System.Drawing.Image)
        Me.btnReturn.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnReturn.Location = New System.Drawing.Point(8, 539)
        Me.btnReturn.Name = "btnReturn"
        Me.btnReturn.Size = New System.Drawing.Size(88, 23)
        Me.btnReturn.TabIndex = 36
        Me.btnReturn.Text = "بازگشت"
        Me.btnReturn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Image = CType(resources.GetObject("btnSave.Image"), System.Drawing.Image)
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSave.Location = New System.Drawing.Point(777, 539)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(88, 23)
        Me.btnSave.TabIndex = 35
        Me.btnSave.Text = "ذخيره"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'frmImportMPPostTransLoad
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(873, 569)
        Me.Controls.Add(Me.btnHelp)
        Me.Controls.Add(Me.pnlSubsriber)
        Me.Controls.Add(Me.btnReturn)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.pg)
        Me.Controls.Add(Me.btnShowTemplate)
        Me.Controls.Add(Me.btnSelect)
        Me.Font = New System.Drawing.Font("Tahoma", 8.0!)
        Me.HelpMaker.SetHelpNavigator(Me, System.Windows.Forms.HelpNavigator.Topic)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmImportMPPostTransLoad"
        Me.HelpMaker.SetShowHelp(Me, True)
        Me.Text = "ورود بارگيري ترانس‌هاي پست‌هاي فوق توزيع از فايل اکسل"
        Me.GroupBox1.ResumeLayout(False)
        CType(Me.dgPostFeeder, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgLPFeederLoad, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgSubscriberInfo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgSubscriber, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgMPFeederNew, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgViewLPPostLoad, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgNewLoad, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgMPFeederEnergy, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgMPPostEnergy, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgMPFeeder, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        CType(Me.dg, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DatasetCcRequester1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmnuChecking.ResumeLayout(False)
        Me.pnlSubsriber.ResumeLayout(False)
        Me.pnlSubsriber.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub frmImportMPPostTransLoad_Activated(sender As Object, e As EventArgs) Handles Me.Activated

        If Not mIsFirstActivated Then Exit Sub
        mIsFirstActivated = False
        If mFormType = "SubscriberInfo" Then
            Dim lDs As New DataSet
            BindingTable("select * from Tbl_Area where IsCenter = 0 ", mCnn, lDs, "Tbl_Area", aIsClearTable:=True)
            For Each lRow As DataRow In lDs.Tables("Tbl_Area").Rows
                Dim lCAreaInfo As New CAreaInfo
                lCAreaInfo.Area = lRow("Area")
                lCAreaInfo.CityId = lRow("CityId")
                mSortedArea.Add(lRow("AreaId"), lCAreaInfo)
            Next

            BindingTable("select * from Tbl_SubscriberSensitivity ", mCnn, lDs, "Tbl_SubscriberSensitivity", aIsClearTable:=True)
            For Each lRow As DataRow In lDs.Tables("Tbl_SubscriberSensitivity").Rows
                mSortedSnesetivity.Add(lRow("SubscriberSensitivityId"), lRow("SubscriberSensitivity"))
            Next


            BindingTable("select * from TblSpec where SpecTypeId IN (100,101,102,103,104) ", mCnn, lDs, "TblSpec", aIsClearTable:=True)
            For Each lRow As DataRow In lDs.Tables("TblSpec").Rows
                mSortedSpec.Add(lRow("SpecId"), lRow("SpecValue"))
            Next

            BindingTable("select * from Tbl_UseType", mCnn, lDs, "Tbl_UseType", aIsClearTable:=True)
            For Each lRow As DataRow In lDs.Tables("Tbl_UseType").Rows
                mSortedUseType.Add(lRow("UseTypeId"), lRow("UseType"))
            Next

            Dim lSQL As String = ""
            lSQL = "select  " &
                    "	Tbl_LPFeeder.LPFeederId, Tbl_LPFeeder.LPFeederCode, Tbl_LPFeeder.LPFeederName, " &
                    "	Tbl_LPPost.LPPostId, Tbl_LPPost.LPPostName, " &
                    "	Tbl_MPFeeder.MPFeederId, Tbl_MPFeeder.MPFeederName, " &
                    "	Tbl_MPPost.MPPostId, Tbl_MPPost.MPPostName " &
                    "from Tbl_LPFeeder  " &
                    "inner join Tbl_LPPost on Tbl_LPFeeder.LPPostId = Tbl_LPPost.LPPostId " &
                    "inner join Tbl_MPFeeder on Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
                    "inner join Tbl_MPPost on Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
                    "	where ISNULL(LPFeederCode,'') <> '' "

            BindingTable(lSQL, mCnn, lDs, "Tbl_LPFeeder", aIsClearTable:=True)
            For Each lRow As DataRow In lDs.Tables("Tbl_LPFeeder").Rows
                Dim lCLPFeederInfo As New CLPFeederInfo
                If Not mSortedLPFeeder.ContainsKey(lRow("LPFeederCode")) Then
                    lCLPFeederInfo.LPFeederId = lRow("LPFeederId")
                    lCLPFeederInfo.LPFeederName = lRow("LPFeederName")
                    lCLPFeederInfo.LPPostId = lRow("LPPostId")
                    lCLPFeederInfo.LPPostName = lRow("LPPostName")
                    lCLPFeederInfo.MPFeederId = lRow("MPFeederId")
                    lCLPFeederInfo.MPFeederName = lRow("MPFeederName")
                    lCLPFeederInfo.MPPostId = lRow("MPPostId")
                    lCLPFeederInfo.MPPostName = lRow("MPPostName")
                    mSortedLPFeeder.Add(lRow("LPFeederCode"), lCLPFeederInfo)
                End If
            Next

            lSQL = "select  " &
                    "	Tbl_LPPost.LPPostId, Tbl_LPPost.LPPostCode, Tbl_LPPost.LPPostName, " &
                    "	Tbl_MPFeeder.MPFeederId, Tbl_MPFeeder.MPFeederName, " &
                    "	Tbl_MPPost.MPPostId, Tbl_MPPost.MPPostName " &
                    "from Tbl_LPPost  " &
                    "inner join Tbl_MPFeeder on Tbl_LPPost.MPFeederId = Tbl_MPFeeder.MPFeederId " &
                    "inner join Tbl_MPPost on Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
                    "	where ISNULL(LPPostCode,'') <> '' "

            BindingTable(lSQL, mCnn, lDs, "Tbl_LPPost", aIsClearTable:=True)
            For Each lRow As DataRow In lDs.Tables("Tbl_LPPost").Rows
                Dim lCLPPostInfo As New CLPPostInfo
                If Not mSortedLPPost.ContainsKey(lRow("LPPostCode")) Then
                    lCLPPostInfo.LPPostId = lRow("LPPostId")
                    lCLPPostInfo.LPPostName = lRow("LPPostName")
                    lCLPPostInfo.MPFeederId = lRow("MPFeederId")
                    lCLPPostInfo.MPFeederName = lRow("MPFeederName")
                    lCLPPostInfo.MPPostId = lRow("MPPostId")
                    lCLPPostInfo.MPPostName = lRow("MPPostName")
                    mSortedLPPost.Add(lRow("LPPostCode"), lCLPPostInfo)
                End If
            Next

            lSQL = "select  " &
                    "	Tbl_MPFeeder.MPFeederId, Tbl_MPFeeder.MPFeederCode, Tbl_MPFeeder.MPFeederName, " &
                    "	Tbl_MPPost.MPPostId, Tbl_MPPost.MPPostName " &
                    "from Tbl_MPFeeder  " &
                    "inner join Tbl_MPPost on Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " &
                    "	where ISNULL(MPFeederCode,'') <> '' "

            BindingTable(lSQL, mCnn, lDs, "Tbl_MPFeeder", aIsClearTable:=True)
            For Each lRow As DataRow In lDs.Tables("Tbl_MPFeeder").Rows
                Dim lMPFeederInfo As New CMPFeederInfo
                If Not mSortedMPFeeder.ContainsKey(lRow("MPFeederCode")) Then
                    lMPFeederInfo.MPFeederId = lRow("MPFeederId")
                    lMPFeederInfo.MPFeederName = lRow("MPFeederName")
                    lMPFeederInfo.MPPostId = lRow("MPPostId")
                    lMPFeederInfo.MPPostName = lRow("MPPostName")
                    mSortedMPFeeder.Add(lRow("MPFeederCode"), lMPFeederInfo)
                End If
            Next

            lSQL = "select  " &
                    "	Tbl_MPPost.MPPostId, Tbl_MPPost.MPPostCode, Tbl_MPPost.MPPostName " &
                    "from Tbl_MPPost " &
                    "	where ISNULL(MPPostCode,'') <> '' "

            BindingTable(lSQL, mCnn, lDs, "Tbl_MPPost", aIsClearTable:=True)
            For Each lRow As DataRow In lDs.Tables("Tbl_MPPost").Rows
                Dim lMPPostInfo As New CMPPostInfo
                If Not mSortedMPPost.ContainsKey(lRow("MPPostCode")) Then
                    lMPPostInfo.MPPostId = lRow("MPPostId")
                    lMPPostInfo.MPPostName = lRow("MPPostName")
                    mSortedMPPost.Add(lRow("MPPostCode"), lMPPostInfo)
                End If
            Next

        End If

    End Sub

    Private Sub frmImportMPPostTransLoad_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadData()
    End Sub
    Private Sub btnShowTemplate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShowTemplate.Click
        If mFormType = "" Then
            cmnuChecking.Show(btnShowTemplate, New Point(0, btnShowTemplate.Height))
        ElseIf mFormType = "MPFeederLoad" Then
            cmnuChecking.Show(btnShowTemplate, New Point(0, btnShowTemplate.Height))
        Else
            Try
                '  OldPattern_Click(sender, e)
                Dim lFileName As String = ""
                If mFormType = "" Then
                    If mVersionTypeId = Version.OldVer Then
                        lFileName = "MPPostTransLoad"
                    ElseIf mVersionTypeId = Version.NewVer Then
                        lFileName = "MPPostTransLoadNew.xls"
                    End If
                ElseIf mFormType = "MPFeederLoad" Then
                    If mVersionTypeId = Version.OldVer Then
                        lFileName = "MPFeederLoad"
                    ElseIf mVersionTypeId = Version.NewVer Then
                        lFileName = "MPFeederLoadNew.xls"
                    End If

                ElseIf mFormType = "MPPostEnergy" Then
                    lFileName = "MPPostEnergy"
                ElseIf mFormType = "MPFeederEnergy" Then
                    lFileName = "MPFeederEnergy"
                ElseIf mFormType = "Subscribers" Then
                    lFileName = "SubscribersInfo"
                ElseIf mFormType = "SubscriberInfo" Then
                    ShowInfo("جهت مقداردهي ستونهاي فايل اکسل، حتماً راهنماي ورود اطلاعات را مشاهده نماييد")
                    lFileName = "Subscribers"
                ElseIf mFormType = "ViewLPPostLoad" Then
                    lFileName = "LPPostLoad.xls"
                ElseIf mFormType = "ViewLPFeederLoad" Then
                    lFileName = "LPFeederLoad.xls"
                End If
                If mExcel Is Nothing Then
                    mExcel = New CExcelManager
                Else
                    mExcel.CloseWorkBook()
                End If
                Dim lPath As String = VB6.GetPath & "\Reports\Excel\"
                Dim lDestPath As String = GetUserTempPath() & "Tac\Havades\" & lFileName

                If File.Exists(lDestPath) Then
                    File.SetAttributes(lDestPath, FileAttributes.Normal)
                End If

                System.IO.File.Copy(lPath & lFileName, lDestPath, True)
                mExcel.OpenWorkBook(GetUserTempPath() & "Tac\Havades\" & lFileName)
                mExcel.ShowExcel()
            Catch ex As Exception
                ShowError(ex)
            End Try
        End If
    End Sub
    Function ExcelTypeVersion() As Integer
        Dim lValidFromExcel As String = ""
        Dim lHeadNewVer As String
        Dim lHeadOldVer As String
        Dim lHeadNewVerFinal As String
        If mExcel Is Nothing Then
            mExcel = New CExcelManager
        Else
            mExcel.CloseWorkBook()
        End If
        mExcel.OpenWorkBook(mOfd.FileName)

        If mFormType = "" Then
            mSheet = "بارگيري ترانس‌هاي فوق توزيع"
            lHeadNewVer = "نام بهره بردارنوع پستکد پست توزیعنام پست توزیعنام ترانستاریخ"
            lHeadOldVer = "کدپستفوقتوزيعنامپستفوقتوزيعنامترانستاريخبارگيريساعتبارگيريباراکتيو(Mw)بارراکتيو(Mvar)".Replace("ی", "ي").Replace("ك", "ک")
        ElseIf mFormType = "MPFeederLoad" Then
            lHeadOldVer = "کدفيدرنامفيدرتاريخبارگيريساعتبارگيريباراکتيو(A)بارراکتيو(A)توان(Mwh)".Replace("ی", "ي").Replace("ك", "ک")
            mSheet = "بارگيري فيدرهاي فشار متوسط"
            lHeadNewVer = "کد فیدرنام فیدرتاریخH1H2H3"
            lHeadNewVerFinal = "کدفيدرنامفيدرتاريخH1H2H3H4H5H6H7H8H9H10H11H12H13H14H15H16H17H18H19H20H21H22H23H24مجموع"
        End If
        lValidFromExcel = mExcel.ReadCell(mSheet, "B2")
        lValidFromExcel &= mExcel.ReadCell(mSheet, "C2")
        lValidFromExcel &= mExcel.ReadCell(mSheet, "D2")
        lValidFromExcel &= mExcel.ReadCell(mSheet, "E2")
        lValidFromExcel &= mExcel.ReadCell(mSheet, "F2")
        lValidFromExcel &= mExcel.ReadCell(mSheet, "G2")
        If lValidFromExcel = lHeadNewVer Then
            For Each i As String In lArrHour
                lValidFromExcel &= mExcel.ReadCell(mSheet, i)
            Next
            lValidFromExcel &= mExcel.ReadCell(mSheet, "AF2")
        Else
            lValidFromExcel &= mExcel.ReadCell(mSheet, "H2")
        End If
        lValidFromExcel = lValidFromExcel.Replace(" ", "").Replace("ی", "ي").Replace("ك", "ک")

        If lValidFromExcel = lHeadNewVerFinal Then
            Return Version.NewVer
        ElseIf lValidFromExcel = lHeadOldVer Then
            Return Version.OldVer
        Else
            Return -1
        End If

    End Function
    Private Sub btnSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelect.Click
        Try
            If mFormType = "SubscriberInfo" Then
                If Not rbCode.Checked And Not rbRamz.Checked And Not rbFileNo.Checked Then
                    pnlSubsriber.Focus()
                    ShowError("لطفا آيتم منحصر به فرد را انتخاب نماييد")
                    Exit Sub
                End If
            End If

            If (mOfd.ShowDialog() = DialogResult.OK) Then
                mVersionTypeId = ExcelTypeVersion()
                Select Case mFormType
                    Case ""
                        If mVersionTypeId = Version.OldVer Then
                            mTbl_MPPostTransLoad.Rows.Clear()
                        ElseIf mVersionTypeId = Version.NewVer Then
                            mTbl_MPPostTransLoadNew.Rows.Clear()
                        End If
                    Case "PostFeeder"
                        mTbl_PostFeederExcel.Rows.Clear()
                        mTbl_PostFeederExcel.Columns.Clear()
                    Case "MPFeederLoad"
                        If mVersionTypeId = Version.OldVer Then
                            mTbl_MPFeederLoad.Rows.Clear()
                        ElseIf mVersionTypeId = Version.NewVer Then
                            mTbl_MPFeederLoadNew.Rows.Clear()
                        End If
                    Case "MPPostEnergy"
                        mTbl_MPPostEnergy.Rows.Clear()
                    Case "MPFeederEnergy"
                        mTbl_MPFeederEnergy.Rows.Clear()
                    Case "Subscribers"
                        mTbl_Subscribers.Rows.Clear()
                    Case "SubscriberInfo"
                        mTbl_Subscriber.Rows.Clear()
                    Case "ViewLPPostLoad"
                        mTbl_LPPostLoad.Rows.Clear()
                    Case "ViewLPFeederLoad"
                        mTbl_LPFeederLoad.Rows.Clear()
                End Select
                If ImportData() Then
                    Me.Cursor = Cursors.WaitCursor
                    RunOperation()
                    Me.Cursor = Cursors.Default
                End If
            End If
        Catch ex As Exception
            If ex.ToString().ToLower().Contains("another process") Then
                ShowError("فايل اکسل مورد نظر را بسته و مجدداً آن را انتخاب نماييد")
            Else
                ShowError(ex)
            End If
        End Try
    End Sub
    Private Sub frmImportMPPostTransLoad_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed
        If Not mExcel Is Nothing Then
            mExcel.CloseApp()
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If mFormType = "" Then
            If mVersionTypeId = Version.NewVer Then
                If dgNewLoad.RowCount = 0 Then Exit Sub
                SaveInfoNew()
            Else
                If dg.RowCount = 0 Then Exit Sub
                SaveInfo()
            End If
        End If
        If mFormType = "MPFeederLoad" Then
            If dgMPFeederNew.RowCount > 0 And mVersionTypeId = Version.NewVer Then
                SaveMPFeederLoadInfoNew()
            ElseIf dgMPFeeder.RowCount > 0 Then
                SaveMPFeederLoadInfo()
            Else
                Exit Sub
            End If
        End If
        If mFormType = "MPPostEnergy" Then
            If dgMPPostEnergy.RowCount = 0 Then Exit Sub
            SaveMPPostEnergyInfo()
        End If
        If mFormType = "MPFeederEnergy" Then
            If dgMPFeederEnergy.RowCount = 0 Then Exit Sub
            SaveMPFeederEnergyInfo()
        End If
        If mFormType = "Subscribers" Then
            If dgSubscriber.RowCount = 0 Then Exit Sub
            SaveSubscribersInfo()
        End If
        If mFormType = "SubscriberInfo" Then
            If dgSubscriberInfo.RowCount = 0 Then Exit Sub
            SaveSubscriberInfo()
        End If
        If mFormType = "ViewLPPostLoad" Then
            If dgViewLPPostLoad.RowCount = 0 Then Exit Sub
            SaveLPPostLoadInfo()
        ElseIf mFormType = "ViewLPFeederLoad" Then
            If dgLPFeederLoad.RowCount = 0 Then Exit Sub
            SaveLPFeederLoadInfo()
        End If
    End Sub

    Private Sub LoadData()
        Dim lSql As String = ""
        Select Case mFormType
            Case ""
                lSql =
                "SELECT Tbl_MPPost.MPPostId, Tbl_MPPost.MPPostCode, Tbl_MPPost.MPPostName, Tbl_MPPostTrans.MPPostTransId, Tbl_MPPostTrans.MPPostTrans " &
                "FROM Tbl_MPPost INNER JOIN Tbl_MPPostTrans ON Tbl_MPPost.MPPostId = Tbl_MPPostTrans.MPPostId "
                BindingTable(lSql, mCnn, mDs, "Tbl_MPPostTrans", , , , , , , True)
                BindingTable("SELECT *, CAST(MPPostTransLoadId AS Bigint) as MPPostTransLoadOldId FROM TblMPPostTransLoad", mCnn, DatasetCcRequester1, "TblMPPostTransLoad", , , , , , , True)
                BindingTable("SELECT * FROM TblMPPostTransLoadHours", mCnn, DatasetCcRequester1, "TblMPPostTransLoadHours", , , , , , , True)
                BindingTable("SELECT * FROM Tbl_Hour", mCnn, DatasetCcRequester1, "Tbl_Hour", , , , , , , True)
                MakeTableMPPostTransLoad()
                MakeTableMPPostTransLoadNewVersion()
                dgNewLoad.Visible = False
            Case "PostFeeder"
                Me.Text = "ورود بارگيري پست های توزیع و فيدرهاي فشار متوسط از فايل اکسل"
                mSheet = "بارگیری پست و فیدر"
                dg.Visible = False
                dgNewLoad.Visible = False
                dgPostFeeder.Visible = True
                initializeExcel()
            Case "MPFeederLoad"
                Me.Text = "ورود بارگيري فيدرهاي فشار متوسط از فايل اکسل"
                dg.Visible = False
                dgNewLoad.Visible = False
                dgMPFeederNew.Visible = False
                dgMPFeeder.Visible = True
                BindingTable("SELECT * FROM Tbl_MPFeeder WHERE NOT MPFeederCode IS NULL", mCnn, mDs, "Tbl_MPFeeder")
                BindingTable("SELECT * FROM Tbl_Hour", mCnn, mDs, "Tbl_Hour", , , , , , , True)
                MakeTableMPFeederLoad()
                MakeTableMPFeederLoadNew()
                mSheet = "بارگيري فيدرهاي فشار متوسط"
            Case "MPPostEnergy"
                Me.Text = "ورود انرژي تحويلي ماهيانه پست‌هاي فوق توزيع از فايل اکسل"
                dg.Visible = False
                dgNewLoad.Visible = False
                dgMPPostEnergy.Visible = True
                mSheet = "انرژي تحويلي پستهاي فوق توزيع"
                lSql = "SELECT TblMPPostMonthly.MPPostMonthlyId, TblMPPostMonthly.MPPostId, Tbl_MPPost.MPPostCode,Tbl_MPPost.MPPostName, " &
                        "		TblMPPostMonthly.YearMonth, TblMPPostMonthly.Energy " &
                        "FROM	TblMPPostMonthly INNER JOIN Tbl_MPPost ON TblMPPostMonthly.MPPostId = Tbl_MPPost.MPPostId "
                BindingTable(lSql, mCnn, mDs, "TblMPPostMonthly", , , , , , , True)
                BindingTable("SELECT * FROM Tbl_MPPost WHERE NOT MPPostCode IS NULL", mCnn, mDs, "Tbl_MPPost", , , , , , , True)
                MakeTableMPPostEnergy()
            Case "MPFeederEnergy"
                Me.Text = "ورود انرژي تحويلي ماهيانه فيدرهاي فشار متوسط از فايل اکسل"
                dg.Visible = False
                dgMPFeederEnergy.Visible = True
                mSheet = "انرژي تحويلي فيدرهاي فشار متوسط"
                lSql = "SELECT TblMPFeederPeak.*, Tbl_MPFeeder.MPFeederCode,Tbl_MPFeeder.MPFeederName " &
                        "FROM	TblMPFeederPeak INNER JOIN Tbl_MPFeeder ON TblMPFeederPeak.MPFeederId = Tbl_MPFeeder.MPFeederId "
                BindingTable(lSql, mCnn, mDs, "TblMPFeederPeak", , , , , , , True)
                BindingTable("SELECT * FROM Tbl_MPFeeder WHERE NOT MPFeederCode IS NULL", mCnn, mDs, "Tbl_MPFeeder", , , , , , , True)
                MakeTableMPFeederEnergy()
            Case "Subscribers"
                Me.Text = "ورود اطلاعات تعداد مشترکين از فايل اکسل"
                dg.Visible = False
                dgSubscriber.Visible = True
                BindingTable("SELECT AreaId, MAX(YearMonth) AS YearMonth FROM TblSubscribers GROUP BY AreaId ", mCnn, mDs, "Tbl_LastUpdateSubscribers", , , , , , , True)
                BindingTable("SELECT * FROM TblSubscribers", mCnn, mDs, "TblSubscribers", , , , , , , True)
                BindingTable("SELECT * FROM Tbl_Area ", mCnn, mDs, "TblArea", , , , , , , True)
                Dim lAreaId As String = WorkingAreaId
                BindingTable("SELECT * FROM Tbl_Area WHERE AreaId = " & lAreaId, mCnn, mDs, "Tbl_Area", , , , , , , True)
                If mDs.Tables("Tbl_Area").Rows(0)("IsSetad") Then
                    lAreaId = ""
                ElseIf mDs.Tables("Tbl_Area").Rows(0)("IsCenter") Then
                    BindingTable("SELECT * FROM Tbl_Area WHERE IsCenter = 0 AND Server121Id = " & mDs.Tables("Tbl_Area").Rows(0)("Server121Id"), mCnn, mDs, "Tbl_SubArea", , , , , , , True)
                    lAreaId = ""
                    For Each lRow As DataRow In mDs.Tables("Tbl_SubArea").Rows
                        lAreaId &= IIf(lAreaId <> "", "," & lRow("AreaId"), lRow("AreaId"))
                    Next
                Else
                    lAreaId = WorkingAreaId
                End If
                BindingTable("SELECT * FROM Tbl_Area WHERE IsCenter = 0 " & IIf(lAreaId <> "", " AND AreaId IN (" & lAreaId & ")", ""), mCnn, mDs, "Tbl_Area", , , , , , , True)
                mSheet = "تعداد مشترکين"
                MakeTableSubscribers()
            Case "ViewLPPostLoad"
                Me.Text = "ورود بارگيري پستهاي توزيع از فايل اکسل"
                lSql =
                "select Tbl_LPPost.LPPostId,LPPostCode,LPPostName,LoadDateTimePersian,LoadTime,LoadDT,ISNULL(SCurrent,0) AS SCurrent,ISNULL(RCurrent,0) AS RCurrent,ISNULL(TCurrent,0) AS TCurrent," &
                " ISNULL(NolCurrent,0) AS NolCurrent , ISNULL(KelidCurrent,0) AS KelidCurrent,ISNULL(tbllppostload.PostCapacity,Tbl_LPPost.PostCapacity) AS PostCapacity, TblLPPostLoad.LPPostLoadId " &
                " from dbo.Tbl_LPPost" &
                " inner join tbllppostload on Tbl_LPPost.LPPostId = TblLPPostLoad.LPPostId "
                BindingTable(lSql, mCnn, mDs, "ViewLPPostLoad_", , , , , , , True)
                dgViewLPPostLoad.Visible = True
                MakeTableLPPostLoad()
                mSheet = "بارگيري پست توزيع"
            Case "ViewLPFeederLoad"
                Me.Text = "ورود بارگيري فيدرهاي فشار ضعيف از فايل اکسل"
                lSql =
                    "SELECT  " &
                    "	Tbl_LPFeeder.LPFeederId, " &
                    "	Tbl_LPFeeder.LPFeederCode, " &
                    "	Tbl_LPFeeder.LPPostId, " &
                    "	LPFeederName, " &
                    "	LoadDateTimePersian, " &
                    "	LoadTime, " &
                    "	LoadDT, " &
                    "	TblLPFeederLoad.RFuseId, " &
                    "	Tbl_RFuse.Fuse AS RFuse, " &
                    "	Tbl_SFuse.Fuse AS SFuse, " &
                    "	Tbl_TFuse.Fuse AS TFuse, " &
                    "	TblLPFeederLoad.SFuseId, " &
                    "	TblLPFeederLoad.TFuseId, " &
                    "	ISNULL(SCurrent, 0) AS SCurrent, " &
                    "	ISNULL(RCurrent, 0) AS RCurrent, " &
                    "	ISNULL(TCurrent, 0) AS TCurrent, " &
                    "	ISNULL(NolCurrent, 0) AS NolCurrent, " &
                    "	ISNULL(KelidCurrent, 0) AS KelidCurrent, " &
                    "	TblLPFeederLoad.LPPostLoadId, " &
                    "	TblLPFeederLoad.LPFeederLoadId, " &
                    "	TblLPFeederLoad.FazSatheMaghtaId, " &
                    "	Tbl_FazSatheMaghta.SatheMaghta, " &
                    "	CountFazSatheMaghta, " &
                    "	TblLPFeederLoad.NolSatheMaghtaId, " &
                    "	Tbl_NolSatheMaghta.SatheMaghta, " &
                    "	CountNolSatheMaghta, " &
                    "	AreaUserId, " &
                    "	EarthValue " &
                    "FROM  " &
                    "	dbo.Tbl_LPFeeder " &
                    "	INNER JOIN TblLPFeederLoad ON Tbl_LPFeeder.LPFeederId = TblLPFeederLoad.LPFeederId " &
                    "	LEFT JOIN Tbl_Fuse Tbl_RFuse ON TblLPFeederLoad.RFuseId = Tbl_RFuse.FuseId " &
                    "	LEFT JOIN Tbl_Fuse Tbl_SFuse ON TblLPFeederLoad.SFuseId = Tbl_SFuse.FuseId " &
                    "	LEFT JOIN Tbl_Fuse Tbl_TFuse ON TblLPFeederLoad.TFuseId = Tbl_TFuse.FuseId " &
                    "	LEFT JOIN Tbl_SatheMaghta Tbl_FazSatheMaghta ON TblLPFeederLoad.FazSatheMaghtaId = Tbl_FazSatheMaghta.SatheMaghtaId " &
                    "	LEFT JOIN Tbl_SatheMaghta Tbl_NolSatheMaghta ON TblLPFeederLoad.NolSatheMaghtaId = Tbl_NolSatheMaghta.SatheMaghtaId " &
                    "	INNER JOIN  " &
                    "	( " &
                    "		SELECT  " &
                    "		  ROW_NUMBER() OVER(PARTITION BY LPFeederId ORDER BY LoadDT DESC)  " &
                    "			AS RowId, " &
                    "		  LPFeederId, LPFeederLoadId " &
                    "		FROM TblLPFeederLoad  " &
                    "	) AS tLastLoad ON TblLPFeederLoad.LPFeederLoadId = tLastLoad.LPFeederLoadId " &
                    "WHERE " &
                    "	tLastLoad.RowId = 1 AND ISNULL(Tbl_LPFeeder.LPFeederCode,'') <> '' "

                BindingTable(lSql, mCnn, mDs, "ViewLPFeederLastLoad", , , , , , , True)
                lSql =
                    "SELECT  " &
                    "	Tbl_LPPost.*, " &
                    "	TblLPPostLoad.* " &
                    "FROM dbo.Tbl_LPPost " &
                    "INNER JOIN TblLPPostLoad ON Tbl_LPPost.LPPostId = TblLPPostLoad.LPPostId " &
                    "INNER JOIN ( " &
                    "	SELECT ROW_NUMBER() OVER ( " &
                    "			PARTITION BY LPPostId ORDER BY LoadDT DESC " &
                    "			) AS RowId, " &
                    "		LPPostId, " &
                    "		LPPostLoadId " &
                    "	FROM TblLPPostLoad " &
                    "	) AS tLastLoad ON TblLPPostLoad.LPPostLoadId = tLastLoad.LPPostLoadId " &
                    "WHERE tLastLoad.RowId = 1 "

                BindingTable(lSql, mCnn, mDs, "ViewLPPostLastLoad", , , , , , , True)

                dgLPFeederLoad.Visible = True
                MakeTableLPFeederLoad()
                mSheet = "بارگيري فيدر فشار ضعيف"
            Case "SubscriberInfo"
                lSql =
                    "SELECT top 0 " &
                    "	Tbl_Subscriber.SubscriberId, " &
                    "	FileNo, " &
                    "	Code, " &
                    "	CounterNo, " &
                    "	RamzCode, " &
                    "	NAME, " &
                    "	Telephone, " &
                    "	TelMobile, " &
                    "	TelFax, " &
                    "	Tbl_Subscriber.AreaId, " &
                    "	Tbl_Subscriber.CityId, " &
                    "	Area, " &
                    "	Tbl_Subscriber.Address, " &
                    "	PostalCode, " &
                    "	Tbl_Subscriber.GPSx, " &
                    "	Tbl_Subscriber.GPSy, " &
                    "	EmailAddress, " &
                    "	Tbl_Subscriber.MPPostId, " &
                    "	MPPostName, " &
                    "	Tbl_Subscriber.MPFeederId, " &
                    "	MPFeederName, " &
                    "	Tbl_Subscriber.LPPostId, " &
                    "	LPPostName, " &
                    "	Tbl_Subscriber.LPFeederId, " &
                    "	LPFeederName, " &
                    "	NationalCode, " &
                    "	TarefeCode, " &
                    "	Tbl_Subscriber.SubscriberSensitivityId, " &
                    "	SubscriberSensitivity, " &
                    "	Tbl_Subscriber.spcSubscriberVoltageId, " &
                    "	Tbl_SubscriberVoltage.SpecValue AS SubscriberVoltage, " &
                    "	Tbl_Subscriber.spcSubscriberTypeId, " &
                    "	Tbl_SubscriberType.SpecValue AS SubscriberType, " &
                    "	Tbl_Subscriber.spcSubscriberPhaseTypeId, " &
                    "	Tbl_SubscriberPhaseType.SpecValue AS SubscriberPhaseType, " &
                    "	ACntrZarib, " &
                    "	Tbl_Subscriber.spcACntrTypeId, " &
                    "	Tbl_ACntrType.SpecValue AS ACntrType, " &
                    "	RCntrZarib, " &
                    "	Tbl_Subscriber.spcRCntrTypeId, " &
                    "	Tbl_RCntrType.SpecValue AS RCntrType, " &
                    "	AFabrikNumber, " &
                    "	RFabrikNumber, " &
                    "	AInstallDate, " &
                    "	RInstallDate, " &
                    "	POWER, " &
                    "	Amper, " &
                    "	Tbl_SubscriberInfo.spcSubscriberStateId, " &
                    "	Tbl_SubscriberState.SpecValue AS SubscriberState, " &
                    "	Tbl_Subscriber.UseTypeId, " &
                    "	Tbl_UseType.UseType, " &
                    "	DebtPrice, " &
                    "	DebtType, " &
                    "	CanOutgoingCall, " &
                    "   CAST(0 AS bit) AS IsUpdate, " &
                    "   CAST(0 AS bit) AS IsInsert, " &
                    "   CAST(NULL AS nvarchar(20)) AS ChangeType " &
                    "FROM  " &
                    "	Tbl_Subscriber " &
                    "	LEFT JOIN Tbl_SubscriberInfo ON Tbl_Subscriber.SubscriberId = Tbl_SubscriberInfo.SubscriberId " &
                    "	LEFT JOIN Tbl_Area ON Tbl_Subscriber.AreaId = Tbl_Area.AreaId " &
                    "	LEFT JOIN Tbl_MPPost ON Tbl_Subscriber.MPPostId = Tbl_MPPost.MPPostId " &
                    "	LEFT JOIN Tbl_MPFeeder ON Tbl_Subscriber.MPFeederId = Tbl_MPFeeder.MPFeederId " &
                    "	LEFT JOIN Tbl_LPPost ON Tbl_Subscriber.LPPostId = Tbl_LPPost.LPPostId " &
                    "	LEFT JOIN Tbl_LPFeeder ON Tbl_Subscriber.LPFeederId = Tbl_LPFeeder.LPFeederId " &
                    "	LEFT JOIN Tbl_SubscriberSensitivity ON Tbl_Subscriber.SubscriberSensitivityId = Tbl_SubscriberSensitivity.SubscriberSensitivityId " &
                    "	LEFT JOIN TblSpec Tbl_SubscriberVoltage ON Tbl_Subscriber.spcSubscriberVoltageId = Tbl_SubscriberVoltage.SpecId " &
                    "	LEFT JOIN TblSpec Tbl_SubscriberType ON Tbl_Subscriber.spcSubscriberTypeId = Tbl_SubscriberType.SpecId " &
                    "	LEFT JOIN TblSpec Tbl_SubscriberPhaseType ON Tbl_Subscriber.spcSubscriberPhaseTypeId = Tbl_SubscriberPhaseType.SpecId " &
                    "	LEFT JOIN TblSpec Tbl_ACntrType ON Tbl_Subscriber.spcACntrTypeId = Tbl_ACntrType.SpecId " &
                    "	LEFT JOIN TblSpec Tbl_RCntrType ON Tbl_Subscriber.spcRCntrTypeId = Tbl_RCntrType.SpecId " &
                    "	LEFT JOIN TblSpec Tbl_SubscriberState ON Tbl_SubscriberInfo.spcSubscriberStateId = Tbl_SubscriberState.SpecId " &
                    "	LEFT JOIN Tbl_UseType ON Tbl_Subscriber.UseTypeId = Tbl_UseType.UseTypeId "

                BindingTable(lSql, mCnn, mDs, "_Tbl_Subscriber", aIsClearTable:=True)

                lSql =
                        "SELECT top 0 " &
                        "	Tbl_Subscriber.SubscriberId, " &
                        "	FileNo, " &
                        "	Code, " &
                        "	CounterNo, " &
                        "	RamzCode, " &
                        "	NAME, " &
                        "	Telephone, " &
                        "	TelMobile, " &
                        "	TelFax, " &
                        "	Tbl_Subscriber.AreaId, " &
                        "   Area, " &
                        "	Tbl_Subscriber.Address, " &
                        "	PostalCode, " &
                        "	Tbl_Subscriber.GPSx, " &
                        "	Tbl_Subscriber.GPSy, " &
                        "	EmailAddress, " &
                        "	CAST(NULL AS int) AS MPPostId, " &
                        "	CAST(NULL AS int) AS MPFeederId, " &
                        "	CAST(NULL AS int) AS LPPostId, " &
                        "	CAST(NULL AS int) AS LPFeederId, " &
                        "	CAST(NULL AS nvarchar(100)) AS MPPostCode, " &
                        "	CAST(NULL AS nvarchar(100)) AS MPFeederCode, " &
                        "	CAST(NULL AS nvarchar(100)) AS LPPostCode, " &
                        "	CAST(NULL AS nvarchar(100)) AS LPFeederCode, " &
                        "	CAST(NULL AS nvarchar(250)) AS MPPostName, " &
                        "	CAST(NULL AS nvarchar(250)) AS MPFeederName, " &
                        "	CAST(NULL AS nvarchar(250)) AS LPPostName, " &
                        "	CAST(NULL AS nvarchar(250)) AS LPFeederName, " &
                        "	NationalCode, " &
                        "	TarefeCode, " &
                        "	Tbl_Subscriber.SubscriberSensitivityId, " &
                        "   CAST(NULL AS nvarchar(250)) AS SubscriberSensitivity, " &
                        "	Tbl_Subscriber.spcSubscriberVoltageId, " &
                        "   CAST(NULL AS nvarchar(250)) AS SubscriberVoltage, " &
                        "	Tbl_Subscriber.spcSubscriberTypeId, " &
                        "   CAST(NULL AS nvarchar(250)) AS SubscriberType, " &
                        "	Tbl_Subscriber.spcSubscriberPhaseTypeId, " &
                        "   CAST(NULL AS nvarchar(250)) AS SubscriberPhaseType, " &
                        "	ACntrZarib, " &
                        "	Tbl_Subscriber.spcACntrTypeId, " &
                        "   CAST(NULL AS nvarchar(250)) AS ACntrType, " &
                        "	RCntrZarib, " &
                        "	Tbl_Subscriber.spcRCntrTypeId, " &
                        "   CAST(NULL AS nvarchar(250)) AS RCntrType, " &
                        "	AFabrikNumber, " &
                        "	RFabrikNumber, " &
                        "	AInstallDate, " &
                        "	RInstallDate, " &
                        "	POWER, " &
                        "	Amper, " &
                        "	Tbl_SubscriberInfo.spcSubscriberStateId, " &
                        "   CAST(NULL AS nvarchar(250)) AS SubscriberState, " &
                        "	Tbl_Subscriber.UseTypeId, " &
                        "   Tbl_UseType.UseType, " &
                        "	DebtPrice, " &
                        "	DebtType, " &
                        "	CAST(NULL AS bit) AS CanOutgoingCall, " &
                        "   Tbl_Area.CityId " &
                        "FROM  " &
                        "	Tbl_Subscriber " &
                        "	LEFT JOIN Tbl_SubscriberInfo ON Tbl_Subscriber.SubscriberId = Tbl_SubscriberInfo.SubscriberId " &
                        "	LEFT JOIN Tbl_Area ON Tbl_Subscriber.AreaId = Tbl_Area.AreaId " &
                        "	LEFT JOIN Tbl_MPPost ON Tbl_Subscriber.MPPostId = Tbl_MPPost.MPPostId " &
                        "	LEFT JOIN Tbl_MPFeeder ON Tbl_Subscriber.MPFeederId = Tbl_MPFeeder.MPFeederId " &
                        "	LEFT JOIN Tbl_LPPost ON Tbl_Subscriber.LPPostId = Tbl_LPPost.LPPostId " &
                        "	LEFT JOIN Tbl_LPFeeder ON Tbl_Subscriber.LPFeederId = Tbl_LPFeeder.LPFeederId " &
                        "	LEFT JOIN Tbl_SubscriberSensitivity ON Tbl_Subscriber.SubscriberSensitivityId = Tbl_SubscriberSensitivity.SubscriberSensitivityId " &
                        "	LEFT JOIN TblSpec Tbl_SubscriberVoltage ON Tbl_Subscriber.spcSubscriberVoltageId = Tbl_SubscriberVoltage.SpecId " &
                        "	LEFT JOIN TblSpec Tbl_SubscriberType ON Tbl_Subscriber.spcSubscriberTypeId = Tbl_SubscriberType.SpecId " &
                        "	LEFT JOIN TblSpec Tbl_SubscriberPhaseType ON Tbl_Subscriber.spcSubscriberPhaseTypeId = Tbl_SubscriberPhaseType.SpecId " &
                        "	LEFT JOIN TblSpec Tbl_ACntrType ON Tbl_Subscriber.spcACntrTypeId = Tbl_ACntrType.SpecId " &
                        "	LEFT JOIN TblSpec Tbl_RCntrType ON Tbl_Subscriber.spcRCntrTypeId = Tbl_RCntrType.SpecId " &
                        "	LEFT JOIN TblSpec Tbl_SubscriberState ON Tbl_SubscriberInfo.spcSubscriberStateId = Tbl_SubscriberState.SpecId " &
                        "	LEFT JOIN Tbl_UseType ON Tbl_Subscriber.UseTypeId = Tbl_UseType.UseTypeId "
                BindingTable(lSql, mCnn, mDs, "Tbl_SubscriberExcel", aIsClearTable:=True)

                Me.Text = "ورود اطلاعات پايه مشترکين از فايل اکسل"
                dg.Visible = False
                dgSubscriberInfo.Visible = True
                pnlSubsriber.Visible = True
                btnHelp.Visible = True
                MakeTableSubscriber()
                mSheet = "اطلاعات پايه مشترکين"
        End Select
        mOfd.Filter = "Excel Files|*.xls; *.xlsx"
    End Sub
    Private Function ImportData() As Boolean
        Dim lValidWord As String = ""
        Dim lValidWordNewVer As String = ""
        Dim lValidWordOLdVer As String = ""
        Dim lValTemp As String = ""
        Select Case mFormType
            Case ""
                mSheet = "بارگيري ترانس‌هاي فوق توزيع"
                lValidWordNewVer = "نامبهرهبردارنوعپستکدپستتوزيعنامپستتوزيعنامترانستاريخH1H2H3H4H5H6H7H8H9H10H11H12H13H14H15H16H17H18H19H20H21H22H23H24مجموع"
                lValTemp = "نام بهره بردارنوع پستکد پست توزیعنام پست توزیعنام ترانستاریخ"
                lValidWordOLdVer = "کدپستفوقتوزيعنامپستفوقتوزيعنامترانستاريخبارگيريساعتبارگيريباراکتيو(Mw)بارراکتيو(Mvar)".Replace("ی", "ي").Replace("ك", "ک")
            Case "PostFeeder"
                lValidWord = "کدGISفيدرفشارضعيفنامفيدرفشارضعيفنامپستتوزيعتاريخبارگيريساعتبارگيريفيوزمنصوبهRفيوزمنصوبهSفيوزمنصوبهTجريانفازRجريانفازSجريانفازTجرياننولفيدرولتاژانتهايفيدرجريانفازRپستجريانفازSپستجريانفازTپستجرياننولپستولتاژRNپستولتاژSNپستولتاژTNپستولتاژRSپستولتاژRTپستولتاژSTپست".Replace("ی", "ي").Replace("ك", "ک")
            Case "MPFeederLoad"
                If mVersionTypeId > -1 Then
                    Return True
                Else
                    ShowError("فايل اکسل به درستي انتخاب نشده است")
                    Return False
                End If
            Case "MPPostEnergy"
                lValidWord = "کدپستنامپستسالماهانرژيتحويلي".Replace("ی", "ي").Replace("ك", "ک")
            Case "MPFeederEnergy"
                lValidWord = "کدفيدرنامفيدرسالماهانرژيتحويلي".Replace("ی", "ي").Replace("ك", "ک")
            Case "Subscribers"
                lValidWord = "ناحيهسالماهتعدادجديدتعدادابطاليانرژيتحويليتعدادفيدر".Replace("ی", "ي").Replace("ك", "ک")
            Case "SubscriberInfo"
                lValidWord = "شمارهپروندهشمارهاشتراکرمزرايانهناممشترکتلفنثابتتلفنهمراهفکس".Replace("ی", "ي").Replace("ك", "ک")
            Case "ViewLPPostLoad"
                lValidWord = "کدGISپستتوزيعنامپستتوزيعتاريخبارگيريساعتبارگيريجريانفازRجريانفازSجريانفازTجرياننولآمپراژکليداصليمتوسطبارپيکپست".Replace("ی", "ي").Replace("ك", "ک")
            Case "ViewLPFeederLoad"
                lValidWord = "کدGISفيدرفشارضعيفنامفيدرفشارضعيفتاريخبارگيريساعتبارگيريفيوزمنصوبهRفيوزمنصوبهSفيوزمنصوبهT"
        End Select
        Dim lValidFromExcel As String = ""
        If mExcel Is Nothing Then
            mExcel = New CExcelManager
        Else
            mExcel.CloseWorkBook()
        End If
        Try
            mExcel.OpenWorkBook(mOfd.FileName)
            Dim dict As New Dictionary(Of String, String)
            dict.Add("Default", "BCDEFG")
            dict.Add("ViewLPPostLoad", "HIJKL")
            dict.Add("PostFeeder", "HIJKLMNOPQRSTUVWX")
            lValidFromExcel = ""
            For Each c As Char In dict.Item("Default").ToCharArray()
                lValidFromExcel &= mExcel.ReadCell(mSheet, c + "2")
            Next
            If lValidFromExcel = lValTemp Then
                For Each i As String In lArrHour
                    lValidFromExcel &= mExcel.ReadCell(mSheet, i)
                Next
                lValidFromExcel &= mExcel.ReadCell(mSheet, "AF2")
            ElseIf dict.ContainsKey(mFormType) Then
                For Each c As Char In dict.Item(mFormType).ToCharArray()
                    lValidFromExcel &= mExcel.ReadCell(mSheet, c + "2")
                Next
            Else
                lValidFromExcel &= mExcel.ReadCell(mSheet, "H2")
            End If

            lValidFromExcel = lValidFromExcel.Replace(" ", "").Replace("ی", "ي").Replace("ك", "ک")
            If lValidFromExcel = lValidWordNewVer And mFormType = "" Then
                mVersionTypeId = Version.NewVer
                dg.Visible = False
                dgNewLoad.Visible = True
            ElseIf lValidFromExcel = lValidWordOLdVer Then
                mVersionTypeId = Version.OldVer
                dg.Visible = True
                dgNewLoad.Visible = False
            ElseIf lValidFromExcel <> lValidWord Then
                ShowError("فايل اکسل به درستي انتخاب نشده است")
                Return False
            End If
            Return True
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Function
    Private Sub RunOperation()
        pg.Visible = True
        pg.Value = 1
        Select Case mFormType
            Case ""
                If mVersionTypeId = Version.NewVer Then
                    If MakeTableMPPostTransLoadExcelNewVer() > 0 Then
                        FillDataGridNew()
                    Else
                        ShowError("اطلاعاتي دريافت نشد")
                    End If
                Else
                    If MakeTableMPPostTransLoadExcel() > 0 Then
                        FillDataGrid()
                    Else
                        ShowError("اطلاعاتي دريافت نشد")
                    End If
                End If
            Case "PostFeeder"
                ParseExcel()
                dgPostFeeder.DataSource = mTbl_PostFeederExcel
                If excel.rows.Count > 0 Then
                    FillPostFeederDataGrid()
                End If
            Case "MPFeederLoad"
                If mVersionTypeId = Version.OldVer AndAlso MakeTableMPFeederLoadExcel() > 0 Then
                    FillMPFeederDataGrid()
                ElseIf mVersionTypeId = Version.NewVer AndAlso MakeTableMPFeederLoadNewVer() > 0 Then
                    MPFeederLoad_FillDataGridNew()
                Else
                    ShowError("اطلاعاتي دريافت نشد")
                End If
            Case "MPPostEnergy"
                If MakeTableMPPostEnergyExcel() > 0 Then
                    FillMPPostEnergyDataGrid()
                Else
                    ShowError("اطلاعاتي دريافت نشد")
                End If
            Case "MPFeederEnergy"
                If MakeTableMPFeederEnergyExcel() > 0 Then
                    FillMPFeederEnergyDataGrid()
                Else
                    ShowError("اطلاعاتي دريافت نشد")
                End If
            Case "Subscribers"
                If MakeTableSubscribersExcel() > 0 Then
                    FillSubscribersDataGrid()
                Else
                    ShowError("اطلاعاتي دريافت نشد")
                End If
            Case "SubscriberInfo"
                If MakeTableSubscriberExcel() > 0 Then
                    FillSubscriberDataGrid()
                Else
                    Me.Cursor = Cursors.Default
                    ShowError("اطلاعاتي دريافت نشد")
                End If
            Case "ViewLPPostLoad"
                If MakeTableLPPostLoadExcel() > 0 Then
                    FillLPPostLoadDataGrid()
                Else
                    ShowError("اطلاعاتي دريافت نشد")
                End If
            Case "ViewLPFeederLoad"
                If MakeTableLPFeederLoadExcel() > 0 Then
                    FillLPFeederLoadDataGrid()
                Else
                    ShowError("اطلاعاتي دريافت نشد")
                End If
        End Select
    End Sub

    ''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function MakeTableMPPostTransLoadExcel() As Integer
        Try
            mTbl_MPPostTransLoadExcel = New DataTable("Tbl_MPPostTransLoadExcel")
            mTbl_MPPostTransLoadExcel.Columns.Add("MPPostCode", GetType(String))
            mTbl_MPPostTransLoadExcel.Columns.Add("MPPostName", GetType(String))
            mTbl_MPPostTransLoadExcel.Columns.Add("MPPostTrans", GetType(String))
            mTbl_MPPostTransLoadExcel.Columns.Add("LoadDatePersian", GetType(String))
            mTbl_MPPostTransLoadExcel.Columns.Add("LoadTime", GetType(String))
            mTbl_MPPostTransLoadExcel.Columns.Add("LoadValue", GetType(Single))
            mTbl_MPPostTransLoadExcel.Columns.Add("ReactiveLoadValue", GetType(Single))

            Dim lTblRowCount As Integer = 0
            Dim lMPPostCode As String = ""
            Dim lMPPostName As String = ""
            Dim lMPTrans As String = ""
            Dim lLoadPersianDate As String = ""
            Dim lLoadTime As String = ""
            Dim lLoadValue As String = ""
            Dim lReactiveLoadValue As String = ""
            Dim lLastNullRecord As Integer = 0
            Dim i As Integer = 3
            Do

                lMPPostCode = mExcel.ReadCellRC(mSheet, i, 2)
                lMPTrans = mExcel.ReadCellRC(mSheet, i, 4)

                If lMPTrans + lMPPostCode = "" Then
                    lLastNullRecord += 1
                Else
                    lTblRowCount += 1
                    lLastNullRecord = 0
                End If

                If lLastNullRecord >= 5 Then
                    Exit Do
                End If
                i += 1
            Loop While True

            i = 3

            pg.Maximum = lTblRowCount
            Do
                lMPPostCode = mExcel.ReadCell(mSheet, "B" & i)
                lMPPostName = mExcel.ReadCell(mSheet, "C" & i)
                lMPTrans = mExcel.ReadCell(mSheet, "D" & i)
                lLoadPersianDate = mExcel.ReadCell(mSheet, "E" & i)
                lLoadTime = mExcel.ReadCell(mSheet, "F" & i)
                lLoadValue = Val(mExcel.ReadCell(mSheet, "G" & i))
                lReactiveLoadValue = Val(mExcel.ReadCell(mSheet, "H" & i))
                AdvanceProgress(pg)

                If lMPTrans + lMPPostCode = "" Then
                    lLastNullRecord += 1
                Else
                    mTbl_MPPostTransLoadExcel.Rows.Add(New Object() {lMPPostCode, lMPPostName, lMPTrans, lLoadPersianDate, lLoadTime, lLoadValue, lReactiveLoadValue})
                    lTblRowCount += 1
                    lLastNullRecord = 0
                End If

                If lLastNullRecord >= 5 Then
                    Exit Do
                End If
                i += 1
            Loop While True

            pg.Visible = False

            Return lTblRowCount
        Catch ex As Exception
            ShowError(ex.ToString)
        End Try

    End Function

    Private Function MakeTableMPPostTransLoadExcelNewVer() As Integer
        Try
            Dim lMPTrans As String = ""
            Dim lMPPostCode As String = ""
            Dim lLastNullRecord As Integer = 0
            Dim lTblRowCount As Integer = 0
            Dim lAreaName As String = ""
            Dim lMPPostType As String = ""
            Dim lSumLoad As String
            Dim lMPPostName As String = ""
            Dim lMPPostTransType As String = ""
            Dim lMPPostTransName As String = ""
            Dim lLoadDatePersian As String = ""
            Dim lLoadValue(27) As String
            Dim i As Integer = 3

            mTbl_MPPostTransLoadExcel = New DataTable("Tbl_MPPostTransLoadExcel")
            mTbl_MPPostTransLoadExcel.Columns.Add("AreaName", GetType(String))
            mTbl_MPPostTransLoadExcel.Columns.Add("MPPostCode", GetType(String))
            mTbl_MPPostTransLoadExcel.Columns.Add("MPPostName", GetType(String))
            mTbl_MPPostTransLoadExcel.Columns.Add("MPPostTrans", GetType(String))
            mTbl_MPPostTransLoadExcel.Columns.Add("LoadDatePersian", GetType(String))
            For index As Integer = 1 To 25
                mTbl_MPPostTransLoadExcel.Columns.Add("LoadValue" & index, GetType(String))
            Next
            mTbl_MPPostTransLoadExcel.Columns.Add("SumLoad", GetType(String))
            Do
                lMPPostCode = mExcel.ReadCellRC(mSheet, i, 2)
                lMPTrans = mExcel.ReadCellRC(mSheet, i, 4)

                If lMPTrans + lMPPostCode = "" Then
                    lLastNullRecord += 1
                Else
                    lTblRowCount += 1
                    lLastNullRecord = 0
                End If

                If lLastNullRecord >= 5 Then
                    Exit Do
                End If
                i += 1
            Loop While True
            i = 3
            pg.Maximum = lTblRowCount

            Do
                Dim lRow As DataRow = mTbl_MPPostTransLoadExcel.NewRow()

                lRow("AreaName") = mExcel.ReadCell(mSheet, "B" & i)
                lRow("MPPostCode") = mExcel.ReadCell(mSheet, "D" & i)
                lRow("MPPostName") = mExcel.ReadCell(mSheet, "E" & i)
                lRow("MPPostTrans") = mExcel.ReadCell(mSheet, "F" & i)
                lRow("LoadDatePersian") = mExcel.ReadCell(mSheet, "G" & i)
                lRow("LoadValue1") = Val(mExcel.ReadCell(mSheet, "H" & i))
                lRow("LoadValue2") = Val(mExcel.ReadCell(mSheet, "I" & i))
                lRow("LoadValue3") = Val(mExcel.ReadCell(mSheet, "J" & i))
                lRow("LoadValue4") = Val(mExcel.ReadCell(mSheet, "K" & i))
                lRow("LoadValue5") = Val(mExcel.ReadCell(mSheet, "L" & i))
                lRow("LoadValue6") = Val(mExcel.ReadCell(mSheet, "M" & i))
                lRow("LoadValue7") = Val(mExcel.ReadCell(mSheet, "N" & i))
                lRow("LoadValue8") = Val(mExcel.ReadCell(mSheet, "O" & i))
                lRow("LoadValue9") = Val(mExcel.ReadCell(mSheet, "P" & i))
                lRow("LoadValue10") = Val(mExcel.ReadCell(mSheet, "Q" & i))
                lRow("LoadValue11") = Val(mExcel.ReadCell(mSheet, "R" & i))
                lRow("LoadValue12") = Val(mExcel.ReadCell(mSheet, "S" & i))
                lRow("LoadValue13") = Val(mExcel.ReadCell(mSheet, "T" & i))
                lRow("LoadValue14") = Val(mExcel.ReadCell(mSheet, "U" & i))
                lRow("LoadValue15") = Val(mExcel.ReadCell(mSheet, "V" & i))
                lRow("LoadValue16") = Val(mExcel.ReadCell(mSheet, "W" & i))
                lRow("LoadValue17") = Val(mExcel.ReadCell(mSheet, "X" & i))
                lRow("LoadValue18") = Val(mExcel.ReadCell(mSheet, "Y" & i))
                lRow("LoadValue19") = Val(mExcel.ReadCell(mSheet, "Z" & i))
                lRow("LoadValue20") = Val(mExcel.ReadCell(mSheet, "AA" & i))
                lRow("LoadValue21") = Val(mExcel.ReadCell(mSheet, "AB" & i))
                lRow("LoadValue22") = Val(mExcel.ReadCell(mSheet, "AC" & i))
                lRow("LoadValue23") = Val(mExcel.ReadCell(mSheet, "AD" & i))
                lRow("LoadValue24") = Val(mExcel.ReadCell(mSheet, "AE" & i))
                lRow("SumLoad") = Val(mExcel.ReadCell(mSheet, "AF" & i))

                'AdvanceProgress(pg)

                If lRow("MPPostTrans") + lRow("MPPostCode") = "" Then
                    lLastNullRecord += 1
                Else
                    mTbl_MPPostTransLoadExcel.Rows.Add(lRow)
                    lTblRowCount += 1
                    lLastNullRecord = 0
                End If

                If lLastNullRecord >= 5 Then
                    Exit Do
                End If
                i += 1
            Loop While True
            pg.Visible = False

            Return lTblRowCount
        Catch ex As Exception
            ShowError(ex.ToString)
        End Try

    End Function


    Private Sub MakeTableMPPostTransLoad()
        mTbl_MPPostTransLoad = New DataTable("Tbl_MPPostTransLoad")
        mTbl_MPPostTransLoad.Columns.Add("MPPostTransLoadId", GetType(Long))
        mTbl_MPPostTransLoad.Columns.Add("MPPostTransId", GetType(Long))
        mTbl_MPPostTransLoad.Columns.Add("MPPostCode", GetType(String))
        mTbl_MPPostTransLoad.Columns.Add("MPPostName", GetType(String))
        mTbl_MPPostTransLoad.Columns.Add("MPPostTrans", GetType(String))
        mTbl_MPPostTransLoad.Columns.Add("LoadDatePersian", GetType(String))
        mTbl_MPPostTransLoad.Columns.Add("LoadTime", GetType(String))
        mTbl_MPPostTransLoad.Columns.Add("LoadValue", GetType(Single))
        mTbl_MPPostTransLoad.Columns.Add("ReactiveLoadValue", GetType(Single))
        mTbl_MPPostTransLoad.Columns.Add("MPPostTransLoadOldId", GetType(Long))
    End Sub
    Private Sub MakeTableMPPostTransLoadNewVersion()
        mTbl_MPPostTransLoadNew = New DataTable("Tbl_MPPostTransLoad")
        mTbl_MPPostTransLoadNew.Columns.Add("MPPostTransLoadId", GetType(Long))
        mTbl_MPPostTransLoadNew.Columns.Add("MPPostTransId", GetType(Long))
        mTbl_MPPostTransLoadNew.Columns.Add("MPPostCode", GetType(String))
        mTbl_MPPostTransLoadNew.Columns.Add("MPPostName", GetType(String))
        mTbl_MPPostTransLoadNew.Columns.Add("MPPostTrans", GetType(String))
        mTbl_MPPostTransLoadNew.Columns.Add("LoadDatePersian", GetType(String))
        For i As Integer = 1 To 25
            mTbl_MPPostTransLoadNew.Columns.Add("LoadValue" & i, GetType(Single))
        Next
        mTbl_MPPostTransLoadNew.Columns.Add("SumLoad", GetType(String))

    End Sub

    Private Sub FillDataGrid()
        Dim lTblMPPostRows() As DataRow
        Dim lMPPostTransLoadId As Long
        Dim lMPPostTransId As Long
        Dim lMPPostCode As String
        Dim lMPPostName As String
        Dim lMPPostTrans As String
        Dim lLoadDatePersian As String
        Dim lLoadTime As String
        Dim lLoadValue As Single
        Dim lReactiveLoadValue As Single
        Dim lMPPostTransLoadOldId As Long
        Dim lAcceptCount As Integer = 0, lFaildCount As Integer = 0
        Dim lMsg As String = ""

        Try
            For Each lRow As DataRow In mTbl_MPPostTransLoadExcel.Rows
                lTblMPPostRows = mDs.Tables("Tbl_MPPostTrans").Select("MPPostCode = '" & lRow("MPPostCode") & "' AND MPPostTrans = '" & lRow("MPPostTrans") & "'")
                If lTblMPPostRows.Length > 0 Then
                    lMPPostTransLoadId = GetAutoInc()
                    lMPPostTransId = lTblMPPostRows(0)("MPPostTransId")
                    lMPPostCode = lRow("MPPostCode")
                    lMPPostName = lTblMPPostRows(0)("MPPostName")
                    lMPPostTrans = lRow("MPPostTrans")
                    lLoadDatePersian = lRow("LoadDatePersian")
                    lLoadTime = lRow("LoadTime")
                    lLoadValue = lRow("LoadValue")
                    lReactiveLoadValue = lRow("ReactiveLoadValue")
                    lMPPostTransLoadOldId = lMPPostTransLoadId
                    mTbl_MPPostTransLoad.Rows.Add(New Object() {lMPPostTransLoadId, lMPPostTransId, lMPPostCode, lMPPostName,
                                                                lMPPostTrans, lLoadDatePersian, lLoadTime, lLoadValue, lReactiveLoadValue, lMPPostTransLoadOldId})
                    lAcceptCount += 1
                Else
                    lFaildCount += 1
                End If
            Next
            dg.DataSource = mTbl_MPPostTransLoad
            lblCount.Text = mTbl_MPPostTransLoad.Rows.Count
            lMsg = "تعداد ترانس‌هاي قابل قبول : " & lAcceptCount & vbCrLf
            lMsg &= "تعداد ترانس‌هاي غير قابل قبول : " & lFaildCount

            MsgBoxF(lMsg, "ورود از فايل اکسل", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Information, MessageBoxDefaultButton.Button1)

        Catch ex As Exception
            ShowError(ex.ToString)
        End Try
    End Sub
    Private Sub FillDataGridNew()
        Dim lTblMPPostRows() As DataRow
        Dim lMPPostTransLoadId As Long
        Dim lMPPostTransId As Long
        Dim lMPPostCode As String
        Dim lMPPostName As String
        Dim lMPPostTrans As String
        Dim lLoadDatePersian As String
        Dim lLoadTime As String
        Dim lLoadValue(27) As String
        Dim lSumLoad As String
        Dim lMPPostTransLoadOldId As Long
        Dim lAcceptCount As Integer = 0, lFaildCount As Integer = 0
        Dim lMsg As String = ""

        Try
            Dim i As Integer = 1

            For Each lRow As DataRow In mTbl_MPPostTransLoadExcel.Rows
                lTblMPPostRows = mDs.Tables("Tbl_MPPostTrans").Select("MPPostCode = '" & lRow("MPPostCode") & "' AND MPPostTrans = '" & lRow("MPPostTrans") & "'")

                If lTblMPPostRows.Length > 0 Then
                    lMPPostTransLoadId = GetAutoInc()
                    lMPPostTransId = lTblMPPostRows(0)("MPPostTransId")
                    lMPPostCode = lRow("MPPostCode")
                    lMPPostName = lTblMPPostRows(0)("MPPostName")
                    lMPPostTrans = lRow("MPPostTrans")
                    lLoadDatePersian = lRow("LoadDatePersian")

                    lLoadValue1 = IIf(IsDBNull(lRow("LoadValue1")), 0, lRow("LoadValue1"))
                    lLoadValue2 = IIf(IsDBNull(lRow("LoadValue2")), 0, lRow("LoadValue2"))
                    lLoadValue3 = IIf(IsDBNull(lRow("LoadValue3")), 0, lRow("LoadValue3"))
                    lLoadValue4 = IIf(IsDBNull(lRow("LoadValue4")), 0, lRow("LoadValue4"))
                    lLoadValue5 = IIf(IsDBNull(lRow("LoadValue5")), 0, lRow("LoadValue5"))
                    lLoadValue6 = IIf(IsDBNull(lRow("LoadValue6")), 0, lRow("LoadValue6"))
                    lLoadValue7 = IIf(IsDBNull(lRow("LoadValue7")), 0, lRow("LoadValue7"))
                    lLoadValue8 = IIf(IsDBNull(lRow("LoadValue8")), 0, lRow("LoadValue8"))
                    lLoadValue9 = IIf(IsDBNull(lRow("LoadValue9")), 0, lRow("LoadValue9"))
                    lLoadValue10 = IIf(IsDBNull(lRow("LoadValue10")), 0, lRow("LoadValue10"))
                    lLoadValue11 = IIf(IsDBNull(lRow("LoadValue11")), 0, lRow("LoadValue11"))
                    lLoadValue12 = IIf(IsDBNull(lRow("LoadValue12")), 0, lRow("LoadValue12"))
                    lLoadValue13 = IIf(IsDBNull(lRow("LoadValue13")), 0, lRow("LoadValue13"))
                    lLoadValue14 = IIf(IsDBNull(lRow("LoadValue14")), 0, lRow("LoadValue14"))
                    lLoadValue15 = IIf(IsDBNull(lRow("LoadValue15")), 0, lRow("LoadValue15"))
                    lLoadValue16 = IIf(IsDBNull(lRow("LoadValue16")), 0, lRow("LoadValue16"))
                    lLoadValue17 = IIf(IsDBNull(lRow("LoadValue17")), 0, lRow("LoadValue17"))
                    lLoadValue18 = IIf(IsDBNull(lRow("LoadValue18")), 0, lRow("LoadValue18"))
                    lLoadValue19 = IIf(IsDBNull(lRow("LoadValue19")), 0, lRow("LoadValue19"))
                    lLoadValue20 = IIf(IsDBNull(lRow("LoadValue20")), 0, lRow("LoadValue20"))
                    lLoadValue21 = IIf(IsDBNull(lRow("LoadValue21")), 0, lRow("LoadValue21"))
                    lLoadValue22 = IIf(IsDBNull(lRow("LoadValue22")), 0, lRow("LoadValue22"))
                    lLoadValue23 = IIf(IsDBNull(lRow("LoadValue23")), 0, lRow("LoadValue23"))
                    lLoadValue24 = IIf(IsDBNull(lRow("LoadValue24")), 0, lRow("LoadValue24"))
                    lSumLoad = IIf(IsDBNull(lRow("SumLoad")), 0, lRow("SumLoad"))

                    lMPPostTransLoadOldId = lMPPostTransLoadId
                    mTbl_MPPostTransLoadNew.Rows.Add(New Object() {lMPPostTransLoadOldId, lMPPostTransId, lMPPostCode, lMPPostName,
                                                                lMPPostTrans, lLoadDatePersian, lLoadValue1, lLoadValue2,
                                                                lLoadValue3, lLoadValue4, lLoadValue5, lLoadValue6, lLoadValue7,
                                                                lLoadValue8, lLoadValue9, lLoadValue10, lLoadValue11, lLoadValue12,
                                                                lLoadValue13, lLoadValue14, lLoadValue15, lLoadValue16, lLoadValue17,
                                                                lLoadValue18, lLoadValue19, lLoadValue20, lLoadValue21, lLoadValue22,
                                                                lLoadValue23, lLoadValue24, lSumLoad})
                    lAcceptCount += 1
                    i = +1
                Else
                    lFaildCount += 1
                End If
            Next
            dgNewLoad.DataSource = mTbl_MPPostTransLoadNew
            lblCount.Text = mTbl_MPPostTransLoadNew.Rows.Count
            lMsg = "تعداد ترانس‌هاي قابل قبول : " & lAcceptCount & vbCrLf
            lMsg &= "تعداد ترانس‌هاي غير قابل قبول : " & lFaildCount

            MsgBoxF(lMsg, "ورود از فايل اکسل", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Information, MessageBoxDefaultButton.Button1)

        Catch ex As Exception
            ShowError(ex.ToString)
        End Try
    End Sub
    Private Sub SaveInfo()
        Dim lNewRow As DatasetCcRequester.TblMPPostTransLoadRow
        Dim lHourNewRow As DatasetCcRequester.TblMPPostTransLoadHoursRow
        Dim lTransLoadRows() As DataRow
        Dim lTransLoadHoursRows() As DataRow
        Dim lKh As New KhayamDateTime
        Dim lHourId As Integer
        Dim lHourRows() As DataRow
        Dim lDate As String = "", lTime As String = "", lMiladiDT As DateTime = DateTime.Now
        Dim IsNewRow As Boolean = False, IsHourNewRow As Boolean = False
        Dim lTrans As SqlTransaction
        Dim lUpdate As New frmUpdateDataSetBT
        Dim lIsSaveOk As Boolean = False
        Dim lHourCount As Integer = 0, lDateCount As Integer = 0, lFaildCount As Integer = 0
        Dim lMsg As String = ""
        Dim lLoadValue As String = ""
        Dim lReactiveLoadValue As String = ""

        Try
            Dim i As Integer = 0
            For Each lRow As DataRow In mTbl_MPPostTransLoad.Rows
                lDate = ValidDate(lRow("LoadDatePersian"))

                lLoadValue = IIf(Not IsDBNull(lRow("LoadValue")), lRow("LoadValue"), "")
                lReactiveLoadValue = IIf(Not IsDBNull(lRow("ReactiveLoadValue")), lRow("ReactiveLoadValue"), "")
                lTime = ValidTime(lRow("LoadTime"))


                If Val(lLoadValue) >= 0 And Val(lReactiveLoadValue) >= 0 Then
                    If lTime = "" Then
                        If lDate <> "Error" And lDate <> "" Then
                            lKh.SetShamsiDate(lDate)
                            lMiladiDT = lKh.GetMiladyDate()
                            lTransLoadRows = DatasetCcRequester1.TblMPPostTransLoad.Select("MPPostTransId = " & lRow("MPPostTransId") & " AND LoadDatePersian = '" & lRow("LoadDatePersian") & "' AND IsDaily = 1")
                            If lTransLoadRows.Length > 0 Then
                                lNewRow = lTransLoadRows(0)
                            Else
                                lNewRow = DatasetCcRequester1.TblMPPostTransLoad.NewTblMPPostTransLoadRow()
                                lNewRow.MPPostTransLoadId = GetAutoInc()
                                IsNewRow = True
                            End If
                            lNewRow.MPPostTransId = lRow("MPPostTransId")
                            lNewRow.YearMonth = lDate.Substring(0, 7)
                            lNewRow.LoadValue = Math.Round(lRow("LoadValue"), 2)
                            lNewRow.ReactiveLoadValue = Math.Round(lRow("ReactiveLoadValue"), 2)
                            lNewRow.IsDaily = True
                            lNewRow.LoadDT = lMiladiDT.ToShortDateString()
                            lNewRow.LoadDatePersian = lDate
                            If IsNewRow Then
                                DatasetCcRequester1.TblMPPostTransLoad.Rows.Add(lNewRow)
                            End If
                            lDateCount += 1
                        Else
                            lFaildCount += 1
                        End If

                    ElseIf lTime <> "Error" Then

                        If lDate <> "Error" And lDate <> "" Then
                            lKh.SetShamsiDate(lDate)
                            lMiladiDT = lKh.GetMiladyDate().ToShortDateString()
                            lHourRows = DatasetCcRequester1.Tbl_Hour.Select("Hour = '" & lTime.Substring(0, 3) & "00'")
                            lHourId = lHourRows(0)("HourId")

                            lTransLoadRows = DatasetCcRequester1.TblMPPostTransLoad.Select("MPPostTransId = " & lRow("MPPostTransId") & " AND LoadDatePersian = '" & lRow("LoadDatePersian") & "' AND IsDaily = 1 ")

                            If lTransLoadRows.Length > 0 Then
                                lNewRow = lTransLoadRows(0)
                                IsNewRow = False
                            Else
                                lNewRow = DatasetCcRequester1.TblMPPostTransLoad.NewTblMPPostTransLoadRow()
                                lNewRow.MPPostTransLoadId = GetAutoInc()
                                IsNewRow = True
                            End If

                            lNewRow.MPPostTransId = lRow("MPPostTransId")
                            lNewRow.YearMonth = lDate.Substring(0, 7)
                            lNewRow.IsDaily = True
                            lNewRow.LoadDT = lMiladiDT
                            lNewRow.LoadDatePersian = lDate
                            lNewRow("MPPostTransLoadOldId") = lNewRow.MPPostTransLoadId
                            If IsNewRow Then
                                DatasetCcRequester1.TblMPPostTransLoad.Rows.Add(lNewRow)
                            End If

                            lTransLoadHoursRows = DatasetCcRequester1.TblMPPostTransLoadHours.Select("MPPostTransLoadId = " & lNewRow.MPPostTransLoadId & " AND HourId = " & lHourId)
                            If lTransLoadHoursRows.Length > 0 Then
                                lHourNewRow = lTransLoadHoursRows(0)
                                IsHourNewRow = False
                            Else
                                lHourNewRow = DatasetCcRequester1.TblMPPostTransLoadHours.NewTblMPPostTransLoadHoursRow()
                                lHourNewRow.MPPostTransLoadHourId = GetAutoInc()
                                IsHourNewRow = True
                            End If

                            lHourNewRow.MPPostTransLoadId = lNewRow.MPPostTransLoadId
                            lHourNewRow.HourId = lHourId
                            lHourNewRow.CurrentValue = Math.Round(lRow("LoadValue"), 2)
                            lHourNewRow.CurrentValueReActive = Math.Round(lRow("ReactiveLoadValue"), 2)
                            lHourNewRow.HourExact = lTime

                            If IsHourNewRow Then
                                DatasetCcRequester1.TblMPPostTransLoadHours.Rows.Add(lHourNewRow)
                            End If

                            lHourCount += 1
                        Else
                            lFaildCount += 1
                        End If
                    Else
                        lFaildCount += 1
                    End If
                Else
                    lFaildCount += 1
                End If
            Next


            lMsg = "تعداد بارگيري روزانه : " & lDateCount & vbCrLf &
                    "تعداد بارگيري ساعت به ساعت : " & lHourCount & vbCrLf &
                    "تعداد رديف داراي خطا : " & lFaildCount
            If MsgBoxF(lMsg & vbCrLf & " آيا اطلاعات بارگيري ذخيره شود؟ ", "تاييد اطلاعات بارگيري", MessageBoxButtons.YesNo, MsgBoxIcon.MsgIcon_Question, MessageBoxDefaultButton.Button1) = DialogResult.No Then
                Exit Sub
            End If

            If mCnn.State <> ConnectionState.Open Then
                mCnn.Open()
            End If

            lTrans = mCnn.BeginTransaction

            lIsSaveOk = lUpdate.UpdateDataSet("TblMPPostTransLoad", DatasetCcRequester1, , , lTrans)
            Dim lRows() As DataRow
            If lIsSaveOk Then
                For Each lRow As DataRow In DatasetCcRequester1.TblMPPostTransLoadHours.Rows
                    lRows = DatasetCcRequester1.TblMPPostTransLoad.Select("MPPostTransLoadOldId = " & lRow("MPPostTransLoadId"))
                    If lRows.Length > 0 And lRow.RowState <> DataRowState.Unchanged Then
                        lRow("MPPostTransLoadId") = lRows(0)("MPPostTransLoadId")
                    End If
                Next
                lIsSaveOk = lUpdate.UpdateDataSet("TblMPPostTransLoadHours", DatasetCcRequester1, , , lTrans)
            End If

            If lIsSaveOk Then
                lTrans.Commit()
                Dim lDlg As New frmInformUserAction("ذخيره اطلاعات با موفقيت صورت گرفت")
                lDlg.ShowDialog()
                lDlg.Dispose()
                Me.Close()
            Else
                lTrans.Rollback()
            End If
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
    End Sub
    Private Sub SaveInfoNew()
        Dim lNewRow As DatasetCcRequester.TblMPPostTransLoadRow
        Dim lHourNewRow As DatasetCcRequester.TblMPPostTransLoadHoursRow
        Dim lTransLoadRows() As DataRow
        Dim lTransLoadHoursRows() As DataRow
        Dim lKh As New KhayamDateTime
        Dim lHourId As Integer
        Dim lHourRows() As DataRow
        Dim lDate As String = "", lTime As String = "", lMiladiDT As DateTime = DateTime.Now
        Dim IsNewRow As Boolean = False, IsHourNewRow As Boolean = False
        Dim lTrans As SqlTransaction
        Dim lUpdate As New frmUpdateDataSetBT
        Dim lIsSaveOk As Boolean = False
        Dim lHourCount As Integer = 0, lDateCount As Integer = 0, lFaildCount As Integer = 0
        Dim lMsg As String = ""
        Dim lLoadValue As String = ""
        Dim lReactiveLoadValue As String = ""

        Try
            Dim i As Integer = 1
            For Each lRow As DataRow In mTbl_MPPostTransLoadNew.Rows
                For j As Integer = 1 To 24
                    lLoadValue = IIf(Not IsDBNull(lRow("LoadValue" & j)), lRow("LoadValue" & j), "")
                    lDate = ValidDate(lRow("LoadDatePersian"))
                    'If lLoadValue = "" Or lLoadValue = 0 Then
                    '    Exit For
                    'End If
                    Select Case j
                        Case 24, 25
                            lTime = "00:00"
                        Case Is < 10
                            lTime = "0" & j & ":00"
                        Case Else
                            lTime = j & ":00"
                    End Select
                    i = +1

                    If Val(lLoadValue) >= 0 And Val(lReactiveLoadValue) >= 0 Then
                        If lTime = "" Then
                            If lDate <> "Error" And lDate <> "" Then
                                lKh.SetShamsiDate(lDate)
                                lMiladiDT = lKh.GetMiladyDate()
                                lTransLoadRows = DatasetCcRequester1.TblMPPostTransLoad.Select("MPPostTransId = " & lRow("MPPostTransId") & " AND LoadDatePersian = '" & lRow("LoadDatePersian") & "' ")
                                If lTransLoadRows.Length > 0 Then
                                    lNewRow = lTransLoadRows(0)
                                Else
                                    lNewRow = DatasetCcRequester1.TblMPPostTransLoad.NewTblMPPostTransLoadRow()
                                    lNewRow.MPPostTransLoadId = GetAutoInc()
                                    IsNewRow = True
                                End If
                                lNewRow.MPPostTransId = lRow("MPPostTransId")
                                lNewRow.YearMonth = lDate.Substring(0, 7)

                                lNewRow.LoadValue = lLoadValue
                                lNewRow.ReactiveLoadValue = 0
                                lNewRow.IsDaily = True
                                lNewRow.LoadDT = lMiladiDT.ToShortDateString()
                                lNewRow.LoadDatePersian = lDate
                                If IsNewRow Then
                                    DatasetCcRequester1.TblMPPostTransLoad.Rows.Add(lNewRow)
                                End If
                                lDateCount += 1
                            Else
                                lFaildCount += 1
                            End If

                        ElseIf lTime <> "Error" Then

                            If lDate <> "Error" And lDate <> "" Then
                                lKh.SetShamsiDate(lDate)
                                lMiladiDT = lKh.GetMiladyDate().ToShortDateString()
                                lHourRows = DatasetCcRequester1.Tbl_Hour.Select("Hour = '" & lTime.Substring(0, 3) & "00'")
                                lHourId = lHourRows(0)("HourId")

                                lTransLoadRows = DatasetCcRequester1.TblMPPostTransLoad.Select("MPPostTransId = " & lRow("MPPostTransId") & " AND LoadDatePersian = '" & lRow("LoadDatePersian") & "' AND IsDaily = 1 ")

                                If lTransLoadRows.Length > 0 Then
                                    lNewRow = lTransLoadRows(0)
                                    IsNewRow = False
                                Else
                                    lNewRow = DatasetCcRequester1.TblMPPostTransLoad.NewTblMPPostTransLoadRow()
                                    lNewRow.MPPostTransLoadId = GetAutoInc()
                                    IsNewRow = True
                                End If

                                lNewRow.MPPostTransId = lRow("MPPostTransId")
                                lNewRow.YearMonth = lDate.Substring(0, 7)
                                lNewRow.IsDaily = True
                                lNewRow.LoadDT = lMiladiDT
                                lNewRow.LoadDatePersian = lDate
                                lNewRow("MPPostTransLoadOldId") = lNewRow.MPPostTransLoadId
                                If IsNewRow Then
                                    DatasetCcRequester1.TblMPPostTransLoad.Rows.Add(lNewRow)
                                End If

                                lTransLoadHoursRows = DatasetCcRequester1.TblMPPostTransLoadHours.Select("MPPostTransLoadId = " & lNewRow.MPPostTransLoadId & " AND HourId = " & lHourId)
                                If lTransLoadHoursRows.Length > 0 Then
                                    lHourNewRow = lTransLoadHoursRows(0)
                                    IsHourNewRow = False
                                Else
                                    lHourNewRow = DatasetCcRequester1.TblMPPostTransLoadHours.NewTblMPPostTransLoadHoursRow()
                                    lHourNewRow.MPPostTransLoadHourId = GetAutoInc()
                                    IsHourNewRow = True
                                End If

                                lHourNewRow.MPPostTransLoadId = lNewRow.MPPostTransLoadId
                                lHourNewRow.HourId = lHourId
                                lHourNewRow.CurrentValue = lLoadValue
                                lHourNewRow.CurrentValueReActive = 0
                                lHourNewRow.HourExact = lTime
                                If IsHourNewRow Then
                                    DatasetCcRequester1.TblMPPostTransLoadHours.Rows.Add(lHourNewRow)
                                End If

                                lHourCount += 1
                            Else
                                lFaildCount += 1
                            End If
                        Else
                            lFaildCount += 1
                        End If
                    Else
                        lFaildCount += 1
                    End If
                Next

            Next
            lMsg = "تعداد بارگيري روزانه : " & lDateCount & vbCrLf &
                    "تعداد بارگيري ساعت به ساعت : " & lHourCount & vbCrLf &
                    "تعداد رديف داراي خطا : " & lFaildCount
            If MsgBoxF(lMsg & vbCrLf & " آيا اطلاعات بارگيري ذخيره شود؟ ", "تاييد اطلاعات بارگيري", MessageBoxButtons.YesNo, MsgBoxIcon.MsgIcon_Question, MessageBoxDefaultButton.Button1) = DialogResult.No Then
                Exit Sub
            End If

            If mCnn.State <> ConnectionState.Open Then
                mCnn.Open()
            End If

            lTrans = mCnn.BeginTransaction

            lIsSaveOk = lUpdate.UpdateDataSet("TblMPPostTransLoad", DatasetCcRequester1, , , lTrans)
            Dim lRows() As DataRow
            If lIsSaveOk Then
                For Each lRow As DataRow In DatasetCcRequester1.TblMPPostTransLoadHours.Rows
                    lRows = DatasetCcRequester1.TblMPPostTransLoad.Select("MPPostTransLoadOldId = " & lRow("MPPostTransLoadId"))
                    If lRows.Length > 0 And lRow.RowState <> DataRowState.Unchanged Then
                        lRow("MPPostTransLoadId") = lRows(0)("MPPostTransLoadId")
                    End If
                Next
                lIsSaveOk = lUpdate.UpdateDataSet("TblMPPostTransLoadHours", DatasetCcRequester1, , , lTrans)
            End If

            If lIsSaveOk Then
                lTrans.Commit()
                Dim lDlg As New frmInformUserAction("ذخيره اطلاعات با موفقيت صورت گرفت")
                lDlg.ShowDialog()
                lDlg.Dispose()
                Me.Close()
            Else
                lTrans.Rollback()
            End If
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
    End Sub
    '''''''''''''''''''''''''''''''''''''''''''''''''MPFeederLoad
    Private Sub MakeTableMPFeederLoad()
        mTbl_MPFeederLoad = New DataTable("TblMPFeederLoad")
        mTbl_MPFeederLoad.Columns.Add("MPFeederLoadId", GetType(Long))
        mTbl_MPFeederLoad.Columns.Add("MPFeederId", GetType(Long))
        mTbl_MPFeederLoad.Columns.Add("MPFeederCode", GetType(String))
        mTbl_MPFeederLoad.Columns.Add("MPFeederName", GetType(String))
        mTbl_MPFeederLoad.Columns.Add("RelDatePersian", GetType(String))
        mTbl_MPFeederLoad.Columns.Add("RelTime", GetType(String))
        mTbl_MPFeederLoad.Columns.Add("CurrentValue", GetType(Single))
        mTbl_MPFeederLoad.Columns.Add("CurrentValueReactive", GetType(Single))
        mTbl_MPFeederLoad.Columns.Add("PowerValue", GetType(Single))
        mTbl_MPFeederLoad.Columns.Add("MPFeederLoadOldId", GetType(Long))
    End Sub
    Private Function MakeTableMPFeederLoadExcel() As Integer
        Try
            mTbl_MPFeederLoadExcel = New DataTable("Tbl_MPFeederLoadExcel")
            mTbl_MPFeederLoadExcel.Columns.Add("MPFeederCode", GetType(String))
            mTbl_MPFeederLoadExcel.Columns.Add("MPFeederName", GetType(String))
            mTbl_MPFeederLoadExcel.Columns.Add("RelDatePersian", GetType(String))
            mTbl_MPFeederLoadExcel.Columns.Add("RelTime", GetType(String))
            mTbl_MPFeederLoadExcel.Columns.Add("CurrentValue", GetType(Single))
            mTbl_MPFeederLoadExcel.Columns.Add("CurrentValueReactive", GetType(Single))
            mTbl_MPFeederLoadExcel.Columns.Add("PowerValue", GetType(Single))

            Dim lTblRowCount As Integer = 0
            Dim lMPFeederCode As String = ""
            Dim lMPFeederName As String = ""
            Dim lRelPersianDate As String = ""
            Dim lRelTime As String = ""
            Dim lCurrentValue As String = ""
            Dim lCurrentValueReactive As String = ""
            Dim lPowerValue As String = ""
            Dim lLastNullRecord As Integer = 0
            Dim i As Integer = 3
            Do

                lMPFeederCode = mExcel.ReadCellRC(mSheet, i, 2)

                If lMPFeederCode = "" Then
                    lLastNullRecord += 1
                Else
                    lTblRowCount += 1
                    lLastNullRecord = 0
                End If

                If lLastNullRecord >= 5 Then
                    Exit Do
                End If
                i += 1
            Loop While True

            i = 3

            pg.Maximum = lTblRowCount
            Do
                lMPFeederCode = mExcel.ReadCell(mSheet, "B" & i)
                lMPFeederName = mExcel.ReadCell(mSheet, "C" & i)
                lRelPersianDate = mExcel.ReadCell(mSheet, "D" & i)
                lRelTime = mExcel.ReadCell(mSheet, "E" & i)
                lCurrentValue = Val(mExcel.ReadCell(mSheet, "F" & i))
                lCurrentValueReactive = Val(mExcel.ReadCell(mSheet, "G" & i))
                lPowerValue = Val(mExcel.ReadCell(mSheet, "H" & i))
                AdvanceProgress(pg)

                If lMPFeederCode = "" Then
                    lLastNullRecord += 1
                Else
                    mTbl_MPFeederLoadExcel.Rows.Add(New Object() {lMPFeederCode, lMPFeederName, lRelPersianDate, lRelTime, lCurrentValue, lCurrentValueReactive, lPowerValue})
                    lTblRowCount += 1
                    lLastNullRecord = 0
                End If

                If lLastNullRecord >= 5 Then
                    Exit Do
                End If
                i += 1
            Loop While True

            pg.Visible = False

            Return lTblRowCount
        Catch ex As Exception
            ShowError(ex.ToString)
        End Try

    End Function
    Private Sub FillMPFeederDataGrid()
        Dim lTblMPFeederRows() As DataRow
        Dim lMPFeederLoadId As Long
        Dim lMPFeederId As Integer
        Dim lMPFeederCode As String
        Dim lMPFeederName As String
        Dim lRelDatePersian As String
        Dim lRelTime As String
        Dim lCurrentValue As Single
        Dim lCurrentValueReactive As Single
        Dim lPowerValue As Single
        Dim lMPFeederLoadOldId As Long
        Dim lAcceptCount As Integer = 0, lFaildCount As Integer = 0
        Dim lMsg As String = ""
        Dim lMPFeederIDs As String = "-1"
        Dim lDates As String = ""

        Try
            For Each lRow As DataRow In mTbl_MPFeederLoadExcel.Rows
                lTblMPFeederRows = mDs.Tables("Tbl_MPFeeder").Select("MPFeederCode = '" & lRow("MPFeederCode") & "' AND IsActive = 1 ")
                If lTblMPFeederRows.Length > 0 Then
                    lMPFeederLoadId = GetAutoInc()
                    lMPFeederId = lTblMPFeederRows(0)("MPFeederId")
                    lMPFeederCode = lRow("MPFeederCode")
                    lMPFeederName = lTblMPFeederRows(0)("MPFeederName")
                    lRelDatePersian = lRow("RelDatePersian")
                    lRelTime = lRow("RelTime")
                    lCurrentValue = lRow("CurrentValue")
                    lCurrentValueReactive = lRow("CurrentValueReactive")
                    lPowerValue = lRow("PowerValue")
                    mTbl_MPFeederLoad.Rows.Add(New Object() {lMPFeederLoadId, lMPFeederId, lMPFeederCode, lMPFeederName,
                                                                 lRelDatePersian, lRelTime, lCurrentValue, lCurrentValueReactive, lPowerValue})
                    lMPFeederIDs &= IIf(lMPFeederIDs = "", lTblMPFeederRows(0)("MPFeederId"), "," & lTblMPFeederRows(0)("MPFeederId"))
                    If Not lDates.Contains(lRow("RelDatePersian")) Then
                        lDates &= IIf(lDates = "", "'" & lRow("RelDatePersian") & "'", ",'" & lRow("RelDatePersian") & "'")
                    End If
                    lAcceptCount += 1
                Else
                    lFaildCount += 1
                End If
            Next
            dgMPFeeder.DataSource = mTbl_MPFeederLoad
            dgMPFeederNew.Visible = False
            dgMPFeeder.Visible = True
            lblCount.Text = mTbl_MPFeederLoad.Rows.Count

            SaveLog("step 0 _ " & lMPFeederIDs & DateTime.Now, "testExcel.log")
            BindingTable("SELECT *, CAST(MPFeederLoadId AS Bigint) as MPFeederLoadOldId FROM Tbl_MPFeederLoad where MPFeederId in (" & lMPFeederIDs & ") AND RelDatePersian in (" & lDates & ")", mCnn, mDs, "Tbl_MPFeederLoad", , , , , , , True)
            BindingTable("SELECT Tbl_MPFeederLoadHours.* FROM Tbl_MPFeederLoadHours inner join Tbl_MPFeederLoad on Tbl_MPFeederLoadHours.MPFeederLoadId = Tbl_MPFeederLoad.MPFeederLoadId where MPFeederId in (" & lMPFeederIDs & ") AND RelDatePersian in (" & lDates & ")", mCnn, mDs, "Tbl_MPFeederLoadHours", , , , , , , True)
            SaveLog("step 0.0 _ " & lMPFeederIDs & DateTime.Now, "testExcel.log")
            mSortedListLoadHour.Clear()
            For Each lRow As DataRow In mDs.Tables("Tbl_MPFeederLoadHours").Rows
                If Not mSortedListLoadHour.ContainsKey(lRow("MPFeederLoadId") & "-" & lRow("HourId")) Then
                    mSortedListLoadHour.Add(lRow("MPFeederLoadId") & "-" & lRow("HourId"), lRow)
                End If
            Next
            SaveLog("step 0.1 _ " & lMPFeederIDs & DateTime.Now, "testExcel.log")

            lMsg = "تعداد فيدرهاي قابل قبول : " & lAcceptCount & vbCrLf
            lMsg &= "تعداد فيدرهاي غير قابل قبول : " & lFaildCount

            MsgBoxF(lMsg, "ورود از فايل اکسل", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Information, MessageBoxDefaultButton.Button1)

        Catch ex As Exception
            ShowError(ex.ToString)
        End Try
    End Sub
    Private Sub SaveMPFeederLoadInfo()
        Dim lNewRow As DataRow
        Dim lHourNewRow As DataRow
        Dim lMPFeederLoadRows() As DataRow
        'Dim lMPFeederLoadHoursRows() As DataRow
        Dim lMPFeederLoadHoursRow As DataRow
        Dim lMPFeederRow As DataRow
        Dim lKh As New KhayamDateTime
        Dim lHourId As Integer
        Dim lHourRows() As DataRow
        Dim lDate As String = "", lTime As String = "", lMiladiDT As DateTime = DateTime.Now
        Dim IsNewRow As Boolean = False, IsHourNewRow As Boolean = False
        Dim lTrans As SqlTransaction
        Dim lUpdate As New frmUpdateDataSetBT
        Dim lIsSaveOk As Boolean = False
        Dim lHourCount As Integer = 0, lDateCount As Integer = 0, lFaildCount As Integer = 0
        Dim lMsg As String = ""
        Dim lCurrentValue As String
        Dim lCurrentValueReactive As String
        Dim lPowerValue As String
        Try
            For Each lRow As DataRow In mTbl_MPFeederLoad.Rows
                lTime = ValidTime(lRow("RelTime"))
                lDate = ValidDate(lRow("RelDatePersian"))
                lCurrentValue = IIf(Not IsDBNull(lRow("CurrentValue")), lRow("CurrentValue"), "")
                lCurrentValueReactive = IIf(Not IsDBNull(lRow("CurrentValueReActive")), lRow("CurrentValueReActive"), "")
                lPowerValue = IIf(Not IsDBNull(lRow("PowerValue")), lRow("PowerValue"), "")
                If Val(lCurrentValue) >= 0 And Val(lCurrentValueReactive) >= 0 And Val(lPowerValue) >= 0 Then
                    If lTime <> "Error" And lTime <> "" Then

                        If lDate <> "Error" And lDate <> "" Then
                            lKh.SetShamsiDate(lDate)
                            lMiladiDT = lKh.GetMiladyDate().ToShortDateString()
                            lHourRows = mDs.Tables("Tbl_Hour").Select("Hour = '" & lTime.Substring(0, 3) & "00'")
                            lHourId = lHourRows(0)("HourId")

                            lMPFeederLoadRows = mDs.Tables("Tbl_MPFeederLoad").Select("MPFeederId = " & lRow("MPFeederId") & " AND RelDatePersian = '" & lRow("RelDatePersian") & "'")
                            If lMPFeederLoadRows.Length > 0 Then
                                lNewRow = lMPFeederLoadRows(0)
                                IsNewRow = False
                            Else
                                lNewRow = mDs.Tables("Tbl_MPFeederLoad").NewRow()
                                lNewRow("MPFeederLoadId") = GetAutoInc()
                                IsNewRow = True
                            End If

                            BindingTable("SELECT * FROM Tbl_MPFeeder WHERE MPFeederId = " & lRow("MPFeederId"), mCnn, mDs, "Tbl_MPFeeder", , , , , , , True)
                            SaveLog("step 1 _ " & DateTime.Now, "testExcel.log")
                            lMPFeederRow = mDs.Tables("Tbl_MPFeeder").Rows(0)
                            lNewRow("AreaId") = lMPFeederRow("AreaId")
                            lNewRow("MPPostId") = lMPFeederRow("MPPostId")
                            lNewRow("MPFeederId") = lMPFeederRow("MPFeederId")
                            lNewRow("RelDate") = lMiladiDT
                            lNewRow("RelDatePersian") = lDate
                            lNewRow("GrowthPercentage") = 0
                            lNewRow("MPFeederLoadOldId") = lNewRow("MPFeederLoadId")
                            If IsNewRow Then
                                mDs.Tables("Tbl_MPFeederLoad").Rows.Add(lNewRow)
                            End If

                            'lMPFeederLoadHoursRows =  mDs.Tables("Tbl_MPFeederLoadHours").Select("MPFeederLoadId = " & lNewRow("MPFeederLoadId") & " AND HourId = " & lHourId)
                            lMPFeederLoadHoursRow = mSortedListLoadHour(lNewRow("MPFeederLoadId") & "-" & lHourId)
                            SaveLog("step 2 _ " & DateTime.Now, "testExcel.log")
                            ' If lMPFeederLoadHoursRows.Length > 0 Then
                            If Not lMPFeederLoadHoursRow Is Nothing Then
                                lHourNewRow = lMPFeederLoadHoursRow
                                IsHourNewRow = False
                            Else
                                lHourNewRow = mDs.Tables("Tbl_MPFeederLoadHours").NewRow()
                                lHourNewRow("MPFeederLoadHourId") = GetAutoInc()
                                IsHourNewRow = True
                            End If

                            lHourNewRow("MPFeederLoadId") = lNewRow("MPFeederLoadId")
                            lHourNewRow("HourId") = lHourId
                            lHourNewRow("CurrentValue") = Math.Round(lRow("CurrentValue"), 2)
                            lHourNewRow("PowerValue") = Math.Round(lRow("PowerValue"), 2)
                            lHourNewRow("CurrentValueReActive") = Math.Round(lRow("CurrentValueReActive"), 2)
                            lHourNewRow("HourExact") = lTime

                            If IsHourNewRow Then
                                mDs.Tables("Tbl_MPFeederLoadHours").Rows.Add(lHourNewRow)
                            End If
                            lHourCount += 1
                        Else
                            lFaildCount += 1
                        End If
                    Else
                        lFaildCount += 1
                    End If
                Else
                    lFaildCount += 1
                End If
            Next

            lMsg = "تعداد بارگيري ساعت به ساعت : " & lHourCount & vbCrLf &
                    "تعداد رديف داراي خطا : " & lFaildCount
            If MsgBoxF(lMsg & vbCrLf & " آيا اطلاعات بارگيري ذخيره شود؟ ", "تاييد اطلاعات بارگيري", MessageBoxButtons.YesNo, MsgBoxIcon.MsgIcon_Question, MessageBoxDefaultButton.Button1) = DialogResult.No Then
                Exit Sub
            End If

            If mCnn.State <> ConnectionState.Open Then
                mCnn.Open()
            End If

            lTrans = mCnn.BeginTransaction
            mSortedListLoad.Clear()
            For Each lRow As DataRow In mDs.Tables("Tbl_MPFeederLoad").Rows
                mSortedListLoad.Add(lRow("MPFeederLoadOldId"), lRow)
            Next

            lIsSaveOk = lUpdate.UpdateDataSet("Tbl_MPFeederLoad", mDs, , , lTrans)
            Dim lRowLoad As DataRow

            SaveLog("step 3 _ " & DateTime.Now, "testExcel.log")
            If lIsSaveOk Then
                For Each lRow As DataRow In mDs.Tables("Tbl_MPFeederLoadHours").Rows

                    lRowLoad = mSortedListLoad(lRow("MPFeederLoadId"))
                    If Not lRowLoad Is Nothing AndAlso lRow.RowState <> DataRowState.Unchanged Then
                        lRow("MPFeederLoadId") = lRowLoad("MPFeederLoadId")
                    End If
                Next
                lIsSaveOk = lUpdate.UpdateDataSet("Tbl_MPFeederLoadHours", mDs, , , lTrans)
            End If

            SaveLog("step 4 _ " & DateTime.Now, "testExcel.log")
            If lIsSaveOk Then
                lTrans.Commit()
                Dim lDlg As New frmInformUserAction("ذخيره اطلاعات با موفقيت صورت گرفت")
                lDlg.ShowDialog()
                lDlg.Dispose()
                Me.Close()
            Else
                lTrans.Rollback()
            End If
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
    End Sub
    '''''''''''''''''''''''''''''''''''''''''''''''''MPFeederLoadNew
    Private Sub MakeTableMPFeederLoadNew()
        mTbl_MPFeederLoadNew = New DataTable("Tbl_MPFeederLoadExcelNew")
        mTbl_MPFeederLoadNew.Columns.Add("MPFeederLoadId", GetType(Long))
        mTbl_MPFeederLoadNew.Columns.Add("MPFeederId", GetType(Long))
        mTbl_MPFeederLoadNew.Columns.Add("MPFeederCode", GetType(String))
        mTbl_MPFeederLoadNew.Columns.Add("MPFeederName", GetType(String))
        mTbl_MPFeederLoadNew.Columns.Add("RelDatePersian", GetType(String))
        For i As Integer = 1 To 25
            mTbl_MPFeederLoadNew.Columns.Add("LoadValue" & i, GetType(Single))
        Next
        mTbl_MPFeederLoadNew.Columns.Add("SumLoad", GetType(String))

    End Sub
    Private Function MakeTableMPFeederLoadNewVer() As Integer
        Try
            Dim lMPFeederCode As String = ""
            Dim lLastNullRecord As Integer = 0
            Dim lTblRowCount As Integer = 0
            Dim lAreaName As String = ""
            Dim lSumLoad As String
            Dim lLoadDatePersian As String = ""
            Dim lLoadValue(27) As String
            Dim i As Integer = 3

            mTbl_MPFeederLoadExcelNew = New DataTable("Tbl_MPFeederLoadExcelNew")
            mTbl_MPFeederLoadExcelNew.Columns.Add("MPFeederLoadId", GetType(Long))
            mTbl_MPFeederLoadExcelNew.Columns.Add("MPFeederId", GetType(Long))
            mTbl_MPFeederLoadExcelNew.Columns.Add("MPFeederCode", GetType(String))
            mTbl_MPFeederLoadExcelNew.Columns.Add("MPFeederName", GetType(String))
            mTbl_MPFeederLoadExcelNew.Columns.Add("RelDatePersian", GetType(String))
            For j As Integer = 1 To 25
                mTbl_MPFeederLoadExcelNew.Columns.Add("LoadValue" & j, GetType(Single))
            Next
            mTbl_MPFeederLoadExcelNew.Columns.Add("SumLoad", GetType(String))
            Do
                lMPFeederCode = mExcel.ReadCellRC(mSheet, i, 2)
                If lMPFeederCode = "" Then
                    lLastNullRecord += 1
                Else
                    lTblRowCount += 1
                    lLastNullRecord = 0
                End If

                If lLastNullRecord >= 5 Then
                    Exit Do
                End If
                i += 1
            Loop While True
            i = 3
            pg.Maximum = lTblRowCount

            Do
                Dim lRow As DataRow = mTbl_MPFeederLoadExcelNew.NewRow()
                lRow("MPFeederCode") = mExcel.ReadCell(mSheet, "B" & i)
                lRow("MPFeederName") = mExcel.ReadCell(mSheet, "C" & i)
                lRow("RelDatePersian") = mExcel.ReadCell(mSheet, "D" & i)
                lRow("LoadValue1") = Val(mExcel.ReadCell(mSheet, "E" & i))
                lRow("LoadValue2") = Val(mExcel.ReadCell(mSheet, "F" & i))
                lRow("LoadValue3") = Val(mExcel.ReadCell(mSheet, "G" & i))
                lRow("LoadValue4") = Val(mExcel.ReadCell(mSheet, "H" & i))
                lRow("LoadValue5") = Val(mExcel.ReadCell(mSheet, "I" & i))
                lRow("LoadValue6") = Val(mExcel.ReadCell(mSheet, "J" & i))
                lRow("LoadValue7") = Val(mExcel.ReadCell(mSheet, "K" & i))
                lRow("LoadValue8") = Val(mExcel.ReadCell(mSheet, "L" & i))
                lRow("LoadValue9") = Val(mExcel.ReadCell(mSheet, "M" & i))
                lRow("LoadValue10") = Val(mExcel.ReadCell(mSheet, "N" & i))
                lRow("LoadValue11") = Val(mExcel.ReadCell(mSheet, "O" & i))
                lRow("LoadValue12") = Val(mExcel.ReadCell(mSheet, "P" & i))
                lRow("LoadValue13") = Val(mExcel.ReadCell(mSheet, "Q" & i))
                lRow("LoadValue14") = Val(mExcel.ReadCell(mSheet, "R" & i))
                lRow("LoadValue15") = Val(mExcel.ReadCell(mSheet, "S" & i))
                lRow("LoadValue16") = Val(mExcel.ReadCell(mSheet, "T" & i))
                lRow("LoadValue17") = Val(mExcel.ReadCell(mSheet, "U" & i))
                lRow("LoadValue18") = Val(mExcel.ReadCell(mSheet, "V" & i))
                lRow("LoadValue19") = Val(mExcel.ReadCell(mSheet, "W" & i))
                lRow("LoadValue20") = Val(mExcel.ReadCell(mSheet, "X" & i))
                lRow("LoadValue21") = Val(mExcel.ReadCell(mSheet, "Y" & i))
                lRow("LoadValue22") = Val(mExcel.ReadCell(mSheet, "Z" & i))
                lRow("LoadValue23") = Val(mExcel.ReadCell(mSheet, "AA" & i))
                lRow("LoadValue24") = Val(mExcel.ReadCell(mSheet, "AB" & i))
                lRow("SumLoad") = Val(mExcel.ReadCell(mSheet, "AC" & i))

                '  AdvanceProgress(pg)

                If lRow("MPFeederName") = "" And lRow("MPFeederCode").ToString() = "" Then
                    lLastNullRecord += 1
                Else
                    mTbl_MPFeederLoadExcelNew.Rows.Add(lRow)
                    lTblRowCount += 1
                    lLastNullRecord = 0
                End If

                If lLastNullRecord >= 5 Then
                    Exit Do
                End If
                i += 1
            Loop While True
            pg.Visible = False

            Return lTblRowCount
        Catch ex As Exception
            ShowError(ex.ToString)
        End Try

    End Function
    Private Sub MPFeederLoad_FillDataGridNew()
        Dim lTblMPFeederRows() As DataRow
        Dim lMPFeederLoadId As Long
        Dim lMPFeederId As Integer
        Dim lMPFeederCode As String
        Dim lMPFeederName As String
        Dim lMPPostTrans As String
        Dim lLoadDatePersian As String
        Dim lLoadTime As String
        Dim lLoadValue(27) As String
        Dim lSumLoad As String
        Dim lMPFeederLoadOldId As Long
        Dim lAcceptCount As Integer = 0, lFaildCount As Integer = 0
        Dim lMsg As String = ""
        Dim lMPFeederIDs As String = ""
        Dim lDates As String = ""
        Try
            Dim i As Integer = 1
            For Each lRow As DataRow In mTbl_MPFeederLoadExcelNew.Rows
                lTblMPFeederRows = mDs.Tables("Tbl_MPFeeder").Select("MPFeederCode = '" & lRow("MPFeederCode") & "' AND IsActive = 1  ")

                If lTblMPFeederRows.Length > 0 Then
                    lMPFeederLoadId = GetAutoInc()
                    lMPFeederId = lTblMPFeederRows(0)("MPFeederId")
                    lMPFeederCode = lRow("MPFeederCode")
                    lMPFeederName = lTblMPFeederRows(0)("MPFeederName")
                    lLoadDatePersian = lRow("RelDatePersian")
                    lLoadValue1 = IIf(IsDBNull(lRow("LoadValue1")), 0, lRow("LoadValue1"))
                    lLoadValue2 = IIf(IsDBNull(lRow("LoadValue2")), 0, lRow("LoadValue2"))
                    lLoadValue3 = IIf(IsDBNull(lRow("LoadValue3")), 0, lRow("LoadValue3"))
                    lLoadValue4 = IIf(IsDBNull(lRow("LoadValue4")), 0, lRow("LoadValue4"))
                    lLoadValue5 = IIf(IsDBNull(lRow("LoadValue5")), 0, lRow("LoadValue5"))
                    lLoadValue6 = IIf(IsDBNull(lRow("LoadValue6")), 0, lRow("LoadValue6"))
                    lLoadValue7 = IIf(IsDBNull(lRow("LoadValue7")), 0, lRow("LoadValue7"))
                    lLoadValue8 = IIf(IsDBNull(lRow("LoadValue8")), 0, lRow("LoadValue8"))
                    lLoadValue9 = IIf(IsDBNull(lRow("LoadValue9")), 0, lRow("LoadValue9"))
                    lLoadValue10 = IIf(IsDBNull(lRow("LoadValue10")), 0, lRow("LoadValue10"))
                    lLoadValue11 = IIf(IsDBNull(lRow("LoadValue11")), 0, lRow("LoadValue11"))
                    lLoadValue12 = IIf(IsDBNull(lRow("LoadValue12")), 0, lRow("LoadValue12"))
                    lLoadValue13 = IIf(IsDBNull(lRow("LoadValue13")), 0, lRow("LoadValue13"))
                    lLoadValue14 = IIf(IsDBNull(lRow("LoadValue14")), 0, lRow("LoadValue14"))
                    lLoadValue15 = IIf(IsDBNull(lRow("LoadValue15")), 0, lRow("LoadValue15"))
                    lLoadValue16 = IIf(IsDBNull(lRow("LoadValue16")), 0, lRow("LoadValue16"))
                    lLoadValue17 = IIf(IsDBNull(lRow("LoadValue17")), 0, lRow("LoadValue17"))
                    lLoadValue18 = IIf(IsDBNull(lRow("LoadValue18")), 0, lRow("LoadValue18"))
                    lLoadValue19 = IIf(IsDBNull(lRow("LoadValue19")), 0, lRow("LoadValue19"))
                    lLoadValue20 = IIf(IsDBNull(lRow("LoadValue20")), 0, lRow("LoadValue20"))
                    lLoadValue21 = IIf(IsDBNull(lRow("LoadValue21")), 0, lRow("LoadValue21"))
                    lLoadValue22 = IIf(IsDBNull(lRow("LoadValue22")), 0, lRow("LoadValue22"))
                    lLoadValue23 = IIf(IsDBNull(lRow("LoadValue23")), 0, lRow("LoadValue23"))
                    lLoadValue24 = IIf(IsDBNull(lRow("LoadValue24")), 0, lRow("LoadValue24"))
                    lSumLoad = IIf(IsDBNull(lRow("SumLoad")), 0, lRow("SumLoad"))

                    lMPFeederLoadOldId = lMPFeederLoadId
                    mTbl_MPFeederLoadNew.Rows.Add(New Object() {lMPFeederLoadOldId, lMPFeederId, lMPFeederCode, lMPFeederName,
                                                                lLoadDatePersian, lLoadValue1, lLoadValue2,
                                                                lLoadValue3, lLoadValue4, lLoadValue5, lLoadValue6, lLoadValue7,
                                                                lLoadValue8, lLoadValue9, lLoadValue10, lLoadValue11, lLoadValue12,
                                                                lLoadValue13, lLoadValue14, lLoadValue15, lLoadValue16, lLoadValue17,
                                                                lLoadValue18, lLoadValue19, lLoadValue20, lLoadValue21, lLoadValue22,
                                                                lLoadValue23, lLoadValue24, lSumLoad})
                    lMPFeederIDs &= IIf(lMPFeederIDs = "", lTblMPFeederRows(0)("MPFeederId"), "," & lTblMPFeederRows(0)("MPFeederId"))
                    If Not lDates.Contains(lRow("RelDatePersian")) Then
                        lDates &= IIf(lDates = "", "'" & lRow("RelDatePersian") & "'", ",'" & lRow("RelDatePersian") & "'")
                    End If
                    lAcceptCount += 1
                    i = +1
                Else
                    lFaildCount += 1
                End If
            Next
            dgMPFeederNew.DataSource = mTbl_MPFeederLoadNew
            dgMPFeederNew.Visible = True
            dgMPFeeder.Visible = False

            If lMPFeederIDs.Length > 0 Then

                BindingTable("SELECT *, CAST(MPFeederLoadId AS Bigint) as MPFeederLoadOldId FROM Tbl_MPFeederLoad where MPFeederId in (" & lMPFeederIDs & ") AND RelDatePersian in (" & lDates & ")", mCnn, mDs, "Tbl_MPFeederLoad", , , , , , , True)
                BindingTable("SELECT Tbl_MPFeederLoadHours.* FROM Tbl_MPFeederLoadHours inner join Tbl_MPFeederLoad on Tbl_MPFeederLoadHours.MPFeederLoadId = Tbl_MPFeederLoad.MPFeederLoadId where MPFeederId in (" & lMPFeederIDs & ") AND RelDatePersian in (" & lDates & ")", mCnn, mDs, "Tbl_MPFeederLoadHours", , , , , , , True)
                mSortedListLoadHour.Clear()
                For Each lRow As DataRow In mDs.Tables("Tbl_MPFeederLoadHours").Rows
                    If Not mSortedListLoadHour.ContainsKey(lRow("MPFeederLoadId") & "-" & lRow("HourId")) Then
                        mSortedListLoadHour.Add(lRow("MPFeederLoadId") & "-" & lRow("HourId"), lRow)
                    End If
                Next
            End If
            lblCount.Text = mTbl_MPFeederLoadNew.Rows.Count
            lMsg = "تعداد فيدر‌هاي قابل قبول : " & lAcceptCount & vbCrLf
            lMsg &= "تعداد فيدر‌هاي غير قابل قبول : " & lFaildCount

            MsgBoxF(lMsg, "ورود از فايل اکسل", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Information, MessageBoxDefaultButton.Button1)

        Catch ex As Exception
            ShowError(ex.ToString)
        End Try
    End Sub
    Private Sub SaveMPFeederLoadInfoNew()
        Dim lNewRow As DataRow
        Dim lHourNewRow As DataRow
        Dim lMPFeederLoadRows() As DataRow
        Dim lMPFeederLoadHoursRow As DataRow
        Dim lMPFeederRow As DataRow
        Dim lKh As New KhayamDateTime
        Dim lHourId As Integer
        Dim lHourRows() As DataRow
        Dim lDate As String = ""
        Dim lTime As String = ""
        Dim lMiladiDT As DateTime = DateTime.Now
        Dim IsNewRow As Boolean = False, IsHourNewRow As Boolean = False
        Dim lTrans As SqlTransaction
        Dim lUpdate As New frmUpdateDataSetBT
        Dim lIsSaveOk As Boolean = False
        Dim lHourCount As Integer = 0, lDateCount As Integer = 0, lFaildCount As Integer = 0
        Dim lMsg As String = ""
        Dim lCurrentValue As String
        Dim lLoadValue As String = ""
        Try
            Dim i As Integer = 1
            For Each lRow As DataRow In mTbl_MPFeederLoadNew.Rows
                For j As Integer = 1 To 24
                    lLoadValue = IIf(Not IsDBNull(lRow("LoadValue" & j)), lRow("LoadValue" & j), "")
                    lDate = ValidDate(lRow("RelDatePersian"))

                    Select Case j
                        Case 24, 25
                            lTime = "00:00"
                        Case Is < 10
                            lTime = "0" & j & ":00"
                        Case Else
                            lTime = j & ":00"
                    End Select
                    i = +1

                    If Val(lLoadValue) >= 0 Then
                        If lTime <> "" Then
                            If lDate <> "Error" And lDate <> "" Then
                                lKh.SetShamsiDate(lDate)
                                lMiladiDT = lKh.GetMiladyDate()
                                lHourRows = mDs.Tables("tbl_Hour").Select("Hour = '" & lTime.Substring(0, 3) & "00'")
                                lHourId = lHourRows(0)("HourId")

                                lMPFeederLoadRows = mDs.Tables("Tbl_MPFeederLoad").Select("MPFeederId = '" & lRow("MPFeederId") & "' AND RelDatePersian = '" & lRow("RelDatePersian") & "'")
                                If lMPFeederLoadRows.Length > 0 Then
                                    lNewRow = lMPFeederLoadRows(0)
                                    IsNewRow = False
                                Else
                                    lNewRow = mDs.Tables("Tbl_MPFeederLoad").NewRow()
                                    lNewRow("MPFeederLoadId") = GetAutoInc()
                                    IsNewRow = True
                                End If

                                BindingTable("SELECT * FROM Tbl_MPFeeder WHERE MPFeederId = " & lRow("MPFeederId"), mCnn, mDs, "Tbl_MPFeeder", , , , , , , True)
                                lMPFeederRow = mDs.Tables("Tbl_MPFeeder").Rows(0)
                                lNewRow("AreaId") = lMPFeederRow("AreaId")
                                lNewRow("MPPostId") = lMPFeederRow("MPPostId")
                                lNewRow("MPFeederId") = lMPFeederRow("MPFeederId")
                                lNewRow("RelDate") = lMiladiDT
                                lNewRow("RelDatePersian") = lDate
                                lNewRow("GrowthPercentage") = 0
                                lNewRow("MPFeederLoadOldId") = lNewRow("MPFeederLoadId")
                                If IsNewRow Then
                                    mDs.Tables("Tbl_MPFeederLoad").Rows.Add(lNewRow)
                                End If
                                lMPFeederLoadHoursRow = mSortedListLoadHour(lNewRow("MPFeederLoadId") & "-" & lHourId)
                                SaveLog("step 2 _ " & DateTime.Now, "testExcel.log")
                                If Not lMPFeederLoadHoursRow Is Nothing Then
                                    lHourNewRow = lMPFeederLoadHoursRow
                                    IsHourNewRow = False
                                Else
                                    lHourNewRow = mDs.Tables("Tbl_MPFeederLoadHours").NewRow()
                                    lHourNewRow("MPFeederLoadHourId") = GetAutoInc()
                                    IsHourNewRow = True
                                End If

                                lHourNewRow("MPFeederLoadId") = lNewRow("MPFeederLoadId")
                                lHourNewRow("HourId") = lHourId
                                lHourNewRow("CurrentValue") = Math.Round(lLoadValue / ((Math.Sqrt(3) * 0.85 * 20000) / 1000000), 3)
                                lHourNewRow("PowerValue") = lLoadValue
                                lHourNewRow("CurrentValueReActive") = 0
                                lHourNewRow("HourExact") = lTime

                                If IsHourNewRow Then
                                    mDs.Tables("Tbl_MPFeederLoadHours").Rows.Add(lHourNewRow)
                                End If
                                lHourCount += 1
                            Else
                                lFaildCount += 1
                            End If

                        Else
                            lFaildCount += 1
                        End If
                    Else
                        lFaildCount += 1
                    End If
                Next
            Next
            lMsg = "تعداد بارگيري ساعت به ساعت : " & lHourCount & vbCrLf &
                    "تعداد رديف داراي خطا : " & lFaildCount
            If MsgBoxF(lMsg & vbCrLf & " آيا اطلاعات بارگيري ذخيره شود؟ ", "تاييد اطلاعات بارگيري", MessageBoxButtons.YesNo, MsgBoxIcon.MsgIcon_Question, MessageBoxDefaultButton.Button1) = DialogResult.No Then
                Exit Sub
            End If

            If mCnn.State <> ConnectionState.Open Then
                mCnn.Open()
            End If

            lTrans = mCnn.BeginTransaction
            mSortedListLoad.Clear()
            For Each lRow As DataRow In mDs.Tables("Tbl_MPFeederLoad").Rows
                mSortedListLoad.Add(lRow("MPFeederLoadOldId"), lRow)
            Next

            lIsSaveOk = lUpdate.UpdateDataSet("Tbl_MPFeederLoad", mDs, , , lTrans)
            Dim lRowLoad As DataRow

            SaveLog("step 3 _ " & DateTime.Now, "testExcel.log")
            If lIsSaveOk Then
                For Each lRow As DataRow In mDs.Tables("Tbl_MPFeederLoadHours").Rows

                    lRowLoad = mSortedListLoad(lRow("MPFeederLoadId"))
                    If Not lRowLoad Is Nothing AndAlso lRow.RowState <> DataRowState.Unchanged Then
                        lRow("MPFeederLoadId") = lRowLoad("MPFeederLoadId")
                    End If
                Next
                lIsSaveOk = lUpdate.UpdateDataSet("Tbl_MPFeederLoadHours", mDs, , , lTrans)
            End If

            SaveLog("step 4 _ " & DateTime.Now, "testExcel.log")
            If lIsSaveOk Then
                lTrans.Commit()
                Dim lDlg As New frmInformUserAction("ذخيره اطلاعات با موفقيت صورت گرفت")
                lDlg.ShowDialog()
                lDlg.Dispose()
                Me.Close()
            Else
                lTrans.Rollback()
            End If
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
    End Sub
    '''''''''''''''''''''''''''''''''''''''''''''''''MPPostEnergy
    Private Sub MakeTableMPPostEnergy()
        mTbl_MPPostEnergy = New DataTable("TblMPPostEnergy")
        mTbl_MPPostEnergy.Columns.Add("MPPostMonthlyId", GetType(Long))
        mTbl_MPPostEnergy.Columns.Add("MPPostId", GetType(Long))
        mTbl_MPPostEnergy.Columns.Add("MPPostCode", GetType(String))
        mTbl_MPPostEnergy.Columns.Add("MPPostName", GetType(String))
        mTbl_MPPostEnergy.Columns.Add("Year", GetType(String))
        mTbl_MPPostEnergy.Columns.Add("Month", GetType(String))
        mTbl_MPPostEnergy.Columns.Add("Energy", GetType(Single))
    End Sub
    Private Function MakeTableMPPostEnergyExcel() As Integer
        Try
            mTbl_MPPostEnergyExcel = New DataTable("Tbl_MPPostEnergyExcel")
            mTbl_MPPostEnergyExcel.Columns.Add("MPPostCode", GetType(String))
            mTbl_MPPostEnergyExcel.Columns.Add("MPPostName", GetType(String))
            mTbl_MPPostEnergyExcel.Columns.Add("Year", GetType(String))
            mTbl_MPPostEnergyExcel.Columns.Add("Month", GetType(String))
            mTbl_MPPostEnergyExcel.Columns.Add("Energy", GetType(Single))

            Dim lTblRowCount As Integer = 0
            Dim lMPPostCode As String = ""
            Dim lMPPostName As String = ""
            Dim lYear As String = ""
            Dim lMonth As String = ""
            Dim lEnergy As String = ""
            Dim lLastNullRecord As Integer = 0
            Dim i As Integer = 3
            Do
                lMPPostCode = mExcel.ReadCellRC(mSheet, i, 2)

                If lMPPostCode = "" Then
                    lLastNullRecord += 1
                Else
                    lTblRowCount += 1
                    lLastNullRecord = 0
                End If

                If lLastNullRecord >= 5 Then
                    Exit Do
                End If
                i += 1
            Loop While True

            i = 3

            pg.Maximum = lTblRowCount
            Do
                lMPPostCode = mExcel.ReadCell(mSheet, "B" & i)
                lMPPostName = mExcel.ReadCell(mSheet, "C" & i)
                lYear = mExcel.ReadCell(mSheet, "D" & i)
                lMonth = mExcel.ReadCell(mSheet, "E" & i)
                lEnergy = Val(mExcel.ReadCell(mSheet, "F" & i))
                AdvanceProgress(pg)

                If lMPPostCode = "" Then
                    lLastNullRecord += 1
                Else
                    mTbl_MPPostEnergyExcel.Rows.Add(New Object() {lMPPostCode, lMPPostName, lYear, lMonth, lEnergy})
                    lTblRowCount += 1
                    lLastNullRecord = 0
                End If

                If lLastNullRecord >= 5 Then
                    Exit Do
                End If
                i += 1
            Loop While True

            pg.Visible = False

            Return lTblRowCount
        Catch ex As Exception
            ShowError(ex.ToString)
        End Try

    End Function
    Private Sub FillMPPostEnergyDataGrid()
        Dim lTblMPPostRows() As DataRow
        Dim lMPPostMonthlyId As Long
        Dim lMPPostId As Integer
        Dim lMPPostCode As String
        Dim lMPPostName As String
        Dim lYear As String
        Dim lMonth As String
        Dim lEnergy As Single
        Dim lAcceptCount As Integer = 0, lFaildCount As Integer = 0
        Dim lMsg As String = ""

        Try
            For Each lRow As DataRow In mTbl_MPPostEnergyExcel.Rows
                lTblMPPostRows = mDs.Tables("Tbl_MPPost").Select("MPPostCode = '" & lRow("MPPostCode") & "' AND IsActive = 1 ")
                If lTblMPPostRows.Length > 0 Then
                    lMPPostMonthlyId = GetAutoInc()
                    lMPPostId = lTblMPPostRows(0)("MPPostId")
                    lMPPostCode = lRow("MPPostCode")
                    lMPPostName = lTblMPPostRows(0)("MPPostName")
                    lYear = lRow("Year")
                    lMonth = lRow("Month")
                    lEnergy = lRow("Energy")
                    mTbl_MPPostEnergy.Rows.Add(New Object() {lMPPostMonthlyId, lMPPostId, lMPPostCode, lMPPostName,
                                                                 lYear, lMonth, lEnergy})
                    lAcceptCount += 1
                Else
                    lFaildCount += 1
                End If
            Next
            dgMPPostEnergy.DataSource = mTbl_MPPostEnergy
            lblCount.Text = mTbl_MPPostEnergy.Rows.Count
            lMsg = "تعداد پست‌هاي قابل قبول : " & lAcceptCount & vbCrLf
            lMsg &= "تعداد پست‌هاي غير قابل قبول : " & lFaildCount

            MsgBoxF(lMsg, "ورود از فايل اکسل", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Information, MessageBoxDefaultButton.Button1)

        Catch ex As Exception
            ShowError(ex.ToString)
        End Try
    End Sub
    Private Sub SaveMPPostEnergyInfo()
        Dim lNewRow As DataRow
        Dim lMPPostRows() As DataRow
        Dim lMPPostRow As DataRow
        Dim lKh As New KhayamDateTime
        Dim lYear As String = "", lMonth As String, lMiladiDT As DateTime = DateTime.Now
        Dim IsNewRow As Boolean = False
        Dim lTrans As SqlTransaction
        Dim lUpdate As New frmUpdateDataSetBT
        Dim lIsSaveOk As Boolean = False
        Dim lAcceptCount As Integer = 0, lFaildCount As Integer = 0
        Dim lMsg As String = ""
        Dim lEnergy As String

        Try
            For Each lRow As DataRow In mTbl_MPPostEnergy.Rows
                lYear = ValidYear(lRow("Year"))
                lMonth = ValidMonth(lRow("Month"))
                If lYear <> "Error" And lMonth <> "Error" Then
                    lMPPostRows = mDs.Tables("TblMPPostMonthly").Select("MPPostId = " & lRow("MPPostId") & " AND YearMonth = '" & lYear & "/" & lMonth & "'")
                    If lMPPostRows.Length > 0 Then
                        lNewRow = lMPPostRows(0)
                        IsNewRow = False
                    Else
                        lNewRow = mDs.Tables("TblMPPostMonthly").NewRow()
                        lNewRow("MPPostMonthlyId") = GetAutoInc()
                        IsNewRow = True
                    End If
                    lNewRow("MPPostId") = lRow("MPPostId")
                    lNewRow("YearMonth") = lYear & "/" & lMonth
                    lNewRow("Energy") = GetFieldValue(lRow("Energy"))
                    If IsNewRow Then
                        mDs.Tables("TblMPPostMonthly").Rows.Add(lNewRow)
                    End If
                    lAcceptCount += 1
                Else
                    lFaildCount += 1
                End If
            Next


            lMsg = "تعداد انرژي تحويلي : " & lAcceptCount & vbCrLf &
                    "تعداد رديف داراي خطا : " & lFaildCount
            If MsgBoxF(lMsg & vbCrLf & " آيا اطلاعات انرژي تحويلي ذخيره شود؟ ", "تاييد اطلاعات انرژي تحويلي", MessageBoxButtons.YesNo, MsgBoxIcon.MsgIcon_Question, MessageBoxDefaultButton.Button1) = DialogResult.No Then
                Exit Sub
            End If

            If mCnn.State <> ConnectionState.Open Then
                mCnn.Open()
            End If

            lTrans = mCnn.BeginTransaction

            lIsSaveOk = lUpdate.UpdateDataSet("TblMPPostMonthly", mDs, , , lTrans)

            If lIsSaveOk Then
                lTrans.Commit()
                Dim lDlg As New frmInformUserAction("ذخيره اطلاعات با موفقيت صورت گرفت")
                lDlg.ShowDialog()
                lDlg.Dispose()
                Me.Close()
            Else
                lTrans.Rollback()
            End If
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
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''MPFeederEnergy
    Private Sub MakeTableMPFeederEnergy()
        mTbl_MPFeederEnergy = New DataTable("TblMPFeederEnergy")
        mTbl_MPFeederEnergy.Columns.Add("MPFeederPeakId", GetType(Long))
        mTbl_MPFeederEnergy.Columns.Add("MPFeederId", GetType(Long))
        mTbl_MPFeederEnergy.Columns.Add("MPFeederCode", GetType(String))
        mTbl_MPFeederEnergy.Columns.Add("MPFeederName", GetType(String))
        mTbl_MPFeederEnergy.Columns.Add("Year", GetType(String))
        mTbl_MPFeederEnergy.Columns.Add("Month", GetType(String))
        mTbl_MPFeederEnergy.Columns.Add("Energy", GetType(Single))
    End Sub
    Private Function MakeTableMPFeederEnergyExcel() As Integer
        Try
            mTbl_MPFeederEnergyExcel = New DataTable("Tbl_MPFeederEnergyExcel")
            mTbl_MPFeederEnergyExcel.Columns.Add("MPFeederCode", GetType(String))
            mTbl_MPFeederEnergyExcel.Columns.Add("MPFeederName", GetType(String))
            mTbl_MPFeederEnergyExcel.Columns.Add("Year", GetType(String))
            mTbl_MPFeederEnergyExcel.Columns.Add("Month", GetType(String))
            mTbl_MPFeederEnergyExcel.Columns.Add("Energy", GetType(Single))

            Dim lTblRowCount As Integer = 0
            Dim lMPFeederCode As String = ""
            Dim lMPFeederName As String = ""
            Dim lYear As String = ""
            Dim lMonth As String = ""
            Dim lEnergy As String = ""
            Dim lLastNullRecord As Integer = 0
            Dim i As Integer = 3
            Do
                lMPFeederCode = mExcel.ReadCellRC(mSheet, i, 2)

                If lMPFeederCode = "" Then
                    lLastNullRecord += 1
                Else
                    lTblRowCount += 1
                    lLastNullRecord = 0
                End If

                If lLastNullRecord >= 5 Then
                    Exit Do
                End If
                i += 1
            Loop While True

            i = 3

            pg.Maximum = lTblRowCount
            Do
                lMPFeederCode = mExcel.ReadCell(mSheet, "B" & i)
                lMPFeederName = mExcel.ReadCell(mSheet, "C" & i)
                lYear = mExcel.ReadCell(mSheet, "D" & i)
                lMonth = mExcel.ReadCell(mSheet, "E" & i)
                lEnergy = Val(mExcel.ReadCell(mSheet, "F" & i))
                AdvanceProgress(pg)

                If lMPFeederCode = "" Then
                    lLastNullRecord += 1
                Else
                    mTbl_MPFeederEnergyExcel.Rows.Add(New Object() {lMPFeederCode, lMPFeederName, lYear, lMonth, lEnergy})
                    lTblRowCount += 1
                    lLastNullRecord = 0
                End If

                If lLastNullRecord >= 5 Then
                    Exit Do
                End If
                i += 1
            Loop While True

            pg.Visible = False

            Return lTblRowCount
        Catch ex As Exception
            ShowError(ex.ToString)
        End Try

    End Function
    Private Sub FillMPFeederEnergyDataGrid()
        Dim lTblMPFeederRows() As DataRow
        Dim lMPFeederPeakId As Long
        Dim lMPFeederId As Integer
        Dim lMPFeederCode As String
        Dim lMPFeederName As String
        Dim lYear As String
        Dim lMonth As String
        Dim lEnergy As Single
        Dim lAcceptCount As Integer = 0, lFaildCount As Integer = 0
        Dim lMsg As String = ""

        Try
            For Each lRow As DataRow In mTbl_MPFeederEnergyExcel.Rows
                lTblMPFeederRows = mDs.Tables("Tbl_MPFeeder").Select("MPFeederCode = '" & lRow("MPFeederCode") & "' AND IsActive = 1 ")
                If lTblMPFeederRows.Length > 0 Then
                    lMPFeederPeakId = GetAutoInc()
                    lMPFeederId = lTblMPFeederRows(0)("MPFeederId")
                    lMPFeederCode = lRow("MPFeederCode")
                    lMPFeederName = lTblMPFeederRows(0)("MPFeederName")
                    lYear = lRow("Year")
                    lMonth = lRow("Month")
                    lEnergy = lRow("Energy")
                    mTbl_MPFeederEnergy.Rows.Add(New Object() {lMPFeederPeakId, lMPFeederId, lMPFeederCode, lMPFeederName,
                                                                 lYear, lMonth, lEnergy})
                    lAcceptCount += 1
                Else
                    lFaildCount += 1
                End If
            Next
            dgMPFeederEnergy.DataSource = mTbl_MPFeederEnergy
            lblCount.Text = mTbl_MPFeederEnergy.Rows.Count
            lMsg = "تعداد فيدرهاي قابل قبول : " & lAcceptCount & vbCrLf
            lMsg &= "تعداد فيدرهاي غير قابل قبول : " & lFaildCount

            MsgBoxF(lMsg, "ورود از فايل اکسل", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Information, MessageBoxDefaultButton.Button1)

        Catch ex As Exception
            ShowError(ex.ToString)
        End Try
    End Sub
    Private Sub SaveMPFeederEnergyInfo()
        Dim lNewRow As DataRow
        Dim lMPFeederRows() As DataRow
        'Dim lMPFeederRow As DataRow
        Dim lKh As New KhayamDateTime
        Dim lYear As String = "", lMonth As String, lMiladiDT As DateTime = DateTime.Now
        Dim IsNewRow As Boolean = False
        Dim lTrans As SqlTransaction
        Dim lUpdate As New frmUpdateDataSetBT
        Dim lIsSaveOk As Boolean = False
        Dim lAcceptCount As Integer = 0, lFaildCount As Integer = 0
        Dim lMsg As String = ""

        Try
            For Each lRow As DataRow In mTbl_MPFeederEnergy.Rows
                lYear = ValidYear(lRow("Year"))
                lMonth = ValidMonth(lRow("Month"))
                If lYear <> "Error" And lMonth <> "Error" Then
                    lMPFeederRows = mDs.Tables("TblMPFeederPeak").Select("MPFeederId = " & lRow("MPFeederId") & " AND YearMonth = '" & lYear & "/" & lMonth & "'")
                    If lMPFeederRows.Length > 0 Then
                        lNewRow = lMPFeederRows(0)
                        IsNewRow = False
                    Else
                        lNewRow = mDs.Tables("TblMPFeederPeak").NewRow()
                        lNewRow("MPFeederPeakId") = GetAutoInc()
                        IsNewRow = True
                        lNewRow("PeakCurrentValue") = DBNull.Value
                        lNewRow("IsDaily") = 0
                        lNewRow("LoadDT") = DBNull.Value
                        lNewRow("LoadDatePersian") = DBNull.Value
                        lNewRow("Voltage") = DBNull.Value
                        lNewRow("PeakSynch") = DBNull.Value
                    End If
                    lNewRow("MPFeederId") = lRow("MPFeederId")
                    lNewRow("YearMonth") = lYear & "/" & lMonth
                    lNewRow("Energy") = GetFieldValue(lRow("Energy"))

                    If IsNewRow Then
                        mDs.Tables("TblMPFeederPeak").Rows.Add(lNewRow)
                    End If
                    lAcceptCount += 1
                Else
                    lFaildCount += 1
                End If
            Next


            lMsg = "تعداد انرژي تحويلي : " & lAcceptCount & vbCrLf &
                    "تعداد رديف داراي خطا : " & lFaildCount
            If MsgBoxF(lMsg & vbCrLf & " آيا اطلاعات انرژي تحويلي ذخيره شود؟ ", "تاييد اطلاعات انرژي تحويلي", MessageBoxButtons.YesNo, MsgBoxIcon.MsgIcon_Question, MessageBoxDefaultButton.Button1) = DialogResult.No Then
                Exit Sub
            End If

            If mCnn.State <> ConnectionState.Open Then
                mCnn.Open()
            End If

            lTrans = mCnn.BeginTransaction

            lIsSaveOk = lUpdate.UpdateDataSet("TblMPFeederPeak", mDs, , , lTrans)

            If lIsSaveOk Then
                lTrans.Commit()
                Dim lDlg As New frmInformUserAction("ذخيره اطلاعات با موفقيت صورت گرفت")
                lDlg.ShowDialog()
                lDlg.Dispose()
                Me.Close()
            Else
                lTrans.Rollback()
            End If
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
    End Sub
    ''''''''''''''''''''''''''''''''''''''''''''''''' LPPostLoad
    Private Function MakeTableLPPostLoadExcel() As Integer
        Try
            Dim lMPTrans As String = ""
            Dim lMPPostCode As String = ""
            Dim lLastNullRecord As Integer = 0
            Dim lTblRowCount As Integer = 0
            Dim lAreaName As String = ""
            Dim lMPPostType As String = ""
            Dim lSumLoad As String
            Dim lMPPostName As String = ""
            Dim lMPPostTransType As String = ""
            Dim lMPPostTransName As String = ""
            Dim lLoadDatePersian As String = ""
            Dim lLoadValue(27) As String
            Dim i As Integer = 3

            mTbl_LPPostLoadExcel = New DataTable("Tbl_MPPostTransLoadExcel")
            mTbl_LPPostLoadExcel.Columns.Add("LPPostGisId", GetType(String))
            mTbl_LPPostLoadExcel.Columns.Add("LPPostId", GetType(Integer))
            mTbl_LPPostLoadExcel.Columns.Add("LPPostName", GetType(String))
            mTbl_LPPostLoadExcel.Columns.Add("LoadDate", GetType(String))
            mTbl_LPPostLoadExcel.Columns.Add("LoadTime", GetType(String))
            mTbl_LPPostLoadExcel.Columns.Add("AmperageKey", GetType(String))
            mTbl_LPPostLoadExcel.Columns.Add("LoadPeak", GetType(String))
            mTbl_LPPostLoadExcel.Columns.Add("RCurrent", GetType(String))
            mTbl_LPPostLoadExcel.Columns.Add("SCurrent", GetType(String))
            mTbl_LPPostLoadExcel.Columns.Add("TCurrent", GetType(String))
            mTbl_LPPostLoadExcel.Columns.Add("NCurrent", GetType(String))

            Do
                lMPPostCode = mExcel.ReadCellRC(mSheet, i, 2)
                lMPTrans = mExcel.ReadCellRC(mSheet, i, 4)

                If lMPTrans + lMPPostCode = "" Then
                    lLastNullRecord += 1
                Else
                    lTblRowCount += 1
                    lLastNullRecord = 0
                End If

                If lLastNullRecord >= 5 Then
                    Exit Do
                End If
                i += 1
            Loop While True
            i = 3
            pg.Maximum = lTblRowCount

            Do
                Dim lRow As DataRow = mTbl_LPPostLoadExcel.NewRow()

                lRow("LPPostGisId") = mExcel.ReadCell(mSheet, "B" & i)
                lRow("LPPostName") = mExcel.ReadCell(mSheet, "C" & i)
                lRow("LoadDate") = mExcel.ReadCell(mSheet, "D" & i)
                lRow("LoadTime") = mExcel.ReadCell(mSheet, "E" & i)
                lRow("RCurrent") = Val(mExcel.ReadCell(mSheet, "F" & i))
                lRow("SCurrent") = Val(mExcel.ReadCell(mSheet, "G" & i))
                lRow("TCurrent") = Val(mExcel.ReadCell(mSheet, "H" & i))
                lRow("NCurrent") = Val(mExcel.ReadCell(mSheet, "I" & i))
                lRow("AmperageKey") = Val(mExcel.ReadCell(mSheet, "J" & i))
                lRow("LoadPeak") = Val(mExcel.ReadCell(mSheet, "K" & i))
                ' AdvanceProgress(pg)

                If lRow("LPPostGisId") + lRow("LPPostName") = "" Then
                    lLastNullRecord += 1
                Else
                    mTbl_LPPostLoadExcel.Rows.Add(lRow)
                    lTblRowCount += 1
                    lLastNullRecord = 0
                End If

                If lLastNullRecord >= 5 Then
                    Exit Do
                End If
                i += 1
            Loop While True

            pg.Visible = False

            Return lTblRowCount
        Catch ex As Exception
            ShowError(ex.ToString)
        End Try

    End Function
    Private Sub MakeTableLPPostLoad()
        mTbl_LPPostLoad = New DataTable("ViewLPPostLoad")
        mTbl_LPPostLoad.Columns.Add("LPPostGisId")
        mTbl_LPPostLoad.Columns.Add("LPPostId")
        mTbl_LPPostLoad.Columns.Add("LPPostName")
        mTbl_LPPostLoad.Columns.Add("LoadTime")
        mTbl_LPPostLoad.Columns.Add("LoadDate")
        mTbl_LPPostLoad.Columns.Add("AmperageKey")
        mTbl_LPPostLoad.Columns.Add("LoadPeak")
        mTbl_LPPostLoad.Columns.Add("RCurrent")
        mTbl_LPPostLoad.Columns.Add("SCurrent")
        mTbl_LPPostLoad.Columns.Add("TCurrent")
        mTbl_LPPostLoad.Columns.Add("NCurrent")
    End Sub
    Private Sub FillLPPostLoadDataGrid()
        Dim lTblLPPostRows() As DataRow
        Dim lTblLPPostInfoRow() As DataRow
        Dim lLPPostId As String
        Dim lLPPostLoadId As Long
        Dim lLPPostGisID As String
        Dim lLPPostName As String
        Dim lLoadTime As String
        Dim lLoadDate As String
        Dim lAmperageKey As Single
        Dim lLoadPeak As Single
        Dim lLoadValue As Single
        Dim lRCurrent As Double
        Dim lSCurrent As Double
        Dim lTCurrent As Double
        Dim lNCurrent As Double
        Dim lAcceptCount As Integer = 0, lFaildCount As Integer = 0
        Dim lMsg As String = ""
        Dim lLPPostIDs As String = "-1"
        Dim lKh As New KhayamDateTime
        Dim lMiladiDT As String
        Try
            For Each lRow As DataRow In mTbl_LPPostLoadExcel.Rows

                lKh.SetShamsiDate(ValidDate(lRow("LoadDate")))
                lMiladiDT = Replace(lKh.GetMiladyDate().ToShortDateString, "/", "-")
                lTblLPPostRows = mDs.Tables("ViewLPPostLoad_").Select("LPPostCode = '" & lRow("LPPostGisId") & "'")
                If lTblLPPostRows.Length = 0 Then
                    Dim lSQL As String = "select Tbl_LPPost.LPPostId,LPPostCode,LPPostName,LoadDateTimePersian,LoadTime,LoadDT,ISNULL(SCurrent,0) AS SCurrent,ISNULL(RCurrent,0) AS RCurrent,ISNULL(TCurrent,0) AS TCurrent," &
                        " ISNULL(NolCurrent,0) AS NolCurrent , ISNULL(KelidCurrent,0) AS KelidCurrent,ISNULL(tbllppostload.PostCapacity,Tbl_LPPost.PostCapacity) AS PostCapacity, TblLPPostLoad.LPPostLoadId " &
                        " from dbo.Tbl_LPPost" &
                        " LEFT join tbllppostload on Tbl_LPPost.LPPostId = TblLPPostLoad.LPPostId " &
                        " where Tbl_LPPost.LPPostCode = '" & lRow("LPPostGisId") & "'"
                    BindingTable(lSQL, mCnn, mDs, "ViewLPPostLoad_")
                    lTblLPPostRows = mDs.Tables("ViewLPPostLoad_").Select("LPPostCode = '" & lRow("LPPostGisId") & "'")
                End If

                If lTblLPPostRows.Length > 0 Then
                    '& "' AND LoadDT >= '" & lMiladiDT & " " & lRow("LoadTime") & "'")

                    '  If lTblLPPostRows.Length = 0 Then
                    lTblLPPostInfoRow = mDs.Tables("ViewLPPostLoad_").Select("LPPostCode = '" & lRow("LPPostGisId") & "' ")
                    lLPPostLoadId = GetAutoInc()
                    lLPPostId = IIf(IsDBNull(lTblLPPostInfoRow(0)("LPPostId")) = True, " ", lTblLPPostInfoRow(0)("LPPostId"))
                    lLPPostGisID = lTblLPPostInfoRow(0)("LPPostCode")
                    lLPPostName = IIf(lTblLPPostInfoRow(0)("LPPostName") = "", " ", lTblLPPostInfoRow(0)("LPPostName"))
                    lLoadTime = ValidTime(lRow("LoadTime"))
                    lLoadDate = IIf(lRow("LoadDate") = "", "_", lRow("LoadDate"))
                    lAmperageKey = lRow("AmperageKey")
                    lLoadPeak = CInt((CInt(lRow("RCurrent")) + CInt(lRow("SCurrent")) + CInt(lRow("TCurrent"))) / 3)
                    lRCurrent = lRow("RCurrent")
                    lSCurrent = lRow("SCurrent")
                    lTCurrent = lRow("TCurrent")
                    lNCurrent = lRow("NCurrent")

                    mTbl_LPPostLoad.Rows.Add(New Object() {lLPPostGisID, lLPPostId, lLPPostName, lLoadTime, lLoadDate,
                                                                  lAmperageKey, lLoadPeak, lRCurrent, lSCurrent, lTCurrent, lNCurrent})

                    lLPPostIDs &= IIf(lLPPostId = 0, lTblLPPostInfoRow(0)("LPPostId"), "," & lTblLPPostInfoRow(0)("LPPostId"))
                End If

                'lAcceptCount += 1
                'Else
                ' lFaildCount += 1
                ' End If
            Next
            dgViewLPPostLoad.DataSource = mTbl_LPPostLoad
            lblCount.Text = mTbl_LPPostLoad.Rows.Count

            SaveLog("step 0 _ " & lLPPostIDs & DateTime.Now, "testExcel.log")
            BindingTable("SELECT *, CAST(LPPostLoadId AS Bigint) as LPPostLoadId FROM TblLPPostLoad where LPPostId in (" & lLPPostIDs & ")", mCnn, mDs, "TblLPPostLoad", , , , , , , True)

            mSortedListLoadHour.Clear()

            SaveLog("step 0.1 _ " & lLPPostIDs & DateTime.Now, "testExcel.log")

            'lMsg = "تعداد پست های قابل قبول : " & lAcceptCount & vbCrLf
            'lMsg &= "تعداد پست های غير قابل قبول : " & lFaildCount

            'MsgBoxF(lMsg, "ورود از فايل اکسل", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Information, MessageBoxDefaultButton.Button1)

        Catch ex As Exception
            ShowError(ex.ToString)
        End Try
    End Sub

    Private Sub SaveLPPostLoadInfo()
        Dim lNewRow As DataRow
        Dim lLPPostRows() As DataRow
        Dim lLPPostLastRow() As DataRow
        Dim lDuplicateRows() As DataRow

        Dim lKh As New KhayamDateTime
        Dim lYear As String = "", lMonth As String, lMiladiDT As String = DateTime.Now
        Dim IsNewRow As Boolean = False
        Dim lTrans As SqlTransaction
        Dim lUpdate As New frmUpdateDataSetBT
        Dim lIsSaveOk As Boolean = False
        Dim lAcceptCount As Integer = 0, lFaildCount As Integer = 0
        Dim lMsg As String = ""
        Dim lEnergy As String

        Dim lLoadDate As String
        Dim lLoadTime As String

        Dim lLoadValue As String
        Dim lLPPostLoadId As Long
        Dim lLPPostGisID As String
        Dim lLPPostId As String
        Dim lLPPostName As String
        Dim lAmperageKey As String
        Dim lLoadPeak As String
        Dim lRCurrent As Double
        Dim lSCurrent As Double
        Dim lTCurrent As Double
        Dim lNCurrent As Double

        Try
            Dim i As Integer = 1
            For Each lRow As DataRow In mTbl_LPPostLoad.Rows
                BindingTable("SELECT Top(1) LPPostId,LoadDateTimePersian,LoadTime, PostCapacity from TblLPPostLoad " &
                                 " where LPPostId = '" & lRow("LPPostId") & "'  " &
                                 " Order By LoadDateTimePersian desc,LoadTime desc", mCnn, mDs, "ViewLastLoad", , , , , , , True)

                If mDs.Tables("ViewLastLoad").Rows.Count > 0 Then
                    If mDs.Tables("ViewLastLoad").Rows(0).Item("LoadDateTimePersian") = lRow("LoadDate") Then
                        If lRow("LoadTime") >= mDs.Tables("ViewLastLoad").Rows(0).Item("LoadTime") Then 'Update

                            lLPPostRows = mDs.Tables("ViewLPPostLoad_").Select("LPPostCode = '" & lRow("LPPostGisId") & "'")
                            Dim lDs As New DataSet
                            BindingTable("select * from TblLPPostLoad where LPPostLoadId = " & lLPPostRows(0)("LPPostLoadId"), mCnn, lDs, "TblLPPostLoad", aIsClearTable:=True)
                            IsNewRow = False
                            lNewRow = lDs.Tables("TblLPPostLoad").Rows(0)
                        Else
                            lFaildCount = lFaildCount + 1
                            Continue For
                        End If
                    ElseIf mDs.Tables("ViewLastLoad").Rows(0).Item("LoadDateTimePersian") < lRow("LoadDate") Then

                        lNewRow = mDs.Tables("TblLPPostLoad").NewRow()
                        lNewRow.Item("LPPostLoadId") = GetAutoInc()
                        lNewRow("PostCapacity") = mDs.Tables("ViewLastLoad").Rows(0)("PostCapacity")
                        IsNewRow = True

                    Else
                        lFaildCount = lFaildCount + 1
                        Continue For
                    End If

                    lNewRow.Item("LoadDateTimePersian") = ValidDate(lRow("LoadDate"))
                    lNewRow.Item("LoadTime") = lRow("LoadTime")
                    lNewRow.Item("LPPostId") = lRow("LPPostId")
                    lNewRow.Item("PostPeakCurrent") = lRow("LoadPeak")
                    lNewRow.Item("SCurrent") = lRow("SCurrent")
                    lNewRow.Item("RCurrent") = lRow("RCurrent")
                    lNewRow.Item("TCurrent") = lRow("TCurrent")
                    lNewRow.Item("NolCurrent") = lRow("NCurrent")
                    lNewRow.Item("KelidCurrent") = lRow("AmperageKey")
                    lKh.SetShamsiDate(ValidDate(lRow("LoadDate")))
                    Dim ltxtDate As New PersianMaskedEditor
                    Dim ltxtTime As New TimeMaskedEditor
                    ltxtDate.Text = lNewRow.Item("LoadDateTimePersian")
                    ltxtTime.Text = lNewRow("LoadTime")
                    ltxtDate.InsertTime(ltxtTime)
                    lMiladiDT = ltxtDate.MiladiDT
                    lNewRow.Item("LoadDT") = lMiladiDT

                    If IsNewRow Then
                        mDs.Tables("TblLPPostLoad").Rows.Add(lNewRow)
                    End If
                    lAcceptCount = lAcceptCount + 1
                Else
                    Dim lDs As New DataSet
                    BindingTable("select ISNULL(PostCapacity,0) as PostCapacity from Tbl_LPPost where LPPostId = " & lRow("LPPostId"), mCnn, lDs, "Tbl_LPPostCapacity")
                    If lDs.Tables("Tbl_LPPostCapacity").Rows.Count > 0 Then
                        lNewRow = mDs.Tables("TblLPPostLoad").NewRow()
                        lNewRow("LPPostLoadId") = GetAutoInc()
                        lNewRow("PostCapacity") = lDs.Tables("Tbl_LPPostCapacity").Rows(0)("PostCapacity")
                        lNewRow("LoadDateTimePersian") = ValidDate(lRow("LoadDate"))
                        lNewRow("LoadTime") = lRow("LoadTime")
                        lNewRow("LPPostId") = lRow("LPPostId")
                        lNewRow("PostPeakCurrent") = lRow("LoadPeak")
                        lNewRow("SCurrent") = lRow("SCurrent")
                        lNewRow("RCurrent") = lRow("RCurrent")
                        lNewRow("TCurrent") = lRow("TCurrent")
                        lNewRow("NolCurrent") = lRow("NCurrent")
                        lNewRow("KelidCurrent") = lRow("AmperageKey")
                        Dim ltxtDate As New PersianMaskedEditor
                        Dim ltxtTime As New TimeMaskedEditor
                        ltxtDate.Text = lNewRow.Item("LoadDateTimePersian")
                        ltxtTime.Text = lNewRow("LoadTime")
                        ltxtDate.InsertTime(ltxtTime)
                        lMiladiDT = ltxtDate.MiladiDT
                        lNewRow.Item("LoadDT") = lMiladiDT

                        mDs.Tables("TblLPPostLoad").Rows.Add(lNewRow)
                        lAcceptCount = lAcceptCount + 1
                    Else
                        lFaildCount = lFaildCount + 1
                        Continue For
                    End If
                End If

            Next

            lMsg = "تعداد بارگيري مورد تایید : " & lAcceptCount & vbCrLf &
                    "تعداد رديف داراي خطا : " & lFaildCount
            If MsgBoxF(lMsg & vbCrLf & " آيا اطلاعات بارگيري ذخيره شود؟ ", "تاييد اطلاعات بارگيري", MessageBoxButtons.YesNo, MsgBoxIcon.MsgIcon_Question, MessageBoxDefaultButton.Button1) = DialogResult.No Then
                Exit Sub
            End If

            If mCnn.State <> ConnectionState.Open Then
                mCnn.Open()
            End If

            lTrans = mCnn.BeginTransaction
            lIsSaveOk = lUpdate.UpdateDataSet("TblLPPostLoad", mDs)

            If lIsSaveOk Then
                lTrans.Commit()
                Dim lDlg As New frmInformUserAction("ذخيره اطلاعات با موفقيت صورت گرفت")
                lDlg.ShowDialog()
                lDlg.Dispose()
                Me.Close()
            Else
                lTrans.Rollback()
            End If

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
    End Sub
    ''''''''''''''''''''''''''''''''''''''''''''''''' LPFeederLoad
    Private Function MakeTableLPFeederLoadExcel() As Integer
        Try
            Dim lLPFeederCode As String = ""
            Dim lLastNullRecord As Integer = 0
            Dim lTblRowCount As Integer = 0
            Dim lAreaName As String = ""
            Dim lSumLoad As String
            Dim lLoadDatePersian As String = ""
            Dim lLoadValue(27) As String
            Dim i As Integer = 3

            mTbl_LPFeederLoadExcel = New DataTable("Tbl_LPFeederLoadExcel")
            mTbl_LPFeederLoadExcel.Columns.Add("LPFeederLoadId", GetType(Long))
            mTbl_LPFeederLoadExcel.Columns.Add("LPFeederCode", GetType(String))
            mTbl_LPFeederLoadExcel.Columns.Add("LPFeederId", GetType(Integer))
            mTbl_LPFeederLoadExcel.Columns.Add("LPFeederName", GetType(String))
            mTbl_LPFeederLoadExcel.Columns.Add("LPPostLoadId", GetType(Long))
            mTbl_LPFeederLoadExcel.Columns.Add("LPPostId", GetType(Integer))
            mTbl_LPFeederLoadExcel.Columns.Add("LoadDate", GetType(String))
            mTbl_LPFeederLoadExcel.Columns.Add("LoadTime", GetType(String))
            mTbl_LPFeederLoadExcel.Columns.Add("RFuseId", GetType(String))
            mTbl_LPFeederLoadExcel.Columns.Add("RFuse", GetType(String))
            mTbl_LPFeederLoadExcel.Columns.Add("SFuseId", GetType(String))
            mTbl_LPFeederLoadExcel.Columns.Add("SFuse", GetType(String))
            mTbl_LPFeederLoadExcel.Columns.Add("TFuseId", GetType(String))
            mTbl_LPFeederLoadExcel.Columns.Add("TFuse", GetType(String))
            mTbl_LPFeederLoadExcel.Columns.Add("RCurrent", GetType(String))
            mTbl_LPFeederLoadExcel.Columns.Add("SCurrent", GetType(String))
            mTbl_LPFeederLoadExcel.Columns.Add("TCurrent", GetType(String))
            mTbl_LPFeederLoadExcel.Columns.Add("NCurrent", GetType(String))
            mTbl_LPFeederLoadExcel.Columns.Add("FazSatheMaghtaId", GetType(String))
            mTbl_LPFeederLoadExcel.Columns.Add("FazSatheMaghta", GetType(String))
            mTbl_LPFeederLoadExcel.Columns.Add("CountFazSatheMaghta", GetType(String))
            mTbl_LPFeederLoadExcel.Columns.Add("NolSatheMaghtaId", GetType(String))
            mTbl_LPFeederLoadExcel.Columns.Add("NolSatheMaghta", GetType(String))
            mTbl_LPFeederLoadExcel.Columns.Add("CountNolSatheMaghta", GetType(String))
            mTbl_LPFeederLoadExcel.Columns.Add("spcCableTypeId", GetType(String))
            mTbl_LPFeederLoadExcel.Columns.Add("CableType", GetType(String))
            mTbl_LPFeederLoadExcel.Columns.Add("KelidCurrent", GetType(String))
            mTbl_LPFeederLoadExcel.Columns.Add("EarthValue", GetType(String))
            mTbl_LPFeederLoadExcel.Columns.Add("LPPostCode", GetType(String))
            mTbl_LPFeederLoadExcel.Columns.Add("LPPostName", GetType(String))
            mTbl_LPFeederLoadExcel.Columns.Add("Area", GetType(String))

            Do
                lLPFeederCode = mExcel.ReadCellRC(mSheet, i, 2)
                If lLPFeederCode = "" Then
                    lLastNullRecord += 1
                Else
                    lTblRowCount += 1
                    lLastNullRecord = 0
                End If

                If lLastNullRecord >= 5 Then
                    Exit Do
                End If
                i += 1
            Loop While True
            i = 3
            pg.Maximum = lTblRowCount

            Dim lDs As New DataSet
            Dim lRows() As DataRow
            BindingTable("select * from Tbl_Fuse ", mCnn, lDs, "Tbl_Fuse", aIsClearTable:=True)
            BindingTable("select * from Tbl_SatheMaghta", mCnn, lDs, "Tbl_SatheMaghta", aIsClearTable:=True)
            BindingTable("select * from TblSpec where SpectypeId = 53", mCnn, lDs, "Tbl_CableType", aIsClearTable:=True)
            Dim lSQL As String =
                "select Tbl_LPFeeder.LPFeederId, Tbl_LPFeeder.LPFeederCode, Tbl_LPFeeder.LPFeederName, Tbl_LPFeeder.LPPostId, " &
                " Tbl_LPPost.LPPostName, Tbl_LPPost.LPPostCode, Tbl_Area.Area, Tbl_Area.AreaId from Tbl_LPFeeder " &
                " inner join Tbl_LPPost on Tbl_LPFeeder.LPPostId = Tbl_LPPost.LPPostId " &
                " inner join Tbl_Area on Tbl_LPFeeder.AreaId = Tbl_Area.AreaId " &
                " where ISNULL(LPFeederCode,'') <> ''"
            BindingTable(lSQL, mCnn, lDs, "Tbl_LPFeeder", aIsClearTable:=True)

            Do
                Dim lRow As DataRow = mTbl_LPFeederLoadExcel.NewRow()

                lRow("LPFeederCode") = mExcel.ReadCell(mSheet, "B" & i)
                lRow("LoadDate") = mExcel.ReadCell(mSheet, "D" & i)
                lRow("LoadTime") = mExcel.ReadCell(mSheet, "E" & i)
                lRow("RFuse") = Val(mExcel.ReadCell(mSheet, "F" & i))
                lRows = lDs.Tables("Tbl_Fuse").Select("Fuse = '" & lRow("RFuse") & "'")
                If lRows.Length > 0 Then
                    lRow("RFuseId") = lRows(0)("FuseId")
                Else
                    lRow("RFuse") = DBNull.Value
                End If
                lRow("SFuse") = Val(mExcel.ReadCell(mSheet, "G" & i))
                lRows = lDs.Tables("Tbl_Fuse").Select("Fuse = '" & lRow("SFuse") & "'")
                If lRows.Length > 0 Then
                    lRow("SFuseId") = lRows(0)("FuseId")
                Else
                    lRow("SFuse") = DBNull.Value
                End If
                lRow("TFuse") = Val(mExcel.ReadCell(mSheet, "H" & i))
                lRows = lDs.Tables("Tbl_Fuse").Select("Fuse = '" & lRow("TFuse") & "'")
                If lRows.Length > 0 Then
                    lRow("TFuseId") = lRows(0)("FuseId")
                Else
                    lRow("TFuse") = DBNull.Value
                End If
                lRow("RCurrent") = Val(mExcel.ReadCell(mSheet, "I" & i))
                lRow("SCurrent") = Val(mExcel.ReadCell(mSheet, "J" & i))
                lRow("TCurrent") = Val(mExcel.ReadCell(mSheet, "K" & i))
                lRow("NCurrent") = Val(mExcel.ReadCell(mSheet, "L" & i))
                lRow("FazSatheMaghta") = mExcel.ReadCell(mSheet, "M" & i)
                lRows = lDs.Tables("Tbl_SatheMaghta").Select("SatheMaghta = '" & lRow("FazSatheMaghta") & "'")
                If lRows.Length > 0 Then
                    lRow("FazSatheMaghtaId") = lRows(0)("SatheMaghtaId")
                Else
                    lRow("FazSatheMaghta") = DBNull.Value
                End If
                lRow("CountFazSatheMaghta") = Val(mExcel.ReadCell(mSheet, "N" & i))
                lRow("NolSatheMaghta") = mExcel.ReadCell(mSheet, "O" & i)
                lRows = lDs.Tables("Tbl_SatheMaghta").Select("SatheMaghta = '" & lRow("NolSatheMaghta") & "'")
                If lRows.Length > 0 Then
                    lRow("NolSatheMaghtaId") = lRows(0)("SatheMaghtaId")
                Else
                    lRow("NolSatheMaghta") = DBNull.Value
                End If
                lRow("CountNolSatheMaghta") = Val(mExcel.ReadCell(mSheet, "P" & i))
                lRow("CableType") = mExcel.ReadCell(mSheet, "Q" & i)
                lRows = lDs.Tables("Tbl_CableType").Select("SpecValue = '" & lRow("CableType") & "'")
                If lRows.Length > 0 Then
                    lRow("spcCableTypeId") = lRows("SpecId")
                Else
                    lRow("CableType") = DBNull.Value
                End If

                If Val(mExcel.ReadCell(mSheet, "R" & i)) > 0 Then
                    lRow("KelidCurrent") = Val(mExcel.ReadCell(mSheet, "R" & i))
                End If
                If Val(mExcel.ReadCell(mSheet, "S" & i)) > 0 Then
                    lRow("EarthValue") = Val(mExcel.ReadCell(mSheet, "S" & i))
                End If

                AdvanceProgress(pg)

                If lRow("LPFeederCode") = "" Then
                    lLastNullRecord += 1
                Else
                    lRows = lDs.Tables("Tbl_LPFeeder").Select("LPFeederCode = '" & lRow("LPFeederCode") & "'")
                    If lRows.Length > 0 Then
                        lRow("LPFeederId") = lRows(0)("LPFeederId")
                        lRow("LPPostId") = lRows(0)("LPPostId")
                        lRow("LPPostName") = lRows(0)("LPPostName")
                        lRow("Area") = lRows(0)("Area")
                        lRow("LPPostCode") = lRows(0)("LPPostCode")
                        lRow("LPFeederName") = lRows(0)("LPFeederName")
                        mTbl_LPFeederLoadExcel.Rows.Add(lRow)
                        lTblRowCount += 1
                        lLastNullRecord = 0
                    End If
                End If

                If lLastNullRecord >= 5 Then
                    Exit Do
                End If
                i += 1
            Loop While True

            pg.Visible = False

            Return lTblRowCount
        Catch ex As Exception
            ShowError(ex.ToString)
        End Try

    End Function
    Private Sub MakeTableLPFeederLoad()
        mTbl_LPFeederLoad = New DataTable("ViewLPFeederLoad")
        mTbl_LPFeederLoad.Columns.Add("LPFeederLoadId")
        mTbl_LPFeederLoad.Columns.Add("LPFeederCode")
        mTbl_LPFeederLoad.Columns.Add("LPFeederId")
        mTbl_LPFeederLoad.Columns.Add("LPFeederName")
        mTbl_LPFeederLoad.Columns.Add("LPPostId")
        mTbl_LPFeederLoad.Columns.Add("LPPostLoadId")
        mTbl_LPFeederLoad.Columns.Add("LoadDate")
        mTbl_LPFeederLoad.Columns.Add("LoadTime")
        mTbl_LPFeederLoad.Columns.Add("RFuseId")
        mTbl_LPFeederLoad.Columns.Add("RFuse")
        mTbl_LPFeederLoad.Columns.Add("SFuseId")
        mTbl_LPFeederLoad.Columns.Add("SFuse")
        mTbl_LPFeederLoad.Columns.Add("TFuseId")
        mTbl_LPFeederLoad.Columns.Add("TFuse")
        mTbl_LPFeederLoad.Columns.Add("RCurrent")
        mTbl_LPFeederLoad.Columns.Add("SCurrent")
        mTbl_LPFeederLoad.Columns.Add("TCurrent")
        mTbl_LPFeederLoad.Columns.Add("NCurrent")
        mTbl_LPFeederLoad.Columns.Add("FazSatheMaghtaId")
        mTbl_LPFeederLoad.Columns.Add("FazSatheMaghta")
        mTbl_LPFeederLoad.Columns.Add("CountFazSatheMaghta")
        mTbl_LPFeederLoad.Columns.Add("NolSatheMaghtaId")
        mTbl_LPFeederLoad.Columns.Add("NolSatheMaghta")
        mTbl_LPFeederLoad.Columns.Add("CountNolSatheMaghta")
        mTbl_LPFeederLoad.Columns.Add("SpcCableTypeId")
        mTbl_LPFeederLoad.Columns.Add("CableType")
        mTbl_LPFeederLoad.Columns.Add("KelidCurrent")
        mTbl_LPFeederLoad.Columns.Add("EarthValue")
        mTbl_LPFeederLoad.Columns.Add("LPPostCode")
        mTbl_LPFeederLoad.Columns.Add("LPPostName")
        mTbl_LPFeederLoad.Columns.Add("Area")
    End Sub
    Private Sub FillLPFeederLoadDataGrid()
        Dim lTblLPFeederRows() As DataRow
        Dim lTblLPPostRows() As DataRow
        Dim lLPFeederId As Integer
        Dim lLPPostId As Integer
        Dim lLPFeederLoadId As Long
        Dim lLPPostLoadId As Long

        Dim lLoadTime As String
        Dim lLoadDate As String
        Dim lKelidCurrent As Double
        Dim lPostPeakCurrent As Double
        Dim lRCurrent As Double
        Dim lSCurrent As Double
        Dim lTCurrent As Double
        Dim lNCurrent As Double
        Dim lRFuseId As Integer
        Dim lSFuseId As Integer
        Dim lTFuseId As Integer

        Dim lFazSatheMaghtaId As Integer
        Dim lCountFazSatheMaghta As Integer
        Dim lNolSatheMaghtaId As Integer
        Dim lCountNolSatheMaghta As Integer


        Dim lAcceptCount As Integer = 0, lFaildCount As Integer = 0
        Dim lMsg As String = ""
        Dim lLPPostIDs As String = "-1"
        Dim lKh As New KhayamDateTime
        Dim lNewRow As DataRow

        mLPFeederLoadIDs.Clear()
        Try
            For Each lRow As DataRow In mTbl_LPFeederLoadExcel.Rows
                Dim lDate As String = ValidDate(lRow("LoadDate"))
                Dim lTime As String = ValidTime(lRow("LoadTime"))

                lTblLPFeederRows = mDs.Tables("ViewLPFeederLastLoad").Select("LPFeederCode = '" & lRow("LPFeederCode") & "' AND LoadDateTimePersian > '" & lDate & "'")
                If lTblLPFeederRows.Length > 0 Then Continue For

                Dim lIsFind As Boolean = False
                lTblLPFeederRows = mDs.Tables("ViewLPFeederLastLoad").Select("LPFeederCode = '" & lRow("LPFeederCode") & "' AND LoadDateTimePersian = '" & lDate & "' AND LoadTime = '" & lTime & "'")
                If lTblLPFeederRows.Length > 0 Then
                    lNewRow = mTbl_LPFeederLoad.NewRow()
                    lIsFind = True
                End If

                If Not lIsFind Then
                    lTblLPFeederRows = mDs.Tables("ViewLPFeederLastLoad").Select("LPFeederCode = '" & lRow("LPFeederCode") & "' AND LoadDateTimePersian = '" & lDate & "'")
                    If lTblLPFeederRows.Length > 0 Then
                        lNewRow = mTbl_LPFeederLoad.NewRow()
                        lIsFind = True
                    End If
                End If
                If lIsFind Then
                    mLPFeederLoadIDs.Add(lTblLPFeederRows(0)("LPFeederLoadId"))
                    lNewRow("LPFeederLoadId") = lTblLPFeederRows(0)("LPFeederLoadId")
                    lNewRow("LPFeederCode") = lTblLPFeederRows(0)("LPFeederCode")
                    lNewRow("LPFeederId") = lTblLPFeederRows(0)("LPFeederId")
                    lNewRow("LPFeederName") = lTblLPFeederRows(0)("LPFeederName")
                    lNewRow("LPPostCode") = lRow("LPPostCode")
                    lNewRow("LPPostName") = lRow("LPPostName")
                    lNewRow("Area") = lRow("Area")
                    lNewRow("LPPostLoadId") = lTblLPFeederRows(0)("LPPostLoadId")
                    lNewRow("LoadDate") = lDate
                    lNewRow("LoadTime") = lTime
                    lNewRow("RFuseId") = lRow("RFuseId")
                    lNewRow("RFuse") = lRow("RFuse")
                    lNewRow("SFuseId") = lRow("SFuseId")
                    lNewRow("SFuse") = lRow("SFuse")
                    lNewRow("TFuseId") = lRow("TFuseId")
                    lNewRow("TFuse") = lRow("TFuse")
                    lNewRow("RCurrent") = lRow("RCurrent")
                    lNewRow("SCurrent") = lRow("SCurrent")
                    lNewRow("TCurrent") = lRow("TCurrent")
                    lNewRow("NCurrent") = lRow("NCurrent")
                    lNewRow("FazSatheMaghtaId") = lRow("FazSatheMaghtaId")
                    lNewRow("FazSatheMaghta") = lRow("FazSatheMaghta")
                    lNewRow("CountFazSatheMaghta") = lRow("CountFazSatheMaghta")
                    lNewRow("NolSatheMaghtaId") = lRow("NolSatheMaghtaId")
                    lNewRow("NolSatheMaghta") = lRow("NolSatheMaghta")
                    lNewRow("CountNolSatheMaghta") = lRow("CountNolSatheMaghta")
                    lNewRow("SpcCableTypeId") = lRow("SpcCableTypeId")
                    lNewRow("CableType") = lRow("CableType")
                    lNewRow("KelidCurrent") = lRow("KelidCurrent")
                    lNewRow("EarthValue") = lRow("EarthValue")
                    mTbl_LPFeederLoad.Rows.Add(lNewRow)

                Else
                    lTblLPPostRows = mDs.Tables("ViewLPPostLastLoad").Select("LPPostId = " & lRow("LPPostId") & " AND LoadDateTimePersian = '" & lDate & "'")
                    If lTblLPPostRows.Length > 0 Then
                        lNewRow = mTbl_LPFeederLoad.NewRow()
                        lNewRow("LPFeederLoadId") = GetAutoInc()
                        lNewRow("LPFeederCode") = lRow("LPFeederCode")
                        lNewRow("LPFeederId") = lRow("LPFeederId")
                        lNewRow("LPFeederName") = lRow("LPFeederName")
                        lNewRow("LPPostCode") = lRow("LPPostCode")
                        lNewRow("LPPostName") = lRow("LPPostName")
                        lNewRow("Area") = lRow("Area")
                        lNewRow("LPPostLoadId") = lTblLPPostRows(0)("LPPostLoadId")
                        lNewRow("LoadDate") = lDate
                        lNewRow("LoadTime") = lTime
                        lNewRow("RFuseId") = lRow("RFuseId")
                        lNewRow("RFuse") = lRow("RFuse")
                        lNewRow("SFuseId") = lRow("SFuseId")
                        lNewRow("SFuse") = lRow("SFuse")
                        lNewRow("TFuseId") = lRow("TFuseId")
                        lNewRow("TFuse") = lRow("TFuse")
                        lNewRow("RCurrent") = lRow("RCurrent")
                        lNewRow("SCurrent") = lRow("SCurrent")
                        lNewRow("TCurrent") = lRow("TCurrent")
                        lNewRow("NCurrent") = lRow("NCurrent")
                        lNewRow("FazSatheMaghtaId") = lRow("FazSatheMaghtaId")
                        lNewRow("FazSatheMaghta") = lRow("FazSatheMaghta")
                        lNewRow("CountFazSatheMaghta") = lRow("CountFazSatheMaghta")
                        lNewRow("NolSatheMaghtaId") = lRow("NolSatheMaghtaId")
                        lNewRow("NolSatheMaghta") = lRow("NolSatheMaghta")
                        lNewRow("CountNolSatheMaghta") = lRow("CountNolSatheMaghta")
                        lNewRow("SpcCableTypeId") = lRow("SpcCableTypeId")
                        lNewRow("CableType") = lRow("CableType")
                        lNewRow("KelidCurrent") = lRow("KelidCurrent")
                        lNewRow("EarthValue") = lRow("EarthValue")
                        mTbl_LPFeederLoad.Rows.Add(lNewRow)
                    End If
                End If
            Next

            dgLPFeederLoad.DataSource = mTbl_LPFeederLoad
            lblCount.Text = mTbl_LPFeederLoad.Rows.Count

            Dim lLPFeederLoadIDs As String
            For Each aLPFeederLoadId As Long In mLPFeederLoadIDs
                lLPFeederLoadIDs &= IIf(lLPFeederLoadIDs = "", aLPFeederLoadId, "," & aLPFeederLoadId)
            Next
            If lLPFeederLoadIDs = "" Then
                lLPFeederLoadIDs = "-1"
            End If
            BindingTable("SELECT * FROM TblLPFeederLoad where LPFeederLoadId IN (" & lLPFeederLoadIDs & ")", mCnn, mDs, "TblLPFeederLoad", , , , , , , True)

        Catch ex As Exception
            ShowError(ex.ToString)
        End Try
    End Sub

    Private Sub SaveLPFeederLoadInfo()
        Dim lNewRow As DataRow
        Dim lKh As New KhayamDateTime
        Dim lIsNew As Boolean = False

        Try
            Dim i As Integer = 1
            Dim lRows() As DataRow
            For Each lRow As DataRow In mTbl_LPFeederLoad.Rows
                lRows = mDs.Tables("TblLPFeederLoad").Select("LPFeederLoadId=" & lRow("LPFeederLoadId"))
                If lRows.Length > 0 Then
                    lNewRow = lRows(0)
                    lIsNew = False
                Else
                    lNewRow = mDs.Tables("TblLPFeederLoad").NewRow()
                    lIsNew = True
                End If

                lNewRow("LPFeederLoadId") = lRow("LPFeederLoadId")
                lNewRow("LPFeederId") = lRow("LPFeederId")
                lNewRow("LPPostLoadId") = lRow("LPPostLoadId")
                lNewRow("LoadDateTimePersian") = lRow("LoadDate")
                lNewRow("LoadTime") = lRow("LoadTime")
                Dim ltxtDate As New PersianMaskedEditor
                Dim ltxtTime As New TimeMaskedEditor
                ltxtDate.Text = lNewRow("LoadDateTimePersian")
                ltxtTime.Text = lNewRow("LoadTime")
                ltxtDate.InsertTime(ltxtTime)
                lNewRow.Item("LoadDT") = ltxtDate.MiladiDT
                lNewRow("RFuseId") = lRow("RFuseId")
                lNewRow("SFuseId") = lRow("SFuseId")
                lNewRow("TFuseId") = lRow("TFuseId")
                lNewRow("RCurrent") = lRow("RCurrent")
                lNewRow("SCurrent") = lRow("SCurrent")
                lNewRow("TCurrent") = lRow("TCurrent")
                lNewRow("NolCurrent") = lRow("NCurrent")
                lNewRow("FazSatheMaghtaId") = lRow("FazSatheMaghtaId")
                lNewRow("CountFazSatheMaghta") = lRow("CountFazSatheMaghta")
                lNewRow("NolSatheMaghtaId") = lRow("NolSatheMaghtaId")
                lNewRow("CountNolSatheMaghta") = lRow("CountNolSatheMaghta")
                lNewRow("SpcCableTypeId") = lRow("SpcCableTypeId")
                lNewRow("KelidCurrent") = lRow("KelidCurrent")
                lNewRow("EarthValue") = lRow("EarthValue")
                lNewRow("FeederPeakCurrent") = Math.Round((Val(lRow("RCurrent")) + Val(lRow("SCurrent")) + Val(lRow("TCurrent"))) / 3)
                lNewRow("AreaUserId") = WorkingUserId

                lNewRow("DEDT") = DateTime.Now
                lNewRow("DEDatePersian") = GetPersianDate(DateTime.Now)
                lNewRow("DETime") = GetTime(DateTime.Now)

                If lIsNew Then
                    mDs.Tables("TblLPFeederLoad").Rows.Add(lNewRow)
                End If
            Next

            Dim lUpdate As New frmUpdateDataSetBT
            lUpdate.UpdateDataSet("TblLPFeederLoad", mDs)

            Dim lDs As New DataSet
            BindingTable("exec spUpdateLPFeederPeak", mCnn, lDs, "TblUpdateLPFeederPeak", aIsClearTable:=True)

            Dim lDlg As New frmInformUserAction("ذخيره اطلاعات با موفقيت صورت گرفت")
            lDlg.ShowDialog()
            lDlg.Dispose()
            Me.Close()

        Catch ex As Exception
            ShowError(ex)
        Finally
            If mCnn.State = ConnectionState.Open Then
                mCnn.Close()
            End If
        End Try
    End Sub
    '''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub MakeTableSubscribers()
        mTbl_Subscribers = New DataTable("Tbl_Subscribers")
        mTbl_Subscribers.Columns.Add("SubscribersId", GetType(Long))
        mTbl_Subscribers.Columns.Add("AreaId", GetType(Integer))
        mTbl_Subscribers.Columns.Add("Area", GetType(String))
        mTbl_Subscribers.Columns.Add("Year", GetType(String))
        mTbl_Subscribers.Columns.Add("Month", GetType(String))
        mTbl_Subscribers.Columns.Add("Count", GetType(Single))
        mTbl_Subscribers.Columns.Add("Ebtali", GetType(Single))
        mTbl_Subscribers.Columns.Add("FeederCount", GetType(Single))
        mTbl_Subscribers.Columns.Add("Energy", GetType(Single))
    End Sub
    Private Function MakeTableSubscribersExcel() As Integer
        Try
            mTbl_SubscribersExcel = New DataTable("Tbl_SubscribersExcel")
            mTbl_SubscribersExcel.Columns.Add("Area", GetType(String))
            mTbl_SubscribersExcel.Columns.Add("Year", GetType(String))
            mTbl_SubscribersExcel.Columns.Add("Month", GetType(String))
            mTbl_SubscribersExcel.Columns.Add("Count", GetType(Single))
            mTbl_SubscribersExcel.Columns.Add("Ebtali", GetType(Single))
            mTbl_SubscribersExcel.Columns.Add("FeederCount", GetType(Single))
            mTbl_SubscribersExcel.Columns.Add("Energy", GetType(Single))

            Dim lTblRowCount As Integer = 0
            Dim lArea As String = ""
            Dim lYear As String = ""
            Dim lMonth As String = ""
            Dim lCount As String = ""
            Dim lEbtali As String = ""
            Dim lLastNullRecord As Integer = 0
            Dim lFeederCount As Integer = 0
            Dim lEnergy As Integer = 0
            Dim i As Integer = 3
            Do
                lArea = mExcel.ReadCellRC(mSheet, i, 2)

                If lArea = "" Then
                    lLastNullRecord += 1
                Else
                    lTblRowCount += 1
                    lLastNullRecord = 0
                End If

                If lLastNullRecord >= 5 Then
                    Exit Do
                End If
                i += 1
            Loop While True

            i = 3

            pg.Maximum = lTblRowCount
            Do
                lArea = mExcel.ReadCell(mSheet, "B" & i)
                lYear = mExcel.ReadCell(mSheet, "C" & i)
                lMonth = mExcel.ReadCell(mSheet, "D" & i)
                lCount = Val(mExcel.ReadCell(mSheet, "E" & i))
                lEbtali = Val(mExcel.ReadCell(mSheet, "F" & i))
                lEnergy = Val(mExcel.ReadCell(mSheet, "G" & i))
                lFeederCount = Val(mExcel.ReadCell(mSheet, "H" & i))
                AdvanceProgress(pg)

                If lArea = "" Then
                    lLastNullRecord += 1
                Else
                    mTbl_SubscribersExcel.Rows.Add(New Object() {lArea, lYear, lMonth, lCount, lEbtali, lFeederCount, lEnergy})
                    lTblRowCount += 1
                    lLastNullRecord = 0
                End If

                If lLastNullRecord >= 5 Then
                    Exit Do
                End If
                i += 1
            Loop While True

            pg.Visible = False

            Return lTblRowCount
        Catch ex As Exception
            ShowError(ex.ToString)
        End Try

    End Function
    Private Sub FillSubscribersDataGrid()
        Dim lTblSubscribersRows() As DataRow
        Dim lSubscribersId As Integer
        Dim lAreaId As Integer
        Dim lArea As String
        Dim lYear As String
        Dim lMonth As String
        Dim lCount As Single
        Dim lEbtali As Single
        Dim lAcceptCount As Integer = 0, lFaildCount As Integer = 0
        Dim lMsg As String = ""
        Dim lFeederCount As Integer
        Dim lEnergy As Integer

        Try
            For Each lRow As DataRow In mTbl_SubscribersExcel.Rows

                For Each lRow_Area As DataRow In mDs.Tables("Tbl_Area").Rows
                    lRow_Area("Area") = ReplaceFarsiToArabi(lRow_Area("Area"))
                Next
                lRow("Area") = ReplaceFarsiToArabi(lRow("Area"))
                lTblSubscribersRows = mDs.Tables("Tbl_Area").Select("Area = '" & lRow("Area") & "'")
                If lTblSubscribersRows.Length > 0 Then
                    lSubscribersId = GetAutoInc()
                    lAreaId = lTblSubscribersRows(0)("AreaId")
                    lYear = lRow("Year")
                    lMonth = lRow("Month")
                    lCount = lRow("Count")
                    lEbtali = lRow("Ebtali")
                    lArea = lRow("Area")
                    lFeederCount = lRow("FeederCount")
                    lEnergy = lRow("Energy")
                    mTbl_Subscribers.Rows.Add(New Object() {lSubscribersId, lAreaId, lArea, lYear,
                                                                  lMonth, lCount, lEbtali, lFeederCount, lEnergy})
                    lAcceptCount += 1
                Else
                    lFaildCount += 1
                End If
            Next
            dgSubscriber.DataSource = mTbl_Subscribers
            lblCount.Text = mTbl_Subscribers.Rows.Count
            lMsg = "تعداد نواحي قابل قبول : " & lAcceptCount & vbCrLf
            lMsg &= "تعداد نواحي غير قابل قبول : " & lFaildCount

            MsgBoxF(lMsg, "ورود از فايل اکسل", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Information, MessageBoxDefaultButton.Button1)

        Catch ex As Exception
            ShowError(ex.ToString)
        End Try
    End Sub
    Private Sub SaveSubscribersInfo()
        Dim lNewRow As DataRow
        Dim lSubscribersRows() As DataRow
        Dim lLastRows() As DataRow
        'Dim lMPFeederRow As DataRow
        Dim lKh As New KhayamDateTime
        Dim lYear As String = "", lMonth As String, lMiladiDT As DateTime = DateTime.Now
        Dim IsNewRow As Boolean = False
        Dim lTrans As SqlTransaction
        Dim lUpdate As New frmUpdateDataset
        Dim lIsSaveOk As Boolean = False
        Dim lAcceptCount As Integer = 0, lFaildCount As Integer = 0
        Dim lMsg As String = ""
        Dim lPrevCount As Integer = 0
        Dim lTotalCount As Integer = 0

        Try
            For Each lRow As DataRow In mTbl_Subscribers.Rows
                lYear = ValidYear(lRow("Year"))
                lMonth = ValidMonth(lRow("Month"))
                If lYear <> "Error" And lMonth <> "Error" Then
                    lSubscribersRows = mDs.Tables("TblSubscribers").Select("AreaId = " & lRow("AreaId") & " AND YearMonth = '" & lYear & "/" & lMonth & "'")
                    If lSubscribersRows.Length > 0 Then
                        lNewRow = lSubscribersRows(0)
                        lNewRow("TotalSubscriberCount") = lSubscribersRows(0)("PrevMonthSubscriberCount") + lRow("Count") - lRow("Ebtali")
                        IsNewRow = False
                    Else
                        lNewRow = mDs.Tables("TblSubscribers").NewRow()
                        lNewRow("SubscribersId") = GetAutoInc()
                        IsNewRow = True

                        lLastRows = mDs.Tables("Tbl_LastUpdateSubscribers").Select("AreaId = " & lRow("AreaId"))
                        Dim lNewLasRow As DataRow
                        If lLastRows.Length > 0 Then
                            lSubscribersRows = mDs.Tables("TblSubscribers").Select("AreaId = " & lRow("AreaId") & " AND YearMonth = '" & lLastRows(0)("YearMonth") & "'")
                            lNewRow("PrevMonthSubscriberCount") = lSubscribersRows(0)("TotalSubscriberCount")
                            lTotalCount = Val(Convert.ToString(lSubscribersRows(0)("TotalSubscriberCount")))
                            lNewRow("TotalSubscriberCount") = lTotalCount + lRow("Count") - lRow("Ebtali")
                            If lYear & "/" & lMonth > lLastRows(0)("YearMonth") Then
                                lLastRows(0)("YearMonth") = lYear & "/" & lMonth
                            End If
                        Else
                            lNewRow("PrevMonthSubscriberCount") = DBNull.Value
                            lNewRow("TotalSubscriberCount") = lRow("Count") - lRow("Ebtali")
                            lNewLasRow = mDs.Tables("Tbl_LastUpdateSubscribers").NewRow()
                            lNewLasRow("AreaId") = lRow("AreaId")
                            lNewLasRow("YearMonth") = lYear & "/" & lMonth
                            mDs.Tables("Tbl_LastUpdateSubscribers").Rows.Add(lNewLasRow)
                        End If
                    End If
                    lNewRow("AreaId") = lRow("AreaId")
                    lNewRow("YearMonth") = lYear & "/" & lMonth
                    lNewRow("NewSubscriberCount") = GetFieldValue(lRow("Count"))
                    lNewRow("Ebtali") = GetFieldValue(lRow("Ebtali"))
                    'lNewRow("FeederCount") = DBNull.Value
                    'lNewRow("Energy") = DBNull.Value
                    lNewRow("FeederCount") = GetFieldValue(lRow("FeederCount"))
                    lNewRow("Energy") = GetFieldValue(lRow("Energy"))

                    If IsNewRow Then
                        mDs.Tables("TblSubscribers").Rows.Add(lNewRow)
                    End If
                    lAcceptCount += 1

                    ComputeTotals(lRow)
                Else
                    lFaildCount += 1
                End If
            Next

            lMsg = "تعداد مشترکين قابل ثبت : " & lAcceptCount & vbCrLf &
                    "تعداد رديف داراي خطا : " & lFaildCount
            If MsgBoxF(lMsg & vbCrLf & " آيا اطلاعات تعداد مشترکين ذخيره شود؟ ", "تاييد اطلاعات مشترکين", MessageBoxButtons.YesNo, MsgBoxIcon.MsgIcon_Question, MessageBoxDefaultButton.Button1) = DialogResult.No Then
                Exit Sub
            End If

            If mCnn.State <> ConnectionState.Open Then
                mCnn.Open()
            End If

            lTrans = mCnn.BeginTransaction

            lIsSaveOk = lUpdate.UpdateDataSet("TblSubscribers", mDs, WorkingAreaId, , , lTrans)

            If lIsSaveOk Then
                lTrans.Commit()
                Dim lDlg As New frmInformUserAction("ذخيره اطلاعات با موفقيت صورت گرفت")
                lDlg.ShowDialog()
                lDlg.Dispose()
                Me.Close()
            Else
                lTrans.Rollback()
            End If
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
    End Sub
    Private Sub ComputeTotals(ByVal aRow As DataRow)
        Dim lRows() As DataRow
        Dim lCenterRows() As DataRow
        Dim lAreaId As String = ""
        Dim lPrev As Integer = 0
        Dim lTotal As Integer = 0
        Dim lCount As Integer = 0
        Dim lEbtali As Integer = 0
        Dim lYearMonth As String = ValidYear(aRow("Year")) & "/" & ValidMonth(aRow("Month"))
        Dim lAllAreaIds As String = ""
        Dim LFeederCount As Integer = 0
        Dim lEnergy As Integer = 0
        Try
            BindingTable("SELECT * FROM Tbl_Area WHERE AreaId = " & aRow("AreaId"), mCnn, mDs, "_Tbl_Area", , , , , , , True)
            If Not IsDBNull(mDs.Tables("_Tbl_Area").Rows(0)("ParentAreaId")) Then
                lCenterRows = mDs.Tables("TblArea").Select("AreaId = " & mDs.Tables("_Tbl_Area").Rows(0)("ParentAreaId"))
                lRows = mDs.Tables("TblArea").Select("ParentAreaId = " & mDs.Tables("_Tbl_Area").Rows(0)("ParentAreaId"))
                For Each lRow As DataRow In lRows
                    lAreaId &= IIf(lAreaId <> "", "," & lRow("AreaId"), lRow("AreaId"))
                Next
                lRows = mDs.Tables("TblSubscribers").Select("AreaId IN (" & lAreaId & ") AND YearMonth = '" & lYearMonth & "'")
                For Each lRow As DataRow In lRows
                    lPrev += GetFieldValue(lRow("PrevMonthSubscriberCount"))
                    lTotal += GetFieldValue(lRow("TotalSubscriberCount"))
                    lCount += GetFieldValue(lRow("NewSubscriberCount"))
                    lEbtali += GetFieldValue(lRow("Ebtali"))
                    LFeederCount += GetFieldValue(lRow("FeederCount"))
                    lEnergy += GetFieldValue(lRow("Energy"))
                Next
                lRows = mDs.Tables("TblSubscribers").Select("AreaId = " & lCenterRows(0)("AreaId") & " AND YearMonth = '" & lYearMonth & "'")
                If lRows.Length > 0 Then
                    lRows(0)("PrevMonthSubscriberCount") = lPrev
                    lRows(0)("TotalSubscriberCount") = lTotal
                    lRows(0)("NewSubscriberCount") = lCount
                    lRows(0)("Ebtali") = lEbtali
                    lRows(0)("FeederCount") = LFeederCount
                    lRows(0)("Energy") = lEnergy
                Else
                    Dim lNewRow As DataRow = mDs.Tables("TblSubscribers").NewRow()
                    lNewRow("SubscribersId") = GetAutoInc()
                    lNewRow("YearMonth") = lYearMonth
                    lNewRow("PrevMonthSubscriberCount") = lPrev
                    lNewRow("TotalSubscriberCount") = lTotal
                    lNewRow("NewSubscriberCount") = lCount
                    lNewRow("AreaId") = lCenterRows(0)("AreaId")
                    'lNewRow("Energy") = DBNull.Value
                    lNewRow("Energy") = lEnergy
                    lNewRow("Ebtali") = lEbtali
                    'lNewRow("FeederCount") = DBNull.Value
                    lNewRow("FeederCount") = LFeederCount
                    mDs.Tables("TblSubscribers").Rows.Add(lNewRow)
                End If
            End If
            lRows = mDs.Tables("TblArea").Select("ParentAreaId IS NULL AND AreaId <> 99")
            For Each lRow As DataRow In lRows
                lAllAreaIds &= IIf(lAllAreaIds <> "", "," & lRow("AreaId"), lRow("AreaId"))
            Next
            lRows = mDs.Tables("TblSubscribers").Select("AreaId IN (" & lAllAreaIds & ") AND YearMonth = '" & lYearMonth & "'")
            lPrev = 0
            lTotal = 0
            lCount = 0
            lEbtali = 0
            LFeederCount = 0
            lEnergy = 0
            For Each lRow As DataRow In lRows
                lPrev += GetFieldValue(lRow("PrevMonthSubscriberCount"))
                lTotal += GetFieldValue(lRow("TotalSubscriberCount"))
                lCount += GetFieldValue(lRow("NewSubscriberCount"))
                lEbtali += GetFieldValue(lRow("Ebtali"))
                LFeederCount += GetFieldValue(lRow("FeederCount"))
                lEnergy += GetFieldValue(lRow("Energy"))
            Next
            lRows = mDs.Tables("TblSubscribers").Select("AreaId = 99 AND YearMonth = '" & lYearMonth & "'")
            If lRows.Length > 0 Then
                lRows(0)("PrevMonthSubscriberCount") = lPrev
                lRows(0)("TotalSubscriberCount") = lTotal
                lRows(0)("NewSubscriberCount") = lCount
                lRows(0)("Ebtali") = lEbtali
                lRows(0)("FeederCount") = LFeederCount
                lRows(0)("Energy") = lEnergy
            Else
                Dim lNewRow As DataRow = mDs.Tables("TblSubscribers").NewRow()
                lNewRow("SubscribersId") = GetAutoInc()
                lNewRow("YearMonth") = lYearMonth
                lNewRow("PrevMonthSubscriberCount") = lPrev
                lNewRow("TotalSubscriberCount") = lTotal
                lNewRow("NewSubscriberCount") = lCount
                lNewRow("AreaId") = 99
                'lNewRow("Energy") = DBNull.Value
                lNewRow("Energy") = lEnergy
                lNewRow("Ebtali") = lEbtali
                'lNewRow("FeederCount") = DBNull.Value
                lNewRow("FeederCount") = LFeederCount
                mDs.Tables("TblSubscribers").Rows.Add(lNewRow)
            End If
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub OldPattern_Click(sender As Object, e As EventArgs) Handles OldPattern.Click
        Try
            mVersionTypeId = Version.OldVer
            Dim lFileName As String = ""
            If mFormType = "" Then
                dg.Visible = True
                dgNewLoad.Visible = False
                If mVersionTypeId = Version.OldVer Then
                    lFileName = "MPPostTransLoad"
                ElseIf mVersionTypeId = Version.NewVer Then
                    lFileName = "MPPostTransLoadNew.xls"
                End If
            ElseIf mFormType = "MPFeederLoad" Then
                lFileName = "MPFeederLoad"
                dgMPFeeder.Visible = True
                dgMPFeederNew.Visible = False
            ElseIf mFormType = "MPPostEnergy" Then
                lFileName = "MPPostEnergy"
            ElseIf mFormType = "MPFeederEnergy" Then
                lFileName = "MPFeederEnergy"
            ElseIf mFormType = "Subscribers" Then
                lFileName = "SubscribersInfo"
            ElseIf mFormType = "ViewLPPostLoad" Then
                lFileName = "LPPostLoad.xls"
            End If
            If mExcel Is Nothing Then
                mExcel = New CExcelManager
            Else
                mExcel.CloseWorkBook()
            End If
            Dim lPath As String = VB6.GetPath & "\Reports\Excel\"
            Dim lDestPath As String = GetUserTempPath() & "Tac\Havades\" & lFileName

            If File.Exists(lDestPath) Then
                File.SetAttributes(lDestPath, FileAttributes.Normal)
            End If

            System.IO.File.Copy(lPath & lFileName, lDestPath, True)
            mExcel.OpenWorkBook(GetUserTempPath() & "Tac\Havades\" & lFileName)
            mExcel.ShowExcel()
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub NewPattern_Click(sender As Object, e As EventArgs) Handles NewPattern.Click
        Try
            mVersionTypeId = Version.NewVer
            Dim lFileName As String = ""
            If mFormType = "" Then
                mSheet = "اصل"
                dg.Visible = False
                dgNewLoad.Visible = True
                lFileName = "MPPostTransLoadNew.xls"
            ElseIf mFormType = "MPFeederLoad" Then
                dgMPFeeder.Visible = False
                dgMPFeederNew.Visible = True
                lFileName = "MPFeederLoadNew.xls"
            End If
            If mExcel Is Nothing Then
                mExcel = New CExcelManager
            Else
                mExcel.CloseWorkBook()
            End If
            Dim lPath As String = VB6.GetPath & "\Reports\Excel\"
            Dim lDestPath As String = GetUserTempPath() & "Tac\Havades\" & lFileName

            If File.Exists(lDestPath) Then
                File.SetAttributes(lDestPath, FileAttributes.Normal)
            End If

            System.IO.File.Copy(lPath & lFileName, lDestPath, True)
            mExcel.OpenWorkBook(GetUserTempPath() & "Tac\Havades\" & lFileName)
            mExcel.ShowExcel()
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub
    ''' '''''''''''''''''''''''''''''''''''''''''''' PostFeederLoad
    '-------------<Omid>---------------
    Private Sub initializeExcel()
        ' ----- Initialize Excel Model DataTable
        mTbl_PostFeederExcel = New DataTable("PostFeederExcel")
        ' ----- Initialize Excel Model Columns
        Me.columns = New List(Of ExcelCol)
        columns.Add(New ExcelCol("LPFeederCode", GetType(String), "B"))
        columns.Add(New ExcelCol("LPFeederName", GetType(String), "C"))
        columns.Add(New ExcelCol("LPPostName", GetType(String), "D"))
        columns.Add(New ExcelCol("LoadDateTimePersian", GetType(String), "E"))
        columns.Add(New ExcelCol("LoadTime", GetType(String), "F"))
        columns.Add(New ExcelCol("RFuse", GetType(String), "G"))
        columns.Add(New ExcelCol("SFuse", GetType(String), "H"))
        columns.Add(New ExcelCol("TFuse", GetType(String), "I"))
        columns.Add(New ExcelCol("FeederRCurrent", GetType(String), "J"))
        columns.Add(New ExcelCol("FeederSCurrent", GetType(String), "K"))
        columns.Add(New ExcelCol("FeederTCurrent", GetType(String), "L"))
        columns.Add(New ExcelCol("FeederTcurrent", GetType(String), "M"))
        columns.Add(New ExcelCol("FeederEndlineVoltage", GetType(String), "N"))
        columns.Add(New ExcelCol("PostRCurrent", GetType(String), "O"))
        columns.Add(New ExcelCol("PostSCurrent", GetType(String), "P"))
        columns.Add(New ExcelCol("PostTCurrent", GetType(String), "Q"))
        columns.Add(New ExcelCol("PostNolCurrent", GetType(String), "R"))
        columns.Add(New ExcelCol("PostvRN", GetType(String), "S"))
        columns.Add(New ExcelCol("PostvSN", GetType(String), "T"))
        columns.Add(New ExcelCol("PostvTN", GetType(String), "U"))
        columns.Add(New ExcelCol("PostvRS", GetType(String), "V"))
        columns.Add(New ExcelCol("PostvTR", GetType(String), "W"))
        columns.Add(New ExcelCol("PostvTS", GetType(String), "X"))
        ' ----- Initialize Excel Model
        Me.excel = New ExcelModel(columns)
        '-------Make DataGrid Visible
    End Sub
    Private Sub ParseExcel()
        Dim i As Integer = 3
        Dim lLastNullRecord As Integer = 0
        Dim LPFeederCode As String
        'Dim lObjList As List(Of String)
        Dim lDataRow As DataRow
        For Each column As ExcelCol In columns
            mTbl_PostFeederExcel.Columns.Add(column.name, column.type)
        Next
        '------- Adding Rows to DataTable & Excel Model
        Try
            Do
                Dim lObjList(columns.Count - 1) As Object
                lDataRow = mTbl_PostFeederExcel.NewRow
                LPFeederCode = mExcel.ReadCellRC(mSheet, i, 2)
                If lLastNullRecord >= 5 Then
                    Exit Do
                End If
                If LPFeederCode = "" Then
                    lLastNullRecord += 1
                    Continue Do
                End If
                'lObjList = New List(Of String)
                Dim row As ExcelRow = New ExcelRow()
                Dim counter As Integer = 0
                For Each column As ExcelCol In columns
                    Dim item As ExcelRowItem = New ExcelRowItem(column)
                    item.value = mExcel.ReadCell(mSheet, column.excelColumn & i)
                    row.Add(item)
                    'lObjList.Add(item.value.ToString())
                    lObjList(counter) = item.value
                    counter += 1
                Next
                excel.rows.Add(row)
                mTbl_PostFeederExcel.Rows.Add(lObjList)
                If lLastNullRecord >= 5 Then
                    Exit Do
                End If
                i += 1
            Loop While True
        Catch e As Exception
            ShowError(e)
        End Try
    End Sub
    Private Function GetIdsFromFeederCodes() As List(Of String)
        Dim codeList As List(Of String) = New List(Of String)
        For Each row As ExcelRow In excel.rows
            For Each item As ExcelRowItem In row.items
                If item.column.name = "LPFeederCode" Then
                    codeList.Add(item.value)
                End If
            Next
        Next
        Return codeList
    End Function
    Private Sub MakeDataSetPostFeeeder01()
        Dim codes As List(Of String) = GetIdsFromFeederCodes()
        Dim lWhere As String = ""
        For Each code As String In codes
            lWhere += ",'" + code + "'"
        Next
        Dim lSQL As String = "SELECT * FROM Tbl_LPFeeder WHERE LPFeederCode IN (" + lWhere.Substring(1) + ")"
        BindingTable(lSQL, mCnn, mDs, "Tbl_LPFeeder", aIsClearTable:=True)
        BindingTable("Select * From Tbl_Fuse", mCnn, mDs, "Tbl_Fuse", aIsClearTable:=True)
    End Sub
    Private Sub MakeDataSetPostFeeeder02()
        Dim lWhere As String = ""
        For Each row As DataRow In mDs.Tables("Tbl_LPFeeder").Rows
            lWhere += "," + row("LPFeederId").ToString()
        Next
        Dim lSQL As String = "EXEC [dbo].[Sp-PostFeederSelect] '" + lWhere.Substring(1) + "' , 1;"
        BindingTable(lSQL, mCnn, mDs, "TblLPFeederLoad", aIsClearTable:=True)
        lSQL = "EXEC [dbo].[Sp-PostFeederSelect] '" + lWhere.Substring(1) + "' , 2;"
        BindingTable(lSQL, mCnn, mDs, "TblLPPostLoad", aIsClearTable:=True)
        lSQL = "EXEC [dbo].[Sp-PostFeederSelect]  '' , 3;"
        BindingTable(lSQL, mCnn, mDs, "Tbl_Fuse", aIsClearTable:=True)
    End Sub

    Private Sub CheckPostFeederDataIntegrity()
        '-----------------Initialize TblFieldsToExcelConverter & TblDBColumns
        InitializeDBColumns()
        Dim lNothingSubstitute As Object = Nothing
        Dim lIsEmpty As Boolean
        Dim excelColExists As Boolean
        Dim excelCol As String
        Dim fuseDict As New Dictionary(Of String, String)
        Dim lFeederRow As DataRow()
        '------------Fill Fuse Dictinary
        For Each row As DataRow In mDs.Tables("Tbl_Fuse").Rows
            If Not fuseDict.ContainsKey(row("Fuse")) Then
                fuseDict.Add(row("Fuse"), row("FuseId"))
            End If
        Next
        pg.Maximum = 2 * mTbl_PostFeederExcel.Rows.Count
        For Each table As String In {"TblLPPostLoad", "TblLPFeederLoad"}
            For Each row As DataRow In mTbl_PostFeederExcel.Rows
                '--------check for existence :
                '----------PostLoad 
                Dim lDtRow1 As DataRow() = mDs.Tables(table).Select("LPFeederCode='" + row("LPFeederCode") + "'" +
                        " AND LoadDateTimePersian='" + row("LoadDateTimePersian") + "'")
                '---------FeederLoad
                Dim lDtRow2 As DataRow() = mDs.Tables(table).Select("LPFeederCode='" + row("LPFeederCode") + "'" +
                        " AND LoadDateTimePersian='" + row("LoadDateTimePersian") + "' AND LoadTime='" + row("LoadTime") + "'")
                '---------Take out LPFeederId ,LPPostId , LPFeederCode
                lFeederRow = mDs.Tables("Tbl_LPFeeder").Select("LPFeederCode = '" & row("LPFeederCode") & "'")
                If lFeederRow.Length = 0 Then
                    Continue For
                End If
                '---------Select or Add New Row
                Dim lNewRow As DataRow = If(lDtRow1.Length > 0, lDtRow1(0), mDs.Tables(table).NewRow())
                '--------Fill the Fields
                For Each col As String In TblDBColumns.Item(table)
                    lIsEmpty = True
                    lNothingSubstitute = Nothing
                    excelColExists = TblFieldsToExcelConverter.Item(table).ContainsKey(col)
                    excelCol = If(excelColExists, TblFieldsToExcelConverter.Item(table).Item(col), Nothing)
                    If excelColExists Then
                        If col.Contains("Fuse") Then
                            lNewRow(col) = If(excelColExists, CInt(fuseDict.Item(row(excelCol))), Nothing)
                            lIsEmpty = False
                        End If
                    End If
                    Select Case col
                        Case "LPPostId"
                            lNewRow(col) = lFeederRow(0)("LPPostId")
                            lIsEmpty = False
                        Case "LPFeederId"
                            lNewRow(col) = lFeederRow(0)("LPFeederId")
                            lIsEmpty = False
                        Case "LPPostLoadId"
                            If table = "TblLPPostLoad" Then
                                lNewRow(col) = If(lDtRow1.Length > 0, lDtRow1(0)("LPPostLoadId"), GetAutoInc())
                                lIsEmpty = False
                            ElseIf table = "TblLPFeederLoad" Then
                                lNewRow(col) = If(lDtRow1.Length > 0, lDtRow1(0)("LPPostLoadId"), DBNull.Value)
                                lIsEmpty = False
                            End If
                        Case "LPFeederLoadId"
                            lNewRow(col) = GetAutoInc()
                            lIsEmpty = False
                    End Select
                    If lIsEmpty Then
                        lNewRow(col) = If(excelColExists, row(excelCol),
                            If(lDtRow1.Length > 0, lNewRow(col), DBNull.Value))
                    End If
                Next
                If lDtRow1.Length = 0 Then
                    mDs.Tables(table).Rows.Add(lNewRow)
                End If

                '--------Progress Bar
                'AdvanceProgress(pg)
            Next
            Dim lUpdate As New frmUpdateDataSetBT
            lUpdate.UpdateDataSet(table, mDs)
        Next
    End Sub
    Private Sub MakeDataSetPostFeeeder()
        MakeDataSetPostFeeeder01()
        MakeDataSetPostFeeeder02()
    End Sub
    Private Sub FillPostFeederDataGrid()
        MakeDataSetPostFeeeder()
        CheckPostFeederDataIntegrity()
    End Sub
    Private Sub SavePostFeederInfo()
        ' Blah blah
    End Sub
    '-------------</Omid>---------------
    '''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub MakeTableSubscriber()
        mTbl_Subscriber = mDs.Tables("_Tbl_Subscriber")
    End Sub
    Private Function MakeTableSubscriberExcel() As Integer
        Try
            mDs.Tables("Tbl_SubscriberExcel").Clear()
            mTbl_SubscriberExcel = mDs.Tables("Tbl_SubscriberExcel")

            Dim lTblRowCount As Integer = 0

            Dim lFileNo As String = ""
            Dim lCode As String = ""
            Dim lCounterNo As String = ""
            Dim lRamzCode As Object
            Dim lNAME As String = ""
            Dim lTelephone As String = ""
            Dim lTelMobile As Object
            Dim lTelFax As Object
            Dim lAreaId As Object
            Dim lArea As Object
            Dim lAddress As Object
            Dim lPostalCode As Object
            Dim lGPSx As Object
            Dim lGPSy As Object
            Dim lEmailAddress As Object
            Dim lMPPostId As Object
            Dim lMPPostName As Object
            Dim lMPFeederId As Object
            Dim lMPFeederName As Object
            Dim lLPPostId As Object
            Dim lLPPostName As Object
            Dim lLPFeederId As Object
            Dim lLPFeederName As Object
            Dim lNationalCode As Object
            Dim lTarefeCode As Object
            Dim lSubscriberSensitivityId As Object
            Dim lSubscriberSensitivity As Object
            Dim lspcSubscriberVoltageId As Object
            Dim lSubscriberVoltage As Object
            Dim lspcSubscriberTypeId As Object
            Dim lSubscriberType As Object
            Dim lspcSubscriberPhaseTypeId As Object
            Dim lSubscriberPhaseType As Object
            Dim lACntrZarib As Object
            Dim lspcACntrTypeId As Object
            Dim lACntrType As Object
            Dim lRCntrZarib As Object
            Dim lspcRCntrTypeId As Object
            Dim lRCntrType As Object
            Dim lAFabrikNumber As Object
            Dim lRFabrikNumber As Object
            Dim lAInstallDate As Object
            Dim lRInstallDate As Object
            Dim lPOWER As Object
            Dim lAmper As Object
            Dim lspcSubscriberStateId As Object
            Dim lSubscriberState As Object
            Dim lUseTypeId As Object
            Dim lUseType As Object
            Dim lDebtPrice As Object
            Dim lDebtType As Object
            Dim lCanOutgoingCall As Object
            Dim lCityId As Object = DBNull.Value

            Dim lMPPostCode As String = ""
            Dim lMPFeederCode As String = ""
            Dim lLPPostCode As String = ""
            Dim lLPFeederCode As String = ""

            Dim lLastNullRecord As Integer = 0

            Dim lDs As New DataSet

            pg.Value = 0
            pg.Visible = True
            pg.Maximum = 2
            AdvanceProgress(pg)

            Dim i As Integer = 3
            Do
                lAreaId = Val(mExcel.ReadCellRC(mSheet, i, 9))
                lNAME = mExcel.ReadCellRC(mSheet, i, 5)

                lFileNo = mExcel.ReadCellRC(mSheet, i, 2)
                lCode = mExcel.ReadCellRC(mSheet, i, 3)
                lRamzCode = mExcel.ReadCellRC(mSheet, i, 4)

                Dim lUniqeItem As String = ""
                If rbCode.Checked Then
                    lUniqeItem = lCode
                ElseIf rbFileNo.Checked Then
                    lUniqeItem = lFileNo
                ElseIf rbRamz.Checked Then
                    lUniqeItem = lRamzCode
                End If

                If lAreaId <= 0 Or lNAME = "" Or lUniqeItem = "" Then
                    lLastNullRecord += 1
                Else
                    lTblRowCount += 1
                    lLastNullRecord = 0
                End If

                If lLastNullRecord >= 5 Then
                    Exit Do
                End If
                i += 1
            Loop While True
            AdvanceProgress(pg)
            i = 3

            pg.Maximum = lTblRowCount
            Do
                If lTblRowCount = 0 Then Exit Do
                lFileNo = mExcel.ReadCell(mSheet, "B" & i)
                lCode = mExcel.ReadCell(mSheet, "C" & i)
                lRamzCode = mExcel.ReadCell(mSheet, "D" & i)
                lNAME = mExcel.ReadCell(mSheet, "E" & i)
                lTelephone = mExcel.ReadCell(mSheet, "F" & i)
                lTelMobile = mExcel.ReadCell(mSheet, "G" & i)
                If lTelMobile = "" Then
                    lTelMobile = DBNull.Value
                End If
                lTelFax = mExcel.ReadCell(mSheet, "H" & i)
                If lTelFax = "" Then
                    lTelFax = DBNull.Value
                End If
                lAreaId = Val(mExcel.ReadCell(mSheet, "I" & i))
                If lAreaId > 0 Then
                    If mSortedArea.Keys.Contains(lAreaId) Then
                        lArea = mSortedArea(lAreaId).Area
                        lCityId = mSortedArea(lAreaId).CityId
                    Else
                        lAreaId = 0
                    End If
                Else
                    lAreaId = 0
                End If
                lAddress = mExcel.ReadCell(mSheet, "J" & i)
                lPostalCode = mExcel.ReadCell(mSheet, "K" & i)
                If lPostalCode = "" Then
                    lPostalCode = DBNull.Value
                End If
                lGPSx = Val(mExcel.ReadCell(mSheet, "L" & i))
                If lGPSx <= 0 Then
                    lGPSx = DBNull.Value
                End If
                lGPSy = Val(mExcel.ReadCell(mSheet, "M" & i))
                If lGPSy <= 0 Then
                    lGPSy = DBNull.Value
                End If
                lEmailAddress = mExcel.ReadCell(mSheet, "N" & i)
                If lEmailAddress = "" Then
                    lEmailAddress = DBNull.Value
                End If
                lMPPostCode = mExcel.ReadCell(mSheet, "O" & i)
                lMPFeederCode = mExcel.ReadCell(mSheet, "P" & i)
                lLPPostCode = mExcel.ReadCell(mSheet, "Q" & i)
                lLPFeederCode = mExcel.ReadCell(mSheet, "R" & i)
                lNationalCode = mExcel.ReadCell(mSheet, "S" & i)
                If lNationalCode = "" Then
                    lNationalCode = DBNull.Value
                End If
                lTarefeCode = mExcel.ReadCell(mSheet, "T" & i)
                If lTarefeCode = "" Then
                    lTarefeCode = DBNull.Value
                End If
                lSubscriberSensitivityId = Val(mExcel.ReadCell(mSheet, "U" & i))
                If lSubscriberSensitivityId > 0 Then
                    If mSortedSnesetivity.ContainsKey(CType(lSubscriberSensitivityId, Integer)) Then
                        lSubscriberSensitivity = mSortedSnesetivity.Item(CType(lSubscriberSensitivityId, Integer))
                    Else
                        lSubscriberSensitivityId = DBNull.Value
                    End If
                Else
                    lSubscriberSensitivityId = DBNull.Value
                End If

                lspcSubscriberVoltageId = Val(mExcel.ReadCell(mSheet, "V" & i))
                If lspcSubscriberVoltageId > 0 Then
                    If mSortedSpec.ContainsKey(CType(lspcSubscriberVoltageId, Integer)) Then
                        lSubscriberVoltage = mSortedSnesetivity.Item(CType(lspcSubscriberVoltageId, Integer))
                    Else
                        lspcSubscriberVoltageId = DBNull.Value
                    End If
                Else
                    lspcSubscriberVoltageId = DBNull.Value
                End If

                lspcSubscriberTypeId = Val(mExcel.ReadCell(mSheet, "W" & i))
                If lspcSubscriberTypeId > 0 Then
                    If mSortedSpec.ContainsKey(CType(lspcSubscriberTypeId, Integer)) Then
                        lSubscriberType = mSortedSnesetivity.Item(CType(lspcSubscriberTypeId, Integer))
                    Else
                        lspcSubscriberTypeId = DBNull.Value
                    End If
                Else
                    lspcSubscriberTypeId = DBNull.Value
                End If

                lspcSubscriberPhaseTypeId = Val(mExcel.ReadCell(mSheet, "X" & i))
                If lspcSubscriberPhaseTypeId > 0 Then
                    If mSortedSpec.ContainsKey(CType(lspcSubscriberPhaseTypeId, Integer)) Then
                        lSubscriberPhaseType = mSortedSnesetivity.Item(CType(lspcSubscriberPhaseTypeId, Integer))
                    Else
                        lspcSubscriberPhaseTypeId = DBNull.Value
                    End If
                Else
                    lspcSubscriberPhaseTypeId = DBNull.Value
                End If

                lACntrZarib = Val(mExcel.ReadCell(mSheet, "Y" & i))
                If lACntrZarib <= 0 Then
                    lACntrZarib = DBNull.Value
                End If
                lspcACntrTypeId = Val(mExcel.ReadCell(mSheet, "Z" & i))
                If lspcACntrTypeId > 0 Then
                    If mSortedSpec.ContainsKey(CType(lspcACntrTypeId, Integer)) Then
                        lACntrType = mSortedSnesetivity.Item(CType(lspcACntrTypeId, Integer))
                    Else
                        lspcACntrTypeId = DBNull.Value
                    End If
                Else
                    lspcACntrTypeId = DBNull.Value
                End If

                lRCntrZarib = Val(mExcel.ReadCell(mSheet, "AA" & i))
                If lRCntrZarib <= 0 Then
                    lRCntrZarib = DBNull.Value
                End If
                lspcRCntrTypeId = Val(mExcel.ReadCell(mSheet, "AB" & i))
                If lspcRCntrTypeId > 0 Then
                    If mSortedSpec.ContainsKey(CType(lspcRCntrTypeId, Integer)) Then
                        lRCntrType = mSortedSnesetivity.Item(CType(lspcRCntrTypeId, Integer))
                    Else
                        lspcRCntrTypeId = DBNull.Value
                    End If
                Else
                    lspcRCntrTypeId = DBNull.Value
                End If

                lAFabrikNumber = mExcel.ReadCell(mSheet, "AC" & i)
                If lAFabrikNumber = "" Then
                    lAFabrikNumber = DBNull.Value
                End If
                lRFabrikNumber = mExcel.ReadCell(mSheet, "AD" & i)
                If lRFabrikNumber = "" Then
                    lRFabrikNumber = DBNull.Value
                End If

                lPOWER = Val(mExcel.ReadCell(mSheet, "AE" & i))
                If lPOWER <= 0 Then
                    lPOWER = DBNull.Value
                End If
                lAmper = Val(mExcel.ReadCell(mSheet, "AF" & i))
                If lAmper <= 0 Then
                    lAmper = DBNull.Value
                End If
                lspcSubscriberStateId = Val(mExcel.ReadCell(mSheet, "AG" & i))
                If lspcSubscriberStateId > 0 Then
                    If mSortedSpec.ContainsKey(CType(lspcSubscriberStateId, Integer)) Then
                        lSubscriberState = mSortedSnesetivity.Item(CType(lspcSubscriberStateId, Integer))
                    Else
                        lspcSubscriberStateId = DBNull.Value
                    End If
                Else
                    lspcSubscriberStateId = DBNull.Value
                End If

                lUseTypeId = Val(mExcel.ReadCell(mSheet, "AH" & i))
                If lUseTypeId > 0 Then
                    If mSortedUseType.ContainsKey(CType(lUseTypeId, Integer)) Then
                        lUseType = mSortedSnesetivity.Item(CType(lUseTypeId, Integer))
                    Else
                        lUseTypeId = DBNull.Value
                    End If
                Else
                    lUseTypeId = DBNull.Value
                End If

                lDebtPrice = Val(mExcel.ReadCell(mSheet, "AI" & i))
                If lDebtPrice < 0 Then
                    lDebtPrice = DBNull.Value
                End If
                lDebtType = mExcel.ReadCell(mSheet, "AJ" & i)
                If lDebtType = "" Then
                    lDebtType = DBNull.Value
                End If
                lCanOutgoingCall = mExcel.ReadCell(mSheet, "AK" & i)

                If lCanOutgoingCall = "1" Then
                    lCanOutgoingCall = True
                ElseIf lCanOutgoingCall = "0" Then
                    lCanOutgoingCall = False
                Else
                    lCanOutgoingCall = DBNull.Value
                End If

                lLPFeederId = DBNull.Value
                lLPPostId = DBNull.Value
                lMPFeederId = DBNull.Value
                lMPPostId = DBNull.Value


                If lLPFeederCode <> "" Then
                    If mSortedLPFeeder.ContainsKey(lLPFeederCode) Then
                        lLPFeederId = mSortedLPFeeder(lLPFeederCode).LPFeederId
                        lLPFeederName = mSortedLPFeeder(lLPFeederCode).LPFeederName
                        lLPPostId = mSortedLPFeeder(lLPFeederCode).LPPostId
                        lLPPostName = mSortedLPFeeder(lLPFeederCode).LPPostName
                        lMPFeederId = mSortedLPFeeder(lLPFeederCode).MPFeederId
                        lMPFeederName = mSortedLPFeeder(lLPFeederCode).MPFeederName
                        lMPPostId = mSortedLPFeeder(lLPFeederCode).MPPostId
                        lMPPostName = mSortedLPFeeder(lLPFeederCode).MPPostName
                    End If
                End If

                If lLPPostCode <> "" Then
                    If mSortedLPPost.ContainsKey(lLPPostCode) Then
                        lLPPostId = mSortedLPPost(lLPPostCode).LPPostId
                        lLPPostName = mSortedLPPost(lLPPostCode).LPPostName
                        lMPFeederId = mSortedLPPost(lLPPostCode).MPFeederId
                        lMPFeederName = mSortedLPPost(lLPPostCode).MPFeederName
                        lMPPostId = mSortedLPPost(lLPPostCode).MPPostId
                        lMPPostName = mSortedLPPost(lLPPostCode).MPPostName
                    End If
                End If

                If lMPFeederCode <> "" Then
                    If mSortedMPFeeder.ContainsKey(lMPFeederCode) Then
                        lMPFeederId = mSortedMPFeeder(lMPFeederCode).MPFeederId
                        lMPFeederName = mSortedMPFeeder(lMPFeederCode).MPFeederName
                        lMPPostId = mSortedMPFeeder(lMPFeederCode).MPPostId
                        lMPPostName = mSortedMPFeeder(lMPFeederCode).MPPostName
                    End If
                End If

                If lMPPostCode <> "" Then
                    If mSortedMPPost.ContainsKey(lMPPostCode) Then
                        lMPPostId = mSortedMPPost(lMPPostCode).MPPostId
                        lMPPostName = mSortedMPPost(lMPPostCode).MPPostName
                    End If
                End If

                AdvanceProgress(pg)

                Dim lUniqeItem As String = ""
                If rbCode.Checked Then
                    lUniqeItem = lCode
                ElseIf rbFileNo.Checked Then
                    lUniqeItem = lFileNo
                ElseIf rbRamz.Checked Then
                    lUniqeItem = lRamzCode
                End If

                If lUniqeItem = "" Then
                    lLastNullRecord += 1
                Else
                    Dim lNewRow As DataRow = mTbl_SubscriberExcel.NewRow()
                    lNewRow("SubscriberId") = GetAutoInc()
                    lNewRow("FileNo") = lFileNo
                    lNewRow("Code") = lCode
                    lNewRow("CounterNo") = lCounterNo
                    lNewRow("RamzCode") = lRamzCode
                    lNewRow("NAME") = ReplaceFarsiToArabi(lNAME)
                    lNewRow("Telephone") = lTelephone
                    lNewRow("TelMobile") = lTelMobile
                    lNewRow("TelFax") = lTelFax
                    lNewRow("AreaId") = lAreaId
                    lNewRow("Area") = lArea
                    lNewRow("Address") = ReplaceFarsiToArabi(lAddress)
                    lNewRow("CityId") = lCityId
                    lNewRow("PostalCode") = lPostalCode
                    lNewRow("GPSx") = lGPSx
                    lNewRow("GPSy") = lGPSy
                    lNewRow("EmailAddress") = lEmailAddress
                    lNewRow("MPPostId") = lMPPostId
                    lNewRow("MPFeederId") = lMPFeederId
                    lNewRow("LPPostId") = lLPPostId
                    lNewRow("LPFeederId") = lLPFeederId
                    lNewRow("MPPostCode") = lMPPostCode
                    lNewRow("MPFeederCode") = lMPFeederCode
                    lNewRow("LPPostCode") = lLPPostCode
                    lNewRow("LPFeederCode") = lLPFeederCode
                    lNewRow("MPPostName") = lMPPostName
                    lNewRow("MPFeederName") = lMPFeederName
                    lNewRow("LPPostName") = lLPPostName
                    lNewRow("LPFeederName") = lLPFeederName
                    lNewRow("NationalCode") = lNationalCode
                    lNewRow("TarefeCode") = lTarefeCode
                    lNewRow("SubscriberSensitivityId") = lSubscriberSensitivityId
                    lNewRow("SubscriberSensitivity") = lSubscriberSensitivity
                    lNewRow("spcSubscriberVoltageId") = lspcSubscriberVoltageId
                    lNewRow("SubscriberVoltage") = lSubscriberVoltage
                    lNewRow("spcSubscriberTypeId") = lspcSubscriberTypeId
                    lNewRow("SubscriberType") = lSubscriberType
                    lNewRow("spcSubscriberPhaseTypeId") = lspcSubscriberPhaseTypeId
                    lNewRow("SubscriberPhaseType") = lSubscriberPhaseType
                    lNewRow("ACntrZarib") = lACntrZarib
                    lNewRow("spcACntrTypeId") = lspcACntrTypeId
                    lNewRow("ACntrType") = lACntrType
                    lNewRow("RCntrZarib") = lRCntrZarib
                    lNewRow("spcRCntrTypeId") = lspcRCntrTypeId
                    lNewRow("RCntrType") = lRCntrType
                    lNewRow("AFabrikNumber") = lAFabrikNumber
                    lNewRow("RFabrikNumber") = lRFabrikNumber
                    lNewRow("POWER") = lPOWER
                    lNewRow("Amper") = lAmper
                    lNewRow("spcSubscriberStateId") = lspcSubscriberStateId
                    lNewRow("SubscriberState") = lSubscriberState
                    lNewRow("UseTypeId") = lUseTypeId
                    lNewRow("UseType") = lUseType
                    lNewRow("DebtPrice") = lDebtPrice
                    lNewRow("DebtType") = lDebtType
                    lNewRow("CanOutgoingCall") = lCanOutgoingCall

                    mTbl_SubscriberExcel.Rows.Add(lNewRow)
                    lTblRowCount += 1
                    lLastNullRecord = 0
                End If

                If lLastNullRecord >= 5 Then
                    Exit Do
                End If
                i += 1
            Loop While True
            pg.Visible = False
            Return lTblRowCount
        Catch ex As Exception
            ShowError(ex.ToString)
            pg.Visible = False
        End Try

    End Function
    Private Sub FillSubscriberDataGrid()

        Dim lDs As New DataSet
        Dim lUpdateCount As Integer = 0
        Dim lInsertCount As Integer = 0
        Dim lUnchangeCount As Integer = 0
        Dim lFaildCount As Integer = 0
        Dim lMsg As String = ""

        pg.Visible = True
        pg.Value = 0

        Try

            mTbl_Subscriber.Rows.Clear()

            pg.Visible = True
            pg.Value = 0

            BindingTable("select * from Tbl_SubscriberInfo ", mCnn, mDs, "Tbl_SubscriberInfo", aIsClearTable:=True)

            If rbCode.Checked Then
                BindingTable("select * from Tbl_Subscriber where ISNULL(Code,'') <> '' ", mCnn, mDs, "Tbl_Subscriber", aIsClearTable:=True)
                If mTbl_SubscriberExcel.Rows.Count > 1000 Then
                    mUseSorted = True
                    pg.Maximum = mDs.Tables("Tbl_Subscriber").Rows.Count
                    For Each lRow As DataRow In mDs.Tables("Tbl_Subscriber").Rows
                        If Not mSortedSubscriber.ContainsKey(lRow("Code")) Then
                            mSortedSubscriber.Add(lRow("Code"), lRow)
                        End If
                        AdvanceProgress(pg)
                    Next
                End If

            ElseIf rbFileNo.Checked Then
                BindingTable("select * from Tbl_Subscriber where ISNULL(FileNo,'') <> '' ", mCnn, mDs, "Tbl_Subscriber", aIsClearTable:=True)
                If mTbl_SubscriberExcel.Rows.Count > 1000 Then
                    mUseSorted = True
                    pg.Maximum = mDs.Tables("Tbl_Subscriber").Rows.Count
                    For Each lRow As DataRow In mDs.Tables("Tbl_Subscriber").Rows
                        If Not mSortedSubscriber.ContainsKey(lRow("FileNo")) Then
                            mSortedSubscriber.Add(lRow("FileNo"), lRow)
                        End If
                        AdvanceProgress(pg)
                    Next
                End If

            ElseIf rbRamz.Checked Then
                BindingTable("select * from Tbl_Subscriber where ISNULL(RamzCode,'') <> '' ", mCnn, mDs, "Tbl_Subscriber", aIsClearTable:=True)
                If mTbl_SubscriberExcel.Rows.Count > 1000 Then
                    mUseSorted = True
                    pg.Maximum = mDs.Tables("Tbl_Subscriber").Rows.Count
                    For Each lRow As DataRow In mDs.Tables("Tbl_Subscriber").Rows
                        If Not mSortedSubscriber.ContainsKey(lRow("RamzCode")) Then
                            mSortedSubscriber.Add(lRow("RamzCode"), lRow)
                        End If
                        AdvanceProgress(pg)
                    Next
                End If

            End If

            pg.Value = 0
            pg.Maximum = mTbl_SubscriberExcel.Rows.Count
            For Each lRow As DataRow In mTbl_SubscriberExcel.Rows

                If lRow("AreaId") <= 0 Or lRow("Name") = "" Or lRow("Address") = "" Then
                    lFaildCount += 1
                    AdvanceProgress(pg)
                    Continue For
                End If

                Dim lNewRow As DataRow
                Dim lSQL As String = ""
                Dim lIsNew As Boolean = False
                Dim lIsUpdate As Boolean = False
                Dim lIsUnChange As Boolean = False

                Dim lDataRow As DataRow = Nothing

                If rbCode.Checked Then
                    If mUseSorted Then
                        If lRow("Code") = "" Then
                            lFaildCount += 1
                            AdvanceProgress(pg)
                            Continue For
                        End If
                        If mSortedSubscriber.ContainsKey(lRow("Code")) Then
                            lDataRow = mSortedSubscriber(lRow("Code"))
                        End If
                    ElseIf mDs.Tables("Tbl_Subscriber").Select("Code = '" & lRow("Code") & "'").Length > 0 Then
                        lDataRow = mDs.Tables("Tbl_Subscriber").Select("Code = '" & lRow("Code") & "'")(0)
                    End If
                ElseIf rbFileNo.Checked Then
                    If mUseSorted Then
                        If lRow("FileNo") = "" Then
                            lFaildCount += 1
                            AdvanceProgress(pg)
                            Continue For
                        End If
                        If mSortedSubscriber.ContainsKey(lRow("FileNo")) Then
                            lDataRow = mSortedSubscriber(lRow("FileNo"))
                        End If
                    ElseIf mDs.Tables("Tbl_Subscriber").Select("FileNo = '" & lRow("FileNo") & "'").Length > 0 Then
                        lDataRow = mDs.Tables("Tbl_Subscriber").Select("FileNo = '" & lRow("FileNo") & "'")(0)
                    End If
                ElseIf rbRamz.Checked Then
                    If mUseSorted Then
                        If lRow("RamzCode") = "" Then
                            lFaildCount += 1
                            AdvanceProgress(pg)
                            Continue For
                        End If
                        If mSortedSubscriber.ContainsKey(lRow("RamzCode")) Then
                            lDataRow = mSortedSubscriber(lRow("RamzCode"))
                        End If
                    ElseIf mDs.Tables("Tbl_Subscriber").Select("RamzCode = '" & lRow("RamzCode") & "'").Length > 0 Then
                        lDataRow = mDs.Tables("Tbl_Subscriber").Select("RamzCode = '" & lRow("RamzCode") & "'")(0)
                    End If
                End If

                lNewRow = mTbl_Subscriber.NewRow()

                If Not IsNothing(lDataRow) Then

                    lNewRow("SubscriberId") = lDataRow("SubscriberId")
                    lRow("SubscriberId") = lDataRow("SubscriberId")

                    For Each lColumn As DataColumn In mTbl_SubscriberExcel.Columns
                        Dim lCol As String = lColumn.ColumnName
                        If mDs.Tables("Tbl_Subscriber").Columns.Contains(lCol) AndAlso mTbl_Subscriber.Columns.Contains(lCol) Then

                            If (Not IsDBNull(lRow(lCol)) And IsDBNull(lDataRow(lCol))) _
                                OrElse (IsDBNull(lRow(lCol)) And Not IsDBNull(lDataRow(lCol))) _
                                OrElse (Not IsDBNull(lRow(lCol)) And Not IsDBNull(lDataRow(lCol)) AndAlso lRow(lCol) <> lDataRow(lCol)) Then
                                lIsUpdate = True

                                lDataRow(lCol) = lRow(lCol)
                            End If
                            lNewRow(lCol) = lRow(lCol)
                        Else
                            If mDs.Tables("_Tbl_Subscriber").Columns.Contains(lCol) AndAlso mTbl_Subscriber.Columns.Contains(lCol) Then
                                lNewRow(lCol) = lRow(lCol)
                            End If
                        End If

                    Next

                    If lIsUpdate Then
                        lNewRow("IsUpdate") = 1
                        lNewRow("IsInsert") = 0

                        Dim lRowsInfo() As DataRow = mDs.Tables("Tbl_SubscriberInfo").Select("SubscriberId = " & lDataRow("SubscriberId"))
                        If lRowsInfo.Length > 0 Then
                            lRowsInfo(0)("spcSubscriberStateId") = lRow("spcSubscriberStateId")
                            lRowsInfo(0)("DebtPrice") = lRow("DebtPrice")
                            lRowsInfo(0)("DebtType") = lRow("DebtType")
                        Else
                            Dim lNewRowsInfo As DataRow = mDs.Tables("Tbl_SubscriberInfo").NewRow()
                            lNewRowsInfo("SubscriberId") = lRow("SubscriberId")
                            lNewRowsInfo("spcSubscriberStateId") = lRow("spcSubscriberStateId")
                            lNewRowsInfo("DebtPrice") = lRow("DebtPrice")
                            lNewRowsInfo("DebtType") = lRow("DebtType")
                            mDs.Tables("Tbl_SubscriberInfo").Rows.Add(lNewRowsInfo)
                        End If

                    Else
                        lNewRow("IsUpdate") = 0
                        lNewRow("IsInsert") = 0
                        lIsUnChange = True
                    End If

                Else
                    lDataRow = mDs.Tables("Tbl_Subscriber").NewRow()
                    For Each lColumn As DataColumn In mTbl_SubscriberExcel.Columns
                        Dim lCol As String = lColumn.ColumnName
                        If mTbl_Subscriber.Columns.Contains(lCol) Then
                            lNewRow(lCol) = lRow(lCol)
                        End If
                    Next

                    lDataRow("SubscriberId") = GetAutoInc()
                    lDataRow("Name") = lRow("Name")
                    lDataRow("Code") = lRow("Code")
                    lDataRow("FileNo") = lRow("FileNo")
                    lDataRow("CounterNo") = lRow("CounterNo")
                    lDataRow("Telephone") = lRow("Telephone")
                    lDataRow("AreaId") = lRow("AreaId")
                    lDataRow("LPPostId") = lRow("LPPostId")
                    lDataRow("LPFeederId") = lRow("LPFeederId")
                    lDataRow("CityId") = lRow("CityId")
                    lDataRow("Address") = lRow("Address")
                    lDataRow("PostalCode") = lRow("PostalCode")
                    lDataRow("GPSx") = lRow("GPSx")
                    lDataRow("GPSy") = lRow("GPSy")
                    lDataRow("MPPostId") = lRow("MPPostId")
                    lDataRow("MPFeederId") = lRow("MPFeederId")
                    lDataRow("TelMobile") = lRow("TelMobile")
                    lDataRow("TelFax") = lRow("TelFax")
                    lDataRow("SubscriberSensitivityId") = lRow("SubscriberSensitivityId")
                    lDataRow("UseTypeId") = lRow("UseTypeId")
                    lDataRow("EmailAddress") = lRow("EmailAddress")
                    lDataRow("RamzCode") = lRow("RamzCode")
                    lDataRow("NationalCode") = lRow("NationalCode")
                    lDataRow("TarefeCode") = lRow("TarefeCode")
                    lDataRow("spcSubscriberPhaseTypeId") = lRow("spcSubscriberPhaseTypeId")
                    lDataRow("Amper") = lRow("Amper")
                    lDataRow("spcSubscriberTypeId") = lRow("spcSubscriberTypeId")
                    lDataRow("Power") = lRow("Power")
                    lDataRow("spcSubscriberVoltageId") = lRow("spcSubscriberVoltageId")
                    lDataRow("AFabrikNumber") = lRow("AFabrikNumber")
                    lDataRow("RFabrikNumber") = lRow("RFabrikNumber")
                    lDataRow("ACntrZarib") = lRow("ACntrZarib")
                    lDataRow("RCntrZarib") = lRow("RCntrZarib")
                    lDataRow("spcACntrTypeId") = lRow("spcACntrTypeId")
                    lDataRow("spcRCntrTypeId") = lRow("spcRCntrTypeId")
                    Dim lTime As New CTimeInfo
                    lDataRow("RegisterDT") = lTime.MiladiDate
                    lDataRow("RegisterDatePersian") = lTime.ShamsiDate
                    lDataRow("RegisterTime") = lTime.HourMin
                    lDataRow("CanOutgoingCall") = lRow("CanOutgoingCall")

                    mDs.Tables("Tbl_Subscriber").Rows.Add(lDataRow)


                    Dim lRowsInfo As DataRow = mDs.Tables("Tbl_SubscriberInfo").NewRow()
                    lRowsInfo("SubscriberId") = lDataRow("SubscriberId")
                    lRowsInfo("spcSubscriberStateId") = lRow("spcSubscriberStateId")
                    lRowsInfo("DebtPrice") = lRow("DebtPrice")
                    lRowsInfo("DebtType") = lRow("DebtType")
                    mDs.Tables("Tbl_SubscriberInfo").Rows.Add(lRowsInfo)

                    lNewRow("IsUpdate") = 0
                    lNewRow("IsInsert") = 1
                    lIsNew = True
                End If

                If lIsUpdate Then
                    lUpdateCount += 1
                    lNewRow("ChangeType") = "ويرايش"
                    mTbl_Subscriber.Rows.Add(lNewRow)
                ElseIf lIsNew Then
                    lInsertCount += 1
                    lNewRow("ChangeType") = "ثبت جديد"
                    mTbl_Subscriber.Rows.Add(lNewRow)
                ElseIf lIsUnChange Then
                    lUnchangeCount += 1
                    lNewRow("ChangeType") = "بدون تغيير"
                    mTbl_Subscriber.Rows.Add(lNewRow)
                Else
                    lFaildCount += 1
                End If

                AdvanceProgress(pg)

            Next

            dgSubscriberInfo.DataSource = mTbl_Subscriber
            lblCount.Text = mTbl_Subscriber.Rows.Count
            lMsg = "تعداد رکوردهای جديد : " & lInsertCount & vbCrLf
            lMsg &= "تعداد رکوردهای ويرايش شده : " & lUpdateCount & vbCrLf
            lMsg &= "تعداد رکوردهای بدون تغيير : " & lUnchangeCount & vbCrLf
            lMsg &= "تعداد رکوردهای غير قابل قبول : " & lFaildCount

            Me.Cursor = Cursors.Default
            MsgBoxF(lMsg, "ورود از فايل اکسل", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Information, MessageBoxDefaultButton.Button1)

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            ShowError(ex.ToString)
        End Try
        Me.Cursor = Cursors.Default
        pg.Visible = False

    End Sub
    Private Sub SaveSubscriberInfo()
        If dgSubscriberInfo.RowCount = 0 Then
            ShowError("رکوردي جهت ذخيره سازي يافت نشد")
            Exit Sub
        End If

        Dim lNewRow As DataRow, lNewRowSubInfo As DataRow
        Dim lSubscribersRows() As DataRow
        Dim lLastRows() As DataRow

        Dim IsNewRow As Boolean = False
        Dim IsNewRowInfo As Boolean = False
        Dim lTrans As SqlTransaction
        Dim lUpdate As New frmUpdateDataSetBT
        Dim lIsSaveOk As Boolean = False
        Dim lAcceptCount As Integer = 0, lFaildCount As Integer = 0
        Dim lMsg As String = ""
        Dim lPrevCount As Integer = 0
        Dim lTotalCount As Integer = 0
        Dim lTime As New CTimeInfo

        Me.Cursor = Cursors.WaitCursor
        pg.Visible = True
        pg.Maximum = 4
        pg.Value = 0
        Try
            If mCnn.State <> ConnectionState.Open Then
                mCnn.Open()
            End If

            lTrans = mCnn.BeginTransaction
            AdvanceProgress(pg)
            lIsSaveOk = lUpdate.UpdateDataSet("Tbl_Subscriber", mDs, aTrans:=lTrans)
            AdvanceProgress(pg)
            If lIsSaveOk Then
                Dim lRows() As DataRow = mDs.Tables("Tbl_SubscriberInfo").Select("", "", DataViewRowState.Added)
                For Each lRow As DataRow In lRows
                    If lRow.RowState = DataRowState.Added AndAlso lRow("SubscriberId") <> lUpdate.GetNewPrimaryKeyID(lRow("SubscriberId")) Then
                        lRow("SubscriberId") = lUpdate.GetNewPrimaryKeyID(lRow("SubscriberId"))
                    End If
                Next
                AdvanceProgress(pg)
                lIsSaveOk = lUpdate.UpdateDataSet("Tbl_SubscriberInfo", mDs, aIsAutoID:=False, aTrans:=lTrans)
                AdvanceProgress(pg)
            End If

            If lIsSaveOk Then
                lTrans.Commit()
            Else
                lTrans.Rollback()

            End If
            pg.Visible = False
            Dim lDlgInfo As New frmInformUserAction("اطلاعات با موفقيت ذخيره شد")
            lDlgInfo.ShowDialog()
            lDlgInfo.Dispose()
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Cursor = Cursors.Default
        Catch ex As Exception
            If Not IsNothing(lTrans) Then
                lTrans.Rollback()
            End If
            Me.Cursor = Cursors.Default
            ShowError(ex)
        Finally
            If mCnn.State = ConnectionState.Open Then
                mCnn.Close()
            End If
        End Try
        Me.Cursor = Cursors.Default
        pg.Visible = False
    End Sub

    Private Sub btnHelp_Click(sender As Object, e As EventArgs) Handles btnHelp.Click
        Dim lMsg As String = ""
        lMsg = "دستورالعمل بسيار مهم" & vbCrLf & "---------------------------------------"
        lMsg &= vbCrLf & "1. ابتدا بايد يکي از آيتم های منحصر به فرد انتخاب گردد تا ورود اطلاعات بر اساس آن صورت پذيرد. در انتخاب اين آيتم دقت نماييد. انتخاب اشتباه اين آيتم منجر به تکراری شدن اطلاعات مي گردد"
        lMsg &= vbCrLf & "2. هيچ گونه تغييري در عناوين ستون هاي فايل اکسل الگو نبايد صورت بگيرد و همچنين ترتيب آنها نبايد تغييري کند"
        lMsg &= vbCrLf & "3. در ستون هايي که نياز به وارد نمودن کد مي باشد، مي توانيد کد آيتم را از جدولي که در ادامه نمايش داده مي شود دريافت نماييد"
        lMsg &= vbCrLf & "4. ورود اطلاعات کد ناحيه، نام مشترک، آدرس اجباري مي باشد و درصورت عدم ورود هرکدام، رکورد غيرقابل قبول در نظر گرفته می شود"
        lMsg &= vbCrLf & "هر گونه عملکردي خارج از دستورالعمل فوق صورت پذيرد، مسئوليت آن به عهده شرکت توزيع مي باشد"
        MsgBoxF(lMsg, "راهنمای ورود اطلاعات", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Information, MessageBoxDefaultButton.Button1)
        Dim lDlg As New frmShowInfoInGrid()
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    '-----------------------------<Omid>---------------
    Private Sub InitializeDBColumns()
        Me.TblFieldsToExcelConverter = New Dictionary(Of String, Object)
        Me.TblDBColumns = New Dictionary(Of String, List(Of String))
        Select Case mFormType
            Case "PostFeeder"
                Dim lConverterFeederLoad As New Dictionary(Of String, String) From {
                    {"LPFeederName", "LPFeederName"},
                    {"LoadDateTimePersian", "LoadDateTimePersian"}, {"LoadTime", "LoadTime"}, {"RFuseId", "RFuse"},
                    {"SFuseId", "SFuse"}, {"TFuseId", "TFuse"}, {"RCurrent", "FeederRCurrent"},
                    {"SCurrent", "FeederSCurrent"}, {"TCurrent", "FeederTCurrent"}, {"EndLineVoltage", "FeederEndlineVoltage"},
                    {"LPFeederId", "LPFeederId"}, {"LPFeederLoadId", "LPFeederLoadId"}
                    }
                Dim lConverterPostLoad As New Dictionary(Of String, String) From {
                    {"LPPostName", "LPPostName"},
                    {"LoadDateTimePersian", "LoadDateTimePersian"}, {"LPPostId", "LPPostId"},
                    {"LoadTime", "LoadTime"}, {"RCurrent", "PostRCurrent"}, {"SCurrent", "PostSCurrent"},
                    {"TCurrent", "PostTCurrent"}, {"NolCurrent", "PostNolCurrent"}, {"vTS", "PostvTS"},
                    {"vRN", "PostvRN"}, {"vTN", "PostvTN"}, {"vRS", "PostvRS"},
                    {"vTR", "PostvTR"}, {"LPPostLoadId", "LPPostLoadId"}
                    }
                Dim lColsFeederLoad As New List(Of String) From {
                    "LPFeederLoadId", "LPFeederId", "FeederPeakCurrent", "ConductorSize", "RCurrent", "SCurrent",
                    "TCurrent", "NolCurrent", "RFuseId", "SFuseId", "TFuseId", "KelidCurrent",
                    "LoadDT", "LoadDateTimePersian", "LoadTime", "FazSatheMaghtaId", "CountFazSatheMaghta", "NolSatheMaghtaId",
                    "CountNolSatheMaghta", "PostPeakCurrent", "FazSatheMaghta2Id", "CountFazSatheMaghta2", "NolSatheMaghta2Id", "CountNolSatheMaghta2",
                    "LPPostLoadId", "DEDT", "DEDatePersian", "DETime", "AreaUserId", "EndLineVoltage",
                    "spcCableTypeId", "vRS", "vTS", "vTR", "vRN", "vTN",
                    "vSN", "veRS", "veTS", "veTR", "veRN", "veTN",
                    "veSN", "IsTakFaze", "CosinPhi", "EarthValue"
                    }
                Dim lColsPostLoad As New List(Of String) From {
                    "LPPostLoadId", "LPPostId", "PostCapacity", "LoadDT", "LoadDateTimePersian", "LoadTime",
                    "FazSatheMaghtaId", "CountFazSatheMaghta", "NolSatheMaghtaId", "CountNolSatheMaghta", "PostPeakCurrent", "RCurrent",
                    "SCurrent", "TCurrent", "NolCurrent", "KelidCurrent", "DEDT", "DEDatePersian",
                    "DETime", "AreaUserId", "vRS", "vTS", "vTR", "vRN",
                    "vTN", "vSN", "IsTakFaze", "EarthValue", "EarthValueE", "LPTransId"
                    }
                TblFieldsToExcelConverter.Add("TblLPFeederLoad", lConverterFeederLoad)
                TblFieldsToExcelConverter.Add("TblLPPostLoad", lConverterPostLoad)
                TblDBColumns.Add("TblLPFeederLoad", lColsFeederLoad)
                TblDBColumns.Add("TblLPPostLoad", lColsPostLoad)
        End Select
    End Sub
    '-------------</Omid>---------------
End Class

Public Class CAreaInfo
    Public Property Area As String
    Public Property CityId As Integer
End Class

Public Class CMPPostInfo
    Public Property MPPostId As Integer
    Public Property MPPostName As String
End Class

Public Class CMPFeederInfo
    Public Property MPFeederId As Integer
    Public Property MPFeederName As String
    Public Property MPPostId As Integer
    Public Property MPPostName As String
End Class

Public Class CLPPostInfo
    Public Property LPPostId As Integer
    Public Property LPPostName As String
    Public Property MPFeederId As Integer
    Public Property MPFeederName As String
    Public Property MPPostId As Integer
    Public Property MPPostName As String
End Class

Public Class CLPFeederInfo
    Public Property LPFeederId As Integer
    Public Property LPFeederName As String
    Public Property LPPostId As Integer
    Public Property LPPostName As String
    Public Property MPFeederId As Integer
    Public Property MPFeederName As String
    Public Property MPPostId As Integer
    Public Property MPPostName As String
End Class


