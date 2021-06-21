<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmReportDisHourByHour
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmReportDisHourByHour))
        Me.Label51 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cmbNwtworkType = New System.Windows.Forms.ComboBox()
        Me.BttnMakeReport = New System.Windows.Forms.Button()
        Me.BttnReturn = New System.Windows.Forms.Button()
        Me.cmbArea = New Bargh_Common.ChkCombo()
        Me.btnDate = New Bargh_Common.DateButton()
        Me.txtDate = New Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor()
        Me.SuspendLayout()
        '
        'HelpMaker
        '
        Me.HelpMaker.HelpNamespace = "Help\ReportsHelp.chm"
        '
        'Label51
        '
        Me.Label51.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label51.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label51.Location = New System.Drawing.Point(176, 25)
        Me.Label51.Name = "Label51"
        Me.Label51.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label51.Size = New System.Drawing.Size(73, 17)
        Me.Label51.TabIndex = 90
        Me.Label51.Text = " تاريخ"
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label2.Location = New System.Drawing.Point(176, 58)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(71, 14)
        Me.Label2.TabIndex = 91
        Me.Label2.Text = "ناحيه مرتبط"
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label3.Location = New System.Drawing.Point(178, 96)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(71, 14)
        Me.Label3.TabIndex = 92
        Me.Label3.Text = "نوع شبکه"
        '
        'cmbNwtworkType
        '
        Me.cmbNwtworkType.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbNwtworkType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbNwtworkType.Location = New System.Drawing.Point(12, 93)
        Me.cmbNwtworkType.Name = "cmbNwtworkType"
        Me.cmbNwtworkType.Size = New System.Drawing.Size(148, 21)
        Me.cmbNwtworkType.TabIndex = 4
        '
        'BttnMakeReport
        '
        Me.BttnMakeReport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BttnMakeReport.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.BttnMakeReport.Location = New System.Drawing.Point(172, 141)
        Me.BttnMakeReport.Name = "BttnMakeReport"
        Me.BttnMakeReport.Size = New System.Drawing.Size(75, 23)
        Me.BttnMakeReport.TabIndex = 5
        Me.BttnMakeReport.Text = "تهيه &گزارش"
        '
        'BttnReturn
        '
        Me.BttnReturn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BttnReturn.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.BttnReturn.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.BttnReturn.Location = New System.Drawing.Point(12, 141)
        Me.BttnReturn.Name = "BttnReturn"
        Me.BttnReturn.Size = New System.Drawing.Size(75, 23)
        Me.BttnReturn.TabIndex = 6
        Me.BttnReturn.Text = "&بازگشت"
        '
        'cmbArea
        '
        Me.cmbArea.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbArea.CheckComboDropDownWidth = 0
        Me.cmbArea.CheckGroup = CType(resources.GetObject("cmbArea.CheckGroup"), System.Collections.ArrayList)
        Me.cmbArea.DropDownDirection = UtilityLibrary.Combos.DropDownDirection.Down
        Me.cmbArea.DropHeight = 500
        Me.cmbArea.Font = New System.Drawing.Font("Tahoma", 8.0!)
        Me.cmbArea.IsGroup = False
        Me.cmbArea.IsMultiSelect = True
        Me.cmbArea.Location = New System.Drawing.Point(12, 58)
        Me.cmbArea.Name = "cmbArea"
        Me.cmbArea.ReadOnlyList = ""
        Me.cmbArea.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.cmbArea.Size = New System.Drawing.Size(148, 23)
        Me.cmbArea.TabIndex = 3
        Me.cmbArea.Text = "ChkCombo1"
        Me.cmbArea.TreeImageList = Nothing
        '
        'btnDate
        '
        Me.btnDate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDate.Location = New System.Drawing.Point(43, 25)
        Me.btnDate.Name = "btnDate"
        Me.btnDate.Size = New System.Drawing.Size(24, 22)
        Me.btnDate.TabIndex = 2
        Me.btnDate.Text = "..."
        '
        'txtDate
        '
        Me.txtDate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.txtDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDate.IntegerDate = 0
        Me.txtDate.IsShadow = False
        Me.txtDate.IsShowCurrentDate = False
        Me.txtDate.Location = New System.Drawing.Point(75, 25)
        Me.txtDate.MaxLength = 10
        Me.txtDate.MiladiDT = CType(resources.GetObject("txtDate.MiladiDT"), Object)
        Me.txtDate.Name = "txtDate"
        Me.txtDate.ReadOnly = True
        Me.txtDate.ReadOnlyMaskedEdit = False
        Me.txtDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDate.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.txtDate.ShamsiDT = "____/__/__"
        Me.txtDate.Size = New System.Drawing.Size(85, 22)
        Me.txtDate.TabIndex = 1
        Me.txtDate.Tag = "999"
        Me.txtDate.Text = "____/__/__"
        Me.txtDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtDate.TimeMaskedEditorOBJ = Nothing
        '
        'frmReportDisHourByHour
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(261, 176)
        Me.Controls.Add(Me.btnDate)
        Me.Controls.Add(Me.txtDate)
        Me.Controls.Add(Me.cmbArea)
        Me.Controls.Add(Me.BttnMakeReport)
        Me.Controls.Add(Me.BttnReturn)
        Me.Controls.Add(Me.cmbNwtworkType)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label51)
        Me.HelpMaker.SetHelpNavigator(Me, System.Windows.Forms.HelpNavigator.Topic)
        Me.Name = "frmReportDisHourByHour"
        Me.HelpMaker.SetShowHelp(Me, True)
        Me.Text = "frmReportDisHourByHour"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label51 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents cmbNwtworkType As ComboBox
    Friend WithEvents BttnMakeReport As Button
    Friend WithEvents BttnReturn As Button
    Friend WithEvents cmbArea As ChkCombo
    Friend WithEvents btnDate As DateButton
    Friend txtDate As PersianMaskedEditor
End Class
