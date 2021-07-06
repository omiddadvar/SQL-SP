using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Bargh_Common;
using System.Drawing;
using Janus.Windows.GridEX;
using Janus.Windows.Common;

namespace Bargh_GIS
{
    public partial class frmTraceHistory : FormBase
    {
        private Classes.CDatabase db;
        private static int[] heights = new int[3] { 95 , 235 , 360};
        private Size GetSize(int i)
        {
            return new Size(this.Size.Width - 20, heights[i]);
        }
        private DataSet mDS;
        private DataTable mBufferDT;
        private long mRequestId;
        private SqlConnection mCnn = null;
        ComboBox FakeComboBox = new ComboBox();
        public frmTraceHistory(long aRequestId)
        {
            InitializeComponent();
            this.mRequestId = aRequestId;
            initialData();
            handleUI03();
        }
        private void initialData() {
            pnlSearchExpand.Size = GetSize(0);
            this.mCnn = new SqlConnection(CommonFunctions.GetConnection());
            this.mDS = new DataSet();
            this.db = new Classes.CDatabase();
            string lSQL = "SELECT * FROM Tbl_Area";
            mBufferDT = db.ExecSQL(lSQL);
            mDS.Tables.Add(mBufferDT.Copy());
            Bargh_Common.CommonFunctions.BindingTable(lSQL, ref mCnn, ref mDS, "Tbl_Area", ref FakeComboBox);
            cboArea.DataSource = mDS.Tables["Tbl_Area"];
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            handleUI01();
        }

        private void btnRequests_Click(object sender, EventArgs e)
        {
            handleUI02();
        }
        private void btnCloseFilter_Click(object sender, EventArgs e)
        {
            handleUI03();
        }
        private void handleUI01() {
            pnlSearchExpand.Size = GetSize(1);
            dgTablet.Visible = dgArea.Visible = dgMaster.Visible = true;
            pnlButtons.Visible = btnCloseFilter.Visible = btnRequests.Visible = btnShow.Visible = true;
            pnlButtons.Top = dgMaster.Bottom + 5;
            dgRequest.Visible = false;
        }
        private void handleUI02()
        {
            pnlSearchExpand.Size = GetSize(2);
            pnlButtons.Top = dgRequest.Bottom + 5;
            dgRequest.Visible = true;
        }
        private void handleUI03() {
            pnlSearchExpand.Size = GetSize(0);
            dgTablet.Visible = dgArea.Visible = dgMaster.Visible = false;
            pnlButtons.Visible = btnCloseFilter.Visible = btnRequests.Visible = btnShow.Visible = false;
            dgRequest.Visible = false;
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            pnlSearchExpand.Expanded = false;
        }
    }
}
