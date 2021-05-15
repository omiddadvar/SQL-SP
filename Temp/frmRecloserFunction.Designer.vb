<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmRecloserFunction
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRecloserFunction))
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cboMPPost = New Bargh_Common.ComboBoxPersian()
        Me.cboArea = New Bargh_Common.ComboBoxPersian()
        Me.cboMPFeeder = New Bargh_Common.ComboBoxPersian()
        Me.txtBoxAddress = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtReadDate = New Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor()
        Me.txtRestartNumber = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtTripNumber = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtFaultNumber = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.BttnReturn = New System.Windows.Forms.Button()
        Me.ButtonSave = New System.Windows.Forms.Button()
        Me.cboKeyType = New Bargh_Common.ComboBoxPersian()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cboRecloser = New Bargh_Common.ComboBoxPersian()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cboRecloserType = New Bargh_Common.ComboBoxPersian()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cboRecloserModel = New Bargh_Common.ComboBoxPersian()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.btnReadDate = New Bargh_Common.DateButton()
        Me.txtReadTime = New Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label2.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label2.Location = New System.Drawing.Point(309, 58)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(90, 18)
        Me.Label2.TabIndex = 25
        Me.Label2.Text = "نام پست فوق توزيع"
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label1.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label1.Location = New System.Drawing.Point(309, 30)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(55, 18)
        Me.Label1.TabIndex = 24
        Me.Label1.Text = "ناحيه مرتبط"
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label3.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label3.Location = New System.Drawing.Point(309, 87)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(94, 18)
        Me.Label3.TabIndex = 26
        Me.Label3.Text = "نام فيدر فشار متوسط"
        '
        'cboMPPost
        '
        Me.cboMPPost.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboMPPost.BackColor = System.Drawing.Color.White
        Me.cboMPPost.DisplayMember = "MPPostName"
        Me.cboMPPost.IsReadOnly = False
        Me.cboMPPost.Location = New System.Drawing.Point(12, 58)
        Me.cboMPPost.Name = "cboMPPost"
        Me.cboMPPost.Size = New System.Drawing.Size(291, 21)
        Me.cboMPPost.TabIndex = 2
        Me.cboMPPost.ValueMember = "MPPostId"
        '
        'cboArea
        '
        Me.cboArea.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboArea.BackColor = System.Drawing.Color.White
        Me.cboArea.DisplayMember = "Area"
        Me.cboArea.IsReadOnly = False
        Me.cboArea.Location = New System.Drawing.Point(12, 30)
        Me.cboArea.Name = "cboArea"
        Me.cboArea.Size = New System.Drawing.Size(291, 21)
        Me.cboArea.TabIndex = 1
        Me.cboArea.ValueMember = "AreaId"
        '
        'cboMPFeeder
        '
        Me.cboMPFeeder.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboMPFeeder.BackColor = System.Drawing.Color.White
        Me.cboMPFeeder.DisplayMember = "MPFeederName"
        Me.cboMPFeeder.IsReadOnly = False
        Me.cboMPFeeder.Location = New System.Drawing.Point(12, 87)
        Me.cboMPFeeder.Name = "cboMPFeeder"
        Me.cboMPFeeder.Size = New System.Drawing.Size(291, 21)
        Me.cboMPFeeder.TabIndex = 3
        Me.cboMPFeeder.ValueMember = "MPFeederId"
        '
        'txtBoxAddress
        '
        Me.txtBoxAddress.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBoxAddress.Location = New System.Drawing.Point(12, 243)
        Me.txtBoxAddress.Multiline = True
        Me.txtBoxAddress.Name = "txtBoxAddress"
        Me.txtBoxAddress.ReadOnly = True
        Me.txtBoxAddress.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtBoxAddress.Size = New System.Drawing.Size(291, 69)
        Me.txtBoxAddress.TabIndex = 8
        '
        'Label8
        '
        Me.Label8.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label8.AutoSize = True
        Me.Label8.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label8.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label8.Location = New System.Drawing.Point(310, 243)
        Me.Label8.Name = "Label8"
        Me.Label8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label8.Size = New System.Drawing.Size(33, 18)
        Me.Label8.TabIndex = 29
        Me.Label8.Text = "آدرس"
        '
        'Label9
        '
        Me.Label9.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label9.AutoSize = True
        Me.Label9.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label9.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label9.Location = New System.Drawing.Point(309, 326)
        Me.Label9.Name = "Label9"
        Me.Label9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label9.Size = New System.Drawing.Size(56, 18)
        Me.Label9.TabIndex = 47
        Me.Label9.Text = "تاريخ قرائت"
        '
        'txtReadDate
        '
        Me.txtReadDate.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtReadDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtReadDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtReadDate.IntegerDate = 0
        Me.txtReadDate.IsShadow = False
        Me.txtReadDate.IsShowCurrentDate = False
        Me.txtReadDate.Location = New System.Drawing.Point(219, 325)
        Me.txtReadDate.MaxLength = 10
        Me.txtReadDate.MiladiDT = CType(resources.GetObject("txtReadDate.MiladiDT"), Object)
        Me.txtReadDate.Name = "txtReadDate"
        Me.txtReadDate.ReadOnly = True
        Me.txtReadDate.ReadOnlyMaskedEdit = False
        Me.txtReadDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtReadDate.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtReadDate.ShamsiDT = "____/__/__"
        Me.txtReadDate.Size = New System.Drawing.Size(84, 20)
        Me.txtReadDate.TabIndex = 9
        Me.txtReadDate.Text = "____/__/__"
        Me.txtReadDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtReadDate.TimeMaskedEditorOBJ = Nothing
        '
        'txtRestartNumber
        '
        Me.txtRestartNumber.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtRestartNumber.Location = New System.Drawing.Point(218, 355)
        Me.txtRestartNumber.Name = "txtRestartNumber"
        Me.txtRestartNumber.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtRestartNumber.Size = New System.Drawing.Size(84, 20)
        Me.txtRestartNumber.TabIndex = 11
        Me.txtRestartNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label10
        '
        Me.Label10.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label10.AutoSize = True
        Me.Label10.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label10.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label10.Location = New System.Drawing.Point(308, 355)
        Me.Label10.Name = "Label10"
        Me.Label10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label10.Size = New System.Drawing.Size(103, 18)
        Me.Label10.TabIndex = 49
        Me.Label10.Text = "Restart تعداد شمارنده "
        '
        'txtTripNumber
        '
        Me.txtTripNumber.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTripNumber.Location = New System.Drawing.Point(218, 384)
        Me.txtTripNumber.Name = "txtTripNumber"
        Me.txtTripNumber.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtTripNumber.Size = New System.Drawing.Size(84, 20)
        Me.txtTripNumber.TabIndex = 12
        Me.txtTripNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label11
        '
        Me.Label11.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label11.AutoSize = True
        Me.Label11.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label11.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label11.Location = New System.Drawing.Point(308, 384)
        Me.Label11.Name = "Label11"
        Me.Label11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label11.Size = New System.Drawing.Size(91, 18)
        Me.Label11.TabIndex = 51
        Me.Label11.Text = "Trip تعداد شمارنده "
        '
        'txtFaultNumber
        '
        Me.txtFaultNumber.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFaultNumber.Location = New System.Drawing.Point(218, 413)
        Me.txtFaultNumber.Name = "txtFaultNumber"
        Me.txtFaultNumber.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtFaultNumber.Size = New System.Drawing.Size(84, 20)
        Me.txtFaultNumber.TabIndex = 13
        Me.txtFaultNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label12
        '
        Me.Label12.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label12.AutoSize = True
        Me.Label12.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label12.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label12.Location = New System.Drawing.Point(307, 413)
        Me.Label12.Name = "Label12"
        Me.Label12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label12.Size = New System.Drawing.Size(95, 18)
        Me.Label12.TabIndex = 53
        Me.Label12.Text = "Fault تعداد شمارنده "
        '
        'BttnReturn
        '
        Me.BttnReturn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BttnReturn.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.BttnReturn.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BttnReturn.Image = CType(resources.GetObject("BttnReturn.Image"), System.Drawing.Image)
        Me.BttnReturn.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.BttnReturn.Location = New System.Drawing.Point(7, 462)
        Me.BttnReturn.Name = "BttnReturn"
        Me.BttnReturn.Size = New System.Drawing.Size(75, 23)
        Me.BttnReturn.TabIndex = 15
        Me.BttnReturn.Text = "&بازگشت"
        Me.BttnReturn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ButtonSave
        '
        Me.ButtonSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonSave.Image = CType(resources.GetObject("ButtonSave.Image"), System.Drawing.Image)
        Me.ButtonSave.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ButtonSave.Location = New System.Drawing.Point(324, 462)
        Me.ButtonSave.Name = "ButtonSave"
        Me.ButtonSave.Size = New System.Drawing.Size(75, 23)
        Me.ButtonSave.TabIndex = 14
        Me.ButtonSave.Text = "&ذخيره"
        Me.ButtonSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cboKeyType
        '
        Me.cboKeyType.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboKeyType.BackColor = System.Drawing.Color.White
        Me.cboKeyType.DisplayMember = "MPCloserType"
        Me.cboKeyType.IsReadOnly = False
        Me.cboKeyType.Location = New System.Drawing.Point(10, 118)
        Me.cboKeyType.Name = "cboKeyType"
        Me.cboKeyType.Size = New System.Drawing.Size(291, 21)
        Me.cboKeyType.TabIndex = 4
        Me.cboKeyType.ValueMember = "MPCloserTypeId"
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label4.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label4.Location = New System.Drawing.Point(307, 118)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(42, 18)
        Me.Label4.TabIndex = 55
        Me.Label4.Text = "نوع کليد"
        '
        'cboRecloser
        '
        Me.cboRecloser.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboRecloser.BackColor = System.Drawing.Color.White
        Me.cboRecloser.DisplayMember = "KeyName"
        Me.cboRecloser.IsReadOnly = False
        Me.cboRecloser.Location = New System.Drawing.Point(10, 149)
        Me.cboRecloser.Name = "cboRecloser"
        Me.cboRecloser.Size = New System.Drawing.Size(291, 21)
        Me.cboRecloser.TabIndex = 5
        Me.cboRecloser.ValueMember = "MPFeederKeyId"
        '
        'Label5
        '
        Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoSize = True
        Me.Label5.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label5.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label5.Location = New System.Drawing.Point(307, 149)
        Me.Label5.Name = "Label5"
        Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label5.Size = New System.Drawing.Size(37, 18)
        Me.Label5.TabIndex = 57
        Me.Label5.Text = "ريکلوزر"
        '
        'cboRecloserType
        '
        Me.cboRecloserType.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboRecloserType.BackColor = System.Drawing.Color.White
        Me.cboRecloserType.DisplayMember = "SpecValue"
        Me.cboRecloserType.IsReadOnly = False
        Me.cboRecloserType.Location = New System.Drawing.Point(10, 181)
        Me.cboRecloserType.Name = "cboRecloserType"
        Me.cboRecloserType.Size = New System.Drawing.Size(291, 21)
        Me.cboRecloserType.TabIndex = 6
        Me.cboRecloserType.ValueMember = "SpecId"
        '
        'Label6
        '
        Me.Label6.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label6.AutoSize = True
        Me.Label6.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label6.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label6.Location = New System.Drawing.Point(307, 181)
        Me.Label6.Name = "Label6"
        Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label6.Size = New System.Drawing.Size(54, 18)
        Me.Label6.TabIndex = 59
        Me.Label6.Text = "نوع ريکلوزر"
        '
        'cboRecloserModel
        '
        Me.cboRecloserModel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboRecloserModel.BackColor = System.Drawing.Color.White
        Me.cboRecloserModel.DisplayMember = "SpecValue"
        Me.cboRecloserModel.IsReadOnly = False
        Me.cboRecloserModel.Location = New System.Drawing.Point(10, 213)
        Me.cboRecloserModel.Name = "cboRecloserModel"
        Me.cboRecloserModel.Size = New System.Drawing.Size(291, 21)
        Me.cboRecloserModel.TabIndex = 7
        Me.cboRecloserModel.ValueMember = "SpecId"
        '
        'Label7
        '
        Me.Label7.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label7.AutoSize = True
        Me.Label7.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label7.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label7.Location = New System.Drawing.Point(307, 213)
        Me.Label7.Name = "Label7"
        Me.Label7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label7.Size = New System.Drawing.Size(57, 18)
        Me.Label7.TabIndex = 61
        Me.Label7.Text = "مدل ريکلوزر"
        '
        'btnReadDate
        '
        Me.btnReadDate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnReadDate.Location = New System.Drawing.Point(189, 326)
        Me.btnReadDate.Name = "btnReadDate"
        Me.btnReadDate.Size = New System.Drawing.Size(24, 22)
        Me.btnReadDate.TabIndex = 62
        Me.btnReadDate.Text = "..."
        '
        'txtReadTime
        '
        Me.txtReadTime.BackColor = System.Drawing.SystemColors.Window
        Me.txtReadTime.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtReadTime.IsShadow = False
        Me.txtReadTime.IsShowCurrentTime = False
        Me.txtReadTime.Location = New System.Drawing.Point(59, 325)
        Me.txtReadTime.MaxLength = 5
        Me.txtReadTime.MiladiDT = Nothing
        Me.txtReadTime.Name = "txtReadTime"
        Me.txtReadTime.ReadOnly = True
        Me.txtReadTime.ReadOnlyMaskedEdit = False
        Me.txtReadTime.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtReadTime.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtReadTime.Size = New System.Drawing.Size(40, 20)
        Me.txtReadTime.TabIndex = 10
        Me.txtReadTime.Text = "__:__"
        Me.txtReadTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label13
        '
        Me.Label13.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label13.AutoSize = True
        Me.Label13.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label13.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label13.Location = New System.Drawing.Point(118, 325)
        Me.Label13.Name = "Label13"
        Me.Label13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label13.Size = New System.Drawing.Size(54, 18)
        Me.Label13.TabIndex = 64
        Me.Label13.Text = "زمان قرائت"
        '
        'frmRecloserFunction
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(407, 497)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.txtReadTime)
        Me.Controls.Add(Me.btnReadDate)
        Me.Controls.Add(Me.cboRecloserModel)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.cboRecloserType)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.cboRecloser)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.cboKeyType)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.BttnReturn)
        Me.Controls.Add(Me.ButtonSave)
        Me.Controls.Add(Me.txtFaultNumber)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.txtTripNumber)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.txtRestartNumber)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.txtReadDate)
        Me.Controls.Add(Me.txtBoxAddress)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.cboMPFeeder)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.cboMPPost)
        Me.Controls.Add(Me.cboArea)
        Me.Name = "frmRecloserFunction"
        Me.Text = "ثبت کارکرد ریکلوزر"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents cboMPPost As ComboBoxPersian
    Friend WithEvents cboArea As ComboBoxPersian
    Friend WithEvents cboMPFeeder As ComboBoxPersian
    Friend WithEvents txtBoxAddress As TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend txtReadDate As PersianMaskedEditor
    Friend WithEvents txtRestartNumber As TextBox
    Friend WithEvents Label10 As Label
    Friend WithEvents txtTripNumber As TextBox
    Friend WithEvents Label11 As Label
    Friend WithEvents txtFaultNumber As TextBox
    Friend WithEvents Label12 As Label
    Friend WithEvents BttnReturn As Button
    Friend WithEvents ButtonSave As Button
    Friend WithEvents cboKeyType As ComboBoxPersian
    Friend WithEvents Label4 As Label
    Friend WithEvents cboRecloser As ComboBoxPersian
    Friend WithEvents Label5 As Label
    Friend WithEvents cboRecloserType As ComboBoxPersian
    Friend WithEvents Label6 As Label
    Friend WithEvents cboRecloserModel As ComboBoxPersian
    Friend WithEvents Label7 As Label
    Friend WithEvents btnReadDate As DateButton
    Friend txtReadTime As TimeMaskedEditor
    Friend WithEvents Label13 As Label
End Class
