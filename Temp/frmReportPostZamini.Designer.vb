<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmReportPostZamini
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmReportPostZamini))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.cmbLPPost = New Bargh_Common.ChkCombo()
        Me.cmbMPFeeder = New Bargh_Common.ChkCombo()
        Me.cmbMPPost = New Bargh_Common.ChkCombo()
        Me.cmbArea = New Bargh_Common.ChkCombo()
        Me.rbDej = New System.Windows.Forms.RadioButton()
        Me.rbFuse = New System.Windows.Forms.RadioButton()
        Me.rbSec = New System.Windows.Forms.RadioButton()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.BttnReturn = New System.Windows.Forms.Button()
        Me.BttnMakeReport = New System.Windows.Forms.Button()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'HelpMaker
        '
        Me.HelpMaker.HelpNamespace = "Help\ReportsHelp.chm"
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.cmbLPPost)
        Me.Panel1.Controls.Add(Me.cmbMPFeeder)
        Me.Panel1.Controls.Add(Me.cmbMPPost)
        Me.Panel1.Controls.Add(Me.cmbArea)
        Me.Panel1.Controls.Add(Me.rbDej)
        Me.Panel1.Controls.Add(Me.rbFuse)
        Me.Panel1.Controls.Add(Me.rbSec)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Location = New System.Drawing.Point(12, 8)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(360, 166)
        Me.Panel1.TabIndex = 0
        '
        'cmbLPPost
        '
        Me.cmbLPPost.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbLPPost.BackColor = System.Drawing.Color.White
        Me.cmbLPPost.CheckComboDropDownWidth = 0
        Me.cmbLPPost.CheckGroup = CType(resources.GetObject("cmbLPPost.CheckGroup"), System.Collections.ArrayList)
        Me.cmbLPPost.DropDownDirection = UtilityLibrary.Combos.DropDownDirection.Down
        Me.cmbLPPost.DropHeight = 500
        Me.cmbLPPost.Enabled = False
        Me.cmbLPPost.IsGroup = False
        Me.cmbLPPost.IsMultiSelect = True
        Me.cmbLPPost.Location = New System.Drawing.Point(13, 131)
        Me.cmbLPPost.Name = "cmbLPPost"
        Me.cmbLPPost.ReadOnly = True
        Me.cmbLPPost.ReadOnlyList = ""
        Me.cmbLPPost.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.cmbLPPost.Size = New System.Drawing.Size(248, 21)
        Me.cmbLPPost.TabIndex = 24
        Me.cmbLPPost.TreeImageList = Nothing
        '
        'cmbMPFeeder
        '
        Me.cmbMPFeeder.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbMPFeeder.BackColor = System.Drawing.Color.White
        Me.cmbMPFeeder.CheckComboDropDownWidth = 0
        Me.cmbMPFeeder.CheckGroup = CType(resources.GetObject("cmbMPFeeder.CheckGroup"), System.Collections.ArrayList)
        Me.cmbMPFeeder.DropDownDirection = UtilityLibrary.Combos.DropDownDirection.Down
        Me.cmbMPFeeder.DropHeight = 500
        Me.cmbMPFeeder.Enabled = False
        Me.cmbMPFeeder.IsGroup = False
        Me.cmbMPFeeder.IsMultiSelect = True
        Me.cmbMPFeeder.Location = New System.Drawing.Point(13, 104)
        Me.cmbMPFeeder.Name = "cmbMPFeeder"
        Me.cmbMPFeeder.ReadOnly = True
        Me.cmbMPFeeder.ReadOnlyList = ""
        Me.cmbMPFeeder.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.cmbMPFeeder.Size = New System.Drawing.Size(248, 21)
        Me.cmbMPFeeder.TabIndex = 23
        Me.cmbMPFeeder.TreeImageList = Nothing
        '
        'cmbMPPost
        '
        Me.cmbMPPost.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbMPPost.BackColor = System.Drawing.Color.White
        Me.cmbMPPost.CheckComboDropDownWidth = 0
        Me.cmbMPPost.CheckGroup = CType(resources.GetObject("cmbMPPost.CheckGroup"), System.Collections.ArrayList)
        Me.cmbMPPost.DropDownDirection = UtilityLibrary.Combos.DropDownDirection.Down
        Me.cmbMPPost.DropHeight = 500
        Me.cmbMPPost.Enabled = False
        Me.cmbMPPost.IsGroup = False
        Me.cmbMPPost.IsMultiSelect = True
        Me.cmbMPPost.Location = New System.Drawing.Point(13, 75)
        Me.cmbMPPost.Name = "cmbMPPost"
        Me.cmbMPPost.ReadOnly = True
        Me.cmbMPPost.ReadOnlyList = ""
        Me.cmbMPPost.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.cmbMPPost.Size = New System.Drawing.Size(248, 21)
        Me.cmbMPPost.TabIndex = 22
        Me.cmbMPPost.TreeImageList = Nothing
        '
        'cmbArea
        '
        Me.cmbArea.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbArea.BackColor = System.Drawing.Color.White
        Me.cmbArea.CheckComboDropDownWidth = 0
        Me.cmbArea.CheckGroup = CType(resources.GetObject("cmbArea.CheckGroup"), System.Collections.ArrayList)
        Me.cmbArea.DropDownDirection = UtilityLibrary.Combos.DropDownDirection.Down
        Me.cmbArea.DropHeight = 500
        Me.cmbArea.Enabled = False
        Me.cmbArea.IsGroup = False
        Me.cmbArea.IsMultiSelect = True
        Me.cmbArea.Location = New System.Drawing.Point(13, 44)
        Me.cmbArea.Name = "cmbArea"
        Me.cmbArea.ReadOnly = True
        Me.cmbArea.ReadOnlyList = ""
        Me.cmbArea.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.cmbArea.Size = New System.Drawing.Size(248, 21)
        Me.cmbArea.TabIndex = 21
        Me.cmbArea.TreeImageList = Nothing
        '
        'rbDej
        '
        Me.rbDej.AutoSize = True
        Me.rbDej.Location = New System.Drawing.Point(42, 14)
        Me.rbDej.Name = "rbDej"
        Me.rbDej.Size = New System.Drawing.Size(61, 17)
        Me.rbDej.TabIndex = 20
        Me.rbDej.TabStop = True
        Me.rbDej.Text = "دژنکتور"
        Me.rbDej.UseVisualStyleBackColor = True
        '
        'rbFuse
        '
        Me.rbFuse.AutoSize = True
        Me.rbFuse.Location = New System.Drawing.Point(129, 14)
        Me.rbFuse.Name = "rbFuse"
        Me.rbFuse.Size = New System.Drawing.Size(106, 17)
        Me.rbFuse.TabIndex = 19
        Me.rbFuse.TabStop = True
        Me.rbFuse.Text = "سکسيونر فيوزدار"
        Me.rbFuse.UseVisualStyleBackColor = True
        '
        'rbSec
        '
        Me.rbSec.AutoSize = True
        Me.rbSec.Location = New System.Drawing.Point(266, 14)
        Me.rbSec.Name = "rbSec"
        Me.rbSec.Size = New System.Drawing.Size(69, 17)
        Me.rbSec.TabIndex = 18
        Me.rbSec.TabStop = True
        Me.rbSec.Text = "سکسيونر"
        Me.rbSec.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label3.Font = New System.Drawing.Font("Mitra", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label3.Location = New System.Drawing.Point(271, 75)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(82, 19)
        Me.Label3.TabIndex = 11
        Me.Label3.Text = "پست فوق توزيع"
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label2.Font = New System.Drawing.Font("Mitra", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label2.Location = New System.Drawing.Point(271, 46)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(61, 19)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "ناحيه مرتبط"
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label4.Font = New System.Drawing.Font("Mitra", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label4.Location = New System.Drawing.Point(271, 104)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(84, 19)
        Me.Label4.TabIndex = 12
        Me.Label4.Text = "فيدر فشار متوسط"
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label1.Font = New System.Drawing.Font("Mitra", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label1.Location = New System.Drawing.Point(271, 133)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(62, 19)
        Me.Label1.TabIndex = 13
        Me.Label1.Text = "پست توزيع"
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.BttnReturn)
        Me.Panel2.Controls.Add(Me.BttnMakeReport)
        Me.Panel2.Location = New System.Drawing.Point(12, 180)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(360, 33)
        Me.Panel2.TabIndex = 1
        '
        'BttnReturn
        '
        Me.BttnReturn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BttnReturn.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.BttnReturn.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BttnReturn.Image = CType(resources.GetObject("BttnReturn.Image"), System.Drawing.Image)
        Me.BttnReturn.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.BttnReturn.Location = New System.Drawing.Point(6, 2)
        Me.BttnReturn.Name = "BttnReturn"
        Me.BttnReturn.Size = New System.Drawing.Size(89, 26)
        Me.BttnReturn.TabIndex = 5
        Me.BttnReturn.Text = "&بازگشت"
        Me.BttnReturn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'BttnMakeReport
        '
        Me.BttnMakeReport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BttnMakeReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BttnMakeReport.Image = CType(resources.GetObject("BttnMakeReport.Image"), System.Drawing.Image)
        Me.BttnMakeReport.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.BttnMakeReport.Location = New System.Drawing.Point(261, 2)
        Me.BttnMakeReport.Name = "BttnMakeReport"
        Me.BttnMakeReport.Size = New System.Drawing.Size(91, 25)
        Me.BttnMakeReport.TabIndex = 2
        Me.BttnMakeReport.Text = "تهيه &گزارش"
        Me.BttnMakeReport.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'frmReportPostZamini
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(384, 220)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.HelpMaker.SetHelpNavigator(Me, System.Windows.Forms.HelpNavigator.Topic)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmReportPostZamini"
        Me.HelpMaker.SetShowHelp(Me, True)
        Me.Text = "گزارش تجهیزات پست زمینی"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents rbDej As RadioButton
    Friend WithEvents rbFuse As RadioButton
    Friend WithEvents rbSec As RadioButton
    Friend WithEvents BttnMakeReport As Button
    Friend WithEvents BttnReturn As Button
    Friend WithEvents cmbLPPost As ChkCombo
    Friend WithEvents cmbMPFeeder As ChkCombo
    Friend WithEvents cmbMPPost As ChkCombo
    Friend WithEvents cmbArea As ChkCombo
End Class
