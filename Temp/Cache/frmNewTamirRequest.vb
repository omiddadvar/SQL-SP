Imports System.Data.SqlClient
Imports Bargh_Common.mdlHashemi
Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Collections.Generic
Imports System.Linq
Imports Janus.Windows.GridEX

Public Class frmNewTamirRequest
    Inherits FormBase

    Private Enum FileWorkStatus
        Wait = 0
        Working = 1
        Success = 2
        Failed = 3
        Skip = 4
    End Enum
    Private Class FileUploadInfo
        Public UploadRow As DatasetTamir.ViewTamirRequestFileRow
        Public Status As FileWorkStatus
        Public Message As String
        'Public Direction As FileTransfer.TransferDirection
    End Class
    Private Class DeletFileInfo
        Public TamirRequestFileId As Long
        Public FileServerId As Long
        Public FileId As Long
        Public FileName As String
        Public Status As FileWorkStatus
        Public Message As String
    End Class

#Region "Data Members"
    Private mCnn As SqlConnection = New SqlConnection(GetConnection())
    Private mDs As DatasetTamir
    Private mDsBT As DatasetBT
    Private mDsReq As New DatasetCcRequester
    Private mDsView As DatasetTamirView

    Private mEditingRow As DataRow = Nothing
    Private mEditingRowConfirm As DataRow = Nothing
    Private mEditingRowAllow As DataRow = Nothing
    Private mEditingRowRequestInform As DataRow = Nothing
    Private mEditingRowSubscriberInform As DataRow = Nothing
    Private mEditingWeakPowerRow As DatasetTamir.TblWeakPowerRow = Nothing
    Private mRowD As DatasetDummy.ViewDummyIdRow
    Private mEditingRowManovr As DatasetTamir.TblTamirRequestDisconnectRow

    Private mTamirRequestId As Long = -1
    Private mRequestId As Long = -1

    Private mIsLoading As Boolean = False
    Private mIsLoadInfo As Boolean = False
    Private mIsLoadingPower As Boolean = False
    Private mIsTabChanging As Boolean = False

    Private mIsNewRequest As Boolean
    Private mIsAddRow As Boolean
    Private mIsNewConfirm As Boolean
    Private mIsAddRowConfirm As Boolean
    Private mIsNewAllow As Boolean
    Private mIsAddRowAllow As Boolean
    Private mIsUpdateCurrentValue As Boolean = True
    Private mIsNewManovr As Boolean
    Private mIsAddRowManovr As Boolean

    Private mIsForConfirm As Boolean
    Private mIsForAllow As Boolean
    Private mIsServer121 As Boolean
    Private mIsSetadConfirmPlanned As Boolean = False

    Private mOperationCount As Integer = 0

    '---- Power Info ----
    Private mCosinPhi As Double = 0.85
    Private mVoltage As Integer = 20000
    Private mCurrentValue As Double = 0.0
    Private mPower As Double = 0.0
    '--------------------

    '-- Max Power Info --
    Private mFirstDay_DT As DateTime
    Private mFirstDay_PDate As String = ""
    Private mMaxPower As Double = 0
    Private mIsMonthly As Boolean = False
    '--------------------

    Private mLastIsSendToSetadMode As Boolean = False

    Private mSumDCInterval As Integer = 0
    Private mSumPower As Double = 0.0
    Private mDCCount As Integer = 0
    Private mRelayCount As Integer = 0
    Private mMultiStepPower As Double = 0.0

    Private mPeymankarText As String = ""
    Private mIsKeyPressed As Boolean = False

    Private mEmergency_EndJobStateId As EndJobStates = mdlHashemi.EndJobStates.ejs_None
    Private mEmergency_MPRequestId As Object = DBNull.Value

    Private mIsMultiStep As Boolean = False
    Private mLastMSDateTime As DateTime

    Private mDCPowerECO As Double = 0.0
    Private mRealDCTime As Integer = 0
    Private mOverlapTime As Integer = 0

    Private mIsManualChangeCL As Boolean = False
    Private mIsTamirDirectToSetad As Boolean

    Private mTamirNetworkTypeId As TamirNetworkType = TamirNetworkType.None
    Private mWeakPowerName As String = "هفته"

    Private mIsFogheToziUser As Boolean = False
    Private mIsFogheToziUserRequested As Boolean = False

    Private mIsForceTamirOperationTime As Boolean
    Private mIsBtnSaveLoadEnabled As Boolean = True

    Private mIsEmergencyEnabled As Boolean = False

    Private mIsAutoChangeTamirType As Boolean = False
    Private mNotTamirRequestNetworkType As String = ""

    Private mIsForceManovrDC As Boolean = False
    Private mMainRequestNumber As String = ""
    Private mManovrRequestNumber As String = ""

    Private mIsForWarmLineConfirm As Boolean
    Private mIsWarmLine As Boolean = False
    Private mIsForceCriticalsAddress As Boolean = CConfig.ReadConfig("IsForceCriticalsAddress", False)

    Private mControlEnableState As New SortedList
    Private mTblTemp_FeederKey As DataTable
    Private mTblBase_FeederKey As DataTable
    Private DatasetTemp1 As New DatasetTamir
    Private DatasetCcRequester1 As New DatasetTamir
    Private mIsFinal As Boolean = False
    Private mMultiStepFeederKey As New SortedList
    Private mTamirRequestMultiStepId As Long = -1

    Private mSaveEnabled As Boolean = False

    Private mIsSendGISSubscriberDCInfo As Boolean = False
    Private mIsThereFeederKey As Boolean = False
    Private mIsShowGISKeyInform As Boolean = False

    Private mIsForceEmergencyReason As Boolean

    Private mIsConfirmEndWarmLine As Boolean = False
    Private mIsSendMessageForConfirm As Boolean = CConfig.ReadConfig("IsSendMessageForConfirm", True)
    Private mIsSendMessageForNotConfirm As Boolean = CConfig.ReadConfig("IsSendMessageForNotConfirm", True)
    Private mIsSendMessageForReturn As Boolean = CConfig.ReadConfig("IsSendMessageForReturn", True)

    '--- File Server ---'
    Private mFileServer As CFileServer = Nothing

    Private mFileUploadList As New List(Of FileUploadInfo)
    Private mCurrentFileUpload As FileUploadInfo = Nothing
    Private mIsUploadFilesFinished As Boolean = True

    Private mFileDeleteList As New List(Of DeletFileInfo)
    Private mCurrentFileDelete As DeletFileInfo = Nothing
    Private mIsDeleteFilesFinished As Boolean = True

    Private mIsInFileOperation As Boolean = False
    Private mIsCloseFormAfterFileOperation As Boolean = False
    Friend WithEvents chkIsSendToInformApp As System.Windows.Forms.CheckBox
    Private mIsUploadAfterFileDelete As Boolean = False
    Friend WithEvents btnSelectLPPost As System.Windows.Forms.Button
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents txtMPFeederKeyGIS As System.Windows.Forms.TextBox
    Friend WithEvents Label118 As System.Windows.Forms.Label
    Friend WithEvents lblPeymankar As System.Windows.Forms.LinkLabel
    Friend WithEvents pnlPeymankar As Bargh_Common.PanelGroupBox
    Friend WithEvents btnPeymankar As System.Windows.Forms.Button
    Friend WithEvents txtPeymankarTel As Bargh_Common.TextBoxPersian
    Friend WithEvents Label34 As System.Windows.Forms.Label
    Private mIsSendToInformApp As Boolean = CConfig.ReadConfig("IsSendToInformApp", False)

    '-------------------'
    Private mIsSelectKey As Boolean = False
    Friend WithEvents btnPeymankarTel As System.Windows.Forms.Button
    Friend WithEvents rbIsReturnedWL As System.Windows.Forms.RadioButton
    Friend WithEvents Label120 As System.Windows.Forms.Label
    Friend WithEvents lblPeymankarManager As System.Windows.Forms.Label
    Friend WithEvents picNazerSearch As PictureBox
    Friend WithEvents txtNazerSearch As TextBoxPersian
    Friend WithEvents btnLPFeeder As Button
    Friend WithEvents txtFeederPart As TextBoxPersian
    Friend WithEvents picFeederPart As PictureBox
    Friend WithEvents txtReturn As TextBox
    Friend WithEvents LabelRetrun As Label
    Private mCntMPFeederKey As Integer
#End Region

#Region " Windows Form Designer generated code "

    Public Sub New(Optional ByVal aTamirRequestId As Long = -1, Optional ByVal aIsForConfirm As Boolean = False, Optional ByVal aIsForAllow As Boolean = False, Optional ByVal aIsForWarmLineConfirm As Boolean = False)
        MyBase.New()

        mIsLoading = True

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        mDs = DatasetTamir1
        mDsBT = DatasetBT1
        mDsView = DatasetTamirView1

        mDsBT.Relations.Clear()

        mTamirRequestId = aTamirRequestId

        mIsForConfirm = aIsForConfirm
        mIsForAllow = aIsForAllow

        mIsForWarmLineConfirm = aIsForWarmLineConfirm

        mIsLoading = False

        mIsFogheToziUser = CheckUserTrustee("IsFoghetoziUser", 30, ApplicationTypes.TamirRequest)

        dgFiles.AddContextMenuItem("حذف فايل", "DeleteFile")
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
    Friend WithEvents ftDocs As FileTransfer
    Friend WithEvents txtLetterDate As Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor
    Friend WithEvents txtLetterNo As Bargh_Common.TextBoxPersian
    Friend WithEvents pnlEmergencyReason As System.Windows.Forms.Panel
    Friend WithEvents txtEmergency As System.Windows.Forms.TextBox
    Friend WithEvents lblEmergencyReason As System.Windows.Forms.Label
    Friend WithEvents lblMPPostTrans As System.Windows.Forms.Label
    Friend WithEvents errpro As System.Windows.Forms.ErrorProvider
    Friend WithEvents chkMPPostTrans As Bargh_Common.ChkCombo
    Friend WithEvents tp6_Docs As TabPage
    Friend WithEvents grpNewFile As GroupBox
    Friend WithEvents btnAddFile As Button
    Friend WithEvents btnBrowseForFileUpload As Button
    Friend WithEvents txtSubject As TextBox
    Friend WithEvents txtUploadFilePath As TextBox
    Friend WithEvents Label117 As Label
    Friend WithEvents Label116 As Label
    Friend WithEvents dgFiles As JGrid
    Friend WithEvents CommonDataSet1 As CommonDataSet
    Friend WithEvents txtTimeDisconnect As Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor
    Friend WithEvents txtDateDisconnect As Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor
    Friend WithEvents txtTimeConnect As Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor
    Friend WithEvents txtDateConnect As Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor
    Friend WithEvents txtTamirRequestNumber As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents pnlNumbers As System.Windows.Forms.Panel
    Friend WithEvents cmbArea As ComboBoxPersian
    Friend WithEvents cmbCity As System.Windows.Forms.ComboBox
    Friend WithEvents txtMaxPower As System.Windows.Forms.TextBox
    Friend WithEvents pnlPostFeeder As PanelGroupBox
    Friend WithEvents cmbMPPost As ComboBoxPersian
    Friend WithEvents cmbMPFeeder As ComboBoxPersian
    Friend WithEvents txtLastPeakBar As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents pnlTamirRequestInfo As PanelGroupBox
    Friend WithEvents pnlConfirmInfo As System.Windows.Forms.Panel
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents Label35 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents pnlConfirm As PanelGroupBox
    Friend WithEvents pnlAllow As PanelGroupBox
    Friend WithEvents pnlManoeuvre As Bargh_Common.PanelGroupBox
    Friend WithEvents pnlManoeuvrePostFeeder As System.Windows.Forms.Panel
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents chkIsManoeuvre As System.Windows.Forms.CheckBox
    Friend WithEvents tp4_Confirms As System.Windows.Forms.TabPage
    Friend WithEvents tp3_OperationList As System.Windows.Forms.TabPage
    Friend WithEvents tp2_DCLocation As System.Windows.Forms.TabPage
    Friend WithEvents tp1_Specs As System.Windows.Forms.TabPage
    Friend WithEvents tp5_nDCFeeder As System.Windows.Forms.TabPage
    Friend WithEvents tbcMain As System.Windows.Forms.TabControl
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnReturn As System.Windows.Forms.Button
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents txtSumTime As System.Windows.Forms.TextBox
    Friend WithEvents lnkRequestNumber As System.Windows.Forms.LinkLabel
    Friend WithEvents txtUsedPower As System.Windows.Forms.TextBox
    Friend WithEvents chkIsWarmLine As System.Windows.Forms.CheckBox
    Friend WithEvents txtPower As Bargh_Common.TextBoxPersian
    Friend WithEvents cmbRequestUserName As ComboBoxPersian
    Friend WithEvents cmbCloserType As System.Windows.Forms.ComboBox
    Friend WithEvents txtAsdresses As System.Windows.Forms.TextBox
    Friend WithEvents txtCriticalLocations As System.Windows.Forms.TextBox
    Friend WithEvents cmbManoeuvreType As Bargh_Common.ComboBoxPersian
    Friend WithEvents pnlAdsress As Bargh_Common.PanelGroupBox
    Friend WithEvents cmbConfirmUserName As ComboBoxPersian
    Friend WithEvents txtTimeConfirm As Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor
    Friend WithEvents txtDateConfirm As Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor
    Friend WithEvents txtUnConfirmReason As Bargh_Common.TextBoxPersian
    Friend WithEvents rbIsConfirm As System.Windows.Forms.RadioButton
    Friend WithEvents rbIsNotConfirm As System.Windows.Forms.RadioButton
    Friend WithEvents txtAllowNumber As System.Windows.Forms.TextBox
    Friend WithEvents txtDateAllowStart As Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor
    Friend WithEvents txtDateAllowEnd As Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor
    Friend WithEvents cmbAllowUserName As ComboBoxPersian
    Friend WithEvents txtDateAllowDE As Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor
    Friend WithEvents txtTimeAllowDE As Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor
    Friend WithEvents txtTimeAllowStart As Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor
    Friend WithEvents txtTimeAllowEnd As Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor
    Friend WithEvents dgFeederDC As Bargh_Common.JGrid
    Friend WithEvents DatasetTamir1 As Bargh_DataSets.DatasetTamir
    Friend WithEvents DatasetBT1 As Bargh_DataSets.DatasetBT
    Friend WithEvents txtCurrentValue As Bargh_Common.TextBoxPersian
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents txtDCInterval As Bargh_Common.TextBoxPersian
    Friend WithEvents Label37 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents cmbState As Bargh_Common.ComboBoxPersian
    Friend WithEvents DatasetDummy1 As Bargh_DataSets.DatasetDummy
    Friend WithEvents DatasetTamirView1 As Bargh_DataSets.DatasetTamirView
    Friend WithEvents dgOperations As Bargh_Common.JGrid
    Friend WithEvents chkIsSendToSetad As System.Windows.Forms.CheckBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents Label38 As System.Windows.Forms.Label
    Friend WithEvents txtFeederDCDayCount As Bargh_Common.TextBoxPersian
    Friend WithEvents btnLoadFeederDC As System.Windows.Forms.Button
    Friend WithEvents Label40 As System.Windows.Forms.Label
    Friend WithEvents cmbWeather As Bargh_Common.ComboBoxPersian
    Friend WithEvents Label36 As System.Windows.Forms.Label
    Friend WithEvents cmbPeymankar As Bargh_Common.ComboBoxPersian
    Friend WithEvents Label42 As System.Windows.Forms.Label
    Friend WithEvents Label44 As System.Windows.Forms.Label
    Friend WithEvents Label46 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents txtLastUsedPower As System.Windows.Forms.TextBox
    Friend WithEvents txtWaitPower As System.Windows.Forms.TextBox
    Friend WithEvents txtRemainPower As System.Windows.Forms.TextBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents pnlAreaCity As System.Windows.Forms.Panel
    Friend WithEvents pnlInfo As Bargh_Common.PanelGroupBox
    Friend WithEvents Label47 As System.Windows.Forms.Label
    Friend WithEvents txtWorkingAddress As System.Windows.Forms.TextBox
    Friend WithEvents txtDCWantedPower As System.Windows.Forms.TextBox
    Friend WithEvents Label49 As System.Windows.Forms.Label
    Friend WithEvents Label50 As System.Windows.Forms.Label
    Friend WithEvents cmbNazer As Bargh_Common.ComboBoxPersian
    Friend WithEvents pnlNazer As System.Windows.Forms.Panel
    Friend WithEvents btnSaveNazer As System.Windows.Forms.Button
    Friend WithEvents cmbFeederPart As Bargh_Common.ComboBoxPersian
    Friend WithEvents Label39 As System.Windows.Forms.Label
    Friend WithEvents cmbPeriod As System.Windows.Forms.ComboBox
    Friend WithEvents pnlDate As System.Windows.Forms.Panel
    Friend WithEvents Label52 As System.Windows.Forms.Label
    Friend WithEvents pnlToDate As System.Windows.Forms.Panel
    Friend WithEvents Label53 As System.Windows.Forms.Label
    Friend WithEvents txtDateTo As Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor
    Friend WithEvents txtDateFrom As Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor
    Friend WithEvents pnlDCDayCount As System.Windows.Forms.Panel
    Friend WithEvents pnlNetworkType As System.Windows.Forms.Panel
    Friend WithEvents pnlLastDCCount As System.Windows.Forms.Panel
    Friend WithEvents txtLastDCCount As Bargh_Common.TextBoxPersian
    Friend WithEvents Label54 As System.Windows.Forms.Label
    Friend WithEvents Label55 As System.Windows.Forms.Label
    Friend WithEvents rbNT_Tamir As System.Windows.Forms.RadioButton
    Friend WithEvents rbNT_NotTamir As System.Windows.Forms.RadioButton
    Friend WithEvents rbNT_Both As System.Windows.Forms.RadioButton
    Friend WithEvents cmbConfirmNaheihUsername As Bargh_Common.ComboBoxPersian
    Friend WithEvents Label56 As System.Windows.Forms.Label
    Friend WithEvents cmbConfirmCenterUsername As Bargh_Common.ComboBoxPersian
    Friend WithEvents Label57 As System.Windows.Forms.Label
    Friend WithEvents cmbConfirmSetadUsername As Bargh_Common.ComboBoxPersian
    Friend WithEvents Label58 As System.Windows.Forms.Label
    Friend WithEvents chkIsEmergency As System.Windows.Forms.CheckBox
    Friend WithEvents lblFeederPart As System.Windows.Forms.Label
    Friend WithEvents cmbLPPost As Bargh_Common.ComboBoxPersian
    Friend WithEvents lblLPPost As System.Windows.Forms.Label
    Friend WithEvents rbLPPost As System.Windows.Forms.RadioButton
    Friend WithEvents rbFeederPart As System.Windows.Forms.RadioButton
    Friend WithEvents chkIsLPPostFeederPart As System.Windows.Forms.CheckBox
    Friend WithEvents pnlLPPostFeederPart As Bargh_Common.BracketPanel
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtSMSCOunt As System.Windows.Forms.TextBox
    Friend WithEvents chkIsIRIB As System.Windows.Forms.CheckBox
    Friend WithEvents mDsReqView As Bargh_DataSets.DatasetCcRequesterView
    Friend WithEvents Label59 As System.Windows.Forms.Label
    Friend WithEvents txtLPPostCountDC As System.Windows.Forms.TextBox
    Friend WithEvents Label60 As System.Windows.Forms.Label
    Friend WithEvents Label61 As System.Windows.Forms.Label
    Friend WithEvents Label62 As System.Windows.Forms.Label
    Friend WithEvents Label63 As System.Windows.Forms.Label
    Friend WithEvents HatchPanel1 As Bargh_Common.HatchPanel
    Friend WithEvents HatchPanel2 As Bargh_Common.HatchPanel
    Friend WithEvents HatchPanel3 As Bargh_Common.HatchPanel
    Friend WithEvents HatchPanel4 As Bargh_Common.HatchPanel
    Friend WithEvents txtTMRequest As Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor
    Friend WithEvents txtDTRequest As Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor
    Friend WithEvents txtTMSendToCenter As Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor
    Friend WithEvents txtDTSendToCenter As Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor
    Friend WithEvents txtDTSendToSetad As Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor
    Friend WithEvents txtTMSendToSetad As Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor
    Friend WithEvents txtTMConfirm As Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor
    Friend WithEvents txtDTConfirm As Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor
    Friend WithEvents pnlEmergency As System.Windows.Forms.Panel
    Friend WithEvents btnEmergencyOK As System.Windows.Forms.Button
    Friend WithEvents btnEmergencyCancel As System.Windows.Forms.Button
    Friend WithEvents lblEmergency As System.Windows.Forms.TextBox
    Friend WithEvents Label64 As System.Windows.Forms.Label
    Private WithEvents txtEmergencyReason As System.Windows.Forms.TextBox
    Friend WithEvents Label65 As System.Windows.Forms.Label
    Friend WithEvents txtEmergencyReasonDisplay As System.Windows.Forms.TextBox
    Friend WithEvents Label66 As System.Windows.Forms.Label
    Friend WithEvents Label67 As System.Windows.Forms.Label
    Friend WithEvents lnkManoeuvreDesc As System.Windows.Forms.LinkLabel
    Friend WithEvents pnlManoeuvreDesc As System.Windows.Forms.Panel
    Friend WithEvents txtManoeuvreDesc As Bargh_Common.TextBoxPersian
    Friend WithEvents picOKManoeuvreDesc As System.Windows.Forms.PictureBox
    Friend WithEvents Label68 As System.Windows.Forms.Label
    Friend WithEvents Label69 As System.Windows.Forms.Label
    Friend WithEvents txtGpsX As Bargh_Common.TextBoxPersian
    Friend WithEvents txtGpsY As Bargh_Common.TextBoxPersian
    Friend WithEvents Label70 As System.Windows.Forms.Label
    Friend WithEvents lblWorkCommandNo As System.Windows.Forms.Label
    Friend WithEvents txtWorkCommandNo As Bargh_Common.TextBoxPersian
    Friend WithEvents rbInCityService As System.Windows.Forms.RadioButton
    Friend WithEvents rbNotInCityService As System.Windows.Forms.RadioButton
    Friend WithEvents Label71 As System.Windows.Forms.Label
    Friend WithEvents rbIsRequestByPeymankar As System.Windows.Forms.RadioButton
    Friend WithEvents rbIsRequestByTozi As System.Windows.Forms.RadioButton
    Friend WithEvents BracketPanel1 As Bargh_Common.BracketPanel
    Friend WithEvents HatchPanel5 As Bargh_Common.HatchPanel
    Friend WithEvents Label72 As System.Windows.Forms.Label
    Friend WithEvents cmbLPFeeder As Bargh_Common.ComboBoxPersian
    Friend WithEvents Label74 As System.Windows.Forms.Label
    Friend WithEvents pnlWorkCommandNo As System.Windows.Forms.Panel
    Friend WithEvents lblDepartment As System.Windows.Forms.Label
    Friend WithEvents cmbDepartment As Bargh_Common.ComboBoxPersian
    Friend WithEvents Label73 As System.Windows.Forms.Label
    Friend WithEvents cmbReferTo As Bargh_Common.ComboBoxPersian
    Friend WithEvents cmbTamirNetworkType As Bargh_Common.ComboBoxPersian
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents Label75 As System.Windows.Forms.Label
    Friend WithEvents rbIsRequestByFogheTozi As System.Windows.Forms.RadioButton
    Friend WithEvents txtProjectNo As Bargh_Common.TextBoxPersian
    Friend WithEvents pnlTamirType As System.Windows.Forms.Panel
    Friend WithEvents Label76 As System.Windows.Forms.Label
    Friend WithEvents cmbTamirType As Bargh_Common.ComboBoxPersian
    Friend WithEvents lblPowerUnit As System.Windows.Forms.Label
    Friend WithEvents ckcmbMPFeeder As Bargh_Common.ChkCombo
    Friend WithEvents btnMultiStep As System.Windows.Forms.Button
    Friend WithEvents Label91 As System.Windows.Forms.Label
    Friend WithEvents Label92 As System.Windows.Forms.Label
    Friend WithEvents Label93 As System.Windows.Forms.Label
    Friend WithEvents Label94 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label77 As System.Windows.Forms.Label
    Friend WithEvents Label78 As System.Windows.Forms.Label
    Friend WithEvents Label79 As System.Windows.Forms.Label
    Friend WithEvents Label80 As System.Windows.Forms.Label
    Friend WithEvents Label81 As System.Windows.Forms.Label
    Friend WithEvents Label82 As System.Windows.Forms.Label
    Friend WithEvents Label83 As System.Windows.Forms.Label
    Friend WithEvents Label84 As System.Windows.Forms.Label
    Friend WithEvents Label85 As System.Windows.Forms.Label
    Friend WithEvents Label86 As System.Windows.Forms.Label
    Friend WithEvents Label87 As System.Windows.Forms.Label
    Friend WithEvents Label88 As System.Windows.Forms.Label
    Friend WithEvents Label89 As System.Windows.Forms.Label
    Friend WithEvents Label90 As System.Windows.Forms.Label
    Friend WithEvents Label95 As System.Windows.Forms.Label
    Friend WithEvents Label96 As System.Windows.Forms.Label
    Friend WithEvents Label97 As System.Windows.Forms.Label
    Friend WithEvents Label98 As System.Windows.Forms.Label
    Friend WithEvents Label99 As System.Windows.Forms.Label
    Friend WithEvents pnlMultiStepInfo As Bargh_Common.PanelGroupBox
    Friend WithEvents pnlLevel6 As System.Windows.Forms.Panel
    Friend WithEvents txtMSBar6 As Bargh_Common.TextBoxPersian
    Friend WithEvents txtMSTime6 As Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor
    Friend WithEvents pnlLevel5 As System.Windows.Forms.Panel
    Friend WithEvents txtMSBar5 As Bargh_Common.TextBoxPersian
    Friend WithEvents txtMSTime5 As Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor
    Friend WithEvents pnlLevel4 As System.Windows.Forms.Panel
    Friend WithEvents txtMSBar4 As Bargh_Common.TextBoxPersian
    Friend WithEvents txtMSTime4 As Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor
    Friend WithEvents pnlLevel3 As System.Windows.Forms.Panel
    Friend WithEvents txtMSBar3 As Bargh_Common.TextBoxPersian
    Friend WithEvents txtMSTime3 As Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor
    Friend WithEvents pnlLevel2 As System.Windows.Forms.Panel
    Friend WithEvents txtMSBar2 As Bargh_Common.TextBoxPersian
    Friend WithEvents txtMSTime2 As Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor
    Friend WithEvents pnlLevel1 As System.Windows.Forms.Panel
    Friend WithEvents txtMSBar1 As Bargh_Common.TextBoxPersian
    Friend WithEvents txtMSTime1 As Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor
    Friend WithEvents btnMSOK As System.Windows.Forms.Button
    Friend WithEvents btnMSCancel As System.Windows.Forms.Button
    Friend WithEvents txtMSDesc6 As System.Windows.Forms.TextBox
    Friend WithEvents txtMSDesc5 As System.Windows.Forms.TextBox
    Friend WithEvents txtMSDesc4 As System.Windows.Forms.TextBox
    Friend WithEvents txtMSDesc3 As System.Windows.Forms.TextBox
    Friend WithEvents txtMSDesc2 As System.Windows.Forms.TextBox
    Friend WithEvents txtMSDesc1 As System.Windows.Forms.TextBox
    Friend WithEvents HatchPanel6 As Bargh_Common.HatchPanel
    Friend WithEvents HatchPanel7 As Bargh_Common.HatchPanel
    Friend WithEvents HatchPanel8 As Bargh_Common.HatchPanel
    Friend WithEvents HatchPanel9 As Bargh_Common.HatchPanel
    Friend WithEvents HatchPanel10 As Bargh_Common.HatchPanel
    Friend WithEvents HatchPanel11 As Bargh_Common.HatchPanel
    Friend WithEvents Label100 As System.Windows.Forms.Label
    Friend WithEvents Label101 As System.Windows.Forms.Label
    Friend WithEvents Label102 As System.Windows.Forms.Label
    Friend WithEvents Label103 As System.Windows.Forms.Label
    Friend WithEvents Label104 As System.Windows.Forms.Label
    Friend WithEvents Label105 As System.Windows.Forms.Label
    Friend WithEvents txtMSDate1 As Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor
    Friend WithEvents txtMSDate2 As Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor
    Friend WithEvents txtMSDate3 As Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor
    Friend WithEvents txtMSDate4 As Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor
    Friend WithEvents txtMSDate5 As Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor
    Friend WithEvents txtMSDate6 As Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor
    Friend WithEvents Label106 As System.Windows.Forms.Label
    Friend WithEvents txtDCCurrent As Bargh_Common.TextBoxPersian
    Friend WithEvents HatchPanel12 As Bargh_Common.HatchPanel
    Friend WithEvents pnlRequestedBy As System.Windows.Forms.Panel
    Friend WithEvents pnlOverlaps As Bargh_Common.PanelGroupBox
    Friend WithEvents btnCloseOverlaps As System.Windows.Forms.Button
    Friend WithEvents dgOverlaps As Bargh_Common.JGrid
    Friend WithEvents sbtnOverlaps As Bargh_Common.StyleButton
    Friend WithEvents picSearchMPPOverlaps As System.Windows.Forms.PictureBox
    Friend WithEvents picSearchMPFOverlaps As System.Windows.Forms.PictureBox
    Friend WithEvents pnlSOverlaps As Bargh_Common.PanelGroupBox
    Friend WithEvents dgSOverlaps As Bargh_Common.JGrid
    Friend WithEvents btnSCloseOverlaps As System.Windows.Forms.Button
    Friend WithEvents lblPeakBar As System.Windows.Forms.Label
    Friend WithEvents pnlLetterInfo As System.Windows.Forms.Panel
    Friend WithEvents Label51 As System.Windows.Forms.Label
    Friend WithEvents btnHidePnlLetter As System.Windows.Forms.PictureBox
    Friend WithEvents Label107 As System.Windows.Forms.Label
    Friend WithEvents Label108 As System.Windows.Forms.Label
    Friend WithEvents txtOperationDesc As Bargh_Common.TextBoxPersian
    Friend WithEvents lblLetterInfo As System.Windows.Forms.LinkLabel
    Friend WithEvents picIsAuotoNumber As System.Windows.Forms.PictureBox
    Friend WithEvents chkIsAutoNumber As System.Windows.Forms.CheckBox
    Friend WithEvents lblWeakPower1 As System.Windows.Forms.Label
    Friend WithEvents lblWeakPower2 As System.Windows.Forms.Label
    Friend WithEvents lblWeakPower5 As System.Windows.Forms.Label
    Friend WithEvents lblWeakPower3 As System.Windows.Forms.Label
    Friend WithEvents lblWeakPower6 As System.Windows.Forms.Label
    Friend WithEvents lblWeakPower4 As System.Windows.Forms.Label
    Friend WithEvents pnlTamirRequestSubject As System.Windows.Forms.Panel
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cmbTamirRequestSubject As Bargh_Common.ComboBoxPersian
    Friend WithEvents rbIsReturned As System.Windows.Forms.RadioButton
    Friend WithEvents lblNotConfirmReason As System.Windows.Forms.Label
    Friend WithEvents picLock As System.Windows.Forms.PictureBox
    Friend WithEvents txtGISFormNo As Bargh_Common.TextBoxPersian
    Friend WithEvents lblGISFormNo As System.Windows.Forms.Label
    Friend WithEvents pnlManoeuvreRequestInfo As Bargh_Common.PanelGroupBox
    Friend WithEvents txtCurrentValueManovr As Bargh_Common.TextBoxPersian
    Friend WithEvents txtTimeDisconnectManovr As Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor
    Friend WithEvents txtDateDisconnectManovr As Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents txtTimeConnectManovr As Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor
    Friend WithEvents txtDateConnectManovr As Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents Label41 As System.Windows.Forms.Label
    Friend WithEvents txtPowerManovr As Bargh_Common.TextBoxPersian
    Friend WithEvents Label43 As System.Windows.Forms.Label
    Friend WithEvents txtDCIntervalManovr As Bargh_Common.TextBoxPersian
    Friend WithEvents Label48 As System.Windows.Forms.Label
    Friend WithEvents Label109 As System.Windows.Forms.Label
    Friend WithEvents Label110 As System.Windows.Forms.Label
    Friend WithEvents tpDCMain As System.Windows.Forms.TabPage
    Friend WithEvents tpDCMon As System.Windows.Forms.TabPage
    Friend WithEvents tbcDCs As System.Windows.Forms.TabControl
    Friend WithEvents lblPowerUnitManovr As System.Windows.Forms.Label
    Friend WithEvents chkIsNotNeedManovrDC As System.Windows.Forms.CheckBox
    Friend WithEvents lnkRequestNumberManovr As System.Windows.Forms.LinkLabel
    Friend WithEvents lblRequestNumber As System.Windows.Forms.Label
    Friend WithEvents lblRequestNumberManovr As System.Windows.Forms.Label
    Friend WithEvents lblTamirRequestNo As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtPowerTotal As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents chkIsConfirmNazer As System.Windows.Forms.CheckBox
    Friend WithEvents pnlWarmLineConfirm As Bargh_Common.PanelGroupBox
    Friend WithEvents txtWarmLineReason As Bargh_Common.TextBoxPersian
    Friend WithEvents rbConfirmWL As System.Windows.Forms.RadioButton
    Friend WithEvents rbNotConfirmWL As System.Windows.Forms.RadioButton
    Friend WithEvents txtWarmLineConfirmUserName As System.Windows.Forms.TextBox
    Friend WithEvents Label45 As System.Windows.Forms.Label
    Friend txtWarmLineTime As Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor
    Friend txtWarmLineDate As Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor
    Friend WithEvents Label111 As System.Windows.Forms.Label
    Friend WithEvents Label112 As System.Windows.Forms.Label
    Friend WithEvents Label113 As System.Windows.Forms.Label
    Friend WithEvents chkIsRemoteDCChange As System.Windows.Forms.CheckBox
    Friend WithEvents chkIsSendSMSSensitive As System.Windows.Forms.CheckBox
    Friend WithEvents PicSendSMSSensitive As System.Windows.Forms.PictureBox
    Friend WithEvents btnFeederKey1 As System.Windows.Forms.Button
    Friend WithEvents btnFeederKey2 As System.Windows.Forms.Button
    Friend WithEvents btnFeederKey3 As System.Windows.Forms.Button
    Friend WithEvents btnFeederKey4 As System.Windows.Forms.Button
    Friend WithEvents btnFeederKey5 As System.Windows.Forms.Button
    Friend WithEvents btnFeederKey6 As System.Windows.Forms.Button
    Friend WithEvents btnFeederKey0 As System.Windows.Forms.Button
    Friend WithEvents cmnuFeederKey As System.Windows.Forms.ContextMenu
    Friend WithEvents cmnuFeederKeyOnDisconnect As System.Windows.Forms.MenuItem
    Friend WithEvents cmnuFeederKeyOnConnect As System.Windows.Forms.MenuItem
    Friend WithEvents pnlMPFeederKey As Bargh_Common.PanelGroupBox
    Friend WithEvents dgFeederKey As Bargh_Common.JGrid
    Friend WithEvents chkIsRemoteChange As System.Windows.Forms.CheckBox
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents pnlMPFeederKeyFilter As System.Windows.Forms.Panel
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents txtMPFeederKey As System.Windows.Forms.TextBox
    Friend WithEvents Label114 As System.Windows.Forms.Label
    Friend WithEvents cmbMPCloserType As Bargh_Common.ComboBoxPersian
    Friend WithEvents Label115 As System.Windows.Forms.Label
    Friend WithEvents btnReturnFeederKey As System.Windows.Forms.Button
    Friend WithEvents picPeymankarSearch As System.Windows.Forms.PictureBox
    Friend WithEvents txtPeymankarSearch As Bargh_Common.TextBoxPersian
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmNewTamirRequest))
        Dim dgSOverlaps_DesignTimeLayout As Janus.Windows.GridEX.GridEXLayout = New Janus.Windows.GridEX.GridEXLayout()
        Dim dgOverlaps_DesignTimeLayout As Janus.Windows.GridEX.GridEXLayout = New Janus.Windows.GridEX.GridEXLayout()
        Dim dgFiles_DesignTimeLayout As Janus.Windows.GridEX.GridEXLayout = New Janus.Windows.GridEX.GridEXLayout()
        Dim dgFeederDC_DesignTimeLayout As Janus.Windows.GridEX.GridEXLayout = New Janus.Windows.GridEX.GridEXLayout()
        Dim dgOperations_DesignTimeLayout As Janus.Windows.GridEX.GridEXLayout = New Janus.Windows.GridEX.GridEXLayout()
        Dim dgFeederKey_Layout_0 As Janus.Windows.GridEX.GridEXLayout = New Janus.Windows.GridEX.GridEXLayout()
        Me.pnlNumbers = New System.Windows.Forms.Panel()
        Me.txtPowerTotal = New System.Windows.Forms.TextBox()
        Me.lblWeakPower1 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtMaxPower = New System.Windows.Forms.TextBox()
        Me.lblWeakPower2 = New System.Windows.Forms.Label()
        Me.txtUsedPower = New System.Windows.Forms.TextBox()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.lblWeakPower5 = New System.Windows.Forms.Label()
        Me.txtLastUsedPower = New System.Windows.Forms.TextBox()
        Me.Label42 = New System.Windows.Forms.Label()
        Me.lblWeakPower3 = New System.Windows.Forms.Label()
        Me.txtWaitPower = New System.Windows.Forms.TextBox()
        Me.Label44 = New System.Windows.Forms.Label()
        Me.lblWeakPower6 = New System.Windows.Forms.Label()
        Me.txtRemainPower = New System.Windows.Forms.TextBox()
        Me.Label46 = New System.Windows.Forms.Label()
        Me.txtDCWantedPower = New System.Windows.Forms.TextBox()
        Me.lblWeakPower4 = New System.Windows.Forms.Label()
        Me.Label49 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lnkRequestNumber = New System.Windows.Forms.LinkLabel()
        Me.txtTamirRequestNumber = New System.Windows.Forms.TextBox()
        Me.lblTamirRequestNo = New System.Windows.Forms.Label()
        Me.lblRequestNumber = New System.Windows.Forms.Label()
        Me.cmbArea = New Bargh_Common.ComboBoxPersian()
        Me.DatasetBT1 = New Bargh_DataSets.DatasetBT()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cmbCity = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.pnlPostFeeder = New Bargh_Common.PanelGroupBox()
        Me.chkMPPostTrans = New Bargh_Common.ChkCombo()
        Me.btnFeederKey0 = New System.Windows.Forms.Button()
        Me.chkIsRemoteDCChange = New System.Windows.Forms.CheckBox()
        Me.picSearchMPPOverlaps = New System.Windows.Forms.PictureBox()
        Me.btnMultiStep = New System.Windows.Forms.Button()
        Me.txtGpsX = New Bargh_Common.TextBoxPersian()
        Me.txtGpsY = New Bargh_Common.TextBoxPersian()
        Me.chkIsLPPostFeederPart = New System.Windows.Forms.CheckBox()
        Me.cmbMPPost = New Bargh_Common.ComboBoxPersian()
        Me.DatasetDummy1 = New Bargh_DataSets.DatasetDummy()
        Me.lblMPPostTrans = New System.Windows.Forms.Label()
        Me.cmbMPFeeder = New Bargh_Common.ComboBoxPersian()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtLastPeakBar = New System.Windows.Forms.TextBox()
        Me.cmbCloserType = New System.Windows.Forms.ComboBox()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.lblFeederPart = New System.Windows.Forms.Label()
        Me.cmbLPPost = New Bargh_Common.ComboBoxPersian()
        Me.btnSelectLPPost = New System.Windows.Forms.Button()
        Me.cmbFeederPart = New Bargh_Common.ComboBoxPersian()
        Me.lblLPPost = New System.Windows.Forms.Label()
        Me.pnlLPPostFeederPart = New Bargh_Common.BracketPanel()
        Me.rbLPPost = New System.Windows.Forms.RadioButton()
        Me.rbFeederPart = New System.Windows.Forms.RadioButton()
        Me.lblPeakBar = New System.Windows.Forms.Label()
        Me.Label59 = New System.Windows.Forms.Label()
        Me.txtLPPostCountDC = New System.Windows.Forms.TextBox()
        Me.Label68 = New System.Windows.Forms.Label()
        Me.Label69 = New System.Windows.Forms.Label()
        Me.Label70 = New System.Windows.Forms.Label()
        Me.cmbLPFeeder = New Bargh_Common.ComboBoxPersian()
        Me.Label72 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.cmbTamirNetworkType = New Bargh_Common.ComboBoxPersian()
        Me.Label74 = New System.Windows.Forms.Label()
        Me.ckcmbMPFeeder = New Bargh_Common.ChkCombo()
        Me.picSearchMPFOverlaps = New System.Windows.Forms.PictureBox()
        Me.pnlSOverlaps = New Bargh_Common.PanelGroupBox()
        Me.dgSOverlaps = New Bargh_Common.JGrid()
        Me.btnSCloseOverlaps = New System.Windows.Forms.Button()
        Me.txtAsdresses = New System.Windows.Forms.TextBox()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.txtCriticalLocations = New System.Windows.Forms.TextBox()
        Me.pnlTamirRequestInfo = New Bargh_Common.PanelGroupBox()
        Me.txtCurrentValue = New Bargh_Common.TextBoxPersian()
        Me.chkIsWarmLine = New System.Windows.Forms.CheckBox()
        Me.txtTimeDisconnect = New Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor()
        Me.txtDateDisconnect = New Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtTimeConnect = New Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor()
        Me.txtDateConnect = New Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.txtPower = New Bargh_Common.TextBoxPersian()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.lblPowerUnit = New System.Windows.Forms.Label()
        Me.txtDCInterval = New Bargh_Common.TextBoxPersian()
        Me.Label37 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.pnlOverlaps = New Bargh_Common.PanelGroupBox()
        Me.dgOverlaps = New Bargh_Common.JGrid()
        Me.btnCloseOverlaps = New System.Windows.Forms.Button()
        Me.sbtnOverlaps = New Bargh_Common.StyleButton()
        Me.pnlConfirmInfo = New System.Windows.Forms.Panel()
        Me.HatchPanel1 = New Bargh_Common.HatchPanel()
        Me.txtTMRequest = New Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor()
        Me.txtDTRequest = New Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.cmbRequestUserName = New Bargh_Common.ComboBoxPersian()
        Me.cmbConfirmNaheihUsername = New Bargh_Common.ComboBoxPersian()
        Me.Label56 = New System.Windows.Forms.Label()
        Me.cmbConfirmCenterUsername = New Bargh_Common.ComboBoxPersian()
        Me.Label57 = New System.Windows.Forms.Label()
        Me.cmbConfirmSetadUsername = New Bargh_Common.ComboBoxPersian()
        Me.Label58 = New System.Windows.Forms.Label()
        Me.Label60 = New System.Windows.Forms.Label()
        Me.Label61 = New System.Windows.Forms.Label()
        Me.Label62 = New System.Windows.Forms.Label()
        Me.Label63 = New System.Windows.Forms.Label()
        Me.txtTMSendToCenter = New Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor()
        Me.txtDTSendToCenter = New Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor()
        Me.txtDTSendToSetad = New Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor()
        Me.txtTMSendToSetad = New Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor()
        Me.txtTMConfirm = New Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor()
        Me.txtDTConfirm = New Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor()
        Me.HatchPanel2 = New Bargh_Common.HatchPanel()
        Me.HatchPanel3 = New Bargh_Common.HatchPanel()
        Me.HatchPanel4 = New Bargh_Common.HatchPanel()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.cmbConfirmUserName = New Bargh_Common.ComboBoxPersian()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.txtTimeConfirm = New Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor()
        Me.txtDateConfirm = New Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor()
        Me.pnlConfirm = New Bargh_Common.PanelGroupBox()
        Me.chkIsSendToInformApp = New System.Windows.Forms.CheckBox()
        Me.PicSendSMSSensitive = New System.Windows.Forms.PictureBox()
        Me.chkIsSendSMSSensitive = New System.Windows.Forms.CheckBox()
        Me.picLock = New System.Windows.Forms.PictureBox()
        Me.txtUnConfirmReason = New Bargh_Common.TextBoxPersian()
        Me.rbIsConfirm = New System.Windows.Forms.RadioButton()
        Me.rbIsNotConfirm = New System.Windows.Forms.RadioButton()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.lblNotConfirmReason = New System.Windows.Forms.Label()
        Me.rbIsReturned = New System.Windows.Forms.RadioButton()
        Me.txtReturn = New System.Windows.Forms.TextBox()
        Me.LabelRetrun = New System.Windows.Forms.Label()
        Me.pnlAllow = New Bargh_Common.PanelGroupBox()
        Me.picIsAuotoNumber = New System.Windows.Forms.PictureBox()
        Me.chkIsAutoNumber = New System.Windows.Forms.CheckBox()
        Me.Label66 = New System.Windows.Forms.Label()
        Me.Label67 = New System.Windows.Forms.Label()
        Me.cmbAllowUserName = New Bargh_Common.ComboBoxPersian()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.txtDateAllowDE = New Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor()
        Me.txtTimeAllowDE = New Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.txtAllowNumber = New System.Windows.Forms.TextBox()
        Me.Label33 = New System.Windows.Forms.Label()
        Me.txtTimeAllowStart = New Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor()
        Me.txtDateAllowStart = New Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor()
        Me.Label35 = New System.Windows.Forms.Label()
        Me.txtDateAllowEnd = New Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor()
        Me.txtTimeAllowEnd = New Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnReturn = New System.Windows.Forms.Button()
        Me.tbcMain = New System.Windows.Forms.TabControl()
        Me.tp6_Docs = New System.Windows.Forms.TabPage()
        Me.dgFiles = New Bargh_Common.JGrid()
        Me.DatasetTamir1 = New Bargh_DataSets.DatasetTamir()
        Me.grpNewFile = New System.Windows.Forms.GroupBox()
        Me.btnAddFile = New System.Windows.Forms.Button()
        Me.btnBrowseForFileUpload = New System.Windows.Forms.Button()
        Me.txtSubject = New System.Windows.Forms.TextBox()
        Me.txtUploadFilePath = New System.Windows.Forms.TextBox()
        Me.Label117 = New System.Windows.Forms.Label()
        Me.Label116 = New System.Windows.Forms.Label()
        Me.tp5_nDCFeeder = New System.Windows.Forms.TabPage()
        Me.cmbPeriod = New System.Windows.Forms.ComboBox()
        Me.pnlDate = New System.Windows.Forms.Panel()
        Me.Label52 = New System.Windows.Forms.Label()
        Me.pnlToDate = New System.Windows.Forms.Panel()
        Me.txtDateTo = New Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor()
        Me.Label53 = New System.Windows.Forms.Label()
        Me.txtDateFrom = New Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor()
        Me.btnLoadFeederDC = New System.Windows.Forms.Button()
        Me.pnlNetworkType = New System.Windows.Forms.Panel()
        Me.rbNT_Tamir = New System.Windows.Forms.RadioButton()
        Me.rbNT_NotTamir = New System.Windows.Forms.RadioButton()
        Me.rbNT_Both = New System.Windows.Forms.RadioButton()
        Me.pnlDCDayCount = New System.Windows.Forms.Panel()
        Me.txtFeederDCDayCount = New Bargh_Common.TextBoxPersian()
        Me.Label39 = New System.Windows.Forms.Label()
        Me.Label38 = New System.Windows.Forms.Label()
        Me.pnlLastDCCount = New System.Windows.Forms.Panel()
        Me.txtLastDCCount = New Bargh_Common.TextBoxPersian()
        Me.Label54 = New System.Windows.Forms.Label()
        Me.Label55 = New System.Windows.Forms.Label()
        Me.dgFeederDC = New Bargh_Common.JGrid()
        Me.mDsReqView = New Bargh_DataSets.DatasetCcRequesterView()
        Me.tp4_Confirms = New System.Windows.Forms.TabPage()
        Me.pnlWarmLineConfirm = New Bargh_Common.PanelGroupBox()
        Me.rbIsReturnedWL = New System.Windows.Forms.RadioButton()
        Me.txtWarmLineReason = New Bargh_Common.TextBoxPersian()
        Me.rbConfirmWL = New System.Windows.Forms.RadioButton()
        Me.rbNotConfirmWL = New System.Windows.Forms.RadioButton()
        Me.txtWarmLineConfirmUserName = New System.Windows.Forms.TextBox()
        Me.Label45 = New System.Windows.Forms.Label()
        Me.txtWarmLineTime = New Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor()
        Me.txtWarmLineDate = New Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor()
        Me.Label111 = New System.Windows.Forms.Label()
        Me.Label112 = New System.Windows.Forms.Label()
        Me.Label113 = New System.Windows.Forms.Label()
        Me.tp3_OperationList = New System.Windows.Forms.TabPage()
        Me.txtOperationDesc = New Bargh_Common.TextBoxPersian()
        Me.Label108 = New System.Windows.Forms.Label()
        Me.dgOperations = New Bargh_Common.JGrid()
        Me.DatasetTamirView1 = New Bargh_DataSets.DatasetTamirView()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.txtSumTime = New System.Windows.Forms.TextBox()
        Me.Label40 = New System.Windows.Forms.Label()
        Me.tp2_DCLocation = New System.Windows.Forms.TabPage()
        Me.picFeederPart = New System.Windows.Forms.PictureBox()
        Me.txtFeederPart = New Bargh_Common.TextBoxPersian()
        Me.btnLPFeeder = New System.Windows.Forms.Button()
        Me.pnlPeymankar = New Bargh_Common.PanelGroupBox()
        Me.btnPeymankarTel = New System.Windows.Forms.Button()
        Me.btnPeymankar = New System.Windows.Forms.Button()
        Me.txtPeymankarTel = New Bargh_Common.TextBoxPersian()
        Me.Label120 = New System.Windows.Forms.Label()
        Me.lblPeymankarManager = New System.Windows.Forms.Label()
        Me.Label34 = New System.Windows.Forms.Label()
        Me.lblPeymankar = New System.Windows.Forms.LinkLabel()
        Me.pnlManoeuvreRequestInfo = New Bargh_Common.PanelGroupBox()
        Me.txtCurrentValueManovr = New Bargh_Common.TextBoxPersian()
        Me.txtTimeDisconnectManovr = New Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor()
        Me.txtDateDisconnectManovr = New Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.txtTimeConnectManovr = New Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor()
        Me.txtDateConnectManovr = New Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.Label41 = New System.Windows.Forms.Label()
        Me.txtPowerManovr = New Bargh_Common.TextBoxPersian()
        Me.Label43 = New System.Windows.Forms.Label()
        Me.lblPowerUnitManovr = New System.Windows.Forms.Label()
        Me.txtDCIntervalManovr = New Bargh_Common.TextBoxPersian()
        Me.Label48 = New System.Windows.Forms.Label()
        Me.Label109 = New System.Windows.Forms.Label()
        Me.Label110 = New System.Windows.Forms.Label()
        Me.chkIsNotNeedManovrDC = New System.Windows.Forms.CheckBox()
        Me.tbcDCs = New System.Windows.Forms.TabControl()
        Me.tpDCMon = New System.Windows.Forms.TabPage()
        Me.tpDCMain = New System.Windows.Forms.TabPage()
        Me.lblLetterInfo = New System.Windows.Forms.LinkLabel()
        Me.pnlManoeuvre = New Bargh_Common.PanelGroupBox()
        Me.lnkManoeuvreDesc = New System.Windows.Forms.LinkLabel()
        Me.pnlManoeuvrePostFeeder = New System.Windows.Forms.Panel()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.cmbManoeuvreType = New Bargh_Common.ComboBoxPersian()
        Me.chkIsManoeuvre = New System.Windows.Forms.CheckBox()
        Me.pnlAdsress = New Bargh_Common.PanelGroupBox()
        Me.Label47 = New System.Windows.Forms.Label()
        Me.BracketPanel1 = New Bargh_Common.BracketPanel()
        Me.txtWorkingAddress = New System.Windows.Forms.TextBox()
        Me.rbNotInCityService = New System.Windows.Forms.RadioButton()
        Me.rbInCityService = New System.Windows.Forms.RadioButton()
        Me.pnlInfo = New Bargh_Common.PanelGroupBox()
        Me.txtPeymankarSearch = New Bargh_Common.TextBoxPersian()
        Me.cmbWeather = New Bargh_Common.ComboBoxPersian()
        Me.Label36 = New System.Windows.Forms.Label()
        Me.cmbPeymankar = New Bargh_Common.ComboBoxPersian()
        Me.lblDepartment = New System.Windows.Forms.Label()
        Me.cmbDepartment = New Bargh_Common.ComboBoxPersian()
        Me.cmbReferTo = New Bargh_Common.ComboBoxPersian()
        Me.Label73 = New System.Windows.Forms.Label()
        Me.pnlRequestedBy = New System.Windows.Forms.Panel()
        Me.rbIsRequestByPeymankar = New System.Windows.Forms.RadioButton()
        Me.rbIsRequestByTozi = New System.Windows.Forms.RadioButton()
        Me.Label71 = New System.Windows.Forms.Label()
        Me.rbIsRequestByFogheTozi = New System.Windows.Forms.RadioButton()
        Me.HatchPanel5 = New Bargh_Common.HatchPanel()
        Me.picPeymankarSearch = New System.Windows.Forms.PictureBox()
        Me.pnlManoeuvreDesc = New System.Windows.Forms.Panel()
        Me.txtManoeuvreDesc = New Bargh_Common.TextBoxPersian()
        Me.picOKManoeuvreDesc = New System.Windows.Forms.PictureBox()
        Me.pnlLetterInfo = New System.Windows.Forms.Panel()
        Me.Label51 = New System.Windows.Forms.Label()
        Me.txtLetterDate = New Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor()
        Me.txtLetterNo = New Bargh_Common.TextBoxPersian()
        Me.btnHidePnlLetter = New System.Windows.Forms.PictureBox()
        Me.Label107 = New System.Windows.Forms.Label()
        Me.pnlMultiStepInfo = New Bargh_Common.PanelGroupBox()
        Me.HatchPanel6 = New Bargh_Common.HatchPanel()
        Me.btnMSOK = New System.Windows.Forms.Button()
        Me.btnMSCancel = New System.Windows.Forms.Button()
        Me.pnlLevel6 = New System.Windows.Forms.Panel()
        Me.txtMSDesc6 = New System.Windows.Forms.TextBox()
        Me.Label91 = New System.Windows.Forms.Label()
        Me.Label92 = New System.Windows.Forms.Label()
        Me.txtMSBar6 = New Bargh_Common.TextBoxPersian()
        Me.txtMSTime6 = New Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor()
        Me.Label93 = New System.Windows.Forms.Label()
        Me.Label94 = New System.Windows.Forms.Label()
        Me.Label105 = New System.Windows.Forms.Label()
        Me.txtMSDate6 = New Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor()
        Me.btnFeederKey6 = New System.Windows.Forms.Button()
        Me.pnlLevel5 = New System.Windows.Forms.Panel()
        Me.txtMSDesc5 = New System.Windows.Forms.TextBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label77 = New System.Windows.Forms.Label()
        Me.txtMSBar5 = New Bargh_Common.TextBoxPersian()
        Me.txtMSTime5 = New Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor()
        Me.Label78 = New System.Windows.Forms.Label()
        Me.Label79 = New System.Windows.Forms.Label()
        Me.Label104 = New System.Windows.Forms.Label()
        Me.txtMSDate5 = New Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor()
        Me.btnFeederKey5 = New System.Windows.Forms.Button()
        Me.pnlLevel4 = New System.Windows.Forms.Panel()
        Me.txtMSDesc4 = New System.Windows.Forms.TextBox()
        Me.Label80 = New System.Windows.Forms.Label()
        Me.Label81 = New System.Windows.Forms.Label()
        Me.txtMSBar4 = New Bargh_Common.TextBoxPersian()
        Me.txtMSTime4 = New Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor()
        Me.Label82 = New System.Windows.Forms.Label()
        Me.Label83 = New System.Windows.Forms.Label()
        Me.Label103 = New System.Windows.Forms.Label()
        Me.txtMSDate4 = New Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor()
        Me.btnFeederKey4 = New System.Windows.Forms.Button()
        Me.pnlLevel3 = New System.Windows.Forms.Panel()
        Me.txtMSDesc3 = New System.Windows.Forms.TextBox()
        Me.Label84 = New System.Windows.Forms.Label()
        Me.Label85 = New System.Windows.Forms.Label()
        Me.txtMSBar3 = New Bargh_Common.TextBoxPersian()
        Me.txtMSTime3 = New Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor()
        Me.Label86 = New System.Windows.Forms.Label()
        Me.Label87 = New System.Windows.Forms.Label()
        Me.Label102 = New System.Windows.Forms.Label()
        Me.txtMSDate3 = New Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor()
        Me.btnFeederKey3 = New System.Windows.Forms.Button()
        Me.pnlLevel2 = New System.Windows.Forms.Panel()
        Me.txtMSDesc2 = New System.Windows.Forms.TextBox()
        Me.Label88 = New System.Windows.Forms.Label()
        Me.Label89 = New System.Windows.Forms.Label()
        Me.txtMSBar2 = New Bargh_Common.TextBoxPersian()
        Me.txtMSTime2 = New Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor()
        Me.Label90 = New System.Windows.Forms.Label()
        Me.Label95 = New System.Windows.Forms.Label()
        Me.Label101 = New System.Windows.Forms.Label()
        Me.txtMSDate2 = New Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor()
        Me.btnFeederKey2 = New System.Windows.Forms.Button()
        Me.pnlLevel1 = New System.Windows.Forms.Panel()
        Me.btnFeederKey1 = New System.Windows.Forms.Button()
        Me.txtMSDate1 = New Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor()
        Me.txtMSDesc1 = New System.Windows.Forms.TextBox()
        Me.Label96 = New System.Windows.Forms.Label()
        Me.Label97 = New System.Windows.Forms.Label()
        Me.txtMSBar1 = New Bargh_Common.TextBoxPersian()
        Me.txtMSTime1 = New Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor()
        Me.Label98 = New System.Windows.Forms.Label()
        Me.Label99 = New System.Windows.Forms.Label()
        Me.Label100 = New System.Windows.Forms.Label()
        Me.HatchPanel7 = New Bargh_Common.HatchPanel()
        Me.HatchPanel8 = New Bargh_Common.HatchPanel()
        Me.HatchPanel9 = New Bargh_Common.HatchPanel()
        Me.HatchPanel10 = New Bargh_Common.HatchPanel()
        Me.HatchPanel11 = New Bargh_Common.HatchPanel()
        Me.Label106 = New System.Windows.Forms.Label()
        Me.txtDCCurrent = New Bargh_Common.TextBoxPersian()
        Me.HatchPanel12 = New Bargh_Common.HatchPanel()
        Me.tp1_Specs = New System.Windows.Forms.TabPage()
        Me.txtNazerSearch = New Bargh_Common.TextBoxPersian()
        Me.pnlTamirRequestSubject = New System.Windows.Forms.Panel()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cmbTamirRequestSubject = New Bargh_Common.ComboBoxPersian()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.cmbState = New Bargh_Common.ComboBoxPersian()
        Me.pnlTamirType = New System.Windows.Forms.Panel()
        Me.Label76 = New System.Windows.Forms.Label()
        Me.cmbTamirType = New Bargh_Common.ComboBoxPersian()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.chkIsIRIB = New System.Windows.Forms.CheckBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtSMSCOunt = New System.Windows.Forms.TextBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.pnlEmergencyReason = New System.Windows.Forms.Panel()
        Me.lblEmergencyReason = New System.Windows.Forms.Label()
        Me.txtEmergency = New System.Windows.Forms.TextBox()
        Me.lnkRequestNumberManovr = New System.Windows.Forms.LinkLabel()
        Me.lblRequestNumberManovr = New System.Windows.Forms.Label()
        Me.txtEmergencyReasonDisplay = New System.Windows.Forms.TextBox()
        Me.pnlNazer = New System.Windows.Forms.Panel()
        Me.picNazerSearch = New System.Windows.Forms.PictureBox()
        Me.Label50 = New System.Windows.Forms.Label()
        Me.cmbNazer = New Bargh_Common.ComboBoxPersian()
        Me.btnSaveNazer = New System.Windows.Forms.Button()
        Me.pnlWorkCommandNo = New System.Windows.Forms.Panel()
        Me.lblWorkCommandNo = New System.Windows.Forms.Label()
        Me.txtWorkCommandNo = New Bargh_Common.TextBoxPersian()
        Me.lblGISFormNo = New System.Windows.Forms.Label()
        Me.txtGISFormNo = New Bargh_Common.TextBoxPersian()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.Label75 = New System.Windows.Forms.Label()
        Me.txtProjectNo = New Bargh_Common.TextBoxPersian()
        Me.chkIsConfirmNazer = New System.Windows.Forms.CheckBox()
        Me.chkIsSendToSetad = New System.Windows.Forms.CheckBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.pnlAreaCity = New System.Windows.Forms.Panel()
        Me.chkIsEmergency = New System.Windows.Forms.CheckBox()
        Me.pnlEmergency = New System.Windows.Forms.Panel()
        Me.Label64 = New System.Windows.Forms.Label()
        Me.txtEmergencyReason = New System.Windows.Forms.TextBox()
        Me.lblEmergency = New System.Windows.Forms.TextBox()
        Me.btnEmergencyOK = New System.Windows.Forms.Button()
        Me.Label65 = New System.Windows.Forms.Label()
        Me.btnEmergencyCancel = New System.Windows.Forms.Button()
        Me.cmnuFeederKey = New System.Windows.Forms.ContextMenu()
        Me.cmnuFeederKeyOnDisconnect = New System.Windows.Forms.MenuItem()
        Me.cmnuFeederKeyOnConnect = New System.Windows.Forms.MenuItem()
        Me.pnlMPFeederKey = New Bargh_Common.PanelGroupBox()
        Me.dgFeederKey = New Bargh_Common.JGrid()
        Me.chkIsRemoteChange = New System.Windows.Forms.CheckBox()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.btnReturnFeederKey = New System.Windows.Forms.Button()
        Me.pnlMPFeederKeyFilter = New System.Windows.Forms.Panel()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.txtMPFeederKeyGIS = New System.Windows.Forms.TextBox()
        Me.txtMPFeederKey = New System.Windows.Forms.TextBox()
        Me.Label118 = New System.Windows.Forms.Label()
        Me.Label114 = New System.Windows.Forms.Label()
        Me.cmbMPCloserType = New Bargh_Common.ComboBoxPersian()
        Me.Label115 = New System.Windows.Forms.Label()
        Me.errpro = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.CommonDataSet1 = New Bargh_Common.CommonDataSet()
        Me.ftDocs = New Bargh_BaseTables.FileTransfer()
        Me.pnlNumbers.SuspendLayout()
        CType(Me.DatasetBT1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlPostFeeder.SuspendLayout()
        CType(Me.picSearchMPPOverlaps, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DatasetDummy1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlLPPostFeederPart.SuspendLayout()
        CType(Me.picSearchMPFOverlaps, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlSOverlaps.SuspendLayout()
        CType(Me.dgSOverlaps, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlTamirRequestInfo.SuspendLayout()
        Me.pnlOverlaps.SuspendLayout()
        CType(Me.dgOverlaps, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlConfirmInfo.SuspendLayout()
        Me.pnlConfirm.SuspendLayout()
        CType(Me.PicSendSMSSensitive, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picLock, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlAllow.SuspendLayout()
        CType(Me.picIsAuotoNumber, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tbcMain.SuspendLayout()
        Me.tp6_Docs.SuspendLayout()
        CType(Me.dgFiles, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DatasetTamir1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpNewFile.SuspendLayout()
        Me.tp5_nDCFeeder.SuspendLayout()
        Me.pnlDate.SuspendLayout()
        Me.pnlToDate.SuspendLayout()
        Me.pnlNetworkType.SuspendLayout()
        Me.pnlDCDayCount.SuspendLayout()
        Me.pnlLastDCCount.SuspendLayout()
        CType(Me.dgFeederDC, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.mDsReqView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tp4_Confirms.SuspendLayout()
        Me.pnlWarmLineConfirm.SuspendLayout()
        Me.tp3_OperationList.SuspendLayout()
        CType(Me.dgOperations, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DatasetTamirView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tp2_DCLocation.SuspendLayout()
        CType(Me.picFeederPart, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlPeymankar.SuspendLayout()
        Me.pnlManoeuvreRequestInfo.SuspendLayout()
        Me.tbcDCs.SuspendLayout()
        Me.pnlManoeuvre.SuspendLayout()
        Me.pnlManoeuvrePostFeeder.SuspendLayout()
        Me.pnlAdsress.SuspendLayout()
        Me.pnlInfo.SuspendLayout()
        Me.pnlRequestedBy.SuspendLayout()
        CType(Me.picPeymankarSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlManoeuvreDesc.SuspendLayout()
        CType(Me.picOKManoeuvreDesc, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlLetterInfo.SuspendLayout()
        CType(Me.btnHidePnlLetter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlMultiStepInfo.SuspendLayout()
        Me.pnlLevel6.SuspendLayout()
        Me.pnlLevel5.SuspendLayout()
        Me.pnlLevel4.SuspendLayout()
        Me.pnlLevel3.SuspendLayout()
        Me.pnlLevel2.SuspendLayout()
        Me.pnlLevel1.SuspendLayout()
        Me.tp1_Specs.SuspendLayout()
        Me.pnlTamirRequestSubject.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.pnlTamirType.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.pnlEmergencyReason.SuspendLayout()
        Me.pnlNazer.SuspendLayout()
        CType(Me.picNazerSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlWorkCommandNo.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.pnlAreaCity.SuspendLayout()
        Me.pnlEmergency.SuspendLayout()
        Me.pnlMPFeederKey.SuspendLayout()
        CType(Me.dgFeederKey, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlMPFeederKeyFilter.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.errpro, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CommonDataSet1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'HelpMaker
        '
        Me.HelpMaker.HelpNamespace = "ReportsHelp.chm"
        '
        'pnlNumbers
        '
        Me.pnlNumbers.Controls.Add(Me.txtPowerTotal)
        Me.pnlNumbers.Controls.Add(Me.lblWeakPower1)
        Me.pnlNumbers.Controls.Add(Me.Label7)
        Me.pnlNumbers.Controls.Add(Me.txtMaxPower)
        Me.pnlNumbers.Controls.Add(Me.lblWeakPower2)
        Me.pnlNumbers.Controls.Add(Me.txtUsedPower)
        Me.pnlNumbers.Controls.Add(Me.Label21)
        Me.pnlNumbers.Controls.Add(Me.lblWeakPower5)
        Me.pnlNumbers.Controls.Add(Me.txtLastUsedPower)
        Me.pnlNumbers.Controls.Add(Me.Label42)
        Me.pnlNumbers.Controls.Add(Me.lblWeakPower3)
        Me.pnlNumbers.Controls.Add(Me.txtWaitPower)
        Me.pnlNumbers.Controls.Add(Me.Label44)
        Me.pnlNumbers.Controls.Add(Me.lblWeakPower6)
        Me.pnlNumbers.Controls.Add(Me.txtRemainPower)
        Me.pnlNumbers.Controls.Add(Me.Label46)
        Me.pnlNumbers.Controls.Add(Me.txtDCWantedPower)
        Me.pnlNumbers.Controls.Add(Me.lblWeakPower4)
        Me.pnlNumbers.Controls.Add(Me.Label49)
        Me.pnlNumbers.Controls.Add(Me.Label1)
        Me.pnlNumbers.Controls.Add(Me.Label2)
        Me.pnlNumbers.Location = New System.Drawing.Point(36, 125)
        Me.pnlNumbers.Name = "pnlNumbers"
        Me.pnlNumbers.Size = New System.Drawing.Size(656, 111)
        Me.pnlNumbers.TabIndex = 3
        '
        'txtPowerTotal
        '
        Me.txtPowerTotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPowerTotal.BackColor = System.Drawing.SystemColors.Control
        Me.txtPowerTotal.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtPowerTotal.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.txtPowerTotal.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.txtPowerTotal.Location = New System.Drawing.Point(206, 7)
        Me.txtPowerTotal.Name = "txtPowerTotal"
        Me.txtPowerTotal.ReadOnly = True
        Me.txtPowerTotal.Size = New System.Drawing.Size(72, 15)
        Me.txtPowerTotal.TabIndex = 0
        Me.txtPowerTotal.Text = "1.65"
        Me.txtPowerTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblWeakPower1
        '
        Me.lblWeakPower1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblWeakPower1.AutoSize = True
        Me.lblWeakPower1.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblWeakPower1.Location = New System.Drawing.Point(488, 33)
        Me.lblWeakPower1.Name = "lblWeakPower1"
        Me.lblWeakPower1.Size = New System.Drawing.Size(116, 12)
        Me.lblWeakPower1.TabIndex = 0
        Me.lblWeakPower1.Text = "سهميه خاموشي هفته"
        '
        'Label7
        '
        Me.Label7.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label7.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label7.Location = New System.Drawing.Point(376, 35)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(28, 10)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "MWh"
        '
        'txtMaxPower
        '
        Me.txtMaxPower.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMaxPower.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMaxPower.Location = New System.Drawing.Point(408, 30)
        Me.txtMaxPower.Name = "txtMaxPower"
        Me.txtMaxPower.ReadOnly = True
        Me.txtMaxPower.Size = New System.Drawing.Size(72, 21)
        Me.txtMaxPower.TabIndex = 0
        Me.txtMaxPower.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblWeakPower2
        '
        Me.lblWeakPower2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblWeakPower2.AutoSize = True
        Me.lblWeakPower2.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblWeakPower2.Location = New System.Drawing.Point(488, 60)
        Me.lblWeakPower2.Name = "lblWeakPower2"
        Me.lblWeakPower2.Size = New System.Drawing.Size(132, 12)
        Me.lblWeakPower2.TabIndex = 0
        Me.lblWeakPower2.Text = "سهميه استفاده شده هفته"
        '
        'txtUsedPower
        '
        Me.txtUsedPower.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtUsedPower.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtUsedPower.Location = New System.Drawing.Point(408, 57)
        Me.txtUsedPower.Name = "txtUsedPower"
        Me.txtUsedPower.ReadOnly = True
        Me.txtUsedPower.Size = New System.Drawing.Size(72, 21)
        Me.txtUsedPower.TabIndex = 1
        Me.txtUsedPower.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label21
        '
        Me.Label21.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label21.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label21.Location = New System.Drawing.Point(376, 62)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(28, 10)
        Me.Label21.TabIndex = 0
        Me.Label21.Text = "MWh"
        '
        'lblWeakPower5
        '
        Me.lblWeakPower5.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblWeakPower5.AutoSize = True
        Me.lblWeakPower5.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblWeakPower5.Location = New System.Drawing.Point(144, 60)
        Me.lblWeakPower5.Name = "lblWeakPower5"
        Me.lblWeakPower5.Size = New System.Drawing.Size(186, 12)
        Me.lblWeakPower5.TabIndex = 0
        Me.lblWeakPower5.Text = "سهميه استفاده شده مازاد از هفته قبل"
        Me.lblWeakPower5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtLastUsedPower
        '
        Me.txtLastUsedPower.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLastUsedPower.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtLastUsedPower.Location = New System.Drawing.Point(64, 57)
        Me.txtLastUsedPower.Name = "txtLastUsedPower"
        Me.txtLastUsedPower.ReadOnly = True
        Me.txtLastUsedPower.Size = New System.Drawing.Size(72, 21)
        Me.txtLastUsedPower.TabIndex = 2
        Me.txtLastUsedPower.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label42
        '
        Me.Label42.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label42.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label42.Location = New System.Drawing.Point(32, 62)
        Me.Label42.Name = "Label42"
        Me.Label42.Size = New System.Drawing.Size(28, 10)
        Me.Label42.TabIndex = 0
        Me.Label42.Text = "MWh"
        '
        'lblWeakPower3
        '
        Me.lblWeakPower3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblWeakPower3.AutoSize = True
        Me.lblWeakPower3.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblWeakPower3.Location = New System.Drawing.Point(488, 87)
        Me.lblWeakPower3.Name = "lblWeakPower3"
        Me.lblWeakPower3.Size = New System.Drawing.Size(163, 12)
        Me.lblWeakPower3.TabIndex = 0
        Me.lblWeakPower3.Text = "خاموشي‌هاي در انتظار تأييد هفته"
        '
        'txtWaitPower
        '
        Me.txtWaitPower.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtWaitPower.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtWaitPower.Location = New System.Drawing.Point(408, 84)
        Me.txtWaitPower.Name = "txtWaitPower"
        Me.txtWaitPower.ReadOnly = True
        Me.txtWaitPower.Size = New System.Drawing.Size(72, 21)
        Me.txtWaitPower.TabIndex = 3
        Me.txtWaitPower.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label44
        '
        Me.Label44.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label44.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label44.Location = New System.Drawing.Point(376, 89)
        Me.Label44.Name = "Label44"
        Me.Label44.Size = New System.Drawing.Size(28, 10)
        Me.Label44.TabIndex = 0
        Me.Label44.Text = "MWh"
        '
        'lblWeakPower6
        '
        Me.lblWeakPower6.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblWeakPower6.AutoSize = True
        Me.lblWeakPower6.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblWeakPower6.Location = New System.Drawing.Point(144, 87)
        Me.lblWeakPower6.Name = "lblWeakPower6"
        Me.lblWeakPower6.Size = New System.Drawing.Size(184, 12)
        Me.lblWeakPower6.TabIndex = 0
        Me.lblWeakPower6.Text = "باقيمانده سهميه با احتساب در انتظارها"
        Me.lblWeakPower6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtRemainPower
        '
        Me.txtRemainPower.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtRemainPower.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtRemainPower.Location = New System.Drawing.Point(64, 84)
        Me.txtRemainPower.Name = "txtRemainPower"
        Me.txtRemainPower.ReadOnly = True
        Me.txtRemainPower.Size = New System.Drawing.Size(72, 21)
        Me.txtRemainPower.TabIndex = 4
        Me.txtRemainPower.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label46
        '
        Me.Label46.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label46.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label46.Location = New System.Drawing.Point(32, 89)
        Me.Label46.Name = "Label46"
        Me.Label46.Size = New System.Drawing.Size(28, 10)
        Me.Label46.TabIndex = 0
        Me.Label46.Text = "MWh"
        '
        'txtDCWantedPower
        '
        Me.txtDCWantedPower.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDCWantedPower.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDCWantedPower.Location = New System.Drawing.Point(64, 30)
        Me.txtDCWantedPower.Name = "txtDCWantedPower"
        Me.txtDCWantedPower.ReadOnly = True
        Me.txtDCWantedPower.Size = New System.Drawing.Size(72, 21)
        Me.txtDCWantedPower.TabIndex = 2
        Me.txtDCWantedPower.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblWeakPower4
        '
        Me.lblWeakPower4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblWeakPower4.AutoSize = True
        Me.lblWeakPower4.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblWeakPower4.Location = New System.Drawing.Point(144, 33)
        Me.lblWeakPower4.Name = "lblWeakPower4"
        Me.lblWeakPower4.Size = New System.Drawing.Size(152, 12)
        Me.lblWeakPower4.TabIndex = 0
        Me.lblWeakPower4.Text = "خاموشي‌هاي بدون مجوز هفته"
        Me.lblWeakPower4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label49
        '
        Me.Label49.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label49.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label49.Location = New System.Drawing.Point(32, 35)
        Me.Label49.Name = "Label49"
        Me.Label49.Size = New System.Drawing.Size(28, 10)
        Me.Label49.TabIndex = 0
        Me.Label49.Text = "MWh"
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label1.Location = New System.Drawing.Point(177, 11)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(28, 10)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "MWh"
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label2.Location = New System.Drawing.Point(274, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(215, 12)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "ميزان انرژي توزيع نشده براي درخواست جاري"
        '
        'lnkRequestNumber
        '
        Me.lnkRequestNumber.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lnkRequestNumber.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lnkRequestNumber.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.lnkRequestNumber.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lnkRequestNumber.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.lnkRequestNumber.Location = New System.Drawing.Point(296, 34)
        Me.lnkRequestNumber.Name = "lnkRequestNumber"
        Me.lnkRequestNumber.Size = New System.Drawing.Size(144, 16)
        Me.lnkRequestNumber.TabIndex = 1
        Me.lnkRequestNumber.TabStop = True
        Me.lnkRequestNumber.Text = "مشخص نشده"
        Me.lnkRequestNumber.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtTamirRequestNumber
        '
        Me.txtTamirRequestNumber.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTamirRequestNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTamirRequestNumber.Location = New System.Drawing.Point(368, 5)
        Me.txtTamirRequestNumber.Name = "txtTamirRequestNumber"
        Me.txtTamirRequestNumber.ReadOnly = True
        Me.txtTamirRequestNumber.Size = New System.Drawing.Size(144, 21)
        Me.txtTamirRequestNumber.TabIndex = 0
        Me.txtTamirRequestNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblTamirRequestNo
        '
        Me.lblTamirRequestNo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTamirRequestNo.AutoSize = True
        Me.lblTamirRequestNo.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblTamirRequestNo.Location = New System.Drawing.Point(520, 7)
        Me.lblTamirRequestNo.Name = "lblTamirRequestNo"
        Me.lblTamirRequestNo.Size = New System.Drawing.Size(96, 13)
        Me.lblTamirRequestNo.TabIndex = 0
        Me.lblTamirRequestNo.Text = "شماره درخواست"
        '
        'lblRequestNumber
        '
        Me.lblRequestNumber.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblRequestNumber.AutoSize = True
        Me.lblRequestNumber.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblRequestNumber.Location = New System.Drawing.Point(448, 34)
        Me.lblRequestNumber.Name = "lblRequestNumber"
        Me.lblRequestNumber.Size = New System.Drawing.Size(129, 13)
        Me.lblRequestNumber.TabIndex = 0
        Me.lblRequestNumber.Text = "شماره پرونده خاموشي"
        '
        'cmbArea
        '
        Me.cmbArea.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbArea.BackColor = System.Drawing.Color.White
        Me.cmbArea.DataSource = Me.DatasetBT1.Tbl_Area
        Me.cmbArea.DisplayMember = "Area"
        Me.cmbArea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbArea.IsReadOnly = False
        Me.cmbArea.Location = New System.Drawing.Point(229, 8)
        Me.cmbArea.Name = "cmbArea"
        Me.cmbArea.Size = New System.Drawing.Size(144, 21)
        Me.cmbArea.TabIndex = 0
        Me.cmbArea.ValueMember = "AreaId"
        '
        'DatasetBT1
        '
        Me.DatasetBT1.DataSetName = "DatasetBT"
        Me.DatasetBT1.Locale = New System.Globalization.CultureInfo("en-US")
        Me.DatasetBT1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label4.Location = New System.Drawing.Point(378, 10)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(34, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "ناحيه"
        '
        'cmbCity
        '
        Me.cmbCity.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbCity.DataSource = Me.DatasetBT1.Tbl_City
        Me.cmbCity.DisplayMember = "City"
        Me.cmbCity.Location = New System.Drawing.Point(10, 8)
        Me.cmbCity.Name = "cmbCity"
        Me.cmbCity.Size = New System.Drawing.Size(144, 21)
        Me.cmbCity.TabIndex = 1
        Me.cmbCity.ValueMember = "CityId"
        '
        'Label5
        '
        Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label5.Location = New System.Drawing.Point(162, 10)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(60, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "شهرستان"
        '
        'pnlPostFeeder
        '
        Me.pnlPostFeeder.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlPostFeeder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlPostFeeder.CaptionBackColor = System.Drawing.Color.Teal
        Me.pnlPostFeeder.CaptionFont = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.pnlPostFeeder.CaptionForeColor = System.Drawing.Color.White
        Me.pnlPostFeeder.CaptionHeight = 16
        Me.pnlPostFeeder.CaptionText = "مشخصات محل قطع"
        Me.pnlPostFeeder.Controls.Add(Me.chkMPPostTrans)
        Me.pnlPostFeeder.Controls.Add(Me.btnFeederKey0)
        Me.pnlPostFeeder.Controls.Add(Me.chkIsRemoteDCChange)
        Me.pnlPostFeeder.Controls.Add(Me.picSearchMPPOverlaps)
        Me.pnlPostFeeder.Controls.Add(Me.btnMultiStep)
        Me.pnlPostFeeder.Controls.Add(Me.txtGpsX)
        Me.pnlPostFeeder.Controls.Add(Me.txtGpsY)
        Me.pnlPostFeeder.Controls.Add(Me.chkIsLPPostFeederPart)
        Me.pnlPostFeeder.Controls.Add(Me.cmbMPPost)
        Me.pnlPostFeeder.Controls.Add(Me.lblMPPostTrans)
        Me.pnlPostFeeder.Controls.Add(Me.cmbMPFeeder)
        Me.pnlPostFeeder.Controls.Add(Me.Label9)
        Me.pnlPostFeeder.Controls.Add(Me.txtLastPeakBar)
        Me.pnlPostFeeder.Controls.Add(Me.cmbCloserType)
        Me.pnlPostFeeder.Controls.Add(Me.Label25)
        Me.pnlPostFeeder.Controls.Add(Me.lblFeederPart)
        Me.pnlPostFeeder.Controls.Add(Me.cmbLPPost)
        Me.pnlPostFeeder.Controls.Add(Me.btnSelectLPPost)
        Me.pnlPostFeeder.Controls.Add(Me.cmbFeederPart)
        Me.pnlPostFeeder.Controls.Add(Me.lblLPPost)
        Me.pnlPostFeeder.Controls.Add(Me.pnlLPPostFeederPart)
        Me.pnlPostFeeder.Controls.Add(Me.lblPeakBar)
        Me.pnlPostFeeder.Controls.Add(Me.Label59)
        Me.pnlPostFeeder.Controls.Add(Me.txtLPPostCountDC)
        Me.pnlPostFeeder.Controls.Add(Me.Label68)
        Me.pnlPostFeeder.Controls.Add(Me.Label69)
        Me.pnlPostFeeder.Controls.Add(Me.Label70)
        Me.pnlPostFeeder.Controls.Add(Me.cmbLPFeeder)
        Me.pnlPostFeeder.Controls.Add(Me.Label72)
        Me.pnlPostFeeder.Controls.Add(Me.Label8)
        Me.pnlPostFeeder.Controls.Add(Me.cmbTamirNetworkType)
        Me.pnlPostFeeder.Controls.Add(Me.Label74)
        Me.pnlPostFeeder.Controls.Add(Me.ckcmbMPFeeder)
        Me.pnlPostFeeder.Controls.Add(Me.picSearchMPFOverlaps)
        Me.pnlPostFeeder.Controls.Add(Me.pnlSOverlaps)
        Me.pnlPostFeeder.IsMoveable = False
        Me.pnlPostFeeder.IsWindowMove = False
        Me.pnlPostFeeder.Location = New System.Drawing.Point(312, 254)
        Me.pnlPostFeeder.Name = "pnlPostFeeder"
        Me.pnlPostFeeder.Size = New System.Drawing.Size(408, 229)
        Me.pnlPostFeeder.TabIndex = 2
        '
        'chkMPPostTrans
        '
        Me.chkMPPostTrans.CheckComboDropDownWidth = 0
        Me.chkMPPostTrans.CheckGroup = CType(resources.GetObject("chkMPPostTrans.CheckGroup"), System.Collections.ArrayList)
        Me.chkMPPostTrans.DropDownDirection = UtilityLibrary.Combos.DropDownDirection.Down
        Me.chkMPPostTrans.DropHeight = 500
        Me.chkMPPostTrans.IsGroup = False
        Me.chkMPPostTrans.IsMultiSelect = True
        Me.chkMPPostTrans.Location = New System.Drawing.Point(120, 70)
        Me.chkMPPostTrans.Name = "chkMPPostTrans"
        Me.chkMPPostTrans.ReadOnlyList = ""
        Me.chkMPPostTrans.Size = New System.Drawing.Size(168, 21)
        Me.chkMPPostTrans.TabIndex = 190
        Me.chkMPPostTrans.Text = "ChkCombo1"
        Me.chkMPPostTrans.TreeImageList = Nothing
        '
        'btnFeederKey0
        '
        Me.btnFeederKey0.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFeederKey0.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnFeederKey0.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.btnFeederKey0.Location = New System.Drawing.Point(4, 19)
        Me.btnFeederKey0.Name = "btnFeederKey0"
        Me.btnFeederKey0.Size = New System.Drawing.Size(48, 22)
        Me.btnFeederKey0.TabIndex = 188
        Me.btnFeederKey0.Tag = "وصل چند مرحله‌اي"
        Me.btnFeederKey0.Text = "کليدزني"
        Me.btnFeederKey0.Visible = False
        '
        'chkIsRemoteDCChange
        '
        Me.chkIsRemoteDCChange.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkIsRemoteDCChange.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.chkIsRemoteDCChange.Location = New System.Drawing.Point(3, 202)
        Me.chkIsRemoteDCChange.Name = "chkIsRemoteDCChange"
        Me.chkIsRemoteDCChange.Size = New System.Drawing.Size(101, 21)
        Me.chkIsRemoteDCChange.TabIndex = 187
        Me.chkIsRemoteDCChange.Tag = "999"
        Me.chkIsRemoteDCChange.Text = "قطع از راه دور"
        '
        'picSearchMPPOverlaps
        '
        Me.picSearchMPPOverlaps.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picSearchMPPOverlaps.Cursor = System.Windows.Forms.Cursors.Hand
        Me.picSearchMPPOverlaps.Image = CType(resources.GetObject("picSearchMPPOverlaps.Image"), System.Drawing.Image)
        Me.picSearchMPPOverlaps.Location = New System.Drawing.Point(96, 50)
        Me.picSearchMPPOverlaps.Name = "picSearchMPPOverlaps"
        Me.picSearchMPPOverlaps.Size = New System.Drawing.Size(16, 16)
        Me.picSearchMPPOverlaps.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picSearchMPPOverlaps.TabIndex = 186
        Me.picSearchMPPOverlaps.TabStop = False
        Me.GlobalToolTip.SetToolTip(Me.picSearchMPPOverlaps, "جستجوي خاموشي‌هاي همزمان فوق توزيع روي پست در بازه تاريخ مشخض شده")
        '
        'btnMultiStep
        '
        Me.btnMultiStep.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnMultiStep.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnMultiStep.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.btnMultiStep.Location = New System.Drawing.Point(53, 19)
        Me.btnMultiStep.Name = "btnMultiStep"
        Me.btnMultiStep.Size = New System.Drawing.Size(90, 22)
        Me.btnMultiStep.TabIndex = 185
        Me.btnMultiStep.Tag = "وصل چند مرحله‌اي"
        Me.btnMultiStep.Text = "وصل چند مرحله‌اي"
        '
        'txtGpsX
        '
        Me.txtGpsX.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtGpsX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtGpsX.CaptinText = ""
        Me.txtGpsX.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.txtGpsX.HasCaption = False
        Me.txtGpsX.IsForceText = False
        Me.txtGpsX.IsFractional = True
        Me.txtGpsX.IsIP = False
        Me.txtGpsX.IsNumberOnly = True
        Me.txtGpsX.IsYear = False
        Me.txtGpsX.Location = New System.Drawing.Point(4, 170)
        Me.txtGpsX.Name = "txtGpsX"
        Me.txtGpsX.Size = New System.Drawing.Size(45, 19)
        Me.txtGpsX.TabIndex = 9
        Me.txtGpsX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtGpsY
        '
        Me.txtGpsY.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtGpsY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtGpsY.CaptinText = ""
        Me.txtGpsY.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.txtGpsY.HasCaption = False
        Me.txtGpsY.IsForceText = False
        Me.txtGpsY.IsFractional = True
        Me.txtGpsY.IsIP = False
        Me.txtGpsY.IsNumberOnly = True
        Me.txtGpsY.IsYear = False
        Me.txtGpsY.Location = New System.Drawing.Point(49, 170)
        Me.txtGpsY.Name = "txtGpsY"
        Me.txtGpsY.Size = New System.Drawing.Size(45, 19)
        Me.txtGpsY.TabIndex = 180
        Me.txtGpsY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'chkIsLPPostFeederPart
        '
        Me.chkIsLPPostFeederPart.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkIsLPPostFeederPart.Location = New System.Drawing.Point(383, 136)
        Me.chkIsLPPostFeederPart.Name = "chkIsLPPostFeederPart"
        Me.chkIsLPPostFeederPart.Size = New System.Drawing.Size(16, 16)
        Me.chkIsLPPostFeederPart.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.chkIsLPPostFeederPart, "عدم قطع کل فيدر فشار متوسط")
        '
        'cmbMPPost
        '
        Me.cmbMPPost.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbMPPost.BackColor = System.Drawing.Color.White
        Me.cmbMPPost.DataBindings.Add(New System.Windows.Forms.Binding("SelectedValue", Me.DatasetDummy1, "ViewDummyId.Dummy01Id", True))
        Me.cmbMPPost.DataSource = Me.DatasetBT1.Tbl_MPPost
        Me.cmbMPPost.DisplayMember = "MPPostName"
        Me.cmbMPPost.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMPPost.IsReadOnly = False
        Me.cmbMPPost.Location = New System.Drawing.Point(120, 47)
        Me.cmbMPPost.Name = "cmbMPPost"
        Me.cmbMPPost.Size = New System.Drawing.Size(168, 21)
        Me.cmbMPPost.TabIndex = 2
        Me.cmbMPPost.ValueMember = "MPPostId"
        '
        'DatasetDummy1
        '
        Me.DatasetDummy1.DataSetName = "DatasetDummy"
        Me.DatasetDummy1.Locale = New System.Globalization.CultureInfo("en-US")
        Me.DatasetDummy1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'lblMPPostTrans
        '
        Me.lblMPPostTrans.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMPPostTrans.AutoSize = True
        Me.lblMPPostTrans.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblMPPostTrans.Location = New System.Drawing.Point(293, 74)
        Me.lblMPPostTrans.Name = "lblMPPostTrans"
        Me.lblMPPostTrans.Size = New System.Drawing.Size(96, 13)
        Me.lblMPPostTrans.TabIndex = 189
        Me.lblMPPostTrans.Text = "ترانس فوق توزيع"
        '
        'cmbMPFeeder
        '
        Me.cmbMPFeeder.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbMPFeeder.BackColor = System.Drawing.Color.White
        Me.cmbMPFeeder.DataBindings.Add(New System.Windows.Forms.Binding("SelectedValue", Me.DatasetDummy1, "ViewDummyId.Dummy02Id", True))
        Me.cmbMPFeeder.DataSource = Me.DatasetBT1.Tbl_MPFeeder
        Me.cmbMPFeeder.DisplayMember = "MPFeederName"
        Me.cmbMPFeeder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMPFeeder.IsReadOnly = False
        Me.cmbMPFeeder.Location = New System.Drawing.Point(120, 94)
        Me.cmbMPFeeder.Name = "cmbMPFeeder"
        Me.cmbMPFeeder.Size = New System.Drawing.Size(165, 21)
        Me.cmbMPFeeder.TabIndex = 3
        Me.cmbMPFeeder.ValueMember = "MPFeederId"
        '
        'Label9
        '
        Me.Label9.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label9.Location = New System.Drawing.Point(294, 98)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(100, 13)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "فيدر فشار متوسط"
        '
        'txtLastPeakBar
        '
        Me.txtLastPeakBar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLastPeakBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtLastPeakBar.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.txtLastPeakBar.Location = New System.Drawing.Point(19, 70)
        Me.txtLastPeakBar.Name = "txtLastPeakBar"
        Me.txtLastPeakBar.ReadOnly = True
        Me.txtLastPeakBar.Size = New System.Drawing.Size(56, 19)
        Me.txtLastPeakBar.TabIndex = 2
        Me.txtLastPeakBar.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.ToolTip1.SetToolTip(Me.txtLastPeakBar, "آخرين بار پيک فيدر")
        '
        'cmbCloserType
        '
        Me.cmbCloserType.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbCloserType.DataBindings.Add(New System.Windows.Forms.Binding("SelectedValue", Me.DatasetDummy1, "ViewDummyId.Dummy03Id", True))
        Me.cmbCloserType.DataSource = Me.DatasetBT1.Tbl_MPCloserType
        Me.cmbCloserType.DisplayMember = "MPCloserType"
        Me.cmbCloserType.Location = New System.Drawing.Point(120, 202)
        Me.cmbCloserType.Name = "cmbCloserType"
        Me.cmbCloserType.Size = New System.Drawing.Size(168, 21)
        Me.cmbCloserType.TabIndex = 7
        Me.cmbCloserType.ValueMember = "MPCloserTypeId"
        '
        'Label25
        '
        Me.Label25.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label25.AutoSize = True
        Me.Label25.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label25.Location = New System.Drawing.Point(294, 208)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(79, 13)
        Me.Label25.TabIndex = 0
        Me.Label25.Text = "نوع قطع کننده"
        '
        'lblFeederPart
        '
        Me.lblFeederPart.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFeederPart.AutoSize = True
        Me.lblFeederPart.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblFeederPart.Location = New System.Drawing.Point(294, 150)
        Me.lblFeederPart.Name = "lblFeederPart"
        Me.lblFeederPart.Size = New System.Drawing.Size(49, 13)
        Me.lblFeederPart.TabIndex = 0
        Me.lblFeederPart.Text = "تکه فيدر"
        '
        'cmbLPPost
        '
        Me.cmbLPPost.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbLPPost.BackColor = System.Drawing.Color.White
        Me.cmbLPPost.DataBindings.Add(New System.Windows.Forms.Binding("SelectedValue", Me.DatasetDummy1, "ViewDummyId.Dummy06Id", True))
        Me.cmbLPPost.DataSource = Me.DatasetBT1.Tbl_LPPost
        Me.cmbLPPost.DisplayMember = "LPPostName"
        Me.cmbLPPost.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbLPPost.DropDownWidth = 300
        Me.cmbLPPost.Enabled = False
        Me.cmbLPPost.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.cmbLPPost.IsReadOnly = False
        Me.cmbLPPost.Location = New System.Drawing.Point(120, 121)
        Me.cmbLPPost.Name = "cmbLPPost"
        Me.cmbLPPost.Size = New System.Drawing.Size(168, 21)
        Me.cmbLPPost.TabIndex = 4
        Me.cmbLPPost.ValueMember = "LPPostId"
        '
        'btnSelectLPPost
        '
        Me.btnSelectLPPost.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnSelectLPPost.Location = New System.Drawing.Point(94, 122)
        Me.btnSelectLPPost.Name = "btnSelectLPPost"
        Me.btnSelectLPPost.Size = New System.Drawing.Size(21, 20)
        Me.btnSelectLPPost.TabIndex = 6
        Me.btnSelectLPPost.Text = "..."
        '
        'cmbFeederPart
        '
        Me.cmbFeederPart.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbFeederPart.BackColor = System.Drawing.Color.White
        Me.cmbFeederPart.DataBindings.Add(New System.Windows.Forms.Binding("SelectedValue", Me.DatasetDummy1, "ViewDummyId.Dummy04Id", True))
        Me.cmbFeederPart.DataSource = Me.DatasetBT1.Tbl_FeederPart
        Me.cmbFeederPart.DisplayMember = "FeederPart"
        Me.cmbFeederPart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbFeederPart.DropDownWidth = 300
        Me.cmbFeederPart.Enabled = False
        Me.cmbFeederPart.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.cmbFeederPart.IsReadOnly = False
        Me.cmbFeederPart.Location = New System.Drawing.Point(120, 148)
        Me.cmbFeederPart.Name = "cmbFeederPart"
        Me.cmbFeederPart.Size = New System.Drawing.Size(168, 21)
        Me.cmbFeederPart.TabIndex = 5
        Me.cmbFeederPart.ValueMember = "FeederPartId"
        '
        'lblLPPost
        '
        Me.lblLPPost.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblLPPost.AutoSize = True
        Me.lblLPPost.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblLPPost.Location = New System.Drawing.Point(294, 123)
        Me.lblLPPost.Name = "lblLPPost"
        Me.lblLPPost.Size = New System.Drawing.Size(64, 13)
        Me.lblLPPost.TabIndex = 0
        Me.lblLPPost.Text = "پست توزيع"
        '
        'pnlLPPostFeederPart
        '
        Me.pnlLPPostFeederPart.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlLPPostFeederPart.BracketWidth = 5.0!
        Me.pnlLPPostFeederPart.Controls.Add(Me.rbLPPost)
        Me.pnlLPPostFeederPart.Controls.Add(Me.rbFeederPart)
        Me.pnlLPPostFeederPart.Direction = Bargh_Common.BracketPanel.Directions.Right
        Me.pnlLPPostFeederPart.Enabled = False
        Me.pnlLPPostFeederPart.LineColor = System.Drawing.Color.DarkBlue
        Me.pnlLPPostFeederPart.LineWidth = 1.0!
        Me.pnlLPPostFeederPart.Location = New System.Drawing.Point(354, 121)
        Me.pnlLPPostFeederPart.Name = "pnlLPPostFeederPart"
        Me.pnlLPPostFeederPart.Size = New System.Drawing.Size(26, 46)
        Me.pnlLPPostFeederPart.TabIndex = 4
        '
        'rbLPPost
        '
        Me.rbLPPost.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rbLPPost.Location = New System.Drawing.Point(0, 1)
        Me.rbLPPost.Name = "rbLPPost"
        Me.rbLPPost.Size = New System.Drawing.Size(16, 16)
        Me.rbLPPost.TabIndex = 0
        '
        'rbFeederPart
        '
        Me.rbFeederPart.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rbFeederPart.Location = New System.Drawing.Point(0, 27)
        Me.rbFeederPart.Name = "rbFeederPart"
        Me.rbFeederPart.Size = New System.Drawing.Size(16, 16)
        Me.rbFeederPart.TabIndex = 1
        '
        'lblPeakBar
        '
        Me.lblPeakBar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPeakBar.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblPeakBar.Location = New System.Drawing.Point(9, 44)
        Me.lblPeakBar.Name = "lblPeakBar"
        Me.lblPeakBar.Size = New System.Drawing.Size(76, 24)
        Me.lblPeakBar.TabIndex = 0
        Me.lblPeakBar.Text = "آخرين بار پيک فيدر فشار متوسط"
        Me.lblPeakBar.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label59
        '
        Me.Label59.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label59.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label59.Location = New System.Drawing.Point(7, 100)
        Me.Label59.Name = "Label59"
        Me.Label59.Size = New System.Drawing.Size(88, 24)
        Me.Label59.TabIndex = 0
        Me.Label59.Text = "تعداد پست توزيع قطع شونده"
        Me.Label59.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtLPPostCountDC
        '
        Me.txtLPPostCountDC.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLPPostCountDC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtLPPostCountDC.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.txtLPPostCountDC.Location = New System.Drawing.Point(19, 128)
        Me.txtLPPostCountDC.Name = "txtLPPostCountDC"
        Me.txtLPPostCountDC.ReadOnly = True
        Me.txtLPPostCountDC.Size = New System.Drawing.Size(56, 19)
        Me.txtLPPostCountDC.TabIndex = 2
        Me.txtLPPostCountDC.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.ToolTip1.SetToolTip(Me.txtLPPostCountDC, "آخرين بار پيک فيدر")
        '
        'Label68
        '
        Me.Label68.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label68.AutoSize = True
        Me.Label68.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label68.Location = New System.Drawing.Point(17, 158)
        Me.Label68.Name = "Label68"
        Me.Label68.Size = New System.Drawing.Size(13, 13)
        Me.Label68.TabIndex = 183
        Me.Label68.Text = "X"
        '
        'Label69
        '
        Me.Label69.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label69.AutoSize = True
        Me.Label69.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label69.Location = New System.Drawing.Point(69, 158)
        Me.Label69.Name = "Label69"
        Me.Label69.Size = New System.Drawing.Size(13, 13)
        Me.Label69.TabIndex = 182
        Me.Label69.Text = "Y"
        '
        'Label70
        '
        Me.Label70.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label70.AutoSize = True
        Me.Label70.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label70.Location = New System.Drawing.Point(36, 154)
        Me.Label70.Name = "Label70"
        Me.Label70.Size = New System.Drawing.Size(26, 13)
        Me.Label70.TabIndex = 183
        Me.Label70.Text = "GPS"
        '
        'cmbLPFeeder
        '
        Me.cmbLPFeeder.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbLPFeeder.BackColor = System.Drawing.Color.White
        Me.cmbLPFeeder.DataBindings.Add(New System.Windows.Forms.Binding("SelectedValue", Me.DatasetDummy1, "ViewDummyId.Dummy11Id", True))
        Me.cmbLPFeeder.DataSource = Me.DatasetBT1.Tbl_LPFeeder
        Me.cmbLPFeeder.DisplayMember = "LPFeederName"
        Me.cmbLPFeeder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbLPFeeder.Enabled = False
        Me.cmbLPFeeder.IsReadOnly = False
        Me.cmbLPFeeder.Location = New System.Drawing.Point(120, 177)
        Me.cmbLPFeeder.Name = "cmbLPFeeder"
        Me.cmbLPFeeder.Size = New System.Drawing.Size(168, 21)
        Me.cmbLPFeeder.TabIndex = 6
        Me.cmbLPFeeder.ValueMember = "LPFeederId"
        '
        'Label72
        '
        Me.Label72.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label72.AutoSize = True
        Me.Label72.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label72.Location = New System.Drawing.Point(294, 181)
        Me.Label72.Name = "Label72"
        Me.Label72.Size = New System.Drawing.Size(94, 13)
        Me.Label72.TabIndex = 0
        Me.Label72.Text = "فيدر فشار ضعيف"
        '
        'Label8
        '
        Me.Label8.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label8.Location = New System.Drawing.Point(294, 49)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(90, 13)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "پست فوق توزيع"
        '
        'cmbTamirNetworkType
        '
        Me.cmbTamirNetworkType.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbTamirNetworkType.BackColor = System.Drawing.Color.White
        Me.cmbTamirNetworkType.DataBindings.Add(New System.Windows.Forms.Binding("SelectedValue", Me.DatasetDummy1, "ViewDummyId.Dummy10Id", True))
        Me.cmbTamirNetworkType.DataSource = Me.DatasetBT1.Tbl_TamirNetworkType
        Me.cmbTamirNetworkType.DisplayMember = "TamirNetworkType"
        Me.cmbTamirNetworkType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTamirNetworkType.IsReadOnly = False
        Me.cmbTamirNetworkType.Location = New System.Drawing.Point(144, 21)
        Me.cmbTamirNetworkType.Name = "cmbTamirNetworkType"
        Me.cmbTamirNetworkType.Size = New System.Drawing.Size(144, 21)
        Me.cmbTamirNetworkType.TabIndex = 1
        Me.cmbTamirNetworkType.ValueMember = "TamirNetworkTypeId"
        '
        'Label74
        '
        Me.Label74.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label74.AutoSize = True
        Me.Label74.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label74.Location = New System.Drawing.Point(294, 24)
        Me.Label74.Name = "Label74"
        Me.Label74.Size = New System.Drawing.Size(58, 13)
        Me.Label74.TabIndex = 0
        Me.Label74.Text = "نوع شبکه"
        '
        'ckcmbMPFeeder
        '
        Me.ckcmbMPFeeder.CheckComboDropDownWidth = 0
        Me.ckcmbMPFeeder.CheckGroup = CType(resources.GetObject("ckcmbMPFeeder.CheckGroup"), System.Collections.ArrayList)
        Me.ckcmbMPFeeder.DropDownDirection = UtilityLibrary.Combos.DropDownDirection.Down
        Me.ckcmbMPFeeder.DropHeight = 500
        Me.ckcmbMPFeeder.IsGroup = False
        Me.ckcmbMPFeeder.IsMultiSelect = True
        Me.ckcmbMPFeeder.Location = New System.Drawing.Point(120, 94)
        Me.ckcmbMPFeeder.Name = "ckcmbMPFeeder"
        Me.ckcmbMPFeeder.ReadOnlyList = ""
        Me.ckcmbMPFeeder.Size = New System.Drawing.Size(168, 21)
        Me.ckcmbMPFeeder.TabIndex = 184
        Me.ckcmbMPFeeder.Text = "ChkCombo1"
        Me.ckcmbMPFeeder.TreeImageList = Nothing
        Me.ckcmbMPFeeder.Visible = False
        '
        'picSearchMPFOverlaps
        '
        Me.picSearchMPFOverlaps.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picSearchMPFOverlaps.Cursor = System.Windows.Forms.Cursors.Hand
        Me.picSearchMPFOverlaps.Image = CType(resources.GetObject("picSearchMPFOverlaps.Image"), System.Drawing.Image)
        Me.picSearchMPFOverlaps.Location = New System.Drawing.Point(96, 99)
        Me.picSearchMPFOverlaps.Name = "picSearchMPFOverlaps"
        Me.picSearchMPFOverlaps.Size = New System.Drawing.Size(16, 16)
        Me.picSearchMPFOverlaps.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picSearchMPFOverlaps.TabIndex = 186
        Me.picSearchMPFOverlaps.TabStop = False
        Me.GlobalToolTip.SetToolTip(Me.picSearchMPFOverlaps, "جستجوي خاموشي‌هاي همزمان فوق توزيع روي فيدر در بازه تاريخ مشخض شده")
        '
        'pnlSOverlaps
        '
        Me.pnlSOverlaps.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlSOverlaps.CaptionBackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.pnlSOverlaps.CaptionFont = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.pnlSOverlaps.CaptionForeColor = System.Drawing.Color.White
        Me.pnlSOverlaps.CaptionHeight = 16
        Me.pnlSOverlaps.CaptionText = "خاموشي‌هاي فوق توزيع همزمان يافت شده"
        Me.pnlSOverlaps.Controls.Add(Me.dgSOverlaps)
        Me.pnlSOverlaps.Controls.Add(Me.btnSCloseOverlaps)
        Me.pnlSOverlaps.IsMoveable = False
        Me.pnlSOverlaps.IsWindowMove = False
        Me.pnlSOverlaps.Location = New System.Drawing.Point(0, 1)
        Me.pnlSOverlaps.Name = "pnlSOverlaps"
        Me.pnlSOverlaps.Size = New System.Drawing.Size(406, 216)
        Me.pnlSOverlaps.TabIndex = 0
        Me.pnlSOverlaps.Visible = False
        '
        'dgSOverlaps
        '
        Me.dgSOverlaps.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgSOverlaps.BackColor = System.Drawing.SystemColors.Control
        Me.dgSOverlaps.BuiltInTextsData = resources.GetString("dgSOverlaps.BuiltInTextsData")
        Me.dgSOverlaps.CurrentRowIndex = -1
        dgSOverlaps_DesignTimeLayout.LayoutString = resources.GetString("dgSOverlaps_DesignTimeLayout.LayoutString")
        Me.dgSOverlaps.DesignTimeLayout = dgSOverlaps_DesignTimeLayout
        Me.dgSOverlaps.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular
        Me.dgSOverlaps.EnableFormatEvent = True
        Me.dgSOverlaps.EnableSaveLayout = True
        Me.dgSOverlaps.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.dgSOverlaps.GroupByBoxFormatStyle.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.dgSOverlaps.GroupByBoxInfoFormatStyle.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.dgSOverlaps.GroupByBoxVisible = False
        Me.dgSOverlaps.HeaderFormatStyle.LineAlignment = Janus.Windows.GridEX.TextAlignment.Center
        Me.dgSOverlaps.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center
        Me.dgSOverlaps.IsColor = False
        Me.dgSOverlaps.IsColumnContextMenu = False
        Me.dgSOverlaps.IsForMonitoring = False
        Me.dgSOverlaps.Location = New System.Drawing.Point(56, 16)
        Me.dgSOverlaps.Name = "dgSOverlaps"
        Me.dgSOverlaps.PrintLandScape = True
        Me.dgSOverlaps.RowFormatStyle.BackColor = System.Drawing.SystemColors.Control
        Me.dgSOverlaps.RowFormatStyle.Font = New System.Drawing.Font("Tahoma", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.dgSOverlaps.RowFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center
        Me.dgSOverlaps.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.[True]
        Me.dgSOverlaps.SaveGridName = ""
        Me.dgSOverlaps.Size = New System.Drawing.Size(352, 200)
        Me.dgSOverlaps.TabIndex = 2
        Me.dgSOverlaps.Tag = "999"
        '
        'btnSCloseOverlaps
        '
        Me.btnSCloseOverlaps.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSCloseOverlaps.Location = New System.Drawing.Point(4, 184)
        Me.btnSCloseOverlaps.Name = "btnSCloseOverlaps"
        Me.btnSCloseOverlaps.Size = New System.Drawing.Size(48, 24)
        Me.btnSCloseOverlaps.TabIndex = 1
        Me.btnSCloseOverlaps.Text = "بستن"
        '
        'txtAsdresses
        '
        Me.txtAsdresses.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAsdresses.BackColor = System.Drawing.Color.White
        Me.txtAsdresses.Location = New System.Drawing.Point(11, 184)
        Me.txtAsdresses.MaxLength = 300
        Me.txtAsdresses.Multiline = True
        Me.txtAsdresses.Name = "txtAsdresses"
        Me.txtAsdresses.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtAsdresses.Size = New System.Drawing.Size(264, 104)
        Me.txtAsdresses.TabIndex = 3
        '
        'Label23
        '
        Me.Label23.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label23.AutoSize = True
        Me.Label23.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label23.Location = New System.Drawing.Point(85, 144)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(193, 13)
        Me.Label23.TabIndex = 0
        Me.Label23.Text = "آدرس مناطقي که خاموش مي‌گردند"
        '
        'Label24
        '
        Me.Label24.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label24.AutoSize = True
        Me.Label24.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label24.Location = New System.Drawing.Point(87, 22)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(199, 13)
        Me.Label24.TabIndex = 0
        Me.Label24.Text = "مراکز حساسي که خاموش مي‌گردند"
        '
        'txtCriticalLocations
        '
        Me.txtCriticalLocations.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCriticalLocations.BackColor = System.Drawing.Color.White
        Me.txtCriticalLocations.Location = New System.Drawing.Point(11, 40)
        Me.txtCriticalLocations.MaxLength = 300
        Me.txtCriticalLocations.Multiline = True
        Me.txtCriticalLocations.Name = "txtCriticalLocations"
        Me.txtCriticalLocations.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtCriticalLocations.Size = New System.Drawing.Size(272, 96)
        Me.txtCriticalLocations.TabIndex = 0
        '
        'pnlTamirRequestInfo
        '
        Me.pnlTamirRequestInfo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlTamirRequestInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlTamirRequestInfo.CaptionBackColor = System.Drawing.Color.Teal
        Me.pnlTamirRequestInfo.CaptionFont = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.pnlTamirRequestInfo.CaptionForeColor = System.Drawing.Color.White
        Me.pnlTamirRequestInfo.CaptionHeight = 16
        Me.pnlTamirRequestInfo.CaptionText = "زمان، بار و انرژي توزيع نشده"
        Me.pnlTamirRequestInfo.Controls.Add(Me.txtCurrentValue)
        Me.pnlTamirRequestInfo.Controls.Add(Me.chkIsWarmLine)
        Me.pnlTamirRequestInfo.Controls.Add(Me.txtTimeDisconnect)
        Me.pnlTamirRequestInfo.Controls.Add(Me.txtDateDisconnect)
        Me.pnlTamirRequestInfo.Controls.Add(Me.Label11)
        Me.pnlTamirRequestInfo.Controls.Add(Me.txtTimeConnect)
        Me.pnlTamirRequestInfo.Controls.Add(Me.txtDateConnect)
        Me.pnlTamirRequestInfo.Controls.Add(Me.Label13)
        Me.pnlTamirRequestInfo.Controls.Add(Me.Label14)
        Me.pnlTamirRequestInfo.Controls.Add(Me.txtPower)
        Me.pnlTamirRequestInfo.Controls.Add(Me.Label16)
        Me.pnlTamirRequestInfo.Controls.Add(Me.lblPowerUnit)
        Me.pnlTamirRequestInfo.Controls.Add(Me.txtDCInterval)
        Me.pnlTamirRequestInfo.Controls.Add(Me.Label37)
        Me.pnlTamirRequestInfo.Controls.Add(Me.Label15)
        Me.pnlTamirRequestInfo.Controls.Add(Me.Label32)
        Me.pnlTamirRequestInfo.Controls.Add(Me.pnlOverlaps)
        Me.pnlTamirRequestInfo.IsMoveable = False
        Me.pnlTamirRequestInfo.IsWindowMove = False
        Me.pnlTamirRequestInfo.Location = New System.Drawing.Point(312, 136)
        Me.pnlTamirRequestInfo.Name = "pnlTamirRequestInfo"
        Me.pnlTamirRequestInfo.Size = New System.Drawing.Size(408, 112)
        Me.pnlTamirRequestInfo.TabIndex = 1
        '
        'txtCurrentValue
        '
        Me.txtCurrentValue.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCurrentValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCurrentValue.CaptinText = ""
        Me.txtCurrentValue.Enabled = False
        Me.txtCurrentValue.HasCaption = False
        Me.txtCurrentValue.IsForceText = False
        Me.txtCurrentValue.IsFractional = True
        Me.txtCurrentValue.IsIP = False
        Me.txtCurrentValue.IsNumberOnly = True
        Me.txtCurrentValue.IsYear = False
        Me.txtCurrentValue.Location = New System.Drawing.Point(250, 84)
        Me.txtCurrentValue.Name = "txtCurrentValue"
        Me.txtCurrentValue.Size = New System.Drawing.Size(48, 21)
        Me.txtCurrentValue.TabIndex = 4
        Me.txtCurrentValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'chkIsWarmLine
        '
        Me.chkIsWarmLine.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkIsWarmLine.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.chkIsWarmLine.Location = New System.Drawing.Point(113, 82)
        Me.chkIsWarmLine.Name = "chkIsWarmLine"
        Me.chkIsWarmLine.Size = New System.Drawing.Size(94, 24)
        Me.chkIsWarmLine.TabIndex = 5
        Me.chkIsWarmLine.Text = "کار در خط گرم"
        '
        'txtTimeDisconnect
        '
        Me.txtTimeDisconnect.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTimeDisconnect.BackColor = System.Drawing.SystemColors.Window
        Me.txtTimeDisconnect.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTimeDisconnect.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTimeDisconnect.IsShadow = False
        Me.txtTimeDisconnect.IsShowCurrentTime = False
        Me.txtTimeDisconnect.Location = New System.Drawing.Point(115, 20)
        Me.txtTimeDisconnect.MaxLength = 5
        Me.txtTimeDisconnect.MiladiDT = Nothing
        Me.txtTimeDisconnect.Name = "txtTimeDisconnect"
        Me.txtTimeDisconnect.ReadOnly = True
        Me.txtTimeDisconnect.ReadOnlyMaskedEdit = False
        Me.txtTimeDisconnect.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTimeDisconnect.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtTimeDisconnect.Size = New System.Drawing.Size(48, 21)
        Me.txtTimeDisconnect.TabIndex = 1
        Me.txtTimeDisconnect.Text = "__:__"
        Me.txtTimeDisconnect.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtDateDisconnect
        '
        Me.txtDateDisconnect.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDateDisconnect.BackColor = System.Drawing.SystemColors.Window
        Me.txtDateDisconnect.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDateDisconnect.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDateDisconnect.IntegerDate = 0
        Me.txtDateDisconnect.IsShadow = False
        Me.txtDateDisconnect.IsShowCurrentDate = False
        Me.txtDateDisconnect.Location = New System.Drawing.Point(163, 20)
        Me.txtDateDisconnect.MaxLength = 10
        Me.txtDateDisconnect.MiladiDT = CType(resources.GetObject("txtDateDisconnect.MiladiDT"), Object)
        Me.txtDateDisconnect.Name = "txtDateDisconnect"
        Me.txtDateDisconnect.ReadOnly = True
        Me.txtDateDisconnect.ReadOnlyMaskedEdit = False
        Me.txtDateDisconnect.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDateDisconnect.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtDateDisconnect.ShamsiDT = "____/__/__"
        Me.txtDateDisconnect.Size = New System.Drawing.Size(85, 21)
        Me.txtDateDisconnect.TabIndex = 0
        Me.txtDateDisconnect.Text = "____/__/__"
        Me.txtDateDisconnect.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtDateDisconnect.TimeMaskedEditorOBJ = Nothing
        '
        'Label11
        '
        Me.Label11.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label11.Location = New System.Drawing.Point(245, 22)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(148, 12)
        Me.Label11.TabIndex = 0
        Me.Label11.Text = "تاريخ و ساعت قطع درخواستي"
        '
        'txtTimeConnect
        '
        Me.txtTimeConnect.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTimeConnect.BackColor = System.Drawing.SystemColors.Window
        Me.txtTimeConnect.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTimeConnect.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTimeConnect.IsShadow = False
        Me.txtTimeConnect.IsShowCurrentTime = False
        Me.txtTimeConnect.Location = New System.Drawing.Point(115, 50)
        Me.txtTimeConnect.MaxLength = 5
        Me.txtTimeConnect.MiladiDT = Nothing
        Me.txtTimeConnect.Name = "txtTimeConnect"
        Me.txtTimeConnect.ReadOnly = True
        Me.txtTimeConnect.ReadOnlyMaskedEdit = False
        Me.txtTimeConnect.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTimeConnect.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtTimeConnect.Size = New System.Drawing.Size(48, 21)
        Me.txtTimeConnect.TabIndex = 3
        Me.txtTimeConnect.Text = "__:__"
        Me.txtTimeConnect.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtDateConnect
        '
        Me.txtDateConnect.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDateConnect.BackColor = System.Drawing.SystemColors.Window
        Me.txtDateConnect.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDateConnect.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDateConnect.IntegerDate = 0
        Me.txtDateConnect.IsShadow = False
        Me.txtDateConnect.IsShowCurrentDate = False
        Me.txtDateConnect.Location = New System.Drawing.Point(163, 50)
        Me.txtDateConnect.MaxLength = 10
        Me.txtDateConnect.MiladiDT = CType(resources.GetObject("txtDateConnect.MiladiDT"), Object)
        Me.txtDateConnect.Name = "txtDateConnect"
        Me.txtDateConnect.ReadOnly = True
        Me.txtDateConnect.ReadOnlyMaskedEdit = False
        Me.txtDateConnect.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDateConnect.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtDateConnect.ShamsiDT = "____/__/__"
        Me.txtDateConnect.Size = New System.Drawing.Size(85, 21)
        Me.txtDateConnect.TabIndex = 2
        Me.txtDateConnect.Text = "____/__/__"
        Me.txtDateConnect.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtDateConnect.TimeMaskedEditorOBJ = Nothing
        '
        'Label13
        '
        Me.Label13.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label13.Location = New System.Drawing.Point(245, 52)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(149, 12)
        Me.Label13.TabIndex = 0
        Me.Label13.Text = "تاريخ و ساعت وصل درخواستي"
        '
        'Label14
        '
        Me.Label14.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label14.Location = New System.Drawing.Point(295, 86)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(102, 12)
        Me.Label14.TabIndex = 0
        Me.Label14.Text = "ميانگين بار قطع شده"
        '
        'txtPower
        '
        Me.txtPower.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPower.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPower.CaptinText = ""
        Me.txtPower.HasCaption = False
        Me.txtPower.IsForceText = False
        Me.txtPower.IsFractional = True
        Me.txtPower.IsIP = False
        Me.txtPower.IsNumberOnly = True
        Me.txtPower.IsYear = False
        Me.txtPower.Location = New System.Drawing.Point(40, 84)
        Me.txtPower.Name = "txtPower"
        Me.txtPower.ReadOnly = True
        Me.txtPower.Size = New System.Drawing.Size(48, 21)
        Me.txtPower.TabIndex = 7
        Me.txtPower.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label16
        '
        Me.Label16.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label16.Location = New System.Drawing.Point(223, 86)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(22, 12)
        Me.Label16.TabIndex = 0
        Me.Label16.Text = "آمپر"
        '
        'lblPowerUnit
        '
        Me.lblPowerUnit.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPowerUnit.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblPowerUnit.Location = New System.Drawing.Point(10, 89)
        Me.lblPowerUnit.Name = "lblPowerUnit"
        Me.lblPowerUnit.Size = New System.Drawing.Size(28, 10)
        Me.lblPowerUnit.TabIndex = 0
        Me.lblPowerUnit.Text = "MWh"
        '
        'txtDCInterval
        '
        Me.txtDCInterval.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDCInterval.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDCInterval.CaptinText = ""
        Me.txtDCInterval.HasCaption = False
        Me.txtDCInterval.IsForceText = False
        Me.txtDCInterval.IsFractional = False
        Me.txtDCInterval.IsIP = False
        Me.txtDCInterval.IsNumberOnly = False
        Me.txtDCInterval.IsYear = False
        Me.txtDCInterval.Location = New System.Drawing.Point(40, 36)
        Me.txtDCInterval.Name = "txtDCInterval"
        Me.txtDCInterval.ReadOnly = True
        Me.txtDCInterval.Size = New System.Drawing.Size(48, 21)
        Me.txtDCInterval.TabIndex = 6
        Me.txtDCInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label37
        '
        Me.Label37.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label37.AutoSize = True
        Me.Label37.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label37.Location = New System.Drawing.Point(8, 39)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(32, 12)
        Me.Label37.TabIndex = 0
        Me.Label37.Text = "دقيقه"
        '
        'Label15
        '
        Me.Label15.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label15.Location = New System.Drawing.Point(5, 67)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(85, 12)
        Me.Label15.TabIndex = 0
        Me.Label15.Text = "انرژي توزيع نشده"
        '
        'Label32
        '
        Me.Label32.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label32.AutoSize = True
        Me.Label32.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label32.Location = New System.Drawing.Point(41, 20)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(50, 12)
        Me.Label32.TabIndex = 0
        Me.Label32.Text = "مدت قطع"
        '
        'pnlOverlaps
        '
        Me.pnlOverlaps.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlOverlaps.CaptionBackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.pnlOverlaps.CaptionFont = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.pnlOverlaps.CaptionForeColor = System.Drawing.Color.White
        Me.pnlOverlaps.CaptionHeight = 16
        Me.pnlOverlaps.CaptionText = "اطلاعات همزماني با خاموشي ديگر"
        Me.pnlOverlaps.Controls.Add(Me.dgOverlaps)
        Me.pnlOverlaps.Controls.Add(Me.btnCloseOverlaps)
        Me.pnlOverlaps.IsMoveable = False
        Me.pnlOverlaps.IsWindowMove = False
        Me.pnlOverlaps.Location = New System.Drawing.Point(0, 0)
        Me.pnlOverlaps.Name = "pnlOverlaps"
        Me.pnlOverlaps.Size = New System.Drawing.Size(408, 112)
        Me.pnlOverlaps.TabIndex = 9
        Me.pnlOverlaps.Visible = False
        '
        'dgOverlaps
        '
        Me.dgOverlaps.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgOverlaps.BackColor = System.Drawing.SystemColors.Control
        Me.dgOverlaps.BuiltInTextsData = resources.GetString("dgOverlaps.BuiltInTextsData")
        Me.dgOverlaps.CurrentRowIndex = -1
        dgOverlaps_DesignTimeLayout.LayoutString = resources.GetString("dgOverlaps_DesignTimeLayout.LayoutString")
        Me.dgOverlaps.DesignTimeLayout = dgOverlaps_DesignTimeLayout
        Me.dgOverlaps.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular
        Me.dgOverlaps.EnableFormatEvent = True
        Me.dgOverlaps.EnableSaveLayout = True
        Me.dgOverlaps.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.dgOverlaps.GroupByBoxFormatStyle.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.dgOverlaps.GroupByBoxInfoFormatStyle.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.dgOverlaps.GroupByBoxVisible = False
        Me.dgOverlaps.HeaderFormatStyle.LineAlignment = Janus.Windows.GridEX.TextAlignment.Center
        Me.dgOverlaps.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center
        Me.dgOverlaps.IsColor = False
        Me.dgOverlaps.IsColumnContextMenu = False
        Me.dgOverlaps.IsForMonitoring = False
        Me.dgOverlaps.Location = New System.Drawing.Point(56, 16)
        Me.dgOverlaps.Name = "dgOverlaps"
        Me.dgOverlaps.PrintLandScape = True
        Me.dgOverlaps.RowFormatStyle.BackColor = System.Drawing.SystemColors.Control
        Me.dgOverlaps.RowFormatStyle.Font = New System.Drawing.Font("Tahoma", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.dgOverlaps.RowFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center
        Me.dgOverlaps.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.[True]
        Me.dgOverlaps.SaveGridName = ""
        Me.dgOverlaps.Size = New System.Drawing.Size(352, 96)
        Me.dgOverlaps.TabIndex = 2
        Me.dgOverlaps.Tag = "999"
        '
        'btnCloseOverlaps
        '
        Me.btnCloseOverlaps.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCloseOverlaps.Location = New System.Drawing.Point(4, 80)
        Me.btnCloseOverlaps.Name = "btnCloseOverlaps"
        Me.btnCloseOverlaps.Size = New System.Drawing.Size(48, 24)
        Me.btnCloseOverlaps.TabIndex = 1
        Me.btnCloseOverlaps.Text = "بستن"
        '
        'sbtnOverlaps
        '
        Me.sbtnOverlaps.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.sbtnOverlaps.BorderColor = System.Drawing.Color.Transparent
        Me.sbtnOverlaps.BorderWidth = 0!
        Me.sbtnOverlaps.ColorEnd = System.Drawing.Color.Red
        Me.sbtnOverlaps.ColorStart = System.Drawing.Color.LavenderBlush
        Me.sbtnOverlaps.Cursor = System.Windows.Forms.Cursors.Hand
        Me.sbtnOverlaps.Font = New System.Drawing.Font("Tahoma", 6.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.sbtnOverlaps.ForeColor = System.Drawing.Color.Blue
        Me.sbtnOverlaps.GradientMode = Bargh_Common.StyleButton.ButtonGradientModes.Centeral
        Me.sbtnOverlaps.Is2Section = True
        Me.sbtnOverlaps.IsBlink = True
        Me.sbtnOverlaps.IsGradient = True
        Me.sbtnOverlaps.IsReverse = False
        Me.sbtnOverlaps.Location = New System.Drawing.Point(312, 136)
        Me.sbtnOverlaps.Name = "sbtnOverlaps"
        Me.sbtnOverlaps.Size = New System.Drawing.Size(125, 16)
        Me.sbtnOverlaps.TabIndex = 8
        Me.sbtnOverlaps.Tag = "999"
        Me.sbtnOverlaps.Text = "همزماني با خاموشي ديگر"
        Me.ToolTip1.SetToolTip(Me.sbtnOverlaps, "همزماني با خاموشي ديگر")
        Me.sbtnOverlaps.Visible = False
        '
        'pnlConfirmInfo
        '
        Me.pnlConfirmInfo.Controls.Add(Me.HatchPanel1)
        Me.pnlConfirmInfo.Controls.Add(Me.txtTMRequest)
        Me.pnlConfirmInfo.Controls.Add(Me.txtDTRequest)
        Me.pnlConfirmInfo.Controls.Add(Me.Label26)
        Me.pnlConfirmInfo.Controls.Add(Me.cmbRequestUserName)
        Me.pnlConfirmInfo.Controls.Add(Me.cmbConfirmNaheihUsername)
        Me.pnlConfirmInfo.Controls.Add(Me.Label56)
        Me.pnlConfirmInfo.Controls.Add(Me.cmbConfirmCenterUsername)
        Me.pnlConfirmInfo.Controls.Add(Me.Label57)
        Me.pnlConfirmInfo.Controls.Add(Me.cmbConfirmSetadUsername)
        Me.pnlConfirmInfo.Controls.Add(Me.Label58)
        Me.pnlConfirmInfo.Controls.Add(Me.Label60)
        Me.pnlConfirmInfo.Controls.Add(Me.Label61)
        Me.pnlConfirmInfo.Controls.Add(Me.Label62)
        Me.pnlConfirmInfo.Controls.Add(Me.Label63)
        Me.pnlConfirmInfo.Controls.Add(Me.txtTMSendToCenter)
        Me.pnlConfirmInfo.Controls.Add(Me.txtDTSendToCenter)
        Me.pnlConfirmInfo.Controls.Add(Me.txtDTSendToSetad)
        Me.pnlConfirmInfo.Controls.Add(Me.txtTMSendToSetad)
        Me.pnlConfirmInfo.Controls.Add(Me.txtTMConfirm)
        Me.pnlConfirmInfo.Controls.Add(Me.txtDTConfirm)
        Me.pnlConfirmInfo.Controls.Add(Me.HatchPanel2)
        Me.pnlConfirmInfo.Controls.Add(Me.HatchPanel3)
        Me.pnlConfirmInfo.Controls.Add(Me.HatchPanel4)
        Me.pnlConfirmInfo.Location = New System.Drawing.Point(36, 231)
        Me.pnlConfirmInfo.Name = "pnlConfirmInfo"
        Me.pnlConfirmInfo.Size = New System.Drawing.Size(656, 120)
        Me.pnlConfirmInfo.TabIndex = 4
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
        Me.HatchPanel1.Location = New System.Drawing.Point(226, 46)
        Me.HatchPanel1.Name = "HatchPanel1"
        Me.HatchPanel1.Size = New System.Drawing.Size(126, 3)
        Me.HatchPanel1.TabIndex = 3
        '
        'txtTMRequest
        '
        Me.txtTMRequest.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTMRequest.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtTMRequest.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTMRequest.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTMRequest.IsShadow = False
        Me.txtTMRequest.IsShowCurrentTime = False
        Me.txtTMRequest.Location = New System.Drawing.Point(32, 8)
        Me.txtTMRequest.MaxLength = 5
        Me.txtTMRequest.MiladiDT = Nothing
        Me.txtTMRequest.Name = "txtTMRequest"
        Me.txtTMRequest.ReadOnly = True
        Me.txtTMRequest.ReadOnlyMaskedEdit = True
        Me.txtTMRequest.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtTMRequest.Size = New System.Drawing.Size(48, 21)
        Me.txtTMRequest.TabIndex = 2
        Me.txtTMRequest.Text = "__:__"
        Me.txtTMRequest.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtDTRequest
        '
        Me.txtDTRequest.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDTRequest.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtDTRequest.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDTRequest.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDTRequest.IntegerDate = 0
        Me.txtDTRequest.IsShadow = False
        Me.txtDTRequest.IsShowCurrentDate = False
        Me.txtDTRequest.Location = New System.Drawing.Point(80, 8)
        Me.txtDTRequest.MaxLength = 10
        Me.txtDTRequest.MiladiDT = CType(resources.GetObject("txtDTRequest.MiladiDT"), Object)
        Me.txtDTRequest.Name = "txtDTRequest"
        Me.txtDTRequest.ReadOnly = True
        Me.txtDTRequest.ReadOnlyMaskedEdit = True
        Me.txtDTRequest.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtDTRequest.ShamsiDT = "____/__/__"
        Me.txtDTRequest.Size = New System.Drawing.Size(85, 21)
        Me.txtDTRequest.TabIndex = 1
        Me.txtDTRequest.Text = "____/__/__"
        Me.txtDTRequest.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtDTRequest.TimeMaskedEditorOBJ = Nothing
        '
        'Label26
        '
        Me.Label26.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label26.AutoSize = True
        Me.Label26.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label26.Location = New System.Drawing.Point(520, 10)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(116, 13)
        Me.Label26.TabIndex = 0
        Me.Label26.Text = "کاربر درخواست کننده"
        '
        'cmbRequestUserName
        '
        Me.cmbRequestUserName.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbRequestUserName.BackColor = System.Drawing.Color.FromArgb(CType(CType(223, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(223, Byte), Integer))
        Me.cmbRequestUserName.DisplayMember = "UserName"
        Me.cmbRequestUserName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.cmbRequestUserName.IsReadOnly = True
        Me.cmbRequestUserName.Location = New System.Drawing.Point(358, 8)
        Me.cmbRequestUserName.Name = "cmbRequestUserName"
        Me.cmbRequestUserName.Size = New System.Drawing.Size(152, 21)
        Me.cmbRequestUserName.TabIndex = 0
        Me.cmbRequestUserName.ValueMember = "AreaUserId"
        '
        'cmbConfirmNaheihUsername
        '
        Me.cmbConfirmNaheihUsername.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbConfirmNaheihUsername.BackColor = System.Drawing.Color.FromArgb(CType(CType(223, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(223, Byte), Integer))
        Me.cmbConfirmNaheihUsername.DisplayMember = "UserName"
        Me.cmbConfirmNaheihUsername.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.cmbConfirmNaheihUsername.IsReadOnly = True
        Me.cmbConfirmNaheihUsername.Location = New System.Drawing.Point(358, 37)
        Me.cmbConfirmNaheihUsername.Name = "cmbConfirmNaheihUsername"
        Me.cmbConfirmNaheihUsername.Size = New System.Drawing.Size(152, 21)
        Me.cmbConfirmNaheihUsername.TabIndex = 0
        Me.cmbConfirmNaheihUsername.ValueMember = "AreaUserId"
        '
        'Label56
        '
        Me.Label56.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label56.AutoSize = True
        Me.Label56.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label56.Location = New System.Drawing.Point(520, 39)
        Me.Label56.Name = "Label56"
        Me.Label56.Size = New System.Drawing.Size(116, 13)
        Me.Label56.TabIndex = 0
        Me.Label56.Text = "کاربر تأييد کننده ناحيه"
        '
        'cmbConfirmCenterUsername
        '
        Me.cmbConfirmCenterUsername.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbConfirmCenterUsername.BackColor = System.Drawing.Color.FromArgb(CType(CType(223, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(223, Byte), Integer))
        Me.cmbConfirmCenterUsername.DisplayMember = "UserName"
        Me.cmbConfirmCenterUsername.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.cmbConfirmCenterUsername.IsReadOnly = True
        Me.cmbConfirmCenterUsername.Location = New System.Drawing.Point(358, 66)
        Me.cmbConfirmCenterUsername.Name = "cmbConfirmCenterUsername"
        Me.cmbConfirmCenterUsername.Size = New System.Drawing.Size(152, 21)
        Me.cmbConfirmCenterUsername.TabIndex = 0
        Me.cmbConfirmCenterUsername.ValueMember = "AreaUserId"
        '
        'Label57
        '
        Me.Label57.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label57.AutoSize = True
        Me.Label57.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label57.Location = New System.Drawing.Point(520, 68)
        Me.Label57.Name = "Label57"
        Me.Label57.Size = New System.Drawing.Size(114, 13)
        Me.Label57.TabIndex = 0
        Me.Label57.Text = "کاربر تأييد کننده مرکز"
        '
        'cmbConfirmSetadUsername
        '
        Me.cmbConfirmSetadUsername.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbConfirmSetadUsername.BackColor = System.Drawing.Color.FromArgb(CType(CType(223, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(223, Byte), Integer))
        Me.cmbConfirmSetadUsername.DisplayMember = "UserName"
        Me.cmbConfirmSetadUsername.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.cmbConfirmSetadUsername.IsReadOnly = True
        Me.cmbConfirmSetadUsername.Location = New System.Drawing.Point(358, 95)
        Me.cmbConfirmSetadUsername.Name = "cmbConfirmSetadUsername"
        Me.cmbConfirmSetadUsername.Size = New System.Drawing.Size(152, 21)
        Me.cmbConfirmSetadUsername.TabIndex = 0
        Me.cmbConfirmSetadUsername.ValueMember = "AreaUserId"
        '
        'Label58
        '
        Me.Label58.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label58.AutoSize = True
        Me.Label58.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label58.Location = New System.Drawing.Point(520, 97)
        Me.Label58.Name = "Label58"
        Me.Label58.Size = New System.Drawing.Size(120, 13)
        Me.Label58.TabIndex = 0
        Me.Label58.Text = "کاربر تأييد کننده نهايي"
        '
        'Label60
        '
        Me.Label60.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label60.AutoSize = True
        Me.Label60.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label60.Location = New System.Drawing.Point(168, 10)
        Me.Label60.Name = "Label60"
        Me.Label60.Size = New System.Drawing.Size(87, 13)
        Me.Label60.TabIndex = 0
        Me.Label60.Text = "زمان درخواست"
        '
        'Label61
        '
        Me.Label61.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label61.AutoSize = True
        Me.Label61.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label61.Location = New System.Drawing.Point(168, 39)
        Me.Label61.Name = "Label61"
        Me.Label61.Size = New System.Drawing.Size(57, 13)
        Me.Label61.TabIndex = 0
        Me.Label61.Text = "زمان تأييد"
        '
        'Label62
        '
        Me.Label62.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label62.AutoSize = True
        Me.Label62.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label62.Location = New System.Drawing.Point(168, 68)
        Me.Label62.Name = "Label62"
        Me.Label62.Size = New System.Drawing.Size(57, 13)
        Me.Label62.TabIndex = 0
        Me.Label62.Text = "زمان تأييد"
        '
        'Label63
        '
        Me.Label63.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label63.AutoSize = True
        Me.Label63.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label63.Location = New System.Drawing.Point(168, 97)
        Me.Label63.Name = "Label63"
        Me.Label63.Size = New System.Drawing.Size(57, 13)
        Me.Label63.TabIndex = 0
        Me.Label63.Text = "زمان تأييد"
        '
        'txtTMSendToCenter
        '
        Me.txtTMSendToCenter.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTMSendToCenter.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtTMSendToCenter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTMSendToCenter.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTMSendToCenter.IsShadow = False
        Me.txtTMSendToCenter.IsShowCurrentTime = False
        Me.txtTMSendToCenter.Location = New System.Drawing.Point(32, 37)
        Me.txtTMSendToCenter.MaxLength = 5
        Me.txtTMSendToCenter.MiladiDT = Nothing
        Me.txtTMSendToCenter.Name = "txtTMSendToCenter"
        Me.txtTMSendToCenter.ReadOnly = True
        Me.txtTMSendToCenter.ReadOnlyMaskedEdit = True
        Me.txtTMSendToCenter.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtTMSendToCenter.Size = New System.Drawing.Size(48, 21)
        Me.txtTMSendToCenter.TabIndex = 2
        Me.txtTMSendToCenter.Text = "__:__"
        Me.txtTMSendToCenter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtDTSendToCenter
        '
        Me.txtDTSendToCenter.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDTSendToCenter.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtDTSendToCenter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDTSendToCenter.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDTSendToCenter.IntegerDate = 0
        Me.txtDTSendToCenter.IsShadow = False
        Me.txtDTSendToCenter.IsShowCurrentDate = False
        Me.txtDTSendToCenter.Location = New System.Drawing.Point(80, 37)
        Me.txtDTSendToCenter.MaxLength = 10
        Me.txtDTSendToCenter.MiladiDT = CType(resources.GetObject("txtDTSendToCenter.MiladiDT"), Object)
        Me.txtDTSendToCenter.Name = "txtDTSendToCenter"
        Me.txtDTSendToCenter.ReadOnly = True
        Me.txtDTSendToCenter.ReadOnlyMaskedEdit = True
        Me.txtDTSendToCenter.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtDTSendToCenter.ShamsiDT = "____/__/__"
        Me.txtDTSendToCenter.Size = New System.Drawing.Size(85, 21)
        Me.txtDTSendToCenter.TabIndex = 1
        Me.txtDTSendToCenter.Text = "____/__/__"
        Me.txtDTSendToCenter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtDTSendToCenter.TimeMaskedEditorOBJ = Nothing
        '
        'txtDTSendToSetad
        '
        Me.txtDTSendToSetad.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDTSendToSetad.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtDTSendToSetad.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDTSendToSetad.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDTSendToSetad.IntegerDate = 0
        Me.txtDTSendToSetad.IsShadow = False
        Me.txtDTSendToSetad.IsShowCurrentDate = False
        Me.txtDTSendToSetad.Location = New System.Drawing.Point(80, 66)
        Me.txtDTSendToSetad.MaxLength = 10
        Me.txtDTSendToSetad.MiladiDT = CType(resources.GetObject("txtDTSendToSetad.MiladiDT"), Object)
        Me.txtDTSendToSetad.Name = "txtDTSendToSetad"
        Me.txtDTSendToSetad.ReadOnly = True
        Me.txtDTSendToSetad.ReadOnlyMaskedEdit = True
        Me.txtDTSendToSetad.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtDTSendToSetad.ShamsiDT = "____/__/__"
        Me.txtDTSendToSetad.Size = New System.Drawing.Size(85, 21)
        Me.txtDTSendToSetad.TabIndex = 1
        Me.txtDTSendToSetad.Text = "____/__/__"
        Me.txtDTSendToSetad.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtDTSendToSetad.TimeMaskedEditorOBJ = Nothing
        '
        'txtTMSendToSetad
        '
        Me.txtTMSendToSetad.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTMSendToSetad.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtTMSendToSetad.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTMSendToSetad.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTMSendToSetad.IsShadow = False
        Me.txtTMSendToSetad.IsShowCurrentTime = False
        Me.txtTMSendToSetad.Location = New System.Drawing.Point(32, 66)
        Me.txtTMSendToSetad.MaxLength = 5
        Me.txtTMSendToSetad.MiladiDT = Nothing
        Me.txtTMSendToSetad.Name = "txtTMSendToSetad"
        Me.txtTMSendToSetad.ReadOnly = True
        Me.txtTMSendToSetad.ReadOnlyMaskedEdit = True
        Me.txtTMSendToSetad.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtTMSendToSetad.Size = New System.Drawing.Size(48, 21)
        Me.txtTMSendToSetad.TabIndex = 2
        Me.txtTMSendToSetad.Text = "__:__"
        Me.txtTMSendToSetad.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtTMConfirm
        '
        Me.txtTMConfirm.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTMConfirm.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtTMConfirm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTMConfirm.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTMConfirm.IsShadow = False
        Me.txtTMConfirm.IsShowCurrentTime = False
        Me.txtTMConfirm.Location = New System.Drawing.Point(32, 95)
        Me.txtTMConfirm.MaxLength = 5
        Me.txtTMConfirm.MiladiDT = Nothing
        Me.txtTMConfirm.Name = "txtTMConfirm"
        Me.txtTMConfirm.ReadOnly = True
        Me.txtTMConfirm.ReadOnlyMaskedEdit = True
        Me.txtTMConfirm.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtTMConfirm.Size = New System.Drawing.Size(48, 21)
        Me.txtTMConfirm.TabIndex = 2
        Me.txtTMConfirm.Text = "__:__"
        Me.txtTMConfirm.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtDTConfirm
        '
        Me.txtDTConfirm.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDTConfirm.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtDTConfirm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDTConfirm.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDTConfirm.IntegerDate = 0
        Me.txtDTConfirm.IsShadow = False
        Me.txtDTConfirm.IsShowCurrentDate = False
        Me.txtDTConfirm.Location = New System.Drawing.Point(80, 95)
        Me.txtDTConfirm.MaxLength = 10
        Me.txtDTConfirm.MiladiDT = CType(resources.GetObject("txtDTConfirm.MiladiDT"), Object)
        Me.txtDTConfirm.Name = "txtDTConfirm"
        Me.txtDTConfirm.ReadOnly = True
        Me.txtDTConfirm.ReadOnlyMaskedEdit = True
        Me.txtDTConfirm.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtDTConfirm.ShamsiDT = "____/__/__"
        Me.txtDTConfirm.Size = New System.Drawing.Size(85, 21)
        Me.txtDTConfirm.TabIndex = 1
        Me.txtDTConfirm.Text = "____/__/__"
        Me.txtDTConfirm.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtDTConfirm.TimeMaskedEditorOBJ = Nothing
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
        Me.HatchPanel2.Location = New System.Drawing.Point(226, 75)
        Me.HatchPanel2.Name = "HatchPanel2"
        Me.HatchPanel2.Size = New System.Drawing.Size(126, 3)
        Me.HatchPanel2.TabIndex = 3
        '
        'HatchPanel3
        '
        Me.HatchPanel3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HatchPanel3.Direction = Bargh_Common.HatchPanel.LeftRight.Right
        Me.HatchPanel3.HatchColor = System.Drawing.Color.DarkBlue
        Me.HatchPanel3.HatchCount = 3.0!
        Me.HatchPanel3.HatchSpaceWidth = 20.0!
        Me.HatchPanel3.HatchStyle = System.Drawing.Drawing2D.DashStyle.Dash
        Me.HatchPanel3.HatchWidth = 1.0!
        Me.HatchPanel3.Location = New System.Drawing.Point(226, 104)
        Me.HatchPanel3.Name = "HatchPanel3"
        Me.HatchPanel3.Size = New System.Drawing.Size(126, 3)
        Me.HatchPanel3.TabIndex = 3
        '
        'HatchPanel4
        '
        Me.HatchPanel4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HatchPanel4.Direction = Bargh_Common.HatchPanel.LeftRight.Right
        Me.HatchPanel4.HatchColor = System.Drawing.Color.DarkBlue
        Me.HatchPanel4.HatchCount = 3.0!
        Me.HatchPanel4.HatchSpaceWidth = 20.0!
        Me.HatchPanel4.HatchStyle = System.Drawing.Drawing2D.DashStyle.Dash
        Me.HatchPanel4.HatchWidth = 1.0!
        Me.HatchPanel4.Location = New System.Drawing.Point(256, 17)
        Me.HatchPanel4.Name = "HatchPanel4"
        Me.HatchPanel4.Size = New System.Drawing.Size(96, 3)
        Me.HatchPanel4.TabIndex = 3
        '
        'Label27
        '
        Me.Label27.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label27.AutoSize = True
        Me.Label27.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label27.Location = New System.Drawing.Point(301, 23)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(86, 13)
        Me.Label27.TabIndex = 0
        Me.Label27.Text = "کاربر تأييد کننده"
        '
        'cmbConfirmUserName
        '
        Me.cmbConfirmUserName.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbConfirmUserName.BackColor = System.Drawing.Color.FromArgb(CType(CType(223, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(223, Byte), Integer))
        Me.cmbConfirmUserName.DisplayMember = "UserName"
        Me.cmbConfirmUserName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.cmbConfirmUserName.IsReadOnly = True
        Me.cmbConfirmUserName.Location = New System.Drawing.Point(166, 21)
        Me.cmbConfirmUserName.Name = "cmbConfirmUserName"
        Me.cmbConfirmUserName.Size = New System.Drawing.Size(132, 21)
        Me.cmbConfirmUserName.TabIndex = 0
        Me.cmbConfirmUserName.ValueMember = "AreaUserId"
        '
        'Label28
        '
        Me.Label28.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label28.AutoSize = True
        Me.Label28.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label28.Location = New System.Drawing.Point(283, 202)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(104, 13)
        Me.Label28.TabIndex = 0
        Me.Label28.Text = "تاريخ و ساعت تأييد"
        '
        'txtTimeConfirm
        '
        Me.txtTimeConfirm.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTimeConfirm.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtTimeConfirm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTimeConfirm.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTimeConfirm.IsShadow = False
        Me.txtTimeConfirm.IsShowCurrentTime = False
        Me.txtTimeConfirm.Location = New System.Drawing.Point(157, 200)
        Me.txtTimeConfirm.MaxLength = 5
        Me.txtTimeConfirm.MiladiDT = Nothing
        Me.txtTimeConfirm.Name = "txtTimeConfirm"
        Me.txtTimeConfirm.ReadOnly = True
        Me.txtTimeConfirm.ReadOnlyMaskedEdit = True
        Me.txtTimeConfirm.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTimeConfirm.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtTimeConfirm.Size = New System.Drawing.Size(42, 21)
        Me.txtTimeConfirm.TabIndex = 5
        Me.txtTimeConfirm.Text = "__:__"
        Me.txtTimeConfirm.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtDateConfirm
        '
        Me.txtDateConfirm.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDateConfirm.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtDateConfirm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDateConfirm.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDateConfirm.IntegerDate = 0
        Me.txtDateConfirm.IsShadow = False
        Me.txtDateConfirm.IsShowCurrentDate = False
        Me.txtDateConfirm.Location = New System.Drawing.Point(200, 200)
        Me.txtDateConfirm.MaxLength = 10
        Me.txtDateConfirm.MiladiDT = CType(resources.GetObject("txtDateConfirm.MiladiDT"), Object)
        Me.txtDateConfirm.Name = "txtDateConfirm"
        Me.txtDateConfirm.ReadOnly = True
        Me.txtDateConfirm.ReadOnlyMaskedEdit = True
        Me.txtDateConfirm.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDateConfirm.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtDateConfirm.ShamsiDT = "____/__/__"
        Me.txtDateConfirm.Size = New System.Drawing.Size(81, 21)
        Me.txtDateConfirm.TabIndex = 4
        Me.txtDateConfirm.Text = "____/__/__"
        Me.txtDateConfirm.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtDateConfirm.TimeMaskedEditorOBJ = Nothing
        '
        'pnlConfirm
        '
        Me.pnlConfirm.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlConfirm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlConfirm.CaptionBackColor = System.Drawing.Color.Teal
        Me.pnlConfirm.CaptionFont = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.pnlConfirm.CaptionForeColor = System.Drawing.Color.White
        Me.pnlConfirm.CaptionHeight = 16
        Me.pnlConfirm.CaptionText = "تأييديه نهايي"
        Me.pnlConfirm.Controls.Add(Me.chkIsSendToInformApp)
        Me.pnlConfirm.Controls.Add(Me.PicSendSMSSensitive)
        Me.pnlConfirm.Controls.Add(Me.chkIsSendSMSSensitive)
        Me.pnlConfirm.Controls.Add(Me.picLock)
        Me.pnlConfirm.Controls.Add(Me.txtUnConfirmReason)
        Me.pnlConfirm.Controls.Add(Me.rbIsConfirm)
        Me.pnlConfirm.Controls.Add(Me.rbIsNotConfirm)
        Me.pnlConfirm.Controls.Add(Me.cmbConfirmUserName)
        Me.pnlConfirm.Controls.Add(Me.Label28)
        Me.pnlConfirm.Controls.Add(Me.txtTimeConfirm)
        Me.pnlConfirm.Controls.Add(Me.txtDateConfirm)
        Me.pnlConfirm.Controls.Add(Me.Label30)
        Me.pnlConfirm.Controls.Add(Me.Label27)
        Me.pnlConfirm.Controls.Add(Me.lblNotConfirmReason)
        Me.pnlConfirm.Controls.Add(Me.rbIsReturned)
        Me.pnlConfirm.Controls.Add(Me.txtReturn)
        Me.pnlConfirm.Controls.Add(Me.LabelRetrun)
        Me.pnlConfirm.Enabled = False
        Me.pnlConfirm.IsMoveable = False
        Me.pnlConfirm.IsWindowMove = False
        Me.pnlConfirm.Location = New System.Drawing.Point(316, 8)
        Me.pnlConfirm.Name = "pnlConfirm"
        Me.pnlConfirm.Size = New System.Drawing.Size(404, 232)
        Me.pnlConfirm.TabIndex = 0
        '
        'chkIsSendToInformApp
        '
        Me.chkIsSendToInformApp.AutoSize = True
        Me.chkIsSendToInformApp.Checked = True
        Me.chkIsSendToInformApp.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkIsSendToInformApp.Enabled = False
        Me.chkIsSendToInformApp.Location = New System.Drawing.Point(107, 178)
        Me.chkIsSendToInformApp.Name = "chkIsSendToInformApp"
        Me.chkIsSendToInformApp.Size = New System.Drawing.Size(196, 17)
        Me.chkIsSendToInformApp.TabIndex = 51
        Me.chkIsSendToInformApp.Text = "اطلاع رساني از طريق نرم افزار موبايل"
        Me.chkIsSendToInformApp.UseVisualStyleBackColor = True
        Me.chkIsSendToInformApp.Visible = False
        '
        'PicSendSMSSensitive
        '
        Me.PicSendSMSSensitive.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PicSendSMSSensitive.Image = CType(resources.GetObject("PicSendSMSSensitive.Image"), System.Drawing.Image)
        Me.PicSendSMSSensitive.Location = New System.Drawing.Point(283, 152)
        Me.PicSendSMSSensitive.Name = "PicSendSMSSensitive"
        Me.PicSendSMSSensitive.Size = New System.Drawing.Size(20, 20)
        Me.PicSendSMSSensitive.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PicSendSMSSensitive.TabIndex = 50
        Me.PicSendSMSSensitive.TabStop = False
        Me.GlobalToolTip.SetToolTip(Me.PicSendSMSSensitive, "اين قسمت در نسخه بدون قرارداد پشتيباني و گارانتي معتبر فعال نمي‌باشد")
        Me.PicSendSMSSensitive.Visible = False
        '
        'chkIsSendSMSSensitive
        '
        Me.chkIsSendSMSSensitive.Enabled = False
        Me.chkIsSendSMSSensitive.Location = New System.Drawing.Point(79, 155)
        Me.chkIsSendSMSSensitive.Name = "chkIsSendSMSSensitive"
        Me.chkIsSendSMSSensitive.Size = New System.Drawing.Size(224, 19)
        Me.chkIsSendSMSSensitive.TabIndex = 49
        Me.chkIsSendSMSSensitive.Text = "اطلاع رساني به مشترکين حساس با SMS"
        '
        'picLock
        '
        Me.picLock.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picLock.Image = CType(resources.GetObject("picLock.Image"), System.Drawing.Image)
        Me.picLock.Location = New System.Drawing.Point(143, 55)
        Me.picLock.Name = "picLock"
        Me.picLock.Size = New System.Drawing.Size(20, 20)
        Me.picLock.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picLock.TabIndex = 48
        Me.picLock.TabStop = False
        Me.GlobalToolTip.SetToolTip(Me.picLock, "اين قسمت در نسخه بدون قرارداد پشتيباني و گارانتي معتبر فعال نمي‌باشد")
        Me.picLock.Visible = False
        '
        'txtUnConfirmReason
        '
        Me.txtUnConfirmReason.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtUnConfirmReason.CaptinText = ""
        Me.txtUnConfirmReason.HasCaption = False
        Me.txtUnConfirmReason.IsForceText = False
        Me.txtUnConfirmReason.IsFractional = True
        Me.txtUnConfirmReason.IsIP = False
        Me.txtUnConfirmReason.IsNumberOnly = False
        Me.txtUnConfirmReason.IsYear = False
        Me.txtUnConfirmReason.Location = New System.Drawing.Point(61, 87)
        Me.txtUnConfirmReason.Multiline = True
        Me.txtUnConfirmReason.Name = "txtUnConfirmReason"
        Me.txtUnConfirmReason.Size = New System.Drawing.Size(232, 62)
        Me.txtUnConfirmReason.TabIndex = 3
        '
        'rbIsConfirm
        '
        Me.rbIsConfirm.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rbIsConfirm.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.rbIsConfirm.Location = New System.Drawing.Point(237, 55)
        Me.rbIsConfirm.Name = "rbIsConfirm"
        Me.rbIsConfirm.Size = New System.Drawing.Size(56, 18)
        Me.rbIsConfirm.TabIndex = 1
        Me.rbIsConfirm.Text = "تأييد"
        '
        'rbIsNotConfirm
        '
        Me.rbIsNotConfirm.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rbIsNotConfirm.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.rbIsNotConfirm.Location = New System.Drawing.Point(165, 55)
        Me.rbIsNotConfirm.Name = "rbIsNotConfirm"
        Me.rbIsNotConfirm.Size = New System.Drawing.Size(72, 18)
        Me.rbIsNotConfirm.TabIndex = 2
        Me.rbIsNotConfirm.Text = "عدم تأييد"
        '
        'Label30
        '
        Me.Label30.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label30.AutoSize = True
        Me.Label30.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label30.Location = New System.Drawing.Point(301, 58)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(69, 13)
        Me.Label30.TabIndex = 0
        Me.Label30.Text = "وضعيت تأييد"
        '
        'lblNotConfirmReason
        '
        Me.lblNotConfirmReason.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblNotConfirmReason.AutoSize = True
        Me.lblNotConfirmReason.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblNotConfirmReason.Location = New System.Drawing.Point(301, 87)
        Me.lblNotConfirmReason.Name = "lblNotConfirmReason"
        Me.lblNotConfirmReason.Size = New System.Drawing.Size(77, 13)
        Me.lblNotConfirmReason.TabIndex = 0
        Me.lblNotConfirmReason.Text = "علت عدم تأييد"
        '
        'rbIsReturned
        '
        Me.rbIsReturned.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rbIsReturned.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.rbIsReturned.Location = New System.Drawing.Point(46, 55)
        Me.rbIsReturned.Name = "rbIsReturned"
        Me.rbIsReturned.Size = New System.Drawing.Size(112, 18)
        Me.rbIsReturned.TabIndex = 2
        Me.rbIsReturned.Text = "عودت درخواست"
        '
        'txtReturn
        '
        Me.txtReturn.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtReturn.Location = New System.Drawing.Point(3, 199)
        Me.txtReturn.Name = "txtReturn"
        Me.txtReturn.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtReturn.Size = New System.Drawing.Size(51, 21)
        Me.txtReturn.TabIndex = 53
        Me.txtReturn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtReturn.Visible = False
        '
        'LabelRetrun
        '
        Me.LabelRetrun.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LabelRetrun.AutoSize = True
        Me.LabelRetrun.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.LabelRetrun.Location = New System.Drawing.Point(57, 203)
        Me.LabelRetrun.Name = "LabelRetrun"
        Me.LabelRetrun.Size = New System.Drawing.Size(101, 13)
        Me.LabelRetrun.TabIndex = 52
        Me.LabelRetrun.Text = "مهلت ارسال مجدد"
        Me.LabelRetrun.Visible = False
        '
        'pnlAllow
        '
        Me.pnlAllow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlAllow.CaptionBackColor = System.Drawing.Color.Teal
        Me.pnlAllow.CaptionFont = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.pnlAllow.CaptionForeColor = System.Drawing.Color.White
        Me.pnlAllow.CaptionHeight = 16
        Me.pnlAllow.CaptionText = "اجازه کار"
        Me.pnlAllow.Controls.Add(Me.picIsAuotoNumber)
        Me.pnlAllow.Controls.Add(Me.chkIsAutoNumber)
        Me.pnlAllow.Controls.Add(Me.Label66)
        Me.pnlAllow.Controls.Add(Me.Label67)
        Me.pnlAllow.Controls.Add(Me.cmbAllowUserName)
        Me.pnlAllow.Controls.Add(Me.Label22)
        Me.pnlAllow.Controls.Add(Me.txtDateAllowDE)
        Me.pnlAllow.Controls.Add(Me.txtTimeAllowDE)
        Me.pnlAllow.Controls.Add(Me.Label19)
        Me.pnlAllow.Controls.Add(Me.txtAllowNumber)
        Me.pnlAllow.Controls.Add(Me.Label33)
        Me.pnlAllow.Controls.Add(Me.txtTimeAllowStart)
        Me.pnlAllow.Controls.Add(Me.txtDateAllowStart)
        Me.pnlAllow.Controls.Add(Me.Label35)
        Me.pnlAllow.Controls.Add(Me.txtDateAllowEnd)
        Me.pnlAllow.Controls.Add(Me.txtTimeAllowEnd)
        Me.pnlAllow.Controls.Add(Me.Label3)
        Me.pnlAllow.Enabled = False
        Me.pnlAllow.IsMoveable = False
        Me.pnlAllow.IsWindowMove = False
        Me.pnlAllow.Location = New System.Drawing.Point(12, 8)
        Me.pnlAllow.Name = "pnlAllow"
        Me.pnlAllow.Size = New System.Drawing.Size(292, 232)
        Me.pnlAllow.TabIndex = 1
        Me.pnlAllow.Visible = False
        '
        'picIsAuotoNumber
        '
        Me.picIsAuotoNumber.Image = CType(resources.GetObject("picIsAuotoNumber.Image"), System.Drawing.Image)
        Me.picIsAuotoNumber.Location = New System.Drawing.Point(169, 51)
        Me.picIsAuotoNumber.Name = "picIsAuotoNumber"
        Me.picIsAuotoNumber.Size = New System.Drawing.Size(12, 12)
        Me.picIsAuotoNumber.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picIsAuotoNumber.TabIndex = 11
        Me.picIsAuotoNumber.TabStop = False
        '
        'chkIsAutoNumber
        '
        Me.chkIsAutoNumber.Location = New System.Drawing.Point(24, 48)
        Me.chkIsAutoNumber.Name = "chkIsAutoNumber"
        Me.chkIsAutoNumber.Size = New System.Drawing.Size(176, 19)
        Me.chkIsAutoNumber.TabIndex = 10
        Me.chkIsAutoNumber.Text = "     توليد خودکار شماره اجازه کار"
        '
        'Label66
        '
        Me.Label66.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label66.AutoSize = True
        Me.Label66.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label66.ForeColor = System.Drawing.Color.Green
        Me.Label66.Location = New System.Drawing.Point(128, 102)
        Me.Label66.Name = "Label66"
        Me.Label66.Size = New System.Drawing.Size(105, 12)
        Me.Label66.TabIndex = 2
        Me.Label66.Text = "(زمان تحويل به پيمانکار)"
        '
        'Label67
        '
        Me.Label67.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label67.AutoSize = True
        Me.Label67.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label67.ForeColor = System.Drawing.Color.Green
        Me.Label67.Location = New System.Drawing.Point(128, 140)
        Me.Label67.Name = "Label67"
        Me.Label67.Size = New System.Drawing.Size(102, 12)
        Me.Label67.TabIndex = 2
        Me.Label67.Text = "(زمان تحويل از پيمانکار)"
        '
        'cmbAllowUserName
        '
        Me.cmbAllowUserName.BackColor = System.Drawing.Color.FromArgb(CType(CType(223, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(223, Byte), Integer))
        Me.cmbAllowUserName.DisplayMember = "UserName"
        Me.cmbAllowUserName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.cmbAllowUserName.IsReadOnly = True
        Me.cmbAllowUserName.Location = New System.Drawing.Point(8, 200)
        Me.cmbAllowUserName.Name = "cmbAllowUserName"
        Me.cmbAllowUserName.Size = New System.Drawing.Size(113, 21)
        Me.cmbAllowUserName.TabIndex = 7
        Me.cmbAllowUserName.ValueMember = "AreaUserId"
        '
        'Label22
        '
        Me.Label22.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label22.AutoSize = True
        Me.Label22.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label22.Location = New System.Drawing.Point(128, 202)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(131, 13)
        Me.Label22.TabIndex = 9
        Me.Label22.Text = "کاربر ثبت کننده اجازه کار"
        '
        'txtDateAllowDE
        '
        Me.txtDateAllowDE.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtDateAllowDE.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDateAllowDE.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDateAllowDE.IntegerDate = 0
        Me.txtDateAllowDE.IsShadow = False
        Me.txtDateAllowDE.IsShowCurrentDate = False
        Me.txtDateAllowDE.Location = New System.Drawing.Point(56, 160)
        Me.txtDateAllowDE.MaxLength = 10
        Me.txtDateAllowDE.MiladiDT = CType(resources.GetObject("txtDateAllowDE.MiladiDT"), Object)
        Me.txtDateAllowDE.Name = "txtDateAllowDE"
        Me.txtDateAllowDE.ReadOnly = True
        Me.txtDateAllowDE.ReadOnlyMaskedEdit = True
        Me.txtDateAllowDE.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDateAllowDE.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtDateAllowDE.ShamsiDT = "____/__/__"
        Me.txtDateAllowDE.Size = New System.Drawing.Size(66, 21)
        Me.txtDateAllowDE.TabIndex = 5
        Me.txtDateAllowDE.Text = "____/__/__"
        Me.txtDateAllowDE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtDateAllowDE.TimeMaskedEditorOBJ = Nothing
        '
        'txtTimeAllowDE
        '
        Me.txtTimeAllowDE.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtTimeAllowDE.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTimeAllowDE.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTimeAllowDE.IsShadow = False
        Me.txtTimeAllowDE.IsShowCurrentTime = False
        Me.txtTimeAllowDE.Location = New System.Drawing.Point(8, 160)
        Me.txtTimeAllowDE.MaxLength = 5
        Me.txtTimeAllowDE.MiladiDT = Nothing
        Me.txtTimeAllowDE.Name = "txtTimeAllowDE"
        Me.txtTimeAllowDE.ReadOnly = True
        Me.txtTimeAllowDE.ReadOnlyMaskedEdit = True
        Me.txtTimeAllowDE.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTimeAllowDE.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtTimeAllowDE.Size = New System.Drawing.Size(48, 21)
        Me.txtTimeAllowDE.TabIndex = 6
        Me.txtTimeAllowDE.Text = "__:__"
        Me.txtTimeAllowDE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label19
        '
        Me.Label19.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label19.Location = New System.Drawing.Point(128, 162)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(149, 13)
        Me.Label19.TabIndex = 8
        Me.Label19.Text = "تاريخ و ساعت ثبت اجازه کار"
        '
        'txtAllowNumber
        '
        Me.txtAllowNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAllowNumber.Location = New System.Drawing.Point(18, 22)
        Me.txtAllowNumber.Name = "txtAllowNumber"
        Me.txtAllowNumber.Size = New System.Drawing.Size(184, 21)
        Me.txtAllowNumber.TabIndex = 0
        Me.txtAllowNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label33
        '
        Me.Label33.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label33.AutoSize = True
        Me.Label33.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label33.Location = New System.Drawing.Point(128, 85)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(162, 13)
        Me.Label33.TabIndex = 0
        Me.Label33.Text = "تاريخ و ساعت شروع اجازه کار"
        '
        'txtTimeAllowStart
        '
        Me.txtTimeAllowStart.BackColor = System.Drawing.SystemColors.Window
        Me.txtTimeAllowStart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTimeAllowStart.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTimeAllowStart.IsShadow = False
        Me.txtTimeAllowStart.IsShowCurrentTime = False
        Me.txtTimeAllowStart.Location = New System.Drawing.Point(8, 88)
        Me.txtTimeAllowStart.MaxLength = 5
        Me.txtTimeAllowStart.MiladiDT = Nothing
        Me.txtTimeAllowStart.Name = "txtTimeAllowStart"
        Me.txtTimeAllowStart.ReadOnly = True
        Me.txtTimeAllowStart.ReadOnlyMaskedEdit = False
        Me.txtTimeAllowStart.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTimeAllowStart.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtTimeAllowStart.Size = New System.Drawing.Size(48, 21)
        Me.txtTimeAllowStart.TabIndex = 2
        Me.txtTimeAllowStart.Text = "__:__"
        Me.txtTimeAllowStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtDateAllowStart
        '
        Me.txtDateAllowStart.BackColor = System.Drawing.SystemColors.Window
        Me.txtDateAllowStart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDateAllowStart.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDateAllowStart.IntegerDate = 0
        Me.txtDateAllowStart.IsShadow = False
        Me.txtDateAllowStart.IsShowCurrentDate = False
        Me.txtDateAllowStart.Location = New System.Drawing.Point(56, 88)
        Me.txtDateAllowStart.MaxLength = 10
        Me.txtDateAllowStart.MiladiDT = CType(resources.GetObject("txtDateAllowStart.MiladiDT"), Object)
        Me.txtDateAllowStart.Name = "txtDateAllowStart"
        Me.txtDateAllowStart.ReadOnly = True
        Me.txtDateAllowStart.ReadOnlyMaskedEdit = False
        Me.txtDateAllowStart.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDateAllowStart.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtDateAllowStart.ShamsiDT = "____/__/__"
        Me.txtDateAllowStart.Size = New System.Drawing.Size(66, 21)
        Me.txtDateAllowStart.TabIndex = 1
        Me.txtDateAllowStart.Text = "____/__/__"
        Me.txtDateAllowStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtDateAllowStart.TimeMaskedEditorOBJ = Nothing
        '
        'Label35
        '
        Me.Label35.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label35.AutoSize = True
        Me.Label35.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label35.Location = New System.Drawing.Point(186, 24)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(89, 13)
        Me.Label35.TabIndex = 0
        Me.Label35.Text = "شماره اجازه کار"
        '
        'txtDateAllowEnd
        '
        Me.txtDateAllowEnd.BackColor = System.Drawing.SystemColors.Window
        Me.txtDateAllowEnd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDateAllowEnd.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDateAllowEnd.IntegerDate = 0
        Me.txtDateAllowEnd.IsShadow = False
        Me.txtDateAllowEnd.IsShowCurrentDate = False
        Me.txtDateAllowEnd.Location = New System.Drawing.Point(56, 128)
        Me.txtDateAllowEnd.MaxLength = 10
        Me.txtDateAllowEnd.MiladiDT = CType(resources.GetObject("txtDateAllowEnd.MiladiDT"), Object)
        Me.txtDateAllowEnd.Name = "txtDateAllowEnd"
        Me.txtDateAllowEnd.ReadOnly = True
        Me.txtDateAllowEnd.ReadOnlyMaskedEdit = False
        Me.txtDateAllowEnd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDateAllowEnd.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtDateAllowEnd.ShamsiDT = "____/__/__"
        Me.txtDateAllowEnd.Size = New System.Drawing.Size(66, 21)
        Me.txtDateAllowEnd.TabIndex = 3
        Me.txtDateAllowEnd.Text = "____/__/__"
        Me.txtDateAllowEnd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtDateAllowEnd.TimeMaskedEditorOBJ = Nothing
        '
        'txtTimeAllowEnd
        '
        Me.txtTimeAllowEnd.BackColor = System.Drawing.SystemColors.Window
        Me.txtTimeAllowEnd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTimeAllowEnd.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTimeAllowEnd.IsShadow = False
        Me.txtTimeAllowEnd.IsShowCurrentTime = False
        Me.txtTimeAllowEnd.Location = New System.Drawing.Point(8, 128)
        Me.txtTimeAllowEnd.MaxLength = 5
        Me.txtTimeAllowEnd.MiladiDT = Nothing
        Me.txtTimeAllowEnd.Name = "txtTimeAllowEnd"
        Me.txtTimeAllowEnd.ReadOnly = True
        Me.txtTimeAllowEnd.ReadOnlyMaskedEdit = False
        Me.txtTimeAllowEnd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTimeAllowEnd.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtTimeAllowEnd.Size = New System.Drawing.Size(48, 21)
        Me.txtTimeAllowEnd.TabIndex = 4
        Me.txtTimeAllowEnd.Text = "__:__"
        Me.txtTimeAllowEnd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label3.Location = New System.Drawing.Point(128, 124)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(155, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "تاريخ و ساعت پايان اجازه کار"
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.btnSave.Image = CType(resources.GetObject("btnSave.Image"), System.Drawing.Image)
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSave.Location = New System.Drawing.Point(608, 568)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(136, 24)
        Me.btnSave.TabIndex = 2
        Me.btnSave.Text = "ذخيره"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnReturn
        '
        Me.btnReturn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnReturn.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnReturn.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.btnReturn.Image = CType(resources.GetObject("btnReturn.Image"), System.Drawing.Image)
        Me.btnReturn.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnReturn.Location = New System.Drawing.Point(8, 568)
        Me.btnReturn.Name = "btnReturn"
        Me.btnReturn.Size = New System.Drawing.Size(136, 24)
        Me.btnReturn.TabIndex = 3
        Me.btnReturn.Text = "بازگشت"
        Me.btnReturn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbcMain
        '
        Me.tbcMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbcMain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.tbcMain.Controls.Add(Me.tp6_Docs)
        Me.tbcMain.Controls.Add(Me.tp5_nDCFeeder)
        Me.tbcMain.Controls.Add(Me.tp4_Confirms)
        Me.tbcMain.Controls.Add(Me.tp3_OperationList)
        Me.tbcMain.Controls.Add(Me.tp2_DCLocation)
        Me.tbcMain.Controls.Add(Me.tp1_Specs)
        Me.tbcMain.ItemSize = New System.Drawing.Size(112, 18)
        Me.tbcMain.Location = New System.Drawing.Point(8, 48)
        Me.tbcMain.Name = "tbcMain"
        Me.tbcMain.SelectedIndex = 0
        Me.tbcMain.Size = New System.Drawing.Size(736, 512)
        Me.tbcMain.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.tbcMain.TabIndex = 0
        '
        'tp6_Docs
        '
        Me.tp6_Docs.Controls.Add(Me.dgFiles)
        Me.tp6_Docs.Controls.Add(Me.grpNewFile)
        Me.tp6_Docs.Location = New System.Drawing.Point(4, 22)
        Me.tp6_Docs.Name = "tp6_Docs"
        Me.tp6_Docs.Padding = New System.Windows.Forms.Padding(3)
        Me.tp6_Docs.Size = New System.Drawing.Size(728, 486)
        Me.tp6_Docs.TabIndex = 5
        Me.tp6_Docs.Text = "مستندات خاموشي"
        Me.tp6_Docs.UseVisualStyleBackColor = True
        '
        'dgFiles
        '
        Me.dgFiles.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.[False]
        Me.dgFiles.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgFiles.BuiltInTextsData = resources.GetString("dgFiles.BuiltInTextsData")
        Me.dgFiles.CurrentRowIndex = -1
        Me.dgFiles.DataMember = "ViewTamirRequestFile"
        Me.dgFiles.DataSource = Me.DatasetTamir1
        dgFiles_DesignTimeLayout.LayoutString = resources.GetString("dgFiles_DesignTimeLayout.LayoutString")
        Me.dgFiles.DesignTimeLayout = dgFiles_DesignTimeLayout
        Me.dgFiles.EnableFormatEvent = True
        Me.dgFiles.EnableSaveLayout = True
        Me.dgFiles.GroupByBoxFormatStyle.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.dgFiles.GroupByBoxInfoFormatStyle.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.dgFiles.GroupByBoxVisible = False
        Me.dgFiles.IsColor = False
        Me.dgFiles.IsColumnContextMenu = True
        Me.dgFiles.IsForMonitoring = False
        Me.dgFiles.Location = New System.Drawing.Point(10, 125)
        Me.dgFiles.Name = "dgFiles"
        Me.dgFiles.PrintLandScape = True
        Me.dgFiles.SaveGridName = ""
        Me.dgFiles.Size = New System.Drawing.Size(705, 340)
        Me.dgFiles.TabIndex = 1
        '
        'DatasetTamir1
        '
        Me.DatasetTamir1.DataSetName = "DatasetTamir"
        Me.DatasetTamir1.Locale = New System.Globalization.CultureInfo("en-US")
        Me.DatasetTamir1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'grpNewFile
        '
        Me.grpNewFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpNewFile.Controls.Add(Me.btnAddFile)
        Me.grpNewFile.Controls.Add(Me.btnBrowseForFileUpload)
        Me.grpNewFile.Controls.Add(Me.txtSubject)
        Me.grpNewFile.Controls.Add(Me.txtUploadFilePath)
        Me.grpNewFile.Controls.Add(Me.Label117)
        Me.grpNewFile.Controls.Add(Me.Label116)
        Me.grpNewFile.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.grpNewFile.Location = New System.Drawing.Point(10, 8)
        Me.grpNewFile.Name = "grpNewFile"
        Me.grpNewFile.Size = New System.Drawing.Size(705, 111)
        Me.grpNewFile.TabIndex = 0
        Me.grpNewFile.TabStop = False
        Me.grpNewFile.Text = "فايل مستند جديد"
        '
        'btnAddFile
        '
        Me.btnAddFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAddFile.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnAddFile.Location = New System.Drawing.Point(10, 81)
        Me.btnAddFile.Name = "btnAddFile"
        Me.btnAddFile.Size = New System.Drawing.Size(210, 24)
        Me.btnAddFile.TabIndex = 2
        Me.btnAddFile.Text = "اضافه نمودن فايل به مستندات خاموشي"
        Me.btnAddFile.UseVisualStyleBackColor = True
        '
        'btnBrowseForFileUpload
        '
        Me.btnBrowseForFileUpload.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnBrowseForFileUpload.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnBrowseForFileUpload.Location = New System.Drawing.Point(10, 18)
        Me.btnBrowseForFileUpload.Name = "btnBrowseForFileUpload"
        Me.btnBrowseForFileUpload.Size = New System.Drawing.Size(88, 24)
        Me.btnBrowseForFileUpload.TabIndex = 2
        Me.btnBrowseForFileUpload.Text = "انتخاب فايل ..."
        Me.btnBrowseForFileUpload.UseVisualStyleBackColor = True
        '
        'txtSubject
        '
        Me.txtSubject.Location = New System.Drawing.Point(10, 49)
        Me.txtSubject.MaxLength = 200
        Me.txtSubject.Name = "txtSubject"
        Me.txtSubject.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtSubject.Size = New System.Drawing.Size(616, 21)
        Me.txtSubject.TabIndex = 1
        '
        'txtUploadFilePath
        '
        Me.txtUploadFilePath.Location = New System.Drawing.Point(100, 20)
        Me.txtUploadFilePath.Name = "txtUploadFilePath"
        Me.txtUploadFilePath.ReadOnly = True
        Me.txtUploadFilePath.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtUploadFilePath.Size = New System.Drawing.Size(440, 21)
        Me.txtUploadFilePath.TabIndex = 1
        '
        'Label117
        '
        Me.Label117.AutoSize = True
        Me.Label117.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label117.Location = New System.Drawing.Point(628, 53)
        Me.Label117.Name = "Label117"
        Me.Label117.Size = New System.Drawing.Size(67, 13)
        Me.Label117.TabIndex = 0
        Me.Label117.Text = "موضوع سند:"
        '
        'Label116
        '
        Me.Label116.AutoSize = True
        Me.Label116.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label116.Location = New System.Drawing.Point(550, 24)
        Me.Label116.Name = "Label116"
        Me.Label116.Size = New System.Drawing.Size(146, 13)
        Me.Label116.TabIndex = 0
        Me.Label116.Text = "مسير فايل مورد نظر براي سند"
        '
        'tp5_nDCFeeder
        '
        Me.tp5_nDCFeeder.Controls.Add(Me.cmbPeriod)
        Me.tp5_nDCFeeder.Controls.Add(Me.pnlDate)
        Me.tp5_nDCFeeder.Controls.Add(Me.btnLoadFeederDC)
        Me.tp5_nDCFeeder.Controls.Add(Me.pnlNetworkType)
        Me.tp5_nDCFeeder.Controls.Add(Me.pnlDCDayCount)
        Me.tp5_nDCFeeder.Controls.Add(Me.pnlLastDCCount)
        Me.tp5_nDCFeeder.Controls.Add(Me.dgFeederDC)
        Me.tp5_nDCFeeder.Location = New System.Drawing.Point(4, 22)
        Me.tp5_nDCFeeder.Name = "tp5_nDCFeeder"
        Me.tp5_nDCFeeder.Size = New System.Drawing.Size(728, 486)
        Me.tp5_nDCFeeder.TabIndex = 4
        Me.tp5_nDCFeeder.Text = "خاموشي‌هاي فيدر"
        '
        'cmbPeriod
        '
        Me.cmbPeriod.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbPeriod.BackColor = System.Drawing.Color.White
        Me.cmbPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbPeriod.Location = New System.Drawing.Point(344, 431)
        Me.cmbPeriod.Name = "cmbPeriod"
        Me.cmbPeriod.Size = New System.Drawing.Size(136, 21)
        Me.cmbPeriod.TabIndex = 34
        Me.ToolTip1.SetToolTip(Me.cmbPeriod, "دوره گزارش")
        '
        'pnlDate
        '
        Me.pnlDate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlDate.Controls.Add(Me.Label52)
        Me.pnlDate.Controls.Add(Me.pnlToDate)
        Me.pnlDate.Controls.Add(Me.txtDateFrom)
        Me.pnlDate.Location = New System.Drawing.Point(488, 429)
        Me.pnlDate.Name = "pnlDate"
        Me.pnlDate.Size = New System.Drawing.Size(232, 24)
        Me.pnlDate.TabIndex = 33
        '
        'Label52
        '
        Me.Label52.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label52.AutoSize = True
        Me.Label52.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label52.Location = New System.Drawing.Point(176, 5)
        Me.Label52.Name = "Label52"
        Me.Label52.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label52.Size = New System.Drawing.Size(60, 13)
        Me.Label52.TabIndex = 15
        Me.Label52.Text = "از تاريخ قطع"
        '
        'pnlToDate
        '
        Me.pnlToDate.Controls.Add(Me.txtDateTo)
        Me.pnlToDate.Controls.Add(Me.Label53)
        Me.pnlToDate.Location = New System.Drawing.Point(0, 2)
        Me.pnlToDate.Name = "pnlToDate"
        Me.pnlToDate.Size = New System.Drawing.Size(96, 20)
        Me.pnlToDate.TabIndex = 3
        '
        'txtDateTo
        '
        Me.txtDateTo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDateTo.BackColor = System.Drawing.SystemColors.Window
        Me.txtDateTo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDateTo.IntegerDate = 0
        Me.txtDateTo.IsShadow = False
        Me.txtDateTo.IsShowCurrentDate = False
        Me.txtDateTo.Location = New System.Drawing.Point(0, 0)
        Me.txtDateTo.MaxLength = 10
        Me.txtDateTo.MiladiDT = CType(resources.GetObject("txtDateTo.MiladiDT"), Object)
        Me.txtDateTo.Name = "txtDateTo"
        Me.txtDateTo.ReadOnly = True
        Me.txtDateTo.ReadOnlyMaskedEdit = False
        Me.txtDateTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDateTo.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtDateTo.ShamsiDT = "____/__/__"
        Me.txtDateTo.Size = New System.Drawing.Size(72, 21)
        Me.txtDateTo.TabIndex = 0
        Me.txtDateTo.Text = "____/__/__"
        Me.txtDateTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtDateTo.TimeMaskedEditorOBJ = Nothing
        '
        'Label53
        '
        Me.Label53.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label53.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label53.Location = New System.Drawing.Point(80, 3)
        Me.Label53.Name = "Label53"
        Me.Label53.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label53.Size = New System.Drawing.Size(12, 14)
        Me.Label53.TabIndex = 15
        Me.Label53.Text = "تا"
        Me.Label53.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtDateFrom
        '
        Me.txtDateFrom.BackColor = System.Drawing.SystemColors.Window
        Me.txtDateFrom.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDateFrom.IntegerDate = 0
        Me.txtDateFrom.IsShadow = False
        Me.txtDateFrom.IsShowCurrentDate = False
        Me.txtDateFrom.Location = New System.Drawing.Point(98, 2)
        Me.txtDateFrom.MaxLength = 10
        Me.txtDateFrom.MiladiDT = CType(resources.GetObject("txtDateFrom.MiladiDT"), Object)
        Me.txtDateFrom.Name = "txtDateFrom"
        Me.txtDateFrom.ReadOnly = True
        Me.txtDateFrom.ReadOnlyMaskedEdit = False
        Me.txtDateFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDateFrom.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtDateFrom.ShamsiDT = "____/__/__"
        Me.txtDateFrom.Size = New System.Drawing.Size(72, 21)
        Me.txtDateFrom.TabIndex = 0
        Me.txtDateFrom.Text = "____/__/__"
        Me.txtDateFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtDateFrom.TimeMaskedEditorOBJ = Nothing
        '
        'btnLoadFeederDC
        '
        Me.btnLoadFeederDC.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnLoadFeederDC.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.btnLoadFeederDC.Location = New System.Drawing.Point(8, 456)
        Me.btnLoadFeederDC.Name = "btnLoadFeederDC"
        Me.btnLoadFeederDC.Size = New System.Drawing.Size(56, 24)
        Me.btnLoadFeederDC.TabIndex = 6
        Me.btnLoadFeederDC.Text = "جستجو"
        '
        'pnlNetworkType
        '
        Me.pnlNetworkType.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlNetworkType.Controls.Add(Me.rbNT_Tamir)
        Me.pnlNetworkType.Controls.Add(Me.rbNT_NotTamir)
        Me.pnlNetworkType.Controls.Add(Me.rbNT_Both)
        Me.pnlNetworkType.Location = New System.Drawing.Point(528, 456)
        Me.pnlNetworkType.Name = "pnlNetworkType"
        Me.pnlNetworkType.Size = New System.Drawing.Size(192, 24)
        Me.pnlNetworkType.TabIndex = 35
        '
        'rbNT_Tamir
        '
        Me.rbNT_Tamir.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rbNT_Tamir.Location = New System.Drawing.Point(128, 0)
        Me.rbNT_Tamir.Name = "rbNT_Tamir"
        Me.rbNT_Tamir.Size = New System.Drawing.Size(64, 24)
        Me.rbNT_Tamir.TabIndex = 0
        Me.rbNT_Tamir.Text = "بابرنامه"
        '
        'rbNT_NotTamir
        '
        Me.rbNT_NotTamir.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rbNT_NotTamir.Location = New System.Drawing.Point(56, 0)
        Me.rbNT_NotTamir.Name = "rbNT_NotTamir"
        Me.rbNT_NotTamir.Size = New System.Drawing.Size(64, 24)
        Me.rbNT_NotTamir.TabIndex = 0
        Me.rbNT_NotTamir.Text = "بي‌برنامه"
        '
        'rbNT_Both
        '
        Me.rbNT_Both.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rbNT_Both.Checked = True
        Me.rbNT_Both.Location = New System.Drawing.Point(-22, 0)
        Me.rbNT_Both.Name = "rbNT_Both"
        Me.rbNT_Both.Size = New System.Drawing.Size(70, 24)
        Me.rbNT_Both.TabIndex = 0
        Me.rbNT_Both.TabStop = True
        Me.rbNT_Both.Text = "هر دو"
        '
        'pnlDCDayCount
        '
        Me.pnlDCDayCount.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlDCDayCount.Controls.Add(Me.txtFeederDCDayCount)
        Me.pnlDCDayCount.Controls.Add(Me.Label39)
        Me.pnlDCDayCount.Controls.Add(Me.Label38)
        Me.pnlDCDayCount.Location = New System.Drawing.Point(104, 429)
        Me.pnlDCDayCount.Name = "pnlDCDayCount"
        Me.pnlDCDayCount.Size = New System.Drawing.Size(232, 24)
        Me.pnlDCDayCount.TabIndex = 35
        Me.pnlDCDayCount.Visible = False
        '
        'txtFeederDCDayCount
        '
        Me.txtFeederDCDayCount.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFeederDCDayCount.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtFeederDCDayCount.CaptinText = ""
        Me.txtFeederDCDayCount.HasCaption = False
        Me.txtFeederDCDayCount.IsForceText = False
        Me.txtFeederDCDayCount.IsFractional = False
        Me.txtFeederDCDayCount.IsIP = False
        Me.txtFeederDCDayCount.IsNumberOnly = True
        Me.txtFeederDCDayCount.IsYear = False
        Me.txtFeederDCDayCount.Location = New System.Drawing.Point(79, 5)
        Me.txtFeederDCDayCount.MaxLength = 3
        Me.txtFeederDCDayCount.Name = "txtFeederDCDayCount"
        Me.txtFeederDCDayCount.Size = New System.Drawing.Size(48, 14)
        Me.txtFeederDCDayCount.TabIndex = 5
        Me.txtFeederDCDayCount.Text = "30"
        Me.txtFeederDCDayCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label39
        '
        Me.Label39.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label39.AutoSize = True
        Me.Label39.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label39.Location = New System.Drawing.Point(1, 4)
        Me.Label39.Name = "Label39"
        Me.Label39.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Label39.Size = New System.Drawing.Size(77, 13)
        Me.Label39.TabIndex = 1
        Me.Label39.Text = "روز گذشته فيدر"
        '
        'Label38
        '
        Me.Label38.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label38.AutoSize = True
        Me.Label38.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label38.Location = New System.Drawing.Point(134, 4)
        Me.Label38.Name = "Label38"
        Me.Label38.Size = New System.Drawing.Size(100, 13)
        Me.Label38.TabIndex = 1
        Me.Label38.Text = "آخرين خاموشي‌هاي"
        '
        'pnlLastDCCount
        '
        Me.pnlLastDCCount.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlLastDCCount.Controls.Add(Me.txtLastDCCount)
        Me.pnlLastDCCount.Controls.Add(Me.Label54)
        Me.pnlLastDCCount.Controls.Add(Me.Label55)
        Me.pnlLastDCCount.Location = New System.Drawing.Point(104, 429)
        Me.pnlLastDCCount.Name = "pnlLastDCCount"
        Me.pnlLastDCCount.Size = New System.Drawing.Size(232, 24)
        Me.pnlLastDCCount.TabIndex = 35
        Me.pnlLastDCCount.Visible = False
        '
        'txtLastDCCount
        '
        Me.txtLastDCCount.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLastDCCount.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtLastDCCount.CaptinText = ""
        Me.txtLastDCCount.HasCaption = False
        Me.txtLastDCCount.IsForceText = False
        Me.txtLastDCCount.IsFractional = False
        Me.txtLastDCCount.IsIP = False
        Me.txtLastDCCount.IsNumberOnly = True
        Me.txtLastDCCount.IsYear = False
        Me.txtLastDCCount.Location = New System.Drawing.Point(146, 5)
        Me.txtLastDCCount.MaxLength = 3
        Me.txtLastDCCount.Name = "txtLastDCCount"
        Me.txtLastDCCount.Size = New System.Drawing.Size(48, 14)
        Me.txtLastDCCount.TabIndex = 5
        Me.txtLastDCCount.Text = "5"
        Me.txtLastDCCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label54
        '
        Me.Label54.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label54.AutoSize = True
        Me.Label54.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label54.Location = New System.Drawing.Point(70, 4)
        Me.Label54.Name = "Label54"
        Me.Label54.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Label54.Size = New System.Drawing.Size(72, 13)
        Me.Label54.TabIndex = 1
        Me.Label54.Text = "خاموشي فيدر"
        '
        'Label55
        '
        Me.Label55.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label55.AutoSize = True
        Me.Label55.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label55.Location = New System.Drawing.Point(200, 4)
        Me.Label55.Name = "Label55"
        Me.Label55.Size = New System.Drawing.Size(33, 13)
        Me.Label55.TabIndex = 1
        Me.Label55.Text = "آخرين"
        '
        'dgFeederDC
        '
        Me.dgFeederDC.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.[False]
        Me.dgFeederDC.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgFeederDC.BuiltInTextsData = resources.GetString("dgFeederDC.BuiltInTextsData")
        Me.dgFeederDC.CurrentRowIndex = -1
        Me.dgFeederDC.DataSource = Me.mDsReqView.ViewMonitoring
        dgFeederDC_DesignTimeLayout.LayoutString = resources.GetString("dgFeederDC_DesignTimeLayout.LayoutString")
        Me.dgFeederDC.DesignTimeLayout = dgFeederDC_DesignTimeLayout
        Me.dgFeederDC.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular
        Me.dgFeederDC.EnableFormatEvent = True
        Me.dgFeederDC.EnableSaveLayout = True
        Me.dgFeederDC.GroupByBoxFormatStyle.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.dgFeederDC.GroupByBoxInfoFormatStyle.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.dgFeederDC.GroupByBoxVisible = False
        Me.dgFeederDC.HeaderFormatStyle.LineAlignment = Janus.Windows.GridEX.TextAlignment.Center
        Me.dgFeederDC.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center
        Me.dgFeederDC.IsColor = False
        Me.dgFeederDC.IsColumnContextMenu = True
        Me.dgFeederDC.IsForMonitoring = False
        Me.dgFeederDC.Location = New System.Drawing.Point(8, 7)
        Me.dgFeederDC.Name = "dgFeederDC"
        Me.dgFeederDC.PrintLandScape = True
        Me.dgFeederDC.RowFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center
        Me.dgFeederDC.SaveGridName = ""
        Me.dgFeederDC.Size = New System.Drawing.Size(712, 417)
        Me.dgFeederDC.TabIndex = 0
        Me.dgFeederDC.TotalRowFormatStyle.BackColor = System.Drawing.Color.AliceBlue
        Me.dgFeederDC.TotalRowFormatStyle.Font = New System.Drawing.Font("Tahoma", 6.75!)
        Me.dgFeederDC.TotalRowFormatStyle.ForeColor = System.Drawing.Color.Navy
        Me.dgFeederDC.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2003
        '
        'mDsReqView
        '
        Me.mDsReqView.DataSetName = "DatasetCcRequesterView"
        Me.mDsReqView.Locale = New System.Globalization.CultureInfo("en-US")
        Me.mDsReqView.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'tp4_Confirms
        '
        Me.tp4_Confirms.Controls.Add(Me.pnlConfirm)
        Me.tp4_Confirms.Controls.Add(Me.pnlAllow)
        Me.tp4_Confirms.Controls.Add(Me.pnlWarmLineConfirm)
        Me.tp4_Confirms.Location = New System.Drawing.Point(4, 22)
        Me.tp4_Confirms.Name = "tp4_Confirms"
        Me.tp4_Confirms.Size = New System.Drawing.Size(728, 486)
        Me.tp4_Confirms.TabIndex = 0
        Me.tp4_Confirms.Text = "تأييديه ها"
        '
        'pnlWarmLineConfirm
        '
        Me.pnlWarmLineConfirm.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlWarmLineConfirm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlWarmLineConfirm.CaptionBackColor = System.Drawing.Color.Teal
        Me.pnlWarmLineConfirm.CaptionFont = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.pnlWarmLineConfirm.CaptionForeColor = System.Drawing.Color.White
        Me.pnlWarmLineConfirm.CaptionHeight = 16
        Me.pnlWarmLineConfirm.CaptionText = "تأييديه خط گرم"
        Me.pnlWarmLineConfirm.Controls.Add(Me.rbIsReturnedWL)
        Me.pnlWarmLineConfirm.Controls.Add(Me.txtWarmLineReason)
        Me.pnlWarmLineConfirm.Controls.Add(Me.rbConfirmWL)
        Me.pnlWarmLineConfirm.Controls.Add(Me.rbNotConfirmWL)
        Me.pnlWarmLineConfirm.Controls.Add(Me.txtWarmLineConfirmUserName)
        Me.pnlWarmLineConfirm.Controls.Add(Me.Label45)
        Me.pnlWarmLineConfirm.Controls.Add(Me.txtWarmLineTime)
        Me.pnlWarmLineConfirm.Controls.Add(Me.txtWarmLineDate)
        Me.pnlWarmLineConfirm.Controls.Add(Me.Label111)
        Me.pnlWarmLineConfirm.Controls.Add(Me.Label112)
        Me.pnlWarmLineConfirm.Controls.Add(Me.Label113)
        Me.pnlWarmLineConfirm.Enabled = False
        Me.pnlWarmLineConfirm.IsMoveable = False
        Me.pnlWarmLineConfirm.IsWindowMove = False
        Me.pnlWarmLineConfirm.Location = New System.Drawing.Point(316, 246)
        Me.pnlWarmLineConfirm.Name = "pnlWarmLineConfirm"
        Me.pnlWarmLineConfirm.Size = New System.Drawing.Size(396, 232)
        Me.pnlWarmLineConfirm.TabIndex = 0
        '
        'rbIsReturnedWL
        '
        Me.rbIsReturnedWL.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rbIsReturnedWL.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.rbIsReturnedWL.Location = New System.Drawing.Point(32, 56)
        Me.rbIsReturnedWL.Name = "rbIsReturnedWL"
        Me.rbIsReturnedWL.Size = New System.Drawing.Size(112, 18)
        Me.rbIsReturnedWL.TabIndex = 3
        Me.rbIsReturnedWL.Text = "عودت درخواست"
        '
        'txtWarmLineReason
        '
        Me.txtWarmLineReason.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtWarmLineReason.CaptinText = ""
        Me.txtWarmLineReason.HasCaption = False
        Me.txtWarmLineReason.IsForceText = False
        Me.txtWarmLineReason.IsFractional = True
        Me.txtWarmLineReason.IsIP = False
        Me.txtWarmLineReason.IsNumberOnly = False
        Me.txtWarmLineReason.IsYear = False
        Me.txtWarmLineReason.Location = New System.Drawing.Point(44, 88)
        Me.txtWarmLineReason.Multiline = True
        Me.txtWarmLineReason.Name = "txtWarmLineReason"
        Me.txtWarmLineReason.Size = New System.Drawing.Size(232, 104)
        Me.txtWarmLineReason.TabIndex = 4
        '
        'rbConfirmWL
        '
        Me.rbConfirmWL.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rbConfirmWL.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.rbConfirmWL.Location = New System.Drawing.Point(220, 56)
        Me.rbConfirmWL.Name = "rbConfirmWL"
        Me.rbConfirmWL.Size = New System.Drawing.Size(56, 18)
        Me.rbConfirmWL.TabIndex = 1
        Me.rbConfirmWL.Text = "تأييد"
        '
        'rbNotConfirmWL
        '
        Me.rbNotConfirmWL.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rbNotConfirmWL.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.rbNotConfirmWL.Location = New System.Drawing.Point(148, 56)
        Me.rbNotConfirmWL.Name = "rbNotConfirmWL"
        Me.rbNotConfirmWL.Size = New System.Drawing.Size(72, 18)
        Me.rbNotConfirmWL.TabIndex = 2
        Me.rbNotConfirmWL.Text = "عدم تأييد"
        '
        'txtWarmLineConfirmUserName
        '
        Me.txtWarmLineConfirmUserName.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtWarmLineConfirmUserName.BackColor = System.Drawing.Color.FromArgb(CType(CType(223, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(223, Byte), Integer))
        Me.txtWarmLineConfirmUserName.Location = New System.Drawing.Point(149, 22)
        Me.txtWarmLineConfirmUserName.Name = "txtWarmLineConfirmUserName"
        Me.txtWarmLineConfirmUserName.ReadOnly = True
        Me.txtWarmLineConfirmUserName.Size = New System.Drawing.Size(132, 21)
        Me.txtWarmLineConfirmUserName.TabIndex = 0
        '
        'Label45
        '
        Me.Label45.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label45.AutoSize = True
        Me.Label45.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label45.Location = New System.Drawing.Point(284, 202)
        Me.Label45.Name = "Label45"
        Me.Label45.Size = New System.Drawing.Size(104, 13)
        Me.Label45.TabIndex = 0
        Me.Label45.Text = "تاريخ و ساعت تأييد"
        '
        'txtWarmLineTime
        '
        Me.txtWarmLineTime.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtWarmLineTime.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtWarmLineTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtWarmLineTime.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtWarmLineTime.IsShadow = False
        Me.txtWarmLineTime.IsShowCurrentTime = False
        Me.txtWarmLineTime.Location = New System.Drawing.Point(143, 200)
        Me.txtWarmLineTime.MaxLength = 5
        Me.txtWarmLineTime.MiladiDT = Nothing
        Me.txtWarmLineTime.Name = "txtWarmLineTime"
        Me.txtWarmLineTime.ReadOnly = True
        Me.txtWarmLineTime.ReadOnlyMaskedEdit = True
        Me.txtWarmLineTime.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtWarmLineTime.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtWarmLineTime.Size = New System.Drawing.Size(48, 21)
        Me.txtWarmLineTime.TabIndex = 5
        Me.txtWarmLineTime.Text = "__:__"
        Me.txtWarmLineTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtWarmLineDate
        '
        Me.txtWarmLineDate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtWarmLineDate.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtWarmLineDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtWarmLineDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtWarmLineDate.IntegerDate = 0
        Me.txtWarmLineDate.IsShadow = False
        Me.txtWarmLineDate.IsShowCurrentDate = False
        Me.txtWarmLineDate.Location = New System.Drawing.Point(191, 200)
        Me.txtWarmLineDate.MaxLength = 10
        Me.txtWarmLineDate.MiladiDT = CType(resources.GetObject("txtWarmLineDate.MiladiDT"), Object)
        Me.txtWarmLineDate.Name = "txtWarmLineDate"
        Me.txtWarmLineDate.ReadOnly = True
        Me.txtWarmLineDate.ReadOnlyMaskedEdit = True
        Me.txtWarmLineDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtWarmLineDate.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtWarmLineDate.ShamsiDT = "____/__/__"
        Me.txtWarmLineDate.Size = New System.Drawing.Size(85, 21)
        Me.txtWarmLineDate.TabIndex = 4
        Me.txtWarmLineDate.Text = "____/__/__"
        Me.txtWarmLineDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtWarmLineDate.TimeMaskedEditorOBJ = Nothing
        '
        'Label111
        '
        Me.Label111.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label111.AutoSize = True
        Me.Label111.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label111.Location = New System.Drawing.Point(284, 59)
        Me.Label111.Name = "Label111"
        Me.Label111.Size = New System.Drawing.Size(69, 13)
        Me.Label111.TabIndex = 0
        Me.Label111.Text = "وضعيت تأييد"
        '
        'Label112
        '
        Me.Label112.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label112.AutoSize = True
        Me.Label112.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label112.Location = New System.Drawing.Point(284, 24)
        Me.Label112.Name = "Label112"
        Me.Label112.Size = New System.Drawing.Size(86, 13)
        Me.Label112.TabIndex = 0
        Me.Label112.Text = "کاربر تأييد کننده"
        '
        'Label113
        '
        Me.Label113.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label113.AutoSize = True
        Me.Label113.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label113.Location = New System.Drawing.Point(284, 88)
        Me.Label113.Name = "Label113"
        Me.Label113.Size = New System.Drawing.Size(77, 13)
        Me.Label113.TabIndex = 0
        Me.Label113.Text = "علت عدم تأييد"
        '
        'tp3_OperationList
        '
        Me.tp3_OperationList.Controls.Add(Me.txtOperationDesc)
        Me.tp3_OperationList.Controls.Add(Me.Label108)
        Me.tp3_OperationList.Controls.Add(Me.dgOperations)
        Me.tp3_OperationList.Controls.Add(Me.Label29)
        Me.tp3_OperationList.Controls.Add(Me.txtSumTime)
        Me.tp3_OperationList.Controls.Add(Me.Label40)
        Me.tp3_OperationList.Location = New System.Drawing.Point(4, 22)
        Me.tp3_OperationList.Name = "tp3_OperationList"
        Me.tp3_OperationList.Size = New System.Drawing.Size(728, 486)
        Me.tp3_OperationList.TabIndex = 1
        Me.tp3_OperationList.Text = "فهرست عمليات"
        '
        'txtOperationDesc
        '
        Me.txtOperationDesc.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtOperationDesc.CaptinText = ""
        Me.txtOperationDesc.HasCaption = False
        Me.txtOperationDesc.IsForceText = False
        Me.txtOperationDesc.IsFractional = True
        Me.txtOperationDesc.IsIP = False
        Me.txtOperationDesc.IsNumberOnly = False
        Me.txtOperationDesc.IsYear = False
        Me.txtOperationDesc.Location = New System.Drawing.Point(317, 458)
        Me.txtOperationDesc.MaxLength = 50
        Me.txtOperationDesc.Multiline = True
        Me.txtOperationDesc.Name = "txtOperationDesc"
        Me.txtOperationDesc.Size = New System.Drawing.Size(344, 21)
        Me.txtOperationDesc.TabIndex = 8
        '
        'Label108
        '
        Me.Label108.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label108.AutoSize = True
        Me.Label108.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label108.Location = New System.Drawing.Point(664, 460)
        Me.Label108.Name = "Label108"
        Me.Label108.Size = New System.Drawing.Size(53, 13)
        Me.Label108.TabIndex = 7
        Me.Label108.Text = "توضيحات"
        '
        'dgOperations
        '
        Me.dgOperations.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgOperations.BuiltInTextsData = resources.GetString("dgOperations.BuiltInTextsData")
        Me.dgOperations.CurrentRowIndex = -1
        Me.dgOperations.DataSource = Me.DatasetTamirView1.ViewTamirOperationList
        dgOperations_DesignTimeLayout.LayoutString = resources.GetString("dgOperations_DesignTimeLayout.LayoutString")
        Me.dgOperations.DesignTimeLayout = dgOperations_DesignTimeLayout
        Me.dgOperations.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular
        Me.dgOperations.EnableFormatEvent = True
        Me.dgOperations.EnableSaveLayout = True
        Me.dgOperations.GroupByBoxFormatStyle.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.dgOperations.GroupByBoxInfoFormatStyle.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.dgOperations.GroupByBoxVisible = False
        Me.dgOperations.HeaderFormatStyle.FontSize = 8.0!
        Me.dgOperations.HeaderFormatStyle.LineAlignment = Janus.Windows.GridEX.TextAlignment.Center
        Me.dgOperations.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center
        Me.dgOperations.IsColor = False
        Me.dgOperations.IsColumnContextMenu = True
        Me.dgOperations.IsForMonitoring = False
        Me.dgOperations.Location = New System.Drawing.Point(8, 7)
        Me.dgOperations.Name = "dgOperations"
        Me.dgOperations.PrintLandScape = True
        Me.dgOperations.SaveGridName = ""
        Me.dgOperations.Size = New System.Drawing.Size(712, 441)
        Me.dgOperations.TabIndex = 5
        '
        'DatasetTamirView1
        '
        Me.DatasetTamirView1.DataSetName = "DatasetTamirView"
        Me.DatasetTamirView1.Locale = New System.Globalization.CultureInfo("en-US")
        Me.DatasetTamirView1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'Label29
        '
        Me.Label29.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label29.AutoSize = True
        Me.Label29.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label29.Location = New System.Drawing.Point(120, 460)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(192, 13)
        Me.Label29.TabIndex = 3
        Me.Label29.Text = "حداکثر زمان مورد نياز براي انجام کار"
        '
        'txtSumTime
        '
        Me.txtSumTime.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtSumTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSumTime.Location = New System.Drawing.Point(48, 458)
        Me.txtSumTime.Name = "txtSumTime"
        Me.txtSumTime.ReadOnly = True
        Me.txtSumTime.Size = New System.Drawing.Size(64, 21)
        Me.txtSumTime.TabIndex = 4
        Me.txtSumTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label40
        '
        Me.Label40.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label40.AutoSize = True
        Me.Label40.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label40.Location = New System.Drawing.Point(8, 460)
        Me.Label40.Name = "Label40"
        Me.Label40.Size = New System.Drawing.Size(36, 13)
        Me.Label40.TabIndex = 3
        Me.Label40.Text = "دقيقه"
        '
        'tp2_DCLocation
        '
        Me.tp2_DCLocation.Controls.Add(Me.picFeederPart)
        Me.tp2_DCLocation.Controls.Add(Me.txtFeederPart)
        Me.tp2_DCLocation.Controls.Add(Me.btnLPFeeder)
        Me.tp2_DCLocation.Controls.Add(Me.pnlPeymankar)
        Me.tp2_DCLocation.Controls.Add(Me.lblPeymankar)
        Me.tp2_DCLocation.Controls.Add(Me.pnlTamirRequestInfo)
        Me.tp2_DCLocation.Controls.Add(Me.pnlManoeuvreRequestInfo)
        Me.tp2_DCLocation.Controls.Add(Me.tbcDCs)
        Me.tp2_DCLocation.Controls.Add(Me.lblLetterInfo)
        Me.tp2_DCLocation.Controls.Add(Me.pnlManoeuvre)
        Me.tp2_DCLocation.Controls.Add(Me.pnlAdsress)
        Me.tp2_DCLocation.Controls.Add(Me.pnlInfo)
        Me.tp2_DCLocation.Controls.Add(Me.pnlPostFeeder)
        Me.tp2_DCLocation.Controls.Add(Me.sbtnOverlaps)
        Me.tp2_DCLocation.Controls.Add(Me.pnlManoeuvreDesc)
        Me.tp2_DCLocation.Controls.Add(Me.pnlLetterInfo)
        Me.tp2_DCLocation.Controls.Add(Me.pnlMultiStepInfo)
        Me.tp2_DCLocation.Location = New System.Drawing.Point(4, 22)
        Me.tp2_DCLocation.Name = "tp2_DCLocation"
        Me.tp2_DCLocation.Size = New System.Drawing.Size(728, 486)
        Me.tp2_DCLocation.TabIndex = 2
        Me.tp2_DCLocation.Text = "مشخصات محل  قطع"
        '
        'picFeederPart
        '
        Me.picFeederPart.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picFeederPart.Cursor = System.Windows.Forms.Cursors.Hand
        Me.picFeederPart.Enabled = False
        Me.picFeederPart.Image = CType(resources.GetObject("picFeederPart.Image"), System.Drawing.Image)
        Me.picFeederPart.Location = New System.Drawing.Point(410, 404)
        Me.picFeederPart.Name = "picFeederPart"
        Me.picFeederPart.Size = New System.Drawing.Size(16, 16)
        Me.picFeederPart.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picFeederPart.TabIndex = 194
        Me.picFeederPart.TabStop = False
        Me.GlobalToolTip.SetToolTip(Me.picFeederPart, "جستجوي خاموشي‌هاي همزمان فوق توزيع روي فيدر در بازه تاريخ مشخض شده")
        '
        'txtFeederPart
        '
        Me.txtFeederPart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtFeederPart.CaptinText = "پيمانکار"
        Me.txtFeederPart.HasCaption = False
        Me.txtFeederPart.IsForceText = False
        Me.txtFeederPart.IsFractional = True
        Me.txtFeederPart.IsIP = False
        Me.txtFeederPart.IsNumberOnly = False
        Me.txtFeederPart.IsYear = False
        Me.txtFeederPart.Location = New System.Drawing.Point(410, 381)
        Me.txtFeederPart.Name = "txtFeederPart"
        Me.txtFeederPart.Size = New System.Drawing.Size(102, 21)
        Me.txtFeederPart.TabIndex = 193
        Me.txtFeederPart.Visible = False
        '
        'btnLPFeeder
        '
        Me.btnLPFeeder.Enabled = False
        Me.btnLPFeeder.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnLPFeeder.Location = New System.Drawing.Point(408, 432)
        Me.btnLPFeeder.Name = "btnLPFeeder"
        Me.btnLPFeeder.Size = New System.Drawing.Size(21, 20)
        Me.btnLPFeeder.TabIndex = 192
        Me.btnLPFeeder.Text = "..."
        '
        'pnlPeymankar
        '
        Me.pnlPeymankar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlPeymankar.CaptionBackColor = System.Drawing.Color.Blue
        Me.pnlPeymankar.CaptionFont = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.pnlPeymankar.CaptionForeColor = System.Drawing.Color.White
        Me.pnlPeymankar.CaptionHeight = 16
        Me.pnlPeymankar.CaptionText = "پيمانکار"
        Me.pnlPeymankar.Controls.Add(Me.btnPeymankarTel)
        Me.pnlPeymankar.Controls.Add(Me.btnPeymankar)
        Me.pnlPeymankar.Controls.Add(Me.txtPeymankarTel)
        Me.pnlPeymankar.Controls.Add(Me.Label120)
        Me.pnlPeymankar.Controls.Add(Me.lblPeymankarManager)
        Me.pnlPeymankar.Controls.Add(Me.Label34)
        Me.pnlPeymankar.IsMoveable = True
        Me.pnlPeymankar.IsWindowMove = False
        Me.pnlPeymankar.Location = New System.Drawing.Point(308, 19)
        Me.pnlPeymankar.Name = "pnlPeymankar"
        Me.pnlPeymankar.Size = New System.Drawing.Size(302, 106)
        Me.pnlPeymankar.TabIndex = 189
        Me.pnlPeymankar.Visible = False
        '
        'btnPeymankarTel
        '
        Me.btnPeymankarTel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPeymankarTel.Image = Global.Havades_App.Resources.phone
        Me.btnPeymankarTel.Location = New System.Drawing.Point(16, 50)
        Me.btnPeymankarTel.Name = "btnPeymankarTel"
        Me.btnPeymankarTel.Size = New System.Drawing.Size(21, 21)
        Me.btnPeymankarTel.TabIndex = 7
        Me.GlobalToolTip.SetToolTip(Me.btnPeymankarTel, "دريافت شماره موبايل از اطلاعات پايه پيمانکار")
        Me.btnPeymankarTel.UseVisualStyleBackColor = True
        '
        'btnPeymankar
        '
        Me.btnPeymankar.BackColor = System.Drawing.Color.Yellow
        Me.btnPeymankar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPeymankar.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.btnPeymankar.Location = New System.Drawing.Point(15, 75)
        Me.btnPeymankar.Name = "btnPeymankar"
        Me.btnPeymankar.Size = New System.Drawing.Size(128, 23)
        Me.btnPeymankar.TabIndex = 6
        Me.btnPeymankar.Text = "تأييد"
        Me.btnPeymankar.UseVisualStyleBackColor = False
        '
        'txtPeymankarTel
        '
        Me.txtPeymankarTel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPeymankarTel.CaptinText = ""
        Me.txtPeymankarTel.HasCaption = False
        Me.txtPeymankarTel.IsForceText = False
        Me.txtPeymankarTel.IsFractional = False
        Me.txtPeymankarTel.IsIP = False
        Me.txtPeymankarTel.IsNumberOnly = True
        Me.txtPeymankarTel.IsYear = False
        Me.txtPeymankarTel.Location = New System.Drawing.Point(40, 50)
        Me.txtPeymankarTel.Name = "txtPeymankarTel"
        Me.txtPeymankarTel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPeymankarTel.Size = New System.Drawing.Size(103, 21)
        Me.txtPeymankarTel.TabIndex = 5
        Me.txtPeymankarTel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label120
        '
        Me.Label120.AutoSize = True
        Me.Label120.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label120.Location = New System.Drawing.Point(198, 42)
        Me.Label120.Name = "Label120"
        Me.Label120.Size = New System.Drawing.Size(63, 12)
        Me.Label120.TabIndex = 1
        Me.Label120.Text = "نام استاد کار"
        Me.Label120.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblPeymankarManager
        '
        Me.lblPeymankarManager.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lblPeymankarManager.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPeymankarManager.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblPeymankarManager.Location = New System.Drawing.Point(175, 61)
        Me.lblPeymankarManager.Name = "lblPeymankarManager"
        Me.lblPeymankarManager.Size = New System.Drawing.Size(109, 37)
        Me.lblPeymankarManager.TabIndex = 1
        Me.lblPeymankarManager.Tag = "999"
        Me.lblPeymankarManager.Text = "_"
        Me.lblPeymankarManager.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label34
        '
        Me.Label34.AutoSize = True
        Me.Label34.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label34.Location = New System.Drawing.Point(37, 20)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(100, 24)
        Me.Label34.TabIndex = 1
        Me.Label34.Text = "شماره تلفن پيمانکار " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "جهت ارسال پيامک "
        Me.Label34.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblPeymankar
        '
        Me.lblPeymankar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPeymankar.AutoSize = True
        Me.lblPeymankar.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblPeymankar.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.lblPeymankar.Location = New System.Drawing.Point(622, 57)
        Me.lblPeymankar.Name = "lblPeymankar"
        Me.lblPeymankar.Size = New System.Drawing.Size(95, 13)
        Me.lblPeymankar.TabIndex = 14
        Me.lblPeymankar.TabStop = True
        Me.lblPeymankar.Text = "نام پيمانکار/اکيپ"
        '
        'pnlManoeuvreRequestInfo
        '
        Me.pnlManoeuvreRequestInfo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlManoeuvreRequestInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlManoeuvreRequestInfo.CaptionBackColor = System.Drawing.Color.Teal
        Me.pnlManoeuvreRequestInfo.CaptionFont = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.pnlManoeuvreRequestInfo.CaptionForeColor = System.Drawing.Color.White
        Me.pnlManoeuvreRequestInfo.CaptionHeight = 16
        Me.pnlManoeuvreRequestInfo.CaptionText = "زمان، بار و انرژي توزيع نشده مربوط به مانور"
        Me.pnlManoeuvreRequestInfo.Controls.Add(Me.txtCurrentValueManovr)
        Me.pnlManoeuvreRequestInfo.Controls.Add(Me.txtTimeDisconnectManovr)
        Me.pnlManoeuvreRequestInfo.Controls.Add(Me.txtDateDisconnectManovr)
        Me.pnlManoeuvreRequestInfo.Controls.Add(Me.Label20)
        Me.pnlManoeuvreRequestInfo.Controls.Add(Me.txtTimeConnectManovr)
        Me.pnlManoeuvreRequestInfo.Controls.Add(Me.txtDateConnectManovr)
        Me.pnlManoeuvreRequestInfo.Controls.Add(Me.Label31)
        Me.pnlManoeuvreRequestInfo.Controls.Add(Me.Label41)
        Me.pnlManoeuvreRequestInfo.Controls.Add(Me.txtPowerManovr)
        Me.pnlManoeuvreRequestInfo.Controls.Add(Me.Label43)
        Me.pnlManoeuvreRequestInfo.Controls.Add(Me.lblPowerUnitManovr)
        Me.pnlManoeuvreRequestInfo.Controls.Add(Me.txtDCIntervalManovr)
        Me.pnlManoeuvreRequestInfo.Controls.Add(Me.Label48)
        Me.pnlManoeuvreRequestInfo.Controls.Add(Me.Label109)
        Me.pnlManoeuvreRequestInfo.Controls.Add(Me.Label110)
        Me.pnlManoeuvreRequestInfo.Controls.Add(Me.chkIsNotNeedManovrDC)
        Me.pnlManoeuvreRequestInfo.IsMoveable = False
        Me.pnlManoeuvreRequestInfo.IsWindowMove = False
        Me.pnlManoeuvreRequestInfo.Location = New System.Drawing.Point(312, 136)
        Me.pnlManoeuvreRequestInfo.Name = "pnlManoeuvreRequestInfo"
        Me.pnlManoeuvreRequestInfo.Size = New System.Drawing.Size(408, 112)
        Me.pnlManoeuvreRequestInfo.TabIndex = 1
        Me.pnlManoeuvreRequestInfo.Visible = False
        '
        'txtCurrentValueManovr
        '
        Me.txtCurrentValueManovr.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCurrentValueManovr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCurrentValueManovr.CaptinText = ""
        Me.txtCurrentValueManovr.Enabled = False
        Me.txtCurrentValueManovr.HasCaption = False
        Me.txtCurrentValueManovr.IsForceText = False
        Me.txtCurrentValueManovr.IsFractional = True
        Me.txtCurrentValueManovr.IsIP = False
        Me.txtCurrentValueManovr.IsNumberOnly = True
        Me.txtCurrentValueManovr.IsYear = False
        Me.txtCurrentValueManovr.Location = New System.Drawing.Point(200, 68)
        Me.txtCurrentValueManovr.Name = "txtCurrentValueManovr"
        Me.txtCurrentValueManovr.Size = New System.Drawing.Size(48, 21)
        Me.txtCurrentValueManovr.TabIndex = 4
        Me.txtCurrentValueManovr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtTimeDisconnectManovr
        '
        Me.txtTimeDisconnectManovr.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTimeDisconnectManovr.BackColor = System.Drawing.SystemColors.Window
        Me.txtTimeDisconnectManovr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTimeDisconnectManovr.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTimeDisconnectManovr.IsShadow = False
        Me.txtTimeDisconnectManovr.IsShowCurrentTime = False
        Me.txtTimeDisconnectManovr.Location = New System.Drawing.Point(115, 20)
        Me.txtTimeDisconnectManovr.MaxLength = 5
        Me.txtTimeDisconnectManovr.MiladiDT = Nothing
        Me.txtTimeDisconnectManovr.Name = "txtTimeDisconnectManovr"
        Me.txtTimeDisconnectManovr.ReadOnly = True
        Me.txtTimeDisconnectManovr.ReadOnlyMaskedEdit = False
        Me.txtTimeDisconnectManovr.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTimeDisconnectManovr.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtTimeDisconnectManovr.Size = New System.Drawing.Size(48, 21)
        Me.txtTimeDisconnectManovr.TabIndex = 1
        Me.txtTimeDisconnectManovr.Text = "__:__"
        Me.txtTimeDisconnectManovr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtDateDisconnectManovr
        '
        Me.txtDateDisconnectManovr.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDateDisconnectManovr.BackColor = System.Drawing.SystemColors.Window
        Me.txtDateDisconnectManovr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDateDisconnectManovr.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDateDisconnectManovr.IntegerDate = 0
        Me.txtDateDisconnectManovr.IsShadow = False
        Me.txtDateDisconnectManovr.IsShowCurrentDate = False
        Me.txtDateDisconnectManovr.Location = New System.Drawing.Point(163, 20)
        Me.txtDateDisconnectManovr.MaxLength = 10
        Me.txtDateDisconnectManovr.MiladiDT = CType(resources.GetObject("txtDateDisconnectManovr.MiladiDT"), Object)
        Me.txtDateDisconnectManovr.Name = "txtDateDisconnectManovr"
        Me.txtDateDisconnectManovr.ReadOnly = True
        Me.txtDateDisconnectManovr.ReadOnlyMaskedEdit = False
        Me.txtDateDisconnectManovr.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDateDisconnectManovr.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtDateDisconnectManovr.ShamsiDT = "____/__/__"
        Me.txtDateDisconnectManovr.Size = New System.Drawing.Size(85, 21)
        Me.txtDateDisconnectManovr.TabIndex = 0
        Me.txtDateDisconnectManovr.Text = "____/__/__"
        Me.txtDateDisconnectManovr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtDateDisconnectManovr.TimeMaskedEditorOBJ = Nothing
        '
        'Label20
        '
        Me.Label20.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label20.Location = New System.Drawing.Point(245, 22)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(141, 12)
        Me.Label20.TabIndex = 0
        Me.Label20.Text = "زمان قطع براي بازگشت مانور"
        '
        'txtTimeConnectManovr
        '
        Me.txtTimeConnectManovr.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTimeConnectManovr.BackColor = System.Drawing.SystemColors.Window
        Me.txtTimeConnectManovr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTimeConnectManovr.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTimeConnectManovr.IsShadow = False
        Me.txtTimeConnectManovr.IsShowCurrentTime = False
        Me.txtTimeConnectManovr.Location = New System.Drawing.Point(115, 44)
        Me.txtTimeConnectManovr.MaxLength = 5
        Me.txtTimeConnectManovr.MiladiDT = Nothing
        Me.txtTimeConnectManovr.Name = "txtTimeConnectManovr"
        Me.txtTimeConnectManovr.ReadOnly = True
        Me.txtTimeConnectManovr.ReadOnlyMaskedEdit = False
        Me.txtTimeConnectManovr.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTimeConnectManovr.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtTimeConnectManovr.Size = New System.Drawing.Size(48, 21)
        Me.txtTimeConnectManovr.TabIndex = 3
        Me.txtTimeConnectManovr.Text = "__:__"
        Me.txtTimeConnectManovr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtDateConnectManovr
        '
        Me.txtDateConnectManovr.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDateConnectManovr.BackColor = System.Drawing.SystemColors.Window
        Me.txtDateConnectManovr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDateConnectManovr.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDateConnectManovr.IntegerDate = 0
        Me.txtDateConnectManovr.IsShadow = False
        Me.txtDateConnectManovr.IsShowCurrentDate = False
        Me.txtDateConnectManovr.Location = New System.Drawing.Point(163, 44)
        Me.txtDateConnectManovr.MaxLength = 10
        Me.txtDateConnectManovr.MiladiDT = CType(resources.GetObject("txtDateConnectManovr.MiladiDT"), Object)
        Me.txtDateConnectManovr.Name = "txtDateConnectManovr"
        Me.txtDateConnectManovr.ReadOnly = True
        Me.txtDateConnectManovr.ReadOnlyMaskedEdit = False
        Me.txtDateConnectManovr.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDateConnectManovr.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtDateConnectManovr.ShamsiDT = "____/__/__"
        Me.txtDateConnectManovr.Size = New System.Drawing.Size(85, 21)
        Me.txtDateConnectManovr.TabIndex = 2
        Me.txtDateConnectManovr.Text = "____/__/__"
        Me.txtDateConnectManovr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtDateConnectManovr.TimeMaskedEditorOBJ = Nothing
        '
        'Label31
        '
        Me.Label31.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label31.AutoSize = True
        Me.Label31.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label31.Location = New System.Drawing.Point(245, 46)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(142, 12)
        Me.Label31.TabIndex = 0
        Me.Label31.Text = "زمان وصل براي بازگشت مانور"
        '
        'Label41
        '
        Me.Label41.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label41.AutoSize = True
        Me.Label41.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label41.Location = New System.Drawing.Point(245, 70)
        Me.Label41.Name = "Label41"
        Me.Label41.Size = New System.Drawing.Size(102, 12)
        Me.Label41.TabIndex = 0
        Me.Label41.Text = "ميانگين بار قطع شده"
        '
        'txtPowerManovr
        '
        Me.txtPowerManovr.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPowerManovr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPowerManovr.CaptinText = ""
        Me.txtPowerManovr.HasCaption = False
        Me.txtPowerManovr.IsForceText = False
        Me.txtPowerManovr.IsFractional = True
        Me.txtPowerManovr.IsIP = False
        Me.txtPowerManovr.IsNumberOnly = True
        Me.txtPowerManovr.IsYear = False
        Me.txtPowerManovr.Location = New System.Drawing.Point(40, 84)
        Me.txtPowerManovr.Name = "txtPowerManovr"
        Me.txtPowerManovr.ReadOnly = True
        Me.txtPowerManovr.Size = New System.Drawing.Size(48, 21)
        Me.txtPowerManovr.TabIndex = 7
        Me.txtPowerManovr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label43
        '
        Me.Label43.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label43.AutoSize = True
        Me.Label43.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label43.Location = New System.Drawing.Point(176, 72)
        Me.Label43.Name = "Label43"
        Me.Label43.Size = New System.Drawing.Size(22, 12)
        Me.Label43.TabIndex = 0
        Me.Label43.Text = "آمپر"
        '
        'lblPowerUnitManovr
        '
        Me.lblPowerUnitManovr.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPowerUnitManovr.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblPowerUnitManovr.Location = New System.Drawing.Point(10, 89)
        Me.lblPowerUnitManovr.Name = "lblPowerUnitManovr"
        Me.lblPowerUnitManovr.Size = New System.Drawing.Size(28, 10)
        Me.lblPowerUnitManovr.TabIndex = 0
        Me.lblPowerUnitManovr.Text = "MWh"
        '
        'txtDCIntervalManovr
        '
        Me.txtDCIntervalManovr.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDCIntervalManovr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDCIntervalManovr.CaptinText = ""
        Me.txtDCIntervalManovr.HasCaption = False
        Me.txtDCIntervalManovr.IsForceText = False
        Me.txtDCIntervalManovr.IsFractional = False
        Me.txtDCIntervalManovr.IsIP = False
        Me.txtDCIntervalManovr.IsNumberOnly = False
        Me.txtDCIntervalManovr.IsYear = False
        Me.txtDCIntervalManovr.Location = New System.Drawing.Point(40, 36)
        Me.txtDCIntervalManovr.Name = "txtDCIntervalManovr"
        Me.txtDCIntervalManovr.ReadOnly = True
        Me.txtDCIntervalManovr.Size = New System.Drawing.Size(48, 21)
        Me.txtDCIntervalManovr.TabIndex = 6
        Me.txtDCIntervalManovr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label48
        '
        Me.Label48.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label48.AutoSize = True
        Me.Label48.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label48.Location = New System.Drawing.Point(8, 39)
        Me.Label48.Name = "Label48"
        Me.Label48.Size = New System.Drawing.Size(32, 12)
        Me.Label48.TabIndex = 0
        Me.Label48.Text = "دقيقه"
        '
        'Label109
        '
        Me.Label109.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label109.AutoSize = True
        Me.Label109.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label109.Location = New System.Drawing.Point(5, 67)
        Me.Label109.Name = "Label109"
        Me.Label109.Size = New System.Drawing.Size(85, 12)
        Me.Label109.TabIndex = 0
        Me.Label109.Text = "انرژي توزيع نشده"
        '
        'Label110
        '
        Me.Label110.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label110.AutoSize = True
        Me.Label110.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label110.Location = New System.Drawing.Point(41, 20)
        Me.Label110.Name = "Label110"
        Me.Label110.Size = New System.Drawing.Size(50, 12)
        Me.Label110.TabIndex = 0
        Me.Label110.Text = "مدت قطع"
        '
        'chkIsNotNeedManovrDC
        '
        Me.chkIsNotNeedManovrDC.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.chkIsNotNeedManovrDC.Location = New System.Drawing.Point(186, 89)
        Me.chkIsNotNeedManovrDC.Name = "chkIsNotNeedManovrDC"
        Me.chkIsNotNeedManovrDC.Size = New System.Drawing.Size(206, 24)
        Me.chkIsNotNeedManovrDC.TabIndex = 10
        Me.chkIsNotNeedManovrDC.Text = "عدم نياز به خاموشي براي بازگشت مانور"
        Me.chkIsNotNeedManovrDC.Visible = False
        '
        'tbcDCs
        '
        Me.tbcDCs.Controls.Add(Me.tpDCMon)
        Me.tbcDCs.Controls.Add(Me.tpDCMain)
        Me.tbcDCs.Font = New System.Drawing.Font("Tahoma", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.tbcDCs.Location = New System.Drawing.Point(646, 136)
        Me.tbcDCs.Name = "tbcDCs"
        Me.tbcDCs.SelectedIndex = 0
        Me.tbcDCs.Size = New System.Drawing.Size(75, 19)
        Me.tbcDCs.TabIndex = 13
        Me.tbcDCs.Tag = "999"
        Me.tbcDCs.Visible = False
        '
        'tpDCMon
        '
        Me.tpDCMon.Font = New System.Drawing.Font("Tahoma", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.tpDCMon.Location = New System.Drawing.Point(4, 19)
        Me.tpDCMon.Name = "tpDCMon"
        Me.tpDCMon.Size = New System.Drawing.Size(67, 0)
        Me.tpDCMon.TabIndex = 1
        Me.tpDCMon.Text = "مانور"
        '
        'tpDCMain
        '
        Me.tpDCMain.Font = New System.Drawing.Font("Tahoma", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.tpDCMain.Location = New System.Drawing.Point(4, 19)
        Me.tpDCMain.Name = "tpDCMain"
        Me.tpDCMain.Size = New System.Drawing.Size(67, 0)
        Me.tpDCMain.TabIndex = 0
        Me.tpDCMain.Text = "اصلي"
        '
        'lblLetterInfo
        '
        Me.lblLetterInfo.AutoSize = True
        Me.lblLetterInfo.Font = New System.Drawing.Font("Tahoma", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblLetterInfo.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.lblLetterInfo.Location = New System.Drawing.Point(351, 107)
        Me.lblLetterInfo.Name = "lblLetterInfo"
        Me.lblLetterInfo.Size = New System.Drawing.Size(63, 11)
        Me.lblLetterInfo.TabIndex = 11
        Me.lblLetterInfo.TabStop = True
        Me.lblLetterInfo.Text = "مشخصات نامه"
        Me.lblLetterInfo.Visible = False
        '
        'pnlManoeuvre
        '
        Me.pnlManoeuvre.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlManoeuvre.CaptionBackColor = System.Drawing.Color.Teal
        Me.pnlManoeuvre.CaptionFont = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.pnlManoeuvre.CaptionForeColor = System.Drawing.Color.White
        Me.pnlManoeuvre.CaptionHeight = 16
        Me.pnlManoeuvre.CaptionText = "مانور"
        Me.pnlManoeuvre.Controls.Add(Me.lnkManoeuvreDesc)
        Me.pnlManoeuvre.Controls.Add(Me.pnlManoeuvrePostFeeder)
        Me.pnlManoeuvre.Controls.Add(Me.chkIsManoeuvre)
        Me.pnlManoeuvre.IsMoveable = False
        Me.pnlManoeuvre.IsWindowMove = False
        Me.pnlManoeuvre.Location = New System.Drawing.Point(8, 408)
        Me.pnlManoeuvre.Name = "pnlManoeuvre"
        Me.pnlManoeuvre.Size = New System.Drawing.Size(296, 75)
        Me.pnlManoeuvre.TabIndex = 3
        '
        'lnkManoeuvreDesc
        '
        Me.lnkManoeuvreDesc.AutoSize = True
        Me.lnkManoeuvreDesc.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.lnkManoeuvreDesc.Location = New System.Drawing.Point(104, 56)
        Me.lnkManoeuvreDesc.Name = "lnkManoeuvreDesc"
        Me.lnkManoeuvreDesc.Size = New System.Drawing.Size(87, 13)
        Me.lnkManoeuvreDesc.TabIndex = 1
        Me.lnkManoeuvreDesc.TabStop = True
        Me.lnkManoeuvreDesc.Text = "نمايش شرح مانور"
        '
        'pnlManoeuvrePostFeeder
        '
        Me.pnlManoeuvrePostFeeder.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlManoeuvrePostFeeder.Controls.Add(Me.Label18)
        Me.pnlManoeuvrePostFeeder.Controls.Add(Me.cmbManoeuvreType)
        Me.pnlManoeuvrePostFeeder.Enabled = False
        Me.pnlManoeuvrePostFeeder.Location = New System.Drawing.Point(1, 35)
        Me.pnlManoeuvrePostFeeder.Name = "pnlManoeuvrePostFeeder"
        Me.pnlManoeuvrePostFeeder.Size = New System.Drawing.Size(290, 19)
        Me.pnlManoeuvrePostFeeder.TabIndex = 1
        '
        'Label18
        '
        Me.Label18.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label18.Location = New System.Drawing.Point(248, 2)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(42, 12)
        Me.Label18.TabIndex = 0
        Me.Label18.Text = "از طريق"
        '
        'cmbManoeuvreType
        '
        Me.cmbManoeuvreType.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbManoeuvreType.BackColor = System.Drawing.Color.White
        Me.cmbManoeuvreType.DataSource = Me.DatasetBT1.Tbl_ManoeuvreType
        Me.cmbManoeuvreType.DisplayMember = "ManoeuvreType"
        Me.cmbManoeuvreType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbManoeuvreType.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.cmbManoeuvreType.IsReadOnly = False
        Me.cmbManoeuvreType.Location = New System.Drawing.Point(10, 0)
        Me.cmbManoeuvreType.Name = "cmbManoeuvreType"
        Me.cmbManoeuvreType.Size = New System.Drawing.Size(233, 19)
        Me.cmbManoeuvreType.TabIndex = 0
        Me.cmbManoeuvreType.ValueMember = "ManoeuvreTypeId"
        '
        'chkIsManoeuvre
        '
        Me.chkIsManoeuvre.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkIsManoeuvre.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.chkIsManoeuvre.Location = New System.Drawing.Point(203, 17)
        Me.chkIsManoeuvre.Name = "chkIsManoeuvre"
        Me.chkIsManoeuvre.Size = New System.Drawing.Size(89, 18)
        Me.chkIsManoeuvre.TabIndex = 0
        Me.chkIsManoeuvre.Text = "امکان مانور "
        Me.chkIsManoeuvre.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnlAdsress
        '
        Me.pnlAdsress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlAdsress.CaptionBackColor = System.Drawing.Color.Teal
        Me.pnlAdsress.CaptionFont = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.pnlAdsress.CaptionForeColor = System.Drawing.Color.White
        Me.pnlAdsress.CaptionHeight = 16
        Me.pnlAdsress.CaptionText = "فهرست مناطق و مراکز خاموشي"
        Me.pnlAdsress.Controls.Add(Me.Label47)
        Me.pnlAdsress.Controls.Add(Me.Label23)
        Me.pnlAdsress.Controls.Add(Me.txtAsdresses)
        Me.pnlAdsress.Controls.Add(Me.BracketPanel1)
        Me.pnlAdsress.Controls.Add(Me.Label24)
        Me.pnlAdsress.Controls.Add(Me.txtCriticalLocations)
        Me.pnlAdsress.Controls.Add(Me.txtWorkingAddress)
        Me.pnlAdsress.Controls.Add(Me.rbNotInCityService)
        Me.pnlAdsress.Controls.Add(Me.rbInCityService)
        Me.pnlAdsress.IsMoveable = False
        Me.pnlAdsress.IsWindowMove = False
        Me.pnlAdsress.Location = New System.Drawing.Point(8, 8)
        Me.pnlAdsress.Name = "pnlAdsress"
        Me.pnlAdsress.Size = New System.Drawing.Size(296, 392)
        Me.pnlAdsress.TabIndex = 2
        '
        'Label47
        '
        Me.Label47.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label47.AutoSize = True
        Me.Label47.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label47.Location = New System.Drawing.Point(204, 294)
        Me.Label47.Name = "Label47"
        Me.Label47.Size = New System.Drawing.Size(82, 13)
        Me.Label47.TabIndex = 0
        Me.Label47.Text = "آدرس محل کار"
        '
        'BracketPanel1
        '
        Me.BracketPanel1.BracketWidth = 5.0!
        Me.BracketPanel1.Direction = Bargh_Common.BracketPanel.Directions.Right
        Me.BracketPanel1.LineColor = System.Drawing.Color.DarkBlue
        Me.BracketPanel1.LineWidth = 1.0!
        Me.BracketPanel1.Location = New System.Drawing.Point(273, 152)
        Me.BracketPanel1.Name = "BracketPanel1"
        Me.BracketPanel1.Size = New System.Drawing.Size(16, 138)
        Me.BracketPanel1.TabIndex = 3
        '
        'txtWorkingAddress
        '
        Me.txtWorkingAddress.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtWorkingAddress.BackColor = System.Drawing.Color.White
        Me.txtWorkingAddress.Location = New System.Drawing.Point(11, 312)
        Me.txtWorkingAddress.MaxLength = 300
        Me.txtWorkingAddress.Multiline = True
        Me.txtWorkingAddress.Name = "txtWorkingAddress"
        Me.txtWorkingAddress.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtWorkingAddress.Size = New System.Drawing.Size(272, 72)
        Me.txtWorkingAddress.TabIndex = 4
        '
        'rbNotInCityService
        '
        Me.rbNotInCityService.Location = New System.Drawing.Point(11, 163)
        Me.rbNotInCityService.Name = "rbNotInCityService"
        Me.rbNotInCityService.Size = New System.Drawing.Size(82, 16)
        Me.rbNotInCityService.TabIndex = 2
        Me.rbNotInCityService.Text = "خارج از شهر"
        '
        'rbInCityService
        '
        Me.rbInCityService.Location = New System.Drawing.Point(104, 163)
        Me.rbInCityService.Name = "rbInCityService"
        Me.rbInCityService.Size = New System.Drawing.Size(99, 16)
        Me.rbInCityService.TabIndex = 1
        Me.rbInCityService.Text = "در محدوده شهر"
        '
        'pnlInfo
        '
        Me.pnlInfo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlInfo.CaptionBackColor = System.Drawing.Color.Teal
        Me.pnlInfo.CaptionFont = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.pnlInfo.CaptionForeColor = System.Drawing.Color.White
        Me.pnlInfo.CaptionHeight = 16
        Me.pnlInfo.CaptionText = "وضعيت درخواست"
        Me.pnlInfo.Controls.Add(Me.txtPeymankarSearch)
        Me.pnlInfo.Controls.Add(Me.cmbWeather)
        Me.pnlInfo.Controls.Add(Me.Label36)
        Me.pnlInfo.Controls.Add(Me.cmbPeymankar)
        Me.pnlInfo.Controls.Add(Me.lblDepartment)
        Me.pnlInfo.Controls.Add(Me.cmbDepartment)
        Me.pnlInfo.Controls.Add(Me.cmbReferTo)
        Me.pnlInfo.Controls.Add(Me.Label73)
        Me.pnlInfo.Controls.Add(Me.pnlRequestedBy)
        Me.pnlInfo.Controls.Add(Me.picPeymankarSearch)
        Me.pnlInfo.IsMoveable = False
        Me.pnlInfo.IsWindowMove = False
        Me.pnlInfo.Location = New System.Drawing.Point(312, 8)
        Me.pnlInfo.Name = "pnlInfo"
        Me.pnlInfo.Size = New System.Drawing.Size(408, 120)
        Me.pnlInfo.TabIndex = 0
        '
        'txtPeymankarSearch
        '
        Me.txtPeymankarSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPeymankarSearch.CaptinText = "پيمانکار"
        Me.txtPeymankarSearch.HasCaption = False
        Me.txtPeymankarSearch.IsForceText = False
        Me.txtPeymankarSearch.IsFractional = True
        Me.txtPeymankarSearch.IsIP = False
        Me.txtPeymankarSearch.IsNumberOnly = False
        Me.txtPeymankarSearch.IsYear = False
        Me.txtPeymankarSearch.Location = New System.Drawing.Point(141, 22)
        Me.txtPeymankarSearch.Name = "txtPeymankarSearch"
        Me.txtPeymankarSearch.Size = New System.Drawing.Size(102, 21)
        Me.txtPeymankarSearch.TabIndex = 187
        Me.txtPeymankarSearch.Visible = False
        '
        'cmbWeather
        '
        Me.cmbWeather.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbWeather.BackColor = System.Drawing.Color.White
        Me.cmbWeather.DataSource = Me.DatasetBT1.Tbl_Weather
        Me.cmbWeather.DisplayMember = "Weather"
        Me.cmbWeather.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbWeather.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.cmbWeather.IsReadOnly = False
        Me.cmbWeather.Location = New System.Drawing.Point(173, 20)
        Me.cmbWeather.Name = "cmbWeather"
        Me.cmbWeather.Size = New System.Drawing.Size(128, 20)
        Me.cmbWeather.TabIndex = 0
        Me.cmbWeather.ValueMember = "WeatherId"
        '
        'Label36
        '
        Me.Label36.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label36.AutoSize = True
        Me.Label36.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label36.Location = New System.Drawing.Point(309, 22)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(92, 13)
        Me.Label36.TabIndex = 5
        Me.Label36.Text = "وضعيت آب و هوا"
        '
        'cmbPeymankar
        '
        Me.cmbPeymankar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbPeymankar.BackColor = System.Drawing.Color.White
        Me.cmbPeymankar.DataBindings.Add(New System.Windows.Forms.Binding("SelectedValue", Me.DatasetDummy1, "ViewDummyId.Dummy07Id", True))
        Me.cmbPeymankar.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.cmbPeymankar.IsReadOnly = False
        Me.cmbPeymankar.Location = New System.Drawing.Point(165, 44)
        Me.cmbPeymankar.Name = "cmbPeymankar"
        Me.cmbPeymankar.Size = New System.Drawing.Size(136, 20)
        Me.cmbPeymankar.TabIndex = 1
        '
        'lblDepartment
        '
        Me.lblDepartment.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDepartment.AutoSize = True
        Me.lblDepartment.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblDepartment.Location = New System.Drawing.Point(285, 70)
        Me.lblDepartment.Name = "lblDepartment"
        Me.lblDepartment.Size = New System.Drawing.Size(115, 13)
        Me.lblDepartment.TabIndex = 4
        Me.lblDepartment.Text = "واحد درخواست کننده"
        '
        'cmbDepartment
        '
        Me.cmbDepartment.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbDepartment.BackColor = System.Drawing.Color.White
        Me.cmbDepartment.DataBindings.Add(New System.Windows.Forms.Binding("SelectedValue", Me.DatasetDummy1, "ViewDummyId.Dummy08Id", True))
        Me.cmbDepartment.DataSource = Me.DatasetBT1.Tbl_Department
        Me.cmbDepartment.DisplayMember = "Department"
        Me.cmbDepartment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbDepartment.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.cmbDepartment.IsReadOnly = False
        Me.cmbDepartment.Location = New System.Drawing.Point(141, 68)
        Me.cmbDepartment.Name = "cmbDepartment"
        Me.cmbDepartment.Size = New System.Drawing.Size(144, 20)
        Me.cmbDepartment.TabIndex = 2
        Me.cmbDepartment.ValueMember = "DepartmentId"
        '
        'cmbReferTo
        '
        Me.cmbReferTo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbReferTo.BackColor = System.Drawing.Color.White
        Me.cmbReferTo.DataBindings.Add(New System.Windows.Forms.Binding("SelectedValue", Me.DatasetDummy1, "ViewDummyId.Dummy09Id", True))
        Me.cmbReferTo.DataSource = Me.DatasetBT1.Tbl_ReferTo
        Me.cmbReferTo.DisplayMember = "ReferTo"
        Me.cmbReferTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbReferTo.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.cmbReferTo.IsReadOnly = False
        Me.cmbReferTo.Location = New System.Drawing.Point(141, 92)
        Me.cmbReferTo.Name = "cmbReferTo"
        Me.cmbReferTo.Size = New System.Drawing.Size(144, 20)
        Me.cmbReferTo.TabIndex = 3
        Me.cmbReferTo.ValueMember = "ReferToId"
        '
        'Label73
        '
        Me.Label73.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label73.AutoSize = True
        Me.Label73.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label73.Location = New System.Drawing.Point(285, 94)
        Me.Label73.Name = "Label73"
        Me.Label73.Size = New System.Drawing.Size(90, 13)
        Me.Label73.TabIndex = 4
        Me.Label73.Text = "واحد اقدام کننده"
        '
        'pnlRequestedBy
        '
        Me.pnlRequestedBy.Controls.Add(Me.rbIsRequestByPeymankar)
        Me.pnlRequestedBy.Controls.Add(Me.rbIsRequestByTozi)
        Me.pnlRequestedBy.Controls.Add(Me.Label71)
        Me.pnlRequestedBy.Controls.Add(Me.rbIsRequestByFogheTozi)
        Me.pnlRequestedBy.Controls.Add(Me.HatchPanel5)
        Me.pnlRequestedBy.Location = New System.Drawing.Point(9, 18)
        Me.pnlRequestedBy.Name = "pnlRequestedBy"
        Me.pnlRequestedBy.Size = New System.Drawing.Size(122, 96)
        Me.pnlRequestedBy.TabIndex = 7
        '
        'rbIsRequestByPeymankar
        '
        Me.rbIsRequestByPeymankar.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.rbIsRequestByPeymankar.Location = New System.Drawing.Point(0, 20)
        Me.rbIsRequestByPeymankar.Name = "rbIsRequestByPeymankar"
        Me.rbIsRequestByPeymankar.Size = New System.Drawing.Size(80, 16)
        Me.rbIsRequestByPeymankar.TabIndex = 4
        Me.rbIsRequestByPeymankar.Text = "پيمانکار"
        '
        'rbIsRequestByTozi
        '
        Me.rbIsRequestByTozi.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.rbIsRequestByTozi.Location = New System.Drawing.Point(0, 41)
        Me.rbIsRequestByTozi.Name = "rbIsRequestByTozi"
        Me.rbIsRequestByTozi.Size = New System.Drawing.Size(80, 16)
        Me.rbIsRequestByTozi.TabIndex = 5
        Me.rbIsRequestByTozi.Text = "شرکت توزيع"
        '
        'Label71
        '
        Me.Label71.AutoSize = True
        Me.Label71.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label71.Location = New System.Drawing.Point(0, 0)
        Me.Label71.Name = "Label71"
        Me.Label71.Size = New System.Drawing.Size(126, 13)
        Me.Label71.TabIndex = 5
        Me.Label71.Text = "خاموشي به درخواست"
        '
        'rbIsRequestByFogheTozi
        '
        Me.rbIsRequestByFogheTozi.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.rbIsRequestByFogheTozi.Location = New System.Drawing.Point(0, 62)
        Me.rbIsRequestByFogheTozi.Name = "rbIsRequestByFogheTozi"
        Me.rbIsRequestByFogheTozi.Size = New System.Drawing.Size(80, 16)
        Me.rbIsRequestByFogheTozi.TabIndex = 6
        Me.rbIsRequestByFogheTozi.Text = "فوق توزيع"
        '
        'HatchPanel5
        '
        Me.HatchPanel5.Direction = Bargh_Common.HatchPanel.LeftRight.Right
        Me.HatchPanel5.HatchColor = System.Drawing.Color.DarkBlue
        Me.HatchPanel5.HatchCount = 3.0!
        Me.HatchPanel5.HatchSpaceWidth = 21.0!
        Me.HatchPanel5.HatchStyle = System.Drawing.Drawing2D.DashStyle.Dash
        Me.HatchPanel5.HatchWidth = 1.0!
        Me.HatchPanel5.Location = New System.Drawing.Point(0, 11)
        Me.HatchPanel5.Name = "HatchPanel5"
        Me.HatchPanel5.Size = New System.Drawing.Size(104, 60)
        Me.HatchPanel5.TabIndex = 6
        '
        'picPeymankarSearch
        '
        Me.picPeymankarSearch.Cursor = System.Windows.Forms.Cursors.Hand
        Me.picPeymankarSearch.Image = CType(resources.GetObject("picPeymankarSearch.Image"), System.Drawing.Image)
        Me.picPeymankarSearch.Location = New System.Drawing.Point(141, 46)
        Me.picPeymankarSearch.Name = "picPeymankarSearch"
        Me.picPeymankarSearch.Size = New System.Drawing.Size(16, 16)
        Me.picPeymankarSearch.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picPeymankarSearch.TabIndex = 186
        Me.picPeymankarSearch.TabStop = False
        Me.GlobalToolTip.SetToolTip(Me.picPeymankarSearch, "جستجوي پيمانکاران")
        '
        'pnlManoeuvreDesc
        '
        Me.pnlManoeuvreDesc.Controls.Add(Me.txtManoeuvreDesc)
        Me.pnlManoeuvreDesc.Controls.Add(Me.picOKManoeuvreDesc)
        Me.pnlManoeuvreDesc.Location = New System.Drawing.Point(8, 408)
        Me.pnlManoeuvreDesc.Name = "pnlManoeuvreDesc"
        Me.pnlManoeuvreDesc.Size = New System.Drawing.Size(296, 70)
        Me.pnlManoeuvreDesc.TabIndex = 5
        Me.pnlManoeuvreDesc.Visible = False
        '
        'txtManoeuvreDesc
        '
        Me.txtManoeuvreDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtManoeuvreDesc.CaptinText = "شرح مانور"
        Me.txtManoeuvreDesc.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.txtManoeuvreDesc.HasCaption = True
        Me.txtManoeuvreDesc.IsForceText = False
        Me.txtManoeuvreDesc.IsFractional = True
        Me.txtManoeuvreDesc.IsIP = False
        Me.txtManoeuvreDesc.IsNumberOnly = False
        Me.txtManoeuvreDesc.IsYear = False
        Me.txtManoeuvreDesc.Location = New System.Drawing.Point(32, 8)
        Me.txtManoeuvreDesc.MaxLength = 199
        Me.txtManoeuvreDesc.Multiline = True
        Me.txtManoeuvreDesc.Name = "txtManoeuvreDesc"
        Me.txtManoeuvreDesc.Size = New System.Drawing.Size(256, 56)
        Me.txtManoeuvreDesc.TabIndex = 3
        '
        'picOKManoeuvreDesc
        '
        Me.picOKManoeuvreDesc.Cursor = System.Windows.Forms.Cursors.Hand
        Me.picOKManoeuvreDesc.Image = CType(resources.GetObject("picOKManoeuvreDesc.Image"), System.Drawing.Image)
        Me.picOKManoeuvreDesc.Location = New System.Drawing.Point(8, 8)
        Me.picOKManoeuvreDesc.Name = "picOKManoeuvreDesc"
        Me.picOKManoeuvreDesc.Size = New System.Drawing.Size(16, 16)
        Me.picOKManoeuvreDesc.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.picOKManoeuvreDesc.TabIndex = 4
        Me.picOKManoeuvreDesc.TabStop = False
        '
        'pnlLetterInfo
        '
        Me.pnlLetterInfo.Controls.Add(Me.Label51)
        Me.pnlLetterInfo.Controls.Add(Me.txtLetterDate)
        Me.pnlLetterInfo.Controls.Add(Me.txtLetterNo)
        Me.pnlLetterInfo.Controls.Add(Me.btnHidePnlLetter)
        Me.pnlLetterInfo.Controls.Add(Me.Label107)
        Me.pnlLetterInfo.Location = New System.Drawing.Point(320, 26)
        Me.pnlLetterInfo.Name = "pnlLetterInfo"
        Me.pnlLetterInfo.Size = New System.Drawing.Size(124, 96)
        Me.pnlLetterInfo.TabIndex = 10
        Me.pnlLetterInfo.Visible = False
        '
        'Label51
        '
        Me.Label51.AutoSize = True
        Me.Label51.Location = New System.Drawing.Point(48, 5)
        Me.Label51.Name = "Label51"
        Me.Label51.Size = New System.Drawing.Size(58, 13)
        Me.Label51.TabIndex = 6
        Me.Label51.Text = "شماره نامه"
        '
        'txtLetterDate
        '
        Me.txtLetterDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtLetterDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtLetterDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLetterDate.IntegerDate = 0
        Me.txtLetterDate.IsShadow = False
        Me.txtLetterDate.IsShowCurrentDate = False
        Me.txtLetterDate.Location = New System.Drawing.Point(32, 70)
        Me.txtLetterDate.MaxLength = 10
        Me.txtLetterDate.MiladiDT = CType(resources.GetObject("txtLetterDate.MiladiDT"), Object)
        Me.txtLetterDate.Name = "txtLetterDate"
        Me.txtLetterDate.ReadOnly = True
        Me.txtLetterDate.ReadOnlyMaskedEdit = False
        Me.txtLetterDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLetterDate.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtLetterDate.ShamsiDT = "____/__/__"
        Me.txtLetterDate.Size = New System.Drawing.Size(88, 21)
        Me.txtLetterDate.TabIndex = 5
        Me.txtLetterDate.Text = "____/__/__"
        Me.txtLetterDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtLetterDate.TimeMaskedEditorOBJ = Nothing
        '
        'txtLetterNo
        '
        Me.txtLetterNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtLetterNo.CaptinText = ""
        Me.txtLetterNo.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.txtLetterNo.HasCaption = True
        Me.txtLetterNo.IsForceText = False
        Me.txtLetterNo.IsFractional = True
        Me.txtLetterNo.IsIP = False
        Me.txtLetterNo.IsNumberOnly = False
        Me.txtLetterNo.IsYear = False
        Me.txtLetterNo.Location = New System.Drawing.Point(32, 23)
        Me.txtLetterNo.Multiline = True
        Me.txtLetterNo.Name = "txtLetterNo"
        Me.txtLetterNo.Size = New System.Drawing.Size(88, 24)
        Me.txtLetterNo.TabIndex = 3
        '
        'btnHidePnlLetter
        '
        Me.btnHidePnlLetter.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnHidePnlLetter.Image = CType(resources.GetObject("btnHidePnlLetter.Image"), System.Drawing.Image)
        Me.btnHidePnlLetter.Location = New System.Drawing.Point(8, 72)
        Me.btnHidePnlLetter.Name = "btnHidePnlLetter"
        Me.btnHidePnlLetter.Size = New System.Drawing.Size(16, 16)
        Me.btnHidePnlLetter.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.btnHidePnlLetter.TabIndex = 4
        Me.btnHidePnlLetter.TabStop = False
        '
        'Label107
        '
        Me.Label107.AutoSize = True
        Me.Label107.Location = New System.Drawing.Point(52, 52)
        Me.Label107.Name = "Label107"
        Me.Label107.Size = New System.Drawing.Size(49, 13)
        Me.Label107.TabIndex = 6
        Me.Label107.Text = "تاريخ نامه"
        '
        'pnlMultiStepInfo
        '
        Me.pnlMultiStepInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlMultiStepInfo.CaptionBackColor = System.Drawing.Color.Teal
        Me.pnlMultiStepInfo.CaptionFont = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.pnlMultiStepInfo.CaptionForeColor = System.Drawing.Color.White
        Me.pnlMultiStepInfo.CaptionHeight = 16
        Me.pnlMultiStepInfo.CaptionText = "اطلاعات وصل‌هاي چند مرحله‌اي"
        Me.pnlMultiStepInfo.Controls.Add(Me.HatchPanel6)
        Me.pnlMultiStepInfo.Controls.Add(Me.btnMSOK)
        Me.pnlMultiStepInfo.Controls.Add(Me.btnMSCancel)
        Me.pnlMultiStepInfo.Controls.Add(Me.pnlLevel6)
        Me.pnlMultiStepInfo.Controls.Add(Me.pnlLevel5)
        Me.pnlMultiStepInfo.Controls.Add(Me.pnlLevel4)
        Me.pnlMultiStepInfo.Controls.Add(Me.pnlLevel3)
        Me.pnlMultiStepInfo.Controls.Add(Me.pnlLevel2)
        Me.pnlMultiStepInfo.Controls.Add(Me.pnlLevel1)
        Me.pnlMultiStepInfo.Controls.Add(Me.HatchPanel7)
        Me.pnlMultiStepInfo.Controls.Add(Me.HatchPanel8)
        Me.pnlMultiStepInfo.Controls.Add(Me.HatchPanel9)
        Me.pnlMultiStepInfo.Controls.Add(Me.HatchPanel10)
        Me.pnlMultiStepInfo.Controls.Add(Me.HatchPanel11)
        Me.pnlMultiStepInfo.Controls.Add(Me.Label106)
        Me.pnlMultiStepInfo.Controls.Add(Me.txtDCCurrent)
        Me.pnlMultiStepInfo.Controls.Add(Me.HatchPanel12)
        Me.pnlMultiStepInfo.IsMoveable = False
        Me.pnlMultiStepInfo.IsWindowMove = False
        Me.pnlMultiStepInfo.Location = New System.Drawing.Point(8, 8)
        Me.pnlMultiStepInfo.Name = "pnlMultiStepInfo"
        Me.pnlMultiStepInfo.Size = New System.Drawing.Size(712, 472)
        Me.pnlMultiStepInfo.TabIndex = 6
        Me.pnlMultiStepInfo.Visible = False
        '
        'HatchPanel6
        '
        Me.HatchPanel6.Direction = Bargh_Common.HatchPanel.LeftRight.Right
        Me.HatchPanel6.HatchColor = System.Drawing.Color.DarkBlue
        Me.HatchPanel6.HatchCount = 1.0!
        Me.HatchPanel6.HatchSpaceWidth = 20.0!
        Me.HatchPanel6.HatchStyle = System.Drawing.Drawing2D.DashStyle.Dash
        Me.HatchPanel6.HatchWidth = 1.0!
        Me.HatchPanel6.Location = New System.Drawing.Point(7, 119)
        Me.HatchPanel6.Name = "HatchPanel6"
        Me.HatchPanel6.Size = New System.Drawing.Size(696, 3)
        Me.HatchPanel6.TabIndex = 8
        '
        'btnMSOK
        '
        Me.btnMSOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnMSOK.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.btnMSOK.Image = CType(resources.GetObject("btnMSOK.Image"), System.Drawing.Image)
        Me.btnMSOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnMSOK.Location = New System.Drawing.Point(391, 437)
        Me.btnMSOK.Name = "btnMSOK"
        Me.btnMSOK.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.btnMSOK.Size = New System.Drawing.Size(81, 25)
        Me.btnMSOK.TabIndex = 6
        Me.btnMSOK.Text = "تأييد"
        Me.btnMSOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnMSCancel
        '
        Me.btnMSCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnMSCancel.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.btnMSCancel.Image = CType(resources.GetObject("btnMSCancel.Image"), System.Drawing.Image)
        Me.btnMSCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnMSCancel.Location = New System.Drawing.Point(239, 437)
        Me.btnMSCancel.Name = "btnMSCancel"
        Me.btnMSCancel.Size = New System.Drawing.Size(81, 25)
        Me.btnMSCancel.TabIndex = 7
        Me.btnMSCancel.Text = "انصراف"
        Me.btnMSCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'pnlLevel6
        '
        Me.pnlLevel6.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlLevel6.Controls.Add(Me.txtMSDesc6)
        Me.pnlLevel6.Controls.Add(Me.Label91)
        Me.pnlLevel6.Controls.Add(Me.Label92)
        Me.pnlLevel6.Controls.Add(Me.txtMSBar6)
        Me.pnlLevel6.Controls.Add(Me.txtMSTime6)
        Me.pnlLevel6.Controls.Add(Me.Label93)
        Me.pnlLevel6.Controls.Add(Me.Label94)
        Me.pnlLevel6.Controls.Add(Me.Label105)
        Me.pnlLevel6.Controls.Add(Me.txtMSDate6)
        Me.pnlLevel6.Controls.Add(Me.btnFeederKey6)
        Me.pnlLevel6.Location = New System.Drawing.Point(7, 358)
        Me.pnlLevel6.Name = "pnlLevel6"
        Me.pnlLevel6.Size = New System.Drawing.Size(696, 56)
        Me.pnlLevel6.TabIndex = 5
        '
        'txtMSDesc6
        '
        Me.txtMSDesc6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMSDesc6.Location = New System.Drawing.Point(0, 3)
        Me.txtMSDesc6.Multiline = True
        Me.txtMSDesc6.Name = "txtMSDesc6"
        Me.txtMSDesc6.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtMSDesc6.Size = New System.Drawing.Size(336, 48)
        Me.txtMSDesc6.TabIndex = 2
        '
        'Label91
        '
        Me.Label91.AutoSize = True
        Me.Label91.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label91.Location = New System.Drawing.Point(620, 8)
        Me.Label91.Name = "Label91"
        Me.Label91.Size = New System.Drawing.Size(70, 12)
        Me.Label91.TabIndex = 0
        Me.Label91.Text = "مرحله ششم:"
        '
        'Label92
        '
        Me.Label92.AutoSize = True
        Me.Label92.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label92.Location = New System.Drawing.Point(570, 31)
        Me.Label92.Name = "Label92"
        Me.Label92.Size = New System.Drawing.Size(89, 12)
        Me.Label92.TabIndex = 0
        Me.Label92.Text = "ميزان بار تأمين شده"
        '
        'txtMSBar6
        '
        Me.txtMSBar6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMSBar6.CaptinText = ""
        Me.txtMSBar6.HasCaption = False
        Me.txtMSBar6.IsForceText = False
        Me.txtMSBar6.IsFractional = True
        Me.txtMSBar6.IsIP = False
        Me.txtMSBar6.IsNumberOnly = True
        Me.txtMSBar6.IsYear = False
        Me.txtMSBar6.Location = New System.Drawing.Point(494, 29)
        Me.txtMSBar6.Name = "txtMSBar6"
        Me.txtMSBar6.Size = New System.Drawing.Size(74, 21)
        Me.txtMSBar6.TabIndex = 1
        Me.txtMSBar6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtMSTime6
        '
        Me.txtMSTime6.BackColor = System.Drawing.SystemColors.Window
        Me.txtMSTime6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMSTime6.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMSTime6.IsShadow = False
        Me.txtMSTime6.IsShowCurrentTime = False
        Me.txtMSTime6.Location = New System.Drawing.Point(386, 4)
        Me.txtMSTime6.MaxLength = 5
        Me.txtMSTime6.MiladiDT = Nothing
        Me.txtMSTime6.Name = "txtMSTime6"
        Me.txtMSTime6.ReadOnly = True
        Me.txtMSTime6.ReadOnlyMaskedEdit = False
        Me.txtMSTime6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMSTime6.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtMSTime6.Size = New System.Drawing.Size(48, 21)
        Me.txtMSTime6.TabIndex = 0
        Me.txtMSTime6.Text = "__:__"
        Me.txtMSTime6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label93
        '
        Me.Label93.AutoSize = True
        Me.Label93.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label93.Location = New System.Drawing.Point(436, 8)
        Me.Label93.Name = "Label93"
        Me.Label93.Size = New System.Drawing.Size(56, 12)
        Me.Label93.TabIndex = 0
        Me.Label93.Text = "ساعت وصل"
        '
        'Label94
        '
        Me.Label94.AutoSize = True
        Me.Label94.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label94.Location = New System.Drawing.Point(339, 8)
        Me.Label94.Name = "Label94"
        Me.Label94.Size = New System.Drawing.Size(43, 12)
        Me.Label94.TabIndex = 0
        Me.Label94.Text = "توضيحات"
        '
        'Label105
        '
        Me.Label105.AutoSize = True
        Me.Label105.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label105.Location = New System.Drawing.Point(570, 8)
        Me.Label105.Name = "Label105"
        Me.Label105.Size = New System.Drawing.Size(48, 12)
        Me.Label105.TabIndex = 0
        Me.Label105.Text = "تاريخ وصل"
        '
        'txtMSDate6
        '
        Me.txtMSDate6.BackColor = System.Drawing.SystemColors.Window
        Me.txtMSDate6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMSDate6.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMSDate6.IntegerDate = 0
        Me.txtMSDate6.IsShadow = False
        Me.txtMSDate6.IsShowCurrentDate = False
        Me.txtMSDate6.Location = New System.Drawing.Point(494, 4)
        Me.txtMSDate6.MaxLength = 10
        Me.txtMSDate6.MiladiDT = CType(resources.GetObject("txtMSDate6.MiladiDT"), Object)
        Me.txtMSDate6.Name = "txtMSDate6"
        Me.txtMSDate6.ReadOnly = True
        Me.txtMSDate6.ReadOnlyMaskedEdit = False
        Me.txtMSDate6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMSDate6.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtMSDate6.ShamsiDT = "____/__/__"
        Me.txtMSDate6.Size = New System.Drawing.Size(74, 21)
        Me.txtMSDate6.TabIndex = 0
        Me.txtMSDate6.Text = "____/__/__"
        Me.txtMSDate6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtMSDate6.TimeMaskedEditorOBJ = Nothing
        '
        'btnFeederKey6
        '
        Me.btnFeederKey6.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFeederKey6.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnFeederKey6.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.btnFeederKey6.Location = New System.Drawing.Point(429, 28)
        Me.btnFeederKey6.Name = "btnFeederKey6"
        Me.btnFeederKey6.Size = New System.Drawing.Size(59, 22)
        Me.btnFeederKey6.TabIndex = 189
        Me.btnFeederKey6.Tag = "6"
        Me.btnFeederKey6.Text = "کليدزني"
        Me.btnFeederKey6.Visible = False
        '
        'pnlLevel5
        '
        Me.pnlLevel5.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlLevel5.Controls.Add(Me.txtMSDesc5)
        Me.pnlLevel5.Controls.Add(Me.Label17)
        Me.pnlLevel5.Controls.Add(Me.Label77)
        Me.pnlLevel5.Controls.Add(Me.txtMSBar5)
        Me.pnlLevel5.Controls.Add(Me.txtMSTime5)
        Me.pnlLevel5.Controls.Add(Me.Label78)
        Me.pnlLevel5.Controls.Add(Me.Label79)
        Me.pnlLevel5.Controls.Add(Me.Label104)
        Me.pnlLevel5.Controls.Add(Me.txtMSDate5)
        Me.pnlLevel5.Controls.Add(Me.btnFeederKey5)
        Me.pnlLevel5.Location = New System.Drawing.Point(7, 299)
        Me.pnlLevel5.Name = "pnlLevel5"
        Me.pnlLevel5.Size = New System.Drawing.Size(696, 56)
        Me.pnlLevel5.TabIndex = 4
        '
        'txtMSDesc5
        '
        Me.txtMSDesc5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMSDesc5.Location = New System.Drawing.Point(0, 3)
        Me.txtMSDesc5.Multiline = True
        Me.txtMSDesc5.Name = "txtMSDesc5"
        Me.txtMSDesc5.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtMSDesc5.Size = New System.Drawing.Size(336, 48)
        Me.txtMSDesc5.TabIndex = 3
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label17.Location = New System.Drawing.Point(620, 8)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(63, 12)
        Me.Label17.TabIndex = 0
        Me.Label17.Text = "مرحله پنجم:"
        '
        'Label77
        '
        Me.Label77.AutoSize = True
        Me.Label77.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label77.Location = New System.Drawing.Point(570, 31)
        Me.Label77.Name = "Label77"
        Me.Label77.Size = New System.Drawing.Size(89, 12)
        Me.Label77.TabIndex = 0
        Me.Label77.Text = "ميزان بار تأمين شده"
        '
        'txtMSBar5
        '
        Me.txtMSBar5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMSBar5.CaptinText = ""
        Me.txtMSBar5.HasCaption = False
        Me.txtMSBar5.IsForceText = False
        Me.txtMSBar5.IsFractional = True
        Me.txtMSBar5.IsIP = False
        Me.txtMSBar5.IsNumberOnly = True
        Me.txtMSBar5.IsYear = False
        Me.txtMSBar5.Location = New System.Drawing.Point(494, 29)
        Me.txtMSBar5.Name = "txtMSBar5"
        Me.txtMSBar5.Size = New System.Drawing.Size(74, 21)
        Me.txtMSBar5.TabIndex = 2
        Me.txtMSBar5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtMSTime5
        '
        Me.txtMSTime5.BackColor = System.Drawing.SystemColors.Window
        Me.txtMSTime5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMSTime5.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMSTime5.IsShadow = False
        Me.txtMSTime5.IsShowCurrentTime = False
        Me.txtMSTime5.Location = New System.Drawing.Point(386, 4)
        Me.txtMSTime5.MaxLength = 5
        Me.txtMSTime5.MiladiDT = Nothing
        Me.txtMSTime5.Name = "txtMSTime5"
        Me.txtMSTime5.ReadOnly = True
        Me.txtMSTime5.ReadOnlyMaskedEdit = False
        Me.txtMSTime5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMSTime5.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtMSTime5.Size = New System.Drawing.Size(48, 21)
        Me.txtMSTime5.TabIndex = 1
        Me.txtMSTime5.Text = "__:__"
        Me.txtMSTime5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label78
        '
        Me.Label78.AutoSize = True
        Me.Label78.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label78.Location = New System.Drawing.Point(436, 8)
        Me.Label78.Name = "Label78"
        Me.Label78.Size = New System.Drawing.Size(56, 12)
        Me.Label78.TabIndex = 0
        Me.Label78.Text = "ساعت وصل"
        '
        'Label79
        '
        Me.Label79.AutoSize = True
        Me.Label79.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label79.Location = New System.Drawing.Point(339, 8)
        Me.Label79.Name = "Label79"
        Me.Label79.Size = New System.Drawing.Size(43, 12)
        Me.Label79.TabIndex = 0
        Me.Label79.Text = "توضيحات"
        '
        'Label104
        '
        Me.Label104.AutoSize = True
        Me.Label104.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label104.Location = New System.Drawing.Point(570, 8)
        Me.Label104.Name = "Label104"
        Me.Label104.Size = New System.Drawing.Size(48, 12)
        Me.Label104.TabIndex = 0
        Me.Label104.Text = "تاريخ وصل"
        '
        'txtMSDate5
        '
        Me.txtMSDate5.BackColor = System.Drawing.SystemColors.Window
        Me.txtMSDate5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMSDate5.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMSDate5.IntegerDate = 0
        Me.txtMSDate5.IsShadow = False
        Me.txtMSDate5.IsShowCurrentDate = False
        Me.txtMSDate5.Location = New System.Drawing.Point(494, 4)
        Me.txtMSDate5.MaxLength = 10
        Me.txtMSDate5.MiladiDT = CType(resources.GetObject("txtMSDate5.MiladiDT"), Object)
        Me.txtMSDate5.Name = "txtMSDate5"
        Me.txtMSDate5.ReadOnly = True
        Me.txtMSDate5.ReadOnlyMaskedEdit = False
        Me.txtMSDate5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMSDate5.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtMSDate5.ShamsiDT = "____/__/__"
        Me.txtMSDate5.Size = New System.Drawing.Size(74, 21)
        Me.txtMSDate5.TabIndex = 0
        Me.txtMSDate5.Text = "____/__/__"
        Me.txtMSDate5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtMSDate5.TimeMaskedEditorOBJ = Nothing
        '
        'btnFeederKey5
        '
        Me.btnFeederKey5.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFeederKey5.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnFeederKey5.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.btnFeederKey5.Location = New System.Drawing.Point(429, 28)
        Me.btnFeederKey5.Name = "btnFeederKey5"
        Me.btnFeederKey5.Size = New System.Drawing.Size(59, 22)
        Me.btnFeederKey5.TabIndex = 189
        Me.btnFeederKey5.Tag = "5"
        Me.btnFeederKey5.Text = "کليدزني"
        Me.btnFeederKey5.Visible = False
        '
        'pnlLevel4
        '
        Me.pnlLevel4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlLevel4.Controls.Add(Me.txtMSDesc4)
        Me.pnlLevel4.Controls.Add(Me.Label80)
        Me.pnlLevel4.Controls.Add(Me.Label81)
        Me.pnlLevel4.Controls.Add(Me.txtMSBar4)
        Me.pnlLevel4.Controls.Add(Me.txtMSTime4)
        Me.pnlLevel4.Controls.Add(Me.Label82)
        Me.pnlLevel4.Controls.Add(Me.Label83)
        Me.pnlLevel4.Controls.Add(Me.Label103)
        Me.pnlLevel4.Controls.Add(Me.txtMSDate4)
        Me.pnlLevel4.Controls.Add(Me.btnFeederKey4)
        Me.pnlLevel4.Location = New System.Drawing.Point(7, 240)
        Me.pnlLevel4.Name = "pnlLevel4"
        Me.pnlLevel4.Size = New System.Drawing.Size(696, 56)
        Me.pnlLevel4.TabIndex = 3
        '
        'txtMSDesc4
        '
        Me.txtMSDesc4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMSDesc4.Location = New System.Drawing.Point(0, 3)
        Me.txtMSDesc4.Multiline = True
        Me.txtMSDesc4.Name = "txtMSDesc4"
        Me.txtMSDesc4.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtMSDesc4.Size = New System.Drawing.Size(336, 48)
        Me.txtMSDesc4.TabIndex = 3
        '
        'Label80
        '
        Me.Label80.AutoSize = True
        Me.Label80.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label80.Location = New System.Drawing.Point(620, 8)
        Me.Label80.Name = "Label80"
        Me.Label80.Size = New System.Drawing.Size(70, 12)
        Me.Label80.TabIndex = 0
        Me.Label80.Text = "مرحله چهارم:"
        '
        'Label81
        '
        Me.Label81.AutoSize = True
        Me.Label81.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label81.Location = New System.Drawing.Point(570, 31)
        Me.Label81.Name = "Label81"
        Me.Label81.Size = New System.Drawing.Size(89, 12)
        Me.Label81.TabIndex = 0
        Me.Label81.Text = "ميزان بار تأمين شده"
        '
        'txtMSBar4
        '
        Me.txtMSBar4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMSBar4.CaptinText = ""
        Me.txtMSBar4.HasCaption = False
        Me.txtMSBar4.IsForceText = False
        Me.txtMSBar4.IsFractional = True
        Me.txtMSBar4.IsIP = False
        Me.txtMSBar4.IsNumberOnly = True
        Me.txtMSBar4.IsYear = False
        Me.txtMSBar4.Location = New System.Drawing.Point(494, 29)
        Me.txtMSBar4.Name = "txtMSBar4"
        Me.txtMSBar4.Size = New System.Drawing.Size(74, 21)
        Me.txtMSBar4.TabIndex = 2
        Me.txtMSBar4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtMSTime4
        '
        Me.txtMSTime4.BackColor = System.Drawing.SystemColors.Window
        Me.txtMSTime4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMSTime4.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMSTime4.IsShadow = False
        Me.txtMSTime4.IsShowCurrentTime = False
        Me.txtMSTime4.Location = New System.Drawing.Point(386, 4)
        Me.txtMSTime4.MaxLength = 5
        Me.txtMSTime4.MiladiDT = Nothing
        Me.txtMSTime4.Name = "txtMSTime4"
        Me.txtMSTime4.ReadOnly = True
        Me.txtMSTime4.ReadOnlyMaskedEdit = False
        Me.txtMSTime4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMSTime4.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtMSTime4.Size = New System.Drawing.Size(48, 21)
        Me.txtMSTime4.TabIndex = 1
        Me.txtMSTime4.Text = "__:__"
        Me.txtMSTime4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label82
        '
        Me.Label82.AutoSize = True
        Me.Label82.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label82.Location = New System.Drawing.Point(436, 8)
        Me.Label82.Name = "Label82"
        Me.Label82.Size = New System.Drawing.Size(56, 12)
        Me.Label82.TabIndex = 0
        Me.Label82.Text = "ساعت وصل"
        '
        'Label83
        '
        Me.Label83.AutoSize = True
        Me.Label83.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label83.Location = New System.Drawing.Point(339, 8)
        Me.Label83.Name = "Label83"
        Me.Label83.Size = New System.Drawing.Size(43, 12)
        Me.Label83.TabIndex = 0
        Me.Label83.Text = "توضيحات"
        '
        'Label103
        '
        Me.Label103.AutoSize = True
        Me.Label103.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label103.Location = New System.Drawing.Point(570, 8)
        Me.Label103.Name = "Label103"
        Me.Label103.Size = New System.Drawing.Size(48, 12)
        Me.Label103.TabIndex = 0
        Me.Label103.Text = "تاريخ وصل"
        '
        'txtMSDate4
        '
        Me.txtMSDate4.BackColor = System.Drawing.SystemColors.Window
        Me.txtMSDate4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMSDate4.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMSDate4.IntegerDate = 0
        Me.txtMSDate4.IsShadow = False
        Me.txtMSDate4.IsShowCurrentDate = False
        Me.txtMSDate4.Location = New System.Drawing.Point(494, 4)
        Me.txtMSDate4.MaxLength = 10
        Me.txtMSDate4.MiladiDT = CType(resources.GetObject("txtMSDate4.MiladiDT"), Object)
        Me.txtMSDate4.Name = "txtMSDate4"
        Me.txtMSDate4.ReadOnly = True
        Me.txtMSDate4.ReadOnlyMaskedEdit = False
        Me.txtMSDate4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMSDate4.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtMSDate4.ShamsiDT = "____/__/__"
        Me.txtMSDate4.Size = New System.Drawing.Size(74, 21)
        Me.txtMSDate4.TabIndex = 0
        Me.txtMSDate4.Text = "____/__/__"
        Me.txtMSDate4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtMSDate4.TimeMaskedEditorOBJ = Nothing
        '
        'btnFeederKey4
        '
        Me.btnFeederKey4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFeederKey4.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnFeederKey4.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.btnFeederKey4.Location = New System.Drawing.Point(429, 28)
        Me.btnFeederKey4.Name = "btnFeederKey4"
        Me.btnFeederKey4.Size = New System.Drawing.Size(59, 22)
        Me.btnFeederKey4.TabIndex = 189
        Me.btnFeederKey4.Tag = "4"
        Me.btnFeederKey4.Text = "کليدزني"
        Me.btnFeederKey4.Visible = False
        '
        'pnlLevel3
        '
        Me.pnlLevel3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlLevel3.Controls.Add(Me.txtMSDesc3)
        Me.pnlLevel3.Controls.Add(Me.Label84)
        Me.pnlLevel3.Controls.Add(Me.Label85)
        Me.pnlLevel3.Controls.Add(Me.txtMSBar3)
        Me.pnlLevel3.Controls.Add(Me.txtMSTime3)
        Me.pnlLevel3.Controls.Add(Me.Label86)
        Me.pnlLevel3.Controls.Add(Me.Label87)
        Me.pnlLevel3.Controls.Add(Me.Label102)
        Me.pnlLevel3.Controls.Add(Me.txtMSDate3)
        Me.pnlLevel3.Controls.Add(Me.btnFeederKey3)
        Me.pnlLevel3.Location = New System.Drawing.Point(7, 181)
        Me.pnlLevel3.Name = "pnlLevel3"
        Me.pnlLevel3.Size = New System.Drawing.Size(696, 56)
        Me.pnlLevel3.TabIndex = 2
        '
        'txtMSDesc3
        '
        Me.txtMSDesc3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMSDesc3.Location = New System.Drawing.Point(0, 3)
        Me.txtMSDesc3.Multiline = True
        Me.txtMSDesc3.Name = "txtMSDesc3"
        Me.txtMSDesc3.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtMSDesc3.Size = New System.Drawing.Size(336, 48)
        Me.txtMSDesc3.TabIndex = 3
        '
        'Label84
        '
        Me.Label84.AutoSize = True
        Me.Label84.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label84.Location = New System.Drawing.Point(620, 8)
        Me.Label84.Name = "Label84"
        Me.Label84.Size = New System.Drawing.Size(65, 12)
        Me.Label84.TabIndex = 0
        Me.Label84.Text = "مرحله سوم:"
        '
        'Label85
        '
        Me.Label85.AutoSize = True
        Me.Label85.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label85.Location = New System.Drawing.Point(570, 31)
        Me.Label85.Name = "Label85"
        Me.Label85.Size = New System.Drawing.Size(89, 12)
        Me.Label85.TabIndex = 0
        Me.Label85.Text = "ميزان بار تأمين شده"
        '
        'txtMSBar3
        '
        Me.txtMSBar3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMSBar3.CaptinText = ""
        Me.txtMSBar3.HasCaption = False
        Me.txtMSBar3.IsForceText = False
        Me.txtMSBar3.IsFractional = True
        Me.txtMSBar3.IsIP = False
        Me.txtMSBar3.IsNumberOnly = True
        Me.txtMSBar3.IsYear = False
        Me.txtMSBar3.Location = New System.Drawing.Point(494, 29)
        Me.txtMSBar3.Name = "txtMSBar3"
        Me.txtMSBar3.Size = New System.Drawing.Size(74, 21)
        Me.txtMSBar3.TabIndex = 2
        Me.txtMSBar3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtMSTime3
        '
        Me.txtMSTime3.BackColor = System.Drawing.SystemColors.Window
        Me.txtMSTime3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMSTime3.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMSTime3.IsShadow = False
        Me.txtMSTime3.IsShowCurrentTime = False
        Me.txtMSTime3.Location = New System.Drawing.Point(386, 4)
        Me.txtMSTime3.MaxLength = 5
        Me.txtMSTime3.MiladiDT = Nothing
        Me.txtMSTime3.Name = "txtMSTime3"
        Me.txtMSTime3.ReadOnly = True
        Me.txtMSTime3.ReadOnlyMaskedEdit = False
        Me.txtMSTime3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMSTime3.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtMSTime3.Size = New System.Drawing.Size(48, 21)
        Me.txtMSTime3.TabIndex = 1
        Me.txtMSTime3.Text = "__:__"
        Me.txtMSTime3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label86
        '
        Me.Label86.AutoSize = True
        Me.Label86.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label86.Location = New System.Drawing.Point(436, 8)
        Me.Label86.Name = "Label86"
        Me.Label86.Size = New System.Drawing.Size(56, 12)
        Me.Label86.TabIndex = 0
        Me.Label86.Text = "ساعت وصل"
        '
        'Label87
        '
        Me.Label87.AutoSize = True
        Me.Label87.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label87.Location = New System.Drawing.Point(339, 8)
        Me.Label87.Name = "Label87"
        Me.Label87.Size = New System.Drawing.Size(43, 12)
        Me.Label87.TabIndex = 0
        Me.Label87.Text = "توضيحات"
        '
        'Label102
        '
        Me.Label102.AutoSize = True
        Me.Label102.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label102.Location = New System.Drawing.Point(570, 8)
        Me.Label102.Name = "Label102"
        Me.Label102.Size = New System.Drawing.Size(48, 12)
        Me.Label102.TabIndex = 0
        Me.Label102.Text = "تاريخ وصل"
        '
        'txtMSDate3
        '
        Me.txtMSDate3.BackColor = System.Drawing.SystemColors.Window
        Me.txtMSDate3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMSDate3.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMSDate3.IntegerDate = 0
        Me.txtMSDate3.IsShadow = False
        Me.txtMSDate3.IsShowCurrentDate = False
        Me.txtMSDate3.Location = New System.Drawing.Point(494, 4)
        Me.txtMSDate3.MaxLength = 10
        Me.txtMSDate3.MiladiDT = CType(resources.GetObject("txtMSDate3.MiladiDT"), Object)
        Me.txtMSDate3.Name = "txtMSDate3"
        Me.txtMSDate3.ReadOnly = True
        Me.txtMSDate3.ReadOnlyMaskedEdit = False
        Me.txtMSDate3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMSDate3.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtMSDate3.ShamsiDT = "____/__/__"
        Me.txtMSDate3.Size = New System.Drawing.Size(74, 21)
        Me.txtMSDate3.TabIndex = 0
        Me.txtMSDate3.Text = "____/__/__"
        Me.txtMSDate3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtMSDate3.TimeMaskedEditorOBJ = Nothing
        '
        'btnFeederKey3
        '
        Me.btnFeederKey3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFeederKey3.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnFeederKey3.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.btnFeederKey3.Location = New System.Drawing.Point(429, 28)
        Me.btnFeederKey3.Name = "btnFeederKey3"
        Me.btnFeederKey3.Size = New System.Drawing.Size(59, 22)
        Me.btnFeederKey3.TabIndex = 189
        Me.btnFeederKey3.Tag = "3"
        Me.btnFeederKey3.Text = "کليدزني"
        Me.btnFeederKey3.Visible = False
        '
        'pnlLevel2
        '
        Me.pnlLevel2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlLevel2.Controls.Add(Me.txtMSDesc2)
        Me.pnlLevel2.Controls.Add(Me.Label88)
        Me.pnlLevel2.Controls.Add(Me.Label89)
        Me.pnlLevel2.Controls.Add(Me.txtMSBar2)
        Me.pnlLevel2.Controls.Add(Me.txtMSTime2)
        Me.pnlLevel2.Controls.Add(Me.Label90)
        Me.pnlLevel2.Controls.Add(Me.Label95)
        Me.pnlLevel2.Controls.Add(Me.Label101)
        Me.pnlLevel2.Controls.Add(Me.txtMSDate2)
        Me.pnlLevel2.Controls.Add(Me.btnFeederKey2)
        Me.pnlLevel2.Location = New System.Drawing.Point(7, 122)
        Me.pnlLevel2.Name = "pnlLevel2"
        Me.pnlLevel2.Size = New System.Drawing.Size(696, 56)
        Me.pnlLevel2.TabIndex = 1
        '
        'txtMSDesc2
        '
        Me.txtMSDesc2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMSDesc2.Location = New System.Drawing.Point(0, 3)
        Me.txtMSDesc2.Multiline = True
        Me.txtMSDesc2.Name = "txtMSDesc2"
        Me.txtMSDesc2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtMSDesc2.Size = New System.Drawing.Size(336, 48)
        Me.txtMSDesc2.TabIndex = 3
        '
        'Label88
        '
        Me.Label88.AutoSize = True
        Me.Label88.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label88.Location = New System.Drawing.Point(620, 8)
        Me.Label88.Name = "Label88"
        Me.Label88.Size = New System.Drawing.Size(58, 12)
        Me.Label88.TabIndex = 0
        Me.Label88.Text = "مرحله دوم:"
        '
        'Label89
        '
        Me.Label89.AutoSize = True
        Me.Label89.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label89.Location = New System.Drawing.Point(570, 31)
        Me.Label89.Name = "Label89"
        Me.Label89.Size = New System.Drawing.Size(89, 12)
        Me.Label89.TabIndex = 0
        Me.Label89.Text = "ميزان بار تأمين شده"
        '
        'txtMSBar2
        '
        Me.txtMSBar2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMSBar2.CaptinText = ""
        Me.txtMSBar2.HasCaption = False
        Me.txtMSBar2.IsForceText = False
        Me.txtMSBar2.IsFractional = True
        Me.txtMSBar2.IsIP = False
        Me.txtMSBar2.IsNumberOnly = True
        Me.txtMSBar2.IsYear = False
        Me.txtMSBar2.Location = New System.Drawing.Point(494, 29)
        Me.txtMSBar2.Name = "txtMSBar2"
        Me.txtMSBar2.Size = New System.Drawing.Size(74, 21)
        Me.txtMSBar2.TabIndex = 2
        Me.txtMSBar2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtMSTime2
        '
        Me.txtMSTime2.BackColor = System.Drawing.SystemColors.Window
        Me.txtMSTime2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMSTime2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMSTime2.IsShadow = False
        Me.txtMSTime2.IsShowCurrentTime = False
        Me.txtMSTime2.Location = New System.Drawing.Point(386, 4)
        Me.txtMSTime2.MaxLength = 5
        Me.txtMSTime2.MiladiDT = Nothing
        Me.txtMSTime2.Name = "txtMSTime2"
        Me.txtMSTime2.ReadOnly = True
        Me.txtMSTime2.ReadOnlyMaskedEdit = False
        Me.txtMSTime2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMSTime2.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtMSTime2.Size = New System.Drawing.Size(48, 21)
        Me.txtMSTime2.TabIndex = 1
        Me.txtMSTime2.Text = "__:__"
        Me.txtMSTime2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label90
        '
        Me.Label90.AutoSize = True
        Me.Label90.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label90.Location = New System.Drawing.Point(436, 8)
        Me.Label90.Name = "Label90"
        Me.Label90.Size = New System.Drawing.Size(56, 12)
        Me.Label90.TabIndex = 0
        Me.Label90.Text = "ساعت وصل"
        '
        'Label95
        '
        Me.Label95.AutoSize = True
        Me.Label95.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label95.Location = New System.Drawing.Point(339, 8)
        Me.Label95.Name = "Label95"
        Me.Label95.Size = New System.Drawing.Size(43, 12)
        Me.Label95.TabIndex = 0
        Me.Label95.Text = "توضيحات"
        '
        'Label101
        '
        Me.Label101.AutoSize = True
        Me.Label101.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label101.Location = New System.Drawing.Point(570, 8)
        Me.Label101.Name = "Label101"
        Me.Label101.Size = New System.Drawing.Size(48, 12)
        Me.Label101.TabIndex = 0
        Me.Label101.Text = "تاريخ وصل"
        '
        'txtMSDate2
        '
        Me.txtMSDate2.BackColor = System.Drawing.SystemColors.Window
        Me.txtMSDate2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMSDate2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMSDate2.IntegerDate = 0
        Me.txtMSDate2.IsShadow = False
        Me.txtMSDate2.IsShowCurrentDate = False
        Me.txtMSDate2.Location = New System.Drawing.Point(494, 4)
        Me.txtMSDate2.MaxLength = 10
        Me.txtMSDate2.MiladiDT = CType(resources.GetObject("txtMSDate2.MiladiDT"), Object)
        Me.txtMSDate2.Name = "txtMSDate2"
        Me.txtMSDate2.ReadOnly = True
        Me.txtMSDate2.ReadOnlyMaskedEdit = False
        Me.txtMSDate2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMSDate2.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtMSDate2.ShamsiDT = "____/__/__"
        Me.txtMSDate2.Size = New System.Drawing.Size(74, 21)
        Me.txtMSDate2.TabIndex = 0
        Me.txtMSDate2.Text = "____/__/__"
        Me.txtMSDate2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtMSDate2.TimeMaskedEditorOBJ = Nothing
        '
        'btnFeederKey2
        '
        Me.btnFeederKey2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFeederKey2.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnFeederKey2.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.btnFeederKey2.Location = New System.Drawing.Point(429, 28)
        Me.btnFeederKey2.Name = "btnFeederKey2"
        Me.btnFeederKey2.Size = New System.Drawing.Size(59, 22)
        Me.btnFeederKey2.TabIndex = 189
        Me.btnFeederKey2.Tag = "2"
        Me.btnFeederKey2.Text = "کليدزني"
        Me.btnFeederKey2.Visible = False
        '
        'pnlLevel1
        '
        Me.pnlLevel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlLevel1.Controls.Add(Me.btnFeederKey1)
        Me.pnlLevel1.Controls.Add(Me.txtMSDate1)
        Me.pnlLevel1.Controls.Add(Me.txtMSDesc1)
        Me.pnlLevel1.Controls.Add(Me.Label96)
        Me.pnlLevel1.Controls.Add(Me.Label97)
        Me.pnlLevel1.Controls.Add(Me.txtMSBar1)
        Me.pnlLevel1.Controls.Add(Me.txtMSTime1)
        Me.pnlLevel1.Controls.Add(Me.Label98)
        Me.pnlLevel1.Controls.Add(Me.Label99)
        Me.pnlLevel1.Controls.Add(Me.Label100)
        Me.pnlLevel1.Location = New System.Drawing.Point(7, 63)
        Me.pnlLevel1.Name = "pnlLevel1"
        Me.pnlLevel1.Size = New System.Drawing.Size(696, 56)
        Me.pnlLevel1.TabIndex = 0
        '
        'btnFeederKey1
        '
        Me.btnFeederKey1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFeederKey1.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnFeederKey1.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.btnFeederKey1.Location = New System.Drawing.Point(429, 28)
        Me.btnFeederKey1.Name = "btnFeederKey1"
        Me.btnFeederKey1.Size = New System.Drawing.Size(59, 22)
        Me.btnFeederKey1.TabIndex = 189
        Me.btnFeederKey1.Tag = "1"
        Me.btnFeederKey1.Text = "کليدزني"
        Me.btnFeederKey1.Visible = False
        '
        'txtMSDate1
        '
        Me.txtMSDate1.BackColor = System.Drawing.SystemColors.Window
        Me.txtMSDate1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMSDate1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMSDate1.IntegerDate = 0
        Me.txtMSDate1.IsShadow = False
        Me.txtMSDate1.IsShowCurrentDate = False
        Me.txtMSDate1.Location = New System.Drawing.Point(494, 4)
        Me.txtMSDate1.MaxLength = 10
        Me.txtMSDate1.MiladiDT = CType(resources.GetObject("txtMSDate1.MiladiDT"), Object)
        Me.txtMSDate1.Name = "txtMSDate1"
        Me.txtMSDate1.ReadOnly = True
        Me.txtMSDate1.ReadOnlyMaskedEdit = False
        Me.txtMSDate1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMSDate1.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtMSDate1.ShamsiDT = "____/__/__"
        Me.txtMSDate1.Size = New System.Drawing.Size(74, 21)
        Me.txtMSDate1.TabIndex = 0
        Me.txtMSDate1.Text = "____/__/__"
        Me.txtMSDate1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtMSDate1.TimeMaskedEditorOBJ = Nothing
        '
        'txtMSDesc1
        '
        Me.txtMSDesc1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMSDesc1.Location = New System.Drawing.Point(0, 3)
        Me.txtMSDesc1.Multiline = True
        Me.txtMSDesc1.Name = "txtMSDesc1"
        Me.txtMSDesc1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtMSDesc1.Size = New System.Drawing.Size(336, 48)
        Me.txtMSDesc1.TabIndex = 3
        '
        'Label96
        '
        Me.Label96.AutoSize = True
        Me.Label96.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label96.Location = New System.Drawing.Point(620, 8)
        Me.Label96.Name = "Label96"
        Me.Label96.Size = New System.Drawing.Size(56, 12)
        Me.Label96.TabIndex = 0
        Me.Label96.Text = "مرحله اول:"
        '
        'Label97
        '
        Me.Label97.AutoSize = True
        Me.Label97.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label97.Location = New System.Drawing.Point(570, 31)
        Me.Label97.Name = "Label97"
        Me.Label97.Size = New System.Drawing.Size(89, 12)
        Me.Label97.TabIndex = 0
        Me.Label97.Text = "ميزان بار تأمين شده"
        '
        'txtMSBar1
        '
        Me.txtMSBar1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMSBar1.CaptinText = ""
        Me.txtMSBar1.HasCaption = False
        Me.txtMSBar1.IsForceText = False
        Me.txtMSBar1.IsFractional = True
        Me.txtMSBar1.IsIP = False
        Me.txtMSBar1.IsNumberOnly = True
        Me.txtMSBar1.IsYear = False
        Me.txtMSBar1.Location = New System.Drawing.Point(494, 29)
        Me.txtMSBar1.Name = "txtMSBar1"
        Me.txtMSBar1.Size = New System.Drawing.Size(74, 21)
        Me.txtMSBar1.TabIndex = 2
        Me.txtMSBar1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtMSTime1
        '
        Me.txtMSTime1.BackColor = System.Drawing.SystemColors.Window
        Me.txtMSTime1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMSTime1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMSTime1.IsShadow = False
        Me.txtMSTime1.IsShowCurrentTime = False
        Me.txtMSTime1.Location = New System.Drawing.Point(386, 4)
        Me.txtMSTime1.MaxLength = 5
        Me.txtMSTime1.MiladiDT = Nothing
        Me.txtMSTime1.Name = "txtMSTime1"
        Me.txtMSTime1.ReadOnly = True
        Me.txtMSTime1.ReadOnlyMaskedEdit = False
        Me.txtMSTime1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMSTime1.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtMSTime1.Size = New System.Drawing.Size(48, 21)
        Me.txtMSTime1.TabIndex = 1
        Me.txtMSTime1.Text = "__:__"
        Me.txtMSTime1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label98
        '
        Me.Label98.AutoSize = True
        Me.Label98.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label98.Location = New System.Drawing.Point(436, 8)
        Me.Label98.Name = "Label98"
        Me.Label98.Size = New System.Drawing.Size(56, 12)
        Me.Label98.TabIndex = 0
        Me.Label98.Text = "ساعت وصل"
        '
        'Label99
        '
        Me.Label99.AutoSize = True
        Me.Label99.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label99.Location = New System.Drawing.Point(339, 8)
        Me.Label99.Name = "Label99"
        Me.Label99.Size = New System.Drawing.Size(43, 12)
        Me.Label99.TabIndex = 0
        Me.Label99.Text = "توضيحات"
        '
        'Label100
        '
        Me.Label100.AutoSize = True
        Me.Label100.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label100.Location = New System.Drawing.Point(570, 8)
        Me.Label100.Name = "Label100"
        Me.Label100.Size = New System.Drawing.Size(48, 12)
        Me.Label100.TabIndex = 0
        Me.Label100.Text = "تاريخ وصل"
        '
        'HatchPanel7
        '
        Me.HatchPanel7.Direction = Bargh_Common.HatchPanel.LeftRight.Right
        Me.HatchPanel7.HatchColor = System.Drawing.Color.DarkBlue
        Me.HatchPanel7.HatchCount = 1.0!
        Me.HatchPanel7.HatchSpaceWidth = 20.0!
        Me.HatchPanel7.HatchStyle = System.Drawing.Drawing2D.DashStyle.Dash
        Me.HatchPanel7.HatchWidth = 1.0!
        Me.HatchPanel7.Location = New System.Drawing.Point(7, 178)
        Me.HatchPanel7.Name = "HatchPanel7"
        Me.HatchPanel7.Size = New System.Drawing.Size(696, 3)
        Me.HatchPanel7.TabIndex = 8
        '
        'HatchPanel8
        '
        Me.HatchPanel8.Direction = Bargh_Common.HatchPanel.LeftRight.Right
        Me.HatchPanel8.HatchColor = System.Drawing.Color.DarkBlue
        Me.HatchPanel8.HatchCount = 1.0!
        Me.HatchPanel8.HatchSpaceWidth = 20.0!
        Me.HatchPanel8.HatchStyle = System.Drawing.Drawing2D.DashStyle.Dash
        Me.HatchPanel8.HatchWidth = 1.0!
        Me.HatchPanel8.Location = New System.Drawing.Point(7, 237)
        Me.HatchPanel8.Name = "HatchPanel8"
        Me.HatchPanel8.Size = New System.Drawing.Size(696, 3)
        Me.HatchPanel8.TabIndex = 8
        '
        'HatchPanel9
        '
        Me.HatchPanel9.Direction = Bargh_Common.HatchPanel.LeftRight.Right
        Me.HatchPanel9.HatchColor = System.Drawing.Color.DarkBlue
        Me.HatchPanel9.HatchCount = 1.0!
        Me.HatchPanel9.HatchSpaceWidth = 20.0!
        Me.HatchPanel9.HatchStyle = System.Drawing.Drawing2D.DashStyle.Dash
        Me.HatchPanel9.HatchWidth = 1.0!
        Me.HatchPanel9.Location = New System.Drawing.Point(7, 296)
        Me.HatchPanel9.Name = "HatchPanel9"
        Me.HatchPanel9.Size = New System.Drawing.Size(696, 3)
        Me.HatchPanel9.TabIndex = 8
        '
        'HatchPanel10
        '
        Me.HatchPanel10.Direction = Bargh_Common.HatchPanel.LeftRight.Right
        Me.HatchPanel10.HatchColor = System.Drawing.Color.DarkBlue
        Me.HatchPanel10.HatchCount = 1.0!
        Me.HatchPanel10.HatchSpaceWidth = 20.0!
        Me.HatchPanel10.HatchStyle = System.Drawing.Drawing2D.DashStyle.Dash
        Me.HatchPanel10.HatchWidth = 1.0!
        Me.HatchPanel10.Location = New System.Drawing.Point(7, 355)
        Me.HatchPanel10.Name = "HatchPanel10"
        Me.HatchPanel10.Size = New System.Drawing.Size(696, 3)
        Me.HatchPanel10.TabIndex = 8
        '
        'HatchPanel11
        '
        Me.HatchPanel11.Direction = Bargh_Common.HatchPanel.LeftRight.Right
        Me.HatchPanel11.HatchColor = System.Drawing.Color.DarkBlue
        Me.HatchPanel11.HatchCount = 1.0!
        Me.HatchPanel11.HatchSpaceWidth = 20.0!
        Me.HatchPanel11.HatchStyle = System.Drawing.Drawing2D.DashStyle.Dash
        Me.HatchPanel11.HatchWidth = 1.0!
        Me.HatchPanel11.Location = New System.Drawing.Point(7, 414)
        Me.HatchPanel11.Name = "HatchPanel11"
        Me.HatchPanel11.Size = New System.Drawing.Size(696, 3)
        Me.HatchPanel11.TabIndex = 8
        '
        'Label106
        '
        Me.Label106.AutoSize = True
        Me.Label106.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label106.Location = New System.Drawing.Point(576, 25)
        Me.Label106.Name = "Label106"
        Me.Label106.Size = New System.Drawing.Size(108, 12)
        Me.Label106.TabIndex = 0
        Me.Label106.Text = "بار به هنگام قطع اوليه"
        '
        'txtDCCurrent
        '
        Me.txtDCCurrent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDCCurrent.CaptinText = ""
        Me.txtDCCurrent.HasCaption = False
        Me.txtDCCurrent.IsForceText = False
        Me.txtDCCurrent.IsFractional = True
        Me.txtDCCurrent.IsIP = False
        Me.txtDCCurrent.IsNumberOnly = True
        Me.txtDCCurrent.IsYear = False
        Me.txtDCCurrent.Location = New System.Drawing.Point(501, 22)
        Me.txtDCCurrent.Name = "txtDCCurrent"
        Me.txtDCCurrent.Size = New System.Drawing.Size(74, 21)
        Me.txtDCCurrent.TabIndex = 2
        Me.txtDCCurrent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'HatchPanel12
        '
        Me.HatchPanel12.Direction = Bargh_Common.HatchPanel.LeftRight.Right
        Me.HatchPanel12.HatchColor = System.Drawing.Color.DarkBlue
        Me.HatchPanel12.HatchCount = 1.0!
        Me.HatchPanel12.HatchSpaceWidth = 20.0!
        Me.HatchPanel12.HatchStyle = System.Drawing.Drawing2D.DashStyle.Dash
        Me.HatchPanel12.HatchWidth = 1.0!
        Me.HatchPanel12.Location = New System.Drawing.Point(7, 60)
        Me.HatchPanel12.Name = "HatchPanel12"
        Me.HatchPanel12.Size = New System.Drawing.Size(696, 3)
        Me.HatchPanel12.TabIndex = 8
        '
        'tp1_Specs
        '
        Me.tp1_Specs.Controls.Add(Me.txtNazerSearch)
        Me.tp1_Specs.Controls.Add(Me.pnlTamirRequestSubject)
        Me.tp1_Specs.Controls.Add(Me.Panel2)
        Me.tp1_Specs.Controls.Add(Me.pnlTamirType)
        Me.tp1_Specs.Controls.Add(Me.Panel3)
        Me.tp1_Specs.Controls.Add(Me.Panel1)
        Me.tp1_Specs.Controls.Add(Me.pnlNumbers)
        Me.tp1_Specs.Controls.Add(Me.pnlConfirmInfo)
        Me.tp1_Specs.Controls.Add(Me.pnlNazer)
        Me.tp1_Specs.Controls.Add(Me.pnlWorkCommandNo)
        Me.tp1_Specs.Controls.Add(Me.Panel4)
        Me.tp1_Specs.Location = New System.Drawing.Point(4, 22)
        Me.tp1_Specs.Name = "tp1_Specs"
        Me.tp1_Specs.Size = New System.Drawing.Size(728, 486)
        Me.tp1_Specs.TabIndex = 3
        Me.tp1_Specs.Text = "مشخصات کلي"
        '
        'txtNazerSearch
        '
        Me.txtNazerSearch.AccessibleName = "picNazerSearch"
        Me.txtNazerSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtNazerSearch.CaptinText = "پيمانکار"
        Me.txtNazerSearch.HasCaption = False
        Me.txtNazerSearch.IsForceText = False
        Me.txtNazerSearch.IsFractional = True
        Me.txtNazerSearch.IsIP = False
        Me.txtNazerSearch.IsNumberOnly = False
        Me.txtNazerSearch.IsYear = False
        Me.txtNazerSearch.Location = New System.Drawing.Point(500, 364)
        Me.txtNazerSearch.Name = "txtNazerSearch"
        Me.txtNazerSearch.Size = New System.Drawing.Size(102, 21)
        Me.txtNazerSearch.TabIndex = 220
        Me.txtNazerSearch.Visible = False
        '
        'pnlTamirRequestSubject
        '
        Me.pnlTamirRequestSubject.Controls.Add(Me.Label6)
        Me.pnlTamirRequestSubject.Controls.Add(Me.cmbTamirRequestSubject)
        Me.pnlTamirRequestSubject.Location = New System.Drawing.Point(36, 96)
        Me.pnlTamirRequestSubject.Name = "pnlTamirRequestSubject"
        Me.pnlTamirRequestSubject.Size = New System.Drawing.Size(316, 30)
        Me.pnlTamirRequestSubject.TabIndex = 2
        '
        'Label6
        '
        Me.Label6.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label6.Location = New System.Drawing.Point(200, 7)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(99, 13)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "موضوع درخواست"
        '
        'cmbTamirRequestSubject
        '
        Me.cmbTamirRequestSubject.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbTamirRequestSubject.BackColor = System.Drawing.Color.White
        Me.cmbTamirRequestSubject.DataBindings.Add(New System.Windows.Forms.Binding("SelectedValue", Me.DatasetDummy1, "ViewDummyId.Dummy13Id", True))
        Me.cmbTamirRequestSubject.DataSource = Me.DatasetBT1.Tbl_TamirRequestSubject
        Me.cmbTamirRequestSubject.DisplayMember = "TamirRequestSubject"
        Me.cmbTamirRequestSubject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTamirRequestSubject.IsReadOnly = False
        Me.cmbTamirRequestSubject.Location = New System.Drawing.Point(6, 5)
        Me.cmbTamirRequestSubject.Name = "cmbTamirRequestSubject"
        Me.cmbTamirRequestSubject.Size = New System.Drawing.Size(184, 21)
        Me.cmbTamirRequestSubject.TabIndex = 0
        Me.cmbTamirRequestSubject.ValueMember = "TamirRequestSubjectId"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.Label12)
        Me.Panel2.Controls.Add(Me.cmbState)
        Me.Panel2.Location = New System.Drawing.Point(332, 356)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(360, 24)
        Me.Panel2.TabIndex = 5
        '
        'Label12
        '
        Me.Label12.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label12.Location = New System.Drawing.Point(192, 3)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(99, 13)
        Me.Label12.TabIndex = 0
        Me.Label12.Text = "وضعيت درخواست"
        '
        'cmbState
        '
        Me.cmbState.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbState.BackColor = System.Drawing.Color.FromArgb(CType(CType(223, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(223, Byte), Integer))
        Me.cmbState.DataSource = Me.DatasetTamir1.Tbl_TamirRequestState
        Me.cmbState.DisplayMember = "TamirRequestState"
        Me.cmbState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.cmbState.IsReadOnly = True
        Me.cmbState.Location = New System.Drawing.Point(24, 1)
        Me.cmbState.Name = "cmbState"
        Me.cmbState.Size = New System.Drawing.Size(160, 21)
        Me.cmbState.TabIndex = 4
        Me.cmbState.ValueMember = "TamirRequestStateId"
        '
        'pnlTamirType
        '
        Me.pnlTamirType.Controls.Add(Me.Label76)
        Me.pnlTamirType.Controls.Add(Me.cmbTamirType)
        Me.pnlTamirType.Location = New System.Drawing.Point(356, 96)
        Me.pnlTamirType.Name = "pnlTamirType"
        Me.pnlTamirType.Size = New System.Drawing.Size(336, 30)
        Me.pnlTamirType.TabIndex = 1
        '
        'Label76
        '
        Me.Label76.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label76.AutoSize = True
        Me.Label76.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label76.Location = New System.Drawing.Point(200, 7)
        Me.Label76.Name = "Label76"
        Me.Label76.Size = New System.Drawing.Size(68, 13)
        Me.Label76.TabIndex = 0
        Me.Label76.Text = "نوع موافقت"
        '
        'cmbTamirType
        '
        Me.cmbTamirType.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbTamirType.BackColor = System.Drawing.Color.White
        Me.cmbTamirType.DataBindings.Add(New System.Windows.Forms.Binding("SelectedValue", Me.DatasetDummy1, "ViewDummyId.Dummy12Id", True))
        Me.cmbTamirType.DataSource = Me.DatasetBT1.Tbl_TamirType
        Me.cmbTamirType.DisplayMember = "TamirType"
        Me.cmbTamirType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTamirType.IsReadOnly = False
        Me.cmbTamirType.Location = New System.Drawing.Point(6, 5)
        Me.cmbTamirType.Name = "cmbTamirType"
        Me.cmbTamirType.Size = New System.Drawing.Size(184, 21)
        Me.cmbTamirType.TabIndex = 0
        Me.cmbTamirType.ValueMember = "TamirTypeId"
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.chkIsIRIB)
        Me.Panel3.Controls.Add(Me.Label10)
        Me.Panel3.Controls.Add(Me.txtSMSCOunt)
        Me.Panel3.Location = New System.Drawing.Point(36, 359)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(288, 80)
        Me.Panel3.TabIndex = 8
        '
        'chkIsIRIB
        '
        Me.chkIsIRIB.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkIsIRIB.Enabled = False
        Me.chkIsIRIB.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.chkIsIRIB.Location = New System.Drawing.Point(11, 55)
        Me.chkIsIRIB.Name = "chkIsIRIB"
        Me.chkIsIRIB.Size = New System.Drawing.Size(266, 24)
        Me.chkIsIRIB.TabIndex = 5
        Me.chkIsIRIB.Text = "اطلاع رساني به صدا و سيما انجام شده است."
        '
        'Label10
        '
        Me.Label10.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label10.Location = New System.Drawing.Point(26, 8)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(242, 13)
        Me.Label10.TabIndex = 0
        Me.Label10.Text = "تعداد کاربر حساس اطلاع رساني شده با SMS"
        '
        'txtSMSCOunt
        '
        Me.txtSMSCOunt.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSMSCOunt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSMSCOunt.Location = New System.Drawing.Point(100, 31)
        Me.txtSMSCOunt.Name = "txtSMSCOunt"
        Me.txtSMSCOunt.ReadOnly = True
        Me.txtSMSCOunt.Size = New System.Drawing.Size(88, 21)
        Me.txtSMSCOunt.TabIndex = 4
        Me.txtSMSCOunt.Text = "0"
        Me.txtSMSCOunt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.pnlEmergencyReason)
        Me.Panel1.Controls.Add(Me.lnkRequestNumberManovr)
        Me.Panel1.Controls.Add(Me.lblRequestNumberManovr)
        Me.Panel1.Controls.Add(Me.txtEmergencyReasonDisplay)
        Me.Panel1.Controls.Add(Me.lnkRequestNumber)
        Me.Panel1.Controls.Add(Me.txtTamirRequestNumber)
        Me.Panel1.Controls.Add(Me.lblTamirRequestNo)
        Me.Panel1.Controls.Add(Me.lblRequestNumber)
        Me.Panel1.Location = New System.Drawing.Point(36, 14)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(656, 83)
        Me.Panel1.TabIndex = 0
        '
        'pnlEmergencyReason
        '
        Me.pnlEmergencyReason.Controls.Add(Me.lblEmergencyReason)
        Me.pnlEmergencyReason.Controls.Add(Me.txtEmergency)
        Me.pnlEmergencyReason.Location = New System.Drawing.Point(8, 8)
        Me.pnlEmergencyReason.Name = "pnlEmergencyReason"
        Me.pnlEmergencyReason.Size = New System.Drawing.Size(264, 66)
        Me.pnlEmergencyReason.TabIndex = 5
        Me.pnlEmergencyReason.Visible = False
        '
        'lblEmergencyReason
        '
        Me.lblEmergencyReason.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblEmergencyReason.AutoSize = True
        Me.lblEmergencyReason.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblEmergencyReason.Location = New System.Drawing.Point(146, 0)
        Me.lblEmergencyReason.Name = "lblEmergencyReason"
        Me.lblEmergencyReason.Size = New System.Drawing.Size(115, 13)
        Me.lblEmergencyReason.TabIndex = 0
        Me.lblEmergencyReason.Text = "علت اضطراري بودن :"
        '
        'txtEmergency
        '
        Me.txtEmergency.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtEmergency.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.txtEmergency.ForeColor = System.Drawing.Color.Black
        Me.txtEmergency.Location = New System.Drawing.Point(3, 18)
        Me.txtEmergency.MaxLength = 500
        Me.txtEmergency.Multiline = True
        Me.txtEmergency.Name = "txtEmergency"
        Me.txtEmergency.Size = New System.Drawing.Size(258, 45)
        Me.txtEmergency.TabIndex = 4
        '
        'lnkRequestNumberManovr
        '
        Me.lnkRequestNumberManovr.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lnkRequestNumberManovr.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lnkRequestNumberManovr.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.lnkRequestNumberManovr.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lnkRequestNumberManovr.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.lnkRequestNumberManovr.Location = New System.Drawing.Point(296, 57)
        Me.lnkRequestNumberManovr.Name = "lnkRequestNumberManovr"
        Me.lnkRequestNumberManovr.Size = New System.Drawing.Size(144, 16)
        Me.lnkRequestNumberManovr.TabIndex = 4
        Me.lnkRequestNumberManovr.TabStop = True
        Me.lnkRequestNumberManovr.Text = "مشخص نشده"
        Me.lnkRequestNumberManovr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lnkRequestNumberManovr.Visible = False
        '
        'lblRequestNumberManovr
        '
        Me.lblRequestNumberManovr.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblRequestNumberManovr.AutoSize = True
        Me.lblRequestNumberManovr.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblRequestNumberManovr.Location = New System.Drawing.Point(448, 57)
        Me.lblRequestNumberManovr.Name = "lblRequestNumberManovr"
        Me.lblRequestNumberManovr.Size = New System.Drawing.Size(204, 13)
        Me.lblRequestNumberManovr.TabIndex = 3
        Me.lblRequestNumberManovr.Text = "شماره پرونده خاموشي بازگشت مانور"
        Me.lblRequestNumberManovr.Visible = False
        '
        'txtEmergencyReasonDisplay
        '
        Me.txtEmergencyReasonDisplay.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtEmergencyReasonDisplay.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.txtEmergencyReasonDisplay.ForeColor = System.Drawing.Color.Maroon
        Me.txtEmergencyReasonDisplay.Location = New System.Drawing.Point(8, 8)
        Me.txtEmergencyReasonDisplay.Multiline = True
        Me.txtEmergencyReasonDisplay.Name = "txtEmergencyReasonDisplay"
        Me.txtEmergencyReasonDisplay.ReadOnly = True
        Me.txtEmergencyReasonDisplay.Size = New System.Drawing.Size(264, 66)
        Me.txtEmergencyReasonDisplay.TabIndex = 2
        Me.txtEmergencyReasonDisplay.Text = "علت ثبت بدون تأييد:"
        Me.txtEmergencyReasonDisplay.Visible = False
        '
        'pnlNazer
        '
        Me.pnlNazer.Controls.Add(Me.picNazerSearch)
        Me.pnlNazer.Controls.Add(Me.Label50)
        Me.pnlNazer.Controls.Add(Me.cmbNazer)
        Me.pnlNazer.Controls.Add(Me.btnSaveNazer)
        Me.pnlNazer.Location = New System.Drawing.Point(332, 383)
        Me.pnlNazer.Name = "pnlNazer"
        Me.pnlNazer.Size = New System.Drawing.Size(360, 33)
        Me.pnlNazer.TabIndex = 1
        '
        'picNazerSearch
        '
        Me.picNazerSearch.AccessibleName = ""
        Me.picNazerSearch.Cursor = System.Windows.Forms.Cursors.Hand
        Me.picNazerSearch.Image = CType(resources.GetObject("picNazerSearch.Image"), System.Drawing.Image)
        Me.picNazerSearch.Location = New System.Drawing.Point(146, 8)
        Me.picNazerSearch.Name = "picNazerSearch"
        Me.picNazerSearch.Size = New System.Drawing.Size(16, 16)
        Me.picNazerSearch.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picNazerSearch.TabIndex = 188
        Me.picNazerSearch.TabStop = False
        Me.GlobalToolTip.SetToolTip(Me.picNazerSearch, "جستجوي پيمانکاران")
        '
        'Label50
        '
        Me.Label50.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label50.AutoSize = True
        Me.Label50.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label50.Location = New System.Drawing.Point(312, 8)
        Me.Label50.Name = "Label50"
        Me.Label50.Size = New System.Drawing.Size(30, 13)
        Me.Label50.TabIndex = 0
        Me.Label50.Text = "ناظر"
        '
        'cmbNazer
        '
        Me.cmbNazer.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbNazer.BackColor = System.Drawing.Color.White
        Me.cmbNazer.DataBindings.Add(New System.Windows.Forms.Binding("SelectedValue", Me.DatasetDummy1, "ViewDummyId.Dummy05Id", True))
        Me.cmbNazer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbNazer.IsReadOnly = False
        Me.cmbNazer.Location = New System.Drawing.Point(168, 6)
        Me.cmbNazer.Name = "cmbNazer"
        Me.cmbNazer.Size = New System.Drawing.Size(136, 21)
        Me.cmbNazer.TabIndex = 1
        '
        'btnSaveNazer
        '
        Me.btnSaveNazer.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSaveNazer.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.btnSaveNazer.Image = CType(resources.GetObject("btnSaveNazer.Image"), System.Drawing.Image)
        Me.btnSaveNazer.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSaveNazer.Location = New System.Drawing.Point(24, 4)
        Me.btnSaveNazer.Name = "btnSaveNazer"
        Me.btnSaveNazer.Size = New System.Drawing.Size(120, 24)
        Me.btnSaveNazer.TabIndex = 2
        Me.btnSaveNazer.Text = "ذخيره ناظر"
        Me.btnSaveNazer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'pnlWorkCommandNo
        '
        Me.pnlWorkCommandNo.Controls.Add(Me.lblWorkCommandNo)
        Me.pnlWorkCommandNo.Controls.Add(Me.txtWorkCommandNo)
        Me.pnlWorkCommandNo.Controls.Add(Me.lblGISFormNo)
        Me.pnlWorkCommandNo.Controls.Add(Me.txtGISFormNo)
        Me.pnlWorkCommandNo.Location = New System.Drawing.Point(332, 416)
        Me.pnlWorkCommandNo.Name = "pnlWorkCommandNo"
        Me.pnlWorkCommandNo.Size = New System.Drawing.Size(360, 56)
        Me.pnlWorkCommandNo.TabIndex = 6
        '
        'lblWorkCommandNo
        '
        Me.lblWorkCommandNo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblWorkCommandNo.AutoSize = True
        Me.lblWorkCommandNo.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblWorkCommandNo.Location = New System.Drawing.Point(172, 8)
        Me.lblWorkCommandNo.Name = "lblWorkCommandNo"
        Me.lblWorkCommandNo.Size = New System.Drawing.Size(95, 13)
        Me.lblWorkCommandNo.TabIndex = 0
        Me.lblWorkCommandNo.Text = "شماره دستور کار"
        '
        'txtWorkCommandNo
        '
        Me.txtWorkCommandNo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtWorkCommandNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtWorkCommandNo.CaptinText = ""
        Me.txtWorkCommandNo.HasCaption = False
        Me.txtWorkCommandNo.IsForceText = False
        Me.txtWorkCommandNo.IsFractional = True
        Me.txtWorkCommandNo.IsIP = False
        Me.txtWorkCommandNo.IsNumberOnly = False
        Me.txtWorkCommandNo.IsYear = False
        Me.txtWorkCommandNo.Location = New System.Drawing.Point(24, 6)
        Me.txtWorkCommandNo.Multiline = True
        Me.txtWorkCommandNo.Name = "txtWorkCommandNo"
        Me.txtWorkCommandNo.Size = New System.Drawing.Size(120, 20)
        Me.txtWorkCommandNo.TabIndex = 0
        Me.txtWorkCommandNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblGISFormNo
        '
        Me.lblGISFormNo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblGISFormNo.AutoSize = True
        Me.lblGISFormNo.Font = New System.Drawing.Font("Tahoma", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblGISFormNo.Location = New System.Drawing.Point(172, 35)
        Me.lblGISFormNo.Name = "lblGISFormNo"
        Me.lblGISFormNo.Size = New System.Drawing.Size(168, 11)
        Me.lblGISFormNo.TabIndex = 0
        Me.lblGISFormNo.Text = "شماره فرم درخواست ثبت تغييرات GIS"
        '
        'txtGISFormNo
        '
        Me.txtGISFormNo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtGISFormNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtGISFormNo.CaptinText = ""
        Me.txtGISFormNo.HasCaption = False
        Me.txtGISFormNo.IsForceText = False
        Me.txtGISFormNo.IsFractional = True
        Me.txtGISFormNo.IsIP = False
        Me.txtGISFormNo.IsNumberOnly = False
        Me.txtGISFormNo.IsYear = False
        Me.txtGISFormNo.Location = New System.Drawing.Point(24, 32)
        Me.txtGISFormNo.Multiline = True
        Me.txtGISFormNo.Name = "txtGISFormNo"
        Me.txtGISFormNo.Size = New System.Drawing.Size(120, 20)
        Me.txtGISFormNo.TabIndex = 1
        Me.txtGISFormNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.Label75)
        Me.Panel4.Controls.Add(Me.txtProjectNo)
        Me.Panel4.Location = New System.Drawing.Point(36, 439)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(288, 34)
        Me.Panel4.TabIndex = 7
        '
        'Label75
        '
        Me.Label75.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label75.AutoSize = True
        Me.Label75.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label75.Location = New System.Drawing.Point(195, 12)
        Me.Label75.Name = "Label75"
        Me.Label75.Size = New System.Drawing.Size(71, 13)
        Me.Label75.TabIndex = 0
        Me.Label75.Text = "شماره پروژه"
        '
        'txtProjectNo
        '
        Me.txtProjectNo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtProjectNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtProjectNo.CaptinText = ""
        Me.txtProjectNo.HasCaption = False
        Me.txtProjectNo.IsForceText = False
        Me.txtProjectNo.IsFractional = True
        Me.txtProjectNo.IsIP = False
        Me.txtProjectNo.IsNumberOnly = False
        Me.txtProjectNo.IsYear = False
        Me.txtProjectNo.Location = New System.Drawing.Point(44, 8)
        Me.txtProjectNo.Multiline = True
        Me.txtProjectNo.Name = "txtProjectNo"
        Me.txtProjectNo.Size = New System.Drawing.Size(144, 20)
        Me.txtProjectNo.TabIndex = 0
        Me.txtProjectNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'chkIsConfirmNazer
        '
        Me.chkIsConfirmNazer.Location = New System.Drawing.Point(255, 568)
        Me.chkIsConfirmNazer.Name = "chkIsConfirmNazer"
        Me.chkIsConfirmNazer.Size = New System.Drawing.Size(168, 24)
        Me.chkIsConfirmNazer.TabIndex = 3
        Me.chkIsConfirmNazer.Text = "خاموشي مورد تاييد ناظر است"
        '
        'chkIsSendToSetad
        '
        Me.chkIsSendToSetad.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkIsSendToSetad.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.chkIsSendToSetad.Location = New System.Drawing.Point(424, 568)
        Me.chkIsSendToSetad.Name = "chkIsSendToSetad"
        Me.chkIsSendToSetad.Size = New System.Drawing.Size(176, 24)
        Me.chkIsSendToSetad.TabIndex = 1
        Me.chkIsSendToSetad.Text = "تأييد مدير و ارسال به ستاد"
        Me.ToolTip1.SetToolTip(Me.chkIsSendToSetad, "در صورت علامت زدن اين کادر، اطلاعات به ستاد يا مرکز ارسال شده و ديگر قابل تغيير ن" &
        "خواهند بود.")
        Me.chkIsSendToSetad.Visible = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 40)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(736, 3)
        Me.GroupBox1.TabIndex = 4
        Me.GroupBox1.TabStop = False
        '
        'pnlAreaCity
        '
        Me.pnlAreaCity.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlAreaCity.Controls.Add(Me.cmbArea)
        Me.pnlAreaCity.Controls.Add(Me.Label4)
        Me.pnlAreaCity.Controls.Add(Me.cmbCity)
        Me.pnlAreaCity.Controls.Add(Me.Label5)
        Me.pnlAreaCity.Location = New System.Drawing.Point(328, 2)
        Me.pnlAreaCity.Name = "pnlAreaCity"
        Me.pnlAreaCity.Size = New System.Drawing.Size(416, 37)
        Me.pnlAreaCity.TabIndex = 5
        '
        'chkIsEmergency
        '
        Me.chkIsEmergency.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.chkIsEmergency.Location = New System.Drawing.Point(8, 10)
        Me.chkIsEmergency.Name = "chkIsEmergency"
        Me.chkIsEmergency.Size = New System.Drawing.Size(224, 20)
        Me.chkIsEmergency.TabIndex = 6
        Me.chkIsEmergency.Text = "ثبت خاموشي بابرنامه بدون نياز به تأييد"
        '
        'pnlEmergency
        '
        Me.pnlEmergency.BackColor = System.Drawing.SystemColors.Control
        Me.pnlEmergency.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlEmergency.Controls.Add(Me.Label64)
        Me.pnlEmergency.Controls.Add(Me.txtEmergencyReason)
        Me.pnlEmergency.Controls.Add(Me.lblEmergency)
        Me.pnlEmergency.Controls.Add(Me.btnEmergencyOK)
        Me.pnlEmergency.Controls.Add(Me.Label65)
        Me.pnlEmergency.Controls.Add(Me.btnEmergencyCancel)
        Me.pnlEmergency.Location = New System.Drawing.Point(7, 37)
        Me.pnlEmergency.Name = "pnlEmergency"
        Me.pnlEmergency.Size = New System.Drawing.Size(737, 523)
        Me.pnlEmergency.TabIndex = 7
        Me.pnlEmergency.Visible = False
        '
        'Label64
        '
        Me.Label64.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label64.AutoSize = True
        Me.Label64.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label64.Location = New System.Drawing.Point(478, 184)
        Me.Label64.Name = "Label64"
        Me.Label64.Size = New System.Drawing.Size(242, 13)
        Me.Label64.TabIndex = 6
        Me.Label64.Text = "لطفا علت ثبت اين نوع  خاموشي را ذکر نماييد:"
        '
        'txtEmergencyReason
        '
        Me.txtEmergencyReason.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtEmergencyReason.Location = New System.Drawing.Point(14, 208)
        Me.txtEmergencyReason.MaxLength = 500
        Me.txtEmergencyReason.Multiline = True
        Me.txtEmergencyReason.Name = "txtEmergencyReason"
        Me.txtEmergencyReason.Size = New System.Drawing.Size(705, 256)
        Me.txtEmergencyReason.TabIndex = 5
        '
        'lblEmergency
        '
        Me.lblEmergency.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblEmergency.BackColor = System.Drawing.SystemColors.Control
        Me.lblEmergency.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lblEmergency.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblEmergency.ForeColor = System.Drawing.Color.Maroon
        Me.lblEmergency.Location = New System.Drawing.Point(10, 48)
        Me.lblEmergency.Multiline = True
        Me.lblEmergency.Name = "lblEmergency"
        Me.lblEmergency.ReadOnly = True
        Me.lblEmergency.Size = New System.Drawing.Size(713, 128)
        Me.lblEmergency.TabIndex = 4
        Me.lblEmergency.Text = "شما در حال ثبت يک درخواست خاموشي بابرنامه از نوع عدم نياز به تأييد مي‌باشيد."
        Me.lblEmergency.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnEmergencyOK
        '
        Me.btnEmergencyOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEmergencyOK.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.btnEmergencyOK.Image = CType(resources.GetObject("btnEmergencyOK.Image"), System.Drawing.Image)
        Me.btnEmergencyOK.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnEmergencyOK.Location = New System.Drawing.Point(378, 472)
        Me.btnEmergencyOK.Name = "btnEmergencyOK"
        Me.btnEmergencyOK.Size = New System.Drawing.Size(88, 24)
        Me.btnEmergencyOK.TabIndex = 0
        Me.btnEmergencyOK.Text = "تأييد"
        Me.btnEmergencyOK.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label65
        '
        Me.Label65.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label65.AutoSize = True
        Me.Label65.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label65.ForeColor = System.Drawing.Color.OrangeRed
        Me.Label65.Location = New System.Drawing.Point(239, 14)
        Me.Label65.Name = "Label65"
        Me.Label65.Size = New System.Drawing.Size(262, 17)
        Me.Label65.TabIndex = 6
        Me.Label65.Text = "ثبت خاموشي بابرنامه بدون نياز به تأييد"
        Me.Label65.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnEmergencyCancel
        '
        Me.btnEmergencyCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnEmergencyCancel.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.btnEmergencyCancel.Image = CType(resources.GetObject("btnEmergencyCancel.Image"), System.Drawing.Image)
        Me.btnEmergencyCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnEmergencyCancel.Location = New System.Drawing.Point(266, 472)
        Me.btnEmergencyCancel.Name = "btnEmergencyCancel"
        Me.btnEmergencyCancel.Size = New System.Drawing.Size(88, 24)
        Me.btnEmergencyCancel.TabIndex = 3
        Me.btnEmergencyCancel.Text = "انصراف"
        Me.btnEmergencyCancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmnuFeederKey
        '
        Me.cmnuFeederKey.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.cmnuFeederKeyOnDisconnect, Me.cmnuFeederKeyOnConnect})
        '
        'cmnuFeederKeyOnDisconnect
        '
        Me.cmnuFeederKeyOnDisconnect.Index = 0
        Me.cmnuFeederKeyOnDisconnect.Text = "هنگام قطع"
        '
        'cmnuFeederKeyOnConnect
        '
        Me.cmnuFeederKeyOnConnect.Index = 1
        Me.cmnuFeederKeyOnConnect.Text = "هنگام وصل"
        '
        'pnlMPFeederKey
        '
        Me.pnlMPFeederKey.BackColor = System.Drawing.SystemColors.Control
        Me.pnlMPFeederKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlMPFeederKey.CaptionBackColor = System.Drawing.Color.RoyalBlue
        Me.pnlMPFeederKey.CaptionFont = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.pnlMPFeederKey.CaptionForeColor = System.Drawing.Color.White
        Me.pnlMPFeederKey.CaptionHeight = 20
        Me.pnlMPFeederKey.CaptionText = "فهرست کليدهاي فيدر انتخاب شده"
        Me.pnlMPFeederKey.Controls.Add(Me.dgFeederKey)
        Me.pnlMPFeederKey.Controls.Add(Me.chkIsRemoteChange)
        Me.pnlMPFeederKey.Controls.Add(Me.btnOk)
        Me.pnlMPFeederKey.Controls.Add(Me.btnReturnFeederKey)
        Me.pnlMPFeederKey.Controls.Add(Me.pnlMPFeederKeyFilter)
        Me.pnlMPFeederKey.IsMoveable = True
        Me.pnlMPFeederKey.IsWindowMove = False
        Me.pnlMPFeederKey.Location = New System.Drawing.Point(113, 96)
        Me.pnlMPFeederKey.Name = "pnlMPFeederKey"
        Me.pnlMPFeederKey.Size = New System.Drawing.Size(529, 272)
        Me.pnlMPFeederKey.TabIndex = 217
        Me.pnlMPFeederKey.Visible = False
        '
        'dgFeederKey
        '
        Me.dgFeederKey.BorderStyle = Janus.Windows.GridEX.BorderStyle.Flat
        Me.dgFeederKey.CurrentRowIndex = -1
        Me.dgFeederKey.EnableFormatEvent = True
        Me.dgFeederKey.EnableSaveLayout = True
        Me.dgFeederKey.GroupByBoxFormatStyle.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.dgFeederKey.GroupByBoxInfoFormatStyle.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.dgFeederKey.GroupByBoxVisible = False
        Me.dgFeederKey.IsColor = False
        Me.dgFeederKey.IsColumnContextMenu = True
        Me.dgFeederKey.IsForMonitoring = False
        dgFeederKey_Layout_0.IsCurrentLayout = True
        dgFeederKey_Layout_0.Key = "KeyStates"
        dgFeederKey_Layout_0.LayoutString = resources.GetString("dgFeederKey_Layout_0.LayoutString")
        Me.dgFeederKey.Layouts.AddRange(New Janus.Windows.GridEX.GridEXLayout() {dgFeederKey_Layout_0})
        Me.dgFeederKey.Location = New System.Drawing.Point(8, 60)
        Me.dgFeederKey.Name = "dgFeederKey"
        Me.dgFeederKey.PrintLandScape = True
        Me.dgFeederKey.SaveGridName = ""
        Me.dgFeederKey.Size = New System.Drawing.Size(512, 176)
        Me.dgFeederKey.TabIndex = 13
        '
        'chkIsRemoteChange
        '
        Me.chkIsRemoteChange.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkIsRemoteChange.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkIsRemoteChange.Location = New System.Drawing.Point(291, 243)
        Me.chkIsRemoteChange.Name = "chkIsRemoteChange"
        Me.chkIsRemoteChange.Size = New System.Drawing.Size(136, 23)
        Me.chkIsRemoteChange.TabIndex = 11
        Me.chkIsRemoteChange.Text = "تغيير وضعيت از راه دور"
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnOk.Image = CType(resources.GetObject("btnOk.Image"), System.Drawing.Image)
        Me.btnOk.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnOk.Location = New System.Drawing.Point(433, 243)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(88, 23)
        Me.btnOk.TabIndex = 10
        Me.btnOk.Text = "&تأييد"
        Me.btnOk.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnReturnFeederKey
        '
        Me.btnReturnFeederKey.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnReturnFeederKey.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnReturnFeederKey.Image = CType(resources.GetObject("btnReturnFeederKey.Image"), System.Drawing.Image)
        Me.btnReturnFeederKey.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnReturnFeederKey.Location = New System.Drawing.Point(8, 243)
        Me.btnReturnFeederKey.Name = "btnReturnFeederKey"
        Me.btnReturnFeederKey.Size = New System.Drawing.Size(88, 23)
        Me.btnReturnFeederKey.TabIndex = 8
        Me.btnReturnFeederKey.Text = "&بازگشت"
        Me.btnReturnFeederKey.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'pnlMPFeederKeyFilter
        '
        Me.pnlMPFeederKeyFilter.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlMPFeederKeyFilter.Controls.Add(Me.PictureBox2)
        Me.pnlMPFeederKeyFilter.Controls.Add(Me.PictureBox1)
        Me.pnlMPFeederKeyFilter.Controls.Add(Me.txtMPFeederKeyGIS)
        Me.pnlMPFeederKeyFilter.Controls.Add(Me.txtMPFeederKey)
        Me.pnlMPFeederKeyFilter.Controls.Add(Me.Label118)
        Me.pnlMPFeederKeyFilter.Controls.Add(Me.Label114)
        Me.pnlMPFeederKeyFilter.Controls.Add(Me.cmbMPCloserType)
        Me.pnlMPFeederKeyFilter.Controls.Add(Me.Label115)
        Me.pnlMPFeederKeyFilter.Location = New System.Drawing.Point(8, 29)
        Me.pnlMPFeederKeyFilter.Name = "pnlMPFeederKeyFilter"
        Me.pnlMPFeederKeyFilter.Size = New System.Drawing.Size(512, 24)
        Me.pnlMPFeederKeyFilter.TabIndex = 12
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Font = New System.Drawing.Font("Tahoma", 8.0!)
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(4, 5)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.PictureBox2.Size = New System.Drawing.Size(16, 16)
        Me.PictureBox2.TabIndex = 14
        Me.PictureBox2.TabStop = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.Font = New System.Drawing.Font("Tahoma", 8.0!)
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(178, 4)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.PictureBox1.Size = New System.Drawing.Size(16, 16)
        Me.PictureBox1.TabIndex = 14
        Me.PictureBox1.TabStop = False
        '
        'txtMPFeederKeyGIS
        '
        Me.txtMPFeederKeyGIS.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMPFeederKeyGIS.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMPFeederKeyGIS.Font = New System.Drawing.Font("Tahoma", 8.0!)
        Me.txtMPFeederKeyGIS.Location = New System.Drawing.Point(26, 3)
        Me.txtMPFeederKeyGIS.Name = "txtMPFeederKeyGIS"
        Me.txtMPFeederKeyGIS.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtMPFeederKeyGIS.Size = New System.Drawing.Size(96, 20)
        Me.txtMPFeederKeyGIS.TabIndex = 13
        '
        'txtMPFeederKey
        '
        Me.txtMPFeederKey.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMPFeederKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMPFeederKey.Font = New System.Drawing.Font("Tahoma", 8.0!)
        Me.txtMPFeederKey.Location = New System.Drawing.Point(200, 2)
        Me.txtMPFeederKey.Name = "txtMPFeederKey"
        Me.txtMPFeederKey.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtMPFeederKey.Size = New System.Drawing.Size(96, 20)
        Me.txtMPFeederKey.TabIndex = 13
        '
        'Label118
        '
        Me.Label118.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label118.AutoSize = True
        Me.Label118.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label118.Location = New System.Drawing.Point(126, 3)
        Me.Label118.Name = "Label118"
        Me.Label118.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Label118.Size = New System.Drawing.Size(39, 18)
        Me.Label118.TabIndex = 12
        Me.Label118.Text = "کد GIS"
        '
        'Label114
        '
        Me.Label114.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label114.AutoSize = True
        Me.Label114.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label114.Location = New System.Drawing.Point(300, 2)
        Me.Label114.Name = "Label114"
        Me.Label114.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Label114.Size = New System.Drawing.Size(40, 18)
        Me.Label114.TabIndex = 12
        Me.Label114.Text = "نام کليد"
        '
        'cmbMPCloserType
        '
        Me.cmbMPCloserType.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbMPCloserType.BackColor = System.Drawing.Color.White
        Me.cmbMPCloserType.DisplayMember = "MPCloserType"
        Me.cmbMPCloserType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMPCloserType.IsReadOnly = False
        Me.cmbMPCloserType.Location = New System.Drawing.Point(344, 2)
        Me.cmbMPCloserType.Name = "cmbMPCloserType"
        Me.cmbMPCloserType.Size = New System.Drawing.Size(120, 21)
        Me.cmbMPCloserType.TabIndex = 7
        Me.cmbMPCloserType.ValueMember = "MPCloserTypeId"
        '
        'Label115
        '
        Me.Label115.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label115.AutoSize = True
        Me.Label115.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label115.Location = New System.Drawing.Point(464, 2)
        Me.Label115.Name = "Label115"
        Me.Label115.Size = New System.Drawing.Size(42, 18)
        Me.Label115.TabIndex = 0
        Me.Label115.Text = "نوع کليد"
        '
        'errpro
        '
        Me.errpro.ContainerControl = Me
        Me.errpro.RightToLeft = True
        '
        'CommonDataSet1
        '
        Me.CommonDataSet1.DataSetName = "CommonDataSet"
        Me.CommonDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'ftDocs
        '
        Me.ftDocs.AreaId = -1
        Me.ftDocs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ftDocs.FileId = CType(0, Long)
        Me.ftDocs.Location = New System.Drawing.Point(150, 404)
        Me.ftDocs.Name = "ftDocs"
        Me.ftDocs.Size = New System.Drawing.Size(450, 146)
        Me.ftDocs.TabIndex = 218
        Me.ftDocs.Visible = False
        '
        'frmNewTamirRequest
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(754, 600)
        Me.Controls.Add(Me.chkIsEmergency)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btnReturn)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.pnlAreaCity)
        Me.Controls.Add(Me.chkIsSendToSetad)
        Me.Controls.Add(Me.tbcMain)
        Me.Controls.Add(Me.pnlEmergency)
        Me.Controls.Add(Me.chkIsConfirmNazer)
        Me.Controls.Add(Me.pnlMPFeederKey)
        Me.Controls.Add(Me.ftDocs)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.HelpMaker.SetHelpNavigator(Me, System.Windows.Forms.HelpNavigator.Topic)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmNewTamirRequest"
        Me.HelpMaker.SetShowHelp(Me, True)
        Me.Text = "درخواست اجازه کار خاموشي با برنامه"
        Me.pnlNumbers.ResumeLayout(False)
        Me.pnlNumbers.PerformLayout()
        CType(Me.DatasetBT1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlPostFeeder.ResumeLayout(False)
        Me.pnlPostFeeder.PerformLayout()
        CType(Me.picSearchMPPOverlaps, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DatasetDummy1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlLPPostFeederPart.ResumeLayout(False)
        CType(Me.picSearchMPFOverlaps, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlSOverlaps.ResumeLayout(False)
        CType(Me.dgSOverlaps, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlTamirRequestInfo.ResumeLayout(False)
        Me.pnlTamirRequestInfo.PerformLayout()
        Me.pnlOverlaps.ResumeLayout(False)
        CType(Me.dgOverlaps, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlConfirmInfo.ResumeLayout(False)
        Me.pnlConfirmInfo.PerformLayout()
        Me.pnlConfirm.ResumeLayout(False)
        Me.pnlConfirm.PerformLayout()
        CType(Me.PicSendSMSSensitive, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picLock, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlAllow.ResumeLayout(False)
        Me.pnlAllow.PerformLayout()
        CType(Me.picIsAuotoNumber, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tbcMain.ResumeLayout(False)
        Me.tp6_Docs.ResumeLayout(False)
        CType(Me.dgFiles, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DatasetTamir1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpNewFile.ResumeLayout(False)
        Me.grpNewFile.PerformLayout()
        Me.tp5_nDCFeeder.ResumeLayout(False)
        Me.pnlDate.ResumeLayout(False)
        Me.pnlDate.PerformLayout()
        Me.pnlToDate.ResumeLayout(False)
        Me.pnlToDate.PerformLayout()
        Me.pnlNetworkType.ResumeLayout(False)
        Me.pnlDCDayCount.ResumeLayout(False)
        Me.pnlDCDayCount.PerformLayout()
        Me.pnlLastDCCount.ResumeLayout(False)
        Me.pnlLastDCCount.PerformLayout()
        CType(Me.dgFeederDC, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.mDsReqView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tp4_Confirms.ResumeLayout(False)
        Me.pnlWarmLineConfirm.ResumeLayout(False)
        Me.pnlWarmLineConfirm.PerformLayout()
        Me.tp3_OperationList.ResumeLayout(False)
        Me.tp3_OperationList.PerformLayout()
        CType(Me.dgOperations, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DatasetTamirView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tp2_DCLocation.ResumeLayout(False)
        Me.tp2_DCLocation.PerformLayout()
        CType(Me.picFeederPart, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlPeymankar.ResumeLayout(False)
        Me.pnlPeymankar.PerformLayout()
        Me.pnlManoeuvreRequestInfo.ResumeLayout(False)
        Me.pnlManoeuvreRequestInfo.PerformLayout()
        Me.tbcDCs.ResumeLayout(False)
        Me.pnlManoeuvre.ResumeLayout(False)
        Me.pnlManoeuvre.PerformLayout()
        Me.pnlManoeuvrePostFeeder.ResumeLayout(False)
        Me.pnlManoeuvrePostFeeder.PerformLayout()
        Me.pnlAdsress.ResumeLayout(False)
        Me.pnlAdsress.PerformLayout()
        Me.pnlInfo.ResumeLayout(False)
        Me.pnlInfo.PerformLayout()
        Me.pnlRequestedBy.ResumeLayout(False)
        Me.pnlRequestedBy.PerformLayout()
        CType(Me.picPeymankarSearch, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlManoeuvreDesc.ResumeLayout(False)
        Me.pnlManoeuvreDesc.PerformLayout()
        CType(Me.picOKManoeuvreDesc, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlLetterInfo.ResumeLayout(False)
        Me.pnlLetterInfo.PerformLayout()
        CType(Me.btnHidePnlLetter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlMultiStepInfo.ResumeLayout(False)
        Me.pnlMultiStepInfo.PerformLayout()
        Me.pnlLevel6.ResumeLayout(False)
        Me.pnlLevel6.PerformLayout()
        Me.pnlLevel5.ResumeLayout(False)
        Me.pnlLevel5.PerformLayout()
        Me.pnlLevel4.ResumeLayout(False)
        Me.pnlLevel4.PerformLayout()
        Me.pnlLevel3.ResumeLayout(False)
        Me.pnlLevel3.PerformLayout()
        Me.pnlLevel2.ResumeLayout(False)
        Me.pnlLevel2.PerformLayout()
        Me.pnlLevel1.ResumeLayout(False)
        Me.pnlLevel1.PerformLayout()
        Me.tp1_Specs.ResumeLayout(False)
        Me.tp1_Specs.PerformLayout()
        Me.pnlTamirRequestSubject.ResumeLayout(False)
        Me.pnlTamirRequestSubject.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.pnlTamirType.ResumeLayout(False)
        Me.pnlTamirType.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.pnlEmergencyReason.ResumeLayout(False)
        Me.pnlEmergencyReason.PerformLayout()
        Me.pnlNazer.ResumeLayout(False)
        Me.pnlNazer.PerformLayout()
        CType(Me.picNazerSearch, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlWorkCommandNo.ResumeLayout(False)
        Me.pnlWorkCommandNo.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.pnlAreaCity.ResumeLayout(False)
        Me.pnlAreaCity.PerformLayout()
        Me.pnlEmergency.ResumeLayout(False)
        Me.pnlEmergency.PerformLayout()
        Me.pnlMPFeederKey.ResumeLayout(False)
        CType(Me.dgFeederKey, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlMPFeederKeyFilter.ResumeLayout(False)
        Me.pnlMPFeederKeyFilter.PerformLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.errpro, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CommonDataSet1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Events"
    Private Sub frmNewTamirRequest_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        mIsServer121 = CConfig.ReadConfig("IsTamirServer121", False)
        mIsTamirDirectToSetad = CConfig.ReadConfig("IsTamirDirectToSetad", False)
        mIsMonthly = CConfig.ReadConfig("IsMaxWeekPowerMonthly", False)
        mIsForceTamirOperationTime = CConfig.ReadConfig("IsForceTamirOperationTime", True)
        mIsSendGISSubscriberDCInfo = CConfig.ReadConfig("IsSendGISSubscriberDCInfo", False)
        mIsForceEmergencyReason = CConfig.ReadConfig("IsForceEmergencyReason", True)

        mIsConfirmEndWarmLine = CheckUserTrustee("IsConfirmEndWarmLine", 30, ApplicationTypes.TamirRequest)

        tbcMain.SelectedTab = tp1_Specs
        tbcDCs.SelectedTab = tpDCMain

        Dim lNow As CTimeInfo = GetServerTimeInfo()

        Dim lIsAccess As Boolean
        lIsAccess = CheckUserTrustee("ShowTamirDisconnectInfo", 30, ApplicationTypes.TamirRequest) Or IsAdmin()
        lnkRequestNumber.Enabled = lIsAccess

        'Dim lIsAccess As Boolean = CheckUserTrustee("CanSaveEmergency", 30, ApplicationTypes.TamirRequest)
        lIsAccess = CheckUserTrustee("CanRequestWithoutConfirm", 30, ApplicationTypes.TamirRequest)
        chkIsEmergency.Enabled = lIsAccess

        Init()
        LoadInfo()

        If mIsTamirPowerMonthly Then
            ComputeUsedPowerMonthly()
        Else
            ComputeUsedPower()
        End If

        SetControlsStates()

        Try
            Dim lCount As Integer = 0
            lCount = GetRegistryAbd("TamirFeederDCCount", 5)
            txtLastDCCount.Text = lCount
            lCount = GetRegistryAbd("TamirFeederDCDayCount", 30)
            txtFeederDCDayCount.Text = lCount
            Dim lValue As String = GetRegistryAbd("TamirFeederPeriod", "ndays")
            cmbPeriod.SelectedValue = lValue
            lValue = GetRegistryAbd("TamirFeederNT", "tamir")
            If lValue = "tamir" Then
                rbNT_Tamir.Checked = True
            ElseIf lValue = "nottamir" Then
                rbNT_NotTamir.Checked = True
            Else
                rbNT_Both.Checked = True
            End If

            If cmbPeriod.SelectedValue = "manual" Then
                Dim lDT As DateTime = lNow.MiladiDate
                Dim lDiff As Integer
                txtDateTo.MiladiDT = lDT
                lDiff = Val(Microsoft.VisualBasic.Right(lNow.ShamsiDate, 2))
                If (lDiff > 0) Then lDiff -= 1
                txtDateFrom.MiladiDT = lDT.AddDays(-lDiff)
            End If

            mIsLoading = True
            If cmbDepartment.Enabled Then
                mDsBT.Tbl_Department.DefaultView.RowFilter = "IsActive = 1"
            End If
            mIsLoading = False

            '-------------- 
            SetForceControl(Label76)
            SetForceControl(lblDepartment)
            SetForceControl(Label73)
            SetForceControl(Label4)
            SetForceControl(Label5)
            SetForceControl(Label11)
            SetForceControl(Label13)
            SetForceControl(Label8)
            SetForceControl(Label74)
            SetForceControl(Label71)
            SetForceControl(Label23)

            SetForceControl(Label9)
            Dim lIsForceTamirMPCloserType As Boolean = CConfig.ReadConfig("IsForceTamirMPCloserType", False)
            If lIsForceTamirMPCloserType Then
                SetForceControl(Label25)
            End If
            Dim lIsForceWorkingAddress As Boolean = CConfig.ReadConfig("IsForceWorkingAddress", False)
            If lIsForceWorkingAddress Then
                SetForceControl(Label47)
            End If

            If CConfig.ReadConfig("IsForceGISFormNo", False) AndAlso (chkIsSendToSetad.Checked Or IsSetadMode Or (mIsForConfirm And Not mIsForWarmLineConfirm)) Then
                SetForceControl(lblWorkCommandNo)
                SetForceControl(lblGISFormNo)
            End If
            '-------------- 
        Catch ex As Exception
            WriteError(ex.ToString)
        End Try

        Dim lTamirRequestMonthDay As Integer = Val(CConfig.ReadConfig("TamirRequestMonthDay", 0))
        If lTamirRequestMonthDay > 0 Then cmbTamirType.Enabled = False

        If mIsFogheToziUser Then
            cmbTamirType.SelectedValue = TamirTypes.BarnamehRiziShodeh
            cmbTamirType.Enabled = False
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.ver_940510) Then
            rbIsReturned.Enabled = False
            If rbIsReturned.Visible Then
                picLock.Visible = True
                picLock.BringToFront()
            End If
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.ver_950803) Then
            Dim lAutoSendSMSSensitive As Boolean
            lAutoSendSMSSensitive = CConfig.ReadConfig("AutoSendSMSSensitive", False)
            If Not lAutoSendSMSSensitive Then
                chkIsSendSMSSensitive.Enabled = False
                PicSendSMSSensitive.Visible = True
                PicSendSMSSensitive.BringToFront()
            End If
        End If

        If chkIsWarmLine.Checked Then
            chkIsSendSMSSensitive.Visible = False
            chkIsSendToInformApp.Visible = False
        ElseIf mIsSendToInformApp Then
            chkIsSendToInformApp.Visible = True
        End If

        Try
            If Not mIsForceTamirOperationTime Then
                dgOperations.RootTable.Columns("Duration").Visible = False
                dgOperations.RootTable.Columns("TotalDuration").Visible = False
            End If
        Catch ex As Exception
            ShowError(ex)
        End Try

        Dim lIsForceTamirManovrDC As Boolean = CConfig.ReadConfig("IsForceTamirManovrDC", True)
        If Not lIsForceTamirManovrDC Then
            chkIsNotNeedManovrDC.Show()
        End If
        If mTamirRequestId = -1 Then
            cmbTamirType.SelectedValue = CConfig.ReadConfig("TamirType", TamirTypes.BarnamehRiziShodeh)
        End If

    End Sub
    Private Sub frmNewTamirRequest_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        If Not mIsFirstActivated Then Exit Sub
        mIsFirstActivated = False

        If mTamirRequestId = -1 Then

            If IsCenter Then
                cmbArea.SelectedIndex = -1
                cmbArea.SelectedIndex = -1
                cmbCity.SelectedIndex = -1
                cmbCity.SelectedIndex = -1
            End If

            cmbWeather.SelectedIndex = -1
            cmbWeather.SelectedIndex = -1
            cmbMPPost.SelectedIndex = -1
            cmbMPPost.SelectedIndex = -1
            cmbMPFeeder.SelectedIndex = -1
            cmbMPFeeder.SelectedIndex = -1
            cmbCloserType.SelectedIndex = -1
            cmbCloserType.SelectedIndex = -1
            cmbManoeuvreType.SelectedIndex = -1
            cmbManoeuvreType.SelectedIndex = -1
            cmbNazer.SelectedIndex = -1
            cmbNazer.SelectedIndex = -1
            cmbPeymankar.SelectedIndex = -1
            cmbPeymankar.SelectedIndex = -1
            cmbReferTo.SelectedIndex = 1
            cmbReferTo.SelectedIndex = 1
            cmbDepartment.SelectedIndex = -1
            cmbDepartment.SelectedIndex = -1

            If Not IsSetadMode Then
                cmbTamirNetworkType.SelectedIndex = -1
                cmbTamirNetworkType.SelectedIndex = -1
                'cmbTamirType.SelectedIndex = -1
                'cmbTamirType.SelectedIndex = -1
            End If

        End If

        LaodUserCombos()
    End Sub
    Private Sub chkIsManoeuvre_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIsManoeuvre.CheckedChanged
        pnlManoeuvrePostFeeder.Enabled = chkIsManoeuvre.Checked
        If chkIsManoeuvre.Checked Then
            tbcDCs.BringToFront()
            tbcDCs.Show()
        Else
            tbcDCs.Hide()
            tbcDCs.SendToBack()
            tbcDCs.SelectedTab = tpDCMain
        End If

        chkIsNotNeedManovrDC_CheckedChanged(sender, e)
    End Sub
    Private Sub btnReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReturn.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If Not mIsUploadFilesFinished Then
            ShowError("هنوز عمليات ارسال فايل‌ها روي فايل سرور به پايان نرسيده است.")
            Exit Sub
        End If

        Dim lIsAccess As Boolean
        If IsAdministrator() And Not IsDebugMode() Then
            ShowError("کاربر admin مجاز به ثبت درخواست خاموشي بابرنامه، تأييد يا صدور مجوز آن نميباشد.", False, MsgBoxIcon.MsgIcon_Hand)
            Exit Sub
        End If
        If mIsForAllow Then
            lIsAccess = CheckUserTrustee("EjazehKar", 30, ApplicationTypes.TamirRequest)
            If Not lIsAccess Then
                ShowError("کاربر گرامي، شما مجاز به ثبت اجازه کار در سامانه نمي‌باشيد.", False, MsgBoxIcon.MsgIcon_Hand)
                Exit Sub
            End If
        End If

        If Not mIsForWarmLineConfirm And Not mIsForConfirm Then
            If cmbTamirType.SelectedValue = TamirTypes.BaMovafeghat Then
                lIsAccess = CheckUserTrustee("CanSaveByAgree", 30, ApplicationTypes.TamirRequest)
                If Not lIsAccess Then
                    ShowError("کاربر گرامي، شما مجاز به ثبت درخواست خاموشي باموافقت در سامانه نمي‌باشيد.", False, MsgBoxIcon.MsgIcon_Hand)
                    Exit Sub
                End If
            End If
        End If
        If chkIsWarmLine.Checked = False And rbIsConfirm.Checked = True AndAlso CheckWarmLineFeeder() Then
            ShowError("کاربر گرامي، هم اکنون يک درخواست خاموشي خط گرم بروي فيدر انتخاب شده تأييد شده است .", False, MsgBoxIcon.MsgIcon_Exclamation)
        End If
        If SaveInfo() Then
            mIsCloseFormAfterFileOperation = True
            mIsUploadAfterFileDelete = True
            If DeleteFiles() Then
                Me.DialogResult = DialogResult.OK
                Me.Close()
            End If
        End If
    End Sub
    Private Sub dgOperations_CurrentCellChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) 'Handles dgOperations.CurrentCellChanged
        'ComputeSumTime()
    End Sub
    Private Sub txtDateDisconnect_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
            txtDateDisconnect.TextChanged, txtDateConnect.TextChanged, txtTimeDisconnect.TextChanged, txtTimeConnect.TextChanged
        ComputeDCInterval()
        If mIsTamirPowerMonthly Then
            ComputeUsedPowerMonthly()
        Else
            ComputeUsedPower()
        End If
    End Sub
    Private Sub cmbArea_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbArea.SelectedIndexChanged, cmbArea.SelectedIndexKeyboardChanged
        If mIsLoading Then Exit Sub
        Dim lAreaId As Integer = -2
        If cmbArea.SelectedIndex > -1 Then
            lAreaId = cmbArea.SelectedValue
        ElseIf IsSetadMode Then
            'cmbCity.SelectedIndex = -1
            'cmbCity.SelectedIndex = -1
            'Exit Sub
        End If

        'mDsBT.Tbl_MPPost.DefaultView.RowFilter = "AreaId = " & lAreaId
        mDsBT.Tbl_FeederPart.Clear()
        mDsBT.Tbl_LPPost.Clear()
        mDsBT.Tbl_MPFeeder.Clear()
        mDsBT.Tbl_MPPost.Clear()
        BindingTable("exec spGetMPPosts_v2 " & lAreaId & ",0,'',''", mCnn, mDsBT, "Tbl_MPPost")
        If lAreaId > -1 Then
            cmbCity.SelectedValue = cmbArea.Items(cmbArea.SelectedIndex)("CityId")
            SearchPeymankar()
        End If
        cmbMPPost.SelectedIndex = -1
        cmbMPPost.SelectedIndex = -1
        cmbPeymankar.SelectedIndex = -1
        cmbPeymankar.SelectedIndex = -1

        If mIsTamirPowerMonthly Then
            ComputeUsedPowerMonthly()
        Else
            ComputeUsedPower()
        End If
    End Sub
    Private Sub cmbMPPost_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMPPost.SelectedIndexChanged
        If mIsLoading Then Exit Sub
        LoadPowerInfo(mIsUpdateCurrentValue)
        mIsLoading = True
        Dim lMPPostId As Integer = -1
        Dim lAreaId As Integer = -1

        If cmbArea.SelectedIndex > -1 Then
            lAreaId = cmbArea.SelectedValue
        End If
        If cmbMPPost.SelectedIndex > -1 Then
            lMPPostId = cmbMPPost.SelectedValue
        End If

        'mDsBT.Tbl_MPFeeder.DefaultView.RowFilter = "MPPostId = " & lMPPostId
        mDsBT.Tbl_MPFeeder.Clear()

        ckcmbMPFeeder.Clear()
        If lMPPostId > -1 Then
            BindingTable("exec spGetMPFeeders_v2 " & lAreaId & "," & lMPPostId & ",0", mCnn, mDsBT, "Tbl_MPFeeder")
            ' BindingTable("select * from Tbl_MPFeeder where MPPostId = " & lMPPostId & " AND IsActive = 1 ", mCnn, mDs, "Tbl_MPFeederAll", aIsClearTable:=True)
            Dim lDs As New DataSet
            Dim lSql As String = " SELECT MPPostTransId, MPPostTrans FROM Tbl_MPPostTrans WHERE MPPostId = " & lMPPostId & " UNION SELECT CAST(-1 AS int) AS MPPostTransId, 'فيدرهاي بدون ترانس' AS MPPostTrans "
            BindingTable(lSql, mCnn, lDs, "Tbl_MPPostTrans", , , , , , , True)

            chkMPPostTrans.Fill(lDs.Tables("Tbl_MPPostTrans"), "MPPostTrans", "MPPostTransId", 20)
            '  ckcmbMPFeeder.Fill(mDs.Tables("Tbl_MPFeederAll"), "MPFeederName", "MPFeederId", 20)
        End If

        cmbMPFeeder.SelectedIndex = -1
        cmbMPFeeder.SelectedIndex = -1
        ComputeLPPostCountDC()
        mIsLoading = False
    End Sub

    Private Sub cmbMPFeeder_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMPFeeder.SelectedIndexChanged
        If mIsLoading Then Exit Sub
        LoadPowerInfo(mIsUpdateCurrentValue)
        mIsLoading = True
        'LoadMPFeederDisconnects()

        If cmbMPFeeder.SelectedIndex > -1 Then
            txtCurrentValue.Enabled = True
            txtCurrentValueManovr.Enabled = True
            LoadFeederParts()
            LoadLPPosts()
            If Not mIsLoadInfo Then LoadSensitiveSubscribers()
        Else
            txtCurrentValue.Enabled = False
            txtCurrentValueManovr.Enabled = False
            txtCurrentValue.Text = ""
            txtCurrentValueManovr.Text = ""
            If cmbTamirNetworkType.SelectedValue <> TamirNetworkType.FT AndAlso Not mIsLoadInfo Then
                If Not mIsManualChangeCL Then
                    txtCriticalLocations.Text = ""
                End If
            End If
        End If

        ComputeLPPostCountDC()
        mIsLoading = False
    End Sub
    Private Sub ckcmbMPFeeder_AfterCheck(sender As Object, e As chkComboEventArgs) Handles ckcmbMPFeeder.AfterCheck
        If mIsLoading Then Exit Sub
        LoadPowerInfo(mIsUpdateCurrentValue)
        mIsLoading = True
        If ckcmbMPFeeder.GetDataList.Length > 0 Then
            txtCurrentValue.Enabled = True
            txtCurrentValueManovr.Enabled = True
            If Not mIsLoadInfo Then LoadSensitiveSubscribers()
        Else
            txtCurrentValue.Enabled = False
            txtCurrentValueManovr.Enabled = False
            txtCurrentValue.Text = ""
            txtCurrentValueManovr.Text = ""
            If cmbTamirNetworkType.SelectedValue <> TamirNetworkType.FT AndAlso Not mIsLoadInfo Then
                If Not mIsManualChangeCL Then
                    txtCriticalLocations.Text = ""
                End If
            End If
        End If
        ComputeLPPostCountDC()
        mIsLoading = False
    End Sub
    Private Sub cmbLPPost_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbLPPost.SelectedIndexChanged, cmbFeederPart.SelectedIndexChanged
        LoadPowerInfo(mIsUpdateCurrentValue)
        ComputeLPPostCountDC()
        LoadLPFeeders()
        If mIsLoadInfo Then Exit Sub
        Dim lDs As New DataSet
        If cmbLPPost.SelectedIndex > -1 Then
            BindingTable("SELECT LEFT(ISNULL(address,''),300) AS address  FROM Tbl_LPPost WHERE LPPostId = " & cmbLPPost.SelectedValue, mCnn, lDs, "Tbl_LPPost", aIsClearTable:=True)
            txtWorkingAddress.Text = lDs.Tables("Tbl_LPPost").Rows(0).Item("Address")
        Else
            txtWorkingAddress.Text = ""
        End If
    End Sub
    Private Sub cmbLPFeeder_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbLPFeeder.SelectedIndexChanged
        If mIsLoading Or cmbTamirNetworkType.SelectedValue <> TamirNetworkType.LP Then Exit Sub
        LoadPowerInfo(mIsUpdateCurrentValue)
        mIsLoading = True

        If cmbLPFeeder.SelectedIndex > -1 Or cmbLPPost.SelectedIndex > -1 Then
            txtCurrentValue.Enabled = True
            txtCurrentValueManovr.Enabled = True
        Else
            txtCurrentValue.Enabled = False
            txtCurrentValueManovr.Enabled = False
            txtCurrentValue.Text = ""
            txtCurrentValueManovr.Text = ""
        End If

        mIsLoading = False
    End Sub
    Private Sub cmbMPPost_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMPPost.SelectionChangeCommitted
        mIsUpdateCurrentValue = True
    End Sub
    Private Sub cmbMPFeeder_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMPFeeder.SelectionChangeCommitted
        mIsUpdateCurrentValue = True
    End Sub
    Private Sub cmbLPPost_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbLPPost.SelectionChangeCommitted
        mIsUpdateCurrentValue = True
    End Sub
    Private Sub cmbLPFeeder_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbLPFeeder.SelectionChangeCommitted
        mIsUpdateCurrentValue = True
    End Sub
    Private Sub cmbManoeuvreType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbManoeuvreType.SelectedIndexChanged
        If Not chkIsManoeuvre.Checked Then
            cmbManoeuvreType.SelectedIndex = -1
            cmbManoeuvreType.SelectedIndex = -1
        End If
    End Sub
    Private Sub tbcMain_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbcMain.SelectedIndexChanged
        mIsTabChanging = True
        Try

            If Not chkIsManoeuvre.Checked Then
                cmbManoeuvreType.SelectedIndex = -1
                cmbManoeuvreType.SelectedIndex = -1
            End If
            LaodUserCombos()
            LoadOperationList()

            Try
                If Not mEditingRow Is Nothing Then
                    If cmbPeymankar.SelectedIndex = -1 Then
                        cmbPeymankar.Text = mPeymankarText
                    End If
                End If
            Catch ex As Exception
            End Try

        Catch ex As Exception
        Finally
            mIsTabChanging = False
        End Try
    End Sub
    Private Sub rbConfirm_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbIsConfirm.CheckedChanged, rbIsNotConfirm.CheckedChanged, rbIsReturned.CheckedChanged
        txtUnConfirmReason.Enabled = rbIsNotConfirm.Checked Or rbIsReturned.Checked
        txtReturn.Visible = rbIsReturned.Checked
        LabelRetrun.Visible = rbIsReturned.Checked
        If rbIsReturned.Checked Then
            lblNotConfirmReason.Text = "علت عودت"
        Else
            lblNotConfirmReason.Text = "علت عدم تأييد"
        End If
        If rbIsConfirm.Checked Then
            chkIsSendSMSSensitive.Enabled = Not PicSendSMSSensitive.Visible
            If chkIsSendSMSSensitive.Enabled AndAlso (mIsForConfirm And Not mIsFogheToziUser) Then
                Dim lIsCheckSendSMSSensitive As Boolean = CConfig.ReadConfig("IsCheckSendSMSSensitive", False)
                chkIsSendSMSSensitive.Checked = lIsCheckSendSMSSensitive
            End If

            chkIsSendToInformApp.Enabled = True
        Else
            chkIsSendSMSSensitive.Enabled = False
            chkIsSendSMSSensitive.Checked = False

            chkIsSendToInformApp.Enabled = False
            chkIsSendToInformApp.Checked = False
        End If
    End Sub
    Private Sub lnkRequestNumber_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkRequestNumber.Click, lnkRequestNumberManovr.Click
        Dim lnk As LinkLabel = sender
        Try
            If Not lnk.Tag Is Nothing Then
                ShowDisconnect(lnk.Tag)
            End If
        Catch ex As Exception
            ShowError("پرونده خاموشي پيدا نشد", False, MsgBoxIcon.MsgIcon_Exclamation)
        End Try
    End Sub
    Private Sub dgOperations_CellValueChanged(ByVal sender As Object, ByVal e As Janus.Windows.GridEX.ColumnActionEventArgs) Handles dgOperations.CellValueChanged
        ComputeSumTime()
    End Sub
    Private Sub dgOperations_CellEdited(ByVal sender As Object, ByVal e As Janus.Windows.GridEX.ColumnActionEventArgs) Handles dgOperations.CellEdited
        If e.Column.Key <> "IsSelected" Then Exit Sub
        Dim lDg As JGrid = CType(sender, JGrid)
        Dim lIsSelect As Boolean = lDg(lDg.CurrentRowIndex, e.Column.Key)
        Dim lCount As Object = lDg(lDg.CurrentRowIndex, "OperationCount")

        Try
            lDg.GetRow(lDg.CurrentRowIndex).DataRow("IsSelected") = lIsSelect
        Catch ex As Exception
            Try
                lDg.GetRow(lDg.CurrentRowIndex).RowStyle.BackColor = Color.Red
                lDg.GetRow(lDg.CurrentRowIndex).RowStyle.ForeColor = Color.White
            Catch ex1 As Exception
            End Try
        End Try

        If lIsSelect Then
            If IsDBNull(lCount) Then
                lDg.SetValue("OperationCount", 1)
            End If
        End If

        ComputeSumTime()
    End Sub
    Private Sub txtCurrentValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCurrentValue.TextChanged
        If txtCurrentValue.Text.Trim.Length = 0 Then
            mIsUpdateCurrentValue = True
        End If
        LoadPowerInfo()
    End Sub
    Private Sub txtCurrentValue_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCurrentValue.KeyPress
        mIsUpdateCurrentValue = False
    End Sub
    Private Sub txtDCInterval_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDCInterval.TextChanged
        LoadPowerInfo()
    End Sub
    Private Sub chkIsWarmLine_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkIsWarmLine.CheckedChanged
        If txtCurrentValue.Text.Length = 0 Then txtCurrentValue.Text = "0"
        LoadPowerInfo()
        If chkIsWarmLine.Checked Then
            Label11.Text = "تاريخ و ساعت شروع عمليات"
            Label13.Text = "تاريخ و ساعت پايان عمليات"
            Label32.Text = "مدت عمليات"
            Label66.Text = "( زمان خروج ريکلوزر )"
            Label67.Text = "( زمان ورود ريکلوزر )"
            mIsWarmLine = True
            Label59.Visible = False
            txtLPPostCountDC.Visible = False
        Else
            Label11.Text = "تاريخ و ساعت قطع درخواستي"
            Label13.Text = "تاريخ و ساعت وصل درخواستي"
            Label32.Text = "مدت قطع"
            Label66.Text = "( زمان تحويل به پيمانکار )"
            Label67.Text = "( زمان تحويل از پيمانکار )"
            mIsWarmLine = False
            Label59.Visible = True
            txtLPPostCountDC.Visible = True
        End If

        If mIsWarmLine And Not mIsForceCriticalsAddress Then
            txtAsdresses.Enabled = False
            txtAsdresses.Text = ""
            SetForceControl(Label47)
        Else
            txtAsdresses.Enabled = True
            RemoveForceControl(Label47)
        End If
    End Sub
    Private Sub btnLoadFeederDC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadFeederDC.Click
        LoadMPFeederDisconnects()
    End Sub
    Private Sub btnSaveNazer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveNazer.Click
        If SaveNazer() Then
            ShowError("ناظر با موفقيت ذخيره شد", False, MsgBoxIcon.MsgIcon_Information)
        End If
    End Sub
    Private Sub cmbPeriod_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbPeriod.SelectedIndexChanged
        SetDateBoxes()
    End Sub
    Private Sub txtFeederDCDayCount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFeederDCDayCount.TextChanged
        SetDateBoxes()
    End Sub
    Private Sub chkIsEmergency_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkIsEmergency.CheckedChanged
        If mIsLoading Then Exit Sub
        CheckEmergency()
    End Sub
    Private Sub chkIsLPPostFeederPart_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIsLPPostFeederPart.CheckedChanged
        pnlLPPostFeederPart.Enabled = chkIsLPPostFeederPart.Checked
        RemoveForceControl(lblFeederPart)
        RemoveForceControl(lblLPPost)
        If Not chkIsLPPostFeederPart.Checked Then
            rbLPPost.Checked = False
            rbFeederPart.Checked = False
        End If
    End Sub
    Private Sub rbLPPostFeederPart_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbLPPost.CheckedChanged, rbFeederPart.CheckedChanged
        cmbLPPost.Enabled = rbLPPost.Checked
        btnSelectLPPost.Enabled = rbLPPost.Checked
        cmbFeederPart.Enabled = rbFeederPart.Checked
        picFeederPart.Enabled = rbFeederPart.Checked
        If Not rbLPPost.Checked Then
            cmbLPPost.SelectedIndex = -1
            cmbLPPost.SelectedIndex = -1
            If cmbTamirNetworkType.SelectedValue = TamirNetworkType.MP Then
                lblPowerUnit.Text = "MWh"
                lblPowerUnitManovr.Text = "MWh"
            End If
        Else
            lblPowerUnit.Text = "KWh"
            lblPowerUnitManovr.Text = "KWh"
        End If
        If Not rbFeederPart.Checked Then
            cmbFeederPart.SelectedIndex = -1
            cmbFeederPart.SelectedIndex = -1
        End If

        RemoveForceControl(lblFeederPart)
        RemoveForceControl(lblLPPost)

        If rbLPPost.Checked Then
            SetForceControl(lblLPPost)
        ElseIf rbFeederPart.Checked Then
            SetForceControl(lblFeederPart)
        End If
    End Sub
    Private Sub chkIsSendToSetad_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkIsSendToSetad.CheckedChanged
        mLastIsSendToSetadMode = chkIsSendToSetad.Checked
        If CConfig.ReadConfig("IsForceGISFormNo", False) AndAlso (mLastIsSendToSetadMode Or IsSetadMode Or (mIsForConfirm And Not mIsForWarmLineConfirm)) Then
            SetForceControl(lblWorkCommandNo)
            SetForceControl(lblGISFormNo)
        Else
            RemoveForceControl(lblWorkCommandNo)
            RemoveForceControl(lblGISFormNo)
        End If
        ChangeChkIsSendToSetad()
    End Sub
    Private Sub dgFeederDC_LoadingRow(ByVal sender As Object, ByVal e As Janus.Windows.GridEX.RowLoadEventArgs) Handles dgFeederDC.LoadingRow
        If e.Row.RowType = Janus.Windows.GridEX.RowType.TotalRow Then

            Dim lHr As Integer = mSumDCInterval \ 60
            Dim lMn As Integer = mSumDCInterval Mod 60
            Dim lSumInterval As String = lHr.ToString("0#") & ":" & lMn.ToString("0#")

            e.Row.Cells("DisconnectInterval").Value = lSumInterval
            e.Row.Cells("DisconnectPower").Value = mSumPower
            e.Row.Cells("IsTamir").Value = mDCCount
            e.Row.Cells("OCEFRelayAction").Value = "تعداد عملکرد رله: " & mRelayCount
        End If
    End Sub
    Private Sub dgFeederDC_SortKeysChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgFeederDC.SortKeysChanged
        MakeRadif()
    End Sub
    Private Sub cmbPeymankar_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbPeymankar.TextChanged
        If mIsKeyPressed Then
            mPeymankarText = cmbPeymankar.Text
            cmbPeymankar.SelectedIndex = -1
            cmbPeriod.Text = mPeymankarText
            mIsKeyPressed = False
        End If
    End Sub
    Private Sub cmbPeymankar_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbPeymankar.KeyPress
        mIsKeyPressed = True
    End Sub
    Private Sub cmbPeymankar_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles cmbPeymankar.MouseDown
        If e.Button = MouseButtons.Right Then
            mIsKeyPressed = True
        End If
    End Sub
    Private Sub cmbPeymankar_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbPeymankar.LostFocus
        mIsKeyPressed = False
    End Sub
    Private Sub btnEmergencyOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEmergencyOK.Click

        If txtEmergencyReason.Text.Trim.Length = 0 Then
            ShowError("لطفا علت ثبت بدون تأييد خاموشي بابرنامه را مشخص نماييد")
            Exit Sub
        Else
            txtEmergencyReasonDisplay.Text = "علت ثبت بدون تأييد: " & vbCrLf & txtEmergencyReason.Text
            txtEmergencyReasonDisplay.Visible = True
        End If

        If Not chkIsLPPostFeederPart.Checked Then
            chkIsSendToSetad.Checked = True
            chkIsSendToSetad.Enabled = False
        End If
        chkIsSendToSetad.Text = "ارسال به ستاد"
        pnlEmergency.Hide()
        pnlEmergency.SendToBack()

        btnSave.Enabled = mSaveEnabled

        tbcMain.Enabled = True

        mDsBT.Tbl_TamirType.DefaultView.RowFilter = Regex.Replace(mDsBT.Tbl_TamirType.DefaultView.RowFilter, "TamirTypeId <> " & TamirTypes.Ezterari, "")
        cmbTamirType.Enabled = False
        cmbTamirType.SelectedValue = TamirTypes.Ezterari
        DatasetDummy1.ViewDummyId.Rows(0)("Dummy12Id") = TamirTypes.Ezterari
    End Sub
    Private Sub btnEmergencyCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEmergencyCancel.Click
        chkIsEmergency.Checked = False
        chkIsSendToSetad.Checked = mLastIsSendToSetadMode

        Dim lIsAccess As Boolean = CheckUserTrustee("TamirRequestConfirm", 30, ApplicationTypes.TamirRequest)
        chkIsSendToSetad.Enabled = (lIsAccess AndAlso Not IsAdministrator())

        If Not IsCenter And Not IsMiscMode Then
            If Not mIsTamirDirectToSetad Then
                chkIsSendToSetad.Text = "ارسال به مرکز"
            Else
                chkIsSendToSetad.Text = "ارسال به ستاد"
            End If
        Else
            chkIsSendToSetad.Text = "تأييد مدير و ارسال به ستاد"
        End If
        txtEmergencyReasonDisplay.Text = ""
        txtEmergencyReasonDisplay.Visible = False
        pnlEmergency.Hide()
        pnlEmergency.SendToBack()
        btnSave.Enabled = mSaveEnabled

        tbcMain.Enabled = True

        lIsAccess = CheckUserTrustee("CanSaveEmergency", 30, ApplicationTypes.TamirRequest)
        If Not lIsAccess Then
            Dim lRowFilter As String = mDsBT.Tbl_TamirType.DefaultView.RowFilter
            mDsBT.Tbl_TamirType.DefaultView.RowFilter &= IIf(lRowFilter <> "", " AND ", "") & "TamirTypeId <> " & TamirTypes.Ezterari
        End If

        If Not mIsFogheToziUser Then
            cmbTamirType.Enabled = True
            cmbTamirType.SelectedIndex = -1
            cmbTamirType.SelectedIndex = -1
        Else
            cmbTamirType.SelectedValue = TamirTypes.BarnamehRiziShodeh
            cmbTamirType.Enabled = False
        End If
    End Sub
    Private Sub lnkManoeuvreDesc_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkManoeuvreDesc.LinkClicked
        pnlManoeuvreDesc.Show()
        pnlManoeuvreDesc.BringToFront()
    End Sub
    Private Sub picOKManoeuvreDesc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picOKManoeuvreDesc.Click
        pnlManoeuvreDesc.Hide()
        pnlManoeuvreDesc.SendToBack()
    End Sub
    Private Sub cmbTamirNetworkType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbTamirNetworkType.SelectedIndexChanged
        Dim lIsChange As Boolean = cmbTamirNetworkType.SelectedValue <> mTamirNetworkTypeId 'mEditingRow("TamirNetworkTypeId")
        CheckNetworkType(lIsChange)
        If lIsChange Then
            LoadPowerInfo(lIsChange)
            mTamirNetworkTypeId = cmbTamirNetworkType.SelectedValue
        End If
        ChangeTamirNetworkType()
        If cmbTamirNetworkType.SelectedValue = TamirNetworkType.MP Then
            btnFeederKeyVisibleState(True)
        End If
        If cmbTamirNetworkType.SelectedValue = TamirNetworkType.FT Then
            chkMPPostTrans.Visible = True
            lblMPPostTrans.Visible = True
        Else
            chkMPPostTrans.Visible = False
            lblMPPostTrans.Visible = False
        End If
    End Sub
    Private Sub rbIsRequestByFogheTozi_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbIsRequestByFogheTozi.CheckedChanged
        Dim lTNT As TamirNetworkType = TamirNetworkType.None
        If rbIsRequestByFogheTozi.Checked Then
            lTNT = TamirNetworkType.FT
            lblLetterInfo.Visible = True
        Else
            lTNT = TamirNetworkType.MP
            lblLetterInfo.Visible = False
            txtLetterDate.Clear()
            txtLetterNo.Clear()
        End If
        DatasetDummy1.ViewDummyId.Rows(0)("Dummy10Id") = lTNT
        cmbTamirNetworkType.SelectedValue = lTNT
        mEditingRow("TamirNetworkTypeId") = lTNT
        mTamirNetworkTypeId = lTNT
    End Sub
    Private Sub rbIsRequestBy_CheckedChanged(sender As Object, e As System.EventArgs) Handles rbIsRequestByFogheTozi.CheckedChanged, rbIsRequestByPeymankar.CheckedChanged, rbIsRequestByTozi.CheckedChanged
        chkMPPostTrans_AfterCheck(sender, Nothing)
    End Sub
    Private Sub btnMultiStep_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMultiStep.Click
        If Not txtDateDisconnect.IsOK Or Not txtTimeDisconnect.IsOK Then
            ShowError("ابتدا تاريخ شروع خاموشي و بار به هنگام قطع را مشخص نماييد.", False, MsgBoxIcon.MsgIcon_Exclamation)
            Exit Sub
        Else
            txtDateDisconnect.InsertTime(txtTimeDisconnect)
        End If
        LoadMultiStepInfo()
        pnlMultiStepInfo.BringToFront()
        mSaveEnabled = btnSave.Enabled
        btnSave.Enabled = False
        pnlMultiStepInfo.Show()
    End Sub
    Private Sub btnMSCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMSCancel.Click
        If DatasetTemp1.TblTamirRequestKey.Columns.Contains("IsTemp") Then
            Dim lDelArray As New ArrayList
            For Each lRow As DataRow In DatasetTemp1.TblTamirRequestKey.Rows
                If lRow.RowState <> DataRowState.Deleted Then
                    If lRow("IsTemp") Then
                        lRow("IsOpen") = lRow("IsTempOpen")
                        lRow("IsClose") = lRow("IsTempClose")
                        lRow("KeyStateId") = lRow("TempKeyStateId")
                    End If
                End If
            Next
            'For Each lRow As DataRow In lDelArray
            '    lRow.Delete()
            'Next
            DatasetTemp1.TblTamirRequestKey.Columns.Remove("IsTemp")
            DatasetTemp1.TblTamirRequestKey.Columns.Remove("IsTempOpen")
            DatasetTemp1.TblTamirRequestKey.Columns.Remove("IsTempClose")
            DatasetTemp1.TblTamirRequestKey.Columns.Remove("TempKeyStateId")
        End If

        pnlMultiStepInfo.Hide()
        pnlMultiStepInfo.SendToBack()
        btnSave.Enabled = mSaveEnabled
        ClearMultiStepForm()
    End Sub
    Private Sub btnMSOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMSOK.Click
        If IsMultiStepsOK() Then
            Dim lSumBar As Double = SetMultiStepInfo()

            If lSumBar > 0 Then
                btnMultiStep.Text = "[" & btnMultiStep.Tag & "]"
                btnMultiStep.Font = New System.Drawing.Font("Tahoma", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
                txtDateConnect.MiladiDT = mLastMSDateTime
                txtTimeConnect.MiladiDT = mLastMSDateTime
                Dim lSum As Double = ComputeMultiSteps()
                txtCurrentValue.Text = lSum
            Else
                btnMultiStep.Text = btnMultiStep.Tag
                btnMultiStep.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
            End If

            btnSave.Enabled = mSaveEnabled
            pnlMultiStepInfo.Hide()
            pnlMultiStepInfo.SendToBack()
            ClearMultiStepForm()
        End If
    End Sub
    Private Sub btnSelectLPPost_Click(sender As Object, e As EventArgs) Handles btnSelectLPPost.Click
        SelectLPPost()
    End Sub
    Private Sub txtMSDate_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMSDate1.GotFocus, txtMSDate2.GotFocus, txtMSDate3.GotFocus, txtMSDate4.GotFocus, txtMSDate5.GotFocus, txtMSDate6.GotFocus
        Dim lTxtDate As PersianMaskedEditor = sender
        If Not lTxtDate.IsOK Then
            lTxtDate.MiladiDT = txtDateDisconnect.MiladiDT
        End If
    End Sub
    Private Sub txtMSTime_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMSTime1.TextChanged, txtMSTime2.TextChanged, txtMSTime3.TextChanged, txtMSTime4.TextChanged, txtMSTime5.TextChanged, txtMSTime6.TextChanged
        Dim lTxtTime As TimeMaskedEditor = sender
        If lTxtTime.IsOK Then
            Try
                Dim lTxtDate As PersianMaskedEditor = Nothing
                lTxtDate = Bargh_Common.mdlHashemi.FindControl(pnlMultiStepInfo.Controls, lTxtTime.Name.Replace("Time", "Date"))
                If Not lTxtDate.IsOK Then
                    lTxtDate.MiladiDT = txtDateDisconnect.MiladiDT
                End If
            Catch ex As Exception
            End Try
        End If
    End Sub
    Private Sub sbtnOverlaps_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sbtnOverlaps.Click
        If Not pnlOverlaps.Visible Then
            pnlOverlaps.BringToFront()
            pnlOverlaps.Show()
        Else
            pnlOverlaps.Hide()
            pnlOverlaps.SendToBack()
        End If
    End Sub
    Private Sub btnCloseOverlaps_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseOverlaps.Click
        pnlOverlaps.Hide()
        pnlOverlaps.SendToBack()
    End Sub
    Private Sub picSearchMPPOverlaps_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles picSearchMPPOverlaps.Click
        FindOverlapTimes(False, False)
        pnlSOverlaps.BringToFront()
        pnlSOverlaps.Show()
    End Sub
    Private Sub picSearchMPFOverlaps_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles picSearchMPFOverlaps.Click
        FindOverlapTimes(False)
        pnlSOverlaps.BringToFront()
        pnlSOverlaps.Show()
    End Sub
    Private Sub btnSCloseOverlaps_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSCloseOverlaps.Click
        pnlSOverlaps.Hide()
        pnlSOverlaps.SendToBack()
    End Sub
    Private Sub txtCriticalLocations_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCriticalLocations.KeyPress
        mIsManualChangeCL = True
    End Sub
    Private Sub btnHidePnlLetter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHidePnlLetter.Click
        pnlLetterInfo.Hide()
        pnlLetterInfo.SendToBack()
    End Sub
    Private Sub lblLetterInfo_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblLetterInfo.LinkClicked
        pnlLetterInfo.Show()
        pnlLetterInfo.BringToFront()
    End Sub
    Private Sub picIsAuotoNumber_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picIsAuotoNumber.Click
        chkIsAutoNumber.Checked = Not chkIsAutoNumber.Checked
    End Sub
    Private Sub chkIsAutoNumber_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIsAutoNumber.CheckedChanged
        txtAllowNumber.ReadOnly = chkIsAutoNumber.Checked
        If chkIsAutoNumber.Checked And (Not IsNumeric(txtAllowNumber.Text) Or mDs.TblTamirRequestAllow.Rows.Count = 0) Then
            txtAllowNumber.Text = "بصورت خوکار توليد خواهد شد"
        ElseIf Not IsNumeric(txtAllowNumber.Text) Then
            txtAllowNumber.Text = ""
        End If
    End Sub
    Private Sub cmbTamirType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbTamirType.SelectedIndexChanged
        If mIsAutoChangeTamirType Then Exit Sub
        'Dim lIsAccessConfirm As Boolean = CheckUserTrustee("TamirRequestConfirm", 30, ApplicationTypes.TamirRequest) AndAlso Not IsAdministrator()
        Dim lIsAccessEmergency As Boolean = CheckUserTrustee("CanSaveEmergency", 30, ApplicationTypes.TamirRequest)

        If cmbTamirType.SelectedValue = TamirTypes.Ezterari Then
            chkIsSendToSetad.Enabled = lIsAccessEmergency
            mIsEmergencyEnabled = lIsAccessEmergency
            'If Not lIsAccessEmergency Then
            chkIsSendToSetad.Checked = False
            'End If
            If Not chkIsEmergency.Checked Then
                pnlEmergencyReason.Visible = True
            End If

            If mIsForceEmergencyReason Then
                SetForceControl(lblEmergencyReason)
            End If
        Else
            'chkIsSendToSetad.Enabled = lIsAccessConfirm
            mIsEmergencyEnabled = False
            chkIsSendToSetad.Checked = False

            pnlEmergencyReason.Visible = False

            RemoveForceControl(lblEmergencyReason)
        End If
    End Sub

    Private Sub tbcDCs_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbcDCs.SelectedIndexChanged
        If tbcDCs.SelectedTab.Name = "tpDCMain" Then
            pnlManoeuvreRequestInfo.SendToBack()
            pnlManoeuvreRequestInfo.Hide()
            pnlTamirRequestInfo.BringToFront()
            pnlTamirRequestInfo.Show()
        ElseIf tbcDCs.SelectedTab.Name = "tpDCMon" Then
            pnlTamirRequestInfo.SendToBack()
            pnlTamirRequestInfo.Hide()
            pnlManoeuvreRequestInfo.BringToFront()
            pnlManoeuvreRequestInfo.Show()
        End If
        tbcDCs.BringToFront()
    End Sub
    Private Sub txtDateDisconnectManovr_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
        txtDateDisconnectManovr.TextChanged, txtDateConnectManovr.TextChanged, txtTimeDisconnectManovr.TextChanged, txtTimeConnectManovr.TextChanged
        ComputeDCIntervalManovr()
    End Sub
    Private Sub txtCurrentValueManovr_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCurrentValueManovr.TextChanged
        LoadPowerInfo(txtCurrentValueManovr.Text.Trim.Length = 0)
    End Sub
    Private Sub txtDCIntervalManovr_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDCIntervalManovr.TextChanged
        LoadPowerInfo()
    End Sub
    Private Sub chkIsNotNeedManovrDC_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIsNotNeedManovrDC.CheckedChanged
        Dim lIsManovr As Boolean = chkIsManoeuvre.Checked And Not chkIsNotNeedManovrDC.Checked

        If Not lIsManovr Then
            txtDateDisconnectManovr.EmptyDate()
            txtDateConnectManovr.EmptyDate()
            txtTimeDisconnectManovr.EmptyTime()
            txtTimeConnectManovr.EmptyTime()
            txtCurrentValueManovr.Text = ""
        End If

        txtDateDisconnectManovr.Enabled = lIsManovr
        txtDateConnectManovr.Enabled = lIsManovr
        txtTimeDisconnectManovr.Enabled = lIsManovr
        txtTimeConnectManovr.Enabled = lIsManovr
        txtCurrentValueManovr.Enabled = lIsManovr

        lblRequestNumberManovr.Visible = lIsManovr
        lnkRequestNumberManovr.Visible = lIsManovr

        lblRequestNumber.Top = IIf(lIsManovr, 34, 44)
        lnkRequestNumber.Top = IIf(lIsManovr, 34, 44)
        lblRequestNumber.Left = IIf(lIsManovr, lblRequestNumber.Left, lblTamirRequestNo.Left)
        lnkRequestNumber.Left = lblRequestNumber.Left - 152
    End Sub
    Private Sub lblPowerUnit_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblPowerUnit.TextChanged
        Label1.Text = lblPowerUnit.Text
    End Sub
    Private Sub txtPower_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPower.TextChanged, txtPowerManovr.TextChanged
        Try
            txtPowerTotal.Text = Val(txtPower.Text) + Val(txtPowerManovr.Text)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub rbIsRequestByPeymankar_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbIsRequestByPeymankar.CheckedChanged
        If CConfig.ReadConfig("IsForcePeymankar", "False") Then
            If rbIsRequestByPeymankar.Checked Then
                SetForceControl(lblPeymankar)
                cmbPeymankar.DropDownStyle = ComboBoxStyle.DropDownList
            Else
                RemoveForceControl(lblPeymankar)
                cmbPeymankar.DropDownStyle = ComboBoxStyle.DropDown
            End If
        End If
    End Sub
    Private Sub chkIsConfirmNazer_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkIsConfirmNazer.CheckedChanged
        If mIsLoading Then Exit Sub
        If chkIsConfirmNazer.Checked Then
            If cmbNazer.SelectedIndex = -1 Then
                chkIsConfirmNazer.Checked = False
                ShowError("لطفا ناظر را مشخص نماييد")
                cmbNazer.Focus()
            End If
        End If
    End Sub

    Private Sub rbConfirmWL_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbConfirmWL.CheckedChanged
        txtWarmLineReason.Enabled = Not rbConfirmWL.Checked
        If rbIsReturnedWL.Checked Then
            Label111.Text = "علت عودت"
        Else
            Label111.Text = "علت عدم تأييد"
        End If
    End Sub
    Private Sub dgFeederDC_LinkClicked(ByVal sender As Object, ByVal e As Janus.Windows.GridEX.ColumnActionEventArgs) Handles dgFeederDC.LinkClicked
        Try
            If e.Column.Key = "RequestNo" Then
                Dim lRequestId As Long
                lRequestId = dgFeederDC.Item(dgFeederDC.CurrentRowIndex, "RequestId")
                ShowDisconnect(lRequestId)
            End If
        Catch ex As Exception
            ShowError("پرونده خاموشي پيدا نشد", False, MsgBoxIcon.MsgIcon_Exclamation)
        End Try
    End Sub

    Private Sub lblPeymankar_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lblPeymankar.LinkClicked
        pnlPeymankar.BringToFront()
        pnlPeymankar.Visible = True
        txtPeymankarTel.Focus()
    End Sub

    Private Sub btnPeymankar_Click(sender As Object, e As EventArgs) Handles btnPeymankar.Click
        pnlPeymankar.Visible = False
        If txtPeymankarTel.Text <> "" Then
            GlobalToolTip.SetToolTip(lblPeymankar, "شماره موبايل جهت ارسال پيامک : " & txtPeymankarTel.Text)
        Else
            GlobalToolTip.SetToolTip(lblPeymankar, "")
        End If
        txtPeymankarTel.BackColor = Color.White
    End Sub

    Private Sub cmbDepartment_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbDepartment.SelectedIndexChanged
        If mIsLoading Then Exit Sub
        Dim lNazerIndex As Integer = cmbNazer.SelectedIndex
        If cmbDepartment.SelectedIndex > -1 Then
            Dim lNazerId As String = cmbNazer.SelectedValue
            SearchAndFillNazer(True)
            Dim lRow() As DataRow = mDs.Tables("TBL_NazerDepartment").Select("NazerId = " & IIf(lNazerId > 0, lNazerId, 0))
            'cmbNazer.DataSource = lDs.Tables("TBL_NazerDepartment")

            If lNazerId Is Nothing Then cmbNazer.SelectedIndex = -1
            If lNazerIndex = -1 Then Exit Sub

            If lRow.Length > 0 Then
                cmbNazer.SelectedValue = lNazerId
            Else
                MsgBoxF("کاربر گرامي باتوجه به انتخاب واحد درخواست کننده؛ مجداً ناظر خود را انتخاب نماييد", "پيغام", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Information, MessageBoxDefaultButton.Button1)
                cmbNazer.SelectedIndex = -1
            End If
        End If
    End Sub

    ' MPFeederKey ----------------------
    Private Sub btnFeederKey0_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFeederKey0.Click
        cmnuFeederKey.Show(btnFeederKey0, New Point(0, btnFeederKey0.Height))
    End Sub
    Private Sub cmnuFeederKeyOnDisconnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuFeederKeyOnDisconnect.Click
        mIsFinal = False
        mTamirRequestMultiStepId = -1
        FillTempTblMPRequestKey(mIsFinal)
    End Sub
    Private Sub cmnuFeederKeyOnConnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuFeederKeyOnConnect.Click
        mIsFinal = True
        mTamirRequestMultiStepId = -1
        FillTempTblMPRequestKey(mIsFinal)
    End Sub
    Private Sub btnFeederKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFeederKey1.Click, btnFeederKey2.Click, btnFeederKey3.Click, btnFeederKey4.Click, btnFeederKey5.Click, btnFeederKey6.Click
        Dim lIndex As Integer = CType(sender, Control).Tag
        Dim lMultiStepId As Long = lIndex
        If mMultiStepFeederKey.ContainsKey(lIndex) Then
            lMultiStepId = mMultiStepFeederKey.Item(lIndex)
        End If
        mTamirRequestMultiStepId = lMultiStepId
        FillTempTblMPRequestKey(False, lMultiStepId)
    End Sub
    Private Sub btnReturnFeederKey_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReturnFeederKey.Click
        If Not IsNothing(mTblTemp_FeederKey) Then
            mTblTemp_FeederKey = DatasetTemp1.TblTamirRequestKey.Copy()
        End If
        For Each lCtrl As Control In Me.Controls
            lCtrl.Enabled = mControlEnableState.Item(lCtrl.Name)
        Next
        mTblTemp_FeederKey.Dispose()
        mTblTemp_FeederKey = Nothing

        mTblBase_FeederKey.Dispose()
        mTblBase_FeederKey = Nothing

        pnlMPFeederKey.Visible = False
    End Sub
    Private Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.ver_960328) Then
            ShowHavadesInfoMessage()
            Exit Sub
        End If
        Dim lRows() As DataRow
        If mTamirRequestMultiStepId > -1 Then
            If Not DatasetTemp1.TblTamirRequestKey.Columns.Contains("IsTemp") Then
                DatasetTemp1.TblTamirRequestKey.Columns.Add("IsTemp", GetType(Boolean))
            End If
            If Not DatasetTemp1.TblTamirRequestKey.Columns.Contains("IsTempOpen") Then
                DatasetTemp1.TblTamirRequestKey.Columns.Add("IsTempOpen", GetType(Boolean))
            End If
            If Not DatasetTemp1.TblTamirRequestKey.Columns.Contains("IsTempClose") Then
                DatasetTemp1.TblTamirRequestKey.Columns.Add("IsTempClose", GetType(Boolean))
            End If
            If Not DatasetTemp1.TblTamirRequestKey.Columns.Contains("TempKeyStateId") Then
                DatasetTemp1.TblTamirRequestKey.Columns.Add("TempKeyStateId", GetType(Integer))
            End If
        End If
        For Each lRow As DataRow In mTblTemp_FeederKey.Rows
            If lRow.RowState <> DataRowState.Deleted Then
                lRows = DatasetTemp1.TblTamirRequestKey.Select("TamirRequestKeyId = " & lRow("TamirRequestKeyId"))
                If lRows.Length > 0 Then

                    Dim lStateId As Integer

                    If mTamirRequestMultiStepId > -1 Then
                        lRows(0)("IsTempOpen") = lRows(0)("IsOpen")
                        lRows(0)("IsTempClose") = lRows(0)("IsClose")

                        lStateId = IIf(Not IsDBNull(lRows(0)("IsOpen")) AndAlso lRows(0)("IsOpen"), 1, IIf(Not IsDBNull(lRows(0)("IsClose")) AndAlso lRows(0)("IsClose"), 2, 0))
                        lRows(0)("TempKeyStateId") = lStateId

                        lRows(0)("IsTemp") = True
                    End If

                    lRows(0)("IsOpen") = lRow("IsOpen")
                    lRows(0)("IsClose") = lRow("IsClose")

                    lStateId = IIf(Not IsDBNull(lRow("IsOpen")) AndAlso lRow("IsOpen"), 1, IIf(Not IsDBNull(lRow("IsClose")) AndAlso lRow("IsClose"), 2, 0))
                    lRows(0)("KeyStateId") = lStateId

                End If
            End If
        Next
        For Each lRow As DataRow In mTblTemp_FeederKey.Rows
            If lRow.RowState <> DataRowState.Deleted Then
                lRows = DatasetTemp1.TblTamirRequestKey.Select("TamirRequestKeyId = " & lRow("TamirRequestKeyId") &
                        IIf(mTamirRequestMultiStepId > -1, " AND TamirRequestMultiStepId = " & mTamirRequestMultiStepId, " AND TamirRequestMultiStepId IS NULL"))
                If lRows.Length > 0 Then
                    lRows(0)("IsRemoteChange") = chkIsRemoteChange.Checked
                End If
            End If
        Next
        For Each lCtrl As Control In Me.Controls
            lCtrl.Enabled = mControlEnableState.Item(lCtrl.Name)
        Next
        mTblTemp_FeederKey.Dispose()
        mTblTemp_FeederKey = Nothing

        mTblBase_FeederKey.Dispose()
        mTblBase_FeederKey = Nothing

        pnlMPFeederKey.Visible = False
    End Sub
    Private Sub dgFeederKey_CellEdited(ByVal sender As Object, ByVal e As Janus.Windows.GridEX.ColumnActionEventArgs) Handles dgFeederKey.CellEdited
        If e.Column.Key = "IsOpen" Then
            If dgFeederKey.Item(dgFeederKey.CurrentRowIndex, "IsOpen") = True Then
                dgFeederKey.SetValue("KeyStateId", 1)
            End If
        ElseIf e.Column.Key = "IsClose" Then
            If dgFeederKey.Item(dgFeederKey.CurrentRowIndex, "IsClose") = True Then
                dgFeederKey.SetValue("KeyStateId", 2)
            End If
        End If
    End Sub
    Private Sub dgFeederKey_CellValueChanged(ByVal sender As Object, ByVal e As Janus.Windows.GridEX.ColumnActionEventArgs) Handles dgFeederKey.CellValueChanged
        Try
            If e.Column.Key = "IsOpen" Then
                If dgFeederKey.Item(dgFeederKey.CurrentRowIndex, "IsClose") = True Then
                    dgFeederKey.SetValue("IsClose", False)
                End If
                If dgFeederKey.Item(dgFeederKey.CurrentRowIndex, "IsOpen") = True Then
                    dgFeederKey.SetValue("KeyStateId", 1)
                End If
            ElseIf e.Column.Key = "IsClose" Then
                If dgFeederKey.Item(dgFeederKey.CurrentRowIndex, "IsOpen") = True Then
                    dgFeederKey.SetValue("IsOpen", False)
                End If
                If dgFeederKey.Item(dgFeederKey.CurrentRowIndex, "IsClose") = True Then
                    dgFeederKey.SetValue("KeyStateId", 2)
                End If
            End If

        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub
    Private Sub cmbMPCloserType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMPCloserType.SelectedIndexChanged
        If mIsLoading Then Exit Sub
        Try
            mTblTemp_FeederKey.DefaultView.RowFilter = CreateMPFeederKeyWhere(mIsFinal, mTamirRequestMultiStepId)
        Catch ex As Exception
        End Try
    End Sub
    Private Sub txtMPFeederKey_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMPFeederKey.TextChanged, txtMPFeederKeyGIS.TextChanged
        Try
            mTblTemp_FeederKey.DefaultView.RowFilter = CreateMPFeederKeyWhere(mIsFinal, mTamirRequestMultiStepId)
        Catch ex As Exception
        End Try
    End Sub
    ' MPFeederKey ----------------------

    Private Sub picPeymankarSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picPeymankarSearch.Click
        txtPeymankarSearch.Visible = Not txtPeymankarSearch.Visible
        If txtPeymankarSearch.Visible Then
            txtPeymankarSearch.Focus()
        Else
            cmbPeymankar.Focus()
        End If
    End Sub
    Private Sub picFeederPart_Click(sender As Object, e As EventArgs) Handles picFeederPart.Click
        txtFeederPart.Visible = Not txtFeederPart.Visible
        If txtFeederPart.Visible Then
            txtFeederPart.Focus()
        Else
            cmbFeederPart.Focus()
        End If
    End Sub
    Private Sub txtPeymankarSearch_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPeymankarSearch.TextChanged
        If txtPeymankarSearch.Visible Then
            SearchPeymankar()
        End If
    End Sub
    Private Sub textNazerSearch_TextChanged(sender As Object, e As EventArgs) Handles txtNazerSearch.TextChanged
        If txtNazerSearch.Visible Then
            SearchAndFillNazer(False)
        End If
    End Sub
    Private Sub txtFeederPart_TextChanged(sender As Object, e As EventArgs) Handles txtFeederPart.TextChanged
        If txtFeederPart.Visible Then
            SearchFeederPart()
        End If
    End Sub
    Private Sub picNazerSearch_Click(sender As Object, e As EventArgs) Handles picNazerSearch.Click
        txtNazerSearch.Visible = Not txtNazerSearch.Visible
        If txtNazerSearch.Visible Then
            txtNazerSearch.Focus()
        Else
            cmbNazer.Focus()
        End If
    End Sub
    Private Sub chkMPPostTrans_AfterCheck(sender As Object, e As chkComboEventArgs) Handles chkMPPostTrans.AfterCheck
        If mIsLoading Then Exit Sub
        LoadPowerInfo(mIsUpdateCurrentValue)
        mIsLoading = True
        Dim lMPPostId As Integer = -1
        Dim lAreaId As Integer = -1
        Dim lMPPostTransIDs As String = ""

        If cmbArea.SelectedIndex > -1 Then
            lAreaId = cmbArea.SelectedValue
        End If
        If chkMPPostTrans.GetDataList().Length > 0 Then
            lMPPostTransIDs = chkMPPostTrans.GetDataList()
        End If

        Dim lMPFeederIDs As String = ckcmbMPFeeder.GetDataList()

        mDsBT.Tbl_MPFeeder.Clear()
        ckcmbMPFeeder.Clear()

        Dim lNetworkType As TamirNetworkType = cmbTamirNetworkType.SelectedValue

        If lMPPostTransIDs <> "" Then

            Dim lWhereArea As String = ""
            Dim lWhereCommonFeeder As String = ""
            Dim lWhere As String = ""

            If lNetworkType <> TamirNetworkType.FT Or Not rbIsRequestByFogheTozi.Checked Then
                lWhereArea = String.Format("(AreaId = {0})", lAreaId)
                lWhereCommonFeeder = "(MPFeederId IN (SELECT Tbl_MPCommonFeeder.MPFeederId FROM Tbl_MPCommonFeeder INNER JOIN Tbl_MPFeeder ON Tbl_MPCommonFeeder.MPFeederId = Tbl_MPFeeder.MPFeederId WHERE (Tbl_MPCommonFeeder.AreaId = " & lAreaId & ") AND (Tbl_MPFeeder.IsActive = 1)))"
                lWhere = String.Format("({0} OR {1}) AND ", lWhereArea, lWhereCommonFeeder)
            End If

            Dim lSQL As String = "SELECT * FROM Tbl_MPFeeder " &
                                "WHERE " & lWhere &
                                "	(ISNULL(MPPostTransId,-1) IN (" & lMPPostTransIDs & ")) " &
                                "	And Tbl_MPFeeder.MPPostId = " & cmbMPPost.SelectedValue &
                                "	And (IsActive = 1) "
            RemoveMoreSpaces(lSQL)
            BindingTable(lSQL, mCnn, mDs, "Tbl_MPFeederAll", aIsClearTable:=True)

            ckcmbMPFeeder.Fill(mDs.Tables("Tbl_MPFeederAll"), "MPFeederName", "MPFeederId", 20)
        End If

        ckcmbMPFeeder.SetData(lMPFeederIDs)
        cmbMPFeeder.SelectedIndex = -1
        cmbMPFeeder.SelectedIndex = -1

        mIsLoading = False
    End Sub

    '--- File Management Section ---'
    Private Sub btnBrowseForFileUpload_Click(sender As Object, e As EventArgs) Handles btnBrowseForFileUpload.Click
        Try
            Dim lOFD As New OpenFileDialog()
            lOFD.CheckFileExists = True
            lOFD.Title = "انتخاب فايل سند براي خاموشي"
            lOFD.Filter = "All Files (*.*)|*.*"
            If lOFD.ShowDialog() = DialogResult.Cancel Then
                Exit Sub
            End If

            txtUploadFilePath.Text = lOFD.FileName
        Catch ex As Exception
        End Try
    End Sub
    Private Sub btnAddFile_Click(sender As Object, e As EventArgs) Handles btnAddFile.Click
        AddFileForUpload()
    End Sub
    Protected Sub dgFiles_BeforeContexdtMenuOpen(sender As Object, e As EventArgs) Handles dgFiles.BeforecontextMenuOpen
        Try
            Dim lRowIndex As Integer = dgFiles.CurrentRowIndex

            Dim lTamirRequestFileId As Long
            Long.TryParse(dgFiles.Item(lRowIndex, "TamirRequestFileId").ToString(), lTamirRequestFileId)

            Dim lDFi As List(Of DeletFileInfo) = (
                From lItem In mFileDeleteList
                Where lItem.TamirRequestFileId = lTamirRequestFileId
                Select lItem
            ).ToList()

            If lDFi.Count = 0 Then
                dgFiles.RenameContextMenuItem("DeleteFile", "حذف فايل")
            Else
                dgFiles.RenameContextMenuItem("DeleteFile", "عدم حذف فايل")
            End If
        Catch ex As Exception
            ShowError(ex.Message)
        End Try
    End Sub
    Private Sub ftDocs_RetryRequest(sender As Object, e As FileServerEventArgs) Handles ftDocs.RetryRequest
        If e.WorkingType = CFileServer.WorkingTypes.Sending Then
            RetryUploadFiles()
        ElseIf e.WorkingType = CFileServer.WorkingTypes.Deleting Then
            RetryDeleteFiles()
        ElseIf e.WorkingType = CFileServer.WorkingTypes.Receiving Then
            DownloadFile(e.FileId, e.FilePath)
        End If
    End Sub
    Private Sub ftDocs_SkipRequest(sender As Object, e As FileServerEventArgs) Handles ftDocs.SkipRequest
        If e.WorkingType = CFileServer.WorkingTypes.Sending Then
            SkipFailUploads()
        ElseIf e.WorkingType = CFileServer.WorkingTypes.Deleting Then
            SkipFailDeletes()
        ElseIf e.WorkingType = CFileServer.WorkingTypes.Receiving Then
            ftDocs.Visible = False
        End If
    End Sub
    Private Sub dgFiles_LinkClicked(sender As Object, e As ColumnActionEventArgs) Handles dgFiles.LinkClicked
        If e.Column.Key = "Content" Then
            Dim lFileId As Long
            Long.TryParse(dgFiles.Item(dgFiles.CurrentRowIndex, "FileId").ToString(), lFileId)
            Dim lSubject As String = dgFiles.Item(dgFiles.CurrentRowIndex, "Subject")
            Dim lFileName As String = dgFiles.Item(dgFiles.CurrentRowIndex, "FileName")
            Dim lAreaUserId As Integer = dgFiles.Item(dgFiles.CurrentRowIndex, "AreaUserId")
            If lAreaUserId <> WorkingUserId And Not IsAdmin() Then
                Dim lIsAccess As Boolean = CheckUserTrustee("CanViewFileFromFileServer", 30, mApplicationType)
                If Not lIsAccess Then
                    ShowNoAccessMessageByTag("CanViewFileFromFileServer")
                    Exit Sub
                End If
            End If
            DownloadFile(lFileId, lFileName, lSubject)
        End If
    End Sub
    Private Sub dgFiles_RowDoubleClick(sender As Object, e As RowActionEventArgs) Handles dgFiles.RowDoubleClick
        Dim lFileId As Long
        Long.TryParse(dgFiles.Item(e.Row.RowIndex, "FileId").ToString(), lFileId)
        Dim lSubject As String = dgFiles.Item(e.Row.RowIndex, "Subject")
        Dim lFileName As String = dgFiles.Item(e.Row.RowIndex, "FileName")
        Dim lAreaUserId As Integer = dgFiles.Item(dgFiles.CurrentRowIndex, "AreaUserId")
        If lAreaUserId <> WorkingUserId And Not IsAdmin() Then
            Dim lIsAccess As Boolean = CheckUserTrustee("CanViewFileFromFileServer", 30, mApplicationType)
            If Not lIsAccess Then
                ShowNoAccessMessageByTag("CanViewFileFromFileServer")
                Exit Sub
            End If
        End If
        DownloadFile(lFileId, lFileName, lSubject)
    End Sub
    Private Sub dgFiles_ContextMenuItemSelect(sender As Object, e As ContextMenuEventArg) Handles dgFiles.OnContextMenuItemSelect
        If e.Key = "DeleteFile" Then
            Dim lAreaUserId As Integer = dgFiles.Item(dgFiles.CurrentRowIndex, "AreaUserId")
            If lAreaUserId <> WorkingUserId And Not IsAdmin() Then
                Dim lIsAccess As Boolean = CheckUserTrustee("CanDeleteFileFromFileServer", 30, ApplicationTypes.TamirRequest)
                If Not lIsAccess Then
                    ShowNoAccessMessageByTag("CanDeleteFileFromFileServer")
                    Exit Sub
                End If
            End If
            AddFileToDeleteList()
        End If
    End Sub
    '-------------------------------'

    Private Sub btnPeymankarTel_Click(sender As Object, e As EventArgs) Handles btnPeymankarTel.Click
        Dim lOldMobile As String = txtPeymankarTel.Text
        If cmbPeymankar.SelectedIndex = -1 Then
            ShowError("جهت دريافت شماره موبايل پيمانکار، ابتدا بايد پيمانکار مورد نظر را از ليست انتخاب نماييد")
            Exit Sub
        End If
        Dim lMobile As Object = mDs.Tables("Tbl_Peymankar").Select("PeymankarId = " & cmbPeymankar.SelectedValue)(0)("Mobile")
        txtPeymankarTel.Text = ""
        If Not IsDBNull(lMobile) Then
            txtPeymankarTel.Text = lMobile
        End If

        If lOldMobile <> txtPeymankarTel.Text Then
            txtPeymankarTel.BackColor = Color.LightPink
        End If

    End Sub
    Private Sub cmbPeymankar_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPeymankar.SelectedIndexChanged
        Try
            lblPeymankarManager.Text = "_"
            If cmbPeymankar.SelectedIndex > -1 Then
                Dim lPeymankarManager As Object = mDs.Tables("Tbl_Peymankar").Select("PeymankarId = " & cmbPeymankar.SelectedValue)(0)("Manager")
                If Not IsDBNull(lPeymankarManager) Then
                    lblPeymankarManager.Text = lPeymankarManager
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub
#End Region

#Region "Methods"
    Private Sub LogTamirNetworkType(Optional ByVal aTitle As String = "")
        SaveLog("---------------", "TNT.log")
        If aTitle <> "" Then
            SaveLog(aTitle, "TNT.log")
            SaveLog("---------------", "TNT.log")
        End If
        Try
            SaveLog("TamirNetworkTypeCombo = " & cmbTamirNetworkType.SelectedValue, "TNT.log")
        Catch ex As Exception
        End Try
        Try
            SaveLog("mTamirNetworkTypeId = " & mTamirNetworkTypeId, "TNT.log")
        Catch ex As Exception
        End Try
        Try
            SaveLog("Dummy10Id = " & mRowD("Dummy10Id"), "TNT.log")
        Catch ex As Exception
        End Try
        Try
            SaveLog("mEditingRow('TamirNetworkTypeId') = " & mEditingRow("TamirNetworkTypeId"), "TNT.log")
        Catch ex As Exception
        End Try
        SaveLog("---------------", "TNT.log")
    End Sub

    Private Sub Init()
        If Not mDs.TblTamirRequest.Columns.Contains("ReturnTimeoutDT") Then
            mDs.TblTamirRequest.Columns.Add("ReturnTimeoutDT")
        End If
        Dim lSQl As String
        Dim lTbl As New DataTable("Tbl_Period")

        lTbl = New DataTable("Tbl_Period")
        lTbl.Columns.Add("ID", System.Type.GetType("System.String"))
        lTbl.Columns.Add("Period", System.Type.GetType("System.String"))
        mDs.Tables.Add(lTbl)
        With lTbl.Rows
            .Add(New String() {"manual", "بازه زماني دلخواه"})
            .Add(New String() {"week", "يک هفته قبل"})
            .Add(New String() {"month", "يک ماه قبل"})
            .Add(New String() {"season", "سه ماه قبل"})
            .Add(New String() {"year", "يک سال قبل"})
            .Add(New String() {"ndays", "n روز گذشته"})
            .Add(New String() {"nDCs", "n خاموشي آخر"})
        End With
        cmbPeriod.ValueMember = "ID"
        cmbPeriod.DisplayMember = "Period"
        cmbPeriod.DataSource = lTbl

        mFirstDay_DT = GetFirstDateOfTamirScope(GetServerTimeInfo().DateOnly)
        mFirstDay_PDate = GetFirstDatePersianOfTamirScope(mFirstDay_DT)

        LoadAreaDataTable(mCnn, mDsBT)
        LoadCityDataTable(mCnn, mDsBT)

        If Not IsCenter Then
            cmbArea.SelectedValue = WorkingAreaId
            cmbCity.SelectedValue = WorkingCityId
            cmbArea.Enabled = False
            If mDsBT.Tbl_City.Rows.Count = 1 Then
                cmbCity.Enabled = False
            End If
        Else
            cmbArea.SelectedIndex = -1
            cmbCity.SelectedIndex = -1
        End If

        lSQl = "SELECT * FROM Tbl_Weather"
        BindingTable(lSQl, mCnn, mDsBT, "Tbl_Weather")

        lSQl = "SELECT * FROM Tbl_Nazer"
        BindingTable(lSQl, mCnn, mDs, "Tbl_Nazer")

        'lSQl = "SELECT * FROM Tbl_MPPost WHERE IsActive = 1"
        'BindingTable(lSQl, mCnn, mDsBT, "Tbl_MPPost")

        'lSQl = "SELECT * FROM Tbl_MPFeeder WHERE IsActive = 1"
        'BindingTable(lSQl, mCnn, mDsBT, "Tbl_MPFeeder")

        lSQl = "SELECT * FROM Tbl_MPCloserType"
        BindingTable(lSQl, mCnn, mDsBT, "Tbl_MPCloserType")

        lSQl = "SELECT * FROM Tbl_ManoeuvreType"
        BindingTable(lSQl, mCnn, mDsBT, "Tbl_ManoeuvreType")

        lSQl = "SELECT * FROM Tbl_TamirOperation"
        BindingTable(lSQl, mCnn, mDs, "Tbl_TamirOperation")

        lSQl = "SELECT * FROM Tbl_TamirRequestState"
        BindingTable(lSQl, mCnn, mDs, "Tbl_TamirRequestState")

        'lSQl = _
        '    " SELECT Tbl_AreaUser.* FROM Tbl_AreaUser LEFT OUTER JOIN Tbl_AreaUserApplication " & _
        '    " ON Tbl_AreaUser.AreaUserId = Tbl_AreaUserApplication.AreaUserId " & _
        '    " WHERE ApplicationId = " & ApplicationTypes.TamirRequest
        lSQl = " SELECT * FROM Tbl_AreaUser"
        RemoveMoreSpaces(lSQl)
        BindingTable(lSQl, mCnn, mDsBT, "Tbl_AreaUser")

        cmbPeymankar.DisplayMember = "PeymankarName"
        cmbPeymankar.ValueMember = "PeymankarId"
        lSQl = "SELECT * FROM Tbl_Peymankar"
        BindingTable(lSQl, mCnn, mDs, "Tbl_Peymankar")
        dgOperations.RootTable.Columns("TamirOperationGroupId").ValueList.PopulateValueList(mDs.Tbl_Peymankar.Rows, "PeymankarId", "PeymankarName")
        cmbPeymankar.DataSource = mDs.Tbl_Peymankar

        Dim lWhereIsActive As String = ""
        If mTamirRequestId > -1 Then
            BindingTable("select * from TblTamirRequest where TamirRequestId = " & mTamirRequestId, mCnn, mDs, "TblTamirRequest", , , , , , , True)
            Dim lState As TamirRequestStates = mDs.Tables("TblTamirRequest").Rows(0)("TamirRequestStateId")
            If lState < mdlHashemi.TamirRequestStates.trs_Confirm Then
                lWhereIsActive = " where IsActive = 1 "
            End If
        Else
            lWhereIsActive = " where IsActive = 1 "
        End If

        lSQl = "SELECT * FROM Tbl_Peymankar " & lWhereIsActive
        BindingTable(lSQl, mCnn, mDs, "Tbl_Peymankar", aIsClearTable:=True)

        'Dim lNetworkTypeId As String = ""
        'If CheckUserTrustee("NewTamirMPRequest", 30) Or IsAdmin() Then
        '    lNetworkTypeId = TamirNetworkType.MP
        'End If
        'If CheckUserTrustee("NewTamirLPRequest", 30) Or IsAdmin() Then
        '    lNetworkTypeId &= "," & TamirNetworkType.LP
        'End If
        'If CheckUserTrustee("NewTamirFTRequest", 30) Or IsAdmin() Then
        '    lNetworkTypeId &= "," & TamirNetworkType.FT
        'End If
        'lNetworkTypeId = Regex.Replace(lNetworkTypeId, "^,", "")
        'If lNetworkTypeId <> "" Then
        '    lNetworkTypeId = " AND TamirNetworkTypeId IN (" & lNetworkTypeId & ")"
        'Else
        '    lNetworkTypeId = " AND TamirNetworkTypeId = -1 "
        'End If
        lSQl = "SELECT * FROM Tbl_TamirNetworkType WHERE IsActive = 1 "
        BindingTable(lSQl, mCnn, mDsBT, "Tbl_TamirNetworkType")

        lSQl = "SELECT * FROM Tbl_ReferTo"
        BindingTable(lSQl, mCnn, mDsBT, "Tbl_ReferTo")

        lSQl = "SELECT * FROM Tbl_Department"
        BindingTable(lSQl, mCnn, mDsBT, "Tbl_Department")

        lSQl = "SELECT * FROM Tbl_TamirType"
        BindingTable(lSQl, mCnn, mDsBT, "Tbl_TamirType")

        lSQl = "SELECT * FROM Tbl_TamirRequestSubject"
        BindingTable(lSQl, mCnn, mDsBT, "Tbl_TamirRequestSubject")

        cmbRequestUserName.DataSource = New DataView(mDsBT.Tbl_AreaUser)
        cmbConfirmUserName.DataSource = New DataView(mDsBT.Tbl_AreaUser)
        cmbConfirmSetadUsername.DataSource = cmbConfirmUserName.DataSource
        cmbConfirmNaheihUsername.DataSource = New DataView(mDsBT.Tbl_AreaUser)
        cmbConfirmCenterUsername.DataSource = New DataView(mDsBT.Tbl_AreaUser)
        cmbAllowUserName.DataSource = New DataView(mDsBT.Tbl_AreaUser)

        BindingTable("SELECT * FROM Tbl_MPCloserType", mCnn, mDs, "Tbl_MPCloserType")
        cmbMPCloserType.DataSource = mDs.Tables("Tbl_MPCloserType")
        cmbMPCloserType.SelectedIndex = -1
        cmbMPCloserType.SelectedIndex = -1

        cmbArea_SelectedIndexChanged(Nothing, Nothing)

        If mIsTamirPowerMonthly Then
            lblWeakPower1.Text = lblWeakPower1.Text.Replace(mWeakPowerName, "ماه")
            lblWeakPower2.Text = lblWeakPower2.Text.Replace(mWeakPowerName, "ماه")
            lblWeakPower3.Text = lblWeakPower3.Text.Replace(mWeakPowerName, "ماه")
            lblWeakPower4.Text = lblWeakPower4.Text.Replace(mWeakPowerName, "ماه")
            lblWeakPower5.Text = lblWeakPower5.Text.Replace(mWeakPowerName, "ماه")
            lblWeakPower6.Text = lblWeakPower6.Text.Replace(mWeakPowerName, "ماه")
            mWeakPowerName = "ماه"
        End If
    End Sub
    Private Sub SetControlsStates()

        Dim lState As TamirRequestStates = mEditingRow("TamirRequestStateId")

        'pnlNazer.Visible = (mTamirRequestId > -1)

        If _
            (
                lState >= TamirRequestStates.trs_Confirm And
                Not (mIsForAllow OrElse IsSpecialSetadUser())
            ) _
            OrElse
            (
                Not IsSetadMode And
                (
                    lState = TamirRequestStates.trs_WaitForSetad Or
                    (Not IsCenter And lState = mdlHashemi.TamirRequestStates.trs_WaitFor121) Or
                    (Not mIsForAllow And lState > TamirRequestStates.trs_WaitForSetad)
                )
            ) _
            OrElse
            (
                IsSetadMode And
                (mIsSetadConfirmPlanned Or (mIsForConfirm And Not mIsForWarmLineConfirm)) And lState = mdlHashemi.TamirRequestStates.trs_Confirm
            ) _
            OrElse
            (
                IsSetadMode And
                lState = mdlHashemi.TamirRequestStates.trs_WaitForSetad And mIsFogheToziUser
            ) Then
            btnSave.Enabled = False
            mIsBtnSaveLoadEnabled = False
        End If


        If _
            (mIsForConfirm Or mIsForAllow) _
            Or (IsSetadMode AndAlso lState = TamirRequestStates.trs_PreNew) _
            Or ((Not IsSetadMode) AndAlso (lState > TamirRequestStates.trs_PreNew And Not (IsCenter And lState = mdlHashemi.TamirRequestStates.trs_WaitFor121 And Not chkIsEmergency.Checked))) _
            Or (lState = mdlHashemi.TamirRequestStates.trs_PreNew And chkIsEmergency.Checked) _
        Then
            If IsSetadMode And lState = mdlHashemi.TamirRequestStates.trs_PreNew _
                And Not IIf(Not IsDBNull(mEditingRow("IsReturned")), mEditingRow("IsReturned"), False) Then

                'If Not mIsFogheToziUser Then mIsSetadConfirmPlanned = True
                If mIsFogheToziUser Then chkIsSendToSetad.Checked = True
                chkIsSendToSetad.Visible = False
                cmbTamirType.SelectedValue = TamirTypes.BarnamehRiziShodeh
                chkIsEmergency.Enabled = False

                If mIsFogheToziUser Then
                    rbIsRequestByFogheTozi.Checked = True
                    pnlRequestedBy.Enabled = False
                End If
                cmbTamirNetworkType.SelectedValue = TamirNetworkType.FT
                mRowD("Dummy10Id") = TamirNetworkType.FT
                mRowD("Dummy12Id") = TamirTypes.BarnamehRiziShodeh
                'If Not mIsFogheToziUser Then rbIsConfirm.Checked = True

                If mIsFogheToziUser Then
                    chkIsSendToSetad.Visible = True
                    chkIsSendToSetad.Text = "ارسال به شرکت توزيع"
                End If
            Else
                If _
                (Not (IsSetadMode AndAlso lState = mdlHashemi.TamirRequestStates.trs_WaitForSetad) And Not mIsFogheToziUser) _
                Or mIsForConfirm Or mIsForAllow _
                Then
                    pnlTamirRequestInfo.Enabled = False

                    'pnlAdsress.Enabled = False
                    txtCriticalLocations.ReadOnly = True
                    txtAsdresses.ReadOnly = True
                    txtWorkingAddress.ReadOnly = True
                    rbInCityService.Enabled = False
                    rbNotInCityService.Enabled = False


                    cmbManoeuvreType.Enabled = False
                    txtManoeuvreDesc.ReadOnly = True
                End If
                If (
                    IsSetadMode And
                    lState = mdlHashemi.TamirRequestStates.trs_WaitForSetad _
                    And mIsFogheToziUser
                ) Then
                    pnlTamirRequestInfo.Enabled = False

                    'pnlAdsress.Enabled = False
                    txtCriticalLocations.ReadOnly = True
                    txtAsdresses.ReadOnly = True
                    txtWorkingAddress.ReadOnly = True
                    rbInCityService.Enabled = False
                    rbNotInCityService.Enabled = False


                    cmbManoeuvreType.Enabled = False
                    txtManoeuvreDesc.ReadOnly = True
                    chkIsSendToSetad.Visible = False
                End If

                chkIsManoeuvre.Enabled = False
                pnlAreaCity.Enabled = False
                pnlInfo.Enabled = False
                txtLetterDate.Enabled = False
                txtLetterNo.Enabled = False
                txtOperationDesc.Enabled = False
                pnlConfirmInfo.Enabled = False

                'pnlPostFeeder.Enabled = False
                cmbTamirNetworkType.Enabled = False
                cmbMPPost.Enabled = False
                cmbMPFeeder.Enabled = False
                cmbLPPost.Enabled = False
                cmbFeederPart.Enabled = False
                picFeederPart.Enabled = False
                cmbLPFeeder.Enabled = False
                btnLPFeeder.Enabled = False
                cmbCloserType.Enabled = False
                chkIsLPPostFeederPart.Enabled = False
                pnlLPPostFeederPart.Enabled = False
                txtGpsX.ReadOnly = True
                txtGpsY.ReadOnly = True
                chkIsRemoteDCChange.Enabled = False
                If Not (IsSetadMode And lState = mdlHashemi.TamirRequestStates.trs_WaitForSetad) Then
                    SetMultiStep_Enabled(False)
                    btnOk.Visible = False
                End If
                'btnMultiStep.Enabled = False
                '-----------------------------
                If mIsForConfirm Then
                    dgOperations.RootTable.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False
                Else
                    dgOperations.RootTable.Columns("IsSelected").EditType = Janus.Windows.GridEX.EditType.NoEdit
                End If
            End If

            If IsSetadMode And lState = mdlHashemi.TamirRequestStates.trs_PreNew And
                            IIf(Not IsDBNull(mEditingRow("IsReturned")), mEditingRow("IsReturned"), False) Then
                btnSave.Enabled = False
            End If

        End If
        If lState = mdlHashemi.TamirRequestStates.trs_PreNew Or chkIsEmergency.Checked Then
            rbIsReturned.Enabled = False
            rbIsReturnedWL.Enabled = False
        End If
        If (mIsForConfirm And Not mIsForWarmLineConfirm) And lState <= TamirRequestStates.trs_Confirm Then
            Dim lIsAccessFeederPartRequest As Boolean = CheckUserTrustee("ConfirmFeederPart", 30, ApplicationTypes.TamirRequest)
            Dim lIsAccessLPPostRequest As Boolean = CheckUserTrustee("ConfirmLPPost", 30, ApplicationTypes.TamirRequest)
            Dim lIsAccessLPRequest As Boolean = CheckUserTrustee("ConfirmLPRequest", 30, ApplicationTypes.TamirRequest)

            pnlAllow.Enabled = False
            pnlConfirm.Enabled = True
            chkIsSendToSetad.Visible = False
            If IsSetadMode Then
                If _
                    lState <> mdlHashemi.TamirRequestStates.trs_PreNew And
                        (Not mIsWarmLine Or Not mIsConfirmEndWarmLine) _
                Then
                    rbIsConfirm.Enabled = False
                    If lState <> mdlHashemi.TamirRequestStates.trs_WaitForSetad And
                        lState <> mdlHashemi.TamirRequestStates.trs_PreNew Then
                        pnlConfirm.Enabled = False
                        btnSave.Enabled = False
                    ElseIf (lState = mdlHashemi.TamirRequestStates.trs_PreNew Or
                            lState = mdlHashemi.TamirRequestStates.trs_WaitForSetad) Then
                        rbIsConfirm.Enabled = True
                    End If

                End If
                If lState = mdlHashemi.TamirRequestStates.trs_PreNew Or
                    lState = mdlHashemi.TamirRequestStates.trs_WaitForSetad Then
                    If cmbTamirNetworkType.SelectedValue = TamirNetworkType.LP Then
                        If Not mIsWarmLine Or Not mIsConfirmEndWarmLine Then
                            If Not IsDBNull(mEditingRow("FeederPartId")) Or Not IsDBNull(mEditingRow("LPPostId")) Or Not IsDBNull(mEditingRow("LPFeederId")) Then
                                If Not lIsAccessLPRequest Then
                                    rbIsConfirm.Enabled = False
                                    errpro.SetError(btnSave, "شما دسترسي به تأييد درخواست خاموشي فشار ضعيف را نداريد")
                                End If
                            End If
                        End If
                    Else
                        If Not mIsWarmLine Or Not mIsConfirmEndWarmLine Then
                            If Not IsDBNull(mEditingRow("FeederPartId")) Then
                                If Not lIsAccessFeederPartRequest Then
                                    rbIsConfirm.Enabled = False
                                    errpro.SetError(btnSave, "شما دسترسي به تأييد درخواست خاموشي تکه فيدر را نداريد")
                                End If
                            ElseIf Not IsDBNull(mEditingRow("LPPostId")) Then
                                If Not lIsAccessLPPostRequest Then
                                    rbIsConfirm.Enabled = False
                                    errpro.SetError(btnSave, "شما دسترسي به تأييد درخواست خاموشي پست توزيع را نداريد")
                                End If
                            End If
                        End If
                    End If
                End If
            ElseIf IsCenter And Not IsSetadMode Then
                If _
                    (lState <> mdlHashemi.TamirRequestStates.trs_PreNew OrElse
                    (IsDBNull(mEditingRow("FeederPartId")) _
                    And IsDBNull(mEditingRow("LPPostId")) _
                    And IsDBNull(mEditingRow("LPFeederId")))) And
                        (Not mIsWarmLine Or Not mIsConfirmEndWarmLine) _
                Then
                    rbIsConfirm.Enabled = False
                    If lState <> mdlHashemi.TamirRequestStates.trs_WaitFor121 And
                        lState <> mdlHashemi.TamirRequestStates.trs_PreNew Then
                        pnlConfirm.Enabled = False
                        btnSave.Enabled = False
                    ElseIf (lState = mdlHashemi.TamirRequestStates.trs_PreNew Or
                            lState = mdlHashemi.TamirRequestStates.trs_WaitFor121) And
                            (Not IsDBNull(mEditingRow("FeederPartId")) _
                            Or Not IsDBNull(mEditingRow("LPPostId")) _
                            Or Not IsDBNull(mEditingRow("LPFeederId"))) _
                    Then
                        rbIsConfirm.Enabled = True
                    End If

                End If
                If lState = mdlHashemi.TamirRequestStates.trs_PreNew Or
                    lState = mdlHashemi.TamirRequestStates.trs_WaitFor121 Then
                    If cmbTamirNetworkType.SelectedValue = TamirNetworkType.LP Then
                        If Not mIsWarmLine Or Not mIsConfirmEndWarmLine Then
                            If Not IsDBNull(mEditingRow("FeederPartId")) Or Not IsDBNull(mEditingRow("LPPostId")) Or Not IsDBNull(mEditingRow("LPFeederId")) Then
                                If Not lIsAccessLPRequest Then
                                    rbIsConfirm.Enabled = False
                                    errpro.SetError(btnSave, "شما دسترسي به تأييد درخواست خاموشي فشار ضعيف را نداريد")
                                End If
                            End If
                        End If
                    Else
                        If Not mIsWarmLine Or Not mIsConfirmEndWarmLine Then
                            If Not IsDBNull(mEditingRow("FeederPartId")) Then
                                If Not lIsAccessFeederPartRequest Then
                                    rbIsConfirm.Enabled = False
                                    errpro.SetError(btnSave, "شما دسترسي به تأييد درخواست خاموشي تکه فيدر را نداريد")
                                End If
                            ElseIf Not IsDBNull(mEditingRow("LPPostId")) Then
                                If Not lIsAccessLPPostRequest Then
                                    rbIsConfirm.Enabled = False
                                    errpro.SetError(btnSave, "شما دسترسي به تأييد درخواست خاموشي پست توزيع را نداريد")
                                End If
                            End If
                        End If
                    End If
                End If
            ElseIf Not IsCenter And Not IsSetadMode Then
                If _
                    (lState <> mdlHashemi.TamirRequestStates.trs_PreNew OrElse
                    (IsDBNull(mEditingRow("FeederPartId")) And
                    IsDBNull(mEditingRow("LPPostId")) And
                    IsDBNull(mEditingRow("LPFeederId")))) And
                        (Not mIsWarmLine Or Not mIsConfirmEndWarmLine) _
                Then
                    rbIsConfirm.Enabled = False
                    If lState <> mdlHashemi.TamirRequestStates.trs_PreNew Then
                        pnlConfirm.Enabled = False
                        btnSave.Enabled = False
                    End If
                End If
                If lState = mdlHashemi.TamirRequestStates.trs_PreNew Then
                    If cmbTamirNetworkType.SelectedValue = TamirNetworkType.LP Then
                        If Not mIsWarmLine Or Not mIsConfirmEndWarmLine Then
                            If Not IsDBNull(mEditingRow("FeederPartId")) Or Not IsDBNull(mEditingRow("LPPostId")) Or Not IsDBNull(mEditingRow("LPFeederId")) Then
                                If Not lIsAccessLPRequest Then
                                    rbIsConfirm.Enabled = False
                                    errpro.SetError(btnSave, "شما دسترسي به تأييد درخواست خاموشي فشار ضعيف را نداريد")
                                End If
                            End If
                        End If

                    Else
                        If Not mIsWarmLine Or Not mIsConfirmEndWarmLine Then
                            If Not IsDBNull(mEditingRow("FeederPartId")) Then
                                If Not lIsAccessFeederPartRequest Then
                                    rbIsConfirm.Enabled = False
                                    errpro.SetError(btnSave, "شما دسترسي به تأييد درخواست خاموشي تکه فيدر را نداريد")
                                ElseIf Not IsDBNull(mEditingRow("LPPostId")) Then
                                    If Not lIsAccessLPPostRequest Then
                                        rbIsConfirm.Enabled = False
                                        errpro.SetError(btnSave, "شما دسترسي به تأييد درخواست خاموشي پست توزيع را نداريد")
                                    End If
                                End If
                            End If
                        End If

                    End If
                End If
            End If
        End If

        'If IsSetadMode And Not (mIsForConfirm Or mIsForAllow) And lState <> mdlHashemi.TamirRequestStates.trs_WaitForSetad Then
        '    btnSave.Enabled = False
        'End If

        If mIsForAllow Then
            If lState < TamirRequestStates.trs_Finish Or (lState = TamirRequestStates.trs_Finish AndAlso IsSpecialSetadUser()) Then
                pnlAllow.Enabled = True
                pnlConfirm.Enabled = False
            End If
        End If

        If mIsForConfirm Or mIsForAllow Then
            tbcMain.SelectedTab = tp4_Confirms
            Dim lRows() As DataRow = mDsBT.Tables("Tbl_TamirNetworkType").Select()

        End If

        Dim lIsAccessEmergency As Boolean = CheckUserTrustee("CanSaveEmergency", 30, ApplicationTypes.TamirRequest)
        Dim lIsAccessTaeen As Boolean
        Dim lIsAccessTaeed As Boolean

        lIsAccessTaeen = CheckUserTrustee("ChangeNazer", 30, ApplicationTypes.TamirRequest) And Not IsAdministrator() 'And IsSetadMode
        lIsAccessTaeed = CheckUserTrustee("ConfirmNazer", 30, ApplicationTypes.TamirRequest) And Not IsAdministrator() 'And IsSetadMode
        pnlNazer.Enabled = pnlNazer.Enabled And (lIsAccessTaeen Or lIsAccessTaeed) And Not ((mIsForConfirm And Not mIsForWarmLineConfirm) Or mIsForAllow)
        btnSaveNazer.Visible = (lIsAccessTaeed Or lIsAccessTaeen) And (IsSetadMode Or mTamirRequestId > -1) And pnlNazer.Enabled
        cmbNazer.Enabled = lIsAccessTaeen
        chkIsConfirmNazer.Enabled = lIsAccessTaeed

        If lState >= mdlHashemi.TamirRequestStates.trs_Confirm Then
            chkIsConfirmNazer.Enabled = False
            btnSaveNazer.Enabled = False
        End If

        Dim lIsAccess As Boolean
        lIsAccess = CheckUserTrustee("TamirRequestConfirm", 30, ApplicationTypes.TamirRequest)
        If Not lIsAccessEmergency Or cmbTamirType.SelectedValue <> TamirTypes.Ezterari Then
            ' chkIsSendToSetad.Enabled = (lIsAccess AndAlso Not IsAdministrator())
        End If

        lIsAccess = CheckUserTrustee("CanSavePlannedTamirRequest", 30, ApplicationTypes.TamirRequest)
        rbIsRequestByFogheTozi.Enabled = lIsAccess
        If Not lIsAccess Then
            If cmbTamirType.SelectedValue <> TamirTypes.BarnamehRiziShodeh Then
                Dim lTT As TamirTypes = cmbTamirType.SelectedValue
                mDsBT.Tbl_TamirType.DefaultView.RowFilter = "TamirTypeId <> " & TamirTypes.BarnamehRiziShodeh
                cmbTamirType.SelectedValue = lTT
                DatasetDummy1.ViewDummyId.Rows(0)("Dummy12Id") = lTT
            Else
                cmbTamirType.Enabled = lIsAccess
            End If

            mIsLoading = True
            Dim lTNT As Integer = cmbTamirNetworkType.SelectedValue
            If cmbTamirNetworkType.SelectedValue <> TamirNetworkType.FT Then
                mDsBT.Tbl_TamirNetworkType.DefaultView.RowFilter = "TamirNetworkTypeId <> " & TamirNetworkType.FT
            Else
                cmbTamirNetworkType.Enabled = False
            End If
            cmbTamirNetworkType.SelectedValue = lTNT
            mIsLoading = False
        End If


        If Not lIsAccessEmergency Then
            If cmbTamirType.SelectedValue <> TamirTypes.Ezterari And Not chkIsEmergency.Checked Then
                Dim lTT As TamirTypes = cmbTamirType.SelectedValue
                Dim lRowFilter As String = mDsBT.Tbl_TamirType.DefaultView.RowFilter
                mDsBT.Tbl_TamirType.DefaultView.RowFilter &= IIf(lRowFilter <> "", " AND ", "") & "TamirTypeId <> " & TamirTypes.Ezterari
                cmbTamirType.SelectedValue = lTT
                DatasetDummy1.ViewDummyId.Rows(0)("Dummy12Id") = lTT
            End If
        End If

        Dim lNow As CTimeInfo = GetServerTimeInfo()
        If mIsForWarmLineConfirm Then
            pnlConfirm.Enabled = False
            pnlWarmLineConfirm.Enabled = True
            chkIsConfirmNazer.Enabled = False
            chkIsSendToSetad.Enabled = False
            cmbTamirNetworkType.Enabled = False
            pnlTamirType.Enabled = False
            pnlTamirRequestSubject.Enabled = False
            pnlNazer.Enabled = False
            pnlWorkCommandNo.Enabled = False
            Panel4.Enabled = False
            chkIsEmergency.Enabled = False
        End If

        If IsForceWarmLineConfirm() Then
            If (Not IsDBNull(mEditingRow("WarmLineConfirmStateId")) AndAlso
                mEditingRow("WarmLineConfirmStateId") > WarmLineConfirmState.wlcs_WaitForConfirm) Or
                lState > mdlHashemi.TamirRequestStates.trs_PreNew Then
                chkIsWarmLine.Enabled = False
            End If
        End If

    End Sub
    Private Sub ComputeSumTime()

        Try

            Dim lValue1 As Object, lValue2 As Object, lValue3 As Object
            Dim lSumTime As Integer = 0, lTotalTime As Integer = 0, lPeoplesCount As Integer = 1
            Dim lRow As DataRow
            Dim lGRow As Janus.Windows.GridEX.GridEXRow
            Dim lRows() As DataRow
            Dim lOPId As Integer

            Dim lGroups As New SortedList
            Dim lGroupId As Object

            mOperationCount = 0
            For Each lRow In mDsView.ViewTamirOperationList.Rows

                lGRow = dgOperations.GetRow(lRow)

                If lGRow.Cells("IsSelected").Value = True Then
                    mOperationCount += 1

                    lOPId = lGRow.Cells("TamirOperationId").Value

                    Try
                        lValue1 = IIf(
                            dgOperations(dgOperations.CurrentRowIndex, "TamirOperationId") = lOPId _
                            AndAlso dgOperations.CurrentColumn.Key = "Duration",
                            dgOperations.CurrentCellValue, lGRow.Cells("Duration").Value)
                    Catch ex As Exception
                        lValue1 = lGRow.Cells("Duration").Value
                    End Try

                    Try
                        lValue2 = IIf(
                            dgOperations(dgOperations.CurrentRowIndex, "TamirOperationId") = lOPId _
                            AndAlso dgOperations.CurrentColumn.Key = "OperationCount",
                            dgOperations.CurrentCellValue, lGRow.Cells("OperationCount").Value)
                    Catch ex As Exception
                        lValue2 = lGRow.Cells("OperationCount").Value
                    End Try

                    Try
                        lValue3 = IIf(
                            dgOperations(dgOperations.CurrentRowIndex, "TamirOperationId") = lOPId _
                            AndAlso dgOperations.CurrentColumn.Key = "PeoplesCount",
                            dgOperations.CurrentCellValue, lGRow.Cells("PeoplesCount").Value)
                    Catch ex As Exception
                        lValue3 = lGRow.Cells("PeoplesCount").Value
                    End Try

                    Try
                        lGroupId = IIf(
                            dgOperations(dgOperations.CurrentRowIndex, "TamirOperationId") = lOPId _
                            AndAlso dgOperations.CurrentColumn.Key = "TamirOperationGroupId",
                            dgOperations.CurrentCellValue, lGRow.Cells("TamirOperationGroupId").Value)
                    Catch ex As Exception
                        lGroupId = lGRow.Cells("TamirOperationGroupId").Value
                    End Try

                    If IsNothing(lGroupId) OrElse IsDBNull(lGroupId) OrElse Not IsNumeric(lGroupId) Then
                        lGroupId = -1
                    End If

                    If Not lGroups.Contains(lGroupId) Then
                        lGroups.Add(lGroupId, 0)
                    End If

                    If _
                        (Not IsNothing(lValue1) AndAlso Not IsDBNull(lValue1) AndAlso IsNumeric(lValue1)) _
                        AndAlso
                        (Not IsNothing(lValue2) AndAlso Not IsDBNull(lValue2) AndAlso IsNumeric(lValue2)) _
                    Then
                        Try
                            lSumTime = Convert.ToInt32(lValue1) * Convert.ToInt32(lValue2)

                            If Not IsNothing(lValue3) AndAlso Not IsDBNull(lValue3) AndAlso IsNumeric(lValue3) Then
                                lPeoplesCount = Val(lValue3)
                            Else
                                lPeoplesCount = 1
                            End If

                            If lPeoplesCount > 1 Then lSumTime /= lPeoplesCount

                            lRows = mDsView.ViewTamirOperationList.Select("TamirOperationId = " & lOPId)
                            If lRows.Length > 0 Then
                                lRows(0)("TotalDuration") = lSumTime
                            End If

                            Try
                                lGRow.Cells("TotalDuration").Value = lSumTime
                            Catch ex As Exception
                            End Try

                            lGroups(lGroupId) += lSumTime
                        Catch ex As Exception
                        End Try
                    End If

                End If

            Next

            lTotalTime = 0
            For Each lSumTime In lGroups.Values
                If lTotalTime < lSumTime Then
                    lTotalTime = lSumTime
                End If
            Next
            If mIsForceTamirOperationTime Then
                txtSumTime.Text = lTotalTime
            Else
                txtSumTime.Text = "0"
            End If

        Catch ex As Exception
            ShowError(ex)
        End Try

    End Sub
    Private Sub ComputeDCInterval()
        If Not txtDateDisconnect.IsOK Or Not txtDateConnect.IsOK Or Not txtTimeDisconnect.IsOK Or Not txtTimeConnect.IsOK Then
            txtDCInterval.Text = ""
            Exit Sub
        End If

        Dim lDT1 As DateTime, lDT2 As DateTime
        Dim lT1 As DateTime, lT2 As DateTime
        Dim lH As Integer, lM As Integer

        lDT1 = txtDateDisconnect.MiladiDT
        lDT2 = txtDateConnect.MiladiDT

        lT1 = txtTimeDisconnect.GetFullDate
        lT2 = txtTimeConnect.GetFullDate

        lDT1 = lDT1.AddHours(-lDT1.Hour).AddHours(lT1.Hour)
        lDT1 = lDT1.AddMinutes(-lDT1.Minute).AddMinutes(lT1.Minute)

        lDT2 = lDT2.AddHours(-lDT2.Hour).AddHours(lT2.Hour)
        lDT2 = lDT2.AddMinutes(-lDT2.Minute).AddMinutes(lT2.Minute)

        txtDCInterval.Text = DateDiff(DateInterval.Minute, lDT1, lDT2)

        Dim lIsAccessConfirm As Boolean = CheckUserTrustee("TamirRequestConfirm", 30, ApplicationTypes.TamirRequest) AndAlso Not IsAdministrator()
        Dim lIsAccessEmergency As Boolean = CheckUserTrustee("CanSaveEmergency", 30, ApplicationTypes.TamirRequest)

        Dim lDT As DateTime = txtDateDisconnect.MiladiDT
        Dim lDTNow As DateTime = GetServerTimeInfo().MiladiDate
        Dim lDiff As Long = DateDiff(DateInterval.Minute, lDTNow, lDT)

        If (lIsAccessEmergency And Not lIsAccessConfirm) Then
            If cmbTamirType.SelectedValue = TamirTypes.Ezterari Then
                If lDiff > (48 * 60) Then
                    'chkIsSendToSetad.Enabled = False
                    chkIsSendToSetad.Checked = False
                Else
                    chkIsSendToSetad.Enabled = True
                End If
                If mIsLoadInfo Then Exit Sub
                mIsEmergencyEnabled = chkIsSendToSetad.Enabled
            End If
        End If
    End Sub
    Private Sub LoadInfo()
        Dim lSQL As String
        Dim lTRequestId As Long
        Dim lRow As DataRow
        Dim lRows() As DataRow

        Dim lNow As CTimeInfo = GetServerTimeInfo()

        Try

            mIsLoading = True
            mIsLoadInfo = True

            If Not IsCenter And Not IsMiscMode Then
                If Not mIsTamirDirectToSetad Then
                    chkIsSendToSetad.Text = "ارسال به مرکز"
                Else
                    chkIsSendToSetad.Text = "ارسال به ستاد"
                End If
            End If
            SearchAndFillNazer(True)
            cmbNazer.DisplayMember = "Nazername"
            cmbNazer.ValueMember = "NazerId"

            lSQL = "SELECT * FROM TblTamirRequest WHERE TamirRequestId = " & mTamirRequestId
            BindingTable(lSQL, mCnn, mDs, "TblTamirRequest")

            lSQL = "SELECT * FROM TblTamirRequestDisconnect WHERE TamirRequestId = " & mTamirRequestId
            BindingTable(lSQL, mCnn, mDs, "TblTamirRequestDisconnect")

            lSQL = "SELECT * FROM TblTamirRequestFTFeeder WHERE TamirRequestId = " & mTamirRequestId
            BindingTable(lSQL, mCnn, mDs, "TblTamirRequestFTFeeder")

            lSQL = "SELECT * FROM TblTamirRequestMultiStep WHERE TamirRequestId = " & mTamirRequestId & " ORDER BY MSDatePersian ASC, MSTime ASC"
            BindingTable(lSQL, mCnn, mDs, "TblTamirRequestMultiStep")

            lSQL = "SELECT * FROM TblTamirRequestAllow WHERE TamirRequestId = " & mTamirRequestId
            BindingTable(lSQL, mCnn, mDs, "TblTamirRequestAllow")

            lSQL = "SELECT * FROM TblTamirRequestConfirm WHERE TamirRequestId = " & mTamirRequestId
            BindingTable(lSQL, mCnn, mDs, "TblTamirRequestConfirm")

            lSQL = "SELECT * FROM TblTamirOperationList WHERE TamirRequestId = " & mTamirRequestId
            BindingTable(lSQL, mCnn, mDs, "TblTamirOperationList")

            lSQL = "SELECT * FROM TblDCOverlap WHERE TamirRequestId = " & mTamirRequestId
            BindingTable(lSQL, mCnn, mDs, "TblDCOverlap")

            lSQL =
                " SELECT Cast(0 As bit) As IsSelected, TblTamirOperationList.TamirOperationListId, TblTamirOperationList.TamirRequestId, " &
                " Tbl_TamirOperation.TamirOperationId, Tbl_TamirOperation.TamirOperation, " &
                " ISNULL(TblTamirOperationList.Duration,Tbl_TamirOperation.Duration) As Duration, TblTamirOperationList.OperationCount, Cast(NULL As Int) As TotalDuration " &
                " , TblTamirOperationList.TamirOperationGroupId, TblTamirOperationList.PeoplesCount, ISNULL(Tbl_TamirNetworkType.TamirNetworkType,'نا مشخص') AS TamirNetworkType , PeymankarId " &
                " FROM  Tbl_TamirOperation " &
                " LEFT JOIN Tbl_TamirNetworkType ON Tbl_TamirOperation.TamirNetworkTypeId = Tbl_TamirNetworkType.TamirNetworkTypeId " &
                " AND Tbl_TamirNetworkType.IsActive = 1 " &
                " LEFT JOIN TblTamirOperationList ON Tbl_TamirOperation.TamirOperationId = TblTamirOperationList.TamirOperationId " &
                " AND TblTamirOperationList.TamirRequestId = " & mTamirRequestId
            RemoveMoreSpaces(lSQL)
            BindingTable(lSQL, mCnn, mDsView, "ViewTamirOperationList")

            For Each lRow In mDsView.ViewTamirOperationList.Rows
                Dim lOpId As Integer = lRow("TamirOperationId")
                lRows = mDs.TblTamirOperationList.Select("TamirOperationId = " & lOpId)
                lRow("IsSelected") = (lRows.Length > 0)
                dgOperations.GetRow(lRow).IsChecked = (lRows.Length > 0)
            Next

            With DatasetDummy1.ViewDummyId
                mRowD = .NewViewDummyIdRow()
                .AddViewDummyIdRow(mRowD)
            End With

            Dim lIsCheckSendSMSSensitive As Boolean = False
            lIsCheckSendSMSSensitive = CConfig.ReadConfig("IsCheckSendSMSSensitive", False)
            chkIsSendSMSSensitive.Checked = lIsCheckSendSMSSensitive

            If mTamirRequestId > -1 AndAlso mDs.Tables("TblTamirRequest").Rows.Count > 0 Then
                SaveLog("TamirRequestId: " & mTamirRequestId, "TNT.log")
                Try
                    SaveLog("TamirRequestNo: " & mEditingRow("TamirRequestNo"), "TNT.log")
                Catch ex As Exception
                    SaveLog("TamirRequestNo: " & ex.Message, "TNT.log")
                End Try

                mIsNewRequest = False
                mIsAddRow = True
                mIsUpdateCurrentValue = False
                mEditingRow = mDs.Tables("TblTamirRequest").Rows(0)
                If mDs.TblTamirRequestDisconnect.Count > 0 Then
                    mEditingRowManovr = mDs.TblTamirRequestDisconnect(0)
                End If
                lTRequestId = mEditingRow("TamirRequestId")

                Dim lAUId As Integer = mEditingRow("AreaUserId")
                mIsFogheToziUserRequested = CheckUserTrustee(lAUId, "IsFoghetoziUser", 30, ApplicationTypes.TamirRequest)
                rbIsReturned.Visible = Not mIsFogheToziUserRequested

                mFirstDay_DT = mEditingRow("SaturdayDT")
                mFirstDay_PDate = GetFirstDatePersianOfTamirScope(mFirstDay_DT)

                If _
                    mEditingRow("TamirRequestStateId") = TamirRequestStates.trs_PreNew _
                    Or (Not IsSetadMode And IsCenter And mEditingRow("TamirRequestStateId") = TamirRequestStates.trs_WaitFor121) _
                Then
                    chkIsSendToSetad.Visible = True
                End If

                Try

                    lSQL =
                        " SELECT " &
                        "	TblTamirRequest.TamirRequestId, TblTamirRequest.TamirRequestNo, TblRequest.RequestNumber, " &
                        "	TblRequestInform.CreateDatePersian, TblRequestInform.SendSMSTypeId, TblRequestInform.RequestInformJobStateId, " &
                        "	Tbl_Subscriber.Name, Tbl_Subscriber.SubscriberId, TblSubscriberInfom.SendSMSStatusId, TblSubscriberInfom.SendSMSDT, Tbl_Subscriber.SubscriberSensitivityId " &
                        " FROM " &
                        "	TblTamirRequest " &
                        "	INNER JOIN TblRequest ON (TblTamirRequest.LastRequestId = TblRequest.RequestId OR TblTamirRequest.EmergencyRequestId = TblRequest.RequestId) " &
                        "	INNER JOIN TblRequestInform ON TblRequest.RequestId = TblRequestInform.RequestId " &
                        "	LEFT OUTER JOIN TblSubscriberInfom ON TblRequestInform.RequestInformId = TblSubscriberInfom.RequestInformId " &
                        "	LEFT OUTER JOIN Tbl_Subscriber ON TblSubscriberInfom.SubscriberId = Tbl_Subscriber.SubscriberId " &
                        " WHERE " &
                        "	TblSubscriberInfom.SendSMSStatusId = 2 " &
                        "	AND (TblRequestInform.SendSMSTypeId = 1 OR (TblRequestInform.SendSMSTypeId = 3 AND Tbl_Subscriber.SubscriberSensitivityId = 3)) " &
                        "	AND TblTamirRequest.TamirRequestId = " & mTamirRequestId
                    RemoveMoreSpaces(lSQL)
                    BindingTable(lSQL, mCnn, mDsView, "TblSMSCount")

                    If mDsView.Tables.Contains("TblSMSCount") AndAlso mDsView.Tables("TblSMSCount").Rows.Count > 0 Then
                        txtSMSCOunt.Text = mDsView.Tables("TblSMSCount").Rows.Count
                    End If

                Catch ex As Exception
                End Try

                Try

                    lSQL =
                        " SELECT " &
                        "	TblTamirRequest.TamirRequestId, " &
                        "	TblRequest.RequestNumber, " &
                        "	TblRequestInform.IsSend_IRIB " &
                        " FROM " &
                        "	TblTamirRequest " &
                        "	INNER JOIN TblRequest ON TblTamirRequest.LastRequestId = TblRequest.RequestId OR TblTamirRequest.EmergencyRequestId = TblRequest.RequestId " &
                        "	INNER JOIN TblRequestInform ON TblRequest.RequestId = TblRequestInform.RequestId " &
                        " WHERE " &
                        "	(TblRequestInform.IsSend_IRIB = 1) " &
                        "	AND TblTamirRequest.TamirRequestId = " & mTamirRequestId
                    RemoveMoreSpaces(lSQL)
                    BindingTable(lSQL, mCnn, mDsView, "TblIRIBCount")

                    If mDsView.Tables.Contains("TblIRIBCount") AndAlso mDsView.Tables("TblIRIBCount").Rows.Count > 0 Then
                        chkIsIRIB.Checked = True
                    End If

                Catch ex As Exception
                End Try

                If mDs.TblTamirRequestMultiStep.Rows.Count > 0 Then
                    mIsMultiStep = True
                    mLastMSDateTime = mDs.TblTamirRequestMultiStep.Rows(mDs.TblTamirRequestMultiStep.Count - 1)("MSDT")
                End If

                txtDateConnect.Enabled = Not mIsMultiStep
                txtTimeConnect.Enabled = Not mIsMultiStep

                BindingTable("SELECT * FROM TblTamirRequestKey WHERE TamirRequestId = " & mTamirRequestId, mCnn, DatasetTemp1, "TblTamirRequestKey", , , , , , , True)

                Try
                    Dim lDs As New DataSet
                    lSQL =
                        " SELECT " &
                        "	TblRequestInfo.* " &
                        " FROM " &
                        "	TblTamirRequest " &
                        "   LEFT JOIN TblTamirRequestConfirm ON TblTamirRequest.TamirRequestId = TblTamirRequestConfirm.TamirRequestId " &
                        "	INNER JOIN TblRequest ON ISNULL(TblTamirRequestConfirm.RequestId, TblTamirRequest.EmergencyRequestId) = TblRequest.RequestId " &
                        "	INNER JOIN TblRequestInfo ON TblRequest.RequestId = TblRequestInfo.RequestId " &
                        " WHERE " &
                        "	TblTamirRequest.TamirRequestId = " & mTamirRequestId
                    RemoveMoreSpaces(lSQL)
                    BindingTable(lSQL, mCnn, lDs, "TblRequestInfo")
                    If lDs.Tables.Contains("TblRequestInfo") AndAlso lDs.Tables("TblRequestInfo").Rows.Count > 0 Then
                        GetDBValue(chkIsSendToInformApp, lDs.Tables("TblRequestInfo")(0)("IsSendToInformApp"))
                    End If

                Catch ex As Exception
                End Try

            Else
                mIsUpdateCurrentValue = True

                mIsNewRequest = True
                mIsAddRow = False
                chkIsSendToSetad.Visible = True

                mEditingRow = mDs.Tables("TblTamirRequest").NewRow
                lTRequestId = GetAutoInc()
                mEditingRow("TamirRequestId") = lTRequestId
                mEditingRow("TamirRequestNo") = -1
                mEditingRow("AreaUserId") = WorkingUserId
                mEditingRow("DEDT") = lNow.MiladiDate
                mEditingRow("DEDatePersian") = lNow.ShamsiDate
                mEditingRow("DETime") = lNow.HourMin
                mEditingRow("IsManoeuvre") = False
                Dim lIsWarmLine As Boolean = False
                lIsWarmLine = CConfig.ReadConfig("IsWarmLineDefault", False)
                mEditingRow("IsWarmLine") = lIsWarmLine
                mEditingRow("IsWatched") = False
                mEditingRow("TamirRequestStateId") = TamirRequestStates.trs_PreNew
                mEditingRow("AreaId") = WorkingAreaId
                mEditingRow("CityId") = WorkingCityId
                mEditingRow("TamirNetworkTypeId") = TamirNetworkType.MP
                mEditingRow("IsNeedManovrDC") = DBNull.Value

                LogTamirNetworkType("Step 1")

                'If IsSetadMode And Not mIsFogheToziUser Then
                '    mIsSetadConfirmPlanned = True
                '    btnSave.Text = "ذخيره و تأييد"
                '    pnlConfirm.Enabled = True
                '    rbIsConfirm.Enabled = False
                '    rbIsNotConfirm.Enabled = False
                '    rbIsReturned.Enabled = False
                '    chkIsSendSMSSensitive.Enabled = True
                'End If

                BindingTable("SELECT * FROM TblTamirRequestKey WHERE TamirRequestId = -1 ", mCnn, DatasetTemp1, "TblTamirRequestKey", , , , , , , True)
            End If

            lSQL = "SELECT * FROM TblTamirRequestFile WHERE TamirRequestId = " & mTamirRequestId
            BindingTable(lSQL, mCnn, mDs, "TblTamirRequestFile")

            lSQL =
                    "SELECT Cast(0 AS bit) AS IsNewFile, TblTamirRequestFile.TamirRequestFileId, TblTamirRequestFile.TamirRequestId, TblTamirRequestFile.FileServerId, TblTamirRequestFile.Subject, TblTamirRequestFile.Comment, TblFileServer.FileId, TblFileServer.FileName, TblFileServer.FileSize, TblFileServer.FilePath, TblFileServer.AreaUserId, Tbl_AreaUser.UserName, Cast('â' AS varchar) AS Content " &
                    "FROM TblTamirRequestFile INNER JOIN TblFileServer ON TblTamirRequestFile.FileServerId = TblFileServer.FileServerId INNER JOIN Tbl_AreaUser ON TblFileServer.AreaUserId = Tbl_AreaUser.AreaUserId " &
                    "WHERE TblTamirRequestFile.TamirRequestId = " & mTamirRequestId
            BindingTable(lSQL, mCnn, mDs, "ViewTamirRequestFile")

            If IsNothing(mEditingRowManovr) Then
                mIsNewManovr = True
                mEditingRowManovr = mDs.TblTamirRequestDisconnect.NewRow
                Dim lTRDId As Long = GetAutoInc()
                mEditingRowManovr.TamirRequestDisconnectId = lTRDId
                mEditingRowManovr.TamirRequestId = lTRequestId
                mEditingRowManovr.AreaUserId = WorkingUserId
                mEditingRowManovr.DEDT = lNow.MiladiDate
                mEditingRowManovr.DEDatePersian = lNow.ShamsiDate
                mEditingRowManovr.DETime = lNow.HourMin
                mEditingRowManovr.DisconnectPower = 0
                mEditingRowManovr.IsManoeuvre = True
            Else
                mIsNewManovr = False
            End If

            mIsNewConfirm = False
            If mDs.TblTamirRequestConfirm.Rows.Count > 0 Then
                mEditingRowConfirm = mDs.TblTamirRequestConfirm.Rows(0)
                mIsAddRowConfirm = True
            Else
                mIsAddRowConfirm = False
                If (mIsForConfirm And Not mIsForWarmLineConfirm) Or mIsSetadConfirmPlanned Then
                    mIsNewConfirm = True
                    cmbConfirmUserName.SelectedValue = WorkingUserId
                    txtDateConfirm.MiladiDT = lNow.MiladiDate
                    txtTimeConfirm.MiladiDT = lNow.MiladiDate
                Else
                    cmbConfirmUserName.SelectedIndex = -1
                End If
            End If

            mIsNewAllow = False
            If mDs.TblTamirRequestAllow.Rows.Count > 0 Then
                mEditingRowAllow = mDs.TblTamirRequestAllow.Rows(0)
                mIsAddRowAllow = True
            Else
                mIsAddRowAllow = False
                If mIsForAllow Then
                    mIsNewAllow = True
                    cmbAllowUserName.SelectedValue = WorkingUserId
                    txtDateAllowDE.MiladiDT = lNow.MiladiDate
                    txtTimeAllowDE.MiladiDT = lNow.MiladiDate
                Else
                    cmbAllowUserName.SelectedIndex = -1
                End If
            End If

            '====================================
            'TblTamirRequest:
            lRow = mEditingRow
            GetDBValue(txtTamirRequestNumber, lRow("TamirRequestNo"), , -1)

            If IsCenter Or mTamirRequestId > -1 Then mIsLoading = False
            GetDBValue(cmbArea, lRow("AreaId"))
            GetDBValue(cmbCity, lRow("CityId"))
            GetDBValue(cmbMPPost, lRow("MPPostId"))
            GetDBValue(cmbMPFeeder, lRow("MPFeederId"))
            GetDBValue(cmbFeederPart, lRow("FeederPartId"))
            GetDBValue(cmbLPPost, lRow("LPPostId"))
            GetDBValue(cmbLPFeeder, lRow("LPFeederId"))
            GetDBValue(txtLetterNo, lRow("LetterNo"))
            GetDBValue(txtLetterDate, lRow("LetterDatePersian"))
            GetDBValue(txtOperationDesc, lRow("OperationDesc"))
            If IsDBNull(mEditingRow("IsNeedManovrDC")) Then
                chkIsNotNeedManovrDC.Checked = False
                lblRequestNumberManovr.Visible = False
                lnkRequestNumberManovr.Visible = False
            Else
                chkIsNotNeedManovrDC.Checked = Not mEditingRow("IsNeedManovrDC")
                lblRequestNumberManovr.Visible = Not chkIsNotNeedManovrDC.Checked
                lnkRequestNumberManovr.Visible = Not chkIsNotNeedManovrDC.Checked
            End If

            mIsLoading = True

            GetDBValue(cmbPeymankar, lRow("PeymankarId"))
            If cmbPeymankar.SelectedIndex = -1 AndAlso Not IsDBNull(lRow("Peymankar")) Then
                cmbPeymankar.Text = lRow("Peymankar")
                mPeymankarText = lRow("Peymankar")
            End If

            If Not IsDBNull(lRow("TamirNetworkTypeId")) AndAlso lRow("TamirNetworkTypeId") = TamirNetworkType.FT AndAlso IsDBNull(lRow("IsRequestByPeymankar")) Then
                rbIsRequestByPeymankar.Checked = False
                rbIsRequestByFogheTozi.Checked = True
            Else
                GetDBValue(rbIsRequestByPeymankar, lRow("IsRequestByPeymankar"))
            End If
            rbIsRequestByTozi.Checked = Not IsDBNull(lRow("IsRequestByPeymankar")) AndAlso Not rbIsRequestByPeymankar.Checked

            GetDBValue(cmbReferTo, lRow("ReferToId"))
            GetDBValue(cmbDepartment, lRow("DepartmentId"))
            SearchAndFillNazer(True)

            GetDBValue(cmbCloserType, lRow("MPCloserTypeId"))
            GetDBValue(txtDateDisconnect, lRow("DisconnectDT"))
            GetDBValue(txtTimeDisconnect, lRow("DisconnectDT"))
            GetDBValue(txtDateConnect, lRow("ConnectDT"))
            GetDBValue(txtTimeConnect, lRow("ConnectDT"))
            GetDBValue(txtDCInterval, lRow("DisconnectInterval"))
            GetDBValue(txtLastPeakBar, lRow("LastMPFeederPeak"))
            GetDBValue(txtCurrentValue, lRow("CurrentValue"))
            GetDBValue(txtCriticalLocations, lRow("CriticalLocations"))
            GetDBValue(txtAsdresses, lRow("CriticalsAddress"))
            GetDBValue(cmbRequestUserName, lRow("AreaUserId"))
            GetDBValue(chkIsManoeuvre, lRow("IsManoeuvre"))
            GetDBValue(txtManoeuvreDesc, lRow("ManoeuvreDesc"))
            GetDBValue(cmbManoeuvreType, lRow("ManoeuvreTypeId"))
            GetDBValue(cmbWeather, lRow("WeatherId"))
            GetDBValue(chkIsWarmLine, lRow("IsWarmLine"))
            'GetDBValue(txtMaxPower, lRow("MaximumPower"))
            GetDBValue(cmbState, lRow("TamirRequestStateId"))
            GetDBValue(txtWorkingAddress, lRow("WorkingAddress"))
            GetDBValue(cmbNazer, lRow("NazerId"))
            GetDBValue(chkIsConfirmNazer, lRow("IsConfirmNazer"))
            GetDBValue(cmbConfirmNaheihUsername, lRow("ConfirmNahiehAreaUserId"))
            GetDBValue(cmbConfirmCenterUsername, lRow("ConfirmCenterAreaUserId"))
            GetDBValue(chkIsEmergency, lRow("IsEmergency"))
            If chkIsEmergency.Checked Then
                cmbTamirType.Enabled = False
            End If

            GetDBValue(txtDTRequest, lRow("DEDT"))
            GetDBValue(txtDTSendToCenter, lRow("SendCenterDT"))
            GetDBValue(txtDTSendToSetad, lRow("SendSetadDT"))

            GetDBValue(txtTMRequest, lRow("DEDT"))
            GetDBValue(txtTMSendToCenter, lRow("SendCenterDT"))
            GetDBValue(txtTMSendToSetad, lRow("SendSetadDT"))

            GetDBValue(txtGpsX, lRow("GpsX"))
            GetDBValue(txtGpsY, lRow("GpsY"))

            GetDBValue(txtPeymankarTel, lRow("PeymankarMobileNo"))
            If txtPeymankarTel.Text <> "" Then
                GlobalToolTip.SetToolTip(lblPeymankar, "شماره موبايل جهت ارسال پيامک : " & txtPeymankarTel.Text)
            End If

            GetDBValue(txtWorkCommandNo, lRow("WorkCommandNo"))
            GetDBValue(txtGISFormNo, lRow("GISFormNo"))
            GetDBValue(txtProjectNo, lRow("ProjectNo"))
            GetDBValue(rbInCityService, lRow("IsInCityService"))
            rbNotInCityService.Checked = Not IsDBNull(lRow("IsInCityService")) AndAlso Not rbInCityService.Checked

            mTamirNetworkTypeId = lRow("TamirNetworkTypeId")
            GetDBValue(cmbTamirNetworkType, lRow("TamirNetworkTypeId"))
            CheckNetworkType(True)
            LogTamirNetworkType("Step 2")

            GetDBValue(cmbTamirType, lRow("TamirTypeId"))
            GetDBValue(txtDCCurrent, lRow("DCCurrentValue"))
            GetDBValue(cmbTamirRequestSubject, lRow("TamirRequestSubjectId"))

            GetDBValue(chkIsRemoteDCChange, lRow("IsRemoteDCChange"))

            If Not mIsForConfirm Then
                GetDBValue(rbIsReturned, mEditingRow("IsReturned"))
                'If rbIsReturned.Checked Then
                GetDBValue(txtUnConfirmReason, mEditingRow("ReturnDesc"))
                'End If
            End If

            If lblPowerUnit.Text.ToLower = "kwh" Then
                GetDBValue(txtPower, lRow("DisconnectPower") * 1000)
            Else
                GetDBValue(txtPower, lRow("DisconnectPower"))
            End If

            mOverlapTime = IIf(Not IsDBNull(mEditingRow("OverlapTime")), mEditingRow("OverlapTime"), 0)
            mDCPowerECO = IIf(Not IsDBNull(mEditingRow("DisconnectPowerECO")), mEditingRow("DisconnectPowerECO"), 0)
            If Not IsDBNull(lRow("DisconnectInterval")) Then
                mRealDCTime = Val(lRow("DisconnectInterval")) - mOverlapTime
            End If

            If Not lRow("EmergencyRequestId") Is DBNull.Value Then

                GetDBValue(txtEmergencyReason, lRow("EmergencyReason"))
                If Not IsDBNull(lRow("EmergencyReason")) Then
                    txtEmergencyReasonDisplay.Text = "علت ثبت بدون تأييد: " & vbCrLf & lRow("EmergencyReason")
                    txtEmergencyReasonDisplay.Visible = True
                End If

                lnkRequestNumber.Tag = lRow("EmergencyRequestId")
                lSQL = "SELECT RequestNumber, EndJobStateId, MPRequestId FROM TblRequest WHERE RequestId = " & lRow("EmergencyRequestId")
                ClearTable(mDs, "TblRequestNumber")
                BindingTable(lSQL, mCnn, mDs, "TblRequestNumber")
                If Not mDs.Tables("TblRequestNumber") Is Nothing AndAlso mDs.Tables("TblRequestNumber").Rows.Count > 0 Then
                    lnkRequestNumber.Text = mDs.Tables("TblRequestNumber").Rows(0)("RequestNumber")
                    mEmergency_EndJobStateId = mDs.Tables("TblRequestNumber").Rows(0)("EndJobStateId")
                    mEmergency_MPRequestId = mDs.Tables("TblRequestNumber").Rows(0)("MPRequestId")
                End If
                chkIsEmergency.Enabled = False

            ElseIf IsDBNull(lRow("EmergencyRequestId")) And Not IsDBNull(mEditingRow("TamirTypeId")) AndAlso mEditingRow("TamirTypeId") = TamirTypes.Ezterari Then
                pnlEmergencyReason.Visible = True
                GetDBValue(txtEmergency, lRow("EmergencyReason"))
                If lRow("TamirRequestStateId") > TamirRequestStates.trs_PreNew Then
                    chkIsEmergency.Enabled = False
                    txtEmergency.ReadOnly = True
                End If
            ElseIf lRow("TamirRequestStateId") > TamirRequestStates.trs_PreNew Then
                chkIsEmergency.Enabled = False
            End If

            If txtCriticalLocations.Text.Trim.Length > 0 Then
                mIsManualChangeCL = True
            End If

            '-------------------
            'TblTamirRequestDisconnect:
            If Not mEditingRowManovr Is Nothing Then
                GetDBValue(txtDateDisconnectManovr, mEditingRowManovr("DisconnectDT"), , True)
                GetDBValue(txtDateDisconnectManovr, mEditingRowManovr("DisconnectDatePersian"))
                GetDBValue(txtTimeDisconnectManovr, mEditingRowManovr("DisconnectTime"))
                GetDBValue(txtDateConnectManovr, mEditingRowManovr("ConnectDT"), , True)
                GetDBValue(txtDateConnectManovr, mEditingRowManovr("ConnectDatePersian"))
                GetDBValue(txtTimeConnectManovr, mEditingRowManovr("ConnectTime"))

                GetDBValue(txtDCIntervalManovr, mEditingRowManovr("DisconnectInterval"))
                GetDBValue(txtCurrentValueManovr, mEditingRowManovr("CurrentValue"))
                If lblPowerUnitManovr.Text.ToLower = "kwh" Then
                    GetDBValue(txtPowerManovr, mEditingRowManovr("DisconnectPower") * 1000)
                Else
                    GetDBValue(txtPowerManovr, mEditingRowManovr("DisconnectPower"))
                End If

                If Not mEditingRowManovr("RequestId") Is DBNull.Value Then
                    lnkRequestNumberManovr.Tag = mEditingRowManovr("RequestId")
                    lSQL = "SELECT RequestNumber FROM TblRequest WHERE RequestId = " & mEditingRowManovr("RequestId")
                    BindingTable(lSQL, mCnn, mDs, "TblRequestNumber", , , , , , , True)
                    If Not mDs.Tables("TblRequestNumber") Is Nothing AndAlso mDs.Tables("TblRequestNumber").Rows.Count > 0 Then
                        lnkRequestNumberManovr.Text = mDs.Tables("TblRequestNumber").Rows(0)("RequestNumber")
                    End If
                End If
            End If

            '-------------------
            'TblTamirRequestConfirm:
            lRow = mEditingRowConfirm
            If Not mEditingRowConfirm Is Nothing Then
                If Not lRow("IsConfirm") Is DBNull.Value Then
                    GetDBValue(rbIsConfirm, lRow("IsConfirm"))
                    GetDBValue(rbIsNotConfirm, Not lRow("IsConfirm"))
                ElseIf Not rbIsReturned.Checked Then
                    rbIsConfirm.Checked = False
                    rbIsNotConfirm.Checked = False
                    txtUnConfirmReason.Enabled = False
                End If
                GetDBValue(txtDateConfirm, lRow("ConfirmDT"))
                GetDBValue(txtTimeConfirm, lRow("ConfirmDT"))
                GetDBValue(txtUnConfirmReason, lRow("UnConfirmReason"))
                GetDBValue(cmbConfirmUserName, lRow("AreaUserId"))
                GetDBValue(chkIsSendSMSSensitive, lRow("IsSendSMSSensitive"))
                If Not lRow("RequestId") Is DBNull.Value Then
                    lnkRequestNumber.Tag = lRow("RequestId")
                    mRequestId = lRow("RequestId")
                    lSQL = "SELECT RequestNumber FROM TblRequest WHERE RequestId = " & lRow("RequestId")
                    ClearTable(mDs, "TblRequestNumber")
                    BindingTable(lSQL, mCnn, mDs, "TblRequestNumber")
                    If Not mDs.Tables("TblRequestNumber") Is Nothing AndAlso mDs.Tables("TblRequestNumber").Rows.Count > 0 Then
                        lnkRequestNumber.Text = mDs.Tables("TblRequestNumber").Rows(0)("RequestNumber")
                    End If
                End If

                GetDBValue(txtDTConfirm, lRow("ConfirmDT"))
                GetDBValue(txtTMConfirm, lRow("ConfirmDT"))
            End If

            '-------------------
            'TblTamirRequestAllow:
            lRow = mEditingRowAllow
            If Not mEditingRowAllow Is Nothing Then
                GetDBValue(txtAllowNumber, lRow("AllowNumber"))
                GetDBValue(txtDateAllowStart, lRow("AllowDT"))
                GetDBValue(txtTimeAllowStart, lRow("AllowDT"))
                GetDBValue(txtDateAllowEnd, lRow("AllowEndDT"))
                GetDBValue(txtTimeAllowEnd, lRow("AllowEndDT"))
                GetDBValue(cmbAllowUserName, lRow("AreaUserId"))
                GetDBValue(txtDateAllowDE, lRow("DEDT"))
                GetDBValue(txtTimeAllowDE, lRow("DEDT"))
                GetDBValue(chkIsAutoNumber, lRow("IsAutoNumber"))
            End If
            '====================================

            mRowD.BeginEdit()
            mRowD("Dummy01Id") = IIf(IsDBNull(mEditingRow("MPPostId")), DBNull.Value, mEditingRow("MPPostId"))
            mRowD("Dummy02Id") = IIf(IsDBNull(mEditingRow("MPFeederId")), DBNull.Value, mEditingRow("MPFeederId"))
            mRowD("Dummy03Id") = IIf(IsDBNull(mEditingRow("MPCloserTypeId")), DBNull.Value, mEditingRow("MPCloserTypeId"))
            mRowD("Dummy04Id") = IIf(IsDBNull(mEditingRow("FeederPartId")), DBNull.Value, mEditingRow("FeederPartId"))
            mRowD("Dummy05Id") = IIf(IsDBNull(mEditingRow("NazerId")), DBNull.Value, mEditingRow("NazerId"))
            mRowD("Dummy06Id") = IIf(IsDBNull(mEditingRow("LPPostId")), DBNull.Value, mEditingRow("LPPostId"))
            mRowD("Dummy07Id") = IIf(IsDBNull(mEditingRow("PeymankarId")), DBNull.Value, mEditingRow("PeymankarId"))
            mRowD("Dummy08Id") = IIf(IsDBNull(mEditingRow("DepartmentId")), DBNull.Value, mEditingRow("DepartmentId"))
            mRowD("Dummy09Id") = IIf(IsDBNull(mEditingRow("ReferToId")), DBNull.Value, mEditingRow("ReferToId"))
            mRowD("Dummy10Id") = IIf(IsDBNull(mEditingRow("TamirNetworkTypeId")), DBNull.Value, mEditingRow("TamirNetworkTypeId"))
            mRowD("Dummy11Id") = IIf(IsDBNull(mEditingRow("LPFeederId")), DBNull.Value, mEditingRow("LPFeederId"))
            mRowD("Dummy12Id") = IIf(IsDBNull(mEditingRow("TamirTypeId")), DBNull.Value, mEditingRow("TamirTypeId"))
            mRowD("Dummy13Id") = IIf(IsDBNull(mEditingRow("TamirRequestSubjectId")), DBNull.Value, mEditingRow("TamirRequestSubjectId"))
            'mRowD.EndEdit()
            LogTamirNetworkType("Step 3")

            mIsLoading = False

            GetDBValue(cmbMPPost, mEditingRow("MPPostId"))
            GetDBValue(cmbMPFeeder, mEditingRow("MPFeederId"))

            If mDs.TblTamirRequestFTFeeder.Count > 0 Then
                Dim lDs As New DataSet

                Dim lMPFeederIDs As String = ""
                For Each lRowMPFeeder As DataRow In mDs.TblTamirRequestFTFeeder.Rows
                    lMPFeederIDs &= IIf(lMPFeederIDs = "", lRowMPFeeder("MPFeederId"), "," & lRowMPFeeder("MPFeederId"))
                Next

                If lMPFeederIDs <> "" Then
                    BindingTable("select ISNULL(MPPostTransId,-1) AS MPPostTransId from Tbl_MPFeeder where MPFeederId IN (" & lMPFeederIDs & ")", mCnn, lDs, "TblSelectedMPTransID")
                    Dim lMPPostTransIDs As String = ""
                    For Each lRowTrans As DataRow In lDs.Tables("TblSelectedMPTransID").Rows
                        lMPPostTransIDs &= IIf(lMPPostTransIDs = "", lRowTrans("MPPostTransId"), "," & lRowTrans("MPPostTransId"))
                    Next
                    chkMPPostTrans.SetData(lMPPostTransIDs)
                End If

                ckcmbMPFeeder.SetData(lMPFeederIDs)

            ElseIf Not IsDBNull(mEditingRow("MPFeederId")) Then
                ckcmbMPFeeder.SetData(mEditingRow("MPFeederId"))
            End If

            GetDBValue(cmbFeederPart, mEditingRow("FeederPartId"))
            GetDBValue(cmbLPPost, mEditingRow("LPPostId"))
            GetDBValue(cmbLPFeeder, mEditingRow("LPFeederId"))
            GetDBValue(cmbTamirNetworkType, mEditingRow("TamirNetworkTypeId"))
            LogTamirNetworkType("Step 4")
            GetDBValue(cmbTamirType, mEditingRow("TamirTypeId"))
            GetDBValue(cmbReferTo, mEditingRow("ReferToId"))
            GetDBValue(cmbDepartment, mEditingRow("DepartmentId"))
            GetDBValue(cmbTamirRequestSubject, mEditingRow("TamirRequestSubjectId"))

            GetDBValue(cmbCloserType, mEditingRow("MPCloserTypeId"))
            GetDBValue(cmbManoeuvreType, mEditingRow("ManoeuvreTypeId"))
            GetDBValue(cmbNazer, mEditingRow("NazerId"))
            GetDBValue(cmbPeymankar, mEditingRow("PeymankarId"))
            If cmbPeymankar.SelectedIndex = -1 AndAlso Not IsDBNull(mEditingRow("Peymankar")) Then
                cmbPeymankar.Text = mEditingRow("Peymankar")
                mPeymankarText = mEditingRow("Peymankar")
            End If

            GetDBValue(cmbTamirNetworkType, mEditingRow("TamirNetworkTypeId"))
            LogTamirNetworkType("Step 5")
            GetDBValue(cmbTamirType, mEditingRow("TamirTypeId"))

            If cmbTamirType.SelectedIndex = -1 Then
                If chkIsEmergency.Checked Then
                    cmbTamirType.SelectedValue = TamirTypes.Ezterari
                    DatasetDummy1.ViewDummyId.Rows(0)("Dummy12Id") = TamirTypes.Ezterari
                Else
                    DatasetDummy1.ViewDummyId.Rows(0)("Dummy12Id") = TamirTypes.BaMovafeghat
                    cmbTamirType.SelectedValue = TamirTypes.BaMovafeghat
                End If
            End If

            If Not chkIsManoeuvre.Checked Then
                cmbManoeuvreType.SelectedIndex = -1
                cmbManoeuvreType.SelectedIndex = -1
            End If

            If mTamirRequestId = -1 OrElse mDs.Tables("TblTamirRequest").Rows.Count = 0 Then
                cmbMPPost.SelectedIndex = -1
                cmbMPPost.SelectedIndex = -1
                cmbMPFeeder.SelectedIndex = -1
                cmbMPFeeder.SelectedIndex = -1
            End If

            If Not IsDBNull(mEditingRow("FeederPartId")) Then
                chkIsLPPostFeederPart.Checked = True
                rbFeederPart.Checked = True
            ElseIf Not IsDBNull(mEditingRow("LPPostId")) Then
                chkIsLPPostFeederPart.Checked = True
                rbLPPost.Checked = True
            End If

            If Not IsDBNull(mEditingRow("LPFeederId")) Then
                chkIsLPPostFeederPart.Checked = True
                cmbLPFeeder.Enabled = True
                btnLPFeeder.Enabled = True
            End If

            ComputeSumTime()

            LoadPowerInfo()

            ComputeLPPostCountDC()

            mLastIsSendToSetadMode = chkIsEmergency.Checked

            If chkIsEmergency.Checked AndAlso chkIsLPPostFeederPart.Checked AndAlso mEditingRow("TamirRequestStateId") = TamirRequestStates.trs_PreNew Then
                chkIsSendToSetad.Text = "ارسال به ستاد"
                chkIsEmergency.Enabled = False
            End If

            If mDs.TblTamirRequestMultiStep.Rows.Count > 0 Then
                btnMultiStep.Text = "[" & btnMultiStep.Tag & "]"
                btnMultiStep.Font = New System.Drawing.Font("Tahoma", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
            End If

            If chkIsAutoNumber.Checked Then
                chkIsAutoNumber.Enabled = False
            End If

            If Not IsDBNull(mEditingRow("WarmLineConfirmStateId")) Then
                If mEditingRow("WarmLineConfirmStateId") = WarmLineConfirmState.wlcs_Confirm Then
                    rbConfirmWL.Checked = True
                ElseIf mEditingRow("WarmLineConfirmStateId") = WarmLineConfirmState.wlcs_NotConfirm Then
                    rbNotConfirmWL.Checked = True
                End If
            End If

            If Not IsDBNull(mEditingRow("WarmLineConfirmAreaUserId")) Then
                BindingTable("select * from Tbl_AreaUser where AreaUserId = " & mEditingRow("WarmLineConfirmAreaUserId"), mCnn, mDs, "Tbl_AreaUserWarmLine", , , , , , , True)
                txtWarmLineConfirmUserName.Text = mDs.Tables("Tbl_AreaUserWarmLine").Rows(0)("UserName")
            End If
            GetDBValue(txtWarmLineReason, mEditingRow("NotConfirmWarmLineReason"))
            GetDBValue(txtWarmLineDate, mEditingRow("WarmLineConfirmDatePersian"))
            GetDBValue(txtWarmLineTime, mEditingRow("WarmLineConfirmTime"))

            If mIsForWarmLineConfirm Then
                txtWarmLineConfirmUserName.Text = WorkingUserName
                txtWarmLineDate.Text = lNow.ShamsiDate
                txtWarmLineTime.Text = lNow.HourMin
            End If

            Dim lSubecribersSourceTypeId As Integer = 1
            BindingTable("select AreaId, ISNULL(SubecribersSourceTypeId,1) AS SubecribersSourceTypeId from Tbl_AreaInfo where AreaId = " & IIf(cmbArea.SelectedIndex > -1, cmbArea.SelectedValue, WorkingAreaId), mCnn, mDs, "Tbl_AreaInfo", aIsClearTable:=True)
            If mDs.Tables("Tbl_AreaInfo").Rows.Count > 0 Then
                lSubecribersSourceTypeId = mDs.Tables("Tbl_AreaInfo").Rows(0)("SubecribersSourceTypeId")
            End If

            If mIsSendGISSubscriberDCInfo AndAlso lSubecribersSourceTypeId = 2 Then
                If DatasetTemp1.Tables("TblTamirRequestKey").Rows.Count > 0 Then
                    mIsThereFeederKey = True
                    chkIsSendSMSSensitive.Text = "اطلاع رساني به مشترکين با SMS"
                End If
            End If
            '--------------omid
            Check_ReturnTime(lNow)
        Catch ex As Exception
            ShowError(ex)
        Finally
            mIsLoadInfo = False
        End Try

    End Sub
    Private Sub LoadOperationList()
        Try
            For Each lRow As DataRow In mDsView.ViewTamirOperationList.Rows
                dgOperations.GetRow(lRow).IsChecked = lRow("IsSelected")
            Next
        Catch ex As Exception
        End Try
    End Sub
    Private Function CheckUserAccess() As Boolean
        CheckUserAccess = True
        Dim lMsg As String = ""
        If mNotTamirRequestNetworkType <> "" Then
            CheckUserAccess = False
            If mNotTamirRequestNetworkType = "FT" Then
                lMsg = "شما مجوز لازم براي درخواست و ويرايش خاموشي فوق توزيع را نداريد" & vbCrLf &
                       "از منوي سامانه وارد مديريت کاربران شده و دسترسي لازم را اعمال نماييد"
            ElseIf mNotTamirRequestNetworkType = "MP" Then
                lMsg = "شما مجوز لازم براي درخواست و ويرايش خاموشي فشار متوسط را نداريد" & vbCrLf &
                       "از منوي سامانه وارد مديريت کاربران شده و دسترسي لازم را اعمال نماييد"
            ElseIf mNotTamirRequestNetworkType = "LP" Then
                lMsg = "شما مجوز لازم براي درخواست و ويرايش خاموشي فشار ضعيف را نداريد" & vbCrLf &
                       "از منوي سامانه وارد مديريت کاربران شده و دسترسي لازم را اعمال نماييد"
            End If
            ShowError(lMsg)
            Return CheckUserAccess
        End If
        If chkIsSendToSetad.Checked Then
            Dim lCanSendTamirRequest As Boolean = CheckUserTrustee("CanSendTamirRequest", 30, ApplicationTypes.TamirRequest)
            If Not lCanSendTamirRequest AndAlso Not mIsEmergencyEnabled Then
                CheckUserAccess = False
                lMsg = "شما مجوز لازم براي ارسال درخواست خاموشي را نداريد" & vbCrLf &
                       "از منوي سامانه وارد مديريت کاربران شده و دسترسي لازم را اعمال نماييد"
                ShowError(lMsg)
                Return CheckUserAccess
            End If
        End If
        Return CheckUserAccess
    End Function

    Private Function IsSaveOK() As Boolean
        'mRowD.EndEdit()
        'mRowD.AcceptChanges()

        Dim lStart As String = "قطع"
        Dim lEnd As String = "وصل"
        If mIsWarmLine Then
            lStart = "شروع عمليات"
            lEnd = "پايان عمليات"
        End If
        Dim lNow As CTimeInfo = GetServerTimeInfo()
        IsSaveOK = False
        Dim lMsg As String = ""

        Dim lState As TamirRequestStates = cmbState.SelectedValue

        If (mIsForConfirm And Not mIsForWarmLineConfirm) AndAlso (rbIsNotConfirm.Checked Or rbIsReturned.Checked) Then
            If txtUnConfirmReason.Text.Trim.Length > 0 Then
                If chkIsManoeuvre.Checked Then
                    mIsForceManovrDC = Not chkIsNotNeedManovrDC.Checked
                End If
                Return True
            Else
                If rbIsNotConfirm.Checked Then
                    ShowError("لطفا علت عدم تاييد را مشخص نماييد")
                End If
                If rbIsReturned.Checked Then
                    ShowError("لطفا علت عودت را مشخص نماييد")
                End If
                txtUnConfirmReason.Focus()
                Return False
            End If
        End If
        If mIsForWarmLineConfirm AndAlso (rbNotConfirmWL.Checked Or rbIsReturnedWL.Checked) Then
            If txtWarmLineReason.Text.Trim.Length > 0 Then
                Return True
            Else
                If rbNotConfirmWL.Checked Then
                    ShowError("لطفا علت عدم تاييد را مشخص نماييد")
                End If
                If rbIsReturnedWL.Checked Then
                    ShowError("لطفا علت عودت را مشخص نماييد")
                End If
                txtWarmLineReason.Focus()
                Return False
            End If
        End If

        '======================
        If cmbTamirType.SelectedIndex = -1 Then
            lMsg &= "- نوع موافقت"
        End If
        '======================
        Dim lInterval As Integer = Val(txtDCInterval.Text)
        If lInterval <= 0 Then
            lMsg &= "- تاريخ و زمان " & lEnd & " بايد بعد از تاريخ و زمان " & lStart & " باشد"
        End If
        '======================
        Try
            If cmbTamirNetworkType.SelectedValue <> TamirNetworkType.FT AndAlso (chkIsSendToSetad.Checked Or (mIsForConfirm And Not mIsForWarmLineConfirm) Or mIsSetadConfirmPlanned Or (IsSetadMode AndAlso lState = TamirRequestStates.trs_PreNew) Or chkIsEmergency.Checked) Then
                If mOperationCount > 0 Then
                    If mIsForceTamirOperationTime Then
                        Dim lSumTime As Integer = Val(txtSumTime.Text)
                        Dim lIsCheckWarmLineOperationTime As Boolean = CConfig.ReadConfig("IsCheckWarmLineOperationTime", True)
                        If lSumTime < lInterval And (Not chkIsWarmLine.Checked Or (chkIsWarmLine.Checked AndAlso lIsCheckWarmLineOperationTime)) Then
                            If chkIsWarmLine.Checked Then
                                lMsg &= "- مدت زمان عمليات خط گرم درخواستي از جمع مدت زمان عمليات‌هاي انتخاب شده در فهرست عمليات، بيشتر است"
                            Else
                                lMsg &= "- مدت زمان قطع درخواستي از جمع مدت زمان عمليات‌ها بيشتر است"
                            End If
                        End If
                    End If
                Else
                    lMsg &= "- لطفا فهرست عمليات هنگام خاموشي را مشخص نماييد"
                End If
            End If
        Catch ex As Exception
        End Try
        '======================

        If mIsForceEmergencyReason AndAlso cmbTamirType.SelectedValue = TamirTypes.Ezterari AndAlso txtEmergency.Text.Trim().Length = 0 AndAlso txtEmergency.Enabled AndAlso txtEmergency.Visible Then
            lMsg &= "- علت اضطراري بودن خاموشي"
        End If
        Dim lIsForcePeymankar As Boolean = CConfig.ReadConfig("IsForcePeymankar", False)
        If lIsForcePeymankar Then
            If rbIsRequestByPeymankar.Checked And cmbPeymankar.SelectedIndex = -1 Then
                lMsg &= "- پيمانکار"
            End If
        End If
        Dim lIsForcePeymankarTel As Boolean = CConfig.ReadConfig("IsForcePeymankarTel", False)
        If lIsForcePeymankarTel AndAlso rbIsRequestByPeymankar.Checked Then
            If txtPeymankarTel.Text.Trim().Length = 0 Then
                lMsg &= "- شماره موبايل پيمانکار"
            ElseIf Not IsMobile(txtPeymankarTel.Text) Then
                lMsg &= "- شماره موبايل پيمانکار بايد به درستي وارد گردد"
            End If
        End If
        If lState = mdlHashemi.TamirRequestStates.trs_PreNew And cmbDepartment.SelectedIndex = -1 Then
            lMsg &= "- واحد درخواست کننده"
        End If
        If lState = mdlHashemi.TamirRequestStates.trs_PreNew And cmbReferTo.SelectedIndex = -1 Then
            lMsg &= "- واحد اقدام کننده"
        End If
        If cmbArea.SelectedIndex = -1 Then
            lMsg &= "- ناحيه"
        End If
        If cmbCity.SelectedIndex = -1 Then
            lMsg &= "- شهرستان"
        End If
        If Not txtDateDisconnect.IsOK Then
            lMsg &= "- تاريخ " & lStart
        End If
        If Not txtTimeDisconnect.IsOK Then
            lMsg &= "- ساعت " & lStart
        End If
        If Not txtDateConnect.IsOK Then
            lMsg &= "- تاريخ " & lEnd
        End If
        If Not txtTimeConnect.IsOK Then
            lMsg &= "- ساعت " & lEnd
        End If
        If cmbMPPost.SelectedIndex = -1 Then
            lMsg &= "- پست فوق توزيع"
        End If
        If cmbTamirNetworkType.SelectedValue <> TamirNetworkType.FT AndAlso cmbMPFeeder.SelectedIndex = -1 Then
            lMsg &= "- فيدر فشار متوسط"
        End If
        If cmbTamirNetworkType.SelectedValue = TamirNetworkType.LP AndAlso cmbMPFeeder.SelectedIndex > -1 AndAlso (cmbLPPost.SelectedIndex = -1 And cmbLPFeeder.SelectedIndex = -1) Then
            lMsg &= "- فيدر فشار ضعيف"
        End If
        If cmbTamirNetworkType.SelectedValue <> TamirNetworkType.FT AndAlso txtCurrentValue.Text.Trim.Length = 0 Then
            lMsg &= "- بار هنگام قطع فيدر"
        End If
        If chkIsManoeuvre.Checked And cmbManoeuvreType.SelectedIndex = -1 And txtManoeuvreDesc.Text.Length = 0 Then
            lMsg &= "- نوع مانور"
        End If

        If Not chkIsWarmLine.Checked Or (chkIsWarmLine.Checked AndAlso mIsForceCriticalsAddress) Then
            If (chkIsSendToSetad.Checked Or (mIsForConfirm And Not mIsForWarmLineConfirm)) And txtAsdresses.Text.Trim.Length = 0 Then
                lMsg &= "- آدرس مناطقي که خاموش مي‌گردند"
            End If
        ElseIf chkIsWarmLine.Checked AndAlso Not mIsForceCriticalsAddress Then
            If (chkIsSendToSetad.Checked Or (mIsForConfirm And Not mIsForWarmLineConfirm)) And txtWorkingAddress.Text.Trim.Length = 0 Then
                lMsg &= "- آدرس محل کار"
            End If
        End If

        If lState = mdlHashemi.TamirRequestStates.trs_PreNew And Not rbIsRequestByPeymankar.Checked And Not rbIsRequestByTozi.Checked And Not rbIsRequestByFogheTozi.Checked Then
            lMsg &= "- درخواست خاموشي توسط پيمانکار است يا شرکت توزيع يا فوق توزيع"
        End If
        If (lState = mdlHashemi.TamirRequestStates.trs_PreNew And Not rbInCityService.Checked And Not rbNotInCityService.Checked) Then
            lMsg &= "- خاموشي در محدوده خدمات شهري است يا خير"
        End If
        Dim lIsForceTamirGPS As Boolean = CConfig.ReadConfig("IsForceTamirGPS", False)
        If lIsForceTamirGPS Then
            If txtGpsX.Text.Trim.Length = 0 Or txtGpsY.Text.Trim.Length = 0 Then
                lMsg &= "- مختصات X و Y"
            End If
        End If
        '==============================
        '-- اجباري بودن آدرس محل کار با توجه به کانفيگ
        Dim lIsForceWorkingAddress As Boolean = CConfig.ReadConfig("IsForceWorkingAddress", False)
        If lIsForceWorkingAddress Then
            If lState = mdlHashemi.TamirRequestStates.trs_PreNew And txtWorkingAddress.Text.Length = 0 Then
                lMsg &= "- آدرس محل کار"
            End If
        End If

        '-- اجباري بودن نوع قطع کننده  با توجه به کانفيگ
        Dim lIsForceTamirMPCloserType As Boolean = CConfig.ReadConfig("IsForceTamirMPCloserType", False)
        If lIsForceTamirMPCloserType Then
            If lState = mdlHashemi.TamirRequestStates.trs_PreNew And cmbCloserType.Visible AndAlso cmbCloserType.SelectedIndex = -1 Then
                lMsg &= "- نوع قطع کننده"
            End If
        End If
        '====================
        '-- اجباري بودن تاييد ناظر با توجه به کانفيگ
        Dim lIsForceTaeedNazer As Boolean = CConfig.ReadConfig("IsForceTaeedNazer", False)
        Dim lIsForceTaeedNazerForWarmLine As Boolean = CConfig.ReadConfig("IsForceTaeedNazerForWarmLine", False)

        If lIsForceTaeedNazer And Not cmbTamirType.SelectedValue = TamirTypes.Ezterari And Not chkIsEmergency.Checked Then
            Dim lIsForceTaeedNazerForLPFeeder As Boolean = CConfig.ReadConfig("IsForceTaeedNazerForLPFeeder", True)
            If (cmbTamirNetworkType.SelectedValue = TamirNetworkType.LP AndAlso Not lIsForceTaeedNazerForLPFeeder) _
                Or (chkIsWarmLine.Checked And Not lIsForceTaeedNazerForWarmLine) Then
            Else
                If (chkIsSendToSetad.Checked Or (mIsForConfirm And Not mIsForWarmLineConfirm)) Then
                    If (Not chkIsConfirmNazer.Checked) Then
                        lMsg &= "- مورد تاييد ناظر قرار نگرفته است"
                    End If
                End If
            End If
        End If

        '-- بررسي ورود اطلاعات مربوط به خاموشي مانور
        mIsForceManovrDC = False
        If chkIsManoeuvre.Checked Then
            mIsForceManovrDC = Not chkIsNotNeedManovrDC.Checked
            'mIsForceManovrDC = CConfig.ReadConfig("IsForceTamirManovrDC", True)
            'If txtDateDisconnectManovr.IsOK OrElse txtTimeDisconnectManovr.IsOK _
            '    OrElse txtDateConnectManovr.IsOK OrElse txtTimeConnectManovr.IsOK Then
            '    mIsForceManovrDC = True
            'End If

            If mIsForceManovrDC Then
                If Not txtDateDisconnectManovr.IsOK Then
                    lMsg &= "- تاريخ قطع براي بازگشت مانور"
                End If
                If Not txtTimeDisconnectManovr.IsOK Then
                    lMsg &= "- ساعت قطع براي بازگشت مانور"
                End If
                If Not txtDateConnectManovr.IsOK Then
                    lMsg &= "- تاريخ وصل براي بازگشت مانور"
                End If
                If Not txtTimeConnectManovr.IsOK Then
                    lMsg &= "- ساعت وصل براي بازگشت مانور"
                End If
            End If
        End If
        '------------------------------------------

        Dim lIsForceGISFormNo As Boolean = CConfig.ReadConfig("IsForceGISFormNo", False)
        If lIsForceGISFormNo AndAlso (chkIsSendToSetad.Checked Or (mIsForConfirm And Not mIsForWarmLineConfirm)) Then
            If txtWorkCommandNo.Text.Trim() = "" And txtGISFormNo.Text.Trim() = "" Then
                lMsg &= "- يکي از موارد «شماره دستور کار» و «شماره فرم درخواست ثبت تغييرات GIS» "
            End If
        End If

        If mIsForConfirm And Not mIsForWarmLineConfirm Then
            If rbIsConfirm.Checked Or rbIsNotConfirm.Checked Or rbIsReturned.Checked Then
                If rbIsNotConfirm.Checked AndAlso txtUnConfirmReason.Text.Trim.Length = 0 Then
                    lMsg &= "- دليل عدم تأييد"
                ElseIf rbIsReturned.Checked AndAlso txtUnConfirmReason.Text.Trim.Length = 0 Then
                    lMsg &= "- دليل عودت"
                End If
                If Not txtDateConfirm.IsOK Then
                    lMsg &= "- تاريخ تأييد"
                End If
                If Not txtTimeConfirm.IsOK Then
                    lMsg &= "- ساعت تأييد"
                End If

                Dim lSubecribersSourceTypeId As Integer = 1
                BindingTable("select AreaId, ISNULL(SubecribersSourceTypeId,1) AS SubecribersSourceTypeId from Tbl_AreaInfo where AreaId = " & cmbArea.SelectedValue, mCnn, mDs, "Tbl_AreaInfo", aIsClearTable:=True)
                If mDs.Tables("Tbl_AreaInfo").Rows.Count > 0 Then
                    lSubecribersSourceTypeId = mDs.Tables("Tbl_AreaInfo").Rows(0)("SubecribersSourceTypeId")
                End If

                If rbIsConfirm.Checked AndAlso mIsThereFeederKey AndAlso chkIsSendSMSSensitive.Checked AndAlso lSubecribersSourceTypeId = 2 Then
                    BindingTable("SELECT * FROM TblTamirRequestSMS WHERE TamirRequestId = " & mTamirRequestId, mCnn, DatasetCcRequester1, "TblTamirRequestSMS", , , , , , , True)
                    If DatasetCcRequester1.Tables("TblTamirRequestSMS").Rows.Count = 0 Then
                        Dim lDurationOfGetGISSubscriber As Integer = CConfig.ReadConfig("DurationOfGetGISSubscriber", "0")
                        Dim lDurationTime As Integer = DateDiff(DateInterval.Minute, mEditingRow("DEDT"), GetServerTimeInfo.MiladiDate)
                        Dim lTime As Integer = lDurationOfGetGISSubscriber - lDurationTime
                        If lTime > 0 Then
                            lMsg &= "- هنوز اطلاعات مشترکيني که خاموش مي گردند دريافت نشده است. شما در حال حاضر نميتوانيد پيامک خاموشي را ارسال نماييد. مدت زمان باقيمانده " & lTime & " دقيقه"
                        End If
                    Else
                        mIsShowGISKeyInform = True
                    End If
                End If
            Else
                lMsg &= "- حالت تأييد"
            End If
        End If

        If mIsForWarmLineConfirm Then
            If rbConfirmWL.Checked Or rbNotConfirmWL.Checked Or rbIsReturnedWL.Checked Then
                If rbNotConfirmWL.Checked AndAlso txtWarmLineReason.Text.Trim.Length = 0 Then
                    lMsg &= "- دليل عدم تأييد"
                ElseIf rbIsReturnedWL.Checked AndAlso txtWarmLineReason.Text.Trim.Length = 0 Then
                    lMsg &= "- دليل عودت"
                End If
                If Not txtWarmLineDate.IsOK Then
                    lMsg &= "- تاريخ تأييد"
                End If
                If Not txtWarmLineTime.IsOK Then
                    lMsg &= "- ساعت تأييد"
                End If
            Else
                lMsg &= "- حالت تأييد"
            End If
        End If

        If mIsForAllow Then
            'If txtAllowNumber.Text.Trim.Length = 0 And Not chkIsAutoNumber.Checked Then
            '    lMsg &= "- شماره اجازه کار"
            'End If
            'If Not txtDateAllowStart.IsOK Then
            '    lMsg &= "- تاريخ شروع اجازه کار"
            'End If
            'If Not txtTimeAllowStart.IsOK Then
            '    lMsg &= "- ساعت شروع اجازه کار"
            'End If
            'If Not IsDateOK(txtDateAllowEnd) Then
            '    lMsg &= "- تاريخ پايان اجازه کار"
            'End If
            'If Not IsTimeOK(txtTimeAllowEnd) Then
            '    lMsg &= "- ساعت پايان اجازه کار"
            'End If
        End If

        '--------------------------------------    
        'Check Count MPFeederKey
        '--------------------------------------
        Dim lISSelectMPFeederKey As Boolean = ReadConfig("IsForceSelectMPFeederKey", False)
        If lISSelectMPFeederKey And Not chkIsWarmLine.Checked And cmbTamirNetworkType.SelectedValue = TamirNetworkType.MP And rbFeederPart.Checked Then
            GetCountMPFeederKey()
            If mCntMPFeederKey > 0 AndAlso mIsSelectKey = False Then
                lMsg &= "  - کليد زني، عدم انتخاب کليد فيدر فشار متوسط به هنگام قطع  "
            End If
        End If
        '======================

        '--> Check Disconnect Date Validity <--
        If lMsg = "" Then
            txtDateDisconnect.InsertTime(txtTimeDisconnect)

            Dim lDT As DateTime = txtDateDisconnect.MiladiDT
            Dim lDTNow As DateTime = lNow.MiladiDate

            'Dim lDay1 As Long = lDTNow.DayOfYear + (lDTNow.Year - 1) * 365
            'Dim lDay2 As Long = lDT.DayOfYear + (lDT.Year - 1) * 365
            'Dim lDiff As Long = lDay2 - lDay1

            Dim lDay1 As String = lDTNow.ToShortDateString()
            Dim lDay2 As String = lDT.ToShortDateString()
            Dim lDiff As Long = DateDiff(DateInterval.Day, Convert.ToDateTime(lDay1), Convert.ToDateTime(lDay2))
            Dim lDiffHour As Integer = DateDiff(DateInterval.Hour, Convert.ToDateTime(lDTNow), Convert.ToDateTime(lDT))
            Dim lDiffMin As Long = DateDiff(DateInterval.Minute, lDTNow, lDT)
            Dim lIsBaMovafeghatLimit As Boolean = Convert.ToBoolean(CConfig.ReadConfig("IsBaMovafeghatLimit", False))
            Dim lIsLPTamirLimit As Boolean = Not Convert.ToBoolean(CConfig.ReadConfig("NoLPTamirLimit", False))
            Dim lIsCheckTimeForAll As Boolean = CConfig.ReadConfig("IsCheckTimeForAll", True)
            Dim lIsCheckHoliday As Boolean = CConfig.ReadConfig("IsCheckHoliday", False)
            Dim lIsHolidayThursday As Boolean = CConfig.ReadConfig("IsHolidayThursday", False)
            Dim lIsHolidayFriday As Boolean = CConfig.ReadConfig("IsHolidayFriday", False)
            Dim lIsNotTamirForHoliday As Boolean = CConfig.ReadConfig("IsNotTamirForHoliday", False)

            Dim lIsCheckHolidayWL As Boolean = CConfig.ReadConfig("IsCheckHolidayWL", False)
            Dim lIsHolidayThursdayWL As Boolean = CConfig.ReadConfig("IsHolidayThursdayWL", False)
            Dim lIsHolidayFridayWL As Boolean = CConfig.ReadConfig("IsHolidayFridayWL", False)
            Dim lIsNotTamirForHolidayWL As Boolean = CConfig.ReadConfig("IsNotTamirForHolidayWL", False)

            Dim lIsHoliday As Boolean = False

            Dim lHoliDayStr As String = ""
            Dim lIsCheckReturnTimeInNewTamir As Boolean = CConfig.ReadConfig("IsCheckReturnTimeInNewTamir", False)

            If Not chkIsWarmLine.Checked Then

                If lIsNotTamirForHoliday Then
                    lIsHoliday = IsHoliday(lDT, True, lIsHolidayThursday, lIsHolidayFriday)
                    If lIsHoliday Then
                        If Not chkIsEmergency.Checked And (cmbTamirType.SelectedValue = TamirTypes.BarnamehRiziShodeh Or (cmbTamirType.SelectedValue = TamirTypes.BaMovafeghat And lIsBaMovafeghatLimit)) Then
                            lMsg &= "- متأسفانه شما نمي توانيد براي روز تعطيل، درخواست خاموشي ثبت نماييد"
                        End If
                    End If
                End If

                Dim lIsDiffHour As Integer = Val(CConfig.ReadConfig("IsTamirLimitHour", 0))

                If Not lIsHoliday Then
                    Dim lHoliDays As Integer = GetHolidays(lDTNow.AddDays(1), lDT, lIsCheckHoliday, lIsHolidayThursday, lIsHolidayFriday)

                    If lIsDiffHour = 0 Then

                        lDiff -= lHoliDays
                        If lHoliDays > 0 Then
                            lHoliDayStr = " (با توجه به " & lHoliDays & " روز تعطيلي تا تاريخ خاموشي )"
                        End If


                        If lDiff >= 0 And lDiffMin > 0 Then
                            If (Not rbIsReturned.Checked Or (rbIsReturned.Checked And lIsCheckReturnTimeInNewTamir And Not mIsForConfirm)) And
                                (Not rbIsReturnedWL.Checked Or (rbIsReturnedWL.Checked And lIsCheckReturnTimeInNewTamir And Not mIsConfirmEndWarmLine)) Then

                                If Not chkIsEmergency.Checked And (cmbTamirNetworkType.SelectedValue <> TamirNetworkType.LP Or lIsLPTamirLimit) Then

                                    If lIsCheckTimeForAll Or Not mIsForConfirm Then

                                        Dim lIsTimeLimit As Boolean = Convert.ToBoolean(CConfig.ReadConfig("TamirTimeLimit", True))

                                        If lIsTimeLimit And (cmbTamirType.SelectedValue = TamirTypes.BarnamehRiziShodeh Or (cmbTamirType.SelectedValue = TamirTypes.BaMovafeghat And lIsBaMovafeghatLimit)) Then
                                            Dim lBeforeHour As Integer = Val(CConfig.ReadConfig("IsTamirBeforeHour", 0))
                                            Dim lTamirBeforeDay As Integer = 0

                                            Dim lIsBeforeHour As Boolean = lBeforeHour > 0 And lBeforeHour < 24

                                            If lIsBeforeHour Then
                                                If mDs.Tables("TblTamirRequest").Rows.Count > 0 AndAlso Not IsDBNull(mDs.Tables("TblTamirRequest").Rows(0)("IsReturned")) AndAlso mDs.Tables("TblTamirRequest").Rows(0)("IsReturned") Then
                                                    lTamirBeforeDay = Val(CConfig.ReadConfig("TamirBeforeOdatDay", 1))
                                                Else
                                                    lTamirBeforeDay = Val(CConfig.ReadConfig("TamirBeforeDay", 1))
                                                End If
                                            End If

                                            If lDiff < lTamirBeforeDay Then
                                                lMsg &= "- تاريخ درخواست خاموشي حداقل بايد قبل از ساعت " & lBeforeHour & "، " & lTamirBeforeDay & " روز قبل باشد" & lHoliDayStr
                                            ElseIf lDiff = lTamirBeforeDay Then
                                                If lDTNow.Hour >= lBeforeHour Then
                                                    lMsg &= "- تاريخ درخواست خاموشي حداقل بايد قبل از ساعت " & lBeforeHour & "، " & lTamirBeforeDay & " روز قبل باشد" & lHoliDayStr
                                                End If
                                            End If

                                        End If

                                    End If
                                End If
                            End If
                        ElseIf Not rbIsNotConfirm.Checked And lState < mdlHashemi.TamirRequestStates.trs_Confirm Then
                            lMsg &= "- زمان خاموشي درخواست شده معتبر نمي‌باشد."
                        End If
                    Else
                        lDiffHour -= (lHoliDays * 24)
                        If lHoliDays > 0 Then
                            lHoliDayStr = " (با توجه به " & lHoliDays & " روز تعطيلي تا تاريخ خاموشي )"
                        End If

                        If lDiff >= 0 And lDiffMin > 0 Then
                            If (Not rbIsReturned.Checked Or (rbIsReturned.Checked And lIsCheckReturnTimeInNewTamir And Not mIsForConfirm)) And
                                (Not rbIsReturnedWL.Checked Or (rbIsReturnedWL.Checked And lIsCheckReturnTimeInNewTamir And Not mIsConfirmEndWarmLine)) Then

                                If Not chkIsEmergency.Checked And (cmbTamirNetworkType.SelectedValue <> TamirNetworkType.LP Or lIsLPTamirLimit) Then

                                    If lIsCheckTimeForAll Or Not mIsForConfirm Then

                                        Dim lIsTimeLimit As Boolean = Convert.ToBoolean(CConfig.ReadConfig("TamirTimeLimit", True))

                                        If lIsTimeLimit And (cmbTamirType.SelectedValue = TamirTypes.BarnamehRiziShodeh Or (cmbTamirType.SelectedValue = TamirTypes.BaMovafeghat And lIsBaMovafeghatLimit)) Then
                                            If lDiffHour < lIsDiffHour Then
                                                lMsg &= "- تاريخ درخواست خاموشي حداقل بايد " & lIsDiffHour & " ساعت قبل از خاموشي باشد" & lHoliDayStr
                                            End If
                                        End If

                                    End If
                                End If
                            End If
                        ElseIf Not rbIsNotConfirm.Checked And lState < mdlHashemi.TamirRequestStates.trs_Confirm Then
                            lMsg &= "- زمان خاموشي درخواست شده معتبر نمي‌باشد."
                        End If
                    End If
                End If



            Else

                If lIsNotTamirForHolidayWL Then
                    lIsHoliday = IsHoliday(lDT, True, lIsHolidayThursdayWL, lIsHolidayFridayWL)
                    If lIsHoliday Then
                        If Not chkIsEmergency.Checked And (cmbTamirType.SelectedValue = TamirTypes.BarnamehRiziShodeh Or (cmbTamirType.SelectedValue = TamirTypes.BaMovafeghat And lIsBaMovafeghatLimit)) Then
                            lMsg &= "- متأسفانه شما نمي توانيد براي روز تعطيل، درخواست عمليات خط گرم ثبت نماييد"
                        End If
                    End If
                End If

                Dim lIsDiffHourWL As Integer = Val(CConfig.ReadConfig("IsTamirLimitHourWL", 0))

                If Not lIsHoliday Then
                    Dim lHoliDays As Integer = GetHolidays(lDTNow.AddDays(1), lDT, lIsCheckHolidayWL, lIsHolidayThursdayWL, lIsHolidayFridayWL)

                    If lIsDiffHourWL = 0 Then

                        lDiff -= lHoliDays
                        If lHoliDays > 0 Then
                            lHoliDayStr = " (با توجه به " & lHoliDays & " روز تعطيلي تا زمان شروع عمليات خط گرم )"
                        End If

                        If lDiff >= 0 And lDiffMin > 0 Then
                            If (Not rbIsReturned.Checked Or (rbIsReturned.Checked And lIsCheckReturnTimeInNewTamir And Not mIsForConfirm)) And
                                (Not rbIsReturnedWL.Checked Or (rbIsReturnedWL.Checked And lIsCheckReturnTimeInNewTamir And Not mIsConfirmEndWarmLine)) Then

                                If Not chkIsEmergency.Checked And (cmbTamirNetworkType.SelectedValue <> TamirNetworkType.LP Or lIsLPTamirLimit) Then

                                    If lIsCheckTimeForAll Or (Not mIsForWarmLineConfirm And Not mIsForConfirm) Then

                                        Dim lIsTimeLimitWL As Boolean = Convert.ToBoolean(CConfig.ReadConfig("TamirTimeLimitWL", True))

                                        If lIsTimeLimitWL And (cmbTamirType.SelectedValue = TamirTypes.BarnamehRiziShodeh Or (cmbTamirType.SelectedValue = TamirTypes.BaMovafeghat And lIsBaMovafeghatLimit)) Then
                                            Dim lBeforeHourWL As Integer = Val(CConfig.ReadConfig("IsTamirBeforeHourWL", 0))
                                            Dim lTamirBeforeDayWL As Integer = 0
                                            Dim lIsBeforeHourWL As Boolean = lBeforeHourWL > 0 And lBeforeHourWL < 24

                                            If lIsBeforeHourWL Then
                                                If mDs.Tables("TblTamirRequest").Rows.Count > 0 AndAlso Not IsDBNull(mDs.Tables("TblTamirRequest").Rows(0)("IsReturned")) AndAlso mDs.Tables("TblTamirRequest").Rows(0)("IsReturned") Then
                                                    lTamirBeforeDayWL = Val(CConfig.ReadConfig("TamirBeforeOdatDayWL", 1))
                                                Else
                                                    lTamirBeforeDayWL = Val(CConfig.ReadConfig("TamirBeforeDayWL", 1))
                                                End If
                                            End If

                                            If lDiff < lTamirBeforeDayWL Then
                                                lMsg &= "- تاريخ درخواست عمليات خط گرم حداقل بايد قبل از ساعت " & lBeforeHourWL & "، " & lTamirBeforeDayWL & " روز قبل باشد" & lHoliDayStr
                                            ElseIf lDiff = lTamirBeforeDayWL Then
                                                If lDTNow.Hour >= lBeforeHourWL Then
                                                    lMsg &= "- تاريخ درخواست عمليات خط گرم حداقل بايد قبل از ساعت " & lBeforeHourWL & "، " & lTamirBeforeDayWL & " روز قبل باشد" & lHoliDayStr
                                                End If
                                            End If
                                        End If

                                    End If
                                End If
                            End If
                        ElseIf Not rbIsNotConfirm.Checked And lState < mdlHashemi.TamirRequestStates.trs_Confirm Then
                            lMsg &= "- زمان عمليات درخواست شده معتبر نمي‌باشد."
                        End If
                    Else

                        lDiffHour -= (lHoliDays * 24)
                        If lHoliDays > 0 Then
                            lHoliDayStr = " (با توجه به " & lHoliDays & " روز تعطيلي تا زمان شروع عمليات خط گرم )"
                        End If

                        If lDiff >= 0 And lDiffMin > 0 Then
                            If (Not rbIsReturned.Checked Or (rbIsReturned.Checked And lIsCheckReturnTimeInNewTamir And Not mIsForConfirm)) And
                                (Not rbIsReturnedWL.Checked Or (rbIsReturnedWL.Checked And lIsCheckReturnTimeInNewTamir And Not mIsConfirmEndWarmLine)) Then

                                If Not chkIsEmergency.Checked And (cmbTamirNetworkType.SelectedValue <> TamirNetworkType.LP Or lIsLPTamirLimit) Then

                                    If lIsCheckTimeForAll Or (Not mIsForWarmLineConfirm And Not mIsForConfirm) Then

                                        Dim lIsTimeLimitWL As Boolean = Convert.ToBoolean(CConfig.ReadConfig("TamirTimeLimit", True))

                                        If lIsTimeLimitWL And (cmbTamirType.SelectedValue = TamirTypes.BarnamehRiziShodeh Or (cmbTamirType.SelectedValue = TamirTypes.BaMovafeghat And lIsBaMovafeghatLimit)) Then
                                            If lDiffHour < lIsDiffHourWL Then
                                                lMsg &= "- تاريخ عمليات خط گرم حداقل بايد " & lIsDiffHourWL & " ساعت قبل از خاموشي باشد " & lHoliDayStr
                                            End If
                                        End If

                                    End If
                                End If
                            End If
                        ElseIf Not rbIsNotConfirm.Checked And lState < mdlHashemi.TamirRequestStates.trs_Confirm Then
                            lMsg &= "- زمان عمليات درخواست شده معتبر نمي‌باشد."
                        End If
                    End If
                End If

            End If

        End If

        If lMsg = "" Then
            If DateDiff(DateInterval.Day, lNow.MiladiDate, txtDateDisconnect.MiladiDT) >= 50 Then
                Dim lWarningMsg As String =
                        "شما در حال درخواست خاموشي، براي بيش از 50 روز آينده هستيد " & vbCrLf &
                        "آيا از درخواست خود اطمينان داريد؟"
                Dim lAns As DialogResult = MsgBoxF(lWarningMsg, "هشدار", MessageBoxButtons.YesNo, MsgBoxIcon.MsgIcon_Exclamation, MessageBoxDefaultButton.Button1)
                If lAns = DialogResult.No Then
                    IsSaveOK = False
                    Exit Function
                End If
            ElseIf DateDiff(DateInterval.Day, lNow.MiladiDate, txtDateDisconnect.MiladiDT) > 30 Then
                Dim lWarningMsg As String =
                        "شما در حال درخواست خاموشي، براي بيش از 30 روز آينده هستيد " & vbCrLf &
                        "آيا از درخواست خود اطمينان داريد؟"
                Dim lAns As DialogResult = MsgBoxF(lWarningMsg, "هشدار", MessageBoxButtons.YesNo, MsgBoxIcon.MsgIcon_Exclamation, MessageBoxDefaultButton.Button1)
                If lAns = DialogResult.No Then
                    IsSaveOK = False
                    Exit Function
                End If
            End If
            If DateDiff(DateInterval.Day, lNow.MiladiDate, txtDateDisconnect.MiladiDT) >= 60 Then
                Dim lWarningMsg As String =
                        "شما اجازه ثبت درخواست خاموشي براي بيش از 60 روز آينده را نداريد "
                ShowError(lWarningMsg)
                IsSaveOK = False
                Exit Function
            End If
        End If

        '--------------------------------------
        '           Checking MPFeeder DC
        '--------------------------------------
        If lMsg = "" Then
            Dim lLastDaysOfMPFeederDC As Integer = CConfig.ReadConfig("LastDaysOfMPFeederDC", 0)
            Dim lSumMPFeederDC As Integer
            Dim lDT As DateTime = txtDateDisconnect.MiladiDT
            Dim lTxt As String = "درخواست"
            If lLastDaysOfMPFeederDC > 0 Then
                If mIsForConfirm Then
                    lTxt = "تأييد"
                End If

                If cmbTamirNetworkType.SelectedValue = TamirNetworkType.MP AndAlso
                cmbFeederPart.SelectedIndex = -1 AndAlso
                Not chkIsWarmLine.Checked AndAlso
                cmbLPPost.SelectedIndex = -1 Then
                    Dim lSQL As String = ""
                    lSQL =
                        " SELECT SUM(cntDCMPFeeder) AS sumDC FROM ( " &
                        " SELECT COUNT(*) as cntDCMPFeeder FROM TblMPRequest " &
                        " INNER JOIN TblRequest ON TblMPRequest.MPRequestId = TblRequest.MPRequestId " &
                        " WHERE TblRequest.IsDisconnectMPFeeder = 1 " &
                        " AND DATEDIFF(DAY,TblMPRequest.DisconnectDT, '" & lDT.Date & "') <= " & lLastDaysOfMPFeederDC &
                        " AND TblMPRequest.MPFeederId = " & cmbMPFeeder.SelectedValue &
                        " AND ISNULL(TblMPRequest.IsWarmLine,0) = 0 " &
                        " UNION " &
                        " SELECT COUNT(*) as cntMPFeederDC FROM TblRequest " &
                        " INNER JOIN TblFogheToziDisconnect ON TblRequest.FogheToziDisconnectId = TblFogheToziDisconnect.FogheToziDisconnectId " &
                        " WHERE TblFogheToziDisconnect.IsFeederMode = 0 " &
                        " AND DATEDIFF(DAY,TblFogheToziDisconnect.DisconnectDT, '" & lDT.Date & "') <= " & lLastDaysOfMPFeederDC &
                        " AND TblFogheToziDisconnect.MPPostId = " & cmbMPPost.SelectedValue &
                        " UNION " &
                        " SELECT COUNT(*) as cntMPFeederDC FROM TblRequest " &
                        " INNER JOIN TblFogheToziDisconnect ON TblRequest.FogheToziDisconnectId = TblFogheToziDisconnect.FogheToziDisconnectId " &
                        " INNER JOIN TblFogheToziDisconnectMPFeeder ON TblFogheToziDisconnect.FogheToziDisconnectId = TblFogheToziDisconnectMPFeeder.FogheToziDisconnectId " &
                        " WHERE TblFogheToziDisconnect.IsFeederMode = 1 " &
                        " AND DATEDIFF(DAY,TblFogheToziDisconnectMPFeeder.DisconnectDT, '" & lDT.Date & "') <= " & lLastDaysOfMPFeederDC &
                        " AND TblFogheToziDisconnectMPFeeder.MPFeederId = " & cmbMPFeeder.SelectedValue &
                        ") t1 "
                    BindingTable(lSQL, mCnn, mDs, "TblGetLastMPFeederDC", , , , , , , True)
                    If mDs.Tables("TblGetLastMPFeederDC").Rows.Count > 0 Then
                        lSumMPFeederDC = mDs.Tables("TblGetLastMPFeederDC").Rows(0)("sumDC")
                        If lSumMPFeederDC > 0 Then
                            Dim lWarningMsg As String =
                                "در " & lLastDaysOfMPFeederDC & " روز گذشته، فيدر " & cmbMPFeeder.Text & " به تعداد " & lSumMPFeederDC & " بار خاموش شده است " & vbCrLf &
                                "آيا خاموشي ديگري را بر روي اين فيدر " & lTxt & " مي کنيد؟"
                            Dim lAns As DialogResult = MsgBoxF(lWarningMsg, "هشدار", MessageBoxButtons.YesNo, MsgBoxIcon.MsgIcon_Exclamation, MessageBoxDefaultButton.Button1)
                            If lAns = DialogResult.No Then
                                IsSaveOK = False
                                Exit Function
                            End If
                        End If
                    End If
                End If
            End If

        End If

        '--------------------------------------
        '           Checking MaxPower
        '--------------------------------------

        If lMsg = "" Then

            If Not ((mIsForConfirm And rbIsNotConfirm.Checked) Or mIsForWarmLineConfirm) Then

                Dim lKh As New KhayamDateTime
                Dim lSaturdayDT As DateTime
                Dim lSaturdayPDate As String
                Dim lExtraPower As Double = 0

                lSaturdayDT = GetFirstDateOfTamirScope(txtDateDisconnect.MiladiDT)
                lKh.SetMiladyDate(lSaturdayDT)
                lSaturdayPDate = lKh.GetShamsiDateStr

                If rbIsRequestByFogheTozi.Checked And cmbTamirNetworkType.SelectedValue = TamirNetworkType.FT Then

                ElseIf lState < mdlHashemi.TamirRequestStates.trs_Confirm AndAlso Not chkIsWarmLine.Checked Then

                    Dim lPower As Double = txtPower.Text

                    Try
                        If chkIsManoeuvre.Checked And Val(txtPowerManovr.Text) > 0 Then
                            lPower += Val(txtPowerManovr.Text)
                        End If
                    Catch ex As Exception
                    End Try

                    If lblPowerUnit.Text.ToLower = "kwh" Then lPower /= 1000

                    If mIsTamirPowerMonthly Then
                        mEditingWeakPowerRow = GetWeakPowerInfoMonthly(cmbArea.SelectedValue, lSaturdayDT, lSaturdayPDate, lPower)
                    Else
                        mEditingWeakPowerRow = GetWeakPowerInfo(cmbArea.SelectedValue, lSaturdayDT, lSaturdayPDate, lPower)
                    End If

                    If Not mEditingWeakPowerRow Is Nothing Then
                        lExtraPower = mEditingWeakPowerRow.LastExtraPower + mEditingWeakPowerRow.UsedPower - mEditingWeakPowerRow.MaxPower
                        If Not IsSetadMode Then
                            lExtraPower += Val(txtWaitPower.Text.Trim)
                        End If
                    Else
                        lExtraPower = 0.000000001
                    End If

                    If lExtraPower > 0 Then
                        lMsg =
                            "مقدار خاموشي درخواستي، از سهميه خاموشي " & mWeakPowerName & " شما به ميزان " & lExtraPower.ToString("0.##") & " مگاوات ساعت بيشتر شده است." & vbCrLf &
                            "آيا درخواست را ثبت ميکنيد؟"
                        Dim lAns As DialogResult = MsgBoxF(lMsg, "هشدار", MessageBoxButtons.OKCancel, MsgBoxIcon.MsgIcon_Hand, MessageBoxDefaultButton.Button1)
                        If lAns = DialogResult.Cancel Then
                            IsSaveOK = False
                            Exit Function
                        Else
                            lMsg = ""
                        End If
                    End If

                    If Not chkIsLPPostFeederPart.Checked And cmbTamirNetworkType.SelectedValue <> TamirNetworkType.FT Then

                        Dim lVRow As DataRowView = cmbMPFeeder.SelectedItem
                        If Not IsDBNull(lVRow("MaxDCCount")) Then

                            Dim lCurDt As DateTime = txtDateDisconnect.MiladiDT
                            Dim lKhDT As New KhayamDateTime
                            Dim lPDate1 As String, lPDate2 As String
                            Dim lDays As Integer, lMonthDays As Integer

                            lKhDT.SetMiladyDate(lCurDt)
                            lPDate1 = lKhDT.GetShamsiDateStr()
                            lDays = Val(lPDate1.Substring(8, 2))
                            lMonthDays = MonthDays(Val(lPDate1.Substring(5, 2)) - 1)

                            lCurDt = lCurDt.AddDays(-lDays + 1)
                            lKhDT.SetMiladyDate(lCurDt)
                            lPDate1 = lKhDT.GetShamsiDateStr()

                            lCurDt = lCurDt.AddDays(lMonthDays - 1)
                            lKhDT.SetMiladyDate(lCurDt)
                            lPDate2 = lKhDT.GetShamsiDateStr()

                            Dim lDCCount As Integer = 0
                            Dim lReqCount As Integer = 0
                            Dim lMaxFeederDC As Integer = lVRow("MaxDCCount")

                            Dim lSQL As String =
                                "SELECT Count(RequestId) As Cnt FROM ViewAllRequest WHERE" &
                                " IsTamir = 1 AND IsDisconnectMPFeeder = 1 " &
                                " AND MPFeederId = " & lVRow("MPFeederId") &
                                " AND (DisconnectDatePersian >= '" & lPDate1 & "')" &
                                " AND (DisconnectDatePersian <= '" & lPDate2 & "')"
                            RemoveMoreSpaces(lSQL)
                            BindingTable(lSQL, mCnn, mDsReq, "ViewTamirCount")

                            If mDsReq.Tables.Contains("ViewTamirCount") AndAlso mDsReq.Tables("ViewTamirCount").Rows.Count > 0 Then
                                lDCCount = mDsReq.Tables("ViewTamirCount").Rows(0)("Cnt")
                            End If

                            lSQL =
                                " SELECT COUNT(TblTamirRequest.TamirRequestId) AS Cnt " &
                                " FROM TblTamirRequest INNER JOIN TblTamirRequestConfirm ON TblTamirRequest.TamirRequestId = TblTamirRequestConfirm.TamirRequestId " &
                                " WHERE " &
                                " 	TblTamirRequestConfirm.IsConfirm = 1 AND " &
                                " 	TblTamirRequest.MPFeederId = " & lVRow("MPFeederId") &
                                " 	AND (TblTamirRequest.DisconnectDatePersian >= '" & lPDate1 & "') " &
                                " 	AND (TblTamirRequest.DisconnectDatePersian <= '" & lPDate2 & "') " &
                                " 	AND TblTamirRequest.FeederPartId IS NULL AND TblTamirRequest.LPPostId IS NULL"
                            RemoveMoreSpaces(lSQL)
                            BindingTable(lSQL, mCnn, mDsReq, "ViewRequestCount")

                            If mDsReq.Tables.Contains("ViewRequestCount") AndAlso mDsReq.Tables("ViewRequestCount").Rows.Count > 0 Then
                                lReqCount = mDsReq.Tables("ViewRequestCount").Rows(0)("Cnt")
                            End If

                            If (lDCCount + lReqCount) >= lMaxFeederDC Then

                                lMsg =
                                    "تعداد خاموشي بابرنامه روي فيدر " &
                                    cmbMPFeeder.Text &
                                    "، از حداکثر تعداد خاموشي مجاز براي آن فيدر (" & lMaxFeederDC & ") بيشتر شده است." & vbCrLf &
                                    "آيا درخواست را ثبت ميکنيد؟"
                                Dim lAns As DialogResult = MsgBoxF(lMsg, "هشدار", MessageBoxButtons.OKCancel, MsgBoxIcon.MsgIcon_Warning, MessageBoxDefaultButton.Button1)
                                If lAns = DialogResult.Cancel Then
                                    IsSaveOK = False
                                    Exit Function
                                Else
                                    lMsg = ""
                                End If

                            End If

                        End If
                    End If

                End If

            ElseIf mIsForConfirm And rbIsNotConfirm.Checked Then

                If chkIsEmergency.Checked Then

                    Dim lUser As String = ""
                    Dim lSQL As String = "SELECT * FROM Tbl_AreaUser WHERE AreaUserId = " & Val(mEditingRow("AreaUserId"))
                    BindingTable(lSQL, mCnn, mDs, "Tbl_AreaUser")
                    If mDs.Tables.Contains("Tbl_AreaUser") AndAlso mDs.Tables("Tbl_AreaUser").Rows.Count > 0 Then
                        lUser = mDs.Tables("Tbl_AreaUser").Rows(0)("UserName")
                    End If

                    lMsg =
                        "اين درخواست توسط کاربر <" & lUser & "> به صورت خاموشي بدون تأييد ثبت شده و خاموشي آن از قبل ايجاد شده و ممکن است انجام نيز شده باشد." & vbCrLf &
                        "آيا از عدم تأييد درخواست اطمينان داريد؟"
                    Dim lAns As DialogResult = MsgBoxF(lMsg, "هشدار", MessageBoxButtons.YesNo, MsgBoxIcon.MsgIcon_Exclamation, MessageBoxDefaultButton.Button1)
                    If lAns = DialogResult.No Then
                        IsSaveOK = False
                        Exit Function
                    Else
                        lMsg = ""
                    End If
                End If

            End If
        End If

        If lMsg = "" Then
            IsSaveOK = True
        Else
            lMsg = "لطفا موارد زير را تصحيح نماييد" & vbCrLf & lMsg.Replace("-", vbCrLf & "-")
            ShowError(lMsg, False, MsgBoxIcon.MsgIcon_Exclamation)
        End If

    End Function
    Private Function SaveInfo() As Boolean
        SaveInfo = False
        'If Not CheckEmergency() Then Exit Function
        If Not CheckUserAccess() Then Exit Function
        If Not IsSaveOK() Then Exit Function
        txtUnConfirmReason.Text = ReplaceFarsiToArabi(txtUnConfirmReason.Text)
        txtWarmLineReason.Text = ReplaceFarsiToArabi(txtWarmLineReason.Text)
        txtOperationDesc.Text = ReplaceFarsiToArabi(txtOperationDesc.Text)
        txtCriticalLocations.Text = ReplaceFarsiToArabi(txtCriticalLocations.Text)
        txtAsdresses.Text = ReplaceFarsiToArabi(txtAsdresses.Text)
        txtWorkingAddress.Text = ReplaceFarsiToArabi(txtWorkingAddress.Text)
        txtEmergency.Text = ReplaceFarsiToArabi(txtEmergency.Text)

        Dim lNow As CTimeInfo = GetServerTimeInfo()
        Dim lRow As DataRow
        Dim lDT As DateTime = lNow.MiladiDate
        Dim lPDate As String = lNow.ShamsiDate
        Dim lTime As String = lNow.HourMin

        Dim lManovrRequestId As Long = -1

        Dim lTrans As SqlTransaction

        Try
            mIsAutoChangeTamirType = True
            txtDateDisconnect.InsertTime(txtTimeDisconnect)
            txtDateConnect.InsertTime(txtTimeConnect)
            If mIsForceManovrDC Then
                txtDateDisconnectManovr.InsertTime(txtTimeDisconnectManovr)
                txtDateConnectManovr.InsertTime(txtTimeConnectManovr)
            End If
            If (mIsForConfirm And Not mIsForWarmLineConfirm) Or mIsSetadConfirmPlanned Then
                txtDateConfirm.InsertTime(txtTimeConfirm)
            End If
            If mIsForAllow Then
                txtDateAllowStart.InsertTime(txtTimeAllowStart)
                If txtDateAllowDE.IsOK Then
                    txtDateAllowEnd.InsertTime(txtTimeAllowEnd)
                End If
                txtDateAllowDE.InsertTime(txtTimeAllowDE)
            End If

            If mCnn.State <> ConnectionState.Open Then
                mCnn.Open()
            End If
            lTrans = mCnn.BeginTransaction

            Dim lState As TamirRequestStates = mEditingRow("TamirRequestStateId")
            '====================================
            'TblTamirRequest:
            lRow = mEditingRow

            SetDBValue(txtWorkCommandNo, lRow("WorkCommandNo"))
            SetDBValue(txtGISFormNo, lRow("GISFormNo"))
            SetDBValue(txtProjectNo, lRow("ProjectNo"))


            If rbInCityService.Checked Or rbNotInCityService.Checked Then
                SetDBValue(rbInCityService, lRow("IsInCityService"))
            Else
                lRow("IsInCityService") = DBNull.Value
            End If

            LogTamirNetworkType("Step 6")
            If Not pnlPostFeeder.Enabled Or Not cmbTamirNetworkType.Enabled Then
                mEditingRow("TamirNetworkTypeId") = mTamirNetworkTypeId
            End If

            If Not (mIsForConfirm And Not mIsForWarmLineConfirm) And Not mIsForAllow Then

                If Not mIsNewRequest Then
                    SetDBValue(txtTamirRequestNumber, lRow("TamirRequestNo"))
                Else
                    lRow("TamirRequestNo") = -1
                End If
                SetDBValue(cmbArea, lRow("AreaId"))
                SetDBValue(cmbCity, lRow("CityId"))
                lRow("Peymankar") = IIf(cmbPeymankar.Text.Trim.Length > 0, cmbPeymankar.Text, DBNull.Value)
                If Not rbIsRequestByFogheTozi.Checked Then
                    SetDBValue(rbIsRequestByPeymankar, lRow("IsRequestByPeymankar"))
                Else
                    lRow("IsRequestByPeymankar") = DBNull.Value
                End If
                SetDBValue(cmbPeymankar, lRow("PeymankarId"))
                SetDBValue(cmbMPPost, lRow("MPPostId"))
                SetDBValue(txtLetterNo, lRow("LetterNo"))
                SetDBValue(txtLetterDate, lRow("LetterDatePersian"))
                SetDBValue(txtOperationDesc, lRow("OperationDesc"))
                If cmbTamirNetworkType.SelectedValue <> TamirNetworkType.FT Then
                    SetDBValue(cmbMPFeeder, lRow("MPFeederId"))
                    Dim lDelArr As New ArrayList
                    For Each lDelRow As DataRow In mDs.TblTamirRequestFTFeeder.Rows
                        lDelArr.Add(lDelRow)
                    Next

                    For Each lDel As DataRow In lDelArr
                        lDel.Delete()
                    Next
                Else

                    Dim lMPFeeders() As String = ckcmbMPFeeder.GetDataListArray()
                    Dim lMPFeedersStr As String = "," & ckcmbMPFeeder.GetDataList() & ","
                    Dim lRows() As DataRow, lNewRow As DatasetTamir.TblTamirRequestFTFeederRow

                    If lMPFeeders.Length > 0 AndAlso lMPFeeders(0) <> "" Then
                        For Each lMPeederId As Integer In lMPFeeders
                            lRows = mDs.TblTamirRequestFTFeeder.Select("MPFeederId = " & lMPeederId)
                            If lRows.Length = 0 Then
                                lNewRow = mDs.TblTamirRequestFTFeeder.NewTblTamirRequestFTFeederRow
                                lNewRow.TamirRequestFTFeederId = GetAutoInc()
                                lNewRow.TamirRequestId = lRow("TamirRequestId")
                                lNewRow.MPFeederId = lMPeederId
                                mDs.TblTamirRequestFTFeeder.AddTblTamirRequestFTFeederRow(lNewRow)
                            End If
                        Next
                    End If

                    Dim lDelFTs As New ArrayList
                    For Each lFTRow As DataRow In mDs.TblTamirRequestFTFeeder.Rows
                        If lMPFeedersStr.IndexOf("," & lFTRow("MPFeederId") & ",") = -1 Then
                            lDelFTs.Add(lFTRow)
                        End If
                    Next
                    For Each lFTRow As DataRow In lDelFTs
                        lFTRow.Delete()
                    Next

                    If mDs.TblTamirRequestFTFeeder.Rows.Count > 0 Then
                        For Each lRowMPF As DataRow In mDs.TblTamirRequestFTFeeder.Rows
                            If lRowMPF.RowState <> DataRowState.Deleted Then
                                lRow("MPFeederId") = lRowMPF("MPFeederId")
                                Exit For
                            End If
                        Next
                    End If

                End If

                SetDBValue(cmbFeederPart, lRow("FeederPartId"))
                SetDBValue(cmbLPPost, lRow("LPPostId"))
                SetDBValue(cmbLPFeeder, lRow("LPFeederId"))
                SetDBValue(cmbCloserType, lRow("MPCloserTypeId"))
                SetDBValue(txtDateDisconnect, lRow("DisconnectDT"), , True)
                SetDBValue(txtDateDisconnect, lRow("DisconnectDatePersian"))
                SetDBValue(txtTimeDisconnect, lRow("DisconnectTime"))
                SetDBValue(txtDateConnect, lRow("ConnectDT"), , True)
                SetDBValue(txtDateConnect, lRow("ConnectDatePersian"))
                SetDBValue(txtTimeConnect, lRow("ConnectTime"))
                SetDBValue(txtDCInterval, lRow("DisconnectInterval"))
                If lblPowerUnit.Text.ToLower = "kwh" Then
                    lRow("DisconnectPower") = Val(txtPower.Text) / 1000
                Else
                    SetDBValue(txtPower, lRow("DisconnectPower"), 0)
                End If
                lRow("IsDCPowerMW") = True
                SetDBValue(txtLastPeakBar, lRow("LastMPFeederPeak"))
                SetDBValue(txtCurrentValue, lRow("CurrentValue"), 0)
                SetDBValue(txtCriticalLocations, lRow("CriticalLocations"))
                SetDBValue(txtAsdresses, lRow("CriticalsAddress"))
                SetDBValue(txtGpsX, lRow("GpsX"))
                SetDBValue(txtGpsY, lRow("GpsY"))

                If IsMobile(txtPeymankarTel.Text) Then
                    SetDBValue(txtPeymankarTel, lRow("PeymankarMobileNo"))
                Else
                    lRow("PeymankarMobileNo") = DBNull.Value
                End If

                SetDBValue(chkIsRemoteDCChange, lRow("IsRemoteDCChange"))
                'SetDBValue(cmbRequestUserName, lRow("AreaUserId"))
                SetDBValue(chkIsManoeuvre, lRow("IsManoeuvre"))
                If chkIsManoeuvre.Checked Then
                    SetDBValue(cmbManoeuvreType, lRow("ManoeuvreTypeId"))
                    SetDBValue(txtManoeuvreDesc, lRow("ManoeuvreDesc"))
                Else
                    lRow("ManoeuvreTypeId") = DBNull.Value
                    lRow("ManoeuvreDesc") = DBNull.Value
                End If
                SetDBValue(cmbWeather, lRow("WeatherId"))
                SetDBValue(chkIsWarmLine, lRow("IsWarmLine"))
                Dim lWarmLineStateId As WarmLineConfirmState

                If IsForceWarmLineConfirm() Then
                    If lState = mdlHashemi.TamirRequestStates.trs_PreNew AndAlso Not mIsForWarmLineConfirm Then
                        If Not IsDBNull(lRow("WarmLineConfirmStateId")) AndAlso lRow("WarmLineConfirmStateId") > lWarmLineStateId.wlcs_WaitForConfirm Then
                        Else
                            If chkIsWarmLine.Checked Then
                                lRow("WarmLineConfirmStateId") = lWarmLineStateId.wlcs_WaitForConfirm
                            Else
                                lRow("WarmLineConfirmStateId") = lWarmLineStateId.wlcs_Unknown
                            End If
                        End If
                    ElseIf mIsForWarmLineConfirm Then
                        If rbConfirmWL.Checked Then
                            lRow("WarmLineConfirmStateId") = lWarmLineStateId.wlcs_Confirm
                        ElseIf rbNotConfirmWL.Checked Then
                            lRow("WarmLineConfirmStateId") = lWarmLineStateId.wlcs_NotConfirm
                            lRow("NotConfirmWarmLineReason") = txtWarmLineReason.Text
                            lRow("IsWarmLine") = False
                        ElseIf rbIsReturnedWL.Checked Then
                            SetDBValue(rbIsReturnedWL, mEditingRow("IsReturned"))
                            SetDBValue(txtWarmLineReason, mEditingRow("ReturnDesc"))
                            lRow("WarmLineConfirmStateId") = lWarmLineStateId.wlcs_WaitForConfirm
                            mEditingRow("IsNeedManovrDC") = DBNull.Value

                            If mIsSendMessageForReturn Then
                                MakeMessageForUserReturn(mEditingRow("AreaUserId"), lTrans)
                            End If
                        End If
                        lRow("WarmLineConfirmAreaUserId") = WorkingUserId
                        lRow("WarmLineConfirmDT") = lNow.MiladiDate
                        lRow("WarmLineConfirmDatePersian") = lNow.ShamsiDate
                        lRow("WarmLineConfirmTime") = lNow.HourMin

                    End If
                End If

                lRow("SaturdayDT") = GetFirstDateOfTamirScope(lRow("DisconnectDT"))
                lRow("SaturdayDatePersian") = GetFirstDatePersianOfTamirScope(lRow("DisconnectDT"))
                SetDBValue(txtMaxPower, lRow("MaximumPower"))
                SetDBValue(cmbNazer, lRow("NazerId"))
                SetDBValue(chkIsConfirmNazer, lRow("IsConfirmNazer"))
                If Not chkIsConfirmNazer.Checked Then
                    lRow("ConfirmNazerAreaUserId") = DBNull.Value
                    lRow("ConfirmNazerDT") = DBNull.Value
                ElseIf IsDBNull(mEditingRow("ConfirmNazerAreaUserId")) Then
                    lRow("ConfirmNazerAreaUserId") = WorkingUserId
                    lRow("ConfirmNazerDT") = lNow.MiladiDate
                End If
                'lRow("WeakPowerId") = mEditingWeakPowerRow("WeakPowerId")
                SetDBValue(txtWorkingAddress, lRow("WorkingAddress"))
                LogTamirNetworkType("Step 7")
                SetDBValue(cmbTamirNetworkType, lRow("TamirNetworkTypeId"))

                SetDBValue(cmbTamirType, lRow("TamirTypeId"))
                CheckTamirType()

                SetDBValue(txtDCCurrent, lRow("DCCurrentValue"))

                SetDBValue(cmbReferTo, lRow("ReferToId"))
                SetDBValue(cmbDepartment, lRow("DepartmentId"))
                SetDBValue(cmbTamirRequestSubject, lRow("TamirRequestSubjectId"))

                If chkIsSendToSetad.Checked Then
                    If Not IsCenter Or IsMiscMode Then
                        lRow("ConfirmNahiehAreaUserId") = WorkingUserId
                    ElseIf IsCenter Then
                        lRow("ConfirmCenterAreaUserId") = WorkingUserId
                    End If

                    Dim lTi As New CTimeInfo
                    If IsCenter Or IsMiscMode Then
                        lRow("SendSetadDT") = lTi.MiladiDate
                        lRow("SendSetadDatePersian") = lTi.ShamsiDate
                        lRow("SendSetadTime") = lTi.HourMin
                    Else
                        lRow("SendCenterDT") = lTi.MiladiDate
                        lRow("SendCenterDatePersian") = lTi.ShamsiDate
                        lRow("SendCenterTime") = lTi.HourMin
                    End If
                End If

                SetDBValue(chkIsEmergency, lRow("IsEmergency"))

                If chkIsEmergency.Checked Then
                    SetDBValue(txtEmergencyReason, lRow("EmergencyReason"))
                ElseIf Not chkIsEmergency.Checked AndAlso cmbTamirType.SelectedValue = TamirTypes.Ezterari Then
                    SetDBValue(txtEmergency, lRow("EmergencyReason"))
                Else
                    lRow("EmergencyReason") = DBNull.Value
                End If

                If chkIsEmergency.Checked AndAlso IsDBNull(lRow("EmergencyRequestId")) Then 'AndAlso mIsNewRequest 
                    lRow("EmergencyRequestId") = MakeDisconnectRequestId(lTrans)
                    If mIsForceManovrDC Then
                        lManovrRequestId = MakeDisconnectManovrId(lRow("EmergencyRequestId"), lTrans)
                        mEditingRowManovr("RequestId") = lManovrRequestId
                    End If
                End If

                Dim lNewState As TamirRequestStates = lRow("TamirRequestStateId")

                If chkIsSendToSetad.Checked Or (lRow("TamirRequestStateId") = TamirRequestStates.trs_PreNew And IsSetadMode) Then 'AndAlso lRow("TamirRequestStateId") = TamirRequestStates.trs_PreNew Then
                    If lRow("TamirRequestStateId") = TamirRequestStates.trs_PreNew Then
                        If Not IsCenter And Not IsMiscMode Then
                            If chkIsEmergency.Checked Or mIsTamirDirectToSetad Then
                                lNewState = mdlHashemi.TamirRequestStates.trs_WaitForSetad
                            Else
                                lNewState = mdlHashemi.TamirRequestStates.trs_WaitFor121
                            End If
                        Else
                            If mIsFogheToziUser AndAlso Not chkIsSendToSetad.Checked Then
                                lNewState = mdlHashemi.TamirRequestStates.trs_PreNew
                            Else
                                lNewState = mdlHashemi.TamirRequestStates.trs_WaitForSetad
                            End If
                        End If
                        'mEditingRow("IsReturned") = False
                    ElseIf IsCenter And Not IsSetadMode And lRow("TamirRequestStateId") = TamirRequestStates.trs_WaitFor121 Then
                        lNewState = mdlHashemi.TamirRequestStates.trs_WaitForSetad
                    End If
                    lRow("TamirRequestStateId") = lNewState
                    cmbState.SelectedValue = lNewState
                End If

                SetDBValue(cmbState, lRow("TamirRequestStateId"))
            End If

            mEditingRow("OverlapTime") = mOverlapTime
            mEditingRow("DisconnectPowerECO") = mDCPowerECO

            '-------------------
            'TblTamirRequestDisconnect:
            mEditingRow("IsNeedManovrDC") = mIsForceManovrDC
            If mIsForceManovrDC Then
                SetDBValue(txtDateDisconnectManovr, mEditingRowManovr("DisconnectDT"), , True)
                SetDBValue(txtDateDisconnectManovr, mEditingRowManovr("DisconnectDatePersian"))
                SetDBValue(txtTimeDisconnectManovr, mEditingRowManovr("DisconnectTime"))
                SetDBValue(txtDateConnectManovr, mEditingRowManovr("ConnectDT"), , True)
                SetDBValue(txtDateConnectManovr, mEditingRowManovr("ConnectDatePersian"))
                SetDBValue(txtTimeConnectManovr, mEditingRowManovr("ConnectTime"))
                SetDBValue(txtDCIntervalManovr, mEditingRowManovr("DisconnectInterval"))
                If lblPowerUnitManovr.Text.ToLower = "kwh" Then
                    mEditingRowManovr("DisconnectPower") = Val(txtPower.Text) / 1000
                Else
                    SetDBValue(txtPowerManovr, mEditingRowManovr("DisconnectPower"), 0)
                End If
                SetDBValue(txtCurrentValueManovr, mEditingRowManovr("CurrentValue"), 0)
            Else
                mEditingRowManovr("DisconnectDT") = DBNull.Value
                mEditingRowManovr("DisconnectDatePersian") = DBNull.Value
                mEditingRowManovr("DisconnectTime") = DBNull.Value
                mEditingRowManovr("ConnectDT") = DBNull.Value
                mEditingRowManovr("ConnectDatePersian") = DBNull.Value
                mEditingRowManovr("ConnectTime") = DBNull.Value
                mEditingRowManovr("DisconnectInterval") = DBNull.Value
                mEditingRowManovr("DisconnectPower") = DBNull.Value
                mEditingRowManovr("CurrentValue") = DBNull.Value
            End If

            '-------------------
            'TblTamirRequestConfirm:
            lRow = mEditingRowConfirm
            If (mIsSetadConfirmPlanned Or (mIsForConfirm And Not mIsForWarmLineConfirm)) AndAlso (rbIsConfirm.Checked Or rbIsNotConfirm.Checked) Then
                If mEditingRowConfirm Is Nothing Then
                    mEditingRowConfirm = mDs.TblTamirRequestConfirm.NewRow
                    lRow = mEditingRowConfirm
                    lRow("TamirRequestId") = mEditingRow("TamirRequestId")
                End If

                SetDBValue(rbIsConfirm, lRow("IsConfirm"))
                SetDBValue(rbIsNotConfirm, Not lRow("IsConfirm"))
                SetDBValue(txtDateConfirm, lRow("ConfirmDT"), , True)
                SetDBValue(txtDateConfirm, lRow("ConfirmDatePersian"))
                SetDBValue(txtTimeConfirm, lRow("ConfirmTime"))
                SetDBValue(txtUnConfirmReason, lRow("UnConfirmReason"))
                SetDBValue(cmbConfirmUserName, lRow("AreaUserId"))
                SetDBValue(chkIsSendSMSSensitive, lRow("IsSendSMSSensitive"))

                If mIsNewConfirm AndAlso rbIsConfirm.Checked AndAlso IsDBNull(lRow("RequestId")) Then
                    If IsDBNull(mEditingRow("IsEmergency")) OrElse (Not mEditingRow("IsEmergency") And IsDBNull(mEditingRow("EmergencyRequestId"))) Then
                        lRow("RequestId") = MakeDisconnectRequestId(lTrans)
                        If mIsForceManovrDC Then
                            lManovrRequestId = MakeDisconnectManovrId(lRow("RequestId"), lTrans)
                            mEditingRowManovr("RequestId") = lManovrRequestId
                        End If
                    Else
                        lRow("RequestId") = mEditingRow("EmergencyRequestId")
                    End If
                End If

                If rbIsConfirm.Checked Then

                    If Not IsSetadMode AndAlso mEditingRow("TamirRequestStateId") = TamirRequestStates.trs_PreNew Then
                        mEditingRow("IsSpecialConfirm") = True
                    End If

                    If mEmergency_EndJobStateId = mdlHashemi.EndJobStates.ejs_Done Or mEmergency_EndJobStateId = mdlHashemi.EndJobStates.ejs_TemporaryDone Then
                        mEditingRow("TamirRequestStateId") = TamirRequestStates.trs_Finish
                    ElseIf mEmergency_EndJobStateId = mdlHashemi.EndJobStates.ejs_Working And Not IsDBNull(mEmergency_MPRequestId) Then
                        mEditingRow("TamirRequestStateId") = TamirRequestStates.trs_Disconnected
                    Else
                        mEditingRow("TamirRequestStateId") = TamirRequestStates.trs_Confirm
                    End If

                    If chkIsSendSMSSensitive.Checked AndAlso Not chkIsWarmLine.Checked Then

                        '-------------------
                        'TblRequestInform
                        mEditingRowRequestInform = mDsReq.TblRequestInform.NewRow
                        mEditingRowRequestInform("RequestInformId") = GetAutoInc()
                        mEditingRowRequestInform("CreateDT") = lNow.MiladiDate
                        mEditingRowRequestInform("CreateDatePersian") = lNow.ShamsiDate
                        mEditingRowRequestInform("CreateTime") = lNow.HourMin
                        mEditingRowRequestInform("AreaUserId") = WorkingUserId
                        mEditingRowRequestInform("RequestId") = mEditingRowConfirm("RequestId")
                        mEditingRowRequestInform("SendSMSTypeId") = SendSMSTypes.smst_Both
                        mEditingRowRequestInform("SendFaxTypeId") = SendFaxtypes.faxt_None
                        mEditingRowRequestInform("SendEmailTypeId") = SendEmailTypes.emailt_None
                        mEditingRowRequestInform("IsShowFromWebSite") = False
                        mEditingRowRequestInform("IsSend_IRIB") = False
                        mEditingRowRequestInform("RequestInformJobStateId") = RequestInformJobStates.rijs_Form

                        mDsReq.TblRequestInform.Rows.Add(mEditingRowRequestInform)

                        If Not mIsThereFeederKey AndAlso mDsReq.TblRequestPostFeeder.Rows.Count > 0 Then
                            '-------------------
                            'TblSubscriberInform
                            Dim lLocationTypeId As LocationTypes = mDsReq.TblRequestPostFeeder(0).LocationTypeId

                            Dim lSubscribers As New ArrayList
                            lSubscribers = LoadSubscribers(mEditingRowConfirm("RequestId"), lLocationTypeId, lTrans)
                            mDsReq.TblSubscriberInfom.Clear()
                            If Not mDsReq.TblSubscriberInfom.Columns.Contains("IsSendSMSAfterConnect") Then
                                mDsReq.TblSubscriberInfom.Columns.Add("IsSendSMSAfterConnect", GetType(Boolean))
                            End If
                            If Not mDsReq.TblSubscriberInfom.Columns.Contains("MediaTypeId") Then
                                mDsReq.TblSubscriberInfom.Columns.Add("MediaTypeId", GetType(Integer))
                            End If
                            If Not mDsReq.TblSubscriberInfom.Columns.Contains("OutgoingCallStatusId") Then
                                mDsReq.TblSubscriberInfom.Columns.Add("OutgoingCallStatusId", GetType(Integer))
                            End If
                            If Not mDsReq.TblSubscriberInfom.Columns.Contains("OutgoingCallTimeFrom") Then
                                mDsReq.TblSubscriberInfom.Columns.Add("OutgoingCallTimeFrom", GetType(String))
                            End If
                            If Not mDsReq.TblSubscriberInfom.Columns.Contains("OutgoingCallTimeTo") Then
                                mDsReq.TblSubscriberInfom.Columns.Add("OutgoingCallTimeTo", GetType(String))
                            End If
                            If Not mDsReq.TblSubscriberInfom.Columns.Contains("ResultDesc") Then
                                mDsReq.TblSubscriberInfom.Columns.Add("ResultDesc", GetType(String))
                            End If


                            For Each lSubscribrtId As String In lSubscribers
                                mEditingRowRequestInform("SendSMSTypeId") = SendSMSTypes.smst_Sensitive
                                mEditingRowSubscriberInform = mDsReq.TblSubscriberInfom.NewTblSubscriberInfomRow
                                mEditingRowSubscriberInform("SubscriberInformId") = GetAutoInc()
                                mEditingRowSubscriberInform("RequestInformId") = mEditingRowRequestInform("RequestInformId")
                                mEditingRowSubscriberInform("SubscriberId") = lSubscribrtId
                                mEditingRowSubscriberInform("SendSMSStatusId") = SendSMSStatus.smss_NotSent
                                mEditingRowSubscriberInform("SendFaxStatusId") = SendFaxStatus.faxs_Nothing
                                mEditingRowSubscriberInform("SendEmailStatusId") = SendEmailStatus.emails_Nothing
                                mEditingRowSubscriberInform("OutgoingCallStatusId") = SendOutgoingCallStatus.telephone_Nothing
                                mEditingRowSubscriberInform("SendSMSDT") = DBNull.Value
                                mEditingRowSubscriberInform("SendFaxDT") = DBNull.Value
                                mEditingRowSubscriberInform("SendEmailDT") = DBNull.Value
                                mEditingRowSubscriberInform("IsSendSMSAfterConnect") = DBNull.Value
                                mEditingRowSubscriberInform("MediaTypeId") = MediaTypes.SMS
                                mEditingRowSubscriberInform("OutgoingCallTimeFrom") = DBNull.Value
                                mEditingRowSubscriberInform("OutgoingCallTimeTo") = DBNull.Value
                                mEditingRowSubscriberInform("ResultDesc") = DBNull.Value

                                mDsReq.TblSubscriberInfom.AddTblSubscriberInfomRow(mEditingRowSubscriberInform)
                            Next

                        End If
                    End If
                Else
                    mEditingRow("TamirRequestStateId") = TamirRequestStates.trs_NotConfirm
                End If
            ElseIf (mIsForConfirm And Not mIsForWarmLineConfirm) And rbIsReturned.Checked Then
                SetDBValue(rbIsReturned, mEditingRow("IsReturned"))
                SetDBValue(txtUnConfirmReason, mEditingRow("ReturnDesc"))
                mEditingRow("TamirRequestStateId") = TamirRequestStates.trs_PreNew
                mEditingRow("IsNeedManovrDC") = DBNull.Value
                If mIsSendMessageForReturn Then
                    MakeMessageForUserReturn(mEditingRow("AreaUserId"), lTrans)
                End If
            ElseIf mIsForWarmLineConfirm And rbIsReturnedWL.Checked Then
                mEditingRow("TamirRequestStateId") = TamirRequestStates.trs_PreNew
            End If
            '----------------omid
            ReturnDT()

            '-------------------
            'TblTamirRequestAllow:
            'lRow = mEditingRowAllow
            'If mIsForAllow AndAlso (txtAllowNumber.Text.Trim.Length > 0 Or chkIsAutoNumber.Checked) Then
            '    If mEditingRowAllow Is Nothing Then
            '        mEditingRowAllow = mDs.TblTamirRequestAllow.NewRow
            '        lRow = mEditingRowAllow
            '        lRow("TamirRequestId") = mEditingRow("TamirRequestId")
            '    End If

            '    If Not chkIsAutoNumber.Checked Then
            '        SetDBValue(txtAllowNumber, lRow("AllowNumber"))
            '    ElseIf Not lRow("IsAutoNumber") Or IsDBNull(lRow("AllowNumber")) Then
            '        'Dim lAllowNumber As Long = GetAutoNumber("TblBazdid", mCnn, lTrans)
            '        'lRow("AllowNumber") = lAllowNumber
            '        'lRow("IsAutoNumber") = True
            '    End If
            '    SetDBValue(txtDateAllowStart, lRow("AllowDT"), , True)
            '    SetDBValue(txtDateAllowStart, lRow("AllowDatePersian"))
            '    SetDBValue(txtTimeAllowStart, lRow("AllowTime"))
            '    SetDBValue(txtDateAllowEnd, lRow("AllowEndDT"), , True)
            '    SetDBValue(txtDateAllowEnd, lRow("AllowEndDatePersian"))
            '    SetDBValue(txtTimeAllowEnd, lRow("AllowEndTime"))
            '    SetDBValue(cmbAllowUserName, lRow("AreaUserId"))
            '    SetDBValue(txtDateAllowDE, lRow("DEDT"), , True)
            '    SetDBValue(txtDateAllowDE, lRow("DEDatePersian"))
            '    SetDBValue(txtTimeAllowDE, lRow("DETime"))

            '    mEditingRow("TamirRequestStateId") = TamirRequestStates.trs_Allowed
            'End If

            '====================================

            If (mIsSetadConfirmPlanned Or (mIsForConfirm And Not mIsForWarmLineConfirm)) AndAlso (rbIsConfirm.Checked Or rbIsNotConfirm.Checked) Then
                MakeMessageForUser(mEditingRow("AreaUserId"), rbIsConfirm.Checked, lTrans)
            End If

            If mIsNewRequest And Not mIsAddRow Then
                mDs.Tables("TblTamirRequest").Rows.Add(mEditingRow)
                mIsAddRow = True
            End If
            If mIsNewManovr And Not mIsAddRowManovr Then
                mDs.TblTamirRequestDisconnect.AddTblTamirRequestDisconnectRow(mEditingRowManovr)
                mIsAddRowManovr = True
            End If
            If (mIsSetadConfirmPlanned Or (mIsForConfirm And Not mIsForWarmLineConfirm)) And mIsNewConfirm And Not mIsAddRowConfirm And Not rbIsReturned.Checked Then
                mDs.TblTamirRequestConfirm.Rows.Add(mEditingRowConfirm)
                mIsAddRowConfirm = True
            End If
            If mIsForAllow And mIsNewAllow And Not mIsAddRowAllow Then
                mDs.TblTamirRequestAllow.Rows.Add(mEditingRowAllow)
                mIsAddRowAllow = True
            End If

            Dim lUpdate As New frmUpdateDataset
            Dim lIsSaveOK As Boolean = True

            If Not IsNothing(mEditingWeakPowerRow) And (mIsSetadConfirmPlanned Or (mIsForConfirm And Not mIsForWarmLineConfirm)) And rbIsConfirm.Checked And Not rbIsNotConfirm.Checked Then
                lIsSaveOK = lUpdate.UpdateDataSet("TblWeakPower", mDs, mEditingRow("AreaId"), , , lTrans)
                If lIsSaveOK Then mEditingRow("WeakPowerId") = mEditingWeakPowerRow("WeakPowerId")
            End If
            If lIsSaveOK Then
                LogTamirNetworkType("Step 8")
                If Not pnlPostFeeder.Enabled Or Not cmbTamirNetworkType.Enabled Or (mIsForConfirm And Not mIsForWarmLineConfirm) Or mIsForAllow Then
                    cmbTamirNetworkType.SelectedValue = mTamirNetworkTypeId
                    mEditingRow("TamirNetworkTypeId") = mTamirNetworkTypeId
                End If
                '
                'TamirNetworkTypeId = 3 
                'AND NOT MPFeederId IS NULL
                'AND LPPostId IS NULL
                '
                If _
                    mEditingRow("TamirNetworkTypeId") = TamirNetworkType.LP AndAlso
                    Not IsDBNull(mEditingRow("MPFeederId")) AndAlso
                    IsDBNull(mEditingRow("LPPostId")) _
                Then
                    mEditingRow("TamirNetworkTypeId") = TamirNetworkType.MP
                End If

                lIsSaveOK = lUpdate.UpdateDataSet("TblTamirRequest", mDs, mEditingRow("AreaId"), , , lTrans)
                If mTamirRequestId = -1 Then
                    SaveLog("New Saved TamirRequestId: " & mEditingRow("TamirRequestId"), "TNT.log")
                    Try
                        SaveLog("New Saved TamirRequestNo: " & mEditingRow("TamirRequestNo"), "TNT.log")
                    Catch ex As Exception
                        SaveLog("New Saved TamirRequestNo: " & ex.Message, "TNT.log")
                    End Try
                End If
                LogTamirNetworkType("Step 9")
            End If
            If lIsSaveOK And mDs.TblTamirRequestDisconnect.Count > 0 Then
                Dim lDelArr As New ArrayList
                For Each lTRDRow As DataRow In mDs.TblTamirRequestDisconnect.Rows
                    If Not mIsForceManovrDC Then lDelArr.Add(lTRDRow)
                Next
                For Each lTRDRow As DataRow In lDelArr
                    lTRDRow.Delete()
                Next
                For Each lTRDRow As DataRow In mDs.TblTamirRequestDisconnect.Rows
                    If lTRDRow.RowState <> DataRowState.Deleted AndAlso lTRDRow.RowState <> DataRowState.Detached Then
                        lTRDRow("TamirRequestId") = mEditingRow("TamirRequestId")
                    End If
                Next
                lIsSaveOK = lUpdate.UpdateDataSet("TblTamirRequestDisconnect", mDs, mEditingRow("AreaId"), , , lTrans)
            End If
            If lIsSaveOK Then
                For Each lOLRow As DataRow In mDs.TblDCOverlap.Rows
                    If lOLRow.RowState <> DataRowState.Deleted Then
                        lOLRow("TamirRequestId") = mEditingRow("TamirRequestId")
                    End If
                Next
                lIsSaveOK = lUpdate.UpdateDataSet("TblDCOverlap", mDs, mEditingRow("AreaId"), , , lTrans)
            End If
            If lIsSaveOK Then
                For Each lFTRow As DataRow In mDs.TblTamirRequestFTFeeder.Rows
                    If lFTRow.RowState <> DataRowState.Deleted Then
                        lFTRow("TamirRequestId") = mEditingRow("TamirRequestId")
                    End If
                Next
                lIsSaveOK = lUpdate.UpdateDataSet("TblTamirRequestFTFeeder", mDs, mEditingRow("AreaId"), , , lTrans)
            End If

            If lIsSaveOK Then
                Dim lDelArray As New ArrayList
                For Each lFeederKeyRow As DataRow In DatasetTemp1.TblTamirRequestKey.Rows
                    If lFeederKeyRow.RowState <> DataRowState.Deleted Then
                        If lFeederKeyRow("KeyStateId") = 0 Then
                            lDelArray.Add(lFeederKeyRow)
                        End If
                        lFeederKeyRow("TamirRequestId") = mEditingRow("TamirRequestId")
                    End If
                Next

                For Each lDelRow As DataRow In lDelArray
                    lDelRow.Delete()
                Next
                If DatasetTemp1.TblTamirRequestKey.Rows.Count > 0 Then
                    lIsSaveOK = lUpdate.UpdateDataSet("TblTamirRequestKey", DatasetTemp1, mEditingRow("AreaId"), , , lTrans, , , , , True)
                End If
            End If

            If lIsSaveOK Then
                For Each lMSRow As DataRow In mDs.TblTamirRequestMultiStep.Rows
                    If lMSRow.RowState <> DataRowState.Deleted Then
                        lMSRow("TamirRequestId") = mEditingRow("TamirRequestId")
                    End If
                Next
                lIsSaveOK = lUpdate.UpdateDataSet("TblTamirRequestMultiStep", mDs, mEditingRow("AreaId"), , , lTrans)
                For Each lFeederKeyRow As DataRow In DatasetTemp1.TblTamirRequestKey.Rows
                    If lFeederKeyRow.RowState <> DataRowState.Deleted Then
                        If Not IsDBNull(lFeederKeyRow("TamirRequestMultiStepId")) Then
                            lFeederKeyRow("TamirRequestMultiStepId") = lUpdate.GetNewPrimaryKeyID(lFeederKeyRow("TamirRequestMultiStepId"))
                        End If
                    End If
                Next
            End If
            If lIsSaveOK AndAlso Not mEditingRowConfirm Is Nothing Then
                mEditingRowConfirm("TamirRequestId") = mEditingRow("TamirRequestId")
                lIsSaveOK = lUpdate.UpdateDataSet("TblTamirRequestConfirm", mDs, mEditingRow("AreaId"), , False, lTrans)
            End If
            If lIsSaveOK AndAlso Not mEditingRowRequestInform Is Nothing Then
                mEditingRowRequestInform("RequestId") = mEditingRowConfirm("RequestId")
                lIsSaveOK = lUpdate.UpdateDataSet("TblRequestInform", mDsReq, mEditingRow("AreaId"), , , lTrans)
            End If
            If lIsSaveOK AndAlso mDsReq.TblSubscriberInfom.Rows.Count > 0 Then
                For Each lSubRow As DataRow In mDsReq.TblSubscriberInfom.Rows
                    lSubRow("RequestInformId") = mEditingRowRequestInform("RequestInformId")
                Next
                lIsSaveOK = lUpdate.UpdateDataSet("TblSubscriberInfom", mDsReq, mEditingRow("AreaId"), , , lTrans)
            End If
            If lIsSaveOK AndAlso Not mEditingRowAllow Is Nothing Then
                mEditingRowAllow("TamirRequestId") = mEditingRow("TamirRequestId")
                lIsSaveOK = lUpdate.UpdateDataSet("TblTamirRequestAllow", mDs, mEditingRow("AreaId"), , False, lTrans)
            End If
            If lIsSaveOK Then
                lIsSaveOK = SaveOPeratins(lTrans)
            End If

            If lIsSaveOK Then
                If DatasetTemp1.TblTamirRequestKey.Rows.Count > 0 Then
                    lIsSaveOK = lUpdate.UpdateDataSet("TblTamirRequestKey", DatasetTemp1, mEditingRow("AreaId"), , , lTrans)
                End If
            End If

            If lIsSaveOK Then
                lTrans.Commit()

                If mIsShowGISKeyInform Then
                    Dim lDlg As New frmInformMethod(mEditingRowConfirm("RequestId"), mEditingRowRequestInform("RequestInformId"), mTamirRequestId)
                    lDlg.ShowDialog()
                End If
            Else
                lTrans.Rollback()
                ShowError("درخواست شما قابل ذخيره شدن نمي‌باشد")
            End If

            SaveInfo = lIsSaveOK
            mIsAutoChangeTamirType = False
        Catch ex As Exception
            Try
                If Not lTrans Is Nothing Then
                    lTrans.Rollback()
                End If
            Catch ex1 As Exception
            End Try
            ShowError(ex)
        Finally
            mIsAutoChangeTamirType = False
        End Try

        Try
            If mCnn.State = ConnectionState.Open Then
                mCnn.Close()
            End If
        Catch ex As Exception
        End Try

    End Function
    '-------------omid
    Private Sub Check_ReturnTime(ByRef aNow As CTimeInfo)
        If pnlConfirm.Enabled Then Exit Sub
        If Not mEditingRow("TamirRequestStateId") = 2 Then Exit Sub
        If IsDBNull(mEditingRow("ReturnTimeoutDT")) Then Exit Sub
        Dim lUpdate As New frmUpdateDataset
        Dim lRemainingTime As New CTimeInfo(mEditingRow("DisconnectDT"))

        If aNow.MiladiDate > mEditingRow("ReturnTimeoutDT") Or
            mEditingRow("DisconnectDT") < mEditingRow("ReturnTimeoutDT") Then
            ShowError("وقت رسیدگی به عودت تمام شده است." & vbCrLf & "این پرونده به حالت عدم تایید تغییر وضعیت داد!")
            mEditingRow("TamirRequestStateId") = 8 '-----عدم تایید
            lUpdate.UpdateDataSet("TblTamirRequest", mDs, mEditingRow("AreaId"))
            btnSave.Enabled = False
        Else
            ShowInfo("شما تا تاریخ " & lRemainingTime.ShamsiDate & "و زمان  " &
                     lRemainingTime.HourMin & "برای تغییرات وقت دارید.")
        End If
    End Sub
    Private Function CheckWarmLineFeeder() As Boolean
        If Not mIsForConfirm Then
            Return True
        End If
        Dim lWhere As String = ""
        Dim lSQL As String = ""
        Dim lDs As New DataSet

        lSQL = "SELECT * From TblTamirRequest Where TamirRequestNo = " & txtTamirRequestNumber.Text
        BindingTable(lSQL, mCnn, lDs, "TblTamirRequest")
        If lDs.Tables("TblTamirRequest").Rows.Count > 0 Then
            Dim lDisDate As String = lDs.Tables("TblTamirRequest").Rows(0).Item("DisconnectDatePersian")
            Dim lConDate As String = lDs.Tables("TblTamirRequest").Rows(0).Item("ConnectDatePersian")
            Dim lDisTime As String = lDs.Tables("TblTamirRequest").Rows(0).Item("DisconnectTime")
            Dim lConTime As String = lDs.Tables("TblTamirRequest").Rows(0).Item("connectTime")

            lWhere &= " AND (TblRequest.DisconnectDatePersian >=  '" & lDisDate & "' " &
                      " AND TblRequest.ConnectDatePersian <= '" & lConDate & "' )" &
                      " AND ((TblRequest.DisconnectTime >= '" & lDisTime & "')" &
                      " AND (TblRequest.connectTime <= '" & lConTime & "'))"
        End If

        If cmbMPFeeder.SelectedIndex > -1 Then
            lWhere &= " AND MPFeederId =" & cmbMPFeeder.SelectedValue
        ElseIf ckcmbMPFeeder.GetDataList.Length > 0 Then
            lWhere &= " AND MPFeederId IN ( '" & ckcmbMPFeeder.GetDataList & "')"
        End If

        lSQL = "SELECT TblMPRequest.EndJobStateId,IsWarmLine,* FROM TblRequest" &
                " INNER JOIN TblMPRequest on TblRequest.MPRequestId = TblMPRequest.MPRequestId" &
                " WHERE TblMPRequest.EndJobStateId  IN (4,5)  AND  IsWarmLine = 1 " &
                 lWhere

        BindingTable(lSQL, mCnn, lDs, "TblFeederWarmLine")
        If lDs.Tables("TblFeederWarmLine").Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Function SaveOPeratins(ByVal aTrans As SqlTransaction) As Boolean
        SaveOPeratins = False

        Try

            Dim lRow As DataRow, lRows() As DataRow
            Dim lGRow As Janus.Windows.GridEX.GridEXRow
            Dim lNewRow As DatasetTamir.TblTamirOperationListRow

            For Each lRow In mDsView.ViewTamirOperationList.Rows
                lGRow = dgOperations.GetRow(lRow)

                lRows = mDs.TblTamirOperationList.Select("TamirOperationId = " & lRow("TamirOperationId"))

                If lGRow.Cells("IsSelected").Value = True Then
                    If lRows.Length > 0 Then
                        lRows(0)("Duration") = lRow("Duration")
                        lRows(0)("OperationCount") = IIf(IsDBNull(lRow("OperationCount")), 1, lRow("OperationCount"))
                        lRows(0)("PeymankarId") = lRow("PeymankarId")
                        lRows(0)("PeoplesCount") = lRow("PeoplesCount")
                    Else
                        lNewRow = mDs.TblTamirOperationList.NewTblTamirOperationListRow
                        lNewRow("TamirOperationListId") = GetAutoInc()
                        lNewRow("TamirRequestId") = mEditingRow("TamirRequestId")
                        lNewRow("TamirOperationId") = lRow("TamirOperationId")
                        lNewRow("Duration") = lRow("Duration")
                        lNewRow("OperationCount") = IIf(IsDBNull(lRow("OperationCount")), 1, lRow("OperationCount"))
                        lNewRow("PeymankarId") = lRow("PeymankarId")
                        lNewRow("PeoplesCount") = lRow("PeoplesCount")
                        mDs.TblTamirOperationList.AddTblTamirOperationListRow(lNewRow)
                    End If
                Else
                    If lRows.Length > 0 Then
                        lRows(0).Delete()
                    End If
                End If

            Next

            'If mDs.TblTamirOperationList.Count = 0 Then
            '    Throw New Exception("- لطفا فهرست عمليات هنگام خاموشي را مشخص نماييد")
            'End If

            Dim lIsSaveOK As Boolean
            Dim lUpdate As New frmUpdateDataset
            lIsSaveOK = lUpdate.UpdateDataSet("TblTamirOperationList", mDs, mEditingRow("AreaId"), , , aTrans)

            SaveOPeratins = lIsSaveOK
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Function MakeDisconnectRequestId(ByVal aTrans As SqlTransaction) As Long
        MakeDisconnectRequestId = -1

        Dim lNow As CTimeInfo = GetServerTimeInfo()

        Dim lIsSaveOK As Boolean = False
        Dim lRequestId As Long = -1

        Dim lIsNullUser As Boolean = True
        Dim lIsRequestUser As Boolean = True

        Try
            lIsNullUser = CConfig.ReadConfig("IsTamirNullUser", True)
        Catch ex As Exception
        End Try

        If lIsNullUser Then
            Try
                lIsRequestUser = CConfig.ReadConfig("IsTamirReqUser", True)
            Catch ex As Exception
            End Try
        End If

        Dim lUserId As Object = IIf(lIsNullUser, IIf(lIsRequestUser, mEditingRow("AreaUserId"), DBNull.Value), WorkingUserId)
        'If WorkingAreaId <> 99 Then
        '    Dim lSQL As String
        '    Try
        '        lSQL = "SELECT * FROM Tbl_AreaUser WHERE AreaId = 99 AND LOWER(UserName) = N'admin'"
        '        BindingTable(lSQL, mCnn, mDs, "Tbl_AreaUser", , , , , aTrans)
        '        If mDs.Tables("Tbl_AreaUser").Rows.Count > 0 Then
        '            lUserId = mDs.Tables("Tbl_AreaUser").Rows(0)("AreaUserId")
        '        End If
        '    Catch ex1 As Exception
        '    End Try
        'End If

        If Not mDsReq.TblRequestInfo.Columns.Contains("IsNotNeedParts") Then
            mDsReq.TblRequestInfo.Columns.Add("IsNotNeedParts")
        End If

        If Not mDsReq.TblRequestInfo.Columns.Contains("NotNeedPartsReason") Then
            mDsReq.TblRequestInfo.Columns.Add("NotNeedPartsReason")
        End If

        If Not mDsReq.TblRequestInfo.Columns.Contains("IsEnterPartsLater") Then
            mDsReq.TblRequestInfo.Columns.Add("IsEnterPartsLater")
        End If

        Dim lTbl As DatasetCcRequester.TblRequestDataTable = mDsReq.TblRequest
        Dim lRow As DatasetCcRequester.TblRequestRow = lTbl.NewTblRequestRow

        Dim lTblInfo As DatasetCcRequester.TblRequestInfoDataTable = mDsReq.TblRequestInfo
        Dim lRowInfo As DatasetCcRequester.TblRequestInfoRow = lTblInfo.NewTblRequestInfoRow

        Dim lTblPF As DatasetCcRequester.TblRequestPostFeederDataTable = mDsReq.TblRequestPostFeeder
        Dim lRowPF As DatasetCcRequester.TblRequestPostFeederRow = Nothing

        '-------------------
        'TblRequest:

        lRow("RequestId") = GetAutoInc()
        'lRow("CallId") = ""
        'lRow("LPRequestId") = ""
        'lRow("MPRequestId") = ""
        'lRow("SecretaryId") = ""
        lRow("AreaUserId") = lUserId 'WorkingUserId 'mEditingRow("AreaUserId")
        'lRow("DataEntrancePersonName") = ""
        lRow("DisconnectDT") = mEditingRow("DisconnectDT")
        lRow("DisconnectDatePersian") = mEditingRow("DisconnectDatePersian")
        lRow("DisconnectTime") = mEditingRow("DisconnectTime")
        'lRow("SubscriberId") = ""
        Dim lPeymankar As String = ""
        If Not IsDBNull(mEditingRow("Peymankar")) Then
            lPeymankar = mEditingRow("Peymankar")
        End If
        If lPeymankar.Length > 50 Then
            lPeymankar = lPeymankar.Substring(0, 49)
        End If
        lRow("SubscriberName") = lPeymankar
        lRow("CityId") = mEditingRow("CityId")
        Dim lIsUseCriticalsAddress As Boolean
        lIsUseCriticalsAddress = CConfig.ReadConfig("IsUseCriticalsAddressForDC", False)
        Dim lAddress As String = ""
        If lIsUseCriticalsAddress Then
            lAddress = IIf(IsDBNull(mEditingRow("CriticalsAddress")), mEditingRow("WorkingAddress"), mEditingRow("CriticalsAddress"))
        Else
            lAddress = IIf(IsDBNull(mEditingRow("WorkingAddress")), mEditingRow("CriticalsAddress"), mEditingRow("WorkingAddress"))
        End If
        If lAddress.Length > 200 Then
            lAddress = lAddress.Substring(0, 199)
        End If
        lRow("Address") = lAddress
        'lRow("Telephone") = ""
        'lRow("PostalCodeId") = ""
        lRow("AreaId") = mEditingRow("AreaId")
        lRow("WeatherId") = mEditingRow("WeatherId")
        'lRow("MainDisconnectGroupSetId") = ""
        'lRow("DisconnectGroupSetId") = ""
        'lRow("Priority") = ""
        'lRow("IsSingleSubscriber") = ""
        'lRow("IsOnePhaseSingleSubscriber") = ""
        'lRow("DisconnectInterval") = mEditingRow("DisconnectInterval")
        'lRow("DisconnectPower") = mEditingRow("DisconnectPower")
        lRow("DataEntryDT") = lNow.MiladiDate
        lRow("DataEntryDTPersian") = lNow.ShamsiDate
        lRow("DataEntryTime") = lNow.HourMin
        lRow("IsDuplicatedRequest") = False
        lRow("EndJobStateId") = EndJobStates.ejs_New
        lRow("ReferToId") = mEditingRow("ReferToId")
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
        lRow("TamirDisconnectFromDT") = mEditingRow("DisconnectDT")
        lRow("TamirDisconnectFromDatePersian") = mEditingRow("DisconnectDatePersian")
        lRow("TamirDisconnectFromTime") = mEditingRow("DisconnectTime")
        lRow("TamirDisconnectToDT") = mEditingRow("ConnectDT")
        lRow("TamirDisconnectToDatePersian") = mEditingRow("ConnectDatePersian")
        lRow("TamirDisconnectToTime") = mEditingRow("ConnectTime")
        'lRow("RequestDEInterval") = ""
        'lRow("CallTypeId") = ""
        'lRow("CallReasonId") = ""
        'lRow("TamirRequestTypeId") = ""
        lRow("DepartmentId") = mEditingRow("DepartmentId")
        'lRow("DisconnectRequestForId") = ""
        lRow("IsLightRequest") = False
        lRow("IsLPRequest") = False
        lRow("IsMPRequest") = False
        lRow("IsTamir") = True
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
        lRow("TamirTypeId") = cmbTamirType.SelectedValue

        'lRow("spcRSTId") = ""
        'lRow("DisconnectPowerECO") = ""

        lTbl.AddTblRequestRow(lRow)

        '-------------------
        'TblRequestInfo:

        lRowInfo("RequestId") = lRow("RequestId")
        'lRowInfo("CallTime") = DBNull.Value
        'lRowInfo("IsRealNotRelated") = DBNull.Value
        'lRowInfo("OperatorConvsType") = DBNull.Value
        'lRowInfo("IsSaveParts") = DBNull.Value
        'lRowInfo("IsInformTamir") = DBNull.Value
        'lRowInfo("IsSaveLight") = DBNull.Value
        'lRowInfo("SubscriberOKType") = DBNull.Value
        lRowInfo("IsWatched") = False
        'lRowInfo("SendTimeInterval") = DBNull.Value
        'lRowInfo("IsRealDuplicated") = DBNull.Value
        Dim lIsForceEnvironmentTypeId As Boolean = CConfig.ReadConfig("IsForceEnvironmentTypeId", "True")
        If lIsForceEnvironmentTypeId Then
            lRowInfo("EnvironmentTypeId") = IIf(mEditingRow("IsInCityService"), 1, 2)
        End If
        lRowInfo("SubscriberCode") = DBNull.Value
        lRowInfo("SendTimeInterval") = -1

        lRowInfo("IsNotNeedParts") = False
        'lRowInfo("NotNeedPartsReason") = False
        lRowInfo("IsEnterPartsLater") = False

        If chkIsSendToInformApp.Visible Then
            lRowInfo("IsSendToInformApp") = chkIsSendToInformApp.Checked
        End If

        lTblInfo.AddTblRequestInfoRow(lRowInfo)

        '-------------------
        'TblRequestPostFeeder:

        If cmbTamirNetworkType.SelectedValue <> TamirNetworkType.FT Then

            lRowPF = lTblPF.NewTblRequestPostFeederRow

            lRowPF("RequestPostFeederId") = GetAutoInc()
            lRowPF("RequestId") = lRow("RequestId")
            SetDBValue(cmbMPPost, lRowPF("MPPostId"))
            SetDBValue(cmbMPFeeder, lRowPF("MPFeederId"))
            If cmbTamirNetworkType.SelectedValue = TamirNetworkType.FT AndAlso cmbMPFeeder.SelectedIndex = -1 Then
                lRowPF("LocationTypeId") = LocationTypes.lt_MPPost
            ElseIf cmbTamirNetworkType.SelectedValue = TamirNetworkType.LP Then
                SetDBValue(cmbLPPost, lRowPF("LPPostId"))
                SetDBValue(cmbLPFeeder, lRowPF("LPFeederId"))
                lRowPF("LocationTypeId") = LocationTypes.lt_LPFeeder
            ElseIf chkIsLPPostFeederPart.Checked And rbLPPost.Checked Then
                SetDBValue(cmbLPPost, lRowPF("LPPostId"))
                lRowPF("LocationTypeId") = LocationTypes.lt_LPPost
            ElseIf chkIsLPPostFeederPart.Checked And rbFeederPart.Checked Then
                SetDBValue(cmbFeederPart, lRowPF("FeederPartId"))
                lRowPF("LocationTypeId") = LocationTypes.lt_FeederPart
            Else
                lRowPF("LocationTypeId") = LocationTypes.lt_MPFeeder
            End If

            lTblPF.AddTblRequestPostFeederRow(lRowPF)

        Else

            If mDs.TblTamirRequestFTFeeder.Rows.Count > 0 Then
                For Each lFTRow As DataRow In mDs.TblTamirRequestFTFeeder.Rows
                    lRowPF = lTblPF.NewTblRequestPostFeederRow
                    lRowPF("RequestPostFeederId") = GetAutoInc()
                    lRowPF("RequestId") = lRow("RequestId")
                    lRowPF("LocationTypeId") = LocationTypes.lt_MPFeeder
                    SetDBValue(cmbMPPost, lRowPF("MPPostId"))
                    lRowPF("MPFeederId") = lFTRow("MPFeederId")
                    lTblPF.AddTblRequestPostFeederRow(lRowPF)
                Next
            Else
                lRowPF = lTblPF.NewTblRequestPostFeederRow
                lRowPF("RequestPostFeederId") = GetAutoInc()
                lRowPF("RequestId") = lRow("RequestId")
                lRowPF("LocationTypeId") = LocationTypes.lt_MPPost
                SetDBValue(cmbMPPost, lRowPF("MPPostId"))
                lTblPF.AddTblRequestPostFeederRow(lRowPF)
            End If

        End If

        '-------------------


        Dim lUpdate As New frmUpdateDataset
        lIsSaveOK = lUpdate.UpdateDataSet("TblRequest", mDsReq, lRow("AreaId"), 1, , aTrans)

        If lIsSaveOK Then
            lRowInfo("RequestId") = lRow("RequestId")
            lIsSaveOK = lUpdate.UpdateDataSet("TblRequestInfo", mDsReq, lRow("AreaId"), 1, False, aTrans)
        End If

        If lIsSaveOK Then
            For Each lRowPF In lTblPF.Rows
                If lRowPF.RowState <> DataRowState.Unchanged Then
                    lRowPF("RequestId") = lRow("RequestId")
                End If
            Next
            lIsSaveOK = lUpdate.UpdateDataSet("TblRequestPostFeeder", mDsReq, lRow("AreaId"), 1, , aTrans)
        End If

        If lIsSaveOK Then lRequestId = lRow("RequestId")
        MakeDisconnectRequestId = lRequestId

        mMainRequestNumber = lRow("RequestNumber")

    End Function
    Private Sub LoadPowerInfo(Optional ByVal aIsUpdateCurrentValue As Boolean = False)
        If mIsLoading Then Exit Sub
        If Not pnlTamirRequestInfo.Enabled And Not pnlPostFeeder.Enabled Then Exit Sub

        If cmbTamirNetworkType.SelectedValue = TamirNetworkType.MP Then

            If rbLPPost.Checked Then
                LoadPowerInfoLP(aIsUpdateCurrentValue)
                If chkIsManoeuvre.Checked Then
                    LoadPowerInfoLPManovr(aIsUpdateCurrentValue)
                End If
            Else
                LoadPowerInfoMP(aIsUpdateCurrentValue)
                If chkIsManoeuvre.Checked Then
                    LoadPowerInfoMPManovr(aIsUpdateCurrentValue)
                End If
            End If

        ElseIf cmbTamirNetworkType.SelectedValue = TamirNetworkType.LP Then

            LoadPowerInfoLP(aIsUpdateCurrentValue)
            If chkIsManoeuvre.Checked Then
                LoadPowerInfoLPManovr(aIsUpdateCurrentValue)
            End If

        ElseIf cmbTamirNetworkType.SelectedValue = TamirNetworkType.FT Then

            LoadPowerInfoMP(aIsUpdateCurrentValue)
            If chkIsManoeuvre.Checked Then
                LoadPowerInfoMPManovr(aIsUpdateCurrentValue)
            End If

        End If
    End Sub
    Private Sub LoadPowerInfoMP(Optional ByVal aIsUpdateCurrentValue As Boolean = False)

        Dim lSQL As String
        Dim lMPFeederId As Integer = -1
        Dim lRow As DataRow
        Dim lYearMonth As String = ""
        Dim lInterval As Integer = 0
        Dim P As Double = 0.0, lCurrentValue As Double = 0.0

        Try
            If chkIsWarmLine.Checked Then
                txtPower.Text = "0"
                Exit Sub
            End If

            lCurrentValue = Val(txtCurrentValue.Text)
            mCurrentValue = Val(txtCurrentValue.Text)
            mCosinPhi = 0.85
            mVoltage = 20000

            If cmbMPFeeder.SelectedIndex > -1 Then
                lMPFeederId = cmbMPFeeder.SelectedValue
            End If

            lSQL = "SELECT * FROM Tbl_MPFeeder WHERE MPFeederId = " & lMPFeederId
            BindingTable(lSQL, mCnn, mDsReq, "Tbl_MPFeeder", , , , , , , True)

            If mDsReq.Tbl_MPFeeder.Rows.Count > 0 Then
                lRow = mDsReq.Tbl_MPFeeder.Rows(0)
                mCosinPhi = IIf(IsDBNull(lRow("CosinPhi")), 0.85, lRow("CosinPhi"))
                mVoltage = IIf(IsDBNull(lRow("Voltage")), 20000, lRow("Voltage"))
            End If

            lSQL =
                " SELECT TOP 1 LoadDatePersian, PeakCurrentValue " &
                " FROM TblMPFeederPeak " &
                " WHERE (IsDaily = 1) AND (MPFeederId = " & lMPFeederId & ") " &
                " ORDER BY LoadDT DESC "
            BindingTable(lSQL, mCnn, mDsReq, "TblMPFeederPeak", , , , , , , True)

            If mDsReq.TblMPFeederPeak.Rows.Count > 0 Then
                lRow = mDsReq.TblMPFeederPeak.Rows(0)
                lYearMonth = Convert.ToString(lRow("LoadDatePersian")).Substring(0, 7)
                If Not IsDBNull(lRow("PeakCurrentValue")) Then
                    lCurrentValue = lRow("PeakCurrentValue")
                End If
            End If

            lSQL =
                " SELECT TOP 1 YearMonth, PeakCurrentValue " &
                " FROM TblMPFeederPeak " &
                " WHERE (IsDaily = 0) AND (MPFeederId = " & lMPFeederId & ") " &
                " ORDER BY YearMonth DESC "
            BindingTable(lSQL, mCnn, mDsReq, "TblMPFeederPeak", , , , , , , True)

            If mDsReq.TblMPFeederPeak.Rows.Count > 0 Then
                lRow = mDsReq.TblMPFeederPeak.Rows(0)
                If lYearMonth < lRow("YearMonth") Then
                    lCurrentValue = lRow("PeakCurrentValue")
                End If
            End If

            lInterval = Val(txtDCInterval.Text.Trim)
            lblPeakBar.Text = "آخرين بار پيک فيدر فشار متوسط"

            If aIsUpdateCurrentValue Then
                mCurrentValue = lCurrentValue
                txtCurrentValue.Text = mCurrentValue
            End If

            mDCPowerECO = 0
            Try
                P = mCurrentValue * (lInterval / 60)
                Dim lPowerFull As Double = (Math.Sqrt(3) * mCosinPhi * mVoltage * P) / 1000000
                mPower = lPowerFull

                FindOverlapTimes()
                If mOverlapTime > 0 Then
                    lInterval = mRealDCTime
                    P = mCurrentValue * (lInterval / 60)
                    mPower = (Math.Sqrt(3) * mCosinPhi * mVoltage * P) / 1000000
                    mDCPowerECO = lPowerFull - mPower
                End If
            Catch ex1 As Exception
            End Try

            txtLastPeakBar.Text = lCurrentValue
            txtPower.Text = Math.Round(mPower, 2)

        Catch ex As Exception
            WriteError(ex.ToString)
        End Try

    End Sub
    Private Sub LoadPowerInfoLP(Optional ByVal aIsUpdateCurrentValue As Boolean = False)
        If mIsLoadingPower Then Exit Sub

        Dim lSQL As String
        Dim lRow As DataRow
        Dim lYearMonth As String = ""
        Dim lInterval As Integer = 0
        Dim P As Double = 0.0, lCurrentValue As Double = 0.0
        Dim A As Double = 1
        Dim lIsLPPost As Boolean = IIf(cmbLPFeeder.SelectedIndex > -1, False, True)
        Dim lTableName As String = IIf(lIsLPPost, "Tbl_LPPost", "Tbl_LPFeeder")
        Dim lPeakName As String = IIf(lIsLPPost, "PostPeakCurrent", "FeederPeakCurrent")
        Dim lId As Integer = -1
        Try
            mIsLoadingPower = True
            txtDateDisconnect.InsertTime(txtTimeDisconnect)
            If IsNothing(txtDateDisconnect.MiladiDT) OrElse IsDBNull(txtDateDisconnect.MiladiDT) Then
                Exit Sub
            End If

            A = CalculateA4LoadDiagrams(txtDateDisconnect.MiladiDT)

            If chkIsWarmLine.Checked Then
                txtPower.Text = "0"
                Exit Sub
            End If

            lCurrentValue = Val(txtCurrentValue.Text)
            mCurrentValue = Val(txtCurrentValue.Text)
            mCosinPhi = 0.85
            mVoltage = 400

            If lIsLPPost Then
                lId = cmbLPPost.SelectedValue
            Else
                lId = cmbLPFeeder.SelectedValue
            End If

            lSQL =
                " SELECT TOP 1 " &
                "	ISNULL(TblLPFeederLoad.FeederPeakCurrent, Tbl_LPFeeder.FeederPeakCurrent) AS FeederPeakCurrent, " &
                "   " & IIf(lIsLPPost, "0.85", "ISNULL(TblLPFeederLoad.CosinPhi, Tbl_LPFeeder.CosinPhi)") & " AS CosinPhi " &
                " FROM " &
                "	TblLPFeederLoad " &
                "	INNER JOIN Tbl_LPFeeder ON TblLPFeederLoad.LPFeederId = Tbl_LPFeeder.LPFeederId " &
                " WHERE " &
                "	TblLPFeederLoad.LPFeederId = " & lId &
                " ORDER BY " &
                "	TblLPFeederLoad.LoadDT DESC "
            If lIsLPPost Then lSQL = lSQL.Replace("Feeder", "Post")
            RemoveMoreSpaces(lSQL)
            BindingTable(lSQL, mCnn, mDsReq, lTableName, , , , , , , True)

            Dim lTblLoadInfo As DataTable = mDsReq.Tables(lTableName)

            If lTblLoadInfo.Rows.Count > 0 Then
                lRow = lTblLoadInfo.Rows(0)
                mCosinPhi = IIf(IsDBNull(lRow("CosinPhi")), 0.85, lRow("CosinPhi"))
                lCurrentValue = IIf(IsDBNull(lRow(lPeakName)), 0, lRow(lPeakName))
                If lIsLPPost Then
                    lblPeakBar.Text = "آخرين بار پيک پست توزيع"
                Else
                    lblPeakBar.Text = "آخرين بار پيک فيدر فشار ضعيف"
                End If
            Else
                lCurrentValue = 0
            End If

            lInterval = Val(txtDCInterval.Text.Trim)

            If aIsUpdateCurrentValue Then
                mCurrentValue = lCurrentValue
                txtCurrentValue.Text = mCurrentValue
            End If

            mDCPowerECO = 0
            Try
                P = mCurrentValue * (lInterval / 60)
                Dim lPowerFull As Double = (Math.Sqrt(3) * mCosinPhi * mVoltage * P * A) / 1000000
                mPower = lPowerFull

                FindOverlapTimes()
                If mOverlapTime > 0 Then
                    lInterval = mRealDCTime
                    P = mCurrentValue * (lInterval / 60)
                    mPower = (Math.Sqrt(3) * mCosinPhi * mVoltage * P * A) / 1000000
                    mDCPowerECO = lPowerFull - mPower
                End If
            Catch ex1 As Exception
            End Try

            txtLastPeakBar.Text = lCurrentValue
            txtPower.Text = Math.Round(mPower, 4) * 1000

        Catch ex As Exception
            WriteError(ex.ToString)
        Finally
            mIsLoadingPower = False
        End Try
    End Sub
    Private Sub LoadMPFeederDisconnects()
        Dim lSQL As String
        Dim lFieldValue As Integer = -1
        Dim lMPFeederId As Integer = -1
        Dim lTbl As DataTable = Nothing
        Dim lLastDCCount As Integer = 0
        Dim lDayDCCount As Integer = 0
        Dim lFilterDate As String = ""
        Dim lSQLMain As String
        Dim lSQLUnion As String = ""
        If cmbMPFeeder.SelectedIndex > -1 Then
            lMPFeederId = cmbMPFeeder.SelectedValue
        End If

        Try

            If cmbPeriod.SelectedValue = "nDCs" AndAlso txtLastDCCount.Text.Trim.Length > 0 Then
                lLastDCCount = txtLastDCCount.Text
            End If
            If cmbPeriod.SelectedValue = "ndays" AndAlso txtFeederDCDayCount.Text.Trim.Length > 0 Then
                lDayDCCount = txtFeederDCDayCount.Text
            End If
            If cmbPeriod.SelectedValue <> "nDCs" Then
                lFilterDate &= IIf(txtDateFrom.IsOK, " AND Tbl_###.DisconnectDatePersian >= '" & txtDateFrom.Text & "'", "")
                lFilterDate &= IIf(txtDateTo.IsOK, " AND Tbl_###.DisconnectDatePersian <= '" & txtDateTo.Text & "'", "")
            End If
            Dim lTableName As String = "", lFieldName As String = ""
            If cmbLPFeeder.SelectedIndex > -1 Then
                lTableName = "ViewAllRequestLP"
                lFieldName = "LPFeederId"
                lFieldValue = cmbLPFeeder.SelectedValue
                lSQLMain = MakeMainSQL(False, lTableName, lFieldName, lFieldValue, lFilterDate)
                lSQLUnion = MakeUnionSQLForLPPost(cmbLPPost.SelectedValue, lFilterDate)
                lSQLUnion &= MakeUnionSQLForFeederPart(cmbLPPost.SelectedValue, lFilterDate)
                lSQLUnion &= MakeUnionSQLForMPFeeder(cmbMPFeeder.SelectedValue, lFilterDate)
                lSQLUnion &= MakeUnionSQLForFT(cmbMPFeeder.SelectedValue, cmbMPPost.SelectedValue, lFilterDate)
            ElseIf cmbLPPost.SelectedIndex > -1 Then
                lTableName = "ViewAllRequest"
                lFieldName = "LPPostId"
                lFieldValue = cmbLPPost.SelectedValue
                lSQLMain = MakeMainSQL(False, lTableName, lFieldName, lFieldValue, lFilterDate)
                lSQLUnion = MakeUnionSQLForFeederPart(lFieldValue, lFilterDate)
                lSQLUnion &= MakeUnionSQLForMPFeeder(cmbMPFeeder.SelectedValue, lFilterDate)
                lSQLUnion &= MakeUnionSQLForFT(cmbMPFeeder.SelectedValue, cmbMPPost.SelectedValue, lFilterDate)
            ElseIf cmbFeederPart.SelectedIndex > -1 Then
                lTableName = ""
                lFieldName = ""
                lFieldValue = cmbFeederPart.SelectedValue
                lSQLMain = MakeMainSQL(True, lTableName, lFieldName, lFieldValue, lFilterDate)
                lSQLUnion = MakeUnionSQLForMPFeeder(cmbMPFeeder.SelectedValue, lFilterDate)
                lSQLUnion &= MakeUnionSQLForFT(cmbMPFeeder.SelectedValue, cmbMPPost.SelectedValue, lFilterDate)
            ElseIf cmbMPFeeder.SelectedIndex > -1 Then
                lTableName = "ViewAllRequestMP"
                lFieldName = "MPFeederId"
                lFieldValue = cmbMPFeeder.SelectedValue
                lSQLMain = MakeMainSQL(False, lTableName, lFieldName, lFieldValue, lFilterDate)
                lSQLUnion = MakeUnionSQLForFT(lFieldValue, cmbMPPost.SelectedValue, lFilterDate)
            Else
                lTableName = "ViewAllRequestFT"
                lFieldName = "MPPostId"
                lFieldValue = cmbMPPost.SelectedValue
                lSQLMain = MakeMainSQL(False, lTableName, lFieldName, lFieldValue, lFilterDate)
            End If

            lSQL =
                "SELECT " & IIf(lLastDCCount > 0, "TOP " & lLastDCCount, "") & " Cast(NULL As int) As Radif, " &
                "   ViewMonitoring.RequestId ,ViewMonitoring.RequestNo , " &
                "   isnull(TblFogheToziDisconnectMPFeeder.DisconnectDatePersian,  ViewMonitoring.DisconnectDatePersian) AS DisconnectDatePersian ," &
                "   isnull(TblFogheToziDisconnectMPFeeder.DisconnectTime, ViewMonitoring.DisconnectTime) AS DisconnectTime ," &
                "   isnull(TblFogheToziDisconnectMPFeeder.ConnectDatePersian, ViewMonitoring.ConnectDatePersian) AS ConnectDatePersian , " &
                "   isnull(TblFogheToziDisconnectMPFeeder.ConnectTime,	ViewMonitoring.ConnectTime ) AS ConnectTime , " &
                "   ViewMonitoring.DisconnectGroupSet , " &
                "   CASE WHEN NOT TblFogheToziDisconnectMPFeeder.FogheToziDisconnectMPFeederId IS NULL THEN " &
                "   CAST(datediff(MINUTE,TblFogheToziDisconnectMPFeeder.DisconnectDT,TblFogheToziDisconnectMPFeeder.ConnectDT) / 60 AS varchar(5)) + ':' + CAST(datediff(MINUTE,TblFogheToziDisconnectMPFeeder.DisconnectDT,TblFogheToziDisconnectMPFeeder.ConnectDT) % 60 AS varchar(2)) " &
                "   ELSE ViewMonitoring.DisconnectInterval End  AS DisconnectInterval, " &
                "   isnull(TblFogheToziDisconnectMPFeeder.DisconnectPower, ViewMonitoring.DisconnectPower) AS DisconnectPower, " &
                "   ViewMonitoring.IsTamir ,ViewMonitoring.EndJobState ,ViewMonitoring.OCEFRelayActionId ,ViewMonitoring.OCEFRelayAction, " &
                "   COUNT(TblMPRequestFeederPart.FeederPartId) AS FeederPartCount, " &
                "   CASE WHEN ViewMonitoring.IsFogheToziRequest = 1 THEN 'فوق توزيع' " &
                "   WHEN ViewMonitoring.IsMPRequest = 1 THEN 'فشار متوسط' " &
                "   WHEN ViewMonitoring.IsLPRequest = 1 THEN 'فشار ضعيف' " &
                "   END AS DisconnectType, " &
                "   CASE WHEN ViewMonitoring.LPFeederId IS NOT NULL THEN 'فيدر فشار ضعيف' " &
                "   WHEN ViewMonitoring.LPPostId IS NOT NULL THEN 'پست توزيع' " &
                "   WHEN COUNT(TblMPRequestFeederPart.FeederPartId) > 0 THEN 'تکه فيدر' " &
                "   WHEN ViewMonitoring.MPFeederId IS NOT NULL THEN 'فيدر فشار متوسط' " &
                "   WHEN ISNULL(ViewMonitoring.MPPostId, ViewMonitoring.MPPostIdFT) IS NOT NULL THEN 'پست فوق توزيع' " &
                "   END AS DisconnectSection " &
                "FROM ViewMonitoring " &
                "LEFT JOIN TblRequest ON TblRequest.RequestId = ViewMonitoring.RequestId " &
                "LEFT JOIN TblMPRequestFeederPart ON TblMPRequestFeederPart.MPRequestId = TblRequest.MPRequestId " &
                "left join TblFogheToziDisconnect on TblRequest.FogheToziDisconnectId = TblFogheToziDisconnect.FogheToziDisconnectId " &
                "left join TblFogheToziDisconnectMPFeeder on TblFogheToziDisconnect.FogheToziDisconnectId = TblFogheToziDisconnectMPFeeder.FogheToziDisconnectId " &
                "   AND TblFogheToziDisconnectMPFeeder.MPFeederId = " & lMPFeederId &
                "WHERE ViewMonitoring.RequestId IN " &
                "(" & lSQLMain &
                "   " & lSQLUnion &
                ") " &
                "GROUP BY ViewMonitoring.RequestId, ViewMonitoring.RequestNo, " &
                "   isnull(TblFogheToziDisconnectMPFeeder.DisconnectDatePersian,  ViewMonitoring.DisconnectDatePersian) ," &
                "   isnull(TblFogheToziDisconnectMPFeeder.DisconnectTime, ViewMonitoring.DisconnectTime) ," &
                "   isnull(TblFogheToziDisconnectMPFeeder.ConnectDatePersian, ViewMonitoring.ConnectDatePersian) , " &
                "   isnull(TblFogheToziDisconnectMPFeeder.ConnectTime,	ViewMonitoring.ConnectTime ) , " &
                "   CASE WHEN NOT TblFogheToziDisconnectMPFeeder.FogheToziDisconnectMPFeederId IS NULL THEN " &
                "   CAST(datediff(MINUTE,TblFogheToziDisconnectMPFeeder.DisconnectDT,TblFogheToziDisconnectMPFeeder.ConnectDT) / 60 AS varchar(5)) + ':' + CAST(datediff(MINUTE,TblFogheToziDisconnectMPFeeder.DisconnectDT,TblFogheToziDisconnectMPFeeder.ConnectDT) % 60 AS varchar(2)) " &
                "   ELSE ViewMonitoring.DisconnectInterval End, " &
                "   isnull(TblFogheToziDisconnectMPFeeder.DisconnectPower, ViewMonitoring.DisconnectPower) , " &
                " ViewMonitoring.DisconnectGroupSet, " &
                " ViewMonitoring.IsTamir, " &
                "   ViewMonitoring.EndJobState, ViewMonitoring.OCEFRelayActionId, ViewMonitoring.OCEFRelayAction, ViewMonitoring.IsFogheToziRequest, ViewMonitoring.IsMPRequest, " &
                "   ViewMonitoring.IsLPRequest, ViewMonitoring.LPFeederId, ViewMonitoring.LPPostId, ViewMonitoring.MPFeederId, ViewMonitoring.MPPostId, ViewMonitoring.MPPostIdFT " &
                "ORDER BY " &
                "   isnull(TblFogheToziDisconnectMPFeeder.DisconnectDatePersian,  ViewMonitoring.DisconnectDatePersian) ," &
                "   isnull(TblFogheToziDisconnectMPFeeder.DisconnectTime, ViewMonitoring.DisconnectTime) "

            RemoveMoreSpaces(lSQL)
            ClearTable(mDsReqView, "ViewMonitoring")
            BindingTable(lSQL, mCnn, mDsReqView, "ViewMonitoring")
            lTbl = mDsReqView.Tables("ViewMonitoring")

            mSumDCInterval = 0
            mSumPower = 0.0
            mDCCount = 0
            mRelayCount = 0
            For Each lRow As DataRow In lTbl.Rows
                Try
                    Dim lInterval As Integer = 0
                    Dim lH As String = "0", lM As String = "0"
                    Dim lValue As String = lRow("DisconnectInterval")
                    Dim lTime() As String = lValue.Split(":")

                    If lTime.Length >= 1 Then lH = lTime(0)
                    If lTime.Length >= 2 Then lM = lTime(1)

                    lInterval = Val(lH) * 60 + Val(lM)

                    mSumDCInterval += lInterval
                    mSumPower += Val(lRow("DisconnectPower"))

                    Dim lIsTamir As Boolean = lRow("IsTamir")
                    If lIsTamir Then
                        mDCCount += 1
                    End If

                    Dim lRelayActionId As Integer = lRow("OCEFRelayActionId")
                    If lRelayActionId > 1 Then
                        mRelayCount += 1
                    End If
                Catch ex As Exception
                End Try
            Next

            dgFeederDC.DataSource = lTbl
            MakeRadif()

            'lSQL = _
            '    " SELECT COUNT(TblMPRequest.MPRequestId) AS Cnt " & _
            '    " FROM TblMPRequest INNER JOIN TblRequest ON TblMPRequest.MPRequestId = TblRequest.MPRequestId " & _
            '    " WHERE TblRequest.IsDisconnectMPFeeder = 1 AND TblMPRequest.OCEFRelayActionId > 1 " & _
            '    " AND TblMPRequest.MPFeederId = " & cmbMPFeeder.SelectedValue & " " & _
            '    lFilterDate.Replace("DisconnectDatePersian", "TblMPRequest.DisconnectDatePersian") & _
            '    IIf(rbNT_Tamir.Checked, " AND TblRequest.IsTamir = 1", IIf(rbNT_NotTamir.Checked, " AND TblRequest.IsTamir = 0", ""))
            'RemoveMoreSpaces(lSQL)
            'ClearTable(mDsReqView, "ViewRelayAction")
            'BindingTable(lSQL, mCnn, mDsReqView, "ViewRelayAction")
            'If mDsReqView.Tables.Contains("ViewRelayAction") AndAlso mDsReqView.Tables("ViewRelayAction").Rows.Count > 0 Then
            '    lTbl = mDsReqView.Tables("ViewRelayAction")
            '    mRelayCount = lTbl.Rows(0)("Cnt")
            'End If
            SetRegistryAbd("TamirFeederDCCount", lLastDCCount)
            SetRegistryAbd("TamirFeederDCDayCount", lDayDCCount)
            SetRegistryAbd("TamirFeederPeriod", cmbPeriod.SelectedValue)
            SetRegistryAbd("TamirFeederNT", IIf(rbNT_Tamir.Checked, "tamir", IIf(rbNT_NotTamir.Checked, "nottamir", "")))

        Catch ex As Exception
        End Try
    End Sub
    Private Function MakeMainSQL(ByVal aIsFeederPart As Boolean, ByVal aTableName As String, ByVal aFieldName As String, ByVal aFieldValue As String, ByVal aFilterDate As String) As String
        Dim lSQL As String = ""
        If aIsFeederPart Then
            lSQL =
            " SELECT RequestId " &
            " FROM TblMPRequest " &
            " INNER JOIN TblRequest ON TblRequest.MPRequestId = TblMPRequest.MPRequestId " &
            " INNER JOIN TblMPRequestFeederPart ON TblMPRequestFeederPart.MPRequestId = TblMPRequest.MPRequestId " &
            " WHERE TblMPRequestFeederPart.FeederPartId = " & aFieldValue & " " & aFilterDate.Replace("Tbl_###", "TblMPRequest")
        Else
            lSQL =
            " SELECT RequestId FROM " & aTableName &
            "   WHERE " & aFieldName & " = " & aFieldValue &
            IIf(cmbLPFeeder.SelectedIndex > -1 Or cmbFeederPart.SelectedIndex > -1, "", IIf(cmbLPPost.SelectedIndex > -1, " AND IsTotalLPPostDisconnected = 1 ", IIf(cmbMPFeeder.SelectedIndex > -1, " AND IsDisconnectMPFeeder = 1 ", ""))) &
            "   " & aFilterDate.Replace("Tbl_###", aTableName) &
            "   " & IIf(rbNT_Tamir.Checked, " AND IsTamir = 1", IIf(rbNT_NotTamir.Checked, " AND IsTamir = 0", ""))
        End If

        Return lSQL
    End Function
    Private Function MakeUnionSQLForFT(ByVal aMPFeederId As Integer, ByVal aMPPostId As Integer, ByVal aFilterDate As String) As String
        Dim lSQL As String =
        " UNION " &
        " SELECT ViewAllRequestFT.RequestId " &
        " FROM ViewAllRequestFT " &
        " LEFT JOIN TblFogheToziDisconnect ON ViewAllRequestFT.FTRequestId = TblFogheToziDisconnect.FogheToziDisconnectId " &
        " WHERE (ViewAllRequestFT.MPFeederId = " & aMPFeederId & " or (ViewAllRequestFT.MPPostId = " & aMPPostId & " AND TblFogheToziDisconnect.IsFeederMode = 0)) " & aFilterDate.Replace("Tbl_###", "ViewAllRequestFT")
        Return lSQL
    End Function
    Private Function MakeUnionSQLForMPFeeder(ByVal aMPFeederId As Integer, ByVal aFilterDate As String) As String
        Dim lSQL As String =
        " UNION " &
        " SELECT RequestId " &
        " FROM ViewAllRequestMP " &
        " WHERE MPFeederId = " & aMPFeederId & " AND  IsDisconnectMPFeeder = 1 " & aFilterDate.Replace("Tbl_###", "ViewAllRequestMP")

        Return lSQL
    End Function
    Private Function MakeUnionSQLForFeederPart(ByVal aLPPostId As Integer, ByVal aFilterDate As String) As String
        Dim lSQL As String =
        " UNION " &
        " SELECT RequestId " &
        " FROM TblMPRequest " &
        " INNER JOIN TblRequest ON TblRequest.MPRequestId = TblMPRequest.MPRequestId " &
        " INNER JOIN TblMPRequestFeederPart ON TblMPRequestFeederPart.MPRequestId = TblMPRequest.MPRequestId  " &
        " INNER JOIN Tbl_LPPost ON TblMPRequestFeederPart.FeederPartId = Tbl_LPPost.FeederPartId " &
        " WHERE Tbl_LPPost.LPPostId = " & aLPPostId & " " &
        "   AND TblMPRequest.IsNotDisconnectFeeder = 1 " & aFilterDate.Replace("Tbl_###", "TblMPRequest")

        Return lSQL
    End Function
    Private Function MakeUnionSQLForLPPost(ByVal aLPPostId As Integer, ByVal aFilterDate As String) As String
        Dim lSQL As String =
        " UNION " &
        " SELECT RequestId " &
        " FROM ViewAllRequest " &
        " WHERE LPPostId = " & aLPPostId & " AND IsTotalLPPostDisconnected = 1 " & aFilterDate.Replace("Tbl_###", "ViewAllRequest")

        Return lSQL
    End Function
    Private Sub MakeMessageForUser(ByVal aAreaUserId As Integer, ByVal aIsConfirm As Boolean, ByVal aTrans As SqlTransaction)
        Dim lNow As CTimeInfo = GetServerTimeInfo()

        Dim lIsSendMessage As Boolean = True
        Try

            Dim lTbl As DatasetCcRequester.TblMessageDataTable = mDsReq.TblMessage
            Dim lRow As DatasetCcRequester.TblMessageRow = lTbl.NewTblMessageRow

            Dim lMsg As String
            If Not aIsConfirm Then
                lMsg =
                    "متأسفانه با درخواست خاموشي به شماره " & txtTamirRequestNumber.Text &
                    " به دليل <" & txtUnConfirmReason.Text.Trim() & "> موافقت نشده است."

                If chkIsEmergency.Checked Then
                    lMsg &= vbCrLf & vbCrLf &
                        "از آنجا که اين درخواست به صورت خاموشي بدون نياز به تأييد ثبت شده است، " &
                        "مسئوليت عدم تأييد آن به عهده کاربر ثبت کننده درخواست خواهد بود."
                End If

                If Not mIsSendMessageForNotConfirm Then
                    lIsSendMessage = False
                End If
            Else
                lMsg =
                    "با درخواست خاموشي به شماره " & txtTamirRequestNumber.Text & " موافقت شده، " &
                    "ساعت شروع خاموشي " & txtTimeDisconnect.Text & "، در تاريخ " & txtDateDisconnect.Text & " مي باشد " &
                     "و هم اکنون رکورد خاموشي بابرنامه مرتبط با آن " & IIf(mMainRequestNumber <> "", "با شماره " & mMainRequestNumber, "") & " در نرم افزار ثبت حوادث ايجاد شده است."
                If mManovrRequestNumber <> "" Then
                    lMsg &= vbCrLf & vbCrLf & "همچنين پرونده ديگري با شماره " & mManovrRequestNumber & " جهت بازگشت مانور پيش بيني شده در نرم افزار ثبت حوادث ايجاد گرديده است."
                End If

                If Not mIsSendMessageForConfirm Then
                    lIsSendMessage = False
                End If
            End If

            If Not lIsSendMessage Then Exit Sub

            Dim lSQL As String = "SELECT * FROM Tbl_AreaUser WHERE AreaUserId = " & aAreaUserId
            BindingTable(lSQL, mCnn, mDsReq, "TblAreaUserInfo", , , , , aTrans)

            lRow("MessageId") = GetAutoInc()
            lRow("SenderAreaId") = WorkingAreaId
            lRow("Sender") = WorkingUserName
            lRow("ReceiverAreaId") = mDsReq.Tables("TblAreaUserInfo").Rows(0)("AreaId")
            lRow("Receiver") = mDsReq.Tables("TblAreaUserInfo").Rows(0)("UserName")
            lRow("SendDT") = lNow.MiladiDate
            lRow("SendDatePersian") = lNow.ShamsiDate
            lRow("SendTime") = lNow.HourMin
            lRow("IsAlarm") = False
            lRow("MessageDesc") = lMsg
            lRow("SendToAllAreas") = False
            lRow("IsValidTillDate") = lNow.MiladiDate
            lRow("ReceiverAreaUserId") = DBNull.Value 'aAreaUserId
            lRow("RequestId") = DBNull.Value

            lTbl.AddTblMessageRow(lRow)

            Dim lUpdate As New frmUpdateDataset
            lUpdate.UpdateDataSet("TblMessage", mDsReq, mDsReq.Tables("TblAreaUserInfo").Rows(0)("AreaId"), , , aTrans)

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Sub MakeMessageForUserReturn(ByVal aAreaUserId As Integer, ByVal aTrans As SqlTransaction)
        Dim lNow As CTimeInfo = GetServerTimeInfo()

        Dim lIsSendMessage As Boolean = True
        Try

            Dim lTbl As DatasetCcRequester.TblMessageDataTable = mDsReq.TblMessage
            Dim lRow As DatasetCcRequester.TblMessageRow = lTbl.NewTblMessageRow

            Dim lMsg As String

            Dim lReason As String = ""
            If mIsForWarmLineConfirm Then
                lReason = txtWarmLineReason.Text & " در تأييد خط گرم "
            Else
                lReason = txtUnConfirmReason.Text
            End If

            lMsg =
                "درخواست خاموشي به شماره " & txtTamirRequestNumber.Text & " به دليل « " & lReason & " » عودت داده شده است " &
                "لطفاً اصلاحات مورد نظر را انجام داده و مجدداً درخواست را ارسال نماييد "

            Dim lDs As New DataSet
            Dim lSQL As String = "SELECT * FROM Tbl_AreaUser WHERE AreaUserId = " & aAreaUserId
            BindingTable(lSQL, mCnn, lDs, "TblAreaUserInfo", , , , , aTrans)

            lRow("MessageId") = GetAutoInc()
            lRow("SenderAreaId") = WorkingAreaId
            lRow("Sender") = WorkingUserName
            lRow("ReceiverAreaId") = lDs.Tables("TblAreaUserInfo").Rows(0)("AreaId")
            lRow("Receiver") = lDs.Tables("TblAreaUserInfo").Rows(0)("UserName")
            lRow("SendDT") = lNow.MiladiDate
            lRow("SendDatePersian") = lNow.ShamsiDate
            lRow("SendTime") = lNow.HourMin
            lRow("IsAlarm") = False
            lRow("MessageDesc") = lMsg
            lRow("SendToAllAreas") = False
            lRow("IsValidTillDate") = lNow.MiladiDate
            lRow("ReceiverAreaUserId") = DBNull.Value 'aAreaUserId
            lRow("RequestId") = DBNull.Value

            lTbl.AddTblMessageRow(lRow)

            Dim lUpdate As New frmUpdateDataset
            lUpdate.UpdateDataSet("TblMessage", mDsReq, lDs.Tables("TblAreaUserInfo").Rows(0)("AreaId"), , , aTrans)

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Sub ComputeUsedPower()
        If rbIsRequestByFogheTozi.Checked And cmbTamirNetworkType.SelectedValue = TamirNetworkType.FT Then Exit Sub
        If mIsLoading Then Exit Sub
        If mFirstDay_PDate = "" Then Exit Sub

        Dim lDs As New DatasetTamir
        Dim lSQL As String
        Dim lServer121Id As Integer, lAreaId As Integer = -1
        Dim lRowWeak As DatasetTamir.TblWeakPowerRow, lRow As DatasetTamir.TblWeakPowerRow
        Dim lDT As DateTime
        Dim lDatePersian As String
        Dim lKh As New KhayamDateTime
        Dim lLastPower As Double

        Try
            If cmbArea.SelectedIndex = -1 Then
                'txtMaxPower.Text = "0"
                'txtUsedPower.Text = "0"
                'Exit Sub
                lAreaId = WorkingAreaId
            Else
                lAreaId = cmbArea.SelectedValue
            End If

            'Try
            '    Dim ldDDT As DateTime = GetFirstDateOfMonth(txtDateDisconnect.MiladiDT)
            '    Dim lpDDT As String = GetFirstDatePersianOfMonth(txtDateDisconnect.MiladiDT)
            'Catch ex As Exception
            'End Try

            If txtDateDisconnect.IsOK Then
                mFirstDay_DT = GetFirstDateOfTamirScope(txtDateDisconnect.MiladiDT)
                mFirstDay_PDate = GetFirstDatePersianOfTamirScope(mFirstDay_DT)
            End If

            mMaxPower = -1

            lSQL = "SELECT Server121Id FROM Tbl_Area WHERE AreaId = " & lAreaId
            BindingTable(lSQL, mCnn, lDs, "Tbl_Area")
            lServer121Id = lDs.Tables("Tbl_Area").Rows(0)("Server121Id")

            '------------------------------
            lSQL =
                "SELECT * FROM Tbl_TamirInfo WHERE " &
                "Server121Id = " & lServer121Id & " AND AreaId " & IIf(mIsServer121, "IS NULL", "= " & lAreaId)
            BindingTable(lSQL, mCnn, lDs, "Tbl_TamirInfo")
            If lDs.Tbl_TamirInfo.Rows.Count > 0 Then
                mMaxPower = lDs.Tbl_TamirInfo.Rows(0)("MaxWeakPower")
            End If
            '------------------------------


            lSQL =
                "SELECT * FROM TblWeakPower WHERE SaturdayDatePersian = '" & mFirstDay_PDate & "'" &
                " AND Server121Id = " & lServer121Id & " AND AreaId " & IIf(mIsServer121, "IS NULL", "= " & lAreaId)
            BindingTable(lSQL, mCnn, lDs, "TblWeakPower")

            If lDs.TblWeakPower.Rows.Count > 0 Then

                lRowWeak = lDs.TblWeakPower.Rows(0)

            Else

                lSQL =
                    "SELECT TOP 1 * FROM TblWeakPower WHERE SaturdayDatePersian < '" & mFirstDay_PDate & "'" &
                    " AND Server121Id = " & lServer121Id & " AND AreaId " & IIf(mIsServer121, "IS NULL", "= " & lAreaId) &
                    " ORDER BY SaturdayDT DESC"
                BindingTable(lSQL, mCnn, lDs, "TblWeakPower")

                If lDs.TblWeakPower.Rows.Count > 0 Then
                    lRow = lDs.TblWeakPower.Rows(0)
                    lDT = lRow("SaturdayDT")
                    lLastPower = lRow.LastExtraPower + lRow.UsedPower - lRow.MaxPower
                    If lLastPower < 0 Then lLastPower = 0
                Else
                    lDT = mFirstDay_DT.AddDays(-7)
                    lLastPower = 0
                End If

                lKh.SetMiladyDate(lDT)
                lDatePersian = lKh.GetShamsiDateStr()

                Do Until lDatePersian >= mFirstDay_PDate
                    lDT = lDT.AddDays(7)
                    lKh.SetMiladyDate(lDT)
                    lDatePersian = lKh.GetShamsiDateStr()
                    lRowWeak = lDs.TblWeakPower.AddTblWeakPowerRow(GetAutoInc(), lDT, lDatePersian, lServer121Id, lAreaId, 0, mMaxPower, lLastPower, 0, mIsTamirPowerMonthly)
                    lLastPower = 0
                Loop

            End If

            If Not IsDBNull(lRowWeak("MaxPower")) AndAlso lRowWeak("MaxPower") > -1 Then
                mMaxPower = lRowWeak("MaxPower")
            End If

            If mMaxPower = -1 Then
                mMaxPower = 0
            End If

            '-----------------------
            '  Compute Wait Powers
            '-----------------------
            Dim lWaitPower As Double
            Dim lRemainPower As Double

            lSQL =
                "SELECT Sum(DisconnectPower) As SumWaitPower FROM TblTamirRequest " &
                " WHERE SaturdayDatePersian = '" & mFirstDay_PDate & "'" &
                " AND TamirRequestStateId < " & TamirRequestStates.trs_Confirm &
                IIf(mTamirRequestId > -1, " AND TamirRequestId <> " & mTamirRequestId, "")
            RemoveMoreSpaces(lSQL)
            BindingTable(lSQL, mCnn, lDs, "View_SumWaitPowers")

            If Not lDs.Tables("View_SumWaitPowers") Is Nothing AndAlso lDs.Tables("View_SumWaitPowers").Rows.Count > 0 AndAlso Not IsDBNull(lDs.Tables("View_SumWaitPowers").Rows(0)("SumWaitPower")) Then
                txtWaitPower.Text = lDs.Tables("View_SumWaitPowers").Rows(0)("SumWaitPower")
            Else
                txtWaitPower.Text = "0"
            End If

            lWaitPower = Val(txtWaitPower.Text)

            '---------------------------
            '  Compute DC Wanted Powers
            '---------------------------
            Dim lDCWantedPower As Double
            lDCWantedPower = ComputeDCWantedPower()

            '---------------------------
            '      Display Info
            '---------------------------
            lRemainPower = mMaxPower - (lRowWeak.UsedPower + lRowWeak.LastExtraPower + lWaitPower + lDCWantedPower - lRowWeak.DCWantedPower)

            txtMaxPower.Text = mMaxPower.ToString("0.##")
            txtUsedPower.Text = Val(lRowWeak.UsedPower - lRowWeak.DCWantedPower).ToString("0.##")
            txtLastUsedPower.Text = Val(lRowWeak.LastExtraPower).ToString("0.##")
            txtDCWantedPower.Text = lDCWantedPower.ToString("0.##")

            If lRemainPower >= 0 Then
                txtRemainPower.Text = lRemainPower.ToString("0.##")
            Else
                txtRemainPower.Text = -lRemainPower.ToString("0.##") & " مازاد"
            End If

        Catch ex As Exception
            ShowError(ex)
        End Try

    End Sub
    Private Function GetWeakPowerInfo(ByVal aAreaId As Integer, ByVal aSaturdayDT As DateTime, ByVal aSaturdayDatePersian As String, ByVal aPower As Double) As DatasetTamir.TblWeakPowerRow
        GetWeakPowerInfo = Nothing

        Dim lDs As New DataSet
        Dim lRow As DatasetTamir.TblWeakPowerRow, lPowerRow As DatasetTamir.TblWeakPowerRow
        Dim lDT As DateTime
        Dim lDatePersian As String
        Dim lKh As New KhayamDateTime
        Dim lServer121Id As Integer
        Dim lSQl As String
        Dim lLastPower As Double
        Dim lDCWantedPower As Double

        Try

            lLastPower = 0

            lSQl = "SELECT Server121Id FROM Tbl_Area WHERE AreaId = " & aAreaId
            BindingTable(lSQl, mCnn, lDs, "Tbl_Area")
            lServer121Id = lDs.Tables("Tbl_Area").Rows(0)("Server121Id")

            lSQl =
                "SELECT * FROM TblWeakPower WHERE SaturdayDatePersian = '" & aSaturdayDatePersian & "'" &
                " AND Server121Id = " & lServer121Id & " AND AreaId " & IIf(mIsServer121, "IS NULL", "= " & aAreaId)
            mDs.TblWeakPower.Clear()
            BindingTable(lSQl, mCnn, mDs, "TblWeakPower")

            If mDs.TblWeakPower.Rows.Count > 0 Then

                lPowerRow = mDs.TblWeakPower.Rows(0)

            Else

                lSQl =
                    "SELECT TOP 1 * FROM TblWeakPower WHERE SaturdayDatePersian < '" & aSaturdayDatePersian & "'" &
                    " AND Server121Id = " & lServer121Id & " AND AreaId " & IIf(mIsServer121, "IS NULL", "= " & aAreaId) &
                    " ORDER BY SaturdayDT DESC"
                BindingTable(lSQl, mCnn, mDs, "TblWeakPower")

                If mDs.TblWeakPower.Rows.Count > 0 Then
                    lRow = mDs.TblWeakPower.Rows(0)
                    lDT = lRow("SaturdayDT")
                    lDatePersian = lRow("SaturdayDatePersian")
                    lLastPower = lRow.LastExtraPower + lRow.UsedPower - lRow.MaxPower
                    If lLastPower < 0 Then lLastPower = 0
                Else
                    lDT = aSaturdayDT.AddDays(-7)
                    lKh.SetMiladyDate(lDT)
                    lDatePersian = lKh.GetShamsiDateStr()
                    lLastPower = 0
                End If

                Do Until lDatePersian >= aSaturdayDatePersian
                    lDT = lDT.AddDays(7)
                    lKh.SetMiladyDate(lDT)
                    lDatePersian = lKh.GetShamsiDateStr()
                    Dim lAreaId As Integer
                    lAreaId = Nothing
                    If Not mIsServer121 Then
                        lAreaId = aAreaId
                    End If

                    lPowerRow = mDs.TblWeakPower.AddTblWeakPowerRow(GetAutoInc(), lDT, lDatePersian, lServer121Id, lAreaId, 0, mMaxPower, lLastPower, 0, mIsTamirPowerMonthly)
                    lLastPower = 0
                    If mIsServer121 Then
                        lPowerRow("AreaId") = DBNull.Value
                    End If
                Loop

            End If

            lDCWantedPower = ComputeDCWantedPower()

            lPowerRow.UsedPower += (aPower + lDCWantedPower - lPowerRow.DCWantedPower)
            If lPowerRow.UsedPower < 0 Then
                lPowerRow.UsedPower = 0
            End If
            lPowerRow.DCWantedPower = lDCWantedPower
            lLastPower = lPowerRow.LastExtraPower

            '--> Computing other weakpowers after this week
            lSQl =
                "SELECT * FROM TblWeakPower WHERE SaturdayDatePersian > '" & aSaturdayDatePersian & "'" &
                " AND Server121Id = " & lServer121Id & " AND AreaId " & IIf(mIsServer121, "IS NULL", "= " & aAreaId) &
                " ORDER BY SaturdayDatePersian"
            BindingTable(lSQl, mCnn, mDs, "TblWeakPower")

            mDs.TblWeakPower.DefaultView.Sort = "SaturdayDatePersian ASC"
            For Each lVRow As DataRowView In mDs.TblWeakPower.DefaultView
                lRow = lVRow.Row

                If lRow.SaturdayDatePersian > aSaturdayDatePersian Then
                    lRow.LastExtraPower = lLastPower
                End If

                lLastPower = lRow.LastExtraPower + lRow.UsedPower - lRow.MaxPower
                If lLastPower < 0 Then lLastPower = 0

            Next
            '----------------------------------------------


            Return lPowerRow

        Catch ex As Exception
            ShowError(ex)
        End Try

    End Function

    Private Sub ComputeUsedPowerMonthly()
        If rbIsRequestByFogheTozi.Checked And cmbTamirNetworkType.SelectedValue = TamirNetworkType.FT Then Exit Sub
        If mIsLoading Then Exit Sub
        If mFirstDay_PDate = "" Then Exit Sub

        Dim lDs As New DatasetTamir
        Dim lSQL As String
        Dim lServer121Id As Integer, lAreaId As Integer = -1
        Dim lRowWeak As DatasetTamir.TblWeakPowerRow, lRow As DatasetTamir.TblWeakPowerRow
        Dim lDT As DateTime
        Dim lKh As New KhayamDateTime
        Dim lDatePersian As String
        Dim lLastPower As Double

        Try
            If cmbArea.SelectedIndex = -1 Then
                'txtMaxPower.Text = "0"
                'txtUsedPower.Text = "0"
                'Exit Sub
                lAreaId = WorkingAreaId
            Else
                lAreaId = cmbArea.SelectedValue
            End If

            'Try
            '    Dim ldDDT As DateTime = GetFirstDateOfMonth(txtDateDisconnect.MiladiDT)
            '    Dim lpDDT As String = GetFirstDatePersianOfMonth(txtDateDisconnect.MiladiDT)
            'Catch ex As Exception
            'End Try

            If txtDateDisconnect.IsOK Then
                mFirstDay_DT = GetFirstDateOfTamirScope(txtDateDisconnect.MiladiDT)
                mFirstDay_PDate = GetFirstDatePersianOfTamirScope(mFirstDay_DT)
            End If

            mMaxPower = -1

            lSQL = "SELECT Server121Id FROM Tbl_Area WHERE AreaId = " & lAreaId
            BindingTable(lSQL, mCnn, lDs, "Tbl_Area")
            lServer121Id = lDs.Tables("Tbl_Area").Rows(0)("Server121Id")

            '------------------------------
            lSQL =
                "SELECT * FROM Tbl_TamirInfo WHERE " &
                "Server121Id = " & lServer121Id & " AND AreaId " & IIf(mIsServer121, "IS NULL", "= " & lAreaId)
            BindingTable(lSQL, mCnn, lDs, "Tbl_TamirInfo")
            If lDs.Tbl_TamirInfo.Rows.Count > 0 Then
                mMaxPower = lDs.Tbl_TamirInfo.Rows(0)("MaxWeakPower")
            End If
            '------------------------------


            lSQL =
                "SELECT * FROM TblWeakPower WHERE SaturdayDatePersian = '" & mFirstDay_PDate & "'" &
                " AND IsMonthly = 1" &
                " AND Server121Id = " & lServer121Id & " AND AreaId " & IIf(mIsServer121, "IS NULL", "= " & lAreaId)
            BindingTable(lSQL, mCnn, lDs, "TblWeakPower")

            If lDs.TblWeakPower.Rows.Count > 0 Then

                lRowWeak = lDs.TblWeakPower.Rows(0)

            Else

                lSQL =
                    "SELECT TOP 1 * FROM TblWeakPower WHERE SaturdayDatePersian < '" & mFirstDay_PDate & "'" &
                    " AND IsMonthly = 1" &
                    " AND Server121Id = " & lServer121Id & " AND AreaId " & IIf(mIsServer121, "IS NULL", "= " & lAreaId) &
                    " ORDER BY SaturdayDT DESC"
                BindingTable(lSQL, mCnn, lDs, "TblWeakPower")

                If lDs.TblWeakPower.Rows.Count > 0 Then
                    lRow = lDs.TblWeakPower.Rows(0)
                    lDT = lRow("SaturdayDT")
                    lDatePersian = lRow("SaturdayDatePersian")
                    lLastPower = lRow.LastExtraPower + lRow.UsedPower - lRow.MaxPower
                    If lLastPower < 0 Then lLastPower = 0
                Else
                    'lDT = mFirstDay_DT.AddMonths(-1)
                    lDT = KhayamDateTime.AddShamsiMonth(mFirstDay_DT, -1)
                    lKh.SetMiladyDate(lDT)
                    lDatePersian = lKh.GetShamsiDateStr()
                    lLastPower = 0
                End If

                Do Until lDatePersian >= mFirstDay_PDate
                    'lDT = lDT.AddMonths(1)
                    lDT = KhayamDateTime.AddShamsiMonth(lDT, 1)
                    lKh.SetMiladyDate(lDT)
                    lDatePersian = lKh.GetShamsiDateStr()
                    lRowWeak = lDs.TblWeakPower.AddTblWeakPowerRow(GetAutoInc(), lDT, lDatePersian, lServer121Id, lAreaId, 0, mMaxPower, lLastPower, 0, mIsTamirPowerMonthly)
                    lLastPower = 0
                Loop
                If lDT > mFirstDay_DT Then
                    lDT = mFirstDay_DT
                End If

            End If

            If Not IsDBNull(lRowWeak("MaxPower")) AndAlso lRowWeak("MaxPower") > -1 Then
                mMaxPower = lRowWeak("MaxPower")
            End If

            If mMaxPower = -1 Then
                mMaxPower = 0
            End If

            '-----------------------
            '  Compute Wait Powers
            '-----------------------
            Dim lWaitPower As Double
            Dim lRemainPower As Double

            lSQL =
                "SELECT Sum(DisconnectPower) As SumWaitPower FROM TblTamirRequest " &
                " WHERE SaturdayDatePersian = '" & mFirstDay_PDate & "'" &
                " AND TamirRequestStateId < " & TamirRequestStates.trs_Confirm &
                IIf(mTamirRequestId > -1, " AND TamirRequestId <> " & mTamirRequestId, "")
            RemoveMoreSpaces(lSQL)
            BindingTable(lSQL, mCnn, lDs, "View_SumWaitPowers")

            If Not lDs.Tables("View_SumWaitPowers") Is Nothing AndAlso lDs.Tables("View_SumWaitPowers").Rows.Count > 0 AndAlso Not IsDBNull(lDs.Tables("View_SumWaitPowers").Rows(0)("SumWaitPower")) Then
                txtWaitPower.Text = lDs.Tables("View_SumWaitPowers").Rows(0)("SumWaitPower")
            Else
                txtWaitPower.Text = "0"
            End If

            lWaitPower = Val(txtWaitPower.Text)

            '---------------------------
            '  Compute DC Wanted Powers
            '---------------------------
            Dim lDCWantedPower As Double
            lDCWantedPower = ComputeDCWantedPower()

            '---------------------------
            '      Display Info
            '---------------------------
            lRemainPower = mMaxPower - (lRowWeak.UsedPower + lRowWeak.LastExtraPower + lWaitPower + lDCWantedPower - lRowWeak.DCWantedPower)

            txtMaxPower.Text = mMaxPower.ToString("0.##")
            txtUsedPower.Text = Val(lRowWeak.UsedPower - lRowWeak.DCWantedPower).ToString("0.##")
            txtLastUsedPower.Text = Val(lRowWeak.LastExtraPower).ToString("0.##")
            txtDCWantedPower.Text = lDCWantedPower.ToString("0.##")

            If lRemainPower >= 0 Then
                txtRemainPower.Text = lRemainPower.ToString("0.##")
            Else
                txtRemainPower.Text = -lRemainPower.ToString("0.##") & " مازاد"
            End If

        Catch ex As Exception
            ShowError(ex)
        End Try

    End Sub
    Private Function GetWeakPowerInfoMonthly(ByVal aAreaId As Integer, ByVal aFirstDay_DT As DateTime, ByVal aFirstDay_DatePersian As String, ByVal aPower As Double) As DatasetTamir.TblWeakPowerRow
        GetWeakPowerInfoMonthly = Nothing

        Dim lDs As New DataSet
        Dim lRow As DatasetTamir.TblWeakPowerRow, lPowerRow As DatasetTamir.TblWeakPowerRow
        Dim lDT As DateTime
        Dim lDatePersian As String
        Dim lKh As New KhayamDateTime
        Dim lServer121Id As Integer
        Dim lSQl As String
        Dim lLastPower As Double
        Dim lDCWantedPower As Double

        Try

            lLastPower = 0

            lSQl = "SELECT Server121Id FROM Tbl_Area WHERE AreaId = " & aAreaId
            BindingTable(lSQl, mCnn, lDs, "Tbl_Area")
            lServer121Id = lDs.Tables("Tbl_Area").Rows(0)("Server121Id")

            lSQl =
                "SELECT * FROM TblWeakPower WHERE SaturdayDatePersian = '" & aFirstDay_DatePersian & "'" &
                " AND IsMonthly = 1" &
                " AND Server121Id = " & lServer121Id & " AND AreaId " & IIf(mIsServer121, "IS NULL", "= " & aAreaId)
            mDs.TblWeakPower.Clear()
            BindingTable(lSQl, mCnn, mDs, "TblWeakPower")

            If mDs.TblWeakPower.Rows.Count > 0 Then

                lPowerRow = mDs.TblWeakPower.Rows(0)

            Else

                lSQl =
                    "SELECT TOP 1 * FROM TblWeakPower WHERE SaturdayDatePersian < '" & aFirstDay_DatePersian & "'" &
                    " AND IsMonthly = 1" &
                    " AND Server121Id = " & lServer121Id & " AND AreaId " & IIf(mIsServer121, "IS NULL", "= " & aAreaId) &
                    " ORDER BY SaturdayDT DESC"
                BindingTable(lSQl, mCnn, mDs, "TblWeakPower")

                If mDs.TblWeakPower.Rows.Count > 0 Then
                    lRow = mDs.TblWeakPower.Rows(0)
                    lDT = lRow("SaturdayDT")
                    lDatePersian = lRow("SaturdayDatePersian")
                    lLastPower = lRow.LastExtraPower + lRow.UsedPower - lRow.MaxPower
                    If lLastPower < 0 Then lLastPower = 0
                Else
                    'lDT = aFirstDay_DT.AddDays(-7)
                    lDT = KhayamDateTime.AddShamsiMonth(aFirstDay_DT, -1)
                    lKh.SetMiladyDate(lDT)
                    lDatePersian = lKh.GetShamsiDateStr()
                    lLastPower = 0
                End If

                Do Until lDatePersian >= mFirstDay_PDate
                    'lDT = lDT.AddDays(7)
                    lDT = KhayamDateTime.AddShamsiMonth(lDT, 1)
                    lKh.SetMiladyDate(lDT)
                    lDatePersian = lKh.GetShamsiDateStr()
                    lPowerRow = mDs.TblWeakPower.AddTblWeakPowerRow(GetAutoInc(), lDT, lDatePersian, lServer121Id, IIf(mIsServer121, Nothing, aAreaId), 0, mMaxPower, lLastPower, 0, mIsTamirPowerMonthly)
                    lLastPower = 0
                Loop
                If lDT > aFirstDay_DT Then
                    lDT = aFirstDay_DT
                End If

            End If

            If mIsServer121 Then
                lPowerRow("AreaId") = DBNull.Value
            End If

            lDCWantedPower = ComputeDCWantedPower()

            lPowerRow.UsedPower += (aPower + lDCWantedPower - lPowerRow.DCWantedPower)
            If lPowerRow.UsedPower < 0 Then
                lPowerRow.UsedPower = 0
            End If
            lPowerRow.DCWantedPower = lDCWantedPower
            lLastPower = lPowerRow.LastExtraPower

            '--> Computing other weakpowers after this week
            lSQl =
                "SELECT * FROM TblWeakPower WHERE SaturdayDatePersian > '" & aFirstDay_DatePersian & "'" &
                " AND Server121Id = " & lServer121Id & " AND AreaId " & IIf(mIsServer121, "IS NULL", "= " & aAreaId) &
                " ORDER BY SaturdayDatePersian"
            BindingTable(lSQl, mCnn, mDs, "TblWeakPower")

            mDs.TblWeakPower.DefaultView.Sort = "SaturdayDatePersian ASC"
            For Each lVRow As DataRowView In mDs.TblWeakPower.DefaultView
                lRow = lVRow.Row

                If lRow.SaturdayDatePersian > aFirstDay_DatePersian Then
                    lRow.LastExtraPower = lLastPower
                End If

                lLastPower = lRow.LastExtraPower + lRow.UsedPower - lRow.MaxPower
                If lLastPower < 0 Then lLastPower = 0

            Next
            '----------------------------------------------


            Return lPowerRow

        Catch ex As Exception
            ShowError(ex)
        End Try

    End Function

    Private Function ComputeDCWantedPower() As Double
        ComputeDCWantedPower = 0.0
        If cmbArea.SelectedIndex = -1 Then Exit Function

        '---------------------------
        '  Compute DC Wanted Powers
        '---------------------------
        Dim lKh As New KhayamDateTime
        Dim lDCWantedPower As Double
        Dim lFridayPDate As String
        Dim lDs As New DataSet
        Dim lSQL As String

        lKh.SetMiladyDate(mFirstDay_DT.AddDays(6))
        lFridayPDate = lKh.GetShamsiDateStr

        Try

            lSQL =
                "SELECT Sum(TblMPRequest.DisconnectPower) As SumDCWantedPower " &
                " FROM TblMPRequest " &
                " INNER JOIN TblRequest ON TblMPRequest.MPRequestId = TblRequest.MPRequestId" &
                " INNER JOIN Tbl_Area ON TblRequest.AreaId = Tbl_Area.AreaId " &
                " WHERE TblRequest.IsTamir = 1 " &
                " AND " & IIf(Not mIsServer121, "TblRequest.AreaId = " & cmbArea.SelectedValue, "Tbl_Area.Server121Id = " & CType(cmbArea.SelectedItem, DataRowView).Row("Server121Id")) &
                " AND ((TblMPRequest.DisconnectReasonId < 1200 OR TblMPRequest.DisconnectReasonId > 1299 OR TblMPRequest.DisconnectReasonId IS NULL) AND TblMPRequest.DisconnectGroupSetId <> 1129 AND TblMPRequest.DisconnectGroupSetId <> 1130)" &
                " AND (TblMPRequest.DisconnectDatePersian >= '" & mFirstDay_PDate & "' AND TblMPRequest.DisconnectDatePersian <= '" & lFridayPDate & "')" &
                " AND TblMPRequest.EndJobStateId IN ( " & EndJobStates.ejs_Done & "," & EndJobStates.ejs_TemporaryDone & ")" &
                " AND NOT RequestId IN (" &
                " 	SELECT RequestId FROM TblTamirRequestConfirm WHERE IsConfirm = 1 " &
                " )"

            RemoveMoreSpaces(lSQL)
            BindingTable(lSQL, mCnn, lDs, "View_SumDCWantedPowers")

            If Not lDs.Tables("View_SumDCWantedPowers") Is Nothing AndAlso lDs.Tables("View_SumDCWantedPowers").Rows.Count > 0 AndAlso Not IsDBNull(lDs.Tables("View_SumDCWantedPowers").Rows(0)("SumDCWantedPower")) Then
                lDCWantedPower = Val(lDs.Tables("View_SumDCWantedPowers").Rows(0)("SumDCWantedPower"))
            Else
                lDCWantedPower = 0.0
            End If

            Return lDCWantedPower

        Catch ex As Exception
            ShowError(ex)
        End Try

    End Function
    Private Function SaveNazer() As Boolean
        SaveNazer = False
        If cmbNazer.SelectedIndex = -1 Then
            ShowError("لطفا ناظر را تعيين نماييد")
            Exit Function
        End If
        Try
            Dim lDs As New DatasetTamir
            Dim lRow As DatasetTamir.TblTamirRequestRow
            Dim lSQL As String

            lSQL = "SELECT * FROM TblTamirRequest WHERE TamirRequestId = " & mTamirRequestId
            BindingTable(lSQL, mCnn, lDs, "TblTamirRequest", , , True)

            If lDs.Tables("TblTamirRequest").Rows.Count > 0 Then
                lRow = lDs.Tables("TblTamirRequest").Rows(0)
                lRow("NazerId") = IIf(cmbNazer.SelectedIndex > -1, cmbNazer.SelectedValue, DBNull.Value)
                lRow("IsConfirmNazer") = chkIsConfirmNazer.Checked
                If chkIsConfirmNazer.Checked Then
                    lRow("ConfirmNazerAreaUserId") = WorkingUserId
                Else
                    lRow("ConfirmNazerAreaUserId") = DBNull.Value
                End If

                Dim lUpdate As New frmUpdateDataset
                If lUpdate.UpdateDataSet("TblTamirRequest", lDs, lRow("AreaId")) Then
                    mEditingRow("NazerId") = lRow("NazerId")
                    mEditingRow("IsConfirmNazer") = lRow("IsConfirmNazer")
                    mEditingRow("ConfirmNazerAreaUserId") = lRow("ConfirmNazerAreaUserId")
                    SaveNazer = True
                End If
            End If
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Function
    Private Sub LoadFeederParts()
        DatasetBT1.Tbl_FeederPart.Clear()
        If cmbMPFeeder.SelectedIndex = -1 Then Exit Sub

        Dim lSQL As String = ""
        Dim lMPFeederId As Integer = cmbMPFeeder.SelectedValue

        Try
            lSQL = "SELECT * FROM Tbl_FeederPart WHERE AreaId = " & cmbArea.SelectedValue & " AND MPFeederId = " & lMPFeederId
            BindingTable(lSQL, mCnn, DatasetBT1, "Tbl_FeederPart", , True)
        Catch ex As Exception
            If ex.Message.IndexOf("Execute requires the command to have a transaction") = -1 Then
                ShowError(ex)
            End If
        End Try

        cmbFeederPart.SelectedIndex = -1
        cmbFeederPart.SelectedIndex = -1
    End Sub
    Private Sub LoadLPPosts()
        DatasetBT1.Tbl_LPPost.Clear()
        If cmbMPFeeder.SelectedIndex = -1 Then Exit Sub

        Dim lSQL As String = ""
        Dim lMPFeederId As Integer = cmbMPFeeder.SelectedValue

        Try
            lSQL = "exec spGetLPPosts " & cmbArea.SelectedValue & "," & lMPFeederId & "," & IIf(mEditingRow("TamirRequestStateId") < TamirRequestStates.trs_Confirm, "0", "1")
            BindingTable(lSQL, mCnn, DatasetBT1, "Tbl_LPPost", , True)
        Catch ex As Exception
            If ex.Message.IndexOf("Execute requires the command to have a transaction") = -1 Then
                ShowError(ex)
            End If
        End Try

        cmbLPPost.SelectedIndex = -1
        cmbLPPost.SelectedIndex = -1
    End Sub
    Private Sub SetDateBoxes()
        If cmbPeriod.SelectedIndex = -1 Then Exit Sub

        Try
            Dim lNow As DateTime = GetServerTimeInfo().MiladiDate

            pnlDate.Enabled = cmbPeriod.SelectedValue = "manual"
            pnlDCDayCount.Visible = cmbPeriod.SelectedValue = "ndays"
            pnlLastDCCount.Visible = cmbPeriod.SelectedValue = "nDCs"
            pnlDate.Enabled = cmbPeriod.SelectedValue = "manual"
            If cmbPeriod.SelectedValue <> "manual" Then
                txtDateTo.MiladiDT = lNow
            End If

            Select Case cmbPeriod.SelectedValue
                Case "week"
                    txtDateFrom.MiladiDT = lNow.AddDays(-7)
                Case "month"
                    txtDateFrom.MiladiDT = lNow.AddDays(-30)
                Case "season"
                    txtDateFrom.MiladiDT = lNow.AddDays(-90)
                Case "year"
                    txtDateFrom.MiladiDT = lNow.AddDays(-365)
                Case "ndays"
                    Dim lCntDays As Integer = Val(txtFeederDCDayCount.Text)
                    If lCntDays <= 0 Then
                        lCntDays = 30
                        txtFeederDCDayCount.Text = 30
                    End If
                    If lCntDays > 0 Then
                        txtDateFrom.MiladiDT = lNow.AddDays(-lCntDays)
                    End If
                Case "nDCs"
                    txtDateFrom.EmptyDate()
                    txtDateTo.EmptyDate()
            End Select

        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub
    Private Sub LaodUserCombos()
        Dim lValue As Object = DBNull.Value

        Try

            If mEditingRow Is Nothing Then Exit Sub
            lValue = mEditingRow("ConfirmNahiehAreaUserId")
            cmbConfirmNaheihUsername.SelectedValue = lValue
            lValue = mEditingRow("ConfirmCenterAreaUserId")
            cmbConfirmCenterUsername.SelectedValue = lValue

            If Not mEditingRowConfirm Is Nothing Then
                lValue = mEditingRowConfirm("AreaUserId")
                cmbConfirmSetadUsername.SelectedValue = lValue
                cmbConfirmUserName.SelectedValue = lValue
            ElseIf Not (mIsSetadConfirmPlanned Or (mIsForConfirm And Not mIsForWarmLineConfirm)) Then 'Or Not IsCenter
                cmbConfirmSetadUsername.SelectedValue = DBNull.Value
                cmbConfirmUserName.SelectedValue = DBNull.Value
            End If

        Catch ex As Exception
        End Try
    End Sub
    Private Function CheckEmergency() As Boolean
        CheckEmergency = True
        If Not chkIsEmergency.Checked Then
            btnEmergencyCancel_Click(Nothing, Nothing)
            'chkIsSendToSetad.Checked = mLastIsSendToSetadMode
            'chkIsSendToSetad.Enabled = True
            'If Not IsCenter And Not IsMiscMode Then
            '    chkIsSendToSetad.Text = "ارسال به مرکز"
            'Else
            '    chkIsSendToSetad.Text = "تأييد مدير و ارسال به ستاد"
            'End If
            'txtEmergencyReasonDisplay.Text = ""
            'pnlEmergency.Hide()
            Exit Function
        End If
        'If IsSetadMode Then Exit Function
        If Not IsDBNull(mEditingRow("EmergencyRequestId")) Then Exit Function
        CheckEmergency = False

        'Dim lAns As DialogResult
        Dim lMsg As String

        Try

            lMsg =
                "شما در حال ثبت يک درخواست خاموشي بابرنامه از نوع بدون نياز به تأييد مي‌باشيد." &
                vbCrLf & vbCrLf &
                "توجه نماييد:" & vbCrLf &
                "پرونده خاموشي اين درخواست بلافاصله ايجاد خواهد شد." & vbCrLf &
                "در صورتي که اين خاموشي اعمال شده و مجوز آن در ستاد صادر نشود، عواقب آن بر عهده کاربر ثبت کننده درخواست خواهد بود."
            lblEmergency.Text = lMsg

            pnlEmergency.Show()
            pnlEmergency.BringToFront()
            mSaveEnabled = btnSave.Enabled
            btnSave.Enabled = False

            tbcMain.Enabled = False

            'lAns = GetConfirm(lMsg, "هشدار", MessageBoxButtons.OKCancel, MsgBoxIcon.MsgIcon_Warning, MessageBoxDefaultButton.Button1)

            'If lAns = DialogResult.OK Then
            '    If Not chkIsLPPostFeederPart.Checked Then
            '        chkIsSendToSetad.Checked = True
            '        chkIsSendToSetad.Enabled = False
            '    End If
            '    chkIsSendToSetad.Text = "ارسال به ستاد"
            '    CheckEmergency = True
            'Else
            '    chkIsEmergency.Checked = False
            '    chkIsSendToSetad.Checked = mLastIsSendToSetadMode
            '    chkIsSendToSetad.Enabled = True
            '    If Not IsCenter And Not IsMiscMode Then
            '        chkIsSendToSetad.Text = "ارسال به مرکز"
            '    Else
            '        chkIsSendToSetad.Text = "تأييد مدير و ارسال به ستاد"
            '    End If
            'End If

        Catch ex As Exception
            ShowError(ex)
        End Try

    End Function
    Private Sub LoadSensitiveSubscribers()
        Dim lSQL As String = ""
        Dim lDs As New DataSet
        Dim lTbl As DataTable
        lSQL =
            " SELECT SubscriberId, Name, Address, Telephone, TelMobile, TelFax, EmailAddress " &
            " FROM Tbl_Subscriber " &
            " WHERE SubscriberSensitivityId = 3 AND MPFeederId = " & cmbMPFeeder.SelectedValue &
            " ORDER BY MPFeederId "
        RemoveMoreSpaces(lSQL)
        BindingTable(lSQL, mCnn, lDs, "Tbl_MPSubs")

        If Not mIsManualChangeCL Then
            txtCriticalLocations.Text = ""
        End If

        If Not lDs.Tables.Contains("Tbl_MPSubs") OrElse lDs.Tables("Tbl_MPSubs").Rows.Count = 0 Then Exit Sub

        lTbl = lDs.Tables("Tbl_MPSubs")

        Dim lSubs As String = ""
        For Each lRow As DataRow In lTbl.Rows
            lSubs &= IIf(lSubs <> "", vbCrLf, "") & lRow("Name") & " - " & lRow("Address")
        Next

        If lSubs <> "" Or Not mIsManualChangeCL Then
            mIsManualChangeCL = False
            If lSubs.Length > 299 Then
                lSubs = lSubs.Substring(0, 299)
            End If
            txtCriticalLocations.Text = lSubs
        End If
    End Sub
    Private Sub MakeRadif()
        Dim lRowEx As Janus.Windows.GridEX.GridEXRow
        Try
            Dim lRadif As Integer = 1
            Dim lVRow As DataRowView
            For i As Integer = 0 To dgFeederDC.RowCount - 1
                Try

                    lRowEx = dgFeederDC.GetRow(i)
                    lVRow = CType(lRowEx.DataRow, DataRowView)
                    If lRowEx.RowType = Janus.Windows.GridEX.RowType.Record Then
                        'lRowEx.BeginEdit()
                        'lRowEx.Cells("Radif").Value = lRadif
                        lVRow("Radif") = lRadif
                        If dgFeederDC.SelectedItems(0).Position = i Then
                            dgFeederDC.SetValue("Radif", lRadif)
                        End If
                        'lRowEx.EndEdit()
                        lRadif += 1
                    End If

                Catch ex As Exception
                End Try
            Next
        Catch ex As Exception

        End Try
    End Sub
    Private Sub ComputeLPPostCountDC()
        If mIsLoading Then Exit Sub
        If cmbMPFeeder.SelectedIndex = -1 Then
            txtLPPostCountDC.Text = 0
            Exit Sub
        End If
        Dim lSQL As String = ""

        If chkIsLPPostFeederPart.Checked Then
            If rbLPPost.Checked Then
                txtLPPostCountDC.Text = 1
                Exit Sub
            ElseIf rbFeederPart.Checked And cmbFeederPart.SelectedIndex > -1 Then
                lSQL = "SELECT Count(LPPostId) As Cnt FROM Tbl_LPPost WHERE IsActive = 1 AND FeederPartId = " & cmbFeederPart.SelectedValue
            End If
        Else
            lSQL = "SELECT Count(LPPostId) As Cnt FROM Tbl_LPPost WHERE IsActive = 1 AND MPFeederId = " & cmbMPFeeder.SelectedValue
        End If

        If lSQL = "" Then Exit Sub

        Dim lDs As New DataSet
        BindingTable(lSQL, mCnn, lDs, "ViewLPPostCountDC")
        If lDs.Tables.Contains("ViewLPPostCountDC") AndAlso lDs.Tables("ViewLPPostCountDC").Rows.Count > 0 Then
            txtLPPostCountDC.Text = lDs.Tables("ViewLPPostCountDC").Rows(0)("Cnt")
        Else
            txtLPPostCountDC.Text = 0
        End If
    End Sub

    Private Sub CheckNetworkType(ByVal aIsChange As Boolean)

        RemoveForceControls()

        Dim lNT As TamirNetworkType = TamirNetworkType.None
        If cmbTamirNetworkType.SelectedIndex > -1 Then
            lNT = cmbTamirNetworkType.SelectedValue
        End If

        If lNT = TamirNetworkType.FT Then

            chkIsLPPostFeederPart.Enabled = False
            chkIsLPPostFeederPart.Checked = False
            btnSelectLPPost.Enabled = False
            lblPowerUnit.Text = "MWh"
            lblPowerUnitManovr.Text = "MWh"

            cmbLPFeeder.Enabled = False
            btnLPFeeder.Enabled = False
            cmbLPFeeder.SelectedIndex = -1
            cmbLPFeeder.SelectedIndex = -1

            cmbMPFeeder.SelectedIndex = -1
            cmbMPFeeder.SelectedIndex = -1
            cmbMPFeeder.Visible = False
            ckcmbMPFeeder.Visible = True

            btnFeederKeyVisibleState(True)

        ElseIf lNT = TamirNetworkType.MP Then

            chkIsLPPostFeederPart.Enabled = True
            btnSelectLPPost.Enabled = True
            rbFeederPart.Enabled = True

            SetForceControl(Label9)

            If Not rbLPPost.Checked Then
                lblPowerUnit.Text = "MWh"
                lblPowerUnitManovr.Text = "MWh"
            Else
                lblPowerUnit.Text = "KWh"
                lblPowerUnitManovr.Text = "KWh"
            End If

            cmbLPFeeder.Enabled = False
            btnLPFeeder.Enabled = False
            cmbLPFeeder.SelectedIndex = -1
            cmbLPFeeder.SelectedIndex = -1

            ckcmbMPFeeder.SelectNone()
            cmbMPFeeder.Visible = True
            ckcmbMPFeeder.Visible = False

            btnFeederKeyVisibleState(True)

        ElseIf lNT = TamirNetworkType.LP Then

            chkIsLPPostFeederPart.Enabled = False
            chkIsLPPostFeederPart.Checked = True
            btnSelectLPPost.Enabled = True
            rbLPPost.Checked = True
            rbFeederPart.Enabled = False
            cmbLPFeeder.Enabled = True
            btnLPFeeder.Enabled = True
            lblPowerUnit.Text = "KWh"
            lblPowerUnitManovr.Text = "KWh"

            cmbFeederPart.SelectedIndex = -1
            cmbFeederPart.SelectedIndex = -1

            ckcmbMPFeeder.SelectNone()
            cmbMPFeeder.Visible = True
            ckcmbMPFeeder.Visible = False

            SetForceControl(Label9)

            btnFeederKeyVisibleState(False)
        Else

            DatasetDummy1.ViewDummyId.Rows(0)("Dummy10Id") = TamirNetworkType.MP
            cmbTamirNetworkType.SelectedValue = TamirNetworkType.MP
            lblPowerUnit.Text = "MWh"
            lblPowerUnitManovr.Text = "MWh"

            ckcmbMPFeeder.SelectNone()
            cmbMPFeeder.Visible = True
            ckcmbMPFeeder.Visible = False

            btnFeederKeyVisibleState(False)

        End If

        rbLPPostFeederPart_CheckedChanged(Nothing, Nothing)

    End Sub
    Private Sub LoadLPFeeders()
        DatasetBT1.Tbl_LPFeeder.Clear()
        If cmbLPPost.SelectedIndex = -1 Then Exit Sub

        Dim lSQL As String = ""
        Dim lLPPostId As Integer = cmbLPPost.SelectedValue

        Try
            lSQL = "exec spGetLPFeeders " & cmbArea.SelectedValue & "," & lLPPostId & "," & IIf(mEditingRow("TamirRequestStateId") < TamirRequestStates.trs_Confirm, "0", "1")
            BindingTable(lSQL, mCnn, DatasetBT1, "Tbl_LPFeeder", , True)
        Catch ex As Exception
            ShowError(ex)
        End Try

        cmbLPFeeder.SelectedIndex = -1
        cmbLPFeeder.SelectedIndex = -1
    End Sub
    Private Function CalculateA4LoadDiagrams(ByVal BaseDT As Date) As Double
        'Calculation of PostZaribDay
        Dim lDs As New DataSet
        Dim A As Double = 1.0
        Dim PostLoadState As Integer = 1
        Dim RelatedDoWFldInTbl_PostZaribDay As String

        Try
            Select Case BaseDT.DayOfWeek
                Case DayOfWeek.Saturday
                    RelatedDoWFldInTbl_PostZaribDay = "Sat"
                Case DayOfWeek.Sunday
                    RelatedDoWFldInTbl_PostZaribDay = "Sun"
                Case DayOfWeek.Monday
                    RelatedDoWFldInTbl_PostZaribDay = "Mon"
                Case DayOfWeek.Tuesday
                    RelatedDoWFldInTbl_PostZaribDay = "Tue"
                Case DayOfWeek.Wednesday
                    RelatedDoWFldInTbl_PostZaribDay = "Wed"
                Case DayOfWeek.Thursday
                    RelatedDoWFldInTbl_PostZaribDay = "Thu"
                Case DayOfWeek.Friday
                    RelatedDoWFldInTbl_PostZaribDay = "Fri"
            End Select
            'Calculation of PostZaribMonth
            Dim lToziTypeId As Integer = GetPostZaribToziTypeId()
            Dim RelatedHourFldInTbl_PostZaribMonth As String
            RelatedHourFldInTbl_PostZaribMonth = "PostZaribMH" & IIf(BaseDT.Hour = 0, "24", Format(BaseDT.Hour + 1, "0#"))
            Dim SQL As String
            SQL = "SELECT " & RelatedHourFldInTbl_PostZaribMonth & " * " &
                  "(SELECT " & RelatedDoWFldInTbl_PostZaribDay & " FROM Tbl_PostZaribDay WHERE PostLoadState=" & PostLoadState & " AND ToziTypeId = " & lToziTypeId & ") AS A " &
                  "FROM Tbl_PostZaribMonth WHERE PostLoadState=" & PostLoadState &
                  " AND MonthId=" & GetPersianDate(BaseDT).Substring(5, 2)
            If Not IsNothing(lDs.Tables("Tbl_MultiplierModule")) Then
                lDs.Tables("Tbl_MultiplierModule").Clear()
            End If
            BindingTable(SQL, mCnn, lDs, "Tbl_MultiplierModule")
            If lDs.Tables("Tbl_MultiplierModule").Rows.Count > 0 Then
                A = lDs.Tables("Tbl_MultiplierModule").Rows(0).Item("A")
            Else
                A = 1
            End If
        Catch ex As Exception
        End Try

        Return A
    End Function
    Private Function FindOverlapTimes(Optional ByVal aIsCurrentDC As Boolean = True, Optional ByVal aIsMPFeeder As Boolean = True) As Integer
        FindOverlapTimes = 0
        mOverlapTime = 0

        If Not aIsCurrentDC And Not (txtDateDisconnect.IsOK And txtDateConnect.IsOK) Then
            ShowError("لطفا زمان قطع و وصل را مشخص نماييد", False, MsgBoxIcon.MsgIcon_Exclamation)
            Exit Function
        End If

        If aIsCurrentDC And Not (txtDateDisconnect.IsOK And txtDateConnect.IsOK And txtTimeDisconnect.IsOK And txtTimeConnect.IsOK) Then
            Exit Function
        End If

        Dim lSQL As String = ""
        Dim lViewName As String = ""

        If aIsCurrentDC Then
            txtDateDisconnect.InsertTime(txtTimeDisconnect)
            txtDateConnect.InsertTime(txtTimeConnect)
            lViewName = "ViewFTOverLaps"
        Else
            txtDateDisconnect.InsertTime(0, 0, 0)
            txtDateConnect.InsertTime(23, 59, 59)
            lViewName = "ViewFTDCs"
        End If

        Dim lDate1 As DateTime = txtDateDisconnect.MiladiDT
        Dim lDate2 As DateTime = txtDateConnect.MiladiDT
        Dim lDCInterval As Integer = DateDiff(DateInterval.Minute, lDate1, lDate2)

        Dim lTblOverlap As DatasetTamir.TblDCOverlapDataTable = mDs.TblDCOverlap
        Dim lLastRow As DataRow = Nothing

        Dim lCTS As Integer = 0
        Dim lDBL As Integer = 0
        Dim lDoubleTime As Integer = 0
        Dim lOverlapTime As Integer = 0

        Dim lMPPost As String = "-1"
        Dim lMPFeeder As String = "-1"

        Dim lMPFeeders() As String

        Try
            If cmbMPPost.SelectedIndex > -1 Then
                lMPPost = cmbMPPost.SelectedValue
            End If
            If aIsMPFeeder Then
                If Not rbIsRequestByFogheTozi.Checked AndAlso cmbMPFeeder.SelectedIndex > -1 Then
                    lMPFeeder = cmbMPFeeder.SelectedValue
                    'ElseIf ckcmbMPFeeder.Visible Then
                    '    lMPFeeders = ckcmbMPFeeder.GetDataListArray
                    '    If lMPFeeders.Length > 0 Then
                    '        lMPFeeder = lMPFeeders(0)
                    '    End If
                End If
            End If

            Dim lNetworkTypeId As Integer = -1
            If cmbTamirNetworkType.SelectedIndex > -1 Then
                lNetworkTypeId = cmbTamirNetworkType.SelectedValue
            End If

            Dim lLPPostId As Integer = -1
            Dim lLPFeederId As Integer = -1
            Dim lFeederPartId As Integer = -1

            If cmbLPPost.SelectedIndex > -1 Then
                lLPPostId = cmbLPPost.SelectedValue
            End If
            If cmbLPFeeder.SelectedIndex > -1 Then
                lLPFeederId = cmbLPFeeder.SelectedValue
            End If
            If cmbFeederPart.SelectedIndex > -1 Then
                lFeederPartId = cmbFeederPart.SelectedValue
            End If

            lSQL =
                "EXEC spFindFTOverlaps '" &
                lDate1.ToString("yyyy/MM/dd HH:mm:ss") & "'" &
                ", '" & lDate2.ToString("yyyy/MM/dd HH:mm:ss") & "'" &
                ", " & lMPPost &
                ", " & lMPFeeder &
                ", " & mRequestId &
                ", " & lNetworkTypeId &
                ", " & lLPPostId &
                ", " & lLPFeederId &
                ", " & lFeederPartId

            BindingTable(lSQL, mCnn, mDs, lViewName, , , , , , , True)

            Try
                If aIsCurrentDC Then
                    dgOverlaps.DataSource = mDs.Tables(lViewName)
                Else
                    dgSOverlaps.DataSource = mDs.Tables(lViewName)
                End If
            Catch ex As Exception
            End Try

            If aIsCurrentDC Then
                DeleteRecords(lTblOverlap, "")

                If mDs.Tables(lViewName).Rows.Count > 0 Then

                    For Each lRowFT As DataRow In mDs.Tables(lViewName).Rows
                        lCTS = lRowFT("CTS")
                        If lCTS > 0 Then
                            lOverlapTime += lCTS
                        End If

                        If Not IsNothing(lLastRow) Then
                            lDBL = DateDiff(DateInterval.Minute, lLastRow("ConnectDT"), lRowFT("DisconnectDT"))
                            If lDBL < 0 Then
                                lDoubleTime += (-lDBL)
                            Else
                                lDBL = 0
                            End If
                        Else
                            lDoubleTime = 0
                        End If

                        Dim lRowORLP As DatasetTamir.TblDCOverlapRow = lTblOverlap.NewTblDCOverlapRow
                        lRowORLP.DCOverlapId = GetAutoInc()
                        lRowORLP.OverlapRequestId = lRowFT("RequestId")
                        lRowORLP.TamirRequestId = mTamirRequestId
                        lRowORLP.OverlapTime = lCTS - lDBL
                        lTblOverlap.AddTblDCOverlapRow(lRowORLP)

                        lLastRow = lRowFT
                    Next

                    lOverlapTime -= lDoubleTime
                    mOverlapTime = lOverlapTime
                    mRealDCTime = lDCInterval - lOverlapTime

                    sbtnOverlaps.BringToFront()
                    sbtnOverlaps.Show()
                Else
                    sbtnOverlaps.Hide()
                    sbtnOverlaps.SendToBack()
                End If
            End If
        Catch ex As Exception
            ShowError(ex)
        End Try

        Return lOverlapTime
    End Function
    Private Const mcMaxStep = 6

    Private Sub ClearMultiStepForm()

        Dim lTxtDate As PersianMaskedEditor = Nothing
        Dim lTxtTime As TimeMaskedEditor = Nothing
        Dim lTxtBar As TextBoxPersian = Nothing
        Dim lTxtDesc As TextBox = Nothing

        Try
            For i As Integer = mcMaxStep To 1 Step -1
                lTxtDate = Bargh_Common.mdlHashemi.FindControl(pnlMultiStepInfo.Controls, "txtMSDate" & i)
                lTxtTime = Bargh_Common.mdlHashemi.FindControl(pnlMultiStepInfo.Controls, "txtMSTime" & i)
                lTxtBar = Bargh_Common.mdlHashemi.FindControl(pnlMultiStepInfo.Controls, "txtMSBar" & i)
                lTxtDesc = Bargh_Common.mdlHashemi.FindControl(pnlMultiStepInfo.Controls, "txtMSDesc" & i)

                lTxtDate.EmptyDate()
                lTxtTime.EmptyTime()
                lTxtBar.Text = ""
                lTxtDesc.Text = ""
            Next
        Catch ex As Exception
        End Try

    End Sub
    Private Function LoadMultiStepInfo() As Double
        Dim lSumbar As Double = 0.0

        Try
            Dim lTxtDate As PersianMaskedEditor = Nothing
            Dim lTxtTime As TimeMaskedEditor = Nothing
            Dim lTxtBar As TextBoxPersian = Nothing
            Dim lTxtDesc As TextBox = Nothing

            If IsDBNull(mEditingRow("DCCurrentValue")) Then
                mEditingRow("DCCurrentValue") = Val(txtLastPeakBar.Text)
            End If
            GetDBValue(txtDCCurrent, mEditingRow("DCCurrentValue"))

            Dim i As Integer = 1
            For Each lRow As DataRow In mDs.TblTamirRequestMultiStep.Rows
                lTxtDate = Bargh_Common.mdlHashemi.FindControl(pnlMultiStepInfo.Controls, "txtMSDate" & i)
                GetDBValue(lTxtDate, lRow("MSDT"), , True)

                lTxtTime = Bargh_Common.mdlHashemi.FindControl(pnlMultiStepInfo.Controls, "txtMSTime" & i)
                GetDBValue(lTxtTime, lRow("MSTime"))

                lTxtBar = Bargh_Common.mdlHashemi.FindControl(pnlMultiStepInfo.Controls, "txtMSBar" & i)
                GetDBValue(lTxtBar, lRow("MSBar"))
                lSumbar += Val(lTxtBar.Text)

                lTxtDesc = Bargh_Common.mdlHashemi.FindControl(pnlMultiStepInfo.Controls, "txtMSDesc" & i)
                GetDBValue(lTxtDesc, lRow("MSDesc"))

                If Not mMultiStepFeederKey.ContainsKey(i) Then
                    mMultiStepFeederKey.Add(i, lRow("TamirRequestMultiStepId"))
                End If

                i += 1
                If i > mcMaxStep Then Exit For
            Next
        Catch ex As Exception
            lSumbar = -1.0
        End Try

        Return lSumbar
    End Function
    Private Function IsMultiStepsOK() As Boolean

        Dim lIsOK As Boolean = False
        Dim lMsg As String = ""
        Dim lIsEmpty As Boolean = True

        Try

            Dim lSum As Double = 0.0
            Dim lTxtDate As PersianMaskedEditor = Nothing
            Dim lTxtTime As TimeMaskedEditor = Nothing
            Dim lTxtBar As TextBoxPersian = Nothing
            Dim lTxtDesc As TextBox = Nothing
            Dim lIsNew As Boolean = False
            mIsMultiStep = False

            For i As Integer = mcMaxStep To 1 Step -1
                lTxtDate = Bargh_Common.mdlHashemi.FindControl(pnlMultiStepInfo.Controls, "txtMSDate" & i)
                lTxtTime = Bargh_Common.mdlHashemi.FindControl(pnlMultiStepInfo.Controls, "txtMSTime" & i)
                lTxtBar = Bargh_Common.mdlHashemi.FindControl(pnlMultiStepInfo.Controls, "txtMSBar" & i)
                lTxtDesc = Bargh_Common.mdlHashemi.FindControl(pnlMultiStepInfo.Controls, "txtMSDesc" & i)

                If lTxtTime.IsOK Then

                    If i = 1 Then
                        txtDateDisconnect.InsertTime(txtTimeDisconnect)
                        lTxtDate.InsertTime(lTxtTime)
                        Dim lDT1 As DateTime = txtDateDisconnect.MiladiDT
                        Dim lDT2 As DateTime = lTxtDate.MiladiDT
                        If DateDiff(DateInterval.Minute, lDT1, lDT2) <= 0 Then
                            lMsg &= "- زمان اولين وصل، بايد بعد از تاريخ قطع اوليه باشد"
                        End If
                    End If

                    If Val(lTxtBar.Text) = 0 Then
                        lMsg &= "- بار تأمين شده در مرحله " & i
                    Else
                        lSum += Val(lTxtBar.Text)
                    End If

                    If lIsEmpty Then
                        lTxtDate.InsertTime(lTxtTime)
                        mLastMSDateTime = lTxtDate.MiladiDT
                    End If

                    lIsEmpty = False
                ElseIf Not lIsEmpty Then
                    lMsg &= "- ساعت وصل در مرحله " & i
                End If
            Next

            If lSum > Val(txtDCCurrent.Text) Then
                lMsg &= "- مجموع بارهاي تأمين شده از وصل‌هاي چند مرحله‌اي بيشتر از بار قطع اوليه شده است"
            End If

            mIsMultiStep = Not lIsEmpty
            txtDateConnect.Enabled = Not mIsMultiStep
            txtTimeConnect.Enabled = Not mIsMultiStep

        Catch ex As Exception
            lMsg = ex.ToString()
        End Try

        If lMsg <> "" Then
            lMsg =
                "لطفا موارد زير را تصحيح نماييد:" & vbCrLf & vbCrLf &
                lMsg.Replace("-", vbCrLf & "-")
            ShowError(lMsg)
        Else
            lIsOK = True
        End If

        Return lIsOK
    End Function
    Private Function SetMultiStepInfo() As Double
        Dim lSumBar As Double = 0.0
        Dim i As Integer = 0

        Try

            Dim lRow As DataRow
            Dim lTxtDate As PersianMaskedEditor = Nothing
            Dim lTxtTime As TimeMaskedEditor = Nothing
            Dim lTxtBar As TextBoxPersian = Nothing
            Dim lTxtDesc As TextBox = Nothing
            Dim lIsNew As Boolean = False

            For i = 1 To mcMaxStep

                lIsNew = mDs.TblTamirRequestMultiStep.Count < i
                If lIsNew Then
                    lRow = mDs.TblTamirRequestMultiStep.NewRow()
                Else
                    lRow = mDs.TblTamirRequestMultiStep.Rows(i - 1)
                End If

                lTxtTime = Bargh_Common.mdlHashemi.FindControl(pnlMultiStepInfo.Controls, "txtMSTime" & i)
                If (lTxtTime.IsOK) Then
                    SetDBValue(lTxtTime, lRow("MSTime"))

                    lTxtDate = Bargh_Common.mdlHashemi.FindControl(pnlMultiStepInfo.Controls, "txtMSDate" & i)
                    lTxtDate.InsertTime(lTxtTime)
                    SetDBValue(lTxtDate, lRow("MSDT"), , True)
                    SetDBValue(lTxtDate, lRow("MSDatePersian"))

                    lTxtBar = Bargh_Common.mdlHashemi.FindControl(pnlMultiStepInfo.Controls, "txtMSBar" & i)
                    SetDBValue(lTxtBar, lRow("MSBar"))
                    lSumBar += Val(lTxtBar.Text)

                    lTxtDesc = Bargh_Common.mdlHashemi.FindControl(pnlMultiStepInfo.Controls, "txtMSDesc" & i)
                    SetDBValue(lTxtDesc, lRow("MSDesc"))

                    If lIsNew Then
                        lRow("TamirRequestMultiStepId") = GetAutoInc()
                        lRow("TamirRequestId") = mTamirRequestId
                    End If
                    For Each lKeyRow As DataRow In DatasetTemp1.TblTamirRequestKey.Rows
                        If lKeyRow.RowState <> DataRowState.Deleted Then
                            If Not IsDBNull(lKeyRow("TamirRequestMultiStepId")) AndAlso lKeyRow("TamirRequestMultiStepId") = i Then
                                lKeyRow("TamirRequestMultiStepId") = lRow("TamirRequestMultiStepId")
                            End If
                        End If
                    Next
                Else
                    Dim lDelArray As New ArrayList
                    lIsNew = False
                    For Each lKeyRow As DataRow In DatasetTemp1.TblTamirRequestKey.Rows
                        If lKeyRow.RowState <> DataRowState.Deleted Then
                            If Not IsDBNull(lKeyRow("TamirRequestMultiStepId")) Then
                                If (Not IsDBNull(lRow("TamirRequestMultiStepId")) AndAlso lKeyRow("TamirRequestMultiStepId") = lRow("TamirRequestMultiStepId")) _
                                 OrElse lKeyRow("TamirRequestMultiStepId") = i Then
                                    lKeyRow("TamirRequestMultiStepId") = DBNull.Value
                                    lDelArray.Add(lKeyRow)
                                End If
                            End If
                        End If
                    Next
                    For Each lDelRow As DataRow In lDelArray
                        lDelRow.Delete()
                    Next

                    lRow.Delete()
                End If

                If lIsNew And lRow.RowState <> DataRowState.Deleted Then
                    mDs.TblTamirRequestMultiStep.Rows.Add(lRow)
                End If
            Next
            If DatasetTemp1.TblTamirRequestKey.Columns.Contains("IsTemp") Then
                DatasetTemp1.TblTamirRequestKey.Columns.Remove("IsTemp")
            End If
            If DatasetTemp1.TblTamirRequestKey.Columns.Contains("IsTempOpen") Then
                DatasetTemp1.TblTamirRequestKey.Columns.Remove("IsTempOpen")
            End If
            If DatasetTemp1.TblTamirRequestKey.Columns.Contains("IsTempClose") Then
                DatasetTemp1.TblTamirRequestKey.Columns.Remove("IsTempClose")
            End If
            If DatasetTemp1.TblTamirRequestKey.Columns.Contains("TempKeyStateId") Then
                DatasetTemp1.TblTamirRequestKey.Columns.Remove("TempKeyStateId")
            End If
            SetDBValue(txtDCCurrent, mEditingRow("DCCurrentValue"))
        Catch ex As Exception
            ShowError(ex)
            lSumBar = -1.0
        End Try

        Return lSumBar
    End Function
    Private Function ComputeMultiSteps() As Double
        Dim lSum As Double = 0

        Try
            Dim lDT1 As DateTime, lDT2 As DateTime
            Dim lKh As New KhayamDateTime
            Dim lDiff As Single
            Dim lCurrentValue As Double = Val(txtDCCurrent.Text)

            txtDateDisconnect.InsertTime(txtTimeDisconnect)
            Dim lFirstDT As DateTime = txtDateDisconnect.MiladiDT
            lDT1 = lFirstDT
            For Each lRow As DataRow In mDs.TblTamirRequestMultiStep.Rows
                lKh.SetShamsiDate(lRow("MSDatePersian"))
                lDT2 = CType(lRow("MSDT"), DateTime)
                lDiff = DateDiff(DateInterval.Minute, lDT1, lDT2) / 60

                lSum += lCurrentValue * lDiff

                lDT1 = lDT2
                lCurrentValue -= lRow("MSBar")
            Next

            lDiff = DateDiff(DateInterval.Minute, lFirstDT, lDT2) / 60
            lSum /= lDiff
        Catch ex As Exception
        End Try

        Return Math.Round(lSum, 2)
    End Function
    Private Sub SetMultiStep_Enabled(ByVal aIsEnabled As Boolean)
        Dim lTxtDate As PersianMaskedEditor = Nothing
        Dim lTxtTime As TimeMaskedEditor = Nothing
        Dim lTxtBar As TextBoxPersian = Nothing
        Dim lTxtDesc As TextBox = Nothing

        txtDCCurrent.Enabled = aIsEnabled
        For i As Integer = 1 To mcMaxStep
            Try
                lTxtDate = Bargh_Common.mdlHashemi.FindControl(pnlMultiStepInfo.Controls, "txtMSDate" & i)
                lTxtDate.ReadOnly = Not aIsEnabled
                lTxtTime = Bargh_Common.mdlHashemi.FindControl(pnlMultiStepInfo.Controls, "txtMSTime" & i)
                lTxtTime.ReadOnly = Not aIsEnabled
                lTxtBar = Bargh_Common.mdlHashemi.FindControl(pnlMultiStepInfo.Controls, "txtMSBar" & i)
                lTxtBar.ReadOnly = Not aIsEnabled
                lTxtDesc = Bargh_Common.mdlHashemi.FindControl(pnlMultiStepInfo.Controls, "txtMSDesc" & i)
                lTxtDesc.ReadOnly = Not aIsEnabled
            Catch ex As Exception
            End Try
        Next
    End Sub

    Private Sub RemoveForceControls()
        RemoveForceControl(Label9)

        RemoveForceControl(lblLPPost)
        RemoveForceControl(lblFeederPart)
    End Sub

    Private Sub CheckTamirType()
        Try
            Dim lTamirRequestMonthDay As Integer = Val(CConfig.ReadConfig("TamirRequestMonthDay", 0))

            If lTamirRequestMonthDay <= 0 Then Exit Sub 'And lIsTimeLimit Then Exit Sub

            Dim lDT_dc As DateTime, lDT_now As DateTime, lDT_month As DateTime, lDT_deadline As DateTime
            Dim lPDate_dc As String, lPDate_now As String, lPDate_month As String, lPDate_deadline As String, lPDate_monthnext
            Dim lDiff As Integer = -1

            Dim lNow As CTimeInfo = GetServerTimeInfo()
            Dim lKh As New KhayamDateTime

            lDT_dc = txtDateDisconnect.MiladiDT
            lDT_now = lNow.MiladiDate
            lDT_month = GetFirstDateOfMonth(lDT_now)

            lPDate_now = lNow.ShamsiDate
            lKh.SetMiladyDate(lDT_dc)
            lPDate_dc = lKh.GetShamsiDateStr()
            lKh.SetMiladyDate(lDT_month)
            lPDate_month = lKh.GetShamsiDateStr()
            lPDate_monthnext = KhayamDateTime.AddShamsiMonth(lPDate_month, 1)

            lDiff = DateDiff(DateInterval.Hour, lDT_now, lDT_dc)
            If lDiff < 48 And Not mIsFogheToziUser Then
                mEditingRow("TamirTypeId") = TamirTypes.Ezterari
                cmbTamirType.SelectedValue = TamirTypes.Ezterari
            ElseIf lTamirRequestMonthDay > 0 Then
                lPDate_deadline = Regex.Replace(lPDate_month, "\d\d$", lTamirRequestMonthDay.ToString("0#"))

                If lPDate_dc > lPDate_monthnext And lPDate_now <= lPDate_deadline Then
                    mEditingRow("TamirTypeId") = TamirTypes.BarnamehRiziShodeh
                    cmbTamirType.SelectedValue = TamirTypes.BarnamehRiziShodeh
                Else
                    mEditingRow("TamirTypeId") = TamirTypes.BaMovafeghat
                    cmbTamirType.SelectedValue = TamirTypes.BaMovafeghat
                End If
            End If

            cmbTamirNetworkType.Enabled = False
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub
    Private Sub ChangeTamirNetworkType()
        If Not (mIsForConfirm And Not mIsForWarmLineConfirm) And Not CheckUserTrustee("NewTamirRequest", 30, ApplicationTypes.TamirRequest) Then
            If cmbTamirNetworkType.SelectedValue = TamirNetworkType.FT AndAlso Not CheckUserTrustee("NewTamirFTRequest", 30, ApplicationTypes.TamirRequest) Then
                mNotTamirRequestNetworkType = "FT"
            ElseIf cmbTamirNetworkType.SelectedValue = TamirNetworkType.MP AndAlso Not CheckUserTrustee("NewTamirMPRequest", 30, ApplicationTypes.TamirRequest) Then
                mNotTamirRequestNetworkType = "MP"
            ElseIf cmbTamirNetworkType.SelectedValue = TamirNetworkType.LP AndAlso Not CheckUserTrustee("NewTamirLPRequest", 30, ApplicationTypes.TamirRequest) Then
                mNotTamirRequestNetworkType = "LP"
            Else
                mNotTamirRequestNetworkType = ""
            End If
        ElseIf chkIsSendToSetad.Checked Then
            ChangeChkIsSendToSetad()
        End If
    End Sub
    Private Sub ChangeChkIsSendToSetad()
        If chkIsSendToSetad.Checked Then
            'Dim lState As TamirRequestStates = mEditingRow("TamirRequestStateId")

            'Dim lIsAccessFeederPartRequest As Boolean = CheckUserTrustee("ConfirmFeederPart", 30)
            'Dim lIsAccessLPRequest As Boolean = CheckUserTrustee("ConfirmLPRequest", 30)

            'If IsCenter And Not IsSetadMode Then
            '    If lState = mdlHashemi.TamirRequestStates.trs_PreNew _
            '            Or lState = mdlHashemi.TamirRequestStates.trs_WaitFor121 Then

            '        If ((cmbFeederPart.SelectedIndex > -1 _
            '        Or cmbLPPost.SelectedIndex > -1) _
            '        And cmbTamirNetworkType.SelectedValue = TamirNetworkType.MP) Then
            '            If lIsAccessFeederPartRequest Then
            '                btnSave.Enabled = True
            '            Else
            '                btnSave.Enabled = False
            '            End If
            '        ElseIf cmbTamirNetworkType.SelectedValue = TamirNetworkType.LP Then
            '            If lIsAccessLPRequest Then
            '                btnSave.Enabled = True
            '            Else
            '                btnSave.Enabled = False
            '            End If
            '        ElseIf (cmbTamirNetworkType.SelectedValue = TamirNetworkType.MP Or cmbTamirNetworkType.SelectedValue = TamirNetworkType.FT) And CheckUserTrustee("TamirRequestConfirm", 30) Then
            '            btnSave.Enabled = True
            '        Else
            '            btnSave.Enabled = False
            '        End If
            '        btnSave.Enabled = btnSave.Enabled And CheckUserTrustee("TamirRequestConfirm", 30)
            '    End If
            'ElseIf Not IsCenter And Not IsSetadMode Then
            '    If lState = mdlHashemi.TamirRequestStates.trs_PreNew Then
            '        If ((cmbFeederPart.SelectedIndex > -1 _
            '        Or cmbLPPost.SelectedIndex > -1) _
            '        And cmbTamirNetworkType.SelectedValue = TamirNetworkType.MP) Then
            '            If lIsAccessFeederPartRequest Then
            '                btnSave.Enabled = True
            '            Else
            '                btnSave.Enabled = False
            '            End If
            '        ElseIf cmbTamirNetworkType.SelectedValue = TamirNetworkType.LP Then
            '            If lIsAccessLPRequest Then
            '                btnSave.Enabled = True
            '            Else
            '                btnSave.Enabled = False
            '            End If
            '        ElseIf (cmbTamirNetworkType.SelectedValue = TamirNetworkType.MP Or cmbTamirNetworkType.SelectedValue = TamirNetworkType.FT) And CheckUserTrustee("TamirRequestConfirm", 30) Then
            '            btnSave.Enabled = True
            '        Else
            '            btnSave.Enabled = False
            '        End If
            '        btnSave.Enabled = btnSave.Enabled And CheckUserTrustee("TamirRequestConfirm", 30)
            '    End If
            'End If
            'If (Not btnSave.Enabled And mIsEmergencyEnabled) _
            '    Or (btnSave.Enabled And mTamirRequestId = -1) Then
            '    If cmbTamirNetworkType.SelectedValue = TamirNetworkType.FT Then
            '        btnSave.Enabled = CheckUserTrustee("NewTamirFTRequest", 30)
            '    ElseIf cmbTamirNetworkType.SelectedValue = TamirNetworkType.MP Then
            '        btnSave.Enabled = CheckUserTrustee("NewTamirMPRequest", 30)
            '    ElseIf cmbTamirNetworkType.SelectedValue = TamirNetworkType.LP Then
            '        btnSave.Enabled = CheckUserTrustee("NewTamirLPRequest", 30)
            '    End If
            'End If
        Else
            ChangeTamirNetworkType()
        End If
    End Sub

    Private Sub LoadPowerInfoMPManovr(Optional ByVal aIsUpdateCurrentValue As Boolean = False)

        Dim lSQL As String
        Dim lMPFeederId As Integer = -1
        Dim lRow As DataRow
        Dim lYearMonth As String = ""
        Dim lInterval As Integer = 0
        Dim P As Double = 0.0, lCurrentValue As Double = 0.0
        Dim lPower As Double = 0.0, lCosinPhi As Double = 0.85
        Dim lVoltage As Integer = 20000

        Try
            lCurrentValue = Val(txtCurrentValueManovr.Text)
            mCosinPhi = 0.85
            mVoltage = 20000

            If cmbMPFeeder.SelectedIndex > -1 Then
                lMPFeederId = cmbMPFeeder.SelectedValue
            End If

            lSQL = "SELECT * FROM Tbl_MPFeeder WHERE MPFeederId = " & lMPFeederId
            BindingTable(lSQL, mCnn, mDsReq, "Tbl_MPFeeder", , , , , , , True)

            If mDsReq.Tbl_MPFeeder.Rows.Count > 0 Then
                lRow = mDsReq.Tbl_MPFeeder.Rows(0)
                lCosinPhi = IIf(IsDBNull(lRow("CosinPhi")), 0.85, lRow("CosinPhi"))
                lVoltage = IIf(IsDBNull(lRow("Voltage")), 20000, lRow("Voltage"))
            End If

            lInterval = Val(txtDCIntervalManovr.Text.Trim)
            lblPeakBar.Text = "آخرين بار پيک فيدر فشار متوسط"

            If aIsUpdateCurrentValue Then
                txtCurrentValueManovr.Text = lCurrentValue
            End If

            Try
                P = lCurrentValue * (lInterval / 60)
                Dim lPowerFull As Double = (Math.Sqrt(3) * mCosinPhi * mVoltage * P) / 1000000
                lPower = lPowerFull
            Catch ex1 As Exception
            End Try

            txtPowerManovr.Text = Math.Round(lPower, 2)

        Catch ex As Exception
            WriteError(ex.ToString)
        End Try

    End Sub
    Private Sub LoadPowerInfoLPManovr(Optional ByVal aIsUpdateCurrentValue As Boolean = False)
        If mIsLoadingPower Then Exit Sub

        Dim lSQL As String
        Dim lRow As DataRow
        Dim lYearMonth As String = ""
        Dim lInterval As Integer = 0
        Dim P As Double = 0.0, lCurrentValue As Double = 0.0
        Dim A As Double = 1
        Dim lIsLPPost As Boolean = IIf(cmbLPFeeder.SelectedIndex > -1, False, True)
        Dim lTableName As String = IIf(lIsLPPost, "Tbl_LPPost", "Tbl_LPFeeder")
        Dim lPeakName As String = IIf(lIsLPPost, "PostPeakCurrent", "FeederPeakCurrent")
        Dim lId As Integer = -1
        Dim lPower As Double = 0.0, lCosinPhi As Double = 0.85
        Dim lVoltage As Integer = 400

        Try
            mIsLoadingPower = True
            txtDateDisconnectManovr.InsertTime(txtTimeDisconnectManovr)
            If IsNothing(txtDateDisconnectManovr.MiladiDT) OrElse IsDBNull(txtDateDisconnectManovr.MiladiDT) Then
                Exit Sub
            End If

            A = CalculateA4LoadDiagrams(txtDateDisconnectManovr.MiladiDT)

            lCurrentValue = Val(txtCurrentValueManovr.Text)
            lCosinPhi = 0.85
            lVoltage = 400

            If lIsLPPost Then
                lId = cmbLPPost.SelectedValue
            Else
                lId = cmbLPFeeder.SelectedValue
            End If

            lSQL =
                " SELECT TOP 1 " &
                "	ISNULL(TblLPFeederLoad.FeederPeakCurrent, Tbl_LPFeeder.FeederPeakCurrent) AS FeederPeakCurrent, " &
                "   " & IIf(lIsLPPost, "0.85", "ISNULL(TblLPFeederLoad.CosinPhi, Tbl_LPFeeder.CosinPhi)") & " AS CosinPhi " &
                " FROM " &
                "	TblLPFeederLoad " &
                "	INNER JOIN Tbl_LPFeeder ON TblLPFeederLoad.LPFeederId = Tbl_LPFeeder.LPFeederId " &
                " WHERE " &
                "	TblLPFeederLoad.LPFeederId = " & lId &
                " ORDER BY " &
                "	TblLPFeederLoad.LoadDT DESC "
            If lIsLPPost Then lSQL = lSQL.Replace("Feeder", "Post")
            RemoveMoreSpaces(lSQL)
            BindingTable(lSQL, mCnn, mDsReq, lTableName, , , , , , , True)

            Dim lTblLoadInfo As DataTable = mDsReq.Tables(lTableName)

            If lTblLoadInfo.Rows.Count > 0 Then
                lRow = lTblLoadInfo.Rows(0)
                lCosinPhi = IIf(IsDBNull(lRow("CosinPhi")), 0.85, lRow("CosinPhi"))
            End If

            lInterval = Val(txtDCIntervalManovr.Text.Trim)

            If aIsUpdateCurrentValue Then
                txtCurrentValueManovr.Text = lCurrentValue
            End If

            Try
                P = mCurrentValue * (lInterval / 60)
                Dim lPowerFull As Double = (Math.Sqrt(3) * mCosinPhi * mVoltage * P * A) / 1000000
                lPower = lPowerFull
            Catch ex1 As Exception
            End Try

            txtPowerManovr.Text = Math.Round(lPower, 4) * 1000

        Catch ex As Exception
            WriteError(ex.ToString)
        Finally
            mIsLoadingPower = False
        End Try
    End Sub
    Private Sub ComputeDCIntervalManovr()
        If Not txtDateDisconnectManovr.IsOK Or Not txtDateConnectManovr.IsOK Or Not txtTimeDisconnectManovr.IsOK Or Not txtTimeConnectManovr.IsOK Then
            txtDCIntervalManovr.Text = ""
            Exit Sub
        End If

        Dim lDT1 As DateTime, lDT2 As DateTime
        Dim lT1 As DateTime, lT2 As DateTime
        Dim lH As Integer, lM As Integer

        lDT1 = txtDateDisconnectManovr.MiladiDT
        lDT2 = txtDateConnectManovr.MiladiDT

        lT1 = txtTimeDisconnectManovr.GetFullDate
        lT2 = txtTimeConnectManovr.GetFullDate

        lDT1 = lDT1.AddHours(-lDT1.Hour).AddHours(lT1.Hour)
        lDT1 = lDT1.AddMinutes(-lDT1.Minute).AddMinutes(lT1.Minute)

        lDT2 = lDT2.AddHours(-lDT2.Hour).AddHours(lT2.Hour)
        lDT2 = lDT2.AddMinutes(-lDT2.Minute).AddMinutes(lT2.Minute)

        txtDCIntervalManovr.Text = DateDiff(DateInterval.Minute, lDT1, lDT2)
    End Sub
    Private Function MakeDisconnectManovrId(ByVal aRelatedRequestId As Long, ByVal aTrans As SqlTransaction) As Long
        MakeDisconnectManovrId = -1
        If Not txtDateDisconnectManovr.IsOK OrElse Not txtTimeDisconnectManovr.IsOK _
            OrElse Not txtDateConnectManovr.IsOK OrElse Not txtTimeConnectManovr.IsOK Then
            Exit Function
        End If

        Dim lNow As CTimeInfo = GetServerTimeInfo()

        Dim lIsSaveOK As Boolean = False
        Dim lRequestId As Long = -1

        Dim lIsNullUser As Boolean = True
        Dim lIsRequestUser As Boolean = True

        Try
            lIsNullUser = CConfig.ReadConfig("IsTamirNullUser", True)
        Catch ex As Exception
        End Try

        If lIsNullUser Then
            Try
                lIsRequestUser = CConfig.ReadConfig("IsTamirReqUser", True)
            Catch ex As Exception
            End Try
        End If

        If Not mDsReq.TblRequestInfo.Columns.Contains("IsNotNeedParts") Then
            mDsReq.TblRequestInfo.Columns.Add("IsNotNeedParts")
        End If

        If Not mDsReq.TblRequestInfo.Columns.Contains("NotNeedPartsReason") Then
            mDsReq.TblRequestInfo.Columns.Add("NotNeedPartsReason")
        End If

        If Not mDsReq.TblRequestInfo.Columns.Contains("IsEnterPartsLater") Then
            mDsReq.TblRequestInfo.Columns.Add("IsEnterPartsLater")
        End If

        Dim lUserId As Object = IIf(lIsNullUser, IIf(lIsRequestUser, mEditingRow("AreaUserId"), DBNull.Value), WorkingUserId)

        Dim lTbl As DatasetCcRequester.TblRequestDataTable = mDsReq.TblRequest
        Dim lRow As DatasetCcRequester.TblRequestRow = lTbl.NewTblRequestRow

        Dim lTblInfo As DatasetCcRequester.TblRequestInfoDataTable = mDsReq.TblRequestInfo
        Dim lRowInfo As DatasetCcRequester.TblRequestInfoRow = lTblInfo.NewTblRequestInfoRow

        Dim lTblPF As DatasetCcRequester.TblRequestPostFeederDataTable = mDsReq.TblRequestPostFeeder
        Dim lRowPF As DatasetCcRequester.TblRequestPostFeederRow = Nothing

        '-------------------
        'TblRequest:

        lRow("RequestId") = GetAutoInc()
        'lRow("CallId") = ""
        'lRow("LPRequestId") = ""
        'lRow("MPRequestId") = ""
        'lRow("SecretaryId") = ""
        lRow("AreaUserId") = lUserId 'WorkingUserId 'mEditingRow("AreaUserId")
        'lRow("DataEntrancePersonName") = ""
        lRow("DisconnectDT") = mEditingRowManovr("DisconnectDT")
        lRow("DisconnectDatePersian") = mEditingRowManovr("DisconnectDatePersian")
        lRow("DisconnectTime") = mEditingRowManovr("DisconnectTime")
        'lRow("SubscriberId") = ""
        Dim lPeymankar As String = ""
        If Not IsDBNull(mEditingRow("Peymankar")) Then
            lPeymankar = mEditingRow("Peymankar")
        End If
        If lPeymankar.Length > 50 Then
            lPeymankar = lPeymankar.Substring(0, 49)
        End If
        lRow("SubscriberName") = lPeymankar
        lRow("CityId") = mEditingRow("CityId")
        Dim lAddress As String = IIf(IsDBNull(mEditingRow("WorkingAddress")), mEditingRow("CriticalsAddress"), mEditingRow("WorkingAddress"))
        If lAddress.Length > 200 Then
            lAddress = lAddress.Substring(0, 199)
        End If
        lRow("Address") = lAddress   '"پست فوق توزيع " & cmbMPPost.Text & ", فيدر فشار متوسط " & cmbMPFeeder.Text
        'lRow("Telephone") = ""
        'lRow("PostalCodeId") = ""
        lRow("AreaId") = mEditingRow("AreaId")
        lRow("WeatherId") = mEditingRow("WeatherId")
        'lRow("MainDisconnectGroupSetId") = ""
        'lRow("DisconnectGroupSetId") = ""
        'lRow("Priority") = ""
        'lRow("IsSingleSubscriber") = ""
        'lRow("IsOnePhaseSingleSubscriber") = ""
        'lRow("DisconnectInterval") = mEditingRow("DisconnectInterval")
        'lRow("DisconnectPower") = mEditingRow("DisconnectPower")
        lRow("DataEntryDT") = lNow.MiladiDate
        lRow("DataEntryDTPersian") = lNow.ShamsiDate
        lRow("DataEntryTime") = lNow.HourMin
        lRow("IsDuplicatedRequest") = False
        lRow("EndJobStateId") = EndJobStates.ejs_New
        lRow("ReferToId") = mEditingRow("ReferToId")
        'lRow("Comments") = ""
        'lRow("IsSentToRelatedArea") = ""
        'lRow("GPSx") = ""
        'lRow("GPSy") = ""
        lRow("IsManoeuvred") = True
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
        lRow("TamirDisconnectFromDT") = mEditingRowManovr("DisconnectDT")
        lRow("TamirDisconnectFromDatePersian") = mEditingRowManovr("DisconnectDatePersian")
        lRow("TamirDisconnectFromTime") = mEditingRowManovr("DisconnectTime")
        lRow("TamirDisconnectToDT") = mEditingRowManovr("ConnectDT")
        lRow("TamirDisconnectToDatePersian") = mEditingRowManovr("ConnectDatePersian")
        lRow("TamirDisconnectToTime") = mEditingRowManovr("ConnectTime")
        'lRow("RequestDEInterval") = ""
        'lRow("CallTypeId") = ""
        'lRow("CallReasonId") = ""
        'lRow("TamirRequestTypeId") = ""
        lRow("DepartmentId") = mEditingRow("DepartmentId")
        'lRow("DisconnectRequestForId") = ""
        lRow("IsLightRequest") = False
        lRow("IsLPRequest") = False
        lRow("IsMPRequest") = False
        lRow("IsTamir") = True
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
        lRow("TamirTypeId") = cmbTamirType.SelectedValue
        lRow("RelatedRequestId") = aRelatedRequestId

        'lRow("spcRSTId") = ""
        'lRow("DisconnectPowerECO") = ""

        lTbl.AddTblRequestRow(lRow)

        '-------------------
        'TblRequestInfo:

        lRowInfo("RequestId") = lRow("RequestId")
        'lRowInfo("CallTime") = DBNull.Value
        'lRowInfo("IsRealNotRelated") = DBNull.Value
        'lRowInfo("OperatorConvsType") = DBNull.Value
        'lRowInfo("IsSaveParts") = DBNull.Value
        'lRowInfo("IsInformTamir") = DBNull.Value
        'lRowInfo("IsSaveLight") = DBNull.Value
        'lRowInfo("SubscriberOKType") = DBNull.Value
        lRowInfo("IsWatched") = False
        'lRowInfo("SendTimeInterval") = DBNull.Value
        'lRowInfo("IsRealDuplicated") = DBNull.Value
        Dim lIsForceEnvironmentTypeId As Boolean = CConfig.ReadConfig("IsForceEnvironmentTypeId", "True")
        If lIsForceEnvironmentTypeId Then
            lRowInfo("EnvironmentTypeId") = IIf(mEditingRow("IsInCityService"), 1, 2)
        End If
        lRowInfo("SubscriberCode") = DBNull.Value
        lRowInfo("SendTimeInterval") = -1

        lRowInfo("IsNotNeedParts") = False
        'lRowInfo("NotNeedPartsReason") = False
        lRowInfo("IsEnterPartsLater") = False

        If chkIsSendToInformApp.Visible Then
            lRowInfo("IsSendToInformApp") = chkIsSendToInformApp.Checked
        End If

        lTblInfo.AddTblRequestInfoRow(lRowInfo)

        '-------------------
        'TblRequestPostFeeder:

        If cmbTamirNetworkType.SelectedValue <> TamirNetworkType.FT Then

            lRowPF = lTblPF.NewTblRequestPostFeederRow

            lRowPF("RequestPostFeederId") = GetAutoInc()
            lRowPF("RequestId") = lRow("RequestId")
            SetDBValue(cmbMPPost, lRowPF("MPPostId"))
            SetDBValue(cmbMPFeeder, lRowPF("MPFeederId"))
            If cmbTamirNetworkType.SelectedValue = TamirNetworkType.FT AndAlso cmbMPFeeder.SelectedIndex = -1 Then
                lRowPF("LocationTypeId") = LocationTypes.lt_MPPost
            ElseIf chkIsLPPostFeederPart.Checked And rbLPPost.Checked Then
                SetDBValue(cmbLPPost, lRowPF("LPPostId"))
                lRowPF("LocationTypeId") = LocationTypes.lt_LPPost
            ElseIf chkIsLPPostFeederPart.Checked And rbFeederPart.Checked Then
                SetDBValue(cmbFeederPart, lRowPF("FeederPartId"))
                lRowPF("LocationTypeId") = LocationTypes.lt_FeederPart
            ElseIf cmbTamirNetworkType.SelectedValue = TamirNetworkType.LP Then
                SetDBValue(cmbLPPost, lRowPF("LPPostId"))
                SetDBValue(cmbLPFeeder, lRowPF("LPFeederId"))
                lRowPF("LocationTypeId") = LocationTypes.lt_LPFeeder
            Else
                lRowPF("LocationTypeId") = LocationTypes.lt_MPFeeder
            End If

            lTblPF.AddTblRequestPostFeederRow(lRowPF)

        Else

            For Each lFTRow As DataRow In mDs.TblTamirRequestFTFeeder.Rows
                lRowPF = lTblPF.NewTblRequestPostFeederRow

                lRowPF("RequestPostFeederId") = GetAutoInc()
                lRowPF("RequestId") = lRow("RequestId")
                lRowPF("LocationTypeId") = LocationTypes.lt_MPFeeder
                SetDBValue(cmbMPPost, lRowPF("MPPostId"))
                lRowPF("MPFeederId") = lFTRow("MPFeederId")

                lTblPF.AddTblRequestPostFeederRow(lRowPF)
            Next

        End If

        '-------------------


        Dim lUpdate As New frmUpdateDataset
        lIsSaveOK = lUpdate.UpdateDataSet("TblRequest", mDsReq, lRow("AreaId"), 1, , aTrans)

        If lIsSaveOK Then
            lRowInfo("RequestId") = lRow("RequestId")
            lIsSaveOK = lUpdate.UpdateDataSet("TblRequestInfo", mDsReq, lRow("AreaId"), 1, False, aTrans)
        End If

        If lIsSaveOK Then
            For Each lRowPF In lTblPF.Rows
                If lRowPF.RowState <> DataRowState.Unchanged Then
                    lRowPF("RequestId") = lRow("RequestId")
                End If
            Next
            lIsSaveOK = lUpdate.UpdateDataSet("TblRequestPostFeeder", mDsReq, lRow("AreaId"), 1, , aTrans)
        End If

        If lIsSaveOK Then lRequestId = lRow("RequestId")
        MakeDisconnectManovrId = lRequestId

        mManovrRequestNumber = lRow("RequestNumber")

    End Function

    Private Function LoadSubscribers(ByVal aRequestId As String, ByVal aLocationType As LocationTypes, ByVal aTrans As SqlTransaction) As ArrayList
        Dim lSQL As String = "", lSubPostSQL As String = ""
        Dim lFieldName As String
        Dim lDs As New DataSet
        Dim lSubscribers As New ArrayList

        If aLocationType = CommonVariables.LocationTypes.lt_MPPost Then
            lFieldName = "MPPostId"
        ElseIf aLocationType = CommonVariables.LocationTypes.lt_MPFeeder Then
            lFieldName = "MPFeederId"
        ElseIf aLocationType = CommonVariables.LocationTypes.lt_LPPost Then
            lFieldName = "LPPostId"
        ElseIf aLocationType = CommonVariables.LocationTypes.lt_FeederPart Then
            lFieldName = "FeederPartId"
        ElseIf aLocationType = CommonVariables.LocationTypes.lt_LPFeeder Then
            lFieldName = "LPFeederId"
        End If

        If aLocationType = CommonVariables.LocationTypes.lt_FeederPart Then
            lSubPostSQL = "SELECT LPPostId FROM Tbl_LPPost WHERE FeederPartId = " & mDsReq.TblRequestPostFeeder(0)("FeederPartId") & ""
        End If

        lSQL =
            " SELECT SubscriberId " &
            " FROM Tbl_Subscriber " &
            " WHERE (" & lFieldName & " = " & mDsReq.TblRequestPostFeeder(0)(lFieldName) & " " &
            IIf(lSubPostSQL = "", "", " OR LPPostId IN (" & lSubPostSQL & ") ") & ") " &
            " AND SubscriberSensitivityId = 3 AND  ISNULL(TelMobile,'') <> '' "
        BindingTable(lSQL, mCnn, lDs, "Tbl_Subscriber", , , , , aTrans)
        If Not lDs.Tables("Tbl_Subscriber") Is Nothing Then
            For Each lSubRow As DataRow In lDs.Tables("Tbl_Subscriber").Rows
                lSubscribers.Add(lSubRow("SubscriberId"))
            Next
        End If

        Return lSubscribers
    End Function

    Private Sub AddColumnTblMPRequestKey()
        If Not DatasetTemp1.TblTamirRequestKey.Columns.Contains("KeyName") Then
            DatasetTemp1.TblTamirRequestKey.Columns.Add("KeyName")
        End If
        If Not DatasetTemp1.TblTamirRequestKey.Columns.Contains("KeyCode") Then
            DatasetTemp1.TblTamirRequestKey.Columns.Add("KeyCode")
        End If
        If Not DatasetTemp1.TblTamirRequestKey.Columns.Contains("GISCode") Then
            DatasetTemp1.TblTamirRequestKey.Columns.Add("GISCode")
        End If
        If Not DatasetTemp1.TblTamirRequestKey.Columns.Contains("MPFeederName") Then
            DatasetTemp1.TblTamirRequestKey.Columns.Add("MPFeederName")
        End If
        If Not DatasetTemp1.TblTamirRequestKey.Columns.Contains("MPFeederId") Then
            DatasetTemp1.TblTamirRequestKey.Columns.Add("MPFeederId")
        End If
        If Not DatasetTemp1.TblTamirRequestKey.Columns.Contains("MPFeederManovre") Then
            DatasetTemp1.TblTamirRequestKey.Columns.Add("MPFeederManovre")
        End If
        If Not DatasetTemp1.TblTamirRequestKey.Columns.Contains("MPCloserTypeId") Then
            DatasetTemp1.TblTamirRequestKey.Columns.Add("MPCloserTypeId")
        End If
        If Not DatasetTemp1.TblTamirRequestKey.Columns.Contains("MPFeederId") Then
            DatasetTemp1.TblTamirRequestKey.Columns.Add("MPFeederId")
        End If
        If Not DatasetTemp1.TblTamirRequestKey.Columns.Contains("KeyComments") Then
            DatasetTemp1.TblTamirRequestKey.Columns.Add("KeyComments")
        End If
        If Not DatasetTemp1.TblTamirRequestKey.Columns.Contains("IsManovre") Then
            DatasetTemp1.TblTamirRequestKey.Columns.Add("IsManovre", Type.GetType("System.Boolean"))
        End If
        If Not DatasetTemp1.TblTamirRequestKey.Columns.Contains("IsOpen") Then
            DatasetTemp1.TblTamirRequestKey.Columns.Add("IsOpen", Type.GetType("System.Boolean"))
        End If
        If Not DatasetTemp1.TblTamirRequestKey.Columns.Contains("IsClose") Then
            DatasetTemp1.TblTamirRequestKey.Columns.Add("IsClose", Type.GetType("System.Boolean"))
        End If
        If Not DatasetTemp1.TblTamirRequestKey.Columns.Contains("ManovrMPFeederId") Then
            DatasetTemp1.TblTamirRequestKey.Columns.Add("ManovrMPFeederId")
        End If
    End Sub
    Private Function CreateMPFeederKeyWhere(Optional ByVal aIsFinal As Boolean = False, Optional ByVal aTamirRequestMultiStepId As Long = -1) As String
        CreateMPFeederKeyWhere = IIf(aTamirRequestMultiStepId > -1, " TamirRequestMultiStepId = " & aTamirRequestMultiStepId, " TamirRequestMultiStepId IS NULL") & " AND (MPFeederId = " & cmbMPFeeder.SelectedValue & " OR ManovrMPFeederId = " & cmbMPFeeder.SelectedValue & ")"
        Dim lMPCloserTypeWhere As String = ""
        Dim lMPFeederKeyNameWhere As String = ""
        Dim lMPFeederKeyGISWhere As String = ""
        Dim lIsFinalWhere As String = ""
        If cmbMPCloserType.SelectedIndex > -1 Then
            lMPCloserTypeWhere = " AND MPCloserTypeId = " & cmbMPCloserType.SelectedValue
        End If
        If txtMPFeederKey.Text <> "" Then
            lMPFeederKeyNameWhere = " AND " & MergeFarsiAndArabi("KeyName", txtMPFeederKey.Text.Replace("*", "[*]").Replace("%", "[%]"), , "")
        End If
        If txtMPFeederKeyGIS.Text <> "" Then
            lMPFeederKeyGISWhere = " AND " & MergeFarsiAndArabi("GISCode", txtMPFeederKeyGIS.Text.Replace("*", "[*]").Replace("%", "[%]"), , "")
        End If
        If aIsFinal Then
            lIsFinalWhere = " AND ISNULL(IsFinalKeyState,0) = 1 "
        Else
            lIsFinalWhere = " AND ISNULL(IsFinalKeyState,0) = 0 "
        End If
        CreateMPFeederKeyWhere &= lMPCloserTypeWhere
        CreateMPFeederKeyWhere &= lMPFeederKeyNameWhere
        CreateMPFeederKeyWhere &= lMPFeederKeyGISWhere
        CreateMPFeederKeyWhere &= lIsFinalWhere
        Return CreateMPFeederKeyWhere
    End Function
    Private Sub FillTempTblMPRequestKey(Optional ByVal aIsFinal As Boolean = False, Optional ByVal aTamirRequestMultiStepId As Long = -1)

        Try
            If cmbMPFeeder.SelectedIndex = -1 Then
                ShowError("لطفا فيدر فشار متوسط را مشخص نماييد")
                cmbMPFeeder.Focus()
                Exit Sub
            End If

            mControlEnableState.Clear()
            For Each lCtrl As Control In Me.Controls
                mControlEnableState.Add(lCtrl.Name, lCtrl.Enabled)
                If lCtrl.Name <> "pnlMPFeederKey" Then
                    lCtrl.Enabled = False
                End If
            Next

            pnlMPFeederKey.BringToFront()
            pnlMPFeederKey.Visible = True

            cmbMPCloserType.SelectedIndex = -1
            cmbMPCloserType.SelectedIndex = -1

        Catch ex As Exception
            ShowError(ex)
        End Try

        Try

            If mTblTemp_FeederKey Is Nothing Then

                If Not DatasetTemp1 Is Nothing Then
                    AddColumnTblMPRequestKey()
                End If
                Dim lSQL As String

                Dim lRows() As DataRow
                Dim lMPRequestKey_Rows() As DataRow
                Dim lNewRow As DataRow
                Dim lIsNew As Boolean = False

                lSQL =
                    "SELECT  " &
                    "	ISNULL(TblTamirRequestKey.TamirRequestKeyId,-1) AS TamirRequestKeyId, " &
                    "	TblTamirRequestKey.TamirRequestId, " &
                    "	Tbl_MPFeederKey.MPFeederId, " &
                    "	CAST (ISNULL(TblTamirRequestKey.IsRemoteChange,0) AS bit) AS IsRemoteChange, " &
                    "	Tbl_MPFeederKey.MPFeederKeyId, " &
                    "	ISNULL(TblTamirRequestKey.KeyStateId,0) AS KeyStateId, " &
                    "	Tbl_MPFeederKey.Comments AS KeyComments, " &
                    "	TblTamirRequestKey.TamirRequestMultiStepId, " &
                    "	Tbl_MPFeederKey.KeyName, " &
                    "	Tbl_MPFeederKey.GISCode, " &
                    "	Tbl_MPFeederKey.KeyCode, " &
                    "	CASE WHEN Tbl_MPFeederKey.ManovrMPFeederId IS NULL THEN CAST(0 as bit) ELSE CAST(1 as bit) END AS IsManovre, " &
                    "	CASE WHEN TblTamirRequestKey.KeyStateId = 1 THEN CAST(1 as bit) ELSE CAST(0 as bit) END AS IsOpen, " &
                    "	CASE WHEN TblTamirRequestKey.KeyStateId = 2 THEN CAST(1 as bit) ELSE CAST(0 as bit) END AS IsClose, " &
                    "   Tbl_MPFeeder.MPFeederName, " &
                    "   Tbl_MPFeeder.MPFeederId, " &
                    "   Tbl_MPFeederManovre.MPFeederName AS MPFeederManovre, " &
                    "   Tbl_MPFeederKey.MPCloserTypeId, " &
                    "   Tbl_MPFeederKey.ManovrMPFeederId, " &
                    "	CAST (ISNULL(TblTamirRequestKey.IsFinalKeyState,0) AS bit) AS IsFinalKeyState " &
                    "FROM  " &
                    "	Tbl_MPFeederKey " &
                    "	LEFT JOIN TblTamirRequestKey ON Tbl_MPFeederKey.MPFeederKeyId = TblTamirRequestKey.MPFeederKeyId " &
                    "       AND TblTamirRequestKey.TamirRequestId = " & mTamirRequestId &
                    IIf(aTamirRequestMultiStepId > -1, " AND TblTamirRequestKey.TamirRequestMultiStepId = " & aTamirRequestMultiStepId, " AND TblTamirRequestKey.TamirRequestMultiStepId IS NULL ") &
                    IIf(aIsFinal, " AND ISNULL(TblTamirRequestKey.IsFinalKeyState,0) = 1 ", " AND ISNULL(TblTamirRequestKey.IsFinalKeyState,0) = 0 ") &
                    "   LEFT JOIN Tbl_MPFeeder ON Tbl_MPFeederKey.MPFeederId = Tbl_MPFeeder.MPFeederId " &
                    "   LEFT JOIN Tbl_MPFeeder Tbl_MPFeederManovre ON Tbl_MPFeederKey.ManovrMPFeederId = Tbl_MPFeederManovre.MPFeederId " &
                    "WHERE (Tbl_MPFeederKey.MPFeederId = " & cmbMPFeeder.SelectedValue &
                    "       OR Tbl_MPFeederKey.ManovrMPFeederId = " & cmbMPFeeder.SelectedValue & ") AND Tbl_MPFeederKey.IsActive = 1 "

                BindingTable(lSQL, mCnn, DatasetCcRequester1, "Tbl_MPFeederKey", , , , , , , True)
                For Each lRow As DataRow In DatasetCcRequester1.Tables("Tbl_MPFeederKey").Rows
                    lRows = DatasetTemp1.TblTamirRequestKey.Select("MPFeederKeyId = " & lRow("MPFeederKeyId") & " AND IsFinalKeyState = " & aIsFinal &
                                            IIf(aTamirRequestMultiStepId > -1, " AND TamirRequestMultiStepId = " & aTamirRequestMultiStepId, " AND TamirRequestMultiStepId IS NULL"))
                    If lRows.Length > 0 Then
                        lNewRow = lRows(0)

                        If lRows(0)("KeyStateId") = 0 Then
                            lNewRow("IsOpen") = False
                            lNewRow("IsClose") = False
                        ElseIf lRows(0)("KeyStateId") = 1 Then
                            lNewRow("IsOpen") = True
                            lNewRow("IsClose") = False
                        ElseIf lRows(0)("KeyStateId") = 2 Then
                            lNewRow("IsOpen") = False
                            lNewRow("IsClose") = True
                        End If
                        lNewRow("IsRemoteChange") = lRows(0)("IsRemoteChange")
                        lNewRow("IsFinalKeyState") = lRows(0)("IsFinalKeyState")
                        lIsNew = False
                    Else
                        lNewRow = DatasetTemp1.TblTamirRequestKey.NewRow()
                        lNewRow("TamirRequestKeyId") = GetAutoInc()
                        lNewRow("KeyStateId") = 0
                        lNewRow("IsOpen") = False
                        lNewRow("IsClose") = False
                        lNewRow("TamirRequestMultiStepId") = IIf(aTamirRequestMultiStepId > -1, aTamirRequestMultiStepId, DBNull.Value)
                        lNewRow("IsRemoteChange") = False
                        lNewRow("IsFinalKeyState") = aIsFinal
                        lIsNew = True

                    End If

                    lNewRow("TamirRequestId") = mTamirRequestId
                    lNewRow("MPFeederKeyId") = lRow("MPFeederKeyId")
                    lNewRow("KeyName") = lRow("KeyName")
                    lNewRow("GISCode") = lRow("GISCode")
                    lNewRow("KeyCode") = lRow("KeyCode")
                    lNewRow("KeyComments") = lRow("KeyComments")
                    lNewRow("IsManovre") = lRow("IsManovre")
                    lNewRow("MPFeederName") = lRow("MPFeederName")
                    lNewRow("MPFeederId") = lRow("MPFeederId")
                    lNewRow("MPFeederManovre") = lRow("MPFeederManovre")
                    lNewRow("MPCloserTypeId") = lRow("MPCloserTypeId")
                    lNewRow("ManovrMPFeederId") = lRow("ManovrMPFeederId")

                    If lIsNew Then
                        DatasetTemp1.TblTamirRequestKey.Rows.Add(lNewRow)
                    End If

                Next

                For Each lRow As DataRow In DatasetTemp1.TblTamirRequestKey.Rows
                    If lRow.RowState <> DataRowState.Deleted Then
                        If lRow("KeyStateId") = 0 Then
                            lRow("IsOpen") = False
                            lRow("IsClose") = False
                        ElseIf lRow("KeyStateId") = 1 Then
                            lRow("IsOpen") = True
                            lRow("IsClose") = False
                        ElseIf lRow("KeyStateId") = 2 Then
                            lRow("IsOpen") = False
                            lRow("IsClose") = True
                        End If
                    End If
                Next

                mTblTemp_FeederKey = DatasetTemp1.TblTamirRequestKey.Copy()
                mTblBase_FeederKey = DatasetTemp1.TblTamirRequestKey.Copy()
            End If

            Dim lMPCloserTypeValue As Integer = -1
            Dim lMPCloserTypeId As Integer = cmbMPCloserType.SelectedIndex

            Try
                cmbMPCloserType.SelectedIndex = lMPCloserTypeId
                cmbMPCloserType.SelectedIndex = lMPCloserTypeId
                If lMPCloserTypeId = -1 And lMPCloserTypeValue > -1 Then
                    cmbMPCloserType.SelectedValue = lMPCloserTypeValue
                    cmbMPCloserType.SelectedValue = lMPCloserTypeValue
                End If
            Catch ex As Exception
                cmbMPCloserType.SelectedIndex = lMPCloserTypeId
                If lMPCloserTypeId = -1 And lMPCloserTypeValue > -1 Then
                    cmbMPCloserType.SelectedValue = lMPCloserTypeValue
                    cmbMPCloserType.SelectedValue = lMPCloserTypeValue
                End If
            End Try


            mTblTemp_FeederKey.DefaultView.RowFilter = CreateMPFeederKeyWhere(aIsFinal, aTamirRequestMultiStepId)
            dgFeederKey.DataSource = mTblTemp_FeederKey.DefaultView

            If aTamirRequestMultiStepId = -1 Then
                If aIsFinal Then
                    dgFeederKey.RootTable.Columns("IsOpen").EditType = Janus.Windows.GridEX.EditType.NoEdit
                    dgFeederKey.RootTable.Columns("IsClose").EditType = Janus.Windows.GridEX.EditType.CheckBox
                Else
                    dgFeederKey.RootTable.Columns("IsOpen").EditType = Janus.Windows.GridEX.EditType.CheckBox
                    dgFeederKey.RootTable.Columns("IsClose").EditType = Janus.Windows.GridEX.EditType.NoEdit
                End If
            Else
                dgFeederKey.RootTable.Columns("IsOpen").EditType = Janus.Windows.GridEX.EditType.CheckBox
                dgFeederKey.RootTable.Columns("IsClose").EditType = Janus.Windows.GridEX.EditType.CheckBox
            End If

            If mTblTemp_FeederKey.DefaultView.Count > 0 Then
                chkIsRemoteChange.Checked = mTblTemp_FeederKey.DefaultView(0)("IsRemoteChange")
            End If

        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub
    Private Sub btnFeederKeyVisibleState(Optional ByVal aVisible As Boolean = False)
        btnFeederKey0.Visible = aVisible
        btnFeederKey1.Visible = aVisible
        btnFeederKey2.Visible = aVisible
        btnFeederKey3.Visible = aVisible
        btnFeederKey4.Visible = aVisible
        btnFeederKey5.Visible = aVisible
        btnFeederKey6.Visible = aVisible
    End Sub
    Private Sub SearchPeymankar()
        Dim lWhere As String = ""
        If txtPeymankarSearch.Text <> "" Then
            lWhere = " AND " & MergeFarsiAndArabi("PeymankarName", txtPeymankarSearch.Text.Replace("*", "[*]").Replace("%", "[%]"), , "")
        End If
        If cmbArea.SelectedIndex > -1 Then
            lWhere &= " AND (AreaId = " & cmbArea.SelectedValue & " OR AreaId IS NULL) "
        End If
        If lWhere <> "" Then
            lWhere = Regex.Replace(lWhere, "^ AND", "")
        End If
        mDs.Tbl_Peymankar.DefaultView.RowFilter = lWhere
    End Sub
    Private Sub SearchFeederPart()
        Dim lWhere As String = ""
        If txtFeederPart.Text <> "" Then
            lWhere = " AND " & MergeFarsiAndArabi("FeederPart", txtFeederPart.Text.Replace("*", "[*]").Replace("%", "[%]"), , "")
        End If
        If cmbArea.SelectedIndex > -1 Then
            lWhere &= " AND (AreaId = " & cmbArea.SelectedValue & " OR AreaId IS NULL) "
        End If
        If lWhere <> "" Then
            lWhere = Regex.Replace(lWhere, "^ AND", "")
        End If
        DatasetBT1.Tbl_FeederPart.DefaultView.RowFilter = lWhere
    End Sub
    ' Filles and Searches "Nazer"s (Used EveryWhere cmbNazer needs to be populated !)   :D
    Private Sub SearchAndFillNazer(isFill As Boolean)
        Dim lWhere As String = ""
        Dim lSQL As String = ""
        'If txtNazerSearch.Text <> "" And Not isFill Then
        If Not isFill Then
            lWhere = " " & MergeFarsiAndArabi("NazerName", txtNazerSearch.Text.Replace("*", "[*]").Replace("%", "[%]"), , "")
            mDs.Tbl_NazerDepartment.DefaultView.RowFilter = lWhere
        End If
        If cmbDepartment.SelectedIndex > -1 And isFill Then
            txtNazerSearch.Text = ""
            lSQL = " SELECT * FROM TBL_Nazer " &
                      " LEFT JOIN Tbl_NazerDepartment ON Tbl_Nazer.NazerId = Tbl_Nazerdepartment.NazerId " &
                      " WHERE Tbl_NazerDepartment.NazerId IS NULL OR Tbl_NazerDepartment.DepartmentId = " & cmbDepartment.SelectedValue
            BindingTable(lSQL, mCnn, mDs, "TBL_NazerDepartment", aIsClearTable:=True)
            mDs.Tbl_NazerDepartment.DefaultView.RowFilter = Nothing
            cmbNazer.DataSource = mDs.Tables("TBL_NazerDepartment")
        End If
    End Sub

    Private Sub GetCountMPFeederKey()
        Try
            Dim lSQL As String = ""
            Dim lDs As New DataSet
            lSQL =
                       "SELECT Tbl_MPFeederKey.* " &
                       "FROM  " &
                       "	Tbl_MPFeederKey " &
                       "   INNER JOIN Tbl_MPFeeder ON Tbl_MPFeederKey.MPFeederId = Tbl_MPFeeder.MPFeederId OR Tbl_MPFeederKey.ManovrMPFeederId = Tbl_MPFeeder.MPFeederId " &
                       "WHERE (Tbl_MPFeeder.MPFeederId = " & cmbMPFeeder.SelectedValue & ") AND Tbl_MPFeederKey.IsActive = 1 "
            BindingTable(lSQL, mCnn, lDs, "Tbl_MPFeederKey", , , , , , , True)
            mCntMPFeederKey = lDs.Tables("Tbl_MPFeederKey").Rows.Count

            mIsSelectKey = False
            If mCntMPFeederKey > 0 Then
                Dim lRows() As DataRow = DatasetTemp1.TblTamirRequestKey.Select("IsFinalKeyState = 0 AND TamirRequestMultiStepId IS NULL AND KeyStateId = 1 ")
                If lRows.Length > 0 Then
                    mIsSelectKey = True
                End If
            End If
        Catch ex As Exception
            mIsSelectKey = True
        End Try

    End Sub
    '--- File Management Section ---'
    Private Sub AddFileForUpload()

        Try
            If String.IsNullOrWhiteSpace(txtUploadFilePath.Text) Then
                Throw New Exception("لطفا فايل مورد نظر براي سند را مشخص نماييد.")
            End If
            If String.IsNullOrWhiteSpace(txtSubject.Text) Then
                Throw New Exception("لطفا موضوع سند را مشخص نماييد.")
            End If
            If Not File.Exists(txtUploadFilePath.Text) Then
                Throw New Exception("فايل مورد نظر يافت نشد.")
            End If

            Dim lFi As New FileInfo(txtUploadFilePath.Text)
            Dim lTblFiles As DatasetTamir.ViewTamirRequestFileDataTable = mDs.ViewTamirRequestFile

            If lTblFiles.Select("FileName = '" & lFi.Name.Replace("'", "''").Replace(";", "") & "'").Length > 0 Then
                Throw New Exception("فايلي با اين نام قبلا به مستندات اضافه شده است.")
            End If
            If lFi.Length > CFileServer.DefaultMaxFileSize Then
                Throw New Exception(String.Format("حجم فايل نبايد بيشتر از {0} کيلوبايت باشد.", CFileServer.DefaultMaxFileSize / 1024))
            End If

            If lFi.Name.Length > 200 Then
                Throw New Exception("نام فايل انتخاب شده نبايد بيش از 200 کاراکتر باشد")
            End If

            Dim lRowFile As DatasetTamir.ViewTamirRequestFileRow = lTblFiles.NewViewTamirRequestFileRow()

            lRowFile.TamirRequestFileId = GetAutoInc()
            lRowFile.TamirRequestId = mTamirRequestId
            lRowFile.FileServerId = -1
            lRowFile.Subject = ReplaceFarsiToArabi(txtSubject.Text)
            lRowFile.FileId = -1
            lRowFile.FileName = lFi.Name
            lRowFile.FilePath = lFi.DirectoryName
            lRowFile.FileSize = lFi.Length
            lRowFile.AreaUserId = WorkingUserId
            lRowFile.UserName = WorkingUserName
            lRowFile("Content") = DBNull.Value
            lRowFile.IsNewFile = True

            lTblFiles.AddViewTamirRequestFileRow(lRowFile)

            Dim lFUi As New FileUploadInfo() With {
                .UploadRow = lRowFile,
                .Status = FileWorkStatus.Wait,
                .Message = ""
                }

            mFileUploadList.Add(lFUi)

            txtSubject.Text = ""
            txtUploadFilePath.Text = ""

            dgFiles.RootTable.Columns().Item("FilePath").Visible = True
        Catch ex As Exception
            ShowError(ex.Message)
        End Try

    End Sub
    Private Sub AddFileToDeleteList()
        Try
            Dim lRowIndex As Integer = dgFiles.CurrentRowIndex

            Dim lFileId As Long
            Long.TryParse(dgFiles.Item(lRowIndex, "FileId").ToString(), lFileId)

            Dim lTamirRequestFileId As Long
            Long.TryParse(dgFiles.Item(lRowIndex, "TamirRequestFileId").ToString(), lTamirRequestFileId)

            If lFileId = -1 Then
                Dim lRows() As DataRow = mDs.ViewTamirRequestFile.Select("TamirRequestFileId = " & lTamirRequestFileId)
                If lRows.Length > 0 Then
                    mDs.ViewTamirRequestFile.Rows.Remove(lRows(0))
                End If
                Exit Sub
            End If

            Dim lFileServerId As Long
            Long.TryParse(dgFiles.Item(lRowIndex, "FileServerId").ToString(), lFileServerId)

            Dim lFileName As String = dgFiles.Item(lRowIndex, "FileName").ToString()

            Dim lDFi As List(Of DeletFileInfo) = (
                From lItem In mFileDeleteList
                Where lItem.FileId = lFileId
                Select lItem
            ).ToList()

            If lDFi.Count = 0 Then

                mFileDeleteList.Add(New DeletFileInfo() With {
                .FileId = lFileId,
                .FileServerId = lFileServerId,
                .FileName = lFileName,
                .TamirRequestFileId = lTamirRequestFileId,
                .Status = FileWorkStatus.Wait,
                .Message = ""
                })

                Dim lFs As New GridEXFormatStyle With {
                    .BackColor = Color.Red
                }
                dgFiles.GetRow(lRowIndex).RowStyle = lFs

            Else

                mFileDeleteList.Remove(lDFi(0))

                Dim lFs As New GridEXFormatStyle With {
                    .BackColor = Color.Transparent
                }
                dgFiles.GetRow(lRowIndex).RowStyle = lFs

            End If
        Catch ex As Exception
            ShowError(ex.Message)
        End Try
    End Sub
    Private Function RetryUploadFiles() As Boolean
        Dim lIsExistsFiles As Boolean = False
        Try
            Dim lFUIs As List(Of FileUploadInfo) = (
                From lItem In mFileUploadList
                Where lItem.Status = FileWorkStatus.Failed
                Select lItem
            ).ToList()

            For Each lFUi As FileUploadInfo In lFUIs
                lFUi.Status = FileWorkStatus.Wait
            Next

            UploadFiles()
        Catch ex As Exception
            ShowError(ex)
        End Try
        Return (Not lIsExistsFiles)
    End Function
    Private Function SkipFailUploads() As Boolean
        Dim lIsExistsFiles As Boolean = False
        Try
            Dim lFUIs As List(Of FileUploadInfo) = (
                From lItem In mFileUploadList
                Where lItem.Status = FileWorkStatus.Failed
                Select lItem
            ).ToList()

            For Each lFUi As FileUploadInfo In lFUIs
                lFUi.Status = FileWorkStatus.Skip
            Next

            UploadFiles()
        Catch ex As Exception
            ShowError(ex)
        End Try
        Return (Not lIsExistsFiles)
    End Function
    Private Function RetryDeleteFiles() As Boolean
        Dim lIsExistsFiles As Boolean = False
        Try
            Dim lDFIs As List(Of DeletFileInfo) = (
                From lItem In mFileDeleteList
                Where lItem.Status = FileWorkStatus.Failed
                Select lItem
            ).ToList()
            For Each lDFi As DeletFileInfo In lDFIs
                lDFi.Status = FileWorkStatus.Wait
            Next

            DeleteFiles()
        Catch ex As Exception
            ShowError(ex)
        End Try
        Return (Not lIsExistsFiles)
    End Function
    Private Function SkipFailDeletes() As Boolean
        Dim lIsExistsFiles As Boolean = False
        Try
            Dim lDFIs As List(Of DeletFileInfo) = (
                From lItem In mFileDeleteList
                Where lItem.Status = FileWorkStatus.Failed
                Select lItem
            ).ToList()

            For Each lDFi As DeletFileInfo In lDFIs
                lDFi.Status = FileWorkStatus.Skip
            Next

            DeleteFiles()
        Catch ex As Exception
            ShowError(ex)
        End Try
        Return (Not lIsExistsFiles)
    End Function
    Private Function UploadFiles() As Boolean
        Dim lIsExistsFiles As Boolean = False
        Try
            Dim lFUIs As List(Of FileUploadInfo) = (
                From lItem In mFileUploadList
                Where lItem.Status = FileWorkStatus.Wait And lItem.UploadRow.RowState <> DataRowState.Detached
                Select lItem
            ).ToList()

            mIsUploadFilesFinished = (lFUIs.Count = 0)

            If lFUIs.Count > 0 Then
                mIsInFileOperation = True
                ftDocs.Visible = True

                If mFileServer Is Nothing Then
                    mFileServer = New CFileServer(cmbArea.SelectedValue)
                    ftDocs.FileServer = mFileServer
                End If

                lIsExistsFiles = True
                mCurrentFileUpload = lFUIs(0)
                'mCurrentFileUpload.Direction = FileTransfer.TransferDirection.Send
                Dim lFilePath As String = mCurrentFileUpload.UploadRow.FilePath
                If Not Regex.IsMatch(lFilePath, "\\$") Then lFilePath &= "\"
                lFilePath &= mCurrentFileUpload.UploadRow.FileName
                ftDocs.SendFile(lFilePath, mCurrentFileUpload.UploadRow.Subject)

            Else
                ftDocs.Visible = False

                If SaveTamirRequestFiles() Then
                    If mIsCloseFormAfterFileOperation Then
                        Me.DialogResult = DialogResult.OK
                        Me.Close()
                    End If
                End If

            End If

        Catch ex As Exception
            ShowError(ex.Message)
            mIsUploadFilesFinished = True
            ftDocs.Visible = False
        Finally
            mIsInFileOperation = lIsExistsFiles
        End Try
        Return (Not lIsExistsFiles)
    End Function
    Private Function DeleteFiles() As Boolean
        Dim lIsExistsFiles As Boolean = False
        Try
            Dim lDFIs As List(Of DeletFileInfo) = (
                From lItem In mFileDeleteList
                Where lItem.Status = FileWorkStatus.Wait
                Select lItem
            ).ToList()

            mIsDeleteFilesFinished = (lDFIs.Count = 0)

            If lDFIs.Count > 0 Then
                mIsInFileOperation = True
                ftDocs.Visible = True

                If mFileServer Is Nothing Then
                    mFileServer = New CFileServer(cmbArea.SelectedValue)
                    ftDocs.FileServer = mFileServer
                End If

                lIsExistsFiles = True
                mCurrentFileDelete = lDFIs(0)
                ftDocs.DeleteFile(mCurrentFileDelete.FileId, mCurrentFileDelete.FileName)

            Else
                ftDocs.Visible = False

                Dim lIsDeleteOK As Boolean = DeleteTamirRequestFiles()
                mIsInFileOperation = False
                If lIsDeleteOK Then
                    If Not mIsUploadAfterFileDelete Then
                        If mIsCloseFormAfterFileOperation Then
                            Me.DialogResult = DialogResult.OK
                            Me.Close()
                        End If
                    Else
                        Return UploadFiles()
                    End If
                End If

            End If

        Catch ex As Exception
            ShowError(ex.Message)
        Finally
            If Not mIsInFileOperation Then 'ممکن است هنگام آپلود اين متغير ست بشود و در اينحالت اينجا نبايد تغيير کند
                mIsInFileOperation = lIsExistsFiles
            End If
        End Try
        Return (Not lIsExistsFiles)
    End Function
    Private Sub FT_WorkDone(ByVal sender As Object, ByVal e As FileServerEventArgs) Handles ftDocs.WorkDone
        Try
            If e.WorkState = CFileServer.WorkStates.Finished Then

                If e.WorkingType = CFileServer.WorkingTypes.Sending Or e.WorkingType = CFileServer.WorkingTypes.Receiving Then

                    With mCurrentFileUpload
                        .Status = FileWorkStatus.Success
                        .Message = e.Message

                        If e.WorkingType = CFileServer.WorkingTypes.Sending Then
                            .UploadRow.FileServerId = e.FileServerId
                            .UploadRow.FileId = e.FileId
                        End If
                    End With

                    If e.WorkingType = CFileServer.WorkingTypes.Sending Then
                        UploadFiles()
                    Else
                        ftDocs.Visible = False
                        Dim lFi As New FileInfo(e.FilePath)
                        Dim lParam As String = String.Format("/select, ""{0}""", e.FilePath)
                        Process.Start("explorer.exe", lParam)
                    End If

                ElseIf e.WorkingType = CFileServer.WorkingTypes.Deleting Then

                    With mCurrentFileDelete
                        .Status = FileWorkStatus.Success
                        .Message = e.Message
                    End With

                    DeleteFiles()

                End If

            ElseIf e.WorkState = CFileServer.WorkStates.Failed Then

                If e.WorkingType = CFileServer.WorkingTypes.Sending Or e.WorkingType = CFileServer.WorkingTypes.Receiving Then

                    With mCurrentFileUpload
                        .Status = FileWorkStatus.Failed
                        .Message = e.Message
                        If e.WorkingType = CFileServer.WorkingTypes.Sending Then
                            .UploadRow.FileName = e.Message
                        End If
                    End With

                ElseIf e.WorkingType = CFileServer.WorkingTypes.Deleting Then

                    With mCurrentFileDelete
                        .Status = FileWorkStatus.Failed
                        .Message = e.Message
                    End With

                End If

            End If
        Catch ex As Exception
            ShowError(ex)
        Finally
            'Application.DoEvents()
        End Try
    End Sub
    Private Function SaveTamirRequestFiles() As Boolean
        Dim lIsOK As Boolean = True
        Try
            Dim lRow_TRF As DatasetTamir.TblTamirRequestFileRow = Nothing

            For Each lRow_View As DatasetTamir.ViewTamirRequestFileRow In mDs.ViewTamirRequestFile
                If lRow_View.RowState <> DataRowState.Deleted AndAlso lRow_View.IsNewFile Then

                    lRow_TRF = mDs.TblTamirRequestFile.NewTblTamirRequestFileRow()
                    With lRow_TRF
                        .TamirRequestFileId = lRow_View.TamirRequestFileId
                        .TamirRequestId = mDs.Tables("TblTamirRequest").Rows(0)("TamirRequestId")
                        .FileServerId = lRow_View.FileServerId
                        .Item("Subject") = lRow_View("Subject")
                        .Item("Comment") = lRow_View("Comment")
                    End With
                    mDs.TblTamirRequestFile.AddTblTamirRequestFileRow(lRow_TRF)

                End If
            Next

            Dim lUpdate As New frmUpdateDataset()
            lUpdate.UpdateDataSet("TblTamirRequestFile", mDs, cmbArea.SelectedValue, aIsUpdateDeleteRecords:=False)

        Catch ex As Exception
            lIsOK = False
            ShowError(ex)
        End Try
        Return lIsOK
    End Function
    Private Function DeleteTamirRequestFiles() As Boolean
        Dim lIsOK As Boolean = True
        Try
            For Each lRow_View As DatasetTamir.ViewTamirRequestFileRow In mDs.ViewTamirRequestFile

                Dim lDFIs As List(Of DeletFileInfo) = (
                    From lItem In mFileDeleteList
                    Where lItem.Status = FileWorkStatus.Success And lItem.FileId = lRow_View.FileId
                    Select lItem
                ).ToList()

                If lDFIs.Count > 0 Then

                    Dim lRows() As DatasetTamir.TblTamirRequestFileRow = mDs.TblTamirRequestFile.Select("TamirRequestFileId = " & lRow_View.TamirRequestFileId)
                    For Each lRow_TRF As DatasetTamir.TblTamirRequestFileRow In lRows
                        lRow_TRF.Delete()
                    Next
                    lRow_View.Delete()

                End If

                lIsOK = True
            Next

            Dim lUpdate As New frmUpdateDataset()
            lUpdate.UpdateDataSet("TblTamirRequestFile", mDs, cmbArea.SelectedValue, aIsApplyOnDeleteRecords:=True)

        Catch ex As Exception
            lIsOK = False
            ShowError(ex)
        End Try
        Return lIsOK
    End Function
    Private Sub DownloadFile(aFileId As Long, aFileName As String, Optional aSubject As String = "")
        Try
            If aFileId < 0 Then Exit Sub

            Dim lSFD As New SaveFileDialog()
            Try
                lSFD.Title = "انتخاب محل ذخيره سند"
                lSFD.FileName = aFileName
                If lSFD.ShowDialog() = DialogResult.Cancel Then
                    Exit Sub
                End If

                txtUploadFilePath.Text = lSFD.FileName
            Catch ex As Exception
            End Try

            mCurrentFileUpload = New FileUploadInfo With {
                .Status = FileWorkStatus.Wait,
                .Message = "",
                .UploadRow = Nothing
            }
            '   .Direction = FileTransfer.TransferDirection.Receive,

            If mFileServer Is Nothing Then
                mFileServer = New CFileServer(cmbArea.SelectedValue)
                ftDocs.FileServer = mFileServer
            End If

            ftDocs.Visible = True
            ftDocs.ReceiveFile(aFileId, lSFD.FileName, aSubject)
        Catch ex As Exception
            ShowError(ex.Message)
        End Try
    End Sub
    '-------------------------------'
    Private Sub SelectLPPost()
        Dim lLPPostId As Integer = -1
        If cmbLPPost.SelectedIndex > -1 Then lLPPostId = cmbLPPost.SelectedValue
        Dim lDlg As frmBaseTablesNotStd
        lDlg = New frmBaseTablesNotStd("Tbl_LPPost", True, lLPPostId, , , , , , , , True, 3)
        If lDlg.ShowDialog = DialogResult.Cancel Then
            lDlg.Dispose()
            Exit Sub
        End If

        lLPPostId = lDlg.SelectedID
        lDlg.Dispose()
        Dim lDs As New DataSet
        Dim lSQL As String =
            " SELECT t0.AreaId,t0.LPPostCode, t2.MPPostId, t1.MPFeederId " &
            " FROM Tbl_LPPost t0 INNER JOIN Tbl_MPFeeder t1 ON t0.MPFeederId = t1.MPFeederId INNER JOIN Tbl_MPPost t2 ON t1.MPPostId = t2.MPPOstId " &
            " WHERE t0.LPPostId = " & lLPPostId
        BindingTable(lSQL, mCnn, lDs, "MPPostFeederSearch")
        Try
            cmbArea.SelectedValue = lDs.Tables("MPPostFeederSearch").Rows(0).Item("AreaId")
            cmbMPPost.SelectedValue = lDs.Tables("MPPostFeederSearch").Rows(0).Item("MPPostId")
            cmbMPFeeder.SelectedValue = lDs.Tables("MPPostFeederSearch").Rows(0).Item("MPFeederId")
            cmbLPPost.SelectedValue = lLPPostId
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub
    Private Sub SelectLPFeeder()
        Dim lLPFeederId As Integer = -1
        If cmbLPFeeder.SelectedIndex > -1 Then lLPFeederId = cmbLPFeeder.SelectedValue
        Dim lDlg As frmBaseTablesNotStd
        lDlg = New frmBaseTablesNotStd("Tbl_LPFeeder", True, lLPFeederId, , , , , , , , True, 3)
        If lDlg.ShowDialog = DialogResult.Cancel Then
            lDlg.Dispose()
            Exit Sub
        End If
        'MsgBox("Feeder Id: " & lLPFeederId)
        lLPFeederId = lDlg.SelectedID
        lDlg.Dispose()
        Dim lDs As New DataSet
        Dim lSQL As String =
            "SELECT t0.AreaId,t0.LPPostCode,t0.LPPostId, t2.MPPostId, t1.MPFeederId " &
            "FROM Tbl_LPFeeder f " &
            "INNER JOIN Tbl_LPPost t0 ON t0.LPPostId = f.LPPostId " &
            "INNER JOIN Tbl_MPFeeder t1 ON t0.MPFeederId = t1.MPFeederId " &
            "INNER JOIN Tbl_MPPost t2 ON t1.MPPostId = t2.MPPOstId " &
            "WHERE f.LPFeederId = " & lLPFeederId
        BindingTable(lSQL, mCnn, lDs, "LPPostFeederSearch")
        Try
            cmbArea.SelectedValue = lDs.Tables("LPPostFeederSearch").Rows(0).Item("AreaId")
            cmbMPPost.SelectedValue = lDs.Tables("LPPostFeederSearch").Rows(0).Item("MPPostId")
            cmbMPFeeder.SelectedValue = lDs.Tables("LPPostFeederSearch").Rows(0).Item("MPFeederId")
            cmbLPPost.SelectedValue = lDs.Tables("LPPostFeederSearch").Rows(0).Item("LPPostId")
            cmbLPFeeder.SelectedValue = lLPFeederId
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub btnLPFeeder_Click(sender As Object, e As EventArgs) Handles btnLPFeeder.Click
        SelectLPFeeder()
    End Sub
    '-----------<omid/>
    Private Sub txtReturn_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtReturn.KeyPress
        If Asc(e.KeyChar) <> 8 And txtReturn.TextLength < 5 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            End If
        End If
    End Sub
    Private Sub ReturnDT()
        If Not pnlConfirm.Enabled Then Exit Sub
        If Not rbIsReturned.Checked Then Exit Sub
        Dim lMins As Integer = Val(txtReturn.Text)
        Dim lRetrunDT As DateTime = GetServerTimeInfo().MiladiDate.AddMinutes(lMins)
        If lRetrunDT > mEditingRow("ConnectDT") Then
            ShowInfo("مهلت زمان عودت از زمان قطع بیشتر می باشد")
        End If
        mEditingRow("ReturnTimeoutDT") = lRetrunDT
    End Sub
#End Region

End Class

