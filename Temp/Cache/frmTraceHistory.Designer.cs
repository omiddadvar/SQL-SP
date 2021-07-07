namespace Bargh_GIS
{
    partial class frmTraceHistory
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTraceHistory));
            Janus.Windows.GridEX.GridEXLayout dgRequest_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout dgArea_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout dgMaster_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout dgTablet_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.MapElementHost = new System.Windows.Forms.Integration.ElementHost();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlSearchExpand = new DevComponents.DotNetBar.ExpandablePanel();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnRequests = new System.Windows.Forms.Button();
            this.btnShow = new System.Windows.Forms.Button();
            this.btnCloseFilter = new System.Windows.Forms.Button();
            this.dgRequest = new Bargh_Common.JGrid();
            this.dgArea = new Bargh_Common.JGrid();
            this.dgMaster = new Bargh_Common.JGrid();
            this.dgTablet = new Bargh_Common.JGrid();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnToDate = new Bargh_Common.DateButton();
            this.btnFromDate = new Bargh_Common.DateButton();
            this.label5 = new System.Windows.Forms.Label();
            this.cboArea = new Bargh_Common.ComboBoxPersian();
            this.label4 = new System.Windows.Forms.Label();
            this.txtFromTime = new Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor();
            this.txtFromDate = new Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor();
            this.txtToTime = new Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor();
            this.txtToDate = new Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlSearchExpand.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgRequest)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgMaster)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgTablet)).BeginInit();
            this.pnlSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // HelpMaker
            // 
            this.HelpMaker.HelpNamespace = "Help\\ReportsHelp.chm";
            // 
            // MapElementHost
            // 
            this.MapElementHost.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MapElementHost.Location = new System.Drawing.Point(12, 35);
            this.MapElementHost.Name = "MapElementHost";
            this.MapElementHost.Size = new System.Drawing.Size(783, 390);
            this.MapElementHost.TabIndex = 0;
            this.MapElementHost.Text = "elementHost1";
            this.MapElementHost.Child = null;
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
            this.btnCancel.Location = new System.Drawing.Point(12, 431);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(102, 23);
            this.btnCancel.TabIndex = 260;
            this.btnCancel.Text = "&بازگشت";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // pnlSearchExpand
            // 
            this.pnlSearchExpand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSearchExpand.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pnlSearchExpand.CanvasColor = System.Drawing.SystemColors.Control;
            this.pnlSearchExpand.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.pnlSearchExpand.Controls.Add(this.pnlButtons);
            this.pnlSearchExpand.Controls.Add(this.dgRequest);
            this.pnlSearchExpand.Controls.Add(this.dgArea);
            this.pnlSearchExpand.Controls.Add(this.dgMaster);
            this.pnlSearchExpand.Controls.Add(this.dgTablet);
            this.pnlSearchExpand.Controls.Add(this.pnlSearch);
            this.pnlSearchExpand.DisabledBackColor = System.Drawing.Color.Empty;
            this.pnlSearchExpand.ExpandOnTitleClick = true;
            this.pnlSearchExpand.HideControlsWhenCollapsed = true;
            this.pnlSearchExpand.Location = new System.Drawing.Point(1, 3);
            this.pnlSearchExpand.Name = "pnlSearchExpand";
            this.pnlSearchExpand.Size = new System.Drawing.Size(794, 360);
            this.pnlSearchExpand.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.pnlSearchExpand.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.pnlSearchExpand.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.CustomizeBackground;
            this.pnlSearchExpand.Style.BackgroundImagePosition = DevComponents.DotNetBar.eBackgroundImagePosition.Center;
            this.pnlSearchExpand.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.pnlSearchExpand.Style.BorderColor.Color = System.Drawing.Color.Transparent;
            this.pnlSearchExpand.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.pnlSearchExpand.Style.GradientAngle = 90;
            this.pnlSearchExpand.TabIndex = 261;
            this.pnlSearchExpand.TitleStyle.Alignment = System.Drawing.StringAlignment.Far;
            this.pnlSearchExpand.TitleStyle.BackColor1.Color = System.Drawing.Color.White;
            this.pnlSearchExpand.TitleStyle.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.pnlSearchExpand.TitleStyle.Border = DevComponents.DotNetBar.eBorderType.RaisedInner;
            this.pnlSearchExpand.TitleStyle.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.pnlSearchExpand.TitleStyle.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.pnlSearchExpand.TitleStyle.GradientAngle = 90;
            this.pnlSearchExpand.TitleText = "جستجو";
            this.pnlSearchExpand.ExpandedChanged += new DevComponents.DotNetBar.ExpandChangeEventHandler(this.pnlSearchExpand_ExpandedChanged);
            // 
            // pnlButtons
            // 
            this.pnlButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlButtons.BackColor = System.Drawing.Color.Transparent;
            this.pnlButtons.Controls.Add(this.btnRequests);
            this.pnlButtons.Controls.Add(this.btnShow);
            this.pnlButtons.Controls.Add(this.btnCloseFilter);
            this.pnlButtons.Location = new System.Drawing.Point(11, 314);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(776, 38);
            this.pnlButtons.TabIndex = 17;
            // 
            // btnRequests
            // 
            this.btnRequests.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnRequests.BackColor = System.Drawing.Color.Transparent;
            this.btnRequests.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRequests.Image = ((System.Drawing.Image)(resources.GetObject("btnRequests.Image")));
            this.btnRequests.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRequests.Location = new System.Drawing.Point(272, 7);
            this.btnRequests.Name = "btnRequests";
            this.btnRequests.Size = new System.Drawing.Size(140, 25);
            this.btnRequests.TabIndex = 13;
            this.btnRequests.Text = "جـسـتـجـو خاموشي ها";
            this.btnRequests.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRequests.UseVisualStyleBackColor = false;
            this.btnRequests.Visible = false;
            this.btnRequests.Click += new System.EventHandler(this.btnRequests_Click);
            // 
            // btnShow
            // 
            this.btnShow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShow.BackColor = System.Drawing.Color.Transparent;
            this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnShow.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnShow.Location = new System.Drawing.Point(522, 7);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(251, 25);
            this.btnShow.TabIndex = 12;
            this.btnShow.Text = "نمايش روِي نقشه";
            this.btnShow.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnShow.UseVisualStyleBackColor = false;
            this.btnShow.Visible = false;
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
            this.btnCloseFilter.Visible = false;
            this.btnCloseFilter.Click += new System.EventHandler(this.btnCloseFilter_Click);
            // 
            // dgRequest
            // 
            this.dgRequest.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.dgRequest.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dgRequest.BackColor = System.Drawing.Color.Gray;
            this.dgRequest.BuiltInTextsData = resources.GetString("dgRequest.BuiltInTextsData");
            this.dgRequest.CurrentRowIndex = -1;
            dgRequest_DesignTimeLayout.LayoutString = resources.GetString("dgRequest_DesignTimeLayout.LayoutString");
            this.dgRequest.DesignTimeLayout = dgRequest_DesignTimeLayout;
            this.dgRequest.EnableFormatEvent = true;
            this.dgRequest.EnableSaveLayout = true;
            this.dgRequest.GroupByBoxFormatStyle.Font = new System.Drawing.Font("Tahoma", 9F);
            this.dgRequest.GroupByBoxInfoFormatStyle.Font = new System.Drawing.Font("Tahoma", 9F);
            this.dgRequest.GroupByBoxVisible = false;
            this.dgRequest.HeaderFormatStyle.BackColor = System.Drawing.Color.White;
            this.dgRequest.HeaderFormatStyle.BackColorGradient = System.Drawing.Color.LightGray;
            this.dgRequest.HeaderFormatStyle.LineAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.dgRequest.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.dgRequest.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            this.dgRequest.IsColor = false;
            this.dgRequest.IsColumnContextMenu = true;
            this.dgRequest.IsForMonitoring = false;
            this.dgRequest.Location = new System.Drawing.Point(11, 196);
            this.dgRequest.Name = "dgRequest";
            this.dgRequest.OwnerDrawnAreas = ((Janus.Windows.GridEX.GridEXOwnerDrawnArea)((((((((Janus.Windows.GridEX.GridEXOwnerDrawnArea.GroupByBoxInfoText | Janus.Windows.GridEX.GridEXOwnerDrawnArea.GroupByBoxTableInfo) 
            | Janus.Windows.GridEX.GridEXOwnerDrawnArea.RowHeaders) 
            | Janus.Windows.GridEX.GridEXOwnerDrawnArea.TableHeaders) 
            | Janus.Windows.GridEX.GridEXOwnerDrawnArea.NewAndTotalRowSeparators) 
            | Janus.Windows.GridEX.GridEXOwnerDrawnArea.PreviewRows) 
            | Janus.Windows.GridEX.GridEXOwnerDrawnArea.Background) 
            | Janus.Windows.GridEX.GridEXOwnerDrawnArea.CardCaption)));
            this.dgRequest.PrintLandScape = true;
            this.dgRequest.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.dgRequest.SaveGridName = "";
            this.dgRequest.SelectionMode = Janus.Windows.GridEX.SelectionMode.MultipleSelection;
            this.dgRequest.Size = new System.Drawing.Size(776, 112);
            this.dgRequest.TabIndex = 15;
            this.dgRequest.Visible = false;
            this.dgRequest.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2010;
            // 
            // dgArea
            // 
            this.dgArea.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.dgArea.BackColor = System.Drawing.Color.Gray;
            this.dgArea.BuiltInTextsData = resources.GetString("dgArea.BuiltInTextsData");
            this.dgArea.CurrentRowIndex = -1;
            dgArea_DesignTimeLayout.LayoutString = resources.GetString("dgArea_DesignTimeLayout.LayoutString");
            this.dgArea.DesignTimeLayout = dgArea_DesignTimeLayout;
            this.dgArea.EnableFormatEvent = true;
            this.dgArea.EnableSaveLayout = true;
            this.dgArea.GroupByBoxFormatStyle.Font = new System.Drawing.Font("Tahoma", 9F);
            this.dgArea.GroupByBoxInfoFormatStyle.Font = new System.Drawing.Font("Tahoma", 9F);
            this.dgArea.GroupByBoxVisible = false;
            this.dgArea.HeaderFormatStyle.BackColor = System.Drawing.Color.White;
            this.dgArea.HeaderFormatStyle.BackColorGradient = System.Drawing.Color.LightGray;
            this.dgArea.HeaderFormatStyle.LineAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.dgArea.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.dgArea.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            this.dgArea.IsColor = false;
            this.dgArea.IsColumnContextMenu = true;
            this.dgArea.IsForMonitoring = false;
            this.dgArea.Location = new System.Drawing.Point(11, 97);
            this.dgArea.Name = "dgArea";
            this.dgArea.OwnerDrawnAreas = ((Janus.Windows.GridEX.GridEXOwnerDrawnArea)((((((((Janus.Windows.GridEX.GridEXOwnerDrawnArea.GroupByBoxInfoText | Janus.Windows.GridEX.GridEXOwnerDrawnArea.GroupByBoxTableInfo) 
            | Janus.Windows.GridEX.GridEXOwnerDrawnArea.RowHeaders) 
            | Janus.Windows.GridEX.GridEXOwnerDrawnArea.TableHeaders) 
            | Janus.Windows.GridEX.GridEXOwnerDrawnArea.NewAndTotalRowSeparators) 
            | Janus.Windows.GridEX.GridEXOwnerDrawnArea.PreviewRows) 
            | Janus.Windows.GridEX.GridEXOwnerDrawnArea.Background) 
            | Janus.Windows.GridEX.GridEXOwnerDrawnArea.CardCaption)));
            this.dgArea.PrintLandScape = true;
            this.dgArea.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.dgArea.SaveGridName = "";
            this.dgArea.SelectionMode = Janus.Windows.GridEX.SelectionMode.MultipleSelection;
            this.dgArea.Size = new System.Drawing.Size(266, 93);
            this.dgArea.TabIndex = 11;
            this.dgArea.Visible = false;
            this.dgArea.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2010;
            // 
            // dgMaster
            // 
            this.dgMaster.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.dgMaster.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dgMaster.BackColor = System.Drawing.Color.Gray;
            this.dgMaster.BuiltInTextsData = resources.GetString("dgMaster.BuiltInTextsData");
            this.dgMaster.CurrentRowIndex = -1;
            dgMaster_DesignTimeLayout.LayoutString = resources.GetString("dgMaster_DesignTimeLayout.LayoutString");
            this.dgMaster.DesignTimeLayout = dgMaster_DesignTimeLayout;
            this.dgMaster.EnableFormatEvent = true;
            this.dgMaster.EnableSaveLayout = true;
            this.dgMaster.GroupByBoxFormatStyle.Font = new System.Drawing.Font("Tahoma", 9F);
            this.dgMaster.GroupByBoxInfoFormatStyle.Font = new System.Drawing.Font("Tahoma", 9F);
            this.dgMaster.GroupByBoxVisible = false;
            this.dgMaster.HeaderFormatStyle.BackColor = System.Drawing.Color.White;
            this.dgMaster.HeaderFormatStyle.BackColorGradient = System.Drawing.Color.LightGray;
            this.dgMaster.HeaderFormatStyle.LineAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.dgMaster.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.dgMaster.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            this.dgMaster.IsColor = false;
            this.dgMaster.IsColumnContextMenu = true;
            this.dgMaster.IsForMonitoring = false;
            this.dgMaster.Location = new System.Drawing.Point(283, 97);
            this.dgMaster.Name = "dgMaster";
            this.dgMaster.OwnerDrawnAreas = ((Janus.Windows.GridEX.GridEXOwnerDrawnArea)((((((((Janus.Windows.GridEX.GridEXOwnerDrawnArea.GroupByBoxInfoText | Janus.Windows.GridEX.GridEXOwnerDrawnArea.GroupByBoxTableInfo) 
            | Janus.Windows.GridEX.GridEXOwnerDrawnArea.RowHeaders) 
            | Janus.Windows.GridEX.GridEXOwnerDrawnArea.TableHeaders) 
            | Janus.Windows.GridEX.GridEXOwnerDrawnArea.NewAndTotalRowSeparators) 
            | Janus.Windows.GridEX.GridEXOwnerDrawnArea.PreviewRows) 
            | Janus.Windows.GridEX.GridEXOwnerDrawnArea.Background) 
            | Janus.Windows.GridEX.GridEXOwnerDrawnArea.CardCaption)));
            this.dgMaster.PrintLandScape = true;
            this.dgMaster.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.dgMaster.SaveGridName = "";
            this.dgMaster.SelectionMode = Janus.Windows.GridEX.SelectionMode.MultipleSelection;
            this.dgMaster.Size = new System.Drawing.Size(244, 93);
            this.dgMaster.TabIndex = 10;
            this.dgMaster.Visible = false;
            this.dgMaster.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2010;
            // 
            // dgTablet
            // 
            this.dgTablet.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.dgTablet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dgTablet.BackColor = System.Drawing.Color.Gray;
            this.dgTablet.BuiltInTextsData = resources.GetString("dgTablet.BuiltInTextsData");
            this.dgTablet.CurrentRowIndex = -1;
            dgTablet_DesignTimeLayout.LayoutString = resources.GetString("dgTablet_DesignTimeLayout.LayoutString");
            this.dgTablet.DesignTimeLayout = dgTablet_DesignTimeLayout;
            this.dgTablet.EnableFormatEvent = true;
            this.dgTablet.EnableSaveLayout = true;
            this.dgTablet.GroupByBoxFormatStyle.Font = new System.Drawing.Font("Tahoma", 9F);
            this.dgTablet.GroupByBoxInfoFormatStyle.Font = new System.Drawing.Font("Tahoma", 9F);
            this.dgTablet.GroupByBoxVisible = false;
            this.dgTablet.HeaderFormatStyle.BackColor = System.Drawing.Color.White;
            this.dgTablet.HeaderFormatStyle.BackColorGradient = System.Drawing.Color.LightGray;
            this.dgTablet.HeaderFormatStyle.LineAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.dgTablet.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.dgTablet.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            this.dgTablet.IsColor = false;
            this.dgTablet.IsColumnContextMenu = true;
            this.dgTablet.IsForMonitoring = false;
            this.dgTablet.Location = new System.Drawing.Point(533, 97);
            this.dgTablet.Name = "dgTablet";
            this.dgTablet.OwnerDrawnAreas = ((Janus.Windows.GridEX.GridEXOwnerDrawnArea)((((((((Janus.Windows.GridEX.GridEXOwnerDrawnArea.GroupByBoxInfoText | Janus.Windows.GridEX.GridEXOwnerDrawnArea.GroupByBoxTableInfo) 
            | Janus.Windows.GridEX.GridEXOwnerDrawnArea.RowHeaders) 
            | Janus.Windows.GridEX.GridEXOwnerDrawnArea.TableHeaders) 
            | Janus.Windows.GridEX.GridEXOwnerDrawnArea.NewAndTotalRowSeparators) 
            | Janus.Windows.GridEX.GridEXOwnerDrawnArea.PreviewRows) 
            | Janus.Windows.GridEX.GridEXOwnerDrawnArea.Background) 
            | Janus.Windows.GridEX.GridEXOwnerDrawnArea.CardCaption)));
            this.dgTablet.PrintLandScape = true;
            this.dgTablet.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.dgTablet.SaveGridName = "";
            this.dgTablet.SelectionMode = Janus.Windows.GridEX.SelectionMode.MultipleSelection;
            this.dgTablet.Size = new System.Drawing.Size(254, 93);
            this.dgTablet.TabIndex = 9;
            this.dgTablet.Visible = false;
            this.dgTablet.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2010;
            // 
            // pnlSearch
            // 
            this.pnlSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSearch.BackColor = System.Drawing.Color.Transparent;
            this.pnlSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Controls.Add(this.btnToDate);
            this.pnlSearch.Controls.Add(this.btnFromDate);
            this.pnlSearch.Controls.Add(this.label5);
            this.pnlSearch.Controls.Add(this.cboArea);
            this.pnlSearch.Controls.Add(this.label4);
            this.pnlSearch.Controls.Add(this.txtFromTime);
            this.pnlSearch.Controls.Add(this.txtFromDate);
            this.pnlSearch.Controls.Add(this.txtToTime);
            this.pnlSearch.Controls.Add(this.txtToDate);
            this.pnlSearch.Controls.Add(this.label3);
            this.pnlSearch.Controls.Add(this.label2);
            this.pnlSearch.Controls.Add(this.label1);
            this.pnlSearch.Location = new System.Drawing.Point(11, 32);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(776, 59);
            this.pnlSearch.TabIndex = 16;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearch.Location = new System.Drawing.Point(26, 12);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(92, 37);
            this.btnSearch.TabIndex = 8;
            this.btnSearch.Text = "جـسـتـجـو";
            this.btnSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnToDate
            // 
            this.btnToDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.btnToDate.Location = new System.Drawing.Point(597, 29);
            this.btnToDate.Name = "btnToDate";
            this.btnToDate.Size = new System.Drawing.Size(24, 22);
            this.btnToDate.TabIndex = 4;
            this.btnToDate.Text = "...";
            // 
            // btnFromDate
            // 
            this.btnFromDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFromDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.btnFromDate.Location = new System.Drawing.Point(597, 6);
            this.btnFromDate.Name = "btnFromDate";
            this.btnFromDate.Size = new System.Drawing.Size(24, 22);
            this.btnFromDate.TabIndex = 2;
            this.btnFromDate.Text = "...";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(420, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 18);
            this.label5.TabIndex = 49;
            this.label5.Text = "ناحیه";
            // 
            // cboArea
            // 
            this.cboArea.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cboArea.BackColor = System.Drawing.Color.White;
            this.cboArea.DisplayMember = "Area";
            this.cboArea.IsReadOnly = false;
            this.cboArea.Location = new System.Drawing.Point(224, 9);
            this.cboArea.Name = "cboArea";
            this.cboArea.Size = new System.Drawing.Size(192, 21);
            this.cboArea.TabIndex = 7;
            this.cboArea.ValueMember = "AreaId";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(545, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 19);
            this.label4.TabIndex = 12;
            this.label4.Text = "و ساعت";
            // 
            // txtFromTime
            // 
            this.txtFromTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFromTime.BackColor = System.Drawing.SystemColors.Window;
            this.txtFromTime.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtFromTime.IsShadow = false;
            this.txtFromTime.IsShowCurrentTime = false;
            this.txtFromTime.Location = new System.Drawing.Point(499, 5);
            this.txtFromTime.MaxLength = 5;
            this.txtFromTime.MiladiDT = null;
            this.txtFromTime.Name = "txtFromTime";
            this.txtFromTime.ReadOnly = true;
            this.txtFromTime.ReadOnlyMaskedEdit = false;
            this.txtFromTime.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtFromTime.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(190)))), ((int)(((byte)(160)))));
            this.txtFromTime.Size = new System.Drawing.Size(40, 20);
            this.txtFromTime.TabIndex = 5;
            this.txtFromTime.Text = "__:__";
            this.txtFromTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtFromDate
            // 
            this.txtFromDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFromDate.BackColor = System.Drawing.SystemColors.Window;
            this.txtFromDate.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtFromDate.IntegerDate = 0;
            this.txtFromDate.IsShadow = false;
            this.txtFromDate.IsShowCurrentDate = false;
            this.txtFromDate.Location = new System.Drawing.Point(627, 6);
            this.txtFromDate.MaxLength = 10;
            this.txtFromDate.MiladiDT = ((object)(resources.GetObject("txtFromDate.MiladiDT")));
            this.txtFromDate.Name = "txtFromDate";
            this.txtFromDate.ReadOnly = true;
            this.txtFromDate.ReadOnlyMaskedEdit = false;
            this.txtFromDate.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtFromDate.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(190)))), ((int)(((byte)(160)))));
            this.txtFromDate.ShamsiDT = "____/__/__";
            this.txtFromDate.Size = new System.Drawing.Size(72, 20);
            this.txtFromDate.TabIndex = 1;
            this.txtFromDate.Text = "____/__/__";
            this.txtFromDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtFromDate.TimeMaskedEditorOBJ = null;
            // 
            // txtToTime
            // 
            this.txtToTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtToTime.BackColor = System.Drawing.SystemColors.Window;
            this.txtToTime.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtToTime.IsShadow = false;
            this.txtToTime.IsShowCurrentTime = false;
            this.txtToTime.Location = new System.Drawing.Point(499, 29);
            this.txtToTime.MaxLength = 5;
            this.txtToTime.MiladiDT = null;
            this.txtToTime.Name = "txtToTime";
            this.txtToTime.ReadOnly = true;
            this.txtToTime.ReadOnlyMaskedEdit = false;
            this.txtToTime.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtToTime.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(190)))), ((int)(((byte)(160)))));
            this.txtToTime.Size = new System.Drawing.Size(40, 20);
            this.txtToTime.TabIndex = 6;
            this.txtToTime.Text = "__:__";
            this.txtToTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtToDate
            // 
            this.txtToDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtToDate.BackColor = System.Drawing.SystemColors.Window;
            this.txtToDate.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtToDate.IntegerDate = 0;
            this.txtToDate.IsShadow = false;
            this.txtToDate.IsShowCurrentDate = false;
            this.txtToDate.Location = new System.Drawing.Point(627, 30);
            this.txtToDate.MaxLength = 10;
            this.txtToDate.MiladiDT = ((object)(resources.GetObject("txtToDate.MiladiDT")));
            this.txtToDate.Name = "txtToDate";
            this.txtToDate.ReadOnly = true;
            this.txtToDate.ReadOnlyMaskedEdit = false;
            this.txtToDate.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtToDate.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(190)))), ((int)(((byte)(160)))));
            this.txtToDate.ShamsiDT = "____/__/__";
            this.txtToDate.Size = new System.Drawing.Size(72, 20);
            this.txtToDate.TabIndex = 3;
            this.txtToDate.Text = "____/__/__";
            this.txtToDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtToDate.TimeMaskedEditorOBJ = null;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(545, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 18);
            this.label3.TabIndex = 13;
            this.label3.Text = "و ساعت";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(705, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 18);
            this.label2.TabIndex = 11;
            this.label2.Text = "تا تاريخ";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(705, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 19);
            this.label1.TabIndex = 10;
            this.label1.Text = "از تاريخ";
            // 
            // frmTraceHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(797, 458);
            this.Controls.Add(this.pnlSearchExpand);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.MapElementHost);
            this.HelpMaker.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.Topic);
            this.Name = "frmTraceHistory";
            this.HelpMaker.SetShowHelp(this, true);
            this.Text = "تاريخچه رديابي";
            this.pnlSearchExpand.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgRequest)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgMaster)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgTablet)).EndInit();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost MapElementHost;
        internal System.Windows.Forms.Button btnCancel;
        private DevComponents.DotNetBar.ExpandablePanel pnlSearchExpand;
        internal Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor txtToDate;
        internal Bargh_Common.mdlPersianMaskedEditor.PersianMaskedEditor txtFromDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel pnlSearch;
        internal Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor txtFromTime;
        internal Bargh_Common.mdlPersianMaskedEditor.TimeMaskedEditor txtToTime;
        internal Bargh_Common.ComboBoxPersian cboArea;
        private System.Windows.Forms.Label label5;
        internal Bargh_Common.DateButton btnToDate;
        internal Bargh_Common.DateButton btnFromDate;
        internal System.Windows.Forms.Button btnSearch;
        internal Bargh_Common.JGrid dgArea;
        internal Bargh_Common.JGrid dgMaster;
        internal Bargh_Common.JGrid dgTablet;
        internal Bargh_Common.JGrid dgRequest;
        internal System.Windows.Forms.Button btnShow;
        internal System.Windows.Forms.Button btnRequests;
        internal System.Windows.Forms.Button btnCloseFilter;
        private System.Windows.Forms.Panel pnlButtons;
    }
}