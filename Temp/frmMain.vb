Imports System
Imports System.Data.SqlClient
Imports System.Xml
Imports System.IO
Imports Microsoft.Win32
Imports System.Drawing
Imports System.Windows
Imports System.Windows.Forms
Imports System.Threading
Imports TelC
Imports CSharp_Functions
Imports System.Text.RegularExpressions

Public Class frmMain
    Inherits FormBase
    Dim LastEventID As Integer = 1
    Dim IsRunTimer As Boolean = False
    Dim IsRunningMsgBox As Boolean = False
    Dim Timers(3) As Long

    '/* West Azarbaijan, Orumieh HardLock Constants:
    '// HL_PWD = "Datis;WA_Orumieh"
    '// HL_DATA_PART = "WA_ORM PwAcc V2.x.x"
    '*/
    'Const HL_PWD = "Datis;WA_Orumieh"
    'Const HL_SERIAL = "MSCo.Ltd T:6933523,6930921 2004-8303-1045" 'Don't care
    'Const HL_DATA_PART = "WA_ORM PwAcc V2.x.x"
    'Const HL_PWD = "Datis;WA_Gilan"
    'Const HL_SERIAL = "MSCo.Ltd T:6933523,6930921 2004-8302-7495" 'Don't care
    'Const HL_DATA_PART = "WA_GLN PwAcc V2.x.x"
    'Const HL_PWD = "Datis;WA_ShomaleGharb"
    'Const HL_SERIAL = "MSCo.Ltd T:6933523,6930921 2400-0505-8172" 'Don't care
    'Const HL_DATA_PART = "WA_SHG PwAcc V2.x.x"

    'Const HL_PWD = "Datis;WA_Orumieh"
    'Const HL_SERIAL = "MSCo.Ltd T:6933523,6930921 2400-0505-8172" 'Don't care
    'Const HL_DATA_PART = "WA_SHG PwAcc V2.x.x"
    Const HL_PWD = "Datis;7844"
    Const HL_PWDUSB = "82532F4A8678D5491C458DC2D588CC1A"
    'Password for USB: "Datis;7844-Y1385"
    'Data for USB: "DatisEng"


    Const HLC_LAUNCH_TIMER_INT = 20      '5 Seconds and disable timer
    Const HLC_POOLING1_TIMER_INT = 600  '600 Seconds = 10 Minutes
    Const HLC_POOLING2_TIMER_INT = 1200 '1200 Seconds = 20 Minutes

    Public HardLockCheckLaunchTimerInterval As Integer
    Public HardLockCheckPooling1TimerInterval As Integer
    Public HardLockCheckPooling2TimerInterval As Integer
    Private CallPoolingIndex As Integer = -1
    '''''''''''''''''''''''
    Private m_HclsCall As HclsCall
    Dim m_ShowRequestCount As Integer = 0
    Dim MaxShowRequestCount As Integer = 5

    Dim mIsFirst As Boolean = True
    Private mfrmDownload As frmDownload = Nothing

    Private mIsLogMitel As Boolean = False

    Private mIsShowFav As Boolean = GetRegistryAbd("HavadesFavorite", False)

    Private mCnn As SqlConnection = New SqlConnection(GetConnection())
    Private mDs As New DataSet

    Private mIsChangeMenuFav As Boolean = False
    Private mIsSendGISSubscriberDCInfo As Boolean = CConfig.ReadConfig("IsSendGISSubscriberDCInfo", False)
    Friend WithEvents mnuTavanirReports As ToolStripMenuItem
    Friend WithEvents mnuReport_14_1 As FavToolStripMenuItem
    Friend WithEvents mnuReport_14_2 As FavToolStripMenuItem
    Friend WithEvents mnuReport_14_3 As FavToolStripMenuItem
    Friend WithEvents mnuReport_14_4 As FavToolStripMenuItem
    Friend WithEvents mnuRepotEkipTrace As ToolStripMenuItem
    Friend WithEvents mnuRep_h_1 As ToolStripMenuItem
    Friend WithEvents MenuButtonItem98 As ToolStripMenuItem
    Friend WithEvents mnuRecloserReport As ToolStripMenuItem
    Private mIsHomaAccess As Boolean = False
    '''''''''''''''''''''''''''''
#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        Dim lIsLogMITEL As String = GetRegistryAbd("IsLogMitel", "False")
        mIsLogMitel = (lIsLogMITEL.ToLower() = "true")
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing Then
                If Not (components Is Nothing) Then
                    components.Dispose()
                End If
            End If
            MyBase.Dispose(disposing)
        Catch ex As Exception
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents MnuReport_4_26 As FavToolStripMenuItem
    Friend WithEvents mnuCCIntelligent As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As ToolStripMenuItem
    Friend WithEvents mnuTrackingCodeSettings As ToolStripMenuItem
    Friend WithEvents mnuSurvyReport As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator75 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuMap121andIGMC As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuExtractionReports As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator76 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuReport_13_1 As Bargh_Common.FavToolStripMenuItem
    Friend WithEvents mnuReport_13_2 As Bargh_Common.FavToolStripMenuItem
    Friend WithEvents mnuReport_13_3 As Bargh_Common.FavToolStripMenuItem
    Friend WithEvents mnuReport_13_4 As Bargh_Common.FavToolStripMenuItem
    Friend WithEvents mnuTransLogs As ToolStripMenuItem
    Friend WithEvents mnuSepLPTRans As ToolStripSeparator
    Friend WithEvents mnuFreeTrans As ToolStripMenuItem
    Friend WithEvents mnuSMSSettings As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator77 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuRequestMultiDC As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator78 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuSimaPortal As ToolStripMenuItem
    Friend WithEvents mnuSepMisc As ToolStripSeparator
    Friend WithEvents TimerHcls As System.Windows.Forms.Timer
    Friend WithEvents StatusBar1 As System.Windows.Forms.StatusBar
    Friend WithEvents SqlDataAdapter1 As System.Data.SqlClient.SqlDataAdapter
    Friend WithEvents SqlSelectCommand1 As System.Data.SqlClient.SqlCommand
    Friend WithEvents SqlInsertCommand1 As System.Data.SqlClient.SqlCommand
    Friend WithEvents SqlUpdateCommand1 As System.Data.SqlClient.SqlCommand
    Friend WithEvents SqlDeleteCommand1 As System.Data.SqlClient.SqlCommand
    Friend WithEvents SqlConnection1 As System.Data.SqlClient.SqlConnection
    Friend WithEvents MenuItem19 As System.Windows.Forms.MenuItem
    Friend WithEvents TimerNewMessages As System.Windows.Forms.Timer
    Friend WithEvents ImageListMain As System.Windows.Forms.ImageList
    Friend WithEvents TimerCallPooling As System.Windows.Forms.Timer
    Friend WithEvents TimerMessagePooling As System.Windows.Forms.Timer
    Friend WithEvents TimerHardLockCheckLaunch As System.Windows.Forms.Timer
    Friend WithEvents TimerHardLockCheckPooling1 As System.Windows.Forms.Timer
    Friend WithEvents TimerHardLockCheckPooling2 As System.Windows.Forms.Timer
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents MenuBar1 As MenuStrip
    Friend WithEvents MenuItem4 As ToolStripMenuItem
    Friend WithEvents MenuItem86 As ToolStripMenuItem
    Friend WithEvents MenuFileMessages As ToolStripMenuItem
    Friend WithEvents MenuItem17 As ToolStripMenuItem
    Friend WithEvents MenuItem11 As ToolStripMenuItem
    Friend WithEvents MenuItem6 As ToolStripMenuItem
    Friend WithEvents MenuToolsRelogin As ToolStripMenuItem
    Friend WithEvents MenuFileExit As ToolStripMenuItem
    Friend WithEvents MenuFile As ToolStripMenuItem
    Friend WithEvents MenuItem22 As ToolStripMenuItem
    Friend WithEvents MenuItem92 As ToolStripMenuItem
    Friend WithEvents MenuItem117 As ToolStripMenuItem
    Friend WithEvents MenuItem15 As ToolStripMenuItem
    Friend WithEvents MenuItem110 As ToolStripMenuItem
    Friend WithEvents MenuItem97 As ToolStripMenuItem
    Friend WithEvents mnuReports As ToolStripMenuItem
    Friend WithEvents mnuRepLPRequest As ToolStripMenuItem
    Friend WithEvents MenuItem56 As FavToolStripMenuItem
    Friend WithEvents MenuItem57 As FavToolStripMenuItem
    Friend WithEvents MenuItem54 As FavToolStripMenuItem
    Friend WithEvents MenuItem65 As FavToolStripMenuItem
    Friend WithEvents MenuItem136 As FavToolStripMenuItem
    Friend WithEvents MenuItem139 As FavToolStripMenuItem
    Friend WithEvents MenuItem100 As FavToolStripMenuItem
    Friend WithEvents MenuItem137 As FavToolStripMenuItem
    Friend WithEvents MenuItem101 As FavToolStripMenuItem
    Friend WithEvents MenuItem138 As FavToolStripMenuItem
    Friend WithEvents MenuItem118 As FavToolStripMenuItem
    Friend WithEvents mnuRepLightRequest As ToolStripMenuItem
    Friend WithEvents MenuItem80 As FavToolStripMenuItem
    Friend WithEvents MenuItem81 As FavToolStripMenuItem
    Friend WithEvents MenuItem140 As FavToolStripMenuItem
    Friend WithEvents MenuItem142 As FavToolStripMenuItem
    Friend WithEvents MenuItem143 As FavToolStripMenuItem
    Friend WithEvents MenuItem144 As FavToolStripMenuItem
    Friend WithEvents MenuItem145 As FavToolStripMenuItem
    Friend WithEvents MenuItem146 As FavToolStripMenuItem
    Friend WithEvents MenuItem147 As FavToolStripMenuItem
    Friend WithEvents MenuItemFogh As ToolStripMenuItem
    Friend WithEvents mnuRepMPRequest As ToolStripMenuItem
    Friend WithEvents MenuItem79 As FavToolStripMenuItem
    Friend WithEvents MenuItem78 As FavToolStripMenuItem
    Friend WithEvents MenuItem77 As FavToolStripMenuItem
    Friend WithEvents MenuItem76 As FavToolStripMenuItem
    Friend WithEvents MenuItem75 As FavToolStripMenuItem
    Friend WithEvents MenuItem74 As FavToolStripMenuItem
    Friend WithEvents MenuItem73 As FavToolStripMenuItem
    Friend WithEvents MenuItem152 As FavToolStripMenuItem
    Friend WithEvents MenuItem157 As FavToolStripMenuItem
    Friend WithEvents MenuItem156 As FavToolStripMenuItem
    Friend WithEvents MenuItem155 As FavToolStripMenuItem
    Friend WithEvents MenuItem149 As FavToolStripMenuItem
    Friend WithEvents MenuItem153 As FavToolStripMenuItem
    Friend WithEvents MenuItem150 As FavToolStripMenuItem
    Friend WithEvents MenuItem154 As FavToolStripMenuItem
    Friend WithEvents MenuItem151 As FavToolStripMenuItem
    Friend WithEvents mnuRepBaseInfo As ToolStripMenuItem
    Friend WithEvents MenuItem121 As FavToolStripMenuItem
    Friend WithEvents mnuReportLPTrans As FavToolStripMenuItem
    Friend WithEvents MenuItem122 As FavToolStripMenuItem
    Friend WithEvents MenuItem126 As FavToolStripMenuItem
    Friend WithEvents MenuItem127 As FavToolStripMenuItem
    Friend WithEvents MenuItem124 As FavToolStripMenuItem
    Friend WithEvents MenuItem125 As FavToolStripMenuItem
    Friend WithEvents MenuItem129 As FavToolStripMenuItem
    Friend WithEvents mnuRepDorehei As ToolStripMenuItem
    Friend WithEvents MenuItem47 As FavToolStripMenuItem
    Friend WithEvents MenuItem107 As FavToolStripMenuItem
    Friend WithEvents mnuRep_7_21 As FavToolStripMenuItem
    Friend WithEvents mnuControlReports As ToolStripMenuItem
    Friend WithEvents MenuItem111 As FavToolStripMenuItem
    Friend WithEvents MenuItem112 As FavToolStripMenuItem
    Friend WithEvents MenuItem113 As FavToolStripMenuItem
    Friend WithEvents MenuBaseTables As ToolStripMenuItem
    Friend WithEvents MenuItem84 As ToolStripMenuItem
    Friend WithEvents MenuItem83 As ToolStripMenuItem
    Friend WithEvents MenuItem27 As ToolStripMenuItem
    Friend WithEvents MenuItem8 As ToolStripMenuItem
    Friend WithEvents MenuItem60 As ToolStripMenuItem
    Friend WithEvents MenuItem67 As ToolStripMenuItem
    Friend WithEvents mnuVillage As ToolStripMenuItem
    Friend WithEvents mnuSection As ToolStripMenuItem
    Friend WithEvents mnuTown As ToolStripMenuItem
    Friend WithEvents mnuRoosta As ToolStripMenuItem

    Friend WithEvents MenuItem5 As ToolStripMenuItem
    Friend WithEvents MenuItem7 As ToolStripMenuItem
    Friend WithEvents MenuItem38 As ToolStripMenuItem
    Friend WithEvents MenuItem9 As ToolStripMenuItem
    Friend WithEvents MenuItem51 As ToolStripMenuItem
    Friend WithEvents MenuItem59 As ToolStripMenuItem
    Friend WithEvents MenuItem64 As ToolStripMenuItem
    Friend WithEvents MenuItem29 As ToolStripMenuItem
    Friend WithEvents MenuItem70 As ToolStripMenuItem
    Friend WithEvents MenuItem69 As ToolStripMenuItem
    Friend WithEvents MenuItem68 As ToolStripMenuItem
    Friend WithEvents MenuItem50 As ToolStripMenuItem
    Friend WithEvents MenuItem3 As ToolStripMenuItem
    Friend WithEvents MenuItem30 As ToolStripMenuItem
    Friend WithEvents MenuItem14 As ToolStripMenuItem
    Friend WithEvents MenuItem52 As ToolStripMenuItem
    Friend WithEvents MenuItem25 As ToolStripMenuItem
    Friend WithEvents MenuItem63 As ToolStripMenuItem
    Friend WithEvents MenuItem46 As ToolStripMenuItem
    Friend WithEvents MenuItem44 As ToolStripMenuItem
    Friend WithEvents MenuItem48 As ToolStripMenuItem
    Friend WithEvents ManuOtherBase As ToolStripMenuItem
    Friend WithEvents MenuItem40 As ToolStripMenuItem
    Friend WithEvents MenuItem41 As ToolStripMenuItem
    Friend WithEvents MenuItem71 As ToolStripMenuItem
    Friend WithEvents MenuItem94 As ToolStripMenuItem
    Friend WithEvents MenuItem32 As ToolStripMenuItem
    Friend WithEvents MenuItem39 As ToolStripMenuItem
    Friend WithEvents MenuItem82 As ToolStripMenuItem
    Friend WithEvents MenuItem34 As ToolStripMenuItem
    Friend WithEvents MenuItem42 As ToolStripMenuItem
    Friend WithEvents MenuItem36 As ToolStripMenuItem
    Friend WithEvents MenuItem2 As ToolStripMenuItem
    Friend WithEvents MenuTools As ToolStripMenuItem
    Friend WithEvents MenuToolsChangePassword As ToolStripMenuItem
    Friend WithEvents MenuToolsUserManagement As ToolStripMenuItem
    Friend WithEvents MenuItem21 As ToolStripMenuItem
    Friend WithEvents mnuQuestion As ToolStripMenuItem
    Friend WithEvents mnuSurvey As ToolStripMenuItem
    Friend WithEvents mnuSurveyIVR As ToolStripMenuItem
    Friend WithEvents mnuGuidSMS As ToolStripMenuItem
    Friend WithEvents mnuSMSErjaDone As ToolStripMenuItem
    Friend WithEvents mnuSMSAfterEzamEkip As ToolStripMenuItem
    Friend WithEvents MnuUISkin As ToolStripMenuItem
    Friend WithEvents MnuUISkin1 As ToolStripMenuItem
    Friend WithEvents MnuUISkin2 As ToolStripMenuItem
    Friend WithEvents MnuUISkin3 As ToolStripMenuItem
    Friend WithEvents MnuUISkin4 As ToolStripMenuItem
    Friend WithEvents MenuConfiguration As ToolStripMenuItem
    Friend WithEvents MenuShowInfo As ToolStripMenuItem
    Friend WithEvents MenuButtonItem1 As ToolStripMenuItem
    Friend WithEvents HlpProvider As System.Windows.Forms.HelpProvider
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents TimerSyncDT As System.Windows.Forms.Timer
    Friend WithEvents MenuButtonItem2 As ToolStripMenuItem
    Friend WithEvents MenuButtonItem3 As ToolStripMenuItem
    Friend WithEvents MenuButtonEkipProfile As ToolStripMenuItem
    Friend WithEvents mnuAVLCar As ToolStripMenuItem
    Friend WithEvents mnuAVLState As ToolStripMenuItem
    Friend WithEvents MenuButtonItemTbl_LPCloserType As ToolStripMenuItem
    Friend WithEvents MenuButtonItemTbl_LPFaultType As ToolStripMenuItem
    Friend WithEvents MenuButtonItemTbl_LPTamirDisconnectFor As ToolStripMenuItem
    Friend WithEvents MenuButtonItemTbl_LPTamirRequestType As ToolStripMenuItem
    Friend WithEvents MenuButtonItemTbl_MPTamirDisconnectFor As ToolStripMenuItem
    Friend WithEvents MenuButtonItemTbl_MPTamirRequestType As ToolStripMenuItem
    Friend WithEvents MenuButtonItemTbl_DisconnectLightRequestFor As ToolStripMenuItem
    Friend WithEvents MenuButtonItemTbl_DisconnectLPRequestFor As ToolStripMenuItem
    Friend WithEvents MenuButtonItemTbl_DisconnectMPRequestFor As ToolStripMenuItem
    Friend WithEvents MenuButtonItemGlobalValue As ToolStripMenuItem
    Friend WithEvents MenuButtonItem5 As FavToolStripMenuItem
    Friend WithEvents MenuButtonItem6 As ToolStripMenuItem
    Friend WithEvents MenuButtonItem7 As ToolStripMenuItem
    Friend WithEvents MenuButtonItem8 As ToolStripMenuItem
    Friend WithEvents MenuButtonItem9 As FavToolStripMenuItem
    Friend WithEvents MenuButtonItem10 As ToolStripMenuItem
    Friend WithEvents MenuButtonItem11 As ToolStripMenuItem
    Friend WithEvents MenuButtonItem12 As ToolStripMenuItem
    Friend WithEvents MenuButtonItem16 As ToolStripMenuItem
    Friend WithEvents MenuButtonItem17 As ToolStripMenuItem
    Friend WithEvents MenuButtonItem18 As ToolStripMenuItem
    Friend WithEvents MenuButtonItem19 As ToolStripMenuItem
    Friend WithEvents MenuButtonItem20 As ToolStripMenuItem
    Friend WithEvents MenuButtonItem21 As ToolStripMenuItem
    Friend WithEvents MenuButtonItem13 As ToolStripMenuItem
    Friend WithEvents MenuButtonItem14 As ToolStripMenuItem
    Friend WithEvents MenuButtonItem22 As ToolStripMenuItem
    Friend WithEvents TimerNewRequest As System.Windows.Forms.Timer
    Friend WithEvents MenuButtonItem4 As FavToolStripMenuItem
    Friend WithEvents MenuButtonItem23 As FavToolStripMenuItem
    Friend WithEvents MenuButtonItem24 As FavToolStripMenuItem
    Friend WithEvents MenuButtonItem25 As FavToolStripMenuItem
    Friend WithEvents MenuButtonItem26 As ToolStripMenuItem
    Friend WithEvents pnlDownloadBox As System.Windows.Forms.Panel
    Friend WithEvents MenuButtonItem28 As FavToolStripMenuItem
    Friend WithEvents MenuButtonItem30 As ToolStripMenuItem
    Friend WithEvents MenuButtonItem31 As ToolStripMenuItem
    Friend WithEvents MenuButtonItem33 As FavToolStripMenuItem
    Friend WithEvents MenuButtonItem34 As ToolStripMenuItem
    Friend WithEvents MenuItemGroupPart As ToolStripMenuItem
    Friend WithEvents MenuButtonItem35 As ToolStripMenuItem
    Friend WithEvents mnuMulitiStepConnection As FavToolStripMenuItem
    Friend WithEvents mnuRptManoeurve As FavToolStripMenuItem

    Friend WithEvents MenuButtonItem36 As FavToolStripMenuItem
    Friend WithEvents MenuButtonItem37 As ToolStripMenuItem
    Friend WithEvents MenuButtonItem38 As FavToolStripMenuItem
    Friend WithEvents MenuButtonItem39 As FavToolStripMenuItem
    Friend WithEvents MenuButtonItem40 As FavToolStripMenuItem
    Friend WithEvents MenuButtonItem41 As FavToolStripMenuItem
    Friend WithEvents MenuButtonItem42 As Bargh_Common.FavToolStripMenuItem
    Friend WithEvents MenuButtonItem43 As Bargh_Common.FavToolStripMenuItem
    Friend WithEvents mnuRepMP_Mode4 As FavToolStripMenuItem
    Friend WithEvents HardLockUSB As AxTINYLib.AxTiny
    Friend WithEvents MenuButtonItem44 As ToolStripMenuItem
    Friend WithEvents MenuItemPublicTelNumbers As ToolStripMenuItem
    Friend WithEvents mnuReportHourByHour As FavToolStripMenuItem
    Friend WithEvents MenuItemMonitoring As ToolStripMenuItem
    Friend WithEvents mnuGroupTrustee As ToolStripMenuItem
    Friend WithEvents MenuItemMonitoringLight As ToolStripMenuItem
    Friend WithEvents mnuRepLoading As ToolStripMenuItem
    Friend WithEvents mnuRepEarting As ToolStripMenuItem
    Friend WithEvents mnuLPFedersLoad As FavToolStripMenuItem
    Friend WithEvents mnuMinCountMP As FavToolStripMenuItem
    Friend WithEvents mnuMinCountMPArea As FavToolStripMenuItem
    Friend WithEvents mnuReportRequests As ToolStripMenuItem
    Friend WithEvents mnuRptRequests As FavToolStripMenuItem
    Friend WithEvents mnuRptChangeHistory As FavToolStripMenuItem
    Friend WithEvents mnuRptEzamEkipInfo As FavToolStripMenuItem
    Friend WithEvents mnuRptReferToInfo As FavToolStripMenuItem
    Friend WithEvents mnuMinCountLPFeeder As FavToolStripMenuItem
    Friend WithEvents mnuMinCountLPPost As FavToolStripMenuItem
    Friend WithEvents mnuRepportSummaryLight As FavToolStripMenuItem
    Friend WithEvents mnuDisconnectPower_Zone As FavToolStripMenuItem
    Friend WithEvents mnuDisconnectCount_Zone As FavToolStripMenuItem
    Friend WithEvents mnuRequestCountDaily As FavToolStripMenuItem
    Friend WithEvents mnuDisconnectCountSubscribers1000 As FavToolStripMenuItem
    Friend WithEvents mnuReport5_1_1_mode2 As FavToolStripMenuItem
    Friend WithEvents imgListMisc As System.Windows.Forms.ImageList
    Friend WithEvents mnuLP_Mode1 As FavToolStripMenuItem
    Friend WithEvents mnuRptReferToCountArea As FavToolStripMenuItem
    Friend WithEvents mnuRptReferToCount As FavToolStripMenuItem
    Friend WithEvents mnuRepMPStatusChart As FavToolStripMenuItem
    Friend WithEvents mnuBillBoard As ToolStripMenuItem
    Friend WithEvents mnuRepMPDisconnectLPPost As FavToolStripMenuItem
    Friend WithEvents mnuExcelReports As ToolStripMenuItem
    Friend WithEvents mnuInform As ToolStripMenuItem
    Friend WithEvents mnuHoma As ToolStripMenuItem
    Friend WithEvents mnuInformMonitoring As ToolStripMenuItem
    Friend WithEvents mnuSubecribersSourceSetting As ToolStripMenuItem
    Friend WithEvents mnuLightRequestSmsSetting As ToolStripMenuItem
    Friend WithEvents mnuSmsContactsInfos As ToolStripMenuItem
    Friend WithEvents mnuSmsContactsSemat As ToolStripMenuItem
    Friend WithEvents mnuSmsContactsGroups As ToolStripMenuItem
    Friend WithEvents mnuSmsContacts As ToolStripMenuItem
    Friend WithEvents mnuSendSmsPanel As ToolStripMenuItem
    Friend WithEvents mnuPeopleVoice As ToolStripMenuItem
    Friend WithEvents mnuUseType As ToolStripMenuItem
    Friend WithEvents mnuFeederPart As ToolStripMenuItem
    Friend WithEvents mnuMPFeederKey As ToolStripMenuItem
    Friend WithEvents mnuNewRequestConfig As ToolStripMenuItem
    Friend WithEvents mnuIsKeyboardBase As ToolStripMenuItem
    Friend WithEvents mnuRepDispaching As FavToolStripMenuItem
    Friend WithEvents mnuRepOtherReport As ToolStripMenuItem
    Friend WithEvents mnuVersionInfo As ToolStripMenuItem
    Friend WithEvents MenuButtonItem50 As ToolStripMenuItem
    Friend WithEvents MenuButtonItem51 As ToolStripMenuItem
    Friend WithEvents MenuButtonItem52 As ToolStripMenuItem
    Friend WithEvents MenuButtonItem53 As ToolStripMenuItem
    Friend WithEvents MenuButtonItem54 As ToolStripMenuItem
    Friend WithEvents MenuButtonItem55 As ToolStripMenuItem
    Friend WithEvents MenuButtonItem56 As ToolStripMenuItem
    Friend WithEvents MenuButtonItem57 As ToolStripMenuItem
    Friend WithEvents MenuButtonItem58 As ToolStripMenuItem
    Friend WithEvents MenuButtonItem59 As ToolStripMenuItem
    Friend WithEvents MenuButtonItem60 As ToolStripMenuItem
    Friend WithEvents MenuButtonItem61 As ToolStripMenuItem
    Friend WithEvents MenuButtonItem62 As ToolStripMenuItem
    Friend WithEvents lblWorkingProvinceName As System.Windows.Forms.Label
    Friend WithEvents lblAppType As System.Windows.Forms.Label
    Friend WithEvents sbpArea As System.Windows.Forms.StatusBarPanel
    Friend WithEvents sbpUserName As System.Windows.Forms.StatusBarPanel
    Friend WithEvents sbpTimers As System.Windows.Forms.StatusBarPanel
    Friend WithEvents sbpInfo As System.Windows.Forms.StatusBarPanel
    Friend WithEvents MenuButtonItem65 As ToolStripMenuItem
    Friend WithEvents mnuManagerSMSST As ToolStripMenuItem
    Friend WithEvents mnuManagerSMSDC As ToolStripMenuItem
    Friend WithEvents mnuDeleteRequest As ToolStripMenuItem
    Friend WithEvents mnuDeletePostFeeder As ToolStripMenuItem
    Friend WithEvents mnuDeleteFeeder As ToolStripMenuItem
    Friend WithEvents mnuDeleteLPFeeder As ToolStripMenuItem
    Friend WithEvents mnuBillBoardSlides As ToolStripMenuItem
    Friend WithEvents mnuHomaMonitoring As ToolStripMenuItem
    Friend WithEvents mnuRepQuestionnaire As FavToolStripMenuItem
    Friend WithEvents mnuRepQuestionnaire2 As FavToolStripMenuItem
    Friend WithEvents mnuReport_9_17 As FavToolStripMenuItem
    Friend WithEvents mnuReport_10_25 As FavToolStripMenuItem
    Friend WithEvents mnuReport_10_26 As FavToolStripMenuItem
    Friend WithEvents mnuDisconnectGroupSetGroup As ToolStripMenuItem
    Friend WithEvents mnuRepDisconnectMonthlyCount As FavToolStripMenuItem
    Friend WithEvents mnuRepDisconnectGroupSetGroup As FavToolStripMenuItem
    Friend WithEvents mnuRepGroupTamir As FavToolStripMenuItem
    Friend WithEvents sbpInfoAll As System.Windows.Forms.StatusBarPanel
    Friend WithEvents mnuLightDaily As FavToolStripMenuItem
    Friend WithEvents mnuLightRequestParts As FavToolStripMenuItem
    Friend WithEvents mnuLightUseParts As FavToolStripMenuItem
    Friend WithEvents mnuLightDisconnectDaily As FavToolStripMenuItem
    Friend WithEvents mnuRepSubscribers As FavToolStripMenuItem
    Friend WithEvents mnuMPPostFeederPeakMonthly As ToolStripMenuItem
    Friend WithEvents mnuMPPostFeederPeakDaily As ToolStripMenuItem
    Friend WithEvents mnuRepRequestAll As FavToolStripMenuItem
    Friend WithEvents mnuRepPowerIndex As FavToolStripMenuItem
    Friend WithEvents mnuRepPowerIndex_Mode2 As FavToolStripMenuItem
    Friend WithEvents mnuRepGroupSetGroup As FavToolStripMenuItem
    Friend WithEvents mnuMPFeederPeak As FavToolStripMenuItem
    Friend WithEvents mnuRepRequestAll2 As FavToolStripMenuItem
    Friend WithEvents mnuRepAllDisconnectFeeder As FavToolStripMenuItem
    Friend WithEvents mnuRepDCPServer121 As FavToolStripMenuItem
    Friend WithEvents mnuRep5_1_1_Byerver121 As FavToolStripMenuItem
    Friend WithEvents MenuErjaBase As ToolStripMenuItem
    Friend WithEvents mnuErjaReasonBase As ToolStripMenuItem
    Friend WithEvents menuErjaOperations As ToolStripMenuItem
    Friend WithEvents mnuMonitoringErja As ToolStripMenuItem
    Friend WithEvents mnuMonitoringTamir As ToolStripMenuItem
    Friend WithEvents mnuRepUndoneReason As FavToolStripMenuItem
    Friend WithEvents mnuLoginLog As ToolStripMenuItem
    Friend WithEvents mnuLoginFailed As ToolStripMenuItem
    Friend WithEvents mnuLPPostCapacity As FavToolStripMenuItem
    Friend WithEvents mnuLPPostFeederCapacity As FavToolStripMenuItem
    Friend WithEvents mnuRepDCRecloser As FavToolStripMenuItem
    Friend WithEvents mnuRepMPFeederPeakEnergyMonthly As FavToolStripMenuItem
    Friend WithEvents mnuRepAreaFeeders As FavToolStripMenuItem
    Friend WithEvents mnuMPPostMonthly As ToolStripMenuItem
    Friend WithEvents mnuRepLPPostNotLoad As FavToolStripMenuItem
    Friend WithEvents mnuRep_1_13 As FavToolStripMenuItem
    Friend WithEvents mnuRepMPPostMonthly As FavToolStripMenuItem
    Friend WithEvents mnuSpec As ToolStripMenuItem
    Friend WithEvents mnuLPTrans As ToolStripMenuItem
    Friend WithEvents mnuLPTransInstaller As ToolStripMenuItem
    Friend WithEvents tlbDebug As ToolStrip
    Friend WithEvents tbd_LPPost As ToolStripButton
    Friend WithEvents tbd_LPFeeder As ToolStripButton
    Friend WithEvents tbd_MPFeeder As ToolStripButton
    Friend WithEvents tbd_MPPost As ToolStripButton
    Friend WithEvents mnuRepCheckLies As FavToolStripMenuItem
    Friend WithEvents mnuDisconnectCountSubscribersArea1000 As FavToolStripMenuItem
    Friend WithEvents mnuReport_1_20 As FavToolStripMenuItem
    Friend WithEvents mnuDisconnectPower_Area As FavToolStripMenuItem
    Friend WithEvents mnuDisconnectCount_Area As FavToolStripMenuItem
    Friend WithEvents mnuRepExcelform1 As FavToolStripMenuItem
    Friend WithEvents mnuRepExcelform2 As FavToolStripMenuItem
    Friend WithEvents mnuRepExcelform3 As FavToolStripMenuItem
    Friend WithEvents mnuRepExcelform4 As FavToolStripMenuItem
    Friend WithEvents mnuRepExcelform5 As FavToolStripMenuItem
    Friend WithEvents mnuRepExcelform6 As FavToolStripMenuItem
    Friend WithEvents mnuRepExcelformDaily As FavToolStripMenuItem
    Friend WithEvents mnuRepExcelformDaily_02 As FavToolStripMenuItem
    Friend WithEvents mnuRepExcelform7 As FavToolStripMenuItem
    Friend WithEvents mnuRepExcelTavanir_01 As FavToolStripMenuItem
    Friend WithEvents mnuRepExcelImportantEvents As FavToolStripMenuItem
    Friend WithEvents mnuRepWantedInfo As FavToolStripMenuItem
    Friend WithEvents mnuRep_LPAndSerghati As FavToolStripMenuItem
    Friend WithEvents rptRep_ListenRequestCallList As FavToolStripMenuItem
    Friend WithEvents rptRep_ListenUserCallCount As FavToolStripMenuItem
    Friend WithEvents mnuRepUserLogin As FavToolStripMenuItem
    Friend WithEvents mnuRepuserAccess As FavToolStripMenuItem
    Friend WithEvents mnuRep_MPCriticalFeeder As FavToolStripMenuItem
    Friend WithEvents mnuRep_LPCriticalFeeder As FavToolStripMenuItem
    Friend WithEvents mnuRepExccelComunicationSystems As FavToolStripMenuItem
    Friend WithEvents mnuRep_LPDisconnectDaily As FavToolStripMenuItem
    Friend WithEvents mnuRep_1_17 As FavToolStripMenuItem
    Friend WithEvents mnuRepMPPostTransLoadMonthly As FavToolStripMenuItem
    Friend WithEvents mnuRepMPPostTransLoadDaily As FavToolStripMenuItem
    Friend WithEvents mnuRepMPPostTransLoadHourByHour As FavToolStripMenuItem
    Friend WithEvents mnuReportsHelp As ToolStripMenuItem
    Friend WithEvents mnuFTReason As ToolStripMenuItem
    Friend WithEvents mnuMainPeak As ToolStripMenuItem
    Friend WithEvents mnuRepMainPeak As FavToolStripMenuItem
    Friend WithEvents mnuRep_FeederParts As FavToolStripMenuItem
    Friend WithEvents mnuRep_1_18 As FavToolStripMenuItem
    Friend WithEvents mnuShowLSLogSettings As ToolStripMenuItem
    Friend WithEvents mnuRpt_3_25 As FavToolStripMenuItem
    Friend WithEvents mnuLP As Bargh_Common.FavToolStripMenuItem
    Friend WithEvents mnuLP_Mode2 As FavToolStripMenuItem
    Friend WithEvents mnuRep_7_22 As FavToolStripMenuItem
    Friend WithEvents mnuRep_7_23 As FavToolStripMenuItem
    Friend WithEvents mnuRep_7_24 As FavToolStripMenuItem
    Friend WithEvents mnuRepExcelGroupSetGroup As FavToolStripMenuItem
    Friend WithEvents mnuRepExcelDisconnectGroupSet As FavToolStripMenuItem
    Friend WithEvents mnuRepEkipPerformance As FavToolStripMenuItem
    Friend WithEvents mnuRepSumEkipPerformance As FavToolStripMenuItem
    Friend WithEvents mnuRepDeleteRequest As FavToolStripMenuItem
    Friend WithEvents mnuLPPostLoadsStat As FavToolStripMenuItem
    Friend WithEvents mnuMPPostTransLoadHours As ToolStripMenuItem
    Friend WithEvents mnuFactory As ToolStripMenuItem
    Friend WithEvents mnuMPHourly As ToolStripMenuItem
    Friend WithEvents mnuRepExcelGISLoadings As FavToolStripMenuItem
    Friend WithEvents mnuRpt_3_26 As FavToolStripMenuItem
    Friend WithEvents mnuReportSMS As ToolStripMenuItem
    Friend WithEvents mnuHomaBase As ToolStripMenuItem
    Friend WithEvents mnuHomaTablet As ToolStripMenuItem
    Friend WithEvents mnuRepCountLPPost As FavToolStripMenuItem
    Friend WithEvents mnu_Num_3_27 As FavToolStripMenuItem
    Friend WithEvents MenuButtonItem27 As FavToolStripMenuItem
    Friend WithEvents mnuRepDelayControl As FavToolStripMenuItem
    Friend WithEvents mnuRepExcelformDaily_03 As FavToolStripMenuItem
    Friend WithEvents rptRep_ListenUserCallCount2 As FavToolStripMenuItem
    Friend WithEvents mnuRepExcelTavanir_02 As FavToolStripMenuItem
    Friend WithEvents mnuRepExcelTavanir_03 As FavToolStripMenuItem
    Friend WithEvents mnuRepExcelformDaily_04 As FavToolStripMenuItem
    Friend WithEvents mnuRepExcelformDaily_05 As FavToolStripMenuItem
    Friend WithEvents mnuRepExcelweek As FavToolStripMenuItem
    Friend WithEvents mnuRepMP_Mode5 As FavToolStripMenuItem
    Friend WithEvents mnuRepMP_Mode7 As FavToolStripMenuItem
    Friend WithEvents mnuRep_3_37 As FavToolStripMenuItem
    Friend WithEvents mnuRep_3_38 As FavToolStripMenuItem
    Friend WithEvents mnuRepExcelLPFeederPayesh As FavToolStripMenuItem
    Friend WithEvents mnuSerghatPart As FavToolStripMenuItem
    Friend WithEvents mnuUtilizationFactor As FavToolStripMenuItem
    Friend WithEvents mnuLPPostEarths As FavToolStripMenuItem
    Friend WithEvents mnuReport_8_13 As FavToolStripMenuItem
    Friend WithEvents tlbCallCenter As ToolStrip
    Friend WithEvents tb_CallConnect As ToolStripButton
    Friend WithEvents tb_CallDisconnect As ToolStripButton
    Friend WithEvents mnuTip As ToolStripMenuItem
    Friend WithEvents mnuAbout As ToolStripMenuItem
    Friend WithEvents mnuThriftPower As FavToolStripMenuItem
    Friend WithEvents mnuReport_10_31 As FavToolStripMenuItem
    Friend WithEvents mnuReport_10_17 As FavToolStripMenuItem
    Friend WithEvents mnuReport_9_16 As FavToolStripMenuItem
    Friend WithEvents mnuReport_10_19 As FavToolStripMenuItem
    Friend WithEvents mnuRepExcelMPFeederManagement As FavToolStripMenuItem
    Friend WithEvents mnuRepExcelLPFeederManagement As FavToolStripMenuItem
    Friend WithEvents mnuMinCountFeederPart As FavToolStripMenuItem
    Friend WithEvents mnuLPPostFeederPartCount As FavToolStripMenuItem
    Friend WithEvents mnuRptHistoryMPFeeder As FavToolStripMenuItem
    Friend WithEvents mnuRptHistoryLPPost As FavToolStripMenuItem
    Friend WithEvents MenuButtonItem29 As FavToolStripMenuItem
    Friend WithEvents MenuButtonItem45 As FavToolStripMenuItem
    Friend WithEvents MnuReport_4_22 As FavToolStripMenuItem
    Friend WithEvents MnuReport_4_23 As FavToolStripMenuItem
    Friend WithEvents MnuReport_4_24 As FavToolStripMenuItem
    Friend WithEvents MnuReport_4_25 As FavToolStripMenuItem
    Friend WithEvents MenuButtonItem47 As FavToolStripMenuItem
    Friend WithEvents mnuMPFeederMonthly As ToolStripMenuItem
    Friend WithEvents mnuRepExcelMPFeederDaily As FavToolStripMenuItem
    Friend WithEvents mnuHavadesConfig As ToolStripMenuItem
    Friend WithEvents mnuSecuritySetting As ToolStripMenuItem
    Friend WithEvents mnuConfig As ToolStripMenuItem
    Friend WithEvents MenuButtonItem48 As ToolStripMenuItem
    Friend WithEvents mnuRepUndoneReasonArea As FavToolStripMenuItem
    Friend WithEvents munRepDisconnectGroupSet As ToolStripMenuItem
    Friend WithEvents munRep_10_15_1 As FavToolStripMenuItem
    Friend WithEvents munRep_10_7_3 As FavToolStripMenuItem
    Friend WithEvents mnuRepDisconnectMonthlyCountMode2 As FavToolStripMenuItem
    Friend WithEvents mnuMPFeederDisconnetInfo As FavToolStripMenuItem
    Friend WithEvents mnuRep_7_27 As FavToolStripMenuItem
    Friend WithEvents mnuRep_7_28 As FavToolStripMenuItem
    Friend WithEvents mnuRepCompare As ToolStripMenuItem
    Friend WithEvents mnuRepCompareDCCount As FavToolStripMenuItem
    Friend WithEvents mnuRepCompareDCEnergy As FavToolStripMenuItem
    Friend WithEvents mnuRep_11_5 As FavToolStripMenuItem
    Friend WithEvents munRep_11_2 As FavToolStripMenuItem
    Friend WithEvents mnuRep_11_3 As FavToolStripMenuItem
    Friend WithEvents mnuRep_11_6 As FavToolStripMenuItem
    Friend WithEvents mnuRep_11_7 As FavToolStripMenuItem
    Friend WithEvents TimerSMSPopup As System.Windows.Forms.Timer
    Friend WithEvents TimerAVL As System.Windows.Forms.Timer
    Friend WithEvents mnuRep_11_8 As FavToolStripMenuItem
    Friend WithEvents mnuRep_11_9 As FavToolStripMenuItem
    Friend WithEvents mnuRep_11_10 As FavToolStripMenuItem
    Friend WithEvents mnuRepLPTransAction As FavToolStripMenuItem
    Friend WithEvents mnuFrmNetworkMonthlyInfo As ToolStripMenuItem
    Friend WithEvents mnuFrmGoal As ToolStripMenuItem
    Friend WithEvents MenuItem9_12 As FavToolStripMenuItem
    Friend WithEvents mnuReport_10_15 As FavToolStripMenuItem
    Friend WithEvents mnuRep_8_14 As FavToolStripMenuItem
    Friend WithEvents mnuReport_8_15 As FavToolStripMenuItem
    Friend WithEvents mnuReport_8_16 As FavToolStripMenuItem
    Friend WithEvents MenuButtonItem63 As FavToolStripMenuItem
    Friend WithEvents mnuDailyNetworkState As FavToolStripMenuItem
    Friend WithEvents MenuButtonItem64 As FavToolStripMenuItem
    Friend WithEvents mnuRptEzamEkipInfo_Mode2 As FavToolStripMenuItem
    Friend WithEvents MenuButtonItem66 As FavToolStripMenuItem
    Friend WithEvents mnuReportDisconnectFeeder_Mode2 As FavToolStripMenuItem
    Friend WithEvents mnuRep_10_21 As FavToolStripMenuItem
    Friend WithEvents mnuReportLPPostEarth As FavToolStripMenuItem
    Friend WithEvents mnuRep_10_23 As FavToolStripMenuItem
    Friend WithEvents tscMain As ToolStripContainer
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents mnuSendMobileAppNotification As ToolStripMenuItem
    Friend WithEvents tlbMain As System.Windows.Forms.ToolStrip
    Friend WithEvents tbRequestsPreview As System.Windows.Forms.ToolStripButton
    Friend WithEvents tbLightRequestsPreview As System.Windows.Forms.ToolStripButton
    Friend WithEvents tbNewRequest As System.Windows.Forms.ToolStripButton
    Friend WithEvents tbMessages As System.Windows.Forms.ToolStripButton
    Friend WithEvents tbSubscribers As System.Windows.Forms.ToolStripButton
    Friend WithEvents tbPhonebook As System.Windows.Forms.ToolStripButton
    Friend WithEvents tbChangeUser As System.Windows.Forms.ToolStripButton
    Friend WithEvents tbQuit As System.Windows.Forms.ToolStripButton
    Friend WithEvents mnuSaveSeparator As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuSaveFav As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator52 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator53 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator54 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator55 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator56 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator57 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator58 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator48 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator49 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator50 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator46 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator47 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator45 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator8 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator9 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator10 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator11 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator14 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator13 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator12 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator18 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator17 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator16 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator15 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator21 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator20 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator19 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator22 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator23 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator24 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator25 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator32 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator33 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator34 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator35 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator36 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator37 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator38 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator39 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator40 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator41 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator42 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator43 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator44 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator29 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator30 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator31 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator26 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator27 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator28 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator51 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator59 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator60 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuReportAVL As FavToolStripMenuItem
    Friend WithEvents mnuReport_9_20 As FavToolStripMenuItem
    Friend WithEvents mnuMPFeederPartLoadHour As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuRep_10_28 As Bargh_Common.FavToolStripMenuItem
    Friend WithEvents mnuRep_10_27 As Bargh_Common.FavToolStripMenuItem
    Friend WithEvents mnuChangeSMSBody As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator61 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator545 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuAllowValidator As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnu_Num_3_34 As Bargh_Common.FavToolStripMenuItem
    Friend WithEvents mnuRep_10_29 As Bargh_Common.FavToolStripMenuItem
    Friend WithEvents mnuRep_10_30 As Bargh_Common.FavToolStripMenuItem
    Friend WithEvents mnuReport_8_19 As Bargh_Common.FavToolStripMenuItem
    Friend WithEvents mnuSubscriberSmsSetting As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator62 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuReport_8_20 As Bargh_Common.FavToolStripMenuItem
    Friend WithEvents mnuReport_8_22 As Bargh_Common.FavToolStripMenuItem
    Friend WithEvents mnuReport_8_23 As Bargh_Common.FavToolStripMenuItem
    Friend WithEvents mnuReport_8_24 As Bargh_Common.FavToolStripMenuItem
    Friend WithEvents ToolStripSeparator63 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuGlobalNetworkPeakHour As ToolStripMenuItem
    Friend WithEvents mnuRptDisFogh As Bargh_Common.FavToolStripMenuItem
    Friend WithEvents mnuRptManourveFogh As Bargh_Common.FavToolStripMenuItem
    Friend WithEvents mnuRpt_2_3 As Bargh_Common.FavToolStripMenuItem
    Friend WithEvents ToolStripSeparator64 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuAreaPeak As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator65 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator70 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator66 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator67 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator68 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator69 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator71 As ToolStripSeparator
    Friend WithEvents mnuStoreOperatin As ToolStripMenuItem
    Friend WithEvents mnuIVRCodes As ToolStripMenuItem
    Friend WithEvents mnulSpecialCallType As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator72 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuGenericParts As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator73 As ToolStripSeparator
    Friend WithEvents mnuReportStore As ToolStripMenuItem
    Friend WithEvents mnuRepExistance As FavToolStripMenuItem
    Friend WithEvents mnuRepCartex As FavToolStripMenuItem
    Friend WithEvents mnuRep_SMS_1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuHomaTabletUser As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuHomaRegisterBilling As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuRep_SMS_2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator74 As ToolStripSeparator

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.TimerHcls = New System.Windows.Forms.Timer(Me.components)
        Me.SqlDataAdapter1 = New System.Data.SqlClient.SqlDataAdapter()
        Me.SqlDeleteCommand1 = New System.Data.SqlClient.SqlCommand()
        Me.SqlConnection1 = New System.Data.SqlClient.SqlConnection()
        Me.SqlInsertCommand1 = New System.Data.SqlClient.SqlCommand()
        Me.SqlSelectCommand1 = New System.Data.SqlClient.SqlCommand()
        Me.SqlUpdateCommand1 = New System.Data.SqlClient.SqlCommand()
        Me.MenuItem19 = New System.Windows.Forms.MenuItem()
        Me.TimerNewMessages = New System.Windows.Forms.Timer(Me.components)
        Me.ImageListMain = New System.Windows.Forms.ImageList(Me.components)
        Me.TimerCallPooling = New System.Windows.Forms.Timer(Me.components)
        Me.TimerMessagePooling = New System.Windows.Forms.Timer(Me.components)
        Me.TimerHardLockCheckLaunch = New System.Windows.Forms.Timer(Me.components)
        Me.TimerHardLockCheckPooling1 = New System.Windows.Forms.Timer(Me.components)
        Me.TimerHardLockCheckPooling2 = New System.Windows.Forms.Timer(Me.components)
        Me.MnuUISkin1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItem50 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItem51 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItem58 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItem59 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItem60 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItem61 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItem62 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItem52 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuUISkin2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItem55 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItem56 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItem57 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuUISkin3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuUISkin4 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItem53 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItem54 = New System.Windows.Forms.ToolStripMenuItem()
        Me.HlpProvider = New System.Windows.Forms.HelpProvider()
        Me.TimerNewRequest = New System.Windows.Forms.Timer(Me.components)
        Me.TimerSyncDT = New System.Windows.Forms.Timer(Me.components)
        Me.MenuButtonItem17 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItem18 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItem19 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItem14 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItem31 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItem37 = New System.Windows.Forms.ToolStripMenuItem()
        Me.imgListMisc = New System.Windows.Forms.ImageList(Me.components)
        Me.MenuButtonItem65 = New System.Windows.Forms.ToolStripMenuItem()
        Me.TimerSMSPopup = New System.Windows.Forms.Timer(Me.components)
        Me.TimerAVL = New System.Windows.Forms.Timer(Me.components)
        Me.tscMain = New System.Windows.Forms.ToolStripContainer()
        Me.tlbMain = New System.Windows.Forms.ToolStrip()
        Me.tbRequestsPreview = New System.Windows.Forms.ToolStripButton()
        Me.tbLightRequestsPreview = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.tbNewRequest = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator51 = New System.Windows.Forms.ToolStripSeparator()
        Me.tbMessages = New System.Windows.Forms.ToolStripButton()
        Me.tbSubscribers = New System.Windows.Forms.ToolStripButton()
        Me.tbPhonebook = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator59 = New System.Windows.Forms.ToolStripSeparator()
        Me.tbChangeUser = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator60 = New System.Windows.Forms.ToolStripSeparator()
        Me.tbQuit = New System.Windows.Forms.ToolStripButton()
        Me.tlbCallCenter = New System.Windows.Forms.ToolStrip()
        Me.tb_CallConnect = New System.Windows.Forms.ToolStripButton()
        Me.tb_CallDisconnect = New System.Windows.Forms.ToolStripButton()
        Me.tlbDebug = New System.Windows.Forms.ToolStrip()
        Me.tbd_MPPost = New System.Windows.Forms.ToolStripButton()
        Me.tbd_MPFeeder = New System.Windows.Forms.ToolStripButton()
        Me.tbd_LPPost = New System.Windows.Forms.ToolStripButton()
        Me.tbd_LPFeeder = New System.Windows.Forms.ToolStripButton()
        Me.HardLockUSB = New AxTINYLib.AxTiny()
        Me.pnlDownloadBox = New System.Windows.Forms.Panel()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.lblAppType = New System.Windows.Forms.Label()
        Me.MenuBar1 = New System.Windows.Forms.MenuStrip()
        Me.MenuItem4 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem86 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator52 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuItemMonitoring = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItemMonitoringLight = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuBillBoard = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuBillBoardSlides = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuMonitoringErja = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuMonitoringTamir = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuHomaMonitoring = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator71 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuStoreOperatin = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator53 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuDeleteRequest = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuDeleteFeeder = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuDeletePostFeeder = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuDeleteLPFeeder = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator54 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuFileMessages = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem17 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator55 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuButtonItem13 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem6 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator56 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuButtonItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItem3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator75 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuMap121andIGMC = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator57 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuRequestMultiDC = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator78 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuToolsRelogin = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator58 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuFileExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem22 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem92 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem117 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItem10 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItem16 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItem20 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItem21 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSepLPTRans = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuFreeTrans = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTransLogs = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem15 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuMPPostTransLoadHours = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem110 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuMPFeederPartLoadHour = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator48 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuButtonItem11 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator49 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuButtonItem12 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator50 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuButtonItem98 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem97 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuMPPostFeederPeakMonthly = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuMPPostFeederPeakDaily = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator64 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuMainPeak = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAreaPeak = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator46 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuMPHourly = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator47 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuMPPostMonthly = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuMPFeederMonthly = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator63 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuGlobalNetworkPeakHour = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuInform = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSMSSettings = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator77 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuInformMonitoring = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSubecribersSourceSetting = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator45 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuLightRequestSmsSetting = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSubscriberSmsSetting = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator62 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuManagerSMSST = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuManagerSMSDC = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuReportSMS = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuRep_SMS_1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuRep_SMS_2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuSmsContactsInfos = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSendSmsPanel = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSmsContactsSemat = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSmsContactsGroups = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSmsContacts = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPeopleVoice = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuSendMobileAppNotification = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator61 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuChangeSMSBody = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator545 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuCCIntelligent = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSurvey = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSurveyIVR = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuGuidSMS = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTrackingCodeSettings = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSMSErjaDone = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSMSAfterEzamEkip = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSurvyReport = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuHoma = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuHomaBase = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuHomaTabletUser = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuHomaTablet = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuHomaRegisterBilling = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuRepotEkipTrace = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuRep_h_1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReportRequests = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuRptRequests = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRep_1_13 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRep_1_17 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRep_1_18 = New Bargh_Common.FavToolStripMenuItem()
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuRptChangeHistory = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRptEzamEkipInfo = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRptEzamEkipInfo_Mode2 = New Bargh_Common.FavToolStripMenuItem()
        Me.ToolStripSeparator9 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuRptReferToInfo = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRptReferToCountArea = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRptReferToCount = New Bargh_Common.FavToolStripMenuItem()
        Me.ToolStripSeparator10 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuRepRequestAll2 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepRequestAll = New Bargh_Common.FavToolStripMenuItem()
        Me.ToolStripSeparator11 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuDisconnectPower_Zone = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuDisconnectCount_Zone = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRequestCountDaily = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuDisconnectCountSubscribers1000 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuDisconnectPower_Area = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuDisconnectCount_Area = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuDisconnectCountSubscribersArea1000 = New Bargh_Common.FavToolStripMenuItem()
        Me.ToolStripSeparator72 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuReport_1_20 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuItemFogh = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuRptDisFogh = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRpt_2_3 = New Bargh_Common.FavToolStripMenuItem()
        Me.ToolStripSeparator74 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuRptManourveFogh = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepMPRequest = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem79 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnu_Num_3_27 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnu_Num_3_34 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepMPDisconnectLPPost = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuItem78 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuItem77 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuButtonItem41 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepMP_Mode4 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepMP_Mode5 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuButtonItem63 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepMP_Mode7 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRep_3_37 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRep_3_38 = New Bargh_Common.FavToolStripMenuItem()
        Me.ToolStripSeparator14 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuItem76 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuItem75 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuButtonItem27 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuButtonItem29 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuMulitiStepConnection = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRptManoeurve = New Bargh_Common.FavToolStripMenuItem()
        Me.ToolStripSeparator13 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuMinCountMP = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuMinCountMPArea = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuMinCountFeederPart = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuLPPostFeederPartCount = New Bargh_Common.FavToolStripMenuItem()
        Me.ToolStripSeparator12 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuItem74 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuButtonItem39 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuButtonItem40 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuItem73 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuItem152 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuItem157 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuItem156 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuItem155 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuItem149 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuItem153 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuItem150 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuItem154 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuItem151 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRpt_3_25 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRpt_3_26 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepLPRequest = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuLP = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuLP_Mode1 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuLP_Mode2 = New Bargh_Common.FavToolStripMenuItem()
        Me.MnuReport_4_22 = New Bargh_Common.FavToolStripMenuItem()
        Me.MnuReport_4_23 = New Bargh_Common.FavToolStripMenuItem()
        Me.ToolStripSeparator18 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuItem56 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuItem57 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuButtonItem45 = New Bargh_Common.FavToolStripMenuItem()
        Me.ToolStripSeparator17 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuMinCountLPPost = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuMinCountLPFeeder = New Bargh_Common.FavToolStripMenuItem()
        Me.MnuReport_4_26 = New Bargh_Common.FavToolStripMenuItem()
        Me.ToolStripSeparator16 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuItem54 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuItem65 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuButtonItem36 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuButtonItem38 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuItem136 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuItem139 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuItem100 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuItem137 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuItem101 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuItem138 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuItem118 = New Bargh_Common.FavToolStripMenuItem()
        Me.ToolStripSeparator15 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuRep_LPDisconnectDaily = New Bargh_Common.FavToolStripMenuItem()
        Me.MnuReport_4_24 = New Bargh_Common.FavToolStripMenuItem()
        Me.MnuReport_4_25 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepLightRequest = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuLightDaily = New Bargh_Common.FavToolStripMenuItem()
        Me.ToolStripSeparator21 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuLightRequestParts = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuLightUseParts = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuButtonItem47 = New Bargh_Common.FavToolStripMenuItem()
        Me.ToolStripSeparator20 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuRepportSummaryLight = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuLightDisconnectDaily = New Bargh_Common.FavToolStripMenuItem()
        Me.ToolStripSeparator19 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuItem80 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuItem81 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuItem140 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuItem142 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuItem143 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuItem144 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuItem145 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuItem146 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuItem147 = New Bargh_Common.FavToolStripMenuItem()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuRepBaseInfo = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem121 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuItem122 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuItem126 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuReportLPTrans = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuItem127 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuItem124 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuItem125 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuItem129 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRep_FeederParts = New Bargh_Common.FavToolStripMenuItem()
        Me.ToolStripSeparator22 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuLPPostCapacity = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuLPPostFeederCapacity = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepAreaFeeders = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepCountLPPost = New Bargh_Common.FavToolStripMenuItem()
        Me.ToolStripSeparator23 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuRep_MPCriticalFeeder = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRep_LPCriticalFeeder = New Bargh_Common.FavToolStripMenuItem()
        Me.ToolStripSeparator24 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuRptHistoryMPFeeder = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRptHistoryLPPost = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepLPTransAction = New Bargh_Common.FavToolStripMenuItem()
        Me.ToolStripSeparator25 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuButtonItem64 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuReportStore = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuRepExistance = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepCartex = New Bargh_Common.FavToolStripMenuItem()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuRepDorehei = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem47 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuItem107 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuButtonItem5 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuButtonItem9 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuReport5_1_1_mode2 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRep_7_28 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuButtonItem4 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuButtonItem23 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuButtonItem42 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuButtonItem43 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuButtonItem24 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuButtonItem25 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepDispaching = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepMPStatusChart = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuMPFeederDisconnetInfo = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepDisconnectMonthlyCount = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepDisconnectMonthlyCountMode2 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepDisconnectGroupSetGroup = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepGroupTamir = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRep5_1_1_Byerver121 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepDCRecloser = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRep_7_21 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRep_7_22 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRep_7_23 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRep_7_24 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRep_7_27 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepLoading = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItem33 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuLPFedersLoad = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuMPFeederPeak = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepMPFeederPeakEnergyMonthly = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepLPPostNotLoad = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepMPPostMonthly = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepMPPostTransLoadMonthly = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepMPPostTransLoadDaily = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepMainPeak = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuLPPostLoadsStat = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuUtilizationFactor = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRep_8_14 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuReport_8_15 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuReport_8_16 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuButtonItem66 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuReport_8_19 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuReport_8_20 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepMPPostTransLoadHourByHour = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuReport_8_22 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuReport_8_23 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuReport_8_24 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepEarting = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuLPPostEarths = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuReport_8_13 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuReportLPPostEarth = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuControlReports = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem111 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuItem9_12 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuItem112 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuItem113 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepCheckLies = New Bargh_Common.FavToolStripMenuItem()
        Me.rptRep_ListenRequestCallList = New Bargh_Common.FavToolStripMenuItem()
        Me.rptRep_ListenUserCallCount = New Bargh_Common.FavToolStripMenuItem()
        Me.rptRep_ListenUserCallCount2 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepUserLogin = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepuserAccess = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepEkipPerformance = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepDelayControl = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepDeleteRequest = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuReportAVL = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuReport_9_20 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepSumEkipPerformance = New Bargh_Common.FavToolStripMenuItem()
        Me.ToolStripSeparator65 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuReport_9_16 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuReport_9_17 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepQuestionnaire = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepQuestionnaire2 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepOtherReport = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItem28 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuReportHourByHour = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepSubscribers = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepPowerIndex = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepPowerIndex_Mode2 = New Bargh_Common.FavToolStripMenuItem()
        Me.munRepDisconnectGroupSet = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuRepGroupSetGroup = New Bargh_Common.FavToolStripMenuItem()
        Me.munRep_10_15_1 = New Bargh_Common.FavToolStripMenuItem()
        Me.munRep_10_7_3 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepAllDisconnectFeeder = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuReportDisconnectFeeder_Mode2 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepDCPServer121 = New Bargh_Common.FavToolStripMenuItem()
        Me.MenuButtonItem48 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuRepUndoneReason = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepUndoneReasonArea = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepWantedInfo = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRep_LPAndSerghati = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuSerghatPart = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuThriftPower = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuReport_10_31 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuReport_10_15 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuReport_10_17 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuReport_10_25 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuReport_10_26 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuReport_10_19 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRep_10_21 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRep_10_23 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRep_10_27 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRep_10_28 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRep_10_29 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRep_10_30 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepCompare = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuRepCompareDCCount = New Bargh_Common.FavToolStripMenuItem()
        Me.munRep_11_2 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRep_11_3 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepCompareDCEnergy = New Bargh_Common.FavToolStripMenuItem()
        Me.ToolStripSeparator70 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuRep_11_5 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRep_11_6 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRep_11_7 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRep_11_8 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRep_11_9 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRep_11_10 = New Bargh_Common.FavToolStripMenuItem()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuExcelReports = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuRepExcelform1 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepExcelform2 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepExcelform3 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepExcelform4 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepExcelform5 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepExcelform6 = New Bargh_Common.FavToolStripMenuItem()
        Me.ToolStripSeparator66 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuRepExcelformDaily = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepExcelformDaily_02 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepExcelformDaily_03 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepExcelformDaily_04 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepExcelformDaily_05 = New Bargh_Common.FavToolStripMenuItem()
        Me.ToolStripSeparator67 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuRepExcelweek = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepExcelLPFeederPayesh = New Bargh_Common.FavToolStripMenuItem()
        Me.ToolStripSeparator68 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuRepExcelTavanir_01 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepExcelTavanir_02 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepExcelTavanir_03 = New Bargh_Common.FavToolStripMenuItem()
        Me.ToolStripSeparator69 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuRepExcelMPFeederManagement = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepExcelLPFeederManagement = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepExcelMPFeederDaily = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepExcelform7 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepExcelImportantEvents = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepExcelGroupSetGroup = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepExcelDisconnectGroupSet = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepExccelComunicationSystems = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuRepExcelGISLoadings = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuDailyNetworkState = New Bargh_Common.FavToolStripMenuItem()
        Me.ToolStripSeparator76 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuExtractionReports = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReport_13_1 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuReport_13_2 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuReport_13_3 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuReport_13_4 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuTavanirReports = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReport_14_1 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuReport_14_2 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuReport_14_3 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuReport_14_4 = New Bargh_Common.FavToolStripMenuItem()
        Me.mnuSaveSeparator = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuSimaPortal = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSepMisc = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuSaveFav = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuBaseTables = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem84 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem83 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem27 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator32 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuItem8 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem11 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem67 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuUseType = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem60 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSection = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTown = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuVillage = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuRoosta = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuIVRCodes = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem5 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem7 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem38 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItemPublicTelNumbers = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator33 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuItem9 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem51 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem59 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator34 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuButtonItem26 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem29 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem70 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItem44 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator35 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuItem69 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem68 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFeederPart = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuMPFeederKey = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator36 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuItem50 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuLPTrans = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuLPTransInstaller = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator37 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuButtonItem34 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator73 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuGenericParts = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem30 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFactory = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator38 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuFrmNetworkMonthlyInfo = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFrmGoal = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator39 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuItem48 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFTReason = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem40 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem41 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem71 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem94 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItemTbl_MPTamirDisconnectFor = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItemTbl_MPTamirRequestType = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItemTbl_DisconnectMPRequestFor = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItem7 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItem8 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem14 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem52 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItemTbl_LPCloserType = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItemTbl_LPFaultType = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItemTbl_LPTamirDisconnectFor = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItemTbl_LPTamirRequestType = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItemTbl_DisconnectLPRequestFor = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem44 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem25 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem63 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItemTbl_DisconnectLightRequestFor = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItem30 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem64 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem46 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator40 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuDisconnectGroupSetGroup = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator41 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuItem32 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonEkipProfile = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAVLCar = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAVLState = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator42 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuQuestion = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator44 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuSpec = New System.Windows.Forms.ToolStripMenuItem()
        Me.ManuOtherBase = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem39 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem82 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem34 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItem35 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem42 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItem22 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItemGroupPart = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem36 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAllowValidator = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnulSpecialCallType = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuErjaBase = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuErjaReasonBase = New System.Windows.Forms.ToolStripMenuItem()
        Me.menuErjaOperations = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuTools = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuConfig = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuToolsChangePassword = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuHavadesConfig = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSecuritySetting = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator29 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuToolsUserManagement = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuGroupTrustee = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator30 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuLoginLog = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuLoginFailed = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuUISkin = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuNewRequestConfig = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuIsKeyboardBase = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator31 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuConfiguration = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuShowInfo = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItemGlobalValue = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItem6 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuShowLSLogSettings = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem21 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuButtonItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator26 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuVersionInfo = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReportsHelp = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator27 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuTip = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator28 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuAbout = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator43 = New System.Windows.Forms.ToolStripSeparator()
        Me.StatusBar1 = New System.Windows.Forms.StatusBar()
        Me.sbpUserName = New System.Windows.Forms.StatusBarPanel()
        Me.sbpTimers = New System.Windows.Forms.StatusBarPanel()
        Me.sbpInfoAll = New System.Windows.Forms.StatusBarPanel()
        Me.sbpInfo = New System.Windows.Forms.StatusBarPanel()
        Me.sbpArea = New System.Windows.Forms.StatusBarPanel()
        Me.lblWorkingProvinceName = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuRecloserReport = New System.Windows.Forms.ToolStripMenuItem()
        Me.tscMain.ContentPanel.SuspendLayout
        Me.tscMain.SuspendLayout
        Me.tlbMain.SuspendLayout
        Me.tlbCallCenter.SuspendLayout
        Me.tlbDebug.SuspendLayout
        CType(Me.HardLockUSB, System.ComponentModel.ISupportInitialize).BeginInit
        Me.GroupBox2.SuspendLayout
        Me.MenuBar1.SuspendLayout
        CType(Me.sbpUserName, System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.sbpTimers, System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.sbpInfoAll, System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.sbpInfo, System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.sbpArea, System.ComponentModel.ISupportInitialize).BeginInit
        Me.SuspendLayout
        '
        'HelpMaker
        '
        Me.HelpMaker.HelpNamespace = "G:\Hashemi\My Works\Sabte Havades\CHM\v1.0.chm"
        '
        'TimerHcls
        '
        Me.TimerHcls.Enabled = True
        Me.TimerHcls.Interval = 5000
        '
        'SqlDataAdapter1
        '
        Me.SqlDataAdapter1.DeleteCommand = Me.SqlDeleteCommand1
        Me.SqlDataAdapter1.InsertCommand = Me.SqlInsertCommand1
        Me.SqlDataAdapter1.SelectCommand = Me.SqlSelectCommand1
        Me.SqlDataAdapter1.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "TblReport", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("RepId", "RepId"), New System.Data.Common.DataColumnMapping("AreaId", "AreaId"), New System.Data.Common.DataColumnMapping("EventDate", "EventDate"), New System.Data.Common.DataColumnMapping("EventTime", "EventTime"), New System.Data.Common.DataColumnMapping("EventYYMM", "EventYYMM"), New System.Data.Common.DataColumnMapping("CutOffRepairTime", "CutOffRepairTime"), New System.Data.Common.DataColumnMapping("CutoffGroupId", "CutoffGroupId"), New System.Data.Common.DataColumnMapping("PrimaryReasonId", "PrimaryReasonId")})})
        Me.SqlDataAdapter1.UpdateCommand = Me.SqlUpdateCommand1
        '
        'SqlDeleteCommand1
        '
        Me.SqlDeleteCommand1.CommandText = "DELETE FROM TblReport WHERE (RepId = @Original_RepId)"
        Me.SqlDeleteCommand1.Connection = Me.SqlConnection1
        Me.SqlDeleteCommand1.Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@Original_RepId", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "RepId", System.Data.DataRowVersion.Original, Nothing)})
        '
        'SqlConnection1
        '
        Me.SqlConnection1.ConnectionString = "workstation id=BLACK;packet size=4096;user id=sa;data source=BLACK;persist securi" &
    "ty info=True;initial catalog=CcRequester;password=rose"
        Me.SqlConnection1.FireInfoMessageEventOnUserErrors = False
        '
        'SqlInsertCommand1
        '
        Me.SqlInsertCommand1.CommandText = resources.GetString("SqlInsertCommand1.CommandText")
        Me.SqlInsertCommand1.Connection = Me.SqlConnection1
        Me.SqlInsertCommand1.Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@AreaId", System.Data.SqlDbType.Int, 4, "AreaId"), New System.Data.SqlClient.SqlParameter("@EventDate", System.Data.SqlDbType.NVarChar, 10, "EventDate"), New System.Data.SqlClient.SqlParameter("@EventTime", System.Data.SqlDbType.NVarChar, 5, "EventTime"), New System.Data.SqlClient.SqlParameter("@EventYYMM", System.Data.SqlDbType.NVarChar, 10, "EventYYMM"), New System.Data.SqlClient.SqlParameter("@CutOffRepairTime", System.Data.SqlDbType.Int, 4, "CutOffRepairTime"), New System.Data.SqlClient.SqlParameter("@CutoffGroupId", System.Data.SqlDbType.Int, 4, "CutoffGroupId"), New System.Data.SqlClient.SqlParameter("@PrimaryReasonId", System.Data.SqlDbType.Int, 4, "PrimaryReasonId")})
        '
        'SqlSelectCommand1
        '
        Me.SqlSelectCommand1.CommandText = "SELECT RepId, AreaId, EventDate, EventTime, EventYYMM, CutOffRepairTime, CutoffGr" &
    "oupId, PrimaryReasonId FROM TblReport"
        Me.SqlSelectCommand1.Connection = Me.SqlConnection1
        '
        'SqlUpdateCommand1
        '
        Me.SqlUpdateCommand1.CommandText = resources.GetString("SqlUpdateCommand1.CommandText")
        Me.SqlUpdateCommand1.Connection = Me.SqlConnection1
        Me.SqlUpdateCommand1.Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@AreaId", System.Data.SqlDbType.Int, 4, "AreaId"), New System.Data.SqlClient.SqlParameter("@EventDate", System.Data.SqlDbType.NVarChar, 10, "EventDate"), New System.Data.SqlClient.SqlParameter("@EventTime", System.Data.SqlDbType.NVarChar, 5, "EventTime"), New System.Data.SqlClient.SqlParameter("@EventYYMM", System.Data.SqlDbType.NVarChar, 10, "EventYYMM"), New System.Data.SqlClient.SqlParameter("@CutOffRepairTime", System.Data.SqlDbType.Int, 4, "CutOffRepairTime"), New System.Data.SqlClient.SqlParameter("@CutoffGroupId", System.Data.SqlDbType.Int, 4, "CutoffGroupId"), New System.Data.SqlClient.SqlParameter("@PrimaryReasonId", System.Data.SqlDbType.Int, 4, "PrimaryReasonId"), New System.Data.SqlClient.SqlParameter("@Original_RepId", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "RepId", System.Data.DataRowVersion.Original, Nothing)})
        '
        'MenuItem19
        '
        Me.MenuItem19.Index = -1
        Me.MenuItem19.Text = ""
        '
        'TimerNewMessages
        '
        Me.TimerNewMessages.Interval = 500
        '
        'ImageListMain
        '
        Me.ImageListMain.ImageStream = CType(resources.GetObject("ImageListMain.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageListMain.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageListMain.Images.SetKeyName(0, "")
        Me.ImageListMain.Images.SetKeyName(1, "")
        Me.ImageListMain.Images.SetKeyName(2, "")
        Me.ImageListMain.Images.SetKeyName(3, "")
        Me.ImageListMain.Images.SetKeyName(4, "")
        Me.ImageListMain.Images.SetKeyName(5, "")
        Me.ImageListMain.Images.SetKeyName(6, "")
        Me.ImageListMain.Images.SetKeyName(7, "")
        Me.ImageListMain.Images.SetKeyName(8, "")
        Me.ImageListMain.Images.SetKeyName(9, "")
        Me.ImageListMain.Images.SetKeyName(10, "")
        Me.ImageListMain.Images.SetKeyName(11, "")
        Me.ImageListMain.Images.SetKeyName(12, "")
        Me.ImageListMain.Images.SetKeyName(13, "")
        '
        'TimerCallPooling
        '
        '
        'TimerMessagePooling
        '
        Me.TimerMessagePooling.Enabled = True
        Me.TimerMessagePooling.Interval = 20000
        '
        'TimerHardLockCheckLaunch
        '
        '
        'TimerHardLockCheckPooling1
        '
        '
        'TimerHardLockCheckPooling2
        '
        '
        'MnuUISkin1
        '
        Me.MnuUISkin1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuButtonItem50})
        Me.MnuUISkin1.Name = "MnuUISkin1"
        Me.MnuUISkin1.Size = New System.Drawing.Size(32, 19)
        Me.MnuUISkin1.Text = "&اشك"
        '
        'MenuButtonItem50
        '
        Me.MenuButtonItem50.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuButtonItem51, Me.MenuButtonItem52})
        Me.MenuButtonItem50.Name = "MenuButtonItem50"
        Me.MenuButtonItem50.Size = New System.Drawing.Size(177, 22)
        Me.MenuButtonItem50.Text = "MenuButtonItem50"
        '
        'MenuButtonItem51
        '
        Me.MenuButtonItem51.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuButtonItem58})
        Me.MenuButtonItem51.Name = "MenuButtonItem51"
        Me.MenuButtonItem51.Size = New System.Drawing.Size(177, 22)
        Me.MenuButtonItem51.Text = "MenuButtonItem51"
        '
        'MenuButtonItem58
        '
        Me.MenuButtonItem58.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuButtonItem59, Me.MenuButtonItem60})
        Me.MenuButtonItem58.Name = "MenuButtonItem58"
        Me.MenuButtonItem58.Size = New System.Drawing.Size(177, 22)
        Me.MenuButtonItem58.Text = "MenuButtonItem58"
        '
        'MenuButtonItem59
        '
        Me.MenuButtonItem59.Name = "MenuButtonItem59"
        Me.MenuButtonItem59.Size = New System.Drawing.Size(177, 22)
        Me.MenuButtonItem59.Text = "MenuButtonItem59"
        '
        'MenuButtonItem60
        '
        Me.MenuButtonItem60.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuButtonItem61, Me.MenuButtonItem62})
        Me.MenuButtonItem60.Name = "MenuButtonItem60"
        Me.MenuButtonItem60.Size = New System.Drawing.Size(177, 22)
        Me.MenuButtonItem60.Text = "MenuButtonItem60"
        '
        'MenuButtonItem61
        '
        Me.MenuButtonItem61.Name = "MenuButtonItem61"
        Me.MenuButtonItem61.Size = New System.Drawing.Size(177, 22)
        Me.MenuButtonItem61.Text = "MenuButtonItem61"
        '
        'MenuButtonItem62
        '
        Me.MenuButtonItem62.Name = "MenuButtonItem62"
        Me.MenuButtonItem62.Size = New System.Drawing.Size(177, 22)
        Me.MenuButtonItem62.Text = "MenuButtonItem62"
        '
        'MenuButtonItem52
        '
        Me.MenuButtonItem52.Name = "MenuButtonItem52"
        Me.MenuButtonItem52.Size = New System.Drawing.Size(177, 22)
        Me.MenuButtonItem52.Text = "MenuButtonItem52"
        '
        'MnuUISkin2
        '
        Me.MnuUISkin2.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuButtonItem55})
        Me.MnuUISkin2.Name = "MnuUISkin2"
        Me.MnuUISkin2.Size = New System.Drawing.Size(32, 19)
        Me.MnuUISkin2.Text = "&پيشه گاه"
        '
        'MenuButtonItem55
        '
        Me.MenuButtonItem55.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuButtonItem56, Me.MenuButtonItem57})
        Me.MenuButtonItem55.Name = "MenuButtonItem55"
        Me.MenuButtonItem55.Size = New System.Drawing.Size(177, 22)
        Me.MenuButtonItem55.Text = "MenuButtonItem55"
        '
        'MenuButtonItem56
        '
        Me.MenuButtonItem56.Name = "MenuButtonItem56"
        Me.MenuButtonItem56.Size = New System.Drawing.Size(177, 22)
        Me.MenuButtonItem56.Text = "MenuButtonItem56"
        '
        'MenuButtonItem57
        '
        Me.MenuButtonItem57.Name = "MenuButtonItem57"
        Me.MenuButtonItem57.Size = New System.Drawing.Size(177, 22)
        Me.MenuButtonItem57.Text = "MenuButtonItem57"
        '
        'MnuUISkin3
        '
        Me.MnuUISkin3.Name = "MnuUISkin3"
        Me.MnuUISkin3.Size = New System.Drawing.Size(32, 19)
        Me.MnuUISkin3.Text = "&رسانه"
        '
        'MnuUISkin4
        '
        Me.MnuUISkin4.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuButtonItem53, Me.MenuButtonItem54})
        Me.MnuUISkin4.Name = "MnuUISkin4"
        Me.MnuUISkin4.Size = New System.Drawing.Size(32, 19)
        Me.MnuUISkin4.Text = "&فلزي"
        '
        'MenuButtonItem53
        '
        Me.MenuButtonItem53.Name = "MenuButtonItem53"
        Me.MenuButtonItem53.Size = New System.Drawing.Size(177, 22)
        Me.MenuButtonItem53.Text = "MenuButtonItem53"
        '
        'MenuButtonItem54
        '
        Me.MenuButtonItem54.Name = "MenuButtonItem54"
        Me.MenuButtonItem54.Size = New System.Drawing.Size(177, 22)
        Me.MenuButtonItem54.Text = "MenuButtonItem54"
        '
        'TimerNewRequest
        '
        Me.TimerNewRequest.Enabled = True
        Me.TimerNewRequest.Interval = 6500
        '
        'TimerSyncDT
        '
        Me.TimerSyncDT.Enabled = True
        Me.TimerSyncDT.Interval = 30000
        '
        'MenuButtonItem17
        '
        Me.MenuButtonItem17.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuButtonItem18, Me.MenuButtonItem19})
        Me.MenuButtonItem17.Name = "MenuButtonItem17"
        Me.MenuButtonItem17.Size = New System.Drawing.Size(32, 19)
        Me.MenuButtonItem17.Text = "بار گيري پست ها و فيدرهاي فشار متوسط"
        '
        'MenuButtonItem18
        '
        Me.MenuButtonItem18.Name = "MenuButtonItem18"
        Me.MenuButtonItem18.Size = New System.Drawing.Size(181, 22)
        Me.MenuButtonItem18.Text = "فيدرهاي فشار متوسط"
        '
        'MenuButtonItem19
        '
        Me.MenuButtonItem19.Name = "MenuButtonItem19"
        Me.MenuButtonItem19.Size = New System.Drawing.Size(181, 22)
        Me.MenuButtonItem19.Text = "پست هاي فوق توزيع"
        '
        'MenuButtonItem14
        '
        Me.MenuButtonItem14.Image = CType(resources.GetObject("MenuButtonItem14.Image"), System.Drawing.Image)
        Me.MenuButtonItem14.Name = "MenuButtonItem14"
        Me.MenuButtonItem14.Size = New System.Drawing.Size(32, 19)
        Me.MenuButtonItem14.Text = "&مانيتورينگ"
        '
        'MenuButtonItem31
        '
        Me.MenuButtonItem31.Name = "MenuButtonItem31"
        Me.MenuButtonItem31.Size = New System.Drawing.Size(32, 19)
        Me.MenuButtonItem31.Text = "علل ق&طع فشار ضعيف"
        '
        'MenuButtonItem37
        '
        Me.MenuButtonItem37.Image = CType(resources.GetObject("MenuButtonItem37.Image"), System.Drawing.Image)
        Me.MenuButtonItem37.Name = "MenuButtonItem37"
        Me.MenuButtonItem37.Size = New System.Drawing.Size(32, 19)
        Me.MenuButtonItem37.Text = "دياگرام Pie chart عوامل قطع فيدرهاي 400 ولت"
        '
        'imgListMisc
        '
        Me.imgListMisc.ImageStream = CType(resources.GetObject("imgListMisc.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgListMisc.TransparentColor = System.Drawing.Color.Transparent
        Me.imgListMisc.Images.SetKeyName(0, "")
        Me.imgListMisc.Images.SetKeyName(1, "")
        Me.imgListMisc.Images.SetKeyName(2, "")
        Me.imgListMisc.Images.SetKeyName(3, "")
        '
        'MenuButtonItem65
        '
        Me.MenuButtonItem65.Name = "MenuButtonItem65"
        Me.MenuButtonItem65.Size = New System.Drawing.Size(32, 19)
        '
        'TimerSMSPopup
        '
        Me.TimerSMSPopup.Interval = 30000
        '
        'TimerAVL
        '
        Me.TimerAVL.Interval = 120000
        '
        'tscMain
        '
        Me.tscMain.BottomToolStripPanelVisible = False
        '
        'tscMain.ContentPanel
        '
        Me.tscMain.ContentPanel.Controls.Add(Me.tlbMain)
        Me.tscMain.ContentPanel.Controls.Add(Me.tlbCallCenter)
        Me.tscMain.ContentPanel.Controls.Add(Me.tlbDebug)
        Me.tscMain.ContentPanel.Size = New System.Drawing.Size(760, 31)
        Me.tscMain.Dock = System.Windows.Forms.DockStyle.Top
        Me.tscMain.LeftToolStripPanelVisible = False
        Me.tscMain.Location = New System.Drawing.Point(0, 24)
        Me.tscMain.Name = "tscMain"
        Me.tscMain.RightToolStripPanelVisible = False
        Me.tscMain.Size = New System.Drawing.Size(760, 56)
        Me.tscMain.TabIndex = 33
        Me.tscMain.Text = "ToolStripContainer1"
        '
        'tlbMain
        '
        Me.tlbMain.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlbMain.Dock = System.Windows.Forms.DockStyle.None
        Me.tlbMain.ImageList = Me.ImageListMain
        Me.tlbMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tbRequestsPreview, Me.tbLightRequestsPreview, Me.ToolStripSeparator4, Me.tbNewRequest, Me.ToolStripSeparator51, Me.tbMessages, Me.tbSubscribers, Me.tbPhonebook, Me.ToolStripSeparator59, Me.tbChangeUser, Me.ToolStripSeparator60, Me.tbQuit})
        Me.tlbMain.Location = New System.Drawing.Point(540, 2)
        Me.tlbMain.Name = "tlbMain"
        Me.tlbMain.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.tlbMain.Size = New System.Drawing.Size(220, 25)
        Me.tlbMain.TabIndex = 3
        Me.tlbMain.Text = "ToolStrip1"
        '
        'tbRequestsPreview
        '
        Me.tbRequestsPreview.ImageIndex = 11
        Me.tbRequestsPreview.Name = "tbRequestsPreview"
        Me.tbRequestsPreview.Size = New System.Drawing.Size(23, 22)
        Me.tbRequestsPreview.Tag = "tbRequestsPreview"
        Me.tbRequestsPreview.ToolTipText = "مانيتورينگ"
        '
        'tbLightRequestsPreview
        '
        Me.tbLightRequestsPreview.ImageIndex = 12
        Me.tbLightRequestsPreview.Name = "tbLightRequestsPreview"
        Me.tbLightRequestsPreview.Size = New System.Drawing.Size(23, 22)
        Me.tbLightRequestsPreview.Tag = "tbLightRequestsPreview"
        Me.tbLightRequestsPreview.ToolTipText = "مانيتورينگ روشنايي معابر"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(6, 25)
        '
        'tbNewRequest
        '
        Me.tbNewRequest.ImageIndex = 13
        Me.tbNewRequest.Name = "tbNewRequest"
        Me.tbNewRequest.Size = New System.Drawing.Size(23, 22)
        Me.tbNewRequest.ToolTipText = "ثبت درخواست جديد"
        '
        'ToolStripSeparator51
        '
        Me.ToolStripSeparator51.Name = "ToolStripSeparator51"
        Me.ToolStripSeparator51.Size = New System.Drawing.Size(6, 25)
        '
        'tbMessages
        '
        Me.tbMessages.ImageIndex = 6
        Me.tbMessages.Name = "tbMessages"
        Me.tbMessages.Size = New System.Drawing.Size(23, 22)
        Me.tbMessages.ToolTipText = "پيغام ها"
        '
        'tbSubscribers
        '
        Me.tbSubscribers.ImageIndex = 7
        Me.tbSubscribers.Name = "tbSubscribers"
        Me.tbSubscribers.Size = New System.Drawing.Size(23, 22)
        Me.tbSubscribers.ToolTipText = "مشتركين"
        '
        'tbPhonebook
        '
        Me.tbPhonebook.ImageIndex = 8
        Me.tbPhonebook.Name = "tbPhonebook"
        Me.tbPhonebook.Size = New System.Drawing.Size(23, 22)
        Me.tbPhonebook.ToolTipText = "دفتر تلفن"
        '
        'ToolStripSeparator59
        '
        Me.ToolStripSeparator59.Name = "ToolStripSeparator59"
        Me.ToolStripSeparator59.Size = New System.Drawing.Size(6, 25)
        '
        'tbChangeUser
        '
        Me.tbChangeUser.ImageIndex = 5
        Me.tbChangeUser.Name = "tbChangeUser"
        Me.tbChangeUser.Size = New System.Drawing.Size(23, 22)
        Me.tbChangeUser.ToolTipText = "تغيير كد كاربري"
        '
        'ToolStripSeparator60
        '
        Me.ToolStripSeparator60.Name = "ToolStripSeparator60"
        Me.ToolStripSeparator60.Size = New System.Drawing.Size(6, 25)
        '
        'tbQuit
        '
        Me.tbQuit.ImageIndex = 4
        Me.tbQuit.Name = "tbQuit"
        Me.tbQuit.Size = New System.Drawing.Size(23, 22)
        Me.tbQuit.ToolTipText = "خروج"
        '
        'tlbCallCenter
        '
        Me.tlbCallCenter.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlbCallCenter.Dock = System.Windows.Forms.DockStyle.None
        Me.tlbCallCenter.ImageList = Me.ImageListMain
        Me.tlbCallCenter.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tb_CallConnect, Me.tb_CallDisconnect})
        Me.tlbCallCenter.Location = New System.Drawing.Point(438, 24)
        Me.tlbCallCenter.Name = "tlbCallCenter"
        Me.tlbCallCenter.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.tlbCallCenter.Size = New System.Drawing.Size(58, 25)
        Me.tlbCallCenter.TabIndex = 2
        Me.tlbCallCenter.Visible = False
        '
        'tb_CallConnect
        '
        Me.tb_CallConnect.Checked = True
        Me.tb_CallConnect.CheckState = System.Windows.Forms.CheckState.Checked
        Me.tb_CallConnect.Image = CType(resources.GetObject("tb_CallConnect.Image"), System.Drawing.Image)
        Me.tb_CallConnect.Name = "tb_CallConnect"
        Me.tb_CallConnect.Size = New System.Drawing.Size(23, 22)
        Me.tb_CallConnect.Tag = "tbRequestsPreview"
        Me.tb_CallConnect.ToolTipText = "اتصال به مرکز تماس"
        '
        'tb_CallDisconnect
        '
        Me.tb_CallDisconnect.Image = CType(resources.GetObject("tb_CallDisconnect.Image"), System.Drawing.Image)
        Me.tb_CallDisconnect.Name = "tb_CallDisconnect"
        Me.tb_CallDisconnect.Size = New System.Drawing.Size(23, 22)
        Me.tb_CallDisconnect.ToolTipText = "قطع ارتباط با مرکز تماس"
        '
        'tlbDebug
        '
        Me.tlbDebug.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlbDebug.Dock = System.Windows.Forms.DockStyle.None
        Me.tlbDebug.Font = New System.Drawing.Font("Tahoma", 8.0!)
        Me.tlbDebug.ImageList = Me.ImageListMain
        Me.tlbDebug.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tbd_MPPost, Me.tbd_MPFeeder, Me.tbd_LPPost, Me.tbd_LPFeeder})
        Me.tlbDebug.Location = New System.Drawing.Point(31, 24)
        Me.tlbDebug.Name = "tlbDebug"
        Me.tlbDebug.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.tlbDebug.Size = New System.Drawing.Size(406, 25)
        Me.tlbDebug.TabIndex = 1
        Me.tlbDebug.Visible = False
        '
        'tbd_MPPost
        '
        Me.tbd_MPPost.Name = "tbd_MPPost"
        Me.tbd_MPPost.Size = New System.Drawing.Size(96, 22)
        Me.tbd_MPPost.Text = "پستهاي فوق توزيع"
        '
        'tbd_MPFeeder
        '
        Me.tbd_MPFeeder.Name = "tbd_MPFeeder"
        Me.tbd_MPFeeder.Size = New System.Drawing.Size(115, 22)
        Me.tbd_MPFeeder.Text = "فيدرهاي فشار متوسط"
        '
        'tbd_LPPost
        '
        Me.tbd_LPPost.Name = "tbd_LPPost"
        Me.tbd_LPPost.Size = New System.Drawing.Size(73, 22)
        Me.tbd_LPPost.Text = "پستهاي توزيع"
        '
        'tbd_LPFeeder
        '
        Me.tbd_LPFeeder.Name = "tbd_LPFeeder"
        Me.tbd_LPFeeder.Size = New System.Drawing.Size(110, 22)
        Me.tbd_LPFeeder.Text = "فيدرهاي فشار ضعيف"
        '
        'HardLockUSB
        '
        Me.HardLockUSB.Enabled = True
        Me.HardLockUSB.Location = New System.Drawing.Point(8, 104)
        Me.HardLockUSB.Name = "HardLockUSB"
        Me.HardLockUSB.OcxState = CType(resources.GetObject("HardLockUSB.OcxState"), System.Windows.Forms.AxHost.State)
        Me.HardLockUSB.Size = New System.Drawing.Size(100, 50)
        Me.HardLockUSB.TabIndex = 27
        Me.HardLockUSB.Visible = False
        '
        'pnlDownloadBox
        '
        Me.pnlDownloadBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.pnlDownloadBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlDownloadBox.Location = New System.Drawing.Point(8, 360)
        Me.pnlDownloadBox.Name = "pnlDownloadBox"
        Me.pnlDownloadBox.Size = New System.Drawing.Size(310, 142)
        Me.pnlDownloadBox.TabIndex = 25
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.lblAppType)
        Me.GroupBox2.Location = New System.Drawing.Point(7, 54)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(240, 49)
        Me.GroupBox2.TabIndex = 17
        Me.GroupBox2.TabStop = False
        '
        'lblAppType
        '
        Me.lblAppType.Font = New System.Drawing.Font("Titr", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblAppType.Location = New System.Drawing.Point(8, 10)
        Me.lblAppType.Name = "lblAppType"
        Me.lblAppType.Size = New System.Drawing.Size(227, 35)
        Me.lblAppType.TabIndex = 0
        Me.lblAppType.Text = "نسخه"
        Me.lblAppType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'MenuBar1
        '
        Me.MenuBar1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.MenuBar1.ImageList = Me.ImageListMain
        Me.MenuBar1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuItem4, Me.MenuFile, Me.MenuItem15, Me.mnuInform, Me.mnuHoma, Me.mnuReports, Me.MenuBaseTables, Me.MenuTools, Me.MenuItem21})
        Me.MenuBar1.Location = New System.Drawing.Point(0, 0)
        Me.MenuBar1.Name = "MenuBar1"
        Me.MenuBar1.Size = New System.Drawing.Size(760, 24)
        Me.MenuBar1.TabIndex = 0
        Me.MenuBar1.Text = "MenuBar1"
        '
        'MenuItem4
        '
        Me.MenuItem4.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuItem86, Me.ToolStripSeparator52, Me.MenuItemMonitoring, Me.MenuItemMonitoringLight, Me.mnuBillBoard, Me.mnuBillBoardSlides, Me.mnuMonitoringErja, Me.mnuMonitoringTamir, Me.mnuHomaMonitoring, Me.ToolStripSeparator71, Me.mnuStoreOperatin, Me.ToolStripSeparator53, Me.mnuDeleteRequest, Me.mnuDeleteFeeder, Me.mnuDeletePostFeeder, Me.mnuDeleteLPFeeder, Me.ToolStripSeparator54, Me.MenuFileMessages, Me.MenuItem17, Me.ToolStripSeparator55, Me.MenuButtonItem13, Me.MenuItem6, Me.ToolStripSeparator56, Me.MenuButtonItem2, Me.MenuButtonItem3, Me.ToolStripSeparator75, Me.mnuMap121andIGMC, Me.ToolStripSeparator57, Me.mnuRequestMultiDC, Me.ToolStripSeparator78, Me.MenuToolsRelogin, Me.ToolStripSeparator58, Me.MenuFileExit})
        Me.MenuItem4.Name = "MenuItem4"
        Me.MenuItem4.Size = New System.Drawing.Size(46, 20)
        Me.MenuItem4.Text = "&پرونده"
        '
        'MenuItem86
        '
        Me.MenuItem86.Image = CType(resources.GetObject("MenuItem86.Image"), System.Drawing.Image)
        Me.MenuItem86.Name = "MenuItem86"
        Me.MenuItem86.Size = New System.Drawing.Size(306, 22)
        Me.MenuItem86.Text = "ثبت &درخواست جديد..."
        '
        'ToolStripSeparator52
        '
        Me.ToolStripSeparator52.Name = "ToolStripSeparator52"
        Me.ToolStripSeparator52.Size = New System.Drawing.Size(303, 6)
        '
        'MenuItemMonitoring
        '
        Me.MenuItemMonitoring.Name = "MenuItemMonitoring"
        Me.MenuItemMonitoring.Size = New System.Drawing.Size(306, 22)
        Me.MenuItemMonitoring.Tag = "MenuItemMonitoring"
        Me.MenuItemMonitoring.Text = "مانيتورينگ"
        '
        'MenuItemMonitoringLight
        '
        Me.MenuItemMonitoringLight.Name = "MenuItemMonitoringLight"
        Me.MenuItemMonitoringLight.Size = New System.Drawing.Size(306, 22)
        Me.MenuItemMonitoringLight.Tag = "MenuItemMonitoringLight"
        Me.MenuItemMonitoringLight.Text = "مانيتورينگ روشنايي معابر"
        '
        'mnuBillBoard
        '
        Me.mnuBillBoard.Name = "mnuBillBoard"
        Me.mnuBillBoard.Size = New System.Drawing.Size(306, 22)
        Me.mnuBillBoard.Tag = "mnuBillBoard"
        Me.mnuBillBoard.Text = "مانيتورينگ نمايشگر ديواري"
        '
        'mnuBillBoardSlides
        '
        Me.mnuBillBoardSlides.Name = "mnuBillBoardSlides"
        Me.mnuBillBoardSlides.Size = New System.Drawing.Size(306, 22)
        Me.mnuBillBoardSlides.Text = "مانيتورينگ اطلاعات حوادث"
        '
        'mnuMonitoringErja
        '
        Me.mnuMonitoringErja.Name = "mnuMonitoringErja"
        Me.mnuMonitoringErja.Size = New System.Drawing.Size(306, 22)
        Me.mnuMonitoringErja.Text = "مانيتورينگ ارجاعات"
        Me.mnuMonitoringErja.Visible = False
        '
        'mnuMonitoringTamir
        '
        Me.mnuMonitoringTamir.Name = "mnuMonitoringTamir"
        Me.mnuMonitoringTamir.Size = New System.Drawing.Size(306, 22)
        Me.mnuMonitoringTamir.Text = "مانيتورينگ درخواست خاموشي"
        Me.mnuMonitoringTamir.Visible = False
        '
        'mnuHomaMonitoring
        '
        Me.mnuHomaMonitoring.Name = "mnuHomaMonitoring"
        Me.mnuHomaMonitoring.Size = New System.Drawing.Size(306, 22)
        Me.mnuHomaMonitoring.Text = "مانيتورينگ هما"
        '
        'ToolStripSeparator71
        '
        Me.ToolStripSeparator71.Name = "ToolStripSeparator71"
        Me.ToolStripSeparator71.Size = New System.Drawing.Size(303, 6)
        '
        'mnuStoreOperatin
        '
        Me.mnuStoreOperatin.Name = "mnuStoreOperatin"
        Me.mnuStoreOperatin.Size = New System.Drawing.Size(306, 22)
        Me.mnuStoreOperatin.Text = "انبارک"
        '
        'ToolStripSeparator53
        '
        Me.ToolStripSeparator53.Name = "ToolStripSeparator53"
        Me.ToolStripSeparator53.Size = New System.Drawing.Size(303, 6)
        Me.ToolStripSeparator53.Visible = False
        '
        'mnuDeleteRequest
        '
        Me.mnuDeleteRequest.Image = CType(resources.GetObject("mnuDeleteRequest.Image"), System.Drawing.Image)
        Me.mnuDeleteRequest.Name = "mnuDeleteRequest"
        Me.mnuDeleteRequest.Size = New System.Drawing.Size(306, 22)
        Me.mnuDeleteRequest.Text = "حذف پرونده"
        Me.mnuDeleteRequest.Visible = False
        '
        'mnuDeleteFeeder
        '
        Me.mnuDeleteFeeder.Image = CType(resources.GetObject("mnuDeleteFeeder.Image"), System.Drawing.Image)
        Me.mnuDeleteFeeder.Name = "mnuDeleteFeeder"
        Me.mnuDeleteFeeder.Size = New System.Drawing.Size(306, 22)
        Me.mnuDeleteFeeder.Text = "انتقال و حذف فيدر فشار متوسط"
        Me.mnuDeleteFeeder.Visible = False
        '
        'mnuDeletePostFeeder
        '
        Me.mnuDeletePostFeeder.Image = CType(resources.GetObject("mnuDeletePostFeeder.Image"), System.Drawing.Image)
        Me.mnuDeletePostFeeder.Name = "mnuDeletePostFeeder"
        Me.mnuDeletePostFeeder.Size = New System.Drawing.Size(306, 22)
        Me.mnuDeletePostFeeder.Text = "انتقال و حذف پست توزيع"
        Me.mnuDeletePostFeeder.Visible = False
        '
        'mnuDeleteLPFeeder
        '
        Me.mnuDeleteLPFeeder.Image = CType(resources.GetObject("mnuDeleteLPFeeder.Image"), System.Drawing.Image)
        Me.mnuDeleteLPFeeder.Name = "mnuDeleteLPFeeder"
        Me.mnuDeleteLPFeeder.Size = New System.Drawing.Size(306, 22)
        Me.mnuDeleteLPFeeder.Text = "انتقال و حذف فيدر فشار ضعيف"
        Me.mnuDeleteLPFeeder.Visible = False
        '
        'ToolStripSeparator54
        '
        Me.ToolStripSeparator54.Name = "ToolStripSeparator54"
        Me.ToolStripSeparator54.Size = New System.Drawing.Size(303, 6)
        '
        'MenuFileMessages
        '
        Me.MenuFileMessages.Image = CType(resources.GetObject("MenuFileMessages.Image"), System.Drawing.Image)
        Me.MenuFileMessages.Name = "MenuFileMessages"
        Me.MenuFileMessages.Size = New System.Drawing.Size(306, 22)
        Me.MenuFileMessages.Text = "&پيغام ها"
        '
        'MenuItem17
        '
        Me.MenuItem17.Image = CType(resources.GetObject("MenuItem17.Image"), System.Drawing.Image)
        Me.MenuItem17.Name = "MenuItem17"
        Me.MenuItem17.Size = New System.Drawing.Size(306, 22)
        Me.MenuItem17.Text = "دفتر &تلفن"
        '
        'ToolStripSeparator55
        '
        Me.ToolStripSeparator55.Name = "ToolStripSeparator55"
        Me.ToolStripSeparator55.Size = New System.Drawing.Size(303, 6)
        '
        'MenuButtonItem13
        '
        Me.MenuButtonItem13.Name = "MenuButtonItem13"
        Me.MenuButtonItem13.Size = New System.Drawing.Size(306, 22)
        Me.MenuButtonItem13.Text = "تعداد مشتركين"
        '
        'MenuItem6
        '
        Me.MenuItem6.Name = "MenuItem6"
        Me.MenuItem6.Size = New System.Drawing.Size(306, 22)
        Me.MenuItem6.Text = "فيدرهاي &حساس"
        '
        'ToolStripSeparator56
        '
        Me.ToolStripSeparator56.Name = "ToolStripSeparator56"
        Me.ToolStripSeparator56.Size = New System.Drawing.Size(303, 6)
        '
        'MenuButtonItem2
        '
        Me.MenuButtonItem2.Image = CType(resources.GetObject("MenuButtonItem2.Image"), System.Drawing.Image)
        Me.MenuButtonItem2.Name = "MenuButtonItem2"
        Me.MenuButtonItem2.Size = New System.Drawing.Size(306, 22)
        Me.MenuButtonItem2.Text = "ضريب &ايام هفته پستها"
        '
        'MenuButtonItem3
        '
        Me.MenuButtonItem3.Image = CType(resources.GetObject("MenuButtonItem3.Image"), System.Drawing.Image)
        Me.MenuButtonItem3.Name = "MenuButtonItem3"
        Me.MenuButtonItem3.Size = New System.Drawing.Size(306, 22)
        Me.MenuButtonItem3.Text = "ضريب ما&ههاي سال پستها"
        '
        'ToolStripSeparator75
        '
        Me.ToolStripSeparator75.Name = "ToolStripSeparator75"
        Me.ToolStripSeparator75.Size = New System.Drawing.Size(303, 6)
        '
        'mnuMap121andIGMC
        '
        Me.mnuMap121andIGMC.Name = "mnuMap121andIGMC"
        Me.mnuMap121andIGMC.Size = New System.Drawing.Size(306, 22)
        Me.mnuMap121andIGMC.Text = "تطابق اطلاعات پايه ثبت حوادث و مديريت شبکه"
        '
        'ToolStripSeparator57
        '
        Me.ToolStripSeparator57.Name = "ToolStripSeparator57"
        Me.ToolStripSeparator57.Size = New System.Drawing.Size(303, 6)
        '
        'mnuRequestMultiDC
        '
        Me.mnuRequestMultiDC.Name = "mnuRequestMultiDC"
        Me.mnuRequestMultiDC.Size = New System.Drawing.Size(306, 22)
        Me.mnuRequestMultiDC.Text = "ثبت خاموشي بابرنامه فوق توزيع به صورت دسته اي"
        '
        'ToolStripSeparator78
        '
        Me.ToolStripSeparator78.Name = "ToolStripSeparator78"
        Me.ToolStripSeparator78.Size = New System.Drawing.Size(303, 6)
        '
        'MenuToolsRelogin
        '
        Me.MenuToolsRelogin.Image = CType(resources.GetObject("MenuToolsRelogin.Image"), System.Drawing.Image)
        Me.MenuToolsRelogin.Name = "MenuToolsRelogin"
        Me.MenuToolsRelogin.Size = New System.Drawing.Size(306, 22)
        Me.MenuToolsRelogin.Text = "تغيير كد &كاربري..."
        '
        'ToolStripSeparator58
        '
        Me.ToolStripSeparator58.Name = "ToolStripSeparator58"
        Me.ToolStripSeparator58.Size = New System.Drawing.Size(303, 6)
        '
        'MenuFileExit
        '
        Me.MenuFileExit.Image = CType(resources.GetObject("MenuFileExit.Image"), System.Drawing.Image)
        Me.MenuFileExit.Name = "MenuFileExit"
        Me.MenuFileExit.Size = New System.Drawing.Size(306, 22)
        Me.MenuFileExit.Text = "&خروج"
        '
        'MenuFile
        '
        Me.MenuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuItem22, Me.MenuItem92, Me.MenuItem117, Me.MenuButtonItem10, Me.MenuButtonItem16, Me.mnuSepLPTRans, Me.mnuFreeTrans, Me.mnuTransLogs})
        Me.MenuFile.Name = "MenuFile"
        Me.MenuFile.Size = New System.Drawing.Size(75, 20)
        Me.MenuFile.Text = "فشار &ضعيف"
        '
        'MenuItem22
        '
        Me.MenuItem22.Image = CType(resources.GetObject("MenuItem22.Image"), System.Drawing.Image)
        Me.MenuItem22.Name = "MenuItem22"
        Me.MenuItem22.Size = New System.Drawing.Size(297, 22)
        Me.MenuItem22.Text = "رويدادهاي &روشنايي معابر"
        Me.MenuItem22.Visible = False
        '
        'MenuItem92
        '
        Me.MenuItem92.Name = "MenuItem92"
        Me.MenuItem92.Size = New System.Drawing.Size(297, 22)
        Me.MenuItem92.Text = "پست ها و فيدرهاي &قطع فشار ضعيف"
        Me.MenuItem92.Visible = False
        '
        'MenuItem117
        '
        Me.MenuItem117.Image = CType(resources.GetObject("MenuItem117.Image"), System.Drawing.Image)
        Me.MenuItem117.Name = "MenuItem117"
        Me.MenuItem117.Size = New System.Drawing.Size(297, 22)
        Me.MenuItem117.Text = "جداول &ضريب بار مصرفي"
        Me.MenuItem117.Visible = False
        '
        'MenuButtonItem10
        '
        Me.MenuButtonItem10.Name = "MenuButtonItem10"
        Me.MenuButtonItem10.Size = New System.Drawing.Size(297, 22)
        Me.MenuButtonItem10.Text = "فهرست مانورهاي شبکه فشار ضعيف"
        '
        'MenuButtonItem16
        '
        Me.MenuButtonItem16.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuButtonItem20, Me.MenuButtonItem21})
        Me.MenuButtonItem16.Name = "MenuButtonItem16"
        Me.MenuButtonItem16.Size = New System.Drawing.Size(297, 22)
        Me.MenuButtonItem16.Text = "بار گيري پست هاي توزيع و فيدرهاي فشار ضعيف"
        '
        'MenuButtonItem20
        '
        Me.MenuButtonItem20.Name = "MenuButtonItem20"
        Me.MenuButtonItem20.Size = New System.Drawing.Size(173, 22)
        Me.MenuButtonItem20.Text = "پست هاي توزيع"
        '
        'MenuButtonItem21
        '
        Me.MenuButtonItem21.Name = "MenuButtonItem21"
        Me.MenuButtonItem21.Size = New System.Drawing.Size(173, 22)
        Me.MenuButtonItem21.Text = "فيدرهاي فشار ضعيف"
        '
        'mnuSepLPTRans
        '
        Me.mnuSepLPTRans.Name = "mnuSepLPTRans"
        Me.mnuSepLPTRans.Size = New System.Drawing.Size(294, 6)
        '
        'mnuFreeTrans
        '
        Me.mnuFreeTrans.Name = "mnuFreeTrans"
        Me.mnuFreeTrans.Size = New System.Drawing.Size(297, 22)
        Me.mnuFreeTrans.Text = "ترانسفوماتورهای آزاد"
        '
        'mnuTransLogs
        '
        Me.mnuTransLogs.Name = "mnuTransLogs"
        Me.mnuTransLogs.Size = New System.Drawing.Size(297, 22)
        Me.mnuTransLogs.Text = "رصد ترانسفورماتورها"
        '
        'MenuItem15
        '
        Me.MenuItem15.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuMPPostTransLoadHours, Me.MenuItem110, Me.mnuMPFeederPartLoadHour, Me.ToolStripSeparator48, Me.MenuButtonItem11, Me.ToolStripSeparator49, Me.MenuButtonItem12, Me.ToolStripSeparator50, Me.MenuButtonItem98, Me.MenuItem97})
        Me.MenuItem15.Name = "MenuItem15"
        Me.MenuItem15.Size = New System.Drawing.Size(80, 20)
        Me.MenuItem15.Text = "فشار &متوسط"
        '
        'mnuMPPostTransLoadHours
        '
        Me.mnuMPPostTransLoadHours.Image = CType(resources.GetObject("mnuMPPostTransLoadHours.Image"), System.Drawing.Image)
        Me.mnuMPPostTransLoadHours.Name = "mnuMPPostTransLoadHours"
        Me.mnuMPPostTransLoadHours.Size = New System.Drawing.Size(315, 22)
        Me.mnuMPPostTransLoadHours.Text = "بار ساعت به ساعت ترانسهاي فوق توزيع"
        '
        'MenuItem110
        '
        Me.MenuItem110.Image = CType(resources.GetObject("MenuItem110.Image"), System.Drawing.Image)
        Me.MenuItem110.Name = "MenuItem110"
        Me.MenuItem110.Size = New System.Drawing.Size(315, 22)
        Me.MenuItem110.Text = "بار &ساعات به ساعت فيدرهاي فشار متوسط"
        '
        'mnuMPFeederPartLoadHour
        '
        Me.mnuMPFeederPartLoadHour.Image = CType(resources.GetObject("mnuMPFeederPartLoadHour.Image"), System.Drawing.Image)
        Me.mnuMPFeederPartLoadHour.Name = "mnuMPFeederPartLoadHour"
        Me.mnuMPFeederPartLoadHour.Size = New System.Drawing.Size(315, 22)
        Me.mnuMPFeederPartLoadHour.Text = "بار ساعت به ساعت تکه فیدر های فشارمتوسط"
        '
        'ToolStripSeparator48
        '
        Me.ToolStripSeparator48.Name = "ToolStripSeparator48"
        Me.ToolStripSeparator48.Size = New System.Drawing.Size(312, 6)
        '
        'MenuButtonItem11
        '
        Me.MenuButtonItem11.Name = "MenuButtonItem11"
        Me.MenuButtonItem11.Size = New System.Drawing.Size(315, 22)
        Me.MenuButtonItem11.Text = "فهرست مانورهاي شبکه فشار متوسط"
        '
        'ToolStripSeparator49
        '
        Me.ToolStripSeparator49.Name = "ToolStripSeparator49"
        Me.ToolStripSeparator49.Size = New System.Drawing.Size(312, 6)
        Me.ToolStripSeparator49.Visible = False
        '
        'MenuButtonItem12
        '
        Me.MenuButtonItem12.Name = "MenuButtonItem12"
        Me.MenuButtonItem12.Size = New System.Drawing.Size(315, 22)
        Me.MenuButtonItem12.Text = "قطعيهاي فوق توزيع و بالاتر"
        Me.MenuButtonItem12.Visible = False
        '
        'ToolStripSeparator50
        '
        Me.ToolStripSeparator50.Name = "ToolStripSeparator50"
        Me.ToolStripSeparator50.Size = New System.Drawing.Size(312, 6)
        '
        'MenuButtonItem98
        '
        Me.MenuButtonItem98.Name = "MenuButtonItem98"
        Me.MenuButtonItem98.Size = New System.Drawing.Size(315, 22)
        Me.MenuButtonItem98.Text = "ثبت کارکرد ریکلوزرها"
        '
        'MenuItem97
        '
        Me.MenuItem97.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuMPPostFeederPeakMonthly, Me.mnuMPPostFeederPeakDaily, Me.ToolStripSeparator64, Me.mnuMainPeak, Me.mnuAreaPeak, Me.ToolStripSeparator46, Me.mnuMPHourly, Me.ToolStripSeparator47, Me.mnuMPPostMonthly, Me.mnuMPFeederMonthly, Me.ToolStripSeparator63, Me.mnuGlobalNetworkPeakHour})
        Me.MenuItem97.Name = "MenuItem97"
        Me.MenuItem97.Size = New System.Drawing.Size(315, 22)
        Me.MenuItem97.Text = "پيک بار فيدرهاي فشار متوسط و ترانسهاي فوق توزيع"
        '
        'mnuMPPostFeederPeakMonthly
        '
        Me.mnuMPPostFeederPeakMonthly.Name = "mnuMPPostFeederPeakMonthly"
        Me.mnuMPPostFeederPeakMonthly.Size = New System.Drawing.Size(278, 22)
        Me.mnuMPPostFeederPeakMonthly.Text = "پيک بار ماهانه"
        '
        'mnuMPPostFeederPeakDaily
        '
        Me.mnuMPPostFeederPeakDaily.Name = "mnuMPPostFeederPeakDaily"
        Me.mnuMPPostFeederPeakDaily.Size = New System.Drawing.Size(278, 22)
        Me.mnuMPPostFeederPeakDaily.Text = "پيک بار روزانه"
        '
        'ToolStripSeparator64
        '
        Me.ToolStripSeparator64.Name = "ToolStripSeparator64"
        Me.ToolStripSeparator64.Size = New System.Drawing.Size(275, 6)
        '
        'mnuMainPeak
        '
        Me.mnuMainPeak.Name = "mnuMainPeak"
        Me.mnuMainPeak.Size = New System.Drawing.Size(278, 22)
        Me.mnuMainPeak.Text = "پيک بار توزيع مرکز"
        '
        'mnuAreaPeak
        '
        Me.mnuAreaPeak.Name = "mnuAreaPeak"
        Me.mnuAreaPeak.Size = New System.Drawing.Size(278, 22)
        Me.mnuAreaPeak.Text = "پیک بار نواحی"
        '
        'ToolStripSeparator46
        '
        Me.ToolStripSeparator46.Name = "ToolStripSeparator46"
        Me.ToolStripSeparator46.Size = New System.Drawing.Size(275, 6)
        '
        'mnuMPHourly
        '
        Me.mnuMPHourly.Name = "mnuMPHourly"
        Me.mnuMPHourly.Size = New System.Drawing.Size(278, 22)
        Me.mnuMPHourly.Text = "بار گيري فيدرها و ترانسها در ساعت خاص روز"
        '
        'ToolStripSeparator47
        '
        Me.ToolStripSeparator47.Name = "ToolStripSeparator47"
        Me.ToolStripSeparator47.Size = New System.Drawing.Size(275, 6)
        '
        'mnuMPPostMonthly
        '
        Me.mnuMPPostMonthly.Name = "mnuMPPostMonthly"
        Me.mnuMPPostMonthly.Size = New System.Drawing.Size(278, 22)
        Me.mnuMPPostMonthly.Text = "انرژي تحويلي ماهانه پست‌هاي فوق توزيع"
        '
        'mnuMPFeederMonthly
        '
        Me.mnuMPFeederMonthly.Name = "mnuMPFeederMonthly"
        Me.mnuMPFeederMonthly.Size = New System.Drawing.Size(278, 22)
        Me.mnuMPFeederMonthly.Text = "انرژي تحويلي ماهانه فيدرهاي فشار متوسط"
        '
        'ToolStripSeparator63
        '
        Me.ToolStripSeparator63.Name = "ToolStripSeparator63"
        Me.ToolStripSeparator63.Size = New System.Drawing.Size(275, 6)
        '
        'mnuGlobalNetworkPeakHour
        '
        Me.mnuGlobalNetworkPeakHour.Name = "mnuGlobalNetworkPeakHour"
        Me.mnuGlobalNetworkPeakHour.Size = New System.Drawing.Size(278, 22)
        Me.mnuGlobalNetworkPeakHour.Text = "جدول ساعت پيک روزانه شبکه سراسری"
        '
        'mnuInform
        '
        Me.mnuInform.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSMSSettings, Me.ToolStripSeparator77, Me.mnuInformMonitoring, Me.mnuSubecribersSourceSetting, Me.ToolStripSeparator45, Me.mnuLightRequestSmsSetting, Me.mnuSubscriberSmsSetting, Me.ToolStripSeparator62, Me.mnuManagerSMSST, Me.mnuManagerSMSDC, Me.ToolStripSeparator3, Me.mnuReportSMS, Me.ToolStripSeparator2, Me.mnuSmsContactsInfos, Me.mnuPeopleVoice, Me.ToolStripSeparator1, Me.mnuSendMobileAppNotification, Me.ToolStripSeparator61, Me.mnuChangeSMSBody, Me.ToolStripSeparator545, Me.mnuCCIntelligent})
        Me.mnuInform.Name = "mnuInform"
        Me.mnuInform.Size = New System.Drawing.Size(79, 20)
        Me.mnuInform.Text = "اطلاع رساني"
        '
        'mnuSMSSettings
        '
        Me.mnuSMSSettings.Name = "mnuSMSSettings"
        Me.mnuSMSSettings.Size = New System.Drawing.Size(319, 22)
        Me.mnuSMSSettings.Text = "تنظيمات کلي اطلاع رساني"
        '
        'ToolStripSeparator77
        '
        Me.ToolStripSeparator77.Name = "ToolStripSeparator77"
        Me.ToolStripSeparator77.Size = New System.Drawing.Size(316, 6)
        '
        'mnuInformMonitoring
        '
        Me.mnuInformMonitoring.Name = "mnuInformMonitoring"
        Me.mnuInformMonitoring.Size = New System.Drawing.Size(319, 22)
        Me.mnuInformMonitoring.Text = "مانيتورينگ اطلاع رساني"
        '
        'mnuSubecribersSourceSetting
        '
        Me.mnuSubecribersSourceSetting.Name = "mnuSubecribersSourceSetting"
        Me.mnuSubecribersSourceSetting.Size = New System.Drawing.Size(319, 22)
        Me.mnuSubecribersSourceSetting.Text = "تنظيمات دريافت فهرست مشترکين جهت ارسال پيامک"
        '
        'ToolStripSeparator45
        '
        Me.ToolStripSeparator45.Name = "ToolStripSeparator45"
        Me.ToolStripSeparator45.Size = New System.Drawing.Size(316, 6)
        '
        'mnuLightRequestSmsSetting
        '
        Me.mnuLightRequestSmsSetting.Name = "mnuLightRequestSmsSetting"
        Me.mnuLightRequestSmsSetting.Size = New System.Drawing.Size(319, 22)
        Me.mnuLightRequestSmsSetting.Text = "تنظيمات ارسال پيامک روشنايي معابر به مشترکين"
        '
        'mnuSubscriberSmsSetting
        '
        Me.mnuSubscriberSmsSetting.Name = "mnuSubscriberSmsSetting"
        Me.mnuSubscriberSmsSetting.Size = New System.Drawing.Size(319, 22)
        Me.mnuSubscriberSmsSetting.Text = "تنظيمات ارسال پيامک خاموشي به مشترکين"
        '
        'ToolStripSeparator62
        '
        Me.ToolStripSeparator62.Name = "ToolStripSeparator62"
        Me.ToolStripSeparator62.Size = New System.Drawing.Size(316, 6)
        '
        'mnuManagerSMSST
        '
        Me.mnuManagerSMSST.Name = "mnuManagerSMSST"
        Me.mnuManagerSMSST.Size = New System.Drawing.Size(319, 22)
        Me.mnuManagerSMSST.Text = "ارسال SMS آماري و پيک بار مرکز به مديران"
        '
        'mnuManagerSMSDC
        '
        Me.mnuManagerSMSDC.Name = "mnuManagerSMSDC"
        Me.mnuManagerSMSDC.Size = New System.Drawing.Size(319, 22)
        Me.mnuManagerSMSDC.Text = "ارسال SMS خاموشي‌ها به مديران"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(316, 6)
        '
        'mnuReportSMS
        '
        Me.mnuReportSMS.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuRep_SMS_1, Me.mnuRep_SMS_2})
        Me.mnuReportSMS.Name = "mnuReportSMS"
        Me.mnuReportSMS.Size = New System.Drawing.Size(319, 22)
        Me.mnuReportSMS.Text = "گزارشات اطلاع رسانی"
        '
        'mnuRep_SMS_1
        '
        Me.mnuRep_SMS_1.Name = "mnuRep_SMS_1"
        Me.mnuRep_SMS_1.Size = New System.Drawing.Size(366, 22)
        Me.mnuRep_SMS_1.Text = "گزارش پیام کوتاه های ارسال شده"
        '
        'mnuRep_SMS_2
        '
        Me.mnuRep_SMS_2.Name = "mnuRep_SMS_2"
        Me.mnuRep_SMS_2.Size = New System.Drawing.Size(366, 22)
        Me.mnuRep_SMS_2.Text = "تعداد پرونده خاموشی اطلاع رسانی شده، نسبت به کل پرونده‌ها"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(316, 6)
        '
        'mnuSmsContactsInfos
        '
        Me.mnuSmsContactsInfos.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSendSmsPanel, Me.mnuSmsContactsSemat, Me.mnuSmsContactsGroups, Me.mnuSmsContacts})
        Me.mnuSmsContactsInfos.Name = "mnuSmsContactsInfos"
        Me.mnuSmsContactsInfos.Size = New System.Drawing.Size(319, 22)
        Me.mnuSmsContactsInfos.Text = "پنل ارسال SMS"
        '
        'mnuSendSmsPanel
        '
        Me.mnuSendSmsPanel.Name = "mnuSendSmsPanel"
        Me.mnuSendSmsPanel.Size = New System.Drawing.Size(168, 22)
        Me.mnuSendSmsPanel.Text = "پنل SMS"
        '
        'mnuSmsContactsSemat
        '
        Me.mnuSmsContactsSemat.Name = "mnuSmsContactsSemat"
        Me.mnuSmsContactsSemat.Size = New System.Drawing.Size(168, 22)
        Me.mnuSmsContactsSemat.Text = "سمت هاي مخاطبين"
        '
        'mnuSmsContactsGroups
        '
        Me.mnuSmsContactsGroups.Name = "mnuSmsContactsGroups"
        Me.mnuSmsContactsGroups.Size = New System.Drawing.Size(168, 22)
        Me.mnuSmsContactsGroups.Text = "گروه هاي مخاطبين"
        '
        'mnuSmsContacts
        '
        Me.mnuSmsContacts.Name = "mnuSmsContacts"
        Me.mnuSmsContacts.Size = New System.Drawing.Size(168, 22)
        Me.mnuSmsContacts.Text = "مخاطبين SMS"
        '
        'mnuPeopleVoice
        '
        Me.mnuPeopleVoice.Name = "mnuPeopleVoice"
        Me.mnuPeopleVoice.Size = New System.Drawing.Size(319, 22)
        Me.mnuPeopleVoice.Text = "صداي مردم"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(316, 6)
        '
        'mnuSendMobileAppNotification
        '
        Me.mnuSendMobileAppNotification.Name = "mnuSendMobileAppNotification"
        Me.mnuSendMobileAppNotification.Size = New System.Drawing.Size(319, 22)
        Me.mnuSendMobileAppNotification.Text = "پیام رسانی به نرم افزار موبایل (برق من)"
        '
        'ToolStripSeparator61
        '
        Me.ToolStripSeparator61.Name = "ToolStripSeparator61"
        Me.ToolStripSeparator61.Size = New System.Drawing.Size(316, 6)
        '
        'mnuChangeSMSBody
        '
        Me.mnuChangeSMSBody.Name = "mnuChangeSMSBody"
        Me.mnuChangeSMSBody.Size = New System.Drawing.Size(319, 22)
        Me.mnuChangeSMSBody.Text = "تغییر متن پیامک‌ها"
        '
        'ToolStripSeparator545
        '
        Me.ToolStripSeparator545.Name = "ToolStripSeparator545"
        Me.ToolStripSeparator545.Size = New System.Drawing.Size(316, 6)
        '
        'mnuCCIntelligent
        '
        Me.mnuCCIntelligent.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSurvey, Me.mnuSurveyIVR, Me.mnuGuidSMS, Me.mnuTrackingCodeSettings, Me.mnuSMSErjaDone, Me.mnuSMSAfterEzamEkip, Me.mnuSurvyReport})
        Me.mnuCCIntelligent.Name = "mnuCCIntelligent"
        Me.mnuCCIntelligent.Size = New System.Drawing.Size(319, 22)
        Me.mnuCCIntelligent.Text = "هوشمند سازی مرکز تماس"
        '
        'mnuSurvey
        '
        Me.mnuSurvey.Name = "mnuSurvey"
        Me.mnuSurvey.Size = New System.Drawing.Size(436, 22)
        Me.mnuSurvey.Text = "نظرسنجی پيامکي"
        '
        'mnuSurveyIVR
        '
        Me.mnuSurveyIVR.Name = "mnuSurveyIVR"
        Me.mnuSurveyIVR.Size = New System.Drawing.Size(436, 22)
        Me.mnuSurveyIVR.Text = "نظرسنجی مرکز تماس"
        '
        'mnuGuidSMS
        '
        Me.mnuGuidSMS.Name = "mnuGuidSMS"
        Me.mnuGuidSMS.Size = New System.Drawing.Size(436, 22)
        Me.mnuGuidSMS.Text = "تنظيمات پيامک راهنمايي مشترکين"
        '
        'mnuTrackingCodeSettings
        '
        Me.mnuTrackingCodeSettings.Name = "mnuTrackingCodeSettings"
        Me.mnuTrackingCodeSettings.Size = New System.Drawing.Size(436, 22)
        Me.mnuTrackingCodeSettings.Text = "تنظیمات پیامک اعلام کد رهگیری به مشترکین"
        '
        'mnuSMSErjaDone
        '
        Me.mnuSMSErjaDone.Name = "mnuSMSErjaDone"
        Me.mnuSMSErjaDone.Size = New System.Drawing.Size(436, 22)
        Me.mnuSMSErjaDone.Text = "تنظيمات ارسال پيامک پس از انجام پرونده ارجاع شده"
        '
        'mnuSMSAfterEzamEkip
        '
        Me.mnuSMSAfterEzamEkip.Name = "mnuSMSAfterEzamEkip"
        Me.mnuSMSAfterEzamEkip.Size = New System.Drawing.Size(436, 22)
        Me.mnuSMSAfterEzamEkip.Text = "تنظيمات ارسال پيامک هشدار قطع برق به مشترکين حساس، پس از اعزام اکيپ"
        '
        'mnuSurvyReport
        '
        Me.mnuSurvyReport.Name = "mnuSurvyReport"
        Me.mnuSurvyReport.Size = New System.Drawing.Size(436, 22)
        Me.mnuSurvyReport.Text = "گزارشات نظرسنجي"
        '
        'mnuHoma
        '
        Me.mnuHoma.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuHomaBase, Me.mnuRepotEkipTrace})
        Me.mnuHoma.Name = "mnuHoma"
        Me.mnuHoma.Size = New System.Drawing.Size(59, 20)
        Me.mnuHoma.Text = "طرح هما"
        '
        'mnuHomaBase
        '
        Me.mnuHomaBase.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuHomaTabletUser, Me.mnuHomaTablet, Me.mnuHomaRegisterBilling})
        Me.mnuHomaBase.Name = "mnuHomaBase"
        Me.mnuHomaBase.Size = New System.Drawing.Size(129, 22)
        Me.mnuHomaBase.Text = "اطلاعات پايه"
        '
        'mnuHomaTabletUser
        '
        Me.mnuHomaTabletUser.Name = "mnuHomaTabletUser"
        Me.mnuHomaTabletUser.Size = New System.Drawing.Size(189, 22)
        Me.mnuHomaTabletUser.Text = "تخصيص استاد کار به کاربر"
        '
        'mnuHomaTablet
        '
        Me.mnuHomaTablet.Name = "mnuHomaTablet"
        Me.mnuHomaTablet.Size = New System.Drawing.Size(189, 22)
        Me.mnuHomaTablet.Text = "تبلت ها"
        '
        'mnuHomaRegisterBilling
        '
        Me.mnuHomaRegisterBilling.Name = "mnuHomaRegisterBilling"
        Me.mnuHomaRegisterBilling.Size = New System.Drawing.Size(189, 22)
        Me.mnuHomaRegisterBilling.Text = "مشترکين"
        '
        'mnuRepotEkipTrace
        '
        Me.mnuRepotEkipTrace.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuRep_h_1})
        Me.mnuRepotEkipTrace.Name = "mnuRepotEkipTrace"
        Me.mnuRepotEkipTrace.Size = New System.Drawing.Size(129, 22)
        Me.mnuRepotEkipTrace.Text = "گزارشات"
        '
        'mnuRep_h_1
        '
        Me.mnuRep_h_1.Name = "mnuRep_h_1"
        Me.mnuRep_h_1.Size = New System.Drawing.Size(232, 22)
        Me.mnuRep_h_1.Text = "گزارش عملکرد اکيپهای آنکال (H-1)"
        '
        'mnuReports
        '
        Me.mnuReports.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuReportRequests, Me.MenuItemFogh, Me.mnuRepMPRequest, Me.mnuRepLPRequest, Me.mnuRepLightRequest, Me.ToolStripSeparator5, Me.mnuRepBaseInfo, Me.ToolStripSeparator6, Me.mnuRepDorehei, Me.mnuRepLoading, Me.mnuRepEarting, Me.mnuControlReports, Me.mnuRepOtherReport, Me.mnuRepCompare, Me.ToolStripSeparator7, Me.mnuExcelReports, Me.ToolStripSeparator76, Me.mnuExtractionReports, Me.mnuTavanirReports, Me.mnuRecloserReport, Me.mnuSaveSeparator, Me.mnuSimaPortal, Me.mnuSepMisc, Me.mnuSaveFav})
        Me.mnuReports.Name = "mnuReports"
        Me.mnuReports.Size = New System.Drawing.Size(72, 20)
        Me.mnuReports.Text = "&گزارشگيري"
        '
        'mnuReportRequests
        '
        Me.mnuReportRequests.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuRptRequests, Me.mnuRep_1_13, Me.mnuRep_1_17, Me.mnuRep_1_18, Me.ToolStripSeparator8, Me.mnuRptChangeHistory, Me.mnuRptEzamEkipInfo, Me.mnuRptEzamEkipInfo_Mode2, Me.ToolStripSeparator9, Me.mnuRptReferToInfo, Me.mnuRptReferToCountArea, Me.mnuRptReferToCount, Me.ToolStripSeparator10, Me.mnuRepRequestAll2, Me.mnuRepRequestAll, Me.ToolStripSeparator11, Me.mnuDisconnectPower_Zone, Me.mnuDisconnectCount_Zone, Me.mnuRequestCountDaily, Me.mnuDisconnectCountSubscribers1000, Me.mnuDisconnectPower_Area, Me.mnuDisconnectCount_Area, Me.mnuDisconnectCountSubscribersArea1000, Me.ToolStripSeparator72, Me.mnuReport_1_20})
        Me.mnuReportRequests.Name = "mnuReportRequests"
        Me.mnuReportRequests.Size = New System.Drawing.Size(206, 22)
        Me.mnuReportRequests.Text = "(1) درخواستها"
        '
        'mnuRptRequests
        '
        Me.mnuRptRequests.FavVisible = False
        Me.mnuRptRequests.Image = CType(resources.GetObject("mnuRptRequests.Image"), System.Drawing.Image)
        Me.mnuRptRequests.IsChange = False
        Me.mnuRptRequests.IsFavVisible = True
        Me.mnuRptRequests.Name = "mnuRptRequests"
        Me.mnuRptRequests.Size = New System.Drawing.Size(375, 22)
        Me.mnuRptRequests.Text = "(1-1)  گزارش درخواستها"
        '
        'mnuRep_1_13
        '
        Me.mnuRep_1_13.FavVisible = False
        Me.mnuRep_1_13.Image = CType(resources.GetObject("mnuRep_1_13.Image"), System.Drawing.Image)
        Me.mnuRep_1_13.IsChange = False
        Me.mnuRep_1_13.IsFavVisible = True
        Me.mnuRep_1_13.Name = "mnuRep_1_13"
        Me.mnuRep_1_13.Size = New System.Drawing.Size(375, 22)
        Me.mnuRep_1_13.Text = "(1-13)  گزارش درخواستها (نمونه 1)"
        '
        'mnuRep_1_17
        '
        Me.mnuRep_1_17.FavVisible = False
        Me.mnuRep_1_17.Image = CType(resources.GetObject("mnuRep_1_17.Image"), System.Drawing.Image)
        Me.mnuRep_1_17.IsChange = False
        Me.mnuRep_1_17.IsFavVisible = True
        Me.mnuRep_1_17.Name = "mnuRep_1_17"
        Me.mnuRep_1_17.Size = New System.Drawing.Size(375, 22)
        Me.mnuRep_1_17.Text = "(1-17)  گزارش درخواستها (نمونه 2)"
        '
        'mnuRep_1_18
        '
        Me.mnuRep_1_18.FavVisible = False
        Me.mnuRep_1_18.Image = CType(resources.GetObject("mnuRep_1_18.Image"), System.Drawing.Image)
        Me.mnuRep_1_18.IsChange = False
        Me.mnuRep_1_18.IsFavVisible = True
        Me.mnuRep_1_18.Name = "mnuRep_1_18"
        Me.mnuRep_1_18.Size = New System.Drawing.Size(375, 22)
        Me.mnuRep_1_18.Text = "(1-18)  گزارش درخواستها (نمونه 3)"
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(372, 6)
        '
        'mnuRptChangeHistory
        '
        Me.mnuRptChangeHistory.FavVisible = False
        Me.mnuRptChangeHistory.Image = CType(resources.GetObject("mnuRptChangeHistory.Image"), System.Drawing.Image)
        Me.mnuRptChangeHistory.IsChange = False
        Me.mnuRptChangeHistory.IsFavVisible = True
        Me.mnuRptChangeHistory.Name = "mnuRptChangeHistory"
        Me.mnuRptChangeHistory.Size = New System.Drawing.Size(375, 22)
        Me.mnuRptChangeHistory.Text = "(1-2) گزارش ويرايش پرونده‌هاي تکميل شده"
        '
        'mnuRptEzamEkipInfo
        '
        Me.mnuRptEzamEkipInfo.FavVisible = False
        Me.mnuRptEzamEkipInfo.Image = CType(resources.GetObject("mnuRptEzamEkipInfo.Image"), System.Drawing.Image)
        Me.mnuRptEzamEkipInfo.IsChange = False
        Me.mnuRptEzamEkipInfo.IsFavVisible = True
        Me.mnuRptEzamEkipInfo.Name = "mnuRptEzamEkipInfo"
        Me.mnuRptEzamEkipInfo.Size = New System.Drawing.Size(375, 22)
        Me.mnuRptEzamEkipInfo.Text = "(1-3) گزارش دستور کار انجام شده"
        '
        'mnuRptEzamEkipInfo_Mode2
        '
        Me.mnuRptEzamEkipInfo_Mode2.FavVisible = False
        Me.mnuRptEzamEkipInfo_Mode2.Image = CType(resources.GetObject("mnuRptEzamEkipInfo_Mode2.Image"), System.Drawing.Image)
        Me.mnuRptEzamEkipInfo_Mode2.IsChange = False
        Me.mnuRptEzamEkipInfo_Mode2.IsFavVisible = True
        Me.mnuRptEzamEkipInfo_Mode2.Name = "mnuRptEzamEkipInfo_Mode2"
        Me.mnuRptEzamEkipInfo_Mode2.Size = New System.Drawing.Size(375, 22)
        Me.mnuRptEzamEkipInfo_Mode2.Text = "(1-19) گزارش دستور کار انجام شده (نمونه 2)"
        '
        'ToolStripSeparator9
        '
        Me.ToolStripSeparator9.Name = "ToolStripSeparator9"
        Me.ToolStripSeparator9.Size = New System.Drawing.Size(372, 6)
        '
        'mnuRptReferToInfo
        '
        Me.mnuRptReferToInfo.FavVisible = False
        Me.mnuRptReferToInfo.Image = CType(resources.GetObject("mnuRptReferToInfo.Image"), System.Drawing.Image)
        Me.mnuRptReferToInfo.IsChange = False
        Me.mnuRptReferToInfo.IsFavVisible = True
        Me.mnuRptReferToInfo.Name = "mnuRptReferToInfo"
        Me.mnuRptReferToInfo.Size = New System.Drawing.Size(375, 22)
        Me.mnuRptReferToInfo.Text = "(1-4) گزارش روزانه ارجاع‌ها"
        '
        'mnuRptReferToCountArea
        '
        Me.mnuRptReferToCountArea.FavVisible = False
        Me.mnuRptReferToCountArea.Image = CType(resources.GetObject("mnuRptReferToCountArea.Image"), System.Drawing.Image)
        Me.mnuRptReferToCountArea.IsChange = False
        Me.mnuRptReferToCountArea.IsFavVisible = True
        Me.mnuRptReferToCountArea.Name = "mnuRptReferToCountArea"
        Me.mnuRptReferToCountArea.Size = New System.Drawing.Size(375, 22)
        Me.mnuRptReferToCountArea.Text = "(1-5) گزارش روزانه ارجاع‌ها به تفکيک ناحيه - واحد اقدام کننده"
        '
        'mnuRptReferToCount
        '
        Me.mnuRptReferToCount.FavVisible = False
        Me.mnuRptReferToCount.Image = CType(resources.GetObject("mnuRptReferToCount.Image"), System.Drawing.Image)
        Me.mnuRptReferToCount.IsChange = False
        Me.mnuRptReferToCount.IsFavVisible = True
        Me.mnuRptReferToCount.Name = "mnuRptReferToCount"
        Me.mnuRptReferToCount.Size = New System.Drawing.Size(375, 22)
        Me.mnuRptReferToCount.Text = "(1-6) گزارش روزانه ارجاع‌ها به تفکيک واحد اقدام کننده - ناحيه"
        '
        'ToolStripSeparator10
        '
        Me.ToolStripSeparator10.Name = "ToolStripSeparator10"
        Me.ToolStripSeparator10.Size = New System.Drawing.Size(372, 6)
        '
        'mnuRepRequestAll2
        '
        Me.mnuRepRequestAll2.FavVisible = False
        Me.mnuRepRequestAll2.Image = CType(resources.GetObject("mnuRepRequestAll2.Image"), System.Drawing.Image)
        Me.mnuRepRequestAll2.IsChange = False
        Me.mnuRepRequestAll2.IsFavVisible = True
        Me.mnuRepRequestAll2.Name = "mnuRepRequestAll2"
        Me.mnuRepRequestAll2.Size = New System.Drawing.Size(375, 22)
        Me.mnuRepRequestAll2.Text = "(1-12) گزارش قطع و وصل فيدرها (نمونه 2)"
        '
        'mnuRepRequestAll
        '
        Me.mnuRepRequestAll.FavVisible = False
        Me.mnuRepRequestAll.Image = CType(resources.GetObject("mnuRepRequestAll.Image"), System.Drawing.Image)
        Me.mnuRepRequestAll.IsChange = False
        Me.mnuRepRequestAll.IsFavVisible = True
        Me.mnuRepRequestAll.Name = "mnuRepRequestAll"
        Me.mnuRepRequestAll.Size = New System.Drawing.Size(375, 22)
        Me.mnuRepRequestAll.Text = "(1-11) گزارش قطع و وصل فيدرها (نمونه 1)"
        '
        'ToolStripSeparator11
        '
        Me.ToolStripSeparator11.Name = "ToolStripSeparator11"
        Me.ToolStripSeparator11.Size = New System.Drawing.Size(372, 6)
        '
        'mnuDisconnectPower_Zone
        '
        Me.mnuDisconnectPower_Zone.FavVisible = False
        Me.mnuDisconnectPower_Zone.Image = CType(resources.GetObject("mnuDisconnectPower_Zone.Image"), System.Drawing.Image)
        Me.mnuDisconnectPower_Zone.IsChange = False
        Me.mnuDisconnectPower_Zone.IsFavVisible = True
        Me.mnuDisconnectPower_Zone.Name = "mnuDisconnectPower_Zone"
        Me.mnuDisconnectPower_Zone.Size = New System.Drawing.Size(375, 22)
        Me.mnuDisconnectPower_Zone.Text = "(1-7) نمودار انرژي توزيع نشده بر اساس منطقه"
        '
        'mnuDisconnectCount_Zone
        '
        Me.mnuDisconnectCount_Zone.FavVisible = False
        Me.mnuDisconnectCount_Zone.Image = CType(resources.GetObject("mnuDisconnectCount_Zone.Image"), System.Drawing.Image)
        Me.mnuDisconnectCount_Zone.IsChange = False
        Me.mnuDisconnectCount_Zone.IsFavVisible = True
        Me.mnuDisconnectCount_Zone.Name = "mnuDisconnectCount_Zone"
        Me.mnuDisconnectCount_Zone.Size = New System.Drawing.Size(375, 22)
        Me.mnuDisconnectCount_Zone.Text = "(1-8) نمودار تعداد خاموشي بر اساس منطقه"
        '
        'mnuRequestCountDaily
        '
        Me.mnuRequestCountDaily.FavVisible = False
        Me.mnuRequestCountDaily.Image = CType(resources.GetObject("mnuRequestCountDaily.Image"), System.Drawing.Image)
        Me.mnuRequestCountDaily.IsChange = False
        Me.mnuRequestCountDaily.IsFavVisible = True
        Me.mnuRequestCountDaily.Name = "mnuRequestCountDaily"
        Me.mnuRequestCountDaily.Size = New System.Drawing.Size(375, 22)
        Me.mnuRequestCountDaily.Text = "(1-9) نمودار تعداد تماسهاي گرفته شده به تفکيک تاريخ تماس"
        '
        'mnuDisconnectCountSubscribers1000
        '
        Me.mnuDisconnectCountSubscribers1000.FavVisible = False
        Me.mnuDisconnectCountSubscribers1000.Image = CType(resources.GetObject("mnuDisconnectCountSubscribers1000.Image"), System.Drawing.Image)
        Me.mnuDisconnectCountSubscribers1000.IsChange = False
        Me.mnuDisconnectCountSubscribers1000.IsFavVisible = True
        Me.mnuDisconnectCountSubscribers1000.Name = "mnuDisconnectCountSubscribers1000"
        Me.mnuDisconnectCountSubscribers1000.Size = New System.Drawing.Size(375, 22)
        Me.mnuDisconnectCountSubscribers1000.Text = "(1-10) نمودار تعداد خاموشي به تفکيک منطقه براي 1000 مشترک"
        '
        'mnuDisconnectPower_Area
        '
        Me.mnuDisconnectPower_Area.FavVisible = False
        Me.mnuDisconnectPower_Area.Image = CType(resources.GetObject("mnuDisconnectPower_Area.Image"), System.Drawing.Image)
        Me.mnuDisconnectPower_Area.IsChange = False
        Me.mnuDisconnectPower_Area.IsFavVisible = True
        Me.mnuDisconnectPower_Area.Name = "mnuDisconnectPower_Area"
        Me.mnuDisconnectPower_Area.Size = New System.Drawing.Size(375, 22)
        Me.mnuDisconnectPower_Area.Text = "(1-14) نمودار انرژي توزيع نشده بر اساس ناحيه"
        '
        'mnuDisconnectCount_Area
        '
        Me.mnuDisconnectCount_Area.FavVisible = False
        Me.mnuDisconnectCount_Area.Image = CType(resources.GetObject("mnuDisconnectCount_Area.Image"), System.Drawing.Image)
        Me.mnuDisconnectCount_Area.IsChange = False
        Me.mnuDisconnectCount_Area.IsFavVisible = True
        Me.mnuDisconnectCount_Area.Name = "mnuDisconnectCount_Area"
        Me.mnuDisconnectCount_Area.Size = New System.Drawing.Size(375, 22)
        Me.mnuDisconnectCount_Area.Text = "(1-15) نمودار تعداد خاموشي بر اساس ناحيه"
        '
        'mnuDisconnectCountSubscribersArea1000
        '
        Me.mnuDisconnectCountSubscribersArea1000.FavVisible = False
        Me.mnuDisconnectCountSubscribersArea1000.Image = CType(resources.GetObject("mnuDisconnectCountSubscribersArea1000.Image"), System.Drawing.Image)
        Me.mnuDisconnectCountSubscribersArea1000.IsChange = False
        Me.mnuDisconnectCountSubscribersArea1000.IsFavVisible = True
        Me.mnuDisconnectCountSubscribersArea1000.Name = "mnuDisconnectCountSubscribersArea1000"
        Me.mnuDisconnectCountSubscribersArea1000.Size = New System.Drawing.Size(375, 22)
        Me.mnuDisconnectCountSubscribersArea1000.Text = "(1-16) نمودار تعداد خاموشي به تفکيک ناحيه براي 1000 مشترک"
        '
        'ToolStripSeparator72
        '
        Me.ToolStripSeparator72.Name = "ToolStripSeparator72"
        Me.ToolStripSeparator72.Size = New System.Drawing.Size(372, 6)
        '
        'mnuReport_1_20
        '
        Me.mnuReport_1_20.FavVisible = False
        Me.mnuReport_1_20.Image = CType(resources.GetObject("mnuReport_1_20.Image"), System.Drawing.Image)
        Me.mnuReport_1_20.IsChange = False
        Me.mnuReport_1_20.IsFavVisible = True
        Me.mnuReport_1_20.Name = "mnuReport_1_20"
        Me.mnuReport_1_20.Size = New System.Drawing.Size(375, 22)
        Me.mnuReport_1_20.Text = "(1-20) فهرست تلفنهای ثابت توسط تماس گیرندگان با تلفن همراه"
        Me.mnuReport_1_20.Visible = False
        '
        'MenuItemFogh
        '
        Me.MenuItemFogh.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuRptDisFogh, Me.mnuRpt_2_3, Me.ToolStripSeparator74, Me.mnuRptManourveFogh})
        Me.MenuItemFogh.Name = "MenuItemFogh"
        Me.MenuItemFogh.Size = New System.Drawing.Size(206, 22)
        Me.MenuItemFogh.Text = "(2) فوق توزيع"
        '
        'mnuRptDisFogh
        '
        Me.mnuRptDisFogh.FavVisible = False
        Me.mnuRptDisFogh.Image = CType(resources.GetObject("mnuRptDisFogh.Image"), System.Drawing.Image)
        Me.mnuRptDisFogh.IsChange = False
        Me.mnuRptDisFogh.IsFavVisible = True
        Me.mnuRptDisFogh.Name = "mnuRptDisFogh"
        Me.mnuRptDisFogh.Size = New System.Drawing.Size(339, 22)
        Me.mnuRptDisFogh.Tag = "mnuRptDisFogh"
        Me.mnuRptDisFogh.Text = "(2-1) گزارش حوادث و قطعی های روزانه فوق توزیع"
        '
        'mnuRpt_2_3
        '
        Me.mnuRpt_2_3.FavVisible = False
        Me.mnuRpt_2_3.Image = CType(resources.GetObject("mnuRpt_2_3.Image"), System.Drawing.Image)
        Me.mnuRpt_2_3.IsChange = False
        Me.mnuRpt_2_3.IsFavVisible = True
        Me.mnuRpt_2_3.Name = "mnuRpt_2_3"
        Me.mnuRpt_2_3.Size = New System.Drawing.Size(339, 22)
        Me.mnuRpt_2_3.Tag = "mnuRpt_2_3"
        Me.mnuRpt_2_3.Text = "(2-3) گزارش حوادث و قطعی های روزانه فوق توزیع نمونه 2"
        '
        'ToolStripSeparator74
        '
        Me.ToolStripSeparator74.Name = "ToolStripSeparator74"
        Me.ToolStripSeparator74.Size = New System.Drawing.Size(336, 6)
        '
        'mnuRptManourveFogh
        '
        Me.mnuRptManourveFogh.FavVisible = False
        Me.mnuRptManourveFogh.Image = CType(resources.GetObject("mnuRptManourveFogh.Image"), System.Drawing.Image)
        Me.mnuRptManourveFogh.IsChange = False
        Me.mnuRptManourveFogh.IsFavVisible = True
        Me.mnuRptManourveFogh.Name = "mnuRptManourveFogh"
        Me.mnuRptManourveFogh.Size = New System.Drawing.Size(339, 22)
        Me.mnuRptManourveFogh.Text = "(2-2) گزارش بار خطوط در مانورهای فوق توزیع"
        '
        'mnuRepMPRequest
        '
        Me.mnuRepMPRequest.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuItem79, Me.mnu_Num_3_27, Me.mnu_Num_3_34, Me.mnuRepMPDisconnectLPPost, Me.MenuItem78, Me.MenuItem77, Me.MenuButtonItem41, Me.mnuRepMP_Mode4, Me.mnuRepMP_Mode5, Me.MenuButtonItem63, Me.mnuRepMP_Mode7, Me.mnuRep_3_37, Me.mnuRep_3_38, Me.ToolStripSeparator14, Me.MenuItem76, Me.MenuItem75, Me.MenuButtonItem27, Me.MenuButtonItem29, Me.mnuMulitiStepConnection, Me.mnuRptManoeurve, Me.ToolStripSeparator13, Me.mnuMinCountMP, Me.mnuMinCountMPArea, Me.mnuMinCountFeederPart, Me.mnuLPPostFeederPartCount, Me.ToolStripSeparator12, Me.MenuItem74, Me.MenuButtonItem39, Me.MenuButtonItem40, Me.MenuItem73, Me.MenuItem152, Me.MenuItem157, Me.MenuItem156, Me.MenuItem155, Me.MenuItem149, Me.MenuItem153, Me.MenuItem150, Me.MenuItem154, Me.MenuItem151, Me.mnuRpt_3_25, Me.mnuRpt_3_26})
        Me.mnuRepMPRequest.Name = "mnuRepMPRequest"
        Me.mnuRepMPRequest.Size = New System.Drawing.Size(206, 22)
        Me.mnuRepMPRequest.Text = "(3) فشار &متوسط"
        '
        'MenuItem79
        '
        Me.MenuItem79.FavVisible = False
        Me.MenuItem79.Image = CType(resources.GetObject("MenuItem79.Image"), System.Drawing.Image)
        Me.MenuItem79.IsChange = False
        Me.MenuItem79.IsFavVisible = True
        Me.MenuItem79.Name = "MenuItem79"
        Me.MenuItem79.Size = New System.Drawing.Size(518, 22)
        Me.MenuItem79.Text = "(3-1) گزارش روزانه حوادث و قطعي هاي فيدرهاي فشار متوسط"
        '
        'mnu_Num_3_27
        '
        Me.mnu_Num_3_27.FavVisible = False
        Me.mnu_Num_3_27.Image = CType(resources.GetObject("mnu_Num_3_27.Image"), System.Drawing.Image)
        Me.mnu_Num_3_27.IsChange = False
        Me.mnu_Num_3_27.IsFavVisible = True
        Me.mnu_Num_3_27.Name = "mnu_Num_3_27"
        Me.mnu_Num_3_27.Size = New System.Drawing.Size(518, 22)
        Me.mnu_Num_3_27.Text = "(3-27) گزارش روزانه حوادث و قطعي هاي فيدرهاي فشار متوسط  (نمونه 2)"
        '
        'mnu_Num_3_34
        '
        Me.mnu_Num_3_34.FavVisible = False
        Me.mnu_Num_3_34.Image = CType(resources.GetObject("mnu_Num_3_34.Image"), System.Drawing.Image)
        Me.mnu_Num_3_34.IsChange = False
        Me.mnu_Num_3_34.IsFavVisible = True
        Me.mnu_Num_3_34.Name = "mnu_Num_3_34"
        Me.mnu_Num_3_34.Size = New System.Drawing.Size(518, 22)
        Me.mnu_Num_3_34.Text = "(3-34) گزارش روزانه حوادث و قطعي هاي فيدرهاي فشار متوسط  (نمونه 3)"
        '
        'mnuRepMPDisconnectLPPost
        '
        Me.mnuRepMPDisconnectLPPost.FavVisible = False
        Me.mnuRepMPDisconnectLPPost.Image = CType(resources.GetObject("mnuRepMPDisconnectLPPost.Image"), System.Drawing.Image)
        Me.mnuRepMPDisconnectLPPost.IsChange = False
        Me.mnuRepMPDisconnectLPPost.IsFavVisible = True
        Me.mnuRepMPDisconnectLPPost.Name = "mnuRepMPDisconnectLPPost"
        Me.mnuRepMPDisconnectLPPost.Size = New System.Drawing.Size(518, 22)
        Me.mnuRepMPDisconnectLPPost.Text = "(3-2) گزارش حوادث و قطعي هاي پست‌هاي توزيع"
        '
        'MenuItem78
        '
        Me.MenuItem78.FavVisible = False
        Me.MenuItem78.Image = CType(resources.GetObject("MenuItem78.Image"), System.Drawing.Image)
        Me.MenuItem78.IsChange = False
        Me.MenuItem78.IsFavVisible = True
        Me.MenuItem78.Name = "MenuItem78"
        Me.MenuItem78.Size = New System.Drawing.Size(518, 22)
        Me.MenuItem78.Text = "(3-3) گزارش قطع و وصل فيدرهاي فشار متوسط (نمونه 1)"
        '
        'MenuItem77
        '
        Me.MenuItem77.FavVisible = False
        Me.MenuItem77.Image = CType(resources.GetObject("MenuItem77.Image"), System.Drawing.Image)
        Me.MenuItem77.IsChange = False
        Me.MenuItem77.IsFavVisible = True
        Me.MenuItem77.Name = "MenuItem77"
        Me.MenuItem77.Size = New System.Drawing.Size(518, 22)
        Me.MenuItem77.Text = "(3-4) گزارش قطع و وصل فيدرهاي فشار متوسط (نمونه 2)"
        '
        'MenuButtonItem41
        '
        Me.MenuButtonItem41.FavVisible = False
        Me.MenuButtonItem41.Image = CType(resources.GetObject("MenuButtonItem41.Image"), System.Drawing.Image)
        Me.MenuButtonItem41.IsChange = False
        Me.MenuButtonItem41.IsFavVisible = True
        Me.MenuButtonItem41.Name = "MenuButtonItem41"
        Me.MenuButtonItem41.Size = New System.Drawing.Size(518, 22)
        Me.MenuButtonItem41.Text = "(3-5) گزارش قطع و وصل فيدرهاي فشار متوسط (نمونه 3)"
        '
        'mnuRepMP_Mode4
        '
        Me.mnuRepMP_Mode4.FavVisible = False
        Me.mnuRepMP_Mode4.Image = CType(resources.GetObject("mnuRepMP_Mode4.Image"), System.Drawing.Image)
        Me.mnuRepMP_Mode4.IsChange = False
        Me.mnuRepMP_Mode4.IsFavVisible = True
        Me.mnuRepMP_Mode4.Name = "mnuRepMP_Mode4"
        Me.mnuRepMP_Mode4.Size = New System.Drawing.Size(518, 22)
        Me.mnuRepMP_Mode4.Text = "(3-24) گزارش قطع و وصل فيدرهاي فشار متوسط (نمونه 4)"
        '
        'mnuRepMP_Mode5
        '
        Me.mnuRepMP_Mode5.FavVisible = False
        Me.mnuRepMP_Mode5.Image = CType(resources.GetObject("mnuRepMP_Mode5.Image"), System.Drawing.Image)
        Me.mnuRepMP_Mode5.IsChange = False
        Me.mnuRepMP_Mode5.IsFavVisible = True
        Me.mnuRepMP_Mode5.Name = "mnuRepMP_Mode5"
        Me.mnuRepMP_Mode5.Size = New System.Drawing.Size(518, 22)
        Me.mnuRepMP_Mode5.Text = "(3-29) گزارش قطع و وصل فيدرهاي فشار متوسط (نمونه 5)"
        '
        'MenuButtonItem63
        '
        Me.MenuButtonItem63.FavVisible = False
        Me.MenuButtonItem63.Image = CType(resources.GetObject("MenuButtonItem63.Image"), System.Drawing.Image)
        Me.MenuButtonItem63.IsChange = False
        Me.MenuButtonItem63.IsFavVisible = True
        Me.MenuButtonItem63.Name = "MenuButtonItem63"
        Me.MenuButtonItem63.Size = New System.Drawing.Size(518, 22)
        Me.MenuButtonItem63.Text = "(3-33) گزارش قطع و وصل فيدرهاي فشار متوسط (نمونه 6)"
        '
        'mnuRepMP_Mode7
        '
        Me.mnuRepMP_Mode7.FavVisible = False
        Me.mnuRepMP_Mode7.Image = CType(resources.GetObject("mnuRepMP_Mode7.Image"), System.Drawing.Image)
        Me.mnuRepMP_Mode7.IsChange = False
        Me.mnuRepMP_Mode7.IsFavVisible = True
        Me.mnuRepMP_Mode7.Name = "mnuRepMP_Mode7"
        Me.mnuRepMP_Mode7.Size = New System.Drawing.Size(518, 22)
        Me.mnuRepMP_Mode7.Text = "(3-36) گزارش قطع و وصل فيدرهاي فشار متوسط (نمونه 7)"
        '
        'mnuRep_3_37
        '
        Me.mnuRep_3_37.FavVisible = False
        Me.mnuRep_3_37.Image = CType(resources.GetObject("mnuRep_3_37.Image"), System.Drawing.Image)
        Me.mnuRep_3_37.IsChange = False
        Me.mnuRep_3_37.IsFavVisible = True
        Me.mnuRep_3_37.Name = "mnuRep_3_37"
        Me.mnuRep_3_37.Size = New System.Drawing.Size(518, 22)
        Me.mnuRep_3_37.Text = "(3-37) گزارش قطع و وصل کليدهاي فشار متوسط"
        '
        'mnuRep_3_38
        '
        Me.mnuRep_3_38.FavVisible = False
        Me.mnuRep_3_38.Image = Global.Havades_App.Resources.favNo
        Me.mnuRep_3_38.IsChange = False
        Me.mnuRep_3_38.IsFavVisible = True
        Me.mnuRep_3_38.Name = "mnuRep_3_38"
        Me.mnuRep_3_38.Size = New System.Drawing.Size(518, 22)
        Me.mnuRep_3_38.Text = "(3-38) گزارش قطع و وصل فيدرهاي فشار متوسط به همراه بار پيش از قطع"
        '
        'ToolStripSeparator14
        '
        Me.ToolStripSeparator14.Name = "ToolStripSeparator14"
        Me.ToolStripSeparator14.Size = New System.Drawing.Size(515, 6)
        '
        'MenuItem76
        '
        Me.MenuItem76.FavVisible = False
        Me.MenuItem76.Image = CType(resources.GetObject("MenuItem76.Image"), System.Drawing.Image)
        Me.MenuItem76.IsChange = False
        Me.MenuItem76.IsFavVisible = True
        Me.MenuItem76.Name = "MenuItem76"
        Me.MenuItem76.Size = New System.Drawing.Size(518, 22)
        Me.MenuItem76.Text = "(3-6) گزارش تجهيزات مصرف شده در شبكه فشار متوسط به تفکيک پرونده"
        '
        'MenuItem75
        '
        Me.MenuItem75.FavVisible = False
        Me.MenuItem75.Image = CType(resources.GetObject("MenuItem75.Image"), System.Drawing.Image)
        Me.MenuItem75.IsChange = False
        Me.MenuItem75.IsFavVisible = True
        Me.MenuItem75.Name = "MenuItem75"
        Me.MenuItem75.Size = New System.Drawing.Size(518, 22)
        Me.MenuItem75.Text = "(3-7) گزارش آماري تجهيزات مصرف شده در شبكه فشار متوسط - فقط قطعات مصرف شده"
        '
        'MenuButtonItem27
        '
        Me.MenuButtonItem27.FavVisible = False
        Me.MenuButtonItem27.Image = CType(resources.GetObject("MenuButtonItem27.Image"), System.Drawing.Image)
        Me.MenuButtonItem27.IsChange = False
        Me.MenuButtonItem27.IsFavVisible = True
        Me.MenuButtonItem27.Name = "MenuButtonItem27"
        Me.MenuButtonItem27.Size = New System.Drawing.Size(518, 22)
        Me.MenuButtonItem27.Text = "(3-28) گزارش آماري تجهيزات مصرف شده در شبكه فشار متوسط به تفکيک ناحيه"
        '
        'MenuButtonItem29
        '
        Me.MenuButtonItem29.FavVisible = False
        Me.MenuButtonItem29.Image = CType(resources.GetObject("MenuButtonItem29.Image"), System.Drawing.Image)
        Me.MenuButtonItem29.IsChange = False
        Me.MenuButtonItem29.IsFavVisible = True
        Me.MenuButtonItem29.Name = "MenuButtonItem29"
        Me.MenuButtonItem29.Size = New System.Drawing.Size(518, 22)
        Me.MenuButtonItem29.Text = "(3-32) گزارش آماري تجهيزات مصرف شده در شبكه فشار متوسط بدون تفکيک نواحي"
        '
        'mnuMulitiStepConnection
        '
        Me.mnuMulitiStepConnection.FavVisible = False
        Me.mnuMulitiStepConnection.Image = CType(resources.GetObject("mnuMulitiStepConnection.Image"), System.Drawing.Image)
        Me.mnuMulitiStepConnection.IsChange = False
        Me.mnuMulitiStepConnection.IsFavVisible = True
        Me.mnuMulitiStepConnection.Name = "mnuMulitiStepConnection"
        Me.mnuMulitiStepConnection.Size = New System.Drawing.Size(518, 22)
        Me.mnuMulitiStepConnection.Text = "(3-8) گزارش مراحل، مانور و وصل چند مرحله‌اي"
        '
        'mnuRptManoeurve
        '
        Me.mnuRptManoeurve.FavVisible = False
        Me.mnuRptManoeurve.Image = CType(resources.GetObject("mnuRptManoeurve.Image"), System.Drawing.Image)
        Me.mnuRptManoeurve.IsChange = False
        Me.mnuRptManoeurve.IsFavVisible = True
        Me.mnuRptManoeurve.Name = "mnuRptManoeurve"
        Me.mnuRptManoeurve.Size = New System.Drawing.Size(518, 22)
        Me.mnuRptManoeurve.Text = "(3-35) گزارش بار خطوط در مانور فشار متوسط"
        '
        'ToolStripSeparator13
        '
        Me.ToolStripSeparator13.Name = "ToolStripSeparator13"
        Me.ToolStripSeparator13.Size = New System.Drawing.Size(515, 6)
        '
        'mnuMinCountMP
        '
        Me.mnuMinCountMP.FavVisible = False
        Me.mnuMinCountMP.Image = CType(resources.GetObject("mnuMinCountMP.Image"), System.Drawing.Image)
        Me.mnuMinCountMP.IsChange = False
        Me.mnuMinCountMP.IsFavVisible = True
        Me.mnuMinCountMP.Name = "mnuMinCountMP"
        Me.mnuMinCountMP.Size = New System.Drawing.Size(518, 22)
        Me.mnuMinCountMP.Text = "(3-9) فهرست قطعي هاي فشار متوسط به ترتيب تعداد دفعات قطع فيدر"
        '
        'mnuMinCountMPArea
        '
        Me.mnuMinCountMPArea.FavVisible = False
        Me.mnuMinCountMPArea.Image = CType(resources.GetObject("mnuMinCountMPArea.Image"), System.Drawing.Image)
        Me.mnuMinCountMPArea.IsChange = False
        Me.mnuMinCountMPArea.IsFavVisible = True
        Me.mnuMinCountMPArea.Name = "mnuMinCountMPArea"
        Me.mnuMinCountMPArea.Size = New System.Drawing.Size(518, 22)
        Me.mnuMinCountMPArea.Text = "(3-10) فهرست قطعي هاي فشار متوسط به ترتيب تعداد دفعات قطع فيدر به تفکيک ناحيه"
        '
        'mnuMinCountFeederPart
        '
        Me.mnuMinCountFeederPart.FavVisible = False
        Me.mnuMinCountFeederPart.Image = CType(resources.GetObject("mnuMinCountFeederPart.Image"), System.Drawing.Image)
        Me.mnuMinCountFeederPart.IsChange = False
        Me.mnuMinCountFeederPart.IsFavVisible = True
        Me.mnuMinCountFeederPart.Name = "mnuMinCountFeederPart"
        Me.mnuMinCountFeederPart.Size = New System.Drawing.Size(518, 22)
        Me.mnuMinCountFeederPart.Text = "(3-30) فهرست قطعي هاي فشار متوسط به ترتيب تعداد دفعات قطع تکه فيدر"
        '
        'mnuLPPostFeederPartCount
        '
        Me.mnuLPPostFeederPartCount.FavVisible = False
        Me.mnuLPPostFeederPartCount.Image = CType(resources.GetObject("mnuLPPostFeederPartCount.Image"), System.Drawing.Image)
        Me.mnuLPPostFeederPartCount.IsChange = False
        Me.mnuLPPostFeederPartCount.IsFavVisible = True
        Me.mnuLPPostFeederPartCount.Name = "mnuLPPostFeederPartCount"
        Me.mnuLPPostFeederPartCount.Size = New System.Drawing.Size(518, 22)
        Me.mnuLPPostFeederPartCount.Text = "(3-31) فهرست قطعي پست‌هاي توزيع بر حسب تکه فيدر"
        '
        'ToolStripSeparator12
        '
        Me.ToolStripSeparator12.Name = "ToolStripSeparator12"
        Me.ToolStripSeparator12.Size = New System.Drawing.Size(515, 6)
        '
        'MenuItem74
        '
        Me.MenuItem74.FavVisible = False
        Me.MenuItem74.Image = CType(resources.GetObject("MenuItem74.Image"), System.Drawing.Image)
        Me.MenuItem74.IsChange = False
        Me.MenuItem74.IsFavVisible = True
        Me.MenuItem74.Name = "MenuItem74"
        Me.MenuItem74.Size = New System.Drawing.Size(518, 22)
        Me.MenuItem74.Text = "(3-11) نمودار مجموع انرژي توزيع نشده بر اساس ماه سال و نوع شبكه فشار متوسط"
        '
        'MenuButtonItem39
        '
        Me.MenuButtonItem39.FavVisible = False
        Me.MenuButtonItem39.Image = CType(resources.GetObject("MenuButtonItem39.Image"), System.Drawing.Image)
        Me.MenuButtonItem39.IsChange = False
        Me.MenuButtonItem39.IsFavVisible = True
        Me.MenuButtonItem39.Name = "MenuButtonItem39"
        Me.MenuButtonItem39.Size = New System.Drawing.Size(518, 22)
        Me.MenuButtonItem39.Text = "(3-12) نمودار تعداد قطعي‌ها بر اساس ماه سال و نوع اشکال در شبكه فشار متوسط"
        '
        'MenuButtonItem40
        '
        Me.MenuButtonItem40.FavVisible = False
        Me.MenuButtonItem40.Image = CType(resources.GetObject("MenuButtonItem40.Image"), System.Drawing.Image)
        Me.MenuButtonItem40.IsChange = False
        Me.MenuButtonItem40.IsFavVisible = True
        Me.MenuButtonItem40.Name = "MenuButtonItem40"
        Me.MenuButtonItem40.Size = New System.Drawing.Size(518, 22)
        Me.MenuButtonItem40.Text = "(3-13) نمودار تعداد قعطي‌ها بر اساس ماه سال و علل اشکالات بوجود آمده در شبكه فشار" &
    " متوسط"
        '
        'MenuItem73
        '
        Me.MenuItem73.FavVisible = False
        Me.MenuItem73.Image = CType(resources.GetObject("MenuItem73.Image"), System.Drawing.Image)
        Me.MenuItem73.IsChange = False
        Me.MenuItem73.IsFavVisible = True
        Me.MenuItem73.Name = "MenuItem73"
        Me.MenuItem73.Size = New System.Drawing.Size(518, 22)
        Me.MenuItem73.Text = "(3-14) دياگرام Pie chart عوامل قطع فيدرهاي فشار متوسط"
        '
        'MenuItem152
        '
        Me.MenuItem152.FavVisible = False
        Me.MenuItem152.Image = CType(resources.GetObject("MenuItem152.Image"), System.Drawing.Image)
        Me.MenuItem152.IsChange = False
        Me.MenuItem152.IsFavVisible = True
        Me.MenuItem152.Name = "MenuItem152"
        Me.MenuItem152.Size = New System.Drawing.Size(518, 22)
        Me.MenuItem152.Text = "(3-15) نمودار تعداد درخواستهاي فشار متوسط به تفكيك تاريخ قطع"
        '
        'MenuItem157
        '
        Me.MenuItem157.FavVisible = False
        Me.MenuItem157.Image = CType(resources.GetObject("MenuItem157.Image"), System.Drawing.Image)
        Me.MenuItem157.IsChange = False
        Me.MenuItem157.IsFavVisible = True
        Me.MenuItem157.Name = "MenuItem157"
        Me.MenuItem157.Size = New System.Drawing.Size(518, 22)
        Me.MenuItem157.Text = "(3-16) نمودار تعداد درخواستهاي فشار متوسط به تفكيك تاريخ قطع و علت خاموشي"
        '
        'MenuItem156
        '
        Me.MenuItem156.FavVisible = False
        Me.MenuItem156.Image = CType(resources.GetObject("MenuItem156.Image"), System.Drawing.Image)
        Me.MenuItem156.IsChange = False
        Me.MenuItem156.IsFavVisible = True
        Me.MenuItem156.Name = "MenuItem156"
        Me.MenuItem156.Size = New System.Drawing.Size(518, 22)
        Me.MenuItem156.Text = "(3-17) نمودار تعداد درخواستهاي فشار متوسط به تفكيك تاريخ قطع و عوامل خاموشي"
        '
        'MenuItem155
        '
        Me.MenuItem155.FavVisible = False
        Me.MenuItem155.Image = CType(resources.GetObject("MenuItem155.Image"), System.Drawing.Image)
        Me.MenuItem155.IsChange = False
        Me.MenuItem155.IsFavVisible = True
        Me.MenuItem155.Name = "MenuItem155"
        Me.MenuItem155.Size = New System.Drawing.Size(518, 22)
        Me.MenuItem155.Text = "(3-18) نمودار تعداد درخواستهاي فشار متوسط به تفكيك تاريخ قطع و علل قطع"
        '
        'MenuItem149
        '
        Me.MenuItem149.FavVisible = False
        Me.MenuItem149.Image = CType(resources.GetObject("MenuItem149.Image"), System.Drawing.Image)
        Me.MenuItem149.IsChange = False
        Me.MenuItem149.IsFavVisible = True
        Me.MenuItem149.Name = "MenuItem149"
        Me.MenuItem149.Size = New System.Drawing.Size(518, 22)
        Me.MenuItem149.Text = "(3-19) نمودار تعداد درخواستهاي فشار متوسط به تفكيك ايام هفته"
        '
        'MenuItem153
        '
        Me.MenuItem153.FavVisible = False
        Me.MenuItem153.Image = CType(resources.GetObject("MenuItem153.Image"), System.Drawing.Image)
        Me.MenuItem153.IsChange = False
        Me.MenuItem153.IsFavVisible = True
        Me.MenuItem153.Name = "MenuItem153"
        Me.MenuItem153.Size = New System.Drawing.Size(518, 22)
        Me.MenuItem153.Text = "(3-20) نمودار تعداد درخواستهاي فشار متوسط به تفكيك ايام هفته و علل قطع"
        '
        'MenuItem150
        '
        Me.MenuItem150.FavVisible = False
        Me.MenuItem150.Image = CType(resources.GetObject("MenuItem150.Image"), System.Drawing.Image)
        Me.MenuItem150.IsChange = False
        Me.MenuItem150.IsFavVisible = True
        Me.MenuItem150.Name = "MenuItem150"
        Me.MenuItem150.Size = New System.Drawing.Size(518, 22)
        Me.MenuItem150.Text = "(3-21) نمودار تعداد درخواستهاي فشار متوسط به تفكيك ماههاي سال"
        '
        'MenuItem154
        '
        Me.MenuItem154.FavVisible = False
        Me.MenuItem154.Image = CType(resources.GetObject("MenuItem154.Image"), System.Drawing.Image)
        Me.MenuItem154.IsChange = False
        Me.MenuItem154.IsFavVisible = True
        Me.MenuItem154.Name = "MenuItem154"
        Me.MenuItem154.Size = New System.Drawing.Size(518, 22)
        Me.MenuItem154.Text = "(3-22) نمودار تعداد درخواستهاي فشار متوسط به تفكيك ماههاي سال و علل قطع"
        '
        'MenuItem151
        '
        Me.MenuItem151.FavVisible = False
        Me.MenuItem151.Image = CType(resources.GetObject("MenuItem151.Image"), System.Drawing.Image)
        Me.MenuItem151.IsChange = False
        Me.MenuItem151.IsFavVisible = True
        Me.MenuItem151.Name = "MenuItem151"
        Me.MenuItem151.Size = New System.Drawing.Size(518, 22)
        Me.MenuItem151.Text = "(3-23) نمودار تعداد درخواستهاي فشار متوسط به تفكيك ماههاي سال و ساعت قطع"
        '
        'mnuRpt_3_25
        '
        Me.mnuRpt_3_25.FavVisible = False
        Me.mnuRpt_3_25.Image = CType(resources.GetObject("mnuRpt_3_25.Image"), System.Drawing.Image)
        Me.mnuRpt_3_25.IsChange = False
        Me.mnuRpt_3_25.IsFavVisible = True
        Me.mnuRpt_3_25.Name = "mnuRpt_3_25"
        Me.mnuRpt_3_25.Size = New System.Drawing.Size(518, 22)
        Me.mnuRpt_3_25.Text = "(3-25) نمودار تعداد قطعي‌ها به تفکيک ناحيه و علت قطع"
        '
        'mnuRpt_3_26
        '
        Me.mnuRpt_3_26.FavVisible = False
        Me.mnuRpt_3_26.Image = CType(resources.GetObject("mnuRpt_3_26.Image"), System.Drawing.Image)
        Me.mnuRpt_3_26.IsChange = False
        Me.mnuRpt_3_26.IsFavVisible = True
        Me.mnuRpt_3_26.Name = "mnuRpt_3_26"
        Me.mnuRpt_3_26.Size = New System.Drawing.Size(518, 22)
        Me.mnuRpt_3_26.Text = "(3-26) گزارش مديريت انرژي توزيع نشده با استفاده از مانورهاي صورت گرفته"
        '
        'mnuRepLPRequest
        '
        Me.mnuRepLPRequest.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuLP, Me.mnuLP_Mode1, Me.mnuLP_Mode2, Me.MnuReport_4_22, Me.MnuReport_4_23, Me.ToolStripSeparator18, Me.MenuItem56, Me.MenuItem57, Me.MenuButtonItem45, Me.ToolStripSeparator17, Me.mnuMinCountLPPost, Me.mnuMinCountLPFeeder, Me.MnuReport_4_26, Me.ToolStripSeparator16, Me.MenuItem54, Me.MenuItem65, Me.MenuButtonItem36, Me.MenuButtonItem38, Me.MenuItem136, Me.MenuItem139, Me.MenuItem100, Me.MenuItem137, Me.MenuItem101, Me.MenuItem138, Me.MenuItem118, Me.ToolStripSeparator15, Me.mnuRep_LPDisconnectDaily, Me.MnuReport_4_24, Me.MnuReport_4_25})
        Me.mnuRepLPRequest.Name = "mnuRepLPRequest"
        Me.mnuRepLPRequest.Size = New System.Drawing.Size(206, 22)
        Me.mnuRepLPRequest.Text = "(4) فشار &ضعيف"
        '
        'mnuLP
        '
        Me.mnuLP.FavVisible = False
        Me.mnuLP.Image = CType(resources.GetObject("mnuLP.Image"), System.Drawing.Image)
        Me.mnuLP.IsChange = False
        Me.mnuLP.IsFavVisible = True
        Me.mnuLP.Name = "mnuLP"
        Me.mnuLP.Size = New System.Drawing.Size(511, 22)
        Me.mnuLP.Text = "(4-1) گزارش روزانه حوادث و قطعي هاي فيدرهاي فشار ضعيف"
        '
        'mnuLP_Mode1
        '
        Me.mnuLP_Mode1.FavVisible = False
        Me.mnuLP_Mode1.Image = CType(resources.GetObject("mnuLP_Mode1.Image"), System.Drawing.Image)
        Me.mnuLP_Mode1.IsChange = False
        Me.mnuLP_Mode1.IsFavVisible = True
        Me.mnuLP_Mode1.Name = "mnuLP_Mode1"
        Me.mnuLP_Mode1.Size = New System.Drawing.Size(511, 22)
        Me.mnuLP_Mode1.Text = "(4-2) گزارش روزانه حوادث و قطعي هاي فيدرهاي فشار ضعيف (نمونه 1)"
        '
        'mnuLP_Mode2
        '
        Me.mnuLP_Mode2.FavVisible = False
        Me.mnuLP_Mode2.Image = CType(resources.GetObject("mnuLP_Mode2.Image"), System.Drawing.Image)
        Me.mnuLP_Mode2.IsChange = False
        Me.mnuLP_Mode2.IsFavVisible = True
        Me.mnuLP_Mode2.Name = "mnuLP_Mode2"
        Me.mnuLP_Mode2.Size = New System.Drawing.Size(511, 22)
        Me.mnuLP_Mode2.Text = "(4-20) گزارش روزانه حوادث و قطعي هاي فيدرهاي فشار ضعيف (نمونه 2)"
        '
        'MnuReport_4_22
        '
        Me.MnuReport_4_22.FavVisible = False
        Me.MnuReport_4_22.Image = CType(resources.GetObject("MnuReport_4_22.Image"), System.Drawing.Image)
        Me.MnuReport_4_22.IsChange = False
        Me.MnuReport_4_22.IsFavVisible = True
        Me.MnuReport_4_22.Name = "MnuReport_4_22"
        Me.MnuReport_4_22.Size = New System.Drawing.Size(511, 22)
        Me.MnuReport_4_22.Text = "(4-22) گزارش روزانه حوادث و قطعي هاي فيدرهاي فشار ضعيف (نمونه 3)"
        '
        'MnuReport_4_23
        '
        Me.MnuReport_4_23.FavVisible = False
        Me.MnuReport_4_23.Image = CType(resources.GetObject("MnuReport_4_23.Image"), System.Drawing.Image)
        Me.MnuReport_4_23.IsChange = False
        Me.MnuReport_4_23.IsFavVisible = True
        Me.MnuReport_4_23.Name = "MnuReport_4_23"
        Me.MnuReport_4_23.Size = New System.Drawing.Size(511, 22)
        Me.MnuReport_4_23.Text = "(4-23) گزارش روزانه حوادث و قطعي هاي فيدرهاي فشار ضعيف (نمونه 4)"
        '
        'ToolStripSeparator18
        '
        Me.ToolStripSeparator18.Name = "ToolStripSeparator18"
        Me.ToolStripSeparator18.Size = New System.Drawing.Size(508, 6)
        '
        'MenuItem56
        '
        Me.MenuItem56.FavVisible = False
        Me.MenuItem56.Image = CType(resources.GetObject("MenuItem56.Image"), System.Drawing.Image)
        Me.MenuItem56.IsChange = False
        Me.MenuItem56.IsFavVisible = True
        Me.MenuItem56.Name = "MenuItem56"
        Me.MenuItem56.Size = New System.Drawing.Size(511, 22)
        Me.MenuItem56.Text = "(4-3) گزارش تجهيزات مصرف شده در شبكه فشار ضعيف به تفکيک پرونده"
        '
        'MenuItem57
        '
        Me.MenuItem57.FavVisible = False
        Me.MenuItem57.Image = CType(resources.GetObject("MenuItem57.Image"), System.Drawing.Image)
        Me.MenuItem57.IsChange = False
        Me.MenuItem57.IsFavVisible = True
        Me.MenuItem57.Name = "MenuItem57"
        Me.MenuItem57.Size = New System.Drawing.Size(511, 22)
        Me.MenuItem57.Text = "(4-5) گزارش آماري تجهيزات مصرف شده در شبكه فشار ضعيف به تفکيک نواحي"
        '
        'MenuButtonItem45
        '
        Me.MenuButtonItem45.FavVisible = False
        Me.MenuButtonItem45.Image = CType(resources.GetObject("MenuButtonItem45.Image"), System.Drawing.Image)
        Me.MenuButtonItem45.IsChange = False
        Me.MenuButtonItem45.IsFavVisible = True
        Me.MenuButtonItem45.Name = "MenuButtonItem45"
        Me.MenuButtonItem45.Size = New System.Drawing.Size(511, 22)
        Me.MenuButtonItem45.Text = "(4-21) گزارش آماري تجهيزات مصرف شده در شبكه فشار ضعيف بدون تفکيک نواحي"
        '
        'ToolStripSeparator17
        '
        Me.ToolStripSeparator17.Name = "ToolStripSeparator17"
        Me.ToolStripSeparator17.Size = New System.Drawing.Size(508, 6)
        '
        'mnuMinCountLPPost
        '
        Me.mnuMinCountLPPost.FavVisible = False
        Me.mnuMinCountLPPost.Image = CType(resources.GetObject("mnuMinCountLPPost.Image"), System.Drawing.Image)
        Me.mnuMinCountLPPost.IsChange = False
        Me.mnuMinCountLPPost.IsFavVisible = True
        Me.mnuMinCountLPPost.Name = "mnuMinCountLPPost"
        Me.mnuMinCountLPPost.Size = New System.Drawing.Size(511, 22)
        Me.mnuMinCountLPPost.Text = "(4-6) فهرست قطعي هاي فشار ضعيف به ترتيب تعداد دفعات قطع پست"
        '
        'mnuMinCountLPFeeder
        '
        Me.mnuMinCountLPFeeder.FavVisible = False
        Me.mnuMinCountLPFeeder.Image = CType(resources.GetObject("mnuMinCountLPFeeder.Image"), System.Drawing.Image)
        Me.mnuMinCountLPFeeder.IsChange = False
        Me.mnuMinCountLPFeeder.IsFavVisible = True
        Me.mnuMinCountLPFeeder.Name = "mnuMinCountLPFeeder"
        Me.mnuMinCountLPFeeder.Size = New System.Drawing.Size(511, 22)
        Me.mnuMinCountLPFeeder.Text = "(4-7) فهرست قطعي هاي فشار ضعيف به ترتيب تعداد دفعات قطع فيدر"
        '
        'MnuReport_4_26
        '
        Me.MnuReport_4_26.FavVisible = False
        Me.MnuReport_4_26.Image = CType(resources.GetObject("MnuReport_4_26.Image"), System.Drawing.Image)
        Me.MnuReport_4_26.IsChange = False
        Me.MnuReport_4_26.IsFavVisible = True
        Me.MnuReport_4_26.Name = "MnuReport_4_26"
        Me.MnuReport_4_26.Size = New System.Drawing.Size(511, 22)
        Me.MnuReport_4_26.Text = "(4-26) فهرست قطعی های فشار ضعیف به ترتیب تعداد دفعات قطع فيدر براساس تماس مشترک"
        '
        'ToolStripSeparator16
        '
        Me.ToolStripSeparator16.Name = "ToolStripSeparator16"
        Me.ToolStripSeparator16.Size = New System.Drawing.Size(508, 6)
        '
        'MenuItem54
        '
        Me.MenuItem54.FavVisible = False
        Me.MenuItem54.Image = CType(resources.GetObject("MenuItem54.Image"), System.Drawing.Image)
        Me.MenuItem54.IsChange = False
        Me.MenuItem54.IsFavVisible = True
        Me.MenuItem54.Name = "MenuItem54"
        Me.MenuItem54.Size = New System.Drawing.Size(511, 22)
        Me.MenuItem54.Text = "(4-8) دياگرام مجموع انرژي توزيع نشده بر اساس ماه سال و علت قطع شبكه فشار ضعيف"
        '
        'MenuItem65
        '
        Me.MenuItem65.FavVisible = False
        Me.MenuItem65.Image = CType(resources.GetObject("MenuItem65.Image"), System.Drawing.Image)
        Me.MenuItem65.IsChange = False
        Me.MenuItem65.IsFavVisible = True
        Me.MenuItem65.Name = "MenuItem65"
        Me.MenuItem65.Size = New System.Drawing.Size(511, 22)
        Me.MenuItem65.Text = "(4-9) دياگرام Pie chart عوامل قطع فيدرهاي 400 ولت"
        '
        'MenuButtonItem36
        '
        Me.MenuButtonItem36.FavVisible = False
        Me.MenuButtonItem36.Image = CType(resources.GetObject("MenuButtonItem36.Image"), System.Drawing.Image)
        Me.MenuButtonItem36.IsChange = False
        Me.MenuButtonItem36.IsFavVisible = True
        Me.MenuButtonItem36.Name = "MenuButtonItem36"
        Me.MenuButtonItem36.Size = New System.Drawing.Size(511, 22)
        Me.MenuButtonItem36.Text = "(4-10) نمودار تعداد قطعي‌ها و اشکالات بوجود آمده در فيدرهاي 400 ولت"
        '
        'MenuButtonItem38
        '
        Me.MenuButtonItem38.FavVisible = False
        Me.MenuButtonItem38.Image = CType(resources.GetObject("MenuButtonItem38.Image"), System.Drawing.Image)
        Me.MenuButtonItem38.IsChange = False
        Me.MenuButtonItem38.IsFavVisible = True
        Me.MenuButtonItem38.Name = "MenuButtonItem38"
        Me.MenuButtonItem38.Size = New System.Drawing.Size(511, 22)
        Me.MenuButtonItem38.Text = "(4-11) نمودار تعداد قطعي و عوامل خاموشي در فيدرهاي 400 ولت"
        '
        'MenuItem136
        '
        Me.MenuItem136.FavVisible = False
        Me.MenuItem136.Image = CType(resources.GetObject("MenuItem136.Image"), System.Drawing.Image)
        Me.MenuItem136.IsChange = False
        Me.MenuItem136.IsFavVisible = True
        Me.MenuItem136.Name = "MenuItem136"
        Me.MenuItem136.Size = New System.Drawing.Size(511, 22)
        Me.MenuItem136.Text = "(4-12) نمودار تعداد درخواستهاي فشار ضعيف به تفكيك تاريخ قطع"
        '
        'MenuItem139
        '
        Me.MenuItem139.FavVisible = False
        Me.MenuItem139.Image = CType(resources.GetObject("MenuItem139.Image"), System.Drawing.Image)
        Me.MenuItem139.IsChange = False
        Me.MenuItem139.IsFavVisible = True
        Me.MenuItem139.Name = "MenuItem139"
        Me.MenuItem139.Size = New System.Drawing.Size(511, 22)
        Me.MenuItem139.Text = "(4-13) نمودار تعداد درخواستهاي فشار ضعيف به تفكيك تاريخ قطع و عوامل قطع"
        '
        'MenuItem100
        '
        Me.MenuItem100.FavVisible = False
        Me.MenuItem100.Image = CType(resources.GetObject("MenuItem100.Image"), System.Drawing.Image)
        Me.MenuItem100.IsChange = False
        Me.MenuItem100.IsFavVisible = True
        Me.MenuItem100.Name = "MenuItem100"
        Me.MenuItem100.Size = New System.Drawing.Size(511, 22)
        Me.MenuItem100.Text = "(4-14) نمودار تعداد درخواستهاي فشار ضعيف به تفكيك ايام هفته"
        '
        'MenuItem137
        '
        Me.MenuItem137.FavVisible = False
        Me.MenuItem137.Image = CType(resources.GetObject("MenuItem137.Image"), System.Drawing.Image)
        Me.MenuItem137.IsChange = False
        Me.MenuItem137.IsFavVisible = True
        Me.MenuItem137.Name = "MenuItem137"
        Me.MenuItem137.Size = New System.Drawing.Size(511, 22)
        Me.MenuItem137.Text = "(4-15) نمودار تعداد درخواستهاي فشار ضعيف به تفكيك ايام هفته و علل قطع"
        '
        'MenuItem101
        '
        Me.MenuItem101.FavVisible = False
        Me.MenuItem101.Image = CType(resources.GetObject("MenuItem101.Image"), System.Drawing.Image)
        Me.MenuItem101.IsChange = False
        Me.MenuItem101.IsFavVisible = True
        Me.MenuItem101.Name = "MenuItem101"
        Me.MenuItem101.Size = New System.Drawing.Size(511, 22)
        Me.MenuItem101.Text = "(4-16) نمودار تعداد درخواستهاي فشار ضعيف به تفكيك ماههاي سال"
        '
        'MenuItem138
        '
        Me.MenuItem138.FavVisible = False
        Me.MenuItem138.Image = CType(resources.GetObject("MenuItem138.Image"), System.Drawing.Image)
        Me.MenuItem138.IsChange = False
        Me.MenuItem138.IsFavVisible = True
        Me.MenuItem138.Name = "MenuItem138"
        Me.MenuItem138.Size = New System.Drawing.Size(511, 22)
        Me.MenuItem138.Text = "(4-17) نمودار تعداد درخواستهاي فشار ضعيف به تفكيك ماههاي سال و علل قطع"
        '
        'MenuItem118
        '
        Me.MenuItem118.FavVisible = False
        Me.MenuItem118.Image = CType(resources.GetObject("MenuItem118.Image"), System.Drawing.Image)
        Me.MenuItem118.IsChange = False
        Me.MenuItem118.IsFavVisible = True
        Me.MenuItem118.Name = "MenuItem118"
        Me.MenuItem118.Size = New System.Drawing.Size(511, 22)
        Me.MenuItem118.Text = "(4-18) نمودار تعداد درخواستهاي فشار ضعيف به تفكيك ماههاي سال و ساعات قطع"
        '
        'ToolStripSeparator15
        '
        Me.ToolStripSeparator15.Name = "ToolStripSeparator15"
        Me.ToolStripSeparator15.Size = New System.Drawing.Size(508, 6)
        '
        'mnuRep_LPDisconnectDaily
        '
        Me.mnuRep_LPDisconnectDaily.FavVisible = False
        Me.mnuRep_LPDisconnectDaily.Image = CType(resources.GetObject("mnuRep_LPDisconnectDaily.Image"), System.Drawing.Image)
        Me.mnuRep_LPDisconnectDaily.IsChange = False
        Me.mnuRep_LPDisconnectDaily.IsFavVisible = True
        Me.mnuRep_LPDisconnectDaily.Name = "mnuRep_LPDisconnectDaily"
        Me.mnuRep_LPDisconnectDaily.Size = New System.Drawing.Size(511, 22)
        Me.mnuRep_LPDisconnectDaily.Text = "(4-19) گزارش روزانه خاموشي‌هاي فشار ضعيف"
        '
        'MnuReport_4_24
        '
        Me.MnuReport_4_24.FavVisible = False
        Me.MnuReport_4_24.Image = CType(resources.GetObject("MnuReport_4_24.Image"), System.Drawing.Image)
        Me.MnuReport_4_24.IsChange = False
        Me.MnuReport_4_24.IsFavVisible = True
        Me.MnuReport_4_24.Name = "MnuReport_4_24"
        Me.MnuReport_4_24.Size = New System.Drawing.Size(511, 22)
        Me.MnuReport_4_24.Text = "(4-24) گزارش خاموشي هاي تک مشترک فشار ضعيف"
        '
        'MnuReport_4_25
        '
        Me.MnuReport_4_25.FavVisible = False
        Me.MnuReport_4_25.Image = CType(resources.GetObject("MnuReport_4_25.Image"), System.Drawing.Image)
        Me.MnuReport_4_25.IsChange = False
        Me.MnuReport_4_25.IsFavVisible = True
        Me.MnuReport_4_25.Name = "MnuReport_4_25"
        Me.MnuReport_4_25.Size = New System.Drawing.Size(511, 22)
        Me.MnuReport_4_25.Text = "(4-25) گزارش بار خطوط در مانور فشار ضعيف"
        '
        'mnuRepLightRequest
        '
        Me.mnuRepLightRequest.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuLightDaily, Me.ToolStripSeparator21, Me.mnuLightRequestParts, Me.mnuLightUseParts, Me.MenuButtonItem47, Me.ToolStripSeparator20, Me.mnuRepportSummaryLight, Me.mnuLightDisconnectDaily, Me.ToolStripSeparator19, Me.MenuItem80, Me.MenuItem81, Me.MenuItem140, Me.MenuItem142, Me.MenuItem143, Me.MenuItem144, Me.MenuItem145, Me.MenuItem146, Me.MenuItem147})
        Me.mnuRepLightRequest.Name = "mnuRepLightRequest"
        Me.mnuRepLightRequest.Size = New System.Drawing.Size(206, 22)
        Me.mnuRepLightRequest.Text = "(5) &روشنايي معابر"
        '
        'mnuLightDaily
        '
        Me.mnuLightDaily.FavVisible = False
        Me.mnuLightDaily.Image = CType(resources.GetObject("mnuLightDaily.Image"), System.Drawing.Image)
        Me.mnuLightDaily.IsChange = False
        Me.mnuLightDaily.IsFavVisible = True
        Me.mnuLightDaily.Name = "mnuLightDaily"
        Me.mnuLightDaily.Size = New System.Drawing.Size(459, 22)
        Me.mnuLightDaily.Text = "(5-1) گزارش روزانه روشنايي معابر"
        '
        'ToolStripSeparator21
        '
        Me.ToolStripSeparator21.Name = "ToolStripSeparator21"
        Me.ToolStripSeparator21.Size = New System.Drawing.Size(456, 6)
        '
        'mnuLightRequestParts
        '
        Me.mnuLightRequestParts.FavVisible = False
        Me.mnuLightRequestParts.Image = CType(resources.GetObject("mnuLightRequestParts.Image"), System.Drawing.Image)
        Me.mnuLightRequestParts.IsChange = False
        Me.mnuLightRequestParts.IsFavVisible = True
        Me.mnuLightRequestParts.Name = "mnuLightRequestParts"
        Me.mnuLightRequestParts.Size = New System.Drawing.Size(459, 22)
        Me.mnuLightRequestParts.Text = "(5-2) گزارش تجهيزات مصرف شده در شبكه روشنايي معابر به تفکيک پرونده"
        '
        'mnuLightUseParts
        '
        Me.mnuLightUseParts.FavVisible = False
        Me.mnuLightUseParts.Image = CType(resources.GetObject("mnuLightUseParts.Image"), System.Drawing.Image)
        Me.mnuLightUseParts.IsChange = False
        Me.mnuLightUseParts.IsFavVisible = True
        Me.mnuLightUseParts.Name = "mnuLightUseParts"
        Me.mnuLightUseParts.Size = New System.Drawing.Size(459, 22)
        Me.mnuLightUseParts.Text = "(5-3) گزارش آماري تجهيزات مصرف شده در شبكه روشنايي معابر به تفکيک نواحي"
        '
        'MenuButtonItem47
        '
        Me.MenuButtonItem47.FavVisible = False
        Me.MenuButtonItem47.Image = CType(resources.GetObject("MenuButtonItem47.Image"), System.Drawing.Image)
        Me.MenuButtonItem47.IsChange = False
        Me.MenuButtonItem47.IsFavVisible = True
        Me.MenuButtonItem47.Name = "MenuButtonItem47"
        Me.MenuButtonItem47.Size = New System.Drawing.Size(459, 22)
        Me.MenuButtonItem47.Text = "(5-7) گزارش آماري تجهيزات مصرف شده در شبكه روشنايي معابر بدون تفکيک نواحي"
        '
        'ToolStripSeparator20
        '
        Me.ToolStripSeparator20.Name = "ToolStripSeparator20"
        Me.ToolStripSeparator20.Size = New System.Drawing.Size(456, 6)
        '
        'mnuRepportSummaryLight
        '
        Me.mnuRepportSummaryLight.FavVisible = False
        Me.mnuRepportSummaryLight.Image = CType(resources.GetObject("mnuRepportSummaryLight.Image"), System.Drawing.Image)
        Me.mnuRepportSummaryLight.IsChange = False
        Me.mnuRepportSummaryLight.IsFavVisible = True
        Me.mnuRepportSummaryLight.Name = "mnuRepportSummaryLight"
        Me.mnuRepportSummaryLight.Size = New System.Drawing.Size(459, 22)
        Me.mnuRepportSummaryLight.Text = "(5-4) خلاصه گزارش روشنايي معابر"
        '
        'mnuLightDisconnectDaily
        '
        Me.mnuLightDisconnectDaily.FavVisible = False
        Me.mnuLightDisconnectDaily.Image = CType(resources.GetObject("mnuLightDisconnectDaily.Image"), System.Drawing.Image)
        Me.mnuLightDisconnectDaily.IsChange = False
        Me.mnuLightDisconnectDaily.IsFavVisible = True
        Me.mnuLightDisconnectDaily.Name = "mnuLightDisconnectDaily"
        Me.mnuLightDisconnectDaily.Size = New System.Drawing.Size(459, 22)
        Me.mnuLightDisconnectDaily.Text = "(5-5) گزارش روزانه حوادث و قطعي هاي فيدرهاي روشنايي معابر"
        '
        'ToolStripSeparator19
        '
        Me.ToolStripSeparator19.Name = "ToolStripSeparator19"
        Me.ToolStripSeparator19.Size = New System.Drawing.Size(456, 6)
        '
        'MenuItem80
        '
        Me.MenuItem80.FavVisible = False
        Me.MenuItem80.Image = CType(resources.GetObject("MenuItem80.Image"), System.Drawing.Image)
        Me.MenuItem80.IsChange = False
        Me.MenuItem80.IsFavVisible = False
        Me.MenuItem80.Name = "MenuItem80"
        Me.MenuItem80.Size = New System.Drawing.Size(459, 22)
        Me.MenuItem80.Text = "(5-x) دياگرام مجموع انرژي توزيع نشده بر اساس ماه سال و علت قطع روشنايي معابر"
        Me.MenuItem80.Visible = False
        '
        'MenuItem81
        '
        Me.MenuItem81.FavVisible = False
        Me.MenuItem81.Image = CType(resources.GetObject("MenuItem81.Image"), System.Drawing.Image)
        Me.MenuItem81.IsChange = False
        Me.MenuItem81.IsFavVisible = True
        Me.MenuItem81.Name = "MenuItem81"
        Me.MenuItem81.Size = New System.Drawing.Size(459, 22)
        Me.MenuItem81.Text = "(5-6) دياگرام Pie chart عوامل قطع فيدرهاي روشنايي معابر"
        '
        'MenuItem140
        '
        Me.MenuItem140.FavVisible = False
        Me.MenuItem140.Image = CType(resources.GetObject("MenuItem140.Image"), System.Drawing.Image)
        Me.MenuItem140.IsChange = False
        Me.MenuItem140.IsFavVisible = False
        Me.MenuItem140.Name = "MenuItem140"
        Me.MenuItem140.Size = New System.Drawing.Size(459, 22)
        Me.MenuItem140.Text = "(5-x) نمودار تعداد درخواستهاي روشنايي معابر به تفكيك تاريخ قطع"
        Me.MenuItem140.Visible = False
        '
        'MenuItem142
        '
        Me.MenuItem142.FavVisible = False
        Me.MenuItem142.Image = CType(resources.GetObject("MenuItem142.Image"), System.Drawing.Image)
        Me.MenuItem142.IsChange = False
        Me.MenuItem142.IsFavVisible = False
        Me.MenuItem142.Name = "MenuItem142"
        Me.MenuItem142.Size = New System.Drawing.Size(459, 22)
        Me.MenuItem142.Text = "(5-x) نمودار تعداد درخواستهاي روشنايي معابر به تفكيك تاريخ قطع و علل قطع"
        Me.MenuItem142.Visible = False
        '
        'MenuItem143
        '
        Me.MenuItem143.FavVisible = False
        Me.MenuItem143.Image = CType(resources.GetObject("MenuItem143.Image"), System.Drawing.Image)
        Me.MenuItem143.IsChange = False
        Me.MenuItem143.IsFavVisible = False
        Me.MenuItem143.Name = "MenuItem143"
        Me.MenuItem143.Size = New System.Drawing.Size(459, 22)
        Me.MenuItem143.Text = "(5-x) نمودار تعداد درخواستهاي روشنايي معابر به تفكيك ايام هفته"
        Me.MenuItem143.Visible = False
        '
        'MenuItem144
        '
        Me.MenuItem144.FavVisible = False
        Me.MenuItem144.Image = CType(resources.GetObject("MenuItem144.Image"), System.Drawing.Image)
        Me.MenuItem144.IsChange = False
        Me.MenuItem144.IsFavVisible = False
        Me.MenuItem144.Name = "MenuItem144"
        Me.MenuItem144.Size = New System.Drawing.Size(459, 22)
        Me.MenuItem144.Text = "(5-x) نمودار تعداد درخواستهاي روشنايي معابر به تفكيك ايام هفته و علل قطع"
        Me.MenuItem144.Visible = False
        '
        'MenuItem145
        '
        Me.MenuItem145.FavVisible = False
        Me.MenuItem145.Image = CType(resources.GetObject("MenuItem145.Image"), System.Drawing.Image)
        Me.MenuItem145.IsChange = False
        Me.MenuItem145.IsFavVisible = False
        Me.MenuItem145.Name = "MenuItem145"
        Me.MenuItem145.Size = New System.Drawing.Size(459, 22)
        Me.MenuItem145.Text = "(5-x) نمودار تعداد درخواستهاي روشنايي معابر به تفكيك ماههاي سال"
        Me.MenuItem145.Visible = False
        '
        'MenuItem146
        '
        Me.MenuItem146.FavVisible = False
        Me.MenuItem146.Image = CType(resources.GetObject("MenuItem146.Image"), System.Drawing.Image)
        Me.MenuItem146.IsChange = False
        Me.MenuItem146.IsFavVisible = False
        Me.MenuItem146.Name = "MenuItem146"
        Me.MenuItem146.Size = New System.Drawing.Size(459, 22)
        Me.MenuItem146.Text = "(5-x) نمودار تعداد درخواستهاي روشنايي معابر به تفكيك ماههاي سال و علل قطع"
        Me.MenuItem146.Visible = False
        '
        'MenuItem147
        '
        Me.MenuItem147.FavVisible = False
        Me.MenuItem147.Image = CType(resources.GetObject("MenuItem147.Image"), System.Drawing.Image)
        Me.MenuItem147.IsChange = False
        Me.MenuItem147.IsFavVisible = False
        Me.MenuItem147.Name = "MenuItem147"
        Me.MenuItem147.Size = New System.Drawing.Size(459, 22)
        Me.MenuItem147.Text = "(5-x) نمودار تعداد درخواستهاي روشنايي معابر به تفكيك ماههاي سال و ساعات قطع"
        Me.MenuItem147.Visible = False
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(203, 6)
        '
        'mnuRepBaseInfo
        '
        Me.mnuRepBaseInfo.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuItem121, Me.MenuItem122, Me.MenuItem126, Me.mnuReportLPTrans, Me.MenuItem127, Me.MenuItem124, Me.MenuItem125, Me.MenuItem129, Me.mnuRep_FeederParts, Me.ToolStripSeparator22, Me.mnuLPPostCapacity, Me.mnuLPPostFeederCapacity, Me.mnuRepAreaFeeders, Me.mnuRepCountLPPost, Me.ToolStripSeparator23, Me.mnuRep_MPCriticalFeeder, Me.mnuRep_LPCriticalFeeder, Me.ToolStripSeparator24, Me.mnuRptHistoryMPFeeder, Me.mnuRptHistoryLPPost, Me.mnuRepLPTransAction, Me.ToolStripSeparator25, Me.MenuButtonItem64, Me.mnuReportStore})
        Me.mnuRepBaseInfo.Name = "mnuRepBaseInfo"
        Me.mnuRepBaseInfo.Size = New System.Drawing.Size(206, 22)
        Me.mnuRepBaseInfo.Text = "(6) &اطلاعات پايه"
        '
        'MenuItem121
        '
        Me.MenuItem121.FavVisible = False
        Me.MenuItem121.Image = CType(resources.GetObject("MenuItem121.Image"), System.Drawing.Image)
        Me.MenuItem121.IsChange = False
        Me.MenuItem121.IsFavVisible = True
        Me.MenuItem121.Name = "MenuItem121"
        Me.MenuItem121.Size = New System.Drawing.Size(397, 22)
        Me.MenuItem121.Text = "(6-1) پستهاي &توزيع"
        '
        'MenuItem122
        '
        Me.MenuItem122.FavVisible = False
        Me.MenuItem122.Image = CType(resources.GetObject("MenuItem122.Image"), System.Drawing.Image)
        Me.MenuItem122.IsChange = False
        Me.MenuItem122.IsFavVisible = True
        Me.MenuItem122.Name = "MenuItem122"
        Me.MenuItem122.Size = New System.Drawing.Size(397, 22)
        Me.MenuItem122.Text = "(6-2) فيدرهاي فشار &ضعيف"
        '
        'MenuItem126
        '
        Me.MenuItem126.FavVisible = False
        Me.MenuItem126.Image = CType(resources.GetObject("MenuItem126.Image"), System.Drawing.Image)
        Me.MenuItem126.IsChange = False
        Me.MenuItem126.IsFavVisible = True
        Me.MenuItem126.Name = "MenuItem126"
        Me.MenuItem126.Size = New System.Drawing.Size(397, 22)
        Me.MenuItem126.Text = "(6-3) &قطعات فشار ضعيف"
        '
        'mnuReportLPTrans
        '
        Me.mnuReportLPTrans.FavVisible = False
        Me.mnuReportLPTrans.Image = CType(resources.GetObject("mnuReportLPTrans.Image"), System.Drawing.Image)
        Me.mnuReportLPTrans.IsChange = False
        Me.mnuReportLPTrans.IsFavVisible = True
        Me.mnuReportLPTrans.Name = "mnuReportLPTrans"
        Me.mnuReportLPTrans.Size = New System.Drawing.Size(397, 22)
        Me.mnuReportLPTrans.Text = "(6-18) ترانس‌هاي پست‌هاي توزيع"
        '
        'MenuItem127
        '
        Me.MenuItem127.FavVisible = False
        Me.MenuItem127.Image = CType(resources.GetObject("MenuItem127.Image"), System.Drawing.Image)
        Me.MenuItem127.IsChange = False
        Me.MenuItem127.IsFavVisible = True
        Me.MenuItem127.Name = "MenuItem127"
        Me.MenuItem127.Size = New System.Drawing.Size(397, 22)
        Me.MenuItem127.Text = "(6-4) قطعات &روشنايي معابر"
        '
        'MenuItem124
        '
        Me.MenuItem124.FavVisible = False
        Me.MenuItem124.Image = CType(resources.GetObject("MenuItem124.Image"), System.Drawing.Image)
        Me.MenuItem124.IsChange = False
        Me.MenuItem124.IsFavVisible = True
        Me.MenuItem124.Name = "MenuItem124"
        Me.MenuItem124.Size = New System.Drawing.Size(397, 22)
        Me.MenuItem124.Text = "(6-5) پستهاي &فوق توزيع"
        '
        'MenuItem125
        '
        Me.MenuItem125.FavVisible = False
        Me.MenuItem125.Image = CType(resources.GetObject("MenuItem125.Image"), System.Drawing.Image)
        Me.MenuItem125.IsChange = False
        Me.MenuItem125.IsFavVisible = True
        Me.MenuItem125.Name = "MenuItem125"
        Me.MenuItem125.Size = New System.Drawing.Size(397, 22)
        Me.MenuItem125.Text = "(6-6) فيدرهاي فشار &متوسط"
        '
        'MenuItem129
        '
        Me.MenuItem129.FavVisible = False
        Me.MenuItem129.Image = CType(resources.GetObject("MenuItem129.Image"), System.Drawing.Image)
        Me.MenuItem129.IsChange = False
        Me.MenuItem129.IsFavVisible = True
        Me.MenuItem129.Name = "MenuItem129"
        Me.MenuItem129.Size = New System.Drawing.Size(397, 22)
        Me.MenuItem129.Text = "(6-7) ق&طعات فشار متوسط"
        '
        'mnuRep_FeederParts
        '
        Me.mnuRep_FeederParts.FavVisible = False
        Me.mnuRep_FeederParts.Image = CType(resources.GetObject("mnuRep_FeederParts.Image"), System.Drawing.Image)
        Me.mnuRep_FeederParts.IsChange = False
        Me.mnuRep_FeederParts.IsFavVisible = True
        Me.mnuRep_FeederParts.Name = "mnuRep_FeederParts"
        Me.mnuRep_FeederParts.Size = New System.Drawing.Size(397, 22)
        Me.mnuRep_FeederParts.Tag = "6-12"
        Me.mnuRep_FeederParts.Text = "(6-12) تکه فيدرها"
        '
        'ToolStripSeparator22
        '
        Me.ToolStripSeparator22.Name = "ToolStripSeparator22"
        Me.ToolStripSeparator22.Size = New System.Drawing.Size(394, 6)
        '
        'mnuLPPostCapacity
        '
        Me.mnuLPPostCapacity.FavVisible = False
        Me.mnuLPPostCapacity.Image = CType(resources.GetObject("mnuLPPostCapacity.Image"), System.Drawing.Image)
        Me.mnuLPPostCapacity.IsChange = False
        Me.mnuLPPostCapacity.IsFavVisible = True
        Me.mnuLPPostCapacity.Name = "mnuLPPostCapacity"
        Me.mnuLPPostCapacity.Size = New System.Drawing.Size(397, 22)
        Me.mnuLPPostCapacity.Text = "(6-8) آمار پست‌هاي ناحيه‌ها به تفکيک قدرت"
        '
        'mnuLPPostFeederCapacity
        '
        Me.mnuLPPostFeederCapacity.FavVisible = False
        Me.mnuLPPostFeederCapacity.Image = CType(resources.GetObject("mnuLPPostFeederCapacity.Image"), System.Drawing.Image)
        Me.mnuLPPostFeederCapacity.IsChange = False
        Me.mnuLPPostFeederCapacity.IsFavVisible = True
        Me.mnuLPPostFeederCapacity.Name = "mnuLPPostFeederCapacity"
        Me.mnuLPPostFeederCapacity.Size = New System.Drawing.Size(397, 22)
        Me.mnuLPPostFeederCapacity.Text = "(6-14) آمار پست‌هاي توزيع فيدرها به تفکيک قدرت"
        '
        'mnuRepAreaFeeders
        '
        Me.mnuRepAreaFeeders.FavVisible = False
        Me.mnuRepAreaFeeders.Image = CType(resources.GetObject("mnuRepAreaFeeders.Image"), System.Drawing.Image)
        Me.mnuRepAreaFeeders.IsChange = False
        Me.mnuRepAreaFeeders.IsFavVisible = True
        Me.mnuRepAreaFeeders.Name = "mnuRepAreaFeeders"
        Me.mnuRepAreaFeeders.Size = New System.Drawing.Size(397, 22)
        Me.mnuRepAreaFeeders.Text = "(6-9) آمار فيدرهاي فشار متوسط، فشار ضعيف و روشنايي معابر نواحي"
        '
        'mnuRepCountLPPost
        '
        Me.mnuRepCountLPPost.FavVisible = False
        Me.mnuRepCountLPPost.Image = CType(resources.GetObject("mnuRepCountLPPost.Image"), System.Drawing.Image)
        Me.mnuRepCountLPPost.IsChange = False
        Me.mnuRepCountLPPost.IsFavVisible = True
        Me.mnuRepCountLPPost.Name = "mnuRepCountLPPost"
        Me.mnuRepCountLPPost.Size = New System.Drawing.Size(397, 22)
        Me.mnuRepCountLPPost.Text = "(6-13) آمار پست هاي توزيع به تفکيک ناحيه ها"
        '
        'ToolStripSeparator23
        '
        Me.ToolStripSeparator23.Name = "ToolStripSeparator23"
        Me.ToolStripSeparator23.Size = New System.Drawing.Size(394, 6)
        '
        'mnuRep_MPCriticalFeeder
        '
        Me.mnuRep_MPCriticalFeeder.FavVisible = False
        Me.mnuRep_MPCriticalFeeder.Image = CType(resources.GetObject("mnuRep_MPCriticalFeeder.Image"), System.Drawing.Image)
        Me.mnuRep_MPCriticalFeeder.IsChange = False
        Me.mnuRep_MPCriticalFeeder.IsFavVisible = True
        Me.mnuRep_MPCriticalFeeder.Name = "mnuRep_MPCriticalFeeder"
        Me.mnuRep_MPCriticalFeeder.Size = New System.Drawing.Size(397, 22)
        Me.mnuRep_MPCriticalFeeder.Text = "(6-10) فيدرهاي حساس فشار متوسط"
        '
        'mnuRep_LPCriticalFeeder
        '
        Me.mnuRep_LPCriticalFeeder.FavVisible = False
        Me.mnuRep_LPCriticalFeeder.Image = CType(resources.GetObject("mnuRep_LPCriticalFeeder.Image"), System.Drawing.Image)
        Me.mnuRep_LPCriticalFeeder.IsChange = False
        Me.mnuRep_LPCriticalFeeder.IsFavVisible = True
        Me.mnuRep_LPCriticalFeeder.Name = "mnuRep_LPCriticalFeeder"
        Me.mnuRep_LPCriticalFeeder.Size = New System.Drawing.Size(397, 22)
        Me.mnuRep_LPCriticalFeeder.Text = "(6-11) فيدرهاي حساس فشار ضعيف"
        '
        'ToolStripSeparator24
        '
        Me.ToolStripSeparator24.Name = "ToolStripSeparator24"
        Me.ToolStripSeparator24.Size = New System.Drawing.Size(394, 6)
        '
        'mnuRptHistoryMPFeeder
        '
        Me.mnuRptHistoryMPFeeder.FavVisible = False
        Me.mnuRptHistoryMPFeeder.Image = CType(resources.GetObject("mnuRptHistoryMPFeeder.Image"), System.Drawing.Image)
        Me.mnuRptHistoryMPFeeder.IsChange = False
        Me.mnuRptHistoryMPFeeder.IsFavVisible = True
        Me.mnuRptHistoryMPFeeder.Name = "mnuRptHistoryMPFeeder"
        Me.mnuRptHistoryMPFeeder.Size = New System.Drawing.Size(397, 22)
        Me.mnuRptHistoryMPFeeder.Tag = "1"
        Me.mnuRptHistoryMPFeeder.Text = "(6-15) گزارش تاريخچه تغييرات فيدر فشار متوسط"
        '
        'mnuRptHistoryLPPost
        '
        Me.mnuRptHistoryLPPost.FavVisible = False
        Me.mnuRptHistoryLPPost.Image = CType(resources.GetObject("mnuRptHistoryLPPost.Image"), System.Drawing.Image)
        Me.mnuRptHistoryLPPost.IsChange = False
        Me.mnuRptHistoryLPPost.IsFavVisible = True
        Me.mnuRptHistoryLPPost.Name = "mnuRptHistoryLPPost"
        Me.mnuRptHistoryLPPost.Size = New System.Drawing.Size(397, 22)
        Me.mnuRptHistoryLPPost.Tag = "2"
        Me.mnuRptHistoryLPPost.Text = "(6-16) گزارش تاريخچه تغييرات پست توزيع"
        '
        'mnuRepLPTransAction
        '
        Me.mnuRepLPTransAction.FavVisible = False
        Me.mnuRepLPTransAction.Image = CType(resources.GetObject("mnuRepLPTransAction.Image"), System.Drawing.Image)
        Me.mnuRepLPTransAction.IsChange = False
        Me.mnuRepLPTransAction.IsFavVisible = True
        Me.mnuRepLPTransAction.Name = "mnuRepLPTransAction"
        Me.mnuRepLPTransAction.Size = New System.Drawing.Size(397, 22)
        Me.mnuRepLPTransAction.Text = "(6-17) گزارش تاريخچه تغييرات ترانس بر روي پست"
        '
        'ToolStripSeparator25
        '
        Me.ToolStripSeparator25.Name = "ToolStripSeparator25"
        Me.ToolStripSeparator25.Size = New System.Drawing.Size(394, 6)
        '
        'MenuButtonItem64
        '
        Me.MenuButtonItem64.FavVisible = False
        Me.MenuButtonItem64.Image = CType(resources.GetObject("MenuButtonItem64.Image"), System.Drawing.Image)
        Me.MenuButtonItem64.IsChange = False
        Me.MenuButtonItem64.IsFavVisible = True
        Me.MenuButtonItem64.Name = "MenuButtonItem64"
        Me.MenuButtonItem64.Size = New System.Drawing.Size(397, 22)
        Me.MenuButtonItem64.Text = "(6-19) گزارش آماري تجهيزات به کار رفته در پستهاي توزيع"
        '
        'mnuReportStore
        '
        Me.mnuReportStore.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuRepExistance, Me.mnuRepCartex})
        Me.mnuReportStore.Name = "mnuReportStore"
        Me.mnuReportStore.Size = New System.Drawing.Size(397, 22)
        Me.mnuReportStore.Text = "(6-20) انبارک"
        '
        'mnuRepExistance
        '
        Me.mnuRepExistance.FavVisible = False
        Me.mnuRepExistance.Image = CType(resources.GetObject("mnuRepExistance.Image"), System.Drawing.Image)
        Me.mnuRepExistance.IsChange = False
        Me.mnuRepExistance.IsFavVisible = True
        Me.mnuRepExistance.Name = "mnuRepExistance"
        Me.mnuRepExistance.Size = New System.Drawing.Size(175, 22)
        Me.mnuRepExistance.Text = "(6-20-1) موجودی انبار"
        '
        'mnuRepCartex
        '
        Me.mnuRepCartex.FavVisible = False
        Me.mnuRepCartex.Image = CType(resources.GetObject("mnuRepCartex.Image"), System.Drawing.Image)
        Me.mnuRepCartex.IsChange = False
        Me.mnuRepCartex.IsFavVisible = True
        Me.mnuRepCartex.Name = "mnuRepCartex"
        Me.mnuRepCartex.Size = New System.Drawing.Size(175, 22)
        Me.mnuRepCartex.Text = "(6-20-2) کارتکس انبار"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(203, 6)
        '
        'mnuRepDorehei
        '
        Me.mnuRepDorehei.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuItem47, Me.MenuItem107, Me.MenuButtonItem5, Me.MenuButtonItem9, Me.mnuReport5_1_1_mode2, Me.mnuRep_7_28, Me.MenuButtonItem4, Me.MenuButtonItem23, Me.MenuButtonItem42, Me.MenuButtonItem43, Me.MenuButtonItem24, Me.MenuButtonItem25, Me.mnuRepDispaching, Me.mnuRepMPStatusChart, Me.mnuMPFeederDisconnetInfo, Me.mnuRepDisconnectMonthlyCount, Me.mnuRepDisconnectMonthlyCountMode2, Me.mnuRepDisconnectGroupSetGroup, Me.mnuRepGroupTamir, Me.mnuRep5_1_1_Byerver121, Me.mnuRepDCRecloser, Me.mnuRep_7_21, Me.mnuRep_7_22, Me.mnuRep_7_23, Me.mnuRep_7_24, Me.mnuRep_7_27})
        Me.mnuRepDorehei.Name = "mnuRepDorehei"
        Me.mnuRepDorehei.Size = New System.Drawing.Size(206, 22)
        Me.mnuRepDorehei.Text = "(7) گزارشات دوره‌اي"
        '
        'MenuItem47
        '
        Me.MenuItem47.FavVisible = False
        Me.MenuItem47.Image = CType(resources.GetObject("MenuItem47.Image"), System.Drawing.Image)
        Me.MenuItem47.IsChange = False
        Me.MenuItem47.IsFavVisible = True
        Me.MenuItem47.Name = "MenuItem47"
        Me.MenuItem47.Size = New System.Drawing.Size(546, 22)
        Me.MenuItem47.Text = "(7-1) جدول تعداد قطعي و انرژي توزيع نشده و مدت زمان خاموشي پستهاي &فوق توزيع"
        '
        'MenuItem107
        '
        Me.MenuItem107.FavVisible = False
        Me.MenuItem107.Image = CType(resources.GetObject("MenuItem107.Image"), System.Drawing.Image)
        Me.MenuItem107.IsChange = False
        Me.MenuItem107.IsFavVisible = True
        Me.MenuItem107.Name = "MenuItem107"
        Me.MenuItem107.Size = New System.Drawing.Size(546, 22)
        Me.MenuItem107.Text = "(7-2) آمار تعداد و ميزان قطع &حفاظتي توزيع فيدرهاي فشار متوسط"
        '
        'MenuButtonItem5
        '
        Me.MenuButtonItem5.FavVisible = False
        Me.MenuButtonItem5.Image = CType(resources.GetObject("MenuButtonItem5.Image"), System.Drawing.Image)
        Me.MenuButtonItem5.IsChange = False
        Me.MenuButtonItem5.IsFavVisible = True
        Me.MenuButtonItem5.Name = "MenuButtonItem5"
        Me.MenuButtonItem5.Size = New System.Drawing.Size(546, 22)
        Me.MenuButtonItem5.Text = "(7-3) خلاصه  گزارش ماهيانه انرژي توزيع نشده ناشي از حوادث و قطعي ها در شبكه هاي ت" &
    "وزيع(فرم 3-4)"
        '
        'MenuButtonItem9
        '
        Me.MenuButtonItem9.FavVisible = False
        Me.MenuButtonItem9.Image = CType(resources.GetObject("MenuButtonItem9.Image"), System.Drawing.Image)
        Me.MenuButtonItem9.IsChange = False
        Me.MenuButtonItem9.IsFavVisible = True
        Me.MenuButtonItem9.Name = "MenuButtonItem9"
        Me.MenuButtonItem9.Size = New System.Drawing.Size(546, 22)
        Me.MenuButtonItem9.Text = "(7-4) گزارش ماهيانه انرژي توزيع نشده (1-1-5)"
        '
        'mnuReport5_1_1_mode2
        '
        Me.mnuReport5_1_1_mode2.FavVisible = False
        Me.mnuReport5_1_1_mode2.Image = CType(resources.GetObject("mnuReport5_1_1_mode2.Image"), System.Drawing.Image)
        Me.mnuReport5_1_1_mode2.IsChange = False
        Me.mnuReport5_1_1_mode2.IsFavVisible = True
        Me.mnuReport5_1_1_mode2.Name = "mnuReport5_1_1_mode2"
        Me.mnuReport5_1_1_mode2.Size = New System.Drawing.Size(546, 22)
        Me.mnuReport5_1_1_mode2.Text = "(7-5) خلاصه گزارش 1_1_5 به تفکيک ناحيه"
        '
        'mnuRep_7_28
        '
        Me.mnuRep_7_28.FavVisible = False
        Me.mnuRep_7_28.Image = CType(resources.GetObject("mnuRep_7_28.Image"), System.Drawing.Image)
        Me.mnuRep_7_28.IsChange = False
        Me.mnuRep_7_28.IsFavVisible = True
        Me.mnuRep_7_28.Name = "mnuRep_7_28"
        Me.mnuRep_7_28.Size = New System.Drawing.Size(546, 22)
        Me.mnuRep_7_28.Text = "(7-28) خلاصه گزارش 1_1_5 به تفکيک ناحيه و سال و ماه"
        '
        'MenuButtonItem4
        '
        Me.MenuButtonItem4.FavVisible = False
        Me.MenuButtonItem4.Image = CType(resources.GetObject("MenuButtonItem4.Image"), System.Drawing.Image)
        Me.MenuButtonItem4.IsChange = False
        Me.MenuButtonItem4.IsFavVisible = True
        Me.MenuButtonItem4.Name = "MenuButtonItem4"
        Me.MenuButtonItem4.Size = New System.Drawing.Size(546, 22)
        Me.MenuButtonItem4.Text = "(7-6) خلاصه گزارش قطعي مشتركين"
        '
        'MenuButtonItem23
        '
        Me.MenuButtonItem23.FavVisible = False
        Me.MenuButtonItem23.Image = CType(resources.GetObject("MenuButtonItem23.Image"), System.Drawing.Image)
        Me.MenuButtonItem23.IsChange = False
        Me.MenuButtonItem23.IsFavVisible = True
        Me.MenuButtonItem23.Name = "MenuButtonItem23"
        Me.MenuButtonItem23.Size = New System.Drawing.Size(546, 22)
        Me.MenuButtonItem23.Text = "(7-8) خلاصه گزارش وضعيت فيدرهاي فشار متوسط به تفکيک پست فوق توزيع"
        '
        'MenuButtonItem42
        '
        Me.MenuButtonItem42.FavVisible = False
        Me.MenuButtonItem42.Image = CType(resources.GetObject("MenuButtonItem42.Image"), System.Drawing.Image)
        Me.MenuButtonItem42.IsChange = False
        Me.MenuButtonItem42.IsFavVisible = True
        Me.MenuButtonItem42.Name = "MenuButtonItem42"
        Me.MenuButtonItem42.Size = New System.Drawing.Size(546, 22)
        Me.MenuButtonItem42.Text = "(7-9) خلاصه گزارش وضعيت فيدرهاي فشار متوسط به ترتيب تعداد حادثه"
        '
        'MenuButtonItem43
        '
        Me.MenuButtonItem43.FavVisible = False
        Me.MenuButtonItem43.Image = CType(resources.GetObject("MenuButtonItem43.Image"), System.Drawing.Image)
        Me.MenuButtonItem43.IsChange = False
        Me.MenuButtonItem43.IsFavVisible = True
        Me.MenuButtonItem43.Name = "MenuButtonItem43"
        Me.MenuButtonItem43.Size = New System.Drawing.Size(546, 22)
        Me.MenuButtonItem43.Text = "(7-10) گزارش مقايسه‌اي قطعي فيدرهاي فشار متوسط"
        '
        'MenuButtonItem24
        '
        Me.MenuButtonItem24.FavVisible = False
        Me.MenuButtonItem24.Image = CType(resources.GetObject("MenuButtonItem24.Image"), System.Drawing.Image)
        Me.MenuButtonItem24.IsChange = False
        Me.MenuButtonItem24.IsFavVisible = False
        Me.MenuButtonItem24.Name = "MenuButtonItem24"
        Me.MenuButtonItem24.Size = New System.Drawing.Size(546, 22)
        Me.MenuButtonItem24.Text = "(7-11) خلاصه گزارش قطعيها و انرژي توزيع نشده"
        '
        'MenuButtonItem25
        '
        Me.MenuButtonItem25.FavVisible = False
        Me.MenuButtonItem25.Image = CType(resources.GetObject("MenuButtonItem25.Image"), System.Drawing.Image)
        Me.MenuButtonItem25.IsChange = False
        Me.MenuButtonItem25.IsFavVisible = True
        Me.MenuButtonItem25.Name = "MenuButtonItem25"
        Me.MenuButtonItem25.Size = New System.Drawing.Size(546, 22)
        Me.MenuButtonItem25.Text = "(7-12) خلاصه گزارش وضعيت پستهاي توزيع و شبکه‌هاي زير مجموعه آن"
        '
        'mnuRepDispaching
        '
        Me.mnuRepDispaching.FavVisible = False
        Me.mnuRepDispaching.Image = CType(resources.GetObject("mnuRepDispaching.Image"), System.Drawing.Image)
        Me.mnuRepDispaching.IsChange = False
        Me.mnuRepDispaching.IsFavVisible = True
        Me.mnuRepDispaching.Name = "mnuRepDispaching"
        Me.mnuRepDispaching.Size = New System.Drawing.Size(546, 22)
        Me.mnuRepDispaching.Text = "(7-13) خلاصه آمار ديسپاچينگ توزيع"
        '
        'mnuRepMPStatusChart
        '
        Me.mnuRepMPStatusChart.FavVisible = False
        Me.mnuRepMPStatusChart.Image = CType(resources.GetObject("mnuRepMPStatusChart.Image"), System.Drawing.Image)
        Me.mnuRepMPStatusChart.IsChange = False
        Me.mnuRepMPStatusChart.IsFavVisible = True
        Me.mnuRepMPStatusChart.Name = "mnuRepMPStatusChart"
        Me.mnuRepMPStatusChart.Size = New System.Drawing.Size(546, 22)
        Me.mnuRepMPStatusChart.Text = "(7-15) نمودار وضعيت فيدرهاي فشار متوسط"
        '
        'mnuMPFeederDisconnetInfo
        '
        Me.mnuMPFeederDisconnetInfo.FavVisible = False
        Me.mnuMPFeederDisconnetInfo.Image = CType(resources.GetObject("mnuMPFeederDisconnetInfo.Image"), System.Drawing.Image)
        Me.mnuMPFeederDisconnetInfo.IsChange = False
        Me.mnuMPFeederDisconnetInfo.IsFavVisible = True
        Me.mnuMPFeederDisconnetInfo.Name = "mnuMPFeederDisconnetInfo"
        Me.mnuMPFeederDisconnetInfo.Size = New System.Drawing.Size(546, 22)
        Me.mnuMPFeederDisconnetInfo.Text = "(7-26) گزارش ماهانه وضعيت فيدر هاي فشار متوسط"
        '
        'mnuRepDisconnectMonthlyCount
        '
        Me.mnuRepDisconnectMonthlyCount.FavVisible = False
        Me.mnuRepDisconnectMonthlyCount.Image = CType(resources.GetObject("mnuRepDisconnectMonthlyCount.Image"), System.Drawing.Image)
        Me.mnuRepDisconnectMonthlyCount.IsChange = False
        Me.mnuRepDisconnectMonthlyCount.IsFavVisible = True
        Me.mnuRepDisconnectMonthlyCount.Name = "mnuRepDisconnectMonthlyCount"
        Me.mnuRepDisconnectMonthlyCount.Size = New System.Drawing.Size(546, 22)
        Me.mnuRepDisconnectMonthlyCount.Text = "(7-16) گزارش ماه به ماه تعداد قطعيهاي فشار متوسط"
        '
        'mnuRepDisconnectMonthlyCountMode2
        '
        Me.mnuRepDisconnectMonthlyCountMode2.FavVisible = False
        Me.mnuRepDisconnectMonthlyCountMode2.Image = CType(resources.GetObject("mnuRepDisconnectMonthlyCountMode2.Image"), System.Drawing.Image)
        Me.mnuRepDisconnectMonthlyCountMode2.IsChange = False
        Me.mnuRepDisconnectMonthlyCountMode2.IsFavVisible = True
        Me.mnuRepDisconnectMonthlyCountMode2.Name = "mnuRepDisconnectMonthlyCountMode2"
        Me.mnuRepDisconnectMonthlyCountMode2.Size = New System.Drawing.Size(546, 22)
        Me.mnuRepDisconnectMonthlyCountMode2.Text = "(7-25) گزارش ماه به ماه تعداد قطعيهاي فشار متوسط (نمونه 2)"
        '
        'mnuRepDisconnectGroupSetGroup
        '
        Me.mnuRepDisconnectGroupSetGroup.FavVisible = False
        Me.mnuRepDisconnectGroupSetGroup.Image = CType(resources.GetObject("mnuRepDisconnectGroupSetGroup.Image"), System.Drawing.Image)
        Me.mnuRepDisconnectGroupSetGroup.IsChange = False
        Me.mnuRepDisconnectGroupSetGroup.IsFavVisible = True
        Me.mnuRepDisconnectGroupSetGroup.Name = "mnuRepDisconnectGroupSetGroup"
        Me.mnuRepDisconnectGroupSetGroup.Size = New System.Drawing.Size(546, 22)
        Me.mnuRepDisconnectGroupSetGroup.Text = "(7-17) گزارش عيوب شبكه فشار متوسط"
        '
        'mnuRepGroupTamir
        '
        Me.mnuRepGroupTamir.FavVisible = False
        Me.mnuRepGroupTamir.Image = CType(resources.GetObject("mnuRepGroupTamir.Image"), System.Drawing.Image)
        Me.mnuRepGroupTamir.IsChange = False
        Me.mnuRepGroupTamir.IsFavVisible = True
        Me.mnuRepGroupTamir.Name = "mnuRepGroupTamir"
        Me.mnuRepGroupTamir.Size = New System.Drawing.Size(546, 22)
        Me.mnuRepGroupTamir.Text = "(7-18) گزارش قطعي فيدرهاي فشار متوسط جهت انجام عمليات بابرنامه به تفکيک ناحيه"
        '
        'mnuRep5_1_1_Byerver121
        '
        Me.mnuRep5_1_1_Byerver121.FavVisible = False
        Me.mnuRep5_1_1_Byerver121.Image = CType(resources.GetObject("mnuRep5_1_1_Byerver121.Image"), System.Drawing.Image)
        Me.mnuRep5_1_1_Byerver121.IsChange = False
        Me.mnuRep5_1_1_Byerver121.IsFavVisible = True
        Me.mnuRep5_1_1_Byerver121.Name = "mnuRep5_1_1_Byerver121"
        Me.mnuRep5_1_1_Byerver121.Size = New System.Drawing.Size(546, 22)
        Me.mnuRep5_1_1_Byerver121.Text = "(7-19) خلاصه گزارش 1_1_5 به تفکيک امور"
        '
        'mnuRepDCRecloser
        '
        Me.mnuRepDCRecloser.FavVisible = False
        Me.mnuRepDCRecloser.Image = CType(resources.GetObject("mnuRepDCRecloser.Image"), System.Drawing.Image)
        Me.mnuRepDCRecloser.IsChange = False
        Me.mnuRepDCRecloser.IsFavVisible = True
        Me.mnuRepDCRecloser.Name = "mnuRepDCRecloser"
        Me.mnuRepDCRecloser.Size = New System.Drawing.Size(546, 22)
        Me.mnuRepDCRecloser.Text = "(7-20) خلاصه گزارش وضعيت فيدرهاي فشار متوسط بر اساس با‌برنامه، بي‌برنامه و ريکلوز" &
    "ري"
        '
        'mnuRep_7_21
        '
        Me.mnuRep_7_21.FavVisible = False
        Me.mnuRep_7_21.Image = CType(resources.GetObject("mnuRep_7_21.Image"), System.Drawing.Image)
        Me.mnuRep_7_21.IsChange = False
        Me.mnuRep_7_21.IsFavVisible = True
        Me.mnuRep_7_21.Name = "mnuRep_7_21"
        Me.mnuRep_7_21.Size = New System.Drawing.Size(546, 22)
        Me.mnuRep_7_21.Text = "(7-21) آمار تعداد و مقدار قطعي حفاظتي توزيع فيدرهاي فشار متوسط بر اساس ناحيه"
        '
        'mnuRep_7_22
        '
        Me.mnuRep_7_22.FavVisible = False
        Me.mnuRep_7_22.Image = CType(resources.GetObject("mnuRep_7_22.Image"), System.Drawing.Image)
        Me.mnuRep_7_22.IsChange = False
        Me.mnuRep_7_22.IsFavVisible = True
        Me.mnuRep_7_22.Name = "mnuRep_7_22"
        Me.mnuRep_7_22.Size = New System.Drawing.Size(546, 22)
        Me.mnuRep_7_22.Text = "(7-22) گزارش متوسط انرژي توزيع نشده به ازاء هر قطعي"
        '
        'mnuRep_7_23
        '
        Me.mnuRep_7_23.FavVisible = False
        Me.mnuRep_7_23.Image = CType(resources.GetObject("mnuRep_7_23.Image"), System.Drawing.Image)
        Me.mnuRep_7_23.IsChange = False
        Me.mnuRep_7_23.IsFavVisible = True
        Me.mnuRep_7_23.Name = "mnuRep_7_23"
        Me.mnuRep_7_23.Size = New System.Drawing.Size(546, 22)
        Me.mnuRep_7_23.Text = "(7-23) گزارش ماهانه متوسط انرژي توزيع نشده به تفکيک علل قطع"
        '
        'mnuRep_7_24
        '
        Me.mnuRep_7_24.FavVisible = False
        Me.mnuRep_7_24.Image = CType(resources.GetObject("mnuRep_7_24.Image"), System.Drawing.Image)
        Me.mnuRep_7_24.IsChange = False
        Me.mnuRep_7_24.IsFavVisible = True
        Me.mnuRep_7_24.Name = "mnuRep_7_24"
        Me.mnuRep_7_24.Size = New System.Drawing.Size(546, 22)
        Me.mnuRep_7_24.Text = "(7-24) نمودار تعداد قطع به تفکيک علل قطع"
        '
        'mnuRep_7_27
        '
        Me.mnuRep_7_27.FavVisible = False
        Me.mnuRep_7_27.Image = CType(resources.GetObject("mnuRep_7_27.Image"), System.Drawing.Image)
        Me.mnuRep_7_27.IsChange = False
        Me.mnuRep_7_27.IsFavVisible = True
        Me.mnuRep_7_27.Name = "mnuRep_7_27"
        Me.mnuRep_7_27.Size = New System.Drawing.Size(546, 22)
        Me.mnuRep_7_27.Text = "(7-27) گزارش خاموشی های کات اوت فیوز به تفکیک فاز ها در فشار متوسط"
        '
        'mnuRepLoading
        '
        Me.mnuRepLoading.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuButtonItem33, Me.mnuLPFedersLoad, Me.mnuMPFeederPeak, Me.mnuRepMPFeederPeakEnergyMonthly, Me.mnuRepLPPostNotLoad, Me.mnuRepMPPostMonthly, Me.mnuRepMPPostTransLoadMonthly, Me.mnuRepMPPostTransLoadDaily, Me.mnuRepMainPeak, Me.mnuLPPostLoadsStat, Me.mnuUtilizationFactor, Me.mnuRep_8_14, Me.mnuReport_8_15, Me.mnuReport_8_16, Me.MenuButtonItem66, Me.mnuReport_8_19, Me.mnuReport_8_20, Me.mnuRepMPPostTransLoadHourByHour, Me.mnuReport_8_22, Me.mnuReport_8_23, Me.mnuReport_8_24})
        Me.mnuRepLoading.Name = "mnuRepLoading"
        Me.mnuRepLoading.Size = New System.Drawing.Size(206, 22)
        Me.mnuRepLoading.Text = "(8) بارگيري"
        '
        'MenuButtonItem33
        '
        Me.MenuButtonItem33.FavVisible = False
        Me.MenuButtonItem33.Image = CType(resources.GetObject("MenuButtonItem33.Image"), System.Drawing.Image)
        Me.MenuButtonItem33.IsChange = False
        Me.MenuButtonItem33.IsFavVisible = True
        Me.MenuButtonItem33.Name = "MenuButtonItem33"
        Me.MenuButtonItem33.Size = New System.Drawing.Size(442, 22)
        Me.MenuButtonItem33.Text = "(8-1) گزارش تعادل بار "
        '
        'mnuLPFedersLoad
        '
        Me.mnuLPFedersLoad.FavVisible = False
        Me.mnuLPFedersLoad.Image = CType(resources.GetObject("mnuLPFedersLoad.Image"), System.Drawing.Image)
        Me.mnuLPFedersLoad.IsChange = False
        Me.mnuLPFedersLoad.IsFavVisible = True
        Me.mnuLPFedersLoad.Name = "mnuLPFedersLoad"
        Me.mnuLPFedersLoad.Size = New System.Drawing.Size(442, 22)
        Me.mnuLPFedersLoad.Text = "(8-2) گزارش بارگيري فيدرهاي فشار ضعيف"
        '
        'mnuMPFeederPeak
        '
        Me.mnuMPFeederPeak.FavVisible = False
        Me.mnuMPFeederPeak.Image = CType(resources.GetObject("mnuMPFeederPeak.Image"), System.Drawing.Image)
        Me.mnuMPFeederPeak.IsChange = False
        Me.mnuMPFeederPeak.IsFavVisible = True
        Me.mnuMPFeederPeak.Name = "mnuMPFeederPeak"
        Me.mnuMPFeederPeak.Size = New System.Drawing.Size(442, 22)
        Me.mnuMPFeederPeak.Text = "(8-3) گزارش پيک بار روزانه و ساعت به ساعت فيدرهاي فشار متوسط"
        '
        'mnuRepMPFeederPeakEnergyMonthly
        '
        Me.mnuRepMPFeederPeakEnergyMonthly.FavVisible = False
        Me.mnuRepMPFeederPeakEnergyMonthly.Image = CType(resources.GetObject("mnuRepMPFeederPeakEnergyMonthly.Image"), System.Drawing.Image)
        Me.mnuRepMPFeederPeakEnergyMonthly.IsChange = False
        Me.mnuRepMPFeederPeakEnergyMonthly.IsFavVisible = True
        Me.mnuRepMPFeederPeakEnergyMonthly.Name = "mnuRepMPFeederPeakEnergyMonthly"
        Me.mnuRepMPFeederPeakEnergyMonthly.Size = New System.Drawing.Size(442, 22)
        Me.mnuRepMPFeederPeakEnergyMonthly.Text = "(8-4) گزارش پيک بار و انرژي تحويلي ماهانه به فيدرها"
        '
        'mnuRepLPPostNotLoad
        '
        Me.mnuRepLPPostNotLoad.FavVisible = False
        Me.mnuRepLPPostNotLoad.Image = CType(resources.GetObject("mnuRepLPPostNotLoad.Image"), System.Drawing.Image)
        Me.mnuRepLPPostNotLoad.IsChange = False
        Me.mnuRepLPPostNotLoad.IsFavVisible = True
        Me.mnuRepLPPostNotLoad.Name = "mnuRepLPPostNotLoad"
        Me.mnuRepLPPostNotLoad.Size = New System.Drawing.Size(442, 22)
        Me.mnuRepLPPostNotLoad.Tag = "mnuRepLPPostNotLoad"
        Me.mnuRepLPPostNotLoad.Text = "(8-5) گزارش پست‌هاي توزيع بارگيري نشده"
        '
        'mnuRepMPPostMonthly
        '
        Me.mnuRepMPPostMonthly.FavVisible = False
        Me.mnuRepMPPostMonthly.Image = CType(resources.GetObject("mnuRepMPPostMonthly.Image"), System.Drawing.Image)
        Me.mnuRepMPPostMonthly.IsChange = False
        Me.mnuRepMPPostMonthly.IsFavVisible = True
        Me.mnuRepMPPostMonthly.Name = "mnuRepMPPostMonthly"
        Me.mnuRepMPPostMonthly.Size = New System.Drawing.Size(442, 22)
        Me.mnuRepMPPostMonthly.Text = "(8-6) گزارش انرژي تحويلي ماهانه به پست‌هاي فوق توزيع"
        '
        'mnuRepMPPostTransLoadMonthly
        '
        Me.mnuRepMPPostTransLoadMonthly.FavVisible = False
        Me.mnuRepMPPostTransLoadMonthly.Image = CType(resources.GetObject("mnuRepMPPostTransLoadMonthly.Image"), System.Drawing.Image)
        Me.mnuRepMPPostTransLoadMonthly.IsChange = False
        Me.mnuRepMPPostTransLoadMonthly.IsFavVisible = True
        Me.mnuRepMPPostTransLoadMonthly.Name = "mnuRepMPPostTransLoadMonthly"
        Me.mnuRepMPPostTransLoadMonthly.Size = New System.Drawing.Size(442, 22)
        Me.mnuRepMPPostTransLoadMonthly.Text = "(8-7) گزارش بارگيري ماهانه ترانسهاي فوق توزيع"
        '
        'mnuRepMPPostTransLoadDaily
        '
        Me.mnuRepMPPostTransLoadDaily.FavVisible = False
        Me.mnuRepMPPostTransLoadDaily.Image = CType(resources.GetObject("mnuRepMPPostTransLoadDaily.Image"), System.Drawing.Image)
        Me.mnuRepMPPostTransLoadDaily.IsChange = False
        Me.mnuRepMPPostTransLoadDaily.IsFavVisible = True
        Me.mnuRepMPPostTransLoadDaily.Name = "mnuRepMPPostTransLoadDaily"
        Me.mnuRepMPPostTransLoadDaily.Size = New System.Drawing.Size(442, 22)
        Me.mnuRepMPPostTransLoadDaily.Text = "(8-8) گزارش بارگيري روزانه ترانسهاي فوق توزيع"
        '
        'mnuRepMainPeak
        '
        Me.mnuRepMainPeak.FavVisible = False
        Me.mnuRepMainPeak.Image = CType(resources.GetObject("mnuRepMainPeak.Image"), System.Drawing.Image)
        Me.mnuRepMainPeak.IsChange = False
        Me.mnuRepMainPeak.IsFavVisible = True
        Me.mnuRepMainPeak.Name = "mnuRepMainPeak"
        Me.mnuRepMainPeak.Size = New System.Drawing.Size(442, 22)
        Me.mnuRepMainPeak.Text = "(8-9) گزارش پيک بار توزيع مرکز"
        '
        'mnuLPPostLoadsStat
        '
        Me.mnuLPPostLoadsStat.FavVisible = False
        Me.mnuLPPostLoadsStat.Image = CType(resources.GetObject("mnuLPPostLoadsStat.Image"), System.Drawing.Image)
        Me.mnuLPPostLoadsStat.IsChange = False
        Me.mnuLPPostLoadsStat.IsFavVisible = True
        Me.mnuLPPostLoadsStat.Name = "mnuLPPostLoadsStat"
        Me.mnuLPPostLoadsStat.Size = New System.Drawing.Size(442, 22)
        Me.mnuLPPostLoadsStat.Text = "(8-10) گزارش آناليز بارگيري پست‌ها توزيع عمومي"
        '
        'mnuUtilizationFactor
        '
        Me.mnuUtilizationFactor.FavVisible = False
        Me.mnuUtilizationFactor.Image = CType(resources.GetObject("mnuUtilizationFactor.Image"), System.Drawing.Image)
        Me.mnuUtilizationFactor.IsChange = False
        Me.mnuUtilizationFactor.IsFavVisible = True
        Me.mnuUtilizationFactor.Name = "mnuUtilizationFactor"
        Me.mnuUtilizationFactor.Size = New System.Drawing.Size(442, 22)
        Me.mnuUtilizationFactor.Text = "(8-11) گزارش ضريب بهره‌برداري"
        '
        'mnuRep_8_14
        '
        Me.mnuRep_8_14.FavVisible = False
        Me.mnuRep_8_14.Image = CType(resources.GetObject("mnuRep_8_14.Image"), System.Drawing.Image)
        Me.mnuRep_8_14.IsChange = False
        Me.mnuRep_8_14.IsFavVisible = True
        Me.mnuRep_8_14.Name = "mnuRep_8_14"
        Me.mnuRep_8_14.Size = New System.Drawing.Size(442, 22)
        Me.mnuRep_8_14.Text = "(8-14) گزارش پيک بارهاي روز و شب ثبت شده فيدرهاي فشار متوسط"
        '
        'mnuReport_8_15
        '
        Me.mnuReport_8_15.FavVisible = False
        Me.mnuReport_8_15.Image = CType(resources.GetObject("mnuReport_8_15.Image"), System.Drawing.Image)
        Me.mnuReport_8_15.IsChange = False
        Me.mnuReport_8_15.IsFavVisible = True
        Me.mnuReport_8_15.Name = "mnuReport_8_15"
        Me.mnuReport_8_15.Size = New System.Drawing.Size(442, 22)
        Me.mnuReport_8_15.Text = "(8-15) گزارش پيک بار همزمان فيدرهاي فشار متوسط از روي بار ساعت به ساعت"
        '
        'mnuReport_8_16
        '
        Me.mnuReport_8_16.FavVisible = False
        Me.mnuReport_8_16.Image = CType(resources.GetObject("mnuReport_8_16.Image"), System.Drawing.Image)
        Me.mnuReport_8_16.IsChange = False
        Me.mnuReport_8_16.IsFavVisible = True
        Me.mnuReport_8_16.Name = "mnuReport_8_16"
        Me.mnuReport_8_16.Size = New System.Drawing.Size(442, 22)
        Me.mnuReport_8_16.Text = "(8-16) گزارش پيک بار همزمان و غير همزمان فيدرهاي فشار متوسط"
        '
        'MenuButtonItem66
        '
        Me.MenuButtonItem66.FavVisible = False
        Me.MenuButtonItem66.Image = CType(resources.GetObject("MenuButtonItem66.Image"), System.Drawing.Image)
        Me.MenuButtonItem66.IsChange = False
        Me.MenuButtonItem66.IsFavVisible = True
        Me.MenuButtonItem66.Name = "MenuButtonItem66"
        Me.MenuButtonItem66.Size = New System.Drawing.Size(442, 22)
        Me.MenuButtonItem66.Text = "(8-17) گزارش نامتعادلي بين فازها در ترانسفورماتورهاي توزيع"
        '
        'mnuReport_8_19
        '
        Me.mnuReport_8_19.FavVisible = False
        Me.mnuReport_8_19.Image = CType(resources.GetObject("mnuReport_8_19.Image"), System.Drawing.Image)
        Me.mnuReport_8_19.IsChange = False
        Me.mnuReport_8_19.IsFavVisible = True
        Me.mnuReport_8_19.Name = "mnuReport_8_19"
        Me.mnuReport_8_19.Size = New System.Drawing.Size(442, 22)
        Me.mnuReport_8_19.Text = "(8-19) گزارش قدرت سرانه خانوار روستایی"
        Me.mnuReport_8_19.Visible = False
        '
        'mnuReport_8_20
        '
        Me.mnuReport_8_20.FavVisible = False
        Me.mnuReport_8_20.Image = CType(resources.GetObject("mnuReport_8_20.Image"), System.Drawing.Image)
        Me.mnuReport_8_20.IsChange = False
        Me.mnuReport_8_20.IsFavVisible = True
        Me.mnuReport_8_20.Name = "mnuReport_8_20"
        Me.mnuReport_8_20.Size = New System.Drawing.Size(442, 22)
        Me.mnuReport_8_20.Text = "(8-20) گزارش ولتاز انتهای خط فیدرهای فشار متوسط"
        '
        'mnuRepMPPostTransLoadHourByHour
        '
        Me.mnuRepMPPostTransLoadHourByHour.FavVisible = False
        Me.mnuRepMPPostTransLoadHourByHour.Image = CType(resources.GetObject("mnuRepMPPostTransLoadHourByHour.Image"), System.Drawing.Image)
        Me.mnuRepMPPostTransLoadHourByHour.IsChange = False
        Me.mnuRepMPPostTransLoadHourByHour.IsFavVisible = True
        Me.mnuRepMPPostTransLoadHourByHour.Name = "mnuRepMPPostTransLoadHourByHour"
        Me.mnuRepMPPostTransLoadHourByHour.Size = New System.Drawing.Size(442, 22)
        Me.mnuRepMPPostTransLoadHourByHour.Text = "(8-21) گزارش بارگيري ساعت به ساعت ترانسهاي فوق توزيع"
        '
        'mnuReport_8_22
        '
        Me.mnuReport_8_22.FavVisible = False
        Me.mnuReport_8_22.Image = CType(resources.GetObject("mnuReport_8_22.Image"), System.Drawing.Image)
        Me.mnuReport_8_22.IsChange = False
        Me.mnuReport_8_22.IsFavVisible = True
        Me.mnuReport_8_22.Name = "mnuReport_8_22"
        Me.mnuReport_8_22.Size = New System.Drawing.Size(442, 22)
        Me.mnuReport_8_22.Tag = "mnuReport_8_22"
        Me.mnuReport_8_22.Text = "(8-22) گزارش پست‌هاي توزيع ولتاژ گیری نشده"
        '
        'mnuReport_8_23
        '
        Me.mnuReport_8_23.FavVisible = False
        Me.mnuReport_8_23.Image = CType(resources.GetObject("mnuReport_8_23.Image"), System.Drawing.Image)
        Me.mnuReport_8_23.IsChange = False
        Me.mnuReport_8_23.IsFavVisible = True
        Me.mnuReport_8_23.Name = "mnuReport_8_23"
        Me.mnuReport_8_23.Size = New System.Drawing.Size(442, 22)
        Me.mnuReport_8_23.Tag = "mnuReport_8_23"
        Me.mnuReport_8_23.Text = "(8-23) گزارش فيدرهای فشار ضعيف ولتاژ گیری نشده"
        '
        'mnuReport_8_24
        '
        Me.mnuReport_8_24.FavVisible = False
        Me.mnuReport_8_24.Image = CType(resources.GetObject("mnuReport_8_24.Image"), System.Drawing.Image)
        Me.mnuReport_8_24.IsChange = False
        Me.mnuReport_8_24.IsFavVisible = True
        Me.mnuReport_8_24.Name = "mnuReport_8_24"
        Me.mnuReport_8_24.Size = New System.Drawing.Size(442, 22)
        Me.mnuReport_8_24.Tag = "mnuReport_8_24"
        Me.mnuReport_8_24.Text = "(8-24) گزارش بار ساعت به ساعت فیدرها در ساعت خاص"
        '
        'mnuRepEarting
        '
        Me.mnuRepEarting.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuLPPostEarths, Me.mnuReport_8_13, Me.mnuReportLPPostEarth})
        Me.mnuRepEarting.Name = "mnuRepEarting"
        Me.mnuRepEarting.Size = New System.Drawing.Size(206, 22)
        Me.mnuRepEarting.Text = "(15) ارتينگ"
        '
        'mnuLPPostEarths
        '
        Me.mnuLPPostEarths.FavVisible = False
        Me.mnuLPPostEarths.Image = CType(resources.GetObject("mnuLPPostEarths.Image"), System.Drawing.Image)
        Me.mnuLPPostEarths.IsChange = False
        Me.mnuLPPostEarths.IsFavVisible = True
        Me.mnuLPPostEarths.Name = "mnuLPPostEarths"
        Me.mnuLPPostEarths.Size = New System.Drawing.Size(248, 22)
        Me.mnuLPPostEarths.Text = "(15-1) گزارش وضعيت ارت پست‌ها"
        '
        'mnuReport_8_13
        '
        Me.mnuReport_8_13.FavVisible = False
        Me.mnuReport_8_13.Image = CType(resources.GetObject("mnuReport_8_13.Image"), System.Drawing.Image)
        Me.mnuReport_8_13.IsChange = False
        Me.mnuReport_8_13.IsFavVisible = True
        Me.mnuReport_8_13.Name = "mnuReport_8_13"
        Me.mnuReport_8_13.Size = New System.Drawing.Size(248, 22)
        Me.mnuReport_8_13.Text = "(15-2) گزارش نتايج ارت‌گيري پست‌ها"
        '
        'mnuReportLPPostEarth
        '
        Me.mnuReportLPPostEarth.FavVisible = False
        Me.mnuReportLPPostEarth.Image = CType(resources.GetObject("mnuReportLPPostEarth.Image"), System.Drawing.Image)
        Me.mnuReportLPPostEarth.IsChange = False
        Me.mnuReportLPPostEarth.IsFavVisible = True
        Me.mnuReportLPPostEarth.Name = "mnuReportLPPostEarth"
        Me.mnuReportLPPostEarth.Size = New System.Drawing.Size(248, 22)
        Me.mnuReportLPPostEarth.Text = "(15-3) گزارش ارت گيري پستهاي توزيع"
        '
        'mnuControlReports
        '
        Me.mnuControlReports.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuItem111, Me.MenuItem9_12, Me.MenuItem112, Me.MenuItem113, Me.mnuRepCheckLies, Me.rptRep_ListenRequestCallList, Me.rptRep_ListenUserCallCount, Me.rptRep_ListenUserCallCount2, Me.mnuRepUserLogin, Me.mnuRepuserAccess, Me.mnuRepEkipPerformance, Me.mnuRepDelayControl, Me.mnuRepDeleteRequest, Me.mnuReportAVL, Me.mnuReport_9_20, Me.mnuRepSumEkipPerformance, Me.ToolStripSeparator65, Me.mnuReport_9_16, Me.mnuReport_9_17, Me.mnuRepQuestionnaire, Me.mnuRepQuestionnaire2})
        Me.mnuControlReports.Name = "mnuControlReports"
        Me.mnuControlReports.Size = New System.Drawing.Size(206, 22)
        Me.mnuControlReports.Text = "(9) گزارشات &كنترلي   "
        '
        'MenuItem111
        '
        Me.MenuItem111.FavVisible = False
        Me.MenuItem111.Image = CType(resources.GetObject("MenuItem111.Image"), System.Drawing.Image)
        Me.MenuItem111.IsChange = False
        Me.MenuItem111.IsFavVisible = True
        Me.MenuItem111.Name = "MenuItem111"
        Me.MenuItem111.Size = New System.Drawing.Size(530, 22)
        Me.MenuItem111.Text = "(9-1) عملكرد ا&پراتورهاي ثبت كننده اطلاعات"
        '
        'MenuItem9_12
        '
        Me.MenuItem9_12.FavVisible = False
        Me.MenuItem9_12.Image = CType(resources.GetObject("MenuItem9_12.Image"), System.Drawing.Image)
        Me.MenuItem9_12.IsChange = False
        Me.MenuItem9_12.IsFavVisible = True
        Me.MenuItem9_12.Name = "MenuItem9_12"
        Me.MenuItem9_12.Size = New System.Drawing.Size(530, 22)
        Me.MenuItem9_12.Text = "(9-12) عملکرد اپراتورهاي تکميل کننده اطلاعات"
        '
        'MenuItem112
        '
        Me.MenuItem112.FavVisible = False
        Me.MenuItem112.Image = CType(resources.GetObject("MenuItem112.Image"), System.Drawing.Image)
        Me.MenuItem112.IsChange = False
        Me.MenuItem112.IsFavVisible = True
        Me.MenuItem112.Name = "MenuItem112"
        Me.MenuItem112.Size = New System.Drawing.Size(530, 22)
        Me.MenuItem112.Text = "(9-2) عملكرد &ناحيه در مرتفع نمودن مشكلات"
        '
        'MenuItem113
        '
        Me.MenuItem113.FavVisible = False
        Me.MenuItem113.Image = CType(resources.GetObject("MenuItem113.Image"), System.Drawing.Image)
        Me.MenuItem113.IsChange = False
        Me.MenuItem113.IsFavVisible = True
        Me.MenuItem113.Name = "MenuItem113"
        Me.MenuItem113.Size = New System.Drawing.Size(530, 22)
        Me.MenuItem113.Text = "(9-3) عملكرد ا&كيپ هاي اعزامي"
        '
        'mnuRepCheckLies
        '
        Me.mnuRepCheckLies.FavVisible = False
        Me.mnuRepCheckLies.Image = CType(resources.GetObject("mnuRepCheckLies.Image"), System.Drawing.Image)
        Me.mnuRepCheckLies.IsChange = False
        Me.mnuRepCheckLies.IsFavVisible = True
        Me.mnuRepCheckLies.Name = "mnuRepCheckLies"
        Me.mnuRepCheckLies.Size = New System.Drawing.Size(530, 22)
        Me.mnuRepCheckLies.Text = "(9-4) کنترل پرونده‌هاي تکراري"
        '
        'rptRep_ListenRequestCallList
        '
        Me.rptRep_ListenRequestCallList.FavVisible = False
        Me.rptRep_ListenRequestCallList.Image = CType(resources.GetObject("rptRep_ListenRequestCallList.Image"), System.Drawing.Image)
        Me.rptRep_ListenRequestCallList.IsChange = False
        Me.rptRep_ListenRequestCallList.IsFavVisible = True
        Me.rptRep_ListenRequestCallList.Name = "rptRep_ListenRequestCallList"
        Me.rptRep_ListenRequestCallList.Size = New System.Drawing.Size(530, 22)
        Me.rptRep_ListenRequestCallList.Text = "(9-5) فهرست پرونده‌هايي که کاربران به مکالمه آنها گوش داده‌اند"
        '
        'rptRep_ListenUserCallCount
        '
        Me.rptRep_ListenUserCallCount.FavVisible = False
        Me.rptRep_ListenUserCallCount.Image = CType(resources.GetObject("rptRep_ListenUserCallCount.Image"), System.Drawing.Image)
        Me.rptRep_ListenUserCallCount.IsChange = False
        Me.rptRep_ListenUserCallCount.IsFavVisible = True
        Me.rptRep_ListenUserCallCount.Name = "rptRep_ListenUserCallCount"
        Me.rptRep_ListenUserCallCount.Size = New System.Drawing.Size(530, 22)
        Me.rptRep_ListenUserCallCount.Text = "(9-6) آمار تعداد گوش دادن به مکالمه‌ها توسط کاربران"
        '
        'rptRep_ListenUserCallCount2
        '
        Me.rptRep_ListenUserCallCount2.FavVisible = False
        Me.rptRep_ListenUserCallCount2.Image = CType(resources.GetObject("rptRep_ListenUserCallCount2.Image"), System.Drawing.Image)
        Me.rptRep_ListenUserCallCount2.IsChange = False
        Me.rptRep_ListenUserCallCount2.IsFavVisible = True
        Me.rptRep_ListenUserCallCount2.Name = "rptRep_ListenUserCallCount2"
        Me.rptRep_ListenUserCallCount2.Size = New System.Drawing.Size(530, 22)
        Me.rptRep_ListenUserCallCount2.Text = "(9-11) آمار تعداد گوش دادن به مکالمه‌ها توسط کاربران نمونه 2"
        '
        'mnuRepUserLogin
        '
        Me.mnuRepUserLogin.FavVisible = False
        Me.mnuRepUserLogin.Image = CType(resources.GetObject("mnuRepUserLogin.Image"), System.Drawing.Image)
        Me.mnuRepUserLogin.IsChange = False
        Me.mnuRepUserLogin.IsFavVisible = True
        Me.mnuRepUserLogin.Name = "mnuRepUserLogin"
        Me.mnuRepUserLogin.Size = New System.Drawing.Size(530, 22)
        Me.mnuRepUserLogin.Text = "(9-7) گزارش ورود و خروج کاربران به سامانه"
        '
        'mnuRepuserAccess
        '
        Me.mnuRepuserAccess.FavVisible = False
        Me.mnuRepuserAccess.Image = CType(resources.GetObject("mnuRepuserAccess.Image"), System.Drawing.Image)
        Me.mnuRepuserAccess.IsChange = False
        Me.mnuRepuserAccess.IsFavVisible = True
        Me.mnuRepuserAccess.Name = "mnuRepuserAccess"
        Me.mnuRepuserAccess.Size = New System.Drawing.Size(530, 22)
        Me.mnuRepuserAccess.Text = "(9-8) گزارش سطوح دسترسي کاربران سامانه"
        '
        'mnuRepEkipPerformance
        '
        Me.mnuRepEkipPerformance.FavVisible = False
        Me.mnuRepEkipPerformance.Image = CType(resources.GetObject("mnuRepEkipPerformance.Image"), System.Drawing.Image)
        Me.mnuRepEkipPerformance.IsChange = False
        Me.mnuRepEkipPerformance.IsFavVisible = True
        Me.mnuRepEkipPerformance.Name = "mnuRepEkipPerformance"
        Me.mnuRepEkipPerformance.Size = New System.Drawing.Size(530, 22)
        Me.mnuRepEkipPerformance.Tag = "mnuRepEkipPerformance"
        Me.mnuRepEkipPerformance.Text = "(9-9) گزارش کارکرد اکيپ‌ها"
        '
        'mnuRepDelayControl
        '
        Me.mnuRepDelayControl.FavVisible = False
        Me.mnuRepDelayControl.Image = CType(resources.GetObject("mnuRepDelayControl.Image"), System.Drawing.Image)
        Me.mnuRepDelayControl.IsChange = False
        Me.mnuRepDelayControl.IsFavVisible = True
        Me.mnuRepDelayControl.Name = "mnuRepDelayControl"
        Me.mnuRepDelayControl.Size = New System.Drawing.Size(530, 22)
        Me.mnuRepDelayControl.Text = "(9-10) کنترل تأخير در تکميل پرونده‌ها توسط اپراتورها"
        '
        'mnuRepDeleteRequest
        '
        Me.mnuRepDeleteRequest.FavVisible = False
        Me.mnuRepDeleteRequest.Image = CType(resources.GetObject("mnuRepDeleteRequest.Image"), System.Drawing.Image)
        Me.mnuRepDeleteRequest.IsChange = False
        Me.mnuRepDeleteRequest.IsFavVisible = True
        Me.mnuRepDeleteRequest.Name = "mnuRepDeleteRequest"
        Me.mnuRepDeleteRequest.Size = New System.Drawing.Size(530, 22)
        Me.mnuRepDeleteRequest.Text = "(9-13) گزارش پرونده هاي حذف شده"
        '
        'mnuReportAVL
        '
        Me.mnuReportAVL.FavVisible = False
        Me.mnuReportAVL.Image = CType(resources.GetObject("mnuReportAVL.Image"), System.Drawing.Image)
        Me.mnuReportAVL.IsChange = False
        Me.mnuReportAVL.IsFavVisible = True
        Me.mnuReportAVL.Name = "mnuReportAVL"
        Me.mnuReportAVL.Size = New System.Drawing.Size(530, 22)
        Me.mnuReportAVL.Tag = "mnuReportAVL"
        Me.mnuReportAVL.Text = "(9-14) گزارش آماری عملکرد خودروهای اعزامی از طريق AVL"
        '
        'mnuReport_9_20
        '
        Me.mnuReport_9_20.FavVisible = False
        Me.mnuReport_9_20.Image = CType(resources.GetObject("mnuReport_9_20.Image"), System.Drawing.Image)
        Me.mnuReport_9_20.IsChange = False
        Me.mnuReport_9_20.IsFavVisible = True
        Me.mnuReport_9_20.Name = "mnuReport_9_20"
        Me.mnuReport_9_20.Size = New System.Drawing.Size(530, 22)
        Me.mnuReport_9_20.Tag = "mnuReport_9_20"
        Me.mnuReport_9_20.Text = "(9-20) گزارش عملکرد خودروهای اعزامی "
        '
        'mnuRepSumEkipPerformance
        '
        Me.mnuRepSumEkipPerformance.FavVisible = False
        Me.mnuRepSumEkipPerformance.Image = CType(resources.GetObject("mnuRepSumEkipPerformance.Image"), System.Drawing.Image)
        Me.mnuRepSumEkipPerformance.IsChange = False
        Me.mnuRepSumEkipPerformance.IsFavVisible = True
        Me.mnuRepSumEkipPerformance.Name = "mnuRepSumEkipPerformance"
        Me.mnuRepSumEkipPerformance.Size = New System.Drawing.Size(530, 22)
        Me.mnuRepSumEkipPerformance.Tag = "mnuRepSumEkipPerformance"
        Me.mnuRepSumEkipPerformance.Text = "(9-15) گزارش تجمعی کارکرد اکيپ‌ها"
        '
        'ToolStripSeparator65
        '
        Me.ToolStripSeparator65.Name = "ToolStripSeparator65"
        Me.ToolStripSeparator65.Size = New System.Drawing.Size(527, 6)
        '
        'mnuReport_9_16
        '
        Me.mnuReport_9_16.FavVisible = False
        Me.mnuReport_9_16.Image = CType(resources.GetObject("mnuReport_9_16.Image"), System.Drawing.Image)
        Me.mnuReport_9_16.IsChange = False
        Me.mnuReport_9_16.IsFavVisible = True
        Me.mnuReport_9_16.Name = "mnuReport_9_16"
        Me.mnuReport_9_16.Size = New System.Drawing.Size(530, 22)
        Me.mnuReport_9_16.Text = "(9-16) گزارش کنترل تطابق پرونده ثبت شده با نظر مشترک"
        '
        'mnuReport_9_17
        '
        Me.mnuReport_9_17.FavVisible = False
        Me.mnuReport_9_17.Image = CType(resources.GetObject("mnuReport_9_17.Image"), System.Drawing.Image)
        Me.mnuReport_9_17.IsChange = False
        Me.mnuReport_9_17.IsFavVisible = True
        Me.mnuReport_9_17.Name = "mnuReport_9_17"
        Me.mnuReport_9_17.Size = New System.Drawing.Size(530, 22)
        Me.mnuReport_9_17.Text = "(9-17) گزارش امتيازدهي عملکرد روزانه اکيپ هاي شيفت عمليات و اتفاقات شبکه"
        '
        'mnuRepQuestionnaire
        '
        Me.mnuRepQuestionnaire.FavVisible = False
        Me.mnuRepQuestionnaire.Image = CType(resources.GetObject("mnuRepQuestionnaire.Image"), System.Drawing.Image)
        Me.mnuRepQuestionnaire.IsChange = False
        Me.mnuRepQuestionnaire.IsFavVisible = True
        Me.mnuRepQuestionnaire.Name = "mnuRepQuestionnaire"
        Me.mnuRepQuestionnaire.Size = New System.Drawing.Size(530, 22)
        Me.mnuRepQuestionnaire.Text = "(9-18) گزارش کنترل عملکرد اداره اتفاقات و عمليات ناحيه"
        '
        'mnuRepQuestionnaire2
        '
        Me.mnuRepQuestionnaire2.FavVisible = False
        Me.mnuRepQuestionnaire2.Image = CType(resources.GetObject("mnuRepQuestionnaire2.Image"), System.Drawing.Image)
        Me.mnuRepQuestionnaire2.IsChange = False
        Me.mnuRepQuestionnaire2.IsFavVisible = True
        Me.mnuRepQuestionnaire2.Name = "mnuRepQuestionnaire2"
        Me.mnuRepQuestionnaire2.Size = New System.Drawing.Size(530, 22)
        Me.mnuRepQuestionnaire2.Text = "(9-19) گزارش کنترل عملکرد اداره اتفاقات و عمليات ناحيه در خصوص صحت بار، زمان و عل" &
    "ت عدم انجام"
        '
        'mnuRepOtherReport
        '
        Me.mnuRepOtherReport.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuButtonItem28, Me.mnuReportHourByHour, Me.mnuRepSubscribers, Me.mnuRepPowerIndex, Me.mnuRepPowerIndex_Mode2, Me.munRepDisconnectGroupSet, Me.mnuRepAllDisconnectFeeder, Me.mnuReportDisconnectFeeder_Mode2, Me.mnuRepDCPServer121, Me.MenuButtonItem48, Me.mnuRepWantedInfo, Me.mnuRep_LPAndSerghati, Me.mnuSerghatPart, Me.mnuThriftPower, Me.mnuReport_10_31, Me.mnuReport_10_15, Me.mnuReport_10_17, Me.mnuReport_10_25, Me.mnuReport_10_26, Me.mnuReport_10_19, Me.mnuRep_10_21, Me.mnuRep_10_23, Me.mnuRep_10_27, Me.mnuRep_10_28, Me.mnuRep_10_29, Me.mnuRep_10_30})
        Me.mnuRepOtherReport.Name = "mnuRepOtherReport"
        Me.mnuRepOtherReport.Size = New System.Drawing.Size(206, 22)
        Me.mnuRepOtherReport.Text = "(10) گزارش‌هاي ديگر"
        '
        'MenuButtonItem28
        '
        Me.MenuButtonItem28.FavVisible = False
        Me.MenuButtonItem28.Image = CType(resources.GetObject("MenuButtonItem28.Image"), System.Drawing.Image)
        Me.MenuButtonItem28.IsChange = False
        Me.MenuButtonItem28.IsFavVisible = True
        Me.MenuButtonItem28.Name = "MenuButtonItem28"
        Me.MenuButtonItem28.Size = New System.Drawing.Size(460, 22)
        Me.MenuButtonItem28.Text = "(10-2) گزارش روزانه ارجاعات"
        '
        'mnuReportHourByHour
        '
        Me.mnuReportHourByHour.FavVisible = False
        Me.mnuReportHourByHour.Image = CType(resources.GetObject("mnuReportHourByHour.Image"), System.Drawing.Image)
        Me.mnuReportHourByHour.IsChange = False
        Me.mnuReportHourByHour.IsFavVisible = True
        Me.mnuReportHourByHour.Name = "mnuReportHourByHour"
        Me.mnuReportHourByHour.Size = New System.Drawing.Size(460, 22)
        Me.mnuReportHourByHour.Text = "(10-3) گزارش انرژي توزيع نشده ساعت به ساعت"
        '
        'mnuRepSubscribers
        '
        Me.mnuRepSubscribers.FavVisible = False
        Me.mnuRepSubscribers.Image = CType(resources.GetObject("mnuRepSubscribers.Image"), System.Drawing.Image)
        Me.mnuRepSubscribers.IsChange = False
        Me.mnuRepSubscribers.IsFavVisible = True
        Me.mnuRepSubscribers.Name = "mnuRepSubscribers"
        Me.mnuRepSubscribers.Size = New System.Drawing.Size(460, 22)
        Me.mnuRepSubscribers.Text = "(10-5) گزارش تعداد مشترکين ناحيه‌ها"
        '
        'mnuRepPowerIndex
        '
        Me.mnuRepPowerIndex.FavVisible = False
        Me.mnuRepPowerIndex.Image = CType(resources.GetObject("mnuRepPowerIndex.Image"), System.Drawing.Image)
        Me.mnuRepPowerIndex.IsChange = False
        Me.mnuRepPowerIndex.IsFavVisible = True
        Me.mnuRepPowerIndex.Name = "mnuRepPowerIndex"
        Me.mnuRepPowerIndex.Size = New System.Drawing.Size(460, 22)
        Me.mnuRepPowerIndex.Text = "(10-6) گزارش شاخصهاي انرژي توزيع نشده امورها"
        '
        'mnuRepPowerIndex_Mode2
        '
        Me.mnuRepPowerIndex_Mode2.FavVisible = False
        Me.mnuRepPowerIndex_Mode2.Image = CType(resources.GetObject("mnuRepPowerIndex_Mode2.Image"), System.Drawing.Image)
        Me.mnuRepPowerIndex_Mode2.IsChange = False
        Me.mnuRepPowerIndex_Mode2.IsFavVisible = True
        Me.mnuRepPowerIndex_Mode2.Name = "mnuRepPowerIndex_Mode2"
        Me.mnuRepPowerIndex_Mode2.Size = New System.Drawing.Size(460, 22)
        Me.mnuRepPowerIndex_Mode2.Text = "(10-16) گزارش شاخصهاي انرژي توزيع نشده فشار متوسط نمونه 2"
        '
        'munRepDisconnectGroupSet
        '
        Me.munRepDisconnectGroupSet.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuRepGroupSetGroup, Me.munRep_10_15_1, Me.munRep_10_7_3})
        Me.munRepDisconnectGroupSet.Name = "munRepDisconnectGroupSet"
        Me.munRepDisconnectGroupSet.Size = New System.Drawing.Size(460, 22)
        Me.munRepDisconnectGroupSet.Text = "(10-7) گزارشات علل قطع"
        '
        'mnuRepGroupSetGroup
        '
        Me.mnuRepGroupSetGroup.FavVisible = False
        Me.mnuRepGroupSetGroup.Image = CType(resources.GetObject("mnuRepGroupSetGroup.Image"), System.Drawing.Image)
        Me.mnuRepGroupSetGroup.IsChange = False
        Me.mnuRepGroupSetGroup.IsFavVisible = True
        Me.mnuRepGroupSetGroup.Name = "mnuRepGroupSetGroup"
        Me.mnuRepGroupSetGroup.Size = New System.Drawing.Size(381, 22)
        Me.mnuRepGroupSetGroup.Text = "(10-7-1) گزارش قطع و وصل فيدرها با علت خاص"
        '
        'munRep_10_15_1
        '
        Me.munRep_10_15_1.FavVisible = False
        Me.munRep_10_15_1.Image = CType(resources.GetObject("munRep_10_15_1.Image"), System.Drawing.Image)
        Me.munRep_10_15_1.IsChange = False
        Me.munRep_10_15_1.IsFavVisible = True
        Me.munRep_10_15_1.Name = "munRep_10_15_1"
        Me.munRep_10_15_1.Size = New System.Drawing.Size(381, 22)
        Me.munRep_10_15_1.Tag = "10_7_2"
        Me.munRep_10_15_1.Text = "(10-7-2) گزارش خاموشي‌هاي فيدرهاي بحراني به تفکيک علل قطع"
        '
        'munRep_10_7_3
        '
        Me.munRep_10_7_3.FavVisible = False
        Me.munRep_10_7_3.Image = CType(resources.GetObject("munRep_10_7_3.Image"), System.Drawing.Image)
        Me.munRep_10_7_3.IsChange = False
        Me.munRep_10_7_3.IsFavVisible = True
        Me.munRep_10_7_3.Name = "munRep_10_7_3"
        Me.munRep_10_7_3.Size = New System.Drawing.Size(381, 22)
        Me.munRep_10_7_3.Tag = "10_7_3"
        Me.munRep_10_7_3.Text = "(10-7-3) گزارش آماری خاموشي‌هاي فيدرهاي بحراني "
        '
        'mnuRepAllDisconnectFeeder
        '
        Me.mnuRepAllDisconnectFeeder.FavVisible = False
        Me.mnuRepAllDisconnectFeeder.Image = CType(resources.GetObject("mnuRepAllDisconnectFeeder.Image"), System.Drawing.Image)
        Me.mnuRepAllDisconnectFeeder.IsChange = False
        Me.mnuRepAllDisconnectFeeder.IsFavVisible = True
        Me.mnuRepAllDisconnectFeeder.Name = "mnuRepAllDisconnectFeeder"
        Me.mnuRepAllDisconnectFeeder.Size = New System.Drawing.Size(460, 22)
        Me.mnuRepAllDisconnectFeeder.Text = "(10-8) گزاش آماري قطع و وصل فيدرها"
        '
        'mnuReportDisconnectFeeder_Mode2
        '
        Me.mnuReportDisconnectFeeder_Mode2.FavVisible = False
        Me.mnuReportDisconnectFeeder_Mode2.Image = CType(resources.GetObject("mnuReportDisconnectFeeder_Mode2.Image"), System.Drawing.Image)
        Me.mnuReportDisconnectFeeder_Mode2.IsChange = False
        Me.mnuReportDisconnectFeeder_Mode2.IsFavVisible = True
        Me.mnuReportDisconnectFeeder_Mode2.Name = "mnuReportDisconnectFeeder_Mode2"
        Me.mnuReportDisconnectFeeder_Mode2.Size = New System.Drawing.Size(460, 22)
        Me.mnuReportDisconnectFeeder_Mode2.Text = "(10-20) گزارش آماري قطع و وصل فيدرها نمونه دو"
        '
        'mnuRepDCPServer121
        '
        Me.mnuRepDCPServer121.FavVisible = False
        Me.mnuRepDCPServer121.Image = CType(resources.GetObject("mnuRepDCPServer121.Image"), System.Drawing.Image)
        Me.mnuRepDCPServer121.IsChange = False
        Me.mnuRepDCPServer121.IsFavVisible = True
        Me.mnuRepDCPServer121.Name = "mnuRepDCPServer121"
        Me.mnuRepDCPServer121.Size = New System.Drawing.Size(460, 22)
        Me.mnuRepDCPServer121.Text = "(10-9) گزارش انرژي توزيع نشده به تفکيک امورها"
        '
        'MenuButtonItem48
        '
        Me.MenuButtonItem48.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuRepUndoneReason, Me.mnuRepUndoneReasonArea})
        Me.MenuButtonItem48.Name = "MenuButtonItem48"
        Me.MenuButtonItem48.Size = New System.Drawing.Size(460, 22)
        Me.MenuButtonItem48.Text = "(10-10) گزارشات عدم انجام"
        '
        'mnuRepUndoneReason
        '
        Me.mnuRepUndoneReason.FavVisible = False
        Me.mnuRepUndoneReason.Image = CType(resources.GetObject("mnuRepUndoneReason.Image"), System.Drawing.Image)
        Me.mnuRepUndoneReason.IsChange = False
        Me.mnuRepUndoneReason.IsFavVisible = True
        Me.mnuRepUndoneReason.Name = "mnuRepUndoneReason"
        Me.mnuRepUndoneReason.Size = New System.Drawing.Size(366, 22)
        Me.mnuRepUndoneReason.Tag = "10-10-1"
        Me.mnuRepUndoneReason.Text = "(10-10-1) گزارش روزانه عدم انجام کار"
        '
        'mnuRepUndoneReasonArea
        '
        Me.mnuRepUndoneReasonArea.FavVisible = False
        Me.mnuRepUndoneReasonArea.Image = CType(resources.GetObject("mnuRepUndoneReasonArea.Image"), System.Drawing.Image)
        Me.mnuRepUndoneReasonArea.IsChange = False
        Me.mnuRepUndoneReasonArea.IsFavVisible = True
        Me.mnuRepUndoneReasonArea.Name = "mnuRepUndoneReasonArea"
        Me.mnuRepUndoneReasonArea.Size = New System.Drawing.Size(366, 22)
        Me.mnuRepUndoneReasonArea.Tag = "10-10-2"
        Me.mnuRepUndoneReasonArea.Text = "(10-10-2) گزارش تعداد خاموشي‌هاي عدم انجام به تفکيک ناحيه"
        '
        'mnuRepWantedInfo
        '
        Me.mnuRepWantedInfo.FavVisible = False
        Me.mnuRepWantedInfo.Image = CType(resources.GetObject("mnuRepWantedInfo.Image"), System.Drawing.Image)
        Me.mnuRepWantedInfo.IsChange = False
        Me.mnuRepWantedInfo.IsFavVisible = True
        Me.mnuRepWantedInfo.Name = "mnuRepWantedInfo"
        Me.mnuRepWantedInfo.Size = New System.Drawing.Size(460, 22)
        Me.mnuRepWantedInfo.Text = "(10-11) گزارش خاموشي‌هاي بابرنامه و اعمال آن"
        '
        'mnuRep_LPAndSerghati
        '
        Me.mnuRep_LPAndSerghati.FavVisible = False
        Me.mnuRep_LPAndSerghati.Image = CType(resources.GetObject("mnuRep_LPAndSerghati.Image"), System.Drawing.Image)
        Me.mnuRep_LPAndSerghati.IsChange = False
        Me.mnuRep_LPAndSerghati.IsFavVisible = True
        Me.mnuRep_LPAndSerghati.Name = "mnuRep_LPAndSerghati"
        Me.mnuRep_LPAndSerghati.Size = New System.Drawing.Size(460, 22)
        Me.mnuRep_LPAndSerghati.Text = "(10-12) گزارش خاموشي‌هاي فشار ضعيف و سرقتي شبکه"
        '
        'mnuSerghatPart
        '
        Me.mnuSerghatPart.FavVisible = False
        Me.mnuSerghatPart.Image = CType(resources.GetObject("mnuSerghatPart.Image"), System.Drawing.Image)
        Me.mnuSerghatPart.IsChange = False
        Me.mnuSerghatPart.IsFavVisible = True
        Me.mnuSerghatPart.Name = "mnuSerghatPart"
        Me.mnuSerghatPart.Size = New System.Drawing.Size(460, 22)
        Me.mnuSerghatPart.Text = "(10-13) گزارش تجهيزات سرقتي خطوط فشار متوسط ، فشار ضعيف و روشنايي معابر"
        '
        'mnuThriftPower
        '
        Me.mnuThriftPower.FavVisible = False
        Me.mnuThriftPower.Image = CType(resources.GetObject("mnuThriftPower.Image"), System.Drawing.Image)
        Me.mnuThriftPower.IsChange = False
        Me.mnuThriftPower.IsFavVisible = True
        Me.mnuThriftPower.Name = "mnuThriftPower"
        Me.mnuThriftPower.Size = New System.Drawing.Size(460, 22)
        Me.mnuThriftPower.Text = "(10-14) گزارش انرژي صرفه جويي شده با استفاده از عمليات خط گرم"
        '
        'mnuReport_10_31
        '
        Me.mnuReport_10_31.FavVisible = False
        Me.mnuReport_10_31.Image = CType(resources.GetObject("mnuReport_10_31.Image"), System.Drawing.Image)
        Me.mnuReport_10_31.IsChange = False
        Me.mnuReport_10_31.IsFavVisible = True
        Me.mnuReport_10_31.Name = "mnuReport_10_31"
        Me.mnuReport_10_31.Size = New System.Drawing.Size(460, 22)
        Me.mnuReport_10_31.Text = "(10-31) گزارش مديريت همزماني با ساير خاموشي ها "
        '
        'mnuReport_10_15
        '
        Me.mnuReport_10_15.FavVisible = False
        Me.mnuReport_10_15.Image = CType(resources.GetObject("mnuReport_10_15.Image"), System.Drawing.Image)
        Me.mnuReport_10_15.IsChange = False
        Me.mnuReport_10_15.IsFavVisible = True
        Me.mnuReport_10_15.Name = "mnuReport_10_15"
        Me.mnuReport_10_15.Size = New System.Drawing.Size(460, 22)
        Me.mnuReport_10_15.Text = "(10-15) گزارش خاموشي هاي فشار ضعيف پست هاي توزيع"
        '
        'mnuReport_10_17
        '
        Me.mnuReport_10_17.FavVisible = False
        Me.mnuReport_10_17.Image = CType(resources.GetObject("mnuReport_10_17.Image"), System.Drawing.Image)
        Me.mnuReport_10_17.IsChange = False
        Me.mnuReport_10_17.IsFavVisible = True
        Me.mnuReport_10_17.Name = "mnuReport_10_17"
        Me.mnuReport_10_17.Size = New System.Drawing.Size(460, 22)
        Me.mnuReport_10_17.Text = "(10-17) گزارش انرژي صرفه جويي شده با استفاده از مانور خطوط فشار متوسط"
        '
        'mnuReport_10_25
        '
        Me.mnuReport_10_25.FavVisible = False
        Me.mnuReport_10_25.Image = CType(resources.GetObject("mnuReport_10_25.Image"), System.Drawing.Image)
        Me.mnuReport_10_25.IsChange = False
        Me.mnuReport_10_25.IsFavVisible = True
        Me.mnuReport_10_25.Name = "mnuReport_10_25"
        Me.mnuReport_10_25.Size = New System.Drawing.Size(460, 22)
        Me.mnuReport_10_25.Tag = "10-25"
        Me.mnuReport_10_25.Text = "(10-25) گزارش شاخص ها"
        '
        'mnuReport_10_26
        '
        Me.mnuReport_10_26.FavVisible = False
        Me.mnuReport_10_26.Image = CType(resources.GetObject("mnuReport_10_26.Image"), System.Drawing.Image)
        Me.mnuReport_10_26.IsChange = False
        Me.mnuReport_10_26.IsFavVisible = True
        Me.mnuReport_10_26.Name = "mnuReport_10_26"
        Me.mnuReport_10_26.Size = New System.Drawing.Size(460, 22)
        Me.mnuReport_10_26.Tag = "10-26"
        Me.mnuReport_10_26.Text = "(10-26) گزارش شاخص هاي قابليت اطمينان"
        '
        'mnuReport_10_19
        '
        Me.mnuReport_10_19.FavVisible = False
        Me.mnuReport_10_19.Image = CType(resources.GetObject("mnuReport_10_19.Image"), System.Drawing.Image)
        Me.mnuReport_10_19.IsChange = False
        Me.mnuReport_10_19.IsFavVisible = True
        Me.mnuReport_10_19.Name = "mnuReport_10_19"
        Me.mnuReport_10_19.Size = New System.Drawing.Size(460, 22)
        Me.mnuReport_10_19.Tag = ""
        Me.mnuReport_10_19.Text = "(10-19) گزارش شاخص SAIDI خطوط فشار متوسط"
        '
        'mnuRep_10_21
        '
        Me.mnuRep_10_21.FavVisible = False
        Me.mnuRep_10_21.Image = CType(resources.GetObject("mnuRep_10_21.Image"), System.Drawing.Image)
        Me.mnuRep_10_21.IsChange = False
        Me.mnuRep_10_21.IsFavVisible = True
        Me.mnuRep_10_21.Name = "mnuRep_10_21"
        Me.mnuRep_10_21.Size = New System.Drawing.Size(460, 22)
        Me.mnuRep_10_21.Text = "(10-21) گزارش عملکرد کات اوت فيوز ترانس‌هاي توزيع"
        '
        'mnuRep_10_23
        '
        Me.mnuRep_10_23.FavVisible = False
        Me.mnuRep_10_23.Image = CType(resources.GetObject("mnuRep_10_23.Image"), System.Drawing.Image)
        Me.mnuRep_10_23.IsChange = False
        Me.mnuRep_10_23.IsFavVisible = True
        Me.mnuRep_10_23.Name = "mnuRep_10_23"
        Me.mnuRep_10_23.Size = New System.Drawing.Size(460, 22)
        Me.mnuRep_10_23.Text = "(10-23) گزارش قطعي فيدرها در ساعات روز "
        '
        'mnuRep_10_27
        '
        Me.mnuRep_10_27.FavVisible = False
        Me.mnuRep_10_27.Image = CType(resources.GetObject("mnuRep_10_27.Image"), System.Drawing.Image)
        Me.mnuRep_10_27.IsChange = False
        Me.mnuRep_10_27.IsFavVisible = True
        Me.mnuRep_10_27.Name = "mnuRep_10_27"
        Me.mnuRep_10_27.Size = New System.Drawing.Size(460, 22)
        Me.mnuRep_10_27.Text = "(10-27) گزارش  ساعت به ساعت خاموشی های تحمیلی شبکه فشار متوسط "
        '
        'mnuRep_10_28
        '
        Me.mnuRep_10_28.FavVisible = False
        Me.mnuRep_10_28.Image = CType(resources.GetObject("mnuRep_10_28.Image"), System.Drawing.Image)
        Me.mnuRep_10_28.IsChange = False
        Me.mnuRep_10_28.IsFavVisible = True
        Me.mnuRep_10_28.Name = "mnuRep_10_28"
        Me.mnuRep_10_28.Size = New System.Drawing.Size(460, 22)
        Me.mnuRep_10_28.Text = "(10-28) گزارش عملکرد روزانه شبکه فشار متوسط "
        '
        'mnuRep_10_29
        '
        Me.mnuRep_10_29.FavVisible = False
        Me.mnuRep_10_29.Image = CType(resources.GetObject("mnuRep_10_29.Image"), System.Drawing.Image)
        Me.mnuRep_10_29.IsChange = False
        Me.mnuRep_10_29.IsFavVisible = True
        Me.mnuRep_10_29.Name = "mnuRep_10_29"
        Me.mnuRep_10_29.Size = New System.Drawing.Size(460, 22)
        Me.mnuRep_10_29.Text = "(10-29) گزارش قطعی‌های بابرنامه با درخواست"
        '
        'mnuRep_10_30
        '
        Me.mnuRep_10_30.FavVisible = False
        Me.mnuRep_10_30.Image = CType(resources.GetObject("mnuRep_10_30.Image"), System.Drawing.Image)
        Me.mnuRep_10_30.IsChange = False
        Me.mnuRep_10_30.IsFavVisible = True
        Me.mnuRep_10_30.Name = "mnuRep_10_30"
        Me.mnuRep_10_30.Size = New System.Drawing.Size(460, 22)
        Me.mnuRep_10_30.Text = "(10-30) گزارش آماری انرژی توزیع نشده به تفکیک علت "
        '
        'mnuRepCompare
        '
        Me.mnuRepCompare.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuRepCompareDCCount, Me.munRep_11_2, Me.mnuRep_11_3, Me.mnuRepCompareDCEnergy, Me.ToolStripSeparator70, Me.mnuRep_11_5, Me.mnuRep_11_6, Me.mnuRep_11_7, Me.mnuRep_11_8, Me.mnuRep_11_9, Me.mnuRep_11_10})
        Me.mnuRepCompare.Name = "mnuRepCompare"
        Me.mnuRepCompare.Size = New System.Drawing.Size(206, 22)
        Me.mnuRepCompare.Text = "(11) گزارشات مقايسه‌اي"
        '
        'mnuRepCompareDCCount
        '
        Me.mnuRepCompareDCCount.FavVisible = False
        Me.mnuRepCompareDCCount.Image = CType(resources.GetObject("mnuRepCompareDCCount.Image"), System.Drawing.Image)
        Me.mnuRepCompareDCCount.IsChange = False
        Me.mnuRepCompareDCCount.IsFavVisible = True
        Me.mnuRepCompareDCCount.Name = "mnuRepCompareDCCount"
        Me.mnuRepCompareDCCount.Size = New System.Drawing.Size(559, 22)
        Me.mnuRepCompareDCCount.Text = "(11-1) گزارش مقايسه‌اي تعداد قطع فيدر"
        '
        'munRep_11_2
        '
        Me.munRep_11_2.FavVisible = False
        Me.munRep_11_2.Image = CType(resources.GetObject("munRep_11_2.Image"), System.Drawing.Image)
        Me.munRep_11_2.IsChange = False
        Me.munRep_11_2.IsFavVisible = True
        Me.munRep_11_2.Name = "munRep_11_2"
        Me.munRep_11_2.Size = New System.Drawing.Size(559, 22)
        Me.munRep_11_2.Text = "(11-2) گزارش مقايسه‌اي انرژي توزيع نشده"
        '
        'mnuRep_11_3
        '
        Me.mnuRep_11_3.FavVisible = False
        Me.mnuRep_11_3.Image = CType(resources.GetObject("mnuRep_11_3.Image"), System.Drawing.Image)
        Me.mnuRep_11_3.IsChange = False
        Me.mnuRep_11_3.IsFavVisible = True
        Me.mnuRep_11_3.Name = "mnuRep_11_3"
        Me.mnuRep_11_3.Size = New System.Drawing.Size(559, 22)
        Me.mnuRep_11_3.Text = "(11-3) گزارش مقايسه‌اي مدت زمان خاموشي"
        '
        'mnuRepCompareDCEnergy
        '
        Me.mnuRepCompareDCEnergy.FavVisible = False
        Me.mnuRepCompareDCEnergy.Image = CType(resources.GetObject("mnuRepCompareDCEnergy.Image"), System.Drawing.Image)
        Me.mnuRepCompareDCEnergy.IsChange = False
        Me.mnuRepCompareDCEnergy.IsFavVisible = True
        Me.mnuRepCompareDCEnergy.Name = "mnuRepCompareDCEnergy"
        Me.mnuRepCompareDCEnergy.Size = New System.Drawing.Size(559, 22)
        Me.mnuRepCompareDCEnergy.Text = "(11-4) گزارش مقايسه‌اي نرخ انرژي توزيع نشده"
        '
        'ToolStripSeparator70
        '
        Me.ToolStripSeparator70.Name = "ToolStripSeparator70"
        Me.ToolStripSeparator70.Size = New System.Drawing.Size(556, 6)
        '
        'mnuRep_11_5
        '
        Me.mnuRep_11_5.FavVisible = False
        Me.mnuRep_11_5.Image = CType(resources.GetObject("mnuRep_11_5.Image"), System.Drawing.Image)
        Me.mnuRep_11_5.IsChange = False
        Me.mnuRep_11_5.IsFavVisible = True
        Me.mnuRep_11_5.Name = "mnuRep_11_5"
        Me.mnuRep_11_5.Size = New System.Drawing.Size(559, 22)
        Me.mnuRep_11_5.Tag = "11-5"
        Me.mnuRep_11_5.Text = "(11-5) گزارش و نمودار مقايسه‌اي نرخ انرژي توزيع نشده شبکه فشار متوسط  به تفکيک ما" &
    "ه"
        '
        'mnuRep_11_6
        '
        Me.mnuRep_11_6.FavVisible = False
        Me.mnuRep_11_6.Image = CType(resources.GetObject("mnuRep_11_6.Image"), System.Drawing.Image)
        Me.mnuRep_11_6.IsChange = False
        Me.mnuRep_11_6.IsFavVisible = True
        Me.mnuRep_11_6.Name = "mnuRep_11_6"
        Me.mnuRep_11_6.Size = New System.Drawing.Size(559, 22)
        Me.mnuRep_11_6.Tag = "11-6"
        Me.mnuRep_11_6.Text = "(11-6) گزارش و نمودار مقايسه‌اي مدت زمان خاموشي براي هر مشترک شبکه فشار متوسط  به" &
    " تفکيک ماه"
        '
        'mnuRep_11_7
        '
        Me.mnuRep_11_7.FavVisible = False
        Me.mnuRep_11_7.Image = CType(resources.GetObject("mnuRep_11_7.Image"), System.Drawing.Image)
        Me.mnuRep_11_7.IsChange = False
        Me.mnuRep_11_7.IsFavVisible = True
        Me.mnuRep_11_7.Name = "mnuRep_11_7"
        Me.mnuRep_11_7.Size = New System.Drawing.Size(559, 22)
        Me.mnuRep_11_7.Tag = "11-7"
        Me.mnuRep_11_7.Text = "(11-7) گزارش و نمودار مقايسه‌اي تعداد خاموشي در هر 100 کيلومتر شبکه فشار متوسط  ب" &
    "ه تفکيک ماه"
        '
        'mnuRep_11_8
        '
        Me.mnuRep_11_8.FavVisible = False
        Me.mnuRep_11_8.Image = CType(resources.GetObject("mnuRep_11_8.Image"), System.Drawing.Image)
        Me.mnuRep_11_8.IsChange = False
        Me.mnuRep_11_8.IsFavVisible = True
        Me.mnuRep_11_8.Name = "mnuRep_11_8"
        Me.mnuRep_11_8.Size = New System.Drawing.Size(559, 22)
        Me.mnuRep_11_8.Text = "(11-8) گزارش مقايسه‌اي تعداد ، مدت و انرژي توزيع نشده"
        '
        'mnuRep_11_9
        '
        Me.mnuRep_11_9.FavVisible = False
        Me.mnuRep_11_9.Image = CType(resources.GetObject("mnuRep_11_9.Image"), System.Drawing.Image)
        Me.mnuRep_11_9.IsChange = False
        Me.mnuRep_11_9.IsFavVisible = True
        Me.mnuRep_11_9.Name = "mnuRep_11_9"
        Me.mnuRep_11_9.Size = New System.Drawing.Size(559, 22)
        Me.mnuRep_11_9.Text = "(11-9) گزارش مقايسه‌اي شاخص‌هاي انرژي با شاخص‌هاي هدف"
        '
        'mnuRep_11_10
        '
        Me.mnuRep_11_10.FavVisible = False
        Me.mnuRep_11_10.Image = CType(resources.GetObject("mnuRep_11_10.Image"), System.Drawing.Image)
        Me.mnuRep_11_10.IsChange = False
        Me.mnuRep_11_10.IsFavVisible = True
        Me.mnuRep_11_10.Name = "mnuRep_11_10"
        Me.mnuRep_11_10.Size = New System.Drawing.Size(559, 22)
        Me.mnuRep_11_10.Text = "(11-10) گزارش شاخص های زمان خاموشی، مدت رفع خاموشی و نرخ انرژی"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(203, 6)
        '
        'mnuExcelReports
        '
        Me.mnuExcelReports.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuRepExcelform1, Me.mnuRepExcelform2, Me.mnuRepExcelform3, Me.mnuRepExcelform4, Me.mnuRepExcelform5, Me.mnuRepExcelform6, Me.ToolStripSeparator66, Me.mnuRepExcelformDaily, Me.mnuRepExcelformDaily_02, Me.mnuRepExcelformDaily_03, Me.mnuRepExcelformDaily_04, Me.mnuRepExcelformDaily_05, Me.ToolStripSeparator67, Me.mnuRepExcelweek, Me.mnuRepExcelLPFeederPayesh, Me.ToolStripSeparator68, Me.mnuRepExcelTavanir_01, Me.mnuRepExcelTavanir_02, Me.mnuRepExcelTavanir_03, Me.ToolStripSeparator69, Me.mnuRepExcelMPFeederManagement, Me.mnuRepExcelLPFeederManagement, Me.mnuRepExcelMPFeederDaily, Me.mnuRepExcelform7, Me.mnuRepExcelImportantEvents, Me.mnuRepExcelGroupSetGroup, Me.mnuRepExcelDisconnectGroupSet, Me.mnuRepExccelComunicationSystems, Me.mnuRepExcelGISLoadings, Me.mnuDailyNetworkState})
        Me.mnuExcelReports.Name = "mnuExcelReports"
        Me.mnuExcelReports.Size = New System.Drawing.Size(206, 22)
        Me.mnuExcelReports.Text = "(12) گزارش‌هاي اکسل"
        '
        'mnuRepExcelform1
        '
        Me.mnuRepExcelform1.FavVisible = False
        Me.mnuRepExcelform1.Image = CType(resources.GetObject("mnuRepExcelform1.Image"), System.Drawing.Image)
        Me.mnuRepExcelform1.IsChange = False
        Me.mnuRepExcelform1.IsFavVisible = True
        Me.mnuRepExcelform1.Name = "mnuRepExcelform1"
        Me.mnuRepExcelform1.Size = New System.Drawing.Size(490, 22)
        Me.mnuRepExcelform1.Tag = "mnuRepExcelform1"
        Me.mnuRepExcelform1.Text = "فرم 1 اکسل: نمودار مقايسه‌اي خاموشي‌هاي اعمال شده به شبکه"
        '
        'mnuRepExcelform2
        '
        Me.mnuRepExcelform2.FavVisible = False
        Me.mnuRepExcelform2.Image = CType(resources.GetObject("mnuRepExcelform2.Image"), System.Drawing.Image)
        Me.mnuRepExcelform2.IsChange = False
        Me.mnuRepExcelform2.IsFavVisible = True
        Me.mnuRepExcelform2.Name = "mnuRepExcelform2"
        Me.mnuRepExcelform2.Size = New System.Drawing.Size(490, 22)
        Me.mnuRepExcelform2.Tag = "mnuRepExcelform2"
        Me.mnuRepExcelform2.Text = "فرم 2 اکسل: فهرست اتفاقات مهم بيست کيلوولت"
        '
        'mnuRepExcelform3
        '
        Me.mnuRepExcelform3.FavVisible = False
        Me.mnuRepExcelform3.Image = CType(resources.GetObject("mnuRepExcelform3.Image"), System.Drawing.Image)
        Me.mnuRepExcelform3.IsChange = False
        Me.mnuRepExcelform3.IsFavVisible = True
        Me.mnuRepExcelform3.Name = "mnuRepExcelform3"
        Me.mnuRepExcelform3.Size = New System.Drawing.Size(490, 22)
        Me.mnuRepExcelform3.Tag = "mnuRepExcelform3"
        Me.mnuRepExcelform3.Text = "فرم 3 اکسل: درصد انرژي توزيع نشده شبکه فشار متوسط و فشار ضعيف به تفکيل علل قطع"
        '
        'mnuRepExcelform4
        '
        Me.mnuRepExcelform4.FavVisible = False
        Me.mnuRepExcelform4.Image = CType(resources.GetObject("mnuRepExcelform4.Image"), System.Drawing.Image)
        Me.mnuRepExcelform4.IsChange = False
        Me.mnuRepExcelform4.IsFavVisible = True
        Me.mnuRepExcelform4.Name = "mnuRepExcelform4"
        Me.mnuRepExcelform4.Size = New System.Drawing.Size(490, 22)
        Me.mnuRepExcelform4.Tag = "mnuRepExcelform4"
        Me.mnuRepExcelform4.Text = "فرم 4 اکسل: شاخص‌هاي قابليت اطمينان شبکه توزيع"
        '
        'mnuRepExcelform5
        '
        Me.mnuRepExcelform5.FavVisible = False
        Me.mnuRepExcelform5.Image = CType(resources.GetObject("mnuRepExcelform5.Image"), System.Drawing.Image)
        Me.mnuRepExcelform5.IsChange = False
        Me.mnuRepExcelform5.IsFavVisible = True
        Me.mnuRepExcelform5.Name = "mnuRepExcelform5"
        Me.mnuRepExcelform5.Size = New System.Drawing.Size(490, 22)
        Me.mnuRepExcelform5.Tag = "mnuRepExcelform5"
        Me.mnuRepExcelform5.Text = "فرم 5 اکسل: فهرست با برنامه بيست کيلوولت"
        '
        'mnuRepExcelform6
        '
        Me.mnuRepExcelform6.FavVisible = False
        Me.mnuRepExcelform6.Image = CType(resources.GetObject("mnuRepExcelform6.Image"), System.Drawing.Image)
        Me.mnuRepExcelform6.IsChange = False
        Me.mnuRepExcelform6.IsFavVisible = True
        Me.mnuRepExcelform6.Name = "mnuRepExcelform6"
        Me.mnuRepExcelform6.Size = New System.Drawing.Size(490, 22)
        Me.mnuRepExcelform6.Tag = "mnuRepExcelform6"
        Me.mnuRepExcelform6.Text = "فرم 6 اکسل: انرژي توزيع نشده بخش توزيع و فوق توزيع"
        '
        'ToolStripSeparator66
        '
        Me.ToolStripSeparator66.Name = "ToolStripSeparator66"
        Me.ToolStripSeparator66.Size = New System.Drawing.Size(487, 6)
        '
        'mnuRepExcelformDaily
        '
        Me.mnuRepExcelformDaily.FavVisible = False
        Me.mnuRepExcelformDaily.Image = CType(resources.GetObject("mnuRepExcelformDaily.Image"), System.Drawing.Image)
        Me.mnuRepExcelformDaily.IsChange = False
        Me.mnuRepExcelformDaily.IsFavVisible = True
        Me.mnuRepExcelformDaily.Name = "mnuRepExcelformDaily"
        Me.mnuRepExcelformDaily.Size = New System.Drawing.Size(490, 22)
        Me.mnuRepExcelformDaily.Tag = "mnuRepExcelformDaily"
        Me.mnuRepExcelformDaily.Text = "گزارش روزانه وضعيت شبکه از نگاه امور ديسپاچينگ و فوريت‌هاي برق (نمونه 1)"
        '
        'mnuRepExcelformDaily_02
        '
        Me.mnuRepExcelformDaily_02.FavVisible = False
        Me.mnuRepExcelformDaily_02.Image = CType(resources.GetObject("mnuRepExcelformDaily_02.Image"), System.Drawing.Image)
        Me.mnuRepExcelformDaily_02.IsChange = False
        Me.mnuRepExcelformDaily_02.IsFavVisible = True
        Me.mnuRepExcelformDaily_02.Name = "mnuRepExcelformDaily_02"
        Me.mnuRepExcelformDaily_02.Size = New System.Drawing.Size(490, 22)
        Me.mnuRepExcelformDaily_02.Tag = "mnuRepExcelformDaily_02"
        Me.mnuRepExcelformDaily_02.Text = "گزارش روزانه وضعيت شبکه از نگاه امور ديسپاچينگ و فوريت‌هاي برق (نمونه 2)"
        '
        'mnuRepExcelformDaily_03
        '
        Me.mnuRepExcelformDaily_03.FavVisible = False
        Me.mnuRepExcelformDaily_03.Image = CType(resources.GetObject("mnuRepExcelformDaily_03.Image"), System.Drawing.Image)
        Me.mnuRepExcelformDaily_03.IsChange = False
        Me.mnuRepExcelformDaily_03.IsFavVisible = True
        Me.mnuRepExcelformDaily_03.Name = "mnuRepExcelformDaily_03"
        Me.mnuRepExcelformDaily_03.Size = New System.Drawing.Size(490, 22)
        Me.mnuRepExcelformDaily_03.Tag = "mnuRepExcelformDaily_03"
        Me.mnuRepExcelformDaily_03.Text = "گزارش روزانه وضعيت شبکه از نگاه امور ديسپاچينگ و فوريت‌هاي برق (نمونه 3)"
        '
        'mnuRepExcelformDaily_04
        '
        Me.mnuRepExcelformDaily_04.FavVisible = False
        Me.mnuRepExcelformDaily_04.Image = CType(resources.GetObject("mnuRepExcelformDaily_04.Image"), System.Drawing.Image)
        Me.mnuRepExcelformDaily_04.IsChange = False
        Me.mnuRepExcelformDaily_04.IsFavVisible = True
        Me.mnuRepExcelformDaily_04.Name = "mnuRepExcelformDaily_04"
        Me.mnuRepExcelformDaily_04.Size = New System.Drawing.Size(490, 22)
        Me.mnuRepExcelformDaily_04.Tag = "mnuRepExcelformDaily_04"
        Me.mnuRepExcelformDaily_04.Text = "گزارش روزانه وضعيت شبکه از نگاه امور ديسپاچينگ و فوريت‌هاي برق (نمونه 4)"
        '
        'mnuRepExcelformDaily_05
        '
        Me.mnuRepExcelformDaily_05.FavVisible = False
        Me.mnuRepExcelformDaily_05.Image = CType(resources.GetObject("mnuRepExcelformDaily_05.Image"), System.Drawing.Image)
        Me.mnuRepExcelformDaily_05.IsChange = False
        Me.mnuRepExcelformDaily_05.IsFavVisible = True
        Me.mnuRepExcelformDaily_05.Name = "mnuRepExcelformDaily_05"
        Me.mnuRepExcelformDaily_05.Size = New System.Drawing.Size(490, 22)
        Me.mnuRepExcelformDaily_05.Tag = "mnuRepExcelformDaily_05"
        Me.mnuRepExcelformDaily_05.Text = "گزارش روزانه وضعيت شبکه از نگاه امور ديسپاچينگ و فوريت‌هاي برق (نمونه 5)"
        '
        'ToolStripSeparator67
        '
        Me.ToolStripSeparator67.Name = "ToolStripSeparator67"
        Me.ToolStripSeparator67.Size = New System.Drawing.Size(487, 6)
        '
        'mnuRepExcelweek
        '
        Me.mnuRepExcelweek.FavVisible = False
        Me.mnuRepExcelweek.Image = CType(resources.GetObject("mnuRepExcelweek.Image"), System.Drawing.Image)
        Me.mnuRepExcelweek.IsChange = False
        Me.mnuRepExcelweek.IsFavVisible = True
        Me.mnuRepExcelweek.Name = "mnuRepExcelweek"
        Me.mnuRepExcelweek.Size = New System.Drawing.Size(490, 22)
        Me.mnuRepExcelweek.Tag = "mnuRepExcelweek"
        Me.mnuRepExcelweek.Text = "گزارش پايش شبکه"
        '
        'mnuRepExcelLPFeederPayesh
        '
        Me.mnuRepExcelLPFeederPayesh.FavVisible = False
        Me.mnuRepExcelLPFeederPayesh.Image = CType(resources.GetObject("mnuRepExcelLPFeederPayesh.Image"), System.Drawing.Image)
        Me.mnuRepExcelLPFeederPayesh.IsChange = False
        Me.mnuRepExcelLPFeederPayesh.IsFavVisible = True
        Me.mnuRepExcelLPFeederPayesh.Name = "mnuRepExcelLPFeederPayesh"
        Me.mnuRepExcelLPFeederPayesh.Size = New System.Drawing.Size(490, 22)
        Me.mnuRepExcelLPFeederPayesh.Tag = "mnuRepExcelLPFeederPayesh"
        Me.mnuRepExcelLPFeederPayesh.Text = "گزارش پايش خاموشي هاي فشار ضعيف با برنامه"
        '
        'ToolStripSeparator68
        '
        Me.ToolStripSeparator68.Name = "ToolStripSeparator68"
        Me.ToolStripSeparator68.Size = New System.Drawing.Size(487, 6)
        '
        'mnuRepExcelTavanir_01
        '
        Me.mnuRepExcelTavanir_01.FavVisible = False
        Me.mnuRepExcelTavanir_01.Image = CType(resources.GetObject("mnuRepExcelTavanir_01.Image"), System.Drawing.Image)
        Me.mnuRepExcelTavanir_01.IsChange = False
        Me.mnuRepExcelTavanir_01.IsFavVisible = True
        Me.mnuRepExcelTavanir_01.Name = "mnuRepExcelTavanir_01"
        Me.mnuRepExcelTavanir_01.Size = New System.Drawing.Size(490, 22)
        Me.mnuRepExcelTavanir_01.Tag = "mnuRepExcelTavanir_01"
        Me.mnuRepExcelTavanir_01.Text = "گزارش توانير (1)"
        '
        'mnuRepExcelTavanir_02
        '
        Me.mnuRepExcelTavanir_02.FavVisible = False
        Me.mnuRepExcelTavanir_02.Image = CType(resources.GetObject("mnuRepExcelTavanir_02.Image"), System.Drawing.Image)
        Me.mnuRepExcelTavanir_02.IsChange = False
        Me.mnuRepExcelTavanir_02.IsFavVisible = True
        Me.mnuRepExcelTavanir_02.Name = "mnuRepExcelTavanir_02"
        Me.mnuRepExcelTavanir_02.Size = New System.Drawing.Size(490, 22)
        Me.mnuRepExcelTavanir_02.Tag = "mnuRepExcelTavanir_02"
        Me.mnuRepExcelTavanir_02.Text = "گزارش توانير (2)"
        '
        'mnuRepExcelTavanir_03
        '
        Me.mnuRepExcelTavanir_03.FavVisible = False
        Me.mnuRepExcelTavanir_03.Image = CType(resources.GetObject("mnuRepExcelTavanir_03.Image"), System.Drawing.Image)
        Me.mnuRepExcelTavanir_03.IsChange = False
        Me.mnuRepExcelTavanir_03.IsFavVisible = True
        Me.mnuRepExcelTavanir_03.Name = "mnuRepExcelTavanir_03"
        Me.mnuRepExcelTavanir_03.Size = New System.Drawing.Size(490, 22)
        Me.mnuRepExcelTavanir_03.Tag = "mnuRepExcelTavanir_03"
        Me.mnuRepExcelTavanir_03.Text = "گزارش توانير (3)"
        '
        'ToolStripSeparator69
        '
        Me.ToolStripSeparator69.Name = "ToolStripSeparator69"
        Me.ToolStripSeparator69.Size = New System.Drawing.Size(487, 6)
        '
        'mnuRepExcelMPFeederManagement
        '
        Me.mnuRepExcelMPFeederManagement.FavVisible = False
        Me.mnuRepExcelMPFeederManagement.Image = CType(resources.GetObject("mnuRepExcelMPFeederManagement.Image"), System.Drawing.Image)
        Me.mnuRepExcelMPFeederManagement.IsChange = False
        Me.mnuRepExcelMPFeederManagement.IsFavVisible = True
        Me.mnuRepExcelMPFeederManagement.Name = "mnuRepExcelMPFeederManagement"
        Me.mnuRepExcelMPFeederManagement.Size = New System.Drawing.Size(490, 22)
        Me.mnuRepExcelMPFeederManagement.Tag = "mnuRepExcelMPFeederManagement"
        Me.mnuRepExcelMPFeederManagement.Text = "گزارش مديريتي وقايع شبکه فشار متوسط و فيدرهاي بحراني"
        '
        'mnuRepExcelLPFeederManagement
        '
        Me.mnuRepExcelLPFeederManagement.FavVisible = False
        Me.mnuRepExcelLPFeederManagement.Image = CType(resources.GetObject("mnuRepExcelLPFeederManagement.Image"), System.Drawing.Image)
        Me.mnuRepExcelLPFeederManagement.IsChange = False
        Me.mnuRepExcelLPFeederManagement.IsFavVisible = True
        Me.mnuRepExcelLPFeederManagement.Name = "mnuRepExcelLPFeederManagement"
        Me.mnuRepExcelLPFeederManagement.Size = New System.Drawing.Size(490, 22)
        Me.mnuRepExcelLPFeederManagement.Tag = "mnuRepExcelLPFeederManagement"
        Me.mnuRepExcelLPFeederManagement.Text = "گزارش تجمعي تعداد و زمان رفع خاموشي شبکه فشار ضعيف"
        '
        'mnuRepExcelMPFeederDaily
        '
        Me.mnuRepExcelMPFeederDaily.FavVisible = False
        Me.mnuRepExcelMPFeederDaily.Image = CType(resources.GetObject("mnuRepExcelMPFeederDaily.Image"), System.Drawing.Image)
        Me.mnuRepExcelMPFeederDaily.IsChange = False
        Me.mnuRepExcelMPFeederDaily.IsFavVisible = True
        Me.mnuRepExcelMPFeederDaily.Name = "mnuRepExcelMPFeederDaily"
        Me.mnuRepExcelMPFeederDaily.Size = New System.Drawing.Size(490, 22)
        Me.mnuRepExcelMPFeederDaily.Tag = "mnuRepExcelMPFeederDaily"
        Me.mnuRepExcelMPFeederDaily.Text = "گزارش مديريتي وقايع شبکه فشار متوسط (نمونه 2)"
        '
        'mnuRepExcelform7
        '
        Me.mnuRepExcelform7.FavVisible = False
        Me.mnuRepExcelform7.Image = CType(resources.GetObject("mnuRepExcelform7.Image"), System.Drawing.Image)
        Me.mnuRepExcelform7.IsChange = False
        Me.mnuRepExcelform7.IsFavVisible = True
        Me.mnuRepExcelform7.Name = "mnuRepExcelform7"
        Me.mnuRepExcelform7.Size = New System.Drawing.Size(490, 22)
        Me.mnuRepExcelform7.Tag = "mnuRepExcelform7"
        Me.mnuRepExcelform7.Text = "گزارش اطلاعات آماري شبکه"
        '
        'mnuRepExcelImportantEvents
        '
        Me.mnuRepExcelImportantEvents.FavVisible = False
        Me.mnuRepExcelImportantEvents.Image = CType(resources.GetObject("mnuRepExcelImportantEvents.Image"), System.Drawing.Image)
        Me.mnuRepExcelImportantEvents.IsChange = False
        Me.mnuRepExcelImportantEvents.IsFavVisible = True
        Me.mnuRepExcelImportantEvents.Name = "mnuRepExcelImportantEvents"
        Me.mnuRepExcelImportantEvents.Size = New System.Drawing.Size(490, 22)
        Me.mnuRepExcelImportantEvents.Tag = "mnuRepExcelImportantEvents"
        Me.mnuRepExcelImportantEvents.Text = "گزارش حوادث مهم شبکه"
        '
        'mnuRepExcelGroupSetGroup
        '
        Me.mnuRepExcelGroupSetGroup.FavVisible = False
        Me.mnuRepExcelGroupSetGroup.Image = CType(resources.GetObject("mnuRepExcelGroupSetGroup.Image"), System.Drawing.Image)
        Me.mnuRepExcelGroupSetGroup.IsChange = False
        Me.mnuRepExcelGroupSetGroup.IsFavVisible = True
        Me.mnuRepExcelGroupSetGroup.Name = "mnuRepExcelGroupSetGroup"
        Me.mnuRepExcelGroupSetGroup.Size = New System.Drawing.Size(490, 22)
        Me.mnuRepExcelGroupSetGroup.Text = "گزارش قطعي بي برنامه فيدرهاي فشار متوسط بر اساس گروه عيب"
        '
        'mnuRepExcelDisconnectGroupSet
        '
        Me.mnuRepExcelDisconnectGroupSet.FavVisible = False
        Me.mnuRepExcelDisconnectGroupSet.Image = CType(resources.GetObject("mnuRepExcelDisconnectGroupSet.Image"), System.Drawing.Image)
        Me.mnuRepExcelDisconnectGroupSet.IsChange = False
        Me.mnuRepExcelDisconnectGroupSet.IsFavVisible = True
        Me.mnuRepExcelDisconnectGroupSet.Name = "mnuRepExcelDisconnectGroupSet"
        Me.mnuRepExcelDisconnectGroupSet.Size = New System.Drawing.Size(490, 22)
        Me.mnuRepExcelDisconnectGroupSet.Tag = "mnuRepExcelDisconnectGroupSet"
        Me.mnuRepExcelDisconnectGroupSet.Text = "گزارش تعداد قطعي و انرژي توزيع نشده فيدرها به تفکيک علل قطع"
        '
        'mnuRepExccelComunicationSystems
        '
        Me.mnuRepExccelComunicationSystems.FavVisible = False
        Me.mnuRepExccelComunicationSystems.Image = CType(resources.GetObject("mnuRepExccelComunicationSystems.Image"), System.Drawing.Image)
        Me.mnuRepExccelComunicationSystems.IsChange = False
        Me.mnuRepExccelComunicationSystems.IsFavVisible = True
        Me.mnuRepExccelComunicationSystems.Name = "mnuRepExccelComunicationSystems"
        Me.mnuRepExccelComunicationSystems.Size = New System.Drawing.Size(490, 22)
        Me.mnuRepExccelComunicationSystems.Text = "گزارش بررسي وضعيت سرورهاي نواحي"
        '
        'mnuRepExcelGISLoadings
        '
        Me.mnuRepExcelGISLoadings.FavVisible = False
        Me.mnuRepExcelGISLoadings.Image = CType(resources.GetObject("mnuRepExcelGISLoadings.Image"), System.Drawing.Image)
        Me.mnuRepExcelGISLoadings.IsChange = False
        Me.mnuRepExcelGISLoadings.IsFavVisible = False
        Me.mnuRepExcelGISLoadings.Name = "mnuRepExcelGISLoadings"
        Me.mnuRepExcelGISLoadings.Size = New System.Drawing.Size(490, 22)
        Me.mnuRepExcelGISLoadings.Tag = "mnuRepExcelGISLoadings"
        Me.mnuRepExcelGISLoadings.Text = "گزارش بارگيري پست‌هاي توزيع و فيدرهاي فشار ضعيف"
        Me.mnuRepExcelGISLoadings.Visible = False
        '
        'mnuDailyNetworkState
        '
        Me.mnuDailyNetworkState.FavVisible = False
        Me.mnuDailyNetworkState.Image = CType(resources.GetObject("mnuDailyNetworkState.Image"), System.Drawing.Image)
        Me.mnuDailyNetworkState.IsChange = False
        Me.mnuDailyNetworkState.IsFavVisible = True
        Me.mnuDailyNetworkState.Name = "mnuDailyNetworkState"
        Me.mnuDailyNetworkState.Size = New System.Drawing.Size(490, 22)
        Me.mnuDailyNetworkState.Tag = "mnuRepExcelDailyNetworkState"
        Me.mnuDailyNetworkState.Text = "اهم گزارش وضعيت شبکه"
        '
        'ToolStripSeparator76
        '
        Me.ToolStripSeparator76.Name = "ToolStripSeparator76"
        Me.ToolStripSeparator76.Size = New System.Drawing.Size(203, 6)
        '
        'mnuExtractionReports
        '
        Me.mnuExtractionReports.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuReport_13_1, Me.mnuReport_13_2, Me.mnuReport_13_3, Me.mnuReport_13_4})
        Me.mnuExtractionReports.Name = "mnuExtractionReports"
        Me.mnuExtractionReports.Size = New System.Drawing.Size(206, 22)
        Me.mnuExtractionReports.Text = "(13) گزارشات پيش بيني"
        '
        'mnuReport_13_1
        '
        Me.mnuReport_13_1.FavVisible = False
        Me.mnuReport_13_1.Image = CType(resources.GetObject("mnuReport_13_1.Image"), System.Drawing.Image)
        Me.mnuReport_13_1.IsChange = False
        Me.mnuReport_13_1.IsFavVisible = True
        Me.mnuReport_13_1.Name = "mnuReport_13_1"
        Me.mnuReport_13_1.Size = New System.Drawing.Size(385, 22)
        Me.mnuReport_13_1.Tag = "13-1"
        Me.mnuReport_13_1.Text = "(13-1) گزارش پيش بيني تعداد خاموشي با استفاده از داده کاوي"
        '
        'mnuReport_13_2
        '
        Me.mnuReport_13_2.FavVisible = False
        Me.mnuReport_13_2.Image = CType(resources.GetObject("mnuReport_13_2.Image"), System.Drawing.Image)
        Me.mnuReport_13_2.IsChange = False
        Me.mnuReport_13_2.IsFavVisible = True
        Me.mnuReport_13_2.Name = "mnuReport_13_2"
        Me.mnuReport_13_2.Size = New System.Drawing.Size(385, 22)
        Me.mnuReport_13_2.Tag = "13-2"
        Me.mnuReport_13_2.Text = "(13-2) گزارش پيش بيني انرژي توزيع نشده با استفاده از داده کاوي"
        '
        'mnuReport_13_3
        '
        Me.mnuReport_13_3.FavVisible = False
        Me.mnuReport_13_3.Image = CType(resources.GetObject("mnuReport_13_3.Image"), System.Drawing.Image)
        Me.mnuReport_13_3.IsChange = False
        Me.mnuReport_13_3.IsFavVisible = True
        Me.mnuReport_13_3.Name = "mnuReport_13_3"
        Me.mnuReport_13_3.Size = New System.Drawing.Size(385, 22)
        Me.mnuReport_13_3.Tag = "13-3"
        Me.mnuReport_13_3.Text = "(13-3) گزارش پيش بيني مدت زمان خاموشي با استفاده از داده کاوي"
        '
        'mnuReport_13_4
        '
        Me.mnuReport_13_4.FavVisible = False
        Me.mnuReport_13_4.Image = CType(resources.GetObject("mnuReport_13_4.Image"), System.Drawing.Image)
        Me.mnuReport_13_4.IsChange = False
        Me.mnuReport_13_4.IsFavVisible = True
        Me.mnuReport_13_4.Name = "mnuReport_13_4"
        Me.mnuReport_13_4.Size = New System.Drawing.Size(385, 22)
        Me.mnuReport_13_4.Text = "(13-4) گزارش پيش بيني بار پست هاي توزيع با استفاده از داده کاوي"
        '
        'mnuTavanirReports
        '
        Me.mnuTavanirReports.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuReport_14_1, Me.mnuReport_14_2, Me.mnuReport_14_3, Me.mnuReport_14_4})
        Me.mnuTavanirReports.Name = "mnuTavanirReports"
        Me.mnuTavanirReports.Size = New System.Drawing.Size(206, 22)
        Me.mnuTavanirReports.Text = "(14) گزارشات طرح‌های توانير"
        '
        'mnuReport_14_1
        '
        Me.mnuReport_14_1.FavVisible = False
        Me.mnuReport_14_1.Image = Global.Havades_App.Resources.favNo
        Me.mnuReport_14_1.IsChange = False
        Me.mnuReport_14_1.IsFavVisible = True
        Me.mnuReport_14_1.Name = "mnuReport_14_1"
        Me.mnuReport_14_1.Size = New System.Drawing.Size(269, 22)
        Me.mnuReport_14_1.Text = "(14-1) گزارش خدمات سبز به تفکیک پرونده"
        '
        'mnuReport_14_2
        '
        Me.mnuReport_14_2.FavVisible = False
        Me.mnuReport_14_2.Image = CType(resources.GetObject("mnuReport_14_2.Image"), System.Drawing.Image)
        Me.mnuReport_14_2.IsChange = False
        Me.mnuReport_14_2.IsFavVisible = True
        Me.mnuReport_14_2.Name = "mnuReport_14_2"
        Me.mnuReport_14_2.Size = New System.Drawing.Size(269, 22)
        Me.mnuReport_14_2.Text = "(14-2) گزارش آماری خدمات سبز"
        '
        'mnuReport_14_3
        '
        Me.mnuReport_14_3.FavVisible = False
        Me.mnuReport_14_3.Image = CType(resources.GetObject("mnuReport_14_3.Image"), System.Drawing.Image)
        Me.mnuReport_14_3.IsChange = False
        Me.mnuReport_14_3.IsFavVisible = True
        Me.mnuReport_14_3.Name = "mnuReport_14_3"
        Me.mnuReport_14_3.Size = New System.Drawing.Size(269, 22)
        Me.mnuReport_14_3.Text = "(14-3) گزارش طرح سيما"
        '
        'mnuReport_14_4
        '
        Me.mnuReport_14_4.FavVisible = False
        Me.mnuReport_14_4.Image = CType(resources.GetObject("mnuReport_14_4.Image"), System.Drawing.Image)
        Me.mnuReport_14_4.IsChange = False
        Me.mnuReport_14_4.IsFavVisible = True
        Me.mnuReport_14_4.Name = "mnuReport_14_4"
        Me.mnuReport_14_4.Size = New System.Drawing.Size(269, 22)
        Me.mnuReport_14_4.Text = "(14-4) گزارش آماري خدمات سبز نمونه 2"
        '
        'mnuSaveSeparator
        '
        Me.mnuSaveSeparator.Name = "mnuSaveSeparator"
        Me.mnuSaveSeparator.Size = New System.Drawing.Size(203, 6)
        '
        'mnuSimaPortal
        '
        Me.mnuSimaPortal.Name = "mnuSimaPortal"
        Me.mnuSimaPortal.Size = New System.Drawing.Size(206, 22)
        Me.mnuSimaPortal.Text = "پورتال طرح سیما"
        Me.mnuSimaPortal.Visible = False
        '
        'mnuSepMisc
        '
        Me.mnuSepMisc.Name = "mnuSepMisc"
        Me.mnuSepMisc.Size = New System.Drawing.Size(203, 6)
        Me.mnuSepMisc.Visible = False
        '
        'mnuSaveFav
        '
        Me.mnuSaveFav.Image = CType(resources.GetObject("mnuSaveFav.Image"), System.Drawing.Image)
        Me.mnuSaveFav.Name = "mnuSaveFav"
        Me.mnuSaveFav.Size = New System.Drawing.Size(206, 22)
        Me.mnuSaveFav.Text = "ذخيره علاقه مندي ها"
        '
        'MenuBaseTables
        '
        Me.MenuBaseTables.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuItem84, Me.MenuItem83, Me.MenuItem27, Me.ToolStripSeparator32, Me.MenuItem8, Me.MenuItem5, Me.ToolStripSeparator33, Me.MenuItem9, Me.MenuItem29, Me.ToolStripSeparator73, Me.mnuGenericParts, Me.MenuItem30, Me.mnuFactory, Me.ToolStripSeparator38, Me.mnuFrmNetworkMonthlyInfo, Me.mnuFrmGoal, Me.ToolStripSeparator39, Me.MenuItem48, Me.MenuItem14, Me.MenuItem25, Me.MenuItem46, Me.ToolStripSeparator40, Me.mnuDisconnectGroupSetGroup, Me.ToolStripSeparator41, Me.MenuItem32, Me.MenuButtonEkipProfile, Me.mnuAVLCar, Me.mnuAVLState, Me.ToolStripSeparator42, Me.mnuQuestion, Me.ToolStripSeparator44, Me.mnuSpec, Me.ManuOtherBase, Me.MenuErjaBase})
        Me.MenuBaseTables.Name = "MenuBaseTables"
        Me.MenuBaseTables.Size = New System.Drawing.Size(74, 20)
        Me.MenuBaseTables.Text = "اطلاعات &پايه"
        '
        'MenuItem84
        '
        Me.MenuItem84.Name = "MenuItem84"
        Me.MenuItem84.Size = New System.Drawing.Size(257, 22)
        Me.MenuItem84.Text = "ا&ستان ها"
        '
        'MenuItem83
        '
        Me.MenuItem83.Name = "MenuItem83"
        Me.MenuItem83.Size = New System.Drawing.Size(257, 22)
        Me.MenuItem83.Text = "&شهرستان ها"
        '
        'MenuItem27
        '
        Me.MenuItem27.Name = "MenuItem27"
        Me.MenuItem27.Size = New System.Drawing.Size(257, 22)
        Me.MenuItem27.Text = "&نواحي"
        '
        'ToolStripSeparator32
        '
        Me.ToolStripSeparator32.Name = "ToolStripSeparator32"
        Me.ToolStripSeparator32.Size = New System.Drawing.Size(254, 6)
        '
        'MenuItem8
        '
        Me.MenuItem8.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuItem11, Me.MenuItem67, Me.mnuUseType, Me.MenuItem60, Me.mnuSection, Me.mnuTown, Me.mnuVillage, Me.mnuRoosta, Me.mnuIVRCodes})
        Me.MenuItem8.Name = "MenuItem8"
        Me.MenuItem8.Size = New System.Drawing.Size(257, 22)
        Me.MenuItem8.Text = "&مشتركين"
        '
        'MenuItem11
        '
        Me.MenuItem11.Image = CType(resources.GetObject("MenuItem11.Image"), System.Drawing.Image)
        Me.MenuItem11.Name = "MenuItem11"
        Me.MenuItem11.Size = New System.Drawing.Size(165, 22)
        Me.MenuItem11.Text = "م&شتركين"
        '
        'MenuItem67
        '
        Me.MenuItem67.Name = "MenuItem67"
        Me.MenuItem67.Size = New System.Drawing.Size(165, 22)
        Me.MenuItem67.Text = "مناطق"
        '
        'mnuUseType
        '
        Me.mnuUseType.Name = "mnuUseType"
        Me.mnuUseType.Size = New System.Drawing.Size(165, 22)
        Me.mnuUseType.Text = "نوع مصرف"
        '
        'MenuItem60
        '
        Me.MenuItem60.Name = "MenuItem60"
        Me.MenuItem60.Size = New System.Drawing.Size(165, 22)
        Me.MenuItem60.Text = "&كد پستي مشتركين"
        Me.MenuItem60.Visible = False
        '
        'mnuSection
        '
        Me.mnuSection.Name = "mnuSection"
        Me.mnuSection.Size = New System.Drawing.Size(165, 22)
        Me.mnuSection.Text = "بخش"
        '
        'mnuTown
        '
        Me.mnuTown.Name = "mnuTown"
        Me.mnuTown.Size = New System.Drawing.Size(165, 22)
        Me.mnuTown.Text = "شهر"
        '
        'mnuVillage
        '
        Me.mnuVillage.Name = "mnuVillage"
        Me.mnuVillage.Size = New System.Drawing.Size(165, 22)
        Me.mnuVillage.Text = "دهستان"
        '
        'mnuRoosta
        '
        Me.mnuRoosta.Name = "mnuRoosta"
        Me.mnuRoosta.Size = New System.Drawing.Size(165, 22)
        Me.mnuRoosta.Text = "روستا"
        '
        'mnuIVRCodes
        '
        Me.mnuIVRCodes.Name = "mnuIVRCodes"
        Me.mnuIVRCodes.Size = New System.Drawing.Size(165, 22)
        Me.mnuIVRCodes.Text = "تعریف کدهای IVR"
        '
        'MenuItem5
        '
        Me.MenuItem5.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuItem7, Me.MenuItem38, Me.MenuItemPublicTelNumbers})
        Me.MenuItem5.Name = "MenuItem5"
        Me.MenuItem5.Size = New System.Drawing.Size(257, 22)
        Me.MenuItem5.Text = "دفتر &تلفن"
        '
        'MenuItem7
        '
        Me.MenuItem7.Name = "MenuItem7"
        Me.MenuItem7.Size = New System.Drawing.Size(210, 22)
        Me.MenuItem7.Text = "&وظيفه"
        '
        'MenuItem38
        '
        Me.MenuItem38.Name = "MenuItem38"
        Me.MenuItem38.Size = New System.Drawing.Size(210, 22)
        Me.MenuItem38.Text = "نوع &تماس"
        '
        'MenuItemPublicTelNumbers
        '
        Me.MenuItemPublicTelNumbers.Name = "MenuItemPublicTelNumbers"
        Me.MenuItemPublicTelNumbers.Size = New System.Drawing.Size(210, 22)
        Me.MenuItemPublicTelNumbers.Text = "شماره باجه تلفن‌هاي عمومي"
        '
        'ToolStripSeparator33
        '
        Me.ToolStripSeparator33.Name = "ToolStripSeparator33"
        Me.ToolStripSeparator33.Size = New System.Drawing.Size(254, 6)
        '
        'MenuItem9
        '
        Me.MenuItem9.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuItem51, Me.MenuItem59, Me.ToolStripSeparator34, Me.MenuButtonItem26})
        Me.MenuItem9.Name = "MenuItem9"
        Me.MenuItem9.Size = New System.Drawing.Size(257, 22)
        Me.MenuItem9.Text = "شبكه فشار &ضعيف"
        '
        'MenuItem51
        '
        Me.MenuItem51.Name = "MenuItem51"
        Me.MenuItem51.Size = New System.Drawing.Size(214, 22)
        Me.MenuItem51.Text = "&فيدر هاي فشار ضعيف"
        '
        'MenuItem59
        '
        Me.MenuItem59.Name = "MenuItem59"
        Me.MenuItem59.Size = New System.Drawing.Size(214, 22)
        Me.MenuItem59.Text = "فيدر هاي &مشترك فشار ضعيف"
        '
        'ToolStripSeparator34
        '
        Me.ToolStripSeparator34.Name = "ToolStripSeparator34"
        Me.ToolStripSeparator34.Size = New System.Drawing.Size(211, 6)
        '
        'MenuButtonItem26
        '
        Me.MenuButtonItem26.Name = "MenuButtonItem26"
        Me.MenuButtonItem26.Size = New System.Drawing.Size(214, 22)
        Me.MenuButtonItem26.Text = "آمپراژ فيوز"
        '
        'MenuItem29
        '
        Me.MenuItem29.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuItem70, Me.MenuButtonItem44, Me.ToolStripSeparator35, Me.MenuItem69, Me.MenuItem68, Me.mnuFeederPart, Me.mnuMPFeederKey, Me.ToolStripSeparator36, Me.MenuItem50, Me.MenuItem3, Me.mnuLPTrans, Me.mnuLPTransInstaller, Me.ToolStripSeparator37, Me.MenuButtonItem34})
        Me.MenuItem29.Name = "MenuItem29"
        Me.MenuItem29.Size = New System.Drawing.Size(257, 22)
        Me.MenuItem29.Text = "شبكه فشار متوسط"
        '
        'MenuItem70
        '
        Me.MenuItem70.Name = "MenuItem70"
        Me.MenuItem70.Size = New System.Drawing.Size(235, 22)
        Me.MenuItem70.Text = "پست هاي &فوق توزيع"
        '
        'MenuButtonItem44
        '
        Me.MenuButtonItem44.Name = "MenuButtonItem44"
        Me.MenuButtonItem44.Size = New System.Drawing.Size(235, 22)
        Me.MenuButtonItem44.Text = "نوع پست فوق توزيع"
        '
        'ToolStripSeparator35
        '
        Me.ToolStripSeparator35.Name = "ToolStripSeparator35"
        Me.ToolStripSeparator35.Size = New System.Drawing.Size(232, 6)
        '
        'MenuItem69
        '
        Me.MenuItem69.Name = "MenuItem69"
        Me.MenuItem69.Size = New System.Drawing.Size(235, 22)
        Me.MenuItem69.Text = "&فيدر هاي فشار متوسط"
        '
        'MenuItem68
        '
        Me.MenuItem68.Name = "MenuItem68"
        Me.MenuItem68.Size = New System.Drawing.Size(235, 22)
        Me.MenuItem68.Text = "فيدر هاي &مشترك فشار متوسط"
        '
        'mnuFeederPart
        '
        Me.mnuFeederPart.Name = "mnuFeederPart"
        Me.mnuFeederPart.Size = New System.Drawing.Size(235, 22)
        Me.mnuFeederPart.Text = "تکه فيدرهاي فشار متوسط"
        '
        'mnuMPFeederKey
        '
        Me.mnuMPFeederKey.Name = "mnuMPFeederKey"
        Me.mnuMPFeederKey.Size = New System.Drawing.Size(235, 22)
        Me.mnuMPFeederKey.Text = "کليد فيدرهاي فشار متوسط"
        '
        'ToolStripSeparator36
        '
        Me.ToolStripSeparator36.Name = "ToolStripSeparator36"
        Me.ToolStripSeparator36.Size = New System.Drawing.Size(232, 6)
        '
        'MenuItem50
        '
        Me.MenuItem50.Name = "MenuItem50"
        Me.MenuItem50.Size = New System.Drawing.Size(235, 22)
        Me.MenuItem50.Text = "پست هاي &توزيع"
        '
        'MenuItem3
        '
        Me.MenuItem3.Name = "MenuItem3"
        Me.MenuItem3.Size = New System.Drawing.Size(235, 22)
        Me.MenuItem3.Text = "نوع پست تو&زيع"
        '
        'mnuLPTrans
        '
        Me.mnuLPTrans.Name = "mnuLPTrans"
        Me.mnuLPTrans.Size = New System.Drawing.Size(235, 22)
        Me.mnuLPTrans.Text = "ترانس‌هاي پست‌هاي توزيع"
        '
        'mnuLPTransInstaller
        '
        Me.mnuLPTransInstaller.Name = "mnuLPTransInstaller"
        Me.mnuLPTransInstaller.Size = New System.Drawing.Size(235, 22)
        Me.mnuLPTransInstaller.Text = "واحد های مجری نصب ترانس"
        '
        'ToolStripSeparator37
        '
        Me.ToolStripSeparator37.Name = "ToolStripSeparator37"
        Me.ToolStripSeparator37.Size = New System.Drawing.Size(232, 6)
        '
        'MenuButtonItem34
        '
        Me.MenuButtonItem34.Name = "MenuButtonItem34"
        Me.MenuButtonItem34.Size = New System.Drawing.Size(235, 22)
        Me.MenuButtonItem34.Text = "کارخانه سازنده ترانسهاي فوق توزيع"
        '
        'ToolStripSeparator73
        '
        Me.ToolStripSeparator73.Name = "ToolStripSeparator73"
        Me.ToolStripSeparator73.Size = New System.Drawing.Size(254, 6)
        '
        'mnuGenericParts
        '
        Me.mnuGenericParts.Name = "mnuGenericParts"
        Me.mnuGenericParts.Size = New System.Drawing.Size(257, 22)
        Me.mnuGenericParts.Text = "مدیریت تجهیزات انبارک"
        '
        'MenuItem30
        '
        Me.MenuItem30.Name = "MenuItem30"
        Me.MenuItem30.Size = New System.Drawing.Size(257, 22)
        Me.MenuItem30.Text = "&واحدهاي تجهيزات"
        '
        'mnuFactory
        '
        Me.mnuFactory.Name = "mnuFactory"
        Me.mnuFactory.Size = New System.Drawing.Size(257, 22)
        Me.mnuFactory.Text = "کارخانه‌هاي سازنده تجهيزات"
        '
        'ToolStripSeparator38
        '
        Me.ToolStripSeparator38.Name = "ToolStripSeparator38"
        Me.ToolStripSeparator38.Size = New System.Drawing.Size(254, 6)
        '
        'mnuFrmNetworkMonthlyInfo
        '
        Me.mnuFrmNetworkMonthlyInfo.Name = "mnuFrmNetworkMonthlyInfo"
        Me.mnuFrmNetworkMonthlyInfo.Size = New System.Drawing.Size(257, 22)
        Me.mnuFrmNetworkMonthlyInfo.Text = "اطلاعات آماري ماهيانه شبکه توزيع"
        '
        'mnuFrmGoal
        '
        Me.mnuFrmGoal.Name = "mnuFrmGoal"
        Me.mnuFrmGoal.Size = New System.Drawing.Size(257, 22)
        Me.mnuFrmGoal.Text = "تعيين اهداف ماهيانه شاخص هاي انرژي"
        '
        'ToolStripSeparator39
        '
        Me.ToolStripSeparator39.Name = "ToolStripSeparator39"
        Me.ToolStripSeparator39.Size = New System.Drawing.Size(254, 6)
        '
        'MenuItem48
        '
        Me.MenuItem48.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFTReason, Me.MenuItem40, Me.MenuItem41, Me.MenuItem71, Me.MenuItem94, Me.MenuButtonItemTbl_MPTamirDisconnectFor, Me.MenuButtonItemTbl_MPTamirRequestType, Me.MenuButtonItemTbl_DisconnectMPRequestFor, Me.MenuButtonItem7, Me.MenuButtonItem8})
        Me.MenuItem48.Name = "MenuItem48"
        Me.MenuItem48.Size = New System.Drawing.Size(257, 22)
        Me.MenuItem48.Text = "رويدادهاي فشار &متوسط"
        '
        'mnuFTReason
        '
        Me.mnuFTReason.Name = "mnuFTReason"
        Me.mnuFTReason.Size = New System.Drawing.Size(253, 22)
        Me.mnuFTReason.Text = "انواع علت درخواست فوق توزيع"
        '
        'MenuItem40
        '
        Me.MenuItem40.Enabled = False
        Me.MenuItem40.Name = "MenuItem40"
        Me.MenuItem40.Size = New System.Drawing.Size(253, 22)
        Me.MenuItem40.Text = "&علل خاموشي"
        Me.MenuItem40.Visible = False
        '
        'MenuItem41
        '
        Me.MenuItem41.Enabled = False
        Me.MenuItem41.Name = "MenuItem41"
        Me.MenuItem41.Size = New System.Drawing.Size(253, 22)
        Me.MenuItem41.Text = "عوا&مل خاموشي"
        Me.MenuItem41.Visible = False
        '
        'MenuItem71
        '
        Me.MenuItem71.Name = "MenuItem71"
        Me.MenuItem71.Size = New System.Drawing.Size(253, 22)
        Me.MenuItem71.Text = "علل &قطع فشار متوسط"
        '
        'MenuItem94
        '
        Me.MenuItem94.Enabled = False
        Me.MenuItem94.Name = "MenuItem94"
        Me.MenuItem94.Size = New System.Drawing.Size(253, 22)
        Me.MenuItem94.Text = "عناوين &كلي علل قطع"
        Me.MenuItem94.Visible = False
        '
        'MenuButtonItemTbl_MPTamirDisconnectFor
        '
        Me.MenuButtonItemTbl_MPTamirDisconnectFor.Name = "MenuButtonItemTbl_MPTamirDisconnectFor"
        Me.MenuButtonItemTbl_MPTamirDisconnectFor.Size = New System.Drawing.Size(253, 22)
        Me.MenuButtonItemTbl_MPTamirDisconnectFor.Text = "نوع در خواست قطع براي رويداد با برنامه"
        '
        'MenuButtonItemTbl_MPTamirRequestType
        '
        Me.MenuButtonItemTbl_MPTamirRequestType.Name = "MenuButtonItemTbl_MPTamirRequestType"
        Me.MenuButtonItemTbl_MPTamirRequestType.Size = New System.Drawing.Size(253, 22)
        Me.MenuButtonItemTbl_MPTamirRequestType.Text = "نوع درخواست"
        '
        'MenuButtonItemTbl_DisconnectMPRequestFor
        '
        Me.MenuButtonItemTbl_DisconnectMPRequestFor.Name = "MenuButtonItemTbl_DisconnectMPRequestFor"
        Me.MenuButtonItemTbl_DisconnectMPRequestFor.Size = New System.Drawing.Size(253, 22)
        Me.MenuButtonItemTbl_DisconnectMPRequestFor.Text = "درخواست قطع براي"
        '
        'MenuButtonItem7
        '
        Me.MenuButtonItem7.Name = "MenuButtonItem7"
        Me.MenuButtonItem7.Size = New System.Drawing.Size(253, 22)
        Me.MenuButtonItem7.Text = "نوع قطع كننده (انواع کليد)"
        '
        'MenuButtonItem8
        '
        Me.MenuButtonItem8.Name = "MenuButtonItem8"
        Me.MenuButtonItem8.Size = New System.Drawing.Size(253, 22)
        Me.MenuButtonItem8.Text = "نوع فالت"
        '
        'MenuItem14
        '
        Me.MenuItem14.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuItem52, Me.MenuButtonItemTbl_LPCloserType, Me.MenuButtonItemTbl_LPFaultType, Me.MenuButtonItemTbl_LPTamirDisconnectFor, Me.MenuButtonItemTbl_LPTamirRequestType, Me.MenuButtonItemTbl_DisconnectLPRequestFor, Me.MenuItem44})
        Me.MenuItem14.Name = "MenuItem14"
        Me.MenuItem14.Size = New System.Drawing.Size(257, 22)
        Me.MenuItem14.Text = "&رويدادهاي فشار ضعيف"
        '
        'MenuItem52
        '
        Me.MenuItem52.Name = "MenuItem52"
        Me.MenuItem52.Size = New System.Drawing.Size(253, 22)
        Me.MenuItem52.Text = "&علت تماس مشتركين"
        '
        'MenuButtonItemTbl_LPCloserType
        '
        Me.MenuButtonItemTbl_LPCloserType.Name = "MenuButtonItemTbl_LPCloserType"
        Me.MenuButtonItemTbl_LPCloserType.Size = New System.Drawing.Size(253, 22)
        Me.MenuButtonItemTbl_LPCloserType.Text = "نوع قطع كننده (انواع کليد)"
        '
        'MenuButtonItemTbl_LPFaultType
        '
        Me.MenuButtonItemTbl_LPFaultType.Name = "MenuButtonItemTbl_LPFaultType"
        Me.MenuButtonItemTbl_LPFaultType.Size = New System.Drawing.Size(253, 22)
        Me.MenuButtonItemTbl_LPFaultType.Text = "نوع فالت"
        '
        'MenuButtonItemTbl_LPTamirDisconnectFor
        '
        Me.MenuButtonItemTbl_LPTamirDisconnectFor.Name = "MenuButtonItemTbl_LPTamirDisconnectFor"
        Me.MenuButtonItemTbl_LPTamirDisconnectFor.Size = New System.Drawing.Size(253, 22)
        Me.MenuButtonItemTbl_LPTamirDisconnectFor.Text = "نوع در خواست قطع براي رويداد با برنامه"
        '
        'MenuButtonItemTbl_LPTamirRequestType
        '
        Me.MenuButtonItemTbl_LPTamirRequestType.Name = "MenuButtonItemTbl_LPTamirRequestType"
        Me.MenuButtonItemTbl_LPTamirRequestType.Size = New System.Drawing.Size(253, 22)
        Me.MenuButtonItemTbl_LPTamirRequestType.Text = "نوع در خواست  براي رويداد با برنامه"
        '
        'MenuButtonItemTbl_DisconnectLPRequestFor
        '
        Me.MenuButtonItemTbl_DisconnectLPRequestFor.Name = "MenuButtonItemTbl_DisconnectLPRequestFor"
        Me.MenuButtonItemTbl_DisconnectLPRequestFor.Size = New System.Drawing.Size(253, 22)
        Me.MenuButtonItemTbl_DisconnectLPRequestFor.Text = "درخواست قطع براي"
        '
        'MenuItem44
        '
        Me.MenuItem44.Name = "MenuItem44"
        Me.MenuItem44.Size = New System.Drawing.Size(253, 22)
        Me.MenuItem44.Text = "علل ق&طع فشار ضعيف"
        '
        'MenuItem25
        '
        Me.MenuItem25.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuItem63, Me.MenuButtonItemTbl_DisconnectLightRequestFor, Me.MenuButtonItem30, Me.MenuItem64})
        Me.MenuItem25.Name = "MenuItem25"
        Me.MenuItem25.Size = New System.Drawing.Size(257, 22)
        Me.MenuItem25.Text = "رويدادهاي روشنايي م&عابر"
        '
        'MenuItem63
        '
        Me.MenuItem63.Name = "MenuItem63"
        Me.MenuItem63.Size = New System.Drawing.Size(185, 22)
        Me.MenuItem63.Text = "&علت تماس مشتركين"
        '
        'MenuButtonItemTbl_DisconnectLightRequestFor
        '
        Me.MenuButtonItemTbl_DisconnectLightRequestFor.Name = "MenuButtonItemTbl_DisconnectLightRequestFor"
        Me.MenuButtonItemTbl_DisconnectLightRequestFor.Size = New System.Drawing.Size(185, 22)
        Me.MenuButtonItemTbl_DisconnectLightRequestFor.Text = "درخواست قطع براي"
        '
        'MenuButtonItem30
        '
        Me.MenuButtonItem30.Name = "MenuButtonItem30"
        Me.MenuButtonItem30.Size = New System.Drawing.Size(185, 22)
        Me.MenuButtonItem30.Text = "علل قطع روشنايي معابر"
        '
        'MenuItem64
        '
        Me.MenuItem64.Name = "MenuItem64"
        Me.MenuItem64.Size = New System.Drawing.Size(185, 22)
        Me.MenuItem64.Text = "تجهيزات روشنايي &معابر"
        Me.MenuItem64.Visible = False
        '
        'MenuItem46
        '
        Me.MenuItem46.Enabled = False
        Me.MenuItem46.Name = "MenuItem46"
        Me.MenuItem46.Size = New System.Drawing.Size(257, 22)
        Me.MenuItem46.Text = "علل &خاموشي"
        Me.MenuItem46.Visible = False
        '
        'ToolStripSeparator40
        '
        Me.ToolStripSeparator40.Name = "ToolStripSeparator40"
        Me.ToolStripSeparator40.Size = New System.Drawing.Size(254, 6)
        '
        'mnuDisconnectGroupSetGroup
        '
        Me.mnuDisconnectGroupSetGroup.Name = "mnuDisconnectGroupSetGroup"
        Me.mnuDisconnectGroupSetGroup.Size = New System.Drawing.Size(257, 22)
        Me.mnuDisconnectGroupSetGroup.Text = "گروه بندي اشکالات بوجود آمده"
        '
        'ToolStripSeparator41
        '
        Me.ToolStripSeparator41.Name = "ToolStripSeparator41"
        Me.ToolStripSeparator41.Size = New System.Drawing.Size(254, 6)
        '
        'MenuItem32
        '
        Me.MenuItem32.Name = "MenuItem32"
        Me.MenuItem32.Size = New System.Drawing.Size(257, 22)
        Me.MenuItem32.Text = "تکنسين‌هاي بازديد"
        '
        'MenuButtonEkipProfile
        '
        Me.MenuButtonEkipProfile.Name = "MenuButtonEkipProfile"
        Me.MenuButtonEkipProfile.Size = New System.Drawing.Size(257, 22)
        Me.MenuButtonEkipProfile.Text = "اكيپ ها"
        '
        'mnuAVLCar
        '
        Me.mnuAVLCar.Name = "mnuAVLCar"
        Me.mnuAVLCar.Size = New System.Drawing.Size(257, 22)
        Me.mnuAVLCar.Text = "خودروها"
        '
        'mnuAVLState
        '
        Me.mnuAVLState.Name = "mnuAVLState"
        Me.mnuAVLState.Size = New System.Drawing.Size(257, 22)
        Me.mnuAVLState.Text = "وضعيت خودروها"
        '
        'ToolStripSeparator42
        '
        Me.ToolStripSeparator42.Name = "ToolStripSeparator42"
        Me.ToolStripSeparator42.Size = New System.Drawing.Size(254, 6)
        '
        'mnuQuestion
        '
        Me.mnuQuestion.Name = "mnuQuestion"
        Me.mnuQuestion.Size = New System.Drawing.Size(257, 22)
        Me.mnuQuestion.Text = "پرسشنامه"
        '
        'ToolStripSeparator44
        '
        Me.ToolStripSeparator44.Name = "ToolStripSeparator44"
        Me.ToolStripSeparator44.Size = New System.Drawing.Size(254, 6)
        '
        'mnuSpec
        '
        Me.mnuSpec.Name = "mnuSpec"
        Me.mnuSpec.Size = New System.Drawing.Size(257, 22)
        Me.mnuSpec.Text = "اطلاعات پايه عمومي"
        '
        'ManuOtherBase
        '
        Me.ManuOtherBase.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuItem39, Me.MenuItem82, Me.MenuItem34, Me.MenuButtonItem35, Me.MenuItem42, Me.MenuButtonItem22, Me.MenuItem2, Me.MenuItemGroupPart, Me.MenuItem36, Me.mnuAllowValidator, Me.mnulSpecialCallType})
        Me.ManuOtherBase.Name = "ManuOtherBase"
        Me.ManuOtherBase.Size = New System.Drawing.Size(257, 22)
        Me.ManuOtherBase.Text = "ساير اطلاعات پايه"
        '
        'MenuItem39
        '
        Me.MenuItem39.Name = "MenuItem39"
        Me.MenuItem39.Size = New System.Drawing.Size(276, 22)
        Me.MenuItem39.Text = "واحدهاي اجرايي"
        '
        'MenuItem82
        '
        Me.MenuItem82.Name = "MenuItem82"
        Me.MenuItem82.Size = New System.Drawing.Size(276, 22)
        Me.MenuItem82.Text = "نحوه ا&طلاع"
        '
        'MenuItem34
        '
        Me.MenuItem34.Name = "MenuItem34"
        Me.MenuItem34.Size = New System.Drawing.Size(276, 22)
        Me.MenuItem34.Text = "وا&حدهاي متقاضي خاموشي بابرنامه"
        '
        'MenuButtonItem35
        '
        Me.MenuButtonItem35.Name = "MenuButtonItem35"
        Me.MenuButtonItem35.Size = New System.Drawing.Size(276, 22)
        Me.MenuButtonItem35.Text = "واحدهاي متقاضي (فوق توزيع)"
        '
        'MenuItem42
        '
        Me.MenuItem42.Name = "MenuItem42"
        Me.MenuItem42.Size = New System.Drawing.Size(276, 22)
        Me.MenuItem42.Text = "علت ع&دم انجام كار"
        '
        'MenuButtonItem22
        '
        Me.MenuButtonItem22.Name = "MenuButtonItem22"
        Me.MenuButtonItem22.Size = New System.Drawing.Size(276, 22)
        Me.MenuButtonItem22.Text = "سطح مقطع"
        '
        'MenuItem2
        '
        Me.MenuItem2.Name = "MenuItem2"
        Me.MenuItem2.Size = New System.Drawing.Size(276, 22)
        Me.MenuItem2.Text = "شرايط &جوي"
        '
        'MenuItemGroupPart
        '
        Me.MenuItemGroupPart.Name = "MenuItemGroupPart"
        Me.MenuItemGroupPart.Size = New System.Drawing.Size(276, 22)
        Me.MenuItemGroupPart.Text = "گروه بندي تجهيزات سرقتي توانير در فرم 3-4"
        '
        'MenuItem36
        '
        Me.MenuItem36.Enabled = False
        Me.MenuItem36.Name = "MenuItem36"
        Me.MenuItem36.Size = New System.Drawing.Size(276, 22)
        Me.MenuItem36.Text = "وضعيت ان&جام كار"
        Me.MenuItem36.Visible = False
        '
        'mnuAllowValidator
        '
        Me.mnuAllowValidator.Name = "mnuAllowValidator"
        Me.mnuAllowValidator.Size = New System.Drawing.Size(276, 22)
        Me.mnuAllowValidator.Text = "مشخصات صادر/ابطال کننده اجازه کار"
        '
        'mnulSpecialCallType
        '
        Me.mnulSpecialCallType.Name = "mnulSpecialCallType"
        Me.mnulSpecialCallType.Size = New System.Drawing.Size(276, 22)
        Me.mnulSpecialCallType.Text = "نوع خاص بودن مکالمات"
        '
        'MenuErjaBase
        '
        Me.MenuErjaBase.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuErjaReasonBase, Me.menuErjaOperations})
        Me.MenuErjaBase.Name = "MenuErjaBase"
        Me.MenuErjaBase.Size = New System.Drawing.Size(257, 22)
        Me.MenuErjaBase.Text = "اطلاعات پايه ارجاع"
        Me.MenuErjaBase.Visible = False
        '
        'mnuErjaReasonBase
        '
        Me.mnuErjaReasonBase.Name = "mnuErjaReasonBase"
        Me.mnuErjaReasonBase.Size = New System.Drawing.Size(126, 22)
        Me.mnuErjaReasonBase.Text = "دلايل ارجاع"
        '
        'menuErjaOperations
        '
        Me.menuErjaOperations.Name = "menuErjaOperations"
        Me.menuErjaOperations.Size = New System.Drawing.Size(126, 22)
        Me.menuErjaOperations.Text = "فهرست بها"
        '
        'MenuTools
        '
        Me.MenuTools.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuConfig, Me.MenuToolsChangePassword, Me.mnuHavadesConfig, Me.mnuSecuritySetting, Me.ToolStripSeparator29, Me.MenuToolsUserManagement, Me.mnuGroupTrustee, Me.ToolStripSeparator30, Me.mnuLoginLog, Me.mnuLoginFailed, Me.MnuUISkin, Me.mnuNewRequestConfig, Me.mnuIsKeyboardBase, Me.ToolStripSeparator31, Me.MenuConfiguration, Me.MenuShowInfo, Me.MenuButtonItemGlobalValue, Me.MenuButtonItem6, Me.mnuShowLSLogSettings})
        Me.MenuTools.Name = "MenuTools"
        Me.MenuTools.Size = New System.Drawing.Size(51, 20)
        Me.MenuTools.Text = "&سامانه"
        '
        'mnuConfig
        '
        Me.mnuConfig.Image = CType(resources.GetObject("mnuConfig.Image"), System.Drawing.Image)
        Me.mnuConfig.Name = "mnuConfig"
        Me.mnuConfig.Size = New System.Drawing.Size(267, 22)
        Me.mnuConfig.Text = "تنظيمات کلي سامانه"
        '
        'MenuToolsChangePassword
        '
        Me.MenuToolsChangePassword.Image = CType(resources.GetObject("MenuToolsChangePassword.Image"), System.Drawing.Image)
        Me.MenuToolsChangePassword.Name = "MenuToolsChangePassword"
        Me.MenuToolsChangePassword.Size = New System.Drawing.Size(267, 22)
        Me.MenuToolsChangePassword.Text = "تنظيمات کاربري"
        '
        'mnuHavadesConfig
        '
        Me.mnuHavadesConfig.Image = CType(resources.GetObject("mnuHavadesConfig.Image"), System.Drawing.Image)
        Me.mnuHavadesConfig.Name = "mnuHavadesConfig"
        Me.mnuHavadesConfig.Size = New System.Drawing.Size(267, 22)
        Me.mnuHavadesConfig.Text = "تنظيمات ثبت خاموشي"
        '
        'mnuSecuritySetting
        '
        Me.mnuSecuritySetting.Image = CType(resources.GetObject("mnuSecuritySetting.Image"), System.Drawing.Image)
        Me.mnuSecuritySetting.Name = "mnuSecuritySetting"
        Me.mnuSecuritySetting.Size = New System.Drawing.Size(267, 22)
        Me.mnuSecuritySetting.Text = "تنظيمات امنیتی"
        Me.mnuSecuritySetting.Visible = False
        '
        'ToolStripSeparator29
        '
        Me.ToolStripSeparator29.Name = "ToolStripSeparator29"
        Me.ToolStripSeparator29.Size = New System.Drawing.Size(264, 6)
        '
        'MenuToolsUserManagement
        '
        Me.MenuToolsUserManagement.Image = CType(resources.GetObject("MenuToolsUserManagement.Image"), System.Drawing.Image)
        Me.MenuToolsUserManagement.Name = "MenuToolsUserManagement"
        Me.MenuToolsUserManagement.Size = New System.Drawing.Size(267, 22)
        Me.MenuToolsUserManagement.Tag = "MenuToolsUserManagement"
        Me.MenuToolsUserManagement.Text = "&مديريت كاربران"
        '
        'mnuGroupTrustee
        '
        Me.mnuGroupTrustee.Image = CType(resources.GetObject("mnuGroupTrustee.Image"), System.Drawing.Image)
        Me.mnuGroupTrustee.Name = "mnuGroupTrustee"
        Me.mnuGroupTrustee.Size = New System.Drawing.Size(267, 22)
        Me.mnuGroupTrustee.Tag = "mnuGroupTrustee"
        Me.mnuGroupTrustee.Text = "تعريف گروه‌هاي دسترسي"
        '
        'ToolStripSeparator30
        '
        Me.ToolStripSeparator30.Name = "ToolStripSeparator30"
        Me.ToolStripSeparator30.Size = New System.Drawing.Size(264, 6)
        '
        'mnuLoginLog
        '
        Me.mnuLoginLog.Image = CType(resources.GetObject("mnuLoginLog.Image"), System.Drawing.Image)
        Me.mnuLoginLog.Name = "mnuLoginLog"
        Me.mnuLoginLog.Size = New System.Drawing.Size(267, 22)
        Me.mnuLoginLog.Text = "تاريخچه ورود به سامانه"
        '
        'mnuLoginFailed
        '
        Me.mnuLoginFailed.Image = CType(resources.GetObject("mnuLoginFailed.Image"), System.Drawing.Image)
        Me.mnuLoginFailed.Name = "mnuLoginFailed"
        Me.mnuLoginFailed.Size = New System.Drawing.Size(267, 22)
        Me.mnuLoginFailed.Text = "گزارش ورودهای ناموفق به سیستم"
        '
        'MnuUISkin
        '
        Me.MnuUISkin.Image = CType(resources.GetObject("MnuUISkin.Image"), System.Drawing.Image)
        Me.MnuUISkin.Name = "MnuUISkin"
        Me.MnuUISkin.Size = New System.Drawing.Size(267, 22)
        Me.MnuUISkin.Text = "نمايش &واسط كاربري"
        '
        'mnuNewRequestConfig
        '
        Me.mnuNewRequestConfig.Image = CType(resources.GetObject("mnuNewRequestConfig.Image"), System.Drawing.Image)
        Me.mnuNewRequestConfig.Name = "mnuNewRequestConfig"
        Me.mnuNewRequestConfig.Size = New System.Drawing.Size(267, 22)
        Me.mnuNewRequestConfig.Text = "تعيين پيش فرض‌هاي ثبت جديد"
        '
        'mnuIsKeyboardBase
        '
        Me.mnuIsKeyboardBase.Name = "mnuIsKeyboardBase"
        Me.mnuIsKeyboardBase.Size = New System.Drawing.Size(267, 22)
        Me.mnuIsKeyboardBase.Text = "ثبت درخواست جديد حساس به صفحه کليد"
        '
        'ToolStripSeparator31
        '
        Me.ToolStripSeparator31.Name = "ToolStripSeparator31"
        Me.ToolStripSeparator31.Size = New System.Drawing.Size(264, 6)
        '
        'MenuConfiguration
        '
        Me.MenuConfiguration.Image = CType(resources.GetObject("MenuConfiguration.Image"), System.Drawing.Image)
        Me.MenuConfiguration.Name = "MenuConfiguration"
        Me.MenuConfiguration.Size = New System.Drawing.Size(267, 22)
        Me.MenuConfiguration.Text = "&پيكربندي نرم افزار"
        '
        'MenuShowInfo
        '
        Me.MenuShowInfo.Name = "MenuShowInfo"
        Me.MenuShowInfo.Size = New System.Drawing.Size(267, 22)
        Me.MenuShowInfo.Text = "نمايش اطلاعات پيکربندي"
        '
        'MenuButtonItemGlobalValue
        '
        Me.MenuButtonItemGlobalValue.Name = "MenuButtonItemGlobalValue"
        Me.MenuButtonItemGlobalValue.Size = New System.Drawing.Size(267, 22)
        Me.MenuButtonItemGlobalValue.Text = "پيكربندي پارامترهاي نرم افزار"
        Me.MenuButtonItemGlobalValue.Visible = False
        '
        'MenuButtonItem6
        '
        Me.MenuButtonItem6.Name = "MenuButtonItem6"
        Me.MenuButtonItem6.Size = New System.Drawing.Size(267, 22)
        Me.MenuButtonItem6.Text = "يكسان سازي كدپيج"
        '
        'mnuShowLSLogSettings
        '
        Me.mnuShowLSLogSettings.Image = CType(resources.GetObject("mnuShowLSLogSettings.Image"), System.Drawing.Image)
        Me.mnuShowLSLogSettings.Name = "mnuShowLSLogSettings"
        Me.mnuShowLSLogSettings.Size = New System.Drawing.Size(267, 22)
        Me.mnuShowLSLogSettings.Text = "تنظيمات Log ضبط مکالمات"
        Me.mnuShowLSLogSettings.Visible = False
        '
        'MenuItem21
        '
        Me.MenuItem21.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuButtonItem1, Me.ToolStripSeparator26, Me.mnuVersionInfo, Me.mnuReportsHelp, Me.ToolStripSeparator27, Me.mnuTip, Me.ToolStripSeparator28, Me.mnuAbout})
        Me.MenuItem21.Name = "MenuItem21"
        Me.MenuItem21.Size = New System.Drawing.Size(48, 20)
        Me.MenuItem21.Text = "&راهنما"
        '
        'MenuButtonItem1
        '
        Me.MenuButtonItem1.Image = CType(resources.GetObject("MenuButtonItem1.Image"), System.Drawing.Image)
        Me.MenuButtonItem1.Name = "MenuButtonItem1"
        Me.MenuButtonItem1.Size = New System.Drawing.Size(172, 22)
        Me.MenuButtonItem1.Text = "&راهنماي نرم افزار"
        '
        'ToolStripSeparator26
        '
        Me.ToolStripSeparator26.Name = "ToolStripSeparator26"
        Me.ToolStripSeparator26.Size = New System.Drawing.Size(169, 6)
        '
        'mnuVersionInfo
        '
        Me.mnuVersionInfo.Image = CType(resources.GetObject("mnuVersionInfo.Image"), System.Drawing.Image)
        Me.mnuVersionInfo.Name = "mnuVersionInfo"
        Me.mnuVersionInfo.Size = New System.Drawing.Size(172, 22)
        Me.mnuVersionInfo.Text = "توضيحات نگارش"
        '
        'mnuReportsHelp
        '
        Me.mnuReportsHelp.Image = CType(resources.GetObject("mnuReportsHelp.Image"), System.Drawing.Image)
        Me.mnuReportsHelp.Name = "mnuReportsHelp"
        Me.mnuReportsHelp.Size = New System.Drawing.Size(172, 22)
        Me.mnuReportsHelp.Text = "راهنماي گزارشات"
        '
        'ToolStripSeparator27
        '
        Me.ToolStripSeparator27.Name = "ToolStripSeparator27"
        Me.ToolStripSeparator27.Size = New System.Drawing.Size(169, 6)
        '
        'mnuTip
        '
        Me.mnuTip.Image = CType(resources.GetObject("mnuTip.Image"), System.Drawing.Image)
        Me.mnuTip.Name = "mnuTip"
        Me.mnuTip.Size = New System.Drawing.Size(172, 22)
        Me.mnuTip.Text = "آخرين مطلب نرم افزار"
        '
        'ToolStripSeparator28
        '
        Me.ToolStripSeparator28.Name = "ToolStripSeparator28"
        Me.ToolStripSeparator28.Size = New System.Drawing.Size(169, 6)
        '
        'mnuAbout
        '
        Me.mnuAbout.Image = CType(resources.GetObject("mnuAbout.Image"), System.Drawing.Image)
        Me.mnuAbout.Name = "mnuAbout"
        Me.mnuAbout.Size = New System.Drawing.Size(172, 22)
        Me.mnuAbout.Text = "در&باره..."
        '
        'ToolStripSeparator43
        '
        Me.ToolStripSeparator43.Name = "ToolStripSeparator43"
        Me.ToolStripSeparator43.Size = New System.Drawing.Size(254, 6)
        Me.ToolStripSeparator43.Visible = False
        '
        'StatusBar1
        '
        Me.StatusBar1.Font = New System.Drawing.Font("Tahoma", 7.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.StatusBar1.Location = New System.Drawing.Point(0, 518)
        Me.StatusBar1.Name = "StatusBar1"
        Me.StatusBar1.Panels.AddRange(New System.Windows.Forms.StatusBarPanel() {Me.sbpUserName, Me.sbpTimers, Me.sbpInfoAll, Me.sbpInfo, Me.sbpArea})
        Me.StatusBar1.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.StatusBar1.ShowPanels = True
        Me.StatusBar1.Size = New System.Drawing.Size(760, 22)
        Me.StatusBar1.TabIndex = 1
        '
        'sbpUserName
        '
        Me.sbpUserName.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.sbpUserName.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring
        Me.sbpUserName.Name = "sbpUserName"
        Me.sbpUserName.Width = 131
        '
        'sbpTimers
        '
        Me.sbpTimers.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.sbpTimers.Name = "sbpTimers"
        Me.sbpTimers.Width = 90
        '
        'sbpInfoAll
        '
        Me.sbpInfoAll.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.sbpInfoAll.Name = "sbpInfoAll"
        Me.sbpInfoAll.Width = 220
        '
        'sbpInfo
        '
        Me.sbpInfo.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.sbpInfo.Name = "sbpInfo"
        Me.sbpInfo.Width = 170
        '
        'sbpArea
        '
        Me.sbpArea.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.sbpArea.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring
        Me.sbpArea.Name = "sbpArea"
        Me.sbpArea.Width = 131
        '
        'lblWorkingProvinceName
        '
        Me.lblWorkingProvinceName.BackColor = System.Drawing.Color.Transparent
        Me.lblWorkingProvinceName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblWorkingProvinceName.Font = New System.Drawing.Font("Titr", 48.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblWorkingProvinceName.ForeColor = System.Drawing.Color.Maroon
        Me.lblWorkingProvinceName.Location = New System.Drawing.Point(0, 0)
        Me.lblWorkingProvinceName.Name = "lblWorkingProvinceName"
        Me.lblWorkingProvinceName.Size = New System.Drawing.Size(760, 540)
        Me.lblWorkingProvinceName.TabIndex = 31
        Me.lblWorkingProvinceName.Tag = ""
        Me.lblWorkingProvinceName.Text = "استان"
        Me.lblWorkingProvinceName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'GroupBox1
        '
        Me.GroupBox1.Location = New System.Drawing.Point(16, 472)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(295, 27)
        Me.GroupBox1.TabIndex = 6
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Visible = False
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(152, 22)
        Me.ToolStripMenuItem2.Text = "1"
        '
        'mnuRecloserReport
        '
        Me.mnuRecloserReport.Image = Global.Havades_App.Resources.search
        Me.mnuRecloserReport.Name = "mnuRecloserReport"
        Me.mnuRecloserReport.ShowShortcutKeys = False
        Me.mnuRecloserReport.Size = New System.Drawing.Size(206, 22)
        Me.mnuRecloserReport.Text = "گزارش عملکرد ريکلوزر"
        '
        'frmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), System.Drawing.Image)
        Me.ClientSize = New System.Drawing.Size(760, 540)
        Me.Controls.Add(Me.tscMain)
        Me.Controls.Add(Me.HardLockUSB)
        Me.Controls.Add(Me.pnlDownloadBox)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.MenuBar1)
        Me.Controls.Add(Me.StatusBar1)
        Me.Controls.Add(Me.lblWorkingProvinceName)
        Me.Controls.Add(Me.GroupBox1)
        Me.HelpMaker.SetHelpNavigator(Me, System.Windows.Forms.HelpNavigator.Topic)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.IsMdiContainer = True
        Me.KeyPreview = True
        Me.MainMenuStrip = Me.MenuBar1
        Me.MinimumSize = New System.Drawing.Size(768, 567)
        Me.Name = "frmMain"
        Me.HlpProvider.SetShowHelp(Me, True)
        Me.HelpMaker.SetShowHelp(Me, True)
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "سيستم مكانيزه حوادث برق - نگارش (84/11/11) 3.0.0"
        Me.tscMain.ContentPanel.ResumeLayout(False)
        Me.tscMain.ContentPanel.PerformLayout
        Me.tscMain.ResumeLayout(False)
        Me.tscMain.PerformLayout
        Me.tlbMain.ResumeLayout(False)
        Me.tlbMain.PerformLayout
        Me.tlbCallCenter.ResumeLayout(False)
        Me.tlbCallCenter.PerformLayout
        Me.tlbDebug.ResumeLayout(False)
        Me.tlbDebug.PerformLayout
        CType(Me.HardLockUSB, System.ComponentModel.ISupportInitialize).EndInit
        Me.GroupBox2.ResumeLayout(False)
        Me.MenuBar1.ResumeLayout(False)
        Me.MenuBar1.PerformLayout
        CType(Me.sbpUserName, System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.sbpTimers, System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.sbpInfoAll, System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.sbpInfo, System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.sbpArea, System.ComponentModel.ISupportInitialize).EndInit
        Me.ResumeLayout(False)
        Me.PerformLayout

    End Sub

#End Region

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "سيستم مكانيزه حوادث برق" '& AppVersion
        lblWorkingProvinceName.Text = CConfig.ReadConfig("ToziName").ToString()
        SetTimerIntervalVariables()
        TimerMessagePooling.Interval = TimerMessagePoolingInterval

        'Dim Key As RegistryKey = Registry.LocalMachine
        m_SkinName = GetRegistryAbd("SkinName", m_SkinName, True)
        ChangeSkin(False)

        Integer.TryParse(GetRegistryAbd("CallPooling", "0", True), CallPoolingIndex)
        If CallPoolingIndex = -1 Then CallPoolingIndex = 0

        Dim lIsKeyboardBase As String
        lIsKeyboardBase = GetRegistryAbd("IsKeyboardBase", "False")
        mIsKeyboardBase = IIf(lIsKeyboardBase = "True", True, False)
        mnuIsKeyboardBase.Checked = mIsKeyboardBase

        'ApplySkin(Me)
        Dim aa As Integer
        For aa = 0 To 3
            Timers(aa) = 0
        Next

        sbpUserName.Text = WorkingUserName & " :كد كاربري فعال"
        sbpArea.Text = WorkingAreaName

        If Not IsAdmin() Then
            MenuToolsUserManagement.Enabled = False
            MenuConfiguration.Enabled = False
            mnuGroupTrustee.Enabled = False
            mnuLoginLog.Enabled = False
            'mnuSecuritySetting.Enabled = False
            mnuLoginFailed.Enabled = False
            'MenuToolsBackupDb.Enabled = False
            'MenuToolsRestoreDb.Enabled = False
        Else
            MenuToolsUserManagement.Enabled = True
            MenuConfiguration.Enabled = True
            mnuGroupTrustee.Enabled = True
            mnuLoginLog.Enabled = True
            'mnuSecuritySetting.Enabled = True
            mnuLoginFailed.Enabled = True
            'MenuToolsBackupDb.Enabled = True
            'MenuToolsRestoreDb.Enabled = True
        End If
        If IsCenter Then
            'MenuItem96.Visible = True
            Me.BackColor = System.Drawing.Color.DarkCyan
            Dim Obj As Object
            Dim i As Integer
            For i = 0 To Me.Controls.Count - 1
                Obj = Me.Controls(i)
                If TypeName(Obj) = "MdiClient" Then
                    Me.Controls(i).BackColor = System.Drawing.Color.DarkCyan
                End If
            Next
            'lblWorkingProvinceName.ForeColor = System.Drawing.Color.LightCyan
            'Me.TransparencyKey = System.Drawing.Color.LightCyan
        End If

        'Initializing Hard Lock Checking Timers:
        InitializeHardLockCheckingTimers()
        CheckUserTrustee(Me, MenuItemMonitoring, 13)
        CheckUserTrustee(Me, MenuItemMonitoringLight, 13)
        tbRequestsPreview.Enabled = IsAdmin() Or CheckUserTrustee("frmMainMenuItemMonitoring", 13)
        tbLightRequestsPreview.Enabled = IsAdmin() Or CheckUserTrustee("frmMainMenuItemMonitoringLight", 13)
        mnuBillBoard.Enabled = IsAdmin() OrElse CheckUserTrustee("MonitoringBillBoard", 13)
        mnuNewRequestConfig.Enabled = IsSetadMode AndAlso IsAdmin()
        'mnuRepLoading.Enabled = IsAdmin() OrElse CheckUserTrustee("CanMakeLoadReport", 32)

        SetVisibleColumnsInDg()

        BindingTable("SELECT * FROM TblReportFavorite WHERE AreaUserId = " & WorkingUserId & " AND ApplicationId = " & mApplicationType.Havades, mCnn, mDs, "TblReportFavorite")
        For Each lRow As DataRow In mDs.Tables("TblReportFavorite").Rows
            SetFavMenuCheck(mnuReports, lRow("MenuName"))
        Next

        Me.Width = Me.Width + 1
        Try
            Dim Ds As New DatasetCcRequester
            Dim Cnn As SqlConnection = New SqlConnection(GetConnection())
            BindingTable("SELECT top 1 * FROM Tbl_Configuration ", Cnn, Ds, "Tbl_Configuration")
            WorkingEnableNetworkAffection4CalX = Ds.Tbl_Configuration.Rows(0).Item("EnableNetworkAffection4CalX")
            WorkingOnePhaseVoltage = Ds.Tbl_Configuration.Rows(0).Item("OnePhaseVoltage")
            WorkingThreePhasesVoltage = Ds.Tbl_Configuration.Rows(0).Item("ThreePhasesVoltage")
            WorkingDefaultCosinPhi = Ds.Tbl_Configuration.Rows(0).Item("DefaultCosinPhi")
        Catch ex As Exception

        End Try

        Try
            Dim Ds1 As New DataSet
            Dim Cnn As SqlConnection = New SqlConnection(GetConnection())
            If BindingTable("SELECT * from  ViewDBVersion", Cnn, Ds1, "ViewDBVersion", , False, False, True) Then
                'lblDBVersion.Text = " نسخه ديتابيس : " & Ds1.Tables("ViewDBVersion").Rows(0).Item("Version") & "  "

                '-- Computing DB Version ( in 2 digits format )
                Dim lDBVer As String
                Dim ldbv1 As String, ldbv2 As String, ldbv3 As String
                Dim lidx1 As Integer, lidx2 As Integer
                lDBVer = Ds1.Tables("ViewDBVersion").Rows(0).Item("Version")
                lDBVer = lDBVer.Substring(1, lDBVer.IndexOf("(") - 1)

                lidx1 = lDBVer.IndexOf(".")
                lidx2 = lDBVer.IndexOf(".", lidx1 + 1)

                ldbv1 = lDBVer.Substring(0, lidx1)
                ldbv2 = lDBVer.Substring(lidx1 + 1, lidx2 - lidx1 - 1)
                ldbv3 = lDBVer.Substring(lidx2 + 1)

                If ldbv1.Length < 2 Then ldbv1 = "0" + ldbv1
                If ldbv2.Length < 2 Then ldbv2 = "0" + ldbv2
                If ldbv3.Length < 2 Then ldbv3 = "0" + ldbv3

                m_DBVersion = ldbv1 + "." + ldbv2 + "." + ldbv3
                '-----------------------
            Else
                If Not BindingTable("SELECT top 0 Server121Id FROM  Tbl_City", Cnn, Ds1, "Tbl_City", , False, False) Then
                    MsgBoxF("آخرين نسخه روزآمد سازي سرور جاري انجام نشده است لطفا با مدير شبكه تماس بگيريد", "هشدار3", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Information, MessageBoxDefaultButton.Button1)
                    Me.Close()
                End If
                If Not BindingTable("SELECT top 0 * FROM  ViewStatRepsDisconnectLPPost", Cnn, Ds1, "ViewControlStatAreaUserDE", , False, False) Then
                    MsgBoxF("آخرين نسخه روزآمد سازي سرور جاري انجام نشده است لطفا با مدير شبكه تماس بگيريد", "هشدار2", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Information, MessageBoxDefaultButton.Button1)
                End If
                If Not BindingTable("SELECT top 0 * FROM  ViewControlStatAreaUserDE", Cnn, Ds1, "ViewControlStatAreaUserDE", , False, False) Then
                    MsgBoxF("آخرين نسخه روزآمد سازي سرور جاري انجام نشده است لطفا با مدير شبكه تماس بگيريد", "هشدار1", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Information, MessageBoxDefaultButton.Button1)
                End If
            End If

        Catch ex As Exception
        End Try

        If Not IsDebugMode() Then
            Dim dlgShift As New frmShift
            Dim lDlgRes As Object
            lDlgRes = dlgShift.ShowDialog()
            If lDlgRes <> DialogResult.OK Then
                SaveLogout()
                dlgShift.Dispose()
                Application.Exit()
                Exit Sub
            End If
            dlgShift.Dispose()
        End If

        Try
            Dim lInstallAppName As InstallAppName
            If IsSetadMode Then
                lInstallAppName = CommonVariables.InstallAppName.BarghSetad
            ElseIf IsMiscMode Then
                lInstallAppName = CommonVariables.InstallAppName.BarghMiscMode
            ElseIf IsCenter Then
                lInstallAppName = CommonVariables.InstallAppName.BarghCenter
            Else
                lInstallAppName = CommonVariables.InstallAppName.BarghNahieh
            End If
            Dim a As New frmDownload()
            pnlDownloadBox.Controls.Add(a)
            a.Dock = DockStyle.Fill
            'SetParent(a.Handle.ToInt64, pnlDownloadBox.Handle.ToInt64)
            'a.Show()
            'a.Left = 0
            'a.Top = 0
            'Dim winampWinEst As WINDOWPLACEMENT
            'GetWindowPlacement(a.Handle.ToInt64, winampWinEst)
            'winampWinEst.rcNormalPosition.Bottom -= winampWinEst.rcNormalPosition.Top
            'winampWinEst.rcNormalPosition.Right_Renamed -= winampWinEst.rcNormalPosition.Left_Renamed
            'winampWinEst.rcNormalPosition.Left_Renamed = 0
            'winampWinEst.rcNormalPosition.Top = 0
            'SetWindowPlacement(a.Handle.ToInt64, winampWinEst)
            mfrmDownload = a
            Me.Focus()
        Catch ex As Exception

        End Try
        'pnlDownloadBox.Visible = False
        'Dim S As New SqlCommand
        'S.Connection = GetConnection()
        'S.CommandText = "zsdl;ksda l;ksda l;"
        'S.ExecuteReader(CommandBehavior.Default)
        SetMenuLocks()

        Try
            If IsActiveCallsServer Then
                MaxShowRequestCount = 5
                If CallPoolingIndex = 2 Or CallPoolingIndex = 3 Then
                    MaxShowRequestCount = 1
                End If

                If Not m_IsTelCMode Then
                    Dim lIsShowLog As String = GetRegistryAbd("HCLSShowLog", "False").ToLower
                    If lIsShowLog = "true" AndAlso IsAdmin() Then
                        mIsShowLogSettings = True
                        mnuShowLSLogSettings.Visible = True
                    End If
                    m_HclsCall = New HclsCall
                    AddHandler m_HclsCall.OnNewCall, AddressOf m_HclsCall_OnNewCall
                    AddHandler m_HclsCall.OnCallerIdChanged, AddressOf m_HclsCall_OnCallerIdChanged
                    AddHandler m_HclsCall.OnRinging, AddressOf m_HclsCall_OnRinging
                    AddHandler m_HclsCall.OnEndCall, AddressOf m_HclsCall_OnEndCall
                ElseIf m_IsTelCMode Then
                    AddHandler mTelCInfo.OnNewCall, AddressOf mTelC_OnNewCall
                    AddHandler mTelCInfo.OnEndCall, AddressOf mTelC_OnEndCall
                End If

                Dim ServerName As String = GetRegistryAbd("CallCenterServerName", , True)

                If Not m_IsTelCMode Then
                    m_HclsCall.BeginInit()
                    Do
                        Application.DoEvents()
                        If Not m_HclsCall.HCLSConnect(ServerName, GetChannelNumber()) Then
                            If MsgBoxF("برقراري ارتباط با ديتابيس سرور مكالمات ميسر نميباشد" & vbCrLf & "امكان پخش مكالمات و نيز ورود اطلاعات از طريق ضبط مكالمات وجود ندارد." & "آيا دوباره سعي شود", "هشدار", MessageBoxButtons.YesNo, MsgBoxIcon.MsgIcon_Question, MessageBoxDefaultButton.Button1) = DialogResult.No Then
                                Exit Do
                            End If
                        Else
                            Exit Do
                        End If
                    Loop
                End If

                Application.DoEvents()
                If m_IsMitelMode Or m_IsTelCMode Then
                    ServerName = GetRegistryAbd("TAPIServerName", , True)
                    If m_IsCallCenterEnable Then
                        If m_IsMitelMode Then
                            If Not m_HclsCall.TAPIConnect(ServerName) Then
                                ''Cannot Find TAPI server 
                                ShowError("خطا به هنگام اتصال به سرور Mitel")
                            End If
                            If Not m_HclsCall.LoginToTAPI(m_MitelAgentId, m_MitelExtension) Then
                                ''Cannot Login To TAPI User
                                ShowError("Cannot Login To Mitel TAPI User")
                            End If
                        Else
                            If Not mTelCInfo.Login(ServerName, m_MitelExtension, m_CRMPsw, False, m_MitelAgentId, "", False) Then
                                ShowError("خطا به هنگام اتصال به سرور " & IIf(m_IsCRMMode, "مرکز تماس", "TelC") & vbCrLf & mTelCInfo.Message)
                            Else
                                m_IsLoginToCallCenter = True
                            End If
                        End If
                    ElseIf Not m_IsTelCMode Then
                        m_HclsCall.SetExtention(m_MitelExtension)
                    End If

                End If
                If Not m_IsTelCMode Then m_HclsCall.EndInit()
            End If

        Catch ex As Exception

        End Try

        If IsSetadMode Then
            lblAppType.Text = "نسخه ستادي"
        ElseIf IsMiscMode Then
            lblAppType.Text = "نسخه ناحيه (متمرکز)"
        ElseIf Not IsCenter Then
            lblAppType.Text = "نسخه ناحيه"
        Else
            lblAppType.Text = "نسخه مرکز 121"
        End If

        If IsSpecialSetadUser() Then
            mnuDeleteRequest.Visible = True
            mnuDeletePostFeeder.Visible = True
            mnuDeleteLPFeeder.Visible = True
            mnuDeleteFeeder.Visible = True
            ToolStripSeparator53.Visible = True
        End If

        If IsErjaAccess Or IsDebugingErja Then
            mnuMonitoringErja.Visible = True
            MenuErjaBase.Visible = True
            Dim lIsAccessToMonitoring As Boolean = CheckUserTrustee("ErjaMonitoring", 27, ApplicationTypes.Erja)
            If Not IsAdmin() And Not lIsAccessToMonitoring Then
                mnuMonitoringErja.Enabled = False
            End If
        End If

        If IsTamirRequestAccess Then
            mnuMonitoringTamir.Visible = True
            Dim lIsAccessToMonitoring As Boolean = CheckUserTrustee("TamirMonitoring", 30, ApplicationTypes.TamirRequest)
            If Not IsAdmin() And Not lIsAccessToMonitoring Then
                mnuMonitoringTamir.Enabled = False
            End If
        End If

        mnuHoma.Visible = False
        mnuHomaMonitoring.Visible = False

        Dim lIsHomaAccess As Boolean = False
        If Convert.ToBoolean(CConfig.ReadConfig("HomaIsActive", "False")) Then
            lIsHomaAccess = True
        End If

        If lIsHomaAccess Then
            If IsSetadMode Then
                mIsHomaAccess = True
            Else
                Dim lHomaWorkingAreas() As String
                Dim lHomaWorkingArea As String = ""
                lHomaWorkingArea = CConfig.ReadConfigText("HomaWorkingAreas", "")
                If String.IsNullOrWhiteSpace(lHomaWorkingArea) Then
                    lHomaWorkingArea = CConfig.ReadConfig("HomaWorkingAreas", "")
                End If
                lHomaWorkingAreas = lHomaWorkingArea.Split(",")
                For Each aArea As String In lHomaWorkingAreas
                    If aArea = WorkingAreaId.ToString() Then
                        mIsHomaAccess = True
                        Exit For
                    End If
                Next
            End If
        End If

        If IsDebugMode() Then
            mIsHomaAccess = True
        End If

        If mIsHomaAccess Then
            mnuHoma.Visible = True
            MenuItemMonitoring.Text = "مانيتورينگ هما"
        End If

        If Not IsSetadMode Then
            mnuRepExccelComunicationSystems.Visible = False
            mnuHavadesConfig.Visible = False
        Else
            If Not IsAdmin() Then
                mnuHavadesConfig.Enabled = False
            End If
            MenuItem86.Visible = False
            If Not IsDebugMode() Then
                tbNewRequest.Visible = False
            End If
            ToolStripSeparator52.Visible = False
        End If


        mnuSubscriberSmsSetting.Visible = False

        If Not IsSetadMode Or Not IsAdministrator() Then
            mnuConfig.Visible = False
            mnuLightRequestSmsSetting.Visible = False
            mnuSubscriberSmsSetting.Visible = False
            ToolStripSeparator62.Visible = False
        ElseIf IsSetadMode Then
            Dim lSMSSubscriber As String = ""
            lSMSSubscriber = CConfig.ReadConfig("SMSSubscriber", "")
            If lSMSSubscriber = "_Active_" Or IsDebugMode() Then
                mnuSubscriberSmsSetting.Visible = True
            End If
        End If

        Dim lIsAccessToNetworkManage As Boolean = False
        lIsAccessToNetworkManage = CConfig.ReadConfig("IsAccessToNetworkManage", False)

        Dim lIsAccessToRequestMultiDC As Boolean = False
        lIsAccessToRequestMultiDC = CConfig.ReadConfig("IsAccessToRequestMultiDC", False)

        If Not IsDebugMode() Then
            If Not lIsAccessToNetworkManage Or Not IsAdmin() Or Not IsSetadMode Then
                mnuMap121andIGMC.Visible = False
                ToolStripSeparator75.Visible = False
            End If

            If Not lIsAccessToRequestMultiDC Or Not IsSetadMode Then
                mnuRequestMultiDC.Visible = False
                ToolStripSeparator78.Visible = False
            End If
        End If


        'If Not IsDebugMode() Then
        'mnuBillBoardSlides.Visible = False
        'End If

        'AddHandler m_HookManager.HookInvoked, AddressOf m_oHookManager_OnHookMessage
        'm_HookManager.InstallHook()
        TimerHcls_Tick(Nothing, Nothing)

        If IsDebugMode() Then
            tlbDebug.Visible = True
        End If

        If IsKaraj Or IsDebugMode() Then
            mnuRepExcelGISLoadings.Visible = True
        End If

        If IsTozi("Karaj") OrElse IsDebugMode() Then
            mnuRepExcelGISLoadings.Visible = True
        Else
            mnuRepExcelGISLoadings.Visible = False
        End If

        Dim lSMSPanel As String = ""
        lSMSPanel = CConfig.ReadConfig("SMSPanel", "")
        If lSMSPanel <> "_Active_" AndAlso Not IsDebugMode() Then
            mnuSmsContactsInfos.Visible = False
        End If

        If m_IsCRMMode And m_IsCallCenterEnable Then
            tlbCallCenter.Visible = True
            CheckCallCenterToolbarStatus()
        ElseIf Not m_IsCallCenterEnable Then
            TimerHcls.Enabled = False
        End If

        Dim lIsPeopleVoice As Boolean = CConfig.ReadConfig("ActivePeopleVoice", False)
        If Not lIsPeopleVoice And Not IsDebugMode() Then
            mnuPeopleVoice.Visible = False
        End If

        If mnuPeopleVoice.Visible Or mnuSmsContactsInfos.Visible Then
            ToolStripSeparator2.Visible = True
        End If

        'Dim lDisablePilotMessageForReportIndex As Boolean = CConfig.ReadConfig("DisablePilotMessageForReportIndex", "True")
        'If lDisablePilotMessageForReportIndex Then
        '    mnuReport_10_26.Text &= " - مقادير گزارش هنوز توسط توانير تاييد نشده است"
        'End If

        Dim lLPTransMonitoringAccess As Object = CConfig.ReadConfig("IsLPTransMonitoringAccess", "False")
        If IsNothing(lLPTransMonitoringAccess) OrElse IsDBNull(lLPTransMonitoringAccess) Then
            lLPTransMonitoringAccess = "False"
        End If
        Dim lIsLPTransMonitoringAccess As Boolean = lLPTransMonitoringAccess.ToString().ToLower() = "true"

        If Not lIsLPTransMonitoringAccess Then
            mnuSepLPTRans.Visible = False
            mnuFreeTrans.Visible = False
            mnuTransLogs.Visible = False
        End If

        If mIsShowFav Then
            FavMenuVisible(mnuReports)
        Else
            VisibleAllMenu(mnuReports)
        End If

        CheckUserTrusteeForReports()
        AddHandler TimerUpdateExit.Tick, AddressOf TimerUpdateExit_Tick

        Dim lIsNoSCU As Object = ReadConfig("NoSCU")
        If lIsNoSCU Is Nothing OrElse lIsNoSCU Is DBNull.Value OrElse String.IsNullOrWhiteSpace(lIsNoSCU.ToString()) Then
            MenuShowInfo.Visible = False
        End If

        Dim lURL As String = CConfig.ReadConfig("SimaURL", "")
        If Not String.IsNullOrWhiteSpace(lURL) Then
            mnuSimaPortal.Visible = True
            mnuSepMisc.Visible = True
        Else
            mnuSimaPortal.Visible = False
            mnuSepMisc.Visible = False
        End If

        Dim lIsShowReport_3_37 As String = CConfig.ReadConfig("IsShow_Rpt_3_37", "False")
        mnuRep_3_37.Visible = (lIsShowReport_3_37 = "True" Or IsDebugMode())
    End Sub
    Private Sub frmMain_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        If Not mIsFirst Then Exit Sub
        mIsFirst = False
        SetFormColors()

        'm_HookManager.InstallHook()
        'AddHandler m_HookManager.HookInvoked, AddressOf m_oHookManager_OnHookMessage

        Dim lTipDlg As New frmTip
        If lTipDlg.IsTipExists Then
            lTipDlg.ShowDialog()
        ElseIf lTipDlg.TipId = 0 Then
            mnuTip.Visible = False
        End If
        lTipDlg.Dispose()
        lTipDlg = Nothing

        TimerNewMessages.Enabled = True

        tscMain.Height = tlbMain.Height + 2
        tlbMain.Top = 0
        tlbCallCenter.Top = 0
        tlbDebug.Top = 0

        tlbMain.Left = tscMain.ContentPanel.Width - tlbMain.Width
        If tlbCallCenter.Visible Then
            tlbCallCenter.Left = tlbMain.Left - tlbCallCenter.Width
            tlbDebug.Left = tlbCallCenter.Left - tlbDebug.Width
        Else
            tlbDebug.Left = tlbMain.Left - tlbDebug.Width
        End If

    End Sub

    Private Sub TimerUpdateExit_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If UpdateIsInProgress Then
            SaveLogout()
            Me.Close()
            Me.Dispose()
            Application.Exit()
            End
            Exit Sub
        End If
        TimerUpdateExit.Stop()
    End Sub

    Private Sub MenuFileMessages_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuFileMessages.Click
        Dim dlg As frmMessagesPreview = New frmMessagesPreview
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuFileExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuFileExit.Click
        Me.Close()
    End Sub

    Private Sub MenuToolsUserManagement_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuToolsUserManagement.Click
        Dim dlg As frmUserManagement = New frmUserManagement
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuConfiguration_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuConfiguration.Click
        Dim EnteredValues As New ArrayList
        Dim i As Integer
        For i = 0 To 32
            EnteredValues.Add("")
        Next

        'Dim Key As RegistryKey = Registry.LocalMachine
        Dim ValueData As Object
        InsertAt(EnteredValues, 0, GetRegistryAbd("ChannelNumber", "1", True))
        InsertAt(EnteredValues, 1, GetRegistryAbd("SharePathOfSoundsOnHost", "\Sounds\", True))
        If GetRegistryAbd("TrustedConnection", "False", True) = "" Then
            InsertAt(EnteredValues, 2, "False")
        Else
            InsertAt(EnteredValues, 2, GetRegistryAbd("TrustedConnection", "False", True))
        End If
        InsertAt(EnteredValues, 3, GetRegistryAbd("MainSQLServerName", "NotDeclared"))
        InsertAt(EnteredValues, 4, mTACEnc.Decrypt(GetRegistryAbd("TACDataCenter_03", "")))
        InsertAt(EnteredValues, 5, mTACEnc.Decrypt(GetRegistryAbd("TACDataCenter_01", "")))
        InsertAt(EnteredValues, 6, mTACEnc.Decrypt(GetRegistryAbd("TACDataCenter_02", "")))
        InsertAt(EnteredValues, 7, GetRegistryAbd("SQLServerName", "NotDeclared"))
        InsertAt(EnteredValues, 8, mTACEnc.Decrypt(GetRegistryAbd("TACDataNahieh_03", "")))
        InsertAt(EnteredValues, 9, mTACEnc.Decrypt(GetRegistryAbd("TACDataNahieh_01", "")))
        InsertAt(EnteredValues, 10, mTACEnc.Decrypt(GetRegistryAbd("TACDataNahieh_02", "")))
        InsertAt(EnteredValues, 11, GetRegistryAbd("CallCenterServerName"))
        InsertAt(EnteredValues, 12, mTACEnc.Decrypt(GetRegistryAbd("TacDataCC_03", "")))
        InsertAt(EnteredValues, 13, mTACEnc.Decrypt(GetRegistryAbd("TacDataCC_01", "")))
        InsertAt(EnteredValues, 14, mTACEnc.Decrypt(GetRegistryAbd("TacDataCC_02", "")))
        InsertAt(EnteredValues, 15, GetRegistryAbd("TimerUpdateDataInterval", "15000", True))
        InsertAt(EnteredValues, 16, GetRegistryAbd("TimerCallPoolingInterval", "5000", True))
        InsertAt(EnteredValues, 17, GetRegistryAbd("TimerMessagePoolingInterval", "15000", True))
        InsertAt(EnteredValues, 18, GetRegistryAbd("VisibleGridsLP", "1,2,3,4,5,6,7,8,9,10,11,12,13", True))
        InsertAt(EnteredValues, 19, GetRegistryAbd("VisibleGridsMP", "1,2,3,4,5,6,7,8,9,10,11,12,13", True))
        InsertAt(EnteredValues, 20, GetRegistryAbd("VisibleGridsLPWidth", "60,40,50,50,65,50,35,60,60,40,50,50,200", True))
        InsertAt(EnteredValues, 21, GetRegistryAbd("VisibleGridsMPWidth", "60,40,50,50,65,50,35,60,60,40,50,50,200", True))

        Dim lSchemaName As String = "SchemaName" 'IIf(Not IsBazdidApp, "SchemaName", "SchemaNameBazdid")
        If GetRegistryAbd(lSchemaName, "", True) = "" Then
            InsertAt(EnteredValues, 22, "Green")
        Else
            InsertAt(EnteredValues, 22, GetRegistryAbd(lSchemaName, "Green", True))
        End If
        If GetRegistryAbd("TrustedConnectionLS", "True", True) = "" Then
            InsertAt(EnteredValues, 23, "False")
        Else
            InsertAt(EnteredValues, 23, GetRegistryAbd("TrustedConnectionLS", "True", True))
        End If
        InsertAt(EnteredValues, 24, GetRegistryAbd("SetadSQLServerName", "NotDeclared"))
        InsertAt(EnteredValues, 25, mTACEnc.Decrypt(GetRegistryAbd("TACDataSetad_03", "")))
        InsertAt(EnteredValues, 26, mTACEnc.Decrypt(GetRegistryAbd("TACDataSetad_01", "")))
        InsertAt(EnteredValues, 27, mTACEnc.Decrypt(GetRegistryAbd("TACDataSetad_02", "")))
        InsertAt(EnteredValues, 28, GetRegistryAbd("IsLSDisabled", "", False))
        InsertAt(EnteredValues, 29, GetRegistryAbd("CallPooling", "0", True))
        InsertAt(EnteredValues, 30, GetRegistryAbd("TAPIServerName"))
        InsertAt(EnteredValues, 31, GetRegistryAbd("IsScreenSaver", "False"))
        InsertAt(EnteredValues, 32, GetRegistryAbd("ScreenSaverInterval", "-1"))
        InsertAt(EnteredValues, 33, GetRegistryAbd("IsMsgAlarm", "true"))
        InsertAt(EnteredValues, 34, GetRegistryAbd("MsgAlarmPath", ""))
        InsertAt(EnteredValues, 35, GetRegistryAbd("IsSMSPopupEnabled", False))
        InsertAt(EnteredValues, 36, GetRegistryAbd("IsAVLAlarmEnabled", False))
        InsertAt(EnteredValues, 37, GetRegistryAbd("IsActiveAlarmNewTaskCenter", False))

        Integer.TryParse(EnteredValues(29), CallPoolingIndex)
        If CallPoolingIndex = -1 Then CallPoolingIndex = 0

        Dim cnn As String = GetConnection()
        Dim dlg As New frmAppConfig(0, EnteredValues, False)
        Dim IsLSDebug As String = GetRegistryAbd("IsLSDisabled", "", False)

        SetScreenSaver(False)

        Dim lAns As DialogResult
        lAns = dlg.ShowDialog()
        dlg.Dispose()

        If cnn <> GetConnection() OrElse IsLSDebug <> GetRegistryAbd("IsLSDisabled", "", False) Then
            IsActiveCallsServer = LCase(GetRegistryAbd("IsLSDisabled", "", False)) <> "true"
            Dim dlg1 As frmLogin = New frmLogin(True)
            Me.Dispose()
            If dlg1.ShowDialog() = DialogResult.OK Then
                dlg1.Dispose()
                Dim lDlg As Form
                If IsErjaApp Then
                    lDlg = New frmMainErja
                ElseIf IsTamirApp Then
                    lDlg = New frmMainTamir
                Else
                    lDlg = New frmMain
                End If
                If EnteredValues(31) = "True" Then
                    SetScreenSaver(True)
                End If
                lDlg.ShowDialog()
                lDlg.Dispose()
                SetScreenSaver(False)
            Else
                SaveLogout()
                dlg1.Dispose()
                Application.Exit()
                End
            End If
        Else
            If EnteredValues(31) = "True" Then
                SetScreenSaver(True)
            Else
                SetScreenSaver(False)
            End If
        End If

        Integer.TryParse(GetRegistryAbd("CallPooling", "0", True), CallPoolingIndex)
        If CallPoolingIndex = -1 Then CallPoolingIndex = 0
    End Sub
    Private Sub MenuToolsRelogin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuToolsRelogin.Click
        Dim UserAns As DialogResult = MsgBoxF("آيا با تغيير كد كاربري موافقيد؟" & vbCrLf & "كد كاربري فعال: " & WorkingUserName, "حصول اطمينان", MessageBoxButtons.YesNo, MsgBoxIcon.MsgIcon_Question, MessageBoxDefaultButton.Button2)
        If UserAns = DialogResult.No Then Exit Sub
        PowerOffHavadesInfoTimer()
        SaveLogout()
        Dim dlg As frmLogin = New frmLogin(True)
        Me.Dispose()
        If dlg.ShowDialog() = DialogResult.OK Then
            dlg.Dispose()

            If m_IsTelCMode Then
                If Not mTelCInfo.Logout() Then
                    ShowError("عدم موفقيت در خروج از مرکز تماس." & vbCrLf & "هنوز تلفن اپراتور قبلي از سيستم تلسي خارج نشده است." & vbCrLf & mTelCInfo.Message)
                Else
                    m_IsLoginToCallCenter = False
                End If
            End If

            Dim lDlg As Form
            If IsErjaApp Then
                lDlg = New frmMainErja
            ElseIf IsTamirApp Then
                lDlg = New frmMainTamir
            Else
                lDlg = New frmMain
            End If
            lDlg.ShowDialog()
            lDlg.Dispose()
        Else
            dlg.Dispose()
            Application.Exit()
            End
        End If
    End Sub

    Private Sub MenuToolsChangePassword_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuToolsChangePassword.Click
        Dim dlg As New frmGeneralSettings(mDs)
        If (dlg.ShowDialog() = DialogResult.OK) Then
            If mIsShowFav <> dlg.IsShowFavorite Then
                mIsShowFav = dlg.IsShowFavorite
                If mIsShowFav Then
                    FavMenuVisible(mnuReports)
                Else
                    VisibleAllMenu(mnuReports)
                End If
            End If
            If dlg.IsDeleteFavorite Then
                SetFavMenuUnCheck(mnuReports)
            End If
        End If
        dlg.Dispose()
    End Sub

    Private Sub FillDs(ByVal TblName As String)

    End Sub

    Private Sub MenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem2.Click
        Dim dlg As frmBaseTables
        dlg = New frmBaseTables("Tbl_Weather")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem36_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem36.Click
        Dim dlg As frmBaseTables
        dlg = New frmBaseTables("Tbl_EndJobState")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem39_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem39.Click
        Dim dlg As frmBaseTables
        dlg = New frmBaseTables("Tbl_ReferTo")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem42_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem42.Click
        Dim dlg As frmBaseTables
        dlg = New frmBaseTables("Tbl_UndoneReason")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem27_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem27.Click
        Dim dlg As frmBaseTablesNotStd
        dlg = New frmBaseTablesNotStd("View_Area")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem34_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem34.Click
        Dim dlg As frmBaseTables
        dlg = New frmBaseTables("Tbl_Department")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem40_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem40.Click
        Dim dlg As New frmBaseTables("Tbl_DisconnectGroupMPPrimary")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem41_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem41.Click
        Dim dlg As frmBaseTables
        dlg = New frmBaseTables("Tbl_DisconnectGroup")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem46_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem46.Click
        Dim dlg As frmBaseTables
        dlg = New frmBaseTables("Tbl_DisconnectGroup")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem30_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem30.Click
        Dim dlg As frmBaseTables
        dlg = New frmBaseTables("Tbl_PartUnit")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem17_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem17.Click
        Dim dlg As New frmPhoneBookPreview
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem63_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem63.Click
        Dim dlg As New frmBaseTables("Tbl_CallReasonLight")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem3.Click
        Dim dlg As New frmBaseTables("Tbl_LPPostType")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem7_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem7.Click
        Dim dlg As New frmBaseTables("Tbl_PhonebookTask")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem38_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem38.Click
        Dim dlg As New frmBaseTables("Tbl_PhonebookType")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem67_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem67.Click
        Dim dlg As New frmBaseTablesNotStd("Tbl_Zone")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub mnuSection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSection.Click
        Dim dlg As New frmBaseTablesNotStd("Tbl_Section")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub mnuTown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuTown.Click
        Dim dlg As New frmBaseTablesNotStd("Tbl_Town")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub mnuVillage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuVillage.Click
        Dim dlg As New frmBaseTablesNotStd("Tbl_Village")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub mnuRoosta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRoosta.Click
        Dim dlg As New frmBaseTablesNotStd("Tbl_Roosta")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem59_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem59.Click
        Dim dlg As New frmBaseTablesNotStd("View_LPCommonFeeder")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem6.Click
        Dim dlg As New frmBaseTablesNotStd("ViewCriticalFeeder")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuItem51_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem51.Click
        Dim dlg As New frmBaseTablesNotStd("Tbl_LPFeeder", , , , , , , , , , True)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuItem55_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim dlg As New frmBaseTablesNotStd("Tbl_LPPart")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem50_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem50.Click
        Dim dlg As New frmBaseTablesNotStd("Tbl_LPPost", , , , , , , , , , True)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem32_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem32.Click
        Dim dlg As New frmBaseTablesNotStd("Tbl_Master")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuItem68_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem68.Click
        Dim dlg As New frmBaseTablesNotStd("View_MPCommonFeeder")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuItem69_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem69.Click
        Dim dlg As New frmBaseTablesNotStd("Tbl_MPFeeder", , , , , , , , , , True)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem35_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim dlg As New frmBaseTablesNotStd("Tbl_MPPart")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem70_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem70.Click
        Dim dlg As New frmBaseTablesNotStd("View_MPPost", , , , , , , , , , True)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem44_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem44.Click
        Dim dlg As New frmBaseTablesNotStd("View_DisconnectGroupLP")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuItem71_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem71.Click
        Dim dlg As New frmBaseTablesNotStd("View_DisconnectGroupMP")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub InitializeHardLockCheckingTimers()
        SetHardLockCheckTimersInterval()
        TimerHardLockCheckLaunch.Interval = HardLockCheckLaunchTimerInterval
        TimerHardLockCheckPooling1.Interval = HardLockCheckPooling1TimerInterval
        TimerHardLockCheckPooling2.Interval = HardLockCheckPooling2TimerInterval
        TimerHardLockCheckLaunch.Enabled = True
        TimerHardLockCheckPooling1.Enabled = True
        TimerHardLockCheckPooling2.Enabled = True
    End Sub

    Private Sub SetMenuLocks()
        Dim lLockImage As Image = imgListMisc.Images(0)

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.rep_Sum_5_1_1) Then
            mnuReport5_1_1_mode2.Image = lLockImage
        Else
            'حذف خلاصه گزارش قطعيها و انرژي توزيع نشده
            If Not IsDebugMode() Then
                MenuButtonItem24.Visible = False
            Else
                MenuButtonItem24.Image = imgListMisc.Images(1)
            End If
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.rep_Sum_5_1_1_ByServevr121) Then
            mnuRep5_1_1_Byerver121.Image = lLockImage
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.rep_LP_Mode1) Then
            mnuLP_Mode1.Image = lLockImage
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.rep_ReferToCountArea) Then
            mnuRptReferToCountArea.Image = lLockImage
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.rep_ReferToCount) Then
            mnuRptReferToCount.Image = lLockImage
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.rep_MPStatusChart) Then
            mnuRepMPStatusChart.Image = lLockImage
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.mnu_BillBoard) Then
            mnuBillBoard.Image = lLockImage
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.rep_MPDisconectLPPost) Then
            mnuRepMPDisconnectLPPost.Image = lLockImage
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.rep_Excel) Then
            mnuExcelReports.Image = lLockImage
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.mnu_RequestInform) Then
            mnuInformMonitoring.Image = lLockImage
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.ver_951115) Then
            mnuLightRequestSmsSetting.Image = lLockImage
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.mnu_FeederPart) Then
            mnuFeederPart.Image = lLockImage
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.mnu_KeyboardBase) Then
            mnuIsKeyboardBase.Checked = False
            mIsKeyboardBase = False
            mnuIsKeyboardBase.Image = lLockImage
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.rep_Dispaching) Then
            mnuRepDispaching.Image = lLockImage
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.itm_Questionnaire) Then
            mnuRepQuestionnaire.Image = lLockImage
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.ver_960515) And Not IsTozi("Ardebil") Then
            mnuRepQuestionnaire2.Image = lLockImage
            mnuReport_9_17.Image = lLockImage
        End If

        If mIsSendGISSubscriberDCInfo OrElse IsDebugMode() Then
            mnuSubecribersSourceSetting.Visible = True
        Else
            mnuSubecribersSourceSetting.Visible = False
        End If

        If IsTozi("Lorestan") OrElse IsDebugMode() Then
            mnuReport_8_19.Visible = True
        Else
            mnuReport_8_19.Visible = False
            mnuReport_8_19.IsFavVisible = False
        End If

        If IsTozi("Kerman") OrElse IsDebugMode() Then
            mnuReport_10_19.Visible = True
        Else
            mnuReport_10_19.Visible = False
            mnuReport_10_19.IsFavVisible = False
        End If

        Dim lIsShowReport_1_20 As Boolean = CConfig.ReadConfig("IsShowReport_1_20", "False").ToString().ToLower() = "true"
        If lIsShowReport_1_20 OrElse IsTozi("Yazd") OrElse IsDebugMode() Then
            mnuReport_1_20.Visible = True
        Else
            mnuReport_1_20.Visible = False
            mnuReport_1_20.IsFavVisible = False
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.mnu_BillBoardSlides) Then
            mnuBillBoardSlides.Image = lLockImage
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.rep_DisconnectCountMonthly) Then
            mnuRepDisconnectMonthlyCount.Image = lLockImage
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.rep_DisconnectGroupSetGroupCount) Then
            mnuRepDisconnectGroupSetGroup.Image = lLockImage
            mnuDisconnectGroupSetGroup.Image = lLockImage
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.rep_GroupTamir) Then
            mnuRepGroupTamir.Image = lLockImage
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.rep_LightParts) Then
            mnuLightRequestParts.Image = lLockImage
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.rep_LightStatParts) Then
            mnuLightUseParts.Image = lLockImage
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.rep_Subscribers) Then
            mnuRepSubscribers.Image = lLockImage
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.itm_DailyPeak) Then
            mnuMPPostFeederPeakDaily.Image = lLockImage
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.rep_SariReports) Then
            mnuRepPowerIndex.Image = lLockImage
            mnuRepGroupSetGroup.Image = lLockImage
            mnuRepAllDisconnectFeeder.Image = lLockImage
            mnuRepDCPServer121.Image = lLockImage
            mnuMPFeederPeak.Image = lLockImage
            mnuRepRequestAll.Image = lLockImage
            mnuRepRequestAll2.Image = lLockImage
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.mnu_OnlineUsers) Then
            mnuLoginLog.Image = lLockImage
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.rep_880129) Then
            mnuLPPostCapacity.Image = lLockImage
            mnuRepDCRecloser.Image = lLockImage
            mnuRepMPFeederPeakEnergyMonthly.Image = lLockImage
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.itm_MPPostMonthly) Then
            mnuMPPostMonthly.Image = lLockImage
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.rep_DupControl) Then
            mnuRepCheckLies.Image = lLockImage
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.rep_7_21) Then
            mnuRep_7_21.Image = lLockImage
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.rep_10_11) Then
            mnuRepWantedInfo.Image = lLockImage
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.itm_CallListen) Then
            rptRep_ListenRequestCallList.Image = lLockImage
            rptRep_ListenUserCallCount.Image = lLockImage
            mnuRepUserLogin.Image = lLockImage
            mnuRepuserAccess.Image = lLockImage
            mnuRep_MPCriticalFeeder.Image = lLockImage
            mnuRep_LPCriticalFeeder.Image = lLockImage
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.rep_ExcelCommSys) Then
            mnuRepExccelComunicationSystems.Image = lLockImage
        End If
        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.rep_ExcelGroupSetGroup) Then
            mnuRepExcelGroupSetGroup.Image = lLockImage
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.rep_1_18) Then
            mnuRep_1_18.Image = lLockImage
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.ver_910223) Then
            mnuLP_Mode2.Image = lLockImage
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.rep_MPPostTransLoads) Then
            mnuRepMPPostTransLoadMonthly.Image = lLockImage
            mnuRepMPPostTransLoadDaily.Image = lLockImage
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.ver_910306) Then
            mnuRep_7_22.Image = lLockImage
            mnuRep_7_23.Image = lLockImage
            mnuRep_7_24.Image = lLockImage
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.rep_9_9) Then
            mnuRepEkipPerformance.Image = lLockImage
        End If

        Dim lIsAccess As Boolean
        lIsAccess = IsAdmin() OrElse CheckUserTrustee("rptQuestionnaire", 23)
        mnuRepQuestionnaire.Visible = lIsAccess
        mnuRepQuestionnaire2.Visible = lIsAccess
        mnuReport_9_17.Visible = lIsAccess

        If IsSetadMode Then
            If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.mnu_MainPeak) Then
                mnuMainPeak.Image = lLockImage
                mnuRepMainPeak.Image = lLockImage
            End If
        Else
            mnuMainPeak.Visible = False
            mnuRepMainPeak.Visible = False
            ToolStripSeparator46.Visible = False
            mnuSubecribersSourceSetting.Visible = False
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.rep_LoadAnalyze) Then
            mnuLPPostLoadsStat.Image = lLockImage
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.ver_930324) Then
            mnuUtilizationFactor.Image = lLockImage
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.ver_930617) Then
            mnuLPPostEarths.Image = lLockImage
            mnuReport_8_13.Image = lLockImage
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.ver_931018) Then
            mnuThriftPower.Image = lLockImage
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.ver_940410) Then
            mnuMPFeederMonthly.Image = lLockImage
            mnuRepExcelMPFeederDaily.Image = lLockImage
            mnuHavadesConfig.Image = lLockImage
        End If
        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.ver_940510) Then
            mnuRepExcelDisconnectGroupSet.Image = lLockImage
            mnuMPFeederDisconnetInfo.Image = lLockImage
            mnuRepCompareDCCount.Image = lLockImage
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.ver_950320) And Not IsTozi("MazandaranWest") Then
            mnuMPFeederKey.Image = lLockImage
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.ver_950331) Then
            mnuRepDeleteRequest.Image = lLockImage
            mnuDeletePostFeeder.Image = lLockImage
        End If
        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.ver_951011) Then
            mnuDeleteFeeder.Image = lLockImage
            mnuReport_10_17.Image = lLockImage
        End If
        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.ver_951115) Then
            mnuReport_9_16.Image = lLockImage
        End If
        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.ver_951115) Then
            mnuReport_9_16.Image = lLockImage
        End If
        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.ver_960328) Then
            mnuRep_10_21.Image = lLockImage
        End If
        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.ver_960515) And Not IsTozi("Ardebil") Then
            mnuQuestion.Image = lLockImage
        End If
        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.ver_970920) And Not IsTozi("Hamedan") Then
            mnuRep_7_28.Image = lLockImage
        End If
        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.ver_970920) And Not IsTozi("Karaj") Then
            mnuSurvey.Image = lLockImage
            mnuSurveyIVR.Image = lLockImage
        End If
        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.ver_971110) And Not IsTozi("Orumieh") Then
            mnuGuidSMS.Image = lLockImage
            mnuSMSErjaDone.Image = lLockImage
            mnuSMSAfterEzamEkip.Image = lLockImage
        End If

        Dim lNotifyServiceAddress As String = CConfig.ReadConfig("TZHavadesTavanirURL", "")
        If String.IsNullOrWhiteSpace(lNotifyServiceAddress) AndAlso Not IsDebugMode() Then
            mnuSendMobileAppNotification.Visible = False
            ToolStripSeparator1.Visible = False
        End If

    End Sub

    Function MakeInsertSQLCommand(ByRef dt As DataTable) As String
        Dim s1 As String = ""
        Dim s2 As String = ""
        Dim s As Object
        Dim i As Integer
        For i = 0 To dt.Columns.Count - 1
            If i = 0 Then
                s1 = dt.Columns(i).ColumnName
                s2 = dt.Rows(0).Item(i)
            Else
                s = dt.Rows(0).Item(i)
                Select Case TypeName(s)
                    Case "DBNull"
                        s1 = s1 & ", " & dt.Columns(i).ColumnName
                        s2 = s2 & ", NULL"
                    Case "Date"
                        Dim DateStr As String = s.Year & "-" & Format(s.Month, "0#") & "-" & Format(s.Day, "0#") & " "
                        DateStr = DateStr & Format(s.Hour, "0#") & ":" & Format(s.Minute, "0#") & ":" & Format(s.Second, "0#")
                        s1 = s1 & ", " & dt.Columns(i).ColumnName
                        s2 = s2 & ", " & "CONVERT(DATETIME, '" & DateStr & "',102)"
                    Case "Boolean"
                        s1 = s1 & ", " & dt.Columns(i).ColumnName
                        If s = False Then
                            s2 = s2 & ", 0"
                        Else
                            s2 = s2 & ", 1"
                        End If
                    Case "String"
                        s1 = s1 & ", " & dt.Columns(i).ColumnName
                        s2 = s2 & ", N'" & s.Replace("'", "''") & "'"
                    Case Else
                        s1 = s1 & ", " & dt.Columns(i).ColumnName
                        s2 = s2 & ", " & s
                End Select
            End If
        Next
        Dim SQL As String = "INSERT INTO " & dt.TableName & "(" & s1 & ") VALUES (" & s2 & ")"
        MakeInsertSQLCommand = SQL
    End Function
    Function MakeUpdateSQLCommand(ByRef dt As DataTable) As String
        Dim s1 As String = ""
        Dim s As Object
        Dim i As Integer
        Dim cm As String = " "
        For i = 1 To dt.Columns.Count - 1
            s = dt.Rows(0).Item(i)
            If i = 2 Then
                cm = ", "
            End If
            Select Case TypeName(s)
                Case "DBNull"
                    s1 = s1 & cm & dt.Columns(i).ColumnName & "=NULL"
                Case "Date"
                    Dim DateStr As String = s.Year & "-" & Format(s.Month, "0#") & "-" & Format(s.Day, "0#") & " "
                    DateStr = DateStr & Format(s.Hour, "0#") & ":" & Format(s.Minute, "0#") & ":" & Format(s.Second, "0#")
                    s1 = s1 & cm & dt.Columns(i).ColumnName & "=CONVERT(DATETIME, '" & DateStr & "',102)"
                Case "String"
                    s1 = s1 & cm & dt.Columns(i).ColumnName & "=N'" & s.Replace("'", "''") & "'"
                Case "Boolean"
                    If s = False Then
                        s1 = s1 & cm & dt.Columns(i).ColumnName & "=0"
                    Else
                        s1 = s1 & cm & dt.Columns(i).ColumnName & "=1"
                    End If
                Case Else
                    s1 = s1 & cm & dt.Columns(i).ColumnName & "=" & s
            End Select
        Next
        Dim SQL As String = "UPDATE " & dt.TableName & " SET " & s1 & " WHERE " & dt.Columns(0).ColumnName & "=" & dt.Rows(0).Item(0)
        MakeUpdateSQLCommand = SQL
    End Function

    Private Sub MenuItem60_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem60.Click
        Dim dlg As New frmBaseTables("Tbl_PostalCode")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuItem11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem11.Click
        Dim dlg As New frmSubscribersPreview(False)
        'dlg.MdiParent = Me
        'dlg.Show()
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuItem52_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem52.Click
        Dim dlg As frmBaseTables
        dlg = New frmBaseTables("Tbl_CallReason")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuItem64_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem64.Click
        Dim dlg As New frmBaseTablesNotStd("Tbl_LightPart")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub TimerNewMessages_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerNewMessages.Tick
        If Not IsActiveTimers() Then Exit Sub
        Timers(0) += 1
        ShowTimers()

        Dim lMem As Long = GC.GetTotalMemory(True)
        If lMem > 400000000 Then
            TimerNewMessages.Enabled = False
            If MsgBoxF("برنامه با کمبود حافظه مواجه شده است. آيا از برنامه خارج مي‌شويد؟", "کمبود حافظه", MessageBoxButtons.YesNo, MsgBoxIcon.MsgIcon_Stop, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                Me.Close()
                Me.Dispose()
                TerminateApplication()
            End If
            TimerNewMessages.Enabled = True
        End If

        TimerNewMessages.Enabled = False
        Dim dlg As New frmMessagesPreview(Now())
        Dim isAlarm As Boolean
        If dlg.UserForTodayHasMessages(isAlarm) Then
            NewMessageReceived = True
            Dim dlgWelcome As New frmInformUserAction("سلام! به سامانه ثبت حوادث برق خوش آمديد")
            dlgWelcome.ShowDialog()
            dlgWelcome.Dispose()
            dlg.ShowDialog()
            dlg.Dispose()
        ElseIf Not IsDebugMode() Then
            NewMessageReceived = False
            NewMessageReceivedId = -1
            MsgBoxF("سلام" & vbCrLf & "به سامانه ثبت حوادث برق خوش آمديد" & vbCrLf & "براي امروز پيغامي نداريد", "كارتابل", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Information, MessageBoxDefaultButton.Button1)
        End If
        TimerCallPooling.Enabled = True


        If Not IsSetadMode And mIsActiveSMSPopupSystem Then
            TimerSMSPopup.Enabled = True
        End If

        If mIsActiveAVLSystem Then
            TimerAVL.Enabled = True
        End If

        dlg.Dispose()
        dlg = Nothing
    End Sub

    Private Sub MenuItem53_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuLP.Click
        Dim dlg As New frmSearchBaseLP(False, 0)
        dlg.ShowDialog()
        dlg.Dispose()
        'dlg.MdiParent = Me.Owner
        'dlg.Show()
    End Sub
    Private Sub MenuItem56_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem56.Click
        Dim dlg As New frmSearchBaseLP(False, 1)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem57_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem57.Click
        Dim dlg As New frmSearchBaseLP(False, 2)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem54_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem54.Click
        Dim dlg As New frmSearchBaseLP(False, 3)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem65_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem65.Click
        Dim dlg As New frmSearchBaseLP(False, 4)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem136_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem136.Click
        Dim dlg As New frmSearchBaseLP(False, 5)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem139_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem139.Click
        Dim dlg As New frmSearchBaseLP(False, 6)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem100_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem100.Click
        Dim dlg As New frmSearchBaseLP(False, 7)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem137_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem137.Click
        Dim dlg As New frmSearchBaseLP(False, 8)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem101_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem101.Click
        Dim dlg As New frmSearchBaseLP(False, 9)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem138_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem138.Click
        Dim dlg As New frmSearchBaseLP(False, 10)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem118_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem118.Click
        Dim dlg As New frmSearchBaseLP(False, 11)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuItem61_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim dlg As New frmSearchBaseLP(True, 1)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem58_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim dlg As New frmSearchBaseLP(True, 2)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem80_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem80.Click
        Dim dlg As New frmSearchBaseLP(True, 3)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem81_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem81.Click
        Dim dlg As New frmSearchBaseLP(True, 3)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem140_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem140.Click
        Dim dlg As New frmSearchBaseLP(True, 5)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem142_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem142.Click
        Dim dlg As New frmSearchBaseLP(True, 6)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem143_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem143.Click
        Dim dlg As New frmSearchBaseLP(True, 7)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem144_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem144.Click
        Dim dlg As New frmSearchBaseLP(True, 8)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem145_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem145.Click
        Dim dlg As New frmSearchBaseLP(True, 9)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem146_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem146.Click
        Dim dlg As New frmSearchBaseLP(True, 10)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem147_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem147.Click
        Dim dlg As New frmSearchBaseLP(True, 11)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuItem79_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem79.Click
        Dim dlg As New frmSearchBaseMP(0)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem78_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem78.Click
        Dim dlg As New frmSearchBaseMP(1)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem77_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem77.Click
        Dim dlg As New frmSearchBaseMP(2)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem76_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem76.Click
        Dim dlg As New frmSearchBaseMP(3)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem75_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem75.Click
        Dim dlg As New frmSearchBaseMP(4)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem74_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem74.Click
        Dim dlg As New frmSearchBaseMP(5)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem73_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem73.Click
        Dim dlg As New frmSearchBaseMP(6)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem152_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem152.Click
        Dim dlg As New frmSearchBaseMP(7)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem157_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem157.Click
        Dim dlg As New frmSearchBaseMP(8)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem156_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem156.Click
        Dim dlg As New frmSearchBaseMP(9)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem155_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem155.Click
        Dim dlg As New frmSearchBaseMP(10)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem149_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem149.Click
        Dim dlg As New frmSearchBaseMP(11)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem153_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem153.Click
        Dim dlg As New frmSearchBaseMP(12)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem150_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem150.Click
        Dim dlg As New frmSearchBaseMP(13)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem154_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem154.Click
        Dim dlg As New frmSearchBaseMP(14)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem151_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem151.Click
        Dim dlg As New frmSearchBaseMP(15)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuItem82_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem82.Click
        Dim dlg As frmBaseTables
        dlg = New frmBaseTables("Tbl_CallType")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem84_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem84.Click
        Dim dlg As frmBaseTables
        dlg = New frmBaseTables("Tbl_Province")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem83_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem83.Click
        Dim dlg As New frmBaseTablesNotStd("View_City")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuItem86_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem86.Click
        'Dim dlg As New frmAddNewRequest(Nothing)
        'dlg.ShowDialog()
        Try
            Dim dlg As Form
            If Not mIsKeyboardBase Then
                dlg = New frmRequest(-1, Nothing, Nothing)
            Else
                dlg = New frmRequestKeyboardBase(-1, Nothing, Nothing)
            End If
            dlg.ShowDialog()
            dlg.Dispose()
            dlg = Nothing
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub
    Private Sub tbNewRequest_Activate(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbNewRequest.Click
        MenuItem86_Click(sender, e)
    End Sub

    Private Sub MenuItem92_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem92.Click
        Dim dlg As New frmDisconnectPF("LP")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub mnuMPPostTransLoadHours_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMPPostTransLoadHours.Click
        Dim dlg As New frmMPPostTransLoad(DateTime.Now)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem110_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem110.Click
        Dim dlg As New frmMPFeederLoadPreview
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    'Private Sub TimerCallPooling_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    If Not IsActiveCallsServer Then
    '        Exit Sub
    '    End If
    '    If CallPoolingIndex = -1 Then
    '        Exit Sub
    '    End If
    '    Timers(2) += 1
    '    ShowTimers()
    '    Dim Tick As Long = GetTickCount
    '    Dim ChannelNumber As String
    '    Try
    '        Dim n As Integer = 5
    '        If CallPoolingIndex = 2 Or CallPoolingIndex = 3 Then
    '            n = 1
    '        End If
    '        If NoConcurrentRequests < n Then
    '            NoConcurrentRequests = NoConcurrentRequests + 1
    '            Dim Ds As New DataSet
    '            Dim Cnn As New SqlConnection(GetConnectionCallCenter())


    '            Dim Splt As Object = Split(Trim(GetChannelNumber()), ",", , CompareMethod.Text)
    '            Dim QueryChannelNumbers As String = ""
    '            Dim i As Integer
    '            For i = 0 To UBound(Splt)
    '                If CheckIntegerField(Trim(Splt(i))) Then
    '                    If Trim(Splt(i)) <> "" Then
    '                        QueryChannelNumbers = QueryChannelNumbers & " OR ChannelNumber=" & Trim(Splt(i))
    '                    End If
    '                End If
    '            Next i
    '            QueryChannelNumbers = "(" & QueryChannelNumbers.Remove(0, 4) & ")"
    '            If Trim(QueryChannelNumbers) <> "" Then
    '                Dim dt As Date = Now.AddHours(-1)
    '                Dim Query As String
    '                Dim lRingQuery As String = IIf(IsLSCallNewVersion, "RingCount > 0", "Digits Like '%R%'")
    '                If CallPoolingIndex = 0 Or CallPoolingIndex = 2 Then
    '                    If LastCallId > 0 Then
    '                        Query = "SELECT TOP 1 * FROM LSCalls WHERE CallID > " & LastCallId & " AND BeginDateTime >= '" & Format(dt, "yyyy/MM/dd HH:mm:ss") & "' AND " & QueryChannelNumbers & " AND " & lRingQuery & " AND Digits Like '%H%' AND (Status = ' ') ORDER BY CallID"
    '                    Else
    '                        Query = "SELECT TOP 1 * FROM LSCalls WHERE CallID > " & LastCallId & " AND BeginDateTime >= '" & Format(dt, "yyyy/MM/dd HH:mm:ss") & "' AND " & QueryChannelNumbers & " AND " & lRingQuery & " AND Digits Like '%H%' AND (Status = ' ') ORDER BY CallID Desc"
    '                    End If
    '                Else
    '                    If LastCallId > 0 Then
    '                        Query = "SELECT TOP 1 * FROM LSCalls WHERE CallID > " & LastCallId & " AND BeginDateTime >= '" & Format(dt, "yyyy/MM/dd HH:mm:ss") & "' AND " & QueryChannelNumbers & " AND " & lRingQuery & " AND Digits Like '%H%' ORDER BY CallID"
    '                    Else
    '                        Query = "SELECT TOP 1 * FROM LSCalls WHERE CallID > " & LastCallId & " AND BeginDateTime >= '" & Format(dt, "yyyy/MM/dd HH:mm:ss") & "' AND " & QueryChannelNumbers & " AND " & lRingQuery & " AND (Status = ' ') AND Digits Like '%H%' ORDER BY CallID Desc"
    '                    End If
    '                End If

    '                'Query = "SELECT TOP 1 * FROM LSCalls WHERE CallID > " & LastCallId & " AND BeginDateTime >= '" & Format(dt, "yyyy/MM/dd HH:mm:ss") & "' AND " & QueryChannelNumbers & " AND (Status = ' ') ORDER BY CallID"
    '                If Not BindingTable(Query, Cnn, Ds, "Calls") Then
    '                    Exit Sub
    '                End If
    '                If Ds.Tables("Calls").Rows.Count > 0 Then
    '                    Dim CallId As Long = Ds.Tables("Calls").Rows(0).Item("CallID")
    '                    If LastCallId <> CallId Then
    '                        LastCallId = CallId
    '                        Dim CallNumber As String = Ds.Tables("Calls").Rows(0).Item("CallerID")
    '                        CallNumber = CallNumber.Replace("R", "")
    '                        Try
    '                            'Dim dlg As New frmRequest(-1, LastCallId, Nothing)
    '                            'dlg.ShowDialog()
    '                        Catch ex As Exception
    '                            ShowError(ex)
    '                        End Try
    '                    End If
    '                End If
    '            End If
    '            NoConcurrentRequests = NoConcurrentRequests - 1
    '        Else
    '            'If Not IsRunningMsgBox Then
    '            '    IsRunningMsgBox = True
    '            '    MsgBoxF("لطفاً هر چه سريعتر نسبت به ثبت اطلاعات درخواست هاي تلفني قبلي اقدام كنيد" & vbCrLf & "سامانه عليرغم وجود تماس تلفني جديد امكان ثبت درخواست جديد را به شما نميدهد", "هشدار", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Warning, MessageBoxDefaultButton.Button1)
    '            '    IsRunningMsgBox = False
    '            'End If
    '            'Ds.Dispose()
    '            'Ds = Nothing
    '            'Cnn = Nothing
    '            'Exit Sub
    '        End If
    '    Catch e1 As Exception
    '        NoConcurrentRequests = NoConcurrentRequests - 1
    '        Exit Sub
    '    End Try
    '    Tick = GetTickCount - Tick + 1

    '    If Tick > 0 And Tick < 100000 Then
    '        TimerCallPoolingInterval = Tick * 10
    '        If TimerCallPoolingInterval < 700 Then
    '            TimerCallPoolingInterval = 700
    '        ElseIf TimerCallPoolingInterval > 4000 Then
    '            TimerCallPoolingInterval = 4000
    '        End If
    '        TimerCallPooling.Interval = TimerCallPoolingInterval
    '    End If
    'End Sub

    Private Sub TimerMessagePooling_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerMessagePooling.Tick
        If Not IsActiveTimers() Then Exit Sub
        Timers(3) += 1
        ShowTimers()
        Static isInRoutine As Boolean = False
        If isInRoutine Then Exit Sub
        isInRoutine = True
        Dim dlg As New frmMessagesPreview(Now())
        Dim isAlarm As Boolean
        If dlg.UserForTodayHasMessages(isAlarm) Then
            NewMessageReceived = True
            If isAlarm Then
                If mIsMsgAlarm Then
                    If Not mIsExternalMsgAlarm Then
                        PlaySound("Alarm.wav")
                    Else
                        PlaySound(mMsgAlarmFile, True)
                    End If
                End If
            End If
        Else
            NewMessageReceived = False
            NewMessageReceivedId = -1
        End If
        dlg.Dispose()
        dlg = Nothing
        isInRoutine = False
    End Sub

    Private Sub MenuItem96_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim dlg As New frmNetworkPower
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem99_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMPPostFeederPeakMonthly.Click
        Dim dlg As New frmMPFeederMonthlyLoadPeak
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAbout.Click
        Me.Visible = False
        Dim dlgMain As New Bargh_Authority.frmSplash
        dlgMain.ShowDialog()
        Me.Visible = True
    End Sub

    'View Timers Status:
    Private Sub ShowTimers()
        'GC.Collect()
        'Dim lMem As Long = GC.GetTotalMemory(True)
        'sbpTimers.Text = Timers(3) & " : " & Timers(2) & " : " & Timers(1) & " : " & Timers(0)
        'sbpTimers.Text = Timers(0) & " : " & Timers(1) & " : " & Timers(2) & " : " & Timers(3)
        sbpTimers.Text = Convert.ToInt32((GetTickCount - CMouseKeyboardHook.mLastTickCount) / 60000)
    End Sub

    Private Sub MenuItem47_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem47.Click
        Dim dlg As New frmMonthlyReportsFilters("جدول تعداد قطعي و انرژي توزيع نشده و مدت زمان خاموشي پستهاي فوق توزيع")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem107_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem107.Click
        Dim dlg As New frmMonthlyReportsFilters("آمار تعداد و ميزان قطع حفاظتي توزيع فيدرهاي فشار متوسط")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub mnuRep_7_21_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRep_7_21.Click
        Dim dlg As New frmMonthlyReportsFilters("آمار تعداد و مقدار قطعي حفاظتي توزيع فيدرهاي فشار متوسط بر اساس ناحيه")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuItem111_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem111.Click
        Dim dlg As New frmControlReportsFilters("عملكرد اپراتورهاي ثبت كننده اطلاعات", 1, , "(9-1)")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem9_12_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem9_12.Click
        Dim dlg As New frmControlReportsFilters("عملكرد اپراتورهاي تکميل كننده اطلاعات", 21, , "(9-12)")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem112_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem112.Click
        Dim dlg As New frmControlReportsFilters("عملكرد ناحيه در مرتفع نمودن مشكلات", 2, , "(9-2)")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem113_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem113.Click
        Dim dlg As New frmControlReportsFilters("عملكرد اكيپ هاي اعزامي", 3, , "(9-3)")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuItem117_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem117.Click
        Dim dlg As New frmCunsumeLoadFactorPreview
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    'Hard Lock Functions and Procedures:
    Public Sub SetHardLockCheckTimersInterval()
        Randomize()
        HardLockCheckLaunchTimerInterval = CInt(Int((HLC_LAUNCH_TIMER_INT * Rnd()) + CInt(HLC_LAUNCH_TIMER_INT / 2))) * 1000 + 20000
        Randomize()
        HardLockCheckPooling1TimerInterval = CInt(Int((HLC_POOLING1_TIMER_INT * Rnd()) + CInt(HLC_POOLING1_TIMER_INT / 2))) * 1000
        Randomize()
        HardLockCheckPooling2TimerInterval = CInt(Int((HLC_POOLING2_TIMER_INT * Rnd()) + CInt(HLC_POOLING2_TIMER_INT / 2))) * 1000
    End Sub
    Public Function IsDongleIsMounted() As Boolean
        If IsDebugMode() Then Return True

        If Not IsSetadMode And Not IsAppUnlocked() Then
            Dim lOK_Hardock As Boolean = False
            Dim lDatisCode As String = ""

            lOK_Hardock = TestLock(WorkingServerIP)

            Dim lIsWin7 As Boolean = IsWindows7()

            If Not lIsWin7 And Not lOK_Hardock Then
                Dim lIsNetwork As Boolean
                With HardLockUSB
                    'WritePrivateProfileString("TestLock","Step1",now.ToString,"Havades.ini")
                    If WorkingServerIP = "" Then
                        GetConnection()
                    End If
                    If WorkingServerIP <> "." And WorkingServerIP.ToLower <> "(local)" Then
                        lIsNetwork = True
                        .ServerIP = WorkingServerIP
                        .NetWorkINIT = True
                    Else
                        lIsNetwork = False
                        .Initialize = True
                    End If
                    'WritePrivateProfileString("TestLock","Step2",now.ToString,"Havades.ini")

                    .UserPassWord = HL_PWDUSB
                    'WritePrivateProfileString("TestLock","Step3",now.ToString,"Havades.ini")
                    .ShowTinyInfo = True
                    'WritePrivateProfileString("TestLock","Step4",now.ToString,"Havades.ini")

                    If .TinyErrCode = 0 Then
                        lDatisCode = .DataPartition
                        'WritePrivateProfileString("TestLock","Step5",now.ToString,"Havades.ini")
                        If lDatisCode = "DatisEng" Then
                            lOK_Hardock = True
                            'WritePrivateProfileString("TestLock", "Step6", Now.ToString, "Havades.ini")
                        End If
                    End If

                    If lIsNetwork Then
                        .NetWorkINIT = False
                    Else
                        .Initialize = False
                    End If
                End With
            End If

            Return lOK_Hardock
        Else
            Return True
        End If
    End Function
    Private Sub TimerHardLockCheckLaunch_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerHardLockCheckLaunch.Tick
        If Not IsActiveTimers() Then Exit Sub
        TimerHardLockCheckLaunch.Enabled = False
        If Not IsDongleIsMounted() Then
            MsgBoxF("خطا در عمليات بنيادين" & vbCrLf & "لطفاً با مسئول سامانه هماهنگ كنيد", "خطاي مهلك", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Error, MessageBoxDefaultButton.Button1)
            TerminateApplication()
            'Me.Close()
            'End
        End If
    End Sub
    Private Sub TimerHardLockCheckPooling1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerHardLockCheckPooling1.Tick
        If Not IsActiveTimers() Then Exit Sub
        TimerHardLockCheckPooling1.Enabled = False
        Exit Sub
        If Not IsDongleIsMounted() Then
            MsgBoxF("خطا در عمليات بنيادين" & vbCrLf & "لطفاً با مسئول سامانه هماهنگ كنيد", "خطاي مهلك", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Error, MessageBoxDefaultButton.Button1)
            TerminateApplication()
            'Me.Close()
            'End
        End If
    End Sub
    Private Sub TimerHardLockCheckPooling2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerHardLockCheckPooling2.Tick
        If Not IsActiveTimers() Then Exit Sub
        TimerHardLockCheckPooling2.Enabled = False
        Exit Sub
        If Not IsDongleIsMounted() Then
            MsgBoxF("خطا در عمليات بنيادين" & vbCrLf & "لطفاً با مسئول سامانه هماهنگ كنيد", "خطاي مهلك", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Error, MessageBoxDefaultButton.Button1)
            TerminateApplication()
            'Me.Close()
            'End
        End If
    End Sub

    Private Sub MenuItem121_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem121.Click
        Dim dlg As New frmReportLPPost
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem122_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem122.Click
        Dim dlg As New frmReportLPFeeder
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem126_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem126.Click
        Dim dlg As New frmReportParts("LP")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem127_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem127.Click
        Dim dlg As New frmReportParts("Light")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem129_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem129.Click
        Dim dlg As New frmReportParts("MP")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem124_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem124.Click
        Dim dlg As New frmReportMPPost
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuItem125_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem125.Click
        Dim dlg As New frmReportMPFeeder
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub frmMain_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        Dim UserAns As DialogResult
        If UpdateIsInProgress Then
            UserAns = DialogResult.OK
        Else
            UserAns = MsgBoxF("آيا با خروج از سامانه موافقيد؟", "حصول اطمينان", MessageBoxButtons.YesNo, MsgBoxIcon.MsgIcon_Question, MessageBoxDefaultButton.Button2)
        End If

        If UserAns = DialogResult.No Then
            e.Cancel = True
        Else
            e.Cancel = False
            SetKeyboardLayout(LANG_EN_US)
            PowerOffHavadesInfoTimer()
            If m_IsTelCMode Then
                If Not UpdateIsInProgress AndAlso Not mTelCInfo.Logout() Then
                    ShowError("عدم موفقيت در خروج از مرکز تماس" & vbCrLf & mTelCInfo.Message)
                    e.Cancel = True
                Else
                    m_IsLoginToCallCenter = False
                End If
            End If
            TerminateApplication()
        End If
    End Sub

    Private Sub MenuButtonItem1_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItem1.Click
        'Dim hlp As Help
        'hlp.ShowHelp(Me, HELP_NAMESPACE)
        Help.ShowHelp(Me, mcHavadesMainHelpFile, HelpNavigator.TableOfContents)
    End Sub

    Private Sub TimerNewRequest_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerNewRequest.Tick
        If Not IsActiveTimers() Then Exit Sub

        Static IsIn As Boolean = False
        If IsIn Then Exit Sub
        IsIn = True
        Try
            Dim Ds As New DataSet
            Dim Cnn As SqlConnection = New SqlConnection(GetConnection(False))
            LoadAreaDataTable(Cnn, Ds)
            Dim lAreaIDs As String = ""
            For Each lRow As DataRow In Ds.Tables("Tbl_Area").Rows
                lAreaIDs &= IIf(lAreaIDs <> "", "," & lRow("AreaId"), lRow("AreaId"))
            Next

            sbpInfo.Text = ""
            Dim AreaQuery As String = ""
            If Not IsCenter Then
                AreaQuery = " And AreaId=" & WorkingAreaId
            Else
                If lAreaIDs <> "" Then
                    AreaQuery = " And AreaId IN (" & lAreaIDs & ") "
                End If
            End If
            If Not BindingTable("SELECT CONVERT(varchar(20), GETDATE(), 20) AS DT", Cnn, Ds, "DateTime", , False, True) Then
                Exit Try
            End If
            BindingTable("SELECT Count(RequestId) As NewCnt FROM TblRequest WHERE DisconnectDatePersian='" & GetPersianDate(Ds.Tables("DateTime").Rows(0).Item("DT")) & "' AND IsLightRequest=0 AND EndJobStateId=4 " & AreaQuery, Cnn, Ds, "TblNewFetched")
            BindingTable("SELECT Count(RequestId) As WorkingCnt FROM TblRequest WHERE DisconnectDatePersian='" & GetPersianDate(Ds.Tables("DateTime").Rows(0).Item("DT")) & "' AND EndJobStateId=5" & AreaQuery, Cnn, Ds, "TblWorkingFetched")

            BindingTable("SELECT Count(RequestId) As NewCnt FROM TblRequest WHERE IsLightRequest=0 AND EndJobStateId= " & EndJobStates.ejs_New & AreaQuery, Cnn, Ds, "TblNewFetched_All")
            BindingTable("SELECT Count(RequestId) As WorkingCnt FROM TblRequest WHERE IsLightRequest=0 AND EndJobStateId=5" & AreaQuery, Cnn, Ds, "TblWorkingFetched_All")

            sbpInfo.Text = " جديد: " & Ds.Tables("TblNewFetched").Rows(0).Item("NewCnt") & "   در حال انجام: " & Ds.Tables("TblWorkingFetched").Rows(0).Item("WorkingCnt")
            sbpInfoAll.Text = "جديد کل: " & Ds.Tables("TblNewFetched_All").Rows(0).Item("NewCnt") & " - در حال انجام کل: " & Ds.Tables("TblWorkingFetched_All").Rows(0).Item("WorkingCnt")
            Dim lIsOffAlarm As Boolean = GetRegistryAbd("IsOffAlarm", "False")
            If lIsOffAlarm Then Exit Try
            If (Not (IsCenter Or IsSetadMode) Or (Not IsSetadMode And IsCenter And IsSpecialCenter)) Then
                If Ds.Tables("TblNewFetched").Rows(0).Item("NewCnt") > 0 Then
                    If Not IsMiscMode Then
                        BindingTable("SELECT Count(TblRequest.RequestId) As NewCnt FROM TblRequest INNER JOIN TblRequestInfo ON TblRequest.RequestId = TblRequestInfo.RequestId WHERE DisconnectDatePersian='" & GetPersianDate(Ds.Tables("DateTime").Rows(0).Item("DT")) & "' AND EndJobStateId=4 And IsLightRequest=0 AND (IsWatched=0 or IsWatched is null) And AreaUserId<> " & WorkingUserId & AreaQuery, Cnn, Ds, "TblNewReq")
                    Else
                        Dim lSQL As String =
                                "SELECT Count(TblRequest.RequestId) As NewCnt " &
                                " FROM TblRequest INNER JOIN TblRequestInfo ON TblRequest.RequestId = TblRequestInfo.RequestId INNER JOIN Tbl_AreaUser ON TblRequest.AreaUserId = Tbl_AreaUser.AreaUserId " &
                                " WHERE " &
                                "   DisconnectDatePersian='" & GetPersianDate(Ds.Tables("DateTime").Rows(0).Item("DT")) & "' " &
                                "   AND EndJobStateId=4 And IsLightRequest=0 AND (IsWatched=0 or IsWatched is null) " &
                                "   AND ( " &
                                "       Tbl_AreaUser.AreaId <> " & WorkingAreaId &
                                "       OR TblRequest.AreaUserId <> " & WorkingUserId & ")" &
                                AreaQuery.Replace("AreaId", "TblRequest.AreaId")

                        RemoveMoreSpaces(lSQL)
                        BindingTable(lSQL, Cnn, Ds, "TblNewReq")
                    End If
                    If Ds.Tables("TblNewReq").Rows(0).Item("NewCnt") > 0 Then
                        PlaySound("NewLP.wav")
                    End If
                End If
            End If
        Catch e1 As Exception
            'ShowError(e1)
        End Try
        IsIn = False
    End Sub


    Private Sub TimerSyncDT_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerSyncDT.Tick
        If Not IsActiveTimers() Then Exit Sub
        TimerSyncDT.Enabled = False
        Dim Ds As New DatasetCcRequester
        Dim Cnn As SqlConnection = New SqlConnection(GetConnection(False))
        If Not BindingTable("SELECT     CONVERT(varchar(20), GETDATE(), 20) AS DT", Cnn, Ds, "DateTime") Then
            TimerSyncDT.Enabled = True
            Exit Sub
        End If
        If Ds.Tables("DateTime").Rows.Count > 0 Then
            Dim dt1 As DateTime = Ds.Tables("DateTime").Rows(0).Item("DT")
            If TypeName(dt1) = "Date" Then
                Dim diff As Integer = DateDiff(DateInterval.Second, Now, dt1)
                If (diff < 0) Then
                    diff = -diff
                End If
                If diff > 20 Then
                    WriteError("TimerSyncDT_Tick -> ServerDateTime=" & dt1.ToString & ",LocalDateTime=" & Now.ToString())
                    Try
                        Today() = dt1
                        TimeOfDay = dt1
                    Catch ex As Exception
                    End Try
                    'diff = DateDiff(DateInterval.Second, Now, dt1)
                End If
            End If
        End If
        TimerSyncDT.Enabled = True
    End Sub

    Private Sub frmMain_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.Control And e.Shift Then
            If e.KeyCode = Keys.Menu Then
                If IsAdmin() Then
                    MenuItem27.Enabled = Not (MenuItem27.Enabled)
                    MenuItem27.Visible = Not (MenuItem27.Visible)
                End If
            End If
            If Not m_IsTelCMode Then
                If e.KeyCode = Keys.F7 Then
                    If Not m_HclsCall Is Nothing Then
                        m_HclsCall.ShowHCLSDebugForm()
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub MenuButtonItem2_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItem2.Click
        Dim dlg As New frmPostZaribDayPreview
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuButtonItem3_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItem3.Click
        Dim dlg As New frmPostZaribMonthPreview
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub


    Private Sub MenuButtonEkipProfile_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonEkipProfile.Click
        Dim dlg As New frmBaseTablesNotStd("TblEkipProfile")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub mnuAVLCare_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAVLCar.Click
        Dim dlg As New frmBaseTablesNotStd("Tbl_AVLCar")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub mnuAVLState_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAVLState.Click
        Dim dlg As New frmBS_BazdidAVLState
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuButtonItemTbl_LPCloserType_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItemTbl_LPCloserType.Click
        Dim dlg As frmBaseTables
        dlg = New frmBaseTables("Tbl_LPCloserType")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuButtonItemTbl_LPFaultType_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItemTbl_LPFaultType.Click
        Dim dlg As frmBaseTables
        dlg = New frmBaseTables("Tbl_LPFaultType")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuButtonItemTbl_LPTamirDisconnectFor_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItemTbl_LPTamirDisconnectFor.Click
        Dim dlg As frmBaseTables
        dlg = New frmBaseTables("Tbl_LPTamirDisconnectFor")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuButtonItemTbl_LPTamirRequestType_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItemTbl_LPTamirRequestType.Click
        Dim dlg As frmBaseTables
        dlg = New frmBaseTables("Tbl_LPTamirRequestType")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuButtonItemTbl_DisconnectLPRequestFor_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItemTbl_DisconnectLPRequestFor.Click
        Dim dlg As frmBaseTables
        dlg = New frmBaseTables("Tbl_DisconnectLPRequestFor")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuButtonItemTbl_DisconnectLightRequestFor_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItemTbl_DisconnectLightRequestFor.Click
        Dim dlg As frmBaseTables
        dlg = New frmBaseTables("Tbl_DisconnectLightRequestFor")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuButtonItemTbl_MPTamirDisconnectFor_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItemTbl_MPTamirDisconnectFor.Click
        Dim dlg As frmBaseTables
        dlg = New frmBaseTables("Tbl_MPTamirDisconnectFor")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuButtonItemTbl_MPTamirRequestType_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItemTbl_MPTamirRequestType.Click
        Dim dlg As frmBaseTables
        dlg = New frmBaseTables("Tbl_MPTamirRequestType")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuButtonItemTbl_DisconnectMPRequestFor_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItemTbl_DisconnectMPRequestFor.Click
        Dim dlg As frmBaseTables
        dlg = New frmBaseTables("Tbl_DisconnectMPRequestFor")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub



    Private Sub MenuButtonItemGlobalValue_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItemGlobalValue.Click
        Dim dlg As New frmGlobalValue
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub


    Private Sub MenuButtonItem5_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItem5.Click
        Dim dlg As New frmControlReportsFilters("خلاصه  گزارش ماهيانه انرژي توزيع نشده ناشي از حوادث و قطعي ها در شبكه هاي توزيع(فرم 3-4)", 11, , "(7-3)")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuButtonItem6_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItem6.Click
        Dim dlg As New frmMakeStdCodePage
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuButtonItem7_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItem7.Click
        Dim dlg As frmBaseTables
        dlg = New frmBaseTables("Tbl_MPCloserType")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuButtonItem8_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItem8.Click
        Dim dlg As frmBaseTables
        dlg = New frmBaseTables("Tbl_MPFaultType")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuButtonItem9_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItem9.Click
        Dim dlg As New frmControlReportsFilters("گزارش ماهيانه انرژي توزيع نشده (1-1-5)", 10, , "(7-4)")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub


    Private Sub MenuButtonItem10_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItem10.Click
        Dim dlg As New frmManoeuvrePreview("LP")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuButtonItem11_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItem11.Click
        Dim dlg As New frmManoeuvrePreview("MP")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuButtonItem12_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItem12.Click
        Dim dlg As New frmFogheToziPreview
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuButtonItem20_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItem20.Click
        Dim dlg As New frmBaseTablesNotStd("Tbl_LPPost", , , , , , , , , , True)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuButtonItem21_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItem21.Click
        Dim dlg As New frmBaseTablesNotStd("Tbl_LPFeeder", , , , , , , , , , True)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuButtonItem13_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItem13.Click
        Dim dlg As New frmSubscibersMontlyPreview(WorkingAreaId)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Dim dlgMonitoring As frmNewRequestsPreview = Nothing
    Private Sub mnuRepLoading_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemMonitoring.Click
        Try
            'Dim lIsAccess As Boolean = False
            'lIsAccess = IsAdmin() Or CheckUserTrustee("HomaCanViewMonitoring", 36, ApplicationTypes.Havades)
            'If Not lIsAccess Then
            '    ShowNoAccessMessageByTag("HomaCanViewMonitoring")
            '    Exit Sub
            'End If

            Dim dlg As New frmNewRequestsPreview(False, , , m_HclsCall, aIsHoma:=mIsHomaAccess)
            dlgMonitoring = dlg
            dlg.ShowDialog()
            dlg.Dispose()
        Catch ex As Exception
            Try
                ShowError(ex)
            Catch ex1 As Exception
                Stop
            End Try
        End Try
        dlgMonitoring = Nothing
    End Sub

    Private Sub MenuButtonItem22_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItem22.Click
        Dim dlg As frmBaseTables
        dlg = New frmBaseTables("Tbl_SatheMaghta")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub


    Private Sub MenuButtonItem4_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItem4.Click
        Dim dlg As New frmControlReportsFilters("خلاصه گزارش قطعي مشتركين", 4, , "(7-6)")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuButtonItem23_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItem23.Click
        Dim dlg As New frmControlReportsFilters("خلاصه گزارش وضعيت فيدرهاي فشار متوسط به تفکيک پست فوق توزيع", 5, , "(7-8)")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuButtonItem24_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItem24.Click
        Dim dlg As New frmControlReportsFilters("خلاصه گزارش قطعيها و انرژي توزيع نشده", 6, , "(7-11)")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuButtonItem25_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItem25.Click
        Dim dlg As New frmControlReportsFilters("خلاصه گزارش وضعيت پستهاي توزيع و شبکه‌هاي زير مجموعه آن", 7, , "(7-12)")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuButtonItem26_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItem26.Click
        Dim dlg As frmBaseTables
        dlg = New frmBaseTables("Tbl_Fuse")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub tbAddNewLPRequest_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Dim dlg As New frmNewRequestsPreview(False, , , m_HclsCall)
            dlgMonitoring = dlg
            dlg.ShowDialog()
            dlg.Dispose()
        Catch ex As Exception
            ShowError(ex)
        End Try
        dlgMonitoring = Nothing
    End Sub


    Private Sub mnuLightDaily_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuLightDaily.Click
        Dim dlg As New frmControlReportsFilters("گزارش روزانه روشنايي معابر", 8, , "(5-1)")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuButtonItem28_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItem28.Click
        Dim dlg As New frmControlReportsFilters("گزارش روزانه ارجاعات", 9, , "(10-2)")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub tbRequestsPreview_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbRequestsPreview.Click
        Try
            Dim dlg As New frmNewRequestsPreview(False, , , m_HclsCall, aIsHoma:=mIsHomaAccess)
            dlgMonitoring = dlg
            dlg.ShowDialog()
            dlg.Dispose()
            dlg = Nothing
        Catch ex As Exception
            Try
                ShowError(ex)
            Catch ex1 As Exception
                Stop
            End Try
        End Try
        dlgMonitoring = Nothing
    End Sub

    Private Sub MenuButtonItem30_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItem30.Click
        Dim dlg As New frmBaseTablesNotStd("View_DisconnectGroupLight")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuButtonItem32_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim dlg As New frmSearchFogheTozi(0)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuButtonItem33_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItem33.Click
        Dim dlg As New frmReportLPPostLoad
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuButtonItem34_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItem34.Click
        Dim dlg As frmBaseTables
        dlg = New frmBaseTables("Tbl_MPTransFactory")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuItemGroupPart_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemGroupPart.Click
        Dim dlg As New frmBaseTablesNotStd("Tbl_GroupPart")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuButtonItem35_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItem35.Click
        Dim dlg As frmBaseTables
        dlg = New frmBaseTables("Tbl_FogheToziRequesterUnit")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuMulitiStepConnection_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMulitiStepConnection.Click
        Dim dlg As New frmSearchBaseMP(16)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub mnuRptManoeurve_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRptManoeurve.Click
        Dim dlg As New frmMakeReportManoeurve("MP")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuButtonItem36_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItem36.Click
        Dim dlg As New frmSearchBaseLP(False, 12)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuButtonItem38_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItem38.Click
        Dim dlg As New frmSearchBaseLP(False, 13)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuButtonItem39_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItem39.Click
        Dim dlg As New frmSearchBaseMP(17)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuButtonItem40_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItem40.Click
        Dim dlg As New frmSearchBaseMP(18)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuButtonItem41_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItem41.Click
        Dim dlg As New frmSearchBaseMP(19)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub mnuRepMP_Mode4_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepMP_Mode4.Click
        Dim dlg As New frmSearchBaseMP(23)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuButtonItem42_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItem42.Click
        Dim dlg As New frmControlReportsFilters("خلاصه گزارش وضعيت فيدرهاي فشار متوسط به ترتيب تعداد کل قطعي", 12, , "(7-9)")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuButtonItem43_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItem43.Click
        Dim dlg As New frmReportCompare("گزارش مقايسه‌اي قطعي فيدرهاي فشار متوسط", 13)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub ButtonItem1_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If IsDongleIsMounted() Then
            MsgBox("قفل با موفقيت آزمايش شد", MsgBoxStyle.OkOnly, "قفل سخت افزاري")
        Else
            MsgBoxF("خطا در آزمايش قفل سخت افزاري" & vbCrLf & "لطفاً با مسئول سامانه هماهنگ كنيد", "خطاي مهلك", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Error, MessageBoxDefaultButton.Button1)
        End If
    End Sub

    Private Sub MenuButtonItem44_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItem44.Click
        Dim dlg As New frmBaseTables("Tbl_MPPostType")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuItemPublicTelNumbers_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemPublicTelNumbers.Click
        Dim dlg As New frmBaseTables("Tbl_PublicPhone")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuItemMonitoringLight_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemMonitoringLight.Click
        Try
            Dim dlg As New frmMonitoringLight(False)
            dlg.ShowDialog()
            dlg.Dispose()
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub mnuReportHourByHour_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReportHourByHour.Click
        Dim dlg As New frmReportHourByHourpower
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub tbLightRequestsPreview_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbLightRequestsPreview.Click
        Try
            Dim dlg As New frmMonitoringLight(False)
            dlg.ShowDialog()
            dlg.Dispose()
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub mnuGroupTrustee_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuGroupTrustee.Click
        Dim dlg As frmGroupTrusteePreview = New frmGroupTrusteePreview
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuLPFedersLoad_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuLPFedersLoad.Click
        Dim dlg As New frmReportLPFeederLoad
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuMinCountMP_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMinCountMP.Click
        Dim dlg As New frmSearchBaseMP(20)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuMinCountMPArea_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMinCountMPArea.Click
        Dim dlg As New frmSearchBaseMP(21)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuRptRequests_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRptRequests.Click
        Dim dlg As New frmSearchBase(0)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuRptChangeHistory_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRptChangeHistory.Click
        Dim dlg As New frmSearchBase(1)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuRptEzamEkipInfo_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRptEzamEkipInfo.Click
        Dim dlg As New frmSearchBase(2)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuRptReferToInfo_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRptReferToInfo.Click
        Dim dlg As New frmSearchBase(3)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuMinCountLPPost_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMinCountLPPost.Click
        Dim dlg As New frmSearchBaseLP(False, 14)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuMinCountLPFeeder_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMinCountLPFeeder.Click
        Dim dlg As New frmSearchBaseLP(False, 15)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuRepportSummaryLight_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepportSummaryLight.Click
        Dim dlg As New frmControlReportsFilters("خلاصه گزارش روشنايي معابر", 13, , "(5-4)")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuDisconnectPower_Zone_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDisconnectPower_Zone.Click
        Dim dlg As New frmSearchBase(4)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuDisconnectCount_Zone_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDisconnectCount_Zone.Click
        Dim dlg As New frmSearchBase(5)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuRequestCountDaily_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRequestCountDaily.Click
        Dim dlg As New frmSearchBase(6)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuDisconnectCountSubscribers1000_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDisconnectCountSubscribers1000.Click
        Dim dlg As New frmSearchBase(7)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuReport5_1_1_mode2_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReport5_1_1_mode2.Click
        Dim dlg As New frmControlReportsFilters("خلاصه گزارش 1_1_5 به تفکيک ناحيه", 14, , "(7-5)")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuLP_Mode1_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuLP_Mode1.Click
        Dim dlg As New frmSearchBaseLP(False, 16)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuRptReferToCountArea_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRptReferToCountArea.Click
        Dim dlg As New frmSearchBase(8)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuRptReferToCount_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRptReferToCount.Click
        Dim dlg As New frmSearchBase(9)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuRepMPStatusChart_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepMPStatusChart.Click
        Dim dlg As New frmControlReportsFilters("نمودار وضعيت فيدرهاي فشار متوسط", 15, , "(7-15)")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuBillBoard_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuBillBoard.Click
        Try
            Dim dlg As New frmBillBoard(False)
            dlg.ShowDialog()
            dlg.Dispose()
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub mnuBillBoardSlides_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuBillBoardSlides.Click
        Try
            Dim dlg As New frmBillBoardSlides
            dlg.ShowDialog()
            dlg.Dispose()
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub mnuRepMPDisconnectLPPost_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepMPDisconnectLPPost.Click
        Dim dlg As New frmSearchBaseMP(22)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuExcelReports_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim dlg As New frmReportsExcel
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub ButtonItem1_Activate_1(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim dlg As New frmSelectPostFeeders
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuInformMonitoring_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuInformMonitoring.Click
        Dim lDlg As New frmRequestInformMonitoring
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuSubecribersSourceSetting_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSubecribersSourceSetting.Click
        If Not IsSetadMode Or Not IsAdministrator() Then
            ShowError("جهت تغيير اين قسمت بايد با کاربري admin نسخه ستاد وارد شويد")
            Exit Sub
        End If
        Dim lDlg As New frmSubscriberSMSChoose
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuSmsSetting_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuLightRequestSmsSetting.Click
        Dim lDlg As New frmSmsSetting
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuUseType_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuUseType.Click
        Dim dlg As New frmBaseTables("Tbl_UseType")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuFeederPart_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFeederPart.Click
        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.mnu_FeederPart) Then
            ShowHavadesInfoMessage()
            Exit Sub
        End If

        Dim dlg As New frmBaseTablesNotStd("Tbl_FeederPart", , , , , , , , , , True)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuMPFeederKey_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMPFeederKey.Click
        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.ver_950320) And Not IsTozi("MazandaranWest") Then
            ShowHavadesInfoMessage()
            Exit Sub
        End If

        Dim dlg As New frmBaseTablesNotStd("Tbl_MPFeederKey", , , , , , , , , , True)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuNewRequestConfig_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewRequestConfig.Click
        Try
            Dim dlg As New frmNewRequestConfig
            dlg.ShowDialog()
            dlg.Dispose()
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub mnuIsKeyboardBase_Activate(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuIsKeyboardBase.Click
        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.mnu_KeyboardBase) Then
            ShowHavadesInfoMessage()
            Exit Sub
        End If

        'Dim Key As RegistryKey = Registry.LocalMachine
        Dim lIsKeyboardBase As String
        mIsKeyboardBase = Not mnuIsKeyboardBase.Checked
        mnuIsKeyboardBase.Checked = mIsKeyboardBase
        lIsKeyboardBase = IIf(mIsKeyboardBase, "True", "False")
        SetRegistryAbd("IsKeyboardBase", lIsKeyboardBase)
    End Sub

    Private Sub mnuRepDispaching_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepDispaching.Click
        Dim dlg As New frmReportDispaching
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuVersionInfo_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuVersionInfo.Click
        Try
            Dim lForm As New frmHTMLViewer(Me.Width - 10)
            lForm.ShowDialog()
            lForm.Dispose()
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    '====>  Skin Functions  <====
    Private Sub ChangeSkinUI(ByVal SchemaName As String)
        Dim ClrSchema As String = ""
        MnuUISkin1.Checked = False
        MnuUISkin2.Checked = False
        MnuUISkin3.Checked = False
        MnuUISkin4.Checked = False
        Select Case SchemaName
            Case "WinAqua", ""
                ClrSchema = "Standard"
                MnuUISkin1.Checked = True
            Case "B-Studio"
                ClrSchema = "LunaSilver"
                MnuUISkin2.Checked = True
            Case "Green", "Media"
                ClrSchema = "LunaOlive"
                MnuUISkin3.Checked = True
            Case "Metallic"
                ClrSchema = "LunaBlue"
                MnuUISkin4.Checked = True
        End Select

    End Sub
    Private Sub MnuUISkin_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MnuUISkin.Click
        ChangeSkin()
    End Sub

    Private Sub ChangeSkin(Optional ByVal aIsShowWindow As Boolean = True)
        Try
            Dim lDlgRes As DialogResult = DialogResult.OK
            Dim lForm As New frmSelectSkin

            lForm.SkinFileName = m_SkinName

            If aIsShowWindow Then
                lDlgRes = lForm.ShowDialog()
            End If

            If lDlgRes = DialogResult.OK Then
                Dim lSkinFilePath As String = lForm.SkinFullFileName
                If File.Exists(lSkinFilePath) Then
                    m_SkinUI.LoadSkinFile(lSkinFilePath)
                End If


                Dim lColor As Color

                lColor = Color.FromArgb(lForm.SkinColor)
                m_SkinColor = Color.FromArgb(lColor.R, lColor.G, lColor.B)

                lColor = Color.FromArgb(lForm.SkinTextColor)
                m_SkinTextColor = Color.FromArgb(lColor.R, lColor.G, lColor.B)

                m_SkinSchema = lForm.SkinSchema
                m_SkinName = lForm.SkinFileName
                m_SkinImageIndex = lForm.SkinImageIndex
                SetFormColors()

                If aIsShowWindow Then
                    'Dim Key As RegistryKey = Registry.LocalMachine
                    SetRegistryAbd("SkinName", lForm.SkinFileName)
                    SetRegistryAbd("SkinColor", lForm.SkinColor)
                    SetRegistryAbd("SkinTextColor", lForm.SkinTextColor)
                    SetRegistryAbd("SkinSchema", lForm.SkinSchema)
                    SetRegistryAbd("SkinImageIndex", lForm.SkinImageIndex)
                End If

                Me.Refresh()
            End If

            DrawCaption()
        Catch ex As Exception
            ShowError(ex)
        End Try

    End Sub
    Private Sub SetFormColors()
        Try
            Dim lBkgImage As Image = Nothing
            If m_SkinImageIndex > -1 Then
                lBkgImage = m_SkinImageList.Images(m_SkinImageIndex)
            End If
            pnlDownloadBox.BackgroundImage = lBkgImage
            GroupBox1.BackgroundImage = lBkgImage
            GroupBox2.BackgroundImage = lBkgImage
            lblWorkingProvinceName.BackgroundImage = lBkgImage
            If Not mfrmDownload Is Nothing Then
                mfrmDownload.BackgroundImage = lBkgImage
            End If
        Catch ex As Exception
        End Try

        Try
            If m_SkinImageIndex = -1 Then
                pnlDownloadBox.BackColor = m_SkinColor
                GroupBox1.BackColor = m_SkinColor
                GroupBox2.BackColor = m_SkinColor
                lblWorkingProvinceName.BackColor = m_SkinColor
            End If
        Catch ex As Exception
        End Try

        'lblDBVersion.BackColor = topSandBarDock.BackColor 'm_SkinColor
        lblWorkingProvinceName.ForeColor = Color.Maroon    'm_SkinTextColor

        ChangeSkinUI(m_SkinSchema)
    End Sub
    '============================

    Private Sub mnuManagerSMSST_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuManagerSMSST.Click
        Try
            Dim dlg As New frmManageSMSPreview_ST
            dlg.ShowDialog()
            dlg.Dispose()
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub mnuManagerSMSDC_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuManagerSMSDC.Click
        Try
            Dim dlg As New frmManageSMSPreview_DC
            dlg.ShowDialog()
            dlg.Dispose()
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub mnuDeleteRequest_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDeleteRequest.Click
        Try
            Dim lDlg As New frmDeleteRequest
            lDlg.ShowDialog()
            lDlg.Dispose()
            lDlg = Nothing
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub mnuDeletePostFeeder_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDeletePostFeeder.Click
        Try
            Dim lDlg As New frmBS_ChangePostFeeder("LPPost")
            lDlg.ShowDialog()
            lDlg.Dispose()
            lDlg = Nothing
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub mnuDeleteLPFeeder_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDeleteLPFeeder.Click
        Try
            Dim lDlg As New frmBS_ChangePostFeeder("LPFeeder")
            lDlg.ShowDialog()
            lDlg.Dispose()
            lDlg = Nothing
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub mnuDeleteFeeder_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDeleteFeeder.Click
        Try
            Dim lDlg As New frmBS_ChangePostFeeder("MPFeeder")
            lDlg.ShowDialog()
            lDlg.Dispose()
            lDlg = Nothing
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub mnuRepQuestionnaire_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepQuestionnaire.Click
        Dim dlg As New frmReportQuestionnaire
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuRepQuestionnaire2_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepQuestionnaire2.Click
        Dim dlg As New frmReportQuestionnaire("9-19")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuReport_9_17_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReport_9_17.Click
        Dim dlg As New frmReportQuestionnaire("9-17")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuRepDisconectCountMonthly_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub mnuRepDisconnectMonthlyCount_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepDisconnectMonthlyCount.Click
        Dim dlg As New frmControlReportsFilters("گزارش ماه به ماه تعداد قطعيهاي شبكه فشار متوسط", 16, , "(7-16)")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuRepCompare_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepDisconnectGroupSetGroup.Click
        Dim dlg As New frmControlReportsFilters("گزارش عيوب شبکه فشار متوسط", 17, , "(7-17)")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuDisconnectGroupSetGroup_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDisconnectGroupSetGroup.Click
        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.rep_DisconnectGroupSetGroupCount) Then
            ShowHavadesInfoMessage()
            Exit Sub
        End If

        Dim dlg As New frmBaseTablesNotStd("Tbl_DisconnectGroupSetGroup")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuRepGroupTamir_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepGroupTamir.Click
        Dim dlg As New frmControlReportsFilters("گزارش قطعي فيدرهاي فشار متوسط جهت انجام عمليات بابرنامه به تفکيک ناحيه", 18, , "(7-18)")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuLightRequestParts_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuLightRequestParts.Click
        Dim dlg As New frmSearchBaseLP(True, 0)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuLightUseParts_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuLightUseParts.Click
        Dim dlg As New frmSearchBaseLP(True, 1)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuLightDisconnectDaily_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuLightDisconnectDaily.Click
        Dim dlg As New frmSearchBaseLP(True, 2)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuRepSubscribers_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepSubscribers.Click
        Dim dlg As New frmReportSubscribers
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuMPPostFeederPeakDaily_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMPPostFeederPeakDaily.Click
        Dim dlg As New frmMPFeederMonthlyLoadPeak(True)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuRepRequestAll_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepRequestAll.Click
        Dim dlg As New frmSearchBase(10)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuRepPowerIndex_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepPowerIndex.Click
        Dim lDlg As New frmReportPowerIndex
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub
    Private Sub mnuRepPowerIndex_Mode2_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepPowerIndex_Mode2.Click
        Dim lDlg As New frmReportPowerIndex("rptPowerIndexCompare_Mode2.rpt")
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuRepGroupSetGroup_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepGroupSetGroup.Click, munRep_10_15_1.Click, munRep_10_7_3.Click, mnuRep_11_5.Click, mnuRep_11_6.Click, mnuRep_11_7.Click, mnuReport_10_25.Click
        Dim lTag = CType(sender, ToolStripMenuItem).Tag
        Dim lDlg As New frmReportGroupSetGroup(lTag)
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuMPFeederPeak_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMPFeederPeak.Click
        Dim lDlg As New frmReportMPFeederPeak(False)
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuRepRequestAll2_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepRequestAll2.Click
        Dim dlg As New frmSearchBase(11)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuRepAllDisconnectFeeder_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepAllDisconnectFeeder.Click
        Dim lDlg As New frmReportAllDisconnectFeeder
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuRepDCPServer121_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepDCPServer121.Click
        Dim lDlg As New frmReportDCPServer121Compare
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuRep5_1_1_Byerver121_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRep5_1_1_Byerver121.Click
        Dim dlg As New frmControlReportsFilters("خلاصه گزارش 1_1_5 به تفکيک امور", 19, , "(7-19)", True)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuErjaReasonBase_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuErjaReasonBase.Click
        Dim dlg As New frmBaseTablesNotStd("Tbl_ErjaReason")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuMonitoringErja_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMonitoringErja.Click
        Try
            Dim dlg As New frmMonitoringErja(False)
            dlg.ShowDialog()
            dlg.Dispose()
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub mnuMonitoringTamir_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMonitoringTamir.Click
        Try
            Dim dlg As New frmMonitoringTamiRequest()
            dlg.ShowDialog()
            dlg.Dispose()
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub mnuErjaParts_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim dlg As New frmBaseTablesNotStd("Tbl_ErjaPart")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub menuErjaOperations_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuErjaOperations.Click
        Dim dlg As New frmBaseTablesNotStd("Tbl_ErjaOperation")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuRepUndoneReason_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepUndoneReason.Click, mnuRepUndoneReasonArea.Click
        Dim lTag As String = CType(sender, ToolStripMenuItem).Tag
        Dim lDlg As New frmReportUndoneReason(lTag)
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuLoginLog_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuLoginLog.Click
        Dim lForm As New frmLoginLog
        lForm.ShowDialog()
        lForm.Dispose()
        lForm = Nothing
    End Sub
    Private Sub mnuLoginFailed_Click(sender As Object, e As EventArgs) Handles mnuLoginFailed.Click
        Dim lDlg As New frmLoginFailed
        lDlg.ShowDialog()
        lDlg.Dispose()
        lDlg = Nothing
    End Sub

    Private Sub mnuLPPostCapacity_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuLPPostCapacity.Click
        Dim dlg As New frmReportLPPost("postcapacity")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuLPPostFeederCapacity_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuLPPostFeederCapacity.Click
        Dim dlg As New frmReportLPPost("postfeedercapacity")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuRepDCRecloser_Activate(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuRepDCRecloser.Click
        Dim dlg As New frmControlReportsFilters("خلاصه گزارش وضعيت فيدرهاي فشار متوسط بر اساس با‌برنامه، بي‌برنامه و ريکلوزري", 20, , "(7-20)")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuRepMPFeederPeakEnergyMonthly_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepMPFeederPeakEnergyMonthly.Click
        Dim lDlg As New frmReportMPFeederPeak(True)
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuRepAreaFeeders_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepAreaFeeders.Click
        Dim lDlg As New frmReportAreaFeeders
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuMPPostMonthly_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMPPostMonthly.Click
        Dim lDlg As New frmMPPostMonthly
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuRepLPPostNotLoad_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepLPPostNotLoad.Click, mnuReport_8_22.Click
        Dim lTag As String = CType(sender, ToolStripMenuItem).Tag
        Dim dlg As New frmReportLPPostNotLoad(lTag)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub mnuReport_8_23_Click(sender As Object, e As EventArgs) Handles mnuReport_8_23.Click
        Dim dlg As New frmReportLPFeederNotLoad()
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub mnuReport_8_24_Click(sender As Object, e As EventArgs) Handles mnuReport_8_24.Click
        Try
            Dim lDlg As New frmMPFeederPeakDayNight("Report_8_24")
            lDlg.ShowDialog()
            lDlg.Dispose()
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub
    Private Sub mnuRep_1_13_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRep_1_13.Click
        Dim dlg As New frmSearchBase(12)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuRepMPPostMonthly_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepMPPostMonthly.Click
        Dim lDlg As New frmReportMPPostMonthly
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuSpec_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSpec.Click
        Dim lDlg As New frmBS_Spec
        lDlg.ShowDialog()
        Try
            lDlg.Dispose()
        Catch ex As Exception
        End Try
        lDlg = Nothing
    End Sub

    Private Sub mnuLPTrans_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuLPTrans.Click
        Try
            Dim lIsAccess As Boolean = IsAdmin() Or CheckUserTrustee("CanManageLPTrans", 9, ApplicationTypes.Havades)
            If Not lIsAccess Then
                MsgBoxF("برای استفاده از بخش نیاز به سطح دسترسی زیر برای کاربر می‌باشد:" & vbCrLf & "- توانايي مديريت و رصد ترانس‌هاي توزيع",
                        "عدم دسترسی", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Exclamation, MessageBoxDefaultButton.Button1)
                Exit Sub
            End If

            Dim lDlg As New frmLPTransPreview
            lDlg.ShowDialog()
            lDlg.Dispose()
            lDlg = Nothing
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub
    Private Sub mnuLPTransInstaller_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuLPTransInstaller.Click
        Try
            Dim lDlg As New frmBaseTables("Tbl_LPTransInstaller")
            lDlg.ShowDialog()
            lDlg.Dispose()
            lDlg = Nothing
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub tbd_MPPost_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbd_MPPost.Click
        Dim dlg As New frmBaseTablesNotStd("View_MPPost", , , , , , , , , , True)
        dlg.ShowDialog()
        dlg.Dispose()
        dlg = Nothing
    End Sub

    Private Sub tbd_MPFeeder_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbd_MPFeeder.Click
        Dim dlg As New frmBaseTablesNotStd("Tbl_MPFeeder", , , , , , , , , , True)
        dlg.ShowDialog()
        dlg.Dispose()
        dlg = Nothing
    End Sub

    Private Sub tbd_LPPost_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbd_LPPost.Click
        Dim dlg As New frmBaseTablesNotStd("Tbl_LPPost", , , , , , , , , , True)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub tbd_LPFeeder_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbd_LPFeeder.Click
        Dim dlg As New frmBaseTablesNotStd("Tbl_LPFeeder", , , , , , , , , , True)
        dlg.ShowDialog()
        dlg.Dispose()
        dlg = Nothing
    End Sub

    Private Sub mnuRepCheckLies_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepCheckLies.Click
        Dim dlg As New frmReportCheckLies
        dlg.ShowDialog()
        dlg.Dispose()
        dlg = Nothing
    End Sub

    Private Sub mnuDisconnectPower_Area_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDisconnectPower_Area.Click
        Dim dlg As New frmSearchBase(13)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuDisconnectCount_Area_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDisconnectCount_Area.Click
        Dim dlg As New frmSearchBase(14)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuDisconnectCountSubscribersArea1000_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDisconnectCountSubscribersArea1000.Click
        Dim dlg As New frmSearchBase(15)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub mnuReport_1_20_Click(sender As Object, e As EventArgs) Handles mnuReport_1_20.Click
        Dim dlg As New frmSearchBase(19)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub mnuRepExcel_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepExcelform1.Click, mnuRepExcelform2.Click, mnuRepExcelform3.Click, mnuRepExcelform4.Click, mnuRepExcelform5.Click, mnuRepExcelform6.Click, mnuRepExcelform7.Click, mnuRepExcelformDaily.Click, mnuRepExcelformDaily_02.Click, mnuRepExcelTavanir_01.Click, mnuRepExcelImportantEvents.Click, mnuRepExcelGISLoadings.Click, mnuRepExcelformDaily_03.Click, mnuRepExcelTavanir_02.Click, mnuRepExcelTavanir_03.Click, mnuRepExcelformDaily_04.Click, mnuRepExcelformDaily_05.Click, mnuRepExcelweek.Click, mnuRepExcelLPFeederPayesh.Click, mnuRepExcelMPFeederManagement.Click, mnuRepExcelLPFeederManagement.Click, mnuRepExcelMPFeederDaily.Click, mnuRepExcelDisconnectGroupSet.Click

        Dim lRepType As String = ""
        Try
            lRepType = CType(sender, ToolStripMenuItem).Tag
            lRepType = lRepType.Replace("mnuRepExcel", "")

            If lRepType = "MPFeederDaily" AndAlso Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.ver_940410) Then
                ShowHavadesInfoMessage()
                Exit Sub
            End If
            If lRepType = "DisconnectGroupSet" AndAlso Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.ver_940510) Then
                ShowHavadesInfoMessage()
                Exit Sub
            End If

            Dim dlg As New frmReportsExcel(lRepType)
            dlg.ShowDialog()
            dlg.Dispose()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub mnuRepWantedInfo_Activate(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuRepWantedInfo.Click
        Dim lDlg As New frmReportPowerIndex("rptWantedInfo.rpt")
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuRep_LPAndSerghati_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRep_LPAndSerghati.Click
        Dim lDlg As New frmReportPowerIndex("rptLPDisconnectAndSerghati.rpt")
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub rptRep_ListenRequestCallList_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rptRep_ListenRequestCallList.Click
        Dim lDlg As New frmReportCallListen("rptListenRequestCallList.rpt")
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub rptRep_ListenUserCallCount_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rptRep_ListenUserCallCount.Click
        Dim lDlg As New frmReportCallListen("rptListenUserCallCount.rpt")
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuRepUserLogin_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepUserLogin.Click
        Dim lDlg As New frmReportCallListen("rptUserLogin.rpt")
        lDlg.ShowDialog()
        lDlg.Dispose()
        lDlg = Nothing
    End Sub

    Private Sub mnuRepuserAccess_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepuserAccess.Click
        Dim lDlg As New frmReportUserAccess
        lDlg.ShowDialog()
        lDlg.Dispose()
        lDlg = Nothing
    End Sub

    Private Sub mnuRep_MPCriticalFeeder_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRep_MPCriticalFeeder.Click
        Dim dlg As New frmReportMPFeeder("Criticals")
        dlg.ShowDialog()
        dlg.Dispose()
        dlg = Nothing
    End Sub
    Private Sub mnuRep_LPCriticalFeeder_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRep_LPCriticalFeeder.Click
        Dim dlg As New frmReportLPFeeder("Criticals")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuComunicationSystems_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepExccelComunicationSystems.Click
        Dim lDlg As New frmExcelComunicationSystems
        lDlg.ShowDialog()
        lDlg.Dispose()
        lDlg = Nothing
    End Sub

    Private Sub mnuRep_LPDisconnectDaily_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRep_LPDisconnectDaily.Click
        Dim dlg As New frmSearchBaseLP(False, 17)
        dlg.ShowDialog()
        dlg.Dispose()
        dlg = Nothing
    End Sub

    Private Sub mnuRep_1_17_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRep_1_17.Click
        Dim dlg As New frmSearchBase(16)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuRepMPPostTransLoadMonthly_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepMPPostTransLoadMonthly.Click
        Dim lDlg As New frmReportMPFeederPeak(True, True)
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuRepMPPostTransLoadDaily_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepMPPostTransLoadDaily.Click
        Dim lDlg As New frmReportMPFeederPeak(False, True)
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Protected Overrides Sub OnKeyDown(ByVal e As System.Windows.Forms.KeyEventArgs)
        If Not IsDebugMode() Then Exit Sub

        If e.Control And e.KeyCode = Keys.F5 Then
            If IO.File.Exists("Reports\ReportHelp.xml") Then
                Try
                    DsReportHelp.ReportHelp.Rows.Clear()
                    DsReportHelp.ReportHelp.Clear()
                    DsReportHelp.Clear()
                    DsReportHelp.ReadXml("Reports\ReportHelp.xml")
                Catch ex As Exception
                End Try
            End If
        End If
    End Sub

    Private Sub mnuReportsHelp_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReportsHelp.Click
        Help.ShowHelp(Me, mcReportHelpFile, HelpNavigator.TableOfContents)
    End Sub

    Private Sub mnuFTReason_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFTReason.Click
        Try
            Dim lDlg As New frmBS_FTReason
            lDlg.ShowDialog()
            lDlg.Dispose()
            lDlg = Nothing
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub mnuMainPeak_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMainPeak.Click
        Try
            Dim lDlg As New frmMainPeak
            lDlg.ShowDialog()
            lDlg.Dispose()
            lDlg = Nothing
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub mnuRepMainPeak_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepMainPeak.Click
        Try
            Dim lDlg As New frmReportMainPeak
            lDlg.ShowDialog()
            lDlg.Dispose()
            lDlg = Nothing
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub mnuRep_FeederParts_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRep_FeederParts.Click
        Dim dlg As New frmReportFeederPart
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

#Region "HCLS 3000"

    Dim m_CallList As New Collection

    Private Sub m_HclsCall_OnNewCall(ByVal CallId As Long, ByVal CallerId As String, ByVal ChannelNumber As Integer, ByRef sender As HclsCall)
        SaveLogForCCPopup(String.Format("m_HclsCall_OnNewCall"))
        Dim cl As New CallInfo
        cl.CallId = CallId
        cl.CallerId = CallerId
        cl.ChannelNumber = ChannelNumber
        m_CallList.Add(cl)
    End Sub
    Private Sub m_HclsCall_OnEndCall(ByVal CallId As Long, ByVal CallTime As Long, ByVal IsSaveOK As Boolean, ByRef sender As HclsCall)
        SaveLogForCCPopup(String.Format("m_HclsCall_OnEndCall"))
        If Not IsSaveOK Then
            'ShowError("CallId = " & CallId & vbCrLf & "Call Time = " & CallTime, False)
            m_CallRequestList.Add(CStr(CallId), CallTime)
        End If

        If CallPoolingIndex = 0 Or CallPoolingIndex = 2 Then
            'CheckIfNotAnswer(CallId)
        End If
    End Sub
    Private Sub m_HclsCall_OnRinging(ByVal CallId As Long, ByVal CallerId As String, ByVal ChannelNumber As Integer, ByRef sender As HclsCall)

    End Sub
    Private Sub m_HclsCall_OnCallerIdChanged(ByVal CallId As Long, ByVal CallerId As String, ByVal ChannelNumber As Integer, ByRef sender As HclsCall)
        For i As Integer = 1 To m_CallList.Count
            Dim cl As CallInfo = m_CallList.Item(i)
            If cl.CallId = CallId And cl.ChannelNumber = ChannelNumber Then
                cl.CallerId = CallerId
                Exit For
            End If
        Next
    End Sub

    Private Sub mTelC_OnNewCall(ByRef sender As CTelCInfo, ByVal TelCInfo As TelCStatus)
        SaveLogForCCPopup(String.Format("OnNewCall Event"))
        Try
            Dim cl As New CallInfo
            cl.CallId = sender.CallId
            cl.CallerId = TelCInfo.CallerId
            cl.CallEventType = sender.CallEventType
            cl.SubcriberCode = TelCInfo.SubscriberCode
            m_CallList.Add(cl)
            SaveLogForCCPopup(String.Format("OnNewCall: (CallId:{0})-(CallerId:{1})", cl.CallId, cl.CallerId))
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub
    Private Sub mTelC_OnEndCall(ByRef sender As CTelCInfo, ByVal TelCInfo As TelCStatus, ByVal CallDuration As Integer)
        Dim lCallId As Integer = sender.CallId
        SaveLogForCCPopup(String.Format("OnEndCall: (CallId:{0})", lCallId))
        If lCallId > -1 Then
            Try
                m_CallRequestList.Add(lCallId, CallDuration)
            Catch ex As Exception
            End Try
        End If

        If CallPoolingIndex = 0 Or CallPoolingIndex = 2 Then
            'CheckIfNotAnswer(sender.CallId)
        End If
    End Sub

    Private Sub TimerSMSPopup_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles TimerSMSPopup.Tick
        If Not IsActiveTimers() Then Exit Sub
        If Not mIsActiveSMSPopupSystem OrElse m_CallList.Count > 0 Then
            Exit Sub
        End If

        Dim SMSPopupEnabled As Object = GetRegistryAbd("IsSMSPopupEnabled", "False")
        If IsNothing(SMSPopupEnabled) Then SMSPopupEnabled = False
        Dim lIsSMSPopupEnabled As Boolean = SMSPopupEnabled
        If Not lIsSMSPopupEnabled Then
            Exit Sub
        End If

        Dim lCnn As SqlConnection = New SqlConnection(GetConnection())
        Dim lDs As New DatasetSMS
        Dim lTbl As DatasetSMS.Tbl_GISSMSReceviedDataTable = lDs.Tbl_GISSMSRecevied
        Dim lRow As DatasetSMS.Tbl_GISSMSReceviedRow = Nothing
        Dim lSQL As String = ""

        Try

            lSQL = "exec spGetNewSubscriberSMS " & WorkingUserId
            BindingTable(lSQL, lCnn, lDs, "Tbl_GISSMSRecevied", , True)

            If lTbl.Count > 0 Then
                lRow = lTbl(0)

                'If lRow.MediaTypeId = MediaTypes.MobileApp AndAlso IsTozi("Yazd") Then
                '    Throw New Exception("MobileApp Access Denied")
                'End If

                Dim cl As New CallInfo With {
                    .CallId = lRow.GISSMSReceviedId,
                    .ChannelNumber = -1,
                    .IsSMSPopup = True,
                    .SubscriberName = lRow.SubscriberName,
                    .Address = lRow.Address
                }

                If Not lRow.IsMobileNoNull Then
                    cl.CallerId = lRow.MobileNo
                Else
                    cl.CallerId = ""
                End If
                If Not IsDBNull(lRow("SubcriberCode")) Then
                    cl.SubcriberCode = lRow.SubcriberCode
                Else
                    cl.SubcriberCode = ""
                End If
                If Not IsDBNull(lRow("LPPostId")) Then
                    cl.LPPostId = lRow.LPPostId
                End If
                If Not IsDBNull(lRow("LPFeederId")) Then
                    cl.LPFeederId = lRow.LPFeederId
                End If
                If Not IsDBNull(lRow("DebtValue")) Then
                    cl.DebtValue = lRow.DebtValue
                End If

                m_CallList.Add(cl)
            End If

        Catch ex As Exception
            WriteError(ex.Message, "Tac\Havades\SMS")
            If IsDebugMode() Then
                ShowError(ex)
            End If
        End Try
    End Sub
    Private Sub TimerAVL_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles TimerAVL.Tick
        If Not IsActiveTimers() Then Exit Sub
        If Not mIsActiveAVLSystem Then
            Exit Sub
        End If
        Dim IsAVLAlarmEnabled As Boolean = GetRegistryAbd("IsAVLAlarmEnabled", False, False)
        If Not IsAVLAlarmEnabled Then
            Exit Sub
        End If

        Dim lCnn As SqlConnection = New SqlConnection(GetConnection())
        Dim lDs As New DataSet
        Dim lRow As DataRow = Nothing
        Dim lSQL As String = ""

        Try

            lSQL =
                    " SELECT 'فشار متوسط' AS ReqType, TblRequest.RequestId, RequestNumber, TblRequest.DisconnectDatePersian, TblRequest.DisconnectTime, Tbl_Master.Name AS MasterName, Tbl_AVLCar.AVLCarCode FROM TblMPRequest INNER JOIN TblRequest ON TblMPRequest.MPRequestId = TblRequest.MPRequestId INNER JOIN TblBazdid ON TblRequest.RequestId = TblBazdid.RequestId INNER JOIN Tbl_Master ON TblBazdid.MasterId = Tbl_Master.MasterId INNER JOIN Tbl_AVLCar ON TblBazdid.AVLCarId = Tbl_AVLCar.AVLCarId WHERE TblMPRequest.EndJobStateId IN (4, 5) AND NOT AVLData IS NULL " &
                    " UNION " &
                    " SELECT 'فشار ضعيف' AS ReqType, TblRequest.RequestId, RequestNumber, TblRequest.DisconnectDatePersian, TblRequest.DisconnectTime, Tbl_Master.Name AS MasterName, Tbl_AVLCar.AVLCarCode FROM TblLPRequest INNER JOIN TblRequest ON TblLPRequest.LPRequestId = TblRequest.LPRequestId INNER JOIN TblBazdid ON TblRequest.RequestId = TblBazdid.RequestId INNER JOIN Tbl_Master ON TblBazdid.MasterId = Tbl_Master.MasterId INNER JOIN Tbl_AVLCar ON TblBazdid.AVLCarId = Tbl_AVLCar.AVLCarId WHERE TblLPRequest.EndJobStateId IN (4, 5) AND NOT AVLData IS NULL "
            BindingTable(lSQL, lCnn, lDs, "Tbl_AVLAlarm", , True, , , , , True)

            If lDs.Tables.Contains("Tbl_AVLAlarm") AndAlso lDs.Tables("Tbl_AVLAlarm").Rows.Count > 0 Then
                frmAVLAlarm.ShowAVLForm(lSQL)
            End If

        Catch ex As Exception
            WriteError(ex.ToString, "Tac\Havades\AVL")
            If IsDebugMode() Then
                ShowError(ex)
            End If
        End Try
    End Sub

    Function GetNewCall() As CallInfo
        For i As Integer = 1 To m_CallList.Count
            Dim cl As CallInfo = m_CallList.Item(i)
            If Not cl.IsShowDialog Then
                Return cl
            End If
        Next
        Return Nothing
    End Function
    Sub RemoveCall(ByVal cl As CallInfo)
        For i As Integer = 1 To m_CallList.Count
            Dim cl1 As CallInfo = m_CallList.Item(i)
            If cl1.CallId = cl.CallId Then
                m_CallList.Remove(i)
                cl.CallerId = Nothing
                cl.CallId = Nothing
                cl.ChannelNumber = Nothing
                Exit Sub
            End If
        Next
    End Sub
    Private Sub RemoveOldCalls()
        Dim cl As CallInfo

        If Not ((CallPoolingIndex = 0 Or CallPoolingIndex = 2) And m_CallList.Count > 0) Then
            Return
        End If
        Dim i As Integer = 0

        While i < m_CallList.Count
            i = i + 1
            cl = m_CallList(i)

            If cl.IsShowDialog = False Then
                Dim IsRemove As Boolean = False
                If cl.IsSMSPopup Then

                ElseIf Not m_IsTelCMode Then
                    Dim l_ChannelInfo As HclsCall.ChannelInfo
                    l_ChannelInfo = m_HclsCall.GetChannelInfo(cl.ChannelNumber, cl.CallId)
                    If IsNothing(l_ChannelInfo.StatusStr) Then
                        IsRemove = True
                    End If
                Else
                    If cl.CallId = mTelCInfo.CallId Then
                        If Not mTelCInfo.IsCalling Then
                            IsRemove = True
                        End If
                    Else
                        IsRemove = True
                    End If
                End If
                If IsRemove Then
                    MakeAutoRequest(cl)
                    RemoveCall(cl)
                    i = i - 1
                End If
            End If
        End While
    End Sub
    Private Sub TimerCallPooling_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerCallPooling.Tick
        SaveLogForCCPopup(String.Format("TimerCallPooling - Step -1"), True)
        If Not IsActiveTimers() Then Exit Sub
        ShowTimers()
        If CallPoolingIndex = -1 Then
            SaveLogForCCPopup(String.Format("TimerCallPooling - Step -2"), True)
            Exit Sub
        End If
        RemoveOldCalls()

        SaveLogForCCPopup(String.Format("TimerCallPooling - Step -3 - ({0},{1})", m_ShowRequestCount, MaxShowRequestCount))
        If m_ShowRequestCount < MaxShowRequestCount Then

            SaveLogForCCPopup(String.Format("TimerCallPooling - Step -4"), True)
            If m_CallList.Count > 0 Then
                SaveLogForCCPopup(String.Format("TimerCallPooling - Step 1"))
                Dim cl As CallInfo = GetNewCall()
                If IsNothing(cl) Then
                    Return
                End If

                SaveLogForCCPopup(String.Format("TimerCallPooling - Step 2"))

                cl.IsShowDialog = True

                If cl.IsSMSPopup Then

                ElseIf CallPoolingIndex = 0 Or CallPoolingIndex = 2 Then
                    If Not m_IsTelCMode Then

                        Dim l_ChannelInfo As HclsCall.ChannelInfo
                        l_ChannelInfo = m_HclsCall.GetChannelInfo(cl.ChannelNumber, cl.CallId)
                        If IsNothing(l_ChannelInfo.StatusStr) Then
                            Return
                        End If

                    Else

                        If cl.CallId = mTelCInfo.CallId Then
                            SaveLogForCCPopup(String.Format("TimerCallPooling - Step 3"))
                            If Not mTelCInfo.IsCalling Then
                                SaveLogForCCPopup(String.Format("TimerCallPooling - Step 4"))
                                Return
                            End If
                        Else
                            SaveLogForCCPopup(String.Format("TimerCallPooling - Step 5"))
                            Return
                        End If

                    End If
                End If

                m_ShowRequestCount += 1

                SaveLogForCCPopup(String.Format("TimerCallPooling - Step 6 - Popup OK"))
                Try
                    Dim dlg As Form
                    If Not mIsKeyboardBase Then
                        dlg = New frmRequest(-1, cl, Nothing)
                    Else
                        dlg = New frmRequestKeyboardBase(-1, cl, Nothing)
                    End If
                    If dlgMonitoring Is Nothing Then
                        dlg.ShowDialog()
                    Else
                        dlg.ShowDialog(dlgMonitoring)
                    End If
                    dlg.Dispose()
                Catch ex As Exception
                    ShowError(ex)
                End Try

                m_ShowRequestCount -= 1
                RemoveCall(cl)
            End If
        ElseIf CallPoolingIndex = 0 Or CallPoolingIndex = 2 Then

        End If

    End Sub

    Private Sub CheckIfNotAnswer(ByVal aCallId As Long)
        For i As Integer = 1 To m_CallList.Count
            Dim cl As CallInfo = m_CallList(i)
            If cl.CallId = aCallId Then
                If Not cl.IsShowDialog Then
                    'cl.IsShowDialog = True
                    MakeAutoRequest(cl)
                    'RemoveCall(cl)
                End If
            End If
        Next
    End Sub

    Private Sub TimerHcls_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerHcls.Tick
        SaveLogForCCPopup(String.Format("TimerHcls_Tick - Step 1"))
        If Not m_IsCallCenterEnable Then Exit Sub
        SaveLogForCCPopup(String.Format("TimerHcls_Tick - Step 2"))
        If (m_IsTelCMode And Not IsActiveCallsServer) OrElse (Not m_IsTelCMode And m_HclsCall Is Nothing) Then

            Dim lStatus As String = String.Format("m_IsTelCMode: {0} - IsActiveCallsServer: {1} - m_HclsCall: {2}", m_IsTelCMode, IsActiveCallsServer, IsNothing(m_HclsCall).ToString())
            SaveLogForCCPopup(String.Format("TimerHcls_Tick - Step 3: {0}", lStatus))

            If m_LastHclsStatus <> 0 Then
                m_LastHclsStatus = 0
                TimerHcls.Enabled = False
            End If
        Else
            SaveLogForCCPopup(String.Format("TimerHcls_Tick - Step 4"))
            If Not m_IsTelCMode Then
                SaveLogForCCPopup(String.Format("TimerHcls_Tick - Step 5"))
                If m_HclsCall.HCLSConnected Then
                    If m_LastHclsStatus <> 2 Then
                        Dim lIsMitelCheck As Boolean = m_LastHclsStatus > 0
                        If mIsLogMitel Then SaveLog("m_LastHclsStatus = " & m_LastHclsStatus, "MITEL.log")
                        m_LastHclsStatus = 2

                        If m_IsMitelMode Then
                            If mIsLogMitel Then
                                Dim lMsg As String = vbCrLf
                                lMsg &= "*** HCLS Connected ***" & vbCrLf
                                SaveLog(lMsg, "MITEL.log")
                            End If

                        End If
                    End If
                Else
                    If m_LastHclsStatus <> 1 Then
                        m_LastHclsStatus = 1

                        If m_IsMitelMode And mIsLogMitel Then
                            Dim lMsg As String = vbCrLf
                            lMsg &= "!!! HCLS Disconnected !!!" & vbCrLf
                            SaveLog(lMsg, "MITEL.log")
                        End If

                        ShowError("در اين لحظه ارتباط با سيستم ضبط مکالمات قطع گرديد" & vbCrLf & "لطفا با مدير شبکه تماس بگيريد")
                    End If
                End If
            Else
                SaveLogForCCPopup(String.Format("TimerHcls_Tick - Step 6"))
                If Not mTelCInfo.IsDisconnected Then
                    SaveLogForCCPopup(String.Format("TimerHcls_Tick - Step 7"))
                    If m_LastHclsStatus <> 2 Then
                        m_LastHclsStatus = 2
                    End If
                Else
                    SaveLogForCCPopup(String.Format("TimerHcls_Tick - Step 8"))
                    If m_LastHclsStatus <> 1 Then
                        SaveLogForCCPopup(String.Format("TimerHcls_Tick - Step 9"))
                        m_LastHclsStatus = 1
                        ShowError("در اين لحظه ارتباط با سيستم ضبط مکالمات قطع گرديد" & vbCrLf & "لطفا با مدير شبکه تماس بگيريد")
                        m_IsLoginToCallCenter = False
                    Else

                        TryConnectToCC()

                    End If
                End If
                CheckCallCenterToolbarStatus()
            End If
        End If
    End Sub
    Private Sub TryConnectToCC()
        If mIsManualCCLogout Then Exit Sub
        Dim lServerName As String = GetRegistryAbd("TAPIServerName", , True)
        Try
            If Not mTelCInfo.IsLogin Then
                mTelCInfo.DirectLogOff(m_MitelAgentId, m_CRMPsw)
            End If
        Catch ex As Exception
        End Try
        If mTelCInfo.Login(lServerName, m_MitelExtension, m_CRMPsw, False, m_MitelAgentId, "", False) Then
            m_IsLoginToCallCenter = True
            ShowInfo("هم اکنون ارتباط شما با مرکز تماس برقرار است.", True)
        End If
        CheckCallCenterToolbarStatus()
    End Sub
#End Region

    Private Function MakeAutoRequest(ByVal aCallInfo As CallInfo) As Long
        MakeAutoRequest = -1

        Dim lDT As DateTime = DateTime.Now

        Dim lKh As New KhayamDateTime
        lKh.SetMiladyDate(lDT)

        Dim lIsSaveOK As Boolean = False
        Dim lRequestId As Long = -1

        Dim lDsReq As New DatasetCcRequester

        Try
            Dim lUserId As Integer = WorkingUserId

            Dim lTbl As DatasetCcRequester.TblRequestDataTable = lDsReq.TblRequest
            Dim lRow As DatasetCcRequester.TblRequestRow = lTbl.NewTblRequestRow

            Dim lTblInfo As DatasetCcRequester.TblRequestInfoDataTable = lDsReq.TblRequestInfo
            Dim lRowInfo As DatasetCcRequester.TblRequestInfoRow = lTblInfo.NewTblRequestInfoRow

            '-------------------
            'TblRequest:

            lRow("RequestId") = GetAutoInc()
            lRow("CallId") = aCallInfo.CallId
            'lRow("LPRequestId") = ""
            'lRow("MPRequestId") = ""
            'lRow("SecretaryId") = ""
            lRow("AreaUserId") = lUserId
            'lRow("DataEntrancePersonName") = ""
            lRow("DisconnectDT") = lDT
            lRow("DisconnectDatePersian") = lKh.GetShamsiDateStr
            lRow("DisconnectTime") = lDT.Hour.ToString("0#") & ":" & lDT.Minute.ToString("0#")
            'lRow("SubscriberId") = ""
            'lRow("SubscriberName") = ""
            lRow("CityId") = WorkingCityId
            lRow("Address") = ""
            lRow("Telephone") = aCallInfo.CallerId
            'lRow("PostalCodeId") = ""
            lRow("AreaId") = WorkingAreaId
            'lRow("WeatherId") = ""
            'lRow("MainDisconnectGroupSetId") = ""
            'lRow("DisconnectGroupSetId") = ""
            lRow("Priority") = 1
            lRow("IsSingleSubscriber") = False
            lRow("IsOnePhaseSingleSubscriber") = False
            'lRow("DisconnectInterval") = ""
            'lRow("DisconnectPower") = ""
            lRow("DataEntryDT") = lDT
            lRow("DataEntryDTPersian") = lKh.GetShamsiDateStr
            lRow("DataEntryTime") = lDT.Hour.ToString("0#") & ":" & lDT.Minute.ToString("0#")
            lRow("IsDuplicatedRequest") = False
            lRow("EndJobStateId") = EndJobStates.ejs_AutoSave
            'lRow("ReferToId") = ""
            'lRow("Comments") = ""
            'lRow("IsSentToRelatedArea") = ""
            'lRow("GPSx") = ""
            'lRow("GPSy") = ""
            lRow("IsManoeuvred") = False
            'lRow("TotalManoeuvreCurrentValue") = ""
            'lRow("TotalManoeuvreTime") = ""
            'lRow("TotalManoeuvrePower") = ""
            'lRow("CommentsDisconnect") = ""
            'lRow("RequestNumber") = ""
            lRow("IsNotRelated") = False
            'lRow("AccurateAddress") = ""
            'lRow("DuplicatedRequestId") = ""
            lRow("TamirRequestDatePersian") = ""
            'lRow("TamirRequestDT") = ""
            'lRow("TamirRequestLetterNo") = ""
            'lRow("TamirDisconnectFromDT") = ""
            'lRow("TamirDisconnectFromDatePersian") = ""
            'lRow("TamirDisconnectFromTime") = ""
            'lRow("TamirDisconnectToDT") = ""
            'lRow("TamirDisconnectToDatePersian") = ""
            'lRow("TamirDisconnectToTime") = ""
            'lRow("RequestDEInterval") = ""
            'lRow("CallTypeId") = ""
            'lRow("CallReasonId") = ""
            'lRow("TamirRequestTypeId") = ""
            'lRow("DepartmentId") = ""
            'lRow("DisconnectRequestForId") = ""
            lRow("IsLightRequest") = False
            lRow("IsLPRequest") = False
            lRow("IsMPRequest") = False
            lRow("IsTamir") = False
            'lRow("ConnectDT") = ""
            lRow("ConnectDatePersian") = ""
            'lRow("ConnectTime") = ""
            'lRow("DepartmentName") = ""
            lRow("IsDisconnectMPFeeder") = False
            'lRow("FogheToziDisconnectId") = ""
            lRow("IsFogheToziRequest") = False
            'lRow("LastUpdateAreaUserId") = ""
            lRow("CreateTimeInterval") = 0
            'lRow("ExtraComments") = ""
            'lRow("ZoneId") = ""
            lRow("HasManoeuvre") = False
            'lRow("Temperature") = ""

            lTbl.AddTblRequestRow(lRow)

            '-------------------
            'TblRequestInfo:

            lRowInfo("RequestId") = lRow("RequestId")
            lRowInfo("CallTime") = aCallInfo.CallTime
            'lRowInfo("IsRealNotRelated") = DBNull.Value
            'lRowInfo("OperatorConvsType") = DBNull.Value
            'lRowInfo("IsSaveParts") = DBNull.Value
            'lRowInfo("IsInformTamir") = DBNull.Value
            'lRowInfo("IsSaveLight") = DBNull.Value
            'lRowInfo("SubscriberOKType") = DBNull.Value
            lRowInfo("IsWatched") = False
            'lRowInfo("SendTimeInterval") = DBNull.Value
            'lRowInfo("IsRealDuplicated") = DBNull.Value
            'lRowInfo("EnvironmentTypeId") = DBNull.Value
            lRowInfo("SubscriberCode") = DBNull.Value

            lTblInfo.AddTblRequestInfoRow(lRowInfo)

            '-------------------


            Dim lTrans As SqlTransaction
            Dim lCnn As SqlConnection = New SqlConnection(GetConnection(False))
            Try
                If lCnn.State <> ConnectionState.Open Then lCnn.Open()
                lTrans = lCnn.BeginTransaction

                Dim lUpdate As New frmUpdateDataset
                lIsSaveOK = lUpdate.UpdateDataSet("TblRequest", lDsReq, lRow("AreaId"), 1, , lTrans)

                If lIsSaveOK Then
                    lRowInfo("RequestId") = lRow("RequestId")
                    lIsSaveOK = lUpdate.UpdateDataSet("TblRequestInfo", lDsReq, lRow("AreaId"), 1, False, lTrans)
                End If

                If m_IsTelCMode AndAlso Not IsDBNull(lRow("CallId")) Then
                    SaveTelCInfo(lRow("CallId"), lRow("AreaId"), lUpdate, lTrans)
                End If

                If lIsSaveOK Then
                    lRequestId = lRow("RequestId")
                    lTrans.Commit()
                    MakeAutoRequest = lRequestId
                End If

            Catch ex As Exception
                lTrans.Rollback()
                SaveLog("!! AutoSave !! " & ex.ToString)
            Finally
                lCnn.Close()
            End Try

        Catch ex As Exception
            SaveLog("!! AutoSave !! " & ex.ToString)
        End Try

    End Function
    Private Sub SaveTelCInfo(ByVal aCallId As Integer, ByVal aAreaId As Integer, ByRef aUpdate As frmUpdateDataset, ByRef aTrans As SqlTransaction)
        Dim lIsCallInfo As Boolean = False
        Dim lTrans As SqlTransaction = aTrans
        Dim lDs As New DataSet

        Try

            Try
                Dim lSQL As String = "SELECT * FROM TblTelCInfo WHERE telcCallId = " & aCallId
                BindingTable(lSQL, aTrans.Connection, lDs, "TblTelCInfo", , True, , , lTrans, , True)
                lIsCallInfo = True
            Catch ex As Exception
            End Try

            If lIsCallInfo AndAlso lDs.Tables("TblTelCInfo").Rows.Count > 0 Then
                With lDs.Tables("TblTelCInfo").Rows(0)
                    .BeginEdit()
                    .Item("AreaId") = aAreaId
                    .EndEdit()
                    aUpdate.UpdateDataSet("TblTelCInfo", lDs, aAreaId, , , lTrans, True, True)
                End With
            End If

        Catch ex As Exception
            'Throw ex
        End Try
    End Sub

    Private Sub mnuRep_1_18_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRep_1_18.Click
        Dim dlg As New frmSearchBase(17)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuShowLSLogSettings_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuShowLSLogSettings.Click
        If mIsShowLogSettings AndAlso IsAdmin() Then
            m_HclsCall.ShowLogSetting()
        Else
            mnuShowLSLogSettings.Visible = False
        End If
    End Sub

    Private Sub mnuRpt_3_25_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRpt_3_25.Click
        Dim dlg As New frmSearchBaseMP(24)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuLP_Mode2_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuLP_Mode2.Click
        Dim dlg As New frmSearchBaseLP(False, 18)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub mnuReport_4_22_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MnuReport_4_22.Click
        Dim dlg As New frmSearchBaseLP(False, 20)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuReport_4_23_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MnuReport_4_23.Click
        Dim dlg As New frmSearchBaseLP(False, 21)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub mnuReport_4_24_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MnuReport_4_24.Click
        Dim dlg As New frmSearchBaseLP(False, 22)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub mnuReport_4_25_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MnuReport_4_25.Click
        Dim dlg As New frmMakeReportManoeurve("LP")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuRep_7_22_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRep_7_22.Click
        Dim dlg As New frmAvgDCPower("7-22")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuRep_7_23_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRep_7_23.Click
        Dim dlg As New frmAvgDCPower("7-23")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuRep_7_24_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRep_7_24.Click
        Dim dlg As New frmAvgDCPower("7-24")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuRepExcelGroupSetGroup_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepExcelGroupSetGroup.Click
        Dim dlg As New frmExcelGroupSetGroup
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuRepEkipPerformance_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepEkipPerformance.Click, mnuRepSumEkipPerformance.Click
        Dim lReportNo As String = CType(sender, ToolStripMenuItem).Tag
        Dim lDlg As New frmReportsEkips(lReportNo)
        lDlg.ShowDialog()
        lDlg.Dispose()
        lDlg = Nothing
    End Sub

    Private Sub mnuRepDeleteRequest_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepDeleteRequest.Click
        Dim lDlg As New frmReportDeleteRequest
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuLPPostLoadsStat_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuLPPostLoadsStat.Click
        Dim dlg As New frmReportLPPostLoad(True)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuFactory_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFactory.Click
        Dim dlg As frmBaseTables
        dlg = New frmBaseTables("Tbl_Factory")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuMPHourly_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMPHourly.Click
        Dim dlg As New frmMPFeederMonthlyLoadPeak(, True)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuRpt_3_26_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRpt_3_26.Click
        Dim lDlg As New frmReportPowerIndex("rptECOPowerManoeuvre .rpt")
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub
    Private Sub mnuRep_SMS_1_Click(sender As Object, e As EventArgs) Handles mnuRep_SMS_1.Click
        Dim lDlg As New frmReport_SMS
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub
    Private Sub mnuRep_SMS_2_Click(sender As Object, e As EventArgs) Handles mnuRep_SMS_2.Click
        Dim lDlg As New frmReport_AllSMS
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub
    Private Sub mnuRepCountLPPost_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepCountLPPost.Click
        Dim dlg As New frmReportLPPost("postCount")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnu_Num_3_27_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Num_3_27.Click
        Dim dlg As New frmSearchBaseMP(25)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub mnu_Num_3_34_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Num_3_34.Click
        Dim dlg As New frmSearchBaseMP(32)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub MenuButtonItem27_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItem27.Click
        Dim dlg As New frmSearchBaseMP(26)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuRepDelayControl_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepDelayControl.Click
        Dim lDlg As New frmReportDelayInterval
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub rptRep_ListenUserCallCount2_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rptRep_ListenUserCallCount2.Click
        Dim lDlg As New frmReportCallListen("rptListenUserCallCount2.rpt")
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuRepMP_Mode5_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepMP_Mode5.Click
        Dim dlg As New frmSearchBaseMP(27)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuRepMP_Mode7_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepMP_Mode7.Click
        Dim dlg As New frmSearchBaseMP(34)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub mnuRep_3_37_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRep_3_37.Click
        Dim dlg As New frmSearchBaseMP(35)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuRep_3_38_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRep_3_38.Click
        Dim dlg As New frmSearchBaseMP(36)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuSerghatPart_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSerghatPart.Click
        Dim lDlg As New frmReportSerghatPart
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuUtilizationFactor_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuUtilizationFactor.Click
        Dim lDlg As New frmReportUtilizationFactor
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuLPPostEarths_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuLPPostEarths.Click
        Dim lDlg As New frmReportLPTransEarth("15-1")
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuReport_8_13_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReport_8_13.Click
        Dim lDlg As New frmReportLPTransEarth("15-2")
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub tb_CallDisconnect_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tb_CallDisconnect.Click
        If Not mTelCInfo.Logout() Then
            ShowError("عدم موفقيت در خروج از مرکز تماس" & vbCrLf & mTelCInfo.Message)
            Exit Sub
        Else
            mIsManualCCLogout = True
            m_IsLoginToCallCenter = False
        End If
        CheckCallCenterToolbarStatus()
    End Sub
    Private Sub tb_CallConnect_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tb_CallConnect.Click
        Dim lServerName As String = GetRegistryAbd("TAPIServerName", , True)
        Try
            If Not mTelCInfo.IsLogin Then
                mTelCInfo.DirectLogOff(m_MitelAgentId, m_CRMPsw)
            End If
        Catch ex As Exception
        End Try
        If Not mTelCInfo.Login(lServerName, m_MitelExtension, m_CRMPsw, False, m_MitelAgentId, "", False) Then
            ShowError("خطا به هنگام اتصال به سرور " & IIf(m_IsCRMMode, "مرکز تماس", "TelC") & vbCrLf & mTelCInfo.Message)
        Else
            mIsManualCCLogout = False
            m_IsLoginToCallCenter = True
            ShowInfo("هم اکنون ارتباط شما با مرکز تماس برقرار است.", True)
        End If
        CheckCallCenterToolbarStatus()
    End Sub
    Private Sub CheckCallCenterToolbarStatus()
        tb_CallConnect.Checked = m_IsLoginToCallCenter
        tb_CallDisconnect.Checked = Not m_IsLoginToCallCenter
    End Sub

    Private Sub mnuTip_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuTip.Click

        Dim lTipDlg As New frmTip(True)
        If lTipDlg.IsTipExists Then
            lTipDlg.ShowDialog()
        End If
        lTipDlg.Dispose()
        lTipDlg = Nothing

    End Sub

    Private Sub mnuThriftPower_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuThriftPower.Click
        Dim lDlg As New frmReportThriftPower
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub
    Private Sub mnuReport_10_17_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReport_10_17.Click
        Dim lDlg As New frmReportThriftPower("10-17")
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub
    Private Sub mnuReport_9_16_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReport_9_16.Click
        Dim lDlg As New frmReportRequestMatch
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub
    Private Sub mnuReport_10_19_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReport_10_19.Click, mnuReport_10_26.Click
        Dim lReportNo As String = CType(sender, ToolStripMenuItem).Tag
        Dim lDlg As New frmReportSAIDI(lReportNo)
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuMinCountFeederPart_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMinCountFeederPart.Click
        Dim dlg As New frmSearchBaseMP(28)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuLPPostFeederPartCount_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuLPPostFeederPartCount.Click
        Dim dlg As New frmSearchBaseMP(29)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuRptHistory_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRptHistoryMPFeeder.Click, mnuRptHistoryLPPost.Click
        Dim lTag As String = CType(sender, ToolStripMenuItem).Tag
        Dim lDlg As New frmReportPostFeederHistory(lTag)
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub MenuButtonItem29_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItem29.Click
        Dim dlg As New frmSearchBaseMP(30)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuButtonItem45_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItem45.Click
        Dim dlg As New frmSearchBaseLP(False, 19)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuButtonItem47_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItem47.Click
        Dim dlg As New frmSearchBaseLP(True, 4)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuMPFeederMonthly_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMPFeederMonthly.Click
        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.ver_940410) Then
            ShowHavadesInfoMessage()
            Exit Sub
        End If
        Dim lDlg As New frmMPPostMonthly("MPFeeder")
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuHavadesConfig_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHavadesConfig.Click
        Dim dlg As New frmHavadesConfig
        dlg.ShowDialog()
        dlg.Dispose()
        dlg = Nothing
    End Sub
    Private Sub mnuSecuritySetting_Click(sender As Object, e As EventArgs) Handles mnuSecuritySetting.Click
        Dim dlg As New frmSecuritySettings
        dlg.ShowDialog()
        dlg.Dispose()
        dlg = Nothing
    End Sub

    Private Sub mnuRepDisconnectMonthlyCountMode2_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepDisconnectMonthlyCountMode2.Click
        Dim dlg As New frmMonthlyReportsFilters("rptDisconnectMonthlyMod2")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuMPFeederDisconnetInfo_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMPFeederDisconnetInfo.Click
        Dim lDlg As New frmMonthlyReportsFilters("MPFeederDisconnetInfo")
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub
    Private Sub mnuRep_7_27_Click(sender As Object, e As EventArgs) Handles mnuRep_7_27.Click
        Dim lDlg As New frmSearchBaseMP(33)
        lDlg.ShowDialog()
        lDlg.Dispose()
        lDlg = Nothing
    End Sub
    Private Sub mnuRep_7_28_Click(sender As Object, e As EventArgs) Handles mnuRep_7_28.Click
        Dim dlg As New frmControlReportsFilters("خلاصه گزارش 1_1_5 به تفکيک ناحيه ، سال و ماه", 28, , "(7-28)")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub mnuRepCompareDCCount_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepCompareDCCount.Click
        Dim lDlg As New frmMonthlyReportsFilters("CompareCount")
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuRepCompareDCEnergy_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepCompareDCEnergy.Click
        Dim lDlg As New frmMonthlyReportsFilters("CompareEnergyRatio")
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub munRep_11_2_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles munRep_11_2.Click
        Dim lDlg As New frmMonthlyReportsFilters("CompareEnergy")
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuRep_11_3_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRep_11_3.Click
        Dim lDlg As New frmMonthlyReportsFilters("CompareInterval")
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuRep_11_8_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRep_11_8.Click
        Dim lDlg As New frmMonthlyReportsFilters("CompareAll")
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuRep_11_9_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRep_11_9.Click
        Dim lDlg As New frmReportPowerIndex("rptGoalCompare.rpt")
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub
    Private Sub mnuRep_11_10_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRep_11_10.Click
        Dim lDlg As New frmMonthlyReportsFilters("Report_11_10")
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuRepLPTransAction_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepLPTransAction.Click
        Dim dlg As New frmReportLPPost("LPTransAction")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuConfig_Activate(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuConfig.Click
        Dim lDlg As New frmConfig
        If lDlg.ShowDialog() = DialogResult.OK Then
            Dim lDlgInfo As New frmInformUserAction("اطلاعات با موفقيت ذخيره گرديد")
            lDlgInfo.ShowDialog()
            lDlgInfo.Dispose()
        End If
        lDlg.Dispose()
    End Sub
    Private Sub mnuReportLPTrans_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReportLPTrans.Click
        Dim dlg As New frmReportLPTrans
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuFrmNetworkMonthlyInfo_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFrmNetworkMonthlyInfo.Click
        Try
            Dim lDlg As New frmNetworkMonthlyInfo
            lDlg.ShowDialog()
            lDlg.Dispose()
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub
    Private Sub mnuFrmGoal_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFrmGoal.Click
        Try
            Dim lDlg As New frmBS_Goal
            lDlg.ShowDialog()
            lDlg.Dispose()
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub mnuReport_10_15_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReport_10_15.Click
        Try
            Dim lDlg As New frmReportLPPostInfo
            lDlg.ShowDialog()
            lDlg.Dispose()
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub mnuRep_8_14_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRep_8_14.Click
        Try
            Dim lDlg As New frmMPFeederPeakDayNight("Report_8_14")
            lDlg.ShowDialog()
            lDlg.Dispose()
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub mnuReport_8_15_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReport_8_15.Click
        Try
            Dim lDlg As New frmMPFeederPeakDayNight("Report_8_15")
            lDlg.ShowDialog()
            lDlg.Dispose()
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub mnuReport_8_16_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReport_8_16.Click
        Try
            Dim lDlg As New frmMPFeederPeakDayNight("Report_8_16")
            lDlg.ShowDialog()
            lDlg.Dispose()
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub mnuSmsContactsSemat_Activate(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuSmsContactsSemat.Click
        Dim dlg As frmBaseTables
        dlg = New frmBaseTables("Tbl_Semat")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuSmsContactsGroups_Activate(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuSmsContactsGroups.Click
        Dim dlg As frmBaseTableSMSContacts
        dlg = New frmBaseTableSMSContacts("Tbl_SMSGroup")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuSmsContacts_Activate(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuSmsContacts.Click
        Dim dlg As frmBaseTableSMSContacts
        dlg = New frmBaseTableSMSContacts("Tbl_SMSContact")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuSendSmsPanel_Activate(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuSendSmsPanel.Click
        Dim lCanReadSMS As Boolean = CheckUserTrustee("CanViewSMSPanel", 33) Or IsAdmin()
        If lCanReadSMS = False Then
            MsgBoxF("شما به اين بخش دسترسي نداريد", "خطا", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Error, MessageBoxDefaultButton.Button1)
        Else
            Dim dlg As frmSMSPanel
            dlg = New frmSMSPanel
            dlg.ShowDialog()
            dlg.Dispose()
        End If
    End Sub

    Private Sub MenuButtonItem63_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItem63.Click
        Dim dlg As New frmSearchBaseMP(31)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuDailyNetworkState_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDailyNetworkState.Click
        Dim lRepType As String = ""
        Try
            lRepType = CType(sender, ToolStripMenuItem).Tag
            lRepType = lRepType.Replace("mnuRepExcel", "")

            Dim dlg As New frmReportsExcel(lRepType)
            dlg.ShowDialog()
            dlg.Dispose()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub MenuButtonItem64_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItem64.Click
        Try
            Dim lDlg As New frmReportLPPostFactoryInfo
            lDlg.ShowDialog()
            lDlg.Dispose()
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub


    Private Sub mnuRptEzamEkipInfo_Mode2_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRptEzamEkipInfo_Mode2.Click
        Dim dlg As New frmSearchBase(18)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub MenuButtonItem66_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuButtonItem66.Click
        Dim dlg As New frmReportLPPostLoad(, "(8-17)")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuPeopleVoice_Activate(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuPeopleVoice.Click
        Dim dlg = New frmPeopleVoice
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuReportDisconnectFeeder_Mode2_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReportDisconnectFeeder_Mode2.Click
        Dim lDlg As New frmReportAllDisconnectFeeder(True)
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub
    Private Sub mnuRep_10_21_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRep_10_21.Click
        Dim lDlg As New frmReportCutOut
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub
    Private Sub mnuQuestion_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuQuestion.Click
        Dim lDlg As New frm_Question
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuSurvey_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSurvey.Click
        If Not IsSetadMode Or Not IsAdmin() Then
            ShowError("تنها کاربران مسئول نرم افزار در نسخه ستاد به اين بخش دسترسي دارند")
            Exit Sub
        End If
        Dim lDlg As New frm_Question(QuestionType.Survey)
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuSurveyIVR_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSurveyIVR.Click
        If Not IsSetadMode Or Not IsAdmin() Then
            ShowError("تنها کاربران مسئول نرم افزار در نسخه ستاد به اين بخش دسترسي دارند")
            Exit Sub
        End If
        Dim lDlg As New frm_Question(QuestionType.SurveyIVR)
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuGuidSMS_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuGuidSMS.Click
        If Not IsSetadMode Or Not IsAdmin() Then
            ShowError("تنها کاربران مسئول نرم افزار در نسخه ستاد به اين بخش دسترسي دارند")
            Exit Sub
        End If
        Dim lDlg As New frmGuidSMSSetting()
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuSMSErjaDone_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSMSErjaDone.Click
        If Not IsSetadMode Or Not IsAdmin() Then
            ShowError("تنها کاربران مسئول نرم افزار در نسخه ستاد به اين بخش دسترسي دارند")
            Exit Sub
        End If
        Dim lDlg As New frmTrackingCodeSMSSettings("SMSErja")
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub
    Private Sub mnuSMSAfterEzamEkip_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSMSAfterEzamEkip.Click
        If Not IsSetadMode Or Not IsAdmin() Then
            ShowError("تنها کاربران مسئول نرم افزار در نسخه ستاد به اين بخش دسترسي دارند")
            Exit Sub
        End If
        Dim lDlg As New frmGuidSMSSetting("SMSAfterEzam")
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuReportLPPostEarth_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReportLPPostEarth.Click
        Dim lDlg As New frmReportLPPostEarth
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuRep_10_23_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRep_10_23.Click
        Dim lDlg As New frmReportHourByHourpower("ExcelReport")
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub


    Private Sub tbQuit_Click(sender As Object, e As EventArgs) Handles tbQuit.Click
        MenuFileExit_Click(sender, e)
    End Sub
    Private Sub tbChangeUser_Click(sender As Object, e As EventArgs) Handles tbChangeUser.Click
        MenuToolsRelogin_Click(sender, e)
    End Sub
    Private Sub tbPhonebook_Click(sender As Object, e As EventArgs) Handles tbPhonebook.Click
        MenuItem17_Click(sender, e)
    End Sub
    Private Sub tbSubscribers_Click(sender As Object, e As EventArgs) Handles tbSubscribers.Click
        MenuItem11_Click(sender, e)
    End Sub
    Private Sub tbMessages_Click(sender As Object, e As EventArgs) Handles tbMessages.Click
        MenuFileMessages_Click(sender, e)
    End Sub
    Private Sub mnuSendMobileAppNotification_Click(sender As Object, e As EventArgs) Handles mnuSendMobileAppNotification.Click
        Try
            Dim lNotifyServiceAddress As String = CConfig.ReadConfig("TZHavadesTavanirURL", "")
            If String.IsNullOrWhiteSpace(lNotifyServiceAddress) Then
                ShowError("متأسفانه اين قسمت از نرم افزار براي شما فعال نمي باشد. لطفاً با پشتيباني شرکت تذرو افزار تماس بگيريد")
                Exit Sub
            End If
            Dim lDlg As New frmSendSameMessagePreview()
            lDlg.ShowDialog()
            lDlg.Dispose()
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub SetFavMenuCheck(aMenu As ToolStripMenuItem, aMenuName As String)
        Try

            For Each lCtrl As Object In aMenu.DropDownItems
                If Not lCtrl.GetType().Name = "ToolStripSeparator" Then

                    Dim lItem As ToolStripMenuItem = CType(lCtrl, ToolStripMenuItem)
                    If lItem.DropDownItems.Count > 0 Then
                        SetFavMenuCheck(lItem, aMenuName)
                    Else
                        If lItem.Name = aMenuName Then
                            lItem.Checked = True
                        End If
                    End If
                End If
            Next

        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub SetFavMenuCheck(aMenu As ToolStripMenuItem)
        Try

            For Each lCtrl As Object In aMenu.DropDownItems
                If Not lCtrl.GetType().Name = "ToolStripSeparator" Then
                    Dim lItem As ToolStripMenuItem = CType(lCtrl, ToolStripMenuItem)
                    If lItem.Name = "mnuSaveFav" Or lItem.Name = "mnuSimaPortal" Then Exit Sub

                    If lItem.DropDownItems.Count > 0 Then
                        SetFavMenuCheck(lItem)
                    Else
                        Dim lFavItem As FavToolStripMenuItem = CType(lCtrl, FavToolStripMenuItem)
                        If lFavItem.IsChange Then
                            mIsChangeMenuFav = True
                            Dim lRows() As DataRow
                            Dim lNewRow As DataRow
                            lRows = mDs.Tables("TblReportFavorite").Select("MenuName = '" & lFavItem.Name & "'")
                            If lRows.Length > 0 Then
                                lNewRow = lRows(0)
                                If lFavItem.Checked Then
                                    lNewRow("MenuName") = lFavItem.Name
                                Else
                                    lNewRow.Delete()
                                End If
                            Else
                                If lFavItem.Checked Then
                                    lNewRow = mDs.Tables("TblReportFavorite").NewRow()
                                    lNewRow("ReportFavoriteId") = GetAutoInc()
                                    lNewRow("ApplicationId") = mApplicationType.Havades
                                    lNewRow("AreaUserId") = WorkingUserId
                                    lNewRow("MenuName") = lFavItem.Name
                                    mDs.Tables("TblReportFavorite").Rows.Add(lNewRow)
                                End If
                            End If
                            lFavItem.IsChange = False
                        End If
                    End If
                End If
            Next

        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub SetFavMenuUnCheck(aMenu As ToolStripMenuItem)
        Try

            For Each lCtrl As Object In aMenu.DropDownItems
                If Not lCtrl.GetType().Name = "ToolStripSeparator" Then
                    Dim lItem As ToolStripMenuItem = CType(lCtrl, ToolStripMenuItem)
                    If lItem.Name = "mnuSaveFav" Then Exit Sub

                    If lItem.DropDownItems.Count > 0 Then
                        SetFavMenuUnCheck(lItem)
                    Else
                        Dim lFavItem As FavToolStripMenuItem = CType(lCtrl, FavToolStripMenuItem)
                        lFavItem.Checked = False
                    End If
                End If
            Next

        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Function FavMenuVisible(aMenu As ToolStripMenuItem) As Boolean
        FavMenuVisible = False
        Try

            For Each lCtrl As Object In aMenu.DropDownItems
                If lCtrl.GetType().Name = "ToolStripSeparator" Then
                    If CType(lCtrl, ToolStripSeparator).Name = "mnuSaveSeparator" Then
                        Exit Function
                    End If
                    CType(lCtrl, ToolStripSeparator).Visible = False
                Else
                    Dim lItem As ToolStripMenuItem = CType(lCtrl, ToolStripMenuItem)
                    If lItem.Name = "mnuSaveFav" Then Exit Function
                    If lItem.DropDownItems.Count > 0 Then
                        SetFavVisible(lItem, True)
                        Dim lFavMenuVisible As Boolean = FavMenuVisible(lItem)
                        If Not lFavMenuVisible Then
                            SetFavVisible(lItem, False)
                        Else
                            SetFavVisible(lItem, True)
                            FavMenuVisible = True
                        End If
                    Else
                        If lItem.Checked Then
                            SetFavVisible(lItem, True)
                            FavMenuVisible = True
                        Else
                            SetFavVisible(lItem, False)
                        End If
                    End If
                End If
            Next

        Catch ex As Exception
            ShowError(ex)
        End Try

        Return FavMenuVisible
    End Function

    Private Sub SetFavVisible(aMenuItem As Object, aVisible As Boolean)
        If TypeOf (aMenuItem) Is FavToolStripMenuItem Then
            CType(aMenuItem, FavToolStripMenuItem).FavVisible = aVisible
        Else
            aMenuItem.Visible = aVisible
        End If
    End Sub

    Private Sub VisibleAllMenu(aMenu As ToolStripMenuItem)
        For Each lCtrl As Object In aMenu.DropDownItems
            If lCtrl.GetType().Name = "ToolStripSeparator" Then
                CType(lCtrl, ToolStripSeparator).Visible = True
            Else
                Dim lItem As ToolStripMenuItem = CType(lCtrl, ToolStripMenuItem)
                If lItem.DropDownItems.Count > 0 Then
                    VisibleAllMenu(lItem)
                End If
                SetFavVisible(lCtrl, True)
            End If
        Next
    End Sub

    Private Sub mnuSaveFav_Click(sender As Object, e As EventArgs) Handles mnuSaveFav.Click
        Try
            mIsChangeMenuFav = False
            SetFavMenuCheck(mnuReports)
            If Not mIsChangeMenuFav Then Exit Sub
            Dim lUpdate As New frmUpdateDataset
            lUpdate.UpdateDataSet("TblReportFavorite", mDs, WorkingAreaId)
            Dim lUpdateInfo As New frmInformUserAction("علاقه مندي ها تغيير يافت")
            lUpdateInfo.ShowDialog()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub mnuReportAVL_Click(sender As Object, e As EventArgs) Handles mnuReportAVL.Click, mnuReport_9_20.Click
        Dim lTag = CType(sender, ToolStripMenuItem).Tag
        Dim lDlg As New frmReportAVL(lTag)
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuMPFeederPartLoadHour_Click(sender As Object, e As EventArgs) Handles mnuMPFeederPartLoadHour.Click
        Dim lDlg As New frmFeederPartLoadPreview()
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuRep_10_27_Click(sender As Object, e As EventArgs) Handles mnuRep_10_27.Click
        Dim lDlg As New FrmReportHourByHourBlackOut
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub


    Private Sub mnuRep_10_28_Click(sender As Object, e As EventArgs) Handles mnuRep_10_28.Click
        Dim lDlg As New frmReportMPPerformanceDaily
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuChangeSMSBody_Click(sender As Object, e As EventArgs) Handles mnuChangeSMSBody.Click
        Dim lDlg As New frmSMSText
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuAllowValidator_Click(sender As Object, e As EventArgs) Handles mnuAllowValidator.Click
        Dim dlg As New frmBaseTablesNotStd("Tbl_AllowValidator")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub mnuRep_10_29_Click(sender As Object, e As EventArgs) Handles mnuRep_10_29.Click
        Dim dlg As New frmReportReqTamirType
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub mnuRep_10_30_Click(sender As Object, e As EventArgs) Handles mnuRep_10_30.Click
        Dim dlg As New frmReportsDCPowerByDate
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub mnuRep_10_31_Click(sender As Object, e As EventArgs) Handles mnuReport_10_31.Click
        Dim lDlg As New frmReport_TamirByDates("rptECOPowerInfo.rpt")
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub
    Private Sub mnuReport_8_19_Click(sender As Object, e As EventArgs) Handles mnuReport_8_19.Click
        Dim lDlg As New Bargh_Reports.frmReportCalcPowerRoostaFamily
        lDlg.ShowDialog()
        lDlg.Dispose()
        lDlg = Nothing
    End Sub

    Private Sub mnuSubscriberSmsSetting_Click(sender As Object, e As EventArgs) Handles mnuSubscriberSmsSetting.Click
        Dim lDlg As New frmSubscriberSMSPreview
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuReport_8_20_Click(sender As Object, e As EventArgs) Handles mnuReport_8_20.Click
        Dim lDlg As New frmReportMPFeederLoad
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub
    Private Sub MenuShowInfo_Click(sender As Object, e As EventArgs) Handles MenuShowInfo.Click
        Dim lDlg As New frmShowInfo
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuRepMPPostTransLoadHourByHour_Click(sender As Object, e As EventArgs) Handles mnuRepMPPostTransLoadHourByHour.Click
        Dim lDlg As New frmReportMPPostHourByHourPeak
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuGlobalNetworkPeakHour_Click(sender As Object, e As EventArgs) Handles mnuGlobalNetworkPeakHour.Click
        Dim lDlg As New frmGlobalPeakTimesPreview
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuRptDisFogh_Click(sender As Object, e As EventArgs) Handles mnuRptDisFogh.Click, mnuRpt_2_3.Click
        Dim lTag = CType(sender, ToolStripMenuItem).Tag
        Dim dlg As New frmSearchFogheTozi(lTag)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub mnuRptManourveFogh_Click(sender As Object, e As EventArgs) Handles mnuRptManourveFogh.Click
        Dim dlg As New frmMakeReportManoeurve("Fogh")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuAreaPeak_Click(sender As Object, e As EventArgs) Handles mnuAreaPeak.Click
        Try
            Dim lDlg As New frmAreaPeak
            lDlg.ShowDialog()
            lDlg.Dispose()
            lDlg = Nothing
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub mnuStoreOperatin_Click(sender As Object, e As EventArgs) Handles mnuStoreOperatin.Click
        Try
            Dim lDlg As New frmStoreOperation
            lDlg.ShowDialog()
            lDlg.Dispose()
            lDlg = Nothing
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub mnuGenericParts_Click(sender As Object, e As EventArgs) Handles mnuGenericParts.Click
        Try
            Dim lDlg As New frmGenericParts
            lDlg.ShowDialog()
            lDlg.Dispose()
            lDlg = Nothing
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub mnuIVRCodes_Click(sender As Object, e As EventArgs) Handles mnuIVRCodes.Click
        Dim lDlg As New frmBaseTablesNotStd("Tbl_IVRCode")
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnulSpecialCallType_Click(sender As Object, e As EventArgs) Handles mnulSpecialCallType.Click
        Dim dlg As frmBaseTables
        dlg = New frmBaseTables("Tbl_SpecialCallType")
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuRepExistance_Click(sender As Object, e As EventArgs) Handles mnuRepExistance.Click
        Dim lDlg As New frmReportStore("Existance")
        lDlg.ShowDialog()
        lDlg.Dispose()
        lDlg = Nothing
    End Sub
    Private Sub mnuRepCartex_Click(sender As Object, e As EventArgs) Handles mnuRepCartex.Click
        Dim lDlg As New frmReportStore("Cartex")
        lDlg.ShowDialog()
        lDlg.Dispose()
        lDlg = Nothing
    End Sub

    Private Sub mnuTrackingCodeSettings_Click(sender As Object, e As EventArgs) Handles mnuTrackingCodeSettings.Click
        If Not IsSetadMode Or Not IsAdmin() Then
            ShowError("تنها کاربران مسئول نرم افزار در نسخه ستاد به اين بخش دسترسي دارند")
            Exit Sub
        End If
        Dim lDlg As New frmTrackingCodeSMSSettings()
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub
    Private Sub mnuSurvyReport_Click(sender As Object, e As EventArgs) Handles mnuSurvyReport.Click
        Dim lDlg As New frmQuestionReport()
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuMap121andIGMC_Click(sender As Object, e As EventArgs) Handles mnuMap121andIGMC.Click
        Dim lDlg As New frmMapMeterBaseInfo()
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuReport_13(sender As Object, e As EventArgs) Handles mnuReport_13_1.Click, mnuReport_13_2.Click, mnuReport_13_3.Click
        Dim lTag As String = CType(sender, ToolStripMenuItem).Tag
        Dim lDlg As New frmReportExtraction(lTag)
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub
    Private Sub mnuReport_13_4_Click(sender As Object, e As EventArgs) Handles mnuReport_13_4.Click
        Dim lDlg As New frmReportExtractionLoad()
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuTransLogs_Click(sender As Object, e As EventArgs) Handles mnuTransLogs.Click
        Try
            Dim lIsAccess As Boolean = IsAdmin() Or CheckUserTrustee("CanManageLPTrans", 9, ApplicationTypes.Havades)
            If Not lIsAccess Then
                ShowNoAccessMessageByTag("CanManageLPTrans")
                Exit Sub
            End If

            Dim lDlg As New frmLPTransMonitoring()
            lDlg.ShowDialog()
            lDlg.Dispose()
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub mnuFreeTrans_Click(sender As Object, e As EventArgs) Handles mnuFreeTrans.Click
        Try
            Dim lIsAccess As Boolean = IsAdmin() Or CheckUserTrustee("CanManageLPTrans", 9, ApplicationTypes.Havades)
            If Not lIsAccess Then
                ShowNoAccessMessageByTag("CanManageLPTrans")
                Exit Sub
            End If

            Dim lDlg As New frmLPTransPreview
            lDlg.ShowDialog()
            lDlg.Dispose()
            lDlg = Nothing
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub CheckUserTrusteeForReports()
        Try
            If IsAdmin() Or IsDebugMode() Then Exit Sub

            For Each lCtrl As Object In mnuReports.DropDownItems
                If lCtrl.GetType().Name = "ToolStripMenuItem" Then
                    Dim lMnu As ToolStripMenuItem = CType(lCtrl, ToolStripMenuItem)

                    If CheckUserTrustee(lMnu.Name, 35, ApplicationTypes.Havades) Then
                        lMnu.ToolTipText = "شما به اين دسته از گزارشات دسترسي نداريد"
                        For Each lCtrl2 As Object In lMnu.DropDownItems
                            If Regex.IsMatch(lCtrl2.GetType().Name, "ToolStripMenuItem$") Then
                                CType(lCtrl2, ToolStripMenuItem).Enabled = False
                            End If
                        Next
                    End If
                End If
            Next
        Catch ex As Exception
            ShowError(ex.ToString())
        End Try

    End Sub

    Private Sub mnuSMSSettings_Click(sender As Object, e As EventArgs) Handles mnuSMSSettings.Click
        If Not IsSetadMode Or Not IsAdmin() Then
            ShowError("تنها کاربران مسئول نرم افزار در نسخه ستاد به اين بخش دسترسي دارند")
            Exit Sub
        End If
        Dim lDlg As New frmSMSSettings
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuRequestMultiDC_Click(sender As Object, e As EventArgs) Handles mnuRequestMultiDC.Click
        Dim lIsAccess As Boolean = False
        lIsAccess = IsAdmin() Or CheckUserTrustee("CanSaveRequestMultiDC", 20, ApplicationTypes.Havades)
        If Not lIsAccess Then
            ShowNoAccessMessageByTag("CanSaveRequestMultiDC")
            Exit Sub
        End If
        Dim lDlg As New frmRequestMultiDC
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuHomaTabletUser_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHomaTabletUser.Click
        Dim lIsAccess As Boolean = False
        lIsAccess = IsAdmin() Or CheckUserTrustee("HomaCanRelationMasterToTablet", 36, ApplicationTypes.Havades)
        If Not lIsAccess Then
            ShowNoAccessMessageByTag("HomaCanRelationMasterToTablet")
            Exit Sub
        End If
        Dim lDlg As New frmHomaTabletUserPreview
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuHomaRegisterBilling_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHomaRegisterBilling.Click
        Dim lIsAccess As Boolean = False
        lIsAccess = IsAdmin() Or CheckUserTrustee("HomaCanViewSubscribers", 36, ApplicationTypes.Havades)
        If Not lIsAccess Then
            ShowNoAccessMessageByTag("HomaCanViewSubscribers")
            Exit Sub
        End If
        Dim lDlg As New frmHomaRegisterBilling
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuHomaTablet_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHomaTablet.Click
        Dim lIsAccess As Boolean = False
        lIsAccess = IsAdmin() Or CheckUserTrustee("HomaCanViewTablet", 36, ApplicationTypes.Havades)
        If Not lIsAccess Then
            ShowNoAccessMessageByTag("HomaCanViewTablet")
            Exit Sub
        End If
        Dim lDlg As New frmHomaTablet
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuHomaMonitoring_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Dim lIsAccess As Boolean = False
            lIsAccess = IsAdmin() Or CheckUserTrustee("HomaCanViewMonitoring", 36, ApplicationTypes.Havades)
            If Not lIsAccess Then
                ShowNoAccessMessageByTag("HomaCanViewMonitoring")
                Exit Sub
            End If

            Dim dlg As New frmNewRequestsPreview(False, , , m_HclsCall, aIsHoma:=True)
            dlgMonitoring = dlg
            dlg.ShowDialog()
            dlg.Dispose()
        Catch ex As Exception
            Try
                ShowError(ex)
            Catch ex1 As Exception
                Stop
            End Try
        End Try
        dlgMonitoring = Nothing
    End Sub

    Private Sub mnuSimaPortal_Click(sender As Object, e As EventArgs) Handles mnuSimaPortal.Click
        Try
            Dim lURL As String = CConfig.ReadConfig("SimaURL", "")
            If Not String.IsNullOrWhiteSpace(lURL) Then
                OpenURL(lURL)
            End If
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub
    Private Sub mnuReport_4_26_Activate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MnuReport_4_26.Click
        Dim dlg As New frmSearchBaseLP(False, 23)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuReport_14_1_Click(sender As Object, e As EventArgs) Handles mnuReport_14_1.Click
        Dim lDlg As New frmReportGreen("14-1")
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuReport_14_2_Click(sender As Object, e As EventArgs) Handles mnuReport_14_2.Click
        Dim lDlg As New frmReportGreen("14-2")
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub
    Private Sub mnuReport_14_4_Click(sender As Object, e As EventArgs) Handles mnuReport_14_4.Click
        Dim lDlg As New frmReportGreen("14-4")
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuReport_14_3_Click(sender As Object, e As EventArgs) Handles mnuReport_14_3.Click
        Dim lDlg As New frmReportSima
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub mnuRep_h_1_Click(sender As Object, e As EventArgs) Handles mnuRep_h_1.Click
        Dim lDlg As New frmHomaReportEkip()
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub

    Private Sub MenuButtonItem98_Click(sender As Object, e As EventArgs) Handles MenuButtonItem98.Click
        Dim dlg As New frmRecloserFunction(-1)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub mnuRecloserReport_Click(sender As Object, e As EventArgs) Handles mnuRecloserReport.Click
        Dim dlg As New frmReportRecloser()
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
End Class
