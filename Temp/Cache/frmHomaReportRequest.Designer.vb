<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmHomaReportRequest
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmHomaReportRequest))
        Me.btnReturn = New System.Windows.Forms.Button()
        Me.btnMakeReport = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.txtDateFrom = New Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor()
        Me.btnDateTo = New Bargh_Common.DateButton()
        Me.btnDateFrom = New Bargh_Common.DateButton()
        Me.txtDateTo = New Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cmbArea = New Bargh_Common.ChkCombo()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'HelpMaker
        '
        Me.HelpMaker.HelpNamespace = "Help\ReportsHelp.chm"
        '
        'btnReturn
        '
        Me.btnReturn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnReturn.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnReturn.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnReturn.Image = CType(resources.GetObject("btnReturn.Image"), System.Drawing.Image)
        Me.btnReturn.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnReturn.Location = New System.Drawing.Point(8, 105)
        Me.btnReturn.Name = "btnReturn"
        Me.btnReturn.Size = New System.Drawing.Size(96, 23)
        Me.btnReturn.TabIndex = 7
        Me.btnReturn.Text = "&بازگشت"
        Me.btnReturn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnMakeReport
        '
        Me.btnMakeReport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnMakeReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMakeReport.Image = CType(resources.GetObject("btnMakeReport.Image"), System.Drawing.Image)
        Me.btnMakeReport.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnMakeReport.Location = New System.Drawing.Point(254, 105)
        Me.btnMakeReport.Name = "btnMakeReport"
        Me.btnMakeReport.Size = New System.Drawing.Size(96, 23)
        Me.btnMakeReport.TabIndex = 6
        Me.btnMakeReport.Text = "تهيه &گزارش"
        Me.btnMakeReport.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.txtDateFrom)
        Me.Panel1.Controls.Add(Me.btnDateTo)
        Me.Panel1.Controls.Add(Me.btnDateFrom)
        Me.Panel1.Controls.Add(Me.txtDateTo)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.cmbArea)
        Me.Panel1.Location = New System.Drawing.Point(7, 7)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(343, 86)
        Me.Panel1.TabIndex = 8
        '
        'txtDateFrom
        '
        Me.txtDateFrom.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDateFrom.BackColor = System.Drawing.SystemColors.Window
        Me.txtDateFrom.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDateFrom.IntegerDate = 0
        Me.txtDateFrom.IsShadow = False
        Me.txtDateFrom.IsShowCurrentDate = False
        Me.txtDateFrom.Location = New System.Drawing.Point(204, 49)
        Me.txtDateFrom.MaxLength = 10
        Me.txtDateFrom.MiladiDT = CType(resources.GetObject("txtDateFrom.MiladiDT"), Object)
        Me.txtDateFrom.Name = "txtDateFrom"
        Me.txtDateFrom.ReadOnly = True
        Me.txtDateFrom.ReadOnlyMaskedEdit = False
        Me.txtDateFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDateFrom.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtDateFrom.ShamsiDT = "____/__/__"
        Me.txtDateFrom.Size = New System.Drawing.Size(65, 20)
        Me.txtDateFrom.TabIndex = 17
        Me.txtDateFrom.Text = "____/__/__"
        Me.txtDateFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtDateFrom.TimeMaskedEditorOBJ = Nothing
        '
        'btnDateTo
        '
        Me.btnDateTo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDateTo.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDateTo.Location = New System.Drawing.Point(20, 49)
        Me.btnDateTo.Name = "btnDateTo"
        Me.btnDateTo.Size = New System.Drawing.Size(28, 22)
        Me.btnDateTo.TabIndex = 20
        Me.btnDateTo.Text = "..."
        '
        'btnDateFrom
        '
        Me.btnDateFrom.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDateFrom.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDateFrom.Location = New System.Drawing.Point(178, 49)
        Me.btnDateFrom.Name = "btnDateFrom"
        Me.btnDateFrom.Size = New System.Drawing.Size(26, 22)
        Me.btnDateFrom.TabIndex = 18
        Me.btnDateFrom.Text = "..."
        '
        'txtDateTo
        '
        Me.txtDateTo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDateTo.BackColor = System.Drawing.SystemColors.Window
        Me.txtDateTo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDateTo.IntegerDate = 0
        Me.txtDateTo.IsShadow = False
        Me.txtDateTo.IsShowCurrentDate = False
        Me.txtDateTo.Location = New System.Drawing.Point(48, 49)
        Me.txtDateTo.MaxLength = 10
        Me.txtDateTo.MiladiDT = CType(resources.GetObject("txtDateTo.MiladiDT"), Object)
        Me.txtDateTo.Name = "txtDateTo"
        Me.txtDateTo.ReadOnly = True
        Me.txtDateTo.ReadOnlyMaskedEdit = False
        Me.txtDateTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDateTo.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtDateTo.ShamsiDT = "____/__/__"
        Me.txtDateTo.Size = New System.Drawing.Size(65, 20)
        Me.txtDateTo.TabIndex = 19
        Me.txtDateTo.Text = "____/__/__"
        Me.txtDateTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtDateTo.TimeMaskedEditorOBJ = Nothing
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label3.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(124, 49)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(38, 18)
        Me.Label3.TabIndex = 11
        Me.Label3.Text = "تا تاريخ"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label2.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(284, 49)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(38, 18)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "از تاريخ"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label5.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label5.Font = New System.Drawing.Font("Mitra", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label5.Location = New System.Drawing.Point(290, 13)
        Me.Label5.Name = "Label5"
        Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label5.Size = New System.Drawing.Size(29, 18)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "ناحيه"
        '
        'cmbArea
        '
        Me.cmbArea.BackColor = System.Drawing.Color.White
        Me.cmbArea.CheckComboDropDownWidth = 0
        Me.cmbArea.CheckGroup = CType(resources.GetObject("cmbArea.CheckGroup"), System.Collections.ArrayList)
        Me.cmbArea.DropDownDirection = UtilityLibrary.Combos.DropDownDirection.Down
        Me.cmbArea.DropHeight = 500
        Me.cmbArea.IsGroup = False
        Me.cmbArea.IsMultiSelect = True
        Me.cmbArea.Location = New System.Drawing.Point(20, 13)
        Me.cmbArea.Name = "cmbArea"
        Me.cmbArea.ReadOnly = True
        Me.cmbArea.ReadOnlyList = ""
        Me.cmbArea.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.cmbArea.Size = New System.Drawing.Size(249, 21)
        Me.cmbArea.TabIndex = 9
        Me.cmbArea.TreeImageList = Nothing
        '
        'frmHomaReportRequest
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(358, 136)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.btnReturn)
        Me.Controls.Add(Me.btnMakeReport)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpMaker.SetHelpNavigator(Me, System.Windows.Forms.HelpNavigator.Topic)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "frmHomaReportRequest"
        Me.HelpMaker.SetShowHelp(Me, True)
        Me.Text = "گزارش پرونده هاي تکميل شده با تبلت"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btnReturn As Button
    Friend WithEvents btnMakeReport As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label5 As Label
    Friend WithEvents cmbArea As ChkCombo
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend txtDateFrom As PersianMaskedEditor
    Friend WithEvents btnDateTo As DateButton
    Friend WithEvents btnDateFrom As DateButton
    Friend txtDateTo As PersianMaskedEditor
End Class
