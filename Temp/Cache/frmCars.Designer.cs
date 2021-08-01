namespace Bargh_GIS
{
    partial class frmCars
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCars));
            Janus.Windows.GridEX.GridEXLayout dg_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.timerRefreshOncall = new System.Windows.Forms.Timer(this.components);
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pnlExpandSearch = new System.Windows.Forms.Panel();
            this.iconPnl = new System.Windows.Forms.Label();
            this.labelSearch = new System.Windows.Forms.Label();
            this.pnlExpandable = new System.Windows.Forms.Panel();
            this.cmdSelNone = new System.Windows.Forms.Button();
            this.cmdSelRev = new System.Windows.Forms.Button();
            this.cmdSelAll = new System.Windows.Forms.Button();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnShow = new System.Windows.Forms.Button();
            this.btnCloseFilter = new System.Windows.Forms.Button();
            this.dg = new Bargh_Common.JGrid();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.chkArea = new Bargh_Common.ChkCombo();
            this.BttnShowEkip = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.MapElementHost = new System.Windows.Forms.Integration.ElementHost();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pnlExpandSearch.SuspendLayout();
            this.pnlExpandable.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg)).BeginInit();
            this.pnlSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // HelpMaker
            // 
            this.HelpMaker.HelpNamespace = "Help\\ReportsHelp.chm";
            // 
            // timerRefreshOncall
            // 
            this.timerRefreshOncall.Enabled = true;
            this.timerRefreshOncall.Tick += new System.EventHandler(this.timerRefreshOncall_Tick);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.btnCancel);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 461);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(826, 35);
            this.panel3.TabIndex = 9;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.BackColor = System.Drawing.SystemColors.Control;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.Location = new System.Drawing.Point(7, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(102, 23);
            this.btnCancel.TabIndex = 259;
            this.btnCancel.Text = "&بازگشت";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.pnlExpandSearch);
            this.panel2.Controls.Add(this.pnlExpandable);
            this.panel2.Controls.Add(this.MapElementHost);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(826, 461);
            this.panel2.TabIndex = 10;
            // 
            // pnlExpandSearch
            // 
            this.pnlExpandSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlExpandSearch.BackColor = System.Drawing.Color.LightSteelBlue;
            this.pnlExpandSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlExpandSearch.Controls.Add(this.iconPnl);
            this.pnlExpandSearch.Controls.Add(this.labelSearch);
            this.pnlExpandSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlExpandSearch.Location = new System.Drawing.Point(3, 3);
            this.pnlExpandSearch.Name = "pnlExpandSearch";
            this.pnlExpandSearch.Size = new System.Drawing.Size(818, 27);
            this.pnlExpandSearch.TabIndex = 268;
            this.pnlExpandSearch.Tag = "999";
            this.pnlExpandSearch.Click += new System.EventHandler(this.pnlExpandSearch_Click);
            this.pnlExpandSearch.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlExpandSearch_Paint);
            // 
            // iconPnl
            // 
            this.iconPnl.BackColor = System.Drawing.Color.Transparent;
            this.iconPnl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.iconPnl.Image = global::Bargh_GIS.Properties.Resources.ArrowUp;
            this.iconPnl.Location = new System.Drawing.Point(2, 5);
            this.iconPnl.Name = "iconPnl";
            this.iconPnl.Size = new System.Drawing.Size(26, 18);
            this.iconPnl.TabIndex = 12;
            this.iconPnl.Tag = "999";
            this.iconPnl.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.iconPnl.Click += new System.EventHandler(this.pnlExpandSearch_Click);
            // 
            // labelSearch
            // 
            this.labelSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelSearch.BackColor = System.Drawing.Color.Transparent;
            this.labelSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.labelSearch.Location = new System.Drawing.Point(740, 4);
            this.labelSearch.Name = "labelSearch";
            this.labelSearch.Size = new System.Drawing.Size(73, 17);
            this.labelSearch.TabIndex = 11;
            this.labelSearch.Tag = "999";
            this.labelSearch.Text = "جستجو";
            this.labelSearch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelSearch.Click += new System.EventHandler(this.pnlExpandSearch_Click);
            // 
            // pnlExpandable
            // 
            this.pnlExpandable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlExpandable.Controls.Add(this.cmdSelNone);
            this.pnlExpandable.Controls.Add(this.cmdSelRev);
            this.pnlExpandable.Controls.Add(this.cmdSelAll);
            this.pnlExpandable.Controls.Add(this.pnlButtons);
            this.pnlExpandable.Controls.Add(this.dg);
            this.pnlExpandable.Controls.Add(this.pnlSearch);
            this.pnlExpandable.Location = new System.Drawing.Point(3, 30);
            this.pnlExpandable.Name = "pnlExpandable";
            this.pnlExpandable.Size = new System.Drawing.Size(820, 260);
            this.pnlExpandable.TabIndex = 267;
            // 
            // cmdSelNone
            // 
            this.cmdSelNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdSelNone.BackColor = System.Drawing.Color.Transparent;
            this.cmdSelNone.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.cmdSelNone.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdSelNone.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmdSelNone.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdSelNone.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdSelNone.Image = ((System.Drawing.Image)(resources.GetObject("cmdSelNone.Image")));
            this.cmdSelNone.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmdSelNone.Location = new System.Drawing.Point(10, 163);
            this.cmdSelNone.Name = "cmdSelNone";
            this.cmdSelNone.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cmdSelNone.Size = new System.Drawing.Size(109, 48);
            this.cmdSelNone.TabIndex = 29;
            this.cmdSelNone.Text = "حذف همه";
            this.cmdSelNone.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdSelNone.UseVisualStyleBackColor = false;
            this.cmdSelNone.Click += new System.EventHandler(this.cmdSelNone_Click);
            // 
            // cmdSelRev
            // 
            this.cmdSelRev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdSelRev.BackColor = System.Drawing.Color.Transparent;
            this.cmdSelRev.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.cmdSelRev.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdSelRev.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmdSelRev.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdSelRev.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdSelRev.Image = ((System.Drawing.Image)(resources.GetObject("cmdSelRev.Image")));
            this.cmdSelRev.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmdSelRev.Location = new System.Drawing.Point(10, 105);
            this.cmdSelRev.Name = "cmdSelRev";
            this.cmdSelRev.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cmdSelRev.Size = new System.Drawing.Size(109, 52);
            this.cmdSelRev.TabIndex = 28;
            this.cmdSelRev.Text = "انتخاب معكوس";
            this.cmdSelRev.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdSelRev.UseVisualStyleBackColor = false;
            this.cmdSelRev.Click += new System.EventHandler(this.cmdSelRev_Click);
            // 
            // cmdSelAll
            // 
            this.cmdSelAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdSelAll.BackColor = System.Drawing.Color.Transparent;
            this.cmdSelAll.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.cmdSelAll.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdSelAll.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmdSelAll.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdSelAll.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdSelAll.Image = ((System.Drawing.Image)(resources.GetObject("cmdSelAll.Image")));
            this.cmdSelAll.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmdSelAll.Location = new System.Drawing.Point(10, 50);
            this.cmdSelAll.Name = "cmdSelAll";
            this.cmdSelAll.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cmdSelAll.Size = new System.Drawing.Size(109, 49);
            this.cmdSelAll.TabIndex = 27;
            this.cmdSelAll.Text = "انتخاب همه";
            this.cmdSelAll.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdSelAll.UseVisualStyleBackColor = false;
            this.cmdSelAll.Click += new System.EventHandler(this.cmdSelAll_Click);
            // 
            // pnlButtons
            // 
            this.pnlButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlButtons.BackColor = System.Drawing.Color.Transparent;
            this.pnlButtons.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlButtons.Controls.Add(this.btnShow);
            this.pnlButtons.Controls.Add(this.btnCloseFilter);
            this.pnlButtons.Location = new System.Drawing.Point(10, 217);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(802, 38);
            this.pnlButtons.TabIndex = 23;
            // 
            // btnShow
            // 
            this.btnShow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShow.BackColor = System.Drawing.Color.Transparent;
            this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnShow.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnShow.Location = new System.Drawing.Point(546, 7);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(251, 25);
            this.btnShow.TabIndex = 12;
            this.btnShow.Text = "نمايش روِي نقشه";
            this.btnShow.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnShow.UseVisualStyleBackColor = false;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // btnCloseFilter
            // 
            this.btnCloseFilter.BackColor = System.Drawing.Color.Transparent;
            this.btnCloseFilter.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCloseFilter.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCloseFilter.Location = new System.Drawing.Point(9, 7);
            this.btnCloseFilter.Name = "btnCloseFilter";
            this.btnCloseFilter.Size = new System.Drawing.Size(110, 25);
            this.btnCloseFilter.TabIndex = 14;
            this.btnCloseFilter.Text = "بستن فیلتر";
            this.btnCloseFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCloseFilter.UseVisualStyleBackColor = false;
            this.btnCloseFilter.Click += new System.EventHandler(this.btnCloseFilter_Click);
            // 
            // dg
            // 
            this.dg.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.dg.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dg.BackColor = System.Drawing.Color.White;
            this.dg.BuiltInTextsData = resources.GetString("dg.BuiltInTextsData");
            this.dg.CurrentRowIndex = -1;
            dg_DesignTimeLayout.LayoutString = resources.GetString("dg_DesignTimeLayout.LayoutString");
            this.dg.DesignTimeLayout = dg_DesignTimeLayout;
            this.dg.EnableFormatEvent = true;
            this.dg.EnableSaveLayout = true;
            this.dg.GroupByBoxFormatStyle.Font = new System.Drawing.Font("Tahoma", 9F);
            this.dg.GroupByBoxInfoFormatStyle.Font = new System.Drawing.Font("Tahoma", 9F);
            this.dg.GroupByBoxVisible = false;
            this.dg.HeaderFormatStyle.BackColor = System.Drawing.Color.White;
            this.dg.HeaderFormatStyle.BackColorGradient = System.Drawing.Color.LightGray;
            this.dg.HeaderFormatStyle.LineAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.dg.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.dg.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            this.dg.IsColor = false;
            this.dg.IsColumnContextMenu = true;
            this.dg.IsForMonitoring = false;
            this.dg.Location = new System.Drawing.Point(125, 50);
            this.dg.Name = "dg";
            this.dg.OwnerDrawnAreas = ((Janus.Windows.GridEX.GridEXOwnerDrawnArea)((((((((Janus.Windows.GridEX.GridEXOwnerDrawnArea.GroupByBoxInfoText | Janus.Windows.GridEX.GridEXOwnerDrawnArea.GroupByBoxTableInfo) 
            | Janus.Windows.GridEX.GridEXOwnerDrawnArea.RowHeaders) 
            | Janus.Windows.GridEX.GridEXOwnerDrawnArea.TableHeaders) 
            | Janus.Windows.GridEX.GridEXOwnerDrawnArea.NewAndTotalRowSeparators) 
            | Janus.Windows.GridEX.GridEXOwnerDrawnArea.PreviewRows) 
            | Janus.Windows.GridEX.GridEXOwnerDrawnArea.Background) 
            | Janus.Windows.GridEX.GridEXOwnerDrawnArea.CardCaption)));
            this.dg.PrintLandScape = true;
            this.dg.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.dg.SaveGridName = "";
            this.dg.SelectionMode = Janus.Windows.GridEX.SelectionMode.MultipleSelection;
            this.dg.Size = new System.Drawing.Size(693, 161);
            this.dg.TabIndex = 21;
            this.dg.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2010;
            // 
            // pnlSearch
            // 
            this.pnlSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSearch.BackColor = System.Drawing.Color.Transparent;
            this.pnlSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSearch.Controls.Add(this.chkArea);
            this.pnlSearch.Controls.Add(this.BttnShowEkip);
            this.pnlSearch.Controls.Add(this.label1);
            this.pnlSearch.Location = new System.Drawing.Point(0, 3);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(817, 41);
            this.pnlSearch.TabIndex = 22;
            // 
            // chkArea
            // 
            this.chkArea.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkArea.CheckComboDropDownWidth = 0;
            this.chkArea.CheckGroup = ((System.Collections.ArrayList)(resources.GetObject("chkArea.CheckGroup")));
            this.chkArea.DropDownDirection = UtilityLibrary.Combos.DropDownDirection.Down;
            this.chkArea.DropHeight = 500;
            this.chkArea.IsGroup = false;
            this.chkArea.IsMultiSelect = true;
            this.chkArea.Location = new System.Drawing.Point(465, 9);
            this.chkArea.Name = "chkArea";
            this.chkArea.ReadOnlyList = "";
            this.chkArea.Size = new System.Drawing.Size(312, 23);
            this.chkArea.TabIndex = 16;
            this.chkArea.TreeImageList = null;
            // 
            // BttnShowEkip
            // 
            this.BttnShowEkip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BttnShowEkip.Location = new System.Drawing.Point(375, 3);
            this.BttnShowEkip.Name = "BttnShowEkip";
            this.BttnShowEkip.Size = new System.Drawing.Size(75, 31);
            this.BttnShowEkip.TabIndex = 15;
            this.BttnShowEkip.Text = "جستجو";
            this.BttnShowEkip.UseVisualStyleBackColor = true;
            this.BttnShowEkip.Click += new System.EventHandler(this.BttnShowEkip_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Mitra", 9F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(783, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 18);
            this.label1.TabIndex = 14;
            this.label1.Text = "ناحيه";
            // 
            // MapElementHost
            // 
            this.MapElementHost.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MapElementHost.Location = new System.Drawing.Point(0, 30);
            this.MapElementHost.Name = "MapElementHost";
            this.MapElementHost.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.MapElementHost.Size = new System.Drawing.Size(824, 426);
            this.MapElementHost.TabIndex = 9;
            this.MapElementHost.Text = "elementHost1";
            this.MapElementHost.Child = null;
            // 
            // frmCars
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(826, 496);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.HelpMaker.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.Topic);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(842, 535);
            this.Name = "frmCars";
            this.HelpMaker.SetShowHelp(this, true);
            this.Text = "طرح هما - نقشه";
            this.Load += new System.EventHandler(this.frmCars_Load);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.pnlExpandSearch.ResumeLayout(false);
            this.pnlExpandable.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dg)).EndInit();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timerRefreshOncall;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Integration.ElementHost MapElementHost;
        internal System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel pnlExpandable;
        private System.Windows.Forms.Panel pnlButtons;
        internal System.Windows.Forms.Button btnShow;
        internal System.Windows.Forms.Button btnCloseFilter;
        internal Bargh_Common.JGrid dg;
        private System.Windows.Forms.Panel pnlSearch;
        private Bargh_Common.ChkCombo chkArea;
        private System.Windows.Forms.Button BttnShowEkip;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlExpandSearch;
        private System.Windows.Forms.Label iconPnl;
        private System.Windows.Forms.Label labelSearch;
        public System.Windows.Forms.Button cmdSelAll;
        public System.Windows.Forms.Button cmdSelRev;
        public System.Windows.Forms.Button cmdSelNone;
    }
}