<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmReportRecloser
    Inherits FormBase

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmReportRecloser))
        Me.txtToDate = New Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor()
        Me.txtFromDate = New Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cmbArea = New Bargh_Common.ChkCombo()
        Me.btnReturn = New System.Windows.Forms.Button()
        Me.btnMakeReport = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btnFromDate = New Bargh_Common.DateButton()
        Me.btnToDate = New Bargh_Common.DateButton()
        Me.txtFaultNumber = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.cmbMPPost = New Bargh_Common.ChkCombo()
        Me.cmbMPFeeder = New Bargh_Common.ChkCombo()
        Me.cmbRecloserModel = New Bargh_Common.ChkCombo()
        Me.SuspendLayout()
        '
        'HelpMaker
        '
        Me.HelpMaker.HelpNamespace = "Help\ReportsHelp.chm"
        '
        'txtToDate
        '
        Me.txtToDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtToDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtToDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.txtToDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtToDate.IntegerDate = 0
        Me.txtToDate.IsShadow = False
        Me.txtToDate.IsShowCurrentDate = False
        Me.txtToDate.Location = New System.Drawing.Point(40, 156)
        Me.txtToDate.MaxLength = 10
        Me.txtToDate.MiladiDT = CType(resources.GetObject("txtToDate.MiladiDT"), Object)
        Me.txtToDate.Name = "txtToDate"
        Me.txtToDate.ReadOnly = True
        Me.txtToDate.ReadOnlyMaskedEdit = False
        Me.txtToDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtToDate.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtToDate.ShamsiDT = "____/__/__"
        Me.txtToDate.Size = New System.Drawing.Size(85, 22)
        Me.txtToDate.TabIndex = 8
        Me.txtToDate.Tag = "999"
        Me.txtToDate.Text = "____/__/__"
        Me.txtToDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtToDate.TimeMaskedEditorOBJ = Nothing
        '
        'txtFromDate
        '
        Me.txtFromDate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFromDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtFromDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtFromDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.txtFromDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFromDate.IntegerDate = 0
        Me.txtFromDate.IsShadow = False
        Me.txtFromDate.IsShowCurrentDate = False
        Me.txtFromDate.Location = New System.Drawing.Point(219, 156)
        Me.txtFromDate.MaxLength = 10
        Me.txtFromDate.MiladiDT = CType(resources.GetObject("txtFromDate.MiladiDT"), Object)
        Me.txtFromDate.Name = "txtFromDate"
        Me.txtFromDate.ReadOnly = True
        Me.txtFromDate.ReadOnlyMaskedEdit = False
        Me.txtFromDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFromDate.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtFromDate.ShamsiDT = "____/__/__"
        Me.txtFromDate.Size = New System.Drawing.Size(85, 22)
        Me.txtFromDate.TabIndex = 6
        Me.txtFromDate.Tag = "999"
        Me.txtFromDate.Text = "____/__/__"
        Me.txtFromDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtFromDate.TimeMaskedEditorOBJ = Nothing
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label3.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(135, 158)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(38, 18)
        Me.Label3.TabIndex = 12
        Me.Label3.Text = "تا تاريخ"
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label2.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(320, 158)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(38, 18)
        Me.Label2.TabIndex = 13
        Me.Label2.Text = "از تاريخ"
        '
        'Label5
        '
        Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoSize = True
        Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label5.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label5.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label5.Location = New System.Drawing.Point(272, 24)
        Me.Label5.Name = "Label5"
        Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label5.Size = New System.Drawing.Size(29, 18)
        Me.Label5.TabIndex = 14
        Me.Label5.Text = "ناحيه"
        '
        'cmbArea
        '
        Me.cmbArea.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbArea.BackColor = System.Drawing.Color.White
        Me.cmbArea.CheckComboDropDownWidth = 0
        Me.cmbArea.CheckGroup = CType(resources.GetObject("cmbArea.CheckGroup"), System.Collections.ArrayList)
        Me.cmbArea.DropDownDirection = UtilityLibrary.Combos.DropDownDirection.Down
        Me.cmbArea.DropHeight = 500
        Me.cmbArea.IsGroup = False
        Me.cmbArea.IsMultiSelect = True
        Me.cmbArea.Location = New System.Drawing.Point(8, 23)
        Me.cmbArea.Name = "cmbArea"
        Me.cmbArea.ReadOnly = True
        Me.cmbArea.ReadOnlyList = ""
        Me.cmbArea.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.cmbArea.Size = New System.Drawing.Size(248, 21)
        Me.cmbArea.TabIndex = 1
        Me.cmbArea.TreeImageList = Nothing
        '
        'btnReturn
        '
        Me.btnReturn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnReturn.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnReturn.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnReturn.Image = CType(resources.GetObject("btnReturn.Image"), System.Drawing.Image)
        Me.btnReturn.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnReturn.Location = New System.Drawing.Point(4, 232)
        Me.btnReturn.Name = "btnReturn"
        Me.btnReturn.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.btnReturn.Size = New System.Drawing.Size(96, 23)
        Me.btnReturn.TabIndex = 12
        Me.btnReturn.Text = "&بازگشت"
        Me.btnReturn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnMakeReport
        '
        Me.btnMakeReport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnMakeReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMakeReport.Image = CType(resources.GetObject("btnMakeReport.Image"), System.Drawing.Image)
        Me.btnMakeReport.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnMakeReport.Location = New System.Drawing.Point(263, 232)
        Me.btnMakeReport.Name = "btnMakeReport"
        Me.btnMakeReport.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.btnMakeReport.Size = New System.Drawing.Size(96, 23)
        Me.btnMakeReport.TabIndex = 11
        Me.btnMakeReport.Text = "تهيه &گزارش"
        Me.btnMakeReport.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label1.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label1.Location = New System.Drawing.Point(269, 93)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(94, 18)
        Me.Label1.TabIndex = 28
        Me.Label1.Text = "نام فيدر فشار متوسط"
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label4.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label4.Location = New System.Drawing.Point(272, 57)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(90, 18)
        Me.Label4.TabIndex = 30
        Me.Label4.Text = "نام پست فوق توزيع"
        '
        'btnFromDate
        '
        Me.btnFromDate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFromDate.Location = New System.Drawing.Point(187, 156)
        Me.btnFromDate.Name = "btnFromDate"
        Me.btnFromDate.Size = New System.Drawing.Size(24, 22)
        Me.btnFromDate.TabIndex = 7
        Me.btnFromDate.Text = "..."
        '
        'btnToDate
        '
        Me.btnToDate.Location = New System.Drawing.Point(7, 156)
        Me.btnToDate.Name = "btnToDate"
        Me.btnToDate.Size = New System.Drawing.Size(24, 22)
        Me.btnToDate.TabIndex = 9
        Me.btnToDate.Text = "..."
        '
        'txtFaultNumber
        '
        Me.txtFaultNumber.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFaultNumber.Location = New System.Drawing.Point(138, 195)
        Me.txtFaultNumber.Name = "txtFaultNumber"
        Me.txtFaultNumber.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtFaultNumber.Size = New System.Drawing.Size(84, 21)
        Me.txtFaultNumber.TabIndex = 10
        Me.txtFaultNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label12
        '
        Me.Label12.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label12.AutoSize = True
        Me.Label12.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label12.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label12.Location = New System.Drawing.Point(240, 195)
        Me.Label12.Name = "Label12"
        Me.Label12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label12.Size = New System.Drawing.Size(126, 18)
        Me.Label12.TabIndex = 66
        Me.Label12.Text = " Fault بيشترين تعداد قطعي "
        '
        'Label7
        '
        Me.Label7.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label7.AutoSize = True
        Me.Label7.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label7.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label7.Location = New System.Drawing.Point(271, 125)
        Me.Label7.Name = "Label7"
        Me.Label7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label7.Size = New System.Drawing.Size(57, 18)
        Me.Label7.TabIndex = 70
        Me.Label7.Text = "مدل ريکلوزر"
        '
        'cmbMPPost
        '
        Me.cmbMPPost.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbMPPost.BackColor = System.Drawing.Color.White
        Me.cmbMPPost.CheckComboDropDownWidth = 0
        Me.cmbMPPost.CheckGroup = CType(resources.GetObject("cmbMPPost.CheckGroup"), System.Collections.ArrayList)
        Me.cmbMPPost.DropDownDirection = UtilityLibrary.Combos.DropDownDirection.Down
        Me.cmbMPPost.DropHeight = 500
        Me.cmbMPPost.IsGroup = False
        Me.cmbMPPost.IsMultiSelect = True
        Me.cmbMPPost.Location = New System.Drawing.Point(8, 56)
        Me.cmbMPPost.Name = "cmbMPPost"
        Me.cmbMPPost.ReadOnly = True
        Me.cmbMPPost.ReadOnlyList = ""
        Me.cmbMPPost.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.cmbMPPost.Size = New System.Drawing.Size(248, 21)
        Me.cmbMPPost.TabIndex = 71
        Me.cmbMPPost.TreeImageList = Nothing
        '
        'cmbMPFeeder
        '
        Me.cmbMPFeeder.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbMPFeeder.BackColor = System.Drawing.Color.White
        Me.cmbMPFeeder.CheckComboDropDownWidth = 0
        Me.cmbMPFeeder.CheckGroup = CType(resources.GetObject("cmbMPFeeder.CheckGroup"), System.Collections.ArrayList)
        Me.cmbMPFeeder.DropDownDirection = UtilityLibrary.Combos.DropDownDirection.Down
        Me.cmbMPFeeder.DropHeight = 500
        Me.cmbMPFeeder.IsGroup = False
        Me.cmbMPFeeder.IsMultiSelect = True
        Me.cmbMPFeeder.Location = New System.Drawing.Point(7, 90)
        Me.cmbMPFeeder.Name = "cmbMPFeeder"
        Me.cmbMPFeeder.ReadOnly = True
        Me.cmbMPFeeder.ReadOnlyList = ""
        Me.cmbMPFeeder.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.cmbMPFeeder.Size = New System.Drawing.Size(248, 21)
        Me.cmbMPFeeder.TabIndex = 72
        Me.cmbMPFeeder.TreeImageList = Nothing
        '
        'cmbRecloserModel
        '
        Me.cmbRecloserModel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbRecloserModel.BackColor = System.Drawing.Color.White
        Me.cmbRecloserModel.CheckComboDropDownWidth = 0
        Me.cmbRecloserModel.CheckGroup = CType(resources.GetObject("cmbRecloserModel.CheckGroup"), System.Collections.ArrayList)
        Me.cmbRecloserModel.DropDownDirection = UtilityLibrary.Combos.DropDownDirection.Down
        Me.cmbRecloserModel.DropHeight = 500
        Me.cmbRecloserModel.IsGroup = False
        Me.cmbRecloserModel.IsMultiSelect = True
        Me.cmbRecloserModel.Location = New System.Drawing.Point(5, 122)
        Me.cmbRecloserModel.Name = "cmbRecloserModel"
        Me.cmbRecloserModel.ReadOnly = True
        Me.cmbRecloserModel.ReadOnlyList = ""
        Me.cmbRecloserModel.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.cmbRecloserModel.Size = New System.Drawing.Size(248, 21)
        Me.cmbRecloserModel.TabIndex = 74
        Me.cmbRecloserModel.TreeImageList = Nothing
        '
        'frmReportRecloser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(365, 263)
        Me.Controls.Add(Me.cmbRecloserModel)
        Me.Controls.Add(Me.cmbMPFeeder)
        Me.Controls.Add(Me.cmbMPPost)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtFaultNumber)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.btnToDate)
        Me.Controls.Add(Me.btnFromDate)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnReturn)
        Me.Controls.Add(Me.btnMakeReport)
        Me.Controls.Add(Me.txtToDate)
        Me.Controls.Add(Me.txtFromDate)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.cmbArea)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.HelpMaker.SetHelpNavigator(Me, System.Windows.Forms.HelpNavigator.Topic)
        Me.MaximizeBox = False
        Me.Name = "frmReportRecloser"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HelpMaker.SetShowHelp(Me, True)
        Me.Text = "گزارش عملکرد کلوزر"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend txtToDate As PersianMaskedEditor
    Friend txtFromDate As PersianMaskedEditor
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents cmbArea As ChkCombo
    Friend WithEvents btnReturn As Button
    Friend WithEvents btnMakeReport As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents btnFromDate As DateButton
    Friend WithEvents btnToDate As DateButton
    Friend WithEvents txtFaultNumber As TextBox
    Friend WithEvents Label12 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents cmbMPPost As ChkCombo
    Friend WithEvents cmbMPFeeder As ChkCombo
    Friend WithEvents cmbRecloserModel As ChkCombo
End Class
