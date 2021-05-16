Imports System
Imports System.Data.SqlClient
Imports System.Xml
Imports System.IO
Imports System.Drawing
Imports System.Windows
Imports System.Windows.Forms
Imports Bargh_UpdateDatabase
Imports System.Text.RegularExpressions
Imports Microsoft.VisualBasic.Compatibility
Public Class frmBaseTablesNotStd
    Inherits FormBase

    Private mCnn As New SqlConnection(GetConnection())

    Dim BT_Name As String
    Dim TableName As String
    Dim SelectMode As Boolean

    Private mSelectedIDs As String
    Private mSelectedNames As String

    Public SelectedID As Integer
    Public SelectedName As String

    Dim OwnerID As Long = -1
    Dim GrandOwnerID As Long = 0
    Dim MPRequestType As String
    Dim EffectedGroup As String
    Dim FilterQuery As String
    Dim IsShowAllMaster As Boolean

    Private mIsShowSearch As Boolean = False
    Private mIsShowSearchGlobal As Boolean = False

    Private mWhere As String = ""
    Private mRowIndex As Integer = -1
    Public mItemNameCol As Integer = 1
    Private mIsFirstView As Boolean = True
    Private mMessageText As String = ""
    Private mAreaId As Integer = -1
    Private mIsLoading As Boolean = True

    Private mIsShowParts As Boolean = False
    Private mIsEditParts As Boolean = False

    Private mIsShowActiveEkipOnly As Boolean = False
    Friend WithEvents btnShowOnGISMap As Button
    Friend WithEvents DataGridBoolColumn13 As System.Windows.Forms.DataGridBoolColumn
    Friend WithEvents dgtc3_MPFeederCode As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents txtCoverPercentage As Bargh_Common.TextBoxPersian
    Friend WithEvents lblCoverPercentage As System.Windows.Forms.Label
    Friend WithEvents dgtc3_CoverPercentage As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents txtGISCode As System.Windows.Forms.TextBox
    Friend WithEvents lblGISCode As System.Windows.Forms.Label
    Friend WithEvents lblMPCloserType As System.Windows.Forms.Label
    Friend WithEvents cmbMPCloserType As Bargh_Common.ComboBoxPersian
    Friend WithEvents lblNotIsAllowSave As System.Windows.Forms.Label
    Private mIsSearch As Boolean = False

    Private mIsDisableAddButton As Boolean = False
    Friend WithEvents TblRecloserFunction As DataGridTableStyle
    Friend WithEvents dgtcKeyName As DataGridTextBoxColumn
    Friend WithEvents dgtcMPFeederName As DataGridTextBoxColumn
    Friend WithEvents dgtcReadDate As DataGridTextBoxColumn
    Friend WithEvents btnRecloserFunction As Button
    Friend WithEvents btnImportSyncFromExcel As Button
    Friend WithEvents btnImportFromExcel As Button
    Friend WithEvents dgtcRecloserFunctionId As DataGridTextBoxColumn
    Friend WithEvents dgtcReadTime As DataGridTextBoxColumn
    Private mTableNameText As String = ""

    Public Event onShowBasket(ByVal aFeederPostId As Integer, ByVal aBazdidTypeId As Integer)

#Region " Windows Form Designer generated code "

    Public Sub New(ByVal BT_Name As String, Optional ByVal SelectMode As Boolean = False, Optional ByVal SelectedID As Integer = -1, Optional ByVal OwnerID As Long = -1, Optional ByVal IsLoadedForLightRequest As Boolean = False, Optional ByVal MPRequestType As String = "", Optional ByVal EffectedGroup As String = "", Optional ByVal FilterQuery As String = "", Optional ByVal IsShowAllMaster As Boolean = False, Optional ByVal aGrandOwnerID As Long = 0, Optional ByVal aIsShowSearch As Boolean = False, Optional ByVal aItemNameCol As Integer = 1, Optional ByVal aMessageText As String = "", Optional ByVal aAreaId As Integer = -1, Optional ByVal aIsShowSearchGlobal As Boolean = False, Optional aShowLoadFromExcelFunc As Func(Of Boolean) = Nothing)
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
        'Add any initialization after the InitializeComponent() call

        Me.BT_Name = BT_Name
        Me.SelectMode = SelectMode
        Me.SelectedID = SelectedID
        Me.OwnerID = OwnerID
        Me.MPRequestType = MPRequestType
        Me.EffectedGroup = EffectedGroup
        Me.FilterQuery = FilterQuery
        Me.IsShowAllMaster = IsShowAllMaster
        Me.GrandOwnerID = aGrandOwnerID
        mIsShowSearch = aIsShowSearch
        mIsShowSearchGlobal = aIsShowSearchGlobal
        mItemNameCol = aItemNameCol
        mMessageText = aMessageText
        mAreaId = aAreaId

        If BT_Name = "ViewLPFeederLoad" Then
            ButtonEditField.Left = ButtonAdd.Left
            ButtonEditField.Text = "نمايش"
            ButtonEditField.Image = btnInVisible.Image
            ButtonAdd.Visible = False
        End If
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
    Friend WithEvents DatasetCcReq1 As Bargh_DataSets.DatasetCcRequester
    Friend WithEvents mDatasetMisc As Bargh_DataSets.DatasetMisc
    Friend WithEvents ButtonEditField As System.Windows.Forms.Button
    Friend WithEvents ButtonAdd As System.Windows.Forms.Button
    Friend WithEvents ButtonDelete As System.Windows.Forms.Button
    Friend WithEvents dg As System.Windows.Forms.DataGrid
    Friend WithEvents BttnSelect As System.Windows.Forms.Button
    Friend WithEvents bttn_MPFeederList As System.Windows.Forms.Button
    Friend WithEvents bttn_LPPost As System.Windows.Forms.Button
    Friend WithEvents bttn_LPFeeder As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Tbl_Master As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents View_MPPost As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents View_Parts As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents View_MPFeeder As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents View_LPPost As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents View_LPFeeder As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents DataGridTextBoxColumn6 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn7 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn10 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn11 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn12 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn13 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn14 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn15 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn22 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn23 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn24 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn25 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn26 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn27 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn28 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn29 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn30 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn31 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn32 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn33 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DatasetCcReqView1 As Bargh_DataSets.DatasetCcRequesterView
    Friend WithEvents View_LPCommonFeeder As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents View_MPCommonFeeder As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents ViewCriticalFeeder As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents DataGridTextBoxColumn34 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn35 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn36 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn37 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn38 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn39 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn40 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn41 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn42 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn43 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn44 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn45 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn46 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn47 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn48 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn49 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn50 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn51 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn52 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents View_City As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents DataGridTextBoxColumn55 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn56 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn57 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn58 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents cboArea As ComboBoxPersian
    Friend WithEvents View_DisconnectGroupLP As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents View_DisconnectGroupMP As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents DataGridTextBoxColumn1 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn2 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn3 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn4 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridBoolColumn1 As System.Windows.Forms.DataGridBoolColumn
    Friend WithEvents DataGridTextBoxColumn5 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn8 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn9 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn53 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridBoolColumn2 As System.Windows.Forms.DataGridBoolColumn
    Friend WithEvents View_Area As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents DataGridTextBoxColumn61 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn62 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn63 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn64 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn65 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridBoolColumn3 As System.Windows.Forms.DataGridBoolColumn
    Friend WithEvents DataGridBoolColumn4 As System.Windows.Forms.DataGridBoolColumn
    Friend WithEvents Bttn_DisconnectReason As System.Windows.Forms.Button
    Friend WithEvents Bttn_DisconnectReasonXPReason As System.Windows.Forms.Button
    Friend WithEvents TblEkipProfile As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents View_MPPostTrans As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents DataGridTextBoxColumn60 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn66 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn67 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents Bttn_MPPostTrans As System.Windows.Forms.Button
    Friend WithEvents bttn_LPPostLoad As System.Windows.Forms.Button
    Friend WithEvents ViewLPPostLoad As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents DataGridTextBoxColumn68 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn69 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn70 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents bttn_LPFeederLoad As System.Windows.Forms.Button
    Friend WithEvents ViewLPFeederLoad As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents DataGridTextBoxColumn71 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn72 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn73 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn74 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn76 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn77 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents View_DisconnectGroupLight As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents DataGridTextBoxColumn78 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn79 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn80 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn81 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridBoolColumn5 As System.Windows.Forms.DataGridBoolColumn
    Friend WithEvents DataGridBoolColumn6 As System.Windows.Forms.DataGridBoolColumn
    Friend WithEvents DataGridBoolColumn7 As System.Windows.Forms.DataGridBoolColumn
    Friend WithEvents DataGridBoolColumn8 As System.Windows.Forms.DataGridBoolColumn
    Friend WithEvents DataGridBoolColumn9 As System.Windows.Forms.DataGridBoolColumn
    Friend WithEvents BttnPartType As System.Windows.Forms.Button
    Friend WithEvents Tbl_GroupPart As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents DataGridTextBoxColumn82 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn83 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents BttnMPLPPart As System.Windows.Forms.Button
    Friend WithEvents VIEW_PartType As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents VIEW_MPPartType As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents VIEW_LPPartType As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents DataGridTextBoxColumn88 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn89 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn90 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn91 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn92 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn93 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn94 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn95 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents grpSearch As System.Windows.Forms.Panel
    Friend WithEvents BtnSearch As System.Windows.Forms.Button
    Friend WithEvents txtLPPostName As System.Windows.Forms.TextBox
    Friend WithEvents cmbArea As ComboBoxPersian
    Friend WithEvents txtPeakBarFrom As System.Windows.Forms.TextBox
    Friend WithEvents txtPeakBarTo As System.Windows.Forms.TextBox
    Friend WithEvents cmbMPPost As ComboBoxPersian
    Friend WithEvents cmbMPFeeder As ComboBoxPersian
    Friend WithEvents DatasetCcReq2 As Bargh_DataSets.DatasetCcRequester
    Friend WithEvents lblArea As System.Windows.Forms.Label
    Friend WithEvents lblMPFeeder As System.Windows.Forms.Label
    Friend WithEvents lblMPPost As System.Windows.Forms.Label
    Friend WithEvents lblLPPostName As System.Windows.Forms.Label
    Friend WithEvents lblPeakBarFrom As System.Windows.Forms.Label
    Friend WithEvents lblPeakBarTo As System.Windows.Forms.Label
    Friend WithEvents lblLPPost As System.Windows.Forms.Label
    Friend WithEvents cmbLPPost As ComboBoxPersian
    Friend WithEvents Tbl_Zone As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents dgtcZoneId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtcArea As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtcZoneName As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents btnZonePreCodes As System.Windows.Forms.Button
    Friend WithEvents dgtc_BazdidCheckListId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_CheckListName As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_CheckListCode As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_CheckListDesc As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_IsActive As System.Windows.Forms.DataGridBoolColumn
    Friend WithEvents dgtc_BazdidPrice As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_ServicePrice As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_FillNumberName As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_BazdidDisconnectType As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_ServiceDisconnectType As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_BazdidType As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_Etebar As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_BazdidCheckListGroup As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DatasetBazdid1 As Bargh_DataSets.DatasetBazdid
    Friend WithEvents DatasetBazdidView1 As Bargh_DataSets.DatasetBazdidView
    Friend WithEvents BTbl_BazdidCheckListGroup As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents dgtc_BazdidCheckListGroupId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_BazdidCheckListGroupName As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_GroupBazdidType As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents btnGroupChekList As System.Windows.Forms.Button
    Friend WithEvents View_BazdidCheckList As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents btnSubCheckList As System.Windows.Forms.Button
    Friend WithEvents BTbl_SubCheckList As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents dgtc_SubCheckListId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_BazdidSubCheckListGroupName As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_OwnerCheckListName As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_SubCheckListName As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_CheckListGroupId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents BTbl_Tajhizat As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents dgtc_TajhizatId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_TajhizatName As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_BazdidTypeName As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents btnCheckListTajhizat As System.Windows.Forms.Button
    Friend WithEvents dgtc_BazdidTypeId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents BTbl_ServicePart As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents dgtc_ServicePartId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_ServicePartName As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_BazdidTypeNamePart As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents BTbl_BazdidMaster As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents dgtc_BazdidMasterId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_Name As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_Address As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_Tel As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_Mobile As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_bazdidMemberIsActive As System.Windows.Forms.DataGridBoolColumn
    Friend WithEvents BTblBazdidEkip As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents dgtc_BazdidEkipId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_BazdidEkipName As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgic_BazdidCheckListIcon As DataGridIconColumn
    Friend WithEvents BTbl_BazdidCheckListSubGroup As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents dgtc_BazdidCheckListSubGroupId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_BazdidCheckListSubGroup_GroupName As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgic_BazdidCheckListSubGroupIcon As DataGridIconColumn
    Friend WithEvents dgtc_BazdidCheckListSubGroupName As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents BTbl_LPPostDetail As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents dgtc_LPPostDetailId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgic_LPPostDetailIcon As DataGridIconColumn
    Friend WithEvents dgtc_LPPostDetailName As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgbc_IsFix As System.Windows.Forms.DataGridBoolColumn
    Friend WithEvents dgtc_IsHavayi As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgbc_IsUserCreated As System.Windows.Forms.DataGridBoolColumn
    Friend WithEvents dgtc_BazdidSubGroupId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_SubGroupName As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_CheckListLPPostDetailName As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_CheckListLPPostDetailId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_SubCheckListCode As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents btnActive_DeActive As System.Windows.Forms.Button
    Friend WithEvents Tbl_FeederPart As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents dgtc_FeederPartId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_FeederPart As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_MPFeederId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_MPFeederName As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents btnFeederPart As System.Windows.Forms.Button
    Friend WithEvents dgtc_MPPostName As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents txtLPPostCode As System.Windows.Forms.TextBox
    Friend WithEvents dgtc_LPPostCode As DGTextEditColumn
    Friend WithEvents lblLPPostCode As System.Windows.Forms.Label
    Friend WithEvents lblAddress As System.Windows.Forms.Label
    Friend WithEvents txtAddress As System.Windows.Forms.TextBox
    Friend WithEvents dgtc_LPPostAddress As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn96 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DatasetErja1 As Bargh_DataSets.DatasetErja
    Friend WithEvents Tbl_ErjaReason As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents dgtc_ErjaReasonId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_ErjaReason As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_NetworkType As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents Tbl_ErjaPart As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents dgtc10_ErjaPartId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc10_ErjaPart As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc10_PartPrice As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc10_PartUnit As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc10_ErjaReason As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc10_NetworkType As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents Tbl_ErjaOperation As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents dgtc11_ErjaOperationId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc11_ErjaOperation As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc11_OperationPrice As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc11_NetworkType As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents Tbl_DisconnectGroupSetGroup As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents dgtc_DisconnectGroupSetGroupId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_DisconnectGroupSetGroup As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_DCGSG_SortOrder As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents cmbActiveState As Bargh_Common.ComboBoxPersian
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents grpSearchGlobal As System.Windows.Forms.Panel
    Friend WithEvents btnSearchGlobal As System.Windows.Forms.Button
    Friend WithEvents cmbActiveStatePostFeeder As Bargh_Common.ComboBoxPersian
    Friend WithEvents btnCheckListPart As System.Windows.Forms.Button
    Friend WithEvents pnlMPPost_Down As System.Windows.Forms.Panel
    Friend WithEvents lblActiveStatePostFeeder As System.Windows.Forms.Label
    Friend WithEvents pnlPeakBar As System.Windows.Forms.Panel
    Friend WithEvents lblCriticalType As System.Windows.Forms.Label
    Friend WithEvents cmbCriticalType As Bargh_Common.ComboBoxPersian
    Friend WithEvents dgtc_ServicePartCode As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_PartUnit As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_PriceOne As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtcSP_ServicePrice As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents Tbl_TamirOperation As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents dgtc_TamirOperationId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_TamirOperation As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DatasetTamir1 As Bargh_DataSets.DatasetTamir
    Friend WithEvents dgtc_OperationDuration As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents Tbl_ManoeuvreType As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents dgtc_ManoeuvreTypeId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_ManoeuvreType As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents btnErjaCause As System.Windows.Forms.Button
    Friend WithEvents Tbl_ErjaCasue As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents dgtcEC_ErjaCauseId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtcEC_ErjaCause As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtcEC_ErjaReasonId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtcEC_ErjaReason As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_BuildYear As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents txtFromBuildYear As Bargh_Common.TextBoxPersian
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents txtToBuildYear As Bargh_Common.TextBoxPersian
    Friend WithEvents pnlLPPostYear As System.Windows.Forms.Panel
    Friend WithEvents dgts_EkipProfileId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgts_EkipProfileName As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgts_EkipProfileIsActive As System.Windows.Forms.DataGridBoolColumn
    Friend WithEvents Tbl_Peymankar As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents dgtc_PeymankarId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_PeymankarName As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_PeymankarAreaId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_PeymankarArea As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtcPostCapacity As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc3_MPFeederId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc3_Area As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc3_MPPostName As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc3_MPFeederName As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc3_IsActive As System.Windows.Forms.DataGridBoolColumn
    Friend WithEvents dgtc3_HavaeiLength As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc3_ZeminiLength As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc5_ZeminiLength As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc5_HavaeiLength As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc40_Manager As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc40_Mobile As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc40_Address As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc3_Ownership As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc4_Ownership As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc5_Ownership As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc5_IsLightFeeder As System.Windows.Forms.DataGridBoolColumn
    Friend WithEvents pnlLighFeeder As System.Windows.Forms.Panel
    Friend WithEvents rbLightLPFeeders As System.Windows.Forms.RadioButton
    Friend WithEvents rbLPLPFeeders As System.Windows.Forms.RadioButton
    Friend WithEvents rbAllLPFeeders As System.Windows.Forms.RadioButton
    Friend WithEvents dgtc_FeederPartCode As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_FPHavayiLength As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_FPZaminiLength As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_FTTrans_SortOrder As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_PartTypeId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_GroupPart As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_PartType As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_IsGroupPartMP As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_PartId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_PartName As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents pnlPartTypeEdit As System.Windows.Forms.Panel
    Friend WithEvents btnEditSerghatiPart As System.Windows.Forms.Button
    Friend WithEvents btnNewSerghatiPart As System.Windows.Forms.Button
    Friend WithEvents dgtc_MpLpPartTypeId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents chkIsShowParts As System.Windows.Forms.CheckBox
    Friend WithEvents dgtc5_LPTransName As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents lblOwnership As System.Windows.Forms.Label
    Friend WithEvents cmbOwnership As Bargh_Common.ComboBoxPersian
    Friend WithEvents btnPartFactories As System.Windows.Forms.Button
    Friend WithEvents dgtcParts_PartId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtcParts_Part As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtcParts_PartUnit As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents btnSavabeghBazdid As System.Windows.Forms.Button
    Friend WithEvents dgtcParts_IsActive As System.Windows.Forms.DataGridBoolColumn
    Friend WithEvents btnImportExcel As System.Windows.Forms.Button
    Friend WithEvents dgtc_VoiceCode As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents btnChangePostArea As System.Windows.Forms.Button
    Friend WithEvents BttnReturn As System.Windows.Forms.Button
    Friend WithEvents lblDesc_btnChangePostArea As System.Windows.Forms.Label
    Friend WithEvents btnChangeLPFeederArea As System.Windows.Forms.Button
    Friend WithEvents lblDesc_btnChangeLPFeederArea As System.Windows.Forms.Label
    Friend WithEvents dgtc_OperationTamirNetworkType As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents btnPointsAndEarthFeeder As System.Windows.Forms.Button
    Friend WithEvents dgic_CheckListCount As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgic2_CheckListCount As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents btnInVisible As System.Windows.Forms.Button
    Friend WithEvents lblSearch As System.Windows.Forms.Label
    Friend WithEvents dgtc_CheckListId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents ViewLPFeederPointsInfo As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents DataGridTextBoxColumn16 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn17 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn18 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc3_InstallDatePersian As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents Tbl_AVLCar As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents AVLCarId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents AVLCarName As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents AVLCarCode As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents AVLCarIsActive As System.Windows.Forms.DataGridBoolColumn
    Friend WithEvents AVLCarArea As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtcPostAreaId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtcLPFeederAreaId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents Tbl_MPFeederKey As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents MPFeederKeyId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents KeyName As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents KeyCode As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents KeyMPCloserType As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents KeyFactory As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents KeyGISCode As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents KeyIsActive As System.Windows.Forms.DataGridBoolColumn
    Friend WithEvents KeyArea As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents KeyMPPost As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents KeyMPFeeder As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents KeyMPFeederId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents KeyManovrMPFeederId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents KeyManovrMPFeeder As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents KeyIsManovr As System.Windows.Forms.DataGridBoolColumn
    Friend WithEvents KeyIsMainKey As System.Windows.Forms.DataGridBoolColumn
    Friend WithEvents chkIsMainKey As System.Windows.Forms.CheckBox
    Friend WithEvents dgtc3_IsDG As System.Windows.Forms.DataGridBoolColumn
    Friend WithEvents dgtc_FeederPartIsActive As System.Windows.Forms.DataGridBoolColumn
    Friend WithEvents dgtc_LPLocalCode As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents pnlSearch As System.Windows.Forms.Panel
    Friend WithEvents txtSearch As System.Windows.Forms.TextBox
    Friend WithEvents btnSearchView As System.Windows.Forms.Button
    Friend WithEvents DataGridTextBoxColumn19 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_FPAreaId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_FPArea As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents txtLPPostLocalCode As System.Windows.Forms.TextBox
    Friend WithEvents lblLPPostLocalCode As System.Windows.Forms.Label
    Friend WithEvents Tbl_Village As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents dgtc_VillageId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_VillageName As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_VillageCode As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_VillageArea As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents btn_LPPostEarth As System.Windows.Forms.Button
    Friend WithEvents ViewLPPostEarth As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents DataGridTextBoxColumn97 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn98 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn99 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn100 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents Tbl_Section As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents dgtc_SectionId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_SectionName As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_SectionCode As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_Area As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents Tbl_Town As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents TownId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents TownName As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents TownCode As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents SectionId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents SectionName As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents Area As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents Tbl_Roosta As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents RoostaId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents RoostaName As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents RoostaCode As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents VillageId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents Roosta_TownName As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents Roosta_SectionName As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents Roosta_VillageName As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents Roosta_Aria As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents Dgd_Sectionname As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgdtc_TownName As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn20 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtc_ErjaPriority As DataGridTextBoxColumn
    Friend WithEvents dgtcParts_PartCode As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents btnLPPostEarthPoint As System.Windows.Forms.Button
    Friend WithEvents ViewLPPostPointEarth As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents LPPostEarthInfoId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents LPPostEarthId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents LPPostName As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents LPPostEarthName As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents ReadDatePersian As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents ReadTime As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents LPPostId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtcTown As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtcSection As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtcVillage As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgtcRoosta As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents Tbl_AllowValidator As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents DataGridTextBoxColumn21 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn75 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn54 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn59 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridBoolColumn10 As System.Windows.Forms.DataGridBoolColumn
    Friend WithEvents DataGridBoolColumn11 As System.Windows.Forms.DataGridBoolColumn
    Friend WithEvents DataGridBoolColumn12 As System.Windows.Forms.DataGridBoolColumn
    Friend WithEvents ViewMPFeederPointsInfo As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents MPFeederPointsInfoId As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents MPFeederPointName As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents MeasureDatePersian As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents btnZoneMPFeeders As Button
    Friend WithEvents dgtcAreaId As DataGridTextBoxColumn
    Friend WithEvents DatasetBT1 As DatasetBT
    Friend WithEvents rbIsMP As System.Windows.Forms.RadioButton
    Friend WithEvents rbIsLP As System.Windows.Forms.RadioButton
    Friend WithEvents rbShowAllEkip As System.Windows.Forms.RadioButton
    Friend WithEvents DGISMP As System.Windows.Forms.DataGridBoolColumn
    Friend WithEvents DGISLP As System.Windows.Forms.DataGridBoolColumn
    Friend WithEvents dgts_EkipISMP As System.Windows.Forms.DataGridBoolColumn
    Friend WithEvents dgts_EkipISLP As System.Windows.Forms.DataGridBoolColumn
    Friend WithEvents pnlEkipNetworkType As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents cmnuReports As System.Windows.Forms.ContextMenu
    Friend WithEvents mnuBlankLPPostExcel As System.Windows.Forms.MenuItem
    Friend WithEvents mnuRegLPPostLoad As System.Windows.Forms.MenuItem
    Friend WithEvents dgtc_IsActiveErjaReason As System.Windows.Forms.DataGridBoolColumn
    Friend WithEvents Tbl_IVRCode As DataGridTableStyle
    Friend WithEvents dgtc_IVRCodeId As DataGridTextBoxColumn
    Friend WithEvents dgtc_IVRCode As DataGridTextBoxColumn
    Friend WithEvents dgtc_IVRDescription As DataGridTextBoxColumn

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBaseTablesNotStd))
        Me.DatasetCcReq1 = New Bargh_DataSets.DatasetCcRequester()
        Me.ButtonEditField = New System.Windows.Forms.Button()
        Me.ButtonAdd = New System.Windows.Forms.Button()
        Me.ButtonDelete = New System.Windows.Forms.Button()
        Me.dg = New System.Windows.Forms.DataGrid()
        Me.Tbl_Master = New System.Windows.Forms.DataGridTableStyle()
        Me.DataGridTextBoxColumn6 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn7 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn10 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn11 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn12 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridBoolColumn6 = New System.Windows.Forms.DataGridBoolColumn()
        Me.DGISMP = New System.Windows.Forms.DataGridBoolColumn()
        Me.DGISLP = New System.Windows.Forms.DataGridBoolColumn()
        Me.View_MPPost = New System.Windows.Forms.DataGridTableStyle()
        Me.DataGridTextBoxColumn13 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn14 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn15 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridBoolColumn7 = New System.Windows.Forms.DataGridBoolColumn()
        Me.View_Parts = New System.Windows.Forms.DataGridTableStyle()
        Me.dgtcParts_PartId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtcParts_PartCode = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtcParts_Part = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtcParts_PartUnit = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtcParts_IsActive = New System.Windows.Forms.DataGridBoolColumn()
        Me.View_MPFeeder = New System.Windows.Forms.DataGridTableStyle()
        Me.dgtc3_MPFeederId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc3_Area = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc3_MPPostName = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc3_MPFeederName = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc3_IsActive = New System.Windows.Forms.DataGridBoolColumn()
        Me.dgtc3_HavaeiLength = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc3_ZeminiLength = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc3_Ownership = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc3_MPFeederCode = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc3_InstallDatePersian = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc3_IsDG = New System.Windows.Forms.DataGridBoolColumn()
        Me.dgtc3_CoverPercentage = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.View_LPPost = New System.Windows.Forms.DataGridTableStyle()
        Me.DataGridTextBoxColumn22 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn23 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn24 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn25 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn26 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn27 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridBoolColumn8 = New System.Windows.Forms.DataGridBoolColumn()
        Me.dgtc_LPPostAddress = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_LPPostCode = New Bargh_Common.DGTextEditColumn()
        Me.dgtc_LPLocalCode = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_BuildYear = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtcPostCapacity = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc4_Ownership = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtcPostAreaId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtcSection = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtcTown = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtcVillage = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtcRoosta = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.View_LPFeeder = New System.Windows.Forms.DataGridTableStyle()
        Me.DataGridTextBoxColumn28 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn76 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn29 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn30 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn31 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn32 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn19 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc5_LPTransName = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn33 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridBoolColumn9 = New System.Windows.Forms.DataGridBoolColumn()
        Me.dgtc5_HavaeiLength = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc5_ZeminiLength = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc5_Ownership = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc5_IsLightFeeder = New System.Windows.Forms.DataGridBoolColumn()
        Me.dgtcLPFeederAreaId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.View_LPCommonFeeder = New System.Windows.Forms.DataGridTableStyle()
        Me.DataGridTextBoxColumn34 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn36 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn37 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn38 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn39 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn35 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn40 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.View_MPCommonFeeder = New System.Windows.Forms.DataGridTableStyle()
        Me.DataGridTextBoxColumn41 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn42 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn43 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn44 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn45 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.ViewCriticalFeeder = New System.Windows.Forms.DataGridTableStyle()
        Me.DataGridTextBoxColumn46 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn47 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn48 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn49 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn50 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn51 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn52 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.View_City = New System.Windows.Forms.DataGridTableStyle()
        Me.DataGridTextBoxColumn55 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn56 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn57 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn58 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.View_DisconnectGroupLP = New System.Windows.Forms.DataGridTableStyle()
        Me.DataGridTextBoxColumn1 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn2 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn3 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn4 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridBoolColumn1 = New System.Windows.Forms.DataGridBoolColumn()
        Me.View_DisconnectGroupMP = New System.Windows.Forms.DataGridTableStyle()
        Me.DataGridTextBoxColumn5 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn8 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn9 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn53 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridBoolColumn2 = New System.Windows.Forms.DataGridBoolColumn()
        Me.DataGridTextBoxColumn96 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.View_DisconnectGroupLight = New System.Windows.Forms.DataGridTableStyle()
        Me.DataGridTextBoxColumn78 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn79 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn80 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn81 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridBoolColumn5 = New System.Windows.Forms.DataGridBoolColumn()
        Me.View_Area = New System.Windows.Forms.DataGridTableStyle()
        Me.DataGridTextBoxColumn61 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn62 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn63 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn64 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn65 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridBoolColumn3 = New System.Windows.Forms.DataGridBoolColumn()
        Me.DataGridBoolColumn4 = New System.Windows.Forms.DataGridBoolColumn()
        Me.DataGridTextBoxColumn77 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.TblEkipProfile = New System.Windows.Forms.DataGridTableStyle()
        Me.dgts_EkipProfileId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgts_EkipProfileName = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgts_EkipProfileIsActive = New System.Windows.Forms.DataGridBoolColumn()
        Me.dgts_EkipISMP = New System.Windows.Forms.DataGridBoolColumn()
        Me.dgts_EkipISLP = New System.Windows.Forms.DataGridBoolColumn()
        Me.View_MPPostTrans = New System.Windows.Forms.DataGridTableStyle()
        Me.DataGridTextBoxColumn60 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn67 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn66 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn74 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridBoolColumn13 = New System.Windows.Forms.DataGridBoolColumn()
        Me.dgtc_FTTrans_SortOrder = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.ViewLPPostLoad = New System.Windows.Forms.DataGridTableStyle()
        Me.DataGridTextBoxColumn68 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn69 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn20 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn70 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.ViewLPFeederLoad = New System.Windows.Forms.DataGridTableStyle()
        Me.DataGridTextBoxColumn71 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn72 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn73 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.Tbl_GroupPart = New System.Windows.Forms.DataGridTableStyle()
        Me.DataGridTextBoxColumn82 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn83 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.VIEW_PartType = New System.Windows.Forms.DataGridTableStyle()
        Me.dgtc_PartTypeId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_GroupPart = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_PartType = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_IsGroupPartMP = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_MpLpPartTypeId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_PartId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_PartName = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.VIEW_MPPartType = New System.Windows.Forms.DataGridTableStyle()
        Me.DataGridTextBoxColumn88 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn89 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn90 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn91 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.VIEW_LPPartType = New System.Windows.Forms.DataGridTableStyle()
        Me.DataGridTextBoxColumn92 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn93 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn94 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn95 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.Tbl_Zone = New System.Windows.Forms.DataGridTableStyle()
        Me.dgtcZoneId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtcAreaId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtcArea = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtcZoneName = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_VoiceCode = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.View_BazdidCheckList = New System.Windows.Forms.DataGridTableStyle()
        Me.dgtc_BazdidCheckListId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_CheckListGroupId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_BazdidTypeId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_BazdidSubGroupId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_CheckListLPPostDetailId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_BazdidCheckListGroup = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_SubGroupName = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_CheckListCode = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_CheckListName = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_CheckListLPPostDetailName = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_CheckListDesc = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_BazdidDisconnectType = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_ServiceDisconnectType = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_BazdidType = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_IsActive = New System.Windows.Forms.DataGridBoolColumn()
        Me.dgtc_BazdidPrice = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_ServicePrice = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_Etebar = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_FillNumberName = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.BTbl_BazdidCheckListGroup = New System.Windows.Forms.DataGridTableStyle()
        Me.dgtc_BazdidCheckListGroupId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_BazdidCheckListGroupName = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_GroupBazdidType = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgic_BazdidCheckListIcon = New Bargh_Common.DataGridIconColumn()
        Me.dgic_CheckListCount = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.BTbl_SubCheckList = New System.Windows.Forms.DataGridTableStyle()
        Me.dgtc_SubCheckListId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_BazdidSubCheckListGroupName = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_OwnerCheckListName = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_SubCheckListCode = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_SubCheckListName = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_CheckListId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_ErjaPriority = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.BTbl_ServicePart = New System.Windows.Forms.DataGridTableStyle()
        Me.dgtc_ServicePartId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_ServicePartCode = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_ServicePartName = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_PartUnit = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_BazdidTypeNamePart = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_PriceOne = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtcSP_ServicePrice = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.BTbl_BazdidMaster = New System.Windows.Forms.DataGridTableStyle()
        Me.dgtc_BazdidMasterId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_Name = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_Address = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_Tel = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_Mobile = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_bazdidMemberIsActive = New System.Windows.Forms.DataGridBoolColumn()
        Me.BTblBazdidEkip = New System.Windows.Forms.DataGridTableStyle()
        Me.dgtc_BazdidEkipId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_BazdidEkipName = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.BTbl_BazdidCheckListSubGroup = New System.Windows.Forms.DataGridTableStyle()
        Me.dgtc_BazdidCheckListSubGroupId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_BazdidCheckListSubGroup_GroupName = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_BazdidCheckListSubGroupName = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgic_BazdidCheckListSubGroupIcon = New Bargh_Common.DataGridIconColumn()
        Me.dgic2_CheckListCount = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.BTbl_LPPostDetail = New System.Windows.Forms.DataGridTableStyle()
        Me.dgtc_LPPostDetailId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgbc_IsUserCreated = New System.Windows.Forms.DataGridBoolColumn()
        Me.dgic_LPPostDetailIcon = New Bargh_Common.DataGridIconColumn()
        Me.dgtc_LPPostDetailName = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgbc_IsFix = New System.Windows.Forms.DataGridBoolColumn()
        Me.dgtc_IsHavayi = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.Tbl_FeederPart = New System.Windows.Forms.DataGridTableStyle()
        Me.dgtc_FeederPartId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_FPAreaId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_MPFeederId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_FPArea = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_MPPostName = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_MPFeederName = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_FeederPart = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_FeederPartCode = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_FeederPartIsActive = New System.Windows.Forms.DataGridBoolColumn()
        Me.dgtc_FPHavayiLength = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_FPZaminiLength = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.Tbl_ErjaReason = New System.Windows.Forms.DataGridTableStyle()
        Me.dgtc_ErjaReasonId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_NetworkType = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_ErjaReason = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_IsActiveErjaReason = New System.Windows.Forms.DataGridBoolColumn()
        Me.Tbl_ErjaPart = New System.Windows.Forms.DataGridTableStyle()
        Me.dgtc10_ErjaPartId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc10_ErjaPart = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc10_PartUnit = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc10_PartPrice = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc10_NetworkType = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc10_ErjaReason = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.Tbl_ErjaOperation = New System.Windows.Forms.DataGridTableStyle()
        Me.dgtc11_ErjaOperationId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc11_ErjaOperation = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc11_OperationPrice = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc11_NetworkType = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.Tbl_DisconnectGroupSetGroup = New System.Windows.Forms.DataGridTableStyle()
        Me.dgtc_DisconnectGroupSetGroupId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_DisconnectGroupSetGroup = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_DCGSG_SortOrder = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.Tbl_TamirOperation = New System.Windows.Forms.DataGridTableStyle()
        Me.dgtc_TamirOperationId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_TamirOperation = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_OperationDuration = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_OperationTamirNetworkType = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.Tbl_ManoeuvreType = New System.Windows.Forms.DataGridTableStyle()
        Me.dgtc_ManoeuvreTypeId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_ManoeuvreType = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.Tbl_ErjaCasue = New System.Windows.Forms.DataGridTableStyle()
        Me.dgtcEC_ErjaCauseId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtcEC_ErjaReasonId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtcEC_ErjaReason = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtcEC_ErjaCause = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.Tbl_Peymankar = New System.Windows.Forms.DataGridTableStyle()
        Me.dgtc_PeymankarId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_PeymankarAreaId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_PeymankarArea = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_PeymankarName = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc40_Manager = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc40_Mobile = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc40_Address = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.ViewLPFeederPointsInfo = New System.Windows.Forms.DataGridTableStyle()
        Me.DataGridTextBoxColumn16 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn17 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn18 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.Tbl_AVLCar = New System.Windows.Forms.DataGridTableStyle()
        Me.AVLCarId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.AVLCarArea = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.AVLCarName = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.AVLCarCode = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.AVLCarIsActive = New System.Windows.Forms.DataGridBoolColumn()
        Me.Tbl_MPFeederKey = New System.Windows.Forms.DataGridTableStyle()
        Me.MPFeederKeyId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.KeyArea = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.KeyMPPost = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.KeyMPFeeder = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.KeyName = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.KeyCode = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.KeyGISCode = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.KeyIsMainKey = New System.Windows.Forms.DataGridBoolColumn()
        Me.KeyIsActive = New System.Windows.Forms.DataGridBoolColumn()
        Me.KeyIsManovr = New System.Windows.Forms.DataGridBoolColumn()
        Me.KeyManovrMPFeeder = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.KeyMPCloserType = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.KeyFactory = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.KeyMPFeederId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.KeyManovrMPFeederId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.Tbl_Village = New System.Windows.Forms.DataGridTableStyle()
        Me.dgtc_VillageId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_VillageArea = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.Dgd_Sectionname = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgdtc_TownName = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_VillageCode = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_VillageName = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.ViewLPPostEarth = New System.Windows.Forms.DataGridTableStyle()
        Me.DataGridTextBoxColumn97 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn98 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn99 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn100 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.Tbl_Section = New System.Windows.Forms.DataGridTableStyle()
        Me.dgtc_SectionId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_SectionCode = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_Area = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_SectionName = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.Tbl_Town = New System.Windows.Forms.DataGridTableStyle()
        Me.TownId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.Area = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.SectionName = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.TownCode = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.TownName = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.SectionId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.Tbl_Roosta = New System.Windows.Forms.DataGridTableStyle()
        Me.RoostaId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.Roosta_Aria = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.Roosta_SectionName = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.Roosta_TownName = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.Roosta_VillageName = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.RoostaCode = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.RoostaName = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.VillageId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.ViewLPPostPointEarth = New System.Windows.Forms.DataGridTableStyle()
        Me.LPPostEarthInfoId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.LPPostEarthId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.LPPostName = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.LPPostEarthName = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.ReadDatePersian = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.ReadTime = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.LPPostId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.Tbl_AllowValidator = New System.Windows.Forms.DataGridTableStyle()
        Me.DataGridTextBoxColumn21 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn75 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn54 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn59 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridBoolColumn10 = New System.Windows.Forms.DataGridBoolColumn()
        Me.DataGridBoolColumn11 = New System.Windows.Forms.DataGridBoolColumn()
        Me.DataGridBoolColumn12 = New System.Windows.Forms.DataGridBoolColumn()
        Me.ViewMPFeederPointsInfo = New System.Windows.Forms.DataGridTableStyle()
        Me.MPFeederPointsInfoId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.MPFeederPointName = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.MeasureDatePersian = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.Tbl_IVRCode = New System.Windows.Forms.DataGridTableStyle()
        Me.dgtc_IVRCodeId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_IVRCode = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_IVRDescription = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.TblRecloserFunction = New System.Windows.Forms.DataGridTableStyle()
        Me.dgtcKeyName = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtcMPFeederName = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtcReadDate = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_TajhizatId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_TajhizatName = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtc_BazdidTypeName = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DatasetCcReqView1 = New Bargh_DataSets.DatasetCcRequesterView()
        Me.BttnSelect = New System.Windows.Forms.Button()
        Me.bttn_MPFeederList = New System.Windows.Forms.Button()
        Me.bttn_LPPost = New System.Windows.Forms.Button()
        Me.bttn_LPFeeder = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cboArea = New Bargh_Common.ComboBoxPersian()
        Me.Bttn_DisconnectReason = New System.Windows.Forms.Button()
        Me.Bttn_DisconnectReasonXPReason = New System.Windows.Forms.Button()
        Me.Bttn_MPPostTrans = New System.Windows.Forms.Button()
        Me.bttn_LPPostLoad = New System.Windows.Forms.Button()
        Me.bttn_LPFeederLoad = New System.Windows.Forms.Button()
        Me.BttnPartType = New System.Windows.Forms.Button()
        Me.BttnMPLPPart = New System.Windows.Forms.Button()
        Me.grpSearch = New System.Windows.Forms.Panel()
        Me.cmbArea = New Bargh_Common.ComboBoxPersian()
        Me.DatasetCcReq2 = New Bargh_DataSets.DatasetCcRequester()
        Me.txtCoverPercentage = New Bargh_Common.TextBoxPersian()
        Me.chkIsMainKey = New System.Windows.Forms.CheckBox()
        Me.BtnSearch = New System.Windows.Forms.Button()
        Me.lblCoverPercentage = New System.Windows.Forms.Label()
        Me.txtLPPostName = New System.Windows.Forms.TextBox()
        Me.lblLPPostName = New System.Windows.Forms.Label()
        Me.lblActiveStatePostFeeder = New System.Windows.Forms.Label()
        Me.cmbActiveStatePostFeeder = New Bargh_Common.ComboBoxPersian()
        Me.txtGISCode = New System.Windows.Forms.TextBox()
        Me.lblGISCode = New System.Windows.Forms.Label()
        Me.cmbMPCloserType = New Bargh_Common.ComboBoxPersian()
        Me.lblMPCloserType = New System.Windows.Forms.Label()
        Me.lblOwnership = New System.Windows.Forms.Label()
        Me.cmbOwnership = New Bargh_Common.ComboBoxPersian()
        Me.lblArea = New System.Windows.Forms.Label()
        Me.pnlMPPost_Down = New System.Windows.Forms.Panel()
        Me.txtLPPostLocalCode = New System.Windows.Forms.TextBox()
        Me.lblLPPostLocalCode = New System.Windows.Forms.Label()
        Me.btnInVisible = New System.Windows.Forms.Button()
        Me.txtLPPostCode = New System.Windows.Forms.TextBox()
        Me.lblLPPostCode = New System.Windows.Forms.Label()
        Me.cmbMPPost = New Bargh_Common.ComboBoxPersian()
        Me.cmbMPFeeder = New Bargh_Common.ComboBoxPersian()
        Me.lblMPFeeder = New System.Windows.Forms.Label()
        Me.lblMPPost = New System.Windows.Forms.Label()
        Me.lblLPPost = New System.Windows.Forms.Label()
        Me.cmbLPPost = New Bargh_Common.ComboBoxPersian()
        Me.lblAddress = New System.Windows.Forms.Label()
        Me.txtAddress = New System.Windows.Forms.TextBox()
        Me.pnlPeakBar = New System.Windows.Forms.Panel()
        Me.txtPeakBarFrom = New System.Windows.Forms.TextBox()
        Me.lblPeakBarTo = New System.Windows.Forms.Label()
        Me.txtPeakBarTo = New System.Windows.Forms.TextBox()
        Me.lblPeakBarFrom = New System.Windows.Forms.Label()
        Me.cmbCriticalType = New Bargh_Common.ComboBoxPersian()
        Me.lblCriticalType = New System.Windows.Forms.Label()
        Me.pnlLighFeeder = New System.Windows.Forms.Panel()
        Me.rbLightLPFeeders = New System.Windows.Forms.RadioButton()
        Me.rbLPLPFeeders = New System.Windows.Forms.RadioButton()
        Me.rbAllLPFeeders = New System.Windows.Forms.RadioButton()
        Me.pnlLPPostYear = New System.Windows.Forms.Panel()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txtFromBuildYear = New Bargh_Common.TextBoxPersian()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.txtToBuildYear = New Bargh_Common.TextBoxPersian()
        Me.btnZonePreCodes = New System.Windows.Forms.Button()
        Me.DatasetBazdid1 = New Bargh_DataSets.DatasetBazdid()
        Me.DatasetBazdidView1 = New Bargh_DataSets.DatasetBazdidView()
        Me.btnGroupChekList = New System.Windows.Forms.Button()
        Me.btnSubCheckList = New System.Windows.Forms.Button()
        Me.btnCheckListTajhizat = New System.Windows.Forms.Button()
        Me.btnActive_DeActive = New System.Windows.Forms.Button()
        Me.btnFeederPart = New System.Windows.Forms.Button()
        Me.DatasetErja1 = New Bargh_DataSets.DatasetErja()
        Me.cmbActiveState = New Bargh_Common.ComboBoxPersian()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.grpSearchGlobal = New System.Windows.Forms.Panel()
        Me.btnSearchGlobal = New System.Windows.Forms.Button()
        Me.btnCheckListPart = New System.Windows.Forms.Button()
        Me.DatasetTamir1 = New Bargh_DataSets.DatasetTamir()
        Me.btnErjaCause = New System.Windows.Forms.Button()
        Me.pnlPartTypeEdit = New System.Windows.Forms.Panel()
        Me.btnEditSerghatiPart = New System.Windows.Forms.Button()
        Me.btnNewSerghatiPart = New System.Windows.Forms.Button()
        Me.chkIsShowParts = New System.Windows.Forms.CheckBox()
        Me.btnPartFactories = New System.Windows.Forms.Button()
        Me.btnSavabeghBazdid = New System.Windows.Forms.Button()
        Me.btnImportExcel = New System.Windows.Forms.Button()
        Me.btnChangePostArea = New System.Windows.Forms.Button()
        Me.BttnReturn = New System.Windows.Forms.Button()
        Me.lblDesc_btnChangePostArea = New System.Windows.Forms.Label()
        Me.btnChangeLPFeederArea = New System.Windows.Forms.Button()
        Me.lblDesc_btnChangeLPFeederArea = New System.Windows.Forms.Label()
        Me.btnPointsAndEarthFeeder = New System.Windows.Forms.Button()
        Me.lblSearch = New System.Windows.Forms.Label()
        Me.btnSearchView = New System.Windows.Forms.Button()
        Me.pnlSearch = New System.Windows.Forms.Panel()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.btn_LPPostEarth = New System.Windows.Forms.Button()
        Me.btnLPPostEarthPoint = New System.Windows.Forms.Button()
        Me.btnZoneMPFeeders = New System.Windows.Forms.Button()
        Me.DatasetBT1 = New Bargh_DataSets.DatasetBT()
        Me.rbIsMP = New System.Windows.Forms.RadioButton()
        Me.rbIsLP = New System.Windows.Forms.RadioButton()
        Me.rbShowAllEkip = New System.Windows.Forms.RadioButton()
        Me.pnlEkipNetworkType = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmnuReports = New System.Windows.Forms.ContextMenu()
        Me.mnuRegLPPostLoad = New System.Windows.Forms.MenuItem()
        Me.mnuBlankLPPostExcel = New System.Windows.Forms.MenuItem()
        Me.btnShowOnGISMap = New System.Windows.Forms.Button()
        Me.lblNotIsAllowSave = New System.Windows.Forms.Label()
        Me.btnRecloserFunction = New System.Windows.Forms.Button()
        Me.btnImportSyncFromExcel = New System.Windows.Forms.Button()
        Me.btnImportFromExcel = New System.Windows.Forms.Button()
        Me.dgtcRecloserFunctionId = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.dgtcReadTime = New System.Windows.Forms.DataGridTextBoxColumn()
        CType(Me.DatasetCcReq1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dg, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DatasetCcReqView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpSearch.SuspendLayout()
        CType(Me.DatasetCcReq2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlMPPost_Down.SuspendLayout()
        Me.pnlPeakBar.SuspendLayout()
        Me.pnlLighFeeder.SuspendLayout()
        Me.pnlLPPostYear.SuspendLayout()
        CType(Me.DatasetBazdid1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DatasetBazdidView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DatasetErja1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpSearchGlobal.SuspendLayout()
        CType(Me.DatasetTamir1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlPartTypeEdit.SuspendLayout()
        Me.pnlSearch.SuspendLayout()
        CType(Me.DatasetBT1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlEkipNetworkType.SuspendLayout()
        Me.SuspendLayout()
        '
        'HelpMaker
        '
        Me.HelpMaker.HelpNamespace = "ReportsHelp.chm"
        '
        'DatasetCcReq1
        '
        Me.DatasetCcReq1.DataSetName = "DatasetCcRequester"
        Me.DatasetCcReq1.Locale = New System.Globalization.CultureInfo("en-US")
        Me.DatasetCcReq1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'ButtonEditField
        '
        Me.ButtonEditField.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonEditField.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonEditField.Image = CType(resources.GetObject("ButtonEditField.Image"), System.Drawing.Image)
        Me.ButtonEditField.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonEditField.Location = New System.Drawing.Point(559, 465)
        Me.ButtonEditField.Name = "ButtonEditField"
        Me.ButtonEditField.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ButtonEditField.Size = New System.Drawing.Size(75, 23)
        Me.ButtonEditField.TabIndex = 2
        Me.ButtonEditField.Text = "ويـرايـش"
        Me.ButtonEditField.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ButtonAdd
        '
        Me.ButtonAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAdd.Image = CType(resources.GetObject("ButtonAdd.Image"), System.Drawing.Image)
        Me.ButtonAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonAdd.Location = New System.Drawing.Point(641, 465)
        Me.ButtonAdd.Name = "ButtonAdd"
        Me.ButtonAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ButtonAdd.Size = New System.Drawing.Size(75, 23)
        Me.ButtonAdd.TabIndex = 1
        Me.ButtonAdd.Text = "جــديــد"
        Me.ButtonAdd.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'ButtonDelete
        '
        Me.ButtonDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ButtonDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonDelete.Image = CType(resources.GetObject("ButtonDelete.Image"), System.Drawing.Image)
        Me.ButtonDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonDelete.Location = New System.Drawing.Point(9, 465)
        Me.ButtonDelete.Name = "ButtonDelete"
        Me.ButtonDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ButtonDelete.Size = New System.Drawing.Size(75, 23)
        Me.ButtonDelete.TabIndex = 6
        Me.ButtonDelete.Text = "حـذف"
        Me.ButtonDelete.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'dg
        '
        Me.dg.AlternatingBackColor = System.Drawing.Color.Silver
        Me.dg.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dg.BackColor = System.Drawing.Color.White
        Me.dg.CaptionBackColor = System.Drawing.Color.Maroon
        Me.dg.CaptionFont = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.dg.CaptionForeColor = System.Drawing.Color.White
        Me.dg.DataMember = ""
        Me.dg.DataSource = Me.DatasetCcReq1
        Me.dg.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dg.ForeColor = System.Drawing.Color.Black
        Me.dg.GridLineColor = System.Drawing.Color.Silver
        Me.dg.HeaderBackColor = System.Drawing.Color.Silver
        Me.dg.HeaderFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dg.HeaderForeColor = System.Drawing.Color.Black
        Me.dg.LinkColor = System.Drawing.Color.Maroon
        Me.dg.Location = New System.Drawing.Point(8, 8)
        Me.dg.Name = "dg"
        Me.dg.ParentRowsBackColor = System.Drawing.Color.Silver
        Me.dg.ParentRowsForeColor = System.Drawing.Color.Black
        Me.dg.ReadOnly = True
        Me.dg.SelectionBackColor = System.Drawing.Color.Maroon
        Me.dg.SelectionForeColor = System.Drawing.Color.White
        Me.dg.Size = New System.Drawing.Size(707, 441)
        Me.dg.TabIndex = 0
        Me.dg.TableStyles.AddRange(New System.Windows.Forms.DataGridTableStyle() {Me.Tbl_Master, Me.View_MPPost, Me.View_Parts, Me.View_MPFeeder, Me.View_LPPost, Me.View_LPFeeder, Me.View_LPCommonFeeder, Me.View_MPCommonFeeder, Me.ViewCriticalFeeder, Me.View_City, Me.View_DisconnectGroupLP, Me.View_DisconnectGroupMP, Me.View_DisconnectGroupLight, Me.View_Area, Me.TblEkipProfile, Me.View_MPPostTrans, Me.ViewLPPostLoad, Me.ViewLPFeederLoad, Me.Tbl_GroupPart, Me.VIEW_PartType, Me.VIEW_MPPartType, Me.VIEW_LPPartType, Me.Tbl_Zone, Me.View_BazdidCheckList, Me.BTbl_BazdidCheckListGroup, Me.BTbl_SubCheckList, Me.BTbl_ServicePart, Me.BTbl_BazdidMaster, Me.BTblBazdidEkip, Me.BTbl_BazdidCheckListSubGroup, Me.BTbl_LPPostDetail, Me.Tbl_FeederPart, Me.Tbl_ErjaReason, Me.Tbl_ErjaPart, Me.Tbl_ErjaOperation, Me.Tbl_DisconnectGroupSetGroup, Me.Tbl_TamirOperation, Me.Tbl_ManoeuvreType, Me.Tbl_ErjaCasue, Me.Tbl_Peymankar, Me.ViewLPFeederPointsInfo, Me.Tbl_AVLCar, Me.Tbl_MPFeederKey, Me.Tbl_Village, Me.ViewLPPostEarth, Me.Tbl_Section, Me.Tbl_Town, Me.Tbl_Roosta, Me.ViewLPPostPointEarth, Me.Tbl_AllowValidator, Me.ViewMPFeederPointsInfo, Me.Tbl_IVRCode, Me.TblRecloserFunction})
        '
        'Tbl_Master
        '
        Me.Tbl_Master.DataGrid = Me.dg
        Me.Tbl_Master.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.DataGridTextBoxColumn6, Me.DataGridTextBoxColumn7, Me.DataGridTextBoxColumn10, Me.DataGridTextBoxColumn11, Me.DataGridTextBoxColumn12, Me.DataGridBoolColumn6, Me.DGISMP, Me.DGISLP})
        Me.Tbl_Master.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.Tbl_Master.MappingName = "Tbl_Master"
        '
        'DataGridTextBoxColumn6
        '
        Me.DataGridTextBoxColumn6.Format = ""
        Me.DataGridTextBoxColumn6.FormatInfo = Nothing
        Me.DataGridTextBoxColumn6.HeaderText = "Id"
        Me.DataGridTextBoxColumn6.MappingName = "MasterId"
        Me.DataGridTextBoxColumn6.NullText = "-"
        Me.DataGridTextBoxColumn6.Width = 0
        '
        'DataGridTextBoxColumn7
        '
        Me.DataGridTextBoxColumn7.Format = ""
        Me.DataGridTextBoxColumn7.FormatInfo = Nothing
        Me.DataGridTextBoxColumn7.HeaderText = "مشخصات"
        Me.DataGridTextBoxColumn7.MappingName = "Name"
        Me.DataGridTextBoxColumn7.NullText = "-"
        Me.DataGridTextBoxColumn7.Width = 145
        '
        'DataGridTextBoxColumn10
        '
        Me.DataGridTextBoxColumn10.Format = ""
        Me.DataGridTextBoxColumn10.FormatInfo = Nothing
        Me.DataGridTextBoxColumn10.HeaderText = "آدرس"
        Me.DataGridTextBoxColumn10.MappingName = "Address"
        Me.DataGridTextBoxColumn10.NullText = "-"
        Me.DataGridTextBoxColumn10.Width = 150
        '
        'DataGridTextBoxColumn11
        '
        Me.DataGridTextBoxColumn11.Format = ""
        Me.DataGridTextBoxColumn11.FormatInfo = Nothing
        Me.DataGridTextBoxColumn11.HeaderText = "تلفن ثابت"
        Me.DataGridTextBoxColumn11.MappingName = "Tel"
        Me.DataGridTextBoxColumn11.NullText = "-"
        Me.DataGridTextBoxColumn11.Width = 75
        '
        'DataGridTextBoxColumn12
        '
        Me.DataGridTextBoxColumn12.Format = ""
        Me.DataGridTextBoxColumn12.FormatInfo = Nothing
        Me.DataGridTextBoxColumn12.HeaderText = "تلفن همراه"
        Me.DataGridTextBoxColumn12.MappingName = "Mobile"
        Me.DataGridTextBoxColumn12.NullText = "-"
        Me.DataGridTextBoxColumn12.Width = 75
        '
        'DataGridBoolColumn6
        '
        Me.DataGridBoolColumn6.HeaderText = "فعال"
        Me.DataGridBoolColumn6.MappingName = "IsActive"
        Me.DataGridBoolColumn6.Width = 50
        '
        'DGISMP
        '
        Me.DGISMP.HeaderText = "فشارمتوسط"
        Me.DGISMP.MappingName = "IsMP"
        Me.DGISMP.Width = 75
        '
        'DGISLP
        '
        Me.DGISLP.HeaderText = "فشارضعیف"
        Me.DGISLP.MappingName = "IsLP"
        Me.DGISLP.Width = 75
        '
        'View_MPPost
        '
        Me.View_MPPost.DataGrid = Me.dg
        Me.View_MPPost.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.DataGridTextBoxColumn13, Me.DataGridTextBoxColumn14, Me.DataGridTextBoxColumn15, Me.DataGridBoolColumn7})
        Me.View_MPPost.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.View_MPPost.MappingName = "View_MPPost"
        '
        'DataGridTextBoxColumn13
        '
        Me.DataGridTextBoxColumn13.Format = ""
        Me.DataGridTextBoxColumn13.FormatInfo = Nothing
        Me.DataGridTextBoxColumn13.HeaderText = "Id"
        Me.DataGridTextBoxColumn13.MappingName = "MPPostId"
        Me.DataGridTextBoxColumn13.NullText = "-"
        Me.DataGridTextBoxColumn13.Width = 0
        '
        'DataGridTextBoxColumn14
        '
        Me.DataGridTextBoxColumn14.Format = ""
        Me.DataGridTextBoxColumn14.FormatInfo = Nothing
        Me.DataGridTextBoxColumn14.HeaderText = "ناحيه"
        Me.DataGridTextBoxColumn14.MappingName = "Area"
        Me.DataGridTextBoxColumn14.NullText = "-"
        Me.DataGridTextBoxColumn14.Width = 200
        '
        'DataGridTextBoxColumn15
        '
        Me.DataGridTextBoxColumn15.Format = ""
        Me.DataGridTextBoxColumn15.FormatInfo = Nothing
        Me.DataGridTextBoxColumn15.HeaderText = "نام پست فوق توزيع"
        Me.DataGridTextBoxColumn15.MappingName = "MPPostName"
        Me.DataGridTextBoxColumn15.NullText = "-"
        Me.DataGridTextBoxColumn15.Width = 235
        '
        'DataGridBoolColumn7
        '
        Me.DataGridBoolColumn7.HeaderText = "فعال"
        Me.DataGridBoolColumn7.MappingName = "IsActive"
        Me.DataGridBoolColumn7.NullText = "False"
        Me.DataGridBoolColumn7.Width = 35
        '
        'View_Parts
        '
        Me.View_Parts.DataGrid = Me.dg
        Me.View_Parts.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.dgtcParts_PartId, Me.dgtcParts_PartCode, Me.dgtcParts_Part, Me.dgtcParts_PartUnit, Me.dgtcParts_IsActive})
        Me.View_Parts.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.View_Parts.MappingName = "View_Parts"
        '
        'dgtcParts_PartId
        '
        Me.dgtcParts_PartId.Format = ""
        Me.dgtcParts_PartId.FormatInfo = Nothing
        Me.dgtcParts_PartId.HeaderText = "Id"
        Me.dgtcParts_PartId.MappingName = "PartId"
        Me.dgtcParts_PartId.NullText = "-"
        Me.dgtcParts_PartId.Width = 0
        '
        'dgtcParts_PartCode
        '
        Me.dgtcParts_PartCode.Format = ""
        Me.dgtcParts_PartCode.FormatInfo = Nothing
        Me.dgtcParts_PartCode.HeaderText = "کد تجهیز"
        Me.dgtcParts_PartCode.MappingName = "PartCode"
        Me.dgtcParts_PartCode.NullText = "-"
        Me.dgtcParts_PartCode.Width = 50
        '
        'dgtcParts_Part
        '
        Me.dgtcParts_Part.Format = ""
        Me.dgtcParts_Part.FormatInfo = Nothing
        Me.dgtcParts_Part.HeaderText = "نام تجهيز"
        Me.dgtcParts_Part.MappingName = "Part"
        Me.dgtcParts_Part.NullText = "-"
        Me.dgtcParts_Part.Width = 405
        '
        'dgtcParts_PartUnit
        '
        Me.dgtcParts_PartUnit.Format = ""
        Me.dgtcParts_PartUnit.FormatInfo = Nothing
        Me.dgtcParts_PartUnit.HeaderText = "واحد"
        Me.dgtcParts_PartUnit.MappingName = "PartUnit"
        Me.dgtcParts_PartUnit.NullText = "-"
        Me.dgtcParts_PartUnit.Width = 90
        '
        'dgtcParts_IsActive
        '
        Me.dgtcParts_IsActive.HeaderText = "فعال"
        Me.dgtcParts_IsActive.MappingName = "IsActive"
        Me.dgtcParts_IsActive.Width = 50
        '
        'View_MPFeeder
        '
        Me.View_MPFeeder.DataGrid = Me.dg
        Me.View_MPFeeder.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.dgtc3_MPFeederId, Me.dgtc3_Area, Me.dgtc3_MPPostName, Me.dgtc3_MPFeederName, Me.dgtc3_IsActive, Me.dgtc3_HavaeiLength, Me.dgtc3_ZeminiLength, Me.dgtc3_Ownership, Me.dgtc3_MPFeederCode, Me.dgtc3_InstallDatePersian, Me.dgtc3_IsDG, Me.dgtc3_CoverPercentage})
        Me.View_MPFeeder.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.View_MPFeeder.MappingName = "View_MPFeeder"
        '
        'dgtc3_MPFeederId
        '
        Me.dgtc3_MPFeederId.Format = ""
        Me.dgtc3_MPFeederId.FormatInfo = Nothing
        Me.dgtc3_MPFeederId.HeaderText = "Id"
        Me.dgtc3_MPFeederId.MappingName = "MPFeederId"
        Me.dgtc3_MPFeederId.NullText = "-"
        Me.dgtc3_MPFeederId.Width = 0
        '
        'dgtc3_Area
        '
        Me.dgtc3_Area.Format = ""
        Me.dgtc3_Area.FormatInfo = Nothing
        Me.dgtc3_Area.HeaderText = "ناحيه"
        Me.dgtc3_Area.MappingName = "Area"
        Me.dgtc3_Area.Width = 75
        '
        'dgtc3_MPPostName
        '
        Me.dgtc3_MPPostName.Format = ""
        Me.dgtc3_MPPostName.FormatInfo = Nothing
        Me.dgtc3_MPPostName.HeaderText = "نام پست فوق توزيع"
        Me.dgtc3_MPPostName.MappingName = "MPPostName"
        Me.dgtc3_MPPostName.NullText = "-"
        Me.dgtc3_MPPostName.Width = 175
        '
        'dgtc3_MPFeederName
        '
        Me.dgtc3_MPFeederName.Format = ""
        Me.dgtc3_MPFeederName.FormatInfo = Nothing
        Me.dgtc3_MPFeederName.HeaderText = "فيدر فشار متوسط"
        Me.dgtc3_MPFeederName.MappingName = "MPFeederName"
        Me.dgtc3_MPFeederName.NullText = "-"
        Me.dgtc3_MPFeederName.Width = 190
        '
        'dgtc3_IsActive
        '
        Me.dgtc3_IsActive.HeaderText = "فعال"
        Me.dgtc3_IsActive.MappingName = "IsActive"
        Me.dgtc3_IsActive.ReadOnly = True
        Me.dgtc3_IsActive.Width = 35
        '
        'dgtc3_HavaeiLength
        '
        Me.dgtc3_HavaeiLength.Format = ""
        Me.dgtc3_HavaeiLength.FormatInfo = Nothing
        Me.dgtc3_HavaeiLength.HeaderText = "طول هوايي"
        Me.dgtc3_HavaeiLength.MappingName = "HavaeiLength"
        Me.dgtc3_HavaeiLength.NullText = "-"
        Me.dgtc3_HavaeiLength.Width = 75
        '
        'dgtc3_ZeminiLength
        '
        Me.dgtc3_ZeminiLength.Format = ""
        Me.dgtc3_ZeminiLength.FormatInfo = Nothing
        Me.dgtc3_ZeminiLength.HeaderText = "طول زميني"
        Me.dgtc3_ZeminiLength.MappingName = "ZeminiLength"
        Me.dgtc3_ZeminiLength.NullText = "-"
        Me.dgtc3_ZeminiLength.Width = 75
        '
        'dgtc3_Ownership
        '
        Me.dgtc3_Ownership.Format = ""
        Me.dgtc3_Ownership.FormatInfo = Nothing
        Me.dgtc3_Ownership.HeaderText = "مالکيت"
        Me.dgtc3_Ownership.MappingName = "Ownership"
        Me.dgtc3_Ownership.NullText = "-"
        Me.dgtc3_Ownership.ReadOnly = True
        Me.dgtc3_Ownership.Width = 75
        '
        'dgtc3_MPFeederCode
        '
        Me.dgtc3_MPFeederCode.Format = ""
        Me.dgtc3_MPFeederCode.FormatInfo = Nothing
        Me.dgtc3_MPFeederCode.HeaderText = "کد فيدر"
        Me.dgtc3_MPFeederCode.MappingName = "MPFeederCode"
        Me.dgtc3_MPFeederCode.NullText = "-"
        Me.dgtc3_MPFeederCode.ReadOnly = True
        Me.dgtc3_MPFeederCode.Width = 200
        '
        'dgtc3_InstallDatePersian
        '
        Me.dgtc3_InstallDatePersian.Format = ""
        Me.dgtc3_InstallDatePersian.FormatInfo = Nothing
        Me.dgtc3_InstallDatePersian.HeaderText = "تاريخ ايجاد فيدر"
        Me.dgtc3_InstallDatePersian.MappingName = "InstallDatePersian"
        Me.dgtc3_InstallDatePersian.NullText = "-"
        Me.dgtc3_InstallDatePersian.ReadOnly = True
        Me.dgtc3_InstallDatePersian.Width = 75
        '
        'dgtc3_IsDG
        '
        Me.dgtc3_IsDG.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.dgtc3_IsDG.HeaderText = "DG"
        Me.dgtc3_IsDG.MappingName = "IsDG"
        Me.dgtc3_IsDG.NullText = "False"
        Me.dgtc3_IsDG.NullValue = False
        Me.dgtc3_IsDG.ReadOnly = True
        Me.dgtc3_IsDG.Width = 35
        '
        'dgtc3_CoverPercentage
        '
        Me.dgtc3_CoverPercentage.Format = ""
        Me.dgtc3_CoverPercentage.FormatInfo = Nothing
        Me.dgtc3_CoverPercentage.HeaderText = "درصد مالکيت"
        Me.dgtc3_CoverPercentage.MappingName = "CoverPercentage"
        Me.dgtc3_CoverPercentage.NullText = "-"
        Me.dgtc3_CoverPercentage.ReadOnly = True
        Me.dgtc3_CoverPercentage.Width = 75
        '
        'View_LPPost
        '
        Me.View_LPPost.DataGrid = Me.dg
        Me.View_LPPost.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.DataGridTextBoxColumn22, Me.DataGridTextBoxColumn23, Me.DataGridTextBoxColumn24, Me.DataGridTextBoxColumn25, Me.DataGridTextBoxColumn26, Me.DataGridTextBoxColumn27, Me.DataGridBoolColumn8, Me.dgtc_LPPostAddress, Me.dgtc_LPPostCode, Me.dgtc_LPLocalCode, Me.dgtc_BuildYear, Me.dgtcPostCapacity, Me.dgtc4_Ownership, Me.dgtcPostAreaId, Me.dgtcSection, Me.dgtcTown, Me.dgtcVillage, Me.dgtcRoosta})
        Me.View_LPPost.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.View_LPPost.MappingName = "View_LPPost"
        '
        'DataGridTextBoxColumn22
        '
        Me.DataGridTextBoxColumn22.Format = ""
        Me.DataGridTextBoxColumn22.FormatInfo = Nothing
        Me.DataGridTextBoxColumn22.HeaderText = "Id"
        Me.DataGridTextBoxColumn22.MappingName = "LPPostId"
        Me.DataGridTextBoxColumn22.NullText = "-"
        Me.DataGridTextBoxColumn22.Width = 0
        '
        'DataGridTextBoxColumn23
        '
        Me.DataGridTextBoxColumn23.Format = ""
        Me.DataGridTextBoxColumn23.FormatInfo = Nothing
        Me.DataGridTextBoxColumn23.HeaderText = "ناحيه"
        Me.DataGridTextBoxColumn23.MappingName = "Area"
        Me.DataGridTextBoxColumn23.NullText = "-"
        Me.DataGridTextBoxColumn23.Width = 95
        '
        'DataGridTextBoxColumn24
        '
        Me.DataGridTextBoxColumn24.Format = ""
        Me.DataGridTextBoxColumn24.FormatInfo = Nothing
        Me.DataGridTextBoxColumn24.HeaderText = "فيدر فشار متوسط"
        Me.DataGridTextBoxColumn24.MappingName = "MPFeederName"
        Me.DataGridTextBoxColumn24.NullText = "-"
        Me.DataGridTextBoxColumn24.Width = 90
        '
        'DataGridTextBoxColumn25
        '
        Me.DataGridTextBoxColumn25.Format = ""
        Me.DataGridTextBoxColumn25.FormatInfo = Nothing
        Me.DataGridTextBoxColumn25.HeaderText = "نام پست توزيع"
        Me.DataGridTextBoxColumn25.MappingName = "LPPostName"
        Me.DataGridTextBoxColumn25.NullText = "-"
        Me.DataGridTextBoxColumn25.Width = 140
        '
        'DataGridTextBoxColumn26
        '
        Me.DataGridTextBoxColumn26.Format = ""
        Me.DataGridTextBoxColumn26.FormatInfo = Nothing
        Me.DataGridTextBoxColumn26.HeaderText = "نوع پست توزيع"
        Me.DataGridTextBoxColumn26.MappingName = "LPPostType"
        Me.DataGridTextBoxColumn26.NullText = "-"
        Me.DataGridTextBoxColumn26.Width = 75
        '
        'DataGridTextBoxColumn27
        '
        Me.DataGridTextBoxColumn27.Format = ""
        Me.DataGridTextBoxColumn27.FormatInfo = Nothing
        Me.DataGridTextBoxColumn27.HeaderText = "بار پيك پست"
        Me.DataGridTextBoxColumn27.MappingName = "PostPeakCurrent"
        Me.DataGridTextBoxColumn27.NullText = "-"
        Me.DataGridTextBoxColumn27.Width = 60
        '
        'DataGridBoolColumn8
        '
        Me.DataGridBoolColumn8.HeaderText = "فعال"
        Me.DataGridBoolColumn8.MappingName = "IsActive"
        Me.DataGridBoolColumn8.Width = 35
        '
        'dgtc_LPPostAddress
        '
        Me.dgtc_LPPostAddress.Format = ""
        Me.dgtc_LPPostAddress.FormatInfo = Nothing
        Me.dgtc_LPPostAddress.HeaderText = "آدرس پست توزيع"
        Me.dgtc_LPPostAddress.MappingName = "Address"
        Me.dgtc_LPPostAddress.NullText = "-"
        Me.dgtc_LPPostAddress.Width = 150
        '
        'dgtc_LPPostCode
        '
        Me.dgtc_LPPostCode.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.dgtc_LPPostCode.Format = ""
        Me.dgtc_LPPostCode.FormatInfo = Nothing
        Me.dgtc_LPPostCode.HeaderText = "کد پست"
        Me.dgtc_LPPostCode.IsNumberOnly = False
        Me.dgtc_LPPostCode.IsRTL = False
        Me.dgtc_LPPostCode.MappingName = "LPPostCode"
        Me.dgtc_LPPostCode.NullText = "-"
        Me.dgtc_LPPostCode.ReadOnly = True
        Me.dgtc_LPPostCode.Width = 75
        '
        'dgtc_LPLocalCode
        '
        Me.dgtc_LPLocalCode.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.dgtc_LPLocalCode.Format = ""
        Me.dgtc_LPLocalCode.FormatInfo = Nothing
        Me.dgtc_LPLocalCode.HeaderText = "کد محلي"
        Me.dgtc_LPLocalCode.MappingName = "LocalCode"
        Me.dgtc_LPLocalCode.NullText = "-"
        Me.dgtc_LPLocalCode.ReadOnly = True
        Me.dgtc_LPLocalCode.Width = 75
        '
        'dgtc_BuildYear
        '
        Me.dgtc_BuildYear.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.dgtc_BuildYear.Format = ""
        Me.dgtc_BuildYear.FormatInfo = Nothing
        Me.dgtc_BuildYear.HeaderText = "سال احداث"
        Me.dgtc_BuildYear.MappingName = "buildYear"
        Me.dgtc_BuildYear.NullText = "-"
        Me.dgtc_BuildYear.Width = 80
        '
        'dgtcPostCapacity
        '
        Me.dgtcPostCapacity.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.dgtcPostCapacity.Format = ""
        Me.dgtcPostCapacity.FormatInfo = Nothing
        Me.dgtcPostCapacity.HeaderText = "ظرفيت پست"
        Me.dgtcPostCapacity.MappingName = "PostCapacity"
        Me.dgtcPostCapacity.Width = 75
        '
        'dgtc4_Ownership
        '
        Me.dgtc4_Ownership.Format = ""
        Me.dgtc4_Ownership.FormatInfo = Nothing
        Me.dgtc4_Ownership.HeaderText = "مالکيت"
        Me.dgtc4_Ownership.MappingName = "Ownership"
        Me.dgtc4_Ownership.NullText = "-"
        Me.dgtc4_Ownership.Width = 75
        '
        'dgtcPostAreaId
        '
        Me.dgtcPostAreaId.Format = ""
        Me.dgtcPostAreaId.FormatInfo = Nothing
        Me.dgtcPostAreaId.MappingName = "AreaId"
        Me.dgtcPostAreaId.ReadOnly = True
        Me.dgtcPostAreaId.Width = 0
        '
        'dgtcSection
        '
        Me.dgtcSection.Format = ""
        Me.dgtcSection.FormatInfo = Nothing
        Me.dgtcSection.HeaderText = "بخش"
        Me.dgtcSection.MappingName = "SectionName"
        Me.dgtcSection.NullText = "-"
        Me.dgtcSection.Width = 75
        '
        'dgtcTown
        '
        Me.dgtcTown.Format = ""
        Me.dgtcTown.FormatInfo = Nothing
        Me.dgtcTown.HeaderText = "شهر"
        Me.dgtcTown.MappingName = "TownName"
        Me.dgtcTown.NullText = "-"
        Me.dgtcTown.Width = 75
        '
        'dgtcVillage
        '
        Me.dgtcVillage.Format = ""
        Me.dgtcVillage.FormatInfo = Nothing
        Me.dgtcVillage.HeaderText = "دهستان"
        Me.dgtcVillage.MappingName = "VillageName"
        Me.dgtcVillage.NullText = "-"
        Me.dgtcVillage.Width = 75
        '
        'dgtcRoosta
        '
        Me.dgtcRoosta.Format = ""
        Me.dgtcRoosta.FormatInfo = Nothing
        Me.dgtcRoosta.HeaderText = "روستا"
        Me.dgtcRoosta.MappingName = "RoostaName"
        Me.dgtcRoosta.NullText = "-"
        Me.dgtcRoosta.Width = 75
        '
        'View_LPFeeder
        '
        Me.View_LPFeeder.DataGrid = Me.dg
        Me.View_LPFeeder.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.DataGridTextBoxColumn28, Me.DataGridTextBoxColumn76, Me.DataGridTextBoxColumn29, Me.DataGridTextBoxColumn30, Me.DataGridTextBoxColumn31, Me.DataGridTextBoxColumn32, Me.DataGridTextBoxColumn19, Me.dgtc5_LPTransName, Me.DataGridTextBoxColumn33, Me.DataGridBoolColumn9, Me.dgtc5_HavaeiLength, Me.dgtc5_ZeminiLength, Me.dgtc5_Ownership, Me.dgtc5_IsLightFeeder, Me.dgtcLPFeederAreaId})
        Me.View_LPFeeder.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.View_LPFeeder.MappingName = "View_LPFeeder"
        '
        'DataGridTextBoxColumn28
        '
        Me.DataGridTextBoxColumn28.Format = ""
        Me.DataGridTextBoxColumn28.FormatInfo = Nothing
        Me.DataGridTextBoxColumn28.HeaderText = "Id"
        Me.DataGridTextBoxColumn28.MappingName = "LPFeederId"
        Me.DataGridTextBoxColumn28.NullText = "-"
        Me.DataGridTextBoxColumn28.Width = 0
        '
        'DataGridTextBoxColumn76
        '
        Me.DataGridTextBoxColumn76.Format = ""
        Me.DataGridTextBoxColumn76.FormatInfo = Nothing
        Me.DataGridTextBoxColumn76.HeaderText = "ناحيه"
        Me.DataGridTextBoxColumn76.MappingName = "Area"
        Me.DataGridTextBoxColumn76.Width = 65
        '
        'DataGridTextBoxColumn29
        '
        Me.DataGridTextBoxColumn29.Format = ""
        Me.DataGridTextBoxColumn29.FormatInfo = Nothing
        Me.DataGridTextBoxColumn29.HeaderText = "نام پست فوق توزيع"
        Me.DataGridTextBoxColumn29.MappingName = "MPPostName"
        Me.DataGridTextBoxColumn29.NullText = "-"
        Me.DataGridTextBoxColumn29.Width = 65
        '
        'DataGridTextBoxColumn30
        '
        Me.DataGridTextBoxColumn30.Format = ""
        Me.DataGridTextBoxColumn30.FormatInfo = Nothing
        Me.DataGridTextBoxColumn30.HeaderText = "فيدر فشار متوسط"
        Me.DataGridTextBoxColumn30.MappingName = "MPFeederName"
        Me.DataGridTextBoxColumn30.NullText = "-"
        Me.DataGridTextBoxColumn30.Width = 80
        '
        'DataGridTextBoxColumn31
        '
        Me.DataGridTextBoxColumn31.Format = ""
        Me.DataGridTextBoxColumn31.FormatInfo = Nothing
        Me.DataGridTextBoxColumn31.HeaderText = "نام پست توزيع"
        Me.DataGridTextBoxColumn31.MappingName = "LPPostName"
        Me.DataGridTextBoxColumn31.NullText = "-"
        Me.DataGridTextBoxColumn31.Width = 110
        '
        'DataGridTextBoxColumn32
        '
        Me.DataGridTextBoxColumn32.Format = ""
        Me.DataGridTextBoxColumn32.FormatInfo = Nothing
        Me.DataGridTextBoxColumn32.HeaderText = "فيدر فشار ضعيف"
        Me.DataGridTextBoxColumn32.MappingName = "LPFeederName"
        Me.DataGridTextBoxColumn32.NullText = "-"
        Me.DataGridTextBoxColumn32.Width = 75
        '
        'DataGridTextBoxColumn19
        '
        Me.DataGridTextBoxColumn19.Format = ""
        Me.DataGridTextBoxColumn19.FormatInfo = Nothing
        Me.DataGridTextBoxColumn19.HeaderText = "کد فيدر فشار ضعيف"
        Me.DataGridTextBoxColumn19.MappingName = "LPFeederCode"
        Me.DataGridTextBoxColumn19.NullText = "-"
        Me.DataGridTextBoxColumn19.Width = 75
        '
        'dgtc5_LPTransName
        '
        Me.dgtc5_LPTransName.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.dgtc5_LPTransName.Format = ""
        Me.dgtc5_LPTransName.FormatInfo = Nothing
        Me.dgtc5_LPTransName.HeaderText = "ترانس"
        Me.dgtc5_LPTransName.MappingName = "TransName"
        Me.dgtc5_LPTransName.NullText = "-"
        Me.dgtc5_LPTransName.Width = 75
        '
        'DataGridTextBoxColumn33
        '
        Me.DataGridTextBoxColumn33.Format = ""
        Me.DataGridTextBoxColumn33.FormatInfo = Nothing
        Me.DataGridTextBoxColumn33.HeaderText = "بار پيك فيدر"
        Me.DataGridTextBoxColumn33.MappingName = "FeederPeakCurrent"
        Me.DataGridTextBoxColumn33.NullText = "-"
        Me.DataGridTextBoxColumn33.Width = 60
        '
        'DataGridBoolColumn9
        '
        Me.DataGridBoolColumn9.HeaderText = "فعال"
        Me.DataGridBoolColumn9.MappingName = "IsActive"
        Me.DataGridBoolColumn9.Width = 35
        '
        'dgtc5_HavaeiLength
        '
        Me.dgtc5_HavaeiLength.Format = ""
        Me.dgtc5_HavaeiLength.FormatInfo = Nothing
        Me.dgtc5_HavaeiLength.HeaderText = "طول هوايي"
        Me.dgtc5_HavaeiLength.MappingName = "HavaeiLength"
        Me.dgtc5_HavaeiLength.NullText = "-"
        Me.dgtc5_HavaeiLength.Width = 75
        '
        'dgtc5_ZeminiLength
        '
        Me.dgtc5_ZeminiLength.Format = ""
        Me.dgtc5_ZeminiLength.FormatInfo = Nothing
        Me.dgtc5_ZeminiLength.HeaderText = "طول زميني"
        Me.dgtc5_ZeminiLength.MappingName = "ZeminiLength"
        Me.dgtc5_ZeminiLength.NullText = "-"
        Me.dgtc5_ZeminiLength.Width = 75
        '
        'dgtc5_Ownership
        '
        Me.dgtc5_Ownership.Format = ""
        Me.dgtc5_Ownership.FormatInfo = Nothing
        Me.dgtc5_Ownership.HeaderText = "مالکيت"
        Me.dgtc5_Ownership.MappingName = "Ownership"
        Me.dgtc5_Ownership.NullText = "-"
        Me.dgtc5_Ownership.Width = 75
        '
        'dgtc5_IsLightFeeder
        '
        Me.dgtc5_IsLightFeeder.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.dgtc5_IsLightFeeder.HeaderText = "روشنايي معابر"
        Me.dgtc5_IsLightFeeder.MappingName = "IsLightFeeder"
        Me.dgtc5_IsLightFeeder.NullText = "--"
        Me.dgtc5_IsLightFeeder.ReadOnly = True
        Me.dgtc5_IsLightFeeder.Width = 40
        '
        'dgtcLPFeederAreaId
        '
        Me.dgtcLPFeederAreaId.Format = ""
        Me.dgtcLPFeederAreaId.FormatInfo = Nothing
        Me.dgtcLPFeederAreaId.MappingName = "AreaId"
        Me.dgtcLPFeederAreaId.ReadOnly = True
        Me.dgtcLPFeederAreaId.Width = 0
        '
        'View_LPCommonFeeder
        '
        Me.View_LPCommonFeeder.DataGrid = Me.dg
        Me.View_LPCommonFeeder.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.DataGridTextBoxColumn34, Me.DataGridTextBoxColumn36, Me.DataGridTextBoxColumn37, Me.DataGridTextBoxColumn38, Me.DataGridTextBoxColumn39, Me.DataGridTextBoxColumn35, Me.DataGridTextBoxColumn40})
        Me.View_LPCommonFeeder.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.View_LPCommonFeeder.MappingName = "View_LPCommonFeeder"
        '
        'DataGridTextBoxColumn34
        '
        Me.DataGridTextBoxColumn34.Format = ""
        Me.DataGridTextBoxColumn34.FormatInfo = Nothing
        Me.DataGridTextBoxColumn34.HeaderText = "Id"
        Me.DataGridTextBoxColumn34.MappingName = "LPCommonFeederId"
        Me.DataGridTextBoxColumn34.NullText = "-"
        Me.DataGridTextBoxColumn34.Width = 0
        '
        'DataGridTextBoxColumn36
        '
        Me.DataGridTextBoxColumn36.Format = ""
        Me.DataGridTextBoxColumn36.FormatInfo = Nothing
        Me.DataGridTextBoxColumn36.HeaderText = "پست فوق توزيع"
        Me.DataGridTextBoxColumn36.MappingName = "MPPostName"
        Me.DataGridTextBoxColumn36.NullText = "-"
        Me.DataGridTextBoxColumn36.Width = 85
        '
        'DataGridTextBoxColumn37
        '
        Me.DataGridTextBoxColumn37.Format = ""
        Me.DataGridTextBoxColumn37.FormatInfo = Nothing
        Me.DataGridTextBoxColumn37.HeaderText = "فيدر فشار متوسط"
        Me.DataGridTextBoxColumn37.MappingName = "MPFeederName"
        Me.DataGridTextBoxColumn37.NullText = "-"
        Me.DataGridTextBoxColumn37.Width = 85
        '
        'DataGridTextBoxColumn38
        '
        Me.DataGridTextBoxColumn38.Format = ""
        Me.DataGridTextBoxColumn38.FormatInfo = Nothing
        Me.DataGridTextBoxColumn38.HeaderText = "پست توزيع"
        Me.DataGridTextBoxColumn38.MappingName = "LPPostName"
        Me.DataGridTextBoxColumn38.NullText = "-"
        Me.DataGridTextBoxColumn38.Width = 85
        '
        'DataGridTextBoxColumn39
        '
        Me.DataGridTextBoxColumn39.Format = ""
        Me.DataGridTextBoxColumn39.FormatInfo = Nothing
        Me.DataGridTextBoxColumn39.HeaderText = "فيدر فشار ضعيف"
        Me.DataGridTextBoxColumn39.MappingName = "LPFeederName"
        Me.DataGridTextBoxColumn39.NullText = "-"
        Me.DataGridTextBoxColumn39.Width = 85
        '
        'DataGridTextBoxColumn35
        '
        Me.DataGridTextBoxColumn35.Format = ""
        Me.DataGridTextBoxColumn35.FormatInfo = Nothing
        Me.DataGridTextBoxColumn35.HeaderText = "ناحيه"
        Me.DataGridTextBoxColumn35.MappingName = "Area"
        Me.DataGridTextBoxColumn35.NullText = "-"
        Me.DataGridTextBoxColumn35.Width = 85
        '
        'DataGridTextBoxColumn40
        '
        Me.DataGridTextBoxColumn40.Format = ""
        Me.DataGridTextBoxColumn40.FormatInfo = Nothing
        Me.DataGridTextBoxColumn40.HeaderText = "درصد اشتراك"
        Me.DataGridTextBoxColumn40.MappingName = "AreaCoverPercentage"
        Me.DataGridTextBoxColumn40.NullText = "-"
        Me.DataGridTextBoxColumn40.Width = 70
        '
        'View_MPCommonFeeder
        '
        Me.View_MPCommonFeeder.DataGrid = Me.dg
        Me.View_MPCommonFeeder.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.DataGridTextBoxColumn41, Me.DataGridTextBoxColumn42, Me.DataGridTextBoxColumn43, Me.DataGridTextBoxColumn44, Me.DataGridTextBoxColumn45})
        Me.View_MPCommonFeeder.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.View_MPCommonFeeder.MappingName = "View_MPCommonFeeder"
        '
        'DataGridTextBoxColumn41
        '
        Me.DataGridTextBoxColumn41.Format = ""
        Me.DataGridTextBoxColumn41.FormatInfo = Nothing
        Me.DataGridTextBoxColumn41.HeaderText = "Id"
        Me.DataGridTextBoxColumn41.MappingName = "MPCommonFeederId"
        Me.DataGridTextBoxColumn41.NullText = "-"
        Me.DataGridTextBoxColumn41.Width = 0
        '
        'DataGridTextBoxColumn42
        '
        Me.DataGridTextBoxColumn42.Format = ""
        Me.DataGridTextBoxColumn42.FormatInfo = Nothing
        Me.DataGridTextBoxColumn42.HeaderText = "پست فوق توزيع"
        Me.DataGridTextBoxColumn42.MappingName = "MPPostName"
        Me.DataGridTextBoxColumn42.NullText = "-"
        Me.DataGridTextBoxColumn42.Width = 140
        '
        'DataGridTextBoxColumn43
        '
        Me.DataGridTextBoxColumn43.Format = ""
        Me.DataGridTextBoxColumn43.FormatInfo = Nothing
        Me.DataGridTextBoxColumn43.HeaderText = "فيدر فشار متوسط"
        Me.DataGridTextBoxColumn43.MappingName = "MPFeederName"
        Me.DataGridTextBoxColumn43.NullText = "-"
        Me.DataGridTextBoxColumn43.Width = 140
        '
        'DataGridTextBoxColumn44
        '
        Me.DataGridTextBoxColumn44.Format = ""
        Me.DataGridTextBoxColumn44.FormatInfo = Nothing
        Me.DataGridTextBoxColumn44.HeaderText = "ناحيه"
        Me.DataGridTextBoxColumn44.MappingName = "Area"
        Me.DataGridTextBoxColumn44.NullText = "-"
        Me.DataGridTextBoxColumn44.Width = 140
        '
        'DataGridTextBoxColumn45
        '
        Me.DataGridTextBoxColumn45.Format = ""
        Me.DataGridTextBoxColumn45.FormatInfo = Nothing
        Me.DataGridTextBoxColumn45.HeaderText = "درصد اشتراك"
        Me.DataGridTextBoxColumn45.MappingName = "AreaCoverPercentage"
        Me.DataGridTextBoxColumn45.NullText = "-"
        Me.DataGridTextBoxColumn45.Width = 75
        '
        'ViewCriticalFeeder
        '
        Me.ViewCriticalFeeder.DataGrid = Me.dg
        Me.ViewCriticalFeeder.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.DataGridTextBoxColumn46, Me.DataGridTextBoxColumn47, Me.DataGridTextBoxColumn48, Me.DataGridTextBoxColumn49, Me.DataGridTextBoxColumn50, Me.DataGridTextBoxColumn51, Me.DataGridTextBoxColumn52})
        Me.ViewCriticalFeeder.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.ViewCriticalFeeder.MappingName = "ViewCriticalFeeder"
        '
        'DataGridTextBoxColumn46
        '
        Me.DataGridTextBoxColumn46.Format = ""
        Me.DataGridTextBoxColumn46.FormatInfo = Nothing
        Me.DataGridTextBoxColumn46.HeaderText = "Id"
        Me.DataGridTextBoxColumn46.MappingName = "CriticalFeederId"
        Me.DataGridTextBoxColumn46.NullText = "-"
        Me.DataGridTextBoxColumn46.Width = 0
        '
        'DataGridTextBoxColumn47
        '
        Me.DataGridTextBoxColumn47.Format = ""
        Me.DataGridTextBoxColumn47.FormatInfo = Nothing
        Me.DataGridTextBoxColumn47.HeaderText = "پست فوق توزيع"
        Me.DataGridTextBoxColumn47.MappingName = "MPPostName"
        Me.DataGridTextBoxColumn47.NullText = "-"
        Me.DataGridTextBoxColumn47.Width = 85
        '
        'DataGridTextBoxColumn48
        '
        Me.DataGridTextBoxColumn48.Format = ""
        Me.DataGridTextBoxColumn48.FormatInfo = Nothing
        Me.DataGridTextBoxColumn48.HeaderText = "فيدر فشار متوسط"
        Me.DataGridTextBoxColumn48.MappingName = "MPFeederName"
        Me.DataGridTextBoxColumn48.NullText = "-"
        Me.DataGridTextBoxColumn48.Width = 85
        '
        'DataGridTextBoxColumn49
        '
        Me.DataGridTextBoxColumn49.Format = ""
        Me.DataGridTextBoxColumn49.FormatInfo = Nothing
        Me.DataGridTextBoxColumn49.HeaderText = "پست توزيع"
        Me.DataGridTextBoxColumn49.MappingName = "LPPostName"
        Me.DataGridTextBoxColumn49.NullText = "-"
        Me.DataGridTextBoxColumn49.Width = 85
        '
        'DataGridTextBoxColumn50
        '
        Me.DataGridTextBoxColumn50.Format = ""
        Me.DataGridTextBoxColumn50.FormatInfo = Nothing
        Me.DataGridTextBoxColumn50.HeaderText = "فيدر فشار ضعيف"
        Me.DataGridTextBoxColumn50.MappingName = "LPFeederName"
        Me.DataGridTextBoxColumn50.NullText = "-"
        Me.DataGridTextBoxColumn50.Width = 85
        '
        'DataGridTextBoxColumn51
        '
        Me.DataGridTextBoxColumn51.Format = ""
        Me.DataGridTextBoxColumn51.FormatInfo = Nothing
        Me.DataGridTextBoxColumn51.HeaderText = "تاريخ آغاز"
        Me.DataGridTextBoxColumn51.MappingName = "TemporaryCriticalStartDatePersian"
        Me.DataGridTextBoxColumn51.NullText = "-"
        Me.DataGridTextBoxColumn51.Width = 78
        '
        'DataGridTextBoxColumn52
        '
        Me.DataGridTextBoxColumn52.Format = ""
        Me.DataGridTextBoxColumn52.FormatInfo = Nothing
        Me.DataGridTextBoxColumn52.HeaderText = "تاريخ پايان"
        Me.DataGridTextBoxColumn52.MappingName = "TemporaryCriticalEndDatePersian"
        Me.DataGridTextBoxColumn52.NullText = "-"
        Me.DataGridTextBoxColumn52.Width = 77
        '
        'View_City
        '
        Me.View_City.DataGrid = Me.dg
        Me.View_City.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.DataGridTextBoxColumn55, Me.DataGridTextBoxColumn56, Me.DataGridTextBoxColumn57, Me.DataGridTextBoxColumn58})
        Me.View_City.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.View_City.MappingName = "View_City"
        '
        'DataGridTextBoxColumn55
        '
        Me.DataGridTextBoxColumn55.Format = ""
        Me.DataGridTextBoxColumn55.FormatInfo = Nothing
        Me.DataGridTextBoxColumn55.HeaderText = "Id"
        Me.DataGridTextBoxColumn55.MappingName = "CityId"
        Me.DataGridTextBoxColumn55.NullText = "-"
        Me.DataGridTextBoxColumn55.Width = 0
        '
        'DataGridTextBoxColumn56
        '
        Me.DataGridTextBoxColumn56.Format = ""
        Me.DataGridTextBoxColumn56.FormatInfo = Nothing
        Me.DataGridTextBoxColumn56.HeaderText = "استان"
        Me.DataGridTextBoxColumn56.MappingName = "Province"
        Me.DataGridTextBoxColumn56.NullText = "-"
        Me.DataGridTextBoxColumn56.ReadOnly = True
        Me.DataGridTextBoxColumn56.Width = 205
        '
        'DataGridTextBoxColumn57
        '
        Me.DataGridTextBoxColumn57.Format = ""
        Me.DataGridTextBoxColumn57.FormatInfo = Nothing
        Me.DataGridTextBoxColumn57.HeaderText = "شهرستان"
        Me.DataGridTextBoxColumn57.MappingName = "City"
        Me.DataGridTextBoxColumn57.NullText = "-"
        Me.DataGridTextBoxColumn57.Width = 220
        '
        'DataGridTextBoxColumn58
        '
        Me.DataGridTextBoxColumn58.Format = ""
        Me.DataGridTextBoxColumn58.FormatInfo = Nothing
        Me.DataGridTextBoxColumn58.HeaderText = "كد بين شهري"
        Me.DataGridTextBoxColumn58.MappingName = "PhoneCode"
        Me.DataGridTextBoxColumn58.NullText = "-"
        Me.DataGridTextBoxColumn58.Width = 70
        '
        'View_DisconnectGroupLP
        '
        Me.View_DisconnectGroupLP.DataGrid = Me.dg
        Me.View_DisconnectGroupLP.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.DataGridTextBoxColumn1, Me.DataGridTextBoxColumn2, Me.DataGridTextBoxColumn3, Me.DataGridTextBoxColumn4, Me.DataGridBoolColumn1})
        Me.View_DisconnectGroupLP.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.View_DisconnectGroupLP.MappingName = "View_DisconnectGroupLP"
        '
        'DataGridTextBoxColumn1
        '
        Me.DataGridTextBoxColumn1.Format = ""
        Me.DataGridTextBoxColumn1.FormatInfo = Nothing
        Me.DataGridTextBoxColumn1.HeaderText = "DisconnectGroupSetId"
        Me.DataGridTextBoxColumn1.MappingName = "DisconnectGroupSetId"
        Me.DataGridTextBoxColumn1.Width = 0
        '
        'DataGridTextBoxColumn2
        '
        Me.DataGridTextBoxColumn2.Format = ""
        Me.DataGridTextBoxColumn2.FormatInfo = Nothing
        Me.DataGridTextBoxColumn2.HeaderText = "DisconnectGroupId"
        Me.DataGridTextBoxColumn2.MappingName = "DisconnectGroupId"
        Me.DataGridTextBoxColumn2.Width = 0
        '
        'DataGridTextBoxColumn3
        '
        Me.DataGridTextBoxColumn3.Format = ""
        Me.DataGridTextBoxColumn3.FormatInfo = Nothing
        Me.DataGridTextBoxColumn3.HeaderText = "قسمت"
        Me.DataGridTextBoxColumn3.MappingName = "DisconnectGroup"
        Me.DataGridTextBoxColumn3.NullText = "-"
        Me.DataGridTextBoxColumn3.Width = 165
        '
        'DataGridTextBoxColumn4
        '
        Me.DataGridTextBoxColumn4.Format = ""
        Me.DataGridTextBoxColumn4.FormatInfo = Nothing
        Me.DataGridTextBoxColumn4.HeaderText = "مجموعه"
        Me.DataGridTextBoxColumn4.MappingName = "DisconnectGroupSet"
        Me.DataGridTextBoxColumn4.NullText = "-"
        Me.DataGridTextBoxColumn4.Width = 290
        '
        'DataGridBoolColumn1
        '
        Me.DataGridBoolColumn1.HeaderText = "با برنامه"
        Me.DataGridBoolColumn1.MappingName = "IsWanted"
        Me.DataGridBoolColumn1.Width = 40
        '
        'View_DisconnectGroupMP
        '
        Me.View_DisconnectGroupMP.DataGrid = Me.dg
        Me.View_DisconnectGroupMP.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.DataGridTextBoxColumn5, Me.DataGridTextBoxColumn8, Me.DataGridTextBoxColumn9, Me.DataGridTextBoxColumn53, Me.DataGridBoolColumn2, Me.DataGridTextBoxColumn96})
        Me.View_DisconnectGroupMP.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.View_DisconnectGroupMP.MappingName = "View_DisconnectGroupMP"
        '
        'DataGridTextBoxColumn5
        '
        Me.DataGridTextBoxColumn5.Format = ""
        Me.DataGridTextBoxColumn5.FormatInfo = Nothing
        Me.DataGridTextBoxColumn5.HeaderText = "DisconnectGroupSetId"
        Me.DataGridTextBoxColumn5.MappingName = "DisconnectGroupSetId"
        Me.DataGridTextBoxColumn5.Width = 0
        '
        'DataGridTextBoxColumn8
        '
        Me.DataGridTextBoxColumn8.Format = ""
        Me.DataGridTextBoxColumn8.FormatInfo = Nothing
        Me.DataGridTextBoxColumn8.HeaderText = "DisconnectGroupId"
        Me.DataGridTextBoxColumn8.MappingName = "DisconnectGroupId"
        Me.DataGridTextBoxColumn8.Width = 0
        '
        'DataGridTextBoxColumn9
        '
        Me.DataGridTextBoxColumn9.Format = ""
        Me.DataGridTextBoxColumn9.FormatInfo = Nothing
        Me.DataGridTextBoxColumn9.HeaderText = "قسمت"
        Me.DataGridTextBoxColumn9.MappingName = "DisconnectGroup"
        Me.DataGridTextBoxColumn9.NullText = "-"
        Me.DataGridTextBoxColumn9.Width = 165
        '
        'DataGridTextBoxColumn53
        '
        Me.DataGridTextBoxColumn53.Format = ""
        Me.DataGridTextBoxColumn53.FormatInfo = Nothing
        Me.DataGridTextBoxColumn53.HeaderText = "مجموعه"
        Me.DataGridTextBoxColumn53.MappingName = "DisconnectGroupSet"
        Me.DataGridTextBoxColumn53.NullText = "-"
        Me.DataGridTextBoxColumn53.Width = 290
        '
        'DataGridBoolColumn2
        '
        Me.DataGridBoolColumn2.HeaderText = "با برنامه"
        Me.DataGridBoolColumn2.MappingName = "IsWanted"
        Me.DataGridBoolColumn2.Width = 40
        '
        'DataGridTextBoxColumn96
        '
        Me.DataGridTextBoxColumn96.Format = ""
        Me.DataGridTextBoxColumn96.FormatInfo = Nothing
        Me.DataGridTextBoxColumn96.HeaderText = "گروه"
        Me.DataGridTextBoxColumn96.MappingName = "DisconnectGroupSetGroup"
        Me.DataGridTextBoxColumn96.NullText = "-"
        Me.DataGridTextBoxColumn96.Width = 150
        '
        'View_DisconnectGroupLight
        '
        Me.View_DisconnectGroupLight.DataGrid = Me.dg
        Me.View_DisconnectGroupLight.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.DataGridTextBoxColumn78, Me.DataGridTextBoxColumn79, Me.DataGridTextBoxColumn80, Me.DataGridTextBoxColumn81, Me.DataGridBoolColumn5})
        Me.View_DisconnectGroupLight.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.View_DisconnectGroupLight.MappingName = "View_DisconnectGroupLight"
        '
        'DataGridTextBoxColumn78
        '
        Me.DataGridTextBoxColumn78.Format = ""
        Me.DataGridTextBoxColumn78.FormatInfo = Nothing
        Me.DataGridTextBoxColumn78.HeaderText = "DisconnectGroupSetId"
        Me.DataGridTextBoxColumn78.MappingName = "DisconnectGroupSetId"
        Me.DataGridTextBoxColumn78.Width = 0
        '
        'DataGridTextBoxColumn79
        '
        Me.DataGridTextBoxColumn79.Format = ""
        Me.DataGridTextBoxColumn79.FormatInfo = Nothing
        Me.DataGridTextBoxColumn79.MappingName = "DisconnectGroupId"
        Me.DataGridTextBoxColumn79.Width = 0
        '
        'DataGridTextBoxColumn80
        '
        Me.DataGridTextBoxColumn80.Format = ""
        Me.DataGridTextBoxColumn80.FormatInfo = Nothing
        Me.DataGridTextBoxColumn80.HeaderText = "قسمت"
        Me.DataGridTextBoxColumn80.MappingName = "DisconnectGroup"
        Me.DataGridTextBoxColumn80.NullText = "-"
        Me.DataGridTextBoxColumn80.Width = 165
        '
        'DataGridTextBoxColumn81
        '
        Me.DataGridTextBoxColumn81.Format = ""
        Me.DataGridTextBoxColumn81.FormatInfo = Nothing
        Me.DataGridTextBoxColumn81.HeaderText = "مجموعه"
        Me.DataGridTextBoxColumn81.MappingName = "DisconnectGroupSet"
        Me.DataGridTextBoxColumn81.NullText = "-"
        Me.DataGridTextBoxColumn81.Width = 290
        '
        'DataGridBoolColumn5
        '
        Me.DataGridBoolColumn5.HeaderText = "با برنامه"
        Me.DataGridBoolColumn5.MappingName = "IsWanted"
        Me.DataGridBoolColumn5.NullValue = "False"
        Me.DataGridBoolColumn5.Width = 75
        '
        'View_Area
        '
        Me.View_Area.DataGrid = Me.dg
        Me.View_Area.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.DataGridTextBoxColumn61, Me.DataGridTextBoxColumn62, Me.DataGridTextBoxColumn63, Me.DataGridTextBoxColumn64, Me.DataGridTextBoxColumn65, Me.DataGridBoolColumn3, Me.DataGridBoolColumn4, Me.DataGridTextBoxColumn77})
        Me.View_Area.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.View_Area.MappingName = "View_Area"
        '
        'DataGridTextBoxColumn61
        '
        Me.DataGridTextBoxColumn61.Format = ""
        Me.DataGridTextBoxColumn61.FormatInfo = Nothing
        Me.DataGridTextBoxColumn61.HeaderText = "Id"
        Me.DataGridTextBoxColumn61.MappingName = "AreaId"
        Me.DataGridTextBoxColumn61.Width = 0
        '
        'DataGridTextBoxColumn62
        '
        Me.DataGridTextBoxColumn62.Format = ""
        Me.DataGridTextBoxColumn62.FormatInfo = Nothing
        Me.DataGridTextBoxColumn62.HeaderText = "استان"
        Me.DataGridTextBoxColumn62.MappingName = "Province"
        Me.DataGridTextBoxColumn62.NullText = "-"
        Me.DataGridTextBoxColumn62.Width = 75
        '
        'DataGridTextBoxColumn63
        '
        Me.DataGridTextBoxColumn63.Format = ""
        Me.DataGridTextBoxColumn63.FormatInfo = Nothing
        Me.DataGridTextBoxColumn63.HeaderText = "شهرستان"
        Me.DataGridTextBoxColumn63.MappingName = "City"
        Me.DataGridTextBoxColumn63.NullText = "-"
        Me.DataGridTextBoxColumn63.Width = 75
        '
        'DataGridTextBoxColumn64
        '
        Me.DataGridTextBoxColumn64.Format = ""
        Me.DataGridTextBoxColumn64.FormatInfo = Nothing
        Me.DataGridTextBoxColumn64.HeaderText = "ناحيه"
        Me.DataGridTextBoxColumn64.MappingName = "Area"
        Me.DataGridTextBoxColumn64.NullText = "-"
        Me.DataGridTextBoxColumn64.Width = 75
        '
        'DataGridTextBoxColumn65
        '
        Me.DataGridTextBoxColumn65.Format = ""
        Me.DataGridTextBoxColumn65.FormatInfo = Nothing
        Me.DataGridTextBoxColumn65.HeaderText = "مرکز مرتبط"
        Me.DataGridTextBoxColumn65.MappingName = "ParentArea"
        Me.DataGridTextBoxColumn65.NullText = "-"
        Me.DataGridTextBoxColumn65.Width = 75
        '
        'DataGridBoolColumn3
        '
        Me.DataGridBoolColumn3.AllowNull = False
        Me.DataGridBoolColumn3.HeaderText = "مرکز"
        Me.DataGridBoolColumn3.MappingName = "IsCenter"
        Me.DataGridBoolColumn3.NullValue = "False"
        Me.DataGridBoolColumn3.Width = 25
        '
        'DataGridBoolColumn4
        '
        Me.DataGridBoolColumn4.AllowNull = False
        Me.DataGridBoolColumn4.HeaderText = "ستاد"
        Me.DataGridBoolColumn4.MappingName = "IsSetad"
        Me.DataGridBoolColumn4.NullValue = "False"
        Me.DataGridBoolColumn4.Width = 25
        '
        'DataGridTextBoxColumn77
        '
        Me.DataGridTextBoxColumn77.Format = ""
        Me.DataGridTextBoxColumn77.FormatInfo = Nothing
        Me.DataGridTextBoxColumn77.HeaderText = "آخرين اتصال(دقيقه)"
        Me.DataGridTextBoxColumn77.MappingName = "LastUpdate"
        Me.DataGridTextBoxColumn77.NullText = "-"
        Me.DataGridTextBoxColumn77.Width = 120
        '
        'TblEkipProfile
        '
        Me.TblEkipProfile.DataGrid = Me.dg
        Me.TblEkipProfile.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.dgts_EkipProfileId, Me.dgts_EkipProfileName, Me.dgts_EkipProfileIsActive, Me.dgts_EkipISMP, Me.dgts_EkipISLP})
        Me.TblEkipProfile.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.TblEkipProfile.MappingName = "TblEkipProfile"
        '
        'dgts_EkipProfileId
        '
        Me.dgts_EkipProfileId.Format = ""
        Me.dgts_EkipProfileId.FormatInfo = Nothing
        Me.dgts_EkipProfileId.MappingName = "EkipProfileId"
        Me.dgts_EkipProfileId.Width = 0
        '
        'dgts_EkipProfileName
        '
        Me.dgts_EkipProfileName.Format = ""
        Me.dgts_EkipProfileName.FormatInfo = Nothing
        Me.dgts_EkipProfileName.HeaderText = "نام پروفايل اکيپ"
        Me.dgts_EkipProfileName.MappingName = "EkipProfileName"
        Me.dgts_EkipProfileName.NullText = "-"
        Me.dgts_EkipProfileName.Width = 400
        '
        'dgts_EkipProfileIsActive
        '
        Me.dgts_EkipProfileIsActive.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.dgts_EkipProfileIsActive.HeaderText = "فعال"
        Me.dgts_EkipProfileIsActive.MappingName = "IsActive"
        Me.dgts_EkipProfileIsActive.NullText = "-"
        Me.dgts_EkipProfileIsActive.NullValue = "False"
        Me.dgts_EkipProfileIsActive.Width = 50
        '
        'dgts_EkipISMP
        '
        Me.dgts_EkipISMP.HeaderText = "فشارمتوسط"
        Me.dgts_EkipISMP.MappingName = "IsMP"
        Me.dgts_EkipISMP.Width = 75
        '
        'dgts_EkipISLP
        '
        Me.dgts_EkipISLP.HeaderText = "فشار ضعیف"
        Me.dgts_EkipISLP.MappingName = "IsLP"
        Me.dgts_EkipISLP.Width = 75
        '
        'View_MPPostTrans
        '
        Me.View_MPPostTrans.DataGrid = Me.dg
        Me.View_MPPostTrans.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.DataGridTextBoxColumn60, Me.DataGridTextBoxColumn67, Me.DataGridTextBoxColumn66, Me.DataGridTextBoxColumn74, Me.DataGridBoolColumn13, Me.dgtc_FTTrans_SortOrder})
        Me.View_MPPostTrans.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.View_MPPostTrans.MappingName = "View_MPPostTrans"
        '
        'DataGridTextBoxColumn60
        '
        Me.DataGridTextBoxColumn60.Format = ""
        Me.DataGridTextBoxColumn60.FormatInfo = Nothing
        Me.DataGridTextBoxColumn60.MappingName = "MPPostTransId"
        Me.DataGridTextBoxColumn60.Width = 0
        '
        'DataGridTextBoxColumn67
        '
        Me.DataGridTextBoxColumn67.Format = ""
        Me.DataGridTextBoxColumn67.FormatInfo = Nothing
        Me.DataGridTextBoxColumn67.HeaderText = "نام پست فوق توزيع"
        Me.DataGridTextBoxColumn67.MappingName = "MPPostName"
        Me.DataGridTextBoxColumn67.Width = 200
        '
        'DataGridTextBoxColumn66
        '
        Me.DataGridTextBoxColumn66.Format = ""
        Me.DataGridTextBoxColumn66.FormatInfo = Nothing
        Me.DataGridTextBoxColumn66.HeaderText = "نام ترانس"
        Me.DataGridTextBoxColumn66.MappingName = "MPPostTrans"
        Me.DataGridTextBoxColumn66.Width = 90
        '
        'DataGridTextBoxColumn74
        '
        Me.DataGridTextBoxColumn74.Format = ""
        Me.DataGridTextBoxColumn74.FormatInfo = Nothing
        Me.DataGridTextBoxColumn74.HeaderText = "قدرت ترانسفورماتور (MvA)"
        Me.DataGridTextBoxColumn74.MappingName = "MPPostTransPower"
        Me.DataGridTextBoxColumn74.NullText = "-"
        Me.DataGridTextBoxColumn74.Width = 150
        '
        'DataGridBoolColumn13
        '
        Me.DataGridBoolColumn13.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.DataGridBoolColumn13.HeaderText = "فعال"
        Me.DataGridBoolColumn13.MappingName = "IsActive"
        Me.DataGridBoolColumn13.NullText = "False"
        Me.DataGridBoolColumn13.ReadOnly = True
        Me.DataGridBoolColumn13.Width = 75
        '
        'dgtc_FTTrans_SortOrder
        '
        Me.dgtc_FTTrans_SortOrder.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.dgtc_FTTrans_SortOrder.Format = ""
        Me.dgtc_FTTrans_SortOrder.FormatInfo = Nothing
        Me.dgtc_FTTrans_SortOrder.HeaderText = "ترتيب نمايش"
        Me.dgtc_FTTrans_SortOrder.MappingName = "SortOrder"
        Me.dgtc_FTTrans_SortOrder.NullText = "-"
        Me.dgtc_FTTrans_SortOrder.Width = 90
        '
        'ViewLPPostLoad
        '
        Me.ViewLPPostLoad.DataGrid = Me.dg
        Me.ViewLPPostLoad.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.DataGridTextBoxColumn68, Me.DataGridTextBoxColumn69, Me.DataGridTextBoxColumn20, Me.DataGridTextBoxColumn70})
        Me.ViewLPPostLoad.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.ViewLPPostLoad.MappingName = "ViewLPPostLoad"
        '
        'DataGridTextBoxColumn68
        '
        Me.DataGridTextBoxColumn68.Format = ""
        Me.DataGridTextBoxColumn68.FormatInfo = Nothing
        Me.DataGridTextBoxColumn68.MappingName = "LPPostLoadId"
        Me.DataGridTextBoxColumn68.Width = 0
        '
        'DataGridTextBoxColumn69
        '
        Me.DataGridTextBoxColumn69.Format = ""
        Me.DataGridTextBoxColumn69.FormatInfo = Nothing
        Me.DataGridTextBoxColumn69.HeaderText = "نام پست توزيع"
        Me.DataGridTextBoxColumn69.MappingName = "LPPostName"
        Me.DataGridTextBoxColumn69.NullText = "-"
        Me.DataGridTextBoxColumn69.Width = 250
        '
        'DataGridTextBoxColumn20
        '
        Me.DataGridTextBoxColumn20.Format = ""
        Me.DataGridTextBoxColumn20.FormatInfo = Nothing
        Me.DataGridTextBoxColumn20.HeaderText = "ترانس"
        Me.DataGridTextBoxColumn20.MappingName = "LPTransName"
        Me.DataGridTextBoxColumn20.NullText = "-"
        Me.DataGridTextBoxColumn20.Width = 150
        '
        'DataGridTextBoxColumn70
        '
        Me.DataGridTextBoxColumn70.Format = ""
        Me.DataGridTextBoxColumn70.FormatInfo = Nothing
        Me.DataGridTextBoxColumn70.HeaderText = "تاريخ بارگيري"
        Me.DataGridTextBoxColumn70.MappingName = "LoadDateTimePersian"
        Me.DataGridTextBoxColumn70.NullText = "-"
        Me.DataGridTextBoxColumn70.Width = 70
        '
        'ViewLPFeederLoad
        '
        Me.ViewLPFeederLoad.DataGrid = Me.dg
        Me.ViewLPFeederLoad.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.DataGridTextBoxColumn71, Me.DataGridTextBoxColumn72, Me.DataGridTextBoxColumn73})
        Me.ViewLPFeederLoad.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.ViewLPFeederLoad.MappingName = "ViewLPFeederLoad"
        '
        'DataGridTextBoxColumn71
        '
        Me.DataGridTextBoxColumn71.Format = ""
        Me.DataGridTextBoxColumn71.FormatInfo = Nothing
        Me.DataGridTextBoxColumn71.MappingName = "LPFeederLoadId"
        Me.DataGridTextBoxColumn71.Width = 0
        '
        'DataGridTextBoxColumn72
        '
        Me.DataGridTextBoxColumn72.Format = ""
        Me.DataGridTextBoxColumn72.FormatInfo = Nothing
        Me.DataGridTextBoxColumn72.HeaderText = "نام فيدر"
        Me.DataGridTextBoxColumn72.MappingName = "LPFeederName"
        Me.DataGridTextBoxColumn72.NullText = "-"
        Me.DataGridTextBoxColumn72.Width = 280
        '
        'DataGridTextBoxColumn73
        '
        Me.DataGridTextBoxColumn73.Format = ""
        Me.DataGridTextBoxColumn73.FormatInfo = Nothing
        Me.DataGridTextBoxColumn73.HeaderText = "تاريخ ركورد گيري"
        Me.DataGridTextBoxColumn73.MappingName = "LoadDateTimePersian"
        Me.DataGridTextBoxColumn73.NullText = "-"
        Me.DataGridTextBoxColumn73.Width = 220
        '
        'Tbl_GroupPart
        '
        Me.Tbl_GroupPart.DataGrid = Me.dg
        Me.Tbl_GroupPart.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.DataGridTextBoxColumn82, Me.DataGridTextBoxColumn83})
        Me.Tbl_GroupPart.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.Tbl_GroupPart.MappingName = "Tbl_GroupPart"
        '
        'DataGridTextBoxColumn82
        '
        Me.DataGridTextBoxColumn82.Format = ""
        Me.DataGridTextBoxColumn82.FormatInfo = Nothing
        Me.DataGridTextBoxColumn82.MappingName = "GroupPartId"
        Me.DataGridTextBoxColumn82.Width = 0
        '
        'DataGridTextBoxColumn83
        '
        Me.DataGridTextBoxColumn83.Format = ""
        Me.DataGridTextBoxColumn83.FormatInfo = Nothing
        Me.DataGridTextBoxColumn83.HeaderText = "انواع تجهيزات"
        Me.DataGridTextBoxColumn83.MappingName = "GroupPart"
        Me.DataGridTextBoxColumn83.Width = 470
        '
        'VIEW_PartType
        '
        Me.VIEW_PartType.DataGrid = Me.dg
        Me.VIEW_PartType.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.dgtc_PartTypeId, Me.dgtc_GroupPart, Me.dgtc_PartType, Me.dgtc_IsGroupPartMP, Me.dgtc_MpLpPartTypeId, Me.dgtc_PartId, Me.dgtc_PartName})
        Me.VIEW_PartType.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.VIEW_PartType.MappingName = "VIEW_PartType"
        '
        'dgtc_PartTypeId
        '
        Me.dgtc_PartTypeId.Format = ""
        Me.dgtc_PartTypeId.FormatInfo = Nothing
        Me.dgtc_PartTypeId.MappingName = "PartTypeId"
        Me.dgtc_PartTypeId.Width = 0
        '
        'dgtc_GroupPart
        '
        Me.dgtc_GroupPart.Format = ""
        Me.dgtc_GroupPart.FormatInfo = Nothing
        Me.dgtc_GroupPart.HeaderText = "نام گروه قطعات"
        Me.dgtc_GroupPart.MappingName = "GroupPart"
        Me.dgtc_GroupPart.Width = 235
        '
        'dgtc_PartType
        '
        Me.dgtc_PartType.Format = ""
        Me.dgtc_PartType.FormatInfo = Nothing
        Me.dgtc_PartType.HeaderText = "نوع قطعه"
        Me.dgtc_PartType.MappingName = "PartType"
        Me.dgtc_PartType.Width = 235
        '
        'dgtc_IsGroupPartMP
        '
        Me.dgtc_IsGroupPartMP.Format = ""
        Me.dgtc_IsGroupPartMP.FormatInfo = Nothing
        Me.dgtc_IsGroupPartMP.MappingName = "IsGroupPartMP"
        Me.dgtc_IsGroupPartMP.Width = 0
        '
        'dgtc_MpLpPartTypeId
        '
        Me.dgtc_MpLpPartTypeId.Format = ""
        Me.dgtc_MpLpPartTypeId.FormatInfo = Nothing
        Me.dgtc_MpLpPartTypeId.MappingName = "MpLpPartTypeId"
        Me.dgtc_MpLpPartTypeId.NullText = "-1"
        Me.dgtc_MpLpPartTypeId.Width = 0
        '
        'dgtc_PartId
        '
        Me.dgtc_PartId.Format = ""
        Me.dgtc_PartId.FormatInfo = Nothing
        Me.dgtc_PartId.MappingName = "PartId"
        Me.dgtc_PartId.NullText = "-1"
        Me.dgtc_PartId.Width = 0
        '
        'dgtc_PartName
        '
        Me.dgtc_PartName.Format = ""
        Me.dgtc_PartName.FormatInfo = Nothing
        Me.dgtc_PartName.HeaderText = "نام قطعه"
        Me.dgtc_PartName.MappingName = "PartName"
        Me.dgtc_PartName.NullText = "-"
        Me.dgtc_PartName.Width = 150
        '
        'VIEW_MPPartType
        '
        Me.VIEW_MPPartType.DataGrid = Me.dg
        Me.VIEW_MPPartType.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.DataGridTextBoxColumn88, Me.DataGridTextBoxColumn89, Me.DataGridTextBoxColumn90, Me.DataGridTextBoxColumn91})
        Me.VIEW_MPPartType.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.VIEW_MPPartType.MappingName = "VIEW_MPPartType"
        '
        'DataGridTextBoxColumn88
        '
        Me.DataGridTextBoxColumn88.Format = ""
        Me.DataGridTextBoxColumn88.FormatInfo = Nothing
        Me.DataGridTextBoxColumn88.MappingName = "MPPartTypeId"
        Me.DataGridTextBoxColumn88.Width = 0
        '
        'DataGridTextBoxColumn89
        '
        Me.DataGridTextBoxColumn89.Format = ""
        Me.DataGridTextBoxColumn89.FormatInfo = Nothing
        Me.DataGridTextBoxColumn89.HeaderText = "گروه قطعات"
        Me.DataGridTextBoxColumn89.MappingName = "GroupPart"
        Me.DataGridTextBoxColumn89.Width = 235
        '
        'DataGridTextBoxColumn90
        '
        Me.DataGridTextBoxColumn90.Format = ""
        Me.DataGridTextBoxColumn90.FormatInfo = Nothing
        Me.DataGridTextBoxColumn90.HeaderText = "نوع قطعه"
        Me.DataGridTextBoxColumn90.MappingName = "PartType"
        Me.DataGridTextBoxColumn90.Width = 85
        '
        'DataGridTextBoxColumn91
        '
        Me.DataGridTextBoxColumn91.Format = ""
        Me.DataGridTextBoxColumn91.FormatInfo = Nothing
        Me.DataGridTextBoxColumn91.HeaderText = "نام قطعه"
        Me.DataGridTextBoxColumn91.MappingName = "MPPart"
        Me.DataGridTextBoxColumn91.Width = 150
        '
        'VIEW_LPPartType
        '
        Me.VIEW_LPPartType.DataGrid = Me.dg
        Me.VIEW_LPPartType.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.DataGridTextBoxColumn92, Me.DataGridTextBoxColumn93, Me.DataGridTextBoxColumn94, Me.DataGridTextBoxColumn95})
        Me.VIEW_LPPartType.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.VIEW_LPPartType.MappingName = "VIEW_LPPartType"
        '
        'DataGridTextBoxColumn92
        '
        Me.DataGridTextBoxColumn92.Format = ""
        Me.DataGridTextBoxColumn92.FormatInfo = Nothing
        Me.DataGridTextBoxColumn92.MappingName = "LPPartTypeId"
        Me.DataGridTextBoxColumn92.Width = 0
        '
        'DataGridTextBoxColumn93
        '
        Me.DataGridTextBoxColumn93.Format = ""
        Me.DataGridTextBoxColumn93.FormatInfo = Nothing
        Me.DataGridTextBoxColumn93.HeaderText = "گروه قطعات"
        Me.DataGridTextBoxColumn93.MappingName = "GroupPart"
        Me.DataGridTextBoxColumn93.Width = 235
        '
        'DataGridTextBoxColumn94
        '
        Me.DataGridTextBoxColumn94.Format = ""
        Me.DataGridTextBoxColumn94.FormatInfo = Nothing
        Me.DataGridTextBoxColumn94.HeaderText = "نوع قطعه"
        Me.DataGridTextBoxColumn94.MappingName = "PartType"
        Me.DataGridTextBoxColumn94.Width = 85
        '
        'DataGridTextBoxColumn95
        '
        Me.DataGridTextBoxColumn95.Format = ""
        Me.DataGridTextBoxColumn95.FormatInfo = Nothing
        Me.DataGridTextBoxColumn95.HeaderText = "نام قطعه"
        Me.DataGridTextBoxColumn95.MappingName = "LPPart"
        Me.DataGridTextBoxColumn95.Width = 150
        '
        'Tbl_Zone
        '
        Me.Tbl_Zone.DataGrid = Me.dg
        Me.Tbl_Zone.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.dgtcZoneId, Me.dgtcAreaId, Me.dgtcArea, Me.dgtcZoneName, Me.dgtc_VoiceCode})
        Me.Tbl_Zone.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.Tbl_Zone.MappingName = "Tbl_Zone"
        '
        'dgtcZoneId
        '
        Me.dgtcZoneId.Format = ""
        Me.dgtcZoneId.FormatInfo = Nothing
        Me.dgtcZoneId.MappingName = "ZoneId"
        Me.dgtcZoneId.NullText = "-"
        Me.dgtcZoneId.Width = 0
        '
        'dgtcAreaId
        '
        Me.dgtcAreaId.Format = ""
        Me.dgtcAreaId.FormatInfo = Nothing
        Me.dgtcAreaId.MappingName = "AreaId"
        Me.dgtcAreaId.NullText = "-"
        Me.dgtcAreaId.Width = 0
        '
        'dgtcArea
        '
        Me.dgtcArea.Format = ""
        Me.dgtcArea.FormatInfo = Nothing
        Me.dgtcArea.HeaderText = "ناحيه"
        Me.dgtcArea.MappingName = "Area"
        Me.dgtcArea.Width = 150
        '
        'dgtcZoneName
        '
        Me.dgtcZoneName.Format = ""
        Me.dgtcZoneName.FormatInfo = Nothing
        Me.dgtcZoneName.HeaderText = "منطقه"
        Me.dgtcZoneName.MappingName = "ZoneName"
        Me.dgtcZoneName.NullText = "-"
        Me.dgtcZoneName.Width = 150
        '
        'dgtc_VoiceCode
        '
        Me.dgtc_VoiceCode.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.dgtc_VoiceCode.Format = ""
        Me.dgtc_VoiceCode.FormatInfo = Nothing
        Me.dgtc_VoiceCode.HeaderText = "کد IVR"
        Me.dgtc_VoiceCode.MappingName = "VoiceCode"
        Me.dgtc_VoiceCode.NullText = "-"
        Me.dgtc_VoiceCode.Width = 101
        '
        'View_BazdidCheckList
        '
        Me.View_BazdidCheckList.DataGrid = Me.dg
        Me.View_BazdidCheckList.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.dgtc_BazdidCheckListId, Me.dgtc_CheckListGroupId, Me.dgtc_BazdidTypeId, Me.dgtc_BazdidSubGroupId, Me.dgtc_CheckListLPPostDetailId, Me.dgtc_BazdidCheckListGroup, Me.dgtc_SubGroupName, Me.dgtc_CheckListCode, Me.dgtc_CheckListName, Me.dgtc_CheckListLPPostDetailName, Me.dgtc_CheckListDesc, Me.dgtc_BazdidDisconnectType, Me.dgtc_ServiceDisconnectType, Me.dgtc_BazdidType, Me.dgtc_IsActive, Me.dgtc_BazdidPrice, Me.dgtc_ServicePrice, Me.dgtc_Etebar, Me.dgtc_FillNumberName})
        Me.View_BazdidCheckList.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.View_BazdidCheckList.MappingName = "View_BazdidCheckList"
        '
        'dgtc_BazdidCheckListId
        '
        Me.dgtc_BazdidCheckListId.Format = ""
        Me.dgtc_BazdidCheckListId.FormatInfo = Nothing
        Me.dgtc_BazdidCheckListId.MappingName = "BazdidCheckListId"
        Me.dgtc_BazdidCheckListId.Width = 0
        '
        'dgtc_CheckListGroupId
        '
        Me.dgtc_CheckListGroupId.Format = ""
        Me.dgtc_CheckListGroupId.FormatInfo = Nothing
        Me.dgtc_CheckListGroupId.MappingName = "BazdidCheckListGroupId"
        Me.dgtc_CheckListGroupId.Width = 0
        '
        'dgtc_BazdidTypeId
        '
        Me.dgtc_BazdidTypeId.Format = ""
        Me.dgtc_BazdidTypeId.FormatInfo = Nothing
        Me.dgtc_BazdidTypeId.MappingName = "BazdidTypeId"
        Me.dgtc_BazdidTypeId.Width = 0
        '
        'dgtc_BazdidSubGroupId
        '
        Me.dgtc_BazdidSubGroupId.Format = ""
        Me.dgtc_BazdidSubGroupId.FormatInfo = Nothing
        Me.dgtc_BazdidSubGroupId.MappingName = "BazdidCheckListSubGroupId"
        Me.dgtc_BazdidSubGroupId.Width = 0
        '
        'dgtc_CheckListLPPostDetailId
        '
        Me.dgtc_CheckListLPPostDetailId.Format = ""
        Me.dgtc_CheckListLPPostDetailId.FormatInfo = Nothing
        Me.dgtc_CheckListLPPostDetailId.MappingName = "LPPostDetailId"
        Me.dgtc_CheckListLPPostDetailId.Width = 0
        '
        'dgtc_BazdidCheckListGroup
        '
        Me.dgtc_BazdidCheckListGroup.Format = ""
        Me.dgtc_BazdidCheckListGroup.FormatInfo = Nothing
        Me.dgtc_BazdidCheckListGroup.HeaderText = "گروه چک ليست"
        Me.dgtc_BazdidCheckListGroup.MappingName = "BazdidCheckListGroupName"
        Me.dgtc_BazdidCheckListGroup.Width = 150
        '
        'dgtc_SubGroupName
        '
        Me.dgtc_SubGroupName.Format = ""
        Me.dgtc_SubGroupName.FormatInfo = Nothing
        Me.dgtc_SubGroupName.HeaderText = "زير گروه چک ليست"
        Me.dgtc_SubGroupName.MappingName = "BazdidCheckListSubGroupName"
        Me.dgtc_SubGroupName.NullText = "-"
        Me.dgtc_SubGroupName.Width = 150
        '
        'dgtc_CheckListCode
        '
        Me.dgtc_CheckListCode.Format = ""
        Me.dgtc_CheckListCode.FormatInfo = Nothing
        Me.dgtc_CheckListCode.HeaderText = "کد چک ليست"
        Me.dgtc_CheckListCode.MappingName = "CheckListCode"
        Me.dgtc_CheckListCode.NullText = "-"
        Me.dgtc_CheckListCode.Width = 75
        '
        'dgtc_CheckListName
        '
        Me.dgtc_CheckListName.Format = ""
        Me.dgtc_CheckListName.FormatInfo = Nothing
        Me.dgtc_CheckListName.HeaderText = "چک ليست"
        Me.dgtc_CheckListName.MappingName = "CheckListName"
        Me.dgtc_CheckListName.Width = 150
        '
        'dgtc_CheckListLPPostDetailName
        '
        Me.dgtc_CheckListLPPostDetailName.Format = ""
        Me.dgtc_CheckListLPPostDetailName.FormatInfo = Nothing
        Me.dgtc_CheckListLPPostDetailName.HeaderText = "بخش پست توزيع"
        Me.dgtc_CheckListLPPostDetailName.MappingName = "LPPostDetailName"
        Me.dgtc_CheckListLPPostDetailName.NullText = "-"
        Me.dgtc_CheckListLPPostDetailName.Width = 150
        '
        'dgtc_CheckListDesc
        '
        Me.dgtc_CheckListDesc.Format = ""
        Me.dgtc_CheckListDesc.FormatInfo = Nothing
        Me.dgtc_CheckListDesc.HeaderText = "توضيحات تکميلي"
        Me.dgtc_CheckListDesc.MappingName = "CheckListDesc"
        Me.dgtc_CheckListDesc.NullText = "-"
        Me.dgtc_CheckListDesc.Width = 300
        '
        'dgtc_BazdidDisconnectType
        '
        Me.dgtc_BazdidDisconnectType.Format = ""
        Me.dgtc_BazdidDisconnectType.FormatInfo = Nothing
        Me.dgtc_BazdidDisconnectType.HeaderText = "نوع قطع بازديد"
        Me.dgtc_BazdidDisconnectType.MappingName = "BazdidDisconnectTypeName"
        Me.dgtc_BazdidDisconnectType.NullText = "ندارد"
        Me.dgtc_BazdidDisconnectType.Width = 75
        '
        'dgtc_ServiceDisconnectType
        '
        Me.dgtc_ServiceDisconnectType.Format = ""
        Me.dgtc_ServiceDisconnectType.FormatInfo = Nothing
        Me.dgtc_ServiceDisconnectType.HeaderText = "نوع قطع سرويس"
        Me.dgtc_ServiceDisconnectType.MappingName = "ServiceDisconnectTypeName"
        Me.dgtc_ServiceDisconnectType.NullText = "ندارد"
        Me.dgtc_ServiceDisconnectType.Width = 75
        '
        'dgtc_BazdidType
        '
        Me.dgtc_BazdidType.Format = ""
        Me.dgtc_BazdidType.FormatInfo = Nothing
        Me.dgtc_BazdidType.HeaderText = "نوع بازديد"
        Me.dgtc_BazdidType.MappingName = "BazdidTypeName"
        Me.dgtc_BazdidType.Width = 75
        '
        'dgtc_IsActive
        '
        Me.dgtc_IsActive.HeaderText = "فعال"
        Me.dgtc_IsActive.MappingName = "IsActive"
        Me.dgtc_IsActive.Width = 30
        '
        'dgtc_BazdidPrice
        '
        Me.dgtc_BazdidPrice.Format = ""
        Me.dgtc_BazdidPrice.FormatInfo = Nothing
        Me.dgtc_BazdidPrice.HeaderText = "تعرفه بازديد"
        Me.dgtc_BazdidPrice.MappingName = "BazdidPrice"
        Me.dgtc_BazdidPrice.NullText = "0"
        Me.dgtc_BazdidPrice.Width = 75
        '
        'dgtc_ServicePrice
        '
        Me.dgtc_ServicePrice.Format = ""
        Me.dgtc_ServicePrice.FormatInfo = Nothing
        Me.dgtc_ServicePrice.HeaderText = "تعرفه سرويس"
        Me.dgtc_ServicePrice.MappingName = "ServicePrice"
        Me.dgtc_ServicePrice.NullText = "0"
        Me.dgtc_ServicePrice.Width = 75
        '
        'dgtc_Etebar
        '
        Me.dgtc_Etebar.Format = ""
        Me.dgtc_Etebar.FormatInfo = Nothing
        Me.dgtc_Etebar.HeaderText = "محل اعتبار سرويس"
        Me.dgtc_Etebar.MappingName = "EtebarName"
        Me.dgtc_Etebar.Width = 75
        '
        'dgtc_FillNumberName
        '
        Me.dgtc_FillNumberName.Format = ""
        Me.dgtc_FillNumberName.FormatInfo = Nothing
        Me.dgtc_FillNumberName.HeaderText = "عدد خاص"
        Me.dgtc_FillNumberName.MappingName = "FillNumberName"
        Me.dgtc_FillNumberName.NullText = "-"
        Me.dgtc_FillNumberName.Width = 75
        '
        'BTbl_BazdidCheckListGroup
        '
        Me.BTbl_BazdidCheckListGroup.DataGrid = Me.dg
        Me.BTbl_BazdidCheckListGroup.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.dgtc_BazdidCheckListGroupId, Me.dgtc_BazdidCheckListGroupName, Me.dgtc_GroupBazdidType, Me.dgic_BazdidCheckListIcon, Me.dgic_CheckListCount})
        Me.BTbl_BazdidCheckListGroup.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.BTbl_BazdidCheckListGroup.MappingName = "BTbl_BazdidCheckListGroup"
        '
        'dgtc_BazdidCheckListGroupId
        '
        Me.dgtc_BazdidCheckListGroupId.Format = ""
        Me.dgtc_BazdidCheckListGroupId.FormatInfo = Nothing
        Me.dgtc_BazdidCheckListGroupId.MappingName = "BazdidCheckListGroupId"
        Me.dgtc_BazdidCheckListGroupId.Width = 0
        '
        'dgtc_BazdidCheckListGroupName
        '
        Me.dgtc_BazdidCheckListGroupName.Format = ""
        Me.dgtc_BazdidCheckListGroupName.FormatInfo = Nothing
        Me.dgtc_BazdidCheckListGroupName.HeaderText = "نام گروه"
        Me.dgtc_BazdidCheckListGroupName.MappingName = "BazdidCheckListGroupName"
        Me.dgtc_BazdidCheckListGroupName.NullText = "-"
        Me.dgtc_BazdidCheckListGroupName.Width = 150
        '
        'dgtc_GroupBazdidType
        '
        Me.dgtc_GroupBazdidType.Format = ""
        Me.dgtc_GroupBazdidType.FormatInfo = Nothing
        Me.dgtc_GroupBazdidType.HeaderText = "نوع بازديد"
        Me.dgtc_GroupBazdidType.MappingName = "BazdidTypeName"
        Me.dgtc_GroupBazdidType.NullText = "-"
        Me.dgtc_GroupBazdidType.Width = 150
        '
        'dgic_BazdidCheckListIcon
        '
        Me.dgic_BazdidCheckListIcon.Format = ""
        Me.dgic_BazdidCheckListIcon.FormatInfo = Nothing
        Me.dgic_BazdidCheckListIcon.HeaderText = "آيکون"
        Me.dgic_BazdidCheckListIcon.MappingName = "BazdidCheckListIcon"
        Me.dgic_BazdidCheckListIcon.Width = 40
        '
        'dgic_CheckListCount
        '
        Me.dgic_CheckListCount.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.dgic_CheckListCount.Format = ""
        Me.dgic_CheckListCount.FormatInfo = Nothing
        Me.dgic_CheckListCount.HeaderText = "تعداد چک‌ليست"
        Me.dgic_CheckListCount.MappingName = "CheckListCount"
        Me.dgic_CheckListCount.ReadOnly = True
        Me.dgic_CheckListCount.Width = 80
        '
        'BTbl_SubCheckList
        '
        Me.BTbl_SubCheckList.DataGrid = Me.dg
        Me.BTbl_SubCheckList.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.dgtc_SubCheckListId, Me.dgtc_BazdidSubCheckListGroupName, Me.dgtc_OwnerCheckListName, Me.dgtc_SubCheckListCode, Me.dgtc_SubCheckListName, Me.dgtc_CheckListId, Me.dgtc_ErjaPriority})
        Me.BTbl_SubCheckList.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.BTbl_SubCheckList.MappingName = "BTbl_SubCheckList"
        '
        'dgtc_SubCheckListId
        '
        Me.dgtc_SubCheckListId.Format = ""
        Me.dgtc_SubCheckListId.FormatInfo = Nothing
        Me.dgtc_SubCheckListId.MappingName = "SubCheckListId"
        Me.dgtc_SubCheckListId.Width = 0
        '
        'dgtc_BazdidSubCheckListGroupName
        '
        Me.dgtc_BazdidSubCheckListGroupName.Format = ""
        Me.dgtc_BazdidSubCheckListGroupName.FormatInfo = Nothing
        Me.dgtc_BazdidSubCheckListGroupName.HeaderText = "نام گروه"
        Me.dgtc_BazdidSubCheckListGroupName.MappingName = "BazdidCheckListGroupName"
        Me.dgtc_BazdidSubCheckListGroupName.NullText = "-"
        Me.dgtc_BazdidSubCheckListGroupName.Width = 150
        '
        'dgtc_OwnerCheckListName
        '
        Me.dgtc_OwnerCheckListName.Format = ""
        Me.dgtc_OwnerCheckListName.FormatInfo = Nothing
        Me.dgtc_OwnerCheckListName.HeaderText = "نام چک ليست"
        Me.dgtc_OwnerCheckListName.MappingName = "CheckListName"
        Me.dgtc_OwnerCheckListName.NullText = "-"
        Me.dgtc_OwnerCheckListName.Width = 150
        '
        'dgtc_SubCheckListCode
        '
        Me.dgtc_SubCheckListCode.Format = ""
        Me.dgtc_SubCheckListCode.FormatInfo = Nothing
        Me.dgtc_SubCheckListCode.HeaderText = "کد ريز خرابي"
        Me.dgtc_SubCheckListCode.MappingName = "SubCheckListCode"
        Me.dgtc_SubCheckListCode.NullText = "-"
        Me.dgtc_SubCheckListCode.Width = 75
        '
        'dgtc_SubCheckListName
        '
        Me.dgtc_SubCheckListName.Format = ""
        Me.dgtc_SubCheckListName.FormatInfo = Nothing
        Me.dgtc_SubCheckListName.HeaderText = "ريز خرابي"
        Me.dgtc_SubCheckListName.MappingName = "SubCheckListName"
        Me.dgtc_SubCheckListName.NullText = "-"
        Me.dgtc_SubCheckListName.Width = 150
        '
        'dgtc_CheckListId
        '
        Me.dgtc_CheckListId.Format = ""
        Me.dgtc_CheckListId.FormatInfo = Nothing
        Me.dgtc_CheckListId.MappingName = "BazdidCheckListId"
        Me.dgtc_CheckListId.Width = 0
        '
        'dgtc_ErjaPriority
        '
        Me.dgtc_ErjaPriority.Format = ""
        Me.dgtc_ErjaPriority.FormatInfo = Nothing
        Me.dgtc_ErjaPriority.HeaderText = "اولویت ارجاع به پیمانکار"
        Me.dgtc_ErjaPriority.MappingName = "ErjaPriority"
        Me.dgtc_ErjaPriority.NullText = "-"
        Me.dgtc_ErjaPriority.ReadOnly = True
        Me.dgtc_ErjaPriority.Width = 50
        '
        'BTbl_ServicePart
        '
        Me.BTbl_ServicePart.DataGrid = Me.dg
        Me.BTbl_ServicePart.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.dgtc_ServicePartId, Me.dgtc_ServicePartCode, Me.dgtc_ServicePartName, Me.dgtc_PartUnit, Me.dgtc_BazdidTypeNamePart, Me.dgtc_PriceOne, Me.dgtcSP_ServicePrice})
        Me.BTbl_ServicePart.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.BTbl_ServicePart.MappingName = "BTbl_ServicePart"
        '
        'dgtc_ServicePartId
        '
        Me.dgtc_ServicePartId.Format = ""
        Me.dgtc_ServicePartId.FormatInfo = Nothing
        Me.dgtc_ServicePartId.MappingName = "ServicePartId"
        Me.dgtc_ServicePartId.Width = 0
        '
        'dgtc_ServicePartCode
        '
        Me.dgtc_ServicePartCode.Format = ""
        Me.dgtc_ServicePartCode.FormatInfo = Nothing
        Me.dgtc_ServicePartCode.HeaderText = "کد قطعه"
        Me.dgtc_ServicePartCode.MappingName = "ServicePartCode"
        Me.dgtc_ServicePartCode.NullText = "-"
        Me.dgtc_ServicePartCode.Width = 50
        '
        'dgtc_ServicePartName
        '
        Me.dgtc_ServicePartName.Format = ""
        Me.dgtc_ServicePartName.FormatInfo = Nothing
        Me.dgtc_ServicePartName.HeaderText = "نام قطعه"
        Me.dgtc_ServicePartName.MappingName = "ServicePartName"
        Me.dgtc_ServicePartName.NullText = "-"
        Me.dgtc_ServicePartName.Width = 150
        '
        'dgtc_PartUnit
        '
        Me.dgtc_PartUnit.Format = ""
        Me.dgtc_PartUnit.FormatInfo = Nothing
        Me.dgtc_PartUnit.HeaderText = "واحد"
        Me.dgtc_PartUnit.MappingName = "PartUnit"
        Me.dgtc_PartUnit.NullText = "-"
        Me.dgtc_PartUnit.Width = 80
        '
        'dgtc_BazdidTypeNamePart
        '
        Me.dgtc_BazdidTypeNamePart.Format = ""
        Me.dgtc_BazdidTypeNamePart.FormatInfo = Nothing
        Me.dgtc_BazdidTypeNamePart.HeaderText = "نوع بازديد"
        Me.dgtc_BazdidTypeNamePart.MappingName = "BazdidTypeName"
        Me.dgtc_BazdidTypeNamePart.NullText = "عمومي"
        Me.dgtc_BazdidTypeNamePart.Width = 101
        '
        'dgtc_PriceOne
        '
        Me.dgtc_PriceOne.Format = ""
        Me.dgtc_PriceOne.FormatInfo = Nothing
        Me.dgtc_PriceOne.HeaderText = "قيمت واحد"
        Me.dgtc_PriceOne.MappingName = "PriceOne"
        Me.dgtc_PriceOne.NullText = "0"
        Me.dgtc_PriceOne.Width = 75
        '
        'dgtcSP_ServicePrice
        '
        Me.dgtcSP_ServicePrice.Format = ""
        Me.dgtcSP_ServicePrice.FormatInfo = Nothing
        Me.dgtcSP_ServicePrice.HeaderText = "هزينه سرويس"
        Me.dgtcSP_ServicePrice.MappingName = "ServicePrice"
        Me.dgtcSP_ServicePrice.NullText = "0"
        Me.dgtcSP_ServicePrice.Width = 75
        '
        'BTbl_BazdidMaster
        '
        Me.BTbl_BazdidMaster.DataGrid = Me.dg
        Me.BTbl_BazdidMaster.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.dgtc_BazdidMasterId, Me.dgtc_Name, Me.dgtc_Address, Me.dgtc_Tel, Me.dgtc_Mobile, Me.dgtc_bazdidMemberIsActive})
        Me.BTbl_BazdidMaster.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.BTbl_BazdidMaster.MappingName = "BTbl_BazdidMaster"
        '
        'dgtc_BazdidMasterId
        '
        Me.dgtc_BazdidMasterId.Format = ""
        Me.dgtc_BazdidMasterId.FormatInfo = Nothing
        Me.dgtc_BazdidMasterId.MappingName = "BazdidMasterId"
        Me.dgtc_BazdidMasterId.NullText = "-"
        Me.dgtc_BazdidMasterId.Width = 0
        '
        'dgtc_Name
        '
        Me.dgtc_Name.Format = ""
        Me.dgtc_Name.FormatInfo = Nothing
        Me.dgtc_Name.HeaderText = "مشخصات"
        Me.dgtc_Name.MappingName = "Name"
        Me.dgtc_Name.NullText = "-"
        Me.dgtc_Name.Width = 145
        '
        'dgtc_Address
        '
        Me.dgtc_Address.Format = ""
        Me.dgtc_Address.FormatInfo = Nothing
        Me.dgtc_Address.HeaderText = "آدرس"
        Me.dgtc_Address.MappingName = "Address"
        Me.dgtc_Address.NullText = "-"
        Me.dgtc_Address.Width = 150
        '
        'dgtc_Tel
        '
        Me.dgtc_Tel.Format = ""
        Me.dgtc_Tel.FormatInfo = Nothing
        Me.dgtc_Tel.HeaderText = "تلفن ثابت"
        Me.dgtc_Tel.MappingName = "Tel"
        Me.dgtc_Tel.NullText = "-"
        Me.dgtc_Tel.Width = 75
        '
        'dgtc_Mobile
        '
        Me.dgtc_Mobile.Format = ""
        Me.dgtc_Mobile.FormatInfo = Nothing
        Me.dgtc_Mobile.HeaderText = "تلفن همراه"
        Me.dgtc_Mobile.MappingName = "Mobile"
        Me.dgtc_Mobile.NullText = "-"
        Me.dgtc_Mobile.Width = 75
        '
        'dgtc_bazdidMemberIsActive
        '
        Me.dgtc_bazdidMemberIsActive.HeaderText = "فعال"
        Me.dgtc_bazdidMemberIsActive.MappingName = "IsActive"
        Me.dgtc_bazdidMemberIsActive.Width = 50
        '
        'BTblBazdidEkip
        '
        Me.BTblBazdidEkip.DataGrid = Me.dg
        Me.BTblBazdidEkip.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.dgtc_BazdidEkipId, Me.dgtc_BazdidEkipName})
        Me.BTblBazdidEkip.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.BTblBazdidEkip.MappingName = "BTblBazdidEkip"
        '
        'dgtc_BazdidEkipId
        '
        Me.dgtc_BazdidEkipId.Format = ""
        Me.dgtc_BazdidEkipId.FormatInfo = Nothing
        Me.dgtc_BazdidEkipId.MappingName = "BazdidEkipId"
        Me.dgtc_BazdidEkipId.Width = 0
        '
        'dgtc_BazdidEkipName
        '
        Me.dgtc_BazdidEkipName.Format = ""
        Me.dgtc_BazdidEkipName.FormatInfo = Nothing
        Me.dgtc_BazdidEkipName.HeaderText = "نام پروفايل اکيپ"
        Me.dgtc_BazdidEkipName.MappingName = "BazdidEkipName"
        Me.dgtc_BazdidEkipName.Width = 400
        '
        'BTbl_BazdidCheckListSubGroup
        '
        Me.BTbl_BazdidCheckListSubGroup.DataGrid = Me.dg
        Me.BTbl_BazdidCheckListSubGroup.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.dgtc_BazdidCheckListSubGroupId, Me.dgtc_BazdidCheckListSubGroup_GroupName, Me.dgtc_BazdidCheckListSubGroupName, Me.dgic_BazdidCheckListSubGroupIcon, Me.dgic2_CheckListCount})
        Me.BTbl_BazdidCheckListSubGroup.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.BTbl_BazdidCheckListSubGroup.MappingName = "BTbl_BazdidCheckListSubGroup"
        '
        'dgtc_BazdidCheckListSubGroupId
        '
        Me.dgtc_BazdidCheckListSubGroupId.Format = ""
        Me.dgtc_BazdidCheckListSubGroupId.FormatInfo = Nothing
        Me.dgtc_BazdidCheckListSubGroupId.MappingName = "BazdidCheckListSubGroupId"
        Me.dgtc_BazdidCheckListSubGroupId.Width = 0
        '
        'dgtc_BazdidCheckListSubGroup_GroupName
        '
        Me.dgtc_BazdidCheckListSubGroup_GroupName.Format = ""
        Me.dgtc_BazdidCheckListSubGroup_GroupName.FormatInfo = Nothing
        Me.dgtc_BazdidCheckListSubGroup_GroupName.HeaderText = "نام گروه چک ليست"
        Me.dgtc_BazdidCheckListSubGroup_GroupName.MappingName = "BazdidCheckListGroupName"
        Me.dgtc_BazdidCheckListSubGroup_GroupName.Width = 150
        '
        'dgtc_BazdidCheckListSubGroupName
        '
        Me.dgtc_BazdidCheckListSubGroupName.Format = ""
        Me.dgtc_BazdidCheckListSubGroupName.FormatInfo = Nothing
        Me.dgtc_BazdidCheckListSubGroupName.HeaderText = "نام زير گروه"
        Me.dgtc_BazdidCheckListSubGroupName.MappingName = "BazdidCheckListSubGroupName"
        Me.dgtc_BazdidCheckListSubGroupName.NullText = "-"
        Me.dgtc_BazdidCheckListSubGroupName.Width = 150
        '
        'dgic_BazdidCheckListSubGroupIcon
        '
        Me.dgic_BazdidCheckListSubGroupIcon.Format = ""
        Me.dgic_BazdidCheckListSubGroupIcon.FormatInfo = Nothing
        Me.dgic_BazdidCheckListSubGroupIcon.HeaderText = "آيکون"
        Me.dgic_BazdidCheckListSubGroupIcon.MappingName = "BazdidCheckListSubGroupIcon"
        Me.dgic_BazdidCheckListSubGroupIcon.Width = 40
        '
        'dgic2_CheckListCount
        '
        Me.dgic2_CheckListCount.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.dgic2_CheckListCount.Format = ""
        Me.dgic2_CheckListCount.FormatInfo = Nothing
        Me.dgic2_CheckListCount.HeaderText = "تعداد چک‌ليست"
        Me.dgic2_CheckListCount.MappingName = "CheckListCount"
        Me.dgic2_CheckListCount.ReadOnly = True
        Me.dgic2_CheckListCount.Width = 80
        '
        'BTbl_LPPostDetail
        '
        Me.BTbl_LPPostDetail.DataGrid = Me.dg
        Me.BTbl_LPPostDetail.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.dgtc_LPPostDetailId, Me.dgbc_IsUserCreated, Me.dgic_LPPostDetailIcon, Me.dgtc_LPPostDetailName, Me.dgbc_IsFix, Me.dgtc_IsHavayi})
        Me.BTbl_LPPostDetail.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.BTbl_LPPostDetail.MappingName = "BTbl_LPPostDetail"
        '
        'dgtc_LPPostDetailId
        '
        Me.dgtc_LPPostDetailId.Format = ""
        Me.dgtc_LPPostDetailId.FormatInfo = Nothing
        Me.dgtc_LPPostDetailId.MappingName = "LPPostDetailId"
        Me.dgtc_LPPostDetailId.Width = 0
        '
        'dgbc_IsUserCreated
        '
        Me.dgbc_IsUserCreated.MappingName = "IsUserCreated"
        Me.dgbc_IsUserCreated.Width = 0
        '
        'dgic_LPPostDetailIcon
        '
        Me.dgic_LPPostDetailIcon.Format = ""
        Me.dgic_LPPostDetailIcon.FormatInfo = Nothing
        Me.dgic_LPPostDetailIcon.HeaderText = "آيکون"
        Me.dgic_LPPostDetailIcon.MappingName = "LPPostDetailIcon"
        Me.dgic_LPPostDetailIcon.Width = 40
        '
        'dgtc_LPPostDetailName
        '
        Me.dgtc_LPPostDetailName.Format = ""
        Me.dgtc_LPPostDetailName.FormatInfo = Nothing
        Me.dgtc_LPPostDetailName.HeaderText = "نام بخش"
        Me.dgtc_LPPostDetailName.MappingName = "LPPostDetailName"
        Me.dgtc_LPPostDetailName.Width = 200
        '
        'dgbc_IsFix
        '
        Me.dgbc_IsFix.HeaderText = "فقط يکي"
        Me.dgbc_IsFix.MappingName = "IsFix"
        Me.dgbc_IsFix.Width = 50
        '
        'dgtc_IsHavayi
        '
        Me.dgtc_IsHavayi.Format = ""
        Me.dgtc_IsHavayi.FormatInfo = Nothing
        Me.dgtc_IsHavayi.HeaderText = "نوع پست"
        Me.dgtc_IsHavayi.MappingName = "LPPostType"
        Me.dgtc_IsHavayi.Width = 75
        '
        'Tbl_FeederPart
        '
        Me.Tbl_FeederPart.DataGrid = Me.dg
        Me.Tbl_FeederPart.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.dgtc_FeederPartId, Me.dgtc_FPAreaId, Me.dgtc_MPFeederId, Me.dgtc_FPArea, Me.dgtc_MPPostName, Me.dgtc_MPFeederName, Me.dgtc_FeederPart, Me.dgtc_FeederPartCode, Me.dgtc_FeederPartIsActive, Me.dgtc_FPHavayiLength, Me.dgtc_FPZaminiLength})
        Me.Tbl_FeederPart.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.Tbl_FeederPart.MappingName = "Tbl_FeederPart"
        '
        'dgtc_FeederPartId
        '
        Me.dgtc_FeederPartId.Format = ""
        Me.dgtc_FeederPartId.FormatInfo = Nothing
        Me.dgtc_FeederPartId.MappingName = "FeederPartId"
        Me.dgtc_FeederPartId.NullText = "-"
        Me.dgtc_FeederPartId.Width = 0
        '
        'dgtc_FPAreaId
        '
        Me.dgtc_FPAreaId.Format = ""
        Me.dgtc_FPAreaId.FormatInfo = Nothing
        Me.dgtc_FPAreaId.MappingName = "AreaId"
        Me.dgtc_FPAreaId.NullText = "-"
        Me.dgtc_FPAreaId.Width = 0
        '
        'dgtc_MPFeederId
        '
        Me.dgtc_MPFeederId.Format = ""
        Me.dgtc_MPFeederId.FormatInfo = Nothing
        Me.dgtc_MPFeederId.MappingName = "MPFeederId"
        Me.dgtc_MPFeederId.NullText = "-"
        Me.dgtc_MPFeederId.Width = 0
        '
        'dgtc_FPArea
        '
        Me.dgtc_FPArea.Format = ""
        Me.dgtc_FPArea.FormatInfo = Nothing
        Me.dgtc_FPArea.HeaderText = "ناحيه"
        Me.dgtc_FPArea.MappingName = "Area"
        Me.dgtc_FPArea.NullText = "-"
        Me.dgtc_FPArea.Width = 120
        '
        'dgtc_MPPostName
        '
        Me.dgtc_MPPostName.Format = ""
        Me.dgtc_MPPostName.FormatInfo = Nothing
        Me.dgtc_MPPostName.HeaderText = "پست فوق توزيع"
        Me.dgtc_MPPostName.MappingName = "MPPostName"
        Me.dgtc_MPPostName.NullText = "-"
        Me.dgtc_MPPostName.Width = 150
        '
        'dgtc_MPFeederName
        '
        Me.dgtc_MPFeederName.Format = ""
        Me.dgtc_MPFeederName.FormatInfo = Nothing
        Me.dgtc_MPFeederName.HeaderText = "فيدر فشار متوسط"
        Me.dgtc_MPFeederName.MappingName = "MPFeederName"
        Me.dgtc_MPFeederName.NullText = "-"
        Me.dgtc_MPFeederName.Width = 150
        '
        'dgtc_FeederPart
        '
        Me.dgtc_FeederPart.Format = ""
        Me.dgtc_FeederPart.FormatInfo = Nothing
        Me.dgtc_FeederPart.HeaderText = "نام تکه فيدر"
        Me.dgtc_FeederPart.MappingName = "FeederPart"
        Me.dgtc_FeederPart.NullText = "-"
        Me.dgtc_FeederPart.Width = 200
        '
        'dgtc_FeederPartCode
        '
        Me.dgtc_FeederPartCode.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.dgtc_FeederPartCode.Format = ""
        Me.dgtc_FeederPartCode.FormatInfo = Nothing
        Me.dgtc_FeederPartCode.HeaderText = "کد تکه فيدر"
        Me.dgtc_FeederPartCode.MappingName = "FeederPartCode"
        Me.dgtc_FeederPartCode.NullText = "-"
        Me.dgtc_FeederPartCode.Width = 80
        '
        'dgtc_FeederPartIsActive
        '
        Me.dgtc_FeederPartIsActive.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.dgtc_FeederPartIsActive.HeaderText = "فعال"
        Me.dgtc_FeederPartIsActive.MappingName = "IsActive"
        Me.dgtc_FeederPartIsActive.NullText = "False"
        Me.dgtc_FeederPartIsActive.Width = 75
        '
        'dgtc_FPHavayiLength
        '
        Me.dgtc_FPHavayiLength.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.dgtc_FPHavayiLength.Format = ""
        Me.dgtc_FPHavayiLength.FormatInfo = Nothing
        Me.dgtc_FPHavayiLength.HeaderText = "طول هوايي"
        Me.dgtc_FPHavayiLength.MappingName = "HavayiLength"
        Me.dgtc_FPHavayiLength.NullText = "-"
        Me.dgtc_FPHavayiLength.Width = 75
        '
        'dgtc_FPZaminiLength
        '
        Me.dgtc_FPZaminiLength.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.dgtc_FPZaminiLength.Format = ""
        Me.dgtc_FPZaminiLength.FormatInfo = Nothing
        Me.dgtc_FPZaminiLength.HeaderText = "طول زميني"
        Me.dgtc_FPZaminiLength.MappingName = "ZaminiLength"
        Me.dgtc_FPZaminiLength.NullText = "-"
        Me.dgtc_FPZaminiLength.Width = 75
        '
        'Tbl_ErjaReason
        '
        Me.Tbl_ErjaReason.DataGrid = Me.dg
        Me.Tbl_ErjaReason.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.dgtc_ErjaReasonId, Me.dgtc_NetworkType, Me.dgtc_ErjaReason, Me.dgtc_IsActiveErjaReason})
        Me.Tbl_ErjaReason.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.Tbl_ErjaReason.MappingName = "Tbl_ErjaReason"
        '
        'dgtc_ErjaReasonId
        '
        Me.dgtc_ErjaReasonId.Format = ""
        Me.dgtc_ErjaReasonId.FormatInfo = Nothing
        Me.dgtc_ErjaReasonId.MappingName = "ErjaReasonId"
        Me.dgtc_ErjaReasonId.Width = 0
        '
        'dgtc_NetworkType
        '
        Me.dgtc_NetworkType.Format = ""
        Me.dgtc_NetworkType.FormatInfo = Nothing
        Me.dgtc_NetworkType.HeaderText = "نوع شبکه"
        Me.dgtc_NetworkType.MappingName = "NetworkType"
        Me.dgtc_NetworkType.NullText = "-"
        Me.dgtc_NetworkType.Width = 150
        '
        'dgtc_ErjaReason
        '
        Me.dgtc_ErjaReason.Format = ""
        Me.dgtc_ErjaReason.FormatInfo = Nothing
        Me.dgtc_ErjaReason.HeaderText = "دليل ارجاع"
        Me.dgtc_ErjaReason.MappingName = "ErjaReason"
        Me.dgtc_ErjaReason.Width = 300
        '
        'dgtc_IsActiveErjaReason
        '
        Me.dgtc_IsActiveErjaReason.HeaderText = "فعال"
        Me.dgtc_IsActiveErjaReason.MappingName = "IsActive"
        Me.dgtc_IsActiveErjaReason.Width = 75
        '
        'Tbl_ErjaPart
        '
        Me.Tbl_ErjaPart.DataGrid = Me.dg
        Me.Tbl_ErjaPart.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.dgtc10_ErjaPartId, Me.dgtc10_ErjaPart, Me.dgtc10_PartUnit, Me.dgtc10_PartPrice, Me.dgtc10_NetworkType, Me.dgtc10_ErjaReason})
        Me.Tbl_ErjaPart.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.Tbl_ErjaPart.MappingName = "Tbl_ErjaPart"
        '
        'dgtc10_ErjaPartId
        '
        Me.dgtc10_ErjaPartId.Format = ""
        Me.dgtc10_ErjaPartId.FormatInfo = Nothing
        Me.dgtc10_ErjaPartId.MappingName = "ErjaPartId"
        Me.dgtc10_ErjaPartId.Width = 0
        '
        'dgtc10_ErjaPart
        '
        Me.dgtc10_ErjaPart.Format = ""
        Me.dgtc10_ErjaPart.FormatInfo = Nothing
        Me.dgtc10_ErjaPart.HeaderText = "نام قطعه"
        Me.dgtc10_ErjaPart.MappingName = "ErjaPart"
        Me.dgtc10_ErjaPart.NullText = "-"
        Me.dgtc10_ErjaPart.Width = 101
        '
        'dgtc10_PartUnit
        '
        Me.dgtc10_PartUnit.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.dgtc10_PartUnit.Format = ""
        Me.dgtc10_PartUnit.FormatInfo = Nothing
        Me.dgtc10_PartUnit.HeaderText = "واحد شمارش"
        Me.dgtc10_PartUnit.MappingName = "PartUnit"
        Me.dgtc10_PartUnit.NullText = "-"
        Me.dgtc10_PartUnit.Width = 50
        '
        'dgtc10_PartPrice
        '
        Me.dgtc10_PartPrice.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.dgtc10_PartPrice.Format = ""
        Me.dgtc10_PartPrice.FormatInfo = Nothing
        Me.dgtc10_PartPrice.HeaderText = "قيمت"
        Me.dgtc10_PartPrice.MappingName = "PartPrice"
        Me.dgtc10_PartPrice.NullText = "-"
        Me.dgtc10_PartPrice.Width = 75
        '
        'dgtc10_NetworkType
        '
        Me.dgtc10_NetworkType.Format = ""
        Me.dgtc10_NetworkType.FormatInfo = Nothing
        Me.dgtc10_NetworkType.HeaderText = "نوع شبکه"
        Me.dgtc10_NetworkType.MappingName = "NetworkType"
        Me.dgtc10_NetworkType.NullText = "-"
        Me.dgtc10_NetworkType.Width = 120
        '
        'dgtc10_ErjaReason
        '
        Me.dgtc10_ErjaReason.Format = ""
        Me.dgtc10_ErjaReason.FormatInfo = Nothing
        Me.dgtc10_ErjaReason.HeaderText = "اشکال بوجود آمده"
        Me.dgtc10_ErjaReason.MappingName = "ErjaReason"
        Me.dgtc10_ErjaReason.NullText = "-"
        Me.dgtc10_ErjaReason.Width = 120
        '
        'Tbl_ErjaOperation
        '
        Me.Tbl_ErjaOperation.DataGrid = Me.dg
        Me.Tbl_ErjaOperation.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.dgtc11_ErjaOperationId, Me.dgtc11_ErjaOperation, Me.dgtc11_OperationPrice, Me.dgtc11_NetworkType})
        Me.Tbl_ErjaOperation.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.Tbl_ErjaOperation.MappingName = "Tbl_ErjaOperation"
        '
        'dgtc11_ErjaOperationId
        '
        Me.dgtc11_ErjaOperationId.Format = ""
        Me.dgtc11_ErjaOperationId.FormatInfo = Nothing
        Me.dgtc11_ErjaOperationId.MappingName = "ErjaOperationId"
        Me.dgtc11_ErjaOperationId.Width = 0
        '
        'dgtc11_ErjaOperation
        '
        Me.dgtc11_ErjaOperation.Format = ""
        Me.dgtc11_ErjaOperation.FormatInfo = Nothing
        Me.dgtc11_ErjaOperation.HeaderText = "نام عمليات"
        Me.dgtc11_ErjaOperation.MappingName = "ErjaOperation"
        Me.dgtc11_ErjaOperation.NullText = "-"
        Me.dgtc11_ErjaOperation.Width = 150
        '
        'dgtc11_OperationPrice
        '
        Me.dgtc11_OperationPrice.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.dgtc11_OperationPrice.Format = ""
        Me.dgtc11_OperationPrice.FormatInfo = Nothing
        Me.dgtc11_OperationPrice.HeaderText = "هزينه عمليات"
        Me.dgtc11_OperationPrice.MappingName = "OperationPrice"
        Me.dgtc11_OperationPrice.NullText = "-"
        Me.dgtc11_OperationPrice.Width = 75
        '
        'dgtc11_NetworkType
        '
        Me.dgtc11_NetworkType.Format = ""
        Me.dgtc11_NetworkType.FormatInfo = Nothing
        Me.dgtc11_NetworkType.HeaderText = "نوع شبکه"
        Me.dgtc11_NetworkType.MappingName = "NetworkType"
        Me.dgtc11_NetworkType.NullText = "-"
        Me.dgtc11_NetworkType.Width = 120
        '
        'Tbl_DisconnectGroupSetGroup
        '
        Me.Tbl_DisconnectGroupSetGroup.DataGrid = Me.dg
        Me.Tbl_DisconnectGroupSetGroup.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.dgtc_DisconnectGroupSetGroupId, Me.dgtc_DisconnectGroupSetGroup, Me.dgtc_DCGSG_SortOrder})
        Me.Tbl_DisconnectGroupSetGroup.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.Tbl_DisconnectGroupSetGroup.MappingName = "Tbl_DisconnectGroupSetGroup"
        '
        'dgtc_DisconnectGroupSetGroupId
        '
        Me.dgtc_DisconnectGroupSetGroupId.Format = ""
        Me.dgtc_DisconnectGroupSetGroupId.FormatInfo = Nothing
        Me.dgtc_DisconnectGroupSetGroupId.MappingName = "DisconnectGroupSetGroupId"
        Me.dgtc_DisconnectGroupSetGroupId.Width = 0
        '
        'dgtc_DisconnectGroupSetGroup
        '
        Me.dgtc_DisconnectGroupSetGroup.Format = ""
        Me.dgtc_DisconnectGroupSetGroup.FormatInfo = Nothing
        Me.dgtc_DisconnectGroupSetGroup.HeaderText = "نام گروه"
        Me.dgtc_DisconnectGroupSetGroup.MappingName = "DisconnectGroupSetGroup"
        Me.dgtc_DisconnectGroupSetGroup.NullText = "-"
        Me.dgtc_DisconnectGroupSetGroup.Width = 250
        '
        'dgtc_DCGSG_SortOrder
        '
        Me.dgtc_DCGSG_SortOrder.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.dgtc_DCGSG_SortOrder.Format = ""
        Me.dgtc_DCGSG_SortOrder.FormatInfo = Nothing
        Me.dgtc_DCGSG_SortOrder.HeaderText = "ترتيب نمايش"
        Me.dgtc_DCGSG_SortOrder.MappingName = "SortOrder"
        Me.dgtc_DCGSG_SortOrder.NullText = "-"
        Me.dgtc_DCGSG_SortOrder.Width = 75
        '
        'Tbl_TamirOperation
        '
        Me.Tbl_TamirOperation.DataGrid = Me.dg
        Me.Tbl_TamirOperation.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.dgtc_TamirOperationId, Me.dgtc_TamirOperation, Me.dgtc_OperationDuration, Me.dgtc_OperationTamirNetworkType})
        Me.Tbl_TamirOperation.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.Tbl_TamirOperation.MappingName = "Tbl_TamirOperation"
        '
        'dgtc_TamirOperationId
        '
        Me.dgtc_TamirOperationId.Format = ""
        Me.dgtc_TamirOperationId.FormatInfo = Nothing
        Me.dgtc_TamirOperationId.MappingName = "TamirOperationId"
        Me.dgtc_TamirOperationId.NullText = "-"
        Me.dgtc_TamirOperationId.Width = 0
        '
        'dgtc_TamirOperation
        '
        Me.dgtc_TamirOperation.Format = ""
        Me.dgtc_TamirOperation.FormatInfo = Nothing
        Me.dgtc_TamirOperation.HeaderText = "عمليات با برنامه"
        Me.dgtc_TamirOperation.MappingName = "TamirOperation"
        Me.dgtc_TamirOperation.NullText = "-"
        Me.dgtc_TamirOperation.Width = 300
        '
        'dgtc_OperationDuration
        '
        Me.dgtc_OperationDuration.Format = ""
        Me.dgtc_OperationDuration.FormatInfo = Nothing
        Me.dgtc_OperationDuration.HeaderText = "مدت انجام"
        Me.dgtc_OperationDuration.MappingName = "Duration"
        Me.dgtc_OperationDuration.NullText = "0"
        Me.dgtc_OperationDuration.Width = 101
        '
        'dgtc_OperationTamirNetworkType
        '
        Me.dgtc_OperationTamirNetworkType.Format = ""
        Me.dgtc_OperationTamirNetworkType.FormatInfo = Nothing
        Me.dgtc_OperationTamirNetworkType.HeaderText = "نوع شبکه"
        Me.dgtc_OperationTamirNetworkType.MappingName = "TamirNetworkType"
        Me.dgtc_OperationTamirNetworkType.Width = 110
        '
        'Tbl_ManoeuvreType
        '
        Me.Tbl_ManoeuvreType.DataGrid = Me.dg
        Me.Tbl_ManoeuvreType.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.dgtc_ManoeuvreTypeId, Me.dgtc_ManoeuvreType})
        Me.Tbl_ManoeuvreType.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.Tbl_ManoeuvreType.MappingName = "Tbl_ManoeuvreType"
        '
        'dgtc_ManoeuvreTypeId
        '
        Me.dgtc_ManoeuvreTypeId.Format = ""
        Me.dgtc_ManoeuvreTypeId.FormatInfo = Nothing
        Me.dgtc_ManoeuvreTypeId.MappingName = "ManoeuvreTypeId"
        Me.dgtc_ManoeuvreTypeId.NullText = "-"
        Me.dgtc_ManoeuvreTypeId.Width = 0
        '
        'dgtc_ManoeuvreType
        '
        Me.dgtc_ManoeuvreType.Format = ""
        Me.dgtc_ManoeuvreType.FormatInfo = Nothing
        Me.dgtc_ManoeuvreType.HeaderText = "اجراي مانور از طريق ..."
        Me.dgtc_ManoeuvreType.MappingName = "ManoeuvreType"
        Me.dgtc_ManoeuvreType.NullText = "-"
        Me.dgtc_ManoeuvreType.Width = 400
        '
        'Tbl_ErjaCasue
        '
        Me.Tbl_ErjaCasue.DataGrid = Me.dg
        Me.Tbl_ErjaCasue.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.dgtcEC_ErjaCauseId, Me.dgtcEC_ErjaReasonId, Me.dgtcEC_ErjaReason, Me.dgtcEC_ErjaCause})
        Me.Tbl_ErjaCasue.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.Tbl_ErjaCasue.MappingName = "Tbl_ErjaCause"
        '
        'dgtcEC_ErjaCauseId
        '
        Me.dgtcEC_ErjaCauseId.Format = ""
        Me.dgtcEC_ErjaCauseId.FormatInfo = Nothing
        Me.dgtcEC_ErjaCauseId.MappingName = "ErjaCauseId"
        Me.dgtcEC_ErjaCauseId.NullText = "-"
        Me.dgtcEC_ErjaCauseId.Width = 0
        '
        'dgtcEC_ErjaReasonId
        '
        Me.dgtcEC_ErjaReasonId.Format = ""
        Me.dgtcEC_ErjaReasonId.FormatInfo = Nothing
        Me.dgtcEC_ErjaReasonId.MappingName = "ErjaReasonId"
        Me.dgtcEC_ErjaReasonId.NullText = "-"
        Me.dgtcEC_ErjaReasonId.Width = 0
        '
        'dgtcEC_ErjaReason
        '
        Me.dgtcEC_ErjaReason.Format = ""
        Me.dgtcEC_ErjaReason.FormatInfo = Nothing
        Me.dgtcEC_ErjaReason.HeaderText = "دليل ارجاع"
        Me.dgtcEC_ErjaReason.MappingName = "ErjaReason"
        Me.dgtcEC_ErjaReason.NullText = "-"
        Me.dgtcEC_ErjaReason.Width = 250
        '
        'dgtcEC_ErjaCause
        '
        Me.dgtcEC_ErjaCause.Format = ""
        Me.dgtcEC_ErjaCause.FormatInfo = Nothing
        Me.dgtcEC_ErjaCause.HeaderText = "علت اشکال بوجود آمده"
        Me.dgtcEC_ErjaCause.MappingName = "ErjaCause"
        Me.dgtcEC_ErjaCause.NullText = "-"
        Me.dgtcEC_ErjaCause.Width = 250
        '
        'Tbl_Peymankar
        '
        Me.Tbl_Peymankar.DataGrid = Me.dg
        Me.Tbl_Peymankar.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.dgtc_PeymankarId, Me.dgtc_PeymankarAreaId, Me.dgtc_PeymankarArea, Me.dgtc_PeymankarName, Me.dgtc40_Manager, Me.dgtc40_Mobile, Me.dgtc40_Address})
        Me.Tbl_Peymankar.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.Tbl_Peymankar.MappingName = "Tbl_Peymankar"
        '
        'dgtc_PeymankarId
        '
        Me.dgtc_PeymankarId.Format = ""
        Me.dgtc_PeymankarId.FormatInfo = Nothing
        Me.dgtc_PeymankarId.MappingName = "PeymankarId"
        Me.dgtc_PeymankarId.Width = 0
        '
        'dgtc_PeymankarAreaId
        '
        Me.dgtc_PeymankarAreaId.Format = ""
        Me.dgtc_PeymankarAreaId.FormatInfo = Nothing
        Me.dgtc_PeymankarAreaId.MappingName = "AreaId"
        Me.dgtc_PeymankarAreaId.Width = 0
        '
        'dgtc_PeymankarArea
        '
        Me.dgtc_PeymankarArea.Format = ""
        Me.dgtc_PeymankarArea.FormatInfo = Nothing
        Me.dgtc_PeymankarArea.HeaderText = "ناحيه مرتبط"
        Me.dgtc_PeymankarArea.MappingName = "Area"
        Me.dgtc_PeymankarArea.NullText = "عمومي"
        Me.dgtc_PeymankarArea.Width = 75
        '
        'dgtc_PeymankarName
        '
        Me.dgtc_PeymankarName.Format = ""
        Me.dgtc_PeymankarName.FormatInfo = Nothing
        Me.dgtc_PeymankarName.HeaderText = "نام پيمانکار"
        Me.dgtc_PeymankarName.MappingName = "PeymankarName"
        Me.dgtc_PeymankarName.Width = 75
        '
        'dgtc40_Manager
        '
        Me.dgtc40_Manager.Format = ""
        Me.dgtc40_Manager.FormatInfo = Nothing
        Me.dgtc40_Manager.HeaderText = "مدير عامل"
        Me.dgtc40_Manager.MappingName = "Manager"
        Me.dgtc40_Manager.NullText = "-"
        Me.dgtc40_Manager.Width = 105
        '
        'dgtc40_Mobile
        '
        Me.dgtc40_Mobile.Format = ""
        Me.dgtc40_Mobile.FormatInfo = Nothing
        Me.dgtc40_Mobile.HeaderText = "همراه"
        Me.dgtc40_Mobile.MappingName = "Mobile"
        Me.dgtc40_Mobile.NullText = "-"
        Me.dgtc40_Mobile.Width = 80
        '
        'dgtc40_Address
        '
        Me.dgtc40_Address.Format = ""
        Me.dgtc40_Address.FormatInfo = Nothing
        Me.dgtc40_Address.HeaderText = "آدرس"
        Me.dgtc40_Address.MappingName = "Address"
        Me.dgtc40_Address.NullText = "-"
        Me.dgtc40_Address.Width = 150
        '
        'ViewLPFeederPointsInfo
        '
        Me.ViewLPFeederPointsInfo.DataGrid = Me.dg
        Me.ViewLPFeederPointsInfo.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.DataGridTextBoxColumn16, Me.DataGridTextBoxColumn17, Me.DataGridTextBoxColumn18})
        Me.ViewLPFeederPointsInfo.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.ViewLPFeederPointsInfo.MappingName = "ViewLPFeederPointsInfo"
        '
        'DataGridTextBoxColumn16
        '
        Me.DataGridTextBoxColumn16.Format = ""
        Me.DataGridTextBoxColumn16.FormatInfo = Nothing
        Me.DataGridTextBoxColumn16.HeaderText = "LPFeederPointsInfoId"
        Me.DataGridTextBoxColumn16.MappingName = "LPFeederPointsInfoId"
        Me.DataGridTextBoxColumn16.Width = 0
        '
        'DataGridTextBoxColumn17
        '
        Me.DataGridTextBoxColumn17.Format = ""
        Me.DataGridTextBoxColumn17.FormatInfo = Nothing
        Me.DataGridTextBoxColumn17.HeaderText = "نام نقطه انتهايي"
        Me.DataGridTextBoxColumn17.MappingName = "LPFeederPointName"
        Me.DataGridTextBoxColumn17.Width = 280
        '
        'DataGridTextBoxColumn18
        '
        Me.DataGridTextBoxColumn18.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.DataGridTextBoxColumn18.Format = ""
        Me.DataGridTextBoxColumn18.FormatInfo = Nothing
        Me.DataGridTextBoxColumn18.HeaderText = "تاريخ ولتاژگيري"
        Me.DataGridTextBoxColumn18.MappingName = "MeasureDatePersian"
        Me.DataGridTextBoxColumn18.Width = 220
        '
        'Tbl_AVLCar
        '
        Me.Tbl_AVLCar.DataGrid = Me.dg
        Me.Tbl_AVLCar.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.AVLCarId, Me.AVLCarArea, Me.AVLCarName, Me.AVLCarCode, Me.AVLCarIsActive})
        Me.Tbl_AVLCar.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.Tbl_AVLCar.MappingName = "Tbl_AVLCar"
        '
        'AVLCarId
        '
        Me.AVLCarId.Format = ""
        Me.AVLCarId.FormatInfo = Nothing
        Me.AVLCarId.HeaderText = "AVLCarId"
        Me.AVLCarId.MappingName = "AVLCarId"
        Me.AVLCarId.ReadOnly = True
        Me.AVLCarId.Width = 0
        '
        'AVLCarArea
        '
        Me.AVLCarArea.Format = ""
        Me.AVLCarArea.FormatInfo = Nothing
        Me.AVLCarArea.HeaderText = "ناحيه"
        Me.AVLCarArea.MappingName = "Area"
        Me.AVLCarArea.ReadOnly = True
        Me.AVLCarArea.Width = 150
        '
        'AVLCarName
        '
        Me.AVLCarName.Format = ""
        Me.AVLCarName.FormatInfo = Nothing
        Me.AVLCarName.HeaderText = "نام خودرو"
        Me.AVLCarName.MappingName = "AVLCarName"
        Me.AVLCarName.ReadOnly = True
        Me.AVLCarName.Width = 150
        '
        'AVLCarCode
        '
        Me.AVLCarCode.Format = ""
        Me.AVLCarCode.FormatInfo = Nothing
        Me.AVLCarCode.HeaderText = "کد خودرو"
        Me.AVLCarCode.MappingName = "AVLCarCode"
        Me.AVLCarCode.ReadOnly = True
        Me.AVLCarCode.Width = 150
        '
        'AVLCarIsActive
        '
        Me.AVLCarIsActive.HeaderText = "فعال"
        Me.AVLCarIsActive.MappingName = "IsActive"
        Me.AVLCarIsActive.ReadOnly = True
        Me.AVLCarIsActive.Width = 70
        '
        'Tbl_MPFeederKey
        '
        Me.Tbl_MPFeederKey.DataGrid = Me.dg
        Me.Tbl_MPFeederKey.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.MPFeederKeyId, Me.KeyArea, Me.KeyMPPost, Me.KeyMPFeeder, Me.KeyName, Me.KeyCode, Me.KeyGISCode, Me.KeyIsMainKey, Me.KeyIsActive, Me.KeyIsManovr, Me.KeyManovrMPFeeder, Me.KeyMPCloserType, Me.KeyFactory, Me.KeyMPFeederId, Me.KeyManovrMPFeederId})
        Me.Tbl_MPFeederKey.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.Tbl_MPFeederKey.MappingName = "Tbl_MPFeederKey"
        '
        'MPFeederKeyId
        '
        Me.MPFeederKeyId.Format = ""
        Me.MPFeederKeyId.FormatInfo = Nothing
        Me.MPFeederKeyId.MappingName = "MPFeederKeyId"
        Me.MPFeederKeyId.Width = 0
        '
        'KeyArea
        '
        Me.KeyArea.Format = ""
        Me.KeyArea.FormatInfo = Nothing
        Me.KeyArea.HeaderText = "ناحيه"
        Me.KeyArea.MappingName = "Area"
        Me.KeyArea.NullText = "-"
        Me.KeyArea.Width = 75
        '
        'KeyMPPost
        '
        Me.KeyMPPost.Format = ""
        Me.KeyMPPost.FormatInfo = Nothing
        Me.KeyMPPost.HeaderText = "پست فوق توزيع"
        Me.KeyMPPost.MappingName = "MPPostName"
        Me.KeyMPPost.NullText = "-"
        Me.KeyMPPost.Width = 75
        '
        'KeyMPFeeder
        '
        Me.KeyMPFeeder.Format = ""
        Me.KeyMPFeeder.FormatInfo = Nothing
        Me.KeyMPFeeder.HeaderText = "فيدر فشار متوسط"
        Me.KeyMPFeeder.MappingName = "MPFeederName"
        Me.KeyMPFeeder.NullText = "-"
        Me.KeyMPFeeder.Width = 75
        '
        'KeyName
        '
        Me.KeyName.Format = ""
        Me.KeyName.FormatInfo = Nothing
        Me.KeyName.HeaderText = "نام کليد"
        Me.KeyName.MappingName = "KeyName"
        Me.KeyName.NullText = "-"
        Me.KeyName.Width = 75
        '
        'KeyCode
        '
        Me.KeyCode.Format = ""
        Me.KeyCode.FormatInfo = Nothing
        Me.KeyCode.HeaderText = "کد کليد"
        Me.KeyCode.MappingName = "KeyCode"
        Me.KeyCode.NullText = "-"
        Me.KeyCode.Width = 75
        '
        'KeyGISCode
        '
        Me.KeyGISCode.Format = ""
        Me.KeyGISCode.FormatInfo = Nothing
        Me.KeyGISCode.HeaderText = "کد GIS"
        Me.KeyGISCode.MappingName = "GISCode"
        Me.KeyGISCode.NullText = "-"
        Me.KeyGISCode.Width = 75
        '
        'KeyIsMainKey
        '
        Me.KeyIsMainKey.HeaderText = "کليد اصلي"
        Me.KeyIsMainKey.MappingName = "IsMainKey"
        Me.KeyIsMainKey.Width = 50
        '
        'KeyIsActive
        '
        Me.KeyIsActive.HeaderText = "فعال"
        Me.KeyIsActive.MappingName = "IsActive"
        Me.KeyIsActive.NullText = ""
        Me.KeyIsActive.Width = 50
        '
        'KeyIsManovr
        '
        Me.KeyIsManovr.HeaderText = "فيدر مانور دارد"
        Me.KeyIsManovr.MappingName = "IsManovr"
        Me.KeyIsManovr.NullText = "False"
        Me.KeyIsManovr.Width = 70
        '
        'KeyManovrMPFeeder
        '
        Me.KeyManovrMPFeeder.Format = ""
        Me.KeyManovrMPFeeder.FormatInfo = Nothing
        Me.KeyManovrMPFeeder.HeaderText = "فيدر مانور"
        Me.KeyManovrMPFeeder.MappingName = "ManovrMPFeederName"
        Me.KeyManovrMPFeeder.NullText = "-"
        Me.KeyManovrMPFeeder.Width = 50
        '
        'KeyMPCloserType
        '
        Me.KeyMPCloserType.Format = ""
        Me.KeyMPCloserType.FormatInfo = Nothing
        Me.KeyMPCloserType.HeaderText = "نوع کليد"
        Me.KeyMPCloserType.MappingName = "MPCloserType"
        Me.KeyMPCloserType.NullText = "-"
        Me.KeyMPCloserType.Width = 75
        '
        'KeyFactory
        '
        Me.KeyFactory.Format = ""
        Me.KeyFactory.FormatInfo = Nothing
        Me.KeyFactory.HeaderText = "سازنده"
        Me.KeyFactory.MappingName = "Factory"
        Me.KeyFactory.NullText = "-"
        Me.KeyFactory.Width = 75
        '
        'KeyMPFeederId
        '
        Me.KeyMPFeederId.Format = ""
        Me.KeyMPFeederId.FormatInfo = Nothing
        Me.KeyMPFeederId.MappingName = "MPFeederId"
        Me.KeyMPFeederId.Width = 0
        '
        'KeyManovrMPFeederId
        '
        Me.KeyManovrMPFeederId.Format = ""
        Me.KeyManovrMPFeederId.FormatInfo = Nothing
        Me.KeyManovrMPFeederId.MappingName = "ManovrMPFeederId"
        Me.KeyManovrMPFeederId.Width = 0
        '
        'Tbl_Village
        '
        Me.Tbl_Village.DataGrid = Me.dg
        Me.Tbl_Village.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.dgtc_VillageId, Me.dgtc_VillageArea, Me.Dgd_Sectionname, Me.dgdtc_TownName, Me.dgtc_VillageCode, Me.dgtc_VillageName})
        Me.Tbl_Village.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.Tbl_Village.MappingName = "Tbl_Village"
        '
        'dgtc_VillageId
        '
        Me.dgtc_VillageId.Format = ""
        Me.dgtc_VillageId.FormatInfo = Nothing
        Me.dgtc_VillageId.MappingName = "VillageId"
        Me.dgtc_VillageId.ReadOnly = True
        Me.dgtc_VillageId.Width = 0
        '
        'dgtc_VillageArea
        '
        Me.dgtc_VillageArea.Format = ""
        Me.dgtc_VillageArea.FormatInfo = Nothing
        Me.dgtc_VillageArea.HeaderText = "ناحيه"
        Me.dgtc_VillageArea.MappingName = "Area"
        Me.dgtc_VillageArea.NullText = ""
        Me.dgtc_VillageArea.Width = 75
        '
        'Dgd_Sectionname
        '
        Me.Dgd_Sectionname.Format = ""
        Me.Dgd_Sectionname.FormatInfo = Nothing
        Me.Dgd_Sectionname.HeaderText = "بخش"
        Me.Dgd_Sectionname.MappingName = "SectionName"
        Me.Dgd_Sectionname.NullText = ""
        Me.Dgd_Sectionname.ReadOnly = True
        Me.Dgd_Sectionname.Width = 75
        '
        'dgdtc_TownName
        '
        Me.dgdtc_TownName.Format = ""
        Me.dgdtc_TownName.FormatInfo = Nothing
        Me.dgdtc_TownName.HeaderText = "شهر"
        Me.dgdtc_TownName.MappingName = "TownName"
        Me.dgdtc_TownName.NullText = ""
        Me.dgdtc_TownName.ReadOnly = True
        Me.dgdtc_TownName.Width = 110
        '
        'dgtc_VillageCode
        '
        Me.dgtc_VillageCode.Format = ""
        Me.dgtc_VillageCode.FormatInfo = Nothing
        Me.dgtc_VillageCode.HeaderText = "کد دهستان"
        Me.dgtc_VillageCode.MappingName = "VillageCode"
        Me.dgtc_VillageCode.NullText = ""
        Me.dgtc_VillageCode.ReadOnly = True
        Me.dgtc_VillageCode.Width = 75
        '
        'dgtc_VillageName
        '
        Me.dgtc_VillageName.Format = ""
        Me.dgtc_VillageName.FormatInfo = Nothing
        Me.dgtc_VillageName.HeaderText = " دهستان"
        Me.dgtc_VillageName.MappingName = "VillageName"
        Me.dgtc_VillageName.ReadOnly = True
        Me.dgtc_VillageName.Width = 150
        '
        'ViewLPPostEarth
        '
        Me.ViewLPPostEarth.DataGrid = Me.dg
        Me.ViewLPPostEarth.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.DataGridTextBoxColumn97, Me.DataGridTextBoxColumn98, Me.DataGridTextBoxColumn99, Me.DataGridTextBoxColumn100})
        Me.ViewLPPostEarth.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.ViewLPPostEarth.MappingName = "ViewLPPostEarth"
        '
        'DataGridTextBoxColumn97
        '
        Me.DataGridTextBoxColumn97.Format = ""
        Me.DataGridTextBoxColumn97.FormatInfo = Nothing
        Me.DataGridTextBoxColumn97.MappingName = "LPPostEarthInfoId"
        Me.DataGridTextBoxColumn97.Width = 0
        '
        'DataGridTextBoxColumn98
        '
        Me.DataGridTextBoxColumn98.Format = ""
        Me.DataGridTextBoxColumn98.FormatInfo = Nothing
        Me.DataGridTextBoxColumn98.HeaderText = "نام پست توزيع"
        Me.DataGridTextBoxColumn98.MappingName = "LPPostName"
        Me.DataGridTextBoxColumn98.NullText = "-"
        Me.DataGridTextBoxColumn98.Width = 300
        '
        'DataGridTextBoxColumn99
        '
        Me.DataGridTextBoxColumn99.Format = ""
        Me.DataGridTextBoxColumn99.FormatInfo = Nothing
        Me.DataGridTextBoxColumn99.HeaderText = "تاريخ ارت گيري"
        Me.DataGridTextBoxColumn99.MappingName = "ReadDatePersian"
        Me.DataGridTextBoxColumn99.NullText = "-"
        Me.DataGridTextBoxColumn99.Width = 75
        '
        'DataGridTextBoxColumn100
        '
        Me.DataGridTextBoxColumn100.Format = ""
        Me.DataGridTextBoxColumn100.FormatInfo = Nothing
        Me.DataGridTextBoxColumn100.HeaderText = "ساعت ارت گيري"
        Me.DataGridTextBoxColumn100.MappingName = "ReadTime"
        Me.DataGridTextBoxColumn100.NullText = "-"
        Me.DataGridTextBoxColumn100.Width = 75
        '
        'Tbl_Section
        '
        Me.Tbl_Section.DataGrid = Me.dg
        Me.Tbl_Section.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.dgtc_SectionId, Me.dgtc_SectionCode, Me.dgtc_Area, Me.dgtc_SectionName})
        Me.Tbl_Section.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.Tbl_Section.MappingName = "Tbl_Section"
        '
        'dgtc_SectionId
        '
        Me.dgtc_SectionId.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.dgtc_SectionId.Format = ""
        Me.dgtc_SectionId.FormatInfo = Nothing
        Me.dgtc_SectionId.HeaderText = "شناسه"
        Me.dgtc_SectionId.MappingName = "SectionId"
        Me.dgtc_SectionId.NullText = ""
        Me.dgtc_SectionId.Width = 0
        '
        'dgtc_SectionCode
        '
        Me.dgtc_SectionCode.Format = ""
        Me.dgtc_SectionCode.FormatInfo = Nothing
        Me.dgtc_SectionCode.HeaderText = "کد بخش"
        Me.dgtc_SectionCode.MappingName = "SectionCode"
        Me.dgtc_SectionCode.NullText = ""
        Me.dgtc_SectionCode.Width = 50
        '
        'dgtc_Area
        '
        Me.dgtc_Area.Format = ""
        Me.dgtc_Area.FormatInfo = Nothing
        Me.dgtc_Area.HeaderText = "نام ناحيه"
        Me.dgtc_Area.MappingName = "Area"
        Me.dgtc_Area.NullText = ""
        Me.dgtc_Area.Width = 110
        '
        'dgtc_SectionName
        '
        Me.dgtc_SectionName.Format = ""
        Me.dgtc_SectionName.FormatInfo = Nothing
        Me.dgtc_SectionName.HeaderText = "نام بخش"
        Me.dgtc_SectionName.MappingName = "SectionName"
        Me.dgtc_SectionName.NullText = ""
        Me.dgtc_SectionName.Width = 150
        '
        'Tbl_Town
        '
        Me.Tbl_Town.DataGrid = Me.dg
        Me.Tbl_Town.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.TownId, Me.Area, Me.SectionName, Me.TownCode, Me.TownName, Me.SectionId})
        Me.Tbl_Town.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.Tbl_Town.MappingName = "Tbl_Town"
        '
        'TownId
        '
        Me.TownId.Format = ""
        Me.TownId.FormatInfo = Nothing
        Me.TownId.MappingName = "TownId"
        Me.TownId.Width = 0
        '
        'Area
        '
        Me.Area.Format = ""
        Me.Area.FormatInfo = Nothing
        Me.Area.HeaderText = "نام ناحيه"
        Me.Area.MappingName = "Area"
        Me.Area.Width = 75
        '
        'SectionName
        '
        Me.SectionName.Format = ""
        Me.SectionName.FormatInfo = Nothing
        Me.SectionName.HeaderText = "نام بخش"
        Me.SectionName.MappingName = "SectionName"
        Me.SectionName.Width = 75
        '
        'TownCode
        '
        Me.TownCode.Format = ""
        Me.TownCode.FormatInfo = Nothing
        Me.TownCode.HeaderText = "کد شهر"
        Me.TownCode.MappingName = "TownCode"
        Me.TownCode.Width = 75
        '
        'TownName
        '
        Me.TownName.Format = ""
        Me.TownName.FormatInfo = Nothing
        Me.TownName.HeaderText = "نام شهر"
        Me.TownName.MappingName = "TownName"
        Me.TownName.Width = 110
        '
        'SectionId
        '
        Me.SectionId.Format = ""
        Me.SectionId.FormatInfo = Nothing
        Me.SectionId.MappingName = "SectionId"
        Me.SectionId.Width = 0
        '
        'Tbl_Roosta
        '
        Me.Tbl_Roosta.DataGrid = Me.dg
        Me.Tbl_Roosta.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.RoostaId, Me.Roosta_Aria, Me.Roosta_SectionName, Me.Roosta_TownName, Me.Roosta_VillageName, Me.RoostaCode, Me.RoostaName, Me.VillageId})
        Me.Tbl_Roosta.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.Tbl_Roosta.MappingName = "Tbl_Roosta"
        '
        'RoostaId
        '
        Me.RoostaId.Format = ""
        Me.RoostaId.FormatInfo = Nothing
        Me.RoostaId.MappingName = "RoostaId"
        Me.RoostaId.Width = 0
        '
        'Roosta_Aria
        '
        Me.Roosta_Aria.Format = ""
        Me.Roosta_Aria.FormatInfo = Nothing
        Me.Roosta_Aria.HeaderText = "نام ناحيه"
        Me.Roosta_Aria.MappingName = "Area"
        Me.Roosta_Aria.Width = 75
        '
        'Roosta_SectionName
        '
        Me.Roosta_SectionName.Format = ""
        Me.Roosta_SectionName.FormatInfo = Nothing
        Me.Roosta_SectionName.HeaderText = "نام بخش"
        Me.Roosta_SectionName.MappingName = "SectionName"
        Me.Roosta_SectionName.Width = 75
        '
        'Roosta_TownName
        '
        Me.Roosta_TownName.Format = ""
        Me.Roosta_TownName.FormatInfo = Nothing
        Me.Roosta_TownName.HeaderText = "نام شهر"
        Me.Roosta_TownName.MappingName = "TownName"
        Me.Roosta_TownName.Width = 75
        '
        'Roosta_VillageName
        '
        Me.Roosta_VillageName.Format = ""
        Me.Roosta_VillageName.FormatInfo = Nothing
        Me.Roosta_VillageName.HeaderText = "نام دهستان"
        Me.Roosta_VillageName.MappingName = "VillageName"
        Me.Roosta_VillageName.Width = 75
        '
        'RoostaCode
        '
        Me.RoostaCode.Format = ""
        Me.RoostaCode.FormatInfo = Nothing
        Me.RoostaCode.HeaderText = "کد روستا"
        Me.RoostaCode.MappingName = "RoostaCode"
        Me.RoostaCode.Width = 75
        '
        'RoostaName
        '
        Me.RoostaName.Format = ""
        Me.RoostaName.FormatInfo = Nothing
        Me.RoostaName.HeaderText = "نام روستا"
        Me.RoostaName.MappingName = "RoostaName"
        Me.RoostaName.Width = 110
        '
        'VillageId
        '
        Me.VillageId.Format = ""
        Me.VillageId.FormatInfo = Nothing
        Me.VillageId.MappingName = "VillageId"
        Me.VillageId.Width = 0
        '
        'ViewLPPostPointEarth
        '
        Me.ViewLPPostPointEarth.DataGrid = Me.dg
        Me.ViewLPPostPointEarth.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.LPPostEarthInfoId, Me.LPPostEarthId, Me.LPPostName, Me.LPPostEarthName, Me.ReadDatePersian, Me.ReadTime, Me.LPPostId})
        Me.ViewLPPostPointEarth.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.ViewLPPostPointEarth.MappingName = "ViewLPPostPointEarth"
        '
        'LPPostEarthInfoId
        '
        Me.LPPostEarthInfoId.Format = ""
        Me.LPPostEarthInfoId.FormatInfo = Nothing
        Me.LPPostEarthInfoId.MappingName = "LPPostEarthInfoId"
        Me.LPPostEarthInfoId.Width = 0
        '
        'LPPostEarthId
        '
        Me.LPPostEarthId.Format = ""
        Me.LPPostEarthId.FormatInfo = Nothing
        Me.LPPostEarthId.MappingName = "LPPostEarthId"
        Me.LPPostEarthId.Width = 0
        '
        'LPPostName
        '
        Me.LPPostName.Format = ""
        Me.LPPostName.FormatInfo = Nothing
        Me.LPPostName.HeaderText = "نام پست"
        Me.LPPostName.MappingName = "LPPostName"
        Me.LPPostName.Width = 180
        '
        'LPPostEarthName
        '
        Me.LPPostEarthName.Format = ""
        Me.LPPostEarthName.FormatInfo = Nothing
        Me.LPPostEarthName.HeaderText = "نام نقطه ارت"
        Me.LPPostEarthName.MappingName = "LPPostEarthName"
        Me.LPPostEarthName.Width = 150
        '
        'ReadDatePersian
        '
        Me.ReadDatePersian.Format = ""
        Me.ReadDatePersian.FormatInfo = Nothing
        Me.ReadDatePersian.HeaderText = "تاريخ ارت گيري"
        Me.ReadDatePersian.MappingName = "ReadDatePersian"
        Me.ReadDatePersian.Width = 120
        '
        'ReadTime
        '
        Me.ReadTime.Format = ""
        Me.ReadTime.FormatInfo = Nothing
        Me.ReadTime.HeaderText = "ساعت ارت گيري"
        Me.ReadTime.MappingName = "ReadTime"
        Me.ReadTime.Width = 75
        '
        'LPPostId
        '
        Me.LPPostId.Format = ""
        Me.LPPostId.FormatInfo = Nothing
        Me.LPPostId.MappingName = "LPPostId"
        Me.LPPostId.Width = 0
        '
        'Tbl_AllowValidator
        '
        Me.Tbl_AllowValidator.DataGrid = Me.dg
        Me.Tbl_AllowValidator.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.DataGridTextBoxColumn21, Me.DataGridTextBoxColumn75, Me.DataGridTextBoxColumn54, Me.DataGridTextBoxColumn59, Me.DataGridBoolColumn10, Me.DataGridBoolColumn11, Me.DataGridBoolColumn12})
        Me.Tbl_AllowValidator.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.Tbl_AllowValidator.MappingName = "Tbl_AllowValidator"
        '
        'DataGridTextBoxColumn21
        '
        Me.DataGridTextBoxColumn21.Format = ""
        Me.DataGridTextBoxColumn21.FormatInfo = Nothing
        Me.DataGridTextBoxColumn21.MappingName = "AllowValidatorId"
        Me.DataGridTextBoxColumn21.Width = 0
        '
        'DataGridTextBoxColumn75
        '
        Me.DataGridTextBoxColumn75.Format = ""
        Me.DataGridTextBoxColumn75.FormatInfo = Nothing
        Me.DataGridTextBoxColumn75.HeaderText = "ناحيه"
        Me.DataGridTextBoxColumn75.MappingName = "Area"
        Me.DataGridTextBoxColumn75.Width = 75
        '
        'DataGridTextBoxColumn54
        '
        Me.DataGridTextBoxColumn54.Format = ""
        Me.DataGridTextBoxColumn54.FormatInfo = Nothing
        Me.DataGridTextBoxColumn54.HeaderText = "نام و نام خانوادگي"
        Me.DataGridTextBoxColumn54.MappingName = "AllowValidatorName"
        Me.DataGridTextBoxColumn54.Width = 120
        '
        'DataGridTextBoxColumn59
        '
        Me.DataGridTextBoxColumn59.Format = ""
        Me.DataGridTextBoxColumn59.FormatInfo = Nothing
        Me.DataGridTextBoxColumn59.MappingName = "AreaId"
        Me.DataGridTextBoxColumn59.Width = 0
        '
        'DataGridBoolColumn10
        '
        Me.DataGridBoolColumn10.HeaderText = "فعال"
        Me.DataGridBoolColumn10.MappingName = "IsActive"
        Me.DataGridBoolColumn10.NullText = ""
        Me.DataGridBoolColumn10.Width = 75
        '
        'DataGridBoolColumn11
        '
        Me.DataGridBoolColumn11.HeaderText = "صادر کننده"
        Me.DataGridBoolColumn11.MappingName = "IsValidator"
        Me.DataGridBoolColumn11.Width = 75
        '
        'DataGridBoolColumn12
        '
        Me.DataGridBoolColumn12.HeaderText = "ابطال کننده"
        Me.DataGridBoolColumn12.MappingName = "IsInvalidator"
        Me.DataGridBoolColumn12.Width = 75
        '
        'ViewMPFeederPointsInfo
        '
        Me.ViewMPFeederPointsInfo.DataGrid = Me.dg
        Me.ViewMPFeederPointsInfo.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.MPFeederPointsInfoId, Me.MPFeederPointName, Me.MeasureDatePersian})
        Me.ViewMPFeederPointsInfo.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.ViewMPFeederPointsInfo.MappingName = "ViewMPFeederPointsInfo"
        '
        'MPFeederPointsInfoId
        '
        Me.MPFeederPointsInfoId.Format = ""
        Me.MPFeederPointsInfoId.FormatInfo = Nothing
        Me.MPFeederPointsInfoId.MappingName = "MPFeederPointsInfoId"
        Me.MPFeederPointsInfoId.Width = 0
        '
        'MPFeederPointName
        '
        Me.MPFeederPointName.Format = ""
        Me.MPFeederPointName.FormatInfo = Nothing
        Me.MPFeederPointName.HeaderText = "نام نقطه انتهایی "
        Me.MPFeederPointName.MappingName = "MPFeederPointName"
        Me.MPFeederPointName.Width = 280
        '
        'MeasureDatePersian
        '
        Me.MeasureDatePersian.Format = ""
        Me.MeasureDatePersian.FormatInfo = Nothing
        Me.MeasureDatePersian.HeaderText = "تاریخ بارگیری"
        Me.MeasureDatePersian.MappingName = "MeasureDatePersian"
        Me.MeasureDatePersian.Width = 220
        '
        'Tbl_IVRCode
        '
        Me.Tbl_IVRCode.DataGrid = Me.dg
        Me.Tbl_IVRCode.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.dgtc_IVRCodeId, Me.dgtc_IVRCode, Me.dgtc_IVRDescription})
        Me.Tbl_IVRCode.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.Tbl_IVRCode.MappingName = "Tbl_IVRCode"
        '
        'dgtc_IVRCodeId
        '
        Me.dgtc_IVRCodeId.Format = ""
        Me.dgtc_IVRCodeId.FormatInfo = Nothing
        Me.dgtc_IVRCodeId.MappingName = "IVRCodeId"
        Me.dgtc_IVRCodeId.Width = 0
        '
        'dgtc_IVRCode
        '
        Me.dgtc_IVRCode.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.dgtc_IVRCode.Format = ""
        Me.dgtc_IVRCode.FormatInfo = Nothing
        Me.dgtc_IVRCode.HeaderText = "کد IVR"
        Me.dgtc_IVRCode.MappingName = "IVRCode"
        Me.dgtc_IVRCode.NullText = "-"
        Me.dgtc_IVRCode.Width = 101
        '
        'dgtc_IVRDescription
        '
        Me.dgtc_IVRDescription.Format = ""
        Me.dgtc_IVRDescription.FormatInfo = Nothing
        Me.dgtc_IVRDescription.HeaderText = "شرح کد"
        Me.dgtc_IVRDescription.MappingName = "Description"
        Me.dgtc_IVRDescription.NullText = "-"
        Me.dgtc_IVRDescription.Width = 250
        '
        'TblRecloserFunction
        '
        Me.TblRecloserFunction.DataGrid = Me.dg
        Me.TblRecloserFunction.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.dgtcRecloserFunctionId, Me.dgtcKeyName, Me.dgtcMPFeederName, Me.dgtcReadDate, Me.dgtcReadTime})
        Me.TblRecloserFunction.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.TblRecloserFunction.MappingName = "TblRecloserFunction"
        '
        'dgtcKeyName
        '
        Me.dgtcKeyName.Format = ""
        Me.dgtcKeyName.FormatInfo = Nothing
        Me.dgtcKeyName.HeaderText = "کلوزر"
        Me.dgtcKeyName.MappingName = "KeyName"
        Me.dgtcKeyName.Width = 150
        '
        'dgtcMPFeederName
        '
        Me.dgtcMPFeederName.Format = ""
        Me.dgtcMPFeederName.FormatInfo = Nothing
        Me.dgtcMPFeederName.HeaderText = "نام فيدر فشار متوسط"
        Me.dgtcMPFeederName.MappingName = "MPFeederName"
        Me.dgtcMPFeederName.Width = 150
        '
        'dgtcReadDate
        '
        Me.dgtcReadDate.Format = ""
        Me.dgtcReadDate.FormatInfo = Nothing
        Me.dgtcReadDate.HeaderText = "تاريخ قرائت "
        Me.dgtcReadDate.MappingName = "ReadDatePersian"
        Me.dgtcReadDate.Width = 75
        '
        'dgtc_TajhizatId
        '
        Me.dgtc_TajhizatId.Format = ""
        Me.dgtc_TajhizatId.FormatInfo = Nothing
        Me.dgtc_TajhizatId.MappingName = "TajhizatId"
        Me.dgtc_TajhizatId.Width = 0
        '
        'dgtc_TajhizatName
        '
        Me.dgtc_TajhizatName.Format = ""
        Me.dgtc_TajhizatName.FormatInfo = Nothing
        Me.dgtc_TajhizatName.HeaderText = "نام تجهيز"
        Me.dgtc_TajhizatName.MappingName = "TajhizatName"
        Me.dgtc_TajhizatName.NullText = "-"
        Me.dgtc_TajhizatName.Width = 150
        '
        'dgtc_BazdidTypeName
        '
        Me.dgtc_BazdidTypeName.Format = ""
        Me.dgtc_BazdidTypeName.FormatInfo = Nothing
        Me.dgtc_BazdidTypeName.HeaderText = "نوع بازديد"
        Me.dgtc_BazdidTypeName.MappingName = "BazdidTypeName"
        Me.dgtc_BazdidTypeName.NullText = "عمومي"
        Me.dgtc_BazdidTypeName.Width = 75
        '
        'DatasetCcReqView1
        '
        Me.DatasetCcReqView1.DataSetName = "DatasetCcRequesterView"
        Me.DatasetCcReqView1.Locale = New System.Globalization.CultureInfo("en-US")
        Me.DatasetCcReqView1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'BttnSelect
        '
        Me.BttnSelect.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BttnSelect.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.BttnSelect.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.BttnSelect.Location = New System.Drawing.Point(641, 502)
        Me.BttnSelect.Name = "BttnSelect"
        Me.BttnSelect.Size = New System.Drawing.Size(75, 23)
        Me.BttnSelect.TabIndex = 7
        Me.BttnSelect.Text = "&انتخاب"
        '
        'bttn_MPFeederList
        '
        Me.bttn_MPFeederList.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bttn_MPFeederList.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.bttn_MPFeederList.Location = New System.Drawing.Point(419, 465)
        Me.bttn_MPFeederList.Name = "bttn_MPFeederList"
        Me.bttn_MPFeederList.Size = New System.Drawing.Size(128, 23)
        Me.bttn_MPFeederList.TabIndex = 4
        Me.bttn_MPFeederList.Text = "فيدرهاي فشار متوسط"
        Me.bttn_MPFeederList.Visible = False
        '
        'bttn_LPPost
        '
        Me.bttn_LPPost.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bttn_LPPost.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.bttn_LPPost.Location = New System.Drawing.Point(434, 465)
        Me.bttn_LPPost.Name = "bttn_LPPost"
        Me.bttn_LPPost.Size = New System.Drawing.Size(112, 23)
        Me.bttn_LPPost.TabIndex = 3
        Me.bttn_LPPost.Text = "پست هاي توزيع"
        Me.bttn_LPPost.Visible = False
        '
        'bttn_LPFeeder
        '
        Me.bttn_LPFeeder.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bttn_LPFeeder.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.bttn_LPFeeder.Location = New System.Drawing.Point(419, 465)
        Me.bttn_LPFeeder.Name = "bttn_LPFeeder"
        Me.bttn_LPFeeder.Size = New System.Drawing.Size(128, 23)
        Me.bttn_LPFeeder.TabIndex = 3
        Me.bttn_LPFeeder.Text = "فيدرهاي فشار ضعيف"
        Me.bttn_LPFeeder.Visible = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Location = New System.Drawing.Point(9, 493)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(707, 3)
        Me.GroupBox1.TabIndex = 9
        Me.GroupBox1.TabStop = False
        '
        'Label5
        '
        Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoSize = True
        Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label5.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label5.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label5.Location = New System.Drawing.Point(511, 504)
        Me.Label5.Name = "Label5"
        Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label5.Size = New System.Drawing.Size(55, 18)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "ناحيه مرتبط"
        Me.Label5.Visible = False
        '
        'cboArea
        '
        Me.cboArea.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboArea.BackColor = System.Drawing.Color.White
        Me.cboArea.DataSource = Me.DatasetCcReq1.Tbl_Area
        Me.cboArea.DisplayMember = "Area"
        Me.cboArea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboArea.IsReadOnly = False
        Me.cboArea.Location = New System.Drawing.Point(319, 504)
        Me.cboArea.Name = "cboArea"
        Me.cboArea.Size = New System.Drawing.Size(185, 21)
        Me.cboArea.TabIndex = 10
        Me.cboArea.ValueMember = "AreaId"
        Me.cboArea.Visible = False
        '
        'Bttn_DisconnectReason
        '
        Me.Bttn_DisconnectReason.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Bttn_DisconnectReason.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Bttn_DisconnectReason.Location = New System.Drawing.Point(402, 465)
        Me.Bttn_DisconnectReason.Name = "Bttn_DisconnectReason"
        Me.Bttn_DisconnectReason.Size = New System.Drawing.Size(128, 23)
        Me.Bttn_DisconnectReason.TabIndex = 4
        Me.Bttn_DisconnectReason.Text = "&اشکالات بوجود آمده"
        Me.Bttn_DisconnectReason.Visible = False
        '
        'Bttn_DisconnectReasonXPReason
        '
        Me.Bttn_DisconnectReasonXPReason.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Bttn_DisconnectReasonXPReason.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Bttn_DisconnectReasonXPReason.Location = New System.Drawing.Point(267, 465)
        Me.Bttn_DisconnectReasonXPReason.Name = "Bttn_DisconnectReasonXPReason"
        Me.Bttn_DisconnectReasonXPReason.Size = New System.Drawing.Size(128, 23)
        Me.Bttn_DisconnectReasonXPReason.TabIndex = 5
        Me.Bttn_DisconnectReasonXPReason.Text = "&علت اشکال بوجود آمده"
        Me.Bttn_DisconnectReasonXPReason.Visible = False
        '
        'Bttn_MPPostTrans
        '
        Me.Bttn_MPPostTrans.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Bttn_MPPostTrans.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Bttn_MPPostTrans.Location = New System.Drawing.Point(507, 503)
        Me.Bttn_MPPostTrans.Name = "Bttn_MPPostTrans"
        Me.Bttn_MPPostTrans.Size = New System.Drawing.Size(128, 23)
        Me.Bttn_MPPostTrans.TabIndex = 12
        Me.Bttn_MPPostTrans.Text = "فهرست ترانسها"
        Me.Bttn_MPPostTrans.Visible = False
        '
        'bttn_LPPostLoad
        '
        Me.bttn_LPPostLoad.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bttn_LPPostLoad.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.bttn_LPPostLoad.Location = New System.Drawing.Point(402, 503)
        Me.bttn_LPPostLoad.Name = "bttn_LPPostLoad"
        Me.bttn_LPPostLoad.Size = New System.Drawing.Size(128, 23)
        Me.bttn_LPPostLoad.TabIndex = 13
        Me.bttn_LPPostLoad.Text = "بارگيري پست توزيع"
        Me.bttn_LPPostLoad.Visible = False
        '
        'bttn_LPFeederLoad
        '
        Me.bttn_LPFeederLoad.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bttn_LPFeederLoad.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.bttn_LPFeederLoad.Location = New System.Drawing.Point(491, 503)
        Me.bttn_LPFeederLoad.Name = "bttn_LPFeederLoad"
        Me.bttn_LPFeederLoad.Size = New System.Drawing.Size(144, 23)
        Me.bttn_LPFeederLoad.TabIndex = 9
        Me.bttn_LPFeederLoad.Text = "بارگيري فيدر فشار ضعيف"
        Me.bttn_LPFeederLoad.Visible = False
        '
        'BttnPartType
        '
        Me.BttnPartType.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BttnPartType.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.BttnPartType.Location = New System.Drawing.Point(259, 503)
        Me.BttnPartType.Name = "BttnPartType"
        Me.BttnPartType.Size = New System.Drawing.Size(144, 23)
        Me.BttnPartType.TabIndex = 15
        Me.BttnPartType.Text = "فهرست نوع اجناس سرقتي"
        Me.BttnPartType.Visible = False
        '
        'BttnMPLPPart
        '
        Me.BttnMPLPPart.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BttnMPLPPart.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.BttnMPLPPart.Location = New System.Drawing.Point(259, 503)
        Me.BttnMPLPPart.Name = "BttnMPLPPart"
        Me.BttnMPLPPart.Size = New System.Drawing.Size(144, 23)
        Me.BttnMPLPPart.TabIndex = 13
        Me.BttnMPLPPart.Text = "فهرست قطعات سرقتي"
        Me.BttnMPLPPart.Visible = False
        '
        'grpSearch
        '
        Me.grpSearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.grpSearch.Controls.Add(Me.cmbArea)
        Me.grpSearch.Controls.Add(Me.txtCoverPercentage)
        Me.grpSearch.Controls.Add(Me.chkIsMainKey)
        Me.grpSearch.Controls.Add(Me.BtnSearch)
        Me.grpSearch.Controls.Add(Me.lblCoverPercentage)
        Me.grpSearch.Controls.Add(Me.txtLPPostName)
        Me.grpSearch.Controls.Add(Me.lblLPPostName)
        Me.grpSearch.Controls.Add(Me.lblActiveStatePostFeeder)
        Me.grpSearch.Controls.Add(Me.cmbActiveStatePostFeeder)
        Me.grpSearch.Controls.Add(Me.txtGISCode)
        Me.grpSearch.Controls.Add(Me.lblGISCode)
        Me.grpSearch.Controls.Add(Me.cmbMPCloserType)
        Me.grpSearch.Controls.Add(Me.lblMPCloserType)
        Me.grpSearch.Controls.Add(Me.lblOwnership)
        Me.grpSearch.Controls.Add(Me.cmbOwnership)
        Me.grpSearch.Controls.Add(Me.lblArea)
        Me.grpSearch.Controls.Add(Me.pnlMPPost_Down)
        Me.grpSearch.Location = New System.Drawing.Point(9, 8)
        Me.grpSearch.Name = "grpSearch"
        Me.grpSearch.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.grpSearch.Size = New System.Drawing.Size(707, 143)
        Me.grpSearch.TabIndex = 16
        Me.grpSearch.Visible = False
        '
        'cmbArea
        '
        Me.cmbArea.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbArea.BackColor = System.Drawing.Color.White
        Me.cmbArea.DataSource = Me.DatasetCcReq2.Tbl_Area
        Me.cmbArea.DisplayMember = "Area"
        Me.cmbArea.Enabled = False
        Me.cmbArea.IsReadOnly = False
        Me.cmbArea.Location = New System.Drawing.Point(427, 13)
        Me.cmbArea.Name = "cmbArea"
        Me.cmbArea.Size = New System.Drawing.Size(184, 21)
        Me.cmbArea.TabIndex = 170
        Me.cmbArea.ValueMember = "AreaId"
        '
        'DatasetCcReq2
        '
        Me.DatasetCcReq2.DataSetName = "DatasetCcRequester"
        Me.DatasetCcReq2.Locale = New System.Globalization.CultureInfo("en-US")
        Me.DatasetCcReq2.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'txtCoverPercentage
        '
        Me.txtCoverPercentage.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCoverPercentage.CaptinText = ""
        Me.txtCoverPercentage.HasCaption = False
        Me.txtCoverPercentage.IsForceText = False
        Me.txtCoverPercentage.IsFractional = False
        Me.txtCoverPercentage.IsIP = False
        Me.txtCoverPercentage.IsNumberOnly = True
        Me.txtCoverPercentage.IsYear = False
        Me.txtCoverPercentage.Location = New System.Drawing.Point(20, 40)
        Me.txtCoverPercentage.MaxLength = 3
        Me.txtCoverPercentage.Name = "txtCoverPercentage"
        Me.txtCoverPercentage.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtCoverPercentage.Size = New System.Drawing.Size(41, 21)
        Me.txtCoverPercentage.TabIndex = 18
        Me.txtCoverPercentage.Text = "100"
        Me.txtCoverPercentage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtCoverPercentage.Visible = False
        '
        'chkIsMainKey
        '
        Me.chkIsMainKey.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkIsMainKey.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkIsMainKey.Location = New System.Drawing.Point(235, 40)
        Me.chkIsMainKey.Name = "chkIsMainKey"
        Me.chkIsMainKey.Size = New System.Drawing.Size(104, 24)
        Me.chkIsMainKey.TabIndex = 184
        Me.chkIsMainKey.Text = "فقط کليد اصلي"
        Me.chkIsMainKey.Visible = False
        '
        'BtnSearch
        '
        Me.BtnSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnSearch.Image = CType(resources.GetObject("BtnSearch.Image"), System.Drawing.Image)
        Me.BtnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.BtnSearch.Location = New System.Drawing.Point(8, 114)
        Me.BtnSearch.Name = "BtnSearch"
        Me.BtnSearch.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.BtnSearch.Size = New System.Drawing.Size(75, 23)
        Me.BtnSearch.TabIndex = 0
        Me.BtnSearch.Text = "جستجو"
        Me.BtnSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblCoverPercentage
        '
        Me.lblCoverPercentage.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCoverPercentage.AutoSize = True
        Me.lblCoverPercentage.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCoverPercentage.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.lblCoverPercentage.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblCoverPercentage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCoverPercentage.Location = New System.Drawing.Point(67, 34)
        Me.lblCoverPercentage.Name = "lblCoverPercentage"
        Me.lblCoverPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCoverPercentage.Size = New System.Drawing.Size(79, 36)
        Me.lblCoverPercentage.TabIndex = 175
        Me.lblCoverPercentage.Text = "درصد مالکيت " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "فيدر فشار متوسط"
        Me.lblCoverPercentage.Visible = False
        '
        'txtLPPostName
        '
        Me.txtLPPostName.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLPPostName.Location = New System.Drawing.Point(163, 13)
        Me.txtLPPostName.Name = "txtLPPostName"
        Me.txtLPPostName.Size = New System.Drawing.Size(176, 21)
        Me.txtLPPostName.TabIndex = 176
        '
        'lblLPPostName
        '
        Me.lblLPPostName.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblLPPostName.AutoSize = True
        Me.lblLPPostName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLPPostName.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.lblLPPostName.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblLPPostName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLPPostName.Location = New System.Drawing.Point(347, 13)
        Me.lblLPPostName.Name = "lblLPPostName"
        Me.lblLPPostName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLPPostName.Size = New System.Drawing.Size(70, 18)
        Me.lblLPPostName.TabIndex = 175
        Me.lblLPPostName.Text = "نام پست توزيع"
        '
        'lblActiveStatePostFeeder
        '
        Me.lblActiveStatePostFeeder.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblActiveStatePostFeeder.AutoSize = True
        Me.lblActiveStatePostFeeder.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblActiveStatePostFeeder.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.lblActiveStatePostFeeder.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblActiveStatePostFeeder.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblActiveStatePostFeeder.Location = New System.Drawing.Point(238, 65)
        Me.lblActiveStatePostFeeder.Name = "lblActiveStatePostFeeder"
        Me.lblActiveStatePostFeeder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblActiveStatePostFeeder.Size = New System.Drawing.Size(29, 18)
        Me.lblActiveStatePostFeeder.TabIndex = 180
        Me.lblActiveStatePostFeeder.Text = "حالت"
        '
        'cmbActiveStatePostFeeder
        '
        Me.cmbActiveStatePostFeeder.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbActiveStatePostFeeder.BackColor = System.Drawing.Color.White
        Me.cmbActiveStatePostFeeder.DisplayMember = "AreaId"
        Me.cmbActiveStatePostFeeder.IsReadOnly = False
        Me.cmbActiveStatePostFeeder.Items.AddRange(New Object() {"فعال", "غير فعال"})
        Me.cmbActiveStatePostFeeder.Location = New System.Drawing.Point(163, 64)
        Me.cmbActiveStatePostFeeder.Name = "cmbActiveStatePostFeeder"
        Me.cmbActiveStatePostFeeder.Size = New System.Drawing.Size(69, 21)
        Me.cmbActiveStatePostFeeder.TabIndex = 179
        Me.cmbActiveStatePostFeeder.ValueMember = "AreaId"
        '
        'txtGISCode
        '
        Me.txtGISCode.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtGISCode.Location = New System.Drawing.Point(-10, 48)
        Me.txtGISCode.Name = "txtGISCode"
        Me.txtGISCode.Size = New System.Drawing.Size(69, 21)
        Me.txtGISCode.TabIndex = 187
        Me.txtGISCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtGISCode.Visible = False
        '
        'lblGISCode
        '
        Me.lblGISCode.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblGISCode.AutoSize = True
        Me.lblGISCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGISCode.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.lblGISCode.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblGISCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblGISCode.Location = New System.Drawing.Point(67, 48)
        Me.lblGISCode.Name = "lblGISCode"
        Me.lblGISCode.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblGISCode.Size = New System.Drawing.Size(39, 18)
        Me.lblGISCode.TabIndex = 186
        Me.lblGISCode.Text = "کد GIS"
        Me.lblGISCode.Visible = False
        '
        'cmbMPCloserType
        '
        Me.cmbMPCloserType.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbMPCloserType.BackColor = System.Drawing.Color.White
        Me.cmbMPCloserType.DisplayMember = "Ownership"
        Me.cmbMPCloserType.IsReadOnly = False
        Me.cmbMPCloserType.Location = New System.Drawing.Point(-36, 72)
        Me.cmbMPCloserType.Name = "cmbMPCloserType"
        Me.cmbMPCloserType.Size = New System.Drawing.Size(95, 21)
        Me.cmbMPCloserType.TabIndex = 182
        Me.cmbMPCloserType.ValueMember = "OwnershipId"
        Me.cmbMPCloserType.Visible = False
        '
        'lblMPCloserType
        '
        Me.lblMPCloserType.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMPCloserType.AutoSize = True
        Me.lblMPCloserType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMPCloserType.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.lblMPCloserType.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblMPCloserType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMPCloserType.Location = New System.Drawing.Point(71, 72)
        Me.lblMPCloserType.Name = "lblMPCloserType"
        Me.lblMPCloserType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMPCloserType.Size = New System.Drawing.Size(42, 18)
        Me.lblMPCloserType.TabIndex = 183
        Me.lblMPCloserType.Text = "نوع کليد"
        Me.lblMPCloserType.Visible = False
        '
        'lblOwnership
        '
        Me.lblOwnership.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblOwnership.AutoSize = True
        Me.lblOwnership.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOwnership.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.lblOwnership.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblOwnership.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOwnership.Location = New System.Drawing.Point(375, 64)
        Me.lblOwnership.Name = "lblOwnership"
        Me.lblOwnership.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOwnership.Size = New System.Drawing.Size(36, 18)
        Me.lblOwnership.TabIndex = 183
        Me.lblOwnership.Text = "مالکيت"
        Me.lblOwnership.Visible = False
        '
        'cmbOwnership
        '
        Me.cmbOwnership.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbOwnership.BackColor = System.Drawing.Color.White
        Me.cmbOwnership.DataSource = Me.DatasetCcReq2.Tbl_Ownership
        Me.cmbOwnership.DisplayMember = "Ownership"
        Me.cmbOwnership.IsReadOnly = False
        Me.cmbOwnership.Location = New System.Drawing.Point(268, 64)
        Me.cmbOwnership.Name = "cmbOwnership"
        Me.cmbOwnership.Size = New System.Drawing.Size(95, 21)
        Me.cmbOwnership.TabIndex = 182
        Me.cmbOwnership.ValueMember = "OwnershipId"
        Me.cmbOwnership.Visible = False
        '
        'lblArea
        '
        Me.lblArea.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblArea.AutoSize = True
        Me.lblArea.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblArea.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.lblArea.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblArea.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblArea.Location = New System.Drawing.Point(622, 13)
        Me.lblArea.Name = "lblArea"
        Me.lblArea.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblArea.Size = New System.Drawing.Size(55, 18)
        Me.lblArea.TabIndex = 175
        Me.lblArea.Text = "ناحيه مرتبط"
        '
        'pnlMPPost_Down
        '
        Me.pnlMPPost_Down.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlMPPost_Down.Controls.Add(Me.txtLPPostLocalCode)
        Me.pnlMPPost_Down.Controls.Add(Me.lblLPPostLocalCode)
        Me.pnlMPPost_Down.Controls.Add(Me.btnInVisible)
        Me.pnlMPPost_Down.Controls.Add(Me.txtLPPostCode)
        Me.pnlMPPost_Down.Controls.Add(Me.lblLPPostCode)
        Me.pnlMPPost_Down.Controls.Add(Me.cmbMPPost)
        Me.pnlMPPost_Down.Controls.Add(Me.cmbMPFeeder)
        Me.pnlMPPost_Down.Controls.Add(Me.lblMPFeeder)
        Me.pnlMPPost_Down.Controls.Add(Me.lblMPPost)
        Me.pnlMPPost_Down.Controls.Add(Me.lblLPPost)
        Me.pnlMPPost_Down.Controls.Add(Me.cmbLPPost)
        Me.pnlMPPost_Down.Controls.Add(Me.lblAddress)
        Me.pnlMPPost_Down.Controls.Add(Me.txtAddress)
        Me.pnlMPPost_Down.Controls.Add(Me.pnlPeakBar)
        Me.pnlMPPost_Down.Controls.Add(Me.cmbCriticalType)
        Me.pnlMPPost_Down.Controls.Add(Me.lblCriticalType)
        Me.pnlMPPost_Down.Controls.Add(Me.pnlLighFeeder)
        Me.pnlMPPost_Down.Controls.Add(Me.pnlLPPostYear)
        Me.pnlMPPost_Down.Location = New System.Drawing.Point(157, 38)
        Me.pnlMPPost_Down.Name = "pnlMPPost_Down"
        Me.pnlMPPost_Down.Size = New System.Drawing.Size(547, 99)
        Me.pnlMPPost_Down.TabIndex = 181
        '
        'txtLPPostLocalCode
        '
        Me.txtLPPostLocalCode.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLPPostLocalCode.Location = New System.Drawing.Point(112, 48)
        Me.txtLPPostLocalCode.Name = "txtLPPostLocalCode"
        Me.txtLPPostLocalCode.Size = New System.Drawing.Size(69, 21)
        Me.txtLPPostLocalCode.TabIndex = 187
        Me.txtLPPostLocalCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtLPPostLocalCode.Visible = False
        '
        'lblLPPostLocalCode
        '
        Me.lblLPPostLocalCode.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblLPPostLocalCode.AutoSize = True
        Me.lblLPPostLocalCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLPPostLocalCode.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.lblLPPostLocalCode.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblLPPostLocalCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLPPostLocalCode.Location = New System.Drawing.Point(189, 48)
        Me.lblLPPostLocalCode.Name = "lblLPPostLocalCode"
        Me.lblLPPostLocalCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLPPostLocalCode.Size = New System.Drawing.Size(70, 18)
        Me.lblLPPostLocalCode.TabIndex = 186
        Me.lblLPPostLocalCode.Text = "کد محلي پست"
        Me.lblLPPostLocalCode.Visible = False
        '
        'btnInVisible
        '
        Me.btnInVisible.Image = CType(resources.GetObject("btnInVisible.Image"), System.Drawing.Image)
        Me.btnInVisible.Location = New System.Drawing.Point(0, 712)
        Me.btnInVisible.Name = "btnInVisible"
        Me.btnInVisible.Size = New System.Drawing.Size(24, 23)
        Me.btnInVisible.TabIndex = 182
        Me.btnInVisible.Visible = False
        '
        'txtLPPostCode
        '
        Me.txtLPPostCode.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLPPostCode.Location = New System.Drawing.Point(112, 24)
        Me.txtLPPostCode.Name = "txtLPPostCode"
        Me.txtLPPostCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLPPostCode.Size = New System.Drawing.Size(69, 21)
        Me.txtLPPostCode.TabIndex = 176
        Me.txtLPPostCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblLPPostCode
        '
        Me.lblLPPostCode.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblLPPostCode.AutoSize = True
        Me.lblLPPostCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLPPostCode.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.lblLPPostCode.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblLPPostCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLPPostCode.Location = New System.Drawing.Point(189, 24)
        Me.lblLPPostCode.Name = "lblLPPostCode"
        Me.lblLPPostCode.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblLPPostCode.Size = New System.Drawing.Size(64, 18)
        Me.lblLPPostCode.TabIndex = 175
        Me.lblLPPostCode.Text = "کد GIS پست"
        '
        'cmbMPPost
        '
        Me.cmbMPPost.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbMPPost.BackColor = System.Drawing.Color.White
        Me.cmbMPPost.DataSource = Me.DatasetCcReq2.Tbl_MPPost
        Me.cmbMPPost.DisplayMember = "MPPostName"
        Me.cmbMPPost.IsReadOnly = False
        Me.cmbMPPost.Location = New System.Drawing.Point(271, 2)
        Me.cmbMPPost.Name = "cmbMPPost"
        Me.cmbMPPost.Size = New System.Drawing.Size(185, 21)
        Me.cmbMPPost.TabIndex = 172
        Me.cmbMPPost.ValueMember = "MPPostId"
        '
        'cmbMPFeeder
        '
        Me.cmbMPFeeder.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbMPFeeder.BackColor = System.Drawing.Color.White
        Me.cmbMPFeeder.DataSource = Me.DatasetCcReq2.Tbl_MPFeeder
        Me.cmbMPFeeder.DisplayMember = "MPFeederName"
        Me.cmbMPFeeder.IsReadOnly = False
        Me.cmbMPFeeder.Location = New System.Drawing.Point(271, 29)
        Me.cmbMPFeeder.Name = "cmbMPFeeder"
        Me.cmbMPFeeder.Size = New System.Drawing.Size(185, 21)
        Me.cmbMPFeeder.TabIndex = 171
        Me.cmbMPFeeder.ValueMember = "MPFeederId"
        '
        'lblMPFeeder
        '
        Me.lblMPFeeder.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMPFeeder.AutoSize = True
        Me.lblMPFeeder.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMPFeeder.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.lblMPFeeder.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblMPFeeder.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMPFeeder.Location = New System.Drawing.Point(461, 29)
        Me.lblMPFeeder.Name = "lblMPFeeder"
        Me.lblMPFeeder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMPFeeder.Size = New System.Drawing.Size(79, 18)
        Me.lblMPFeeder.TabIndex = 174
        Me.lblMPFeeder.Text = "فيدر فشار متوسط"
        '
        'lblMPPost
        '
        Me.lblMPPost.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMPPost.AutoSize = True
        Me.lblMPPost.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMPPost.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.lblMPPost.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblMPPost.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMPPost.Location = New System.Drawing.Point(461, 2)
        Me.lblMPPost.Name = "lblMPPost"
        Me.lblMPPost.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMPPost.Size = New System.Drawing.Size(75, 18)
        Me.lblMPPost.TabIndex = 173
        Me.lblMPPost.Text = "پست فوق توزيع"
        '
        'lblLPPost
        '
        Me.lblLPPost.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblLPPost.AutoSize = True
        Me.lblLPPost.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLPPost.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.lblLPPost.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblLPPost.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLPPost.Location = New System.Drawing.Point(461, 56)
        Me.lblLPPost.Name = "lblLPPost"
        Me.lblLPPost.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLPPost.Size = New System.Drawing.Size(55, 18)
        Me.lblLPPost.TabIndex = 178
        Me.lblLPPost.Text = "پست توزيع"
        Me.lblLPPost.Visible = False
        '
        'cmbLPPost
        '
        Me.cmbLPPost.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbLPPost.BackColor = System.Drawing.Color.White
        Me.cmbLPPost.DataSource = Me.DatasetCcReq2.Tbl_LPPost
        Me.cmbLPPost.DisplayMember = "LPPostName"
        Me.cmbLPPost.IsReadOnly = False
        Me.cmbLPPost.Location = New System.Drawing.Point(271, 56)
        Me.cmbLPPost.Name = "cmbLPPost"
        Me.cmbLPPost.Size = New System.Drawing.Size(185, 21)
        Me.cmbLPPost.TabIndex = 177
        Me.cmbLPPost.ValueMember = "LPPostId"
        Me.cmbLPPost.Visible = False
        '
        'lblAddress
        '
        Me.lblAddress.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblAddress.AutoSize = True
        Me.lblAddress.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAddress.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.lblAddress.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblAddress.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAddress.Location = New System.Drawing.Point(461, 56)
        Me.lblAddress.Name = "lblAddress"
        Me.lblAddress.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAddress.Size = New System.Drawing.Size(33, 18)
        Me.lblAddress.TabIndex = 175
        Me.lblAddress.Text = "آدرس"
        Me.lblAddress.Visible = False
        '
        'txtAddress
        '
        Me.txtAddress.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAddress.Location = New System.Drawing.Point(272, 56)
        Me.txtAddress.Name = "txtAddress"
        Me.txtAddress.Size = New System.Drawing.Size(184, 21)
        Me.txtAddress.TabIndex = 176
        Me.txtAddress.Visible = False
        '
        'pnlPeakBar
        '
        Me.pnlPeakBar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlPeakBar.Controls.Add(Me.txtPeakBarFrom)
        Me.pnlPeakBar.Controls.Add(Me.lblPeakBarTo)
        Me.pnlPeakBar.Controls.Add(Me.txtPeakBarTo)
        Me.pnlPeakBar.Controls.Add(Me.lblPeakBarFrom)
        Me.pnlPeakBar.Location = New System.Drawing.Point(0, -2)
        Me.pnlPeakBar.Name = "pnlPeakBar"
        Me.pnlPeakBar.Size = New System.Drawing.Size(264, 24)
        Me.pnlPeakBar.TabIndex = 179
        '
        'txtPeakBarFrom
        '
        Me.txtPeakBarFrom.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPeakBarFrom.Location = New System.Drawing.Point(112, 2)
        Me.txtPeakBarFrom.Name = "txtPeakBarFrom"
        Me.txtPeakBarFrom.Size = New System.Drawing.Size(69, 21)
        Me.txtPeakBarFrom.TabIndex = 176
        '
        'lblPeakBarTo
        '
        Me.lblPeakBarTo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPeakBarTo.AutoSize = True
        Me.lblPeakBarTo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPeakBarTo.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.lblPeakBarTo.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblPeakBarTo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPeakBarTo.Location = New System.Drawing.Point(88, 2)
        Me.lblPeakBarTo.Name = "lblPeakBarTo"
        Me.lblPeakBarTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPeakBarTo.Size = New System.Drawing.Size(14, 18)
        Me.lblPeakBarTo.TabIndex = 175
        Me.lblPeakBarTo.Text = "تا"
        Me.lblPeakBarTo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtPeakBarTo
        '
        Me.txtPeakBarTo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPeakBarTo.Location = New System.Drawing.Point(8, 2)
        Me.txtPeakBarTo.Name = "txtPeakBarTo"
        Me.txtPeakBarTo.Size = New System.Drawing.Size(69, 21)
        Me.txtPeakBarTo.TabIndex = 176
        '
        'lblPeakBarFrom
        '
        Me.lblPeakBarFrom.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPeakBarFrom.AutoSize = True
        Me.lblPeakBarFrom.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPeakBarFrom.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.lblPeakBarFrom.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblPeakBarFrom.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPeakBarFrom.Location = New System.Drawing.Point(192, 2)
        Me.lblPeakBarFrom.Name = "lblPeakBarFrom"
        Me.lblPeakBarFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPeakBarFrom.Size = New System.Drawing.Size(72, 18)
        Me.lblPeakBarFrom.TabIndex = 175
        Me.lblPeakBarFrom.Text = "از بار پيک پست"
        '
        'cmbCriticalType
        '
        Me.cmbCriticalType.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbCriticalType.BackColor = System.Drawing.Color.White
        Me.cmbCriticalType.DisplayMember = "AreaId"
        Me.cmbCriticalType.IsReadOnly = False
        Me.cmbCriticalType.Items.AddRange(New Object() {"حساس", "حساس دائم", "حساس موقت"})
        Me.cmbCriticalType.Location = New System.Drawing.Point(112, 56)
        Me.cmbCriticalType.Name = "cmbCriticalType"
        Me.cmbCriticalType.Size = New System.Drawing.Size(69, 21)
        Me.cmbCriticalType.TabIndex = 179
        Me.cmbCriticalType.ValueMember = "AreaId"
        Me.cmbCriticalType.Visible = False
        '
        'lblCriticalType
        '
        Me.lblCriticalType.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCriticalType.AutoSize = True
        Me.lblCriticalType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCriticalType.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.lblCriticalType.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblCriticalType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCriticalType.Location = New System.Drawing.Point(189, 56)
        Me.lblCriticalType.Name = "lblCriticalType"
        Me.lblCriticalType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCriticalType.Size = New System.Drawing.Size(64, 18)
        Me.lblCriticalType.TabIndex = 180
        Me.lblCriticalType.Text = "نوع حساسيت"
        Me.lblCriticalType.Visible = False
        '
        'pnlLighFeeder
        '
        Me.pnlLighFeeder.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlLighFeeder.Controls.Add(Me.rbLightLPFeeders)
        Me.pnlLighFeeder.Controls.Add(Me.rbLPLPFeeders)
        Me.pnlLighFeeder.Controls.Add(Me.rbAllLPFeeders)
        Me.pnlLighFeeder.Location = New System.Drawing.Point(232, 73)
        Me.pnlLighFeeder.Name = "pnlLighFeeder"
        Me.pnlLighFeeder.Size = New System.Drawing.Size(224, 27)
        Me.pnlLighFeeder.TabIndex = 181
        Me.pnlLighFeeder.Visible = False
        '
        'rbLightLPFeeders
        '
        Me.rbLightLPFeeders.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rbLightLPFeeders.AutoSize = True
        Me.rbLightLPFeeders.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rbLightLPFeeders.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.rbLightLPFeeders.Location = New System.Drawing.Point(153, 5)
        Me.rbLightLPFeeders.Name = "rbLightLPFeeders"
        Me.rbLightLPFeeders.Size = New System.Drawing.Size(65, 23)
        Me.rbLightLPFeeders.TabIndex = 0
        Me.rbLightLPFeeders.Text = "روشنايي"
        '
        'rbLPLPFeeders
        '
        Me.rbLPLPFeeders.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rbLPLPFeeders.AutoSize = True
        Me.rbLPLPFeeders.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rbLPLPFeeders.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.rbLPLPFeeders.Location = New System.Drawing.Point(68, 5)
        Me.rbLPLPFeeders.Name = "rbLPLPFeeders"
        Me.rbLPLPFeeders.Size = New System.Drawing.Size(81, 23)
        Me.rbLPLPFeeders.TabIndex = 0
        Me.rbLPLPFeeders.Text = "غير روشنايي"
        '
        'rbAllLPFeeders
        '
        Me.rbAllLPFeeders.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rbAllLPFeeders.AutoSize = True
        Me.rbAllLPFeeders.Checked = True
        Me.rbAllLPFeeders.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rbAllLPFeeders.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.rbAllLPFeeders.Location = New System.Drawing.Point(12, 5)
        Me.rbAllLPFeeders.Name = "rbAllLPFeeders"
        Me.rbAllLPFeeders.Size = New System.Drawing.Size(48, 23)
        Me.rbAllLPFeeders.TabIndex = 0
        Me.rbAllLPFeeders.TabStop = True
        Me.rbAllLPFeeders.Text = "همه"
        '
        'pnlLPPostYear
        '
        Me.pnlLPPostYear.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlLPPostYear.Controls.Add(Me.Label13)
        Me.pnlLPPostYear.Controls.Add(Me.txtFromBuildYear)
        Me.pnlLPPostYear.Controls.Add(Me.Label14)
        Me.pnlLPPostYear.Controls.Add(Me.txtToBuildYear)
        Me.pnlLPPostYear.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.pnlLPPostYear.Location = New System.Drawing.Point(91, 70)
        Me.pnlLPPostYear.Name = "pnlLPPostYear"
        Me.pnlLPPostYear.Size = New System.Drawing.Size(173, 24)
        Me.pnlLPPostYear.TabIndex = 179
        Me.pnlLPPostYear.Visible = False
        '
        'Label13
        '
        Me.Label13.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label13.AutoSize = True
        Me.Label13.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label13.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label13.Location = New System.Drawing.Point(98, 2)
        Me.Label13.Name = "Label13"
        Me.Label13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label13.Size = New System.Drawing.Size(63, 18)
        Me.Label13.TabIndex = 20
        Me.Label13.Text = "از سال احداث"
        '
        'txtFromBuildYear
        '
        Me.txtFromBuildYear.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFromBuildYear.CaptinText = ""
        Me.txtFromBuildYear.HasCaption = False
        Me.txtFromBuildYear.IsForceText = False
        Me.txtFromBuildYear.IsFractional = False
        Me.txtFromBuildYear.IsIP = False
        Me.txtFromBuildYear.IsNumberOnly = True
        Me.txtFromBuildYear.IsYear = True
        Me.txtFromBuildYear.Location = New System.Drawing.Point(58, 2)
        Me.txtFromBuildYear.MaxLength = 4
        Me.txtFromBuildYear.Name = "txtFromBuildYear"
        Me.txtFromBuildYear.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtFromBuildYear.Size = New System.Drawing.Size(32, 21)
        Me.txtFromBuildYear.TabIndex = 18
        Me.txtFromBuildYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label14
        '
        Me.Label14.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label14.AutoSize = True
        Me.Label14.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label14.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label14.Location = New System.Drawing.Point(39, 2)
        Me.Label14.Name = "Label14"
        Me.Label14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label14.Size = New System.Drawing.Size(14, 18)
        Me.Label14.TabIndex = 21
        Me.Label14.Text = "تا"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtToBuildYear
        '
        Me.txtToBuildYear.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtToBuildYear.CaptinText = ""
        Me.txtToBuildYear.HasCaption = False
        Me.txtToBuildYear.IsForceText = False
        Me.txtToBuildYear.IsFractional = False
        Me.txtToBuildYear.IsIP = False
        Me.txtToBuildYear.IsNumberOnly = True
        Me.txtToBuildYear.IsYear = True
        Me.txtToBuildYear.Location = New System.Drawing.Point(2, 2)
        Me.txtToBuildYear.MaxLength = 4
        Me.txtToBuildYear.Name = "txtToBuildYear"
        Me.txtToBuildYear.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtToBuildYear.Size = New System.Drawing.Size(32, 21)
        Me.txtToBuildYear.TabIndex = 19
        Me.txtToBuildYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnZonePreCodes
        '
        Me.btnZonePreCodes.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnZonePreCodes.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnZonePreCodes.Location = New System.Drawing.Point(402, 465)
        Me.btnZonePreCodes.Name = "btnZonePreCodes"
        Me.btnZonePreCodes.Size = New System.Drawing.Size(128, 23)
        Me.btnZonePreCodes.TabIndex = 5
        Me.btnZonePreCodes.Text = "پيش شماره‌هاي منطقه"
        Me.btnZonePreCodes.Visible = False
        '
        'DatasetBazdid1
        '
        Me.DatasetBazdid1.DataSetName = "DatasetBazdid"
        Me.DatasetBazdid1.Locale = New System.Globalization.CultureInfo("en-US")
        Me.DatasetBazdid1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'DatasetBazdidView1
        '
        Me.DatasetBazdidView1.DataSetName = "DatasetBazdidView"
        Me.DatasetBazdidView1.Locale = New System.Globalization.CultureInfo("en-US")
        Me.DatasetBazdidView1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'btnGroupChekList
        '
        Me.btnGroupChekList.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGroupChekList.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnGroupChekList.Location = New System.Drawing.Point(395, 465)
        Me.btnGroupChekList.Name = "btnGroupChekList"
        Me.btnGroupChekList.Size = New System.Drawing.Size(136, 23)
        Me.btnGroupChekList.TabIndex = 5
        Me.btnGroupChekList.Tag = "چک ليست‌هاي مربوطه"
        Me.btnGroupChekList.Text = "چک ليست‌هاي مربوطه"
        Me.btnGroupChekList.Visible = False
        '
        'btnSubCheckList
        '
        Me.btnSubCheckList.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSubCheckList.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnSubCheckList.Location = New System.Drawing.Point(452, 465)
        Me.btnSubCheckList.Name = "btnSubCheckList"
        Me.btnSubCheckList.Size = New System.Drawing.Size(93, 23)
        Me.btnSubCheckList.TabIndex = 5
        Me.btnSubCheckList.Tag = "ريز خرابي"
        Me.btnSubCheckList.Text = "ريز خرابي"
        Me.btnSubCheckList.Visible = False
        '
        'btnCheckListTajhizat
        '
        Me.btnCheckListTajhizat.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCheckListTajhizat.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnCheckListTajhizat.Location = New System.Drawing.Point(352, 465)
        Me.btnCheckListTajhizat.Name = "btnCheckListTajhizat"
        Me.btnCheckListTajhizat.Size = New System.Drawing.Size(96, 23)
        Me.btnCheckListTajhizat.TabIndex = 5
        Me.btnCheckListTajhizat.Tag = "تجهيزات لازم"
        Me.btnCheckListTajhizat.Text = "تجهيزات لازم"
        Me.btnCheckListTajhizat.Visible = False
        '
        'btnActive_DeActive
        '
        Me.btnActive_DeActive.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnActive_DeActive.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnActive_DeActive.Location = New System.Drawing.Point(252, 465)
        Me.btnActive_DeActive.Name = "btnActive_DeActive"
        Me.btnActive_DeActive.Size = New System.Drawing.Size(96, 23)
        Me.btnActive_DeActive.TabIndex = 5
        Me.btnActive_DeActive.Tag = "تجهيزات لازم"
        Me.btnActive_DeActive.Text = "فعال/غير فعال"
        Me.btnActive_DeActive.Visible = False
        '
        'btnFeederPart
        '
        Me.btnFeederPart.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFeederPart.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnFeederPart.Location = New System.Drawing.Point(246, 465)
        Me.btnFeederPart.Name = "btnFeederPart"
        Me.btnFeederPart.Size = New System.Drawing.Size(72, 23)
        Me.btnFeederPart.TabIndex = 3
        Me.btnFeederPart.Text = "تکه فيدرها"
        Me.btnFeederPart.Visible = False
        '
        'DatasetErja1
        '
        Me.DatasetErja1.DataSetName = "DatasetErja"
        Me.DatasetErja1.Locale = New System.Globalization.CultureInfo("en-US")
        Me.DatasetErja1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'cmbActiveState
        '
        Me.cmbActiveState.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbActiveState.BackColor = System.Drawing.Color.White
        Me.cmbActiveState.DisplayMember = "AreaId"
        Me.cmbActiveState.IsReadOnly = False
        Me.cmbActiveState.Items.AddRange(New Object() {"فعال", "غير فعال"})
        Me.cmbActiveState.Location = New System.Drawing.Point(505, 8)
        Me.cmbActiveState.Name = "cmbActiveState"
        Me.cmbActiveState.Size = New System.Drawing.Size(104, 21)
        Me.cmbActiveState.TabIndex = 176
        Me.cmbActiveState.ValueMember = "AreaId"
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label1.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(620, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(29, 18)
        Me.Label1.TabIndex = 177
        Me.Label1.Text = "حالت"
        '
        'grpSearchGlobal
        '
        Me.grpSearchGlobal.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpSearchGlobal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.grpSearchGlobal.Controls.Add(Me.btnSearchGlobal)
        Me.grpSearchGlobal.Controls.Add(Me.Label1)
        Me.grpSearchGlobal.Controls.Add(Me.cmbActiveState)
        Me.grpSearchGlobal.Location = New System.Drawing.Point(9, 8)
        Me.grpSearchGlobal.Name = "grpSearchGlobal"
        Me.grpSearchGlobal.Size = New System.Drawing.Size(707, 40)
        Me.grpSearchGlobal.TabIndex = 180
        Me.grpSearchGlobal.Visible = False
        '
        'btnSearchGlobal
        '
        Me.btnSearchGlobal.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSearchGlobal.Image = CType(resources.GetObject("btnSearchGlobal.Image"), System.Drawing.Image)
        Me.btnSearchGlobal.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSearchGlobal.Location = New System.Drawing.Point(8, 7)
        Me.btnSearchGlobal.Name = "btnSearchGlobal"
        Me.btnSearchGlobal.Size = New System.Drawing.Size(75, 23)
        Me.btnSearchGlobal.TabIndex = 0
        Me.btnSearchGlobal.Text = "جستجو"
        Me.btnSearchGlobal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnCheckListPart
        '
        Me.btnCheckListPart.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCheckListPart.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnCheckListPart.Location = New System.Drawing.Point(369, 503)
        Me.btnCheckListPart.Name = "btnCheckListPart"
        Me.btnCheckListPart.Size = New System.Drawing.Size(176, 23)
        Me.btnCheckListPart.TabIndex = 5
        Me.btnCheckListPart.Tag = "قطعات مورد نياز هنگام سرويس"
        Me.btnCheckListPart.Text = "قطعات مورد نياز هنگام سرويس"
        Me.btnCheckListPart.Visible = False
        '
        'DatasetTamir1
        '
        Me.DatasetTamir1.DataSetName = "DatasetTamir"
        Me.DatasetTamir1.Locale = New System.Globalization.CultureInfo("en-US")
        Me.DatasetTamir1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'btnErjaCause
        '
        Me.btnErjaCause.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnErjaCause.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnErjaCause.Location = New System.Drawing.Point(411, 465)
        Me.btnErjaCause.Name = "btnErjaCause"
        Me.btnErjaCause.Size = New System.Drawing.Size(120, 23)
        Me.btnErjaCause.TabIndex = 5
        Me.btnErjaCause.Tag = "ريز خرابي"
        Me.btnErjaCause.Text = "علل اشکال بوجود آمده"
        Me.btnErjaCause.Visible = False
        '
        'pnlPartTypeEdit
        '
        Me.pnlPartTypeEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlPartTypeEdit.Controls.Add(Me.btnEditSerghatiPart)
        Me.pnlPartTypeEdit.Controls.Add(Me.btnNewSerghatiPart)
        Me.pnlPartTypeEdit.Location = New System.Drawing.Point(259, 465)
        Me.pnlPartTypeEdit.Name = "pnlPartTypeEdit"
        Me.pnlPartTypeEdit.Size = New System.Drawing.Size(248, 23)
        Me.pnlPartTypeEdit.TabIndex = 181
        Me.pnlPartTypeEdit.Visible = False
        '
        'btnEditSerghatiPart
        '
        Me.btnEditSerghatiPart.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEditSerghatiPart.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEditSerghatiPart.Image = CType(resources.GetObject("btnEditSerghatiPart.Image"), System.Drawing.Image)
        Me.btnEditSerghatiPart.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnEditSerghatiPart.Location = New System.Drawing.Point(5, 0)
        Me.btnEditSerghatiPart.Name = "btnEditSerghatiPart"
        Me.btnEditSerghatiPart.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnEditSerghatiPart.Size = New System.Drawing.Size(120, 23)
        Me.btnEditSerghatiPart.TabIndex = 2
        Me.btnEditSerghatiPart.Text = "ويرايش قطعه سرقتي"
        Me.btnEditSerghatiPart.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnNewSerghatiPart
        '
        Me.btnNewSerghatiPart.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNewSerghatiPart.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNewSerghatiPart.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnNewSerghatiPart.Location = New System.Drawing.Point(133, 0)
        Me.btnNewSerghatiPart.Name = "btnNewSerghatiPart"
        Me.btnNewSerghatiPart.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnNewSerghatiPart.Size = New System.Drawing.Size(112, 23)
        Me.btnNewSerghatiPart.TabIndex = 1
        Me.btnNewSerghatiPart.Text = "قطعه سرقتي جديد"
        Me.btnNewSerghatiPart.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'chkIsShowParts
        '
        Me.chkIsShowParts.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkIsShowParts.Checked = True
        Me.chkIsShowParts.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkIsShowParts.Font = New System.Drawing.Font("Mitra", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.chkIsShowParts.Location = New System.Drawing.Point(499, 457)
        Me.chkIsShowParts.Name = "chkIsShowParts"
        Me.chkIsShowParts.Size = New System.Drawing.Size(56, 32)
        Me.chkIsShowParts.TabIndex = 182
        Me.chkIsShowParts.Text = "نمايش قطعات"
        Me.chkIsShowParts.Visible = False
        '
        'btnPartFactories
        '
        Me.btnPartFactories.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPartFactories.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnPartFactories.Location = New System.Drawing.Point(398, 465)
        Me.btnPartFactories.Name = "btnPartFactories"
        Me.btnPartFactories.Size = New System.Drawing.Size(152, 23)
        Me.btnPartFactories.TabIndex = 5
        Me.btnPartFactories.Tag = "کارخانه‌هاي سازنده قطعه"
        Me.btnPartFactories.Text = "کارخانه‌هاي سازنده قطعه"
        Me.btnPartFactories.Visible = False
        '
        'btnSavabeghBazdid
        '
        Me.btnSavabeghBazdid.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSavabeghBazdid.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnSavabeghBazdid.Location = New System.Drawing.Point(641, 502)
        Me.btnSavabeghBazdid.Name = "btnSavabeghBazdid"
        Me.btnSavabeghBazdid.Size = New System.Drawing.Size(75, 23)
        Me.btnSavabeghBazdid.TabIndex = 3
        Me.btnSavabeghBazdid.Text = "سوابق بازديد"
        Me.btnSavabeghBazdid.Visible = False
        '
        'btnImportExcel
        '
        Me.btnImportExcel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnImportExcel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnImportExcel.Location = New System.Drawing.Point(467, 465)
        Me.btnImportExcel.Name = "btnImportExcel"
        Me.btnImportExcel.Size = New System.Drawing.Size(80, 23)
        Me.btnImportExcel.TabIndex = 183
        Me.btnImportExcel.Text = "ورود از اکسل"
        Me.btnImportExcel.Visible = False
        '
        'btnChangePostArea
        '
        Me.btnChangePostArea.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnChangePostArea.Location = New System.Drawing.Point(324, 465)
        Me.btnChangePostArea.Name = "btnChangePostArea"
        Me.btnChangePostArea.Size = New System.Drawing.Size(104, 23)
        Me.btnChangePostArea.TabIndex = 184
        Me.btnChangePostArea.Text = "تغيير ناحيه پست‌ها"
        Me.btnChangePostArea.Visible = False
        '
        'BttnReturn
        '
        Me.BttnReturn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BttnReturn.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.BttnReturn.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BttnReturn.Image = CType(resources.GetObject("BttnReturn.Image"), System.Drawing.Image)
        Me.BttnReturn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BttnReturn.Location = New System.Drawing.Point(9, 502)
        Me.BttnReturn.Name = "BttnReturn"
        Me.BttnReturn.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.BttnReturn.Size = New System.Drawing.Size(75, 23)
        Me.BttnReturn.TabIndex = 14
        Me.BttnReturn.Text = "&بازگشت"
        Me.BttnReturn.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblDesc_btnChangePostArea
        '
        Me.lblDesc_btnChangePostArea.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDesc_btnChangePostArea.BackColor = System.Drawing.Color.White
        Me.lblDesc_btnChangePostArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblDesc_btnChangePostArea.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblDesc_btnChangePostArea.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblDesc_btnChangePostArea.Location = New System.Drawing.Point(278, 399)
        Me.lblDesc_btnChangePostArea.Name = "lblDesc_btnChangePostArea"
        Me.lblDesc_btnChangePostArea.Size = New System.Drawing.Size(184, 64)
        Me.lblDesc_btnChangePostArea.TabIndex = 185
        Me.lblDesc_btnChangePostArea.Tag = "999"
        Me.lblDesc_btnChangePostArea.Text = "اين دکمه براي تغيير «ناحيه»ي پستهايي از زير مجموعه فيدر انتخابي مي‌باشد که در ناح" &
    "يه‌اي متفاوت از ناحيه فيدر، ثبت شده‌اند."
        Me.lblDesc_btnChangePostArea.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblDesc_btnChangePostArea.Visible = False
        '
        'btnChangeLPFeederArea
        '
        Me.btnChangeLPFeederArea.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnChangeLPFeederArea.Location = New System.Drawing.Point(316, 465)
        Me.btnChangeLPFeederArea.Name = "btnChangeLPFeederArea"
        Me.btnChangeLPFeederArea.Size = New System.Drawing.Size(99, 23)
        Me.btnChangeLPFeederArea.TabIndex = 186
        Me.btnChangeLPFeederArea.Text = "تغيير ناحيه فيدرها"
        Me.btnChangeLPFeederArea.Visible = False
        '
        'lblDesc_btnChangeLPFeederArea
        '
        Me.lblDesc_btnChangeLPFeederArea.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDesc_btnChangeLPFeederArea.BackColor = System.Drawing.Color.White
        Me.lblDesc_btnChangeLPFeederArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblDesc_btnChangeLPFeederArea.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblDesc_btnChangeLPFeederArea.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblDesc_btnChangeLPFeederArea.Location = New System.Drawing.Point(273, 399)
        Me.lblDesc_btnChangeLPFeederArea.Name = "lblDesc_btnChangeLPFeederArea"
        Me.lblDesc_btnChangeLPFeederArea.Size = New System.Drawing.Size(184, 64)
        Me.lblDesc_btnChangeLPFeederArea.TabIndex = 187
        Me.lblDesc_btnChangeLPFeederArea.Tag = "999"
        Me.lblDesc_btnChangeLPFeederArea.Text = "اين دکمه براي تغيير «ناحيه»ي فيدرهاي فشار ضعيفي از زير مجموعه پست انتخابي مي‌باشد" &
    " که در ناحيه‌اي متفاوت از ناحيه پست، ثبت شده‌اند."
        Me.lblDesc_btnChangeLPFeederArea.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblDesc_btnChangeLPFeederArea.Visible = False
        '
        'btnPointsAndEarthFeeder
        '
        Me.btnPointsAndEarthFeeder.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPointsAndEarthFeeder.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnPointsAndEarthFeeder.Location = New System.Drawing.Point(359, 503)
        Me.btnPointsAndEarthFeeder.Name = "btnPointsAndEarthFeeder"
        Me.btnPointsAndEarthFeeder.Size = New System.Drawing.Size(128, 23)
        Me.btnPointsAndEarthFeeder.TabIndex = 188
        Me.btnPointsAndEarthFeeder.Text = "نقاط انتهايي و ارت فيدر"
        Me.btnPointsAndEarthFeeder.Visible = False
        '
        'lblSearch
        '
        Me.lblSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSearch.AutoSize = True
        Me.lblSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSearch.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.lblSearch.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSearch.Location = New System.Drawing.Point(659, 0)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSearch.Size = New System.Drawing.Size(51, 18)
        Me.lblSearch.TabIndex = 189
        Me.lblSearch.Text = "  جستجو  "
        Me.lblSearch.Visible = False
        '
        'btnSearchView
        '
        Me.btnSearchView.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSearchView.Image = CType(resources.GetObject("btnSearchView.Image"), System.Drawing.Image)
        Me.btnSearchView.Location = New System.Drawing.Point(14, 12)
        Me.btnSearchView.Name = "btnSearchView"
        Me.btnSearchView.Size = New System.Drawing.Size(24, 20)
        Me.btnSearchView.TabIndex = 191
        Me.btnSearchView.Visible = False
        '
        'pnlSearch
        '
        Me.pnlSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlSearch.BackColor = System.Drawing.Color.Maroon
        Me.pnlSearch.Controls.Add(Me.txtSearch)
        Me.pnlSearch.Location = New System.Drawing.Point(39, 12)
        Me.pnlSearch.Name = "pnlSearch"
        Me.pnlSearch.Size = New System.Drawing.Size(167, 20)
        Me.pnlSearch.TabIndex = 190
        Me.pnlSearch.Visible = False
        '
        'txtSearch
        '
        Me.txtSearch.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.txtSearch.Location = New System.Drawing.Point(-2, -1)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(167, 20)
        Me.txtSearch.TabIndex = 0
        '
        'btn_LPPostEarth
        '
        Me.btn_LPPostEarth.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btn_LPPostEarth.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btn_LPPostEarth.Location = New System.Drawing.Point(262, 503)
        Me.btn_LPPostEarth.Name = "btn_LPPostEarth"
        Me.btn_LPPostEarth.Size = New System.Drawing.Size(128, 23)
        Me.btn_LPPostEarth.TabIndex = 192
        Me.btn_LPPostEarth.Text = "ارت گيري پست توزيع"
        Me.btn_LPPostEarth.Visible = False
        '
        'btnLPPostEarthPoint
        '
        Me.btnLPPostEarthPoint.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnLPPostEarthPoint.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnLPPostEarthPoint.Location = New System.Drawing.Point(537, 503)
        Me.btnLPPostEarthPoint.Name = "btnLPPostEarthPoint"
        Me.btnLPPostEarthPoint.Size = New System.Drawing.Size(98, 23)
        Me.btnLPPostEarthPoint.TabIndex = 193
        Me.btnLPPostEarthPoint.Text = "نقاط ارت پست"
        Me.btnLPPostEarthPoint.UseVisualStyleBackColor = True
        Me.btnLPPostEarthPoint.Visible = False
        '
        'btnZoneMPFeeders
        '
        Me.btnZoneMPFeeders.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnZoneMPFeeders.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnZoneMPFeeders.Location = New System.Drawing.Point(459, 502)
        Me.btnZoneMPFeeders.Name = "btnZoneMPFeeders"
        Me.btnZoneMPFeeders.Size = New System.Drawing.Size(176, 23)
        Me.btnZoneMPFeeders.TabIndex = 5
        Me.btnZoneMPFeeders.Tag = "فیدرهای فشار متوسط منطقه"
        Me.btnZoneMPFeeders.Text = "فیدرهای فشار متوسط منطقه"
        Me.btnZoneMPFeeders.Visible = False
        '
        'DatasetBT1
        '
        Me.DatasetBT1.DataSetName = "DatasetBT"
        Me.DatasetBT1.Locale = New System.Globalization.CultureInfo("en-US")
        Me.DatasetBT1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'rbIsMP
        '
        Me.rbIsMP.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rbIsMP.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.rbIsMP.Location = New System.Drawing.Point(112, 18)
        Me.rbIsMP.Name = "rbIsMP"
        Me.rbIsMP.Size = New System.Drawing.Size(74, 20)
        Me.rbIsMP.TabIndex = 195
        Me.rbIsMP.Text = "فشار متوسط"
        '
        'rbIsLP
        '
        Me.rbIsLP.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rbIsLP.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.rbIsLP.Location = New System.Drawing.Point(44, 18)
        Me.rbIsLP.Name = "rbIsLP"
        Me.rbIsLP.Size = New System.Drawing.Size(69, 20)
        Me.rbIsLP.TabIndex = 196
        Me.rbIsLP.Text = "فشار ضعيف"
        '
        'rbShowAllEkip
        '
        Me.rbShowAllEkip.Checked = True
        Me.rbShowAllEkip.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rbShowAllEkip.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.rbShowAllEkip.Location = New System.Drawing.Point(2, 18)
        Me.rbShowAllEkip.Name = "rbShowAllEkip"
        Me.rbShowAllEkip.Size = New System.Drawing.Size(41, 20)
        Me.rbShowAllEkip.TabIndex = 197
        Me.rbShowAllEkip.TabStop = True
        Me.rbShowAllEkip.Text = "هردو"
        '
        'pnlEkipNetworkType
        '
        Me.pnlEkipNetworkType.Controls.Add(Me.Label2)
        Me.pnlEkipNetworkType.Controls.Add(Me.rbShowAllEkip)
        Me.pnlEkipNetworkType.Controls.Add(Me.rbIsLP)
        Me.pnlEkipNetworkType.Controls.Add(Me.rbIsMP)
        Me.pnlEkipNetworkType.Location = New System.Drawing.Point(113, 497)
        Me.pnlEkipNetworkType.Name = "pnlEkipNetworkType"
        Me.pnlEkipNetworkType.Size = New System.Drawing.Size(190, 39)
        Me.pnlEkipNetworkType.TabIndex = 198
        Me.pnlEkipNetworkType.Visible = False
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label2.Location = New System.Drawing.Point(129, -1)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(55, 16)
        Me.Label2.TabIndex = 198
        Me.Label2.Text = "حوزه کاری:"
        '
        'cmnuReports
        '
        Me.cmnuReports.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuRegLPPostLoad, Me.mnuBlankLPPostExcel})
        '
        'mnuRegLPPostLoad
        '
        Me.mnuRegLPPostLoad.Index = 0
        Me.mnuRegLPPostLoad.Text = "ثبت بارگیری پست توزیع"
        '
        'mnuBlankLPPostExcel
        '
        Me.mnuBlankLPPostExcel.Index = 1
        Me.mnuBlankLPPostExcel.Text = "چاپ فرم خام بارگیری پست توزیع"
        '
        'btnShowOnGISMap
        '
        Me.btnShowOnGISMap.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnShowOnGISMap.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnShowOnGISMap.Image = CType(resources.GetObject("btnShowOnGISMap.Image"), System.Drawing.Image)
        Me.btnShowOnGISMap.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnShowOnGISMap.Location = New System.Drawing.Point(90, 465)
        Me.btnShowOnGISMap.Name = "btnShowOnGISMap"
        Me.btnShowOnGISMap.Size = New System.Drawing.Size(150, 23)
        Me.btnShowOnGISMap.TabIndex = 199
        Me.btnShowOnGISMap.Text = "نمایش روی نقشه GIS"
        Me.btnShowOnGISMap.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnShowOnGISMap.UseVisualStyleBackColor = True
        Me.btnShowOnGISMap.Visible = False
        '
        'lblNotIsAllowSave
        '
        Me.lblNotIsAllowSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblNotIsAllowSave.BackColor = System.Drawing.Color.White
        Me.lblNotIsAllowSave.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblNotIsAllowSave.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.lblNotIsAllowSave.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblNotIsAllowSave.Location = New System.Drawing.Point(623, 408)
        Me.lblNotIsAllowSave.Name = "lblNotIsAllowSave"
        Me.lblNotIsAllowSave.Size = New System.Drawing.Size(93, 80)
        Me.lblNotIsAllowSave.TabIndex = 200
        Me.lblNotIsAllowSave.Tag = "999"
        Me.lblNotIsAllowSave.Text = "### بايد از سمت GIS اضافه گردند"
        Me.lblNotIsAllowSave.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblNotIsAllowSave.Visible = False
        '
        'btnRecloserFunction
        '
        Me.btnRecloserFunction.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRecloserFunction.Enabled = False
        Me.btnRecloserFunction.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnRecloserFunction.Location = New System.Drawing.Point(422, 464)
        Me.btnRecloserFunction.Name = "btnRecloserFunction"
        Me.btnRecloserFunction.Size = New System.Drawing.Size(116, 23)
        Me.btnRecloserFunction.TabIndex = 203
        Me.btnRecloserFunction.Tag = "عملکرد ریکلوزر ها"
        Me.btnRecloserFunction.Text = "عملکرد ریکلوزر ها"
        Me.btnRecloserFunction.Visible = False
        '
        'btnImportSyncFromExcel
        '
        Me.btnImportSyncFromExcel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnImportSyncFromExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnImportSyncFromExcel.Font = New System.Drawing.Font("Tahoma", 8.0!)
        Me.btnImportSyncFromExcel.Image = CType(resources.GetObject("btnImportSyncFromExcel.Image"), System.Drawing.Image)
        Me.btnImportSyncFromExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnImportSyncFromExcel.Location = New System.Drawing.Point(91, 465)
        Me.btnImportSyncFromExcel.Name = "btnImportSyncFromExcel"
        Me.btnImportSyncFromExcel.Size = New System.Drawing.Size(262, 23)
        Me.btnImportSyncFromExcel.TabIndex = 205
        Me.btnImportSyncFromExcel.Text = "ورود اطلاعات همزمان پست و فیدر از فايل اکسل"
        Me.btnImportSyncFromExcel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnImportSyncFromExcel.Visible = False
        '
        'btnImportFromExcel
        '
        Me.btnImportFromExcel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnImportFromExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnImportFromExcel.Font = New System.Drawing.Font("Tahoma", 8.0!)
        Me.btnImportFromExcel.Image = CType(resources.GetObject("btnImportFromExcel.Image"), System.Drawing.Image)
        Me.btnImportFromExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnImportFromExcel.Location = New System.Drawing.Point(368, 465)
        Me.btnImportFromExcel.Name = "btnImportFromExcel"
        Me.btnImportFromExcel.Size = New System.Drawing.Size(168, 23)
        Me.btnImportFromExcel.TabIndex = 204
        Me.btnImportFromExcel.Text = "ورود اطلاعات از فايل اکسل"
        Me.btnImportFromExcel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnImportFromExcel.Visible = False
        '
        'dgtcRecloserFunctionId
        '
        Me.dgtcRecloserFunctionId.Format = ""
        Me.dgtcRecloserFunctionId.FormatInfo = Nothing
        Me.dgtcRecloserFunctionId.MappingName = "RecloserFunctionId"
        Me.dgtcRecloserFunctionId.Width = 0
        '
        'dgtcReadTime
        '
        Me.dgtcReadTime.Format = ""
        Me.dgtcReadTime.FormatInfo = Nothing
        Me.dgtcReadTime.HeaderText = "زمان قرائت "
        Me.dgtcReadTime.MappingName = "ReadTime"
        Me.dgtcReadTime.Width = 60
        '
        'frmBaseTablesNotStd
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.ClientSize = New System.Drawing.Size(723, 539)
        Me.Controls.Add(Me.btnImportSyncFromExcel)
        Me.Controls.Add(Me.btnImportFromExcel)
        Me.Controls.Add(Me.btnRecloserFunction)
        Me.Controls.Add(Me.lblSearch)
        Me.Controls.Add(Me.bttn_LPFeederLoad)
        Me.Controls.Add(Me.btnPointsAndEarthFeeder)
        Me.Controls.Add(Me.btnShowOnGISMap)
        Me.Controls.Add(Me.cboArea)
        Me.Controls.Add(Me.pnlEkipNetworkType)
        Me.Controls.Add(Me.btnZoneMPFeeders)
        Me.Controls.Add(Me.btnLPPostEarthPoint)
        Me.Controls.Add(Me.btnSearchView)
        Me.Controls.Add(Me.pnlSearch)
        Me.Controls.Add(Me.btnGroupChekList)
        Me.Controls.Add(Me.lblDesc_btnChangeLPFeederArea)
        Me.Controls.Add(Me.lblDesc_btnChangePostArea)
        Me.Controls.Add(Me.btnFeederPart)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.ButtonAdd)
        Me.Controls.Add(Me.ButtonDelete)
        Me.Controls.Add(Me.BttnReturn)
        Me.Controls.Add(Me.BttnSelect)
        Me.Controls.Add(Me.ButtonEditField)
        Me.Controls.Add(Me.BttnPartType)
        Me.Controls.Add(Me.btnChangePostArea)
        Me.Controls.Add(Me.chkIsShowParts)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.bttn_LPPost)
        Me.Controls.Add(Me.bttn_MPFeederList)
        Me.Controls.Add(Me.bttn_LPFeeder)
        Me.Controls.Add(Me.Bttn_DisconnectReasonXPReason)
        Me.Controls.Add(Me.btnActive_DeActive)
        Me.Controls.Add(Me.btnChangeLPFeederArea)
        Me.Controls.Add(Me.btnPartFactories)
        Me.Controls.Add(Me.pnlPartTypeEdit)
        Me.Controls.Add(Me.btnErjaCause)
        Me.Controls.Add(Me.btnCheckListTajhizat)
        Me.Controls.Add(Me.Bttn_DisconnectReason)
        Me.Controls.Add(Me.btnZonePreCodes)
        Me.Controls.Add(Me.btnSubCheckList)
        Me.Controls.Add(Me.btnImportExcel)
        Me.Controls.Add(Me.btnSavabeghBazdid)
        Me.Controls.Add(Me.btnCheckListPart)
        Me.Controls.Add(Me.BttnMPLPPart)
        Me.Controls.Add(Me.bttn_LPPostLoad)
        Me.Controls.Add(Me.Bttn_MPPostTrans)
        Me.Controls.Add(Me.grpSearch)
        Me.Controls.Add(Me.grpSearchGlobal)
        Me.Controls.Add(Me.dg)
        Me.Controls.Add(Me.btn_LPPostEarth)
        Me.Controls.Add(Me.lblNotIsAllowSave)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.HelpMaker.SetHelpNavigator(Me, System.Windows.Forms.HelpNavigator.Topic)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(576, 493)
        Me.Name = "frmBaseTablesNotStd"
        Me.HelpMaker.SetShowHelp(Me, True)
        Me.ShowInTaskbar = False
        Me.Text = "جداول پايه"
        CType(Me.DatasetCcReq1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dg, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DatasetCcReqView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpSearch.ResumeLayout(False)
        Me.grpSearch.PerformLayout()
        CType(Me.DatasetCcReq2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlMPPost_Down.ResumeLayout(False)
        Me.pnlMPPost_Down.PerformLayout()
        Me.pnlPeakBar.ResumeLayout(False)
        Me.pnlPeakBar.PerformLayout()
        Me.pnlLighFeeder.ResumeLayout(False)
        Me.pnlLighFeeder.PerformLayout()
        Me.pnlLPPostYear.ResumeLayout(False)
        Me.pnlLPPostYear.PerformLayout()
        CType(Me.DatasetBazdid1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DatasetBazdidView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DatasetErja1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpSearchGlobal.ResumeLayout(False)
        Me.grpSearchGlobal.PerformLayout()
        CType(Me.DatasetTamir1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlPartTypeEdit.ResumeLayout(False)
        Me.pnlSearch.ResumeLayout(False)
        Me.pnlSearch.PerformLayout()
        CType(Me.DatasetBT1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlEkipNetworkType.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

#Region "Hashemi"

    Private Sub AdjustControlsTabOrder()
        AdjustTabOrdersFromString(Me.Controls, "dg,ButtonAdd,ButtonEditField,Bttn_DisconnectReason,Bttn_DisconnectReasonXPReason,bttn_LPPost,bttn_MPFeederList,bttn_LPFeeder,ButtonDelete,BttnSelect,bttn_LPFeederLoad,btnLPPostEarthPoint,bttn_LPPostLoad,Bttn_MPPostTrans,cboArea,BttnMPLPPart,BttnPartType,BttnReturn")
    End Sub

#End Region

#Region "Events"
    Private Sub frmBaseTablesNotStd_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ApplySkin(Me)

        mIsLoading = True

        AdjustSearchSection()
        Dim lSql As String = ""
        Dim lDs As New DataSet

        Dim lTableId As TableId = TableId.tbl_Nothing

        If grpSearch.Visible Then
            LoadAreaDataTable(mCnn, DatasetCcReq2, cmbArea)
            cmbArea.SelectedValue = IIf(mAreaId = -1, WorkingAreaId, mAreaId)
            If IsCenter Then
                cmbArea.Enabled = True
            End If
            cmbMPPost.SelectedIndex = -1

            BindingTable("SELECT * FROM Tbl_Ownership", mCnn, DatasetCcReq2, "Tbl_Ownership", cmbOwnership)
        End If

        If SelectMode Then
            BttnSelect.Visible = True
        Else
            BttnSelect.Visible = False
        End If

        Dim lIsAccess As Boolean
        Dim lTag As String = ""
        Dim lSysObj As Integer = 0
        If BT_Name <> "Tbl_Peymankar" Then
            CheckUserTrustee(Me, ButtonAdd, 9)
            CheckUserTrustee(Me, ButtonEditField, 9)
            CheckUserTrustee(Me, ButtonDelete, 9)
        Else
            If BT_Name = "Tbl_Peymankar" Then
                lIsAccess = IsAdmin() OrElse CheckUserTrustee("CanSavePeymankar", 30)
                ButtonAdd.Enabled = lIsAccess
                ButtonEditField.Enabled = lIsAccess
                ButtonDelete.Enabled = lIsAccess
            End If
        End If
        '*CheckUserTrustee(Me.Name & "_" & ButtonAdd.Name, 9)
        '*CheckUserTrustee(Me.Name & "_" & ButtonEditField.Name, 9)
        '*CheckUserTrustee(Me.Name & "_" & ButtonDelete.Name, 9)
        If BT_Name = "ViewLPPostLoad" Or BT_Name = "ViewLPFeederLoad" Then
            btnImportFromExcel.Visible = True
            btnImportSyncFromExcel.Visible = True
        End If
        Select Case BT_Name
            Case "Tbl_LPPost", "Tbl_LPPostCommonFeeder", "Tbl_LPPostCommonLightFeeder"
                lTag = "frmBS_LPPostButtonSave"
                lSysObj = 9
                If BT_Name = "Tbl_LPPost" Then
                    lTableId = TableId.tbl_LPPost
                End If
            Case "Tbl_LPFeeder", "Tbl_LPFeederCommonFeeder", "Tbl_LightFeeder", "Tbl_LightFeederCommonFeeder"
                lTag = "frmBS_LPFeederButtonSave"
                lSysObj = 9
                If BT_Name = "Tbl_LPFeeder" Or BT_Name = "Tbl_LightFeeder" Then
                    lTableId = TableId.tbl_LPFeeder
                End If
            Case "ViewLPPostLoad"
                lTag = "frmBS_LPPostLoadBttnSave"
                lSysObj = 21
            Case "ViewLPPostEarth"
                lTag = "frmBS_LPPostLoadBttnSave"
                lSysObj = 21
            Case "ViewLPPostPointEarth"
                lTag = "frmBS_LPPostLoadBttnSave"
                lSysObj = 21
            Case "ViewLPFeederLoad"
                lTag = "frmBS_LPFeederLoadBttnSave"
                lSysObj = 21
            Case "ViewLPFeederPointsInfo"
                lTag = "frmBS_LPFeederPointLoadbtnSave"
                lSysObj = 21
            Case "Tbl_ErjaReason"
                lSysObj = 9
                lTag = "frmBS_ErjaReasonbtnSave"
            Case "Tbl_ErjaCause"
                lSysObj = 9
                lTag = "frmBS_ErjaReasonbtnSave"
            Case "Tbl_ErjaOperation"
                lSysObj = 9
                lTag = "frmBS_ErjaOperationbtnSave"
            Case "Tbl_ErjaPart"
                lSysObj = 9
                lTag = "frmBS_ErjaPartbtnSave"
            Case "VIEW_PartType"
                chkIsShowParts.Show()
                chkIsShowParts_CheckedChanged(sender, e)
            Case "Tbl_MPFeeder"
                lTag = "frmBS_MPFeederButtonSave"
                lSysObj = 9
                lTableId = TableId.tbl_MPFeeder
            Case "View_MPPost"
                lTag = "frmBS_MPPostButtonSave"
                lSysObj = 9
                lTableId = TableId.tbl_MPPost
            Case "Tbl_FeederPart"
                lTag = "frmBS_FeederPartbtnSave"
                lSysObj = 9
            Case "View_MPPostTrans"
                lTag = "frmBS_MPPostTransButtonSave"
                lSysObj = 9
                lTableId = TableId.tbl_MPostTrans
            Case "Tbl_AVLCar"
                lTag = "frmBS_AVLCarButtonSave"
                lSysObj = 9
            Case "Tbl_MPFeederKey"
                lTag = "frmBS_MPFeederKeyButtonSave"
                lSysObj = 9
                lSql = " SELECT * FROM tbl_MPCloserType "
                BindingTable(lSql, mCnn, lDs, "tbl_MPCloserType")
                cmbMPCloserType.DataSource = lDs.Tables("tbl_MPCloserType")
                cmbMPCloserType.DisplayMember = "MPCloserType"
                cmbMPCloserType.ValueMember = "MPCloserTypeId"
                cmbMPCloserType.SelectedIndex = -1
                lTableId = TableId.tbl_MPFeederKey
                btnRecloserFunction.Visible = True
                btnRecloserFunction.Enabled = True
            Case "Tbl_Village"
                lTag = "frmBS_VillageButtonSave"
                lSysObj = 9
            Case "Tbl_Section"
                lTag = "frmBS_VillageButtonSave"
                lSysObj = 9
            Case "Tbl_Town"
                lTag = "frmBS_VillageButtonSave"
                lSysObj = 9
            Case "Tbl_Roosta"
                lTag = "frmBS_VillageButtonSave"
                lSysObj = 9
            Case "Tbl_AllowValidator"
                lTag = "frmBS_AllowValidatorButtonSave"
                lSysObj = 9
        End Select

        If lSysObj > 0 Then
            lIsAccess = CheckUserTrustee(lTag, lSysObj) Or IsAdmin()
            ButtonAdd.Enabled = lIsAccess
            ButtonDelete.Enabled = lIsAccess
            ButtonEditField.Enabled = lIsAccess
        End If

        SetButtonsEnableState()
        If Not grpSearch.Visible Then
            LoadData()
        End If

        cmbActiveStatePostFeeder.SelectedIndex = 0
        AdjustControlsTabOrder()
        mIsLoading = False

        If grpSearch.Visible Then
            If IsCenter Then
                cmbArea_SelectedIndexKeyboardChanged(sender, e)
            Else
                cmbArea_SelectedIndexChanged(sender, e)
            End If
            mIsLoading = True
            BtnSearch_Click(sender, e)
        End If

        lIsAccess = IsAdmin() OrElse CheckUserTrustee("frmBS_LPPostLoadbtnDelete", 21)
        If TableName = "ViewLPPostLoad" And Not lIsAccess Then
            ButtonDelete.Enabled = False
        End If
        If (TableName = "ViewLPPostEarth" Or TableName = "ViewLPPostPointEarth") And Not lIsAccess Then
            ButtonDelete.Enabled = False
        End If

        lIsAccess = CheckUserTrustee("frmBS_LPFeederLengthEdit", 9) OrElse IsAdmin()
        If TableName = "View_LPFeeder" And lIsAccess Then
            ButtonEditField.Enabled = True

            lTableId = TableId.tbl_LPFeeder

        End If

        lIsAccess = CheckUserTrustee("frmBS_MPFeederLengthEdit", 9) OrElse IsAdmin()
        If TableName = "View_MPFeeder" And lIsAccess Then
            ButtonEditField.Enabled = True

            lTableId = TableId.tbl_MPFeeder

        End If

        lIsAccess = CheckUserTrustee("frmBS_LPFeederPeakEdit", 9) OrElse IsAdmin()
        If TableName = "View_LPFeeder" And lIsAccess Then
            ButtonEditField.Enabled = True

            lTableId = TableId.tbl_LPFeeder

        End If

        mIsShowActiveEkipOnly = CConfig.ReadConfig("IsShowActiveEkipOnly", "False")

        mIsDisableAddButton = False
        Select Case lTableId
            Case TableId.tbl_LPFeeder
                mTableNameText = "فيدرهاي فشار ضعيف"
                mIsDisableAddButton = Convert.ToBoolean(CConfig.ReadConfig("IsDisableInsertLPFeederFromApp", False))
            Case TableId.tbl_LPPost
                mTableNameText = "پست هاي توزيع"
                mIsDisableAddButton = Convert.ToBoolean(CConfig.ReadConfig("IsDisableInsertLPPostFromApp", False))
            Case TableId.tbl_MPFeeder
                mTableNameText = "فيدرهاي فشار متوسط"
                mIsDisableAddButton = Convert.ToBoolean(CConfig.ReadConfig("IsDisableInsertMPFeederFromApp", False))
            Case TableId.tbl_MPFeederKey
                mTableNameText = "کليدهاي فشار متوسط"
                mIsDisableAddButton = Convert.ToBoolean(CConfig.ReadConfig("IsDisableInsertMPFeederKeyFromApp", False))
            Case TableId.tbl_MPostTrans
                mTableNameText = "ترانس هاي فوق توزيع"
                mIsDisableAddButton = Convert.ToBoolean(CConfig.ReadConfig("IsDisableInsertMPPostTransFromApp", False))
            Case TableId.tbl_MPPost
                mTableNameText = "پست هاي فوق توزيع"
                mIsDisableAddButton = Convert.ToBoolean(CConfig.ReadConfig("IsDisableInsertMPPostFromApp", False))
            Case Else

        End Select

        mIsLoading = False
    End Sub
    Private Sub ButtonDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDelete.Click
        Dim row = dg.CurrentRowIndex
        If row < 0 Then
            Exit Sub
        End If
        If BT_Name = "Tbl_LPPostCommonFeeder" Or BT_Name = "Tbl_LPPostCommonLightFeeder" Then
            If Not (IsSetadMode OrElse (IsCenter And Not IsSpecialCenter)) Then
                If IsCenter Then
                    If ("," & WorkingCenterAreaIDs & ",").IndexOf("," & dg.Item(row, 12) & ",") = -1 Then
                        ShowError("شما اجازه حذف پست مربوط به ناحيه ديگر را نداريد")
                        Exit Sub
                    End If
                Else
                    If dg.Item(row, 12) <> cmbArea.SelectedValue Then
                        ShowError("شما اجازه حذف پست مربوط به ناحيه ديگر را نداريد")
                        Exit Sub
                    End If
                End If
            End If
        End If
        If BT_Name = "Tbl_LPFeederCommonFeeder" Or BT_Name = "Tbl_LightFeederCommonFeeder" Then
            If Not (IsSetadMode OrElse (IsCenter And Not IsSpecialCenter)) Then
                If IsCenter Then
                    If ("," & WorkingCenterAreaIDs & ",").IndexOf("," & dg.Item(row, 13) & ",") = -1 Then
                        ShowError("شما اجازه حذف فيدر مربوط به ناحيه ديگر را نداريد")
                        Exit Sub
                    End If
                Else
                    If dg.Item(row, 13) <> IIf(mAreaId = -1, cmbArea.SelectedValue, mAreaId) Then
                        ShowError("شما اجازه حذف فيدر مربوط به ناحيه ديگر را نداريد")
                        Exit Sub
                    End If
                End If
            End If
        End If

        Dim UserAns As DialogResult = MsgBoxF("آيا از حذف اين ركورد اطمينان داريد؟", "حصول اطمينان", MessageBoxButtons.YesNo, MsgBoxIcon.MsgIcon_Question, MessageBoxDefaultButton.Button2)
        If UserAns = DialogResult.No Then
            Exit Sub
        End If
        Dim ID As Long = dg.Item(row, 0)
        Dim SQL As String = "Select * from "
        Dim tblName As String = ""
        Select Case BT_Name
            Case "View_Area"
                tblName = "Tbl_Area"
                SQL &= tblName & " where AreaId=" & ID
            Case "View_LPCommonFeeder"
                tblName = "Tbl_LPCommonFeeder"
                SQL &= tblName & " where LPCommonFeederId=" & ID
            Case "View_MPCommonFeeder"
                tblName = "Tbl_MPCommonFeeder"
                SQL &= tblName & " where MPCommonFeederId=" & ID
            Case "ViewCriticalFeeder"
                tblName = "TblCriticalFeeder"
                SQL &= tblName & " where CriticalFeederId=" & ID
            Case "View_DisconnectGroupLP"
                tblName = "Tbl_DisconnectGroupSet"
                SQL &= tblName & " where DisconnectGroupSetID=" & ID
            Case "View_DisconnectGroupLight"
                tblName = "Tbl_DisconnectGroupSet"
                SQL &= tblName & " where DisconnectGroupSetID=" & ID
            Case "View_DisconnectGroupMP"
                tblName = "Tbl_DisconnectGroupSet"
                SQL &= tblName & " where DisconnectGroupSetID=" & ID
            Case "View_City"
                tblName = "Tbl_City"
                SQL &= tblName & " where  CityID=" & ID
            Case "Tbl_Master"
                tblName = "Tbl_Master"
                SQL &= tblName & " where  MasterID=" & ID
            Case "View_MPPost"
                tblName = "Tbl_MPPost"
                SQL &= tblName & " where  MPPostID=" & ID
            Case "Tbl_LPPart"
                tblName = BT_Name
                SQL &= tblName & " where  LPPartID=" & ID
            Case "Tbl_MPPart"
                tblName = BT_Name
                SQL &= tblName & " where  MPPartID=" & ID
            Case "Tbl_LightPart"
                tblName = BT_Name
                SQL &= tblName & " where  LightPartID=" & ID
            Case "Tbl_MPFeeder"
                tblName = "Tbl_MPFeeder"
                SQL &= tblName & " where  MPFeederID=" & ID
            Case "Tbl_FeederPart"
                tblName = "Tbl_FeederPart"
                SQL &= tblName & " WHERE FeederPartId = " & ID
            Case "Tbl_LPPost", "Tbl_LPPostCommonFeeder", "Tbl_LPPostCommonLightFeeder"
                tblName = "Tbl_LPPost"
                SQL &= tblName & " where  LPPostID=" & ID
            Case "Tbl_LPFeeder", "Tbl_LPFeederCommonFeeder", "Tbl_LightFeeder", "Tbl_LightFeederCommonFeeder"
                tblName = "Tbl_LPFeeder"
                SQL &= tblName & " where  LPFeederID=" & ID
            Case "TblEkipProfile"
                tblName = "TblEkipProfile"
                SQL &= tblName & " where  EkipProfileID=" & ID
            Case "View_MPPostTrans"
                tblName = "Tbl_MPPostTrans"
                SQL &= tblName & " where  MPPostTransId=" & ID
            Case "ViewLPPostLoad"
                tblName = "TblLPPostLoad"
                SQL &= tblName & " where  MPFeederKeyId=" & ID
            Case "TblRecloserFunction"
                tblName = "TblRecloserFunction"
                SQL &= tblName & " where  RecloserFunctionId=" & ID
            Case "ViewLPPostEarth", "ViewLPPostPointEarth"
                tblName = "TblLPPostEarthInfo"
                SQL &= tblName & " where  LPPostEarthInfoId=" & ID
            Case "ViewLPFeederLoad"
                tblName = "TblLPFeederLoad"
                SQL &= tblName & " where  LPFeederLoadId=" & ID
            Case "ViewLPFeederPointsInfo"
                tblName = "TblLPFeederPointsInfo"
                SQL &= tblName & " where  LPFeederPointsInfoId=" & ID
            Case "ViewMPFeederPointsInfo"
                tblName = "TblMPFeederPointsInfo"
                SQL &= tblName & " where  MPFeederPointsInfoId=" & ID
            Case "VIEW_PartType"
                tblName = "Tbl_PartType"
                SQL &= tblName & " where  PartTypeId=" & ID
            Case "VIEW_MPPartType"
                tblName = "Tbl_MPPartType"
                SQL &= tblName & " where  MPPartTypeId=" & ID
            Case "VIEW_LPPartType"
                tblName = "Tbl_LPPartType"
                SQL &= tblName & " where  LPPartTypeId=" & ID
            Case "Tbl_Zone"
                tblName = "Tbl_Zone"
                SQL &= tblName & " where  ZoneId = " & ID
            Case "Tbl_IVRCode"
                tblName = "Tbl_IVRCode"
                SQL &= tblName & " where  IVRCodeId = " & ID
            Case "Tbl_Section"
                tblName = "Tbl_Section"
                SQL &= tblName & " where  SectionId = " & ID
            Case "Tbl_Town"
                tblName = "Tbl_Town"
                SQL &= tblName & " where  TownId = " & ID
            Case "Tbl_Village"
                tblName = "Tbl_Village"
                SQL &= tblName & " where  VillageId = " & ID
            Case "Tbl_Roosta"
                tblName = "Tbl_Roosta"
                SQL &= tblName & " where  RoostaId = " & ID
            Case "Tbl_AVLCar"
                tblName = "Tbl_AVLCar"
                SQL &= tblName & " where  AVLCarID = " & ID
            Case "Tbl_MPFeederKey"
                tblName = "Tbl_MPFeederKey"
                SQL &= tblName & " where  MPFeederKeyId = " & ID
            Case "Tbl_AllowValidator"
                tblName = "Tbl_AllowValidator"
                SQL &= tblName & " where  AllowValidatorId = " & ID
                '--------------------------------------
                '       Bazdid Applicatin Part
                '--------------------------------------
            Case "View_BazdidCheckList"
                tblName = "BTbl_BazdidCheckList"
                SQL &= tblName & " where  BazdidCheckListId = " & ID
            Case "View_BazdidCheckListSubGroup"
                tblName = "BTbl_BazdidCheckList"
                SQL &= tblName & " where  BazdidCheckListId = " & ID
            Case "BTbl_BazdidCheckListGroup"
                tblName = "BTbl_BazdidCheckListGroup"
                SQL &= tblName & " where  BazdidCheckListGroupId = " & ID
            Case "BTbl_SubCheckList"
                tblName = "BTbl_SubCheckList"
                SQL &= tblName & " where SubCheckListId = " & ID
            Case "BTbl_Tajhizat"
                tblName = "BTbl_Tajhizat"
                SQL &= tblName & " where TajhizatId = " & ID
            Case "BTbl_ServicePart"
                tblName = "BTbl_ServicePart"
                SQL &= tblName & " where ServicePartId = " & ID
            Case "BTblBazdidEkip"
                tblName = "BTblBazdidEkip"
                SQL &= tblName & " where  BazdidEkipId=" & ID
            Case "BTbl_BazdidMaster"
                tblName = "BTbl_BazdidMaster"
                SQL &= tblName & " where  BazdidMasterId=" & ID
            Case "BTbl_BazdidCheckListSubGroup"
                tblName = "BTbl_BazdidCheckListSubGroup"
                SQL &= tblName & " where  BazdidCheckListSubGroupId = " & ID
            Case "BTbl_LPPostDetail"
                tblName = "BTbl_LPPostDetail"
                SQL &= tblName & " where  LPPostDetailId = " & ID
            Case "Tbl_DisconnectGroupSetGroup"
                tblName = "Tbl_DisconnectGroupSetGroup"
                SQL &= tblName & " where DisconnectGroupSetGroupId = " & ID
                '--------------------------------------
                '       Erja Applicatin Part
                '--------------------------------------
            Case "Tbl_ErjaReason"
                If ID >= 1000 And ID < 1999 Then
                    ShowError("دليل ارجاع انتخاب شده، از دستورالعمل‌هاي شرکت توانير بوده و قابل حذف نمي‌باشد.")
                    Exit Sub
                End If
                tblName = "Tbl_ErjaReason"
                SQL &= tblName & " where ErjaReasonId = " & ID
            Case "Tbl_ErjaCause"
                tblName = "Tbl_ErjaCause"
                SQL &= tblName & " where ErjaCauseId = " & ID
            Case "Tbl_ErjaPart"
                tblName = "Tbl_ErjaPart"
                SQL &= tblName & " where ErjaPartId = " & ID
            Case "Tbl_ErjaOperation"
                tblName = "Tbl_ErjaOperation"
                SQL &= tblName & " where ErjaOperationId = " & ID
                '--------------------------------------
                '    Tamir Request Applicatin Part
                '--------------------------------------
            Case "Tbl_TamirOperation"
                tblName = "Tbl_TamirOperation"
                SQL &= tblName & " where TamirOperationId = " & ID
            Case "Tbl_ManoeuvreType"
                tblName = "Tbl_ManoeuvreType"
                SQL &= tblName & " where ManoeuvreTypeId = " & ID
            Case "Tbl_Peymankar"
                tblName = "Tbl_Peymankar"
                SQL &= tblName & " where PeymankarId = " & ID
        End Select
        'DatasetCcReq1.Tables(tblName).Clear()
        Dim Ds As New DataSet
        BindingTable(SQL, mCnn, Ds, tblName)
        If Ds.Tables(tblName).Rows.Count > 0 Then
            If tblName = "Tbl_MPCommonFeeder" Then
                Dim i As Integer
                Dim dt As DataTable = Ds.Tables(tblName)
                'tblName <> "Tbl_MPCommonFeeder" Added at 83/03/26
                If tblName <> "TblMPRequestPart" And tblName <> "TblLPTamir" And tblName <> "TblBazdid" And tblName <> "Tbl_MPFeederLoadHours" And tblName <> "Tbl_CunsumeLoadFactorHours" And tblName <> "TblCalcPower" And tblName <> "Tbl_MPCommonFeeder" Then
                    For i = 0 To dt.Rows.Count - 1
                        If dt.Rows(i).Item("AreaID") = WorkingAreaId Then
                            Ds.Tables(tblName).Rows(0).Delete()
                            Dim Dlg As New frmUpdateDataSetBT
                            If Dlg.UpdateDataSet(tblName, Ds) Then
                                LoadData()
                            End If
                        Else
                            Me.Cursor.Current = Cursors.Default
                            MsgBoxF("امكان ذخيره اطلاعات براي ناحيه اي بجز ناحيه جاري وجود ندارد", "خطا", MessageBoxButtons.OK, MsgBoxIcon.MsgIcon_Exclamation, MessageBoxDefaultButton.Button1)
                            Exit Sub
                        End If
                    Next
                End If
                Ds.Tables(tblName).Rows(0).Delete()
                Dim DlgUpd As New frmUpdateDataSetBT
                If DlgUpd.UpdateDataSet(tblName, Ds) Then
                    LoadData()
                End If

            ElseIf tblName = "BTbl_SubCheckList" Then
                Dim lSQL As String
                Dim lTrans As SqlTransaction
                Dim lIsSaveOk As Boolean = False
                Dim lUpdateBTblBazdidResultCheckList As New frmUpdateDataset
                Dim lUpdateBTblSubCheckList As New frmUpdateDataSetBT
                If MsgBoxF("با حذف اين آيتم، کليه عيبهايي که با " &
                            "اين ريز خرابي ثبت شده اند به حالت بدون " &
                            "ريز خرابي تغيير وضعيت خواهند داد و گزارشات " &
                            "مرتبطي که قبلا تهيه شده اند، ديگر معتبر نخواهند بود. " &
                            "آيا ادامه مي‌دهيد ؟", "هشدار", MessageBoxButtons.YesNo, MsgBoxIcon.MsgIcon_Warning, MessageBoxDefaultButton.Button1) Then

                    Dim lBazdidAreaId As Integer = WorkingAreaId
                    Try
                        'lSQL = _
                        '    "SELECT BTblBazdidTiming.AreaId " & _
                        '    "FROM " & _
                        '    "	BTblBazdidResultCheckList " & _
                        '    "	INNER JOIN BTblBazdidResultAddress ON BTblBazdidResultCheckList.BazdidResultAddressId = BTblBazdidResultAddress.BazdidResultAddressId " & _
                        '    "	INNER JOIN BTblBazdidResult ON BTblBazdidResultAddress.BazdidResultId = BTblBazdidResult.BazdidResultId " & _
                        '    "	INNER JOIN BTblBazdidTiming ON BTblBazdidResult.BazdidTimingId = BTblBazdidTiming.BazdidTimingId " & _
                        '    "WHERE " & _
                        '    "	BTblBazdidResultCheckList.SubCheckListId = " & Ds.Tables(tblName).Rows(0)("SubCheckListId")
                        'BindingTable(lSQL, mCnn, Ds, "ViewCheckListArea", , True, , , , , True)
                        'If Ds.Tables("ViewCheckListArea").Rows.Count > 0 Then
                        '    lBazdidAreaId = Ds.Tables("ViewCheckListArea").Rows(0)("AreaId")
                        'End If

                        lSQL = "SELECT * FROM BTblBazdidResultCheckList WHERE SubCheckListId = " & Ds.Tables(tblName).Rows(0)("SubCheckListId")
                        BindingTable(lSQL, mCnn, Ds, "BTblBazdidResultCheckList", , , , , , , True)
                        For Each lRow As DataRow In Ds.Tables("BTblBazdidResultCheckList").Rows
                            lRow("SubCheckListId") = DBNull.Value
                        Next

                        If mCnn.State <> ConnectionState.Open Then
                            mCnn.Open()
                        End If
                        lTrans = mCnn.BeginTransaction()

                        lIsSaveOk = lUpdateBTblBazdidResultCheckList.UpdateDataSet("BTblBazdidResultCheckList", Ds, lBazdidAreaId, , , lTrans)
                        If lIsSaveOk Then
                            Ds.Tables(tblName).Rows(0).Delete()
                            lIsSaveOk = lUpdateBTblSubCheckList.UpdateDataSet("BTbl_SubCheckList", Ds, , , lTrans)
                        End If

                        If lIsSaveOk Then
                            lTrans.Commit()
                            Dim lUpdateInfo As New frmInformUserAction("ريزخرابي با موفقيت حذف گرديد")
                            lUpdateInfo.ShowDialog()
                            lUpdateInfo.Dispose()
                            LoadData()
                        Else
                            lTrans.Rollback()
                        End If
                    Catch ex As Exception
                        If Not IsNothing(lTrans) Then
                            lTrans.Rollback()
                        End If
                        ShowError(ex)
                    End Try
                    If mCnn.State = ConnectionState.Open Then
                        mCnn.Close()
                    End If

                End If

            Else
                Ds.Tables(tblName).Rows(0).Delete()
                If tblName = "TblCriticalFeeder" Then
                    Dim DlgUpd As New frmUpdateDataset
                    If DlgUpd.UpdateDataSet(tblName, Ds, Nothing) Then
                        LoadData()
                    End If
                ElseIf tblName = "Tbl_Zone" Or tblName = "Tbl_Village" Or tblName = "Tbl_IVRCode" Then
                    Dim DlgUpd As New frmUpdateDataSetBT
                    If DlgUpd.UpdateDataSet(tblName, Ds, Nothing) Then
                        LoadData()
                    End If
                Else
                    If tblName = "TblEkipProfile" Or tblName = "BTblBazdidEkip`" Then
                        Dim DlgUpd As New frmUpdateDataset
                        If DlgUpd.UpdateDataSet(tblName, Ds, WorkingAreaId) Then
                            LoadData()
                        End If
                    Else
                        Dim DlgUpd As New frmUpdateDataSetBT
                        If DlgUpd.UpdateDataSet(tblName, Ds) Then
                            LoadData()
                        End If
                    End If
                End If
            End If
        Else
            LoadData()
        End If
    End Sub
    Private Sub ButtonAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAdd.Click, btnNewSerghatiPart.Click
        If BT_Name = "VIEW_PartType" Then mIsEditParts = CType(sender, Button).Name = "btnNewSerghatiPart"

        If mIsDisableAddButton Then
            lblNotIsAllowSave.Text = lblNotIsAllowSave.Text.Replace("###", mTableNameText)
            ShowError(lblNotIsAllowSave.Text)
            Exit Sub
        End If

        AddOrEdit(-1)
    End Sub
    Private Sub ButtonEditField_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEditField.Click, btnEditSerghatiPart.Click
        Dim row = dg.CurrentRowIndex
        If row < 0 Then
            Exit Sub
        End If

        If BT_Name = "Tbl_MPFeederKey" Then
            Dim ID As Long = dg.Item(row, 0)
            AddOrEdit(ID)
        Else
            Dim ID As Long = dg.Item(row, 0)
            If BT_Name = "VIEW_PartType" Then mIsEditParts = CType(sender, Button).Name = "btnEditSerghatiPart"
            AddOrEdit(ID)
        End If
    End Sub
    Private Sub BttnSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BttnSelect.Click
        Dim lRowIndex As Object
        lRowIndex = dg.CurrentRowIndex
        If lRowIndex < 0 Then
            Exit Sub
        End If
        SelectedID = dg.Item(lRowIndex, 0)
        SelectedName = dg.Item(lRowIndex, mItemNameCol)

        mSelectedIDs = ""
        mSelectedNames = ""
        lRowIndex = 0
        Do
            Try
                If dg.IsSelected(lRowIndex) Then
                    mSelectedIDs &= IIf(mSelectedIDs <> "", ",", "") & dg(lRowIndex, 0)
                    mSelectedNames &= IIf(mSelectedNames <> "", ",", "") & dg(lRowIndex, mItemNameCol)
                End If
                lRowIndex += 1
            Catch ex As Exception
                Exit Do
            End Try
        Loop

        If mSelectedIDs = "" Then
            mSelectedIDs = SelectedID
            mSelectedNames = SelectedName
        End If

        DialogResult = DialogResult.OK
        Me.Close()
    End Sub
    Private Sub MPFeederList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bttn_MPFeederList.Click
        Dim row = dg.CurrentRowIndex
        If row < 0 Then
            Exit Sub
        End If
        Dim ID As Integer = dg.Item(row, 0)
        Dim dlg As New frmBaseTablesNotStd("Tbl_MPFeeder", , , ID, , , , , , , , , , , True)
        dlg.ShowDialog()
        dlg.Dispose()
        LoadData()
    End Sub
    Private Sub bttn_LPPost_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bttn_LPPost.Click
        Dim row = dg.CurrentRowIndex
        If row < 0 Then
            Exit Sub
        End If
        Dim ID As Integer = dg.Item(row, 0)
        Dim dlg As New frmBaseTablesNotStd("Tbl_LPPost", , , ID, , , , , , OwnerID, , , , , True)
        dlg.ShowDialog()
        dlg.Dispose()
        LoadData()
    End Sub
    Private Sub btnFeederPart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFeederPart.Click
        Dim row = dg.CurrentRowIndex
        If row < 0 Then
            Exit Sub
        End If

        If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.mnu_FeederPart) Then
            ShowHavadesInfoMessage()
            Exit Sub
        End If

        Dim ID As Integer = dg.Item(row, 0)
        Dim dlg As New frmBaseTablesNotStd("Tbl_FeederPart", , , ID)
        dlg.ShowDialog()
        dlg.Dispose()
        LoadData()
    End Sub
    Private Sub bttn_LPFeeder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bttn_LPFeeder.Click
        Dim row = dg.CurrentRowIndex
        If row < 0 Then
            Exit Sub
        End If
        Dim ID As Integer = dg.Item(row, 0)
        Dim dlg As frmBaseTablesNotStd
        If BT_Name = "Tbl_LPPostCommonFeeder" Then
            dlg = New frmBaseTablesNotStd("Tbl_LPFeederCommonFeeder", , , ID, , , , , , , , , , cmbArea.SelectedValue, True)
        ElseIf BT_Name = "Tbl_LPPostCommonLightFeeder" Then
            dlg = New frmBaseTablesNotStd("Tbl_LightFeederCommonFeeder", , , ID, , , , , , , , , , cmbArea.SelectedValue, True)
        Else
            dlg = New frmBaseTablesNotStd("Tbl_LPFeeder", , , ID, , , , , , , , , , , True)
        End If

        dlg.ShowDialog()
        dlg.Dispose()
        LoadData()
    End Sub
    Private Sub cboArea_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboArea.SelectedIndexChanged
        Dim SQL As String
        Dim lDs As DataSet
        Dim lIsWhere As String = ""
        Select Case TableName
            Case "TblEkipProfile"
                lDs = DatasetCcReq1
                DatasetCcReq1.TblEkipProfile.Clear()
                If rbIsLP.Checked Then
                    lIsWhere = " AND (IsLP = 1 OR IsLP IS NULL AND IsMP IS NULL)"
                ElseIf rbIsMP.Checked Then
                    lIsWhere = " AND (IsMP = 1 OR IsMP IS NULL AND IsLP IS NULL)"
                End If
                If cboArea.SelectedIndex > -1 Then
                    SQL = "SELECT * FROM TblEkipProfile Where ApplicationId = " & IIf(mApplicationType = mdlHashemi.ApplicationTypes.TamirRequest, ApplicationTypes.Havades, mApplicationType) & " AND AreaID=" & cboArea.SelectedValue & IIf(mIsShowActiveEkipOnly, " AND IsActive = 1", "") & lIsWhere & " ORDER BY EkipProfileName "
                Else
                    SQL = "SELECT * FROM TblEkipProfile Where ApplicationId = " & IIf(mApplicationType = mdlHashemi.ApplicationTypes.TamirRequest, ApplicationTypes.Havades, mApplicationType) & " AND " & WorkingCenterAreaQuery & IIf(mIsShowActiveEkipOnly, " AND IsActive = 1", "") & lIsWhere & " ORDER BY EkipProfileName "
                End If
            Case "Tbl_AVLCar"
                lDs = DatasetCcReq1
                DatasetCcReq1.Tbl_AVLCar.Clear()
                If cboArea.SelectedIndex > -1 Then
                    SQL = "select Tbl_AVLCar.*, Tbl_Area.Area FROM Tbl_AVLCar inner join Tbl_Area on Tbl_AVLCar.AreaId = Tbl_Area.AreaId Where Tbl_Area.AreaId=" & cboArea.SelectedValue
                Else
                    SQL = "select Tbl_AVLCar.*, Tbl_Area.Area FROM Tbl_AVLCar inner join Tbl_Area on Tbl_AVLCar.AreaId = Tbl_Area.AreaId where " & WorkingCenterAreaQuery
                End If
            Case "BTblBazdidEkip"
                lDs = DatasetBazdid1
                DatasetBazdid1.BTblBazdidEkip.Clear()
                If cboArea.SelectedIndex > -1 Then
                    SQL = "SELECT * FROM BTblBazdidEkip Where AreaID=" & cboArea.SelectedValue & " ORDER BY BazdidEkipName"
                Else
                    SQL = "SELECT * FROM BTblBazdidEkip Where " & WorkingCenterAreaQuery & " ORDER BY BazdidEkipName"
                End If
            Case "Tbl_Zone"
                lDs = DatasetCcReq1
                DatasetCcReq1.Tbl_Zone.Clear()
                If cboArea.SelectedIndex > -1 Then
                    SQL = "" &
                     " SELECT Tbl_Zone.ZoneId, Tbl_Area.Area, Tbl_Zone.ZoneName " &
                     " FROM Tbl_Zone INNER JOIN Tbl_Area ON Tbl_Zone.AreaId = Tbl_Area.AreaId" &
                     " WHERE Tbl_Zone.AreaId = " & cboArea.SelectedValue &
                     " ORDER BY Tbl_Area.Area, Tbl_Zone.ZoneName"
                Else
                    SQL = "" &
                     " SELECT Tbl_Zone.ZoneId, Tbl_Area.Area, Tbl_Zone.ZoneName " &
                     " FROM Tbl_Zone INNER JOIN Tbl_Area ON Tbl_Zone.AreaId = Tbl_Area.AreaId" &
                     " WHERE " & WorkingCenterAreaQuery.Replace("AreaId", "Tbl_Zone.AreaId") &
                     " ORDER BY Tbl_Area.Area, Tbl_Zone.ZoneName"
                End If
            Case "Tbl_Village"
                lDs = DatasetCcReq1
                DatasetCcReq1.Tbl_Village.Clear()
                If cboArea.SelectedIndex > -1 Then
                    SQL = "" &
                     " SELECT Tbl_Village.VillageId, Tbl_Area.Area, Tbl_Village.VillageName, Tbl_Village.VillageCode " &
                     " FROM Tbl_Village INNER JOIN Tbl_Area ON Tbl_Village.AreaId = Tbl_Area.AreaId" &
                     " WHERE Tbl_Village.AreaId = " & cboArea.SelectedValue &
                     " ORDER BY Tbl_Area.Area, Tbl_Village.VillageName"
                Else
                    SQL = "" &
                     " SELECT Tbl_Village.VillageId, Tbl_Area.Area, Tbl_Village.VillageName, Tbl_Village.VillageCode " &
                     " FROM Tbl_Village INNER JOIN Tbl_Area ON Tbl_Village.AreaId = Tbl_Area.AreaId" &
                     " WHERE " & WorkingCenterAreaQuery.Replace("AreaId", "Tbl_Village.AreaId") &
                     " ORDER BY Tbl_Area.Area, Tbl_Village.VillageName"
                End If
            Case "Tbl_Master"
                lDs = DatasetCcReq1
                DatasetCcReq1.Tbl_Master.Clear()
                If rbIsLP.Checked = True Then
                    lIsWhere = " AND (IsLP = 1 OR IsLP IS NULL AND IsMP IS NULL)"
                ElseIf rbIsMP.Checked = True Then
                    lIsWhere = " AND (IsMP = 1 OR IsLP IS NULL AND IsMP IS NULL)"
                End If
                If cboArea.SelectedIndex > -1 Then
                    SQL = "SELECT * FROM Tbl_Master Where ApplicationId = " & IIf(mApplicationType = mdlHashemi.ApplicationTypes.TamirRequest, ApplicationTypes.Havades, mApplicationType) & " AND AreaID = " & cboArea.SelectedValue & " " & lIsWhere
                Else
                    SQL = "SELECT * FROM Tbl_Master Where ApplicationId = " & IIf(mApplicationType = mdlHashemi.ApplicationTypes.TamirRequest, ApplicationTypes.Havades, mApplicationType) & " AND " & WorkingCenterAreaQuery & " " & lIsWhere
                End If
                If (SelectMode And Not IsShowAllMaster) Or mIsShowActiveEkipOnly Then
                    SQL &= " AND IsActive = 1 "
                End If
            Case "Tbl_AllowValidator"
                lDs = DatasetCcReq1
                DatasetCcReq1.Tables("Tbl_AllowValidator").Clear()
                If cboArea.SelectedIndex > -1 Then
                    SQL = "select Tbl_AllowValidator.*, ISNULL(Tbl_Area.Area, 'همه نواحی') AS Area FROM Tbl_AllowValidator LEFT join Tbl_Area on Tbl_AllowValidator.AreaId = Tbl_Area.AreaId Where Tbl_AllowValidator.AreaId IS NULL OR Tbl_Area.AreaId=" & cboArea.SelectedValue
                Else
                    SQL = "select Tbl_AllowValidator.*, ISNULL(Tbl_Area.Area, 'همه نواحی') AS Area FROM Tbl_AllowValidator LEFT join Tbl_Area on Tbl_AllowValidator.AreaId = Tbl_Area.AreaId where Tbl_AllowValidator.AreaId IS NULL OR " & WorkingCenterAreaQuery
                End If
            Case "BTbl_BazdidMaster"
                lDs = DatasetBazdid1
                DatasetBazdid1.BTbl_BazdidMaster.Clear()
                If cboArea.SelectedIndex > -1 Then
                    SQL = "SELECT * FROM BTbl_BazdidMaster Where AreaID = " & cboArea.SelectedValue
                Else
                    SQL = "SELECT * FROM BTbl_BazdidMaster Where " & WorkingCenterAreaQuery
                End If
                If SelectMode And Not IsShowAllMaster Then
                    SQL &= " AND IsActive = 1 "
                End If
        End Select
        BindingTable(SQL, mCnn, lDs, TableName)
        dg.DataSource = Nothing
        dg.DataSource = lDs
        dg.DataMember = TableName

        If BT_Name = "TblEkipProfile" Then
            dg.DataSource = DatasetCcReq1.TblEkipProfile.DefaultView
        End If

        If BT_Name = "Tbl_Master" Then
            dg.DataSource = DatasetCcReq1.Tbl_Master.DefaultView
        End If

        Try
            dg.CaptionText = System.Text.RegularExpressions.Regex.Replace(dg.CaptionText, ": [\d]*", ": " & lDs.Tables(TableName).Rows.Count)
        Catch ex As Exception
        End Try
    End Sub
    Private Sub frmBaseTablesNotStd_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.Control And e.Alt And e.Shift Then
            If e.KeyCode = Keys.Insert Then
                If IsAdmin() Then
                    ButtonAdd.Enabled = Not (ButtonAdd.Enabled)
                    ButtonEditField.Enabled = Not (ButtonEditField.Enabled)
                    ButtonDelete.Enabled = Not (ButtonDelete.Enabled)
                End If
            End If
        End If
    End Sub
    Private Sub Bttn_DisconnectReason_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bttn_DisconnectReason.Click
        Dim row = dg.CurrentRowIndex
        If row < 0 Then
            Exit Sub
        End If
        Dim Id As Integer = dg.Item(row, 0)
        Dim dlg As Form
        Select Case BT_Name
            Case "View_DisconnectGroupLP"
                dlg = New frmBS_DisconnectReason(Id)
            Case "View_DisconnectGroupLight"
                dlg = New frmBS_DisconnectReason(Id)
            Case "View_DisconnectGroupMP"
                dlg = New frmBS_DisconnectReason(Id)
        End Select
        If dlg.ShowDialog() = DialogResult.OK Then
            LoadData()
        End If
        dlg.Dispose()
    End Sub
    Private Sub Bttn_DisconnectReasonXPReason_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bttn_DisconnectReasonXPReason.Click
        Dim row = dg.CurrentRowIndex
        If row < 0 Then
            Exit Sub
        End If
        Dim Id As Integer = dg.Item(row, 0)
        Dim dlg As Form
        Select Case BT_Name
            Case "View_DisconnectGroupLP"
                dlg = New frmBS_DisconnectReasonReason(Id)
            Case "View_DisconnectGroupLight"
                dlg = New frmBS_DisconnectReasonReason(Id)
            Case "View_DisconnectGroupMP"
                dlg = New frmBS_DisconnectReasonReason(Id)
        End Select
        dlg.ShowDialog()
        'If dlg.ShowDialog() = DialogResult.OK Then
        '    LoadData()
        'End If
        dlg.Dispose()
    End Sub
    Private Sub Bttn_MPPostTrans_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bttn_MPPostTrans.Click
        Dim row = dg.CurrentRowIndex
        If row < 0 Then
            Exit Sub
        End If
        Dim ID As Integer = dg.Item(row, 0)
        Dim dlg As New frmBaseTablesNotStd("View_MPPostTrans", , , ID)
        dlg.ShowDialog()
        dlg.Dispose()
        LoadData()
    End Sub
    Private Sub bttn_LPPostLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bttn_LPPostLoad.Click
        If IsTozi("MazandaranWest") Then
            cmnuReports.Show(bttn_LPPostLoad, New Point(0, bttn_LPPostLoad.Height))
        Else
            mnuRegLPPostLoad_Click(sender, e)
        End If
    End Sub
    Private Sub mnuRegLPPostLoad_Click(sender As Object, e As EventArgs) Handles mnuRegLPPostLoad.Click
        Dim row = dg.CurrentRowIndex
        If row < 0 Then
            Exit Sub
        End If
        If BT_Name = "Tbl_LPPostCommonFeeder" Or BT_Name = "Tbl_LPPostCommonLightFeeder" Then
            If Not (IsSetadMode OrElse (IsCenter And Not IsSpecialCenter)) Then
                If IsCenter Then
                    If ("," & WorkingCenterAreaIDs & ",").IndexOf("," & dg.Item(row, 13) & ",") = -1 Then
                        ShowError("شما اجازه بارگيري پست مربوط به ناحيه ديگر را نداريد")
                        Exit Sub
                    End If
                Else
                    If dg.Item(row, 13) <> cmbArea.SelectedValue Then
                        ShowError("شما اجازه بارگيري پست مربوط به ناحيه ديگر را نداريد")
                        Exit Sub
                    End If
                End If
            End If
        End If

        Dim ID As Integer = dg.Item(row, 0)
        Dim dlg As New frmBaseTablesNotStd("ViewLPPostLoad", , , ID)
        dlg.ShowDialog()
        dlg.Dispose()
        LoadData()
    End Sub
    Private Sub mnuBlankLPPostExcel_Click(sender As Object, e As EventArgs) Handles mnuBlankLPPostExcel.Click
        Try
            Dim lExcel As CExcelManager = Nothing
            Dim lFileName As String
            lFileName = "LPPostLoadBlank.xlsx"
            If lExcel Is Nothing Then
                lExcel = New CExcelManager
            Else
                lExcel.CloseWorkBook()
            End If
            Dim lPath As String = VB6.GetPath & "\Reports\Excel\"
            Dim lDestPath As String = GetUserTempPath() & "Tac\Havades\" & lFileName

            If File.Exists(lDestPath) Then
                File.SetAttributes(lDestPath, FileAttributes.Normal)
            End If
            System.IO.File.Copy(lPath & lFileName, lDestPath, True)
            lExcel.OpenWorkBook(GetUserTempPath() & "Tac\Havades\" & lFileName)
            lExcel.ShowExcel_2007Up()
            'lExcel.ShowExcel()
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub
    Private Sub bttn_LPFeederLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bttn_LPFeederLoad.Click
        Dim row = dg.CurrentRowIndex
        If row < 0 Then
            Exit Sub
        End If

        If BT_Name = "Tbl_LPFeederCommonFeeder" Or BT_Name = "Tbl_LightFeederCommonFeeder" Then
            If Not (IsSetadMode OrElse (IsCenter And Not IsSpecialCenter)) Then
                If IsCenter Then
                    If ("," & WorkingCenterAreaIDs & ",").IndexOf("," & dg.Item(row, 13) & ",") = -1 Then
                        ShowError("شما اجازه بارگيري فيدر مربوط به ناحيه ديگر را نداريد")
                        Exit Sub
                    End If
                Else
                    If dg.Item(row, 13) <> IIf(mAreaId = -1, cmbArea.SelectedValue, mAreaId) Then
                        ShowError("شما اجازه بارگيري فيدر مربوط به ناحيه ديگر را نداريد")
                        Exit Sub
                    End If
                End If
            End If
        End If

        Dim ID As Integer = dg.Item(row, 0)
        Dim dlg As New frmBaseTablesNotStd("ViewLPFeederLoad", , , ID)
        dlg.ShowDialog()
        dlg.Dispose()
        LoadData()
    End Sub
    Private Sub BttnPartType_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BttnPartType.Click
        Dim row = dg.CurrentRowIndex
        If row < 0 Then
            Exit Sub
        End If
        Dim ID As Integer = dg.Item(row, 0)
        Dim dlg As New frmBaseTablesNotStd("VIEW_PartType", , , ID)
        dlg.ShowDialog()
        dlg.Dispose()
        LoadData()
    End Sub
    Private Sub BttnMPLPPart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BttnMPLPPart.Click
        Dim row = dg.CurrentRowIndex
        If row < 0 Then
            Exit Sub
        End If
        Dim ID As Integer = dg.Item(row, 0)
        Dim lIsMP As Boolean = dg.Item(row, 3)
        Dim dlg As frmBaseTablesNotStd
        If lIsMP Then
            dlg = New frmBaseTablesNotStd("VIEW_MPPartType", , , ID, , , , , , OwnerID)
        Else
            dlg = New frmBaseTablesNotStd("VIEW_LPPartType", , , ID, , , , , , OwnerID)
        End If
        dlg.ShowDialog()
        dlg.Dispose()
        LoadData()
    End Sub
    Private Sub grpSearch_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles grpSearch.Enter

    End Sub
    Private Sub cmbArea_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbArea.SelectedIndexChanged
        If mIsLoading Then Exit Sub
        mIsLoading = True
        If cmbArea.SelectedIndex >= 0 Then
            Dim Ds As Bargh_DataSets.DatasetCcRequester = DatasetCcReq2
            Ds.Tbl_MPPost.Clear()
            cmbMPPost.Text = ""
            Ds.Tbl_MPFeeder.Clear()
            cmbMPFeeder.Text = ""
            BindingTable("Exec spGetMPPosts_v2 " & cmbArea.SelectedValue & ",0,'',''", mCnn, Ds, "Tbl_MPPost", cmbMPPost)
            'If Ds.Tbl_MPPost.Rows.Count > 0 Then
            '    cmbMPPost.SelectedIndex = 0
            'End If
        Else
            cmbMPPost.SelectedIndex = -1
        End If
        mIsLoading = False
    End Sub
    Private Sub cmbArea_SelectedIndexKeyboardChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbArea.SelectedIndexKeyboardChanged
        If mIsLoading Then Exit Sub
        mIsLoading = True

        Dim Ds As Bargh_DataSets.DatasetCcRequester = DatasetCcReq2
        Ds.Tbl_MPPost.Clear()
        cmbMPPost.Text = ""
        Ds.Tbl_MPFeeder.Clear()
        cmbMPFeeder.Text = ""
        BindingTable("Exec spGetMPPosts_v2 -1,0,'',''", mCnn, Ds, "Tbl_MPPost", cmbMPPost)
        cmbArea.SelectedIndex = -1
        cmbArea.SelectedIndex = -1
        cmbMPPost.SelectedIndex = -1
        cmbMPPost.SelectedIndex = -1

        mIsLoading = False
    End Sub
    Private Sub cmbMPPost_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMPPost.SelectedIndexChanged
        If cmbMPPost.SelectedIndex >= 0 Then
            Dim Ds As Bargh_DataSets.DatasetCcRequester = DatasetCcReq2
            Ds.Tbl_MPFeeder.Clear()
            cmbMPFeeder.Text = ""
            BindingTable("SELECT * FROM Tbl_MPFeeder Where MPPostID=" & cmbMPPost.SelectedValue & " ORDER BY MPFeederName", mCnn, Ds, "Tbl_MPFeeder", cmbMPFeeder)
        End If
    End Sub
    Private Sub cmbMPFeeder_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMPFeeder.SelectedIndexChanged
        If Not cmbLPPost.Visible Then Exit Sub
        If cmbMPFeeder.SelectedIndex >= 0 Then
            Dim Ds As Bargh_DataSets.DatasetCcRequester = DatasetCcReq2
            Ds.Tbl_LPPart.Clear()
            cmbLPPost.Text = ""
            BindingTable("SELECT * FROM Tbl_LPPost Where MPFeederId=" & cmbMPFeeder.SelectedValue & " ORDER BY LPPostName", mCnn, Ds, "Tbl_LPPost", cmbLPPost)
        End If
    End Sub
    Private Sub btnZonePreCodes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnZonePreCodes.Click
        Dim row = dg.CurrentRowIndex
        If row < 0 Then
            Exit Sub
        End If
        Dim ID As Integer = dg.Item(row, 0)
        Dim dlg As New frmBaseTables("Tbl_ZonePreCode", ID)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub btnGroupChekList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGroupChekList.Click
        Dim row = dg.CurrentRowIndex
        If row < 0 Then
            Exit Sub
        End If
        Dim ID As Integer = dg.Item(row, 0)
        If BT_Name = "BTbl_BazdidCheckListSubGroup" Then
            Dim dlg As New frmBaseTablesNotStd("View_BazdidCheckListSubGroup", , , ID)
            dlg.ShowDialog()
            dlg.Dispose()
        Else
            Dim dlg As New frmBaseTablesNotStd("View_BazdidCheckList", , , ID)
            dlg.ShowDialog()
            dlg.Dispose()
        End If
    End Sub
    Private Sub btnSubCheckList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSubCheckList.Click
        Dim row = dg.CurrentRowIndex
        If row < 0 Then
            Exit Sub
        End If
        Dim ID As Integer = dg.Item(row, 0)
        Dim lGroupId As Integer = dg.Item(row, 1)
        Dim dlg As New frmBaseTablesNotStd("BTbl_SubCheckList", , , ID, , , , , , lGroupId)
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub
    Private Sub btnCheckListTajhizat_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCheckListTajhizat.Click
        SelectCheckListTajhizat()
    End Sub
    Private Sub btnCheckListPart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCheckListPart.Click
        SelectCheckListParts()
    End Sub
    Private Sub btnZoneMPFeeders_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnZoneMPFeeders.Click
        SelectZoneMPFeeders()
    End Sub
    Private Sub dg_CurrentCellChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dg.CurrentCellChanged
        If dg.CurrentRowIndex = -1 Then Exit Sub
        Dim lTableList As String
        lTableList = "View_BazdidCheckList,BTbl_BazdidCheckListGroup,BTbl_LPPostDetail,Tbl_MPPart,Tbl_LPPart,Tbl_LightPart,BTbl_BazdidCheckListSubGroup,View_BazdidCheckListSubGroup,Tbl_Zone"

        If lTableList.IndexOf(BT_Name) = -1 Then Exit Sub
        Dim lRowIndex As Integer = dg.CurrentRowIndex
        If lRowIndex = mRowIndex Then Exit Sub

        Dim lID As Integer = dg.Item(lRowIndex, 0)
        Dim lSQL As String

        '---------------------------
        If BT_Name = "View_BazdidCheckList" Or BT_Name = "View_BazdidCheckListSubGroup" Then

            Try
                DatasetBazdidView1.Tables("View_SubCheckList").Clear()
            Catch ex As Exception
            End Try

            lSQL = "SELECT SubCheckListId FROM BTbl_SubCheckList WHERE BazdidCheckListId = " & lID
            BindingTable(lSQL, mCnn, DatasetBazdidView1, "View_SubCheckList")
            If DatasetBazdidView1.Tables("View_SubCheckList").Rows.Count > 0 Then
                btnSubCheckList.Text = "[" & btnSubCheckList.Tag & "]"
            Else
                btnSubCheckList.Text = btnSubCheckList.Tag
            End If

            Try
                DatasetBazdidView1.Tables("View_CheckListTajhizat").Clear()
            Catch ex As Exception
            End Try

            lSQL = "SELECT CheckListTajhizatId FROM BTbl_CheckListTajhizat WHERE BazdidCheckListId = " & lID
            BindingTable(lSQL, mCnn, DatasetBazdidView1, "View_CheckListTajhizat")
            If DatasetBazdidView1.Tables("View_CheckListTajhizat").Rows.Count > 0 Then
                btnCheckListTajhizat.Text = "[" & btnCheckListTajhizat.Tag & "]"
            Else
                btnCheckListTajhizat.Text = btnCheckListTajhizat.Tag
            End If

            Try
                DatasetBazdidView1.Tables("View_CheckListPart").Clear()
            Catch ex As Exception
            End Try

            lSQL = "SELECT CheckListPartId FROM BTbl_CheckListPart WHERE BazdidCheckListId = " & lID
            BindingTable(lSQL, mCnn, DatasetBazdidView1, "View_CheckListPart")
            If DatasetBazdidView1.Tables("View_CheckListPart").Rows.Count > 0 Then
                btnCheckListPart.Text = "[" & btnCheckListPart.Tag & "]"
            Else
                btnCheckListPart.Text = btnCheckListPart.Tag
            End If

            '---------------------------
        ElseIf BT_Name = "BTbl_BazdidCheckListGroup" Then
            Try
                DatasetBazdidView1.Tables("View_BazdidGroupCheckList").Clear()
            Catch ex As Exception
            End Try

            lSQL = "SELECT BazdidCheckListId FROM BTbl_BazdidCheckList WHERE BazdidCheckListGroupId = " & lID

            Try
                DatasetBazdidView1.Tables("View_BazdidGroupCheckList").Clear()
            Catch ex As Exception
            End Try
            BindingTable(lSQL, mCnn, DatasetBazdidView1, "View_BazdidGroupCheckList")
            If DatasetBazdidView1.Tables("View_BazdidGroupCheckList").Rows.Count > 0 Then
                btnGroupChekList.Text = "[" & btnGroupChekList.Tag & "]"
            Else
                btnGroupChekList.Text = btnGroupChekList.Tag
            End If
            '---------------------------

        ElseIf BT_Name = "BTbl_BazdidCheckListSubGroup" Then
            Try
                DatasetBazdidView1.Tables("View_BazdidCheckListSubGroup").Clear()
            Catch ex As Exception
            End Try

            lSQL = "SELECT BazdidCheckListId FROM BTbl_BazdidCheckList WHERE BazdidCheckListSubGroupId = " & lID

            Try
                DatasetBazdidView1.Tables("View_BazdidCheckListSubGroup").Clear()
            Catch ex As Exception
            End Try
            BindingTable(lSQL, mCnn, DatasetBazdidView1, "View_BazdidCheckListSubGroup", , , , , , , True)
            If DatasetBazdidView1.Tables("View_BazdidCheckListSubGroup").Rows.Count > 0 Then
                btnGroupChekList.Text = "[" & btnGroupChekList.Tag & "]"
            Else
                btnGroupChekList.Text = btnGroupChekList.Tag
            End If
            '---------------------------
        ElseIf BT_Name = "BTbl_LPPostDetail" Then
            Dim lIsUserCreated As Boolean = dg.Item(lRowIndex, 1)
            ButtonDelete.Enabled = lIsUserCreated
            '---------------------------
        ElseIf BT_Name = "Tbl_Peymankar" Then
            Try
                Dim lIsPublicArea As Boolean = dg(lRowIndex, 2)
                ButtonEditField.Enabled = Not lIsPublicArea
                ButtonDelete.Enabled = Not lIsPublicArea
            Catch ex As Exception
                ButtonEditField.Enabled = False
                ButtonDelete.Enabled = False
            End Try
        ElseIf BT_Name = "Tbl_MPPart" Or BT_Name = "Tbl_LPPart" Or BT_Name = "Tbl_LightPart" Then

            Dim lTableName As String = BT_Name + "Factory"
            Dim lColPartId As String = BT_Name.Replace("Tbl_", "") & "Id"

            lSQL = "SELECT * FROM " & lTableName & " WHERE " & lColPartId & " = " & lID
            BindingTable(lSQL, mCnn, DatasetCcReq1, lTableName, , , , , , , True)

            If DatasetCcReq1.Tables(lTableName).Rows.Count > 0 Then
                btnPartFactories.Text = "[" & btnPartFactories.Tag & "]"
            Else
                btnPartFactories.Text = btnPartFactories.Tag
            End If

        ElseIf BT_Name = "Tbl_Zone" Then
            lSQL = "SELECT MPFeederZoneId FROM Tbl_MPFeederZone WHERE ZoneId = " & lID

            BindingTable(lSQL, mCnn, DatasetBT1, "Tbl_MPFeederZone", aIsClearTable:=True)
            If DatasetBT1.Tbl_MPFeederZone.Count > 0 Then
                btnZoneMPFeeders.Text = "[" & btnZoneMPFeeders.Tag & "]"
            Else
                btnZoneMPFeeders.Text = btnZoneMPFeeders.Tag
            End If

        End If

        mRowIndex = lRowIndex
    End Sub
    Private Sub frmBaseTablesNotStd_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        If Not mIsFirstView Then Exit Sub
        mIsFirstView = False

        If IsCenter Then
            If mAreaId = -1 Then
                cmbArea.SelectedIndex = -1
                cmbArea.SelectedIndex = -1
            End If
        End If

        cmbMPPost.SelectedIndex = -1
        cmbMPPost.SelectedIndex = -1

        cmbMPFeeder.SelectedIndex = -1
        cmbMPFeeder.SelectedIndex = -1

        cmbLPPost.SelectedIndex = -1
        cmbLPPost.SelectedIndex = -1

        cmbOwnership.SelectedIndex = -1
        cmbOwnership.SelectedIndex = -1

        If FilterQuery = "" Then
            LoadData()
        End If

        btnShowOnGISMap.RightToLeft = RightToLeft.Yes
        lblSearch.BringToFront()
    End Sub
    Private Sub dg_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles dg.Click
        If BT_Name <> "View_BazdidCheckList" Then Exit Sub

        If dg.CurrentCell.ColumnNumber = 14 Then
            If TypeName(dg.Item(dg.CurrentCell)) <> "DBNull" Then
                dg.Item(dg.CurrentCell) = Not (dg.Item(dg.CurrentCell))
            Else
                dg.Item(dg.CurrentCell) = True
            End If
        End If
    End Sub
    Private Sub btnActive_DeActive_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnActive_DeActive.Click
        Dim lSQLAll As String
        Dim lSQL As String
        lSQLAll = "SELECT * FROM BTbl_BazdidCheckList" &
          IIf(OwnerID > 0, IIf(BT_Name = "View_BazdidCheckListSubGroup", " WHERE BazdidCheckListSubGroupId = " & OwnerID, " WHERE BazdidCheckListGroupId = " & OwnerID), "")
        lSQL = lSQLAll & IIf(OwnerID > 0, " AND ", " WHERE ") & "IsActive = 1"
        DatasetBazdid1.BTbl_BazdidCheckList.Clear()
        Try
            DatasetBazdid1.Tables("BTbl_BazdidCheckList_IsActive").Clear()
        Catch ex As Exception
        End Try
        BindingTable(lSQLAll, mCnn, DatasetBazdid1, "BTbl_BazdidCheckList")
        BindingTable(lSQL, mCnn, DatasetBazdid1, "BTbl_BazdidCheckList_IsActive")

        Dim dlg As New frmSelectGridItems(DatasetBazdid1.Tables("BTbl_BazdidCheckList_IsActive"), "BTbl_BazdidCheckList", "BazdidCheckListId", "CheckListName", "")
        dlg.Condition = IIf(OwnerID > 0, IIf(BT_Name = "View_BazdidCheckListSubGroup", "BazdidCheckListSubGroupId = " & OwnerID, "BazdidCheckListGroupId = " & OwnerID), "")
        dlg.TitleText = "فهرست چک ليست‌ها براي فعال يا غير فعال شدن"

        If dlg.ShowDialog() = DialogResult.OK Then
            Dim lRow As DatasetBazdid.BTbl_BazdidCheckListRow
            Dim lRows() As DatasetBazdid.BTbl_BazdidCheckListRow

            Dim lViewSelection As CommonDataSet.ViewSelectionDataTable
            lViewSelection = dlg.ViewSelectionTable
            Dim i As Integer
            For i = 0 To lViewSelection.Rows.Count - 1
                With lViewSelection.Rows(i)
                    lRows = DatasetBazdid1.BTbl_BazdidCheckList.Select("BazdidCheckListId = " & .Item("ItemID"))
                    If lRows.Length > 0 Then
                        lRow = lRows(0)
                        lRow("IsActive") = .Item("ItemIsSelected")
                    End If
                End With
            Next
            Dim lUpdate As New frmUpdateDataSetBT
            lUpdate.UpdateDataSet("BTbl_BazdidCheckList", DatasetBazdid1)
            LoadData()
        End If
        dlg.Dispose()
    End Sub
    Private Sub BtnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSearch.Click
        If sender.Name = "BtnSearch" AndAlso BT_Name = "Tbl_FeederPart" Then
            If Not IsSupportItem(mdlHavadesInfo.HavadesGaranteeItems.ver_910128) Then
                ShowHavadesInfoMessage()
                Exit Sub
            End If
        End If

        mWhere = ""
        If Not grpSearch.Visible Then Exit Sub

        Dim lPost_Feeder_Field As String
        Dim lPeakField As String

        If cmbArea.SelectedIndex > -1 Then
            mWhere = " AND AreaId = " & cmbArea.SelectedValue
            If BT_Name = "Tbl_LPPostCommonFeeder" Or BT_Name = "Tbl_LPPostCommonLightFeeder" Then
                mWhere =
                    " AND ((AreaId = " & cmbArea.SelectedValue & ")  " &
                    " OR (LPPostId IN  " &
                    " 	(SELECT LPPostId  " &
                    " 	FROM Tbl_LPFeeder  " &
                    " 	WHERE AreaId = " & cmbArea.SelectedValue & " and (IsActive=" & IIf(cmbActiveStatePostFeeder.SelectedIndex = 0, "1", "0") & " )) " &
                    " 	)  " &
                    " OR (LPPostId IN  " &
                    " 	(SELECT Tbl_LPFeeder.LPPostId  " &
                    " 	FROM Tbl_LPCommonFeeder INNER JOIN Tbl_LPFeeder ON Tbl_LPCommonFeeder.LPFeederId = Tbl_LPFeeder.LPFeederId  " &
                    " 	WHERE Tbl_LPCommonFeeder.AreaId = " & cmbArea.SelectedValue & " and (IsActive=" & IIf(cmbActiveStatePostFeeder.SelectedIndex = 0, "1", "0") & ")) " &
                    " 	)) "

            End If
            If BT_Name = "Tbl_LPFeederCommonFeeder" Or BT_Name = "Tbl_LightFeederCommonFeeder" Then
                mWhere =
                    " AND ((AreaId = " & cmbArea.SelectedValue & ")  " &
                    " OR (LPFeederId IN  " &
                    " 	(SELECT Tbl_LPCommonFeeder.LPFeederId  " &
                    " 	FROM Tbl_LPCommonFeeder INNER JOIN Tbl_LPFeeder ON Tbl_LPCommonFeeder.LPFeederId = Tbl_LPFeeder.LPFeederId " &
                    " 	WHERE Tbl_LPCommonFeeder.AreaId = " & cmbArea.SelectedValue & " and (IsActive=" & IIf(cmbActiveStatePostFeeder.SelectedIndex = 0, "1", "0") & ")) " &
                    " 	)) AND IsLightFeeder = " & IIf(BT_Name = "Tbl_LPFeederCommonFeeder", 0, 1)

            End If
        End If

        If pnlMPPost_Down.Visible AndAlso cmbMPPost.Visible AndAlso cmbMPPost.SelectedIndex > -1 Then
            mWhere &= " AND MPPostId = " & cmbMPPost.SelectedValue
        End If

        If pnlMPPost_Down.Visible AndAlso cmbMPFeeder.Visible AndAlso cmbMPFeeder.SelectedIndex > -1 Then
            mWhere &= " AND MPFeederId = " & cmbMPFeeder.SelectedValue
        End If

        If pnlMPPost_Down.Visible AndAlso cmbLPPost.Visible AndAlso cmbLPPost.SelectedIndex > -1 Then
            mWhere &= " AND LPPostId = " & cmbLPPost.SelectedValue
        End If

        If txtLPPostCode.Visible Then
            If BT_Name = "Tbl_MPFeederKey" And txtLPPostCode.Text.Trim <> "" Then
                mWhere &= " AND " & MergeFarsiAndArabi("KeyCode", txtLPPostCode.Text.Trim)
            ElseIf txtLPPostCode.Text.Trim <> "" Then
                mWhere &= " AND " & MergeFarsiAndArabi("LPPostCode", txtLPPostCode.Text.Trim)
            End If
        End If

        If txtLPPostLocalCode.Visible = True And txtLPPostLocalCode.Text.Trim <> "" Then
            mWhere &= " AND " & MergeFarsiAndArabi("LocalCode", txtLPPostLocalCode.Text.Trim)
        End If

        If txtLPPostName.Text <> "" Then
            If BT_Name = "Tbl_LPPost" Or BT_Name = "Tbl_LPPostCommonFeeder" Or BT_Name = "Tbl_LPPostCommonLightFeeder" Then
                lPost_Feeder_Field = "LPPostName"
            ElseIf BT_Name = "Tbl_LPFeeder" Or BT_Name = "Tbl_LPFeederCommonFeeder" Or BT_Name = "Tbl_LightFeeder" Or BT_Name = "Tbl_LightFeederCommonFeeder" Then
                lPost_Feeder_Field = "LPFeederName"
            ElseIf BT_Name = "Tbl_MPFeeder" Then
                lPost_Feeder_Field = "MPFeederName"
            ElseIf BT_Name = "View_MPPost" Then
                lPost_Feeder_Field = "MPPostName"
            ElseIf BT_Name = "Tbl_FeederPart" Then
                lPost_Feeder_Field = "FeederPart"
            ElseIf BT_Name = "Tbl_MPFeederKey" Then
                lPost_Feeder_Field = "KeyName"
            End If
            If Not IsNothing(lPost_Feeder_Field) AndAlso lPost_Feeder_Field <> "" Then
                mWhere &= " AND " & MergeFarsiAndArabi(lPost_Feeder_Field, txtLPPostName.Text)
            End If
        End If

        Select Case BT_Name
            Case "Tbl_LPFeeder", "Tbl_LightFeeder", "Tbl_LPPost", "Tbl_MPFeeder", "Tbl_MPFeederKey", "Tbl_FeederPart"
                If Val(txtCoverPercentage.Text) > 0 Then
                    mWhere &= " AND CoverPercentage = " & Val(txtCoverPercentage.Text)
                End If
        End Select

        If BT_Name = "Tbl_LPPost" Or BT_Name = "Tbl_LPPostCommonFeeder" Or BT_Name = "Tbl_LPPostCommonLightFeeder" Then
            lPeakField = "PostPeakCurrent"
        Else
            lPeakField = "FeederPeakCurrent"
        End If

        If Val(txtPeakBarFrom.Text) > 0 Then
            mWhere &= " AND " & lPeakField & " >= " & txtPeakBarFrom.Text
        End If

        If Val(txtPeakBarTo.Text) > 0 Then
            mWhere &= " AND " & lPeakField & " <= " & txtPeakBarTo.Text
        End If

        If txtAddress.Visible AndAlso txtAddress.Text.Trim <> "" Then
            mWhere &= " AND " & MergeFarsiAndArabi("Address", txtAddress.Text.Trim)
        End If

        If cmbActiveStatePostFeeder.SelectedIndex > -1 Then
            mWhere &= " AND IsActive = " & IIf(cmbActiveStatePostFeeder.SelectedIndex = 0, "1", "0")
        End If

        If chkIsMainKey.Visible AndAlso chkIsMainKey.Checked Then
            mWhere &= " AND IsMainKey = 1 "
        End If

        If pnlLPPostYear.Visible Then
            If Trim(txtFromBuildYear.Text) <> "" Then
                mWhere &= " AND BuildYear >= " & txtFromBuildYear.Text
            End If
            If Trim(txtToBuildYear.Text) <> "" Then
                mWhere &= " AND BuildYear <= " & txtToBuildYear.Text
            End If
        End If

        If cmbCriticalType.Visible AndAlso cmbCriticalType.SelectedIndex > -1 Then
            If cmbCriticalType.SelectedIndex = 0 Then
                mWhere &= " AND (IsCritical =1 OR IsTemporaryCritical = 1)"
            ElseIf cmbCriticalType.SelectedIndex = 1 Then
                mWhere &= " AND (IsCritical =1)"
            ElseIf cmbCriticalType.SelectedIndex = 2 Then
                mWhere &= " AND (IsTemporaryCritical =1)"
            End If
        End If

        If pnlLighFeeder.Visible Then
            If rbLightLPFeeders.Checked Then
                mWhere &= " AND IsLightFeeder = 1"
            ElseIf rbLPLPFeeders.Checked Then
                mWhere &= " AND IsLightFeeder = 0"
            End If
        End If

        If cmbOwnership.Visible And cmbOwnership.SelectedIndex > -1 Then
            mWhere &= " AND OwnershipId = " & cmbOwnership.SelectedValue
        End If

        If BT_Name = "Tbl_MPFeederKey" Then
            mWhere = mWhere.Replace("IsActive", "Tbl_MPFeederKey.IsActive").Replace("AreaId", "Tbl_Area.AreaId")
            mWhere = mWhere.Replace("MPPostId", "Tbl_MPPost.MPPostId").Replace("MPFeederId", "Tbl_MPFeeder.MPFeederId")
            If txtGISCode.Text.Trim.Length > 0 Then
                mWhere &= " AND " & MergeFarsiAndArabi("GISCode", txtGISCode.Text.Trim)
            End If
            If cmbMPCloserType.SelectedValue > 0 Then
                mWhere &= " AND Tbl_MPFeederKey.MPCloserTypeId = " & cmbMPCloserType.SelectedValue
            End If
        End If

        If mWhere <> "" Then mWhere = mWhere.Remove(0, 5)
        LoadData()
    End Sub
    Private Sub btnSearchGlobal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearchGlobal.Click
        mWhere = ""
        If Not grpSearchGlobal.Visible Then Exit Sub

        If cmbActiveState.SelectedIndex > -1 Then
            mWhere &= " AND IsActive = " & IIf(cmbActiveState.SelectedIndex = 0, "1", "0")
        End If

        If mWhere <> "" Then mWhere = mWhere.Remove(0, 5)
        LoadData()
    End Sub
    Private Sub btnErjaCause_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnErjaCause.Click
        Dim row = dg.CurrentRowIndex
        If row < 0 Then
            Exit Sub
        End If
        Dim ID As Integer = dg.Item(row, 0)
        Dim dlg As New frmBaseTablesNotStd("Tbl_ErjaCause", , , ID)
        dlg.ShowDialog()
        dlg.Dispose()
        LoadData()
    End Sub
    Private Sub chkIsShowParts_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIsShowParts.CheckedChanged
        If BT_Name = "VIEW_PartType" Then
            mIsShowParts = chkIsShowParts.Checked
            pnlPartTypeEdit.Visible = mIsShowParts
            LoadData()
            dgtc_PartName.Width = IIf(mIsShowParts, 150, 0)
            dgtc_PartType.Width = IIf(mIsShowParts, 150, 235)
            Me.Width += IIf(mIsShowParts, 1, -1) * 85
        End If
    End Sub
    Private Sub btnPartFactories_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPartFactories.Click
        SelectPartFactories()
    End Sub
    Private Sub btnSavabeghBazdid_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSavabeghBazdid.Click
        Dim row = dg.CurrentRowIndex
        If row < 0 Then
            Exit Sub
        End If
        Dim ID As Integer = dg.Item(row, 0)
        If BT_Name = "Tbl_MPFeeder" Then
            Dim dlg As New frmBazdidSavabegh(ID, "", "")
            dlg.ShowDialog()
            dlg.Dispose()
        End If
        If BT_Name = "Tbl_LPPost" Or BT_Name = "Tbl_LPPostCommonFeeder" Or BT_Name = "Tbl_LPPostCommonLightFeeder" Then
            Dim dlg As New frmBazdidSavabegh("", ID, "")
            dlg.ShowDialog()
            dlg.Dispose()
        End If
        If BT_Name = "Tbl_LPFeeder" Or BT_Name = "Tbl_LPFeederCommonFeeder" Or BT_Name = "Tbl_LightFeederCommonFeeder" Or BT_Name = "Tbl_LightFeeder" Then
            Dim dlg As New frmBazdidSavabegh("", "", ID)
            dlg.ShowDialog()
            dlg.Dispose()
        End If
    End Sub
    Private Sub btnImportExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImportExcel.Click
        Try
            Dim lDlg As New frmImportServicePart
            If lDlg.ShowDialog() = DialogResult.OK Then
                LoadData()
            End If
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub
    Private Sub btnChangePostArea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChangePostArea.Click
        Try
            Dim row = dg.CurrentRowIndex
            If row < 0 Then
                Exit Sub
            End If
            Dim ID As Integer = dg.Item(row, 0)
            Dim dlg As New frmChangeLPPostAreaOfMPFeeder(ID)
            dlg.ShowDialog()
            dlg.Dispose()
        Catch ex As Exception
            ShowError(ex)
        End Try

    End Sub
    Private Sub btnChangePostArea_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnChangePostArea.MouseEnter
        lblDesc_btnChangePostArea.Visible = True
        lblDesc_btnChangePostArea.BringToFront()
    End Sub
    Private Sub _MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnChangePostArea.MouseLeave
        lblDesc_btnChangePostArea.Visible = False
        lblDesc_btnChangePostArea.SendToBack()
    End Sub
    Private Sub btnChangeLPFeederArea_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnChangeLPFeederArea.MouseEnter
        lblDesc_btnChangeLPFeederArea.Visible = True
        lblDesc_btnChangeLPFeederArea.BringToFront()
    End Sub
    Private Sub btnChangeLPFeederArea_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnChangeLPFeederArea.MouseLeave
        lblDesc_btnChangeLPFeederArea.Visible = False
        lblDesc_btnChangeLPFeederArea.SendToBack()
    End Sub
    Private Sub btnChangeLPFeederArea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChangeLPFeederArea.Click
        Try
            Dim row = dg.CurrentRowIndex
            If row < 0 Then
                Exit Sub
            End If
            If BT_Name = "Tbl_LPPostCommonFeeder" Or BT_Name = "Tbl_LPPostCommonLightFeeder" Then
                If Not (IsSetadMode OrElse (IsCenter And Not IsSpecialCenter)) Then
                    If IsCenter Then
                        If ("," & WorkingCenterAreaIDs & ",").IndexOf("," & dg.Item(row, 12) & ",") = -1 Then
                            ShowError("شما اجازه تغيير ناحيه فيدرهاي پست مربوط به ناحيه ديگر را نداريد")
                            Exit Sub
                        End If
                    Else
                        If dg.Item(row, 12) <> cmbArea.SelectedValue Then
                            ShowError("شما اجازه تغيير ناحيه فيدرهاي پست مربوط به ناحيه ديگر را نداريد")
                            Exit Sub
                        End If
                    End If
                End If
            End If

            Dim ID As Integer = dg.Item(row, 0)
            Dim dlg As New frmChangeLPFeederAreaOfLPPost(ID)
            dlg.ShowDialog()
            dlg.Dispose()
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub
    Private Sub btnPointsAndEarthFeeder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPointsAndEarthFeeder.Click
        If dg.CurrentRowIndex < 0 Then
            Exit Sub
        End If
        Dim lFeederId As Long
        lFeederId = dg.Item(dg.CurrentRowIndex, 0)
        Dim lDlg As New frmBasePointsAndEarthFeeder(lFeederId, BT_Name)
        lDlg.ShowDialog()
    End Sub
    Private Sub btnSearchView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearchView.Click
        If Not mIsSearch Then
            pnlSearch.Visible = True
            mIsSearch = True
            txtSearch.Focus()
        Else
            pnlSearch.Visible = False
            mIsSearch = False
        End If
    End Sub
    Private Sub txtSearch_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSearch.TextChanged
        ShowPartsFilter()
    End Sub
    Private Sub btn_LPPostEarth_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_LPPostEarth.Click
        Dim row = dg.CurrentRowIndex
        If row < 0 Then
            Exit Sub
        End If
        If BT_Name = "Tbl_LPPostCommonFeeder" Or BT_Name = "Tbl_LPPostCommonLightFeeder" Then
            If Not (IsSetadMode OrElse (IsCenter And Not IsSpecialCenter)) Then
                If IsCenter Then
                    If ("," & WorkingCenterAreaIDs & ",").IndexOf("," & dg.Item(row, 12) & ",") = -1 Then
                        ShowError("شما اجازه ارت گيري پست مربوط به ناحيه ديگر را نداريد")
                        Exit Sub
                    End If
                Else
                    If dg.Item(row, 12) <> cmbArea.SelectedValue Then
                        ShowError("شما اجازه ارت گيري پست مربوط به ناحيه ديگر را نداريد")
                        Exit Sub
                    End If
                End If
            End If
        End If

        Dim ID As Integer = dg.Item(row, 0)
        Dim dlg As New frmBaseTablesNotStd("ViewLPPostEarth", , , ID)
        dlg.ShowDialog()
        dlg.Dispose()
        LoadData()
    End Sub
    Private Sub btnLPPostEarthPoint_Click(sender As Object, e As EventArgs) Handles btnLPPostEarthPoint.Click
        If dg.CurrentRowIndex < 0 Then Exit Sub

        Dim ID As Integer = dg.Item(dg.CurrentRowIndex, 0)
        Dim lDlg As New frmBaseLPPostEarthPreview(ID)

        lDlg.ShowDialog()
        lDlg.Dispose()

    End Sub
    Private Sub btnImportFromExcel_Click(sender As Object, e As EventArgs)
        If BT_Name = "" Then
            Dim lIsAccess As Boolean = False
            lIsAccess = CheckUserTrustee("frmBS_LPFeederLoadBttnSave", 21) Or IsAdmin()
            If Not lIsAccess Then
                ShowNoAccessMessageByTag("frmBS_LPFeederLoadBttnSave")
                Exit Sub
            End If
        End If

        Dim lDlg As New frmImportMPPostTransLoad(BT_Name)
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub
    Private Sub btnImportSyncFromExcel_Click(sender As Object, e As EventArgs)
        Dim lIsAccess As Boolean = False
        lIsAccess = CheckUserTrustee("frmBS_LPFeederLoadBttnSave", 21) Or IsAdmin()
        If Not lIsAccess Then
            ShowNoAccessMessageByTag("frmBS_LPFeederLoadBttnSave")
            Exit Sub
        End If
        Dim lDlg As New frmImportMPPostTransLoad("PostFeeder")
        lDlg.ShowDialog()
        lDlg.Dispose()
    End Sub
    Private Sub rdbISLP_CheckedChanged(sender As Object, e As EventArgs) Handles rbIsLP.CheckedChanged, rbIsMP.CheckedChanged, rbShowAllEkip.CheckedChanged
        If mIsLoading Then Exit Sub
        cboArea_SelectedIndexChanged(sender, e)
    End Sub
    Private Sub btnShowOnGISMap_Click(sender As Object, e As EventArgs) Handles btnShowOnGISMap.Click
        ShowOnGISMap()
    End Sub
#End Region

#Region "Methods"
    Sub AddOrEdit(ByVal Id As Long)
        Dim dlg As Form = Nothing
        Select Case BT_Name
            Case "View_Area"
                dlg = New frmBS_Area(Id)
            Case "View_LPCommonFeeder"
                dlg = New frmBS_LPCommonFeeder(Id)
            Case "View_MPCommonFeeder"
                dlg = New frmBS_MPCommonFeeder(Id)
            Case "ViewCriticalFeeder"
                dlg = New frmBS_CriticalFeeder(Id)
            Case "View_DisconnectGroupLP"
                dlg = New frmBS_DisconnectGroup(Id, False)
            Case "View_DisconnectGroupLight"
                dlg = New frmBS_DisconnectGroup(Id, True)
            Case "View_DisconnectGroupMP"
                dlg = New frmBS_DisconnectGroupMP(Id)
            Case "View_City"
                dlg = New frmBS_City(Id)
            Case "Tbl_Master"
                If Id = -1 And cboArea.SelectedIndex = -1 Then
                    ShowError("لطفا ناحيه مرتبط را مشخص نماييد", False, MsgBoxIcon.MsgIcon_Exclamation)
                    Exit Sub
                End If
                dlg = New frmBS_Master(Id, cboArea.SelectedValue)
            Case "View_MPPost"
                dlg = New frmBS_MPPost(Id)
            Case "Tbl_LPPart"
                dlg = New frmBS_Parts(Id, BT_Name)
            Case "Tbl_MPPart"
                dlg = New frmBS_Parts(Id, BT_Name)
            Case "VIEW_PartType"
                If Not mIsEditParts Then
                    dlg = New frmBS_PartType(Id, OwnerID)
                Else
                    Dim lIsMP As Boolean = dg.Item(dg.CurrentRowIndex, 3)
                    Dim lPartTypeId As Integer = dg.Item(dg.CurrentRowIndex, 0)
                    Dim lMpLpPartTypeId As Integer = -1
                    If Not IsDBNull(dg.Item(dg.CurrentRowIndex, 4)) And Id > -1 Then
                        lMpLpPartTypeId = dg.Item(dg.CurrentRowIndex, 4)
                    End If
                    If lIsMP Then
                        dlg = New frmBS_MPPartType(lMpLpPartTypeId, lPartTypeId, OwnerID)
                    Else
                        dlg = New frmBS_LPPartType(lMpLpPartTypeId, lPartTypeId, OwnerID)
                    End If
                End If
            Case "VIEW_MPPartType"
                dlg = New frmBS_MPPartType(Id, OwnerID, GrandOwnerID)
            Case "VIEW_LPPartType"
                dlg = New frmBS_LPPartType(Id, OwnerID, GrandOwnerID)
            Case "Tbl_LightPart"
                dlg = New frmBS_Parts(Id, BT_Name)
            Case "Tbl_MPFeeder"
                dlg = New frmBS_MPFeeder(Id, OwnerID)
            Case "Tbl_FeederPart"
                dlg = New frmBS_FeederPart(Id, OwnerID)
            Case "Tbl_LPPost", "Tbl_LPPostCommonFeeder", "Tbl_LPPostCommonLightFeeder"
                dlg = New frmBS_LPPost(Id, OwnerID, GrandOwnerID)
            Case "Tbl_LPFeeder", "Tbl_LPFeederCommonFeeder", "Tbl_LightFeederCommonFeeder", "Tbl_LightFeeder"
                dlg = New frmBS_LPFeeder(Id, OwnerID)
            Case "TblEkipProfile"
                If cboArea.SelectedIndex = -1 Then
                    ShowError("لطفا ناحيه مرتبط را مشخص نماييد", False, MsgBoxIcon.MsgIcon_Exclamation)
                    Exit Sub
                End If
                dlg = New frmBS_EkipProfile(Id, cboArea.SelectedValue)
            Case "View_MPPostTrans"
                dlg = New frmBS_MPPostTrans(Id, OwnerID)
            Case "ViewLPPostLoad"
                dlg = New frmBS_LPPostLoad(Id, OwnerID)
            Case "TblRecloserFunction"
                dlg = New frmRecloserFunction(Id)
            Case "ViewLPPostEarth"
                dlg = New frmBS_LPPostEarth(Id, OwnerID)
            Case "ViewLPPostPointEarth"
                dlg = New frmBS_LPPostEarth(Id, -1, OwnerID)
            Case "ViewLPFeederLoad"
                Dim lMsg As String
                lMsg =
                    "براي بارگيري فيدرهاي فشار ضعيف، از طريق بارگيري پست توزيع آن فيدر اقدام نماييد." & vbCrLf &
                    "بعد از بارگيري پست توزيع از شما براي بارگيري فيدرهاي آن پست درخواست خواهد شد." & vbCrLf &
                    "و يا آخرين بارگيري پست را انتخاب و ويرايش نماييد و از پنجره بارگيري پست، دکمه بارگيري فيدرها را کليک نماييد."
                'ShowError(lMsg, False, MsgBoxIcon.MsgIcon_Hand)
                dlg = New frmBS_LPFeederLoad(Id, OwnerID)
            Case "ViewLPFeederPointsInfo"
                dlg = New frmBS_LPFeederPointLoad(OwnerID, , , , Id)
            Case "ViewMPFeederPointsInfo"
                dlg = New frmBS_MPFeederPointLoad(OwnerID, , , , Id)
            Case "Tbl_Zone"
                dlg = New frmBS_Zone(Id, OwnerID)
            Case "Tbl_IVRCode"
                dlg = New frmBS_IVRCode(Id)
            Case "Tbl_Section"
                dlg = New frmBS_Section(Id, OwnerID)
            Case "Tbl_Town"
                dlg = New frmBS_Town(Id, OwnerID)
            Case "Tbl_Village"
                dlg = New frmBS_Village(Id, OwnerID)
            Case "Tbl_Roosta"
                dlg = New frmBS_Roosta(Id, OwnerID)
            Case "Tbl_AVLCar"
                dlg = New frmBS_AVLCar(Id)
            Case "Tbl_MPFeederKey"
                dlg = New frmBS_MPFeederKey(Id, OwnerID)
            Case "Tbl_AllowValidator"
                dlg = New frmBS_AllowValidator(Id)
            Case "View_BazdidCheckList"
                dlg = New frmBS_BTbl_BazdidCheckList(Id, OwnerID)
            Case "View_BazdidCheckListSubGroup"
                dlg = New frmBS_BTbl_BazdidCheckList(Id, OwnerID)
            Case "BTbl_BazdidCheckListGroup"
                dlg = New frmBS_BazdidCheckListGroup(Id)
            Case "BTbl_SubCheckList"
                Dim IsDependent As Boolean = True
                Dim lOwnerId As Integer
                lOwnerId = OwnerID
                If Id = -1 And OwnerID = -1 And DatasetBazdid1.BTbl_SubCheckList.Rows.Count = 0 Then
                    IsDependent = False
                ElseIf Id = -1 And OwnerID = -1 And DatasetBazdid1.BTbl_SubCheckList.Rows.Count > 0 Then
                    OwnerID = dg.Item(dg.CurrentRowIndex, 5)
                ElseIf Id > -1 And OwnerID = -1 Then
                    OwnerID = dg.Item(dg.CurrentRowIndex, 5)
                End If
                dlg = New frmBS_SubCheckList(Id, OwnerID, IsDependent)
                OwnerID = lOwnerId

            Case "BTbl_Tajhizat"
                dlg = New frmBS_Tajhizat(Id)
            Case "BTbl_ServicePart"
                dlg = New frmBS_ServicePart(Id)
            Case "BTbl_BazdidMaster"
                If Id = -1 And cboArea.SelectedIndex = -1 Then
                    ShowError("لطفا ناحيه مرتبط را مشخص نماييد", False, MsgBoxIcon.MsgIcon_Exclamation)
                    Exit Sub
                End If
                dlg = New frmBS_Master(Id, cboArea.SelectedValue)
            Case "BTblBazdidEkip"
                If cboArea.SelectedIndex = -1 Then
                    ShowError("لطفا ناحيه مرتبط را مشخص نماييد", False, MsgBoxIcon.MsgIcon_Exclamation)
                    Exit Sub
                End If
                dlg = New frmBS_EkipProfile(Id, cboArea.SelectedValue)
            Case "BTbl_BazdidCheckListSubGroup"
                dlg = New frmBS_BazdidCheckListSubGroup(Id)
            Case "BTbl_LPPostDetail"
                dlg = New frmBS_LPPostDetail(Id)
            Case "Tbl_DisconnectGroupSetGroup"
                dlg = New frmBS_DisconnectGroupSetGroup(Id)
                '--------------------------------------
                '       Erja Applicatin Part
                '--------------------------------------
            Case "Tbl_ErjaReason"
                dlg = New frmBS_ErjaReason(Id)
            Case "Tbl_ErjaCause"
                dlg = New frmBS_ErjaCause(Id, OwnerID)
            Case "Tbl_ErjaPart"
                dlg = New frmBS_ErjaPart(Id)
            Case "Tbl_ErjaOperation"
                dlg = New frmBS_ErjaOperation(Id)
                '--------------------------------------
                '   Tamir Request Application Part
                '--------------------------------------
            Case "Tbl_TamirOperation"
                dlg = New frmBs_TamirOperation(Id)
            Case "Tbl_ManoeuvreType"
                dlg = New frmBs_ManoeuvreType(Id)
            Case "Tbl_Peymankar"
                dlg = New frmBS_Peymankar(Id)
        End Select

        If Not dlg Is Nothing Then
            If dlg.ShowDialog() = DialogResult.OK Then
                AssignToBasket(dlg, Id)
                If BT_Name <> "Tbl_Master" And BT_Name <> "BTbl_BazdidMaster" And BT_Name <> "TblEkipProfile" Then
                    LoadData()
                Else
                    cboArea_SelectedIndexChanged(Nothing, Nothing)
                End If
            End If
            dlg.Dispose()
        End If
    End Sub
    Private Sub SelectCheckListParts()
        Dim row = dg.CurrentRowIndex
        If row < 0 Then
            Exit Sub
        End If

        Dim ID As Integer = dg.Item(row, 0)
        Dim lBazdidTypeId As Integer = dg.Item(row, 2)

        Dim lSQL As String = "SELECT * FROM BTbl_CheckListPart WHERE BazdidCheckListId = " & ID

        Try
            DatasetBazdid1.BTbl_CheckListPart.Clear()
        Catch ex As Exception
        End Try
        BindingTable(lSQL, mCnn, DatasetBazdid1, "BTbl_CheckListPart")

        Dim dlg As New frmSelectGridItems(DatasetBazdid1.BTbl_CheckListPart, "BTbl_ServicePart", "ServicePartId", "ServicePartName", "", , , , , , "DefaultQuantity")
        dlg.Condition = "BazdidTypeId = " & lBazdidTypeId & " OR BazdidTypeId IS NULL"
        dlg.TitleText = "فهرست قطعاتي که ميتوان انتخاب نمود"

        If dlg.ShowDialog() = DialogResult.OK Then
            Dim lRow As DatasetBazdid.BTbl_CheckListPartRow
            For Each lRow In DatasetBazdid1.BTbl_CheckListPart.Rows
                lRow.Delete()
            Next

            Dim lViewSelection As CommonDataSet.ViewSelectionDataTable
            lViewSelection = dlg.ViewSelectionTable
            Dim i As Integer
            For i = 0 To lViewSelection.Rows.Count - 1
                With lViewSelection.Rows(i)
                    If .Item("ItemIsSelected") Then
                        lRow = DatasetBazdid1.BTbl_CheckListPart.NewRow()
                        lRow("CheckListPartId") = GetAutoInc()
                        lRow("ServicePartId") = .Item("ItemID")
                        lRow("BazdidCheckListId") = ID
                        lRow("DefaultQuantity") = .Item("DefaultQuantity")
                        DatasetBazdid1.BTbl_CheckListPart.AddBTbl_CheckListPartRow(lRow)
                    End If
                End With
            Next
            Dim lDlg As New frmUpdateDataSetBT
            lDlg.UpdateDataSet("BTbl_CheckListPart", DatasetBazdid1)
        End If
        dlg.Dispose()
    End Sub
    Private Sub SelectCheckListTajhizat()
        Dim row = dg.CurrentRowIndex
        If row < 0 Then
            Exit Sub
        End If

        Dim ID As Integer = dg.Item(row, 0)
        Dim lBazdidTypeId As Integer = dg.Item(row, 2)

        Dim lSQL As String = "SELECT * FROM BTbl_CheckListTajhizat WHERE BazdidCheckListId = " & ID

        Try
            DatasetBazdid1.BTbl_CheckListTajhizat.Clear()
        Catch ex As Exception
        End Try
        BindingTable(lSQL, mCnn, DatasetBazdid1, "BTbl_CheckListTajhizat")

        Dim dlg As New frmSelectGridItems(DatasetBazdid1.BTbl_CheckListTajhizat, "BTbl_Tajhizat", "TajhizatId", "TajhizatName", "")
        dlg.Condition = "BazdidTypeId = " & lBazdidTypeId & " OR BazdidTypeId IS NULL"
        dlg.TitleText = "فهرست تجهيزاتي که ميتوان انتخاب نمود"

        If dlg.ShowDialog() = DialogResult.OK Then
            Dim lRow As DatasetBazdid.BTbl_CheckListTajhizatRow
            For Each lRow In DatasetBazdid1.BTbl_CheckListTajhizat.Rows
                lRow.Delete()
            Next

            Dim lViewSelection As CommonDataSet.ViewSelectionDataTable
            lViewSelection = dlg.ViewSelectionTable
            Dim i As Integer
            For i = 0 To lViewSelection.Rows.Count - 1
                With lViewSelection.Rows(i)
                    If .Item("ItemIsSelected") Then
                        lRow = DatasetBazdid1.BTbl_CheckListTajhizat.NewRow()
                        lRow("CheckListTajhizatId") = GetAutoInc()
                        lRow("TajhizatId") = .Item("ItemID")
                        lRow("BazdidCheckListId") = ID
                        DatasetBazdid1.BTbl_CheckListTajhizat.AddBTbl_CheckListTajhizatRow(lRow)
                    End If
                End With
            Next
            Dim lDlg As New frmUpdateDataSetBT
            lDlg.UpdateDataSet("BTbl_CheckListTajhizat", DatasetBazdid1)
        End If
        dlg.Dispose()
    End Sub
    Private Sub SelectPartFactories()
        Dim row = dg.CurrentRowIndex
        If row < 0 Then
            Exit Sub
        End If

        Dim lPartId As Integer = dg.Item(row, 0)
        Dim lName As String = dg.Item(row, 2)

        Dim lTableName As String = BT_Name + "Factory"
        Dim lColPkId As String = BT_Name.Replace("Tbl_", "") & "FactoryId"
        Dim lColPartId As String = BT_Name.Replace("Tbl_", "") & "Id"

        Dim dlg As New frmSelectGridItems(DatasetCcReq1.Tables(lTableName), "Tbl_Factory", "FactoryId", "Factory", "")
        dlg.TitleText = "فهرست کارخانه‌هاي سازنده قطعه: " & lName

        If dlg.ShowDialog() = DialogResult.OK Then
            Dim lRow As DataRow
            For Each lRow In DatasetCcReq1.Tables(lTableName).Rows
                lRow.Delete()
            Next

            Dim lViewSelection As CommonDataSet.ViewSelectionDataTable
            lViewSelection = dlg.ViewSelectionTable
            Dim i As Integer
            For i = 0 To lViewSelection.Rows.Count - 1
                With lViewSelection.Rows(i)
                    If .Item("ItemIsSelected") Then
                        lRow = DatasetCcReq1.Tables(lTableName).NewRow()
                        lRow(lColPkId) = GetAutoInc()
                        lRow(lColPartId) = lPartId
                        lRow("FactoryId") = .Item("ItemID")
                        DatasetCcReq1.Tables(lTableName).Rows.Add(lRow)
                    End If
                End With
            Next
            Dim lDlg As New frmUpdateDataSetBT
            lDlg.UpdateDataSet(lTableName, DatasetCcReq1)

            If DatasetCcReq1.Tables(lTableName).Rows.Count > 0 Then
                btnPartFactories.Text = "[" & btnPartFactories.Tag & "]"
            Else
                btnPartFactories.Text = btnPartFactories.Tag
            End If
        End If
        dlg.Dispose()
    End Sub
    Private Sub AdjustSearchSection()

        If mIsShowSearch Or mIsShowSearchGlobal Then
            Dim lGrp As Panel
            If mIsShowSearchGlobal Then ' ",View_MPPost,Tbl_MPFeeder,".IndexOf("," & BT_Name & ",") > -1 Then
                lGrp = grpSearchGlobal
            ElseIf mIsShowSearch Then ' ",Tbl_LPPost,Tbl_LPFeeder,".IndexOf("," & BT_Name & ",") > -1 Then
                lGrp = grpSearch
            End If

            Select Case BT_Name
                Case "View_MPPost"
                    pnlMPPost_Down.Visible = False
                    cmbActiveStatePostFeeder.Top = cmbMPPost.Top + pnlMPPost_Down.Top
                    lblActiveStatePostFeeder.Top = cmbActiveStatePostFeeder.Top + 3
                    BtnSearch.Top = grpSearch.Height - 36 'cmbMPFeeder.Top + pnlMPPost_Down.Top
                    grpSearch.Height -= BtnSearch.Height + 5
                    lblLPPostName.Text = "پست فوق توزيع"
                Case "Tbl_MPFeeder"
                    cmbMPFeeder.Visible = False
                    lblMPFeeder.Visible = False
                    txtLPPostCode.Visible = False
                    lblLPPostCode.Visible = False
                    pnlPeakBar.Visible = False
                    lblCriticalType.Visible = True
                    cmbCriticalType.Visible = True
                    cmbActiveStatePostFeeder.Top = cmbMPPost.Top + pnlMPPost_Down.Top
                    lblActiveStatePostFeeder.Top = cmbActiveStatePostFeeder.Top + 3
                    BtnSearch.Top = grpSearch.Height - 36 'cmbMPFeeder.Top + pnlMPPost_Down.Top
                    grpSearch.Height -= BtnSearch.Height + 5
                    lblCriticalType.Top = lblActiveStatePostFeeder.Top - pnlMPPost_Down.Top
                    cmbCriticalType.Top = cmbActiveStatePostFeeder.Top - pnlMPPost_Down.Top
                    lblLPPostName.Text = "فيدر فشار متوسط"
                    lblLPPostCode.Hide()
                    txtLPPostCode.Hide()
                    lblOwnership.Show()
                    cmbOwnership.Show()
                    btnPointsAndEarthFeeder.Visible = True
                    lblCoverPercentage.Visible = True
                    txtCoverPercentage.Visible = True
                    'btnPointsAndEarthFeeder.Location = New Point(100, 465)
                Case "Tbl_LPPost", "Tbl_LPPostCommonFeeder", "Tbl_LPPostCommonLightFeeder"
                    lblAddress.Visible = True
                    txtAddress.Visible = True
                    pnlLPPostYear.Visible = True
                    lblLPPostLocalCode.Visible = True
                    txtLPPostLocalCode.Visible = True

                    If BT_Name = "Tbl_LPPost" Then
                        lblCoverPercentage.Visible = True
                        txtCoverPercentage.Visible = True
                    End If
                Case "Tbl_LPFeeder", "Tbl_LPFeederCommonFeeder", "Tbl_LightFeederCommonFeeder", "Tbl_LightFeeder"
                    lblLPPost.Visible = True
                    cmbLPPost.Visible = True
                    btnPointsAndEarthFeeder.Visible = True
                    lblLPPostName.Text = "نام فيدر"
                    lblPeakBarFrom.Text = "از بار پيک فيدر"
                    lblPeakBarTo.Text = "تا"
                    lblCriticalType.Visible = True
                    cmbCriticalType.Visible = True
                    pnlLighFeeder.Visible = True
                    lblOwnership.Show()
                    cmbOwnership.Show()
                    lblLPPostCode.Hide()
                    txtLPPostCode.Hide()

                    If BT_Name = "Tbl_LPFeeder" Then
                        lblCoverPercentage.Visible = True
                        txtCoverPercentage.Visible = True
                    End If
                Case "Tbl_FeederPart"
                    lblLPPostCode.Visible = False
                    txtLPPostCode.Visible = False
                    'lblActiveStatePostFeeder.Visible = False
                    'cmbActiveStatePostFeeder.Visible = False
                    pnlPeakBar.Visible = False
                    lblLPPostName.Text = "نام تکه فيدر"

                    lblCoverPercentage.Visible = True
                    txtCoverPercentage.Visible = True
                Case "Tbl_MPFeederKey"
                    lblLPPostName.Text = "نام کليد"
                    lblLPPostCode.Text = "کد کليد"
                    pnlPeakBar.Visible = False
                    chkIsMainKey.Visible = True
                    txtGISCode.Visible = True
                    lblGISCode.Visible = True
                    lblMPCloserType.Visible = True
                    cmbMPCloserType.Visible = True
                    lblGISCode.Top = cmbActiveStatePostFeeder.Top + cmbActiveStatePostFeeder.Height + 5
                    lblGISCode.Left = cmbActiveStatePostFeeder.Left + cmbActiveStatePostFeeder.Width + 110
                    txtGISCode.Top = cmbActiveStatePostFeeder.Top + cmbActiveStatePostFeeder.Height + 5
                    txtGISCode.Left = lblGISCode.Left - 5 - txtGISCode.Width
                    lblMPCloserType.Top = txtGISCode.Top
                    lblMPCloserType.Left = txtGISCode.Left - 5 - lblMPCloserType.Width
                    cmbMPCloserType.Top = txtGISCode.Top '+ txtGISCode.Height
                    cmbMPCloserType.Left = lblMPCloserType.Left - 5 - cmbMPCloserType.Width
                    lblCoverPercentage.Visible = True
                    txtCoverPercentage.Visible = True
            End Select

            Dim dgHeight As Integer = dg.Height
            dg.Height = dgHeight - lGrp.Height + 5
            dg.Top = lGrp.Top + lGrp.Height + 5
            lGrp.Visible = True
            lblSearch.Visible = True
        End If

    End Sub
    Public ReadOnly Property SelectedIDs() As String
        Get
            Return mSelectedIDs
        End Get
    End Property
    Public ReadOnly Property SelectedNames() As String
        Get
            Return mSelectedNames
        End Get
    End Property
    Sub LoadData()
        Dim lDs As New DataSet
        Dim lRowIndex = dg.CurrentRowIndex
        Dim LAW As String = "", LAA As String = ""
        Dim LAF As String = ""
        Dim lAllCenterArea As String = ""
        If LegalAreaQuery <> "" Then
            LAW = " WHERE " & LegalAreaQuery
            LAA = " AND " & LegalAreaQuery
            LAF = IIf(FilterQuery <> "", " AND ", " WHERE ") & LegalAreaQuery
        ElseIf IsCenter Then
            lAllCenterArea = WorkingCenterAreaQuery
            If Not IsSetadMode Then
                LAW = " WHERE " & WorkingCenterAreaQuery
                LAA = " AND " & WorkingCenterAreaQuery
                If BT_Name = "Tbl_LPPostCommonFeeder" Or BT_Name = "Tbl_LPPostCommonLightFeeder" Then
                    LAA =
                        " AND ((AreaId IN (" & WorkingCenterAreaIDs & "))  " &
                        " OR (LPPostId IN  " &
                        " 	(SELECT LPPostId  " &
                        " 	FROM Tbl_LPFeeder  " &
                        " 	WHERE AreaId IN (" & WorkingCenterAreaIDs & ") and (IsActive=" & IIf(cmbActiveStatePostFeeder.SelectedIndex = 0, "1", "0") & " )) " &
                        " 	)  " &
                        " OR (LPPostId IN  " &
                        " 	(SELECT Tbl_LPFeeder.LPPostId  " &
                        " 	FROM Tbl_LPCommonFeeder INNER JOIN Tbl_LPFeeder ON Tbl_LPCommonFeeder.LPFeederId = Tbl_LPFeeder.LPFeederId  " &
                        " 	WHERE Tbl_LPCommonFeeder.AreaId IN (" & WorkingCenterAreaIDs & ") and (IsActive=" & IIf(cmbActiveStatePostFeeder.SelectedIndex = 0, "1", "0") & ")) " &
                        " 	)) "

                End If
                If BT_Name = "Tbl_LPFeederCommonFeeder" Or BT_Name = "Tbl_LightFeederCommonFeeder" Then
                    LAA =
                        " AND ((AreaId IN (" & WorkingCenterAreaIDs & "))  " &
                        " OR (LPFeederId IN  " &
                        " 	(SELECT Tbl_LPCommonFeeder.LPFeederId  " &
                        " 	FROM Tbl_LPCommonFeeder INNER JOIN Tbl_LPFeeder ON Tbl_LPCommonFeeder.LPFeederId = Tbl_LPFeeder.LPFeederId " &
                        " 	WHERE Tbl_LPCommonFeeder.AreaId IN (" & WorkingCenterAreaIDs & ") and (IsActive=" & IIf(cmbActiveStatePostFeeder.SelectedIndex = 0, "1", "0") & ")) " &
                        " 	)) AND IsLightFeeder = " & IIf(BT_Name = "Tbl_LPFeederCommonFeeder", 0, 1)

                End If
                LAF = IIf(FilterQuery <> "", " AND ", " WHERE ") & WorkingCenterAreaQuery
            End If
        ElseIf Not IsCenter Then
            LAW = " WHERE AreaId = " & WorkingAreaId
            LAA = " AND AreaId = " & WorkingAreaId
            If BT_Name = "Tbl_LPPostCommonFeeder" Or BT_Name = "Tbl_LPPostCommonLightFeeder" Then
                LAA =
                    " AND ((AreaId = " & cmbArea.SelectedValue & ")  " &
                    " OR (LPPostId IN  " &
                    " 	(SELECT LPPostId  " &
                    " 	FROM Tbl_LPFeeder  " &
                    " 	WHERE AreaId = " & cmbArea.SelectedValue & " and (IsActive=" & IIf(cmbActiveStatePostFeeder.SelectedIndex = 0, "1", "0") & " )) " &
                    " 	)  " &
                    " OR (LPPostId IN  " &
                    " 	(SELECT Tbl_LPFeeder.LPPostId  " &
                    " 	FROM Tbl_LPCommonFeeder INNER JOIN Tbl_LPFeeder ON Tbl_LPCommonFeeder.LPFeederId = Tbl_LPFeeder.LPFeederId  " &
                    " 	WHERE Tbl_LPCommonFeeder.AreaId = " & cmbArea.SelectedValue & " and (IsActive=" & IIf(cmbActiveStatePostFeeder.SelectedIndex = 0, "1", "0") & ")) " &
                    " 	)) "

            End If
            If BT_Name = "Tbl_LPFeederCommonFeeder" Or BT_Name = "Tbl_LightFeederCommonFeeder" Then
                LAA =
                    " AND ((AreaId = " & mAreaId & ")  " &
                    " OR (LPFeederId IN  " &
                    " 	(SELECT Tbl_LPCommonFeeder.LPFeederId  " &
                    " 	FROM Tbl_LPCommonFeeder INNER JOIN Tbl_LPFeeder ON Tbl_LPCommonFeeder.LPFeederId = Tbl_LPFeeder.LPFeederId " &
                    " 	WHERE Tbl_LPCommonFeeder.AreaId = " & mAreaId & " and (IsActive=" & IIf(cmbActiveStatePostFeeder.SelectedIndex = 0, "1", "0") & ")) " &
                    " 	)) AND IsLightFeeder = " & IIf(BT_Name = "Tbl_LPFeederCommonFeeder", 0, 1)

            End If
            LAF = IIf(FilterQuery <> "", " AND ", " WHERE ") & "AreaId = " & WorkingAreaId
        End If

        lDs = DatasetCcReq1
        'If mMessageText <> "" Then
        '    Me.Text = mMessageText
        'End If

        DatasetCcReq1.Clear()
        dg.CaptionText = ""
        TableName = BT_Name
        Dim SQL As String = "" '"Select * from " & BT_Name
        Select Case BT_Name
            Case "View_Area"
                SQL = "SELECT * FROM View_Area" & LAW & " ORDER BY Province, City, Area, ParentArea, IsSetad, IsCenter"
                dg.DataSource = DatasetCcReq1.View_Area
                dg.CaptionText = "نواحي"
            Case "View_MPPost"
                SQL = "SELECT * FROM View_MPPost" & IIf(mWhere <> "", " WHERE " & mWhere & (FilterQuery & LAF).Replace("WHERE", " AND"), FilterQuery & LAF) & " ORDER BY Area, SortOrder, MPPostName" 'Where AreaID=" & ModuleToolsAMb.WorkingAreaId

                dg.DataSource = DatasetCcReq1.View_MPPost
                bttn_MPFeederList.Visible = True
                Bttn_MPPostTrans.Visible = True
                btnShowOnGISMap.Visible = True
                dg.CaptionText = "شبكه فشار متوسط: پست هاي فوق توزيع"
            Case "Tbl_GroupPart"
                SQL = "SELECT * FROM Tbl_GroupPart " & FilterQuery & LAF & " ORDER BY GroupPartId "

                dg.DataSource = DatasetCcReq1.Tbl_GroupPart
                BttnPartType.Visible = True
                dg.CaptionText = "گروه تحهيزات سرقتي"
            Case "Tbl_MPFeeder"
                If OwnerID >= 0 Then
                    SQL = "SELECT * FROM View_MPFeeder WHERE MPPostID = " & OwnerID & LAA & IIf(mWhere <> "", " AND " & mWhere, "") & " ORDER BY MPPostName, SortOrder, MPFeederName"
                Else
                    '& IIf(mWhere <> "", "WHERE " & mWhere, LAW) & "
                    SQL = "SELECT * FROM View_MPFeeder " & IIf(mWhere <> "", "WHERE " & mWhere & LAA, LAW) & " ORDER BY MPPostName, SortOrder, MPFeederName"
                End If
                TableName = "View_MPFeeder"
                dg.DataSource = DatasetCcReq1.View_MPFeeder
                bttn_LPPost.Visible = True
                btnFeederPart.Visible = True
                btnChangePostArea.Visible = True
                btnShowOnGISMap.Visible = True

                Dim lIsAccess As Boolean = False
                lIsAccess = CheckUserTrustee("CanChangeAllLPPostArea", 9) Or IsAdmin()
                btnChangePostArea.Enabled = lIsAccess

                dg.CaptionText = "شبكه فشار متوسط: فيدرهاي فشار متوسط"

                If IsBazdidApp Then
                    btnSavabeghBazdid.Visible = True
                End If

            Case "Tbl_FeederPart"
                If OwnerID >= 0 Then
                    SQL = "SELECT Tbl_FeederPart.*, Tbl_Area.Area, Tbl_MPFeeder.MPFeederName, Tbl_MPPost.MPPostName, ISNULL(Tbl_MPFeeder.CoverPercentage, 100) AS CoverPercentage  FROM Tbl_FeederPart INNER JOIN Tbl_Area ON Tbl_FeederPart.AreaId = Tbl_Area.AreaId INNER JOIN Tbl_MPFeeder ON Tbl_FeederPart.MPFeederId = Tbl_MPFeeder.MPFeederId INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId WHERE Tbl_FeederPart.MPFeederID=" & OwnerID & LAA.Replace("AreaId", "Tbl_FeederPart.AreaId") & IIf(mWhere <> "", " AND " & mWhere, "") & " ORDER BY Area, MPPostName, MPFeederName , FeederPart"
                Else
                    If IsCenter Then
                        SQL = "SELECT Tbl_FeederPart.*, Tbl_Area.Area, Tbl_MPFeeder.MPFeederName, Tbl_MPPost.MPPostName, ISNULL(Tbl_MPFeeder.CoverPercentage, 100) AS CoverPercentage FROM Tbl_FeederPart INNER JOIN Tbl_Area ON Tbl_FeederPart.AreaId = Tbl_Area.AreaId INNER JOIN Tbl_MPFeeder ON Tbl_FeederPart.MPFeederId = Tbl_MPFeeder.MPFeederId INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " & IIf(mWhere <> "", "WHERE " & mWhere.Replace("IsActive", "Tbl_FeederPart.IsActive").Replace("AreaId", "Tbl_FeederPart.AreaId") & LAA.Replace("AreaId", "Tbl_FeederPart.AreaId"), LAW.Replace("AreaId", "Tbl_FeederPart.AreaId")) & " ORDER BY Area, MPPostName, MPFeederName , FeederPart"
                    Else
                        SQL = "SELECT Tbl_FeederPart.*, Tbl_Area.Area, Tbl_MPFeeder.MPFeederName, Tbl_MPPost.MPPostName, ISNULL(Tbl_MPFeeder.CoverPercentage, 100) AS CoverPercentage FROM Tbl_FeederPart INNER JOIN Tbl_Area ON Tbl_FeederPart.AreaId = Tbl_Area.AreaId INNER JOIN Tbl_MPFeeder ON Tbl_FeederPart.MPFeederId = Tbl_MPFeeder.MPFeederId INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId " & IIf(mWhere <> "", "WHERE " & mWhere.Replace("IsActive", "Tbl_FeederPart.IsActive").Replace("AreaId", "Tbl_FeederPart.AreaId"), "") & " ORDER BY Area, MPPostName, MPFeederName , FeederPart"
                    End If
                End If
                SQL = SQL.Replace(" AreaId", " Tbl_FeederPart.AreaId")
                SQL = SQL.Replace(" MPPostId", " Tbl_MPFeeder.MPPostId")
                SQL = SQL.Replace(" MPFeederId", " Tbl_MPFeeder.MPFeederId")
                SQL = SQL.Replace(" IsActive", " Tbl_MPFeeder.IsActive")
                TableName = "Tbl_FeederPart"
                dg.DataSource = DatasetCcReq1.Tbl_FeederPart
                dg.CaptionText = "شبكه فشار متوسط: تکه فيدرهاي فشار متوسط"
            Case "VIEW_PartType"
                If OwnerID >= 0 Then
                    SQL = "SELECT * FROM " & IIf(mIsShowParts, "View_PartTypeMembers", "VIEW_PartType") & " where GroupPartId=" & OwnerID & " ORDER BY PartType"
                Else
                    SQL = "SELECT * FROM " & IIf(mIsShowParts, "View_PartTypeMembers", "VIEW_PartType") & " ORDER BY PartType"
                End If
                'TableName = "VIEW_PartType"
                'dg.DataSource = DatasetCcReq1.VIEW_PartType
                BttnMPLPPart.Visible = True
                dg.CaptionText = IIf(mIsShowParts, "فهرست اجناس سرقتي توانير", "نوع اجناس سرقتي")
            Case "VIEW_MPPartType"
                If OwnerID >= 0 Then
                    SQL = "SELECT * FROM VIEW_MPPartType where PartTypeId =" & OwnerID & LAA & " ORDER BY MPPartTypeId"
                Else
                    SQL = "SELECT * FROM VIEW_MPPartType " & LAW & " ORDER BY MPPartTypeId"
                End If
                'TableName = "VIEW_MPPartType"
                'dg.DataSource = DatasetCcReq1.VIEW_MPPartType
                dg.CaptionText = "فهرست اجناس سرقت شده"
            Case "VIEW_LPPartType"
                If OwnerID >= 0 Then
                    SQL = "SELECT * FROM VIEW_LPPartType where PartTypeId =" & OwnerID & " ORDER BY LPPartTypeId"
                Else
                    SQL = "SELECT * FROM VIEW_LPPartType ORDER BY LPPartTypeId"
                End If
                'TableName = "VIEW_LPPartType"
                'dg.DataSource = DatasetCcReq1.VIEW_LPPartType
                dg.CaptionText = "فهرست اجناس سرقت شده"
            Case "View_LPCommonFeeder"
                SQL = "SELECT * FROM View_LPCommonFeeder ORDER BY MPPostName, MPFeederName, LPPostName, LPFeederName, Area"
                dg.DataSource = DatasetCcReqView1.View_LPCommonFeeder
                dg.CaptionText = "شبكه فشار ضعيف: فيدرهاي مشترك فشار ضعيف"
            Case "View_MPCommonFeeder"
                SQL = "SELECT * FROM View_MPCommonFeeder ORDER BY MPPostName, MPFeederName, Area"
                dg.DataSource = DatasetCcReqView1.View_MPCommonFeeder
                dg.CaptionText = "شبكه فشار متوسط: فيدرهاي مشترك فشار متوسط"
            Case "ViewCriticalFeeder"
                Dim lLPFeederJoin As String = " LEFT JOIN Tbl_LPFeeder ON ViewCriticalFeeder.LPFeederId = Tbl_LPFeeder.LPFeederId "
                Dim lLPPostJoin As String = " LEFT JOIN Tbl_LPPost ON ViewCriticalFeeder.LPPostId = Tbl_LPPost.LPPostId "
                Dim lMPFeederJoin As String = " LEFT JOIN Tbl_MPFeeder ON ViewCriticalFeeder.MPFeederId = Tbl_MPFeeder.MPFeederId "
                Dim lMPPostJoin As String = " LEFT JOIN Tbl_MPPost ON ViewCriticalFeeder.MPPostId = Tbl_MPPost.MPPostId "
                Dim lAreaJoin As String = " INNER JOIN Tbl_Area ON ISNULL(Tbl_LPFeeder.AreaId, ISNULL(Tbl_LPPost.AreaId,ISNULL(Tbl_MPFeeder.AreaId, Tbl_MPPost.AreaId))) = Tbl_Area.AreaId "
                Dim lJoin As String = lLPFeederJoin & lLPPostJoin & lMPFeederJoin & lMPPostJoin & lAreaJoin
                SQL = "SELECT ViewCriticalFeeder.* FROM ViewCriticalFeeder" & lJoin & IIf(mWhere <> "", " WHERE " & mWhere & (FilterQuery & LAF.Replace("AreaId", "Tbl_Area.AreaId")).Replace("WHERE", " AND"), FilterQuery & LAF.Replace("AreaId", "Tbl_Area.AreaId")) & " ORDER BY ViewCriticalFeeder.MPPostName, ViewCriticalFeeder.MPFeederName, ViewCriticalFeeder.LPPostName, ViewCriticalFeeder.LPFeederName"
                dg.DataSource = DatasetCcReqView1.ViewCriticalFeeder
                dg.CaptionText = "فيدرهاي حساس"
            Case "View_DisconnectGroupLP"
                SQL = "SELECT * FROM View_DisconnectGroupLP" & " ORDER BY DisconnectGroup, DisconnectGroupSet"
                dg.DataSource = DatasetCcReq1.View_DisconnectGroupLP
                Bttn_DisconnectReason.Visible = True
                Bttn_DisconnectReasonXPReason.Visible = True
                dg.CaptionText = "شبكه فشار ضعيف: علل قطع"
            Case "View_DisconnectGroupLight"
                SQL = "SELECT * FROM View_DisconnectGroupLight" & " ORDER BY DisconnectGroup, DisconnectGroupSet"

                dg.DataSource = DatasetCcReq1.View_DisconnectGroupLight
                Bttn_DisconnectReason.Visible = True
                Bttn_DisconnectReasonXPReason.Visible = True
                dg.CaptionText = "روشنايي معابر: علل قطع"

            Case "View_DisconnectGroupMP"
                Dim FilterStr As String = ""
                If EffectedGroup = "FOGHETOZI" Then
                    FilterStr = " AND IsFogheTozi=1"
                ElseIf EffectedGroup = "TOZI" Then
                    FilterStr = " AND IsFogheTozi=0"
                End If
                If MPRequestType = "WANTED" Then
                    FilterStr = " AND IsWanted=1"
                ElseIf MPRequestType = "UNWANTED" Then
                    FilterStr = " AND IsWanted=0"
                End If
                If FilterStr <> "" Then
                    FilterStr = " WHERE" & FilterStr.Remove(1, 4)
                End If
                SQL = "SELECT * FROM View_DisconnectGroupMP" & FilterStr & " ORDER BY DisconnectGroup, DisconnectGroupSet"
                dg.DataSource = DatasetCcReq1.View_DisconnectGroupMP
                Bttn_DisconnectReason.Visible = True
                Bttn_DisconnectReasonXPReason.Visible = True
                dg.CaptionText = "شبكه فشار متوسط: علل قطع"
            Case "View_City"
                If IsCenter Then
                    SQL = "SELECT * FROM View_City" & " ORDER BY Province, City"
                Else
                    SQL = "SELECT * FROM View_City Where ProvinceId=" & WorkingProvinceId & " ORDER BY Province, City"
                End If
                dg.DataSource = DatasetCcReq1.View_City
                dg.CaptionText = "شهرستان ها"
            Case "Tbl_Master"
                'BindingTable("SELECT Tbl_Area.* FROM Tbl_Area INNER JOIN Tbl_City ON Tbl_Area.CityId = Tbl_City.CityId WHERE Tbl_City.ProvinceId=" & WorkingProvinceId & " AND AreaID<>1 ORDER BY Area", mCnn, DatasetCcReq1, "Tbl_Area")
                LoadAreaDataTable(mCnn, DatasetCcReq1, cboArea, True, False, True)
                Dim ActiveStr As String = ""
                If (SelectMode And Not IsShowAllMaster) Or mIsShowActiveEkipOnly Then
                    ActiveStr = " And IsActive=1"
                End If
                pnlEkipNetworkType.Visible = True
                If IsCenter Then
                    Label5.Visible = True
                    cboArea.Visible = True
                    btnSearchView.Visible = True
                    If OwnerID > -1 Then
                        cboArea.Enabled = False
                        cboArea.SelectedValue = OwnerID
                    End If
                    If cboArea.SelectedIndex > -1 Then
                        SQL = "SELECT * FROM Tbl_Master Where ApplicationId = " & IIf(mApplicationType = mdlHashemi.ApplicationTypes.TamirRequest, ApplicationTypes.Havades, mApplicationType) & " AND AreaID=" & cboArea.SelectedValue & ActiveStr & " ORDER BY Name, Address, Tel, Mobile"
                    Else
                        SQL = "SELECT * FROM Tbl_Master Where ApplicationId = " & IIf(mApplicationType = mdlHashemi.ApplicationTypes.TamirRequest, ApplicationTypes.Havades, mApplicationType) & ActiveStr & LAA & " ORDER BY Name, Address, Tel, Mobile"
                        SQL = SQL.Replace("Where  And", "Where")
                    End If
                    SQL = SQL.Replace("Where  And", "Where")

                Else
                    SQL = "SELECT * FROM Tbl_Master Where ApplicationId = " & IIf(mApplicationType = mdlHashemi.ApplicationTypes.TamirRequest, ApplicationTypes.Havades, mApplicationType) & " AND AreaID=" & WorkingAreaId & ActiveStr & " ORDER BY Name, Address, Tel, Mobile"
                    cboArea.SelectedValue = WorkingAreaId
                End If

                dg.DataSource = DatasetCcReq1.Tbl_Master
                dg.CaptionText = "تکنسين / کارشناس بازديد"
            Case "Tbl_LPPart"
                SQL = "SELECT     Tbl_LPPart.LPPartId AS PartId, Tbl_LPPart.LPPart AS Part, Tbl_LPPart.PartUnitId, Tbl_PartUnit.PartUnit, IsActive,PartCode " &
                      " FROM         Tbl_PartUnit RIGHT OUTER JOIN " &
                      " Tbl_LPPart ON Tbl_PartUnit.PartUnitId = Tbl_LPPart.PartUnitId " & " ORDER BY Part, PartUnit"
                dg.DataSource = DatasetCcReq1.View_Parts
                TableName = "View_Parts"
                dg.CaptionText = "شبكه فشار ضعيف: تجهيرزات و قطعات"
                btnPartFactories.Show()
            Case "Tbl_MPPart"
                SQL = "SELECT     Tbl_MPPart.MPPartId AS PartId, Tbl_MPPart.MPPart AS Part, Tbl_MPPart.PartUnitId, Tbl_PartUnit.PartUnit, IsActive,PartCode " &
                      " FROM         Tbl_PartUnit RIGHT OUTER JOIN " &
                      " Tbl_MPPart ON Tbl_PartUnit.PartUnitId = Tbl_MPPart.PartUnitId " & " ORDER BY Part, PartUnit"
                dg.DataSource = DatasetCcReq1.View_Parts
                TableName = "View_Parts"
                dg.CaptionText = "شبكه فشار متوسط: تجهيزات و قطعات"
                btnPartFactories.Show()
            Case "Tbl_LightPart"
                SQL = "SELECT     Tbl_LightPart.LightPartId AS PartId, Tbl_LightPart.LightPart AS Part, Tbl_LightPart.PartUnitId, Tbl_PartUnit.PartUnit, IsActive,PartCode " &
                      " FROM         Tbl_PartUnit RIGHT OUTER JOIN " &
                      " Tbl_LightPart ON Tbl_PartUnit.PartUnitId = Tbl_LightPart.PartUnitId " & " ORDER BY Part, PartUnit"
                dg.DataSource = DatasetCcReq1.View_Parts
                TableName = "View_Parts"
                dg.CaptionText = "روشنايي معابر: تجهيزات و قطعات"
                btnPartFactories.Show()
            Case "View_MPPostTrans"
                btnShowOnGISMap.Visible = True
                If OwnerID >= 0 Then
                    SQL = "SELECT * FROM View_MPPostTrans where MPPostID=" & OwnerID & " ORDER BY MPPostName, SortOrder, MPPostTrans"
                Else
                    SQL = "SELECT * FROM View_MPPostTrans" & FilterQuery & " ORDER BY MPPostName, SortOrder, MPPostTrans"
                End If
                TableName = "View_MPPostTrans"
                dg.DataSource = DatasetCcReq1.View_MPFeeder
                dg.CaptionText = "شبكه فشار متوسط: ترانسهاي فوق توزيع"
            Case "ViewLPPostLoad"
                If OwnerID >= 0 Then
                    SQL = "SELECT * FROM ViewLPPostLoad where LPPostID=" & OwnerID & " ORDER BY LPPostName, LoadDateTimePersian"
                Else
                    SQL = "SELECT * FROM ViewLPPostLoad" & FilterQuery & " ORDER BY ORDER BY LPPostName, LoadDateTimePersian"
                End If
                TableName = "ViewLPPostLoad"
                dg.DataSource = DatasetCcReq1.ViewLPPostLoad
                dg.CaptionText = "شبكه فشار ضعيف: فهرست بارگيري هاي پست توزيع"
            Case "TblRecloserFunction"
                SQL = "SELECT rf.*,f.MPFeederName, k.KeyName From TblRecloserFunction rf
                        INNER JOIN tbl_MPfeeder f ON f.MPFeederId = rf.MPFeederId
                        INNER JOIN Tbl_MPFeederKey k ON k.MPFeederKeyId = rf.MPFeederKeyId
                        where rf.MPFeederKeyId = " & OwnerID
                BindingTable(SQL, mCnn, lDs, "TblRecloserFunction")
                dg.DataSource = lDs
                TableName = "TblRecloserFunction"
                dg.CaptionText = "شبكه فشار متوسط: فهرست عملکرد ريکلوزرها"
            Case "ViewLPPostEarth"
                If OwnerID >= 0 Then
                    SQL = "SELECT * FROM ViewLPPostEarth where LPPostID=" & OwnerID & " AND LPPostEarthId IS NULL ORDER BY LPPostName, ReadDatePersian, ReadTime"
                Else
                    SQL = "SELECT * FROM ViewLPPostEarth" & FilterQuery & " ORDER BY ORDER BY LPPostName, ReadDatePersian, ReadTime"
                End If
                BindingTable(SQL, mCnn, lDs, "View_LPPostEarth")
                dg.DataSource = lDs
                TableName = "ViewLPPostEarth"
                dg.CaptionText = "شبكه فشار ضعيف: فهرست ارت گيري هاي پست توزيع"
            Case "ViewLPPostPointEarth"
                If OwnerID >= 0 Then
                    SQL = "SELECT * FROM ViewLPPostEarth where LPPostEarthId=" & OwnerID & " ORDER BY LPPostEarthName, ReadDatePersian, ReadTime"
                Else
                    SQL = "SELECT * FROM ViewLPPostEarth" & FilterQuery & " ORDER BY ORDER BY LPPostName, ReadDatePersian, ReadTime"
                End If
                BindingTable(SQL, mCnn, lDs, "View_LPPostEarth")
                dg.DataSource = lDs
                TableName = "ViewLPPostPointEarth"
                dg.CaptionText = "شبكه فشار ضعيف: فهرست ارت گيري هاي نقاط پست توزيع"
            Case "ViewLPFeederLoad"
                If OwnerID >= 0 Then
                    SQL = "SELECT * FROM ViewLPFeederLoad where LPFeederID=" & OwnerID & " ORDER BY LPFeederName, LoadDateTimePersian"
                Else
                    SQL = "SELECT * FROM ViewLPFeederLoad" & FilterQuery & " ORDER BY ORDER BY LPFeederName, LoadDateTimePersian"
                End If
                TableName = "ViewLPFeederLoad"
                dg.DataSource = DatasetCcReq1.ViewLPFeederLoad
                dg.CaptionText = "شبكه فشار ضعيف: فهرست بارگيري هاي فيدرهاي فشار ضعيف"
            Case "ViewLPFeederPointsInfo"
                If OwnerID >= 0 Then
                    SQL = "SELECT * FROM ViewLPFeederPointsInfo where LPFeederPointId=" & OwnerID & " ORDER BY LPFeederPointName, MeasureDatePersian"
                Else
                    SQL = "SELECT * FROM ViewLPFeederPointsInfo" & FilterQuery & " ORDER BY ORDER BY LPFeederPointName, MeasureDatePersian"
                End If
                TableName = "ViewLPFeederPointsInfo"
                dg.DataSource = DatasetCcReq1.Tables("ViewLPFeederPointsInfo")
                dg.CaptionText = "شبكه فشار ضعيف: فهرست ولتاژ گيري هاي نقاط انتهاي فيدر"
            Case "ViewMPFeederPointsInfo"
                If OwnerID >= 0 Then
                    SQL = "SELECT * FROM ViewMPFeederPointsInfo where  MPFeederPointId=" & OwnerID & " ORDER BY MPFeederPointName, MeasureDatePersian"
                Else
                    SQL = "SELECT * FROM ViewMPFeederPointsInfo" & FilterQuery & " ORDER BY ORDER BY MPFeederPointName, MeasureDatePersian"
                End If
                TableName = "ViewMPFeederPointsInfo"
                dg.DataSource = DatasetCcReq1.Tables("ViewMPFeederPointsInfo")
                dg.CaptionText = "شبكه فشار متوسط: فهرست ولتاژ گيري هاي نقاط انتهاي فيدر"
            Case "Tbl_LPPost", "Tbl_LPPostCommonFeeder", "Tbl_LPPostCommonLightFeeder"
                If OwnerID >= 0 Then
                    SQL = "SELECT * FROM View_LPPost where MPFeederID=" & OwnerID & LAA & IIf(mWhere <> "", " AND " & mWhere, "") & " ORDER BY Area, MPFeederName, LPPostName, LPPostType"
                Else
                    If IsCenter Then
                        SQL = "SELECT * FROM View_LPPost " & IIf(mWhere <> "", "WHERE " & mWhere & LAA, LAW) & " ORDER BY Area, MPFeederName, LPPostName, LPPostType"
                    Else
                        SQL = "SELECT * FROM View_LPPost " & IIf(mWhere <> "", "WHERE " & mWhere, "") & " ORDER BY Area, MPFeederName, LPPostName, LPPostType"
                    End If

                End If
                TableName = "View_LPPost"
                dg.DataSource = DatasetCcReq1.View_LPPost
                bttn_LPFeeder.Visible = True
                btnChangeLPFeederArea.Visible = True

                If BT_Name = "Tbl_LPPost" Then
                    btnShowOnGISMap.Visible = True
                End If

                Dim lIsAccess As Boolean = False
                lIsAccess = CheckUserTrustee("CanChangeAllLPFeederArea", 9) Or IsAdmin()
                btnChangeLPFeederArea.Enabled = lIsAccess

                If IsBazdidApp Then
                    btnSavabeghBazdid.Visible = True
                End If
                bttn_LPPostLoad.Visible = True
                btn_LPPostEarth.Visible = True
                btnLPPostEarthPoint.Visible = True
                dg.CaptionText = "شبكه فشار ضعيف: پست هاي توزيع"
            Case "Tbl_LPFeeder", "Tbl_LPFeederCommonFeeder", "Tbl_LightFeeder", "Tbl_LightFeederCommonFeeder"
                If OwnerID >= 0 Then
                    SQL = "SELECT * FROM View_LPFeeder where LPPostID=" & OwnerID & LAA & IIf(mWhere <> "", " AND " & mWhere, "") & " ORDER BY MPPostName, MPFeederName, LPPostName, LPFeederName"
                Else
                    If IsCenter Then
                        SQL = "SELECT * FROM View_LPFeeder " & IIf(mWhere <> "", "WHERE " & mWhere & LAA, LAW) & " ORDER BY MPPostName, MPFeederName, LPPostName, LPFeederName"
                    Else
                        SQL = "SELECT * FROM View_LPFeeder " & IIf(mWhere <> "", "WHERE " & mWhere, "") & " ORDER BY MPPostName, MPFeederName, LPPostName, LPFeederName"
                    End If

                End If
                TableName = "View_LPFeeder"
                dg.DataSource = DatasetCcReq1.View_LPFeeder
                bttn_LPFeederLoad.Visible = True
                If BT_Name = "Tbl_LPFeeder" Then
                    btnShowOnGISMap.Visible = True
                End If

                If IsBazdidApp Then
                    btnSavabeghBazdid.Visible = True
                End If
                dg.CaptionText = "شبكه فشار ضعيف: فيدرهاي فشار ضعيف"
            Case "TblEkipProfile"
                'BindingTable("SELECT Tbl_Area.* FROM Tbl_Area INNER JOIN Tbl_City ON Tbl_Area.CityId = Tbl_City.CityId WHERE Tbl_City.ProvinceId=" & WorkingProvinceId & " AND AreaID<>1 ORDER BY Area", mCnn, DatasetCcReq1, "Tbl_Area")
                LoadAreaDataTable(mCnn, DatasetCcReq1, cboArea, True, False, True)
                Label5.Visible = True
                cboArea.Visible = True
                btnSearchView.Visible = True
                pnlEkipNetworkType.Visible = True
                If IsCenter Then
                    cboArea.Enabled = True
                    If OwnerID > -1 Then
                        cboArea.Enabled = False
                        cboArea.SelectedValue = OwnerID
                    End If
                    If cboArea.SelectedIndex > -1 Then
                        SQL = "SELECT * FROM TblEkipProfile Where ApplicationId = " & IIf(mApplicationType = mdlHashemi.ApplicationTypes.TamirRequest, ApplicationTypes.Havades, mApplicationType) & " AND AreaID=" & cboArea.SelectedValue & IIf(SelectMode Or mIsShowActiveEkipOnly, " AND IsActive=1", "") & " ORDER BY EkipProfileName"
                    End If
                Else
                    SQL = "SELECT * FROM TblEkipProfile Where ApplicationId = " & IIf(mApplicationType = mdlHashemi.ApplicationTypes.TamirRequest, ApplicationTypes.Havades, mApplicationType) & " AND AreaID=" & WorkingAreaId & IIf(SelectMode Or mIsShowActiveEkipOnly, " AND IsActive=1", "") & " ORDER BY EkipProfileName"
                    cboArea.SelectedValue = WorkingAreaId
                    cboArea.Enabled = False
                End If
                dg.DataSource = DatasetCcReq1.TblEkipProfile
                dg.CaptionText = "اكيپ ها"
            Case "Tbl_Zone"
                'LoadAreaDataTable(mCnn, DatasetCcReq1, cboArea, True)
                'Label5.Visible = True
                'cboArea.Visible = True
                btnZoneMPFeeders.Visible = True
                If IsCenter Then
                    SQL = "" &
                        " SELECT Tbl_Zone.*, Tbl_Area.Area " &
                        " FROM Tbl_Zone INNER JOIN Tbl_Area ON Tbl_Zone.AreaId = Tbl_Area.AreaId" &
                        IIf(LAW <> "",
                            LAW.Replace("AreaId", "Tbl_Zone.AreaId"),
                            IIf(Not IsSetadMode,
                            " WHERE " & lAllCenterArea.Replace("AreaId", "Tbl_Zone.AreaId"),
                            "")) &
                        " ORDER BY Tbl_Area.Area, Tbl_Zone.ZoneName"
                Else
                    SQL = "" &
                        " SELECT Tbl_Zone.*, Tbl_Area.Area " &
                        " FROM Tbl_Zone INNER JOIN Tbl_Area ON Tbl_Zone.AreaId = Tbl_Area.AreaId" &
                        " WHERE Tbl_Zone.AreaId = " & WorkingAreaId &
                        " ORDER BY Tbl_Area.Area, Tbl_Zone.ZoneName"
                End If
                TableName = "Tbl_Zone"
                dg.DataSource = DatasetCcReq1.Tbl_Zone
                dg.CaptionText = "فهرست مناطق"
                btnZonePreCodes.Visible = True
            Case "Tbl_Section"
                If IsCenter Then
                    SQL = "" &
                        " SELECT Tbl_Section.*, Tbl_Area.Area " &
                        " FROM Tbl_Section INNER JOIN Tbl_Area ON Tbl_Section.AreaId = Tbl_Area.AreaId" &
                        IIf(LAW <> "",
                            LAW.Replace("AreaId", "Tbl_Section.AreaId"),
                            IIf(Not IsSetadMode,
                            " WHERE " & lAllCenterArea.Replace("AreaId", "Tbl_Section.AreaId"),
                            "")) &
                        " ORDER BY Tbl_Area.Area, Tbl_Section.SectionName"
                Else
                    SQL = "" &
                        " SELECT Tbl_Section.*, Tbl_Area.Area " &
                        " FROM Tbl_Section INNER JOIN Tbl_Area ON Tbl_Section.AreaId = Tbl_Area.AreaId" &
                        " WHERE Tbl_Section.AreaId = " & WorkingAreaId &
                        " ORDER BY Tbl_Area.Area, Tbl_Section.SectionName"
                End If
                TableName = "Tbl_Section"
                dg.DataSource = DatasetCcReq1.Tbl_Section
                dg.CaptionText = "فهرست بخش ها"
            Case "Tbl_Town"
                If IsCenter Then
                    SQL = "" &
                        " SELECT Tbl_Town.*, Tbl_Area.Area , Tbl_Section.SectionName " &
                        " FROM Tbl_Town  INNER JOIN   Tbl_Section on Tbl_Section.SectionId = Tbl_Town.SectionId INNER JOIN Tbl_Area ON Tbl_Area.AreaId = Tbl_Section.AreaId  " &
                        IIf(LAW <> "", LAW,
                            IIf(Not IsSetadMode,
                            " WHERE " & lAllCenterArea,
                            "")) &
                        " ORDER BY Tbl_Area.Area , Tbl_Section.SectionName, Tbl_Town.TownName"
                Else
                    SQL = "" &
                        " SELECT Tbl_Town.*, Tbl_Area.Area, Tbl_Section.SectionName " &
                        " FROM Tbl_Town INNER JOIN Tbl_Area ON Tbl_Area.AreaId = Tbl_Section.AreaId inner join  Tbl_Section on Tbl_Section.SectionId = Tbl_Town.SectionId " &
                        " WHERE Tbl_Area.AreaId = " & WorkingAreaId &
                        " ORDER BY Tbl_Area.Area , Tbl_Section.SectionName, Tbl_Town.TownName"
                End If
                TableName = "Tbl_Town"
                dg.DataSource = DatasetCcReq1.Tbl_Town
                dg.CaptionText = "فهرست شهر ها"
            Case "Tbl_Village"
                If IsCenter Then
                    SQL = "" &
                        " SELECT Tbl_Village.*, Tbl_Area.Area, Tbl_Section.SectionName,Tbl_Town.TownName " &
                        " FROM Tbl_Village left join Tbl_Town on Tbl_Village.TownId = Tbl_Town.TownId " &
                        " left join Tbl_Section   on  Tbl_Town.SectionId  = Tbl_Section.SectionId  " &
                        " left JOIN Tbl_Area ON Tbl_Section.AreaId = Tbl_Area.AreaId" &
                        IIf(LAW <> "",
                            LAW.Replace("AreaId", "Tbl_Village.AreaId"),
                            IIf(Not IsSetadMode,
                            " WHERE " & lAllCenterArea.Replace("AreaId", "Tbl_Village.AreaId"),
                            "")) &
                        " ORDER BY Tbl_Area.Area, Tbl_Village.VillageName"
                Else
                    SQL = "" &
                        " SELECT Tbl_Village.*, Tbl_Area.Area, Tbl_Section.SectionName,Tbl_Town.TownName " &
                        " FROM Tbl_Village left join Tbl_Town on Tbl_Village.TownId = Tbl_Town.TownId" &
                          " left join Tbl_Section   on  Tbl_Town.SectionId  = Tbl_Section.SectionId  " &
                        " left JOIN Tbl_Area ON Tbl_Section.AreaId = Tbl_Area.AreaId" &
                        " WHERE Tbl_Village.AreaId = " & WorkingAreaId &
                        " ORDER BY Tbl_Area.Area, Tbl_Section.SectionName,Tbl_Town.TownName, Tbl_Village.VillageName"
                End If
                TableName = "Tbl_Village"
                dg.DataSource = DatasetCcReq1.Tbl_Village
                dg.CaptionText = "فهرست دهستان ها"

            Case "Tbl_Roosta"
                If IsCenter Then
                    SQL = "" &
                        " SELECT Tbl_Roosta.*, Tbl_Area.Area , Tbl_Section.SectionName, Tbl_Town.TownName, Tbl_Village.VillageName " &
                        " FROM Tbl_Roosta  INNER JOIN Tbl_Village  ON Tbl_Roosta.VillageId = Tbl_Village.VillageId  INNER JOIN   Tbl_Town  ON Tbl_Village.TownId = Tbl_Town.TownId INNER JOIN Tbl_Section  ON Tbl_Town.SectionId = Tbl_Section.SectionId  INNER JOIN Tbl_Area on Tbl_Area.AreaId = Tbl_Section.AreaId  " &
                        IIf(LAW <> "",
                            LAW.Replace("AreaId", "Tbl_Village.AreaId"),
                            IIf(Not IsSetadMode,
                            " WHERE " & lAllCenterArea.Replace("AreaId", "Tbl_Village.AreaId"),
                            "")) &
                        " ORDER BY Tbl_Area.Area , Tbl_Section.SectionName, Tbl_Town.TownName, Tbl_Village.VillageName,Tbl_Roosta.RoostaName"
                Else
                    SQL = "" &
                        " SELECT Tbl_Roosta.*, Tbl_Area.Area , Tbl_Section.SectionName, Tbl_Town.TownName, Tbl_Village.VillageName" &
                        " FROM Tbl_Roosta  INNER JOIN Tbl_Village  ON Tbl_Roosta.VillageId = Tbl_Village.VillageId  INNER JOIN   Tbl_Town  ON Tbl_Village.TownId = Tbl_Town.TownId INNER JOIN Tbl_Section  ON Tbl_Town.SectionId = Tbl_Section.SectionId  INNER JOIN Tbl_Area on Tbl_Area.AreaId = Tbl_Section.AreaId " &
                        " WHERE Tbl_Village.AreaId = " & WorkingAreaId &
                        " ORDER BY Tbl_Area.Area , Tbl_Section.SectionName, Tbl_Town.TownName, Tbl_Village.VillageName,Tbl_Roosta.RoostaName"
                End If
                TableName = "Tbl_Roosta"
                dg.DataSource = DatasetCcReq1.Tbl_Roosta
                dg.CaptionText = "فهرست روستا ها"
            Case "Tbl_AVLCar"
                LoadAreaDataTable(mCnn, DatasetCcReq1, cboArea, True, False, True)
                Label5.Visible = True
                cboArea.Visible = True
                If IsCenter Then
                    cboArea.Enabled = True
                    If OwnerID > -1 Then
                        cboArea.Enabled = False
                        cboArea.SelectedValue = OwnerID
                    End If
                    If cboArea.SelectedIndex > -1 Then
                        SQL =
                            " SELECT Tbl_AVLCar.*, Tbl_Area.Area " &
                            " FROM Tbl_AVLCar INNER JOIN Tbl_Area ON Tbl_AVLCar.AreaId = Tbl_Area.AreaId" &
                            " WHERE Tbl_AVLCar.AreaId = " & cboArea.SelectedValue & IIf(SelectMode, " AND IsActive=1", "") &
                            " ORDER BY Tbl_Area.Area, Tbl_AVLCar.AVLCarName"
                    Else
                        SQL = "" &
                            " SELECT Tbl_AVLCar.*, Tbl_Area.Area " &
                            " FROM Tbl_AVLCar INNER JOIN Tbl_Area ON Tbl_AVLCar.AreaId = Tbl_Area.AreaId" &
                            IIf(LAW <> "",
                                LAW.Replace("AreaId", "Tbl_AVLCar.AreaId"),
                                IIf(Not IsSetadMode,
                                " WHERE " & lAllCenterArea.Replace("AreaId", "Tbl_AVLCar.AreaId"),
                                "")) & IIf(SelectMode, " AND IsActive=1", "") &
                            " ORDER BY Tbl_Area.Area, Tbl_AVLCar.AVLCarName"
                    End If

                Else
                    SQL =
                        " SELECT Tbl_AVLCar.*, Tbl_Area.Area " &
                        " FROM Tbl_AVLCar INNER JOIN Tbl_Area ON Tbl_AVLCar.AreaId = Tbl_Area.AreaId" &
                        " WHERE Tbl_AVLCar.AreaId = " & WorkingAreaId &
                        IIf(SelectMode, " AND IsActive = 1", "") &
                        " ORDER BY Tbl_Area.Area, Tbl_AVLCar.AVLCarName"
                    cboArea.SelectedValue = WorkingAreaId
                    cboArea.Enabled = False
                End If
                TableName = "Tbl_AVLCar"
                dg.DataSource = DatasetCcReq1.Tbl_AVLCar
                dg.CaptionText = "فهرست خودروها"
            Case "Tbl_DisconnectGroupSetGroup"
                SQL = "SELECT * FROM Tbl_DisconnectGroupSetGroup ORDER BY SortOrder"
                dg.DataSource = DatasetCcReq1.Tbl_DisconnectGroupSetGroup
                TableName = "Tbl_DisconnectGroupSetGroup"
                dg.CaptionText = "گروه‌هاي علل قطع"
                lDs = DatasetErja1
            Case "Tbl_MPFeederKey"
                btnShowOnGISMap.Visible = True

                LAA = LAA.Replace("AreaId", "Tbl_Area.AreaId")
                LAW = LAW.Replace("AreaId", "Tbl_Area.AreaId")
                SQL =
                "SELECT DISTINCT " &
                " 	Tbl_MPFeederKey.*," &
                " 	Tbl_Area.Area," &
                " 	Tbl_MPPost.MPPostName," &
                " 	Tbl_MPFeeder.MPFeederName," &
                " 	Tbl_MPCloserType.MPCloserType," &
                " 	TblSpec.SpecValue AS Factory," &
                "   tManovr.MPFeederName As ManovrMPFeederName, " &
                "   CASE WHEN ISNULL(ManovrMPFeederId,-1) > -1 THEN CAST(1 as bit) ELSE CAST(0 as bit) END AS IsManovr, " &
                "   ISNULL(Tbl_MPFeeder.CoverPercentage, 100) AS CoverPercentage " &
                " FROM " &
                " 	Tbl_MPFeederKey" &
                " 	INNER JOIN Tbl_MPFeeder ON Tbl_MPFeederKey.MPFeederId = Tbl_MPFeeder.MPFeederId" &
                " 	INNER JOIN Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId" &
                "   LEFT JOIN Tbl_MPCommonFeeder on Tbl_MPFeeder.MPFeederId = Tbl_MPCommonFeeder.MPFeederId " &
                " 	INNER JOIN Tbl_Area ON Tbl_MPFeeder.AreaId = Tbl_Area.AreaId " &
                        IIf(IsSetadMode, "", " OR (Tbl_Area.AreaId = Tbl_MPCommonFeeder.AreaId " & IIf(IsCenter, " AND NOT Tbl_MPFeeder.AreaId IN (" & WorkingCenterAreaIDs & "))", ")")) &
                " 	LEFT JOIN Tbl_MPCloserType ON Tbl_MPFeederKey.MPCloserTypeId = Tbl_MPCloserType.MPCloserTypeId" &
                " 	LEFT JOIN TblSpec ON Tbl_MPFeederKey.spcFactoryId = TblSpec.SpecId " &
                " 	LEFT JOIN Tbl_MPFeeder tManovr ON Tbl_MPFeederKey.ManovrMPFeederId = tManovr.MPFeederId"

                Dim lWhere As String = mWhere.Replace("CoverPercentage", "ISNULL(Tbl_MPFeeder.CoverPercentage, 100)")

                If OwnerID >= 0 Then
                    SQL &= " where MPFeederKeyId = " & OwnerID & LAA & IIf(lWhere <> "", " AND " & lWhere, "") & " ORDER BY Area, MPPostName, Tbl_MPFeeder.MPFeederName, KeyName "
                Else
                    If IsCenter Then
                        SQL &= IIf(lWhere <> "", " WHERE " & lWhere & LAA, LAW) & " ORDER BY Area, MPPostName, Tbl_MPFeeder.MPFeederName, KeyName "
                    Else
                        SQL &= IIf(lWhere <> "", " WHERE " & lWhere, "") & " ORDER BY Area, MPPostName, Tbl_MPFeeder.MPFeederName, KeyName "
                    End If

                End If
                TableName = "Tbl_MPFeederKey"
                dg.DataSource = DatasetCcReq1.Tables("Tbl_MPFeederKey")
                dg.CaptionText = "فهرست کليد فيدرهاي فشار متوسط"

            Case "Tbl_AllowValidator"
                If IsCenter Then
                    SQL = "" &
                        " SELECT Tbl_AllowValidator.*, ISNULL(Tbl_Area.Area, 'همه نواحي') AS Area " &
                        " FROM Tbl_AllowValidator LEFT JOIN Tbl_Area on Tbl_Area.AreaId = Tbl_AllowValidator.AreaId  " &
                        IIf(LAW <> "",
                            LAW.Replace("AreaId", "Tbl_AllowValidator.AreaId"),
                            IIf(Not IsSetadMode,
                            " WHERE " & lAllCenterArea.Replace("AreaId", "Tbl_AllowValidator.AreaId"),
                            "")) &
                        " ORDER BY ISNULL(Tbl_Area.Area, 'همه نواحي') , Tbl_AllowValidator.AllowValidatorName "
                Else
                    SQL = "" &
                        " SELECT Tbl_AllowValidator.*, ISNULL(Tbl_Area.Area, 'همه نواحي') as Area " &
                        " FROM Tbl_AllowValidator LEFT JOIN Tbl_Area on Tbl_Area.AreaId = Tbl_AllowValidator.AreaId " &
                        " WHERE Tbl_AllowValidator.AreaId IS NULL OR Tbl_AllowValidator.AreaId = " & WorkingAreaId &
                        " ORDER BY ISNULL(Tbl_Area.Area, 'همه نواحي') , Tbl_AllowValidator.AllowValidatorName "
                End If
                TableName = "Tbl_AllowValidator"
                dg.DataSource = DatasetCcReq1.Tables("Tbl_AllowValidator")
                dg.CaptionText = "فهرست صادر/ابطال کنندگان اجازه کار"


                '--------------------------------------
                '       Bazdid Applicatin Part
                '--------------------------------------
            Case "View_BazdidCheckList"
                SQL = "SELECT * FROM View_BazdidCheckList " &
                IIf(OwnerID > 0, " WHERE BazdidCheckListGroupId = " & OwnerID, "") &
                " ORDER BY CheckListCode"
                TableName = "View_BazdidCheckList"
                dg.DataSource = DatasetBazdidView1.View_BazdidCheckList
                dg.CaptionText = "فهرست چک ليست‌ها"
                lDs = DatasetBazdidView1
                btnSubCheckList.Visible = True
                'btnCheckListTajhizat.Visible = True
                btnCheckListPart.Visible = True
                btnActive_DeActive.Visible = True
                btnSearchView.Visible = True

                If Not IsSetadMode Then
                    ButtonDelete.Enabled = False
                End If
            Case "View_BazdidCheckListSubGroup"
                SQL = "SELECT * FROM View_BazdidCheckList " &
                IIf(OwnerID > 0, " WHERE BazdidCheckListSubGroupId = " & OwnerID, "") &
                " ORDER BY CheckListCode"
                TableName = "View_BazdidCheckList"
                dg.DataSource = DatasetBazdidView1.View_BazdidCheckList
                dg.CaptionText = "فهرست چک ليست‌ها"
                lDs = DatasetBazdidView1
                btnSubCheckList.Visible = True
                'btnCheckListTajhizat.Visible = True
                btnCheckListPart.Visible = True
                btnActive_DeActive.Visible = True
                btnSearchView.Visible = True

                If Not IsSetadMode Then
                    ButtonDelete.Enabled = False
                End If
            Case "BTbl_BazdidCheckListGroup"
                SQL =
                    "SELECT  " &
                    "	BTbl_BazdidCheckListGroup.BazdidCheckListGroupId, " &
                    "	BTbl_BazdidCheckListGroup.BazdidCheckListGroupName, " &
                    "	BTbl_BazdidType.BazdidTypeName, " &
                    "	BTbl_BazdidCheckListGroup.BazdidCheckListIcon, " &
                    "	ISNULL(View_CheckListCount.CheckListCount,0) AS CheckListCount " &
                    "FROM  " &
                    "	BTbl_BazdidCheckListGroup " &
                    "	INNER JOIN BTbl_BazdidType ON BTbl_BazdidCheckListGroup.BazdidTypeId = BTbl_BazdidType.BazdidTypeId " &
                    "	LEFT JOIN " &
                    "	( " &
                    "	SELECT  " &
                    "		BazdidCheckListGroupId, " &
                    "		COUNT(BazdidCheckListId) AS CheckListCount " &
                    "	FROM  " &
                    "		BTbl_BazdidCheckList " &
                    "	GROUP BY " &
                    "		BazdidCheckListGroupId " &
                    "	) AS View_CheckListCount ON BTbl_BazdidCheckListGroup.BazdidCheckListGroupId = View_CheckListCount.BazdidCheckListGroupId " &
                    "ORDER BY  " &
                    "	BTbl_BazdidCheckListGroup.BazdidCheckListGroupName "
                TableName = "BTbl_BazdidCheckListGroup"
                dg.DataSource = DatasetBazdid1.BTbl_BazdidCheckListGroup
                dg.CaptionText = "فهرست گروه‌بندي چک ليست‌ها"
                lDs = DatasetBazdid1
                btnGroupChekList.Visible = True
            Case "BTbl_SubCheckList"
                'ButtonDelete.Visible = False
                SQL =
                    " SELECT t1.SubCheckListId, t3.BazdidCheckListGroupName, t2.CheckListName, t1.SubCheckListName, t1.SubCheckListCode, t1.BazdidCheckListId, t1.ErjaPriority " &
                    " FROM BTbl_SubCheckList t1 " &
                    " INNER JOIN BTbl_BazdidCheckList t2 ON t1.BazdidCheckListId = t2.BazdidCheckListId  " &
                    " INNER JOIN BTbl_BazdidCheckListGroup t3 ON t2.BazdidCheckListGroupId = t3.BazdidCheckListGroupId " &
                    " LEFT JOIN BTbl_LPPostDetail ON t2.LPPostDetailId = BTbl_LPPostDetail.LPPostDetailId " &
                    " LEFT JOIN BTbl_BazdidCheckListSubGroup ON t2.BazdidCheckListSubGroupId = BTbl_BazdidCheckListSubGroup.BazdidCheckListSubGroupId " &
                    IIf(OwnerID > -1, " WHERE t1.BazdidCheckListId = " & OwnerID, "")
                TableName = "BTbl_SubCheckList"
                dg.DataSource = DatasetBazdid1.BTbl_SubCheckList
                dg.CaptionText = "فهرست ريز خرابي‌ها"
                lDs = DatasetBazdid1
            Case "BTbl_Tajhizat"
                SQL =
                    " SELECT BTbl_Tajhizat.TajhizatId, BTbl_Tajhizat.TajhizatName, BTbl_BazdidType.BazdidTypeName " &
                    " FROM BTbl_Tajhizat LEFT OUTER JOIN BTbl_BazdidType ON BTbl_Tajhizat.BazdidTypeId = BTbl_BazdidType.BazdidTypeId "

                TableName = "BTbl_Tajhizat"
                dg.DataSource = DatasetBazdid1.BTbl_Tajhizat
                dg.CaptionText = "تجهيزات"
                lDs = DatasetBazdid1
            Case "BTbl_ServicePart"
                SQL =
                    " SELECT t1.ServicePartId, t1.ServicePartName, t3.PartUnit, t2.BazdidTypeName, t1.ServicePartCode, t1.ServicePrice, t1.PriceOne " &
                    " FROM BTbl_ServicePart t1 LEFT OUTER JOIN BTbl_BazdidType t2 ON t1.BazdidTypeId = t2.BazdidTypeId " &
                    " LEFT JOIN Tbl_PartUnit t3 ON t1.PartUnitId = t3.PartUnitId"

                TableName = "BTbl_ServicePart"
                dg.DataSource = DatasetBazdid1.BTbl_ServicePart
                dg.CaptionText = "قطعات"
                btnImportExcel.Visible = True
                If Not IsSetadMode Then
                    btnImportExcel.Enabled = False
                End If
                btnSearchView.Visible = True
                lDs = DatasetBazdid1
            Case "BTblBazdidEkip"
                LoadAreaDataTable(mCnn, DatasetCcReq1, cboArea)
                Label5.Visible = True
                cboArea.Visible = True
                If IsCenter Then
                    cboArea.Enabled = True
                    If OwnerID > -1 Then
                        cboArea.Enabled = False
                        cboArea.SelectedValue = OwnerID
                    End If
                    If cboArea.SelectedIndex > -1 Then
                        SQL = "SELECT * FROM BTblBazdidEkip Where AreaID=" & cboArea.SelectedValue & " ORDER BY BazdidEkipName"
                    End If
                Else
                    SQL = "SELECT * FROM BTblBazdidEkip Where AreaID=" & WorkingAreaId & " ORDER BY BazdidEkipName"
                    cboArea.SelectedValue = WorkingAreaId
                    cboArea.Enabled = False
                End If
                dg.DataSource = DatasetBazdid1.BTblBazdidEkip
                dg.CaptionText = "اكيپ ها"
                lDs = DatasetBazdid1
            Case "BTbl_BazdidMaster"
                LoadAreaDataTable(mCnn, DatasetCcReq1, cboArea)
                Dim ActiveStr As String = ""
                If SelectMode And Not IsShowAllMaster Then
                    ActiveStr = " And IsActive=1"
                End If
                If IsCenter Then
                    Label5.Visible = True
                    cboArea.Visible = True
                    If OwnerID > -1 Then
                        cboArea.Enabled = False
                        cboArea.SelectedValue = OwnerID
                    End If
                    If cboArea.SelectedIndex > -1 Then
                        SQL = "SELECT * FROM BTbl_BazdidMaster Where AreaID=" & cboArea.SelectedValue & ActiveStr & " ORDER BY Name, Address, Tel, Mobile"
                    Else
                        SQL = "SELECT * FROM BTbl_BazdidMaster Where " & ActiveStr & LAA & " ORDER BY Name, Address, Tel, Mobile"
                        SQL = SQL.Replace("Where  And", "Where")
                        SQL = SQL.Replace("Where  ORDER", "ORDER")
                    End If
                    SQL = SQL.Replace("Where  And", "Where")
                    SQL = SQL.Replace("Where  ORDER", "ORDER")
                Else
                    SQL = "SELECT * FROM BTbl_BazdidMaster Where AreaID = " & WorkingAreaId & ActiveStr & " ORDER BY Name, Address, Tel, Mobile"
                    cboArea.SelectedValue = WorkingAreaId
                End If

                dg.DataSource = DatasetBazdid1.BTbl_BazdidMaster
                dg.CaptionText = "تکنسين / کارشناس بازديد"
                lDs = DatasetBazdid1
            Case "BTbl_BazdidCheckListSubGroup"
                SQL =
                    "SELECT  " &
                    "	BTbl_BazdidCheckListSubGroup.BazdidCheckListSubGroupId, " &
                    "	BTbl_BazdidCheckListGroup.BazdidCheckListGroupName, " &
                    "	BTbl_BazdidCheckListSubGroup.BazdidCheckListSubGroupName, " &
                    "	BTbl_BazdidCheckListSubGroup.BazdidCheckListSubGroupIcon, " &
                    "	ISNULL(View_CheckListCount.CheckListCount,0) AS CheckListCount " &
                    "FROM  " &
                    "	BTbl_BazdidCheckListSubGroup " &
                    "	INNER JOIN BTbl_BazdidCheckListGroup ON BTbl_BazdidCheckListSubGroup.BazdidCheckListGroupId = BTbl_BazdidCheckListGroup.BazdidCheckListGroupId " &
                    "	LEFT JOIN " &
                    "	( " &
                    "	SELECT  " &
                    "		BazdidCheckListSubGroupId, " &
                    "		COUNT(BazdidCheckListId) AS CheckListCount " &
                    "	FROM  " &
                    "		BTbl_BazdidCheckList " &
                    "	GROUP BY " &
                    "		BazdidCheckListSubGroupId " &
                    "	) AS View_CheckListCount ON BTbl_BazdidCheckListSubGroup.BazdidCheckListSubGroupId = View_CheckListCount.BazdidCheckListSubGroupId " &
                    IIf(OwnerID > 0, " WHERE BazdidCheckListGroupId = " & OwnerID, "") &
                    " ORDER BY  " &
                    "	BTbl_BazdidCheckListSubGroup.BazdidCheckListSubGroupName "

                TableName = "BTbl_BazdidCheckListSubGroup"
                dg.DataSource = DatasetBazdid1.BTbl_BazdidCheckListSubGroup
                dg.CaptionText = "فهرست زير گروه چک ليست‌ها"
                lDs = DatasetBazdid1
                btnGroupChekList.Visible = True
                'btnChekList.Visible = True
                If Not IsSetadMode Then
                    ButtonDelete.Enabled = False
                End If
            Case "BTbl_LPPostDetail"
                SQL =
                    " SELECT LPPostDetailId, IsUserCreated, LPPostDetailIcon,  LPPostDetailName, IsFix, " &
                    " LPPostType = CASE IsHavayi WHEN 1 THEN 'هوايي' ELSE 'زميني' END, LPPostDetailIcon " &
                    " FROM BTbl_LPPostDetail"
                TableName = "BTbl_LPPostDetail"
                dg.DataSource = DatasetBazdid1.BTbl_LPPostDetail
                dg.CaptionText = "فهرست بخشهاي پست توزيع"
                lDs = DatasetBazdid1
                '--------------------------------------
                '       Erja Applicatin Part
                '--------------------------------------
            Case "Tbl_ErjaReason"
                SQL =
                    " SELECT Tbl_ErjaReason.ErjaReasonId, Tbl_ErjaReason.ErjaReason, Tbl_ErjaReason.NetworkTypeId, Tbl_NetworkType.NetworkType, Tbl_ErjaReason.IsActive " &
                    " FROM Tbl_ErjaReason LEFT JOIN Tbl_NetworkType ON Tbl_ErjaReason.NetworkTypeId = Tbl_NetworkType.NetworkTypeId "
                dg.DataSource = DatasetErja1.Tbl_ErjaReason
                TableName = "Tbl_ErjaReason"
                dg.CaptionText = "دلايل ارجاع"
                lDs = DatasetErja1
                btnErjaCause.Visible = True
            Case "Tbl_ErjaCause"
                If OwnerID >= 0 Then
                    SQL =
                        " SELECT Tbl_ErjaCause.ErjaCauseId, Tbl_ErjaCause.ErjaCause, Tbl_ErjaCause.ErjaReasonId, Tbl_ErjaReason.ErjaReason " &
                        " FROM Tbl_ErjaCause INNER JOIN Tbl_ErjaReason ON Tbl_ErjaCause.ErjaReasonId = Tbl_ErjaReason.ErjaReasonId " &
                        " WHERE Tbl_ErjaCause.ErjaReasonId = " & OwnerID
                Else
                    SQL =
                        " SELECT Tbl_ErjaCause.ErjaCauseId, Tbl_ErjaCause.ErjaCause, Tbl_ErjaCause.ErjaReasonId, Tbl_ErjaReason.ErjaReason " &
                        " FROM Tbl_ErjaCause INNER JOIN Tbl_ErjaReason ON Tbl_ErjaCause.ErjaReasonId = Tbl_ErjaReason.ErjaReasonId "
                End If
                SQL &= " ORDER BY Tbl_ErjaCause.ErjaCause"
                dg.DataSource = DatasetErja1.Tbl_ErjaCause
                TableName = "Tbl_ErjaCause"
                dg.CaptionText = "علل اشکالات بوجود آمده ارجاع"
                lDs = DatasetErja1
            Case "Tbl_ErjaPart"
                SQL =
                    " SELECT Tbl_ErjaPart.ErjaPartId, Tbl_ErjaPart.ErjaPart, Tbl_ErjaPart.PartUnitId, Tbl_ErjaPart.ErjaReasonId, Tbl_ErjaPart.PartPrice, Tbl_ErjaReason.ErjaReason, ISNULL(tNT1.NetworkType, tNT2.NetworkType) As NetworkType, Tbl_PartUnit.PartUnit " &
                    " FROM Tbl_ErjaPart LEFT OUTER JOIN Tbl_PartUnit ON Tbl_ErjaPart.PartUnitId = Tbl_PartUnit.PartUnitId LEFT OUTER JOIN Tbl_ErjaReason ON Tbl_ErjaReason.ErjaReasonId = Tbl_ErjaPart.ErjaReasonId LEFT OUTER JOIN Tbl_NetworkType tNT1 ON Tbl_ErjaPart.NetworkTypeId = tNT1.NetworkTypeId LEFT OUTER JOIN Tbl_NetworkType tNT2 ON Tbl_ErjaReason.NetworkTypeId = tNT2.NetworkTypeId "
                dg.DataSource = DatasetErja1.Tbl_ErjaPart
                TableName = "Tbl_ErjaPart"
                dg.CaptionText = "قطعات مصرفي"
                lDs = DatasetErja1
            Case "Tbl_ErjaOperation"
                SQL =
                    " SELECT Tbl_ErjaOperation.ErjaOperationId, Tbl_ErjaOperation.ErjaOperation, Tbl_ErjaOperation.NetworkTypeId, Tbl_ErjaOperation.OperationPrice, Tbl_NetworkType.NetworkType " &
                    " FROM Tbl_ErjaOperation LEFT OUTER JOIN Tbl_NetworkType ON Tbl_ErjaOperation.NetworkTypeId = Tbl_NetworkType.NetworkTypeId "
                dg.DataSource = DatasetErja1.Tbl_ErjaOperation
                TableName = "Tbl_ErjaOperation"
                dg.CaptionText = "فهرست بها"
                lDs = DatasetErja1
            Case "Tbl_TamirOperation"
                SQL =
                    " SELECT Tbl_TamirOperation.*, Tbl_TamirNetworkType.TamirNetworkType " &
                    " FROM Tbl_TamirOperation LEFT OUTER JOIN Tbl_TamirNetworkType ON Tbl_TamirOperation.TamirNetworkTypeId = Tbl_TamirNetworkType.TamirNetworkTypeId AND Tbl_TamirNetworkType.IsActive = 1 "
                dg.DataSource = DatasetTamir1.Tbl_TamirOperation
                TableName = "Tbl_TamirOperation"
                dg.CaptionText = "فهرست عمليات‌هاي بابرنامه"
                lDs = DatasetTamir1
            Case "Tbl_ManoeuvreType"
                SQL = "SELECT * FROM Tbl_ManoeuvreType"
                dg.DataSource = DatasetTamir1.Tbl_ManoeuvreType
                TableName = "Tbl_ManoeuvreType"
                dg.CaptionText = "انواع مانور"
                lDs = DatasetTamir1
            Case "Tbl_Peymankar"
                SQL = " SELECT Tbl_Peymankar.*, Tbl_Area.Area FROM Tbl_Peymankar LEFT OUTER JOIN Tbl_Area ON Tbl_Peymankar.AreaId = Tbl_Area.AreaId"
                If Not IsCenter Then
                    SQL &= " WHERE (Tbl_Peymankar.AreaId = " & WorkingAreaId & " OR Tbl_Peymankar.AreaId IS NULL)"
                Else
                    If LAW <> "" Then
                        SQL &= LAW.Replace("AreaId", "Tbl_Peymankar.AreaId")
                    ElseIf Not IsSetadMode Then
                        SQL &= " WHERE Tbl_Peymankar.AreaId IN (" & WorkingCenterAreaIDs & ")"
                    End If
                End If
                dg.DataSource = DatasetTamir1.Tbl_Peymankar
                TableName = "Tbl_Peymankar"
                dg.CaptionText = "فهرست پيمانکاران"
                lDs = DatasetTamir1
            Case "Tbl_IVRCode"
                SQL = " SELECT * FROM Tbl_IVRCode ORDER BY IVRCode"
                TableName = "Tbl_IVRCode"
                dg.DataSource = DatasetBT1.Tbl_IVRCode
                dg.CaptionText = "فهرست کدهای IVR ثبت شده در مرکز تماس"
                lDs = DatasetBT1
            Case Else
                SQL = "Select * from " & BT_Name
                dg.CaptionText = ""
                lDs = DatasetCcReq1
        End Select

        If mMessageText <> "" Then
            dg.CaptionText = mMessageText
        End If

        If FilterQuery <> "" Then
            Dim lSQL As String = SQL
            Dim lOrderByClause As String = ""
            Dim lWhereClause As String = ""
            Dim lWhereIndex As Integer = -1
            Dim lOrderByIndex As Integer = SQL.ToLower.IndexOf("order by")

            If lOrderByIndex >= 0 Then
                lOrderByClause = SQL.Substring(lOrderByIndex)
                lSQL = SQL.Remove(lOrderByIndex, lOrderByClause.Length).Trim
            End If

            lWhereIndex = lSQL.ToLower.IndexOf("where")
            If lWhereIndex >= 0 Then
                lWhereClause = lSQL.Substring(lWhereIndex)
                lSQL = lSQL.Remove(lWhereIndex, lWhereClause.Length).Trim
            End If

            If lWhereClause <> "" Then
                lWhereClause = "AND (" & lWhereClause.Remove(0, 5).Trim & ")"
            End If

            FilterQuery = Regex.Replace(FilterQuery, "^[\s]*where", "", RegexOptions.IgnoreCase)
            'If FilterQuery.ToLower.IndexOf("where") > -1 Then
            '    FilterQuery = FilterQuery.Remove(FilterQuery.ToLower.IndexOf("where"), 5).Trim
            'End If
            If TableName = "BTbl_SubCheckList" Then
                FilterQuery = FilterQuery.Replace("BazdidTypeId", "t2.BazdidTypeId").Replace("BazdidCheckListGroupId", "t2.BazdidCheckListGroupId").Replace("BazdidCheckListSubGroupId", "t2.BazdidCheckListSubGroupId").Replace("LPPostDetailId", "t2.LPPostDetailId")
            End If
            SQL = lSQL + " WHERE (" & FilterQuery & ") " & lWhereClause & " " & lOrderByClause
        End If

        DatasetCcReq1.TblEkipProfile.Clear()

        Try
            lDs.Tables(BT_Name).Clear()
        Catch ex As Exception
        End Try
        BindingTable(SQL, mCnn, lDs, TableName, , , , , , , True)
        dg.DataSource = Nothing
        dg.DataSource = lDs
        dg.DataMember = TableName
        If lRowIndex >= 0 Then
            If lRowIndex < lDs.Tables(TableName).Rows.Count Then
                dg.CurrentRowIndex = lRowIndex
            End If
        End If

        Try
            dg.CaptionText &= " ( تعداد فهرست شده: " & lDs.Tables(TableName).Rows.Count & " )"
        Catch ex As Exception
        End Try

        If SelectedID >= 0 Then
            Dim i As Integer
            For i = 0 To lDs.Tables(TableName).Rows.Count - 1
                If dg.Item(i, 0) = SelectedID Then
                    BindingContext(lDs, TableName).Position = i
                    Exit For
                End If
            Next
            SelectedID = -1
        End If

        Try
            If BT_Name = "View_BazdidCheckList" Or BT_Name = "View_BazdidCheckListSubGroup" Then
                dg.DataSource = DatasetBazdidView1.View_BazdidCheckList.DefaultView
            ElseIf BT_Name = "BTbl_ServicePart" Then
                dg.DataSource = DatasetBazdid1.BTbl_ServicePart.DefaultView
            ElseIf BT_Name = "TblEkipProfile" Then
                dg.DataSource = DatasetCcReq1.TblEkipProfile.DefaultView
            ElseIf BT_Name = "Tbl_Master" Then
                dg.DataSource = DatasetCcReq1.Tbl_Master.DefaultView

            End If
        Catch ex As Exception
        End Try

        dg_CurrentCellChanged(Nothing, Nothing)
    End Sub
    Private Sub SetButtonsEnableState()
        If BT_Name = "View_Area" Or BT_Name = "View_DisconnectGroupMP" Or
           BT_Name = "View_DisconnectGroupLP" Or BT_Name = "View_DisconnectGroupLight" Or
           BT_Name = "BTbl_BazdidCheckListGroup" Then
            ButtonAdd.Enabled = False
            'ButtonEditField.Enabled = False
            ButtonDelete.Enabled = False

        End If
        If BT_Name = "Tbl_GroupPart" Then
            ButtonAdd.Enabled = False
            ButtonEditField.Enabled = False
            ButtonDelete.Enabled = False
        End If
        If IsSetadMode Then

        ElseIf IsMiscMode Then

        ElseIf IsCenter And Not IsSpecialCenter Then
            Select Case BT_Name
                Case _
                    "Tbl_LPPost", "Tbl_LPPostCommonFeeder", "Tbl_LPPostCommonLightFeeder", "Tbl_LPFeeder", "Tbl_LPFeederCommonFeeder", "Tbl_LightFeeder", "Tbl_LightFeederCommonFeeder", "Tbl_Master", "BTbl_BazdidMaster",
                    "ViewLPPostLoad", "ViewLPFeederLoad", "ViewLPFeederPointsInfo", "View_MPPostTrans", "ViewLPPostEarth", "ViewLPPostPointEarth"
                    ButtonAdd.Enabled = False
                    ButtonDelete.Enabled = False
            End Select
        Else
            Select Case BT_Name
                Case "View_LPCommonFeeder"
                    ButtonAdd.Enabled = False
                    ButtonDelete.Enabled = False
                Case "View_MPCommonFeeder"
                    ButtonAdd.Enabled = False
                    ButtonDelete.Enabled = False
                    'Case "ViewCriticalFeeder"                
                    'Case "View_City"
                Case "View_MPPost"
                    ButtonAdd.Enabled = False
                    ButtonDelete.Enabled = False
                Case "Tbl_LPPart"
                    ButtonAdd.Enabled = False
                    ButtonDelete.Enabled = False
                Case "Tbl_MPPart"
                    ButtonAdd.Enabled = False
                    ButtonDelete.Enabled = False
                Case "Tbl_LightPart"
                    ButtonAdd.Enabled = False
                    ButtonDelete.Enabled = False
                Case "Tbl_MPFeeder"
                    ButtonAdd.Enabled = False
                    ButtonDelete.Enabled = False
                    'Case "Tbl_FeederPart"
                    '    ButtonAdd.Enabled = False
                    '    ButtonDelete.Enabled = False
                Case "View_MPPostTrans"
                    ButtonAdd.Enabled = False
                    ButtonDelete.Enabled = False
            End Select
        End If
        'If IsMiscMode Then
        '    Dim SQL As String = ""
        '    Select Case BT_Name
        '        Case "View_MPPostTrans"
        '            SQL = " "
        '            If OwnerID >= 0 Then
        '                SQL = "SELECT * FROM Tbl_MPPost where MPPostID=" & OwnerID & " And AreaId=" & WorkingAreaId
        '            End If
        '    End Select
        '    If SQL <> "" Then
        '        Dim ds As New DataSet
        '        BindingTable(SQL, mCnn, ds, "aa")
        '        If IsNothing(ds.Tables("aa")) Then
        '            ButtonAdd.Enabled = False
        '            ButtonDelete.Enabled = False
        '            ButtonEditField.Enabled = False
        '        ElseIf ds.Tables("aa").Rows.Count = 0 Then
        '            ButtonAdd.Enabled = False
        '            ButtonDelete.Enabled = False
        '            ButtonEditField.Enabled = False
        '        End If
        '    End If
        'End If
    End Sub
    Private Sub AssignToBasket(ByVal aDlg As Form, ByVal aId As Long)
        If IsBazdidApp And aId = -1 Then
            Dim lErrorMsg1 As String = ""
            Dim lErrorMsg2 As String = ""
            Dim lIsAccess As Boolean
            lIsAccess = CheckUserTrustee("frmBazdidBasketbtnSave", 9) Or IsAdmin()
            If Not lIsAccess Then
                lErrorMsg1 = "کاربر گرامي، شما دسترسي لازم براي تعريف مسير را نداريد" & vbCrLf & "لطفا با کاربر ادمين تماس بگيريد"
            End If
            lIsAccess = CheckUserTrustee("CanAssignNewPostFeederToBasket", 9) Or IsAdmin()
            If Not lIsAccess Then
                lErrorMsg2 = "کاربر گرامي، شما دسترسي لازم براي اختصاص مسير به فيدر يا پست ايجاد شده را نداريد" & vbCrLf & "لطفا با کاربر ادمين تماس بگيريد"
            End If

            If BT_Name = "Tbl_MPFeeder" Then
                If MsgBoxF("آيا مي‌خواهيد براي فيدر فشار متوسط ايجاد شده مسير تعيين نماييد؟", "تعيين مسير", MessageBoxButtons.YesNo, MsgBoxIcon.MsgIcon_Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                    If lErrorMsg1 <> "" Then
                        ShowError(lErrorMsg1)
                    ElseIf lErrorMsg2 <> "" Then
                        ShowError(lErrorMsg2)
                    Else
                        aId = CType(aDlg, frmBS_MPFeeder).MPFeederId
                        RaiseEvent onShowBasket(aId, 1)
                    End If
                End If
            ElseIf BT_Name = "Tbl_LPPost" Then
                If MsgBoxF("آيا مي‌خواهيد براي پست توزيع ايجاد شده مسير تعيين نماييد؟", "تعيين مسير", MessageBoxButtons.YesNo, MsgBoxIcon.MsgIcon_Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                    If lErrorMsg1 <> "" Then
                        ShowError(lErrorMsg1)
                    ElseIf lErrorMsg2 <> "" Then
                        ShowError(lErrorMsg2)
                    Else
                        aId = CType(aDlg, frmBS_LPPost).LPPostId
                        RaiseEvent onShowBasket(aId, 2)
                    End If
                End If
            ElseIf BT_Name = "Tbl_LPFeeder" Then
                If MsgBoxF("آيا مي‌خواهيد براي فيدر فشار ضعيف ايجاد شده مسير تعيين نماييد؟", "تعيين مسير", MessageBoxButtons.YesNo, MsgBoxIcon.MsgIcon_Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                    If lErrorMsg1 <> "" Then
                        ShowError(lErrorMsg1)
                    ElseIf lErrorMsg2 <> "" Then
                        ShowError(lErrorMsg2)
                    Else
                        aId = CType(aDlg, frmBS_LPFeeder).LPFeederId
                        RaiseEvent onShowBasket(aId, 3)
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub ShowPartsFilter()
        Try
            If BT_Name = "View_BazdidCheckList" Or BT_Name = "View_BazdidCheckListSubGroup" Then

                If IsNothing(DatasetBazdidView1.View_BazdidCheckList) Then Exit Sub
                Dim lFilterQuery As String = ""

                If txtSearch.Text <> "" Then
                    lFilterQuery &= MergeFarsiAndArabi("CheckListName", txtSearch.Text.Replace("*", "[*]").Replace("%", "[%]"), , "")
                End If
                DatasetBazdidView1.View_BazdidCheckList.DefaultView.RowFilter = lFilterQuery
            End If
            If BT_Name = "BTbl_ServicePart" Then

                If IsNothing(DatasetBazdid1.BTbl_ServicePart) Then Exit Sub
                Dim lFilterQuery As String = ""

                If txtSearch.Text <> "" Then
                    lFilterQuery &= MergeFarsiAndArabi("ServicePartName", txtSearch.Text.Replace("*", "[*]").Replace("%", "[%]"), , "")
                End If
                DatasetBazdid1.BTbl_ServicePart.DefaultView.RowFilter = lFilterQuery
            End If
            If BT_Name = "TblEkipProfile" Then
                If IsNothing(DatasetCcReq1.TblEkipProfile) Then Exit Sub
                Dim lFilterQuery As String = ""

                If txtSearch.Text <> "" Then
                    lFilterQuery &= MergeFarsiAndArabi("EkipProfileName", txtSearch.Text.Replace("*", "[*]").Replace("%", "[%]"), , "")
                End If
                DatasetCcReq1.TblEkipProfile.DefaultView.RowFilter = lFilterQuery

            End If
            If BT_Name = "Tbl_Master" Then
                If IsNothing(DatasetCcReq1.Tbl_Master) Then Exit Sub
                Dim lFilterQuery As String = ""

                If txtSearch.Text <> "" Then
                    lFilterQuery &= MergeFarsiAndArabi("Name", txtSearch.Text.Replace("*", "[*]").Replace("%", "[%]"), , "")
                End If
                DatasetCcReq1.Tbl_Master.DefaultView.RowFilter = lFilterQuery

            End If
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub
    Private Sub SelectZoneMPFeeders()
        Dim lRowIndex = dg.CurrentRowIndex
        If lRowIndex < 0 Then
            Exit Sub
        End If

        Dim lRow As DatasetBT.Tbl_MPFeederZoneRow
        Dim lZoneId As Integer = dg.Item(lRowIndex, 0)
        Dim lAreaId As Integer = dg.Item(lRowIndex, 1)
        Dim lZoneName As String = dg.Item(lRowIndex, 2)
        Dim lIDs As String = ""

        Dim lSQL As String = "SELECT * FROM Tbl_MPFeederZone WHERE ZoneId = " & lZoneId
        BindingTable(lSQL, mCnn, DatasetBT1, "Tbl_MPFeederZone", aIsClearTable:=True)
        For Each lRow In DatasetBT1.Tbl_MPFeederZone
            lIDs &= IIf(lIDs.Length > 0, ",", "") & lRow.MPFeederId.ToString()
        Next

        lSQL =
            "SELECT Tbl_MPFeeder.MPFeederId,'فيدر ' + Tbl_MPFeeder.MPFeederName + ' از پست فوق توزيع ' + Tbl_MPPost.MPPostName AS MPFeederName FROM Tbl_MPFeeder inner join Tbl_MPPost ON Tbl_MPFeeder.MPPostId = Tbl_MPPost.MPPostId WHERE Tbl_MPFeeder.AreaId =  " & lAreaId &
            "UNION " &
            "SELECT t1.MPFeederId,'فيدر ' + t2.MPFeederName + ' از پست فوق توزيع ' + Tbl_MPPost.MPPostName AS MPFeederName FROM Tbl_MPCommonFeeder t1 INNER JOIN Tbl_MPFeeder t2 ON t1.MPFeederId = t2.MPFeederId  inner join Tbl_MPPost ON t2.MPPostId = Tbl_MPPost.MPPostId WHERE t1.AreaId = " & lAreaId
        BindingTable(lSQL, mCnn, DatasetBT1, "View_MPFeeders", aIsClearTable:=True)

        Dim dlg As New frmSelectGridItems(
            aSelectionTable:=DatasetBT1.Tables("View_MPFeeders"),
            aTableName:="View_MPFeeders",
            aID_Field:="MPFeederId",
            aItemName_Field:="MPFeederName",
            aItemCode_Field:="",
            aCondition:="",
            aIsView:=True,
            aSelectedItems:=lIDs,
            aIsSaveMode:=True)

        dlg.TitleText = String.Format("فهرست فیدرهای موجود در منطقه {0}", lZoneName)

        If dlg.ShowDialog() = DialogResult.OK Then
            For Each lRow In DatasetBT1.Tbl_MPFeederZone.Rows
                lRow.Delete()
            Next

            Dim lViewSelection As CommonDataSet.ViewSelectionDataTable
            lViewSelection = dlg.ViewSelectionTable
            Dim i As Integer
            For i = 0 To lViewSelection.Rows.Count - 1
                With lViewSelection.Rows(i)
                    If .Item("ItemIsSelected") Then
                        lRow = DatasetBT1.Tbl_MPFeederZone.NewRow()
                        lRow("MPFeederZoneId") = GetAutoInc()
                        lRow("MPFeederId") = .Item("ItemID")
                        lRow("ZoneId") = lZoneId
                        DatasetBT1.Tbl_MPFeederZone.AddTbl_MPFeederZoneRow(lRow)
                    End If
                End With
            Next
            Dim lDlg As New frmUpdateDataSetBT
            lDlg.UpdateDataSet("Tbl_MPFeederZone", DatasetBT1)
        End If
        dlg.Dispose()
    End Sub

    Private Sub ShowOnGISMap()
        If dg.CurrentRowIndex = -1 Then Exit Sub
        Dim lTableList As String
        lTableList = "View_MPPost,View_MPFeeder,Tbl_LPPost,Tbl_LPFeeder,Tbl_MPFeederKey,View_MPPostTrans"
        If lTableList.IndexOf(BT_Name) = -1 Then Exit Sub

        Dim lDs As New DataSet
        Dim lRowIndex As Integer = dg.CurrentRowIndex
        Dim lID As String = dg.Item(lRowIndex, 0).ToString()
        Dim lSQL As String
        Dim lGISTableCode As String = "-"
        Dim lGISCode As String = "-"

        Select Case BT_Name
            Case "View_MPPost"
                lSQL = "SELECT ISNULL(MPPostCode,'-') AS GISCode, 'HV_Substat' AS GISTableCode FROM Tbl_MPPost WHERE MPPostId = " & lID
            Case "View_MPPostTrans"
                lSQL = "SELECT ISNULL(MPPostTransCode, '-') AS GISCode, 'HV_Transformer' AS GISTableCode FROM Tbl_MPPostTrans WHERE MPPostTransId = " & lID
            Case "Tbl_MPFeeder"
                lSQL = "SELECT ISNULL(MPFeederCode,'-') AS GISCode, 'MV_Feeder' AS GISTableCode FROM Tbl_MPFeeder WHERE  MPFeederId= " & lID
            Case "Tbl_LPPost"
                lSQL = "SELECT ISNULL(LPPostCode,'-') AS GISCode, GISTableCode = CASE IsHavayi WHEN 1 THEN 'Pl_MDSub' ELSE 'Pd_MDSub' END FROM Tbl_LPPost WHERE LPPostId = " & lID
            Case "Tbl_LPFeeder"
                lSQL = "SELECT ISNULL(LPFeederCode,'-') AS GISCode, 'LV_Feeder' AS GISTableCode FROM Tbl_LPFeeder WHERE LPFeederId = " & lID
            Case "Tbl_MPFeederKey"
                lSQL =
                    "SELECT ISNULL(GISCode,'-') AS GISCode, ISNULL(MPCloserTypeCode,'-') AS GISTableCode " &
                    "FROM Tbl_MPFeederKey LEFT JOIN Tbl_MPCloserType On Tbl_MPFeederKey.MPCloserTypeId = Tbl_MPCloserType.MPCloserTypeId " &
                    "WHERE MPFeederKeyId = " & lID
        End Select

        If lSQL <> "" Then
            BindingTable(lSQL, mCnn, lDs, "ViewGISInfo")
            If lDs.Tables.Contains("ViewGISInfo") AndAlso lDs.Tables("ViewGISInfo").Rows.Count > 0 Then
                Dim lRowGIS As DataRow = lDs.Tables("ViewGISInfo").Rows(0)
                lGISCode = lRowGIS("GISCode")
                lGISTableCode = lRowGIS("GISTableCode")
            End If
        End If

        Dim lWebGIS As New CWebGIS()
        lWebGIS.AddParam("GIS_Param_GISTableCode", lGISTableCode)
        lWebGIS.AddParam("GIS_Param_GISCode", lGISCode)

        Dim lURL As String = lWebGIS.GetFullURL("GIS_Method_ShowTajhizOnGIS")
        lWebGIS.ShowWebGIS(lURL)
    End Sub
#End Region

    Private Sub ButtonAdd_MouseEnter(sender As Object, e As EventArgs) Handles ButtonAdd.MouseEnter
        If Not mIsDisableAddButton Then Exit Sub
        lblNotIsAllowSave.Left = ButtonAdd.Left - (lblNotIsAllowSave.Width - ButtonAdd.Width)
        lblNotIsAllowSave.Top = ButtonAdd.Top - (lblNotIsAllowSave.Height - ButtonAdd.Height)
        lblNotIsAllowSave.Text = lblNotIsAllowSave.Text.Replace("###", mTableNameText)
        lblNotIsAllowSave.Visible = True
        lblNotIsAllowSave.BringToFront()
    End Sub

    Private Sub lblNotIsAllowSave_MouseLeave(sender As Object, e As EventArgs) Handles lblNotIsAllowSave.MouseLeave
        lblNotIsAllowSave.Visible = False
        lblNotIsAllowSave.SendToBack()
    End Sub

    Private Sub btnRecloserFunction_Click(sender As Object, e As EventArgs) Handles btnRecloserFunction.Click
        Dim row As Integer = dg.CurrentRowIndex
        If row < 0 Then
            Exit Sub
        End If
        Dim ID As Long = dg.Item(row, 0)
        Dim dlg As New frmBaseTablesNotStd("TblRecloserFunction", , , ID)
        dlg.ShowDialog()
        dlg.Dispose()
        LoadData()
    End Sub
End Class
